Option Strict On
Option Explicit On 

Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmFasesProduccionAdicional
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
        Friend WithEvents dtgFases As System.Windows.Forms.DataGrid
        Friend WithEvents ScgToolBar1 As Proyecto_SCGToolBar.SCGToolBar
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFasesProduccionAdicional))
            Me.dtgFases = New System.Windows.Forms.DataGrid
            Me.ScgToolBar1 = New Proyecto_SCGToolBar.SCGToolBar
            CType(Me.dtgFases, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'dtgFases
            '
            Me.dtgFases.AllowSorting = False
            Me.dtgFases.BackgroundColor = System.Drawing.Color.White
            Me.dtgFases.CaptionVisible = False
            Me.dtgFases.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgFases.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.dtgFases.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.dtgFases.HeaderBackColor = System.Drawing.Color.White
            Me.dtgFases.HeaderForeColor = System.Drawing.SystemColors.ControlText
            Me.dtgFases.Location = New System.Drawing.Point(3, 30)
            Me.dtgFases.Name = "dtgFases"
            Me.dtgFases.SelectionBackColor = System.Drawing.Color.FloralWhite
            Me.dtgFases.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
            Me.dtgFases.Size = New System.Drawing.Size(245, 97)
            Me.dtgFases.TabIndex = 2
            '
            'ScgToolBar1
            '
            Me.ScgToolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
            Me.ScgToolBar1.DropDownArrows = True
            Me.ScgToolBar1.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.ScgToolBar1.Location = New System.Drawing.Point(0, 0)
            Me.ScgToolBar1.Name = "ScgToolBar1"
            Me.ScgToolBar1.ShowToolTips = True
            Me.ScgToolBar1.Size = New System.Drawing.Size(251, 28)
            Me.ScgToolBar1.TabIndex = 4
            '
            'frmFasesProduccionAdicional
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 15.0!)
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.ClientSize = New System.Drawing.Size(251, 135)
            Me.Controls.Add(Me.dtgFases)
            Me.Controls.Add(Me.ScgToolBar1)
            Me.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmFasesProduccionAdicional"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "<SCG> Fases Producción"
            CType(Me.dtgFases, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region "Constructores"

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        Public Sub New(ByVal p_noOrden As String)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()
            'Add any initialization after the InitializeComponent() call
            m_strNoOrden = p_noOrden


        End Sub

#End Region

#Region "Declaraciones"

        Private m_strNoOrden As String

        Private m_dtpFasesProduccion As New SCGDataAccess.FaseProduccionDataAdapter
        Private m_dstFasesProduccion As New FaseProduccionDataset

        Private m_dtpFasesxOrden As New SCGDataAccess.FasesXOrdenDataAdapter
        Private m_dstFasesxOrdenOriginal As New FasesXOrdenDataset
        'Private m_dstFasesxOrdenNuevo As New FasesXOrdenDataset

        'Constantes que guardan el nombre de las columnas de la tabla de Fases por orden
        Private mc_strNoOrden As String = "NoOrden"
        Private mc_intNoFase As String = "NoFase"
        Private mc_decDuracionHorasAprobadas As String = "DuracionHorasAprobadas"
        Private mc_strDescripcion As String = "Descripcion"



#End Region

#Region "Eventos"

        Private Sub ScgToolBar1_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Guardar

            Try


                'actualiza los cambios
                Call m_dtpFasesxOrden.Update2(m_dstFasesxOrdenOriginal)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub ScgToolBar1_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Cerrar
            Try
                Me.Close()
                Dispose()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub frmFasesProduccionAdicional_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                Call m_dtpFasesxOrden.Fill(m_dstFasesxOrdenOriginal, m_strNoOrden)

                With m_dstFasesxOrdenOriginal.SCGTA_TB_FasesxOrden.DefaultView
                    .AllowDelete = False
                    .AllowEdit = True
                    .AllowNew = False
                End With

                estiloGrid()


                dtgFases.DataSource = m_dstFasesxOrdenOriginal.SCGTA_TB_FasesxOrden

                With ScgToolBar1
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cancelar).Visible = False
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Visible = True
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Visible = False
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Buscar).Visible = False
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cerrar).Visible = True
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Visible = False
                End With



                'DELEGA LA FUNCIONALIDAD DEL EVENTO DEL CAMBIO DE UN REGISTRO A LA FUNCION DE VERIFICAR
                'CAMBIOS VALIDOS
                AddHandler m_dstFasesxOrdenOriginal.SCGTA_TB_FasesxOrden.SCGTA_TB_FasesxOrdenRowChanged, _
                AddressOf verificarCambiosValidos


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

#End Region

#Region "Procedimientos"


        '-----------------------------------------------------------------------------------
        ' Nombre: verificarCambiosValidos.
        '
        ' Descripcion: delega del evento SCGTA_TB_FasesxOrdenRowChanged del dataTable
        '              m_dstFasesxOrdenOriginal.SCGTA_TB_FasesxOrden el cual es una variable
        '              local
        ' Logica Especial: controla el flujo verificando si el procedimiento se está llamando
        '                  asi mismo. mediande el if y el Rollback. la accion es rollback sale
        '                  sale de la funcion.
        '
        ' Dorian Alvarado Murillo. 19-04-06
        '------------------------------------------------------------------------------------
        Private Sub verificarCambiosValidos(ByVal sender As Object, _
                                            ByVal e As DMSOneFramework.FasesXOrdenDataset.SCGTA_TB_FasesxOrdenRowChangeEvent)

            Try

                'Si el evento se llama por segunda vez entonces se sale de la funcion 
                'porque solemente se requiere que se ejecute una vez
                If e.Row.RowState = DataRowState.Unchanged Then

                    Exit Sub
                End If

                'verifica que si existe un valor cero en el registro actual
                If e.Row.DuracionHorasAprobadas = 0 Then

                    'rechaza los cambios hechos en el datarow
                    e.Row.RejectChanges()
                    'e.Row.DuracionHorasAprobadas = CDec(e.Row(mc_decDuracionHorasAprobadas, DataRowVersion.Original))

                Else

                    'actualiza los cambios de la celda actual
                    Call m_dtpFasesxOrden.Update2(m_dstFasesxOrdenOriginal)

                End If



            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally

            End Try



        End Sub

        Private Function cargarFasesxOrdenOriginal(ByVal dstFasesxOrdenOriginal As FasesXOrdenDataset, ByVal dstFasesxOrdenNuevo As FasesXOrdenDataset) As FasesXOrdenDataset

            Dim drwFasesxOrdenOriginal As FasesXOrdenDataset.SCGTA_TB_FasesxOrdenRow
            'Dim drwFasesxOrdenNuevo As FasesXOrdenDataset.SCGTA_TB_FasesxOrdenRow
            Dim intNoFase As Integer
            Dim decDuracionAprobadasHrs As Decimal

            Try

                For Each drwFasesxOrdenOriginal In dstFasesxOrdenOriginal.SCGTA_TB_FasesxOrden

                    intNoFase = drwFasesxOrdenOriginal.NoFase

                    If drwFasesxOrdenOriginal.IsDuracionHorasAprobadasNull Then
                        decDuracionAprobadasHrs = 0
                    Else
                        decDuracionAprobadasHrs = drwFasesxOrdenOriginal.DuracionHorasAprobadas
                    End If

                    dstFasesxOrdenNuevo.SCGTA_TB_FasesxOrden.FindByNoFase(intNoFase).DuracionHorasAprobadas = decDuracionAprobadasHrs

                Next drwFasesxOrdenOriginal

                Return dstFasesxOrdenNuevo

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try


        End Function

        Private Function cargarFasesProduccion(ByVal dstFasesProduccion As FaseProduccionDataset, ByVal dstFasesxOrden As FasesXOrdenDataset, ByVal strNoOrden As String) As FasesXOrdenDataset

            Dim drwFasesProduccion As FaseProduccionDataset.SCGTA_TB_FasesProduccionRow
            Dim drwFasesxOrden As FasesXOrdenDataset.SCGTA_TB_FasesxOrdenRow

            Try

                For Each drwFasesProduccion In dstFasesProduccion.SCGTA_TB_FasesProduccion

                    drwFasesxOrden = DirectCast(dstFasesxOrden.SCGTA_TB_FasesxOrden.NewRow,  _
                                                FasesXOrdenDataset.SCGTA_TB_FasesxOrdenRow)

                    drwFasesxOrden.NoOrden = strNoOrden

                    drwFasesxOrden.NoFase = drwFasesProduccion.NoFase

                    drwFasesxOrden.Descripcion = drwFasesProduccion.Descripcion

                    drwFasesxOrden.DuracionHorasAprobadas = 0

                    dstFasesxOrden.SCGTA_TB_FasesxOrden.AddSCGTA_TB_FasesxOrdenRow(drwFasesxOrden)

                Next drwFasesProduccion

                Return dstFasesxOrden

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try

        End Function

        Private Sub estiloGrid()

            'Estilo del grid para el datagrid que cargar las fases.

            Try

                'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.

                'Declaraciones generales
                Dim tsConfiguracion As New DataGridTableStyle

                dtgFases.TableStyles.Clear()

                Dim tcNoFase As New DataGridTextBoxColumn
                Dim tcDescripcion As New DataGridTextBoxColumn
                Dim tcOtorgado As New DataGridTextBoxColumn
                Dim tcNoOrden As New DataGridTextBoxColumn

                tsConfiguracion.MappingName = m_dstFasesxOrdenOriginal.SCGTA_TB_FasesxOrden.TableName()

                With tcNoOrden
                    .Width = 0
                    .HeaderText = ""
                    .MappingName = m_dstFasesxOrdenOriginal.SCGTA_TB_FasesxOrden.Columns(mc_strNoOrden).ColumnName
                End With

                'Carga la columna codigo con las propiedades
                With tcNoFase
                    .Width = 0
                    .HeaderText = ""
                    .MappingName = m_dstFasesxOrdenOriginal.SCGTA_TB_FasesxOrden.Columns(mc_intNoFase).ColumnName
                End With

                'Carga la columna descripcion con las propiedades
                With tcDescripcion
                    .Width = 140
                    .HeaderText = My.Resources.ResourceUI.FasesProduccion
                    .MappingName = m_dstFasesxOrdenOriginal.SCGTA_TB_FasesxOrden.Columns(mc_strDescripcion).ColumnName
                    .ReadOnly = True
                End With

                'Carga la columna estado lógico con las propiedades
                With tcOtorgado
                    .Width = 65
                    .HeaderText = My.Resources.ResourceUI.Otorgado
                    .MappingName = m_dstFasesxOrdenOriginal.SCGTA_TB_FasesxOrden.Columns(mc_decDuracionHorasAprobadas).ColumnName
                    .TextBox.MaxLength = 6
                    '.Format = formatoOtorgado
                End With

                'Agrega las columnas al tableStyle
                tsConfiguracion.GridColumnStyles.Add(tcNoOrden)
                tsConfiguracion.GridColumnStyles.Add(tcNoFase)
                tsConfiguracion.GridColumnStyles.Add(tcDescripcion)
                tsConfiguracion.GridColumnStyles.Add(tcOtorgado)

                'Establece propiedades del datagrid (colores estándares).
                tsConfiguracion.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
                tsConfiguracion.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                tsConfiguracion.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
                tsConfiguracion.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))

                'Hace que el datagrid adopte las propiedades del TableStyle.
                dtgFases.TableStyles.Add(tsConfiguracion)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try
        End Sub

#End Region

    End Class

End Namespace






