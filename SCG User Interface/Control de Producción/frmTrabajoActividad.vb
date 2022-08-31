Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmTrabajoActividad

#Region "Constructor"

        Public Sub New(ByVal p_intCodigoColaborador As Integer, _
                       ByVal p_strNombreColaborador As String, _
                       ByVal p_strIdActividad As Integer, _
                       ByVal p_strNombreActividad As String, _
                       ByVal p_intIdAsignacion As Integer, _
                       ByVal p_strNoOrden As String, _
                       ByVal p_intEstadoOrden As Integer, _
                       ByVal p_intNoFase As Integer, _
                       ByVal p_dstCol As ColaboradorDataset, _
                       ByVal p_blnYaIniciada As Boolean, _
                       ByVal p_blnActividadSuspendida As Boolean)

            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
            m_intCodigoColaborador = p_intCodigoColaborador
            m_strNombreColaborador = p_strNombreColaborador
            m_strIdActividad = p_strIdActividad
            m_strNombreActividad = p_strNombreActividad
            m_intIdAsignacion = p_intIdAsignacion
            m_strNoOrden = p_strNoOrden
            m_intEstadoorden = p_intEstadoOrden
            m_intNoFase = p_intNoFase
            m_dstCol = p_dstCol
            m_blnYaIniciada = p_blnYaIniciada
            m_blnActividadSuspendida = p_blnActividadSuspendida

            m_blnOrdenIniciada = False
            m_blnValidarHoraYFecha = False

        End Sub

#End Region

#Region "Declaraciones"

#Region "Variables"

        Private m_intCodigoColaborador As Integer
        Private m_strNombreColaborador As String
        Private m_strIdActividad As Integer
        Private m_strNombreActividad As String
        Private m_intIdAsignacion As Integer
        Private m_strNoOrden As String
        Private m_intEstadoorden As Integer
        Private m_intNoFase As Integer

        Private m_blnYaIniciada As Boolean
        Private m_blnActividadSuspendida As Boolean

        Private m_blnOrdenIniciada As Boolean

        Private m_dblValorUnidadTiempo As Double

        Private m_strDescripcionUnidadTiempo As String

        Private m_blnValidarHoraYFecha As Boolean

        Private m_dblTiempoAsignado As Double

#End Region

#Region "Objetos Generales"

        Private m_objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

#End Region

#Region "Constantes"

        Private Const mc_strProcesoManual As String = "Manual"
        Private Const mc_intEstadoProceso As String = "2"

        'Por Fase
        Private Const mc_Estado_Finalizado As String = "Finalizado"
        Private Const mc_Estado_Suspendido As String = "Suspendido"
        Private Const mc_Estado_NoIniciado As String = "No iniciado"
        Private Const mc_Estado_Iniciado As String = "Iniciado"

        'Principal
        Private mc_PriEstado_NoIniciada As String = My.Resources.ResourceUI.NoIniciada
        Private mc_PriEstado_Proceso As String = My.Resources.ResourceUI.Enproceso
        Private mc_PriEstado_Suspendida As String = My.Resources.ResourceUI.Suspendida
        Private mc_PriEstado_Finalizada As String = My.Resources.ResourceUI.Finalizada
        Private mc_PriEstado_Cerrada As String = My.Resources.ResourceUI.Cerrada
        Private mc_PriEstado_Cancelada As String = My.Resources.ResourceUI.Cancelada

#End Region

#Region "Acceso a datos"

        Private m_dstCol As ColaboradorDataset

        Private m_adpCol As SCGDataAccess.ColaboradorDataAdapter

#End Region

#Region "Eventos"

        Public Event e_AsignacionRealizada(ByVal p_blnOrdenIniciada As Boolean)

#End Region

#End Region

#Region "Eventos"

        Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
            Try
                Me.Close()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub btnAsignar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAsignar.Click

            Dim drwCol As ColaboradorDataset.SCGTA_TB_ControlColaboradorRow

            Try
                m_dstCol.RejectChanges()
                drwCol = m_dstCol.SCGTA_TB_ControlColaborador.FindByID(m_intIdAsignacion)
                If drwCol IsNot Nothing Then
                    If (chkHoraInicio.Checked) Or (txtTiempo.Text) <> "" Then
                        drwCol.FechaInicio = New Date(dtpFechaInicio.Value.Year, dtpFechaInicio.Value.Month, dtpFechaInicio.Value.Day, dtpHoraInicio.Value.Hour, dtpHoraInicio.Value.Minute, 0)
                        If m_blnActividadSuspendida Then
                            If (chkHoraFin.Checked) Or (txtTiempo.Text) <> "" Then
                                drwCol.FechaFin = New Date(dtpFechaFin.Value.Year, dtpFechaFin.Value.Month, dtpFechaFin.Value.Day, dtpHoraFin.Value.Hour, dtpHoraFin.Value.Minute, 0)
                            End If
                        End If
                        If (chkHoraInicio.Enabled) Or (txtTiempo.Text) <> "" Then
                            Call IniciarProceso()
                        End If

                        If (chkHoraFin.Checked And Not m_blnActividadSuspendida) Or (Trim(txtTiempo.Text) <> "" And Not m_blnActividadSuspendida) Then

                            drwCol.FechaFin = New Date(dtpFechaFin.Value.Year, dtpFechaFin.Value.Month, dtpFechaFin.Value.Day, dtpHoraFin.Value.Hour, dtpHoraFin.Value.Minute, 0)

                            If rbtRangoHoras.Checked = True Then
                                Call FinalizarProceso()
                            Else
                                drwCol.TiempoHoras = m_dblTiempoAsignado
                                Call FinalizarProceso(True)
                            End If


                        End If

                    End If
                End If

                RaiseEvent e_AsignacionRealizada(m_blnOrdenIniciada)

                Me.Close()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub frmTrabajoActividad_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim drwCol As ColaboradorDataset.SCGTA_TB_ControlColaboradorRow

            Try

                lblActividad.Text = m_strNombreActividad
                lblColaborador.Text = m_strNombreColaborador

                Dim fechaHoraServidor As Date = m_objUtilitarios.CargarFechaHoraServidor()

                dtpHoraInicio.Value = fechaHoraServidor
                dtpHoraFin.Value = fechaHoraServidor

                dtpFechaInicio.Value = fechaHoraServidor
                dtpFechaFin.Value = fechaHoraServidor

                m_blnValidarHoraYFecha = True

                If m_blnYaIniciada Then

                    drwCol = m_dstCol.SCGTA_TB_ControlColaborador.FindByID(m_intIdAsignacion)
                    chkHoraInicio.Checked = True
                    chkHoraInicio.Enabled = False
                    dtpFechaInicio.Value = drwCol.FechaInicio
                    dtpHoraInicio.Value = drwCol.FechaInicio
                    dtpHoraInicio.Enabled = False
                    dtpFechaInicio.Enabled = False

                End If

                CargarUnidadesTiempoGlobales()

                If g_intUnidadTiempo <> -1 Then
                    rbtTiempo.Text = m_strDescripcionUnidadTiempo
                End If

                chkHoraFin.Enabled = False
                chkHoraInicio.Enabled = False

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try


        End Sub

        Private Sub chkHoraFin_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkHoraFin.CheckedChanged
            Try

                dtpHoraFin.Enabled = chkHoraFin.Checked
                dtpFechaFin.Enabled = chkHoraFin.Checked

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub chkHoraInicio_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkHoraInicio.CheckedChanged

            Try

                dtpHoraInicio.Enabled = chkHoraInicio.Checked
                dtpFechaInicio.Enabled = chkHoraInicio.Checked

                If Not chkHoraInicio.Checked Then

                    chkHoraFin.Checked = False

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try


        End Sub

        Private Sub dtpFechaInicio_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpFechaInicio.ValueChanged

            Try
                If m_blnValidarHoraYFecha Then
                    dtpFechaFin.MinDate = New Date(dtpFechaInicio.Value.Year, dtpFechaInicio.Value.Month, dtpFechaInicio.Value.Day, 0, 0, 0)
                    dtpHoraFin.MinDate = dtpFechaFin.MinDate
                    dtpHoraInicio.MinDate = New Date(dtpFechaInicio.Value.Year, dtpFechaInicio.Value.Month, dtpFechaInicio.Value.Day, 0, 0, 0)
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try


        End Sub

        Private Sub dtpHoraInicio_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If m_blnValidarHoraYFecha Then
                    dtpFechaFin.MinDate = New Date(dtpFechaInicio.Value.Year, dtpFechaInicio.Value.Month, dtpFechaInicio.Value.Day, dtpFechaInicio.Value.Hour, dtpFechaInicio.Value.Minute, 0)
                    dtpHoraFin.MinDate = New Date(dtpFechaInicio.Value.Year, dtpFechaInicio.Value.Month, dtpFechaInicio.Value.Day, dtpHoraInicio.Value.Hour, dtpHoraInicio.Value.Minute + 1, 0)
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try


        End Sub

#End Region

#Region "Métodos"

        Private Sub IniciarProceso()
            Dim objDA As New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim objBLSBO As DMSOneFramework.BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal
            Dim strMonedaLocal As String
            Dim strMonedaSistema As String

            objBLSBO = New DMSOneFramework.BLSBO.GlobalFunctionsSBO
            strMonedaSistema = objBLSBO.RetornarMonedaSistema
            strMonedaLocal = objBLSBO.RetornarMonedaLocal
            If strMonedaSistema <> strMonedaLocal Then
                decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(strMonedaSistema, Today, strConectionString, True)
            Else
                decTipoCambio = 1
            End If
            If decTipoCambio <> -1 Then

                If DMSOneFramework.SCGDataAccess.Utilitarios.GetPostingPeriod(Today).Trim <> "" Then

                    Dim adpFaseXOrdenEstados As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
                    Dim adpTiempos As New DMSOneFramework.SCGDataAccess.TiemposMuertosDataAdapter
                    '''''''


                    objDA.UpdateIniciar(m_dstCol.SCGTA_TB_ControlColaborador, mc_strProcesoManual)

                    ProcesoIniciarFaseXOrden()

                Else

                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOPeriodoFiscalInvalido)

                End If
            Else

                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOActualizarTipoCambio)

            End If

        End Sub

        Private Sub ProcesoIniciarFaseXOrden()

            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
            Dim intUpdateResult As Integer

            If Not OrdenIniciada() Then
                IniciarOrden()
            End If

            intUpdateResult = objDA.IniciarFase(m_strNoOrden, m_intNoFase)

        End Sub

        Private Sub IniciarOrden()

            Dim drdOrden As OrdenTrabajoDataset.SCGTA_TB_OrdenRow
            Dim objAdapter As New SCGDataAccess.OrdenTrabajoDataAdapter
            Dim dtsOrden As New OrdenTrabajoDataset
            objAdapter.Fill(dtsOrden, m_strNoOrden)

            With dtsOrden
                drdOrden = .SCGTA_TB_Orden.FindByNoOrden(m_strNoOrden)
                If Not IsNothing(drdOrden) Then
                    With drdOrden
                        .Estado = Utilitarios.GEnum_EstadoOrden.dmsProceso
                    End With

                    objAdapter = New SCGDataAccess.OrdenTrabajoDataAdapter
                    objAdapter.Actualizar(dtsOrden)
                End If
            End With

            DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.ActualizarEstadoCotizacion(drdOrden.NoCotizacion, mc_PriEstado_Proceso)

        End Sub

        Private Function OrdenIniciada() As Boolean
            Dim blnResult As Boolean

            If m_intEstadoorden = mc_intEstadoProceso Then
                blnResult = True
            Else
                blnResult = False
            End If

            Return blnResult
        End Function

        Overloads Sub FinalizarProceso()

            Dim objDA As New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim objBLSBO As BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal
            Dim strMonedaLocal As String
            Dim strMonedaSistema As String

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
                    objDA.UpdateFinalizar(m_dstCol.SCGTA_TB_ControlColaborador, mc_strProcesoManual)
                Else
                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOPeriodoFiscalInvalido)
                End If
            Else
                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOActualizarTipoCambio)
            End If

        End Sub

        Overloads Sub FinalizarProceso(ByVal bolTiempoDigitado As Boolean)

            Dim objDA As New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim objBLSBO As BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal
            Dim strMonedaLocal As String
            Dim strMonedaSistema As String

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


                    objDA.UpdateFinalizar(m_dstCol.SCGTA_TB_ControlColaborador, mc_strProcesoManual, bolTiempoDigitado)
                Else

                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOPeriodoFiscalInvalido)

                End If
            Else
                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOActualizarTipoCambio)

            End If

        End Sub


#End Region

        Sub CargarUnidadesTiempoGlobales()
            If g_intUnidadTiempo <> -1 Then
                Dim adpUnidadTiempoDataAdapter As New DMSONEDKFramework.UnidadTiempoDataAdapter
                Dim dstUnidadTiempoDataSet As New DMSONEDKFramework.UnidadTiempoDataSet
                Dim drwFila() As DataRow
                adpUnidadTiempoDataAdapter.Fill(dstUnidadTiempoDataSet)
                drwFila = dstUnidadTiempoDataSet.SCGTA_TB_UnidadTiempo.Select("CodigoUnidadTiempo = " & g_intUnidadTiempo)
                m_dblValorUnidadTiempo = drwFila(0)("TiempoMinutosUnidadTiempo")
                m_strDescripcionUnidadTiempo = drwFila(0)("DescripcionUnidadTiempo")
            End If
        End Sub


        Sub AsignarTiempos(ByVal datMinutosRestar As TimeSpan)
            Dim datTiempo As Date
            datTiempo = Now
            datTiempo = datTiempo.Subtract(datMinutosRestar)
            dtpFechaInicio.Value = datTiempo
            dtpHoraInicio.Value = datTiempo
            dtpFechaFin.Value = Now.Date
            dtpHoraFin.Value = Now
        End Sub


        Private Sub rbtTiempoUnidadTiempo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtTiempo.CheckedChanged
            If rbtTiempo.Checked = True Then
                txtTiempo.ReadOnly = False
                rbtRangoHoras.Checked = False
            Else
                txtTiempo.ReadOnly = True
                txtTiempo.Text = String.Empty
            End If
        End Sub

        

        Private Sub rbtRangoHoras_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtRangoHoras.CheckedChanged
            If rbtRangoHoras.Checked = True Then
                chkHoraFin.Enabled = True
                chkHoraInicio.Enabled = True
                rbtTiempo.Checked = False
            Else
                chkHoraFin.Enabled = False
                chkHoraInicio.Enabled = False
            End If
        End Sub

        Private Sub txtTiempo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTiempo.TextChanged

        End Sub

        Private Sub txtTiempo_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTiempo.Validated
            Dim dblMinutos As Double
            Dim datMinutosRestar As TimeSpan

            If Trim(txtTiempo.Text) = "" Then
                txtTiempo.Text = 0
            End If

            If g_intUnidadTiempo <> -1 Then
                dblMinutos = CDbl(txtTiempo.Text) * CDbl(m_dblValorUnidadTiempo)
            Else
                dblMinutos = CDbl(txtTiempo.Text)
            End If

            m_dblTiempoAsignado = dblMinutos / 60

            datMinutosRestar = New TimeSpan(0, dblMinutos, 0)

            AsignarTiempos(datMinutosRestar)

        End Sub

        
    End Class

End Namespace