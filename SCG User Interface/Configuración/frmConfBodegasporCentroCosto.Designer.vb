Namespace SCG_User_Interface

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmConfBodegasporCentroCosto
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfBodegasporCentroCosto))
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Me.GroupBox1 = New System.Windows.Forms.GroupBox
            Me.dtgBodegasConf = New System.Windows.Forms.DataGridView
            Me.IDCentroCostoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewComboBoxColumn
            Me.CCDataset = New DMSOneFramework.CentroCostoDataset
            Me.RepuestosDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewComboBoxColumn
            Me.BodegSBODataset = New DMSOneFramework.BodegasSBODataset
            Me.ServiciosDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewComboBoxColumn
            Me.SuministrosDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewComboBoxColumn
            Me.ServiciosEXDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewComboBoxColumn
            Me.Proceso = New System.Windows.Forms.DataGridViewComboBoxColumn
            Me.ConfBodXCCDataSet = New DMSOneFramework.ConfBodegasXCentroCostoDataSet
            Me.btnAceptar = New System.Windows.Forms.Button
            Me.btnCancelar = New System.Windows.Forms.Button
            Me.GroupBox1.SuspendLayout()
            CType(Me.dtgBodegasConf, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.CCDataset, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.BodegSBODataset, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ConfBodXCCDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'GroupBox1
            '
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            Me.GroupBox1.Controls.Add(Me.dtgBodegasConf)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.TabStop = False
            '
            'dtgBodegasConf
            '
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(240, Byte), Integer))
            Me.dtgBodegasConf.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
            resources.ApplyResources(Me.dtgBodegasConf, "dtgBodegasConf")
            Me.dtgBodegasConf.AutoGenerateColumns = False
            Me.dtgBodegasConf.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dtgBodegasConf.BackgroundColor = System.Drawing.Color.White
            DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
            DataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dtgBodegasConf.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
            Me.dtgBodegasConf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgBodegasConf.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDCentroCostoDataGridViewTextBoxColumn, Me.RepuestosDataGridViewTextBoxColumn, Me.ServiciosDataGridViewTextBoxColumn, Me.SuministrosDataGridViewTextBoxColumn, Me.ServiciosEXDataGridViewTextBoxColumn, Me.Proceso})
            Me.dtgBodegasConf.DataMember = "SCGTA_SP_SelConfBodegasXCentroCosto"
            Me.dtgBodegasConf.DataSource = Me.ConfBodXCCDataSet
            DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
            DataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(44, Byte), Integer))
            DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dtgBodegasConf.DefaultCellStyle = DataGridViewCellStyle3
            Me.dtgBodegasConf.GridColor = System.Drawing.Color.Silver
            Me.dtgBodegasConf.Name = "dtgBodegasConf"
            DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
            DataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dtgBodegasConf.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
            Me.dtgBodegasConf.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            '
            'IDCentroCostoDataGridViewTextBoxColumn
            '
            Me.IDCentroCostoDataGridViewTextBoxColumn.DataPropertyName = "IDCentroCosto"
            Me.IDCentroCostoDataGridViewTextBoxColumn.DataSource = Me.CCDataset
            Me.IDCentroCostoDataGridViewTextBoxColumn.DisplayMember = "SCGTA_TB_CentroCosto.Descripcion"
            Me.IDCentroCostoDataGridViewTextBoxColumn.DisplayStyleForCurrentCellOnly = True
            Me.IDCentroCostoDataGridViewTextBoxColumn.FillWeight = 121.8274!
            resources.ApplyResources(Me.IDCentroCostoDataGridViewTextBoxColumn, "IDCentroCostoDataGridViewTextBoxColumn")
            Me.IDCentroCostoDataGridViewTextBoxColumn.Name = "IDCentroCostoDataGridViewTextBoxColumn"
            Me.IDCentroCostoDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
            Me.IDCentroCostoDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
            Me.IDCentroCostoDataGridViewTextBoxColumn.ValueMember = "SCGTA_TB_CentroCosto.CodCentroCosto"
            '
            'CCDataset
            '
            Me.CCDataset.DataSetName = "CentroCostoDataset"
            Me.CCDataset.Locale = New System.Globalization.CultureInfo("en-US")
            Me.CCDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'RepuestosDataGridViewTextBoxColumn
            '
            Me.RepuestosDataGridViewTextBoxColumn.DataPropertyName = "Repuestos"
            Me.RepuestosDataGridViewTextBoxColumn.DataSource = Me.BodegSBODataset
            Me.RepuestosDataGridViewTextBoxColumn.DisplayMember = "SCGTA_VW_Bodegas.WhsName"
            Me.RepuestosDataGridViewTextBoxColumn.DisplayStyleForCurrentCellOnly = True
            Me.RepuestosDataGridViewTextBoxColumn.FillWeight = 76.40144!
            resources.ApplyResources(Me.RepuestosDataGridViewTextBoxColumn, "RepuestosDataGridViewTextBoxColumn")
            Me.RepuestosDataGridViewTextBoxColumn.Name = "RepuestosDataGridViewTextBoxColumn"
            Me.RepuestosDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
            Me.RepuestosDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
            Me.RepuestosDataGridViewTextBoxColumn.ValueMember = "SCGTA_VW_Bodegas.WhsCode"
            '
            'BodegSBODataset
            '
            Me.BodegSBODataset.DataSetName = "BodegasSBODataset"
            Me.BodegSBODataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'ServiciosDataGridViewTextBoxColumn
            '
            Me.ServiciosDataGridViewTextBoxColumn.DataPropertyName = "Servicios"
            Me.ServiciosDataGridViewTextBoxColumn.DataSource = Me.BodegSBODataset
            Me.ServiciosDataGridViewTextBoxColumn.DisplayMember = "SCGTA_VW_Bodegas.WhsName"
            Me.ServiciosDataGridViewTextBoxColumn.DisplayStyleForCurrentCellOnly = True
            Me.ServiciosDataGridViewTextBoxColumn.FillWeight = 76.40144!
            resources.ApplyResources(Me.ServiciosDataGridViewTextBoxColumn, "ServiciosDataGridViewTextBoxColumn")
            Me.ServiciosDataGridViewTextBoxColumn.Name = "ServiciosDataGridViewTextBoxColumn"
            Me.ServiciosDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
            Me.ServiciosDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
            Me.ServiciosDataGridViewTextBoxColumn.ValueMember = "SCGTA_VW_Bodegas.WhsCode"
            '
            'SuministrosDataGridViewTextBoxColumn
            '
            Me.SuministrosDataGridViewTextBoxColumn.DataPropertyName = "Suministros"
            Me.SuministrosDataGridViewTextBoxColumn.DataSource = Me.BodegSBODataset
            Me.SuministrosDataGridViewTextBoxColumn.DisplayMember = "SCGTA_VW_Bodegas.WhsName"
            Me.SuministrosDataGridViewTextBoxColumn.DisplayStyleForCurrentCellOnly = True
            Me.SuministrosDataGridViewTextBoxColumn.FillWeight = 76.40144!
            resources.ApplyResources(Me.SuministrosDataGridViewTextBoxColumn, "SuministrosDataGridViewTextBoxColumn")
            Me.SuministrosDataGridViewTextBoxColumn.Name = "SuministrosDataGridViewTextBoxColumn"
            Me.SuministrosDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
            Me.SuministrosDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
            Me.SuministrosDataGridViewTextBoxColumn.ValueMember = "SCGTA_VW_Bodegas.WhsCode"
            '
            'ServiciosEXDataGridViewTextBoxColumn
            '
            Me.ServiciosEXDataGridViewTextBoxColumn.DataPropertyName = "ServiciosEX"
            Me.ServiciosEXDataGridViewTextBoxColumn.DataSource = Me.BodegSBODataset
            Me.ServiciosEXDataGridViewTextBoxColumn.DisplayMember = "SCGTA_VW_Bodegas.WhsName"
            Me.ServiciosEXDataGridViewTextBoxColumn.DisplayStyleForCurrentCellOnly = True
            Me.ServiciosEXDataGridViewTextBoxColumn.FillWeight = 126.6793!
            resources.ApplyResources(Me.ServiciosEXDataGridViewTextBoxColumn, "ServiciosEXDataGridViewTextBoxColumn")
            Me.ServiciosEXDataGridViewTextBoxColumn.Name = "ServiciosEXDataGridViewTextBoxColumn"
            Me.ServiciosEXDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
            Me.ServiciosEXDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
            Me.ServiciosEXDataGridViewTextBoxColumn.ValueMember = "SCGTA_VW_Bodegas.WhsCode"
            '
            'Proceso
            '
            Me.Proceso.DataPropertyName = "Proceso"
            Me.Proceso.DataSource = Me.BodegSBODataset
            Me.Proceso.DisplayMember = "SCGTA_VW_Bodegas.WhsName"
            Me.Proceso.DisplayStyleForCurrentCellOnly = True
            Me.Proceso.FillWeight = 122.2889!
            resources.ApplyResources(Me.Proceso, "Proceso")
            Me.Proceso.Name = "Proceso"
            Me.Proceso.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
            Me.Proceso.ValueMember = "SCGTA_VW_Bodegas.WhsCode"
            '
            'ConfBodXCCDataSet
            '
            Me.ConfBodXCCDataSet.DataSetName = "ConfBodegasXCentroCostoDataSet"
            Me.ConfBodXCCDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'btnAceptar
            '
            resources.ApplyResources(Me.btnAceptar, "btnAceptar")
            Me.btnAceptar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnAceptar.ForeColor = System.Drawing.Color.Black
            Me.btnAceptar.Name = "btnAceptar"
            '
            'btnCancelar
            '
            resources.ApplyResources(Me.btnCancelar, "btnCancelar")
            Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancelar.ForeColor = System.Drawing.Color.Black
            Me.btnCancelar.Name = "btnCancelar"
            '
            'frmConfBodegasporCentroCosto
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.btnCancelar)
            Me.Controls.Add(Me.btnAceptar)
            Me.Controls.Add(Me.GroupBox1)
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Name = "frmConfBodegasporCentroCosto"
            Me.GroupBox1.ResumeLayout(False)
            CType(Me.dtgBodegasConf, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.CCDataset, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.BodegSBODataset, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ConfBodXCCDataSet, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents dtgBodegasConf As System.Windows.Forms.DataGridView
        Friend WithEvents btnAceptar As System.Windows.Forms.Button
        Friend WithEvents ConfBodXCCDataSet As DMSOneFramework.ConfBodegasXCentroCostoDataSet
        Friend WithEvents CCDataset As DMSOneFramework.CentroCostoDataset
        Friend WithEvents BodegSBODataset As DMSOneFramework.BodegasSBODataset
        Friend WithEvents btnCancelar As System.Windows.Forms.Button
        Friend WithEvents IDCentroCostoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewComboBoxColumn
        Friend WithEvents RepuestosDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewComboBoxColumn
        Friend WithEvents ServiciosDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewComboBoxColumn
        Friend WithEvents SuministrosDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewComboBoxColumn
        Friend WithEvents ServiciosEXDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewComboBoxColumn
        Friend WithEvents Proceso As System.Windows.Forms.DataGridViewComboBoxColumn
    End Class

End Namespace
