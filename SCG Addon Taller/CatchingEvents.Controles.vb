Imports System.Threading
Imports DMS_Addon.Agendas
Imports DMS_Addon.GastosContratoVentas
Imports DMS_Addon.LlamadaServicio
Imports DMS_Addon.Requisiciones
Imports DMS_Addon.Ventas
Imports SAPbouiCOM
Imports SCG.Cifrado
Imports SCG.Financiamiento
Imports SCG.Requisiciones
Imports SCG.ServicioPostVenta
Imports Company = SAPbobsCOM.Company
Imports System.IO

Partial Public Class CatchingEvents
    Private Const Llave As String = "28AF0447C8C24547892C3EA083098FF0"
    Private Const Vector As String = "C91F7F3B36564ac2BCC53B10"
#Region "Metodos Carga de Addon"

    Private Function SetApplication() As Boolean

        Dim SboGuiApi As SboGuiApi
        Dim sConnectionString As String
        Dim intConnect As Integer
        Dim strUserSAP, strPasswordSAP, strUserSQL, strPasswordSQL, strLicenseServer, strSingleSignOn As String
        Dim intTypeServer As Integer
        Try
            'ManejaProcesosAddon()
            SboGuiApi = New SboGuiApi
            If Environment.GetCommandLineArgs.Length < 2 Then
                sConnectionString = "0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056"
            Else
                sConnectionString = CStr(Environment.GetCommandLineArgs.GetValue(1))
            End If
            '0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056
            Call SboGuiApi.Connect(sConnectionString)
            If Not SboGuiApi Is Nothing Then
                SBO_Application = SboGuiApi.GetApplication
                If Not SBO_Application Is Nothing Then
                    DMS_Connector.Company.ApplicationSBO = SBO_Application
                    If DMS_Connector.Helpers.GetUserAndPassword(strUserSAP, strPasswordSAP, strUserSQL, strPasswordSQL, intTypeServer, strLicenseServer, strSingleSignOn) Then
                        strUserSAP = Cifra.DesEncripta(strUserSAP, Llave, Vector)
                        strPasswordSAP = Cifra.DesEncripta(strPasswordSAP, Llave, Vector)
                        strUserSQL = Cifra.DesEncripta(strUserSQL, Llave, Vector)
                        strPasswordSQL = Cifra.DesEncripta(strPasswordSQL, Llave, Vector)
                        DBUser = strUserSQL
                        DBPassword = strPasswordSQL
                        intConnect = DMS_Connector.Company.ConnectCompany(SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName, strLicenseServer,
                                                           strUserSAP, strPasswordSAP, strUserSQL, strPasswordSQL, intTypeServer, strSingleSignOn)
                        If intConnect <> 0 Then
                            SBO_Application.SetStatusBarMessage(String.Format(My.Resources.Resource.AddonNoInicializado, intConnect) + " " + DMS_Connector.Company.CompanySBO.GetLastErrorDescription, BoMessageTime.bmt_Long, True)
                            Call Windows.Forms.Application.Exit()
                            Environment.Exit(0)
                        Else
                            m_oCompany = DMS_Connector.Company.CompanySBO
                            DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.InicializandoAddon, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
                            Utilitarios.bLoadInputEvents = False
                            Return CargarConfiguracionInicial()
                        End If
                    Else
                        SBO_Application.SetStatusBarMessage(String.Format(My.Resources.Resource.AddonNoInicializadoParametros, intConnect), BoMessageTime.bmt_Long, True)
                        Call Windows.Forms.Application.Exit()
                        Environment.Exit(0)
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Environment.Exit(0)
        Finally
        End Try

    End Function

    Private Sub EscribirTXT(ByVal p_strValor As String)
        Dim strUbicacion As String = String.Empty
        Try
            Dim strPath As String = Path.GetTempPath()
            Dim file As System.IO.StreamWriter
            file = My.Computer.FileSystem.OpenTextFileWriter(strPath + "log64.txt", True)
            file.WriteLine(p_strValor)
            file.Close()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Function CargarConfiguracionInicial() As Boolean
        Try
            SBO_Application.StatusBar.SetText(My.Resources.Resource.CargandoParamGenAddon, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
            DMS_Connector.Configuracion.Carga_ParametrizacionesGenerales()
            If DMS_Connector.Configuracion.ParamGenAddon Is Nothing Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ProblemaCargandoParamGenAddon, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                Return False
            End If
            SBO_Application.StatusBar.SetText(My.Resources.Resource.CargandoConfSucursal, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
            DMS_Connector.Configuracion.Carga_Configuracion_Sucursal()
            If DMS_Connector.Configuracion.ConfiguracionSucursales Is Nothing Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ProblemaCargandoConfSucursal, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                Return False
            End If
            DMS_Connector.Configuracion.CargaTipoOT()
            DMS_Connector.Configuracion.CargaEstadosTrasladado()
            DMS_Connector.Configuracion.CargaEstadosAprobado()
            DMS_Connector.Configuracion.Carga_ConfiguracionMensajeria()
            DMS_Connector.Configuracion.Carga_ConfiguracionNumeraciones()
            DMS_Connector.Configuracion.Carga_Dimensiones()
            SBO_Application.StatusBar.SetText(My.Resources.Resource.FinCargaConfiguraciones, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
        Return True
    End Function

    Private Function CrearInstanciasClases(ByVal p_Application As Application, ByVal p_Company As Company) As String

        oGestorMenu = New GestorMenu(p_Application)
        oGestorFormularios = New GestorFormularios(p_Application)
        m_oRecepcionVHUI = New RecepcionVehiculo(p_Application, p_Company)
        m_oMediosPago = New MediosDePago(p_Application, p_Company)
        m_oCampana = New Campaña(p_Application, p_Company)
        m_oOportunidadVenta = New OportunidadVenta(p_Application, p_Company)
        m_oSalidaMercancia = New SalidasMercancia(p_Application, p_Company)
        m_oEntradaMercancia = New EntradasMercancia(p_Application, p_Company)
        m_oComprasEnVentas = New ComprasEnProcesoVentas(p_Company, p_Application)
        m_oCompras = New ComprasCls(p_Company, ComprasCls.TrabajaConSucursal.Si, p_Application)
        ''Agrega Menu Configuraciones
        m_oPermisos = New NivelesPlanVentasCls(p_Application, p_Company)
        '' Agrega Menu Niveles PV
        m_oNivelesPV = New NivelesPV(p_Application, p_Company, "SCGD_PRM")
        m_oLineasFactura = New ConfiguracionLineasAdicionalesFacturaCls(p_Application, p_Company)
        m_oLineasDesgloce = New ConfiguracionLineasDesgloceCobroCls(p_Application, p_Company)
        m_oTransaccionesCompras = New ConfiguracionTransaccionesCompraCls(p_Application, p_Company)
        m_oCotizacion = New CotizacionCLS(p_Application, p_Company)
        m_oBuscadorCV = New BuscadorContratoVentaCls(p_Application, p_Company)
        m_oListadoCV = New ListadoContratosCls(p_Application, p_Company)
        m_oListadoContratosReversados = New ListadoContratoReversadosCls(p_Application, p_Company)
        m_oListaContratos_a_Reversar = New ListadoContratosFacturadosReversadosCls(p_Application, p_Company)
        m_oListaContratosSegPV = New ListadoContratosSeguroPostVenta(p_Application, p_Company)
        m_oTrasladoCostos = New TrasladoCostosDeUnidadesCls(p_Application, p_Company)
        m_oGoodReceive = New GoodReceiveCls(p_Application, p_Company)
        m_oGoodIssue = New GoodIssueCls(p_Application, p_Company)
        m_oListadoGR = New Listado_GRCls(p_Application, p_Company)
        m_oRecosteos = New RecosteosCls(p_Application, p_Company)
        m_oVehiculosACostear = New VehiculosSinCostearCls(p_Application)
        m_oInventarioVehiculos = New ConsultaInventarioVehiculosCls(p_Application, p_Company)
        m_oCosteoMultiplesUnidades = New CosteoMultiple(p_Application, p_Company)
        m_oSalidasMultiplesUnidades = New SalidaMultiple(p_Application, p_Company)
        m_oConfiguracionGeneral = New ConfiguracionesGenerelesAddOn(p_Application)
        m_oReportesCosteo = New ReportesCosteoCls(p_Application, p_Company)
        m_oCFLbyFS = New ChooseFromListByFormattedSCls(p_Application, p_Company)
        m_oVehiculos = New VehiculosCls(p_Company, p_Application)
        m_oPropiedades = New ConfiguracionPropiedadesVehiculosCls(p_Application, p_Company)
        m_oCVenta = New ContratoVentasCls(p_Company, p_Application)
        m_oEntradaMercanciasEnCompras = New EntradasMercanciasEnCompras(p_Company, p_Application)
        m_oDocumentoProcesoCompra = New DocumentoProcesoCompra(p_Company, p_Application)
        m_oFacturaProveedores = New FacturaProveedores(p_Company, p_Application)
        m_oNotaCreditoProveedor = New NotaCreditoProveedor(p_Company, p_Application)
        m_oDevolucionMercancia = New DevolucionMercancia(p_Company, p_Application)
        m_oFacturaClientes = New FacturaClientes(p_Company, p_Application)
        m_oReporteCV = New ContratoVentasReportesCls(p_Company, p_Application)
        m_oFormularioPermisosVendedoresXTI = New VendedoresPorTipoInventario(p_Company, p_Application)
        m_FormularioBalance = New BalanceFormulario(p_Application, p_Company)
        m_oFormularioBusquedaOT = New BusquedaOrdenesTrabajo(p_Application, p_Company, strMenuBusqeudaOt)
        m_oEstadosOT = New EntregaVehiculosOT(p_Company, p_Application)
        m_oFacturaInterna = New FacturaInterna(p_Application, p_Company)
        m_oListaCVXUnidad = New ListaContXUnidad(p_Application, p_Company)
        m_oTransferenciaItems = New TransferenciaItems(p_Application, p_Company)
        m_oFormularioGastosCV = New GastosAdicionales(p_Application, p_Company)
        m_oPagoRecibido = New PagoRecibido(p_Application, p_Company)
        m_oFormularioSeleccionaGastosOT = New SeleccionarGastosCostosOT(p_Application, p_Company)
        m_oFormularioCrearDocumentosGastos = New CrearDocumentosGastosCostos(p_Application, p_Company)
        m_oFormularioSeriesNumeracion = New NumeracionSeries(p_Company, p_Application)
        m_oUsuariosxNivel = New UsuariosPorNAprob(p_Company, p_Application)
        m_oFormularioVehiculoArticuloVenta = New VehiculoArticuloVenta(p_Company, p_Application)
        m_oFormularioVehiculoColorSeleccion = New VehiculoColoresSeleccion(p_Company, p_Application)
        m_oFormularioListaPreciosSeleccion = New ListaPreciosSeleccion(p_Company, p_Application)
        m_oFormularioSeleccionEmpleados = New ListaEmpleadosSeleccion(p_Application, p_Company)
        m_oFormularioComentariosCV = New ComentariosHistorial(p_Company, p_Application)
        m_oFormularioComentariosIV = New ComentariosInventarioV(p_Company, p_Application)
        m_oFormularioSeleciconMarcaEstiloModelo = New VehiculoSeleccionMarcaEstilo(p_Company, p_Application)
        m_oTipoOtInterna = New TipoOtInterna(p_Company, p_Application)
        m_oOrdenVenta = New OrdenVenta(p_Application, p_Company)
        m_oSolicitaOtEsp = New SolicitaOTEspecial(p_Company, p_Application)
        m_oAsignacionMultiple = New AsignacionMultiple(p_Company, p_Application)
        g_oFormularioVisitas = New Visita(p_Application, p_Company)
        g_oFormularioBusquedaControlProceso = New BusquedaControlProceso(p_Application, p_Company)
        g_oFormularioControlCrearVisita = New ControlCrearVisita(p_Application, p_Company)
        g_oFormularioControlVisita = New ControlVisita(p_Application, p_Company)
        g_oFormularioOfertaVentas = New OfertaVentas(p_Application, p_Company)
        m_oNotaCreditoClientes = New NotaCreditoClientes(p_Company, p_Application)
        m_oFormularioSeleccionaUnidadDev = New SeleccionUnidadDevolucion(p_Application, p_Company)
        m_oFormularioSeleccionLineasPedidos = New SeleccionLineasPedidos(p_Application, p_Company)
        m_oFormularioSeleccionLineasRecepcion = New SeleccionLineasRecepcion(p_Application, p_Company)
        m_oSociosNegocio = New MaestroSociosNegocio(p_Company, p_Application)
        m_oMaestroEmpleados = New MaestroEmpleados(p_Company, p_Application)
        m_oFormSeleccionUbicaciones = New ListaUbicaciones(p_Application, p_Company)
        m_oComprasEnVentas = New ComprasEnProcesoVentas(p_Company, p_Application)
        m_oAgendas = New FormularioAgendaSBO(p_Application, p_Company)
        m_oLlamadaServicio = New FormularioLLamadaServicioSBO(p_Application, p_Company)
        If blnDraft Then
            Dim req As RequisicionTraslado = New RequisicionTraslado(p_Company)
            m_oFormularioListadoRequisiciones = New ListadoRequisicionesConPermisos(p_Application, p_Company)
            m_oFormularioRequisiciones = New FormularioRequisiconesConPermisos(p_Application, p_Company, req)
            m_oManejadorRequisicionesTraslados = New ManejadorRequisicionesTraslados(p_Company, p_Application, blnUsaConfiguracionInternaTaller)
            AddHandler m_oFormularioRequisiciones.TrasladoRealizado, AddressOf m_oManejadorRequisicionesTraslados.TrasladoRealizado
            AddHandler req.ActualizaEncabezado, AddressOf m_oManejadorRequisicionesTraslados.ActualizaTransferenciaStock
            AddHandler req.ActualizaLineaTraslado, AddressOf m_oManejadorRequisicionesTraslados.ActualizaLineaTransferenciaStock
            AddHandler m_oFormularioRequisiciones.LineasCanceladas, AddressOf m_oManejadorRequisicionesTraslados.LineasCanceladas
            AddHandler m_oFormularioRequisiciones.LocalizationNeeded, AddressOf m_oManejadorRequisicionesTraslados.LocalizationNeeded
            AddHandler m_oFormularioRequisiciones.AjusteCantidadRealizado, AddressOf m_oManejadorRequisicionesTraslados.AjusteCantidadRealizado
        End If
        If m_blnUsaPlanDeVentas Then
            m_oFormularioPresupuestos = New FormularioPresupuestos(p_Application, p_Company)
            m_oRefacturacion = New Refacturacion(p_Application, p_Company)
        End If
        If m_blnUsaCosteoVehículo Then
            m_oCosteoMultiplesUnidades = New CosteoMultiple(p_Application, p_Company)
            m_oSalidasMultiplesUnidades = New SalidaMultiple(p_Application, p_Company)
        End If
        If DMS_Connector.Configuracion.ParamGenAddon.U_Usa_Fin.Trim().Equals("Y") Then
            m_blnFinanciamiento = True
            m_oFormularioPrestamo = New PrestamoFormularioConPermisos(p_Application, p_Company, menuFinanc)
            m_oFormularioPrestamo.CargaFormulario = AddressOf oGestorFormularios.CargaFormulario
            m_oFormularioPlanPagos = New PlanPagosFormulario(p_Application, p_Company, DBUser, DBPassword, Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLPlanPagos)
            m_oFormularioPrestamo.FormPlanPlagos = m_oFormularioPlanPagos
            m_oFormularioConfFinanc = New ConfiguracionFormularioConPermisos(p_Application, p_Company, menuFinanc)
            m_oMenuReportesFinanciamiento = New ReportesConPermisos(menuFinanc)
            m_oFormularioEstadoCuentas = New EstadosCuentaConPermisos(p_Application, p_Company)
            m_oFormularioHistoricoPagos = New HistoricoPagosConPermisos(p_Application, p_Company)
            m_oFormularioCuotasVencidas = New CuotasVencidasConPermisos(p_Application, p_Company)
            m_oFormularioSaldos = New SaldosConPermisos(p_Application, p_Company)
        End If
        If DMS_Connector.Configuracion.ParamGenAddon.U_Usa_Plc.Trim().Equals("Y") Then
            m_blnUsaPlacas = True
            m_oFormularioPlacas = New ExpedienteFormularioConPermisos(p_Application, p_Company, menuPlacas)
            m_oFormularioPlacas.CargaFormulario = AddressOf oGestorFormularios.CargaFormulario
            m_oFormularioPlacaGrupos = New GrupoFormularioConPermisos(p_Application, p_Company, menuPlacas)
            m_oFormularioPlacaGrupos.CargaFormulario = AddressOf oGestorFormularios.CargaFormulario
            m_oMenuReportesPlacas = New ReportesPlacasConPermisos(menuPlacas)
            m_oFormularioVehiculoTipoEvento = New VehiculosTipoEventoConPermisos(p_Application, p_Company)
            m_oFormularioContratoTraspaso = New ContratoTraspasoConPermisos(p_Application, p_Company)
            m_oFormularioComision = New ComisionConPermisos(p_Application, p_Company)
            m_oFormularioVehiculosProblemas = New VehiculosProblemasConPermisos(p_Application, p_Company)
        End If
        If DMS_Connector.Configuracion.ParamGenAddon.U_UsaAXEV.Trim().Equals("Y") Then
            m_blnUsaAsocXEspecif = True
            m_oFormularioAsocArticuloxEspecif = New AsociacionArticuloxEspecifConPermisos(p_Application, p_Company)
        End If
        m_oFormularioCitaXTipoAgenda = New CitasPorTipoAgendaFecha(p_Company, p_Application, menuInformesDMS, mc_strUIDCitasXTipo)
        m_oReporteOrdenesEspeciales = New ReporteOrdenesEspeciales(p_Company, p_Application, menuInformesDMS, mc_strUISCGD_ReporteOrdenes)
        m_oFormularioUnidadesVendidas = New UnidadesVendidasConPermisos(p_Company, p_Application, menuInformesDMS, mc_strUISCGD_RptUnidadesVend)
        m_oFormularioBalanceOT = New BalanceOrdenesTrabajo(p_Application, p_Company, menuInformesDMS, mc_strUIDFormBalanceOT)
        m_oFormularioBodegaProceso = New BodegaProceso(p_Company, p_Application, menuInformesDMS, mc_strUID_FORM_BodegasP)
        m_oFormularioFacturacionvehiculo = New FacturacionVehiculosPorVendedor(p_Company, p_Application, menuInformesDMS, mc_strUID_FORM_FacturacioVehi)
        m_oFormularioOrdenesDeTrabajoPorEstado = New OrdenesDeTrabajoPorEstado(p_Company, p_Application, menuInformesDMS, mc_strUID_FORM_OrdenesTrabajoEstado)
        m_oFormularioHistorialVehiculo = New HistorialVehiculo(p_Company, p_Application, menuInformesDMS, mc_strUID_FORM_ReporteHistorialVehiculo)
        m_oFormularioReporteFacturacionOT = New ReporteFacturacionOrdenesTrabajo(p_Company, p_Application, menuInformesDMS, mc_strUID_FORM_ReporteFacturacionOT)
        m_oFormularioFactutacionOTInternas = New FactutacionOTInternas(p_Company, p_Application, menuInformesDMS, mc_strUID_FORM_ReporteFacturacionInterna)
        m_oFormularioReporteAntiguedadVehiculos = New ReporteAntiguedadVehiculos(p_Company, p_Application, menuInformesDMS, mc_strUID_FORM_ReporteAntiguedadVehiculos)
        m_oFormularioReporteServiciosExternosXOrden = New ReporteServiciosExternosXOrden(p_Company, p_Application, menuInformesDMS, mc_strUID_FORM_ReporteServiciosExternosXOrden)
        m_oFormularioReporteFacturacionMecanicos = New ReporteFacturacionMecanicos(p_Company, p_Application, menuInformesDMS, mc_strUID_FORM_ReporteFacMecanicos)
        m_oFormularioReporteFinanciamientoContratoVentas = New ReporteFinanciamientoContratoVentas(p_Company, p_Application)
        m_oFormMantenEspecificacionPorModelo = New EspecificacionPorModeloCls(p_Application, p_Company, mc_strUISCGD_EspecificosModelo)
        m_oFormularioConfigNivelesAprob = New ConfiguracionNivAprobacion(p_Application, p_Company, strMenuConfigNivAprob)
        m_oFormConfInterfazFord = New ConfiguracionInterfazFord(p_Application, p_Company, mc_strUISCGD_ConfFordInterface)
        m_oFormConfIntTDS = New ConfiguracionInterfazTSD(SBO_Application, m_oCompany, mc_strUISCGD_ConfTSDInterface)
        m_oFormConfIntAudatex = New ConfiguracionInterfazAudatex(SBO_Application, m_oCompany, mc_strUISCGD_ConfAudatexInterface)
        m_oFormularioConfMsJ = New ConfiguracionMensajeriaDMS(p_Application, p_Company, mc_strUISCGD_FormConfMSJ)
        m_oMenuConfiguracionDMS = New ConfiguracionesDMSConPermisos()
        m_oFormularioParametrosAplicacion = New ParametrosDeAplicacionConPermisos(p_Application, p_Company, mc_strUISCGD_FormParamAplicacion)
        m_oFormularioAgendasConfiguracion = New AgendasConfiguracion(p_Application, p_Company, mc_strUISCGD_FormAgendasConfiguracion)
        m_oFormularioBusquedasCitas = New BusquedasCitasConPermisos(p_Application, p_Company, menuCitas, mc_strUISCGD_BusqCitas)
        m_oFormularioCitas = New CitasReservacion(p_Application, p_Company, menuCitas, mc_strUISCGD_Citas)
        m_oFormularioCargarPanelCitas = New CargarPanelCitasConPermisos(p_Application, p_Company, menuCitas, mc_strUISCGD_CargPanelCitas)
        m_oFormularioSuspensionAgenda = New AgendaSuspension(p_Application, p_Company, menuCitas, mc_strUISCGD_SuspenderAgenda)
        m_oFormularioListadoSolicitudEspecificos = New ListadoSolicitudEspecificosConPermisos(p_Application, p_Company, strMenuListadoSolEsp)
        m_oFormularioSolicitudEspecificos = New SolicitudEspecificosConPermisos(p_Application, p_Company, strMenuSolEsp)
        m_oFormularioIncluirRepOT = New IncluirRepuestosOT(p_Application, p_Company, strMenuIncluirRepOT)
        m_oFormularioSeleccionaRepuestosOT = New SeleccionarRepuestosOT(p_Application, p_Company, m_oFormularioIncluirRepOT)
        m_oFormularioIncluirGastoOT = New IncluirGastosCostosOT(p_Application, p_Company, strMenuIncluirGastosOT)
        m_oFormularioPedidoVehiculos = New PedidoDeVehiculos(p_Application, p_Company, mc_StrPedidoVehiculos)
        m_oFormularioEntradaDeVehiculos = New EntradaDeVehiculos(p_Application, p_Company, mc_strEntradaDeVehiculos)
        m_oFormularioCosteoDeEntradas = New CosteoDeEntradas(p_Application, p_Company, m_oFormularioSeleccionLineasRecepcion, mc_strCosteoDeEntradas)
        m_oFormularioDevolucionDeVehiculos = New DevolucionDeVehiculos(p_Application, p_Company, mc_strDevolucionDeVehiculos)
        m_oEmbarqueVehiculos = New EmbarqueVehiculos(p_Application, p_Company, strMenuEmbarqueVehiculos)
        m_oFormularioAvaUs = New AvaluoUsados(SBO_Application, m_oCompany, mc_strUISCGD_FormAVA)
        If False Then
            g_oFormularioVisitas = New Visita(p_Application, p_Company)
            g_oFormularioBusquedaControlProceso = New BusquedaControlProceso(p_Application, p_Company)
            g_oFormularioControlCrearVisita = New ControlCrearVisita(p_Application, p_Company)
            g_oFormularioControlVisita = New ControlVisita(p_Application, p_Company)
            g_oFormularioOfertaVentas = New OfertaVentas(p_Application, p_Company)
        End If
        m_oDimensionesContables = New DimensionContableDMS(SBO_Application, m_oCompany, mc_strUISCGD_DimensionContableDMS)
        m_oDimensionesContablesOTs = New DimensionContableDMSOTs(SBO_Application, m_oCompany, mc_strUISCGD_DimensionContableDMSOTs)
        m_oSolicitudOTEspecial = New SolicitudOrdenEspecial(SBO_Application, m_oCompany, mc_strUISCGD_SolicituOTE)
        m_oFormularioRazonSuspension = New RazonesSuspensionConPermisos(SBO_Application, m_oCompany)
        m_oFormularioFinAct = New FinalizaActividad(SBO_Application, m_oCompany, Environment.CurrentDirectory + My.Resources.Resource.frmFinalizaActividades)
        m_oFormularioAsignacionMultipleOT = New AsignacionMultipleConPermisos(SBO_Application, m_oCompany)
        m_oFormularioAdicionalesOT = New AdicionalesOTConPermisos(SBO_Application, m_oCompany)
        m_oFormularioTrackRep = New TrackingRepuestos(SBO_Application, m_oCompany)
        m_oFormularioTrackSolEspecificos = New TrackingSolEspecificos(SBO_Application, m_oCompany)
        m_oFormularioAdicionalesCitasArt = New BuscadorArticulosCitas(SBO_Application, m_oCompany)
        m_oFormularioDocumentoCompra = New DocumentoCompraConPermisos(SBO_Application, m_oCompany)
        m_oFormularioBuscarProveedores = New BuscadorProveedoresConPermisos(SBO_Application, m_oCompany)
        m_oFormularioOTEspecial = New OTEspecialConPermisos(SBO_Application, m_oCompany)
        m_oFormularioOrdenTrabajo = New OrdenTrabajoConPermisos(SBO_Application, m_oCompany, m_oFormularioAsignacionMultipleOT, m_oFormularioRazonSuspension, m_oFormularioFinAct, m_oFormularioTrackRep, m_oFormularioDocumentoCompra, m_oFormularioBuscarProveedores, m_oFormularioTrackSolEspecificos)
        m_oFormularioKardexInventarioVehiculo = New KardexInventarioVehiculo(SBO_Application, m_oCompany)
        'm_oFormularioCargaMasivaVehiculos = New CargaMasivaVehiculos()
        m_oFormularioSociosNegocios = New ReporteSociosNegocios(p_Company, p_Application, menuInformesDMS, "SCGD_RSN")
        m_oFormularioReporteVehiculosRecurrentesTaller = New ReporteVehiculosRecurrentesTaller(p_Application, p_Company, mc_strUID_FORM_ReporteVehiculosRecurrentesTaller)
        m_oFormularioReporteVentasXAsesorServicio = New ReporteVentasXAsesorServicio(p_Application, p_Company, mc_strUID_FORM_ReporteVentasXAsesorServicio)
        m_oCotizacion_ProcesaOT = New Cotizacion_ProcesaOT(p_Application, p_Company)

    End Function

    Private Sub AgregarMenus()
        If m_blnUsaVehículos Then
            Call m_oVehiculos.AddMenuItems()
            If m_blnUsaPlanDeVentas Then
                m_oInventarioVehiculos.AddMenuItems()
            End If
            If m_blnUsaCosteoVehículo Then
                m_oReportesCosteo.AddMenuItems()
            End If
            Call m_oListaCVXUnidad.AddMenuItems()
            'm_oFormularioCargaMasivaVehiculos.AddMenuItems()
        End If
        If m_blnUsaOrdenesDeTrabajo Then
            Call m_oCotizacion.AddMenuItems()
            Call m_oCotizacion.AddMenuItemsFI()
            Call m_oFacturaInterna.AddMenuItems()
            Call m_oEstadosOT.AddMenuItems()
        End If

        If m_blnUsaPlanDeVentas Then
            m_udoMenusPlanVentas = Utilitarios.MenusPlandeVentas(m_oCompany.Server, m_oCompany.CompanyDB, SBO_Application.Language)
            Call m_oCVenta.AddMenuItems()
            Call m_oReporteCV.AddMenuItems()
            Call m_oBuscadorCV.AddMenuItems(m_udoMenusPlanVentas)
            Call m_oListadoCV.AddMenuItems()
            Call m_oListadoContratosReversados.AddMenuItems()
            Call m_oListaContratos_a_Reversar.AddMenuItems()
            Call m_oListaContratosSegPV.AddMenuItems()
            Call m_oTrasladoCostos.AddMenuItems()
            oGestorMenu.AgregaSubMenu(m_oFormularioPresupuestos)
            oGestorMenu.AgregaSubMenu(m_oRefacturacion)
        End If

        If m_blnUsaCosteoVehículo Then
            Call m_oVehiculosACostear.AddMenuItems()
            oGestorMenu.AgregaSubMenu(m_oCosteoMultiplesUnidades)
            oGestorMenu.AgregaSubMenu(m_oSalidasMultiplesUnidades)
        End If

        If m_blnUsaOrdenesDeTrabajo Then
            m_oAgendas.AgregaMenu()
            If blnDraft AndAlso m_oFormularioRequisiciones IsNot Nothing Then
                oGestorMenu.AgregaSubMenu(m_oFormularioRequisiciones)
                oGestorMenu.AgregaSubMenu(m_oFormularioListadoRequisiciones)
            End If
        End If

        If DMS_Connector.Configuracion.ParamGenAddon.U_Usa_Fin.Trim().Equals("Y") Then
            oGestorMenu.AgregarMenu(menuFinanc, My.Resources.Resource.MenuFinanciamiento, Windows.Forms.Application.StartupPath & "\" & "financ.bmp")
            oGestorMenu.AgregaSubMenu(m_oFormularioPrestamo)
            oGestorMenu.AgregaSubMenu(m_oFormularioConfFinanc)
            oGestorMenu.AgregaSubMenu(m_oMenuReportesFinanciamiento, 2)
            oGestorMenu.AgregaSubMenu(m_oFormularioEstadoCuentas)
            oGestorMenu.AgregaSubMenu(m_oFormularioHistoricoPagos)
            oGestorMenu.AgregaSubMenu(m_oFormularioCuotasVencidas)
            oGestorMenu.AgregaSubMenu(m_oFormularioSaldos)
        End If

        If DMS_Connector.Configuracion.ParamGenAddon.U_Usa_Plc.Trim().Equals("Y") Then
            oGestorMenu.AgregarMenu(menuPlacas, My.Resources.Resource.MenuPlacas, Windows.Forms.Application.StartupPath & "\" & "placas.bmp")
            oGestorMenu.AgregaSubMenu(m_oFormularioPlacas)
            oGestorMenu.AgregaSubMenu(m_oFormularioPlacaGrupos)
            oGestorMenu.AgregaSubMenu(m_oMenuReportesPlacas, 2)
            oGestorMenu.AgregaSubMenu(m_oFormularioVehiculoTipoEvento)
            oGestorMenu.AgregaSubMenu(m_oFormularioContratoTraspaso)
            oGestorMenu.AgregaSubMenu(m_oFormularioComision)
            oGestorMenu.AgregaSubMenu(m_oFormularioVehiculosProblemas)
        End If

        If DMS_Connector.Configuracion.ParamGenAddon.U_UsaAXEV.Trim().Equals("Y") Then oGestorMenu.AgregaSubMenu(m_oFormularioAsocArticuloxEspecif)

        '*********************************************************************************************************
        'Agregando menus para INFORMES DMS
        oGestorMenu.AgregarMenu(menuInformesDMS, My.Resources.Resource.InformesDMS, Windows.Forms.Application.StartupPath & "\" & "InfDMS.bmp")
        oGestorMenu.AgregaSubMenu(m_oFormularioCitaXTipoAgenda)
        oGestorMenu.AgregaSubMenu(m_oReporteOrdenesEspeciales)
        oGestorMenu.AgregaSubMenu(m_oFormularioUnidadesVendidas)
        oGestorMenu.AgregaSubMenu(m_oFormularioBalanceOT)
        oGestorMenu.AgregaSubMenu(m_oFormularioBodegaProceso)
        oGestorMenu.AgregaSubMenu(m_oFormularioSociosNegocios)
        oGestorMenu.AgregaSubMenu(m_oFormularioReporteVehiculosRecurrentesTaller)
        oGestorMenu.AgregaSubMenu(m_oFormularioReporteVentasXAsesorServicio)
        oGestorMenu.AgregaSubMenu(m_oFormularioFacturacionvehiculo)
        oGestorMenu.AgregaSubMenu(m_oFormularioOrdenesDeTrabajoPorEstado)
        oGestorMenu.AgregaSubMenu(m_oFormularioHistorialVehiculo)
        oGestorMenu.AgregaSubMenu(m_oFormularioReporteFacturacionOT)
        oGestorMenu.AgregaSubMenu(m_oFormularioFactutacionOTInternas)
        oGestorMenu.AgregaSubMenu(m_oFormularioReporteAntiguedadVehiculos)
        oGestorMenu.AgregaSubMenu(m_oFormularioReporteServiciosExternosXOrden)
        oGestorMenu.AgregaSubMenu(m_oFormularioReporteFacturacionMecanicos)
        oGestorMenu.AgregaSubMenu(m_oFormularioReporteFinanciamientoContratoVentas)
        m_oPermisos.AddMenuItems()
        oGestorMenu.AgregaSubMenu(m_oNivelesPV)
        m_oConfiguracionGeneral.AddMenuItems()
        oGestorMenu.AgregaSubMenu(m_oFormMantenEspecificacionPorModelo)
        oGestorMenu.AgregaSubMenu(m_oFormularioConfigNivelesAprob)
        oGestorMenu.AgregaSubMenu(m_oFormConfInterfazFord)
        ' Configuraciones Interfaz TSD  *******************************
        oGestorMenu.AgregaSubMenu(m_oFormConfIntTDS)
        ' Configuraciones Interfaz TSD  *******************************
        ' Configuraciones Interfaz Audatex  *******************************
        oGestorMenu.AgregaSubMenu(m_oFormConfIntAudatex)
        ' Configuraciones Interfaz Audatex  *******************************
        oGestorMenu.AgregaSubMenu(m_oFormularioConfMsJ)
        oGestorMenu.AgregaSubMenu(m_oMenuConfiguracionDMS, 2)
        oGestorMenu.AgregaSubMenu(m_oFormularioParametrosAplicacion)
        oGestorMenu.AgregaSubMenu(m_oFormularioAgendasConfiguracion)
        oGestorMenu.AgregarMenu(menuCitas, My.Resources.Resource.menuCitas, Windows.Forms.Application.StartupPath & "\" & "citas.bmp")
        oGestorMenu.AgregaSubMenu(m_oFormularioBusquedasCitas)
        If String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType) Or DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType = "1" Then
            oGestorMenu.AgregaSubMenu(m_oFormularioCitas)
        End If
        oGestorMenu.AgregaSubMenu(m_oFormularioCargarPanelCitas)
        oGestorMenu.AgregaSubMenu(m_oFormularioSuspensionAgenda)
        oGestorMenu.AgregaSubMenu(m_oFormularioBusquedaOT)
        oGestorMenu.AgregaSubMenu(m_oFormularioListadoSolicitudEspecificos)
        oGestorMenu.AgregaSubMenu(m_oFormularioSolicitudEspecificos)
        oGestorMenu.AgregaSubMenu(m_oFormularioIncluirRepOT)
        oGestorMenu.AgregaSubMenu(m_oFormularioIncluirGastoOT)
        oGestorMenu.AgregarMenu("SCGD_CEIM", My.Resources.Resource.MenuComprasEImportacion, Windows.Forms.Application.StartupPath & "\" & "citas.bmp")
        oGestorMenu.AgregaSubMenu(m_oFormularioPedidoVehiculos)
        oGestorMenu.AgregaSubMenu(m_oFormularioEntradaDeVehiculos)
        oGestorMenu.AgregaSubMenu(m_oFormularioCosteoDeEntradas)
        oGestorMenu.AgregaSubMenu(m_oFormularioDevolucionDeVehiculos)
        oGestorMenu.AgregaSubMenu(m_oEmbarqueVehiculos)
        ' Avalúo Vehículos Usados  *******************************
        oGestorMenu.AgregaSubMenu(m_oFormularioAvaUs)
        ' Avalúo Vehículos Usados  *******************************
        '*****************************PROTOTIPOS****************************************************************************
        If False Then
            oGestorMenu.AgregarMenu("SCGD_PRO", "Prototipo", Nothing)
            oGestorMenu.AgregaSubMenu(g_oFormularioVisitas)
            oGestorMenu.AgregaSubMenu(g_oFormularioBusquedaControlProceso)
            oGestorMenu.AgregaSubMenu(g_oFormularioControlCrearVisita)
            oGestorMenu.AgregaSubMenu(g_oFormularioControlVisita)
            oGestorMenu.AgregaSubMenu(g_oFormularioOfertaVentas)
        End If

        '*********************************************************************************************************
        oGestorMenu.AgregaSubMenu(m_oDimensionesContables)
        oGestorMenu.AgregaSubMenu(m_oDimensionesContablesOTs)
        oGestorMenu.AgregaSubMenu(m_oSolicitudOTEspecial)
        oGestorMenu.AgregaSubMenu(m_oFormularioOrdenTrabajo)
        oGestorMenu.AgregaSubMenu(m_oFormularioKardexInventarioVehiculo)
        ConstructorDisponibilidadEmpleados.AgregarMenu()

        Call m_oFormularioPermisosVendedoresXTI.AddMenuItems()
        If m_blnUsaPlanDeVentas Then
            m_oLineasFactura.AddMenuItems()
            m_oLineasDesgloce.AddMenuItems()
        End If
        If m_blnUsaCosteoVehículo Then
            m_oTransaccionesCompras.AddMenuItems()
        End If
        If m_blnUsaVehículos Then
            m_oPropiedades.AddMenuItems()
        End If
        If Not Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO) Then
            oGestorMenu.AgregarMenu(_menuSCG, My.Resources.Resource.NombreMenuAddonDMS, Windows.Forms.Application.StartupPath & "\" & _imgDMSSBO)
            oGestorMenu.AgregarSubMenu(_menuDMS, My.Resources.Resource.NombreSubMenuAdddonDMS, -1, Windows.Forms.Application.StartupPath & "\" & _imgDMSSBO, _menuSCG)
        End If
        ConstructorRestablecerCantidadesPendientes.AgregarMenu()
        ConstructorCitas.AgregarMenu()
        ConstructorReporteBodegaReservas.AgregarMenu()
        AdministradorLicencias.AgregarMenu()
        ReAperturaOTNormal.AgregarMenu()

        '*****************************Interface John Deere****************************************************************************
        InterfaceJohnDeereModulo.AgregarMenu()
        'InterfaceJohnDeereConfiguration.AgregarMenu()
        '*********************************************************************************************************
    End Sub

#End Region
End Class
