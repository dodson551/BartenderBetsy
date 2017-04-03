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
Imports System.Collections
Imports System.Data.SqlClient


#If MYSQL_NATIVE Then
Imports MySql.Data.MySqlClient
#End If

Public Module modDatabase

  Private asDateTimeFields() As String = New String() {"activation_date", "arrival_date", "booted_at", "checked_in_at", "created_at", "current_zone_timestamp", "last_seen_timestamp", "date_end", "date_start", "DateCreated", "DateSent", "DateUpdated", "deleted_at", "door_last_updated_at", "end_date", "end_time", "ends_at", "expiration_date", "file_timestamp", "last_run_at", "logged_in_at", "logged_out_at", "reader_last_active_at", "remember_token_expires_at", "start_date", "start_time", "starts_at", "timestamp", "update_interval", "update_time", "updated_at", "zone_updated_at", "last_synced_at"}
  Public alDateTimeFields As New ArrayList(asDateTimeFields)

  Private asPictureFields() As String = New String() {"picture", "thumbnail", "tiny", "id_image", "fp1", "fp2", "header_image", "image", "template", "raw_data", "logo_file"}
  Public alPictureFields As New ArrayList(asPictureFields)

  Public Const DATE_FORMAT_SQLITE = "yyyy-MM-dd HH:mm:ss.FFFFFFF"
  Public Const TIME_FORMAT_SQLITE = "HH:mm:ss.FFFFFFF"
  Public Const DAY_FORMAT_SQLITE = "yyyy-MM-dd"
  Public Const DATETIME_FORMAT_SQLITE_NO_FRACTIONS = "yyyy-MM-dd HH:mm:ss"
  Public Const DATE_FORMAT_SQLITE_MS = "yyyy-MM-dd HH:mm:ss.fff" 'lowercase f forces output, like sqlite produces

  'Database Globals
  Public g_sDatabasePassword As String 'Not a setting. But a Local Variable
  Public g_htTableCreateStatements As New Collections.Hashtable
  Public g_htTableVersions As New Collections.Hashtable
  Public g_lstTablesIndexes As New List(Of DBIndexInfo)
  Public g_bConnectionValid As Boolean = False
  Public g_bDBHashUpToDate As Boolean
  'Connection Type (Done so no inadvertent writing of globals)
  Dim m_iConnType As Integer = INVALID_ID

  Public Class DBIndexInfo
    Public Name As String
    Public Table As String
    Public Fields As String
    Public Sub New(sName As String, sTable As String, sFields As String)
      Name = sName
      Table = sTable
      Fields = sFields
    End Sub
  End Class
  Public Sub DBIndexAddStandard(sName As String, sTable As String, sFields As String)

    If String.IsNullOrEmpty(sName) Then
      sName = sTable & "_" & sFields.Replace(",", "_")
    End If
    If Not DBIndexExistsInList(sName, sTable) Then
      g_lstTablesIndexes.Add(New DBIndexInfo(sName, sTable, sFields))
    End If
  End Sub
  Public Function DBIndexExistsInList(sName As String, sTable As String) As Boolean
    For Each dbi As DBIndexInfo In g_lstTablesIndexes
      If (dbi.Table = sTable) And (dbi.Name = sName) Then Return True
    Next
    Return False
  End Function
  Public Function CreateAllIndexex(conn As DbConnection, trans As DbTransaction, ByRef sErr As String) As Boolean
    Dim bRet As Boolean = True
    For Each dbi As DBIndexInfo In g_lstTablesIndexes
      If (RunCreateIndex(dbi, conn, trans, sErr) = False) Then
        Return False
      End If
    Next
    Return bRet
  End Function

  Public Function RunCreateIndex(dbi As DBIndexInfo, conn As DbConnection, trans As DbTransaction, ByRef sErr As String) As Boolean
    'Parse the DBIndexExists and create the index for the g_iConnType
    Dim sSQL As String = ""
    If (g_iConnType = DB_TYPES.SQLSERVER) Then
      sSQL = "IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = '" & dbi.Name & "' )" & vbCrLf & _
      "BEGIN " & vbCrLf & _
      "  CREATE INDEX " & dbi.Name & " ON [dbo].[" & dbi.Table & "](" & dbi.Fields & ");" & vbCrLf & _
      "END"
    Else
      '"CREATE INDEX IF NOT EXISTS users_zones_zone_id ON users_zones(zone_id)"
      sSQL = "CREATE INDEX IF NOT EXISTS " & dbi.Name & " ON " & dbi.Table & "(" & dbi.Fields & ")"
    End If
    Dim bRet As Boolean = False
    Try
      If (ExecuteNonQuery(sSQL, sErr, conn, trans) < 0) Then
        bRet = False
      Else
        bRet = True
      End If

    Catch ex As Exception
      sErr = GetExceptionText(ex)
      bRet = False
    End Try
    Return bRet

  End Function

  Public g_sPathData As String
  Public ReadOnly Property g_iConnType As DB_TYPES
    Get
      Return m_iConnType
    End Get
  End Property


  'Connection String (Done so no inadvertent writing of globals)
  Dim m_sConnString As String = ""
  Public ReadOnly Property g_sConn As String
    Get
      Return m_sConnString
    End Get
  End Property
  Public Sub SetGlobalConnString(sConn As String, iConnType As DB_TYPES)
    m_sConnString = sConn
    m_iConnType = iConnType
    My.Settings.ConnectionString = sConn
    My.Settings.ConnectionType = iConnType
    My.Settings.Save()
  End Sub
  ' License Data (Done so no inadvertent writing of globals)
  Dim m_sLicenseData As String = ""
  Public ReadOnly Property g_LicenseData As String
    Get
      Return m_sLicenseData
    End Get
  End Property
  Public Sub SetGlobalLicenseData(ByVal value As String)
    m_sLicenseData = value
    'Write the Value to the Registry
    If (m_sLicenseData IsNot Nothing) Then
      My.Settings.LicenseData = value
      My.Settings.Save()
    End If
  End Sub

  Public Function GetSQLiteDatabaseLocation(sDatabaseConnectionString As String) As String
    Dim tc As Char() = {" ", """", ";"}
    Return sDatabaseConnectionString.Replace("Data Source=", "").Trim(tc)
  End Function


#Region "Global DB Settings"
  Public Sub GetGlobalConnectionSettingsAndDataDir()
    GetAndCreateAppDataPath()
    m_sConnString = My.Settings.ConnectionString
    m_iConnType = My.Settings.ConnectionType

  End Sub


#End Region


  Public Enum DB_TYPES
    UNSPECIFIED = -1
    SQLITE = 0
    MYSQL = 1
    OLEDB = 2
    ODBC = 3
    SQLSERVER = 4
  End Enum

  Private _getFieldDataBySQL As String
  Private Property GetFieldDataBySQL(ByVal p1 As String, ByVal p2 As String) As String
    Get
      Return _getFieldDataBySQL
    End Get
    Set(ByVal value As String)
      _getFieldDataBySQL = value
    End Set
  End Property

  Public Function DBTypeName(ByVal iConnType As DB_TYPES) As String
    Dim sType As String
    Select Case iConnType
      Case DB_TYPES.MYSQL
        sType = "MySQL"
      Case DB_TYPES.SQLITE
        sType = "SQLite"
      Case DB_TYPES.OLEDB
        sType = "MS Access"
      Case DB_TYPES.ODBC
        sType = "ODBC"
      Case DB_TYPES.SQLSERVER
        sType = "SQL SERVER"
      Case Else
        sType = "Unknown"
    End Select
    Return sType
  End Function

  Public Function DBTypeFromName(sDBTypeName As String) As Integer
    Dim iConnType As DB_TYPES
    Select Case iConnType
      Case "MySQL", CStr(DB_TYPES.MYSQL)
        iConnType = DB_TYPES.MYSQL
      Case "SQLite", CStr(DB_TYPES.SQLITE)
        iConnType = DB_TYPES.SQLITE
      Case "MS Access", "MSAccess", "Access", CStr(DB_TYPES.OLEDB)
        iConnType = DB_TYPES.OLEDB
      Case "ODBC", CStr(DB_TYPES.ODBC)
        iConnType = DB_TYPES.ODBC
      Case "SQL SERVER", "SQLSERVER", "MSSQLSERVER", CStr(DB_TYPES.SQLSERVER)
        iConnType = DB_TYPES.SQLSERVER
      Case "Unknown", CStr(DB_TYPES.UNSPECIFIED)
        iConnType = DB_TYPES.UNSPECIFIED
    End Select
    Return iConnType
  End Function

  Public Function NewDbParameter(ByVal sName As String, ByVal oType As DbType, Optional ByVal oValue As Object = Nothing) As DbParameter
    Dim p As DbParameter

    If (g_iConnType = DB_TYPES.SQLITE) Then
      p = New SQLiteParameter(sName, oType)
#If MYSQL_NATIVE Then
    ElseIf (g_iConnType = DB_TYPES.MYSQL) Then
      p = New MySqlParameter(sName, TypeConvertor.ToMySQLDbType(oType))
#End If

    ElseIf (g_iConnType = DB_TYPES.ODBC) Then
      p = New OdbcParameter(sName, oType)
    ElseIf (g_iConnType = DB_TYPES.OLEDB) Then
      p = New OleDbParameter(sName, oType)
    ElseIf (g_iConnType = DB_TYPES.SQLSERVER) Then
      p = New SqlClient.SqlParameter(sName, TypeConvertor.ToSqlServerFromDbType(oType))
    Else
      p = New SQLiteParameter(sName, oType)
    End If
    If (oValue IsNot Nothing) Then
      p.Value = oValue
    End If
    Return p
  End Function


  Private Function ReplaceSQLServerSpecificConstructs(sSqliteQuery As String) As String
    '"(?=((?<=^|\s)SELECT (?<AFTERSELECT>.+? )LIMIT (?<LIMIT>\d{1,4})(?=\s|$)|(?<=\()SELECT (?<AFTERSELECT>.+? )LIMIT (?<LIMIT>\d{1,4})(?=\))))"

    Debug.WriteLine("Incoming Query:" & vbCrLf & sSqliteQuery)
    Dim sQ As String = sSqliteQuery.ToLower()

    'REPLACE THE shortcut format of LIMIT (LIMIT x,y) WITH full format (OFFSET x, LIMIT y)
    sSqliteQuery = Regex.Replace(sSqliteQuery, "\blimit\s+(?<OFFSET>\d{1,6})\s*,\s*(?<LIMIT>\d{1,6})\b", " OFFSET ${OFFSET} LIMIT ${LIMIT} ", RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    sQ = sSqliteQuery.ToLower()

    'TODO: THIS NEEDS MORE WORK. SQL SERVER 2008 DOES NOT HAVE AN EASY WAY OF DOING PAGINATION
    If sQ.Contains("offset") Then
      sSqliteQuery = Regex.Replace(sSqliteQuery, "\bOFFSET\s+\d{1,4}\s*", "", RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
      sQ = sSqliteQuery.ToLower()
    End If

    'LIMIT keyword is equal to SQL Server's TOP keyword and is written immediately after SELECT
    If sQ.Contains("limit") Then
      'Replace LIMIT with TOP for subqueries
      sSqliteQuery = Regex.Replace(sSqliteQuery, "SELECT (?<AFTERSELECT>[^()]+ FROM [^()]+) LIMIT\s+(?<LIMIT>\d{1,4})",
                                   "SELECT TOP ${LIMIT} ${AFTERSELECT}", RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)

      sQ = sSqliteQuery.ToLower()

      'Now replace it for the main query
      If Regex.IsMatch(sQ, "limit\s+\d{1,4}\s*$") AndAlso (Not Regex.IsMatch(sQ, "^select\s+top\s+\d{1,4}")) Then
        sSqliteQuery = Regex.Replace(sSqliteQuery, "SELECT\s+(?<AFTERSELECT>.+)\s+LIMIT\s+(?<LIMIT>\d{1,4})\s*$", _
                                     "SELECT TOP ${LIMIT} ${AFTERSELECT}", RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase).Replace(sSqliteQuery, "").Replace(sSqliteQuery, "")

        sQ = sSqliteQuery.ToLower()
      End If
    End If

    'TRIM() function is not there in SQL Server. We replace it with LTRIM(RTRIM())
    If (Not sQ.Contains("dbo.trim(")) AndAlso sQ.Contains("trim") Then
      sSqliteQuery = Regex.Replace(sSqliteQuery, "\bTRIM\s*\((?<TEXT>.+?)\)", "LTRIM(RTRIM(${TEXT}))", RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
      sQ = sSqliteQuery.ToLower()
    End If

    'BLOB is IMAGE in SQL Server
    If sQ.Contains("blob") Then
      sSqliteQuery = Regex.Replace(sSqliteQuery, "\bAS\s+BLOB", "AS IMAGE", RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
      sQ = sSqliteQuery.ToLower()
    End If

    'User is a keyword in T-SQL and should be bracketted
    If sQ.Contains("user") Then
      sSqliteQuery = Regex.Replace(sSqliteQuery, "\bAS\s+User,", " AS [User],", RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
      sQ = sSqliteQuery.ToLower()
    End If

    'datetime('now') is equal to GETDATE() in SQL Server
    If sQ.Contains("localtime") Then
      sSqliteQuery = Regex.Replace(sSqliteQuery, "\bdatetime\s*\(\s*'now'\)", "GETDATE()", RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
      sQ = sSqliteQuery.ToLower()
    End If

    If (sSqliteQuery.Contains("NOW()")) Then
      sSqliteQuery = SQL_NOW(sSqliteQuery)
    End If

    If (sSqliteQuery.Contains("||")) Then
      sSqliteQuery = sSqliteQuery.Replace("||", "+")
    End If

    'It's ISNULL in SQL Server
    If (sSqliteQuery.Contains("IFNULL(")) Then
      sSqliteQuery = sSqliteQuery.Replace("IFNULL(", "ISNULL(")
    End If

    'ORDER BY in a sub-query is not permitted by SQL Server (plz note that it doesn't make sense to use ORDER BY in a sub-query in any RDBMS, but
    'SQLite allows it)
    If sQ.Contains("order by") Then
      'sSqliteQuery = Regex.Replace(sSqliteQuery, "\sORDER\s+BY [^)]+\)(?!\s*$)", ")", RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
      'sQ = sSqliteQuery.ToLower()
    End If

    If sQ.Contains("strftime") Then
      sSqliteQuery = Regex.Replace(sSqliteQuery, "strftime\(\s*'%m/%d/%Y - %H:%M'\s*,\s*(?<EXP>\w+(\.\w+)?)\)", _
                                   "CONVERT(varchar, ${EXP}, 101) + ' - ' + CONVERT(varchar, DATEPART(HH, ${EXP})) + ':' + CONVERT(varchar, DATEPART(N, ${EXP}))", _
                                   RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)

      sSqliteQuery = Regex.Replace(sSqliteQuery, "strftime\(\s*'%m/%d/%Y'\s*,\s*(?<EXP>\w+(\.\w+)?)\)", _
                                   "CONVERT(varchar, ${EXP}, 101)", _
                                   RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
      sQ = sSqliteQuery.ToLower()
    End If

    If sQ.Contains("datetime") Then
      sSqliteQuery = Regex.Replace(sSqliteQuery, "\bdatetime\s*\(\s*(?<EXP>\w+(\s*\.\s*\w+)?)\s*,\s*'?\w+'?\)", _
                                   "CONVERT(varchar, ${EXP}, 120)", _
                                   RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
      sQ = sSqliteQuery.ToLower()
    End If

    Debug.WriteLine("Converted Query: " & vbCrLf & sSqliteQuery)

    Return sSqliteQuery
  End Function


  Public Function NewDbDataAdapter(ByVal sSQL As String, ByVal conn As DbConnection) As DbDataAdapter
#If SQL_TRACE Then
    ConnectionTrace(conn, sSQL)
#End If
    If (TypeOf conn Is SQLiteConnection) Then
      Return New SQLiteDataAdapter(sSQL, conn)
#If MYSQL_NATIVE Then
    ElseIf (TypeOf conn Is MySqlConnection) Then
      Return New MySqlDataAdapter(sSQL, conn)
#End If
    ElseIf (TypeOf conn Is OdbcConnection) Then
      Return New OdbcDataAdapter(sSQL, conn)
    ElseIf (TypeOf conn Is OleDbConnection) Then
      Return New OleDbDataAdapter(sSQL, conn)
    ElseIf (TypeOf conn Is SqlClient.SqlConnection) Then
      Return New SqlClient.SqlDataAdapter(ReplaceSQLServerSpecificConstructs(sSQL), conn)
    Else
      Return New SQLiteDataAdapter(sSQL, conn)
    End If
  End Function

  Public Function NewDbDataAdapter(ByVal cmd As DbCommand) As DbDataAdapter
    If (TypeOf cmd Is SQLiteCommand) Then
      Return New SQLiteDataAdapter(cmd)
#If MYSQL_NATIVE Then
    ElseIf (TypeOf cmd Is MySqlCommand) Then
      Return New MySqlDataAdapter(cmd)
#End If
    ElseIf (TypeOf cmd Is OdbcCommand) Then
      Return New OdbcDataAdapter(cmd)
    ElseIf (TypeOf cmd Is OleDbCommand) Then
      Return New OleDbDataAdapter(cmd)
    ElseIf (TypeOf cmd Is SqlClient.SqlCommand) Then
      cmd.CommandText = ReplaceSQLServerSpecificConstructs(cmd.CommandText)
      Return New SqlClient.SqlDataAdapter(cmd)
    Else
      Return New SQLiteDataAdapter(cmd)
    End If
  End Function

  Public Function NewDbCommand(ByVal sSQL As String, ByVal conn As DbConnection) As DbCommand
    sSQL = SQL_NOW(sSQL)
#If SQL_TRACE Then
    ConnectionTrace(conn, sSQL)
#End If
    If (TypeOf conn Is SQLiteConnection) Then
      Return New SQLiteCommand(sSQL, conn)
#If MYSQL_NATIVE Then
    ElseIf (TypeOf conn Is MySqlConnection) Then
      Return New MySqlCommand(sSQL, conn)
#End If
    ElseIf (TypeOf conn Is OdbcConnection) Then
      Return New OdbcCommand(sSQL, conn)
    ElseIf (TypeOf conn Is OleDbConnection) Then
      Return New OleDbCommand(sSQL, conn)
    ElseIf (TypeOf conn Is SqlClient.SqlConnection) Then
      Return New SqlClient.SqlCommand(ReplaceSQLServerSpecificConstructs(sSQL), conn)
    Else
      Return Nothing
    End If
  End Function

  Public Function NewDbCommand(ByVal sSQL As String, ByVal conn As DbConnection, ByVal trans As DbTransaction) As DbCommand
#If SQL_TRACE Then
    ConnectionTrace(conn, sSQL)
#End If
    If (TypeOf conn Is SQLiteConnection) Then
      Return New SQLiteCommand(sSQL, conn, trans)
#If MYSQL_NATIVE Then
    ElseIf (TypeOf conn Is MySqlConnection) Then
      Return New MySqlCommand(sSQL, conn, trans)
#End If
    ElseIf (TypeOf conn Is OdbcConnection) Then
      Return New OdbcCommand(sSQL, conn, trans)
    ElseIf (TypeOf conn Is OleDbConnection) Then
      Return New OleDbCommand(sSQL, conn, trans)
    ElseIf (TypeOf conn Is SqlClient.SqlConnection) Then
      Return New SqlClient.SqlCommand(ReplaceSQLServerSpecificConstructs(sSQL), conn, trans)
    Else
      Return Nothing
    End If
  End Function



  Public Sub AddNewDBParameter(ByRef cmd As DbCommand, ByVal sFieldName As String, ByVal oData As Object)
    'Anonymous parameters don't have names
    Dim bAnonymous As Boolean = False
    If (sFieldName = "") Then
      bAnonymous = True
    End If
    If (TypeOf cmd Is SQLiteCommand) Then
      If Not bAnonymous Then
        cmd.Parameters.Add(NewDbParameter(sFieldName, TypeConvertor.ToDbType(oData.GetType)))
        cmd.Parameters(sFieldName).Value = oData
      Else
        cmd.Parameters.Add(New SQLiteParameter(TypeConvertor.ToDbType(oData.GetType), oData))
      End If
#If MYSQL_NATIVE Then
    ElseIf (TypeOf cmd Is MySqlCommand) Then
      cmd.Parameters.Add(New MySqlParameter(sFieldName, oData))
#End If
    ElseIf (TypeOf cmd Is OleDbCommand) Then
      'OleDB doesn't allow anonymous params
      cmd.Parameters.Add(NewDbParameter(sFieldName, TypeConvertor.ToDbType(oData.GetType)))
      cmd.Parameters(sFieldName).Value = oData
    ElseIf (TypeOf cmd Is OdbcCommand) Then
      cmd.Parameters.Add(New OdbcParameter(sFieldName, oData))
    ElseIf (TypeOf cmd Is SqlClient.SqlCommand) Then
      If (alPictureFields.Contains(sFieldName.Replace("@", ""))) Then
        Dim p As DbParameter = New SqlClient.SqlParameter(sFieldName, SqlDbType.Image)
        p.Value = oData
        cmd.Parameters.Add(p)
      Else
        Dim p As DbParameter = New SqlClient.SqlParameter(sFieldName, TypeConvertor.ToSqlServerDbType(oData.GetType))
        p.Value = oData
        cmd.Parameters.Add(p)
      End If


    End If

  End Sub



  Public Function ChangeDBPassword(ByVal sConnectionString As String, ByVal sPassword As String) As Boolean
    Try
      If (g_iConnType = DB_TYPES.SQLITE) Then
        Dim cnn As SQLiteConnection = NewDBConnection(sConnectionString)
        cnn.Open()
        cnn.ChangePassword(sPassword)
        cnn.Close()
        Return True
      ElseIf (g_iConnType = DB_TYPES.MYSQL) Then
        Return False
      ElseIf (g_iConnType = DB_TYPES.OLEDB) Then
        Return False
      ElseIf (g_iConnType = DB_TYPES.SQLSERVER) Then
        Return False
      Else
        Return False
      End If
    Catch ex As Exception
      Return False
    End Try
  End Function

#If SQL_TRACE Then

  Public Sub ConnectionCreated(sender As Object)
    AddToDatabaseLog(sender.GetHashCode() & " SQLite Connection Created." & vbCrLf & GetStackTraceString(10), "SQL")
  End Sub
  Public Sub ConnectionDisposed(sender As Object, e As System.EventArgs)
    AddToDatabaseLog(sender.GetHashCode() & " SQLite Connection Disposed." & vbCrLf & GetStackTraceString(10), "SQL")
  End Sub

  Public Sub ConnectionTrace(sender As Object, sSQL As String)
    AddToDatabaseLog(sender.GetHashCode() & " SQLite Trace:" & sSQL, "SQL")
  End Sub
#End If
  Public g_sSQLiteDBJournalMode As String = "DELETE" 'DELETE | TRUNCATE | PERSIST | MEMORY | WAL | OFF 
  Public g_sSQLiteDBSynchronousMode As String = "FULL" '0 | OFF | 1 | NORMAL | 2 | FULL
  Public g_sSQLiteDBLockingMode As String = "NORMAL" 'NORMAL | EXCLUSIVE


  Public Function NewDBConnection(ByVal sConnectionString As String, Optional ByVal db_type As DB_TYPES = DB_TYPES.UNSPECIFIED) As DbConnection

    If (db_type = DB_TYPES.UNSPECIFIED) Then
      If g_iConnType = DB_TYPES.UNSPECIFIED Then
        db_type = DB_TYPES.SQLITE
      Else
        db_type = g_iConnType
      End If
    Else
      db_type = db_type
    End If

    Select Case db_type

      Case DB_TYPES.SQLITE
        Dim conn As New SQLiteConnection(sConnectionString & ";Synchronous=" & g_sSQLiteDBSynchronousMode & ";locking_mode=" & g_sSQLiteDBLockingMode & ";journal_mode=" & g_sSQLiteDBJournalMode & ";")
#If SQL_TRACE Then
        ConnectionCreated(conn)
        AddHandler conn.Disposed, AddressOf ConnectionDisposed
#End If
        Return conn
#If MYSQL_NATIVE Then
      Case DB_TYPES.MYSQL
        Return New MySqlConnection(sConnectionString)
#End If

      Case DB_TYPES.SQLSERVER
        Return New SqlClient.SqlConnection(sConnectionString)

      Case DB_TYPES.OLEDB
        Return New OleDbConnection(sConnectionString)
      Case DB_TYPES.ODBC
        Return New OdbcConnection(sConnectionString)
      Case Else

        Return Nothing

    End Select

  End Function

  Public Function TryDBConnection(localConnString As String, ByRef sErr As String, Optional db_type As DB_TYPES = DB_TYPES.UNSPECIFIED) As Boolean

    Dim conn As DbConnection, bRet As Boolean = False
    If (db_type = DB_TYPES.UNSPECIFIED) Then
      If g_iConnType = DB_TYPES.UNSPECIFIED Then
        db_type = DB_TYPES.SQLITE
      Else
        db_type = g_iConnType
      End If
    Else
      db_type = db_type
    End If

    sErr = "Connection Successful" & vbCrLf & localConnString
    If db_type = DB_TYPES.SQLITE Then
      Dim tc As Char() = {" ", """", ";"}
      If File.Exists(localConnString.Replace("Data Source=", "").Trim(tc)) = False Then
        'SQLite Database Doesn't Exist
        g_bConnectionValid = False
        sErr = "Specified SQLite File in Connection String Does not exist" & vbCrLf & localConnString
        Return g_bConnectionValid
      End If
    End If

    Try
      ' Open Database connection
      conn = NewDBConnection(localConnString, db_type)
      conn.Open()
      conn.Close()
      bRet = True
    Catch ex As Exception
      sErr = "Error connecting to database" & vbCrLf & ex.Message
    End Try
    g_bConnectionValid = bRet
    Return bRet

  End Function

  Public Function TryDBConnectionAndReport(sConn As String, Optional db_type As DB_TYPES = DB_TYPES.UNSPECIFIED) As Boolean

    Dim bRet As Boolean, sErr As String = ""
    If (db_type = DB_TYPES.UNSPECIFIED) Then
      If g_iConnType = DB_TYPES.UNSPECIFIED Then
        db_type = DB_TYPES.SQLITE
      Else
        db_type = g_iConnType
      End If
    Else
      db_type = db_type
    End If

    bRet = TryDBConnection(sConn, sErr, db_type)

    If (bRet = False) Then
      MsgBox(sErr, MsgBoxStyle.OkOnly, "Database Connection Error")
    Else
      MsgBox(sErr, MsgBoxStyle.OkOnly, "Database Connection Valid")
    End If

    Return bRet
  End Function
  Public Function TryToOpenNewDBFile(OpenFileDialog As FileDialog, dbtype As DB_TYPES, sErr As String) As String
    Dim ret As MsgBoxResult = MsgBoxResult.Cancel

    TryToOpenNewDBFile = "" 'failure to connect default
    sErr = ""
    OpenFileDialog.Title = "Please Enter Name Of New Database"
    OpenFileDialog.CheckFileExists = False
    OpenFileDialog.InitialDirectory = g_sPathData
    If dbtype = DB_TYPES.SQLITE Then
      OpenFileDialog.FileName = "xpress_entry.db3"
      OpenFileDialog.Filter = "SQLite Database (*.DB3)|*.db3| All files (*.*)|*.*"
    Else
      sErr = "Unsupported database type for new databases."
    End If
    If sErr = "" Then
      Do
        If (OpenFileDialog.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
          Dim sFileName As String = OpenFileDialog.FileName
          If sFileName <> "" Then
            If File.Exists(sFileName) Then
              ret = MsgBox("File exists (" & sFileName & "). Would you like to use this database instead of creating a new one?  Answering No will allow you to select a different unused file name.",
                           MsgBoxStyle.YesNoCancel Or MsgBoxStyle.MsgBoxSetForeground, "File Exists")
            ElseIf SaveTextToFile("", sFileName) Then
              ret = MsgBoxResult.Yes
            Else
              ret = MsgBoxResult.Cancel
            End If
            If ret = MsgBoxResult.Yes Then
              Dim sConn = "Data Source=" & sFileName & ";"
              'because TryDBConnection doesn't take a connection type, and it can be different now
              'need to send temporary new dbtype without resetting g_sConn
              If TryDBConnection(sConn, sErr, dbtype) Then
                TryToOpenNewDBFile = sConn
              End If
            End If
          End If
        Else
          Exit Do 'canceled file dialog
        End If
      Loop While ret = MsgBoxResult.No 'answered no to use database - try again
      'either canceled or answered yes to something - exiting with successful db file name or blank string if nothing to do
    End If
  End Function

  Public Function convertListToArray(ByVal ls As List(Of String)) As String()
    Dim aString As String() = Nothing, i As Integer = 0
    If ls.Count > 0 Then
      ReDim aString(0 To ls.Count - 1)
      For Each o As Object In ls
        aString(i) = o
        i += 1
      Next
    End If
    If aString Is Nothing Then ReDim aString(-1)
    Return aString
  End Function

  '===============================================================================
  ' Name:    GetFieldFromField
  ' Purpose: Return sfield value from table named sTable where id = lID
  '===============================================================================
  Public Function GetFieldFromField(ByVal sSearchValue As Object, ByVal sTable As String, _
                                 Optional ByVal sField As String = "name", _
                                 Optional ByVal sNullString As String = "", _
                                 Optional conn As DbConnection = Nothing, _
                                 Optional trans As DbTransaction = Nothing, _
                                 Optional search_field As String = "id", _
                                 Optional sOrder As String = "") As String

    Dim rdr As DbDataReader = Nothing
    Dim cmd As DbCommand = Nothing
    Dim sSQL As String = ""
    Dim bFound As Boolean = False
    Dim sRet As String = ""
    Dim bIncomingConnection As Boolean = (conn IsNot Nothing)

    Dim sSearchActual As String = ""
    If (TypeOf sSearchValue Is String) Then
      sSearchActual = search_field & " ='" & sSearchValue & "'"
    Else
      sSearchActual = search_field & " =" & sSearchValue
    End If

    Try
      'If we are looking for user's name then concatenate fields to make name
      If sTable = "users" And sField = "name" Then
        sSQL = "Select " & GetSQLForFullName() & " AS FieldValue " & _
             "FROM " & sTable & " WHERE " & sSearchActual
      Else
        sSQL = "Select " & sField & " As FieldValue FROM " & sTable & " WHERE " & sSearchActual
      End If
      If (sOrder <> "") Then
        sSQL &= sOrder
      End If
      'If Not g_SharedConnection Is Nothing AndAlso (g_SharedConnection.State = ConnectionState.Open) Then conn = g_SharedConnection
      If (Not bIncomingConnection) Then
        conn = NewDBConnection(g_sConn)
        conn.Open()
      End If

      cmd = NewDbCommand(sSQL, conn)
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If
      rdr = cmd.ExecuteReader()

      If rdr.HasRows Then
        While rdr.Read
          sRet = NullToString(rdr.Item("FieldValue"))
          bFound = True
        End While
      End If
      rdr.Close()
      If Not bFound Then sRet = sNullString
    Catch e As Exception
      AddToDatabaseLog(GetExceptionText(e), "Error")
    Finally
      If (Not bIncomingConnection) Then
        Call CloseCmdAndConn(cmd, conn)
      End If
    End Try
    Return sRet
  End Function

  '===============================================================================
  ' Name:    GetFieldFromID
  ' Purpose: Return sfield value from table named sTable where id = lID
  '===============================================================================
  Public Function GetFieldFromID(ByVal lID As Integer, ByVal sTable As String, _
                                 Optional ByVal sField As String = "name", _
                                 Optional ByVal sNullString As String = "", _
                                 Optional conn As DbConnection = Nothing, _
                                 Optional trans As DbTransaction = Nothing, _
                                 Optional search_field As String = "id", _
                                 Optional sOrder As String = "", _
                                 Optional bSearchDeleted As Boolean = True) As String

    Dim rdr As DbDataReader = Nothing
    Dim cmd As DbCommand = Nothing
    Dim sSQL As String = ""
    Dim bFound As Boolean = False
    GetFieldFromID = ""
    Dim bIncomingConnection As Boolean = (conn IsNot Nothing)

    Try
      'If we are looking for user's name then concatenate fields to make name
      If sTable = "users" And sField = "name" Then
        sSQL = "Select " & GetSQLForFullName() & " AS FieldValue " & _
             "FROM " & sTable & " WHERE " & search_field & " =" & lID
      Else
        sSQL = "Select " & sField & " As FieldValue FROM " & sTable & " WHERE " & search_field & " =" & lID
      End If
      If Not bSearchDeleted Then
        sSQL &= " AND " & sTable & ".deleted_at IS NULL "
      End If
      If (sOrder <> "") Then
        sSQL &= sOrder
      End If
      'If Not g_SharedConnection Is Nothing AndAlso (g_SharedConnection.State = ConnectionState.Open) Then conn = g_SharedConnection
      If (Not bIncomingConnection) Then
        conn = NewDBConnection(g_sConn)
        conn.Open()
      End If

      cmd = NewDbCommand(sSQL, conn)
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If
      rdr = cmd.ExecuteReader()

      If rdr.HasRows Then
        While rdr.Read
          GetFieldFromID = NullToString(rdr.Item("FieldValue"))
          bFound = True
        End While
      End If
      rdr.Close()
      If Not bFound Then GetFieldFromID = sNullString
    Catch e As Exception
      AddToDatabaseLog(GetExceptionText(e), "Error")
    Finally
      If (Not bIncomingConnection) Then
        Call CloseCmdAndConn(cmd, conn)
      End If
    End Try

  End Function

  '===============================================================================
  ' Name:    GetIDFromField
  ' Purpose: Return 'id' field value from passed in table where sField = sFieldValue
  '===============================================================================
  Public Function GetIDFromField(ByVal sFieldValue As String, ByVal sTable As String, _
                                 Optional ByVal sField As String = "name", _
                                 Optional ByVal vNull As Object = vbNull, _
                                 Optional ByVal bSearchDeleted As Boolean = False, _
                                 Optional conn As DbConnection = Nothing, _
                                 Optional trans As DbTransaction = Nothing, _
                                 Optional ByVal bUseLikeInsteadofEquals As Boolean = False) As Integer

    Dim rdr As DbDataReader = Nothing
    Dim cmd As DbCommand = Nothing
    Dim sSQL As String = ""
    Dim bFound As Boolean = False
    Dim bIncomingConnection As Boolean = (conn IsNot Nothing)

    Dim iRet As Integer = INVALID_ID
    GetIDFromField = INVALID_ID
    Try

      Dim dbParam As DbParameter = NewDbParameter("param1", DbType.String, sFieldValue)

      If sTable = "users" And sField = "name" Then
        sSQL = "Select id FROM " & sTable & " WHERE " & GetSQLForFullName()
      Else
        sSQL = "Select id FROM " & sTable & " WHERE " & sField
      End If
      If (bUseLikeInsteadofEquals) Then
        sSQL &= " LIKE '" & NoSingleQuote(sFieldValue) & "'"
      Else
        sSQL &= " = '" & NoSingleQuote(sFieldValue) & "'"
      End If

      If (Not bSearchDeleted) Then

        'Add a deleted_at is null clause to query if table contains deleted_at field
        Select Case sTable
          Case "companies", "badges", "doors", "fingerprints", "groups", "groups_users", "groups_zones", "users_zones", "items", "manufacturers", "readers", "roles", "users", "zones", "pre_registration"
            sSQL &= " AND deleted_at IS NULL"
        End Select
      End If

      If Not bIncomingConnection Then
        conn = NewDBConnection(g_sConn)
        conn.Open()
      End If
      cmd = NewDbCommand(sSQL, conn)
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If

      cmd.Parameters.Add(dbParam)

      rdr = cmd.ExecuteReader()

      If rdr.HasRows Then
        While rdr.Read
          iRet = rdr.Item("id")
          bFound = True
          Exit While
        End While
      End If

      If Not bFound Then iRet = INVALID_ID

      rdr.Close()
    Catch e As Exception
      iRet = INVALID_ID
      AddToDatabaseLog(GetExceptionText(e), "Error")
    Finally
      If (Not bIncomingConnection) Then
        Call CloseCmdAndConn(cmd, conn)
      End If

    End Try
    Return iRet
  End Function

  Public Function GetSQLForFirstLastName(ByVal sTableName As String) As String
    If (g_iConnType = DB_TYPES.MYSQL) Then
      Return "CONCAT(" & sTableName & ".last_name,', '," & sTableName & ".first_name)"
    ElseIf (g_iConnType = DB_TYPES.SQLITE) Then

      'Dim sNoNullSQL As String = "CASE WHEN (%%='' or %% IS NULL) THEN '' WHEN %% != '' THEN %% END"
      'Return "RTRIM(" & _
      '       sNoNullSQL.Replace("%%", "last_name") & " || ', ' || " & _
      '       sNoNullSQL.Replace("%%", "first_name") & " || ' ' || " & _
      '       sNoNullSQL.Replace("%%", "mi") & _
      '       ")"
      Return sTableName & ".visitor_last_name || ', ' || " & sTableName & ".visitor_first_name"

    ElseIf g_iConnType = DB_TYPES.SQLSERVER Then
      Return "ISNULL(" & sTableName & ".last_name, '') + ', ' + ISNULL(" & sTableName & ".first_name, '')"
    Else
      Return ""
    End If

  End Function

  Public Function GetSQLForFullName() As String
    If (g_iConnType = DB_TYPES.MYSQL) Then
      Return "CONCAT(last_name,', ',first_name, if(mi='' or mi IS NULL,'', CONCAT(' ', mi)))"
    ElseIf (g_iConnType = DB_TYPES.SQLITE) Then

      Dim sNoNullSQL As String = "CASE WHEN (%%='' or %% IS NULL) THEN '' WHEN %% != '' THEN %% END"
      Return "RTRIM(" & _
             sNoNullSQL.Replace("%%", "last_name") & " || ', ' || " & _
             sNoNullSQL.Replace("%%", "first_name") & " || ' ' || " & _
             sNoNullSQL.Replace("%%", "mi") & _
             ")"

      'Return "last_name || ', ' || first_name || CASE WHEN (mi='' or mi IS NULL) THEN '' WHEN mi != '' THEN ' ' || mi END"
    ElseIf g_iConnType = DB_TYPES.SQLSERVER Then
      Return "ISNULL(last_name, '') + ', ' + ISNULL(first_name, '') + ' ' + ISNULL(mi, '')"
    Else
      Return ""
    End If

  End Function


  '===============================================================================
  '    Name: GenerateHash(ByVal SourceText As String) As String
  ' Remarks: Create an encoding object to ensure the encoding standard for the source text
  '===============================================================================
  Public Function GenerateHash(ByVal SourceText As String) As String
    Dim sRet As String = ""
    Dim Ue As New UnicodeEncoding()
    Dim ByteSourceText() As Byte = Ue.GetBytes(SourceText)
#If USE_MD5_CRYPT Then
    'Instantiate an MD5 Provider object
    'Compute the hash value from the source
    Dim ByteHashNew() As Byte = cMD5Crypt.RunMd5HashAsByteArray(ByteSourceText)
    'Dim sMD5New As String = Md5New.RunMd5HastAsString(SourceText)
    'And convert it to String format for return
    sRet = Convert.ToBase64String(ByteHashNew)

#Else
    'Create an encoding object to ensure the encoding standard for the source text
    'Retrieve a byte array based on the source text
    'Instantiate an MD5 Provider object
    Dim Md5 As New MD5CryptoServiceProvider()
    'Compute the hash value from the source
    Dim ByteHash() As Byte = Md5.ComputeHash(ByteSourceText)

    Dim sOldMD5Str As String = ByteArrayToString(ByteHash)
    'And convert it to String format for return
    sRet = Convert.ToBase64String(ByteHash)
#End If
    Return sRet
  End Function

  Public Function GetLastUpdated(ByVal sTable As String, Optional ByVal sField As String = "updated_at", Optional ByVal conn As DbConnection = Nothing) As String
    Dim rows As DataRowCollection = GetDataRowsBySQL("SELECT " & sField & " FROM " & sTable & " ORDER BY " & sField & " DESC LIMIT 1", conn)
    If (rows.Count > 0) Then
      Try
        Dim dtt As DateTime = rows(0).Item(sField)
        Return Format(dtt, DATE_FORMAT_SQLITE)
      Catch ex As Exception
        Return Format(New DateTime(1970, 1, 1), DATE_FORMAT_SQLITE)
      End Try
    Else
      Return Format(New DateTime(1970, 1, 1), DATE_FORMAT_SQLITE)
    End If
  End Function


  '===============================================================================
  '    Name: Sub HashTableFromString
  ' Remarks:
  '===============================================================================
  Public Function HashTableFromString(ByVal sURL As String, _
                    Optional ByVal sSectionSplit As String = "&", _
                    Optional ByVal sKeyValueSplit As String = "=", _
                    Optional ByVal sIgnoreBefore As String = "?", _
                    Optional ByVal sIgnoreBeforeKeyVal As String = " ") As Collections.Hashtable
    Dim ht As New Collections.Hashtable
    Dim asSections() As String
    Dim asKeyVal() As String
    Dim iPos As Integer = sURL.IndexOf(sIgnoreBefore)
    Dim trim_char() As Char = {"&"}
    asSections = Split(sURL.Substring(iPos + sIgnoreBefore.Length), sSectionSplit)

    For Each sSection As String In asSections
      asKeyVal = Split(sSection, sKeyValueSplit)
      If asKeyVal.Length = 2 Then
        ht.Add(asKeyVal(0).TrimStart(trim_char).Trim, asKeyVal(1).Trim)
      End If
    Next

    Return ht
  End Function


  '==============================================================================
  ' Name:    GetMaxIdFromTable
  ' Purpose: Return max 'id' field value from passed in table
  '===============================================================================
  Public Function GetMaxIdFromTable(ByVal sTable As String, _
                                    Optional ByVal conn As DbConnection = Nothing, _
                                    Optional ByVal trans As DbTransaction = Nothing, _
                                    Optional ByRef sErr As String = "") As Integer

    Dim rdr As DbDataReader = Nothing
    Dim cmd As DbCommand = Nothing
    Dim sSQL As String = "Select max(id) as MaxID FROM " & sTable
    Dim iRet As Integer = INVALID_ID
    Dim bIncomingConn As Boolean = False
    Try
      If (conn Is Nothing) Then
        conn = NewDBConnection(g_sConn)
        conn.Open()
      Else
        bIncomingConn = True
      End If
      cmd = NewDbCommand(sSQL, conn)
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If

      rdr = cmd.ExecuteReader()

      If rdr.HasRows Then
        While rdr.Read
          iRet = rdr.Item("MaxID")
        End While
      End If
      rdr.Close()
    Catch ex As Exception
      sErr = ("Error getting the max id from table " & sTable & ": " & ex.Message.ToString)
    Finally
      If (Not bIncomingConn) Then
        CloseCmdAndConn(cmd, conn)
      End If

    End Try

    Return iRet
  End Function


  Public Function ExecuteScalarQuery(ByVal sSQL As String, Optional ByVal sConnectionString As String = "", _
                                     Optional ByVal conn As DbConnection = Nothing, _
                                     Optional ByVal trans As DbTransaction = Nothing, _
                                     Optional ByVal sErr As String = "") As Integer
    Dim lNum As Integer

    If (sConnectionString = "") Then
      sConnectionString = g_sConn
    End If
    Dim cmd As DbCommand = Nothing
    Dim bUsingIncomingConnection As Boolean = True
    If (conn Is Nothing) Then
      bUsingIncomingConnection = False
      conn = NewDBConnection(sConnectionString, g_iConnType)
    End If


    Try
      If conn.State = ConnectionState.Closed Then
        conn.Open()
      End If
      cmd = NewDbCommand(sSQL, conn)
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If
      lNum = cmd.ExecuteScalar()
      '     sErr = "SQL SUCCESS:  " & lNum & " items Found"
    Catch ex As Exception
      lNum = -1
      sErr = "ExecuteScalarQuery FAILURE: " & ex.Message
      'MsgBox(sErr)
      Debug.Print(Now.ToLongTimeString & sErr)
    Finally
      If cmd IsNot Nothing Then
        cmd.Dispose()
        cmd = Nothing
      End If

    End Try
    If Not bUsingIncomingConnection Then
      conn.Close()
      conn.Dispose()
      conn = Nothing
    End If
    Return lNum
  End Function


  Public Function ExecuteScalarQuery(ByVal sSQL As String,
                                     ByVal lstParams As List(Of DbParameter), _
                                     Optional ByVal sConnectionString As String = "", _
                                     Optional ByVal conn As DbConnection = Nothing, _
                                     Optional ByVal trans As DbTransaction = Nothing, _
                                     Optional ByVal sErr As String = "") As Integer
    Dim lNum As Integer

    If (sConnectionString = "") Then
      sConnectionString = g_sConn
    End If
    Dim cmd As DbCommand = Nothing
    Dim bUsingIncomingConnection As Boolean = True
    If (conn Is Nothing) Then
      bUsingIncomingConnection = False
      conn = NewDBConnection(sConnectionString, g_iConnType)
    End If


    Try
      If conn.State = ConnectionState.Closed Then
        conn.Open()
      End If
      cmd = NewDbCommand(sSQL, conn)
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If
      For Each p As DbParameter In lstParams
        cmd.Parameters.Add(p)
      Next

      lNum = cmd.ExecuteScalar()
      '     sErr = "SQL SUCCESS:  " & lNum & " items Found"
    Catch ex As Exception
      lNum = -1
      sErr = "ExecuteScalarQuery FAILURE: " & ex.Message

    Finally
      If cmd IsNot Nothing Then
        cmd.Dispose()
        cmd = Nothing
      End If

    End Try
    If Not bUsingIncomingConnection Then
      conn.Close()
      conn.Dispose()
      conn = Nothing
    End If
    Return lNum
  End Function

  Public Function ExecuteScalarQueryGeneral(ByVal sSQL As String, Optional ByVal sConnectionString As String = "", _
                                   Optional ByVal conn As DbConnection = Nothing, _
                                   Optional ByVal trans As DbTransaction = Nothing, _
                                   Optional ByVal sErr As String = "") As Object
    Dim RetVal As Object

    If (sConnectionString = "") Then
      sConnectionString = g_sConn
    End If
    Dim cmd As DbCommand = Nothing
    Dim bUsingIncomingConnection As Boolean = True
    If (conn Is Nothing) Then
      bUsingIncomingConnection = False
      conn = NewDBConnection(sConnectionString, g_iConnType)
    End If

    Try
      If conn.State = ConnectionState.Closed Then
        conn.Open()
      End If
      cmd = NewDbCommand(sSQL, conn)
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If

      RetVal = cmd.ExecuteScalar()
      '     sErr = "SQL SUCCESS:  " & lNum & " items Found"
    Catch ex As Exception
      RetVal = Nothing
      sErr = "ExecuteScalarQueryGeneral FAILURE: " & ex.Message
      'MsgBox(sErr)
      Debug.Print(Now.ToLongTimeString & sErr)
    Finally
      If cmd IsNot Nothing Then
        cmd.Dispose()
        cmd = Nothing
      End If
    End Try

    If Not bUsingIncomingConnection Then
      conn.Close()
      conn.Dispose()
      conn = Nothing
    End If

    Return RetVal
  End Function

  '
  Public Function ExecuteNonQuery(ByRef sSQL As String, _
                                  ByVal lstParams As List(Of DbParameter), _
                                  Optional ByRef sErr As String = "", _
                                  Optional ByVal connection As DbConnection = Nothing, _
                                  Optional ByVal transaction As DbTransaction = Nothing) As Integer
    Dim lNum As Integer
    Dim s As New Stopwatch
    s.Start()
    ' SQL_NOW() is a database speciic instance of a UTC time for now
    sSQL = SQL_NOW(sSQL)
    Dim conn As DbConnection = Nothing
    Dim cmd As DbCommand = Nothing
    Dim bUsingIncomingConnection As Boolean = False

    Using conn
      If Not (connection Is Nothing) Then
        bUsingIncomingConnection = True
        conn = connection
      Else
        conn = NewDBConnection(g_sConn)
        conn.Open()
      End If

      cmd = NewDbCommand(sSQL, conn)
      If (transaction IsNot Nothing) Then cmd.Transaction = transaction

      For Each p As DbParameter In lstParams
        cmd.Parameters.Add(p)
      Next
      sErr = ""
      Try
        lNum = cmd.ExecuteNonQuery()
        sErr = "SQL SUCCESS:  " & lNum & " items Changed"
      Catch ex As Exception
        sErr = "ExecuteNonQuery FAILURE: " & sSQL.Replace(vbCrLf, " ") & vbCrLf & ex.Message
        Debug.Print(Now.ToLongTimeString & sErr)
        'MsgBox(sErr)
        lNum = -1
      Finally
        cmd.Dispose()
        cmd = Nothing
        s.Stop()
        sErr = "Time for ExecuteNonQuery: " & s.ElapsedMilliseconds & vbCrLf & sErr
      End Try
      If (Not bUsingIncomingConnection) Then
        conn.Close()
        conn.Dispose()
        conn = Nothing
      End If
    End Using
    Return lNum

  End Function

  Public Function ExecuteNonQuery(ByRef sSQL As String, Optional ByRef sErr As String = "", _
                                  Optional ByVal connection As DbConnection = Nothing, _
                                  Optional ByVal transaction As DbTransaction = Nothing) As Integer
    Dim lNum As Integer
    Dim s As New Stopwatch
    s.Start()

    Dim conn As DbConnection = Nothing
    Dim cmd As DbCommand = Nothing
    Dim bUsingIncomingConnection As Boolean = False

    Using conn
      If Not (connection Is Nothing) Then
        bUsingIncomingConnection = True
        conn = connection
      Else
        conn = NewDBConnection(g_sConn)
        conn.Open()
      End If

      cmd = NewDbCommand(sSQL, conn)
      If (transaction IsNot Nothing) Then cmd.Transaction = transaction
      sErr = ""
      Try
        lNum = cmd.ExecuteNonQuery()
        If (lNum = -1) Then
          lNum = 0
        End If
        sErr = "SQL SUCCESS:  " & lNum & " items Changed"
      Catch ex As Exception
        sErr = "ExecuteNonQuery FAILURE: " & sSQL.Replace(vbCrLf, " ") & vbCrLf & ex.Message
        Debug.Print(Now.ToLongTimeString & sErr)
        'MsgBox(sErr)
        lNum = -1
      Finally
        cmd.Dispose()
        cmd = Nothing
        s.Stop()
        sErr = "Time for ExecuteNonQuery: " & s.ElapsedMilliseconds & vbCrLf & sErr
      End Try
      If (Not bUsingIncomingConnection) Then
        conn.Close()
        conn.Dispose()
        conn = Nothing
      End If
    End Using
    Return lNum
  End Function

  'THIS IS INCOMPLETE!  Make sure your data types are being handled appropriately. Defaults to VarBinary.
  'Public Function GetOleDbType(ByVal data As Object) As OleDbType
  '  If TypeOf data Is String Then
  '    Return OleDbType.VarChar
  '  ElseIf TypeOf data Is Boolean Then
  '    Return OleDbType.Boolean
  '  ElseIf TypeOf data Is Double Then
  '    Return OleDbType.Currency
  '  ElseIf TypeOf data Is [Enum] Then
  '    Return OleDbType.Numeric
  '  ElseIf TypeOf data Is Integer Then
  '    Return OleDbType.Numeric
  '  ElseIf TypeOf data Is Date Then
  '    Return OleDbType.Date
  '  Else 'If TypeOf data Is object Then
  '    Return OleDbType.VarBinary
  '  End If
  'End Function

  Public Function GetDbType(ByVal data As Object) As DbType
    If TypeOf data Is String Then
      Return DbType.String
    ElseIf TypeOf data Is Boolean Then
      Return DbType.Boolean
    ElseIf TypeOf data Is Double Then
      Return DbType.Currency
    ElseIf TypeOf data Is [Enum] Then
      Return DbType.Int32
    ElseIf TypeOf data Is Integer Then
      Return DbType.Int32
    ElseIf TypeOf data Is Date Then
      Return DbType.Date
    ElseIf TypeOf data Is DateTime Then
      Return DbType.Date
    Else 'If TypeOf data Is object Then
      Return DbType.Binary
    End If
  End Function

  Public Function UpdateRecord(ByVal lsFields As List(Of String), ByVal loData As List(Of Object), _
                               ByVal sTable As String, ByVal sConnString As String, _
                               Optional ByRef sWhere As String = "", Optional ByRef iRowsAffected As Integer = 0, _
                               Optional ByRef conn As DbConnection = Nothing, _
                               Optional ByVal trans As DbTransaction = Nothing, _
                               Optional ByRef sErr As String = "", _
                               Optional ByVal bCloseConn As Boolean = True) As Boolean
    Dim cmd As DbCommand = Nothing
    Dim bDebug As Boolean = False
    If bDebug Then Debug.Print("Update:" & sTable)

    'Make sure our lists match up
    If (lsFields.Count <> loData.Count) AndAlso (lsFields.Count > 0) Then
      sErr = "Data to update is no good."
      Return False
    End If

    Try
      If conn Is Nothing Then
        conn = NewDBConnection(sConnString)
        conn.Open()
      End If

      cmd = conn.CreateCommand()
      If Not trans Is Nothing Then cmd.Transaction = trans

      'Build insert command fields and add data params to DbCommand object
      Dim sFieldsSQL As String = ""
      For i As Integer = 0 To lsFields.Count - 1
        Dim field As String = lsFields(i)

        Dim data As Object = IIf(loData(i) Is Nothing, DBNull.Value, loData(i))
        If (TypeOf loData(i) Is System.DateTime) Then
          Dim dtRef As DateTime = loData(i)
          DateTime.SpecifyKind(dtRef, DateTimeKind.Unspecified)
        End If
        If (loData(i) Is Nothing) Then
          sFieldsSQL &= field & "=@" & field & ","
          AddNewDBParameter(cmd, "@" & field, data)
        Else
          sFieldsSQL &= field & "=@" & field & ","
          AddNewDBParameter(cmd, "@" & field, data)
        End If
        If bDebug Then Debug.Print(field & " : " & data.ToString)

        'Make sure your data types are being handled appropriately. GetDbType defaults to Binary.
        'cmd.Parameters.Add(NewDbParameter("@" & field, GetDbType(data)))
        'cmd.Parameters.Add(NewDbParameter("@" & field, TypeConvertor.ToDbType(data.GetType)))
        'cmd.Parameters("@" & field).Value = data
      Next

      'Remove trailing commas
      sFieldsSQL = sFieldsSQL.Substring(0, sFieldsSQL.Length - 1)

      'Assemble insert command
      cmd.CommandText = "UPDATE " & sTable & " SET " & sFieldsSQL & " " & sWhere

      'Execute the query
      iRowsAffected = cmd.ExecuteNonQuery()

    Catch ex As Exception
      sErr = "Database update failed: " & ex.Message
    Finally
      If bCloseConn Then
        CloseCmdAndConn(cmd, conn)
      Else
        If cmd IsNot Nothing Then cmd.Dispose()
      End If
    End Try

    Return (iRowsAffected > 0)
  End Function

  Public Function InsertRecord(ByVal lsFields As List(Of String), ByVal loData As List(Of Object), _
                               ByVal sTable As String, ByVal sConnString As String, _
                               Optional ByRef iRowsAffected As Integer = 0, _
                               Optional ByRef conn As DbConnection = Nothing, _
                               Optional ByRef trans As DbTransaction = Nothing, _
                               Optional ByRef sErr As String = "", _
                               Optional ByVal bCloseConn As Boolean = True,
                               Optional ByRef iLastInsertedID As Integer = INVALID_ID) As Boolean
    Dim cmd As DbCommand = Nothing
    Dim bDebug As Boolean = False
    Dim iValID As Integer = INVALID_ID
    If bDebug Then Debug.Print("Insert:" & sTable)
    'Make sure our lists match up
    If (lsFields.Count <> loData.Count) AndAlso (lsFields.Count > 0) Then
      sErr = "Data to insert is no good."
      Return False
    End If

    Try
      If conn Is Nothing Then
        conn = NewDBConnection(sConnString)
        conn.Open()
      End If

      cmd = conn.CreateCommand()
      If Not trans Is Nothing Then cmd.Transaction = trans

      'Build insert command fields and params and add data params to DbCommand object
      Dim sFieldsSQL As String = "", sParamsSQL As String = ""
      For i As Integer = 0 To lsFields.Count - 1
        Dim field As String = lsFields(i)
        Dim data As Object = IIf(loData(i) Is Nothing, DBNull.Value, loData(i))
        If (TypeOf loData(i) Is System.DateTime) Then
          Dim dtRef As DateTime = loData(i)
          DateTime.SpecifyKind(dtRef, DateTimeKind.Unspecified)
        End If

        If lsFields(i) = "id" Then
          iValID = loData(i)
        End If

        sFieldsSQL &= field & ","
        sParamsSQL &= "@" & field & ","
        'cmd.Parameters.AddWithValue("@" & field, data)
        'cmd.Parameters.Add(NewDbParameter("@" & field, GetDbType(data)))
        If bDebug Then Debug.Print(field & " : " & data.ToString)
        AddNewDBParameter(cmd, "@" & field, data)
        'cmd.Parameters.Add(NewDbParameter("@" & field, TypeConvertor.ToDbType(data.GetType)))
        'cmd.Parameters("@" & field).Value = data
      Next

      'Remove trailing commas
      sFieldsSQL = sFieldsSQL.Substring(0, sFieldsSQL.Length - 1)
      sParamsSQL = sParamsSQL.Substring(0, sParamsSQL.Length - 1)

      'Assemble insert command
      cmd.CommandText = "INSERT INTO " & sTable & " (" & sFieldsSQL & ") " & "VALUES (" & sParamsSQL & ")"

      'Execute the query
      iRowsAffected = cmd.ExecuteNonQuery()

      If iValID = INVALID_ID Then
        iLastInsertedID = ExecuteScalarQuery(SQL_LAST_INSERTED_IDENTITY(conn), sConnString, conn, trans)
      Else
        iLastInsertedID = iValID
      End If

    Catch ex As Exception
      sErr = "Database insert failed: " & ex.Message
    Finally
      If bCloseConn Then
        CloseCmdAndConn(cmd, conn)
      Else
        If cmd IsNot Nothing Then cmd.Dispose()
      End If
    End Try

    Return (iRowsAffected > 0)
  End Function

  '===============================================================================
  '    Name: Function CreateSqlWHERE
  ' Remarks: Create a where statement using the passed in search text
  '          ex: "WHERE ((desc LIKE '%foo%' OR name LIKE '%foo%') AND (desc LIKE '%bar%' OR name LIKE '%bar%'))
  '===============================================================================
  Public Function CreateSqlWhereLike(ByVal sFields As List(Of String), _
                                     ByVal sSearchTerm As String, _
                                     Optional ByVal bIncludeWHERE As Boolean = True, _
                                     Optional ByVal bIgnoreID As Boolean = True
                                     ) As String
    Dim sWHERE As String = ""

    If sSearchTerm <> "" Then
      'words don't have to be adjacent
      Dim sWords() As String = Split(sSearchTerm)

      For Each s As String In sWords
        sWHERE &= "("

        For Each field As String In sFields
          If field = "id" Then
            If bIgnoreID Then
              Continue For
            End If
          End If

          If (InStr(field, ".") > 0) Then
            sWHERE += "" & field.Trim & " LIKE '%" & s.Replace("'", "''") & "%' OR "
          Else
            sWHERE += "[" & field.Trim & "] LIKE '%" & s.Replace("'", "''") & "%' OR "
          End If


        Next

        'Get rid of trailing 'OR'
        If (sWHERE.Length = 1) Then
          sWHERE = ""
        Else
          sWHERE = sWHERE.Substring(0, sWHERE.Length - " OR ".Length) & ") AND "
        End If

      Next

      'Get rid of trailing 'AND'
      sWHERE = IIf(bIncludeWHERE, " WHERE ", "") & " (" & sWHERE.Substring(0, sWHERE.Length - " AND ".Length) & ")"
    End If

    Return sWHERE
  End Function

  '===============================================================================
  '    Name: Function CreateSqlWhereEquals
  ' Remarks: Create a where statement using the passed in search text must equal one of the fields
  '          ex: "WHERE ((desc = 'foo' OR name = 'foo') AND (desc = 'bar' OR name = 'bar'))
  '===============================================================================
  Public Function CreateSqlWhereEquals(ByVal sFields As List(Of String), ByVal sSearchTerm As String, _
                                 Optional ByVal bIncludeWHERE As Boolean = True, _
                                 Optional ByVal bSplit As Boolean = True) As String
    Dim sWHERE As String = ""

    If sSearchTerm <> "" Then
      'words don't have to be adjacent
      Dim sWords() As String
      If (bSplit) Then
        sWords = Split(sSearchTerm)
      Else
        ReDim sWords(0 To 0)
        sWords(0) = sSearchTerm
      End If


      For Each s As String In sWords
        sWHERE &= "("
        For Each field As String In sFields
          If (InStr(field, ".") > 0) Then
            sWHERE += "" & field.Trim & " = '" & s.Replace("'", "''") & "' OR "
          Else
            sWHERE += "[" & field.Trim & "] = '" & s.Replace("'", "''") & "' OR "
          End If

        Next

        'Get rid of trailing 'OR'
        sWHERE = sWHERE.Substring(0, sWHERE.Length - " OR ".Length) & ") AND "
      Next

      'Get rid of trailing 'AND'
      sWHERE = IIf(bIncludeWHERE, " WHERE ", "") & " (" & sWHERE.Substring(0, sWHERE.Length - " AND ".Length) & ")"
    End If

    Return sWHERE
  End Function

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

  ''' <summary>
  ''' Copies a record to a new field in the database
  ''' If iExistingRecordID is -1 then it'll just make a raw copy(not recomended)
  ''' </summary>
  ''' <param name="sTable">Table we're copying to/from</param>
  ''' <param name="iExistingRecordID">Existing record we're copying</param>
  ''' <param name="lstFieldNames">List of field names we're changing from copied record- defaults to just 'Name'</param>
  ''' <param name="lstFieldValues">List of field values we're changing from copied record- defaults to just 'New Record'</param>
  ''' <param name="conn"></param>
  ''' <param name="trans"></param>
  ''' <returns>Number of records inserted.  Good value is 1</returns>
  ''' <remarks></remarks>
  ''' 
  Public Function CopyRecord(sTable As String, iExistingRecordID As Integer, _
                                        Optional lstFieldNames As List(Of String) = Nothing, Optional lstFieldValues As List(Of Object) = Nothing, _
                                        Optional ByRef conn As DbConnection = Nothing, Optional ByRef trans As DbTransaction = Nothing, _
                                        Optional ByRef iInsertedID As Integer = INVALID_ID) As Integer
    Dim sSQL As String = "", sErr As String = ""
    Dim lstParams As New List(Of DbParameter)
    Dim iRet As Integer = INVALID_ID


    'List of all the fields in the specified table
    Dim lstFieldList As List(Of String) = GetTableFieldsList(sTable, conn, trans)

    'Auto-fill in the created_at and updated_at fields
    Dim bHasUpdatedAt As Boolean = lstFieldList.Contains("updated_at")
    Dim bHasCreatedAt As Boolean = lstFieldList.Contains("created_at")

    If (lstFieldNames Is Nothing) Then
      lstFieldNames = New List(Of String)
      lstFieldNames.Add("name")

      If (bHasUpdatedAt) Then
        lstFieldNames.Add("updated_at")
      End If
      If (bHasCreatedAt) Then
        lstFieldNames.Add("created_at")
      End If
    End If

    If (lstFieldValues Is Nothing) Then
      lstFieldValues = New List(Of Object)
      lstFieldValues.Add("New Record")
      If (bHasUpdatedAt) Then
        lstFieldValues.Add(UtcNowForDBInsert())
      End If
      If (bHasCreatedAt) Then
        lstFieldValues.Add(UtcNowForDBInsert())
      End If
    End If

    'Remove bad inputs cases
    If (lstFieldList Is Nothing) OrElse lstFieldList.Count = 0 Then
      Return -1
    End If
    If (lstFieldNames.Count <> lstFieldValues.Count) Then
      Return -1
    End If

    'List of all the non-replaced fields and special fields
    'Ignore the id field 
    'Ignore updated_at or created_at or deleted_at
    'Ignore anything in our replacement names
    'We pull out our replacement names so we can order them properly
    Dim sBaseFieldList As String = ""
    For Each sField As String In lstFieldList
      If (sField.ToLower.Trim = "id") OrElse (sField.ToLower.Trim = "updated_at") OrElse (sField.ToLower.Trim = "created_at") OrElse (sField.ToLower.Trim = "deleted_at") Then
        Continue For
      End If
      If (lstFieldNames.Contains(sField)) Then
        Continue For
      End If
      sBaseFieldList &= sField & ","
    Next
    'Leave the comma at the end of the base field list.  We WILL be following this with more
    'sBaseFieldList = sBaseFieldList.TrimEnd(",")

    sSQL = "INSERT INTO " & sTable & " ("

    'Add the base field list
    sSQL &= sBaseFieldList

    'We are still just the field list with the new names in our order
    For Each sField As String In lstFieldNames
      sSQL &= sField & ","
    Next
    sSQL = sSQL.TrimEnd(",")

    'Close the field list
    sSQL &= ") "


    'Build the values.  They'll be a select statement from sTable
    sSQL &= " SELECT "

    'Start with the base field list
    sSQL &= sBaseFieldList

    'Instead of just the name, use the parameter
    For Each sField As String In lstFieldNames
      sSQL &= "@" & sField & ","
    Next
    sSQL = sSQL.TrimEnd(",")

    sSQL &= " FROM " & sTable & " WHERE id=" & iExistingRecordID
    'Add the parameters for the query
    For i As Integer = 0 To lstFieldNames.Count - 1
      lstParams.Add(NewDbParameter(lstFieldNames(i), TypeConvertor.ToDbType(lstFieldValues(i).GetType), lstFieldValues(i)))
    Next

    iRet = ExecuteNonQuery(sSQL, lstParams, sErr, conn, trans)
    If (iRet <> 1) Then
      MsgBox("Error Copying Record: " & sTable & "  " & iExistingRecordID)
    End If

    iInsertedID = ExecuteScalarQuery(SQL_LAST_INSERTED_IDENTITY(conn), g_sConn, conn, trans)

    Return iRet
  End Function

  Public Function GetDataRowFromTable(ByVal iID As Integer, ByVal sTable As String, Optional ByVal sSQL As String = "", Optional ByRef sErr As String = "", _
                                      Optional ByVal conn As DbConnection = Nothing, Optional ByVal trans As DbTransaction = Nothing) As DataRow
    If (sSQL = "") Then
      sSQL = "SELECT * from " & sTable & " WHERE id=" & iID
    End If
    Dim drColl As DataRowCollection = GetDataRowsBySQL(sSQL, conn, trans, sErr)
    If (drColl Is Nothing) Then
      ' sErr filled out due to error
      Return Nothing
    ElseIf drColl.Count <= 0 Then
      sErr = "No Data Returned"
      Return Nothing
    Else
      Return drColl(0)
    End If
  End Function


  '===============================================================================
  '    Name: GetDataRows
  ' Remarks: Returns Multiple DataRows from a pre-build select query
  '          This is good for non-unique fields
  '===============================================================================
  Public Function GetDataRows(ByVal sTablename As String, _
                                  ByVal sSearchFieldName As String, _
                                  ByVal sSearchFieldData As String, _
                                  Optional ByVal conn As DbConnection = Nothing, _
                                  Optional ByVal trans As DbTransaction = Nothing) As DataRowCollection
    Dim sSQL As String = "SELECT * FROM " & sTablename & " WHERE " & sSearchFieldName & " = '" & sSearchFieldData & "'"
    Return GetDataRowsBySQL(sSQL, conn, trans)
  End Function

  '===============================================================================
  '    Name: GetDataRowsBySQL
  ' Remarks: Returns Multiple DataRows with any SQL query
  '===============================================================================
  Public Function GetDataRowsBySQL(ByVal sSQL As String, _
                                   Optional ByVal conn As DbConnection = Nothing, _
                                   Optional ByVal trans As DbTransaction = Nothing, _
                                   Optional ByRef sErr As String = "") As DataRowCollection

    Dim bUsingIncomingConnection As Boolean = True
    Dim da As DbDataAdapter = Nothing
    Dim cmd As DbCommand = Nothing
    Try
      If (conn Is Nothing) Then


        bUsingIncomingConnection = False

        conn = NewDBConnection(g_sConn)
        conn.Open()
      End If
      cmd = NewDbCommand(sSQL, conn)
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If

      da = NewDbDataAdapter(cmd)


      Dim dt As New DataTable
      da.Fill(dt)
      Return dt.Rows

    Catch ex As Exception
      AddToDatabaseLog("Error accessing database GetDataRowsBySQL: " & vbCrLf & GetExceptionText(ex), "DB Error")
      sErr = ex.Message
    Finally
      If (Not bUsingIncomingConnection) Then
        If Not (conn Is Nothing) Then
          If (conn.State = ConnectionState.Open) Then conn.Close()
          conn.Dispose()
        End If
      End If
      If Not da Is Nothing Then da.Dispose()
      If Not cmd Is Nothing Then cmd.Dispose()
    End Try
    Return Nothing
  End Function

  Public Function GetDataRowsByCommand(ByVal cmd As DbCommand, Optional ByVal sConnectionString As String = "", _
                                     Optional ByVal conn As DbConnection = Nothing, _
                                     Optional ByRef sErr As String = "") As DataRowCollection



    Dim bUsingIncomingConnection As Boolean = True
    Dim da As DbDataAdapter = Nothing

    Try
      If (conn Is Nothing) Then
        Dim sConn As String = ""
        If (sConnectionString <> "") Then
          sConn = sConnectionString
        Else
          sConn = g_sConn
        End If

        bUsingIncomingConnection = False

        conn = NewDBConnection(sConn)
      End If
      If (cmd.Connection Is Nothing) Then
        cmd.Connection = conn
      End If
      da = NewDbDataAdapter(cmd)

      Dim dt As New DataTable
      da.Fill(dt)
      Return dt.Rows

    Catch e As Exception
      sErr = e.Message
      AddToDatabaseLog("GetDataRowsBySQL: " & GetExceptionText(e), "Error")
    Finally
      If (Not bUsingIncomingConnection) Then
        If Not (conn Is Nothing) Then
          If (conn.State = ConnectionState.Open) Then conn.Close()
          conn.Dispose()
        End If
      End If
      If Not da Is Nothing Then da.Dispose()
    End Try
    Return Nothing
  End Function



  Public Function PutListOfTablesIntoComboBox(ByRef cmb As System.Windows.Forms.ComboBox) As Boolean
    Dim dbConn As DbConnection = Nothing
    Dim cmd As DbCommand = Nothing
    Dim da As DbDataAdapter = Nothing
    Dim bRet As Boolean = True
    cmb.Items.Clear()

    Try
      dbConn = NewDBConnection(g_sConn)

      Dim dt As DataTable

      Try
        dbConn.Open()

        dt = dbConn.GetSchema("Tables")

        For Each dr As DataRow In dt.Rows
          cmb.Items.Add(dr.Item("TABLE_NAME"))
        Next

        dbConn.Close()
        dbConn.Dispose()
      Catch ex As Exception
        Debug.WriteLine("Exception Loading Tables:" & ex.Message)
        bRet = False
      Finally
        If dbConn.State = ConnectionState.Open Then
          dbConn.Close()
        End If
      End Try

    Catch ex As Exception
      MsgBox("Exception Loading Tables into Combo Box:" & ex.Message)
      bRet = False
    End Try
    Return bRet
  End Function

  '===============================================================================
  '    Name: GetDataTableBySQL
  ' Remarks: Returns Multiple DataTable with any SQL query
  '===============================================================================
  Public Function GetDataTableBySQL(ByVal sSQL As String, _
                                    Optional ByVal conn As DbConnection = Nothing, _
                                    Optional ByVal trans As DbTransaction = Nothing, _
                                    Optional ByRef sErr As String = "") As DataTable
    Dim bUseIncoming As Boolean = False
    Dim cmd As DbCommand = Nothing
    Dim da As DbDataAdapter = Nothing

    Try
      If (conn Is Nothing) Then
        conn = NewDBConnection(g_sConn)
      Else
        bUseIncoming = True
      End If
      cmd = NewDbCommand(sSQL, conn)
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If
      da = NewDbDataAdapter(cmd)

      Dim dt As New DataTable
      da.Fill(dt)
      Return dt

    Catch e As Exception
      sErr = e.Message
      AddToDatabaseLog("GetDataTableBySQL: " & GetExceptionText(e), "Error")
    Finally
      If (conn IsNot Nothing) AndAlso Not bUseIncoming Then
        If (conn.State = ConnectionState.Open) Then conn.Close()
        conn.Dispose()
      End If
      If Not da Is Nothing Then da.Dispose()
      If Not (cmd Is Nothing) Then cmd.Dispose()
    End Try
    Return Nothing
  End Function

  ''' <summary>
  ''' Returns a datatable containing the results of a sql query
  ''' list of parameters substitutes for parameters named @pname1, @pname2, @pname3, ect...
  ''' </summary>
  ''' <param name="sSQL"></param>
  ''' <param name="sErr"></param>
  ''' <param name="lstParameters"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetDataTableBySQL(ByVal sSQL As String, ByVal lstParameters As List(Of String), _
                                    Optional ByVal conn As DbConnection = Nothing, _
                                    Optional ByVal trans As DbTransaction = Nothing, Optional ByRef sErr As String = "") As DataTable
    Dim dt As New DataTable
    Dim bUseIncoming As Boolean = False
    Dim cmd As DbCommand = Nothing
    Dim da As DbDataAdapter = Nothing

    Try
      If (conn Is Nothing) Then
        conn = NewDBConnection(g_sConn)
      Else
        bUseIncoming = True
      End If
      cmd = NewDbCommand(sSQL, conn)
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If

      If lstParameters IsNot Nothing Then
        Dim i As Integer = 1
        For Each p As String In lstParameters

          Dim dbParam As DbParameter = NewDbParameter("@pname" & i.ToString, DbType.Object, p)
          cmd.Parameters.Add(dbParam)
          i += 1
        Next
      End If


      da = NewDbDataAdapter(cmd)
      da.Fill(dt)

    Catch ex As Exception
      sErr = ex.Message
      Debug.WriteLine("GetDataTableBySQL: " & sErr)
    Finally
      If Not da Is Nothing Then
        da.Dispose()
      End If
      If Not (cmd Is Nothing) Then
        cmd.Dispose()
      End If
    End Try

    Return dt
  End Function

  '===============================================================================
  '    Name: GetDataTableBySQL
  ' Remarks: Returns Multiple DataTable with any SQL query
  '===============================================================================
  Public Function GetDataTableBySQL(ByVal sSQL As String, _
                                    ByVal lstParams As List(Of DbParameter),
                                    Optional ByVal conn As DbConnection = Nothing, _
                                    Optional ByVal trans As DbTransaction = Nothing, _
                                    Optional ByRef sErr As String = "" _
                                    ) As DataTable
    Dim bUseIncoming As Boolean = False
    Dim cmd As DbCommand = Nothing
    Dim da As DbDataAdapter = Nothing

    Try
      If (conn Is Nothing) Then
        conn = NewDBConnection(g_sConn)
      Else
        bUseIncoming = True
      End If
      cmd = NewDbCommand(sSQL, conn)
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If

      If lstParams IsNot Nothing Then
        For Each p As DbParameter In lstParams
          cmd.Parameters.Add(p)
        Next
      End If

      da = NewDbDataAdapter(cmd)

      Dim dt As New DataTable
      da.Fill(dt)
      Return dt

    Catch e As Exception
      sErr = e.Message
      AddToDatabaseLog("GetDataTableBySQL: " & GetExceptionText(e), "Error")
    Finally
      If (conn IsNot Nothing) AndAlso Not bUseIncoming Then
        If (conn.State = ConnectionState.Open) Then conn.Close()
        conn.Dispose()
      End If
      If Not da Is Nothing Then da.Dispose()
      If Not (cmd Is Nothing) Then cmd.Dispose()
    End Try
    Return Nothing
  End Function

  '===============================================================================
  '    Name: GetDataTableBySQL
  ' Remarks: Returns Multiple DataTable with any SQL query
  '===============================================================================
  Public Function GetDataSetBySQL(ByVal sSQL As String, Optional ByRef sErr As String = "") As DataSet
    Dim conn As DbConnection = Nothing
    Dim cmd As DbCommand = Nothing
    Dim da As DbDataAdapter = Nothing

    Try
      conn = NewDBConnection(g_sConn)
      da = NewDbDataAdapter(sSQL, conn)

      Dim ds As New DataSet
      da.Fill(ds)
      Return ds

    Catch e As Exception
      sErr = e.Message
      AddToDatabaseLog("GetDataSetBySQL: " & GetExceptionText(e), "Error")
    Finally
      If Not (conn Is Nothing) Then
        If (conn.State = ConnectionState.Open) Then conn.Close()
        conn.Dispose()
      End If
      If Not da Is Nothing Then da.Dispose()
      If Not (cmd Is Nothing) Then cmd.Dispose()
    End Try
    Return Nothing
  End Function

  '===============================================================================
  ' Name:    GetFilteredDatatableFromDataSet
  ' Purpose: Used to filter a table in a dataset.
  '          Ex filter: "Color = 'F'"
  '===============================================================================
  Public Function GetFilteredDatatableFromDataSet(ByVal FilterExpression As String, ByVal ds As DataSet, ByVal sTableName As String) As DataTable
    Dim dt As DataTable = ds.Tables(sTableName)
    Dim dt2 As DataTable = dt.Clone
    Dim dr2 As DataRow() = dt.Select(FilterExpression)
    For Each row As DataRow In dr2
      dt2.ImportRow(row)
    Next

    Return dt2
  End Function

  '===============================================================================
  ' Name:    DoAnyAdminsExist
  ' Purpose: Used to determine if this is the first time the program has been used
  '          and database is empty.
  '===============================================================================
  Public Const SQL_DO_ADMINS_EXIST_XPE As String = "Select COUNT(users.id) as counter FROM " & _
              "users INNER JOIN roles ON users.role_id = roles.id WHERE roles.is_admin <> 0 LIMIT 1"

  Public Function DoAnyAdminsExist(Optional sSQL As String = SQL_DO_ADMINS_EXIST_XPE) As Boolean
    Dim iNumAdmins As Integer
    iNumAdmins = ExecuteScalarQuery(sSQL)
    Return (iNumAdmins > 0)
  End Function



  Public Function DBQueryToDataTable(ByVal sSQL As String, ByRef dt As DataTable, ByVal Conn As DbConnection, _
                                     Optional ByVal bAddIdPrimaryKeyConstraint As Boolean = True, _
                                     Optional ByRef sErr As String = "", _
                                     Optional ByRef bUpdatingDtBooleanFlag As Boolean = True) As Boolean

    Dim cmd As DbCommand = Nothing
    Dim adptr As DbDataAdapter = Nothing
    Dim bRet As Boolean = False

    ' If a global is monitoring datatable availability
    bUpdatingDtBooleanFlag = True


    Try

      adptr = NewDbDataAdapter(sSQL, Conn)
      cmd = NewDbCommand(sSQL, Conn)
      adptr.SelectCommand = cmd

      If Not (dt Is Nothing) Then dt.Dispose()
      dt = New DataTable
      adptr.Fill(dt)

      'Allow editing (readonly is set to true by default)
      For i As Integer = 0 To dt.Columns.Count - 1
        dt.Columns(i).ReadOnly = False
      Next

      'Add a primary key for easy lookups 
      If bAddIdPrimaryKeyConstraint Then
        Dim keys(0) As DataColumn
        keys(0) = dt.Columns("id")
        dt.PrimaryKey = keys
      End If

      bRet = True
    Catch ex As Exception
      sErr = "Error filling datatable from query: " & ex.Message.ToString & vbCrLf & "Query: " & sSQL
    Finally
      ' If a global is monitoring datatable availability
      bUpdatingDtBooleanFlag = False
    End Try

    Return bRet
  End Function


  Public Function DBQueryToDataTable(ByVal sSQL As String, ByRef dt As DataTable, ByVal sConn As String, _
                                     Optional ByVal bAddIdPrimaryKeyConstraint As Boolean = True, _
                                     Optional ByRef sErr As String = "", _
                                     Optional ByRef bUpdatingDtBooleanFlag As Boolean = True, _
                                     Optional ByVal conn As DbConnection = Nothing) As Boolean

    Dim bCloseConnection As Boolean = (conn Is Nothing)

    Dim cmd As DbCommand = Nothing
    Dim adptr As DbDataAdapter = Nothing
    Dim bRet As Boolean = False

    ' If a global is monitoring datatable availability
    bUpdatingDtBooleanFlag = True


    Try

      If (bCloseConnection) Then
        'Open DB connection
        conn = NewDBConnection(sConn)
        conn.Open()
      End If

      adptr = NewDbDataAdapter(sSQL, conn)
      'cmd = NewDbCommand(sSQL, conn)
      'adptr.SelectCommand = cmd

      If dt Is Nothing Then dt = New DataTable
      adptr.Fill(dt)
      adptr.Dispose()

      'Allow editing (readonly is set to true by default)
      For i As Integer = 0 To dt.Columns.Count - 1
        If dt.Columns(i).Expression Is Nothing Then dt.Columns(i).ReadOnly = False
      Next

      'Add a primary key for easy lookups 
      If bAddIdPrimaryKeyConstraint Then
        Dim keys(0) As DataColumn
        keys(0) = dt.Columns("id")
        dt.PrimaryKey = keys
      End If

      bRet = True
    Catch ex As Exception
      sErr = "Error filling datatable from query: " & ex.Message.ToString & vbCrLf & "Query: " & sSQL
      AddToDatabaseLog(sErr & vbCrLf & GetExceptionText(ex), "Error")
    Finally

      If (bCloseConnection) Then
        CloseCmdAndConn(cmd, conn)
      End If

      ' If a global is monitoring datatable availability
      bUpdatingDtBooleanFlag = False
    End Try

    Return bRet
  End Function


  Public Function DBQueryToDataSet(ByVal sSQL As String, ByRef ds As DataSet,
                                   ByVal sDataTableName As String, ByVal sConn As String, _
                                     Optional ByVal bAddIdPrimaryKeyConstraint As Boolean = True, _
                                     Optional ByRef sErr As String = "", _
                                     Optional ByRef bUpdatingDtBooleanFlag As Boolean = True) As Boolean

    Dim conn As DbConnection = Nothing
    Dim cmd As DbCommand = Nothing
    Dim adptr As DbDataAdapter = Nothing
    Dim bRet As Boolean = False

    ' If a global is monitoring datatable availability
    bUpdatingDtBooleanFlag = True


    Try
      'Open DB connection
      conn = NewDBConnection(sConn)
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

      ' If a global is monitoring datatable availability
      bUpdatingDtBooleanFlag = False
    End Try

    Return bRet
  End Function

  Public Function GetFieldFromDB(ByVal sSQL As String, ByRef lstParams As List(Of DbParameter), _
                                 ByVal sConn As String, ByVal sFieldToSelect As String, _
                                 Optional ByVal oNull As Object = INVALID_ID, _
                                 Optional ByVal conn As DbConnection = Nothing, _
                                 Optional ByVal trans As DbTransaction = Nothing) As Object

    Dim rdr As DbDataReader = Nothing
    Dim cmd As DbCommand = Nothing
    Dim oRet As Object = Nothing
    Dim bFound As Boolean = False
    Dim bUseIncomingConn As Boolean = (conn IsNot Nothing)
    Try

      If Not bUseIncomingConn Then
        conn = NewDBConnection(sConn)
        conn.Open()
      End If

      cmd = NewDbCommand(sSQL, conn)
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If
      For Each p As DbParameter In lstParams
        cmd.Parameters.Add(p)
      Next

      rdr = cmd.ExecuteReader()

      If rdr.HasRows Then
        While rdr.Read
          oRet = rdr.Item(sFieldToSelect)
          bFound = True
          Exit While
        End While
      End If
      rdr.Close()
    Catch e As Exception
      AddToDatabaseLog(GetExceptionText(e), "Error")
    Finally
      If Not bUseIncomingConn Then
        CloseCmdAndConn(cmd, conn)
      End If

    End Try

    If Not bFound OrElse oRet Is System.DBNull.Value Then oRet = oNull
    Return oRet
  End Function


  '===============================================================================
  ' Name:    GetFieldFromDB  
  ' Purpose:  Return sfield value from sTable using where filter
  '===============================================================================
  Public Function GetFieldFromDB(ByVal sTable As String, ByVal sWHERE As String, _
                                 ByVal sConn As String, ByVal sField As String, _
                                 Optional ByVal oNull As Object = INVALID_ID, _
                                 Optional ByVal conn As DbConnection = Nothing, _
                                 Optional ByVal trans As DbTransaction = Nothing) As Object

    Dim rdr As DbDataReader = Nothing
    Dim cmd As DbCommand = Nothing
    Dim sSQL As String = ""
    Dim oRet As Object = Nothing
    Dim bFound As Boolean = False
    Dim bUseIncomingConn As Boolean = (conn IsNot Nothing)
    Try
      sSQL = "Select " & sField & " As FieldValue FROM " & sTable & IIf(sWHERE <> "", " WHERE " & sWHERE, "")
      If Not bUseIncomingConn Then
        conn = NewDBConnection(sConn)
        conn.Open()
      End If

      cmd = NewDbCommand(sSQL, conn)
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If
      rdr = cmd.ExecuteReader()

      If rdr.HasRows Then
        While rdr.Read
          oRet = rdr.Item("FieldValue")
          bFound = True
          Exit While
        End While
      End If
      rdr.Close()
    Catch e As Exception
      AddToDatabaseLog(GetExceptionText(e), "Error")
    Finally
      If Not bUseIncomingConn Then
        CloseCmdAndConn(cmd, conn)
      End If

    End Try

    If Not bFound OrElse oRet Is System.DBNull.Value Then oRet = oNull
    Return oRet
  End Function

  '===============================================================================
  ' Name:    DoRecordsExist
  ' Purpose: Return true if any records are returned from executed SQL statement
  '===============================================================================
  Public Function DoRecordsExist(ByVal sSQL As String, _
                                 Optional ByVal oParamaters As List(Of DbParameter) = Nothing, _
                                 Optional ByVal conn As DbConnection = Nothing, _
                                 Optional ByVal trans As DbTransaction = Nothing, _
                                 Optional ByRef sErr As String = "") As Boolean
    Dim cmd As DbCommand = Nothing
    Dim bRecordsExist As Boolean
    Dim bIncomingConnection As Boolean = (conn IsNot Nothing)
    Try
      If (Not bIncomingConnection) Then

        conn = NewDBConnection(g_sConn)
        conn.Open()
      End If

      sSQL = "SELECT CASE WHEN EXISTS(" & sSQL & ") THEN 1 ELSE 0 END AS AnyData"

      cmd = NewDbCommand(sSQL, conn)
      If (oParamaters IsNot Nothing) Then
        For Each p As DbParameter In oParamaters
          cmd.Parameters.Add(p)
        Next
      End If
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If

      Dim res = cmd.ExecuteScalar()
      bRecordsExist = (res IsNot Nothing AndAlso res = 1)

    Catch e As Exception
      AddToDatabaseLog(GetExceptionText(e), "Error")
      sErr = ("Error looking for DB record: " & e.ToString & "   " & vbCrLf & sSQL)
    Finally
      If (Not bIncomingConnection) Then
        CloseCmdAndConn(cmd, conn)
      End If
    End Try

    Return bRecordsExist
  End Function
#Region "DataGridView Functions"

  Public Sub FillDataGridView(dgv As DataGridView, drc As DataRowCollection,
                              Optional bClearRows As Boolean = True,
                              Optional bClearCols As Boolean = True,
                              Optional sHideColumns As String = "id,ID")
    Dim i As Integer, j As Integer
    Dim lsHide As New List(Of String)(sHideColumns.Split(","c))

    If bClearRows Then dgv.Rows.Clear()
    If bClearCols Then dgv.Columns.Clear()

    dgv.BringToFront()
    dgv.Show()

    If drc Is Nothing Then Exit Sub

    Dim iRows As Integer = drc.Count
    If (iRows = 0) Then Exit Sub
    Dim iCols As Integer = drc(0).Table.Columns.Count
    If iCols < 1 Then Exit Sub

    ' Create an unbound DataGridView by declaring a column count.
    dgv.ColumnCount = iCols
    dgv.ColumnHeadersVisible = True
    dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize

    ' Set the column header names.
    For i = 0 To iCols - 1
      Dim sColName = drc(0).Table.Columns(i).ColumnName
      dgv.Columns(i).Name = sColName
      dgv.Columns(i).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
      If lsHide.Contains(sColName) Then dgv.Columns(i).Visible = False
      ' dt.Columns.Add(drc(0).Table.Columns(i).ColumnName)
    Next

    ' Populate the rows.
    For i = 0 To iRows - 1
      Try
        Dim sRow(0 To iCols - 1) As String
        For j = 0 To iCols - 1
          sRow(j) = NullToString(drc(i)(j))
        Next

        dgv.Rows.Add(sRow)
        ' dt.Rows.Add(drc(i))
      Catch ex As Exception
        MsgBox("FillDataGridView Error:" & ex.Message)
      End Try
    Next

  End Sub
#End Region


#Region "Combobox/Listbox Manipulation Functions"

  Public Sub FillComboOrListFromDbCmd(ByRef cbo As Object, ByRef cmd As DbCommand)
    'Debug.Print(Now.ToLongTimeString & " : " & cmd.CommandText)

    Dim rdr As DbDataReader = Nothing
    rdr = cmd.ExecuteReader
    cbo.visible = False
    cbo.BeginUpdate()

    cbo.Items.Clear()
    While rdr.Read
      cbo.Items.Add(NullToString(rdr.Item(0)))
    End While
    cbo.EndUpdate()
    cbo.visible = True
    rdr.Close()
  End Sub

  Public Sub FillComboOrListFromDbCmd(ByRef cbo As Object, ByRef sSQL As String, Optional ByVal conn As DbConnection = Nothing)
    'Debug.Print(Now.ToLongTimeString & " : " & sSQL)
    Dim dt As DataTable = GetDataTableBySQL(sSQL, conn)

    Try
      Dim aStr(dt.Rows.Count - 1) As ItemWithID
      Dim i As Integer = 0

      cbo.visible = False
      cbo.BeginUpdate()
      cbo.SuspendLayout()
      For Each row As DataRow In dt.Rows
        'colBValues.Add(row(0).ToString)
        aStr(i) = New ItemWithID(NullToString(row("name")), NullToInteger(row("id")))
        i += 1
      Next

      cbo.Items.AddRange(aStr)

    Catch ex As Exception
      AddToDatabaseLog("Error in FillComboOrListFromDbCmd:" & GetExceptionText(ex), "Error")
    Finally
      cbo.ResumeLayout()
      cbo.EndUpdate()
      cbo.visible = True
    End Try

  End Sub
  'This function requires an ID field in the SQL query
  ''' <summary>
  ''' Returns combo box filled from query
  ''' </summary>
  ''' <param name="cbo"></param>
  ''' <param name="sSQL"></param>
  ''' <param name="sConn"></param>
  ''' <param name="id_name"></param>
  ''' <param name="display_text_name"></param>
  ''' <param name="sFirstItem"></param>
  ''' <param name="sSecondItem"></param>
  ''' <param name="sItemToSelect"></param>
  ''' <param name="bForceSelection"></param>
  ''' <param name="bIsListIdField"></param>
  ''' <param name="bAllowBlankEntries"></param>
  ''' <param name="sErr"></param>
  ''' <remarks>If using a datagridviewcombobox, set the display member and value member 
  ''' Such as:  
  ''' FillComboBoxFromDB(cboColumn, "SELECT name, id FROM badge_types", g_sConn, "id", "name")
  ''' cboColumn.ValueMember = "id"
  ''' cboColumn.DisplayMember = "name"
  ''' 
  ''' </remarks>
  Public Sub FillComboBoxFromDB(ByRef cbo As Object, ByVal sSQL As String, _
                              ByVal sConn As String, _
                              ByVal id_name As String, _
                              ByVal display_text_name As String, _
                              Optional ByVal sFirstItem As String = "DO_NOT_INCLUDE", _
                              Optional ByVal sSecondItem As String = "DO_NOT_INCLUDE", _
                              Optional ByVal sItemToSelect As String = "", _
                              Optional ByVal bForceSelection As Boolean = False,
                              Optional ByVal bIsListIdField As Boolean = False, _
                              Optional ByVal bAllowBlankEntries As Boolean = False, _
                              Optional ByRef sErr As String = "")


    Dim bl As New BindingList(Of ItemWithID)

    Dim conn As DbConnection = Nothing
    Dim rdr As DbDataReader = Nothing
    Dim cmd As DbCommand = Nothing
    Dim tc() As Char = {" ", "{", "}", "(", ")", "[", "]", "?"}

    cbo.Items.Clear()

    Try
      If sFirstItem <> "DO_NOT_INCLUDE" Then
        cbo.Items.Add(New ItemWithID(sFirstItem, INVALID_ID))
      End If
      If sSecondItem <> "DO_NOT_INCLUDE" Then
        cbo.Items.Add(New ItemWithID(sSecondItem, INVALID_ID))
      End If

      conn = NewDBConnection(sConn)
      conn.Open()
      cmd = NewDbCommand(sSQL, conn)
      rdr = cmd.ExecuteReader()

      If rdr.HasRows Then
        While rdr.Read
          If (rdr.Item(display_text_name).ToString.Trim(tc) <> "") OrElse bAllowBlankEntries Then
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
      AddToDatabaseLog(GetExceptionText(e), "Error")
      sErr = "Failed to Fill combo box: " & e.Message
      Debug.Print("Error loading combobox from db: " & e.Message)
    Finally
      CloseCmdAndConn(cmd, conn)
    End Try
  End Sub

  Public Sub FillComboBoxFromDB(ByVal cbo As Object, ByVal sSQL As String,
                          Optional ByVal sItemToSelect As String = "",
                          Optional ByVal bForceSelection As Boolean = False,
                          Optional ByVal iItemIDToSelect As Integer = INVALID_ID,
                          Optional ByVal bHasIdField As Boolean = True,
                          Optional ByVal sFieldToSelect As String = "name",
                          Optional ByVal conn As DbConnection = Nothing,
                          Optional ByVal trans As DbTransaction = Nothing,
                          Optional ByVal bHasListIdField As Boolean = False,
                          Optional ByVal bAllowBlankEntries As Boolean = False,
                          Optional ByVal sFirstItem As String = "DO_NOT_INCLUDE",
                          Optional ByVal sSecondItem As String = "DO_NOT_INCLUDE")

    Dim rdr As DbDataReader = Nothing
    Dim cmd As DbCommand = Nothing
    cbo.BeginUpdate()
    cbo.Items.Clear()

    If sFirstItem <> "DO_NOT_INCLUDE" Then
      cbo.Items.Add(New ItemWithID(sFirstItem, INVALID_ID))
    End If

    If sSecondItem <> "DO_NOT_INCLUDE" Then
      cbo.Items.Add(New ItemWithID(sSecondItem, -2))
    End If

    Dim bUseIncomingConn As Boolean = (conn IsNot Nothing)

    If Not bUseIncomingConn Then
      conn = NewDBConnection(g_sConn)
      conn.Open()
    End If

    cmd = NewDbCommand(sSQL, conn)
    rdr = cmd.ExecuteReader()

    If rdr.HasRows Then
      While rdr.Read
        Try
          If Not IsDBNull(rdr.Item(sFieldToSelect)) Then
            If bHasListIdField Then
              cbo.Items.Add(New ListItemWithID(rdr.Item(sFieldToSelect), rdr.Item("id")))
            ElseIf bHasIdField Then
              cbo.Items.Add(New ItemWithID(rdr.Item(sFieldToSelect), rdr.Item("id")))
            Else
              cbo.Items.Add(rdr.Item(sFieldToSelect))
            End If
          End If
        Catch ex As Exception
          Debug.Print(ex.Message)
        End Try
      End While
      rdr.Close()
    End If

    If sItemToSelect <> "" Or iItemIDToSelect <> INVALID_ID Then
      Dim iTempIndex As Integer = -1
      For iTempIndex = 0 To cbo.items.count - 1
        If (cbo.items(iTempIndex).ToString = sItemToSelect) Then
          'cbo.Text = sItemToSelect
          Exit For
        End If

        If bHasIdField Then
          Dim iwi As ItemWithID
          If (TypeOf cbo.Items(iTempIndex) Is ItemWithID) Then
            iwi = cbo.Items(iTempIndex)
            If (iwi.ID = iItemIDToSelect) Then
              'cbo.selectedindex = iTempIndex
              Exit For
            End If
          End If
        End If
      Next
      If (iTempIndex <> -1) And iTempIndex < cbo.Items.Count Then
        cbo.SelectedIndex = iTempIndex
      End If

    Else
      If (bForceSelection And cbo.Items.Count > 0) Then cbo.SelectedIndex = 0
    End If

    If Not bUseIncomingConn Then
      Call CloseCmdAndConn(cmd, conn)
    End If
    cbo.EndUpdate()

  End Sub

  Public Function SelectDistinct(ByVal SourceTable As DataTable, ByVal ParamArray FieldNames() As String) As DataTable
    Dim lastValues() As Object
    Dim newTable As DataTable

    If FieldNames Is Nothing OrElse FieldNames.Length = 0 Then
      Throw New ArgumentNullException("FieldNames")
    End If

    lastValues = New Object(FieldNames.Length - 1) {}
    newTable = New DataTable

    For Each field As String In FieldNames
      newTable.Columns.Add(field, SourceTable.Columns(field).DataType)
    Next

    For Each Row As DataRow In SourceTable.Select("", String.Join(", ", FieldNames))
      If Not fieldValuesAreEqual(lastValues, Row, FieldNames) Then
        newTable.Rows.Add(createRowClone(Row, newTable.NewRow(), FieldNames))

        setLastValues(lastValues, Row, FieldNames)
      End If
    Next

    Return newTable
  End Function

  Private Function fieldValuesAreEqual(ByVal lastValues() As Object, ByVal currentRow As DataRow, ByVal fieldNames() As String) As Boolean
    Dim areEqual As Boolean = True

    For i As Integer = 0 To fieldNames.Length - 1
      If lastValues(i) Is Nothing OrElse Not lastValues(i).Equals(currentRow(fieldNames(i))) Then
        areEqual = False
        Exit For
      End If
    Next

    Return areEqual
  End Function

  Private Function createRowClone(ByVal sourceRow As DataRow, ByVal newRow As DataRow, ByVal fieldNames() As String) As DataRow
    For Each field As String In fieldNames
      newRow(field) = sourceRow(field)
    Next

    Return newRow
  End Function

  Private Sub setLastValues(ByVal lastValues() As Object, ByVal sourceRow As DataRow, ByVal fieldNames() As String)
    For i As Integer = 0 To fieldNames.Length - 1
      lastValues(i) = sourceRow(fieldNames(i))
    Next
  End Sub

#Region "Code for Generic FillTreeViewFromDT"


  '' needs to be made generic
  'Public Sub FillTreeViewFromDT(ByVal tv As TreeView, ByVal dt As DataTable, ByVal sFilter As String, _
  '                            ByVal sSort As String, Optional ByVal id_name As String = "id", _
  '                            Optional ByVal display_text_name As String = "name", Optional ByVal ItemToSelect As ItemWithID = Nothing, _
  '                            Optional ByVal bForceSelection As Boolean = False)
  '  tv.Nodes.Clear()

  '  Dim dtCatalogItemNames As DataTable = SelectDistinct(dt, {"catalog_item_id", "description"})
  '  For Each dr As DataRow In dtCatalogItemNames.Rows
  '    Dim sNode As String = dr.Item("description")
  '    Dim nodeCatalogItem As New TreeNode(sNode)
  '    nodeCatalogItem.Tag = dr.Item("catalog_item_id")
  '    '
  '    nodeCatalogItem.ImageIndex = 7
  '    nodeCatalogItem.SelectedImageIndex = 8
  '    Dim adrMatchedCatalogItems As DataRow() = dt.Select("catalog_item_id =" & dr.Item("catalog_item_id"))
  '    For Each item In adrMatchedCatalogItems
  '      Dim iid As New ItemWithID(item("stock_no"), item("id"))
  '      Dim nodeItem As New TreeNode(item("stock_no"))
  '      nodeItem.Tag = iid
  '      nodeItem.ImageIndex = 9
  '      nodeItem.SelectedImageIndex = 10

  '      nodeCatalogItem.Nodes.Add(nodeItem) 'iid)

  '    Next
  '    tv.Nodes.Add(nodeCatalogItem)
  '  Next

  'End Sub

#End Region

  Public Function FillItemWithIdListBoxFromDT(ByVal cbo As Object, ByVal dt As DataTable, ByVal sFilter As String, _
                                        ByVal sSort As String, Optional ByVal id_name As String = "id", _
                                        Optional ByVal display_text_name As String = "name", Optional ByVal ItemToSelect As ItemWithID = Nothing, _
                                        Optional ByVal bForceSelection As Boolean = False) As Boolean

    FillComboBoxFromDT(cbo, dt, sFilter, sSort, id_name, display_text_name, "")
    Dim bSelectedFound As Boolean = False
    Try
      If ItemToSelect IsNot Nothing Then
        For i As Integer = 0 To cbo.items.count - 1
          Dim item As ItemWithID = cbo.items(i)
          If (item.ID = ItemToSelect.ID) Then
            cbo.SelectedItem = item
            bSelectedFound = True
            Exit For
          End If
        Next
      End If

      If (bForceSelection AndAlso (cbo.SelectedItem.count = 0) AndAlso (cbo.Items.Count > 0)) Then cbo.SelectedIndex = 0

    Catch e As Exception
      AddToDatabaseLog(GetExceptionText(e), "Error")
      Debug.Print("Error loading listbox from datatable: " & e.Message)
    End Try
    Return bSelectedFound
  End Function

  Public Sub FillComboBoxFromDT(ByVal cbo As Object, ByVal dt As DataTable, ByVal sFilter As String, _
                                ByVal sSort As String, Optional ByVal id_name As String = "id", _
                                Optional ByVal display_text_name As String = "name", Optional ByVal sItemToSelect As String = "", _
                                Optional ByVal bForceSelection As Boolean = False, _
                                Optional ByVal iOffset As Integer = 0, Optional ByVal iLimit As Integer = 0, _
                                Optional ByVal sTooltipField As String = "")
    'Debug.WriteLine("FillComboBoxFromDT")

    'Dim cbo2 As ComboBox
    cbo.SuspendLayout()
    cbo.Items.Clear()

    Try
      Dim dr() As DataRow = dt.Select(sFilter, sSort)
      Dim iCnt As Integer = 0
      Dim iid As ItemWithID

      'Loop through each row and add id and name to the combobox
      For i As Integer = iOffset To dr.Length - 1
        iid = New ItemWithID(dr(i).Item(display_text_name).ToString, NullToInteger(dr(i).Item(id_name)))
        cbo.Items.Add(iid)

        'Increment counter if we want to exit after reaching a max number of items
        If iLimit > 0 Then
          iCnt += 1

          'Have we reached our limit of items?
          If (iCnt >= iLimit) Then Exit For
        End If
      Next

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
      AddToDatabaseLog(GetExceptionText(e), "Error")
      Debug.Print("Error loading combobox from datatable: " & e.Message)
    Finally
      cbo.ResumeLayout()
    End Try
  End Sub

  Public Sub FillBindingListFromDT(ByRef lst As BindingList(Of ItemWithID), ByVal dt As DataTable, ByVal sFilter As String, _
                              ByVal sSort As String, Optional ByVal id_name As String = "id", _
                              Optional ByVal display_text_name As String = "name", Optional ByVal sItemToSelect As String = "", _
                              Optional ByVal bForceSelection As Boolean = False, _
                              Optional ByVal iOffset As Integer = 0, Optional ByVal iLimit As Integer = 0, _
                              Optional ByVal sTooltipField As String = "")
    'Debug.WriteLine("FillComboBoxFromDT")

    'Dim cbo2 As ComboBox

    Try
      Dim dr() As DataRow = dt.Select(sFilter, sSort)
      Dim iCnt As Integer = 0
      Dim iid As ItemWithID

      'Loop through each row and add id and name to the combobox
      For i As Integer = iOffset To dr.Length - 1
        iid = New ItemWithID(dr(i).Item(display_text_name).ToString, NullToInteger(dr(i).Item(id_name)))
        lst.Add(iid)

        'Increment counter if we want to exit after reaching a max number of items
        If iLimit > 0 Then
          iCnt += 1

          'Have we reached our limit of items?
          If (iCnt >= iLimit) Then Exit For
        End If
      Next


    Catch e As Exception
      AddToDatabaseLog(GetExceptionText(e), "Error")
      Debug.Print("Error loading combobox from datatable: " & e.Message)
    Finally
    End Try
  End Sub

  Public Function DeleteRecordWithFlag(ByVal sTable As String, ByVal sWHERE As String, _
                                       Optional ByRef sErr As String = "", _
                                       Optional ByRef lRowsAffected As Integer = 0) As Boolean
    Dim bRet As Boolean = False
    Dim sSQL As String = "UPDATE " & sTable & " SET deleted_at=NOW(), updated_at=NOW() " & sWHERE
    Try
      lRowsAffected = ExecuteNonQuery(sSQL, sErr)
      bRet = True

    Catch ex As Exception
      sErr = "Error deleting record: " & ex.Message.ToString
    End Try

    Return bRet
  End Function

  Public Function DeleteRecord(ByVal sTable As String, ByVal sWHERE As String, _
                                Optional ByRef sErr As String = "",
                                Optional ByRef iRowsAffected As Integer = -1, _
                                Optional ByVal conn As DbConnection = Nothing, _
                                Optional ByVal trans As DbTransaction = Nothing) As Boolean
    Dim bRet As Boolean = False
    Dim cmd As DbCommand = Nothing
    Dim sSQL As String = "DELETE FROM " & sTable & " " & sWHERE
    Dim bUseIncomingConn As Boolean = (conn IsNot Nothing)


    Try
      If (Not bUseIncomingConn) Then
        conn = NewDBConnection(g_sConn)
        conn.Open()
      End If

      cmd = NewDbCommand(sSQL, conn)
      If (trans IsNot Nothing) Then
        cmd.Transaction = trans
      End If
      iRowsAffected = cmd.ExecuteNonQuery

      bRet = True

    Catch ex As Exception
      sErr = "Error deleting record: " & ex.Message.ToString
    Finally
      If Not bUseIncomingConn Then
        CloseCmdAndConn(cmd, conn)
      End If
    End Try

    Return bRet
  End Function

  '===============================================================================
  ' Name:    GetFieldFromDTRecord (DataTable)
  ' Purpose: Return field value from passed in DataTable 
  ' Remark:  -1 is used instead of INVALID_ID
  '===============================================================================
  Public Function GetFieldFromDTRecord(ByVal dt As DataTable, ByVal sFilter As String, _
                     ByVal sField As String, Optional ByVal oNull As Object = -1) As Object

    Dim bFound As Boolean = False
    Dim oRet As Object = Nothing

    Try
      Dim row() As DataRow = dt.Select(sFilter)

      If row.Length > 0 Then
        'Returns the value of the first record
        oRet = row(0).Item(sField)
        bFound = True
      End If

    Catch ex As Exception
      AddToDatabaseLog("GetFieldFromDataTableRecord(DataTable) : " & vbCrLf & GetExceptionText(ex), "Error")
    End Try

    If Not bFound OrElse oRet Is System.DBNull.Value Then oRet = oNull
    Return oRet
  End Function

  '===============================================================================
  ' Name:    GetRowCountFromDT (DataTable)
  ' Purpose: Return row count from passed in DataTable and filter
  '===============================================================================
  Public Function GetRowCountFromDT(ByVal dt As DataTable, ByVal sFilter As String) As Integer
    Dim iRet As Integer = 0

    Try
      Dim row() As DataRow = dt.Select(sFilter)
      iRet = row.Length
    Catch ex As Exception
      AddToDatabaseLog("GetRowCountFromDT(DataTable) : " & vbCrLf & GetExceptionText(ex), "Error")
    End Try

    Return iRet
  End Function

#End Region '



  Public Function GetTableFieldsArray(ByVal sTable As String, ByVal conn As DbConnection, _
                                    ByVal trans As DbTransaction) As String()
    Dim lsFields As List(Of String) = GetTableFieldsList(sTable, conn, trans)
    Return convertListToArray(lsFields)
  End Function

  Public Function GetTableFieldsList(ByVal sTable As String, ByVal conn As DbConnection, _
                                    ByVal trans As DbTransaction) As List(Of String)

    ' Variable Declaration
    Dim rdr As DbDataReader = Nothing
    Dim cmd As DbCommand = Nothing
    Dim dt As New DataTable
    Dim lsFields As List(Of String) = New List(Of String)


    Try

      dt = GetDataTableBySQL("SELECT * FROM " & sTable & " LIMIT 0;", conn, trans)
      If dt Is Nothing Then Return lsFields
      For Each col As DataColumn In dt.Columns
        lsFields.Add(col.ColumnName)
      Next
    Catch ex As Exception
      Debug.Print("GetTableFields: " & ex.Message)

    End Try

    Try
    Catch ex As Exception

    End Try

    Return lsFields

  End Function

  Public Function GetAllDatabaseTables() As List(Of String)
    Dim asTableNames As New List(Of String)
    Dim ls = New List(Of String)
    Dim bRet As Boolean = GetAllDatabaseTables(asTableNames)
    Return asTableNames
  End Function

  Public Function GetAllDatabaseTables(ByRef lsTableNames As List(Of String)) As Boolean

    Dim bRet As Boolean
    Dim conn As DbConnection = Nothing
    Dim dtUserTables As DataTable = Nothing

    '' create a restrictions array to select specific rows
    '' Tables collection has 4 restrictions:
    '' null values indicate no restriction
    Dim restrictions(3) As String
    restrictions(0) = Nothing ' table_catalog
    restrictions(1) = Nothing ' table_schema
    restrictions(2) = Nothing ' table_name
    restrictions(3) = "table" ' table_type

    lsTableNames = New List(Of String)

    Try
      conn = NewDBConnection(g_sConn)
      conn.Open()
      dtUserTables = conn.GetSchema("Tables")

      For Each dr As DataRow In dtUserTables.Rows
        If dr.Item(3).ToString = "table" Then
          lsTableNames.Add(dr.Item(2).ToString)
        End If
      Next

      bRet = True

    Catch ex As Exception
      bRet = False
    End Try

    If conn IsNot Nothing Then
      conn.Close()
    End If

    Return bRet

  End Function

  Public Function GetTableSchema(ByVal sTable As String, ByVal conn As DbConnection) As String
    Dim sSQLCheck As String = ""
    Dim iType As DB_TYPES = g_iConnType
    If (conn IsNot Nothing) Then
      If (TypeOf (conn) Is SqlClient.SqlConnection) Then
        iType = DB_TYPES.SQLSERVER
      ElseIf (TypeOf (conn) Is SQLiteConnection) Then
        iType = DB_TYPES.SQLITE
      Else
        iType = DB_TYPES.MYSQL
      End If
    End If
    If (iType = DB_TYPES.SQLITE) Then
      sSQLCheck = "SELECT sql FROM " & _
                  "(SELECT * FROM sqlite_master UNION ALL " & _
                  " SELECT * FROM sqlite_temp_master) " & _
                  "WHERE tbl_name = '" & sTable & "' " & _
                  "AND type!='meta' AND sql NOT NULL AND name NOT LIKE 'sqlite_%' " & _
                  "ORDER BY substr(type,2,1), name "

    Else
      sSQLCheck = "show create table " & sTable
    End If
    Dim rows As DataRowCollection = GetDataRowsBySQL(sSQLCheck, conn)
    If Not rows Is Nothing Then
      If rows.Count > 0 Then
        If (g_iConnType = DB_TYPES.SQLITE) Then
          Return rows(0).Item("sql")
        ElseIf (g_iConnType = DB_TYPES.ODBC) Then
          If (rows(0).Table.Columns.IndexOf("create table") >= 0) Then
            Dim sRow As String = rows(0).Item("create table").ToString.Replace(vbCr, "").Replace(vbLf, "")

            Return sRow


          ElseIf (rows(0).Table.Columns.IndexOf("create view") >= 0) Then
            Return rows(0).Item("create view").ToString.Replace(vbCr, "").Replace(vbLf, "")
          End If

        End If

      End If
    End If
    Return ""
  End Function

  Public Function BuildSQLServerConnectionString(sSQLServerName As String, _
                                                    sSQLServerInstance As String, _
                                                    sSQLServerPort As String, _
                                                    sSQLServerDatabaseName As String, _
                                          bWindowsAuthentication As String, _
                                          sSQLServerUserName As String, _
                                          sSQLServerPassword As String) As String
    Dim sSQLServerConnString As String = ""

    'Data Source with Instance/Port
    If (sSQLServerName = "") Then
      sSQLServerConnString = "Data Source=(local)"
    Else
      sSQLServerConnString = "Data Source=" & sSQLServerName & ""
    End If
    If (Not String.IsNullOrEmpty(sSQLServerInstance)) Then
      sSQLServerConnString &= "\" & sSQLServerInstance
    End If
    If (Not String.IsNullOrEmpty(sSQLServerPort)) Then
      sSQLServerConnString &= "," & sSQLServerPort
    End If
    sSQLServerConnString &= ";"

    If Not String.IsNullOrEmpty(sSQLServerDatabaseName) Then
      sSQLServerConnString &= "Database=" & sSQLServerDatabaseName & ";"
    End If

    'Windows Authentication or UID/Password
    If (bWindowsAuthentication) Then
      sSQLServerConnString &= "Integrated Security=True;"
    Else
      sSQLServerConnString &= String.Format("User ID={0};Password={1};", sSQLServerUserName, sSQLServerPassword)
    End If
    Return sSQLServerConnString
  End Function

  Public Function CreateSQLServerDatabaseFromSQLite(sSQLServerName As String, _
                                                    sSQLServerInstance As String, _
                                                    sSQLServerPort As String, _
                                                    sSQLServerDatabaseName As String, _
                                          bWindowsAuthentication As String, _
                                          sSQLServerUserName As String, _
                                          sSQLServerPassword As String, _
                                          ByRef sErr As String) As Boolean
    Dim bRet As Boolean = True
    Dim sSQLiteConnection As String = "Data Source=:memory:;"
    'do this from an in memory database using the g_htTableCreateStatements
    Dim connSQLite As DbConnection = NewDBConnection(sSQLiteConnection, DB_TYPES.SQLITE)
    Dim connSqlServer As New SqlClient.SqlConnection()

    Dim sSQLServerConnString As String = BuildSQLServerConnectionString(sSQLServerName, sSQLServerInstance, sSQLServerPort, "", bWindowsAuthentication, sSQLServerUserName, sSQLServerPassword)
    AddToDatabaseLog("SQL Server Conn String: " & sSQLServerConnString, "Info")
    connSqlServer.ConnectionString = sSQLServerConnString
    Try
      connSqlServer.Open()
    Catch ex As Exception
      sErr = "Error with SQL Server Connection: " & ex.Message
      bRet = False
    Finally
      If Not bRet Then
        If connSqlServer IsNot Nothing Then
          'likely won't happen....
          If connSqlServer.State = ConnectionState.Open Then
            connSqlServer.Close()
          End If
          connSqlServer.Dispose()
          connSqlServer = Nothing
        End If

      End If
    End Try

    'Check if the SQL Server Database Exists
    Dim dr As DataRowCollection = GetDataRowsBySQL("SELECT db_id('" & sSQLServerDatabaseName & "');", connSqlServer, , sErr)
    Dim bCreateDatabase As Boolean = False
    If dr Is Nothing Then
      bRet = False
      sErr = "Could not query for existing database: " & sErr
      bCreateDatabase = True
    ElseIf dr.Count = 0 Then
      bCreateDatabase = True

    ElseIf dr.Count = 1 Then
      'Database exists, proceed with creation
      bCreateDatabase = (dr(0)(0) Is DBNull.Value)
    End If
    If bCreateDatabase Then
      'Database does not exist, try to create it
      Dim iRet As Integer = -1
      Try
        iRet = ExecuteNonQuery("CREATE DATABASE " & sSQLServerDatabaseName & ";", sErr, connSqlServer)
        bRet = True
      Catch ex As Exception
        sErr = ex.Message
        bRet = False
      End Try

    End If

    'Exit if we failed in creation
    If Not bRet Then Return False

    Try
      connSQLite.Open()
      ExecuteNonQuery("USE [" & sSQLServerDatabaseName & "]", sErr, connSqlServer)
      'Create all the tables
      For Each sTableName As String In g_htTableCreateStatements.Keys
        If (CreateTableIfNecessary(sTableName, sErr, connSqlServer, Nothing, Nothing, connSQLite) = False) Then
          Return False
        End If


      Next

    Catch ex As Exception
      sErr = ex.Message
      bRet = False
    Finally
      If connSQLite IsNot Nothing Then
        connSQLite.Close()
      End If
      connSQLite.Dispose()
      If connSqlServer IsNot Nothing Then
        'likely won't happen....
        If connSqlServer.State = ConnectionState.Open Then
          connSqlServer.Close()
        End If
        connSqlServer.Dispose()
        connSqlServer = Nothing
      End If
    End Try


    Return bRet
  End Function

  '1: Create all SQL Server Table Statements
  '2: Run all create statements
  '3: Use cStartupImport to AutoMap without external_id as primary key and import all data 
  '4: Go through and check that all tables and columns are the same!!!
  'To get the list of tables:
  'SELECT name FROM my_db.sqlite_master WHERE type='table';
  Public Function BuildCreateTableStatementForSQLServerFromSQLite( _
                      sTableName As String, conn As DbConnection, _
                      ByRef sErr As String, ByRef sOutputCreateStatement As String) As Boolean

    Dim bRet As Boolean = True
    Try

      Dim drColumns As DataRowCollection = GetDataRowsBySQL("PRAGMA table_info(" & sTableName & ");", conn, , sErr)
      If drColumns Is Nothing Then
        Return False
      End If

      Dim sColumnsInfo As String = ""
      Dim lstPKs As New List(Of String)
      'Go through all the columns and build our columns info for table creation
      For Each dr As DataRow In drColumns
        Dim sCol As String = ""
        Dim iColID As Integer = NullToInteger(dr("cid"))
        Dim iIsPrimaryKey As Integer = NullToInteger(dr("pk"))
        Dim sColName As String = NullToString(dr("name"))
        Dim sColType As String = NullToString(dr("type"))
        Dim iColNotNull As Integer = NullToInteger(dr("notnull"))
        Dim sDefaultvalue As String = NullToString(dr("dflt_value"))

        If (iIsPrimaryKey <> 0) Then
          lstPKs.Add(sColName)
        End If
        sCol = "[" + sColName + "] "
        Dim bAddSize As Boolean = False
        sColType = sColType.ToUpper()
        If (sColType.StartsWith("INTEGER(1)")) Or (sColType.StartsWith("BOOLEAN")) Then
          sCol &= "[bit]"
        ElseIf (sColType.StartsWith("INTEGER")) Then
          sCol &= "[int]"
          If iIsPrimaryKey <> 0 Then
            sCol &= " IDENTITY(1,1) "
          End If
        ElseIf (sColType.StartsWith("TEXT")) Then
          sCol &= "[varchar](max)"
        ElseIf (sColType.StartsWith("VARCHAR")) Then
          bAddSize = True
          sCol &= "[varchar]"
        ElseIf (sColType.StartsWith("TICKS")) Then
          sCol &= "[datetime]"
        ElseIf (sColType.StartsWith("DOUBLE")) Then
          sCol &= "[float]"
        ElseIf (sColType.StartsWith("BLOB")) Or (sColType.StartsWith("LONGVARBINARY")) Then
          'sCol &= "[varbinary(max)]"
          sCol &= "[image]"
        ElseIf (sColType.StartsWith("VARBINARY")) Then
          sCol &= "[varbinary](max)"
        Else
          bRet = False
        End If

        If bAddSize And (sColType.Contains("(")) Then
          sCol &= sColType.Substring(sColType.IndexOf("("))
        End If

        If (iColNotNull = 0) And (iIsPrimaryKey = 0) Then
          sCol &= " NULL "
        Else
          sCol &= " NOT NULL "
        End If

        If (Not String.IsNullOrEmpty(sDefaultvalue)) Then
          sCol &= " DEFAULT(" & sDefaultvalue & ") "
        End If

        sCol = sCol & ","
        If bRet = False Then
          sErr = "Error on Column: " & sColName & " " & sColType
        End If
        sColumnsInfo &= sCol
      Next


      sOutputCreateStatement = "CREATE TABLE "

      sOutputCreateStatement &= "[" & sTableName & "] " & _
        " (" & sColumnsInfo & "" & _
      "CONSTRAINT [PK_" & sTableName & "] PRIMARY KEY CLUSTERED " & _
      "("
      For Each sPKField As String In lstPKs
        sOutputCreateStatement &= "[" & sPKField & "] ASC,"
      Next
      sOutputCreateStatement = sOutputCreateStatement.TrimEnd(",")
      sOutputCreateStatement &= ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY];"
      'sOutputCreateStatement &= vbCrLf & "GO;" & vbCrLf
    Catch ex As Exception
      sErr = ex.Message
      bRet = False
    End Try

    Return bRet
  End Function

  Public Function UpdateOrCreateTable(ByVal sTableName As String, _
                                      ByVal sCreateSQL As String, _
                                      ByVal conn As DbConnection, _
                                      ByVal trans As DbTransaction,
                                      ByRef sErr As String,
                                      Optional dicRenameList As Dictionary(Of String, String) = Nothing) As Boolean

    'dicRenameList is a special list of new,old field name matches, so fields can be renamed easily without losing data

    'Dim bConnExisted As Boolean = False
    Dim bUsingIncomingConnection As Boolean = True
    Try
      If (conn Is Nothing) Then
        bUsingIncomingConnection = False
        conn = NewDBConnection(g_sConn)
        conn.Open()
      End If


      Dim sBackupName As String = sTableName & "_backup_" & Format(Now, "yyyy_MM_dd_HH_mm_ss")
      Dim sNewTableName As String = sTableName & "_new_" & Format(Now, "yyyy_MM_dd_HH_mm_ss")
      'Create a backup of the table
      Dim ilRet As Integer = ExecuteNonQuery("create table " & sBackupName & " as select * FROM " & sTableName, sErr, conn, trans)

      If (ExecuteNonQuery("CREATE TABLE " & sNewTableName & " " & sCreateSQL, sErr, conn, trans) < 0) Then Return False

      'Dim sSelectList As String = ""
      'conn.Open()
      Dim dt_backup As New DataTable
      Dim da As DbDataAdapter = NewDbDataAdapter("SELECT * FROM " & sBackupName & " LIMIT 1", conn)
      da.Fill(dt_backup)

      Dim dt_newtable As New DataTable
      Dim da2 As DbDataAdapter = NewDbDataAdapter("SELECT * FROM " & sNewTableName & " LIMIT 1", conn)
      da2.Fill(dt_newtable)
      'There are 3 cases
      '1) We removed a column from the table(backup will have more columns than new table)
      '2) We changed columns in the table
      '3) We added a column to the table (backup will have fewer columns than new table)
      '4) columns can be renamed, now, too, preserving data

      Dim arSelectList As New List(Of String)
      'have to search in same order we're adding to select list below, for cases where column was just moved
      'otherwise iColIndex below won't correspond correctly
      For Each new_col As DataColumn In dt_newtable.Columns
        Dim bExists As Boolean = False
        For Each col As DataColumn In dt_backup.Columns
          'If the column doesn't exist in the new table, don't add it
          If (new_col.ColumnName = col.ColumnName) Then
            bExists = True
            Exit For
          End If
        Next
        If (bExists) Then
          arSelectList.Add(new_col.ColumnName & ",")
        End If
      Next

      'In addition, we want to go through all the new columns and make sure they were added
      'We will just add a NULL in there......
      Dim iColIndex As Integer = 0
      Dim sFieldOrderList As String = ""
      For Each new_col As DataColumn In dt_newtable.Columns
        '   If new_col is an id column, the insert will add it automatically - skip!
        If new_col.ColumnName = "id" And Not dt_backup.Columns.Contains("id") Then Continue For
        'If our new column name isn't in the list, then add it
        If Not arSelectList.Contains(new_col.ColumnName & ",") Then
          If dicRenameList IsNot Nothing AndAlso dicRenameList.ContainsKey(new_col.ColumnName) Then
            arSelectList.Insert(iColIndex, dicRenameList(new_col.ColumnName) & " as " & new_col.ColumnName & ",")
          ElseIf new_col.DataType Is GetType(System.Boolean) Then
            arSelectList.Insert(iColIndex, "0 as " & new_col.ColumnName & ",")
          ElseIf new_col.DataType Is GetType(System.Int16) OrElse _
            new_col.DataType Is GetType(System.Int32) OrElse _
            new_col.DataType Is GetType(System.Int64) Then
            arSelectList.Insert(iColIndex, "-1 as " & new_col.ColumnName & ",")
          ElseIf new_col.DataType Is GetType(System.String) Then
            arSelectList.Insert(iColIndex, "'' as " & new_col.ColumnName & ",")
          Else
            arSelectList.Insert(iColIndex, "NULL as " & new_col.ColumnName & ",")
          End If
        End If
        'can't just add them by a single ordered named list, have to map them, in case the fields just moved position
        sFieldOrderList &= new_col.ColumnName & ","
        iColIndex += 1
      Next
      Dim sSelectList As String = ""
      For Each str As String In arSelectList
        sSelectList &= str
      Next
      sSelectList = sSelectList.TrimEnd(",")
      sFieldOrderList = sFieldOrderList.TrimEnd(",")

      Dim iRet As Integer = -1
      iRet = ExecuteNonQuery("Insert into " & sNewTableName & " (" & sFieldOrderList & ") SELECT " & sSelectList & " FROM " & sBackupName, sErr, conn, trans)

      If (iRet >= 0) Then
        'Drop the original table
        iRet = ExecuteNonQuery("DROP TABLE IF EXISTS " & sTableName, sErr, conn, trans)

        'Rename the newtable to the original table
        iRet = ExecuteNonQuery("ALTER TABLE " & sNewTableName & " RENAME TO " & sTableName, sErr, conn, trans)

        ExecuteNonQuery("DROP TABLE " & sBackupName, sErr, conn, trans)
        ' trans.Commit()
      End If
      Return (iRet >= 0)
    Catch ex As Exception
      Return False
    Finally
      If Not bUsingIncomingConnection Then
        conn.Close()
        conn.Dispose()
        conn = Nothing
      End If
    End Try
  End Function

#Region "CSV importing functions"
  Public Function LinesInFile(ByVal sFile As String) As Integer
    Dim sr As IO.StreamReader = Nothing
    Dim intLines As Integer = 0
    Try
      sr = New IO.StreamReader(sFile)
      While Not sr.EndOfStream
        sr.ReadLine()
        If (intLines > 1000) Then
          Return -1
        End If
        intLines += 1
      End While

    Catch ex As Exception

    Finally
      If Not sr Is Nothing Then sr.Close()
    End Try

    Return intLines
  End Function



#End Region '"CSV importing functions"

  Private Const TRANSACTION_SIZE = 4000
  Private Const USE_TRANSACTIONS = False

  '===============================================================================
  '    Name: UpdateDatabaseFromDataTable(ByVal sFileName As String, _
  '                                ByRef dtThisDataTable As DataTable) As FileInfo
  ' Remarks: Fill DataTable(dtThisDataTable) from file(sFileName)
  '===============================================================================
  Public Function UpdateDatabaseFromDataTable(ByVal dt As DataTable, _
                                           ByVal sList As String, _
                                           ByVal bEmptyDB As Boolean, _
                                           ByRef iAffectedRowCnt As Integer, _
                                           Optional ByVal bUseTransaction As Boolean = True) As Boolean
    Dim bRet As Boolean = False

    Dim iRow As Integer = 0, iLines As Integer = -1

    Dim sw As New Stopwatch
    sw.Start()
    Dim bCommitted As Boolean = False
    Dim htFieldNames As Collections.Hashtable = Nothing
    Dim sErr As String = ""

    bUseTransaction = bUseTransaction Or USE_TRANSACTIONS

    iLines = dt.Rows.Count
    Dim conn As DbConnection = Nothing
    Dim trans As DbTransaction = Nothing
    Try
      conn = NewDBConnection(g_sConn)
      conn.Open()
      trans = conn.BeginTransaction
      'Add the columns to the data table
      htFieldNames = New Collections.Hashtable
      For i As Integer = 0 To dt.Columns.Count - 1

        htFieldNames.Add(dt.Columns(i).ColumnName, i)

        'asHeaderDEBUG(i) = asLine(i)
      Next

      For Each r As DataRow In dt.Rows
        'Double quotes within a field containing a comma have been replaced by two stars "**" to prevent errors
        'Split the line with handling for commas, single and double quotes
        Debug.Print(iRow & " : " & sw.ElapsedMilliseconds)
        'Begin transaction?
        If ((iRow Mod TRANSACTION_SIZE) = 1) Then
          If (USE_TRANSACTIONS) Then
            'trans =
            bCommitted = False
          End If
        End If

        Dim bDeleted As Boolean = False
        If (r.Table.Columns.Contains("deleted_at")) Then
          If (NullToString(r("deleted_at")) <> "") Then
            bDeleted = True
          End If
        End If

        Try
          Dim sID As String = ""
          'If the database is empty, we'll always INSERT(saves time)
          If Not (bEmptyDB) Then
            'Update the items record if the id OR stock_no matches a user in our db.
            sID = NullToString(r("id"))
            If (sID <> "") Then
              sID = GetFieldFromDB(sList, " id = " & sID, g_sConn, "id", "")
            End If
          End If
          'If (sList = "items") Then
          '  'sID = GetFieldDataBySQL("SELECT id FROM items WHERE id=" & GetCSVColumnValue("id", asLine, htFieldNames) & " OR " & _
          '  '"stock_no='" & GetCSVColumnValue("stock_no", asLine, htFieldNames) & "'", "id")
          'Else

          'End If

          If (sList = "items") Then
            'First Items should find the catalog Item it is associated with.  That Id should be added to the row
            'Items should only insert.  There should not be a need to update your items
            bRet = InsertGenericRecord("items", r, htFieldNames, conn, trans)
          End If

          If sList <> "items" Then
            If (sID = "") Then
              'If dgv IsNot Nothing Then
              '  frmImportCsv.dgvSource.Rows(iRow).DefaultCellStyle.BackColor = Color.Yellow
              'End If
              bRet = DoTableInsert(sList, r, htFieldNames, conn, trans)
            Else
              bRet = DoTableUpdate(sList, r, htFieldNames, conn, trans)
            End If
          End If

        Catch ex As Exception
          'GC.Collect()
          bRet = False
          'RaiseEvent UpdateUiWithSyncMessage("Error loading CSV from file: " & ex.Message)
          GoTo ExitFunction
        End Try


        ' show update on the label so user knows what is going on
        If ((iRow > 0) And (((iRow Mod 25) = 0) Or (iRow = 1))) Then
          Dim sOf As String = IIf(iLines <> -1, " of " & (iLines - 1), "")
          'RaiseEvent UpdateUiWithSyncMessage("Loading " & sList & " : " & iRow & sOf)

        End If

        If bUseTransaction Then
          If ((iRow Mod TRANSACTION_SIZE) = 0) AndAlso (iRow > 0) Then
            trans.Commit()
            bCommitted = True
          End If
        End If

        iRow += 1

      Next
      If bUseTransaction AndAlso ((iRow Mod TRANSACTION_SIZE) <> 0) AndAlso (iRow > 0) AndAlso (bCommitted = False) Then
        trans.Commit()
      End If

      ' show update on the label so user knows what is going on
      If iLines > -1 Then
        'RaiseEvent UpdateUiWithSyncMessage("Loaded : " & (iLines - 1) & " records")
      End If

    Catch ex As Exception
      'RaiseEvent UpdateUiWithSyncMessage("Error loading CSV from file: " & ex.Message)
      If Not (trans Is Nothing) Then trans.Rollback()
      sErr = ex.ToString

      bRet = False
    Finally
      If Not (conn Is Nothing) Then conn.Close()
    End Try

ExitFunction:
    iAffectedRowCnt = (iRow) 'don't include header in row count
    Return bRet
  End Function



#Region "Generic Commands"

  Public Sub ClearGenericCommands()
    htGenericUpdateCommands = New Collections.Hashtable
    htGenericInsertCommands = New Collections.Hashtable
  End Sub

  Public htDynamicUpdateCommands As New Collections.Hashtable
  Public htDynamicInsertCommands As New Collections.Hashtable
  Public Sub ClearDynamicCommands()
    htDynamicUpdateCommands = New Collections.Hashtable
    htDynamicInsertCommands = New Collections.Hashtable
  End Sub
  Public Function InsertRecordDynamic(ByVal lsFields As List(Of String), ByVal loData As List(Of Object), _
                               ByVal sTable As String, ByVal sConnString As String, _
                               Optional ByRef iRowsAffected As Integer = 0, _
                               Optional ByRef conn As DbConnection = Nothing, _
                               Optional ByRef trans As DbTransaction = Nothing, _
                               Optional ByRef sErr As String = "", _
                               Optional ByVal bCloseConn As Boolean = True,
                               Optional ByRef iLastInsertedID As Integer = INVALID_ID) As Boolean
    Dim cmd As DbCommand = Nothing
    Dim bDebug As Boolean = False
    Dim bAnonymousParameters As Boolean = False
    'ODBC doesn't always allow named parameters
    'Use anonymous ones in this scenario
    If (TypeOf conn Is OdbcConnection) Then
      bAnonymousParameters = True
    End If
    If bDebug Then Debug.Print("Insert:" & sTable)
    'Make sure our lists match up
    If (lsFields.Count <> loData.Count) AndAlso (lsFields.Count > 0) Then
      sErr = "Data to insert is no good."
      Return False
    End If
    If (lsFields.Count = 0) Then
      sErr = "No Defined Mappings"
      Return False

    End If

    Try
      If conn Is Nothing Then
        conn = NewDBConnection(sConnString)
        conn.Open()
      End If
      cmd = conn.CreateCommand()


      'Build insert command fields and params and add data params to DbCommand object
      Dim sFieldsSQL As String = "", sParamsSQL As String = ""
      For i As Integer = 0 To lsFields.Count - 1

        Dim field As String = lsFields(i)
        Dim data As Object = IIf(loData(i) Is Nothing, DBNull.Value, loData(i))
        If (TypeOf loData(i) Is System.DateTime) Then
          Dim dtRef As DateTime = loData(i)
          DateTime.SpecifyKind(dtRef, DateTimeKind.Unspecified)
        End If
        sFieldsSQL &= field & ","
        If (Not bAnonymousParameters) Then
          sParamsSQL &= "@" & field & ","
        Else
          sParamsSQL &= "?,"
        End If

        If (Not bAnonymousParameters) Then
          AddNewDBParameter(cmd, "@" & field, data)
        Else
          AddNewDBParameter(cmd, "?", data)
        End If
        'cmd.Parameters.Add(NewDbParameter("@" & field, TypeConvertor.ToDbType(data.GetType)))
        'cmd.Parameters("@" & field).Value = data
      Next

      'Remove trailing commas
      sFieldsSQL = sFieldsSQL.Substring(0, sFieldsSQL.Length - 1)
      sParamsSQL = sParamsSQL.Substring(0, sParamsSQL.Length - 1)

      If Not trans Is Nothing Then cmd.Transaction = trans
      'Assemble insert command
      cmd.CommandText = "INSERT INTO " & sTable & " (" & sFieldsSQL & ") " & "VALUES (" & sParamsSQL & ")"




      'Execute the query
      iRowsAffected = cmd.ExecuteNonQuery()

      iLastInsertedID = ExecuteScalarQuery(SQL_LAST_INSERTED_IDENTITY(conn), sConnString, conn, trans)

    Catch ex As Exception
      sErr = "Database insert failed: " & ex.Message
    Finally
      If bCloseConn Then
        CloseCmdAndConn(cmd, conn)
      Else
        If cmd IsNot Nothing Then cmd.Dispose()
      End If
    End Try

    Return (iRowsAffected > 0)
  End Function



  Public Function UpdateRecordDynamic(ByVal lsFields As List(Of String), ByVal loData As List(Of Object), _
                               ByVal sTable As String, ByVal sConnString As String, _
                               Optional ByRef sWhere As String = "", Optional ByRef iRowsAffected As Integer = 0, _
                               Optional ByVal bCheckDataChanged As Boolean = False,
                               Optional ByRef conn As DbConnection = Nothing, _
                               Optional ByVal trans As DbTransaction = Nothing, _
                               Optional ByRef sErr As String = "", _
                               Optional ByVal bCloseConn As Boolean = True) As Boolean
    Dim cmd As DbCommand = Nothing
    Dim bDebug As Boolean = False
    If bDebug Then Debug.Print("Update:" & sTable)

    'Make sure our lists match up
    If (lsFields.Count <> loData.Count) AndAlso (lsFields.Count > 0) Then
      sErr = "Data to update is no good."
      Return False
    End If

    Try
      If conn Is Nothing Then
        conn = NewDBConnection(sConnString)
        conn.Open()
      End If

      cmd = conn.CreateCommand()
      If Not trans Is Nothing Then cmd.Transaction = trans

      'Build insert command fields and add data params to DbCommand object
      Dim sFieldsSQL As String = ""
      For i As Integer = 0 To lsFields.Count - 1

        Dim field As String = lsFields(i)
        Dim data As Object = IIf(loData(i) Is Nothing, DBNull.Value, loData(i))

        'If we're trying to update a sql server connection, we can't update the ID table
        If TypeOf (conn) Is SqlClient.SqlConnection Then
          If (field.ToLower() = "id") Then
            'Add the parameter for the WHERE clause but don't add it to the field SQL statement
            AddNewDBParameter(cmd, "@" & field, data)
            Continue For
          End If
        End If
        If (TypeOf loData(i) Is System.DateTime) Then
          Dim dtRef As DateTime = loData(i)
          DateTime.SpecifyKind(dtRef, DateTimeKind.Unspecified)
        End If
        If (loData(i) Is Nothing) Then
          sFieldsSQL &= field & "=@" & field & ","
          AddNewDBParameter(cmd, "@" & field, data)
        Else
          sFieldsSQL &= field & "=@" & field & ","
          AddNewDBParameter(cmd, "@" & field, data)
        End If
        If bDebug Then Debug.Print(field & " : " & data.ToString)

        'Make sure your data types are being handled appropriately. GetDbType defaults to Binary.
        'cmd.Parameters.Add(NewDbParameter("@" & field, GetDbType(data)))
        'cmd.Parameters.Add(NewDbParameter("@" & field, TypeConvertor.ToDbType(data.GetType)))
        'cmd.Parameters("@" & field).Value = data
      Next

      'Remove trailing commas
      sFieldsSQL = sFieldsSQL.Substring(0, sFieldsSQL.Length - 1)

      'Assemble insert command
      cmd.CommandText = "UPDATE " & sTable & " SET " & sFieldsSQL & " " & sWhere

      'Execute the query
      iRowsAffected = cmd.ExecuteNonQuery()

    Catch ex As Exception
      sErr = "Database update failed: " & ex.Message
      iRowsAffected = -1
    Finally
      If bCloseConn Then
        CloseCmdAndConn(cmd, conn)
      Else
        If cmd IsNot Nothing Then cmd.Dispose()
      End If
    End Try

    Return (iRowsAffected >= 0)
  End Function



#Region "Generic Record Insert/Update"

  Dim htGenericUpdateCommands As New Collections.Hashtable
  Dim htGenericInsertCommands As New Collections.Hashtable
  Public Function InsertGenericRecord(ByVal sTable As String, _
                    ByVal drItem As DataRow, _
                    ByVal htFieldNames As Collections.Hashtable, _
                    ByVal conn As DbConnection, _
                    ByVal trans As DbTransaction) As Integer
    Dim iRet As Integer = 0
    Dim cmd As DbCommand
    If (htGenericInsertCommands.ContainsKey(sTable)) Then
      cmd = htGenericInsertCommands(sTable)
    Else
      cmd = BuildInsertCommand(sTable)
      cmd.Prepare()
      htGenericInsertCommands.Add(sTable, cmd)
    End If
    ' TBD - it looks like all the commands in the hash table are being opened, never to close 
    '       This could significantly impact the database.  
    '  To show this, import a CSV table.  - putting a connection here might fix it.
    If Not (conn Is Nothing) Then '(cmd.Connection.State <> ConnectionState.Open) 
      cmd.Connection = conn
    End If

    If (cmd.Connection.State <> ConnectionState.Open) Then
      cmd.Connection.Open()
    End If

    If (conn IsNot Nothing) And (trans IsNot Nothing) Then '(cmd.Connection.State <> ConnectionState.Open) 
      cmd.Transaction = trans
    End If

    AddAllColumnValues(cmd.Parameters, drItem, htFieldNames)

    iRet = cmd.ExecuteNonQuery

    If (conn Is Nothing) Then '(cmd.Connection.State <> ConnectionState.Open) 
      cmd.Connection.Close()
    End If
    Return iRet
  End Function

  Public Function UpdateGenericRecord(ByVal sTable As String, _
                    ByVal drItem As DataRow, _
                    ByVal htFieldNames As Collections.Hashtable, _
                    ByVal conn As DbConnection, _
                    ByVal trans As DbTransaction) As Integer
    Dim iRet As Integer = 0
    Dim cmd As DbCommand = Nothing

    'All fields in the table will be updated if _ is false.

    If (htGenericUpdateCommands.ContainsKey(sTable)) Then
      cmd = htGenericUpdateCommands(sTable)

    Else
      cmd = BuildUpdateCommandUsingDbSchema(sTable)
      cmd.Prepare()
      htGenericUpdateCommands.Add(sTable, cmd)
    End If

    If Not (conn Is Nothing) Then '(cmd.Connection.State <> ConnectionState.Open) 
      cmd.Connection = conn
    End If

    If (cmd.Connection.State <> ConnectionState.Open) Then
      cmd.Connection.Open()
    End If

    If (conn IsNot Nothing) And (trans IsNot Nothing) Then
      cmd.Transaction = trans
    End If

    ''If table is corrupt in htUpdateCommands, then update it
    'If (cmd Is Nothing) OrElse (cmd.Connection Is Nothing) OrElse _
    '   (cmd.Connection.State <> ConnectionState.Open) Then
    '  htUpdateCommands.Add(sTable, cmd)
    '  cmd.Prepare()
    '  htUpdateCommands.Item(sTable) = cmd
    'End If



    If trans IsNot Nothing Then cmd.Transaction = trans
    AddAllColumnValues(cmd.Parameters, drItem, htFieldNames)


    iRet = cmd.ExecuteNonQuery

    If (conn Is Nothing) Then '(cmd.Connection.State <> ConnectionState.Open) 
      cmd.Connection.Close()
    End If

    Return iRet
  End Function


  Public Sub AddAllColumnValues(ByRef parameters As Data.SQLite.SQLiteParameterCollection, _
                 ByVal item As DataRow, ByVal htListHeaders As Collections.Hashtable)
    For Each param As Data.SQLite.SQLiteParameter In parameters
      'Dim sVal As String = GetCSVColumnValue(param.ParameterName.Replace("@", ""), items, htListHeaders)
      If (Not item.Table.Columns.Contains(param.ParameterName.Replace("@", ""))) Then
        param.Value = Nothing
        Continue For
      End If
      Dim sVal As String = NullToString(item(param.ParameterName.Replace("@", "")))
      sVal = sVal.Replace("""", "")
      If (param.DbType = DbType.String) Then
        Try
          'It's only necessary to replace 1 single quote with 2 if we are inserting/updating via query.
          ' Not necessary since we are using a parameterized command
          param.Value = sVal '.Replace("'", "''")
        Catch ex As Exception
          param.Value = ""
        End Try

      ElseIf (param.DbType = DbType.Boolean) Then
        Try
          If sVal = "" Then
            param.Value = False
          Else
            param.Value = CBool(sVal)
          End If

        Catch ex As Exception
          param.Value = False
        End Try

      ElseIf (param.DbType = DbType.Int32) Or (param.DbType = DbType.Int64) Then

        Try
          If (sVal = "") Then
            param.Value = DBNull.Value
          Else
            param.Value = CInt(sVal)
          End If
        Catch ex As Exception
          param.Value = DBNull.Value
        End Try

      ElseIf (param.DbType = DbType.Double) Then
        Try
          If (sVal = "") Then
            param.Value = DBNull.Value
          Else
            param.Value = Val(sVal)
          End If
        Catch ex As Exception
          param.Value = DBNull.Value
        End Try

      Else
        Try
          param.Value = item(param.ParameterName.Replace("@", ""))
        Catch ex As Exception
          param.Value = DBNull.Value
        End Try
      End If
    Next
  End Sub

#End Region '"TrackingType Insert/Update"

  Public Function BuildInsertCommand(ByVal sTableName As String) As DbCommand
    Dim sSQL As String = ""
    Dim conn As DbConnection = NewDBConnection(g_sConn)

    conn.Open()
    Dim outBoundCmd As DbCommand = NewDbCommand("", conn)

    Dim rdr As DbDataReader = Nothing
    Dim cmd As DbCommand = NewDbCommand("SELECT * FROM " & sTableName & " LIMIT 0", conn)
    Try

      rdr = cmd.ExecuteReader()

      sSQL = "INSERT INTO " & sTableName & " ("
      Dim sFields As String = ""
      For i As Integer = 0 To rdr.FieldCount - 1
        'For insert, add all columns
        sFields &= rdr.GetName(i) & ","
        outBoundCmd.Parameters.Add(NewDbParameter("@" & rdr.GetName(i), TypeConvertor.ToDbType(rdr.GetFieldType(i))))
      Next
      sFields = sFields.TrimEnd(",")

      sSQL &= sFields & ") VALUES (@" & sFields.Replace(",", ",@") & ")"
      outBoundCmd.CommandText = sSQL

    Catch ex As Exception

    Finally
      rdr.Close()
      CloseCmdAndConn(cmd, conn)

    End Try

    Return outBoundCmd
  End Function


  Public Function BuildUpdateCommandUsingDbSchema(ByVal sTableName As String) As DbCommand
    Dim sSQL As String = ""
    Dim conn As DbConnection = NewDBConnection(g_sConn)

    conn.Open()
    Dim outBoundCmd As DbCommand = NewDbCommand("", conn)

    Dim rdr As DbDataReader = Nothing
    Dim cmd As DbCommand = NewDbCommand("SELECT * FROM " & sTableName & " LIMIT 0", conn)
    Try

      rdr = cmd.ExecuteReader()

      sSQL = "UPDATE " & sTableName & " SET "
      For i As Integer = 0 To rdr.FieldCount - 1
        If (rdr.GetName(i) <> "id") Then
          'For update, add an item for everything but id
          sSQL &= rdr.GetName(i) & "=@" & rdr.GetName(i) & ","
        End If
        outBoundCmd.Parameters.Add(NewDbParameter("@" & rdr.GetName(i), TypeConvertor.ToDbType(rdr.GetFieldType(i))))
      Next
      sSQL = sSQL.TrimEnd(",")
      sSQL &= " WHERE id=@id"
      outBoundCmd.Parameters.Add(NewDbParameter("@id", DbType.Int32))
      outBoundCmd.CommandText = sSQL

    Catch ex As Exception

    Finally
      rdr.Close()
      CloseCmdAndConn(cmd, conn)
    End Try

    Return outBoundCmd
  End Function

  ''' <summary>
  ''' Builds Update Command with Custom Where clause.  htWhereParams key is the parameter name and value is its database datatype
  ''' </summary>
  ''' <param name="sTableName"></param>
  ''' <param name="htWhereParams"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  ''' 
  Public Function BuildUpdateCommandUsingDbSchemaWithParams(ByVal sTableName As String, ByVal htWhereParams As Hashtable) As DbCommand
    Dim sSQL As String = ""
    Dim conn As DbConnection = NewDBConnection(g_sConn)

    conn.Open()
    Dim outBoundCmd As DbCommand = NewDbCommand("", conn)

    Dim rdr As DbDataReader = Nothing
    Dim cmd As DbCommand = NewDbCommand("SELECT * FROM " & sTableName & " LIMIT 0", conn)
    Try

      rdr = cmd.ExecuteReader()

      sSQL = "UPDATE " & sTableName & " SET "
      For i As Integer = 0 To rdr.FieldCount - 1
        If (rdr.GetName(i) <> "id") Then
          'For update, add an item for everything but id
          sSQL &= rdr.GetName(i) & "=@" & rdr.GetName(i) & ","
        End If
        outBoundCmd.Parameters.Add(NewDbParameter("@" & rdr.GetName(i), TypeConvertor.ToDbType(rdr.GetFieldType(i))))
      Next
      sSQL = sSQL.TrimEnd(",")
      sSQL &= " WHERE "
      For Each key As String In htWhereParams.Keys
        sSQL &= key & "=@" & key & " AND "
        outBoundCmd.Parameters.Add(NewDbParameter("@" & key, htWhereParams(key)))
      Next
      'cut off the last AND
      sSQL = sSQL.Substring(0, sSQL.Length - 4)
      outBoundCmd.CommandText = sSQL

    Catch ex As Exception

    Finally
      rdr.Close()
      CloseCmdAndConn(cmd, conn)
    End Try

    Return outBoundCmd
  End Function

#End Region


  Public Function DoTableInsert(ByVal sList As String, ByVal item As DataRow, _
               ByVal htFieldNames As Collections.Hashtable, ByVal conn As DbConnection, ByVal trans As DbTransaction) As Boolean
    Dim iRet As Integer = -1

    iRet = InsertGenericRecord(sList, item, htFieldNames, conn, trans)

    Return (iRet > 0)
  End Function

  Public Function DoTableUpdate(ByVal sList As String, ByVal item As DataRow, _
               ByVal htFieldNames As Collections.Hashtable, ByVal conn As DbConnection, _
               ByVal trans As DbTransaction) As Boolean
    Dim iRet As Integer = -1
    iRet = UpdateGenericRecord(sList, item, htFieldNames, conn, trans)

    Return (iRet > 0)
  End Function


  '===============================================================================
  '    Name: function SplitDBRecord
  ' Remarks: Split record into an array appropriately handling commas and quotes
  '===============================================================================
  Private Function SplitDBRecord(ByVal lne As String, ByRef sErr As String) As String()
    Dim iFieldStart As Integer, iFieldEnd As Integer
    Dim iFieldCount As Integer
    Dim saLne() As String = Nothing
    Dim bEmptyField As Boolean = False

    Try
      While True 'Add each field to the array
        iFieldStart = IIf(iFieldEnd = 0, 0, iFieldEnd + 1)
        iFieldCount = iFieldCount + 1
        bEmptyField = False

        'if this is the last field and contains nothing ex: 'data1,data2,', set field data to empty string
        If (iFieldStart >= lne.Length) OrElse (lne.Substring(iFieldStart, 1) = ",") Then
          iFieldEnd = iFieldStart
          bEmptyField = True

          'If the field starts with a double quote then figure out where the field ends
        ElseIf lne.Substring(iFieldStart, 1) = """" Then

          For i As Integer = (iFieldStart + 1) To Len(lne)
            'Replace double quotes with garbage string. Skip sets of two double quotes.
            If lne.Replace("""""", "@@").Substring(i, 1) = """" Then
              'We have one double quote. Goto the next field
              iFieldEnd = i + 1 : Exit For
            End If
          Next 'l

        Else
          'Find the comma at the end of the field
          iFieldEnd = lne.IndexOf(",", iFieldStart)

          'Use the line length if this is the last field
          If (iFieldEnd < 1) Then iFieldEnd = lne.Length
        End If

        'Dimension the array
        ReDim Preserve saLne(iFieldCount - 1)

        'Fill the array element
        Dim sValue As String = ""
        If Not bEmptyField Then
          sValue = PrepareCsvStringForDB(lne.Substring(iFieldStart, iFieldEnd - iFieldStart))
        End If

        If (sValue.Length > 0) Then
          'Remove leading and trailing quotes
          If (sValue.StartsWith("""")) Then sValue = sValue.TrimStart("""")
          If (sValue.EndsWith("""")) Then sValue = sValue.TrimEnd("""")
        End If

        saLne(iFieldCount - 1) = sValue

        'Exit if we just inserted the last field
        If (iFieldEnd >= lne.Length) Or (iFieldStart >= lne.Length) Then Exit While
      End While

    Catch ex As Exception
      sErr = "Error in CSV File:  ex.Message" & vbCrLf & "Line: " & lne
    End Try

    Return saLne
  End Function

  '===============================================================================
  '    Name: Sub CreateJoinCSVInsertSQL
  ' Remarks: Remove extra quotations used around fields containing commas and also prepare single
  '          quotes for database inserting ' -> ''
  '===============================================================================
  Private Function PrepareCsvStringForDB(ByRef sLine As String) As String
    Dim s As String = sLine

    'It's only necessary to replace 1 single quote with 2 if we are inserting/updating via query.
    ' Not necessary if we are using a parameterized command
    's = s.Replace("'", "''")
    s = s.Replace(""",", ",")
    s = s.Replace(",""", ",")
    s = s.Replace("""""", """")

    Return s
  End Function


  Public Function AddSyncDateForTable(ByVal sTable As String, ByVal conn As DbConnection) As Boolean
    'We don't need this for muster_activities because we create a badge_activities for it.
    ExecuteNonQuery("DROP TRIGGER IF EXISTS update_sync_dates_" & sTable, , conn)
    If (ExecuteNonQuery("CREATE TRIGGER update_sync_dates_" & sTable & " AFTER UPDATE ON  " & sTable & " " & _
              " BEGIN" & _
              "  UPDATE sync_data SET updated_at = NOW() " & _
              " WHERE table_name = '" & sTable & "';" & _
              "  END;", , conn) < 0) Then Return False

    ExecuteNonQuery("DROP TRIGGER IF EXISTS insert_sync_dates_" & sTable, , conn)
    If (ExecuteNonQuery("CREATE TRIGGER insert_sync_dates_" & sTable & " AFTER INSERT ON  " & sTable & " " & _
              " BEGIN" & _
              "  UPDATE sync_data SET updated_at = NOW() " & _
              " WHERE table_name = '" & sTable & "';" & _
              "  END;", , conn) < 0) Then Return False

    'Insert an item into the sync_data table when we don't have one
    If (ExecuteScalarQuery("select count(*) from sync_data where table_name = '" & sTable & "'", g_sConn, conn) = 0) Then
      ExecuteNonQuery("INSERT into sync_data (table_name, created_at, updated_at) VALUES('" & sTable & "',NOW(),NOW())", , conn)
    End If
    Return True
  End Function

  'Create the table in the database using the passed in statement if the table schema is different or
  '  the table is missing

  Public Function CreateTableIfNecessary(ByVal sTable As String, _
                                          ByRef sErr As String, _
                                          ByVal conn As DbConnection, _
                                          ByVal trans As DbTransaction,
                                          Optional dicRenameList As Dictionary(Of String, String) = Nothing, _
                                          Optional connSQLiteLookup As DbConnection = Nothing) As Boolean

    Dim sSQL As String
    Dim sError As String = ""
    'Let's make it work for SQL Server

    Dim iConnType As DB_TYPES = DB_TYPES.UNSPECIFIED
    If conn IsNot Nothing Then
      If (TypeOf conn Is SQLiteConnection) Then
        iConnType = DB_TYPES.SQLITE
      ElseIf (TypeOf conn Is SqlClient.SqlConnection) Then
        iConnType = DB_TYPES.SQLSERVER
      End If
    Else
      iConnType = g_iConnType
    End If

    Dim sSQLCreate As String = "", bRet As Boolean = False

    If iConnType = DB_TYPES.SQLITE Then
      'It's an SQLite Connection and we'll proceed
      sSQLCreate = g_htTableCreateStatements(sTable)
      If String.IsNullOrEmpty(sSQLCreate) Then
        sErr = "Table CREATE statement does not exist for " & sTable
        Return False
      End If



      Dim sTableSchema As String = GetTableSchema(sTable, conn)
      Dim sSQLCreateCompare As String = "CREATE TABLE " & sTable & " " & sSQLCreate

      ' These lines are meant to make the SQL formats match if there are differences
      sTableSchema = RemoveMoreThanOneSpace(sTableSchema).Replace("[", "").Replace("]", "").Replace("""", "").Replace("( ", "(").Replace(" )", ")").Replace(", ", ",")
      sSQLCreateCompare = RemoveMoreThanOneSpace(sSQLCreateCompare).Replace("[", "").Replace("]", "").Replace("( ", "(").Replace(" )", ")").Replace(", ", ",")

      If (sTableSchema <> "") AndAlso (sTableSchema <> sSQLCreateCompare) Then

        'Drop/Recreate the table and data
        Debug.Print(Now.ToLongTimeString & " : " & sTable & " : " & sSQLCreate.Replace(vbCrLf, ""))
        bRet = UpdateOrCreateTable(sTable, sSQLCreate, conn, trans, sErr, dicRenameList)

      Else

        sSQLCreate = "CREATE TABLE IF NOT EXISTS " & sTable & " " & sSQLCreate
        If (ExecuteNonQuery(sSQLCreate, sErr, conn, trans) < 0) Then
          bRet = False
        Else
          'Only run the update commands if our SQL was good
          Dim iCount As Integer = ExecuteScalarQuery("SELECT count(type) FROM sqlite_master WHERE type='table' AND name='" & sTable & "'", g_sConn, conn, trans)
          If (iCount > 0) And sTable <> "settings" Then
            'Make sure every table has its update_at and created_at set
            Dim lsFields As List(Of String) = GetTableFieldsList(sTable, conn, trans)
            ' perform update_at commands if the field exist
            If lsFields.Contains("updated_at") Then
              sSQL = "UPDATE " & sTable & " SET updated_at=NOW() where updated_at IS NULL"
              ExecuteNonQuery(sSQL, sError, conn, trans)
            End If
            ' perform created_at commands if the field exist
            If lsFields.Contains("created_at") Then
              sSQL = "UPDATE " & sTable & " SET created_at=NOW() where created_at IS NULL"
              ExecuteNonQuery(sSQL, sError, conn, trans)
            End If

          End If

          bRet = True
        End If

      End If
    ElseIf iConnType = DB_TYPES.SQLSERVER Then
      Dim bLocalSQLiteDB As Boolean = False
      'For now we can't just check if
      If connSQLiteLookup Is Nothing Then
        connSQLiteLookup = NewDBConnection("Data Source=:memory:", DB_TYPES.SQLITE)
        connSQLiteLookup.Open()
        bLocalSQLiteDB = True
      End If
      'Make the above call to setup the SQL in memory database
      CreateTableIfNecessary(sTable, sErr, connSQLiteLookup, Nothing)

      Dim iTableVersion As Integer = GetExtendedPropertyDBVersion(sTable, conn)
      Dim iExpectedTableVersion As Integer = g_htTableVersions(sTable)

      If iTableVersion <> iExpectedTableVersion Then

DoItAgain:

        Dim drTableExists As DataRowCollection = GetDataRowsBySQL(String.Format("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{0}'", sTable), conn, trans, sErr)
        If (drTableExists IsNot Nothing) AndAlso drTableExists.Count > 0 Then
          'The table already exists.
          'Check if it needs to be updated
          bRet = UpdateOrCreateTable(sTable, sSQLCreate, conn, trans, sErr, dicRenameList)
#If DEBUG Then
          'This is just the debug code. If it already exists we should try to upgrade the table.
          ExecuteNonQuery("DROP TABLE " & sTable, , conn, trans)
          GoTo DoItAgain
#End If
        Else
          Dim sTableCreateStatement As String = ""
          If Not BuildCreateTableStatementForSQLServerFromSQLite(sTable, connSQLiteLookup, sErr, sTableCreateStatement) Then
            bRet = False
            sErr = "Error Creating Table: " & sTable & vbCrLf & sErr
          Else
            Dim iRet As Integer = ExecuteNonQuery(sTableCreateStatement, sErr, conn, trans)
            If (iRet < 0) AndAlso Not sErr.Contains("SQL SUCCESS") Then
              bRet = False
            Else
              bRet = True
            End If
          End If
        End If
        AddOrUpdateExtendedPropertyDBVersion(sTable, iExpectedTableVersion, conn)
      End If


      'Clean up the local SQLite connection we used to create the SQL Server database
      If bLocalSQLiteDB Then
        connSQLiteLookup.Close()
        connSQLiteLookup.Dispose()
        connSQLiteLookup = Nothing
      End If
    End If

    Return bRet

  End Function

  ''' Returns the value of specified extended property of the database. Returns Nothing if the specified property is not defined.
  Private Function GetExtendedPropertyDBVersion(sTable As String, ByVal conn As DbConnection) As Integer
    Dim bIncomingConnection As Boolean = False
    Dim iRet As Integer = -1
    Try
      If conn Is Nothing Then
        bIncomingConnection = True
        conn = NewDBConnection(g_sConn)
        conn.Open()
      End If
      Dim cmd As DbCommand = conn.CreateCommand()

      cmd.CommandText = String.Format("SELECT value FROM fn_listextendedproperty('DBVersion', N'Schema','dbo',N'Table', '{0}', default, default)", sTable)

      Dim sRet As String = ""

      sRet = cmd.ExecuteScalar()
      If (Not String.IsNullOrEmpty(sRet)) Then
        iRet = Integer.Parse(sRet)
      End If
    Catch ex As Exception
      'Couldn't find the property
      iRet = -1
#If DEBUG Then
      MsgBox(ex)
#End If
    Finally
      If bIncomingConnection Then
        If conn IsNot Nothing Then
          If conn.State = ConnectionState.Open Then
            conn.Close()
          End If
          conn.Dispose()
          conn = Nothing
        End If
      End If
    End Try
    Return iRet


  End Function

  ''' Adds or updates extended property to the database with the specified name and value for the given table
  Private Sub AddOrUpdateExtendedPropertyDBVersion(sTable As String, propValue As String, conn As DbConnection)
    Dim bIncomingConnection As Boolean = False
    Dim cmd As DbCommand = conn.CreateCommand()
    If conn Is Nothing Then
      bIncomingConnection = True
      conn = NewDBConnection(g_sConn)
      conn.Open()
    End If
    Try

      If GetExtendedPropertyDBVersion(sTable, conn) < 0 Then
        '      EXEC sp_addextendedproperty
        '@name = N'DBVersion', 
        '@value = '0',
        '@level0type = N'Schema', @level0name = 'dbo',
        '@level1type = N'Table', @level1name='startup_import_tables'

        cmd.CommandText = String.Format("EXEC sp_addextendedproperty @name = N'DBVersion', " & _
                                        "@value = '{0}', @level0type=N'Schema',@level0name=N'dbo'," & _
                                        "@level1type=N'Table',@level1name='{1}'", propValue, sTable)
      Else
        cmd.CommandText = String.Format("EXEC sp_updateextendedproperty @name = N'DBVersion', @value = '{0}', " & _
                                        "@level0type=N'Schema',@level0name=N'dbo'," & _
                                        "@level1type=N'Table',@level1name='{1}'", propValue, sTable)
      End If

      cmd.ExecuteNonQuery()
    Catch ex As Exception
#If DEBUG Then
      MsgBox(ex)
#End If
    Finally
      If bIncomingConnection Then
        If conn IsNot Nothing Then
          If conn.State = ConnectionState.Open Then
            conn.Close()
          End If
          conn.Dispose()
          conn = Nothing
        End If
      End If
    End Try


  End Sub
  Public Function ParseConnectionString(ByVal sString As String, ByVal sKey As String) As String
    Dim iStart As Integer = 0, iEnd As Integer = 0
    iStart = InStr(sString.ToLower, sKey.ToLower)
    If (iStart <= 0) Then
      Return ""
    End If

    iEnd = InStr(iStart, sString, ";")
    If (iEnd <= iStart) Then
      iEnd = sString.Length
    End If
    Dim sRet As String = Mid(sString, iStart + Len(sKey), iEnd - iStart - Len(sKey))
    Return sRet
  End Function


  '===============================================================================
  '    Name: NullToLong 
  ' Remarks: Returns 0 if passed in value is null
  '===============================================================================
  Public Function NullToLong(ByVal Value As Object, Optional ByVal iDefault As Long = INVALID_ID) As Long
    Dim lRet As Long = iDefault
    Try
      If Not ((IsDBNull(Value) = True) Or (Value Is Nothing)) Then
        lRet = CLng(Value)
      End If
    Catch
      Debug.Print("Invalid Cast Exception in NullToLong")
      lRet = iDefault
    End Try
    Return lRet
  End Function


  '===============================================================================
  '    Name: Function NoSingleQuote
  ' Remarks: Prepare string for database
  '===============================================================================
  Public Function NoSingleQuote(ByVal sValue As String) As String
    If String.IsNullOrEmpty(sValue) Then Return ""

    Select Case g_iConnType
      Case DB_TYPES.MYSQL
        Return sValue.Trim.Replace("'", "\'")
      Case DB_TYPES.OLEDB
        Return sValue.Trim.Replace("'", "''")
      Case Else 'DB_TYPES.SQLITE
        Return sValue.Trim.Replace("'", "''")
    End Select
  End Function


  Public Function SaveDataTableToCSV(ByVal dt As DataTable, ByVal sFile As String) As Boolean

    Dim writer As StreamWriter = Nothing
    Try
      writer = New StreamWriter(sFile)
      Dim i As Integer = 0
      For Each col As DataColumn In dt.Columns
        If i = dt.Columns.Count - 1 Then
          writer.WriteLine(col.ColumnName)
        Else
          writer.Write(String.Concat(col.ColumnName, ","))
        End If
        i += 1
      Next

      For Each row As DataRow In dt.Rows
        i = 0
        For j As Integer = 0 To dt.Columns.Count - 1
          Dim cell As Object = row(j)
          If (cell Is DBNull.Value) Then
            writer.Write("")
          Else
            If (dt.Columns(j).DataType Is GetType(String)) Then
              writer.Write("""" & cell.replace("""", """""") & """")
            Else
              writer.Write(cell.ToString())
            End If

          End If

          'Add a comma if it's not the end
          If (j < dt.Columns.Count - 1) Then
            writer.Write(",")
          End If
        Next
        writer.Write(vbCrLf)
      Next

      Return True

    Catch ex As Exception
      MsgBox("Error Creating .csv File: " & ex.Message)
      Return False
    Finally
      If writer IsNot Nothing Then writer.Close()
    End Try

  End Function

#Region "Logging"

  ' Function to display parent function
  Public Function GetCallingFunction(ByVal iDepth As Integer, ByVal stackTrace As StackTrace) As String

    Dim stackFrame As StackFrame = stackTrace.GetFrame(iDepth)
    Dim methodBase As Reflection.MethodBase = stackFrame.GetMethod()
    ' Displays 
    '   Return String.Format("  {0}  Line {1} File: {2}", methodBase.Name, stackTrace.GetFrame(iDepth).GetFileLineNumber().ToString, stackTrace.GetFrame(iDepth).GetFileName())
    Return String.Format("  {0} ", methodBase.Name)

  End Function
  Public Function GetExceptionText(ByVal e As Exception, Optional ByVal iDepth As Integer = 12) As String
    Dim sb As New StringBuilder
    Try

      sb.AppendLine("--------------------------------------------")
      sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"))
      sb.AppendLine("Please email this file to support@telaeris.com")
      sb.AppendLine("along with a description of what you were doing")
      sb.AppendLine("--------------------------------------------")
      sb.AppendLine("------" & My.Application.Info.Title & " v" & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build & "." & My.Application.Info.Version.Revision & "------")
      sb.AppendLine("---Calling Functions---------------------------")
      sb.AppendLine(GetStackTraceString(iDepth))
      sb.AppendLine("---Exception Message--------------------------")
      sb.AppendLine(e.Message)
      sb.AppendLine("---Exception Stack Trace----------------------")
      sb.AppendLine(e.StackTrace)
      'sb.AppendLine("-Exception Function----------------------")
      'sb.AppendLine(e.)
    Catch ex As Exception

    End Try
    Return sb.ToString
  End Function

  Public Function GetStackTraceString(Optional ByVal iDepth As Integer = 5) As String
    Dim sb As New StringBuilder
    Try
      Dim stackTrace As New StackTrace()

      sb.AppendLine("-Calling Functions---------------------------")
      For i As Integer = 1 To stackTrace.FrameCount - 1
        sb.AppendLine(GetCallingFunction(i, stackTrace))
        If (i > iDepth) Then Exit For
      Next
      Return sb.ToString
    Catch ex As Exception
      Return "Error Getting Stack Trace"
    End Try
  End Function



#End Region

#Region "SHARED SQL STATEMENTS"


  ''' <summary>
  ''' SQL for determining if table exists
  ''' </summary>
  ''' <param name="sTable">Name of table</param>
  ''' <returns>1 if table exists, 0 otherwise</returns>
  ''' <remarks></remarks>
  Public Function SQL_TABLE_EXISTS(ByVal sTable As String) As String
    Dim sRet As String = ""

    Select Case g_iConnType
      'Case DB_TYPES.MYSQL
      'Case DB_TYPES.OLEDB
      'Case DB_TYPES.ODBC
      Case DB_TYPES.SQLSERVER
        sRet = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES " & _
          "WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{0}'"

      Case DB_TYPES.SQLITE
        sRet = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='{0}'"

      Case Else
        Throw New Exception("Unsupported Database Type: " & DBTypeName(g_iConnType))

    End Select

    sRet = String.Format(sRet, sTable)

    Return sRet
  End Function

  ''' <summary>
  ''' Generic get * from Table SQL
  ''' </summary>
  ''' <param name="sTable"></param>
  ''' <param name="sFields"></param>
  ''' <param name="sWhere"></param>
  ''' <param name="sOrderBy"></param>
  ''' <param name="iLimit"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function SQL_BASE_SELECT(ByVal sTable As String, _
                                    Optional ByVal sFields As String = "*", _
                                    Optional ByVal sWhere As String = "", _
                                    Optional ByVal sOrderBy As String = "", _
                                    Optional ByVal iLimit As Integer = -1) As String
    Dim sRet As String = ""
    Dim sLimit As String = ""

    Select Case g_iConnType
      'Case DB_TYPES.MYSQL
      'Case DB_TYPES.OLEDB
      'Case DB_TYPES.ODBC

      Case DB_TYPES.SQLSERVER
        If (sWhere.Trim <> "") AndAlso (Not sWhere.Trim.ToUpper.StartsWith("WHERE")) Then
          sWhere = " WHERE " & sWhere
        End If

        If (sOrderBy.Trim <> "") AndAlso (Not sOrderBy.Trim.ToUpper.StartsWith("ORDER BY")) Then
          sOrderBy = " ORDER BY " & sOrderBy
        End If

        If iLimit > 0 Then
          sLimit = " TOP " & iLimit & " "c
        End If

        sRet = "Select " & sLimit & sFields & " FROM " & sTable & sWhere & sOrderBy


      Case DB_TYPES.SQLITE
        If (sWhere.Trim <> "") AndAlso (Not sWhere.Trim.ToUpper.StartsWith("WHERE")) Then
          sWhere = " WHERE " & sWhere
        End If

        If (sOrderBy.Trim <> "") AndAlso (Not sOrderBy.Trim.ToUpper.StartsWith("ORDER BY")) Then
          sOrderBy = " ORDER BY " & sOrderBy
        End If

        If iLimit > 0 Then
          sLimit = " LIMIT " & iLimit
        End If

        sRet = "Select " & sFields & " FROM " & sTable & sWhere & sOrderBy & sLimit

      Case Else
        Throw New Exception("Unsupported Database Type: " & DBTypeName(g_iConnType))

    End Select
    Return sRet

  End Function

  Public Function SQL_LAST_INSERTED_IDENTITY(dbconn As DbConnection) As String
    Dim iConnType As DB_TYPES = DB_TYPES.SQLITE
    If (TypeOf (dbconn) Is SQLite.SQLiteConnection) Then
      iConnType = DB_TYPES.SQLITE
    ElseIf (TypeOf (dbconn) Is SqlClient.SqlConnection) Then
      iConnType = DB_TYPES.SQLSERVER
    Else

    End If
    Select Case iConnType
      Case DB_TYPES.SQLSERVER
        Return "SELECT @@IDENTITY"
      Case DB_TYPES.SQLITE
        Return "SELECT last_insert_rowid()"
      Case Else
        Throw New Exception("Unsupported Database Type: " & DBTypeName(g_iConnType))
    End Select
  End Function

  Public Function UtcNowForDBInsert() As DateTime
    Return New DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified)
  End Function

  ''' <summary>
  ''' SQL for current time/date in UTC
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function SQL_NOW(sSQL As String) As String
    Dim sRet As String = ""

    'Don't need the replace if not needed
    If Not String.IsNullOrEmpty(sSQL) Then
      If Not sSQL.Contains("NOW()") Then
        Return sSQL
      End If
    End If

    Select Case g_iConnType
      Case DB_TYPES.MYSQL
        'Case DB_TYPES.OLEDB
        'Case DB_TYPES.ODBC
        sRet = "UTC_TIMESTAMP()"
      Case DB_TYPES.SQLSERVER
        sRet = "GETUTCDATE()"

      Case DB_TYPES.SQLITE
        ' sRet = "'" & Format(SQL_DATE(UtcNowForDBInsert()), DATE_FORMAT_SQLITE) & "'"   ' 2013-12-01 06:32:09.4592190
        ' sRet = "DATETIME('now')"                                         ' 2013-12-01 06:32:09
        sRet = "STRFTIME('%Y-%m-%d %H:%M:%f', 'now')"                      ' 2013-12-01 06:32:09.459    <--- Chosen so that there are not minor differences

      Case Else
        Throw New Exception("Unsupported Database Type: " & DBTypeName(g_iConnType))

    End Select

    If (String.IsNullOrEmpty(sSQL)) Then
      Return sRet
    Else
      Return sSQL.Replace("NOW()", sRet)
    End If

  End Function

  Public Function SQL_LOCALTIME(sField As String) As String
    Dim sRet As String = ""
    Dim iType As DB_TYPES = g_iConnType

    Select Case iType
      Case DB_TYPES.SQLITE
        sRet = "datetime(" & sField & ",'localtime')"
      Case DB_TYPES.SQLSERVER
        sRet = " CONVERT(datetime, " & _
                      "       SWITCHOFFSET(CONVERT(datetimeoffset," & sField & "), " & _
                      "       DATENAME(TzOffset, SYSDATETIMEOFFSET())))"
      Case Else
        Throw New Exception("Need to Specify Localtime for db type: " & iType)
    End Select

    Return sRet

  End Function

  ''' <summary>
  ''' SQL for current time/date in UTC
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function SQL_DATE(ByVal dtNow As DateTime) As String
    Dim sRet As String = ""
    Select Case g_iConnType
      'Case DB_TYPES.MYSQL
      'Case DB_TYPES.OLEDB
      'Case DB_TYPES.ODBC
      Case DB_TYPES.SQLSERVER
        sRet = "GETUTCDATE(" & dtNow & ")"

      Case DB_TYPES.SQLITE
        Dim sDT As String = String.Format("{0:yyyy-MM-dd HH:mm:ss.ffffff}", dtNow)
        ' sRet = "'" & Format(SQL_DATE(UtcNowForDBInsert()), DATE_FORMAT_SQLITE) & "'"   ' 2013-12-01 06:32:09.4592190
        ' sRet = "DATETIME('now')"                                         ' 2013-12-01 06:32:09
        sRet = sDT
        ' "STRFTIME('%Y-%m-%d %H:%M:%f', 'now')"                      ' 2013-12-01 06:32:09.459    <--- Chosen so that there are not minor differences

      Case Else
        Throw New Exception("Unsupported Database Type: " & DBTypeName(g_iConnType))

    End Select

    Return sRet
  End Function

#End Region

  Public Sub AddConnectionToRecentDBList(ByRef scRecentDBs As System.Collections.Specialized.StringCollection, _
                                         sConn As String, iConnType As DB_TYPES, _
                                         Optional iMaxCount As Integer = 10)

    Dim sConnInfo As String = iConnType & ":" & sConn
    Dim i As Long

    'remove old instances of this connection
    If scRecentDBs.Contains(sConnInfo) Then
      i = scRecentDBs.IndexOf(sConnInfo)
      scRecentDBs.RemoveAt(i)
    End If

    'add latest to top of list
    If sConn & "" <> "" Then scRecentDBs.Insert(0, sConnInfo)

    If (iMaxCount > 0) Then
      'keep the list to specified length
      Do While scRecentDBs.Count > iMaxCount
        scRecentDBs.RemoveAt(scRecentDBs.Count - 1)
      Loop
    End If

  End Sub


End Module