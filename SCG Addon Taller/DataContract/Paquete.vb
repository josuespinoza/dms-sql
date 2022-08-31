Imports System

<Serializable()> _
Public Class Paquete
    Public Property ItemCode() As String
        Get
            Return strItemCode
        End Get
        Set(ByVal value As String)
            strItemCode = value
        End Set
    End Property
    Private strItemCode As String

    Public Property ItemCodePadre() As String
        Get
            Return strItemCodePadre
        End Get
        Set(ByVal value As String)
            strItemCodePadre = value
        End Set
    End Property
    Private strItemCodePadre As String

    Public Property TreeType() As SAPbobsCOM.BoItemTreeTypes
        Get
            Return strTreeType
        End Get
        Set(ByVal value As SAPbobsCOM.BoItemTreeTypes)
            strTreeType = value
        End Set
    End Property
    Private strTreeType As SAPbobsCOM.BoItemTreeTypes

    Public Property IDPaquetePadre() As String
        Get
            Return strIDPaquetePadre
        End Get
        Set(ByVal value As String)
            strIDPaquetePadre = value
        End Set
    End Property
    Private strIDPaquetePadre As String

    Public Property TreeTypePadre() As SAPbobsCOM.BoItemTreeTypes
        Get
            Return strTreeTypePadre
        End Get
        Set(ByVal value As SAPbobsCOM.BoItemTreeTypes)
            strTreeTypePadre = value
        End Set
    End Property
    Private strTreeTypePadre As SAPbobsCOM.BoItemTreeTypes

    Public Property Aprobado() As Integer
        Get
            Return intAprobado
        End Get
        Set(ByVal value As Integer)
            intAprobado = value
        End Set
    End Property
    Private intAprobado As Integer

    Public Property AprobadoPadre() As Integer
        Get
            Return intAprobadoPadre
        End Get
        Set(ByVal value As Integer)
            intAprobadoPadre = value
        End Set
    End Property
    Private intAprobadoPadre As Integer

    Public Property LineNumCotizacion() As Integer
        Get
            Return intLineNumCotizacion
        End Get
        Set(ByVal value As Integer)
            intLineNumCotizacion = value
        End Set
    End Property
    Private intLineNumCotizacion As Integer

    Public Property LineNumCotizacionPadre() As Integer
        Get
            Return intLineNumCotizacionPadre
        End Get
        Set(ByVal value As Integer)
            intLineNumCotizacionPadre = value
        End Set
    End Property
    Private intLineNumCotizacionPadre As Integer

    Public Property IDItem() As String
        Get
            Return strIDItem
        End Get
        Set(ByVal value As String)
            strIDItem = value
        End Set
    End Property
    Private strIDItem As String

End Class


