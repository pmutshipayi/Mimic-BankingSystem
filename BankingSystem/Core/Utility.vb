Public Module Utility
    ''' <summary>
    ''' Generate random numbers
    ''' </summary>
    ''' <param name="length"></param>
    ''' <returns></returns>
    Public Function GenerateNumber(length As Integer) As String
        Return GenerateStrings("0123456789", length)
    End Function

    Public Function GenerateStrings(allowedChars As String, ByVal stringLength As Integer) As String
        Dim chars As Char() = New Char(stringLength - 1) {}
        Dim rd = New Random()
        For i As Integer = 0 To stringLength - 1
            chars(i) = allowedChars(rd.[Next](0, allowedChars.Length))
        Next
        Return New String(chars)
    End Function
End Module
