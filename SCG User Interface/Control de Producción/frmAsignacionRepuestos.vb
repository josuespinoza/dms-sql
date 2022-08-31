Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmAsignacionRepuestos

#Region "Declaraciones"

#Region "Variables"

        Private m_strNoOrden As String
        Private m_intNoCotizacion As Integer
        Private m_intEstadoOrden As Integer

        Private m_blnOrdenIniciada As Boolean

#End Region

#Region "Datasets"

        Public m_dstRep As RepuestosxOrdenDataset

#End Region

#Region "Adapters"

        Private m_adpRep As SCGDataAccess.RepuestosxOrdenDataAdapter

#End Region

#Region "Eventos"

        Public Event e_AsignacionRealizada()

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

        Private Sub frmAsignacionRepuestos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                Call CargarGridRepuestos()

                Call CargarComboEmpleados()

                Call OrdenarColumnasGrid()

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

        Private Sub chkSeleccionarTodas_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSeleccionarTodas.CheckStateChanged

            Dim drwRepuestos As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow

            For Each drwRepuestos In m_dstRep.SCGTA_TB_RepuestosxOrden
                drwRepuestos.Check = chkSeleccionarTodas.Checked
            Next

        End Sub

        Private Sub btnAsignar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsignar.Click
            Try

                Dim objSBOCommons As New BLSBO.GlobalFunctionsSBO()
                Dim drwRepuestos As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
                Dim intCodigoColaborador As Integer
                Dim strNombreColaborador As String

                If cbocolaborador.Text.Trim <> "" Then

                    intCodigoColaborador = Busca_Codigo_Texto(cbocolaborador.Text)
                    strNombreColaborador = Busca_Codigo_Texto(cbocolaborador.Text, False)

                    For Each drwRepuestos In m_dstRep.SCGTA_TB_RepuestosxOrden
                        If drwRepuestos.Check Then
                            If drwRepuestos IsNot Nothing Then
                                '"LABEL" objSBOCommons.AgregarEmpleadoRealiza(m_intNoCotizacion, intCodigoColaborador, drwRepuestos.LineNum, strNombreColaborador)
                                Utilitarios.AsignarEmpleado(m_intNoCotizacion, intCodigoColaborador, drwRepuestos.LineNum, strNombreColaborador)

                                drwRepuestos.NombEmpleado = strNombreColaborador
                                drwRepuestos.AcceptChanges()
                            End If
                        Else
                            drwRepuestos.RejectChanges()
                        End If
                    Next

                    RaiseEvent e_AsignacionRealizada()

                Else
                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarcolaborador)
                End If

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

#End Region

#Region "Métodos"

        Private Sub CargarGridRepuestos()
            Dim dtwRepuestosXOrden As DataView

            m_adpRep = New SCGDataAccess.RepuestosxOrdenDataAdapter

            m_dstRep = Nothing

            m_dstRep = New RepuestosxOrdenDataset

            Call m_adpRep.Fill(m_dstRep, m_strNoOrden, -1, 1, 1)

            GlobalesUI.LlenarRepuestosXOrdenResources(m_dstRep)



            dtwRepuestosXOrden = New DataView(m_dstRep.SCGTA_TB_RepuestosxOrden)

            dtwRepuestosXOrden.RowFilter = "EstadoLinea='Aprobada'"

            dtgRepuestos.DataSource = dtwRepuestosXOrden

        End Sub

        Private Sub CargarComboEmpleados()

            objUtilitarios.CargarCombos(cbocolaborador, 16, 0, G_strIDSucursal)

        End Sub

        Private Sub OrdenarColumnasGrid()

            CheckDataGridViewCheckBoxColumn.DisplayIndex = 0
            NoRepuestoDataGridViewTextBoxColumn.DisplayIndex = 1
            ItemnameDataGridViewTextBoxColumn.DisplayIndex = 2
            DescEstadoResources.DisplayIndex = 3
            DataGridViewTextBoxColumn1.DisplayIndex = 4
            NombEmpleado.DisplayIndex = 5
            'DataGridViewTextBoxColumn1.Visible = False
        End Sub

        'Private Sub ProcesoIniciarFaseXOrden(ByVal p_intNofase As Integer)

        '    Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
        '    Dim intUpdateResult As Integer

        '    If Not OrdenIniciada() Then
        '        IniciarOrden()
        '    End If

        '    intUpdateResult = objDA.IniciarFase(m_strNoOrden, p_intNofase)

        'End Sub

        'Private Sub IniciarOrden()

        '    Dim drdOrden As OrdenTrabajoDataset.SCGTA_TB_OrdenRow
        '    Dim objAdapter As New SCGDataAccess.OrdenTrabajoDataAdapter
        '    Dim dtsOrden As New OrdenTrabajoDataset
        '    objAdapter.Fill(dtsOrden, m_strNoOrden)

        '    With dtsOrden
        '        drdOrden = .SCGTA_TB_Orden.FindByNoOrden(m_strNoOrden)
        '        If Not IsNothing(drdOrden) Then
        '            With drdOrden
        '                .Estado = Utilitarios.GEnum_EstadoOrden.dmsProceso
        '            End With
        '            m_blnOrdenIniciada = True
        '            objAdapter = New SCGDataAccess.OrdenTrabajoDataAdapter
        '            objAdapter.Actualizar(dtsOrden)
        '        End If
        '    End With

        '    DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.ActualizarEstadoCotizacion(drdOrden.NoCotizacion, mc_PriEstado_Proceso)

        'End Sub

        'Private Function OrdenIniciada() As Boolean
        '    Dim blnResult As Boolean

        '    If m_intEstadoorden = mc_intEstadoProceso Then
        '        blnResult = True
        '    Else
        '        blnResult = False
        '    End If

        '    Return blnResult
        'End Function

#End Region

    End Class

End Namespace

