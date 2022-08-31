
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Class frmBusquedaSolicitudesEspecificos

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

#End Region

#Region "Declaraciones"

#Region "Variables"

        'Variables para la búsqueda
        Private m_intNoVisita As Integer
        Private m_strNoVehiculo As String
        Private m_strNoOrden As String
        Private m_intNoSolicitud As Integer
        Private m_strCodMarca As String
        Private m_strCodEstilo As String
        Private m_strCodModelo As String
        Private m_strPlaca As String
        Private m_dtSolicitud_ini As Date
        Private m_dtRespuesta_ini As Date
        Private m_dtSolicitud_fin As Date
        Private m_dtRespuesta_fin As Date
        Private m_intCodEstado As Integer

#End Region

#Region "Acceso a Datos"

        Private m_adpMarcas As MarcaDataAdapter
        Private m_drdMarcas As SqlClient.SqlDataReader
        Private m_adpEstilos As EstiloDataAdapter
        Private m_drdEstilos As SqlClient.SqlDataReader
        Private m_adpModelos As ModelosDataAdapter
        Private m_drdModelos As SqlClient.SqlDataReader

        Private m_dtsSolicitudEspecificos As New SolicitudEspecificosDataset
        Private m_adpSolicitudEspecificos As New SolicitudEspecificosDataAdapter

#End Region

#Region "Objetos Generales"

        Private m_objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        Private WithEvents m_objSolicitudEspecificos As frmSolicitudEspecificos

#End Region

#End Region

#Region "Métodos"

        Private Sub LimpiarCriteriosBusqueda()

            m_dtsSolicitudEspecificos.SCGTA_SP_SelSolicitudEspecifico.Rows.Clear()

            txtPlaca.Text = ""
            m_strPlaca = ""

            txtNoVisita.Text = ""
            m_intNoVisita = -1
            
            txtNoOrden.Text = ""
            m_strNoOrden = ""

            txtNoSolicitud.Text = ""
            m_intNoSolicitud = -1

            cboMarca.DataSource = Nothing
            chkMarca.Checked = False
            m_strCodMarca = ""

            chkEstado.Checked = False
            m_intCodEstado = -1

            txtNoVehiculo.Text = ""
            m_strNoVehiculo = ""

            cboEstilo.DataSource = Nothing
            chkEstilo.Checked = False
            m_strCodEstilo = ""

            cboModelo.DataSource = Nothing
            chkModelo.Checked = False
            m_strCodModelo = ""


            dtpAperturaini.Value = m_objUtilitarios.CargarFechaHoraServidor()
            m_dtSolicitud_ini = Nothing
            m_dtSolicitud_fin = Nothing

            dtpCompromisoini.Value = m_objUtilitarios.CargarFechaHoraServidor()
            m_dtRespuesta_ini = Nothing
                m_dtRespuesta_fin = Nothing

            dtgDetalles.DataSource = Nothing
            dtgDetalles.DataSource = m_dtsSolicitudEspecificos
            dtgDetalles.DataMember = m_dtsSolicitudEspecificos.SCGTA_SP_SelSolicitudEspecifico.TableName

        End Sub

        Private Sub BusquedaSolicitudes()

            Try

                If ValidarCriteriosBusqueda() = True Then

                    m_adpSolicitudEspecificos = New SCGDataAccess.SolicitudEspecificosDataAdapter

                    m_dtsSolicitudEspecificos.Dispose()

                    m_dtsSolicitudEspecificos = Nothing

                    m_dtsSolicitudEspecificos = New SolicitudEspecificosDataset


                    If Me.txtPlaca.Text <> "" Then
                        m_strPlaca = Trim(Me.txtPlaca.Text)
                    Else
                        m_strPlaca = ""
                    End If


                    If Me.txtNoVisita.Text <> "" And Me.txtNoVisita.Text <> "0" Then
                        m_intNoVisita = CInt(Me.txtNoVisita.Text)
                    Else
                        m_intNoVisita = -1
                    End If


                    If Me.txtNoOrden.Text <> "" Then
                        m_strNoOrden = Trim(Me.txtNoOrden.Text)
                    Else
                        m_strNoOrden = ""
                    End If

                    If Me.txtNoSolicitud.Text <> "" And Me.txtNoSolicitud.Text <> "0" Then
                        m_intNoSolicitud = CInt(Trim(Me.txtNoSolicitud.Text))
                    Else
                        m_intNoSolicitud = -1
                    End If

                    If Me.cboMarca.Text <> "" Then
                        m_strCodMarca = Me.cboMarca.SelectedValue
                    Else
                        m_strCodMarca = ""
                    End If


                    If Me.cboEstado.Text <> "" Then
                        m_intCodEstado = Me.cboEstado.SelectedIndex
                    Else
                        m_intCodEstado = -1
                    End If

                    If txtNoVehiculo.Text <> "" Then
                        m_strNoVehiculo = txtNoVehiculo.Text
                    Else
                        m_strNoVehiculo = ""
                    End If

                    If Me.cboEstilo.Text <> "" Then
                        m_strCodEstilo = Me.cboEstilo.SelectedValue
                    Else
                        m_strCodEstilo = ""
                    End If

                    If Me.cboModelo.Text <> "" Then
                        m_strCodModelo = Me.cboModelo.SelectedValue
                    Else
                        m_strCodModelo = ""
                    End If

                    If chkSolicitud.Checked Then
                        m_dtSolicitud_ini = New Date(dtpAperturaini.Value.Year, dtpAperturaini.Value.Month, dtpAperturaini.Value.Day, 0, 0, 0)
                        m_dtSolicitud_fin = New Date(dtpAperturafin.Value.Year, dtpAperturafin.Value.Month, dtpAperturafin.Value.Day, 23, 59, 0)
                    Else
                        m_dtSolicitud_ini = Nothing
                        m_dtSolicitud_fin = Nothing
                    End If

                    If chkRespuesta.Checked Then
                        m_dtRespuesta_ini = New Date(dtpCompromisoini.Value.Year, dtpCompromisoini.Value.Month, dtpCompromisoini.Value.Day, 0, 0, 0)
                        m_dtRespuesta_fin = New Date(dtpCompromisofin.Value.Year, dtpCompromisofin.Value.Month, dtpCompromisofin.Value.Day, 23, 59, 0)
                    Else
                        m_dtRespuesta_ini = Nothing
                        m_dtRespuesta_fin = Nothing
                    End If

                    m_dtsSolicitudEspecificos = Nothing
                    m_dtsSolicitudEspecificos = New SolicitudEspecificosDataset

                    Call m_adpSolicitudEspecificos.Fill(m_dtsSolicitudEspecificos, m_intNoSolicitud, m_strNoOrden, m_strNoVehiculo, m_strPlaca, _
                    m_intNoVisita, m_intCodEstado, m_strCodMarca, m_strCodEstilo, m_strCodModelo, m_dtSolicitud_ini, _
                    m_dtSolicitud_fin, m_dtRespuesta_ini, m_dtRespuesta_fin)

                    GlobalesUI.LlenarEstadoSolicitudEspecificosResources(m_dtsSolicitudEspecificos)

                    dtgDetalles.DataSource = Nothing
                    dtgDetalles.DataSource = m_dtsSolicitudEspecificos
                    dtgDetalles.DataMember = m_dtsSolicitudEspecificos.SCGTA_SP_SelSolicitudEspecifico.TableName

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub


        Private Function ValidarCriteriosBusqueda() As Boolean

            'Variable que determina el si existen o no criterios de búsqueda en caso que no exista ninguno 
            'se devuelve la variable con un false en caso que exista uno o más se devuelve con un true.
            Dim blnValido As Boolean
            Try


                'Se inicia como false...en caso que exista al menos un criterio se convierte en true
                blnValido = False

                If Me.txtNoOrden.Text <> "" Then
                    blnValido = True
                ElseIf Me.txtNoSolicitud.Text <> "" Then
                    blnValido = True
                ElseIf Me.txtNoVisita.Text <> "" Then
                    blnValido = True
                ElseIf txtNoVehiculo.Text <> "" Then
                    blnValido = True
                ElseIf Me.txtPlaca.Text <> "" Then
                    blnValido = True
                ElseIf Me.cboEstado.Text <> "" Then
                    blnValido = True
                ElseIf Me.chkMarca.Checked Then
                    blnValido = True
                ElseIf chkEstilo.Checked Then
                    blnValido = True
                ElseIf chkModelo.Checked Then
                    blnValido = True
                ElseIf chkSolicitud.Checked = True Then
                    blnValido = True
                ElseIf chkRespuesta.Checked Then
                    blnValido = True
                End If

                Return blnValido

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Function

#End Region

#Region "Eventos"

        Private Sub frmBusquedaSolicitudesEspecificos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try


                dtpAperturaini.Value = m_objUtilitarios.CargarFechaHoraServidor.Date
                dtpCompromisoini.Value = dtpAperturaini.Value
                dtpCompromisofin.Value = dtpAperturaini.Value
                dtpAperturafin.Value = dtpAperturaini.Value

                tlbSolicitudespecificos.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
                tlbSolicitudespecificos.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False
                tlbSolicitudespecificos.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Visible = False
                tlbSolicitudespecificos.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Visible = False
                tlbSolicitudespecificos.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Visible = False

                'clsUtilidadCombos.CargarComboEstadoSolicitudesEspecificas(cboEstado)

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub chkMarca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMarca.Click
            Try

                If chkMarca.Checked Then

                    m_adpMarcas = New MarcaDataAdapter
                    m_drdMarcas = Nothing

                    m_adpMarcas.CargaMarcasdeVehiculo(m_drdMarcas)
                    Utilitarios.CargarComboSourceByReader(cboMarca, m_drdMarcas)

                Else

                    cboMarca.DataSource = Nothing
                    cboEstilo.DataSource = Nothing
                    chkEstilo.Checked = False
                    chkModelo.Checked = False

                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally

                m_drdMarcas = Nothing
                m_adpMarcas = Nothing

                'Agregado 01072010
                If m_drdMarcas IsNot Nothing Then
                    If Not m_drdMarcas.IsClosed Then
                        Call m_drdMarcas.Close()
                    End If
                End If

            End Try

        End Sub

        Private Sub chkEstilo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEstilo.CheckedChanged


            Try

                If chkMarca.Checked Then
                    If chkEstilo.Checked Then

                        m_adpEstilos = New EstiloDataAdapter
                        m_drdEstilos = Nothing

                        m_adpEstilos.CargaEstilosdeVehiculo(m_drdEstilos, cboMarca.SelectedValue)
                        Utilitarios.CargarComboSourceByReader(cboEstilo, m_drdEstilos)
                    Else
                        cboEstilo.DataSource = Nothing
                        chkModelo.Checked = False
                        cboModelo.DataSource = Nothing

                    End If
                Else

                    chkModelo.Checked = False
                    cboModelo.DataSource = Nothing
                    cboEstilo.DataSource = Nothing
                End If

            Catch ex As Exception


                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally

                m_drdMarcas = Nothing
                m_adpMarcas = Nothing

                'Agregado 01072010
                If m_drdEstilos IsNot Nothing Then
                    If Not m_drdEstilos.IsClosed Then
                        Call m_drdEstilos.Close()
                    End If
                End If

            End Try

        End Sub

        Private Sub cboMarca_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMarca.SelectedIndexChanged

            Try

                If chkEstilo.Checked Then

                    m_adpEstilos = New EstiloDataAdapter
                    m_drdEstilos = Nothing

                    m_adpEstilos.CargaEstilosdeVehiculo(m_drdEstilos, cboMarca.SelectedValue)
                    Utilitarios.CargarComboSourceByReader(cboEstilo, m_drdEstilos)
                Else

                    cboEstilo.DataSource = Nothing

                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally
                'Agregado 01072010
                If m_drdEstilos IsNot Nothing Then
                    If Not m_drdEstilos.IsClosed Then
                        Call m_drdEstilos.Close()
                    End If
                End If
            End Try

        End Sub

        Private Sub chkEstado_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEstado.CheckedChanged

            Try

                If chkEstado.Checked Then
                    clsUtilidadCombos.CargarComboEstadoSolicitudesEspecificas(cboEstado)
                Else
                    cboEstado.DataSource = Nothing
                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub chkModelo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkModelo.CheckedChanged

            Try

                If chkModelo.Checked Then

                    m_adpModelos = New ModelosDataAdapter
                    m_drdModelos = Nothing

                    m_adpModelos.CargaModelosdeVehiculo(m_drdModelos, cboEstilo.SelectedValue)
                    Utilitarios.CargarComboSourceByReader(cboModelo, m_drdModelos)

                Else

                    cboEstilo.DataSource = Nothing

                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally
                'Agregado 02072010
                If m_drdModelos IsNot Nothing Then
                    If Not m_drdModelos.IsClosed Then
                        Call m_drdModelos.Close()
                    End If
                End If
            End Try

        End Sub

        Private Sub tlbSolicitudespecificos_Click_Buscar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbSolicitudespecificos.Click_Buscar

            Try

                Call BusquedaSolicitudes()

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub tlbSolicitudespecificos_Click_Cancelar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbSolicitudespecificos.Click_Cancelar

            Try

                Call LimpiarCriteriosBusqueda()

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub tlbSolicitudespecificos_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbSolicitudespecificos.Click_Cerrar

            Try

                Me.Close()

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub dtgDetalles_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgDetalles.DoubleClick

            Dim intIDSolicitud As Integer
            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean




            Try

                If dtgDetalles.CurrentRow IsNot Nothing Then

                    intIDSolicitud = CInt(dtgDetalles.CurrentRow.Cells(0).Value)

                    For Each Forma_Nueva In Me.MdiParent.MdiChildren

                        If Forma_Nueva.Name = "frmSolicitudEspecificos" Then
                            blnExisteForm = True
                        End If

                    Next

                    If Not blnExisteForm Then

                        m_objSolicitudEspecificos = New frmSolicitudEspecificos(intIDSolicitud)

                        m_objSolicitudEspecificos.MdiParent = Me.MdiParent

                        m_objSolicitudEspecificos.Show()

                    End If

                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub frmBusquedaSolicitudes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboEstado.KeyPress, cboEstilo.KeyPress, cboMarca.KeyPress, cboModelo.KeyPress, _
                                                                                                                         dtpAperturafin.KeyPress, dtpAperturaini.KeyPress, dtpCompromisofin.KeyPress, _
                                                                                                                         dtpCompromisoini.KeyPress, txtNoOrden.KeyPress, txtNoSolicitud.KeyPress, txtNoVehiculo.KeyPress, _
                                                                                                                         txtNoVisita.KeyPress, txtPlaca.KeyPress
            Try

                If Asc(e.KeyChar) = Keys.Enter Then

                    Call BusquedaSolicitudes()

                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub


        Private Sub m_objSolicitudEspecificos_eSolicitudCreada(ByVal p_intNoOslicitud As Integer) Handles m_objSolicitudEspecificos.eSolicitudCreada
            Try

                Call LimpiarCriteriosBusqueda()

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

#End Region

        Private Sub lblNoOrden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblNoOrden.Click

        End Sub
    End Class

End Namespace