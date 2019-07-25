Public Interface IEmployeeService
    Function CreateUserAsync(user As User) As Task
    Function DeleteAccountsAsync(ids As List(Of Integer)) As Task
    Function GetByIdAsync(id As Integer) As Task(Of User)
    Function IsEmailExistAsync(email As String) As Task(Of Boolean)
    Function SetPermissionsAsync(userId As Integer, permissions As List(Of Permission)) As Task
    Function UpdateAsync(updated As User) As Task
    Function LoginAsync(userName As String, password As String) As Task(Of Boolean)
    Function GetAllAsync() As Task(Of List(Of User))
    Function GetAsync(username As String) As Task(Of User)
End Interface
