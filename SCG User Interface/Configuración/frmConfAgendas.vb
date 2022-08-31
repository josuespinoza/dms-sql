Imports DMSOneFramework
Imports Proyecto_SCGToolBar.SCGToolBar
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmConfAgendas

#Region "Constructures"

        Sub New(ByVal cargaforma As Boolean)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()


            ' Add any initialization after the InitializeComponent() call.

        End Sub

#End Region

#Region "Declaraciones"

#Region "Acceso a datos"

        Private m_dstAgendas As New AgendaDataset
        Private m_adpAgendas As New AgendaDataAdapter

#End Region

#Region "Variables"

        Private m_intID As Integer
        Private m_strDescripcion As String
        Private m_strAbreviatura As String
        Private m_blnEstadoLogico As Boolean
        Private m_intCantidadCitas As Integer
        ' Private m_intCodTecnico As Integer
        Private m_intCodTecnico As Nullable(Of Integer)

        Private m_intCodAsesor As Integer
        Private m_intRazonCita As Integer
        Private m_strArticuloAgenda As String


        Private WithEvents m_objBuscador As New Buscador.SubBuscador

#End Region

#End Region

#Region "Eventos"

        Private Sub tlbAgendas_Click_Cancelar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbAgendas.Click_Cancelar

            Try

                Call Limpiar()

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub tlbAgendas_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbAgendas.Click_Cerrar

            Try

                Me.Close()

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub tlbAgendas_Click_Eliminar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbAgendas.Click_Eliminar

            Try

                Call Eliminar()

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub tlbAgendas_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbAgendas.Click_Guardar

            Try

                Call Guardar()

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub tlbAgendas_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbAgendas.Click_Nuevo

            Try

                Call EstadoControles(2)

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub frmConfAgendas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                m_adpAgendas.Fill(m_dstAgendas)
                AgendaDataTableBindingSource.DataSource = m_dstAgendas.SCGTA_TB_Agendas
                Call EstadoControles(1)

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub dtgAgendas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgAgendas.Click

            Try

                If dtgAgendas.SelectedRows.Count > 0 Then

                    m_intID = CInt(dtgAgendas.SelectedRows(0).Cells(1).Value)
                    m_strDescripcion = dtgAgendas.SelectedRows(0).Cells(2).Value
                    txtAgenda.Text = m_strDescripcion
                    nudIntervalo.Value = dtgAgendas.SelectedRows(0).Cells(3).Value
                    m_strAbreviatura = dtgAgendas.SelectedRows(0).Cells(4).Value
                    txtAbreviatura.Text = m_strAbreviatura

                    If IsDBNull(dtgAgendas.SelectedRows(0).Cells(5).Value) Then

                        m_intCodAsesor = 0
                    Else

                        m_intCodAsesor = dtgAgendas.SelectedRows(0).Cells(5).Value
                    End If

                    If IsDBNull(dtgAgendas.SelectedRows(0).Cells(6).Value) Then
                        m_intCodTecnico = Nothing

                    Else

                        m_intCodTecnico = CType(dtgAgendas.SelectedRows(0).Cells(6).Value, Integer)
                    End If

                    If IsDBNull(dtgAgendas.SelectedRows(0).Cells(7).Value) Then
                        m_intRazonCita = 0

                    Else

                        m_intRazonCita = dtgAgendas.SelectedRows(0).Cells(7).Value
                    End If

                    If IsDBNull(dtgAgendas.SelectedRows(0).Cells(8).Value) Then
                        m_strArticuloAgenda = ""
                    Else
                        m_strArticuloAgenda = dtgAgendas.SelectedRows(0).Cells(8).Value
                    End If

                    If IsDBNull(dtgAgendas.SelectedRows(0).Cells(9).Value) Then
                        txtAsesorAgenda.Text = ""
                    Else
                        txtAsesorAgenda.Text = dtgAgendas.SelectedRows(0).Cells(9).Value

                    End If
                    If IsDBNull(dtgAgendas.SelectedRows(0).Cells(10).Value) Then
                        txtTecnicoAgenda.Text = ""
                    Else

                        txtTecnicoAgenda.Text = dtgAgendas.SelectedRows(0).Cells(10).Value
                    End If

                    If IsDBNull(dtgAgendas.SelectedRows(0).Cells(11).Value) Then
                        txtRazonCita.Text = ""
                    Else

                        txtRazonCita.Text = dtgAgendas.SelectedRows(0).Cells(11).Value
                    End If

                    If IsDBNull(dtgAgendas.SelectedRows(0).Cells(12).Value) Then
                        txtArticuloAgenda.Text = ""
                    Else

                        txtArticuloAgenda.Text = dtgAgendas.SelectedRows(0).Cells(12).Value
                    End If

                    Call EstadoControles(3)

                End If

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub objBuscador_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_objBuscador.AppAceptar

            Try

                Select Case sender.name

                    Case "PictureBoxTecnico"
                        'txtTecnicoAgenda.Text = Arreglo_Campos(0)
                        m_intCodTecnico = CType(Arreglo_Campos(0), Integer)
                        txtTecnicoAgenda.Text = Arreglo_Campos(1)

                    Case "PictureBoxAsesor"
                        'txtAsesorAgenda.Text = Arreglo_Campos(0)
                        m_intCodAsesor = Arreglo_Campos(0)
                        txtAsesorAgenda.Text = Arreglo_Campos(1)

                    Case "PictureBoxRazon"
                        'txtRazonCita.Text = Arreglo_Campos(0)
                        m_intRazonCita = Arreglo_Campos(0)
                        txtRazonCita.Text = Arreglo_Campos(1)


                    Case "PictureBoxArticuloCita"
                        'txtArticuloAgenda.Text = Arreglo_Campos(0)
                        m_strArticuloAgenda = Arreglo_Campos(0)
                        txtArticuloAgenda.Text = Arreglo_Campos(1)


                End Select

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub PictureBoxTecnico_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBoxTecnico.Click
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador = New Buscador.SubBuscador
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.TituloEmpleados
                m_objBuscador.Titulos = My.Resources.ResourceUI.Cod & "," & My.Resources.ResourceUI.Apellido & "," & My.Resources.ResourceUI.Nombre  '"Codigo, Nombre, Apellido"
                m_objBuscador.Criterios = "empID,firstName, lastName"
                m_objBuscador.Tabla = "SCGTA_VW_OHEM"
                m_objBuscador.Where = ""
                'objBuscador.Criterios_Ocultos = 0
                m_objBuscador.Activar_Buscador(sender)

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub PictureBoxAsesor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBoxAsesor.Click
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador = New Buscador.SubBuscador
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorAsesores
                m_objBuscador.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre & "," & My.Resources.ResourceUI.Apellido '"Código, Nombre, Apellidos"
                m_objBuscador.Criterios = "empID, FirstName, lastName"
                m_objBuscador.Tabla = "SCGTA_VW_OHEM"
                m_objBuscador.Where = "userId is not null"
                m_objBuscador.Activar_Buscador(sender)

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub PictureBoxRazon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBoxRazon.Click

            Try
                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador = New Buscador.SubBuscador
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.TituloRazonCita
                m_objBuscador.Titulos = My.Resources.ResourceUI.Razon & "," & My.Resources.ResourceUI.Descripcion  '"Razon, Descripcion"
                m_objBuscador.Criterios = "NoRazon, Descripcion"
                m_objBuscador.Tabla = " SCGTA_VW_RazonCita"
                m_objBuscador.Where = "EstadoLogico = 1"
                m_objBuscador.Activar_Buscador(sender)


            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            End Try

        End Sub

        Private Sub PictureBoxArticuloCita_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBoxArticuloCita.Click

            Try
                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador = New Buscador.SubBuscador
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.TituloArticulos '"Articulos"
                m_objBuscador.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre '"Codigo, NombreArticulo"
                m_objBuscador.Criterios = "ItemCode, ItemName"
                m_objBuscador.Tabla = "SCGTA_VW_OITM"
                m_objBuscador.Activar_Buscador(sender)


            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            End Try

        End Sub


#End Region

#Region "Métodos"

        Private Sub Guardar()

            If txtAgenda.Text.Trim(" ") <> "" Then
                m_strDescripcion = txtAgenda.Text.Trim(" ")
                If nudIntervalo.Value <> 0 Then
                    m_intCantidadCitas = nudIntervalo.Value
                    If txtAbreviatura.Text.Trim(" ") <> "" Then
                        m_strAbreviatura = txtAbreviatura.Text.Trim(" ")
                        'If txtTecnicoAgenda.Text.Trim("") <> "" Then
                        'm_intCodTecnico = txtTecnicoAgenda.Text.Trim(" ")
                        'If txtAsesorAgenda.Text.Trim("") <> "" Then
                        ' m_intCodAsesor = txtAsesorAgenda.Text.Trim(" ")
                        ' If txtRazonCita.Text.Trim("") <> "" Then
                        '  m_intRazonCita = txtRazonCita.Text.Trim(" ")

                        'If txtArticuloAgenda.Text.Trim("") <> "" Then
                        'm_strArticuloAgenda = txtArticuloAgenda.Text.Trim(" ")

                        If m_intID = 0 Then
                            Call Insertar()
                        Else
                            Call Actualizar()
                        End If
                        m_adpAgendas.Update(m_dstAgendas)
                        Call Limpiar()
                        'Else
                        '    MessageBox.Show("Debe agregar Articulo en la Agenda")
                        'End If

                        'Else
                        '    MessageBox.Show("Debe agregar Razon en la Agenda")
                        'End If
                        'Else
                        '    MessageBox.Show("Debe agregar Asesor en la Agenda")
                        'End If
                        'Else
                        '    MessageBox.Show("Debe agregar Tecnico en la Agenda")
                        'End If

                    Else
                        MessageBox.Show(My.Resources.ResourceUI.MensajeDebeIngresarAbrevAgenda)
                    End If
                Else
                    MessageBox.Show(My.Resources.ResourceUI.MensajeDebeIngresarIntervaloCitas)
                End If
            Else
                MessageBox.Show(My.Resources.ResourceUI.MensajeDebeingresarDescipAgenda)
            End If

        End Sub

        Private Sub Insertar()

            Dim drwAgenda As AgendaDataset.SCGTA_TB_AgendasRow

            drwAgenda = m_dstAgendas.SCGTA_TB_Agendas.NewSCGTA_TB_AgendasRow

            With drwAgenda

                .Agenda = m_strDescripcion
                .IntervaloCitas = m_intCantidadCitas
                .Abreviatura = m_strAbreviatura

                If m_intCodTecnico Is Nothing Then
                    .SetCodTecnicoNull()
                Else
                    .CodTecnico = m_intCodTecnico
                End If

                .CodAsesor = m_intCodAsesor
                .RazonCita = m_intRazonCita
                .ArticuloCita = m_strArticuloAgenda



            End With
            m_dstAgendas.SCGTA_TB_Agendas.AddSCGTA_TB_AgendasRow(drwAgenda)

        End Sub

        Private Sub Eliminar()

            If m_intID > 0 Then
                Dim drwAgenda As AgendaDataset.SCGTA_TB_AgendasRow
                drwAgenda = m_dstAgendas.SCGTA_TB_Agendas.FindByID(m_intID)
                If drwAgenda IsNot Nothing Then

                    drwAgenda.EstadoLogico = False

                End If

            End If
            AgendaDataTableBindingSource.DataSource = Nothing
            AgendaDataTableBindingSource.DataSource = m_dstAgendas.SCGTA_TB_Agendas
        End Sub

        Private Sub Actualizar()

            Dim drwAgenda As AgendaDataset.SCGTA_TB_AgendasRow
            drwAgenda = m_dstAgendas.SCGTA_TB_Agendas.FindByID(m_intID)
            If drwAgenda IsNot Nothing Then

                With drwAgenda

                    .Agenda = m_strDescripcion
                    .IntervaloCitas = m_intCantidadCitas
                    .Abreviatura = m_strAbreviatura
                    '.CodTecnico = m_intCodTecnico
                    If m_intCodTecnico Is Nothing Then
                        .SetCodTecnicoNull()
                    Else
                        .CodTecnico = m_intCodTecnico
                    End If

                    .CodAsesor = m_intCodAsesor
                    .RazonCita = m_intRazonCita
                    .ArticuloCita = m_strArticuloAgenda

                End With

            End If

        End Sub

        Private Sub Limpiar()

            m_intID = 0
            m_strDescripcion = ""
            m_blnEstadoLogico = False
            m_intCantidadCitas = 0
            txtAgenda.Clear()
            nudIntervalo.Value = 15
            txtAbreviatura.Clear()
            txtTecnicoAgenda.Clear()
            txtAsesorAgenda.Clear()
            txtRazonCita.Clear()
            txtArticuloAgenda.Clear()
            m_dstAgendas = Nothing
            m_dstAgendas = New AgendaDataset
            m_adpAgendas.Fill(m_dstAgendas)
            AgendaDataTableBindingSource.DataSource = m_dstAgendas.SCGTA_TB_Agendas
            Call EstadoControles(1)

        End Sub

        Private Sub EstadoControles(ByVal p_intComoMostrarlos As Integer)

            With tlbAgendas

                .Buttons(enumButton.Exportar).Visible = False
                .Buttons(enumButton.Imprimir).Visible = False
                .Buttons(enumButton.Buscar).Visible = False

                Select Case p_intComoMostrarlos
                    Case 1 'Estado Inicial

                        .Buttons(enumButton.Guardar).Enabled = False
                        .Buttons(enumButton.Eliminar).Enabled = False
                        .Buttons(enumButton.Nuevo).Enabled = True
                        .Buttons(enumButton.Cancelar).Enabled = True
                        .Buttons(enumButton.Cerrar).Enabled = True
                        txtAgenda.ReadOnly = True
                        nudIntervalo.ReadOnly = True
                        txtAbreviatura.ReadOnly = True

                    Case 2 'Estado al presionar nuevo
                        .Buttons(enumButton.Guardar).Enabled = True
                        .Buttons(enumButton.Eliminar).Enabled = False
                        .Buttons(enumButton.Nuevo).Enabled = False
                        .Buttons(enumButton.Cancelar).Enabled = True
                        .Buttons(enumButton.Cerrar).Enabled = True
                        txtAgenda.ReadOnly = False
                        nudIntervalo.ReadOnly = False
                        txtAbreviatura.ReadOnly = False

                    Case 3 'Estado Modificación
                        .Buttons(enumButton.Guardar).Enabled = True
                        .Buttons(enumButton.Eliminar).Enabled = True
                        .Buttons(enumButton.Nuevo).Enabled = False
                        .Buttons(enumButton.Cancelar).Enabled = True
                        .Buttons(enumButton.Cerrar).Enabled = True
                        txtAgenda.ReadOnly = False
                        nudIntervalo.ReadOnly = False
                        txtAbreviatura.ReadOnly = False

                End Select

            End With

        End Sub

#End Region


    End Class

End Namespace