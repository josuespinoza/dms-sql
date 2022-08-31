Imports System

<Serializable()> _
Public Class ConfiguracionNivelAprobacion
    Public Property CodigoSucursal() As String
        Get
            Return strCodigoSucursal
        End Get
        Set(ByVal value As String)
            strCodigoSucursal = value
        End Set
    End Property
    Private strCodigoSucursal As String

    Public Property CodigoNivelAprobacion() As String
        Get
            Return strCodigoNivelAprobacion
        End Get
        Set(ByVal value As String)
            strCodigoNivelAprobacion = value
        End Set
    End Property
    Private strCodigoNivelAprobacion As String

    Public Property CodigoUsuario() As String
        Get
            Return strCodigoUsuario
        End Get
        Set(ByVal value As String)
            strCodigoUsuario = value
        End Set
    End Property
    Private strCodigoUsuario As String

    Public Property NombreUsuario() As String
        Get
            Return strNombreUsuario
        End Get
        Set(ByVal value As String)
            strNombreUsuario = value
        End Set
    End Property
    Private strNombreUsuario As String

    Public Property RecibeMensaje() As Boolean
        Get
            Return blnRecibeMensaje
        End Get
        Set(ByVal value As Boolean)
            blnRecibeMensaje = value
        End Set
    End Property
    Private blnRecibeMensaje As Boolean


    Public Property UsaMenuContratoVenta() As Boolean
        Get
            Return blnUsaMenuContratoVenta
        End Get
        Set(ByVal value As Boolean)
            blnUsaMenuContratoVenta = value
        End Set
    End Property
    Private blnUsaMenuContratoVenta As Boolean

    Public Property ManejaAprobacion() As Boolean
        Get
            Return blnManejaAprobacion
        End Get
        Set(ByVal value As Boolean)
            blnManejaAprobacion = value
        End Set
    End Property
    Private blnManejaAprobacion As Boolean
End Class
