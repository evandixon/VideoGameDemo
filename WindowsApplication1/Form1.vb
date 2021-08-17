Imports Logic
Imports System.Runtime.InteropServices
Imports System.Text

Public Class Form1
    WithEvents m As New Logic.Map()
    Const DataDir = "data"
#Region "KeyMapping"
    Dim keys As New Dictionary(Of Windows.Forms.Keys, Boolean)

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If _bgmProc IsNot Nothing Then
            _bgmProc.Close()
            _bgmProc.Dispose()
        End If
    End Sub
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        SetKeyDicVal(e.KeyCode, True)
        If e.KeyCode = Windows.Forms.Keys.Escape Then
            Me.Close()
        End If
    End Sub
    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        SetKeyDicVal(e.KeyCode, False)
    End Sub
    Sub SetKeyDicVal(KeyCode As Keys, Value As Boolean)
        If keys.ContainsKey(KeyCode) Then
            keys(KeyCode) = Value
        Else
            keys.Add(KeyCode, Value)
        End If
    End Sub
    Function GetKeyDicVal(KeyCode As Keys) As Boolean
        If keys.ContainsKey(KeyCode) Then
            Return keys(KeyCode)
        Else
            Return False
        End If
    End Function
#End Region
#Region "MapMaking"
#Region "Tiles"
    Private Function t0() As Tile
        Dim out As New Tile(0)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
    Private Function t1() As Tile
        Return New Tile(1)
    End Function
    Private Function t2() As Tile
        Dim out As New Tile(2)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
    Private Function t3() As Tile
        Dim out As New Tile(3)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
    Private Function t4() As Tile
        Dim out As New Tile(4)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
    Private Function t5() As Tile
        Dim out As New Tile(5)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
    Private Function t6() As Tile
        Dim out As New Tile(6)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
    Private Function t7() As Tile
        Dim out As New Tile(7)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
    Private Function t8() As Tile
        Dim out As New Tile(8)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
    Private Function t9() As Tile
        Dim out As New Tile(9)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
    Private Function tA() As Tile
        Dim out As New Tile(10)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
    Private Function tB() As Tile
        Dim out As New Tile(11)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
    Private Function tC() As Tile
        Dim out As New Tile(12)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
    Private Function tD() As Tile
        Dim out As New Tile(13)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
    Private Function tE() As Tile
        Dim out As New Tile(14)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
    Private Function tF() As Tile
        Dim out As New Tile(15)
        out.CanEnterBottom = False
        out.CanEnterTop = False
        out.CanEnterLeft = False
        out.CanEnterRight = False
        Return out
    End Function
#End Region
    Private Function Box_CharacterTemplate() As Character
        Dim out As New Character
        Dim b As New Bitmap(DataDir & "\box.png")
        out.SpriteDown = New AnimatedSprite(b)
        out.SpriteUp = New AnimatedSprite(b)
        out.SpriteLeft = New AnimatedSprite(b)
        out.SpriteRight = New AnimatedSprite(b)
        out.SpriteDownLeft = New AnimatedSprite(b)
        out.SpriteDownRight = New AnimatedSprite(b)
        out.SpriteUpLeft = New AnimatedSprite(b)
        out.SpriteUpRight = New AnimatedSprite(b)
        out.SpriteDownMoving = New AnimatedSprite(b)
        out.SpriteUpMoving = New AnimatedSprite(b)
        out.SpriteLeftMoving = New AnimatedSprite(b)
        out.SpriteRightMoving = New AnimatedSprite(b)
        out.SpriteDownLeftMoving = New AnimatedSprite(b)
        out.SpriteDownRightMoving = New AnimatedSprite(b)
        out.SpriteUpLeftMoving = New AnimatedSprite(b)
        out.SpriteUpRightMoving = New AnimatedSprite(b)
        out.HeadShot() = New TileTemplate(b)
        out.BaseMaxHealth = 1
        out.CurrentHealth = 0
        'out.AIName = "Wanderer"
        out.InternalID = Guid.NewGuid
        IO.File.WriteAllBytes(IO.Path.Combine(Environment.CurrentDirectory, "Characters\box.char"), out.ToBinary)
        Return out
    End Function
    Private Function Box2_CharacterTemplate() As Character
        Dim out As New Character
        Dim b As New Bitmap(DataDir & "\box.png")
        out.SpriteDown = New AnimatedSprite(b)
        out.SpriteUp = New AnimatedSprite(b)
        out.SpriteLeft = New AnimatedSprite(b)
        out.SpriteRight = New AnimatedSprite(b)
        out.SpriteDownLeft = New AnimatedSprite(b)
        out.SpriteDownRight = New AnimatedSprite(b)
        out.SpriteUpLeft = New AnimatedSprite(b)
        out.SpriteUpRight = New AnimatedSprite(b)
        out.SpriteDownMoving = New AnimatedSprite(b)
        out.SpriteUpMoving = New AnimatedSprite(b)
        out.SpriteLeftMoving = New AnimatedSprite(b)
        out.SpriteRightMoving = New AnimatedSprite(b)
        out.SpriteDownLeftMoving = New AnimatedSprite(b)
        out.SpriteDownRightMoving = New AnimatedSprite(b)
        out.SpriteUpLeftMoving = New AnimatedSprite(b)
        out.SpriteUpRightMoving = New AnimatedSprite(b)
        out.HeadShot() = New TileTemplate(b)
        out.BaseMaxHealth = 5
        out.CurrentHealth = 5
        'out.AIName = "DeathTrap" '"Wanderer"
        out.InternalID = Guid.NewGuid
        IO.File.WriteAllBytes(IO.Path.Combine(Environment.CurrentDirectory, "Characters\box2.char"), out.ToBinary)
        Return out
    End Function
    Private Function BoxTable_CharacterTemplate() As Character
        Dim out As New Character
        Dim b As New Bitmap(DataDir & "\TableWithBoxCutter.png")
        out.SpriteDown = New AnimatedSprite(b)
        out.SpriteUp = New AnimatedSprite(b)
        out.SpriteLeft = New AnimatedSprite(b)
        out.SpriteRight = New AnimatedSprite(b)
        out.SpriteDownLeft = New AnimatedSprite(b)
        out.SpriteDownRight = New AnimatedSprite(b)
        out.SpriteUpLeft = New AnimatedSprite(b)
        out.SpriteUpRight = New AnimatedSprite(b)
        out.SpriteDownMoving = New AnimatedSprite(b)
        out.SpriteUpMoving = New AnimatedSprite(b)
        out.SpriteLeftMoving = New AnimatedSprite(b)
        out.SpriteRightMoving = New AnimatedSprite(b)
        out.SpriteDownLeftMoving = New AnimatedSprite(b)
        out.SpriteDownRightMoving = New AnimatedSprite(b)
        out.SpriteUpLeftMoving = New AnimatedSprite(b)
        out.SpriteUpRightMoving = New AnimatedSprite(b)
        out.HeadShot() = New TileTemplate(b)
        out.BaseMaxHealth = 9001
        out.CurrentHealth = 9001
        out.InternalID = Guid.NewGuid
        IO.File.WriteAllBytes(IO.Path.Combine(Environment.CurrentDirectory, "Characters\boxTable.char"), out.ToBinary)
        Return out
    End Function
    Private Function SC_CharacterTemplate() As Character
        Dim out As New Character
        Dim b As New Bitmap(48, 48)
        out.SpriteDown = New AnimatedSprite(b)
        out.SpriteUp = New AnimatedSprite(b)
        out.SpriteLeft = New AnimatedSprite(b)
        out.SpriteRight = New AnimatedSprite(b)
        out.SpriteDownLeft = New AnimatedSprite(b)
        out.SpriteDownRight = New AnimatedSprite(b)
        out.SpriteUpLeft = New AnimatedSprite(b)
        out.SpriteUpRight = New AnimatedSprite(b)
        out.SpriteDownMoving = New AnimatedSprite(b)
        out.SpriteUpMoving = New AnimatedSprite(b)
        out.SpriteLeftMoving = New AnimatedSprite(b)
        out.SpriteRightMoving = New AnimatedSprite(b)
        out.SpriteDownLeftMoving = New AnimatedSprite(b)
        out.SpriteDownRightMoving = New AnimatedSprite(b)
        out.SpriteUpLeftMoving = New AnimatedSprite(b)
        out.SpriteUpRightMoving = New AnimatedSprite(b)
        out.HeadShot() = New TileTemplate(b)
        out.InternalID = Guid.NewGuid
        Return out
    End Function
#Region "Maps"
    Private Function GetMap_Mercuh_Training1() As Map
        Dim out As New Map
        out.TileTemplate = New Logic.TileTemplate(System.Drawing.Bitmap.FromFile(DataDir & "\Mercuh\template.png"))
        out.Tiles = {DirectCast({t6(), t5(), t5(), t5(), t7(), t0(), t0(), t0()}, Logic.Tile()),
                     DirectCast({t8(), t1(), t1(), t1(), t9(), t5(), t5(), t5()}, Logic.Tile()),
                     DirectCast({t8(), t1(), t1(), t1(), t1(), t1(), t1(), t1()}, Logic.Tile()),
                     DirectCast({t8(), t1(), t1(), t1(), tE(), tD(), tD(), tD()}, Logic.Tile()),
                     DirectCast({tB(), tD(), tD(), tD(), tC(), t0(), t0(), t0()}, Logic.Tile())}
        out.WarpPointCollection.Add(New Point(5, 2), New WarpPoint("Mercuh_Training2", New Point(1, 2)))
        out.Tiles(0)(0).ContainedCharacter = SC_CharacterTemplate()
        out.Tiles(0)(0).ContainedCharacter.AIName = "Mercuh_Training1_SC"
        IO.File.WriteAllBytes(IO.Path.Combine(Environment.CurrentDirectory, "Maps", "mercuh_training1.map"), out.ToBinary)
        Return out
    End Function
    Private Function GetMap_Mercuh_Training2() As Map
        Dim out As New Map
        out.TileTemplate = New Logic.TileTemplate(System.Drawing.Bitmap.FromFile(DataDir & "\Mercuh\template.png"))
        out.Tiles = {DirectCast({t0(), t0(), t6(), t5(), t5(), t5(), t5(), t5(), t7(), t0()}, Logic.Tile()),
                     DirectCast({t5(), t5(), tA(), t1(), t1(), t1(), t1(), t1(), t9(), t5()}, Logic.Tile()),
                     DirectCast({t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1()}, Logic.Tile()),
                     DirectCast({tD(), tD(), tF(), t1(), t1(), t1(), t1(), t1(), tE(), tD()}, Logic.Tile()),
                     DirectCast({t0(), t0(), t8(), t1(), t1(), t1(), t1(), t1(), t3(), t0()}, Logic.Tile()),
                     DirectCast({t0(), t0(), t8(), t1(), t1(), t1(), t1(), t1(), t3(), t0()}, Logic.Tile()),
                     DirectCast({t0(), t0(), t8(), t1(), t1(), t1(), t1(), t1(), t3(), t0()}, Logic.Tile()),
                     DirectCast({t0(), t0(), tB(), tD(), tD(), tD(), tD(), tD(), tC(), t0()}, Logic.Tile())}
        out.Tiles(0)(0).ContainedCharacter = SC_CharacterTemplate()
        out.Tiles(0)(0).ContainedCharacter.AIName = "Mercuh_Training2_SC"
        out.Tiles(2)(4).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(3)(3).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(3)(4).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(1)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(2)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(3)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(4)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(5)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(5)(5).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(5)(4).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(1)(7).ContainedCharacter = Box_CharacterTemplate()
        out.WarpPointCollection.Add(New Point(0, 2), New WarpPoint("Mercuh_Training1", New Point(4, 2)))
        out.WarpPointCollection.Add(New Point(9, 2), New WarpPoint("Mercuh_Training3", New Point(1, 2)))
        IO.File.WriteAllBytes(IO.Path.Combine(Environment.CurrentDirectory, "Maps", "mercuh_training2.map"), out.ToBinary)
        Return out
    End Function
    Private Function GetMap_Mercuh_Training3() As Map
        Dim out As New Map
        out.TileTemplate = New Logic.TileTemplate(System.Drawing.Bitmap.FromFile(DataDir & "\Mercuh\template.png"))
        out.Tiles = {DirectCast({t0(), t0(), t6(), t5(), t5(), t5(), t5(), t5(), t7(), t0()}, Logic.Tile()),
                     DirectCast({t5(), t5(), tA(), t1(), t1(), t1(), t1(), t1(), t9(), t5()}, Logic.Tile()),
                     DirectCast({t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1()}, Logic.Tile()),
                     DirectCast({tD(), tD(), tF(), t1(), t1(), t1(), t1(), t1(), tE(), tD()}, Logic.Tile()),
                     DirectCast({t0(), t0(), t8(), t1(), t1(), t1(), t1(), t1(), t3(), t0()}, Logic.Tile()),
                     DirectCast({t0(), t0(), t8(), t1(), t1(), t1(), t1(), t1(), t3(), t0()}, Logic.Tile()),
                     DirectCast({t0(), t0(), t8(), t1(), t1(), t1(), t1(), t1(), t3(), t0()}, Logic.Tile()),
                     DirectCast({t0(), t0(), tB(), tD(), tD(), tD(), tD(), tD(), tC(), t0()}, Logic.Tile())}
        out.Tiles(0)(0).ContainedCharacter = SC_CharacterTemplate()
        out.Tiles(0)(0).ContainedCharacter.AIName = "Mercuh_Training3_SC"
        out.Tiles(1)(3).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(4)(3).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(6)(3).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(1)(5).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(2)(4).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(3)(3).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(3)(4).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(1)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(2)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(3)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(4)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(5)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(5)(5).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(5)(4).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(1)(7).ContainedCharacter = Box_CharacterTemplate()
        out.WarpPointCollection.Add(New Point(0, 2), New WarpPoint("Mercuh_Training2", New Point(8, 2)))
        out.WarpPointCollection.Add(New Point(9, 2), New WarpPoint("Mercuh_Training4", New Point(1, 2)))
        IO.File.WriteAllBytes(IO.Path.Combine(Environment.CurrentDirectory, "Maps", "mercuh_training3.map"), out.ToBinary)
        Return out
    End Function
    Private Function GetMap_Mercuh_Training4() As Map
        Dim out As New Map
        out.TileTemplate = New Logic.TileTemplate(System.Drawing.Bitmap.FromFile(DataDir & "\Mercuh\template.png"))
        out.Tiles = {DirectCast({t0(), t0(), t6(), t5(), t5(), t5(), t5(), t5(), t7(), t0()}, Logic.Tile()),
                     DirectCast({t5(), t5(), tA(), t1(), t1(), t1(), t1(), t1(), t9(), t5()}, Logic.Tile()),
                     DirectCast({t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1()}, Logic.Tile()),
                     DirectCast({tD(), tD(), tF(), t1(), t1(), t1(), t1(), t1(), tE(), tD()}, Logic.Tile()),
                     DirectCast({t0(), t0(), t8(), t1(), t1(), t1(), t1(), t1(), t3(), t0()}, Logic.Tile()),
                     DirectCast({t0(), t0(), t8(), t1(), t1(), t1(), t1(), t1(), t3(), t0()}, Logic.Tile()),
                     DirectCast({t0(), t0(), t8(), t1(), t1(), t1(), t1(), t1(), t3(), t0()}, Logic.Tile()),
                     DirectCast({t0(), t0(), tB(), tD(), tD(), tD(), tD(), tD(), tC(), t0()}, Logic.Tile())}
        out.Tiles(0)(0).ContainedCharacter = SC_CharacterTemplate()
        out.Tiles(0)(0).ContainedCharacter.AIName = "Mercuh_Training4_SC"
        out.Tiles(1)(7).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(2)(7).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(3)(7).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(4)(7).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(5)(7).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(6)(7).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(1)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(2)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(3)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(4)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(5)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(6)(6).ContainedCharacter = Box_CharacterTemplate()
        out.Tiles(1)(5).ContainedCharacter = BoxTable_CharacterTemplate()
        out.Tiles(1)(5).ContainedCharacter.AIName = "Mercuh_Training4_Table"
        out.WarpPointCollection.Add(New Point(0, 2), New WarpPoint("Mercuh_Training3", New Point(8, 2)))
        out.WarpPointCollection.Add(New Point(9, 2), New WarpPoint("Mercuh_Training5", New Point(1, 2)))
        IO.File.WriteAllBytes(IO.Path.Combine(Environment.CurrentDirectory, "Maps", "mercuh_training4.map"), out.ToBinary)
        Return out
    End Function
    Private Function GetMap_Mercuh_Training5() As Map
        Dim out As New Map
        out.TileTemplate = New Logic.TileTemplate(System.Drawing.Bitmap.FromFile(DataDir & "\Mercuh\template.png"))
        out.Tiles = {DirectCast({t0(), t0(), t6(), t5(), t5(), t5(), t5(), t5(), t7(), t0()}, Logic.Tile()),
                     DirectCast({t5(), t5(), tA(), t1(), t1(), t1(), t1(), t1(), t9(), t5()}, Logic.Tile()),
                     DirectCast({t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1()}, Logic.Tile()),
                     DirectCast({tD(), tD(), tF(), t1(), t1(), t1(), t1(), t1(), tE(), tD()}, Logic.Tile()),
                     DirectCast({t0(), t0(), t8(), t1(), t1(), t1(), t1(), t1(), t3(), t0()}, Logic.Tile()),
                     DirectCast({t0(), t0(), t8(), t1(), t1(), t1(), t1(), t1(), t3(), t0()}, Logic.Tile()),
                     DirectCast({t0(), t0(), t8(), t1(), t1(), t1(), t1(), t1(), t3(), t0()}, Logic.Tile()),
                     DirectCast({t0(), t0(), tB(), tD(), tD(), tD(), tD(), tD(), tC(), t0()}, Logic.Tile())}
        out.Tiles(0)(0).ContainedCharacter = SC_CharacterTemplate()
        out.Tiles(0)(0).ContainedCharacter.AIName = "Mercuh_Training5_SC"
        out.Tiles(1)(7).ContainedCharacter = Box2_CharacterTemplate()
        out.Tiles(2)(7).ContainedCharacter = Box2_CharacterTemplate()
        out.Tiles(3)(7).ContainedCharacter = Box2_CharacterTemplate()
        out.Tiles(4)(7).ContainedCharacter = Box2_CharacterTemplate()
        out.Tiles(5)(7).ContainedCharacter = Box2_CharacterTemplate()
        out.Tiles(6)(7).ContainedCharacter = Box2_CharacterTemplate()
        out.Tiles(1)(6).ContainedCharacter = Box2_CharacterTemplate()
        out.Tiles(2)(6).ContainedCharacter = Box2_CharacterTemplate()
        out.Tiles(3)(6).ContainedCharacter = Box2_CharacterTemplate()
        out.Tiles(4)(6).ContainedCharacter = Box2_CharacterTemplate()
        out.Tiles(5)(6).ContainedCharacter = Box2_CharacterTemplate()
        out.Tiles(6)(6).ContainedCharacter = Box2_CharacterTemplate()
        out.WarpPointCollection.Add(New Point(0, 2), New WarpPoint("Mercuh_Training4", New Point(8, 2)))
        out.WarpPointCollection.Add(New Point(9, 2), New WarpPoint("Mercuh_Training1", New Point(1, 2)))
        IO.File.WriteAllBytes(IO.Path.Combine(Environment.CurrentDirectory, "Maps", "mercuh_training5.map"), out.ToBinary)
        Return out
    End Function
#End Region

#End Region
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Build Maps
        GetMap_Mercuh_Training1()
        GetMap_Mercuh_Training2()
        GetMap_Mercuh_Training3()
        GetMap_Mercuh_Training4()
        GetMap_Mercuh_Training5()
        'full screen
        'Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        ' Me.TopMost = True
        'Me.WindowState = FormWindowState.Maximized

        'initialize
        m.LoadNewMap("Mercuh_Training1", New Point(1, 2))
        'm.BGMName = "STRM_MainMENU"
        'Size
        Dim i1 As Bitmap = m.PaintMap
        'Me.Size = i1.Size
    End Sub
#Region "Timer-Related"
    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Timer1.Start()
    End Sub
    Sub RepaintMap()
        PictureBox1.Image = m.PaintMap()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        If m.AcceptsMovementInput Then
            If GetKeyDicVal(Windows.Forms.Keys.D) AndAlso GetKeyDicVal(Windows.Forms.Keys.W) Then
                m.MoveMaintCharacterUpRight(GetKeyDicVal(Windows.Forms.Keys.ShiftKey))
            ElseIf GetKeyDicVal(Windows.Forms.Keys.A) AndAlso GetKeyDicVal(Windows.Forms.Keys.W) Then
                m.MoveMaintCharacterUpleft(GetKeyDicVal(Windows.Forms.Keys.ShiftKey))
            ElseIf GetKeyDicVal(Windows.Forms.Keys.D) AndAlso GetKeyDicVal(Windows.Forms.Keys.S) Then
                m.MoveMaintCharacterDownRight(GetKeyDicVal(Windows.Forms.Keys.ShiftKey))
            ElseIf GetKeyDicVal(Windows.Forms.Keys.A) AndAlso GetKeyDicVal(Windows.Forms.Keys.S) Then
                m.MoveMaintCharacterDownLeft(GetKeyDicVal(Windows.Forms.Keys.ShiftKey))
            Else
                If GetKeyDicVal(Windows.Forms.Keys.W) Then
                    m.MoveMaintCharacterUp(GetKeyDicVal(Windows.Forms.Keys.ShiftKey))
                ElseIf GetKeyDicVal(Windows.Forms.Keys.S) Then
                    m.MoveMaintCharacterDown(GetKeyDicVal(Windows.Forms.Keys.ShiftKey))
                ElseIf GetKeyDicVal(Windows.Forms.Keys.D) Then
                    m.MoveMaintCharacterRight(GetKeyDicVal(Windows.Forms.Keys.ShiftKey))
                ElseIf GetKeyDicVal(Windows.Forms.Keys.A) Then
                    m.MoveMaintCharacterLeft(GetKeyDicVal(Windows.Forms.Keys.ShiftKey))
                End If
            End If
            If GetKeyDicVal(Windows.Forms.Keys.E) Then
                m.Interact()
            End If
        End If
        If m.CanMainCharAttack Then
            If GetKeyDicVal(Windows.Forms.Keys.J) Then
                m.Attack()
            End If
        End If
        RepaintMap()
        Timer1.Start()
    End Sub
#End Region
#Region "Sound Related"
    Dim _fgmSoundPlayer As New Media.SoundPlayer
    Sub PlayFGM(FGMName As String)
        Static lastFile As String = ""
        If Not String.IsNullOrEmpty(FGMName) Then
            Dim l As String = IO.Path.Combine(Environment.CurrentDirectory, "FGM", FGMName & ".wav")
            If Not lastFile.ToLower = l.ToLower Then
                lastFile = l
                _fgmSoundPlayer.SoundLocation = l
                _fgmSoundPlayer.Load()
                _fgmSoundPlayer.Play()
            End If
        Else
            _fgmSoundPlayer.Stop()
            lastFile = ""
        End If
    End Sub
    Dim _bgmProc As Process
    Sub PlayBGM(BGMName As String)
        Static lastFile As String = ""
        If Not String.IsNullOrEmpty(BGMName) Then
            Dim l As String = IO.Path.Combine(Environment.CurrentDirectory, "BGM", BGMName & ".wav")
            If Not lastFile.ToLower = l.ToLower Then
                If _bgmProc IsNot Nothing Then
                    _bgmProc.Kill()
                Else
                    _bgmProc = New Process
                    _bgmProc.StartInfo.FileName = IO.Path.Combine(Environment.CurrentDirectory, "PlayMySoundLoop.exe")
                End If
                lastFile = l
                _bgmProc.StartInfo.Arguments = """" & l & """ " & Process.GetCurrentProcess.Id

                _bgmProc.Start()
            End If
        Else
            lastFile = ""
        End If
    End Sub

    Private Sub m_BGMNameChanged(NewBGMName As String) Handles m.BGMNameChanged
        PlayBGM(NewBGMName)
    End Sub
    'Private Sub m_FirstFrameDrawn() Handles m.FirstFrameDrawn
    '    PlayBGM(m.BGMName)
    'End Sub
    Private Sub m_NewMapLoaded() Handles m.NewMapLoaded
        PlayBGM(m.BGMName)
    End Sub
    Private Sub m_ForegroundAudioChanged(NewAudioName As String) Handles m.ForegroundAudioChanged
        PlayFGM(NewAudioName)
    End Sub
#End Region
End Class
