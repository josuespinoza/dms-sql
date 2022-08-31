Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmAsignarActividades

#Region "Declaraciones"

#Region "Variables"

        Private m_strNoOrden As String
        Private m_intNoCotizacion As Integer
        Private m_intEstadoOrden As Integer

        Private m_blnOrdenIniciada As Boolean

#End Region

#Region "Datasets"

        Public m_dstAct As ActividadesXFaseDataset

#End Region

#Region "Adapters"

        Private m_adpAct As SCGDataAccess.ActividadesXFaseDataAdapter

#End Region

#Region "Eventos"

        Public Event e_AsignacionRealizada(ByVal p_blnOrdenIniciada As Boolean)

#End Region

#Region "Objetos Generales"

        Private objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

#End Region

#Region "Constantes"

        'Por Fase
        Private Const mc_Estado_Finalizado As String = "Finalizado"
        Private Const mc_Estado_Suspendido As String = "Suspendido"
        Private Const mc_Estado_NoIniciado As String = "No iniciado"
        Private Const mc_Estado_Iniciado As String = "Iniciado"

        'Por Orden
        Private Const mc_intEstadoProceso As String = "2"

        'Principal
        Private mc_PriEstado_NoIniciada As String = My.Resources.ResourceUI.NoIniciada
        Private mc_PriEstado_Proceso As String = My.Resources.ResourceUI.Enproceso
        Private mc_PriEstado_Suspendida As String = My.Resources.ResourceUI.Suspendida
        Private mc_PriEstado_Finalizada As String = My.Resources.ResourceUI.Finalizada
        Private mc_PriEstado_Cerrada As String = My.Resources.ResourceUI.Cerrada
        Private mc_PriEstado_Cancelada As String = My.Resources.ResourceUI.Cancelada

        Private Const mc_AsignacionUnicaMO As String = "AsignacionUnicaMO"

#End Region

#End Region

#Region "Constructor"

        Public Sub New(ByVal p_strNoOrden As String, ByVal p_intNoCotizacion As Integer, ByVal p_intEstadoOrden As Integer)

            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
            m_strNoOrden = p_strNoOrden
            m_intNoCotizacion = p_intNoCotizacion
            m_intEstadoOrden = p_intEstadoOrden

            m_blnOrdenIniciada = False

        End Sub

#End Region

#Region "Eventos"

        Private Sub frmAsignarActividades_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                Call CargarGridActividades()

                Call CargarComboEmpleados()

                Call InicializarHoras()

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try


        End Sub

        Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

            Try

                Me.Close()

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
                chkHoraFin.Enabled = chkHoraInicio.Checked

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

                dtpFechaFin.MinDate = New Date(dtpFechaInicio.Value.Year, dtpFechaInicio.Value.Month, dtpFechaInicio.Value.Day, 0, 0, 0)
                dtpHoraFin.MinDate = dtpFechaFin.MinDate
                dtpHoraInicio.MinDate = New Date(dtpFechaInicio.Value.Year, dtpFechaInicio.Value.Month, dtpFechaInicio.Value.Day, 0, 0, 0)

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try


        End Sub

        Private Sub dtpHoraInicio_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpHoraInicio.ValueChanged

            Try

                dtpFechaFin.MinDate = New Date(dtpFechaInicio.Value.Year, dtpFechaInicio.Value.Month, dtpFechaInicio.Value.Day, dtpFechaInicio.Value.Hour, dtpFechaInicio.Value.Minute, 0)
                dtpHoraFin.MinDate = New Date(dtpFechaInicio.Value.Year, dtpFechaInicio.Value.Month, dtpFechaInicio.Value.Day, dtpHoraInicio.Value.Hour, dtpHoraInicio.Value.Minute, 0)

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try


        End Sub

#End Region

#Region "Métodos"

        Private Sub CargarGridActividades()

            m_adpAct = New SCGDataAccess.ActividadesXFaseDataAdapter

            m_dstAct = Nothing

            m_dstAct = New ActividadesXFaseDataset


            Call m_adpAct.FillbyFilters(m_dstAct, m_strNoOrden, 0, 1)

            dtgActividades.DataSource = m_dstAct.SCGTA_TB_ActividadesxOrden

        End Sub

        Private Sub CargarComboEmpleados()

            objUtilitarios.CargarCombos(cbocolaborador, 16, 0, G_strIDSucursal)

        End Sub

        Private Sub InicializarHoras()

            dtpHoraFin.Value = objUtilitarios.CargarFechaHoraServidor()
            dtpHoraInicio.Value = dtpHoraFin.Value

        End Sub

#End Region

        Private Sub btnAsignar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsignar.Click
            Try

                Dim objSBOCommons As New BLSBO.GlobalFunctionsSBO()
                Dim drwActividad As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
                Dim intCodigoColaborador As Integer
                Dim strNombreColaborador As String
                Dim blnYaIniciada As Boolean

                If cbocolaborador.Text.Trim <> "" Then

                    blnYaIniciada = False
                    intCodigoColaborador = Busca_Codigo_Texto(cbocolaborador.Text)
                    strNombreColaborador = Busca_Codigo_Texto(cbocolaborador.Text, False)
                    For Each drwActividad In m_dstAct.SCGTA_TB_ActividadesxOrden
                        If drwActividad.Check Then

                            'Valida si solo se puede hacer una asignación de mecanico unica por actividad
                            If ValidarAsignacionUnicaMO(drwActividad.ID) = True Then
                                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.ValidarAsignacionUnicaMO & "  " & drwActividad.ItemName)

                            Else

                                AsinacionNueva(drwActividad.NoFase, drwActividad.ID, intCodigoColaborador, strNombreColaborador, m_intNoCotizacion)
                                If drwActividad IsNot Nothing Then
                                    'LABEL' objSBOCommons.AgregarEmpleadoRealiza(m_intNoCotizacion, intCodigoColaborador, drwActividad.LineNum, strNombreColaborador)
                                    'Utilitarios.AsignarEmpleado(m_intNoCotizacion, intCodigoColaborador, drwActividad.LineNum, strNombreColaborador)

                                    ActualizaReAsignacion(drwActividad.ID)
                                End If



                                If Not blnYaIniciada And chkHoraInicio.Checked Then
                                    Call ProcesoIniciarFaseXOrden(drwActividad.NoFase)
                                    blnYaIniciada = True

                                End If

                            End If


                        Else
                            drwActividad.RejectChanges()
                        End If
                    Next
                    RaiseEvent e_AsignacionRealizada(m_blnOrdenIniciada)

                Else
                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarcolaborador)
                End If

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub ActualizaReAsignacion(ByVal p_intIDActividad As Integer)
            Dim objDA As New SCGDataAccess.ColaboradorDataAdapter

            Try
                objDA.UpdateReAsignarColaborador(p_intIDActividad)

            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        Private Sub AsinacionNueva(ByVal p_intNoFase As Integer, ByVal p_intIDActividad As Integer, _
                                   ByVal p_intCodigoColaborador As Integer, ByVal p_strNombreColaborador As String, ByVal p_intDocEntry As Integer)
            Dim objDA As New SCGDataAccess.ColaboradorDataAdapter
            Dim dtsAsignados As New ColaboradorDataset
            Dim dtrAsignando As ColaboradorDataset.SCGTA_TB_ControlColaboradorRow
            Dim m_objCotizacion As SAPbobsCOM.Documents
            Dim oLineasCotizacion As SAPbobsCOM.Document_Lines
            Dim m_strValorId As String
            Dim m_strValorIdEmp As String


            Try
                dtrAsignando = dtsAsignados.SCGTA_TB_ControlColaborador.NewSCGTA_TB_ControlColaboradorRow

                With dtrAsignando
                    .NoFase = p_intNoFase
                    .NoOrden = m_strNoOrden
                    If chkReproceso.Checked Then
                        .Reproceso = 1
                    Else
                        .Reproceso = 0
                    End If
                    .EmpID = p_intCodigoColaborador
                    .EmpNombre = p_strNombreColaborador
                    If chkHoraInicio.Checked Then
                        .FechaInicio = New Date(dtpFechaInicio.Value.Year, dtpFechaInicio.Value.Month, dtpFechaInicio.Value.Day, dtpHoraInicio.Value.Hour, dtpHoraInicio.Value.Minute, 0)
                        If chkHoraFin.Checked Then
                            .FechaFin = New Date(dtpFechaFin.Value.Year, dtpFechaFin.Value.Month, dtpFechaFin.Value.Day, dtpHoraFin.Value.Hour, dtpHoraFin.Value.Minute, 0)

                        End If

                    End If

                    .TiempoHoras = 0
                    .Estado = mc_Estado_NoIniciado
                    .Costo = 0
                    .IDActividad = p_intIDActividad
                End With

                dtsAsignados.SCGTA_TB_ControlColaborador.AddSCGTA_TB_ControlColaboradorRow(dtrAsignando)

                objDA.InsertarNuevo(dtsAsignados)

                ''Inserta Mecanico en Cotizacion 09/05/2014
                m_objCotizacion = G_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                m_objCotizacion.GetByKey(p_intDocEntry)
                oLineasCotizacion = m_objCotizacion.Lines

                For i As Integer = 0 To oLineasCotizacion.Count - 1

                    oLineasCotizacion.SetCurrentLine(i)
                    m_strValorId = oLineasCotizacion.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString.Trim()
                    m_strValorIdEmp = oLineasCotizacion.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim()
                    If (p_intIDActividad = m_strValorId) Then

                        If (String.IsNullOrEmpty(m_strValorIdEmp)) Then
                            oLineasCotizacion.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = Busca_Codigo_Texto(cbocolaborador.Text)
                            oLineasCotizacion.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = Busca_Codigo_Texto(cbocolaborador.Text, False)


                        Else

                            oLineasCotizacion.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = ""
                            oLineasCotizacion.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = "Varios"


                        End If
                    End If


                Next

                m_objCotizacion.Update()

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try


        End Sub

        Private Sub ProcesoIniciarFaseXOrden(ByVal p_intNofase As Integer)

            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
            Dim intUpdateResult As Integer

            If Not OrdenIniciada() Then
                IniciarOrden()
            End If

            intUpdateResult = objDA.IniciarFase(m_strNoOrden, p_intNofase)

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
                    m_blnOrdenIniciada = True
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

        Private Sub chkSeleccionarTodas_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSeleccionarTodas.CheckStateChanged

            Dim drwActividad As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
            For Each drwActividad In m_dstAct.SCGTA_TB_ActividadesxOrden
                drwActividad.Check = chkSeleccionarTodas.Checked
            Next

        End Sub



        Private Function ValidarAsignacionUnicaMO(ByVal p_intIDActividad As Integer) As Boolean
            Try
                '--------agregado para transferencias de Stock a borrador
                Dim adpConf As New ConfiguracionDataAdapter
                Dim dstConf As New ConfiguracionDataSet
                '    Dim drwConf As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow
                Dim blnValida As Boolean = False

                Dim strEstadoActividad As String = String.Empty
                Dim objDAColaborador As DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
                Dim dstColaborador As New ColaboradorDataset
                Dim drwControlColaborador As ColaboradorDataset.SCGTA_TB_ControlColaboradorRow

                Dim valorRetorno As Boolean = False

                adpConf.Fill(dstConf)

                If ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracionDraft(dstConf.SCGTA_TB_Configuracion, mc_AsignacionUnicaMO, "") Then
                    blnValida = True
                Else
                    blnValida = False
                End If
                '---------------------------------------------------------

                If blnValida = True Then

                    objDAColaborador = New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter

                    objDAColaborador.SelControlColaboradorxActividad(dstColaborador, m_strNoOrden, p_intIDActividad)


                    For Each drwControlColaborador In dstColaborador.SCGTA_TB_ControlColaborador
                        If drwControlColaborador.Estado <> "Suspendido" Then
                            valorRetorno = True
                            Return valorRetorno
                        End If
                    Next

                    Return False
                End If



            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            End Try
        End Function

    End Class

End Namespace