Imports BankingSystem
Imports Microsoft.EntityFrameworkCore

Public Class CustomerService
    Implements ICustomerService

    Private ReadOnly _db As AppDbCtx
    Public Const IDNumberExistError = "The ID number already exist"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="db"></param>
    Public Sub New(db As AppDbCtx)
        _db = db
    End Sub

    ''' <summary>
    ''' Check if the ID number already exist
    ''' </summary>
    ''' <param name="idNumber"></param>
    ''' <returns></returns>
    Public Async Function IDNumberExistAsnyc(idNumber As String) As Task(Of Boolean) Implements ICustomerService.IDNumberExistAsnyc
        Return Await _db.Customers.AnyAsync(Function(x) x.IDNumber = idNumber)
    End Function

    ''' <summary>
    ''' Get a customer by id
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Async Function GetAsync(id As Integer) As Task(Of Customer) Implements ICustomerService.GetAsync
        Return Await _db.Customers.FirstOrDefaultAsync(Function(x) x.Id = id)
    End Function

    ''' <summary>
    ''' Create a customer
    ''' </summary>
    ''' <param name="customer"></param>
    ''' <returns></returns>
    Public Async Function CreateAsync(customer As Customer) As Task Implements ICustomerService.CreateAsync
        Dim idNumberExist = Await IDNumberExistAsnyc(customer.IDNumber)
        If idNumberExist Then
            Throw New FriendException(IDNumberExistError)
        End If
        _db.Customers.Add(customer)
        Await _db.SaveChangesAsync()
    End Function

    ''' <summary>
    ''' Delete customers
    ''' </summary>
    ''' <param name="ids"></param>
    ''' <returns></returns>
    Public Async Function DeleteAsync(ids As List(Of Integer)) As Task Implements ICustomerService.DeleteAsync
        Dim customers = Await (From customer In _db.Customers
                               Where ids.Contains(customer.Id)).ToListAsync()
        _db.Customers.RemoveRange(customers)
        Await _db.SaveChangesAsync()
    End Function

    ''' <summary>
    ''' Update a customer
    ''' </summary>
    ''' <param name="updated"></param>
    ''' <returns></returns>
    Public Async Function UpdateAsync(updated As Customer) As Task Implements ICustomerService.UpdateAsync
        Dim customer = Await GetAsync(updated.Id)
        If customer.IDNumber <> updated.IDNumber Then
            If Await IDNumberExistAsnyc(updated.IDNumber) Then
                Throw New FriendException(IDNumberExistError)
            End If
        End If
        _db.DetachEntity(updated, updated.Id)
        _db.Update(updated)
        Await _db.SaveChangesAsync()
    End Function

    ''' <summary>
    ''' Get all bank accounts of the given customer
    ''' </summary>
    ''' <param name="customerId"></param>
    ''' <returns></returns>
    Public Async Function GetAccounts(customerId As Integer) As Task(Of IList(Of BankAccount)) Implements ICustomerService.GetAccounts
        Return Await _db.BankAccounts.Where(Function(x) x.CustomerId = customerId).ToListAsync()
    End Function

    ''' <summary>
    ''' Get all customers
    ''' </summary>
    ''' <returns></returns>
    Public Async Function GetAllAsync() As Task(Of List(Of Customer)) Implements ICustomerService.GetAllAsync
        Return Await _db.Customers.ToListAsync()
    End Function

    ''' <summary>
    ''' Get customer by id number
    ''' </summary>
    ''' <param name="idNumber"></param>
    ''' <returns></returns>
    Public Async Function GetAsync(idNumber As String) As Task(Of Customer) Implements ICustomerService.GetAsync
        Return Await _db.Customers.FirstOrDefaultAsync(Function(x) x.IDNumber = idNumber)
    End Function
End Class