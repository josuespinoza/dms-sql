Imports System

<Serializable()> _
Public Class ConfiguracionOrdenTrabajo
    Implements IEquatable(Of ConfiguracionOrdenTrabajo)

    Public Overloads Function Equals(ByVal other As ConfiguracionOrdenTrabajo) _
    As Boolean Implements IEquatable(Of ConfiguracionOrdenTrabajo).Equals
        If Me.TipoOT = other.TipoOT Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Property TipoOT() As String
        Get
            Return strTipoOT
        End Get
        Set(ByVal value As String)
            strTipoOT = value
        End Set
    End Property
    Private strTipoOT As String

    Public Property UsaDimensiones() As Boolean
        Get
            Return blnUsaDimensiones
        End Get
        Set(ByVal value As Boolean)
            blnUsaDimensiones = value
        End Set
    End Property
    Private blnUsaDimensiones As Boolean

    Private _strUsaDimAEM As String

    Public Property UsaDimensionAsientoEntradaMercancia() As Boolean
        Get
            Return blnUsaDimensionAsientoEntradaMercancia
        End Get
        Set(ByVal value As Boolean)
            blnUsaDimensionAsientoEntradaMercancia = value
        End Set
    End Property
    Private blnUsaDimensionAsientoEntradaMercancia As Boolean

    Public Property UsaDimensionAsientoFacturaProveedor() As Boolean
        Get
            Return blnUsaDimensionAsientoFacturaProveedor
        End Get
        Set(ByVal value As Boolean)
            blnUsaDimensionAsientoFacturaProveedor = value
        End Set
    End Property
    Private blnUsaDimensionAsientoFacturaProveedor As Boolean
End Class
