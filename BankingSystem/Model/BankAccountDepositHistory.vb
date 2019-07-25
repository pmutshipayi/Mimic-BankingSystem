Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Public Class BankAccountDepositHistory : Inherits Entity
    ''' <summary>
    ''' Get or set the account id
    ''' </summary>
    ''' <returns></returns>
    <Required>
    Public Property AccountId As Integer

    ''' <summary>
    ''' Get or set the amount to deposit
    ''' </summary>
    ''' <returns></returns>
    <Required>
    Public Property Amount As Double

    ''' <summary>
    ''' Get or set the deposit reference
    ''' </summary>
    ''' <returns></returns>
    Public Property Reference As String

    ''' <summary>
    ''' Get or set the date and time the money has been deposit
    ''' </summary>
    ''' <returns></returns>
    <Required>
    Public Property DateDeposed As Date = Date.Now

    ''' <summary>
    ''' Get or set the bank account
    ''' </summary>
    ''' <returns></returns>
    <ForeignKey(NameOf(AccountId))>
    Public Property Account As BankAccount
End Class