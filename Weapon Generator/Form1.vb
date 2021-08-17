Imports Logic
Public Class Form1
    Dim checklevel As Boolean = False
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.AddRange(WeaponType.GetWeaponTypes)
        ComboBox1.SelectedIndex = 0
        checklevel = True
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim w As Weapon = DirectCast(ComboBox1.SelectedItem, WeaponType).GenerateWeapon(txtName.Text, numLevel.Value)
        txtName.Text = w.Name
        numDamage.Value = w.BaseDamage
        txtTurnCost.Text = w.FrameWait
        CheckBox1.Checked = w.IsRecharge
        numMagazine.Value = w.Magazine
        numCurrentLoad.Value = w.CurrentMagazineLoad
        numRange.Value = w.Range
        'txtAccuracy.Text = w.Accuracy
        txtReload.Text = w.ReloadSpeed
        'txtCritHit.Text = w.CriticalHitChance
    End Sub
    Sub ReloadLevelRequirement()
        If checklevel Then
            On Error Resume Next
            Dim w As New Weapon
            w.Name = txtName.Text
            w.Type = ComboBox1.SelectedItem
            w.BaseDamage = numDamage.Value
            w.FrameWait = txtTurnCost.Text
            w.IsRecharge = CheckBox1.Checked
            w.Magazine = numMagazine.Value
            w.CurrentMagazineLoad = numCurrentLoad.Value
            w.Range = numRange.Value
            ' w.Accuracy = txtAccuracy.Text
            w.ReloadSpeed = txtReload.Text
            'w.CriticalHitChance = txtCritHit.Text
            Label17.Text = "Level Requirement: " & w.LevelRequirement
        End If
    End Sub
    Private Sub txtAccuracy_TextChanged(sender As Object, e As EventArgs) Handles txtTurnCost.TextChanged, txtReload.TextChanged
        ReloadLevelRequirement()
    End Sub

    Private Sub numMagazine_ValueChanged(sender As Object, e As EventArgs) Handles numRange.ValueChanged, numMagazine.ValueChanged, numDamage.ValueChanged, numCurrentLoad.ValueChanged
        ReloadLevelRequirement()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim w As Weapon = Weapon.FromBinary(IO.File.ReadAllBytes(OpenFileDialog1.FileName))
            txtName.Text = w.Name
            ComboBox1.SelectedItem = w.Type
            numDamage.Value = w.BaseDamage
            txtTurnCost.Text = w.FrameWait
            CheckBox1.Checked = w.IsRecharge
            numMagazine.Value = w.Magazine
            numCurrentLoad.Value = w.CurrentMagazineLoad
            numRange.Value = w.Range
            'txtAccuracy.Text = w.Accuracy
            txtReload.Text = w.ReloadSpeed
            'txtCritHit.Text = w.CriticalHitChance
            ReloadLevelRequirement()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim w As New Weapon
            w.Name = txtName.Text
            w.Type = ComboBox1.SelectedItem
            w.BaseDamage = numDamage.Value
            w.FrameWait = txtTurnCost.Text
            w.IsRecharge = CheckBox1.Checked
            w.Magazine = numMagazine.Value
            w.CurrentMagazineLoad = numCurrentLoad.Value
            w.Range = numRange.Value
            'w.Accuracy = txtAccuracy.Text
            w.ReloadSpeed = txtReload.Text
            'w.CriticalHitChance = txtCritHit.Text
            IO.File.WriteAllBytes(SaveFileDialog1.FileName, w.ToByteArr)
        End If
    End Sub
End Class
