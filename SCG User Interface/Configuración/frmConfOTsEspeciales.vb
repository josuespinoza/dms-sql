Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmConfOTsEspeciales

#Region "Constructures"

        Sub New(ByVal cargaforma As Boolean)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()


            ' Add any initialization after the InitializeComponent() call.

        End Sub

#End Region

#Region "Declaraciones"

#Region "Acceso a datos"

        Private m_adpConfOrdenesEspeciales As New ConfOTsEspecialesDataAdapter
        Private m_drwConfOrdenesEspeciales As ConfOrdenesEspeciales.SCGTA_TB_ConfOrdenesEspecialesRow

        Private m_drdTiposOrdenes As SqlClient.SqlDataReader
        Private m_adpTiposOrdenes As New TipoOrdenDataAdapter

        Private m_adpUsuario As New UsuariosOTEspecialDataAdapter

#End Region

#Region "Variables"

        Private m_intID As Integer
        Private m_intIDAsesor As Integer
        Private m_intIDtipoOrden As Integer
        Private m_blnUsaListaPrecioCliente As Boolean
        Private m_strCardCodeCliente As String
        

#End Region

#Region "Objetos"

        Private WithEvents m_objBuscador As New Buscador.SubBuscador

        Private m_objUtilitarios As New Utilitarios(strConexionADO)

        Private m_objMensajeria As New Proyecto_SCGMSGBox.SCGMSGBox

#End Region

#End Region

#Region "Eventos"

        Private Sub tlbOTsEspeciales_Click_Cancelar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbOTsEspeciales.Click_Cancelar

            Try

                Call Limpiar()

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub tlbOTsEspeciales_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbOTsEspeciales.Click_Cerrar

            Try

                Me.Close()

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub tlbOTsEspeciales_Click_Eliminar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbOTsEspeciales.Click_Eliminar

            Try

                Call Eliminar()

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub tlbOTsEspeciales_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbOTsEspeciales.Click_Guardar

            Try

                Call Guardar()

            Catch ex As SqlClient.SqlException

                If ex.ErrorCode = -2146232060 Then
                    m_objMensajeria.msgInformationCustom(My.Resources.ResourceUI.MensajeTipoordenTieneConfiguracion)
                    'Call Limpiar()
                Else
                    Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                    'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
                End If

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub tlbOTsEspeciales_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbOTsEspeciales.Click_Nuevo

            Try

                Call EstadoControles(2)

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub frmConfAgendas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                If g_blnGeneraOTsEspeciales Then

                    m_adpConfOrdenesEspeciales.Fill(m_dtsConfOrdenesEspeciales)
                    dtgTiposConfigurados.DataSource = m_dtsConfOrdenesEspeciales

                    m_adpTiposOrdenes.Fill(m_drdTiposOrdenes)

                    Call Utilitarios.CargarComboSourceByReader(cboTiposOrdenes, m_drdTiposOrdenes)

                    Call EstadoControles(1)

                Else

                    m_objMensajeria.msgInformationCustom(My.Resources.ResourceUI.MensajeCreacionOTEspecNoHabilitada)

                End If

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
                'Agregado 02072010
                If m_drdTiposOrdenes IsNot Nothing Then
                    If Not m_drdTiposOrdenes.IsClosed Then
                        Call m_drdTiposOrdenes.Close()
                    End If
                End If

            End Try

        End Sub

        Private Sub dtgTiposConfigurados_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgTiposConfigurados.Click

            Try

                If dtgTiposConfigurados.CurrentRow IsNot Nothing Then

                    m_intID = CInt(dtgTiposConfigurados.CurrentRow.Cells(0).Value)
                    m_drwConfOrdenesEspeciales = m_dtsConfOrdenesEspeciales.SCGTA_TB_ConfOrdenesEspeciales.FindByID(m_intID)
                    cboTiposOrdenes.Text = m_drwConfOrdenesEspeciales.DescTipoOrden
                    txtAsesor.Tag = m_drwConfOrdenesEspeciales.IDAsesor
                    txtAsesor.Text = m_drwConfOrdenesEspeciales.NombreAsesor
                    If Not m_drwConfOrdenesEspeciales.IsCardCodeClienteNull Then
                        txtCliente.Tag = m_drwConfOrdenesEspeciales.CardCodeCliente
                        If Not String.IsNullOrEmpty(txtCliente.Tag) Then
                            UsaListaPreciosCheckBox.Enabled = True
                        Else
                            UsaListaPreciosCheckBox.Enabled = False
                        End If
                        If Not m_drwConfOrdenesEspeciales.IsCardNameClienteNull Then
                            txtCliente.Text = m_drwConfOrdenesEspeciales.CardNameCliente
                        Else
                            txtCliente.Text = ""
                        End If
                    Else
                        UsaListaPreciosCheckBox.Enabled = False

                    End If
                    If m_dtsUsuarioOTEspecial IsNot Nothing Then
                        m_dtsUsuarioOTEspecial = Nothing
                        m_dtsUsuarioOTEspecial = New UsuariosOTEspecialDataset

                    End If

                    UsaListaPreciosCheckBox.Checked = m_drwConfOrdenesEspeciales.UsaListaPrecios

                    m_adpUsuario.Fill(m_dtsUsuarioOTEspecial, m_intID)

                    dtgUsuarios.DataSource = m_dtsUsuarioOTEspecial
                    dtgUsuarios.DataMember = m_dtsUsuarioOTEspecial.SCGTA_TB_ConfUsuariosConfOTEspecial.TableName
                    Call EstadoControles(3)

                End If

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub picCliente_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picCliente.Click
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorClientes
                m_objBuscador.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre
                m_objBuscador.Criterios = "CardCode, CardName"
                m_objBuscador.Tabla = "SCGTA_VW_Clientes"
                m_objBuscador.Where = ""
                m_objBuscador.Criterios_Ocultos = 0
                m_objBuscador.Activar_Buscador(sender)

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub picAsesor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picAsesor.Click
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador = New Buscador.SubBuscador
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorAsesores

                m_objBuscador.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre & "," & My.Resources.ResourceUI.Apellido

                m_objBuscador.Criterios = "empID, FirstName, lastName"
                m_objBuscador.Tabla = "SCGTA_VW_OHEM"
                m_objBuscador.Where = "userId is not null"
                m_objBuscador.Activar_Buscador(sender)

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub objBuscador_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_objBuscador.AppAceptar

            Try

                Select Case sender.name

                    Case picCliente.Name

                        txtCliente.Tag = Arreglo_Campos(0)
                        txtCliente.Text = Arreglo_Campos(1)
                        UsaListaPreciosCheckBox.Enabled = True

                    Case picAsesor.Name
                        txtAsesor.Tag = Arreglo_Campos(0)
                        txtAsesor.Text = Arreglo_Campos(1) + " " + Arreglo_Campos(2)

                    Case btnAgregar.Name

                        Dim intCantidad As Integer
                        Dim drwUsuario As UsuariosOTEspecialDataset.SCGTA_TB_ConfUsuariosConfOTEspecialRow

                        For intCantidad = 0 To m_objBuscador.OUT_DataTable.Rows.Count - 1

                            drwUsuario = m_dtsUsuarioOTEspecial.SCGTA_TB_ConfUsuariosConfOTEspecial.NewSCGTA_TB_ConfUsuariosConfOTEspecialRow
                            drwUsuario.IDConfOTEspecial = m_intID
                            drwUsuario.Usuario = m_objBuscador.OUT_DataTable.Rows.Item(intCantidad).Item(0)

                            m_dtsUsuarioOTEspecial.SCGTA_TB_ConfUsuariosConfOTEspecial.AddSCGTA_TB_ConfUsuariosConfOTEspecialRow(drwUsuario)

                        Next

                End Select

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub frmConfOTsEspeciales_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

            If Not g_blnGeneraOTsEspeciales Then
                Me.Close()
            End If

        End Sub

#End Region

#Region "Métodos"

        Private Sub Guardar()

            Dim blnGuardar As Boolean = True
            Dim cnConeccion As SqlClient.SqlConnection = Nothing
            Dim tnTransaccion As SqlClient.SqlTransaction = Nothing
            Dim drwUsuarios As UsuariosOTEspecialDataset.SCGTA_TB_ConfUsuariosConfOTEspecialRow

            Try

                errConfOrdenesEspeciales.Clear()

                If txtAsesor.Text.Trim(" ") <> "" Then
                    m_intIDAsesor = txtAsesor.Tag

                Else
                    ''blnGuardar = False
                    ''errConfOrdenesEspeciales.SetError(txtAsesor, My.Resources.ResourceUI.MensajeDebeSeleccionarAsesor)
                    ''errConfOrdenesEspeciales.SetIconAlignment(txtAsesor, ErrorIconAlignment.MiddleRight)
                    m_intIDAsesor = -1
                End If

                If txtCliente.Tag <> "" Then
                    m_strCardCodeCliente = txtCliente.Tag
                End If


                If cboTiposOrdenes.SelectedIndex > -1 Then

                    m_intIDtipoOrden = cboTiposOrdenes.SelectedValue

                Else
                    blnGuardar = False
                    errConfOrdenesEspeciales.SetError(cboTiposOrdenes, My.Resources.ResourceUI.MensajeDebeSeleccionarTipoOT)
                    errConfOrdenesEspeciales.SetIconAlignment(cboTiposOrdenes, ErrorIconAlignment.MiddleRight)

                End If

                m_blnUsaListaPrecioCliente = UsaListaPreciosCheckBox.Checked

                If blnGuardar Then

                    If m_intID = 0 Then
                        Call Insertar()
                    Else
                        Call Actualizar()
                    End If

                    m_adpConfOrdenesEspeciales.Update(m_dtsConfOrdenesEspeciales, cnConeccion, tnTransaccion)

                    If m_intID = 0 Then
                        For Each drwUsuarios In m_dtsUsuarioOTEspecial.SCGTA_TB_ConfUsuariosConfOTEspecial.Rows
                            If drwUsuarios.RowState <> DataRowState.Deleted Then
                                drwUsuarios.IDConfOTEspecial = m_drwConfOrdenesEspeciales.ID
                            End If
                        Next
                    End If
                    m_adpUsuario.Update(m_dtsUsuarioOTEspecial, cnConeccion, tnTransaccion)
                    tnTransaccion.Commit()

                    Call Limpiar()

                End If
            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

                tnTransaccion.Rollback()
                Throw ex

            Finally
                cnConeccion.Close()
            End Try
        End Sub

        Private Sub Insertar()

            m_drwConfOrdenesEspeciales = m_dtsConfOrdenesEspeciales.SCGTA_TB_ConfOrdenesEspeciales.NewSCGTA_TB_ConfOrdenesEspecialesRow

            With m_drwConfOrdenesEspeciales

                .IDAsesor = m_intIDAsesor
                .IDTipoOrden = m_intIDtipoOrden
                .CardCodeCliente = m_strCardCodeCliente
                .UsaListaPrecios = m_blnUsaListaPrecioCliente

            End With
            m_dtsConfOrdenesEspeciales.SCGTA_TB_ConfOrdenesEspeciales.AddSCGTA_TB_ConfOrdenesEspecialesRow(m_drwConfOrdenesEspeciales)

        End Sub

        Private Sub Eliminar()

            Dim blnGuardar As Boolean = True
            Dim cnConeccion As SqlClient.SqlConnection = Nothing
            Dim tnTransaccion As SqlClient.SqlTransaction = Nothing

            If m_intID > 0 Then
                m_drwConfOrdenesEspeciales = m_dtsConfOrdenesEspeciales.SCGTA_TB_ConfOrdenesEspeciales.FindByID(m_intID)
                If m_drwConfOrdenesEspeciales IsNot Nothing Then


                    m_drwConfOrdenesEspeciales.Delete()



                End If

            End If
            'dtgTiposConfigurados.DataSource = Nothing
            'dtgTiposConfigurados.DataSource = m_dtsConfOrdenesEspeciales

            'm_dtsConfOrdenesEspeciales.SCGTA_TB_ConfOrdenesEspeciales.RemoveSCGTA_TB_ConfOrdenesEspecialesRow(m_drwConfOrdenesEspeciales)

            m_dtsConfOrdenesEspeciales.AcceptChanges()
            m_adpConfOrdenesEspeciales.CrearDeleteOTEspecial(m_intID)
        End Sub

        Private Sub Actualizar()

            m_drwConfOrdenesEspeciales = m_dtsConfOrdenesEspeciales.SCGTA_TB_ConfOrdenesEspeciales.FindByID(m_intID)
            If m_drwConfOrdenesEspeciales IsNot Nothing Then

                With m_drwConfOrdenesEspeciales

                    .IDAsesor = m_intIDAsesor
                    .IDTipoOrden = m_intIDtipoOrden
                    .CardCodeCliente = m_strCardCodeCliente
                    .UsaListaPrecios = m_blnUsaListaPrecioCliente

                End With

            End If

        End Sub

        Private Sub Limpiar()

            m_intID = 0
            m_strCardCodeCliente = ""
            m_intIDAsesor = 0
            txtAsesor.Clear()
            txtCliente.Clear()
            txtAsesor.Tag = ""
            txtCliente.Tag = ""
            m_dtsConfOrdenesEspeciales = Nothing
            m_dtsConfOrdenesEspeciales = New ConfOrdenesEspeciales
            m_adpConfOrdenesEspeciales.Fill(m_dtsConfOrdenesEspeciales)
            dtgTiposConfigurados.DataSource = m_dtsConfOrdenesEspeciales
            errConfOrdenesEspeciales.Clear()
            If m_dtsUsuarioOTEspecial IsNot Nothing Then
                m_dtsUsuarioOTEspecial = Nothing
                m_dtsUsuarioOTEspecial = New UsuariosOTEspecialDataset
            End If

            dtgUsuarios.DataSource = m_dtsUsuarioOTEspecial
            dtgUsuarios.DataMember = m_dtsUsuarioOTEspecial.SCGTA_TB_ConfUsuariosConfOTEspecial.TableName
            UsaListaPreciosCheckBox.Checked = False
            UsaListaPreciosCheckBox.Enabled = False

            Call EstadoControles(1)
            cboTiposOrdenes.SelectedIndex = -1

        End Sub

        Private Sub EstadoControles(ByVal p_intComoMostrarlos As Integer)

            With tlbOTsEspeciales

                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False
                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Buscar).Visible = False

                Select Case p_intComoMostrarlos
                    Case 1 'Estado Inicial

                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False
                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = False
                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True
                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cancelar).Enabled = True
                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cerrar).Enabled = True
                        picAsesor.Enabled = False
                        cboTiposOrdenes.Enabled = False
                        picAsesor.Enabled = False

                    Case 2 'Estado al presionar nuevo
                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = True
                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = False
                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = False
                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cancelar).Enabled = True
                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cerrar).Enabled = True
                        picAsesor.Enabled = True
                        cboTiposOrdenes.Enabled = True
                        picCliente.Enabled = True

                    Case 3 'Estado Modificación
                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = True
                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = True
                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = False
                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cancelar).Enabled = True
                        .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cerrar).Enabled = True
                        picAsesor.Enabled = True
                        cboTiposOrdenes.Enabled = False
                        picAsesor.Enabled = True

                End Select

            End With

        End Sub

#End Region

        Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
            Dim DATemp As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                DATemp = New DMSOneFramework.SCGDataAccess.DAConexion

                With m_objBuscador

                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarratituloBuscadorusuariosSBO
                    .Titulos = My.Resources.ResourceUI.Usuario & "," & My.Resources.ResourceUI.Nombre
                    .Criterios = "User_Code,U_Name"
                    .Criterios_OcultosEx = ""
                    .MultiSeleccion = True
                    .Tabla = "SCGTA_VW_OUSR"
                    If G_strIDSucursal <> "" Then
                        .Where = "branch=" & G_strIDSucursal
                    Else
                        .Where = ""
                    End If
                    .Activar_Buscador(sender)
                End With

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
            Dim drwUsuario As UsuariosOTEspecialDataset.SCGTA_TB_ConfUsuariosConfOTEspecialRow
            Dim blnEliminarLineas As Boolean

            Try
                blnEliminarLineas = True
                Do While blnEliminarLineas
                    blnEliminarLineas = False
                    For Each drwUsuario In m_dtsUsuarioOTEspecial.SCGTA_TB_ConfUsuariosConfOTEspecial.Rows
                        If drwUsuario.RowState <> DataRowState.Deleted Then
                            If drwUsuario.Check Then
                                drwUsuario.Delete()
                                blnEliminarLineas = True
                                Exit For
                            End If
                        End If

                    Next
                Loop

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

    End Class

End Namespace