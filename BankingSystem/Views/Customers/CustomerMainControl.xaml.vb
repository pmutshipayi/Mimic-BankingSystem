Imports System.Collections.ObjectModel
Imports System.ComponentModel.DataAnnotations
Imports MahApps.Metro.Controls
Imports MahApps.Metro.Controls.Dialogs
Imports MahApps.Metro.SimpleChildWindow

Public Class CustomerMainControl
    Private ReadOnly _service As ICustomerService
    Private ReadOnly _bankAccountService As BankAccountService
    Private ReadOnly _data As ObservableCollection(Of Customer)
    Private ReadOnly _win As MetroWindow

    Public Sub New(service As ICustomerService, bankAccountService As BankAccountService)

        ' This call is required by the designer.
        InitializeComponent()
        _service = service
        _bankAccountService = bankAccountService
        _win = Windows.Application.Current.Windows.OfType(Of MetroWindow).SingleOrDefault(Function(x) x.IsActive)

        ' Add any initialization after the InitializeComponent() call.

        _data = New ObservableCollection(Of Customer)
        dataGrid.ItemsSource = _data
        FetchData()
    End Sub

    ''' <summary>
    ''' Fetch customers
    ''' </summary>
    Private Async Sub FetchData()
        Dim all = Await _service.GetAllAsync().ConfigureAwait(False)
        _data.Clear()
        For Each item In all
            _data.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' When search button is clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub SearchBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim customer = Await _service.GetAsync(searchBox.Text).ConfigureAwait(False)
        If customer Is Nothing Then
            Await _win.ShowMessageAsync("No result", "nothing found!")
            Return
        End If
        _data.Clear()
        _data.Add(customer)
    End Sub

    ''' <summary>
    ''' When add button is clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub AddNewBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim customer = New Customer() With {
            .FirstName = form.firstName.Text,
            .LastName = form.lastName.Text,
            .Email = form.email.Text,
            .IDNumber = form.idNumber.Text,
            .Phone = form.phoneNumber.Text,
            .CellPhone = form.cellNumber.Text,
            .Address = form.physicalAddress.Text,
            .DateOfBirth = form.dateOfBirth.SelectedDate.Value
        }
        Dim err = Nothing
        Try
            Await _service.CreateAsync(customer)
            FetchData()
            ClearForm()
        Catch ex As ValidationException
            err = ex.Message
        Catch ex As FriendException
            err = ex.Message
        Catch ex As Exception
            err = "Something went wrong"
        End Try
        If err IsNot Nothing Then
            Await _win.ShowMessageAsync("Error", err)
        End If
    End Sub

    ''' <summary>
    ''' When clear button is clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ClearFormBtn_Click(sender As Object, e As RoutedEventArgs)
        ClearForm()
    End Sub

    ''' <summary>
    ''' Clear the form
    ''' </summary>
    Private Sub ClearForm()
        form.firstName.Text = ""
        form.lastName.Text = ""
        form.email.Text = ""
        form.physicalAddress.Text = ""
        form.idNumber.Text = ""
        form.phoneNumber.Text = ""
        form.cellNumber.Text = ""
        form.dateOfBirth.SelectedDate = Nothing
    End Sub

    ''' <summary>
    ''' When create bank account button is clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddBankAccountBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedCustomer = CType(dataGrid.SelectedItem, Customer)
        If selectedCustomer Is Nothing Then
            Return
        End If
        Dim accountForm = New AddBankAccountForm()
        Dim win = New MetroWindow() With {
            .Content = accountForm,
            .ShowCloseButton = False,
            .Width = 400,
            .Height = 300
        }
        AddHandler accountForm.cancelBtn.Click, Sub()
                                                    win.Close()
                                                End Sub
        AddHandler accountForm.createBtn.Click, Async Sub()
                                                    If String.IsNullOrWhiteSpace(accountForm.accountName.Text) Then
                                                        Await win.ShowMessageAsync("Error", "The account name is required")
                                                        Return
                                                    End If
                                                    Dim account = New BankAccount() With {
                                                        .Balance = accountForm.balance.Text,
                                                        .AccountName = accountForm.accountName.Text,
                                                        .CustomerId = selectedCustomer.Id
                                                    }
                                                    Dim err = Nothing
                                                    Try
                                                        Await _bankAccountService.CreateAsync(account).ConfigureAwait(False)
                                                        win.Close()
                                                    Catch ex As FriendException
                                                        err = ex.Message
                                                    Catch ex As ValidationException
                                                        err = ex.Message
                                                    Catch ex As Exception
                                                        err = ex.ToString()
                                                    End Try
                                                    If err IsNot Nothing Then
                                                        Await win.ShowMessageAsync("Error", err)
                                                    End If
                                                End Sub
        win.ShowDialog()
    End Sub

    ''' <summary>
    ''' When view accounts buttons is clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub ViewAccountBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedCustomer = CType(dataGrid.SelectedItem, Customer)
        If selectedCustomer Is Nothing Then
            Return
        End If
        Dim accountView = New ViewBankAccounts(_bankAccountService, selectedCustomer.Id)
        Await _win.ShowChildWindowAsync(accountView)
    End Sub
End Class