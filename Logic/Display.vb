Imports System.Drawing

Public Class Map
    Public Sub LoadNewMap(NewMapName As String, Point As Point)
        LoadNewMap(Map.FromBinary(IO.File.ReadAllBytes(IO.Path.Combine(Environment.CurrentDirectory, "Maps", NewMapName & ".map"))), NewMapName, Point)
    End Sub
    Public Sub LoadNewMap(Map As Map, NewMapName As String, EntryPoint As Point)
        Dim c As Character = MainCharacter
        If c Is Nothing Then
            c = SaveFile.CurrentSaveFile.Player
        End If
        Dim newMap As Map = Map.FromBinary(IO.File.ReadAllBytes(IO.Path.Combine(Environment.CurrentDirectory, "Maps", NewMapName & ".map")))
        ResetTemporaryProperties()
        Tiles = newMap.Tiles
        TileTemplate = newMap.TileTemplate
        BGMName = newMap.BGMName
        WarpPointCollection = newMap.WarpPointCollection
        Tiles(EntryPoint.Y)(EntryPoint.X).ContainedCharacter = c
        MainCharacter = Tiles(EntryPoint.Y)(EntryPoint.X).ContainedCharacter
        MapName = NewMapName

        'ensure characters are loaded
        For y As Integer = 0 To Tiles.Length - 1
            For x As Integer = 0 To Tiles(y).Length - 1
                If Tiles(y)(x).ContainedCharacter IsNot Nothing Then Tiles(y)(x).ContainedCharacter.AILogic.Load(Me, Tiles(y)(x).ContainedCharacter, New Point(x, y))
            Next
        Next

        RaiseEvent NewMapLoaded()
    End Sub
#Region "Events"
    Public Event MainCharacterAttacked()
    Public Event MainCharacterDefeated()
    Public Event EnemyDefeated(ByVal Enemy As Character)
    Public Event NewMapLoaded()
    Public Event WeaponFired(ByVal WeaponUsed As Weapon)
    Public Event FirstFrameDrawn()

    Public Event PendingDialogComplete(ByRef M As Map)
    Public Event ForegroundAudioChanged(NewAudioName As String)
    Public Event BGMNameChanged(NewBGMName As String)
#End Region
#Region "Constants"
    Public Const CharacterMovementStep As Integer = 12
    Public Const ViewingWindowTileCountWidth As Integer = 25
    Public Const ViewingWindowTileCountHeight As Integer = 16
    Const fontSizeHUD As Double = 14
    Const fontSizeDialog As Double = 16
    Const fontFace As String = "Times New Roman"
    Const requiredInteractWait As Integer = 25
#End Region
#Region "Savable Properties"
    Public Property WarpPointCollection As New Dictionary(Of Point, WarpPoint)
    Public Property TileTemplate As TileTemplate
    Public Property Tiles As Tile()()
    Dim _bgmName As String = ""
    Public Property BGMName As String
        Get
            Return _bgmName
        End Get
        Set(value As String)
            _bgmName = value
            RaiseEvent BGMNameChanged(value)
        End Set
    End Property
#End Region
#Region "Temporary Properties"
    Private Sub ResetTemporaryProperties()
        InteractframeCount = 0
        _acceptsmovementinput = True
        Bullets = New List(Of Bullet)
        _mainCharGuid = Guid.Empty
        _mainCharTile = Nothing
        _mapName = ""
        _firstFrameDrawn = False
        _background = Nothing
        _lastImage = Nothing
    End Sub
    ''' <summary>
    ''' Based on filename, so  that's why it is temporary
    ''' </summary>
    ''' <remarks></remarks>
    Dim _mapName As String = ""
    Public Property MapName As String
        Get
            Return _mapName
        End Get
        Set(value As String)
            _mapName = MapName
        End Set
    End Property

    Private InteractframeCount As Integer = 0
    Dim _firstFrameDrawn As Boolean = False
    Dim _mainCharTile As New Point
    Dim _mainCharGuid As Guid

    Private _lastImage As Bitmap = Nothing
    Dim _background As Bitmap = Nothing

    ''' <summary>
    ''' When setting, must be set to a character on a tile on the map.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MainCharacter As Character
        Get
            If Tiles IsNot Nothing Then
                If Tiles(_mainCharTile.Y)(_mainCharTile.X).ContainedCharacter IsNot Nothing AndAlso Tiles(_mainCharTile.Y)(_mainCharTile.X).ContainedCharacter.InternalID = _mainCharGuid Then
                    Return Tiles(_mainCharTile.Y)(_mainCharTile.X).ContainedCharacter
                Else
                    _mainCharTile = FindCharacterLocation(_mainCharGuid)
                    Return Tiles(_mainCharTile.Y)(_mainCharTile.X).ContainedCharacter
                End If
            Else
                Return Nothing
            End If
        End Get
        Set(value As Character)
            _mainCharGuid = value.InternalID
        End Set
    End Property
    Public ReadOnly Property MainCharacterLocation As Point
        Get
            If Tiles(_mainCharTile.Y)(_mainCharTile.X).ContainedCharacter IsNot Nothing AndAlso Tiles(_mainCharTile.Y)(_mainCharTile.X).ContainedCharacter.InternalID = _mainCharGuid Then
                Return _mainCharTile
            Else
                _mainCharTile = FindCharacterLocation(_mainCharGuid)
                Return _mainCharTile
            End If
        End Get
    End Property
    Public Function FindCharacterLocation(CharacterID As Guid) As Point
        Dim out As Point = Nothing
        For count As Integer = 0 To Tiles.Length - 1
            Dim row As Tile() = Tiles(count)
            For count2 As Integer = 0 To row.Length - 1
                If row(count2).ContainedCharacter IsNot Nothing AndAlso row(count2).ContainedCharacter.InternalID = CharacterID Then
                    out = New Point
                    out.X = count2
                    out.Y = count
                    Exit For
                End If
            Next
        Next
        If Not out = Nothing Then
            Return out
        Else
            'no character by that guid found
            Return Nothing
        End If
    End Function

    Dim _acceptsmovementinput As Boolean = True
    Public Property AcceptsMovementInput As Boolean
        Get
            If MainCharacter IsNot Nothing Then
                Return _acceptsmovementinput
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            _acceptsmovementinput = value
        End Set
    End Property
    Public Property Bullets As New List(Of Bullet)
    Public ReadOnly Property CanMainCharAttack As Boolean
        Get
            If MainCharacter IsNot Nothing Then
                Return MainCharacter.AttackFrameWait = 0
            Else
                Return False
            End If
        End Get
    End Property
#End Region
#Region "Dialouge"
    Dim _foregroundAudioName As String
    ''' <summary>
    ''' Name of the audio file to play in the foreground.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ForegroundAudioName As String
        Get
            Return _foregroundAudioName
        End Get
        Set(value As String)
            If Not _foregroundAudioName = value Then
                _foregroundAudioName = value
                RaiseEvent ForegroundAudioChanged(value)
            End If
        End Set
    End Property
    Private Property PendingDialog As New List(Of DialogLine)
    Public Sub DisplayDialog(Dialog As String)
        PendingDialog.Add(New DialogLine(Dialog))
    End Sub
    Public Sub DisplayDialog(Dialog As String, Headshot As Bitmap)
        PendingDialog.Add(New DialogLine(Dialog, Headshot))
    End Sub
    Public Sub DisplayDialog(Dialog As String, Headshot As Bitmap, ForegroundAudioName As String)
        PendingDialog.Add(New DialogLine(Dialog, Headshot, ForegroundAudioName))
    End Sub
#End Region
#Region "Movement"
    Private Sub MoveCharacterUp(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            'ensure new tile exists
            If l.Y > 0 Then
                'ensure there is no other character
                If Tiles(l.Y - 1)(l.X).ContainedCharacter Is Nothing Then
                    'ensure tile can be entered
                    If Tiles(l.Y - 1)(l.X).CanEnterBottom Then
                        FaceCharacterUp(CharacterID)
                        'move character
                        Tiles(l.Y - 1)(l.X).ContainedCharacter = Tiles(l.Y)(l.X).ContainedCharacter
                        Tiles(l.Y - 1)(l.X).ContainedCharacterY = TileTemplate.TileSize * 1
                        Tiles(l.Y - 1)(l.X).ContainedCharacterYFinal = 0
                        Tiles(l.Y - 1)(l.X).ContainedCharacterYStep = CharacterMovementStep
                        Tiles(l.Y - 1)(l.X).ContainedCharacter.IsMoving = True
                        Tiles(l.Y)(l.X).ContainedCharacter = Nothing
                        'Tiles(l.Y)(l.X).ContainedCharacterX = 0
                        Tiles(l.Y)(l.X).ContainedCharacterY = 0
                        'Tiles(l.Y)(l.X).ContainedCharacterXFinal = 0
                        Tiles(l.Y)(l.X).ContainedCharacterYFinal = 0
                        'Tiles(l.Y)(l.X).ContainedCharacterXStep = 2
                        Tiles(l.Y)(l.X).ContainedCharacterYStep = 0
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub MoveCharacterDown(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            'ensure new tile exists
            If l.Y < Tiles.Length - 1 Then
                'ensure there isn't another character
                If Tiles(l.Y + 1)(l.X).ContainedCharacter Is Nothing Then
                    'ensure tile can be entered
                    If Tiles(l.Y + 1)(l.X).CanEnterTop Then
                        FaceCharacterDown(CharacterID)

                        'move character
                        Tiles(l.Y + 1)(l.X).ContainedCharacter = Tiles(l.Y)(l.X).ContainedCharacter
                        Tiles(l.Y + 1)(l.X).ContainedCharacterY = TileTemplate.TileSize * -1
                        Tiles(l.Y + 1)(l.X).ContainedCharacterYFinal = 0
                        Tiles(l.Y + 1)(l.X).ContainedCharacter.IsMoving = True
                        Tiles(l.Y + 1)(l.X).ContainedCharacterYStep = CharacterMovementStep
                        Tiles(l.Y)(l.X).ContainedCharacter = Nothing
                        'Tiles(l.Y)(l.X).ContainedCharacterX = 0
                        Tiles(l.Y)(l.X).ContainedCharacterY = 0
                        'Tiles(l.Y)(l.X).ContainedCharacterXFinal = 0
                        Tiles(l.Y)(l.X).ContainedCharacterYFinal = 0
                        'Tiles(l.Y)(l.X).ContainedCharacterXStep = 2
                        Tiles(l.Y)(l.X).ContainedCharacterYStep = 0
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub MoveCharacterRight(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            'ensure new tile exists
            If l.X < Tiles(l.Y).Length - 1 Then
                'ensure there isn't another character
                If Tiles(l.Y)(l.X + 1).ContainedCharacter Is Nothing Then
                    'ensure tile can be entered
                    If Tiles(l.Y)(l.X + 1).CanEnterLeft Then
                        FaceCharacterRight(CharacterID)
                        Tiles(l.Y)(l.X).ContainedCharacter.IsMoving = True
                        'move character
                        Tiles(l.Y)(l.X + 1).ContainedCharacter = Tiles(l.Y)(l.X).ContainedCharacter
                        Tiles(l.Y)(l.X + 1).ContainedCharacterX = TileTemplate.TileSize * -1
                        Tiles(l.Y)(l.X + 1).ContainedCharacterXFinal = 0
                        Tiles(l.Y)(l.X + 1).ContainedCharacterXStep = CharacterMovementStep
                        Tiles(l.Y)(l.X + 1).ContainedCharacter.IsMoving = True
                        Tiles(l.Y)(l.X).ContainedCharacter = Nothing
                        Tiles(l.Y)(l.X).ContainedCharacterX = 0
                        'Tiles(l.Y)(l.X).ContainedCharacterY = 0
                        Tiles(l.Y)(l.X).ContainedCharacterXFinal = 0
                        'Tiles(l.Y)(l.X).ContainedCharacterYFinal = 0
                        Tiles(l.Y)(l.X).ContainedCharacterXStep = 0
                        'Tiles(l.Y)(l.X).ContainedCharacterYStep = 2
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub MoveCharacterLeft(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            'ensure new tile exists
            If l.X > 0 Then
                'ensure there isn't another character
                If Tiles(l.Y)(l.X - 1).ContainedCharacter Is Nothing Then
                    'ensure tile can be entered
                    If Tiles(l.Y)(l.X - 1).CanEnterRight Then
                        FaceCharacterLeft(CharacterID)
                        'move character
                        Tiles(l.Y)(l.X - 1).ContainedCharacter = Tiles(l.Y)(l.X).ContainedCharacter
                        Tiles(l.Y)(l.X - 1).ContainedCharacterX = TileTemplate.TileSize * 1
                        Tiles(l.Y)(l.X - 1).ContainedCharacterXFinal = 0
                        Tiles(l.Y)(l.X - 1).ContainedCharacterXStep = CharacterMovementStep
                        Tiles(l.Y)(l.X - 1).ContainedCharacter.IsMoving = True
                        Tiles(l.Y)(l.X).ContainedCharacter = Nothing
                        Tiles(l.Y)(l.X).ContainedCharacterX = 0
                        'Tiles(l.Y)(l.X).ContainedCharacterY = 0
                        Tiles(l.Y)(l.X).ContainedCharacterXFinal = 0
                        'Tiles(l.Y)(l.X).ContainedCharacterYFinal = 0
                        Tiles(l.Y)(l.X).ContainedCharacterXStep = 0
                        'Tiles(l.Y)(l.X).ContainedCharacterYStep = 2
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub MoveCharacterUpRight(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            'ensure new tile exists
            If l.Y > 0 AndAlso l.X < Tiles(l.Y).Length - 1 Then
                'ensure there is no other character
                If Tiles(l.Y - 1)(l.X + 1).ContainedCharacter Is Nothing Then
                    'ensure tile can be entered
                    If Tiles(l.Y - 1)(l.X + 1).CanEnterBottom AndAlso Tiles(l.Y - 1)(l.X + 1).CanEnterLeft Then
                        FaceCharacterUpRight(CharacterID)
                        'move character
                        Tiles(l.Y - 1)(l.X + 1).ContainedCharacter = Tiles(l.Y)(l.X).ContainedCharacter
                        Tiles(l.Y - 1)(l.X + 1).ContainedCharacterY = TileTemplate.TileSize * 1
                        Tiles(l.Y - 1)(l.X + 1).ContainedCharacterYFinal = 0
                        Tiles(l.Y - 1)(l.X + 1).ContainedCharacterYStep = CharacterMovementStep
                        Tiles(l.Y - 1)(l.X + 1).ContainedCharacterX = TileTemplate.TileSize * -1
                        Tiles(l.Y - 1)(l.X + 1).ContainedCharacterXFinal = 0
                        Tiles(l.Y - 1)(l.X + 1).ContainedCharacterXStep = CharacterMovementStep
                        Tiles(l.Y - 1)(l.X + 1).ContainedCharacter.IsMoving = True
                        Tiles(l.Y)(l.X).ContainedCharacter = Nothing
                        Tiles(l.Y)(l.X).ContainedCharacterX = 0
                        Tiles(l.Y)(l.X).ContainedCharacterY = 0
                        Tiles(l.Y)(l.X).ContainedCharacterXFinal = 0
                        Tiles(l.Y)(l.X).ContainedCharacterYFinal = 0
                        Tiles(l.Y)(l.X).ContainedCharacterXStep = 0
                        Tiles(l.Y)(l.X).ContainedCharacterYStep = 0
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub MoveCharacterUpLeft(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            'ensure new tile exists
            If l.Y > 0 AndAlso l.X > 0 Then
                'ensure there is no other character
                If Tiles(l.Y - 1)(l.X - 1).ContainedCharacter Is Nothing Then
                    'ensure tile can be entered
                    If Tiles(l.Y - 1)(l.X - 1).CanEnterBottom AndAlso Tiles(l.Y - 1)(l.X - 1).CanEnterRight Then
                        FaceCharacterUpLeft(CharacterID)
                        'move character
                        Tiles(l.Y - 1)(l.X - 1).ContainedCharacter = Tiles(l.Y)(l.X).ContainedCharacter
                        Tiles(l.Y - 1)(l.X - 1).ContainedCharacterY = TileTemplate.TileSize * 1
                        Tiles(l.Y - 1)(l.X - 1).ContainedCharacterYFinal = 0
                        Tiles(l.Y - 1)(l.X - 1).ContainedCharacterYStep = CharacterMovementStep
                        Tiles(l.Y - 1)(l.X - 1).ContainedCharacterX = TileTemplate.TileSize * 1
                        Tiles(l.Y - 1)(l.X - 1).ContainedCharacterXFinal = 0
                        Tiles(l.Y - 1)(l.X - 1).ContainedCharacterXStep = CharacterMovementStep
                        Tiles(l.Y - 1)(l.X - 1).ContainedCharacter.IsMoving = True
                        Tiles(l.Y)(l.X).ContainedCharacter = Nothing
                        Tiles(l.Y)(l.X).ContainedCharacterX = 0
                        Tiles(l.Y)(l.X).ContainedCharacterY = 0
                        Tiles(l.Y)(l.X).ContainedCharacterXFinal = 0
                        Tiles(l.Y)(l.X).ContainedCharacterYFinal = 0
                        Tiles(l.Y)(l.X).ContainedCharacterXStep = 0
                        Tiles(l.Y)(l.X).ContainedCharacterYStep = 0
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub MoveCharacterDownRight(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            'ensure new tile exists
            If l.Y < Tiles.Length - 1 AndAlso l.X < Tiles(l.Y).Length - 1 Then
                'ensure there is no other character
                If Tiles(l.Y + 1)(l.X + 1).ContainedCharacter Is Nothing Then
                    'ensure tile can be entered
                    If Tiles(l.Y + 1)(l.X + 1).CanEnterTop AndAlso Tiles(l.Y + 1)(l.X + 1).CanEnterLeft Then
                        FaceCharacterDownRight(CharacterID)
                        'move character
                        Tiles(l.Y + 1)(l.X + 1).ContainedCharacter = Tiles(l.Y)(l.X).ContainedCharacter
                        Tiles(l.Y + 1)(l.X + 1).ContainedCharacterY = TileTemplate.TileSize * -1
                        Tiles(l.Y + 1)(l.X + 1).ContainedCharacterYFinal = 0
                        Tiles(l.Y + 1)(l.X + 1).ContainedCharacterYStep = CharacterMovementStep
                        Tiles(l.Y + 1)(l.X + 1).ContainedCharacterX = TileTemplate.TileSize * -1
                        Tiles(l.Y + 1)(l.X + 1).ContainedCharacterXFinal = 0
                        Tiles(l.Y + 1)(l.X + 1).ContainedCharacterXStep = CharacterMovementStep
                        Tiles(l.Y + 1)(l.X + 1).ContainedCharacter.IsMoving = True
                        Tiles(l.Y)(l.X).ContainedCharacter = Nothing
                        Tiles(l.Y)(l.X).ContainedCharacterX = 0
                        Tiles(l.Y)(l.X).ContainedCharacterY = 0
                        Tiles(l.Y)(l.X).ContainedCharacterXFinal = 0
                        Tiles(l.Y)(l.X).ContainedCharacterYFinal = 0
                        Tiles(l.Y)(l.X).ContainedCharacterXStep = 0
                        Tiles(l.Y)(l.X).ContainedCharacterYStep = 0
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub MoveCharacterDownLeft(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            'ensure new tile exists
            If l.Y < Tiles.Length - 1 AndAlso l.X > 0 Then
                'ensure there is no other character
                If Tiles(l.Y + 1)(l.X - 1).ContainedCharacter Is Nothing Then
                    'ensure tile can be entered
                    If Tiles(l.Y + 1)(l.X - 1).CanEnterTop AndAlso Tiles(l.Y + 1)(l.X - 1).CanEnterRight Then
                        FaceCharacterDownLeft(CharacterID)
                        'move character
                        Tiles(l.Y + 1)(l.X - 1).ContainedCharacter = Tiles(l.Y)(l.X).ContainedCharacter
                        Tiles(l.Y + 1)(l.X - 1).ContainedCharacterY = TileTemplate.TileSize * -1
                        Tiles(l.Y + 1)(l.X - 1).ContainedCharacterYFinal = 0
                        Tiles(l.Y + 1)(l.X - 1).ContainedCharacterYStep = CharacterMovementStep
                        Tiles(l.Y + 1)(l.X - 1).ContainedCharacterX = TileTemplate.TileSize * 1
                        Tiles(l.Y + 1)(l.X - 1).ContainedCharacterXFinal = 0
                        Tiles(l.Y + 1)(l.X - 1).ContainedCharacterXStep = CharacterMovementStep
                        Tiles(l.Y + 1)(l.X - 1).ContainedCharacter.IsMoving = True
                        Tiles(l.Y)(l.X).ContainedCharacter = Nothing
                        Tiles(l.Y)(l.X).ContainedCharacterX = 0
                        Tiles(l.Y)(l.X).ContainedCharacterY = 0
                        Tiles(l.Y)(l.X).ContainedCharacterXFinal = 0
                        Tiles(l.Y)(l.X).ContainedCharacterYFinal = 0
                        Tiles(l.Y)(l.X).ContainedCharacterXStep = 0
                        Tiles(l.Y)(l.X).ContainedCharacterYStep = 0
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub FaceCharacterUp(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.Up
        End If
    End Sub
    Private Sub FaceCharacterDown(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.Down
        End If
    End Sub
    Private Sub FaceCharacterRight(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.Right
        End If
    End Sub
    Private Sub FaceCharacterLeft(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.Left
        End If
    End Sub

    Private Sub FaceCharacterUpRight(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.UpRight
        End If
    End Sub
    Private Sub FaceCharacterUpLeft(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.UpLeft
        End If
    End Sub
    Private Sub FaceCharacterDownRight(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.DownRight
        End If
    End Sub
    Private Sub FaceCharacterDownLeft(CharacterID As Guid)
        Dim l As Point = FindCharacterLocation(CharacterID)
        'ensure character exists
        If Tiles(l.Y)(l.X).ContainedCharacter IsNot Nothing Then
            Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.DownLeft
        End If
    End Sub
#End Region
#Region "Input"
    Public Sub CheckForWarpPoint()
        If WarpPointCollection.ContainsKey(MainCharacterLocation) Then
            LoadNewMap(WarpPointCollection(MainCharacterLocation).NewMapName, WarpPointCollection(MainCharacterLocation).NewPoint)
        End If
    End Sub
    Public Sub MoveMaintCharacterUp(Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            If Not StayStill AndAlso MainCharacter.FacingDirection = Direction.Up Then
                MoveCharacterUp(MainCharacter.InternalID)
                AcceptsMovementInput = False
                CheckForWarpPoint()
            Else
                FaceCharacterUp(MainCharacter.InternalID)
            End If
        End If
    End Sub
    Public Sub MoveMaintCharacterDown(Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            If Not StayStill AndAlso MainCharacter.FacingDirection = Direction.Down Then
                MoveCharacterDown(MainCharacter.InternalID)
                AcceptsMovementInput = False
                CheckForWarpPoint()
            Else
                FaceCharacterDown(MainCharacter.InternalID)
            End If
        End If
    End Sub
    Public Sub MoveMaintCharacterRight(Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            If Not StayStill AndAlso MainCharacter.FacingDirection = Direction.Right Then
                MoveCharacterRight(MainCharacter.InternalID)
                AcceptsMovementInput = False
                CheckForWarpPoint()
            Else
                FaceCharacterRight(MainCharacter.InternalID)
            End If
        End If
    End Sub
    Public Sub MoveMaintCharacterLeft(Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            If Not StayStill AndAlso MainCharacter.FacingDirection = Direction.Left Then
                MoveCharacterLeft(MainCharacter.InternalID)
                AcceptsMovementInput = False
                CheckForWarpPoint()
            Else
                FaceCharacterLeft(MainCharacter.InternalID)
            End If
        End If
    End Sub
    Public Sub MoveMaintCharacterUpRight(Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            If Not StayStill AndAlso MainCharacter.FacingDirection = Direction.UpRight Then
                MoveCharacterUpRight(MainCharacter.InternalID)
                AcceptsMovementInput = False
                CheckForWarpPoint()
            Else
                FaceCharacterUpRight(MainCharacter.InternalID)
            End If
        End If
    End Sub
    Public Sub MoveMaintCharacterUpleft(Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            If Not StayStill AndAlso MainCharacter.FacingDirection = Direction.UpLeft Then
                MoveCharacterUpLeft(MainCharacter.InternalID)
                AcceptsMovementInput = False
                CheckForWarpPoint()
            Else
                FaceCharacterUpLeft(MainCharacter.InternalID)
            End If
        End If
    End Sub
    Public Sub MoveMaintCharacterDownRight(Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            If Not StayStill AndAlso MainCharacter.FacingDirection = Direction.DownRight Then
                MoveCharacterDownRight(MainCharacter.InternalID)
                AcceptsMovementInput = False
                CheckForWarpPoint()
            Else
                FaceCharacterDownRight(MainCharacter.InternalID)
            End If
        End If
    End Sub
    Public Sub MoveMaintCharacterDownLeft(Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            If Not StayStill AndAlso MainCharacter.FacingDirection = Direction.DownLeft Then
                MoveCharacterDownLeft(MainCharacter.InternalID)
                AcceptsMovementInput = False
                CheckForWarpPoint()
            Else
                FaceCharacterDownLeft(MainCharacter.InternalID)
            End If
        End If
    End Sub

#Region "NPC Character Movement"
    Private AcceptsMovementInputDefs As New Dictionary(Of Guid, Boolean)
    Public Function CanCharacterMove(CharacterID As Guid) As Boolean
        If AcceptsMovementInputDefs.ContainsKey(CharacterID) Then
            Return AcceptsMovementInputDefs(CharacterID)
        Else
            Return True
        End If
    End Function
    Private Sub SetCanCharacterMove(CharacterID As Guid, CanMove As Boolean)
        If AcceptsMovementInputDefs.ContainsKey(CharacterID) Then
            AcceptsMovementInputDefs(CharacterID) = CanMove
        Else
            AcceptsMovementInputDefs.Add(CharacterID, CanMove)
        End If
    End Sub
    Public Sub MoveNPCUp(CharacterID As Guid, Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            Dim l As Point = FindCharacterLocation(CharacterID)
            If Not StayStill Then 'AndAlso Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.Up Then
                MoveCharacterUp(CharacterID)
                SetCanCharacterMove(CharacterID, False)
            Else
                FaceCharacterUp(CharacterID)
            End If
        End If
    End Sub
    Public Sub MoveNPCDown(CharacterID As Guid, Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            Dim l As Point = FindCharacterLocation(CharacterID)
            If Not StayStill Then 'AndAlso Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.Down Then
                MoveCharacterDown(CharacterID)
                SetCanCharacterMove(CharacterID, False)
            Else
                FaceCharacterDown(CharacterID)
            End If
        End If
    End Sub
    Public Sub MoveNPCLeft(CharacterID As Guid, Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            Dim l As Point = FindCharacterLocation(CharacterID)
            If Not StayStill Then 'AndAlso Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.Left Then
                MoveCharacterLeft(CharacterID)
                SetCanCharacterMove(CharacterID, False)
            Else
                FaceCharacterLeft(CharacterID)
            End If
        End If
    End Sub
    Public Sub MoveNPCRight(CharacterID As Guid, Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            Dim l As Point = FindCharacterLocation(CharacterID)
            If Not StayStill Then 'AndAlso Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.Right Then
                MoveCharacterRight(CharacterID)
                SetCanCharacterMove(CharacterID, False)
            Else
                FaceCharacterRight(CharacterID)
            End If
        End If
    End Sub
    Public Sub MoveNPCUpLeft(CharacterID As Guid, Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            Dim l As Point = FindCharacterLocation(CharacterID)
            If Not StayStill Then 'AndAlso Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.UpLeft Then
                MoveCharacterUpLeft(CharacterID)
                SetCanCharacterMove(CharacterID, False)
            Else
                FaceCharacterUpLeft(CharacterID)
            End If
        End If
    End Sub
    Public Sub MoveNPCUpRight(CharacterID As Guid, Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            Dim l As Point = FindCharacterLocation(CharacterID)
            If Not StayStill Then 'AndAlso Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.UpRight Then
                MoveCharacterUpRight(CharacterID)
                SetCanCharacterMove(CharacterID, False)
            Else
                FaceCharacterUpRight(CharacterID)
            End If
        End If
    End Sub
    Public Sub MoveNPCDownLeft(CharacterID As Guid, Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            Dim l As Point = FindCharacterLocation(CharacterID)
            If Not StayStill Then 'AndAlso Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.DownLeft Then
                MoveCharacterDownLeft(CharacterID)
                SetCanCharacterMove(CharacterID, False)
            Else
                FaceCharacterDownLeft(CharacterID)
            End If
        End If
    End Sub
    Public Sub MoveNPCDownRight(CharacterID As Guid, Optional StayStill As Boolean = False)
        If PendingDialog.Count = 0 Then
            Dim l As Point = FindCharacterLocation(CharacterID)
            If Not StayStill Then 'AndAlso Tiles(l.Y)(l.X).ContainedCharacter.FacingDirection = Direction.DownRight Then
                MoveCharacterDownRight(CharacterID)
                SetCanCharacterMove(CharacterID, False)
            Else
                FaceCharacterDownRight(CharacterID)
            End If
        End If
    End Sub
#End Region

    Public Sub Interact()
        If InteractframeCount > requiredInteractWait Then
            If PendingDialog.Count > 0 Then
                'If CanProceedDialog Then
                PendingDialog.RemoveAt(0)
                InteractframeCount = 0
                'CanProceedDialog = False
                If PendingDialog.Count = 0 Then 'if that was the last dialog 
                    RaiseEvent PendingDialogComplete(Me)
                End If
                'End If
            Else
                Dim p As Point = MainCharacter.GetFacingTileLocation(MainCharacterLocation)
                If Tiles.Length > p.Y AndAlso Tiles(p.Y).Length > p.X Then
                    If Tiles(p.Y)(p.X).ContainedCharacter IsNot Nothing Then
                        Tiles(p.Y)(p.X).ContainedCharacter.AILogic.OnInteracted(Me, Tiles(p.Y)(p.X).ContainedCharacter, p)
                        InteractframeCount = 0
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub IncreaseInteract()
        If Not InteractframeCount > requiredInteractWait Then InteractframeCount += 1
    End Sub
    Public Sub Attack()
        CharacterAttack(MainCharacter.InternalID)
    End Sub
    Public Sub Reload()
        If CanMainCharAttack Then
            MainCharacter.Weapons(MainCharacter.CurrentWeaponIndex) = MainCharacter.ReloadWeapon(MainCharacter.Weapons(MainCharacter.CurrentWeaponIndex))
        End If
    End Sub
#End Region
#Region "Drawing"
    Public ReadOnly Property ViewingWindow As Rectangle
        Get
            Dim s As Integer = TileTemplate.TileSize
            If MainCharacter IsNot Nothing Then
                Return New Rectangle(((MainCharacterLocation.X - (ViewingWindowTileCountWidth / 2) + 1) * s) + Tiles(_mainCharTile.Y)(_mainCharTile.X).ContainedCharacterX, ((MainCharacterLocation.Y - (ViewingWindowTileCountHeight / 2) + 1) * s) + Tiles(_mainCharTile.Y)(_mainCharTile.X).ContainedCharacterY, ViewingWindowTileCountWidth * s, ViewingWindowTileCountHeight * s)
            Else
                Return New Rectangle(0, 0, ViewingWindowTileCountWidth * s, ViewingWindowTileCountHeight * s)
            End If
        End Get
    End Property
    Public Function PaintFullBackground() As Bitmap
        Dim out As New Bitmap(Tiles(0).Length * TileTemplate.TileSize, Tiles.Length * TileTemplate.TileSize)
        If MainCharacter Is Nothing Then Return out
        Dim g As Graphics = Graphics.FromImage(out)
        For count As Integer = 0 To Tiles.Length - 1
            Dim row As Tile() = Tiles(count)
            For count2 As Integer = 0 To row.Length - 1
                g.DrawImage(TileTemplate.GetTileImage(row(count2).TileTextureIndex), New Rectangle(count2 * TileTemplate.TileSize, count * TileTemplate.TileSize, TileTemplate.TileSize, TileTemplate.TileSize))
            Next
        Next
        Return out
    End Function
    Public Function PaintFullMap() As Bitmap
        If _background Is Nothing Then
            _background = PaintFullBackground()
        End If
        Dim out As Bitmap = _background.Clone
        Dim g As Graphics = Graphics.FromImage(out)

        'Position main character
        Dim c As Point = MainCharacterLocation
        If Tiles(c.Y)(c.X).ContainedCharacterX = Tiles(c.Y)(c.X).ContainedCharacterXFinal AndAlso Tiles(c.Y)(c.X).ContainedCharacterY = Tiles(c.Y)(c.X).ContainedCharacterYFinal Then
            AcceptsMovementInput = True
        End If


        'Draw Bullets
        For Each b In Bullets
            g.DrawImage(b.Sprite.GetImage, b.Location.X * TileTemplate.TileSize + b.SpriteX, b.Location.Y * TileTemplate.TileSize + b.SpriteY, b.Sprite.AnimationTemplate.TileSize, b.Sprite.AnimationTemplate.TileSize)
        Next
        MoveBullets()


        For Ycount As Integer = 0 To Tiles.Length - 1
            Dim row As Tile() = Tiles(Ycount)
            For xCount As Integer = 0 To row.Length - 1
                If row(xCount).ContainedCharacter IsNot Nothing Then
                    'Do character turn if not main character
                    If Not Tiles(Ycount)(xCount).ContainedCharacter.InternalID = MainCharacter.InternalID Then
                        If CanCharacterMove(Tiles(Ycount)(xCount).ContainedCharacter.InternalID) Then
                            Tiles(Ycount)(xCount).ContainedCharacter.AILogic.DoTurn(Me, Tiles(Ycount)(xCount).ContainedCharacter, New Point(xCount, Ycount))
                        Else
                        End If
                    End If
                End If
            Next
        Next
        For Ycount As Integer = 0 To Tiles.Length - 1
            Dim row As Tile() = Tiles(Ycount)
            For xCount As Integer = 0 To row.Length - 1
                If row(xCount).ContainedCharacter IsNot Nothing Then


                    Dim xFin As Boolean = False
                    Dim yFIn As Boolean = False
                    'Move character if needed
                    If Not row(xCount).ContainedCharacterX = row(xCount).ContainedCharacterXFinal Then
                        If row(xCount).ContainedCharacterX > row(xCount).ContainedCharacterXFinal Then
                            Tiles(Ycount)(xCount).ContainedCharacterX -= row(xCount).ContainedCharacterXStep
                        Else
                            Tiles(Ycount)(xCount).ContainedCharacterX += row(xCount).ContainedCharacterXStep
                        End If
                    Else
                        xFin = True
                    End If
                    If Not row(xCount).ContainedCharacterY = row(xCount).ContainedCharacterYFinal Then
                        If row(xCount).ContainedCharacterY > row(xCount).ContainedCharacterYFinal Then
                            Tiles(Ycount)(xCount).ContainedCharacterY -= row(xCount).ContainedCharacterYStep
                        Else
                            Tiles(Ycount)(xCount).ContainedCharacterY += row(xCount).ContainedCharacterYStep
                        End If
                    Else
                        yFIn = True
                    End If

                    Tiles(Ycount)(xCount).ContainedCharacter.IsMoving = Not (xFin And yFIn)
                    SetCanCharacterMove(Tiles(Ycount)(xCount).ContainedCharacter.InternalID, (xFin And yFIn))

                    If Tiles(Ycount)(xCount).ContainedCharacter.AttackFrameWait > 0 Then Tiles(Ycount)(xCount).ContainedCharacter.AttackFrameWait -= 1


                    'Draw Character
                    g.DrawImage(row(xCount).ContainedCharacter.Sprite.GetImage, New Rectangle(xCount * TileTemplate.TileSize + row(xCount).ContainedCharacterX, Ycount * TileTemplate.TileSize + row(xCount).ContainedCharacterY, TileTemplate.TileSize, TileTemplate.TileSize))
                    If row(xCount).ContainedCharacter.DamageDisplayFrameCount < row(xCount).ContainedCharacter.DamageDisplayFrameLength Then
                        g.DrawString("-" & row(xCount).ContainedCharacter.DamageDisplayInitialDamage - row(xCount).ContainedCharacter.CurrentHealth, New Font(fontFace, fontSizeDialog, FontStyle.Bold), Brushes.Red, xCount * TileTemplate.TileSize + Tiles(Ycount)(xCount).ContainedCharacterX, Ycount * TileTemplate.TileSize - (fontSizeHUD * 2) + Tiles(Ycount)(xCount).ContainedCharacterY)
                        Tiles(Ycount)(xCount).ContainedCharacter.DamageDisplayFrameCount += 1
                    End If
                End If
            Next
        Next
        Return out
    End Function
    Public Overloads Function PaintMap(Optional IncludeHUD As Boolean = True) As Bitmap
        If MainCharacter IsNot Nothing AndAlso MainCharacter.CurrentHealth > 0 Then
            If PendingDialog.Count = 0 OrElse _lastImage Is Nothing Then
                Dim out As New Bitmap(ViewingWindow.Width, ViewingWindow.Height)
                Dim map As Bitmap = PaintFullMap()
                Dim g As Graphics = Graphics.FromImage(out)
                'Dim s As Integer = TileTemplate.TileSize
                g.DrawImage(map, New Rectangle(0, 0, out.Width, out.Height), ViewingWindow, GraphicsUnit.Pixel)
                If MainCharacter Is Nothing Then GoTo Died
                If IncludeHUD AndAlso MainCharacter.Weapons.Count > 0 Then
                    g.FillRectangle(Brushes.Black, 0, 0, out.Width, CInt(fontSizeHUD * 5))
                    g.FillRectangle(Brushes.Red, CInt(fontSizeHUD * 7), 15, 100, CInt(fontSizeHUD * 1.6))
                    g.FillRectangle(Brushes.Green, CInt(fontSizeHUD * 7), 15, CInt((MainCharacter.CurrentHealth / MainCharacter.MaxHealth) * 100), CInt(fontSizeHUD * 1.6))
                    g.DrawString("Health: " & vbCrLf &
                                "Weapon: " & MainCharacter.Weapons(MainCharacter.CurrentWeaponIndex).Name & vbCrLf &
                                 "Ammo: " &
                                            MainCharacter.Weapons(MainCharacter.CurrentWeaponIndex).CurrentMagazineLoad &
                                            "/" & MainCharacter.Weapons(MainCharacter.CurrentWeaponIndex).Magazine &
                                            " of " &
                                            MainCharacter.GetHeldAmmoCount(MainCharacter.Weapons(MainCharacter.CurrentWeaponIndex).Type),
                                            New Font(fontFace, fontSizeHUD, FontStyle.Bold), Brushes.Red, 20, 20)
                    g.DrawString("                       " & MainCharacter.CurrentHealth.ToString,
                                            New Font(fontFace, fontSizeHUD, FontStyle.Bold), Brushes.Black, 20, 20)
                End If
                'g.DrawRectangle(Pens.Black, 0, 0, ViewingWindowTileCountWidth * (s + 1), ViewingWindowTileCountWidth * (s + 1))
                _lastImage = out
                IncreaseInteract()
                FirstFrameDrawnCheck()
                ForegroundAudioName = ""
                Return out
            Else
                Dim out As Bitmap = _lastImage
                Dim g As Graphics = Graphics.FromImage(out)

                g.FillRectangle(Brushes.Black, 10, out.Height - 175, out.Width - 40, 125)
                If InteractframeCount > requiredInteractWait Then
                    g.DrawString(PendingDialog(0).Line & vbCrLf & "->", New Font(fontFace, fontSizeDialog), Brushes.White, 15, out.Height - 170)
                    'CanProceedDialog = True
                Else
                    g.DrawString(PendingDialog(0).Line, New Font(fontFace, fontSizeDialog), Brushes.White, 15, out.Height - 170)
                End If
                If PendingDialog(0).Headshot IsNot Nothing Then
                    g.FillRectangle(Brushes.Black, 10, (out.Height - 175) - 15 - PendingDialog(0).Headshot.Height, PendingDialog(0).Headshot.Width + 10, PendingDialog(0).Headshot.Height + 10)
                    g.DrawImage(PendingDialog(0).Headshot, 15, (out.Height - 175) - 10 - PendingDialog(0).Headshot.Height)
                End If
                IncreaseInteract()
                ForegroundAudioName = PendingDialog(0).ForegroundAudioName
                FirstFrameDrawnCheck()
                Return out
            End If
        Else
Died:       Dim out As New Bitmap(ViewingWindow.Width, ViewingWindow.Height)
            Dim g As Graphics = Graphics.FromImage(out)

            g.FillRectangle(Brushes.Black, 10, out.Height - 175, out.Width - 40, 125)
            g.DrawString("You have died.", New Font(fontFace, fontSizeDialog), Brushes.White, 15, out.Height - 170)
            Return out
        End If
    End Function
    ''' <summary>
    ''' Check to see if the FirstFrameDrawn event has fired yet.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FirstFrameDrawnCheck()
        If Not _firstFrameDrawn Then
            _firstFrameDrawn = True
            RaiseEvent FirstFrameDrawn()
        End If
    End Sub
#End Region
#Region "Bullets"
    Public Sub DetectBulletCollision()
        For count As Integer = Bullets.Count - 1 To 0 Step -1
            Dim b As Bullet = Bullets(count)
            If b IsNot Nothing Then
                If Tiles(b.Location.Y)(b.Location.X).ContainedCharacter IsNot Nothing Then
                    Tiles(b.Location.Y)(b.Location.X).ContainedCharacter.TakeDamage(b.Damage)
                    If Tiles(b.Location.Y)(b.Location.X).ContainedCharacter.CurrentHealth <= 0 Then
                        If Not Tiles(b.Location.Y)(b.Location.X).ContainedCharacter.InternalID = MainCharacter.InternalID Then
                            MainCharacter.RecieveExp(Tiles(b.Location.Y)(b.Location.X).ContainedCharacter.ExpReleasedWhenDefeated, b.FiringWeapon)
                            RaiseEvent EnemyDefeated(Tiles(b.Location.Y)(b.Location.X).ContainedCharacter)
                            Tiles(b.Location.Y)(b.Location.X).ContainedCharacter = Nothing
                        Else
                            RaiseEvent MainCharacterDefeated()
                            Tiles(b.Location.Y)(b.Location.X).ContainedCharacter = Nothing
                        End If
                    End If
                    Bullets.RemoveAt(count)
                End If
            End If
        Next
    End Sub
    Public Sub MoveBullets()
        DetectBulletCollision() 'For character Movement

        For count As Integer = Bullets.Count - 1 To 0 Step -1
            Dim removed As Boolean = False
            'Up
            If Not removed Then
                If Bullets(count).FacingDirection = Direction.Up OrElse Bullets(count).FacingDirection = Direction.UpRight OrElse Bullets(count).FacingDirection = Direction.UpLeft Then
                    If Bullets(count).SpriteY = Bullets(count).SpriteYFinal Then
                        If Bullets(count).Location.Y > 0 Then
                            If Tiles(Bullets(count).Location.Y - 1)(Bullets(count).Location.X).CanEnterTop Then
                                Bullets(count).Location = New Point(Bullets(count).Location.X, Bullets(count).Location.Y - 1)
                                Bullets(count).SpriteY = TileTemplate.TileSize
                                Bullets(count).TilesTraveled += 1
                            Else
                                Bullets.RemoveAt(count)
                                removed = True
                            End If
                        Else
                            Bullets.RemoveAt(count)
                            removed = True
                        End If
                    Else
                        Bullets(count).SpriteY -= Bullets(count).SpriteYStep
                    End If
                End If
            End If
            'Down
            If Not removed Then
                If Bullets(count).FacingDirection = Direction.Down OrElse Bullets(count).FacingDirection = Direction.DownRight OrElse Bullets(count).FacingDirection = Direction.DownLeft Then
                    If Bullets(count).SpriteY = Bullets(count).SpriteYFinal Then
                        If Bullets(count).Location.Y < Tiles.Length - 1 Then
                            If Tiles(Bullets(count).Location.Y + 1)(Bullets(count).Location.X).CanEnterBottom Then
                                Bullets(count).Location = New Point(Bullets(count).Location.X, Bullets(count).Location.Y + 1)
                                Bullets(count).SpriteY = TileTemplate.TileSize * -1
                                Bullets(count).TilesTraveled += 1
                            Else
                                Bullets.RemoveAt(count)
                                removed = True
                            End If
                        Else
                            Bullets.RemoveAt(count)
                            removed = True
                        End If
                    Else
                        Bullets(count).SpriteY += Bullets(count).SpriteYStep
                    End If
                End If
            End If
            'Left
            If Not removed Then
                If Bullets(count).FacingDirection = Direction.Left OrElse Bullets(count).FacingDirection = Direction.DownLeft OrElse Bullets(count).FacingDirection = Direction.UpLeft Then
                    If Bullets(count).SpriteX = Bullets(count).SpriteXFinal Then
                        If Bullets(count).Location.X > 0 Then
                            If Tiles(Bullets(count).Location.Y)(Bullets(count).Location.X - 1).CanEnterRight Then
                                Bullets(count).Location = New Point(Bullets(count).Location.X - 1, Bullets(count).Location.Y)
                                Bullets(count).SpriteX = TileTemplate.TileSize
                                Bullets(count).TilesTraveled += 1
                            Else
                                Bullets.RemoveAt(count)
                                removed = True
                            End If
                        Else
                            Bullets.RemoveAt(count)
                            removed = True
                        End If

                    Else
                        Bullets(count).SpriteX -= Bullets(count).SpriteXStep
                    End If
                End If
            End If
            'Right
            If Not removed Then
                If Bullets(count).FacingDirection = Direction.Right OrElse Bullets(count).FacingDirection = Direction.UpRight OrElse Bullets(count).FacingDirection = Direction.DownRight Then
                    If Bullets(count).SpriteX = Bullets(count).SpriteXFinal Then
                        If Bullets(count).Location.X < Tiles(Bullets(count).Location.Y).Length - 1 Then
                            If Tiles(Bullets(count).Location.Y)(Bullets(count).Location.X + 1).CanEnterLeft Then
                                Bullets(count).Location = New Point(Bullets(count).Location.X + 1, Bullets(count).Location.Y)
                                Bullets(count).SpriteX = TileTemplate.TileSize * -1
                                Bullets(count).TilesTraveled += 1
                            Else
                                Bullets.RemoveAt(count)
                                removed = True
                            End If
                        Else
                            Bullets.RemoveAt(count)
                            removed = True
                        End If
                    Else
                        Bullets(count).SpriteX += Bullets(count).SpriteXStep
                    End If
                End If
            End If
            If Not removed Then
                If Bullets(count).TilesTraveled >= Bullets(count).FiringWeapon.Range Then
                    Bullets.RemoveAt(count)
                    removed = True
                End If
            End If
        Next

        DetectBulletCollision() 'For bullet Movement
    End Sub
    Public Sub CharacterAttack(AttackingCharacter As Guid)
        Dim cl As Point = FindCharacterLocation(AttackingCharacter)
        Dim l As Point = Tiles(cl.Y)(cl.X).ContainedCharacter.GetFacingTileLocation(cl)
        If Tiles(cl.Y)(cl.X).ContainedCharacter.CanAttack Then
            If Tiles.Length > l.Y AndAlso l.Y > 0 Then
                If Tiles(l.Y).Length > l.X AndAlso l.X > 0 Then
                    Dim canFire As Boolean = False
                    Select Case Tiles(cl.Y)(cl.X).ContainedCharacter.FacingDirection
                        Case Direction.Up
                            canFire = Tiles(l.Y)(l.X).CanEnterBottom
                        Case Direction.Down
                            canFire = Tiles(l.Y)(l.X).CanEnterTop
                        Case Direction.Left
                            canFire = Tiles(l.Y)(l.X).CanEnterRight
                        Case Direction.Right
                            canFire = Tiles(l.Y)(l.X).CanEnterLeft
                        Case Direction.UpLeft
                            canFire = Tiles(l.Y)(l.X).CanEnterBottom AndAlso Tiles(l.Y)(l.X).CanEnterRight
                        Case Direction.UpRight
                            canFire = Tiles(l.Y)(l.X).CanEnterBottom AndAlso Tiles(l.Y)(l.X).CanEnterLeft
                        Case Direction.DownLeft
                            canFire = Tiles(l.Y)(l.X).CanEnterTop AndAlso Tiles(l.Y)(l.X).CanEnterRight
                        Case Direction.DownRight
                            canFire = Tiles(l.Y)(l.X).CanEnterTop AndAlso Tiles(l.Y)(l.X).CanEnterLeft
                    End Select
                    If canFire AndAlso Tiles(cl.Y)(cl.X).ContainedCharacter.Weapons.Count > 0 Then
                        Dim b As New Bullet
                        b.FiringWeapon = Tiles(cl.Y)(cl.X).ContainedCharacter.Weapons(Tiles(cl.Y)(cl.X).ContainedCharacter.CurrentWeaponIndex)
                        b.Damage = Tiles(cl.Y)(cl.X).ContainedCharacter.CalculateDamage(Tiles(cl.Y)(cl.X).ContainedCharacter.Weapons(Tiles(cl.Y)(cl.X).ContainedCharacter.CurrentWeaponIndex))
                        b.FacingDirection = Tiles(cl.Y)(cl.X).ContainedCharacter.FacingDirection
                        b.Location = l
                        Tiles(cl.Y)(cl.X).ContainedCharacter.AttackFrameWait = Tiles(cl.Y)(cl.X).ContainedCharacter.Weapons(Tiles(cl.Y)(cl.X).ContainedCharacter.CurrentWeaponIndex).FrameWait
                        Bullets.Add(b)
                    End If
                End If
            End If
        End If
    End Sub
#End Region
#Region "Saving"
    Public Function ToBinary() As Byte()
        Dim overallParts As New List(Of Byte())

        'Tiles
        Dim TileParts As New List(Of Byte())
        For Each row As Tile() In Tiles
            Dim rowTiles As New List(Of Byte())
            For Each tile In row
                rowTiles.Add(tile.ToBinary)
            Next
            TileParts.Add(FileFormatLib.FileFormatLib.ToBinary(rowTiles))
        Next
        overallParts.Add(FileFormatLib.FileFormatLib.ToBinary(TileParts))

        'TileTemplate
        Dim templateStream As New IO.MemoryStream
        TileTemplate.Template.Save(templateStream, Imaging.ImageFormat.Bmp)
        overallParts.Add(templateStream.ToArray)

        overallParts.Add(System.Text.UnicodeEncoding.Unicode.GetBytes(BGMName))

        'WarpPoints
        Dim warpOut As New FileFormatLib.SaveableStringDictionary
        For Each p As Point In WarpPointCollection.Keys
            Dim pointString As String = p.X & "," & p.Y
            If Not warpOut.ContainsKey(pointString) Then
                warpOut.Add(pointString, WarpPointCollection(p).ToString)
            End If
        Next
        overallParts.Add(warpOut.ToBinary)

        Return FileFormatLib.FileFormatLib.ToBinary(overallParts)
    End Function
    Public Shared Function FromBinary(Binary() As Byte) As Map
        Dim OverallParts As List(Of Byte()) = FileFormatLib.FileFormatLib.FromBinary(Binary)
        Dim out As New Map

        Dim TileParts As List(Of Byte()) = FileFormatLib.FileFormatLib.FromBinary(OverallParts(0))
        Dim rowsOut As Tile()() = {}
        For Each rowRaw As Byte() In TileParts
            Dim rowTiles As New List(Of Tile)
            For Each _Tile In FileFormatLib.FileFormatLib.FromBinary(rowRaw)
                rowTiles.Add(Tile.FromBinary(_Tile))
            Next
            Array.Resize(rowsOut, rowsOut.Length + 1)
            rowsOut(rowsOut.Length - 1) = rowTiles.ToArray
        Next
        out.Tiles = rowsOut

        Dim buffer As Byte() = OverallParts(1)
        Dim templateStream As New IO.MemoryStream(buffer)
        out.TileTemplate = New TileTemplate(Bitmap.FromStream(templateStream))

        out.BGMName = System.Text.UnicodeEncoding.Unicode.GetString(OverallParts(2))

        Dim warpPoints As FileFormatLib.SaveableStringDictionary = FileFormatLib.SaveableStringDictionary.FromBinary(OverallParts(3))
        For Each p In warpPoints.Keys
            Dim pointActual As New Point(p.Split(",")(0), p.Split(",")(1))
            If Not out.WarpPointCollection.ContainsKey(pointActual) Then
                out.WarpPointCollection.Add(pointActual, WarpPoint.FromString(warpPoints(p)))
            End If
        Next

        Return out
    End Function
#End Region

End Class
Public Class Tile
#Region "Savable Properties"
    Public Property TileTextureIndex As Integer
    Public Property CanEnterTop As Boolean = True
    Public Property CanEnterBottom As Boolean = True
    Public Property CanEnterLeft As Boolean = True
    Public Property CanEnterRight As Boolean = True
#End Region
#Region "Temporary Properties"
    Dim _containedCharacter As Character
    Public Property ContainedCharacter As Character
        Get
            Return _containedCharacter
        End Get
        Set(ByVal value As Character)
            _containedCharacter = value
        End Set
    End Property
    Public Property ContainedCharacterX As Integer
    Public Property ContainedCharacterY As Integer
    Public Property ContainedCharacterXFinal As Integer
    Public Property ContainedCharacterYFinal As Integer
    Public Property ContainedCharacterXStep As Integer
    Public Property ContainedCharacterYStep As Integer
#End Region
#Region "Saving"
    Public Function ToBinary() As Byte()
        Dim parts As New List(Of Byte())
        parts.Add(BitConverter.GetBytes(TileTextureIndex))
        parts.Add(BitConverter.GetBytes(CanEnterBottom))
        parts.Add(BitConverter.GetBytes(CanEnterTop))
        parts.Add(BitConverter.GetBytes(CanEnterLeft))
        parts.Add(BitConverter.GetBytes(CanEnterRight))
        If ContainedCharacter IsNot Nothing Then parts.Add(ContainedCharacter.ToBinary)
        Return FileFormatLib.FileFormatLib.ToBinary(parts)
    End Function
    Public Shared Function FromBinary(Binary() As Byte) As Tile
        Dim parts As List(Of Byte()) = FileFormatLib.FileFormatLib.FromBinary(Binary)
        Dim out As New Tile
        out.TileTextureIndex = BitConverter.ToInt32(parts(0), 0)
        out.CanEnterBottom = BitConverter.ToBoolean(parts(1), 0)
        out.CanEnterTop = BitConverter.ToBoolean(parts(2), 0)
        out.CanEnterLeft = BitConverter.ToBoolean(parts(3), 0)
        out.CanEnterRight = BitConverter.ToBoolean(parts(4), 0)
        If parts.Count > 5 Then out.ContainedCharacter = Character.FromBinary(parts(5))
        Return out
    End Function
#End Region
#Region "Constructors"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(_TileIndex As Integer)
        MyBase.New()
        TileTextureIndex = _TileIndex
    End Sub
#End Region
End Class
Public Class TileTemplate
    Public ReadOnly Property TileSize As Integer
        Get
            Return Template.Height
        End Get
    End Property
    Dim _template As Bitmap
    Public Property Template As Bitmap
        Get
            Return _template
        End Get
        Set(value As Bitmap)
            _template = value
        End Set
    End Property
    Default Public ReadOnly Property TileImage(Index) As Bitmap
        Get
            Return GetTileImage(Index)
        End Get
    End Property
    Public Function GetTileImage(ImageIndex As Integer) As Bitmap
        Dim out As New Bitmap(TileSize, TileSize)
        Dim g As Graphics = Graphics.FromImage(out)
        g.DrawImage(Template, New Rectangle(0, 0, TileSize, TileSize), New Rectangle(ImageIndex * TileSize, 0, TileSize, TileSize), GraphicsUnit.Pixel)
        Return out
    End Function
    Public ReadOnly Property TileCount As Integer
        Get
            Return (Template.Width / TileSize)
        End Get
    End Property
    Public Sub New(Template As Bitmap)
        _template = Template
    End Sub
#Region "Saving"
    Public Function ToBinary() As Byte()
        Using templateStream As New IO.MemoryStream
            Template.Save(templateStream, Imaging.ImageFormat.Png)
            Return templateStream.ToArray
        End Using
    End Function
    Public Shared Function FromBinary(Binary As Byte()) As TileTemplate
        Dim buffer As Byte() = Binary
        Using templateStream As New IO.MemoryStream(buffer)
            Return New TileTemplate(Bitmap.FromStream(templateStream))
        End Using
    End Function
#End Region
End Class
Public Class AnimatedSprite

    Public Property AnimationTemplate As TileTemplate
    Public Function GetImage() As Bitmap
        Dim outImage As Bitmap
        Static slowdownCounter As Integer = 0
        Static AnimationIndex As Integer = 0
        If slowdownCounter = 10 Then
            AnimationIndex += 1
            If AnimationIndex >= AnimationTemplate.TileCount Then
                AnimationIndex = 0
            End If
        ElseIf slowdownCounter > 10 Then
            slowdownCounter = 0
        End If
        outImage = AnimationTemplate.GetTileImage(AnimationIndex)
        slowdownCounter += 1
        Return outImage
    End Function
    Public Sub New(Template As Bitmap)
        MyBase.New()
        AnimationTemplate = New TileTemplate(Template)
    End Sub
    Public Sub New(Template As TileTemplate)
        MyBase.New()
        AnimationTemplate = Template
    End Sub
#Region "Saving"
    Public Function ToBinary() As Byte()
        Return AnimationTemplate.ToBinary
    End Function
    Public Shared Function FromBinary(Binary() As Byte) As AnimatedSprite
        Return New AnimatedSprite(TileTemplate.FromBinary(Binary))
    End Function
#End Region
End Class
Public Class DialogLine
    Public Property Line As String
    Public Property Headshot As Bitmap
    Public Property ForegroundAudioName As String
    Public Sub New(Text As String)
        Line = Text
    End Sub
    Public Sub New(Text As String, Image As Bitmap)
        Line = Text
        Headshot = Image
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Text">Text to display.</param>
    ''' <param name="Image">Headshot of the character talking.</param>
    ''' <param name="AudioName">Foreground audio to play.</param>
    ''' <remarks></remarks>
    Public Sub New(Text As String, Image As Bitmap, AudioName As String)
        Line = Text
        Headshot = Image
        ForegroundAudioName = AudioName
    End Sub
End Class