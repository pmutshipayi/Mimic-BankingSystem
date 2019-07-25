Imports BankingSystem
Imports Microsoft.EntityFrameworkCore

Public Class BankAccountService
    Implements IBankAccountService

    ''' <summary>
    ''' The database contex 
    ''' </summary>
    Private ReadOnly _db As AppDbCtx

    ''' <summary>
    ''' The customer service class
    ''' </summary>
    Private ReadOnly _customerService As ICustomerService

    ''' <summary>
    ''' Error message the account holder is found
    ''' </summary>
    Public Const AccountHolderNotFound = "The account holder wasn't found"

    ''' <summary>
    ''' Error message the account has been suspended
    ''' </summary>
    Public Const AccountHolderSuspended = "The account holder has been suspended"

    ''' <summary>
    '''  Error message, when the amount to be deposit is invalid
    ''' </summary>
    Public Const InvalidDepositAmount = "The amount to be deposited is invalid"

    ''' <summary>
    ''' Error message when the bank account is suspended
    ''' </summary>
    Public Const AccountSuspended = "This account has been suspended"

    ''' <summary>
    ''' Error message when the amount to be withdraw is more than the account balance
    ''' </summary>
    Public Const AmountToWithdrawMoreThanBalance = "The balance is lower than the amount you want to withdraw"

    Public Sub New(db As AppDbCtx, customerService As ICustomerService)
        _db = db
        _customerService = customerService
    End Sub

    ''' <summary>
    ''' Activate or suspend accounts
    ''' </summary>
    ''' <param name="accounts"></param>
    ''' <param name="activate"></param>
    ''' <returns></returns>
    Private Async Function ActiveOrSuspendAsync(accounts As IEnumerable(Of BankAccount), activate As Boolean) As Task
        If accounts.Any() = False Then Return
        For Each account In accounts
            account.IsActive = activate
            _db.DetachEntity(account, account.Id)
        Next
        _db.UpdateRange(accounts)
        Await _db.SaveChangesAsync()
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="accountId"></param>
    ''' <returns></returns>
    Private Async Function GetForDepositOrWithdraw(accountId As Integer) As Task(Of BankAccount)
        Dim account = Await _db.BankAccounts.Include(Function(x) x.Customer).FirstOrDefaultAsync(Function(x) x.Id = accountId)
        If account Is Nothing Then Throw New FriendException("Account not found")
        If account.IsActive = False Then Throw New FriendException(AccountSuspended)
        If account.Customer.IsActive = False Then Throw New FriendException(AccountHolderSuspended)
        Return account
    End Function

    ''' <summary>
    ''' Get wether the given account number exist
    ''' </summary>
    ''' <param name="accountNumber"></param>
    ''' <returns></returns>
    Public Async Function AccountNumberExistAsync(accountNumber As String) As Task(Of Boolean) Implements IBankAccountService.AccountNumberExistAsync
        Return Await _db.BankAccounts.AnyAsync(Function(x) x.AccountNumber = accountNumber)
    End Function

    ''' <summary>
    ''' Get a account by id
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Async Function GetAsync(id As Integer) As Task(Of BankAccount) Implements IBankAccountService.GetAsync
        Return Await _db.BankAccounts.FirstOrDefaultAsync(Function(x) x.Id = id)
    End Function

    ''' <summary>
    ''' Get bank accounts by id
    ''' </summary>
    ''' <param name="ids"></param>
    ''' <returns></returns>
    Public Async Function GetAsync(ids As IEnumerable(Of Integer)) As Task(Of IList(Of BankAccount)) Implements IBankAccountService.GetAsync
        Return Await (From account In _db.BankAccounts
                      Where ids.Contains(account.Id)).ToListAsync()
    End Function

    ''' <summary>
    ''' Create a bank account
    ''' </summary>
    ''' <param name="bankAccount"></param>
    ''' <returns></returns>
    Public Async Function CreateAsync(bankAccount As BankAccount) As Task Implements IBankAccountService.CreateAsync
        Dim customer = Await _customerService.GetAsync(bankAccount.CustomerId)
        If customer Is Nothing Then
            Throw New FriendException(AccountHolderNotFound)
        End If
        If customer.IsActive <> True Then
            Throw New FriendException(AccountHolderSuspended)
        End If
        Dim accountNumber As String = GenerateNumber(16)
        If Await AccountNumberExistAsync(accountNumber) = True Then
            Throw New FriendException("We failed to open an account, try again later")
        End If
        bankAccount.AccountNumber = accountNumber
        _db.BankAccounts.Add(bankAccount)
        Await _db.SaveChangesAsync()
    End Function

    ''' <summary>
    ''' Delete bank accounts
    ''' </summary>
    ''' <param name="ids"></param>
    ''' <returns></returns>
    Public Async Function DeleteAsync(ids As List(Of Integer)) As Task Implements IBankAccountService.DeleteAsync
        Dim accounts = Await GetAsync(ids)
        _db.RemoveRange(accounts)
        Await _db.SaveChangesAsync()
    End Function

    ''' <summary>
    ''' Suspend bank accounts
    ''' </summary>
    ''' <param name="ids"></param>
    ''' <returns></returns>
    Public Async Function SuspendAsync(ids As List(Of Integer)) As Task Implements IBankAccountService.SuspendAsync
        Dim accounts = Await GetAsync(ids)
        Await ActiveOrSuspendAsync(accounts, False)
    End Function

    ''' <summary>
    ''' Active bank accounts
    ''' </summary>
    ''' <param name="ids"></param>
    ''' <returns></returns>
    Public Async Function ActivateAsync(ids As List(Of Integer)) As Task Implements IBankAccountService.ActivateAsync
        Dim accounts = Await GetAsync(ids)
        Await ActiveOrSuspendAsync(accounts, True)
    End Function

    ''' <summary>
    ''' Deposit money in an account
    ''' </summary>
    ''' <param name="accountId"></param>
    ''' <param name="amount"></param>
    ''' <returns></returns>
    Public Async Function DepositeAsync(accountId As Integer, amount As Double) As Task Implements IBankAccountService.DepositeAsync
        Dim account = Await GetForDepositOrWithdraw(accountId)
        If amount <= 0 Then Throw New FriendException(InvalidDepositAmount)

        _db.DetachEntity(account, account.Id)
        account.Balance += amount
        _db.BankAccounts.Update(account)

        ' Save history

        Dim history = New BankAccountDepositHistory() With {
            .Amount = amount,
            .AccountId = accountId
        }
        _db.Add(history)
        Await _db.SaveChangesAsync()
    End Function

    ''' <summary>
    ''' Withdraw money in an account
    ''' </summary>
    ''' <param name="accountId"></param>
    ''' <param name="amount"></param>
    ''' <returns></returns>
    Public Async Function WithdrawAsync(accountId As Integer, amount As Integer) As Task Implements IBankAccountService.WithdrawAsync
        Dim account = Await GetForDepositOrWithdraw(accountId)
        If amount > account.Balance Then Throw New FriendException(AmountToWithdrawMoreThanBalance)
        If amount <= 0 Then Throw New FriendException("Invalid amount")
        account.Balance -= amount
        _db.DetachEntity(account, account.Id)
        _db.Update(account)
        Await _db.SaveChangesAsync()
    End Function

    ''' <summary>
    ''' Get all bank accounts for a given customer
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <returns></returns>
    Public Async Function GetAllAccountOfUserAsync(userId As Integer) As Task(Of List(Of BankAccount)) Implements IBankAccountService.GetAllAccountOfUserAsync
        Return Await _db.BankAccounts.Where(Function(x) x.CustomerId = userId).ToListAsync()
    End Function

    ''' <summary>
    ''' Get all deposits history for the given account
    ''' </summary>
    ''' <param name="accountId"></param>
    ''' <returns></returns>
    Public Async Function GetAllDepositsAsync(accountId As Integer) As Task(Of List(Of BankAccountDepositHistory)) Implements IBankAccountService.GetAllDepositsAsync
        Return Await _db.DepositHistory.Where(Function(x) x.AccountId = accountId).ToListAsync()
    End Function

    ''' <summary>
    ''' Get all withdraw of the given account
    ''' </summary>
    ''' <param name="accountId"></param>
    ''' <returns></returns>
    Public Async Function GetAllWithdrawAsync(accountId As Integer) As Task(Of List(Of AccountWithdrawHistory)) Implements IBankAccountService.GetAllWithdrawAsync
        Return Await _db.AccountWithdraw.Where(Function(x) x.AccountId = accountId).ToListAsync()
    End Function

    Public Async Function GetAllDepositsAsync() As Task(Of List(Of BankAccountDepositHistory)) Implements IBankAccountService.GetAllDepositsAsync
        Return Await _db.DepositHistory.ToListAsync()
    End Function

    Public Async Function GetAllWithdrawAsync() As Task(Of List(Of AccountWithdrawHistory)) Implements IBankAccountService.GetAllWithdrawAsync
        Return Await _db.AccountWithdraw.ToListAsync()
    End Function
End Class