Imports System.ComponentModel.DataAnnotations

Public Class Person : Inherits Entity
    ''' <summary>
    ''' Get or set the first name
    ''' </summary>
    ''' <returns></returns>
    <Required(ErrorMessage:="The first name is requried")>
    Public Property FirstName As String

    ''' <summary>
    ''' Get or set the last name
    ''' </summary>
    ''' <returns></returns>
    <Required(ErrorMessage:="The last name is required")>
    Public Property LastName As String

    ''' <summary>
    ''' Get or set the customer ID number
    ''' </summary>
    ''' <returns></returns>
    Public Property IDNumber As String

    ''' <summary>
    ''' Get or set the email address
    ''' </summary>
    ''' <returns></returns>
    <Required(ErrorMessage:="The email address is required")>
    <EmailAddress(ErrorMessage:="The email address provided is invalid")>
    Public Property Email As String

    ''' <summary>
    ''' Get or set the phone number
    ''' </summary>
    ''' <returns></returns>
    Public Property Phone As String

    ''' <summary>
    ''' Get or set the cellphone number
    ''' </summary>
    ''' <returns></returns>
    Public Property CellPhone As String

    ''' <summary>
    ''' Get or set the physical address
    ''' </summary>
    ''' <returns></returns>
    Public Property Address As String

    ''' <summary>
    ''' Get or set the date of birth
    ''' </summary>
    ''' <returns></returns>
    Public Property DateOfBirth As Date

    ''' <summary>
    ''' Get or set the id of employee who create this customer
    ''' </summary>
    ''' <returns></returns>
    Public Property CreatedByUserId As Integer?
End Class
