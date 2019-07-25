Imports System.Data.SQLite
Imports BankingSystem
Imports Microsoft.EntityFrameworkCore

Public Module TestHelper
    Public Function CreateDb() As AppDbCtx
        Dim connection = New SQLiteConnection("DataSource=:memory:")
        connection.Open()
        Dim dbContextOptions = New DbContextOptionsBuilder(Of AppDbCtx)().EnableSensitiveDataLogging(True).UseSqlite(connection).Options
        Dim db = New AppDbCtx(dbContextOptions)
        db.Database.EnsureCreated()
        Return db
    End Function
End Module
