Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmDetalleVisita
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
            Dim OrdenTrabajoDatasetGrid As DMSOneFramework.OrdenTrabajoDataset
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetalleVisita))
            Me.grpCitas = New System.Windows.Forms.GroupBox
            Me.dtgOrdenes = New System.Windows.Forms.DataGridView
            Me.NoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.TipoDescDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescMarcaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescEstiloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescModeloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoDescDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescipcionEstadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechacierreDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoVisitaDescDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.PlacaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IDVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodMarcaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodEstiloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodModeloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ConoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoVisitaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaaperturaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechacompromisoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodTipoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ObservacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoVisitaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CheckDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ClienteFacturarDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.MontoReparacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.AsesorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoCotizacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.HoraCompDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaCompDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.OTPadreDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CardCodeOrigDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CardNameOrigDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.VINDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.AnoVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NombreAsesorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.lblLine9 = New System.Windows.Forms.Label
            Me.lblFechaReporte = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.lblEstado = New System.Windows.Forms.Label
            Me.txtNoVisita = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label1 = New System.Windows.Forms.Label
            Me.lblNumeroVisita = New System.Windows.Forms.Label
            Me.txtNombreCliente = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtNoVehiculo = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label7 = New System.Windows.Forms.Label
            Me.lblNoVehiculo = New System.Windows.Forms.Label
            Me.txtCodCliente = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label2 = New System.Windows.Forms.Label
            Me.lblCliente = New System.Windows.Forms.Label
            Me.Label5 = New System.Windows.Forms.Label
            Me.lblMarca = New System.Windows.Forms.Label
            Me.Label6 = New System.Windows.Forms.Label
            Me.lblEstilo = New System.Windows.Forms.Label
            Me.txtMarca = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtEstilo = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtFechaApertura = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.cboEstadoVisita = New SCGComboBox.SCGComboBox
            Me.txtIdentCliente = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label10 = New System.Windows.Forms.Label
            Me.lblIdentCliente = New System.Windows.Forms.Label
            Me.txtModelo = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label11 = New System.Windows.Forms.Label
            Me.lblModelo = New System.Windows.Forms.Label
            Me.lblLine4 = New System.Windows.Forms.Label
            Me.txtCono = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label8 = New System.Windows.Forms.Label
            Me.lblLine3 = New System.Windows.Forms.Label
            Me.txtPlaca = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label13 = New System.Windows.Forms.Label
            Me.txtNombreAsesor = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label15 = New System.Windows.Forms.Label
            Me.txtCodAsesor = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.lblAsesor = New System.Windows.Forms.Label
            Me.Label12 = New System.Windows.Forms.Label
            Me.dtpCierre = New System.Windows.Forms.DateTimePicker
            Me.Label4 = New System.Windows.Forms.Label
            Me.txtHoraCompromiso = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtFechaCompromiso = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label9 = New System.Windows.Forms.Label
            Me.lblFechaCompromiso = New System.Windows.Forms.Label
            Me.txtHoraApertura = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.btnAceptar = New System.Windows.Forms.Button
            Me.btnCerrar = New System.Windows.Forms.Button
            Me.Panel7 = New System.Windows.Forms.Panel
            Me.btnArchivos = New System.Windows.Forms.Button
            Me.VisualizarUDFVisita = New ControlUDF.VisualizarUDF
            Me.SeccionesDataset1 = New DMSOneFramework.SeccionesDataset
            OrdenTrabajoDatasetGrid = New DMSOneFramework.OrdenTrabajoDataset
            CType(OrdenTrabajoDatasetGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpCitas.SuspendLayout()
            CType(Me.dtgOrdenes, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.SeccionesDataset1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'OrdenTrabajoDatasetGrid
            '
            OrdenTrabajoDatasetGrid.DataSetName = "OrdenTrabajoDataset"
            OrdenTrabajoDatasetGrid.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'grpCitas
            '
            Me.grpCitas.BackColor = System.Drawing.SystemColors.Control
            Me.grpCitas.Controls.Add(Me.dtgOrdenes)
            resources.ApplyResources(Me.grpCitas, "grpCitas")
            Me.grpCitas.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.grpCitas.Name = "grpCitas"
            Me.grpCitas.TabStop = False
            '
            'dtgOrdenes
            '
            Me.dtgOrdenes.AllowUserToAddRows = False
            Me.dtgOrdenes.AllowUserToDeleteRows = False
            Me.dtgOrdenes.AutoGenerateColumns = False
            Me.dtgOrdenes.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgOrdenes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgOrdenes.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NoOrdenDataGridViewTextBoxColumn, Me.TipoDescDataGridViewTextBoxColumn, Me.DescMarcaDataGridViewTextBoxColumn, Me.DescEstiloDataGridViewTextBoxColumn, Me.DescModeloDataGridViewTextBoxColumn, Me.EstadoDescDataGridViewTextBoxColumn, Me.DescipcionEstadoDataGridViewTextBoxColumn, Me.FechacierreDataGridViewTextBoxColumn, Me.EstadoVisitaDescDataGridViewTextBoxColumn, Me.PlacaDataGridViewTextBoxColumn, Me.IDVehiculoDataGridViewTextBoxColumn, Me.CodMarcaDataGridViewTextBoxColumn, Me.CodEstiloDataGridViewTextBoxColumn, Me.CodModeloDataGridViewTextBoxColumn, Me.EstadoDataGridViewTextBoxColumn, Me.ConoDataGridViewTextBoxColumn, Me.NoVisitaDataGridViewTextBoxColumn, Me.FechaaperturaDataGridViewTextBoxColumn, Me.FechacompromisoDataGridViewTextBoxColumn, Me.CodTipoOrdenDataGridViewTextBoxColumn, Me.ObservacionDataGridViewTextBoxColumn, Me.EstadoVisitaDataGridViewTextBoxColumn, Me.NoVehiculoDataGridViewTextBoxColumn, Me.CheckDataGridViewTextBoxColumn, Me.ClienteFacturarDataGridViewTextBoxColumn, Me.MontoReparacionDataGridViewTextBoxColumn, Me.AsesorDataGridViewTextBoxColumn, Me.NoCotizacionDataGridViewTextBoxColumn, Me.HoraCompDataGridViewTextBoxColumn, Me.FechaCompDataGridViewTextBoxColumn, Me.OTPadreDataGridViewTextBoxColumn, Me.CardCodeOrigDataGridViewTextBoxColumn, Me.CardNameOrigDataGridViewTextBoxColumn, Me.VINDataGridViewTextBoxColumn, Me.AnoVehiculoDataGridViewTextBoxColumn, Me.NombreAsesorDataGridViewTextBoxColumn})
            Me.dtgOrdenes.DataMember = "SCGTA_TB_Orden"
            Me.dtgOrdenes.DataSource = OrdenTrabajoDatasetGrid
            Me.dtgOrdenes.GridColor = System.Drawing.Color.Silver
            resources.ApplyResources(Me.dtgOrdenes, "dtgOrdenes")
            Me.dtgOrdenes.Name = "dtgOrdenes"
            Me.dtgOrdenes.ReadOnly = True
            '
            'NoOrdenDataGridViewTextBoxColumn
            '
            Me.NoOrdenDataGridViewTextBoxColumn.DataPropertyName = "NoOrden"
            resources.ApplyResources(Me.NoOrdenDataGridViewTextBoxColumn, "NoOrdenDataGridViewTextBoxColumn")
            Me.NoOrdenDataGridViewTextBoxColumn.Name = "NoOrdenDataGridViewTextBoxColumn"
            Me.NoOrdenDataGridViewTextBoxColumn.ReadOnly = True
            '
            'TipoDescDataGridViewTextBoxColumn
            '
            Me.TipoDescDataGridViewTextBoxColumn.DataPropertyName = "TipoDesc"
            resources.ApplyResources(Me.TipoDescDataGridViewTextBoxColumn, "TipoDescDataGridViewTextBoxColumn")
            Me.TipoDescDataGridViewTextBoxColumn.Name = "TipoDescDataGridViewTextBoxColumn"
            Me.TipoDescDataGridViewTextBoxColumn.ReadOnly = True
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
            'EstadoDescDataGridViewTextBoxColumn
            '
            Me.EstadoDescDataGridViewTextBoxColumn.DataPropertyName = "EstadoDesc"
            resources.ApplyResources(Me.EstadoDescDataGridViewTextBoxColumn, "EstadoDescDataGridViewTextBoxColumn")
            Me.EstadoDescDataGridViewTextBoxColumn.Name = "EstadoDescDataGridViewTextBoxColumn"
            Me.EstadoDescDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DescipcionEstadoDataGridViewTextBoxColumn
            '
            Me.DescipcionEstadoDataGridViewTextBoxColumn.DataPropertyName = "DescipcionEstado"
            resources.ApplyResources(Me.DescipcionEstadoDataGridViewTextBoxColumn, "DescipcionEstadoDataGridViewTextBoxColumn")
            Me.DescipcionEstadoDataGridViewTextBoxColumn.Name = "DescipcionEstadoDataGridViewTextBoxColumn"
            Me.DescipcionEstadoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FechacierreDataGridViewTextBoxColumn
            '
            Me.FechacierreDataGridViewTextBoxColumn.DataPropertyName = "Fecha_cierre"
            resources.ApplyResources(Me.FechacierreDataGridViewTextBoxColumn, "FechacierreDataGridViewTextBoxColumn")
            Me.FechacierreDataGridViewTextBoxColumn.Name = "FechacierreDataGridViewTextBoxColumn"
            Me.FechacierreDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EstadoVisitaDescDataGridViewTextBoxColumn
            '
            Me.EstadoVisitaDescDataGridViewTextBoxColumn.DataPropertyName = "EstadoVisitaDesc"
            resources.ApplyResources(Me.EstadoVisitaDescDataGridViewTextBoxColumn, "EstadoVisitaDescDataGridViewTextBoxColumn")
            Me.EstadoVisitaDescDataGridViewTextBoxColumn.Name = "EstadoVisitaDescDataGridViewTextBoxColumn"
            Me.EstadoVisitaDescDataGridViewTextBoxColumn.ReadOnly = True
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
            'EstadoDataGridViewTextBoxColumn
            '
            Me.EstadoDataGridViewTextBoxColumn.DataPropertyName = "Estado"
            resources.ApplyResources(Me.EstadoDataGridViewTextBoxColumn, "EstadoDataGridViewTextBoxColumn")
            Me.EstadoDataGridViewTextBoxColumn.Name = "EstadoDataGridViewTextBoxColumn"
            Me.EstadoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ConoDataGridViewTextBoxColumn
            '
            Me.ConoDataGridViewTextBoxColumn.DataPropertyName = "Cono"
            resources.ApplyResources(Me.ConoDataGridViewTextBoxColumn, "ConoDataGridViewTextBoxColumn")
            Me.ConoDataGridViewTextBoxColumn.Name = "ConoDataGridViewTextBoxColumn"
            Me.ConoDataGridViewTextBoxColumn.ReadOnly = True
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
            'ObservacionDataGridViewTextBoxColumn
            '
            Me.ObservacionDataGridViewTextBoxColumn.DataPropertyName = "Observacion"
            resources.ApplyResources(Me.ObservacionDataGridViewTextBoxColumn, "ObservacionDataGridViewTextBoxColumn")
            Me.ObservacionDataGridViewTextBoxColumn.Name = "ObservacionDataGridViewTextBoxColumn"
            Me.ObservacionDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EstadoVisitaDataGridViewTextBoxColumn
            '
            Me.EstadoVisitaDataGridViewTextBoxColumn.DataPropertyName = "EstadoVisita"
            resources.ApplyResources(Me.EstadoVisitaDataGridViewTextBoxColumn, "EstadoVisitaDataGridViewTextBoxColumn")
            Me.EstadoVisitaDataGridViewTextBoxColumn.Name = "EstadoVisitaDataGridViewTextBoxColumn"
            Me.EstadoVisitaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoVehiculoDataGridViewTextBoxColumn
            '
            Me.NoVehiculoDataGridViewTextBoxColumn.DataPropertyName = "NoVehiculo"
            resources.ApplyResources(Me.NoVehiculoDataGridViewTextBoxColumn, "NoVehiculoDataGridViewTextBoxColumn")
            Me.NoVehiculoDataGridViewTextBoxColumn.Name = "NoVehiculoDataGridViewTextBoxColumn"
            Me.NoVehiculoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CheckDataGridViewTextBoxColumn
            '
            Me.CheckDataGridViewTextBoxColumn.DataPropertyName = "Check"
            resources.ApplyResources(Me.CheckDataGridViewTextBoxColumn, "CheckDataGridViewTextBoxColumn")
            Me.CheckDataGridViewTextBoxColumn.Name = "CheckDataGridViewTextBoxColumn"
            Me.CheckDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ClienteFacturarDataGridViewTextBoxColumn
            '
            Me.ClienteFacturarDataGridViewTextBoxColumn.DataPropertyName = "ClienteFacturar"
            resources.ApplyResources(Me.ClienteFacturarDataGridViewTextBoxColumn, "ClienteFacturarDataGridViewTextBoxColumn")
            Me.ClienteFacturarDataGridViewTextBoxColumn.Name = "ClienteFacturarDataGridViewTextBoxColumn"
            Me.ClienteFacturarDataGridViewTextBoxColumn.ReadOnly = True
            '
            'MontoReparacionDataGridViewTextBoxColumn
            '
            Me.MontoReparacionDataGridViewTextBoxColumn.DataPropertyName = "MontoReparacion"
            resources.ApplyResources(Me.MontoReparacionDataGridViewTextBoxColumn, "MontoReparacionDataGridViewTextBoxColumn")
            Me.MontoReparacionDataGridViewTextBoxColumn.Name = "MontoReparacionDataGridViewTextBoxColumn"
            Me.MontoReparacionDataGridViewTextBoxColumn.ReadOnly = True
            '
            'AsesorDataGridViewTextBoxColumn
            '
            Me.AsesorDataGridViewTextBoxColumn.DataPropertyName = "Asesor"
            resources.ApplyResources(Me.AsesorDataGridViewTextBoxColumn, "AsesorDataGridViewTextBoxColumn")
            Me.AsesorDataGridViewTextBoxColumn.Name = "AsesorDataGridViewTextBoxColumn"
            Me.AsesorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoCotizacionDataGridViewTextBoxColumn
            '
            Me.NoCotizacionDataGridViewTextBoxColumn.DataPropertyName = "NoCotizacion"
            resources.ApplyResources(Me.NoCotizacionDataGridViewTextBoxColumn, "NoCotizacionDataGridViewTextBoxColumn")
            Me.NoCotizacionDataGridViewTextBoxColumn.Name = "NoCotizacionDataGridViewTextBoxColumn"
            Me.NoCotizacionDataGridViewTextBoxColumn.ReadOnly = True
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
            'OTPadreDataGridViewTextBoxColumn
            '
            Me.OTPadreDataGridViewTextBoxColumn.DataPropertyName = "OTPadre"
            resources.ApplyResources(Me.OTPadreDataGridViewTextBoxColumn, "OTPadreDataGridViewTextBoxColumn")
            Me.OTPadreDataGridViewTextBoxColumn.Name = "OTPadreDataGridViewTextBoxColumn"
            Me.OTPadreDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CardCodeOrigDataGridViewTextBoxColumn
            '
            Me.CardCodeOrigDataGridViewTextBoxColumn.DataPropertyName = "CardCodeOrig"
            resources.ApplyResources(Me.CardCodeOrigDataGridViewTextBoxColumn, "CardCodeOrigDataGridViewTextBoxColumn")
            Me.CardCodeOrigDataGridViewTextBoxColumn.Name = "CardCodeOrigDataGridViewTextBoxColumn"
            Me.CardCodeOrigDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CardNameOrigDataGridViewTextBoxColumn
            '
            Me.CardNameOrigDataGridViewTextBoxColumn.DataPropertyName = "CardNameOrig"
            resources.ApplyResources(Me.CardNameOrigDataGridViewTextBoxColumn, "CardNameOrigDataGridViewTextBoxColumn")
            Me.CardNameOrigDataGridViewTextBoxColumn.Name = "CardNameOrigDataGridViewTextBoxColumn"
            Me.CardNameOrigDataGridViewTextBoxColumn.ReadOnly = True
            '
            'VINDataGridViewTextBoxColumn
            '
            Me.VINDataGridViewTextBoxColumn.DataPropertyName = "VIN"
            resources.ApplyResources(Me.VINDataGridViewTextBoxColumn, "VINDataGridViewTextBoxColumn")
            Me.VINDataGridViewTextBoxColumn.Name = "VINDataGridViewTextBoxColumn"
            Me.VINDataGridViewTextBoxColumn.ReadOnly = True
            '
            'AnoVehiculoDataGridViewTextBoxColumn
            '
            Me.AnoVehiculoDataGridViewTextBoxColumn.DataPropertyName = "AnoVehiculo"
            resources.ApplyResources(Me.AnoVehiculoDataGridViewTextBoxColumn, "AnoVehiculoDataGridViewTextBoxColumn")
            Me.AnoVehiculoDataGridViewTextBoxColumn.Name = "AnoVehiculoDataGridViewTextBoxColumn"
            Me.AnoVehiculoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NombreAsesorDataGridViewTextBoxColumn
            '
            Me.NombreAsesorDataGridViewTextBoxColumn.DataPropertyName = "NombreAsesor"
            resources.ApplyResources(Me.NombreAsesorDataGridViewTextBoxColumn, "NombreAsesorDataGridViewTextBoxColumn")
            Me.NombreAsesorDataGridViewTextBoxColumn.Name = "NombreAsesorDataGridViewTextBoxColumn"
            Me.NombreAsesorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'lblLine9
            '
            Me.lblLine9.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine9, "lblLine9")
            Me.lblLine9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine9.Name = "lblLine9"
            '
            'lblFechaReporte
            '
            resources.ApplyResources(Me.lblFechaReporte, "lblFechaReporte")
            Me.lblFechaReporte.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblFechaReporte.Name = "lblFechaReporte"
            '
            'Label3
            '
            Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.Name = "Label3"
            '
            'lblEstado
            '
            resources.ApplyResources(Me.lblEstado, "lblEstado")
            Me.lblEstado.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblEstado.Name = "lblEstado"
            '
            'txtNoVisita
            '
            Me.txtNoVisita.AceptaNegativos = False
            Me.txtNoVisita.BackColor = System.Drawing.Color.White
            Me.txtNoVisita.EstiloSBO = True
            resources.ApplyResources(Me.txtNoVisita, "txtNoVisita")
            Me.txtNoVisita.MaxDecimales = 0
            Me.txtNoVisita.MaxEnteros = 0
            Me.txtNoVisita.Millares = False
            Me.txtNoVisita.Name = "txtNoVisita"
            Me.txtNoVisita.ReadOnly = True
            Me.txtNoVisita.Size_AdjustableHeight = 20
            Me.txtNoVisita.TeclasDeshacer = True
            Me.txtNoVisita.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'lblNumeroVisita
            '
            resources.ApplyResources(Me.lblNumeroVisita, "lblNumeroVisita")
            Me.lblNumeroVisita.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNumeroVisita.Name = "lblNumeroVisita"
            '
            'txtNombreCliente
            '
            Me.txtNombreCliente.AceptaNegativos = False
            Me.txtNombreCliente.BackColor = System.Drawing.Color.White
            Me.txtNombreCliente.EstiloSBO = True
            resources.ApplyResources(Me.txtNombreCliente, "txtNombreCliente")
            Me.txtNombreCliente.MaxDecimales = 0
            Me.txtNombreCliente.MaxEnteros = 0
            Me.txtNombreCliente.Millares = False
            Me.txtNombreCliente.Name = "txtNombreCliente"
            Me.txtNombreCliente.ReadOnly = True
            Me.txtNombreCliente.Size_AdjustableHeight = 20
            Me.txtNombreCliente.TeclasDeshacer = True
            Me.txtNombreCliente.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtNoVehiculo
            '
            Me.txtNoVehiculo.AceptaNegativos = False
            Me.txtNoVehiculo.BackColor = System.Drawing.Color.White
            Me.txtNoVehiculo.EstiloSBO = True
            resources.ApplyResources(Me.txtNoVehiculo, "txtNoVehiculo")
            Me.txtNoVehiculo.MaxDecimales = 0
            Me.txtNoVehiculo.MaxEnteros = 0
            Me.txtNoVehiculo.Millares = False
            Me.txtNoVehiculo.Name = "txtNoVehiculo"
            Me.txtNoVehiculo.ReadOnly = True
            Me.txtNoVehiculo.Size_AdjustableHeight = 20
            Me.txtNoVehiculo.TeclasDeshacer = True
            Me.txtNoVehiculo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label7
            '
            Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label7, "Label7")
            Me.Label7.Name = "Label7"
            '
            'lblNoVehiculo
            '
            resources.ApplyResources(Me.lblNoVehiculo, "lblNoVehiculo")
            Me.lblNoVehiculo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoVehiculo.Name = "lblNoVehiculo"
            '
            'txtCodCliente
            '
            Me.txtCodCliente.AceptaNegativos = False
            Me.txtCodCliente.BackColor = System.Drawing.Color.White
            Me.txtCodCliente.EstiloSBO = True
            resources.ApplyResources(Me.txtCodCliente, "txtCodCliente")
            Me.txtCodCliente.MaxDecimales = 0
            Me.txtCodCliente.MaxEnteros = 0
            Me.txtCodCliente.Millares = False
            Me.txtCodCliente.Name = "txtCodCliente"
            Me.txtCodCliente.ReadOnly = True
            Me.txtCodCliente.Size_AdjustableHeight = 20
            Me.txtCodCliente.TeclasDeshacer = True
            Me.txtCodCliente.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'lblCliente
            '
            resources.ApplyResources(Me.lblCliente, "lblCliente")
            Me.lblCliente.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCliente.Name = "lblCliente"
            '
            'Label5
            '
            Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.Name = "Label5"
            '
            'lblMarca
            '
            resources.ApplyResources(Me.lblMarca, "lblMarca")
            Me.lblMarca.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblMarca.Name = "lblMarca"
            '
            'Label6
            '
            Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label6, "Label6")
            Me.Label6.Name = "Label6"
            '
            'lblEstilo
            '
            resources.ApplyResources(Me.lblEstilo, "lblEstilo")
            Me.lblEstilo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblEstilo.Name = "lblEstilo"
            '
            'txtMarca
            '
            Me.txtMarca.AceptaNegativos = False
            Me.txtMarca.BackColor = System.Drawing.Color.White
            Me.txtMarca.EstiloSBO = True
            resources.ApplyResources(Me.txtMarca, "txtMarca")
            Me.txtMarca.MaxDecimales = 0
            Me.txtMarca.MaxEnteros = 0
            Me.txtMarca.Millares = False
            Me.txtMarca.Name = "txtMarca"
            Me.txtMarca.ReadOnly = True
            Me.txtMarca.Size_AdjustableHeight = 20
            Me.txtMarca.TeclasDeshacer = True
            Me.txtMarca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtEstilo
            '
            Me.txtEstilo.AceptaNegativos = False
            Me.txtEstilo.BackColor = System.Drawing.Color.White
            Me.txtEstilo.EstiloSBO = True
            resources.ApplyResources(Me.txtEstilo, "txtEstilo")
            Me.txtEstilo.MaxDecimales = 0
            Me.txtEstilo.MaxEnteros = 0
            Me.txtEstilo.Millares = False
            Me.txtEstilo.Name = "txtEstilo"
            Me.txtEstilo.ReadOnly = True
            Me.txtEstilo.Size_AdjustableHeight = 20
            Me.txtEstilo.TeclasDeshacer = True
            Me.txtEstilo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtFechaApertura
            '
            Me.txtFechaApertura.AceptaNegativos = False
            Me.txtFechaApertura.BackColor = System.Drawing.Color.White
            Me.txtFechaApertura.EstiloSBO = True
            resources.ApplyResources(Me.txtFechaApertura, "txtFechaApertura")
            Me.txtFechaApertura.MaxDecimales = 0
            Me.txtFechaApertura.MaxEnteros = 0
            Me.txtFechaApertura.Millares = False
            Me.txtFechaApertura.Name = "txtFechaApertura"
            Me.txtFechaApertura.ReadOnly = True
            Me.txtFechaApertura.Size_AdjustableHeight = 20
            Me.txtFechaApertura.TeclasDeshacer = True
            Me.txtFechaApertura.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'cboEstadoVisita
            '
            Me.cboEstadoVisita.BackColor = System.Drawing.Color.White
            Me.cboEstadoVisita.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstadoVisita.EstiloSBO = True
            resources.ApplyResources(Me.cboEstadoVisita, "cboEstadoVisita")
            Me.cboEstadoVisita.Name = "cboEstadoVisita"
            '
            'txtIdentCliente
            '
            Me.txtIdentCliente.AceptaNegativos = False
            Me.txtIdentCliente.BackColor = System.Drawing.Color.White
            Me.txtIdentCliente.EstiloSBO = True
            resources.ApplyResources(Me.txtIdentCliente, "txtIdentCliente")
            Me.txtIdentCliente.MaxDecimales = 0
            Me.txtIdentCliente.MaxEnteros = 0
            Me.txtIdentCliente.Millares = False
            Me.txtIdentCliente.Name = "txtIdentCliente"
            Me.txtIdentCliente.ReadOnly = True
            Me.txtIdentCliente.Size_AdjustableHeight = 20
            Me.txtIdentCliente.TeclasDeshacer = True
            Me.txtIdentCliente.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.Numeric
            '
            'Label10
            '
            Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label10, "Label10")
            Me.Label10.Name = "Label10"
            '
            'lblIdentCliente
            '
            resources.ApplyResources(Me.lblIdentCliente, "lblIdentCliente")
            Me.lblIdentCliente.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblIdentCliente.Name = "lblIdentCliente"
            '
            'txtModelo
            '
            Me.txtModelo.AceptaNegativos = False
            Me.txtModelo.BackColor = System.Drawing.Color.White
            Me.txtModelo.EstiloSBO = True
            resources.ApplyResources(Me.txtModelo, "txtModelo")
            Me.txtModelo.MaxDecimales = 0
            Me.txtModelo.MaxEnteros = 0
            Me.txtModelo.Millares = False
            Me.txtModelo.Name = "txtModelo"
            Me.txtModelo.ReadOnly = True
            Me.txtModelo.Size_AdjustableHeight = 20
            Me.txtModelo.TeclasDeshacer = True
            Me.txtModelo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label11
            '
            Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label11, "Label11")
            Me.Label11.Name = "Label11"
            '
            'lblModelo
            '
            resources.ApplyResources(Me.lblModelo, "lblModelo")
            Me.lblModelo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblModelo.Name = "lblModelo"
            '
            'lblLine4
            '
            Me.lblLine4.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine4, "lblLine4")
            Me.lblLine4.Name = "lblLine4"
            '
            'txtCono
            '
            Me.txtCono.AceptaNegativos = False
            Me.txtCono.BackColor = System.Drawing.Color.White
            Me.txtCono.EstiloSBO = True
            resources.ApplyResources(Me.txtCono, "txtCono")
            Me.txtCono.MaxDecimales = 0
            Me.txtCono.MaxEnteros = 0
            Me.txtCono.Millares = False
            Me.txtCono.Name = "txtCono"
            Me.txtCono.ReadOnly = True
            Me.txtCono.Size_AdjustableHeight = 20
            Me.txtCono.TeclasDeshacer = True
            Me.txtCono.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.Numeric
            '
            'Label8
            '
            resources.ApplyResources(Me.Label8, "Label8")
            Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label8.Name = "Label8"
            '
            'lblLine3
            '
            Me.lblLine3.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine3, "lblLine3")
            Me.lblLine3.Name = "lblLine3"
            '
            'txtPlaca
            '
            Me.txtPlaca.AceptaNegativos = False
            Me.txtPlaca.BackColor = System.Drawing.Color.White
            Me.txtPlaca.EstiloSBO = True
            resources.ApplyResources(Me.txtPlaca, "txtPlaca")
            Me.txtPlaca.MaxDecimales = 0
            Me.txtPlaca.MaxEnteros = 0
            Me.txtPlaca.Millares = False
            Me.txtPlaca.Name = "txtPlaca"
            Me.txtPlaca.ReadOnly = True
            Me.txtPlaca.Size_AdjustableHeight = 20
            Me.txtPlaca.TeclasDeshacer = True
            Me.txtPlaca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label13
            '
            resources.ApplyResources(Me.Label13, "Label13")
            Me.Label13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label13.Name = "Label13"
            '
            'txtNombreAsesor
            '
            Me.txtNombreAsesor.AceptaNegativos = False
            Me.txtNombreAsesor.BackColor = System.Drawing.Color.White
            Me.txtNombreAsesor.EstiloSBO = True
            resources.ApplyResources(Me.txtNombreAsesor, "txtNombreAsesor")
            Me.txtNombreAsesor.MaxDecimales = 0
            Me.txtNombreAsesor.MaxEnteros = 0
            Me.txtNombreAsesor.Millares = False
            Me.txtNombreAsesor.Name = "txtNombreAsesor"
            Me.txtNombreAsesor.ReadOnly = True
            Me.txtNombreAsesor.Size_AdjustableHeight = 20
            Me.txtNombreAsesor.TeclasDeshacer = True
            Me.txtNombreAsesor.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label15
            '
            Me.Label15.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label15, "Label15")
            Me.Label15.Name = "Label15"
            '
            'txtCodAsesor
            '
            Me.txtCodAsesor.AceptaNegativos = False
            Me.txtCodAsesor.BackColor = System.Drawing.Color.White
            Me.txtCodAsesor.EstiloSBO = True
            resources.ApplyResources(Me.txtCodAsesor, "txtCodAsesor")
            Me.txtCodAsesor.MaxDecimales = 0
            Me.txtCodAsesor.MaxEnteros = 0
            Me.txtCodAsesor.Millares = False
            Me.txtCodAsesor.Name = "txtCodAsesor"
            Me.txtCodAsesor.ReadOnly = True
            Me.txtCodAsesor.Size_AdjustableHeight = 20
            Me.txtCodAsesor.TeclasDeshacer = True
            Me.txtCodAsesor.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblAsesor
            '
            resources.ApplyResources(Me.lblAsesor, "lblAsesor")
            Me.lblAsesor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblAsesor.Name = "lblAsesor"
            '
            'Label12
            '
            resources.ApplyResources(Me.Label12, "Label12")
            Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label12.Name = "Label12"
            '
            'dtpCierre
            '
            resources.ApplyResources(Me.dtpCierre, "dtpCierre")
            Me.dtpCierre.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCierre.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCierre.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCierre.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCierre.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCierre.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpCierre.Name = "dtpCierre"
            Me.dtpCierre.Value = New Date(2006, 2, 2, 0, 0, 0, 0)
            '
            'Label4
            '
            Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label4.Name = "Label4"
            '
            'txtHoraCompromiso
            '
            Me.txtHoraCompromiso.AceptaNegativos = False
            Me.txtHoraCompromiso.BackColor = System.Drawing.Color.White
            Me.txtHoraCompromiso.EstiloSBO = True
            resources.ApplyResources(Me.txtHoraCompromiso, "txtHoraCompromiso")
            Me.txtHoraCompromiso.MaxDecimales = 0
            Me.txtHoraCompromiso.MaxEnteros = 0
            Me.txtHoraCompromiso.Millares = False
            Me.txtHoraCompromiso.Name = "txtHoraCompromiso"
            Me.txtHoraCompromiso.ReadOnly = True
            Me.txtHoraCompromiso.Size_AdjustableHeight = 20
            Me.txtHoraCompromiso.TeclasDeshacer = True
            Me.txtHoraCompromiso.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtFechaCompromiso
            '
            Me.txtFechaCompromiso.AceptaNegativos = False
            Me.txtFechaCompromiso.BackColor = System.Drawing.Color.White
            Me.txtFechaCompromiso.EstiloSBO = True
            resources.ApplyResources(Me.txtFechaCompromiso, "txtFechaCompromiso")
            Me.txtFechaCompromiso.MaxDecimales = 0
            Me.txtFechaCompromiso.MaxEnteros = 0
            Me.txtFechaCompromiso.Millares = False
            Me.txtFechaCompromiso.Name = "txtFechaCompromiso"
            Me.txtFechaCompromiso.ReadOnly = True
            Me.txtFechaCompromiso.Size_AdjustableHeight = 20
            Me.txtFechaCompromiso.TeclasDeshacer = True
            Me.txtFechaCompromiso.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label9
            '
            Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label9, "Label9")
            Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label9.Name = "Label9"
            '
            'lblFechaCompromiso
            '
            resources.ApplyResources(Me.lblFechaCompromiso, "lblFechaCompromiso")
            Me.lblFechaCompromiso.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblFechaCompromiso.Name = "lblFechaCompromiso"
            '
            'txtHoraApertura
            '
            Me.txtHoraApertura.AceptaNegativos = False
            Me.txtHoraApertura.BackColor = System.Drawing.Color.White
            Me.txtHoraApertura.EstiloSBO = True
            resources.ApplyResources(Me.txtHoraApertura, "txtHoraApertura")
            Me.txtHoraApertura.MaxDecimales = 0
            Me.txtHoraApertura.MaxEnteros = 0
            Me.txtHoraApertura.Millares = False
            Me.txtHoraApertura.Name = "txtHoraApertura"
            Me.txtHoraApertura.ReadOnly = True
            Me.txtHoraApertura.Size_AdjustableHeight = 20
            Me.txtHoraApertura.TeclasDeshacer = True
            Me.txtHoraApertura.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'btnAceptar
            '
            resources.ApplyResources(Me.btnAceptar, "btnAceptar")
            Me.btnAceptar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnAceptar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.btnAceptar.Name = "btnAceptar"
            '
            'btnCerrar
            '
            resources.ApplyResources(Me.btnCerrar, "btnCerrar")
            Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCerrar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.btnCerrar.Name = "btnCerrar"
            '
            'Panel7
            '
            Me.Panel7.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.Panel7, "Panel7")
            Me.Panel7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Panel7.Name = "Panel7"
            '
            'btnArchivos
            '
            resources.ApplyResources(Me.btnArchivos, "btnArchivos")
            Me.btnArchivos.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.btnArchivos.Name = "btnArchivos"
            '
            'VisualizarUDFVisita
            '
            resources.ApplyResources(Me.VisualizarUDFVisita, "VisualizarUDFVisita")
            Me.VisualizarUDFVisita.BackColor = System.Drawing.SystemColors.Control
            Me.VisualizarUDFVisita.CampoLlave = Nothing
            Me.VisualizarUDFVisita.CodigoFormularioSBO = 0
            Me.VisualizarUDFVisita.CodigoUsuario = 0
            Me.VisualizarUDFVisita.Conexion = Nothing
            Me.VisualizarUDFVisita.Form = Nothing
            Me.VisualizarUDFVisita.Name = "VisualizarUDFVisita"
            Me.VisualizarUDFVisita.NombreBaseDatosSBO = Nothing
            Me.VisualizarUDFVisita.Tabla = Nothing
            Me.VisualizarUDFVisita.VisualizarUDFSBO = False
            Me.VisualizarUDFVisita.Where = Nothing
            '
            'SeccionesDataset1
            '
            Me.SeccionesDataset1.DataSetName = "SeccionesDataset"
            Me.SeccionesDataset1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'frmDetalleVisita
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.VisualizarUDFVisita)
            Me.Controls.Add(Me.btnArchivos)
            Me.Controls.Add(Me.txtHoraApertura)
            Me.Controls.Add(Me.txtHoraCompromiso)
            Me.Controls.Add(Me.txtFechaCompromiso)
            Me.Controls.Add(Me.txtNombreAsesor)
            Me.Controls.Add(Me.txtCodAsesor)
            Me.Controls.Add(Me.txtPlaca)
            Me.Controls.Add(Me.txtCono)
            Me.Controls.Add(Me.txtModelo)
            Me.Controls.Add(Me.txtIdentCliente)
            Me.Controls.Add(Me.cboEstadoVisita)
            Me.Controls.Add(Me.txtFechaApertura)
            Me.Controls.Add(Me.txtEstilo)
            Me.Controls.Add(Me.txtMarca)
            Me.Controls.Add(Me.txtNoVehiculo)
            Me.Controls.Add(Me.txtNoVisita)
            Me.Controls.Add(Me.txtNombreCliente)
            Me.Controls.Add(Me.txtCodCliente)
            Me.Controls.Add(Me.Label9)
            Me.Controls.Add(Me.lblFechaCompromiso)
            Me.Controls.Add(Me.Label15)
            Me.Controls.Add(Me.lblAsesor)
            Me.Controls.Add(Me.lblLine3)
            Me.Controls.Add(Me.Label13)
            Me.Controls.Add(Me.lblLine4)
            Me.Controls.Add(Me.Label8)
            Me.Controls.Add(Me.btnAceptar)
            Me.Controls.Add(Me.btnCerrar)
            Me.Controls.Add(Me.Label11)
            Me.Controls.Add(Me.lblModelo)
            Me.Controls.Add(Me.Label10)
            Me.Controls.Add(Me.lblIdentCliente)
            Me.Controls.Add(Me.Label6)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.lblMarca)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.Panel7)
            Me.Controls.Add(Me.dtpCierre)
            Me.Controls.Add(Me.Label12)
            Me.Controls.Add(Me.lblLine9)
            Me.Controls.Add(Me.lblFechaReporte)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.lblEstado)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.lblNumeroVisita)
            Me.Controls.Add(Me.Label7)
            Me.Controls.Add(Me.lblNoVehiculo)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.lblCliente)
            Me.Controls.Add(Me.grpCitas)
            Me.Controls.Add(Me.lblEstilo)
            Me.MaximizeBox = False
            Me.Name = "frmDetalleVisita"
            CType(OrdenTrabajoDatasetGrid, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpCitas.ResumeLayout(False)
            CType(Me.dtgOrdenes, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.SeccionesDataset1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents grpCitas As System.Windows.Forms.GroupBox
        Public WithEvents lblLine9 As System.Windows.Forms.Label
        Friend WithEvents lblFechaReporte As System.Windows.Forms.Label
        Public WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents lblEstado As System.Windows.Forms.Label
        Friend WithEvents txtNoVisita As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lblNumeroVisita As System.Windows.Forms.Label
        Friend WithEvents txtNombreCliente As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtNoVehiculo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents lblNoVehiculo As System.Windows.Forms.Label
        Friend WithEvents txtCodCliente As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents lblCliente As System.Windows.Forms.Label
        Public WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents lblMarca As System.Windows.Forms.Label
        Public WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents lblEstilo As System.Windows.Forms.Label
        Friend WithEvents txtMarca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtEstilo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtFechaApertura As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents cboEstadoVisita As SCGComboBox.SCGComboBox
        Friend WithEvents txtIdentCliente As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents lblIdentCliente As System.Windows.Forms.Label
        Friend WithEvents txtModelo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label11 As System.Windows.Forms.Label
        Friend WithEvents lblModelo As System.Windows.Forms.Label
        Friend WithEvents btnAceptar As System.Windows.Forms.Button
        Friend WithEvents btnCerrar As System.Windows.Forms.Button
        Public WithEvents lblLine4 As System.Windows.Forms.Label
        Friend WithEvents txtCono As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Public WithEvents lblLine3 As System.Windows.Forms.Label
        Friend WithEvents txtPlaca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label13 As System.Windows.Forms.Label
        Friend WithEvents txtNombreAsesor As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label15 As System.Windows.Forms.Label
        Friend WithEvents txtCodAsesor As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblAsesor As System.Windows.Forms.Label
        Friend WithEvents Label12 As System.Windows.Forms.Label
        Friend WithEvents dtpCierre As System.Windows.Forms.DateTimePicker
        Friend WithEvents Panel7 As System.Windows.Forms.Panel
        Public WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents txtHoraCompromiso As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtFechaCompromiso As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents lblFechaCompromiso As System.Windows.Forms.Label
        Friend WithEvents txtHoraApertura As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents dtgOrdenes As System.Windows.Forms.DataGridView
        Friend WithEvents btnArchivos As System.Windows.Forms.Button
        Friend WithEvents NoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents TipoDescDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescMarcaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescEstiloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescModeloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoDescDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescipcionEstadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechacierreDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoVisitaDescDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents PlacaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodMarcaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodEstiloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodModeloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ConoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoVisitaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaaperturaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechacompromisoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodTipoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ObservacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoVisitaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CheckDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ClienteFacturarDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents MontoReparacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents AsesorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoCotizacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents HoraCompDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaCompDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents OTPadreDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CardCodeOrigDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CardNameOrigDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents VINDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents AnoVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NombreAsesorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents VisualizarUDFVisita As ControlUDF.VisualizarUDF
        Friend WithEvents SeccionesDataset1 As DMSOneFramework.SeccionesDataset
    End Class
End Namespace