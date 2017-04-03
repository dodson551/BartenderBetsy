Imports System.Text

Public Module modProcessWebRequest

  Public Function URLDecode(ByVal StringToDecode As String) As String

    Dim TempAns As String = ""
    Dim CurChr As Integer

    CurChr = 1

    Do Until CurChr - 1 = Len(StringToDecode)
      Select Case Mid(StringToDecode, CurChr, 1)
        Case "+"
          TempAns = TempAns & " "
        Case "%"
          TempAns = TempAns & Chr(Val("&h" & _
             Mid(StringToDecode, CurChr + 1, 2)))
          CurChr = CurChr + 2
        Case Else
          TempAns = TempAns & Mid(StringToDecode, CurChr, 1)
      End Select

      CurChr = CurChr + 1
    Loop

    URLDecode = TempAns
  End Function

  Public Function ProcessWebRequest(ByRef hd As HttpData) As Boolean
    ' Generic Variables
    Dim sErr As String = "", bRet As Boolean = False
    Dim iValidEventsCount As Integer = 0
    Dim sOverrideFileName As String = ""
    Dim obTempZipFile As Ionic.Zip.ZipFile = New Ionic.Zip.ZipFile()
    Dim bEntrySaved As Boolean = True
    Dim sbResp As New StringBuilder
    Dim sResp As String = ""
    Dim bLiveRequest As Boolean = False
    Dim sURLFile As String = URLDecode(hd.Request_Filename)
    Dim sVersion As String = My.Application.Info.ProductName & " " &
                             System.String.Format("Version {0}.{1:00}.{2:00}.{3:00}",
                             My.Application.Info.Version.Major,
                             My.Application.Info.Version.Minor,
                             My.Application.Info.Version.Build,
                             My.Application.Info.Version.Revision)

#If TELAERIS_SERVICE Then
    WriteLog("Process Web Request Called." & hd.Request_BodyAscii, EventLogEntryType.Information)
#End If

    Try

#If TELAERIS_SERVICE Then
      WriteLog("In TryDbConnection... " & vbCrLf &
             "Connection String-" & g_sConn & vbCrLf &
             "Type- " & g_iConnType)
#End If

      If Not TryDBConnection(g_sConn, sErr) Then
        sbResp.Append("<!doctype html><html><head></head><body>")
        sbResp.Append("Database connect failed")
        sbResp.AppendLine("<br><font color=black>@cs</font><br />").Replace("@cs", g_sConn)
        sbResp.AppendLine("<br><font color=black>@version</font><br />").Replace("@version", sVersion)
        sbResp.Append("</body></html>")
      Else ' Got the database, let try something else...
        ' We'll do different things if we are a "GET" or "POST"
        If (hd.Request_Method = "POST") Then 'ALL TagTrakker Update messages are posts
          sbResp = New StringBuilder
          sResp = ProcessPost(hd)
          Debug.WriteLine(hd.Response_BodyAscii)
          sbResp.Append(sResp)
        Else ' "GET"
          If sURLFile.ToLower = "/tagtrakker" Or sURLFile = "/" Then
            sbResp.Append("Server alive : " & Format(Now, DATETIME_FORMAT_SQLITE_NO_FRACTIONS) & "<br>" & vbCrLf)
            sbResp.Append(HTML_HEAD)
            sbResp.AppendLine("<p>Valid URLs</p>")
            sbResp.AppendLine("<ul>")
            'sbResp.Append("<li>Installation:<a href=""/TagTrakker/Install"">Install</a></li>")
            sbResp.Append("<li>Item Search: <a href=""/TagTrakker/search"">Search</a></li>")
            sbResp.AppendLine("</ul>")
            sbResp.AppendLine("<br><font color=black>@version</font><br />").Replace("@version", sVersion)
            sbResp.AppendLine("<br><font color=black>@cs</font><br />").Replace("@cs", g_sConn)
            sbResp.AppendLine("<br><font color=black>DatabaseName=@db_name</font><br />").Replace("@db_name", g_Settings.DatabaseName)

            sbResp.Append(HTML_END)
          ElseIf InStr(sURLFile.ToLower, "/TagTrakker/Install", CompareMethod.Text) > 0 Then  ' Serve the handheld file
            sbResp.Append(HTML_HEAD)
            sbResp.Append(Now.ToLongTimeString & " : File Not Currently Available failed")
            sbResp.Append(HTML_END)

          ElseIf InStr(sURLFile.ToLower, "/TagTrakker/Config", CompareMethod.Text) > 0 Then  ' Serve the handheld file
            sbResp.Append(HTML_HEAD)
            sbResp.Append("<p>")
            sbResp.Append("Connection String:" & g_sConn & "<br/>")
            sbResp.Append("Connection Type:" & g_iConnType & "<br/>")
            sbResp.Append("License Data:" & g_LicenseData & "<br/>")
            sbResp.Append("</p>")
            sbResp.Append(HTML_END)
            '''''''''''Lists for Event Data''''''''''''''''''''''''''''''
          ElseIf InStr(sURLFile.ToLower, "/TagTrakker/Lists", CompareMethod.Text) > 0 Then
            TagTrakkerBuildLists(sURLFile, obTempZipFile)
            If (obTempZipFile IsNot Nothing) Then
              hd.Response_ContentType = "application/zip"
            Else
              sbResp.AppendLine(HTML_HEAD & "Error During Building Sync List" & HTML_END)
            End If
            '''''''''''Lists for Event Data''''''''''''''''''''''''''''''
          ElseIf InStr(sURLFile.ToLower, "/TagTrakker/GetTable", CompareMethod.Text) > 0 Then
            GetTableAsZip(sURLFile, obTempZipFile)
            If (obTempZipFile IsNot Nothing) Then
              hd.Response_ContentType = "application/zip"
            Else
              sbResp.AppendLine(HTML_HEAD & "Error During Building Sync List" & HTML_END)
            End If

            '''''''''''Live mode functions''''''''''''''''''''''''''''''
          ElseIf InStr(sURLFile.ToLower, "/live", CompareMethod.Text) Then
            bLiveRequest = True
            WebRequestLiveHandlers(hd)

            '''''''''''Audits summary page''''''''''''''''''''''''''''''
          ElseIf InStr(sURLFile.ToLower, "/tagtrakker/audits", CompareMethod.Text) > 0 Then
            Dim sAuditID As String = ParseURL(sURLFile, "audit_id")
            Dim sUserID As String = ParseURL(sURLFile, "user_id")
            Dim sGUID As String = ParseURL(sURLFile, "guid")
            'user_id=" & g_iLoggedInAdminUserID & "&guid=" & g_sLoggedInAdminHash)
            If (sAuditID = "") Then
              If (ValidateGUID("users", sUserID, sGUID, sbResp) = False) Then
                GoTo SendResponse
              End If
              Dim drAudits As DataRowCollection = GetDataRowsBySQL("SELECT * FROM Audits ORDER BY created_at DESC")
              sbResp.Append(HTML_HEAD)
              If (drAudits.Count = 0) Then
                sbResp.Append("No Email Audits in database.")

              Else
                'Summarize all the outstanding audits
                sbResp.Append("<table><tr><th>Time</th><th>Admin</th><th>Status</th><th>Link</th></tr>")
                For Each dr As DataRow In drAudits

                  sbResp.Append("<tr>")
                  sbResp.Append("<td>" & dr("timestamp") & "</td>")
                  sbResp.Append("<td>" & dr("audit_admin_email") & "</td>")
                  sbResp.Append("<td>" & dr("status") & "</td>")
                  sbResp.Append("<td><a href=""" & g_Settings.GlobalURL & "/TagTrakker/audits?audit_id=" & dr("id") & "&guid=" & dr("guid") & """>Link</a></td>")

                  sbResp.Append("</tr>")
                Next
                sbResp.Append("</table>")

              End If
              sbResp.Append(HTML_END)
            Else

              If (ValidateGUID("audits", sAuditID, sGUID, sbResp) = False) Then
                GoTo SendResponse
              End If
              Dim sSQL As String = SQL_AUDIT_SUMMARY() & " WHERE audits.id = " & sAuditID & " ORDER BY last_name ASC"
              'Get the actual results for the specified audit
              Dim dtRes As DataSet = GetDataSetBySQL(sSQL)
              sbResp.Append(BuildXSLTString(dtRes, g_Settings.XSLTAuditSummary, sErr))
            End If

            '''''''''''Audit compliance page''''''''''''''''''''''''''''''
          ElseIf InStr(sURLFile.ToLower, "/tagtrakker/resendauditemail", CompareMethod.Text) > 0 Then
            Dim sAuditEmail As String = ParseURL(sURLFile, "audit_email_id")
            Dim sGUID As String = ParseURL(sURLFile, "guid")
            If (ValidateGUID("audit_emails", sAuditEmail, sGUID, sbResp) = False) Then
              GoTo SendResponse
            End If
            g_cAuditManager.ResendAuditEmail(CInt(sAuditEmail))
            sbResp.Append(HTML_HEAD)
            sbResp.Append("Thank you.  The Audit Email has been resent. <br>")
            sbResp.Append(HTML_END)
            '''''''''''Audit compliance page''''''''''''''''''''''''''''''
          ElseIf InStr(sURLFile.ToLower, "/tagtrakker/auditcompliance", CompareMethod.Text) > 0 Then
            Dim sAuditEmail As String = ParseURL(sURLFile, "audit_email_id")
            Dim sSQL As String = SQL_AUDIT_USER_ITEMS() & " WHERE audit_email_id = " & sAuditEmail
            Dim sAction As String = ParseURL(sURLFile, "action")
            Dim sAuditItem As String = ParseURL(sURLFile, "audit_item_id")
            Dim sGUID As String = ParseURL(sURLFile, "guid")

            If (ValidateGUID("audit_emails", sAuditEmail, sGUID, sbResp) = False) Then
              GoTo SendResponse
            End If

            'Update the items first, then get the results
            'We're overloading this route quite a bit

            Dim bStatusChange As Boolean = False
            If (sAction = "confirm_all") Then
              bStatusChange = True
              ExecuteNonQuery("UPDATE audit_items set status='" & AUDIT_ITEM_STATUS_CONFIRMED & "', updated_at=NOW() where status = 'unconfirmed' AND audit_email_id=" & sAuditEmail)
            End If
            If (bStatusChange = True) Then
              g_cAuditManager.CheckAuditIsDone(CInt(sAuditEmail))
            End If

            Dim dtRes As DataSet = GetDataSetBySQL(sSQL)

            sbResp.Append(BuildXSLTString(dtRes, g_Settings.XSLTAuditCompliance, sErr))


            '''''''''''Transfer Item page''''''''''''''''''''''''''''''
          ElseIf InStr(sURLFile.ToLower, "/tagtrakker/transferitem", CompareMethod.Text) > 0 Then
            Dim sAuditItem As String = ParseURL(sURLFile, "audit_item_id")
            Dim sGUID As String = ParseURL(sURLFile, "guid")

            If (ValidateGUID("audit_items", sAuditItem, sGUID, sbResp) = False) Then
              GoTo SendResponse
            End If


            Dim sSQL As String = SQL_AUDIT_USER_ITEMS() & " WHERE audit_items.id = " & sAuditItem

            'Update the items first, then get the results
            Dim dtRes As DataSet = GetDataSetBySQL(sSQL)

            'dtRes.Tables[0] is the Item Table
            'We also want to add a table for all the users so we can populate the Combo box
            Dim dt As DataTable = GetDataTableBySQL("SELECT id, (first_name || ' ' || last_name) as full_name FROM USERS where deleted_at is null ORDER BY last_name asc")
            dt.TableName = "User"
            dtRes.Tables.Add(dt)

            sbResp.Append(BuildXSLTString(dtRes, g_Settings.XSLTTransferItem, sErr))

            '''''''''''COnfirm/Reject item transfer''''''''''''''''''''''''''''''
            '''''''''''Item Search Result''''''''''''''''''''''''''''''
          ElseIf InStr(sURLFile.ToLower, "/tagtrakker/search_result", CompareMethod.Text) > 0 Then

            Dim sSQL As String = SQL_ITEMS_WITH_EXTRA_INFO(g_Settings.UseSerialNumberInsteadOfTagNumber)
            Dim sStockNo As String = ParseURL(sURLFile, "stock_no")
            Dim sPartial As String = ParseURL(sURLFile, "partial")
            'Filter using item id or stock number
            Dim bPartial As Boolean = False
            If (sPartial = "true") OrElse (sPartial = "on") Then
              bPartial = True
            End If

            Dim sFields As New List(Of String)

            sFields.Add("ci.description")
            sFields.Add("j.name")
            sFields.Add("u.first_name")
            sFields.Add("u.last_name")
            sFields.Add("c.name")
            sFields.Add("manufacturer.name")
            sFields.Add("l.description")
            sFields.Add("category")
            sFields.Add("serial_number")
            sFields.Add("stock_no")
            sFields.Add("alternate_stock_no")
            If (bPartial) Then
              sSQL &= " AND (i.stock_no = '" & sStockNo & "' OR " & CreateSqlWhereLike(sFields, sStockNo, False) & ")"
            Else
              sFields.Add("i.stock_no")
              sSQL &= " AND " & CreateSqlWhereEquals(sFields, sStockNo, False)
            End If


            Dim dtRes As DataSet = GetDataSetBySQL(sSQL)

            If (dtRes Is Nothing) OrElse dtRes.Tables.Count <= 0 OrElse dtRes.Tables(0).Rows.Count <= 0 Then
              sbResp.Append(HTML_HEAD)
              sbResp.Append("<p>Not Found</p>")
              sbResp.Append("<form id=""codeform"" method=""get"" action=""/TagTrakker/search_result"">")
              sbResp.Append("<label for=""stock_no"">Search: </label>")
              sbResp.Append("<input type=""text"" id=""stock_no"" value="""" name=""stock_no"">")
              sbResp.Append("<br/><label for=""parital_match"">Partial Match:</label>")
              sbResp.Append("<input type=""checkbox"" id=""partial"" checked=""yes""  name=""partial"">")
              sbResp.Append("<input type=""submit"" value=""Submit"" style=""margin-bottom:5px;font-family:verdana;"">")

              sbResp.Append("</form>")

            Else

              'Add the result
              sbResp.Append(BuildXSLTString(dtRes, g_Settings.XSLTItemMobile, sErr))

            End If
            '''''''''''Item Search''''''''''''''''''''''''''''''
          ElseIf InStr(sURLFile.ToLower, "/tagtrakker/search", CompareMethod.Text) > 0 Then
            sbResp.Append(HTML_HEAD)
            sbResp.Append("<form id=""codeform"" method=""get"" action=""/TagTrakker/search_result"">")

            sbResp.Append("<label for=""stock_no"">Search: </label>")
            sbResp.Append("<input type=""text"" id=""stock_no"" value="""" name=""stock_no"">")
            sbResp.Append("<br/><label for=""parital_match"">Partial Match: </label>")
            sbResp.Append("<input type=""checkbox"" id=""partial""  checked=""yes""   name=""partial"">")
            sbResp.Append("<input type=""submit"" value=""Submit"" style=""margin-bottom:5px;font-family:verdana;"">")

            sbResp.Append("</form>")
            ' sbResp.Append("<br/>Installation:<a href=""/TagTrakker/Install"">Install</a>")
            sbResp.Append("<br/>Item Search:<a href=""/TagTrakker/search"">Search</a>")

            sbResp.Append(HTML_END)


          Else '''''''''''FAILURE'''''''''''''''''''''''''''''''
            sbResp.Append(HTML_HEAD)
            sbResp.Append("Failure: TagTrakker does not support this URL" & vbCrLf & sURLFile)
            'sbResp.Append("<br/>Installation:<a href=""/TagTrakker/Install"">Install</a>")
            sbResp.Append("<br/>Item Search:<a href=""/TagTrakker/search"">Search</a>")

            sbResp.Append(HTML_END)
            hd.Response_Text = "Error"
          End If
        End If
      End If
SendResponse:

      Debug.WriteLine(hd.Response_BodyAscii)
      If Not bLiveRequest Then
        hd.Response_Body = ASCIIEncoding.ASCII.GetBytes(sbResp.ToString)
        Debug.Print(sbResp.ToString)
        If (hd.Response_ContentType = "application/zip") Then
          ' TODO: can we stream this file directly?  or at least put into temporary dir?
          Dim sTempFileName As String = System.IO.Path.GetTempPath & "TempZipFile_" & hd.Request_ID & ".zip"
          obTempZipFile.Save(sTempFileName)
          obTempZipFile.Dispose()
          obTempZipFile = Nothing
          hd.Response_Body = File.ReadAllBytes(sTempFileName)
          File.Delete(sTempFileName)
        ElseIf (hd.Response_ContentType = "application/exe") Or (hd.Response_ContentType = "application/x-cab-compressed") Then
          hd.Response_Body = File.ReadAllBytes(sbResp.ToString)

        Else 'If (HttpConn.Response_ContentType.Contains("text")) Or 
          Dim e As New System.Text.UTF8Encoding
          hd.Response_Body = e.GetBytes(sbResp.ToString)
          hd.Response_ContentLength = sbResp.Length
        End If
      End If

      bRet = True
    Catch ex As Exception
      sErr = DateTime.Now.ToLongTimeString & vbCrLf &
            "TagTrakker Server Error Parsing URL" & vbCrLf &
            ("<br><font color=black>@version</font><br />").Replace("@version", sVersion) & vbCrLf &
            ex.Message & vbCrLf & sbResp.ToString

      RaiseEvent ServerError(Mid(sErr, 1, 400))
      bRet = False
    Finally
      'GC.Collect()
    End Try
    Return bRet

  End Function

End Module
