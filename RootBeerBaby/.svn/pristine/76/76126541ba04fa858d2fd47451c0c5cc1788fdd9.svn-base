Imports System.Data.SQLite

Public Class frmAddFaveFlave

  ''' <summary>
  ''' Use this sub to fill your list box with values from your database of all ingredients.
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub filllstBoxIngs()
    If My.Settings.Relay_Mode = "1" Then
      Dim dt As DataTable = runQuery("SELECT Name,id FROM Ingredients WHERE id < 9")
      lstBoxIngredients.ValueMember = "Name"
      lstBoxIngredients.DisplayMember = "Name"
      lstBoxIngredients.DataSource = dt
    ElseIf My.Settings.Relay_Mode = "2" Then
      Dim dt As DataTable = runQuery("SELECT Name,id FROM Ingredients WHERE id < 17")
      lstBoxIngredients.ValueMember = "Name"
      lstBoxIngredients.DisplayMember = "Name"
      lstBoxIngredients.DataSource = dt
    ElseIf My.Settings.Relay_Mode = "3" Then
      Dim dt As DataTable = runQuery("SELECT Name,id FROM Ingredients WHERE id < 25")
      lstBoxIngredients.ValueMember = "Name"
      lstBoxIngredients.DisplayMember = "Name"
      lstBoxIngredients.DataSource = dt
    ElseIf My.Settings.Relay_Mode = "4" Then
      Dim dt As DataTable = runQuery("SELECT Name,id FROM Ingredients WHERE id < 33")
      lstBoxIngredients.ValueMember = "Name"
      lstBoxIngredients.DisplayMember = "Name"
      lstBoxIngredients.DataSource = dt
    End If
  End Sub

  Private Sub frmAddFaveFlave_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
    tTimer.Stop()
    tTimer.Dispose()
    g_Add_Flave.Dispose()
    g_Add_Flave.Close()
    g_Add_Flave = Nothing
  End Sub

  Private Sub tTimer_Tick(sender As Object, e As EventArgs) Handles tTimer.Tick
    If My.Settings.UserName = "Public" Then
      Me.Text = "Logged in as public user"
    Else
      Me.Text = "Logged in as user: " & My.Settings.UserName.ToString
    End If
  End Sub

  Private Sub frmAddFaveFlave_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    tTimer.Start()
    If g_Add_Flave IsNot Nothing Then
      filllstBoxIngs()
    Else
      g_Add_Flave = Me
      Me.frmAddFaveFlave_Load(Nothing, Nothing)
    End If
  End Sub

  ''' <summary>
  ''' When the selected index of the listbox is changed, the id of the flavor from the database is grabbed and stored in another listbox.
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub lstBoxIngredients_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstBoxIngredients.SelectedIndexChanged
    Dim lstParams As New List(Of String)
    lstParams.Add(lstBoxIngredients.SelectedValue)
    Dim dt As DataTable = paramQuery("SELECT id FROM Ingredients WHERE Name = @pname1", lstParams)
    lstIngIDBox.DataSource = dt
    lstIngIDBox.ValueMember = "id"
    lstIngIDBox.DisplayMember = "id"
  End Sub

  ''' <summary>
  ''' This click event adds things from the list box of ingredients and amounts to the recipe listview
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub btnAddToRec_Click(sender As Object, e As EventArgs) Handles btnAddToRec.Click
    Dim items = lstBoxIngredients.SelectedValue
    Dim amounts = lstBoxAmount.SelectedItem
    Dim ID = lstIngIDBox.Text
    If lstBoxAmount.SelectedItem Is Nothing Then
      MsgBox("Add amount to ingredient selection")
    Else
      Dim itm As ListViewItem
      itm = lstViewRecipe.Items.Add(items)
      itm.SubItems.Add(amounts)
      itm.SubItems.Add(ID)
    End If
  End Sub

  Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
    For Each i As ListViewItem In lstViewRecipe.SelectedItems
      lstViewRecipe.Items.Remove(i)
    Next
  End Sub

  ''' <summary>
  ''' This takes all of the information stored in the recipe listview temporarily and saves it in the database.
  ''' Because it is the save for the public users, these recipes are visible to everyone.
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub btnSavePublic_Click(sender As Object, e As EventArgs) Handles btnSavePublic.Click
    Dim lstParams As New List(Of String)
    lstParams.Add(txtNameBox.Text)
    lstParams.Add(My.Settings.UserID.ToString)
    lstParams.Add(Date.Now)
    lstParams.Add("True")
    Dim dt As DataTable = paramQuery("INSERT INTO Recipes (Name, User_ID, Created_At,Public) VALUES (@pname1,@pname2,@pname3,@pname4)", lstParams)
    Dim dt4 As DataTable = paramQuery("SELECT id FROM Recipes WHERE Name LIKE @pname1", lstParams)
    lstIDBox.DataSource = dt4
    lstIDBox.DisplayMember = "id"
    lstIDBox.ValueMember = "id"
    Dim dr2 As DialogResult = SaveDialogBox.ShowDialog
    If dr2 = DialogResult.OK Then
      Dim lstParams2 As New List(Of String)
      For Each entry As ListViewItem In lstViewRecipe.Items
        lstParams2.Add(lstIDBox.Text)
        lstParams2.Add(entry.SubItems(2).Text)
        lstParams2.Add(entry.Text)
        lstParams2.Add(entry.SubItems(1).Text)
        Dim i As Integer = insertQuery(("INSERT INTO Recipe_Ingredients (Recipe_ID,Ingredient_ID,Ingredient_Name,Parts) VALUES (@pname1,@pname2,@pname3,@pname4)"), lstParams2)
        lstParams2.Clear()
      Next
    ElseIf dr2 = DialogResult.Cancel Then
      SaveDialogBox.Close()
      Dim dt3 As DataTable = paramQuery("DELETE FROM Recipes WHERE Name = @pname1", lstParams)
      txtNameBox.Text = ""
    End If
    lstParams.Clear()
  End Sub

  ''' <summary>
  ''' This does the same thing as the above save event. It just does so for individual users only.
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
    Dim lstParams As New List(Of String)
    lstParams.Add(txtNameBox.Text)
    lstParams.Add(My.Settings.UserID.ToString)
    lstParams.Add(Date.Now)
    lstParams.Add("False")
    Dim dt As DataTable = paramQuery("INSERT INTO Recipes (Name, User_ID, Created_At,Public) VALUES (@pname1,@pname2,@pname3,@pname4)", lstParams)
    Dim dt4 As DataTable = paramQuery("SELECT id FROM Recipes WHERE Name LIKE @pname1", lstParams)
    lstIDBox.DataSource = dt4
    lstIDBox.DisplayMember = "id"
    lstIDBox.ValueMember = "id"
    Dim dr2 As DialogResult = SaveDialogBox.ShowDialog
    If dr2 = DialogResult.OK Then
      Dim lstParams2 As New List(Of String)
      For Each entry As ListViewItem In lstViewRecipe.Items
        lstParams2.Add(lstIDBox.Text)
        lstParams2.Add(entry.SubItems(2).Text)
        lstParams2.Add(entry.Text)
        lstParams2.Add(entry.SubItems(1).Text)
        Dim i As Integer = insertQuery(("INSERT INTO Recipe_Ingredients (Recipe_ID,Ingredient_ID,Ingredient_Name,Parts) VALUES (@pname1,@pname2,@pname3,@pname4)"), lstParams2)
        lstParams2.Clear()
      Next
    ElseIf dr2 = DialogResult.Cancel Then
      SaveDialogBox.Close()
      Dim dt3 As DataTable = paramQuery("DELETE FROM Recipes WHERE Name = @pname1", lstParams)
      txtNameBox.Text = ""
    End If
    lstParams.Clear()
  End Sub

  Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
    Main_Page()
    Me.Close()
  End Sub

  Private Sub btnFavorites_Click(sender As Object, e As EventArgs) Handles btnFavorites.Click
    My_Faves()
    Me.Close()
  End Sub

  Private Sub btnClearAll_Click(sender As Object, e As EventArgs) Handles btnClearAll.Click
    lstViewRecipe.Items.Clear()
  End Sub

  ''' <summary>
  ''' This drawitem event specifies how it possible for the selected index in the list box or listview to be a different color.
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Public Sub lstBoxIngredients_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles lstBoxIngredients.DrawItem
    e.DrawBackground()
    If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
      e.Graphics.FillRectangle(Brushes.RosyBrown, e.Bounds)
    End If
    Using b As New SolidBrush(e.ForeColor)
      e.Graphics.DrawString(lstBoxIngredients.GetItemText(lstBoxIngredients.Items(e.Index)), e.Font, b, e.Bounds)
    End Using
    e.DrawFocusRectangle()
  End Sub

  Public Sub lstBoxAmount_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles lstBoxAmount.DrawItem
    e.DrawBackground()
    If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
      e.Graphics.FillRectangle(Brushes.RosyBrown, e.Bounds)
    End If
    Using b As New SolidBrush(e.ForeColor)
      e.Graphics.DrawString(lstBoxAmount.GetItemText(lstBoxAmount.Items(e.Index)), e.Font, b, e.Bounds)
    End Using
    e.DrawFocusRectangle()
  End Sub

  Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
    Settings()
  End Sub

  Private Sub btnRefPage_Click(sender As Object, e As EventArgs) Handles btnRefPage.Click
    Me.frmAddFaveFlave_Load(Nothing, Nothing)
  End Sub

End Class