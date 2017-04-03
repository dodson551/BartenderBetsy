Public Class frmMain

  'What you see here is pretty self explanatory. There are some events that handle page navigation.
  'There is also the login information stuff and that is about it.
  'If you start trying to add stuff on this page, you will feel great about yourself.

  Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    tTimer.Start()
    g_sConnString = My.Settings.ConnectionString
    If My.Settings.UserName = "Public" Then
      Me.Text = "Logged in as public user"
    Else
      Me.Text = "Logged in as user: " & My.Settings.UserName.ToString
    End If
  End Sub

  Private Sub tTimer_Tick(sender As Object, e As EventArgs) Handles tTimer.Tick
    If My.Settings.UserName = "Public" Then
      Me.Text = "Logged in as public user"
    Else
      Me.Text = "Logged in as user: " & My.Settings.UserName.ToString
    End If
  End Sub

  Private Sub frmMain_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
    tTimer.Stop()
  End Sub

  Private Sub btnSettings_Click_1(sender As Object, e As EventArgs) Handles btnSettings.Click
    Settings()
  End Sub

  Private Sub btnDavesFaves_Click_1(sender As Object, e As EventArgs) Handles btnDavesFaves.Click
    Daves_Faves()
    Me.Close()
  End Sub

  Private Sub btnMyFaves_Click_1(sender As Object, e As EventArgs) Handles btnMyFaves.Click
    My_Faves()
    Me.Close()
  End Sub

  Private Sub btnCreateFlave_Click_1(sender As Object, e As EventArgs) Handles btnCreateFlave.Click
    Create_Flave()
    Me.Close()
  End Sub

  Private Sub btnUsers_Click(sender As Object, e As EventArgs) Handles btnUsers.Click
    frmLogin.Show()
  End Sub
End Class