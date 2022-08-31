Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmAsignacionTiempos
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAsignacionTiempos))
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Me.cboFases = New SCGComboBox.SCGComboBox
            Me.btnAgregar = New System.Windows.Forms.Button
            Me.btnCancelar = New System.Windows.Forms.Button
            Me.Label47 = New System.Windows.Forms.Label
            Me.Label4 = New System.Windows.Forms.Label
            Me.dtgActividades = New System.Windows.Forms.DataGridView
            Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EmpIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaInicioDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaFinDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ReprocesoDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
            Me.CostoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.TiempoHorasDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoFaseDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ReferenciaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EmpNombreDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CheckDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
            Me.IndicadorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoRazonDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.RazonDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ProcesoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IDActividadDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ActividadDescDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.TotalUnidadTiempoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.dstActividades = New DMSOneFramework.ColaboradorDataset
            CType(Me.dtgActividades, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dstActividades, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'cboFases
            '
            Me.cboFases.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboFases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboFases.EstiloSBO = True
            resources.ApplyResources(Me.cboFases, "cboFases")
            Me.cboFases.Name = "cboFases"
            '
            'btnAgregar
            '
            resources.ApplyResources(Me.btnAgregar, "btnAgregar")
            Me.btnAgregar.Name = "btnAgregar"
            Me.btnAgregar.UseVisualStyleBackColor = True
            '
            'btnCancelar
            '
            resources.ApplyResources(Me.btnCancelar, "btnCancelar")
            Me.btnCancelar.Name = "btnCancelar"
            Me.btnCancelar.UseVisualStyleBackColor = True
            '
            'Label47
            '
            resources.ApplyResources(Me.Label47, "Label47")
            Me.Label47.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label47.Name = "Label47"
            '
            'Label4
            '
            Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Name = "Label4"
            '
            'dtgActividades
            '
            Me.dtgActividades.AllowUserToAddRows = False
            Me.dtgActividades.AllowUserToDeleteRows = False
            Me.dtgActividades.AllowUserToResizeRows = False
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            Me.dtgActividades.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
            Me.dtgActividades.AutoGenerateColumns = False
            Me.dtgActividades.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtgActividades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dtgActividades.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDDataGridViewTextBoxColumn, Me.EmpIDDataGridViewTextBoxColumn, Me.FechaInicioDataGridViewTextBoxColumn, Me.FechaFinDataGridViewTextBoxColumn, Me.ReprocesoDataGridViewCheckBoxColumn, Me.CostoDataGridViewTextBoxColumn, Me.TiempoHorasDataGridViewTextBoxColumn, Me.NoOrdenDataGridViewTextBoxColumn, Me.NoFaseDataGridViewTextBoxColumn, Me.EstadoDataGridViewTextBoxColumn, Me.ReferenciaDataGridViewTextBoxColumn, Me.EmpNombreDataGridViewTextBoxColumn, Me.CheckDataGridViewCheckBoxColumn, Me.IndicadorDataGridViewTextBoxColumn, Me.NoRazonDataGridViewTextBoxColumn, Me.RazonDataGridViewTextBoxColumn, Me.ProcesoDataGridViewTextBoxColumn, Me.IDActividadDataGridViewTextBoxColumn, Me.ActividadDescDataGridViewTextBoxColumn, Me.TotalUnidadTiempoDataGridViewTextBoxColumn})
            Me.dtgActividades.DataMember = "SCGTA_TB_ControlColaborador"
            Me.dtgActividades.DataSource = Me.dstActividades
            resources.ApplyResources(Me.dtgActividades, "dtgActividades")
            Me.dtgActividades.Name = "dtgActividades"
            DataGridViewCellStyle2.BackColor = System.Drawing.Color.White
            DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
            DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White
            DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
            Me.dtgActividades.RowsDefaultCellStyle = DataGridViewCellStyle2
            Me.dtgActividades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
            Me.dtgActividades.ShowCellErrors = False
            Me.dtgActividades.ShowCellToolTips = False
            Me.dtgActividades.ShowEditingIcon = False
            Me.dtgActividades.ShowRowErrors = False
            '
            'IDDataGridViewTextBoxColumn
            '
            Me.IDDataGridViewTextBoxColumn.DataPropertyName = "ID"
            resources.ApplyResources(Me.IDDataGridViewTextBoxColumn, "IDDataGridViewTextBoxColumn")
            Me.IDDataGridViewTextBoxColumn.Name = "IDDataGridViewTextBoxColumn"
            Me.IDDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EmpIDDataGridViewTextBoxColumn
            '
            Me.EmpIDDataGridViewTextBoxColumn.DataPropertyName = "EmpID"
            resources.ApplyResources(Me.EmpIDDataGridViewTextBoxColumn, "EmpIDDataGridViewTextBoxColumn")
            Me.EmpIDDataGridViewTextBoxColumn.Name = "EmpIDDataGridViewTextBoxColumn"
            '
            'FechaInicioDataGridViewTextBoxColumn
            '
            Me.FechaInicioDataGridViewTextBoxColumn.DataPropertyName = "FechaInicio"
            resources.ApplyResources(Me.FechaInicioDataGridViewTextBoxColumn, "FechaInicioDataGridViewTextBoxColumn")
            Me.FechaInicioDataGridViewTextBoxColumn.Name = "FechaInicioDataGridViewTextBoxColumn"
            '
            'FechaFinDataGridViewTextBoxColumn
            '
            Me.FechaFinDataGridViewTextBoxColumn.DataPropertyName = "FechaFin"
            resources.ApplyResources(Me.FechaFinDataGridViewTextBoxColumn, "FechaFinDataGridViewTextBoxColumn")
            Me.FechaFinDataGridViewTextBoxColumn.Name = "FechaFinDataGridViewTextBoxColumn"
            '
            'ReprocesoDataGridViewCheckBoxColumn
            '
            Me.ReprocesoDataGridViewCheckBoxColumn.DataPropertyName = "Reproceso"
            resources.ApplyResources(Me.ReprocesoDataGridViewCheckBoxColumn, "ReprocesoDataGridViewCheckBoxColumn")
            Me.ReprocesoDataGridViewCheckBoxColumn.Name = "ReprocesoDataGridViewCheckBoxColumn"
            '
            'CostoDataGridViewTextBoxColumn
            '
            Me.CostoDataGridViewTextBoxColumn.DataPropertyName = "Costo"
            resources.ApplyResources(Me.CostoDataGridViewTextBoxColumn, "CostoDataGridViewTextBoxColumn")
            Me.CostoDataGridViewTextBoxColumn.Name = "CostoDataGridViewTextBoxColumn"
            '
            'TiempoHorasDataGridViewTextBoxColumn
            '
            Me.TiempoHorasDataGridViewTextBoxColumn.DataPropertyName = "TiempoHoras"
            resources.ApplyResources(Me.TiempoHorasDataGridViewTextBoxColumn, "TiempoHorasDataGridViewTextBoxColumn")
            Me.TiempoHorasDataGridViewTextBoxColumn.Name = "TiempoHorasDataGridViewTextBoxColumn"
            '
            'NoOrdenDataGridViewTextBoxColumn
            '
            Me.NoOrdenDataGridViewTextBoxColumn.DataPropertyName = "NoOrden"
            resources.ApplyResources(Me.NoOrdenDataGridViewTextBoxColumn, "NoOrdenDataGridViewTextBoxColumn")
            Me.NoOrdenDataGridViewTextBoxColumn.Name = "NoOrdenDataGridViewTextBoxColumn"
            Me.NoOrdenDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoFaseDataGridViewTextBoxColumn
            '
            Me.NoFaseDataGridViewTextBoxColumn.DataPropertyName = "NoFase"
            resources.ApplyResources(Me.NoFaseDataGridViewTextBoxColumn, "NoFaseDataGridViewTextBoxColumn")
            Me.NoFaseDataGridViewTextBoxColumn.Name = "NoFaseDataGridViewTextBoxColumn"
            '
            'EstadoDataGridViewTextBoxColumn
            '
            Me.EstadoDataGridViewTextBoxColumn.DataPropertyName = "Estado"
            resources.ApplyResources(Me.EstadoDataGridViewTextBoxColumn, "EstadoDataGridViewTextBoxColumn")
            Me.EstadoDataGridViewTextBoxColumn.Name = "EstadoDataGridViewTextBoxColumn"
            '
            'ReferenciaDataGridViewTextBoxColumn
            '
            Me.ReferenciaDataGridViewTextBoxColumn.DataPropertyName = "Referencia"
            resources.ApplyResources(Me.ReferenciaDataGridViewTextBoxColumn, "ReferenciaDataGridViewTextBoxColumn")
            Me.ReferenciaDataGridViewTextBoxColumn.Name = "ReferenciaDataGridViewTextBoxColumn"
            '
            'EmpNombreDataGridViewTextBoxColumn
            '
            Me.EmpNombreDataGridViewTextBoxColumn.DataPropertyName = "EmpNombre"
            resources.ApplyResources(Me.EmpNombreDataGridViewTextBoxColumn, "EmpNombreDataGridViewTextBoxColumn")
            Me.EmpNombreDataGridViewTextBoxColumn.Name = "EmpNombreDataGridViewTextBoxColumn"
            Me.EmpNombreDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CheckDataGridViewCheckBoxColumn
            '
            Me.CheckDataGridViewCheckBoxColumn.DataPropertyName = "Check"
            resources.ApplyResources(Me.CheckDataGridViewCheckBoxColumn, "CheckDataGridViewCheckBoxColumn")
            Me.CheckDataGridViewCheckBoxColumn.Name = "CheckDataGridViewCheckBoxColumn"
            '
            'IndicadorDataGridViewTextBoxColumn
            '
            Me.IndicadorDataGridViewTextBoxColumn.DataPropertyName = "Indicador"
            resources.ApplyResources(Me.IndicadorDataGridViewTextBoxColumn, "IndicadorDataGridViewTextBoxColumn")
            Me.IndicadorDataGridViewTextBoxColumn.Name = "IndicadorDataGridViewTextBoxColumn"
            '
            'NoRazonDataGridViewTextBoxColumn
            '
            Me.NoRazonDataGridViewTextBoxColumn.DataPropertyName = "NoRazon"
            resources.ApplyResources(Me.NoRazonDataGridViewTextBoxColumn, "NoRazonDataGridViewTextBoxColumn")
            Me.NoRazonDataGridViewTextBoxColumn.Name = "NoRazonDataGridViewTextBoxColumn"
            '
            'RazonDataGridViewTextBoxColumn
            '
            Me.RazonDataGridViewTextBoxColumn.DataPropertyName = "Razon"
            resources.ApplyResources(Me.RazonDataGridViewTextBoxColumn, "RazonDataGridViewTextBoxColumn")
            Me.RazonDataGridViewTextBoxColumn.Name = "RazonDataGridViewTextBoxColumn"
            '
            'ProcesoDataGridViewTextBoxColumn
            '
            Me.ProcesoDataGridViewTextBoxColumn.DataPropertyName = "Proceso"
            resources.ApplyResources(Me.ProcesoDataGridViewTextBoxColumn, "ProcesoDataGridViewTextBoxColumn")
            Me.ProcesoDataGridViewTextBoxColumn.Name = "ProcesoDataGridViewTextBoxColumn"
            '
            'IDActividadDataGridViewTextBoxColumn
            '
            Me.IDActividadDataGridViewTextBoxColumn.DataPropertyName = "IDActividad"
            resources.ApplyResources(Me.IDActividadDataGridViewTextBoxColumn, "IDActividadDataGridViewTextBoxColumn")
            Me.IDActividadDataGridViewTextBoxColumn.Name = "IDActividadDataGridViewTextBoxColumn"
            '
            'ActividadDescDataGridViewTextBoxColumn
            '
            Me.ActividadDescDataGridViewTextBoxColumn.DataPropertyName = "ActividadDesc"
            resources.ApplyResources(Me.ActividadDescDataGridViewTextBoxColumn, "ActividadDescDataGridViewTextBoxColumn")
            Me.ActividadDescDataGridViewTextBoxColumn.Name = "ActividadDescDataGridViewTextBoxColumn"
            Me.ActividadDescDataGridViewTextBoxColumn.ReadOnly = True
            '
            'TotalUnidadTiempoDataGridViewTextBoxColumn
            '
            Me.TotalUnidadTiempoDataGridViewTextBoxColumn.DataPropertyName = "TotalUnidadTiempo"
            resources.ApplyResources(Me.TotalUnidadTiempoDataGridViewTextBoxColumn, "TotalUnidadTiempoDataGridViewTextBoxColumn")
            Me.TotalUnidadTiempoDataGridViewTextBoxColumn.Name = "TotalUnidadTiempoDataGridViewTextBoxColumn"
            '
            'dstActividades
            '
            Me.dstActividades.DataSetName = "ColaboradorDataset"
            Me.dstActividades.Locale = New System.Globalization.CultureInfo("en-US")
            Me.dstActividades.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'frmAsignacionTiempos
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.Label47)
            Me.Controls.Add(Me.cboFases)
            Me.Controls.Add(Me.dtgActividades)
            Me.Controls.Add(Me.btnAgregar)
            Me.Controls.Add(Me.btnCancelar)
            Me.Controls.Add(Me.Label4)
            Me.MaximizeBox = False
            Me.Name = "frmAsignacionTiempos"
            CType(Me.dtgActividades, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dstActividades, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents dtgActividades As System.Windows.Forms.DataGridView
        Friend WithEvents cboFases As SCGComboBox.SCGComboBox
        Friend WithEvents dstActividades As DMSOneFramework.ColaboradorDataset
        Friend WithEvents btnAgregar As System.Windows.Forms.Button
        Friend WithEvents btnCancelar As System.Windows.Forms.Button
        Public WithEvents Label47 As System.Windows.Forms.Label
        Public WithEvents Label4 As System.Windows.Forms.Label

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
        Friend WithEvents IDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EmpIDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaInicioDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaFinDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ReprocesoDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents CostoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents TiempoHorasDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoFaseDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ReferenciaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EmpNombreDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CheckDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents IndicadorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoRazonDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents RazonDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ProcesoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDActividadDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ActividadDescDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents TotalUnidadTiempoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    End Class
End Namespace