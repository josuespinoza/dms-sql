Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon
Imports System.Data.SqlClient

Namespace SCG_User_Interface
    Public Class frmVisitas
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents grpCitas As System.Windows.Forms.GroupBox
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents ComboBox4 As SCGComboBox.SCGComboBox
        Friend WithEvents lblNoVisita As System.Windows.Forms.Label
        Friend WithEvents lblCliente As System.Windows.Forms.Label
        Friend WithEvents cboEstado As SCGComboBox.SCGComboBox
        Friend WithEvents tlbVisitas As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents dtpCierreini As System.Windows.Forms.DateTimePicker
        Friend WithEvents txtCliente As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtCodCliente As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtCono As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtPlaca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents dtpCompromisoini As System.Windows.Forms.DateTimePicker
        Friend WithEvents dtpAperturaini As System.Windows.Forms.DateTimePicker
        Friend WithEvents txtNoVisita As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents SubBuscador1 As Buscador.SubBuscador
        Friend WithEvents picCliente As System.Windows.Forms.PictureBox
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents btnCerrar As System.Windows.Forms.Button
        Public WithEvents lblLine10 As System.Windows.Forms.Label
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Public WithEvents lblLine4 As System.Windows.Forms.Label
        Public WithEvents lblLine3 As System.Windows.Forms.Label
        Public WithEvents lblLine9 As System.Windows.Forms.Label
        Public WithEvents lblLine8 As System.Windows.Forms.Label
        Public WithEvents lblLine6 As System.Windows.Forms.Label
        Public WithEvents lblLine2 As System.Windows.Forms.Label
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents txtNoVehiculo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Public WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents txtCedula As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblIdentCliente As System.Windows.Forms.Label
        Friend WithEvents txtNombreAsesor As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents picAsesor As System.Windows.Forms.PictureBox
        Friend WithEvents txtAsesor As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblAsesor As System.Windows.Forms.Label
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents dtpCierrefin As System.Windows.Forms.DateTimePicker
        Friend WithEvents Panel3 As System.Windows.Forms.Panel
        Friend WithEvents dtpCompromisofin As System.Windows.Forms.DateTimePicker
        Friend WithEvents Panel4 As System.Windows.Forms.Panel
        Friend WithEvents dtpAperturafin As System.Windows.Forms.DateTimePicker
        Friend WithEvents chkCierre As System.Windows.Forms.CheckBox
        Friend WithEvents chkCompromiso As System.Windows.Forms.CheckBox
        Friend WithEvents chkApertura As System.Windows.Forms.CheckBox
        Friend WithEvents SubBuscador2 As Buscador.SubBuscador
        Friend WithEvents Panel5 As System.Windows.Forms.Panel
        Friend WithEvents Panel6 As System.Windows.Forms.Panel
        Friend WithEvents Panel7 As System.Windows.Forms.Panel
        Friend WithEvents chkEstado As System.Windows.Forms.CheckBox
        Friend WithEvents cboModelo As SCGComboBox.SCGComboBox
        Public WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents chkModelo As System.Windows.Forms.CheckBox
        Friend WithEvents cboMarca As SCGComboBox.SCGComboBox
        Friend WithEvents cboEstilo As SCGComboBox.SCGComboBox
        Public WithEvents Label7 As System.Windows.Forms.Label
        Public WithEvents lblLine7 As System.Windows.Forms.Label
        Friend WithEvents chkMarca As System.Windows.Forms.CheckBox
        Friend WithEvents chkEstilo As System.Windows.Forms.CheckBox
        Friend WithEvents dtgVisitas As System.Windows.Forms.DataGridView
        Friend WithEvents dtgOrdenes As System.Windows.Forms.DataGridView
        Friend WithEvents NoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents TipoDescDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescipcionEstadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaaperturaDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechacompromisoDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ObservacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents PlacaDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDVehiculoDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodMarcaDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescMarcaDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodEstiloDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescEstiloDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodModeloDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescModeloDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoDescDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ConoDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoVisitaDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechacierreDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodTipoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoVisitaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoVisitaDescDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoVehiculoDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CheckDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ClienteFacturarDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents MontoReparacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents AsesorDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoCotizacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents HoraCompDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaCompDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents OTPadreDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CardCodeOrigDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CardNameOrigDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents VINDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents AnoVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NombreAsesorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoVisitaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CardNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents PlacaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescMarcaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescEstiloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescModeloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescripcionEstadoVisitaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaaperturaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechacompromisoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechacierreDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents AsesorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodColorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CardCodeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ConoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescColorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents AsesorNombreDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IdentClienteDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents HoracompromisoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CotizacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodMarcaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodModeloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodEstiloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodEstadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents grpCriteriosBusqueda As System.Windows.Forms.GroupBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim OrdenTrabajoDatasetGrid As DMSOneFramework.OrdenTrabajoDataset
            Dim VisitaDatasetGrid As DMSOneFramework.VisitaDataset
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVisitas))
            Me.grpCitas = New System.Windows.Forms.GroupBox
            Me.dtgOrdenes = New System.Windows.Forms.DataGridView
            Me.NoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.TipoDescDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescipcionEstadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaaperturaDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechacompromisoDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ObservacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.PlacaDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IDVehiculoDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodMarcaDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescMarcaDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodEstiloDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescEstiloDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodModeloDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescModeloDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoDescDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ConoDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoVisitaDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechacierreDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodTipoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoVisitaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoVisitaDescDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoVehiculoDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CheckDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ClienteFacturarDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.MontoReparacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.AsesorDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoCotizacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.HoraCompDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaCompDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.OTPadreDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CardCodeOrigDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CardNameOrigDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.VINDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.AnoVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NombreAsesorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.dtgVisitas = New System.Windows.Forms.DataGridView
            Me.NoVisitaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CardNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NoVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.PlacaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescMarcaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescEstiloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescModeloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescripcionEstadoVisitaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechaaperturaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EstadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechacompromisoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.FechacierreDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.AsesorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodColorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CardCodeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IDVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ConoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescColorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.AsesorNombreDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IdentClienteDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.HoracompromisoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CotizacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodMarcaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodModeloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodEstiloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CodEstadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.Label5 = New System.Windows.Forms.Label
            Me.btnCerrar = New System.Windows.Forms.Button
            Me.grpCriteriosBusqueda = New System.Windows.Forms.GroupBox
            Me.cboEstado = New SCGComboBox.SCGComboBox
            Me.txtNoVisita = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtNoVehiculo = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Panel4 = New System.Windows.Forms.Panel
            Me.txtNombreAsesor = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.picAsesor = New System.Windows.Forms.PictureBox
            Me.txtAsesor = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtCliente = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtCedula = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.picCliente = New System.Windows.Forms.PictureBox
            Me.txtCodCliente = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtCono = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtPlaca = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.cboModelo = New SCGComboBox.SCGComboBox
            Me.Label10 = New System.Windows.Forms.Label
            Me.chkModelo = New System.Windows.Forms.CheckBox
            Me.Panel5 = New System.Windows.Forms.Panel
            Me.Panel6 = New System.Windows.Forms.Panel
            Me.Panel7 = New System.Windows.Forms.Panel
            Me.dtpCierreini = New System.Windows.Forms.DateTimePicker
            Me.lblLine10 = New System.Windows.Forms.Label
            Me.dtpCompromisoini = New System.Windows.Forms.DateTimePicker
            Me.dtpAperturaini = New System.Windows.Forms.DateTimePicker
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.dtpCierrefin = New System.Windows.Forms.DateTimePicker
            Me.Panel3 = New System.Windows.Forms.Panel
            Me.dtpCompromisofin = New System.Windows.Forms.DateTimePicker
            Me.dtpAperturafin = New System.Windows.Forms.DateTimePicker
            Me.cboMarca = New SCGComboBox.SCGComboBox
            Me.cboEstilo = New SCGComboBox.SCGComboBox
            Me.Label9 = New System.Windows.Forms.Label
            Me.lblAsesor = New System.Windows.Forms.Label
            Me.Label7 = New System.Windows.Forms.Label
            Me.Label4 = New System.Windows.Forms.Label
            Me.lblIdentCliente = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.SubBuscador2 = New Buscador.SubBuscador
            Me.SubBuscador1 = New Buscador.SubBuscador
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.lblCliente = New System.Windows.Forms.Label
            Me.lblLine7 = New System.Windows.Forms.Label
            Me.lblLine4 = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.lblLine3 = New System.Windows.Forms.Label
            Me.Label6 = New System.Windows.Forms.Label
            Me.lblLine9 = New System.Windows.Forms.Label
            Me.lblLine8 = New System.Windows.Forms.Label
            Me.lblLine6 = New System.Windows.Forms.Label
            Me.lblLine2 = New System.Windows.Forms.Label
            Me.lblNoVisita = New System.Windows.Forms.Label
            Me.chkCompromiso = New System.Windows.Forms.CheckBox
            Me.chkCierre = New System.Windows.Forms.CheckBox
            Me.chkApertura = New System.Windows.Forms.CheckBox
            Me.chkMarca = New System.Windows.Forms.CheckBox
            Me.chkEstilo = New System.Windows.Forms.CheckBox
            Me.chkEstado = New System.Windows.Forms.CheckBox
            Me.tlbVisitas = New Proyecto_SCGToolBar.SCGToolBar
            OrdenTrabajoDatasetGrid = New DMSOneFramework.OrdenTrabajoDataset
            VisitaDatasetGrid = New DMSOneFramework.VisitaDataset
            CType(OrdenTrabajoDatasetGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(VisitaDatasetGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpCitas.SuspendLayout()
            CType(Me.dtgOrdenes, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtgVisitas, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpCriteriosBusqueda.SuspendLayout()
            CType(Me.picAsesor, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picCliente, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'OrdenTrabajoDatasetGrid
            '
            OrdenTrabajoDatasetGrid.DataSetName = "OrdenTrabajoDataset"
            OrdenTrabajoDatasetGrid.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'VisitaDatasetGrid
            '
            VisitaDatasetGrid.DataSetName = "VisitaDataset"
            VisitaDatasetGrid.Locale = New System.Globalization.CultureInfo("en-US")
            VisitaDatasetGrid.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'grpCitas
            '
            Me.grpCitas.BackColor = System.Drawing.SystemColors.Control
            Me.grpCitas.Controls.Add(Me.dtgOrdenes)
            Me.grpCitas.Controls.Add(Me.dtgVisitas)
            Me.grpCitas.Controls.Add(Me.Label5)
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
            Me.dtgOrdenes.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NoOrdenDataGridViewTextBoxColumn, Me.TipoDescDataGridViewTextBoxColumn, Me.DescipcionEstadoDataGridViewTextBoxColumn, Me.FechaaperturaDataGridViewTextBoxColumn1, Me.FechacompromisoDataGridViewTextBoxColumn1, Me.ObservacionDataGridViewTextBoxColumn, Me.PlacaDataGridViewTextBoxColumn1, Me.IDVehiculoDataGridViewTextBoxColumn1, Me.CodMarcaDataGridViewTextBoxColumn1, Me.DescMarcaDataGridViewTextBoxColumn1, Me.CodEstiloDataGridViewTextBoxColumn1, Me.DescEstiloDataGridViewTextBoxColumn1, Me.CodModeloDataGridViewTextBoxColumn1, Me.DescModeloDataGridViewTextBoxColumn1, Me.EstadoDataGridViewTextBoxColumn1, Me.EstadoDescDataGridViewTextBoxColumn, Me.ConoDataGridViewTextBoxColumn1, Me.NoVisitaDataGridViewTextBoxColumn1, Me.FechacierreDataGridViewTextBoxColumn1, Me.CodTipoOrdenDataGridViewTextBoxColumn, Me.EstadoVisitaDataGridViewTextBoxColumn, Me.EstadoVisitaDescDataGridViewTextBoxColumn, Me.NoVehiculoDataGridViewTextBoxColumn1, Me.CheckDataGridViewTextBoxColumn, Me.ClienteFacturarDataGridViewTextBoxColumn, Me.MontoReparacionDataGridViewTextBoxColumn, Me.AsesorDataGridViewTextBoxColumn1, Me.NoCotizacionDataGridViewTextBoxColumn, Me.HoraCompDataGridViewTextBoxColumn, Me.FechaCompDataGridViewTextBoxColumn, Me.OTPadreDataGridViewTextBoxColumn, Me.CardCodeOrigDataGridViewTextBoxColumn, Me.CardNameOrigDataGridViewTextBoxColumn, Me.VINDataGridViewTextBoxColumn, Me.AnoVehiculoDataGridViewTextBoxColumn, Me.NombreAsesorDataGridViewTextBoxColumn})
            Me.dtgOrdenes.DataMember = "SCGTA_TB_Orden"
            Me.dtgOrdenes.DataSource = OrdenTrabajoDatasetGrid
            Me.dtgOrdenes.GridColor = System.Drawing.Color.Silver
            resources.ApplyResources(Me.dtgOrdenes, "dtgOrdenes")
            Me.dtgOrdenes.Name = "dtgOrdenes"
            Me.dtgOrdenes.ReadOnly = True
            Me.dtgOrdenes.ShowEditingIcon = False
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
            'DescipcionEstadoDataGridViewTextBoxColumn
            '
            Me.DescipcionEstadoDataGridViewTextBoxColumn.DataPropertyName = "DescipcionEstado"
            resources.ApplyResources(Me.DescipcionEstadoDataGridViewTextBoxColumn, "DescipcionEstadoDataGridViewTextBoxColumn")
            Me.DescipcionEstadoDataGridViewTextBoxColumn.Name = "DescipcionEstadoDataGridViewTextBoxColumn"
            Me.DescipcionEstadoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FechaaperturaDataGridViewTextBoxColumn1
            '
            Me.FechaaperturaDataGridViewTextBoxColumn1.DataPropertyName = "Fecha_apertura"
            resources.ApplyResources(Me.FechaaperturaDataGridViewTextBoxColumn1, "FechaaperturaDataGridViewTextBoxColumn1")
            Me.FechaaperturaDataGridViewTextBoxColumn1.Name = "FechaaperturaDataGridViewTextBoxColumn1"
            Me.FechaaperturaDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'FechacompromisoDataGridViewTextBoxColumn1
            '
            Me.FechacompromisoDataGridViewTextBoxColumn1.DataPropertyName = "Fecha_compromiso"
            resources.ApplyResources(Me.FechacompromisoDataGridViewTextBoxColumn1, "FechacompromisoDataGridViewTextBoxColumn1")
            Me.FechacompromisoDataGridViewTextBoxColumn1.Name = "FechacompromisoDataGridViewTextBoxColumn1"
            Me.FechacompromisoDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'ObservacionDataGridViewTextBoxColumn
            '
            Me.ObservacionDataGridViewTextBoxColumn.DataPropertyName = "Observacion"
            resources.ApplyResources(Me.ObservacionDataGridViewTextBoxColumn, "ObservacionDataGridViewTextBoxColumn")
            Me.ObservacionDataGridViewTextBoxColumn.Name = "ObservacionDataGridViewTextBoxColumn"
            Me.ObservacionDataGridViewTextBoxColumn.ReadOnly = True
            '
            'PlacaDataGridViewTextBoxColumn1
            '
            Me.PlacaDataGridViewTextBoxColumn1.DataPropertyName = "Placa"
            resources.ApplyResources(Me.PlacaDataGridViewTextBoxColumn1, "PlacaDataGridViewTextBoxColumn1")
            Me.PlacaDataGridViewTextBoxColumn1.Name = "PlacaDataGridViewTextBoxColumn1"
            Me.PlacaDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'IDVehiculoDataGridViewTextBoxColumn1
            '
            Me.IDVehiculoDataGridViewTextBoxColumn1.DataPropertyName = "IDVehiculo"
            resources.ApplyResources(Me.IDVehiculoDataGridViewTextBoxColumn1, "IDVehiculoDataGridViewTextBoxColumn1")
            Me.IDVehiculoDataGridViewTextBoxColumn1.Name = "IDVehiculoDataGridViewTextBoxColumn1"
            Me.IDVehiculoDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'CodMarcaDataGridViewTextBoxColumn1
            '
            Me.CodMarcaDataGridViewTextBoxColumn1.DataPropertyName = "CodMarca"
            resources.ApplyResources(Me.CodMarcaDataGridViewTextBoxColumn1, "CodMarcaDataGridViewTextBoxColumn1")
            Me.CodMarcaDataGridViewTextBoxColumn1.Name = "CodMarcaDataGridViewTextBoxColumn1"
            Me.CodMarcaDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'DescMarcaDataGridViewTextBoxColumn1
            '
            Me.DescMarcaDataGridViewTextBoxColumn1.DataPropertyName = "DescMarca"
            resources.ApplyResources(Me.DescMarcaDataGridViewTextBoxColumn1, "DescMarcaDataGridViewTextBoxColumn1")
            Me.DescMarcaDataGridViewTextBoxColumn1.Name = "DescMarcaDataGridViewTextBoxColumn1"
            Me.DescMarcaDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'CodEstiloDataGridViewTextBoxColumn1
            '
            Me.CodEstiloDataGridViewTextBoxColumn1.DataPropertyName = "CodEstilo"
            resources.ApplyResources(Me.CodEstiloDataGridViewTextBoxColumn1, "CodEstiloDataGridViewTextBoxColumn1")
            Me.CodEstiloDataGridViewTextBoxColumn1.Name = "CodEstiloDataGridViewTextBoxColumn1"
            Me.CodEstiloDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'DescEstiloDataGridViewTextBoxColumn1
            '
            Me.DescEstiloDataGridViewTextBoxColumn1.DataPropertyName = "DescEstilo"
            resources.ApplyResources(Me.DescEstiloDataGridViewTextBoxColumn1, "DescEstiloDataGridViewTextBoxColumn1")
            Me.DescEstiloDataGridViewTextBoxColumn1.Name = "DescEstiloDataGridViewTextBoxColumn1"
            Me.DescEstiloDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'CodModeloDataGridViewTextBoxColumn1
            '
            Me.CodModeloDataGridViewTextBoxColumn1.DataPropertyName = "CodModelo"
            resources.ApplyResources(Me.CodModeloDataGridViewTextBoxColumn1, "CodModeloDataGridViewTextBoxColumn1")
            Me.CodModeloDataGridViewTextBoxColumn1.Name = "CodModeloDataGridViewTextBoxColumn1"
            Me.CodModeloDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'DescModeloDataGridViewTextBoxColumn1
            '
            Me.DescModeloDataGridViewTextBoxColumn1.DataPropertyName = "DescModelo"
            resources.ApplyResources(Me.DescModeloDataGridViewTextBoxColumn1, "DescModeloDataGridViewTextBoxColumn1")
            Me.DescModeloDataGridViewTextBoxColumn1.Name = "DescModeloDataGridViewTextBoxColumn1"
            Me.DescModeloDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'EstadoDataGridViewTextBoxColumn1
            '
            Me.EstadoDataGridViewTextBoxColumn1.DataPropertyName = "Estado"
            resources.ApplyResources(Me.EstadoDataGridViewTextBoxColumn1, "EstadoDataGridViewTextBoxColumn1")
            Me.EstadoDataGridViewTextBoxColumn1.Name = "EstadoDataGridViewTextBoxColumn1"
            Me.EstadoDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'EstadoDescDataGridViewTextBoxColumn
            '
            Me.EstadoDescDataGridViewTextBoxColumn.DataPropertyName = "EstadoDesc"
            resources.ApplyResources(Me.EstadoDescDataGridViewTextBoxColumn, "EstadoDescDataGridViewTextBoxColumn")
            Me.EstadoDescDataGridViewTextBoxColumn.Name = "EstadoDescDataGridViewTextBoxColumn"
            Me.EstadoDescDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ConoDataGridViewTextBoxColumn1
            '
            Me.ConoDataGridViewTextBoxColumn1.DataPropertyName = "Cono"
            resources.ApplyResources(Me.ConoDataGridViewTextBoxColumn1, "ConoDataGridViewTextBoxColumn1")
            Me.ConoDataGridViewTextBoxColumn1.Name = "ConoDataGridViewTextBoxColumn1"
            Me.ConoDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'NoVisitaDataGridViewTextBoxColumn1
            '
            Me.NoVisitaDataGridViewTextBoxColumn1.DataPropertyName = "NoVisita"
            resources.ApplyResources(Me.NoVisitaDataGridViewTextBoxColumn1, "NoVisitaDataGridViewTextBoxColumn1")
            Me.NoVisitaDataGridViewTextBoxColumn1.Name = "NoVisitaDataGridViewTextBoxColumn1"
            Me.NoVisitaDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'FechacierreDataGridViewTextBoxColumn1
            '
            Me.FechacierreDataGridViewTextBoxColumn1.DataPropertyName = "Fecha_cierre"
            resources.ApplyResources(Me.FechacierreDataGridViewTextBoxColumn1, "FechacierreDataGridViewTextBoxColumn1")
            Me.FechacierreDataGridViewTextBoxColumn1.Name = "FechacierreDataGridViewTextBoxColumn1"
            Me.FechacierreDataGridViewTextBoxColumn1.ReadOnly = True
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
            'NoVehiculoDataGridViewTextBoxColumn1
            '
            Me.NoVehiculoDataGridViewTextBoxColumn1.DataPropertyName = "NoVehiculo"
            resources.ApplyResources(Me.NoVehiculoDataGridViewTextBoxColumn1, "NoVehiculoDataGridViewTextBoxColumn1")
            Me.NoVehiculoDataGridViewTextBoxColumn1.Name = "NoVehiculoDataGridViewTextBoxColumn1"
            Me.NoVehiculoDataGridViewTextBoxColumn1.ReadOnly = True
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
            'AsesorDataGridViewTextBoxColumn1
            '
            Me.AsesorDataGridViewTextBoxColumn1.DataPropertyName = "Asesor"
            resources.ApplyResources(Me.AsesorDataGridViewTextBoxColumn1, "AsesorDataGridViewTextBoxColumn1")
            Me.AsesorDataGridViewTextBoxColumn1.Name = "AsesorDataGridViewTextBoxColumn1"
            Me.AsesorDataGridViewTextBoxColumn1.ReadOnly = True
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
            'dtgVisitas
            '
            Me.dtgVisitas.AllowUserToAddRows = False
            Me.dtgVisitas.AllowUserToDeleteRows = False
            Me.dtgVisitas.AutoGenerateColumns = False
            Me.dtgVisitas.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgVisitas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgVisitas.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NoVisitaDataGridViewTextBoxColumn, Me.CardNameDataGridViewTextBoxColumn, Me.NoVehiculoDataGridViewTextBoxColumn, Me.PlacaDataGridViewTextBoxColumn, Me.DescMarcaDataGridViewTextBoxColumn, Me.DescEstiloDataGridViewTextBoxColumn, Me.DescModeloDataGridViewTextBoxColumn, Me.DescripcionEstadoVisitaDataGridViewTextBoxColumn, Me.FechaaperturaDataGridViewTextBoxColumn, Me.EstadoDataGridViewTextBoxColumn, Me.FechacompromisoDataGridViewTextBoxColumn, Me.FechacierreDataGridViewTextBoxColumn, Me.AsesorDataGridViewTextBoxColumn, Me.CodColorDataGridViewTextBoxColumn, Me.CardCodeDataGridViewTextBoxColumn, Me.IDVehiculoDataGridViewTextBoxColumn, Me.ConoDataGridViewTextBoxColumn, Me.DescColorDataGridViewTextBoxColumn, Me.AsesorNombreDataGridViewTextBoxColumn, Me.IdentClienteDataGridViewTextBoxColumn, Me.HoracompromisoDataGridViewTextBoxColumn, Me.CotizacionDataGridViewTextBoxColumn, Me.CodMarcaDataGridViewTextBoxColumn, Me.CodModeloDataGridViewTextBoxColumn, Me.CodEstiloDataGridViewTextBoxColumn, Me.CodEstadoDataGridViewTextBoxColumn})
            Me.dtgVisitas.DataMember = "SCGTA_TB_Visita"
            Me.dtgVisitas.DataSource = VisitaDatasetGrid
            Me.dtgVisitas.GridColor = System.Drawing.Color.Silver
            resources.ApplyResources(Me.dtgVisitas, "dtgVisitas")
            Me.dtgVisitas.Name = "dtgVisitas"
            Me.dtgVisitas.ReadOnly = True
            '
            'NoVisitaDataGridViewTextBoxColumn
            '
            Me.NoVisitaDataGridViewTextBoxColumn.DataPropertyName = "NoVisita"
            resources.ApplyResources(Me.NoVisitaDataGridViewTextBoxColumn, "NoVisitaDataGridViewTextBoxColumn")
            Me.NoVisitaDataGridViewTextBoxColumn.Name = "NoVisitaDataGridViewTextBoxColumn"
            Me.NoVisitaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CardNameDataGridViewTextBoxColumn
            '
            Me.CardNameDataGridViewTextBoxColumn.DataPropertyName = "CardName"
            resources.ApplyResources(Me.CardNameDataGridViewTextBoxColumn, "CardNameDataGridViewTextBoxColumn")
            Me.CardNameDataGridViewTextBoxColumn.Name = "CardNameDataGridViewTextBoxColumn"
            Me.CardNameDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoVehiculoDataGridViewTextBoxColumn
            '
            Me.NoVehiculoDataGridViewTextBoxColumn.DataPropertyName = "NoVehiculo"
            resources.ApplyResources(Me.NoVehiculoDataGridViewTextBoxColumn, "NoVehiculoDataGridViewTextBoxColumn")
            Me.NoVehiculoDataGridViewTextBoxColumn.Name = "NoVehiculoDataGridViewTextBoxColumn"
            Me.NoVehiculoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'PlacaDataGridViewTextBoxColumn
            '
            Me.PlacaDataGridViewTextBoxColumn.DataPropertyName = "Placa"
            resources.ApplyResources(Me.PlacaDataGridViewTextBoxColumn, "PlacaDataGridViewTextBoxColumn")
            Me.PlacaDataGridViewTextBoxColumn.Name = "PlacaDataGridViewTextBoxColumn"
            Me.PlacaDataGridViewTextBoxColumn.ReadOnly = True
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
            'DescripcionEstadoVisitaDataGridViewTextBoxColumn
            '
            Me.DescripcionEstadoVisitaDataGridViewTextBoxColumn.DataPropertyName = "DescripcionEstadoVisita"
            resources.ApplyResources(Me.DescripcionEstadoVisitaDataGridViewTextBoxColumn, "DescripcionEstadoVisitaDataGridViewTextBoxColumn")
            Me.DescripcionEstadoVisitaDataGridViewTextBoxColumn.Name = "DescripcionEstadoVisitaDataGridViewTextBoxColumn"
            Me.DescripcionEstadoVisitaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FechaaperturaDataGridViewTextBoxColumn
            '
            Me.FechaaperturaDataGridViewTextBoxColumn.DataPropertyName = "Fecha_apertura"
            resources.ApplyResources(Me.FechaaperturaDataGridViewTextBoxColumn, "FechaaperturaDataGridViewTextBoxColumn")
            Me.FechaaperturaDataGridViewTextBoxColumn.Name = "FechaaperturaDataGridViewTextBoxColumn"
            Me.FechaaperturaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EstadoDataGridViewTextBoxColumn
            '
            Me.EstadoDataGridViewTextBoxColumn.DataPropertyName = "Estado"
            resources.ApplyResources(Me.EstadoDataGridViewTextBoxColumn, "EstadoDataGridViewTextBoxColumn")
            Me.EstadoDataGridViewTextBoxColumn.Name = "EstadoDataGridViewTextBoxColumn"
            Me.EstadoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FechacompromisoDataGridViewTextBoxColumn
            '
            Me.FechacompromisoDataGridViewTextBoxColumn.DataPropertyName = "Fecha_compromiso"
            resources.ApplyResources(Me.FechacompromisoDataGridViewTextBoxColumn, "FechacompromisoDataGridViewTextBoxColumn")
            Me.FechacompromisoDataGridViewTextBoxColumn.Name = "FechacompromisoDataGridViewTextBoxColumn"
            Me.FechacompromisoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FechacierreDataGridViewTextBoxColumn
            '
            Me.FechacierreDataGridViewTextBoxColumn.DataPropertyName = "Fecha_cierre"
            resources.ApplyResources(Me.FechacierreDataGridViewTextBoxColumn, "FechacierreDataGridViewTextBoxColumn")
            Me.FechacierreDataGridViewTextBoxColumn.Name = "FechacierreDataGridViewTextBoxColumn"
            Me.FechacierreDataGridViewTextBoxColumn.ReadOnly = True
            '
            'AsesorDataGridViewTextBoxColumn
            '
            Me.AsesorDataGridViewTextBoxColumn.DataPropertyName = "Asesor"
            resources.ApplyResources(Me.AsesorDataGridViewTextBoxColumn, "AsesorDataGridViewTextBoxColumn")
            Me.AsesorDataGridViewTextBoxColumn.Name = "AsesorDataGridViewTextBoxColumn"
            Me.AsesorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodColorDataGridViewTextBoxColumn
            '
            Me.CodColorDataGridViewTextBoxColumn.DataPropertyName = "CodColor"
            resources.ApplyResources(Me.CodColorDataGridViewTextBoxColumn, "CodColorDataGridViewTextBoxColumn")
            Me.CodColorDataGridViewTextBoxColumn.Name = "CodColorDataGridViewTextBoxColumn"
            Me.CodColorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CardCodeDataGridViewTextBoxColumn
            '
            Me.CardCodeDataGridViewTextBoxColumn.DataPropertyName = "CardCode"
            resources.ApplyResources(Me.CardCodeDataGridViewTextBoxColumn, "CardCodeDataGridViewTextBoxColumn")
            Me.CardCodeDataGridViewTextBoxColumn.Name = "CardCodeDataGridViewTextBoxColumn"
            Me.CardCodeDataGridViewTextBoxColumn.ReadOnly = True
            '
            'IDVehiculoDataGridViewTextBoxColumn
            '
            Me.IDVehiculoDataGridViewTextBoxColumn.DataPropertyName = "IDVehiculo"
            resources.ApplyResources(Me.IDVehiculoDataGridViewTextBoxColumn, "IDVehiculoDataGridViewTextBoxColumn")
            Me.IDVehiculoDataGridViewTextBoxColumn.Name = "IDVehiculoDataGridViewTextBoxColumn"
            Me.IDVehiculoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ConoDataGridViewTextBoxColumn
            '
            Me.ConoDataGridViewTextBoxColumn.DataPropertyName = "Cono"
            resources.ApplyResources(Me.ConoDataGridViewTextBoxColumn, "ConoDataGridViewTextBoxColumn")
            Me.ConoDataGridViewTextBoxColumn.Name = "ConoDataGridViewTextBoxColumn"
            Me.ConoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DescColorDataGridViewTextBoxColumn
            '
            Me.DescColorDataGridViewTextBoxColumn.DataPropertyName = "DescColor"
            resources.ApplyResources(Me.DescColorDataGridViewTextBoxColumn, "DescColorDataGridViewTextBoxColumn")
            Me.DescColorDataGridViewTextBoxColumn.Name = "DescColorDataGridViewTextBoxColumn"
            Me.DescColorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'AsesorNombreDataGridViewTextBoxColumn
            '
            Me.AsesorNombreDataGridViewTextBoxColumn.DataPropertyName = "AsesorNombre"
            resources.ApplyResources(Me.AsesorNombreDataGridViewTextBoxColumn, "AsesorNombreDataGridViewTextBoxColumn")
            Me.AsesorNombreDataGridViewTextBoxColumn.Name = "AsesorNombreDataGridViewTextBoxColumn"
            Me.AsesorNombreDataGridViewTextBoxColumn.ReadOnly = True
            '
            'IdentClienteDataGridViewTextBoxColumn
            '
            Me.IdentClienteDataGridViewTextBoxColumn.DataPropertyName = "IdentCliente"
            resources.ApplyResources(Me.IdentClienteDataGridViewTextBoxColumn, "IdentClienteDataGridViewTextBoxColumn")
            Me.IdentClienteDataGridViewTextBoxColumn.Name = "IdentClienteDataGridViewTextBoxColumn"
            Me.IdentClienteDataGridViewTextBoxColumn.ReadOnly = True
            '
            'HoracompromisoDataGridViewTextBoxColumn
            '
            Me.HoracompromisoDataGridViewTextBoxColumn.DataPropertyName = "Hora_compromiso"
            resources.ApplyResources(Me.HoracompromisoDataGridViewTextBoxColumn, "HoracompromisoDataGridViewTextBoxColumn")
            Me.HoracompromisoDataGridViewTextBoxColumn.Name = "HoracompromisoDataGridViewTextBoxColumn"
            Me.HoracompromisoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CotizacionDataGridViewTextBoxColumn
            '
            Me.CotizacionDataGridViewTextBoxColumn.DataPropertyName = "Cotizacion"
            resources.ApplyResources(Me.CotizacionDataGridViewTextBoxColumn, "CotizacionDataGridViewTextBoxColumn")
            Me.CotizacionDataGridViewTextBoxColumn.Name = "CotizacionDataGridViewTextBoxColumn"
            Me.CotizacionDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodMarcaDataGridViewTextBoxColumn
            '
            Me.CodMarcaDataGridViewTextBoxColumn.DataPropertyName = "CodMarca"
            resources.ApplyResources(Me.CodMarcaDataGridViewTextBoxColumn, "CodMarcaDataGridViewTextBoxColumn")
            Me.CodMarcaDataGridViewTextBoxColumn.Name = "CodMarcaDataGridViewTextBoxColumn"
            Me.CodMarcaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodModeloDataGridViewTextBoxColumn
            '
            Me.CodModeloDataGridViewTextBoxColumn.DataPropertyName = "CodModelo"
            resources.ApplyResources(Me.CodModeloDataGridViewTextBoxColumn, "CodModeloDataGridViewTextBoxColumn")
            Me.CodModeloDataGridViewTextBoxColumn.Name = "CodModeloDataGridViewTextBoxColumn"
            Me.CodModeloDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodEstiloDataGridViewTextBoxColumn
            '
            Me.CodEstiloDataGridViewTextBoxColumn.DataPropertyName = "CodEstilo"
            resources.ApplyResources(Me.CodEstiloDataGridViewTextBoxColumn, "CodEstiloDataGridViewTextBoxColumn")
            Me.CodEstiloDataGridViewTextBoxColumn.Name = "CodEstiloDataGridViewTextBoxColumn"
            Me.CodEstiloDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodEstadoDataGridViewTextBoxColumn
            '
            Me.CodEstadoDataGridViewTextBoxColumn.DataPropertyName = "CodEstado"
            resources.ApplyResources(Me.CodEstadoDataGridViewTextBoxColumn, "CodEstadoDataGridViewTextBoxColumn")
            Me.CodEstadoDataGridViewTextBoxColumn.Name = "CodEstadoDataGridViewTextBoxColumn"
            Me.CodEstadoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'Label5
            '
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label5.Name = "Label5"
            '
            'btnCerrar
            '
            Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            resources.ApplyResources(Me.btnCerrar, "btnCerrar")
            Me.btnCerrar.Name = "btnCerrar"
            '
            'grpCriteriosBusqueda
            '
            Me.grpCriteriosBusqueda.Controls.Add(Me.cboEstado)
            Me.grpCriteriosBusqueda.Controls.Add(Me.txtNoVisita)
            Me.grpCriteriosBusqueda.Controls.Add(Me.txtNoVehiculo)
            Me.grpCriteriosBusqueda.Controls.Add(Me.Panel4)
            Me.grpCriteriosBusqueda.Controls.Add(Me.txtNombreAsesor)
            Me.grpCriteriosBusqueda.Controls.Add(Me.picAsesor)
            Me.grpCriteriosBusqueda.Controls.Add(Me.txtAsesor)
            Me.grpCriteriosBusqueda.Controls.Add(Me.txtCliente)
            Me.grpCriteriosBusqueda.Controls.Add(Me.txtCedula)
            Me.grpCriteriosBusqueda.Controls.Add(Me.picCliente)
            Me.grpCriteriosBusqueda.Controls.Add(Me.txtCodCliente)
            Me.grpCriteriosBusqueda.Controls.Add(Me.txtCono)
            Me.grpCriteriosBusqueda.Controls.Add(Me.txtPlaca)
            Me.grpCriteriosBusqueda.Controls.Add(Me.cboModelo)
            Me.grpCriteriosBusqueda.Controls.Add(Me.Label10)
            Me.grpCriteriosBusqueda.Controls.Add(Me.chkModelo)
            Me.grpCriteriosBusqueda.Controls.Add(Me.Panel5)
            Me.grpCriteriosBusqueda.Controls.Add(Me.Panel6)
            Me.grpCriteriosBusqueda.Controls.Add(Me.Panel7)
            Me.grpCriteriosBusqueda.Controls.Add(Me.dtpCierreini)
            Me.grpCriteriosBusqueda.Controls.Add(Me.lblLine10)
            Me.grpCriteriosBusqueda.Controls.Add(Me.dtpCompromisoini)
            Me.grpCriteriosBusqueda.Controls.Add(Me.dtpAperturaini)
            Me.grpCriteriosBusqueda.Controls.Add(Me.Panel1)
            Me.grpCriteriosBusqueda.Controls.Add(Me.dtpCierrefin)
            Me.grpCriteriosBusqueda.Controls.Add(Me.Panel3)
            Me.grpCriteriosBusqueda.Controls.Add(Me.dtpCompromisofin)
            Me.grpCriteriosBusqueda.Controls.Add(Me.dtpAperturafin)
            Me.grpCriteriosBusqueda.Controls.Add(Me.cboMarca)
            Me.grpCriteriosBusqueda.Controls.Add(Me.cboEstilo)
            Me.grpCriteriosBusqueda.Controls.Add(Me.Label9)
            Me.grpCriteriosBusqueda.Controls.Add(Me.lblAsesor)
            Me.grpCriteriosBusqueda.Controls.Add(Me.Label7)
            Me.grpCriteriosBusqueda.Controls.Add(Me.Label4)
            Me.grpCriteriosBusqueda.Controls.Add(Me.lblIdentCliente)
            Me.grpCriteriosBusqueda.Controls.Add(Me.Label1)
            Me.grpCriteriosBusqueda.Controls.Add(Me.Label2)
            Me.grpCriteriosBusqueda.Controls.Add(Me.SubBuscador2)
            Me.grpCriteriosBusqueda.Controls.Add(Me.SubBuscador1)
            Me.grpCriteriosBusqueda.Controls.Add(Me.lblLine1)
            Me.grpCriteriosBusqueda.Controls.Add(Me.lblCliente)
            Me.grpCriteriosBusqueda.Controls.Add(Me.lblLine7)
            Me.grpCriteriosBusqueda.Controls.Add(Me.lblLine4)
            Me.grpCriteriosBusqueda.Controls.Add(Me.Label3)
            Me.grpCriteriosBusqueda.Controls.Add(Me.lblLine3)
            Me.grpCriteriosBusqueda.Controls.Add(Me.Label6)
            Me.grpCriteriosBusqueda.Controls.Add(Me.lblLine9)
            Me.grpCriteriosBusqueda.Controls.Add(Me.lblLine8)
            Me.grpCriteriosBusqueda.Controls.Add(Me.lblLine6)
            Me.grpCriteriosBusqueda.Controls.Add(Me.lblLine2)
            Me.grpCriteriosBusqueda.Controls.Add(Me.lblNoVisita)
            Me.grpCriteriosBusqueda.Controls.Add(Me.chkCompromiso)
            Me.grpCriteriosBusqueda.Controls.Add(Me.chkCierre)
            Me.grpCriteriosBusqueda.Controls.Add(Me.chkApertura)
            Me.grpCriteriosBusqueda.Controls.Add(Me.chkMarca)
            Me.grpCriteriosBusqueda.Controls.Add(Me.chkEstilo)
            Me.grpCriteriosBusqueda.Controls.Add(Me.chkEstado)
            resources.ApplyResources(Me.grpCriteriosBusqueda, "grpCriteriosBusqueda")
            Me.grpCriteriosBusqueda.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.grpCriteriosBusqueda.Name = "grpCriteriosBusqueda"
            Me.grpCriteriosBusqueda.TabStop = False
            '
            'cboEstado
            '
            Me.cboEstado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstado.EstiloSBO = True
            resources.ApplyResources(Me.cboEstado, "cboEstado")
            Me.cboEstado.Name = "cboEstado"
            '
            'txtNoVisita
            '
            Me.txtNoVisita.AceptaNegativos = False
            Me.txtNoVisita.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoVisita.EstiloSBO = True
            resources.ApplyResources(Me.txtNoVisita, "txtNoVisita")
            Me.txtNoVisita.MaxDecimales = 0
            Me.txtNoVisita.MaxEnteros = 0
            Me.txtNoVisita.Millares = False
            Me.txtNoVisita.Name = "txtNoVisita"
            Me.txtNoVisita.Size_AdjustableHeight = 20
            Me.txtNoVisita.TeclasDeshacer = True
            Me.txtNoVisita.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.Numeric
            '
            'txtNoVehiculo
            '
            Me.txtNoVehiculo.AceptaNegativos = False
            Me.txtNoVehiculo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoVehiculo.EstiloSBO = True
            resources.ApplyResources(Me.txtNoVehiculo, "txtNoVehiculo")
            Me.txtNoVehiculo.MaxDecimales = 0
            Me.txtNoVehiculo.MaxEnteros = 0
            Me.txtNoVehiculo.Millares = False
            Me.txtNoVehiculo.Name = "txtNoVehiculo"
            Me.txtNoVehiculo.Size_AdjustableHeight = 20
            Me.txtNoVehiculo.TeclasDeshacer = True
            Me.txtNoVehiculo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Panel4
            '
            resources.ApplyResources(Me.Panel4, "Panel4")
            Me.Panel4.Name = "Panel4"
            '
            'txtNombreAsesor
            '
            Me.txtNombreAsesor.AceptaNegativos = False
            Me.txtNombreAsesor.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
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
            'picAsesor
            '
            Me.picAsesor.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picAsesor, "picAsesor")
            Me.picAsesor.Name = "picAsesor"
            Me.picAsesor.TabStop = False
            '
            'txtAsesor
            '
            Me.txtAsesor.AceptaNegativos = False
            Me.txtAsesor.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtAsesor.EstiloSBO = True
            resources.ApplyResources(Me.txtAsesor, "txtAsesor")
            Me.txtAsesor.MaxDecimales = 0
            Me.txtAsesor.MaxEnteros = 0
            Me.txtAsesor.Millares = False
            Me.txtAsesor.Name = "txtAsesor"
            Me.txtAsesor.ReadOnly = True
            Me.txtAsesor.Size_AdjustableHeight = 20
            Me.txtAsesor.TeclasDeshacer = True
            Me.txtAsesor.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtCliente
            '
            Me.txtCliente.AceptaNegativos = False
            Me.txtCliente.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtCliente.EstiloSBO = True
            resources.ApplyResources(Me.txtCliente, "txtCliente")
            Me.txtCliente.MaxDecimales = 0
            Me.txtCliente.MaxEnteros = 0
            Me.txtCliente.Millares = False
            Me.txtCliente.Name = "txtCliente"
            Me.txtCliente.ReadOnly = True
            Me.txtCliente.Size_AdjustableHeight = 20
            Me.txtCliente.TeclasDeshacer = True
            Me.txtCliente.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtCedula
            '
            Me.txtCedula.AceptaNegativos = False
            Me.txtCedula.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtCedula.EstiloSBO = True
            resources.ApplyResources(Me.txtCedula, "txtCedula")
            Me.txtCedula.MaxDecimales = 0
            Me.txtCedula.MaxEnteros = 0
            Me.txtCedula.Millares = False
            Me.txtCedula.Name = "txtCedula"
            Me.txtCedula.ReadOnly = True
            Me.txtCedula.Size_AdjustableHeight = 20
            Me.txtCedula.TeclasDeshacer = True
            Me.txtCedula.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.Numeric
            '
            'picCliente
            '
            Me.picCliente.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picCliente, "picCliente")
            Me.picCliente.Name = "picCliente"
            Me.picCliente.TabStop = False
            '
            'txtCodCliente
            '
            Me.txtCodCliente.AceptaNegativos = False
            Me.txtCodCliente.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
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
            'txtCono
            '
            Me.txtCono.AceptaNegativos = False
            Me.txtCono.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtCono.EstiloSBO = True
            resources.ApplyResources(Me.txtCono, "txtCono")
            Me.txtCono.MaxDecimales = 0
            Me.txtCono.MaxEnteros = 0
            Me.txtCono.Millares = False
            Me.txtCono.Name = "txtCono"
            Me.txtCono.Size_AdjustableHeight = 20
            Me.txtCono.TeclasDeshacer = True
            Me.txtCono.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.Numeric
            '
            'txtPlaca
            '
            Me.txtPlaca.AceptaNegativos = False
            Me.txtPlaca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtPlaca.EstiloSBO = True
            resources.ApplyResources(Me.txtPlaca, "txtPlaca")
            Me.txtPlaca.MaxDecimales = 0
            Me.txtPlaca.MaxEnteros = 0
            Me.txtPlaca.Millares = False
            Me.txtPlaca.Name = "txtPlaca"
            Me.txtPlaca.Size_AdjustableHeight = 20
            Me.txtPlaca.TeclasDeshacer = True
            Me.txtPlaca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'cboModelo
            '
            Me.cboModelo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboModelo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboModelo.EstiloSBO = True
            resources.ApplyResources(Me.cboModelo, "cboModelo")
            Me.cboModelo.Name = "cboModelo"
            '
            'Label10
            '
            Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label10, "Label10")
            Me.Label10.Name = "Label10"
            '
            'chkModelo
            '
            resources.ApplyResources(Me.chkModelo, "chkModelo")
            Me.chkModelo.Name = "chkModelo"
            Me.chkModelo.UseVisualStyleBackColor = True
            '
            'Panel5
            '
            resources.ApplyResources(Me.Panel5, "Panel5")
            Me.Panel5.Name = "Panel5"
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
            'dtpCierreini
            '
            Me.dtpCierreini.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCierreini.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCierreini.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCierreini.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCierreini.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpCierreini, "dtpCierreini")
            Me.dtpCierreini.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpCierreini.Name = "dtpCierreini"
            Me.dtpCierreini.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'lblLine10
            '
            Me.lblLine10.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine10, "lblLine10")
            Me.lblLine10.Name = "lblLine10"
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
            'Panel1
            '
            resources.ApplyResources(Me.Panel1, "Panel1")
            Me.Panel1.Name = "Panel1"
            '
            'dtpCierrefin
            '
            Me.dtpCierrefin.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCierrefin.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCierrefin.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCierrefin.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCierrefin.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpCierrefin, "dtpCierrefin")
            Me.dtpCierrefin.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpCierrefin.Name = "dtpCierrefin"
            Me.dtpCierrefin.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
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
            'cboMarca
            '
            Me.cboMarca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboMarca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboMarca.EstiloSBO = True
            resources.ApplyResources(Me.cboMarca, "cboMarca")
            Me.cboMarca.Name = "cboMarca"
            '
            'cboEstilo
            '
            Me.cboEstilo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboEstilo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstilo.EstiloSBO = True
            resources.ApplyResources(Me.cboEstilo, "cboEstilo")
            Me.cboEstilo.Name = "cboEstilo"
            '
            'Label9
            '
            Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label9, "Label9")
            Me.Label9.Name = "Label9"
            '
            'lblAsesor
            '
            resources.ApplyResources(Me.lblAsesor, "lblAsesor")
            Me.lblAsesor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblAsesor.Name = "lblAsesor"
            '
            'Label7
            '
            Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label7, "Label7")
            Me.Label7.Name = "Label7"
            '
            'Label4
            '
            Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Name = "Label4"
            '
            'lblIdentCliente
            '
            resources.ApplyResources(Me.lblIdentCliente, "lblIdentCliente")
            Me.lblIdentCliente.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblIdentCliente.Name = "lblIdentCliente"
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'Label2
            '
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label2.Name = "Label2"
            '
            'SubBuscador2
            '
            Me.SubBuscador2.BackColor = System.Drawing.Color.Black
            Me.SubBuscador2.Barra_Titulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador2.ConsultarDBPorFiltrado = False
            Me.SubBuscador2.Criterios = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador2.Criterios_Ocultos = 0
            Me.SubBuscador2.Criterios_OcultosEx = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador2.IN_DataTable = Nothing
            resources.ApplyResources(Me.SubBuscador2, "SubBuscador2")
            Me.SubBuscador2.MultiSeleccion = False
            Me.SubBuscador2.Name = "SubBuscador2"
            Me.SubBuscador2.SQL_Cnn = Nothing
            Me.SubBuscador2.Tabla = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador2.Titulos = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador2.Where = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'SubBuscador1
            '
            Me.SubBuscador1.BackColor = System.Drawing.Color.Black
            Me.SubBuscador1.Barra_Titulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador1.ConsultarDBPorFiltrado = False
            Me.SubBuscador1.Criterios = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador1.Criterios_Ocultos = 0
            Me.SubBuscador1.Criterios_OcultosEx = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador1.IN_DataTable = Nothing
            resources.ApplyResources(Me.SubBuscador1, "SubBuscador1")
            Me.SubBuscador1.MultiSeleccion = False
            Me.SubBuscador1.Name = "SubBuscador1"
            Me.SubBuscador1.SQL_Cnn = Nothing
            Me.SubBuscador1.Tabla = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador1.Titulos = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador1.Where = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'lblLine1
            '
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.Name = "lblLine1"
            '
            'lblCliente
            '
            resources.ApplyResources(Me.lblCliente, "lblCliente")
            Me.lblCliente.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCliente.Name = "lblCliente"
            '
            'lblLine7
            '
            Me.lblLine7.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine7, "lblLine7")
            Me.lblLine7.Name = "lblLine7"
            '
            'lblLine4
            '
            Me.lblLine4.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine4, "lblLine4")
            Me.lblLine4.Name = "lblLine4"
            '
            'Label3
            '
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label3.Name = "Label3"
            '
            'lblLine3
            '
            Me.lblLine3.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine3, "lblLine3")
            Me.lblLine3.Name = "lblLine3"
            '
            'Label6
            '
            resources.ApplyResources(Me.Label6, "Label6")
            Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label6.Name = "Label6"
            '
            'lblLine9
            '
            Me.lblLine9.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine9, "lblLine9")
            Me.lblLine9.Name = "lblLine9"
            '
            'lblLine8
            '
            Me.lblLine8.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine8, "lblLine8")
            Me.lblLine8.Name = "lblLine8"
            '
            'lblLine6
            '
            Me.lblLine6.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine6, "lblLine6")
            Me.lblLine6.Name = "lblLine6"
            '
            'lblLine2
            '
            Me.lblLine2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine2, "lblLine2")
            Me.lblLine2.Name = "lblLine2"
            '
            'lblNoVisita
            '
            resources.ApplyResources(Me.lblNoVisita, "lblNoVisita")
            Me.lblNoVisita.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoVisita.Name = "lblNoVisita"
            '
            'chkCompromiso
            '
            resources.ApplyResources(Me.chkCompromiso, "chkCompromiso")
            Me.chkCompromiso.Name = "chkCompromiso"
            Me.chkCompromiso.UseVisualStyleBackColor = True
            '
            'chkCierre
            '
            resources.ApplyResources(Me.chkCierre, "chkCierre")
            Me.chkCierre.Name = "chkCierre"
            Me.chkCierre.UseVisualStyleBackColor = True
            '
            'chkApertura
            '
            resources.ApplyResources(Me.chkApertura, "chkApertura")
            Me.chkApertura.Name = "chkApertura"
            Me.chkApertura.UseVisualStyleBackColor = True
            '
            'chkMarca
            '
            resources.ApplyResources(Me.chkMarca, "chkMarca")
            Me.chkMarca.Name = "chkMarca"
            Me.chkMarca.UseVisualStyleBackColor = True
            '
            'chkEstilo
            '
            resources.ApplyResources(Me.chkEstilo, "chkEstilo")
            Me.chkEstilo.Name = "chkEstilo"
            Me.chkEstilo.UseVisualStyleBackColor = True
            '
            'chkEstado
            '
            resources.ApplyResources(Me.chkEstado, "chkEstado")
            Me.chkEstado.Name = "chkEstado"
            Me.chkEstado.UseVisualStyleBackColor = True
            '
            'tlbVisitas
            '
            resources.ApplyResources(Me.tlbVisitas, "tlbVisitas")
            Me.tlbVisitas.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.tlbVisitas.Name = "tlbVisitas"
            '
            'frmVisitas
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.grpCriteriosBusqueda)
            Me.Controls.Add(Me.grpCitas)
            Me.Controls.Add(Me.tlbVisitas)
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.Name = "frmVisitas"
            Me.Tag = "Servicio al Cliente,1"
            CType(OrdenTrabajoDatasetGrid, System.ComponentModel.ISupportInitialize).EndInit()
            CType(VisitaDatasetGrid, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpCitas.ResumeLayout(False)
            CType(Me.dtgOrdenes, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtgVisitas, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpCriteriosBusqueda.ResumeLayout(False)
            Me.grpCriteriosBusqueda.PerformLayout()
            CType(Me.picAsesor, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picCliente, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

#End Region

#Region "Declaraciones"

        'Declaracion de objeto dataAdapter y Dataset.
        Private m_adpVisita As SCGDataAccess.VisitasDataAdapter
        Public m_dstVisita As VisitaDataset

        Private m_adpOrden As SCGDataAccess.OrdenTrabajoDataAdapter
        Public m_dstOrden As OrdenTrabajoDataset


        'Declaración de las constantes con el nombre de las columnas del Dataset Visita
        Private Const mc_strNoVisita As String = "NoVisita"
        Private Const mc_strCodEstado As String = "CodEstado"
        Private Const mc_strCodModelo As String = "CodModelo"
        Private Const mc_strNoVehiculo As String = "NoVehiculo"
        Private Const mc_strCodMarca As String = "CodMarca"
        Private Const mc_strCodEstilo As String = "CodEstilo"
        Private Const mc_strDescEstilo As String = "DescEstilo"
        Private Const mc_strDescModelo As String = "DescModelo"
        Private Const mc_strDescMarca As String = "DescMarca"
        Private Const mc_strFecha_apertura As String = "Fecha_apertura"
        Private Const mc_strFecha_compromiso As String = "Fecha_compromiso"
        Private Const mc_strFecha_cierre As String = "Fecha_cierre"
        Private Const mc_strFecha_apertura_ini As String = "Fecha_apertura_ini"
        Private Const mc_strFecha_compromiso_ini As String = "Fecha_compromiso_ini"
        Private Const mc_strFecha_cierre_ini As String = "Fecha_cierre_ini"
        Private Const mc_strFecha_apertura_fin As String = "Fecha_apertura_fin"
        Private Const mc_strFecha_compromiso_fin As String = "Fecha_compromiso_fin"
        Private Const mc_strFecha_cierre_fin As String = "Fecha_cierre_fin"
        Private Const mc_strFecha_entrega As String = "Fecha_entrega"
        Private Const mc_strAsesor As String = "Asesor"
        Private Const mc_strCono As String = "Cono"
        Private Const mc_strPlaca As String = "Placa"
        Private Const mc_strEstado As String = "Estado"
        Private Const mc_strCardName As String = "CardName"
        Private Const mc_strCardCode As String = "CardCode"
        Private Const mc_strDescTipoOrden As String = "TipoDesc"
        Private Const mc_strDescripcionEstadoVisita As String = "DescripcionEstadoVisita"


        'Declaración de las constantes con el nombre de las columnas del Dataset Orden
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strNoFaseActual As String = "NoFaseActual"
        Private Const mc_strCodTipoOrden As String = "CodTipoOrden"
        Private Const mc_strNoVisitaOrden As String = "FaseProduccion"
        Private Const mc_strFecha_aperturaOrden As String = "Fecha_apertura"
        Private Const mc_strFecha_cierreOrden As String = "Fecha_cierre"
        Private Const mc_strEstadoOrden As String = "EstadoDesc"
        Private Const mc_strPaneles As String = "Paneles"
        Private Const mc_strObservacion As String = "Observacion"
        Private Const mc_strRepuestosPendiente As String = "RepuestosPendiente"
        Private Const mc_strTiempoAprobado As String = "TiempoAprobado"
        Private Const mc_strTiempoTaller As String = "TiempoTaller"
        Private Const mc_stProrrateo As String = "Prorrateo"
        Private Const mc_strPorcentajeProrrateo As String = "PorcentajeProrrateo"
        Private Const mc_strCostoManoObra As String = "CostoManoObra"
        Private Const mc_strCostoSuministro As String = "CostoSuministro"
        Private Const mc_strCostoSuministroPintura As String = "CostoSuministroPintura"
        Private Const mc_strCodMarcaOrden As String = "CodMarca"
        Private Const mc_strPrioridad As String = "Prioridad"
        Private Const mc_strFaseDescripcion As String = "FaseDescripcion"
        Private Const mc_strMarcaOrden As String = "MarcaDescripcion"
        Private Const mc_strModelo As String = "ModeloDescripcion"
        Private Const mc_strDescripcionEstado As String = "DescipcionEstado"

        Private Const mc_strMarca As String = "DescMarca"


        'Parametros que sirven para sabe cual símbolo utilizar en la búsqueda de Visitas según las fechas.
        Private strSimbolo As String


        'Nombre de la constante de la tabla
        Private mc_strTableName As String = "SCGTA_TB_Visita"

        'Tipo de inserción si es una actualización en la base de datos o una inserción.
        Private intTipoInsercion As Integer

        'Declaracion de un row del dataset, el cual sirve para insertar como para modificar y eliminar.
        Private drwVisita As VisitaDataset.SCGTA_TB_VisitaRow

        Private m_adpMarcas As MarcaDataAdapter
        Private m_drdMarcas As SqlDataReader
        Private m_adpEstilos As EstiloDataAdapter
        Private m_drdEstilos As SqlDataReader
        Private m_adpModelos As ModelosDataAdapter
        Private m_drdModelos As SqlDataReader

        Private objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        'Variables para la búsqueda
        Private m_intNoVisita As Integer
        Private m_strNoVehiculo As String
        Private m_strCardCode As String
        Private m_strAsesor As String
        Private m_intCodMarca As Integer
        Private m_intCodEstilo As Integer
        Private m_intCodModelo As Integer
        Private m_strPlaca As String
        Private m_dtApertura_ini As Date
        Private m_dtCompromiso_ini As Date
        Private m_dtCierre_ini As Date
        Private m_dtApertura_fin As Date
        Private m_dtCompromiso_fin As Date
        Private m_dtCierre_fin As Date
        Private m_intCodEstado As Integer
        Private m_strCono As String
        Private m_intTipoInsercion As Integer


        Private WithEvents objfrmOperYOrden As frmOrden
        Private WithEvents objfrmCtrlVisita As frmDetalleVisita

#End Region

#Region "Eventos"

        Private Sub frmVisitas_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
            If Asc(e.KeyChar) = Keys.Escape Then Me.Close()
        End Sub

#Region "Toolbar"


        Private Sub ScgToolBar1_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbVisitas.Click_Cerrar

            Try
                Me.Close()
                m_dstVisita.Dispose()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub


        Private Sub ScgToolBar1_Click_Eliminar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbVisitas.Click_Eliminar


        End Sub


        Private Sub ScgToolBar1_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbVisitas.Click_Nuevo

            Try


                tlbVisitas.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Buscar).Enabled = True

                tlbVisitas.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True

                tlbVisitas.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = True

                'Dim form As frmCtrlCrearVisita

                'If IsNothing(form) Then

                '    form = New frmCtrlCrearVisita
                '    Me.AddOwnedForm(form)


                'End If

                'form.ShowDialog()

                limpiarCriteriosBusqueda()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub


        Private Sub ScgToolBar1_Click_Buscar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbVisitas.Click_Buscar
            Try


                tlbVisitas.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = True

                BusquedaVisitas()


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub


        Private Sub ScgToolBar1_Click_Cancelar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbVisitas.Click_Cancelar
            Try
                'Limpia los txt y combos
                limpiarCriteriosBusqueda()
                'Se deben limpiar todas las variables de busqueda
                'limpiarCriteriosBusqueda()
                limpiarVariablesBusqueda()

                'dtgVisitas.DataSource = Nothing
                'dtgOrdenes.DataSource = Nothing

                m_dstVisita.Clear()
                m_dstOrden.Clear()


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub


#End Region

#Region "Validacion del KeyPress Enter en los criterios de busqueda"


        Private Sub txtCono_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCono.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub txtNoVisita_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNoVisita.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
        End Sub

        Private Sub txtPlaca_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPlaca.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub cboEstado_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboEstado.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub cboMarca_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboMarca.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub txtNoVehiculo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNoVehiculo.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtpCompromiso_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpCompromisoini.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtpApertura_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpAperturaini.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtpCierre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpCierreini.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub
#End Region

        Private Sub frmVisitas_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                dtpAperturaini.Value = objUtilitarios.CargarFechaHoraServidor.Date
                dtpCierreini.Value = objUtilitarios.CargarFechaHoraServidor.Date
                dtpCompromisoini.Value = objUtilitarios.CargarFechaHoraServidor.Date

                dtpAperturafin.Value = objUtilitarios.CargarFechaHoraServidor.Date
                dtpCierrefin.Value = objUtilitarios.CargarFechaHoraServidor.Date
                dtpCompromisofin.Value = objUtilitarios.CargarFechaHoraServidor.Date

                m_adpVisita = New SCGDataAccess.VisitasDataAdapter

                m_dstVisita = New VisitaDataset

                'Se ocultan los botones del toolbar que no se van utilizar
                tlbVisitas.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Visible = False
                tlbVisitas.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
                tlbVisitas.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False
                tlbVisitas.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Visible = False
                tlbVisitas.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Visible = False

                'clsUtilidadCombos.CargarComboEstadoVisitas(cboEstado)
                'cboEstado.SelectedIndex = -1

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub dtgVisitas_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgVisitas.CellDoubleClick

            Dim Forma_Nueva As Form

            Dim blnExisteForm As Boolean

            Try


                If m_dstVisita.SCGTA_TB_Visita.Rows.Count <> 0 Then
                    For Each Forma_Nueva In Me.MdiParent.MdiChildren

                        If Forma_Nueva.Name = "frmCtrlVisita" Then
                            blnExisteForm = True
                        End If

                    Next

                    If Not blnExisteForm Then

                        objfrmCtrlVisita = New frmDetalleVisita

                        objfrmCtrlVisita.MdiParent = Me.MdiParent

                        objfrmCtrlVisita.cargarDatos(m_dstVisita, drwVisita.NoVisita)

                        objfrmCtrlVisita.Show()

                        dtgVisitas.DataSource = m_dstVisita

                        dtgVisitas.Refresh()

                        limpiarCriteriosBusqueda()

                    End If
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub dtgVisitas_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgVisitas.CurrentCellChanged

            Try
                Me.Cursor = Cursors.WaitCursor

                'llama a la función que  cambia el detalle de servicio segun sea la celda seleccionada
                Call MostrarDetalle()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
                Me.Cursor = Cursors.Arrow
            End Try

        End Sub

        Private Sub picCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picCliente.Click
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                SubBuscador1.SQL_Cnn = DATemp.ObtieneConexion
                SubBuscador1.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorClientes
                SubBuscador1.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre & "," & My.Resources.ResourceUI.Identificacion
                SubBuscador1.Criterios = "CardCode, CardName, LicTradNum"
                SubBuscador1.Tabla = "SCGTA_VW_Clientes"
                SubBuscador1.Where = ""
                SubBuscador1.Activar_Buscador(sender)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub SubBuscador_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles SubBuscador1.AppAceptar
            Try

                txtCliente.Text = Arreglo_Campos(1)
                txtCodCliente.Text = Arreglo_Campos(0)
                txtCedula.Text = Arreglo_Campos(2)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub SubBuscador_AppAceptar2(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles SubBuscador2.AppAceptar
            Try

                txtAsesor.Text = Arreglo_Campos(0)
                txtNombreAsesor.Text = Arreglo_Campos(1)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub frmVisitas_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed

            Try

                objfrmOperYOrden = Nothing

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        'Private Sub objfrmConfEstadoExp_RetornaDatos() Handles objfrmConfEstadoExp.RetornaDatos
        '    objUtilitarios.CargarCombos(Me.cboEstado, 9)
        '    Me.cboEstado.Items.Add("")
        'End Sub

        'Private Sub objfrmConfMarcas_RetornaDatos() Handles objfrmConfMarcas.RetornaDatos

        '    objUtilitarios.CargarCombos(Me.cboMarca, 3)
        '    Me.cboMarca.Items.Add("")

        'End Sub

        Private Sub txtCono_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCono.LostFocus

            If txtCono.Text = 0 Then
                txtCono.Clear()
            End If

        End Sub

        Private Sub txtNoVisita_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNoVisita.LostFocus

            If txtNoVisita.Text = 0 Then
                txtNoVisita.Clear()
            End If

        End Sub

        Private Sub frmVisitas_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

            dtgVisitas.Update()

        End Sub

        Private Sub picAsesor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picAsesor.Click

            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                SubBuscador2.SQL_Cnn = DATemp.ObtieneConexion
                SubBuscador2.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorEmpleados
                SubBuscador2.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre
                SubBuscador2.Criterios = "empID, Nombre"
                SubBuscador2.Tabla = "SCGTA_VW_EMPLEADOS"
                SubBuscador2.Where = "branch = " & G_strIDSucursal
                SubBuscador2.Activar_Buscador(sender)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub chkMarca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMarca.Click
            Try

                If chkMarca.Checked Then

                    m_adpMarcas = New MarcaDataAdapter
                    m_drdMarcas = Nothing

                    m_adpMarcas.CargaMarcasdeVehiculo(m_drdMarcas)
                    Utilitarios.CargarComboSourceByReader(cboMarca, m_drdMarcas)

                Else

                    cboMarca.DataSource = Nothing
                    cboEstilo.DataSource = Nothing
                    chkEstilo.Checked = False
                    chkModelo.Checked = False

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

            Finally

                m_drdMarcas = Nothing
                m_adpMarcas = Nothing

                'Agregado 01072010
                If m_drdMarcas IsNot Nothing Then
                    If Not m_drdMarcas.IsClosed Then
                        Call m_drdMarcas.Close()
                    End If
                End If

            End Try

        End Sub

        Private Sub chkEstilo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEstilo.CheckedChanged


            Try

                If chkMarca.Checked Then
                    If chkEstilo.Checked Then

                        m_adpEstilos = New EstiloDataAdapter
                        m_drdEstilos = Nothing

                        m_adpEstilos.CargaEstilosdeVehiculo(m_drdEstilos, cboMarca.SelectedValue)
                        Utilitarios.CargarComboSourceByReader(cboEstilo, m_drdEstilos)
                    Else
                        cboEstilo.DataSource = Nothing
                        chkModelo.Checked = False
                        cboModelo.DataSource = Nothing

                    End If
                Else
                    'ENVIAR MENSAJE
                    chkModelo.Checked = False
                    cboModelo.DataSource = Nothing
                    cboEstilo.DataSource = Nothing
                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally

                m_drdMarcas = Nothing
                m_adpMarcas = Nothing

                'Agregado 01072010
                If m_drdEstilos IsNot Nothing Then
                    If Not m_drdEstilos.IsClosed Then
                        Call m_drdEstilos.Close()
                    End If
                End If

            End Try

        End Sub

        Private Sub cboMarca_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMarca.SelectedIndexChanged
            Try
                If chkEstilo.Checked Then

                    m_adpEstilos = New EstiloDataAdapter
                    m_drdEstilos = Nothing

                    m_adpEstilos.CargaEstilosdeVehiculo(m_drdEstilos, cboMarca.SelectedValue)
                    Utilitarios.CargarComboSourceByReader(cboEstilo, m_drdEstilos)
                Else
                    cboEstilo.DataSource = Nothing

                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            Finally
                'Agregado 01072010
                If m_drdEstilos IsNot Nothing Then
                    If Not m_drdEstilos.IsClosed Then
                        Call m_drdEstilos.Close()
                    End If
                End If
            End Try
        End Sub

        Private Sub chkEstado_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEstado.CheckedChanged

            If chkEstado.Checked Then

                'cboEstado.SelectedIndex = 1
                clsUtilidadCombos.CargarComboEstadoVisitas(cboEstado)

            Else

                'cboEstado.SelectedIndex = -1
                cboEstado.DataSource = Nothing

            End If

        End Sub

#End Region

#Region "Métodos"

        Private Sub limpiarOrdenes()
            Try


                'En caso que hayan ordenes cargadas en el dataset de orden y mostradas en el datagrid...se limpian
                If Not IsNothing(m_dstOrden) Then
                    If m_dstOrden.SCGTA_TB_Orden.Rows.Count <> 0 Then
                        m_dstOrden.Clear()
                        m_dstOrden.Dispose()
                        Me.dtgOrdenes.Refresh()
                    End If
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub limpiarCriteriosBusqueda()
            Try


                Me.txtCodCliente.Clear()
                Me.txtCliente.Clear()
                Me.txtCono.Clear()
                Me.txtPlaca.Clear()
                Me.txtNoVisita.Clear()

                Me.cboEstado.Text = ""
                Me.cboMarca.Text = ""

                txtCodCliente.Clear()
                txtCliente.Clear()
                txtAsesor.Clear()
                txtCedula.Clear()
                txtNombreAsesor.Clear()
                txtNoVisita.Clear()
                cboEstado.Text = ""
                txtNoVehiculo.Clear()
                txtPlaca.Clear()
                txtCono.Clear()
                cboEstilo.Text = ""
                cboMarca.Text = ""
                dtpAperturaini.Value = objUtilitarios.CargarFechaHoraServidor
                dtpAperturaini.Value = New Date(dtpAperturaini.Value.Year, dtpAperturaini.Value.Month, dtpAperturaini.Value.Day, 0, 0, 0)
                dtpCompromisoini.Value = objUtilitarios.CargarFechaHoraServidor
                dtpCompromisoini.Value = New Date(dtpCompromisoini.Value.Year, dtpCompromisoini.Value.Month, dtpCompromisoini.Value.Day, 0, 0, 0)
                dtpCierreini.Value = objUtilitarios.CargarFechaHoraServidor
                dtpCierreini.Value = New Date(dtpCierreini.Value.Year, dtpCierreini.Value.Month, dtpCierreini.Value.Day, 0, 0, 0)
                dtpAperturafin.Value = objUtilitarios.CargarFechaHoraServidor
                dtpAperturafin.Value = New Date(dtpAperturafin.Value.Year, dtpAperturafin.Value.Month, dtpAperturafin.Value.Day, 23, 59, 59)
                dtpCompromisofin.Value = objUtilitarios.CargarFechaHoraServidor
                dtpCompromisofin.Value = New Date(dtpCompromisofin.Value.Year, dtpCompromisofin.Value.Month, dtpCompromisofin.Value.Day, 23, 59, 59)
                dtpCierrefin.Value = objUtilitarios.CargarFechaHoraServidor
                dtpCierrefin.Value = New Date(dtpCierrefin.Value.Year, dtpCierrefin.Value.Month, dtpCierrefin.Value.Day, 23, 59, 59)

                chkApertura.Checked = False
                chkCierre.Checked = False
                chkCompromiso.Checked = False

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub limpiarVariablesBusqueda()

            Try
                m_intNoVisita = Nothing
                m_strNoVehiculo = ""
                m_strCardCode = ""
                m_strAsesor = ""
                m_intCodMarca = Nothing
                m_intCodEstilo = Nothing
                m_intCodModelo = Nothing
                m_strPlaca = ""
                m_dtApertura_ini = Nothing
                m_dtCompromiso_ini = Nothing
                m_dtCierre_ini = Nothing
                m_dtApertura_fin = Nothing
                m_dtCompromiso_fin = Nothing
                m_dtCierre_fin = Nothing
                m_intCodEstado = Nothing
                m_strCono = Nothing
                m_intTipoInsercion = Nothing

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Function ValidarCriteriosBusqueda() As Boolean

            'Variable que determina el si existen o no criterios de búsqueda en caso que no exista ninguno 
            'se devuelve la variable con un false en caso que exista uno o más se devuelve con un true.
            Dim blnValido As Boolean
            Try


                'Se inicia como false...en caso que exista al menos un criterio se convierte en true
                blnValido = False

                If Me.txtCodCliente.Text <> "" Then
                    blnValido = True
                ElseIf Me.txtCono.Text <> "" Then
                    blnValido = True
                ElseIf Me.txtNoVisita.Text <> "" Then
                    blnValido = True
                ElseIf txtNoVehiculo.Text <> "" Then
                    blnValido = True
                ElseIf txtAsesor.Text <> "" Then
                    blnValido = True
                ElseIf Me.txtPlaca.Text <> "" Then
                    blnValido = True
                ElseIf Me.cboEstado.Text <> "" Then
                    blnValido = True
                ElseIf Me.chkMarca.Checked Then
                    blnValido = True
                ElseIf chkEstilo.Checked Then
                    blnValido = True
                ElseIf chkApertura.Checked = True Then
                    blnValido = True
                ElseIf chkCompromiso.Checked Then
                    blnValido = True
                ElseIf chkCierre.Checked Then
                    blnValido = True
                End If

                Return blnValido

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Function

        Private Sub BusquedaVisitas()
            Try


                If ValidarCriteriosBusqueda() = True Then

                    m_adpVisita = New SCGDataAccess.VisitasDataAdapter

                    m_dstVisita.Dispose()

                    m_dstVisita = Nothing

                    m_dstVisita = New VisitaDataset

                    'Para que permita hacer varias búsquedas agregando paulatinamente los criterios,
                    'las variables de busqueda solo se deben limpiar cuando el campo de texto correspondiente
                    'está vacío



                    If Me.txtCodCliente.Text <> "" Then
                        m_strCardCode = Trim(Me.txtCodCliente.Text)
                    Else
                        m_strCardCode = Nothing
                    End If


                    If Me.txtPlaca.Text <> "" Then
                        m_strPlaca = Trim(Me.txtPlaca.Text)
                    Else
                        m_strPlaca = ""
                    End If


                    If Me.txtNoVisita.Text <> "" Then
                        m_intNoVisita = CInt(Me.txtNoVisita.Text)
                    Else
                        m_intNoVisita = Nothing
                    End If


                    If Me.cboMarca.Text <> "" Then
                        m_intCodMarca = Me.cboMarca.SelectedValue
                    Else
                        m_intCodMarca = Nothing
                    End If


                    If Me.cboEstado.Text <> "" Then
                        m_intCodEstado = Me.cboEstado.SelectedIndex + 1
                    Else
                        m_intCodEstado = Nothing
                    End If

                    If Me.txtCono.Text <> "" Then
                        m_strCono = Me.txtCono.Text
                    Else
                        m_strCono = ""
                    End If

                    If txtAsesor.Text <> "" Then
                        m_strAsesor = txtAsesor.Text
                    Else
                        m_strAsesor = Nothing
                    End If

                    If txtNoVehiculo.Text <> "" Then
                        m_strNoVehiculo = txtNoVehiculo.Text
                    Else
                        m_strNoVehiculo = Nothing
                    End If

                    If Me.cboEstilo.Text <> "" Then
                        m_intCodEstilo = Me.cboEstilo.SelectedValue
                    Else
                        m_intCodEstilo = Nothing
                    End If

                    If chkApertura.Checked Then
                        m_dtApertura_ini = dtpAperturaini.Value
                        m_dtApertura_fin = dtpAperturafin.Value
                    Else
                        m_dtApertura_ini = Nothing
                        m_dtApertura_fin = Nothing
                    End If

                    If chkCompromiso.Checked Then
                        m_dtCompromiso_ini = dtpCompromisoini.Value
                        m_dtCompromiso_fin = dtpCompromisofin.Value
                    Else
                        m_dtCompromiso_ini = Nothing
                        m_dtCompromiso_fin = Nothing
                    End If

                    If chkCierre.Checked Then
                        m_dtCierre_ini = dtpCierreini.Value
                        m_dtCierre_fin = dtpCierrefin.Value
                    Else
                        m_dtCierre_ini = Nothing
                        m_dtCierre_fin = Nothing
                    End If

                    Call m_adpVisita.Fill(m_dstVisita, m_strCardCode, m_strPlaca, m_intNoVisita, m_intCodMarca, _
                    m_intCodModelo, m_intCodEstilo, m_intCodEstado, m_dtApertura_ini, m_dtCierre_ini, m_dtCompromiso_ini, _
                    m_dtApertura_fin, m_dtCierre_fin, m_dtCompromiso_fin, m_strCono, m_strNoVehiculo, m_strAsesor)

                    LlenarEstadoVisitaResources(m_dstVisita)



                    'Hace que no se puede eliminar, editar ni agregar en el datagrid.
                    'With m_dstVisita.SCGTA_TB_Visita.DefaultView
                    '    .AllowDelete = False
                    '    .AllowNew = False
                    '    .AllowEdit = False
                    'End With

                    dtgVisitas.DataSource = m_dstVisita

                    MostrarDetalle()

                    If m_dstVisita.SCGTA_TB_Visita.Rows.Count > 0 Then

                        limpiarCriteriosBusqueda()

                    End If

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub


        Private Sub busquedaOrdenes()

            Try


                'Metodo para buscar las ordenes que cargan en el dtgOrdenes segün el Visita seleccionado en el dtgVisitas
                m_adpOrden = New SCGDataAccess.OrdenTrabajoDataAdapter

                If Not IsNothing(m_dstOrden) Then

                    m_dstOrden.Dispose()

                    m_dstOrden = Nothing

                End If

                'Se setea un nuevo dataset
                m_dstOrden = New OrdenTrabajoDataset

                'Se carga el dataset con las ordenes asociadas al Visita
                Call m_adpOrden.Fill(m_dstOrden, CInt(dtgVisitas.Rows.Item(dtgVisitas.CurrentRow.Index).Cells(0).Value))

                LlenarEstadoOrdenTrabajoResources(m_dstOrden)

                'Se establece que en el dataset no se pueda modificar, eliminar y agregar desde el datagrid
                'With m_dstOrden.SCGTA_TB_Orden.DefaultView

                '    .AllowDelete = False
                '    .AllowEdit = False
                '    .AllowNew = False

                'End With


                'Se carga el datagrid con el dataset de ordenes
                dtgOrdenes.DataSource = m_dstOrden

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Public Function Busca_Codigo_Texto(ByVal strTempItem As String, Optional ByVal blnGetCodigo As Boolean = True) As String

            '------------------------------------------------ Documentación SCG -----------------------------------------------------------
            '-- Busca el texto en el string enviado....si usas true busca el de la derecha y si usas falses busca el de la izquierda
            '------------------------------------------------------------------------------------------------------------------------------------

            Dim strCod_Item_Comp As String
            Dim strTemp As String
            Dim intCharCont As Integer
            Dim strTextoNoCodigo As String = ""

            strTemp = ""
            strCod_Item_Comp = ""
            Try


                If strTempItem <> "" Then

                    For intCharCont = strTempItem.Length - 1 To 0 Step -1
                        If Char.IsWhiteSpace(strTempItem.Chars(intCharCont)) Then
                            Exit For
                        End If
                        strTemp = strTemp & strTempItem.Chars(intCharCont)
                    Next

                    If strTempItem.Length > 0 Then
                        strTextoNoCodigo = strTempItem.Substring(0, strTempItem.Length - (strTempItem.Length - intCharCont)).Trim
                    End If
                    For intCharCont = strTemp.Length - 1 To 0 Step -1
                        strCod_Item_Comp = strCod_Item_Comp & strTemp.Chars(intCharCont)
                    Next

                    If blnGetCodigo Then
                        Return strCod_Item_Comp
                    Else
                        Return strTextoNoCodigo
                    End If

                End If

                Return ""

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
                Return ""
            End Try
        End Function

        Public Sub Busca_Item_Combo(ByRef Combo As ComboBox, ByVal Cod_Item As String)
            Try
                Dim intItemCont As Integer
                Dim strTempItem As String
                Dim strCod_Item_Comp As String
                Dim blnExiste As Boolean

                With Combo

                    If .Items.Count <> 0 Then
                        blnExiste = False
                        For intItemCont = 0 To .Items.Count - 1
                            strTempItem = .Items(intItemCont)
                            strCod_Item_Comp = Busca_Codigo_Texto(strTempItem)
                            If Cod_Item = strCod_Item_Comp Then
                                blnExiste = True
                                Exit For
                            End If
                        Next
                        If blnExiste Then
                            .Text = .Items(intItemCont)
                        End If
                    End If

                End With
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub MostrarDetalle()
            Dim intNoVisita As Integer                'Carga el número de Visita

            Try
                'Se pone la inserción en modo de modificación.
                intTipoInsercion = 2
                'limpia las ordenes del Visita anteriormente seleccionado que estaban en el grid
                limpiarOrdenes()
                'limpia el campo detalle que se refiere al Visita anteriormente seleccionado

                'Se valida que almenos exista un valor en el datagrid (o sino se cae al seleccionar)
                If dtgVisitas.CurrentRow IsNot Nothing AndAlso m_dstVisita.SCGTA_TB_Visita.Rows.Count <> 0 Then


                    'Se asignan los codigos correspondientes a las variables según la selección en el datagrid
                    intNoVisita = dtgVisitas.Rows.Item(dtgVisitas.CurrentRow.Index).Cells(0).Value

                    drwVisita = m_dstVisita.SCGTA_TB_Visita.FindByNoVisita(intNoVisita)


                    'Se habilita tanto la modificación como la eliminación del row.
                    tlbVisitas.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = True
                    tlbVisitas.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = True
                    tlbVisitas.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True

                    'Se pone el focus en el txt esto para facilidad de modificación y para que no pueda eliminar 
                    'el row presionando supreme y no se caiga el código.
                    'Me.txtNoVisita.Focus()
                    Me.txtNoVisita.SelectAll()

                    busquedaOrdenes()

                    tlbVisitas.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = True

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

#End Region

        Private Sub chkModelo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkModelo.CheckedChanged
            Try

                If chkEstilo.Checked Then
                    If chkModelo.Checked Then

                        m_adpModelos = New ModelosDataAdapter
                        m_drdModelos = Nothing

                        m_adpModelos.CargaModelosdeVehiculo(m_drdModelos, cboEstilo.SelectedValue)
                        Utilitarios.CargarComboSourceByReader(cboModelo, m_drdModelos)
                    Else
                        cboModelo.DataSource = Nothing

                    End If
                Else

                    cboModelo.DataSource = Nothing
                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally

                m_drdModelos = Nothing
                m_adpModelos = Nothing

                'Agregado 02072010
                If m_drdModelos IsNot Nothing Then
                    If Not m_drdModelos.IsClosed Then
                        Call m_drdModelos.Close()
                    End If
                End If

            End Try
        End Sub

        Private Sub cboEstilo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboEstilo.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
        End Sub

        Private Sub cboEstilo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEstilo.SelectedIndexChanged
            Try
                If chkModelo.Checked Then

                    m_adpModelos = New ModelosDataAdapter
                    m_drdModelos = Nothing

                    m_adpModelos.CargaModelosdeVehiculo(m_drdModelos, cboEstilo.SelectedValue)
                    Utilitarios.CargarComboSourceByReader(cboModelo, m_drdModelos)
                Else

                    cboModelo.DataSource = Nothing

                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            Finally
                'Agregado 02072010
                If m_drdModelos IsNot Nothing Then
                    If Not m_drdModelos.IsClosed Then
                        Call m_drdModelos.Close()
                    End If
                End If
            End Try
        End Sub

        Private Sub txtCodCliente_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCodCliente.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
        End Sub

        Private Sub txtCliente_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCliente.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
        End Sub

        Private Sub txtCedula_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCedula.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
        End Sub

        Private Sub txtAsesor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAsesor.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
        End Sub

        Private Sub txtNombreAsesor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNombreAsesor.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
        End Sub

        Private Sub cboModelo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboModelo.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
        End Sub

        Private Sub dtpAperturafin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpAperturafin.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
        End Sub

        Private Sub dtpCompromisofin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpCompromisofin.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
        End Sub

        Private Sub dtpCierrefin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpCierrefin.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then BusquedaVisitas()
        End Sub

        Private Sub dtgOrdenes_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgOrdenes.CellDoubleClick

            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean
            Try

                'Validacion de que el datagrid no este vacio
                If dtgOrdenes.CurrentRow.Index <> -1 Then


                    For Each Forma_Nueva In Me.MdiParent.MdiChildren
                        If Forma_Nueva.Name = "frmOrden" Then
                            blnExisteForm = True
                        End If
                    Next

                    If Not blnExisteForm Then
                        objfrmOperYOrden = New frmOrden(m_dstOrden, CStr(dtgOrdenes.Rows.Item(dtgOrdenes.CurrentRow.Index).Cells(0).Value))

                        objfrmOperYOrden.MdiParent = Me.MdiParent
                        objfrmOperYOrden.Show()
                    End If

                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub grpCriteriosBusqueda_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpCriteriosBusqueda.Enter

        End Sub
    End Class


End Namespace
