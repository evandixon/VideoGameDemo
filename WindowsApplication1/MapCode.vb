Imports Logic
Public Class MapCode
    Public Class SciCo1
#Region "MapMaking"
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
        Private Function GetMap() As Map
            Dim out As New Map
            out.TileTemplate = New Logic.TileTemplate(System.Drawing.Bitmap.FromFile("C:\Users\Evan\Desktop\scico\template.png"))
            out.Tiles = {DirectCast({t0(), t0(), t0(), t0(), t0(), t0(), t0(), t0(), t0(), t0(), t0()}, Logic.Tile()),
                         DirectCast({t0(), t0(), t0(), t8(), t1(), t1(), t1(), t3(), t0(), t0(), t0()}, Logic.Tile()),
                         DirectCast({t0(), t0(), t0(), t8(), t1(), t1(), t1(), t3(), t0(), t0(), t0()}, Logic.Tile()),
                         DirectCast({t0(), t0(), t0(), t8(), t1(), t1(), t1(), t3(), t0(), t0(), t0()}, Logic.Tile()),
                         DirectCast({t0(), t0(), t0(), t8(), t1(), t1(), t1(), t3(), t0(), t0(), t0()}, Logic.Tile()),
                         DirectCast({t0(), t0(), t0(), t8(), t1(), t1(), t1(), t3(), t0(), t0(), t0()}, Logic.Tile()),
                         DirectCast({t6(), t5(), t5(), tA(), t1(), t1(), t1(), t9(), t5(), t5(), t7()}, Logic.Tile()),
                         DirectCast({t8(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t3()}, Logic.Tile()),
                         DirectCast({t8(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t3()}, Logic.Tile()),
                         DirectCast({t8(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t3()}, Logic.Tile()),
                         DirectCast({t8(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t3()}, Logic.Tile()),
                         DirectCast({t2(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t1(), t4()}, Logic.Tile()),
                         DirectCast({t0(), t0(), t0(), t8(), t1(), t1(), t1(), t3(), t0(), t0(), t0()}, Logic.Tile()),
                         DirectCast({t0(), t0(), t0(), t8(), t1(), t1(), t1(), t3(), t0(), t0(), t0()}, Logic.Tile()),
                         DirectCast({t0(), t0(), t0(), t8(), t1(), t1(), t1(), t3(), t0(), t0(), t0()}, Logic.Tile()),
                         DirectCast({t0(), t0(), t0(), t8(), t1(), t1(), t1(), t3(), t0(), t0(), t0()}, Logic.Tile())}
            'out.Tiles(4)(3).ContainedCharacter = Character.FromBinary(IO.File.ReadAllBytes("c:\Users\Evan\Desktop\Char1\Char.bin"))
            'out.Tiles(4)(3).ContainedCharacter.AIName = "StationarySingleWeaponRangedAttack"

            'out.Tiles(4)(3).ContainedCharacter.Weapons.Add(Weapon.FromBinary(IO.File.ReadAllBytes("C:\users\evan\desktop\char1\ssi1.weapon")))
            'out.Tiles(4)(3).ContainedCharacter.Weapons(0).BaseDamage = 10

            'out.Tiles(8)(3).ContainedCharacter = Character.FromBinary(IO.File.ReadAllBytes("c:\Users\Evan\Desktop\Char1\Char.bin"))
            'out.Tiles(8)(3).ContainedCharacter.AIName = "StationarySingleWeaponRangedAttack"

            'out.Tiles(8)(3).ContainedCharacter.Weapons.Add(Weapon.FromBinary(IO.File.ReadAllBytes("C:\users\evan\desktop\char1\ssi1.weapon")))
            'out.Tiles(8)(3).ContainedCharacter.Weapons(0).BaseDamage = 10

            IO.File.WriteAllBytes(IO.Path.Combine(Environment.CurrentDirectory, "Maps", "scico1.map"), out.ToBinary)
            Return out
        End Function
#End Region
    End Class
End Class
