Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Public Class BankAccount : Inherits Entity
    ''' <summary>
    ''' Get the id of the customer owner of the account
    ''' </summary>
    ''' <returns></returns>
    <Required(ErrorMessage:="You must specify the account holder")>
    Public Property CustomerId As Integer

    ''' <summary>
    ''' Get or set the name of the account
    ''' </summary>
    ''' <returns></returns>
    Public Property AccountName As String

    ''' <summary>
    ''' Get or set the account number
    ''' </summary>
    ''' <returns></returns>
    <Required>
    Public Property AccountNumber As String

    ''' <summary>
    ''' Get the date and time the account was created
    ''' </summary>
    ''' <returns></returns>
    Public Property DateCreated As Date = Date.Now

    ''' <summary>
    ''' Get or set the balance
    ''' </summary>
    ''' <returns></returns>
    <Required>
    Public Property Balance As Double

    ''' <summary>
    ''' Get or set wether the account is active
    ''' </summary>
    ''' <returns></returns>
    Public Property IsActive As Boolean = True

    ''' <summary>
    ''' Get the customer owner of the account
    ''' </summary>
    ''' <returns></returns>
    <ForeignKey(NameOf(CustomerId))>
    Public Property Customer As Customer
End Class