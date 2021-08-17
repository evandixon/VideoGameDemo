''' <summary>
''' Depricated; will possibly be deleted
''' </summary>
''' <remarks></remarks>
Public Class Battlefield
#Region "Current"
    Public Shared Property Current As Battlefield
    Shared Sub New()
        Current = New Battlefield
    End Sub
#End Region
    WithEvents _enemies As EnemyList
    Public Property Enemies As EnemyList
        Get
            Return _enemies
        End Get
        Set(value As EnemyList)
            _enemies = value
        End Set
    End Property
#Region "Events"
#Region "Enemies"
    Public Event EnemyDefeated(Enemy As Enemy)
    Private Sub _enemies_EnemyDefeated(Enemy As Character) Handles _enemies.EnemyDefeated
        RaiseEvent EnemyDefeated(Enemy)
    End Sub
    Public Event EnemyDidAttack(DamageDealt As Integer, DamagedFoe As Character, Enemy As Enemy)
    Private Sub _enemies_EnemyDidAttack(DamageDealt As Integer, DamagedFoe As Character, Enemy As Enemy) Handles _enemies.EnemyDidAttack
        RaiseEvent EnemyDidAttack(DamageDealt, DamagedFoe, Enemy)
    End Sub
#End Region
#Region "Player"
    Public Event PlayerLeveledUp(Player As Character, NewLevel As Integer)
    Private Sub _player_LeveledUp(sender As Character, NewLevel As Integer) Handles _player.LeveledUp
        RaiseEvent PlayerLeveledUp(Player, NewLevel)
    End Sub
#End Region
#End Region
    WithEvents _player As Character
    Public Property Player As Character
        Get
            Return _player
        End Get
        Set(value As Character)
            _player = value
        End Set
    End Property
    Public ReadOnly Property PlayerIsAlive As Boolean
        Get
            Return Player.CurrentHealth > 0
        End Get
    End Property
    Public Sub EndPlayerTurn()
        For Each item In Enemies
            item.DoTurn()
        Next
        For count As Integer = Enemies.Count - 1 To 0 Step -1
            If Enemies(count).CurrentHealth <= 0 Then
                Enemies.RemoveAt(count)
            End If
        Next
        For count As Integer = Enemies.Count - 1 To 0 Step -1
            Enemies(count).TurnLengthLeft = 1
        Next
        Player.TurnLengthLeft = 1
    End Sub
    Public Sub New()
        Enemies = New EnemyList
        Enemies.Add(New lvl1Dummy)
        'Player = Characters.KnifeHolder
    End Sub
End Class