Imports System.Data.SQLite

Public Class frmLogin

  Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    btnLogin.DialogResult = Nothing
    lblLoginStatus.Visible = True
    FillComboBoxFromDB(cbUsername, "SELECT id, user_name FROM Users", "id", "user_name")
  End Sub

  Private Sub cbUsername_TextChanged(sender As Object, e As EventArgs) Handles cbUsername.TextChanged
    Dim dt As DataTable = runQuery("SELECT id FROM Users WHERE user_name = '" & cbUsername.Text & "'")
    lstIDBox.DataSource = dt
    lstIDBox.DisplayMember = "id"
    lstIDBox.ValueMember = "id"
  End Sub

  ''' <summary>
  ''' This sub will create a new user in the database with a username and a password.
  ''' This user will be able to create recipes for themselves personally, or have the option to share with others.
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub btnNewUser_Click(sender As Object, e As EventArgs) Handles btnNewUser.Click
    lblLoginStatus.Visible = False
    lblLoginStatus.ForeColor = Color.Black
    If cbUsername.Text = "" And txtPassword.Text = "" Then
      lblLoginStatus.Visible = True
      lblLoginStatus.Text = "No login information entered. Please try again."
      lblLoginStatus.ForeColor = Color.Red
    Else
      lblLoginStatus.Visible = True
      Dim lstParams As New List(Of String)
      lstParams.Add(cbUsername.Text)
      lstParams.Add(txtPassword.Text)
      Try
        Dim i As Integer = insertQuery("INSERT INTO Users (user_name,password) VALUES (@pname1,@pname2)", lstParams)
      Catch ex As Exception
        MessageBox.Show(ex.Message)
      End Try
      lblLoginStatus.Text = "New user created successfully!"
    End If
  End Sub

  ''' <summary>
  ''' When a user tries to log in, this will check to make sure everything is good with their attempt and then log them in successfully if they have the right information.
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
    Dim conn As New SQLite.SQLiteConnection(g_sConnString)
    Try
      conn.Open()
      Dim sSQL As String = "SELECT * FROM users WHERE user_name ='" & cbUsername.Text & "' and password = '" & txtPassword.Text & "'"
      Dim comm As SQLiteCommand
      comm = conn.CreateCommand
      comm.CommandText = sSQL
      comm.ExecuteNonQuery()
      Dim dr As SQLiteDataReader = comm.ExecuteReader()
      Dim count As Integer = 0
      While dr.Read()
        count += 1
      End While
      If count = 1 Then
        My.Settings.UserID = lstIDBox.Text
        My.Settings.UserName = cbUsername.Text
        My.Settings.Save()
        Me.Close()
      ElseIf count > 1 Then
        MsgBox("Duplicate username and password. Try again.")
      ElseIf count < 1 Then
        MsgBox("Username or password does not match. Try again.")
      End If
      comm.Dispose()
      conn.Close()
      conn.Dispose()
    Catch ex As Exception
      MessageBox.Show(ex.Message)
    End Try
  End Sub

  Private Sub btnPublic_Click(sender As Object, e As EventArgs) Handles btnPublic.Click
    My.Settings.UserID = "0"
    My.Settings.UserName = "Public"
    My.Settings.Save()
    Me.Close()
  End Sub

  Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
    My.Settings.UserID = "0"
    My.Settings.UserName = "Public"
    My.Settings.Save()
    Me.Close()
  End Sub
End Class