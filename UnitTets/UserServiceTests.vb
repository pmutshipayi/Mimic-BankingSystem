Imports System
Imports BankingSystem
Imports Microsoft.EntityFrameworkCore
Imports Xunit

Namespace UnitTets
    Public Class UserServiceTests
        <Fact>
        Async Function CreateUserAsync() As Task
            ' Arrage 
            Dim db = TestHelper.CreateDb()
            Dim service = New EmployeeService(db)
            Dim user = New User() With {
               .FirstName = "Alex",
               .LastName = "Paul",
               .Password = "New password",
               .Email = "alex@gmaicom"
            }

            ' Act

            Await service.CreateUserAsync(user)

            ' Assert

            Dim f = db.Users.First()
            Assert.Equal(f, user)
        End Function


        <Fact>
        Async Function CreateAccount_WithExistingEmail() As Task
            ' Arrage 
            Dim db = TestHelper.CreateDb()
            Dim service = New EmployeeService(db)
            db.Add(New User() With {
                .Email = "ali@yahho.fr",
                .FirstName = "Pom",
                .LastName = "Some",
                .Password = "Password"
            })
            db.SaveChanges()
            Dim user = New User() With {
               .FirstName = "Alex",
               .LastName = "Paul",
               .Password = "ali@yahho.fr",
               .Email = "ali@yahho.fr"
            }

            ' Act

            Dim err = Await Assert.ThrowsAnyAsync(Of FriendException)(Async Function()
                                                                          Await service.CreateUserAsync(user)
                                                                      End Function)

            ' Assert

            Assert.Equal(EmployeeService.EmailUsed, err.Message)
        End Function

        <Fact>
        Async Function DeleteAccounts() As Task
            ' Arrage 
            Dim db = TestHelper.CreateDb()
            Dim service = New EmployeeService(db)
            Dim user = New User() With {
                .Email = "ali@yahho.fr",
                .FirstName = "Pom",
                .LastName = "Some",
                .Password = "Password"
            }
            db.Add(user)
            db.SaveChanges()
            Dim ids = New List(Of Integer) From {
                user.Id
            }

            ' Act

            Await service.DeleteAccountsAsync(ids)

            ' Assert

            Dim f = db.Users.FirstOrDefault()
            Assert.Null(f)
        End Function

        <Fact>
        Async Function Update() As Task
            ' Arrage 
            Dim db = TestHelper.CreateDb()
            Dim service = New EmployeeService(db)
            Dim user = New User() With {
                .Email = "ali@yahho.fr",
                .FirstName = "Pom",
                .LastName = "Some",
                .Password = "Password"
            }
            db.Add(user)
            db.SaveChanges()

            Dim updated = CType(user.Clone(), User)
            updated.FirstName = "john"

            ' Act

            Await service.UpdateAsync(updated)

            ' Assert

            Dim f = db.Users.First()
            Assert.Equal("john", f.FirstName)
        End Function

        <Fact>
        Async Function UpdateDuplicateEmail() As Task
            ' Arrage 
            Dim db = TestHelper.CreateDb()
            Dim service = New EmployeeService(db)
            Dim user = New User() With {
                .Email = "ali@yahho.fr",
                .FirstName = "Pom",
                .LastName = "Some",
                .Password = "Password"
            }
            Dim user2 = New User() With {
                .Email = "email@hello.com",
                .FirstName = "Pom",
                .LastName = "Some",
                .Password = "Password"
            }
            db.AddRange(user, user2)
            db.SaveChanges()

            Dim updated = CType(user.Clone(), User)
            updated.Email = "email@hello.com"

            ' Act

            Dim update = Await Assert.ThrowsAsync(Of FriendException)(Async Function()
                                                                          Await service.UpdateAsync(updated)
                                                                      End Function)

            ' Assert

            Assert.Equal(EmployeeService.EmailUsed, update.Message)
        End Function

        <Fact>
        Async Function UpdatePermissions() As Task
            ' Arrage 
            Dim db = TestHelper.CreateDb()
            Dim service = New EmployeeService(db)
            Dim user = New User() With {
                .Email = "ali@yahho.fr",
                .FirstName = "Pom",
                .LastName = "Some",
                .Password = "Password",
                .Permissions = New List(Of UserPermission) From {
                   New UserPermission() With {
                       .Permission = Permission.ManageEmployees
                   }
                }
            }
            db.Add(user)
            db.SaveChanges()

            Dim permissions = New List(Of Permission) From {
                Permission.ManageCustomers
            }

            ' Act

            Await service.SetPermissionsAsync(user.Id, permissions)

            ' Assert

            Dim f = db.Users.Include(Function(x) x.Permissions).First().Permissions
            Assert.Single(f)
            Assert.Equal(Permission.ManageCustomers, f.First().Permission)
        End Function
    End Class
End Namespace