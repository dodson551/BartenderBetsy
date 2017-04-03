<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreate_Flave
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCreate_Flave))
    Me.Panel1 = New System.Windows.Forms.Panel()
    Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
    Me.tlpNavigation = New System.Windows.Forms.TableLayoutPanel()
    Me.btnRefPage = New AutoScaleButton.AutoScaleButton()
    Me.btnSettings = New AutoScaleButton.AutoScaleButton()
    Me.btnFavorite = New AutoScaleButton.AutoScaleButton()
    Me.btnHome = New AutoScaleButton.AutoScaleButton()
    Me.btnUsers = New AutoScaleButton.AutoScaleButton()
    Me.tlpMiddle = New System.Windows.Forms.TableLayoutPanel()
    Me.PictureBox6 = New System.Windows.Forms.PictureBox()
    Me.lstBoxIngs = New System.Windows.Forms.ListBox()
    Me.tlpLeftMiddle = New System.Windows.Forms.TableLayoutPanel()
    Me.btnFlavor = New AutoScaleButton.AutoScaleButton()
    Me.tlpAmounts = New System.Windows.Forms.TableLayoutPanel()
    Me.btnAm7 = New System.Windows.Forms.Button()
    Me.btnAm8 = New System.Windows.Forms.Button()
    Me.btnAm9 = New System.Windows.Forms.Button()
    Me.btnAm1 = New System.Windows.Forms.Button()
    Me.btnAm2 = New System.Windows.Forms.Button()
    Me.btnAm3 = New System.Windows.Forms.Button()
    Me.btnAm4 = New System.Windows.Forms.Button()
    Me.btnAm5 = New System.Windows.Forms.Button()
    Me.btnAm6 = New System.Windows.Forms.Button()
    Me.tlpBottom = New System.Windows.Forms.TableLayoutPanel()
    Me.lstViewCreatedRecipe = New System.Windows.Forms.ListView()
    Me.clmID = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
    Me.clmIngs = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
    Me.clmParts = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
    Me.tlpButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.tlpSlider = New System.Windows.Forms.TableLayoutPanel()
    Me.Label6 = New System.Windows.Forms.Label()
    Me.tbAmount = New System.Windows.Forms.TrackBar()
    Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
    Me.Label5 = New System.Windows.Forms.Label()
    Me.Label4 = New System.Windows.Forms.Label()
    Me.Label3 = New System.Windows.Forms.Label()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.btnClearAll = New AutoScaleButton.AutoScaleButton()
    Me.btnRemove = New AutoScaleButton.AutoScaleButton()
    Me.btnCreateRecipe = New AutoScaleButton.AutoScaleButton()
    Me.PictureBox1 = New System.Windows.Forms.PictureBox()
    Me.pnlRecipeStatus = New System.Windows.Forms.Panel()
    Me.txtPart = New System.Windows.Forms.TextBox()
    Me.lblam = New System.Windows.Forms.Label()
    Me.lblRecipeStatus = New System.Windows.Forms.Label()
    Me.pBarRecipe = New System.Windows.Forms.ProgressBar()
    Me.lstIDbox = New System.Windows.Forms.ListBox()
    Me.ListBox1 = New System.Windows.Forms.ListBox()
    Me.lstIngName = New System.Windows.Forms.ListBox()
    Me.tTimer2 = New System.Windows.Forms.Timer(Me.components)
    Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
    Me.Panel1.SuspendLayout()
    Me.tlpMain.SuspendLayout()
    Me.tlpNavigation.SuspendLayout()
    Me.tlpMiddle.SuspendLayout()
    CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.tlpLeftMiddle.SuspendLayout()
    Me.tlpAmounts.SuspendLayout()
    Me.tlpBottom.SuspendLayout()
    Me.tlpButtons.SuspendLayout()
    Me.tlpSlider.SuspendLayout()
    CType(Me.tbAmount, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.TableLayoutPanel1.SuspendLayout()
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlRecipeStatus.SuspendLayout()
    Me.SuspendLayout()
    '
    'Panel1
    '
    Me.Panel1.BackColor = System.Drawing.Color.Tan
    Me.Panel1.Controls.Add(Me.tlpMain)
    Me.Panel1.Controls.Add(Me.pnlRecipeStatus)
    Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Panel1.ForeColor = System.Drawing.Color.Black
    Me.Panel1.Location = New System.Drawing.Point(0, 0)
    Me.Panel1.Margin = New System.Windows.Forms.Padding(2)
    Me.Panel1.Name = "Panel1"
    Me.Panel1.Size = New System.Drawing.Size(1264, 761)
    Me.Panel1.TabIndex = 2
    '
    'tlpMain
    '
    Me.tlpMain.ColumnCount = 1
    Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpMain.Controls.Add(Me.tlpNavigation, 0, 1)
    Me.tlpMain.Controls.Add(Me.tlpMiddle, 0, 2)
    Me.tlpMain.Controls.Add(Me.tlpBottom, 0, 3)
    Me.tlpMain.Controls.Add(Me.PictureBox1, 0, 0)
    Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpMain.Location = New System.Drawing.Point(0, 0)
    Me.tlpMain.Name = "tlpMain"
    Me.tlpMain.RowCount = 4
    Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
    Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
    Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.0!))
    Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
    Me.tlpMain.Size = New System.Drawing.Size(1264, 737)
    Me.tlpMain.TabIndex = 95
    '
    'tlpNavigation
    '
    Me.tlpNavigation.ColumnCount = 5
    Me.tlpNavigation.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.0!))
    Me.tlpNavigation.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.0!))
    Me.tlpNavigation.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.0!))
    Me.tlpNavigation.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.0!))
    Me.tlpNavigation.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.0!))
    Me.tlpNavigation.Controls.Add(Me.btnRefPage, 4, 0)
    Me.tlpNavigation.Controls.Add(Me.btnSettings, 3, 0)
    Me.tlpNavigation.Controls.Add(Me.btnFavorite, 2, 0)
    Me.tlpNavigation.Controls.Add(Me.btnHome, 1, 0)
    Me.tlpNavigation.Controls.Add(Me.btnUsers, 0, 0)
    Me.tlpNavigation.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpNavigation.Location = New System.Drawing.Point(3, 113)
    Me.tlpNavigation.Name = "tlpNavigation"
    Me.tlpNavigation.RowCount = 1
    Me.tlpNavigation.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpNavigation.Size = New System.Drawing.Size(1258, 67)
    Me.tlpNavigation.TabIndex = 0
    '
    'btnRefPage
    '
    Me.btnRefPage.AutoScaleBorder = 0
    Me.btnRefPage.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.refresh_page_button
    Me.btnRefPage.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnRefPage.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnRefPage.FlatAppearance.BorderSize = 2
    Me.btnRefPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnRefPage.Location = New System.Drawing.Point(1171, 3)
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
    Me.btnSettings.Font = New System.Drawing.Font("Poor Richard", 26.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnSettings.Location = New System.Drawing.Point(807, 3)
    Me.btnSettings.Name = "btnSettings"
    Me.btnSettings.Size = New System.Drawing.Size(358, 61)
    Me.btnSettings.TabIndex = 2
    Me.btnSettings.UseVisualStyleBackColor = False
    '
    'btnFavorite
    '
    Me.btnFavorite.AutoScaleBorder = 0
    Me.btnFavorite.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.Favorites_button
    Me.btnFavorite.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnFavorite.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnFavorite.FlatAppearance.BorderSize = 2
    Me.btnFavorite.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnFavorite.Font = New System.Drawing.Font("Poor Richard", 26.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnFavorite.Location = New System.Drawing.Point(455, 3)
    Me.btnFavorite.Name = "btnFavorite"
    Me.btnFavorite.Size = New System.Drawing.Size(346, 61)
    Me.btnFavorite.TabIndex = 1
    Me.btnFavorite.UseVisualStyleBackColor = False
    '
    'btnHome
    '
    Me.btnHome.AutoScaleBorder = 0
    Me.btnHome.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.Home_button
    Me.btnHome.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnHome.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnHome.FlatAppearance.BorderSize = 2
    Me.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnHome.Font = New System.Drawing.Font("Poor Richard", 26.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnHome.Location = New System.Drawing.Point(91, 3)
    Me.btnHome.Name = "btnHome"
    Me.btnHome.Size = New System.Drawing.Size(358, 61)
    Me.btnHome.TabIndex = 0
    Me.btnHome.UseVisualStyleBackColor = False
    '
    'btnUsers
    '
    Me.btnUsers.AutoScaleBorder = 0
    Me.btnUsers.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.user_button
    Me.btnUsers.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnUsers.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnUsers.FlatAppearance.BorderSize = 2
    Me.btnUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnUsers.Location = New System.Drawing.Point(3, 3)
    Me.btnUsers.Name = "btnUsers"
    Me.btnUsers.Size = New System.Drawing.Size(82, 61)
    Me.btnUsers.TabIndex = 4
    Me.btnUsers.UseVisualStyleBackColor = False
    '
    'tlpMiddle
    '
    Me.tlpMiddle.ColumnCount = 3
    Me.tlpMiddle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.0!))
    Me.tlpMiddle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.75!))
    Me.tlpMiddle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.25!))
    Me.tlpMiddle.Controls.Add(Me.PictureBox6, 2, 0)
    Me.tlpMiddle.Controls.Add(Me.lstBoxIngs, 1, 0)
    Me.tlpMiddle.Controls.Add(Me.tlpLeftMiddle, 0, 0)
    Me.tlpMiddle.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpMiddle.Location = New System.Drawing.Point(3, 186)
    Me.tlpMiddle.Name = "tlpMiddle"
    Me.tlpMiddle.RowCount = 1
    Me.tlpMiddle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpMiddle.Size = New System.Drawing.Size(1258, 325)
    Me.tlpMiddle.TabIndex = 94
    '
    'PictureBox6
    '
    Me.PictureBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.PictureBox6.Dock = System.Windows.Forms.DockStyle.Fill
    Me.PictureBox6.Location = New System.Drawing.Point(840, 2)
    Me.PictureBox6.Margin = New System.Windows.Forms.Padding(2)
    Me.PictureBox6.Name = "PictureBox6"
    Me.PictureBox6.Size = New System.Drawing.Size(416, 321)
    Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.PictureBox6.TabIndex = 93
    Me.PictureBox6.TabStop = False
    '
    'lstBoxIngs
    '
    Me.lstBoxIngs.BackColor = System.Drawing.SystemColors.Menu
    Me.lstBoxIngs.ColumnWidth = 300
    Me.lstBoxIngs.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lstBoxIngs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
    Me.lstBoxIngs.Font = New System.Drawing.Font("Poor Richard", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lstBoxIngs.FormattingEnabled = True
    Me.lstBoxIngs.ItemHeight = 40
    Me.lstBoxIngs.Location = New System.Drawing.Point(303, 2)
    Me.lstBoxIngs.Margin = New System.Windows.Forms.Padding(2)
    Me.lstBoxIngs.Name = "lstBoxIngs"
    Me.lstBoxIngs.ScrollAlwaysVisible = True
    Me.lstBoxIngs.Size = New System.Drawing.Size(533, 321)
    Me.lstBoxIngs.TabIndex = 86
    '
    'tlpLeftMiddle
    '
    Me.tlpLeftMiddle.ColumnCount = 1
    Me.tlpLeftMiddle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpLeftMiddle.Controls.Add(Me.btnFlavor, 0, 0)
    Me.tlpLeftMiddle.Controls.Add(Me.tlpAmounts, 0, 1)
    Me.tlpLeftMiddle.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpLeftMiddle.Location = New System.Drawing.Point(3, 3)
    Me.tlpLeftMiddle.Name = "tlpLeftMiddle"
    Me.tlpLeftMiddle.RowCount = 2
    Me.tlpLeftMiddle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.tlpLeftMiddle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.0!))
    Me.tlpLeftMiddle.Size = New System.Drawing.Size(295, 319)
    Me.tlpLeftMiddle.TabIndex = 94
    '
    'btnFlavor
    '
    Me.btnFlavor.AutoScaleBorder = 0
    Me.btnFlavor.AutoScaleImage = Nothing
    Me.btnFlavor.BackColor = System.Drawing.Color.RosyBrown
    Me.btnFlavor.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnFlavor.Enabled = False
    Me.btnFlavor.FlatAppearance.BorderSize = 2
    Me.btnFlavor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnFlavor.Font = New System.Drawing.Font("Poor Richard", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnFlavor.Location = New System.Drawing.Point(3, 3)
    Me.btnFlavor.Name = "btnFlavor"
    Me.btnFlavor.Size = New System.Drawing.Size(289, 57)
    Me.btnFlavor.TabIndex = 90
    Me.btnFlavor.Text = "Vanilla"
    Me.btnFlavor.UseVisualStyleBackColor = False
    '
    'tlpAmounts
    '
    Me.tlpAmounts.ColumnCount = 3
    Me.tlpAmounts.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.tlpAmounts.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.tlpAmounts.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.tlpAmounts.Controls.Add(Me.btnAm7, 0, 2)
    Me.tlpAmounts.Controls.Add(Me.btnAm8, 0, 2)
    Me.tlpAmounts.Controls.Add(Me.btnAm9, 0, 2)
    Me.tlpAmounts.Controls.Add(Me.btnAm1, 0, 0)
    Me.tlpAmounts.Controls.Add(Me.btnAm2, 1, 0)
    Me.tlpAmounts.Controls.Add(Me.btnAm3, 2, 0)
    Me.tlpAmounts.Controls.Add(Me.btnAm4, 0, 1)
    Me.tlpAmounts.Controls.Add(Me.btnAm5, 1, 1)
    Me.tlpAmounts.Controls.Add(Me.btnAm6, 2, 1)
    Me.tlpAmounts.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpAmounts.Location = New System.Drawing.Point(3, 66)
    Me.tlpAmounts.Name = "tlpAmounts"
    Me.tlpAmounts.RowCount = 3
    Me.tlpAmounts.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.tlpAmounts.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.tlpAmounts.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.tlpAmounts.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.tlpAmounts.Size = New System.Drawing.Size(289, 250)
    Me.tlpAmounts.TabIndex = 91
    '
    'btnAm7
    '
    Me.btnAm7.BackColor = System.Drawing.Color.Wheat
    Me.btnAm7.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnAm7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnAm7.Font = New System.Drawing.Font("Poor Richard", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnAm7.Location = New System.Drawing.Point(2, 168)
    Me.btnAm7.Margin = New System.Windows.Forms.Padding(2)
    Me.btnAm7.Name = "btnAm7"
    Me.btnAm7.Size = New System.Drawing.Size(92, 80)
    Me.btnAm7.TabIndex = 74
    Me.btnAm7.Text = "7 Parts"
    Me.btnAm7.UseVisualStyleBackColor = False
    '
    'btnAm8
    '
    Me.btnAm8.BackColor = System.Drawing.Color.Wheat
    Me.btnAm8.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnAm8.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnAm8.Font = New System.Drawing.Font("Poor Richard", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnAm8.Location = New System.Drawing.Point(98, 168)
    Me.btnAm8.Margin = New System.Windows.Forms.Padding(2)
    Me.btnAm8.Name = "btnAm8"
    Me.btnAm8.Size = New System.Drawing.Size(92, 80)
    Me.btnAm8.TabIndex = 73
    Me.btnAm8.Text = "8 Parts"
    Me.btnAm8.UseVisualStyleBackColor = False
    '
    'btnAm9
    '
    Me.btnAm9.BackColor = System.Drawing.Color.Wheat
    Me.btnAm9.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnAm9.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnAm9.Font = New System.Drawing.Font("Poor Richard", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnAm9.Location = New System.Drawing.Point(194, 168)
    Me.btnAm9.Margin = New System.Windows.Forms.Padding(2)
    Me.btnAm9.Name = "btnAm9"
    Me.btnAm9.Size = New System.Drawing.Size(93, 80)
    Me.btnAm9.TabIndex = 72
    Me.btnAm9.Text = "9 Parts"
    Me.btnAm9.UseVisualStyleBackColor = False
    '
    'btnAm1
    '
    Me.btnAm1.BackColor = System.Drawing.Color.Wheat
    Me.btnAm1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnAm1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnAm1.Font = New System.Drawing.Font("Poor Richard", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnAm1.Location = New System.Drawing.Point(2, 2)
    Me.btnAm1.Margin = New System.Windows.Forms.Padding(2)
    Me.btnAm1.Name = "btnAm1"
    Me.btnAm1.Size = New System.Drawing.Size(92, 79)
    Me.btnAm1.TabIndex = 66
    Me.btnAm1.Text = "1 Part"
    Me.btnAm1.UseVisualStyleBackColor = False
    '
    'btnAm2
    '
    Me.btnAm2.BackColor = System.Drawing.Color.Wheat
    Me.btnAm2.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnAm2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnAm2.Font = New System.Drawing.Font("Poor Richard", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnAm2.Location = New System.Drawing.Point(98, 2)
    Me.btnAm2.Margin = New System.Windows.Forms.Padding(2)
    Me.btnAm2.Name = "btnAm2"
    Me.btnAm2.Size = New System.Drawing.Size(92, 79)
    Me.btnAm2.TabIndex = 67
    Me.btnAm2.Text = "2 Parts"
    Me.btnAm2.UseVisualStyleBackColor = False
    '
    'btnAm3
    '
    Me.btnAm3.BackColor = System.Drawing.Color.Wheat
    Me.btnAm3.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnAm3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnAm3.Font = New System.Drawing.Font("Poor Richard", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnAm3.Location = New System.Drawing.Point(194, 2)
    Me.btnAm3.Margin = New System.Windows.Forms.Padding(2)
    Me.btnAm3.Name = "btnAm3"
    Me.btnAm3.Size = New System.Drawing.Size(93, 79)
    Me.btnAm3.TabIndex = 68
    Me.btnAm3.Text = "3 Parts"
    Me.btnAm3.UseVisualStyleBackColor = False
    '
    'btnAm4
    '
    Me.btnAm4.BackColor = System.Drawing.Color.Wheat
    Me.btnAm4.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnAm4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnAm4.Font = New System.Drawing.Font("Poor Richard", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnAm4.Location = New System.Drawing.Point(2, 85)
    Me.btnAm4.Margin = New System.Windows.Forms.Padding(2)
    Me.btnAm4.Name = "btnAm4"
    Me.btnAm4.Size = New System.Drawing.Size(92, 79)
    Me.btnAm4.TabIndex = 69
    Me.btnAm4.Text = "4 Parts"
    Me.btnAm4.UseVisualStyleBackColor = False
    '
    'btnAm5
    '
    Me.btnAm5.BackColor = System.Drawing.Color.Wheat
    Me.btnAm5.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnAm5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnAm5.Font = New System.Drawing.Font("Poor Richard", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnAm5.Location = New System.Drawing.Point(98, 85)
    Me.btnAm5.Margin = New System.Windows.Forms.Padding(2)
    Me.btnAm5.Name = "btnAm5"
    Me.btnAm5.Size = New System.Drawing.Size(92, 79)
    Me.btnAm5.TabIndex = 70
    Me.btnAm5.Text = "5 Parts"
    Me.btnAm5.UseVisualStyleBackColor = False
    '
    'btnAm6
    '
    Me.btnAm6.BackColor = System.Drawing.Color.Wheat
    Me.btnAm6.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnAm6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnAm6.Font = New System.Drawing.Font("Poor Richard", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnAm6.Location = New System.Drawing.Point(194, 85)
    Me.btnAm6.Margin = New System.Windows.Forms.Padding(2)
    Me.btnAm6.Name = "btnAm6"
    Me.btnAm6.Size = New System.Drawing.Size(93, 79)
    Me.btnAm6.TabIndex = 71
    Me.btnAm6.Text = "6 Parts"
    Me.btnAm6.UseVisualStyleBackColor = False
    '
    'tlpBottom
    '
    Me.tlpBottom.ColumnCount = 3
    Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.0!))
    Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.75!))
    Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.25!))
    Me.tlpBottom.Controls.Add(Me.lstViewCreatedRecipe, 1, 0)
    Me.tlpBottom.Controls.Add(Me.tlpButtons, 0, 0)
    Me.tlpBottom.Controls.Add(Me.btnCreateRecipe, 2, 0)
    Me.tlpBottom.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpBottom.Location = New System.Drawing.Point(3, 517)
    Me.tlpBottom.Name = "tlpBottom"
    Me.tlpBottom.RowCount = 1
    Me.tlpBottom.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpBottom.Size = New System.Drawing.Size(1258, 217)
    Me.tlpBottom.TabIndex = 95
    '
    'lstViewCreatedRecipe
    '
    Me.lstViewCreatedRecipe.BackColor = System.Drawing.SystemColors.Menu
    Me.lstViewCreatedRecipe.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clmID, Me.clmIngs, Me.clmParts})
    Me.lstViewCreatedRecipe.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lstViewCreatedRecipe.Font = New System.Drawing.Font("Poor Richard", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lstViewCreatedRecipe.FullRowSelect = True
    Me.lstViewCreatedRecipe.GridLines = True
    Me.lstViewCreatedRecipe.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
    Me.lstViewCreatedRecipe.Location = New System.Drawing.Point(303, 2)
    Me.lstViewCreatedRecipe.Margin = New System.Windows.Forms.Padding(2)
    Me.lstViewCreatedRecipe.Name = "lstViewCreatedRecipe"
    Me.lstViewCreatedRecipe.Size = New System.Drawing.Size(533, 213)
    Me.lstViewCreatedRecipe.TabIndex = 65
    Me.lstViewCreatedRecipe.UseCompatibleStateImageBehavior = False
    Me.lstViewCreatedRecipe.View = System.Windows.Forms.View.Details
    '
    'clmID
    '
    Me.clmID.Text = "ID"
    Me.clmID.Width = 0
    '
    'clmIngs
    '
    Me.clmIngs.Text = "Ingredients"
    Me.clmIngs.Width = 400
    '
    'clmParts
    '
    Me.clmParts.Text = "Parts"
    Me.clmParts.Width = 125
    '
    'tlpButtons
    '
    Me.tlpButtons.ColumnCount = 1
    Me.tlpButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpButtons.Controls.Add(Me.tlpSlider, 0, 0)
    Me.tlpButtons.Controls.Add(Me.btnClearAll, 0, 2)
    Me.tlpButtons.Controls.Add(Me.btnRemove, 0, 1)
    Me.tlpButtons.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpButtons.Location = New System.Drawing.Point(3, 3)
    Me.tlpButtons.Name = "tlpButtons"
    Me.tlpButtons.RowCount = 3
    Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
    Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
    Me.tlpButtons.Size = New System.Drawing.Size(295, 211)
    Me.tlpButtons.TabIndex = 66
    '
    'tlpSlider
    '
    Me.tlpSlider.BackColor = System.Drawing.Color.Tan
    Me.tlpSlider.ColumnCount = 1
    Me.tlpSlider.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpSlider.Controls.Add(Me.Label6, 0, 0)
    Me.tlpSlider.Controls.Add(Me.tbAmount, 0, 1)
    Me.tlpSlider.Controls.Add(Me.TableLayoutPanel1, 0, 2)
    Me.tlpSlider.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpSlider.Location = New System.Drawing.Point(3, 3)
    Me.tlpSlider.Name = "tlpSlider"
    Me.tlpSlider.RowCount = 3
    Me.tlpSlider.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
    Me.tlpSlider.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
    Me.tlpSlider.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
    Me.tlpSlider.Size = New System.Drawing.Size(289, 99)
    Me.tlpSlider.TabIndex = 3
    '
    'Label6
    '
    Me.Label6.AutoSize = True
    Me.Label6.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label6.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label6.Location = New System.Drawing.Point(3, 0)
    Me.Label6.Name = "Label6"
    Me.Label6.Size = New System.Drawing.Size(283, 29)
    Me.Label6.TabIndex = 3
    Me.Label6.Text = "Select Total Volume to Create:"
    Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'tbAmount
    '
    Me.tbAmount.BackColor = System.Drawing.Color.RosyBrown
    Me.tbAmount.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tbAmount.LargeChange = 8
    Me.tbAmount.Location = New System.Drawing.Point(3, 32)
    Me.tbAmount.Maximum = 34
    Me.tbAmount.Minimum = 2
    Me.tbAmount.Name = "tbAmount"
    Me.tbAmount.Size = New System.Drawing.Size(283, 23)
    Me.tbAmount.SmallChange = 8
    Me.tbAmount.TabIndex = 0
    Me.tbAmount.TickFrequency = 8
    Me.tbAmount.Value = 2
    '
    'TableLayoutPanel1
    '
    Me.TableLayoutPanel1.ColumnCount = 5
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.TableLayoutPanel1.Controls.Add(Me.Label5, 4, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.Label4, 3, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.Label3, 2, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.Label2, 1, 0)
    Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 61)
    Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
    Me.TableLayoutPanel1.RowCount = 1
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.TableLayoutPanel1.Size = New System.Drawing.Size(283, 35)
    Me.TableLayoutPanel1.TabIndex = 1
    '
    'Label5
    '
    Me.Label5.AutoSize = True
    Me.Label5.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label5.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label5.Location = New System.Drawing.Point(227, 0)
    Me.Label5.Name = "Label5"
    Me.Label5.Size = New System.Drawing.Size(53, 35)
    Me.Label5.TabIndex = 4
    Me.Label5.Text = "32 oz"
    Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'Label4
    '
    Me.Label4.AutoSize = True
    Me.Label4.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label4.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label4.Location = New System.Drawing.Point(171, 0)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(50, 35)
    Me.Label4.TabIndex = 3
    Me.Label4.Text = "24 oz"
    Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label3.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label3.Location = New System.Drawing.Point(115, 0)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(50, 35)
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
    Me.Label1.Size = New System.Drawing.Size(50, 35)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "2 oz"
    Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label2.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label2.Location = New System.Drawing.Point(59, 0)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(50, 35)
    Me.Label2.TabIndex = 1
    Me.Label2.Text = "8 oz"
    Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'btnClearAll
    '
    Me.btnClearAll.AutoScaleBorder = 0
    Me.btnClearAll.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.clear_all_button
    Me.btnClearAll.BackColor = System.Drawing.Color.BlanchedAlmond
    Me.btnClearAll.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnClearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnClearAll.Location = New System.Drawing.Point(3, 160)
    Me.btnClearAll.Name = "btnClearAll"
    Me.btnClearAll.Size = New System.Drawing.Size(289, 48)
    Me.btnClearAll.TabIndex = 1
    Me.btnClearAll.UseVisualStyleBackColor = False
    '
    'btnRemove
    '
    Me.btnRemove.AutoScaleBorder = 0
    Me.btnRemove.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.Remove_button
    Me.btnRemove.BackColor = System.Drawing.Color.BlanchedAlmond
    Me.btnRemove.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnRemove.Location = New System.Drawing.Point(3, 108)
    Me.btnRemove.Name = "btnRemove"
    Me.btnRemove.Size = New System.Drawing.Size(289, 46)
    Me.btnRemove.TabIndex = 0
    Me.btnRemove.UseVisualStyleBackColor = False
    '
    'btnCreateRecipe
    '
    Me.btnCreateRecipe.AutoScaleBorder = 0
    Me.btnCreateRecipe.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.Create_a_Flave1
    Me.btnCreateRecipe.BackColor = System.Drawing.Color.Wheat
    Me.btnCreateRecipe.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnCreateRecipe.FlatAppearance.BorderSize = 2
    Me.btnCreateRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnCreateRecipe.Location = New System.Drawing.Point(841, 3)
    Me.btnCreateRecipe.Name = "btnCreateRecipe"
    Me.btnCreateRecipe.Size = New System.Drawing.Size(414, 211)
    Me.btnCreateRecipe.TabIndex = 67
    Me.btnCreateRecipe.UseVisualStyleBackColor = False
    '
    'PictureBox1
    '
    Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.PictureBox1.Image = Global.RootBeerMixer.My.Resources.Resources.Create_Flave_Banner
    Me.PictureBox1.Location = New System.Drawing.Point(3, 3)
    Me.PictureBox1.Name = "PictureBox1"
    Me.PictureBox1.Size = New System.Drawing.Size(1258, 104)
    Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.PictureBox1.TabIndex = 96
    Me.PictureBox1.TabStop = False
    '
    'pnlRecipeStatus
    '
    Me.pnlRecipeStatus.BackColor = System.Drawing.Color.Wheat
    Me.pnlRecipeStatus.Controls.Add(Me.txtPart)
    Me.pnlRecipeStatus.Controls.Add(Me.lblam)
    Me.pnlRecipeStatus.Controls.Add(Me.lblRecipeStatus)
    Me.pnlRecipeStatus.Controls.Add(Me.pBarRecipe)
    Me.pnlRecipeStatus.Controls.Add(Me.lstIDbox)
    Me.pnlRecipeStatus.Controls.Add(Me.ListBox1)
    Me.pnlRecipeStatus.Controls.Add(Me.lstIngName)
    Me.pnlRecipeStatus.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.pnlRecipeStatus.Location = New System.Drawing.Point(0, 737)
    Me.pnlRecipeStatus.Margin = New System.Windows.Forms.Padding(2)
    Me.pnlRecipeStatus.Name = "pnlRecipeStatus"
    Me.pnlRecipeStatus.Size = New System.Drawing.Size(1264, 24)
    Me.pnlRecipeStatus.TabIndex = 94
    Me.pnlRecipeStatus.Visible = False
    '
    'txtPart
    '
    Me.txtPart.Location = New System.Drawing.Point(212, 1)
    Me.txtPart.Name = "txtPart"
    Me.txtPart.Size = New System.Drawing.Size(58, 20)
    Me.txtPart.TabIndex = 93
    '
    'lblam
    '
    Me.lblam.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblam.AutoSize = True
    Me.lblam.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblam.ForeColor = System.Drawing.Color.Black
    Me.lblam.Location = New System.Drawing.Point(0, 0)
    Me.lblam.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
    Me.lblam.Name = "lblam"
    Me.lblam.Size = New System.Drawing.Size(33, 22)
    Me.lblam.TabIndex = 78
    Me.lblam.Text = "0.0"
    Me.lblam.Visible = False
    '
    'lblRecipeStatus
    '
    Me.lblRecipeStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblRecipeStatus.AutoSize = True
    Me.lblRecipeStatus.Location = New System.Drawing.Point(993, 7)
    Me.lblRecipeStatus.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
    Me.lblRecipeStatus.Name = "lblRecipeStatus"
    Me.lblRecipeStatus.Size = New System.Drawing.Size(77, 13)
    Me.lblRecipeStatus.TabIndex = 1
    Me.lblRecipeStatus.Text = "Recipe Status:"
    Me.lblRecipeStatus.Visible = False
    '
    'pBarRecipe
    '
    Me.pBarRecipe.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pBarRecipe.ForeColor = System.Drawing.Color.LimeGreen
    Me.pBarRecipe.Location = New System.Drawing.Point(1085, 2)
    Me.pBarRecipe.Margin = New System.Windows.Forms.Padding(2)
    Me.pBarRecipe.Name = "pBarRecipe"
    Me.pBarRecipe.Size = New System.Drawing.Size(170, 19)
    Me.pBarRecipe.TabIndex = 0
    Me.pBarRecipe.Visible = False
    '
    'lstIDbox
    '
    Me.lstIDbox.FormattingEnabled = True
    Me.lstIDbox.Location = New System.Drawing.Point(153, 3)
    Me.lstIDbox.Margin = New System.Windows.Forms.Padding(2)
    Me.lstIDbox.Name = "lstIDbox"
    Me.lstIDbox.Size = New System.Drawing.Size(54, 17)
    Me.lstIDbox.TabIndex = 90
    '
    'ListBox1
    '
    Me.ListBox1.FormattingEnabled = True
    Me.ListBox1.Location = New System.Drawing.Point(95, 2)
    Me.ListBox1.Margin = New System.Windows.Forms.Padding(2)
    Me.ListBox1.Name = "ListBox1"
    Me.ListBox1.Size = New System.Drawing.Size(54, 17)
    Me.ListBox1.TabIndex = 92
    '
    'lstIngName
    '
    Me.lstIngName.FormattingEnabled = True
    Me.lstIngName.Location = New System.Drawing.Point(37, 2)
    Me.lstIngName.Margin = New System.Windows.Forms.Padding(2)
    Me.lstIngName.Name = "lstIngName"
    Me.lstIngName.Size = New System.Drawing.Size(54, 17)
    Me.lstIngName.TabIndex = 91
    '
    'tTimer2
    '
    Me.tTimer2.Interval = 500
    '
    'Timer1
    '
    Me.Timer1.Interval = 1125
    '
    'frmCreate_Flave
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(1264, 761)
    Me.Controls.Add(Me.Panel1)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Margin = New System.Windows.Forms.Padding(2)
    Me.MinimumSize = New System.Drawing.Size(772, 625)
    Me.Name = "frmCreate_Flave"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Create a Flave"
    Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
    Me.Panel1.ResumeLayout(False)
    Me.tlpMain.ResumeLayout(False)
    Me.tlpNavigation.ResumeLayout(False)
    Me.tlpMiddle.ResumeLayout(False)
    CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
    Me.tlpLeftMiddle.ResumeLayout(False)
    Me.tlpAmounts.ResumeLayout(False)
    Me.tlpBottom.ResumeLayout(False)
    Me.tlpButtons.ResumeLayout(False)
    Me.tlpSlider.ResumeLayout(False)
    Me.tlpSlider.PerformLayout()
    CType(Me.tbAmount, System.ComponentModel.ISupportInitialize).EndInit()
    Me.TableLayoutPanel1.ResumeLayout(False)
    Me.TableLayoutPanel1.PerformLayout()
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlRecipeStatus.ResumeLayout(False)
    Me.pnlRecipeStatus.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents Panel1 As System.Windows.Forms.Panel
  Friend WithEvents lstViewCreatedRecipe As System.Windows.Forms.ListView
  Friend WithEvents clmID As System.Windows.Forms.ColumnHeader
  Friend WithEvents clmIngs As System.Windows.Forms.ColumnHeader
  Friend WithEvents btnAm1 As System.Windows.Forms.Button
  Friend WithEvents btnAm6 As System.Windows.Forms.Button
  Friend WithEvents btnAm2 As System.Windows.Forms.Button
  Friend WithEvents btnAm5 As System.Windows.Forms.Button
  Friend WithEvents btnAm3 As System.Windows.Forms.Button
  Friend WithEvents btnAm4 As System.Windows.Forms.Button
  Friend WithEvents lstBoxIngs As System.Windows.Forms.ListBox
  Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
  Friend WithEvents tlpMain As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents tlpNavigation As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents btnHome As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnFavorite As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnSettings As AutoScaleButton.AutoScaleButton
  Friend WithEvents tlpMiddle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents tlpLeftMiddle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents btnFlavor As AutoScaleButton.AutoScaleButton
  Friend WithEvents tlpAmounts As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents btnAm7 As System.Windows.Forms.Button
  Friend WithEvents btnAm8 As System.Windows.Forms.Button
  Friend WithEvents btnAm9 As System.Windows.Forms.Button
  Friend WithEvents tlpBottom As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents tlpButtons As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents btnCreateRecipe As AutoScaleButton.AutoScaleButton
  Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
  Friend WithEvents btnRefPage As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnRemove As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnClearAll As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnUsers As AutoScaleButton.AutoScaleButton
  Friend WithEvents tTimer2 As System.Windows.Forms.Timer
  Friend WithEvents clmParts As System.Windows.Forms.ColumnHeader
  Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents tlpSlider As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents Label6 As System.Windows.Forms.Label
  Friend WithEvents tbAmount As System.Windows.Forms.TrackBar
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents Label4 As System.Windows.Forms.Label
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents pnlRecipeStatus As System.Windows.Forms.Panel
  Friend WithEvents lblam As System.Windows.Forms.Label
  Friend WithEvents lblRecipeStatus As System.Windows.Forms.Label
  Friend WithEvents pBarRecipe As System.Windows.Forms.ProgressBar
  Friend WithEvents lstIDbox As System.Windows.Forms.ListBox
  Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
  Friend WithEvents lstIngName As System.Windows.Forms.ListBox
  Friend WithEvents Timer1 As System.Windows.Forms.Timer
  Friend WithEvents txtPart As System.Windows.Forms.TextBox
End Class
