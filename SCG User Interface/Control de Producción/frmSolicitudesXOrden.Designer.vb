Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmSolicitudesXOrden
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
            Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSolicitudesXOrden))
            Me.grpItems = New System.Windows.Forms.GroupBox
            Me.dtgDetalles = New System.Windows.Forms.DataGridView
            Me.Check = New System.Windows.Forms.DataGridViewCheckBoxColumn
            Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.SolicitadoPorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaSolicitudDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.RespondidoPorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaRespuestaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescEstadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoResources = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
            Me.NoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.PlacaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IDVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodMarcaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescMarcaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodEstiloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescEstiloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodModeloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescModeloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
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
            Me.VINDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.AnoVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.m_dtsSolicitudEspecificos = New DMSOneFramework.SolicitudEspecificosDataset
            Me.btnAceptar = New System.Windows.Forms.Button
            Me.txtNoOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.lblLine2 = New System.Windows.Forms.Label
            Me.lblNoOrden = New System.Windows.Forms.Label
            Me.btnCancelarSolicitud = New System.Windows.Forms.Button
            Me.grpItems.SuspendLayout()
            CType(Me.dtgDetalles, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.m_dtsSolicitudEspecificos, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
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
            DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            Me.dtgDetalles.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle3
            Me.dtgDetalles.AutoGenerateColumns = False
            Me.dtgDetalles.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtgDetalles.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Check, Me.IDDataGridViewTextBoxColumn, Me.SolicitadoPorDataGridViewTextBoxColumn, Me.FechaSolicitudDataGridViewTextBoxColumn, Me.RespondidoPorDataGridViewTextBoxColumn, Me.FechaRespuestaDataGridViewTextBoxColumn, Me.DescEstadoDataGridViewTextBoxColumn, Me.EstadoResources, Me.EstadoDataGridViewCheckBoxColumn, Me.NoOrdenDataGridViewTextBoxColumn, Me.PlacaDataGridViewTextBoxColumn, Me.IDVehiculoDataGridViewTextBoxColumn, Me.CodMarcaDataGridViewTextBoxColumn, Me.DescMarcaDataGridViewTextBoxColumn, Me.CodEstiloDataGridViewTextBoxColumn, Me.DescEstiloDataGridViewTextBoxColumn, Me.CodModeloDataGridViewTextBoxColumn, Me.DescModeloDataGridViewTextBoxColumn, Me.NoVisitaDataGridViewTextBoxColumn, Me.FechaaperturaDataGridViewTextBoxColumn, Me.FechacompromisoDataGridViewTextBoxColumn, Me.CodTipoOrdenDataGridViewTextBoxColumn, Me.NoCotizacionDataGridViewTextBoxColumn, Me.TipoDescDataGridViewTextBoxColumn, Me.ObservacionDataGridViewTextBoxColumn, Me.NoVehiculoDataGridViewTextBoxColumn, Me.HoraCompDataGridViewTextBoxColumn, Me.FechaCompDataGridViewTextBoxColumn, Me.VINDataGridViewTextBoxColumn, Me.AnoVehiculoDataGridViewTextBoxColumn})
            Me.dtgDetalles.DataMember = "SCGTA_SP_SelSolicitudEspecifico"
            Me.dtgDetalles.DataSource = Me.m_dtsSolicitudEspecificos
            resources.ApplyResources(Me.dtgDetalles, "dtgDetalles")
            Me.dtgDetalles.MultiSelect = False
            Me.dtgDetalles.Name = "dtgDetalles"
            DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgDetalles.RowsDefaultCellStyle = DataGridViewCellStyle4
            Me.dtgDetalles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
            '
            'Check
            '
            Me.Check.DataPropertyName = "Check"
            Me.Check.HeaderText = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Check.Name = "Check"
            resources.ApplyResources(Me.Check, "Check")
            '
            'IDDataGridViewTextBoxColumn
            '
            Me.IDDataGridViewTextBoxColumn.DataPropertyName = "ID"
            resources.ApplyResources(Me.IDDataGridViewTextBoxColumn, "IDDataGridViewTextBoxColumn")
            Me.IDDataGridViewTextBoxColumn.Name = "IDDataGridViewTextBoxColumn"
            Me.IDDataGridViewTextBoxColumn.ReadOnly = True
            '
            'SolicitadoPorDataGridViewTextBoxColumn
            '
            Me.SolicitadoPorDataGridViewTextBoxColumn.DataPropertyName = "SolicitadoPor"
            resources.ApplyResources(Me.SolicitadoPorDataGridViewTextBoxColumn, "SolicitadoPorDataGridViewTextBoxColumn")
            Me.SolicitadoPorDataGridViewTextBoxColumn.Name = "SolicitadoPorDataGridViewTextBoxColumn"
            Me.SolicitadoPorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FechaSolicitudDataGridViewTextBoxColumn
            '
            Me.FechaSolicitudDataGridViewTextBoxColumn.DataPropertyName = "FechaSolicitud"
            resources.ApplyResources(Me.FechaSolicitudDataGridViewTextBoxColumn, "FechaSolicitudDataGridViewTextBoxColumn")
            Me.FechaSolicitudDataGridViewTextBoxColumn.Name = "FechaSolicitudDataGridViewTextBoxColumn"
            Me.FechaSolicitudDataGridViewTextBoxColumn.ReadOnly = True
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
            'DescEstadoDataGridViewTextBoxColumn
            '
            Me.DescEstadoDataGridViewTextBoxColumn.DataPropertyName = "DescEstado"
            resources.ApplyResources(Me.DescEstadoDataGridViewTextBoxColumn, "DescEstadoDataGridViewTextBoxColumn")
            Me.DescEstadoDataGridViewTextBoxColumn.Name = "DescEstadoDataGridViewTextBoxColumn"
            Me.DescEstadoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EstadoResources
            '
            Me.EstadoResources.DataPropertyName = "DescEstadoResources"
            resources.ApplyResources(Me.EstadoResources, "EstadoResources")
            Me.EstadoResources.Name = "EstadoResources"
            Me.EstadoResources.ReadOnly = True
            '
            'EstadoDataGridViewCheckBoxColumn
            '
            Me.EstadoDataGridViewCheckBoxColumn.DataPropertyName = "Estado"
            resources.ApplyResources(Me.EstadoDataGridViewCheckBoxColumn, "EstadoDataGridViewCheckBoxColumn")
            Me.EstadoDataGridViewCheckBoxColumn.Name = "EstadoDataGridViewCheckBoxColumn"
            Me.EstadoDataGridViewCheckBoxColumn.ReadOnly = True
            '
            'NoOrdenDataGridViewTextBoxColumn
            '
            Me.NoOrdenDataGridViewTextBoxColumn.DataPropertyName = "NoOrden"
            resources.ApplyResources(Me.NoOrdenDataGridViewTextBoxColumn, "NoOrdenDataGridViewTextBoxColumn")
            Me.NoOrdenDataGridViewTextBoxColumn.Name = "NoOrdenDataGridViewTextBoxColumn"
            Me.NoOrdenDataGridViewTextBoxColumn.ReadOnly = True
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
            'DescMarcaDataGridViewTextBoxColumn
            '
            Me.DescMarcaDataGridViewTextBoxColumn.DataPropertyName = "DescMarca"
            resources.ApplyResources(Me.DescMarcaDataGridViewTextBoxColumn, "DescMarcaDataGridViewTextBoxColumn")
            Me.DescMarcaDataGridViewTextBoxColumn.Name = "DescMarcaDataGridViewTextBoxColumn"
            Me.DescMarcaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodEstiloDataGridViewTextBoxColumn
            '
            Me.CodEstiloDataGridViewTextBoxColumn.DataPropertyName = "CodEstilo"
            resources.ApplyResources(Me.CodEstiloDataGridViewTextBoxColumn, "CodEstiloDataGridViewTextBoxColumn")
            Me.CodEstiloDataGridViewTextBoxColumn.Name = "CodEstiloDataGridViewTextBoxColumn"
            Me.CodEstiloDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DescEstiloDataGridViewTextBoxColumn
            '
            Me.DescEstiloDataGridViewTextBoxColumn.DataPropertyName = "DescEstilo"
            resources.ApplyResources(Me.DescEstiloDataGridViewTextBoxColumn, "DescEstiloDataGridViewTextBoxColumn")
            Me.DescEstiloDataGridViewTextBoxColumn.Name = "DescEstiloDataGridViewTextBoxColumn"
            Me.DescEstiloDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodModeloDataGridViewTextBoxColumn
            '
            Me.CodModeloDataGridViewTextBoxColumn.DataPropertyName = "CodModelo"
            resources.ApplyResources(Me.CodModeloDataGridViewTextBoxColumn, "CodModeloDataGridViewTextBoxColumn")
            Me.CodModeloDataGridViewTextBoxColumn.Name = "CodModeloDataGridViewTextBoxColumn"
            Me.CodModeloDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DescModeloDataGridViewTextBoxColumn
            '
            Me.DescModeloDataGridViewTextBoxColumn.DataPropertyName = "DescModelo"
            resources.ApplyResources(Me.DescModeloDataGridViewTextBoxColumn, "DescModeloDataGridViewTextBoxColumn")
            Me.DescModeloDataGridViewTextBoxColumn.Name = "DescModeloDataGridViewTextBoxColumn"
            Me.DescModeloDataGridViewTextBoxColumn.ReadOnly = True
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
            'm_dtsSolicitudEspecificos
            '
            Me.m_dtsSolicitudEspecificos.DataSetName = "SolicitudEspecificosDataset"
            Me.m_dtsSolicitudEspecificos.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'btnAceptar
            '
            resources.ApplyResources(Me.btnAceptar, "btnAceptar")
            Me.btnAceptar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnAceptar.Name = "btnAceptar"
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
            Me.txtNoOrden.ReadOnly = True
            Me.txtNoOrden.Size_AdjustableHeight = 22
            Me.txtNoOrden.TeclasDeshacer = True
            Me.txtNoOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblLine2
            '
            Me.lblLine2.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLine2, "lblLine2")
            Me.lblLine2.Name = "lblLine2"
            '
            'lblNoOrden
            '
            resources.ApplyResources(Me.lblNoOrden, "lblNoOrden")
            Me.lblNoOrden.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoOrden.Name = "lblNoOrden"
            '
            'btnCancelarSolicitud
            '
            resources.ApplyResources(Me.btnCancelarSolicitud, "btnCancelarSolicitud")
            Me.btnCancelarSolicitud.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancelarSolicitud.Name = "btnCancelarSolicitud"
            '
            'frmSolicitudesXOrden
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.grpItems)
            Me.Controls.Add(Me.btnAceptar)
            Me.Controls.Add(Me.lblNoOrden)
            Me.Controls.Add(Me.lblLine2)
            Me.Controls.Add(Me.txtNoOrden)
            Me.Controls.Add(Me.btnCancelarSolicitud)
            Me.MaximizeBox = False
            Me.Name = "frmSolicitudesXOrden"
            Me.grpItems.ResumeLayout(False)
            CType(Me.dtgDetalles, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.m_dtsSolicitudEspecificos, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents m_dtsSolicitudEspecificos As DMSOneFramework.SolicitudEspecificosDataset
        Friend WithEvents grpItems As System.Windows.Forms.GroupBox
        Friend WithEvents dtgDetalles As System.Windows.Forms.DataGridView
        Friend WithEvents btnAceptar As System.Windows.Forms.Button
        Friend WithEvents txtNoOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents lblLine2 As System.Windows.Forms.Label
        Friend WithEvents lblNoOrden As System.Windows.Forms.Label
        Friend WithEvents btnCancelarSolicitud As System.Windows.Forms.Button
        Friend WithEvents Check As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents IDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents SolicitadoPorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaSolicitudDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents RespondidoPorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaRespuestaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescEstadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoResources As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents NoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents PlacaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodMarcaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescMarcaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodEstiloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescEstiloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodModeloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescModeloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
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
        Friend WithEvents VINDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents AnoVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    End Class

End Namespace