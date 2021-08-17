Public Class SaveableStringDictionary
    Inherits Dictionary(Of String, String)
    Public Function ToBinary() As Byte()
        Dim e As New System.Text.UnicodeEncoding
        Dim list As New List(Of Byte())
        For Each key In Me.Keys
            list.Add(e.GetBytes(key.Replace("&", "&amp;").Replace("ÿ", "&255;") & "ÿ" & Me.Item(key).Replace("ÿ", "&255;").Replace("&", "&amp;")))
        Next
        Return FileFormatLib.ToBinary(list)
    End Function
    Public Overrides Function Equals(obj As Object) As Boolean
        If TypeOf obj Is SaveableStringDictionary Then
            Array.Sort(Keys.ToArray)
            Dim keys2 As String() = DirectCast(obj, SaveableStringDictionary).Keys.ToArray
            Array.Sort(keys2)
            If Keys.Count = keys2.Count Then
                For Count As Integer = 0 To Keys.Count - 1
                    If Not Keys(Count) = keys2(Count) Then
                        Return False
                        Exit Function
                    End If
                Next
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function
    Public Shared Function FromBinary(Binary As Byte()) As SaveableStringDictionary
        Dim e As New System.Text.UnicodeEncoding
        Dim list As List(Of Byte()) = FileFormatLib.FromBinary(Binary)
        Dim out As New SaveableStringDictionary
        For Each section In list
            Dim parts() As String = e.GetString(section).Split("ÿ")
            Dim key As String = parts(0).Replace("&255;", "ÿ").Replace("&amp;", "&")
            Dim value As String = parts(1).Replace("&255;", "ÿ").Replace("&amp;", "&")
            If Not out.ContainsKey(key) Then
                out.Add(key, value)
            End If
        Next
        Return out
    End Function
End Class
Public Class FileFormatLib
    Public Shared Function ByteArrToString(ByteArr As Byte()) As String
        Dim out As String = ""
        For Each item In ByteArr
            out += Conversion.Hex(item).PadLeft(2, "0")
        Next
        Return out
    End Function
    ''' <summary>
    ''' Only works with strings made with ByteArrToString()
    ''' </summary>
    ''' <param name="Input"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function StringToByteArr(Input As String) As Byte()
        Dim out() As Byte = {}
        For count As Integer = 0 To Input.Length - 1 Step 2
            Array.Resize(out, out.Length + 1)
            If Input.Length - 1 >= count + 1 Then
                out(out.Length - 1) = Convert.ToByte(Input(count) & Input(count + 1), 16)
            End If
        Next
        Return out
    End Function
    Public Shared Function ToBinary(Sections As List(Of Byte())) As Byte()
        Dim size As UInt64 = 0
        For Each s In Sections
            size += s.LongLength
        Next
        Dim out(Sections.LongCount * 24 + size + 7) As Byte
        Dim overallIndex As Integer = Sections.LongCount * 24 + 8
        Dim i As New IndexCollection
        For sectionNum As Integer = 0 To Sections.LongCount - 1
            Dim s As Byte() = Sections(sectionNum)
            i.Indecies.Add(sectionNum, {overallIndex, s.Length})
            For count As Integer = 0 To s.LongLength - 1
                out(overallIndex) = s(count)
                overallIndex += 1
            Next
        Next
        Dim indexBytes As Byte() = i.ToByteArr
        For count As Integer = 0 To indexBytes.LongLength - 1
            out(count + 8) = indexBytes(count)
        Next
        Dim indexLength As Long = indexBytes.LongLength
        Dim indexLengthBytes As Byte() = BitConverter.GetBytes(indexLength)
        For count As Integer = 0 To 7
            out(count) = indexLengthBytes(count)
        Next
        'Dim outStream As New IO.MemoryStream(out)
        'Dim g As New System.IO.Compression.GZipStream(outStream, IO.Compression.CompressionMode.Compress)
        'Dim compressed As Byte() = {}
        'While True
        '    Dim b As Integer = outStream.ReadByte
        '    If Not b = -1 Then
        '        Array.Resize(compressed, compressed.Length + 1)
        '        compressed(compressed.Length - 1) = b
        '    Else
        '        Exit While
        '    End If
        'End While
        Return out
    End Function
    Public Shared Function SubByteArr(ByteArr As Byte(), Index As Integer, EndPoint As Integer) As Byte()
        Dim output(Math.Max(Math.Min(EndPoint, ByteArr.Length) - Index, 0)) As Byte
        For count As Integer = 0 To output.Length - 1
            output(count) = ByteArr(count + Index)
        Next
        Return output
    End Function
    Shared Function FromBinary(ByteArr As Byte()) As List(Of Byte())
        'Dim instream As New IO.MemoryStream(ByteArr)
        'Dim g As New IO.Compression.GZipStream(instream, IO.Compression.CompressionMode.Decompress)
        Dim binary() As Byte = ByteArr
        'While True
        '    Dim b As Integer = instream.ReadByte
        '    If Not b = -1 Then
        '        Array.Resize(binary, binary.Length + 1)
        '        binary(binary.Length - 1) = b
        '    Else
        '        Exit While
        '    End If
        'End While

        Dim indexLength As Long = BitConverter.ToInt64(binary, 0)
        If indexLength > 0 Then
            Dim i As IndexCollection = IndexCollection.FromByteArr(SubByteArr(binary, 8, indexLength + 8))
            Dim outArr As Byte()() = {}
            Array.Resize(outArr, i.Indecies.Keys.Count)
            For Each key In i.Indecies.Keys
                outArr(key) = SubByteArr(ByteArr, i.Indecies(key)(0), i.Indecies(key)(1) + i.Indecies(key)(0) - 1)
            Next
            Dim outList As New List(Of Byte())
            outList.AddRange(outArr)
            'Dim out As New FileFormat
            'out.Sections = outList
            Return outList
        Else
            Return New List(Of Byte())
        End If
    End Function
End Class
Public Class IndexCollection
    Public Property Indecies As New Dictionary(Of Long, Long())
    Public Function ToByteArr() As Byte()
        Dim out((24 * Indecies.Count) - 1) As Byte
        Dim overallIndex As Integer = 0
        Dim localIndex As Integer = 0
        For Each item In Indecies.Keys
            For Each b In (New Index(item, Indecies(item)(0), Indecies(item)(1))).ToByteArr
                out(overallIndex) = b
                overallIndex += 1
                localIndex += 1
            Next
            localIndex = 0
        Next
        Return out
    End Function
    Public Shared Function FromByteArr(ByteArr As Byte()) As IndexCollection
        Dim out As New IndexCollection
        For count As Integer = 0 To ByteArr.LongLength - 1 Step 24
            If ByteArr.LongLength - 1 >= count + 24 Then
                Dim i As Index = Index.FromByteArr({ByteArr(count), ByteArr(count + 1), ByteArr(count + 2), ByteArr(count + 3), ByteArr(count + 4), ByteArr(count + 5), ByteArr(count + 6), ByteArr(count + 7), ByteArr(count + 8), ByteArr(count + 9), ByteArr(count + 10), ByteArr(count + 11), ByteArr(count + 12), ByteArr(count + 13), ByteArr(count + 14), ByteArr(count + 15), ByteArr(count + 16), ByteArr(count + 17), ByteArr(count + 18), ByteArr(count + 19), ByteArr(count + 20), ByteArr(count + 21), ByteArr(count + 22), ByteArr(count + 23)})
                If Not out.Indecies.ContainsKey(i.SectionIndex) Then
                    out.Indecies.Add(i.SectionIndex, {i.ByteIndex, i.ByteLength})
                End If
            Else 'This line is not a full index, so don't add it
            End If
        Next
        Return out
    End Function
End Class
Public Class Index
    Public Property SectionIndex As Long
    Public Property ByteIndex As Long
    Public Property ByteLength As Long
    Public Sub New(Section As Long, ByteLocation As Long, ByteLeng As Long)
        SectionIndex = Section
        ByteIndex = ByteLocation
        ByteLength = ByteLeng
    End Sub
    Public Function ToByteArr() As Byte()
        Dim out(23) As Byte
        Dim sec() As Byte = BitConverter.GetBytes(SectionIndex)
        Dim byt() As Byte = BitConverter.GetBytes(ByteIndex)
        Dim len() As Byte = BitConverter.GetBytes(ByteLength)
        For count As Byte = 0 To 7
            out(count) = sec(count)
        Next
        For count As Byte = 0 To 7
            out(count + 8) = byt(count)
        Next
        For count As Byte = 0 To 7
            out(count + 16) = len(count)
        Next
        Return out
    End Function
    Public Shared Function FromByteArr(ByteArr As Byte()) As Index
        Return New Index(BitConverter.ToInt64(ByteArr, 0), BitConverter.ToInt64(ByteArr, 8), BitConverter.ToInt32(ByteArr, 16))
    End Function
End Class