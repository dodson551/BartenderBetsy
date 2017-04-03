Imports LitJson
Imports System.Data
Imports System.Data.Common
Imports System.Data.SQLite
Imports System.IO
Imports System.Net

Module modLitJSONHelper
  Public Class ItemWithData
    'Private m_sValue As String = ""
    Public Name As String
    Public data As DataRow
    Public Sub New(ByVal sValue As String, ByVal objDataRow As DataRow)
      Name = sValue
      data = objDataRow
    End Sub
    Public Overrides Function ToString() As String
      Return Name
    End Function

    Public Shared Operator =(ByVal item1 As ItemWithData, ByVal item2 As ItemWithData) As Boolean
      Return item1.data.Item("id") = item2.data.Item("id")
    End Operator
    Public Shared Operator <>(ByVal item1 As ItemWithData, ByVal item2 As ItemWithData) As Boolean
      Return Not (item1 = item2)
    End Operator
    Public Function DisplayString(ByVal bIncludeSN As Boolean)
      Dim sRet As String = ""
      sRet &= data.Item("description") & vbCrLf
      If bIncludeSN Then
        sRet &= "SN: " & data.Item("SN") & vbCrLf
      End If
      sRet &= "Tag: " & data.Item("TN") & vbCrLf

      Return sRet

    End Function
  End Class
  Public Class ItemWithIDAndRequired
    Inherits ItemWithData
    Public Required As Boolean
    Public Sub New(ByVal sValue As String, ByVal objDataRow As DataRow, ByVal bRequired As Boolean)
      MyBase.New(sValue, objDataRow)
      Required = bRequired
    End Sub
    Public Overrides Function ToString() As String
      Return MyBase.ToString & IIf(Required, " - Required", " - Not Required")
    End Function
  End Class

  'Builds a JSON Object from fields and values
  'Returns JsonData object
  Public Function BuildJSONRecordFromFieldsAndValues(ByVal sObjectName As String, _
                                                     ByVal asFields As List(Of String), _
                                                     ByVal asValues As List(Of String)) As LitJson.JsonData
    Dim data As New LitJson.JsonWriter()
    'If you pass in an empty name, then don't add a property name to the object
    If (sObjectName <> "") Then
      data.WritePropertyName(sObjectName)
    End If
    data.WriteObjectStart()
    For i As Integer = 0 To asFields.Count - 1
      data.WritePropertyName(asFields(i))
      data.Write(asValues(i))
    Next
    data.WriteObjectEnd()
    Return JsonMapper.ToObject(data.ToString)
  End Function

  '===============================================================================
  '    Name: FillDataTableFromFile(ByVal sFileName As String, _
  '                                ByRef dtThisDataTable As DataTable) As FileInfo
  ' Remarks: Fill DataTable(dtThisDataTable) from file(sFileName)
  '           Assume that there is a records object in the file.
  '===============================================================================
  Public Function FillDataTableFromJSONFile(ByVal sFileName As String, _
                                        ByRef dtThisDataTable As DataTable) As FileInfo

    Dim reader As StreamReader = Nothing
    Dim FileProps As FileInfo = Nothing
    Try
      FileProps = New FileInfo(sFileName)
      'Exit if the file doesn't exist
      If (Not File.Exists(sFileName)) Then Return FileProps
      reader = New StreamReader(sFileName)

      Dim jsonData As JsonData = JsonMapper.ToObject(reader)
      FillDataTableFromJsonData(dtThisDataTable, jsonData)




    Catch ex As Exception
      dtThisDataTable = Nothing
    Finally
      If Not reader Is Nothing Then reader.Close()

    End Try

    Return FileProps
  End Function

  '===============================================================================
  '    Name: GetAppPath() As String
  ' Remarks: Method for getting application path differs depending on what platform
  '          is being used
  '===============================================================================
  Public Function App_Path() As String
    If (InStr(System.Environment.OSVersion.ToString, "Windows NT", CompareMethod.Text) > 0) Then
      'GetPathName = My.Application.Info.DirectoryPath & "\".Replace("\\", "\") & sFileName
      App_Path = System.IO.Path.GetDirectoryName( _
                   System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).ToString & "\".Replace("\\", "\")
      If (InStr(App_Path, "file:\", CompareMethod.Text) = 1) Then
        App_Path = Mid(App_Path, 7)
      End If
    Else
      App_Path = System.IO.Path.GetDirectoryName( _
                   System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).ToString & "\".Replace("\\", "\")
    End If
    Dim tc() As Char = {" ", vbCr, vbLf, vbTab, vbNullChar}
    Return App_Path.Trim(tc)
  End Function

  ''===============================================================================
  ''    Name: HttpGet(ByVal sURL As String, _
  ''                  Optional ByVal iTimeout As Integer = -1) As String
  '' Remarks: 
  ''===============================================================================
  'Public Function HttpGet(ByVal sURL As String, _
  '                        Optional ByVal iTimeout As Integer = -1, _
  '                        Optional ByVal sCookie As String = "") As Object
  '  Dim Req As HttpWebRequest
  '  Dim SourceStream As System.IO.Stream
  '  Dim Response As HttpWebResponse

  '  Try
  '    'create a web request to the URL  
  '    Req = HttpWebRequest.Create(sURL)
  '    If (sCookie <> "") Then
  '      Req.Headers.Set("Cookie", sCookie)
  '    End If
  '    Req.Method = "GET"
  '    If iTimeout <> -1 Then Req.Timeout = iTimeout

  '    Req.Accept = "application/json"
  '    'Req.ContentType = "application/json"
  '    'get a response from web site  
  '    Response = Req.GetResponse()

  '    'Source stream with requested document  
  '    SourceStream = Response.GetResponseStream()

  '    'SourceStream has no ReadAll, so we must read data block-by-block  
  '    'Temporary Buffer and block size  
  '    Dim Buffer(4096) As Byte, BlockSize As Integer

  '    'Memory stream to store data  
  '    Dim TempStream As New MemoryStream
  '    Do
  '      BlockSize = SourceStream.Read(Buffer, 0, 4096)
  '      If BlockSize > 0 Then TempStream.Write(Buffer, 0, BlockSize)
  '    Loop While BlockSize > 0

  '    '		Lighttpd response for ContentType: "application/json; charset=utf-8"	
  '    If (InStr(Response.ContentType, "application/json") > 0) Then
  '      'return the document as s JsonData Object
  '      Dim jsonData As JsonData = JsonMapper.ToObject(System.Text.Encoding.ASCII.GetString(TempStream.ToArray, 0, TempStream.Length))
  '      HttpGet = jsonData
  '    Else
  '      'return the document as a string
  '      HttpGet = System.Text.Encoding.ASCII.GetString(TempStream.ToArray, 0, TempStream.Length)
  '    End If


  '    ' Close Streams
  '    SourceStream.Close()
  '    Response.Close()
  '  Catch ex As Exception
  '    HttpGet = "HttpGet Error" & vbCrLf & sURL & vbCrLf & ex.Message
  '  End Try
  'End Function

  Public Function FillDataTableFromJsonData(ByRef dtThisDataTable As DataTable, ByVal data As JsonData) As DataTable
    ' This example shows how to use the high-resolution counter to 
    ' time an operation. 
    ' Get counter value before the operation starts.


    If (data Is Nothing) Then
      Return Nothing
    Else
      dtThisDataTable = New DataTable
    End If

    Dim primaryKeyColumns(0 To 0) As DataColumn
    'Get the keys from the first row
    If (data("records").Count <= 0) Then
      Return Nothing
    End If
    Dim dict As System.Collections.IDictionary = data("records").Item(0)
    Dim jsonDataTypes As JsonData = data("records").Item(0)
    dtThisDataTable.BeginLoadData()
    For Each sColName As String In dict.Keys
      Dim item As JsonData = jsonDataTypes(sColName)
      If (item Is Nothing) Then
        dtThisDataTable.Columns.Add(sColName)
      ElseIf (item.IsBoolean) Then
        dtThisDataTable.Columns.Add(sColName, GetType(Boolean))
      ElseIf (item.IsDouble) Then
        dtThisDataTable.Columns.Add(sColName, GetType(Double))
      ElseIf (item.IsInt) Then
        dtThisDataTable.Columns.Add(sColName, GetType(Integer))
      ElseIf (item.IsLong) Then
        dtThisDataTable.Columns.Add(sColName, GetType(Long))
      ElseIf (item.IsString) Then
        dtThisDataTable.Columns.Add(sColName, GetType(String))
      Else
        'Assume it's  an object?!
        dtThisDataTable.Columns.Add(sColName)
      End If
      If (sColName = "id") Then
        primaryKeyColumns(0) = dtThisDataTable.Columns("id")
        dtThisDataTable.PrimaryKey = primaryKeyColumns
      End If

    Next


    For Each jsonRow As JsonData In data.Item("records")
      Dim values(0 To dtThisDataTable.Columns.Count - 1) As Object
      'Dim value_dict As System.Collections.IDictionary = jsonRow
      Array.Clear(values, 0, values.Length)
      Dim i As Integer = 0
      For Each col As DataColumn In dtThisDataTable.Columns
        Dim item As JsonData
        Try
          item = jsonRow.Item(col.ColumnName)
        Catch ex As System.Collections.Generic.KeyNotFoundException
          item = Nothing
        End Try
        If (item Is Nothing) Then
          values(i) = Nothing
        ElseIf (item.IsBoolean) Then
          values(i) = CBool(item.ToString)
        ElseIf (item.IsDouble) Then
          values(i) = CDbl(item)
        ElseIf (item.IsInt) Then
          values(i) = CInt(item)
        ElseIf (item.IsLong) Then
          values(i) = CLng(item)
        ElseIf (item.IsString) Then
          values(i) = item.ToString
        Else
          'Assume it's  an object?!
          values(i) = item
        End If
        i += 1
      Next

      dtThisDataTable.Rows.Add(values)
    Next
    dtThisDataTable.EndLoadData()

    Return dtThisDataTable
  End Function

  'Public Function BuildNewItemJSONRecord(ByVal sObjectName As String, _
  '                                       ByVal dr As DataRow, _
  '                                       Optional ByVal asExcludeFields() As String = Nothing)

  'End Function

  'Public Function BuildNewItemActivityJSONRecord(ByVal sObjectName As String, _
  '                                               ByVal dr As DataRow, _
  '                                               Optional ByVal asExcludeFields() As String = Nothing)
  '  Dim data As New LitJson.JsonWriter()
  '  'If you pass in an empty name, then don't add a property name to the object


  '  If (sObjectName <> "") Then
  '    data.WriteObjectStart()
  '    data.WritePropertyName(sObjectName)
  '  End If

  '  data.WriteObjectStart()
  '  For Each clm As DataColumn In dr.Table.Columns
  '    Dim sColumnName As String = clm.ColumnName

  '    'Skip 'exclude' fields
  '    If (asExcludeFields IsNot Nothing) Then
  '      Dim j As Integer = 0
  '      While j < asExcludeFields.Length
  '        Dim sExcludeField As String = asExcludeFields(j)
  '        If (sColumnName = sExcludeField) Then
  '          'Skip this column
  '          Continue For
  '        End If
  '        j += 1
  '      End While
  '    End If

  '    'Get the column name from the parent table
  '    data.WritePropertyName(sColumnName)

  '    'Get the value from the field
  '    Dim val As String = dr.Item(sColumnName).ToString
  '    If (val Is DBNull.Value) OrElse val Is Nothing Then
  '      Dim emptyString As String = Nothing
  '      data.Write(emptyString)
  '    Else
  '      data.Write(val)
  '    End If

  '  Next
  '  If (sObjectName <> "") Then
  '    data.WriteObjectEnd()
  '  End If
  '  data.WriteObjectEnd()
  '  Return JsonMapper.ToObject(data.ToString)
  'End Function

  Public Structure REFERENCING_FIELD
    Dim sReferencedTable As String
    Dim sReferencingTable As String
    Dim sReferencedField As String
    Dim sReferencingField As String
    Dim sJsonFieldName As String
  End Structure

  'Builds a JSON Object from a DataRow.  Includes all referencing records as well. 
  'Returns JsonData object.  liRecordIDs and lsRecordTableNames will be filled with the id and table name of
  '  each record inserted into the json object
  Public Function BuildJSONRecordFromDataRow(ByVal sObjectName As String, _
                                             ByVal dr As DataRow, ByVal sTableName As String, _
                                             Optional ByVal lsExcludeFields As List(Of String) = Nothing, _
                                             Optional ByVal lsReferencingFields As List(Of REFERENCING_FIELD) = Nothing, _
                                             Optional ByRef liRecordIDs As List(Of Integer) = Nothing, _
                                             Optional ByRef lsRecordTableNames As List(Of String) = Nothing, _
                                             Optional ByRef htPackaged As Hashtable = Nothing) As LitJson.JsonData
    Dim data As New LitJson.JsonWriter()
    'If you pass in an empty name, then don't add a property name to the object

    If (sObjectName <> "") Then
      data.WriteObjectStart()
      data.WritePropertyName(sObjectName)
    End If

    data.WriteObjectStart()

    For Each clm As DataColumn In dr.Table.Columns

      'Get the column name and value from the field
      Dim sColumnName As String = clm.ColumnName

      Dim value As String = dr.Item(sColumnName).ToString
      If (TypeOf (dr.Item(sColumnName)) Is Boolean) Then value = value.ToLower()

      'Store the id and table name of this record
      Select Case sColumnName
        Case "id"
          If liRecordIDs IsNot Nothing Then
            liRecordIDs.Add(CInt(Val(value)))
            lsRecordTableNames.Add(sTableName)
          End If
      End Select

      'Skip this column if it appears in our list of excluded fields (lsExcludeFields)
      Dim bSkipThisColumn As Boolean = False
      If Not lsExcludeFields Is Nothing Then
        For Each field As String In lsExcludeFields
          If (sColumnName = field) Then
            bSkipThisColumn = True
          End If
        Next
      End If

      If bSkipThisColumn = False Then
        'Get the column name from the parent table
        data.WritePropertyName(sColumnName)

        If (value Is DBNull.Value) OrElse value Is Nothing Then
          Dim emptyString As String = Nothing
          data.Write(emptyString)
        Else
          data.Write(value)
        End If
      End If

      'Search through our list of referencing fields looking for any references to the current field (sColumnName)
      'If Not lsReferencingFields Is Nothing Then
      '  For Each ref As REFERENCING_FIELD In lsReferencingFields
      '    If (sColumnName = ref.sReferencedField) AndAlso (sTableName = ref.sReferencedTable) Then
      '
      '      'Get all referencing records from table (sReferencingTable)
      '      Dim drRows As DataRowCollection = GetDataRows(ref.sReferencingTable, ref.sReferencingField, value)
      '
      '      If ((Not drRows Is Nothing) AndAlso (drRows.Count > 0)) Then
      '        Dim lstPackaged As List(Of Integer)
      '        If (Not htPackaged.ContainsKey(ref.sReferencingTable)) Then
      '          lstPackaged = New List(Of Integer)
      '          'Add the new list
      '          htPackaged.Add(ref.sReferencingTable, lstPackaged)
      '        Else
      '          lstPackaged = htPackaged(ref.sReferencingTable)
      '        End If
      '
      '
      '        'Records exist. Write property name and start array
      '        data.WritePropertyName(ref.sJsonFieldName)
      '        data.WriteArrayStart()
      '
      '        'Loop through and add each record to the JSON object
      '        For k As Integer = 0 To drRows.Count - 1
      '          Dim dr_ref As DataRow = drRows(k)
      '          'Add this item to our "already packaged" list 
      '          lstPackaged.Add(dr_ref("id"))
      '          'Recursivly add rows to our JSON object
      '          Debug.WriteLine("Adding Child Row for " & sTableName & "(" & dr_ref("id") & ") of type " & ref.sReferencedTable)
      '          Dim jsonRow As JsonData = BuildJSONRecordFromDataRow("", dr_ref, ref.sReferencingTable, _
      '                                                               lsExcludeFields, lsReferencingFields, liRecordIDs, lsRecordTableNames, htPackaged)
      '          data.Put(jsonRow.ToJson)
      '
      '          'Add seperater if this isn't the last row
      '          If (k < (drRows.Count - 1)) Then
      '            data.Put(",")
      '          End If
      '        Next
      '
      '        'Not sure this is needed, let's remove it for now.
      '        ' htPackaged(ref.sReferencingTable) = lstPackaged
      '        data.WriteArrayEnd() 'End array
      '      End If
      '    End If
      '  Next
      'End If

    Next
    If (sObjectName <> "") Then
      data.WriteObjectEnd()
    End If
    data.WriteObjectEnd()

#If DEBUG Then
    If (sTableName = "items") Then
      Debug.WriteLine("Added Row for " & sTableName & " with Stock#: '" & dr("stock_no") & "' AND with id: " & dr("id"))
    ElseIf (sTableName = "inspections") Then
      Debug.WriteLine("Added Row for " & sTableName & " with Stock#: '" & dr("stock_no") & "' AND with item_id: " & dr("item_id") & " AND with id: " & dr("id"))
    Else
      Debug.WriteLine("Added Row for " & sTableName & " with id: " & dr("id"))
    End If

#End If
    Return JsonMapper.ToObject(data.ToString)
  End Function
#If WindowsCE <> True And PocketPC <> True Then


  ' Public Sub FillDataGridViewBySQL(ByVal sSQL As String, ByRef dgr As DataGridView)
  '   Dim dtAct As New DataTable
  '   dgr.DataSource = Nothing
  '   SyncLock g_SQLConnection
  '     Try
  '       Dim da As Data.Common.DbDataAdapter = NewDbDataAdapter(sSQL, g_SQLConnection)
  '       da.Fill(dtAct)
  '       'Fill datagrid from datatable
  '       If Not (dtAct Is Nothing) Then dgr.DataSource = dtAct
  '     Catch ex As Exception
  '       '        Console.WriteLine("FillActivityListBox: " & ex.Message)
  '       dgr.DataSource = Nothing
  '     Finally
  '
  '       Cursor.Current = Cursors.Default
  '     End Try
  '   End SyncLock
  ' End Sub
#End If
End Module

