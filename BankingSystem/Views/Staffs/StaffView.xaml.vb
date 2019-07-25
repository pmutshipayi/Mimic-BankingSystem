Imports System.Collections.ObjectModel
Imports System.ComponentModel.DataAnnotations
Imports MahApps.Metro.Controls
Imports MahApps.Metro.Controls.Dialogs

Public Class StaffView

    ''' <summary>
    ''' The employee service class
    ''' </summary>
    Private ReadOnly _userService As IEmployeeService

    ''' <summary>
    ''' Hold all employees and it bindided to the datagrid
    ''' </summary>
    Private ReadOnly _data As ObservableCollection(Of User)

    ''' <summary>
    ''' Represent the current active window
    ''' </summary>
    Private ReadOnly _win As MetroWindow

    Public Sub New(userService As IEmployeeService)

        ' This call is required by the designer.
        InitializeComponent()
        _win = Windows.Application.Current.Windows.OfType(Of MetroWindow).SingleOrDefault(Function(x) x.IsActive)
        _userService = userService

        ' Add any initialization after the InitializeComponent() call.

        _data = New ObservableCollection(Of User)
        dataGrid.ItemsSource = _data
        FetchData()
    End Sub

    ''' <summary>
    ''' fetch all employees
    ''' </summary>
    Private Async Sub FetchData()
        Dim all = Await _userService.GetAllAsync().ConfigureAwait(False)
        _data.Clear()
        For Each item In all
            _data.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' When refresh button is clicked, re fetch the data
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RefreshBtn_Click(sender As Object, e As RoutedEventArgs)
        FetchData()
    End Sub

    ''' <summary>
    ''' When Add new employee button is clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Async Function AddNew_Click_1(sender As Object, e As RoutedEventArgs) As Task
        Dim err = Nothing
        Dim user = New User() With {
            .FirstName = form.firstName.Text,
            .LastName = form.lastName.Text,
            .Email = form.email.Text,
            .IDNumber = form.idNumber.Text,
            .Phone = form.phoneNumber.Text,
            .CellPhone = form.cellNumber.Text,
            .DateOfBirth = form.dateOfBirth.Text,
            .Address = form.physicalAddress.Text,
            .Password = form.password.Password
        }

        Try
            Await _userService.CreateUserAsync(user).ConfigureAwait(False)
            ClearForm()
            FetchData()
        Catch ex As FriendException
            err = ex.Message
        Catch ex As ValidationException
            err = ex.Message
        Catch ex As Exception
            err = "Something went wrong"
        End Try
        If err IsNot Nothing Then
            Dim win = Windows.Application.Current.Windows.OfType(Of MetroWindow).SingleOrDefault(Function(x) x.IsActive)
            Await win.ShowMessageAsync("Operation failed", err)
        End If
    End Function

    ''' <summary>
    ''' When clear form button is clicked
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
        form.physicalAddress.Text = ""
        form.email.Text = ""
        form.phoneNumber.Text = ""
        form.cellNumber.Text = ""
        form.idNumber.Text = ""
        form.dateOfBirth.Text = ""
    End Sub

    ''' <summary>
    ''' When search button is clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub SearchBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim user = Await _userService.GetAsync(searchBox.Text).ConfigureAwait(False)
        If user Is Nothing Then
            Await _win.ShowMessageAsync("No result", "Nothing was found using the search term")
            Return
        End If
        _data.Clear()
        _data.Add(user)
    End Sub

    ''' <summary>
    ''' When delete button is clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub DeleteBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim items = New List(Of User)
        For Each item In dataGrid.SelectedItems
            items.Add(item)
        Next
        If items.Any() = False Then Return
        Dim messageBoxResult As MessageBoxResult = MessageBox.Show("Are you sure you want to delete the selected users?", "Delete Confirmation", MessageBoxButton.YesNo)
        If messageBoxResult = MessageBoxResult.Yes Then
            Dim err = Nothing
            Try
                Dim ids = items.Select(Function(x) x.Id).ToList()
                Await _userService.DeleteAccountsAsync(ids)
                FetchData()
            Catch ex As Exception
                err = "The deletion failed please try again later"
            End Try
            If err IsNot Nothing Then
                Await _win.ShowMessageAsync("Error", err)
            End If
        End If
    End Sub

    ''' <summary>
    ''' When update button is clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub UpdateBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedUser = CType(dataGrid.SelectedItem, User)
        If selectedUser Is Nothing Then Return
        Dim myDockPanel = New DockPanel()

        ' Create a stack panel which will have two button (update and cancel)

        Dim stackPanel = New StackPanel() With {
            .Orientation = Orientation.Horizontal,
            .Margin = New Thickness(10)
        }

        Dim updateBtn = New Button() With {
            .Content = "Update"
        }
        Dim cancelBtn = New Button() With {
            .Content = "Cancel"
        }

        stackPanel.Children.Add(updateBtn)
        stackPanel.Children.Add(cancelBtn)
        DockPanel.SetDock(stackPanel, Dock.Top)
        myDockPanel.Children.Add(stackPanel)

        Dim updateForm = New StaffForm()

        updateForm.firstName.Text = selectedUser.FirstName
        updateForm.idNumber.Text = selectedUser.IDNumber
        updateForm.lastName.Text = selectedUser.LastName
        updateForm.email.Text = selectedUser.Email
        updateForm.phoneNumber.Text = selectedUser.Phone
        updateForm.cellNumber.Text = selectedUser.CellPhone
        updateForm.physicalAddress.Text = selectedUser.Address
        updateForm.dateOfBirth.SelectedDate = selectedUser.DateOfBirth
        updateForm.password.Visibility = Visibility.Collapsed
        updateForm.passwordLabel.Visibility = Visibility.Collapsed

        myDockPanel.Children.Add(updateForm)
        Dim win = New MetroWindow With {
            .Content = myDockPanel,
            .Width = 500,
            .ShowCloseButton = False
        }

        ' Events
        ' When cancel button is click

        AddHandler cancelBtn.Click, Sub()
                                        win.Close()
                                    End Sub

        ' When update button is clicked
        AddHandler updateBtn.Click, Async Sub()
                                        selectedUser.IDNumber = updateForm.idNumber.Text
                                        selectedUser.Email = updateForm.email.Text
                                        selectedUser.FirstName = updateForm.firstName.Text
                                        selectedUser.LastName = updateForm.lastName.Text
                                        selectedUser.Phone = updateForm.phoneNumber.Text
                                        selectedUser.CellPhone = updateForm.cellNumber.Text
                                        selectedUser.Address = updateForm.physicalAddress.Text
                                        selectedUser.DateOfBirth = updateForm.dateOfBirth.SelectedDate.Value.Date

                                        Dim err = Nothing
                                        Try
                                            Await _userService.UpdateAsync(selectedUser)
                                            win.Close()
                                            FetchData()
                                        Catch ex As FriendException
                                            err = ex.Message
                                        Catch ex As ValidationException
                                            err = ex.Message
                                        Catch ex As Exception
                                            err = "Something went wrong0"
                                        End Try
                                        If err IsNot Nothing Then
                                            Await win.ShowMessageAsync("Error", err)
                                        End If
                                    End Sub

        win.ShowDialog()
    End Sub

    ''' <summary>
    ''' When the change password button is clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub UpdatePassword_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedUser = CType(dataGrid.SelectedItem, User)
        If selectedUser Is Nothing Then Return
        Dim passwordForm = New ChangePasswordView()
        Dim win = New MetroWindow() With {
            .Content = passwordForm,
            .Width = 350,
            .Height = 200,
            .ShowCloseButton = False
        }

        ' When cancel button is clicked on the change password form

        AddHandler passwordForm.cancelBtn.Click, Sub()
                                                     win.Close()
                                                 End Sub

        ' When the change button is clicked on the change password form

        AddHandler passwordForm.changeBtn.Click, Async Sub()
                                                     If passwordForm.newPassword.Password <> passwordForm.confirmPassword.Password Then
                                                         Await win.ShowMessageAsync("Error", "The two password doesn't match")
                                                         Return
                                                     End If
                                                     Dim err = Nothing
                                                     Try
                                                         selectedUser.Password = passwordForm.newPassword.Password
                                                         Await _userService.UpdateAsync(selectedUser)
                                                         win.Close()
                                                         Return
                                                     Catch ex As ValidationException
                                                         err = ex.Message
                                                     Catch ex As Exception
                                                         err = "Failed to update the password"
                                                     End Try
                                                     If err IsNot Nothing Then
                                                         Await win.ShowMessageAsync("Error", err)
                                                     End If
                                                 End Sub
        win.ShowDialog()
    End Sub
End Class