
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports Proyecto_SCGToolBar

Namespace SCG_User_Interface


    Public Class frmConfRazonesCita
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents ScgToolBar1 As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents lblFase As System.Windows.Forms.Label
        Friend WithEvents txtrazones As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents dtgRazones As System.Windows.Forms.DataGridView
        Friend WithEvents NoRazonDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescripcionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoLogicoDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim RazonesCitaDatasetGrid As DMSOneFramework.RazonesCitaDataset
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfRazonesCita))
            Me.ScgToolBar1 = New Proyecto_SCGToolBar.SCGToolBar
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.lblFase = New System.Windows.Forms.Label
            Me.txtrazones = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.dtgRazones = New System.Windows.Forms.DataGridView
            Me.NoRazonDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescripcionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoLogicoDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
            RazonesCitaDatasetGrid = New DMSOneFramework.RazonesCitaDataset
            CType(RazonesCitaDatasetGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtgRazones, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'RazonesCitaDatasetGrid
            '
            RazonesCitaDatasetGrid.DataSetName = "RazonesCitaDataset"
            RazonesCitaDatasetGrid.Locale = New System.Globalization.CultureInfo("en-US")
            RazonesCitaDatasetGrid.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'ScgToolBar1
            '
            resources.ApplyResources(Me.ScgToolBar1, "ScgToolBar1")
            Me.ScgToolBar1.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.ScgToolBar1.Name = "ScgToolBar1"
            '
            'lblLine1
            '
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.Name = "lblLine1"
            '
            'lblFase
            '
            resources.ApplyResources(Me.lblFase, "lblFase")
            Me.lblFase.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblFase.Name = "lblFase"
            '
            'txtrazones
            '
            Me.txtrazones.AceptaNegativos = False
            Me.txtrazones.BackColor = System.Drawing.Color.White
            Me.txtrazones.EstiloSBO = True
            resources.ApplyResources(Me.txtrazones, "txtrazones")
            Me.txtrazones.MaxDecimales = 0
            Me.txtrazones.MaxEnteros = 0
            Me.txtrazones.Millares = False
            Me.txtrazones.Name = "txtrazones"
            Me.txtrazones.ReadOnly = True
            Me.txtrazones.Size_AdjustableHeight = 20
            Me.txtrazones.TeclasDeshacer = True
            Me.txtrazones.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AlfaNumeric
            '
            'dtgRazones
            '
            Me.dtgRazones.AllowUserToAddRows = False
            Me.dtgRazones.AllowUserToDeleteRows = False
            Me.dtgRazones.AutoGenerateColumns = False
            Me.dtgRazones.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgRazones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgRazones.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NoRazonDataGridViewTextBoxColumn, Me.DescripcionDataGridViewTextBoxColumn, Me.EstadoLogicoDataGridViewCheckBoxColumn})
            Me.dtgRazones.DataMember = "SCGTA_TB_RazonesCita"
            Me.dtgRazones.DataSource = RazonesCitaDatasetGrid
            Me.dtgRazones.GridColor = System.Drawing.Color.Silver
            resources.ApplyResources(Me.dtgRazones, "dtgRazones")
            Me.dtgRazones.Name = "dtgRazones"
            Me.dtgRazones.ReadOnly = True
            '
            'NoRazonDataGridViewTextBoxColumn
            '
            Me.NoRazonDataGridViewTextBoxColumn.DataPropertyName = "NoRazon"
            resources.ApplyResources(Me.NoRazonDataGridViewTextBoxColumn, "NoRazonDataGridViewTextBoxColumn")
            Me.NoRazonDataGridViewTextBoxColumn.Name = "NoRazonDataGridViewTextBoxColumn"
            Me.NoRazonDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DescripcionDataGridViewTextBoxColumn
            '
            Me.DescripcionDataGridViewTextBoxColumn.DataPropertyName = "Descripcion"
            resources.ApplyResources(Me.DescripcionDataGridViewTextBoxColumn, "DescripcionDataGridViewTextBoxColumn")
            Me.DescripcionDataGridViewTextBoxColumn.Name = "DescripcionDataGridViewTextBoxColumn"
            Me.DescripcionDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EstadoLogicoDataGridViewCheckBoxColumn
            '
            Me.EstadoLogicoDataGridViewCheckBoxColumn.DataPropertyName = "EstadoLogico"
            resources.ApplyResources(Me.EstadoLogicoDataGridViewCheckBoxColumn, "EstadoLogicoDataGridViewCheckBoxColumn")
            Me.EstadoLogicoDataGridViewCheckBoxColumn.Name = "EstadoLogicoDataGridViewCheckBoxColumn"
            Me.EstadoLogicoDataGridViewCheckBoxColumn.ReadOnly = True
            '
            'frmConfRazonesCita
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.dtgRazones)
            Me.Controls.Add(Me.txtrazones)
            Me.Controls.Add(Me.lblLine1)
            Me.Controls.Add(Me.lblFase)
            Me.Controls.Add(Me.ScgToolBar1)
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmConfRazonesCita"
            CType(RazonesCitaDatasetGrid, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtgRazones, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

#End Region

#Region "Declaraciones"
        Friend Event RetornaDatos()
        Private m_adpRazones As SCGDataAccess.RazonesCitaDataAdapter
        Private m_dstRazones As RazonesCitaDataset

        'Constantes que guardan el nombre de las columnas 
        Private mc_strCodigo As String = "NoRazon"
        Private mc_strDescripcion As String = "Descripcion"
        Private mc_strEstadoLogico As String = "EstadoLogico"
        Private v_intUltimoCodigo As Integer

        'Nombre de la constante de la tabla
        Private mc_strTableName As String = "SCGTA_TB_RazonesCita"

        'Tipo de inserción si es una actualización en la base de datos o una inserción.
        Private intTipoInsercion As Integer

        Private drw As RazonesCitaDataset.SCGTA_TB_RazonesCitaRow


#End Region

#Region "Eventos"
        Private Sub frmConfRazonesCita_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            intTipoInsercion = 1 'Se inicializa el tipo de inserción en modo nuevo (insertar)

            cargar()

            'Se ocultan los botones del toolbar que no se van utilizar
            ScgToolBar1.Buttons(SCGToolBar.enumButton.Exportar).Visible = False
            ScgToolBar1.Buttons(SCGToolBar.enumButton.Imprimir).Visible = False
            ScgToolBar1.Buttons(SCGToolBar.enumButton.Buscar).Visible = False
            ScgToolBar1.Buttons(SCGToolBar.enumButton.Cancelar).Visible = False

            'Se inicializan los botones eliminar y guardar inhabilitados ya que no se puede almacenar nada vacio ni eliminar si no esta un row seleccionado
            ScgToolBar1.Buttons(SCGToolBar.enumButton.Eliminar).Enabled = False
            ScgToolBar1.Buttons(SCGToolBar.enumButton.Guardar).Enabled = False

        End Sub

        Private Sub ScgToolBar1_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Guardar

            guardar()

        End Sub


        Private Sub ScgToolBar1_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Cerrar

            Me.Close()
            m_dstRazones.Dispose()


        End Sub


        Private Sub ScgToolBar1_Click_Eliminar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Eliminar
            Eliminar()

        End Sub
        Private Sub Eliminar()

            Try
                'Se cambia la columna estado lógico en 0 del row seleccionado. Esto con el objetivo de que se registre un cambio en el dataset.
                m_dstRazones.Tables(m_dstRazones.SCGTA_TB_RazonesCita.TableName).Rows(dtgRazones.CurrentRow.Index).Item("EstadoLogico") = 0

                'Actualiza todos los cambios hechos en el el dataset. Importante que la eliminacion no es física sino virtual,
                'simplemente modifica un estado lógico de 1 a 0 por eso se usa el comando update
                m_adpRazones.Delete(m_dstRazones)

                'Se vuelve a cargar el datagrid con el objetivo de que se vea actualizado el cambio.
                cargar()

                'Se limpia el textfield
                Me.txtrazones.Clear()

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub ScgToolBar1_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Nuevo
            'Se pone el tipo de inserción tipo INSERT.
            intTipoInsercion = 1
            Me.txtrazones.Clear()
            txtrazones.ReadOnly = False
            Me.txtrazones.Focus()
        End Sub

        Private Sub txtRazones_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtrazones.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then

                guardar()

            End If
        End Sub
#End Region

#Region "Métodos"

        Private Sub cargar()

            m_adpRazones = New SCGDataAccess.RazonesCitaDataAdapter
            m_dstRazones = New RazonesCitaDataset
            'estiloGrid()
            Call m_adpRazones.Fill(m_dstRazones)
            'With m_dstRazones.SCGTA_TB_RazonesCita.DefaultView
            '    .AllowDelete = True
            '    .AllowEdit = True
            '    .AllowNew = True
            'End With
            dtgRazones.DataSource = m_dstRazones.SCGTA_TB_RazonesCita

        End Sub

        'Private Sub estiloGrid()


        '    'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.

        '    'Declaraciones generales
        '    Dim tsConfiguracion As New DataGridTableStyle
        '    dtgRazones.TableStyles.Clear()

        '    Dim tcCodigo As New DataGridTextBoxColumn
        '    Dim tcDescripcion As New DataGridTextBoxColumn
        '    Dim tcEstadoLogico As New DataGridTextBoxColumn

        '    tsConfiguracion.MappingName = m_dstRazones.SCGTA_TB_RazonesCita.TableName()


        '    'Carga la columna codigo con las propiedades
        '    With tcCodigo
        '        .Width = 0
        '        .HeaderText = My.Resources.ResourceUI.Codigo '"Código"
        '        .MappingName = m_dstRazones.SCGTA_TB_RazonesCita.Columns(mc_strCodigo).ColumnName
        '        .Format = "###"
        '        .ReadOnly = True
        '    End With

        '    'Carga la columna descripcion con las propiedades
        '    With tcDescripcion
        '        .Width = 234
        '        .HeaderText = My.Resources.ResourceUI.Razones
        '        .MappingName = m_dstRazones.SCGTA_TB_RazonesCita.Columns(mc_strDescripcion).ColumnName
        '        .ReadOnly = True
        '    End With

        '    'Carga la columna estado lógico con las propiedades
        '    With tcEstadoLogico
        '        .Width = 0
        '        .HeaderText = My.Resources.ResourceUI.Estado
        '        .MappingName = m_dstRazones.SCGTA_TB_RazonesCita.Columns(mc_strEstadoLogico).ColumnName
        '        .Format = "###"
        '        .ReadOnly = True
        '    End With

        '    'Agrega las columnas al tableStyle
        '    tsConfiguracion.GridColumnStyles.Add(tcCodigo)
        '    tsConfiguracion.GridColumnStyles.Add(tcDescripcion)
        '    tsConfiguracion.GridColumnStyles.Add(tcEstadoLogico)

        '    'Establece propiedades del datagrid (colores estándares).
        '    tsConfiguracion.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
        '    tsConfiguracion.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
        '    tsConfiguracion.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
        '    tsConfiguracion.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))

        '    'Hace que el datagrid adopte las propiedades del TableStyle.
        '    dtgRazones.TableStyles.Add(tsConfiguracion)
        '    dtgRazones.ReadOnly = True

        'End Sub

        Private Sub guardar()

            Try
                If txtrazones.Text <> "" Then
                    If intTipoInsercion = 1 Then 'Es una nueva fase de producción.


                        'Dim i As String

                        'Primero es el número del último código rgistrado, después se le suma uno
                        'para que se registre en el dataset con un número mayor que nunca va a existir.
                        'Dim n As Integer


                        '-- Crea un objeto Datarow del objeto Dataset Fase
                        Dim drw As RazonesCitaDataset.SCGTA_TB_RazonesCitaRow


                        'Se valida que si no existen valores en la base de datos ponga n en 1 o sino se cae al ingresar el primer row.
                        'If m_dstRazones.SCGTA_TB_RazonesCita.Rows.Count = 0 Then

                        '    n = 1

                        'Else

                        '    n = m_dstRazones.SCGTA_TB_RazonesCita.Rows.Count

                        '    i = dtgRazones.Item(n - 1, 0)

                        '    n = CInt(i) + 1
                        'End If




                        '-- Se declara un nuevo row
                        drw = m_dstRazones.SCGTA_TB_RazonesCita.NewRow()


                        '-- Carga el row con los datos adecuados.
                        'drw.NoRazon = n
                        drw.Descripcion = Me.txtrazones.Text
                        'drw.EstadoLogico = 1


                        '-- Inserta el row en el Dataset 
                        m_dstRazones.SCGTA_TB_RazonesCita.AddSCGTA_TB_RazonesCitaRow(drw)

                        'Actualiza la base de datos todos los cambios hechos en el el dataset.
                        m_adpRazones.Update(m_dstRazones)

                        cargar()

                        Me.txtrazones.Clear()
                        txtrazones.Focus()

                        'Al presionar el botón de nuevo se inhabilitan estos botones por lo cual se tienen que volver a habilitar
                        ScgToolBar1.Buttons(SCGToolBar.enumButton.Eliminar).Enabled = True
                        ScgToolBar1.Buttons(SCGToolBar.enumButton.Cerrar).Enabled = True
                        ScgToolBar1.Buttons(SCGToolBar.enumButton.Nuevo).Enabled = True

                    ElseIf intTipoInsercion = 2 Then


                        'Se modifica la actividad en el row seleccionado
                        drw.Descripcion = Me.txtrazones.Text

                        'Se modifica en la base de datos mediante los metodos de la capa de negocios.
                        m_adpRazones.Update(m_dstRazones)

                        m_dstRazones.Reset()

                        'Se refresca el grid
                        cargar()

                        'Se limpia el Textfield
                        Me.txtrazones.Clear()

                        txtrazones.ReadOnly = True
                    End If
                Else
                    objSCGMSGBox.msgRequeridos()
                End If

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub


#End Region

        Private Sub frmConfRazonesCita_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
            If Asc(e.KeyChar) = Keys.Escape Then Me.Close()
        End Sub

        Private Sub txtrazones_KeyPress1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtrazones.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then
                guardar()
            End If
        End Sub

        Private Sub dtgRazones_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtgRazones.KeyDown
            If e.KeyCode = Keys.Delete Then
                Eliminar()
            End If
        End Sub

        Private Sub dtgRazones_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgRazones.CellClick
            Dim i As Integer 'Carga el codigo de agencia

            Try
                'Se pone la inserción en modo de modificación.
                intTipoInsercion = 2
                txtrazones.ReadOnly = False
                'Se le asigna el c[odigo de la agencia seleccionado a i con el objetivo de hacer una busqueda por codigo
                i = dtgRazones.Rows.Item(dtgRazones.CurrentRow.Index).Cells(0).Value

                drw = m_dstRazones.SCGTA_TB_RazonesCita.FindByNoRazon(i)

                Me.txtrazones.Text = drw.Descripcion

                'Se habilita tanto la modificación como la eliminación del row.
                ScgToolBar1.Buttons(SCGToolBar.enumButton.Eliminar).Enabled = True
                ScgToolBar1.Buttons(SCGToolBar.enumButton.Guardar).Enabled = True
                ScgToolBar1.Buttons(SCGToolBar.enumButton.Nuevo).Enabled = True

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub
    End Class
End Namespace
