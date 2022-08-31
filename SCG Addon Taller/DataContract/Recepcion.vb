Imports System

<Serializable()> _
Public Class Recepcion
    Public Property DocEntryCotizacion() As Integer
        Get
            Return intDocEntryCotizacion
        End Get
        Set(ByVal value As Integer)
            intDocEntryCotizacion = value
        End Set
    End Property
    Private intDocEntryCotizacion As Integer

    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property GeneraOT() As Integer
        Get
            Return intGeneraOT
        End Get
        Set(ByVal value As Integer)
            intGeneraOT = value
        End Set
    End Property
    Private intGeneraOT As Integer

    Public Property IDSucursal() As String
        Get
            Return strIDSucursal
        End Get
        Set(ByVal value As String)
            strIDSucursal = value
        End Set
    End Property
    Private strIDSucursal As String

    Public Property ImprimeOR() As Integer
        Get
            Return strImprimeOR
        End Get
        Set(ByVal value As Integer)
            strImprimeOR = value
        End Set
    End Property
    Private strImprimeOR As Integer

    Public Property ValidaKM() As Boolean
        Get
            Return blnValidaKM
        End Get
        Set(ByVal value As Boolean)
            blnValidaKM = value
        End Set
    End Property
    Private blnValidaKM As Boolean

    Public Property ValidaHorasServicio() As Boolean
        Get
            Return blnValidaHorasServicio
        End Get
        Set(ByVal value As Boolean)
            blnValidaHorasServicio = value
        End Set
    End Property
    Private blnValidaHorasServicio As Boolean

    Public Property CodigoUnidad() As String
        Get
            Return strCodigoUnidad
        End Get
        Set(ByVal value As String)
            strCodigoUnidad = value
        End Set
    End Property
    Private strCodigoUnidad As String

    Public Property CodigoAsesor() As Integer
        Get
            Return intCodigoAsesor
        End Get
        Set(ByVal value As Integer)
            intCodigoAsesor = value
        End Set
    End Property
    Private intCodigoAsesor As Integer

    Public Property TipoOT() As Integer
        Get
            Return intTipoOT
        End Get
        Set(ByVal value As Integer)
            intTipoOT = value
        End Set
    End Property
    Private intTipoOT As Integer

    Public Property CotizacionCancelled() As SAPbobsCOM.BoYesNoEnum
        Get
            Return strCotizacionCancelled
        End Get
        Set(ByVal value As SAPbobsCOM.BoYesNoEnum)
            strCotizacionCancelled = value
        End Set
    End Property
    Private strCotizacionCancelled As SAPbobsCOM.BoYesNoEnum

    Public Property CotizacionDocumentStatus() As SAPbobsCOM.BoStatus
        Get
            Return strCotizacionDocumentStatus
        End Get
        Set(ByVal value As SAPbobsCOM.BoStatus)
            strCotizacionDocumentStatus = value
        End Set
    End Property
    Private strCotizacionDocumentStatus As SAPbobsCOM.BoStatus

    Public Property CardType() As SAPbobsCOM.BoCardTypes
        Get
            Return strCardType
        End Get
        Set(ByVal value As SAPbobsCOM.BoCardTypes)
            strCardType = value
        End Set
    End Property
    Private strCardType As SAPbobsCOM.BoCardTypes
End Class
