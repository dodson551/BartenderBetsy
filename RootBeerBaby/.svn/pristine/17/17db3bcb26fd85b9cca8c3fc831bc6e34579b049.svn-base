Imports System.Collections
Imports System.Runtime.CompilerServices
Imports System.Data.SQLite
Imports System.IO
Imports System.Threading

Public Class frmCreate_Flave

  ' This is where all of my variables are initialized.
  ' m_relay(1-4) are all objects that I am declaring to use as my different relays.
  ' myThread is set up as a thread to later use to process some information in the background.
  ' m_interval is an integer used for counting through a loop.
  Public WithEvents m_relay As DIN3Relay
  Public WithEvents m_relay2 As DIN3Relay
  Public WithEvents m_relay3 As DIN3Relay
  Public WithEvents m_relay4 As DIN3Relay
  Dim myThread As Threading.Thread
  Public m_interval As Integer = 1

#Region "Form Load and Subs"

  'when the page is loaded, the things in this section run and set up the page for the user
  Private Sub frmCreate_Flave_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    tTimer2.Start()
    Dim sqlconnect As New SQLite.SQLiteConnection()
    Dim sqlcommand As SQLiteCommand
    sqlconnect.ConnectionString = g_sConnString
    sqlconnect.Open()
    sqlcommand = sqlconnect.CreateCommand
    sqlcommand.CommandText = "SELECT Image FROM Ingredients WHERE id = 1"
    Dim sqlreader As SQLiteDataReader = sqlcommand.ExecuteReader()
    While sqlreader.Read()
      PictureBox6.Image = BlobToImage(sqlreader("image"))
    End While
    sqlcommand.Dispose()
    sqlconnect.Close()
    sqlconnect.Dispose()
    lstIDbox.Text = 1
    get_info()
    btnFlavor.Text = lstBoxIngs.SelectedValue.ToString
    Timer1.Interval = My.Settings.pump_interval
  End Sub

  Private Sub frmCreate_Flave_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
    tTimer2.Stop()
    tTimer2.Dispose()
    g_Create_Flave.Dispose()
    g_Create_Flave.Close()
    g_Create_Flave = Nothing
  End Sub

  Private Sub tTimer2_Tick(sender As Object, e As EventArgs) Handles tTimer2.Tick
    If My.Settings.UserName = "Public" Then
      Me.Text = "Logged in as public user"
    Else
      Me.Text = "Logged in as user: " & My.Settings.UserName.ToString
    End If
  End Sub

  ''' <summary>
  ''' this grabs the image from the database that is associated with the ingredient that is selected.
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub lstBoxIngs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstBoxIngs.SelectedIndexChanged
    ListBox1.Text = lstBoxIngs.SelectedValue.ToString
    Dim sqlconnect As New SQLite.SQLiteConnection()
    Dim sqlcommand As SQLiteCommand
    sqlconnect.ConnectionString = g_sConnString
    sqlconnect.Open()
    sqlcommand = sqlconnect.CreateCommand
    sqlcommand.CommandText = "SELECT Image FROM Ingredients WHERE Name = '" + ListBox1.Text + "'"
    Dim sqlreader As SQLiteDataReader = sqlcommand.ExecuteReader()
    While sqlreader.Read()
      PictureBox6.Image = BlobToImage(sqlreader("image"))
    End While
    sqlcommand.Dispose()
    sqlconnect.Close()
    sqlconnect.Dispose()
    btnFlavor.Text = lstBoxIngs.SelectedValue.ToString
    lstIngName.Text = lstBoxIngs.SelectedValue.ToString
    Dim lstParams As New List(Of String)
    lstParams.Add(btnFlavor.Text)
    Dim dt As DataTable = paramQuery("SELECT id FROM Ingredients WHERE Name = @pname1", lstParams)
    lstIDbox.DataSource = dt
    lstIDbox.DisplayMember = "id"
    lstIDbox.ValueMember = "id"
  End Sub

  ''' <summary>
  ''' this grabs information about the ingredients from the database and displays them according to what relay mode you are operating in.
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub get_info()
    If My.Settings.Relay_Mode = "1" Then
      Dim dt As DataTable = runQuery("SELECT Name FROM Ingredients WHERE id < 9")
      lstBoxIngs.DataSource = dt
      lstBoxIngs.ValueMember = "Name"
      lstBoxIngs.DisplayMember = "Name"
    ElseIf My.Settings.Relay_Mode = "2" Then
      Dim dt As DataTable = runQuery("SELECT Name FROM Ingredients WHERE id < 17")
      lstBoxIngs.DataSource = dt
      lstBoxIngs.ValueMember = "Name"
      lstBoxIngs.DisplayMember = "Name"
    ElseIf My.Settings.Relay_Mode = "3" Then
      Dim dt As DataTable = runQuery("SELECT Name FROM Ingredients WHERE id < 25")
      lstBoxIngs.DataSource = dt
      lstBoxIngs.ValueMember = "Name"
      lstBoxIngs.DisplayMember = "Name"
    ElseIf My.Settings.Relay_Mode = "4" Then
      Dim dt As DataTable = runQuery("SELECT Name FROM Ingredients WHERE id < 33")
      lstBoxIngs.DataSource = dt
      lstBoxIngs.ValueMember = "Name"
      lstBoxIngs.DisplayMember = "Name"
    End If
  End Sub

#End Region

#Region "Recipe Creation"

  'Quite a bit happens when you click the create recipe button.
  'Firstly, the software checks the connection to the relay by pinging the unit and makes sure it is detected on the network. 
  'If this is successful, it sets up the relay unit as a new object to be used to create your physical recipe
  'The second step is that the Recipe_Dialog is opened, see further explanation under that sub

    Public Sub btnCreateRecipe_Click(sender As Object, e As EventArgs) Handles btnCreateRecipe.Click
    If lstViewCreatedRecipe.Items.Count = 0 Then
      MsgBox("Add ingredients to the recipe you would like to use.")
    End If
    If My.Settings.Relay_Mode = "1" Then
      If My.Computer.Network.Ping(My.Settings.IPAddress) Then
        m_relay = New DIN3Relay(My.Settings.IPAddress, My.Settings.Login, My.Settings.Password, True, 5000)
        m_relay.Connect()
        Recipe_Dialog()
      Else
        MsgBox("Board connection error. Check physical connections. Redirecting to settings menu.")
        Settings()
      End If
    ElseIf My.Settings.Relay_Mode = "2" Then
      If My.Computer.Network.Ping(My.Settings.IPAddress2) And My.Computer.Network.Ping(My.Settings.IPAddress) Then
        m_relay = New DIN3Relay(My.Settings.IPAddress, My.Settings.Login, My.Settings.Password, True, 5000)
        m_relay.Connect()
        m_relay2 = New DIN3Relay(My.Settings.IPAddress2, My.Settings.Login2, My.Settings.Password2, True, 5000)
        m_relay2.Connect()
        Recipe_Dialog()
      Else
        MsgBox("Board connection error. Check physical connections. Redirecting to settings menu.")
        Settings()
      End If
    ElseIf My.Settings.Relay_Mode = "3" Then
      If My.Computer.Network.Ping(My.Settings.IPAddress3) And My.Computer.Network.Ping(My.Settings.IPAddress2) And My.Computer.Network.Ping(My.Settings.IPAddress) Then
        m_relay = New DIN3Relay(My.Settings.IPAddress, My.Settings.Login, My.Settings.Password, True, 5000)
        m_relay.Connect()
        m_relay2 = New DIN3Relay(My.Settings.IPAddress2, My.Settings.Login2, My.Settings.Password2, True, 5000)
        m_relay2.Connect()
        m_relay3 = New DIN3Relay(My.Settings.IPAddress3, My.Settings.Login3, My.Settings.Password3, True, 5000)
        m_relay3.Connect()
        Recipe_Dialog()
      Else
        MsgBox("Board connection error. Check physical connections. Redirecting to settings menu.")
        Settings()
      End If
    ElseIf My.Settings.Relay_Mode = "4" Then
      If My.Computer.Network.Ping(My.Settings.IPAddress4) And My.Computer.Network.Ping(My.Settings.IPAddress3) And My.Computer.Network.Ping(My.Settings.IPAddress2) And My.Computer.Network.Ping(My.Settings.IPAddress) Then
        m_relay = New DIN3Relay(My.Settings.IPAddress, My.Settings.Login, My.Settings.Password, True, 5000)
        m_relay.Connect()
        m_relay2 = New DIN3Relay(My.Settings.IPAddress2, My.Settings.Login2, My.Settings.Password2, True, 5000)
        m_relay2.Connect()
        m_relay3 = New DIN3Relay(My.Settings.IPAddress3, My.Settings.Login3, My.Settings.Password3, True, 5000)
        m_relay3.Connect()
        m_relay4 = New DIN3Relay(My.Settings.IPAddress, My.Settings.Login, My.Settings.Password, True, 5000)
        m_relay4.Connect()
        Recipe_Dialog()
      Else
        MsgBox("Board connection error. Check physical connections. Redirecting to settings menu.")
        Settings()
      End If
    End If
  End Sub

  ''' <summary>
  ''' Opens a dialog asking if you would for sure like to create the recipe.
  ''' If yes, it will move onto the Create_Recipe sub
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub Recipe_Dialog()
    Dim dr As DialogResult = confirmDialog.ShowDialog
    If dr = Windows.Forms.DialogResult.OK Then
      Create_Recipe()
    ElseIf dr = DialogResult.Cancel Then
      confirmDialog.Close()
    End If
  End Sub

  ' This sub is the meat and potatoes of what actually creates the drinks. 
  ' First, you define the sRequest as the beginning of the webrequest that you will send to the relay
  ' Next, it checks the id of the ingredients and then associates these with a relay id
  ' Then, there is some math done that determines how long each relay should stay on so that the correct amount is dispensed
  ' From here, the sRequest turns the correct relay units on, and then runs through the Calculate function, which turns off the relays at the right time
  ' The rate_drink from pops up so that you can make notes and rate the recipe that you just created

  Public Sub Create_Recipe()
    Dim iRelay As Integer
    Dim sRequest1 As String = "{0}outlet?"
    Dim sRequest2 As String = "{0}outlet?"
    Dim sRequest3 As String = "{0}outlet?"
    Dim sRequest4 As String = "{0}outlet?"
    For Each item As ListViewItem In Me.lstViewCreatedRecipe.Items
      iRelay = item.Text
      If iRelay <= 8 Then
        sRequest1 = sRequest1 & "&" & iRelay & "={1}"
      ElseIf iRelay > 8 And iRelay <= 16 Then
        iRelay = iRelay - 8
        sRequest2 = sRequest2 & "&" & iRelay & "={1}"
      ElseIf iRelay > 16 And iRelay <= 24 Then
        iRelay = iRelay - 16
        sRequest3 = sRequest3 & "&" & iRelay & "={1}"
      ElseIf iRelay > 24 And iRelay <= 32 Then
        iRelay = iRelay - 24
        sRequest4 = sRequest4 & "&" & iRelay & "={1}"
      End If
    Next
    Dim tTotal As Integer
    If tbAmount.Value = 2 Then
      tTotal = 4 * My.Settings.pump_interval
      My.Settings.total_time = tTotal
    ElseIf tbAmount.Value = 10 Then
      tTotal = 16 * My.Settings.pump_interval
      My.Settings.total_time = tTotal
    ElseIf tbAmount.Value = 18 Then
      tTotal = 32 * My.Settings.pump_interval
      My.Settings.total_time = tTotal
    ElseIf tbAmount.Value = 26 Then
      tTotal = 48 * My.Settings.pump_interval
      My.Settings.total_time = tTotal
    ElseIf tbAmount.Value = 34 Then
      tTotal = 64 * My.Settings.pump_interval
      My.Settings.total_time = tTotal
    End If
    Try
      If My.Settings.Relay_Mode = "1" Then
        m_relay.Relay_Set_ID(sRequest1, "ON")
        Calculate(lstViewCreatedRecipe, tTotal, frm:=Me)
        Rate_Drink(lstViewCreatedRecipe, frm:=Me)
        lstViewCreatedRecipe.Items.Clear()
      ElseIf My.Settings.Relay_Mode = "2" Then
        m_relay.Relay_Set_ID(sRequest1, "ON")
        m_relay2.Relay_Set_ID(sRequest2, "ON")
        Calculate(lstViewCreatedRecipe, tTotal, frm:=Me)
        Rate_Drink(lstViewCreatedRecipe, frm:=Me)
        lstViewCreatedRecipe.Items.Clear()
      ElseIf My.Settings.Relay_Mode = "3" Then
        m_relay.Relay_Set_ID(sRequest1, "ON")
        m_relay2.Relay_Set_ID(sRequest2, "ON")
        m_relay3.Relay_Set_ID(sRequest3, "ON")
        Calculate(lstViewCreatedRecipe, tTotal, frm:=Me)
        Rate_Drink(lstViewCreatedRecipe, frm:=Me)
        lstViewCreatedRecipe.Items.Clear()
      ElseIf My.Settings.Relay_Mode = "4" Then
        m_relay.Relay_Set_ID(sRequest1, "ON")
        m_relay2.Relay_Set_ID(sRequest2, "ON")
        m_relay3.Relay_Set_ID(sRequest3, "ON")
        m_relay4.Relay_Set_ID(sRequest4, "ON")
        Calculate(lstViewCreatedRecipe, tTotal, frm:=Me)
        Rate_Drink(lstViewCreatedRecipe, frm:=Me)
        lstViewCreatedRecipe.Items.Clear()
      End If
    Catch ex As Exception
      MessageBox.Show(ex.Message)
    End Try
  End Sub

#End Region

#Region "Quantity button clicks"

  ' There is some math here that is done so that you are able to add specific part amounts to your recipe.
  ' This add_lst_item sub will catch any ingredients that already have been added and will add more parts to them accordingly.

  Public Sub add_lst_item()
    Dim bFound As Boolean = False
    For i As Integer = 0 To lstViewCreatedRecipe.Items.Count - 1
      Dim itm As ListViewItem = lstViewCreatedRecipe.Items(i)
      If lstIngName.Text = itm.SubItems(1).Text Then
        Dim cPart As Double = Val(itm.SubItems(2).Text)
        Dim addPart As Double = Val(txtPart.Text)
        Dim newPart As Double
        newPart = cPart + addPart
        itm.SubItems.Item(2).Text = newPart
        bFound = True
        Exit For
      End If
    Next
    If bFound = False Then
      Dim itm As New ListViewItem
      itm = lstViewCreatedRecipe.Items.Add(lstIDbox.Text)
      itm.SubItems.Add(btnFlavor.Text)
      itm.SubItems.Add(txtPart.Text)
    End If
  End Sub

  Private Sub btnAm1_Click(sender As Object, e As EventArgs) Handles btnAm1.Click
    lstIngName.Text = btnFlavor.Text
    txtPart.Text = "1"
    add_lst_item()
  End Sub

  Private Sub btnAm2_Click(sender As Object, e As EventArgs) Handles btnAm2.Click
    lstIngName.Text = btnFlavor.Text
    txtPart.Text = "2"
    add_lst_item()
  End Sub

  Private Sub btnAm3_Click(sender As Object, e As EventArgs) Handles btnAm3.Click
    lstIngName.Text = btnFlavor.Text
    txtPart.Text = "3"
    add_lst_item()
  End Sub

  Private Sub btnAm4_Click(sender As Object, e As EventArgs) Handles btnAm4.Click
    lstIngName.Text = btnFlavor.Text
    txtPart.Text = "4"
    add_lst_item()
  End Sub

  Private Sub btnAm5_Click(sender As Object, e As EventArgs) Handles btnAm5.Click
    lstIngName.Text = btnFlavor.Text
    txtPart.Text = "5"
    add_lst_item()
  End Sub

  Private Sub btnAm6_Click(sender As Object, e As EventArgs) Handles btnAm6.Click
    lstIngName.Text = btnFlavor.Text
    txtPart.Text = "6"
    add_lst_item()
  End Sub

  Private Sub btnAm7_Click(sender As Object, e As EventArgs) Handles btnAm7.Click
    lstIngName.Text = btnFlavor.Text
    txtPart.Text = "7"
    add_lst_item()
  End Sub

  Private Sub btnAm8_Click(sender As Object, e As EventArgs) Handles btnAm8.Click
    lstIngName.Text = btnFlavor.Text
    txtPart.Text = "8"
    add_lst_item()
  End Sub

  Private Sub btnAm9_Click(sender As Object, e As EventArgs) Handles btnAm9.Click
    lstIngName.Text = btnFlavor.Text
    txtPart.Text = "9"
    add_lst_item()
  End Sub

#End Region

#Region "Page Functions"

  'This whole region is comprised of basic page navigation and other basic page/UI functions.
  'The blobtoimage stuff is just to convert the image from the database, which is stored as a blob, into an image visible on the form

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

  Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
    For Each i As ListViewItem In lstViewCreatedRecipe.SelectedItems
      lstViewCreatedRecipe.Items.Remove(i)
    Next
  End Sub

  Private Sub btnClearAll_Click(sender As Object, e As EventArgs) Handles btnClearAll.Click
    lstViewCreatedRecipe.Items.Clear()
  End Sub

  Public Sub lstBoxIngs_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles lstBoxIngs.DrawItem
    e.DrawBackground()
    If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
      e.Graphics.FillRectangle(Brushes.RosyBrown, e.Bounds)
    End If
    Using b As New SolidBrush(e.ForeColor)
      e.Graphics.DrawString(lstBoxIngs.GetItemText(lstBoxIngs.Items(e.Index)), e.Font, b, e.Bounds)
    End Using
    e.DrawFocusRectangle()
  End Sub

  Private Sub btnFavorite_Click(sender As Object, e As EventArgs) Handles btnFavorite.Click
    My_Faves()
    Me.Close()
  End Sub

  Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
    Settings()
  End Sub

  Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
    Main_Page()
    Me.Close()
  End Sub

  Private Sub btnUsers_Click(sender As Object, e As EventArgs) Handles btnUsers.Click
    frmLogin.Show()
  End Sub

  Private Sub btnRefPage_Click(sender As Object, e As EventArgs) Handles btnRefPage.Click
    Me.frmCreate_Flave_Load(Nothing, Nothing)
  End Sub

  Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
    tbAmount.Value = 2
  End Sub

  Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
    tbAmount.Value = 10
  End Sub

  Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
    tbAmount.Value = 18
  End Sub

  Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
    tbAmount.Value = 26
  End Sub

  Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click
    tbAmount.Value = 34
  End Sub

#End Region

End Class