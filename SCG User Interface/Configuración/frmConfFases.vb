Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmConfFases


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
        Friend WithEvents btnCerrar As System.Windows.Forms.Button
        Friend WithEvents ScgToolBar1 As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents lblFase As System.Windows.Forms.Label
        Friend WithEvents cboCentroCosto As SCGComboBox.SCGComboBox
        Friend WithEvents lblCentroCosto As System.Windows.Forms.Label
        Friend WithEvents txtfase As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents lblLine2 As System.Windows.Forms.Label
        Public WithEvents lblLineaUnidad As System.Windows.Forms.Label
        Friend WithEvents lblUnidad As System.Windows.Forms.Label
        Friend WithEvents rbtnUnidadCantidad As System.Windows.Forms.RadioButton
        Friend WithEvents rbtnUnidadMonto As System.Windows.Forms.RadioButton
        Friend WithEvents dtgFases As System.Windows.Forms.DataGridView
        Friend WithEvents NoFaseDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescripcionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodCentroCostoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoLogicoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents UnidadDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescUnidadDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim FaseProduccionDatasetGrid As DMSOneFramework.FaseProduccionDataset
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfFases))
            Me.btnCerrar = New System.Windows.Forms.Button
            Me.ScgToolBar1 = New Proyecto_SCGToolBar.SCGToolBar
            Me.lblLine2 = New System.Windows.Forms.Label
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.lblFase = New System.Windows.Forms.Label
            Me.cboCentroCosto = New SCGComboBox.SCGComboBox
            Me.lblCentroCosto = New System.Windows.Forms.Label
            Me.txtfase = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.lblLineaUnidad = New System.Windows.Forms.Label
            Me.lblUnidad = New System.Windows.Forms.Label
            Me.rbtnUnidadCantidad = New System.Windows.Forms.RadioButton
            Me.rbtnUnidadMonto = New System.Windows.Forms.RadioButton
            Me.dtgFases = New System.Windows.Forms.DataGridView
            Me.NoFaseDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescripcionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodCentroCostoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoLogicoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.UnidadDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescUnidadDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            FaseProduccionDatasetGrid = New DMSOneFramework.FaseProduccionDataset
            CType(FaseProduccionDatasetGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtgFases, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'FaseProduccionDatasetGrid
            '
            FaseProduccionDatasetGrid.DataSetName = "FaseProduccionDataset"
            FaseProduccionDatasetGrid.Locale = New System.Globalization.CultureInfo("en-US")
            FaseProduccionDatasetGrid.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'btnCerrar
            '
            resources.ApplyResources(Me.btnCerrar, "btnCerrar")
            Me.btnCerrar.Name = "btnCerrar"
            '
            'ScgToolBar1
            '
            resources.ApplyResources(Me.ScgToolBar1, "ScgToolBar1")
            Me.ScgToolBar1.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.ScgToolBar1.Name = "ScgToolBar1"
            '
            'lblLine2
            '
            Me.lblLine2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine2, "lblLine2")
            Me.lblLine2.Name = "lblLine2"
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
            'cboCentroCosto
            '
            Me.cboCentroCosto.BackColor = System.Drawing.Color.White
            Me.cboCentroCosto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboCentroCosto.EstiloSBO = True
            resources.ApplyResources(Me.cboCentroCosto, "cboCentroCosto")
            Me.cboCentroCosto.Items.AddRange(New Object() {Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation})
            Me.cboCentroCosto.Name = "cboCentroCosto"
            '
            'lblCentroCosto
            '
            resources.ApplyResources(Me.lblCentroCosto, "lblCentroCosto")
            Me.lblCentroCosto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCentroCosto.Name = "lblCentroCosto"
            '
            'txtfase
            '
            Me.txtfase.AceptaNegativos = False
            Me.txtfase.BackColor = System.Drawing.Color.White
            Me.txtfase.EstiloSBO = True
            resources.ApplyResources(Me.txtfase, "txtfase")
            Me.txtfase.MaxDecimales = 0
            Me.txtfase.MaxEnteros = 0
            Me.txtfase.Millares = False
            Me.txtfase.Name = "txtfase"
            Me.txtfase.ReadOnly = True
            Me.txtfase.Size_AdjustableHeight = 20
            Me.txtfase.TeclasDeshacer = True
            Me.txtfase.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AlfaNumeric
            '
            'lblLineaUnidad
            '
            Me.lblLineaUnidad.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLineaUnidad, "lblLineaUnidad")
            Me.lblLineaUnidad.Name = "lblLineaUnidad"
            '
            'lblUnidad
            '
            resources.ApplyResources(Me.lblUnidad, "lblUnidad")
            Me.lblUnidad.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblUnidad.Name = "lblUnidad"
            '
            'rbtnUnidadCantidad
            '
            resources.ApplyResources(Me.rbtnUnidadCantidad, "rbtnUnidadCantidad")
            Me.rbtnUnidadCantidad.Name = "rbtnUnidadCantidad"
            Me.rbtnUnidadCantidad.TabStop = True
            Me.rbtnUnidadCantidad.UseVisualStyleBackColor = True
            '
            'rbtnUnidadMonto
            '
            resources.ApplyResources(Me.rbtnUnidadMonto, "rbtnUnidadMonto")
            Me.rbtnUnidadMonto.Name = "rbtnUnidadMonto"
            Me.rbtnUnidadMonto.TabStop = True
            Me.rbtnUnidadMonto.UseVisualStyleBackColor = True
            '
            'dtgFases
            '
            Me.dtgFases.AllowUserToAddRows = False
            Me.dtgFases.AllowUserToDeleteRows = False
            Me.dtgFases.AutoGenerateColumns = False
            Me.dtgFases.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgFases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgFases.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NoFaseDataGridViewTextBoxColumn, Me.DescripcionDataGridViewTextBoxColumn, Me.CodCentroCostoDataGridViewTextBoxColumn, Me.EstadoLogicoDataGridViewTextBoxColumn, Me.UnidadDataGridViewTextBoxColumn, Me.DescUnidadDataGridViewTextBoxColumn})
            Me.dtgFases.DataMember = "SCGTA_TB_FasesProduccion"
            Me.dtgFases.DataSource = FaseProduccionDatasetGrid
            Me.dtgFases.GridColor = System.Drawing.Color.Silver
            resources.ApplyResources(Me.dtgFases, "dtgFases")
            Me.dtgFases.Name = "dtgFases"
            Me.dtgFases.ReadOnly = True
            '
            'NoFaseDataGridViewTextBoxColumn
            '
            Me.NoFaseDataGridViewTextBoxColumn.DataPropertyName = "NoFase"
            resources.ApplyResources(Me.NoFaseDataGridViewTextBoxColumn, "NoFaseDataGridViewTextBoxColumn")
            Me.NoFaseDataGridViewTextBoxColumn.Name = "NoFaseDataGridViewTextBoxColumn"
            Me.NoFaseDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DescripcionDataGridViewTextBoxColumn
            '
            Me.DescripcionDataGridViewTextBoxColumn.DataPropertyName = "Descripcion"
            resources.ApplyResources(Me.DescripcionDataGridViewTextBoxColumn, "DescripcionDataGridViewTextBoxColumn")
            Me.DescripcionDataGridViewTextBoxColumn.Name = "DescripcionDataGridViewTextBoxColumn"
            Me.DescripcionDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodCentroCostoDataGridViewTextBoxColumn
            '
            Me.CodCentroCostoDataGridViewTextBoxColumn.DataPropertyName = "CodCentroCosto"
            resources.ApplyResources(Me.CodCentroCostoDataGridViewTextBoxColumn, "CodCentroCostoDataGridViewTextBoxColumn")
            Me.CodCentroCostoDataGridViewTextBoxColumn.Name = "CodCentroCostoDataGridViewTextBoxColumn"
            Me.CodCentroCostoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EstadoLogicoDataGridViewTextBoxColumn
            '
            Me.EstadoLogicoDataGridViewTextBoxColumn.DataPropertyName = "EstadoLogico"
            resources.ApplyResources(Me.EstadoLogicoDataGridViewTextBoxColumn, "EstadoLogicoDataGridViewTextBoxColumn")
            Me.EstadoLogicoDataGridViewTextBoxColumn.Name = "EstadoLogicoDataGridViewTextBoxColumn"
            Me.EstadoLogicoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'UnidadDataGridViewTextBoxColumn
            '
            Me.UnidadDataGridViewTextBoxColumn.DataPropertyName = "Unidad"
            resources.ApplyResources(Me.UnidadDataGridViewTextBoxColumn, "UnidadDataGridViewTextBoxColumn")
            Me.UnidadDataGridViewTextBoxColumn.Name = "UnidadDataGridViewTextBoxColumn"
            Me.UnidadDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DescUnidadDataGridViewTextBoxColumn
            '
            Me.DescUnidadDataGridViewTextBoxColumn.DataPropertyName = "DescUnidad"
            resources.ApplyResources(Me.DescUnidadDataGridViewTextBoxColumn, "DescUnidadDataGridViewTextBoxColumn")
            Me.DescUnidadDataGridViewTextBoxColumn.Name = "DescUnidadDataGridViewTextBoxColumn"
            Me.DescUnidadDataGridViewTextBoxColumn.ReadOnly = True
            '
            'frmConfFases
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.dtgFases)
            Me.Controls.Add(Me.rbtnUnidadMonto)
            Me.Controls.Add(Me.rbtnUnidadCantidad)
            Me.Controls.Add(Me.lblLineaUnidad)
            Me.Controls.Add(Me.lblUnidad)
            Me.Controls.Add(Me.txtfase)
            Me.Controls.Add(Me.lblLine2)
            Me.Controls.Add(Me.lblLine1)
            Me.Controls.Add(Me.lblFase)
            Me.Controls.Add(Me.cboCentroCosto)
            Me.Controls.Add(Me.lblCentroCosto)
            Me.Controls.Add(Me.btnCerrar)
            Me.Controls.Add(Me.ScgToolBar1)
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmConfFases"
            Me.Tag = "Configuración,1"
            CType(FaseProduccionDatasetGrid, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtgFases, System.ComponentModel.ISupportInitialize).EndInit()
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

        Private m_adpFase As FaseProduccionDataAdapter
        Private m_dstFase As New FaseProduccionDataset

        '-- Constantes que guardan el nombre de las columnas 
        Private mc_intNoCentroCosto As String = "CodCentroCosto"
        Private mc_intNoFase As String = "NoFase"
        Private mc_strDescripcion As String = "Descripcion"
        Private mc_strEstadoLogico As String = "EstadoLogico"
        Private mc_strUnidad As String = "Unidad"
        Private mc_strDescUnidad As String = "DescUnidad"



        '-- Nombre de la constante de la tabla donde se consultan las fases de producción
        Private mc_strTableName As String = "SCGTA_TB_FasesProduccion"

        '-- Se inicializa un objeto tipo Utilitarios que recibe el string de conexión con el objetivo de usarla en funciones como carga combos.
        Private objUtilitarios As New Utilitarios(strConectionString)

        '-- Tipo de inserción que se va a relizar si un update o un insert si el tipo de inserción es 1 es un insert de un nuevo objeto si no es un 2 es un update.
        Private intTipoInsercion As Integer

        Private drw As FaseProduccionDataset.SCGTA_TB_FasesProduccionRow

        Private intFaseProduccion As Integer

#End Region

#Region "Eventos"

        Private Sub frmConfFases_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                intTipoInsercion = 1

                intFaseProduccion = 1
                'cargar(intFaseProduccion)

                'Carga los centros de costo en el combo
                objUtilitarios.CargarCombos(Me.cboCentroCosto, 2)

                If Me.cboCentroCosto.Items.Count <> 0 Then
                    'Carga las fases de produccion en el dataset
                    Me.cboCentroCosto.SelectedIndex = 0

                End If

                'Se ocultan los botones del toolbar que no se van utilizar
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Buscar).Visible = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cancelar).Visible = False

                'Se inicializan los botones eliminar y guardar inhabilitados ya que no se puede almacenar nada vacio ni eliminar si no esta un row seleccionado
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

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

            'Dim i As Integer
            If cboCentroCosto.Items.Count > 0 Then

                '-- Se cambia la columna estado lógico en 0 para modificar el row - y se le resta un número a la variable i para obtener el verdadero row en el dataset.
                m_dstFase.Tables(m_dstFase.SCGTA_TB_FasesProduccion.TableName).Rows(dtgFases.CurrentRow.Index).Item("EstadoLogico") = 0

                'Se inicializa la fase de producción
                intFaseProduccion = CInt(dtgFases.Rows.Item(dtgFases.CurrentRow.Index).Cells(0).Value)
                m_adpFase.Delete(m_dstFase)


                cargar(CInt(Busca_Codigo_Texto(cboCentroCosto.SelectedItem, True)))
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True
                intTipoInsercion = 1
                'Se limpian los textfields.
                txtfase.Clear()
            End If

        End Sub

        Private Sub cboCentroCosto_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCentroCosto.SelectedIndexChanged

            Try
                If intTipoInsercion <> 2 Then
                    cargar(CInt(Busca_Codigo_Texto(cboCentroCosto.Text, True)))
                    txtfase.Clear()
                End If
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try




        End Sub

        Private Sub ScgToolBar1_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Cerrar
            Try
                Me.Close()
                m_dstFase.Dispose()
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub ScgToolBar1_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Nuevo
            Try
                intTipoInsercion = 1
                txtfase.Clear()
                txtfase.Focus()
                cboCentroCosto.Enabled = True
                txtfase.ReadOnly = False
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub frmConfFases_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Escape Then Me.Close()
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub txtfase_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtfase.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    guardar()
                End If
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

#End Region

#Region "Métodos"

        Private Sub cargar(ByVal intCodCentroCosto As Integer)
            Try
                If cboCentroCosto.Items.Count > 0 Then
                    If Not IsNothing(m_dstFase) Then
                        m_dstFase.Dispose()
                    End If

                    'Inicializa el DataAdapter con la conexión
                    m_adpFase = New SCGDataAccess.FaseProduccionDataAdapter

                    'Inicializa el DataAdapter con la conexión
                    m_dstFase = New FaseProduccionDataset

                    Call m_adpFase.Fill(m_dstFase, intCodCentroCosto)

                    With m_dstFase.SCGTA_TB_FasesProduccion.DefaultView
                        .AllowDelete = True
                        .AllowEdit = True
                        .AllowNew = True
                    End With

                    'Carga el datagrid con el dataset en memoria
                    dtgFases.DataSource = m_dstFase.SCGTA_TB_FasesProduccion

                    'objUtilitarios.CargarCombos(cboCentroCosto, 2)
                End If
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try



        End Sub

        'Private Sub estiloGrid()

        '    'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.

        '    'Declaraciones generales
        '    Dim tsConfiguracion As New DataGridTableStyle

        '    dtgFases.TableStyles.Clear()

        '    Dim tcNoCentroCosto As New DataGridTextBoxColumn
        '    Dim tcNoFase As New DataGridTextBoxColumn
        '    Dim tcDescripcion As New DataGridTextBoxColumn
        '    Dim tcEstadoLogico As New DataGridTextBoxColumn
        '    Dim tcUnidad As New DataGridTextBoxColumn

        '    tsConfiguracion.MappingName = m_dstFase.SCGTA_TB_FasesProduccion.TableName


        '    'Carga la columna codigo con las propiedades
        '    With tcNoFase
        '        .Width = 0
        '        .HeaderText = My.Resources.ResourceUI.NoFase
        '        .MappingName = m_dstFase.SCGTA_TB_FasesProduccion.Columns(mc_intNoFase).ColumnName
        '        .Format = "###"
        '        .ReadOnly = True
        '    End With

        '    'Carga la columna descripcion con las propiedades
        '    With tcDescripcion
        '        .Width = 190
        '        .HeaderText = My.Resources.ResourceUI.Fase
        '        .MappingName = m_dstFase.SCGTA_TB_FasesProduccion.Columns(mc_strDescripcion).ColumnName
        '        .ReadOnly = True
        '    End With

        '    With tcNoCentroCosto
        '        .Width = 0
        '        .HeaderText = My.Resources.ResourceUI.CentroCosto
        '        .MappingName = m_dstFase.SCGTA_TB_FasesProduccion.Columns(mc_intNoCentroCosto).ColumnName
        '        .Format = "###"
        '        .ReadOnly = True
        '    End With


        '    'Carga la columna descripcion con las propiedades
        '    With tcEstadoLogico
        '        .Width = 0
        '        .HeaderText = My.Resources.ResourceUI.EstadoLogico
        '        .MappingName = m_dstFase.SCGTA_TB_FasesProduccion.Columns(mc_strEstadoLogico).ColumnName
        '        .ReadOnly = True
        '    End With

        '    With tcUnidad
        '        .Width = 50
        '        .HeaderText = My.Resources.ResourceUI.Unidad
        '        .MappingName = m_dstFase.SCGTA_TB_FasesProduccion.Columns(mc_strDescUnidad).ColumnName
        '        .ReadOnly = True
        '    End With

        '    'Agrega las columnas al tableStyle

        '    tsConfiguracion.GridColumnStyles.Add(tcNoFase)
        '    tsConfiguracion.GridColumnStyles.Add(tcDescripcion)
        '    tsConfiguracion.GridColumnStyles.Add(tcNoCentroCosto)
        '    tsConfiguracion.GridColumnStyles.Add(tcEstadoLogico)
        '    tsConfiguracion.GridColumnStyles.Add(tcUnidad)


        '    'Establece propiedades del datagrid (colores estándares).
        '    tsConfiguracion.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
        '    tsConfiguracion.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
        '    tsConfiguracion.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
        '    tsConfiguracion.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))


        '    'Hace que el datagrid adopte las propiedades del TableStyle.
        '    dtgFases.TableStyles.Add(tsConfiguracion)
        '    dtgFases.ReadOnly = True


        'End Sub

        Public Function Busca_Codigo_Texto(ByVal strTempItem As String, Optional ByVal blnGetCodigo As Boolean = True) As String

            '------------------------------------------------ Documentación SCG -----------------------------------------------------------
            '-- Busca el texto en el string enviado....si usas true busca el de la derecha y si usas falses busca el de la izquierda
            '------------------------------------------------------------------------------------------------------------------------------------

            Dim strCod_Item_Comp As String = ""
            Dim strTemp As String = ""
            Dim intCharCont As Integer
            Dim strTextoNoCodigo As String = ""

            strTemp = ""
            strCod_Item_Comp = ""

            If strTempItem <> "" Then
                For intCharCont = strTempItem.Length - 1 To 0 Step -1
                    If Char.IsWhiteSpace(strTempItem.Chars(intCharCont)) Then
                        Exit For
                    End If
                    strTemp = strTemp & strTempItem.Chars(intCharCont)
                Next
                If strTempItem.Length > 0 Then
                    strTextoNoCodigo = strTempItem.Substring(0, strTempItem.Length - (strTempItem.Length - intCharCont)).Trim
                End If
                For intCharCont = strTemp.Length - 1 To 0 Step -1
                    strCod_Item_Comp = strCod_Item_Comp & strTemp.Chars(intCharCont)
                Next

                If blnGetCodigo Then
                    Return strCod_Item_Comp
                Else
                    Return strTextoNoCodigo
                End If
            Else
                Return ""
            End If

        End Function

        Public Sub Busca_Item_Combo(ByRef Combo As ComboBox, ByVal Cod_Item As String)

            Dim intItemCont As Integer
            Dim strTempItem As String
            Dim strCod_Item_Comp As String
            Dim blnExiste As Boolean

            With Combo

                If .Items.Count <> 0 Then
                    blnExiste = False
                    For intItemCont = 0 To .Items.Count - 1
                        strTempItem = .Items(intItemCont)
                        strCod_Item_Comp = Busca_Codigo_Texto(strTempItem)
                        If Cod_Item = strCod_Item_Comp Then
                            blnExiste = True
                            Exit For
                        End If
                    Next
                    If blnExiste Then
                        .Text = .Items(intItemCont)
                    End If
                End If

            End With

        End Sub

        Private Sub guardar()

            Try
                If cboCentroCosto.Text <> vbNullString Then

                    If intTipoInsercion = 1 Then 'Es una nueva fase de producción.

                        'Dim i As String

                        'Dim n As Integer


                        '-- Crea un objeto Datarow del objeto Dataset Fase
                        Dim drwFase As FaseProduccionDataset.SCGTA_TB_FasesProduccionRow


                        'Se valida que si no existen valores en la base de datos ponga n en 1 o sino se cae al ingresar el primer row.
                        'If m_dstFase.SCGTA_TB_FasesProduccion.Rows.Count = 0 Then

                        '    n = 1

                        'Else

                        '    n = m_dstFase.SCGTA_TB_FasesProduccion.Rows.Count

                        '    i = dtgFases.Item(n - 1, 0)

                        '    n = CInt(i) + 1

                        'End If

                        Dim intCodCentroCosto As Integer


                        '-- Se declara un nuevo row
                        drwFase = m_dstFase.SCGTA_TB_FasesProduccion.NewRow()


                        '-- Carga el row con los datos adecuados.
                        'drwFase.NoFase = n
                        drwFase.Descripcion = txtfase.Text
                        drwFase.CodCentroCosto = CInt(Busca_Codigo_Texto(cboCentroCosto.SelectedItem, True))
                        intCodCentroCosto = CInt(Busca_Codigo_Texto(cboCentroCosto.SelectedItem, True))
                        drwFase.EstadoLogico = 1
                        If rbtnUnidadCantidad.Checked Then
                            drwFase.Unidad = 1
                        Else
                            drwFase.Unidad = 2
                        End If


                        '-- Inserta el row en el Dataset 
                        m_dstFase.SCGTA_TB_FasesProduccion.AddSCGTA_TB_FasesProduccionRow(drwFase)

                        'Actualiza la base de datos todos los cambios hechos en el el dataset.
                        m_adpFase.Update(m_dstFase)

                        cargar(intCodCentroCosto)

                        txtfase.Clear()

                        'Al presionar el botón de nuevo se inhabilitan estos botones por lo cual se tienen que volver a habilitar
                        ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = True
                        ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cerrar).Enabled = True
                        ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True

                    ElseIf intTipoInsercion = 2 Then


                        drw.Descripcion = txtfase.Text()
                        drw.CodCentroCosto = Busca_Codigo_Texto(cboCentroCosto.SelectedItem, True)
                        If rbtnUnidadCantidad.Checked Then
                            drw.Unidad = 1
                        Else
                            drw.Unidad = 2
                        End If
                        cboCentroCosto.Enabled = False
                        txtfase.ReadOnly = True

                        'Se modifica en la base de datos mediante los metodos de la capa de negocios.
                        m_adpFase.Update(m_dstFase)

                        m_dstFase.Reset()

                        'Se refresca el grid
                        cargar(CInt(Busca_Codigo_Texto(cboCentroCosto.SelectedItem, True)))


                        txtfase.Clear()


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

        Private Sub dtgFases_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgFases.CellClick
            Dim a As Integer
            Dim index As Integer = dtgFases.CurrentRow.Index
            'Se valida que cuando se selecciona algo exista al menos un row en el dataset.

            Try


                If m_dstFase.SCGTA_TB_FasesProduccion.Rows.Count <> 0 Then

                    cboCentroCosto.Enabled = True
                    txtfase.ReadOnly = False
                    'Se pone la inserción en modo de modificación.
                    intTipoInsercion = 2

                    'Se cargan los codigos en las respectivas variables
                    a = dtgFases.Rows.Item(index).Cells(0).Value

                    'Recibe el combo y el código como string, retorna el nombre del combo colacado en él.
                    Busca_Item_Combo(cboCentroCosto, CStr(dtgFases.Rows.Item(index).Cells(2).Value))  '

                    drw = m_dstFase.SCGTA_TB_FasesProduccion.FindByNoFase(a)

                    If Not IsNothing(drw) Then
                        txtfase.Text = drw.Descripcion
                        If drw.Unidad = 1 Then
                            rbtnUnidadCantidad.Checked = True
                        Else
                            rbtnUnidadMonto.Checked = True
                        End If
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

        Private Sub dtgFases_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtgFases.KeyDown
            If e.KeyCode = Keys.Delete Then
                Eliminar()
            End If
        End Sub

       
    End Class

End Namespace