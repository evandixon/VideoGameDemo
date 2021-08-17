Imports Logic
Module Module1
    WithEvents battlefield As New Battlefield
    Sub Main()
        'ResetCharacters()
        DisplayStats()
        CheckCommand()
        Console.ReadLine()
    End Sub
    Sub ResetCharacters()
        battlefield.Enemies.Clear()

    End Sub
    Sub CheckCommand()
        Dim command As String = Console.ReadLine
        Select Case command
            'Case "reset"
            '    ResetCharacters()
            '    DisplayStats()
            'Case "reset dummy"
            '    Console.WriteLine("New dummy.")
            '    ResetDummy()
            '    DisplayStats()
            Case "attack"
                battlefield.Current.Enemies(0) = battlefield.Current.Player.DealDamage(battlefield.Current.Player.Weapons(0), battlefield.Current.Enemies(0))
                DisplayStats()
                If battlefield.Current.Player.TurnLengthLeft < battlefield.Player.Weapons(0).TurnCost Then
                    battlefield.Current.EndPlayerTurn()
                End If
            Case "doubletest"
                Console.WriteLine(Common.ByteArrToHex(BitConverter.GetBytes(CType(1, Double))))
                Console.WriteLine(Common.ByteArrToHex(BitConverter.GetBytes(Double.MaxValue)))
            Case Else
                If command.StartsWith("weapongen") Then
                    Dim w As Weapon = WeaponType.KnifeWeaponType.GenerateWeapon("Knife", command.Split(" ")(1))
                    Console.WriteLine(w.DebugStats)
                    'Console.WriteLine("")
                    'Console.WriteLine(Weapon.FromByteArr(w.ToByteArr).DebugStats)
                End If
        End Select

        CheckCommand()
    End Sub
    Sub DisplayStats()
        Console.WriteLine(battlefield.Player.DebugStats)
        Console.WriteLine("")
        Console.WriteLine(battlefield.Enemies(0).DebugStats)
    End Sub

    Private Sub battlefield_EnemyDidAttack(DamageDealt As Integer, DamagedFoe As Character, Enemy As Enemy) Handles battlefield.EnemyDidAttack
        Console.WriteLine(Enemy.Name & " dealt " & DamageDealt & " damage to" & DamagedFoe.Name)
    End Sub
    Private Sub player_LeveledUp(sender As Object, NewLevel As Integer) Handles battlefield.PlayerLeveledUp
        Console.WriteLine("Player leveled up to level " & NewLevel)
    End Sub

    Private Sub dummy_Defeated(sender As Object) Handles battlefield.EnemyDefeated
        Console.WriteLine("Player defeated dummy.")
        Console.WriteLine("Player recieved " & sender.ExpReleasedWhenDefeated & " experience.")
        battlefield.Current.Player.RecieveExp(sender.ExpReleasedWhenDefeated, battlefield.Current.Player.Weapons(0))
        Console.WriteLine("Resetting dummy...")
        'ResetDummy()
        DisplayStats()
    End Sub
End Module
