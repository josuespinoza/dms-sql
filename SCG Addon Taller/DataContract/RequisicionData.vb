Imports System

<Serializable()> _
Public Class RequisicionData
    Public Property DocEntry() As Integer
        Get
            Return intDocEntry
        End Get
        Set(ByVal value As Integer)
            intDocEntry = value
        End Set
    End Property
    Private intDocEntry As Integer

    Public Property ItemCode() As String
        Get
            Return strItemCode
        End Get
        Set(ByVal value As String)
            strItemCode = value
        End Set
    End Property
    Private strItemCode As String

    Public Property Description() As String
        Get
            Return strDescription
        End Get
        Set(ByVal value As String)
            strDescription = value
        End Set
    End Property
    Private strDescription As String


    Public Property CantidadDisponible() As Double
        Get
            Return dblCantidadDisponible
        End Get
        Set(ByVal value As Double)
            dblCantidadDisponible = value
        End Set
    End Property
    Private dblCantidadDisponible As Double

    Public Property TipoArticulo() As Integer
        Get
            Return intTipoArticulo
        End Get
        Set(ByVal value As Integer)
            intTipoArticulo = value
        End Set
    End Property
    Private intTipoArticulo As Integer

    Public Property DescripcionTipoArticulo() As String
        Get
            Return strDescripcionTipoArticulo
        End Get
        Set(ByVal value As String)
            strDescripcionTipoArticulo = value
        End Set
    End Property
    Private strDescripcionTipoArticulo As String

    Public Property CantidadTransferir() As Double
        Get
            Return dblCantidadTransferir
        End Get
        Set(ByVal value As Double)
            dblCantidadTransferir = value
        End Set
    End Property
    Private dblCantidadTransferir As Double

    Public Property CantidadOriginal() As Double
        Get
            Return dblCantidadOriginal
        End Get
        Set(ByVal value As Double)
            dblCantidadOriginal = value
        End Set
    End Property
    Private dblCantidadOriginal As Double

    Public Property CantidadAjuste() As Double
        Get
            Return dblCantidadAjuste
        End Get
        Set(ByVal value As Double)
            dblCantidadAjuste = value
        End Set
    End Property
    Private dblCantidadAjuste As Double

    Public Property CantidadRecibida() As Double
        Get
            Return dblCantidadRecibida
        End Get
        Set(ByVal value As Double)
            dblCantidadRecibida = value
        End Set
    End Property
    Private dblCantidadRecibida As Double

    Public Property CantidadSolicitada() As Double
        Get
            Return dblCantidadSolicitada
        End Get
        Set(ByVal value As Double)
            dblCantidadSolicitada = value
        End Set
    End Property
    Private dblCantidadSolicitada As Double

    Public Property CantidadPendiente() As Double
        Get
            Return dblCantidadPendiente
        End Get
        Set(ByVal value As Double)
            dblCantidadPendiente = value
        End Set
    End Property
    Private dblCantidadPendiente As Double

    Public Property CantidadPendienteBodega() As Double
        Get
            Return dblCantidadPendienteBodega
        End Get
        Set(ByVal value As Double)
            dblCantidadPendienteBodega = value
        End Set
    End Property
    Private dblCantidadPendienteBodega As Double

    Public Property CantidadPendienteDevolucion() As Double
        Get
            Return dblCantidadPendienteDevolucion
        End Get
        Set(ByVal value As Double)
            dblCantidadPendienteDevolucion = value
        End Set
    End Property
    Private dblCantidadPendienteDevolucion As Double

    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property Entregado() As String
        Get
            Return strEntregado
        End Get
        Set(ByVal value As String)
            strEntregado = value
        End Set
    End Property
    Private strEntregado As String

    Public Property ID() As String
        Get
            Return strID
        End Get
        Set(ByVal value As String)
            strID = value
        End Set
    End Property
    Private strID As String

    Public Property Procesar() As Boolean
        Get
            Return blnProcesar
        End Get
        Set(ByVal value As Boolean)
            blnProcesar = value
        End Set
    End Property
    Private blnProcesar As Boolean

    Public Property BodegaOrigen() As String
        Get
            Return strBodegaOrigen
        End Get
        Set(ByVal value As String)
            strBodegaOrigen = value
        End Set
    End Property
    Private strBodegaOrigen As String

    Public Property BodegaDestino() As String
        Get
            Return strBodegaDestino
        End Get
        Set(ByVal value As String)
            strBodegaDestino = value
        End Set
    End Property
    Private strBodegaDestino As String

    Public Property UsaUbicaciones() As Boolean
        Get
            Return blnUsaUbicaciones
        End Get
        Set(ByVal value As Boolean)
            blnUsaUbicaciones = value
        End Set
    End Property
    Private blnUsaUbicaciones As Boolean

    Public Property UbicacionDBP() As String
        Get
            Return strUbicacionDBP
        End Get
        Set(ByVal value As String)
            strUbicacionDBP = value
        End Set
    End Property
    Private strUbicacionDBP As String

    Public Property CodigoEstadoLinea() As Integer
        Get
            Return intCodigoEstadoLinea
        End Get
        Set(ByVal value As Integer)
            intCodigoEstadoLinea = value
        End Set
    End Property
    Private intCodigoEstadoLinea As Integer

    Public Property EstadoLinea() As String
        Get
            Return strEstadoLinea
        End Get
        Set(ByVal value As String)
            strEstadoLinea = value
        End Set
    End Property
    Private strEstadoLinea As String

    Public Property CentroCosto() As String
        Get
            Return strCentroCosto
        End Get
        Set(ByVal value As String)
            strCentroCosto = value
        End Set
    End Property
    Private strCentroCosto As String

    Public Property Check() As Integer
        Get
            Return intCheck
        End Get
        Set(ByVal value As Integer)
            intCheck = value
        End Set
    End Property
    Private intCheck As Integer

    Public Property UbicacionOrigen() As String
        Get
            Return strUbicacionOrigen
        End Get
        Set(ByVal value As String)
            strUbicacionOrigen = value
        End Set
    End Property
    Private strUbicacionOrigen As String

    Public Property UbicacionDestino() As String
        Get
            Return strUbicacionDestino
        End Get
        Set(ByVal value As String)
            strUbicacionDestino = value
        End Set
    End Property
    Private strUbicacionDestino As String

    Public Property DescripcionUbicacionOrigen() As String
        Get
            Return strDescripcionUbicacionOrigen
        End Get
        Set(ByVal value As String)
            strDescripcionUbicacionOrigen = value
        End Set
    End Property
    Private strDescripcionUbicacionOrigen As String

    Public Property DescripcionUbicacionDestino() As String
        Get
            Return strDescripcionUbicacionDestino
        End Get
        Set(ByVal value As String)
            strDescripcionUbicacionDestino = value
        End Set
    End Property
    Private strDescripcionUbicacionDestino As String

    Public Property LineNumOrigen() As Integer
        Get
            Return intLineNumOrigen
        End Get
        Set(ByVal value As Integer)
            intLineNumOrigen = value
        End Set
    End Property
    Private intLineNumOrigen As Integer

    Public Property DocumentoOrigen() As Integer
        Get
            Return intDocumentoOrigen
        End Get
        Set(ByVal value As Integer)
            intDocumentoOrigen = value
        End Set
    End Property
    Private intDocumentoOrigen As Integer

    Public Property LineaSucursalID() As String
        Get
            Return strLineaSucursalID
        End Get
        Set(ByVal value As String)
            strLineaSucursalID = value
        End Set
    End Property
    Private strLineaSucursalID As String

    Public Property CodigoEstadoRequisicion() As Integer
        Get
            Return intCodigoEstadoRequisicion
        End Get
        Set(ByVal value As Integer)
            intCodigoEstadoRequisicion = value
        End Set
    End Property
    Private intCodigoEstadoRequisicion As Integer

    Public Property EstadoRequisicion() As String
        Get
            Return strEstadoRequisicion
        End Get
        Set(ByVal value As String)
            strEstadoRequisicion = value
        End Set
    End Property
    Private strEstadoRequisicion As String

    Public Property CodigoCliente() As String
        Get
            Return strCodigoCliente
        End Get
        Set(ByVal value As String)
            strCodigoCliente = value
        End Set
    End Property
    Private strCodigoCliente As String

    Public Property NombreCliente() As String
        Get
            Return strNombreCliente
        End Get
        Set(ByVal value As String)
            strNombreCliente = value
        End Set
    End Property
    Private strNombreCliente As String

    Public Property CodigoTipoRequisicion() As Integer
        Get
            Return intCodigoTipoRequisicion
        End Get
        Set(ByVal value As Integer)
            intCodigoTipoRequisicion = value
        End Set
    End Property
    Private intCodigoTipoRequisicion As Integer

    Public Property TipoRequisicion() As String
        Get
            Return strTipoRequisicion
        End Get
        Set(ByVal value As String)
            strTipoRequisicion = value
        End Set
    End Property
    Private strTipoRequisicion As String

    Public Property TipoDocumento() As String
        Get
            Return strTipoDocumento
        End Get
        Set(ByVal value As String)
            strTipoDocumento = value
        End Set
    End Property
    Private strTipoDocumento As String

    Public Property Usuario() As String
        Get
            Return strUsuario
        End Get
        Set(ByVal value As String)
            strUsuario = value
        End Set
    End Property
    Private strUsuario As String

    Public Property Comentario() As String
        Get
            Return strComentario
        End Get
        Set(ByVal value As String)
            strComentario = value
        End Set
    End Property
    Private strComentario As String

    Public Property Data() As String
        Get
            Return strData
        End Get
        Set(ByVal value As String)
            strData = value
        End Set
    End Property
    Private strData As String

    Public Property SucursalID() As String
        Get
            Return strSucursalID
        End Get
        Set(ByVal value As String)
            strSucursalID = value
        End Set
    End Property
    Private strSucursalID As String

    Public Property IdRepxOrd() As String
        Get
            Return strIdRepxOrd
        End Get
        Set(ByVal value As String)
            strIdRepxOrd = value
        End Set
    End Property
    Private strIdRepxOrd As String

    Public Property RequisicionDevolucion() As Boolean
        Get
            Return blnRequisicionDevolucion
        End Get
        Set(ByVal value As Boolean)
            blnRequisicionDevolucion = value
        End Set
    End Property
    Private blnRequisicionDevolucion As Boolean

    Public Property BodegaUbicacion() As String
        Get
            Return strBodegaUbicacion
        End Get
        Set(ByVal value As String)
            strBodegaUbicacion = value
        End Set
    End Property
    Private strBodegaUbicacion As String

    Public Property Aplicado() As Boolean
        Get
            Return blnAplicado
        End Get
        Set(ByVal value As Boolean)
            blnAplicado = value
        End Set
    End Property
    Private blnAplicado As Boolean

    Public Property SerieCita() As String
        Get
            Return strSerieCita
        End Get
        Set(ByVal value As String)
            strSerieCita = value
        End Set
    End Property
    Private strSerieCita As String

    Public Property NumeroCita() As String
        Get
            Return strNumeroCita
        End Get
        Set(ByVal value As String)
            strNumeroCita = value
        End Set
    End Property
    Private strNumeroCita As String
End Class
