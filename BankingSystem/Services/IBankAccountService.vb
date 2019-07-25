Imports BankingSystem

Public Interface IBankAccountService
    Function AccountNumberExistAsync(accountNumber As String) As Task(Of Boolean)
    Function ActivateAsync(ids As List(Of Integer)) As Task
    Function CreateAsync(bankAccount As BankAccount) As Task
    Function DeleteAsync(ids As List(Of Integer)) As Task
    Function DepositeAsync(accountId As Integer, amount As Double) As Task
    Function GetAsync(ids As IEnumerable(Of Integer)) As Task(Of IList(Of BankAccount))
    Function GetAsync(id As Integer) As Task(Of BankAccount)
    Function SuspendAsync(ids As List(Of Integer)) As Task
    Function WithdrawAsync(accountId As Integer, amount As Integer) As Task
    Function GetAllAccountOfUserAsync(userId As Integer) As Task(Of List(Of BankAccount))
    Function GetAllDepositsAsync(accountId As Integer) As Task(Of List(Of BankAccountDepositHistory))
    Function GetAllWithdrawAsync(accountId As Integer) As Task(Of List(Of AccountWithdrawHistory))
    Function GetAllDepositsAsync() As Task(Of List(Of BankAccountDepositHistory))
    Function GetAllWithdrawAsync() As Task(Of List(Of AccountWithdrawHistory))
End Interface
