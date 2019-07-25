Imports System.ComponentModel.DataAnnotations

Public Class Entity : Implements ICloneable
    <Key>
    Public Property Id As Integer

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function Clone() As Object Implements ICloneable.Clone
        Return MemberwiseClone()
    End Function
End Class
