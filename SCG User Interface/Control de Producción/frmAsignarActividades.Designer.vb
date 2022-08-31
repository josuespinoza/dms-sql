Namespace SCG_User_Interface

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmAsignarActividades
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

        'Form reemplaza a Dispose para limpiar la lista de componentes.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Requerido por el Diseñador de Windows Forms
        Private components As System.ComponentModel.IContainer

        'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
        'Se puede modificar usando el Diseñador de Windows Forms.  
        'No lo modifique con el editor de código.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim dtsPlantillaGrid As DMSOneFramework.ActividadesXFaseDataset
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAsignarActividades))
            Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Me.grpDatosAsignacion = New System.Windows.Forms.GroupBox
            Me.Panel3 = New System.Windows.Forms.Panel
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.Label1 = New System.Windows.Forms.Label
            Me.chkReproceso = New System.Windows.Forms.CheckBox
            Me.dtpFechaFin = New System.Windows.Forms.DateTimePicker
            Me.dtpFechaInicio = New System.Windows.Forms.DateTimePicker
            Me.dtpHoraInicio = New System.Windows.Forms.DateTimePicker
            Me.dtpHoraFin = New System.Windows.Forms.DateTimePicker
            Me.lblLineaInicio = New System.Windows.Forms.Label
            Me.lblLineaFin = New System.Windows.Forms.Label
            Me.chkHoraFin = New System.Windows.Forms.CheckBox
            Me.chkHoraInicio = New System.Windows.Forms.CheckBox
            Me.cbocolaborador = New SCGComboBox.SCGComboBox
            Me.lblLine3 = New System.Windows.Forms.Label
            Me.lblColaborador = New System.Windows.Forms.Label
            Me.dtgActividades = New System.Windows.Forms.DataGridView
            Me.CheckDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
            Me.NoActividadDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ItemNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoFaseDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FaseDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ColaborasAsignadosDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.AdicionalDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.LineNumDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DuracionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoLineaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ObservacionesDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CantidadDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.PrecioAcordadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IDEmpleadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodEstadoLineaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.LineNumFatherDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.bdsAsignacion = New System.Windows.Forms.BindingSource(Me.components)
            Me.btnAsignar = New System.Windows.Forms.Button
            Me.btnCancelar = New System.Windows.Forms.Button
            Me.chkSeleccionarTodas = New System.Windows.Forms.CheckBox
            Me.Panel2 = New System.Windows.Forms.Panel
            dtsPlantillaGrid = New DMSOneFramework.ActividadesXFaseDataset
            CType(dtsPlantillaGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpDatosAsignacion.SuspendLayout()
            CType(Me.dtgActividades, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.bdsAsignacion, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'dtsPlantillaGrid
            '
            dtsPlantillaGrid.DataSetName = "ActividadesXFaseDataset"
            dtsPlantillaGrid.EnforceConstraints = False
            dtsPlantillaGrid.Locale = New System.Globalization.CultureInfo("en-US")
            dtsPlantillaGrid.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'grpDatosAsignacion
            '
            Me.grpDatosAsignacion.Controls.Add(Me.Panel3)
            Me.grpDatosAsignacion.Controls.Add(Me.Panel1)
            Me.grpDatosAsignacion.Controls.Add(Me.Label1)
            Me.grpDatosAsignacion.Controls.Add(Me.chkReproceso)
            Me.grpDatosAsignacion.Controls.Add(Me.dtpFechaFin)
            Me.grpDatosAsignacion.Controls.Add(Me.dtpFechaInicio)
            Me.grpDatosAsignacion.Controls.Add(Me.dtpHoraInicio)
            Me.grpDatosAsignacion.Controls.Add(Me.dtpHoraFin)
            Me.grpDatosAsignacion.Controls.Add(Me.lblLineaInicio)
            Me.grpDatosAsignacion.Controls.Add(Me.lblLineaFin)
            Me.grpDatosAsignacion.Controls.Add(Me.chkHoraFin)
            Me.grpDatosAsignacion.Controls.Add(Me.chkHoraInicio)
            Me.grpDatosAsignacion.Controls.Add(Me.cbocolaborador)
            Me.grpDatosAsignacion.Controls.Add(Me.lblLine3)
            Me.grpDatosAsignacion.Controls.Add(Me.lblColaborador)
            resources.ApplyResources(Me.grpDatosAsignacion, "grpDatosAsignacion")
            Me.grpDatosAsignacion.Name = "grpDatosAsignacion"
            Me.grpDatosAsignacion.TabStop = False
            '
            'Panel3
            '
            resources.ApplyResources(Me.Panel3, "Panel3")
            Me.Panel3.Name = "Panel3"
            '
            'Panel1
            '
            resources.ApplyResources(Me.Panel1, "Panel1")
            Me.Panel1.Name = "Panel1"
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'chkReproceso
            '
            resources.ApplyResources(Me.chkReproceso, "chkReproceso")
            Me.chkReproceso.Name = "chkReproceso"
            Me.chkReproceso.UseVisualStyleBackColor = True
            '
            'dtpFechaFin
            '
            Me.dtpFechaFin.CustomFormat = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            resources.ApplyResources(Me.dtpFechaFin, "dtpFechaFin")
            Me.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpFechaFin.Name = "dtpFechaFin"
            '
            'dtpFechaInicio
            '
            Me.dtpFechaInicio.CustomFormat = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            resources.ApplyResources(Me.dtpFechaInicio, "dtpFechaInicio")
            Me.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpFechaInicio.Name = "dtpFechaInicio"
            '
            'dtpHoraInicio
            '
            resources.ApplyResources(Me.dtpHoraInicio, "dtpHoraInicio")
            Me.dtpHoraInicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpHoraInicio.Name = "dtpHoraInicio"
            Me.dtpHoraInicio.ShowUpDown = True
            '
            'dtpHoraFin
            '
            resources.ApplyResources(Me.dtpHoraFin, "dtpHoraFin")
            Me.dtpHoraFin.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpHoraFin.Name = "dtpHoraFin"
            Me.dtpHoraFin.ShowUpDown = True
            '
            'lblLineaInicio
            '
            Me.lblLineaInicio.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLineaInicio, "lblLineaInicio")
            Me.lblLineaInicio.Name = "lblLineaInicio"
            '
            'lblLineaFin
            '
            Me.lblLineaFin.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLineaFin, "lblLineaFin")
            Me.lblLineaFin.Name = "lblLineaFin"
            '
            'chkHoraFin
            '
            resources.ApplyResources(Me.chkHoraFin, "chkHoraFin")
            Me.chkHoraFin.Name = "chkHoraFin"
            Me.chkHoraFin.UseVisualStyleBackColor = True
            '
            'chkHoraInicio
            '
            resources.ApplyResources(Me.chkHoraInicio, "chkHoraInicio")
            Me.chkHoraInicio.Name = "chkHoraInicio"
            Me.chkHoraInicio.UseVisualStyleBackColor = True
            '
            'cbocolaborador
            '
            Me.cbocolaborador.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cbocolaborador.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbocolaborador.EstiloSBO = True
            resources.ApplyResources(Me.cbocolaborador, "cbocolaborador")
            Me.cbocolaborador.Name = "cbocolaborador"
            '
            'lblLine3
            '
            Me.lblLine3.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLine3, "lblLine3")
            Me.lblLine3.Name = "lblLine3"
            '
            'lblColaborador
            '
            resources.ApplyResources(Me.lblColaborador, "lblColaborador")
            Me.lblColaborador.Name = "lblColaborador"
            '
            'dtgActividades
            '
            Me.dtgActividades.AllowUserToAddRows = False
            Me.dtgActividades.AllowUserToDeleteRows = False
            Me.dtgActividades.AllowUserToResizeRows = False
            DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            DataGridViewCellStyle3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            Me.dtgActividades.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle3
            Me.dtgActividades.AutoGenerateColumns = False
            Me.dtgActividades.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtgActividades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dtgActividades.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CheckDataGridViewCheckBoxColumn, Me.NoActividadDataGridViewTextBoxColumn, Me.ItemNameDataGridViewTextBoxColumn, Me.NoOrdenDataGridViewTextBoxColumn, Me.NoFaseDataGridViewTextBoxColumn, Me.EstadoDataGridViewTextBoxColumn, Me.FaseDataGridViewTextBoxColumn, Me.ColaborasAsignadosDataGridViewTextBoxColumn, Me.AdicionalDataGridViewTextBoxColumn, Me.LineNumDataGridViewTextBoxColumn, Me.DuracionDataGridViewTextBoxColumn, Me.EstadoLineaDataGridViewTextBoxColumn, Me.ObservacionesDataGridViewTextBoxColumn, Me.CantidadDataGridViewTextBoxColumn, Me.IDDataGridViewTextBoxColumn, Me.PrecioAcordadoDataGridViewTextBoxColumn, Me.IDEmpleadoDataGridViewTextBoxColumn, Me.CodEstadoLineaDataGridViewTextBoxColumn, Me.LineNumFatherDataGridViewTextBoxColumn})
            Me.dtgActividades.DataSource = Me.bdsAsignacion
            resources.ApplyResources(Me.dtgActividades, "dtgActividades")
            Me.dtgActividades.Name = "dtgActividades"
            DataGridViewCellStyle4.BackColor = System.Drawing.Color.White
            DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
            DataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black
            DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White
            DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black
            Me.dtgActividades.RowsDefaultCellStyle = DataGridViewCellStyle4
            Me.dtgActividades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
            Me.dtgActividades.ShowCellErrors = False
            Me.dtgActividades.ShowCellToolTips = False
            Me.dtgActividades.ShowEditingIcon = False
            Me.dtgActividades.ShowRowErrors = False
            '
            'CheckDataGridViewCheckBoxColumn
            '
            Me.CheckDataGridViewCheckBoxColumn.DataPropertyName = "Check"
            Me.CheckDataGridViewCheckBoxColumn.HeaderText = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.CheckDataGridViewCheckBoxColumn.Name = "CheckDataGridViewCheckBoxColumn"
            resources.ApplyResources(Me.CheckDataGridViewCheckBoxColumn, "CheckDataGridViewCheckBoxColumn")
            '
            'NoActividadDataGridViewTextBoxColumn
            '
            Me.NoActividadDataGridViewTextBoxColumn.DataPropertyName = "NoActividad"
            resources.ApplyResources(Me.NoActividadDataGridViewTextBoxColumn, "NoActividadDataGridViewTextBoxColumn")
            Me.NoActividadDataGridViewTextBoxColumn.Name = "NoActividadDataGridViewTextBoxColumn"
            Me.NoActividadDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ItemNameDataGridViewTextBoxColumn
            '
            Me.ItemNameDataGridViewTextBoxColumn.DataPropertyName = "ItemName"
            resources.ApplyResources(Me.ItemNameDataGridViewTextBoxColumn, "ItemNameDataGridViewTextBoxColumn")
            Me.ItemNameDataGridViewTextBoxColumn.Name = "ItemNameDataGridViewTextBoxColumn"
            Me.ItemNameDataGridViewTextBoxColumn.ReadOnly = True
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
            Me.NoFaseDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EstadoDataGridViewTextBoxColumn
            '
            Me.EstadoDataGridViewTextBoxColumn.DataPropertyName = "Estado"
            resources.ApplyResources(Me.EstadoDataGridViewTextBoxColumn, "EstadoDataGridViewTextBoxColumn")
            Me.EstadoDataGridViewTextBoxColumn.Name = "EstadoDataGridViewTextBoxColumn"
            Me.EstadoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FaseDataGridViewTextBoxColumn
            '
            Me.FaseDataGridViewTextBoxColumn.DataPropertyName = "Fase"
            resources.ApplyResources(Me.FaseDataGridViewTextBoxColumn, "FaseDataGridViewTextBoxColumn")
            Me.FaseDataGridViewTextBoxColumn.Name = "FaseDataGridViewTextBoxColumn"
            Me.FaseDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ColaborasAsignadosDataGridViewTextBoxColumn
            '
            Me.ColaborasAsignadosDataGridViewTextBoxColumn.DataPropertyName = "ColaborasAsignados"
            resources.ApplyResources(Me.ColaborasAsignadosDataGridViewTextBoxColumn, "ColaborasAsignadosDataGridViewTextBoxColumn")
            Me.ColaborasAsignadosDataGridViewTextBoxColumn.Name = "ColaborasAsignadosDataGridViewTextBoxColumn"
            Me.ColaborasAsignadosDataGridViewTextBoxColumn.ReadOnly = True
            '
            'AdicionalDataGridViewTextBoxColumn
            '
            Me.AdicionalDataGridViewTextBoxColumn.DataPropertyName = "Adicional"
            resources.ApplyResources(Me.AdicionalDataGridViewTextBoxColumn, "AdicionalDataGridViewTextBoxColumn")
            Me.AdicionalDataGridViewTextBoxColumn.Name = "AdicionalDataGridViewTextBoxColumn"
            Me.AdicionalDataGridViewTextBoxColumn.ReadOnly = True
            '
            'LineNumDataGridViewTextBoxColumn
            '
            Me.LineNumDataGridViewTextBoxColumn.DataPropertyName = "LineNum"
            resources.ApplyResources(Me.LineNumDataGridViewTextBoxColumn, "LineNumDataGridViewTextBoxColumn")
            Me.LineNumDataGridViewTextBoxColumn.Name = "LineNumDataGridViewTextBoxColumn"
            Me.LineNumDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DuracionDataGridViewTextBoxColumn
            '
            Me.DuracionDataGridViewTextBoxColumn.DataPropertyName = "Duracion"
            resources.ApplyResources(Me.DuracionDataGridViewTextBoxColumn, "DuracionDataGridViewTextBoxColumn")
            Me.DuracionDataGridViewTextBoxColumn.Name = "DuracionDataGridViewTextBoxColumn"
            Me.DuracionDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EstadoLineaDataGridViewTextBoxColumn
            '
            Me.EstadoLineaDataGridViewTextBoxColumn.DataPropertyName = "EstadoLinea"
            resources.ApplyResources(Me.EstadoLineaDataGridViewTextBoxColumn, "EstadoLineaDataGridViewTextBoxColumn")
            Me.EstadoLineaDataGridViewTextBoxColumn.Name = "EstadoLineaDataGridViewTextBoxColumn"
            Me.EstadoLineaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ObservacionesDataGridViewTextBoxColumn
            '
            Me.ObservacionesDataGridViewTextBoxColumn.DataPropertyName = "Observaciones"
            resources.ApplyResources(Me.ObservacionesDataGridViewTextBoxColumn, "ObservacionesDataGridViewTextBoxColumn")
            Me.ObservacionesDataGridViewTextBoxColumn.Name = "ObservacionesDataGridViewTextBoxColumn"
            Me.ObservacionesDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CantidadDataGridViewTextBoxColumn
            '
            Me.CantidadDataGridViewTextBoxColumn.DataPropertyName = "Cantidad"
            resources.ApplyResources(Me.CantidadDataGridViewTextBoxColumn, "CantidadDataGridViewTextBoxColumn")
            Me.CantidadDataGridViewTextBoxColumn.Name = "CantidadDataGridViewTextBoxColumn"
            Me.CantidadDataGridViewTextBoxColumn.ReadOnly = True
            '
            'IDDataGridViewTextBoxColumn
            '
            Me.IDDataGridViewTextBoxColumn.DataPropertyName = "ID"
            resources.ApplyResources(Me.IDDataGridViewTextBoxColumn, "IDDataGridViewTextBoxColumn")
            Me.IDDataGridViewTextBoxColumn.Name = "IDDataGridViewTextBoxColumn"
            Me.IDDataGridViewTextBoxColumn.ReadOnly = True
            '
            'PrecioAcordadoDataGridViewTextBoxColumn
            '
            Me.PrecioAcordadoDataGridViewTextBoxColumn.DataPropertyName = "PrecioAcordado"
            resources.ApplyResources(Me.PrecioAcordadoDataGridViewTextBoxColumn, "PrecioAcordadoDataGridViewTextBoxColumn")
            Me.PrecioAcordadoDataGridViewTextBoxColumn.Name = "PrecioAcordadoDataGridViewTextBoxColumn"
            Me.PrecioAcordadoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'IDEmpleadoDataGridViewTextBoxColumn
            '
            Me.IDEmpleadoDataGridViewTextBoxColumn.DataPropertyName = "IDEmpleado"
            resources.ApplyResources(Me.IDEmpleadoDataGridViewTextBoxColumn, "IDEmpleadoDataGridViewTextBoxColumn")
            Me.IDEmpleadoDataGridViewTextBoxColumn.Name = "IDEmpleadoDataGridViewTextBoxColumn"
            Me.IDEmpleadoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodEstadoLineaDataGridViewTextBoxColumn
            '
            Me.CodEstadoLineaDataGridViewTextBoxColumn.DataPropertyName = "CodEstadoLinea"
            resources.ApplyResources(Me.CodEstadoLineaDataGridViewTextBoxColumn, "CodEstadoLineaDataGridViewTextBoxColumn")
            Me.CodEstadoLineaDataGridViewTextBoxColumn.Name = "CodEstadoLineaDataGridViewTextBoxColumn"
            Me.CodEstadoLineaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'LineNumFatherDataGridViewTextBoxColumn
            '
            Me.LineNumFatherDataGridViewTextBoxColumn.DataPropertyName = "LineNumFather"
            resources.ApplyResources(Me.LineNumFatherDataGridViewTextBoxColumn, "LineNumFatherDataGridViewTextBoxColumn")
            Me.LineNumFatherDataGridViewTextBoxColumn.Name = "LineNumFatherDataGridViewTextBoxColumn"
            Me.LineNumFatherDataGridViewTextBoxColumn.ReadOnly = True
            '
            'bdsAsignacion
            '
            Me.bdsAsignacion.DataMember = "SCGTA_TB_ActividadesxOrden"
            Me.bdsAsignacion.DataSource = dtsPlantillaGrid
            '
            'btnAsignar
            '
            Me.btnAsignar.BackColor = System.Drawing.SystemColors.Control
            resources.ApplyResources(Me.btnAsignar, "btnAsignar")
            Me.btnAsignar.ForeColor = System.Drawing.Color.Black
            Me.btnAsignar.Name = "btnAsignar"
            Me.btnAsignar.UseVisualStyleBackColor = False
            '
            'btnCancelar
            '
            Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            resources.ApplyResources(Me.btnCancelar, "btnCancelar")
            Me.btnCancelar.Name = "btnCancelar"
            '
            'chkSeleccionarTodas
            '
            resources.ApplyResources(Me.chkSeleccionarTodas, "chkSeleccionarTodas")
            Me.chkSeleccionarTodas.Name = "chkSeleccionarTodas"
            Me.chkSeleccionarTodas.UseVisualStyleBackColor = True
            '
            'Panel2
            '
            resources.ApplyResources(Me.Panel2, "Panel2")
            Me.Panel2.Name = "Panel2"
            '
            'frmAsignarActividades
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.CancelButton = Me.btnCancelar
            Me.Controls.Add(Me.grpDatosAsignacion)
            Me.Controls.Add(Me.dtgActividades)
            Me.Controls.Add(Me.btnAsignar)
            Me.Controls.Add(Me.btnCancelar)
            Me.Controls.Add(Me.chkSeleccionarTodas)
            Me.Controls.Add(Me.Panel2)
            Me.MaximizeBox = False
            Me.Name = "frmAsignarActividades"
            CType(dtsPlantillaGrid, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpDatosAsignacion.ResumeLayout(False)
            Me.grpDatosAsignacion.PerformLayout()
            CType(Me.dtgActividades, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.bdsAsignacion, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents grpDatosAsignacion As System.Windows.Forms.GroupBox
        Friend WithEvents lblColaborador As System.Windows.Forms.Label
        Friend WithEvents cbocolaborador As SCGComboBox.SCGComboBox
        Public WithEvents lblLine3 As System.Windows.Forms.Label
        Friend WithEvents dtgActividades As System.Windows.Forms.DataGridView
        Friend WithEvents btnAsignar As System.Windows.Forms.Button
        Friend WithEvents btnCancelar As System.Windows.Forms.Button
        Friend WithEvents bdsAsignacion As System.Windows.Forms.BindingSource
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents chkReproceso As System.Windows.Forms.CheckBox
        Friend WithEvents dtpFechaFin As System.Windows.Forms.DateTimePicker
        Friend WithEvents dtpFechaInicio As System.Windows.Forms.DateTimePicker
        Friend WithEvents dtpHoraInicio As System.Windows.Forms.DateTimePicker
        Friend WithEvents dtpHoraFin As System.Windows.Forms.DateTimePicker
        Public WithEvents lblLineaInicio As System.Windows.Forms.Label
        Public WithEvents lblLineaFin As System.Windows.Forms.Label
        Friend WithEvents chkHoraFin As System.Windows.Forms.CheckBox
        Friend WithEvents chkHoraInicio As System.Windows.Forms.CheckBox
        Friend WithEvents chkSeleccionarTodas As System.Windows.Forms.CheckBox
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents Panel3 As System.Windows.Forms.Panel
        Friend WithEvents CheckDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents NoActividadDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ItemNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoFaseDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FaseDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColaborasAsignadosDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents AdicionalDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents LineNumDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DuracionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoLineaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ObservacionesDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CantidadDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents PrecioAcordadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDEmpleadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodEstadoLineaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents LineNumFatherDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    End Class

End Namespace