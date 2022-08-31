Imports System

<Serializable()> _
Public Class ConfiguracionGeneral
    Public Property UsaBackOrder() As Boolean
        Get
            Return blnUsaBackOrder
        End Get
        Set(ByVal value As Boolean)
            blnUsaBackOrder = value
        End Set
    End Property
    Private blnUsaBackOrder As Boolean

    Public Property UsaAsientoServicioExterno() As Boolean
        Get
            Return blnUsaAsientoServicioExterno
        End Get
        Set(ByVal value As Boolean)
            blnUsaAsientoServicioExterno = value
        End Set
    End Property
    Private blnUsaAsientoServicioExterno As Boolean

    Public Property UsaCostosSEPorFacturaProveedor() As Boolean
        Get
            Return blnUsaCostosSEPorFacturaProveedor
        End Get
        Set(ByVal value As Boolean)
            blnUsaCostosSEPorFacturaProveedor = value
        End Set
    End Property
    Private blnUsaCostosSEPorFacturaProveedor As Boolean

    Public Property UsaOTInterna() As Boolean
        Get
            Return blnUsaOTInterna
        End Get
        Set(ByVal value As Boolean)
            blnUsaOTInterna = value
        End Set
    End Property
    Private blnUsaOTInterna As Boolean
End Class
