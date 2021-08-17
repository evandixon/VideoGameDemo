''' <summary>
''' Depricated; needs changing
''' </summary>
''' <remarks></remarks>
Public MustInherit Class Enemy
    Inherits Character

    Public Event Attacked(DamageDealt As Integer, DamagedFoe As Character, Enemy As Enemy)
    Public Shadows Event Defeated(Enemy As Enemy)
    Public MustOverride Sub DoTurn()

    Public Sub Attack(CurrentWeapon As Weapon)
        Dim UndamagedPlayer As Character = Battlefield.Current.Player
        Battlefield.Current.Player = DealDamage(CurrentWeapon, Battlefield.Current.Player)
        RaiseEvent Attacked(UndamagedPlayer.CurrentHealth - Battlefield.Current.Player.CurrentHealth, Battlefield.Current.Player, Me)
    End Sub
    Protected Sub RaiseEventDefeated(Enemy As Enemy)
        RaiseEvent Defeated(Me)
    End Sub
End Class
Public Class EnemyList
    Inherits Generic.List(Of Enemy)
    Public Event EnemyDidAttack(DamageDealt As Integer, DamagedFoe As Character, Enemy As Enemy)
    Public Event EnemyDefeated(Enemy As Character)
    Public Overloads Sub Add(Item As Enemy)
        MyBase.Add(Item)
        AddHandler (Item.Attacked), AddressOf EnemyAttackedHandler
        AddHandler (Item.Defeated), AddressOf EnemyDefeatedHandler
    End Sub
    Public Overloads Sub RemoteAt(Index As Integer)
        RemoveHandler DirectCast(Item(Index), Enemy).Attacked, AddressOf EnemyAttackedHandler
        RemoveHandler DirectCast(Item(Index), Enemy).Defeated, AddressOf EnemyDefeatedHandler
        MyBase.RemoveAt(Index)
    End Sub
    Private Sub EnemyAttackedHandler(DamageDealt As Integer, DamagedFoe As Character, Enemy As Enemy)
        RaiseEvent EnemyDidAttack(DamageDealt, DamagedFoe, Enemy)
    End Sub
    Private Sub EnemyDefeatedHandler(Enemy As Enemy)
        RaiseEvent EnemyDefeated(Enemy)
    End Sub
End Class
Public Class lvl1Dummy
    Inherits Enemy
    Public Overrides Sub DoTurn()
        While (TurnLengthLeft >= Weapons(0).TurnCost) AndAlso CurrentHealth > 0 AndAlso Battlefield.Current.PlayerIsAlive
            Attack(Weapons(0))
        End While
        If CurrentHealth < 1 Then
            RaiseEventDefeated(Me)
        End If
    End Sub

    Public Sub New()
        Name = "Dummy"
        Me.Weapons.Add(Weapon.FromByteArr(My.Resources.lvl1ButterKnife))
    End Sub
End Class