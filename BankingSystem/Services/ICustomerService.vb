Public Interface ICustomerService
    Function CreateAsync(customer As Customer) As Task
    Function DeleteAsync(ids As List(Of Integer)) As Task
    Function GetAccounts(customerId As Integer) As Task(Of IList(Of BankAccount))
    Function GetAsync(id As Integer) As Task(Of Customer)
    Function IDNumberExistAsnyc(idNumber As String) As Task(Of Boolean)
    Function UpdateAsync(updated As Customer) As Task
    Function GetAllAsync() As Task(Of List(Of Customer))
    Function GetAsync(idNumber As String) As Task(Of Customer)
End Interface
