Imports System.ComponentModel.DataAnnotations

Public Class User : Inherits Person
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    <Required>
    <MinLength(6, ErrorMessage:="The password is too short")>
    Public Property Password As String

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property Permissions As ICollection(Of UserPermission)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property DateCreated As Date = Date.Now
End Class