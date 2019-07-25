Imports System.Collections.ObjectModel

Public Class HomeView

    Private ReadOnly _accounts As ObservableCollection(Of BankAccount)
    Private ReadOnly _deposits As ObservableCollection(Of BankAccountDepositHistory)
    Private ReadOnly _withdraws As ObservableCollection(Of AccountWithdrawHistory)
    Private ReadOnly bankAccountService As IBankAccountService
    Private ReadOnly customerId As Integer

    Public Sub New(bankAccountService As IBankAccountService)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _deposits = New ObservableCollection(Of BankAccountDepositHistory)
        _withdraws = New ObservableCollection(Of AccountWithdrawHistory)
        _accounts = New ObservableCollection(Of BankAccount)

        dataGridWithdraw.ItemsSource = _withdraws
        dataGridDeposit.ItemsSource = _deposits
        Me.bankAccountService = bankAccountService
        FetchAccounts()
    End Sub

    ''' <summary>
    ''' Fetch all acounts for the selected customer
    ''' </summary>
    Private Async Sub FetchAccounts()
        Dim allDeposits = Await bankAccountService.GetAllDepositsAsync().ConfigureAwait(False)
        _deposits.Clear()
        For Each item In allDeposits
            _deposits.Add(item)
        Next
        Dim all = Await bankAccountService.GetAllWithdrawAsync().ConfigureAwait(False)
        _withdraws.Clear()
        For Each item In all
            _withdraws.Add(item)
        Next
    End Sub
End Class