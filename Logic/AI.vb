Imports System.Drawing
Public MustInherit Class AI
    Public Property Name As String
    Public Shared Function GetByName(AIName As String) As AI
        Dim out As AI
        If AIManager.Current.AIDefinitions.ContainsKey(AIName) Then
            out = AIManager.Current.AIDefinitions(AIName)
        Else
            out = New Definitions.Dummy
        End If
        Return out
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="M">The map the character is on.</param>
    ''' <param name="CurrentCharacter">This character.  The one being loaded.</param>
    ''' <param name="CurrentLocation">The location of this character.</param>
    ''' <remarks></remarks>
    Public MustOverride Sub Load(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As System.Drawing.Point)
    Public MustOverride Sub OnInteracted(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As System.Drawing.Point)
    Public MustOverride Sub DoTurn(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As System.Drawing.Point)

    ''' <summary>
    ''' Contains code for loading AI's from DLLs
    ''' </summary>
    ''' <remarks></remarks>
    Private Class AIManager
        Public Property AIDefinitions As Dictionary(Of String, AI)
        Private Sub New() 'declared private so nothing else can make a new instance, which may consume a lot of memory.
            ReLoadAI()
        End Sub
        Public Sub ReLoadAI()
            AIDefinitions = New Dictionary(Of String, AI)
            AIDefinitions.Add("StationarySingleWeaponAdjacentAttack", New Definitions.StationarySingleWeaponAdjacentAttack)
            AIDefinitions.Add("StationarySingleWeaponRangedAttack", New Definitions.StationarySingleWeaponRangedAttack)
            AIDefinitions.Add("Wanderer", New Definitions.Wanderer)
            AIDefinitions.Add("Dummy", New Definitions.Dummy)
            AIDefinitions.Add("Mercuh_Training1_SC", New Definitions.MapScriptControls.Mercuh_Training1_SC)
            AIDefinitions.Add("Mercuh_Training2_SC", New Definitions.MapScriptControls.Mercuh_Training2_SC)
            AIDefinitions.Add("Mercuh_Training3_SC", New Definitions.MapScriptControls.Mercuh_Training3_SC)
            AIDefinitions.Add("Mercuh_Training4_SC", New Definitions.MapScriptControls.Mercuh_Training4_SC)
            AIDefinitions.Add("Mercuh_Training5_SC", New Definitions.MapScriptControls.Mercuh_Training5_SC)
            AIDefinitions.Add("Mercuh_Training4_Table", New Definitions.MapScriptControls.Mercuh_Training4_Table)
            AIDefinitions.Add("DeathTrap", New Definitions.DeathTrap)
            'Load external AI's
            If IO.Directory.Exists(IO.Path.Combine(Environment.CurrentDirectory, "AI")) Then
                For Each file In IO.Directory.GetFiles(IO.Path.Combine(Environment.CurrentDirectory, "AI"))
                    Dim a As System.Reflection.Assembly = Nothing
                    Try
                        a = System.Reflection.Assembly.LoadFrom(file) 'Load the DLL
                    Catch ex As Exception
                        'do nothing, if it failed, a will be nothing and the next code will not run, ignoring this file which may or may not be a valid assembly.
                    End Try
                    If a IsNot Nothing Then
                        Dim types As Type() = a.GetTypes 'Get all classes in the DLL
                        Dim plugins As New Generic.List(Of AI) 'This will be the list of instances of your interface
                        For Each item In types 'Look at each class in the DLL
                            If item.IsSubclassOf(GetType(AI)) Then
                                plugins.Add(a.CreateInstance(item.ToString))
                            End If
                        Next
                        For Each item In plugins
                            If Not AIDefinitions.ContainsKey(item.Name) Then
                                AIDefinitions.Add(item.Name, item)
                            End If
                        Next
                    End If
                Next
            End If
        End Sub

        Private Shared _current As AIManager = New AIManager
        ''' <summary>
        ''' Returns an AI manager.  Use this to avoid using too much memory (which multiple definitions will do).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Current As AIManager
            Get
                Return _current
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Contains a few pre-defined AI's.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Definitions
        Public Class Dummy
            Inherits AI
            Public Overrides Sub OnInteracted(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As System.Drawing.Point)
                'do nothing
            End Sub
            Public Overrides Sub DoTurn(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As System.Drawing.Point)
                'do nothing
            End Sub
            Public Overrides Sub Load(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As System.Drawing.Point)
                'do nothing
            End Sub
            Public Sub New()
                Name = "Dummy"
            End Sub
        End Class
        Public Class StationarySingleWeaponRangedAttack
            Inherits AI
            Public Overrides Sub DoTurn(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                If CurrentCharacter.Weapons.Count > 0 Then
                    Dim mainCharLoc As Point = M.MainCharacterLocation
                    Dim range As Integer = CurrentCharacter.Weapons(CurrentCharacter.CurrentWeaponIndex).Range
                    If Math.Abs(mainCharLoc.X - CurrentLocation.X) <= range AndAlso Math.Abs(mainCharLoc.Y - CurrentLocation.Y) <= range Then 'Main character is within range
                        Dim xDif As Integer = mainCharLoc.X - CurrentLocation.X
                        Dim yDif As Integer = mainCharLoc.Y - CurrentLocation.Y
                        If xDif >= (range * -1) AndAlso xDif < 0 AndAlso yDif = 0 Then 'Main character is left
                            M.MoveNPCLeft(CurrentCharacter.InternalID, True)
                        ElseIf xDif <= (range) AndAlso xDif > 0 AndAlso yDif = 0 Then 'Main character is right
                            M.MoveNPCRight(CurrentCharacter.InternalID, True)
                        ElseIf xDif = 0 AndAlso 0 < yDif AndAlso yDif <= range Then 'Main character is down
                            M.MoveNPCDown(CurrentCharacter.InternalID, True)
                        ElseIf xDif = 0 AndAlso yDif < 0 AndAlso yDif >= (range * -1) Then 'Main character is up
                            M.MoveNPCUp(CurrentCharacter.InternalID, True)
                        ElseIf 0 > xDif AndAlso xDif >= range * -1 AndAlso 0 > yDif AndAlso yDif >= range * -1 Then 'upleft
                            M.MoveNPCUpLeft(CurrentCharacter.InternalID, True)
                        ElseIf 0 > xDif AndAlso xDif >= range * -1 AndAlso 0 < yDif AndAlso yDif <= range Then 'downleft
                            M.MoveNPCDownLeft(CurrentCharacter.InternalID, True)
                        ElseIf 0 < xDif AndAlso xDif <= range AndAlso 0 < yDif AndAlso yDif <= range Then 'downright
                            M.MoveNPCDownRight(CurrentCharacter.InternalID, True)
                        ElseIf 0 < xDif AndAlso xDif <= range AndAlso 0 > yDif AndAlso yDif >= range * -1 Then 'upright
                            M.MoveNPCUpRight(CurrentCharacter.InternalID, True)
                        End If
                        Dim commenceAttack As Boolean = True
                        For y As Integer = mainCharLoc.Y To CurrentLocation.Y Step Math.Max(yDif / Math.Max(Math.Abs(yDif), 1), 1)
                            For x As Integer = mainCharLoc.X To CurrentLocation.X Step Math.Max(xDif / Math.Max(Math.Abs(xDif), 1), 1)
                                Select Case CurrentCharacter.FacingDirection
                                    Case Direction.Up
                                        If Not M.Tiles(y)(x).CanEnterBottom Then
                                            commenceAttack = False
                                            GoTo Attack
                                        End If
                                    Case Direction.Down
                                        If Not M.Tiles(y)(x).CanEnterTop Then
                                            commenceAttack = False
                                            GoTo Attack
                                        End If
                                    Case Direction.Left
                                        If Not M.Tiles(y)(x).CanEnterRight Then
                                            commenceAttack = False
                                            GoTo Attack
                                        End If
                                    Case Direction.Right
                                        If Not M.Tiles(y)(x).CanEnterLeft Then
                                            commenceAttack = False
                                            GoTo Attack
                                        End If
                                    Case Direction.UpLeft
                                        If Not (M.Tiles(y)(x).CanEnterBottom AndAlso M.Tiles(y)(x).CanEnterRight) Then
                                            commenceAttack = False
                                            GoTo Attack
                                        End If
                                    Case Direction.UpRight
                                        If Not (M.Tiles(y)(x).CanEnterBottom AndAlso M.Tiles(y)(x).CanEnterLeft) Then
                                            commenceAttack = False
                                            GoTo Attack
                                        End If
                                    Case Direction.DownLeft
                                        If Not (M.Tiles(y)(x).CanEnterTop AndAlso M.Tiles(y)(x).CanEnterRight) Then
                                            commenceAttack = False
                                            GoTo Attack
                                        End If
                                    Case Direction.DownRight
                                        If Not (M.Tiles(y)(x).CanEnterTop AndAlso M.Tiles(y)(x).CanEnterLeft) Then
                                            commenceAttack = False
                                            GoTo Attack
                                        End If
                                End Select
                            Next
                        Next
Attack:                 If commenceAttack Then M.CharacterAttack(CurrentCharacter.InternalID)
                    End If
                End If
            End Sub
            Public Overrides Sub OnInteracted(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)

            End Sub
            Public Overrides Sub Load(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)

            End Sub
        End Class
        Public Class StationarySingleWeaponAdjacentAttack
            Inherits AI
            Public Overrides Sub DoTurn(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Drawing.Point)
                If CurrentCharacter.Weapons.Count > 0 Then
                    Dim mainCharLoc As Point = M.MainCharacterLocation
                    Dim range As Integer = 1 'CurrentCharacter.Weapons(CurrentCharacter.CurrentWeaponIndex).Range
                    If Math.Abs(mainCharLoc.X - CurrentLocation.X) <= range AndAlso Math.Abs(mainCharLoc.Y - CurrentLocation.Y) <= range Then 'Main character is within range
                        Dim xDif As Integer = mainCharLoc.X - CurrentLocation.X
                        Dim yDif As Integer = mainCharLoc.Y - CurrentLocation.Y
                        If xDif <= (range * -1) AndAlso yDif = 0 Then 'Main character is left
                            M.MoveNPCLeft(CurrentCharacter.InternalID, True)
                        ElseIf xDif <= (range) AndAlso yDif = 0 Then 'Main character is right
                            M.MoveNPCRight(CurrentCharacter.InternalID, True)
                        ElseIf xDif = 0 AndAlso 0 < yDif AndAlso yDif <= range Then 'Main character is down
                            M.MoveNPCDown(CurrentCharacter.InternalID, True)
                        ElseIf xDif = 0 AndAlso yDif < 0 AndAlso yDif >= (range * -1) Then 'Main character is up
                            M.MoveNPCUp(CurrentCharacter.InternalID, True)
                        ElseIf 0 > xDif AndAlso xDif >= range * -1 AndAlso 0 > yDif AndAlso yDif >= range * -1 Then 'upleft
                            M.MoveNPCUpLeft(CurrentCharacter.InternalID, True)
                        ElseIf 0 > xDif AndAlso xDif >= range * -1 AndAlso 0 < yDif AndAlso yDif <= range Then 'downleft
                            M.MoveNPCDownLeft(CurrentCharacter.InternalID, True)
                        ElseIf 0 < xDif AndAlso xDif <= range AndAlso 0 < yDif AndAlso yDif <= range Then 'downright
                            M.MoveNPCDownRight(CurrentCharacter.InternalID, True)
                        ElseIf 0 < xDif AndAlso xDif <= range AndAlso 0 > yDif AndAlso yDif >= range * -1 Then 'upright
                            M.MoveNPCUpRight(CurrentCharacter.InternalID, True)
                        End If
                        M.CharacterAttack(CurrentCharacter.InternalID)
                    End If
                End If
            End Sub
            Public Overrides Sub OnInteracted(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Drawing.Point)
                'do nothing
            End Sub
            Public Sub New()
                Name = "StationarySingleWeapon"
            End Sub
            Public Overrides Sub Load(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)

            End Sub
        End Class

        Public Class Wanderer
            Inherits AI

            Public Overrides Sub DoTurn(ByRef M As Map, ByVal CurrentCharacter As Character, ByVal CurrentLocation As Point)
                If M.CanCharacterMove(CurrentCharacter.InternalID) Then
                    Select Case (New Random).Next(0, 7)
                        Case 0
                            M.MoveNPCUp(CurrentCharacter.InternalID)
                        Case 1
                            M.MoveNPCDown(CurrentCharacter.InternalID)
                        Case 2
                            M.MoveNPCLeft(CurrentCharacter.InternalID)
                        Case 3
                            M.MoveNPCRight(CurrentCharacter.InternalID)
                        Case 4
                            M.MoveNPCUpRight(CurrentCharacter.InternalID)
                        Case 5
                            M.MoveNPCUpLeft(CurrentCharacter.InternalID)
                        Case 6
                            M.MoveNPCDownRight(CurrentCharacter.InternalID)
                        Case 7
                            M.MoveNPCLeft(CurrentCharacter.InternalID)
                    End Select
                End If
            End Sub

            Public Overrides Sub Load(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)

            End Sub

            Public Overrides Sub OnInteracted(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)

            End Sub
        End Class

        Public Class MapScriptControls
            Public Class Mercuh_Training1_SC
                Inherits AI

                Public Overrides Sub DoTurn(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    'Do Nothing
                End Sub

                Public Overrides Sub Load(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    If Not SaveFile.CurrentSaveFile.GetEventVariable("Mercuh_Training1_LoadMessageGiven") = True Then
                        M.DisplayDialog("Good, you're awake.  I'm the intern here at" & vbCrLf &
                                        "Mercuh Incorporated.  Press E to proceed to the next bit" & vbCrLf &
                                        "of dialog.")
                        M.DisplayDialog("Intern: You are about to be" & vbCrLf &
                                        "put through a series of tests.")
                        M.DisplayDialog("Intern: These tests will prepare you for defeating our enemy,")
                        M.DisplayDialog("Intern: SciCo.  They're just that: psycho.  They killed our" & vbCrLf &
                                        "founder and we will bring them down.")
                        M.DisplayDialog("Intern: Anyway, please proceed to the next room" & vbCrLf &
                                        "using W, A, S, and D to move.")
                        SaveFile.CurrentSaveFile.SetEventVariable("Mercuh_Training1_LoadMessageGiven", True)
                    ElseIf SaveFile.CurrentSaveFile.GetEventVariable("Mercuh_DidRepeat") = True AndAlso SaveFile.CurrentSaveFile.GetEventVariable("Mercuh_DidRepeatDialogPlayed") = False Then
                        M.DisplayDialog("Intern: By the way, building an exit is expensive.")
                        M.DisplayDialog("Intern: So, you'll be solving these tests," & vbCrLf & "for the rest of your life.")
                        M.DisplayDialog("Intern: Have fun.")
                        SaveFile.CurrentSaveFile.SetEventVariable("Mercuh_DidRepeatDialogPlayed", True)
                    End If
                End Sub

                Public Overrides Sub OnInteracted(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    'Do Nothing
                End Sub
            End Class

            Public Class Mercuh_Training2_SC
                Inherits AI

                Public Overrides Sub DoTurn(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    'Do Nothing
                End Sub

                Public Overrides Sub Load(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    If Not SaveFile.CurrentSaveFile.GetEventVariable("Mercuh_Training2_LoadMessageGiven") = True Then
                        M.DisplayDialog("Intern: For the first test, please navigate" & vbCrLf & "this simple maze using the WASD control scheme.")
                        M.DisplayDialog("Intern: To prevent cheating, we used generic" & vbCrLf & "brand super glue to glue the boxes to the floor.")
                        M.DisplayDialog("Intern: Here at Mercuh, we save money to make money.")
                        SaveFile.CurrentSaveFile.SetEventVariable("Mercuh_Training2_LoadMessageGiven", True)
                    End If
                End Sub

                Public Overrides Sub OnInteracted(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    'Do Nothing
                End Sub
            End Class
            Public Class Mercuh_Training3_SC
                Inherits AI

                Public Overrides Sub DoTurn(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    'Do Nothing
                End Sub

                Public Overrides Sub Load(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    If Not SaveFile.CurrentSaveFile.GetEventVariable("Mercuh_Training3_LoadMessageGiven") = True Then
                        M.DisplayDialog("Intern: The boxes in this test are closer together.")
                        M.DisplayDialog("Intern: You can move diagonally to sqeeze past them.")
                        M.DisplayDialog("Intern: Use W+D, W+A, S+A, and S+D to move diagonally.")
                        SaveFile.CurrentSaveFile.SetEventVariable("Mercuh_Training3_LoadMessageGiven", True)
                    End If
                End Sub

                Public Overrides Sub OnInteracted(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    'Do Nothing
                End Sub
            End Class
            Public Class Mercuh_Training4_SC
                Inherits AI

                Public Overrides Sub DoTurn(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    'Do Nothing
                End Sub

                Public Overrides Sub Load(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    If Not SaveFile.CurrentSaveFile.GetEventVariable("Mercuh_Training4_LoadMessageGiven") = True Then
                        M.DisplayDialog("Intern: One does not simply walk past these boxes.")
                        M.DisplayDialog("Intern: Pick up a box cutter from that table using E.")
                        M.DisplayDialog("Intern: Use the box cutter by pressing J while in front of a box.")
                        SaveFile.CurrentSaveFile.SetEventVariable("Mercuh_Training4_LoadMessageGiven", True)
                    End If
                End Sub

                Public Overrides Sub OnInteracted(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    'Do Nothing
                End Sub
            End Class
            Public Class Mercuh_Training5_SC
                Inherits AI

                Public Overrides Sub DoTurn(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    If M.MainCharacterLocation = New Point(8, 2) Then
                        SaveFile.CurrentSaveFile.SetEventVariable("Mercuh_DidRepeat", True)
                    End If
                End Sub

                Public Overrides Sub Load(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    If Not SaveFile.CurrentSaveFile.GetEventVariable("Mercuh_Training5_LoadMessageGiven") = True Then
                        M.DisplayDialog("Intern: These boxes are more durable.")
                        M.DisplayDialog("Intern: It may take you longer to cut through them.")
                        M.DisplayDialog("Intern: But please destroy as few as posible.")
                        M.DisplayDialog("Intern: These boxes cost 10 cents more than the others." & vbCrLf & "Mercuh Incorporated doesn't want to pay that.")
                        SaveFile.CurrentSaveFile.SetEventVariable("Mercuh_Training5_LoadMessageGiven", True)
                    End If
                End Sub

                Public Overrides Sub OnInteracted(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    'Do Nothing
                End Sub
            End Class
            Public Class Mercuh_Training4_Table
                Inherits AI
                Private Function BoxCutter() As Weapon
                    Dim out As New Weapon
                    Dim b As New Bitmap(48, 48)
                    out.Name = "Box Cutter"
                    out.Type = WeaponType.GetWeaponByID(2)
                    out.BaseDamage = 1
                    out.FrameWait = 12
                    out.IsRecharge = True
                    out.Magazine = 0
                    out.CurrentMagazineLoad = 0
                    out.Range = 1
                    out.ReloadSpeed = 0
                    out.BulletUp = New AnimatedSprite(b)
                    out.BulletDown = New AnimatedSprite(b)
                    out.BulletLeft = New AnimatedSprite(b)
                    out.BulletRight = New AnimatedSprite(b)
                    out.BulletUpRight = New AnimatedSprite(b)
                    out.BulletUpLeft = New AnimatedSprite(b)
                    out.BulletDownRight = New AnimatedSprite(b)
                    out.BulletDownLeft = New AnimatedSprite(b)
                    out.Icon = b
                    out.BulletSpeed = 1
                    Return out
                End Function
                Public Overrides Sub DoTurn(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)

                End Sub

                Public Overrides Sub Load(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)

                End Sub

                Public Overrides Sub OnInteracted(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                    If M.MainCharacter.Weapons.Count = 0 Then
                        M.DisplayDialog("It's a table with several Box Cutters on it.")
                        M.MainCharacter.Weapons.Add(BoxCutter)
                        M.DisplayDialog("Picked up a Box Cutter.")
                    Else
                        M.DisplayDialog("It's a table with several Box Cutters on it.")
                    End If
                End Sub
            End Class
        End Class
        Public Class DeathTrap
            Inherits AI.Definitions.StationarySingleWeaponAdjacentAttack
            Private Function BoxCutter() As Weapon
                Dim out As New Weapon
                Dim b As New Bitmap(48, 48)
                out.Name = "Box Cutter"
                out.Type = WeaponType.GetWeaponByID(2)
                out.BaseDamage = 1
                out.FrameWait = 20
                out.IsRecharge = True
                out.Magazine = 0
                out.CurrentMagazineLoad = 0
                out.Range = 100
                out.ReloadSpeed = 0
                out.BulletUp = New AnimatedSprite(b)
                out.BulletDown = New AnimatedSprite(b)
                out.BulletLeft = New AnimatedSprite(b)
                out.BulletRight = New AnimatedSprite(b)
                out.BulletUpRight = New AnimatedSprite(b)
                out.BulletUpLeft = New AnimatedSprite(b)
                out.BulletDownRight = New AnimatedSprite(b)
                out.BulletDownLeft = New AnimatedSprite(b)
                out.Icon = b
                out.BulletSpeed = 100
                Return out
            End Function
            Public Overrides Sub DoTurn(ByRef M As Map, ByVal CurrentCharacter As Character, CurrentLocation As Point)
                If CurrentCharacter.Weapons.Count = 0 Then
                    CurrentCharacter.Weapons.Add(BoxCutter)
                End If
                MyBase.DoTurn(M, CurrentCharacter, CurrentLocation)
            End Sub
        End Class
End Class
End Class