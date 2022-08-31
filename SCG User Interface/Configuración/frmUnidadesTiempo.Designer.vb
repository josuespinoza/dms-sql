Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmUnidadesTiempo
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUnidadesTiempo))
            Me.txtDescripcion = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.lblEquivalenciaMinutos = New System.Windows.Forms.Label
            Me.lblNombreUnidad = New System.Windows.Forms.Label
            Me.tlbUnidadesTiempo = New Proyecto_SCGToolBar.SCGToolBar
            Me.txtTiempo = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.dtgUnidadesTiempo = New System.Windows.Forms.DataGridView
            Me.CodigoUnidadTiempoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescripcionUnidadTiempoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.TiempoMinutosUnidadTiempoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.dstUnidadesTiempoDataSet = New DMSONEDKFramework.UnidadTiempoDataSet
            Me.Label16 = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            CType(Me.dtgUnidadesTiempo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dstUnidadesTiempoDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'txtDescripcion
            '
            Me.txtDescripcion.AceptaNegativos = False
            Me.txtDescripcion.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtDescripcion.EstiloSBO = True
            resources.ApplyResources(Me.txtDescripcion, "txtDescripcion")
            Me.txtDescripcion.MaxDecimales = 0
            Me.txtDescripcion.MaxEnteros = 0
            Me.txtDescripcion.Millares = False
            Me.txtDescripcion.Name = "txtDescripcion"
            Me.txtDescripcion.ReadOnly = True
            Me.txtDescripcion.Size_AdjustableHeight = 20
            Me.txtDescripcion.TeclasDeshacer = True
            Me.txtDescripcion.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AlfaNumeric
            '
            'lblEquivalenciaMinutos
            '
            resources.ApplyResources(Me.lblEquivalenciaMinutos, "lblEquivalenciaMinutos")
            Me.lblEquivalenciaMinutos.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblEquivalenciaMinutos.Name = "lblEquivalenciaMinutos"
            '
            'lblNombreUnidad
            '
            resources.ApplyResources(Me.lblNombreUnidad, "lblNombreUnidad")
            Me.lblNombreUnidad.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNombreUnidad.Name = "lblNombreUnidad"
            '
            'tlbUnidadesTiempo
            '
            resources.ApplyResources(Me.tlbUnidadesTiempo, "tlbUnidadesTiempo")
            Me.tlbUnidadesTiempo.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.tlbUnidadesTiempo.Name = "tlbUnidadesTiempo"
            '
            'txtTiempo
            '
            Me.txtTiempo.AceptaNegativos = False
            Me.txtTiempo.BackColor = System.Drawing.Color.White
            Me.txtTiempo.EstiloSBO = True
            resources.ApplyResources(Me.txtTiempo, "txtTiempo")
            Me.txtTiempo.MaxDecimales = 0
            Me.txtTiempo.MaxEnteros = 0
            Me.txtTiempo.Millares = False
            Me.txtTiempo.Name = "txtTiempo"
            Me.txtTiempo.ReadOnly = True
            Me.txtTiempo.Size_AdjustableHeight = 20
            Me.txtTiempo.TeclasDeshacer = True
            Me.txtTiempo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.NumDecimal
            '
            'dtgUnidadesTiempo
            '
            Me.dtgUnidadesTiempo.AutoGenerateColumns = False
            Me.dtgUnidadesTiempo.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgUnidadesTiempo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgUnidadesTiempo.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CodigoUnidadTiempoDataGridViewTextBoxColumn, Me.DescripcionUnidadTiempoDataGridViewTextBoxColumn, Me.TiempoMinutosUnidadTiempoDataGridViewTextBoxColumn})
            Me.dtgUnidadesTiempo.DataMember = "SCGTA_TB_UnidadTiempo"
            Me.dtgUnidadesTiempo.DataSource = Me.dstUnidadesTiempoDataSet
            resources.ApplyResources(Me.dtgUnidadesTiempo, "dtgUnidadesTiempo")
            Me.dtgUnidadesTiempo.Name = "dtgUnidadesTiempo"
            '
            'CodigoUnidadTiempoDataGridViewTextBoxColumn
            '
            Me.CodigoUnidadTiempoDataGridViewTextBoxColumn.DataPropertyName = "CodigoUnidadTiempo"
            resources.ApplyResources(Me.CodigoUnidadTiempoDataGridViewTextBoxColumn, "CodigoUnidadTiempoDataGridViewTextBoxColumn")
            Me.CodigoUnidadTiempoDataGridViewTextBoxColumn.Name = "CodigoUnidadTiempoDataGridViewTextBoxColumn"
            '
            'DescripcionUnidadTiempoDataGridViewTextBoxColumn
            '
            Me.DescripcionUnidadTiempoDataGridViewTextBoxColumn.DataPropertyName = "DescripcionUnidadTiempo"
            resources.ApplyResources(Me.DescripcionUnidadTiempoDataGridViewTextBoxColumn, "DescripcionUnidadTiempoDataGridViewTextBoxColumn")
            Me.DescripcionUnidadTiempoDataGridViewTextBoxColumn.Name = "DescripcionUnidadTiempoDataGridViewTextBoxColumn"
            Me.DescripcionUnidadTiempoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'TiempoMinutosUnidadTiempoDataGridViewTextBoxColumn
            '
            Me.TiempoMinutosUnidadTiempoDataGridViewTextBoxColumn.DataPropertyName = "TiempoMinutosUnidadTiempo"
            resources.ApplyResources(Me.TiempoMinutosUnidadTiempoDataGridViewTextBoxColumn, "TiempoMinutosUnidadTiempoDataGridViewTextBoxColumn")
            Me.TiempoMinutosUnidadTiempoDataGridViewTextBoxColumn.Name = "TiempoMinutosUnidadTiempoDataGridViewTextBoxColumn"
            Me.TiempoMinutosUnidadTiempoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'dstUnidadesTiempoDataSet
            '
            Me.dstUnidadesTiempoDataSet.DataSetName = "UnidadTiempoDataSet"
            Me.dstUnidadesTiempoDataSet.Namespace = "http://www.tempuri.org/UnidadTiempo"
            '
            'Label16
            '
            Me.Label16.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label16, "Label16")
            Me.Label16.Name = "Label16"
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'frmUnidadesTiempo
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.Label16)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.dtgUnidadesTiempo)
            Me.Controls.Add(Me.txtTiempo)
            Me.Controls.Add(Me.lblEquivalenciaMinutos)
            Me.Controls.Add(Me.lblNombreUnidad)
            Me.Controls.Add(Me.txtDescripcion)
            Me.Controls.Add(Me.tlbUnidadesTiempo)
            Me.MaximizeBox = False
            Me.Name = "frmUnidadesTiempo"
            CType(Me.dtgUnidadesTiempo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dstUnidadesTiempoDataSet, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents txtDescripcion As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblEquivalenciaMinutos As System.Windows.Forms.Label
        Friend WithEvents lblNombreUnidad As System.Windows.Forms.Label
        Friend WithEvents tlbUnidadesTiempo As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents txtTiempo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents dtgUnidadesTiempo As System.Windows.Forms.DataGridView
        Private WithEvents dstUnidadesTiempoDataSet As DMSONEDKFramework.UnidadTiempoDataSet
        Friend WithEvents CodigoUnidadTiempoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescripcionUnidadTiempoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents TiempoMinutosUnidadTiempoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Label16 As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
    End Class
End Namespace