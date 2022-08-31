Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmBusquedaSolicitudesEspecificos
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
            Dim SolicitudEspecificosDataset1 As DMSOneFramework.SolicitudEspecificosDataset
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBusquedaSolicitudesEspecificos))
            Me.grpItems = New System.Windows.Forms.GroupBox
            Me.dtgDetalles = New System.Windows.Forms.DataGridView
            Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaSolicitudDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.SolicitadoPorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescEstadoResources = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescEstadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescMarcaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescEstiloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescModeloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.RespondidoPorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaRespuestaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
            Me.PlacaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IDVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodMarcaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodEstiloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodModeloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoVisitaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaaperturaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechacompromisoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodTipoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoCotizacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.TipoDescDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ObservacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.HoraCompDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaCompDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.tlbSolicitudespecificos = New Proyecto_SCGToolBar.SCGToolBar
            Me.grpOrdenInfo = New System.Windows.Forms.GroupBox
            Me.cboEstado = New SCGComboBox.SCGComboBox
            Me.lblLine6 = New System.Windows.Forms.Label
            Me.chkEstado = New System.Windows.Forms.CheckBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.txtNoVehiculo = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.lblNoVehiculo = New System.Windows.Forms.Label
            Me.lblLine2 = New System.Windows.Forms.Label
            Me.txtPlaca = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label6 = New System.Windows.Forms.Label
            Me.txtNoSolicitud = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label3 = New System.Windows.Forms.Label
            Me.lblNoSolicitud = New System.Windows.Forms.Label
            Me.Panel6 = New System.Windows.Forms.Panel
            Me.Panel7 = New System.Windows.Forms.Panel
            Me.dtpCompromisoini = New System.Windows.Forms.DateTimePicker
            Me.dtpAperturaini = New System.Windows.Forms.DateTimePicker
            Me.Panel3 = New System.Windows.Forms.Panel
            Me.dtpCompromisofin = New System.Windows.Forms.DateTimePicker
            Me.Panel4 = New System.Windows.Forms.Panel
            Me.dtpAperturafin = New System.Windows.Forms.DateTimePicker
            Me.Panel9 = New System.Windows.Forms.Panel
            Me.lblLine9 = New System.Windows.Forms.Label
            Me.Panel10 = New System.Windows.Forms.Panel
            Me.Label2 = New System.Windows.Forms.Label
            Me.chkRespuesta = New System.Windows.Forms.CheckBox
            Me.chkSolicitud = New System.Windows.Forms.CheckBox
            Me.txtNoVisita = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.cboMarca = New SCGComboBox.SCGComboBox
            Me.cboModelo = New SCGComboBox.SCGComboBox
            Me.cboEstilo = New SCGComboBox.SCGComboBox
            Me.lblLine5 = New System.Windows.Forms.Label
            Me.lblLine8 = New System.Windows.Forms.Label
            Me.lblLine7 = New System.Windows.Forms.Label
            Me.chkModelo = New System.Windows.Forms.CheckBox
            Me.chkEstilo = New System.Windows.Forms.CheckBox
            Me.lblLine3 = New System.Windows.Forms.Label
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.txtNoOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.lblNoOrden = New System.Windows.Forms.Label
            Me.lblNoexpediente = New System.Windows.Forms.Label
            Me.chkMarca = New System.Windows.Forms.CheckBox
            SolicitudEspecificosDataset1 = New DMSOneFramework.SolicitudEspecificosDataset
            CType(SolicitudEspecificosDataset1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpItems.SuspendLayout()
            CType(Me.dtgDetalles, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpOrdenInfo.SuspendLayout()
            Me.SuspendLayout()
            '
            'SolicitudEspecificosDataset1
            '
            SolicitudEspecificosDataset1.DataSetName = "SolicitudEspecificosDataset"
            SolicitudEspecificosDataset1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'grpItems
            '
            Me.grpItems.Controls.Add(Me.dtgDetalles)
            resources.ApplyResources(Me.grpItems, "grpItems")
            Me.grpItems.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.grpItems.Name = "grpItems"
            Me.grpItems.TabStop = False
            '
            'dtgDetalles
            '
            Me.dtgDetalles.AllowUserToAddRows = False
            Me.dtgDetalles.AllowUserToDeleteRows = False
            Me.dtgDetalles.AllowUserToResizeRows = False
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            Me.dtgDetalles.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
            Me.dtgDetalles.AutoGenerateColumns = False
            Me.dtgDetalles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
            Me.dtgDetalles.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtgDetalles.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDDataGridViewTextBoxColumn, Me.NoOrdenDataGridViewTextBoxColumn, Me.FechaSolicitudDataGridViewTextBoxColumn, Me.SolicitadoPorDataGridViewTextBoxColumn, Me.DescEstadoResources, Me.DescEstadoDataGridViewTextBoxColumn, Me.DescMarcaDataGridViewTextBoxColumn, Me.DescEstiloDataGridViewTextBoxColumn, Me.DescModeloDataGridViewTextBoxColumn, Me.RespondidoPorDataGridViewTextBoxColumn, Me.FechaRespuestaDataGridViewTextBoxColumn, Me.EstadoDataGridViewCheckBoxColumn, Me.PlacaDataGridViewTextBoxColumn, Me.IDVehiculoDataGridViewTextBoxColumn, Me.CodMarcaDataGridViewTextBoxColumn, Me.CodEstiloDataGridViewTextBoxColumn, Me.CodModeloDataGridViewTextBoxColumn, Me.NoVisitaDataGridViewTextBoxColumn, Me.FechaaperturaDataGridViewTextBoxColumn, Me.FechacompromisoDataGridViewTextBoxColumn, Me.CodTipoOrdenDataGridViewTextBoxColumn, Me.NoCotizacionDataGridViewTextBoxColumn, Me.TipoDescDataGridViewTextBoxColumn, Me.ObservacionDataGridViewTextBoxColumn, Me.NoVehiculoDataGridViewTextBoxColumn, Me.HoraCompDataGridViewTextBoxColumn, Me.FechaCompDataGridViewTextBoxColumn})
            Me.dtgDetalles.DataMember = "SCGTA_SP_SelSolicitudEspecifico"
            Me.dtgDetalles.DataSource = SolicitudEspecificosDataset1
            resources.ApplyResources(Me.dtgDetalles, "dtgDetalles")
            Me.dtgDetalles.MultiSelect = False
            Me.dtgDetalles.Name = "dtgDetalles"
            Me.dtgDetalles.ReadOnly = True
            DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(207, Byte), Integer), CType(CType(49, Byte), Integer))
            DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgDetalles.RowsDefaultCellStyle = DataGridViewCellStyle2
            Me.dtgDetalles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
            '
            'IDDataGridViewTextBoxColumn
            '
            Me.IDDataGridViewTextBoxColumn.DataPropertyName = "ID"
            resources.ApplyResources(Me.IDDataGridViewTextBoxColumn, "IDDataGridViewTextBoxColumn")
            Me.IDDataGridViewTextBoxColumn.Name = "IDDataGridViewTextBoxColumn"
            Me.IDDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoOrdenDataGridViewTextBoxColumn
            '
            Me.NoOrdenDataGridViewTextBoxColumn.DataPropertyName = "NoOrden"
            resources.ApplyResources(Me.NoOrdenDataGridViewTextBoxColumn, "NoOrdenDataGridViewTextBoxColumn")
            Me.NoOrdenDataGridViewTextBoxColumn.Name = "NoOrdenDataGridViewTextBoxColumn"
            Me.NoOrdenDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FechaSolicitudDataGridViewTextBoxColumn
            '
            Me.FechaSolicitudDataGridViewTextBoxColumn.DataPropertyName = "FechaSolicitud"
            resources.ApplyResources(Me.FechaSolicitudDataGridViewTextBoxColumn, "FechaSolicitudDataGridViewTextBoxColumn")
            Me.FechaSolicitudDataGridViewTextBoxColumn.Name = "FechaSolicitudDataGridViewTextBoxColumn"
            Me.FechaSolicitudDataGridViewTextBoxColumn.ReadOnly = True
            '
            'SolicitadoPorDataGridViewTextBoxColumn
            '
            Me.SolicitadoPorDataGridViewTextBoxColumn.DataPropertyName = "SolicitadoPor"
            resources.ApplyResources(Me.SolicitadoPorDataGridViewTextBoxColumn, "SolicitadoPorDataGridViewTextBoxColumn")
            Me.SolicitadoPorDataGridViewTextBoxColumn.Name = "SolicitadoPorDataGridViewTextBoxColumn"
            Me.SolicitadoPorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DescEstadoResources
            '
            Me.DescEstadoResources.DataPropertyName = "DescEstadoResources"
            resources.ApplyResources(Me.DescEstadoResources, "DescEstadoResources")
            Me.DescEstadoResources.Name = "DescEstadoResources"
            Me.DescEstadoResources.ReadOnly = True
            '
            'DescEstadoDataGridViewTextBoxColumn
            '
            Me.DescEstadoDataGridViewTextBoxColumn.DataPropertyName = "DescEstado"
            resources.ApplyResources(Me.DescEstadoDataGridViewTextBoxColumn, "DescEstadoDataGridViewTextBoxColumn")
            Me.DescEstadoDataGridViewTextBoxColumn.Name = "DescEstadoDataGridViewTextBoxColumn"
            Me.DescEstadoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DescMarcaDataGridViewTextBoxColumn
            '
            Me.DescMarcaDataGridViewTextBoxColumn.DataPropertyName = "DescMarca"
            resources.ApplyResources(Me.DescMarcaDataGridViewTextBoxColumn, "DescMarcaDataGridViewTextBoxColumn")
            Me.DescMarcaDataGridViewTextBoxColumn.Name = "DescMarcaDataGridViewTextBoxColumn"
            Me.DescMarcaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DescEstiloDataGridViewTextBoxColumn
            '
            Me.DescEstiloDataGridViewTextBoxColumn.DataPropertyName = "DescEstilo"
            resources.ApplyResources(Me.DescEstiloDataGridViewTextBoxColumn, "DescEstiloDataGridViewTextBoxColumn")
            Me.DescEstiloDataGridViewTextBoxColumn.Name = "DescEstiloDataGridViewTextBoxColumn"
            Me.DescEstiloDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DescModeloDataGridViewTextBoxColumn
            '
            Me.DescModeloDataGridViewTextBoxColumn.DataPropertyName = "DescModelo"
            resources.ApplyResources(Me.DescModeloDataGridViewTextBoxColumn, "DescModeloDataGridViewTextBoxColumn")
            Me.DescModeloDataGridViewTextBoxColumn.Name = "DescModeloDataGridViewTextBoxColumn"
            Me.DescModeloDataGridViewTextBoxColumn.ReadOnly = True
            '
            'RespondidoPorDataGridViewTextBoxColumn
            '
            Me.RespondidoPorDataGridViewTextBoxColumn.DataPropertyName = "RespondidoPor"
            resources.ApplyResources(Me.RespondidoPorDataGridViewTextBoxColumn, "RespondidoPorDataGridViewTextBoxColumn")
            Me.RespondidoPorDataGridViewTextBoxColumn.Name = "RespondidoPorDataGridViewTextBoxColumn"
            Me.RespondidoPorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FechaRespuestaDataGridViewTextBoxColumn
            '
            Me.FechaRespuestaDataGridViewTextBoxColumn.DataPropertyName = "FechaRespuesta"
            resources.ApplyResources(Me.FechaRespuestaDataGridViewTextBoxColumn, "FechaRespuestaDataGridViewTextBoxColumn")
            Me.FechaRespuestaDataGridViewTextBoxColumn.Name = "FechaRespuestaDataGridViewTextBoxColumn"
            Me.FechaRespuestaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EstadoDataGridViewCheckBoxColumn
            '
            Me.EstadoDataGridViewCheckBoxColumn.DataPropertyName = "Estado"
            resources.ApplyResources(Me.EstadoDataGridViewCheckBoxColumn, "EstadoDataGridViewCheckBoxColumn")
            Me.EstadoDataGridViewCheckBoxColumn.Name = "EstadoDataGridViewCheckBoxColumn"
            Me.EstadoDataGridViewCheckBoxColumn.ReadOnly = True
            '
            'PlacaDataGridViewTextBoxColumn
            '
            Me.PlacaDataGridViewTextBoxColumn.DataPropertyName = "Placa"
            resources.ApplyResources(Me.PlacaDataGridViewTextBoxColumn, "PlacaDataGridViewTextBoxColumn")
            Me.PlacaDataGridViewTextBoxColumn.Name = "PlacaDataGridViewTextBoxColumn"
            Me.PlacaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'IDVehiculoDataGridViewTextBoxColumn
            '
            Me.IDVehiculoDataGridViewTextBoxColumn.DataPropertyName = "IDVehiculo"
            resources.ApplyResources(Me.IDVehiculoDataGridViewTextBoxColumn, "IDVehiculoDataGridViewTextBoxColumn")
            Me.IDVehiculoDataGridViewTextBoxColumn.Name = "IDVehiculoDataGridViewTextBoxColumn"
            Me.IDVehiculoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodMarcaDataGridViewTextBoxColumn
            '
            Me.CodMarcaDataGridViewTextBoxColumn.DataPropertyName = "CodMarca"
            resources.ApplyResources(Me.CodMarcaDataGridViewTextBoxColumn, "CodMarcaDataGridViewTextBoxColumn")
            Me.CodMarcaDataGridViewTextBoxColumn.Name = "CodMarcaDataGridViewTextBoxColumn"
            Me.CodMarcaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodEstiloDataGridViewTextBoxColumn
            '
            Me.CodEstiloDataGridViewTextBoxColumn.DataPropertyName = "CodEstilo"
            resources.ApplyResources(Me.CodEstiloDataGridViewTextBoxColumn, "CodEstiloDataGridViewTextBoxColumn")
            Me.CodEstiloDataGridViewTextBoxColumn.Name = "CodEstiloDataGridViewTextBoxColumn"
            Me.CodEstiloDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodModeloDataGridViewTextBoxColumn
            '
            Me.CodModeloDataGridViewTextBoxColumn.DataPropertyName = "CodModelo"
            resources.ApplyResources(Me.CodModeloDataGridViewTextBoxColumn, "CodModeloDataGridViewTextBoxColumn")
            Me.CodModeloDataGridViewTextBoxColumn.Name = "CodModeloDataGridViewTextBoxColumn"
            Me.CodModeloDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoVisitaDataGridViewTextBoxColumn
            '
            Me.NoVisitaDataGridViewTextBoxColumn.DataPropertyName = "NoVisita"
            resources.ApplyResources(Me.NoVisitaDataGridViewTextBoxColumn, "NoVisitaDataGridViewTextBoxColumn")
            Me.NoVisitaDataGridViewTextBoxColumn.Name = "NoVisitaDataGridViewTextBoxColumn"
            Me.NoVisitaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FechaaperturaDataGridViewTextBoxColumn
            '
            Me.FechaaperturaDataGridViewTextBoxColumn.DataPropertyName = "Fecha_apertura"
            resources.ApplyResources(Me.FechaaperturaDataGridViewTextBoxColumn, "FechaaperturaDataGridViewTextBoxColumn")
            Me.FechaaperturaDataGridViewTextBoxColumn.Name = "FechaaperturaDataGridViewTextBoxColumn"
            Me.FechaaperturaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FechacompromisoDataGridViewTextBoxColumn
            '
            Me.FechacompromisoDataGridViewTextBoxColumn.DataPropertyName = "Fecha_compromiso"
            resources.ApplyResources(Me.FechacompromisoDataGridViewTextBoxColumn, "FechacompromisoDataGridViewTextBoxColumn")
            Me.FechacompromisoDataGridViewTextBoxColumn.Name = "FechacompromisoDataGridViewTextBoxColumn"
            Me.FechacompromisoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodTipoOrdenDataGridViewTextBoxColumn
            '
            Me.CodTipoOrdenDataGridViewTextBoxColumn.DataPropertyName = "CodTipoOrden"
            resources.ApplyResources(Me.CodTipoOrdenDataGridViewTextBoxColumn, "CodTipoOrdenDataGridViewTextBoxColumn")
            Me.CodTipoOrdenDataGridViewTextBoxColumn.Name = "CodTipoOrdenDataGridViewTextBoxColumn"
            Me.CodTipoOrdenDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoCotizacionDataGridViewTextBoxColumn
            '
            Me.NoCotizacionDataGridViewTextBoxColumn.DataPropertyName = "NoCotizacion"
            resources.ApplyResources(Me.NoCotizacionDataGridViewTextBoxColumn, "NoCotizacionDataGridViewTextBoxColumn")
            Me.NoCotizacionDataGridViewTextBoxColumn.Name = "NoCotizacionDataGridViewTextBoxColumn"
            Me.NoCotizacionDataGridViewTextBoxColumn.ReadOnly = True
            '
            'TipoDescDataGridViewTextBoxColumn
            '
            Me.TipoDescDataGridViewTextBoxColumn.DataPropertyName = "TipoDesc"
            resources.ApplyResources(Me.TipoDescDataGridViewTextBoxColumn, "TipoDescDataGridViewTextBoxColumn")
            Me.TipoDescDataGridViewTextBoxColumn.Name = "TipoDescDataGridViewTextBoxColumn"
            Me.TipoDescDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ObservacionDataGridViewTextBoxColumn
            '
            Me.ObservacionDataGridViewTextBoxColumn.DataPropertyName = "Observacion"
            resources.ApplyResources(Me.ObservacionDataGridViewTextBoxColumn, "ObservacionDataGridViewTextBoxColumn")
            Me.ObservacionDataGridViewTextBoxColumn.Name = "ObservacionDataGridViewTextBoxColumn"
            Me.ObservacionDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoVehiculoDataGridViewTextBoxColumn
            '
            Me.NoVehiculoDataGridViewTextBoxColumn.DataPropertyName = "NoVehiculo"
            resources.ApplyResources(Me.NoVehiculoDataGridViewTextBoxColumn, "NoVehiculoDataGridViewTextBoxColumn")
            Me.NoVehiculoDataGridViewTextBoxColumn.Name = "NoVehiculoDataGridViewTextBoxColumn"
            Me.NoVehiculoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'HoraCompDataGridViewTextBoxColumn
            '
            Me.HoraCompDataGridViewTextBoxColumn.DataPropertyName = "Hora_Comp"
            resources.ApplyResources(Me.HoraCompDataGridViewTextBoxColumn, "HoraCompDataGridViewTextBoxColumn")
            Me.HoraCompDataGridViewTextBoxColumn.Name = "HoraCompDataGridViewTextBoxColumn"
            Me.HoraCompDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FechaCompDataGridViewTextBoxColumn
            '
            Me.FechaCompDataGridViewTextBoxColumn.DataPropertyName = "Fecha_Comp"
            resources.ApplyResources(Me.FechaCompDataGridViewTextBoxColumn, "FechaCompDataGridViewTextBoxColumn")
            Me.FechaCompDataGridViewTextBoxColumn.Name = "FechaCompDataGridViewTextBoxColumn"
            Me.FechaCompDataGridViewTextBoxColumn.ReadOnly = True
            '
            'tlbSolicitudespecificos
            '
            resources.ApplyResources(Me.tlbSolicitudespecificos, "tlbSolicitudespecificos")
            Me.tlbSolicitudespecificos.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.tlbSolicitudespecificos.Name = "tlbSolicitudespecificos"
            '
            'grpOrdenInfo
            '
            Me.grpOrdenInfo.Controls.Add(Me.cboEstado)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine6)
            Me.grpOrdenInfo.Controls.Add(Me.chkEstado)
            Me.grpOrdenInfo.Controls.Add(Me.Label1)
            Me.grpOrdenInfo.Controls.Add(Me.txtNoVehiculo)
            Me.grpOrdenInfo.Controls.Add(Me.lblNoVehiculo)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine2)
            Me.grpOrdenInfo.Controls.Add(Me.txtPlaca)
            Me.grpOrdenInfo.Controls.Add(Me.Label6)
            Me.grpOrdenInfo.Controls.Add(Me.txtNoSolicitud)
            Me.grpOrdenInfo.Controls.Add(Me.Label3)
            Me.grpOrdenInfo.Controls.Add(Me.lblNoSolicitud)
            Me.grpOrdenInfo.Controls.Add(Me.Panel6)
            Me.grpOrdenInfo.Controls.Add(Me.Panel7)
            Me.grpOrdenInfo.Controls.Add(Me.dtpCompromisoini)
            Me.grpOrdenInfo.Controls.Add(Me.dtpAperturaini)
            Me.grpOrdenInfo.Controls.Add(Me.Panel3)
            Me.grpOrdenInfo.Controls.Add(Me.dtpCompromisofin)
            Me.grpOrdenInfo.Controls.Add(Me.Panel4)
            Me.grpOrdenInfo.Controls.Add(Me.dtpAperturafin)
            Me.grpOrdenInfo.Controls.Add(Me.Panel9)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine9)
            Me.grpOrdenInfo.Controls.Add(Me.Panel10)
            Me.grpOrdenInfo.Controls.Add(Me.Label2)
            Me.grpOrdenInfo.Controls.Add(Me.chkRespuesta)
            Me.grpOrdenInfo.Controls.Add(Me.chkSolicitud)
            Me.grpOrdenInfo.Controls.Add(Me.txtNoVisita)
            Me.grpOrdenInfo.Controls.Add(Me.cboMarca)
            Me.grpOrdenInfo.Controls.Add(Me.cboModelo)
            Me.grpOrdenInfo.Controls.Add(Me.cboEstilo)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine5)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine8)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine7)
            Me.grpOrdenInfo.Controls.Add(Me.chkModelo)
            Me.grpOrdenInfo.Controls.Add(Me.chkEstilo)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine3)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine1)
            Me.grpOrdenInfo.Controls.Add(Me.txtNoOrden)
            Me.grpOrdenInfo.Controls.Add(Me.lblNoOrden)
            Me.grpOrdenInfo.Controls.Add(Me.lblNoexpediente)
            Me.grpOrdenInfo.Controls.Add(Me.chkMarca)
            resources.ApplyResources(Me.grpOrdenInfo, "grpOrdenInfo")
            Me.grpOrdenInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.grpOrdenInfo.Name = "grpOrdenInfo"
            Me.grpOrdenInfo.TabStop = False
            '
            'cboEstado
            '
            Me.cboEstado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstado.EstiloSBO = True
            resources.ApplyResources(Me.cboEstado, "cboEstado")
            Me.cboEstado.ForeColor = System.Drawing.Color.Black
            Me.cboEstado.Name = "cboEstado"
            '
            'lblLine6
            '
            Me.lblLine6.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLine6, "lblLine6")
            Me.lblLine6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine6.Name = "lblLine6"
            '
            'chkEstado
            '
            resources.ApplyResources(Me.chkEstado, "chkEstado")
            Me.chkEstado.Name = "chkEstado"
            Me.chkEstado.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label1.Name = "Label1"
            '
            'txtNoVehiculo
            '
            Me.txtNoVehiculo.AceptaNegativos = False
            Me.txtNoVehiculo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoVehiculo.EstiloSBO = True
            resources.ApplyResources(Me.txtNoVehiculo, "txtNoVehiculo")
            Me.txtNoVehiculo.ForeColor = System.Drawing.Color.Black
            Me.txtNoVehiculo.MaxDecimales = 0
            Me.txtNoVehiculo.MaxEnteros = 0
            Me.txtNoVehiculo.Millares = False
            Me.txtNoVehiculo.Name = "txtNoVehiculo"
            Me.txtNoVehiculo.Size_AdjustableHeight = 20
            Me.txtNoVehiculo.TeclasDeshacer = True
            Me.txtNoVehiculo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblNoVehiculo
            '
            resources.ApplyResources(Me.lblNoVehiculo, "lblNoVehiculo")
            Me.lblNoVehiculo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoVehiculo.Name = "lblNoVehiculo"
            '
            'lblLine2
            '
            Me.lblLine2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine2, "lblLine2")
            Me.lblLine2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine2.Name = "lblLine2"
            '
            'txtPlaca
            '
            Me.txtPlaca.AceptaNegativos = False
            Me.txtPlaca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtPlaca.EstiloSBO = True
            resources.ApplyResources(Me.txtPlaca, "txtPlaca")
            Me.txtPlaca.ForeColor = System.Drawing.Color.Black
            Me.txtPlaca.MaxDecimales = 0
            Me.txtPlaca.MaxEnteros = 0
            Me.txtPlaca.Millares = False
            Me.txtPlaca.Name = "txtPlaca"
            Me.txtPlaca.Size_AdjustableHeight = 20
            Me.txtPlaca.TeclasDeshacer = True
            Me.txtPlaca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label6
            '
            resources.ApplyResources(Me.Label6, "Label6")
            Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label6.Name = "Label6"
            '
            'txtNoSolicitud
            '
            Me.txtNoSolicitud.AceptaNegativos = False
            Me.txtNoSolicitud.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoSolicitud.EstiloSBO = True
            resources.ApplyResources(Me.txtNoSolicitud, "txtNoSolicitud")
            Me.txtNoSolicitud.ForeColor = System.Drawing.Color.Black
            Me.txtNoSolicitud.MaxDecimales = 0
            Me.txtNoSolicitud.MaxEnteros = 0
            Me.txtNoSolicitud.Millares = False
            Me.txtNoSolicitud.Name = "txtNoSolicitud"
            Me.txtNoSolicitud.Size_AdjustableHeight = 20
            Me.txtNoSolicitud.TeclasDeshacer = True
            Me.txtNoSolicitud.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.Numeric
            '
            'Label3
            '
            Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label3.Name = "Label3"
            '
            'lblNoSolicitud
            '
            resources.ApplyResources(Me.lblNoSolicitud, "lblNoSolicitud")
            Me.lblNoSolicitud.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoSolicitud.Name = "lblNoSolicitud"
            '
            'Panel6
            '
            resources.ApplyResources(Me.Panel6, "Panel6")
            Me.Panel6.Name = "Panel6"
            '
            'Panel7
            '
            resources.ApplyResources(Me.Panel7, "Panel7")
            Me.Panel7.Name = "Panel7"
            '
            'dtpCompromisoini
            '
            Me.dtpCompromisoini.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCompromisoini.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCompromisoini.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCompromisoini.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCompromisoini.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpCompromisoini, "dtpCompromisoini")
            Me.dtpCompromisoini.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpCompromisoini.Name = "dtpCompromisoini"
            Me.dtpCompromisoini.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'dtpAperturaini
            '
            Me.dtpAperturaini.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpAperturaini.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpAperturaini.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpAperturaini.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpAperturaini.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpAperturaini, "dtpAperturaini")
            Me.dtpAperturaini.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpAperturaini.MaxDate = New Date(3000, 12, 31, 0, 0, 0, 0)
            Me.dtpAperturaini.Name = "dtpAperturaini"
            Me.dtpAperturaini.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'Panel3
            '
            resources.ApplyResources(Me.Panel3, "Panel3")
            Me.Panel3.Name = "Panel3"
            '
            'dtpCompromisofin
            '
            Me.dtpCompromisofin.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCompromisofin.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCompromisofin.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCompromisofin.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCompromisofin.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpCompromisofin, "dtpCompromisofin")
            Me.dtpCompromisofin.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpCompromisofin.Name = "dtpCompromisofin"
            Me.dtpCompromisofin.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'Panel4
            '
            resources.ApplyResources(Me.Panel4, "Panel4")
            Me.Panel4.Name = "Panel4"
            '
            'dtpAperturafin
            '
            Me.dtpAperturafin.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpAperturafin.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpAperturafin.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpAperturafin.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpAperturafin.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpAperturafin, "dtpAperturafin")
            Me.dtpAperturafin.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpAperturafin.MaxDate = New Date(3000, 12, 31, 0, 0, 0, 0)
            Me.dtpAperturafin.Name = "dtpAperturafin"
            Me.dtpAperturafin.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'Panel9
            '
            resources.ApplyResources(Me.Panel9, "Panel9")
            Me.Panel9.Name = "Panel9"
            '
            'lblLine9
            '
            Me.lblLine9.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLine9, "lblLine9")
            Me.lblLine9.Name = "lblLine9"
            '
            'Panel10
            '
            resources.ApplyResources(Me.Panel10, "Panel10")
            Me.Panel10.Name = "Panel10"
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'chkRespuesta
            '
            resources.ApplyResources(Me.chkRespuesta, "chkRespuesta")
            Me.chkRespuesta.Name = "chkRespuesta"
            Me.chkRespuesta.UseVisualStyleBackColor = True
            '
            'chkSolicitud
            '
            resources.ApplyResources(Me.chkSolicitud, "chkSolicitud")
            Me.chkSolicitud.Name = "chkSolicitud"
            Me.chkSolicitud.UseVisualStyleBackColor = True
            '
            'txtNoVisita
            '
            Me.txtNoVisita.AceptaNegativos = False
            Me.txtNoVisita.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoVisita.EstiloSBO = True
            resources.ApplyResources(Me.txtNoVisita, "txtNoVisita")
            Me.txtNoVisita.ForeColor = System.Drawing.Color.Black
            Me.txtNoVisita.MaxDecimales = 0
            Me.txtNoVisita.MaxEnteros = 0
            Me.txtNoVisita.Millares = False
            Me.txtNoVisita.Name = "txtNoVisita"
            Me.txtNoVisita.Size_AdjustableHeight = 20
            Me.txtNoVisita.TeclasDeshacer = True
            Me.txtNoVisita.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.Numeric
            '
            'cboMarca
            '
            Me.cboMarca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboMarca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboMarca.EstiloSBO = True
            resources.ApplyResources(Me.cboMarca, "cboMarca")
            Me.cboMarca.ForeColor = System.Drawing.Color.Black
            Me.cboMarca.Name = "cboMarca"
            '
            'cboModelo
            '
            Me.cboModelo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboModelo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboModelo.EstiloSBO = True
            resources.ApplyResources(Me.cboModelo, "cboModelo")
            Me.cboModelo.ForeColor = System.Drawing.Color.Black
            Me.cboModelo.Name = "cboModelo"
            '
            'cboEstilo
            '
            Me.cboEstilo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboEstilo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstilo.EstiloSBO = True
            resources.ApplyResources(Me.cboEstilo, "cboEstilo")
            Me.cboEstilo.ForeColor = System.Drawing.Color.Black
            Me.cboEstilo.Name = "cboEstilo"
            '
            'lblLine5
            '
            Me.lblLine5.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine5, "lblLine5")
            Me.lblLine5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine5.Name = "lblLine5"
            '
            'lblLine8
            '
            Me.lblLine8.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine8, "lblLine8")
            Me.lblLine8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine8.Name = "lblLine8"
            '
            'lblLine7
            '
            Me.lblLine7.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine7, "lblLine7")
            Me.lblLine7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine7.Name = "lblLine7"
            '
            'chkModelo
            '
            resources.ApplyResources(Me.chkModelo, "chkModelo")
            Me.chkModelo.Name = "chkModelo"
            Me.chkModelo.UseVisualStyleBackColor = True
            '
            'chkEstilo
            '
            resources.ApplyResources(Me.chkEstilo, "chkEstilo")
            Me.chkEstilo.Name = "chkEstilo"
            Me.chkEstilo.UseVisualStyleBackColor = True
            '
            'lblLine3
            '
            Me.lblLine3.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine3, "lblLine3")
            Me.lblLine3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine3.Name = "lblLine3"
            '
            'lblLine1
            '
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine1.Name = "lblLine1"
            '
            'txtNoOrden
            '
            Me.txtNoOrden.AceptaNegativos = False
            Me.txtNoOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoOrden.EstiloSBO = True
            resources.ApplyResources(Me.txtNoOrden, "txtNoOrden")
            Me.txtNoOrden.ForeColor = System.Drawing.Color.Black
            Me.txtNoOrden.MaxDecimales = 0
            Me.txtNoOrden.MaxEnteros = 0
            Me.txtNoOrden.Millares = False
            Me.txtNoOrden.Name = "txtNoOrden"
            Me.txtNoOrden.Size_AdjustableHeight = 20
            Me.txtNoOrden.TeclasDeshacer = True
            Me.txtNoOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblNoOrden
            '
            resources.ApplyResources(Me.lblNoOrden, "lblNoOrden")
            Me.lblNoOrden.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoOrden.Name = "lblNoOrden"
            '
            'lblNoexpediente
            '
            resources.ApplyResources(Me.lblNoexpediente, "lblNoexpediente")
            Me.lblNoexpediente.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoexpediente.Name = "lblNoexpediente"
            '
            'chkMarca
            '
            resources.ApplyResources(Me.chkMarca, "chkMarca")
            Me.chkMarca.Name = "chkMarca"
            Me.chkMarca.UseVisualStyleBackColor = True
            '
            'frmBusquedaSolicitudesEspecificos
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.grpOrdenInfo)
            Me.Controls.Add(Me.grpItems)
            Me.Controls.Add(Me.tlbSolicitudespecificos)
            Me.MaximizeBox = False
            Me.Name = "frmBusquedaSolicitudesEspecificos"
            CType(SolicitudEspecificosDataset1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpItems.ResumeLayout(False)
            CType(Me.dtgDetalles, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpOrdenInfo.ResumeLayout(False)
            Me.grpOrdenInfo.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents grpItems As System.Windows.Forms.GroupBox
        Friend WithEvents dtgDetalles As System.Windows.Forms.DataGridView
        Friend WithEvents tlbSolicitudespecificos As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents grpOrdenInfo As System.Windows.Forms.GroupBox
        Public WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents lblNoSolicitud As System.Windows.Forms.Label
        Friend WithEvents Panel7 As System.Windows.Forms.Panel
        Friend WithEvents dtpAperturaini As System.Windows.Forms.DateTimePicker
        Friend WithEvents Panel3 As System.Windows.Forms.Panel
        Friend WithEvents dtpCompromisofin As System.Windows.Forms.DateTimePicker
        Friend WithEvents Panel4 As System.Windows.Forms.Panel
        Friend WithEvents dtpAperturafin As System.Windows.Forms.DateTimePicker
        Friend WithEvents Panel10 As System.Windows.Forms.Panel
        Public WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents chkRespuesta As System.Windows.Forms.CheckBox
        Friend WithEvents chkSolicitud As System.Windows.Forms.CheckBox
        Friend WithEvents txtNoVisita As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents cboMarca As SCGComboBox.SCGComboBox
        Friend WithEvents cboModelo As SCGComboBox.SCGComboBox
        Friend WithEvents cboEstilo As SCGComboBox.SCGComboBox
        Public WithEvents lblLine5 As System.Windows.Forms.Label
        Public WithEvents lblLine8 As System.Windows.Forms.Label
        Public WithEvents lblLine7 As System.Windows.Forms.Label
        Friend WithEvents chkModelo As System.Windows.Forms.CheckBox
        Friend WithEvents chkEstilo As System.Windows.Forms.CheckBox
        Public WithEvents lblLine3 As System.Windows.Forms.Label
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Friend WithEvents txtNoOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblNoOrden As System.Windows.Forms.Label
        Friend WithEvents lblNoexpediente As System.Windows.Forms.Label
        Friend WithEvents chkMarca As System.Windows.Forms.CheckBox
        Friend WithEvents txtNoSolicitud As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents cboEstado As SCGComboBox.SCGComboBox
        Public WithEvents lblLine6 As System.Windows.Forms.Label
        Friend WithEvents chkEstado As System.Windows.Forms.CheckBox
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents txtNoVehiculo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblNoVehiculo As System.Windows.Forms.Label
        Public WithEvents lblLine2 As System.Windows.Forms.Label
        Friend WithEvents txtPlaca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents IDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaSolicitudDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents SolicitadoPorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescEstadoResources As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescEstadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescMarcaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescEstiloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescModeloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents RespondidoPorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaRespuestaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents PlacaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodMarcaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodEstiloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodModeloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoVisitaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaaperturaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechacompromisoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodTipoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoCotizacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents TipoDescDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ObservacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents HoraCompDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaCompDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Public WithEvents lblLine9 As System.Windows.Forms.Label
        Friend WithEvents Panel6 As System.Windows.Forms.Panel
        Friend WithEvents dtpCompromisoini As System.Windows.Forms.DateTimePicker
        Friend WithEvents Panel9 As System.Windows.Forms.Panel
    End Class
End Namespace