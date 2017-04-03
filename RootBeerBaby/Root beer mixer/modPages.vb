Imports System.IO
Imports System.Data.SQLite

'~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'
'this module controls page openings and stuff.
'if you are looking at this, you should now awkwardly look at Alex, and make him feel really uncomfortable...
'~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'

Module modPages

  Public g_Main_Page As Form
  Public g_Create_Flave As Form
  Public g_Add_Flave As Form
  Public g_My_Faves As Form
  Public g_Settings As Form
  Public g_Daves_Faves As Form

#Region "Page Opennings"

  Public Sub Main_Page()
    frmMain.Show()
  End Sub

  Public Sub Create_Flave()
    If g_Create_Flave IsNot Nothing Then
      g_Create_Flave.Refresh()
    Else
      g_Create_Flave = New frmCreate_Flave
      g_Create_Flave.Show()
    End If
  End Sub

  Public Sub Confirm_Flave()
    frmConfirmFlave.Show()
  End Sub

  Public Sub My_Faves()
    If g_My_Faves IsNot Nothing Then
      g_My_Faves.Refresh()
    Else
      g_My_Faves = New frmMyFaves
      g_My_Faves.Show()
    End If
  End Sub

  Public Sub Delete_Dialog()
    DeleteDialog.Show()
  End Sub

  Public Sub Add_New()
    If g_Add_Flave IsNot Nothing Then
      g_Add_Flave.Refresh()
    Else
      g_Add_Flave = New frmAddFaveFlave
      g_Add_Flave.Show()
    End If
  End Sub

  Public Sub Settings()
    If g_Settings IsNot Nothing Then
      g_Settings.Refresh()
    Else
      g_Settings = New frmSettings
      g_Settings.Show()
    End If
  End Sub

  Public Sub Daves_Faves()
    If g_Daves_Faves IsNot Nothing Then
      g_Daves_Faves.Refresh()
    Else
      g_Daves_Faves = New frmDavesFaves
      g_Daves_Faves.Show()
    End If
  End Sub

  Public Sub Login()
    frmLogin.Show()
  End Sub

#End Region

End Module

