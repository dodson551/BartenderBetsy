Imports System.Runtime.CompilerServices
Imports System.Data.SQLite
Imports System.IO

Public Class frmTestWebRequest

  Public WithEvents m_relay As DIN3Relay
  Public WithEvents m_relay2 As DIN3Relay
  Public m_interval As Integer = 1

#Region "Loading and Connecting"

  Private Sub frmTestWebRequest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    ConnectAllRelays()
  End Sub

  Public Sub ConnectAllRelays()
    m_relay = Nothing
    m_relay = New DIN3Relay(My.Settings.IPAddress, My.Settings.Login, My.Settings.Password, True, 5000)
    m_relay.Connect()
    m_relay.RefreshStatus()
    m_relay2 = Nothing
    m_relay2 = New DIN3Relay(My.Settings.IPAddress2, My.Settings.Login2, My.Settings.Password2, True, 5000)
    m_relay2.Connect()
    m_relay2.RefreshStatus()
    'm_Relay3 = Nothing
    'm_Relay3 = New DIN3Relay(My.Settings.IPAddress3, My.Settings.Login3, My.Settings.Password3, True, 5000)
    'm_Relay3.Connect()
    'm_Relay3.RefreshStatus()
  End Sub

#End Region

#Region "Button Presses"

  Private Sub btnOne_Click(sender As Object, e As EventArgs) Handles btnOne.Click
    lstViewID.Items.Add(btnOne.Text)
  End Sub

  Private Sub btnTwo_Click(sender As Object, e As EventArgs) Handles btnTwo.Click
    lstViewID.Items.Add(btnTwo.Text)
  End Sub

  Private Sub btnThree_Click(sender As Object, e As EventArgs) Handles btnThree.Click
    lstViewID.Items.Add(btnThree.Text)
  End Sub

  Private Sub btnFour_Click(sender As Object, e As EventArgs) Handles btnFour.Click
    lstViewID.Items.Add(btnFour.Text)
  End Sub

  Private Sub btnFive_Click(sender As Object, e As EventArgs) Handles btnFive.Click
    lstViewID.Items.Add(btnFive.Text)
  End Sub

  Private Sub btnSix_Click(sender As Object, e As EventArgs) Handles btnSix.Click
    lstViewID.Items.Add(btnSix.Text)
  End Sub

  Private Sub btnSeven_Click(sender As Object, e As EventArgs) Handles btnSeven.Click
    lstViewID.Items.Add(btnSeven.Text)
  End Sub

  Private Sub btnEight_Click(sender As Object, e As EventArgs) Handles btnEight.Click
    lstViewID.Items.Add(btnEight.Text)
  End Sub

  Private Sub btn9_Click(sender As Object, e As EventArgs) Handles btn9.Click
    lstViewID.Items.Add(btn9.Text)
  End Sub

  Private Sub btn10_Click(sender As Object, e As EventArgs) Handles btn10.Click
    lstViewID.Items.Add(btn10.Text)
  End Sub

  Private Sub btn11_Click(sender As Object, e As EventArgs) Handles btn11.Click
    lstViewID.Items.Add(btn11.Text)
  End Sub

  Private Sub btn12_Click(sender As Object, e As EventArgs) Handles btn12.Click
    lstViewID.Items.Add(btn12.Text)
  End Sub

  Private Sub btn13_Click(sender As Object, e As EventArgs) Handles btn13.Click
    lstViewID.Items.Add(btn13.Text)
  End Sub

  Private Sub btn14_Click(sender As Object, e As EventArgs) Handles btn14.Click
    lstViewID.Items.Add(btn14.Text)
  End Sub

  Private Sub btn15_Click(sender As Object, e As EventArgs) Handles btn15.Click
    lstViewID.Items.Add(btn15.Text)
  End Sub

  Private Sub btn16_Click(sender As Object, e As EventArgs) Handles btn16.Click
    lstViewID.Items.Add(btn16.Text)
  End Sub

  Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
    lstViewID.Items.Clear()
  End Sub

#End Region

#Region "Relay Control"

  Private Sub btnOn_Click(sender As Object, e As EventArgs) Handles btnOn.Click
    Dim iRelay As Integer
    Dim sRequest As String = "{0}outlet?" & iRelay & "={1}" '{0}outlet?1=ON&4=ON
    Dim sRequest2 As String = "{0}outlet?" & iRelay & "={1}" '{0}outlet?1=ON&4=ON
    For Each item As ListViewItem In Me.lstViewID.Items
      iRelay = item.Text
      If iRelay <= 8 Then
        sRequest = sRequest & "&" & iRelay & "={1}"
      ElseIf iRelay > 8 And iRelay <= 16 Then
        iRelay = iRelay - 8
        sRequest2 = sRequest2 & "&" & iRelay & "={1}"
      End If
    Next
    MsgBox(sRequest & vbCrLf & sRequest2)
    Try
      m_relay.Relay_Set_ID(sRequest, "ON")
      m_relay2.Relay_Set_ID(sRequest2, "ON")
    Catch ex As Exception
      MessageBox.Show(ex.Message)
    End Try
    Timer1.Enabled = True
  End Sub

  Private Sub btnOff_Click(sender As Object, e As EventArgs) Handles btnOff.Click
    Try
      m_relay.Relay_SetAll(False)
      m_relay2.Relay_SetAll(False)
    Catch ex As Exception
      MessageBox.Show(ex.Message)
    End Try
  End Sub

#End Region

  Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
    Dim sReq1 As String = "{0}outlet?={1}"
    Dim sReq2 As String = "{0}outlet?={1}"
    If m_interval = 1 Then
      lblAm.Text = "0.5"
      m_interval += 1
      sReq1 = "{0}outlet?1={1}&2={1}"
      m_relay.Relay_Set_ID(sReq1, "OFF")
    ElseIf m_interval = 2 Then
      lblAm.Text = "1.0"
      m_interval += 1
      sReq1 = "{0}outlet?3={1}&4={1}"
      m_relay.Relay_Set_ID(sReq1, "OFF")
    ElseIf m_interval = 3 Then
      lblAm.Text = "1.5"
      m_interval += 1
      sReq2 = "{0}outlet?1={1}"
      m_relay2.Relay_Set_ID(sReq2, "OFF")
    ElseIf m_interval = 4 Then
      lblAm.Text = "2.0"
      m_interval += 1
      sReq2 = "{0}outlet?2={1}"
      m_relay2.Relay_Set_ID(sReq2, "OFF")
    ElseIf m_interval = 5 Then
      lblAm.Text = "2.5"
      m_interval += 1
      sReq2 = "{0}outlet?3={1}"
      m_relay2.Relay_Set_ID(sReq2, "OFF")
    ElseIf m_interval = 6 Then
      lblAm.Text = "3.0"
      m_interval += 1
      sReq2 = "{0}outlet?4={1}"
      m_relay2.Relay_Set_ID(sReq2, "OFF")
    Else
      Timer1.Enabled = False
      lblAm.Text = "0.0"
      m_interval = 1
    End If
  End Sub
End Class