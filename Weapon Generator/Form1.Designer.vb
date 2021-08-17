<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.numDamage = New System.Windows.Forms.NumericUpDown()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.numMagazine = New System.Windows.Forms.NumericUpDown()
        Me.numCurrentLoad = New System.Windows.Forms.NumericUpDown()
        Me.numRange = New System.Windows.Forms.NumericUpDown()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.txtReload = New System.Windows.Forms.TextBox()
        Me.txtTurnCost = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.numLevel = New System.Windows.Forms.NumericUpDown()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        CType(Me.numDamage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numMagazine, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numCurrentLoad, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numRange, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numLevel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Type"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 67)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(74, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Base Damage"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 93)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Frame Wait"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 117)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Is Recharge"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 139)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(53, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Magazine"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 165)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(68, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Current Load"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(11, 191)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(39, 13)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Range"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(12, 246)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(75, 13)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Reload Speed"
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(111, 12)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(253, 20)
        Me.txtName.TabIndex = 12
        '
        'ComboBox1
        '
        Me.ComboBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(111, 38)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(254, 21)
        Me.ComboBox1.TabIndex = 13
        '
        'numDamage
        '
        Me.numDamage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numDamage.Location = New System.Drawing.Point(112, 65)
        Me.numDamage.Maximum = New Decimal(New Integer() {276447232, 23283, 0, 0})
        Me.numDamage.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numDamage.Name = "numDamage"
        Me.numDamage.Size = New System.Drawing.Size(253, 20)
        Me.numDamage.TabIndex = 14
        Me.numDamage.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox1.Location = New System.Drawing.Point(112, 117)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox1.TabIndex = 16
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'numMagazine
        '
        Me.numMagazine.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numMagazine.Location = New System.Drawing.Point(111, 137)
        Me.numMagazine.Maximum = New Decimal(New Integer() {276447232, 23283, 0, 0})
        Me.numMagazine.Name = "numMagazine"
        Me.numMagazine.Size = New System.Drawing.Size(253, 20)
        Me.numMagazine.TabIndex = 17
        '
        'numCurrentLoad
        '
        Me.numCurrentLoad.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numCurrentLoad.Location = New System.Drawing.Point(111, 163)
        Me.numCurrentLoad.Maximum = New Decimal(New Integer() {276447232, 23283, 0, 0})
        Me.numCurrentLoad.Name = "numCurrentLoad"
        Me.numCurrentLoad.Size = New System.Drawing.Size(253, 20)
        Me.numCurrentLoad.TabIndex = 18
        '
        'numRange
        '
        Me.numRange.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numRange.Location = New System.Drawing.Point(111, 189)
        Me.numRange.Maximum = New Decimal(New Integer() {276447232, 23283, 0, 0})
        Me.numRange.Name = "numRange"
        Me.numRange.Size = New System.Drawing.Size(253, 20)
        Me.numRange.TabIndex = 19
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(13, 388)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 26
        Me.Button1.Text = "Save"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Location = New System.Drawing.Point(302, 388)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 27
        Me.Button2.Text = "Open"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.Location = New System.Drawing.Point(94, 388)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(202, 23)
        Me.Button3.TabIndex = 28
        Me.Button3.Text = "Randomize! (Based on weapon type)"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'txtReload
        '
        Me.txtReload.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtReload.Location = New System.Drawing.Point(111, 243)
        Me.txtReload.Name = "txtReload"
        Me.txtReload.Size = New System.Drawing.Size(253, 20)
        Me.txtReload.TabIndex = 30
        Me.txtReload.Text = "1"
        '
        'txtTurnCost
        '
        Me.txtTurnCost.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTurnCost.Location = New System.Drawing.Point(111, 90)
        Me.txtTurnCost.Name = "txtTurnCost"
        Me.txtTurnCost.Size = New System.Drawing.Size(253, 20)
        Me.txtTurnCost.TabIndex = 32
        Me.txtTurnCost.Text = "1"
        '
        'Label16
        '
        Me.Label16.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(94, 369)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(97, 13)
        Me.Label16.TabIndex = 33
        Me.Label16.Text = "Approximate Level:"
        '
        'numLevel
        '
        Me.numLevel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numLevel.Location = New System.Drawing.Point(197, 367)
        Me.numLevel.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numLevel.Name = "numLevel"
        Me.numLevel.Size = New System.Drawing.Size(99, 20)
        Me.numLevel.TabIndex = 34
        Me.numLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.numLevel.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(108, 292)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(91, 13)
        Me.Label17.TabIndex = 35
        Me.Label17.Text = "Required Level: 1"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "Binary files (*.bin)|*.bin|All Files (*.*)|*.*"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = "Binary files (*.bin)|*.bin|All Files (*.*)|*.*"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(389, 423)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.numLevel)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.txtTurnCost)
        Me.Controls.Add(Me.txtReload)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.numRange)
        Me.Controls.Add(Me.numCurrentLoad)
        Me.Controls.Add(Me.numMagazine)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.numDamage)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "Weapon Generator"
        CType(Me.numDamage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numMagazine, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numCurrentLoad, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numRange, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numLevel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents numDamage As System.Windows.Forms.NumericUpDown
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents numMagazine As System.Windows.Forms.NumericUpDown
    Friend WithEvents numCurrentLoad As System.Windows.Forms.NumericUpDown
    Friend WithEvents numRange As System.Windows.Forms.NumericUpDown
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents txtReload As System.Windows.Forms.TextBox
    Friend WithEvents txtTurnCost As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents numLevel As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog

End Class
