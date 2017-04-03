<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
    Me.btnMyFaves = New AutoScaleButton.AutoScaleButton()
    Me.btnDavesFaves = New AutoScaleButton.AutoScaleButton()
    Me.Panel1 = New System.Windows.Forms.Panel()
    Me.btnUsers = New AutoScaleButton.AutoScaleButton()
    Me.btnCreateFlave = New AutoScaleButton.AutoScaleButton()
    Me.Panel2 = New System.Windows.Forms.Panel()
    Me.Panel4 = New System.Windows.Forms.Panel()
    Me.Panel5 = New System.Windows.Forms.Panel()
    Me.btnSettings = New AutoScaleButton.AutoScaleButton()
    Me.tlpNavigation = New System.Windows.Forms.TableLayoutPanel()
    Me.PictureBox1 = New System.Windows.Forms.PictureBox()
    Me.PictureBox2 = New System.Windows.Forms.PictureBox()
    Me.PictureBox3 = New System.Windows.Forms.PictureBox()
    Me.tTimer = New System.Windows.Forms.Timer(Me.components)
    Me.Panel1.SuspendLayout()
    Me.Panel2.SuspendLayout()
    Me.Panel4.SuspendLayout()
    Me.Panel5.SuspendLayout()
    Me.tlpNavigation.SuspendLayout()
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'btnMyFaves
    '
    Me.btnMyFaves.AutoScaleBorder = 0
    Me.btnMyFaves.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.My_Faves_button
    Me.btnMyFaves.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnMyFaves.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnMyFaves.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnMyFaves.Font = New System.Drawing.Font("Poor Richard", 36.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnMyFaves.Location = New System.Drawing.Point(0, 0)
    Me.btnMyFaves.Name = "btnMyFaves"
    Me.btnMyFaves.Size = New System.Drawing.Size(272, 154)
    Me.btnMyFaves.TabIndex = 15
    Me.btnMyFaves.UseVisualStyleBackColor = False
    '
    'btnDavesFaves
    '
    Me.btnDavesFaves.AutoScaleBorder = 0
    Me.btnDavesFaves.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.Daves_Faves_button
    Me.btnDavesFaves.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnDavesFaves.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnDavesFaves.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnDavesFaves.Font = New System.Drawing.Font("Poor Richard", 36.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnDavesFaves.Location = New System.Drawing.Point(0, 0)
    Me.btnDavesFaves.Name = "btnDavesFaves"
    Me.btnDavesFaves.Size = New System.Drawing.Size(272, 154)
    Me.btnDavesFaves.TabIndex = 16
    Me.btnDavesFaves.UseVisualStyleBackColor = False
    '
    'Panel1
    '
    Me.Panel1.BackColor = System.Drawing.Color.DimGray
    Me.Panel1.Controls.Add(Me.btnUsers)
    Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Panel1.Location = New System.Drawing.Point(3, 3)
    Me.Panel1.Name = "Panel1"
    Me.Panel1.Size = New System.Drawing.Size(145, 154)
    Me.Panel1.TabIndex = 0
    '
    'btnUsers
    '
    Me.btnUsers.AutoScaleBorder = 0
    Me.btnUsers.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.user_button
    Me.btnUsers.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnUsers.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnUsers.Font = New System.Drawing.Font("Poor Richard", 36.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnUsers.Location = New System.Drawing.Point(0, 0)
    Me.btnUsers.Name = "btnUsers"
    Me.btnUsers.Size = New System.Drawing.Size(145, 154)
    Me.btnUsers.TabIndex = 18
    Me.btnUsers.UseVisualStyleBackColor = False
    '
    'btnCreateFlave
    '
    Me.btnCreateFlave.AutoScaleBorder = 0
    Me.btnCreateFlave.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.Create_Flave_Navigation
    Me.btnCreateFlave.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnCreateFlave.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnCreateFlave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnCreateFlave.Font = New System.Drawing.Font("Poor Richard", 36.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnCreateFlave.Location = New System.Drawing.Point(0, 0)
    Me.btnCreateFlave.Name = "btnCreateFlave"
    Me.btnCreateFlave.Size = New System.Drawing.Size(272, 154)
    Me.btnCreateFlave.TabIndex = 14
    Me.btnCreateFlave.UseVisualStyleBackColor = False
    '
    'Panel2
    '
    Me.Panel2.BackColor = System.Drawing.Color.DimGray
    Me.Panel2.Controls.Add(Me.btnCreateFlave)
    Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Panel2.Location = New System.Drawing.Point(154, 3)
    Me.Panel2.Name = "Panel2"
    Me.Panel2.Size = New System.Drawing.Size(272, 154)
    Me.Panel2.TabIndex = 0
    '
    'Panel4
    '
    Me.Panel4.BackColor = System.Drawing.Color.DimGray
    Me.Panel4.Controls.Add(Me.btnMyFaves)
    Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Panel4.Location = New System.Drawing.Point(432, 3)
    Me.Panel4.Name = "Panel4"
    Me.Panel4.Size = New System.Drawing.Size(272, 154)
    Me.Panel4.TabIndex = 0
    '
    'Panel5
    '
    Me.Panel5.BackColor = System.Drawing.Color.DimGray
    Me.Panel5.Controls.Add(Me.btnDavesFaves)
    Me.Panel5.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Panel5.Location = New System.Drawing.Point(710, 3)
    Me.Panel5.Name = "Panel5"
    Me.Panel5.Size = New System.Drawing.Size(272, 154)
    Me.Panel5.TabIndex = 0
    '
    'btnSettings
    '
    Me.btnSettings.AutoScaleBorder = 0
    Me.btnSettings.AutoScaleImage = Global.RootBeerMixer.My.Resources.Resources.Settings_button
    Me.btnSettings.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.btnSettings.Dock = System.Windows.Forms.DockStyle.Fill
    Me.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnSettings.Font = New System.Drawing.Font("Poor Richard", 36.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnSettings.Location = New System.Drawing.Point(988, 3)
    Me.btnSettings.Name = "btnSettings"
    Me.btnSettings.Size = New System.Drawing.Size(273, 154)
    Me.btnSettings.TabIndex = 17
    Me.btnSettings.UseVisualStyleBackColor = False
    '
    'tlpNavigation
    '
    Me.tlpNavigation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.tlpNavigation.BackColor = System.Drawing.Color.Black
    Me.tlpNavigation.ColumnCount = 5
    Me.tlpNavigation.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.0!))
    Me.tlpNavigation.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.0!))
    Me.tlpNavigation.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.0!))
    Me.tlpNavigation.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.0!))
    Me.tlpNavigation.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.0!))
    Me.tlpNavigation.Controls.Add(Me.btnSettings, 4, 0)
    Me.tlpNavigation.Controls.Add(Me.Panel5, 3, 0)
    Me.tlpNavigation.Controls.Add(Me.Panel4, 2, 0)
    Me.tlpNavigation.Controls.Add(Me.Panel2, 1, 0)
    Me.tlpNavigation.Controls.Add(Me.Panel1, 0, 0)
    Me.tlpNavigation.Location = New System.Drawing.Point(0, 144)
    Me.tlpNavigation.Name = "tlpNavigation"
    Me.tlpNavigation.RowCount = 1
    Me.tlpNavigation.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.tlpNavigation.Size = New System.Drawing.Size(1264, 160)
    Me.tlpNavigation.TabIndex = 13
    '
    'PictureBox1
    '
    Me.PictureBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
    Me.PictureBox1.Image = Global.RootBeerMixer.My.Resources.Resources.Main_Banner
    Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
    Me.PictureBox1.Name = "PictureBox1"
    Me.PictureBox1.Size = New System.Drawing.Size(1264, 144)
    Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.PictureBox1.TabIndex = 15
    Me.PictureBox1.TabStop = False
    '
    'PictureBox2
    '
    Me.PictureBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.PictureBox2.Image = Global.RootBeerMixer.My.Resources.Resources.Main_Logo
    Me.PictureBox2.Location = New System.Drawing.Point(0, 301)
    Me.PictureBox2.Name = "PictureBox2"
    Me.PictureBox2.Size = New System.Drawing.Size(1264, 460)
    Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.PictureBox2.TabIndex = 16
    Me.PictureBox2.TabStop = False
    '
    'PictureBox3
    '
    Me.PictureBox3.Dock = System.Windows.Forms.DockStyle.Fill
    Me.PictureBox3.Location = New System.Drawing.Point(0, 0)
    Me.PictureBox3.Name = "PictureBox3"
    Me.PictureBox3.Size = New System.Drawing.Size(1264, 761)
    Me.PictureBox3.TabIndex = 17
    Me.PictureBox3.TabStop = False
    '
    'frmMain
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(1264, 761)
    Me.Controls.Add(Me.tlpNavigation)
    Me.Controls.Add(Me.PictureBox1)
    Me.Controls.Add(Me.PictureBox2)
    Me.Controls.Add(Me.PictureBox3)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Margin = New System.Windows.Forms.Padding(2)
    Me.MinimumSize = New System.Drawing.Size(772, 625)
    Me.Name = "frmMain"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Flavor Flave"
    Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
    Me.Panel1.ResumeLayout(False)
    Me.Panel2.ResumeLayout(False)
    Me.Panel4.ResumeLayout(False)
    Me.Panel5.ResumeLayout(False)
    Me.tlpNavigation.ResumeLayout(False)
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents btnSettings As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnDavesFaves As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnMyFaves As AutoScaleButton.AutoScaleButton
  Friend WithEvents btnCreateFlave As AutoScaleButton.AutoScaleButton
  Friend WithEvents Panel1 As System.Windows.Forms.Panel
  Friend WithEvents Panel2 As System.Windows.Forms.Panel
  Friend WithEvents Panel4 As System.Windows.Forms.Panel
  Friend WithEvents Panel5 As System.Windows.Forms.Panel
  Friend WithEvents tlpNavigation As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
  Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
  Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
  Friend WithEvents btnUsers As AutoScaleButton.AutoScaleButton
  Friend WithEvents tTimer As System.Windows.Forms.Timer
End Class
