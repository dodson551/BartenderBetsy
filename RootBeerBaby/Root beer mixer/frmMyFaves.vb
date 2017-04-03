Imports System.Data.SQLite
Imports System.Web.Script.Serialization
Imports System.Data.Common
Imports System.IO

Public Class frmMyFaves

  'm_relay(1-4) are your relay objects that you will use to create your recipes 
  'm_interval is your counter variable that will help you keep track of loops
  'myThread will be used later to do background work for this form

  Public WithEvents m_relay As DIN3Relay
  Public WithEvents m_relay2 As DIN3Relay
  Public WithEvents m_relay3 As DIN3Relay
  Public WithEvents m_relay4 As DIN3Relay
  Public m_interval As Integer = 1
  Dim myThread As Threading.Thread

#Region "Form Load and basic subs"

  Private Sub frmMyFaves_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    tTimer2.Start()
    Update_Recipes()
  End Sub

  Private Sub frmMyFaves_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
    tTimer2.Stop()
    tTimer2.Dispose()
    g_My_Faves.Dispose()
    g_My_Faves.Close()
    g_My_Faves = Nothing
  End Sub

  Private Sub tTimer2_Tick(sender As Object, e As EventArgs) Handles tTimer2.Tick
    If My.Settings.UserName = "Public" Then
      Me.Text = "Logged in as public user"
    Else
      Me.Text = "Logged in as user: " & My.Settings.UserName.ToString
    End If
  End Sub

  ''' <summary>
  ''' Checks to see whether or not a recipe will be displayed in the recipes box based on its permision level (public or user based).
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub Update_Recipes()
    getrecipes_info()
    If My.Settings.UserName = "Public" Then
      Dim dt As DataTable = runQuery("SELECT Name, id FROM Recipes WHERE Public = 'True' and Deleted_At IS NULL")
      lstRecipesBox.ValueMember = "Name"
      lstRecipesBox.DisplayMember = "Name"
      lstRecipesBox.DataSource = dt
    Else
      Dim dt As DataTable = runQuery("SELECT Name, id FROM Recipes WHERE User_ID = '" & My.Settings.UserID & "' and Deleted_At IS NULL")
      lstRecipesBox.ValueMember = "Name"
      lstRecipesBox.DisplayMember = "Name"
      lstRecipesBox.DataSource = dt
    End If
    If lstRecipesBox.Text = Nothing Then
      lstRecipesBox.Enabled = False
    Else
      lstRecipesBox.Enabled = True
    End If
  End Sub

  ''' <summary>
  ''' Fills a specified listview based on parameters and returns the ingredient id, name, and amount of parts added to the recipe.
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub getrecipes_info()
    lstViewInfo.Items.Clear()
    If My.Settings.Relay_Mode = "1" Then
      Dim sSQL As String = "SELECT Ingredient_ID, Ingredient_Name, Parts FROM Recipe_Ingredients WHERE Ingredient_ID < 9 and Recipe_ID = '" & lstID.Text & "'"
      Fill_ListView(sSQL, lstViewInfo)
    ElseIf My.Settings.Relay_Mode = "2" Then
      Dim sSQL As String = "SELECT Ingredient_ID, Ingredient_Name, Parts FROM Recipe_Ingredients WHERE Ingredient_ID < 17 and Recipe_ID = '" & lstID.Text & "'"
      Fill_ListView(sSQL, lstViewInfo)
    ElseIf My.Settings.Relay_Mode = "3" Then
      Dim sSQL As String = "SELECT Ingredient_ID, Ingredient_Name, Parts FROM Recipe_Ingredients WHERE Ingredient_ID < 25 and Recipe_ID = '" & lstID.Text & "'"
      Fill_ListView(sSQL, lstViewInfo)
    ElseIf My.Settings.Relay_Mode = "4" Then
      Dim sSQL As String = "SELECT Ingredient_ID, Ingredient_Name, Parts FROM Recipe_Ingredients WHERE Ingredient_ID < 33 and Recipe_ID = '" & lstID.Text & "'"
      Fill_ListView(sSQL, lstViewInfo)
    End If
  End Sub

#End Region

#Region "Passing Recipes to Relay"

  'this sub checks the rating of the recipe from the drink and displays it on the page
  Private Sub lstRecipesBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstRecipesBox.SelectedIndexChanged
    Try
      Dim lstParams As New List(Of String)
      lstParams.Add(lstRecipesBox.SelectedValue)
      Dim dt As DataTable = paramQuery("SELECT id, Rating FROM Recipes WHERE Name LIKE @pname1", lstParams)
      lstID.DataSource = dt
      lstID.DisplayMember = "id"
      lstID.ValueMember = "id"
      lstRating.DataSource = dt
      lstRating.DisplayMember = "Rating"
      lstRating.ValueMember = "Rating"
      If lstRating.Text = 0 Then
        pb1.Image = My.Resources.star_grey
        pb2.Image = My.Resources.star_grey
        pb3.Image = My.Resources.star_grey
        pb4.Image = My.Resources.star_grey
        pb5.Image = My.Resources.star_grey
      ElseIf lstRating.Text = 1 Then
        pb1.Image = My.Resources.star_gold
        pb2.Image = My.Resources.star_grey
        pb3.Image = My.Resources.star_grey
        pb4.Image = My.Resources.star_grey
        pb5.Image = My.Resources.star_grey
      ElseIf lstRating.Text = 2 Then
        pb1.Image = My.Resources.star_gold
        pb2.Image = My.Resources.star_gold
        pb3.Image = My.Resources.star_grey
        pb4.Image = My.Resources.star_grey
        pb5.Image = My.Resources.star_grey
      ElseIf lstRating.Text = 3 Then
        pb1.Image = My.Resources.star_gold
        pb2.Image = My.Resources.star_gold
        pb3.Image = My.Resources.star_gold
        pb4.Image = My.Resources.star_grey
        pb5.Image = My.Resources.star_grey
      ElseIf lstRating.Text = 4 Then
        pb1.Image = My.Resources.star_gold
        pb2.Image = My.Resources.star_gold
        pb3.Image = My.Resources.star_gold
        pb4.Image = My.Resources.star_gold
        pb5.Image = My.Resources.star_grey
      ElseIf lstRating.Text = 5 Then
        pb1.Image = My.Resources.star_gold
        pb2.Image = My.Resources.star_gold
        pb3.Image = My.Resources.star_gold
        pb4.Image = My.Resources.star_gold
        pb5.Image = My.Resources.star_gold
      End If
      getrecipes_info()
    Catch ex As Exception
    End Try
  End Sub

  'Quite a bit happens when you click the use recipe button.
  'Firstly, the software checks the connection to the relay by pinging the unit and makes sure it is detected on the network. 
  'If this is successful, it sets up the relay unit as a new object to be used to create your physical recipe
  'The second step is that the Recipe_Dialog is opened, see further explanation under that sub

  Private Sub btnUseRecipe_Click(sender As Object, e As EventArgs) Handles btnUseRecipe.Click
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

  Public Sub Create_Recipe()
    pBarRecipe.Value = 0
    Dim iRelay As Integer
    Dim sRequest As String = "{0}outlet?"
    Dim sRequest2 As String = "{0}outlet?"
    Dim sRequest3 As String = "{0}outlet?"
    Dim sRequest4 As String = "{0}outlet?"
    For Each item As ListViewItem In Me.lstViewInfo.Items
      iRelay = item.Text
      If iRelay <= 8 Then
        sRequest = sRequest & "&" & iRelay & "={1}"
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
        m_relay.Relay_Set_ID(sRequest, "ON")
        Calculate(lstViewInfo, tTotal, frm:=Me)
      ElseIf My.Settings.Relay_Mode = "2" Then
        m_relay.Relay_Set_ID(sRequest, "ON")
        m_relay2.Relay_Set_ID(sRequest2, "ON")
        Calculate(lstViewInfo, tTotal, frm:=Me)
      ElseIf My.Settings.Relay_Mode = "3" Then
        m_relay.Relay_Set_ID(sRequest, "ON")
        m_relay2.Relay_Set_ID(sRequest2, "ON")
        m_relay3.Relay_Set_ID(sRequest3, "ON")
        Calculate(lstViewInfo, tTotal, frm:=Me)
      ElseIf My.Settings.Relay_Mode = "4" Then
        m_relay.Relay_Set_ID(sRequest, "ON")
        m_relay2.Relay_Set_ID(sRequest2, "ON")
        m_relay3.Relay_Set_ID(sRequest3, "ON")
        m_relay4.Relay_Set_ID(sRequest4, "ON")
        Calculate(lstViewInfo, tTotal, frm:=Me)
      End If
    Catch ex As Exception
      MessageBox.Show(ex.Message)
    End Try
  End Sub

#End Region

#Region "Page Nav"

  'This whole region is comprised of basic page navigation and other basic page/UI functions.
  'The blobtoimage stuff is just to convert the image from the database, which is stored as a blob, into an image visible on the form

  Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
    Settings()
  End Sub

  Private Sub btnNewFlave_Click(sender As Object, e As EventArgs) Handles btnNewFlave.Click
    Create_Flave()
    Me.Close()
  End Sub

  Private Sub btnMainPage_Click(sender As Object, e As EventArgs) Handles btnMainPage.Click
    Main_Page()
    Me.Close()
  End Sub

  Private Sub btnAddRecipe_Click_1(sender As Object, e As EventArgs) Handles btnAddRecipe.Click
    Add_New()
    Me.Close()
  End Sub

  Private Sub btnDeleteRecipe_Click_1(sender As Object, e As EventArgs) Handles btnDeleteRecipe.Click
    Dim dr As DialogResult = DeleteDialog.ShowDialog
    If dr = DialogResult.OK Then
      Dim lstParams As New List(Of String)
      lstParams.Add(lstRecipesBox.SelectedValue)
      Dim dt As DataTable = paramQuery("UPDATE Recipes SET Deleted_At = datetime('now','localtime') WHERE Name = @pname1", lstParams)
      Dim dt2 As DataTable = runQuery("SELECT Name, id FROM Recipes WHERE Deleted_At IS NULL")
      lstRecipesBox.ValueMember = "Name"
      lstRecipesBox.DisplayMember = "Name"
      lstRecipesBox.DataSource = dt2
    ElseIf dr = DialogResult.Cancel Then
      DeleteDialog.Close()
    End If
  End Sub

  Private Sub btnSearchRecipes_Click_1(sender As Object, e As EventArgs) Handles btnSearchRecipes.Click
    Dim lstParams As New List(Of String)
    lstParams.Add(txtSearchBox.Text)
    Dim dt As DataTable = paramQuery("SELECT Name, id FROM Recipes WHERE Name LIKE @pname1", lstParams)
    lstRecipesBox.ValueMember = "Name"
    lstRecipesBox.DisplayMember = "Name"
    lstRecipesBox.DataSource = dt
  End Sub

  Private Sub btnReloadList_Click_1(sender As Object, e As EventArgs) Handles btnReloadList.Click
    Dim dt As DataTable = runQuery("SELECT Name, id FROM Recipes WHERE Deleted_At IS NULL")
    lstRecipesBox.ValueMember = "Name"
    lstRecipesBox.DisplayMember = "Name"
    lstRecipesBox.DataSource = dt
    txtSearchBox.Text = ""
  End Sub

  Private Sub btnCreateNewFlave_Click(sender As Object, e As EventArgs)
    Create_Flave()
    Me.Close()
  End Sub

  Public Sub lstRecipesBox_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles lstRecipesBox.DrawItem
    e.DrawBackground()
    If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
      e.Graphics.FillRectangle(Brushes.RosyBrown, e.Bounds)
    End If
    Using b As New SolidBrush(e.ForeColor)
      e.Graphics.DrawString(lstRecipesBox.GetItemText(lstRecipesBox.Items(e.Index)), e.Font, b, e.Bounds)
    End Using
    e.DrawFocusRectangle()
  End Sub

  Private Sub btnRefPage_Click(sender As Object, e As EventArgs) Handles btnRefPage.Click
    Me.frmMyFaves_Load(Nothing, Nothing)
  End Sub

  Private Sub btnUsers_Click(sender As Object, e As EventArgs) Handles btnUsers.Click
    frmLogin.Show()
  End Sub

  Private Sub frmMyFaves_TextChanged(sender As Object, e As EventArgs) Handles Me.TextChanged
    Update_Recipes()
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



