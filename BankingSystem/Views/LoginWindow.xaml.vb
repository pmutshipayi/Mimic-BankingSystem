Imports MahApps.Metro.Controls.Dialogs

Public Class LoginWindow
    Private ReadOnly _userService As IEmployeeService
    Private ReadOnly customerService As ICustomerService
    Private ReadOnly bankAccountService As BankAccountService

    Public Sub New(userService As IEmployeeService, customerService As ICustomerService, bankAccountService As BankAccountService)

        ' This call is required by the designer.

        InitializeComponent()
        _userService = userService
        Me.customerService = customerService
        Me.bankAccountService = bankAccountService

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    ''' <summary>
    ''' When login button is clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub LoginBtn_Click(sender As Object, e As RoutedEventArgs)
        ring.Visibility = Visibility.Visible
        Dim err = ""
        Try
            Dim login = Await _userService.LoginAsync(userName.Text, password.Password).ConfigureAwait(False)
            If login Then
                Dim main = New MainWindow(_userService, customerService, bankAccountService)
                main.Show()
                Close()
            Else
                err = "Invalid username or password"
            End If
        Catch ex As FriendException
            err = ex.Message
        Catch ex As Exception
            err = "Something went wrong."
        End Try
        Await ShowMessageAsync("Error", err)
        ring.Visibility = Visibility.Collapsed
    End Sub
End Class