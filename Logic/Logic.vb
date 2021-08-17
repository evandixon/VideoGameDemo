Imports System.Drawing
'TODO: make class Range cryptographically random
Public Class Common
    Public Const LevelCap As Integer = 100
    Public Const CriticalHitMultiplier As Integer = 3
    Public Class NeedsReloadException
        Inherits Exception
    End Class
    Public Shared Function GetLevel(Experience As Integer) As Integer
        Return Math.Min((((Experience / 1200) + 1) ^ 0.5), LevelCap)
    End Function
    ''' <summary>
    ''' Turns a byte array into a string containing hex
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ByteArrToHex(ByteArr As Byte()) As String
        Dim out As String = ""
        For Each item In ByteArr
            out += Conversion.Hex(item).PadLeft(2, "0"c) & " "
        Next
        Return out.Trim
    End Function
    Public Shared Function BoolToByte(Bool As Boolean) As Byte
        If Bool Then
            Return 1
        Else
            Return 0
        End If
    End Function
    Public Shared Function SubByteArr(ByteArr As Byte(), Index As Integer, EndPoint As Integer) As Byte()
        Dim output(EndPoint - Index) As Byte
        For count As Integer = 0 To output.Length - 1
            output(count) = ByteArr(count + Index)
        Next
        Return output
    End Function
    Public Shared Function GetRandomDouble(Min As Double, Max As Double) As Double
        Static r As New Random
        Return r.Next(Min * 100, Max * 100) / 100
    End Function
End Class
Public Class Range
    Public Property MinValue As Integer
    Public Property MaxValue As Integer
    Public Sub New(Min As Integer, Max As Integer)
        MinValue = Min
        MaxValue = Max
    End Sub
    Public Function RandomNumberInRange() As Integer
        Static r As New Random
        Return r.Next(MinValue, MaxValue)
    End Function
End Class
Public Class RangeDouble
    Public Property MinValue As Double
    Public Property MaxValue As Double
    Public Sub New(Min As Double, Max As Double)
        MinValue = Min
        MaxValue = Max
    End Sub
    Public Function RandomNumberInRange() As Double
        Return Common.GetRandomDouble(MinValue, MaxValue)
    End Function
End Class
Public Class WeaponType
    Public Property Name As String
    Public Property ID As Int16
    Public Property BaseDamageRange As Range
    Public Property FrameWaitRange As Range
    Public Property IsRechargeWeapon As Boolean
    Public Property MagazineRange As Range
    Public Property RangeRange As Range
    'Public Property AccuracyRange As RangeDouble
    Public Property BaseLevelRequirement As Integer
    Public Property MaxHeldAmmo As Integer
    Public Property ReloadSpeedRange As Range
    'Public Property CriticalHitChance As RangeDouble
    Public Overrides Function Equals(obj As Object) As Boolean
        Return TypeOf obj Is WeaponType AndAlso DirectCast(obj, WeaponType).ID = ID
    End Function
    Public Function GenerateWeapon(Name As String, ApproximateLevel As Integer) As Weapon
        Dim x As New Weapon
        x.Name = Name
        x.Type = Me
        x.BaseDamage = New Range(Math.Min(Math.Max(BaseDamageRange.MinValue, ApproximateLevel * 5), BaseDamageRange.MaxValue), Math.Min(BaseDamageRange.MaxValue, Math.Max((ApproximateLevel + 10) * 10, BaseDamageRange.MinValue))).RandomNumberInRange
        'x.TurnCost = TurnCostRange.RandomNumberInRange
        x.FrameWait = FrameWaitRange.RandomNumberInRange
        x.Magazine = MagazineRange.RandomNumberInRange
        x.Range = RangeRange.RandomNumberInRange
        'x.Accuracy = AccuracyRange.RandomNumberInRange
        x.IsRecharge = IsRechargeWeapon
        x.ReloadSpeed = ReloadSpeedRange.RandomNumberInRange
        'x.CriticalHitChance = CriticalHitChance.RandomNumberInRange
        Return x
    End Function
    'Public Shared Function RevolverWeaponType() As WeaponType
    '    Dim x As New WeaponType
    '    x.Name = "Revolver"
    '    x.ID = 1
    '    x.BaseDamageRange = New Range(100, 1000) 'Power of 1000 damage increased to 1200 with proficiency should kill level 100 enemy with two hits.  THis seems a bit excessive for a ranged weapon.  TODO: lower revolver's max power and let 1000 be max power of a non-ranged weapon (like a sword).
    '    x.TurnCostRange = New RangeDouble(0.75, 1)
    '    x.IsRechargeWeapon = True
    '    x.MagazineRange = New Range(6, 9)
    '    x.RangeRange = New Range(3, 10)
    '    x.AccuracyRange = New RangeDouble(0.75, 1)
    '    x.BaseLevelRequirement = 11
    '    x.MaxHeldAmmo = 200
    '    x.ReloadSpeedRange = New RangeDouble(1, 1)
    '    x.CriticalHitChance = New RangeDouble(0.1, 0.5)
    '    Return x
    'End Function
    Public Shared Function KnifeWeaponType() As WeaponType
        Dim x As New WeaponType
        x.ID = 2
        x.Name = "Knife"
        x.BaseDamageRange = New Range(10, 300)
        'x.TurnCostRange = New RangeDouble(0.1, 0.5)
        x.FrameWaitRange = New Range(4, 8)
        x.IsRechargeWeapon = True
        x.MagazineRange = New Range(0, 0)
        x.RangeRange = New Range(1, 1)
        'x.AccuracyRange = New RangeDouble(0.9, 1)
        x.BaseLevelRequirement = 1
        x.MaxHeldAmmo = 0
        x.ReloadSpeedRange = New Range(0, 0)
        'x.CriticalHitChance = New RangeDouble(0.1, 0.5)
        Return x
    End Function
    Public Shared Function ElectricWeaponType() As WeaponType
        Dim x As New WeaponType
        x.ID = 3
        x.Name = "Electric Weapon"
        x.BaseDamageRange = New Range(10, 300)
        'x.TurnCostRange = New RangeDouble(0.1, 0.5)
        x.FrameWaitRange = New Range(4, 8)
        x.IsRechargeWeapon = True
        x.MagazineRange = New Range(0, 0)
        x.RangeRange = New Range(1, 1)
        'x.AccuracyRange = New RangeDouble(0.9, 1)
        x.BaseLevelRequirement = 1
        x.MaxHeldAmmo = 0
        x.ReloadSpeedRange = New Range(0, 0)
        'x.CriticalHitChance = New RangeDouble(0.1, 0.5)
        Return x
    End Function
    Public Shared Function GetWeaponTypes() As WeaponType()
        Return {KnifeWeaponType(), ElectricWeaponType()}
    End Function
    Public Shared Function GetWeaponByID(ID As Int16) As WeaponType
        Select Case ID
            'Case 1
            '    Return RevolverWeaponType()
            Case 2
                Return KnifeWeaponType()
            Case 3
                Return ElectricWeaponType()
            Case Else
                Return Nothing
        End Select
    End Function
    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class
Public Class Weapon
    Public Property Name As String = ""
    Public Property Type As WeaponType
    Public Property BaseDamage As Integer
    ''' <summary>
    ''' Amount of frames that must pass to fire the weapon again
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FrameWait As Integer
    ''' <summary>
    ''' If the turn cost is greater than 1, this property controls whether the weapon is used after waiting said number of turns (charge) or before waiting said number of turns (recharge).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsRecharge As Boolean
    ''' <summary>
    ''' Magazine size of the weapon.  Set to 0 if the weapon doesn't need ammo (like a sword).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Magazine As Integer
    Public Property CurrentMagazineLoad As Integer
    Public Property Range As Integer
    'Public Property Accuracy As Double
    ''' <summary>
    ''' Amount of frames that must pass to finish reloading the weapon
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ReloadSpeed As Integer
    'Public Property CriticalHitChance As Double
    Public ReadOnly Property LevelRequirement As Integer
        Get
            'TODO: find accurate level requirement algorith, after making things more balanced.
            Return 1 'Math.Max(Type.BaseLevelRequirement, (BaseDamage / 10 * Accuracy * ((1 - TurnCost) + 1))) 'Might need to be changed in the future
        End Get
    End Property
    Public Property BulletSpeed As Double
    Public Property Icon As Bitmap
    Public Property BulletUp As AnimatedSprite
    Public Property BulletDown As AnimatedSprite
    Public Property BulletLeft As AnimatedSprite
    Public Property BulletRight As AnimatedSprite
    Public Property BulletUpRight As AnimatedSprite
    Public Property BulletUpLeft As AnimatedSprite
    Public Property BulletDownRight As AnimatedSprite
    Public Property BulletDownLeft As AnimatedSprite

    Public Function ToBinary() As Byte()
        Dim outparts As New List(Of Byte())
        Dim e As New System.Text.UnicodeEncoding
        outparts.Add(e.GetBytes(Name))
        outparts.Add(BitConverter.GetBytes(Type.ID))
        outparts.Add(BitConverter.GetBytes(BaseDamage))
        outparts.Add(BitConverter.GetBytes(FrameWait))
        outparts.Add(BitConverter.GetBytes(IsRecharge))
        outparts.Add(BitConverter.GetBytes(Magazine))
        outparts.Add(BitConverter.GetBytes(CurrentMagazineLoad))
        outparts.Add(BitConverter.GetBytes(Range))
        outparts.Add(BitConverter.GetBytes(ReloadSpeed))
        outparts.Add(BulletUp.ToBinary)
        outparts.Add(BulletDown.ToBinary)
        outparts.Add(BulletLeft.ToBinary)
        outparts.Add(BulletRight.ToBinary)
        outparts.Add(BulletUpRight.ToBinary)
        outparts.Add(BulletUpLeft.ToBinary)
        outparts.Add(BulletDownRight.ToBinary)
        outparts.Add(BulletLeft.ToBinary)
        Dim iconStream As New IO.MemoryStream
        Icon.Save(iconStream, Imaging.ImageFormat.Png)
        outparts.Add(iconStream.ToArray)
        iconStream.Dispose()
        outparts.Add(BitConverter.GetBytes(BulletSpeed))
        Return FileFormatLib.FileFormatLib.ToBinary(outparts)
    End Function
    Public Shared Function FromBinary(Binary As Byte()) As Weapon
        Dim out As New Weapon
        Dim inparts As List(Of Byte()) = FileFormatLib.FileFormatLib.FromBinary(Binary)
        Dim e As New System.Text.UnicodeEncoding
        out.Name = e.GetString(inparts(0))
        out.Type = WeaponType.GetWeaponByID(BitConverter.ToInt16(inparts(1), 0))
        out.BaseDamage = BitConverter.ToInt32(inparts(2), 0)
        out.FrameWait = BitConverter.ToInt32(inparts(3), 0)
        out.IsRecharge = BitConverter.ToBoolean(inparts(4), 0)
        out.Magazine = BitConverter.ToInt32(inparts(5), 0)
        out.CurrentMagazineLoad = BitConverter.ToInt32(inparts(6), 0)
        out.Range = BitConverter.ToInt32(inparts(7), 0)
        out.ReloadSpeed = BitConverter.ToInt32(inparts(8), 0)
        out.BulletUp = AnimatedSprite.FromBinary(inparts(9))
        out.BulletDown = AnimatedSprite.FromBinary(inparts(10))
        out.BulletLeft = AnimatedSprite.FromBinary(inparts(11))
        out.BulletRight = AnimatedSprite.FromBinary(inparts(12))
        out.BulletUpRight = AnimatedSprite.FromBinary(inparts(13))
        out.BulletUpLeft = AnimatedSprite.FromBinary(inparts(14))
        out.BulletDownRight = AnimatedSprite.FromBinary(inparts(15))
        out.BulletDownLeft = AnimatedSprite.FromBinary(inparts(16))
        Dim iconStream As New IO.MemoryStream(inparts(17))
        out.Icon = Bitmap.FromStream(iconStream)
        iconStream.Dispose()
        If inparts.Count > 18 Then
            out.BulletSpeed = BitConverter.ToSingle(inparts(18), 0)
        Else
            out.BulletSpeed = 1
        End If
        Return out
    End Function
End Class
Public Class WeaponProficiency
    Public Property Experience As Integer = 0
    Public ReadOnly Property Level As Integer
        Get
            Return Common.GetLevel(Experience) - 1
        End Get
    End Property
    ''' <summary>
    ''' multiply damage by this
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DamageMultiplier As Decimal
        Get
            Return Level / 100 + 1
        End Get
    End Property
    ''' <summary>
    ''' Subtract from level requirement; level 100 proficiency can reduce level requirement by 20 levels.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LevelRequirementReduction As Integer
        Get
            Return Math.Round(Level * 0.2)
        End Get
    End Property
    ''' <summary>
    ''' Multiply with turn cost; level 100 proficiency can half turn cost
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TurnCostReduction As Double
        Get
            Return Level * 0.005
        End Get
    End Property
    ' ''' <summary>
    ' ''' Multiply with accuracy; level 100 proficiency can double accuracy
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    ''' <summary>
    ''' Depricated; returns 1
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property AccuracyIncrease As Double
        Get
            Return 1 'Level * 0.005 + 1
        End Get
    End Property
    ' ''' <summary>
    ' ''' Multiply with reload speed; level 100 proficiency can half reload speed
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    ''' <summary>
    ''' Depricated; returns 1
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ReloadReduction As Double
        Get
            Return 1 'Level * 0.005
        End Get
    End Property
    ' ''' <summary>
    ' ''' Multiply with Cit. Hit Chance
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    ''' <summary>
    ''' depricated; returns 1
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CriticalHitMultiplier As Double
        Get
            Return 1 'Level * 0.005 + 1
        End Get
    End Property
    Public Sub New(Exp As Integer)
        MyBase.New()
        Experience = Exp
    End Sub
    Public Function ToBinary() As Byte()
        Return BitConverter.GetBytes(Experience)
    End Function
    Public Shared Function FromBinary(Binary As Byte()) As WeaponProficiency
        Return New WeaponProficiency(BitConverter.ToInt32(Binary, 0))
    End Function
End Class
Public Enum Direction
    Up
    Down
    Left
    Right
    UpRight
    UpLeft
    DownRight
    DownLeft
End Enum
Public Class Character
#Region "Events"
    Public Event LeveledUp(sender As Character, ByVal NewLevel As Integer)
    Public Event Defeated(sender As Character)
    Public Event ProficiencyLeveledUp(sender As Character, NewLevel As Integer, RelevantWeaponType As WeaponType)
    Public Event AttackMissed(sender As Character)
    Public Event CriticalHitDealt(sender As Character, DamageDealt As Integer)
#End Region
#Region "Properties"
    Public Property InternalID As Guid = Guid.NewGuid
    Public Property Name As String = "Unnamed Character"
    Public Property SpriteUp As AnimatedSprite
    Public Property SpriteDown As AnimatedSprite
    Public Property SpriteLeft As AnimatedSprite
    Public Property SpriteRight As AnimatedSprite
    Public Property SpriteUpMoving As AnimatedSprite
    Public Property SpriteDownMoving As AnimatedSprite
    Public Property SpriteLeftMoving As AnimatedSprite
    Public Property SpriteRightMoving As AnimatedSprite
    Public Property SpriteUpRight As AnimatedSprite
    Public Property SpriteUpLeft As AnimatedSprite
    Public Property SpriteDownRight As AnimatedSprite
    Public Property SpriteDownLeft As AnimatedSprite
    Public Property SpriteUpRightMoving As AnimatedSprite
    Public Property SpriteUpLeftMoving As AnimatedSprite
    Public Property SpriteDownRightMoving As AnimatedSprite
    Public Property SpriteDownLeftMoving As AnimatedSprite
    Public Property Weapons As New List(Of Weapon)
    Public Property BaseMaxHealth As Integer = 100
    'Public Property TurnLengthLeft As Double = 1
    Public Property FacingDirection As Direction = Direction.Down
    Public Property CurrentHealth As Integer = 100
    Public Property BaseResistance As Decimal = 1
    Public Property Experience As Integer = 0
    Public Property HeldAmmo As New Dictionary(Of WeaponType, Integer)
    Public Property Proficiency As New Dictionary(Of WeaponType, WeaponProficiency)
    Public Property AIName As String = "Dummy"
    Public Property CurrentWeaponIndex As Integer = 0
    Public Property HeadShot As TileTemplate
#End Region
#Region "Temporary Properties"
    Public Property IsMoving As Boolean
    ''' <summary>
    ''' How many frames must be waited in order to attack again
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AttackFrameWait As Integer
    Public ReadOnly Property CanAttack As Boolean
        Get
            Return AttackFrameWait = 0
        End Get
    End Property
    Public Property DamageDisplayFrameCount As Integer = Map.CharacterMovementStep * 4 + 1
    Public Property DamageDisplayFrameLength As Integer = Map.CharacterMovementStep * 4
    Public Property DamageDisplayInitialDamage As Integer
#End Region
#Region "Readonly Properties"
    Public ReadOnly Property Sprite As AnimatedSprite
        Get
            If IsMoving Then
                Select Case FacingDirection
                    Case Direction.Up
                        Return SpriteUpMoving
                    Case Direction.Down
                        Return SpriteDownMoving
                    Case Direction.Left
                        Return SpriteLeftMoving
                    Case Direction.Right
                        Return SpriteRightMoving
                    Case Direction.UpRight
                        Return SpriteUpRightMoving
                    Case Direction.UpLeft
                        Return SpriteUpLeftMoving
                    Case Direction.DownRight
                        Return SpriteDownRightMoving
                    Case Direction.DownLeft
                        Return SpriteDownLeftMoving
                    Case Else 'Something's wrong
                        Return SpriteDownMoving
                End Select
            Else
                Select Case FacingDirection
                    Case Direction.Up
                        Return SpriteUp
                    Case Direction.Down
                        Return SpriteDown
                    Case Direction.Left
                        Return SpriteLeft
                    Case Direction.Right
                        Return SpriteRight
                    Case Direction.UpRight
                        Return SpriteUpRight
                    Case Direction.UpLeft
                        Return SpriteUpLeft
                    Case Direction.DownRight
                        Return SpriteDownRight
                    Case Direction.DownLeft
                        Return SpriteDownLeft
                    Case Else 'Something's wrong
                        Return SpriteDown
                End Select
            End If
        End Get
    End Property

    Public ReadOnly Property MaxHealth As Integer
        Get
            Return BaseMaxHealth + ((Level - 1) * 20) 'Given a base max health of 100, and a level cap of 100, max health is 2080
        End Get
    End Property

    Public ReadOnly Property Level As Integer
        Get
            Return Common.GetLevel(Experience)
        End Get
    End Property

    Public ReadOnly Property Resistance As Decimal
        Get
            Return BaseResistance * Level 'Same level enemies should take damage equal to power of weapon (with added proficiency damage)
        End Get
    End Property
    ''' <summary>
    ''' Returns coordinates for the tile the character is facing.  Does NOT check if the tile actually exists.
    ''' </summary>
    ''' <param name="CurrentLocation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFacingTileLocation(CurrentLocation As System.Drawing.Point) As System.Drawing.Point
        Dim out As System.Drawing.Point
        Select Case FacingDirection
            Case Direction.Up
                out = New Point(CurrentLocation.X, CurrentLocation.Y - 1)
            Case Direction.Down
                out = New Point(CurrentLocation.X, CurrentLocation.Y + 1)
            Case Direction.Left
                out = New Point(CurrentLocation.X - 1, CurrentLocation.Y)
            Case Direction.Right
                out = New Point(CurrentLocation.X + 1, CurrentLocation.Y)
            Case Direction.UpRight
                out = New Point(CurrentLocation.X + 1, CurrentLocation.Y - 1)
            Case Direction.UpLeft
                out = New Point(CurrentLocation.X - 1, CurrentLocation.Y - 1)
            Case Direction.DownRight
                out = New Point(CurrentLocation.X + 1, CurrentLocation.Y + 1)
            Case Direction.DownLeft
                out = New Point(CurrentLocation.X - 1, CurrentLocation.Y + 1)
            Case Else 'Something's wrong
                out = New Point(CurrentLocation.X, CurrentLocation.Y - 1)
        End Select
        Return out
    End Function
    Public ReadOnly Property AILogic As AI
        Get
            Return AI.GetByName(AIName)
        End Get
    End Property
#End Region
#Region "Functions"
    ''' <summary>
    ''' Reloads the given weapon.   Returns the reloaded weapon.
    ''' </summary>
    ''' <param name="Weapon"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReloadWeapon(ByVal Weapon As Weapon) As Weapon
        If Not HeldAmmo.ContainsKey(Weapon.Type) Then
            HeldAmmo.Add(Weapon.Type, 0)
        End If
        If Not Weapon.Magazine = 0 Then
            Dim a As Integer = HeldAmmo(Weapon.Type)
            If a >= Weapon.Magazine Then
                Weapon.CurrentMagazineLoad = Weapon.Magazine
                HeldAmmo(Weapon.Type) -= Weapon.Magazine
            Else 'Can't fully load the gun.
                Weapon.CurrentMagazineLoad = a
                HeldAmmo(Weapon.Type) = 0
            End If
            AttackFrameWait = Weapon.ReloadSpeed
            Return Weapon
        Else
            Return Weapon 'This weapon doesn't use ammo.
        End If
    End Function
    Public Sub ObtainAmmo(WeaponType As WeaponType, Ammo As Integer)
        If HeldAmmo.ContainsKey(WeaponType) Then
            HeldAmmo(WeaponType) = Math.Min(HeldAmmo(WeaponType) + Ammo, WeaponType.MaxHeldAmmo)
        Else
            HeldAmmo.Add(WeaponType, Math.Min(Ammo, WeaponType.MaxHeldAmmo))
        End If
    End Sub
    Public Function GetHeldAmmoCount(WeaponType As WeaponType) As Integer
        If HeldAmmo.ContainsKey(WeaponType) Then
            Return HeldAmmo(WeaponType)
        Else
            Return 0
        End If
    End Function
    Public Function CanUseWeapon(Weapon As Weapon) As Boolean
        Return Level >= Weapon.LevelRequirement - GetProficiency(Weapon.Type).LevelRequirementReduction
    End Function
    ''' <summary>
    ''' Lowers the character's health
    ''' </summary>
    ''' <param name="Damage">Damage to be inflicted; resisted based on level, so same level enemies deal and take equal damage</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TakeDamage(Damage As Integer) As Integer
        If DamageDisplayFrameCount >= DamageDisplayFrameLength Then
            DamageDisplayInitialDamage = CurrentHealth
        End If
        DamageDisplayFrameCount = 0
        CurrentHealth -= Damage / Resistance
        If CurrentHealth < 1 Then
            RaiseEvent Defeated(Me)
        End If
        Return Damage / Resistance
    End Function
    Public Sub RecieveExp(Exp As Integer, WeaponUsed As Weapon)
        Dim CurrentLevel As Integer = Level
        Experience += Exp
        If Not Level = CurrentLevel Then
            RaiseEvent LeveledUp(Me, Level)
        End If
        If Not Proficiency.ContainsKey(WeaponUsed.Type) Then
            Proficiency.Add(WeaponUsed.Type, New WeaponProficiency(0))
        End If
        Dim CurrentProficiencyLevel As Integer = Proficiency(WeaponUsed.Type).Level
        Proficiency(WeaponUsed.Type).Experience += Exp
        If Not Proficiency(WeaponUsed.Type).Level = CurrentProficiencyLevel Then
            RaiseEvent ProficiencyLeveledUp(Me, Proficiency(WeaponUsed.Type).Level, WeaponUsed.Type)
        End If
    End Sub
    Public Function ExpReleasedWhenDefeated() As Integer
        Return BaseMaxHealth 'To be changed in the future
    End Function
    Public Function GetProficiency(WeaponType As WeaponType) As WeaponProficiency
        If Not Proficiency.ContainsKey(WeaponType) Then
            Proficiency.Add(WeaponType, New WeaponProficiency(0))
        End If
        Return Proficiency(WeaponType)
    End Function
    Public Function CalculateDamage(CurrentWeapon As Weapon) As Integer
        If CurrentWeapon.Magazine = 0 OrElse CurrentWeapon.CurrentMagazineLoad > 0 Then
            If Not CurrentWeapon.Magazine = 0 Then CurrentWeapon.CurrentMagazineLoad -= 1
            Dim d As Integer = CurrentWeapon.BaseDamage * Level * GetProficiency(CurrentWeapon.Type).DamageMultiplier
            Return d
        Else
            Throw New Common.NeedsReloadException
        End If
    End Function
#End Region
#Region "Depricated Functions"

    '''' <summary>
    '''' Deals damage to the character DealDamageTo, then returns the damaged character
    '''' </summary>
    '''' <param name="CurrentWeapon">Weapon to deal damage with</param>
    '''' <param name="DealDamageTo"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Function DealDamage(CurrentWeapon As Weapon, DealDamageTo As Character) As Character
    '    DealDamageTo.TakeDamage(CalculateDamage(CurrentWeapon))
    '    Return DealDamageTo
    'End Function
#End Region
#Region "Saving"
    Public Function ToBinary() As Byte()
        Dim outparts As New List(Of Byte())
        Dim e As New System.Text.UnicodeEncoding
        outparts.Add(e.GetBytes(InternalID.ToString))
        outparts.Add(e.GetBytes(Name))
        outparts.Add(SpriteUp.ToBinary)
        outparts.Add(SpriteDown.ToBinary)
        outparts.Add(SpriteLeft.ToBinary)
        outparts.Add(SpriteRight.ToBinary)
        outparts.Add(SpriteUpMoving.ToBinary)
        outparts.Add(SpriteDownMoving.ToBinary)
        outparts.Add(SpriteLeftMoving.ToBinary)
        outparts.Add(SpriteRightMoving.ToBinary)

        outparts.Add(SpriteUpRight.ToBinary)
        outparts.Add(SpriteUpLeft.ToBinary)
        outparts.Add(SpriteDownRight.ToBinary)
        outparts.Add(SpriteDownLeft.ToBinary)
        outparts.Add(SpriteUpRightMoving.ToBinary)
        outparts.Add(SpriteUpLeftMoving.ToBinary)
        outparts.Add(SpriteDownRightMoving.ToBinary)
        outparts.Add(SpriteDownLeftMoving.ToBinary)

        Dim weaponList As New List(Of Byte())
        For Each item In Weapons
            weaponList.Add(item.ToBinary)
        Next
        outparts.Add(FileFormatLib.FileFormatLib.ToBinary(weaponList))

        outparts.Add(BitConverter.GetBytes(BaseMaxHealth))
        outparts.Add(BitConverter.GetBytes(FacingDirection))
        outparts.Add(BitConverter.GetBytes(CurrentHealth))
        outparts.Add(BitConverter.GetBytes(CType(BaseResistance, Double)))
        outparts.Add(BitConverter.GetBytes(Experience))

        Dim ammoParts As New List(Of Byte())
        For Each key As WeaponType In HeldAmmo.Keys
            Dim p As Byte() = BitConverter.GetBytes(key.ID)
            Array.Resize(p, p.Length + 4)
            Dim val As Byte() = BitConverter.GetBytes(HeldAmmo(key))
            For count As Integer = 0 To 3
                p(count + 4) = val(count)
            Next
            ammoParts.Add(p)
        Next
        outparts.Add(FileFormatLib.FileFormatLib.ToBinary(ammoParts))

        Dim profParts As New List(Of Byte())
        For Each key As WeaponType In Proficiency.Keys
            Dim p As Byte() = BitConverter.GetBytes(key.ID)
            Array.Resize(p, p.Length + 4)
            Dim val As Byte() = Proficiency(key).ToBinary
            For count As Integer = 0 To 3
                p(count + 4) = val(count)
            Next
            profParts.Add(p)
        Next
        outparts.Add(FileFormatLib.FileFormatLib.ToBinary(profParts))

        outparts.Add(e.GetBytes(AIName))
        outparts.Add(BitConverter.GetBytes(CurrentWeaponIndex))
        outparts.Add(HeadShot.ToBinary)

        Return FileFormatLib.FileFormatLib.ToBinary(outparts)
    End Function
    Public Shared Function FromBinary(Binary As Byte()) As Character
        Dim out As New Character
        Dim inparts As List(Of Byte()) = FileFormatLib.FileFormatLib.FromBinary(Binary)
        Dim e As New System.Text.UnicodeEncoding
        'out.InternalID = Guid.Parse(e.GetString(inparts(0)))
        out.Name = e.GetString(inparts(1))
        out.SpriteUp = AnimatedSprite.FromBinary(inparts(2))
        out.SpriteDown = AnimatedSprite.FromBinary(inparts(3))
        out.SpriteLeft = AnimatedSprite.FromBinary(inparts(4))
        out.SpriteRight = AnimatedSprite.FromBinary(inparts(5))
        out.SpriteUpMoving = AnimatedSprite.FromBinary(inparts(6))
        out.SpriteDownMoving = AnimatedSprite.FromBinary(inparts(7))
        out.SpriteLeftMoving = AnimatedSprite.FromBinary(inparts(8))
        out.SpriteRightMoving = AnimatedSprite.FromBinary(inparts(9))
        out.SpriteUpRight = AnimatedSprite.FromBinary(inparts(10))
        out.SpriteUpLeft = AnimatedSprite.FromBinary(inparts(11))
        out.SpriteDownRight = AnimatedSprite.FromBinary(inparts(12))
        out.SpriteDownLeft = AnimatedSprite.FromBinary(inparts(13))
        out.SpriteUpRightMoving = AnimatedSprite.FromBinary(inparts(14))
        out.SpriteUpLeftMoving = AnimatedSprite.FromBinary(inparts(15))
        out.SpriteDownRightMoving = AnimatedSprite.FromBinary(inparts(16))
        out.SpriteDownLeftMoving = AnimatedSprite.FromBinary(inparts(17))

        Dim weaponList As List(Of Byte()) = FileFormatLib.FileFormatLib.FromBinary(inparts(18))
        For Each item In weaponList
            out.Weapons.Add(Weapon.FromBinary(item))
        Next

        out.BaseMaxHealth = BitConverter.ToInt32(inparts(19), 0)
        out.FacingDirection = BitConverter.ToInt32(inparts(20), 0)
        out.CurrentHealth = BitConverter.ToInt32(inparts(21), 0)
        out.BaseResistance = BitConverter.ToDouble(inparts(22), 0)
        out.Experience = BitConverter.ToInt32(inparts(23), 0)

        Dim ammoParts As List(Of Byte()) = FileFormatLib.FileFormatLib.FromBinary(inparts(24))
        For Each ammo In ammoParts
            out.HeldAmmo.Add(WeaponType.GetWeaponByID(BitConverter.ToInt32(ammo, 0)), BitConverter.ToInt32(ammo, 4))
        Next

        Dim profParts As List(Of Byte()) = FileFormatLib.FileFormatLib.FromBinary(inparts(25))
        For Each prof In profParts
            out.Proficiency.Add(WeaponType.GetWeaponByID(BitConverter.ToInt32(prof, 0)), New WeaponProficiency(BitConverter.ToInt32(prof, 4)))
        Next

        out.AIName = e.GetString(inparts(26))
        out.CurrentWeaponIndex = BitConverter.ToInt32(inparts(27), 0)
        out.HeadShot = TileTemplate.FromBinary(inparts(28))

        Return out
    End Function
#End Region
    Public Function DebugStats() As String
        Return "Name: " & Name & vbCrLf & _
                "Max Health: " & MaxHealth & vbCrLf & _
                "Current Health: " & CurrentHealth & vbCrLf & _
                "Resistance: " & Resistance & vbCrLf & _
                "Level: " & Level & vbCrLf & _
                "Experience: " & Experience
    End Function
    Public Sub New()
        MyBase.New()
        InternalID = Guid.NewGuid
    End Sub
End Class
Public Class Bullet
    Public Property FiringWeapon As Weapon
    Public Property Location As Point
    Public Property FacingDirection As Direction
    Public ReadOnly Property Sprite As AnimatedSprite
        Get
            Select Case FacingDirection
                Case Direction.Up
                    Return FiringWeapon.BulletUp
                Case Direction.Down
                    Return FiringWeapon.BulletDown
                Case Direction.Left
                    Return FiringWeapon.BulletLeft
                Case Direction.Right
                    Return FiringWeapon.BulletRight
                Case Direction.UpRight
                    Return FiringWeapon.BulletUpRight
                Case Direction.UpLeft
                    Return FiringWeapon.BulletUpLeft
                Case Direction.DownRight
                    Return FiringWeapon.BulletDownRight
                Case Direction.DownLeft
                    Return FiringWeapon.BulletDownLeft
                Case Else 'Something's wrong
                    Return FiringWeapon.BulletRight
            End Select
        End Get
    End Property
    Dim _tilestraveled As Decimal = 0
    Public Property TilesTraveled As Integer
        Get
            Return Math.Truncate(_tilestraveled)
        End Get
        Set(value As Integer)
            If FacingDirection = Direction.UpRight OrElse FacingDirection = Direction.UpLeft OrElse FacingDirection = Direction.DownRight OrElse FacingDirection = Direction.DownLeft Then
                _tilestraveled = value / 2
            Else
                _tilestraveled = value
            End If
        End Set
    End Property
    Public Property SpriteX As Integer = 0
    Public Property SpriteY As Integer = 0
    Public Property SpriteXFinal As Integer = 0
    Public Property SpriteYFinal As Integer = 0
    Public ReadOnly Property SpriteXStep As Integer
        Get
            Return Map.CharacterMovementStep * FiringWeapon.BulletSpeed
        End Get
    End Property
    Public readonly Property SpriteYStep As Integer 
        Get
            Return Map.CharacterMovementStep * FiringWeapon.BulletSpeed
        End Get
    End Property
    Public Property Damage As Integer
End Class
Public Class WarpPoint
    Public Property NewMapName As String
    Public Property NewPoint As Point
    Public Overrides Function ToString() As String
        Return NewMapName & "?" & NewPoint.X & "," & NewPoint.Y
    End Function
    Public Shared Function FromString(Str As String) As WarpPoint
        Dim out As New WarpPoint
        Dim parts As String() = Str.Split("?")
        out.NewMapName = parts(0)
        out.NewPoint = New Point(parts(1).Split(",")(0), parts(1).Split(",")(1))
        Return out
    End Function
    Public Sub New(Map As String, EntryPoint As Point)
        NewMapName = Map
        NewPoint = EntryPoint
    End Sub
    Public Sub New()

    End Sub
End Class
Public Class Item
    Public Enum ItemType
        Weapon
        Ammo
    End Enum
End Class