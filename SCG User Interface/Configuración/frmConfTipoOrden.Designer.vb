

Namespace SCG_User_Interface

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmConfTipoOrden
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfTipoOrden))
            Me.TipoOrdenDatasetGrid = New DMSOneFramework.TipoOrdenDataset
            Me.dtsCentrosCosto = New DMSOneFramework.CentroCostoDataset
            Me.ScgToolBar1 = New Proyecto_SCGToolBar.SCGToolBar
            Me.txtTipoOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.cboCentroCosto = New SCGComboBox.SCGComboBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.lblCentroCosto = New System.Windows.Forms.Label
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.lblTipo = New System.Windows.Forms.Label
            Me.dtgTipoOrden = New System.Windows.Forms.DataGridView
            Me.CodTipoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescripcionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoLogicoDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
            Me.CodCentroCostoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            CType(Me.TipoOrdenDatasetGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtsCentrosCosto, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtgTipoOrden, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'TipoOrdenDatasetGrid
            '
            Me.TipoOrdenDatasetGrid.DataSetName = "TipoOrdenDataset"
            Me.TipoOrdenDatasetGrid.Locale = New System.Globalization.CultureInfo("en-US")
            Me.TipoOrdenDatasetGrid.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'dtsCentrosCosto
            '
            Me.dtsCentrosCosto.DataSetName = "CentroCostoDataset"
            Me.dtsCentrosCosto.Locale = New System.Globalization.CultureInfo("en-US")
            Me.dtsCentrosCosto.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'ScgToolBar1
            '
            resources.ApplyResources(Me.ScgToolBar1, "ScgToolBar1")
            Me.ScgToolBar1.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.ScgToolBar1.Name = "ScgToolBar1"
            '
            'txtTipoOrden
            '
            Me.txtTipoOrden.AceptaNegativos = False
            Me.txtTipoOrden.BackColor = System.Drawing.Color.White
            Me.txtTipoOrden.EstiloSBO = True
            resources.ApplyResources(Me.txtTipoOrden, "txtTipoOrden")
            Me.txtTipoOrden.MaxDecimales = 0
            Me.txtTipoOrden.MaxEnteros = 0
            Me.txtTipoOrden.Millares = False
            Me.txtTipoOrden.Name = "txtTipoOrden"
            Me.txtTipoOrden.ReadOnly = True
            Me.txtTipoOrden.Size_AdjustableHeight = 20
            Me.txtTipoOrden.TeclasDeshacer = True
            Me.txtTipoOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'cboCentroCosto
            '
            Me.cboCentroCosto.BackColor = System.Drawing.Color.White
            Me.cboCentroCosto.DataSource = Me.dtsCentrosCosto
            Me.cboCentroCosto.DisplayMember = "SCGTA_TB_CentroCosto.Descripcion"
            Me.cboCentroCosto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboCentroCosto.EstiloSBO = True
            resources.ApplyResources(Me.cboCentroCosto, "cboCentroCosto")
            Me.cboCentroCosto.Name = "cboCentroCosto"
            Me.cboCentroCosto.ValueMember = "SCGTA_TB_CentroCosto.CodCentroCosto"
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'lblCentroCosto
            '
            resources.ApplyResources(Me.lblCentroCosto, "lblCentroCosto")
            Me.lblCentroCosto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCentroCosto.Name = "lblCentroCosto"
            '
            'lblLine1
            '
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.Name = "lblLine1"
            '
            'lblTipo
            '
            resources.ApplyResources(Me.lblTipo, "lblTipo")
            Me.lblTipo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblTipo.Name = "lblTipo"
            '
            'dtgTipoOrden
            '
            Me.dtgTipoOrden.AllowUserToAddRows = False
            Me.dtgTipoOrden.AllowUserToDeleteRows = False
            Me.dtgTipoOrden.AutoGenerateColumns = False
            Me.dtgTipoOrden.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgTipoOrden.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgTipoOrden.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CodTipoOrdenDataGridViewTextBoxColumn, Me.DescripcionDataGridViewTextBoxColumn, Me.EstadoLogicoDataGridViewCheckBoxColumn, Me.CodCentroCostoDataGridViewTextBoxColumn})
            Me.dtgTipoOrden.DataMember = "SCGTA_TB_TipoOrden"
            Me.dtgTipoOrden.DataSource = Me.TipoOrdenDatasetGrid
            resources.ApplyResources(Me.dtgTipoOrden, "dtgTipoOrden")
            Me.dtgTipoOrden.Name = "dtgTipoOrden"
            Me.dtgTipoOrden.ReadOnly = True
            '
            'CodTipoOrdenDataGridViewTextBoxColumn
            '
            Me.CodTipoOrdenDataGridViewTextBoxColumn.DataPropertyName = "CodTipoOrden"
            resources.ApplyResources(Me.CodTipoOrdenDataGridViewTextBoxColumn, "CodTipoOrdenDataGridViewTextBoxColumn")
            Me.CodTipoOrdenDataGridViewTextBoxColumn.Name = "CodTipoOrdenDataGridViewTextBoxColumn"
            Me.CodTipoOrdenDataGridViewTextBoxColumn.ReadOnly = True
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
            'CodCentroCostoDataGridViewTextBoxColumn
            '
            Me.CodCentroCostoDataGridViewTextBoxColumn.DataPropertyName = "CodCentroCosto"
            resources.ApplyResources(Me.CodCentroCostoDataGridViewTextBoxColumn, "CodCentroCostoDataGridViewTextBoxColumn")
            Me.CodCentroCostoDataGridViewTextBoxColumn.Name = "CodCentroCostoDataGridViewTextBoxColumn"
            Me.CodCentroCostoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'frmConfTipoOrden
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.dtgTipoOrden)
            Me.Controls.Add(Me.txtTipoOrden)
            Me.Controls.Add(Me.cboCentroCosto)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.lblCentroCosto)
            Me.Controls.Add(Me.lblLine1)
            Me.Controls.Add(Me.lblTipo)
            Me.Controls.Add(Me.ScgToolBar1)
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmConfTipoOrden"
            Me.Tag = "Configuración,1"
            CType(Me.TipoOrdenDatasetGrid, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtsCentrosCosto, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtgTipoOrden, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
'        Friend WithEvents imglst_SCG As System.Windows.Forms.ImageList
        Friend WithEvents ScgToolBar1 As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents dtsCentrosCosto As DMSOneFramework.CentroCostoDataset
        Friend WithEvents TipoOrdenDatasetGrid As DMSOneFramework.TipoOrdenDataset
        Friend WithEvents txtTipoOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents cboCentroCosto As SCGComboBox.SCGComboBox
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lblCentroCosto As System.Windows.Forms.Label
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Friend WithEvents lblTipo As System.Windows.Forms.Label
        Friend WithEvents dtgTipoOrden As System.Windows.Forms.DataGridView
        Friend WithEvents CodTipoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescripcionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoLogicoDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents CodCentroCostoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    End Class
End Namespace