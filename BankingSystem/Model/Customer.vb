Imports System.ComponentModel.DataAnnotations.Schema

Public Class Customer : Inherits Person
    ''' <summary>
    ''' Get or set wether the customer account is not active
    ''' </summary>
    ''' <returns></returns>
    Public Property IsActive As Boolean = True

    ''' <summary>
    ''' Banks account for the customers
    ''' </summary>
    ''' <returns></returns>
    Public Property Accounts As ICollection(Of BankAccount)

    ''' <summary>
    ''' Get or set total accounts for the customer
    ''' </summary>
    ''' <returns></returns>
    <NotMapped>
    Public Property CountAccounts As Integer
End Class