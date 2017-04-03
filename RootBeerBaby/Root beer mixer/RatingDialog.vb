Imports System.Windows.Forms

Public Class RatingDialog

  Public m_rating As Integer = 0

  Private Sub DrinkDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    m_rating = 0
  End Sub

  Private Sub pb1Star_Click(sender As Object, e As EventArgs) Handles pb1Star.Click
    pb1Star.Image = My.Resources.star_gold
    pb2Stars.Image = My.Resources.star_grey
    pb3Stars.Image = My.Resources.star_grey
    pb4Stars.Image = My.Resources.star_grey
    pb5Stars.Image = My.Resources.star_grey
    m_rating = 1
  End Sub

  Private Sub pb2Stars_Click(sender As Object, e As EventArgs) Handles pb2Stars.Click
    pb1Star.Image = My.Resources.star_gold
    pb2Stars.Image = My.Resources.star_gold
    pb3Stars.Image = My.Resources.star_grey
    pb4Stars.Image = My.Resources.star_grey
    pb5Stars.Image = My.Resources.star_grey
    m_rating = 2
  End Sub

  Private Sub pb3Stars_Click(sender As Object, e As EventArgs) Handles pb3Stars.Click
    pb1Star.Image = My.Resources.star_gold
    pb2Stars.Image = My.Resources.star_gold
    pb3Stars.Image = My.Resources.star_gold
    pb4Stars.Image = My.Resources.star_grey
    pb5Stars.Image = My.Resources.star_grey
    m_rating = 3
  End Sub

  Private Sub pb4Stars_Click(sender As Object, e As EventArgs) Handles pb4Stars.Click
    pb1Star.Image = My.Resources.star_gold
    pb2Stars.Image = My.Resources.star_gold
    pb3Stars.Image = My.Resources.star_gold
    pb4Stars.Image = My.Resources.star_gold
    pb5Stars.Image = My.Resources.star_grey
    m_rating = 4
  End Sub

  Private Sub pb5Stars_Click(sender As Object, e As EventArgs) Handles pb5Stars.Click
    pb1Star.Image = My.Resources.star_gold
    pb2Stars.Image = My.Resources.star_gold
    pb3Stars.Image = My.Resources.star_gold
    pb4Stars.Image = My.Resources.star_gold
    pb5Stars.Image = My.Resources.star_gold
    m_rating = 5
  End Sub

  Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
    m_rating = 0
    Me.Close()
  End Sub

  ''' <summary>
  ''' inserts into the database the rating of the drink that was just created. It inserts the name as well as the name, the user, and any comments on the recipe
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub btnRate_Click(sender As Object, e As EventArgs) Handles btnRate.Click
    Dim lstParams As New List(Of String)
    lstParams.Add(txtName.Text)
    lstParams.Add(My.Settings.UserID.ToString)
    lstParams.Add(Date.Now)
    lstParams.Add("True")
    lstParams.Add(m_rating)
    lstParams.Add(txtComments.Text)
    Dim dt As DataTable = paramQuery("INSERT INTO Recipes (Name,User_ID,Created_At,Public,Rating,Comments) VALUES (@pname1,@pname2,@pname3,@pname4,@pname5,@pname6)", lstParams)
    Dim dt2 As DataTable = paramQuery("SELECT id FROM Recipes WHERE Name = @pname1", lstParams)
    lstIDBox.DataSource = dt2
    lstIDBox.DisplayMember = "id"
    lstIDBox.ValueMember = "id"
    Dim lstParams2 As New List(Of String)
    For Each entry As ListViewItem In lstViewInfo.Items
      lstParams2.Add(lstIDBox.Text)
      lstParams2.Add(entry.SubItems(2).Text)
      lstParams2.Add(entry.Text)
      lstParams2.Add(entry.SubItems(1).Text)
      Dim i As Integer = insertQuery("INSERT INTO Recipe_Ingredients (Recipe_ID, Ingredient_ID, Ingredient_Name,Parts) VALUES (@pname1,@pname2,@pname3,@pname4)", lstParams2)
      lstParams2.Clear()
    Next
    lstParams.Clear()
    Me.Close()
  End Sub
End Class
