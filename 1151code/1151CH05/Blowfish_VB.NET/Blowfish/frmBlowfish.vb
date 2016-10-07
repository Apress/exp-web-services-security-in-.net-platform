Option Strict Off
Option Explicit On
Friend Class frmBLOWFISH
    Inherits System.Windows.Forms.Form
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents txtCipher64 As System.Windows.Forms.TextBox
    Public WithEvents optAlphaKey As System.Windows.Forms.RadioButton
    Public WithEvents optHexKey As System.Windows.Forms.RadioButton
    Public WithEvents grpKeyForm As System.Windows.Forms.GroupBox
    Public WithEvents txtCipherHex As System.Windows.Forms.TextBox
    Public WithEvents txtCipher As System.Windows.Forms.TextBox
    Public WithEvents txtDecrypt As System.Windows.Forms.TextBox
    Public WithEvents cmdDecrypt As System.Windows.Forms.Button
    Public WithEvents txtKeyAsString As System.Windows.Forms.TextBox
    Public WithEvents cmdSetKey As System.Windows.Forms.Button
    Public WithEvents cmdEncrypt As System.Windows.Forms.Button
    Public WithEvents txtPlain As System.Windows.Forms.TextBox
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents _Label2_1 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents _Label2_0 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label2 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Public WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnBrowse2 As System.Windows.Forms.Button
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtSourceFile As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse1 As System.Windows.Forms.Button
    Public WithEvents txtPassword As System.Windows.Forms.TextBox
    Public WithEvents txtPassword2 As System.Windows.Forms.TextBox
    Public WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkENCFiles As System.Windows.Forms.CheckBox
    Friend WithEvents chkENCFilesSubFolders As System.Windows.Forms.CheckBox
    Friend WithEvents chkENCOverlay As System.Windows.Forms.CheckBox
    Friend WithEvents btnAction As System.Windows.Forms.Button
    Friend WithEvents grpBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rdoEncrypt As System.Windows.Forms.RadioButton
    Friend WithEvents rdoDecrypt As System.Windows.Forms.RadioButton
    Friend WithEvents txtOutputFile As System.Windows.Forms.TextBox
    Friend WithEvents txtStatus As System.Windows.Forms.TextBox
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents lblShowPasswords As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
Me.components = New System.ComponentModel.Container()
Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmBLOWFISH))
Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
Me.txtCipher64 = New System.Windows.Forms.TextBox()
Me.grpKeyForm = New System.Windows.Forms.GroupBox()
Me.optAlphaKey = New System.Windows.Forms.RadioButton()
Me.optHexKey = New System.Windows.Forms.RadioButton()
Me.txtCipherHex = New System.Windows.Forms.TextBox()
Me.txtCipher = New System.Windows.Forms.TextBox()
Me.txtDecrypt = New System.Windows.Forms.TextBox()
Me.cmdDecrypt = New System.Windows.Forms.Button()
Me.txtKeyAsString = New System.Windows.Forms.TextBox()
Me.cmdSetKey = New System.Windows.Forms.Button()
Me.cmdEncrypt = New System.Windows.Forms.Button()
Me.txtPassword = New System.Windows.Forms.TextBox()
Me.txtPlain = New System.Windows.Forms.TextBox()
Me.Label7 = New System.Windows.Forms.Label()
Me._Label2_1 = New System.Windows.Forms.Label()
Me.Label5 = New System.Windows.Forms.Label()
Me.Label4 = New System.Windows.Forms.Label()
Me.Label33 = New System.Windows.Forms.Label()
Me._Label2_0 = New System.Windows.Forms.Label()
Me.Label1 = New System.Windows.Forms.Label()
Me.Label2 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
Me.txtPassword2 = New System.Windows.Forms.TextBox()
Me.Label6 = New System.Windows.Forms.Label()
Me.Label11 = New System.Windows.Forms.Label()
Me.txtOutputFile = New System.Windows.Forms.TextBox()
Me.btnBrowse2 = New System.Windows.Forms.Button()
Me.btnAction = New System.Windows.Forms.Button()
Me.Label3 = New System.Windows.Forms.Label()
Me.Label13 = New System.Windows.Forms.Label()
Me.txtSourceFile = New System.Windows.Forms.TextBox()
Me.btnBrowse1 = New System.Windows.Forms.Button()
Me.chkENCFiles = New System.Windows.Forms.CheckBox()
Me.chkENCFilesSubFolders = New System.Windows.Forms.CheckBox()
Me.chkENCOverlay = New System.Windows.Forms.CheckBox()
Me.grpBox1 = New System.Windows.Forms.GroupBox()
Me.rdoDecrypt = New System.Windows.Forms.RadioButton()
Me.rdoEncrypt = New System.Windows.Forms.RadioButton()
Me.txtStatus = New System.Windows.Forms.TextBox()
Me.btnClear = New System.Windows.Forms.Button()
Me.lblShowPasswords = New System.Windows.Forms.Label()
Me.grpKeyForm.SuspendLayout()
CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
Me.grpBox1.SuspendLayout()
Me.SuspendLayout()
'
'txtCipher64
'
Me.txtCipher64.AcceptsReturn = True
Me.txtCipher64.AutoSize = False
Me.txtCipher64.BackColor = System.Drawing.SystemColors.Menu
Me.txtCipher64.Cursor = System.Windows.Forms.Cursors.IBeam
Me.txtCipher64.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.txtCipher64.ForeColor = System.Drawing.SystemColors.WindowText
Me.txtCipher64.Location = New System.Drawing.Point(464, 192)
Me.txtCipher64.MaxLength = 0
Me.txtCipher64.Multiline = True
Me.txtCipher64.Name = "txtCipher64"
Me.txtCipher64.ReadOnly = True
Me.txtCipher64.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.txtCipher64.Size = New System.Drawing.Size(256, 25)
Me.txtCipher64.TabIndex = 19
Me.txtCipher64.Text = ""
'
'grpKeyForm
'
Me.grpKeyForm.BackColor = System.Drawing.SystemColors.Control
Me.grpKeyForm.Controls.AddRange(New System.Windows.Forms.Control() {Me.optAlphaKey, Me.optHexKey})
Me.grpKeyForm.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.grpKeyForm.ForeColor = System.Drawing.SystemColors.ControlText
Me.grpKeyForm.Location = New System.Drawing.Point(400, 9)
Me.grpKeyForm.Name = "grpKeyForm"
Me.grpKeyForm.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.grpKeyForm.Size = New System.Drawing.Size(257, 41)
Me.grpKeyForm.TabIndex = 2
Me.grpKeyForm.TabStop = False
Me.grpKeyForm.Text = "Key form:"
'
'optAlphaKey
'
Me.optAlphaKey.BackColor = System.Drawing.SystemColors.Control
Me.optAlphaKey.Checked = True
Me.optAlphaKey.Cursor = System.Windows.Forms.Cursors.Default
Me.optAlphaKey.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.optAlphaKey.ForeColor = System.Drawing.SystemColors.ControlText
Me.optAlphaKey.Location = New System.Drawing.Point(136, 16)
Me.optAlphaKey.Name = "optAlphaKey"
Me.optAlphaKey.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.optAlphaKey.Size = New System.Drawing.Size(89, 17)
Me.optAlphaKey.TabIndex = 18
Me.optAlphaKey.TabStop = True
Me.optAlphaKey.Text = "Alpha"
'
'optHexKey
'
Me.optHexKey.BackColor = System.Drawing.SystemColors.Control
Me.optHexKey.Cursor = System.Windows.Forms.Cursors.Default
Me.optHexKey.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.optHexKey.ForeColor = System.Drawing.SystemColors.ControlText
Me.optHexKey.Location = New System.Drawing.Point(32, 16)
Me.optHexKey.Name = "optHexKey"
Me.optHexKey.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.optHexKey.Size = New System.Drawing.Size(81, 17)
Me.optHexKey.TabIndex = 17
Me.optHexKey.Text = "Hex string"
'
'txtCipherHex
'
Me.txtCipherHex.AcceptsReturn = True
Me.txtCipherHex.AutoSize = False
Me.txtCipherHex.BackColor = System.Drawing.SystemColors.Menu
Me.txtCipherHex.Cursor = System.Windows.Forms.Cursors.IBeam
Me.txtCipherHex.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.txtCipherHex.ForeColor = System.Drawing.SystemColors.WindowText
Me.txtCipherHex.Location = New System.Drawing.Point(464, 160)
Me.txtCipherHex.MaxLength = 0
Me.txtCipherHex.Multiline = True
Me.txtCipherHex.Name = "txtCipherHex"
Me.txtCipherHex.ReadOnly = True
Me.txtCipherHex.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.txtCipherHex.Size = New System.Drawing.Size(256, 25)
Me.txtCipherHex.TabIndex = 15
Me.txtCipherHex.Text = ""
'
'txtCipher
'
Me.txtCipher.AcceptsReturn = True
Me.txtCipher.AutoSize = False
Me.txtCipher.BackColor = System.Drawing.SystemColors.Menu
Me.txtCipher.Cursor = System.Windows.Forms.Cursors.IBeam
Me.txtCipher.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.txtCipher.ForeColor = System.Drawing.SystemColors.WindowText
Me.txtCipher.Location = New System.Drawing.Point(464, 96)
Me.txtCipher.MaxLength = 0
Me.txtCipher.Multiline = True
Me.txtCipher.Name = "txtCipher"
Me.txtCipher.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.txtCipher.Size = New System.Drawing.Size(256, 56)
Me.txtCipher.TabIndex = 14
Me.txtCipher.Text = ""
'
'txtDecrypt
'
Me.txtDecrypt.AcceptsReturn = True
Me.txtDecrypt.AutoSize = False
Me.txtDecrypt.BackColor = System.Drawing.SystemColors.Menu
Me.txtDecrypt.Cursor = System.Windows.Forms.Cursors.IBeam
Me.txtDecrypt.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.txtDecrypt.ForeColor = System.Drawing.SystemColors.WindowText
Me.txtDecrypt.Location = New System.Drawing.Point(80, 160)
Me.txtDecrypt.MaxLength = 0
Me.txtDecrypt.Multiline = True
Me.txtDecrypt.Name = "txtDecrypt"
Me.txtDecrypt.ReadOnly = True
Me.txtDecrypt.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.txtDecrypt.Size = New System.Drawing.Size(256, 56)
Me.txtDecrypt.TabIndex = 10
Me.txtDecrypt.Text = ""
'
'cmdDecrypt
'
Me.cmdDecrypt.BackColor = System.Drawing.SystemColors.Control
Me.cmdDecrypt.Cursor = System.Windows.Forms.Cursors.Default
Me.cmdDecrypt.Enabled = False
Me.cmdDecrypt.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.cmdDecrypt.ForeColor = System.Drawing.SystemColors.ControlText
Me.cmdDecrypt.Location = New System.Drawing.Point(7, 182)
Me.cmdDecrypt.Name = "cmdDecrypt"
Me.cmdDecrypt.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.cmdDecrypt.Size = New System.Drawing.Size(68, 33)
Me.cmdDecrypt.TabIndex = 6
Me.cmdDecrypt.Text = "&Decrypt It"
'
'txtKeyAsString
'
Me.txtKeyAsString.AcceptsReturn = True
Me.txtKeyAsString.AutoSize = False
Me.txtKeyAsString.BackColor = System.Drawing.SystemColors.Menu
Me.txtKeyAsString.Cursor = System.Windows.Forms.Cursors.IBeam
Me.txtKeyAsString.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.txtKeyAsString.ForeColor = System.Drawing.SystemColors.WindowText
Me.txtKeyAsString.Location = New System.Drawing.Point(464, 64)
Me.txtKeyAsString.MaxLength = 0
Me.txtKeyAsString.Name = "txtKeyAsString"
Me.txtKeyAsString.ReadOnly = True
Me.txtKeyAsString.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.txtKeyAsString.Size = New System.Drawing.Size(313, 25)
Me.txtKeyAsString.TabIndex = 7
Me.txtKeyAsString.Text = ""
'
'cmdSetKey
'
Me.cmdSetKey.BackColor = System.Drawing.SystemColors.Control
Me.cmdSetKey.Cursor = System.Windows.Forms.Cursors.Default
Me.cmdSetKey.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.cmdSetKey.ForeColor = System.Drawing.SystemColors.ControlText
Me.cmdSetKey.Location = New System.Drawing.Point(672, 16)
Me.cmdSetKey.Name = "cmdSetKey"
Me.cmdSetKey.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.cmdSetKey.Size = New System.Drawing.Size(121, 33)
Me.cmdSetKey.TabIndex = 3
Me.cmdSetKey.Text = "&Set Key"
'
'cmdEncrypt
'
Me.cmdEncrypt.BackColor = System.Drawing.SystemColors.Control
Me.cmdEncrypt.Cursor = System.Windows.Forms.Cursors.Default
Me.cmdEncrypt.Enabled = False
Me.cmdEncrypt.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.cmdEncrypt.ForeColor = System.Drawing.SystemColors.ControlText
Me.cmdEncrypt.Location = New System.Drawing.Point(7, 116)
Me.cmdEncrypt.Name = "cmdEncrypt"
Me.cmdEncrypt.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.cmdEncrypt.Size = New System.Drawing.Size(68, 33)
Me.cmdEncrypt.TabIndex = 5
Me.cmdEncrypt.Text = "&Encrypt It"
'
'txtPassword
'
Me.txtPassword.AcceptsReturn = True
Me.txtPassword.AutoSize = False
Me.txtPassword.BackColor = System.Drawing.SystemColors.Window
Me.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam
Me.txtPassword.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.txtPassword.ForeColor = System.Drawing.SystemColors.WindowText
Me.txtPassword.Location = New System.Drawing.Point(120, 8)
Me.txtPassword.MaxLength = 0
Me.txtPassword.Name = "txtPassword"
Me.txtPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
Me.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.txtPassword.Size = New System.Drawing.Size(272, 25)
Me.txtPassword.TabIndex = 0
Me.txtPassword.Text = ""
'
'txtPlain
'
Me.txtPlain.AcceptsReturn = True
Me.txtPlain.AutoSize = False
Me.txtPlain.BackColor = System.Drawing.SystemColors.Window
Me.txtPlain.Cursor = System.Windows.Forms.Cursors.IBeam
Me.txtPlain.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.txtPlain.ForeColor = System.Drawing.SystemColors.WindowText
Me.txtPlain.Location = New System.Drawing.Point(80, 96)
Me.txtPlain.MaxLength = 0
Me.txtPlain.Multiline = True
Me.txtPlain.Name = "txtPlain"
Me.txtPlain.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.txtPlain.Size = New System.Drawing.Size(256, 56)
Me.txtPlain.TabIndex = 4
Me.txtPlain.Text = ""
'
'Label7
'
Me.Label7.BackColor = System.Drawing.SystemColors.Control
Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
Me.Label7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
Me.Label7.Location = New System.Drawing.Point(400, 192)
Me.Label7.Name = "Label7"
Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.Label7.Size = New System.Drawing.Size(57, 25)
Me.Label7.TabIndex = 20
Me.Label7.Text = "(Radix64):"
'
'_Label2_1
'
Me._Label2_1.BackColor = System.Drawing.SystemColors.Control
Me._Label2_1.Cursor = System.Windows.Forms.Cursors.Default
Me._Label2_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me._Label2_1.ForeColor = System.Drawing.SystemColors.ControlText
Me.Label2.SetIndex(Me._Label2_1, CType(1, Short))
Me._Label2_1.Location = New System.Drawing.Point(400, 160)
Me._Label2_1.Name = "_Label2_1"
Me._Label2_1.RightToLeft = System.Windows.Forms.RightToLeft.No
Me._Label2_1.Size = New System.Drawing.Size(65, 17)
Me._Label2_1.TabIndex = 12
Me._Label2_1.Text = "(In hex):"
'
'Label5
'
Me.Label5.BackColor = System.Drawing.SystemColors.Control
Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
Me.Label5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
Me.Label5.Location = New System.Drawing.Point(16, 160)
Me.Label5.Name = "Label5"
Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.Label5.Size = New System.Drawing.Size(65, 17)
Me.Label5.TabIndex = 11
Me.Label5.Text = "Deciphered"
'
'Label4
'
Me.Label4.BackColor = System.Drawing.SystemColors.Control
Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
Me.Label4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
Me.Label4.Location = New System.Drawing.Point(400, 64)
Me.Label4.Name = "Label4"
Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.Label4.Size = New System.Drawing.Size(57, 17)
Me.Label4.TabIndex = 8
Me.Label4.Text = "Active key:"
'
'Label33
'
Me.Label33.BackColor = System.Drawing.SystemColors.Control
Me.Label33.Cursor = System.Windows.Forms.Cursors.Default
Me.Label33.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.Label33.ForeColor = System.Drawing.SystemColors.ControlText
Me.Label33.Location = New System.Drawing.Point(53, 9)
Me.Label33.Name = "Label33"
Me.Label33.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.Label33.Size = New System.Drawing.Size(55, 16)
Me.Label33.TabIndex = 4
Me.Label33.Text = "Password"
'
'_Label2_0
'
Me._Label2_0.BackColor = System.Drawing.SystemColors.Control
Me._Label2_0.Cursor = System.Windows.Forms.Cursors.Default
Me._Label2_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me._Label2_0.ForeColor = System.Drawing.SystemColors.ControlText
Me.Label2.SetIndex(Me._Label2_0, CType(0, Short))
Me._Label2_0.Location = New System.Drawing.Point(400, 96)
Me._Label2_0.Name = "_Label2_0"
Me._Label2_0.RightToLeft = System.Windows.Forms.RightToLeft.No
Me._Label2_0.Size = New System.Drawing.Size(65, 17)
Me._Label2_0.TabIndex = 3
Me._Label2_0.Text = "Cipher text:"
'
'Label1
'
Me.Label1.BackColor = System.Drawing.SystemColors.Control
Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
Me.Label1.Location = New System.Drawing.Point(16, 96)
Me.Label1.Name = "Label1"
Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.Label1.Size = New System.Drawing.Size(65, 16)
Me.Label1.TabIndex = 2
Me.Label1.Text = "Plain text:"
'
'txtPassword2
'
Me.txtPassword2.AcceptsReturn = True
Me.txtPassword2.AutoSize = False
Me.txtPassword2.BackColor = System.Drawing.SystemColors.Window
Me.txtPassword2.Cursor = System.Windows.Forms.Cursors.IBeam
Me.txtPassword2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.txtPassword2.ForeColor = System.Drawing.SystemColors.WindowText
Me.txtPassword2.Location = New System.Drawing.Point(120, 32)
Me.txtPassword2.MaxLength = 0
Me.txtPassword2.Name = "txtPassword2"
Me.txtPassword2.PasswordChar = Microsoft.VisualBasic.ChrW(42)
Me.txtPassword2.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.txtPassword2.Size = New System.Drawing.Size(272, 25)
Me.txtPassword2.TabIndex = 1
Me.txtPassword2.Text = ""
'
'Label6
'
Me.Label6.BackColor = System.Drawing.SystemColors.Control
Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
Me.Label6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
Me.Label6.Location = New System.Drawing.Point(8, 34)
Me.Label6.Name = "Label6"
Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.Label6.Size = New System.Drawing.Size(105, 17)
Me.Label6.TabIndex = 24
Me.Label6.Text = "Re-Enter Password"
'
'Label11
'
Me.Label11.Location = New System.Drawing.Point(16, 272)
Me.Label11.Name = "Label11"
Me.Label11.Size = New System.Drawing.Size(64, 16)
Me.Label11.TabIndex = 35
Me.Label11.Text = "Output File"
'
'txtOutputFile
'
Me.txtOutputFile.Font = New System.Drawing.Font("Arial", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.txtOutputFile.Location = New System.Drawing.Point(80, 272)
Me.txtOutputFile.Name = "txtOutputFile"
Me.txtOutputFile.Size = New System.Drawing.Size(520, 18)
Me.txtOutputFile.TabIndex = 9
Me.txtOutputFile.Text = "C:\"
'
'btnBrowse2
'
Me.btnBrowse2.Location = New System.Drawing.Point(608, 272)
Me.btnBrowse2.Name = "btnBrowse2"
Me.btnBrowse2.Size = New System.Drawing.Size(24, 24)
Me.btnBrowse2.TabIndex = 10
Me.btnBrowse2.Text = "..."
'
'btnAction
'
Me.btnAction.Location = New System.Drawing.Point(560, 304)
Me.btnAction.Name = "btnAction"
Me.btnAction.TabIndex = 15
Me.btnAction.Text = "Encrypt"
'
'Label3
'
Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.Label3.Location = New System.Drawing.Point(16, 352)
Me.Label3.Name = "Label3"
Me.Label3.Size = New System.Drawing.Size(720, 48)
Me.Label3.TabIndex = 30
'
'Label13
'
Me.Label13.Location = New System.Drawing.Point(16, 240)
Me.Label13.Name = "Label13"
Me.Label13.Size = New System.Drawing.Size(64, 16)
Me.Label13.TabIndex = 29
Me.Label13.Text = "Source File"
'
'txtSourceFile
'
Me.txtSourceFile.Font = New System.Drawing.Font("Arial", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.txtSourceFile.Location = New System.Drawing.Point(80, 240)
Me.txtSourceFile.Name = "txtSourceFile"
Me.txtSourceFile.Size = New System.Drawing.Size(520, 18)
Me.txtSourceFile.TabIndex = 7
Me.txtSourceFile.Text = "C:\"
'
'btnBrowse1
'
Me.btnBrowse1.Location = New System.Drawing.Point(608, 240)
Me.btnBrowse1.Name = "btnBrowse1"
Me.btnBrowse1.Size = New System.Drawing.Size(24, 24)
Me.btnBrowse1.TabIndex = 8
Me.btnBrowse1.Text = "..."
'
'chkENCFiles
'
Me.chkENCFiles.Location = New System.Drawing.Point(640, 238)
Me.chkENCFiles.Name = "chkENCFiles"
Me.chkENCFiles.Size = New System.Drawing.Size(64, 16)
Me.chkENCFiles.TabIndex = 11
Me.chkENCFiles.Text = "All Files"
'
'chkENCFilesSubFolders
'
Me.chkENCFilesSubFolders.Location = New System.Drawing.Point(640, 258)
Me.chkENCFilesSubFolders.Name = "chkENCFilesSubFolders"
Me.chkENCFilesSubFolders.Size = New System.Drawing.Size(160, 16)
Me.chkENCFilesSubFolders.TabIndex = 12
Me.chkENCFilesSubFolders.Text = "All Files and Subfolders"
'
'chkENCOverlay
'
Me.chkENCOverlay.Location = New System.Drawing.Point(640, 279)
Me.chkENCOverlay.Name = "chkENCOverlay"
Me.chkENCOverlay.Size = New System.Drawing.Size(112, 16)
Me.chkENCOverlay.TabIndex = 13
Me.chkENCOverlay.Text = "Overlay Original?"
'
'grpBox1
'
Me.grpBox1.Controls.AddRange(New System.Windows.Forms.Control() {Me.rdoDecrypt, Me.rdoEncrypt})
Me.grpBox1.Location = New System.Drawing.Point(304, 304)
Me.grpBox1.Name = "grpBox1"
Me.grpBox1.Size = New System.Drawing.Size(240, 41)
Me.grpBox1.TabIndex = 14
Me.grpBox1.TabStop = False
Me.grpBox1.Text = "Crypt Method"
'
'rdoDecrypt
'
Me.rdoDecrypt.Location = New System.Drawing.Point(128, 16)
Me.rdoDecrypt.Name = "rdoDecrypt"
Me.rdoDecrypt.Size = New System.Drawing.Size(64, 16)
Me.rdoDecrypt.TabIndex = 1
Me.rdoDecrypt.Text = "Decrypt"
'
'rdoEncrypt
'
Me.rdoEncrypt.Checked = True
Me.rdoEncrypt.Location = New System.Drawing.Point(48, 16)
Me.rdoEncrypt.Name = "rdoEncrypt"
Me.rdoEncrypt.Size = New System.Drawing.Size(64, 16)
Me.rdoEncrypt.TabIndex = 0
Me.rdoEncrypt.TabStop = True
Me.rdoEncrypt.Text = "Encrypt"
'
'txtStatus
'
Me.txtStatus.Location = New System.Drawing.Point(16, 416)
Me.txtStatus.Multiline = True
Me.txtStatus.Name = "txtStatus"
Me.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
Me.txtStatus.Size = New System.Drawing.Size(800, 96)
Me.txtStatus.TabIndex = 17
Me.txtStatus.Text = ""
'
'btnClear
'
Me.btnClear.Location = New System.Drawing.Point(752, 368)
Me.btnClear.Name = "btnClear"
Me.btnClear.Size = New System.Drawing.Size(64, 23)
Me.btnClear.TabIndex = 16
Me.btnClear.Text = "Clear Log"
'
'lblShowPasswords
'
Me.lblShowPasswords.Location = New System.Drawing.Point(368, 64)
Me.lblShowPasswords.Name = "lblShowPasswords"
Me.lblShowPasswords.Size = New System.Drawing.Size(16, 16)
Me.lblShowPasswords.TabIndex = 49
'
'frmTAPDANCE
'
Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
Me.ClientSize = New System.Drawing.Size(824, 517)
Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblShowPasswords, Me.btnClear, Me.txtStatus, Me.grpBox1, Me.chkENCOverlay, Me.chkENCFilesSubFolders, Me.chkENCFiles, Me.Label11, Me.txtOutputFile, Me.btnBrowse2, Me.btnAction, Me.Label3, Me.Label13, Me.txtSourceFile, Me.btnBrowse1, Me.txtPassword2, Me.Label6, Me.txtCipher64, Me.grpKeyForm, Me.txtCipherHex, Me.txtCipher, Me.txtDecrypt, Me.cmdDecrypt, Me.txtKeyAsString, Me.cmdSetKey, Me.cmdEncrypt, Me.txtPassword, Me.txtPlain, Me.Label7, Me._Label2_1, Me.Label5, Me.Label4, Me.Label33, Me._Label2_0, Me.Label1})
Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
Me.Location = New System.Drawing.Point(4, 23)
Me.MaximizeBox = False
Me.Name = "frmTAPDANCE"
Me.Text = "BLOWFISH.NET"
Me.grpKeyForm.ResumeLayout(False)
CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
Me.grpBox1.ResumeLayout(False)
Me.ResumeLayout(False)

    End Sub
#End Region

#Region "conversion notice"
    '****************************************
    'CONVERTED TO .NET by TODD ACHESON,  todd@acheson.com
    'January 2003
    'The .NET version definitely could be streamlined and 
    'cleaned up a bit to be more efficient.  Used the upgrade wizard
    'and tinkered with it a bit myself.  S and P box arrays are the 
    'same as the VB6 version.
    'Added ability to crypt one file, all files in a folder, and
    'all files/folders in a one folder recursively.
    '*****************************************
#End Region

    Dim aKey() As Byte
    Dim strCipher As String ' Used to store ciphertext
    Dim objDataTable As New DataTable()
    Dim objDataRowGrid As DataRow
    Dim blnCrypt As Boolean = True

    Private Sub cmdEncrypt_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEncrypt.Click
        strCipher = blf_StringEnc(txtPlain.Text)
        txtCipher.Text = strCipher
        txtCipherHex.Text = cv_HexFromString(strCipher)
        txtCipher64.Text = EncodeStr64(strCipher)
        cmdDecrypt.Enabled = True
    End Sub

    Private Sub cmdDecrypt_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDecrypt.Click
        txtDecrypt.Text = blf_StringDec(strCipher)
    End Sub

    Private Sub cmdSetKey_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSetKey.Click
        Try
            ' Get key bytes from user's string
            If Len(Trim(txtPassword.Text)) = 0 Then Err.Raise(9999, , "Please enter a password (twice)")
            If txtPassword.Text <> txtPassword2.Text Then Err.Raise(9999, , "The passwords you entered do not match.")
            ' What format is it in
            If optHexKey.Checked Then
                ' In hex format
                aKey = cv_BytesFromHex(txtPassword.Text)
            Else
                ' User has provided a plain alpha string
                aKey = cv_BytesFromString(txtPassword.Text)
            End If
            ' Show key
            txtKeyAsString.Text = cv_HexFromBytes(aKey)
            'Initialise key
            blf_KeyInit(aKey)
            ' Allow encrypt
            cmdEncrypt.Enabled = True
            ' Put user in plaintext box
            txtPlain.Focus()
        Catch
            Label3.Text = Err.Description
        Finally
            txtPassword.Focus()
        End Try

    End Sub

    Private Sub btnBrowse1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse1.Click
        OpenFileDialog1.InitialDirectory = "C:\"
        OpenFileDialog1.Filter = "All files (*.*)|*.*"
        OpenFileDialog1.FilterIndex = 1
        OpenFileDialog1.Title = "Select Source File to ENCRYPT"
        OpenFileDialog1.RestoreDirectory = True
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            txtSourceFile.Text = OpenFileDialog1.FileName
            txtOutputFile.Text = CreateInitialOutputFileName(txtSourceFile.Text, True)
        End If
    End Sub

    Private Sub btnBrowse2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse2.Click
        OpenFileDialog1.InitialDirectory = "C:\"
        OpenFileDialog1.Filter = "All files (*.*)|*.*"
        OpenFileDialog1.FilterIndex = 1
        OpenFileDialog1.Title = "Select Destination for ENCRYPTED File"
        OpenFileDialog1.RestoreDirectory = True
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            txtOutputFile.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub FileList(ByVal Pathname As String, _
                        ByVal blnRecursive As Boolean, _
                        Optional ByVal DirCount As Long = 0, _
                        Optional ByVal FileCount As Long = 0)
        Try
            Dim ShortName As String, LongName As String
            Dim NextDir As String
            Static FolderList As Collection
            Dim strExt As String
            Dim xCount As Integer
            If FolderList Is Nothing Then
                FolderList = New Collection()
                FolderList.Add(Mid(Pathname, 1, Len(Pathname) - 1))
                DirCount = 0
                FileCount = 0
            End If
            objDataTable = New DataTable()
            objDataTable.Columns.Add(New DataColumn("lngFileID", GetType(Integer)))
            objDataTable.Columns.Add(New DataColumn("File Name"))

            Do
                'Obtain next directory from list
                NextDir = FolderList.Item(1)
                'Remove next directory from list
                FolderList.Remove(1)
                'List files in directory
                ShortName = Dir(NextDir & "\*.*", vbNormal Or vbArchive Or vbDirectory Or vbHidden Or vbReadOnly Or vbSystem Or vbVolume)

                Do While ShortName > ""
                    If ShortName = "." Or ShortName = ".." Then
                        'skip it
                    Else
                        LongName = NextDir & "\" & ShortName
                        If (GetAttr(LongName) And vbDirectory) > 0 Then
                            'it's a directory - add it to the list of directories to process
                            If blnRecursive = True Then
                                FolderList.Add(LongName)
                                DirCount = DirCount + 1
                            End If
                        Else
                            CryptOneFile(LongName, CreateInitialOutputFileName(LongName, blnCrypt))

                            objDataRowGrid = objDataTable.NewRow
                            objDataRowGrid(0) = xCount + 1
                            objDataRowGrid(1) = LongName
                            objDataTable.Rows.Add(objDataRowGrid)
                            xCount += 1
                        End If
                    End If
                    ShortName = Dir()
                Loop
            Loop Until FolderList.Count = 0
            FolderList = Nothing
        Catch
            Label3.Text = Err.Description
            Label3.ForeColor = Color.Red
        End Try
    End Sub

    Private Sub btnAction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAction.Click
        Try
            Cursor = Cursors.WaitCursor
            txtStatus.Text = txtStatus.Text.Insert(Len(txtStatus.Text), "*************" & vbCrLf)
            txtStatus.Refresh()
            txtStatus.Text = txtStatus.Text.Insert(Len(txtStatus.Text), "Starting Crypt..." & vbCrLf)
            txtStatus.Refresh()

            If txtSourceFile.Text = "" Or txtOutputFile.Text = "" Or txtPassword.Text = "" Or txtPassword2.Text = "" Then
                Err.Raise(9999, , "You must enter a source filename, destination filename, and a password.")
                Exit Sub
            ElseIf txtPassword.Text <> txtPassword2.Text Then
                Err.Raise(9999, , "The passwords you entered do NOT match.  Please try again.")
                Exit Sub
            End If
            cmdSetKey_Click(Me, e)
            If chkENCFiles.Checked = False And chkENCFilesSubFolders.Checked = False Then
                CryptOneFile(txtSourceFile.Text, txtOutputFile.Text)
            Else
                FileList(System.IO.Path.GetDirectoryName(txtSourceFile.Text) & "\", chkENCFilesSubFolders.Checked, True)
            End If
            txtStatus.Text = txtStatus.Text.Insert(Len(txtStatus.Text), "Complete" & vbCrLf & "=============" & vbCrLf)
            txtStatus.Refresh()
            Cursor = Cursors.Default
        Catch
            Cursor = Cursors.Default
            Label3.Text = Err.Description
        End Try

    End Sub

    Private Sub CryptOneFile(ByVal pstrSourceFile As String, ByVal pstrOutputFile As String)
        Dim objStream As System.IO.FileStream
        Dim objReader As System.IO.BinaryReader
        Dim intATTR As Integer
        Dim objFIle As System.IO.File

        objStream = New System.IO.FileStream(pstrSourceFile, IO.FileMode.OpenOrCreate)
        objReader = New System.IO.BinaryReader(objStream)

        Dim allBytes() As Byte
        allBytes = objReader.ReadBytes(objStream.Length)
        objReader.Close()
        objStream.Close()
        objReader = Nothing

        txtStatus.Text = txtStatus.Text.Insert(Len(txtStatus.Text), "File: " & pstrSourceFile)
        txtStatus.Refresh()

        allBytes = blf_ByteCrypt(allBytes, blnCrypt)

        intATTR = objFIle.GetAttributes(pstrSourceFile)
        Dim objWriter As System.IO.BinaryWriter
        If chkENCOverlay.Checked = True Then
            pstrOutputFile = pstrSourceFile
            objFIle.Delete(pstrSourceFile)
        End If

        objStream = New System.IO.FileStream(pstrOutputFile, IO.FileMode.Create)
        objWriter = New System.IO.BinaryWriter(objStream)
        objWriter.Write(allBytes)
        objWriter.Close()
        objStream.Close()
        objWriter = Nothing
        objStream = Nothing
        allBytes = Nothing
        objFIle.SetAttributes(pstrOutputFile, intATTR)

        If blnCrypt = True Then
            txtStatus.Text = txtStatus.Text.Insert(Len(txtStatus.Text), "; Encrypted to " & pstrOutputFile & vbCrLf)
        Else
            txtStatus.Text = txtStatus.Text.Insert(Len(txtStatus.Text), "; Decrypted to " & pstrOutputFile & vbCrLf)
        End If
        txtStatus.Refresh()

        GC.Collect()
    End Sub


    Private Function CreateInitialOutputFileName(ByVal pstrSourceFile As String, ByVal blnEncrypt As Boolean) As String
        Dim S As String
        Dim ext As String 'file extension
        Dim mainpath As String 'the file path minus the extension
        Dim n As Integer 'location of period in filepath

        n = pstrSourceFile.IndexOf(".") ' returns -1 if there is no extension
        If n <> -1 Then 'extract the extension
            ext = pstrSourceFile.Substring(n + 1)
            mainpath = pstrSourceFile.Substring(0, pstrSourceFile.Length - ext.Length - 1)
        Else
            mainpath = pstrSourceFile
        End If
        If blnEncrypt = False Then
            If mainpath.Substring(mainpath.Length - 2) = "xx" Then 'this file will be decrypted
                mainpath = mainpath.Substring(0, mainpath.Length - 2)
                If ext <> "" Then mainpath &= "_NEW." & ext
                Return mainpath
            End If
        Else
            mainpath &= "xx"
            If ext <> "" Then mainpath &= "." & ext
            Return mainpath
        End If

    End Function

    Private Sub chkENCFiles_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkENCFiles.CheckedChanged
        If chkENCFiles.Checked = True Then
            txtOutputFile.Enabled = False
        Else
            If chkENCFilesSubFolders.Checked = True Then
                txtOutputFile.Enabled = False
            Else
                txtOutputFile.Enabled = True
            End If
        End If
    End Sub

    Private Sub chkENCFilesSubFolders_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkENCFilesSubFolders.CheckedChanged
        If chkENCFilesSubFolders.Checked = True Then
            chkENCFiles.Checked = True
            txtOutputFile.Enabled = False
        Else
            If chkENCFiles.Checked = True Then
                txtOutputFile.Enabled = False
            Else
                chkENCFiles.Checked = True
                txtOutputFile.Enabled = True
            End If
        End If
    End Sub

    Private Sub chkOverlay_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkENCOverlay.CheckedChanged
        If chkENCOverlay.Checked = True Then
            txtOutputFile.Enabled = False
        Else
            txtOutputFile.Enabled = True
        End If
    End Sub

    Private Sub rdoEncrypt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoEncrypt.CheckedChanged
        If rdoEncrypt.Checked = True Then
            btnAction.Text = "Encrypt"
            blnCrypt = True
        Else
            btnAction.Text = "Decrypt"
            blnCrypt = False
        End If
    End Sub

    Private Sub rdoDecrypt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoDecrypt.CheckedChanged
        If rdoDecrypt.Checked = True Then
            btnAction.Text = "Decrypt"
            blnCrypt = False
        Else
            btnAction.Text = "Encrypt"
            blnCrypt = True
        End If
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtStatus.Text = ""
    End Sub

    Private Sub lblShowPasswords_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblShowPasswords.DoubleClick
        MsgBox("Password1: = " & txtPassword.Text & vbCrLf & "Password2: = " & txtPassword2.Text)
    End Sub

End Class