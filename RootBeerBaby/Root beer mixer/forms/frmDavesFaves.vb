Imports System.Data.SQLite
Imports System.Web.Script.Serialization
Imports System.Data.Common
Imports System.IO

'The basic functionality of this form is the same that is found on the create_flave form and on the myFaves form. 
'There are small changes in terms of what tables in the database are referenced but otherwise the recipe creation uses the same format and same math.
'One major difference that exists however, is in the create recipe section. 
'There is no rate_drink form that pops up, dave's faves are set recipes, not experimental so there is no rating system for them

Public Class frmDavesFaves

  Public WithEvents m_relay As DIN3Relay
  Public WithEvents m_relay2 As DIN3Relay
  Public WithEvents m_relay3 As DIN3Relay
  Public WithEvents m_relay4 As DIN3Relay
  Public m_interval As Integer = 1
  Dim myThread As Threading.Thread

#Region "Navigation"

  Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
    Main_Page()
    Me.Close()
  End Sub

  Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
    Settings()
  End Sub

  Private Sub btnRefPage_Click(sender As Object, e As EventArgs) Handles btnRefPage.Click
    Me.frmDavesFaves_Load(Nothing, Nothing)
  End Sub

  Public Sub lstDavesFaves_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles lstDavesFaves.DrawItem
    e.DrawBackground()
    If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
      e.Graphics.FillRectangle(Brushes.RosyBrown, e.Bounds)
    End If
    Using b As New SolidBrush(e.ForeColor)
      e.Graphics.DrawString(lstDavesFaves.GetItemText(lstDavesFaves.Items(e.Index)), e.Font, b, e.Bounds)
    End Using
    e.DrawFocusRectangle()
  End Sub

  Public Sub Reload_Daves_Faves()
    MsgBox("Settings changes have occured." & vbCrLf & "Please refresh the page.")
    Me.Refresh()
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

#Region "Form Load Queries"

  Private Sub frmDavesFaves_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    tTimer2.Start()
    Dim dt As DataTable = runQuery("SELECT Name FROM Daves_Recipes WHERE Deleted_At IS NULL")
    lstDavesFaves.DataSource = dt
    lstDavesFaves.ValueMember = "Name"
    lstDavesFaves.DisplayMember = "Name"
    lstID.Text = "1"
    get_Daves_Recipes_info()
  End Sub

  Private Sub tTimer2_Tick(sender As Object, e As EventArgs) Handles tTimer2.Tick
    If My.Settings.UserName = "Public" Then
      Me.Text = "Logged in as public user"
    Else
      Me.Text = "Logged in as user: " & My.Settings.UserName.ToString
    End If
  End Sub

  Private Sub frmDavesFaves_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
    tTimer2.Stop()
    tTimer2.Dispose()
    g_Daves_Faves.Dispose()
    g_Daves_Faves.Close()
    g_Daves_Faves = Nothing
  End Sub

  Public Sub get_Daves_Recipes_info()
    lstViewRecipe.Items.Clear()
    If My.Settings.Relay_Mode = "1" Then
      Dim sSQL As String = "SELECT Ingredient_ID, Ingredient_Name, Parts FROM Daves_Faves WHERE Ingredient_ID < 9 and Recipe_ID = '" & lstID.Text & "'"
      Fill_ListView(sSQL, lstViewRecipe)
    ElseIf My.Settings.Relay_Mode = "2" Then
      Dim sSQL As String = "SELECT Ingredient_ID, Ingredient_Name, Parts FROM Daves_Faves WHERE Ingredient_ID < 17 and Recipe_ID = '" & lstID.Text & "'"
      Fill_ListView(sSQL, lstViewRecipe)
    ElseIf My.Settings.Relay_Mode = "3" Then
      Dim sSQL As String = "SELECT Ingredient_ID, Ingredient_Name, Parts FROM Daves_Faves WHERE Ingredient_ID < 25 and Recipe_ID = '" & lstID.Text & "'"
      Fill_ListView(sSQL, lstViewRecipe)
    ElseIf My.Settings.Relay_Mode = "4" Then
      Dim sSQL As String = "SELECT Ingredient_ID, Ingredient_Name, Parts FROM Daves_Faves WHERE Ingredient_ID < 33 and Recipe_ID = '" & lstID.Text & "'"
      Fill_ListView(sSQL, lstViewRecipe)
    End If
  End Sub

  Private Sub lstDavesFaves_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstDavesFaves.SelectedIndexChanged
    Try
      Dim lstParams As New List(Of String)
      lstParams.Add(lstDavesFaves.SelectedValue.ToString)
      Dim dt As DataTable = paramQuery("SELECT id FROM Daves_Recipes WHERE Name = @pname1", lstParams)
      lstID.DataSource = dt
      lstID.ValueMember = "id"
      lstID.DisplayMember = "id"
      get_Daves_Recipes_info()
    Catch ex As Exception
      MessageBox.Show(ex.Message)
    End Try
  End Sub

#End Region

#Region "Relay Coding"

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
      Create_Recipe_Production()
    ElseIf dr = DialogResult.Cancel Then
      confirmDialog.Close()
    End If
  End Sub

#Region "Production Mode"

  Public Sub Create_Recipe_Production()
    Dim iRelay As Integer
    Dim sRequest As String = "{0}outlet?"
    Dim sRequest2 As String = "{0}outlet?"
    Dim sRequest3 As String = "{0}outlet?"
    Dim sRequest4 As String = "{0}outlet?"
    For Each item As ListViewItem In Me.lstViewRecipe.Items
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
        Calculate(lstViewRecipe, tTotal, frm:=Me)
      ElseIf My.Settings.Relay_Mode = "2" Then
        m_relay.Relay_Set_ID(sRequest, "ON")
        m_relay2.Relay_Set_ID(sRequest2, "ON")
        Calculate(lstViewRecipe, tTotal, frm:=Me)
      ElseIf My.Settings.Relay_Mode = "3" Then
        m_relay.Relay_Set_ID(sRequest, "ON")
        m_relay2.Relay_Set_ID(sRequest2, "ON")
        m_relay3.Relay_Set_ID(sRequest3, "ON")
        Calculate(lstViewRecipe, tTotal, frm:=Me)
      ElseIf My.Settings.Relay_Mode = "4" Then
        m_relay.Relay_Set_ID(sRequest, "ON")
        m_relay2.Relay_Set_ID(sRequest2, "ON")
        m_relay3.Relay_Set_ID(sRequest3, "ON")
        m_relay4.Relay_Set_ID(sRequest4, "ON")
        Calculate(lstViewRecipe, tTotal, frm:=Me)
      End If
    Catch ex As Exception
      MessageBox.Show(ex.Message)
    End Try
  End Sub

#End Region

#End Region

End Class