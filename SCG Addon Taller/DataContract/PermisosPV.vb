Imports System

<Serializable()> _
Public Class PermisosPV

    Private _Code As String
    Public Property Code() As String
        Get
            Return _Code
        End Get
        Set(ByVal value As String)
            _Code = value
        End Set
    End Property

    Private _U_Usuario As String
    Public Property U_Usuario() As String
        Get
            Return _U_Usuario
        End Get
        Set(ByVal value As String)
            _U_Usuario = value
        End Set
    End Property

End Class
