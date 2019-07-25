Imports System.Runtime.CompilerServices
Imports Microsoft.EntityFrameworkCore

Public Module DbExtensions
    ''' <summary>
    ''' 
    ''' </summary>
    <Extension()>
    Sub DetachEntity(Of TEntity As Entity)(context As AppDbCtx, entity As TEntity, id As Integer)
        Dim local = context.[Set](Of TEntity)().Local.FirstOrDefault(Function(x) x.Id = id)
        If local IsNot Nothing Then
            context.Entry(local).State = EntityState.Detached
        End If
        context.Entry(entity).State = EntityState.Modified
    End Sub
End Module