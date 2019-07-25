Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Public Class UserPermission : Inherits Entity
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    <Required>
    Public Property Permission As Permission

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property UserId As Integer

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    <ForeignKey(NameOf(UserId))>
    Public Property User As User
End Class
