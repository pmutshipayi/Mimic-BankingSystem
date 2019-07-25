Imports System.ComponentModel.DataAnnotations
Imports System.Threading
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.ChangeTracking

Public Class AppDbCtx : Inherits DbContext

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dbContext"></param>
    Public Sub New(ByVal dbContext As DbContextOptions(Of AppDbCtx))
        MyBase.New(dbContext)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub Validate()
        Dim entities = From e In ChangeTracker.Entries
                       Where e.State = EntityState.Added Or e.State = EntityState.Modified
                       Select e.Entity
        For Each entity In entities
            Dim validationCtx = New ValidationContext(entity)
            Validator.ValidateObject(entity, validationCtx, True)
        Next
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Function SaveChanges() As Integer
        Validate()
        Return MyBase.SaveChanges()
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cancellationToken"></param>
    ''' <returns></returns>
    Public Overrides Function SaveChangesAsync(Optional cancellationToken As CancellationToken = Nothing) As Task(Of Integer)
        Validate()
        Return MyBase.SaveChangesAsync(cancellationToken)
    End Function

    Public Property Users As DbSet(Of User)
    Public Property UserPermissions As DbSet(Of UserPermission)
    Public Property Customers As DbSet(Of Customer)
    Public Property BankAccounts As DbSet(Of BankAccount)
    Public Property DepositHistory As DbSet(Of BankAccountDepositHistory)
    Public Property AccountWithdraw As DbSet(Of AccountWithdrawHistory)
End Class