Imports System.Data.SQLite
Imports System.IO
Imports Microsoft.EntityFrameworkCore
Imports Ninject

Class Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

    Private _container As IKernel
    Private _db As AppDbCtx

    Protected Overrides Sub OnStartup(e As StartupEventArgs)
        MyBase.OnStartup(e)
        CreateDb()
        ConfigureContainer()
        ComposeObjects()
        Current.MainWindow.Show()
    End Sub

    ''' <summary>
    ''' Entablish connection to the database or create the database 
    ''' </summary>
    Private Sub CreateDb()
        Dim dbExist = True
        If File.Exists("db.sqlite") = False Then
            SQLiteConnection.CreateFile("db.sqlite")
            dbExist = False
        End If
        Dim connection = New SQLiteConnection("DataSource=db.sqlite")
        Dim dbContextOptions = New DbContextOptionsBuilder(Of AppDbCtx)().EnableSensitiveDataLogging(True).UseSqlite(connection).Options
        _db = New AppDbCtx(dbContextOptions)
        _db.Database.EnsureCreated()
        If dbExist = False Then

            ' Create the default user

            _db.Users.Add(New User() With {
                .FirstName = "admin",
                .LastName = "admin",
                .IDNumber = "123456789",
                .Email = "admin@gmail.com",
                .Password = "123456789"
            })
            _db.SaveChanges()
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub ConfigureContainer()
        _container = New StandardKernel()
        _container.Bind(Of AppDbCtx)().ToConstant(_db).InSingletonScope()
        _container.Bind(Of ICustomerService)().To(Of CustomerService).InTransientScope()
        _container.Bind(Of IEmployeeService)().To(Of EmployeeService).InTransientScope()
        _container.Bind(Of IBankAccountService)().To(Of BankAccountService).InTransientScope()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub ComposeObjects()
        Current.MainWindow = _container.Get(Of LoginWindow)
        'Current.MainWindow = _container.Get(Of MainWindow)
        Current.MainWindow.Title = "Login"
    End Sub
End Class
