Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Class frmConfRazonesSuspension
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
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents txtReproceso As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents ScgToolBar1 As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents dtgSuspension As System.Windows.Forms.DataGridView
        Friend WithEvents IDRazonDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents RazonDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoLogicoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Public WithEvents lblLine2 As System.Windows.Forms.Label
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim RazonesSuspensionDatasetGrid As DMSOneFramework.RazonesSuspensionDataset
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfRazonesSuspension))
            Me.lblLine2 = New System.Windows.Forms.Label
            Me.Label4 = New System.Windows.Forms.Label
            Me.txtReproceso = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.ScgToolBar1 = New Proyecto_SCGToolBar.SCGToolBar
            Me.dtgSuspension = New System.Windows.Forms.DataGridView
            Me.IDRazonDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.RazonDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoLogicoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            RazonesSuspensionDatasetGrid = New DMSOneFramework.RazonesSuspensionDataset
            CType(RazonesSuspensionDatasetGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtgSuspension, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'RazonesSuspensionDatasetGrid
            '
            RazonesSuspensionDatasetGrid.DataSetName = "RazonesSuspensionDataset"
            RazonesSuspensionDatasetGrid.Locale = New System.Globalization.CultureInfo("en-US")
            RazonesSuspensionDatasetGrid.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'lblLine2
            '
            Me.lblLine2.AccessibleDescription = Nothing
            Me.lblLine2.AccessibleName = Nothing
            resources.ApplyResources(Me.lblLine2, "lblLine2")
            Me.lblLine2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.lblLine2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine2.Name = "lblLine2"
            '
            'Label4
            '
            Me.Label4.AccessibleDescription = Nothing
            Me.Label4.AccessibleName = Nothing
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label4.Name = "Label4"
            '
            'txtReproceso
            '
            Me.txtReproceso.AccessibleDescription = Nothing
            Me.txtReproceso.AccessibleName = Nothing
            Me.txtReproceso.AceptaNegativos = False
            resources.ApplyResources(Me.txtReproceso, "txtReproceso")
            Me.txtReproceso.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtReproceso.BackgroundImage = Nothing
            Me.txtReproceso.EstiloSBO = True
            Me.txtReproceso.MaxDecimales = 0
            Me.txtReproceso.MaxEnteros = 0
            Me.txtReproceso.Millares = False
            Me.txtReproceso.Name = "txtReproceso"
            Me.txtReproceso.ReadOnly = True
            Me.txtReproceso.Size_AdjustableHeight = 20
            Me.txtReproceso.TeclasDeshacer = True
            Me.txtReproceso.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'ScgToolBar1
            '
            Me.ScgToolBar1.AccessibleDescription = Nothing
            Me.ScgToolBar1.AccessibleName = Nothing
            resources.ApplyResources(Me.ScgToolBar1, "ScgToolBar1")
            Me.ScgToolBar1.BackgroundImage = Nothing
            Me.ScgToolBar1.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.ScgToolBar1.Font = Nothing
            Me.ScgToolBar1.Name = "ScgToolBar1"
            '
            'dtgSuspension
            '
            Me.dtgSuspension.AccessibleDescription = Nothing
            Me.dtgSuspension.AccessibleName = Nothing
            Me.dtgSuspension.AllowUserToAddRows = False
            Me.dtgSuspension.AllowUserToDeleteRows = False
            resources.ApplyResources(Me.dtgSuspension, "dtgSuspension")
            Me.dtgSuspension.AutoGenerateColumns = False
            Me.dtgSuspension.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgSuspension.BackgroundImage = Nothing
            Me.dtgSuspension.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgSuspension.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDRazonDataGridViewTextBoxColumn, Me.RazonDataGridViewTextBoxColumn, Me.EstadoLogicoDataGridViewTextBoxColumn})
            Me.dtgSuspension.DataMember = "SCGTA_TB_RazonSuspension"
            Me.dtgSuspension.DataSource = RazonesSuspensionDatasetGrid
            Me.dtgSuspension.Font = Nothing
            Me.dtgSuspension.GridColor = System.Drawing.Color.Silver
            Me.dtgSuspension.Name = "dtgSuspension"
            Me.dtgSuspension.ReadOnly = True
            '
            'IDRazonDataGridViewTextBoxColumn
            '
            Me.IDRazonDataGridViewTextBoxColumn.DataPropertyName = "IDRazon"
            resources.ApplyResources(Me.IDRazonDataGridViewTextBoxColumn, "IDRazonDataGridViewTextBoxColumn")
            Me.IDRazonDataGridViewTextBoxColumn.Name = "IDRazonDataGridViewTextBoxColumn"
            Me.IDRazonDataGridViewTextBoxColumn.ReadOnly = True
            '
            'RazonDataGridViewTextBoxColumn
            '
            Me.RazonDataGridViewTextBoxColumn.DataPropertyName = "Razon"
            resources.ApplyResources(Me.RazonDataGridViewTextBoxColumn, "RazonDataGridViewTextBoxColumn")
            Me.RazonDataGridViewTextBoxColumn.Name = "RazonDataGridViewTextBoxColumn"
            Me.RazonDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EstadoLogicoDataGridViewTextBoxColumn
            '
            Me.EstadoLogicoDataGridViewTextBoxColumn.DataPropertyName = "EstadoLogico"
            resources.ApplyResources(Me.EstadoLogicoDataGridViewTextBoxColumn, "EstadoLogicoDataGridViewTextBoxColumn")
            Me.EstadoLogicoDataGridViewTextBoxColumn.Name = "EstadoLogicoDataGridViewTextBoxColumn"
            Me.EstadoLogicoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'frmConfRazonesSuspension
            '
            Me.AccessibleDescription = Nothing
            Me.AccessibleName = Nothing
            resources.ApplyResources(Me, "$this")
            Me.BackgroundImage = Nothing
            Me.Controls.Add(Me.dtgSuspension)
            Me.Controls.Add(Me.lblLine2)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.txtReproceso)
            Me.Controls.Add(Me.ScgToolBar1)
            Me.Font = Nothing
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmConfRazonesSuspension"
            Me.Tag = "Configuración,1"
            CType(RazonesSuspensionDatasetGrid, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtgSuspension, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region "Constructor"

        'Sub New()

        '    ' This call is required by the Windows Form Designer.
        '    InitializeComponent()


        '    ' Add any initialization after the InitializeComponent() call.

        'End Sub

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

#End Region

#Region "Declaraciones"

        'Friend Event RetornaDatos()
        Private m_adpSusp As SCGDataAccess.RazonesSuspensionDataAdapter
        Private m_dstSusp As RazonesSuspensionDataset

        '-- Constantes que guardan el nombre de las columnas 
        Private mc_intIDRazon As String = "IDRazon"
        Private mc_strDescripcion As String = "Razon"
        Private mc_strEstadoLogico As String = "EstadoLogico"

        '-- Nombre de la constante de la tabla donde se consultan las fases de producción
        Private mc_strTableName As String = "SCGTA_TB_RazonSuspension"

        '-- Se inicializa un objeto tipo Utilitarios que recibe el string de conexión con el objetivo de usarla en funciones como carga combos.
        Private objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        '-- Tipo de inserción que se va a relizar si un update o un insert si el tipo de inserción es 1 es un insert de un nuevo objeto si no es un 2 es un update.
        Private intTipoInsercion As Integer

        Private drwRazonSuspencion As RazonesSuspensionDataset.SCGTA_TB_RazonSuspensionRow

#End Region

#Region "Eventos"

        Private Sub frmConfRazonesSuspension_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            intTipoInsercion = 1

            'Da formato al grid.
            cargar()
            'estiloGrid()


            'Se ocultan los botones del toolbar que no se van utilizar
            ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
            ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False
            ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Buscar).Visible = False
            ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cancelar).Visible = False

            ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = False
            ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False

        End Sub

        Private Sub ScgToolBar1_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Guardar

            guardar()

        End Sub

        Private Sub ScgToolBar1_Click_Eliminar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Eliminar
            Eliminar()
        End Sub

        Private Sub Eliminar()
            Try

                '-- Se cambia la columna estado lógico en 0 para modificar el row - y se le resta un número a la variable i para obtener el verdadero row en el dataset.
                If dtgSuspension.CurrentRow.Index <> -1 Then
                    'm_dstSusp.SCGTA_TB_RazonSuspension.Rows(dtgSuspension.CurrentRow.Index).Delete()
                    m_dstSusp.SCGTA_TB_RazonSuspension.FindByIDRazon(dtgSuspension.Rows.Item(dtgSuspension.CurrentRow.Index).Cells(0).Value).Delete()
                    'Se inicializa la fase de producción
                    'intFaseProduccion = CInt(dtgSuspension.Item(dtgSuspension.CurrentRowIndex, 1))
                    m_adpSusp.Delete(m_dstSusp)

                    'Se limpian los textfields.
                    txtReproceso.Clear()
                End If

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub ScgToolBar1_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Cerrar

            Me.Close()
            m_dstSusp.Dispose()

        End Sub

        Private Sub ScgToolBar1_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Nuevo
            intTipoInsercion = 1
            txtReproceso.Clear()
            txtReproceso.Focus()
            ' cboFasesProd.Enabled = True
            txtReproceso.ReadOnly = False

        End Sub

        Private Sub txtReproceso_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtReproceso.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then

                guardar()

            End If

        End Sub

        Private Sub frmConfRazonesSuspension_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

            If Asc(e.KeyChar) = Keys.Escape Then Me.Close()

        End Sub

        Private Sub dtgSuspension_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtgSuspension.KeyDown

            If e.KeyCode = Keys.Delete Then
                Eliminar()
            End If

        End Sub

#End Region

#Region "Métodos"

        Private Sub cargar()

            Try
                If Not IsNothing(m_dstSusp) Then
                    m_dstSusp.Dispose()
                End If

                m_adpSusp = New SCGDataAccess.RazonesSuspensionDataAdapter

                m_dstSusp = New RazonesSuspensionDataset

                Call m_adpSusp.Fill(m_dstSusp)

                With m_dstSusp.SCGTA_TB_RazonSuspension.DefaultView
                    .AllowDelete = True
                    .AllowEdit = True
                    .AllowNew = True
                End With

                dtgSuspension.DataSource = m_dstSusp.SCGTA_TB_RazonSuspension

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try


        End Sub

        'Private Sub estiloGrid()

        '    'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.

        '    'Declaraciones generales
        '    Dim tsConfiguracion As New DataGridTableStyle

        '    dtgSuspension.TableStyles.Clear()

        '    Dim tcNoActividad As New DataGridTextBoxColumn
        '    Dim tcDescripcion As New DataGridTextBoxColumn
        '    Dim tcEstadoLogico As New DataGridTextBoxColumn
        '    If m_dstSusp Is Nothing Then
        '        m_dstSusp = New RazonesSuspensionDataset
        '    End If
        '    tsConfiguracion.MappingName = m_dstSusp.SCGTA_TB_RazonSuspension.TableName


        '    'Carga la columna codigo con las propiedades
        '    With tcNoActividad
        '        .Width = 0
        '        .HeaderText = My.Resources.ResourceUI.IDRazon
        '        .MappingName = mc_intIDRazon
        '        .Format = "###"
        '        .ReadOnly = True
        '    End With


        '    'Carga la columna descripcion con las propiedades
        '    With tcDescripcion
        '        .Width = 234
        '        .HeaderText = My.Resources.ResourceUI.Razon
        '        .MappingName = mc_strDescripcion
        '        .ReadOnly = True
        '    End With


        '    'Carga la columna descripcion con las propiedades
        '    With tcEstadoLogico
        '        .Width = 0
        '        .HeaderText = My.Resources.ResourceUI.Estado
        '        .MappingName = mc_strEstadoLogico
        '        .ReadOnly = True
        '    End With


        '    'Agrega las columnas al tableStyle
        '    tsConfiguracion.GridColumnStyles.Add(tcNoActividad)
        '    tsConfiguracion.GridColumnStyles.Add(tcDescripcion)
        '    tsConfiguracion.GridColumnStyles.Add(tcEstadoLogico)


        '    'Establece propiedades del datagrid (colores estándares).
        '    tsConfiguracion.SelectionBackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(194, Byte), CType(0, Byte))
        '    tsConfiguracion.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
        '    tsConfiguracion.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
        '    tsConfiguracion.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))


        '    'Hace que el datagrid adopte las propiedades del TableStyle.
        '    dtgSuspension.TableStyles.Add(tsConfiguracion)

        'End Sub

        Private Sub guardar()
            Try
                'Dim nfase As Integer
                If txtReproceso.Text <> "" Then
                    If intTipoInsercion = 1 Then 'Es una nueva suspensión

                        'Dim i As String
                        'Dim n As Integer

                        Dim drwSusp As RazonesSuspensionDataset.SCGTA_TB_RazonSuspensionRow


                        '-- Se declara un nuevo row
                        drwSusp = m_dstSusp.SCGTA_TB_RazonSuspension.NewRow()


                        '-- Carga el row con los datos adecuados.
                        drwSusp.Razon = txtReproceso.Text

                        m_dstSusp.SCGTA_TB_RazonSuspension.AddSCGTA_TB_RazonSuspensionRow(drwSusp)

                        'Actualiza la base de datos todos los cambios hechos en el el dataset.
                        If m_adpSusp Is Nothing Then
                            m_adpSusp = New RazonesSuspensionDataAdapter
                        End If
                        m_adpSusp.Update(m_dstSusp)

                        cargar()
                        txtReproceso.Clear()

                        'Al presionar el botón de nuevo se inhabilitan estos botones por lo cual se tienen que volver a habilitar
                        ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = True
                        ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cerrar).Enabled = True
                        ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True
                        txtReproceso.Focus()

                    ElseIf intTipoInsercion = 2 Then


                        'Se modifica la actividad en el row seleccionado
                        drwRazonSuspencion.Razon = txtReproceso.Text

                        'Se modifica en la base de datos mediante los metodos de la capa de negocios.
                        m_adpSusp.Update(m_dstSusp)

                        'Se refresca el grid
                        cargar()
                        txtReproceso.Clear()

                        txtReproceso.ReadOnly = True

                    End If
                End If
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

#End Region

        Private Sub dtgSuspension_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgSuspension.CellClick
            Try
                Dim intIDRazon As Integer

                'Se valida que cuando se selecciona algo exista al menos un row en el dataset.
                If m_dstSusp.SCGTA_TB_RazonSuspension.Rows.Count <> 0 Then

                    'Se pone la inserción en modo de modificación.
                    intTipoInsercion = 2
                    'cboFasesProd.Enabled = True
                    txtReproceso.ReadOnly = False

                    'Se cargan los codigos en las respectivas variables
                    intIDRazon = dtgSuspension.Rows.Item(dtgSuspension.CurrentRow.Index).Cells(0).Value

                    drwRazonSuspencion = m_dstSusp.SCGTA_TB_RazonSuspension.FindByIDRazon(intIDRazon)

                    txtReproceso.Text = drwRazonSuspencion.Razon

                    'Se habilita tanto la modificación como la eliminación del row.
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = True
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = True


                End If
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub
    End Class

End Namespace
