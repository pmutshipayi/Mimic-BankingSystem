Imports System.ComponentModel.DataAnnotations

Public Class AccountWithdrawHistory : Inherits Entity
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    <Required>
    Public Property AccountId As Integer

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    <Required>
    Public Property Amount As Double

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property DateWithdraw As Date = Date.Now
End Class