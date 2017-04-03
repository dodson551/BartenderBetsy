<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWelcome
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWelcome))
    Me.pnl1 = New System.Windows.Forms.Panel()
    Me.lblTitle = New System.Windows.Forms.Label()
    Me.Panel2 = New System.Windows.Forms.Panel()
    Me.PictureBox1 = New System.Windows.Forms.PictureBox()
    Me.PictureBox3 = New System.Windows.Forms.PictureBox()
    Me.Panel1 = New System.Windows.Forms.Panel()
    Me.btnCreateDB = New System.Windows.Forms.Button()
    Me.btnChooseNewDB = New System.Windows.Forms.Button()
    Me.btnSelectDB = New System.Windows.Forms.Button()
    Me.txtDatabaseFile = New System.Windows.Forms.TextBox()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.OpenFileDialogChooseDB = New System.Windows.Forms.OpenFileDialog()
    Me.SaveFileDialogSaveDB = New System.Windows.Forms.SaveFileDialog()
    Me.pnl1.SuspendLayout()
    Me.Panel2.SuspendLayout()
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.Panel1.SuspendLayout()
    Me.SuspendLayout()
    '
    'pnl1
    '
    Me.pnl1.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
    Me.pnl1.Controls.Add(Me.lblTitle)
    Me.pnl1.Dock = System.Windows.Forms.DockStyle.Top
    Me.pnl1.Location = New System.Drawing.Point(0, 0)
    Me.pnl1.Name = "pnl1"
    Me.pnl1.Size = New System.Drawing.Size(782, 85)
    Me.pnl1.TabIndex = 1
    '
    'lblTitle
    '
    Me.lblTitle.AutoSize = True
    Me.lblTitle.Font = New System.Drawing.Font("Poor Richard", 36.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblTitle.ForeColor = System.Drawing.Color.AntiqueWhite
    Me.lblTitle.Location = New System.Drawing.Point(51, 9)
    Me.lblTitle.Name = "lblTitle"
    Me.lblTitle.Size = New System.Drawing.Size(680, 68)
    Me.lblTitle.TabIndex = 0
    Me.lblTitle.Text = "Root Beer Baby Flavor Mixer"
    '
    'Panel2
    '
    Me.Panel2.BackColor = System.Drawing.Color.Coral
    Me.Panel2.Controls.Add(Me.PictureBox1)
    Me.Panel2.Controls.Add(Me.PictureBox3)
    Me.Panel2.Dock = System.Windows.Forms.DockStyle.Left
    Me.Panel2.Location = New System.Drawing.Point(0, 85)
    Me.Panel2.Name = "Panel2"
    Me.Panel2.Size = New System.Drawing.Size(236, 508)
    Me.Panel2.TabIndex = 5
    '
    'PictureBox1
    '
    Me.PictureBox1.Image = Global.RootBeerMixer.My.Resources.Resources.RootBeerFloat
    Me.PictureBox1.Location = New System.Drawing.Point(-126, 282)
    Me.PictureBox1.Name = "PictureBox1"
    Me.PictureBox1.Size = New System.Drawing.Size(495, 431)
    Me.PictureBox1.TabIndex = 4
    Me.PictureBox1.TabStop = False
    '
    'PictureBox3
    '
    Me.PictureBox3.Image = Global.RootBeerMixer.My.Resources.Resources.markbeerbaby
    Me.PictureBox3.Location = New System.Drawing.Point(-12, 0)
    Me.PictureBox3.Name = "PictureBox3"
    Me.PictureBox3.Size = New System.Drawing.Size(248, 276)
    Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.PictureBox3.TabIndex = 7
    Me.PictureBox3.TabStop = False
    '
    'Panel1
    '
    Me.Panel1.BackColor = System.Drawing.Color.Wheat
    Me.Panel1.Controls.Add(Me.btnCreateDB)
    Me.Panel1.Controls.Add(Me.btnChooseNewDB)
    Me.Panel1.Controls.Add(Me.btnSelectDB)
    Me.Panel1.Controls.Add(Me.txtDatabaseFile)
    Me.Panel1.Controls.Add(Me.Label1)
    Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Panel1.Location = New System.Drawing.Point(236, 85)
    Me.Panel1.Name = "Panel1"
    Me.Panel1.Size = New System.Drawing.Size(546, 508)
    Me.Panel1.TabIndex = 6
    '
    'btnCreateDB
    '
    Me.btnCreateDB.BackColor = System.Drawing.Color.AntiqueWhite
    Me.btnCreateDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnCreateDB.Font = New System.Drawing.Font("Poor Richard", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnCreateDB.Location = New System.Drawing.Point(39, 408)
    Me.btnCreateDB.Name = "btnCreateDB"
    Me.btnCreateDB.Size = New System.Drawing.Size(470, 78)
    Me.btnCreateDB.TabIndex = 4
    Me.btnCreateDB.Text = "Create New Database "
    Me.btnCreateDB.UseVisualStyleBackColor = False
    '
    'btnChooseNewDB
    '
    Me.btnChooseNewDB.BackColor = System.Drawing.Color.AntiqueWhite
    Me.btnChooseNewDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnChooseNewDB.Font = New System.Drawing.Font("Poor Richard", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnChooseNewDB.Location = New System.Drawing.Point(282, 324)
    Me.btnChooseNewDB.Name = "btnChooseNewDB"
    Me.btnChooseNewDB.Size = New System.Drawing.Size(227, 78)
    Me.btnChooseNewDB.TabIndex = 3
    Me.btnChooseNewDB.Text = "Choose Database" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "from file"
    Me.btnChooseNewDB.UseVisualStyleBackColor = False
    '
    'btnSelectDB
    '
    Me.btnSelectDB.BackColor = System.Drawing.Color.AntiqueWhite
    Me.btnSelectDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnSelectDB.Font = New System.Drawing.Font("Poor Richard", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnSelectDB.Location = New System.Drawing.Point(39, 324)
    Me.btnSelectDB.Name = "btnSelectDB"
    Me.btnSelectDB.Size = New System.Drawing.Size(227, 78)
    Me.btnSelectDB.TabIndex = 2
    Me.btnSelectDB.Text = "Use Selected" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Database"
    Me.btnSelectDB.UseVisualStyleBackColor = False
    '
    'txtDatabaseFile
    '
    Me.txtDatabaseFile.Location = New System.Drawing.Point(39, 282)
    Me.txtDatabaseFile.Name = "txtDatabaseFile"
    Me.txtDatabaseFile.Size = New System.Drawing.Size(470, 22)
    Me.txtDatabaseFile.TabIndex = 1
    Me.txtDatabaseFile.Text = "C:\ProgramData\Telaeris\Root Beer Baby\DavesFlaves.db"
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Font = New System.Drawing.Font("Poor Richard", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label1.Location = New System.Drawing.Point(35, 26)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(474, 230)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = resources.GetString("Label1.Text")
    '
    'frmWelcome
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(782, 593)
    Me.Controls.Add(Me.Panel1)
    Me.Controls.Add(Me.Panel2)
    Me.Controls.Add(Me.pnl1)
    Me.Name = "frmWelcome"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Welcome"
    Me.pnl1.ResumeLayout(False)
    Me.pnl1.PerformLayout()
    Me.Panel2.ResumeLayout(False)
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
    Me.Panel1.ResumeLayout(False)
    Me.Panel1.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents pnl1 As System.Windows.Forms.Panel
  Friend WithEvents lblTitle As System.Windows.Forms.Label
  Friend WithEvents Panel2 As System.Windows.Forms.Panel
  Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
  Friend WithEvents Panel1 As System.Windows.Forms.Panel
  Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents btnSelectDB As System.Windows.Forms.Button
  Friend WithEvents txtDatabaseFile As System.Windows.Forms.TextBox
  Friend WithEvents btnChooseNewDB As System.Windows.Forms.Button
  Friend WithEvents OpenFileDialogChooseDB As System.Windows.Forms.OpenFileDialog
  Friend WithEvents btnCreateDB As System.Windows.Forms.Button
  Friend WithEvents SaveFileDialogSaveDB As System.Windows.Forms.SaveFileDialog
End Class
