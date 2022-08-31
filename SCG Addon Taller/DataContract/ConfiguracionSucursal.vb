Imports System

<Serializable()> _
Public Class ConfiguracionSucursal
    Public Property DocEntry() As Integer
        Get
            Return intDocEntry
        End Get
        Set(ByVal value As Integer)
            intDocEntry = value
        End Set
    End Property
    Private intDocEntry As Integer

    Public Property SucursalID() As String
        Get
            Return strSucursalID
        End Get
        Set(ByVal value As String)
            strSucursalID = value
        End Set
    End Property
    Private strSucursalID As String

    Public Property CentroCosto() As String
        Get
            Return strCentroCosto
        End Get
        Set(ByVal value As String)
            strCentroCosto = value
        End Set
    End Property
    Private strCentroCosto As String

    Public Property UsaRequisiciones() As Boolean
        Get
            Return blnUsaRequisiciones
        End Get
        Set(ByVal value As Boolean)
            blnUsaRequisiciones = value
        End Set
    End Property
    Private blnUsaRequisiciones As Boolean

    Public Property UsaRepuestos() As Boolean
        Get
            Return blnUsaRepuestos
        End Get
        Set(ByVal value As Boolean)
            blnUsaRepuestos = value
        End Set
    End Property
    Private blnUsaRepuestos As Boolean

    Public Property UsaServicios() As Boolean
        Get
            Return blnUsaServicios
        End Get
        Set(ByVal value As Boolean)
            blnUsaServicios = value
        End Set
    End Property
    Private blnUsaServicios As Boolean

    Public Property UsaServiciosExternos() As Boolean
        Get
            Return blnUsaServiciosExternos
        End Get
        Set(ByVal value As Boolean)
            blnUsaServiciosExternos = value
        End Set
    End Property
    Private blnUsaServiciosExternos As Boolean

    Public Property UsaSuministros() As Boolean
        Get
            Return blnUsaSuministros
        End Get
        Set(ByVal value As Boolean)
            blnUsaSuministros = value
        End Set
    End Property
    Private blnUsaSuministros As Boolean

    Public Property AsignaTecnicoOT() As Boolean
        Get
            Return blnAsignaTecnicoOT
        End Get
        Set(ByVal value As Boolean)
            blnAsignaTecnicoOT = value
        End Set
    End Property
    Private blnAsignaTecnicoOT As Boolean

    Public Property ValidaRequisicionesPendientes() As Boolean
        Get
            Return blnValidaRequisicionesPendientes
        End Get
        Set(ByVal value As Boolean)
            blnValidaRequisicionesPendientes = value
        End Set
    End Property
    Private blnValidaRequisicionesPendientes As Boolean

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

    Public Property ValidaEntregaRepuesto() As Boolean
        Get
            Return blnValidaEntregaRepuesto
        End Get
        Set(ByVal value As Boolean)
            blnValidaEntregaRepuesto = value
        End Set
    End Property
    Private blnValidaEntregaRepuesto As Boolean

    Public Property FinalizaOTActPendientes() As Boolean
        Get
            Return blnFinalizaOTActPendientes
        End Get
        Set(ByVal value As Boolean)
            blnFinalizaOTActPendientes = value
        End Set
    End Property
    Private blnFinalizaOTActPendientes As Boolean

    Public Property UsaOfertaCompra() As Boolean
        Get
            Return blnUsaOfertaCompra
        End Get
        Set(ByVal value As Boolean)
            blnUsaOfertaCompra = value
        End Set
    End Property
    Private blnUsaOfertaCompra As Boolean

    Public Property UsaOrdenCompra() As Boolean
        Get
            Return blnUsaOrdenCompra
        End Get
        Set(ByVal value As Boolean)
            blnUsaOrdenCompra = value
        End Set
    End Property
    Private blnUsaOrdenCompra As Boolean

    Public Property CantidadCopiasOT() As Integer
        Get
            Return intCantidadCopiasOT
        End Get
        Set(ByVal value As Integer)
            intCantidadCopiasOT = value
        End Set
    End Property
    Private intCantidadCopiasOT As Integer

    Public Property SerieNumeracionTrasnferencia() As String
        Get
            Return strSerieNumeracionTrasnferencia
        End Get
        Set(ByVal value As String)
            strSerieNumeracionTrasnferencia = value
        End Set
    End Property
    Private strSerieNumeracionTrasnferencia As String

    Public Property BodegaRepuesto() As String
        Get
            Return strBodegaRepuesto
        End Get
        Set(ByVal value As String)
            strBodegaRepuesto = value
        End Set
    End Property
    Private strBodegaRepuesto As String

    Public Property BodegaProceso() As String
        Get
            Return strBodegaProceso
        End Get
        Set(ByVal value As String)
            strBodegaProceso = value
        End Set
    End Property
    Private strBodegaProceso As String

    Public Property BodegaServicioExterno() As String
        Get
            Return strBodegaServicioExterno
        End Get
        Set(ByVal value As String)
            strBodegaServicioExterno = value
        End Set
    End Property
    Private strBodegaServicioExterno As String

    Public Property BodegaSuministro() As String
        Get
            Return strBodegaSuministro
        End Get
        Set(ByVal value As String)
            strBodegaSuministro = value
        End Set
    End Property
    Private strBodegaSuministro As String

    Public Property AsignacionAutomaticaColaborador() As Boolean
        Get
            Return blnAsignacionAutomaticaColaborador
        End Get
        Set(ByVal value As Boolean)
            blnAsignacionAutomaticaColaborador = value
        End Set
    End Property
    Private blnAsignacionAutomaticaColaborador As Boolean

    Public Property UsaClienteLead() As Boolean
        Get
            Return blnUsaClienteLead
        End Get
        Set(ByVal value As Boolean)
            blnUsaClienteLead = value
        End Set
    End Property
    Private blnUsaClienteLead As Boolean

    Public Property CentroCostoTipoOT() As String
        Get
            Return strCentroCostoTipoOT
        End Get
        Set(ByVal value As String)
            strCentroCostoTipoOT = value
        End Set
    End Property
    Private strCentroCostoTipoOT As String

    Public Property UsaServiciosExternosInventariables() As Boolean
        Get
            Return blnUsaServiciosExternosInventariables
        End Get
        Set(ByVal value As Boolean)
            blnUsaServiciosExternosInventariables = value
        End Set
    End Property
    Private blnUsaServiciosExternosInventariables As Boolean

    Public Property UsaUbicaciones() As Boolean
        Get
            Return blnUsaUbicaciones
        End Get
        Set(ByVal value As Boolean)
            blnUsaUbicaciones = value
        End Set
    End Property
    Private blnUsaUbicaciones As Boolean

    Public Property UsuarioDisminuye() As Boolean
        Get
            Return blnUsuarioDisminuye
        End Get
        Set(ByVal value As Boolean)
            blnUsuarioDisminuye = value
        End Set
    End Property
    Private blnUsuarioDisminuye As Boolean

    Public Property UsaCosteoManoObra() As Boolean
        Get
            Return blnUsaCosteoManoObra
        End Get
        Set(ByVal value As Boolean)
            blnUsaCosteoManoObra = value
        End Set
    End Property
    Private blnUsaCosteoManoObra As Boolean

    Public Property UsaTiempoEstandar() As Boolean
        Get
            Return blnUsaTiempoEstandar
        End Get
        Set(ByVal value As Boolean)
            blnUsaTiempoEstandar = value
        End Set
    End Property
    Private blnUsaTiempoEstandar As Boolean

    Public Property UsaTiempoReal() As Boolean
        Get
            Return blnUsaTiempoReal
        End Get
        Set(ByVal value As Boolean)
            blnUsaTiempoReal = value
        End Set
    End Property
    Private blnUsaTiempoReal As Boolean

    Public Property MonedaManoObra() As String
        Get
            Return strMonedaManoObra
        End Get
        Set(ByVal value As String)
            strMonedaManoObra = value
        End Set
    End Property
    Private strMonedaManoObra As String

    Public Property CuentaCreditoManoObra() As String
        Get
            Return strCuentaCreditoManoObra
        End Get
        Set(ByVal value As String)
            strCuentaCreditoManoObra = value
        End Set
    End Property
    Private strCuentaCreditoManoObra As String

    Public Property UsaAsientosGastos() As Boolean
        Get
            Return blnUsaAsientosGastos
        End Get
        Set(ByVal value As Boolean)
            blnUsaAsientosGastos = value
        End Set
    End Property
    Private blnUsaAsientosGastos As Boolean

    Public Property MonedaOtrosGastos() As String
        Get
            Return strMonedaOtrosGastos
        End Get
        Set(ByVal value As String)
            strMonedaOtrosGastos = value
        End Set
    End Property
    Private strMonedaOtrosGastos As String

    Public Property CuentaDebitoOtrosGastos() As String
        Get
            Return strCuentaDebitoOtrosGastos
        End Get
        Set(ByVal value As String)
            strCuentaDebitoOtrosGastos = value
        End Set
    End Property
    Private strCuentaDebitoOtrosGastos As String

    Public Property CuentaCreditoOtrosGastos() As String
        Get
            Return strCuentaCreditoOtrosGastos
        End Get
        Set(ByVal value As String)
            strCuentaCreditoOtrosGastos = value
        End Set
    End Property
    Private strCuentaCreditoOtrosGastos As String

    Public Property UsaAsientoServicioExterno() As Boolean
        Get
            Return blnUsaAsientoServicioExterno
        End Get
        Set(ByVal value As Boolean)
            blnUsaAsientoServicioExterno = value
        End Set
    End Property
    Private blnUsaAsientoServicioExterno As Boolean

    Public Property UsaDimensiones() As Boolean
        Get
            Return blnUsaDimensiones
        End Get
        Set(ByVal value As Boolean)
            blnUsaDimensiones = value
        End Set
    End Property
    Private blnUsaDimensiones As Boolean
End Class
