<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMyFaves
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMyFaves))
    Me.lstID = New System.Windows.Forms.ListBox()
    Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlLeft = New System.Windows.Forms.Panel()
    Me.tlpButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.btnReloadList = New AutoScaleButton.AutoScaleButton()
    Me.btnSearchRecipes = New AutoScaleButton.AutoScaleButton()
    Me.btnAddRecipe = New AutoScaleButton.AutoScaleButton()
    Me.btnDeleteRecipe = New AutoScaleButton.AutoScaleButton()
    Me.txtSearchBox = New System.Windows.Forms.TextBox()
    Me.pnlMiddle = New System.Windows.Forms.Panel()
    Me.lstRecipesBox = New System.Windows.Forms.ListBox()
    Me.pnlRight = New System.Windows.Forms.Panel()
    Me.tlpRecipeInfo = New System.Windows.Forms.TableLayoutPanel()
    Me.tlpSlider = New System.Windows.Forms.TableLayoutPanel()
    Me.Label6 = New System.Windows.Forms.Label()
    Me.tbAmount = New System.Windows.Forms.TrackBar()
    Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
    Me.Label5 = New System.Windows.Forms.Label()
    Me.Label4 = New System.Windows.Forms.Label()
    Me.Label3 = New System.Windows.Forms.Label()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.btnUseRecipe = New AutoScaleButton.AutoScaleButton()
    Me.lstViewInfo = New System.Windows.Forms.ListView()
    Me.clmID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.clmIng = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.clmParts = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.tlpStars = New System.Windows.Forms.TableLayoutPanel()
    Me.pb5 = New System.Windows.Forms.PictureBox()
    Me.pb4 = New System.Windows.Forms.PictureBox()
    Me.pb3 = New System.Windows.Forms.PictureBox()
    Me.pb2 = New System.Windows.Forms.PictureBox()
    Me.pb1 = New System.Windows.Forms.PictureBox()
    Me.lstAmounts = New System.Windows.Forms.ListBox()
    Me.lstRecipesInfo = New System.Windows.Forms.ListBox()
    Me.pnlRecipeStatus = New System.Windows.Forms.Panel()
    Me.lstRating = New System.Windows.Forms.ListBox()
    Me.lstIngID = New System.Windows.Forms.ListBox()
    Me.pBarRecipe = New System.Windows.Forms.ProgressBar()
    Me.lblRecipeStatus = New System.Windows.Forms.Label()
    Me.pnlMain = New System.Windows.Forms.Panel()
    Me.tlpHeader = New System.Windows.Forms.TableLayoutPanel()
    Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
    Me.btnUsers = New AutoScaleButton.AutoScaleButton()
    Me.btnRefPage = New AutoScaleButton.AutoScaleButton()
    Me.btnSettings = New AutoScaleButton.AutoScaleButton()
    Me.btnNewFlave = New AutoScaleButton.AutoScaleButton()
    Me.btnMainPage = New AutoScaleButton.AutoScaleButton()
    Me.PictureBox1 = New System.Windows.Forms.PictureBox()
    Me.tTimer2 = New System.Windows.Forms.Timer(Me.components)
    Me.tlpMain.SuspendLayout()
    Me.pnlLeft.SuspendLayout()
    Me.tlpButtons.SuspendLayout()
    Me.pnlMiddle.SuspendLayout()
    Me.pnlRight.SuspendLayout()
    Me.tlpRecipeInfo.SuspendLayout()
    Me.tlpSlider.SuspendLayout()
    CType(Me.tbAmount, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.TableLayoutPanel2.SuspendLayout()
    Me.tlpStars.SuspendLayout()
    CType(Me.pb5, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pb4, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pb3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pb2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pb1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlRecipeStatus.SuspendLayout()
    Me.pnlMain.SuspendLayout()
    Me.tlpHeader.SuspendLayout()
    Me.TableLayoutPanel1.SuspendLayout()
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'lstID
    '
    Me.lstID.FormattingEnabled = True
    Me.lstID.Location = New System.Drawing.Point(6, 5)
    Me.lstID.Margin = New System.Windows.Forms.Padding(2)
    Me.lstID.Name = "lstID"
    Me.lstID.Size = New System.Drawing.Size(54, 17)
    Me.lstID.TabIndex = 20
    '
    'tlpMain
    '
    Me.tlpMain.ColumnCount = 3
    Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.0!))
    Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.0!))
    Me.tlpMain.Controls.Add(Me.pnlLeft, 0, 0)
    Me.tlpMain.Controls.Add(Me.pnlMiddle, 1, 0)
    Me.tlpMain.Controls.Add(Me.pnlRight, 2, 0)
    Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpMain.Location = New System.Drawing.Point(3, 186)
    Me.tlpMain.Name = "tlpMain"
    Me.tlpMain.RowCount = 1
    Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpMain.Size = New System.Drawing.Size(1256, 546)
    Me.tlpMain.TabIndex = 97
    '
    'pnlLeft
    '
    Me.pnlLeft.Controls.Add(Me.tlpButtons)
    Me.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlLeft.Location = New System.Drawing.Point(3, 3)
    Me.pnlLeft.Name = "pnlLeft"
    Me.pnlLeft.Size = New System.Drawing.Size(245, 540)
    Me.pnlLeft.TabIndex = 0
    '
    'tlpButtons
    '
    Me.tlpButtons.ColumnCount = 1
    Me.tlpButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpButtons.Controls.Add(Me.btnReloadList, 0, 4)
    Me.tlpButtons.Controls.Add(Me.btnSearchRecipes, 0, 2)
    Me.tlpButtons.Controls.Add(Me.btnAddRecipe, 0, 0)
    Me.tlpButtons.Controls.Add(Me.btnDeleteRecipe, 0, 1)
    Me.tlpButtons.Controls.Add(Me.txtSearchBox, 0, 3)
    Me.tlpButtons.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpButtons.Location = New System.Drawing.Point(0, 0)
    Me.tlpButtons.Name = "tlpButtons"
    Me.tlpButtons.RowCount = 5
    Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
    Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
    Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
    Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5!))
    Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.5!))
    Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.tlpButtons.Size = New System.Drawing.Size(245, 540)
    Me.tlpButtons.TabIndex = 19
    '
    'btnReloadList
    '
    Me.btnReloadList.AutoScaleBorder = 0
    Me.btnReloadList.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.Refresh_button
    Me.btnReloadList.BackColor = System.Drawing.Color.NavajoWhite
    Me.btnReloadList.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnReloadList.FlatAppearance.BorderSize = 2
    Me.btnReloadList.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnReloadList.Font = New System.Drawing.Font("Poor Richard", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnReloadList.Location = New System.Drawing.Point(3, 448)
    Me.btnReloadList.Name = "btnReloadList"
    Me.btnReloadList.Size = New System.Drawing.Size(239, 89)
    Me.btnReloadList.TabIndex = 17
    Me.btnReloadList.UseVisualStyleBackColor = False
    '
    'btnSearchRecipes
    '
    Me.btnSearchRecipes.AutoScaleBorder = 0
    Me.btnSearchRecipes.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.Search_button
    Me.btnSearchRecipes.BackColor = System.Drawing.Color.NavajoWhite
    Me.btnSearchRecipes.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnSearchRecipes.FlatAppearance.BorderSize = 2
    Me.btnSearchRecipes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnSearchRecipes.Font = New System.Drawing.Font("Poor Richard", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnSearchRecipes.Location = New System.Drawing.Point(3, 273)
    Me.btnSearchRecipes.Name = "btnSearchRecipes"
    Me.btnSearchRecipes.Size = New System.Drawing.Size(239, 129)
    Me.btnSearchRecipes.TabIndex = 18
    Me.btnSearchRecipes.UseVisualStyleBackColor = False
    '
    'btnAddRecipe
    '
    Me.btnAddRecipe.AutoScaleBorder = 0
    Me.btnAddRecipe.AutoScaleImage = CType(resources.GetObject("btnAddRecipe.AutoScaleImage"), System.Drawing.Image)
    Me.btnAddRecipe.BackColor = System.Drawing.Color.NavajoWhite
    Me.btnAddRecipe.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnAddRecipe.FlatAppearance.BorderSize = 2
    Me.btnAddRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnAddRecipe.Font = New System.Drawing.Font("Poor Richard", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnAddRecipe.Location = New System.Drawing.Point(3, 3)
    Me.btnAddRecipe.Name = "btnAddRecipe"
    Me.btnAddRecipe.Size = New System.Drawing.Size(239, 129)
    Me.btnAddRecipe.TabIndex = 16
    Me.btnAddRecipe.UseVisualStyleBackColor = False
    '
    'btnDeleteRecipe
    '
    Me.btnDeleteRecipe.AutoScaleBorder = 0
    Me.btnDeleteRecipe.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.Delete_button
    Me.btnDeleteRecipe.BackColor = System.Drawing.Color.NavajoWhite
    Me.btnDeleteRecipe.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnDeleteRecipe.FlatAppearance.BorderSize = 2
    Me.btnDeleteRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnDeleteRecipe.Font = New System.Drawing.Font("Poor Richard", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnDeleteRecipe.Location = New System.Drawing.Point(3, 138)
    Me.btnDeleteRecipe.Name = "btnDeleteRecipe"
    Me.btnDeleteRecipe.Size = New System.Drawing.Size(239, 129)
    Me.btnDeleteRecipe.TabIndex = 18
    Me.btnDeleteRecipe.UseVisualStyleBackColor = False
    '
    'txtSearchBox
    '
    Me.txtSearchBox.BackColor = System.Drawing.SystemColors.Menu
    Me.txtSearchBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.txtSearchBox.Dock = System.Windows.Forms.DockStyle.Fill
    Me.txtSearchBox.Font = New System.Drawing.Font("Poor Richard", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtSearchBox.Location = New System.Drawing.Point(2, 407)
    Me.txtSearchBox.Margin = New System.Windows.Forms.Padding(2)
    Me.txtSearchBox.Name = "txtSearchBox"
    Me.txtSearchBox.Size = New System.Drawing.Size(241, 40)
    Me.txtSearchBox.TabIndex = 5
    '
    'pnlMiddle
    '
    Me.pnlMiddle.Controls.Add(Me.lstRecipesBox)
    Me.pnlMiddle.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlMiddle.Location = New System.Drawing.Point(254, 3)
    Me.pnlMiddle.Name = "pnlMiddle"
    Me.pnlMiddle.Size = New System.Drawing.Size(559, 540)
    Me.pnlMiddle.TabIndex = 1
    '
    'lstRecipesBox
    '
    Me.lstRecipesBox.BackColor = System.Drawing.SystemColors.ButtonHighlight
    Me.lstRecipesBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.lstRecipesBox.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lstRecipesBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
    Me.lstRecipesBox.Font = New System.Drawing.Font("Poor Richard", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lstRecipesBox.FormattingEnabled = True
    Me.lstRecipesBox.ItemHeight = 55
    Me.lstRecipesBox.Location = New System.Drawing.Point(0, 0)
    Me.lstRecipesBox.Margin = New System.Windows.Forms.Padding(2)
    Me.lstRecipesBox.Name = "lstRecipesBox"
    Me.lstRecipesBox.ScrollAlwaysVisible = True
    Me.lstRecipesBox.Size = New System.Drawing.Size(559, 540)
    Me.lstRecipesBox.TabIndex = 1
    '
    'pnlRight
    '
    Me.pnlRight.Controls.Add(Me.tlpRecipeInfo)
    Me.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlRight.Location = New System.Drawing.Point(819, 3)
    Me.pnlRight.Name = "pnlRight"
    Me.pnlRight.Size = New System.Drawing.Size(434, 540)
    Me.pnlRight.TabIndex = 2
    '
    'tlpRecipeInfo
    '
    Me.tlpRecipeInfo.ColumnCount = 1
    Me.tlpRecipeInfo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpRecipeInfo.Controls.Add(Me.tlpSlider, 0, 2)
    Me.tlpRecipeInfo.Controls.Add(Me.btnUseRecipe, 0, 3)
    Me.tlpRecipeInfo.Controls.Add(Me.lstViewInfo, 0, 1)
    Me.tlpRecipeInfo.Controls.Add(Me.tlpStars, 0, 0)
    Me.tlpRecipeInfo.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpRecipeInfo.Location = New System.Drawing.Point(0, 0)
    Me.tlpRecipeInfo.Name = "tlpRecipeInfo"
    Me.tlpRecipeInfo.RowCount = 3
    Me.tlpRecipeInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
    Me.tlpRecipeInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.tlpRecipeInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.tlpRecipeInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
    Me.tlpRecipeInfo.Size = New System.Drawing.Size(434, 540)
    Me.tlpRecipeInfo.TabIndex = 0
    '
    'tlpSlider
    '
    Me.tlpSlider.BackColor = System.Drawing.Color.Tan
    Me.tlpSlider.ColumnCount = 1
    Me.tlpSlider.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpSlider.Controls.Add(Me.Label6, 0, 0)
    Me.tlpSlider.Controls.Add(Me.tbAmount, 0, 1)
    Me.tlpSlider.Controls.Add(Me.TableLayoutPanel2, 0, 2)
    Me.tlpSlider.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpSlider.Location = New System.Drawing.Point(3, 354)
    Me.tlpSlider.Name = "tlpSlider"
    Me.tlpSlider.RowCount = 3
    Me.tlpSlider.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
    Me.tlpSlider.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
    Me.tlpSlider.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
    Me.tlpSlider.Size = New System.Drawing.Size(428, 102)
    Me.tlpSlider.TabIndex = 20
    '
    'Label6
    '
    Me.Label6.AutoSize = True
    Me.Label6.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label6.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label6.Location = New System.Drawing.Point(3, 0)
    Me.Label6.Name = "Label6"
    Me.Label6.Size = New System.Drawing.Size(422, 30)
    Me.Label6.TabIndex = 3
    Me.Label6.Text = "Select Total Volume to Create:"
    Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'tbAmount
    '
    Me.tbAmount.BackColor = System.Drawing.Color.RosyBrown
    Me.tbAmount.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tbAmount.LargeChange = 8
    Me.tbAmount.Location = New System.Drawing.Point(3, 33)
    Me.tbAmount.Maximum = 34
    Me.tbAmount.Minimum = 2
    Me.tbAmount.Name = "tbAmount"
    Me.tbAmount.Size = New System.Drawing.Size(422, 24)
    Me.tbAmount.SmallChange = 8
    Me.tbAmount.TabIndex = 0
    Me.tbAmount.TickFrequency = 8
    Me.tbAmount.Value = 2
    '
    'TableLayoutPanel2
    '
    Me.TableLayoutPanel2.ColumnCount = 5
    Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.TableLayoutPanel2.Controls.Add(Me.Label5, 4, 0)
    Me.TableLayoutPanel2.Controls.Add(Me.Label4, 3, 0)
    Me.TableLayoutPanel2.Controls.Add(Me.Label3, 2, 0)
    Me.TableLayoutPanel2.Controls.Add(Me.Label1, 0, 0)
    Me.TableLayoutPanel2.Controls.Add(Me.Label2, 1, 0)
    Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
    Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 63)
    Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
    Me.TableLayoutPanel2.RowCount = 1
    Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.TableLayoutPanel2.Size = New System.Drawing.Size(422, 36)
    Me.TableLayoutPanel2.TabIndex = 1
    '
    'Label5
    '
    Me.Label5.AutoSize = True
    Me.Label5.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label5.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label5.Location = New System.Drawing.Point(339, 0)
    Me.Label5.Name = "Label5"
    Me.Label5.Size = New System.Drawing.Size(80, 36)
    Me.Label5.TabIndex = 4
    Me.Label5.Text = "32 oz"
    Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'Label4
    '
    Me.Label4.AutoSize = True
    Me.Label4.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label4.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label4.Location = New System.Drawing.Point(255, 0)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(78, 36)
    Me.Label4.TabIndex = 3
    Me.Label4.Text = "24 oz"
    Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label3.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label3.Location = New System.Drawing.Point(171, 0)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(78, 36)
    Me.Label3.TabIndex = 2
    Me.Label3.Text = "16 oz"
    Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label1.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label1.Location = New System.Drawing.Point(3, 0)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(78, 36)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "2 oz"
    Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label2.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label2.Location = New System.Drawing.Point(87, 0)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(78, 36)
    Me.Label2.TabIndex = 1
    Me.Label2.Text = "8 oz"
    Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'btnUseRecipe
    '
    Me.btnUseRecipe.AutoScaleBorder = 0
    Me.btnUseRecipe.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.create_flave_button
    Me.btnUseRecipe.BackColor = System.Drawing.Color.NavajoWhite
    Me.btnUseRecipe.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnUseRecipe.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
    Me.btnUseRecipe.FlatAppearance.BorderSize = 3
    Me.btnUseRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnUseRecipe.Font = New System.Drawing.Font("Poor Richard", 36.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnUseRecipe.Location = New System.Drawing.Point(3, 462)
    Me.btnUseRecipe.Name = "btnUseRecipe"
    Me.btnUseRecipe.Size = New System.Drawing.Size(428, 75)
    Me.btnUseRecipe.TabIndex = 18
    Me.btnUseRecipe.UseVisualStyleBackColor = False
    '
    'lstViewInfo
    '
    Me.lstViewInfo.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clmID, Me.clmIng, Me.clmParts})
    Me.lstViewInfo.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lstViewInfo.Font = New System.Drawing.Font("Poor Richard", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lstViewInfo.Location = New System.Drawing.Point(3, 84)
    Me.lstViewInfo.Name = "lstViewInfo"
    Me.lstViewInfo.Size = New System.Drawing.Size(428, 264)
    Me.lstViewInfo.TabIndex = 19
    Me.lstViewInfo.UseCompatibleStateImageBehavior = False
    Me.lstViewInfo.View = System.Windows.Forms.View.Details
    '
    'clmID
    '
    Me.clmID.Text = "ID"
    Me.clmID.Width = 0
    '
    'clmIng
    '
    Me.clmIng.Text = "Ingredient"
    Me.clmIng.Width = 300
    '
    'clmParts
    '
    Me.clmParts.Text = "Parts"
    Me.clmParts.Width = 100
    '
    'tlpStars
    '
    Me.tlpStars.BackColor = System.Drawing.Color.RosyBrown
    Me.tlpStars.ColumnCount = 5
    Me.tlpStars.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.tlpStars.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.tlpStars.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.tlpStars.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.tlpStars.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.tlpStars.Controls.Add(Me.pb5, 4, 0)
    Me.tlpStars.Controls.Add(Me.pb4, 3, 0)
    Me.tlpStars.Controls.Add(Me.pb3, 2, 0)
    Me.tlpStars.Controls.Add(Me.pb2, 1, 0)
    Me.tlpStars.Controls.Add(Me.pb1, 0, 0)
    Me.tlpStars.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpStars.Location = New System.Drawing.Point(3, 3)
    Me.tlpStars.Name = "tlpStars"
    Me.tlpStars.RowCount = 1
    Me.tlpStars.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpStars.Size = New System.Drawing.Size(428, 75)
    Me.tlpStars.TabIndex = 21
    '
    'pb5
    '
    Me.pb5.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pb5.Image = CType(resources.GetObject("pb5.Image"), System.Drawing.Image)
    Me.pb5.Location = New System.Drawing.Point(343, 3)
    Me.pb5.Name = "pb5"
    Me.pb5.Size = New System.Drawing.Size(82, 69)
    Me.pb5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pb5.TabIndex = 4
    Me.pb5.TabStop = False
    '
    'pb4
    '
    Me.pb4.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pb4.Image = CType(resources.GetObject("pb4.Image"), System.Drawing.Image)
    Me.pb4.Location = New System.Drawing.Point(258, 3)
    Me.pb4.Name = "pb4"
    Me.pb4.Size = New System.Drawing.Size(79, 69)
    Me.pb4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pb4.TabIndex = 3
    Me.pb4.TabStop = False
    '
    'pb3
    '
    Me.pb3.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pb3.Image = CType(resources.GetObject("pb3.Image"), System.Drawing.Image)
    Me.pb3.Location = New System.Drawing.Point(173, 3)
    Me.pb3.Name = "pb3"
    Me.pb3.Size = New System.Drawing.Size(79, 69)
    Me.pb3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pb3.TabIndex = 2
    Me.pb3.TabStop = False
    '
    'pb2
    '
    Me.pb2.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pb2.Image = CType(resources.GetObject("pb2.Image"), System.Drawing.Image)
    Me.pb2.Location = New System.Drawing.Point(88, 3)
    Me.pb2.Name = "pb2"
    Me.pb2.Size = New System.Drawing.Size(79, 69)
    Me.pb2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pb2.TabIndex = 1
    Me.pb2.TabStop = False
    '
    'pb1
    '
    Me.pb1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pb1.Image = CType(resources.GetObject("pb1.Image"), System.Drawing.Image)
    Me.pb1.Location = New System.Drawing.Point(3, 3)
    Me.pb1.Name = "pb1"
    Me.pb1.Size = New System.Drawing.Size(79, 69)
    Me.pb1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pb1.TabIndex = 0
    Me.pb1.TabStop = False
    '
    'lstAmounts
    '
    Me.lstAmounts.BackColor = System.Drawing.SystemColors.HighlightText
    Me.lstAmounts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.lstAmounts.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lstAmounts.FormattingEnabled = True
    Me.lstAmounts.ItemHeight = 15
    Me.lstAmounts.Location = New System.Drawing.Point(64, 5)
    Me.lstAmounts.Margin = New System.Windows.Forms.Padding(2)
    Me.lstAmounts.Name = "lstAmounts"
    Me.lstAmounts.SelectionMode = System.Windows.Forms.SelectionMode.None
    Me.lstAmounts.Size = New System.Drawing.Size(84, 17)
    Me.lstAmounts.TabIndex = 19
    '
    'lstRecipesInfo
    '
    Me.lstRecipesInfo.BackColor = System.Drawing.SystemColors.HighlightText
    Me.lstRecipesInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.lstRecipesInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lstRecipesInfo.FormattingEnabled = True
    Me.lstRecipesInfo.ItemHeight = 15
    Me.lstRecipesInfo.Location = New System.Drawing.Point(152, 5)
    Me.lstRecipesInfo.Margin = New System.Windows.Forms.Padding(2)
    Me.lstRecipesInfo.Name = "lstRecipesInfo"
    Me.lstRecipesInfo.SelectionMode = System.Windows.Forms.SelectionMode.None
    Me.lstRecipesInfo.Size = New System.Drawing.Size(100, 17)
    Me.lstRecipesInfo.TabIndex = 1
    '
    'pnlRecipeStatus
    '
    Me.pnlRecipeStatus.BackColor = System.Drawing.Color.Wheat
    Me.pnlRecipeStatus.Controls.Add(Me.lstRating)
    Me.pnlRecipeStatus.Controls.Add(Me.lstIngID)
    Me.pnlRecipeStatus.Controls.Add(Me.lstRecipesInfo)
    Me.pnlRecipeStatus.Controls.Add(Me.lstID)
    Me.pnlRecipeStatus.Controls.Add(Me.lstAmounts)
    Me.pnlRecipeStatus.Controls.Add(Me.pBarRecipe)
    Me.pnlRecipeStatus.Controls.Add(Me.lblRecipeStatus)
    Me.pnlRecipeStatus.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.pnlRecipeStatus.Location = New System.Drawing.Point(0, 735)
    Me.pnlRecipeStatus.Margin = New System.Windows.Forms.Padding(2)
    Me.pnlRecipeStatus.Name = "pnlRecipeStatus"
    Me.pnlRecipeStatus.Size = New System.Drawing.Size(1262, 24)
    Me.pnlRecipeStatus.TabIndex = 95
    Me.pnlRecipeStatus.Visible = False
    '
    'lstRating
    '
    Me.lstRating.FormattingEnabled = True
    Me.lstRating.Location = New System.Drawing.Point(315, 5)
    Me.lstRating.Margin = New System.Windows.Forms.Padding(2)
    Me.lstRating.Name = "lstRating"
    Me.lstRating.Size = New System.Drawing.Size(54, 17)
    Me.lstRating.TabIndex = 22
    '
    'lstIngID
    '
    Me.lstIngID.FormattingEnabled = True
    Me.lstIngID.Location = New System.Drawing.Point(257, 5)
    Me.lstIngID.Margin = New System.Windows.Forms.Padding(2)
    Me.lstIngID.Name = "lstIngID"
    Me.lstIngID.Size = New System.Drawing.Size(54, 17)
    Me.lstIngID.TabIndex = 21
    '
    'pBarRecipe
    '
    Me.pBarRecipe.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pBarRecipe.ForeColor = System.Drawing.Color.LimeGreen
    Me.pBarRecipe.Location = New System.Drawing.Point(1089, 2)
    Me.pBarRecipe.Margin = New System.Windows.Forms.Padding(2)
    Me.pBarRecipe.Name = "pBarRecipe"
    Me.pBarRecipe.Size = New System.Drawing.Size(170, 19)
    Me.pBarRecipe.TabIndex = 0
    Me.pBarRecipe.Visible = False
    '
    'lblRecipeStatus
    '
    Me.lblRecipeStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblRecipeStatus.AutoSize = True
    Me.lblRecipeStatus.Location = New System.Drawing.Point(1003, 7)
    Me.lblRecipeStatus.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
    Me.lblRecipeStatus.Name = "lblRecipeStatus"
    Me.lblRecipeStatus.Size = New System.Drawing.Size(77, 13)
    Me.lblRecipeStatus.TabIndex = 1
    Me.lblRecipeStatus.Text = "Recipe Status:"
    Me.lblRecipeStatus.Visible = False
    '
    'pnlMain
    '
    Me.pnlMain.BackColor = System.Drawing.Color.PapayaWhip
    Me.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.pnlMain.Controls.Add(Me.tlpHeader)
    Me.pnlMain.Controls.Add(Me.pnlRecipeStatus)
    Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlMain.Location = New System.Drawing.Point(0, 0)
    Me.pnlMain.Margin = New System.Windows.Forms.Padding(2)
    Me.pnlMain.Name = "pnlMain"
    Me.pnlMain.Size = New System.Drawing.Size(1264, 761)
    Me.pnlMain.TabIndex = 2
    '
    'tlpHeader
    '
    Me.tlpHeader.ColumnCount = 1
    Me.tlpHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpHeader.Controls.Add(Me.tlpMain, 0, 2)
    Me.tlpHeader.Controls.Add(Me.TableLayoutPanel1, 0, 1)
    Me.tlpHeader.Controls.Add(Me.PictureBox1, 0, 0)
    Me.tlpHeader.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpHeader.Location = New System.Drawing.Point(0, 0)
    Me.tlpHeader.Name = "tlpHeader"
    Me.tlpHeader.RowCount = 3
    Me.tlpHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
    Me.tlpHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
    Me.tlpHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.0!))
    Me.tlpHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.tlpHeader.Size = New System.Drawing.Size(1262, 735)
    Me.tlpHeader.TabIndex = 3
    '
    'TableLayoutPanel1
    '
    Me.TableLayoutPanel1.ColumnCount = 5
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.0!))
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.0!))
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.0!))
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.0!))
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.0!))
    Me.TableLayoutPanel1.Controls.Add(Me.btnUsers, 0, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.btnRefPage, 4, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.btnSettings, 3, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.btnNewFlave, 2, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.btnMainPage, 1, 0)
    Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 113)
    Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
    Me.TableLayoutPanel1.RowCount = 1
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.TableLayoutPanel1.Size = New System.Drawing.Size(1256, 67)
    Me.TableLayoutPanel1.TabIndex = 98
    '
    'btnUsers
    '
    Me.btnUsers.AutoScaleBorder = 0
    Me.btnUsers.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.user_button
    Me.btnUsers.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnUsers.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnUsers.FlatAppearance.BorderSize = 2
    Me.btnUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnUsers.Location = New System.Drawing.Point(3, 3)
    Me.btnUsers.Name = "btnUsers"
    Me.btnUsers.Size = New System.Drawing.Size(81, 61)
    Me.btnUsers.TabIndex = 4
    Me.btnUsers.UseVisualStyleBackColor = False
    '
    'btnRefPage
    '
    Me.btnRefPage.AutoScaleBorder = 0
    Me.btnRefPage.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.refresh_page_button
    Me.btnRefPage.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnRefPage.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnRefPage.FlatAppearance.BorderSize = 2
    Me.btnRefPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnRefPage.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnRefPage.Location = New System.Drawing.Point(1169, 3)
    Me.btnRefPage.Name = "btnRefPage"
    Me.btnRefPage.Size = New System.Drawing.Size(84, 61)
    Me.btnRefPage.TabIndex = 3
    Me.btnRefPage.UseVisualStyleBackColor = False
    '
    'btnSettings
    '
    Me.btnSettings.AutoScaleBorder = 0
    Me.btnSettings.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.Settings_button
    Me.btnSettings.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnSettings.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnSettings.FlatAppearance.BorderSize = 2
    Me.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnSettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnSettings.Location = New System.Drawing.Point(805, 3)
    Me.btnSettings.Name = "btnSettings"
    Me.btnSettings.Size = New System.Drawing.Size(358, 61)
    Me.btnSettings.TabIndex = 2
    Me.btnSettings.UseVisualStyleBackColor = False
    '
    'btnNewFlave
    '
    Me.btnNewFlave.AutoScaleBorder = 0
    Me.btnNewFlave.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.Create_New_button1
    Me.btnNewFlave.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnNewFlave.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnNewFlave.FlatAppearance.BorderSize = 2
    Me.btnNewFlave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnNewFlave.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnNewFlave.Location = New System.Drawing.Point(454, 3)
    Me.btnNewFlave.Name = "btnNewFlave"
    Me.btnNewFlave.Size = New System.Drawing.Size(345, 61)
    Me.btnNewFlave.TabIndex = 1
    Me.btnNewFlave.UseVisualStyleBackColor = False
    '
    'btnMainPage
    '
    Me.btnMainPage.AutoScaleBorder = 0
    Me.btnMainPage.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.Home_button
    Me.btnMainPage.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnMainPage.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnMainPage.FlatAppearance.BorderSize = 2
    Me.btnMainPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnMainPage.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnMainPage.Location = New System.Drawing.Point(90, 3)
    Me.btnMainPage.Name = "btnMainPage"
    Me.btnMainPage.Size = New System.Drawing.Size(358, 61)
    Me.btnMainPage.TabIndex = 0
    Me.btnMainPage.UseVisualStyleBackColor = False
    '
    'PictureBox1
    '
    Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.PictureBox1.Image = Global.RootBeerMixer.My.Resources.Resources.Fave_Flaves_Banner
    Me.PictureBox1.Location = New System.Drawing.Point(3, 3)
    Me.PictureBox1.Name = "PictureBox1"
    Me.PictureBox1.Size = New System.Drawing.Size(1256, 104)
    Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.PictureBox1.TabIndex = 99
    Me.PictureBox1.TabStop = False
    '
    'tTimer2
    '
    '
    'frmMyFaves
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(1264, 761)
    Me.Controls.Add(Me.pnlMain)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Margin = New System.Windows.Forms.Padding(2)
    Me.Name = "frmMyFaves"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "My Faves"
    Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
    Me.tlpMain.ResumeLayout(False)
    Me.pnlLeft.ResumeLayout(False)
    Me.tlpButtons.ResumeLayout(False)
    Me.tlpButtons.PerformLayout()
    Me.pnlMiddle.ResumeLayout(False)
    Me.pnlRight.ResumeLayout(False)
    Me.tlpRecipeInfo.ResumeLayout(False)
    Me.tlpSlider.ResumeLayout(False)
    Me.tlpSlider.PerformLayout()
    CType(Me.tbAmount, System.ComponentModel.ISupportInitialize).EndInit()
    Me.TableLayoutPanel2.ResumeLayout(False)
    Me.TableLayoutPanel2.PerformLayout()
    Me.tlpStars.ResumeLayout(False)
    CType(Me.pb5, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pb4, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pb3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pb2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pb1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlRecipeStatus.ResumeLayout(False)
    Me.pnlRecipeStatus.PerformLayout()
    Me.pnlMain.ResumeLayout(False)
    Me.tlpHeader.ResumeLayout(False)
    Me.TableLayoutPanel1.ResumeLayout(False)
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents lstID As System.Windows.Forms.ListBox
  Friend WithEvents tlpMain As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlLeft As System.Windows.Forms.Panel
  Friend WithEvents tlpButtons As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents btnReloadList As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnSearchRecipes As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnAddRecipe As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnDeleteRecipe As AutoScaleButton.AutoScaleButton
  Friend WithEvents txtSearchBox As System.Windows.Forms.TextBox
  Friend WithEvents pnlMiddle As System.Windows.Forms.Panel
  Friend WithEvents lstRecipesBox As System.Windows.Forms.ListBox
  Friend WithEvents pnlRight As System.Windows.Forms.Panel
  Friend WithEvents tlpRecipeInfo As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents btnUseRecipe As AutoScaleButton.AutoScaleButton
  Friend WithEvents lstAmounts As System.Windows.Forms.ListBox
  Friend WithEvents lstRecipesInfo As System.Windows.Forms.ListBox
  Friend WithEvents pnlRecipeStatus As System.Windows.Forms.Panel
  Friend WithEvents pBarRecipe As System.Windows.Forms.ProgressBar
  Friend WithEvents lblRecipeStatus As System.Windows.Forms.Label
  Friend WithEvents pnlMain As System.Windows.Forms.Panel
  Friend WithEvents tlpHeader As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents btnMainPage As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnNewFlave As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnSettings As AutoScaleButton.AutoScaleButton
  Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
  Friend WithEvents btnRefPage As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnUsers As AutoScaleButton.AutoScaleButton
  Friend WithEvents tTimer2 As System.Windows.Forms.Timer
  Friend WithEvents lstViewInfo As System.Windows.Forms.ListView
  Friend WithEvents clmID As System.Windows.Forms.ColumnHeader
  Friend WithEvents clmIng As System.Windows.Forms.ColumnHeader
  Friend WithEvents clmParts As System.Windows.Forms.ColumnHeader
  Friend WithEvents lstIngID As System.Windows.Forms.ListBox
  Friend WithEvents tlpSlider As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents Label6 As System.Windows.Forms.Label
  Friend WithEvents tbAmount As System.Windows.Forms.TrackBar
  Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents Label4 As System.Windows.Forms.Label
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents tlpStars As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pb5 As System.Windows.Forms.PictureBox
  Friend WithEvents pb4 As System.Windows.Forms.PictureBox
  Friend WithEvents pb3 As System.Windows.Forms.PictureBox
  Friend WithEvents pb2 As System.Windows.Forms.PictureBox
  Friend WithEvents pb1 As System.Windows.Forms.PictureBox
  Friend WithEvents lstRating As System.Windows.Forms.ListBox
End Class
