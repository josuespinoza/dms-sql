Imports System

<Serializable()> _
Public Class ContratoVentaEncabezado
    Public Property DocEntry() As Integer
        Get
            Return intDocEntry
        End Get
        Set(ByVal value As Integer)
            intDocEntry = value
        End Set
    End Property
    Private intDocEntry As Integer

    Public Property DocNum() As Integer
        Get
            Return intDocNum
        End Get
        Set(ByVal value As Integer)
            intDocNum = value
        End Set
    End Property
    Private intDocNum As Integer

    Public Property CodigoSucursal() As String
        Get
            Return strCodigoSucursal
        End Get
        Set(ByVal value As String)
            strCodigoSucursal = value
        End Set
    End Property
    Private strCodigoSucursal As String

    Public Property Vendedor() As String
        Get
            Return strVendedor
        End Get
        Set(ByVal value As String)
            strVendedor = value
        End Set
    End Property
    Private strVendedor As String

    Public Property Estado() As Integer
        Get
            Return intEstado
        End Get
        Set(ByVal value As Integer)
            intEstado = value
        End Set
    End Property
    Private intEstado As Integer

    Public Property UsaFinPropio() As Boolean
        Get
            Return blnUsaFinPropio
        End Get
        Set(ByVal value As Boolean)
            blnUsaFinPropio = value
        End Set
    End Property
    Private blnUsaFinPropio As Boolean

    Public Property UsaFinExterno() As Boolean
        Get
            Return blnUsaFinExterno
        End Get
        Set(ByVal value As Boolean)
            blnUsaFinExterno = value
        End Set
    End Property
    Private blnUsaFinExterno As Boolean

    Public Property ComentarioRechazo() As String
        Get
            Return strComentarioRechazo
        End Get
        Set(ByVal value As String)
            strComentarioRechazo = value
        End Set
    End Property
    Private strComentarioRechazo As String

    Public Property CodigoClienteVehiculo() As String
        Get
            Return strCodigoClienteVehiculo
        End Get
        Set(ByVal value As String)
            strCodigoClienteVehiculo = value
        End Set
    End Property
    Private strCodigoClienteVehiculo As String
End Class
