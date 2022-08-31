Namespace SCG_User_Interface

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
        Partial Class frmRampas
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
            Dim RampasDataSetGrid As DMSOneFramework.RampasDataSet
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRampas))
            Me.ScgTbRampas = New Proyecto_SCGToolBar.SCGToolBar
            Me.dtgRampas = New System.Windows.Forms.DataGridView
            Me.IDRampaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescripcionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoLogicoDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
            RampasDataSetGrid = New DMSOneFramework.RampasDataSet
            CType(RampasDataSetGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtgRampas, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'RampasDataSetGrid
            '
            RampasDataSetGrid.DataSetName = "RampasDataSet"
            RampasDataSetGrid.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'ScgTbRampas
            '
            resources.ApplyResources(Me.ScgTbRampas, "ScgTbRampas")
            Me.ScgTbRampas.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.ScgTbRampas.Name = "ScgTbRampas"
            '
            'dtgRampas
            '
            Me.dtgRampas.AutoGenerateColumns = False
            Me.dtgRampas.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgRampas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgRampas.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDRampaDataGridViewTextBoxColumn, Me.DescripcionDataGridViewTextBoxColumn, Me.EstadoLogicoDataGridViewCheckBoxColumn})
            Me.dtgRampas.DataMember = "SCGTA_TB_Rampas"
            Me.dtgRampas.DataSource = RampasDataSetGrid
            Me.dtgRampas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
            Me.dtgRampas.GridColor = System.Drawing.Color.Silver
            resources.ApplyResources(Me.dtgRampas, "dtgRampas")
            Me.dtgRampas.Name = "dtgRampas"
            '
            'IDRampaDataGridViewTextBoxColumn
            '
            Me.IDRampaDataGridViewTextBoxColumn.DataPropertyName = "IDRampa"
            resources.ApplyResources(Me.IDRampaDataGridViewTextBoxColumn, "IDRampaDataGridViewTextBoxColumn")
            Me.IDRampaDataGridViewTextBoxColumn.Name = "IDRampaDataGridViewTextBoxColumn"
            Me.IDRampaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DescripcionDataGridViewTextBoxColumn
            '
            Me.DescripcionDataGridViewTextBoxColumn.DataPropertyName = "Descripcion"
            resources.ApplyResources(Me.DescripcionDataGridViewTextBoxColumn, "DescripcionDataGridViewTextBoxColumn")
            Me.DescripcionDataGridViewTextBoxColumn.Name = "DescripcionDataGridViewTextBoxColumn"
            '
            'EstadoLogicoDataGridViewCheckBoxColumn
            '
            Me.EstadoLogicoDataGridViewCheckBoxColumn.DataPropertyName = "EstadoLogico"
            Me.EstadoLogicoDataGridViewCheckBoxColumn.HeaderText = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.EstadoLogicoDataGridViewCheckBoxColumn.Name = "EstadoLogicoDataGridViewCheckBoxColumn"
            resources.ApplyResources(Me.EstadoLogicoDataGridViewCheckBoxColumn, "EstadoLogicoDataGridViewCheckBoxColumn")
            '
            'frmRampas
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.dtgRampas)
            Me.Controls.Add(Me.ScgTbRampas)
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.MaximizeBox = False
            Me.Name = "frmRampas"
            CType(RampasDataSetGrid, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtgRampas, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents ScgTbRampas As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents dtgRampas As System.Windows.Forms.DataGridView
        Friend WithEvents IDRampaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescripcionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoLogicoDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn

    End Class

End Namespace

