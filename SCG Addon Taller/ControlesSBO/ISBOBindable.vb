Option Strict On
Option Explicit On

Namespace ControlesSBO


    Public Interface ISBOBindable
        Sub AsignarBinding(ByVal tabla As String, ByVal columna As String)
    End Interface
End Namespace