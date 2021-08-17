Public Class SaveFile
    Const DataDir = "data"
    Public Property EventVariables As New FileFormatLib.SaveableStringDictionary
    Public Function GetEventVariable(Key As String) As Object
        If EventVariables.ContainsKey(Key) Then
            Return EventVariables(Key)
        Else
            Return Nothing
        End If
    End Function
    Public Sub SetEventVariable(Key As String, Value As Object)
        If EventVariables.ContainsKey(Key) Then
            EventVariables(Key) = Value
        Else
            EventVariables.Add(Key, Value)
        End If
    End Sub
    Public Property MapName As String
    Public Property Location As Drawing.Point
    Public Property Player As Character
    Public Property Money As Integer
    Public Function ToBinary() As Byte()
        Throw New NotImplementedException
    End Function
    Public Shared Function FromBinary(Binary As Byte()) As SaveFile
        Throw New NotImplementedException
    End Function
    Public Shared Function GetLastSaveFile() As SaveFile
        Dim out As New SaveFile
        out.EventVariables = New FileFormatLib.SaveableStringDictionary
        out.MapName = "Map1"
        out.Location = New Drawing.Point(1, 1)
        out.Money = 0
        Dim c As Character = Character.FromBinary(IO.File.ReadAllBytes(DataDir & "\Char1\Char.bin"))
        'c.Weapons.Add(Weapon.FromBinary(IO.File.ReadAllBytes("C:\Users\Evan\Dropbox\LogicEngine\data\char1\ssi1.weapon")))
        out.Player = c
        Return out
    End Function

    Public Shared Property CurrentSaveFile As SaveFile = GetLastSaveFile()
End Class
