Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Class frmReprocesos
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
        Friend WithEvents txtDetalle As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents dtgReprocesosxOrden As System.Windows.Forms.DataGrid
        Friend WithEvents lblNoOrden As System.Windows.Forms.Label
        Friend WithEvents scgtbReproceso As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents lblNoFase As System.Windows.Forms.Label
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReprocesos))
            Me.txtDetalle = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.dtgReprocesosxOrden = New System.Windows.Forms.DataGrid
            Me.scgtbReproceso = New Proyecto_SCGToolBar.SCGToolBar
            Me.lblNoOrden = New System.Windows.Forms.Label
            Me.lblNoFase = New System.Windows.Forms.Label
            CType(Me.dtgReprocesosxOrden, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'txtDetalle
            '
            Me.txtDetalle.AceptaNegativos = False
            Me.txtDetalle.BackColor = System.Drawing.Color.White
            Me.txtDetalle.EstiloSBO = True
            Me.txtDetalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.txtDetalle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.txtDetalle.Location = New System.Drawing.Point(3, 213)
            Me.txtDetalle.MaxDecimales = 0
            Me.txtDetalle.MaxEnteros = 0
            Me.txtDetalle.Millares = False
            Me.txtDetalle.Multiline = True
            Me.txtDetalle.Name = "txtDetalle"
            Me.txtDetalle.ReadOnly = True
            Me.txtDetalle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.txtDetalle.Size = New System.Drawing.Size(744, 45)
            Me.txtDetalle.Size_AdjustableHeight = 45
            Me.txtDetalle.TabIndex = 2
            Me.txtDetalle.TeclasDeshacer = True
            Me.txtDetalle.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'dtgReprocesosxOrden
            '
            Me.dtgReprocesosxOrden.BackgroundColor = System.Drawing.Color.White
            Me.dtgReprocesosxOrden.CaptionVisible = False
            Me.dtgReprocesosxOrden.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgReprocesosxOrden.HeaderForeColor = System.Drawing.SystemColors.ControlText
            Me.dtgReprocesosxOrden.Location = New System.Drawing.Point(3, 50)
            Me.dtgReprocesosxOrden.Name = "dtgReprocesosxOrden"
            Me.dtgReprocesosxOrden.ReadOnly = True
            Me.dtgReprocesosxOrden.Size = New System.Drawing.Size(744, 159)
            Me.dtgReprocesosxOrden.TabIndex = 1
            '
            'scgtbReproceso
            '
            Me.scgtbReproceso.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
            Me.scgtbReproceso.DropDownArrows = True
            Me.scgtbReproceso.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.scgtbReproceso.Location = New System.Drawing.Point(0, 0)
            Me.scgtbReproceso.Name = "scgtbReproceso"
            Me.scgtbReproceso.ShowToolTips = True
            Me.scgtbReproceso.Size = New System.Drawing.Size(752, 28)
            Me.scgtbReproceso.TabIndex = 5062
            '
            'lblNoOrden
            '
            Me.lblNoOrden.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblNoOrden.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoOrden.Location = New System.Drawing.Point(3, 31)
            Me.lblNoOrden.Name = "lblNoOrden"
            Me.lblNoOrden.Size = New System.Drawing.Size(164, 16)
            Me.lblNoOrden.TabIndex = 0
            Me.lblNoOrden.Text = "No. Orden:"
            '
            'lblNoFase
            '
            Me.lblNoFase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblNoFase.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoFase.Location = New System.Drawing.Point(215, 31)
            Me.lblNoFase.Name = "lblNoFase"
            Me.lblNoFase.Size = New System.Drawing.Size(320, 16)
            Me.lblNoFase.TabIndex = 5064
            Me.lblNoFase.Text = "Decripción de Fase:"
            '
            'frmReprocesos
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.ClientSize = New System.Drawing.Size(752, 271)
            Me.Controls.Add(Me.lblNoFase)
            Me.Controls.Add(Me.lblNoOrden)
            Me.Controls.Add(Me.dtgReprocesosxOrden)
            Me.Controls.Add(Me.txtDetalle)
            Me.Controls.Add(Me.scgtbReproceso)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.Name = "frmReprocesos"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "<SCG> Reprocesos"
            CType(Me.dtgReprocesosxOrden, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region "Declaraciones"
        Private m_adpReprocesosxOrden As New ReprocesosxOrdenDataAdapter
        Private m_dstReprocesosxOrden As New ReprocesosxOrdenDataset
        Private m_strNoOrden As String
        Private m_intNoFase As Integer
        Private m_strDescripcionFase As String

        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strNoReproceso As String = "NoReproceso"
        Private Const mc_strNoReprocesoxOrden As String = "NoReprocesoxOrden"
        Private Const mc_strFecha As String = "Fecha"
        Private Const mc_strObservacion As String = "Observacion"
        Private Const mc_strNoColaborador As String = "NoColaborador"
        Private Const mc_strTiempoManoObra As String = "TiempoManoObra"
        Private Const mc_strCosto As String = "Costo"
        Private Const mc_strFechaFin As String = "FechaFin"
        Private Const mc_strNombreFase As String = "FasedeProduccion"
        Private Const mc_strRazon As String = "RazondeProceso"
        Private Const mc_strNombreColaborador As String = "Nombre"


#End Region

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        Public Sub New(ByVal NoOrden As String, _
                       ByVal NoFase As Integer, _
                       ByVal Descripcion As String)

            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()
            'Add any initialization after the InitializeComponent() call

            m_strNoOrden = NoOrden
            m_intNoFase = NoFase
            m_strDescripcionFase = Descripcion

        End Sub

#End Region

#Region "Eventos"

        Private Sub frmReprocesos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try
                Call Refrescar()
                With scgtbReproceso
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cancelar).Visible = False
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Visible = False
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Visible = False
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Buscar).Visible = False
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False
                End With

                m_dstReprocesosxOrden.SCGTA_TB_ReprocesosxOrden.DefaultView.AllowDelete = False
                m_dstReprocesosxOrden.SCGTA_TB_ReprocesosxOrden.DefaultView.AllowEdit = False
                m_dstReprocesosxOrden.SCGTA_TB_ReprocesosxOrden.DefaultView.AllowNew = False

                lblNoOrden.Text &= " " & m_strNoOrden
                lblNoFase.Text &= " " & m_strDescripcionFase

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            Finally

            End Try

        End Sub


        Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            ' Dim form As frmCtrlReproceso
            'If IsNothing(form) Then
            '    'form = New frmCtrlReproceso
            '    Me.AddOwnedForm(form)
            'End If
            'form.Show()
        End Sub

        Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Me.Close()
        End Sub


        Private Sub scgtbReproceso_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles scgtbReproceso.Click_Cerrar
            Call Me.Close()
        End Sub



        Private Sub scgtbReproceso_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles scgtbReproceso.Click_Nuevo

            'Dim frmReprocesos As New frmCtrlReproceso(m_strNoOrden, m_intNoFase, m_strDescripcionFase)

            'frmReprocesos.Owner = Me

            'Call frmReprocesos.ShowDialog()

            'With scgtbReproceso
            '    .Buttons(.enumButton.Nuevo).Enabled = True
            '    .Buttons(.enumButton.Imprimir).Enabled = True
            'End With

            'Call Refrescar()

        End Sub
#End Region

#Region "Metodos"
        Public Sub Refrescar()
            Call m_dstReprocesosxOrden.Clear()
            Call m_adpReprocesosxOrden.Fill(m_dstReprocesosxOrden, m_strNoOrden, m_intNoFase, _
                                            G_strCompaniaSCG, gc_strAplicacion)
            dtgReprocesosxOrden.DataSource = m_dstReprocesosxOrden.SCGTA_TB_ReprocesosxOrden
            Call estiloGridReprocesos(dtgReprocesosxOrden)
        End Sub

        Private Sub estiloGridReprocesos(ByRef dtgRequisito As DataGrid)

            'Dim mensaje As String
            'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.
            'Declaraciones generales
            Dim tsReprocesos As New DataGridTableStyle

            Call dtgRequisito.TableStyles.Clear()

            Dim tcNoOrden As New DataGridTextBoxColumn
            Dim tcFasedeProduccion As New DataGridTextBoxColumn
            Dim tcColaborador As New DataGridTextBoxColumn
            Dim tcRazondeProceso As New DataGridTextBoxColumn
            Dim tcFecha As New DataGridTextBoxColumn
            Dim tcNoReprocesoxOrden As New DataGridTextBoxColumn
            Dim tcObservacion As New DataGridTextBoxColumn
            Dim tcNoColaborador As New DataGridTextBoxColumn

            Try

                tsReprocesos.MappingName = m_dstReprocesosxOrden.SCGTA_TB_ReprocesosxOrden.TableName

                With tcNoOrden
                    .Width = 0
                    .HeaderText = "No. Orden"
                    .MappingName = m_dstReprocesosxOrden.SCGTA_TB_ReprocesosxOrden.Columns(mc_strNoOrden).ColumnName

                End With

                With tcFasedeProduccion
                    .Width = 150
                    .HeaderText = "Fase de Producción"
                    .MappingName = m_dstReprocesosxOrden.SCGTA_TB_ReprocesosxOrden.Columns(mc_strNombreFase).ColumnName

                End With

                With tcColaborador
                    .Width = 150
                    .HeaderText = "Colaborador"
                    .MappingName = m_dstReprocesosxOrden.SCGTA_TB_ReprocesosxOrden.Columns(mc_strNombreColaborador).ColumnName
                End With

                With tcRazondeProceso
                    .Width = 240
                    .HeaderText = "Razón de Reproceso"
                    .MappingName = m_dstReprocesosxOrden.SCGTA_TB_ReprocesosxOrden.Columns(mc_strRazon).ColumnName
                End With

                With tcFecha
                    .Width = 148
                    .HeaderText = "Fecha"
                    .MappingName = m_dstReprocesosxOrden.SCGTA_TB_ReprocesosxOrden.Columns(mc_strFecha).ColumnName
                End With

                'With tcNoReprocesoxOrden
                '    .Width = 75
                '    .HeaderText = "NoReprocesoxOrden"
                '    .MappingName = m_dstReprocesosxOrden.SCGTA_TB_ReprocesosxOrden.Columns(mc_strNoReprocesoxOrden).ColumnName
                'End With

                With tcObservacion
                    .Width = 0
                    .HeaderText = "Observacion"
                    .MappingName = m_dstReprocesosxOrden.SCGTA_TB_ReprocesosxOrden.Columns(mc_strObservacion).ColumnName
                End With

                'With tcNoColaborador
                '    .Width = 0
                '    .HeaderText = "Nombre"
                '    .MappingName = m_dstReprocesosxOrden.SCGTA_TB_ReprocesosxOrden.Columns(mc_strNoColaborador).ColumnName
                'End With

                'With tcRazondeProceso
                '    .Width = 0
                '    .HeaderText = "Razón de proceso"
                '    .MappingName = m_dstReprocesosxOrden.SCGTA_TB_ReprocesosxOrden.Columns(mc_strRazon).ColumnName
                'End With

                'Agrega las columnas al tableStyle
                ' tsReprocesos.GridColumnStyles.Add(tcNoReprocesoxOrden)
                tsReprocesos.GridColumnStyles.Add(tcNoOrden)
                tsReprocesos.GridColumnStyles.Add(tcFasedeProduccion)
                tsReprocesos.GridColumnStyles.Add(tcColaborador)
                tsReprocesos.GridColumnStyles.Add(tcRazondeProceso)
                tsReprocesos.GridColumnStyles.Add(tcFecha)
                tsReprocesos.GridColumnStyles.Add(tcObservacion)
                'tsReprocesos.GridColumnStyles.Add(tcNoColaborador)


                'Establece propiedades del datagrid (colores estándares).
                tsReprocesos.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
                tsReprocesos.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                tsReprocesos.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
                tsReprocesos.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))

                'Hace que el datagrid adopte las propiedades del TableStyle.

                dtgRequisito.TableStyles.Add(tsReprocesos)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'objSCGMSGBox.msgExclamationCustom(ex.Message)
            End Try

        End Sub
#End Region

        Private Sub dtgReprocesosxOrden_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgReprocesosxOrden.CurrentCellChanged
            Try
                If dtgReprocesosxOrden.VisibleRowCount >= 1 Then

                    txtDetalle.Text = dtgReprocesosxOrden.Item(dtgReprocesosxOrden.CurrentCell.RowNumber, 5)

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            Finally
            End Try
        End Sub
    End Class
End Namespace
