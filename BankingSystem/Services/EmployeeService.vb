Imports BankingSystem
Imports Microsoft.EntityFrameworkCore

Public Class EmployeeService
    Implements IEmployeeService

    Public Const EmailUsed = "The email is used by another employee"
    Private ReadOnly _db As AppDbCtx

    Sub New(db As AppDbCtx)
        _db = db
    End Sub

    ''' <summary>
    ''' Check wether the given email address alrady exist
    ''' </summary>
    ''' <param name="email"></param>
    ''' <returns></returns>
    Public Async Function IsEmailExistAsync(email As String) As Task(Of Boolean) Implements IEmployeeService.IsEmailExistAsync
        Return Await _db.Users.AnyAsync(Function(x) x.Email = email)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Async Function GetByIdAsync(id As Integer) As Task(Of User) Implements IEmployeeService.GetByIdAsync
        Return Await _db.Users.FirstOrDefaultAsync(Function(x) x.Id = id)
    End Function

    ''' <summary>
    ''' Create a user account
    ''' </summary>
    ''' <param name="user"></param>
    ''' <returns></returns>
    Public Async Function CreateUserAsync(user As User) As Task Implements IEmployeeService.CreateUserAsync
        If user Is Nothing Then
            Throw New FriendException("Fail to create an employee account")
        End If
        Dim emailExist = Await IsEmailExistAsync(user.Email)
        If emailExist Then
            Throw New FriendException(EmailUsed)
        End If
        _db.Users.Add(user)
        Await _db.SaveChangesAsync()
    End Function

    ''' <summary>
    ''' Delete accounts
    ''' </summary>
    ''' <param name="ids">user ids to deleted</param>
    ''' <returns></returns>
    Public Async Function DeleteAccountsAsync(ids As List(Of Integer)) As Task Implements IEmployeeService.DeleteAccountsAsync
        Dim users = _db.Users.Where(Function(x) ids.Contains(x.Id))
        _db.Users.RemoveRange(users)
        Await _db.SaveChangesAsync()
    End Function

    ''' <summary>
    ''' Update an employee account
    ''' </summary>
    ''' <param name="updated"></param>
    ''' <returns></returns>
    Public Async Function UpdateAsync(updated As User) As Task Implements IEmployeeService.UpdateAsync
        Dim user = Await _db.Users.FirstOrDefaultAsync(Function(x) x.Id = updated.Id)
        If user Is Nothing Then
            Throw New FriendException("The account you trying to update wasn't found")
        End If
        If user.Email <> updated.Email Then
            Dim isEmailUsed = Await IsEmailExistAsync(updated.Email)
            If isEmailUsed Then
                Throw New FriendException(EmailUsed)
            End If
        End If
        _db.DetachEntity(Of User)(updated, updated.Id)
        _db.Users.Update(updated)
        Await _db.SaveChangesAsync()
    End Function

    ''' <summary>
    ''' Set user permissions
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <param name="permissions"></param>
    ''' <returns></returns>
    Public Async Function SetPermissionsAsync(userId As Integer, permissions As List(Of Permission)) As Task Implements IEmployeeService.SetPermissionsAsync
        Dim user = Await _db.Users.Include(Function(x) x.Permissions).FirstOrDefaultAsync(Function(x) x.Id = userId)
        If user Is Nothing Then
            Throw New FriendException("The employee account wasn't found")
        End If
        _db.UserPermissions.RemoveRange(user.Permissions)
        Dim userPermissions = permissions.Select(Function(permission) New UserPermission() With {
            .Permission = permission,
            .UserId = userId
        })

        ' Add permissions

        _db.UserPermissions.AddRange(userPermissions)
        Await _db.SaveChangesAsync()
    End Function

    ''' <summary>
    ''' Login
    ''' </summary>
    ''' <param name="userName"></param>
    ''' <param name="password"></param>
    ''' <returns></returns>
    Public Async Function LoginAsync(userName As String, password As String) As Task(Of Boolean) Implements IEmployeeService.LoginAsync
        If String.IsNullOrWhiteSpace(userName) Then Throw New FriendException("The username is required")
        If String.IsNullOrWhiteSpace(password) Then Throw New FriendException("The password is required")
        Return Await _db.Users.AnyAsync(Function(x) x.Email = userName Or x.IDNumber = userName And x.Password = password)
    End Function

    ''' <summary>
    ''' Get all employees
    ''' </summary>
    ''' <returns></returns>
    Public Async Function GetAllAsync() As Task(Of List(Of User)) Implements IEmployeeService.GetAllAsync
        Return Await _db.Users.ToListAsync()
    End Function

    ''' <summary>
    ''' Get a user by username (email or id numer)
    ''' </summary>
    ''' <param name="username"></param>
    ''' <returns></returns>
    Public Async Function GetAsync(username As String) As Task(Of User) Implements IEmployeeService.GetAsync
        Return Await _db.Users.FirstOrDefaultAsync(Function(x) x.Email = username Or x.IDNumber = username)
    End Function
End Class