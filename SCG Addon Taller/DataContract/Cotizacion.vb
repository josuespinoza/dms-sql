Imports System

<Serializable()> _
Public Class Cotizacion
    Public Property DocEntry() As Integer
        Get
            Return intDocEntry
        End Get
        Set(ByVal value As Integer)
            intDocEntry = value
        End Set
    End Property
    Private intDocEntry As Integer

    Public Property LineNum() As Integer
        Get
            Return intLineNum
        End Get
        Set(ByVal value As Integer)
            intLineNum = value
        End Set
    End Property
    Private intLineNum As Integer

    Public Property ItemCode() As String
        Get
            Return strItemCode
        End Get
        Set(ByVal value As String)
            strItemCode = value
        End Set
    End Property
    Private strItemCode As String

    Public Property Currency As String
        Get
            Return strCurrency
        End Get
        Set(value As String)
            strCurrency = value
        End Set
    End Property
    Private strCurrency As String

    Public Property Description() As String
        Get
            Return strDescription
        End Get
        Set(ByVal value As String)
            strDescription = value
        End Set
    End Property
    Private strDescription As String

    Public Property Quantity() As Double
        Get
            Return dblQuantity
        End Get
        Set(ByVal value As Double)
            dblQuantity = value
        End Set
    End Property
    Private dblQuantity As Double

    Public Property OriginalQuantity() As Double
        Get
            Return dblOriginalQuantity
        End Get
        Set(ByVal value As Double)
            dblOriginalQuantity = value
        End Set
    End Property
    Private dblOriginalQuantity As Double

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

    Public Property CantidadPendienteTraslado() As Double
        Get
            Return dblCantidadPendienteTraslado
        End Get
        Set(ByVal value As Double)
            dblCantidadPendienteTraslado = value
        End Set
    End Property
    Private dblCantidadPendienteTraslado As Double

    Public Property CantidadPendienteDevolucion() As Double
        Get
            Return dblCantidadPendienteDevolucion
        End Get
        Set(ByVal value As Double)
            dblCantidadPendienteDevolucion = value
        End Set
    End Property
    Private dblCantidadPendienteDevolucion As Double

    Public Property Price() As Double
        Get
            Return dblPrice
        End Get
        Set(ByVal value As Double)
            dblPrice = value
        End Set
    End Property
    Private dblPrice As Double

    Public Property TreeType() As SAPbobsCOM.BoItemTreeTypes
        Get
            Return strTreeType
        End Get
        Set(ByVal value As SAPbobsCOM.BoItemTreeTypes)
            strTreeType = value
        End Set
    End Property

    Public Property DiscPrcnt() As Double
        Get
            Return dblDiscPrcnt
        End Get
        Set(ByVal value As Double)
            dblDiscPrcnt = value
        End Set
    End Property
    Private dblDiscPrcnt As Double

    Private strTreeType As SAPbobsCOM.BoItemTreeTypes

    Public Property IdRepxOrd() As Integer
        Get
            Return intIdRepxOrd
        End Get
        Set(ByVal value As Integer)
            intIdRepxOrd = value
        End Set
    End Property
    Private intIdRepxOrd As Integer

    Public Property Aprobado() As Integer
        Get
            Return intAprobado
        End Get
        Set(ByVal value As Integer)
            intAprobado = value
        End Set
    End Property
    Private intAprobado As Integer

    Public Property Trasladado() As Integer
        Get
            Return intTrasladado
        End Get
        Set(ByVal value As Integer)
            intTrasladado = value
        End Set
    End Property
    Private intTrasladado As Integer

    Public Property AprobadoOriginal() As Integer
        Get
            Return intAprobadoOriginal
        End Get
        Set(ByVal value As Integer)
            intAprobadoOriginal = value
        End Set
    End Property
    Private intAprobadoOriginal As Integer

    Public Property TrasladadoOriginal() As Integer
        Get
            Return intTrasladadoOriginal
        End Get
        Set(ByVal value As Integer)
            intTrasladadoOriginal = value
        End Set
    End Property
    Private intTrasladadoOriginal As Integer

    Public Property Costo() As Double
        Get
            Return dblCosto
        End Get
        Set(ByVal value As Double)
            dblCosto = value
        End Set
    End Property
    Private dblCosto As Double

    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property OTHija() As Integer
        Get
            Return intOTHija
        End Get
        Set(ByVal value As Integer)
            intOTHija = value
        End Set
    End Property
    Private intOTHija As Integer

    Public Property Entregado() As String
        Get
            Return strEntregado
        End Get
        Set(ByVal value As String)
            strEntregado = value
        End Set
    End Property
    Private strEntregado As String

    Public Property TipoArticulo() As Integer
        Get
            Return intTipoArticulo
        End Get
        Set(ByVal value As Integer)
            intTipoArticulo = value
        End Set
    End Property
    Private intTipoArticulo As Integer

    Public Property Comprar() As String
        Get
            Return strComprar
        End Get
        Set(ByVal value As String)
            strComprar = value
        End Set
    End Property
    Private strComprar As String

    Public Property ID() As String
        Get
            Return strID
        End Get
        Set(ByVal value As String)
            strID = value
        End Set
    End Property
    Private strID As String

    Public Property Sucursal() As String
        Get
            Return strSucursal
        End Get
        Set(ByVal value As String)
            strSucursal = value
        End Set
    End Property
    Private strSucursal As String

    Public Property CentroCosto() As String
        Get
            Return strCentroCosto
        End Get
        Set(ByVal value As String)
            strCentroCosto = value
        End Set
    End Property
    Private strCentroCosto As String

    Public Property TipoOT() As String
        Get
            Return strTipoOT
        End Get
        Set(ByVal value As String)
            strTipoOT = value
        End Set
    End Property
    Private strTipoOT As String

    Public Property Procesar() As Boolean
        Get
            Return blnProcesar
        End Get
        Set(ByVal value As Boolean)
            blnProcesar = value
        End Set
    End Property
    Private blnProcesar As Boolean

    Public Property ProcesarInteger() As Integer
        Get
            Return intProcesar
        End Get
        Set(ByVal value As Integer)
            intProcesar = value
        End Set
    End Property
    Private intProcesar As Integer

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

    Public Property BodegaRepuesto() As String
        Get
            Return strBodegaRepuesto
        End Get
        Set(ByVal value As String)
            strBodegaRepuesto = value
        End Set
    End Property
    Private strBodegaRepuesto As String

    Public Property BodegaServicio() As String
        Get
            Return strBodegaServicio
        End Get
        Set(ByVal value As String)
            strBodegaServicio = value
        End Set
    End Property
    Private strBodegaServicio As String

    Public Property BodegaSuministro() As String
        Get
            Return strBodegaSuministro
        End Get
        Set(ByVal value As String)
            strBodegaSuministro = value
        End Set
    End Property
    Private strBodegaSuministro As String

    Public Property BodegaServicioExterno() As String
        Get
            Return strBodegaServicioExterno
        End Get
        Set(ByVal value As String)
            strBodegaServicioExterno = value
        End Set
    End Property
    Private strBodegaServicioExterno As String

    Public Property BodegaProceso() As String
        Get
            Return strBodegaProceso
        End Get
        Set(ByVal value As String)
            strBodegaProceso = value
        End Set
    End Property
    Private strBodegaProceso As String

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

    Public Property DuracionEstandar() As Integer
        Get
            Return intDuracionEstandar
        End Get
        Set(ByVal value As Integer)
            intDuracionEstandar = value
        End Set
    End Property
    Private intDuracionEstandar As Integer

    Public Property EstadoActividad() As String
        Get
            Return strEstadoActividad
        End Get
        Set(ByVal value As String)
            strEstadoActividad = value
        End Set
    End Property
    Private strEstadoActividad As String

    Public Property EmpleadoAsignado() As String
        Get
            Return strEmpleadoAsignado
        End Get
        Set(ByVal value As String)
            strEmpleadoAsignado = value
        End Set
    End Property
    Private strEmpleadoAsignado As String

    Public Property NombreEmpleado() As String
        Get
            Return strNombreEmpleado
        End Get
        Set(ByVal value As String)
            strNombreEmpleado = value
        End Set
    End Property
    Private strNombreEmpleado As String

    Public Property CostoReal() As Double
        Get
            Return dblCostoReal
        End Get
        Set(ByVal value As Double)
            dblCostoReal = value
        End Set
    End Property
    Private dblCostoReal As Double


    Public Property CostoEstandar() As Double
        Get
            Return dblCostoEstandar
        End Get
        Set(ByVal value As Double)
            dblCostoEstandar = value
        End Set
    End Property
    Private dblCostoEstandar As Double

    Public Property FechaInicioActividad() As String
        Get
            Return strFechaInicioActividad
        End Get
        Set(ByVal value As String)
            strFechaInicioActividad = value
        End Set
    End Property
    Private strFechaInicioActividad As String

    Public Property FechaFinalActividad() As String
        Get
            Return strFechaFinalActividad
        End Get
        Set(ByVal value As String)
            strFechaFinalActividad = value
        End Set
    End Property
    Private strFechaFinalActividad As String

    Public Property HoraInicio() As String
        Get
            Return strHoraInicio
        End Get
        Set(ByVal value As String)
            strHoraInicio = value
        End Set
    End Property
    Private strHoraInicio As String

    Public Property FaseProduccion() As String
        Get
            Return strFaseProduccion
        End Get
        Set(ByVal value As String)
            strFaseProduccion = value
        End Set
    End Property
    Private strFaseProduccion As String

    Public Property PertenecePaquete() As String
        Get
            Return strPertenecePaquete
        End Get
        Set(ByVal value As String)
            strPertenecePaquete = value
        End Set
    End Property
    Private strPertenecePaquete As String

    Public Property CantidadStock() As Double
        Get
            Return dblCantidadStock
        End Get
        Set(ByVal value As Double)
            dblCantidadStock = value
        End Set
    End Property
    Private dblCantidadStock As Double

    Public Property TipoMovimiento() As Integer
        Get
            Return intTipoMovimiento
        End Get
        Set(ByVal value As Integer)
            intTipoMovimiento = value
        End Set
    End Property
    Private intTipoMovimiento As Integer

    Public Property Resultado() As String
        Get
            Return strResultado
        End Get
        Set(ByVal value As String)
            strResultado = value
        End Set
    End Property
    Private strResultado As String

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

    Public Property PaquetePadre() As String
        Get
            Return strPaquetePadre
        End Get
        Set(ByVal value As String)
            strPaquetePadre = value
        End Set
    End Property
    Private strPaquetePadre As String

    Public Property VisOrder() As Integer
        Get
            Return intVisOrder
        End Get
        Set(ByVal value As Integer)
            intVisOrder = value
        End Set
    End Property
    Private intVisOrder As Integer

    Public Property RequisicionDevolucion() As Boolean
        Get
            Return blnRequisicionDevolucion
        End Get
        Set(ByVal value As Boolean)
            blnRequisicionDevolucion = value
        End Set
    End Property
    Private blnRequisicionDevolucion As Boolean

    Public Property EsAdicional() As Boolean
        Get
            Return blnEsAdicional
        End Get
        Set(ByVal value As Boolean)
            blnEsAdicional = value
        End Set
    End Property
    Private blnEsAdicional As Boolean

    Public Property ProcesamientoLinea() As Integer
        Get
            Return intProcesamientoLinea
        End Get
        Set(ByVal value As Integer)
            intProcesamientoLinea = value
        End Set
    End Property
    Private intProcesamientoLinea As Integer

    Public Property VatGroup() As String
        Get
            Return strVatGroup
        End Get
        Set(ByVal value As String)
            strVatGroup = value
        End Set
    End Property
    Private strVatGroup As String

    Public Property TaxCode() As String
        Get
            Return strTaxCode
        End Get
        Set(ByVal value As String)
            strTaxCode = value
        End Set
    End Property
    Private strTaxCode As String

    Public Property FreeText() As String
        Get
            Return strFreeText
        End Get
        Set(ByVal value As String)
            strFreeText = value
        End Set
    End Property
    Private strFreeText As String

    Public Property LineDscPrcnt As String
        Get
            Return strLineDscPrcnt
        End Get
        Set(value As String)
            strLineDscPrcnt = value
        End Set
    End Property
    Private strLineDscPrcnt As String
End Class


