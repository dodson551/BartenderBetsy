<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDavesFaves
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDavesFaves))
    Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
    Me.tlpNav = New System.Windows.Forms.TableLayoutPanel()
    Me.btnHome = New AutoScaleButton.AutoScaleButton()
    Me.btnRefPage = New AutoScaleButton.AutoScaleButton()
    Me.btnSettings = New AutoScaleButton.AutoScaleButton()
    Me.AutoScaleButton1 = New AutoScaleButton.AutoScaleButton()
    Me.tlpBottom = New System.Windows.Forms.TableLayoutPanel()
    Me.tlpInfo = New System.Windows.Forms.TableLayoutPanel()
    Me.lstDavesFaves = New System.Windows.Forms.ListBox()
    Me.lblDavesFaves = New System.Windows.Forms.Label()
    Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
    Me.tlpSlider = New System.Windows.Forms.TableLayoutPanel()
    Me.Label6 = New System.Windows.Forms.Label()
    Me.tbAmount = New System.Windows.Forms.TrackBar()
    Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
    Me.Label5 = New System.Windows.Forms.Label()
    Me.Label4 = New System.Windows.Forms.Label()
    Me.Label3 = New System.Windows.Forms.Label()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.lstViewRecipe = New System.Windows.Forms.ListView()
    Me.clmID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.clmIngs = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.clmParts = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.tlpCreate = New System.Windows.Forms.TableLayoutPanel()
    Me.btnUseRecipe = New AutoScaleButton.AutoScaleButton()
    Me.lstID = New System.Windows.Forms.ListBox()
    Me.lstDFAMS = New System.Windows.Forms.ListBox()
    Me.pbHeader = New System.Windows.Forms.PictureBox()
    Me.tTimer2 = New System.Windows.Forms.Timer(Me.components)
    Me.Label7 = New System.Windows.Forms.Label()
    Me.tlpMain.SuspendLayout()
    Me.tlpNav.SuspendLayout()
    Me.tlpBottom.SuspendLayout()
    Me.tlpInfo.SuspendLayout()
    Me.TableLayoutPanel1.SuspendLayout()
    Me.tlpSlider.SuspendLayout()
    CType(Me.tbAmount, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.TableLayoutPanel2.SuspendLayout()
    Me.tlpCreate.SuspendLayout()
    CType(Me.pbHeader, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'tlpMain
    '
    Me.tlpMain.BackColor = System.Drawing.Color.Tan
    Me.tlpMain.ColumnCount = 1
    Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpMain.Controls.Add(Me.tlpNav, 0, 1)
    Me.tlpMain.Controls.Add(Me.tlpBottom, 0, 2)
    Me.tlpMain.Controls.Add(Me.pbHeader, 0, 0)
    Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpMain.Location = New System.Drawing.Point(0, 0)
    Me.tlpMain.Name = "tlpMain"
    Me.tlpMain.RowCount = 3
    Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
    Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
    Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.0!))
    Me.tlpMain.Size = New System.Drawing.Size(1264, 761)
    Me.tlpMain.TabIndex = 0
    '
    'tlpNav
    '
    Me.tlpNav.ColumnCount = 4
    Me.tlpNav.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.0!))
    Me.tlpNav.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.0!))
    Me.tlpNav.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.0!))
    Me.tlpNav.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.0!))
    Me.tlpNav.Controls.Add(Me.btnHome, 0, 0)
    Me.tlpNav.Controls.Add(Me.btnRefPage, 3, 0)
    Me.tlpNav.Controls.Add(Me.btnSettings, 2, 0)
    Me.tlpNav.Controls.Add(Me.AutoScaleButton1, 1, 0)
    Me.tlpNav.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpNav.Location = New System.Drawing.Point(3, 117)
    Me.tlpNav.Name = "tlpNav"
    Me.tlpNav.RowCount = 1
    Me.tlpNav.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpNav.Size = New System.Drawing.Size(1258, 70)
    Me.tlpNav.TabIndex = 0
    '
    'btnHome
    '
    Me.btnHome.AutoScaleBorder = 0
    Me.btnHome.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.Home_button
    Me.btnHome.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnHome.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnHome.FlatAppearance.BorderSize = 2
    Me.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnHome.Location = New System.Drawing.Point(3, 3)
    Me.btnHome.Name = "btnHome"
    Me.btnHome.Size = New System.Drawing.Size(383, 64)
    Me.btnHome.TabIndex = 0
    Me.btnHome.UseVisualStyleBackColor = False
    '
    'btnRefPage
    '
    Me.btnRefPage.AutoScaleBorder = 0
    Me.btnRefPage.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.refresh_page_button
    Me.btnRefPage.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnRefPage.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnRefPage.FlatAppearance.BorderSize = 2
    Me.btnRefPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnRefPage.Location = New System.Drawing.Point(1170, 3)
    Me.btnRefPage.Name = "btnRefPage"
    Me.btnRefPage.Size = New System.Drawing.Size(85, 64)
    Me.btnRefPage.TabIndex = 2
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
    Me.btnSettings.Location = New System.Drawing.Point(781, 3)
    Me.btnSettings.Name = "btnSettings"
    Me.btnSettings.Size = New System.Drawing.Size(383, 64)
    Me.btnSettings.TabIndex = 1
    Me.btnSettings.UseVisualStyleBackColor = False
    '
    'AutoScaleButton1
    '
    Me.AutoScaleButton1.AutoScaleBorder = 0
    Me.AutoScaleButton1.AutoScaleImage = Nothing
    Me.AutoScaleButton1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.AutoScaleButton1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.AutoScaleButton1.FlatAppearance.BorderSize = 2
    Me.AutoScaleButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.AutoScaleButton1.Location = New System.Drawing.Point(392, 3)
    Me.AutoScaleButton1.Name = "AutoScaleButton1"
    Me.AutoScaleButton1.Size = New System.Drawing.Size(383, 64)
    Me.AutoScaleButton1.TabIndex = 3
    Me.AutoScaleButton1.UseVisualStyleBackColor = False
    '
    'tlpBottom
    '
    Me.tlpBottom.ColumnCount = 1
    Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpBottom.Controls.Add(Me.tlpInfo, 0, 0)
    Me.tlpBottom.Controls.Add(Me.tlpCreate, 0, 1)
    Me.tlpBottom.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpBottom.Location = New System.Drawing.Point(3, 193)
    Me.tlpBottom.Name = "tlpBottom"
    Me.tlpBottom.RowCount = 2
    Me.tlpBottom.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.0!))
    Me.tlpBottom.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.tlpBottom.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.tlpBottom.Size = New System.Drawing.Size(1258, 565)
    Me.tlpBottom.TabIndex = 1
    '
    'tlpInfo
    '
    Me.tlpInfo.ColumnCount = 2
    Me.tlpInfo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
    Me.tlpInfo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
    Me.tlpInfo.Controls.Add(Me.lstDavesFaves, 0, 1)
    Me.tlpInfo.Controls.Add(Me.lblDavesFaves, 0, 0)
    Me.tlpInfo.Controls.Add(Me.TableLayoutPanel1, 1, 1)
    Me.tlpInfo.Controls.Add(Me.Label7, 1, 0)
    Me.tlpInfo.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpInfo.Location = New System.Drawing.Point(3, 3)
    Me.tlpInfo.Name = "tlpInfo"
    Me.tlpInfo.RowCount = 2
    Me.tlpInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
    Me.tlpInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.0!))
    Me.tlpInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.tlpInfo.Size = New System.Drawing.Size(1252, 446)
    Me.tlpInfo.TabIndex = 0
    '
    'lstDavesFaves
    '
    Me.lstDavesFaves.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.lstDavesFaves.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lstDavesFaves.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
    Me.lstDavesFaves.Font = New System.Drawing.Font("Poor Richard", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lstDavesFaves.FormattingEnabled = True
    Me.lstDavesFaves.ItemHeight = 41
    Me.lstDavesFaves.Location = New System.Drawing.Point(3, 47)
    Me.lstDavesFaves.Name = "lstDavesFaves"
    Me.lstDavesFaves.Size = New System.Drawing.Size(745, 396)
    Me.lstDavesFaves.TabIndex = 0
    '
    'lblDavesFaves
    '
    Me.lblDavesFaves.AutoSize = True
    Me.lblDavesFaves.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lblDavesFaves.Font = New System.Drawing.Font("Poor Richard", 26.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDavesFaves.Location = New System.Drawing.Point(3, 0)
    Me.lblDavesFaves.Name = "lblDavesFaves"
    Me.lblDavesFaves.Size = New System.Drawing.Size(745, 44)
    Me.lblDavesFaves.TabIndex = 3
    Me.lblDavesFaves.Text = "Dave's Faves:"
    '
    'TableLayoutPanel1
    '
    Me.TableLayoutPanel1.ColumnCount = 1
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.TableLayoutPanel1.Controls.Add(Me.tlpSlider, 0, 1)
    Me.TableLayoutPanel1.Controls.Add(Me.lstViewRecipe, 0, 0)
    Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.TableLayoutPanel1.Location = New System.Drawing.Point(754, 47)
    Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
    Me.TableLayoutPanel1.RowCount = 2
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.0!))
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
    Me.TableLayoutPanel1.Size = New System.Drawing.Size(495, 396)
    Me.TableLayoutPanel1.TabIndex = 4
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
    Me.tlpSlider.Location = New System.Drawing.Point(3, 280)
    Me.tlpSlider.Name = "tlpSlider"
    Me.tlpSlider.RowCount = 3
    Me.tlpSlider.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
    Me.tlpSlider.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
    Me.tlpSlider.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
    Me.tlpSlider.Size = New System.Drawing.Size(489, 113)
    Me.tlpSlider.TabIndex = 21
    '
    'Label6
    '
    Me.Label6.AutoSize = True
    Me.Label6.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label6.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label6.Location = New System.Drawing.Point(3, 0)
    Me.Label6.Name = "Label6"
    Me.Label6.Size = New System.Drawing.Size(483, 33)
    Me.Label6.TabIndex = 3
    Me.Label6.Text = "Select Total Volume to Create:"
    Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'tbAmount
    '
    Me.tbAmount.BackColor = System.Drawing.Color.RosyBrown
    Me.tbAmount.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tbAmount.LargeChange = 8
    Me.tbAmount.Location = New System.Drawing.Point(3, 36)
    Me.tbAmount.Maximum = 34
    Me.tbAmount.Minimum = 2
    Me.tbAmount.Name = "tbAmount"
    Me.tbAmount.Size = New System.Drawing.Size(483, 27)
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
    Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 69)
    Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
    Me.TableLayoutPanel2.RowCount = 1
    Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.TableLayoutPanel2.Size = New System.Drawing.Size(483, 41)
    Me.TableLayoutPanel2.TabIndex = 1
    '
    'Label5
    '
    Me.Label5.AutoSize = True
    Me.Label5.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label5.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label5.Location = New System.Drawing.Point(387, 0)
    Me.Label5.Name = "Label5"
    Me.Label5.Size = New System.Drawing.Size(93, 41)
    Me.Label5.TabIndex = 4
    Me.Label5.Text = "32 oz"
    Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'Label4
    '
    Me.Label4.AutoSize = True
    Me.Label4.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label4.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label4.Location = New System.Drawing.Point(291, 0)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(90, 41)
    Me.Label4.TabIndex = 3
    Me.Label4.Text = "24 oz"
    Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label3.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label3.Location = New System.Drawing.Point(195, 0)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(90, 41)
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
    Me.Label1.Size = New System.Drawing.Size(90, 41)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "2 oz"
    Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label2.Font = New System.Drawing.Font("Poor Richard", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label2.Location = New System.Drawing.Point(99, 0)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(90, 41)
    Me.Label2.TabIndex = 1
    Me.Label2.Text = "8 oz"
    Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'lstViewRecipe
    '
    Me.lstViewRecipe.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clmID, Me.clmIngs, Me.clmParts})
    Me.lstViewRecipe.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lstViewRecipe.Font = New System.Drawing.Font("Poor Richard", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lstViewRecipe.Location = New System.Drawing.Point(3, 3)
    Me.lstViewRecipe.Name = "lstViewRecipe"
    Me.lstViewRecipe.Size = New System.Drawing.Size(489, 271)
    Me.lstViewRecipe.TabIndex = 22
    Me.lstViewRecipe.UseCompatibleStateImageBehavior = False
    Me.lstViewRecipe.View = System.Windows.Forms.View.Details
    '
    'clmID
    '
    Me.clmID.Text = "ID"
    Me.clmID.Width = 0
    '
    'clmIngs
    '
    Me.clmIngs.Text = "Ingredients"
    Me.clmIngs.Width = 325
    '
    'clmParts
    '
    Me.clmParts.Text = "Parts"
    Me.clmParts.Width = 125
    '
    'tlpCreate
    '
    Me.tlpCreate.ColumnCount = 3
    Me.tlpCreate.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
    Me.tlpCreate.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.0!))
    Me.tlpCreate.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
    Me.tlpCreate.Controls.Add(Me.btnUseRecipe, 1, 0)
    Me.tlpCreate.Controls.Add(Me.lstID, 0, 0)
    Me.tlpCreate.Controls.Add(Me.lstDFAMS, 2, 0)
    Me.tlpCreate.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tlpCreate.Location = New System.Drawing.Point(3, 455)
    Me.tlpCreate.Name = "tlpCreate"
    Me.tlpCreate.RowCount = 1
    Me.tlpCreate.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpCreate.Size = New System.Drawing.Size(1252, 107)
    Me.tlpCreate.TabIndex = 1
    '
    'btnUseRecipe
    '
    Me.btnUseRecipe.AutoScaleBorder = 0
    Me.btnUseRecipe.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.create_flave_button
    Me.btnUseRecipe.BackColor = System.Drawing.Color.NavajoWhite
    Me.btnUseRecipe.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnUseRecipe.FlatAppearance.BorderSize = 2
    Me.btnUseRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnUseRecipe.Location = New System.Drawing.Point(128, 3)
    Me.btnUseRecipe.Name = "btnUseRecipe"
    Me.btnUseRecipe.Size = New System.Drawing.Size(995, 101)
    Me.btnUseRecipe.TabIndex = 0
    Me.btnUseRecipe.UseVisualStyleBackColor = False
    '
    'lstID
    '
    Me.lstID.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lstID.FormattingEnabled = True
    Me.lstID.Location = New System.Drawing.Point(3, 3)
    Me.lstID.Name = "lstID"
    Me.lstID.Size = New System.Drawing.Size(119, 101)
    Me.lstID.TabIndex = 1
    Me.lstID.Visible = False
    '
    'lstDFAMS
    '
    Me.lstDFAMS.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lstDFAMS.FormattingEnabled = True
    Me.lstDFAMS.Location = New System.Drawing.Point(1129, 3)
    Me.lstDFAMS.Name = "lstDFAMS"
    Me.lstDFAMS.Size = New System.Drawing.Size(120, 101)
    Me.lstDFAMS.TabIndex = 2
    Me.lstDFAMS.Visible = False
    '
    'pbHeader
    '
    Me.pbHeader.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pbHeader.Image = Global.RootBeerMixer.My.Resources.Resources.Daves_Faves_banner
    Me.pbHeader.Location = New System.Drawing.Point(3, 3)
    Me.pbHeader.Name = "pbHeader"
    Me.pbHeader.Size = New System.Drawing.Size(1258, 108)
    Me.pbHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pbHeader.TabIndex = 2
    Me.pbHeader.TabStop = False
    '
    'tTimer2
    '
    Me.tTimer2.Interval = 500
    '
    'Label7
    '
    Me.Label7.AutoSize = True
    Me.Label7.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Label7.Font = New System.Drawing.Font("Poor Richard", 26.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label7.Location = New System.Drawing.Point(754, 0)
    Me.Label7.Name = "Label7"
    Me.Label7.Size = New System.Drawing.Size(495, 44)
    Me.Label7.TabIndex = 5
    Me.Label7.Text = "Recipe Contents:"
    '
    'frmDavesFaves
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(1264, 761)
    Me.Controls.Add(Me.tlpMain)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Margin = New System.Windows.Forms.Padding(2)
    Me.Name = "frmDavesFaves"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Dave's Faves"
    Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
    Me.tlpMain.ResumeLayout(False)
    Me.tlpNav.ResumeLayout(False)
    Me.tlpBottom.ResumeLayout(False)
    Me.tlpInfo.ResumeLayout(False)
    Me.tlpInfo.PerformLayout()
    Me.TableLayoutPanel1.ResumeLayout(False)
    Me.tlpSlider.ResumeLayout(False)
    Me.tlpSlider.PerformLayout()
    CType(Me.tbAmount, System.ComponentModel.ISupportInitialize).EndInit()
    Me.TableLayoutPanel2.ResumeLayout(False)
    Me.TableLayoutPanel2.PerformLayout()
    Me.tlpCreate.ResumeLayout(False)
    CType(Me.pbHeader, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents tlpMain As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents tlpNav As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents btnHome As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnRefPage As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnSettings As AutoScaleButton.AutoScaleButton
  Friend WithEvents AutoScaleButton1 As AutoScaleButton.AutoScaleButton
  Friend WithEvents tlpBottom As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents tlpInfo As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lstDavesFaves As System.Windows.Forms.ListBox
  Friend WithEvents tlpCreate As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblDavesFaves As System.Windows.Forms.Label
  Friend WithEvents btnUseRecipe As AutoScaleButton.AutoScaleButton
  Friend WithEvents lstID As System.Windows.Forms.ListBox
  Friend WithEvents lstDFAMS As System.Windows.Forms.ListBox
  Friend WithEvents pbHeader As System.Windows.Forms.PictureBox
  Friend WithEvents tTimer2 As System.Windows.Forms.Timer
  Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents tlpSlider As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents Label6 As System.Windows.Forms.Label
  Friend WithEvents tbAmount As System.Windows.Forms.TrackBar
  Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents Label4 As System.Windows.Forms.Label
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents lstViewRecipe As System.Windows.Forms.ListView
  Friend WithEvents clmID As System.Windows.Forms.ColumnHeader
  Friend WithEvents clmIngs As System.Windows.Forms.ColumnHeader
  Friend WithEvents clmParts As System.Windows.Forms.ColumnHeader
  Friend WithEvents Label7 As System.Windows.Forms.Label
End Class
