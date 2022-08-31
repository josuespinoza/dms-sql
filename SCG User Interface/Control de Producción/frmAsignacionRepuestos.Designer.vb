Namespace SCG_User_Interface

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmAsignacionRepuestos
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAsignacionRepuestos))
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Me.chkSeleccionarTodas = New System.Windows.Forms.CheckBox
            Me.btnCancelar = New System.Windows.Forms.Button
            Me.btnAsignar = New System.Windows.Forms.Button
            Me.grpDatosAsignacion = New System.Windows.Forms.GroupBox
            Me.cbocolaborador = New SCGComboBox.SCGComboBox
            Me.lblLine3 = New System.Windows.Forms.Label
            Me.lblColaborador = New System.Windows.Forms.Label
            Me.dtgRepuestos = New System.Windows.Forms.DataGridView
            Me.DescEstadoResources = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NombEmpleado = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CheckDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
            Me.NoRepuestoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ItemnameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodEstadoRepDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CantidadDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.AdicionalDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CantidadEstadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoRepDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoAdicionalDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaSolicitudDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaCompromisoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.BodegaDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
            Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.TipoArticuloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.LineNumDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoLineaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ObservacionesDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.PrecioAcordadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.InformacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoTransfDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodEstadoLineaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.LineNumFatherDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CantidadPendienteDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CantidadSolicitadaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CantidadRecibidaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CantidadPendienteTrasladoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.TrasladadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ItemNameEspecificoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ItemCodeEspecificoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.GenericoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.BindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.RepuestosxOrdenDataset = New DMSOneFramework.RepuestosxOrdenDataset
            Me.grpDatosAsignacion.SuspendLayout()
            CType(Me.dtgRepuestos, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.BindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.RepuestosxOrdenDataset, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'chkSeleccionarTodas
            '
            resources.ApplyResources(Me.chkSeleccionarTodas, "chkSeleccionarTodas")
            Me.chkSeleccionarTodas.Name = "chkSeleccionarTodas"
            Me.chkSeleccionarTodas.UseVisualStyleBackColor = True
            '
            'btnCancelar
            '
            Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            resources.ApplyResources(Me.btnCancelar, "btnCancelar")
            Me.btnCancelar.Name = "btnCancelar"
            '
            'btnAsignar
            '
            Me.btnAsignar.BackColor = System.Drawing.SystemColors.Control
            resources.ApplyResources(Me.btnAsignar, "btnAsignar")
            Me.btnAsignar.ForeColor = System.Drawing.Color.Black
            Me.btnAsignar.Name = "btnAsignar"
            Me.btnAsignar.UseVisualStyleBackColor = False
            '
            'grpDatosAsignacion
            '
            Me.grpDatosAsignacion.Controls.Add(Me.cbocolaborador)
            Me.grpDatosAsignacion.Controls.Add(Me.lblLine3)
            Me.grpDatosAsignacion.Controls.Add(Me.lblColaborador)
            resources.ApplyResources(Me.grpDatosAsignacion, "grpDatosAsignacion")
            Me.grpDatosAsignacion.Name = "grpDatosAsignacion"
            Me.grpDatosAsignacion.TabStop = False
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
            Me.lblLine3.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine3, "lblLine3")
            Me.lblLine3.Name = "lblLine3"
            '
            'lblColaborador
            '
            resources.ApplyResources(Me.lblColaborador, "lblColaborador")
            Me.lblColaborador.Name = "lblColaborador"
            '
            'dtgRepuestos
            '
            Me.dtgRepuestos.AllowUserToAddRows = False
            Me.dtgRepuestos.AllowUserToDeleteRows = False
            Me.dtgRepuestos.AllowUserToResizeRows = False
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(240, Byte), Integer))
            DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(240, Byte), Integer))
            Me.dtgRepuestos.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
            Me.dtgRepuestos.AutoGenerateColumns = False
            Me.dtgRepuestos.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dtgRepuestos.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
            Me.dtgRepuestos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dtgRepuestos.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DescEstadoResources, Me.NombEmpleado, Me.DataGridViewTextBoxColumn1, Me.CheckDataGridViewCheckBoxColumn, Me.NoRepuestoDataGridViewTextBoxColumn, Me.ItemnameDataGridViewTextBoxColumn, Me.NoOrdenDataGridViewTextBoxColumn, Me.CodEstadoRepDataGridViewTextBoxColumn, Me.CantidadDataGridViewTextBoxColumn, Me.AdicionalDataGridViewTextBoxColumn, Me.CantidadEstadoDataGridViewTextBoxColumn, Me.EstadoRepDataGridViewTextBoxColumn, Me.NoAdicionalDataGridViewTextBoxColumn, Me.FechaSolicitudDataGridViewTextBoxColumn, Me.FechaCompromisoDataGridViewTextBoxColumn, Me.BodegaDataGridViewCheckBoxColumn, Me.IDDataGridViewTextBoxColumn, Me.TipoArticuloDataGridViewTextBoxColumn, Me.LineNumDataGridViewTextBoxColumn, Me.EstadoLineaDataGridViewTextBoxColumn, Me.ObservacionesDataGridViewTextBoxColumn, Me.PrecioAcordadoDataGridViewTextBoxColumn, Me.InformacionDataGridViewTextBoxColumn, Me.EstadoTransfDataGridViewTextBoxColumn, Me.CodEstadoLineaDataGridViewTextBoxColumn, Me.LineNumFatherDataGridViewTextBoxColumn, Me.CantidadPendienteDataGridViewTextBoxColumn, Me.CantidadSolicitadaDataGridViewTextBoxColumn, Me.CantidadRecibidaDataGridViewTextBoxColumn, Me.CantidadPendienteTrasladoDataGridViewTextBoxColumn, Me.TrasladadoDataGridViewTextBoxColumn, Me.ItemNameEspecificoDataGridViewTextBoxColumn, Me.ItemCodeEspecificoDataGridViewTextBoxColumn, Me.GenericoDataGridViewTextBoxColumn})
            Me.dtgRepuestos.DataSource = Me.BindingSource
            DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dtgRepuestos.DefaultCellStyle = DataGridViewCellStyle3
            resources.ApplyResources(Me.dtgRepuestos, "dtgRepuestos")
            Me.dtgRepuestos.Name = "dtgRepuestos"
            DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dtgRepuestos.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
            Me.dtgRepuestos.RowHeadersVisible = False
            DataGridViewCellStyle5.BackColor = System.Drawing.Color.White
            DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
            DataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black
            DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White
            DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black
            Me.dtgRepuestos.RowsDefaultCellStyle = DataGridViewCellStyle5
            Me.dtgRepuestos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
            Me.dtgRepuestos.ShowCellErrors = False
            Me.dtgRepuestos.ShowCellToolTips = False
            Me.dtgRepuestos.ShowEditingIcon = False
            Me.dtgRepuestos.ShowRowErrors = False
            '
            'DescEstadoResources
            '
            Me.DescEstadoResources.DataPropertyName = "DescEstadoResources"
            resources.ApplyResources(Me.DescEstadoResources, "DescEstadoResources")
            Me.DescEstadoResources.Name = "DescEstadoResources"
            Me.DescEstadoResources.ReadOnly = True
            '
            'NombEmpleado
            '
            Me.NombEmpleado.DataPropertyName = "NombEmpleado"
            resources.ApplyResources(Me.NombEmpleado, "NombEmpleado")
            Me.NombEmpleado.Name = "NombEmpleado"
            Me.NombEmpleado.ReadOnly = True
            '
            'DataGridViewTextBoxColumn1
            '
            Me.DataGridViewTextBoxColumn1.DataPropertyName = "DescEstadoResources"
            resources.ApplyResources(Me.DataGridViewTextBoxColumn1, "DataGridViewTextBoxColumn1")
            Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
            Me.DataGridViewTextBoxColumn1.ReadOnly = True
            '
            'CheckDataGridViewCheckBoxColumn
            '
            Me.CheckDataGridViewCheckBoxColumn.DataPropertyName = "Check"
            Me.CheckDataGridViewCheckBoxColumn.FillWeight = 25.0!
            Me.CheckDataGridViewCheckBoxColumn.HeaderText = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            resources.ApplyResources(Me.CheckDataGridViewCheckBoxColumn, "CheckDataGridViewCheckBoxColumn")
            Me.CheckDataGridViewCheckBoxColumn.Name = "CheckDataGridViewCheckBoxColumn"
            '
            'NoRepuestoDataGridViewTextBoxColumn
            '
            Me.NoRepuestoDataGridViewTextBoxColumn.DataPropertyName = "NoRepuesto"
            resources.ApplyResources(Me.NoRepuestoDataGridViewTextBoxColumn, "NoRepuestoDataGridViewTextBoxColumn")
            Me.NoRepuestoDataGridViewTextBoxColumn.Name = "NoRepuestoDataGridViewTextBoxColumn"
            Me.NoRepuestoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ItemnameDataGridViewTextBoxColumn
            '
            Me.ItemnameDataGridViewTextBoxColumn.DataPropertyName = "Itemname"
            resources.ApplyResources(Me.ItemnameDataGridViewTextBoxColumn, "ItemnameDataGridViewTextBoxColumn")
            Me.ItemnameDataGridViewTextBoxColumn.Name = "ItemnameDataGridViewTextBoxColumn"
            Me.ItemnameDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoOrdenDataGridViewTextBoxColumn
            '
            Me.NoOrdenDataGridViewTextBoxColumn.DataPropertyName = "NoOrden"
            resources.ApplyResources(Me.NoOrdenDataGridViewTextBoxColumn, "NoOrdenDataGridViewTextBoxColumn")
            Me.NoOrdenDataGridViewTextBoxColumn.Name = "NoOrdenDataGridViewTextBoxColumn"
            '
            'CodEstadoRepDataGridViewTextBoxColumn
            '
            Me.CodEstadoRepDataGridViewTextBoxColumn.DataPropertyName = "CodEstadoRep"
            resources.ApplyResources(Me.CodEstadoRepDataGridViewTextBoxColumn, "CodEstadoRepDataGridViewTextBoxColumn")
            Me.CodEstadoRepDataGridViewTextBoxColumn.Name = "CodEstadoRepDataGridViewTextBoxColumn"
            '
            'CantidadDataGridViewTextBoxColumn
            '
            Me.CantidadDataGridViewTextBoxColumn.DataPropertyName = "Cantidad"
            resources.ApplyResources(Me.CantidadDataGridViewTextBoxColumn, "CantidadDataGridViewTextBoxColumn")
            Me.CantidadDataGridViewTextBoxColumn.Name = "CantidadDataGridViewTextBoxColumn"
            '
            'AdicionalDataGridViewTextBoxColumn
            '
            Me.AdicionalDataGridViewTextBoxColumn.DataPropertyName = "Adicional"
            resources.ApplyResources(Me.AdicionalDataGridViewTextBoxColumn, "AdicionalDataGridViewTextBoxColumn")
            Me.AdicionalDataGridViewTextBoxColumn.Name = "AdicionalDataGridViewTextBoxColumn"
            '
            'CantidadEstadoDataGridViewTextBoxColumn
            '
            Me.CantidadEstadoDataGridViewTextBoxColumn.DataPropertyName = "CantidadEstado"
            resources.ApplyResources(Me.CantidadEstadoDataGridViewTextBoxColumn, "CantidadEstadoDataGridViewTextBoxColumn")
            Me.CantidadEstadoDataGridViewTextBoxColumn.Name = "CantidadEstadoDataGridViewTextBoxColumn"
            '
            'EstadoRepDataGridViewTextBoxColumn
            '
            Me.EstadoRepDataGridViewTextBoxColumn.DataPropertyName = "EstadoRep"
            resources.ApplyResources(Me.EstadoRepDataGridViewTextBoxColumn, "EstadoRepDataGridViewTextBoxColumn")
            Me.EstadoRepDataGridViewTextBoxColumn.Name = "EstadoRepDataGridViewTextBoxColumn"
            Me.EstadoRepDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoAdicionalDataGridViewTextBoxColumn
            '
            Me.NoAdicionalDataGridViewTextBoxColumn.DataPropertyName = "NoAdicional"
            resources.ApplyResources(Me.NoAdicionalDataGridViewTextBoxColumn, "NoAdicionalDataGridViewTextBoxColumn")
            Me.NoAdicionalDataGridViewTextBoxColumn.Name = "NoAdicionalDataGridViewTextBoxColumn"
            '
            'FechaSolicitudDataGridViewTextBoxColumn
            '
            Me.FechaSolicitudDataGridViewTextBoxColumn.DataPropertyName = "Fecha_Solicitud"
            resources.ApplyResources(Me.FechaSolicitudDataGridViewTextBoxColumn, "FechaSolicitudDataGridViewTextBoxColumn")
            Me.FechaSolicitudDataGridViewTextBoxColumn.Name = "FechaSolicitudDataGridViewTextBoxColumn"
            '
            'FechaCompromisoDataGridViewTextBoxColumn
            '
            Me.FechaCompromisoDataGridViewTextBoxColumn.DataPropertyName = "Fecha_Compromiso"
            resources.ApplyResources(Me.FechaCompromisoDataGridViewTextBoxColumn, "FechaCompromisoDataGridViewTextBoxColumn")
            Me.FechaCompromisoDataGridViewTextBoxColumn.Name = "FechaCompromisoDataGridViewTextBoxColumn"
            '
            'BodegaDataGridViewCheckBoxColumn
            '
            Me.BodegaDataGridViewCheckBoxColumn.DataPropertyName = "Bodega"
            resources.ApplyResources(Me.BodegaDataGridViewCheckBoxColumn, "BodegaDataGridViewCheckBoxColumn")
            Me.BodegaDataGridViewCheckBoxColumn.Name = "BodegaDataGridViewCheckBoxColumn"
            '
            'IDDataGridViewTextBoxColumn
            '
            Me.IDDataGridViewTextBoxColumn.DataPropertyName = "ID"
            resources.ApplyResources(Me.IDDataGridViewTextBoxColumn, "IDDataGridViewTextBoxColumn")
            Me.IDDataGridViewTextBoxColumn.Name = "IDDataGridViewTextBoxColumn"
            '
            'TipoArticuloDataGridViewTextBoxColumn
            '
            Me.TipoArticuloDataGridViewTextBoxColumn.DataPropertyName = "TipoArticulo"
            resources.ApplyResources(Me.TipoArticuloDataGridViewTextBoxColumn, "TipoArticuloDataGridViewTextBoxColumn")
            Me.TipoArticuloDataGridViewTextBoxColumn.Name = "TipoArticuloDataGridViewTextBoxColumn"
            '
            'LineNumDataGridViewTextBoxColumn
            '
            Me.LineNumDataGridViewTextBoxColumn.DataPropertyName = "LineNum"
            resources.ApplyResources(Me.LineNumDataGridViewTextBoxColumn, "LineNumDataGridViewTextBoxColumn")
            Me.LineNumDataGridViewTextBoxColumn.Name = "LineNumDataGridViewTextBoxColumn"
            '
            'EstadoLineaDataGridViewTextBoxColumn
            '
            Me.EstadoLineaDataGridViewTextBoxColumn.DataPropertyName = "EstadoLinea"
            resources.ApplyResources(Me.EstadoLineaDataGridViewTextBoxColumn, "EstadoLineaDataGridViewTextBoxColumn")
            Me.EstadoLineaDataGridViewTextBoxColumn.Name = "EstadoLineaDataGridViewTextBoxColumn"
            '
            'ObservacionesDataGridViewTextBoxColumn
            '
            Me.ObservacionesDataGridViewTextBoxColumn.DataPropertyName = "Observaciones"
            resources.ApplyResources(Me.ObservacionesDataGridViewTextBoxColumn, "ObservacionesDataGridViewTextBoxColumn")
            Me.ObservacionesDataGridViewTextBoxColumn.Name = "ObservacionesDataGridViewTextBoxColumn"
            '
            'PrecioAcordadoDataGridViewTextBoxColumn
            '
            Me.PrecioAcordadoDataGridViewTextBoxColumn.DataPropertyName = "PrecioAcordado"
            resources.ApplyResources(Me.PrecioAcordadoDataGridViewTextBoxColumn, "PrecioAcordadoDataGridViewTextBoxColumn")
            Me.PrecioAcordadoDataGridViewTextBoxColumn.Name = "PrecioAcordadoDataGridViewTextBoxColumn"
            '
            'InformacionDataGridViewTextBoxColumn
            '
            Me.InformacionDataGridViewTextBoxColumn.DataPropertyName = "Informacion"
            resources.ApplyResources(Me.InformacionDataGridViewTextBoxColumn, "InformacionDataGridViewTextBoxColumn")
            Me.InformacionDataGridViewTextBoxColumn.Name = "InformacionDataGridViewTextBoxColumn"
            '
            'EstadoTransfDataGridViewTextBoxColumn
            '
            Me.EstadoTransfDataGridViewTextBoxColumn.DataPropertyName = "EstadoTransf"
            resources.ApplyResources(Me.EstadoTransfDataGridViewTextBoxColumn, "EstadoTransfDataGridViewTextBoxColumn")
            Me.EstadoTransfDataGridViewTextBoxColumn.Name = "EstadoTransfDataGridViewTextBoxColumn"
            '
            'CodEstadoLineaDataGridViewTextBoxColumn
            '
            Me.CodEstadoLineaDataGridViewTextBoxColumn.DataPropertyName = "CodEstadoLinea"
            resources.ApplyResources(Me.CodEstadoLineaDataGridViewTextBoxColumn, "CodEstadoLineaDataGridViewTextBoxColumn")
            Me.CodEstadoLineaDataGridViewTextBoxColumn.Name = "CodEstadoLineaDataGridViewTextBoxColumn"
            '
            'LineNumFatherDataGridViewTextBoxColumn
            '
            Me.LineNumFatherDataGridViewTextBoxColumn.DataPropertyName = "LineNumFather"
            resources.ApplyResources(Me.LineNumFatherDataGridViewTextBoxColumn, "LineNumFatherDataGridViewTextBoxColumn")
            Me.LineNumFatherDataGridViewTextBoxColumn.Name = "LineNumFatherDataGridViewTextBoxColumn"
            '
            'CantidadPendienteDataGridViewTextBoxColumn
            '
            Me.CantidadPendienteDataGridViewTextBoxColumn.DataPropertyName = "CantidadPendiente"
            resources.ApplyResources(Me.CantidadPendienteDataGridViewTextBoxColumn, "CantidadPendienteDataGridViewTextBoxColumn")
            Me.CantidadPendienteDataGridViewTextBoxColumn.Name = "CantidadPendienteDataGridViewTextBoxColumn"
            '
            'CantidadSolicitadaDataGridViewTextBoxColumn
            '
            Me.CantidadSolicitadaDataGridViewTextBoxColumn.DataPropertyName = "CantidadSolicitada"
            resources.ApplyResources(Me.CantidadSolicitadaDataGridViewTextBoxColumn, "CantidadSolicitadaDataGridViewTextBoxColumn")
            Me.CantidadSolicitadaDataGridViewTextBoxColumn.Name = "CantidadSolicitadaDataGridViewTextBoxColumn"
            '
            'CantidadRecibidaDataGridViewTextBoxColumn
            '
            Me.CantidadRecibidaDataGridViewTextBoxColumn.DataPropertyName = "CantidadRecibida"
            resources.ApplyResources(Me.CantidadRecibidaDataGridViewTextBoxColumn, "CantidadRecibidaDataGridViewTextBoxColumn")
            Me.CantidadRecibidaDataGridViewTextBoxColumn.Name = "CantidadRecibidaDataGridViewTextBoxColumn"
            '
            'CantidadPendienteTrasladoDataGridViewTextBoxColumn
            '
            Me.CantidadPendienteTrasladoDataGridViewTextBoxColumn.DataPropertyName = "CantidadPendienteTraslado"
            resources.ApplyResources(Me.CantidadPendienteTrasladoDataGridViewTextBoxColumn, "CantidadPendienteTrasladoDataGridViewTextBoxColumn")
            Me.CantidadPendienteTrasladoDataGridViewTextBoxColumn.Name = "CantidadPendienteTrasladoDataGridViewTextBoxColumn"
            '
            'TrasladadoDataGridViewTextBoxColumn
            '
            Me.TrasladadoDataGridViewTextBoxColumn.DataPropertyName = "Trasladado"
            resources.ApplyResources(Me.TrasladadoDataGridViewTextBoxColumn, "TrasladadoDataGridViewTextBoxColumn")
            Me.TrasladadoDataGridViewTextBoxColumn.Name = "TrasladadoDataGridViewTextBoxColumn"
            '
            'ItemNameEspecificoDataGridViewTextBoxColumn
            '
            Me.ItemNameEspecificoDataGridViewTextBoxColumn.DataPropertyName = "ItemNameEspecifico"
            resources.ApplyResources(Me.ItemNameEspecificoDataGridViewTextBoxColumn, "ItemNameEspecificoDataGridViewTextBoxColumn")
            Me.ItemNameEspecificoDataGridViewTextBoxColumn.Name = "ItemNameEspecificoDataGridViewTextBoxColumn"
            '
            'ItemCodeEspecificoDataGridViewTextBoxColumn
            '
            Me.ItemCodeEspecificoDataGridViewTextBoxColumn.DataPropertyName = "ItemCodeEspecifico"
            resources.ApplyResources(Me.ItemCodeEspecificoDataGridViewTextBoxColumn, "ItemCodeEspecificoDataGridViewTextBoxColumn")
            Me.ItemCodeEspecificoDataGridViewTextBoxColumn.Name = "ItemCodeEspecificoDataGridViewTextBoxColumn"
            '
            'GenericoDataGridViewTextBoxColumn
            '
            Me.GenericoDataGridViewTextBoxColumn.DataPropertyName = "Generico"
            resources.ApplyResources(Me.GenericoDataGridViewTextBoxColumn, "GenericoDataGridViewTextBoxColumn")
            Me.GenericoDataGridViewTextBoxColumn.Name = "GenericoDataGridViewTextBoxColumn"
            '
            'BindingSource
            '
            Me.BindingSource.DataMember = "SCGTA_TB_RepuestosxOrden"
            Me.BindingSource.DataSource = Me.RepuestosxOrdenDataset
            '
            'RepuestosxOrdenDataset
            '
            Me.RepuestosxOrdenDataset.DataSetName = "RepuestosxOrdenDataset"
            Me.RepuestosxOrdenDataset.Locale = New System.Globalization.CultureInfo("en-US")
            Me.RepuestosxOrdenDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'frmAsignacionRepuestos
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.chkSeleccionarTodas)
            Me.Controls.Add(Me.btnCancelar)
            Me.Controls.Add(Me.btnAsignar)
            Me.Controls.Add(Me.grpDatosAsignacion)
            Me.Controls.Add(Me.dtgRepuestos)
            Me.MaximizeBox = False
            Me.Name = "frmAsignacionRepuestos"
            Me.grpDatosAsignacion.ResumeLayout(False)
            Me.grpDatosAsignacion.PerformLayout()
            CType(Me.dtgRepuestos, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.BindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.RepuestosxOrdenDataset, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents chkSeleccionarTodas As System.Windows.Forms.CheckBox
        Friend WithEvents btnCancelar As System.Windows.Forms.Button
        Friend WithEvents btnAsignar As System.Windows.Forms.Button
        Friend WithEvents grpDatosAsignacion As System.Windows.Forms.GroupBox
        Friend WithEvents cbocolaborador As SCGComboBox.SCGComboBox
        Public WithEvents lblLine3 As System.Windows.Forms.Label
        Friend WithEvents lblColaborador As System.Windows.Forms.Label
        Friend WithEvents dtgRepuestos As System.Windows.Forms.DataGridView
        Friend WithEvents RepuestosxOrdenDataset As DMSOneFramework.RepuestosxOrdenDataset
        Friend WithEvents BindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents DescEstadoResources As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NombEmpleado As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CheckDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents NoRepuestoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ItemnameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodEstadoRepDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CantidadDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents AdicionalDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CantidadEstadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoRepDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoAdicionalDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaSolicitudDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaCompromisoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents BodegaDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents IDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents TipoArticuloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents LineNumDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoLineaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ObservacionesDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents PrecioAcordadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents InformacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoTransfDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodEstadoLineaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents LineNumFatherDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CantidadPendienteDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CantidadSolicitadaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CantidadRecibidaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CantidadPendienteTrasladoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents TrasladadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ItemNameEspecificoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ItemCodeEspecificoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents GenericoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    End Class

End Namespace
