Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmConfCentroCosto
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
        Friend WithEvents txtCentroCosto As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblCentroCosto As System.Windows.Forms.Label
        Friend WithEvents dtgCentrosCosto As System.Windows.Forms.DataGridView
        Friend WithEvents m_adpNormasReparto As DMSOneFramework.CentroCostoDatasetTableAdapters.SCGTA_VW_NormasRepartoTableAdapter
        Friend WithEvents cboNormas As SCGComboBox.SCGComboBox
        Friend WithEvents CodCentroCostoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescripcionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoLogicoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NormaReparto As System.Windows.Forms.DataGridViewTextBoxColumn
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim CentroCostoDataset1 As DMSOneFramework.CentroCostoDataset
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfCentroCosto))
            Me.ScgToolBar1 = New Proyecto_SCGToolBar.SCGToolBar
            Me.txtCentroCosto = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.lblCentroCosto = New System.Windows.Forms.Label
            Me.dtgCentrosCosto = New System.Windows.Forms.DataGridView
            Me.CodCentroCostoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescripcionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoLogicoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NormaReparto = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.m_adpNormasReparto = New DMSOneFramework.CentroCostoDatasetTableAdapters.SCGTA_VW_NormasRepartoTableAdapter
            Me.cboNormas = New SCGComboBox.SCGComboBox
            CentroCostoDataset1 = New DMSOneFramework.CentroCostoDataset
            CType(CentroCostoDataset1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtgCentrosCosto, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CentroCostoDataset1
            '
            CentroCostoDataset1.DataSetName = "CentroCostoDataset"
            CentroCostoDataset1.Locale = New System.Globalization.CultureInfo("en-US")
            CentroCostoDataset1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'ScgToolBar1
            '
            resources.ApplyResources(Me.ScgToolBar1, "ScgToolBar1")
            Me.ScgToolBar1.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.ScgToolBar1.Name = "ScgToolBar1"
            '
            'txtCentroCosto
            '
            Me.txtCentroCosto.AceptaNegativos = False
            Me.txtCentroCosto.BackColor = System.Drawing.Color.White
            Me.txtCentroCosto.EstiloSBO = True
            resources.ApplyResources(Me.txtCentroCosto, "txtCentroCosto")
            Me.txtCentroCosto.MaxDecimales = 0
            Me.txtCentroCosto.MaxEnteros = 0
            Me.txtCentroCosto.Millares = False
            Me.txtCentroCosto.Name = "txtCentroCosto"
            Me.txtCentroCosto.ReadOnly = True
            Me.txtCentroCosto.Size_AdjustableHeight = 20
            Me.txtCentroCosto.TeclasDeshacer = True
            Me.txtCentroCosto.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblLine1
            '
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.Name = "lblLine1"
            '
            'lblCentroCosto
            '
            resources.ApplyResources(Me.lblCentroCosto, "lblCentroCosto")
            Me.lblCentroCosto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCentroCosto.Name = "lblCentroCosto"
            '
            'dtgCentrosCosto
            '
            Me.dtgCentrosCosto.AllowUserToAddRows = False
            Me.dtgCentrosCosto.AllowUserToDeleteRows = False
            Me.dtgCentrosCosto.AutoGenerateColumns = False
            Me.dtgCentrosCosto.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgCentrosCosto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgCentrosCosto.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CodCentroCostoDataGridViewTextBoxColumn, Me.DescripcionDataGridViewTextBoxColumn, Me.EstadoLogicoDataGridViewTextBoxColumn, Me.NormaReparto})
            Me.dtgCentrosCosto.DataMember = "SCGTA_TB_CentroCosto"
            Me.dtgCentrosCosto.DataSource = CentroCostoDataset1
            Me.dtgCentrosCosto.GridColor = System.Drawing.Color.Silver
            resources.ApplyResources(Me.dtgCentrosCosto, "dtgCentrosCosto")
            Me.dtgCentrosCosto.Name = "dtgCentrosCosto"
            Me.dtgCentrosCosto.ReadOnly = True
            '
            'CodCentroCostoDataGridViewTextBoxColumn
            '
            Me.CodCentroCostoDataGridViewTextBoxColumn.DataPropertyName = "CodCentroCosto"
            resources.ApplyResources(Me.CodCentroCostoDataGridViewTextBoxColumn, "CodCentroCostoDataGridViewTextBoxColumn")
            Me.CodCentroCostoDataGridViewTextBoxColumn.Name = "CodCentroCostoDataGridViewTextBoxColumn"
            Me.CodCentroCostoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DescripcionDataGridViewTextBoxColumn
            '
            Me.DescripcionDataGridViewTextBoxColumn.DataPropertyName = "Descripcion"
            resources.ApplyResources(Me.DescripcionDataGridViewTextBoxColumn, "DescripcionDataGridViewTextBoxColumn")
            Me.DescripcionDataGridViewTextBoxColumn.Name = "DescripcionDataGridViewTextBoxColumn"
            Me.DescripcionDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EstadoLogicoDataGridViewTextBoxColumn
            '
            Me.EstadoLogicoDataGridViewTextBoxColumn.DataPropertyName = "EstadoLogico"
            resources.ApplyResources(Me.EstadoLogicoDataGridViewTextBoxColumn, "EstadoLogicoDataGridViewTextBoxColumn")
            Me.EstadoLogicoDataGridViewTextBoxColumn.Name = "EstadoLogicoDataGridViewTextBoxColumn"
            Me.EstadoLogicoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NormaReparto
            '
            Me.NormaReparto.DataPropertyName = "NormaReparto"
            resources.ApplyResources(Me.NormaReparto, "NormaReparto")
            Me.NormaReparto.Name = "NormaReparto"
            Me.NormaReparto.ReadOnly = True
            Me.NormaReparto.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
            '
            'm_adpNormasReparto
            '
            Me.m_adpNormasReparto.ClearBeforeFill = True
            '
            'cboNormas
            '
            Me.cboNormas.BackColor = System.Drawing.Color.White
            Me.cboNormas.DataSource = CentroCostoDataset1
            Me.cboNormas.DisplayMember = "SCGTA_VW_NormasReparto.OcrCode"
            Me.cboNormas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboNormas.EstiloSBO = True
            resources.ApplyResources(Me.cboNormas, "cboNormas")
            Me.cboNormas.Name = "cboNormas"
            Me.cboNormas.ValueMember = "SCGTA_VW_NormasReparto.OcrCode"
            '
            'frmConfCentroCosto
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.cboNormas)
            Me.Controls.Add(Me.dtgCentrosCosto)
            Me.Controls.Add(Me.txtCentroCosto)
            Me.Controls.Add(Me.lblLine1)
            Me.Controls.Add(Me.lblCentroCosto)
            Me.Controls.Add(Me.ScgToolBar1)
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmConfCentroCosto"
            CType(CentroCostoDataset1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtgCentrosCosto, System.ComponentModel.ISupportInitialize).EndInit()
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
        Private m_adpCentroCosto As SCGDataAccess.CentroCostoDataAdapter
        Private m_dstCentroCosto As CentroCostoDataset

        '-- Constantes que guardan el nombre de las columnas 
        Private mc_intCodCentroCosto As String = "CodCentroCosto"
        Private mc_strDescripcion As String = "Descripcion"
        Private mc_strEstadoLogico As String = "EstadoLogico"


        '-- Nombre de la constante de la tabla donde se consultan los estados de requisitos.
        Private mc_strTableName As String = "SCGTA_TB_CentroCosto"

        '-- Se inicializa un objeto tipo Utilitarios que recibe el string de conexión con el objetivo de usarla en funciones como carga combos.
        Private objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        '-- String de conexión
        'Public Const gc_strCadenaDeConexionSAP As String = "Integrated Security=sspi;Data Source=despinoza;Initial Catalog=BD_Sistema_Taller;Persist Security Info=False"

        '-- Tipo de inserción que se va a relizar si un update o un insert si el tipo de inserción es 1 es un insert de un nuevo objeto si no es un 2 es un update.
        Private intTipoInsercion As Integer

        Private drwCentroCosto As CentroCostoDataset.SCGTA_TB_CentroCostoRow


#End Region

#Region "Métodos"

        Private Sub cargarEstilos()

            Try
                'Inicializa el DataAdapter con la conexión
                m_adpCentroCosto = New SCGDataAccess.CentroCostoDataAdapter

                'Inicializa el DataAdapter con la conexión
                m_dstCentroCosto = New CentroCostoDataset

                m_adpNormasReparto.Connection.ConnectionString = SCGDataAccess.DAConexion.ConnectionString
                m_adpNormasReparto.Fill(m_dstCentroCosto.SCGTA_VW_NormasReparto)
                Call m_adpCentroCosto.Fill(m_dstCentroCosto)

                'With m_dstCentroCosto.SCGTA_TB_CentroCosto.DefaultView
                '    .AllowDelete = True
                '    .AllowEdit = True
                '    .AllowNew = True
                'End With

                'Carga el datagrid con el dataset en memoria
                dtgCentrosCosto.DataSource = m_dstCentroCosto.SCGTA_TB_CentroCosto

                cboNormas.DataSource = m_dstCentroCosto.SCGTA_VW_NormasReparto
                cboNormas.DisplayMember = "OcrCode"
                cboNormas.ValueMember = "OcrCode"

                '--! Jonathan Vargas V

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try



        End Sub

        Private Sub estiloGrid()

            'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.

            'Declaraciones generales
            'Dim tsConfiguracion As New DataGridTableStyle

            'dtgCentrosCosto.TableStyles.Clear()

            'Dim tcCodCentroCosto As New DataGridTextBoxColumn
            'Dim tcDescripcion As New DataGridTextBoxColumn
            'Dim tcEstadoLogico As New DataGridTextBoxColumn


            'tsConfiguracion.MappingName = m_dstCentroCosto.SCGTA_TB_CentroCosto.TableName

            ''Carga la columna codigo con las propiedades
            'With tcCodCentroCosto
            '    .Width = 0
            '    .HeaderText = My.Resources.ResourceUI.Cod
            '    .MappingName = m_dstCentroCosto.SCGTA_TB_CentroCosto.Columns(mc_intCodCentroCosto).ColumnName
            '    .Format = "###"
            '    .ReadOnly = True
            'End With


            ''Carga la columna descripcion con las propiedades
            'With tcDescripcion
            '    .Width = 234
            '    .HeaderText = My.Resources.ResourceUI.NombreCentroCosto
            '    .MappingName = m_dstCentroCosto.SCGTA_TB_CentroCosto.Columns(mc_strDescripcion).ColumnName
            '    .ReadOnly = True
            'End With


            ''Carga la columna descripcion con las propiedades
            'With tcEstadoLogico
            '    .Width = 0
            '    .HeaderText = ""
            '    .MappingName = m_dstCentroCosto.SCGTA_TB_CentroCosto.Columns(mc_strEstadoLogico).ColumnName
            '    .ReadOnly = True
            'End With


            ''Agrega las columnas al tableStyle
            'tsConfiguracion.GridColumnStyles.Add(tcCodCentroCosto)
            'tsConfiguracion.GridColumnStyles.Add(tcDescripcion)
            'tsConfiguracion.GridColumnStyles.Add(tcEstadoLogico)


            ''Establece propiedades del datagrid (colores estándares).
            'tsConfiguracion.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
            'tsConfiguracion.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
            'tsConfiguracion.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
            'tsConfiguracion.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))

            ''Hace que el datagrid adopte las propiedades del TableStyle.
            'dtgCentrosCosto.TableStyles.Add(tsConfiguracion)
            'dtgCentrosCosto.ReadOnly = True


        End Sub

        Private Sub guardar()
            Try
                If txtCentroCosto.Text.Trim <> vbNullString Then

                    If intTipoInsercion = 1 Then 'Es un nuevo centro de costo

                        '---------------------------------------- SCG Documentación ---------------------------------------------------
                        ' Sirve para agregar los valores que se desean insertar en el dataset con el objetivo de más adelante
                        ' ser registrados en la Base de Datos.
                        '---------------------------------------------------------------------------------------------------------------------

                        '-- Crea un objeto Datarow del objeto Dataset

                        Dim drwCentroCosto_new As CentroCostoDataset.SCGTA_TB_CentroCostoRow

                        '-- Se declara un nuevo row
                        drwCentroCosto_new = m_dstCentroCosto.SCGTA_TB_CentroCosto.NewRow


                        '-- Carga el row con los datos adecuados.
                        'drwCentroCosto_new.CodCentroCosto = n
                        drwCentroCosto_new.Descripcion = Me.txtCentroCosto.Text
                        If (cboNormas.SelectedValue IsNot Nothing) Then
                            drwCentroCosto_new.NormaReparto = cboNormas.SelectedValue.ToString()
                        End If
                        drwCentroCosto_new.EstadoLogico = 1


                        '-- Inserta el row en el Dataset 
                        m_dstCentroCosto.SCGTA_TB_CentroCosto.AddSCGTA_TB_CentroCostoRow(drwCentroCosto_new)


                        'Actualiza la base de datos todos los cambios hechos en el el dataset.
                        m_adpCentroCosto.Update(m_dstCentroCosto)

                        cargarEstilos()

                        Me.txtCentroCosto.Clear()


                    ElseIf intTipoInsercion = 2 Then


                        'Se modifica la actividad en el row seleccionado
                        drwCentroCosto.Descripcion = Me.txtCentroCosto.Text
                        If cboNormas.SelectedIndex = -1 Then drwCentroCosto.NormaReparto = Nothing _
                            Else drwCentroCosto.NormaReparto = cboNormas.SelectedValue.ToString()

                        'Se modifica en la base de datos mediante los metodos de la capa de negocios.
                        m_adpCentroCosto.Update(m_dstCentroCosto)

                        m_dstCentroCosto.Reset()

                        'Se refresca el grid
                        cargarEstilos()

                        'Se limpia el Textfield
                        Me.txtCentroCosto.Clear()


                    End If

                    'Al presionar el botón de nuevo se inhabilitan estos botones por lo cual se tienen que volver a habilitar
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = False
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cerrar).Enabled = True
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False

                Else
                    objSCGMSGBox.msgRequeridos()
                End If



            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try


        End Sub

#End Region

#Region "Eventos"

        Private Sub ScgToolBar1_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Guardar
            Try
                guardar()
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try


        End Sub

        Private Sub ScgToolBar1_Click_Eliminar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Eliminar
            Try
                Eliminar()
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub Eliminar()

            Dim i As Integer  'Valor que va cargar el código de estado que está en el dataset.
            Try
                'Se carga i con el valor del código del tipo de orden.
                i = dtgCentrosCosto.Rows.Item(dtgCentrosCosto.CurrentRow.Index).Cells(0).Value

                'Se busca el row seleccionado, segun el código cargado en i
                drwCentroCosto = m_dstCentroCosto.SCGTA_TB_CentroCosto.FindByCodCentroCosto(i)

                'Se asigna el valor de 0 al estado logico del estado requisito, esto equivale a que se hace una eliminación lógica.
                drwCentroCosto.EstadoLogico = 0

                'Se ejecuta la sentencia Delete, en la capa de negocios.
                m_adpCentroCosto.Delete(m_dstCentroCosto)

                'Se refresca el grid.
                cargarEstilos()

                'Limpiar textfield
                Me.txtCentroCosto.Clear()
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cerrar).Enabled = True
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False


                '---!  Jonathan Vargas (18 agosto 2005)  !---'
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try


        End Sub

        Private Sub ScgToolBar1_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Cerrar
            Me.Close()
            m_dstCentroCosto.Dispose()

        End Sub

        Private Sub ScgToolBar1_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Nuevo
            intTipoInsercion = 1
            Me.txtCentroCosto.Clear()
            Me.txtCentroCosto.Focus()
            txtCentroCosto.ReadOnly = False
        End Sub

        Private Sub frmConfEstilo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'Si decide meter un parametro en la actividad se almacenará.
            intTipoInsercion = 1

            'Cargas las actividades en el dataset
            cargarEstilos()

            'Da formato al grid.
            estiloGrid()

            'Se ocultan los botones del toolbar que no se van utilizar
            ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
            ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False
            ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Buscar).Visible = False
            ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cancelar).Visible = False

            'Se inicializan los botones eliminar y guardar inhabilitados ya que no se puede almacenar nada vacio ni eliminar si no esta un row seleccionado
            ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = False
            ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False

        End Sub

        'Private Sub dtgCentrosCosto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtgCentrosCosto.CellClick

        'End Sub

        Private Sub txtCentroCosto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCentroCosto.KeyPress

            If Asc(e.KeyChar) = Keys.Enter Then

                guardar()

            End If
        End Sub

        Private Sub txtCentroCosto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCentroCosto.GotFocus
            Me.txtCentroCosto.BackColor = System.Drawing.Color.FromArgb(CType(254, Byte), CType(244, Byte), CType(149, Byte))
        End Sub

        Private Sub txtCentroCosto_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCentroCosto.LostFocus
            Me.txtCentroCosto.BackColor = Color.White
        End Sub

        Private Sub frmConfCentroCosto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
            If Asc(e.KeyChar) = Keys.Escape Then Me.Close()
        End Sub

        Private Sub dtgCentrosCosto_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgCentrosCosto.CellClick
            Dim intCodClase As Integer    'Donde se carga el código de color

            Try
                'Se valida que cuando se selecciona algo exista al menos un row en el dataset.
                If m_dstCentroCosto.SCGTA_TB_CentroCosto.Rows.Count <> 0 Then


                    'Se pone la inserción en modo de modificación.
                    intTipoInsercion = 2
                    txtCentroCosto.ReadOnly = False

                    intCodClase = dtgCentrosCosto.Rows.Item(dtgCentrosCosto.CurrentRow.Index).Cells(0).Value

                    drwCentroCosto = m_dstCentroCosto.SCGTA_TB_CentroCosto.FindByCodCentroCosto(intCodClase)

                    Me.txtCentroCosto.Text = drwCentroCosto.Descripcion
                    If drwCentroCosto.IsNormaRepartoNull Then
                        cboNormas.SelectedIndex = -1
                    Else
                        cboNormas.SelectedValue = drwCentroCosto.NormaReparto

                    End If


                    'Se habilita tanto la modificación como la eliminación del row.
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = True
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = True
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True
                End If
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub dtgCentrosCosto_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dtgCentrosCosto.DataError

        End Sub

        Private Sub dtgCentrosCosto_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtgCentrosCosto.KeyDown
            If e.KeyCode = Keys.Delete Then
                If dtgCentrosCosto.SelectedRows.Count <> 0 Then
                    Eliminar()
                Else
                    If dtgCentrosCosto.CurrentCell.OwningColumn Is Me.NormaReparto Then
                        drwCentroCosto.NormaReparto = Nothing
                        cboNormas.SelectedIndex = -1
                    End If
                End If
            End If

        End Sub

#End Region

    End Class

End Namespace