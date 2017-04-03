Imports System.Data.SQLite

Public Class frmWelcome

  Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnSelectDB.Click
    My.Settings.ConnectionString = "Data Source =" & txtDatabaseFile.Text
    If txtDatabaseFile.Text = "" Then
      My.Settings.ConnectionString = ""
    End If
    My.Settings.Save()
    g_sConnString = My.Settings.ConnectionString
    frmMain.Show()
    frmMain.WindowState = FormWindowState.Normal
    frmMain.ShowInTaskbar = True
    Me.Close()
  End Sub

  Private Sub btnChooseNewDB_Click(sender As Object, e As EventArgs) Handles btnChooseNewDB.Click
    OpenFileDialogChooseDB.ShowDialog()
    txtDatabaseFile.Text = OpenFileDialogChooseDB.FileName
    My.Settings.Save()
    g_sConnString = My.Settings.ConnectionString

  End Sub

  Private Sub btnCreateDB_Click(sender As Object, e As EventArgs) Handles btnCreateDB.Click
   
  End Sub
End Class