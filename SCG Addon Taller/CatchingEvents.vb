Option Strict Off
Option Explicit On

Imports DMS_Addon.Agendas
Imports DMS_Addon.ComprasCls
Imports DMS_Addon.LlamadaServicio
Imports DMS_Addon.Requisiciones
Imports DMS_Addon.GastosContratoVentas
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.Requisiciones
Imports SAPbouiCOM
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework
Imports DMS_Addon.Ventas
Imports System.Collections.Generic
Imports System.Linq
Imports SCG.Financiamiento
Imports Company = SAPbobsCOM.Company
Imports SCG.Integration
Imports SCG.Integration.InterfaceDPM

Partial Public Class CatchingEvents

    Friend Shared DBPassword As String
    Friend Shared DBUser As String
    Friend Shared IDFormMarcaEstiloModelo As String
    Friend Shared DireccionConfiguracion As String
    Friend Shared ListaFormsCFL As String

#Region "Declaraciones"

    Dim oGestorMenu As GestorMenu
    Dim oGestorFormularios As GestorFormularios

    'DocumentoProcesoCompra
    Private m_oDocumentoProcesoCompra As DocumentoProcesoCompra

    'Agregado 14072010
    Dim editCell As EditText

    'Variables
    Private m_intRowOT As Integer = 0
    Private m_blnUsaPlacas As Boolean
    Public Shared m_blnUsaOrdenesDeTrabajo As Boolean
    Private m_blnUsaPlanDeVentas As Boolean
    Private m_blnUsaVehículos As Boolean
    Private m_blnUsaCosteoVehículo As Boolean
    Private m_blnUsaAsocXEspecif As Boolean
    Private m_blnLineaOT As Boolean = False
    Private m_blnFinanciamiento As Boolean
    Private oVersionModuloCita As frmListaCitas.VersionModuloCita

    Private WithEvents SBO_Application As Application
    Private m_oCompany As New Company
    Private sXml As String

    Private oMatrix As Matrix
    Public oFilters As EventFilters

    Private oFilter As EventFilter
    Private m_oFormularioPresupuestos As FormularioPresupuestos

    Private m_oLlamadaServicio As FormularioLLamadaServicioSBO
    Public Shared m_oAgendas As FormularioAgendaSBO
    Private m_oCompras As ComprasCls
    'objeto compras en proceso de ventas
    Private m_oComprasEnVentas As ComprasEnProcesoVentas
    Private m_oRecepcionVHUI As RecepcionVehiculo
    'Pagos recibidos
    Private m_oMediosPago As MediosDePago
    'campana
    Private m_oCampana As Campaña
    Private m_oOportunidadVenta As OportunidadVenta
    Private m_oFormularioRequisiciones As FormularioRequisiconesConPermisos
    Private m_oFormularioListadoRequisiciones As ListadoRequisicionesConPermisos
    Private m_oFormularioListadoSolicitudEspecificos As ListadoSolicitudEspecificosConPermisos
    Private m_oFormularioSolicitudEspecificos As SolicitudEspecificosConPermisos
    Private m_oFormularioPrestamo As PrestamoFormularioConPermisos
    Private m_oFormularioConfFinanc As ConfiguracionFormularioConPermisos
    Private m_oMenuReportesFinanciamiento As ReportesConPermisos
    Private m_oFormularioEstadoCuentas As EstadosCuentaConPermisos
    Private m_oFormularioHistoricoPagos As HistoricoPagosConPermisos
    Private m_oFormularioCuotasVencidas As CuotasVencidasConPermisos
    Private m_oFormularioSaldos As SaldosConPermisos
    Private m_oFormularioPlanPagos As PlanPagosFormulario
    Private m_oFormularioPlacas As ExpedienteFormularioConPermisos
    Private m_oFormularioPlacaGrupos As GrupoFormularioConPermisos
    Private m_oMenuReportesPlacas As ReportesPlacasConPermisos
    Private m_oFormularioVehiculoTipoEvento As VehiculosTipoEventoConPermisos
    Private m_oFormularioContratoTraspaso As ContratoTraspasoConPermisos
    Private m_oFormularioComision As ComisionConPermisos
    Private m_oFormularioVehiculosProblemas As VehiculosProblemasConPermisos
    Private m_oFormularioAsocArticuloxEspecif As AsociacionArticuloxEspecifConPermisos
    Private m_oManejadorRequisicionesTraslados As ManejadorRequisicionesTraslados
    Private m_oSalidaMercancia As SalidasMercancia
    Private m_oEntradaMercancia As EntradasMercancia
    Private m_oCotizacion As CotizacionCLS
    Private m_oVehiculos As VehiculosCls
    Private m_oListaCVXUnidad As ListaContXUnidad
    Private m_oTransferenciaItems As TransferenciaItems
    Private m_oCVenta As ContratoVentasCls
    'Entradas de mercancias en compras
    Private m_oEntradaMercanciasEnCompras As EntradasMercanciasEnCompras
    Private m_oCotizacion_ProcesaOT As Cotizacion_ProcesaOT

    'Formulario carga masiva de vehículos
    'Private m_oFormularioCargaMasivaVehiculos As CargaMasivaVehiculos
    'Private Const mc_strCargaMasivaVehiculos As String = "SCGD_CMDV"
    'factura proveedores 
    Private m_oFacturaProveedores As FacturaProveedores
    'Notas de Credito Proveedores
    Private m_oNotaCreditoProveedor As NotaCreditoProveedor
    'factura proveedores 
    Private m_oDevolucionMercancia As DevolucionMercancia
    'factura clintes 
    Private m_oFacturaClientes As FacturaClientes
    'Reportes de contratos de venta 
    Private m_oReporteCV As ContratoVentasReportesCls
    'Estados OT
    Private m_oEstadosOT As EntregaVehiculosOT
    'unidades por nivel
    Private m_oUsuariosxNivel As UsuariosPorNAprob
    Private m_oBuscadorCV As BuscadorContratoVentaCls
    Private m_oListadoCV As ListadoContratosCls
    Private m_oGoodReceive As GoodReceiveCls
    Private m_oGoodIssue As GoodIssueCls
    Private m_oFacturaInterna As FacturaInterna
    Private m_oListadoGR As Listado_GRCls
    Private m_oRecosteos As RecosteosCls
    Private m_oVehiculosACostear As VehiculosSinCostearCls
    Private m_oCFLbyFS As ChooseFromListByFormattedSCls
    Private m_oPermisos As NivelesPlanVentasCls
    Private m_oNivelesPV As NivelesPV
    Private m_oPropiedades As ConfiguracionPropiedadesVehiculosCls
    Private m_oInventarioVehiculos As ConsultaInventarioVehiculosCls
    Private m_oReportesCosteo As ReportesCosteoCls
    Private m_oLineasFactura As ConfiguracionLineasAdicionalesFacturaCls
    Private m_oLineasDesgloce As ConfiguracionLineasDesgloceCobroCls
    Private m_oTransaccionesCompras As ConfiguracionTransaccionesCompraCls
    Private m_oConfiguracionGeneral As ConfiguracionesGenerelesAddOn
    Private m_strMensajePreFormDataEvent As String
    Private m_oPagoRecibido As PagoRecibido
    Private m_oRefacturacion As Refacturacion

    Private m_oFormularioOrdenTrabajo As OrdenTrabajoConPermisos
    Private m_oFormularioAsignacionMultipleOT As AsignacionMultipleConPermisos
    Private m_oFormularioRazonSuspension As RazonesSuspensionConPermisos
    Private m_oFormularioAdicionalesOT As AdicionalesOTConPermisos
    Private m_oFormularioTrackRep As SCG.ServicioPostVenta.TrackingRepuestos
    Private m_oFormularioFinAct As SCG.ServicioPostVenta.FinalizaActividad
    Private m_oFormularioTrackSolEspecificos As SCG.ServicioPostVenta.TrackingSolEspecificos


    Private m_oFormularioAdicionalesCitasArt As BuscadorArticulosCitas
    Private m_oFormularioDocumentoCompra As DocumentoCompraConPermisos
    Private m_oFormularioBuscarProveedores As BuscadorProveedoresConPermisos
    Private m_oFormularioOTEspecial As OTEspecialConPermisos

    Private m_FormularioBalance As BalanceFormulario
    Private m_oFormularioCitaXTipoAgenda As CitasPorTipoAgendaFecha
    Private m_oFormularioUnidadesVendidas As UnidadesVendidasConPermisos

    Private m_oFormMantenEspecificacionPorModelo As EspecificacionPorModeloCls

    'vendedores por tipo de inventario
    Private m_oFormularioPermisosVendedoresXTI As VendedoresPorTipoInventario

    Private m_oFormularioGastosCV As GastosAdicionales

    'Formulario para busqeudas de ordenes de trabajo 
    Private m_oFormularioBusquedaOT As BusquedaOrdenesTrabajo

    'ROINER CAMACHO ESQUIVEL
    'Formulario para incluir repuestos a ordenes de trabajo 
    Private m_oFormularioIncluirRepOT As IncluirRepuestosOT

    'Formulario para seleccionar repuestos
    Private m_oFormularioSeleccionaRepuestosOT As SeleccionarRepuestosOT

    'Formulario para incluir repuestos a ordenes de trabajo 
    Private m_oFormularioConfigNivelesAprob As ConfiguracionNivAprobacion

    'Formulario Reporte de Ordenes de Trabajo 
    Private m_oFormularioBalanceOT As BalanceOrdenesTrabajo

    ''Formulario para incluir Gastos/Costos a ordenes de trabajo
    Private m_oFormularioIncluirGastoOT As IncluirGastosCostosOT

    ''Formulario para seleccionar Gastos
    Private m_oFormularioSeleccionaGastosOT As SeleccionarGastosCostosOT
    Private m_oFormularioCrearDocumentosGastos As CrearDocumentosGastosCostos

    '*****************
    Private m_oMenuConfiguracionDMS As ConfiguracionesDMSConPermisos
    Private m_oFormularioParametrosAplicacion As ParametrosDeAplicacionConPermisos
    Private m_oFormularioAgendasConfiguracion As AgendasConfiguracion
    Private m_oFormularioCitas As CitasReservacion
    Private m_oFormularioBusquedasCitas As BusquedasCitasConPermisos
    Private m_oFormularioCargarPanelCitas As CargarPanelCitasConPermisos
    Private m_oFormularioSuspensionAgenda As AgendaSuspension
    Private m_oFormularioConfMsJ As ConfiguracionMensajeriaDMS
    Private m_oFormConfInterfazFord As ConfiguracionInterfazFord
    Private m_oFormularioAvaUs As AvaluoUsados

    Private m_oListadoContratosReversados As ListadoContratoReversadosCls
    Private m_oListaContratos_a_Reversar As ListadoContratosFacturadosReversadosCls
    Private m_oListaContratosSegPV As ListadoContratosSeguroPostVenta
    Private m_oFormularioSeriesNumeracion As NumeracionSeries
    '*****************
    Private m_oSolicitudOTEspecial As SolicitudOrdenEspecial
    Private m_oReporteOrdenesEspeciales As ReporteOrdenesEspeciales

    '*****************
    Private m_oDimensionesContables As DimensionContableDMS

    '***************** Buscar Articulo Venta en Maestro Vehiculo
    Private m_oFormularioVehiculoArticuloVenta As VehiculoArticuloVenta
    Private m_oFormularioVehiculoColorSeleccion As VehiculoColoresSeleccion
    Private m_oFormularioSeleciconMarcaEstiloModelo As VehiculoSeleccionMarcaEstilo

    '***************** Seleccionar vehiculos para devolucion
    Private m_oFormularioSeleccionaUnidadDev As SeleccionUnidadDevolucion
    Private m_oFormularioSeleccionLineasPedidos As SeleccionLineasPedidos
    Private m_oFormularioSeleccionLineasRecepcion As SeleccionLineasRecepcion

    '----------------------PROTOTIPO
    Private g_oFormularioVisitas As Visita
    Private g_oFormularioBusquedaControlProceso As BusquedaControlProceso
    Private g_oFormularioControlCrearVisita As ControlCrearVisita
    Private g_oFormularioControlVisita As ControlVisita
    Private g_oFormularioOfertaVentas As OfertaVentas

    '--------------------------Seleccion de ubicaciones
    Private m_oFormSeleccionUbicaciones As ListaUbicaciones
    '--------------------------Seleccion de ubicaciones

    '***************** PEDIDOS A FABRICA

    Private m_oFormularioPedidoVehiculos As PedidoDeVehiculos
    Private m_oFormularioEntradaDeVehiculos As EntradaDeVehiculos
    Private m_oFormularioCosteoDeEntradas As CosteoDeEntradas
    Private m_oFormularioDevolucionDeVehiculos As DevolucionDeVehiculos

    Private mc_StrPedidoVehiculos As String = "SCGD_PDV"
    Friend Shared mc_strEntradaDeVehiculos As String = "SCGD_EDV"
    Private mc_strCosteoDeEntradas As String = "SCGD_CDP"
    Friend Shared mc_strDevolucionDeVehiculos As String = "SCGD_DDV"

    Private mc_strSeleccionarUnidadDev As String = "SCGD_SUD"
    Private mc_strSeleccionLineasPedidos As String = "SCGD_SLP"
    Private mc_strSeleccionLinasRecepcion As String = "SCGD_SLR"

    '*****************
    'para Traslado de Costos
    Private m_oTrasladoCostos As TrasladoCostosDeUnidadesCls

    'CosteoMultiplesUnidades
    Private m_oCosteoMultiplesUnidades As CosteoMultiple

    'SalidaMultiplesUnidades
    Private m_oSalidasMultiplesUnidades As SalidaMultiple

    Private m_oEmbarqueVehiculos As EmbarqueVehiculos

    'Reporte Bodega Proceso
    Private m_oFormularioBodegaProceso As BodegaProceso
    'Reporte Socios de Negocios
    Private m_oFormularioSociosNegocios As ReporteSociosNegocios
    'Reporte Vehiculos Recurrentes Taller
    Private m_oFormularioReporteVehiculosRecurrentesTaller As ReporteVehiculosRecurrentesTaller
    'Reporte VentasX Asesor de Servicio
    Private m_oFormularioReporteVentasXAsesorServicio As ReporteVentasXAsesorServicio
    'Reporte Facturacion Vehiculo
    Private m_oFormularioFacturacionvehiculo As FacturacionVehiculosPorVendedor

    'Reporte Ordenes de Trabajo por Estado
    Private m_oFormularioOrdenesDeTrabajoPorEstado As OrdenesDeTrabajoPorEstado
    'Reporte Historial Vehiculo
    Private m_oFormularioHistorialVehiculo As HistorialVehiculo
    'Reporte Facturacion OT Interna
    Private m_oFormularioFactutacionOTInternas As FactutacionOTInternas
    'Reporte de antiguedad de vehículos
    Private m_oFormularioReporteAntiguedadVehiculos As ReporteAntiguedadVehiculos
    'Reporte Facturacion Órdenes Trabajo
    Private m_oFormularioReporteFacturacionOT As ReporteFacturacionOrdenesTrabajo
    'Reporte Servicios Externos por OT
    Private m_oFormularioReporteServiciosExternosXOrden As ReporteServiciosExternosXOrden
    'Reporte de facturacion de mecanicos
    Private m_oFormularioReporteFacturacionMecanicos As ReporteFacturacionMecanicos
    '
    Private m_oFormularioReporteFinanciamientoContratoVentas As ReporteFinanciamientoContratoVentas


    'Clases para manejo de Generacion de Factura Interna desde pantalla de Orden de Venta
    Private m_oTipoOtInterna As TipoOtInterna
    Private m_oOrdenVenta As OrdenVenta

    Private m_oSolicitaOtEsp As SolicitaOTEspecial
    Private m_oAsignacionMultiple As AsignacionMultiple
    Private m_oFormularioListaPreciosSeleccion As ListaPreciosSeleccion
    Private m_oFormConfIntTDS As ConfiguracionInterfazTSD
    Private m_oFormConfIntAudatex As ConfiguracionInterfazAudatex
    Private m_oFormularioSeleccionEmpleados As ListaEmpleadosSeleccion
    Private m_oFormularioComentariosCV As ComentariosHistorial
    Private m_oFormularioComentariosIV As ComentariosInventarioV
    Private m_oSociosNegocio As MaestroSociosNegocio
    Private m_oMaestroEmpleados As MaestroEmpleados

    'Kardex Inventario Vehiculos
    Private m_oFormularioKardexInventarioVehiculo As KardexInventarioVehiculo

    Private Const mc_strIDBotonEjecucion As String = "1"
    Private Const mc_strIDMatriz As String = "38"
    Private Const mc_strBotonFotos As String = "SCGD_btVf"


    Private Const mc_strFacturadeCompra As String = "141"
    Private Const mc_strOrdenDeCompra As String = "142"
    Private Const mc_strIdFormaCotizacion As String = "149"
    Private Const mc_strFormMediosPago As String = "146"
    Private Const mc_strNotadeCredito As String = "181"
    Private Const mc_strEntrdadeinventario As String = "721"
    Private Const mc_strSalidadeInventario As String = "720"
    Private Const mc_strEntradadeMercancia As String = "143"
    Private Const mc_strSalidadeMercancia As String = "182"
    Private Const mc_strOrdenDeVenta As String = "139"
    Private Const mc_strFacturaReserva As String = "60091"
    Private Const mc_strFacturaCliente As String = "133"
    Private Const mc_strBoleta As String = "65304"
    Private Const mc_strFacturaExentaDeudores As String = "65302"
    Private Const mc_strEntregas As String = "140"
    Private Const mc_strFacturaProveedores As String = "60092"
    Private Const mc_strTrasladoInventario As String = "940"
    Private Const mc_stridGeneraOV As String = "SCGD_DET_2"
    Private Const mc_strMatrizFormularios As String = "38"
    Private Const mc_strIDItemCodeColumn As String = "1"
    Private Const mc_strIDItemNameColumn As String = "3"
    Private Const mc_strMaestroArticulos As String = "150"
    Private Const mc_strUsuarios As String = "20700"
    Private Const mc_strMaestroEmpleados As String = "60100"
    Private Const mc_strSociosNegocios As String = "134"
    Private Const mc_strVisualizadorfotos As String = "149"

    Private Const mc_strOportunidadVenta As String = "320"
    Private Const mc_strSalidaMercancia As String = "720"
    Private Const mc_strEntradaMercancia As String = "721"
    Private Const mc_strOfertaDeCompra As String = "540000988"
    '******************
    Private Const mc_strNotaDebito As String = "65306"
    Private Const mc_strRegistroDiario As String = "392"

    Private Const mc_strPagoRecibido As String = "170"

    Private Const mc_strRefacturacion As String = "SCGD_Refact"

    'campañas
    Private Const mc_strIdFormCampaña As String = "1320000022"
    Private Const mc_strCampana As String = "1320000022"
    '******************

    '-----------------------------------------------------------------------
    'para el formulario de los Documentos Preliminares
    Private Const mc_strDocumentoPreliminar As String = "3002"
    Private blnTransferenciaDesdeDraft As Boolean = False
    Public intValor As Integer
    Public oMatrixDraft As Matrix
    Private Const m_CantidadRecibida As Integer = 3
    Public blnManejarFormularioTransferencia As Boolean = False
    Public blnCerrarFormTransferencia As Boolean = False
    Private blnDraft As Boolean = False

    '-----------------------------------------------------------------------

    'Private clsExceptionHandler As New SCGExceptionHandler.clsExceptionHandler
    Public Const gc_NombreAplicacion As String = "Addon SCG DMSOne"

    'Constantes
    Private Const mc_strMaestroVehiculos As String = "SCGD_frmMaestroVehiculos"
    Private Const mc_strControlVehiculo As String = "SCGD_DET_1"
    Private Const mc_strUniqueID As String = "SCGD_DET_1"
    Private Const mc_intNoModoFind As Integer = 1

    Private Const mc_strContratoVenta As String = "SCGD_frmContVent"
    Private Const mc_strControlCVenta As String = "SCGD_frmContVent"
    Private Const mc_strUniqueIDCV As String = "SCGD_frmContVent"
    Private Const mc_strUniqueIDBCV As String = "SCGD_frmBuscador_CV"
    Private Const mc_strUniqueIDLCV As String = "SCGD_frmListadoCV"
    '*******************************************
    Private Const mc_strUniqueIDLCVR As String = "SCGD_Revertir"
    '*******************************************
    Private Const mc_strUniqueIDVSC As String = "SCGD_frmVeh_Cos"
    Private Const mc_strUniqueIDNivelesPV As String = "SCGD_PRM"

    Private Const mc_strUniqueIDPropiedades As String = "SCGD_PROP"
    Private Const mc_strUniqueIDLineasFactura As String = "SCGD_ConfLineasSum"
    Private Const mc_strUniqueIDLineasDesgloce As String = "SCGD_CONFLINEASRES"
    Private Const mc_strUniqueIDTransaccionesCompras As String = "SCGD_TRANS_C"
    Private Const mc_strUniqueIDInventariovehiculos As String = "SCGD_INV_VEHI"
    Private Const mc_strUniqueIDReportesCosteo As String = "SCGD_Rep_Cost"
    Private Const mc_strUniqueIDConfiguracionesGenerales As String = "SCGD_ADMIN"
    Private Const mc_strUniqueIDCosteoMultiplesUnidades As String = "SCGD_frm_CMU"

    Private Const mc_strUniqueIDSalidaMultiplesUnidades As String = "SCGD_frm_SMU"

    'Listado de Contratos de Venta

    '**************************************************

    Private Const mc_strUniqueIDContRevertidos As String = "SCGD_LCR"
    '**************************************************
    Private Const mc_strUniqueIDListaARevertir As String = "SCGD_frmListadoAReversar"
    Private Const mc_strUniqueIDConSegPV As String = "SCGD_CSPV"
    Private Const mc_strUniqueIDLCVLAR As String = "SCGD_frmListadoAReversar"
    '**************************************************************
    Private Const mc_strUITrasladoCostos As String = "SCGD_frmTrasCos"
    Private Const mc_strUITrasC As String = "SCGD_TCU"
    Private Const mc_strUIListaContXUnidad As String = "SCGD_CONTXVEH"


    'Vehículos Sin Costear

    Private Const mc_strUIGOODENT As String = "SCGD_GOODENT"
    Private Const mc_strUIGOODISSUE As String = "SCGD_GOODISSUE"
    Private Const mc_strUILISTADOGR As String = "SCGD_List_GR"
    Private Const mc_strUIRecosteos As String = "SCGD_Recosteo"
    Private Const mc_strUIFacturasInt As String = "SCGD_FAC_INT"
    Private Const mc_strUISCGD_Revertir As String = "SCGD_Revertir"


    'Requisiciones
    Private Const mc_strUISCGD_FormRequisicion As String = "SCGD_FormRequisicion"

    'Financiamiento
    Private Const mc_strUISCGD_FormPrestamo As String = "SCGD_PRESTAMOS"
    Private Const mc_strUISCGD_FormConfFin As String = "SCGD_CONF_FIN"
    Private Const mc_strUISCGD_FormPlanPagos As String = "SCGD_PlanTeorico"
    Private Const mc_strUISCGD_FormEstadosCuenta As String = "SCGD_EST_CUENTA"
    Private Const mc_strUISCGD_FormHistoricoPagos As String = "SCGD_HIST_PAGOS"
    Private Const mc_strUISCGD_FormCuotasVencidas As String = "SCGD_VENCIDAS"
    Private Const mc_strUISCGD_FormSaldos As String = "SCGD_SALDOS"

    'Placas
    Private Const mc_strUISCGD_FormPlacas As String = "SCGD_PLACAS"
    Private Const mc_strUISCGD_FormPlacaGrupos As String = "SCGD_GRUPO_PLACAS"
    Private Const mc_strUISCGD_FormVehiculosTipoEvento As String = "SCGD_VEH_TIPEVEN"
    Private Const mc_strUISCGD_FormContratoTraspaso As String = "SCGD_VEH_TRASP"
    Private Const mc_strUISCGD_FormComision As String = "SCGD_COM_PLC"
    Private Const mc_strUISCGD_FormVehiculosProblemas As String = "SCGD_VEH_PROB"

    'Balance
    Private Const mc_FormBalance As String = "SCGD_Balance"

    'Gastos
    Private Const mc_strUISCGD_FormGastos As String = "SCGD_Gastos"

    'reporte Unidades Vendidas
    Private Const mc_strUISCGD_RptUnidadesVend As String = "SCGD_RUV"
    Private Const mc_strUSCG_MantEspecifiacionXModelo As String = "SCGD_EPM"

    ' Mantenimiento Especificos por Modelo o estilo
    Private Const mc_strUISCGD_EspecificosModelo As String = "SCGD_EPM"

    'busqueda de OT
    Private Const strMenuBusqeudaOt As String = "SCGD_BOT"

    'ListadoSolicitudEspecificos
    Private Const strMenuListadoSolEsp As String = "SCGD_LSE"

    'SolicitudEspecificos
    Private Const strMenuSolEsp As String = "SCGD_SolEs"

    'Incluir repuesto OT
    Friend Shared strMenuIncluirRepOT As String = "SCGD_INR"

    'Incluir Gastos a la OT
    Friend Shared strMenuIncluirGastosOT As String = "SCGD_ING"

    'ConfigNivAprob
    Private Const strMenuConfigNivAprob As String = "SCGD_MSJ"

    'Asociación Artículo por especificación
    Private Const mc_strUISCGD_FormAsocArtxEsp = "SCGD_ASOC_AXE"

    'Confuraciones DMS (Externo)
    Private Const mc_strUISCGD_FormParamAplicacion As String = "SCGD_PDA"
    Private Const mc_strUISCGD_FormAgendasConfiguracion As String = "SCGD_AGD"
    Private Const mc_strUISCGD_FormConfMSJ As String = "SCGD_CMSJ"
    Private Const mc_strUISCGD_FormAVA As String = "SCGD_AVA"

    'Configuracion Interface Ford
    Private Const mc_strUISCGD_ConfFordInterface As String = "SCGD_FICon"

    'Configuracion Interface TSD
    Private Const mc_strUISCGD_ConfTSDInterface As String = "SCGD_TIC"

    'Configuracion Interface TSD
    Private Const mc_strUISCGD_ConfJohnDeereInterface As String = "SCGD_IJD"

    'Configuracion Interface Audatex
    Private Const mc_strUISCGD_ConfAudatexInterface As String = "SCGD_AIC"

    'Citas
    Private Const mc_strUISCGD_Citas As String = "SCGD_CIT"
    Private Const mc_strUISCGD_BusqCitas As String = "SCGD_BCT"
    Private Const mc_strUISCGD_CargPanelCitas As String = "SCGD_CPC"
    Private Const mc_strUISCGD_SuspenderAgenda As String = "SCGD_SDA"

    'Reporte Ordenes Especiales.
    Private Const mc_strUISCGD_ReporteOrdenes As String = "SCGD_ROT"


    'Solicitud OT Especial
    Private Const mc_strUISCGD_SolicituOTE As String = "SCGD_SOT"

    'Dimensiones Contables DMS
    Private Const mc_strUISCGD_DimensionContableDMS As String = "SCGD_DIM"
    Private Const mc_strUISCGD_DimensionContableDMSOTs As String = "SCGD_DOT"

    Private Const mc_strGeneraOV As String = "SCGD_GeneraOV"
    Private Const mc_strGeneraFI As String = "SCGD_GeneraFI"

    Private m_strConsulta As String
    Private m_strUsuario As String
    Private m_strSucursalTaller As String

    'Choose From List cargados por formatted search
    Private m_strUIDVehiMarcaEtc As String

    Private m_strDocEntryByBeforeAction As String = ""
    Private m_strDocEntryByStatusBar As String = ""

    Private m_striUDF_Cod_Unid As String = "U_SCGD_Cod_Unid"

    'se utiliza para validar que tipo de matriz se esta accediendo
    Private Const intMatrizServicio As String = "39"
    Private Const intMatrizArticulos As String = "38"
    Private Const intMatrizAsiento As String = "76"

    Private blnFilaTieneOT As Boolean = False
    Private blnOVFilaTieneOT As Boolean = False
    Private m_NumOT_OV As String = ""

    'variable booleana para determinar si se deshabilita el menu de navegacion
    'en caso de que los contratos sean visto por empleados
    Private blnUsaEmpleadoContrato As Boolean = False
    Private blnEsNivelTramite As Boolean = False

    Private Structure FormaCotizacionVehiculo

        Public intCountCotizacion As Integer
        Public intCountVehiculo As Integer

    End Structure


    Private m_udoMenusPlanVentas As Dictionary(Of String, Utilitarios.MenusPlanVentas)
    Private m_udoMenu As Utilitarios.MenusPlanVentas
    Private m_blnOrdenCompraActualizada As Boolean = False

    Private Const _nombreExeDMS As String = "SCG DMS One.exe"
    Private Const _menuSCG As String = "SCGD_MenuSCG"
    Private Const _menuDMS As String = "SCGD_MenuDMS"
    Private Const _imgDMSSBO As String = "DMSOne.bmp"

    Private Const menuFinanc As String = "SCGD_FNC"

    'Placas
    Private Const menuPlacas As String = "SCGD_PLA"

    'Citas
    Private Const menuCitas As String = "SCGD_CITS"

    'Interface John Deere
    Private Const menuInterfaceJohnDeere As String = "SCGD_PLA"

    'Informes DMS
    Private Const menuInformesDMS As String = "SCGD_IND"
    Private Const mc_strUIDCitasXTipo As String = "SCGD_CXT"
    Private Const mc_strUID_FORM_CitasXTipo As String = "SCGD_CITASXT"
    Private Const mc_strUID_FORM_BodegasP As String = "SCGD_RBPP"
    Private Const mc_strUID_FORM_ReporteSociosNegocios As String = "SCGD_RSN"
    Private Const mc_strUID_FORM_ReporteVehiculosRecurrentesTaller As String = "SCGD_RVRT"
    Private Const mc_strUID_FORM_ReporteVentasXAsesorServicio As String = "SCGD_RVAS"
    Private Const mc_strUID_FORM_FacturacioVehi As String = "SCGD_FVP"
    Private Const mc_strUID_FORM_OrdenesTrabajoEstado As String = "SCGD_OTE"
    Private Const mc_strUID_FORM_ReporteHistorialVehiculo As String = "SCGD_RHV"
    Private Const mc_strUID_FORM_ReporteFacturacionInterna As String = "SCGD_RFI"
    Private Const mc_strUID_FORM_ReporteFacturacionOT As String = "SCGD_RFO"
    Private Const mc_strUID_FORM_ReporteAntiguedadVehiculos As String = "SCGD_RAV"
    Private Const mc_strUID_FORM_ReporteServiciosExternosXOrden As String = "SCGD_RSE"
    Private Const mc_strUID_FORM_ReporteFacMecanicos As String = "SCGD_FPM"

    'Contratos Venta reportes

    Private Const mc_strFormReportesCV As String = "SCGD_REP_CV"

    'Estados OT
    Private Const mc_strFORM_EstadosOT As String = "SCGD_ESTOT"

    'Unidades por Nivel
    Private Const mc_strFormUnidadesPorNivel As String = "SCGD_UXN"

    'vendedores por niveles de aprobacion 
    Private Const mc_strFormVendedoresTipoInv As String = "SCGD_VENDXTI"

    'formulario de busqueda de ot
    Private Const mc_strUIDFormBusquedas As String = "SCGD_BOT"

    'formulario de incluir repuestos a la ot
    Private Const mc_strUIDFormIncluirRepOT As String = "SCGD_INR"

    'formulario de incluir Costos y gastos y a la ot
    Private Const mc_strUIDFormIncluirGastosOT As String = "SCGD_ING"

    'formulario de CONFIGURACION de mensajeria
    Private Const mc_strUIDFormConfiguracionMSJ As String = "SCGD_MSJ"

    'Formulario de seleccion de repuestos para la OT
    Private Const mc_strUIDFormSeleccionRepOT As String = "SCGD_SROT"

    'Formularios de seleccion de Gastos para la Ot
    Private Const mc_strUIDFormSeleccionGasOT As String = "SCGD_SGOT"

    'Formulario Crear Documentos Gastos/Costos
    Private Const mc_strUIDFormCrearDocGastosCostos As String = "SCGD_GenDoc"
    'Formulario de balance ot rpt 
    Private Const mc_strUIDFormBalanceOT As String = "SCGD_BOR"

    Private Const mc_strVehiTraza As String = "SCGD_VEHITRAZA"
    Private Const mc_strFormCero As String = "0"


    'Formulario Series de Numeración
    Private Const mc_strFormNumeracionSeries As String = "SCGD_NSE"
    Private Const mc_strFormVehiculosArticulosVenta As String = "SCGD_VAV"
    Private Const mc_strFormVehiculoSeleciconColor As String = "SCGD_VSC"
    Private Const mc_strFormSeleccionMarcaEstilo As String = "SCGD_SME"
    Private Const mc_strFormListaPreciosSelecicon As String = "SCGD_VSLP"
    Private Const mc_strFormComentarioHCV As String = "SCGD_CHCV"

    '  Private Const mc_strFormVehiculoSeleccionMarcaEstilo As String = "SCGD_SME"


    Private Const strMenuEmbarqueVehiculos As String = "SCGD_EMV"

    Private Const mc_strSolicitudOTEspecial As String = "SCGD_SOT"

    Private Const mc_strNotaCreditoCliente As String = "179"

    'Nota de Credito Clientes 
    Private m_oNotaCreditoClientes As NotaCreditoClientes

    Private Const m_oVentanaAutorizaciones As String = "50106"
    Private DocAprobacionHabilitado As Boolean = False
    'Documento Tipo Draft
    Private Const mc_strDocDraft As String = "112"

    Private Const mc_strDimensionesContables As String = "SCGD_DIM"

    Private Const mc_strDimensionesContablesOTs As String = "SCGD_DOT"


    Private m_oDimensionesContablesOTs As DimensionContableDMSOTs

    Private Const g_strFormTipoOTInterna As String = "SCGD_TOTI"
    Private Const g_strFormSolOTEspecial As String = "SCGD_SOTE"

    Private Const g_strFormOT As String = "SCGD_ORDT"
    Private Const g_strFormAsigMultOT As String = "SCGD_ASIM"
    Private Const g_strFormRazonSuspension As String = "SCGD_RAZO"
    Private Const g_strFormAdicionalesOT As String = "SCGD_ADIC"
    Private Const g_strFormTrack As String = "SCG_TRA"
    Private Const g_strFormFinAct As String = "SCGD_FIAct"
    Private Const g_strFormAdicionalesArtCitas As String = "SCGD_BCI"
    Private Const g_strFormDocumentoCompra As String = "SCGD_DOCC"
    Private Const g_strFormBusquedaProveedores As String = "SCGD_BPRO"
    Private Const g_strFormOTEspecial As String = "SCGD_OTES"
    Private Const g_strFormAsignacionMultiple As String = "SCGD_ASM"
    Private Const g_strConfMsj As String = "SCGD_CMSJ"
    Private Const mc_strFormListaEmpleados As String = "SCGD_VSEP"
    Private Const mc_strFormKardex As String = "SCGD_KDEX"
    Private Const mc_strFormSelUbi As String = "SCGD_SLUB"
    Private Const mc_strFormLstReq As String = "SCGD_LSRQ"
    Private Const mc_strFormLstSolEsp As String = "SCGD_LSE"
    Private Const mc_strFormSolEsp As String = "SCGD_SolEs"
    Private blnUsaConfiguracionInternaTaller As Boolean = False
    Private intSucursal As Integer

#End Region

#Region "Constructor"

    Public Sub New(ByVal direccionConfiguracion As String, ByVal idFormMarcaEstiloModelo As String, ByVal strListaFormsCFL As String)
        MyBase.New()
        CatchingEvents.DireccionConfiguracion = direccionConfiguracion
        CatchingEvents.IDFormMarcaEstiloModelo = idFormMarcaEstiloModelo
        ListaFormsCFL = strListaFormsCFL
        Dim strUsuario As String
        Dim strSucursal As String
        Dim oDataTableSucursal As Data.DataTable
        Dim oDataRowSucursal As DataRow

        Try
            If SetApplication() Then
                For Each drCmds As DataRow In Utilitarios.EjecutarConsultaDataTable(DMS_Connector.Queries.GetStrSpecificQuery("strCMDS")).Rows
                    Select Case drCmds.Item("Code").ToString.Trim
                        Case "1"
                            If drCmds.Item("Canceled").ToString.Trim.Equals("N") Then m_blnUsaOrdenesDeTrabajo = True
                        Case "2"
                            If drCmds.Item("Canceled").ToString.Trim.Equals("N") Then m_blnUsaVehículos = True
                        Case "3"
                            If m_blnUsaVehículos Then If drCmds.Item("Canceled").ToString.Trim.Equals("N") Then m_blnUsaPlanDeVentas = True
                        Case "4"
                            If m_blnUsaVehículos Then If drCmds.Item("Canceled").ToString.Trim.Equals("N") Then m_blnUsaCosteoVehículo = True
                    End Select
                Next
                strUsuario = SBO_Application.Company.UserName
                blnUsaConfiguracionInternaTaller = Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO)

                If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                    oDataTableSucursal = Utilitarios.EjecutarConsultaDataTable(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strSucursalesOTMult"), strUsuario))
                Else
                    oDataTableSucursal = Utilitarios.EjecutarConsultaDataTable(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strSucursalesOT"), strUsuario))
                End If

                If oDataTableSucursal.Rows.Count <> 0 Then
                    oDataRowSucursal = oDataTableSucursal.Rows(0)
                    strSucursal = oDataRowSucursal.Item("Name")
                    intSucursal = oDataRowSucursal.Item("Code")
                Else
                    strSucursal = String.Empty
                    intSucursal = 0
                End If

                'verifica si el usuario esta asociado a una sucursal con una base de datos de taller asociada
                If String.IsNullOrEmpty(strSucursal) Then
                    m_blnUsaOrdenesDeTrabajo = False
                Else
                    If m_blnUsaOrdenesDeTrabajo Then
                        If Not blnUsaConfiguracionInternaTaller Then

                            'Verifico el valor de la propiedad para los Documento preliminares de transferencia de Stock
                            Dim strCadenaConexionBDTaller As String
                            Utilitarios.DevuelveCadenaConexionBDTaller(DMS_Connector.Company.ApplicationSBO, strCadenaConexionBDTaller)
                            Dim adpConf As New ConfiguracionDataAdapter(strCadenaConexionBDTaller)

                            If Not String.IsNullOrEmpty(adpConf.objDAConexion.ConnectionString.Trim()) Then
                                Dim dstConf As New ConfiguracionDataSet
                                Dim objUtilitariosCls As New Utilitarios
                                adpConf.Fill(dstConf)
                                blnDraft = False
                                If objUtilitariosCls.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, "CreaDraftTransferenciasStock", "") Then
                                    blnDraft = True
                                End If
                            End If
                        Else
                            blnDraft = False
                            If DMS_Connector.Configuracion.ConfiguracionSucursales.Where(Function(fSucu) fSucu.U_Sucurs.Trim() = intSucursal.ToString.Trim()).Count > 0 Then
                                If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(fSucu) fSucu.U_Sucurs.Trim() = intSucursal.ToString.Trim()).U_Requis.Trim = "Y" Then
                                    blnDraft = True
                                End If
                            Else
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.NoExisteConfiguracionSucursal, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
                            End If
                        End If
                    End If
                End If
                Call CrearInstanciasClases(DMS_Connector.Company.ApplicationSBO, DMS_Connector.Company.CompanySBO)
                Call SetFilters()
                SBO_Application.StatusBar.SetText(My.Resources.Resource.AgregandoMenus, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
                'Valida fecha de vencimiento de Licencia
                NotificacionExpiracionLicencia()
                'Agrega Menus
                Call AgregarMenus()
                SBO_Application.StatusBar.SetText(My.Resources.Resource.AddonInicializado, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                sXml = GestorMenu.MenusManager.GenerateXml(MenuAction.Add)
                SBO_Application.LoadBatchActions(sXml)
            End If

        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

#End Region
#Region "Procedimientos y Funciones"

    Private Sub ManejaProcesosAddon()
        Try
            Dim processList As Process()
            Dim currentProcess As Process

            'Dim strProceso_NombreMaquina As String
            Dim strProEjec_ID As String
            Dim strProEjec_IdSession As String
            Dim strProEjec_NombreProceso As String

            'Dim strSystema_NombreMaquina As String
            Dim strProNuev_ID As String
            Dim strProNuev_IdSession As String
            Dim strProNuev_NombreProceso As String

            processList = Process.GetProcesses()
            currentProcess = Process.GetCurrentProcess()

            For Each process As Process In processList

                strProEjec_ID = process.Id.ToString()
                strProEjec_IdSession = process.SessionId.ToString()
                strProEjec_NombreProceso = process.ProcessName

                strProNuev_ID = currentProcess.Id.ToString()
                strProNuev_IdSession = currentProcess.SessionId.ToString()
                strProNuev_NombreProceso = currentProcess.ProcessName

                'strSystema_NombreProceso = "SCG.DMSOne.AddonTaller.Sais"

                If strProEjec_NombreProceso = strProNuev_NombreProceso And
                    strProEjec_IdSession = strProNuev_IdSession And
                    strProEjec_ID <> strProNuev_ID Then

                    process.Kill()

                End If
            Next
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Private Sub SetFilters()
        Try
            Dim blnDevuelveConversionUT As Boolean = False

            If Utilitarios.DevuelveConversionUnidadesTiempo(SBO_Application, blnUsaConfiguracionInternaTaller, intSucursal) <> 0 Then
                blnDevuelveConversionUT = True
            End If

            m_strUIDVehiMarcaEtc = IDFormMarcaEstiloModelo

            'Create a new EventFilters object
            oFilters = New EventFilters

            'Validate
            oFilter = oFilters.Add(BoEventTypes.et_GOT_FOCUS)
            If m_blnUsaOrdenesDeTrabajo Then
                oFilter.AddEx(mc_strIdFormaCotizacion)
            End If
            oFilter.AddEx("SCGD_frmContVent")

            oFilter.AddEx(mc_strTrasladoInventario)


            oFilter = oFilters.Add(BoEventTypes.et_MATRIX_COLLAPSE_PRESSED)
            If m_blnUsaOrdenesDeTrabajo Then
                oFilter.AddEx(mc_strIdFormaCotizacion)
            End If

            'Key pressed
            If m_blnUsaOrdenesDeTrabajo Then
                oFilter = oFilters.Add(BoEventTypes.et_KEY_DOWN)
                oFilter.AddEx(FormularioAgendaSBO.FormType)

                'Item clicked
                oFilter = oFilters.Add(BoEventTypes.et_CLICK)
                oFilter.AddEx(FormularioAgendaSBO.FormType)
                oFilter.AddEx("-9876")

                oFilter = oFilters.Add(BoEventTypes.et_RIGHT_CLICK)
                oFilter.AddEx("139")
                oFilter.AddEx("149")
            End If
            oFilter.AddEx("198")

            'LOAD
            oFilter = oFilters.Add(BoEventTypes.et_FORM_LOAD)
            If m_blnUsaOrdenesDeTrabajo Then
                oFilter.AddEx(mc_strFacturadeCompra) 'AR Invoice Form
                oFilter.AddEx(mc_strNotadeCredito)
                oFilter.AddEx(mc_strEntradadeMercancia)
                oFilter.AddEx(mc_strSalidadeMercancia)
                oFilter.AddEx(mc_strIdFormaCotizacion)
                oFilter.AddEx(mc_strOrdenDeCompra)
                oFilter.AddEx(mc_strOrdenDeVenta)
                oFilter.AddEx(mc_strFacturaCliente)
                oFilter.AddEx(mc_strBoleta)
                oFilter.AddEx(mc_strFacturaReserva)
                oFilter.AddEx(mc_strTrasladoInventario)
                oFilter.AddEx(mc_strFacturaProveedores)
                oFilter.AddEx(mc_stridGeneraOV)
                oFilter.AddEx(mc_strGeneraFI)
                oFilter.AddEx(mc_strMaestroArticulos)

                'Llamada de servicio
                oFilter.AddEx(FormularioLLamadaServicioSBO.FormType)

                oFilter.AddEx(mc_strNotaCreditoCliente)
                oFilter.AddEx("198")
                oFilter.AddEx(m_oVentanaAutorizaciones)

            End If

            oFilter.AddEx(mc_strUITrasC)


            'Linked Botton
            oFilter = oFilters.Add(BoEventTypes.et_MATRIX_LINK_PRESSED)
            If m_blnUsaPlanDeVentas Then
                oFilter.AddEx(mc_strUniqueIDBCV)
                oFilter.AddEx(mc_strControlCVenta)
                oFilter.AddEx(mc_strUniqueIDLCV)
                If m_blnUsaVehículos Then
                    oFilter.AddEx(mc_strUniqueIDInventariovehiculos)
                End If
            End If

            If m_blnUsaCosteoVehículo Then
                oFilter.AddEx(mc_strUniqueIDVSC)
                oFilter.AddEx(mc_strUIGOODISSUE)
                oFilter.AddEx(mc_strUILISTADOGR)
                oFilter.AddEx(mc_strUIRecosteos)
            End If

            If m_blnUsaOrdenesDeTrabajo Then
                oFilter.AddEx(mc_strUIFacturasInt)
                oFilter.AddEx(mc_strGeneraFI)
            End If

            '''''para documentos preliminares tipo transferencia de Stock
            oFilter.AddEx("198")
            oFilter.AddEx(mc_strDocumentoPreliminar)
            oFilter.AddEx(mc_strFormKardex)

            'Unload
            oFilter = oFilters.Add(BoEventTypes.et_FORM_UNLOAD)
            If m_blnUsaOrdenesDeTrabajo Then
                Call oFilter.AddEx(mc_strIdFormaCotizacion)
                Call oFilter.AddEx(mc_strOrdenDeCompra)
            End If

            If m_blnUsaVehículos Then
                Call oFilter.AddEx(mc_strControlVehiculo)
            End If

            If m_blnUsaPlanDeVentas Then
                Call oFilter.AddEx(mc_strControlCVenta)
                Call oFilter.AddEx(mc_strUniqueIDBCV)
            End If
            If m_blnUsaCosteoVehículo Then
                oFilter.AddEx(mc_strUniqueIDVSC)
            End If


            oFilter = oFilters.Add(BoEventTypes.et_LOST_FOCUS)
            If m_blnUsaCosteoVehículo Then
                oFilter.AddEx(mc_strUniqueIDReportesCosteo)
            End If

            'ITEM_PRESSED
            oFilter = oFilters.Add(BoEventTypes.et_ITEM_PRESSED)
            oFilter.AddEx(mc_strUniqueIDNivelesPV)
            oFilter.AddEx(mc_strUniqueIDConfiguracionesGenerales)
            oFilter.AddEx(mc_strUniqueIDCosteoMultiplesUnidades)
            oFilter.AddEx(mc_strUniqueIDSalidaMultiplesUnidades)
            If m_blnUsaOrdenesDeTrabajo Then
                oFilter.AddEx(mc_strFacturadeCompra) 'AR Invoice Form
                oFilter.AddEx(mc_strNotadeCredito)
                oFilter.AddEx(mc_strEntradadeMercancia)
                oFilter.AddEx(mc_strSalidadeMercancia)
                oFilter.AddEx(mc_strTrasladoInventario)
                oFilter.AddEx(mc_strIdFormaCotizacion)
                oFilter.AddEx(mc_stridGeneraOV)
                oFilter.AddEx(mc_strGeneraFI)
                oFilter.AddEx(mc_strUIFacturasInt)
                oFilter.AddEx(CStr(mc_strNotaDebito))
                oFilter.AddEx(g_strFormAdicionalesArtCitas)
                oFilter.AddEx(CStr(mc_strRegistroDiario))
                oFilter.AddEx(mc_strUISCGD_FormRequisicion)
                oFilter.AddEx(mc_strSolicitudOTEspecial)
                oFilter.AddEx(FormularioLLamadaServicioSBO.FormType)
                oFilter.AddEx(mc_strNotaCreditoCliente)
                oFilter.AddEx(m_oVentanaAutorizaciones)
            End If
            If DMS_Connector.Configuracion.ParamGenAddon.U_Usa_Fin.Trim().Equals("Y") Then
                oFilter.AddEx(mc_strUISCGD_FormPrestamo)
            End If

            If m_blnUsaPlacas Then

                oFilter.AddEx(mc_strUISCGD_FormPlacas)
                oFilter.AddEx(mc_strUISCGD_FormPlacaGrupos)

            End If

            If m_blnUsaAsocXEspecif Then
                oFilter.AddEx(mc_strUISCGD_FormAsocArtxEsp)
            End If

            If m_blnUsaVehículos Then
                oFilter.AddEx(m_strUIDVehiMarcaEtc)
                oFilter.AddEx(mc_strControlVehiculo)
                oFilter.AddEx(mc_strUniqueIDPropiedades)
                If m_blnUsaPlanDeVentas Then
                    oFilter.AddEx(mc_strUniqueIDInventariovehiculos)
                End If
                If m_blnUsaCosteoVehículo Then
                    oFilter.AddEx(mc_strUniqueIDReportesCosteo)
                End If
            End If
            If m_blnUsaPlanDeVentas Then
                oFilter.AddEx(mc_strControlCVenta)
                oFilter.AddEx(mc_strUniqueIDBCV)
                oFilter.AddEx(mc_strUniqueIDLCV)
                oFilter.AddEx(mc_strUniqueIDLCVR)
                oFilter.AddEx(mc_strUniqueIDLineasFactura)
                oFilter.AddEx(m_oFormularioPresupuestos.FormType)

            End If
            If m_blnUsaCosteoVehículo Then
                oFilter.AddEx(mc_strUniqueIDVSC)
                oFilter.AddEx(mc_strUIGOODENT)
                oFilter.AddEx(mc_strUIGOODISSUE)
                oFilter.AddEx(mc_strUILISTADOGR)
                oFilter.AddEx(mc_strUIRecosteos)
            End If

            oFilter.AddEx(mc_strUITrasC)

            'DOUBLE CLICK
            oFilter = oFilters.Add(BoEventTypes.et_DOUBLE_CLICK)
            If m_blnUsaVehículos Then
                oFilter.AddEx(m_strUIDVehiMarcaEtc)

            End If
            'para el formulario de Documentos Preliminares
            oFilter.AddEx(mc_strDocumentoPreliminar)
            'para docucumentos preliminares
            oFilter.AddEx("198")

            'MENU CLICK
            oFilter = oFilters.Add(BoEventTypes.et_MENU_CLICK)
            If m_blnUsaVehículos Then
                oFilter.AddEx(mc_strControlVehiculo)
            End If
            If m_blnUsaPlanDeVentas Then
                oFilter.AddEx(mc_strUniqueIDLineasFactura)
                oFilter.AddEx(mc_strUniqueIDLineasDesgloce)
            End If
            If m_blnUsaOrdenesDeTrabajo Then
                oFilter.AddEx(mc_strIdFormaCotizacion)
                oFilter.AddEx(mc_strSolicitudOTEspecial)

            End If
            If m_blnUsaCosteoVehículo Then
                oFilter.AddEx(mc_strUniqueIDTransaccionesCompras)
            End If
            '''' se agrega para simular el cierre del formulario'''''
            oFilter.AddEx("514")
            oFilter.AddEx("1291")


            'FORM_CLOSED
            oFilter = oFilters.Add(BoEventTypes.et_FORM_CLOSE)
            If m_blnUsaVehículos Then
                oFilter.AddEx(mc_strControlVehiculo)
            End If
            If m_blnUsaOrdenesDeTrabajo Then
                oFilter.AddEx(mc_stridGeneraOV)
                oFilter.AddEx(mc_strGeneraFI)
            End If
            If m_blnUsaPlanDeVentas Then
                oFilter.AddEx(mc_strControlCVenta)
                oFilter.AddEx(mc_strUniqueIDBCV)
            End If
            If m_blnUsaCosteoVehículo Then
                oFilter.AddEx(mc_strUniqueIDVSC)
            End If

            oFilter.AddEx("198")
            oFilter.AddEx("940")

            'CHOOSEFROMLIST
            oFilter = oFilters.Add(BoEventTypes.et_CHOOSE_FROM_LIST)
            oFilter.AddEx(mc_strUniqueIDConfiguracionesGenerales)
            oFilter.AddEx(mc_strUISCGD_FormRequisicion)

            oFilter.AddEx(mc_strUITrasC)
            'es de prueba para la salida contable de vehiculo
            oFilter.AddEx(mc_strUIGOODISSUE)

            If m_blnUsaVehículos Then
                oFilter.AddEx(mc_strControlVehiculo)
                oFilter.AddEx(m_strUIDVehiMarcaEtc)
                oFilter.AddEx(mc_strUIListaContXUnidad)
            End If
            If m_blnUsaOrdenesDeTrabajo Then
                oFilter.AddEx(mc_strIdFormaCotizacion)
                oFilter.AddEx(mc_stridGeneraOV)
                oFilter.AddEx(mc_strGeneraFI)

                'Llamada Servicios
                oFilter.AddEx(FormularioLLamadaServicioSBO.FormType)
            End If
            If m_blnUsaPlanDeVentas Then
                oFilter.AddEx(mc_strControlCVenta)
                oFilter.AddEx(mc_strUniqueIDLineasFactura)
                oFilter.AddEx(mc_strUniqueIDLineasDesgloce)

            End If
            If m_blnUsaCosteoVehículo Then
                oFilter.AddEx(mc_strUniqueIDTransaccionesCompras)
            End If

            oFilter.AddEx(mc_strFormKardex)

            'COMBO_SELECT
            oFilter = oFilters.Add(BoEventTypes.et_COMBO_SELECT)
            oFilter.AddEx(mc_strUniqueIDConfiguracionesGenerales)
            If m_blnUsaVehículos Then
                oFilter.AddEx(mc_strControlVehiculo)
                If m_blnUsaPlanDeVentas Then
                    oFilter.AddEx(mc_strUniqueIDInventariovehiculos)
                End If
            End If
            If m_blnUsaPlanDeVentas Then
                oFilter.AddEx(mc_strControlCVenta)
                oFilter.AddEx(mc_strUniqueIDLineasFactura)
                oFilter.AddEx(mc_strUniqueIDLineasDesgloce)
                oFilter.AddEx(m_oFormularioPresupuestos.FormType)
            End If
            If m_blnUsaOrdenesDeTrabajo Then
                oFilter.AddEx(mc_strUIFacturasInt)
            End If
            If m_blnUsaOrdenesDeTrabajo Then
                oFilter.AddEx(mc_strIdFormaCotizacion)
            End If

            'traslado de costos entre unidades
            oFilter.AddEx(mc_strUITrasC)
            'oFilter.AddEx("139")

            'Form Data load
            oFilter = oFilters.Add(BoEventTypes.et_FORM_DATA_LOAD)
            oFilter.AddEx(mc_strUISCGD_FormRequisicion)
            oFilter.AddEx("SCGD_EDV")

            'traslado de costos entre unidades
            oFilter.AddEx(mc_strUITrasC)


            If m_blnUsaOrdenesDeTrabajo Then
                oFilter.AddEx(mc_strUIFacturasInt)
                oFilter.AddEx(FormularioLLamadaServicioSBO.FormType)
                oFilter.AddEx(mc_strSolicitudOTEspecial)
            End If

            If m_blnUsaVehículos Then
                oFilter.AddEx(mc_strControlVehiculo)
                oFilter.AddEx(mc_strUniqueIDPropiedades)
            End If
            If m_blnUsaPlanDeVentas Then
                oFilter.AddEx(mc_strControlCVenta)
                oFilter.AddEx(mc_strUniqueIDLineasFactura)
                oFilter.AddEx(mc_strUniqueIDLineasDesgloce)
            End If
            If m_blnUsaCosteoVehículo Then
                oFilter.AddEx(mc_strUIGOODENT)
                oFilter.AddEx(mc_strUIGOODISSUE)
                oFilter.AddEx(mc_strUniqueIDTransaccionesCompras)
            End If


            '********************
            If m_blnUsaOrdenesDeTrabajo = True Then
                'If Utilitarios.DevuelveConversionUnidadesTiempo(SBO_Application) <> 0 Then
                '    oFilter.AddEx(CStr(mc_strMaestroArticulos))
                'End If
                If blnDevuelveConversionUT Then
                    oFilter.AddEx(CStr(mc_strMaestroArticulos))
                End If

            End If

            '*******************

            'Form Data Updated

            oFilter = oFilters.Add(BoEventTypes.et_FORM_DATA_UPDATE)
            If m_blnUsaOrdenesDeTrabajo Then
                oFilter.AddEx(mc_strOrdenDeCompra)
            End If
            If m_blnUsaOrdenesDeTrabajo = True Then
                If blnDevuelveConversionUT Then
                    oFilter.AddEx(mc_strMaestroArticulos)
                End If
            End If


            If m_blnUsaVehículos Then
                oFilter.AddEx(mc_strControlVehiculo)
                oFilter.AddEx(mc_strUniqueIDPropiedades)
            End If
            If m_blnUsaPlanDeVentas Then
                oFilter.AddEx(mc_strControlCVenta)
            End If

            ''''''para documentos Preliminares'''''''''''
            oFilter.AddEx(mc_strDocumentoPreliminar)
            oFilter.AddEx(mc_strIdFormaCotizacion)
            oFilter.AddEx(mc_strUITrasC)


            'Form Data Add
            oFilter = oFilters.Add(BoEventTypes.et_FORM_DATA_ADD)

            If m_blnUsaOrdenesDeTrabajo Then
                oFilter.AddEx(mc_strFacturaCliente)
                oFilter.AddEx(mc_strBoleta)
                oFilter.AddEx(mc_strEntregas)
                ''''probando validacion del codigo de unidad en este evento
                oFilter.AddEx(mc_strFacturadeCompra)

                'Cita
                oFilter.AddEx(FormularioLLamadaServicioSBO.FormType)
                oFilter.AddEx(mc_strNotaCreditoCliente)
                oFilter.AddEx(m_oVentanaAutorizaciones)
            End If
            If m_blnUsaOrdenesDeTrabajo AndAlso blnDevuelveConversionUT Then
                oFilter.AddEx(mc_strMaestroArticulos)
            End If

            If m_blnUsaPlanDeVentas Then
                oFilter.AddEx(mc_strControlCVenta)
            End If


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function GenerarIDFormulario(ByVal strFormUID As String) As String

        Dim blnUIDForm As Boolean = False
        Dim intIdentifier As Integer = 0
        While (Not blnUIDForm)
            If (Not (ValidarSiFormularioAbierto(strFormUID & intIdentifier, False))) Then
                strFormUID = strFormUID + CStr(intIdentifier)
                blnUIDForm = True
            Else
                intIdentifier += 1
            End If

        End While

        Return strFormUID

    End Function

    Private Function ValidarSiFormularioAbierto(ByVal strFormUID As String, _
                                                ByVal blnselectIfOpen As Boolean) As Boolean
        '*******************************************************************    
        'Nombre: DibujarFormularioMaestroVehiculos()
        'Propósito:  
        'Acepta:    
        'Retorna:   
        'Desarrollador: Yeiner Aguirre 0.
        'Fecha: 
        '*******************************************************************
        Dim intI As Integer = 0
        Dim blnFound As Boolean = False
        Dim frmForma As SAPbouiCOM.Form

        Dim a As Integer = SBO_Application.Forms.Count

        While (Not blnFound AndAlso intI < SBO_Application.Forms.Count)

            frmForma = SBO_Application.Forms.Item(intI)

            If frmForma.UniqueID = strFormUID Then
                blnFound = True
                If (blnselectIfOpen) Then
                    If Not (frmForma.Selected) Then
                        SBO_Application.Forms.Item(strFormUID).Select()
                    End If
                End If
            Else

                intI += 1
            End If

        End While

        If (blnFound) Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function ExisteCotizacionVehiculoenLista(ByVal ListaCotizacionVehiculo As  _
                                                    System.Collections.Generic.IList(Of FormaCotizacionVehiculo), _
                                                    ByVal TypeCountVehiculo As Integer, _
                                                    ByVal TypeCountCotizacion As Integer, _
                                                    ByRef oCotizacionVehiculo As FormaCotizacionVehiculo) As Boolean

        Dim intIndice As Integer

        Try

            For intIndice = 0 To ListaCotizacionVehiculo.Count - 1

                If ListaCotizacionVehiculo.Item(intIndice).intCountVehiculo = TypeCountVehiculo _
                    AndAlso ListaCotizacionVehiculo.Item(intIndice).intCountCotizacion = TypeCountCotizacion Then
                    oCotizacionVehiculo = ListaCotizacionVehiculo.Item(intIndice)
                    Return True
                Else
                    Return False
                End If
            Next
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        End Try

    End Function

    Private Function ExisteTypeCountFormaCotizacion(ByVal ListaCotizacionVehiculo As  _
                                                      System.Collections.Generic.IList(Of FormaCotizacionVehiculo), _
                                                      ByVal TypeCountVehiculo As Integer, _
                                                      ByRef TypeCountCotizacion As Integer) As Boolean

        Dim intIndice As Integer

        Try

            For intIndice = 0 To ListaCotizacionVehiculo.Count - 1

                If ListaCotizacionVehiculo.Item(intIndice).intCountVehiculo = TypeCountVehiculo Then

                    TypeCountCotizacion = ListaCotizacionVehiculo.Item(intIndice).intCountCotizacion
                    Return True

                Else

                    Return False
                End If
            Next
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Ejecuta el addon de DMS
    ''' </summary>
    ''' <param name="p_nombreExeDMS">Ruta del archivo ejecutable</param>
    Private Sub EjecutarAddonDMS(ByVal p_nombreExeDMS As String)

        Dim archivo As String = p_nombreExeDMS
        Dim oGestorAddon As New GestorAddon
        Dim argumentos As String = ""

        Dim usuario As String = SBO_Application.Company.UserName
        Dim password As String = ""
        Dim servidorSQL As String = m_oCompany.Server
        Dim dbSbo As String = m_oCompany.CompanyDB
        Dim nombreServidorLicSAP As String = ""
        Dim puertoServidorLicSAP As String = ""
        Dim nombreServidorLicSCG As String = ""
        Dim puertoServidorLicSCG As String = ""
        Dim dbUser As String = "" 'm_oCompany.DbUserName
        Dim dbPassword As String = ""
        Dim usaAutenticacionWin As Boolean = m_oCompany.UseTrusted
        Dim passwordUsuarioInterno As String = "scgadmin"
        Dim codigoSucursal As String = Utilitarios.ObtieneIdSucursal(DMS_Connector.Company.ApplicationSBO).ToString()

        argumentos = Chr(34) & usuario & Chr(34) & " " & _
                     Chr(34) & password & Chr(34) & " " & _
                     Chr(34) & servidorSQL & Chr(34) & " " & _
                     Chr(34) & dbSbo & Chr(34) & " " & _
                     Chr(34) & nombreServidorLicSAP & Chr(34) & " " & _
                     Chr(34) & puertoServidorLicSAP & Chr(34) & " " & _
                     Chr(34) & nombreServidorLicSCG & Chr(34) & " " & _
                     Chr(34) & puertoServidorLicSCG & Chr(34) & " " & _
                     Chr(34) & dbUser & Chr(34) & " " & _
                     Chr(34) & dbPassword & Chr(34) & " " & _
                     Chr(34) & usaAutenticacionWin & Chr(34) & " " & _
                     Chr(34) & passwordUsuarioInterno & Chr(34) & " " & _
                     Chr(34) & codigoSucursal & Chr(34)

        oGestorAddon.EjecutarAddon(archivo, argumentos)

    End Sub

    ''' <summary>
    ''' Abre un formulario nuevo con el número de documento especificado
    ''' </summary>
    ''' <param name="pVal">pVal con la información del evento</param>
    ''' <remarks></remarks>
    Private Sub AbrirFormulariosLinkButton(ByRef pVal As SAPbouiCOM.ItemEvent)
        Dim strConsulta As String = String.Empty
        Dim strDocEntry As String = String.Empty
        Dim strCodigoUnidad As String = String.Empty
        Dim strValores() As String
        Dim strCitaSerie As String = String.Empty
        Dim strNumSerie As String = String.Empty
        Dim strNumCita As String = String.Empty
        Dim oFormularioTemporal As SAPbouiCOM.Form

        Try
            'Solamente se abren los formularios hasta haber terminado todas las acciones en el formulario actual
            If pVal.ActionSuccess Then
                'Obtiene el formulario abierto
                oFormularioTemporal = SBO_Application.Forms.ActiveForm

                Select Case pVal.FormTypeEx
                    Case "SCGD_ORDT" 'Formulario de órdenes de trabajo
                        Select Case pVal.ItemUID
                            Case "109" 'LinkButton hacia Datos maestros del vehículo 
                                If Not m_oVehiculos Is Nothing Then
                                    strCodigoUnidad = oFormularioTemporal.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoUni", 0).Trim()
                                    'Consulta el DocEntry de la unidad
                                    strConsulta = "SELECT TOP 1 T0.""DocEntry"" FROM ""@SCGD_VEHICULO"" T0 WITH (nolock) WHERE T0.""U_Cod_Unid"" = '{0}'"
                                    strConsulta = String.Format(strConsulta, strCodigoUnidad)
                                    strDocEntry = DMS_Connector.Helpers.EjecutarConsulta(strConsulta)

                                    If Not String.IsNullOrEmpty(strDocEntry) AndAlso Not String.IsNullOrEmpty(strCodigoUnidad) Then
                                        If Not ValidarSiFormularioAbierto(mc_strControlVehiculo, True) Then
                                            'Abre el formulario vehículos con la unidad seleccionada
                                            Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", strDocEntry, True, "", 0, True, False, VehiculosCls.ModoFormulario.scgTaller)
                                        End If
                                    End If

                                End If
                            Case "130" 'LinkButton hacia la cita
                                If Not m_oFormularioCitas Is Nothing Then
                                    'Obtiene la configuración del tamaño de las celdas de la agenda
                                    If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType) Then
                                        oVersionModuloCita = DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType
                                    Else
                                        oVersionModuloCita = frmListaCitas.VersionModuloCita.Estandar
                                    End If

                                    'Obtiene el número de serie y número de la cita
                                    strCitaSerie = oFormularioTemporal.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoCita", 0).Trim()
                                    strValores = strCitaSerie.Split("-")
                                    If strValores.Length = 2 Then
                                        strNumSerie = strValores(0)
                                        strNumCita = strValores(1)
                                    End If

                                    If oVersionModuloCita = frmListaCitas.VersionModuloCita.Estandar Then
                                        'Consulta el DocEntry de la cita
                                        strConsulta = "SELECT TOP 1 T0.""DocEntry"" FROM ""@SCGD_CITA"" T0 WITH (nolock) WHERE T0.""U_Num_Serie"" = '{0}' AND T0.""U_NumCita"" = '{1}'"
                                        strConsulta = String.Format(strConsulta, strNumSerie, strNumCita)
                                        strDocEntry = DMS_Connector.Helpers.EjecutarConsulta(strConsulta)

                                        If Not String.IsNullOrEmpty(strDocEntry) AndAlso Not String.IsNullOrEmpty(strNumSerie) AndAlso Not String.IsNullOrEmpty(strNumCita) Then
                                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioCitas, True) Then
                                                'Abre el formulario de citas con la cita seleccionada
                                                m_oFormularioCitas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioCitas)
                                                m_oFormularioCitas.CargarCitaDesdePanel_Existe(strDocEntry)
                                            End If
                                        End If
                                    Else
                                        ConstructorCitas.CrearInstanciaFormularioExistente(strNumSerie, strNumCita)
                                    End If
                                End If
                        End Select
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

#End Region

#Region "SBO Events"

    Private Sub SBO_Application_AppEvent(ByVal EventType As BoAppEventTypes) Handles SBO_Application.AppEvent
        Try
            If CBool(BoAppEventTypes.aet_CompanyChanged) _
               Or CBool(BoAppEventTypes.aet_LanguageChanged) _
               Or CBool(BoAppEventTypes.aet_ShutDown) _
               Or CBool(BoAppEventTypes.aet_ServerTerminition) Then

                Dim xml As String = Replace(sXml, "add", "remove")

                SBO_Application.LoadBatchActions(xml)

                If m_oCompany.Connect() Then
                    m_oCompany.Disconnect()
                End If

                Windows.Forms.Application.Exit()

            End If

        Catch ex As IO.IOException
            Windows.Forms.Application.Exit()
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Windows.Forms.Application.Exit()
        End Try
    End Sub

    Private Sub SBO_Application_ItemEvent(ByVal FormUID As String, _
                                          ByRef pVal As ItemEvent, _
                                          ByRef BubbleEvent As Boolean) Handles SBO_Application.ItemEvent

        Try

            Dim oEdit As SAPbouiCOM.EditText
            Dim otmpForm As SAPbouiCOM.Form
            Dim oFormFacturaCliente As SAPbouiCOM.Form
            Dim oformOrdenVenta As SAPbouiCOM.Form
            Dim oItem As SAPbouiCOM.Item
            Dim sButton As SAPbouiCOM.Button
            Dim oMatriz As SAPbouiCOM.Matrix
            Dim strTypeVehiculo As String
            Dim intTypeCountVehiculo As Integer
            Dim strDocnumCompras As String
            Dim strIDContrato As String

            'Agendas Modal
            If FormularioAgendaSBO.Modal AndAlso pVal.FormTypeEx <> FormularioAgendaSBO.FormType AndAlso pVal.FormTypeEx <> "10000075" AndAlso pVal.BeforeAction AndAlso FormularioAgendaSBO.ActualizandoFormularioPadre = False Then
                SBO_Application.Forms.Item(FormularioAgendaSBO.FormType).Select()
                BubbleEvent = False
                Return
            End If

            If m_oFormularioPresupuestos IsNot Nothing AndAlso pVal.FormTypeEx = m_oFormularioPresupuestos.FormType Then m_oFormularioPresupuestos.ItemEvent(FormUID, pVal, BubbleEvent)

            If pVal.FormTypeEx = FormularioAgendaSBO.FormType And (pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE) And FormularioAgendaSBO.Modal Then
                FormularioAgendaSBO.Modal = False
            End If

            If pVal.FormTypeEx = "0" AndAlso Utilitarios.bLoadInputEvents Then
                m_oFormularioComentariosCV.ItemEvents(pVal, BubbleEvent)
            ElseIf pVal.FormTypeEx = "0" AndAlso Utilitarios.bLoadInvVehiEvents Then
                m_oFormularioComentariosIV.ItemEvents(pVal, BubbleEvent)
            End If

            ' El form de Facturas
            If pVal.FormTypeEx = mc_strFacturadeCompra _
                Or pVal.FormTypeEx = mc_strNotadeCredito _
                Or pVal.FormTypeEx = mc_strEntradadeMercancia _
                Or pVal.FormTypeEx = mc_strSalidadeMercancia _
                Or pVal.FormTypeEx = mc_strIdFormaCotizacion _
                Or pVal.FormTypeEx = mc_strMaestroVehiculos _
                Or pVal.FormTypeEx = mc_strControlVehiculo _
                Or pVal.FormTypeEx = mc_stridGeneraOV _
                Or pVal.FormTypeEx = mc_strControlCVenta _
                Or pVal.FormTypeEx = mc_strOrdenDeCompra _
                Or pVal.FormTypeEx = mc_strFacturaCliente _
                Or pVal.FormTypeEx = mc_strBoleta _
                Or pVal.FormTypeEx = mc_strEntregas _
                Or pVal.FormTypeEx = mc_strOrdenDeVenta _
                Or pVal.FormTypeEx = mc_strFacturaReserva _
                Or pVal.FormTypeEx = mc_strUniqueIDBCV _
                Or pVal.FormTypeEx = mc_strTrasladoInventario _
                Or pVal.FormTypeEx = mc_strFacturaProveedores _
                Or pVal.FormTypeEx = mc_strUniqueIDLCV _
                Or pVal.FormTypeEx = mc_strUniqueIDLCVR _
                Or pVal.FormTypeEx = mc_strUniqueIDLCVLAR _
                Or pVal.FormTypeEx = m_strUIDVehiMarcaEtc _
                Or pVal.FormTypeEx = mc_strUniqueIDVSC _
                Or pVal.FormTypeEx = mc_strUIGOODENT _
                Or pVal.FormTypeEx = mc_strUIGOODISSUE _
                Or pVal.FormTypeEx = mc_strUILISTADOGR _
                Or pVal.FormTypeEx = mc_strUIRecosteos _
                Or pVal.FormTypeEx = mc_strUniqueIDPropiedades _
                Or pVal.FormTypeEx = mc_strUniqueIDNivelesPV _
                Or pVal.FormTypeEx = mc_strUniqueIDLineasFactura _
                Or pVal.FormTypeEx = mc_strUniqueIDLineasDesgloce _
                Or pVal.FormTypeEx = mc_strUniqueIDTransaccionesCompras _
                Or pVal.FormTypeEx = mc_strUniqueIDInventariovehiculos _
                Or pVal.FormTypeEx = mc_strUniqueIDReportesCosteo _
                Or pVal.FormTypeEx = mc_strGeneraFI _
                Or pVal.FormTypeEx = mc_strMaestroArticulos _
                Or pVal.FormTypeEx = mc_strUIFacturasInt _
                Or pVal.FormTypeEx = mc_strUniqueIDConfiguracionesGenerales _
                Or pVal.FormTypeEx = FormularioAgendaSBO.FormType _
                Or pVal.FormTypeEx = FormularioLLamadaServicioSBO.FormType _
                Or pVal.FormTypeEx = mc_strDocumentoPreliminar Or pVal.FormTypeEx = "198" Or pVal.FormTypeEx = "940" _
                Or pVal.FormTypeEx = mc_strNotaDebito Or pVal.FormTypeEx = mc_strRegistroDiario _
                Or pVal.FormTypeEx = mc_strUniqueIDListaARevertir _
                Or pVal.FormTypeEx = mc_strUITrasladoCostos _
                Or pVal.FormTypeEx = mc_strUniqueIDContRevertidos _
                Or pVal.FormTypeEx = mc_strOportunidadVenta _
                Or pVal.FormTypeEx = mc_strSalidaMercancia _
                Or pVal.FormTypeEx = mc_strEntradaMercancia _
                Or pVal.FormTypeEx = mc_strUISCGD_Revertir _
                Or pVal.FormTypeEx = mc_strUISCGD_FormRequisicion _
                Or pVal.FormTypeEx = mc_strUITrasC _
                Or pVal.FormTypeEx = mc_strUIListaContXUnidad _
                Or pVal.FormTypeEx = mc_strUISCGD_FormPrestamo _
                Or pVal.FormTypeEx = mc_strUISCGD_FormConfFin _
                Or pVal.FormTypeEx = mc_strUISCGD_FormPlanPagos _
                Or pVal.FormTypeEx = mc_strUISCGD_FormEstadosCuenta _
                Or pVal.FormTypeEx = mc_strUISCGD_FormHistoricoPagos _
                Or pVal.FormTypeEx = mc_strUISCGD_FormCuotasVencidas _
                Or pVal.FormTypeEx = mc_strUISCGD_FormSaldos _
                Or pVal.FormTypeEx = mc_strUISCGD_FormPlacas _
                Or pVal.FormTypeEx = mc_strUISCGD_FormPlacaGrupos _
                Or pVal.FormTypeEx = mc_strUISCGD_FormVehiculosTipoEvento _
                Or pVal.FormTypeEx = mc_strUISCGD_FormContratoTraspaso _
                Or pVal.FormTypeEx = mc_strUISCGD_FormComision _
                Or pVal.FormTypeEx = mc_strUISCGD_FormVehiculosProblemas _
                Or pVal.FormTypeEx = mc_FormBalance _
                Or pVal.FormTypeEx = mc_strFORM_EstadosOT _
                Or pVal.FormTypeEx = mc_strUISCGD_FormGastos _
                Or pVal.FormTypeEx = mc_strUIDCitasXTipo _
                Or pVal.FormTypeEx = mc_strUID_FORM_CitasXTipo _
                Or pVal.FormTypeEx = mc_strFormReportesCV _
                Or pVal.FormTypeEx = mc_strOfertaDeCompra _
                Or pVal.FormTypeEx = mc_strPagoRecibido _
                Or pVal.FormTypeEx = mc_strRefacturacion _
                Or pVal.FormTypeEx = mc_strUniqueIDCosteoMultiplesUnidades _
                Or pVal.FormTypeEx = mc_strUniqueIDSalidaMultiplesUnidades _
                Or pVal.FormTypeEx = mc_strFormUnidadesPorNivel _
                Or pVal.FormTypeEx = mc_strUISCGD_RptUnidadesVend _
                Or pVal.FormTypeEx = mc_strFormCero _
                Or pVal.FormTypeEx = mc_strFormVendedoresTipoInv _
                Or pVal.FormTypeEx = mc_strUIDFormBusquedas _
                Or pVal.FormTypeEx = mc_strUISCGD_FormAsocArtxEsp _
                Or pVal.FormTypeEx = mc_strUIDFormBalanceOT _
                Or pVal.FormTypeEx = mc_strUIDFormIncluirRepOT _
                Or pVal.FormTypeEx = mc_strUIDFormSeleccionRepOT _
                Or pVal.FormTypeEx = mc_strUSCG_MantEspecifiacionXModelo _
                Or pVal.FormTypeEx = mc_strUISCGD_FormParamAplicacion _
                Or pVal.FormTypeEx = mc_strUISCGD_FormAgendasConfiguracion _
                Or pVal.FormTypeEx = mc_strUISCGD_Citas _
                Or pVal.FormTypeEx = mc_strFormNumeracionSeries _
                Or pVal.FormTypeEx = mc_strUISCGD_BusqCitas _
                Or pVal.FormTypeEx = mc_strUISCGD_CargPanelCitas _
                Or pVal.FormTypeEx = mc_strUISCGD_SuspenderAgenda _
                Or pVal.FormTypeEx = mc_strIdFormCampaña _
                Or pVal.FormTypeEx = strMenuConfigNivAprob _
                Or pVal.FormTypeEx = strMenuEmbarqueVehiculos _
                Or pVal.FormTypeEx = mc_strFormVehiculosArticulosVenta _
                Or pVal.FormTypeEx = mc_strFormVehiculoSeleciconColor _
                Or pVal.FormTypeEx = "SCGD_CVS" _
                Or pVal.FormTypeEx = "SCGD_COT" _
                Or pVal.FormTypeEx = mc_strUID_FORM_BodegasP _
                Or pVal.FormTypeEx = mc_strSolicitudOTEspecial _
                Or pVal.FormTypeEx = mc_strUISCGD_ReporteOrdenes _
                Or pVal.FormTypeEx = mc_strUIDFormIncluirGastosOT _
                Or pVal.FormTypeEx = mc_strUID_FORM_FacturacioVehi _
                Or pVal.FormTypeEx = mc_strUIDFormSeleccionGasOT _
                Or pVal.FormTypeEx = mc_strNotaCreditoCliente _
                Or pVal.FormTypeEx = m_oVentanaAutorizaciones _
                Or pVal.FormTypeEx = mc_strUISCGD_FormConfFin _
                Or pVal.FormTypeEx = mc_strUIDFormCrearDocGastosCostos _
                Or pVal.FormTypeEx = mc_strDimensionesContables _
                Or pVal.FormTypeEx = mc_strDimensionesContablesOTs _
                Or pVal.FormTypeEx = mc_strCosteoDeEntradas _
                Or pVal.FormTypeEx = mc_StrPedidoVehiculos _
                Or pVal.FormTypeEx = mc_strEntradaDeVehiculos _
                Or pVal.FormTypeEx = g_strFormTipoOTInterna _
                Or pVal.FormTypeEx = g_strFormSolOTEspecial _
                Or pVal.FormTypeEx = g_strFormOT _
                Or pVal.FormTypeEx = g_strFormAsigMultOT _
                Or pVal.FormTypeEx = g_strFormSolOTEspecial _
                Or pVal.FormTypeEx = mc_strUID_FORM_OrdenesTrabajoEstado _
                Or pVal.FormTypeEx = mc_strUID_FORM_ReporteHistorialVehiculo _
                Or pVal.FormTypeEx = mc_strUID_FORM_ReporteFacturacionOT _
                Or pVal.FormTypeEx = mc_strUID_FORM_ReporteFacturacionInterna _
                Or pVal.FormTypeEx = mc_strUID_FORM_ReporteAntiguedadVehiculos _
                Or pVal.FormTypeEx = g_strFormAsignacionMultiple _
                Or pVal.FormTypeEx = mc_strUID_FORM_ReporteServiciosExternosXOrden _
                Or pVal.FormTypeEx = mc_strUID_FORM_ReporteFacMecanicos _
                Or pVal.FormTypeEx = g_strFormAdicionalesOT _
                Or pVal.FormTypeEx = g_strFormDocumentoCompra _
                Or pVal.FormTypeEx = g_strFormBusquedaProveedores _
                Or pVal.FormTypeEx = mc_strFormListaPreciosSelecicon _
                Or pVal.FormTypeEx = mc_strFormListaEmpleados _
                Or pVal.FormTypeEx = g_strFormOTEspecial _
                Or pVal.FormTypeEx = g_strFormRazonSuspension _
                Or pVal.FormTypeEx = mc_strFormComentarioHCV _
                Or pVal.FormTypeEx = "-9876" _
                Or pVal.FormTypeEx = mc_strDevolucionDeVehiculos _
                Or pVal.FormTypeEx = mc_strSeleccionarUnidadDev _
                Or pVal.FormTypeEx = g_strFormAdicionalesArtCitas _
                Or pVal.FormTypeEx = mc_strFormKardex _
                Or pVal.FormTypeEx = mc_strSeleccionLineasPedidos _
                Or pVal.FormTypeEx = mc_strSeleccionLinasRecepcion _
                Or pVal.FormTypeEx = mc_strSociosNegocios _
                Or pVal.FormTypeEx = mc_strMaestroEmpleados _
                Or pVal.FormTypeEx = g_strConfMsj _
                Or pVal.FormTypeEx = mc_strFormSeleccionMarcaEstilo _
                Or pVal.FormTypeEx = g_strFormTrack _
                Or pVal.FormTypeEx = mc_strFormSelUbi _
                Or pVal.FormTypeEx = mc_strFormLstReq _
                Or pVal.FormTypeEx = g_strFormFinAct _
                Or pVal.FormTypeEx = mc_strUniqueIDConSegPV _
                Or pVal.FormTypeEx = mc_strFormLstSolEsp _
                Or pVal.FormTypeEx = mc_strFormSolEsp _
                Or pVal.FormTypeEx = mc_strUISCGD_FormAVA _
                Or pVal.FormTypeEx = mc_strFormMediosPago _
                Or pVal.FormTypeEx = mc_strUID_FORM_ReporteSociosNegocios _
                Or pVal.FormTypeEx = mc_strUID_FORM_ReporteVehiculosRecurrentesTaller _
                Or pVal.FormTypeEx = mc_strUID_FORM_ReporteVentasXAsesorServicio _
                Or pVal.FormTypeEx = "SCGD_ODE" _
                Or pVal.FormTypeEx = "SCGD_SRCP" _
                Or pVal.FormTypeEx = "SCGD_CCIT" _
                Or pVal.FormTypeEx = "SCGD_RABR" _
                Or pVal.FormTypeEx = "SCGD_ISSC" _
                Or pVal.FormTypeEx = "SCGD_TIMEL" _
                Or pVal.FormTypeEx = "SCGD_OLAD" _
                Or pVal.FormTypeEx = "SCGD_REAOT" _
                Or pVal.FormTypeEx = "SCGD_RFC" _
                Or pVal.FormTypeEx = "SCGD_RFC" _
                OrElse pVal.FormTypeEx = "1250000000" Then

                'Or pVal.FormTypeEx = mc_strCargaMasivaVehiculos _



                If Not m_oFormularioRequisiciones Is Nothing AndAlso m_blnUsaOrdenesDeTrabajo Then
                    m_oFormularioRequisiciones.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent, m_oFormSeleccionUbicaciones)

                    'validacion para verificar la bodega de destino por cada articulo seleccionado
                    If pVal.ItemUID = "btnTrasl" _
                        And pVal.Before_Action Then

                        Dim strBodegaDestino As String = ""
                        Dim MatrizReq As SAPbouiCOM.Matrix

                        Dim xmlDocMatrix As Xml.XmlDocument
                        Dim matrixXml As String

                        Dim ListaCodigoUnidad As Generic.IList(Of String) = New Generic.List(Of String)
                        Dim oForm As SAPbouiCOM.Form

                        oForm = SBO_Application.Forms.Item(FormUID)

                        MatrizReq = (DirectCast(oForm.Items.Item("mtxReq").Specific, SAPbouiCOM.Matrix))

                        matrixXml = MatrizReq.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

                        xmlDocMatrix = New Xml.XmlDocument
                        xmlDocMatrix.LoadXml(matrixXml)

                        Try
                            For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")
                                Dim elementoSel As Xml.XmlNode
                                Dim elementoUnidad As Xml.XmlNode
                                Dim elementoBodegaDestino As Xml.XmlNode

                                elementoSel = node.SelectSingleNode("Columns/Column/Value[../ID = 'colChk']")
                                elementoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'colCodArt']")
                                elementoBodegaDestino = node.SelectSingleNode("Columns/Column/Value[../ID = 'colCdBDest']")

                                If Not elementoUnidad.InnerText = String.Empty _
                                        And Not elementoBodegaDestino.InnerText = String.Empty _
                                        And elementoSel.InnerText = "1" Then

                                    strBodegaDestino = elementoBodegaDestino.InnerText.Trim()

                                    If String.IsNullOrEmpty(strBodegaDestino) Then
                                        SBO_Application.StatusBar.SetText(My.Resources.Resource.El_Item + elementoUnidad.InnerText + My.Resources.Resource.ErrorBodegaDestino,
                                                                          BoMessageTime.bmt_Medium,
                                                                          BoStatusBarMessageType.smt_Error)
                                        BubbleEvent = False
                                    End If
                                End If
                            Next
                        Catch ex As Exception
                            Utilitarios.ManejadorErrores(ex, SBO_Application)
                        End Try
                    End If 'boton de traslado 

                End If

                If Not m_oFormularioPrestamo Is Nothing AndAlso m_blnFinanciamiento Then

                    m_oFormularioPrestamo.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent, System.Windows.Forms.Application.StartupPath)

                    If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

                        Dim oForm As SAPbouiCOM.Form
                        Dim strUnidad As String
                        Dim strIDVeh As String
                        Dim strNumeroCV As String

                        oForm = SBO_Application.Forms.Item(FormUID)

                        If pVal.BeforeAction Then
                            If pVal.ItemUID = "lkContrato" AndAlso pVal.BeforeAction = True AndAlso pVal.FormTypeEx = m_oFormularioPrestamo.FormType Then

                                m_oCVenta.m_blnCargoManejarEstados = True

                            End If
                        ElseIf pVal.ActionSuccess Then

                            If pVal.ItemUID = "lkUnidad" AndAlso pVal.FormTypeEx = m_oFormularioPrestamo.FormType Then

                                m_oVehiculos = New VehiculosCls(m_oCompany, SBO_Application)

                                strUnidad = oForm.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Cod_Unid", 0)
                                strUnidad = strUnidad.Trim()
                                strIDVeh = Utilitarios.EjecutarConsulta("SELECT DocEntry FROM [@SCGD_VEHICULO] WHERE U_Cod_Unid = '" & strUnidad & "'", SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                                Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                             strIDVeh, _
                                                             True, _
                                                             "", _
                                                             0, True, False, VehiculosCls.ModoFormulario.scgVentas)

                            End If

                            If pVal.ItemUID = "lkContrato" AndAlso pVal.ActionSuccess = True AndAlso pVal.BeforeAction = False AndAlso pVal.FormTypeEx = m_oFormularioPrestamo.FormType Then

                                strNumeroCV = oForm.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Cont_Ven", 0)
                                strNumeroCV = strNumeroCV.Trim()

                                If Not ValidarSiFormularioAbierto(ContratoVentasCls.FormType, False) Then

                                    Call m_oCVenta.DibujarFormularioContratoVentas("", False)
                                    Call m_oCVenta.CargarContrato(strNumeroCV, ContratoVentasCls.FormType)
                                    Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item(ContratoVentasCls.FormType), False)

                                Else

                                    SBO_Application.Forms.Item(ContratoVentasCls.FormType).Select()

                                End If

                                m_oCVenta.m_blnCargoManejarEstados = False

                            End If
                        End If

                    End If

                End If
                If Not m_oFormularioConfFinanc Is Nothing AndAlso m_blnFinanciamiento Then
                    m_oFormularioConfFinanc.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If Not m_oFormularioPlanPagos Is Nothing AndAlso m_blnFinanciamiento Then
                    m_oFormularioPlanPagos.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If Not m_oFormularioEstadoCuentas Is Nothing AndAlso m_blnFinanciamiento Then
                    m_oFormularioEstadoCuentas.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If Not m_oFormularioHistoricoPagos Is Nothing AndAlso m_blnFinanciamiento Then
                    m_oFormularioHistoricoPagos.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If Not m_oFormularioCuotasVencidas Is Nothing AndAlso m_blnFinanciamiento Then
                    m_oFormularioCuotasVencidas.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If Not m_oFormularioSaldos Is Nothing AndAlso m_blnFinanciamiento Then
                    m_oFormularioSaldos.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If Not m_oFormularioSolicitudEspecificos Is Nothing AndAlso pVal.FormUID = mc_strFormSolEsp Then
                    m_oFormularioSolicitudEspecificos.ApplicationSBOOnItemEvent(pVal, BubbleEvent)
                End If

                If pVal.FormUID = mc_strUISCGD_FormAVA AndAlso Not m_oFormularioAvaUs Is Nothing Then
                    If (pVal.EventType = BoEventTypes.et_ITEM_PRESSED AndAlso pVal.ActionSuccess AndAlso pVal.ItemUID = "lnkOT") Then
                        If (m_oFormularioOrdenTrabajo IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioOrdenTrabajo, activarSiEstaAbierto:=True) Then
                                m_oFormularioOrdenTrabajo.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioOrdenTrabajo)
                            End If
                        End If
                        m_oFormularioAvaUs.LinkOT(pVal, BubbleEvent, m_oFormularioOrdenTrabajo)
                    Else
                        m_oFormularioAvaUs.ApplicationSBOOnItemEvent(pVal, BubbleEvent, m_oVehiculos)
                    End If
                End If

                If Not m_oFormularioPlacas Is Nothing AndAlso m_blnUsaPlacas Then

                    m_oFormularioPlacas.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)

                    If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

                        Dim oForm As SAPbouiCOM.Form
                        Dim strUnidad As String
                        Dim strIDVeh As String
                        Dim strNumeroCV As String

                        oForm = SBO_Application.Forms.Item(FormUID)

                        If pVal.ItemUID = "lkUnidad" AndAlso pVal.ActionSuccess = True AndAlso pVal.BeforeAction = False AndAlso pVal.FormTypeEx = m_oFormularioPlacas.FormType Then

                            If Not ValidarSiFormularioAbierto(mc_strUniqueID, False) Then

                                m_oVehiculos = New VehiculosCls(m_oCompany, SBO_Application)

                                strUnidad = oForm.DataSources.DBDataSources.Item("@SCGD_PLACA").GetValue("U_Num_Unid", 0)
                                strUnidad = strUnidad.Trim()
                                strIDVeh = Utilitarios.EjecutarConsulta("SELECT DocEntry FROM [@SCGD_VEHICULO] WHERE U_Cod_Unid = '" & strUnidad & "'", SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                                Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                             strIDVeh, _
                                                             True, _
                                                             "", _
                                                             0, True, False, VehiculosCls.ModoFormulario.scgVentas)
                            End If

                        End If

                        If pVal.ItemUID = "lkNoCV" AndAlso pVal.ActionSuccess = True AndAlso pVal.BeforeAction = False AndAlso pVal.FormTypeEx = m_oFormularioPlacas.FormType Then

                            strNumeroCV = oForm.DataSources.DBDataSources.Item("@SCGD_PLACA").GetValue("U_Num_CV", 0)
                            strNumeroCV = strNumeroCV.Trim()

                            If Not ValidarSiFormularioAbierto(ContratoVentasCls.FormType, False) Then

                                Call m_oCVenta.DibujarFormularioContratoVentas("", False)
                                Call m_oCVenta.CargarContrato(strNumeroCV, ContratoVentasCls.FormType)
                                Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item(ContratoVentasCls.FormType), False)

                            Else

                                SBO_Application.Forms.Item(ContratoVentasCls.FormType).Select()

                            End If

                            m_oCVenta.m_blnCargoManejarEstados = False

                        ElseIf pVal.ItemUID = "lkNoCV" AndAlso pVal.BeforeAction = True AndAlso pVal.FormTypeEx = m_oFormularioPlacas.FormType Then

                            m_oCVenta.m_blnCargoManejarEstados = True

                        End If

                    End If

                End If

                If Not m_oFormularioPlacaGrupos Is Nothing AndAlso m_blnUsaPlacas Then

                    m_oFormularioPlacaGrupos.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)

                    If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

                        Dim oForm As SAPbouiCOM.Form
                        Dim strUnidad As String
                        Dim strIDVeh As String


                        oForm = SBO_Application.Forms.Item(FormUID)

                        If pVal.ItemUID = "lkUnidadG" AndAlso pVal.ActionSuccess = True AndAlso pVal.BeforeAction = False Then

                            If Not ValidarSiFormularioAbierto(mc_strUniqueID, False) Then

                                m_oVehiculos = New VehiculosCls(m_oCompany, SBO_Application)

                                strUnidad = m_oFormularioPlacaGrupos.EditTextUnidad.ObtieneValorUserDataSource()
                                strUnidad = strUnidad.Trim()
                                strIDVeh = Utilitarios.EjecutarConsulta("SELECT DocEntry FROM [@SCGD_VEHICULO] WHERE U_Cod_Unid = '" & strUnidad & "'", SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                                Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                             strIDVeh, _
                                                             True, _
                                                             "", _
                                                             0, True, False, VehiculosCls.ModoFormulario.scgVentas)
                            End If
                        End If
                    End If
                End If

                If Not m_oFormularioVehiculoTipoEvento Is Nothing AndAlso m_blnUsaPlacas Then
                    m_oFormularioVehiculoTipoEvento.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oFormularioContratoTraspaso Is Nothing AndAlso m_blnUsaPlacas Then
                    m_oFormularioContratoTraspaso.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oFormularioComision Is Nothing AndAlso m_blnUsaPlacas Then
                    m_oFormularioComision.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oFormularioVehiculosProblemas Is Nothing AndAlso m_blnUsaPlacas Then
                    m_oFormularioVehiculosProblemas.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oFormularioGastosCV Is Nothing Then
                    m_oFormularioGastosCV.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oFormularioUnidadesVendidas Is Nothing Then
                    m_oFormularioUnidadesVendidas.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oFormularioAsocArticuloxEspecif Is Nothing AndAlso m_blnUsaAsocXEspecif Then
                    m_oFormularioAsocArticuloxEspecif.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oFormMantenEspecificacionPorModelo Is Nothing Then
                    m_oFormMantenEspecificacionPorModelo.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oFormularioCitaXTipoAgenda Is Nothing Then
                    m_oFormularioCitaXTipoAgenda.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oFormularioBodegaProceso Is Nothing Then
                    m_oFormularioBodegaProceso.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oFormularioSociosNegocios Is Nothing Then
                    m_oFormularioSociosNegocios.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oFormularioFacturacionvehiculo Is Nothing Then
                    m_oFormularioFacturacionvehiculo.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oFormularioOrdenesDeTrabajoPorEstado Is Nothing Then
                    m_oFormularioOrdenesDeTrabajoPorEstado.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If Not m_oFormularioHistorialVehiculo Is Nothing Then
                    m_oFormularioHistorialVehiculo.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If Not m_oFormularioReporteFacturacionOT Is Nothing Then
                    m_oFormularioReporteFacturacionOT.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If Not m_oFormularioReporteFacturacionMecanicos Is Nothing Then
                    m_oFormularioReporteFacturacionMecanicos.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If Not m_oFormularioFactutacionOTInternas Is Nothing Then
                    m_oFormularioFactutacionOTInternas.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If Not m_oFormularioReporteAntiguedadVehiculos Is Nothing Then
                    m_oFormularioReporteAntiguedadVehiculos.ApplicationSBOItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If Not m_oFormularioReporteServiciosExternosXOrden Is Nothing Then
                    m_oFormularioReporteServiciosExternosXOrden.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If Not m_oFormularioReporteFinanciamientoContratoVentas Is Nothing Then
                    m_oFormularioReporteFinanciamientoContratoVentas.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                'Manejo de eventos para el formulario de Numeración de Series
                If pVal.FormTypeEx = mc_strFormNumeracionSeries Then
                    Call m_oFormularioSeriesNumeracion.ManejadorEventoItemPress(pVal, FormUID, BubbleEvent)
                End If

                'Formulario de Busqueda de Articulos de Venta [Maestro vehiculos]
                If pVal.FormTypeEx = mc_strFormVehiculosArticulosVenta Then
                    Call m_oFormularioVehiculoArticuloVenta.ManejadorEventoItemPress(pVal, FormUID, BubbleEvent)
                End If
                'Formulario de Busqueda de colores [Maestro Vehiculo]
                If pVal.FormTypeEx = mc_strFormVehiculoSeleciconColor Then
                    Call m_oFormularioVehiculoColorSeleccion.ManejadorEventoItemPress(pVal, FormUID, BubbleEvent)
                End If
                'Formulario lista ubicaciones
                If pVal.FormTypeEx = mc_strFormSelUbi Then
                    Call m_oFormSeleccionUbicaciones.ManejadorEventoItemPress(pVal, FormUID, BubbleEvent)
                End If
                'Formulario Seleccion Marca/Estilo/Modelo
                If Not m_oFormularioSeleciconMarcaEstiloModelo Is Nothing AndAlso
                    pVal.FormTypeEx = mc_strFormSeleccionMarcaEstilo Then

                    If pVal.EventType = BoEventTypes.et_ITEM_PRESSED OrElse
                         pVal.EventType = BoEventTypes.et_COMBO_SELECT Then
                        Call m_oFormularioSeleciconMarcaEstiloModelo.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                    End If
                End If

                If pVal.FormTypeEx = mc_strSeleccionarUnidadDev Then
                    If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then
                        Call m_oFormularioSeleccionaUnidadDev.ManejadorEventoItemPress(pVal, FormUID, BubbleEvent)
                    End If
                End If

                If pVal.FormTypeEx = g_strFormOT Then
                    Call m_oFormularioOrdenTrabajo.ManejadorEventoItemPress(pVal, FormUID, BubbleEvent)
                End If

                If Not m_oFormularioParametrosAplicacion Is Nothing Then
                    m_oFormularioParametrosAplicacion.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oFormularioAgendasConfiguracion Is Nothing Then
                    m_oFormularioAgendasConfiguracion.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                'Citas
                If Not m_oFormularioCitas Is Nothing Then
                    m_oFormularioCitas.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent, m_oVehiculos, m_oFormularioAdicionalesCitasArt)
                End If

                If Not m_oFormularioBusquedasCitas Is Nothing Then
                    m_oFormularioBusquedasCitas.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)

                    If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

                        If pVal.ItemUID = "btnCitas" AndAlso pVal.ActionSuccess = True AndAlso pVal.BeforeAction = False AndAlso pVal.FormTypeEx = m_oFormularioBusquedasCitas.FormType Then
                            If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType) Then
                                oVersionModuloCita = DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType
                            Else
                                oVersionModuloCita = frmListaCitas.VersionModuloCita.Estandar
                            End If

                            If oVersionModuloCita = frmListaCitas.VersionModuloCita.Estandar Then
                                If Not oGestorFormularios.FormularioAbierto(m_oFormularioCitas, activarSiEstaAbierto:=True) Then
                                    m_oFormularioCitas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioCitas)
                                End If
                            Else
                                ConstructorCitas.CrearInstanciaFormulario()
                            End If
                        End If

                    End If

                End If

                'Citas
                If Not m_oFormularioCargarPanelCitas Is Nothing Then
                    m_oFormularioCargarPanelCitas.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent, m_oFormularioCitas)
                End If

                If Not m_oFormularioSuspensionAgenda Is Nothing Then
                    m_oFormularioSuspensionAgenda.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oReporteOrdenesEspeciales Is Nothing Then
                    m_oReporteOrdenesEspeciales.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not m_oRefacturacion Is Nothing AndAlso pVal.FormTypeEx = m_oRefacturacion.FormType Then

                    m_oRefacturacion.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)

                    If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

                        Dim oForm As SAPbouiCOM.Form
                        Dim strNumeroCV As String

                        oForm = SBO_Application.Forms.Item(FormUID)

                        If pVal.ItemUID = "lkCont" AndAlso pVal.ActionSuccess = True AndAlso pVal.BeforeAction = False AndAlso pVal.FormTypeEx = m_oRefacturacion.FormType Then

                            strNumeroCV = m_oRefacturacion.EditTextContrato.ObtieneValorUserDataSource()

                            If Not ValidarSiFormularioAbierto(ContratoVentasCls.FormType, False) Then

                                Call m_oCVenta.DibujarFormularioContratoVentas("", False)
                                Call m_oCVenta.CargarContrato(strNumeroCV, ContratoVentasCls.FormType)
                                Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item(ContratoVentasCls.FormType), False)

                            Else

                                SBO_Application.Forms.Item(ContratoVentasCls.FormType).Select()

                            End If

                            m_oCVenta.m_blnCargoManejarEstados = False

                        ElseIf pVal.ItemUID = "lkCont" AndAlso pVal.BeforeAction = True AndAlso pVal.FormTypeEx = m_oRefacturacion.FormType Then

                            m_oCVenta.m_blnCargoManejarEstados = True

                        End If

                    End If

                End If

                'manejo de eventos para busquedas de OT 
                If Not m_oFormularioBusquedaOT Is Nothing Then
                    m_oFormularioBusquedaOT.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                'Manejo de eventos para Inclusion de Repuestos en la OT
                If Not m_oFormularioIncluirRepOT Is Nothing And pVal.FormUID = "SCGD_AROT" Then
                    If pVal.ItemUID = "btnAdd" Then
                        If m_oFormularioIncluirRepOT.FormularioSBO.Items.Item("btnAdd").Enabled Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioSeleccionaRepuestosOT, activarSiEstaAbierto:=True) Then
                                m_oFormularioSeleccionaRepuestosOT.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioSeleccionaRepuestosOT)
                            End If
                        End If
                    Else
                        m_oFormularioIncluirRepOT.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                    End If


                End If

                'Manejo de eventos para selección de repuestos a la ot
                If pVal.FormTypeEx = mc_strUIDFormSeleccionRepOT And Not m_oFormularioSeleccionaRepuestosOT Is Nothing Then
                    m_oFormularioSeleccionaRepuestosOT.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                'manejo de eventos para balance de OT 
                If Not m_oFormularioBalanceOT Is Nothing Then
                    m_oFormularioBalanceOT.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If Not g_oFormularioControlVisita Is Nothing Then
                    g_oFormularioControlVisita.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If Not g_oFormularioOfertaVentas Is Nothing Then
                    g_oFormularioOfertaVentas.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                '*********************** PROTOTIPO
                '12-03-2014  Compras e Importacion

                If pVal.FormTypeEx = mc_StrPedidoVehiculos And Not m_oFormularioPedidoVehiculos Is Nothing Then
                    m_oFormularioPedidoVehiculos.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If pVal.FormTypeEx = mc_strEntradaDeVehiculos And Not m_oFormularioEntradaDeVehiculos Is Nothing Then
                    m_oFormularioEntradaDeVehiculos.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If pVal.FormTypeEx = mc_strCosteoDeEntradas And Not m_oFormularioCosteoDeEntradas Is Nothing Then
                    m_oFormularioCosteoDeEntradas.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If pVal.FormTypeEx = mc_strDevolucionDeVehiculos And Not m_oFormularioDevolucionDeVehiculos Is Nothing Then
                    m_oFormularioDevolucionDeVehiculos.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If pVal.FormTypeEx = mc_strSeleccionLinasRecepcion And Not m_oFormularioSeleccionLineasRecepcion Is Nothing Then
                    m_oFormularioSeleccionLineasRecepcion.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If
                If pVal.FormTypeEx = mc_strSeleccionLineasPedidos And Not m_oFormularioSeleccionLineasPedidos Is Nothing Then
                    m_oFormularioSeleccionLineasPedidos.ApplicationSboOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                'Manejo de eventos ventana inclusion Gastos/Costos a la OT
                If Not m_oFormularioIncluirGastoOT Is Nothing Then
                    m_oFormularioIncluirGastoOT.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If pVal.FormTypeEx = mc_strUIDFormSeleccionGasOT And Not m_oFormularioSeleccionaGastosOT Is Nothing Then
                    Call m_oFormularioSeleccionaGastosOT.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If pVal.FormTypeEx = mc_strUIDFormCrearDocGastosCostos And Not m_oFormularioCrearDocumentosGastos Is Nothing Then
                    Call m_oFormularioCrearDocumentosGastos.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If pVal.FormTypeEx = g_strFormAsigMultOT And Not m_oFormularioAsignacionMultipleOT Is Nothing Then
                    m_oFormularioAsignacionMultipleOT.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If pVal.FormTypeEx = g_strFormRazonSuspension And Not m_oFormularioRazonSuspension Is Nothing Then
                    m_oFormularioRazonSuspension.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If pVal.FormTypeEx = g_strFormAdicionalesOT And Not m_oFormularioAdicionalesOT Is Nothing Then
                    m_oFormularioAdicionalesOT.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If pVal.FormTypeEx = g_strFormTrack And Not m_oFormularioTrackRep Is Nothing Then
                    m_oFormularioTrackRep.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If pVal.FormTypeEx = g_strFormFinAct And Not m_oFormularioFinAct Is Nothing Then
                    m_oFormularioFinAct.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If pVal.FormTypeEx = g_strFormAdicionalesArtCitas And Not m_oFormularioAdicionalesCitasArt Is Nothing Then
                    m_oFormularioAdicionalesCitasArt.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent, m_oFormularioCitas)
                End If

                If pVal.FormTypeEx = g_strFormDocumentoCompra And Not m_oFormularioDocumentoCompra Is Nothing Then
                    m_oFormularioDocumentoCompra.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent, m_oFormularioOrdenTrabajo)
                End If

                If pVal.FormTypeEx = g_strFormBusquedaProveedores And Not m_oFormularioBuscarProveedores Is Nothing Then
                    m_oFormularioBuscarProveedores.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                If pVal.FormTypeEx = g_strFormOTEspecial And Not m_oFormularioOTEspecial Is Nothing Then
                    m_oFormularioOTEspecial.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                End If

                Select Case pVal.EventType
                    Case SAPbouiCOM.BoEventTypes.et_KEY_DOWN
                        If pVal.FormTypeEx = FormularioAgendaSBO.FormType AndAlso pVal.ActionSuccess AndAlso m_blnUsaOrdenesDeTrabajo Then
                            m_oAgendas.ManejadorEventoKeyDown(pVal.FormUID, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strIdFormaCotizacion Then
                            Call m_oRecepcionVHUI.ManejadorEventoKeyDown(FormUID, pVal, BubbleEvent)
                        End If

                    Case SAPbouiCOM.BoEventTypes.et_CLICK

                        Select Case pVal.FormTypeEx
                            Case "-9876"
                                If m_blnLineaOT Then
                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.CambiarLineasCotizacion, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                    BubbleEvent = False
                                    Exit Sub
                                End If

                            Case mc_strControlVehiculo

                                If pVal.BeforeAction Then

                                    otmpForm = SBO_Application.Forms.Item(FormUID)

                                    m_oVehiculos.ManejoEventosCombo(otmpForm, pVal, FormUID, BubbleEvent)
                                End If

                            Case mc_strOrdenDeCompra

                                If pVal.ActionSuccess Then
                                    m_oComprasEnVentas.ManejadorEventosItemPressed(pVal.FormUID, pVal, BubbleEvent)
                                End If

                            Case mc_strIdFormaCotizacion
                                If pVal.BeforeAction Then
                                    Call m_oCotizacion.ManejadorEventoClickedPress(FormUID, pVal, BubbleEvent)
                                    If pVal.ItemUID = "SCGD_LKOT" Then
                                        If Not AdministradorLicencias.LicenciaUsuarioValida(DMS_Connector.Company.CompanySBO.UserSignature, pVal.ItemUID) Then
                                            BubbleEvent = False
                                            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorLicencia, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                                        End If
                                    End If
                                End If
                            Case mc_strFacturaCliente, mc_strBoleta

                            Case g_strFormOT
                                AbrirFormulariosLinkButton(pVal)
                        End Select

                        If pVal.FormTypeEx = FormularioAgendaSBO.FormType AndAlso m_blnUsaOrdenesDeTrabajo Then
                            m_oAgendas.ManejadorEventoItemClicked(pVal.FormUID, pVal, BubbleEvent)
                        End If

                        'Verifica si la linea de la cotizacion esta asociada una orden de trabajo
                        If pVal.FormTypeEx = mc_strIdFormaCotizacion And pVal.ActionSuccess AndAlso m_blnUsaOrdenesDeTrabajo Then

                            Dim form As Form = SBO_Application.Forms.Item(pVal.FormUID)

                            If m_oCotizacion.FilaTieneNumeroOT(form, pVal.Row) Then
                                blnFilaTieneOT = True
                            Else
                                blnFilaTieneOT = False
                            End If

                        End If

                        'Verifica si la linea de la Orden de Venta esta asociada una orden de trabajo
                        If pVal.FormTypeEx = mc_strOrdenDeVenta And pVal.ActionSuccess AndAlso m_blnUsaOrdenesDeTrabajo Then

                            Dim form As Form = SBO_Application.Forms.Item(pVal.FormUID)

                            If m_oOrdenVenta.FilaTieneNumeroOT(form, pVal.Row, pVal.ItemUID, m_NumOT_OV) Then
                                blnOVFilaTieneOT = True
                            Else
                                blnOVFilaTieneOT = False
                            End If

                        End If

                        'Agregado 03/02/2011: Manejo de evento click sobre la matriz de vehiculos usados
                        If pVal.FormTypeEx = mc_strControlCVenta Then

                            Call m_oCVenta.ManejadorEventoClick(pVal, FormUID, BubbleEvent)

                        End If

                        'Agregado 31/05/2012: Manejo de impresion de reportes cv
                        If pVal.FormTypeEx = mc_strFormReportesCV Then
                            Call m_oReporteCV.ManejadorEventoClick(pVal, FormUID, BubbleEvent, m_oCompany, DBUser, DBPassword)
                        End If

                    Case SAPbouiCOM.BoEventTypes.et_VALIDATE

                        If pVal.FormTypeEx = mc_strControlCVenta Then
                            otmpForm = SBO_Application.Forms.Item(FormUID)
                            m_oCVenta.ManejoEventoValidate(otmpForm, pVal)
                        End If

                        'If pVal.FormTypeEx = mc_strCargaMasivaVehiculos Then
                        '    m_oFormularioCargaMasivaVehiculos.Validate(pVal)
                        'End If

                        If pVal.FormTypeEx = mc_strCosteoDeEntradas Then
                            m_oFormularioCosteoDeEntradas.ManejadorEventoValidate(pVal.FormTypeEx, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strEntradaDeVehiculos Then
                            m_oFormularioEntradaDeVehiculos.ManejadorEventoValidate(pVal.FormTypeEx, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_StrPedidoVehiculos Then
                            m_oFormularioPedidoVehiculos.ManejadorEventoValidate(pVal.FormTypeEx, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strControlVehiculo Then
                            m_oVehiculos.ManejadorEventoValidate(pVal.FormTypeEx, pVal, BubbleEvent)
                        End If



                        If pVal.FormTypeEx = mc_strUISCGD_Citas Then
                            m_oFormularioCitas.ManejadorEventoValidate(pVal.FormTypeEx, pVal, BubbleEvent)
                        End If


                        'Agregado 29/10/2010: Guarda valores antes de validate
                    Case SAPbouiCOM.BoEventTypes.et_GOT_FOCUS
                        If pVal.FormTypeEx = mc_strControlCVenta Then
                            otmpForm = SBO_Application.Forms.Item(FormUID)
                            m_oCVenta.ManejoEventoGotFocus(otmpForm, pVal)
                        End If
                        If pVal.FormTypeEx = mc_strFacturaCliente OrElse pVal.FormTypeEx = mc_strBoleta Then
                            Call m_oFacturaClientes.ManejadorEventoGOTFOCUSPress(pVal, pVal.FormUID, BubbleEvent)
                        End If

                    Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS

                        If pVal.FormTypeEx = mc_strUniqueIDReportesCosteo AndAlso pVal.ActionSuccess Then
                            BubbleEvent = False
                            Select Case pVal.ItemUID
                                Case ReportesCosteoCls.mc_strdatTransit
                                    '                                    Utilitarios.Fecha(pVal.FormUID, ReportesCosteoCls.mc_strdatTransit, SBO_Application, m_strTextoFechaAnterior)
                                Case "datInventa"
                                    '                                    Utilitarios.Fecha(pVal.FormUID, "datInventa", SBO_Application, m_strTextoFechaAnterior)
                                Case ReportesCosteoCls.mc_strtxtInicio
                                    '                                    Utilitarios.Fecha(pVal.FormUID, ReportesCosteoCls.mc_strtxtInicio, SBO_Application, m_strTextoFechaAnterior)
                                Case ReportesCosteoCls.mc_strtxtFin
                                    '                                    Utilitarios.Fecha(pVal.FormUID, ReportesCosteoCls.mc_strtxtFin, SBO_Application, m_strTextoFechaAnterior)
                            End Select

                        End If




                        If pVal.FormTypeEx = mc_strUIDFormConfiguracionMSJ AndAlso pVal.ActionSuccess Then
                            BubbleEvent = False
                            Select Case pVal.ItemUID
                                Case "txtSucu"
                                    otmpForm = SBO_Application.Forms.Item(FormUID)
                                    m_oFormularioConfigNivelesAprob.ManejoEventoGotFocus(otmpForm, pVal)
                            End Select

                        End If

                        If pVal.FormTypeEx = mc_strFormMediosPago Then
                            m_oMediosPago.ManejadorEventoLostFocus(pVal.FormTypeEx, pVal, BubbleEvent)
                        End If

                    Case SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED

                        If pVal.FormTypeEx = mc_strDocumentoPreliminar AndAlso pVal.BeforeAction Then

                            blnTransferenciaDesdeDraft = True

                            otmpForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                            Dim oEditText As SAPbouiCOM.EditText
                            Dim oRefItem As SAPbouiCOM.Item


                            oRefItem = otmpForm.Items.Item("3")
                            oMatrixDraft = DirectCast(oRefItem.Specific, Matrix)


                            intValor = pVal.Row

                            oEditText = CType(oMatrixDraft.Columns.Item(1).Cells.Item(intValor).Specific, EditText)
                            Dim strDocEntryBorrar As String = oEditText.Value


                        End If

                        If pVal.FormTypeEx = mc_strGeneraFI AndAlso pVal.BeforeAction AndAlso pVal.ItemUID = "grdOV" Then

                            BubbleEvent = False
                            strIDContrato = m_oCotizacion.DevolverIDFactura(pVal.Row, pVal.FormTypeEx, pVal.ColUID)
                            'Call m_oCotizacion.DesActivarLinkBotton(pVal.FormTypeEx)
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto(mc_strUIFacturasInt, False) Then
                                Call m_oFacturaInterna.CargaFormulario()
                                Call m_oFacturaInterna.CargarFactura(strIDContrato)
                            End If

                        End If

                        If pVal.FormTypeEx = mc_strUIListaContXUnidad AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "col_CV" Then



                        End If

                        If pVal.FormTypeEx = mc_strUniqueIDBCV AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "colIDCont" Then

                            strIDContrato = m_oBuscadorCV.DevolverIDContrato(pVal.Row, pVal.FormTypeEx)

                            'valido se valida que se use Plan de ventas por empleado y
                            'que el contrato sea en tramite o nivel 1
                            If m_udoMenu.intNivel = 1 Then
                                blnUsaEmpleadoContrato = m_udoMenu.blnPorEmpleado
                                blnEsNivelTramite = True
                            End If

                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_frmContVent", False) Then
                                Call m_oCVenta.DibujarFormularioContratoVentas("", False)
                                Call m_oCVenta.CargarContrato(strIDContrato, "SCGD_frmContVent", blnUsaEmpleadoContrato, blnEsNivelTramite)
                                blnEsNivelTramite = False
                            End If

                            m_oCVenta.m_blnCargoManejarEstados = False

                        ElseIf pVal.FormTypeEx = mc_strUniqueIDBCV AndAlso pVal.BeforeAction AndAlso pVal.ColUID = "colIDCont" Then

                            m_oCVenta.m_blnCargoManejarEstados = True

                        End If

                        If pVal.FormTypeEx = mc_strUniqueIDLCV AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "colIDCont" Then

                            strIDContrato = m_oListadoCV.DevolverIDContrato(pVal.Row, pVal.FormTypeEx)
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_frmContVent", False) Then
                                Call m_oCVenta.DibujarFormularioContratoVentas("", False)
                                Call m_oCVenta.CargarContrato(strIDContrato, "SCGD_frmContVent")
                            End If

                            m_oCVenta.m_blnCargoManejarEstados = False

                        ElseIf pVal.FormTypeEx = mc_strUniqueIDLCV AndAlso pVal.BeforeAction AndAlso pVal.ColUID = "colIDCont" Then

                            m_oCVenta.m_blnCargoManejarEstados = True

                        End If

                        If pVal.FormTypeEx = mc_strFormLstReq AndAlso pVal.ActionSuccess Then
                            If (m_oFormularioRequisiciones IsNot Nothing) Then
                                If Not oGestorFormularios.FormularioAbierto(m_oFormularioRequisiciones, activarSiEstaAbierto:=True) Then
                                    m_oFormularioRequisiciones.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioRequisiciones)
                                End If
                            End If
                            m_oFormularioListadoRequisiciones.ManejadorEventoLinkPress(pVal, FormUID, BubbleEvent, m_oFormularioRequisiciones)
                        End If

                        If pVal.FormTypeEx = mc_strFormLstSolEsp AndAlso pVal.ActionSuccess Then
                            If (m_oFormularioSolicitudEspecificos IsNot Nothing) Then
                                If Not oGestorFormularios.FormularioAbierto(m_oFormularioSolicitudEspecificos, activarSiEstaAbierto:=True) Then
                                    m_oFormularioSolicitudEspecificos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioSolicitudEspecificos)
                                End If
                            End If
                            m_oFormularioListadoSolicitudEspecificos.ManejadorEventoLinkPress(pVal, BubbleEvent, m_oFormularioSolicitudEspecificos)
                        End If

                        If pVal.FormTypeEx = mc_strUIDFormBusquedas AndAlso pVal.ActionSuccess Then
                            m_oFormularioBusquedaOT.ManejadorEventoLinkPress(pVal, BubbleEvent, m_oFormularioOrdenTrabajo)
                        End If

                        If pVal.FormTypeEx = mc_strUIListaContXUnidad AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "col_CV" Then

                            strIDContrato = m_oListaCVXUnidad.DevolverIDContrato(pVal.Row, pVal.FormTypeEx)
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_frmContVent", False) Then
                                Call m_oCVenta.DibujarFormularioContratoVentas("", False)
                                Call m_oCVenta.CargarContrato(strIDContrato, "SCGD_frmContVent")
                            End If

                            m_oCVenta.m_blnCargoManejarEstados = False

                        ElseIf pVal.FormTypeEx = mc_strUIListaContXUnidad AndAlso pVal.BeforeAction AndAlso pVal.ColUID = "col_CV" Then

                            m_oCVenta.m_blnCargoManejarEstados = True

                        End If


                        '*************************************************************************************************************
                        If pVal.FormTypeEx = mc_strUISCGD_Revertir AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "colNumCont" Then
                            'If pVal.FormTypeEx = mc_strUniqueIDContRevertidos AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "colNumCont" Then
                            'If pVal.FormTypeEx = mc_strUniqueIDContRevertidos AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "colNumCont" Then

                            strIDContrato = m_oListadoContratosReversados.DevolverIDContrato(pVal.Row, pVal.FormTypeEx)
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_frmContVent", False) Then
                                Call m_oCVenta.DibujarFormularioContratoVentas("", False)
                                Call m_oCVenta.CargarContrato(strIDContrato, "SCGD_frmContVent")
                            End If

                            m_oCVenta.m_blnCargoManejarEstados = False

                        ElseIf pVal.FormTypeEx = mc_strUISCGD_Revertir AndAlso pVal.BeforeAction AndAlso pVal.ColUID = "colNumCont" Then

                            m_oCVenta.m_blnCargoManejarEstados = True

                        End If

                        'Agregado 23/09/2011: Cargar prestamo en lista de contratos reversados
                        '*************************************************************************************************************
                        If pVal.FormTypeEx = mc_strUISCGD_Revertir AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "col_Prest" Then

                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioPrestamo, activarSiEstaAbierto:=True) Then

                                Dim strPrestamo As String
                                Dim oMatrix As SAPbouiCOM.Matrix

                                m_oFormularioPrestamo.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioPrestamo)

                                oMatrix = DirectCast(SBO_Application.Forms.Item("SCGD_Revertir_").Items.Item("mtx_01").Specific, SAPbouiCOM.Matrix)
                                strPrestamo = oMatrix.Columns.Item("col_Prest").Cells.Item(pVal.Row).Specific.String()

                                m_oFormularioPrestamo.CargarPrestamo(strPrestamo)

                            End If

                        End If

                        '*************************************************************************************************************
                        If pVal.FormTypeEx = mc_strUniqueIDListaARevertir AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "colIDCont" Then
                            'If pVal.FormTypeEx = mc_strUniqueIDContRevertidos AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "colNumCont" Then

                            strIDContrato = m_oListaContratos_a_Reversar.DevolverIDContrato(pVal.Row, pVal.FormTypeEx)
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_frmContVent", False) Then
                                Call m_oCVenta.DibujarFormularioContratoVentas("", False)
                                Call m_oCVenta.CargarContrato(strIDContrato, "SCGD_frmContVent")
                            End If

                            m_oCVenta.m_blnCargoManejarEstados = False
                        ElseIf pVal.FormTypeEx = mc_strUniqueIDConSegPV AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "colIDCont" Then

                            strIDContrato = m_oListaContratosSegPV.DevolverIDContrato(pVal.Row, pVal.FormTypeEx)
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_frmContVent", False) Then
                                Call m_oCVenta.DibujarFormularioContratoVentas("", False)
                                Call m_oCVenta.CargarContrato(strIDContrato, "SCGD_frmContVent", False, False, True)
                            End If

                            m_oCVenta.m_blnCargoManejarEstados = False

                        ElseIf pVal.FormTypeEx = mc_strUniqueIDListaARevertir AndAlso pVal.BeforeAction AndAlso pVal.ColUID = "colIDCont" Then
                            m_oCVenta.m_blnCargoManejarEstados = True
                        ElseIf pVal.FormTypeEx = mc_strUniqueIDConSegPV AndAlso pVal.BeforeAction AndAlso pVal.ColUID = "colIDCont" Then
                            m_oCVenta.m_blnCargoManejarEstados = True
                        End If

                        If pVal.FormTypeEx = mc_strUISCGD_Revertir AndAlso pVal.BeforeAction Then

                            Dim blnMultiplesUsados As Boolean = False

                            If pVal.ColUID = "colAsEnt" Or pVal.ColUID = "colAsEnRev" Or pVal.ColUID = "colSaCoVeh" Then

                                blnMultiplesUsados = m_oListadoContratosReversados.ValidarEntradas(pVal.Row, pVal.ColUID)

                                If blnMultiplesUsados = True Then

                                    BubbleEvent = False

                                End If

                            End If

                        End If

                        '*************************************************************************************************************

                        If pVal.FormTypeEx = mc_strUniqueIDVSC AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "cl_Cont" Then

                            strIDContrato = m_oVehiculosACostear.DevolverIDContrato(pVal.Row, pVal.FormTypeEx, "cl_Cont")
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_frmContVent", False) Then
                                Call m_oCVenta.DibujarFormularioContratoVentas("", False, False, True)
                                Call m_oCVenta.CargarContrato(strIDContrato, "SCGD_frmContVent")
                            End If

                            m_oCVenta.m_blnCargoManejarEstados = False

                        ElseIf pVal.FormTypeEx = mc_strUniqueIDVSC AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "cl_Cont" Then

                            m_oCVenta.m_blnCargoManejarEstados = True

                        End If

                        If pVal.FormTypeEx = mc_strUniqueIDVSC AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "cl_Unid" Then

                            strIDContrato = m_oVehiculosACostear.DevolverIDContrato(pVal.Row, pVal.FormTypeEx, "cl_Unid")
                            strIDContrato = Utilitarios.EjecutarConsulta("Select Code from [@SCGD_VEHICULO] where U_Cod_Unid = '" & strIDContrato & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then
                                Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                          strIDContrato, _
                                                          True, _
                                                          "", _
                                                          0,
                                                          True,
                                                          False,
                                                          VehiculosCls.ModoFormulario.scgVentas) 'Llamado a la ventana Maestro de Vehiculos
                            End If


                        End If



                        If pVal.FormTypeEx = mc_strUniqueIDInventariovehiculos AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "Col_Unid" Then

                            strIDContrato = m_oInventarioVehiculos.DevolverCodeVehiculo(pVal.Row, pVal.FormUID)
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then
                                Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                          strIDContrato, _
                                                          True, _
                                                          "", _
                                                          0,
                                                          True,
                                                          False,
                                                          VehiculosCls.ModoFormulario.scgTaller) 'Llamado a la ventana Maestro de Vehiculos de Tipo Servicio

                            End If


                        End If


                        If pVal.FormTypeEx = mc_strFORM_EstadosOT AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "Col_Unid" Then

                            strIDContrato = m_oEstadosOT.DevolverCodeVehiculo(pVal.Row, pVal.FormUID)
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then
                                Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                          strIDContrato, _
                                                          True, _
                                                          "", _
                                                          0, True, False, VehiculosCls.ModoFormulario.scgVentas)
                            End If


                        End If

                        If pVal.FormTypeEx = mc_strUILISTADOGR AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "V_1" Then

                            strIDContrato = m_oListadoGR.DevolverDatoGoodReceipt(pVal.FormUID, pVal.Row)
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto(mc_strUIGOODENT, False) Then
                                Call m_oGoodReceive.CargaFormularioGoodReceive("", "", "", "", "", "", strIDContrato, "", "")
                            End If


                        End If

                        'Agregado 13/01/2011: Maneja evento de carga de vehiculos en la matriz del contrato de venta
                        If pVal.ActionSuccess AndAlso pVal.FormTypeEx = mc_strControlCVenta _
                        AndAlso (pVal.ItemUID = "mtx_Vehi" Or pVal.ItemUID = "lkVehUs") _
                        AndAlso Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then

                            Call m_oCVenta.CargarVentanaDMVehiculo(pVal, SBO_Application, m_oVehiculos)

                        End If
                        If pVal.ActionSuccess AndAlso pVal.FormTypeEx = mc_strEntradaDeVehiculos AndAlso
                            (pVal.ItemUID = "mtx_Unidad") AndAlso
                            Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then

                            otmpForm = SBO_Application.Forms.ActiveForm
                            'editCell = DirectCast(otmpForm.Items.Item("txtIDVehi").Specific, EditText)
                            Dim oMatrix As SAPbouiCOM.Matrix
                            Dim strUnidad As String
                            Dim strIDVeh As String

                            oMatrix = DirectCast(otmpForm.Items.Item("mtx_Unidad").Specific, SAPbouiCOM.Matrix)
                            strUnidad = oMatrix.Columns.Item("col_Unid").Cells.Item(pVal.Row).Specific.String()
                            strIDVeh = Utilitarios.EjecutarConsulta("SELECT DocEntry FROM [@SCGD_VEHICULO] WHERE U_Cod_Unid = '" & strUnidad & "'", SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)
                            If Not String.IsNullOrEmpty(strIDVeh) Then
                                Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                         strIDVeh, _
                                                         True, _
                                                         "", _
                                                         0, True, False, VehiculosCls.ModoFormulario.scgTaller)
                            Else
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.EntradaDeVehiculosLaUnidadNoExiste, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            End If

                            otmpForm = Nothing
                        End If
                        If pVal.ActionSuccess AndAlso pVal.FormTypeEx = mc_strCosteoDeEntradas AndAlso
                           (pVal.ItemUID = "mtx_Vehi") AndAlso
                           Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then

                            otmpForm = SBO_Application.Forms.ActiveForm
                            'editCell = DirectCast(otmpForm.Items.Item("txtIDVehi").Specific, EditText)
                            Dim oMatrix As SAPbouiCOM.Matrix
                            Dim strUnidad As String
                            Dim strIDVeh As String

                            oMatrix = DirectCast(otmpForm.Items.Item("mtx_Vehi").Specific, SAPbouiCOM.Matrix)
                            strUnidad = oMatrix.Columns.Item("col_Cod").Cells.Item(pVal.Row).Specific.String()
                            strIDVeh = Utilitarios.EjecutarConsulta("SELECT DocEntry FROM [@SCGD_VEHICULO] WHERE U_Cod_Unid = '" & strUnidad & "'", SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                            Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                         strIDVeh, _
                                                         True, _
                                                         "", _
                                                         0, True, False, VehiculosCls.ModoFormulario.scgTaller)

                            otmpForm = Nothing
                        End If
                        'JVR Devolucion de Vehiculos

                        If pVal.ActionSuccess AndAlso pVal.FormTypeEx = mc_strDevolucionDeVehiculos AndAlso
                          (pVal.ItemUID = "mtxVeh") AndAlso
                          Not ValidarSiFormularioAbierto("SCGD_DET_1", False) AndAlso
                          pVal.ColUID = "col_Unid" Then

                            otmpForm = SBO_Application.Forms.ActiveForm

                            Dim oMatrix As SAPbouiCOM.Matrix
                            Dim strUnidad As String
                            Dim strIDVeh As String

                            oMatrix = DirectCast(otmpForm.Items.Item("mtxVeh").Specific, SAPbouiCOM.Matrix)
                            strUnidad = oMatrix.Columns.Item("col_Unid").Cells.Item(pVal.Row).Specific.String()
                            strIDVeh = Utilitarios.EjecutarConsulta("SELECT DocEntry FROM [@SCGD_VEHICULO] WHERE U_Cod_Unid = '" & strUnidad & "'", SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                            Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                         strIDVeh, _
                                                         True, _
                                                         "", _
                                                         0, True, False, VehiculosCls.ModoFormulario.scgTaller)

                            otmpForm = Nothing
                        End If


                        '********************************** ********* ******** **************************************************
                        'Agregado 09/05/2013: Maneja evento de Carga de Citas en la matriz de Búsquedas de Citas
                        '********************************** ********* ******** **************************************************

                        If pVal.ActionSuccess AndAlso pVal.FormTypeEx = m_oFormularioBusquedaOT.FormType AndAlso pVal.ItemUID = "mtxBusq" Then

                            If pVal.ColUID = "ColDocCit" Then
                                otmpForm = SBO_Application.Forms.ActiveForm

                                Dim oMatrix As SAPbouiCOM.Matrix
                                Dim strCodCita As String
                                Dim SerieCita As String = String.Empty
                                Dim NumeroCita As String = String.Empty

                                oMatrix = DirectCast(otmpForm.Items.Item("mtxBusq").Specific, SAPbouiCOM.Matrix)
                                strCodCita = oMatrix.Columns.Item("ColDocCit").Cells.Item(pVal.Row).Specific.String()

                                'Obtiene la configuración del tamaño de las celdas de la agenda
                                If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType) Then
                                    oVersionModuloCita = DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType
                                Else
                                    oVersionModuloCita = frmListaCitas.VersionModuloCita.Estandar
                                End If

                                If oVersionModuloCita = frmListaCitas.VersionModuloCita.Estandar Then
                                    If Not oGestorFormularios.FormularioAbierto(m_oFormularioCitas, activarSiEstaAbierto:=True) Then
                                        m_oFormularioCitas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioCitas)
                                        m_oFormularioCitas.CargarCitaDesdePanel_Existe(strCodCita)
                                    End If
                                Else
                                    ConstructorCitas.CrearInstanciaFormularioExistente(strCodCita)
                                End If

                                otmpForm = Nothing

                            End If


                        End If

                        '********************************** ********* ******** **************************************************
                        'Agregado 09/05/2013: Maneja evento de Carga de Citas en la matriz de Búsquedas de Citas
                        '********************************** ********* ******** **************************************************

                        If pVal.ActionSuccess AndAlso pVal.FormTypeEx = m_oFormularioBusquedasCitas.FormType AndAlso pVal.ItemUID = "mtxBusq" Then
                            If pVal.ColUID = "ColDocCit" Then
                                otmpForm = SBO_Application.Forms.ActiveForm

                                Dim oMatrix As SAPbouiCOM.Matrix
                                Dim strCodCita As String
                                Dim SerieCita As String = String.Empty
                                Dim NumeroCita As String = String.Empty

                                oMatrix = DirectCast(otmpForm.Items.Item("mtxBusq").Specific, SAPbouiCOM.Matrix)
                                strCodCita = oMatrix.Columns.Item("ColDocCit").Cells.Item(pVal.Row).Specific.String()

                                'Obtiene la configuración del tamaño de las celdas de la agenda
                                If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType) Then
                                    oVersionModuloCita = DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType
                                Else
                                    oVersionModuloCita = frmListaCitas.VersionModuloCita.Estandar
                                End If

                                If oVersionModuloCita = frmListaCitas.VersionModuloCita.Estandar Then
                                    If Not oGestorFormularios.FormularioAbierto(m_oFormularioCitas, activarSiEstaAbierto:=True) Then
                                        m_oFormularioCitas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioCitas)
                                        m_oFormularioCitas.CargarCitaDesdePanel_Existe(strCodCita)
                                    End If
                                Else
                                    ConstructorCitas.CrearInstanciaFormularioExistente(strCodCita)
                                End If

                                otmpForm = Nothing

                            End If


                        End If

                        '********************************** ********* ******** **************************************************
                        'Agregado 09/05/2013: Maneja evento de Carga de Vehículos en la matriz de Búsquedas de Ordenes 
                        '********************************** ********* ******** **************************************************

                        If pVal.FormTypeEx = m_oFormularioBusquedasCitas.FormType AndAlso pVal.ActionSuccess AndAlso pVal.ItemUID = "mtxBusq" Then

                            If pVal.ColUID = "ColNoUni" AndAlso Not ValidarSiFormularioAbierto(mc_strUniqueID, False) Then

                                otmpForm = SBO_Application.Forms.ActiveForm

                                Dim oMatrix As SAPbouiCOM.Matrix
                                Dim strNumUnidad As String
                                Dim strCodVehi As String

                                m_oVehiculos = New VehiculosCls(m_oCompany, SBO_Application)

                                oMatrix = DirectCast(otmpForm.Items.Item("mtxBusq").Specific, SAPbouiCOM.Matrix)
                                strNumUnidad = oMatrix.Columns.Item("ColNoUni").Cells.Item(pVal.Row).Specific.String()
                                strNumUnidad = strNumUnidad.Trim()
                                strCodVehi = Utilitarios.EjecutarConsulta("SELECT DocEntry FROM [@SCGD_VEHICULO] WHERE U_Cod_Unid = '" & strNumUnidad & "'", SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                                Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                         strCodVehi, _
                                                         True, _
                                                         "", _
                                                         0, True, False, VehiculosCls.ModoFormulario.scgTaller)

                            End If

                        End If



                        'Agregado 25/05/2012: Maneja evento de carga de vehiculos en Balance contrato ventas
                        If pVal.ActionSuccess AndAlso pVal.FormTypeEx = mc_FormBalance _
                        AndAlso (pVal.ItemUID = "mtxVehic") _
                        AndAlso Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then

                            otmpForm = SBO_Application.Forms.ActiveForm

                            Dim oMatrix As SAPbouiCOM.Matrix
                            Dim strUnidad As String
                            Dim strIDVeh As String

                            oMatrix = DirectCast(otmpForm.Items.Item("mtxVehic").Specific, SAPbouiCOM.Matrix)
                            strUnidad = oMatrix.Columns.Item("Col_Unid").Cells.Item(pVal.Row).Specific.String()
                            strIDVeh = Utilitarios.EjecutarConsulta("SELECT DocEntry FROM [@SCGD_VEHICULO] WHERE U_Cod_Unid = '" & strUnidad & "'", SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                            Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                         strIDVeh, _
                                                         True, _
                                                         "", _
                                                         0, True, False, VehiculosCls.ModoFormulario.scgVentas)

                            otmpForm = Nothing
                        End If

                        'Agregado 06/04/2011: Maneja evento de carga de vehiculos en la matriz de Traslado de CosTos
                        If pVal.ActionSuccess AndAlso pVal.FormTypeEx = mc_strUITrasC _
                       AndAlso (pVal.ItemUID = "mtx_01") _
                       AndAlso Not ValidarSiFormularioAbierto("SCGD_TCT", False) Then

                            otmpForm = SBO_Application.Forms.ActiveForm

                            If otmpForm.TypeEx <> "806" Then

                                Dim oMatrix As SAPbouiCOM.Matrix
                                Dim strUnidad As String
                                Dim strIDVeh As String

                                oMatrix = DirectCast(otmpForm.Items.Item("mtx_01").Specific, SAPbouiCOM.Matrix)

                                If pVal.ItemUID = "mtx_01" Then

                                    If pVal.ColUID = "colUnidad" Then
                                        strUnidad = oMatrix.Columns.Item("colUnidad").Cells.Item(pVal.Row).Specific.String()
                                        strIDVeh = Utilitarios.EjecutarConsulta("SELECT DocEntry FROM [@SCGD_VEHICULO] WHERE U_Cod_Unid = '" & strUnidad & "'", SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                                        Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                                     strIDVeh, _
                                                                     True, _
                                                                     "", _
                                                                     0, True, False, VehiculosCls.ModoFormulario.scgVentas)

                                    ElseIf pVal.ColUID = "colEntrada" Then
                                        Dim valor As String = oMatrix.Columns.Item("colEntrada").Cells.Item(pVal.Row).Specific.String()
                                        If valor <> "" AndAlso Not ValidarSiFormularioAbierto(mc_strUIGOODENT, False) Then
                                            Call m_oGoodReceive.CargaFormularioGoodReceive("", "", "", "", "", "", valor, "", "")
                                        End If
                                    End If

                                End If

                            End If
                        End If

                        'Agregado 19/11/2010: Carga entradas en la salida de vehiculo
                        If pVal.FormTypeEx = mc_strUIGOODISSUE AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "col_1" Then

                            strIDContrato = m_oGoodIssue.DevolverDatoGoodReceipt(pVal.FormUID, pVal.Row)
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto(mc_strUIGOODENT, False) Then
                                Call m_oGoodReceive.CargaFormularioGoodReceive("", "", "", "", "", "", strIDContrato, "", "")
                            End If

                        End If

                        If pVal.FormTypeEx = mc_strUIRecosteos AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "V_1" Then

                            strIDContrato = m_oRecosteos.DevolverDatoGoodReceipt(pVal.FormUID, pVal.Row)
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto(mc_strUIGOODENT, False) Then
                                Call m_oGoodReceive.CargaFormularioGoodReceive("", "", "", "", "", "", strIDContrato, "", "")
                            End If


                        End If
                        If pVal.FormTypeEx = mc_strUniqueIDCosteoMultiplesUnidades AndAlso pVal.ActionSuccess Then

                            Dim oMatrix As SAPbouiCOM.Matrix
                            Dim strUnidad As String
                            Dim strIDVeh As String

                            otmpForm = SBO_Application.Forms.ActiveForm

                            If pVal.ItemUID = "mtx_Recost" Then

                                oMatrix = DirectCast(otmpForm.Items.Item("mtx_Recost").Specific, SAPbouiCOM.Matrix)

                                Dim valor As String = oMatrix.Columns.Item("col_DocEn").Cells.Item(pVal.Row).Specific.String()
                                If valor <> "" AndAlso Not ValidarSiFormularioAbierto(mc_strUIGOODENT, False) Then
                                    Call m_oGoodReceive.CargaFormularioGoodReceive("", "", "", "", "", "", valor, "", "")
                                End If
                            End If

                            If pVal.ItemUID = "mtx_VehSin" Then

                                oMatrix = DirectCast(otmpForm.Items.Item("mtx_VehSin").Specific, SAPbouiCOM.Matrix)
                                strUnidad = oMatrix.Columns.Item("col_Unid").Cells.Item(pVal.Row).Specific.String()
                                If strUnidad <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then
                                    strIDVeh = Utilitarios.EjecutarConsulta("SELECT DocEntry FROM [@SCGD_VEHICULO] WHERE U_Cod_Unid = '" & strUnidad & "'", SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                                    Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                                 strIDVeh, _
                                                                 True, _
                                                                 "", _
                                                                 0, True, False, VehiculosCls.ModoFormulario.scgTaller)

                                    otmpForm = Nothing

                                End If
                            End If


                        End If
                        If pVal.FormTypeEx = mc_strUniqueIDSalidaMultiplesUnidades AndAlso pVal.ActionSuccess Then

                            Dim oMatrix As SAPbouiCOM.Matrix
                            Dim strUnidad As String
                            Dim strIDVeh As String

                            otmpForm = SBO_Application.Forms.ActiveForm

                            If pVal.ItemUID = "mtx_Recost" Then

                                oMatrix = DirectCast(otmpForm.Items.Item("mtx_Recost").Specific, SAPbouiCOM.Matrix)

                                Dim valor As String = oMatrix.Columns.Item("col_DocEn").Cells.Item(pVal.Row).Specific.String()
                                If valor <> "" AndAlso Not ValidarSiFormularioAbierto(mc_strUIGOODENT, False) Then
                                    Call m_oGoodReceive.CargaFormularioGoodReceive("", "", "", "", "", "", valor, "", "")
                                End If
                            End If

                        End If

                        If pVal.FormTypeEx = mc_strFormKardex AndAlso pVal.ActionSuccess AndAlso pVal.ColUID = "col_DocEn" Then

                            Dim oMatrix As SAPbouiCOM.Matrix
                            Dim strUnidad As String
                            Dim strIDVeh As String

                            otmpForm = SBO_Application.Forms.ActiveForm

                            If pVal.ItemUID = "mtxVehi2" Then

                                oMatrix = DirectCast(otmpForm.Items.Item("mtxVehi2").Specific, SAPbouiCOM.Matrix)

                                Dim TipoDocumento As String = oMatrix.Columns.Item("col_TipoD").Cells.Item(pVal.Row).Specific.String()

                                Dim valor As String = oMatrix.Columns.Item("col_DocEn").Cells.Item(pVal.Row).Specific.String()

                                If TipoDocumento = "ENT" Then

                                    If valor <> "" AndAlso Not ValidarSiFormularioAbierto(mc_strUIGOODENT, False) Then
                                        Call m_oGoodReceive.CargaFormularioGoodReceive("", "", "", "", "", "", valor, "", "")
                                    End If
                                ElseIf TipoDocumento = "SAL" Then

                                    If valor <> "" AndAlso Not ValidarSiFormularioAbierto(mc_strUIGOODISSUE, False) Then
                                        Call m_oGoodIssue.CargaFormularioGoodIssue(valor, True)
                                    End If
                                ElseIf TipoDocumento = "TRL" Then

                                    If valor <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_TCU", False) Then
                                        m_oTrasladoCostos.CargaFormularioTrasladoCostos(True, valor)
                                    End If




                                End If

                            End If

                        End If

                    Case SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD

                        If pVal.FormTypeEx = mc_strIdFormaCotizacion Then

                            m_oRecepcionVHUI.ManejadorEventoClose(FormUID, pVal, BubbleEvent)

                        End If
                    Case SAPbouiCOM.BoEventTypes.et_FORM_RESIZE
                        'Oferta de Ventas cambio de tamaño, reacomoda los controles
                        If pVal.FormTypeEx = mc_strIdFormaCotizacion AndAlso m_blnUsaOrdenesDeTrabajo Then
                            m_oRecepcionVHUI.FormResizeEvent(FormUID, pVal, BubbleEvent)
                        End If
                        'Orden de Ventas cambio de tamaño, reacomoda los controles
                        If pVal.FormTypeEx = mc_strOrdenDeVenta Then
                            m_oOrdenVenta.FormResizeEvent(pVal.FormUID, pVal, BubbleEvent)
                        End If
                        'Factura de Ventas y Boleta cambio de tamaño, reacomoda los controles
                        If pVal.FormTypeEx = mc_strFacturaCliente Or pVal.FormTypeEx = mc_strBoleta Then
                            m_oFacturaClientes.FormResizeEvent(pVal.FormUID, pVal, BubbleEvent)
                        End If
                    Case SAPbouiCOM.BoEventTypes.et_FORM_LOAD

                        If pVal.FormTypeEx = mc_strSolicitudOTEspecial Then

                        End If

                        If pVal.FormType = mc_strIdFormaCotizacion Then
                            Dim oMenuItem As MenuItem
                            oMenuItem = SBO_Application.Menus.Item("1293")
                            oMenuItem.Enabled = False
                        End If

                        If pVal.FormTypeEx = FormularioLLamadaServicioSBO.FormType AndAlso m_blnUsaOrdenesDeTrabajo Then
                            m_oLlamadaServicio.ManejadorEventoLoad(FormUID, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strIdFormaCotizacion AndAlso m_blnUsaOrdenesDeTrabajo Then
                            ConstructorOfertaVentas.CargarControles(FormUID, pVal, BubbleEvent)
                            m_oRecepcionVHUI.ActivarAvaluo(FormUID, pVal, BubbleEvent)
                        End If

                        'Agregado 03/08/2017: Manejo para la ventana medios de pago en pagos recibidos
                        If pVal.FormTypeEx = mc_strFormMediosPago Then
                            m_oMediosPago.ManejadorEventoLoad(FormUID, pVal, BubbleEvent)
                        End If

                        'Agregado 27/09/2010: Manejo para cargar componentes de oportunidad de venta
                        If pVal.FormTypeEx = mc_strOportunidadVenta AndAlso pVal.BeforeAction Then
                            m_oOportunidadVenta.ManejoEventoLoad(pVal)
                        End If

                        'Agregado 04/11/2010: Manejo para cargar componentes de salida de mercancia
                        If pVal.FormTypeEx = mc_strSalidaMercancia AndAlso pVal.BeforeAction Then
                            m_oSalidaMercancia.ManejoEventoLoad(pVal)
                        End If

                        'Agregado 13/12/2010: Manejo para cargar componentes de entrada de mercancia
                        If pVal.FormTypeEx = mc_strEntradaMercancia AndAlso pVal.BeforeAction Then

                            m_oEntradaMercancia.ManejoEventoLoad(pVal)
                        End If

                        If pVal.FormTypeEx = mc_strOrdenDeCompra AndAlso m_blnUsaOrdenesDeTrabajo Then

                            'Manejo de la carga de ordenes de compra 
                            m_oCompras.ManejaEventoLoad(FormUID, pVal, BubbleEvent)
                            m_oComprasEnVentas.ManejaEventoLoad(FormUID, pVal, BubbleEvent)

                        End If


                        If pVal.FormTypeEx = mc_strOrdenDeVenta _
                            AndAlso Not pVal.BeforeAction _
                            AndAlso pVal.ActionSuccess AndAlso m_blnUsaOrdenesDeTrabajo Then

                            oformOrdenVenta = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                            'Call RecepcionVehiculo.AgregaContotrolNoOT(oformOrdenVenta, "ORDR", SBO_Application)
                            RecepcionVehiculo.AgregarControlesDocumentos(oformOrdenVenta)
                        End If

                        If pVal.FormTypeEx = mc_strTrasladoInventario _
                            AndAlso Not pVal.BeforeAction _
                            AndAlso pVal.ActionSuccess AndAlso m_blnUsaOrdenesDeTrabajo Then

                            oformOrdenVenta = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                            'Call RecepcionVehiculo.AgregaContotrolNoOT(oformOrdenVenta, "OWTR", SBO_Application, 115, 302, 415)
                            RecepcionVehiculo.AgregarControlesDocumentos(oformOrdenVenta)

                            Dim l_strSQL As String
                            Dim l_strValidaEntrega As String

                            l_strSQL = "select U_Entrega_Rep from [@SCGD_CONF_SUCURSAL] " +
                                                  " where U_Sucurs = (select branch from OUSR where User_Code = '{0}')"

                            l_strValidaEntrega = Utilitarios.EjecutarConsulta(String.Format(l_strSQL, SBO_Application.Company.UserName),
                                                                              m_oCompany.CompanyDB,
                                                                              m_oCompany.Server)

                            If l_strValidaEntrega.Equals("Y") Then
                                Call TransferenciaItems.AgregaControlCheck(oformOrdenVenta, "OWTR", SBO_Application, 172, -3)
                            End If

                        End If

                        If pVal.FormTypeEx = mc_strFacturaProveedores _
                                                   AndAlso Not pVal.BeforeAction _
                                                   AndAlso pVal.ActionSuccess AndAlso m_blnUsaOrdenesDeTrabajo Then

                            oformOrdenVenta = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                            'Call RecepcionVehiculo.AgregaContotrolNoOT(oformOrdenVenta, "OPCH", SBO_Application)
                            RecepcionVehiculo.AgregarControlesDocumentos(oformOrdenVenta)
                        End If



                        If (pVal.FormTypeEx = mc_strFacturaCliente Or pVal.FormTypeEx = mc_strBoleta) _
                         AndAlso Not pVal.BeforeAction _
                         AndAlso pVal.ActionSuccess AndAlso m_blnUsaOrdenesDeTrabajo Then

                            oFormFacturaCliente = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                            'Call RecepcionVehiculo.AgregaContotrolNoOT(oFormFacturaCliente, "OINV", SBO_Application)
                            RecepcionVehiculo.AgregarControlesDocumentos(oFormFacturaCliente)
                        End If


                        'Codigo comentado para la obtencion de la factura de clientes en el "Copiar a "
                        'desde el documento de Pedido de Clientes -- para el tema de Ubicaciones

                        'If pVal.FormTypeEx = mc_strFacturaCliente _
                        ' AndAlso pVal.BeforeAction _
                        ' AndAlso Not pVal.ActionSuccess AndAlso m_blnUsaOrdenesDeTrabajo Then

                        '    'oFormFacturaCliente = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                        '    If blnUbicaciones Then
                        '        m_oFacturaClientes.AgregarUbicacionDefectoBodegaProceso(pVal, pVal.FormTypeEx, BubbleEvent)
                        '    End If
                        'End If


                        If pVal.FormTypeEx = mc_strOrdenDeCompra _
                         AndAlso Not pVal.BeforeAction _
                         AndAlso pVal.ActionSuccess AndAlso m_blnUsaOrdenesDeTrabajo Then

                            oFormFacturaCliente = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                            'Call RecepcionVehiculo.AgregaContotrolNoOT(oFormFacturaCliente, "OPOR", SBO_Application)
                            RecepcionVehiculo.AgregarControlesDocumentos(oFormFacturaCliente)
                        End If


                        If pVal.FormTypeEx = mc_strOfertaDeCompra _
                      AndAlso Not pVal.BeforeAction _
                      AndAlso pVal.ActionSuccess AndAlso m_blnUsaOrdenesDeTrabajo Then

                            oFormFacturaCliente = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                            'Call RecepcionVehiculo.AgregaContotrolNoOT(oFormFacturaCliente, "OPQT", SBO_Application)
                            RecepcionVehiculo.AgregarControlesDocumentos(oFormFacturaCliente)
                        End If

                        If pVal.FormTypeEx = mc_strFacturaReserva _
                           AndAlso Not pVal.BeforeAction _
                           AndAlso pVal.ActionSuccess AndAlso m_blnUsaOrdenesDeTrabajo Then

                            oFormFacturaCliente = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                            'Call RecepcionVehiculo.AgregaContotrolNoOT(oFormFacturaCliente, "OINV", SBO_Application)
                            RecepcionVehiculo.AgregarControlesDocumentos(oFormFacturaCliente)
                        End If

                        'manejo de la carga del formulario de campañas
                        If pVal.FormTypeEx = mc_strIdFormCampaña Then
                            If Utilitarios.EjecutarConsulta("Select U_CnpDMS from [@SCGD_ADMIN]", m_oCompany.CompanyDB, m_oCompany.Server) = "Y" Then

                                m_oCampana.ManejadorEventoLoad(FormUID, pVal, BubbleEvent)

                            End If
                        End If

                        If pVal.FormTypeEx = mc_strUISCGD_Citas Then

                            m_oFormularioCitas.ManejadorEventoLoad(FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = m_oVentanaAutorizaciones Then

                            DocAprobacionHabilitado = False


                        End If

                        If pVal.FormTypeEx = mc_strUISCGD_FormConfFin Then
                            m_oFormularioConfFinanc.ManejadorEventoLoad(FormUID, pVal, BubbleEvent,
                                                                        CatchingEvents.DBUser, CatchingEvents.DBPassword)
                        End If

                        If pVal.FormTypeEx = mc_strOrdenDeVenta Then
                            m_oOrdenVenta.ManejadorEventoLoad(FormUID, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strSociosNegocios Then
                            m_oSociosNegocio.ManejadorEventoLoad(FormUID, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strMaestroEmpleados Then
                            m_oMaestroEmpleados.ManejadorEventoLoad(FormUID, pVal, BubbleEvent)
                        End If

                        'If pVal.FormTypeEx = mc_strPresupuestos Then
                        '    m_oFormularioPresupuestos.ManejadorEventoLoad(FormUID, pVal, BubbleEvent)
                        'End If

                    Case SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD


                        If pVal.FormTypeEx = mc_strControlVehiculo Then

                            Call m_oVehiculos.ManejadorEventoUnLoad(FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strOrdenDeCompra Then

                            m_oCompras.ManejadorEventoUnload(FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strControlCVenta Then

                            Call m_oCVenta.ManejadorEventoUnLoad(FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strUniqueIDBCV Then

                            Call m_oCVenta.ManejadorEventoUnLoad(FormUID, pVal, BubbleEvent)

                        End If

                    Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST

                        otmpForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                        ''Agregado 27/09/2010: Manejar estado del boton de GenerarCV
                        'If pVal.FormTypeEx = mc_strOportunidadVenta Then

                        '    Dim oForm As SAPbouiCOM.Form
                        '    oForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                        '    Dim strSocio As String

                        '    strSocio = oForm.Items.Item("9").Specific.value

                        '    If String.IsNullOrEmpty(strSocio) Then
                        '        oForm.Items.Item("btGeneraCV").Enabled = False
                        '    Else
                        '        oForm.Items.Item("btGeneraCV").Enabled = True
                        '    End If
                        'End If

                        '''''''Filtras cuentas de asiento
                        ''If pVal.FormTypeEx = "392" AndAlso pVal.BeforeAction AndAlso pVal.ItemUID = "76" AndAlso (pVal.ColUID = "1" Or pVal.ColUID = "2") Then
                        ''    Dim cuentas As List(Of String) = New List(Of String)()
                        ''    cuentas.Add("1-1-75-001")
                        ''    cuentas.Add("4-1-10-001")
                        ''    Dim cflFilter As ChooseFromListAccountsFilter = New ChooseFromListAccountsFilter(cuentas, pVal, SBO_Application)
                        ''    cflFilter.ApplyFilter()
                        ''End If


                        '17/08/2011: No permite cargar CFL de formularios configurados en app.config de DMS One

                        If Not String.IsNullOrEmpty(ListaFormsCFL) Then

                            Dim strFormsCFL() As String
                            Dim strForm As String = ""
                            Dim oRefItem As SAPbouiCOM.Item
                            Dim oMatrix As SAPbouiCOM.Matrix
                            Dim valorCelda As String
                            Dim strUsuarioSBO As String
                            Dim strIDUsuario As String
                            Dim strCuentaValida As String = ""

                            strFormsCFL = ListaFormsCFL.Split(",")

                            For Each strForm In strFormsCFL

                                If pVal.FormTypeEx = strForm AndAlso pVal.BeforeAction = True Then

                                    If (pVal.ItemUID = "76" AndAlso (pVal.ColUID = "1" OrElse pVal.ColUID = "2")) OrElse (pVal.ItemUID = "39" AndAlso pVal.ColUID = "2") Then

                                        oRefItem = otmpForm.Items.Item(pVal.ItemUID)

                                        oMatrix = DirectCast(oRefItem.Specific, Matrix)

                                        editCell = DirectCast(oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific, EditText)
                                        valorCelda = editCell.String

                                        strUsuarioSBO = SBO_Application.Company.UserName

                                        strIDUsuario = Utilitarios.EjecutarConsulta("Select USERID From [OUSR] Where USER_CODE = '" & strUsuarioSBO & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                                        If Not pVal.ItemUID = "76" OrElse Not pVal.ColUID = "2" Then

                                            strCuentaValida = Utilitarios.EjecutarConsulta("Select U_Cod_Cta From [@SCGD_DETALLE_CTAS] Where Code = '" & strIDUsuario & "' And U_Cod_Cta = '" & valorCelda & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                                        ElseIf pVal.ItemUID = "76" AndAlso pVal.ColUID = "2" Then

                                            strCuentaValida = Utilitarios.EjecutarConsulta("Select U_Cod_Cta From [@SCGD_DETALLE_CTAS] Where Code = '" & strIDUsuario & "' And U_Nombre_Cta = '" & valorCelda & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                                        End If

                                        'If String.IsNullOrEmpty(strCuentaValida) Then

                                        '    BubbleEvent = False

                                        '    Exit For

                                        'End If

                                    End If

                                End If

                            Next

                        End If


                        'If (pVal.FormTypeEx = mc_strRegistroDiario OrElse pVal.FormTypeEx = mc_strFacturadeCompra) AndAlso pVal.BeforeAction = True Then

                        '    BubbleEvent = False

                        'End If

                        If pVal.FormTypeEx = mc_strUITrasC Then
                            m_oTrasladoCostos.ManejadorEventoChooseFromList(pVal, pVal.FormUID, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strUIListaContXUnidad Then
                            m_oListaCVXUnidad.ManejadorEventoChooseFromList(pVal.FormUID, pVal, BubbleEvent)
                        End If

                        'Inventario Vehiculos
                        If pVal.FormTypeEx = mc_strUniqueIDInventariovehiculos Then
                            m_oInventarioVehiculos.ManejadorEventoChooseFromList(pVal.FormUID, pVal, BubbleEvent)
                        End If
                        'Llamada de Servicios
                        If pVal.FormTypeEx = FormularioLLamadaServicioSBO.FormType AndAlso m_blnUsaOrdenesDeTrabajo Then
                            m_oLlamadaServicio.ManejadorEventoChooseFromList(pVal.FormUID, pVal, BubbleEvent)
                        End If


                        If pVal.FormTypeEx = mc_strUniqueIDNivelesPV AndAlso Not pVal.BeforeAction Then

                            m_oNivelesPV.ManejadorEventoChooseFromList(pVal, pVal.FormUID, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strUniqueIDLineasFactura Then

                            m_oLineasFactura.ManejadorEventoChooseFromList(pVal, pVal.FormUID, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strUniqueIDLineasDesgloce AndAlso Not pVal.BeforeAction Then

                            m_oLineasDesgloce.ManejadorEventoChooseFromList(pVal, pVal.FormUID, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strUniqueIDTransaccionesCompras Then

                            m_oTransaccionesCompras.ManejadorEventoChooseFromList(pVal, pVal.FormUID, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = CStr(mc_strIdFormaCotizacion) AndAlso pVal.BeforeAction Then
                            If pVal.Row > 0 Then
                                If pVal.ColUID = mc_strIDItemCodeColumn OrElse pVal.ColUID = mc_strIDItemNameColumn Then
                                    'SBO_Application.StatusBar.SetText("Error 1", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    'oItem = otmpForm.Items.Item(mc_strIDBotonEjecucion)
                                    'sButton = DirectCast(oItem.Specific, SAPbouiCOM.ButtonCombo)

                                    'SBO_Application.StatusBar.SetText("Error 2", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    If otmpForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or otmpForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then

                                        oItem = otmpForm.Items.Item(mc_strMatrizFormularios)
                                        oMatriz = DirectCast(oItem.Specific, SAPbouiCOM.Matrix)

                                        Dim col As EditText
                                        'Valida si la línea ha sido procesada por DMS para OT o prepicking
                                        col = DirectCast(oMatriz.Columns.Item("U_SCGD_ID").Cells.Item(pVal.Row).Specific, EditText)
                                        If oMatriz.RowCount <> pVal.Row AndAlso Not String.IsNullOrEmpty(col.Value) Then
                                            m_blnLineaOT = True
                                            BubbleEvent = False
                                            SBO_Application.StatusBar.SetText(My.Resources.Resource.CambiarLineasCotizacion, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                                        Else
                                            m_blnLineaOT = False
                                        End If

                                    End If

                                End If

                            End If
                        End If
                        If pVal.FormTypeEx = CStr(mc_strIdFormaCotizacion) Then

                            m_oRecepcionVHUI.ManejadorEventoChooseFromList(FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strFormMediosPago Then
                            m_oMediosPago.ManejadorEventoChooseFromList(FormUID, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = CStr(mc_strIdFormCampaña) Then

                            m_oCampana.ManejadorEventoChooseFromList(FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_stridGeneraOV Then

                            m_oCotizacion.ManejadorEventoChooseFromList(FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strGeneraFI Then

                            m_oCotizacion.ManejadorEventoChooseFromListFI(FormUID, pVal, BubbleEvent)

                        End If



                        If pVal.FormTypeEx = mc_strControlVehiculo Then

                            otmpForm = SBO_Application.Forms.Item(FormUID)

                            m_oVehiculos.ManejadorEventoChooseFromList(pVal, FormUID, BubbleEvent)

                            'If pVal.FormMode = 2 Or pVal.FormMode = 3 Then
                            '    m_oVehiculos.ManejadorEventoChooseFromList(pVal, FormUID, BubbleEvent)
                            'End If
                            'If Not pVal.BeforeAction AndAlso pVal.ActionSuccess Then
                            '    If pVal.FormMode = mc_intNoModoFind Then
                            '        m_oVehiculos.ManejadorEventoChooseFromList(pVal, FormUID, BubbleEvent)
                            '        otmpForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                            '    End If
                            'End If
                        End If


                        If pVal.FormTypeEx = mc_strControlCVenta Then

                            otmpForm = SBO_Application.Forms.Item(FormUID)

                            If pVal.FormMode = 2 Or pVal.FormMode = 3 Then
                                m_oCVenta.ManejadorEventoChooseFromList(pVal, FormUID, BubbleEvent)
                            End If

                            If pVal.FormMode = mc_intNoModoFind Then
                                m_oCVenta.ManejadorEventoChooseFromList(pVal, FormUID, BubbleEvent)
                                otmpForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                            End If
                        End If

                        'No implementado
                        'If pVal.FormTypeEx = "SCGD_REP_CV" Then

                        '    otmpForm = SBO_Application.Forms.Item(FormUID)

                        '    If pVal.FormMode = 1 Or pVal.FormMode = 3 Or pVal.FormMode = 2 Then
                        '        m_oReporteCV.ManejadorEventoChooseFromList(pVal, FormUID, BubbleEvent)
                        '    End If

                        'End If

                        If pVal.FormTypeEx = mc_strUniqueIDConfiguracionesGenerales Then

                            m_oConfiguracionGeneral.ManejadorEventoChooseFromList(pVal, pVal.FormUID, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strUIGOODISSUE Then
                            m_oGoodIssue.ManejadorEventoChooseFromList(pVal.FormUID, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = strMenuEmbarqueVehiculos Then
                            m_oEmbarqueVehiculos.ManejadorEventoChooseFromList(pVal, pVal.FormUID, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strUISCGD_DimensionContableDMS Then
                            m_oDimensionesContables.ManejadorEventoChooseFromList(pVal, pVal.FormUID, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strUISCGD_DimensionContableDMSOTs Then
                            m_oDimensionesContablesOTs.ManejadorEventoChooseFromList(pVal, pVal.FormUID, BubbleEvent)
                        End If
                        If pVal.FormTypeEx = mc_strUniqueIDVSC Then
                            m_oVehiculosACostear.ManejadorEventosChooseFromList(FormUID, pVal, BubbleEvent)
                        End If
                        If pVal.FormTypeEx = mc_strUniqueIDCosteoMultiplesUnidades Then
                            m_oCosteoMultiplesUnidades.ManejadorEventosChooseFromList(FormUID, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strFormKardex Then
                            m_oFormularioKardexInventarioVehiculo.ManejadorEventoChooseFromList(FormUID, pVal, BubbleEvent)
                        End If
                    Case SAPbouiCOM.BoEventTypes.et_COMBO_SELECT

                        If pVal.ActionSuccess Then

                            Select Case pVal.FormTypeEx
                                Case mc_strUIFacturasInt
                                    m_oFacturaInterna.CargarCombosEstiloyModelo(FormUID, pVal.ItemUID)

                                Case mc_strControlVehiculo
                                    otmpForm = SBO_Application.Forms.Item(FormUID)
                                    m_oVehiculos.ManejoEventosCombo(otmpForm, pVal, FormUID, BubbleEvent)

                                Case mc_strUniqueIDLineasFactura
                                    m_oLineasFactura.ManejadorEventoComboSelect(pVal.FormUID, pVal, BubbleEvent)

                                Case mc_strUniqueIDInventariovehiculos
                                    m_oInventarioVehiculos.ManejoEventosCombo(SBO_Application.Forms.Item(FormUID), pVal, pVal.FormUID, BubbleEvent)

                                Case mc_strUniqueIDLineasDesgloce
                                    m_oLineasDesgloce.ManejadorEventoComboSelect(pVal.FormUID, pVal, BubbleEvent)

                                Case mc_strControlCVenta
                                    otmpForm = SBO_Application.Forms.Item(FormUID)
                                    m_oCVenta.ManejoEventosCombo(otmpForm, pVal, FormUID, BubbleEvent, m_oCVenta.strMonedaOrigen, m_oCVenta.strTipoCambioMoneda)
                                    'Case mc_strUniqueIDConfiguracionesGenerales
                                    '    otmpForm = SBO_Application.Forms.Item(FormUID)
                                    '    m_oConfiguracionGeneral.ManejoEventosCombo(otmpForm, pVal, pVal.FormUID, BubbleEvent)

                                Case mc_strUITrasC
                                    otmpForm = SBO_Application.Forms.Item(FormUID)
                                    m_oTrasladoCostos.ManejoEventosCombo(otmpForm, pVal, FormUID)

                                Case mc_strOportunidadVenta


                                    m_oOportunidadVenta.ManejoEventosCombo(SBO_Application.Forms.Item(FormUID), pVal, BubbleEvent)

                                Case mc_strUIDFormConfiguracionMSJ
                                    otmpForm = SBO_Application.Forms.Item(FormUID)
                                    m_oFormularioConfigNivelesAprob.ManejoEventosCombo(otmpForm, pVal, FormUID, BubbleEvent)

                                Case mc_strIdFormaCotizacion.ToString()

                                    Dim oForm As SAPbouiCOM.Form = SBO_Application.Forms.GetForm(mc_strIdFormaCotizacion, 0)

                                    Call m_oCotizacion.ManejadorEventoComboBox(oForm, pVal, BubbleEvent)

                                Case mc_strUniqueIDVSC
                                    m_oVehiculosACostear.ManejadroEventoCombo(FormUID, pVal, BubbleEvent)

                                Case g_strFormAsignacionMultiple
                                    m_oAsignacionMultiple.ManejadroEventoCombo(FormUID, pVal, BubbleEvent)
                                Case g_strConfMsj
                                    otmpForm = SBO_Application.Forms.Item(FormUID)
                                    m_oFormularioConfMsJ.ManejoEventosCombo(otmpForm, pVal, FormUID, BubbleEvent)
                                Case mc_strFormLstSolEsp
                                    Call m_oFormularioListadoSolicitudEspecificos.ManejadorEventoComboSelected(pVal, BubbleEvent)
                                Case mc_strUID_FORM_ReporteVehiculosRecurrentesTaller
                                    If Not m_oFormularioReporteVehiculosRecurrentesTaller Is Nothing Then
                                        m_oFormularioReporteVehiculosRecurrentesTaller.ManejadorEventoComboSelected(pVal, BubbleEvent)
                                    End If
                                Case mc_strUID_FORM_ReporteVentasXAsesorServicio
                                    If Not m_oFormularioReporteVentasXAsesorServicio Is Nothing Then
                                        m_oFormularioReporteVentasXAsesorServicio.ManejadorEventoComboSelected(pVal, BubbleEvent)
                                    End If
                            End Select

                        ElseIf pVal.BeforeAction Then

                            Select Case pVal.FormTypeEx


                                Case mc_strIdFormaCotizacion.ToString()
                                    'If pVal.ItemUID = "10000329" AndAlso Not m_oCotizacion.PermitirCancelar(SBO_Application.Forms.ActiveForm.UniqueID, BubbleEvent) Then
                                    '    BubbleEvent = False
                                    '    SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeCopiarACotizacion, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                    'End If
                                    If pVal.ItemUID = "10000329" Then
                                        Call m_oCotizacion.PermitirCancelar(SBO_Application.Forms.ActiveForm.UniqueID, BubbleEvent)
                                    End If

                                Case mc_strControlCVenta
                                    otmpForm = SBO_Application.Forms.Item(FormUID)
                                    'Agregado 22/10/2010: Guarda la moneda y el tipo de cambio antes de cambiar el combo box
                                    'If otmpForm.DataSources.DBDataSources.Item("@SCG_CVENTA").GetValue("U_Pre_Vta", 0) <> 0 Then
                                    If String.IsNullOrEmpty(m_oCVenta.strMonedaOrigen) And String.IsNullOrEmpty(m_oCVenta.strTipoCambioMoneda) Then  'And String.IsNullOrEmpty(m_oCVenta.strPrecioVentaOrigen) And String.IsNullOrEmpty(m_oCVenta.strPrecioAccsOrigen) Then
                                        m_oCVenta.strMonedaOrigen = otmpForm.DataSources.DBDataSources.Item("@SCGD_CVENTA").GetValue("U_Moneda", 0)
                                        m_oCVenta.strTipoCambioMoneda = otmpForm.DataSources.DBDataSources.Item("@SCGD_CVENTA").GetValue("U_SCGD_TipoCambio", 0)
                                        'm_oCVenta.strPrecioVentaOrigen = otmpForm.DataSources.DBDataSources.Item("@SCG_CVENTA").GetValue("U_Pre_Vta", 0)
                                        'm_oCVenta.strPrecioAccsOrigen = otmpForm.DataSources.DBDataSources.Item("@SCG_CVENTA").GetValue("U_Ext_Adi", 0)
                                    End If
                                    'Else
                                    'm_oCVenta.strMonedaOrigen = String.Empty
                                    'm_oCVenta.strTipoCambioMoneda = String.Empty
                                    'End If
                                Case mc_strUIDFormConfiguracionMSJ
                                    otmpForm = SBO_Application.Forms.Item(FormUID)
                                    m_oFormularioConfigNivelesAprob.ManejoEventosCombo(otmpForm, pVal, FormUID, BubbleEvent)

                                    'codigo para la captura del "Copia a" en el pedido de Clientes para la factura
                                    'de Clientes
                                Case "139"
                                    'otmpForm = SBO_Application.Forms.Item(FormUID)
                                    'Dim item As Item = otmpForm.Items.Item("10000329")
                                    'Dim combobox As ComboBox = CType(item.Specific, ComboBox)
                                    'Dim intFormQueLlama As String = pVal.FormTypeEx
                                    'Dim intNumeroDocumento As Integer = combobox.ValidValues.Item(pVal.PopUpIndicator).Description

                                    'If intNumeroDocumento = "13" Then
                                    '    blnUbicaciones = True

                                    '    'm_oFacturaClientes.AgregarUbicacionDefectoBodegaProceso()


                                    '    '

                                    'End If

                            End Select

                        End If

                    Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED

                        'If pVal.FormTypeEx = "149" AndAlso pVal.ActionSuccess Then

                        '    Dim form As Form = SBO_Application.Forms.Item(pVal.FormUID)
                        '    Dim item As Item = form.Items.Item("38")
                        '    Dim matrix As Matrix = CType(item.Specific, Matrix)

                        '    m_oCotizacion.FilaTieneNumeroOT(pVal, form)

                        'End If

                        'If pVal.ItemUID = "btnTrasl" And pVal.FormTypeEx = "SCGD_FormRequisicion" Then

                        '    m_oFormularioRequisiciones.ApplicationSBOOnItemEvent(FormUID, pVal, BubbleEvent)
                        'End If

                        'captura eventos del formulario 0
                        If pVal.FormTypeEx = "0" Then

                            Select Case pVal.ItemUID
                                Case "1"
                                    If DevolucionMercancia.EsDevolucionCompras Or NotaCreditoProveedor.EsNotaCredito Then
                                        CalculoCantidades.ControlDocumentos(True)
                                    End If

                                    'manejo para la seleccion "si"
                                    If m_oComprasEnVentas.CancelarOc Then

                                        'maneja el cancelar ordenes de compra ACTION SUCCESS
                                        m_oComprasEnVentas.BorraReferenciaOrdenConContVta()

                                    ElseIf m_oFacturaProveedores.CreaAsiento Then
                                        'm_oFacturaProveedores.CreaAsiento = False
                                        ''Finalizar transaccion para crear asiento 
                                        'm_oFacturaProveedores.FinalizaTransaccion()

                                    ElseIf m_oFacturaClientes.CreaAsiento Then
                                        'm_oFacturaClientes.CreaAsiento = False
                                        ''Finalizar transaccion para crear asiento 
                                        'm_oFacturaClientes.FinalizaTransaccion()

                                        '*********************************************************/
                                        '******************/Se comenta este codigo para evitar la duplicidad del llamado a estos metodos
                                        'ElseIf m_oCotizacion.blnValidarCamposHS_KM Then

                                        '    If Not m_oCotizacion.ValidarKilometraje_HorasServicio(BubbleEvent) Then

                                        '        BubbleEvent = False
                                        '    Else
                                        '        If Not String.IsNullOrEmpty(m_oCotizacion.strIDVehiculoHS_KM) Then
                                        '            m_oCotizacion.ActualizarDatosVehiculo(m_oCotizacion.strIDVehiculoHS_KM)
                                        '            m_oCotizacion.strIDVehiculoHS_KM = String.Empty
                                        '        End If
                                        '    End If
                                        '*********************************************************/

                                    End If

                                Case "2"
                                    If DevolucionMercancia.EsDevolucionCompras Or NotaCreditoProveedor.EsNotaCredito Then
                                        CalculoCantidades.ControlDocumentos(False)
                                    End If
                                    'manejo para el "no"
                                    'If m_oEntradaMercanciasEnCompras.CreaAsiento Then
                                    If m_oCotizacion.blnValidarCamposHS_KM Then
                                        m_oCotizacion.blnValidarCamposHS_KM = False
                                    End If
                                    ''Finalizar transaccion para crear asiento 
                                    'm_oEntradaMercanciasEnCompras.CreaAsiento = False
                                    'm_oEntradaMercanciasEnCompras.FinalizaTransaccion()

                                    'Else
                                    'If m_oFacturaClientes.CreaAsiento Then
                                    '    m_oFacturaClientes.CreaAsiento = False
                                    '    'Finalizar transaccion para crear asiento 
                                    '    m_oFacturaClientes.FinalizaTransaccion()
                                    'End If
                            End Select

                        End If

                        If pVal.FormTypeEx = "940" Then

                            Call m_oTransferenciaItems.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)
                            ' Call TransferenciaItems.ActualizaCotizacion()
                        End If

                        If pVal.FormTypeEx = mc_strUIListaContXUnidad Then

                            Call m_oListaCVXUnidad.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)

                        End If

                        'Agregado 28/09/2010 Maneja el evento del boton de Genera CV
                        If pVal.FormTypeEx = mc_strOportunidadVenta Then

                            Call m_oOportunidadVenta.ManejadorEventoItemPressed(pVal, BubbleEvent)

                        End If

                        'Agregado 05/11/2010: Manejo de item pressed de componentes de Salidas de Mercancia
                        If pVal.FormTypeEx = CStr(mc_strSalidaMercancia) Then

                            Call m_oSalidaMercancia.ManejadorEventoItemPress(pVal, BubbleEvent)

                        End If

                        'Agregado 26/06/2012: Manejo de item pressed de componentes de Pagos Recibidos
                        If pVal.FormTypeEx = CStr(mc_strPagoRecibido) Then

                            Call m_oPagoRecibido.ManejadorEventoItemPressed(pVal, BubbleEvent)

                        End If

                        'Agregado 13/12/2010: Manejo de item pressed de componentes de Entradas de Mercancia
                        If pVal.FormTypeEx = CStr(mc_strEntradaMercancia) Then

                            Call m_oEntradaMercancia.ManejadorEventoItemPressed(pVal, BubbleEvent)

                        End If

                        'Factura de proveedores - Validacion para el codigo de unidad

                        If pVal.ItemUID = "1" And pVal.FormTypeEx = "141" Then

                            Dim oRefItem As SAPbouiCOM.Item
                            Dim oMatrix As SAPbouiCOM.Matrix
                            Dim strValorMatriz As String
                            Dim intItemAcctCode As Integer

                            Dim xmlDocMatrix As Xml.XmlDocument
                            Dim XmlNode As Xml.XmlNode
                            Dim matrixXml As String
                            Dim blnMatrizServicio As Boolean = False
                            Dim contador As Integer = 0
                            Dim ListaInventarioTransito As Generic.IList(Of String)
                            Dim ListaInventarioStock As Generic.IList(Of String)

                            If pVal.BeforeAction Then


                                otmpForm = SBO_Application.Forms.GetForm(SBO_Application.Forms.ActiveForm.Type, SBO_Application.Forms.ActiveForm.TypeCount)

                                Dim DatatableCuentasInventarioTransito_Stock As System.Data.DataTable = Utilitarios.EjecutarConsultaDataTable("SELECT U_Transito, U_Stock FROM dbo.[@SCGD_ADMIN4]", m_oCompany.CompanyDB, m_oCompany.Server)

                                ListaInventarioTransito = New Generic.List(Of String)
                                ListaInventarioStock = New Generic.List(Of String)

                                For Each dr As System.Data.DataRow In DatatableCuentasInventarioTransito_Stock.Rows

                                    If Not ListaInventarioTransito.Contains(dr.Item("U_Transito").ToString.Trim) Then
                                        ListaInventarioTransito.Add(dr.Item("U_Transito").ToString.Trim)
                                    End If

                                    If Not ListaInventarioStock.Contains(dr.Item("U_Stock").ToString.Trim) Then
                                        ListaInventarioStock.Add(dr.Item("U_Stock").ToString.Trim)
                                    End If
                                Next


                                If Not otmpForm.Mode = BoFormMode.fm_FIND_MODE Then


                                    Dim cboCombo As SAPbouiCOM.ComboBox

                                    cboCombo = DirectCast(otmpForm.Items.Item("3").Specific, SAPbouiCOM.ComboBox)

                                    If cboCombo.Selected.Value = "S" Then
                                        blnMatrizServicio = True
                                        oRefItem = otmpForm.Items.Item(intMatrizServicio)
                                        strValorMatriz = intMatrizServicio
                                        intItemAcctCode = 2

                                    Else
                                        blnMatrizServicio = False
                                        oRefItem = otmpForm.Items.Item(intMatrizArticulos)
                                        strValorMatriz = intMatrizArticulos
                                        intItemAcctCode = 37
                                    End If

                                    oMatrix = DirectCast(oRefItem.Specific, Matrix)

                                    '******************************
                                    'oMatriz = oForm.Items.Item("38").Specific
                                    matrixXml = oMatrix.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

                                    xmlDocMatrix = New Xml.XmlDocument
                                    xmlDocMatrix.LoadXml(matrixXml)

                                    contador = 1

                                    For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")

                                        Dim elementoCodigoUnidad As Xml.XmlNode
                                        Dim elementoAcctCode As Xml.XmlNode
                                        Dim elementoCodigoTransaccion As Xml.XmlNode

                                        elementoCodigoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'U_SCGD_Cod_Unid']")

                                        If blnMatrizServicio Then
                                            elementoAcctCode = node.SelectSingleNode("Columns/Column/Value[../ID = '2']")
                                        Else
                                            elementoAcctCode = node.SelectSingleNode("Columns/Column/Value[../ID = '29']")
                                        End If

                                        elementoCodigoTransaccion = node.SelectSingleNode("Columns/Column/Value[../ID = 'U_SCGD_Cod_Tran']")

                                        Dim Unidad As String = elementoCodigoUnidad.InnerText.Trim
                                        Dim Transaccion As String = elementoCodigoTransaccion.InnerText.Trim
                                        Dim cuenta As String = elementoAcctCode.InnerText.Trim

                                        If ListaInventarioTransito.Contains(cuenta) Or ListaInventarioStock.Contains(cuenta) Then

                                            If ValidarCamposParaCuentaTransito(contador, strValorMatriz, otmpForm, cuenta, Unidad) = False Then
                                                BubbleEvent = False
                                                Exit Select
                                            End If

                                        Else
                                            If Not String.IsNullOrEmpty(Unidad) Or Not String.IsNullOrEmpty(Transaccion) Then

                                                If ValidarCamposParaCuentaTransito(contador, strValorMatriz, otmpForm, cuenta, Unidad) = False Then
                                                    BubbleEvent = False
                                                    Exit Select
                                                End If

                                            End If

                                        End If

                                        contador = contador + 1

                                    Next
                                End If
                            End If
                        End If

                        'Nota de debito de proveedores - Validacion para el codigo de unidad
                        If pVal.ItemUID = "1" And pVal.FormTypeEx = "65306" Then

                            Dim oRefItem As SAPbouiCOM.Item
                            Dim oMatrix As SAPbouiCOM.Matrix
                            Dim strValorMatriz As String
                            Dim cboCombo As SAPbouiCOM.ComboBox
                            Dim intItemAcctCode As Integer

                            Dim xmlDocMatrix As Xml.XmlDocument
                            Dim XmlNode As Xml.XmlNode
                            Dim matrixXml As String
                            Dim blnMatrizServicio As Boolean = False
                            Dim contador As Integer = 0

                            If pVal.BeforeAction Then


                                otmpForm = SBO_Application.Forms.GetForm(SBO_Application.Forms.ActiveForm.Type, SBO_Application.Forms.ActiveForm.TypeCount)


                                If Not otmpForm.Mode = BoFormMode.fm_FIND_MODE Then


                                    cboCombo = DirectCast(otmpForm.Items.Item("3").Specific, SAPbouiCOM.ComboBox)

                                    If cboCombo.Selected.Value = "S" Then
                                        blnMatrizServicio = True
                                        oRefItem = otmpForm.Items.Item(intMatrizServicio)
                                        strValorMatriz = intMatrizServicio
                                        intItemAcctCode = 2
                                    Else
                                        blnMatrizServicio = False
                                        oRefItem = otmpForm.Items.Item(intMatrizArticulos)
                                        strValorMatriz = intMatrizArticulos
                                        intItemAcctCode = 37
                                    End If

                                    oMatrix = DirectCast(oRefItem.Specific, Matrix)

                                    '******************************
                                    'oMatriz = oForm.Items.Item("38").Specific
                                    matrixXml = oMatrix.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

                                    xmlDocMatrix = New Xml.XmlDocument
                                    xmlDocMatrix.LoadXml(matrixXml)

                                    contador = 1

                                    For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")

                                        Dim elementoCodigoUnidad As Xml.XmlNode
                                        Dim elementoAcctCode As Xml.XmlNode
                                        Dim elementoCodigoTransaccion As Xml.XmlNode

                                        elementoCodigoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'U_SCGD_Cod_Unid']")

                                        If blnMatrizServicio Then
                                            elementoAcctCode = node.SelectSingleNode("Columns/Column/Value[../ID = '2']")
                                        Else
                                            elementoAcctCode = node.SelectSingleNode("Columns/Column/Value[../ID = '29']")
                                        End If

                                        elementoCodigoTransaccion = node.SelectSingleNode("Columns/Column/Value[../ID = 'U_SCGD_Cod_Tran']")

                                        Dim Unidad As String = elementoCodigoUnidad.InnerText.Trim
                                        Dim Transaccion As String = elementoCodigoTransaccion.InnerText.Trim
                                        Dim cuenta As String = elementoAcctCode.InnerText.Trim

                                        If Not String.IsNullOrEmpty(Unidad) Or Not String.IsNullOrEmpty(cuenta) Or Not String.IsNullOrEmpty(Transaccion) Then

                                            If ValidarCamposParaCuentaTransito(contador, strValorMatriz, otmpForm, cuenta, Unidad) = False Then
                                                BubbleEvent = False
                                                Exit Select
                                            End If

                                        End If

                                        contador = contador + 1

                                    Next




                                    'For i As Integer = 1 To oMatrix.RowCount

                                    '    'Agregado 14072010
                                    '    editCell = DirectCast(oMatrix.Columns.Item(intItemAcctCode).Cells.Item(i).Specific, EditText)
                                    '    Dim valorFormatCode As String = editCell.String
                                    '    editCell = DirectCast(oMatrix.Columns.Item("U_SCGD_Cod_Unid").Cells.Item(i).Specific, EditText)
                                    '    Dim valorCodUnidad As String = editCell.String
                                    '    'Dim valorFormatCode As String = oMatrix.Columns.Item(intItemAcctCode).Cells.Item(i).Specific.string()
                                    '    'Dim valorCodUnidad As String = oMatrix.Columns.Item("U_Cod_Unid").Cells.Item(i).Specific.string()

                                    '    If ValidarCamposParaCuentaTransito(i, strValorMatriz, otmpForm, valorFormatCode, valorCodUnidad) = False Then
                                    '        BubbleEvent = False
                                    '        Exit Select
                                    '    End If

                                    'Next i
                                    '
                                End If
                            End If
                        End If

                        If pVal.FormTypeEx = "181" Then
                            m_oNotaCreditoProveedor.ItemPress(pVal, FormUID, BubbleEvent)
                        End If

                        'Nota de credito - Validacion para el codigo de unidad
                        If pVal.ItemUID = "1" And pVal.FormTypeEx = "181" Then

                            Dim oRefItem As SAPbouiCOM.Item
                            Dim oMatrix As SAPbouiCOM.Matrix
                            Dim strValorMatriz As String
                            Dim cboCombo As SAPbouiCOM.ComboBox
                            Dim intItemAcctCode As Integer

                            Dim xmlDocMatrix As Xml.XmlDocument
                            Dim XmlNode As Xml.XmlNode
                            Dim matrixXml As String
                            Dim blnMatrizServicio As Boolean = False
                            Dim contador As Integer = 0

                            'If pVal.ActionSuccess Then
                            If pVal.BeforeAction Then



                                otmpForm = SBO_Application.Forms.GetForm(SBO_Application.Forms.ActiveForm.Type, SBO_Application.Forms.ActiveForm.TypeCount)

                                If Not otmpForm.Mode = BoFormMode.fm_FIND_MODE Then


                                    cboCombo = DirectCast(otmpForm.Items.Item("3").Specific, SAPbouiCOM.ComboBox)

                                    If cboCombo.Selected.Value = "S" Then
                                        blnMatrizServicio = True
                                        oRefItem = otmpForm.Items.Item(intMatrizServicio)
                                        strValorMatriz = intMatrizServicio
                                        intItemAcctCode = 2

                                    Else
                                        blnMatrizServicio = False
                                        oRefItem = otmpForm.Items.Item(intMatrizArticulos)
                                        strValorMatriz = intMatrizArticulos
                                        intItemAcctCode = 37
                                    End If

                                    oMatrix = DirectCast(oRefItem.Specific, Matrix)

                                    '******************************
                                    'oMatriz = oForm.Items.Item("38").Specific
                                    matrixXml = oMatrix.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

                                    xmlDocMatrix = New Xml.XmlDocument
                                    xmlDocMatrix.LoadXml(matrixXml)

                                    contador = 1

                                    For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")

                                        Dim elementoCodigoUnidad As Xml.XmlNode
                                        Dim elementoAcctCode As Xml.XmlNode
                                        Dim elementoCodigoTransaccion As Xml.XmlNode

                                        elementoCodigoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'U_SCGD_Cod_Unid']")

                                        If blnMatrizServicio Then
                                            elementoAcctCode = node.SelectSingleNode("Columns/Column/Value[../ID = '2']")
                                        Else
                                            elementoAcctCode = node.SelectSingleNode("Columns/Column/Value[../ID = '29']")
                                        End If

                                        elementoCodigoTransaccion = node.SelectSingleNode("Columns/Column/Value[../ID = 'U_SCGD_Cod_Tran']")

                                        Dim Unidad As String = elementoCodigoUnidad.InnerText.Trim
                                        Dim Transaccion As String = elementoCodigoTransaccion.InnerText.Trim
                                        Dim cuenta As String = elementoAcctCode.InnerText.Trim

                                        If Not String.IsNullOrEmpty(Unidad) Or Not String.IsNullOrEmpty(cuenta) Or Not String.IsNullOrEmpty(Transaccion) Then

                                            If ValidarCamposParaCuentaTransito(contador, strValorMatriz, otmpForm, cuenta, Unidad) = False Then
                                                BubbleEvent = False
                                                Exit Select
                                            End If

                                        End If

                                        contador = contador + 1

                                    Next

                                    'For i As Integer = 1 To oMatrix.RowCount

                                    '    'Agregado 14072010
                                    '    editCell = DirectCast(oMatrix.Columns.Item(intItemAcctCode).Cells.Item(i).Specific, EditText)
                                    '    Dim valorFormatCode As String = editCell.String
                                    '    editCell = DirectCast(oMatrix.Columns.Item("U_SCGD_Cod_Unid").Cells.Item(i).Specific, EditText)
                                    '    Dim valorCodUnidad As String = editCell.String
                                    '    'Dim valorFormatCode As String = oMatrix.Columns.Item(intItemAcctCode).Cells.Item(i).Specific.string()
                                    '    'Dim valorCodUnidad As String = oMatrix.Columns.Item("U_Cod_Unid").Cells.Item(i).Specific.string()

                                    '    If ValidarCamposParaCuentaTransito(i, strValorMatriz, otmpForm, valorFormatCode, valorCodUnidad) = False Then
                                    '        BubbleEvent = False
                                    '        Exit Select
                                    '    End If

                                    'Next i
                                End If
                            End If
                        End If

                        'Registro de diario (Asiento) - Validacion del codigo de unidad
                        If pVal.ItemUID = "1" And pVal.FormTypeEx = "392" And pVal.BeforeAction Then

                            Dim oRefItem As SAPbouiCOM.Item
                            Dim oMatrix As SAPbouiCOM.Matrix
                            'Dim intColumnFormatCode As Integer
                            Dim strValorMatriz As String

                            otmpForm = SBO_Application.Forms.GetForm(SBO_Application.Forms.ActiveForm.Type, SBO_Application.Forms.ActiveForm.TypeCount)

                            If otmpForm.Mode = BoFormMode.fm_ADD_MODE Or otmpForm.Mode = BoFormMode.fm_EDIT_MODE Or otmpForm.Mode = BoFormMode.fm_UPDATE_MODE Then


                                Dim intValidar As Integer = CInt(otmpForm.DataSources.DBDataSources.Item("OJDT").GetValue("U_SCGD_AplVal", 0))

                                If intValidar = 1 Then
                                    Try
                                        oRefItem = otmpForm.Items.Item(intMatrizAsiento)

                                        strValorMatriz = intMatrizAsiento

                                        oMatrix = DirectCast(oRefItem.Specific, Matrix)

                                        For i As Integer = 1 To oMatrix.RowCount

                                            'Agregado 14072010
                                            editCell = DirectCast(oMatrix.Columns.Item(34).Cells.Item(i).Specific, EditText)
                                            Dim valorFormatCode As String = editCell.String
                                            editCell = DirectCast(oMatrix.Columns.Item("U_SCGD_Cod_Unidad").Cells.Item(i).Specific, EditText)
                                            Dim valorCodUnidad As String = editCell.String
                                            If ValidarCamposParaCuentaTransito(i, strValorMatriz, otmpForm, valorFormatCode, valorCodUnidad) = False Then
                                                BubbleEvent = False
                                                Exit Select
                                            End If
                                        Next i
                                    Catch ex As Exception
                                    End Try
                                End If
                            End If

                        End If

                        If pVal.FormTypeEx = FormularioLLamadaServicioSBO.FormType AndAlso m_blnUsaOrdenesDeTrabajo Then
                            m_oLlamadaServicio.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strUIFacturasInt Then

                            Call m_oFacturaInterna.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strUIGOODISSUE Then
                            Call m_oGoodIssue.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)
                        End If


                        If pVal.FormTypeEx = mc_strControlCVenta AndAlso pVal.ActionSuccess AndAlso pVal.ItemUID = "lk_Entrada" Then

                            strIDContrato = m_oCVenta.IDEntradaMercancia().ToString()
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto(mc_strUIGOODENT, False) Then
                                Call m_oGoodReceive.CargaFormularioGoodReceive("", "", "", "", "", "", strIDContrato, "", "")
                            End If
                        End If

                        If pVal.FormTypeEx = mc_strControlCVenta AndAlso pVal.ActionSuccess AndAlso pVal.ItemUID = "lkPrestamo" Then

                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioPrestamo, activarSiEstaAbierto:=True) Then

                                Dim strPrestamo As String
                                Dim oform As SAPbouiCOM.Form

                                oform = SBO_Application.Forms.Item(pVal.FormUID)

                                m_oFormularioPrestamo.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioPrestamo)

                                strPrestamo = oform.DataSources.DBDataSources.Item("@SCGD_CVENTA").GetValue("U_Prestamo", 0).Trim()

                                'Carga Préstamo desde el Contrato de Ventas.
                                m_oFormularioPrestamo.CargarPrestamo(strPrestamo)

                            End If

                        End If

                        If Not pVal.BeforeAction _
                          AndAlso (pVal.ItemUID = "btnGenerar" Or pVal.ItemUID = "btnCancel" Or pVal.ItemUID = "btnAct") _
                          AndAlso pVal.FormTypeEx = mc_stridGeneraOV Then

                            m_oCotizacion.ManejadorEventoItemPressedGenOV(FormUID, pVal, BubbleEvent)

                        End If

                        If Not pVal.BeforeAction _
                            AndAlso (pVal.ItemUID = "btnGenerar" Or pVal.ItemUID = "btnCancel" Or pVal.ItemUID = "btnAct") _
                            AndAlso pVal.FormTypeEx = "SCGD_REAOT" Then
                            ReAperturaOTNormal.ItemEvent(FormUID, pVal, BubbleEvent)
                        End If

                        If Not pVal.BeforeAction _
                         AndAlso (pVal.ItemUID = "btnGenerar" Or pVal.ItemUID = "btnCancel" Or pVal.ItemUID = "btnAct") _
                         AndAlso pVal.FormTypeEx = mc_strGeneraFI Then

                            m_oCotizacion.ManejadorEventoItemPressedGenFI(FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strUIGOODISSUE AndAlso pVal.ActionSuccess AndAlso pVal.ItemUID = "38" Then

                            strIDContrato = m_oGoodIssue.DevolverIDEntrada(pVal.FormTypeEx)
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto(mc_strUIGOODENT, False) Then
                                Call m_oGoodReceive.CargaFormularioGoodReceive("", "", "", "", "", "", strIDContrato, "", "")
                                ' Call m_oCVenta.CargarContrato(strIDContrato, mc_strUIGOODENT)
                            End If

                        End If

                        If pVal.FormTypeEx = mc_strUIGOODISSUE AndAlso pVal.ActionSuccess AndAlso pVal.ItemUID = "37" Then

                            strIDContrato = m_oGoodIssue.DevolverIDContrato(pVal.FormTypeEx)
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_frmContVent", False) Then
                                Call m_oCVenta.DibujarFormularioContratoVentas("", False)
                                Call m_oCVenta.CargarContrato(strIDContrato, "SCGD_frmContVent")
                                Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item("SCGD_frmContVent"), False)
                            End If

                            m_oCVenta.m_blnCargoManejarEstados = False

                        ElseIf pVal.FormTypeEx = mc_strUIGOODISSUE AndAlso pVal.BeforeAction AndAlso pVal.ItemUID = "37" Then

                            m_oCVenta.m_blnCargoManejarEstados = True

                        End If

                        If Not pVal.BeforeAction _
                        AndAlso (pVal.ItemUID = "1") _
                        AndAlso pVal.FormTypeEx = mc_strUIGOODENT AndAlso pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then

                            m_oGoodReceive.CrearAsientos(pVal, BubbleEvent)

                        End If

                        If Not pVal.BeforeAction _
                        AndAlso (pVal.ItemUID = "btn_Genera") _
                        AndAlso pVal.FormTypeEx = mc_strUIGOODENT Then

                            m_oGoodReceive.CrearAsientos(pVal, BubbleEvent)

                        End If

                        If pVal.BeforeAction _
                        AndAlso (pVal.ItemUID = "1") _
                        AndAlso pVal.FormTypeEx = mc_strUIGOODENT AndAlso pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If SBO_Application.MessageBox(My.Resources.Resource.NopuedeModificar, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                                BubbleEvent = False
                            Else
                                m_oGoodReceive.ValidarTipoCambio(BubbleEvent)
                            End If

                        End If

                        If Not pVal.BeforeAction _
                          AndAlso (pVal.ItemUID = "btnRefresh" Or pVal.ItemUID = "btnClose") _
                          AndAlso pVal.FormTypeEx = mc_strUniqueIDBCV Then

                            m_oBuscadorCV.ManejadorEventoItemPressedBCV(FormUID, pVal, BubbleEvent)

                        End If

                        If Not pVal.BeforeAction _
                          AndAlso (pVal.ItemUID = "btnRefresh" Or pVal.ItemUID = "btnClose") _
                          AndAlso pVal.FormTypeEx = mc_strUniqueIDLCV Then

                            m_oListadoCV.ManejadorEventoItemPressedBCV(FormUID, pVal, BubbleEvent)

                        End If

                        '****************************************************************************************************************
                        If Not pVal.BeforeAction _
                       AndAlso (pVal.ItemUID = "btnRefresh" Or pVal.ItemUID = "btnClose") _
                       AndAlso pVal.FormTypeEx = mc_strUniqueIDLCVR Then

                            m_oListadoContratosReversados.ManejadorEventoItemPressedBCV(FormUID, pVal, BubbleEvent)

                        End If
                        '****************************************************************************************************************

                        If Not pVal.BeforeAction _
                          AndAlso (pVal.ItemUID = "btnActuali" Or pVal.ItemUID = "btnCerrar" Or pVal.ItemUID = "chkFac") _
                          AndAlso pVal.FormTypeEx = mc_strUniqueIDVSC Then

                            m_oVehiculosACostear.ManejadorEventoItemPressedBCV(FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strUniqueIDCosteoMultiplesUnidades Then

                            m_oCosteoMultiplesUnidades.ManejadorEventoItemPressedBCV(FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strUniqueIDSalidaMultiplesUnidades Then

                            m_oSalidasMultiplesUnidades.ManejadorEventoItemPressedBCV(FormUID, pVal, BubbleEvent)

                        End If


                        If Not pVal.BeforeAction _
                          AndAlso (pVal.ItemUID = "btnRefresh" Or pVal.ItemUID = "btnCerrar") _
                          AndAlso pVal.FormTypeEx = mc_strUILISTADOGR Then

                            m_oListadoGR.ManejadorEventoItemPressed(FormUID, pVal, BubbleEvent)

                        End If

                        If Not pVal.BeforeAction _
                          AndAlso (pVal.ItemUID = "btnRefresh" Or pVal.ItemUID = "btnCerrar") _
                          AndAlso pVal.FormTypeEx = mc_strUIRecosteos Then

                            m_oRecosteos.ManejadorEventoItemPressed(FormUID, pVal, BubbleEvent)

                        End If

                        If Not pVal.BeforeAction _
                          AndAlso (pVal.ItemUID = "btnCostear") _
                          AndAlso pVal.FormTypeEx = mc_strUILISTADOGR Then

                            Dim strGoodReceipt As String

                            If Not ValidarSiFormularioAbierto(mc_strUIGOODISSUE, False) Then

                                strGoodReceipt = m_oListadoGR.DevolverDatoGoodReceipt(pVal.FormUID)
                                If Not String.IsNullOrEmpty(strGoodReceipt) Then
                                    Call m_oGoodIssue.CargaFormularioGoodIssue(strGoodReceipt)
                                    SBO_Application.Forms.Item(mc_strUILISTADOGR).Close()
                                End If
                            End If

                        End If

                        If Not pVal.BeforeAction _
                        AndAlso pVal.ItemUID = "btnCostear" _
                        AndAlso pVal.FormTypeEx = mc_strUIRecosteos Then
                            Dim strUnidad As String = ""
                            Dim strVIN As String = ""
                            Dim strMarca As String = ""
                            Dim strModelo As String = ""
                            Dim strEstilo As String = ""
                            Dim strIDVehiculo As String = ""
                            Dim strDocRecepcion As String = ""
                            Dim strDocPedido As String = ""

                            If Not ValidarSiFormularioAbierto(mc_strUIGOODENT, False) Then

                                m_oRecosteos.DevolverDatosVehiculo(strUnidad, strVIN, strMarca, strEstilo, strModelo, pVal.FormUID, strIDVehiculo, strDocRecepcion, strDocPedido)
                                If Not String.IsNullOrEmpty(strIDVehiculo) Then
                                    Call m_oGoodReceive.CargaFormularioGoodReceive(strUnidad, strVIN, strMarca, strEstilo, strModelo, strIDVehiculo, "", strDocRecepcion, strDocPedido)
                                    SBO_Application.Forms.Item(mc_strUIRecosteos).Close()
                                End If
                            End If
                        End If

                        If Not pVal.BeforeAction _
                        AndAlso pVal.ItemUID = "btnCostear" _
                        AndAlso pVal.FormTypeEx = mc_strUniqueIDVSC Then
                            Dim strUnidad As String = ""
                            Dim strVIN As String = ""
                            Dim strMarca As String = ""
                            Dim strModelo As String = ""
                            Dim strEstilo As String = ""
                            Dim strIDVehiculo As String = ""
                            Dim strDocRecepcion As String = ""
                            Dim strDocPedido As String = ""

                            If Not ValidarSiFormularioAbierto(mc_strUIGOODENT, False) Then

                                m_oVehiculosACostear.DevolverDatosVehiculo(strUnidad, strVIN, strMarca, strEstilo, strModelo, pVal.FormUID, strIDVehiculo, strDocRecepcion, strDocPedido)
                                If Not String.IsNullOrEmpty(strIDVehiculo) Then
                                    Call m_oGoodReceive.CargaFormularioGoodReceive(strUnidad, strVIN, strMarca, strEstilo, strModelo, strIDVehiculo, "", strDocRecepcion, strDocPedido)
                                    SBO_Application.Forms.Item(mc_strUniqueIDVSC).Close()
                                End If
                            End If
                        End If

                        If Not pVal.BeforeAction _
                     AndAlso pVal.ItemUID = "btnCU" _
                     AndAlso pVal.FormTypeEx = mc_strUniqueIDCosteoMultiplesUnidades Then

                            Dim oMatrix As SAPbouiCOM.Matrix
                            Dim form As SAPbouiCOM.Form = SBO_Application.Forms.Item(mc_strUniqueIDCosteoMultiplesUnidades)
                            oMatrix = DirectCast(form.Items.Item("mtx_VehSin").Specific, SAPbouiCOM.Matrix)
                            m_oCosteoMultiplesUnidades.EntradasMultiples(form, oMatrix, pVal)

                        End If

                        If Not pVal.BeforeAction _
                   AndAlso pVal.ItemUID = "btnCU" _
                   AndAlso pVal.FormTypeEx = mc_strUniqueIDSalidaMultiplesUnidades Then

                            Dim oMatrix As SAPbouiCOM.Matrix
                            Dim form As SAPbouiCOM.Form = SBO_Application.Forms.Item(mc_strUniqueIDSalidaMultiplesUnidades)
                            oMatrix = DirectCast(form.Items.Item("mtx_Recost").Specific, SAPbouiCOM.Matrix)
                            m_oSalidasMultiplesUnidades.SalidasMultiples(form, oMatrix, pVal)

                        End If

                        If pVal.FormTypeEx = mc_strUIFacturasInt AndAlso pVal.ActionSuccess AndAlso pVal.ItemUID = "44" Then

                            strIDContrato = m_oFacturaInterna.DevolverIDVehiculo(pVal.FormUID)
                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then
                                Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                          strIDContrato, _
                                                          True, _
                                                          "", _
                                                          0, True, False, VehiculosCls.ModoFormulario.scgTaller)
                            End If


                        End If

                        If pVal.BeforeAction _
                            AndAlso (pVal.ItemUID = "1" Or pVal.ItemUID = "2") _
                            AndAlso pVal.FormTypeEx = m_strUIDVehiMarcaEtc Then

                            m_oCFLbyFS.ManejadorEventoItemPressedCFLbyFS(FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strControlCVenta _
                            AndAlso pVal.FormUID = mc_strUniqueIDCV _
                            AndAlso Not pVal.BeforeAction Then

                            otmpForm = SBO_Application.Forms.Item(FormUID)
                            Call m_oCVenta.ManejoEventosTab(otmpForm, pVal)

                        End If

                        'Manejo tabs en Inventario Vehiculos
                        If pVal.FormTypeEx = mc_strUniqueIDInventariovehiculos _
                            AndAlso Not pVal.BeforeAction Then

                            otmpForm = SBO_Application.Forms.Item(FormUID)
                            Call m_oInventarioVehiculos.ManejoEventosTab(otmpForm, pVal)
                        End If

                        'manejo de TABs en CosteoMultiple
                        If pVal.FormTypeEx = mc_strUniqueIDCosteoMultiplesUnidades _
                          AndAlso pVal.FormUID = mc_strUniqueIDCosteoMultiplesUnidades _
                          AndAlso Not pVal.BeforeAction Then

                            otmpForm = SBO_Application.Forms.Item(FormUID)
                            Call m_oCosteoMultiplesUnidades.ManejoEventosTab(otmpForm, pVal)

                        End If

                        If pVal.FormTypeEx = mc_strIdFormaCotizacion Then

                            If pVal.ItemUID = "SCGD_LKOT" Then
                                If (m_oFormularioOrdenTrabajo IsNot Nothing) Then
                                    If Not oGestorFormularios.FormularioAbierto(m_oFormularioOrdenTrabajo, activarSiEstaAbierto:=True) Then
                                        m_oFormularioOrdenTrabajo.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioOrdenTrabajo)
                                    End If
                                    m_oRecepcionVHUI.ManejadorEventoLinkPress(pVal, BubbleEvent, m_oFormularioOrdenTrabajo)
                                End If
                            End If

                            Call m_oRecepcionVHUI.ManejadorEventoItemPressed(FormUID, pVal, BubbleEvent, m_oVehiculos, strTypeVehiculo, intTypeCountVehiculo)

                            m_oCotizacion.DocNumNuevo = m_strDocEntryByStatusBar
                            m_strDocEntryByStatusBar = ""
                            If DMS_Connector.Configuracion.ParamGenAddon.U_OT_SAP = "Y" Then
                                Call m_oCotizacion_ProcesaOT.ManejadorEventoItemPressed_TallerSAP(FormUID, pVal, BubbleEvent)
                            Else
                                Call m_oCotizacion.ManejadorEventoItemPressed_TallerExterno(FormUID, pVal, BubbleEvent)
                            End If

                        End If



                        'manejo de item pressed en Campañas
                        If pVal.FormTypeEx = mc_strIdFormCampaña Then
                            m_oCampana.ManejadorEventoItemPressed(FormUID, pVal, BubbleEvent)
                        End If

                        'IMPRIMIR FICHA VEHÍCULO DESDE PLAN DE VENTAS
                        If pVal.FormTypeEx = mc_strControlCVenta _
                            AndAlso pVal.ItemUID = "btnFicha" _
                            AndAlso Not pVal.BeforeAction _
                            AndAlso pVal.FormMode <> 3 Then
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ImprimirFicha, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                            Call m_oCVenta.ImprimirReporte(SBO_Application.Forms.ActiveForm, My.Resources.Resource.rptFichaVehículo + ".rpt", My.Resources.Resource.FichaVehículo, False, False, True)
                            'SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizado, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        End If


                        'IMPRIMIR FICHA DE VEHÍCULO
                        If pVal.FormTypeEx = mc_strControlVehiculo AndAlso pVal.ItemUID = "btnFicha" AndAlso pVal.ActionSuccess Then
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ImprimirFicha, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                            Call m_oVehiculos.ImprimirFichaVehículo(FormUID, pVal, BubbleEvent)
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizado, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        ElseIf pVal.FormTypeEx = mc_strControlVehiculo Then
                            m_oVehiculos.ManejadorEventoItemPressed(FormUID, pVal, BubbleEvent, m_blnUsaCosteoVehículo)
                        End If




                        'IMPRIMIR FICHA DE VEHÍCULO
                        If pVal.FormTypeEx = mc_strUIGOODENT AndAlso pVal.ItemUID = "Print" AndAlso pVal.ActionSuccess Then
                            Call m_oGoodReceive.ImprimirReporteCostoVehiculo(FormUID, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strControlVehiculo AndAlso pVal.ItemUID = "del" AndAlso pVal.ActionSuccess Then

                            Call m_oVehiculos.EliminarComponente(FormUID)

                        End If

                        If pVal.FormTypeEx = mc_strControlCVenta AndAlso pVal.ItemUID = "del" AndAlso pVal.ActionSuccess Then

                            Call m_oCVenta.EliminarItem(FormUID, 1)

                        End If

                        If pVal.ActionSuccess AndAlso pVal.FormTypeEx = mc_strControlCVenta _
                        AndAlso (pVal.ItemUID = "lkVeh_Nue" Or pVal.ItemUID = "lkVehUs") _
                        AndAlso Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then

                            otmpForm = SBO_Application.Forms.ActiveForm
                            If pVal.ItemUID = "lkVeh_Nue" Then
                                'Agregado 14072010
                                editCell = DirectCast(otmpForm.Items.Item("txtIDVehi").Specific, EditText)
                                Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                             editCell.String, _
                                                             True, _
                                                             "", _
                                                             0, True, False, VehiculosCls.ModoFormulario.scgVentas)
                            ElseIf pVal.ItemUID = "lkVehUs" Then
                                'Agregado 14072010
                                editCell = DirectCast(otmpForm.Items.Item("txtIDV_Us").Specific, EditText)
                                Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                             editCell.String, _
                                                             True, _
                                                             "", _
                                                             0, True, False, VehiculosCls.ModoFormulario.scgVentas)
                            End If
                            otmpForm = Nothing
                        End If

                        If pVal.FormTypeEx = mc_strControlCVenta Then

                            Call m_oCVenta.ManejadorEventoItemPress(pVal, pVal.FormUID, BubbleEvent)

                        End If

                        'manejo de eventos para Entradas de Mercancia en Compras
                        If pVal.FormTypeEx = mc_strEntradadeMercancia Then

                            Call m_oEntradaMercanciasEnCompras.ManejadorEventoItemPress(pVal, pVal.FormUID, BubbleEvent)
                            'Call m_oEntradaMercanciasEnCompras.ManejadorEventoMenu(pVal, BubbleEvent)
                        End If
                        'manejo de eventos para cierre entrada mercancia
                        If pVal.FormTypeEx = "1250000000" Then
                            Call m_oEntradaMercanciasEnCompras.ManejadorEventoItemPressCierre(pVal, pVal.FormUID, BubbleEvent)
                        End If
                        'manejo de eventos para Factura proveedores en Compras
                        If pVal.FormTypeEx = mc_strFacturadeCompra Then

                            Call m_oFacturaProveedores.ManejadorEventoItemPress(pVal, pVal.FormUID, BubbleEvent)

                        End If

                        'manejo de eventos para Devolucion de Mercancia
                        If pVal.FormTypeEx = "182" Then

                            Call m_oDevolucionMercancia.ManejadorEventoItemPress(pVal, pVal.FormUID, BubbleEvent)

                        End If


                        'manejo de eventos para Factura cliente en Vetnas
                        If pVal.FormTypeEx = mc_strFacturaCliente Or pVal.FormTypeEx = mc_strBoleta Then

                            Call m_oFacturaClientes.ManejadorEventoItemPress(pVal, pVal.FormUID, BubbleEvent)

                        End If

                        'Manejo de eventos para balances
                        If pVal.FormTypeEx = mc_FormBalance Then
                            Call m_FormularioBalance.ManejadorEventoItemPress(pVal, pVal.FormUID, BubbleEvent, SBO_Application, m_oCompany)
                            Call m_FormularioBalance.ManejadorEventoLostFocus(pVal, pVal.FormUID, BubbleEvent, SBO_Application, m_oCompany)
                        End If

                        'Agregado 39/08/2012: Manejo MENSAJERIA APROBACION
                        If pVal.FormTypeEx = mc_strUIDFormConfiguracionMSJ Then
                            Call m_oFormularioConfigNivelesAprob.ManejadorEventoItemPress(pVal, FormUID, BubbleEvent, m_oCompany, DBUser, DBPassword)
                        End If

                        'manejo de itemevent para vendedores por tipo inevntario
                        If pVal.FormTypeEx = mc_strFormVendedoresTipoInv Then
                            Call m_oFormularioPermisosVendedoresXTI.ManejadorEventoItemPress(pVal, FormUID, BubbleEvent, m_oCompany)
                        End If

                        'Agregado 39/08/2012: Manejo unidades por nivel
                        If pVal.FormTypeEx = mc_strFormUnidadesPorNivel Then
                            Call m_oUsuariosxNivel.ManejadorEventoItemPress(pVal, FormUID, BubbleEvent, m_oCompany)
                        End If

                        ''''***********para traslado de costos entre unidades********************
                        If pVal.FormTypeEx = mc_strUITrasC Then

                            Call m_oTrasladoCostos.ManejadorEventoItemPress(pVal, pVal.FormUID, BubbleEvent)

                        End If
                        '**************************************************
                        If pVal.FormTypeEx = mc_strUniqueIDContRevertidos Then

                            Call m_oListadoContratosReversados.ManejadorEventoItemPressedBCV(pVal.FormUID, pVal, BubbleEvent)

                        End If

                        '**************************************************

                        '**************************************************
                        If pVal.FormTypeEx = mc_strUniqueIDListaARevertir Then
                            Call m_oListaContratos_a_Reversar.ManejadorEventoItemPressedBCV(pVal.FormUID, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strUniqueIDConSegPV Then
                            Call m_oListaContratosSegPV.ManejadorEventoItemPressedBCV(pVal.FormUID, pVal, BubbleEvent)
                        End If

                        '**************************************************


                        If pVal.FormTypeEx = mc_strUniqueIDNivelesPV Then

                            m_oNivelesPV.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strUniqueIDLineasFactura Then

                            Call m_oLineasFactura.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)

                        End If


                        If pVal.FormTypeEx = mc_strUniqueIDPropiedades Then

                            Call m_oPropiedades.ManejadorEventoItemPressedBCV(pVal.FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strUniqueIDInventariovehiculos Then

                            Call m_oInventarioVehiculos.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strUniqueIDConfiguracionesGenerales Then

                            Call m_oConfiguracionGeneral.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strUniqueIDReportesCosteo Then

                            Call m_oReportesCosteo.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strFORM_EstadosOT Then

                            Call m_oEstadosOT.ManejadorEventosItemPressed(pVal.FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strUIGOODENT Then

                            Call m_oGoodReceive.ManejadorEventoItemPressedBCV(pVal.FormUID, pVal, BubbleEvent)

                        End If

                        '***************************************
                        'solicitud de OT Especial - Agregado 11-12-2013
                        If pVal.FormTypeEx = mc_strSolicitudOTEspecial Then

                            Call m_oSolicitudOTEspecial.ManejadorEventoItemPressedBCV(pVal.FormUID, pVal, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = mc_strSolicitudOTEspecial AndAlso pVal.ActionSuccess = True AndAlso pVal.BeforeAction = False And pVal.ItemUID = "btnVeh" Then
                            Dim oForm As SAPbouiCOM.Form
                            Dim strUnidad As String
                            Dim strIDVeh As String
                            Dim strNumeroCV As String

                            oForm = SBO_Application.Forms.Item(FormUID)
                            m_oVehiculos = New VehiculosCls(m_oCompany, SBO_Application)

                            strUnidad = oForm.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Cod_Uni", 0)
                            strUnidad = strUnidad.Trim()
                            strIDVeh = oForm.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Id_Vehi", 0)
                            strIDVeh = strIDVeh.Trim

                            Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                                         strIDVeh, _
                                                         True, _
                                                         "", _
                                                         0, True, False, VehiculosCls.ModoFormulario.scgTaller)




                        End If
                        '***************************************


                        If pVal.FormTypeEx = mc_strControlVehiculo _
                            AndAlso pVal.FormUID = mc_strUniqueID _
                            AndAlso Not pVal.BeforeAction Then

                            otmpForm = SBO_Application.Forms.Item(FormUID)
                            Call m_oVehiculos.ManejoEventosTab(otmpForm, pVal)

                        End If

                        If pVal.FormTypeEx = mc_strUIGOODENT _
                                                    AndAlso pVal.FormUID = mc_strUIGOODENT _
                                                    AndAlso Not pVal.BeforeAction Then

                            otmpForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                            Call m_oGoodReceive.ManejoEventosTab(otmpForm, pVal)

                        End If

                        '****************************************************

                        ' If pVal.ItemUID = mc_strIDBotonEjecucion _
                        'AndAlso pVal.FormTypeEx = mc_strControlVehiculo _
                        'AndAlso pVal.BeforeAction AndAlso Not pVal.ActionSuccess Then

                        '     'Este es codigo de pruebas luego lo acomodo
                        '     otmpForm = CType(SBO_Application.Forms.Item(FormUID), SAPbouiCOM.Form)
                        '     'oItem = otmpForm.Items.Item(mc_strIDBotonEjecucion)
                        '     'sButton = CType(oItem.Specific, SAPbouiCOM.Button)

                        '     'oEdit = CType(otmpForm.Items.Item("txtNumVeh").Specific, SAPbouiCOM.EditText)

                        '     'consulta si valida la longitud del VIN no puede ser mayor a 17 caracteres
                        '     Dim strValidaCantidadVIN As String = Utilitarios.EjecutarConsulta("Select U_ValongVIN from [@SCGD_ADMIN] where Code = 'DMS'", m_oCompany.CompanyDB, m_oCompany.Server)

                        '     'Valida el numero de VIN del Vehiculo, si y solo si, tiene el check de validar VIN
                        '     If Not pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE And Not pVal.FormMode = SAPbouiCOM.BoFormMode.fm_OK_MODE And _
                        '         Not pVal.FormMode = SAPbouiCOM.BoFormMode.fm_FIND_MODE Then

                        '         Dim strValidaVIN As String = Utilitarios.EjecutarConsulta("Select U_SCGD_VIN from [@SCGD_ADMIN] where Code = 'DMS'", m_oCompany.CompanyDB, m_oCompany.Server)
                        '         Dim strValidaUnidadVacia As String = Utilitarios.EjecutarConsulta("Select U_SCGD_Uni from [@SCGD_ADMIN] where Code = 'DMS'", m_oCompany.CompanyDB, m_oCompany.Server)

                        '         If strValidaVIN = "Y" Then
                        '             If m_oVehiculos.ValidarNumeroVIN() Then

                        '                 SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeExisteVIN, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        '                 BubbleEvent = False

                        '             End If
                        '         End If

                        '         If strValidaUnidadVacia = "Y" Then
                        '             Dim codigoVehiculo As String = otmpForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_Cod_Unid", 0)

                        '             If codigoVehiculo = String.Empty Then
                        '                 SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeUnidadVacia, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        '                 BubbleEvent = False
                        '             End If

                        '         End If

                        '     End If

                        '     ' strDescripcionBoton = sButton.Caption

                        '     If pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                        '         'm_oVehiculos.CargarConsecutivoAlCrear()
                        '     End If

                        '     If pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE OrElse pVal.FormMode = SAPbouiCOM.BoFormMode.fm_EDIT_MODE OrElse pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                        '         'If Not m_oVehiculos.ValidarOpcionNoRepetida(otmpForm, VehiculosCls.mc_strPlaca) Then
                        '         '    BubbleEvent = False
                        '         '    If SBO_Application.MessageBox(My.Resources.Resource.PlacaYaRegistrada, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then

                        '         '        m_oVehiculos.CargarVehiculo(VehiculosCls.mc_strPlaca)
                        '         '    End If

                        '         'Else

                        '         'Erick Sanabria Bravo: Validación Si usa Fecha Reserva 20/11/2013
                        '         '__________________________________________________________________'
                        '         Dim strDispoVehiculo As String = otmpForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_Dispo", 0)
                        '         Dim strFechaReservaVehiculo As String = otmpForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_FchRsva", 0)
                        '         Dim strConsulta_Dispo As String = "Select U_Disp_Res From [@SCGD_ADMIN]"
                        '         Dim strDispoReservado As String = Utilitarios.EjecutarConsulta(strConsulta_Dispo, m_oCompany.CompanyDB, m_oCompany.Server)

                        '         If m_oVehiculos.ValidarSiFechaReserva(otmpForm, strDispoVehiculo, strFechaReservaVehiculo, strDispoReservado) Then
                        '             BubbleEvent = False
                        '             SBO_Application.StatusBar.SetText("Debe Ingresar Fecha de Reserva", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        '         Else
                        '             If (strDispoVehiculo <> strDispoReservado) Then
                        '                 otmpForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_FchRsva", 0, "")
                        '             End If
                        '         End If
                        '         '__________________________________________________________________'
                        '         'Erick Sanabria Bravo: Validación Si usa Fecha Reserva 20/11/2013

                        '         If Not m_oVehiculos.ValidarOpcionNoRepetida(VehiculosCls.mc_strUnidad) Then

                        '             BubbleEvent = False
                        '             If SBO_Application.MessageBox(My.Resources.Resource.UnidadYaRegistrada, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then

                        '                 m_oVehiculos.CargarVehiculo(VehiculosCls.mc_strUnidad)
                        '             End If
                        '         Else
                        '             strMarcaSelected = m_oVehiculos.ObtenerIDControles(otmpForm, VehiculosCls.mc_strMarca, VehiculosCls.TipoControl.ComboBox)
                        '             strEstiloSelected = m_oVehiculos.ObtenerIDControles(otmpForm, VehiculosCls.mc_strEstilo, VehiculosCls.TipoControl.ComboBox)

                        '             If strMarcaSelected = mc_strDetenerSBO Or strEstiloSelected = mc_strDetenerSBO Then
                        '                 BubbleEvent = False
                        '                 SBO_Application.StatusBar.SetText(My.Resources.Resource.RequeridasMarcaYEstilo, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        '             ElseIf strMarcaSelected = mc_strNoUpdateCombo Then
                        '                 blnActualizarCombos = True
                        '             Else

                        '                 strMarcaSelected = m_oVehiculos.ObtenerIDControles(otmpForm, "cboMarca", VehiculosCls.TipoControl.ComboBox)
                        '                 strEstiloSelected = m_oVehiculos.ObtenerIDControles(otmpForm, "cboEst", VehiculosCls.TipoControl.ComboBox)
                        '                 strModeloSelected = m_oVehiculos.ObtenerIDControles(otmpForm, "cboModelo", VehiculosCls.TipoControl.ComboBox)
                        '                 'strNumVehiculo = m_oVehiculos.ObtenerIDControles(otmpForm, "txtNumVeh", VehiculosCls.TipoControl.EditText)
                        '                 blnActualizarCombos = False
                        '             End If

                        '             If strValidaCantidadVIN = "Y" Then

                        '                 If Utilitarios.ValidarLongitudVIN(otmpForm, m_oCompany, "@SCGD_VEHICULO", "U_Num_VIN") Then
                        '                     SBO_Application.SetStatusBarMessage(My.Resources.Resource.ErrorLongitudVIN, BoMessageTime.bmt_Short)
                        '                     BubbleEvent = False
                        '                 End If

                        '             End If

                        '         End If
                        '     End If

                        '     '****agregado para verificar actualizacion de la tabla @SCGD_ACCXVEH********************************************************************
                        '     If otmpForm.Mode = BoFormMode.fm_UPDATE_MODE Then
                        '         Dim mtx As Matrix = DirectCast(otmpForm.Items.Item("mtx_0").Specific, Matrix)

                        '         Dim codigoVehiculo As String = otmpForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("Code", 0)
                        '         codigoVehiculo = codigoVehiculo.Trim()
                        '         Dim strCodeTraza As String = Utilitarios.EjecutarConsulta("Select Code from [@SCGD_VEHITRAZA] where Code = " & codigoVehiculo & " And LineId = 1", m_oCompany.CompanyDB, m_oCompany.Server)

                        '         mtx.FlushToDataSource()
                        '         If otmpForm.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").Size = 0 Then

                        '             otmpForm.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").SetValue("Code", otmpForm.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").Offset, CType(codigoVehiculo, Integer))
                        '             mtx.LoadFromDataSource()

                        '         End If

                        '         If String.IsNullOrEmpty(strCodeTraza) Then
                        '             otmpForm.DataSources.DBDataSources.Item("@SCGD_VEHITRAZA").SetValue("Code", otmpForm.DataSources.DBDataSources.Item("@SCGD_VEHITRAZA").Offset, CType(codigoVehiculo, Integer))
                        '         End If

                        '     End If

                        '     If pVal.FormMode = SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                        '         m_oVehiculos.ManejadorEventoItemPressed(FormUID, pVal, BubbleEvent, m_blnUsaCosteoVehículo)
                        '     End If
                        '     '****************************************************************************************************************************************
                        '     'm_strNoVehiculo = oEdit.String
                        '     m_oVehiculos.ManejarModoFormulario(otmpForm)
                        ' End If


                        ' If pVal.ItemUID = mc_strIDBotonEjecucion _
                        ' AndAlso pVal.FormTypeEx = mc_strControlVehiculo _
                        ' AndAlso pVal.ActionSuccess Then


                        '     'If otmpForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE _
                        '     'Or otmpForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then

                        '     otmpForm = CType(SBO_Application.Forms.Item(FormUID), SAPbouiCOM.Form)

                        '     'valida que el formulario no sea nulo
                        '     If otmpForm IsNot Nothing Then

                        '         If Not blnActualizarCombos AndAlso Not String.IsNullOrEmpty(strNumVehiculo) Then

                        '             Call m_oVehiculos.InsertarDetallesCombos(strMarcaSelected, strEstiloSelected, strModeloSelected, strNumVehiculo, strColorSelected, m_strNoVehiculo, otmpForm)

                        '             If m_blnUsaCosteoVehículo Then
                        '                 m_oVehiculos.EnviarACosteo()
                        '             End If

                        '             If m_oVehiculos.CierraForma Then

                        '                 ' aqui se debe hacer la llamada con la lista
                        '                 If Not oformCotizacion Is Nothing OrElse Not IsNothing(oformCita) Then

                        '                     m_oVehiculos.EjecutaEvento(m_strNoVehiculo, pVal, oformCotizacion)
                        '                     Call otmpForm.Close()
                        '                 End If

                        '                 'Else
                        '                 'otmpForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE
                        '             End If

                        '         End If

                        '     End If
                        ' End If

                        '****************************************************


                        If pVal.ItemUID = "lkContV_I" _
                            AndAlso pVal.FormTypeEx = mc_strControlVehiculo _
                            AndAlso pVal.ActionSuccess Then

                            Dim oForm As SAPbouiCOM.Form
                            oForm = SBO_Application.Forms.Item(FormUID)

                            strIDContrato = oForm.DataSources.DBDataSources.Item("@SCGD_VEHITRAZA").GetValue("U_NumCV_I", 0)

                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_frmContVent", False) Then
                                Call m_oCVenta.DibujarFormularioContratoVentas("", False)
                                Call m_oCVenta.CargarContrato(strIDContrato, "SCGD_frmContVent")
                                Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item("SCGD_frmContVent"), False)

                                m_oCVenta.m_blnCargoManejarEstados = False

                            ElseIf pVal.FormTypeEx = mc_strVehiTraza AndAlso pVal.BeforeAction AndAlso pVal.ItemUID = "lkContV_I" Then

                                m_oCVenta.m_blnCargoManejarEstados = True

                            End If
                        End If

                        If pVal.ItemUID = "lkContV_S" _
                            AndAlso pVal.FormTypeEx = mc_strControlVehiculo _
                            AndAlso pVal.ActionSuccess Then

                            Dim oForm As SAPbouiCOM.Form
                            oForm = SBO_Application.Forms.Item(FormUID)

                            strIDContrato = oForm.DataSources.DBDataSources.Item("@SCGD_VEHITRAZA").GetValue("U_NumCV_V", 0)

                            If strIDContrato <> "" AndAlso Not ValidarSiFormularioAbierto("SCGD_frmContVent", False) Then
                                Call m_oCVenta.DibujarFormularioContratoVentas("", False)
                                Call m_oCVenta.CargarContrato(strIDContrato, "SCGD_frmContVent")
                                Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item("SCGD_frmContVent"), False)

                                m_oCVenta.m_blnCargoManejarEstados = False

                            ElseIf pVal.FormTypeEx = mc_strVehiTraza AndAlso pVal.BeforeAction AndAlso pVal.ItemUID = "lkContV_I" Then

                                m_oCVenta.m_blnCargoManejarEstados = True

                            End If
                        End If


                        If (pVal.ItemUID = mc_strIDBotonEjecucion _
                           AndAlso pVal.BeforeAction) _
                           AndAlso (pVal.FormTypeEx = mc_strFacturadeCompra _
                                   Or pVal.FormTypeEx = mc_strNotadeCredito _
                                   Or pVal.FormTypeEx = mc_strSalidadeInventario _
                                   Or pVal.FormTypeEx = mc_strEntrdadeinventario _
                                   Or pVal.FormTypeEx = mc_strEntradadeMercancia _
                                   Or pVal.FormTypeEx = mc_strSalidadeMercancia) Then

                            otmpForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                            'oItem = otmpForm.Items.Item(mc_strIDBotonEjecucion)
                            'sButton = CType(oItem.Specific, SAPbouiCOM.Button)
                            'strLabel = sButton.Caption

                            If otmpForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then

                                If pVal.FormTypeEx = mc_strFacturadeCompra Or pVal.FormTypeEx = mc_strNotadeCredito Or pVal.FormTypeEx = mc_strEntradadeMercancia Or pVal.FormTypeEx = mc_strSalidadeMercancia Then
                                    oItem = otmpForm.Items.Item("8")
                                    oEdit = DirectCast(oItem.Specific, SAPbouiCOM.EditText)
                                    m_strDocEntryByBeforeAction = oEdit.String

                                ElseIf pVal.FormTypeEx = mc_strSalidadeInventario Or pVal.FormTypeEx = mc_strEntrdadeinventario Then
                                    oItem = otmpForm.Items.Item("7")
                                    oEdit = DirectCast(oItem.Specific, SAPbouiCOM.EditText)
                                    m_strDocEntryByBeforeAction = oEdit.String
                                End If



                            End If

                        End If


                        If pVal.FormTypeEx = mc_strVisualizadorfotos And pVal.ItemUID = mc_strBotonFotos And pVal.ActionSuccess = False Then

                            otmpForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                            If otmpForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then

                                oItem = otmpForm.Items.Item("SCGD_etOT")
                                oEdit = DirectCast(oItem.Specific, SAPbouiCOM.EditText)
                                Dim Orden As String = oEdit.String

                                If (Not String.IsNullOrEmpty(Orden.ToString().Trim())) Then
                                    Dim Fotos As New frmVisualFotos(Orden, SBO_Application, m_oCompany)
                                    Fotos.Enabled = True
                                    Call Fotos.ShowDialog()
                                Else
                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.NoTieneOrdenAsociada, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                End If
                            End If
                        End If

                        If (pVal.ItemUID = mc_strIDBotonEjecucion _
                           AndAlso Not pVal.BeforeAction _
                           AndAlso pVal.ActionSuccess) _
                           AndAlso (pVal.FormTypeEx = mc_strFacturadeCompra _
                                   Or pVal.FormTypeEx = mc_strNotadeCredito _
                                   Or pVal.FormTypeEx = mc_strSalidadeInventario _
                                   Or pVal.FormTypeEx = mc_strEntrdadeinventario _
                                   Or pVal.FormTypeEx = mc_strEntradadeMercancia _
                                   Or pVal.FormTypeEx = mc_strSalidadeMercancia) Then

                            otmpForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                            'oItem = otmpForm.Items.Item(mc_strIDBotonEjecucion)
                            'sButton = CType(oItem.Specific, SAPbouiCOM.Button)
                            'strLabel = sButton.Caption

                            If otmpForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then

                                If pVal.FormTypeEx = mc_strFacturadeCompra Then

                                    ''Cambios Proceso Josue Inicio
                                    'Dim strDocEntry As String = otmpForm.DataSources.DBDataSources.Item("OPCH").GetValue("DocEntry", 0).ToString.Trim()
                                    'm_oDocumentoProcesoCompra = New DocumentoProcesoCompra(m_oCompany, SBO_Application)
                                    'Call m_oDocumentoProcesoCompra.ProcesaDocumentoMarketing(strDocEntry, 0)
                                    ''Cambios Proceso Josue Fin


                                    'Dim l_strDocCosteo As String
                                    ''botón presionado

                                    If m_strDocEntryByStatusBar = "" Then
                                        strDocnumCompras = m_strDocEntryByBeforeAction
                                    Else
                                        strDocnumCompras = m_strDocEntryByStatusBar
                                    End If

                                    'Dim m_strTipo As String = otmpForm.DataSources.DBDataSources.Item("OPCH").GetValue("DocType", 0).ToString.Trim()

                                    'If Not String.IsNullOrEmpty(m_strTipo) AndAlso m_strTipo = "I" Then

                                    '    Dim TipoDocumentoBase As String
                                    '    TipoDocumentoBase = Utilitarios.EjecutarConsulta(String.Format("SELECT distinct BaseType FROM [PCH1] with (nolock) " &
                                    '                                                                   "INNER JOIN OPCH with (nolock) on OPCH.DocEntry = PCH1.DocEntry " &
                                    '                                                                    "WHERE OPCH.DocNum = {0}", strDocnumCompras), m_oCompany.CompanyDB, m_oCompany.Server)

                                    '    If TipoDocumentoBase.Trim() = "540000006" Then

                                    '        Call m_oCompras.RecorreDocumentosMarketingSinProcesar(m_oCompany, _
                                    '                                        SAPbobsCOM.BoObjectTypes.oPurchaseInvoices, _
                                    '                                        540000006, _
                                    '                                        strDocnumCompras)
                                    '    Else
                                    '        Call m_oCompras.RecorreDocumentosMarketingSinProcesar(m_oCompany, _
                                    '                                           SAPbobsCOM.BoObjectTypes.oPurchaseInvoices, _
                                    '                                            SAPbobsCOM.BoObjectTypes.oPurchaseOrders, _
                                    '                                            strDocnumCompras)

                                    '    End If
                                    'End If

                                    ' Actualiza el Campo "U_NumFactura" en la ventana de "Costeo de Entradas" cuando se crea la factura de proveedor, desde el borrador

                                    If m_strDocEntryByStatusBar = "" Then
                                        strDocnumCompras = m_strDocEntryByBeforeAction
                                    Else
                                        strDocnumCompras = m_strDocEntryByStatusBar
                                    End If

                                    Dim l_strDocCosteo As String

                                    l_strDocCosteo = Utilitarios.EjecutarConsulta("Select U_SCGD_DocCost from OPCH with (nolock) where DocNum = '" & strDocnumCompras & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                                    If Not String.IsNullOrEmpty(l_strDocCosteo) Then
                                        m_oFormularioCosteoDeEntradas.ActualizaDocumentoCosteo(l_strDocCosteo)
                                    End If

                                    ''Call m_oCompras.RecorreDocumentosMarketingSinProcesar(m_oCompany, _
                                    ''                                          mc_strFacturasAcreedoresSinProcesar, _
                                    ''                                          SAPbobsCOM.BoObjectTypes.oPurchaseInvoices, _
                                    ''                                          SAPbobsCOM.BoObjectTypes.oPurchaseOrders, _
                                    ''                                          strDocnumCompras)


                                ElseIf pVal.FormTypeEx = mc_strNotadeCredito Then

                                    If m_strDocEntryByStatusBar = "" Then

                                        strDocnumCompras = m_strDocEntryByBeforeAction
                                    Else

                                        strDocnumCompras = m_strDocEntryByStatusBar

                                    End If


                                    'Call m_oCompras.RecorreDocumentosMarketingSinProcesar(m_oCompany,
                                    '                                              SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes, _
                                    '                                              SAPbobsCOM.BoObjectTypes.oPurchaseInvoices, _
                                    '                                              SAPbobsCOM.BoObjectTypes.oPurchaseOrders, _
                                    '                                              strDocnumCompras)

                                ElseIf pVal.FormTypeEx = mc_strEntradadeMercancia Then

                                    ''llamada a metodo para entradas de mercancia

                                    'If m_strDocEntryByStatusBar = "" Then

                                    '    strDocnumCompras = m_strDocEntryByBeforeAction
                                    'Else

                                    '    strDocnumCompras = m_strDocEntryByStatusBar

                                    'End If

                                    'Dim TipoDocumentoBase As String = String.Empty

                                    'TipoDocumentoBase = Utilitarios.EjecutarConsulta(String.Format("SELECT distinct BaseType FROM [PDN1] with (nolock) " &
                                    '                                                                "INNER JOIN OPDN with (nolock) on OPDN.DocEntry = PDN1.DocEntry " &
                                    '                                                                "WHERE OPDN.DocNum = {0}", strDocnumCompras), m_oCompany.CompanyDB, m_oCompany.Server)

                                    'If TipoDocumentoBase.Trim() = "540000006" Then

                                    '    Call m_oCompras.RecorreDocumentosMarketingSinProcesar(m_oCompany, _
                                    '                                          SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes, _
                                    '                                           540000006, _
                                    '                                          strDocnumCompras)


                                    'Else
                                    '    Call m_oCompras.RecorreDocumentosMarketingSinProcesar(m_oCompany, _
                                    '                                          SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes, _
                                    '                                           SAPbobsCOM.BoObjectTypes.oPurchaseOrders, _
                                    '                                           strDocnumCompras)


                                    'End If



                                ElseIf pVal.FormTypeEx = mc_strSalidadeMercancia Then

                                    If m_strDocEntryByStatusBar = "" Then

                                        strDocnumCompras = m_strDocEntryByBeforeAction
                                    Else

                                        strDocnumCompras = m_strDocEntryByStatusBar

                                    End If

                                    'Call m_oCompras.RecorreDocumentosMarketingSinProcesar(m_oCompany,
                                    '                                           SAPbobsCOM.BoObjectTypes.oPurchaseReturns, _
                                    '                                           SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes, _
                                    '                                           SAPbobsCOM.BoObjectTypes.oPurchaseOrders, _
                                    '                                           strDocnumCompras)

                                ElseIf pVal.FormTypeEx = mc_strIdFormaCotizacion Then


                                    Call m_oCotizacion.RecorrerCotizacionesSinProcesar()

                                End If
                            Else


                                If otmpForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then

                                    If pVal.FormTypeEx = mc_strIdFormaCotizacion Then

                                        Call m_oCotizacion.RecorrerCotizacionesSinProcesar()

                                    End If

                                End If

                            End If

                        End If

                        If pVal.ItemUID = "1" And pVal.FormTypeEx = mc_strUITrasC Then

                            If Not pVal.BeforeAction Then
                                otmpForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                                Dim oeditDocentry As SAPbouiCOM.EditText = DirectCast(otmpForm.Items.Item("txtDocEnt").Specific, EditText)

                                If oeditDocentry.Value = String.Empty Then
                                    Exit Sub
                                Else
                                    otmpForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                                End If


                            End If

                        End If

                        'manejo de eventos para Nota de Credito Clientes

                        If pVal.FormTypeEx = mc_strNotaCreditoCliente Then

                            Call m_oNotaCreditoClientes.ManejadorEventoItemPress(pVal, pVal.FormUID, BubbleEvent)

                        End If

                        If pVal.FormTypeEx = m_oVentanaAutorizaciones Then

                            If pVal.ItemUID = "1" And pVal.Before_Action = True Then
                                If pVal.FormTypeEx <> mc_strOfertaDeCompra And (pVal.FormTypeEx = mc_strOrdenDeCompra Or pVal.FormTypeEx = m_oVentanaAutorizaciones) Then
                                    DocAprobacionHabilitado = True
                                End If
                            End If

                        End If

                        If pVal.FormTypeEx = mc_strOrdenDeVenta Then
                            m_oOrdenVenta.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = g_strFormTipoOTInterna Then
                            m_oTipoOtInterna.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = g_strFormSolOTEspecial Then
                            m_oSolicitaOtEsp.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = g_strFormAsignacionMultiple Then
                            m_oAsignacionMultiple.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)
                        End If

                        '***************************************
                        'solicitud de OT Especial - Agregado 11-12-2013
                        If pVal.FormTypeEx = mc_strDimensionesContables Then
                            Call m_oDimensionesContables.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strDimensionesContablesOTs Then
                            Call m_oDimensionesContablesOTs.ManejadorEventoItemPressed(pVal.FormUID, pVal, BubbleEvent)
                        End If

                        'Formulario de Busqueda de Listas de Precios [Parámetros de aplicacione]
                        If pVal.FormTypeEx = mc_strFormListaPreciosSelecicon Then
                            Call m_oFormularioListaPreciosSeleccion.ManejadorEventoItemPress(pVal, pVal.FormUID, BubbleEvent)
                        End If

                        'Formulario de Busqueda de Listas de Precios [Parámetros de aplicacione]
                        If pVal.FormTypeEx = mc_strFormListaEmpleados Then
                            Call m_oFormularioSeleccionEmpleados.ManejadorEventoItemPress(pVal, pVal.FormUID, BubbleEvent, m_oCompany)
                        End If

                        'Formulario de Busqueda de Listas de Precios [Parámetros de aplicacione]
                        If pVal.FormTypeEx = g_strConfMsj Then
                            Call m_oFormularioConfMsJ.ManejadorEventoItemPress(pVal, pVal.FormUID, BubbleEvent, m_oCompany)
                        End If

                        'Formulario de Lista de Requisiciones
                        If pVal.FormTypeEx = mc_strFormLstReq Then
                            Call m_oFormularioListadoRequisiciones.ManejadorEventoItemPress(pVal, pVal.FormUID, BubbleEvent)
                        End If

                        'Formulario de Lista de Solicitud de Específicos
                        If pVal.FormTypeEx = mc_strFormLstSolEsp Then
                            Call m_oFormularioListadoSolicitudEspecificos.ManejadorEventoItemPress(pVal, pVal.FormUID, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strFormKardex Then

                            m_oFormularioKardexInventarioVehiculo.ManejadorEventoItemPressed(FormUID, pVal, BubbleEvent)


                        End If

                        If pVal.FormTypeEx = mc_strFormMediosPago Then
                            Call m_oMediosPago.ManejadorEventoItemPressed(FormUID, pVal, BubbleEvent)
                        End If

                        'If pVal.FormTypeEx = mc_strCargaMasivaVehiculos Then
                        '    m_oFormularioCargaMasivaVehiculos.ItemPressed(pVal)
                        'End If

                        If pVal.FormTypeEx = mc_strUID_FORM_ReporteVehiculosRecurrentesTaller And pVal.ActionSuccess Then
                            If Not m_oFormularioReporteVehiculosRecurrentesTaller Is Nothing Then
                                m_oFormularioReporteVehiculosRecurrentesTaller.ManejadorEventoItemPressed(pVal, BubbleEvent)
                            End If
                        End If

                        If pVal.FormTypeEx = mc_strUID_FORM_ReporteVentasXAsesorServicio And pVal.ActionSuccess Then
                            If Not m_oFormularioReporteVentasXAsesorServicio Is Nothing Then
                                m_oFormularioReporteVentasXAsesorServicio.ManejadorEventoItemPressed(pVal, BubbleEvent)
                            End If
                        End If



                    Case SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK

                        If pVal.BeforeAction _
                                AndAlso (pVal.ItemUID = "4") _
                                AndAlso pVal.FormTypeEx = m_strUIDVehiMarcaEtc Then

                            m_oCFLbyFS.ManejadorEventoItemPressedCFLbyFS(FormUID, pVal, BubbleEvent)

                        Else
                            If pVal.BeforeAction And pVal.FormTypeEx = mc_strDocumentoPreliminar Then

                                blnManejarFormularioTransferencia = True

                                otmpForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                                Dim oEditText As SAPbouiCOM.EditText
                                Dim oRefItem As SAPbouiCOM.Item

                                oRefItem = otmpForm.Items.Item("3")
                                oMatrixDraft = DirectCast(oRefItem.Specific, Matrix)

                                'Dim intDocEntryDraft As Integer = oMatrixDraft.GetNextSelectedRow(0, BoOrderType.ot_RowOrder)
                                intValor = pVal.Row

                                oEditText = CType(oMatrixDraft.Columns.Item(1).Cells.Item(intValor).Specific, EditText)


                                Dim strDocEntryBorrar As String = oEditText.Value

                                blnTransferenciaDesdeDraft = True
                                'blnTransferenciaNormal = False

                            End If
                        End If

                        Select Case pVal.FormTypeEx
                            Case mc_strFormVehiculoSeleciconColor
                                m_oFormularioVehiculoColorSeleccion.ManejadorEventoDobleClick(FormUID, pVal, BubbleEvent)
                            Case mc_strFormListaPreciosSelecicon
                                m_oFormularioListaPreciosSeleccion.ManejadorEventoDobleClick(FormUID, pVal, BubbleEvent)
                            Case mc_strUISCGD_BusqCitas
                                m_oFormularioBusquedasCitas.ManejadorEventoDobleClick(FormUID, pVal, BubbleEvent)
                            Case mc_strFormVehiculosArticulosVenta
                                m_oFormularioVehiculoArticuloVenta.ManejadorEventoDobleClick(FormUID, pVal, BubbleEvent)
                            Case mc_strFormListaEmpleados
                                Call m_oFormularioSeleccionEmpleados.ManejadorEventoDobleClick(pVal, pVal.FormUID, BubbleEvent)
                            Case mc_strFormSelUbi
                                Call m_oFormSeleccionUbicaciones.ManejadorEventoDobleClick(pVal, FormUID, BubbleEvent)

                        End Select

                    Case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE

                        If pVal.FormTypeEx = "940" Then 'And (pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE) Then

                            'If blnTransferenciaDesdeDraft Then 'And Not blnTransferenciaNormal Then
                            'otmpForm = SBO_Application.Forms.GetForm("940", 0)
                            'otmpForm.ActiveItem = "2"
                            'otmpForm.Items.Item("2").Click()
                            'Exit Select

                            'otmpForm.Close()
                            '    otmpForm.Select()
                            '    Call EliminarDocumentoDraft(otmpForm, oMatrixDraft, intValor)
                            'End If


                            blnTransferenciaDesdeDraft = False
                            blnManejarFormularioTransferencia = False

                        End If

                        If pVal.FormTypeEx = mc_strContratoVenta Then
                            Call m_oCVenta.ManejadorEventoFormClose(pVal, pVal.FormUID, BubbleEvent)
                        End If

                        If pVal.FormTypeEx = mc_strIdFormaCotizacion Then

                            'If pVal.BeforeAction Then
                            otmpForm = SBO_Application.Forms.GetForm(SBO_Application.Forms.ActiveForm.Type, SBO_Application.Forms.ActiveForm.TypeCount)


                            If otmpForm.Mode = BoFormMode.fm_UPDATE_MODE And pVal.Before_Action Then


                                m_oCotizacion.blnValidarCamposHS_KM = True

                            Else

                                m_oCotizacion.blnValidarCamposHS_KM = False
                            End If


                        End If

                        If pVal.FormTypeEx = mc_strFacturaCliente Or pVal.FormTypeEx = mc_strBoleta Then

                            Call m_oFacturaClientes.ManejadorEventoClose(pVal, pVal.FormUID, BubbleEvent)

                        End If
                End Select

                ControladorDisponibilidadEmpleados.ItemEvent(pVal.FormUID, pVal, BubbleEvent)
                ControladorRestablecerCantidadesPendientes.ItemEvent(pVal.FormUID, pVal, BubbleEvent)
                ControladorCitas.ItemEvent(pVal.FormUID, pVal, BubbleEvent)
                ControladorBusquedaArticulosCitas.ItemEvent(pVal.FormUID, pVal, BubbleEvent)
                ControladorReporteBodegaReservas.ItemEvent(pVal.FormUID, pVal, BubbleEvent)
                OfertaCompra.ItemEvent(FormUID, pVal, BubbleEvent)
                OrdenCompra.ItemEvent(FormUID, pVal, BubbleEvent)
                SCG.ServicioPostVenta.RegistroTiempo.ItemEvent(FormUID, pVal, BubbleEvent)
                AdministradorLicencias.ItemEvent(FormUID, pVal, BubbleEvent)
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Private Sub SBO_Application_StatusBarEvent(ByVal Text As String, ByVal MessageType As SAPbouiCOM.BoStatusBarMessageType) Handles SBO_Application.StatusBarEvent

        Dim strMensaje As String

        m_strDocEntryByStatusBar = ""

        If MessageType = SAPbouiCOM.BoStatusBarMessageType.smt_Warning Then
            strMensaje = My.Resources.Resource.MensajeConsecutivoDiferente

            If Text Like strMensaje & "*" Then
                m_strDocEntryByStatusBar = Text.Split(CChar(":"))(1).Trim
            End If
        End If


        If MessageType = SAPbouiCOM.BoStatusBarMessageType.smt_Error Then
            strMensaje = My.Resources.Resource.MensajeDatosModificados

            If Text Like My.Resources.Resource.ExtensionDLL_Fallo Then
                m_strDocEntryByStatusBar = strMensaje.Trim

                If Not String.IsNullOrEmpty(m_strDocEntryByStatusBar) Then
                    SBO_Application.StatusBar.SetText(m_strDocEntryByStatusBar, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                End If

            End If
        End If

        If Not String.IsNullOrEmpty(m_strMensajePreFormDataEvent) Then
            SBO_Application.StatusBar.SetText(m_strMensajePreFormDataEvent, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End If
        m_strMensajePreFormDataEvent = ""

    End Sub

    Private Sub SBO_Application_MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, _
                                          ByRef BubbleEvent As Boolean) Handles SBO_Application.MenuEvent
        Try
            If pVal.BeforeAction Then
                If Not AdministradorLicencias.LicenciaUsuarioValida(DMS_Connector.Company.CompanySBO.UserSignature, pVal.MenuUID) Then
                    BubbleEvent = False
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorLicencia, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return
                End If
            End If

            Dim oForm As SAPbouiCOM.Form

            If FormularioAgendaSBO.Modal Then
                BubbleEvent = False
                Return
            End If

            If Not pVal.BeforeAction Then

                Select Case pVal.MenuUID
                    Case IDMenus.IDMenus.m_oFormularioPresupuestos
                        If (m_oFormularioPresupuestos IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioPresupuestos, activarSiEstaAbierto:=True) Then
                                m_oFormularioPresupuestos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioPresupuestos)

                                m_oFormularioPresupuestos.ManejadorEventoLoad(pVal, BubbleEvent)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioRequisiciones
                        If (m_oFormularioRequisiciones IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioRequisiciones, activarSiEstaAbierto:=True) Then
                                m_oFormularioRequisiciones.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioRequisiciones)
                            End If
                        End If

                    Case IDMenus.IDMenus.strMenuListRequisiciones
                        If (m_oFormularioListadoRequisiciones IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioListadoRequisiciones, activarSiEstaAbierto:=True) Then
                                m_oFormularioListadoRequisiciones.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioListadoRequisiciones)
                            End If
                        End If


                    Case IDMenus.IDMenus.strMenuListSolicitudEspecificos
                        If (m_oFormularioListadoSolicitudEspecificos IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioListadoSolicitudEspecificos, activarSiEstaAbierto:=True) Then
                                m_oFormularioListadoSolicitudEspecificos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioListadoSolicitudEspecificos)
                            End If
                        End If

                    Case IDMenus.IDMenus.strMenuSolicitudEspecificos
                        If (m_oFormularioSolicitudEspecificos IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioSolicitudEspecificos, activarSiEstaAbierto:=True) Then
                                m_oFormularioSolicitudEspecificos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioSolicitudEspecificos)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioConfFinanc
                        If (m_oFormularioConfFinanc IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioConfFinanc, activarSiEstaAbierto:=True) Then
                                m_oFormularioConfFinanc.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioConfFinanc)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioPrestamo
                        If (m_oFormularioPrestamo IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioPrestamo, activarSiEstaAbierto:=True) Then
                                m_oFormularioPrestamo.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioPrestamo)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioEstadoCuentas
                        If (m_oFormularioEstadoCuentas IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioEstadoCuentas, activarSiEstaAbierto:=True) Then
                                m_oFormularioEstadoCuentas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioEstadoCuentas)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioHistoricoPagos
                        If (m_oFormularioHistoricoPagos IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioHistoricoPagos, activarSiEstaAbierto:=True) Then
                                m_oFormularioHistoricoPagos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioHistoricoPagos)
                            End If
                        End If
                    Case IDMenus.IDMenus.m_oFormularioCuotasVencidas
                        If (m_oFormularioCuotasVencidas IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioCuotasVencidas, activarSiEstaAbierto:=True) Then
                                m_oFormularioCuotasVencidas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioCuotasVencidas)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioSaldos
                        If (m_oFormularioSaldos IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioSaldos, activarSiEstaAbierto:=True) Then
                                m_oFormularioSaldos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioSaldos)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioPlacas
                        If (m_oFormularioPlacas IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioPlacas, activarSiEstaAbierto:=True) Then
                                m_oFormularioPlacas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioPlacas)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioPlacaGrupos
                        If (m_oFormularioPlacaGrupos IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioPlacaGrupos, activarSiEstaAbierto:=True) Then
                                m_oFormularioPlacaGrupos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioPlacaGrupos)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioVehiculoTipoEvento
                        If (m_oFormularioVehiculoTipoEvento IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioVehiculoTipoEvento, activarSiEstaAbierto:=True) Then
                                m_oFormularioVehiculoTipoEvento.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioVehiculoTipoEvento)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioContratoTraspaso
                        If (m_oFormularioContratoTraspaso IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioContratoTraspaso, activarSiEstaAbierto:=True) Then
                                m_oFormularioContratoTraspaso.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioContratoTraspaso)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioComision
                        If (m_oFormularioComision IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioComision, activarSiEstaAbierto:=True) Then
                                m_oFormularioComision.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioComision)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioVehiculosProblemas
                        If (m_oFormularioVehiculosProblemas IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioVehiculosProblemas, activarSiEstaAbierto:=True) Then
                                m_oFormularioVehiculosProblemas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioVehiculosProblemas)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oRefacturacion
                        If (m_oRefacturacion IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oRefacturacion, activarSiEstaAbierto:=True) Then
                                m_oRefacturacion.FormularioSBO = oGestorFormularios.CargaFormulario(m_oRefacturacion)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioUnidadesVendidas
                        If (m_oFormularioUnidadesVendidas IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioUnidadesVendidas, activarSiEstaAbierto:=True) Then
                                m_oFormularioUnidadesVendidas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioUnidadesVendidas)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormMantenEspecificacionPorModelo
                        If (m_oFormMantenEspecificacionPorModelo IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormMantenEspecificacionPorModelo, activarSiEstaAbierto:=True) Then
                                m_oFormMantenEspecificacionPorModelo.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormMantenEspecificacionPorModelo)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioCitaXTipoAgenda
                        If (m_oFormularioCitaXTipoAgenda IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioCitaXTipoAgenda, activarSiEstaAbierto:=True) Then
                                m_oFormularioCitaXTipoAgenda.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioCitaXTipoAgenda)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioParametrosAplicacion
                        If (m_oFormularioParametrosAplicacion IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioParametrosAplicacion, activarSiEstaAbierto:=True) Then
                                m_oFormularioParametrosAplicacion.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioParametrosAplicacion)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormConfInterfazFord
                        If (m_oFormConfInterfazFord IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormConfInterfazFord, activarSiEstaAbierto:=True) Then
                                m_oFormConfInterfazFord.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormConfInterfazFord)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormConfInterfazTSD
                        If (m_oFormConfIntTDS IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormConfIntTDS, activarSiEstaAbierto:=True) Then
                                m_oFormConfIntTDS.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormConfIntTDS)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormConfInterfazAudatex
                        If (m_oFormConfIntAudatex IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormConfIntAudatex, activarSiEstaAbierto:=True) Then
                                m_oFormConfIntAudatex.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormConfIntAudatex)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioAgendasConfiguracion
                        If (m_oFormularioAgendasConfiguracion IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioAgendasConfiguracion, activarSiEstaAbierto:=True) Then
                                m_oFormularioAgendasConfiguracion.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioAgendasConfiguracion)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioConfMsJ
                        If (m_oFormularioConfMsJ IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioConfMsJ, activarSiEstaAbierto:=True) Then
                                m_oFormularioConfMsJ.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioConfMsJ)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioCitas
                        If (m_oFormularioCitas IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioCitas, activarSiEstaAbierto:=True) Then
                                m_oFormularioCitas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioCitas)
                            End If
                        End If
                    Case IDMenus.IDMenus.m_oFormularioBodegaProceso
                        If (m_oFormularioBodegaProceso IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioBodegaProceso, activarSiEstaAbierto:=True) Then
                                m_oFormularioBodegaProceso.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioBodegaProceso)
                            End If
                        End If
                    Case IDMenus.IDMenus.m_oFormularioSociosNegocios
                        If (m_oFormularioSociosNegocios IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioSociosNegocios, activarSiEstaAbierto:=True) Then
                                m_oFormularioSociosNegocios.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioSociosNegocios)
                            End If
                        End If
                    Case IDMenus.IDMenus.m_oFormularioReporteVehiculosRecurrentesTaller
                        If (m_oFormularioReporteVehiculosRecurrentesTaller IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioReporteVehiculosRecurrentesTaller, activarSiEstaAbierto:=True) Then
                                m_oFormularioReporteVehiculosRecurrentesTaller.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioReporteVehiculosRecurrentesTaller)
                            End If
                        End If
                    Case IDMenus.IDMenus.m_oFormularioReporteVentasXAsesorServicio
                        If (m_oFormularioReporteVentasXAsesorServicio IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioReporteVentasXAsesorServicio, activarSiEstaAbierto:=True) Then
                                m_oFormularioReporteVentasXAsesorServicio.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioReporteVentasXAsesorServicio)
                            End If
                        End If
                    Case IDMenus.IDMenus.m_oFormularioFacturacionvehiculo
                        If (m_oFormularioFacturacionvehiculo IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioFacturacionvehiculo, activarSiEstaAbierto:=True) Then
                                m_oFormularioFacturacionvehiculo.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioFacturacionvehiculo)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioOrdenesDeTrabajoPorEstado
                        If (m_oFormularioOrdenesDeTrabajoPorEstado IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioOrdenesDeTrabajoPorEstado, activarSiEstaAbierto:=True) Then
                                m_oFormularioOrdenesDeTrabajoPorEstado.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioOrdenesDeTrabajoPorEstado)
                            End If
                        End If
                    Case IDMenus.IDMenus.m_oFormularioHistorialVehiculo
                        If (m_oFormularioHistorialVehiculo IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioHistorialVehiculo, activarSiEstaAbierto:=True) Then
                                m_oFormularioHistorialVehiculo.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioHistorialVehiculo)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioReporteFacturacionOT
                        If (m_oFormularioReporteFacturacionOT IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioReporteFacturacionOT, activarSiEstaAbierto:=True) Then
                                m_oFormularioReporteFacturacionOT.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioReporteFacturacionOT)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioFactutacionOTInternas
                        If (m_oFormularioFactutacionOTInternas IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioFactutacionOTInternas, activarSiEstaAbierto:=True) Then
                                m_oFormularioFactutacionOTInternas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioFactutacionOTInternas)
                            End If
                        End If
                    Case IDMenus.IDMenus.m_oFormularioReporteAntiguedadVehiculos
                        If (m_oFormularioReporteAntiguedadVehiculos IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioReporteAntiguedadVehiculos, activarSiEstaAbierto:=True) Then
                                m_oFormularioReporteAntiguedadVehiculos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioReporteAntiguedadVehiculos)
                            End If
                        End If
                    Case IDMenus.IDMenus.m_oFormularioReporteServiciosExternosXOrden
                        If (m_oFormularioReporteServiciosExternosXOrden IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioReporteServiciosExternosXOrden, activarSiEstaAbierto:=True) Then
                                m_oFormularioReporteServiciosExternosXOrden.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioReporteServiciosExternosXOrden)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioReporteFinanciamientoContratoVentas
                        If (m_oFormularioReporteFinanciamientoContratoVentas IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioReporteFinanciamientoContratoVentas, activarSiEstaAbierto:=True) Then
                                m_oFormularioReporteFinanciamientoContratoVentas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioReporteFinanciamientoContratoVentas)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioReporteFacturaciondemecanico
                        If (mc_strUID_FORM_ReporteFacMecanicos IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioReporteFacturacionMecanicos, activarSiEstaAbierto:=True) Then
                                m_oFormularioReporteFacturacionMecanicos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioReporteFacturacionMecanicos)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioBusquedasCitas
                        If (m_oFormularioBusquedasCitas IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioBusquedasCitas, activarSiEstaAbierto:=True) Then
                                m_oFormularioBusquedasCitas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioBusquedasCitas)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioCargarPanelCitas
                        If (m_oFormularioCargarPanelCitas IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioCargarPanelCitas, activarSiEstaAbierto:=True) Then
                                m_oFormularioCargarPanelCitas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioCargarPanelCitas)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioSuspensionAgenda
                        If (m_oFormularioSuspensionAgenda IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioSuspensionAgenda, activarSiEstaAbierto:=True) Then
                                m_oFormularioSuspensionAgenda.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioSuspensionAgenda)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioPedidoVehiculos
                        If (m_oFormularioPedidoVehiculos IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioPedidoVehiculos, activarSiEstaAbierto:=True) Then
                                m_oFormularioPedidoVehiculos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioPedidoVehiculos)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioEntradaDeVehiculos
                        If (m_oFormularioEntradaDeVehiculos IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioEntradaDeVehiculos, activarSiEstaAbierto:=True) Then
                                m_oFormularioEntradaDeVehiculos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioEntradaDeVehiculos)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioCosteoDeEntradas
                        If (m_oFormularioCosteoDeEntradas IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioCosteoDeEntradas, activarSiEstaAbierto:=True) Then
                                m_oFormularioCosteoDeEntradas.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioCosteoDeEntradas)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioDevolucionDeVehiculos
                        If (m_oFormularioDevolucionDeVehiculos IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioDevolucionDeVehiculos, activarSiEstaAbierto:=True) Then
                                m_oFormularioDevolucionDeVehiculos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioDevolucionDeVehiculos)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oCosteoMultiplesUnidades
                        If (m_oCosteoMultiplesUnidades IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oCosteoMultiplesUnidades, activarSiEstaAbierto:=True) Then
                                m_oCosteoMultiplesUnidades.FormularioSBO = oGestorFormularios.CargaFormulario(m_oCosteoMultiplesUnidades)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oSalidasMultiplesUnidades
                        If (m_oSalidasMultiplesUnidades IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oSalidasMultiplesUnidades, activarSiEstaAbierto:=True) Then
                                m_oSalidasMultiplesUnidades.FormularioSBO = oGestorFormularios.CargaFormulario(m_oSalidasMultiplesUnidades)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioBusquedaOT
                        If (m_oFormularioBusquedaOT IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioBusquedaOT, activarSiEstaAbierto:=True) Then
                                m_oFormularioBusquedaOT.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioBusquedaOT)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioIncluirRepOT
                        If (m_oFormularioIncluirRepOT IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioIncluirRepOT, activarSiEstaAbierto:=True) Then
                                m_oFormularioIncluirRepOT.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioIncluirRepOT)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioAsocArticuloxEspecif
                        If (m_oFormularioAsocArticuloxEspecif IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioAsocArticuloxEspecif, activarSiEstaAbierto:=True) Then
                                m_oFormularioAsocArticuloxEspecif.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioAsocArticuloxEspecif)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioConfigNivelesAprob
                        If (m_oFormularioConfigNivelesAprob IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioConfigNivelesAprob, activarSiEstaAbierto:=True) Then
                                m_oFormularioConfigNivelesAprob.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioConfigNivelesAprob)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioIncluirGastoOT
                        If (m_oFormularioIncluirGastoOT IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioIncluirGastoOT, activarSiEstaAbierto:=True) Then
                                m_oFormularioIncluirGastoOT.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioIncluirGastoOT)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioBalanceOT
                        If (m_oFormularioBalanceOT IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioBalanceOT, activarSiEstaAbierto:=True) Then
                                m_oFormularioBalanceOT.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioBalanceOT)
                            End If
                        End If
                    Case IDMenus.IDMenus.m_oEmbarqueVehiculos
                        If (m_oEmbarqueVehiculos IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oEmbarqueVehiculos, activarSiEstaAbierto:=True) Then
                                m_oEmbarqueVehiculos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oEmbarqueVehiculos)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oAvaluoUsados
                        If (m_oFormularioAvaUs IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioAvaUs, activarSiEstaAbierto:=True) Then
                                m_oFormularioAvaUs.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioAvaUs)
                            End If
                        End If

                    Case IDMenus.IDMenus.g_oFormularioVisitas
                        If (g_oFormularioVisitas IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(g_oFormularioVisitas, activarSiEstaAbierto:=True) Then
                                g_oFormularioVisitas.FormularioSBO = oGestorFormularios.CargaFormulario(g_oFormularioVisitas)
                            End If
                        End If

                    Case IDMenus.IDMenus.g_oFormularioBusquedaControlProceso
                        If (g_oFormularioBusquedaControlProceso IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(g_oFormularioBusquedaControlProceso, activarSiEstaAbierto:=True) Then
                                g_oFormularioBusquedaControlProceso.FormularioSBO = oGestorFormularios.CargaFormulario(g_oFormularioBusquedaControlProceso)
                            End If
                        End If

                    Case IDMenus.IDMenus.g_oFormularioControlCrearVisita
                        If (g_oFormularioControlCrearVisita IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(g_oFormularioControlCrearVisita, activarSiEstaAbierto:=True) Then
                                g_oFormularioControlCrearVisita.FormularioSBO = oGestorFormularios.CargaFormulario(g_oFormularioControlCrearVisita)
                            End If
                        End If

                    Case IDMenus.IDMenus.g_oFormularioControlVisita
                        If (g_oFormularioControlVisita IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(g_oFormularioControlVisita, activarSiEstaAbierto:=True) Then
                                g_oFormularioControlVisita.FormularioSBO = oGestorFormularios.CargaFormulario(g_oFormularioControlVisita)
                            End If
                        End If

                    Case IDMenus.IDMenus.g_oFormularioOfertaVentas
                        If (g_oFormularioOfertaVentas IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(g_oFormularioOfertaVentas, activarSiEstaAbierto:=True) Then
                                g_oFormularioOfertaVentas.FormularioSBO = oGestorFormularios.CargaFormulario(g_oFormularioOfertaVentas)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oSolicitudOTEspecial
                        If (m_oSolicitudOTEspecial IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oSolicitudOTEspecial, activarSiEstaAbierto:=True) Then
                                m_oSolicitudOTEspecial.FormularioSBO = oGestorFormularios.CargaFormulario(m_oSolicitudOTEspecial)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oReporteOrdenesEspeciales
                        If (m_oReporteOrdenesEspeciales IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oReporteOrdenesEspeciales, activarSiEstaAbierto:=True) Then
                                m_oReporteOrdenesEspeciales.FormularioSBO = oGestorFormularios.CargaFormulario(m_oReporteOrdenesEspeciales)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oDimensionesContables
                        If (m_oDimensionesContables IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oDimensionesContables, activarSiEstaAbierto:=True) Then
                                m_oDimensionesContables.FormularioSBO = oGestorFormularios.CargaFormulario(m_oDimensionesContables)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oDimensionesContablesOTs
                        If (m_oDimensionesContablesOTs IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oDimensionesContablesOTs, activarSiEstaAbierto:=True) Then
                                m_oDimensionesContablesOTs.FormularioSBO = oGestorFormularios.CargaFormulario(m_oDimensionesContablesOTs)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oFormularioOrdenTrabajo
                        If (m_oFormularioOrdenTrabajo IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioOrdenTrabajo, activarSiEstaAbierto:=True) Then
                                m_oFormularioOrdenTrabajo.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioOrdenTrabajo)
                            End If
                        End If
                    Case IDMenus.IDMenus.m_oFormularioKardexInventarioVehiculo
                        If (m_oFormularioKardexInventarioVehiculo IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oFormularioKardexInventarioVehiculo, activarSiEstaAbierto:=True) Then
                                m_oFormularioKardexInventarioVehiculo.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioKardexInventarioVehiculo)
                            End If
                        End If

                    Case IDMenus.IDMenus.FormularioAgendaSBO
                        If Not ValidarSiFormularioAbierto(FormularioAgendaSBO.FormType, True) AndAlso m_blnUsaOrdenesDeTrabajo Then
                            m_oAgendas.CargarFormulario()
                        End If

                    Case IDMenus.IDMenus._menuDMS
                        EjecutarAddonDMS(System.Windows.Forms.Application.StartupPath & "\" & _nombreExeDMS)

                    Case IDMenus.IDMenus.m_oListaCVXUnidad

                        If Not ValidarSiFormularioAbierto(mc_strUIListaContXUnidad, False) Then

                            Call m_oListaCVXUnidad.CargaFormularioListaContXUnidad()

                        End If

                    Case IDMenus.IDMenus.m_oVehiculos 'Datos Maestros vehículo

                        If Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then

                            Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", "", False, "", 0, False, True, VehiculosCls.ModoFormulario.scgVentas)

                        End If

                    Case IDMenus.IDMenus.m_oVehiculosServicio 'Datos Maestros vehículo (Servicio)

                        If Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then

                            Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", "", False, "", 0, False, True, VehiculosCls.ModoFormulario.scgTaller)

                        End If

                    Case IDMenus.IDMenus.mc_strUIDSubGeneraOV  'Generacion Orden de Venta

                        If Not ValidarSiFormularioAbierto(mc_strGeneraOV, False) Then

                            Call m_oCotizacion.CargaFormularioGeneraOV()

                        End If

                    Case IDMenus.IDMenus.mc_strUIDSubGeneraFI  'Generacion Orden de Venta

                        If Not ValidarSiFormularioAbierto(mc_strGeneraFI, False) Then

                            Call m_oCotizacion.CargaFormularioGeneraFI()

                        End If

                    Case IDMenus.IDMenus.m_oCVentaCrear 'Crear Contrato venta

                        If Not ValidarSiFormularioAbierto("SCGD_frmContVent", False) Then

                            Call m_oCVenta.DibujarFormularioContratoVentas("", False, True)

                        End If

                    Case IDMenus.IDMenus.m_oCVenta 'Contratos de Venta

                        If Not ValidarSiFormularioAbierto("SCGD_frmContVent", False) Then

                            Call m_oCVenta.DibujarFormularioContratoVentas("", False)

                        End If

                    Case IDMenus.IDMenus.mc_strUIDCV_Listado

                        If Not ValidarSiFormularioAbierto(mc_strUniqueIDLCV, False) Then

                            Call m_oListadoCV.CargaFormularioListadoCV()

                        End If

                        '*********************************************************************************
                    Case IDMenus.IDMenus.mc_strUIDCV_ListadoReversados

                        If Not ValidarSiFormularioAbierto("SCGD_Revertir_", False) Then

                            Call m_oListadoContratosReversados.CargaFormularioListadoContRevertidos()

                        End If
                        '*********************************************************************************
                    Case IDMenus.IDMenus.m_oTrasladoCostos
                        'Traslado de costos entre unidades...
                        If Not ValidarSiFormularioAbierto("SCGD_TCU_", False) Then

                            Call m_oTrasladoCostos.CargaFormularioTrasladoCostos()

                        End If
                        '*********************************************************************************
                    Case IDMenus.IDMenus.mc_strUIDCV_ListaARevertir
                        'mc_strUIDCV_ListaARevertir
                        If Not ValidarSiFormularioAbierto(mc_strUniqueIDLCVLAR, False) Then
                            Call m_oListaContratos_a_Reversar.CargaFormularioListadoCV()
                        End If

                    Case IDMenus.IDMenus.mc_strUIDCV_ListaSegurosPV
                        'mc_strUIDCV_ListaARevertir
                        If Not ValidarSiFormularioAbierto(mc_strUniqueIDConSegPV, False) Then
                            Call m_oListaContratosSegPV.CargaFormularioListadoCV()
                        End If
                        '*********************************************************************************

                    Case IDMenus.IDMenus.mc_strUIDGood_Receive

                        If Not ValidarSiFormularioAbierto(mc_strUIGOODENT, False) Then


                            Call m_oGoodReceive.CargaFormularioGoodReceive("", "", "", "", "", "", "", "", "")

                        End If
                    Case IDMenus.IDMenus.mc_strUIDGood_Issue

                        If Not ValidarSiFormularioAbierto(mc_strUIGOODISSUE, False) Then

                            Call m_oGoodIssue.CargaFormularioGoodIssue("")

                        End If

                    Case IDMenus.IDMenus.mc_strUIDFac_Interna


                        If Not ValidarSiFormularioAbierto(mc_strUIFacturasInt, False) Then

                            Call m_oFacturaInterna.CargaFormulario()

                        End If

                    Case IDMenus.IDMenus.mc_strUIDListadoGR

                        If Not ValidarSiFormularioAbierto(mc_strUILISTADOGR, False) Then

                            Call m_oListadoGR.CargaFormularioListadoGR()

                        End If

                    Case IDMenus.IDMenus.mc_strUIDListadoRecosteos

                        If Not ValidarSiFormularioAbierto(mc_strUIRecosteos, False) Then

                            Call m_oRecosteos.CargaFormularioListadoGR()

                        End If

                    Case IDMenus.IDMenus.mc_strUIDVehiculosCostear

                        If Not ValidarSiFormularioAbierto(mc_strUniqueIDVSC, False) Then

                            Call m_oVehiculosACostear.CargaFormularioVehiculosSinCostear()

                        End If


                    Case IDMenus.IDMenus.mc_strUIDReportes
                        If Not ValidarSiFormularioAbierto(ReportesCosteoCls.mc_strFormID, False) Then
                            m_oReportesCosteo.CargaFormulario()
                        End If

                    Case IDMenus.IDMenus.mc_strUIDInventarioVehiculos
                        If Not ValidarSiFormularioAbierto("SCGD_INV_VEHI", True) Then
                            m_oInventarioVehiculos.CargarFormulario()
                        End If
                        'Case IDMenus.IDMenus.strMenuCargaMasivaVehiculos
                        '    m_oFormularioCargaMasivaVehiculos.CargarFormulario()
                    Case IDMenus.IDMenus.strMenuBuscar    'Menú Buscar

                        Select Case SBO_Application.Forms.ActiveForm.TypeEx

                            'Case "SCGD_TCU"
                            '    Call m_oTrasladoCostos.MenuEvent(pVal, BubbleEvent)

                            Case mc_strUISCGD_FormPlacas
                                m_oFormularioPlacas.PermisosPlacas()

                            Case mc_strUISCGD_FormPrestamo
                                m_oFormularioPrestamo.LimpiarPago()
                                m_oFormularioPrestamo.ButtonAbonar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                                m_oFormularioPrestamo.ButtonReversar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

                            Case mc_strIdFormaCotizacion.ToString()

                                'Erick Sanabria 10.09.2013. Corrección en la Oferta de Venta cuando se oprime busqueda o nuevo y 
                                'el usuario no está en Sucursal de Taller.
                                m_strUsuario = Trim(SBO_Application.Company.UserName.ToString())
                                m_strConsulta = "Select [Name] " &
                                                "From dbo.OUSR USR Inner Join [dbo].[@SCGD_SUCURSALES] SUC " &
                                                "On USR.Branch=SUC.Code " &
                                                "Where USR.USER_CODE='" & m_strUsuario & "'"

                                m_strSucursalTaller = Utilitarios.EjecutarConsulta(m_strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)

                                If (m_strSucursalTaller <> "") Then
                                    m_oRecepcionVHUI.SeleccionarFolderInicial()
                                End If

                                'Case mc_strControlCVenta

                            Case mc_strUIGOODENT
                                Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item(mc_strUIGOODENT), True)
                                Utilitarios.FormularioDeshabilitado(SBO_Application.Forms.Item(mc_strUIGOODENT), True)
                                Dim obItem As Item = DirectCast(SBO_Application.Forms.Item(mc_strUIGOODENT).Items.Item("1"), Item)
                                obItem.Enabled = True
                                Dim obItemGenerar As Item = DirectCast(SBO_Application.Forms.Item(mc_strUIGOODENT).Items.Item("btn_Genera"), Item)
                                obItemGenerar.Enabled = True
                            Case mc_strUIGOODISSUE
                                Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item(mc_strUIGOODISSUE), True)
                                Utilitarios.FormularioDeshabilitado(SBO_Application.Forms.Item(mc_strUIGOODISSUE), True)
                                Dim editFormatCode As EditText = DirectCast(SBO_Application.Forms.Item(mc_strUIGOODISSUE).Items.Item("txtFormatC").Specific, EditText)
                                Dim editDescripcion As EditText = DirectCast(SBO_Application.Forms.Item(mc_strUIGOODISSUE).Items.Item("txtDscp").Specific, EditText)
                                editFormatCode.Value = String.Empty
                                editDescripcion.Value = String.Empty
                                m_oGoodIssue.HabilitarBoton1(SBO_Application.Forms.ActiveForm)


                            Case mc_strUIFacturasInt
                                Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item(mc_strUIFacturasInt), True)
                                Utilitarios.FormularioDeshabilitado(SBO_Application.Forms.Item(mc_strUIFacturasInt), True)

                            Case mc_strUniqueIDPropiedades
                                Utilitarios.FormularioDeshabilitado(SBO_Application.Forms.Item(mc_strUniqueIDPropiedades), True)

                            Case mc_strControlCVenta 'Contrato de Venta
                                m_oCVenta.CargarFormularioActual()
                                Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item("SCGD_frmContVent"), True)

                            Case "SCGD_DET_1"
                                Call m_oVehiculos.HabilitarCombos(SBO_Application.Forms.Item("SCGD_DET_1"), VehiculosCls.mc_strMarca)
                                Call m_oVehiculos.HabilitarCombos(SBO_Application.Forms.Item("SCGD_DET_1"), VehiculosCls.mc_strEstilo)
                                Call m_oVehiculos.HabilitarCombos(SBO_Application.Forms.Item("SCGD_DET_1"), VehiculosCls.mc_strModelo)
                                Call m_oVehiculos.ManejarModoFormulario(SBO_Application.Forms.Item("SCGD_DET_1"))
                            Case mc_strUniqueIDLineasFactura
                                Utilitarios.FormularioDeshabilitado(SBO_Application.Forms.Item(mc_strUniqueIDLineasFactura), True)

                            Case mc_strUniqueIDLineasDesgloce
                                Utilitarios.FormularioDeshabilitado(SBO_Application.Forms.Item(mc_strUniqueIDLineasDesgloce), True)

                            Case mc_strUniqueIDTransaccionesCompras
                                Utilitarios.FormularioDeshabilitado(SBO_Application.Forms.Item(mc_strUniqueIDTransaccionesCompras), True)

                            Case mc_strUITrasC
                                SBO_Application.Forms.Item("SCGD_TCU_").Items.Item("txtDocEnt").Enabled = True
                                SBO_Application.Forms.Item("SCGD_TCU_").Items.Item("1").Visible = True
                                SBO_Application.Forms.Item("SCGD_TCU_").DataBrowser.BrowseBy = "txtDocEnt"

                            Case mc_strCampana

                                m_oCampana.LimpiaInfoCampanasDMS()

                            Case mc_strIdFormaCotizacion

                                m_oCotizacion.blnValidarCamposHS_KM = False

                            Case g_strFormOT

                                m_oFormularioOrdenTrabajo.ManejadorEventoMenuEvent(False, True)

                            Case "SCGD_CIT"
                                Call m_oFormularioCitas.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case "SCGD_CCIT"
                                ControladorCitas.MenuEvent(SBO_Application.Forms.ActiveForm.TypeEx, SBO_Application.Forms.ActiveForm.UniqueID, pVal, BubbleEvent)
                            Case "SCGD_AGD"
                                Call m_oFormularioAgendasConfiguracion.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case "SCGD_SDA"
                                Call m_oFormularioSuspensionAgenda.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strEntradaDeVehiculos
                                Call m_oFormularioEntradaDeVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strCosteoDeEntradas
                                Call m_oFormularioCosteoDeEntradas.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_StrPedidoVehiculos
                                '6925
                                Call m_oFormularioPedidoVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strDevolucionDeVehiculos
                                Call m_oFormularioDevolucionDeVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strDimensionesContables
                                Call m_oDimensionesContables.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strDimensionesContablesOTs
                                Call m_oDimensionesContablesOTs.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                        End Select

                    Case IDMenus.IDMenus.strMenuNuevo 'Menu nuevo

                        Select Case SBO_Application.Forms.ActiveForm.TypeEx
                            'Case "SCGD_TCU"
                            '    Call m_oTrasladoCostos.MenuEvent(pVal, BubbleEvent)
                            Case "-" + mc_strUISCGD_FormAVA
                                Call m_oFormularioAvaUs.ManejadorEventosMenus(pVal, BubbleEvent)
                            Case mc_strUISCGD_FormAVA
                                Call m_oFormularioAvaUs.ManejadorEventosMenus(pVal, BubbleEvent)
                            Case mc_strUISCGD_FormPlacas

                                m_strSucursalTaller = Utilitarios.EjecutarConsulta(m_strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)

                                If (m_strSucursalTaller <> "") Then
                                    m_oFormularioPlacas.PermisosPlacas()
                                End If

                            Case mc_strUIGOODISSUE

                                Dim editFormatCode As EditText = DirectCast(SBO_Application.Forms.Item(mc_strUIGOODISSUE).Items.Item("txtFormatC").Specific, EditText)
                                Dim editDescripcion As EditText = DirectCast(SBO_Application.Forms.Item(mc_strUIGOODISSUE).Items.Item("txtDscp").Specific, EditText)
                                editFormatCode.Value = String.Empty
                                editDescripcion.Value = String.Empty
                                m_oGoodIssue.DesHabilitarBoton1(SBO_Application.Forms.ActiveForm)

                            Case mc_strIdFormaCotizacion.ToString()
                                oForm = SBO_Application.Forms.ActiveForm

                                'Erick Sanabria 10.09.2013. Corrección en la Oferta de Venta cuando se oprime busqueda o nuevo y 
                                'el usuario no está en Sucursal de Taller. 
                                m_strUsuario = Trim(SBO_Application.Company.UserName.ToString())
                                m_strConsulta = "Select [Name] " &
                                                "From dbo.OUSR USR Inner Join [dbo].[@SCGD_SUCURSALES] SUC " &
                                                "On USR.Branch=SUC.Code " &
                                                "Where USR.USER_CODE='" & m_strUsuario & "'"

                                If (m_strSucursalTaller <> "") _
                                   Then
                                    m_oRecepcionVHUI.AsignaValoresdeRecepcionControlesUIDefecto(oForm)
                                End If

                                Dim oMatrix As SAPbouiCOM.Matrix
                                oMatrix = DirectCast(oForm.Items.Item("38").Specific, SAPbouiCOM.Matrix)

                                If oMatrix.Columns.Item("U_SCGD_Aprobado").Visible Then
                                    oMatrix.Columns.Item("U_SCGD_Aprobado").Editable = True
                                End If

                                Call m_oCotizacion.ManejadorEventoMenu(SBO_Application.Forms.ActiveForm, pVal, BubbleEvent)

                            Case mc_strUniqueIDCV
                                m_oCVenta.NuevoContrato()

                            Case "SCGD_DET_1"
                                Call m_oVehiculos.HabilitarCombos(SBO_Application.Forms.Item("SCGD_DET_1"), VehiculosCls.mc_strMarca)
                                Call m_oVehiculos.HabilitarCombos(SBO_Application.Forms.Item("SCGD_DET_1"), VehiculosCls.mc_strEstilo)
                                Call m_oVehiculos.HabilitarCombos(SBO_Application.Forms.Item("SCGD_DET_1"), VehiculosCls.mc_strModelo)
                                Call m_oVehiculos.ManejarModoFormulario(SBO_Application.Forms.Item("SCGD_DET_1"))
                                Call m_oVehiculos.AgregaLineaMatrizBonos(SBO_Application.Forms.Item("SCGD_DET_1"))

                                Dim strMonedaDefecto As String = Utilitarios.EjecutarConsulta("Select U_Mon_Def From [@SCGD_ADMIN] where Code = 'DMS'", m_oCompany.CompanyDB, m_oCompany.Server)
                                If Not String.IsNullOrEmpty(strMonedaDefecto) Then
                                    SBO_Application.Forms.Item("SCGD_DET_1").DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_Moneda", 0, strMonedaDefecto)
                                End If

                            Case mc_strUniqueIDLineasFactura
                                m_oLineasFactura.HabilitarCampos(mc_strUniqueIDLineasFactura)

                            Case mc_strUniqueIDLineasDesgloce
                                m_oLineasDesgloce.HabilitarCampos(mc_strUniqueIDLineasDesgloce)

                            Case mc_strUniqueIDTransaccionesCompras
                                Utilitarios.FormularioDeshabilitado(SBO_Application.Forms.Item(mc_strUniqueIDTransaccionesCompras), True)
                                m_oTransaccionesCompras.HabilitarCampos(mc_strUniqueIDTransaccionesCompras, True)

                            Case mc_strUniqueIDPropiedades
                                Utilitarios.FormularioDeshabilitado(SBO_Application.Forms.Item(mc_strUniqueIDPropiedades), True)

                                'Agregado 05/11/2010: Maneja estado de edit de contrato de venta en salida de mercancia
                            Case CStr(mc_strSalidaMercancia)
                                oForm = SBO_Application.Forms.ActiveForm
                                m_oSalidaMercancia.ManejarEstado(oForm)

                                'Agregado 13/12/2010: Maneja estado de edit de contrato de venta en entrada de mercancia
                            Case CStr(mc_strEntradaMercancia)
                                oForm = SBO_Application.Forms.ActiveForm
                                m_oEntradaMercancia.ManejarEstado(oForm)

                            Case mc_strUITrasC
                                SBO_Application.Forms.Item("SCGD_TCU_").Items.Item("txtDocEnt").Enabled = False
                                SBO_Application.Forms.Item("SCGD_TCU_").Items.Item("1").Visible = False
                            Case mc_strCampana

                                m_oCampana.LimpiaInfoCampanasDMS()

                            Case g_strFormOT

                                m_oFormularioOrdenTrabajo.ManejadorEventoMenuEvent(True, False)
                            Case "SCGD_CIT", "-SCGD_CIT"
                                Call m_oFormularioCitas.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case "SCGD_CCIT"
                                ControladorCitas.MenuEvent(SBO_Application.Forms.ActiveForm.TypeEx, SBO_Application.Forms.ActiveForm.UniqueID, pVal, BubbleEvent)
                            Case "SCGD_AGD"
                                Call m_oFormularioAgendasConfiguracion.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case "SCGD_SDA"
                                Call m_oFormularioSuspensionAgenda.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strEntradaDeVehiculos
                                Call m_oFormularioEntradaDeVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strCosteoDeEntradas
                                Call m_oFormularioCosteoDeEntradas.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_StrPedidoVehiculos
                                '6925
                                Call m_oFormularioPedidoVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strDevolucionDeVehiculos
                                Call m_oFormularioDevolucionDeVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strDimensionesContables
                                Call m_oDimensionesContables.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strDimensionesContablesOTs
                                Call m_oDimensionesContablesOTs.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                        End Select

                    Case IDMenus.IDMenus.strMenuEliminar
                        Select Case SBO_Application.Forms.ActiveForm.TypeEx

                            Case mc_strUIGOODISSUE
                        End Select

                    Case IDMenus.IDMenus.strMenuCancelar 'Menu Cancelar
                        If SBO_Application.Forms.ActiveForm.TypeEx = "142" And m_blnOrdenCompraActualizada Then

                            SBO_Application.Forms.GetForm("0", 1)

                            'm_oCompras.ManejarDocumentoACancelar()
                            'm_blnOrdenCompraActualizada = False

                        End If
                        Select Case SBO_Application.Forms.ActiveForm.TypeEx
                            Case "142"
                                OrdenCompra.CancelarCompra(SBO_Application.Forms.ActiveForm.UniqueID, pVal, BubbleEvent)
                            Case "540000988"
                                OfertaCompra.CancelarCompra(SBO_Application.Forms.ActiveForm.UniqueID, pVal, BubbleEvent)
                        End Select
                    Case IDMenus.IDMenus.strMenuCerrar
                        Select Case SBO_Application.Forms.ActiveForm.TypeEx
                            Case "142"
                                OrdenCompra.CancelarCompra(SBO_Application.Forms.ActiveForm.UniqueID, pVal, BubbleEvent)
                            Case "540000988"
                                OfertaCompra.CancelarCompra(SBO_Application.Forms.ActiveForm.UniqueID, pVal, BubbleEvent)
                        End Select
                    Case IDMenus.IDMenus.m_oPermisos
                        If (m_oNivelesPV IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(m_oNivelesPV, activarSiEstaAbierto:=True) Then
                                m_oNivelesPV.FormularioSBO = oGestorFormularios.CargaFormulario(m_oNivelesPV)
                            End If
                        End If

                    Case IDMenus.IDMenus.m_oPropiedades
                        If Not ValidarSiFormularioAbierto("SCGD_PROP", False) Then
                            m_oPropiedades.CargaFormulario()
                        End If

                    Case IDMenus.IDMenus.m_oLineasFactura
                        If Not ValidarSiFormularioAbierto("SCGD_ConfLineasSum", False) Then
                            m_oLineasFactura.CargaFormulario()
                        End If

                    Case IDMenus.IDMenus.m_oLineasDesgloce
                        If Not ValidarSiFormularioAbierto("SCGD_Adic_Des", False) Then
                            m_oLineasDesgloce.CargaFormulario()
                        End If

                    Case IDMenus.IDMenus.m_oTransaccionesCompras
                        If Not ValidarSiFormularioAbierto(mc_strUniqueIDTransaccionesCompras, False) Then
                            m_oTransaccionesCompras.CargaFormulario()
                        End If

                    Case IDMenus.IDMenus.m_oConfiguracionGeneral
                        If Not ValidarSiFormularioAbierto(mc_strUniqueIDConfiguracionesGenerales, False) Then
                            m_oConfiguracionGeneral.CargaFormulario()
                        End If

                    Case IDMenus.IDMenus.mc_strUIDGeneradorRepCV

                        'si no se ha abierto el formulario de reportes de cv se abre
                        If Not ValidarSiFormularioAbierto("SCGD_REP_CV", True) Then
                            Call m_oReporteCV.CargarFormularioReportes()
                        End If

                    Case IDMenus.IDMenus.mc_strUIDEstadosOT

                        'si no se ha abierto el formulario de reportes de cv se abre
                        If Not ValidarSiFormularioAbierto("SCGD_ESTOT", True) Then
                            Call m_oEstadosOT.CargarFormulario()
                        End If

                    Case IDMenus.IDMenus.strMenuBorrarLinea
                        'maneja el borrar linea 
                        oForm = SBO_Application.Forms.ActiveForm
                        If oForm.Type = mc_strOrdenDeCompra Then
                            'maneja el borrar linea de orden de compra
                            m_oComprasEnVentas.IngresaListaAccEliminar(BubbleEvent)
                        End If

                    Case IDMenus.IDMenus.mc_strUIDVendedoresTipoInv

                        'si no se ha abierto el formulario de reportes de cv se abre
                        If Not ValidarSiFormularioAbierto("SCGD_VENDXTI", True) Then
                            Call m_oFormularioPermisosVendedoresXTI.CargarFormularioVendedoresTipoInventario()
                        End If

                    Case IDMenus.IDMenus.strMenuPrimerRegistroDatos
                        Select Case SBO_Application.Forms.ActiveForm.TypeEx
                            Case "SCGD_CIT"
                                Call m_oFormularioCitas.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case "SCGD_AGD"
                                Call m_oFormularioAgendasConfiguracion.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case "SCGD_SDA"
                                Call m_oFormularioSuspensionAgenda.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strEntradaDeVehiculos
                                Call m_oFormularioEntradaDeVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strCosteoDeEntradas
                                Call m_oFormularioCosteoDeEntradas.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_StrPedidoVehiculos
                                '6925
                                Call m_oFormularioPedidoVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strDevolucionDeVehiculos
                                Call m_oFormularioDevolucionDeVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strIdFormaCotizacion
                                m_oCotizacion.blnValidarCamposHS_KM = False
                            Case mc_strDimensionesContables
                                Call m_oDimensionesContables.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strDimensionesContablesOTs
                                Call m_oDimensionesContablesOTs.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                        End Select

                    Case IDMenus.IDMenus.strMenuRegistroDatosSiguiente
                        Select Case SBO_Application.Forms.ActiveForm.TypeEx
                            Case "SCGD_CIT"
                                Call m_oFormularioCitas.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case "SCGD_AGD"
                                Call m_oFormularioAgendasConfiguracion.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case "SCGD_SDA"
                                Call m_oFormularioSuspensionAgenda.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strEntradaDeVehiculos
                                Call m_oFormularioEntradaDeVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strCosteoDeEntradas
                                Call m_oFormularioCosteoDeEntradas.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_StrPedidoVehiculos
                                '6925
                                Call m_oFormularioPedidoVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strDevolucionDeVehiculos
                                Call m_oFormularioDevolucionDeVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strIdFormaCotizacion
                                m_oCotizacion.blnValidarCamposHS_KM = False
                            Case mc_strDimensionesContables
                                Call m_oDimensionesContables.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strDimensionesContablesOTs
                                Call m_oDimensionesContablesOTs.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                        End Select

                    Case IDMenus.IDMenus.strMenuRegistroDatosAnterior
                        Select Case SBO_Application.Forms.ActiveForm.TypeEx
                            Case "SCGD_CIT"
                                Call m_oFormularioCitas.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case "SCGD_AGD"
                                Call m_oFormularioAgendasConfiguracion.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case "SCGD_SDA"
                                Call m_oFormularioSuspensionAgenda.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strEntradaDeVehiculos
                                Call m_oFormularioEntradaDeVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strCosteoDeEntradas
                                Call m_oFormularioCosteoDeEntradas.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_StrPedidoVehiculos
                                '6925
                                Call m_oFormularioPedidoVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strDevolucionDeVehiculos
                                Call m_oFormularioDevolucionDeVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strIdFormaCotizacion
                                m_oCotizacion.blnValidarCamposHS_KM = False
                            Case mc_strDimensionesContables
                                Call m_oDimensionesContables.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strDimensionesContablesOTs
                                Call m_oDimensionesContablesOTs.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                        End Select

                    Case IDMenus.IDMenus.strMenuUltimoRegistroDatos
                        Select Case SBO_Application.Forms.ActiveForm.TypeEx
                            Case "SCGD_CIT"
                                Call m_oFormularioCitas.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case "SCGD_AGD"
                                Call m_oFormularioAgendasConfiguracion.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case "SCGD_SDA"
                                Call m_oFormularioSuspensionAgenda.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strEntradaDeVehiculos
                                Call m_oFormularioEntradaDeVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strCosteoDeEntradas
                                Call m_oFormularioCosteoDeEntradas.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_StrPedidoVehiculos
                                '6925
                                Call m_oFormularioPedidoVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strDevolucionDeVehiculos
                                Call m_oFormularioDevolucionDeVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strIdFormaCotizacion
                                m_oCotizacion.blnValidarCamposHS_KM = False
                            Case mc_strDimensionesContables
                                Call m_oDimensionesContables.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strDimensionesContablesOTs
                                Call m_oDimensionesContablesOTs.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                        End Select
                    Case IDMenus.IDMenus.MenuDisponibilidadEmpleados
                        ConstructorDisponibilidadEmpleados.CrearInstanciaFormulario()
                    Case IDMenus.IDMenus.MenuCitas
                        ConstructorCitas.CrearInstanciaFormulario()
                    Case IDMenus.IDMenus.MenuRestablecerCantidadesPendientes
                        ConstructorRestablecerCantidadesPendientes.CrearInstanciaFormulario()
                    Case IDMenus.IDMenus.MenuReporteAuditoriaBodegaReservas
                        ConstructorReporteBodegaReservas.CrearInstanciaFormulario()
                    Case IDMenus.IDMenus.MenuLicenseManager
                        AdministradorLicencias.AbrirFormulario()
                    Case IDMenus.IDMenus.strMenuReAperturaOT
                        ReAperturaOT.AbrirFormulario()

                    Case IDMenus.IDMenus.MenuInterfaceJohnDeere
                        InterfaceJohnDeereModulo.AbrirFormulario()
                    Case IDMenus.IDMenus.MenuConfigurationJohnDeere
                        InterfaceJohnDeereConfiguration.AbrirFormulario()
                    Case Else

                        If m_udoMenusPlanVentas IsNot Nothing Then
                            If m_udoMenusPlanVentas.ContainsKey(pVal.MenuUID) Then

                                If Not ValidarSiFormularioAbierto(mc_strUniqueIDBCV, False) Then

                                    m_udoMenusPlanVentas.TryGetValue(pVal.MenuUID, m_udoMenu)

                                    m_oBuscadorCV.EstadoFormulario = CInt(m_udoMenu.intNivel)
                                    m_oBuscadorCV.UsaEmpleado = m_udoMenu.blnPorEmpleado

                                    Call m_oBuscadorCV.CargaFormularioBusquedaCV()

                                End If

                            End If
                        End If

                End Select

            Else
                'Before action 
                Select Case pVal.MenuUID
                    Case IDMenus.IDMenus.strMenuBorrarLinea  'Menú Eliminar línea
                        If SBO_Application.Forms.ActiveForm IsNot Nothing Then
                            oForm = SBO_Application.Forms.ActiveForm
                            Select Case oForm.Type
                                Case mc_strIdFormaCotizacion
                                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                        If m_intRowOT > 0 AndAlso Not blnFilaTieneOT Then
                                            blnFilaTieneOT = m_oCotizacion.FilaTieneNumeroOT(oForm, m_intRowOT)
                                            m_intRowOT = 0
                                        End If
                                        If blnFilaTieneOT Then
                                            BubbleEvent = False
                                            SBO_Application.StatusBar.SetText(My.Resources.Resource.EliminarLineasCotizacion, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                            blnFilaTieneOT = False
                                        End If
                                    End If
                                    ValidaEliminarLineas(oForm, BubbleEvent)
                                Case mc_strOportunidadVenta
                                    m_oOportunidadVenta.ManejoEventosMenu(oForm, pVal, BubbleEvent)

                                Case mc_strOrdenDeVenta
                                    'm_oOrdenVenta.FilaTieneNumeroOT(oForm, pVal, BubbleEvent)
                                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                        If m_intRowOT > 0 AndAlso Not blnFilaTieneOT Then
                                            blnOVFilaTieneOT = m_oOrdenVenta.FilaTieneNumeroOT(oForm, m_intRowOT, mc_strIDMatriz, m_NumOT_OV)
                                            m_intRowOT = 0
                                        End If
                                        If blnOVFilaTieneOT Then
                                            BubbleEvent = False
                                            Dim strErrorMessage As String = String.Format(My.Resources.Resource.ErrorEliminaLineaConOT, m_NumOT_OV)
                                            SBO_Application.StatusBar.SetText(strErrorMessage, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            blnOVFilaTieneOT = False
                                        End If
                                    End If

                                Case mc_strOrdenDeCompra
                                    'maneja el borrar linea de orden de compra en el Before Action 
                                    BubbleEvent = m_oComprasEnVentas.ValidaNumeroLineas(oForm)

                            End Select
                        End If

                    Case IDMenus.IDMenus.strMenuCancelar

                        Select Case SBO_Application.Forms.ActiveForm.TypeEx
                            Case "142"
                                m_oComprasEnVentas.CancelarOc = True
                                OrdenCompra.CancelarCompra(SBO_Application.Forms.ActiveForm.UniqueID, pVal, BubbleEvent)
                            Case mc_StrPedidoVehiculos
                                Call m_oFormularioPedidoVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strEntradaDeVehiculos
                                Call m_oFormularioEntradaDeVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case SBO_Application.Forms.ActiveForm.TypeEx = mc_strCosteoDeEntradas
                                Call m_oFormularioCosteoDeEntradas.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strPagoRecibido
                                Call m_oPagoRecibido.ManejadorEventoMenu(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                                Exit Sub

                            Case mc_strIdFormaCotizacion.ToString()

                                Call m_oCotizacion.PermitirCancelar(SBO_Application.Forms.ActiveForm.UniqueID, BubbleEvent)
                            Case mc_strOrdenDeVenta
                                m_oOrdenVenta.PermitirCancelar(SBO_Application.Forms.ActiveForm.UniqueID, BubbleEvent)
                            Case "142"
                                m_oCompras.ObtieneNumeroDocumentoACancelar()

                            Case mc_strContratoVenta

                                If SBO_Application.MessageBox(My.Resources.Resource.PreguntaCancelarContrato, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then

                                    BubbleEvent = False

                                Else

                                    Call m_oCVenta.ManejadorEventoMenuCancelar(pVal, SBO_Application.Forms.ActiveForm)

                                End If
                            Case "540000988"
                                OfertaCompra.CancelarCompra(SBO_Application.Forms.ActiveForm.UniqueID, pVal, BubbleEvent)
                        End Select

                    Case IDMenus.IDMenus.strMenuCerrar
                        Select Case SBO_Application.Forms.ActiveForm.TypeEx
                            Case mc_strIdFormaCotizacion.ToString()

                                m_oCotizacion.PermitirCancelar(SBO_Application.Forms.ActiveForm.UniqueID, BubbleEvent)
                            Case "142"
                                m_oCompras.ObtieneNumeroDocumentoACancelar()
                                OrdenCompra.CancelarCompra(SBO_Application.Forms.ActiveForm.UniqueID, pVal, BubbleEvent)
                            Case "540000988"
                                OfertaCompra.CancelarCompra(SBO_Application.Forms.ActiveForm.UniqueID, pVal, BubbleEvent)
                        End Select
                    Case IDMenus.IDMenus.strMenuBuscar
                        Select Case SBO_Application.Forms.ActiveForm.TypeEx
                            Case "SCGD_MSJS"
                                Call m_oFormularioConfigNivelesAprob.ManejadorEventoMenuBuscar(pVal, SBO_Application.Forms.ActiveForm)
                            Case mc_strContratoVenta
                                Call m_oCVenta.ManejadorEventosMenusBefore(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                            Case mc_strUIGOODISSUE
                                m_oGoodIssue.DesHabilitarBoton1(SBO_Application.Forms.ActiveForm)
                            Case mc_strIdFormaCotizacion
                                Call m_oCotizacion.ManejadorEventoMenu(SBO_Application.Forms.ActiveForm, pVal, BubbleEvent)
                                Exit Sub
                        End Select

                    Case IDMenus.IDMenus.strMenuNuevo 'Nuevo
                        Select Case SBO_Application.Forms.ActiveForm.TypeEx
                            Case mc_strOportunidadVenta
                                Call m_oOportunidadVenta.ManejoEventosMenu(SBO_Application.Forms.ActiveForm, pVal, BubbleEvent)
                            Case mc_strContratoVenta
                                Call m_oCVenta.ManejadorEventoMenuNuevo(pVal, SBO_Application.Forms.ActiveForm)
                        End Select

                    Case IDMenus.IDMenus.m_oCVentaCrear 'Crear Contrato venta

                        If Not ValidarSiFormularioAbierto("SCGD_frmContVent", False) Then

                            Call m_oCVenta.ValidaTipoCambio(BubbleEvent)

                        End If

                    Case IDMenus.IDMenus.strMenuDuplicar
                        Select Case SBO_Application.Forms.ActiveForm.TypeEx
                            Case mc_strIdFormaCotizacion.ToString()
                                Call m_oCotizacion.ManejadorEventoMenu(SBO_Application.Forms.ActiveForm, pVal, BubbleEvent)
                                Exit Sub
                        End Select

                    Case IDMenus.IDMenus.m_oCVenta 'Contratos de Venta

                        If Not ValidarSiFormularioAbierto("SCGD_frmContVent", False) Then

                            Call m_oCVenta.ValidaTipoCambio(BubbleEvent)

                        End If

                    Case IDMenus.IDMenus.m_oFormularioPedidoVehiculos
                        If Not ValidarSiFormularioAbierto(mc_StrPedidoVehiculos, False) Then
                            Call m_oFormularioPedidoVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                        End If

                    Case IDMenus.IDMenus.m_oFormularioEntradaDeVehiculos
                        If Not ValidarSiFormularioAbierto(mc_strEntradaDeInventario, False) Then
                            Call m_oFormularioEntradaDeVehiculos.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                        End If

                    Case IDMenus.IDMenus.m_oFormularioCosteoDeEntradas
                        If Not ValidarSiFormularioAbierto(mc_strCosteoDeEntradas, False) Then
                            Call m_oFormularioCosteoDeEntradas.ManejadorEventosMenus(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)
                        End If

                    Case IDMenus.IDMenus.strMenuEliminar
                        Select Case SBO_Application.Forms.ActiveForm.TypeEx
                            Case mc_strControlVehiculo
                                Call m_oVehiculos.EliminarVehiculo(pVal, BubbleEvent)
                        End Select

                    Case IDMenus.IDMenus.strMenuRegistroDatosSiguiente

                        Select Case SBO_Application.Forms.ActiveForm.TypeEx

                            Case mc_strContratoVenta
                                Call m_oCVenta.ManejadorEventosMenusBefore(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)

                            Case mc_strUIGOODISSUE
                                m_oGoodIssue.DesHabilitarBoton1(SBO_Application.Forms.ActiveForm)
                        End Select
                    Case IDMenus.IDMenus.strMenuRegistroDatosAnterior

                        Select Case SBO_Application.Forms.ActiveForm.TypeEx

                            Case mc_strContratoVenta
                                Call m_oCVenta.ManejadorEventosMenusBefore(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)

                            Case mc_strUIGOODISSUE
                                m_oGoodIssue.DesHabilitarBoton1(SBO_Application.Forms.ActiveForm)
                        End Select

                    Case IDMenus.IDMenus.strMenuPrimerRegistroDatos

                        Select Case SBO_Application.Forms.ActiveForm.TypeEx

                            Case mc_strContratoVenta
                                Call m_oCVenta.ManejadorEventosMenusBefore(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)

                            Case mc_strUIGOODISSUE
                                m_oGoodIssue.DesHabilitarBoton1(SBO_Application.Forms.ActiveForm)
                        End Select

                    Case IDMenus.IDMenus.strMenuUltimoRegistroDatos

                        Select Case SBO_Application.Forms.ActiveForm.TypeEx

                            Case mc_strContratoVenta
                                Call m_oCVenta.ManejadorEventosMenusBefore(pVal, SBO_Application.Forms.ActiveForm, BubbleEvent)

                            Case mc_strUIGOODISSUE
                                m_oGoodIssue.DesHabilitarBoton1(SBO_Application.Forms.ActiveForm)
                        End Select

                End Select

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try

    End Sub

    Private Sub ValidaEliminarLineas(ByRef oFormulario As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Dim SerieCita As String = String.Empty
        Dim NumeroOT As String = String.Empty
        Dim UsaRequisicionReserva As String = String.Empty
        Dim Sucursal As String = String.Empty
        Try
            SerieCita = oFormulario.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_NoCita", 0).Trim()
            Sucursal = oFormulario.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).Trim()
            If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)) IsNot Nothing Then
                UsaRequisicionReserva = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_UsePrepicking.Trim
            End If

            If UsaRequisicionReserva = "Y" AndAlso Not String.IsNullOrEmpty(SerieCita) Then
                BubbleEvent = False
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.EliminarLineasCotizacion, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub SBO_Application_FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean) Handles SBO_Application.FormDataEvent

        Try

            Dim otmpForm As SAPbouiCOM.Form
            Dim strKey As String = ""
            Dim xmlDocKey As New Xml.XmlDocument
            m_blnOrdenCompraActualizada = False

            If Not m_oFormularioRequisiciones Is Nothing AndAlso m_blnUsaOrdenesDeTrabajo Then
                m_oFormularioRequisiciones.ApplicationSBOOnFormDataEvent(BusinessObjectInfo, BubbleEvent)
            End If

            If Not m_oFormularioPrestamo Is Nothing AndAlso m_blnFinanciamiento Then
                m_oFormularioPrestamo.ApplicationSBOOnDataEvent(BusinessObjectInfo, BubbleEvent)
            End If

            If Not m_oFormularioPlacas Is Nothing AndAlso m_blnUsaPlacas Then
                m_oFormularioPlacas.ApplicationSBOOnDataEvent(BusinessObjectInfo)
            End If

            Select Case BusinessObjectInfo.EventType
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE
                    If Not FormularioLLamadaServicioSBO.HuboError AndAlso FormularioLLamadaServicioSBO.FormType = BusinessObjectInfo.FormTypeEx AndAlso BusinessObjectInfo.ActionSuccess = True AndAlso m_blnUsaOrdenesDeTrabajo Then
                        m_oLlamadaServicio.CreaCotizacion(BusinessObjectInfo)
                    End If
                    If BusinessObjectInfo.ActionSuccess Then
                        Select Case BusinessObjectInfo.FormTypeEx
                            Case "SCGD_DET_1"
                                Dim ID As String = ObtieneValorEditText("txtVIN", SBO_Application.Forms.ActiveForm)
                                UpdateFechaSync(BusinessObjectInfo, "SCGD_DET_1", ID)
                            Case mc_strUniqueIDPropiedades
                                m_oPropiedades.EliminarUsuariosBD()
                            Case mc_strControlCVenta
                                'If BusinessObjectInfo.ActionSuccess Then
                                '    otmpForm = SBO_Application.Forms.Item(BusinessObjectInfo.FormUID)
                                '    m_oCVenta.ManejoEventoData(otmpForm)
                                'End If
                                'm_oCVenta.EliminarAccesoriosBD()
                                m_oListadoCV.ActulizarLista()
                            Case mc_strMaestroArticulos.ToString()
                                CalculaTiempoUnidades(BusinessObjectInfo, blnUsaConfiguracionInternaTaller)
                                UpdateFechaSync(BusinessObjectInfo, mc_strMaestroArticulos.ToString)
                            Case mc_strOrdenDeCompra.ToString()

                                m_blnOrdenCompraActualizada = True

                            Case mc_strUsuarios.ToString()
                                UpdateFechaSync(BusinessObjectInfo, mc_strUsuarios.ToString)
                            Case mc_strMaestroEmpleados.ToString()
                                UpdateFechaSync(BusinessObjectInfo, mc_strMaestroEmpleados.ToString)
                            Case mc_strSociosNegocios.ToString()
                                UpdateFechaSync(BusinessObjectInfo, mc_strSociosNegocios.ToString)
                            Case "11066"
                                UpdateFechaSync(BusinessObjectInfo, "11066")
                        End Select

                    End If

                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If BusinessObjectInfo.ActionSuccess Then
                        Select Case BusinessObjectInfo.FormTypeEx
                            'C
                            Case "SCGD_DET_1"
                                m_oVehiculos.AgregarComponentesPorDefecto = False
                                m_oVehiculos.ManejadorEventoFormDataLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID), BubbleEvent)
                                m_oVehiculos.AgregarComponentesPorDefecto = True
                                m_oVehiculos.ManejarModoFormulario(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID))
                            Case mc_strUniqueIDPropiedades
                                m_oPropiedades.HabilitarCampos(mc_strUniqueIDPropiedades, False)
                                m_oPropiedades.LimpiarLineasAEliminar()
                            Case mc_strUIGOODENT
                                Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID), False)
                                Dim obItem As Item = DirectCast(SBO_Application.Forms.Item(mc_strUIGOODENT).Items.Item("1"), Item)
                                obItem.Enabled = True
                                Dim obItemGenerar As Item = DirectCast(SBO_Application.Forms.Item(mc_strUIGOODENT).Items.Item("btn_Genera"), Item)
                                obItemGenerar.Enabled = True
                            Case mc_strUIGOODISSUE
                                Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID), False)

                                Call m_oGoodIssue.AgregarValoresCuenta(SBO_Application.Forms.GetForm(mc_strUIGOODISSUE, 0))

                                'Call m_oGoodIssue.AgregarValoresCuenta(SBO_Application.Forms.ActiveForm)
                                Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID), False)

                            Case mc_strSolicitudOTEspecial

                                m_oSolicitudOTEspecial.ManejadorEventoLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID), BubbleEvent)

                                'Utilitarios.FormularioDeshabilitado(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID), False)

                            Case mc_strUIFacturasInt
                                Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID), False)
                                Utilitarios.FormularioDeshabilitado(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID), False)
                                m_oFacturaInterna.CargarCombosEstiloyModelo(BusinessObjectInfo.FormUID, "")
                                m_oFacturaInterna.ManejadorEventoFormDataLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID), BubbleEvent)
                            Case mc_strControlCVenta

                                If Not m_oCVenta.m_blnCargoManejarEstados = True Then

                                    m_oCVenta.ManejarEstados(SBO_Application.Forms.Item("SCGD_frmContVent"))
                                    m_oCVenta.ManejoEventosCombo(SBO_Application.Forms.Item("SCGD_frmContVent"), ContratoVentasCls.mc_strMarcaUS, BusinessObjectInfo.FormUID)

                                End If
                                m_oCVenta.ManejadorEventoFormDataLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID), BubbleEvent)

                            Case mc_strUniqueIDLineasFactura
                                m_oLineasFactura.HabilitarCampos(mc_strUniqueIDLineasFactura)

                            Case mc_strUniqueIDLineasDesgloce
                                m_oLineasDesgloce.HabilitarCampos(mc_strUniqueIDLineasDesgloce)

                            Case mc_strUniqueIDTransaccionesCompras
                                m_oTransaccionesCompras.HabilitarCampos(mc_strUniqueIDTransaccionesCompras, False)

                            Case FormularioLLamadaServicioSBO.FormType
                                If m_blnUsaOrdenesDeTrabajo Then m_oLlamadaServicio.DeshabilitaFechaCita()
                                'Agregado 12/10/2010: Maneja estado de boton de Genera CV
                            Case mc_strOportunidadVenta
                                m_oOportunidadVenta.ManejarEstados(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID))
                                'Agregado 05/11/2010: Maneja estado de edit de contrato de venta en salida de mercancia 
                            Case CStr(mc_strSalidaMercancia)
                                m_oSalidaMercancia.ManejarEstado(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID))
                                'Agregado 13/12/2010: Maneja estado de edit de contrato de venta en entrada de mercancia
                            Case CStr(mc_strEntradaMercancia)
                                m_oEntradaMercancia.ManejarEstado(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID))
                            Case CStr(mc_strUISCGD_Citas)
                                m_oFormularioCitas.ManejadorEventoFormDataLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID))
                            Case "SCGD_CCIT"
                                ControladorCitas.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                            Case mc_strIdFormaCotizacion
                                m_oCotizacion.ManejadorEventoFormData(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID))
                                'Agregado 11/06/2013: Manejo del evento Load del formulario Cotizacion
                            Case mc_strUISCGD_SuspenderAgenda
                                m_oFormularioSuspensionAgenda.CargarCombosLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID))
                            Case CStr(mc_strCampana)
                                m_oCampana.ManejoFormDataLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID), BubbleEvent)
                            Case CStr("SCGD_AGD")
                                m_oFormularioAgendasConfiguracion.ManejadorEventoFormDataLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID), BubbleEvent)
                            Case mc_strUIDFormConfiguracionMSJ
                                m_oFormularioConfigNivelesAprob.ManejadorEventoFormDataLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID), BubbleEvent)
                            Case mc_strOrdenDeVenta
                                m_oOrdenVenta.ManejadorEventoFormDataLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID), BubbleEvent)
                            Case mc_StrPedidoVehiculos
                                m_oFormularioPedidoVehiculos.ManejadorEventoFormDataLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID))

                            Case mc_strIdFormaCotizacion
                                m_oCotizacion.blnValidarCamposHS_KM = False
                            Case mc_strCosteoDeEntradas
                                m_oFormularioCosteoDeEntradas.ManejadorEventoFormDataLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID))
                            Case mc_strEntradaDeVehiculos
                                m_oFormularioEntradaDeVehiculos.ManejadorEventoFormDataLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID))
                            Case g_strFormOT
                                m_oFormularioOrdenTrabajo.ManejadorEventoFormDataLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID))
                            Case mc_strUISCGD_FormParamAplicacion
                                m_oFormularioParametrosAplicacion.ManejadorEventoFormDataLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID))
                            Case mc_strDevolucionDeVehiculos
                                m_oFormularioDevolucionDeVehiculos.ManejadorEventoFormDataLoad(SBO_Application.Forms.Item(BusinessObjectInfo.FormUID))
                            Case mc_strFormSolEsp
                                m_oFormularioSolicitudEspecificos.ManejadorEventoFormDataLoad(BubbleEvent)
                            Case mc_strUISCGD_FormAVA
                                m_oFormularioAvaUs.ManejadorEventoFormDataLoad(BubbleEvent)
                            Case mc_strDimensionesContables
                                m_oDimensionesContables.ManejadorEventoFormDataLoad(BusinessObjectInfo.EventType)
                            Case mc_strDimensionesContablesOTs
                                m_oDimensionesContablesOTs.ManejadorEventoFormDataLoad(BusinessObjectInfo.EventType)
                        End Select

                    End If

                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD

                    If Not FormularioLLamadaServicioSBO.HuboError AndAlso FormularioLLamadaServicioSBO.FormType = BusinessObjectInfo.FormTypeEx AndAlso BusinessObjectInfo.ActionSuccess = True AndAlso m_blnUsaOrdenesDeTrabajo Then
                        m_oLlamadaServicio.CreaCotizacion(BusinessObjectInfo)
                    End If

                    Select Case BusinessObjectInfo.FormTypeEx
                        Case mc_strFacturadeCompra
                            'factura de proveedores compra
                            If BusinessObjectInfo.ActionSuccess Then
                                If BusinessObjectInfo.Type <> mc_strDocDraft Then
                                    m_oFacturaProveedores.ManejaFacturaProveedor()
                                End If
                                DocAprobacionHabilitado = False

                                'JVR
                                xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
                                Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
                                m_oFormularioCosteoDeEntradas.m_strFacturaProv = strKey
                            End If
                        Case mc_strEntradadeMercancia
                            'entrada de mercancia de Compras 
                            If BusinessObjectInfo.ActionSuccess Then
                                'se valida si se presenta la ventana de Aprobaciones
                                'para no crear el asiento por Servicios Externos
                                If BusinessObjectInfo.Type <> mc_strDocDraft Then
                                    m_oEntradaMercanciasEnCompras.blnDocCerrar = False
                                    m_oEntradaMercanciasEnCompras.ReprocesoEntradaMercancia()
                                End If
                                DocAprobacionHabilitado = False
                            End If
                        Case mc_strNotadeCredito
                            'Nota de credito proovedores
                            If BusinessObjectInfo.ActionSuccess Then
                                xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
                                Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
                                m_oNotaCreditoProveedor.ManejaNotaCredito(strKey)
                            End If
                        Case mc_strOrdenDeCompra
                            'Orden de compra
                            If BusinessObjectInfo.ActionSuccess Then
                                xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
                                Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
                                m_oDocumentoProcesoCompra.ManejaOrdenCompra(strKey)
                            End If
                        Case "11066"
                            If BusinessObjectInfo.ActionSuccess Then
                                UpdateFechaSync(BusinessObjectInfo, "11066")
                            End If

                            'Agregado 27/10/2010: Manejar los precios individuales de accesorios
                            'Case mc_strControlCVenta
                            'If BusinessObjectInfo.ActionSuccess Then
                            '    otmpForm = SBO_Application.Forms.Item(BusinessObjectInfo.FormUID)
                            '    m_oCVenta.ManejoEventoData(otmpForm)
                            'End If
                            'Agregado 05/11/2010: Guarda numero de salida de mercancia generada
                        Case CStr(mc_strSalidaMercancia)
                            If BusinessObjectInfo.ActionSuccess Then
                                xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
                                Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
                                m_oSalidaMercancia.strNumeroSalida = strKey
                            End If

                            'Agregado 26/06/2012: Guarda numero de pago recibido generado
                        Case CStr(mc_strPagoRecibido)
                            If BusinessObjectInfo.ActionSuccess Then
                                xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
                                Utilitarios.SacarValorObjectKey("PaymentParams", "DocEntry", strKey, xmlDocKey)
                                m_oPagoRecibido.strPagoRecibido = strKey
                            End If

                            'Agregado 13/12/2010: Guarda numero de entrada de mercancia generada
                        Case CStr(mc_strEntradaMercancia)
                            If BusinessObjectInfo.ActionSuccess Then
                                xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
                                Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
                                m_oEntradaMercancia.strNumeroEntrada = strKey
                            End If

                        Case "SCGD_DET_1"
                            If BusinessObjectInfo.ActionSuccess Then
                                Dim ID As String = ObtieneValorEditText("txtVIN", SBO_Application.Forms.ActiveForm)
                                UpdateFechaSync(BusinessObjectInfo, "SCGD_DET_1", ID)
                            End If
                        Case mc_strUsuarios.ToString()
                            If BusinessObjectInfo.ActionSuccess Then
                                UpdateFechaSync(BusinessObjectInfo, mc_strUsuarios.ToString)
                            End If
                        Case mc_strMaestroEmpleados.ToString()
                            If BusinessObjectInfo.ActionSuccess Then
                                UpdateFechaSync(BusinessObjectInfo, mc_strMaestroEmpleados.ToString)
                            End If
                        Case mc_strSociosNegocios.ToString()
                            If BusinessObjectInfo.ActionSuccess Then
                                UpdateFechaSync(BusinessObjectInfo, mc_strSociosNegocios.ToString)
                            End If
                        Case mc_strMaestroArticulos.ToString()
                            CalculaTiempoUnidades(BusinessObjectInfo, blnUsaConfiguracionInternaTaller)
                            If BusinessObjectInfo.ActionSuccess Then
                                UpdateFechaSync(BusinessObjectInfo, mc_strMaestroArticulos.ToString)
                            End If
                        Case mc_strFacturaCliente.ToString(), mc_strBoleta, mc_strFacturaExentaDeudores
                            If BusinessObjectInfo.ActionSuccess Then

                                xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
                                Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
                                'se valida si se presenta la ventana de Aprobaciones
                                'para no crear el asiento por Servicios Externos
                                If BusinessObjectInfo.Type <> mc_strDocDraft Then

                                    Call CotizacionCLS.CambiaEstadoAFacturado(BusinessObjectInfo.FormUID, m_oCompany, SBO_Application, blnUsaConfiguracionInternaTaller, m_oFacturaClientes.ListaBaseEntry, m_oFacturaClientes.BooleanBaseEntry)

                                    'verifica si se puede crear el asiento 
                                    If m_oFacturaClientes.CreaAsiento Then
                                        m_oFacturaClientes.CreaAsiento = False
                                        'Finaliza la transaccion creando el asiento
                                        m_oFacturaClientes.ManejaAsientosFacturaCliente(strKey)
                                    End If
                                End If
                                DocAprobacionHabilitado = False

                            End If
                        Case mc_strEntregas.ToString()
                            If BusinessObjectInfo.ActionSuccess Then
                                xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
                                Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
                                'm_oCotizacion.RealizarCosteo(strKey, False)
                            End If
                            'Case "SCGD_PDV"
                            '    If BusinessObjectInfo.ActionSuccess Then
                            '        xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
                            '        Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
                            '        'm_oCotizacion.RealizarCosteo(strKey, False)
                            '    End If

                            ''codigo para cuando se crea la transferencia desde preliminar
                        Case mc_strTrasladoInventario.ToString()

                            blnManejarFormularioTransferencia = False

                            Dim ActionSuccess As Boolean = BusinessObjectInfo.ActionSuccess

                            'If blnTransferenciaDesdeDraft Or blnTrasferenciaDesdeMensajeria Then
                            If blnTransferenciaDesdeDraft Then

                                If Not BusinessObjectInfo.BeforeAction Then

                                    Dim strDocEntry As String = BusinessObjectInfo.ObjectKey
                                    Dim oDocuments As SAPbobsCOM.StockTransfer
                                    Dim oDocumentsLine As SAPbobsCOM.StockTransfer_Lines = Nothing
                                    Dim oCotizacion As New CotizacionCLS(SBO_Application, m_oCompany)
                                    Dim ListaLineNum As New Generic.List(Of Integer)
                                    Dim intCodigoCotizacion As Int16
                                    Dim strNumeroOT As String = String.Empty

                                    oDocuments = DirectCast(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer), SAPbobsCOM.StockTransfer)

                                    If oDocuments.Browser.GetByKeys(strDocEntry) Then

                                        oDocumentsLine = oDocuments.Lines

                                        strDocEntry = oDocuments.DocEntry.ToString()
                                        strNumeroOT = oDocuments.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString()
                                        intCodigoCotizacion = CShort(oDocuments.UserFields.Fields.Item("U_SCGD_CodCotizacion").Value)

                                        For i As Integer = 0 To oDocumentsLine.Count - 1

                                            oDocumentsLine.SetCurrentLine(i)
                                            ListaLineNum.Add(CInt(oDocumentsLine.UserFields.Fields.Item("U_SCGD_LinenumOrigen").Value))

                                        Next i

                                    Else
                                        strDocEntry = ""
                                    End If

                                    Call oCotizacion.ActualizarLineasDeLaCotizacion(intCodigoCotizacion, oDocumentsLine, ListaLineNum)

                                    Call ActualizarLineasOrdenTrabajo(strNumeroOT, ListaLineNum, oCotizacion.strIdSucursal)

                                    If Not oCotizacion Is Nothing Then
                                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
                                        oCotizacion = Nothing
                                    End If

                                    'elimina el documento draft
                                    If blnTransferenciaDesdeDraft Then


                                        otmpForm = SBO_Application.Forms.GetForm("3002", 0)
                                        otmpForm.Select()

                                        Call EliminarDocumentoDraft(otmpForm, oMatrixDraft, intValor, False)

                                        otmpForm = SBO_Application.Forms.GetForm("3002", 0)
                                        otmpForm.Select()
                                        otmpForm.Items.Item("1").Click()

                                        otmpForm = Nothing

                                        'se activa el formulario de Transferencia de Stock
                                        SBO_Application.Forms.GetForm("940", 0).Select()

                                        otmpForm = SBO_Application.Forms.ActiveForm

                                    End If
                                End If
                            End If

                        Case mc_strNotaCreditoCliente.ToString()
                            If BusinessObjectInfo.ActionSuccess Then

                                If BusinessObjectInfo.Type <> mc_strDocDraft Then
                                    xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
                                    Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
                                    'm_oCotizacion.RealizarCosteo(strKey, True)

                                    'verifica si se puede crear el asiento 
                                    If m_oNotaCreditoClientes.CreaAsiento Then
                                        m_oNotaCreditoClientes.CreaAsiento = False

                                        'Finaliza la transaccion creando el asiento
                                        'If Not DocAprobacionHabilitado Then
                                        m_oNotaCreditoClientes.FinalizaTransaccion(strKey)
                                    End If



                                End If
                                DocAprobacionHabilitado = False


                            End If

                    End Select
                    'Case SAPbouiCOM.BoEventTypes.et_FORM_LOAD

                    '    If BusinessObjectInfo.FormTypeEx = "133" Then


                    '    End If

            End Select

        Catch ex As Exception
            m_strMensajePreFormDataEvent = ex.Message
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try

    End Sub

    Sub CalculaTiempoUnidades(ByRef BusinessObjectInfo As BusinessObjectInfo, Optional ByVal p_blnUsaConfiguracionInternaTaller As Boolean = False)

        Dim oItemSAP As SAPbobsCOM.IItems

        Dim dblConversionTiempo As Double
        Dim dblTiempoEnSegundos As Double
        oItemSAP = DirectCast(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems), SAPbobsCOM.Items)


        If BusinessObjectInfo.BeforeAction = False Then

            If oItemSAP.Browser.GetByKeys(BusinessObjectInfo.ObjectKey) Then
                dblConversionTiempo = Utilitarios.DevuelveConversionUnidadesTiempo(SBO_Application, p_blnUsaConfiguracionInternaTaller, intSucursal)

                If dblConversionTiempo <> 0 Then
                    If dblConversionTiempo = 0 Then
                        dblTiempoEnSegundos = 0
                    Else
                        dblTiempoEnSegundos = CDbl(oItemSAP.UserFields.Fields.Item("U_SCGD_Duracion").Value) / dblConversionTiempo
                    End If

                    If dblTiempoEnSegundos <> 0 Then
                        oItemSAP.UserFields.Fields.Item("U_SCGD_DrcionUndTmpo").Value = CStr(dblTiempoEnSegundos)
                    Else
                        oItemSAP.UserFields.Fields.Item("U_SCGD_DrcionUndTmpo").Value = 0
                    End If

                    oItemSAP.Update()
                End If
            End If
        End If
    End Sub

    Private Sub EliminarDocumentoDraft(ByRef p_formDraf As SAPbouiCOM.Form, ByRef p_Matrix As SAPbouiCOM.Matrix, ByVal p_DocEntryDraft As Integer, Optional ByVal blnEliminarDesdeMensajeria As Boolean = False)

        Dim intSelrow As Integer = p_Matrix.GetNextSelectedRow(0, BoOrderType.ot_SelectionOrder)
        If intSelrow <> -1 Then

            SBO_Application.Menus.Item("1283").Enabled = True
            SBO_Application.Menus.Item("1283").Activate()
            p_formDraf.Items.Item("1").Click()

            blnTransferenciaDesdeDraft = False

        End If

    End Sub

    Private Sub ActualizarLineasOrdenTrabajo(ByVal p_strNumeroOT As String, ByVal p_listLineNum As Generic.List(Of Integer), ByVal p_strIdSucursal As String)

        Dim strCadenaConexionBDTaller As String = ""

        ' Private m_dstOrdenTrabajoAnterior As OrdenTrabajoDataset
        Dim m_adpOrdenTrabajo As New DMSOneFramework.SCGDataAccess.OrdenTrabajoDataAdapter
        Dim m_dstOrdenTrabajo As New DMSOneFramework.OrdenTrabajoDataset

        Dim m_dstRepuestosxOrden As DMSOneFramework.RepuestosxOrdenDataset
        Dim m_adpRepuestosxOrden As DMSOneFramework.SCGDataAccess.RepuestosxOrdenDataAdapter

        Dim m_dstSuministrosxOrden As DMSOneFramework.SuministrosDataset
        Dim m_adpSuministrosxOrden As DMSOneFramework.SCGDataAccess.SuministrosDataAdapter

        Dim m_dstActividadesxOrden As DMSOneFramework.ActividadesXFaseDataset
        Dim m_adpActividadesxOrden As ActividadesXFaseDataAdapter

        'Dim m_dstAsignacionesColaboradores As New DMSOneFramework.ColaboradorDataset

        Dim objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strCadenaConexionBDTaller)

        'datarows

        '        Dim m_drwOrdenTrabajo As DMSOneFramework.OrdenTrabajoDataset.SCGTA_TB_OrdenRow
        '        Dim m_drwRepuestos As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
        '        Dim m_drwSuministros As DMSOneFramework.SuministrosDataset.SCGTA_VW_SuministrosRowChangeEvent
        '        Dim m_drwActividades As DMSOneFramework.ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow

        'Actualización de la cotización

        m_dstActividadesxOrden = Nothing
        m_dstRepuestosxOrden = Nothing
        m_dstSuministrosxOrden = Nothing


        Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, p_strIdSucursal, strCadenaConexionBDTaller)

        m_dstRepuestosxOrden = New DMSOneFramework.RepuestosxOrdenDataset
        m_adpRepuestosxOrden = New RepuestosxOrdenDataAdapter(strCadenaConexionBDTaller)

        m_dstSuministrosxOrden = New DMSOneFramework.SuministrosDataset
        m_adpSuministrosxOrden = New SuministrosDataAdapter(strCadenaConexionBDTaller)

        m_dstActividadesxOrden = New DMSOneFramework.ActividadesXFaseDataset
        m_adpActividadesxOrden = New ActividadesXFaseDataAdapter(strCadenaConexionBDTaller)


        m_dstOrdenTrabajo.EnforceConstraints = False
        m_adpOrdenTrabajo.Fill_x_OrdenTrabajo(m_dstRepuestosxOrden, p_strNumeroOT)


        For m As Integer = 0 To p_listLineNum.Count - 1

            For Each drwRep As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow In m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows

                If p_listLineNum(m) = drwRep.LineNum Then

                    With drwRep
                        .CodEstadoRep = m_CantidadRecibida
                    End With
                    Exit For
                End If
            Next

        Next

        m_adpRepuestosxOrden.UpdateCodigoRepuesto(m_dstRepuestosxOrden)

    End Sub

    'Private Function VerificarTipoDocumentoDraft(ByVal p_docentry As Integer) As Boolean

    '    Dim oRecordset As SAPbobsCOM.Recordset
    '    Dim strQuery As String

    '    Try
    '        oRecordset = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
    '        strQuery = "select docentry, jrnlmemo from ODRF where DocEntry = " & p_docentry & " and ObjType = 67 "

    '        oRecordset.DoQuery(strQuery)
    '        Dim strDescripcion As String = oRecordset.Fields.Item(0).Value
    '        If strDescripcion <> 0 Then
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    Catch ex As Exception

    '    End Try

    'End Function

    Private Sub UpdateFechaSync(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByVal numeroFormSap As String, Optional ByVal ID As String = "0")

        If BusinessObjectInfo.BeforeAction Then Return

        'Dim fechaAnsi As String
        'fechaAnsi = Date.Now.Year.ToString

        'If Date.Now.Month.ToString.Length = 2 Then
        '    fechaAnsi &= Date.Now.Month.ToString
        'Else
        '    fechaAnsi &= "0" & Date.Now.Month.ToString
        'End If

        'If Date.Now.Day.ToString.Length = 2 Then
        '    fechaAnsi &= Date.Now.Day.ToString & " "
        'Else
        '    fechaAnsi &= "0" & Date.Now.Day.ToString & " "
        'End If

        'If Date.Now.Hour.ToString.Length = 2 Then
        '    fechaAnsi &= Date.Now.Hour.ToString & ":"
        'Else
        '    fechaAnsi &= "0" & Date.Now.Hour.ToString & ":"
        'End If

        'If Date.Now.Minute.ToString.Length = 2 Then
        '    fechaAnsi &= Date.Now.Minute.ToString & ":"
        'Else
        '    fechaAnsi &= "0" & Date.Now.Minute.ToString & ":"
        'End If

        'If Date.Now.Second.ToString.Length = 2 Then
        '    fechaAnsi &= Date.Now.Second.ToString
        'Else
        '    fechaAnsi &= "0" & Date.Now.Second.ToString
        'End If

        'Select Case numeroFormSap

        '    Case mc_strUsuarios.ToString()
        '        ''Comentado por error al crear un usuario después de modificar uno inmediatamente anterior
        '        'Dim oItemSAP As SAPbobsCOM.Users
        '        'Dim key As Integer = CInt(Utilitarios.ParseKey(BusinessObjectInfo.ObjectKey, "/UserParams/USERID"))
        '        'oItemSAP = DirectCast(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUsers), SAPbobsCOM.Users)
        '        'oItemSAP.GetByKey(key)
        '        ''                oItemSAP.Browser.GetByKeys(BusinessObjectInfo.ObjectKey)
        '        'oItemSAP.UserFields.Fields.Item("U_SCGD_fechaSync").Value = fechaAnsi
        '        'oItemSAP.Update()

        '    Case mc_strMaestroEmpleados.ToString()
        '        Dim oItemSAP As SAPbobsCOM.EmployeesInfo
        '        oItemSAP = DirectCast(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oEmployeesInfo), SAPbobsCOM.EmployeesInfo)
        '        oItemSAP.Browser.GetByKeys(BusinessObjectInfo.ObjectKey)
        '        oItemSAP.UserFields.Fields.Item("U_SCGD_fechaSync").Value = fechaAnsi
        '        oItemSAP.Update()

        '    Case mc_strMaestroArticulos.ToString()
        '        Dim oItemSAP As SAPbobsCOM.Items
        '        oItemSAP = DirectCast(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems), SAPbobsCOM.Items)
        '        oItemSAP.Browser.GetByKeys(BusinessObjectInfo.ObjectKey)
        '        oItemSAP.UserFields.Fields.Item("U_SCGD_fechaSync").Value = fechaAnsi
        '        oItemSAP.Update()

        '    Case mc_strSociosNegocios.ToString()
        '        Dim oItemSAP As SAPbobsCOM.BusinessPartners
        '        oItemSAP = DirectCast(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners), SAPbobsCOM.BusinessPartners)
        '        oItemSAP.Browser.GetByKeys(BusinessObjectInfo.ObjectKey)
        '        oItemSAP.UserFields.Fields.Item("U_SCGD_fechaSync").Value = fechaAnsi
        '        oItemSAP.Update()

        '    Case "SCGD_DET_1"
        '        Dim baseDatos As String
        '        baseDatos = SBO_Application.Company.DatabaseName
        '        Dim Server As String
        '        Server = SBO_Application.Company.ServerName
        '        Dim strConsulta As String
        '        strConsulta = "UPDATE [@SCGD_VEHICULO] set U_fechaSync = '" & fechaAnsi & "' Where U_Num_VIN = '" & ID & "'"
        '        Utilitarios.EjecutarConsulta(strConsulta, baseDatos, Server)

        '    Case "11066"
        '        Dim baseDatos As String
        '        baseDatos = SBO_Application.Company.DatabaseName
        '        Dim Server As String
        '        Server = SBO_Application.Company.ServerName
        '        Dim strConsulta As String
        '        strConsulta = "UPDATE [@SCGD_CATEGORIA_SERV] set U_fechaSync = '" & fechaAnsi & "'"
        '        Utilitarios.EjecutarConsulta(strConsulta, baseDatos, Server)

        'End Select

    End Sub

    Private Function ObtieneValorEditText(ByVal itemUID As String, ByVal sboForm As Form) As String
        Dim sboItem As Item
        Dim sboEditText As EditText

        sboItem = sboForm.Items.Item(itemUID)
        sboEditText = DirectCast(sboItem.Specific, EditText)
        Return sboEditText.Value
    End Function


    Private Function ValidarCuentaTransito(ByVal p_form As SAPbouiCOM.Form, ByVal p_formatcode As String) As Boolean

        Dim objUtilitarios As New Utilitarios
        Dim baseDatos As String
        baseDatos = SBO_Application.Company.DatabaseName
        Dim Server As String
        Server = SBO_Application.Company.ServerName

        Dim valorcampo As String = p_formatcode
        If Not valorcampo = String.Empty Then

            Dim strConsultaSCGAdmin4 As String

            strConsultaSCGAdmin4 = "Select LineId FROM [@SCGD_ADMIN4] WHERE U_Transito = '" & p_formatcode & "' OR U_Stock = '" & p_formatcode & "'"
            Dim strExisteCuenta As String = Utilitarios.EjecutarConsulta(strConsultaSCGAdmin4, baseDatos, Server)

            If Not strExisteCuenta = String.Empty Then
                Return True
            Else
                Return False
            End If
        End If

    End Function


    ''' <summary>
    ''' Valida que el UDF Codigo de Unidad exista
    ''' </summary>
    ''' <param name="p_form"> formulario</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidarCampoUnidad(ByVal p_form As SAPbouiCOM.Form, ByVal p_strMatrz As String) As Boolean

        Dim oRefItem As SAPbouiCOM.Item
        Dim strCampoUnidad As String

        Dim objUtilitarios As New Utilitarios
        Dim baseDatos As String
        baseDatos = SBO_Application.Company.DatabaseName
        Dim Server As String
        Server = SBO_Application.Company.ServerName

        Dim xmlDocMatrix As Xml.XmlDocument
        Dim XmlNode As Xml.XmlNode
        Dim matrixXml As String
        Dim blnMatrizServicio As Boolean = False
        Dim contador As Integer = 0

        If p_form.Type = 392 Then
            strCampoUnidad = "U_SCGD_Cod_Unidad"
        Else
            strCampoUnidad = "U_SCGD_Cod_Unid"
        End If
        oRefItem = p_form.Items.Item(p_strMatrz)

        oMatrix = DirectCast(oRefItem.Specific, Matrix)

        If p_form.Mode = BoFormMode.fm_ADD_MODE Or p_form.Mode = BoFormMode.fm_UPDATE_MODE Then

            '******************************
            matrixXml = oMatrix.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

            xmlDocMatrix = New Xml.XmlDocument
            xmlDocMatrix.LoadXml(matrixXml)

            contador = 1

            For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")

                Dim elementoCodigoUnidad As Xml.XmlNode
                Dim elementoCodigoTransaccion As Xml.XmlNode

                If p_form.Type = 392 Then
                    elementoCodigoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'U_SCGD_Cod_Unidad']")
                    'strCampoUnidad = "U_SCGD_Cod_Unidad"
                Else
                    elementoCodigoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'U_SCGD_Cod_Unid']")
                    'strCampoUnidad = "U_SCGD_Cod_Unid"
                End If

                elementoCodigoTransaccion = node.SelectSingleNode("Columns/Column/Value[../ID = 'U_SCGD_Cod_Tran']")

                Dim Unidad As String = elementoCodigoUnidad.InnerText.Trim
                Dim Transaccion As String = elementoCodigoTransaccion.InnerText.Trim

                If Not Unidad = String.Empty Then

                    Dim strConsulta As String
                    strConsulta = "select code  FROM [@SCGD_VEHICULO] where U_Cod_Unid ='" & Unidad & "'"
                    Dim ver As String = Utilitarios.EjecutarConsulta(strConsulta, baseDatos, Server)
                    If ver = String.Empty Then
                        'If SBO_Application.MessageBox(My.Resources.Resource.NoExsiteCodigoUnidad & valorcampo & My.Resources.Resource.NoExisteCU, 1, "ok") = 1 Then
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.NoExsiteCodigoUnidad & " " & Unidad & My.Resources.Resource.NoExisteCU, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return True
                        'End If
                    End If

                End If
                contador = contador + 1
            Next
        End If


    End Function

    Private Function ValidarCodigoTransaccion(ByVal p_form As SAPbouiCOM.Form, ByVal p_valorMatriz As String, ByVal p_strCampo As String) As Boolean

        Dim oRefItem As SAPbouiCOM.Item
        Dim strCampoUnidad As String

        Dim cboCombo As SAPbouiCOM.ComboBox

        Dim xmlDocMatrix As Xml.XmlDocument
        Dim XmlNode As Xml.XmlNode
        Dim matrixXml As String
        Dim blnMatrizServicio As Boolean = False
        Dim contador As Integer = 0

        oRefItem = p_form.Items.Item(p_valorMatriz)

        If p_form.Type = 392 Then
            strCampoUnidad = "U_SCGD_Cod_Unidad"
        Else
            strCampoUnidad = "U_SCGD_Cod_Unid"
        End If

        oMatrix = DirectCast(oRefItem.Specific, Matrix)

        If p_form.Mode = BoFormMode.fm_ADD_MODE Or p_form.Mode = BoFormMode.fm_UPDATE_MODE Then


            '******************************
            matrixXml = oMatrix.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

            xmlDocMatrix = New Xml.XmlDocument
            xmlDocMatrix.LoadXml(matrixXml)

            contador = 1

            For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")
                Dim elementoCodigoUnidad As Xml.XmlNode
                Dim elementoCodigoTransaccion As Xml.XmlNode

                If p_form.Type = 392 Then
                    elementoCodigoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'U_SCGD_Cod_Unidad']")
                    'strCampoUnidad = "U_SCGD_Cod_Unidad"
                Else
                    elementoCodigoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'U_SCGD_Cod_Unid']")
                    'strCampoUnidad = "U_SCGD_Cod_Unid"
                End If

                elementoCodigoTransaccion = node.SelectSingleNode("Columns/Column/Value[../ID = 'U_SCGD_Cod_Tran']")

                Dim Unidad As String = elementoCodigoUnidad.InnerText.Trim
                Dim Transaccion As String = elementoCodigoTransaccion.InnerText.Trim


                If Not Unidad = String.Empty Then

                    If Transaccion = String.Empty Then

                        SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeDefinaCodigoTransaccion & contador, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return True
                    End If
                ElseIf Unidad = String.Empty Then
                    If Not Transaccion = String.Empty Then
                        SBO_Application.StatusBar.SetText("Defina Codigo de Unidad en la linea:  " & contador, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return True
                    End If

                End If
                contador = contador + 1
            Next

        End If
    End Function


    Private Function ValidarCamposParaCuentaTransito(ByVal p_fila As Integer, ByVal p_strMatriz As String, ByVal p_form As SAPbouiCOM.Form, ByVal p_CuentaMayor As String, ByVal p_CodigoUnidad As String) As Boolean

        If p_CuentaMayor = String.Empty And p_CodigoUnidad <> String.Empty Then

            If ValidarCampoUnidad(p_form, p_strMatriz) = True Then
                Return False
            End If

            If ValidarCodigoTransaccion(p_form, p_strMatriz, m_striUDF_Cod_Unid) Then
                Return False
            End If

            SBO_Application.StatusBar.SetText(My.Resources.Resource.DigiteCuentaTransito & " " & p_fila, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False

        ElseIf p_CuentaMayor <> String.Empty And p_CodigoUnidad = String.Empty Then

            If ValidarCuentaTransito(p_form, p_CuentaMayor) = True Then

                If p_CodigoUnidad = String.Empty Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.DigiteCodigoUnidadCuentaTransito & " " & p_fila, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    Return False
                End If

            Else

                If ValidarCampoUnidad(p_form, p_strMatriz) = True Then
                    Return False
                End If

                If ValidarCodigoTransaccion(p_form, p_strMatriz, m_striUDF_Cod_Unid) Then
                    Return False
                End If

                Return True

            End If

        ElseIf p_CuentaMayor <> String.Empty And p_CodigoUnidad <> String.Empty Then

            If ValidarCuentaTransito(p_form, p_CuentaMayor) = True Then

                If ValidarCampoUnidad(p_form, p_strMatriz) = True Then
                    Return False
                End If

                If ValidarCodigoTransaccion(p_form, p_strMatriz, m_striUDF_Cod_Unid) Then
                    Return False
                End If
                Return True

            Else

                If ValidarCampoUnidad(p_form, p_strMatriz) = True Then
                    Return False
                End If

                If ValidarCodigoTransaccion(p_form, p_strMatriz, m_striUDF_Cod_Unid) Then
                    Return False
                End If

                SBO_Application.StatusBar.SetText(My.Resources.Resource.DigiteCuentaTransito & " " & p_fila, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False

            End If
        Else
            Return True
        End If



    End Function

    Private Sub SBO_Application_RightClickEvent(ByRef eventInfo As SAPbouiCOM.ContextMenuInfo, ByRef BubbleEvent As Boolean) Handles SBO_Application.RightClickEvent

        If eventInfo.BeforeAction AndAlso m_blnUsaOrdenesDeTrabajo Then
            Select Case eventInfo.ItemUID
                Case mc_strIDMatriz
                    If eventInfo.Row > 0 Then
                        m_intRowOT = eventInfo.Row
                    End If
            End Select
        End If

    End Sub

#End Region

    Private Sub SBO_Application_LayoutKeyEvent(ByRef eventInfo As SAPbouiCOM.LayoutKeyInfo, ByRef BubbleEvent As Boolean) Handles SBO_Application.LayoutKeyEvent

    End Sub
End Class

