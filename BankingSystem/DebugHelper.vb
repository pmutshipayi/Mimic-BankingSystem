Imports System.IO

Public Module DebugHelper
    Const FilePath = "C:\Users\user\Documents\new 3.html"
    Public Sub Append(txt As String)
        File.AppendAllText(FilePath, vbNewLine & txt)
    End Sub
    Public Sub Write(txt As String)
        File.WriteAllText(FilePath, txt)
    End Sub
End Module
