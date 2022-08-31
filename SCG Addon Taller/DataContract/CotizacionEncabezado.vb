Imports System

<Serializable()> _
Public Class CotizacionEncabezado
    Public Property DocEntry() As Integer
        Get
            Return intDocEntry
        End Get
        Set(ByVal value As Integer)
            intDocEntry = value
        End Set
    End Property
    Private intDocEntry As Integer

    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property Comments() As String
        Get
            Return strComments
        End Get
        Set(value As String)
            strComments = value
        End Set
    End Property
    Private strComments As String

    Public Property SlpCode() As String
        Get
            Return strSlpCode
        End Get
        Set(value As String)
            strSlpCode = value
        End Set
    End Property
    Private strSlpCode As String

    Public Property Sucursal() As String
        Get
            Return strSucural
        End Get
        Set(ByVal value As String)
            strSucural = value
        End Set
    End Property
    Private strSucural As String

    Public Property GeneraOT() As Integer
        Get
            Return intGeneraOT
        End Get
        Set(ByVal value As Integer)
            intGeneraOT = value
        End Set
    End Property
    Private intGeneraOT As Integer

    Public Property EstadoCotizacionID() As String
        Get
            Return strEstadoCotizacionID
        End Get
        Set(ByVal value As String)
            strEstadoCotizacionID = value
        End Set
    End Property
    Private strEstadoCotizacionID As String

    Public Property FechaCreacionOT() As Date
        Get
            Return dateFechaCreacionOT
        End Get
        Set(ByVal value As Date)
            dateFechaCreacionOT = value
        End Set
    End Property
    Private dateFechaCreacionOT As Date

    Public Property HoraCreacionOT() As Date
        Get
            Return dateHoraCreacionOT
        End Get
        Set(ByVal value As Date)
            dateHoraCreacionOT = value
        End Set
    End Property
    Private dateHoraCreacionOT As Date

    Public Property GeneraRecepcion() As String
        Get
            Return strGeneraRecepcion
        End Get
        Set(ByVal value As String)
            strGeneraRecepcion = value
        End Set
    End Property
    Private strGeneraRecepcion As String

    Public Property OTPadre() As String
        Get
            Return strOTPadre
        End Get
        Set(ByVal value As String)
            strOTPadre = value
        End Set
    End Property
    Private strOTPadre As String

    Public Property NoOTReferencia() As String
        Get
            Return strNoOTReferencia
        End Get
        Set(ByVal value As String)
            strNoOTReferencia = value
        End Set
    End Property
    Private strNoOTReferencia As String

    Public Property NumeroVIN() As String
        Get
            Return strNumeroVIN
        End Get
        Set(ByVal value As String)
            strNumeroVIN = value
        End Set
    End Property
    Private strNumeroVIN As String

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

    Public Property CodigoMarca() As String
        Get
            Return strCodigoMarca
        End Get
        Set(ByVal value As String)
            strCodigoMarca = value
        End Set
    End Property
    Private strCodigoMarca As String

    Public Property CodigoProyecto() As String
        Get
            Return strCodigoProyecto
        End Get
        Set(ByVal value As String)
            strCodigoProyecto = value
        End Set
    End Property
    Private strCodigoProyecto As String

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

    Public Property CardCode() As String
        Get
            Return strCardCode
        End Get
        Set(ByVal value As String)
            strCardCode = value
        End Set
    End Property
    Private strCardCode As String

    Public Property CardType() As SAPbobsCOM.BoCardTypes
        Get
            Return strCardType
        End Get
        Set(ByVal value As SAPbobsCOM.BoCardTypes)
            strCardType = value
        End Set
    End Property
    Private strCardType As SAPbobsCOM.BoCardTypes

    Public Property NoVisita() As String
        Get
            Return strNoVisita
        End Get
        Set(ByVal value As String)
            strNoVisita = value
        End Set
    End Property
    Private strNoVisita As String

    Public Property EstadoCotizacion() As String
        Get
            Return strEstadoCotizacion
        End Get
        Set(ByVal value As String)
            strEstadoCotizacion = value
        End Set
    End Property
    Private strEstadoCotizacion As String

    Public Property NoSerieCita() As String
        Get
            Return strNoSerieCita
        End Get
        Set(ByVal value As String)
            strNoSerieCita = value
        End Set
    End Property
    Private strNoSerieCita As String

    Public Property CardName() As String
        Get
            Return strCardName
        End Get
        Set(ByVal value As String)
            strCardName = value
        End Set
    End Property
    Private strCardName As String

    Public Property NombreAsesor() As String
        Get
            Return strNombreAsesor
        End Get
        Set(ByVal value As String)
            strNombreAsesor = value
        End Set
    End Property
    Private strNombreAsesor As String


    Public Property Cono() As String
        Get
            Return strCono
        End Get
        Set(ByVal value As String)
            strCono = value
        End Set
    End Property
    Private strCono As String

    Public Property Year() As String
        Get
            Return strYear
        End Get
        Set(ByVal value As String)
            strYear = value
        End Set
    End Property
    Private strYear As String

    Public Property DescripcionMarca() As String
        Get
            Return strDescripcionMarca
        End Get
        Set(ByVal value As String)
            strDescripcionMarca = value
        End Set
    End Property
    Private strDescripcionMarca As String

    Public Property DescripcionModelo() As String
        Get
            Return strDescripcionModelo
        End Get
        Set(ByVal value As String)
            strDescripcionModelo = value
        End Set
    End Property
    Private strDescripcionModelo As String

    Public Property DescripcionEstilo() As String
        Get
            Return strDescripcionEstilo
        End Get
        Set(ByVal value As String)
            strDescripcionEstilo = value
        End Set
    End Property
    Private strDescripcionEstilo As String

    Public Property CodigoModelo() As String
        Get
            Return strCodigoModelo
        End Get
        Set(ByVal value As String)
            strCodigoModelo = value
        End Set
    End Property
    Private strCodigoModelo As String

    Public Property CodigoEstilo() As String
        Get
            Return strCodigoEstilo
        End Get
        Set(ByVal value As String)
            strCodigoEstilo = value
        End Set
    End Property
    Private strCodigoEstilo As String

    Public Property HorasServicio() As Double
        Get
            Return dblHorasServicio
        End Get
        Set(ByVal value As Double)
            dblHorasServicio = value
        End Set
    End Property
    Private dblHorasServicio As Double

    Public Property Kilometraje() As Integer
        Get
            Return dblKilometraje
        End Get
        Set(ByVal value As Integer)
            dblKilometraje = value
        End Set
    End Property
    Private dblKilometraje As Integer

    Public Property Placa() As String
        Get
            Return strPlaca
        End Get
        Set(ByVal value As String)
            strPlaca = value
        End Set
    End Property
    Private strPlaca As String

    Public Property NombreClienteOT() As String
        Get
            Return strNombreClienteOT
        End Get
        Set(ByVal value As String)
            strNombreClienteOT = value
        End Set
    End Property
    Private strNombreClienteOT As String

    Public Property CodigoClienteOT() As String
        Get
            Return strCodigoClienteOT
        End Get
        Set(ByVal value As String)
            strCodigoClienteOT = value
        End Set
    End Property
    Private strCodigoClienteOT As String

    Public Property FechaRecepcion() As Date
        Get
            Return dateFechaRecepcion
        End Get
        Set(ByVal value As Date)
            dateFechaRecepcion = value
        End Set
    End Property
    Private dateFechaRecepcion As Date

    Public Property HoraRecepcion() As String
        Get
            Return strHoraRecepcion
        End Get
        Set(ByVal value As String)
            strHoraRecepcion = value
        End Set
    End Property
    Private strHoraRecepcion As String

    Public Property NivelGasolina() As Integer
        Get
            Return intNivelGasolina
        End Get
        Set(ByVal value As Integer)
            intNivelGasolina = value
        End Set
    End Property
    Private intNivelGasolina As Integer

    Public Property Observaciones() As String
        Get
            Return strObservaciones
        End Get
        Set(ByVal value As String)
            strObservaciones = value
        End Set
    End Property
    Private strObservaciones As String

    Public Property CodeMaestroVehiculo() As Integer
        Get
            Return intCodeMaestroVehiculo
        End Get
        Set(ByVal value As Integer)
            intCodeMaestroVehiculo = value
        End Set
    End Property
    Private intCodeMaestroVehiculo As Integer

    Public Property NoCita As String
        Get
            Return strNoCita
        End Get
        Set(value As String)
            strNoCita = value
        End Set
    End Property
    Private strNoCita As String

    Public Property DocCurrency() As String
        Get
            Return strDocCurrency
        End Get
        Set(ByVal value As String)
            strDocCurrency = value
        End Set
    End Property
    Private strDocCurrency As String

    Public Property BPLId As String
        Get
            Return sBPLId
        End Get
        Set(ByVal value As String)
            sBPLId = value
        End Set
    End Property
    Private sBPLId As String

    Public Property Serie As String
        Get
            Return strSerie
        End Get
        Set(value As String)
            strSerie = value
        End Set
    End Property
    Private strSerie As String

    Public Property DiscountPercent As String
        Get
            Return strDiscountPercent
        End Get
        Set(value As String)
            strDiscountPercent = value
        End Set
    End Property
    Private strDiscountPercent As String
End Class
