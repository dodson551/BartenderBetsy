Imports System.Data
Imports System.Data.Common
Imports System.Data.Odbc
Imports System.Data.OleDb
Imports System.Data.SQLite
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Forms
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports Microsoft.Win32
Imports System.Text.RegularExpressions
Imports System.ComponentModel

Public Module modSQLiteControl

  Public g_sConnString As String = ""

  Dim sConnString As New SQLiteConnection With {.ConnectionString = g_sConnString}
  Public Function Hasconnection() As Boolean
    Try
      sConnString.Open()
      sConnString.Close()
      Return True

    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
    Return False
  End Function


  ''' <summary>
  ''' This function is really important. You will use this a lot when grabbing things from your database based on certain parameters you wish to use.
  ''' Just don't accidentally name your list of parameters lstParams2...
  ''' Trust me, it is a huge headache, you are not ready for it...
  ''' </summary>
  ''' <param name="query"></param>
  ''' <param name="sParams"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function paramQuery(query As String, sParams As List(Of String))
    Dim sConnString As New SQLiteConnection With {.ConnectionString = g_sConnString}
    Dim sqlitecmd As New SQLiteCommand
    Dim sqlitedt As New DataTable
    Try
      sConnString.Open()
      sqlitecmd = New SQLiteCommand(query, sConnString)
      If sParams IsNot Nothing Then
        Dim i As Integer = 1
        For Each p As String In sParams
          sqlitecmd.Parameters.AddWithValue("@pname" & i.ToString, p)
          i += 1
        Next
      End If
      'FILL DATASET
      Dim sqliteda As New SQLiteDataAdapter(sqlitecmd)

      sqliteda.Fill(sqlitedt)

    Catch ex As Exception
      MsgBox("query failed: " & ex.Message)
      MsgBox(query)
    Finally
      sqlitecmd.Dispose()
      sqlitecmd = Nothing
      sConnString.Close()
      sConnString.Dispose()
      sConnString = Nothing
    End Try
    Return sqlitedt
  End Function

  ''' <summary>
  ''' This function is not quite as widely used as the paramquery function but it is still used a lot.
  ''' You will use this anytime you want to insert anything into the database.
  ''' </summary>
  ''' <param name="query"></param>
  ''' <param name="sParams"></param>
  ''' <param name="sErr"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function insertQuery(query As String, sParams As List(Of String), Optional ByRef sErr As String = "")
    Dim sConnString As New SQLiteConnection With {.ConnectionString = g_sConnString}
    Dim i As Integer = 1
    Dim sqlitecmd As SQLiteCommand
    Try
      sConnString.Open()
      sqlitecmd = New SQLiteCommand(query, sConnString)

      For Each p As String In sParams
        sqlitecmd.Parameters.AddWithValue("@pname" & i.ToString, p)
        i += 1
      Next
      sErr = ""
      Try
        i = sqlitecmd.ExecuteNonQuery()
        sErr = "SQL Success:  " & i & "items changed"

      Catch ex As Exception
        sErr = "ExecuteNonQuery FAILURE: " & query.Replace(vbCrLf, " ") & vbCrLf & ex.Message
        i = -1
      Finally
        sqlitecmd.Dispose()
        sqlitecmd = Nothing
        sConnString.Close()
        sConnString.Dispose()
        sConnString = Nothing
      End Try
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
    Return i
  End Function

  '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'
  'These functions define new database connection items that you will want to use
  'newdbconnection is obvious
  'newdbcommand is the SQL you will use to do something in the DB
  'newdbdataadapter is something I'm not 100% clear on, ask Alex
  'newdbparameter gives you a new parameter you can use in your paramQuery

  Public Function NewDBConnection(ByVal sConnectionString As String) As DbConnection
    Dim conn As New SQLiteConnection(g_sConnString)
    Return conn
  End Function

  Public Function NewDbCommand(ByVal sSQL As String, ByVal sConnString As DbConnection) As DbCommand
    Return New SQLite.SQLiteCommand(sSQL, sConnString)
  End Function

  Public Function NewDbDataAdapter(ByVal sSQL As String, ByVal conn As DbConnection) As DbDataAdapter
    If (TypeOf conn Is SQLiteConnection) Then
      Return New SQLiteDataAdapter(sSQL, conn)
    End If
    Return New SQLiteDataAdapter(sSQL, conn)
  End Function

  Public Function NewDbParameter(ByVal sName As String)
    Dim p As DbParameter
    p = New SQLiteParameter(sName)
    Return p
  End Function

  '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'

  Public Sub addnewParam(Name As String, Value As Object)
    Dim sqliteParams As New List(Of SQLiteParameter)
    Dim newParam As New SQLiteParameter With {.ParameterName = Name, .Value = Value}
    sqliteParams.Add(newParam)

  End Sub

  ''' <summary>
  ''' Not used all that much, this is just a call to do a query from the database without any parameter specification.
  ''' Something you will use if you want to grab all of some table without limiting what information you are trying to pull from it.
  ''' </summary>
  ''' <param name="sSQL"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function runQuery(ByVal sSQL As String)
    Dim sConnString As String = g_sConnString
    Dim dt As DataTable = Nothing
    Dim ds As New DataSet

    Dim con As New SQLiteConnection(sConnString)
    con.Open()
    Try
      Using cmd As New SQLiteCommand(sSQL, con)

        Using da As New SQLiteDataAdapter(cmd)
          da.Fill(ds)
          dt = ds.Tables(0)
        End Using
        cmd.Dispose()
      End Using

    Catch ex As Exception

    Finally
      con.Close()
      con.Dispose()

    End Try
    Return dt
  End Function

  ''' <summary>
  ''' Never really used to its full potential, probably pretty useful for grabbing pictures from a DB.
  ''' </summary>
  ''' <param name="Query"></param>
  ''' <param name="sParams"></param>
  ''' <param name="filename"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function pictureQuery(Query As String, sParams As Byte, filename As String)
    Dim sConnString As New SQLiteConnection With {.ConnectionString = g_sConnString}
    Dim i As Integer = 1
    Dim sqlitecmd As SQLiteCommand

    sConnString.Open()
    sqlitecmd = New SQLiteCommand(Query, sConnString)

    Using picture As Image = Image.FromFile(filename)
      Using Stream As New IO.MemoryStream
        picture.Save(Stream, Imaging.ImageFormat.Jpeg)

      End Using
    End Using
    Return i
  End Function

  ''' <summary>
  ''' Use this function to take all of the information from a datatable and use it to fill a listview.
  ''' Super handy if you want to do one DB call to get all the information you need from your DB.
  ''' </summary>
  ''' <param name="sSQL"></param>
  ''' <param name="lstView"></param>
  ''' <remarks></remarks>
  Public Sub Fill_ListView(ByRef sSQL As String, ByRef lstView As ListView)
    Dim SQLconnect As New SQLite.SQLiteConnection()
    SQLconnect.ConnectionString = g_sConnString
    SQLconnect.Open()
    Dim SQLcommand As New SQLiteCommand(sSQL, SQLconnect)
    Dim da As New SQLiteDataAdapter(SQLcommand)
    Dim ds As New DataSet
    da.Fill(ds)
    Dim i As Integer = 0
    Dim j As Integer = 0
    Dim itemcol(50) As String
    For i = 0 To ds.Tables(0).Rows.Count - 1
      For j = 0 To ds.Tables(0).Columns.Count - 1
        itemcol(j) = ds.Tables(0).Rows(i)(j).ToString()
      Next
      Dim lstitm As New ListViewItem(itemcol)
      lstView.Items.Add(lstitm)
    Next
  End Sub

  ''' <summary>
  ''' Much like the sub above, very useful for filling comboboxes, listboxes, and other things with your DB information.
  ''' </summary>
  ''' <param name="cbo"></param>
  ''' <param name="sSQL"></param>
  ''' <param name="id_name"></param>
  ''' <param name="display_text_name"></param>
  ''' <param name="sFirstItem"></param>
  ''' <param name="sSecondItem"></param>
  ''' <param name="sItemToSelect"></param>
  ''' <param name="bForceSelection"></param>
  ''' <param name="bIsListIdField"></param>
  ''' <remarks></remarks>
  Public Sub FillComboBoxFromDB(ByRef cbo As Object, ByVal sSQL As String, _
                              ByVal id_name As String, _
                              ByVal display_text_name As String, _
                              Optional ByVal sFirstItem As String = "DO_NOT_INCLUDE", _
                              Optional ByVal sSecondItem As String = "DO_NOT_INCLUDE", _
                              Optional ByVal sItemToSelect As String = "", _
                              Optional ByVal bForceSelection As Boolean = False,
                              Optional ByVal bIsListIdField As Boolean = False)


    Dim rdr As SQLiteDataReader = Nothing
    Dim cmd As DbCommand = Nothing
    Dim tc() As Char = {" ", "{", "}", "(", ")", "[", "]", "?"}
    Dim sConnString As New SQLiteConnection With {.ConnectionString = g_sConnString}

    cbo.Items.Clear()

    Try
      If sFirstItem <> "DO_NOT_INCLUDE" Then
        cbo.Items.Add(New ItemWithID(sFirstItem, -1))
      End If
      If sSecondItem <> "DO_NOT_INCLUDE" Then
        cbo.Items.Add(New ItemWithID(sSecondItem, -1))
      End If

      sConnString.Open()
      cmd = NewDbCommand(sSQL, sConnString)
      rdr = cmd.ExecuteReader()

      If rdr.HasRows Then
        While rdr.Read
          If (rdr.Item(display_text_name).ToString.Trim(tc) <> "") Then
            If bIsListIdField Then
              cbo.Items.Add(New ListItemWithID(rdr.Item(display_text_name).ToString, rdr.Item(id_name)))
            Else
              cbo.Items.add(New ItemWithID(rdr.Item(display_text_name).ToString, rdr.Item(id_name)))
            End If
          End If

        End While

        rdr.Close()
      End If

      If sItemToSelect <> "" Then
        cbo.Text = sItemToSelect
        Dim iTempIndex As Integer = -1
        For iTempIndex = 0 To cbo.items.count - 1
          If (cbo.items(iTempIndex).ToString = sItemToSelect) Then
            Exit For
          End If

        Next
        If (iTempIndex <> -1) And iTempIndex < cbo.Items.Count Then
          cbo.SelectedIndex = iTempIndex
        End If

      Else
        If (bForceSelection And cbo.Items.Count > 0) Then cbo.SelectedIndex = 0
      End If
    Catch e As Exception

    Finally
      CloseCmdAndConn(cmd, sConnString)
    End Try
  End Sub

  ''' <summary>
  ''' Haven't really used this function anywhere, would be used to directly insert a table from a database into a dataset.
  ''' </summary>
  ''' <param name="sSQL"></param>
  ''' <param name="ds"></param>
  ''' <param name="sDataTableName"></param>
  ''' <param name="sConn"></param>
  ''' <param name="bAddIdPrimaryKeyConstraint"></param>
  ''' <param name="sErr"></param>
  ''' <param name="bUpdatingDtBooleanFlag"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function DBQueryToDataSet(ByVal sSQL As String, ByRef ds As DataSet,
                                   ByVal sDataTableName As String, ByVal sConn As String, _
                                      Optional ByVal bAddIdPrimaryKeyConstraint As Boolean = True, _
                                      Optional ByRef sErr As String = "", _
                                      Optional ByRef bUpdatingDtBooleanFlag As Boolean = True) As Boolean
    Dim conn As DbConnection = Nothing
    Dim cmd As DbCommand = Nothing
    Dim adptr As DbDataAdapter = Nothing
    Dim bRet As Boolean = False
    bUpdatingDtBooleanFlag = True
    Try
      'open db connection
      conn = NewDBConnection(g_sConnString)
      conn.Open()
      adptr = NewDbDataAdapter(sSQL, conn)
      cmd = NewDbCommand(sSQL, conn)
      adptr.SelectCommand = cmd
      If Not (ds Is Nothing) Then ds.Dispose()
      adptr.Fill(ds, sDataTableName)
      bRet = True
    Catch ex As Exception
      sErr = "Error filling dataset from query: " & ex.Message.ToString & vbCrLf & "Query: " & sSQL
    Finally
      CloseCmdAndConn(cmd, conn)
      bUpdatingDtBooleanFlag = False
    End Try
    Return bRet
  End Function

  Public Function NullToString(ByVal vArg As Object, Optional ByVal sDefault As String = "") As String
    Dim sRet As String = sDefault
    If vArg Is System.DBNull.Value OrElse vArg Is Nothing Then
      sRet = sDefault
    Else
      Try
        sRet = CStr(vArg)
      Catch ex As Exception
        'sRet is already set to sDefault at top
      End Try
    End If
    Return sRet
  End Function

  ''' <summary>
  ''' This sub will close both a SQLiteCommand and a SQLiteConnection.
  ''' It will also dispose of both of these as well.
  ''' </summary>
  ''' <param name="cmd"></param>
  ''' <param name="conn"></param>
  ''' <remarks></remarks>
  Public Sub CloseCmdAndConn(ByRef cmd As DbCommand, ByRef conn As DbConnection)
    If Not conn Is Nothing AndAlso conn.State = System.Data.ConnectionState.Open Then
      conn.Close()
    End If
    If Not conn Is Nothing Then
      conn.Dispose()
      conn = Nothing
    End If
    If Not cmd Is Nothing Then
      cmd.Dispose()
      cmd = Nothing
    End If
  End Sub
End Module

Public Class ListItemWithID
  Inherits ListViewItem
  Public Value As String = ""
  Public ID As String = ""
  Public Sub New()
    Value = ""
    ID = -1
  End Sub
  Public Sub New(ByVal sValue As String, ByVal sID As String)
    Value = sValue
    ID = sID
  End Sub

  Public Overrides Function ToString() As String
    Return Value
  End Function
End Class

'~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'
'Class for defining an ItemWithID, very useful in this application
'~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'

Public Class ItemWithID
  'Private m_sValue As String = ""
  Public Name As String
  Public ID As String = ""

  Public Sub New()
    Name = ""
    ID = -1
  End Sub
  Public Sub New(ByVal sValue As String, ByVal sID As String)
    Name = sValue
    ID = sID
  End Sub

  Public Overrides Function ToString() As String
    Return Name
  End Function

  Public Shared Operator =(ByVal item1 As ItemWithID, ByVal item2 As ItemWithID) As Boolean
    Return item1.ID = item2.ID
  End Operator

  Public Shared Operator <>(ByVal item1 As ItemWithID, ByVal item2 As ItemWithID) As Boolean
    Return Not (item1 = item2)
  End Operator

  Public Shared Function GetListIdIndex(iID As ItemWithID, lst As System.Windows.Forms.ListBox) As Integer
    Dim iRet As Integer = -1, iListID As ItemWithID
    For i As Integer = 0 To lst.Items.Count - 1
      iListID = lst.Items(i)
      If iID.ID = iListID.ID Then
        iRet = i
        Exit For
      End If
    Next
    Return iRet
  End Function

  Public Shared Function GetListEntryIndex(iID As ItemWithID, lst As System.Windows.Forms.ListBox) As Integer
    Dim iRet As Integer = -1, iListID As ItemWithID
    For i As Integer = 0 To lst.Items.Count - 1
      iListID = lst.Items(i)
      If (iID.ID = iListID.ID) And (iID.Name = iListID.Name) Then
        iRet = i
        Exit For
      End If
    Next
    Return iRet
  End Function

  Public Shared Function GetComboIDValue(ByVal cbo As System.Windows.Forms.ComboBox, Optional ByVal oDefault As Object = Nothing) As Object
    If (cbo.SelectedIndex >= 0) Then
      Return cbo.Items(cbo.SelectedIndex).ID
    Else
      If oDefault Is Nothing Then
        Return DBNull.Value
      Else
        Return oDefault
      End If
    End If
  End Function
  Public Shared Sub SetComboEntry(ByRef cbo As System.Windows.Forms.ComboBox, ByVal iItemWithID As Integer, Optional bIgnoreINVALID As Boolean = False)
    Dim iIndex As Integer = -1
    Try
      If bIgnoreINVALID Or iItemWithID <> -1 Then
        Dim iListID As ItemWithID
        For i As Integer = 0 To cbo.Items.Count - 1
          iListID = cbo.Items(i)
          If (iItemWithID = iListID.ID) Then
            iIndex = i
            Exit For
          End If
        Next
      End If
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
    Try
      'cbo.Select() ' needed so object can be accessed
      cbo.SelectedIndex = iIndex
    Catch ex As Exception
    End Try
  End Sub

  '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'

End Class
