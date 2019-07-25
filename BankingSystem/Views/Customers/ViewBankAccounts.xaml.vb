Imports System.Collections.ObjectModel
Imports System.ComponentModel.DataAnnotations
Imports MahApps.Metro.Controls
Imports MahApps.Metro.Controls.Dialogs
Imports MahApps.Metro.SimpleChildWindow

Public Class ViewBankAccounts

    Private ReadOnly _bankAccountService As IBankAccountService
    Private ReadOnly _customerId As Integer
    Private ReadOnly _accounts As ObservableCollection(Of BankAccount)
    Private ReadOnly _deposits As ObservableCollection(Of BankAccountDepositHistory)
    Private ReadOnly _withdraws As ObservableCollection(Of AccountWithdrawHistory)
    Private ReadOnly _win As MetroWindow

    Public Sub New(bankAccountService As IBankAccountService, customerId As Integer)

        ' This call is required by the designer.
        InitializeComponent()
        _bankAccountService = bankAccountService
        _customerId = customerId
        _win = Windows.Application.Current.Windows.OfType(Of MetroWindow).FirstOrDefault(Function(x) x.IsActive)

        ' Add any initialization after the InitializeComponent() call.

        _customerId = customerId
        _deposits = New ObservableCollection(Of BankAccountDepositHistory)
        _withdraws = New ObservableCollection(Of AccountWithdrawHistory)
        _accounts = New ObservableCollection(Of BankAccount)
        dataGrid.ItemsSource = _accounts
        dataGridDeposit.ItemsSource = _deposits
        dataGridWithdraw.ItemsSource = _withdraws
        FetchAccounts()
    End Sub

    ''' <summary>
    ''' Fetch all acounts for the selected customer
    ''' </summary>
    Private Async Sub FetchAccounts()
        Dim all = Await _bankAccountService.GetAllAccountOfUserAsync(_customerId).ConfigureAwait(False)
        _accounts.Clear()
        For Each account In all
            _accounts.Add(account)
        Next
    End Sub

    ''' <summary>
    ''' When show statement button is clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub ShowStatmentBtn_Click(sender As Object, e As RoutedEventArgs)
        RefreshStatement()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub RefreshStatement()
        Dim selectedAccount = CType(dataGrid.SelectedItem, BankAccount)
        If selectedAccount Is Nothing Then
            Return
        End If
        RefreshDepositHistory(selectedAccount.Id)
        RefreshWithdrawHistory(selectedAccount.Id)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Async Sub RefreshDepositHistory(accountId As Integer)
        Dim allDeposits = Await _bankAccountService.GetAllDepositsAsync(accountId).ConfigureAwait(False)
        _deposits.Clear()
        For Each item In allDeposits
            _deposits.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="accountId"></param>
    Private Async Sub RefreshWithdrawHistory(accountId As Integer)
        Dim all = Await _bankAccountService.GetAllWithdrawAsync(accountId).ConfigureAwait(False)
        _withdraws.Clear()
        For Each item In all
            _withdraws.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' When deposit button is clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub DepositBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedAccount = CType(dataGrid.SelectedItem, BankAccount)
        If selectedAccount Is Nothing Then
            Return
        End If
        Dim depositForm = New DepositDialog()
        AddHandler depositForm.cancelBtn.Click, Sub()
                                                    depositForm.Close()
                                                End Sub
        AddHandler depositForm.depositBtn.Click, Async Sub()
                                                     If String.IsNullOrWhiteSpace(depositForm.amount.Text) Then
                                                         Await _win.ShowMessageAsync("Error", "Amount is required")
                                                         Return
                                                     End If
                                                     If String.IsNullOrWhiteSpace(depositForm.reference.Text) Then
                                                         Await _win.ShowMessageAsync("Error", "The reference is required")
                                                         Return
                                                     End If
                                                     Dim err = Nothing
                                                     Try
                                                         Await _bankAccountService.DepositeAsync(selectedAccount.Id, depositForm.amount.Text)
                                                         RefreshDepositHistory(selectedAccount.Id)
                                                         FetchAccounts()
                                                         depositForm.Close()
                                                     Catch ex As FriendException
                                                         err = ex.Message
                                                     Catch ex As ValidationException
                                                         err = ex.Message
                                                     Catch ex As Exception
                                                         err = "Something went wrong"
                                                     End Try
                                                     If err IsNot Nothing Then
                                                         Await _win.ShowMessageAsync("Error", err)
                                                     End If
                                                 End Sub
        Await _win.ShowChildWindowAsync(depositForm)
    End Sub
End Class