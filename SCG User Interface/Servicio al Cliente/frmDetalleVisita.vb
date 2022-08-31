Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Imports SCG.UX.Windows


Namespace SCG_User_Interface

    Public Class frmDetalleVisita

#Region "Declaraciones"

#Region "Objetos Generales"

        Private objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        Private m_alstFases As ArrayList
        Private m_alstFasesProduccion As ArrayList
        Private m_alstMensajes As ArrayList

        Private WithEvents objfrmOperYOrden As frmOrden

#End Region

#Region "Acceso a Datos"

        Private m_dstVisita As DMSOneFramework.VisitaDataset

        Private m_adpOrden As OrdenTrabajoDataAdapter
        Public m_dstOrden As OrdenTrabajoDataset

        Private m_drwVisita As DMSOneFramework.VisitaDataset.SCGTA_TB_VisitaRow

        Private m_adpVisita As SCGDataAccess.VisitasDataAdapter

        Private m_cnConeccionVisita As SqlClient.SqlConnection
        Private m_tnnTransaccionVisita As SqlClient.SqlTransaction

#End Region

#Region "Constantes"

        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strCodTipoOrden As String = "CodTipoOrden"
        Private Const mc_strFecha_aperturaOrden As String = "Fecha_apertura"
        Private Const mc_strFecha_cierreOrden As String = "Fecha_cierre"
        Private Const mc_strEstadoOrden As String = "EstadoDesc"
        Private Const mc_strObservacion As String = "Observacion"
        Private Const mc_strCodMarcaOrden As String = "CodMarca"
        Private Const mc_strDescMarca As String = "DescMarca"
        Private Const mc_strDescModelo As String = "DescModelo"
        Private Const mc_strDescEstilo As String = "DescEstilo"
        Private Const mc_strDescTipoOrden As String = "TipoDesc"
        Private Const mc_strDescripcionEstado As String = "DescipcionEstado"

        Private Const mc_NumEstadoOrden_NoIniciada As String = "1"
        Private Const mc_NumEstadoOrden_Proceso As String = "2"
        Private Const mc_NumEstadoOrden_Suspendida As String = "3"
        Private Const mc_NumEstadoOrden_Finalizada As String = "4"
        Private Const mc_NumEstadoOrden_Cancelada As String = "5"

        'Principal
        Private Const mc_PriEstado_NoIniciada As String = "No Iniciada"
        Private Const mc_PriEstado_Proceso As String = "Proceso"
        Private Const mc_PriEstado_Suspendida As String = "Suspendida"
        Private Const mc_PriEstado_Finalizada As String = "Finalizada"
        Private Const mc_PriEstado_Rechazo As String = "Rechazo"
        Private Const mc_PriEstado_Cerrada As String = "Cerrada"
        Private Const mc_PriEstado_Cancelada As String = "Cancelada"

#End Region

#Region "Enums"

        Public Enum enumEstadoVisita

            scgProceso = 1
            scgSuspendida = 2
            scgFinalizada = 3
            scgEntregada = 4

        End Enum

#End Region

#End Region

#Region "Constructor"

        Public Sub New()
            MyBase.New()

            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        End Sub

        Public Sub New(ByVal p_blnEstado As Boolean)

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        End Sub

#End Region

#Region "Métodos"

        Public Sub cargarDatos(ByRef p_dstVisita As DMSOneFramework.VisitaDataset, ByVal decNoVisita As Decimal)

            Try
                Dim intNoVehiculo As String  'variable para saber cual vehículo cargar.
                'Dim strCardCode As String     'Variable para cliente cargar.


                cargarDatosVisita(p_dstVisita, decNoVisita)

                'Se igual el dataset referenciado con uno declarado en la forma para poder usar sus valores
                m_dstVisita = p_dstVisita

                'Se carga el row seleccionado por consultar
                m_drwVisita = m_dstVisita.SCGTA_TB_Visita.FindByNoVisita(decNoVisita)

                'Se carga el Número de vehículo en la variable intNoVehiculo con el objetivo de poder enviarla 
                'al procedimiento cargarvehiculos y que traiga el vehículo seleccionado.
                intNoVehiculo = m_drwVisita.NoVehiculo

                Call cargarDatosOrdenes(decNoVisita)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub cargarDatosVisita(ByRef p_dstVisita As DMSOneFramework.VisitaDataset, ByVal decNoVisita As Integer)

            Dim strHora As String = ""
            Dim strMinutos As String = ""
            Dim datHora As Date

            Try
                'Procedimiento para cargar los datos en el tab de datos generales del Visita. 
                'Recibe por referencia el dataset que se cargo en el maestro.

                m_dstVisita = p_dstVisita

                m_drwVisita = m_dstVisita.SCGTA_TB_Visita.FindByNoVisita(decNoVisita)

                Me.txtNoVisita.Text = m_drwVisita.NoVisita

                txtCodCliente.Text = m_drwVisita.CardCode

                txtNombreCliente.Text = m_drwVisita.CardName

                txtIdentCliente.Text = m_drwVisita.IdentCliente

                txtNoVehiculo.Text = m_drwVisita.NoVehiculo

                txtMarca.Text = m_drwVisita.DescMarca

                txtPlaca.Text = m_drwVisita.Placa

                txtEstilo.Text = m_drwVisita.DescEstilo

                txtCono.Text = m_drwVisita.Cono

                txtModelo.Text = m_drwVisita.DescModelo

                If Not m_drwVisita.IsAsesorNull Then

                    Me.txtCodAsesor.Text = m_drwVisita.Asesor
                    Me.txtNombreAsesor.Text = m_drwVisita.AsesorNombre

                End If

                clsUtilidadCombos.CargarComboEstadoVisitas(cboEstadoVisita)
                If Not m_drwVisita.IsEstadoNull Then
                    cboEstadoVisita.SelectedIndex = m_drwVisita.CodEstado - 1
                End If

                If Not m_drwVisita.IsFecha_aperturaNull Then
                    datHora = m_drwVisita.Fecha_apertura
                    Me.txtFechaApertura.Text = m_drwVisita.Fecha_apertura.ToShortDateString
                    Me.txtHoraApertura.Text = datHora.ToShortTimeString
                End If

                If Not m_drwVisita.IsFecha_compromisoNull Then
                    txtFechaCompromiso.Text = m_drwVisita.Fecha_compromiso.Date
                Else
                    txtFechaCompromiso.Text = m_drwVisita.Fecha_apertura.Date
                End If

                If Not m_drwVisita.IsHora_compromisoNull Then
                    If CStr(m_drwVisita.Hora_compromiso).Length = 3 Then
                        strHora = CStr(m_drwVisita.Hora_compromiso).Substring(0, 1)
                        strMinutos = CStr(m_drwVisita.Hora_compromiso).Substring(1, 2)
                    ElseIf CStr(m_drwVisita.Hora_compromiso).Length = 4 Then
                        strHora = CStr(m_drwVisita.Hora_compromiso).Substring(0, 2)
                        strMinutos = CStr(m_drwVisita.Hora_compromiso).Substring(2, 2)
                    ElseIf CStr(m_drwVisita.Hora_compromiso).Length <= 2 Then
                        strHora = CStr(m_drwVisita.Hora_compromiso)
                        strMinutos = "00"
                    End If

                    datHora = New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, strHora, strMinutos, 0)
                    txtHoraCompromiso.Text = datHora.ToShortTimeString
                End If

                If Not m_drwVisita.IsFecha_cierreNull Then
                    Me.dtpCierre.Value = m_drwVisita.Fecha_cierre

                End If

                VisualizarUDFVisita.Conexion = SCGDataAccess.DAConexion.ConnectionString

                VisualizarUDFVisita.Where = "NoVisita = " & CInt(txtNoVisita.Text) & ""

                'VisualizarUDFVisita.CargarDatosUDF("NoVisita = '" & CInt(txtNoVisita.Text) & "'")

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub cargarDatosOrdenes(ByVal intNoVisita As Integer)


            m_dstOrden = New OrdenTrabajoDataset
            m_adpOrden = New OrdenTrabajoDataAdapter


            'Se carga el dataset con las ordenes asociadas al Visita
            Call m_adpOrden.Fill(m_dstOrden, intNoVisita)

            LlenarEstadoOrdenTrabajoResources(m_dstOrden)

            'Se carga el datagrid con el dataset de ordenes
            dtgOrdenes.DataSource = m_dstOrden


            'En el evento click del datagrid Visitas se obtiene el número de Visita y el se carga un dataset con las ordenes asociadas.
            'estiloGridOrdenes()

        End Sub


        Private Function ActualizarOrdenTrabajo(ByRef p_drwOrden As OrdenTrabajoDataset.SCGTA_TB_OrdenRow) As Boolean

            Dim intResultVerific As Integer
            Dim objAdapter As SCGDataAccess.OrdenTrabajoDataAdapter
            Dim strMensajeError As String = ""
            Dim blnFinalizarOrden As Boolean
            Dim blnSuspenderOrden As Boolean
            'Dim blnCancelarOrden As Boolean
            Dim intUpdateResult As Integer
            Dim objDARepuestos As RepuestosxOrdenDataAdapter
            Dim objDASuministros As SuministrosDataAdapter
            Dim blnResultado As Boolean

            intResultVerific = VerificarCambiaEstado()
            blnFinalizarOrden = False

            'Actualiza las bodegas de los repuestos
            objDARepuestos = New RepuestosxOrdenDataAdapter
            objDASuministros = New SuministrosDataAdapter

            If intResultVerific >= 0 Then
                If p_drwOrden.Estado <> mc_NumEstadoOrden_Cancelada Then

                    'Agregado 10/08/06. Alejandra. Al finalizar la orden desde el tab Principal,
                    ' debe finalizar las fases de produccion que tienen estado "En Proceso" o "Suspendida"
                    If (p_drwOrden.Estado <> mc_NumEstadoOrden_Finalizada And p_drwOrden.Estado <> mc_NumEstadoOrden_Cancelada And (cboEstadoVisita.SelectedIndex + 1) = 3) Then
                        'If ValidarDatosSAP() Then 'En la siguiente llamada a la funcion se calcularán costos, por lo tanto debe validar algunos campos antes
                        blnFinalizarOrden = FinalizarTodasFasesOrden(False, p_drwOrden.NoOrden)
                        'Else
                        '   blnFinalizarOrden = False
                        'End If
                    ElseIf (p_drwOrden.Estado <> mc_NumEstadoOrden_Finalizada And p_drwOrden.Estado <> mc_NumEstadoOrden_Cancelada And (cboEstadoVisita.SelectedIndex + 1) = 2) Then
                        blnSuspenderOrden = SuspenderTodasFasesOrden(p_drwOrden.NoOrden)
                    End If
                    '''''''''''''

                    'With m_dtsOrden

                    'drdOrden = .SCGTA_TB_Orden.FindByNoOrden(m_strNoOrden)

                    If Not IsNothing(p_drwOrden) And (blnFinalizarOrden Or blnSuspenderOrden) Then

                        With p_drwOrden

                            If ((cboEstadoVisita.SelectedIndex + 1) = enumEstadoVisita.scgFinalizada) Then

                                .Estado = mc_NumEstadoOrden_Finalizada
                                .EstadoDesc = mc_PriEstado_Finalizada
                                .Fecha_cierre = objUtilitarios.CargarFechaHoraServidor

                                DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.ActualizarEstadoCotizacion(p_drwOrden.NoCotizacion, mc_PriEstado_Finalizada)
                                m_alstMensajes.Add(p_drwOrden.NoOrden)

                                'adpMensajeria.CreaMensajeDMS_SBO_Cotizacion("La orden de trabajo ha sido finalizada", MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, p_drwOrden.NoOrden)
                            ElseIf ((cboEstadoVisita.SelectedIndex + 1) = enumEstadoVisita.scgSuspendida) Then

                                .Estado = mc_NumEstadoOrden_Suspendida
                                .EstadoDesc = mc_PriEstado_Suspendida
                                .Fecha_cierre = objUtilitarios.CargarFechaHoraServidor

                                DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.ActualizarEstadoCotizacion(p_drwOrden.NoCotizacion, mc_PriEstado_Suspendida)

                                'Genera mensaje en SBO para el asesor
                                m_alstMensajes.Add(p_drwOrden.NoOrden)
                                'adpMensajeria.CreaMensajeDMS_SBO_Cotizacion("La orden de trabajo ha sido suspendida", MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, p_drwOrden.NoOrden)
                            End If 'cboEstado <> Finalizada

                        End With

                        objAdapter = New SCGDataAccess.OrdenTrabajoDataAdapter

                        intUpdateResult = objAdapter.Actualizar(m_dstOrden, m_cnConeccionVisita, m_tnnTransaccionVisita)

                        'If blnFinalizarOrden And intUpdateResult <> 0 Then
                        '    CalculoCostosCierreOrden(drdOrden.NoOrden)
                        blnResultado = True     'End If
                    Else
                        blnResultado = False
                    End If

                    'End With

                    Return blnResultado
                Else
                    Return True
                End If
            Else

                objSCGMSGBox.msgExclamationCustom(strMensajeError)


                Return False

            End If

        End Function

        Private Function VerificarCambiaEstado() As Integer
            Dim intValueResult As Integer

            If m_drwVisita.CodEstado <> (cboEstadoVisita.SelectedIndex + 1) Then
                intValueResult = (cboEstadoVisita.SelectedIndex + 1)
            Else
                intValueResult = 0
            End If

            Return intValueResult

        End Function

        Private Function ValidarDatosSAP()

            'Valida que el tipo de cambio y el periodo fiscal sean validos antes de realizar calculo de costos

            Dim blnValido As Boolean = True
            Dim objBLSBO As BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal
            Dim strMonedaLocal As String
            Dim strMonedaSistema As String

            Try

                objBLSBO = New BLSBO.GlobalFunctionsSBO
                strMonedaSistema = objBLSBO.RetornarMonedaSistema
                strMonedaLocal = objBLSBO.RetornarMonedaLocal
                If strMonedaSistema <> strMonedaLocal Then
                    decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(strMonedaSistema, Today, strConectionString, True)
                Else
                    decTipoCambio = 1
                End If
                If decTipoCambio <> -1 Then

                    If SCGDataAccess.Utilitarios.GetPostingPeriod(Today).Trim <> "" Then

                    Else

                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOPeriodoFiscalInvalido)
                        blnValido = False


                    End If
                Else

                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOActualizarTipoCambio)
                    blnValido = False
                End If

                Return blnValido

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try
        End Function

        Private Function VerificaExistenPendientes(ByVal p_strNoOrden As String) As Boolean
            Dim blnResult As Boolean = True

            If ValidarColaboradoresAsignados(p_strNoOrden) <> 0 Then
                blnResult = False
            End If

            If ValidarItemsPendientes(p_strNoOrden) <> 0 Then
                blnResult = False
            End If
            If ValidarSolicitudEspecificosPendientes(p_strNoOrden) <> 0 Then
                blnResult = False
            End If

            If ValidarSuministrosNoTrasladados(p_strNoOrden) <> 0 Then
                blnResult = False
            End If

            Return blnResult

        End Function

        Private Sub SuspenderFase(ByVal p_intNoFase As Integer, ByVal p_intNoSuspension As Integer, _
                                  ByVal p_strNoOrden As String)
            Dim objDA As New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim dstColaborador As New ColaboradorDataset

            objDA.SelColabIniciadosXOrdenXFase(dstColaborador, p_strNoOrden, p_intNoFase)
            ModificarDataSet(dstColaborador)
            objDA.UpdateSuspender(dstColaborador.SCGTA_TB_ControlColaborador, 0, "Manual", Nothing)

            ProcesoSuspenderFaseXOrden(p_strNoOrden, p_intNoFase)

        End Sub

        Private Sub ProcesoSuspenderFaseXOrden(ByVal p_strNoOrden As String, ByVal p_intFase As Integer)

            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
            objDA.SuspenderFase(p_strNoOrden, p_intFase, m_cnConeccionVisita, m_tnnTransaccionVisita)

        End Sub

        Private Function SuspenderTodasFasesOrden(ByVal p_strNoOrden As String)

            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
            Dim strEstadoFase As String
            Dim intIndice As Integer
            Dim blnSuspender As Boolean
            Dim drdFases As SqlClient.SqlDataReader

            Try
                blnSuspender = False
                drdFases = objUtilitarios.ReaderFasesProd(p_strNoOrden)
                m_alstFasesProduccion = Nothing
                m_alstFasesProduccion = New ArrayList
                While drdFases.Read
                    m_alstFasesProduccion.Add(drdFases.Item(1))
                End While
                drdFases.Close()
                For intIndice = 0 To m_alstFasesProduccion.Count - 1

                    strEstadoFase = objUtilitarios.retornaEstadoFase(p_strNoOrden, m_alstFasesProduccion(intIndice))

                    If (strEstadoFase = mc_PriEstado_Proceso) Then
                        SuspenderFase(m_alstFasesProduccion(intIndice), 0, p_strNoOrden)
                    End If

                Next intIndice
                blnSuspender = True

                Return blnSuspender
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            Finally
                'Agregado 05072010
                Call m_cnConeccionVisita.Close()

            End Try
        End Function

        Private Function ValidarColaboradoresAsignados(ByVal p_strNoOrden As String) As Integer
            Dim adpControlColaborador As New SCGDataAccess.ColaboradorDataAdapter(True)
            Dim intResult As Integer

            intResult = adpControlColaborador.VerificarColAsig(p_strNoOrden, m_cnConeccionVisita, m_tnnTransaccionVisita)

            Return intResult

        End Function

        Private Function ValidarSolicitudEspecificosPendientes(ByVal p_strNoOrden As String) As Integer
            Dim adpSolicitudEspecificos As New SCGDataAccess.SolicitudEspecificosDataAdapter
            Dim dtsSolicitudEspecificos As New SolicitudEspecificosDataset
            adpSolicitudEspecificos.Fill(dtsSolicitudEspecificos, , p_strNoOrden, , , , 0, , , , , , , , m_cnConeccionVisita, m_tnnTransaccionVisita, True)
            Dim intResult As Integer

            intResult = dtsSolicitudEspecificos.SCGTA_SP_SelSolicitudEspecifico.Rows.Count

            Return intResult

        End Function

        Private Function ValidarSuministrosNoTrasladados(ByVal p_strNoOrden As String) As Integer

            Dim intResult As Integer
            Dim a_drwSuministros() As System.Data.DataRow
            Dim adpSum As New SuministrosDataAdapter()
            Dim dtsSum As New SuministrosDataset
            adpSum.Fill(dtsSum, p_strNoOrden, -1, -1, m_cnConeccionVisita, m_tnnTransaccionVisita)

            a_drwSuministros = dtsSum.SCGTA_VW_Suministros.Select("Trasladada = 3")

            intResult = a_drwSuministros.Length
            Return intResult

        End Function

        Private Function ValidarItemsPendientes(ByVal p_strNoOrden As String) As Integer
            Dim adpRepXEstado As New SCGDataAccess.RepuestosxEstadoDataAdapter
            Dim intResult As Integer

            intResult = adpRepXEstado.ValidarItemsPendientes(p_strNoOrden)

            Return intResult

        End Function

        Private Function HayColaboradoresPendientesEnAlgunaFase(ByVal p_strNoOrden As String, Optional ByVal p_intValidarSuspencion As Integer = 0) As Boolean
            'Agregado 10/08/06. Alejandra. Determina si hay colaboradores iniciados en alguna de todas las fases
            Dim blnColaboradores As Boolean
            Dim drdFases As SqlClient.SqlDataReader = Nothing

            Try
                m_alstFasesProduccion = New ArrayList
                blnColaboradores = False

                drdFases = objUtilitarios.ReaderFasesProd(p_strNoOrden)

                While drdFases.Read
                    m_alstFasesProduccion.Add(drdFases.Item(1))
                    If VerificarColaboradoresPendientes(p_strNoOrden, drdFases.Item(1), p_intValidarSuspencion) Then
                        blnColaboradores = True
                    End If
                End While

                Return blnColaboradores

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            Finally
                If Not drdFases is Nothing then drdFases.Close()
            End Try
        End Function

        Private Function VerificarColaboradoresPendientes(ByVal p_NoOrden As String, _
                                                          ByVal p_NoFase As Integer, _
                                                          Optional ByVal p_intValidarSuspencion As Integer = 0) As Boolean
            Dim objDA As New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim blnResult As Boolean

            blnResult = objDA.VerificarColPendi(p_NoOrden, p_NoFase, p_intValidarSuspencion)

            Return blnResult

        End Function

        Private Sub ModificarDataSet(ByRef p_dstColab As ColaboradorDataset)
            'Establece el campo Check en True para que los rows cambien al estado Modified y puedan
            'ser detectados por el Update
            Dim drw As ColaboradorDataset.SCGTA_TB_ControlColaboradorRow

            Try

                For Each drw In p_dstColab.SCGTA_TB_ControlColaborador.Rows
                    drw.Check = True
                Next

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try

        End Sub

        Private Sub FinalizarFase(ByVal intFase As Integer, ByVal p_strNoOrden As String)

            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
            Dim objDACol As DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim dstColaborador As New ColaboradorDataset

            If VerificarColaboradoresPendientes(p_strNoOrden, intFase) Then

                objDACol = New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter

                'Finaliza los colaboradores de la fase
                objDACol.SelColaboradoresAFinalizar(dstColaborador, p_strNoOrden, intFase)
                ModificarDataSet(dstColaborador)
                objDACol.UpdateFinalizar(dstColaborador.SCGTA_TB_ControlColaborador, "Manual", m_cnConeccionVisita, m_tnnTransaccionVisita)

                objDA.FinalizarFase(p_strNoOrden, intFase, m_cnConeccionVisita, m_tnnTransaccionVisita)

            Else 'No hay colaboradores Pendientes

                objDA.FinalizarFase(p_strNoOrden, intFase, m_cnConeccionVisita, m_tnnTransaccionVisita)

            End If
        End Sub

        Private Function FinalizarTodasFasesOrden(ByVal p_blnCancelarOrden As Boolean, _
                                                  ByVal p_strNoOrden As String)

            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
            Dim strEstadoFase As String
            Dim intIndice As Integer
            Dim blnFinalizar As Boolean
            Dim drdFases As SqlClient.SqlDataReader

            Try
                blnFinalizar = False

                drdFases = objUtilitarios.ReaderFasesProd(p_strNoOrden)
                m_alstFasesProduccion = Nothing
                m_alstFasesProduccion = New ArrayList
                While drdFases.Read
                    m_alstFasesProduccion.Add(drdFases.Item(1))
                End While
                drdFases.Close()
                For intIndice = 0 To m_alstFasesProduccion.Count - 1

                    strEstadoFase = objUtilitarios.retornaEstadoFase(p_strNoOrden, m_alstFasesProduccion(intIndice), m_cnConeccionVisita, m_tnnTransaccionVisita)

                    If (strEstadoFase = mc_PriEstado_Suspendida Or strEstadoFase = mc_PriEstado_Proceso) Then
                        FinalizarFase(m_alstFasesProduccion(intIndice), p_strNoOrden)
                    End If

                Next intIndice
                blnFinalizar = True

                Return blnFinalizar

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            Finally
                'Agregado 05072010
                Call m_cnConeccionVisita.Close()

            End Try
        End Function

#End Region

#Region "Eventos"

        Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click

            Try

                Me.Dispose()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
            Dim drwOrden As OrdenTrabajoDataset.SCGTA_TB_OrdenRow
            Dim blnFinalizada As Boolean = True
            Dim blnProcesarFinalizarOSuspender As Boolean = False
            Dim blnSoloCambiarEstado As Boolean = False
            Dim intNumeroMensaje As Integer
            Dim adpMensajeria As New SCGDataAccess.MensajeriaSBOTallerDataAdapter

            Try

                'UDFS
                VisualizarUDFVisita.UpdateDatosUDF(Me)
                VisualizarUDFVisita.LimpiarUDF()

                If m_drwVisita.CodEstado <> (cboEstadoVisita.SelectedIndex + 1) Then

                    m_alstMensajes = Nothing
                    m_alstMensajes = New ArrayList

                    Select Case m_drwVisita.CodEstado
                        Case enumEstadoVisita.scgEntregada
                            If (cboEstadoVisita.SelectedIndex + 1) <> enumEstadoVisita.scgEntregada Then
                                MessageBox.Show(My.Resources.ResourceUI.MensajeNoSePuedeCambiarEstadoVisitaFinalizada)
                            End If
                        Case enumEstadoVisita.scgFinalizada
                            Select Case (cboEstadoVisita.SelectedIndex + 1)
                                Case enumEstadoVisita.scgProceso
                                    MessageBox.Show(My.Resources.ResourceUI.MensajeNoSePuedeReiniciarVisitaFinalizada)
                                Case enumEstadoVisita.scgSuspendida
                                    MessageBox.Show(My.Resources.ResourceUI.MensajeNoSePuedeSuspenderVisitaFinalizada)
                                Case enumEstadoVisita.scgEntregada
                                    blnSoloCambiarEstado = True
                            End Select
                        Case enumEstadoVisita.scgProceso
                            Select Case (cboEstadoVisita.SelectedIndex + 1)
                                Case enumEstadoVisita.scgFinalizada
                                    blnProcesarFinalizarOSuspender = True
                                Case enumEstadoVisita.scgSuspendida
                                    blnProcesarFinalizarOSuspender = True
                                Case enumEstadoVisita.scgEntregada
                                    MessageBox.Show(My.Resources.ResourceUI.MensajeParaMarcarVisitaEntregadaDebeFinalizarla)
                            End Select
                        Case enumEstadoVisita.scgSuspendida
                            Select Case (cboEstadoVisita.SelectedIndex + 1)
                                Case enumEstadoVisita.scgFinalizada
                                    blnProcesarFinalizarOSuspender = True
                                Case enumEstadoVisita.scgProceso
                                    blnSoloCambiarEstado = True
                                Case enumEstadoVisita.scgEntregada
                                    MessageBox.Show(My.Resources.ResourceUI.MensajeParaMarcarVisitaEntregadaDebeFinalizarla)
                            End Select
                    End Select
                    If blnProcesarFinalizarOSuspender Then
                        DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.IniciaTransaccion()
                        For Each drwOrden In m_dstOrden.SCGTA_TB_Orden.Rows
                            If VerificaExistenPendientes(drwOrden.NoOrden) Or (cboEstadoVisita.SelectedIndex + 1) = enumEstadoVisita.scgSuspendida Then
                                If Not ActualizarOrdenTrabajo(drwOrden) Then
                                    blnFinalizada = False
                                    Exit For
                                End If
                            Else
                                If drwOrden.Estado <> mc_NumEstadoOrden_Cancelada Then
                                    MessageBox.Show(My.Resources.ResourceUI.MensajeNoSePuedeFinalizarlaVisitaXqOrden & " " & drwOrden.NoOrden & " " & My.Resources.ResourceUI.MensajeTienePendientes)
                                    blnFinalizada = False
                                    Exit For
                                End If
                            End If
                        Next
                    ElseIf blnSoloCambiarEstado Then
                        m_drwVisita.CodEstado = cboEstadoVisita.SelectedIndex + 1
                        m_adpVisita = New VisitasDataAdapter
                        m_adpVisita.Update(m_dstVisita)
                        blnFinalizada = True
                    End If
                    If blnFinalizada Then
                        If m_cnConeccionVisita IsNot Nothing Then
                            If m_cnConeccionVisita.State = ConnectionState.Open Then
                                m_tnnTransaccionVisita.Commit()
                                m_cnConeccionVisita.Close()
                            End If
                        End If
                        DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.FinalizaTransaccion(SCGBusinessLogic.MetodosCompartidosSBOCls.EstadoDeTransaccion.Commit)
                        For intNumeroMensaje = 0 To m_alstMensajes.Count - 1
                            If (cboEstadoVisita.SelectedIndex + 1) = enumEstadoVisita.scgSuspendida Then
                                adpMensajeria.CreaMensajeDMS_SBO_Cotizacion(My.Resources.ResourceUI.MensajeLaOTHaSidoSuspendida, _
                                    My.Resources.ResourceUI.Suspendida, MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, CStr(m_alstMensajes.Item(intNumeroMensaje)))
                            ElseIf (cboEstadoVisita.SelectedIndex + 1) = enumEstadoVisita.scgFinalizada Then
                                adpMensajeria.CreaMensajeDMS_SBO_Cotizacion(My.Resources.ResourceUI.MensajeLaOTHaSidoFinalizada, _
                                    My.Resources.ResourceUI.Finalizada, MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, CStr(m_alstMensajes.Item(intNumeroMensaje)))
                            End If
                        Next

                    Else
                        m_tnnTransaccionVisita.Rollback()
                        m_cnConeccionVisita.Close()
                        DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.FinalizaTransaccion(SCGBusinessLogic.MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                        m_alstMensajes.Clear()
                    End If
                End If
                Me.Dispose()

            Catch ex As Exception

                If m_cnConeccionVisita IsNot Nothing Then
                    If m_cnConeccionVisita.State = ConnectionState.Open Then
                        m_tnnTransaccionVisita.Rollback()
                        m_cnConeccionVisita.Close()
                    End If
                End If
                DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.FinalizaTransaccion(SCGBusinessLogic.MetodosCompartidosSBOCls.EstadoDeTransaccion.Commit)
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally
                'Call m_cnConeccionVisita.Close()

            End Try

        End Sub

        Private Sub dtgOrdenes_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgOrdenes.CellDoubleClick
            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean

            Try

                'Validacion de que el datagrid no este vacio
                If dtgOrdenes.CurrentRow.Index <> -1 Then


                    For Each Forma_Nueva In Me.MdiParent.MdiChildren
                        If Forma_Nueva.Name = "frmOrden" Then
                            blnExisteForm = True
                        End If
                    Next

                    If Not blnExisteForm Then
                        objfrmOperYOrden = New frmOrden(m_dstOrden, CStr(dtgOrdenes.Rows.Item(dtgOrdenes.CurrentRow.Index).Cells(0).Value))

                        objfrmOperYOrden.MdiParent = Me.MdiParent
                        objfrmOperYOrden.Show()
                    End If

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub btnArchivos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnArchivos.Click
            Dim archivoDigital As FrmArchivoDigital = New FrmArchivoDigital(My.Resources.ResourceUI.TituloArchivosDigitales, "SCGTA_TB_Visita", m_drwVisita.NoVisita, g_strTablaArchivosDigitales, SCGDataAccess.DAConexion.strConectionString, 10, GlobalesUI.g_TipoSkin)
            archivoDigital.StartPosition = FormStartPosition.CenterParent
            archivoDigital.ShowDialog()

        End Sub

#End Region

        Private Sub frmDetalleVisita_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                Visualizacion_UDF()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Public Sub Visualizacion_UDF()

            Try

                VisualizarUDFVisita.Tabla = "SCGTA_TB_Visita"

                VisualizarUDFVisita.Conexion = SCGDataAccess.DAConexion.ConnectionString

                VisualizarUDFVisita.CampoLlave = "NoVisita = " & CInt(txtNoVisita.Text)

                VisualizarUDFVisita.Form = Me

                VisualizarUDFVisita.VisualizarUDF()

                VisualizarUDFVisita.Where = "NoVisita = " & CInt(txtNoVisita.Text)

                VisualizarUDFVisita.CargarComboCategorias()

                VisualizarUDFVisita.CargarDatosUDF("NoVisita = " & CInt(txtNoVisita.Text))

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex

            End Try

        End Sub

    End Class

End Namespace