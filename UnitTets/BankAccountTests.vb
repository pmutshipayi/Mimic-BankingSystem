Imports BankingSystem
Imports Moq
Imports Xunit

Public Class BankAccountTests
    <Fact>
    Async Function Create() As Task
        ' Arrange

        Dim db = CreateDb()
        Dim bankAccount = New BankAccount() With {
            .Balance = 100,
            .CustomerId = 100
        }
        Dim mockCustomerService = New Mock(Of ICustomerService)
        mockCustomerService.Setup(Function(x) x.GetAsync(It.Is(Of Integer)(Function(e) e = 100))).ReturnsAsync(New Customer())
        Dim service = New BankAccountService(db, mockCustomerService.Object)

        ' Act

        Await service.CreateAsync(bankAccount)

        ' Assert

        Dim f = db.BankAccounts.First()
        Assert.Equal(bankAccount, f)
    End Function

    <Fact>
    Async Function CreateButCustomerDoesNotExist() As Task
        ' Arrange

        Dim db = CreateDb()
        Dim bankAccount = New BankAccount() With {
            .Balance = 100,
            .CustomerId = 100
        }
        Dim mockCustomerService = New Mock(Of ICustomerService)
        mockCustomerService.Setup(Function(x) x.GetAsync(It.Is(Of Integer)(Function(e) e = 100)))
        Dim service = New BankAccountService(db, mockCustomerService.Object)

        ' Act

        Dim create = Await Assert.ThrowsAsync(Of FriendException)(Async Function()
                                                                      Await service.CreateAsync(bankAccount)
                                                                  End Function)

        ' Assert

        Assert.Equal(BankAccountService.AccountHolderNotFound, create.Message)
    End Function

    <Fact>
    Async Function CreateCustomerSuspended() As Task
        ' Arrange

        Dim db = CreateDb()
        Dim bankAccount = New BankAccount() With {
            .Balance = 100,
            .CustomerId = 100
        }
        Dim mockCustomerService = New Mock(Of ICustomerService)
        mockCustomerService.Setup(Function(x) x.GetAsync(It.IsAny(Of Integer))).ReturnsAsync(New Customer() With {.IsActive = False})
        Dim service = New BankAccountService(db, mockCustomerService.Object)

        ' Act

        Dim create = Await Assert.ThrowsAsync(Of FriendException)(Async Function()
                                                                      Await service.CreateAsync(bankAccount)
                                                                  End Function)

        ' Assert

        Assert.Equal(BankAccountService.AccountHolderSuspended, create.Message)
    End Function

    <Fact>
    Async Function Delete() As Task
        ' Arrange

        Dim db = TestHelper.CreateDb()
        Dim bankAccount = New BankAccount() With {
            .Balance = 100,
            .CustomerId = 100,
            .AccountNumber = "1204455224714"
        }
        db.BankAccounts.Add(bankAccount)
        db.SaveChanges()

        Dim ids = New List(Of Integer) From {
            bankAccount.Id
        }
        Dim service = New BankAccountService(db, Nothing)

        ' Act

        Await service.DeleteAsync(ids)

        ' Assert

        Dim f = db.BankAccounts.FirstOrDefault()
        Assert.Null(f)
    End Function

    <Fact>
    Async Function Suspend() As Task
        ' Arrange

        Dim db = CreateDb()
        Dim bankAccount = New BankAccount() With {
            .Balance = 100,
            .CustomerId = 100,
            .AccountNumber = "1204455224714"
        }
        db.BankAccounts.Add(bankAccount)
        db.SaveChanges()

        Dim ids = New List(Of Integer) From {
            bankAccount.Id
        }
        Dim service = New BankAccountService(db, Nothing)

        ' Act

        Await service.SuspendAsync(ids)

        ' Assert

        Dim f = db.BankAccounts.First()
        Assert.False(f.IsActive)
    End Function

    <Fact>
    Async Function ActivateAccounts() As Task
        ' Arrange

        Dim db = CreateDb()
        Dim bankAccount = New BankAccount() With {
            .Balance = 100,
            .CustomerId = 100,
            .AccountNumber = "1204455224714",
            .IsActive = False
        }
        db.BankAccounts.Add(bankAccount)
        db.SaveChanges()

        Dim ids = New List(Of Integer) From {
            bankAccount.Id
        }
        Dim service = New BankAccountService(db, Nothing)

        ' Act 

        Await service.ActivateAsync(ids)

        ' Assert

        Dim f = db.BankAccounts.First()
        Assert.True(f.IsActive)
    End Function

    <Fact>
    Public Async Function Deposit() As Task
        ' Arrange

        Dim db = CreateDb()
        Dim bankAccount = New BankAccount() With {
            .Balance = 100,
            .Customer = New Customer() With {
                .Email = "frdc@gmail.com",
                .FirstName = "Aler",
                .LastName = "Jon",
                .IDNumber = "567wef6e"
            },
            .AccountNumber = "1204455224714"
        }
        db.BankAccounts.Add(bankAccount)
        db.SaveChanges()

        Dim mockCustomerService = New Mock(Of ICustomerService)
        mockCustomerService.Setup(Function(x) x.GetAsync(It.Is(Of Integer)(Function(e) e = 100))).ReturnsAsync(New Customer())
        Dim service = New BankAccountService(db, mockCustomerService.Object)
        ' Act

        Await service.DepositeAsync(bankAccount.Id, 50)

        ' Assert

        Dim f = db.BankAccounts.First()
        Assert.Equal(150, f.Balance)
    End Function

    <Fact>
    Async Function DepositeInvalidAmount() As Task
        ' Arrange

        Dim db = CreateDb()
        Dim bankAccount = New BankAccount() With {
            .Balance = 100,
            .Customer = New Customer() With {
                .Email = "frdc@gmail.com",
                .FirstName = "Aler",
                .LastName = "Jon",
                .IDNumber = "567wef6e"
            },
            .AccountNumber = "1204455224714"
        }
        db.BankAccounts.Add(bankAccount)
        db.SaveChanges()
        Dim mockCustomerService = New Mock(Of ICustomerService)
        mockCustomerService.Setup(Function(x) x.GetAsync(It.Is(Of Integer)(Function(e) e = 100))).ReturnsAsync(New Customer())
        Dim service = New BankAccountService(db, mockCustomerService.Object)

        ' Act

        Dim deposit = Await Assert.ThrowsAsync(Of FriendException)(Async Function()
                                                                       Await service.DepositeAsync(bankAccount.Id, 0)
                                                                   End Function)

        ' Assert

        Assert.Equal(BankAccountService.InvalidDepositAmount, deposit.Message)
    End Function

    <Fact>
    Public Async Function DepositeSuspendedAccount() As Task
        ' Arrange

        Dim db = TestHelper.CreateDb()
        Dim bankAccount = New BankAccount() With {
            .Balance = 100,
            .Customer = New Customer() With {
                .Email = "frdc@gmail.com",
                .FirstName = "Aler",
                .LastName = "Jon",
                .IDNumber = "567wef6e"
            },
            .AccountNumber = "1204455224714",
            .IsActive = False
        }
        db.BankAccounts.Add(bankAccount)
        db.SaveChanges()
        Dim mockCustomerService = New Mock(Of ICustomerService)
        mockCustomerService.Setup(Function(x) x.GetAsync(It.Is(Of Integer)(Function(e) e = 100))).ReturnsAsync(New Customer())
        Dim service = New BankAccountService(db, mockCustomerService.Object)

        ' Act

        Dim deposit = Await Assert.ThrowsAsync(Of FriendException)(Async Function()
                                                                       Await service.DepositeAsync(bankAccount.Id, 80)
                                                                   End Function)

        ' Assert

        Assert.Equal(BankAccountService.AccountHolderSuspended, deposit.Message)
    End Function

    <Fact>
    Public Async Function DepositeCustomerSuspended() As Task
        ' Arrange

        Dim db = TestHelper.CreateDb()
        Dim bankAccount = New BankAccount() With {
            .Balance = 100,
            .Customer = New Customer() With {
                .Email = "frdc@gmail.com",
                .FirstName = "Aler",
                .LastName = "Jon",
                .IDNumber = "567wef6e",
                .IsActive = False
            },
            .AccountNumber = "1204455224714"
        }
        db.BankAccounts.Add(bankAccount)
        db.SaveChanges()
        Dim mockCustomerService = New Mock(Of ICustomerService)
        mockCustomerService.Setup(Function(x) x.GetAsync(It.Is(Of Integer)(Function(e) e = 100)))
        Dim service = New BankAccountService(db, mockCustomerService.Object)

        ' Act

        Dim deposit = Await Assert.ThrowsAsync(Of FriendException)(Async Function()
                                                                       Await service.DepositeAsync(bankAccount.Id, 450)
                                                                   End Function)

        ' Assert

        Assert.Equal(BankAccountService.AccountHolderSuspended, deposit.Message)
    End Function

    <Fact>
    Public Async Function Withdraw() As Task
        ' Arrange

        Dim db = TestHelper.CreateDb()
        Dim bankAccount = New BankAccount() With {
            .Balance = 100,
            .Customer = New Customer() With {
                .Email = "frdc@gmail.com",
                .FirstName = "Aler",
                .LastName = "Jon",
                .IDNumber = "567wef6e"
            },
            .AccountNumber = "1204455224714"
        }
        db.BankAccounts.Add(bankAccount)
        db.SaveChanges()
        Dim service = New BankAccountService(db, Nothing)

        ' Act

        Await service.WithdrawAsync(bankAccount.Id, 70)

        ' Assert

        Dim f = db.BankAccounts.First()
        Assert.Equal(30, f.Balance)
    End Function

    <Fact>
    Public Async Function WithdrawAccountSuspended() As Task
        ' Arrange

        Dim db = TestHelper.CreateDb()
        Dim bankAccount = New BankAccount() With {
            .Balance = 100,
            .Customer = New Customer() With {
                .Email = "frdc@gmail.com",
                .FirstName = "Aler",
                .LastName = "Jon",
                .IDNumber = "567wef6e"
            },
            .IsActive = False,
            .AccountNumber = "1204455224714"
        }
        db.BankAccounts.Add(bankAccount)
        db.SaveChanges()
        Dim service = New BankAccountService(db, Nothing)

        ' Act

        Dim deposit = Await Assert.ThrowsAsync(Of FriendException)(Async Function()
                                                                       Await service.WithdrawAsync(bankAccount.Id, 450)
                                                                   End Function)

        ' Assert

        Assert.Equal(BankAccountService.AccountSuspended, deposit.Message)
    End Function

    <Fact>
    Public Async Function WithdrawAmountMoreThanBalance() As Task
        ' Arrange

        Dim db = TestHelper.CreateDb()
        Dim bankAccount = New BankAccount() With {
            .Balance = 100,
            .Customer = New Customer() With {
                .Email = "frdc@gmail.com",
                .FirstName = "Aler",
                .LastName = "Jon",
                .IDNumber = "567wef6e"
            },
            .AccountNumber = "1204455224714"
        }
        db.BankAccounts.Add(bankAccount)
        db.SaveChanges()
        Dim service = New BankAccountService(db, Nothing)

        ' Act

        Dim deposit = Await Assert.ThrowsAsync(Of FriendException)(Async Function()
                                                                       Await service.WithdrawAsync(bankAccount.Id, 550)
                                                                   End Function)

        ' Assert

        Assert.Equal(BankAccountService.AmountToWithdrawMoreThanBalance, deposit.Message)
    End Function

    <Fact>
    Public Async Function WithrawCustomerSuspended() As Task
        ' Arrange

        Dim db = TestHelper.CreateDb()
        Dim bankAccount = New BankAccount() With {
            .Balance = 100,
            .Customer = New Customer() With {
                .Email = "frdc@gmail.com",
                .FirstName = "Aler",
                .LastName = "Jon",
                .IDNumber = "567wef6e",
                .IsActive = False
            },
            .AccountNumber = "1204455224714"
        }
        db.BankAccounts.Add(bankAccount)
        db.SaveChanges()
        Dim service = New BankAccountService(db, Nothing)

        ' Act

        Dim deposit = Await Assert.ThrowsAsync(Of FriendException)(Async Function()
                                                                       Await service.WithdrawAsync(bankAccount.Id, 450)
                                                                   End Function)

        ' Assert

        Assert.Equal(BankAccountService.AccountHolderSuspended, deposit.Message)
    End Function
End Class