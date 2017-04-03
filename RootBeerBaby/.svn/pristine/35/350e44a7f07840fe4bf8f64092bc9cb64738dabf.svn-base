Imports System.Runtime.CompilerServices
Imports System.Data.SQLite
Imports System.IO
Imports System.Threading

Public Class frmSettings

  'm_relay(1-4) are declared as objects here to use to actuate with webrequests later on
  'conn is your global sqlite connection string
  'iDIN is used to identify your relay numbers so that you can reference the different relays correctly.
  'relay_thread is a thread to do background stuff.

  Public WithEvents m_Relay As DIN3Relay
  Public WithEvents m_Relay2 As DIN3Relay
  Public WithEvents m_Relay3 As DIN3Relay
  Public WithEvents m_Relay4 As DIN3Relay
  Dim conn As New SQLiteConnection(g_sConnString)
  Dim relay_thread As Threading.Thread
  Public iDIN As Integer = 0

  ''' <summary>
  ''' loads some settings from the settings menu that fills some text boxes, pretty straight forward
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    txtUsername.Text = My.Settings.Login
    txtPassword.Text = My.Settings.Password
    txtIP.Text = My.Settings.IPAddress
    txtUsername2.Text = My.Settings.Login2
    txtPassword2.Text = My.Settings.Password2
    txtIP2.Text = My.Settings.IPAddress2
    cmdConnect3.Text = My.Settings.Login3
    txtPassword3.Text = My.Settings.Password3
    txtIP3.Text = My.Settings.IPAddress3
    txtDatabaseFileName.Text = My.Settings.ConnectionString
    Refresh_DGV()
    txtInterval.Text = My.Settings.pump_interval.ToString
    Calculate()
  End Sub

  Private Sub frmSettings_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
    g_Settings.Dispose()
    g_Settings.Close()
    g_Settings = Nothing
  End Sub

#Region "Relay Connection/Accuation"

  ''' <summary>
  ''' used to ping the relay units that are connected so that you can check if they are working.
  ''' </summary>
  ''' <param name="i"></param>
  ''' <remarks></remarks>
  Public Sub Ping_Relays(ByVal i As Integer)
    Try
      If i = 1 Then
        If My.Computer.Network.Ping(My.Settings.IPAddress) Then
          m_Relay.Connect()
          txtDebug.Text = "Ping request successful. Relay module at " & My.Settings.IPAddress.ToString & " is connected."
        Else
          txtDebug.Text = "Ping request to the relay module has timed out. Check the physical connection to the board."
        End If
      ElseIf i = 2 Then
        If My.Computer.Network.Ping(My.Settings.IPAddress2) Then
          m_Relay2.Connect()
          txtDebug.Text = "Ping request successful. Relay module at " & My.Settings.IPAddress2.ToString & " is connected."
        Else
          txtDebug.Text = "Ping request to the relay module has timed out. Check the physical connection to the board."
        End If
      ElseIf i = 3 Then
        If My.Computer.Network.Ping(My.Settings.IPAddress3) Then
          m_Relay3.Connect()
          txtDebug.Text = "Ping request successful. Relay module at " & My.Settings.IPAddress3.ToString & " is connected."
        Else
          txtDebug.Text = "Ping request to the relay module has timed out. Check the physical connection to the board."
        End If
      ElseIf i = 4 Then
        If My.Computer.Network.Ping(My.Settings.IPAddress4) Then
          m_Relay4.Connect()
          txtDebug.Text = "Ping request successful. Relay module at " & My.Settings.IPAddress4.ToString & " is connected."
        Else
          txtDebug.Text = "Ping request to the relay module has timed out. Check the physical connection to the board."
        End If
      End If
    Catch ex As Exception
      txtDebug.Text = ex.Message.ToString
    End Try
  End Sub

  'connection and commanding all the relays to turn on and off is found here
  'this section is mainly for testing to make sure the unit is working correctly and also if you want to drain the lines or clean them

  Private Sub cmdConnect_Click(sender As Object, e As EventArgs) Handles cmdConnect.Click
    txtDebug.Text = ""
    My.Settings.IPAddress = txtIP.Text
    My.Settings.Login = txtUsername.Text
    My.Settings.Password = txtPassword.Text
    My.Settings.Save()
    m_Relay = Nothing
    m_Relay = New DIN3Relay(My.Settings.IPAddress, My.Settings.Login, My.Settings.Password, True, 5000)
    txtCmdSent.Text = Now.ToLocalTime & ": Ping request sent to " & My.Settings.IPAddress
    Ping_Relays(1)
  End Sub

  Private Sub cmdConnect2_Click(sender As Object, e As EventArgs) Handles cmdConnect2.Click
    txtDebug.Text = ""
    My.Settings.IPAddress2 = txtIP2.Text
    My.Settings.Login2 = txtUsername2.Text
    My.Settings.Password2 = txtPassword2.Text
    My.Settings.Save()
    m_Relay2 = Nothing
    m_Relay2 = New DIN3Relay(My.Settings.IPAddress2, My.Settings.Login2, My.Settings.Password2, True, 5000)
    txtCmdSent.Text = Now.ToLocalTime & ": Ping request sent to " & My.Settings.IPAddress2
    Ping_Relays(2)
  End Sub

  Private Sub cmdConnect3_Click(sender As Object, e As EventArgs) Handles cmdConnect3.Click
    txtDebug.Text = ""
    My.Settings.IPAddress3 = txtIP3.Text
    My.Settings.Login3 = cmdConnect3.Text
    My.Settings.Password3 = txtPassword3.Text
    My.Settings.Save()
    m_Relay3 = Nothing
    m_Relay3 = New DIN3Relay(My.Settings.IPAddress3, My.Settings.Login3, My.Settings.Password3, True, 5000)
    txtCmdSent.Text = Now.ToLocalTime & ": Ping request sent to " & My.Settings.IPAddress3
    Ping_Relays(3)
  End Sub

  Private Sub cmdAllOn_Click(sender As Object, e As EventArgs) Handles cmdAllOn.Click
    txtDebug.Text = ""
    m_Relay = Nothing
    m_Relay = New DIN3Relay(My.Settings.IPAddress, My.Settings.Login, My.Settings.Password, True, 5000)
    Try
      m_Relay.Connect()
      m_Relay.Relay_SetAll(True)
    Catch ex As Exception
      MessageBox.Show(ex.Message)
    End Try
  End Sub

  Private Sub cmdAllOn2_Click(sender As Object, e As EventArgs) Handles cmdAllOn2.Click
    txtDebug.Text = ""
    m_Relay2 = Nothing
    m_Relay2 = New DIN3Relay(My.Settings.IPAddress2, My.Settings.Login2, My.Settings.Password2, True, 5000)
    Try
      m_Relay2.Connect()
      m_Relay2.Relay_SetAll(True)
    Catch ex As Exception
      MessageBox.Show(ex.Message)
    End Try
  End Sub

  Private Sub cmdAllOn3_Click(sender As Object, e As EventArgs) Handles cmdAllOn3.Click
    txtDebug.Text = ""
    m_Relay3 = Nothing
    m_Relay3 = New DIN3Relay(My.Settings.IPAddress3, My.Settings.Login3, My.Settings.Password3, True, 5000)
    Try
      m_Relay3.Connect()
      m_Relay3.Relay_SetAll(True)
    Catch ex As Exception
      MessageBox.Show(ex.Message)
    End Try
  End Sub

  Private Sub cmdAllOff_Click(sender As Object, e As EventArgs) Handles cmdAllOff.Click
    txtDebug.Text = ""
    m_Relay = Nothing
    m_Relay = New DIN3Relay(My.Settings.IPAddress, My.Settings.Login, My.Settings.Password, True, 5000)
    Try
      m_Relay.Connect()
      m_Relay.Relay_SetAll(False)
    Catch ex As Exception
      MessageBox.Show(ex.Message)
    End Try
  End Sub

  Private Sub cmdAllOff2_Click(sender As Object, e As EventArgs) Handles cmdAllOff2.Click
    txtDebug.Text = ""
    m_Relay2 = Nothing
    m_Relay2 = New DIN3Relay(My.Settings.IPAddress2, My.Settings.Login2, My.Settings.Password2, True, 5000)
    Try
      m_Relay2.Connect()
      m_Relay2.Relay_SetAll(False)
    Catch ex As Exception
      MessageBox.Show(ex.Message)
    End Try
  End Sub

  Private Sub cmdAllOff3_Click(sender As Object, e As EventArgs) Handles cmdAllOff3.Click
    txtDebug.Text = ""
    m_Relay3 = Nothing
    m_Relay3 = New DIN3Relay(My.Settings.IPAddress3, My.Settings.Login3, My.Settings.Password3, True, 5000)
    Try
      m_Relay3.Connect()
      m_Relay3.Relay_SetAll(False)
    Catch ex As Exception
      MessageBox.Show(ex.Message)
    End Try
  End Sub

  Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
    System.Diagnostics.Process.Start("http://" & My.Settings.IPAddress)
  End Sub

  Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
    System.Diagnostics.Process.Start("http://" & My.Settings.IPAddress2)
  End Sub

  Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
    System.Diagnostics.Process.Start("http://" & My.Settings.IPAddress3)
  End Sub

#End Region

#Region "Flavors Tab"

  Dim SQLDelete As String = "DELETE FROM Ingredients WHERE id = ?"

  'when you enter the row of the ingredients datagridview, you can see what ingredient you have selected and its information
  Private Sub dgvFlavors_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFlavors.RowEnter
    'Retrieve id,name,Volume_Cap from the row
    Dim id As Integer = Integer.Parse(NullToString(dgvFlavors.Rows(e.RowIndex).Cells(0).Value.ToString()))
    Dim Name As String = (NullToString(dgvFlavors.Rows(e.RowIndex).Cells(1).Value))
    txtID.Text = id.ToString()
    txtName.Text = Name
    Dim lstParams As New List(Of String)
    lstParams.Add(txtName.Text)
    Dim dt As DataTable = paramQuery("SELECT id FROM Ingredients WHERE Name = @pname1", lstParams)
    'lstID.DataSource = dt
    'lstID.DisplayMember = "id"
    'lstID.ValueMember = "id"
  End Sub

  Private Sub Clean()
    txtID.Text = ""
    txtName.Text = ""
    txtImageFile.Text = ""
  End Sub

  'there is probably some more work here that needs to be done
  'I haven't figured out a good way to save objects in the database if you are to hot-swap ingredient id's and information
  'definitely something worth looking into 
  Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
    Dim ofdp = OpenFileDialogPicture
    Dim lstParams As New List(Of String)
    lstParams.Add(txtID.Text)
    lstParams.Add(txtName.Text)
    lstParams.Add(Date.Now)
    Dim dr As DialogResult = ImageDialog.ShowDialog
    If dr = DialogResult.Cancel Then
      'Dim i As Integer = insertQuery("UPDATE Ingredients SET id = @pname1, Name = @pname2,Added_on = @pname3 WHERE id = '" + lstID.Text + "'", lstParams)
      Refresh_DGV()
    ElseIf dr = DialogResult.OK Then
      OpenFileDialogPicture.ShowDialog()
      Dim SQLconnect As New SQLite.SQLiteConnection()
      Dim SQLcommand As New SQLiteCommand
      SQLconnect.ConnectionString = g_sConnString
      SQLconnect.Open()
      SQLcommand = SQLconnect.CreateCommand
      SQLcommand.Parameters.Add(ImageToBlob(ofdp.FileName))
      SQLcommand.CommandText = "UPDATE Ingredients SET Image = @image WHERE Name = '" + txtName.Text + "'"
      SQLcommand.ExecuteNonQuery()
      SQLcommand.Dispose()
      SQLconnect.Close()
      SQLconnect.Dispose()
      Refresh_DGV()
      txtImageFile.Text = ""
      txtImageFile.Enabled = True
    End If
  End Sub

  Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
    'check the existence of selected id
    If Not String.IsNullOrEmpty(txtID.Text) Then
      If conn.State <> ConnectionState.Open Then
        conn.Open()
      End If
      'create command from the DELETE statement
      Dim command As SQLiteCommand = conn.CreateCommand()
      command.CommandText = SQLDelete
      'add id as parameter
      command.Parameters.AddWithValue("id", Integer.Parse(txtID.Text))
      'execute the SQL statement
      command.ExecuteNonQuery()
      conn.Close()
      Clean()
      Refresh_DGV()
    End If
  End Sub

  Private Sub btnAddImage_Click(sender As Object, e As EventArgs) Handles btnAddImage.Click
    OpenFileDialogPicture.ShowDialog()
  End Sub

  Private Sub OpenFileDialogPicture_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialogPicture.FileOk
    txtImageFile.Text = OpenFileDialogPicture.FileName
    txtImageFile.Enabled = False
  End Sub

  'this is where you will add new ingredients into the database
  'adds the information to the database first, and then adds the picture to the corresponding information
  Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
    Dim ofdp = OpenFileDialogPicture
    Dim lstParams As New List(Of String)
    lstParams.Add(txtID.Text)
    lstParams.Add(txtName.Text)
    lstParams.Add(Date.Now)
    lstParams.Add("Yes")
    Dim i As Integer = insertQuery("INSERT INTO Ingredients (id,Name,Added_on,Image,Has_Image) VALUES (@pname1,@pname2,@pname3,1,@pname4)", lstParams)
    Dim SQLconnect As New SQLite.SQLiteConnection()
    Dim SQLcommand As New SQLiteCommand
    If txtImageFile.Text = "" Then
      MsgBox("Please choose an image to add to the database with your new flavor.")
      OpenFileDialogPicture.ShowDialog()
    Else
      SQLconnect.ConnectionString = g_sConnString
      SQLconnect.Open()
      SQLcommand = SQLconnect.CreateCommand
      SQLcommand.Parameters.Add(ImageToBlob(ofdp.FileName))
      SQLcommand.CommandText = "UPDATE Ingredients SET Image = @image WHERE Name = '" + txtName.Text + "'"
      SQLcommand.ExecuteNonQuery()
      SQLcommand.Dispose()
      SQLconnect.Close()
      SQLconnect.Dispose()
      Refresh_DGV()
      txtImageFile.Text = ""
      txtImageFile.Enabled = True
    End If
  End Sub

#End Region

#Region "Picture Loading"

  Public Function BlobToImage(ByVal blob)
    Dim mstream As New System.IO.MemoryStream
    Dim pData() As Byte = DirectCast(blob, Byte())
    mstream.Write(pData, 0, Convert.ToInt32(pData.Length))
    Dim bm As Bitmap = New Bitmap(mstream, False)
    mstream.Dispose()
    Return bm
  End Function

  Public Overloads Function ImageToBlob(ByVal filePath As String)
    Dim fs As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
    Dim br As BinaryReader = New BinaryReader(fs)
    Dim bm() As Byte = br.ReadBytes(fs.Length)
    br.Close()
    fs.Close()
    Dim photo() As Byte = bm
    Dim SQLparm As New SQLiteParameter("@image", photo)
    SQLparm.DbType = DbType.Binary
    SQLparm.Value = photo
    Return SQLparm
  End Function

  Public Overloads Function ImageToBlob(ByVal image As Image)
    Dim ms As New MemoryStream()
    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
    Dim photo() As Byte = ms.ToArray()
    Dim SQLparm As New SQLiteParameter("@image", photo)
    SQLparm.DbType = DbType.Binary
    SQLparm.Value = photo
    Return SQLparm
  End Function

#End Region

#Region "Database Connection"

  Private Sub btnChooseNewDB_Click(sender As Object, e As EventArgs) Handles btnChooseNewDB.Click
    OpenFileDialogDatabase.ShowDialog()
  End Sub

  Private Sub OpenFileDialogDatabase_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialogDatabase.FileOk
    txtDatabaseFileName.Text = "Data Source = " & OpenFileDialogDatabase.FileName
  End Sub

  'This sub saves the current connection string in your database textbox as a setting in your settings file. 
  'Also refreshes your page to show the changes the new database has brought.
  Private Sub btnSaveDB_Click(sender As Object, e As EventArgs) Handles btnSaveDB.Click
    My.Settings.ConnectionString = ""
    My.Settings.ConnectionString = txtDatabaseFileName.Text
    g_sConnString = My.Settings.ConnectionString
    MsgBox(txtDatabaseFileName.Text & " is now your database connection string")
    My.Settings.Save()
    Me.Refresh_DGV()
    Me.Show()
  End Sub

  Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
    txtDatabaseFileName.Text = ""
  End Sub

  'this sub creates your new database. All of the red text in the "" is your SQL statements that get created as soon as you choose to create a new database
  Private Sub btnCreateNewDB_Click(sender As Object, e As EventArgs) Handles btnCreateNewDB.Click
    Dim sqlConnection As New SQLite.SQLiteConnection()
    Dim sqlCommand As New SQLiteCommand("", sqlConnection)
    Dim dr As DialogResult = SaveDBLocation.ShowDialog
    Try
      If dr = Windows.Forms.DialogResult.Cancel Then
        MsgBox("No DB created.")
      Else
        sqlConnection.ConnectionString = "Data Source = " & SaveDBLocation.FileName & ".db"
        sqlConnection.Open()
        sqlCommand.CommandText = "CREATE TABLE Ingredients (id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Volume_Cap INTEGER, Added_on VARCHAR(20), Image BLOB, Deleted_At DATETIME);"
        sqlCommand.ExecuteNonQuery()
        sqlCommand.CommandText = "CREATE TABLE Recipes (id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Created_AT VARCHAR(20) NOT NULL, Deleted_At DATETIME);"
        sqlCommand.ExecuteNonQuery()
        sqlCommand.CommandText = "CREATE TABLE [Recipe_Ingredients] ([Recipe_ID] INTEGER(10) NOT NULL, [Ingredient_ID] INTEGER(10) NOT NULL,[Ingredient_Name] VARCHAR(20) NOT NULL, [Volume] DECIMAL(10, 1) NOT NULL);"
        sqlCommand.ExecuteNonQuery()
        sqlCommand.CommandText = "CREATE VIEW Recipes_View AS " & _
                                    "SELECT Recipes.Name AS Recipe, " & _
                                    "Ingredients.name AS Ingredients, " & _
                                    "Recipe_Ingredients.Volume, " & _
                                    "Recipes.Created_AT AS Date " & _
                                    "FROM Recipe_Ingredients " & _
                                    "LEFT JOIN Recipes ON Recipes.[id] = Recipe_Ingredients.[Recipe_ID] " & _
                                    "LEFT JOIN Ingredients ON Ingredients.[id] = Recipe_Ingredients.[Ingredient_ID];"
        sqlCommand.ExecuteNonQuery()
        sqlConnection.Close()
        sqlConnection.Dispose()
        MsgBox("Database successfully created.")
      End If
    Catch ex As Exception
      Debug.WriteLine(ex.Message)
    End Try
  End Sub

  Public Sub Refresh_DGV()
    Me.DsIngredients1.Clear()
    'dgvFlavors.Rows.Clear()
    Dim bRet As Boolean = False
    Dim sErr As String = ""
    Dim sSQL As String = "SELECT id,Name,Added_on,Has_Image FROM Ingredients"
    bRet = DBQueryToDataSet(sSQL, Me.DsIngredients1, "dtIngredients", g_sConnString, True, sErr)
  End Sub

#End Region

#Region "Settings"

  Private Sub btnCalculate_Click(sender As Object, e As EventArgs) Handles btnCalculate.Click
    Calculate()
    My.Settings.pump_interval = txtInterval.Text
    My.Settings.Save()
  End Sub

  Public Sub Calculate()
    txt2int.Text = (txtInterval.Text * 2)
    txt3int.Text = (txtInterval.Text * 3)
    txt4int.Text = (txtInterval.Text * 4)
    txt5int.Text = (txtInterval.Text * 5)
    txt6int.Text = (txtInterval.Text * 6)
  End Sub

  Private Sub lstBoxRelayMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstBoxRelayMode.SelectedIndexChanged
    Dim iRelayMode As Integer = lstBoxRelayMode.SelectedIndex + 1
    My.Settings.Relay_Mode = iRelayMode.ToString
    My.Settings.Save()
  End Sub

  'makes font size of the datagridview bigger
  Private Sub btnPlus_Click(sender As Object, e As EventArgs) Handles btnPlus.Click
    Dim sFontsize As String = dgvFlavors.DefaultCellStyle.Font.Size.ToString
    If sFontsize < 48 Then
      sFontsize = sFontsize + 1
    ElseIf sFontsize >= 48 Then
      sFontsize = sFontsize
    End If
    dgvFlavors.DefaultCellStyle.Font = New Font("Microsoft Sans Serif", sFontsize)
    dgvFlavors.AlternatingRowsDefaultCellStyle.Font = New Font("Microsoft Sans Serif", sFontsize)
    dgvFlavors.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
    dgvFlavors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
  End Sub

  'makes font size of the datagridview smaller
  Private Sub btnMinus_Click(sender As Object, e As EventArgs) Handles btnMinus.Click
    Dim sFontsize As String = dgvFlavors.DefaultCellStyle.Font.Size.ToString
    If sFontsize > 5 Then
      sFontsize = sFontsize - 1
    ElseIf sFontsize <= 5 Then
      sFontsize = sFontsize
    End If
    dgvFlavors.DefaultCellStyle.Font = New Font("Microsoft Sans Serif", sFontsize)
    dgvFlavors.AlternatingRowsDefaultCellStyle.Font = New Font("Microsoft Sans Serif", sFontsize)
    dgvFlavors.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
    dgvFlavors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
  End Sub

#End Region

#Region "Delegate Subs"

  Private Delegate Sub DelegateCommandResult(ByRef bResult As Boolean, ByRef strResult As String, ByRef strSourceAddress As String)
  Private Delegate Sub DelegateCommandSent(ByRef strSourceAddress As String)

  Private Sub Relay_CommandResult(ByRef bResult As Boolean, ByRef strResult As String, ByRef strSourceAddress As String) Handles m_Relay.CommandResult, m_Relay2.CommandResult, m_Relay3.CommandResult
    If Me.InvokeRequired Then
      Me.Invoke(New DelegateCommandResult(AddressOf Relay_CommandResult), New Object() {bResult, strResult, strSourceAddress})
    Else
      Dim strDebug As String = Now.ToLocalTime & " (" & bResult.ToString.Substring(0, 1) & ") " & strResult
      txtDebug.Text = strDebug & vbCrLf & txtDebug.Text
    End If
  End Sub

  Private Sub Relay_CommandSent(ByRef strSourceAddress As String) Handles m_Relay.CommandSent, m_Relay2.CommandSent, m_Relay3.CommandSent
    If Me.InvokeRequired Then
      Me.Invoke(New DelegateCommandSent(AddressOf Relay_CommandSent), New Object() {strSourceAddress})
    Else
      txtCmdSent.Text = Now.ToLocalTime & " : " & strSourceAddress
    End If
  End Sub

#End Region

  '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'
  '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'
  'Todo: Haven't gotten this section to work quite right yet
  'Basically, trying to drag rows over from one datagridview to another so that the id of the flavor could be adjusted on the fly
  'This would allow you to quickly move around flavors to different pumps depending on the stock of the flavor

  Private fromIndex As Integer
  Private dragIndex As Integer
  Private dragRect As Rectangle

  Private Sub dgvFlavors_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles dgvFlavors.MouseDown
    Dim sName As String = dgvFlavors.CurrentCell.Value
    If e.Button = Windows.Forms.MouseButtons.Left Then
      If dgvFlavors.CurrentCell IsNot Nothing AndAlso dgvFlavors.CurrentCell.Value IsNot Nothing Then
        DoDragDrop(sName, DragDropEffects.Copy)
      End If
    End If
  End Sub

  Private Sub dgv_pumps_DragDrop(sender As Object, e As DragEventArgs) Handles dgv_pumps.DragDrop
    If e.Data IsNot Nothing Then
      Dim text As String = e.Data.GetData(DataFormats.Text)
      Dim clientPoint As Point = dgv_pumps.PointToClient(New Point(e.X, e.Y))
      Dim info As DataGridView.HitTestInfo = dgv_pumps.HitTest(clientPoint.X, clientPoint.Y)
      If info.Type = DataGridViewHitTestType.Cell Then
        dgv_pumps.Rows(info.RowIndex).Cells(info.ColumnIndex).Value = text
      End If
    End If
    '   Dim p As Point = dgv_pumps.PointToClient(New Point(e.X, e.Y))
    '   dragIndex = dgv_pumps.HitTest(p.X, p.Y).RowIndex
    '   If (e.Effect = DragDropEffects.Move) Then
    '     Dim dragRow As DataGridViewRow = e.Data.GetData(GetType(DataGridViewRow))
    '     If dragIndex < 0 Then dragIndex = dgv_pumps.RowCount - 1
    '     dgv_pumps.Rows.RemoveAt(fromIndex)
    '     dgv_pumps.Rows.Insert(dragIndex, dragRow)
    '   End If
  End Sub

  Private Sub dgv_pumps_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs) Handles dgv_pumps.DragEnter
    If (e.Data.GetDataPresent(DataFormats.Text)) Then
      e.Effect = DragDropEffects.Copy
    Else
      e.Effect = DragDropEffects.None
    End If
  End Sub

  Private Sub dgv_pumps_DragOver(ByVal sender As Object, ByVal e As DragEventArgs)
    e.Effect = DragDropEffects.Move
  End Sub

  Private Sub dgv_pumps_MouseDown(sender As Object, e As MouseEventArgs)
    fromIndex = dgv_pumps.HitTest(e.X, e.Y).RowIndex
    If fromIndex > -1 Then
      Dim dragSize As Size = SystemInformation.DragSize
      dragRect = New Rectangle(New Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize)
    Else
      dragRect = Rectangle.Empty
    End If
  End Sub

  Private Sub dgv_pumps_MouseMove(sender As Object, e As MouseEventArgs)
    If (e.Button And MouseButtons.Left) = Windows.Forms.MouseButtons.Left Then
      If (dragRect <> Rectangle.Empty AndAlso Not dragRect.Contains(e.X, e.Y)) Then
        dgv_pumps.DoDragDrop(dgv_pumps.Rows(fromIndex), DragDropEffects.Move)
      End If
    End If
  End Sub

  Public Function Add_DIN()
    If iDIN = 0 Then
      Dim row As New DataGridViewRow
      For i As Integer = 0 To 7
        Dim pumpnum As Integer = i + 1
        dgv_pumps.Rows.Insert(i, New String() {"1", pumpnum})
      Next
      iDIN += 1
    ElseIf iDIN = 1 Then
      Dim row As New DataGridViewRow
      For i As Integer = 0 To 7
        Dim pumpnum As Integer = i + 1
        dgv_pumps.Rows.Insert(i, New String() {"2", pumpnum})
      Next
      iDIN += 1
    ElseIf iDIN = 2 Then
      Dim row As New DataGridViewRow
      For i As Integer = 0 To 7
        Dim pumpnum As Integer = i + 1
        dgv_pumps.Rows.Insert(i, New String() {"3", pumpnum})
      Next
      iDIN += 1
    ElseIf iDIN = 3 Then
      Dim row As New DataGridViewRow
      For i As Integer = 0 To 7
        Dim pumpnum As Integer = i + 1
        dgv_pumps.Rows.Insert(i, New String() {"4", pumpnum})
      Next
      iDIN += 1
    End If
    Return iDIN
  End Function

  Private Sub btnAddDIN_Click(sender As Object, e As EventArgs) Handles btnAddDIN.Click
    Add_DIN()
  End Sub
  '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'
  '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'

  Private Sub dgvFlavors_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFlavors.CellContentClick

  End Sub
End Class
