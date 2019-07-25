Imports MahApps.Metro.Controls

Class MainWindow
    Private ReadOnly _userService As IEmployeeService
    Private ReadOnly _customerService As ICustomerService
    Private ReadOnly bankAccountService As BankAccountService

    Public Sub New(userService As IEmployeeService, customerService As ICustomerService, bankAccountService As BankAccountService)

        ' This call is required by the designer.
        InitializeComponent()
        _userService = userService
        _customerService = customerService
        Me.bankAccountService = bankAccountService

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    ''' <summary>
    ''' Onclick left menu
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub HamburgerMenuControl_ItemClick(sender As Object, e As ItemClickEventArgs)
        Dim item = CType(e.ClickedItem, HamburgerMenuIconItem)
        Select Case item.Label
            Case "Customers"
                contentArea.Content = New CustomerMainControl(_customerService, bankAccountService)
            Case "Staff"
                contentArea.Content = New StaffView(_userService)
            Case "Home"
                contentArea.Content = New HomeView(bankAccountService)
        End Select
    End Sub
End Class