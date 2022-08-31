Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmOrdenesEspeciales
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrdenesEspeciales))
            Me.btnNueva = New System.Windows.Forms.Button
            Me.btnCerrar = New System.Windows.Forms.Button
            Me.dtgTiposConfigurados = New System.Windows.Forms.DataGridView
            Me.TipoDescDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CardCodeOrigDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CardNameOrigDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ObservacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.PlacaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IDVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodMarcaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescMarcaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodEstiloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescEstiloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodModeloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescModeloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoDescDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ConoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoVisitaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaaperturaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechacierreDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechacompromisoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodTipoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoVisitaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoVisitaDescDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CheckDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ClienteFacturarDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.MontoReparacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.AsesorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoCotizacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.HoraCompDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaCompDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.OTPadreDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.m_dtsOrdenTrabajo = New DMSOneFramework.OrdenEspecialDataset
            CType(Me.dtgTiposConfigurados, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.m_dtsOrdenTrabajo, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'btnNueva
            '
            resources.ApplyResources(Me.btnNueva, "btnNueva")
            Me.btnNueva.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnNueva.Name = "btnNueva"
            '
            'btnCerrar
            '
            resources.ApplyResources(Me.btnCerrar, "btnCerrar")
            Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCerrar.Name = "btnCerrar"
            '
            'dtgTiposConfigurados
            '
            Me.dtgTiposConfigurados.AllowUserToAddRows = False
            Me.dtgTiposConfigurados.AllowUserToDeleteRows = False
            Me.dtgTiposConfigurados.AutoGenerateColumns = False
            Me.dtgTiposConfigurados.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgTiposConfigurados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgTiposConfigurados.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.TipoDescDataGridViewTextBoxColumn, Me.NoOrdenDataGridViewTextBoxColumn, Me.CardCodeOrigDataGridViewTextBoxColumn, Me.CardNameOrigDataGridViewTextBoxColumn, Me.ObservacionDataGridViewTextBoxColumn, Me.PlacaDataGridViewTextBoxColumn, Me.IDVehiculoDataGridViewTextBoxColumn, Me.CodMarcaDataGridViewTextBoxColumn, Me.DescMarcaDataGridViewTextBoxColumn, Me.CodEstiloDataGridViewTextBoxColumn, Me.DescEstiloDataGridViewTextBoxColumn, Me.CodModeloDataGridViewTextBoxColumn, Me.DescModeloDataGridViewTextBoxColumn, Me.EstadoDataGridViewTextBoxColumn, Me.EstadoDescDataGridViewTextBoxColumn, Me.ConoDataGridViewTextBoxColumn, Me.NoVisitaDataGridViewTextBoxColumn, Me.FechaaperturaDataGridViewTextBoxColumn, Me.FechacierreDataGridViewTextBoxColumn, Me.FechacompromisoDataGridViewTextBoxColumn, Me.CodTipoOrdenDataGridViewTextBoxColumn, Me.EstadoVisitaDataGridViewTextBoxColumn, Me.EstadoVisitaDescDataGridViewTextBoxColumn, Me.NoVehiculoDataGridViewTextBoxColumn, Me.CheckDataGridViewTextBoxColumn, Me.ClienteFacturarDataGridViewTextBoxColumn, Me.MontoReparacionDataGridViewTextBoxColumn, Me.AsesorDataGridViewTextBoxColumn, Me.NoCotizacionDataGridViewTextBoxColumn, Me.HoraCompDataGridViewTextBoxColumn, Me.FechaCompDataGridViewTextBoxColumn, Me.OTPadreDataGridViewTextBoxColumn})
            Me.dtgTiposConfigurados.DataMember = "SCGTA_TB_Orden"
            Me.dtgTiposConfigurados.DataSource = Me.m_dtsOrdenTrabajo
            resources.ApplyResources(Me.dtgTiposConfigurados, "dtgTiposConfigurados")
            Me.dtgTiposConfigurados.Name = "dtgTiposConfigurados"
            Me.dtgTiposConfigurados.ReadOnly = True
            '
            'TipoDescDataGridViewTextBoxColumn
            '
            Me.TipoDescDataGridViewTextBoxColumn.DataPropertyName = "TipoDesc"
            resources.ApplyResources(Me.TipoDescDataGridViewTextBoxColumn, "TipoDescDataGridViewTextBoxColumn")
            Me.TipoDescDataGridViewTextBoxColumn.Name = "TipoDescDataGridViewTextBoxColumn"
            Me.TipoDescDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoOrdenDataGridViewTextBoxColumn
            '
            Me.NoOrdenDataGridViewTextBoxColumn.DataPropertyName = "NoOrden"
            resources.ApplyResources(Me.NoOrdenDataGridViewTextBoxColumn, "NoOrdenDataGridViewTextBoxColumn")
            Me.NoOrdenDataGridViewTextBoxColumn.Name = "NoOrdenDataGridViewTextBoxColumn"
            Me.NoOrdenDataGridViewTextBoxColumn.ReadOnly = True
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
            'ObservacionDataGridViewTextBoxColumn
            '
            Me.ObservacionDataGridViewTextBoxColumn.DataPropertyName = "Observacion"
            resources.ApplyResources(Me.ObservacionDataGridViewTextBoxColumn, "ObservacionDataGridViewTextBoxColumn")
            Me.ObservacionDataGridViewTextBoxColumn.Name = "ObservacionDataGridViewTextBoxColumn"
            Me.ObservacionDataGridViewTextBoxColumn.ReadOnly = True
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
            'EstadoDataGridViewTextBoxColumn
            '
            Me.EstadoDataGridViewTextBoxColumn.DataPropertyName = "Estado"
            resources.ApplyResources(Me.EstadoDataGridViewTextBoxColumn, "EstadoDataGridViewTextBoxColumn")
            Me.EstadoDataGridViewTextBoxColumn.Name = "EstadoDataGridViewTextBoxColumn"
            Me.EstadoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EstadoDescDataGridViewTextBoxColumn
            '
            Me.EstadoDescDataGridViewTextBoxColumn.DataPropertyName = "EstadoDesc"
            resources.ApplyResources(Me.EstadoDescDataGridViewTextBoxColumn, "EstadoDescDataGridViewTextBoxColumn")
            Me.EstadoDescDataGridViewTextBoxColumn.Name = "EstadoDescDataGridViewTextBoxColumn"
            Me.EstadoDescDataGridViewTextBoxColumn.ReadOnly = True
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
            'FechacierreDataGridViewTextBoxColumn
            '
            Me.FechacierreDataGridViewTextBoxColumn.DataPropertyName = "Fecha_cierre"
            resources.ApplyResources(Me.FechacierreDataGridViewTextBoxColumn, "FechacierreDataGridViewTextBoxColumn")
            Me.FechacierreDataGridViewTextBoxColumn.Name = "FechacierreDataGridViewTextBoxColumn"
            Me.FechacierreDataGridViewTextBoxColumn.ReadOnly = True
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
            'EstadoVisitaDataGridViewTextBoxColumn
            '
            Me.EstadoVisitaDataGridViewTextBoxColumn.DataPropertyName = "EstadoVisita"
            resources.ApplyResources(Me.EstadoVisitaDataGridViewTextBoxColumn, "EstadoVisitaDataGridViewTextBoxColumn")
            Me.EstadoVisitaDataGridViewTextBoxColumn.Name = "EstadoVisitaDataGridViewTextBoxColumn"
            Me.EstadoVisitaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EstadoVisitaDescDataGridViewTextBoxColumn
            '
            Me.EstadoVisitaDescDataGridViewTextBoxColumn.DataPropertyName = "EstadoVisitaDesc"
            resources.ApplyResources(Me.EstadoVisitaDescDataGridViewTextBoxColumn, "EstadoVisitaDescDataGridViewTextBoxColumn")
            Me.EstadoVisitaDescDataGridViewTextBoxColumn.Name = "EstadoVisitaDescDataGridViewTextBoxColumn"
            Me.EstadoVisitaDescDataGridViewTextBoxColumn.ReadOnly = True
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
            'm_dtsOrdenTrabajo
            '
            Me.m_dtsOrdenTrabajo.DataSetName = "OrdenEspecialDataset"
            Me.m_dtsOrdenTrabajo.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'frmOrdenesEspeciales
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.dtgTiposConfigurados)
            Me.Controls.Add(Me.btnNueva)
            Me.Controls.Add(Me.btnCerrar)
            Me.MaximizeBox = False
            Me.Name = "frmOrdenesEspeciales"
            CType(Me.dtgTiposConfigurados, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.m_dtsOrdenTrabajo, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents dtgTiposConfigurados As System.Windows.Forms.DataGridView
        Friend WithEvents btnNueva As System.Windows.Forms.Button
        Friend WithEvents btnCerrar As System.Windows.Forms.Button
        Friend WithEvents m_dtsOrdenTrabajo As DMSOneFramework.OrdenEspecialDataset
        Friend WithEvents TipoDescDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CardCodeOrigDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CardNameOrigDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ObservacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents PlacaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodMarcaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescMarcaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodEstiloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescEstiloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodModeloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescModeloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoDescDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ConoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoVisitaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaaperturaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechacierreDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechacompromisoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodTipoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoVisitaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoVisitaDescDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CheckDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ClienteFacturarDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents MontoReparacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents AsesorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoCotizacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents HoraCompDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaCompDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents OTPadreDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn

    End Class

End Namespace