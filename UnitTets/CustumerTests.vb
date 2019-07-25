Imports BankingSystem
Imports Xunit

Public Class CustumerTests
    <Fact>
    Async Function Create() As Task
        ' Arrange

        Dim db = TestHelper.CreateDb()
        Dim customer = New Customer() With {
            .FirstName = "dan",
            .LastName = "julia",
            .IDNumber = "PTA140",
            .Email = "dome@gmail.com"
        }
        Dim service = New CustomerService(db)

        ' Act

        Await service.CreateAsync(customer)

        ' Assert

        Dim f = db.Customers.First()
        Assert.Equal(customer, f)
    End Function

    <Fact>
    Async Function CreateWithAnExistingIDnumber() As Task
        ' Arrange

        Dim db = TestHelper.CreateDb()
        Dim oldCustomer = New Customer() With {
            .FirstName = "dan",
            .LastName = "julia",
            .IDNumber = "PTA140",
            .Email = "dome@gmail.com"
        }
        db.Customers.Add(oldCustomer)
        db.SaveChanges()
        Dim customer = New Customer() With {
            .FirstName = "dan",
            .LastName = "julia",
            .IDNumber = "PTA140",
            .Email = "dome@gmail.com"
        }
        Dim service = New CustomerService(db)

        ' Act

        Dim create = Await Assert.ThrowsAsync(Of FriendException)(Async Function()
                                                                      Await service.CreateAsync(customer)
                                                                  End Function)

        ' Assert

        Assert.Equal(CustomerService.IDNumberExistError, create.Message)
    End Function

    <Fact>
    Async Function DeleteAccounts() As Task
        ' Arrange

        Dim db = TestHelper.CreateDb()
        Dim oldCustomer = New Customer() With {
            .FirstName = "dan",
            .LastName = "julia",
            .IDNumber = "PTA140",
            .Email = "dome@gmail.com"
        }
        db.Customers.Add(oldCustomer)
        db.SaveChanges()
        Dim ids = New List(Of Integer) From {
            oldCustomer.Id
        }
        Dim service = New CustomerService(db)

        ' Act

        Await service.DeleteAsync(ids)

        ' Assert

        Dim f = db.Customers.FirstOrDefault()
        Assert.Null(f)
    End Function

    <Fact>
    Async Function Update() As Task
        ' Arrange

        Dim db = TestHelper.CreateDb()
        Dim oldCustomer = New Customer() With {
            .FirstName = "dan",
            .LastName = "julia",
            .IDNumber = "PTA140",
            .Email = "dome@gmail.com"
        }
        db.Customers.Add(oldCustomer)
        db.SaveChanges()
        Dim updated = CType(oldCustomer.Clone(), Customer)
        updated.FirstName = "new first name"
        Dim service = New CustomerService(db)

        ' Act

        Await service.UpdateAsync(updated)

        ' Assert

        Dim f = db.Customers.First()
        Assert.Equal("new first name", f.FirstName)
    End Function

    <Fact>
    Async Function UpdateDuplicateIDnumber() As Task
        ' Arrange

        Dim db = TestHelper.CreateDb()
        Dim oldCustomer = New Customer() With {
            .FirstName = "dan",
            .LastName = "julia",
            .IDNumber = "ROSAM",
            .Email = "PTA100@gmail.com"
        }
        Dim customer2 = New Customer() With {
            .FirstName = "dan",
            .LastName = "julia",
            .IDNumber = "PTA140",
            .Email = "dome@gmail.com"
        }
        db.Customers.AddRange(oldCustomer, customer2)
        db.SaveChanges()

        Dim updated = CType(oldCustomer.Clone(), Customer)
        updated.IDNumber = customer2.IDNumber
        Dim service = New CustomerService(db)

        ' Act

        Dim update = Await Assert.ThrowsAsync(Of FriendException)(Async Function()
                                                                      Await service.UpdateAsync(updated)
                                                                  End Function)

        ' Assert

        Assert.Equal(CustomerService.IDNumberExistError, update.Message)
    End Function
End Class
