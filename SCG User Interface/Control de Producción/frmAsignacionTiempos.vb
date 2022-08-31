Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon



Namespace SCG_User_Interface

    Public Class frmAsignacionTiempos



        Public Event e_AsignacionRealizada(ByVal p_blnOrdenIniciada As Boolean)




        Private m_adpAct As SCGDataAccess.ColaboradorDataAdapter
        Private m_strNoOrden As String
        Private m_intNoCotizacion As String
        Private m_intEstadoOrden As Integer
        Private m_dblValorUnidadTiempo As Double
        Private m_strDescripcionUnidadTiempo As String
        Private Const mc_strProcesoManual As String = "Manual"
        Private Const mc_intEstadoProceso As String = "2"
        Private Const mc_PriEstado_Proceso As String = "Proceso"
        Private m_intNoFase As Integer
        Private m_intIDActividad As Integer




        Public Sub New(ByVal p_strNoOrden As String, ByVal p_intNoCotizacion As Integer, ByVal p_intEstadoOrden As Integer)

            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
            m_strNoOrden = p_strNoOrden
            m_intNoCotizacion = p_intNoCotizacion
            m_intEstadoOrden = p_intEstadoOrden

            'm_blnOrdenIniciada = False

        End Sub


        Private Sub CargarGridActividades(ByVal intNumeroFase As Integer)

            m_adpAct = New SCGDataAccess.ColaboradorDataAdapter
            'dstActividades = Nothing

            Call m_adpAct.Fill(dstActividades, m_strNoOrden, intNumeroFase, 1, True)
            'EstiloDataGridViewActividades()

        End Sub

        Private Sub CargarFases()

            cboFases.DataSource = Nothing
            Dim drd As SqlClient.SqlDataReader
            Dim objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)
            Dim alsFases As New ArrayList

            drd = objUtilitarios.ReaderFasesProd(m_strNoOrden)


            clsUtilidadCombos.CargarComboSourceByReader(cboFases, drd, "Descripcion", "NoFase")
            drd.Close()

        End Sub


        Private Sub frmAsignacionTiempos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            CargarUnidadesTiempoGlobales()

            If g_intUnidadTiempo <> -1 Then
                dtgActividades.Columns("TotalUnidadTiempoDataGridViewTextBoxColumn").HeaderText = m_strDescripcionUnidadTiempo
            End If
            CargarFases()
            cboFases.SelectedIndex = -1
        End Sub



        Private Sub cboFases_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFases.SelectedIndexChanged

            dstActividades.SCGTA_TB_ControlColaborador.Clear()
            If Trim(cboFases.ValueMember) <> String.Empty Then
                m_intNoFase = cboFases.SelectedValue
                CargarGridActividades(m_intNoFase)
            End If

        End Sub

        Sub CalcularTiempoHoras()
            Dim intIndice As Integer
            Dim dblTiempoDigitado As Decimal
            For intIndice = 0 To dstActividades.SCGTA_TB_ControlColaborador.Rows.Count - 1
                If Not dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice)("TotalUnidadTiempo") Is System.DBNull.Value Then
                    If Not String.IsNullOrEmpty(dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice)("TotalUnidadTiempo")) Then
                        dblTiempoDigitado = dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice)("TotalUnidadTiempo")
                    Else
                        dblTiempoDigitado = 0
                    End If
                    If g_intUnidadTiempo = -1 Then
                        dblTiempoDigitado = dblTiempoDigitado / 60
                    Else
                        dblTiempoDigitado = dblTiempoDigitado * m_dblValorUnidadTiempo / 60
                    End If
                End If

                dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice)("TiempoHoras") = dblTiempoDigitado

            Next
        End Sub

        Private Sub dtgActividades_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgActividades.CellEndEdit

            If Not IsNumeric(dstActividades.SCGTA_TB_ControlColaborador.Rows(dtgActividades.CurrentRow.Index)("TotalUnidadTiempo")) Then
                dstActividades.SCGTA_TB_ControlColaborador.Rows(dtgActividades.CurrentRow.Index)("TotalUnidadTiempo") = System.DBNull.Value
            End If

            '    '            Dim dblTiempoDigitado As Double
            '    'Try
            '    '    If Not dstActividades.SCGTA_TB_ControlColaborador.Rows(dtgActividades.CurrentRow.Index)("TotalUnidadTiempo") Is System.DBNull.Value Then
            '    '        If Not String.IsNullOrEmpty(dstActividades.SCGTA_TB_ControlColaborador.Rows(dtgActividades.CurrentRow.Index)("TotalUnidadTiempo")) Then
            '    '            dblTiempoDigitado = dstActividades.SCGTA_TB_ControlColaborador.Rows(dtgActividades.CurrentRow.Index)("TotalUnidadTiempo")
            '    '        Else
            '    '            dblTiempoDigitado = 0
            '    '        End If
            '    '    Else
            '    '        dblTiempoDigitado = 0
            '    '    End If

            '    '    If g_intUnidadTiempo = -1 Then
            '    '        dblTiempoDigitado = dblTiempoDigitado / 60
            '    '    Else
            '    '        dblTiempoDigitado = dblTiempoDigitado * m_dblValorUnidadTiempo / 60
            '    '    End If

            '    '    dstActividades.SCGTA_TB_ControlColaborador.Rows(dtgActividades.CurrentRow.Index)("TiempoHoras") = dblTiempoDigitado

            '    'Catch ex As Exception
            '    '    MsgBox(ex.Message)
            '    'End Try
        End Sub

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

        Overloads Sub FinalizarProceso(ByVal bolTiempoDigitado As Boolean)

            Dim objDA As New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim objBLSBO As BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal
            Dim strMonedaLocal As String
            Dim strMonedaSistema As String

            objBLSBO = New BLSBO.GlobalFunctionsSBO
            strMonedaLocal = objBLSBO.RetornarMonedaLocal
            strMonedaSistema = objBLSBO.RetornarMonedaSistema
            If strMonedaLocal <> strMonedaSistema Then
                decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(strMonedaSistema, Today, strConectionString, True)
            Else
                decTipoCambio = 1
            End If
            If decTipoCambio <> -1 Then
                If SCGDataAccess.Utilitarios.GetPostingPeriod(Today).Trim <> "" Then
                    objDA.UpdateFinalizar(dstActividades.SCGTA_TB_ControlColaborador, mc_strProcesoManual, bolTiempoDigitado)
                Else
                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOPeriodoFiscalInvalido)
                End If
            Else
                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOActualizarTipoCambio)
            End If

        End Sub

        Private Sub IniciarProceso()
            Dim objDA As New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim objBLSBO As DMSOneFramework.BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal
            Dim strMonedaLocal As String
            Dim strMonedaSistema As String


            objBLSBO = New DMSOneFramework.BLSBO.GlobalFunctionsSBO
            strMonedaLocal = objBLSBO.RetornarMonedaLocal
            strMonedaSistema = objBLSBO.RetornarMonedaSistema

            If strMonedaSistema <> strMonedaLocal Then
                decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(strMonedaSistema, Today, strConectionString, True)
            Else
                decTipoCambio = 1
            End If
            If decTipoCambio <> -1 Then
                If DMSOneFramework.SCGDataAccess.Utilitarios.GetPostingPeriod(Today).Trim <> "" Then
                    Dim adpFaseXOrdenEstados As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
                    Dim adpTiempos As New DMSOneFramework.SCGDataAccess.TiemposMuertosDataAdapter
                    ''''''
                    objDA.UpdateIniciar(dstActividades.SCGTA_TB_ControlColaborador, mc_strProcesoManual)
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

            If m_intEstadoOrden = mc_intEstadoProceso Then
                blnResult = True
            Else
                blnResult = False
            End If

            Return blnResult
        End Function




        Sub CargarFechasFin()
            Dim intIndice As Integer
            For intIndice = 0 To dstActividades.SCGTA_TB_ControlColaborador.Rows.Count - 1
                If Not dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice)("TotalUnidadTiempo") Is System.DBNull.Value Then
                    dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice)("FechaFin") = Now
                Else
                    dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice).RejectChanges()
                End If

            Next
        End Sub

        Sub BorrarFechasFin()
            Dim intIndice As Integer
            For intIndice = 0 To dstActividades.SCGTA_TB_ControlColaborador.Rows.Count - 1
                If Not dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice)("TotalUnidadTiempo") Is System.DBNull.Value Then
                    dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice)("FechaFin") = System.DBNull.Value
                Else
                    dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice).RejectChanges()
                End If

            Next
        End Sub

        Sub CalcularFechaInicio()

            Dim intIndice As Integer
            Dim datFechaInicio As Date
            Dim datFechaFin As Date
            Dim intHorasARestar As TimeSpan

            For intIndice = 0 To dstActividades.SCGTA_TB_ControlColaborador.Rows.Count - 1
                If Not dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice)("FechaFin") Is System.DBNull.Value Then
                    datFechaFin = dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice)("FechaFin")

                    If Not dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice)("TiempoHoras") Is System.DBNull.Value Then
                        intHorasARestar = New TimeSpan(0, dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice)("TiempoHoras") * 60, 0)
                    Else
                        intHorasARestar = New TimeSpan(0, 0, 0)
                    End If

                    datFechaInicio = datFechaFin.Subtract(intHorasARestar)

                    If Not dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice)("TotalUnidadTiempo") Is System.DBNull.Value Then
                        dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice)("FechaInicio") = datFechaInicio
                    End If
                Else
                    dstActividades.SCGTA_TB_ControlColaborador.Rows(intIndice).RejectChanges()
                End If
            Next

        End Sub


        Private Sub dtgActividades_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgActividades.CellContentClick

        End Sub

        Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
            Try
                'DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.IniciaTransaccion()
                CalcularTiempoHoras()
                CargarFechasFin()
                CalcularFechaInicio()
                BorrarFechasFin()
                IniciarProceso()
                CargarFechasFin()
                FinalizarProceso(True)
                'DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.FinalizaTransaccion(SCGBusinessLogic.MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                RaiseEvent e_AsignacionRealizada(False)

                Me.Close()
            Catch ex As Exception
                'DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.FinalizaTransaccion(SCGBusinessLogic.MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try


        End Sub

        Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
            dstActividades.RejectChanges()
            Me.Close()
        End Sub
    End Class
End Namespace