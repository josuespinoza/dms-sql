Imports System.Collections.Generic

Public Class RestablecerCantidades
    Public LineasDocumento As List(Of LineaRestablecerCantidades)
    Public Resultado As String

    Sub New()
        LineasDocumento = New List(Of LineaRestablecerCantidades)
        Resultado = String.Empty
    End Sub
End Class
