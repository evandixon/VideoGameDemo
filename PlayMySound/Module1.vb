Module Module1
    Sub Main()
        Dim args As String() = Environment.GetCommandLineArgs
        If args.Length > 2 Then
            If IO.File.Exists(args(1)) Then
                Sound.PlaySound(args(1), Nothing, Sound.SND_Loop + Sound.SND_ASYNC)
                Process.GetProcessById(args(2)).WaitForExit()
            End If
        End If
    End Sub
End Module
Public Class Sound
    Declare Auto Function PlaySound Lib "winmm.dll" (ByVal name _
      As String, ByVal hmod As Integer, ByVal flags As Integer) As Integer

    Declare Auto Function PlaySound Lib "winmm.dll" (ByVal name _
      As Byte(), ByVal hmod As Integer, ByVal flags As Integer) As Integer

    Public Const SND_SYNC = &H0 ' play synchronously 
    Public Const SND_ASYNC = &H1 ' play asynchronously 
    Public Const SND_MEMORY = &H4  'Play wav in memory
    Public Const SND_Loop = &H8
    Public Const SND_ALIAS = &H10000 'Play system alias wav 
    Public Const SND_NODEFAULT = &H2
    Public Const SND_FILENAME = &H20000 ' name is file name 
    Public Const SND_RESOURCE = &H40004 ' name is resource name or atom 
    Public Function PlaySoundLoop(name As String)
        Return PlaySound(name, Nothing, SND_Loop)
    End Function
    Public Function PlaySoundSync(name As String)
        Return PlaySound(name, Nothing, SND_SYNC)
    End Function
End Class