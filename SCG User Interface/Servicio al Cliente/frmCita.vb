Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports Proyecto_SCGToolBar

Namespace SCG_User_Interface
    Public Class frmCita
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
        Friend WithEvents tlbClientes As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents txtRazon As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents dtgCitas As System.Windows.Forms.DataGridView
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Public WithEvents Label10 As System.Windows.Forms.Label
        Public WithEvents Label11 As System.Windows.Forms.Label
        Friend WithEvents dtpHasta As System.Windows.Forms.DateTimePicker
        Public WithEvents Label12 As System.Windows.Forms.Label
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents dtpDesde As System.Windows.Forms.DateTimePicker
        Public WithEvents Label13 As System.Windows.Forms.Label
        Friend WithEvents chkDesde As System.Windows.Forms.CheckBox
        Friend WithEvents cboAgenda As SCGComboBox.SCGComboBox
        Public WithEvents Label14 As System.Windows.Forms.Label
        Friend WithEvents txtNoVehiculo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label15 As System.Windows.Forms.Label
        Friend WithEvents Label16 As System.Windows.Forms.Label
        Friend WithEvents txtPlaca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label17 As System.Windows.Forms.Label
        Friend WithEvents Label18 As System.Windows.Forms.Label
        Friend WithEvents cboEstilos As SCGComboBox.SCGComboBox
        Friend WithEvents cboModelo As SCGComboBox.SCGComboBox
        Friend WithEvents cboMarca As SCGComboBox.SCGComboBox
        Friend WithEvents txtnombre As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picCliente As System.Windows.Forms.PictureBox
        Friend WithEvents txtNoCita As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label20 As System.Windows.Forms.Label
        Friend WithEvents Label21 As System.Windows.Forms.Label
        Public WithEvents Label22 As System.Windows.Forms.Label
        Friend WithEvents txtCliente As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label24 As System.Windows.Forms.Label
        Friend WithEvents Label25 As System.Windows.Forms.Label
        Friend WithEvents chkMarca As System.Windows.Forms.CheckBox
        Friend WithEvents chkModelo As System.Windows.Forms.CheckBox
        Friend WithEvents chkEstilo As System.Windows.Forms.CheckBox
        Friend WithEvents chkAgenda As System.Windows.Forms.CheckBox
        Friend WithEvents chkHasta As System.Windows.Forms.CheckBox
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents IDCita As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoCitaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechayHoraDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CardNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents RazonDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EstadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents AgendaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents PlacaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescMarcaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EmpNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDRazonDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CardCodeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoCotizacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDAgendaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoSerieDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoConsecutivoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodMarcaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ObservacionesDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodEstiloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescEstiloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodModeloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescModeloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents AnoVehiculoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents VINDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EmpIdDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CreadaPorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechayHoraEnHorarioDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CombustibleDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CilindradaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodTecnico As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescripcionTecnico As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim m_dtsCitasVisual As DMSOneFramework.CitasDataset
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCita))
            Me.grpCitas = New System.Windows.Forms.GroupBox()
            Me.dtgCitas = New System.Windows.Forms.DataGridView()
            Me.IDCita = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.NoCitaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.FechayHoraDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CardNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.RazonDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.EstadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.AgendaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.PlacaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DescMarcaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.EmpNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.IDRazonDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CardCodeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.NoCotizacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.IDAgendaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.NoSerieDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.NoConsecutivoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CodMarcaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ObservacionesDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CodEstiloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DescEstiloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CodModeloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DescModeloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.IDVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.NoVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.AnoVehiculoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.VINDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.EmpIdDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CreadaPorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.FechayHoraEnHorarioDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CombustibleDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CilindradaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CodTecnico = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DescripcionTecnico = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.txtRazon = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Panel1 = New System.Windows.Forms.Panel()
            Me.Label10 = New System.Windows.Forms.Label()
            Me.Label11 = New System.Windows.Forms.Label()
            Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
            Me.Label12 = New System.Windows.Forms.Label()
            Me.Panel2 = New System.Windows.Forms.Panel()
            Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
            Me.Label13 = New System.Windows.Forms.Label()
            Me.chkDesde = New System.Windows.Forms.CheckBox()
            Me.cboAgenda = New SCGComboBox.SCGComboBox()
            Me.Label14 = New System.Windows.Forms.Label()
            Me.txtNoVehiculo = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label15 = New System.Windows.Forms.Label()
            Me.Label16 = New System.Windows.Forms.Label()
            Me.txtPlaca = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label17 = New System.Windows.Forms.Label()
            Me.Label18 = New System.Windows.Forms.Label()
            Me.cboEstilos = New SCGComboBox.SCGComboBox()
            Me.cboModelo = New SCGComboBox.SCGComboBox()
            Me.cboMarca = New SCGComboBox.SCGComboBox()
            Me.txtnombre = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picCliente = New System.Windows.Forms.PictureBox()
            Me.txtNoCita = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label20 = New System.Windows.Forms.Label()
            Me.Label21 = New System.Windows.Forms.Label()
            Me.Label22 = New System.Windows.Forms.Label()
            Me.txtCliente = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label24 = New System.Windows.Forms.Label()
            Me.Label25 = New System.Windows.Forms.Label()
            Me.chkMarca = New System.Windows.Forms.CheckBox()
            Me.chkModelo = New System.Windows.Forms.CheckBox()
            Me.chkEstilo = New System.Windows.Forms.CheckBox()
            Me.chkAgenda = New System.Windows.Forms.CheckBox()
            Me.chkHasta = New System.Windows.Forms.CheckBox()
            Me.tlbClientes = New Proyecto_SCGToolBar.SCGToolBar()
            m_dtsCitasVisual = New DMSOneFramework.CitasDataset()
            CType(m_dtsCitasVisual, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpCitas.SuspendLayout()
            CType(Me.dtgCitas, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.GroupBox1.SuspendLayout()
            CType(Me.picCliente, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'm_dtsCitasVisual
            '
            m_dtsCitasVisual.DataSetName = "CitasDataset"
            m_dtsCitasVisual.Locale = New System.Globalization.CultureInfo("en-US")
            m_dtsCitasVisual.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'grpCitas
            '
            Me.grpCitas.BackColor = System.Drawing.SystemColors.Control
            Me.grpCitas.Controls.Add(Me.dtgCitas)
            Me.grpCitas.Controls.Add(Me.txtRazon)
            resources.ApplyResources(Me.grpCitas, "grpCitas")
            Me.grpCitas.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.grpCitas.Name = "grpCitas"
            Me.grpCitas.TabStop = False
            '
            'dtgCitas
            '
            Me.dtgCitas.AllowUserToAddRows = False
            Me.dtgCitas.AllowUserToDeleteRows = False
            Me.dtgCitas.AllowUserToResizeRows = False
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            Me.dtgCitas.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
            Me.dtgCitas.AutoGenerateColumns = False
            Me.dtgCitas.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtgCitas.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDCita, Me.NoCitaDataGridViewTextBoxColumn, Me.FechayHoraDataGridViewTextBoxColumn, Me.CardNameDataGridViewTextBoxColumn, Me.RazonDataGridViewTextBoxColumn, Me.EstadoDataGridViewTextBoxColumn, Me.AgendaDataGridViewTextBoxColumn, Me.PlacaDataGridViewTextBoxColumn, Me.DescMarcaDataGridViewTextBoxColumn, Me.EmpNameDataGridViewTextBoxColumn, Me.IDRazonDataGridViewTextBoxColumn, Me.CardCodeDataGridViewTextBoxColumn, Me.NoCotizacionDataGridViewTextBoxColumn, Me.IDAgendaDataGridViewTextBoxColumn, Me.NoSerieDataGridViewTextBoxColumn, Me.NoConsecutivoDataGridViewTextBoxColumn, Me.CodMarcaDataGridViewTextBoxColumn, Me.ObservacionesDataGridViewTextBoxColumn, Me.CodEstiloDataGridViewTextBoxColumn, Me.DescEstiloDataGridViewTextBoxColumn, Me.CodModeloDataGridViewTextBoxColumn, Me.DescModeloDataGridViewTextBoxColumn, Me.IDVehiculoDataGridViewTextBoxColumn, Me.NoVehiculoDataGridViewTextBoxColumn, Me.AnoVehiculoDataGridViewTextBoxColumn, Me.VINDataGridViewTextBoxColumn, Me.EmpIdDataGridViewTextBoxColumn, Me.CreadaPorDataGridViewTextBoxColumn, Me.FechayHoraEnHorarioDataGridViewTextBoxColumn, Me.CombustibleDataGridViewTextBoxColumn, Me.CilindradaDataGridViewTextBoxColumn, Me.CodTecnico, Me.DescripcionTecnico})
            Me.dtgCitas.DataMember = "SCGTA_TB_Citas"
            Me.dtgCitas.DataSource = m_dtsCitasVisual
            Me.dtgCitas.GridColor = System.Drawing.Color.Silver
            resources.ApplyResources(Me.dtgCitas, "dtgCitas")
            Me.dtgCitas.MultiSelect = False
            Me.dtgCitas.Name = "dtgCitas"
            Me.dtgCitas.ReadOnly = True
            DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(207, Byte), Integer), CType(CType(49, Byte), Integer))
            DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgCitas.RowsDefaultCellStyle = DataGridViewCellStyle2
            Me.dtgCitas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            '
            'IDCita
            '
            Me.IDCita.DataPropertyName = "IDCita"
            resources.ApplyResources(Me.IDCita, "IDCita")
            Me.IDCita.Name = "IDCita"
            Me.IDCita.ReadOnly = True
            '
            'NoCitaDataGridViewTextBoxColumn
            '
            Me.NoCitaDataGridViewTextBoxColumn.DataPropertyName = "NoCita"
            resources.ApplyResources(Me.NoCitaDataGridViewTextBoxColumn, "NoCitaDataGridViewTextBoxColumn")
            Me.NoCitaDataGridViewTextBoxColumn.Name = "NoCitaDataGridViewTextBoxColumn"
            Me.NoCitaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FechayHoraDataGridViewTextBoxColumn
            '
            Me.FechayHoraDataGridViewTextBoxColumn.DataPropertyName = "FechayHora"
            resources.ApplyResources(Me.FechayHoraDataGridViewTextBoxColumn, "FechayHoraDataGridViewTextBoxColumn")
            Me.FechayHoraDataGridViewTextBoxColumn.Name = "FechayHoraDataGridViewTextBoxColumn"
            Me.FechayHoraDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CardNameDataGridViewTextBoxColumn
            '
            Me.CardNameDataGridViewTextBoxColumn.DataPropertyName = "CardName"
            resources.ApplyResources(Me.CardNameDataGridViewTextBoxColumn, "CardNameDataGridViewTextBoxColumn")
            Me.CardNameDataGridViewTextBoxColumn.Name = "CardNameDataGridViewTextBoxColumn"
            Me.CardNameDataGridViewTextBoxColumn.ReadOnly = True
            '
            'RazonDataGridViewTextBoxColumn
            '
            Me.RazonDataGridViewTextBoxColumn.DataPropertyName = "Razon"
            resources.ApplyResources(Me.RazonDataGridViewTextBoxColumn, "RazonDataGridViewTextBoxColumn")
            Me.RazonDataGridViewTextBoxColumn.Name = "RazonDataGridViewTextBoxColumn"
            Me.RazonDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EstadoDataGridViewTextBoxColumn
            '
            Me.EstadoDataGridViewTextBoxColumn.DataPropertyName = "Estado"
            resources.ApplyResources(Me.EstadoDataGridViewTextBoxColumn, "EstadoDataGridViewTextBoxColumn")
            Me.EstadoDataGridViewTextBoxColumn.Name = "EstadoDataGridViewTextBoxColumn"
            Me.EstadoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'AgendaDataGridViewTextBoxColumn
            '
            Me.AgendaDataGridViewTextBoxColumn.DataPropertyName = "Agenda"
            resources.ApplyResources(Me.AgendaDataGridViewTextBoxColumn, "AgendaDataGridViewTextBoxColumn")
            Me.AgendaDataGridViewTextBoxColumn.Name = "AgendaDataGridViewTextBoxColumn"
            Me.AgendaDataGridViewTextBoxColumn.ReadOnly = True
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
            'EmpNameDataGridViewTextBoxColumn
            '
            Me.EmpNameDataGridViewTextBoxColumn.DataPropertyName = "empName"
            resources.ApplyResources(Me.EmpNameDataGridViewTextBoxColumn, "EmpNameDataGridViewTextBoxColumn")
            Me.EmpNameDataGridViewTextBoxColumn.Name = "EmpNameDataGridViewTextBoxColumn"
            Me.EmpNameDataGridViewTextBoxColumn.ReadOnly = True
            '
            'IDRazonDataGridViewTextBoxColumn
            '
            Me.IDRazonDataGridViewTextBoxColumn.DataPropertyName = "IDRazon"
            resources.ApplyResources(Me.IDRazonDataGridViewTextBoxColumn, "IDRazonDataGridViewTextBoxColumn")
            Me.IDRazonDataGridViewTextBoxColumn.Name = "IDRazonDataGridViewTextBoxColumn"
            Me.IDRazonDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CardCodeDataGridViewTextBoxColumn
            '
            Me.CardCodeDataGridViewTextBoxColumn.DataPropertyName = "CardCode"
            resources.ApplyResources(Me.CardCodeDataGridViewTextBoxColumn, "CardCodeDataGridViewTextBoxColumn")
            Me.CardCodeDataGridViewTextBoxColumn.Name = "CardCodeDataGridViewTextBoxColumn"
            Me.CardCodeDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoCotizacionDataGridViewTextBoxColumn
            '
            Me.NoCotizacionDataGridViewTextBoxColumn.DataPropertyName = "NoCotizacion"
            resources.ApplyResources(Me.NoCotizacionDataGridViewTextBoxColumn, "NoCotizacionDataGridViewTextBoxColumn")
            Me.NoCotizacionDataGridViewTextBoxColumn.Name = "NoCotizacionDataGridViewTextBoxColumn"
            Me.NoCotizacionDataGridViewTextBoxColumn.ReadOnly = True
            '
            'IDAgendaDataGridViewTextBoxColumn
            '
            Me.IDAgendaDataGridViewTextBoxColumn.DataPropertyName = "IDAgenda"
            resources.ApplyResources(Me.IDAgendaDataGridViewTextBoxColumn, "IDAgendaDataGridViewTextBoxColumn")
            Me.IDAgendaDataGridViewTextBoxColumn.Name = "IDAgendaDataGridViewTextBoxColumn"
            Me.IDAgendaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoSerieDataGridViewTextBoxColumn
            '
            Me.NoSerieDataGridViewTextBoxColumn.DataPropertyName = "NoSerie"
            resources.ApplyResources(Me.NoSerieDataGridViewTextBoxColumn, "NoSerieDataGridViewTextBoxColumn")
            Me.NoSerieDataGridViewTextBoxColumn.Name = "NoSerieDataGridViewTextBoxColumn"
            Me.NoSerieDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoConsecutivoDataGridViewTextBoxColumn
            '
            Me.NoConsecutivoDataGridViewTextBoxColumn.DataPropertyName = "NoConsecutivo"
            resources.ApplyResources(Me.NoConsecutivoDataGridViewTextBoxColumn, "NoConsecutivoDataGridViewTextBoxColumn")
            Me.NoConsecutivoDataGridViewTextBoxColumn.Name = "NoConsecutivoDataGridViewTextBoxColumn"
            Me.NoConsecutivoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodMarcaDataGridViewTextBoxColumn
            '
            Me.CodMarcaDataGridViewTextBoxColumn.DataPropertyName = "CodMarca"
            resources.ApplyResources(Me.CodMarcaDataGridViewTextBoxColumn, "CodMarcaDataGridViewTextBoxColumn")
            Me.CodMarcaDataGridViewTextBoxColumn.Name = "CodMarcaDataGridViewTextBoxColumn"
            Me.CodMarcaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ObservacionesDataGridViewTextBoxColumn
            '
            Me.ObservacionesDataGridViewTextBoxColumn.DataPropertyName = "Observaciones"
            resources.ApplyResources(Me.ObservacionesDataGridViewTextBoxColumn, "ObservacionesDataGridViewTextBoxColumn")
            Me.ObservacionesDataGridViewTextBoxColumn.Name = "ObservacionesDataGridViewTextBoxColumn"
            Me.ObservacionesDataGridViewTextBoxColumn.ReadOnly = True
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
            'IDVehiculoDataGridViewTextBoxColumn
            '
            Me.IDVehiculoDataGridViewTextBoxColumn.DataPropertyName = "IDVehiculo"
            resources.ApplyResources(Me.IDVehiculoDataGridViewTextBoxColumn, "IDVehiculoDataGridViewTextBoxColumn")
            Me.IDVehiculoDataGridViewTextBoxColumn.Name = "IDVehiculoDataGridViewTextBoxColumn"
            Me.IDVehiculoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NoVehiculoDataGridViewTextBoxColumn
            '
            Me.NoVehiculoDataGridViewTextBoxColumn.DataPropertyName = "NoVehiculo"
            resources.ApplyResources(Me.NoVehiculoDataGridViewTextBoxColumn, "NoVehiculoDataGridViewTextBoxColumn")
            Me.NoVehiculoDataGridViewTextBoxColumn.Name = "NoVehiculoDataGridViewTextBoxColumn"
            Me.NoVehiculoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'AnoVehiculoDataGridViewTextBoxColumn
            '
            Me.AnoVehiculoDataGridViewTextBoxColumn.DataPropertyName = "AnoVehiculo"
            resources.ApplyResources(Me.AnoVehiculoDataGridViewTextBoxColumn, "AnoVehiculoDataGridViewTextBoxColumn")
            Me.AnoVehiculoDataGridViewTextBoxColumn.Name = "AnoVehiculoDataGridViewTextBoxColumn"
            Me.AnoVehiculoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'VINDataGridViewTextBoxColumn
            '
            Me.VINDataGridViewTextBoxColumn.DataPropertyName = "VIN"
            resources.ApplyResources(Me.VINDataGridViewTextBoxColumn, "VINDataGridViewTextBoxColumn")
            Me.VINDataGridViewTextBoxColumn.Name = "VINDataGridViewTextBoxColumn"
            Me.VINDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EmpIdDataGridViewTextBoxColumn
            '
            Me.EmpIdDataGridViewTextBoxColumn.DataPropertyName = "empId"
            resources.ApplyResources(Me.EmpIdDataGridViewTextBoxColumn, "EmpIdDataGridViewTextBoxColumn")
            Me.EmpIdDataGridViewTextBoxColumn.Name = "EmpIdDataGridViewTextBoxColumn"
            Me.EmpIdDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CreadaPorDataGridViewTextBoxColumn
            '
            Me.CreadaPorDataGridViewTextBoxColumn.DataPropertyName = "CreadaPor"
            resources.ApplyResources(Me.CreadaPorDataGridViewTextBoxColumn, "CreadaPorDataGridViewTextBoxColumn")
            Me.CreadaPorDataGridViewTextBoxColumn.Name = "CreadaPorDataGridViewTextBoxColumn"
            Me.CreadaPorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FechayHoraEnHorarioDataGridViewTextBoxColumn
            '
            Me.FechayHoraEnHorarioDataGridViewTextBoxColumn.DataPropertyName = "FechayHoraEnHorario"
            resources.ApplyResources(Me.FechayHoraEnHorarioDataGridViewTextBoxColumn, "FechayHoraEnHorarioDataGridViewTextBoxColumn")
            Me.FechayHoraEnHorarioDataGridViewTextBoxColumn.Name = "FechayHoraEnHorarioDataGridViewTextBoxColumn"
            Me.FechayHoraEnHorarioDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CombustibleDataGridViewTextBoxColumn
            '
            Me.CombustibleDataGridViewTextBoxColumn.DataPropertyName = "Combustible"
            resources.ApplyResources(Me.CombustibleDataGridViewTextBoxColumn, "CombustibleDataGridViewTextBoxColumn")
            Me.CombustibleDataGridViewTextBoxColumn.Name = "CombustibleDataGridViewTextBoxColumn"
            Me.CombustibleDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CilindradaDataGridViewTextBoxColumn
            '
            Me.CilindradaDataGridViewTextBoxColumn.DataPropertyName = "Cilindrada"
            resources.ApplyResources(Me.CilindradaDataGridViewTextBoxColumn, "CilindradaDataGridViewTextBoxColumn")
            Me.CilindradaDataGridViewTextBoxColumn.Name = "CilindradaDataGridViewTextBoxColumn"
            Me.CilindradaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodTecnico
            '
            Me.CodTecnico.DataPropertyName = "CodTecnico"
            resources.ApplyResources(Me.CodTecnico, "CodTecnico")
            Me.CodTecnico.Name = "CodTecnico"
            Me.CodTecnico.ReadOnly = True
            '
            'DescripcionTecnico
            '
            Me.DescripcionTecnico.DataPropertyName = "DescripcionTecnico"
            resources.ApplyResources(Me.DescripcionTecnico, "DescripcionTecnico")
            Me.DescripcionTecnico.Name = "DescripcionTecnico"
            Me.DescripcionTecnico.ReadOnly = True
            '
            'txtRazon
            '
            Me.txtRazon.AceptaNegativos = False
            Me.txtRazon.BackColor = System.Drawing.Color.White
            Me.txtRazon.EstiloSBO = True
            Me.txtRazon.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            resources.ApplyResources(Me.txtRazon, "txtRazon")
            Me.txtRazon.MaxDecimales = 0
            Me.txtRazon.MaxEnteros = 0
            Me.txtRazon.Millares = False
            Me.txtRazon.Name = "txtRazon"
            Me.txtRazon.ReadOnly = True
            Me.txtRazon.Size_AdjustableHeight = 39
            Me.txtRazon.TeclasDeshacer = True
            Me.txtRazon.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.Label1)
            Me.GroupBox1.Controls.Add(Me.Panel1)
            Me.GroupBox1.Controls.Add(Me.Label10)
            Me.GroupBox1.Controls.Add(Me.Label11)
            Me.GroupBox1.Controls.Add(Me.dtpHasta)
            Me.GroupBox1.Controls.Add(Me.Label12)
            Me.GroupBox1.Controls.Add(Me.Panel2)
            Me.GroupBox1.Controls.Add(Me.dtpDesde)
            Me.GroupBox1.Controls.Add(Me.Label13)
            Me.GroupBox1.Controls.Add(Me.chkDesde)
            Me.GroupBox1.Controls.Add(Me.cboAgenda)
            Me.GroupBox1.Controls.Add(Me.Label14)
            Me.GroupBox1.Controls.Add(Me.txtNoVehiculo)
            Me.GroupBox1.Controls.Add(Me.Label15)
            Me.GroupBox1.Controls.Add(Me.Label16)
            Me.GroupBox1.Controls.Add(Me.txtPlaca)
            Me.GroupBox1.Controls.Add(Me.Label17)
            Me.GroupBox1.Controls.Add(Me.Label18)
            Me.GroupBox1.Controls.Add(Me.cboEstilos)
            Me.GroupBox1.Controls.Add(Me.cboModelo)
            Me.GroupBox1.Controls.Add(Me.cboMarca)
            Me.GroupBox1.Controls.Add(Me.txtnombre)
            Me.GroupBox1.Controls.Add(Me.picCliente)
            Me.GroupBox1.Controls.Add(Me.txtNoCita)
            Me.GroupBox1.Controls.Add(Me.Label20)
            Me.GroupBox1.Controls.Add(Me.Label21)
            Me.GroupBox1.Controls.Add(Me.Label22)
            Me.GroupBox1.Controls.Add(Me.txtCliente)
            Me.GroupBox1.Controls.Add(Me.Label24)
            Me.GroupBox1.Controls.Add(Me.Label25)
            Me.GroupBox1.Controls.Add(Me.chkMarca)
            Me.GroupBox1.Controls.Add(Me.chkModelo)
            Me.GroupBox1.Controls.Add(Me.chkEstilo)
            Me.GroupBox1.Controls.Add(Me.chkAgenda)
            Me.GroupBox1.Controls.Add(Me.chkHasta)
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            Me.GroupBox1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.TabStop = False
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'Panel1
            '
            resources.ApplyResources(Me.Panel1, "Panel1")
            Me.Panel1.Name = "Panel1"
            '
            'Label10
            '
            Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label10, "Label10")
            Me.Label10.Name = "Label10"
            '
            'Label11
            '
            Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label11, "Label11")
            Me.Label11.Name = "Label11"
            '
            'dtpHasta
            '
            Me.dtpHasta.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHasta.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpHasta.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpHasta.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHasta.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpHasta, "dtpHasta")
            Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpHasta.Name = "dtpHasta"
            Me.dtpHasta.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'Label12
            '
            Me.Label12.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label12, "Label12")
            Me.Label12.Name = "Label12"
            '
            'Panel2
            '
            resources.ApplyResources(Me.Panel2, "Panel2")
            Me.Panel2.Name = "Panel2"
            '
            'dtpDesde
            '
            Me.dtpDesde.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpDesde.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpDesde.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpDesde.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpDesde.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpDesde, "dtpDesde")
            Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpDesde.Name = "dtpDesde"
            Me.dtpDesde.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'Label13
            '
            Me.Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label13, "Label13")
            Me.Label13.Name = "Label13"
            '
            'chkDesde
            '
            resources.ApplyResources(Me.chkDesde, "chkDesde")
            Me.chkDesde.Name = "chkDesde"
            Me.chkDesde.UseVisualStyleBackColor = True
            '
            'cboAgenda
            '
            Me.cboAgenda.BackColor = System.Drawing.Color.White
            Me.cboAgenda.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboAgenda.EstiloSBO = True
            resources.ApplyResources(Me.cboAgenda, "cboAgenda")
            Me.cboAgenda.Items.AddRange(New Object() {Global.SCG_User_Interface.My.Resources.ResourceUI.String1})
            Me.cboAgenda.Name = "cboAgenda"
            '
            'Label14
            '
            Me.Label14.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label14, "Label14")
            Me.Label14.Name = "Label14"
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
            'Label15
            '
            Me.Label15.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label15, "Label15")
            Me.Label15.Name = "Label15"
            '
            'Label16
            '
            resources.ApplyResources(Me.Label16, "Label16")
            Me.Label16.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label16.Name = "Label16"
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
            Me.txtPlaca.Size_AdjustableHeight = 20
            Me.txtPlaca.TeclasDeshacer = True
            Me.txtPlaca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label17
            '
            Me.Label17.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label17, "Label17")
            Me.Label17.Name = "Label17"
            '
            'Label18
            '
            resources.ApplyResources(Me.Label18, "Label18")
            Me.Label18.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label18.Name = "Label18"
            '
            'cboEstilos
            '
            Me.cboEstilos.BackColor = System.Drawing.Color.White
            Me.cboEstilos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstilos.EstiloSBO = True
            resources.ApplyResources(Me.cboEstilos, "cboEstilos")
            Me.cboEstilos.Items.AddRange(New Object() {Global.SCG_User_Interface.My.Resources.ResourceUI.String1})
            Me.cboEstilos.Name = "cboEstilos"
            '
            'cboModelo
            '
            Me.cboModelo.BackColor = System.Drawing.Color.White
            Me.cboModelo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboModelo.EstiloSBO = True
            resources.ApplyResources(Me.cboModelo, "cboModelo")
            Me.cboModelo.Items.AddRange(New Object() {Global.SCG_User_Interface.My.Resources.ResourceUI.String1})
            Me.cboModelo.Name = "cboModelo"
            '
            'cboMarca
            '
            Me.cboMarca.BackColor = System.Drawing.Color.White
            Me.cboMarca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboMarca.EstiloSBO = True
            resources.ApplyResources(Me.cboMarca, "cboMarca")
            Me.cboMarca.Items.AddRange(New Object() {Global.SCG_User_Interface.My.Resources.ResourceUI.String1})
            Me.cboMarca.Name = "cboMarca"
            '
            'txtnombre
            '
            Me.txtnombre.AceptaNegativos = False
            Me.txtnombre.BackColor = System.Drawing.Color.White
            Me.txtnombre.EstiloSBO = True
            resources.ApplyResources(Me.txtnombre, "txtnombre")
            Me.txtnombre.MaxDecimales = 0
            Me.txtnombre.MaxEnteros = 0
            Me.txtnombre.Millares = False
            Me.txtnombre.Name = "txtnombre"
            Me.txtnombre.ReadOnly = True
            Me.txtnombre.Size_AdjustableHeight = 20
            Me.txtnombre.TeclasDeshacer = True
            Me.txtnombre.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picCliente
            '
            Me.picCliente.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picCliente, "picCliente")
            Me.picCliente.Name = "picCliente"
            Me.picCliente.TabStop = False
            '
            'txtNoCita
            '
            Me.txtNoCita.AceptaNegativos = False
            Me.txtNoCita.BackColor = System.Drawing.Color.White
            Me.txtNoCita.EstiloSBO = True
            resources.ApplyResources(Me.txtNoCita, "txtNoCita")
            Me.txtNoCita.MaxDecimales = 0
            Me.txtNoCita.MaxEnteros = 0
            Me.txtNoCita.Millares = False
            Me.txtNoCita.Name = "txtNoCita"
            Me.txtNoCita.Size_AdjustableHeight = 20
            Me.txtNoCita.TeclasDeshacer = True
            Me.txtNoCita.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label20
            '
            Me.Label20.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label20, "Label20")
            Me.Label20.Name = "Label20"
            '
            'Label21
            '
            resources.ApplyResources(Me.Label21, "Label21")
            Me.Label21.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label21.Name = "Label21"
            '
            'Label22
            '
            Me.Label22.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label22, "Label22")
            Me.Label22.Name = "Label22"
            '
            'txtCliente
            '
            Me.txtCliente.AceptaNegativos = False
            Me.txtCliente.BackColor = System.Drawing.Color.White
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
            'Label24
            '
            Me.Label24.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label24, "Label24")
            Me.Label24.Name = "Label24"
            '
            'Label25
            '
            resources.ApplyResources(Me.Label25, "Label25")
            Me.Label25.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label25.Name = "Label25"
            '
            'chkMarca
            '
            resources.ApplyResources(Me.chkMarca, "chkMarca")
            Me.chkMarca.Name = "chkMarca"
            Me.chkMarca.UseVisualStyleBackColor = True
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
            'chkAgenda
            '
            resources.ApplyResources(Me.chkAgenda, "chkAgenda")
            Me.chkAgenda.Name = "chkAgenda"
            Me.chkAgenda.UseVisualStyleBackColor = True
            '
            'chkHasta
            '
            resources.ApplyResources(Me.chkHasta, "chkHasta")
            Me.chkHasta.Name = "chkHasta"
            Me.chkHasta.UseVisualStyleBackColor = True
            '
            'tlbClientes
            '
            resources.ApplyResources(Me.tlbClientes, "tlbClientes")
            Me.tlbClientes.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.tlbClientes.Name = "tlbClientes"
            '
            'frmCita
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.grpCitas)
            Me.Controls.Add(Me.GroupBox1)
            Me.Controls.Add(Me.tlbClientes)
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.Name = "frmCita"
            Me.Tag = "Servicio al Cliente,1"
            CType(m_dtsCitasVisual, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpCitas.ResumeLayout(False)
            Me.grpCitas.PerformLayout()
            CType(Me.dtgCitas, System.ComponentModel.ISupportInitialize).EndInit()
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
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

#Region "Objetos"

        'Declaracion de objeto dataAdapter y Dataset.
        Private m_adpCita As SCGDataAccess.CitasDataAdapter
        Public m_dstCita As CitasDataset

        'Declaracion de un row del dataset, el cual sirve para insertar como para modificar y eliminar.
        Private drwCita As CitasDataset.SCGTA_TB_CitasRow

        Private objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        Private WithEvents objfrmOpenCita As frmDetalleCita
        Private WithEvents objBuscador As New Buscador.SubBuscador

        'Objetos para los catálogos
        Private m_adpMarcas As MarcaDataAdapter
        Private m_drdMarcas As SqlClient.SqlDataReader

        Private m_adpEstilos As EstiloDataAdapter
        Private m_drdEstilos As SqlClient.SqlDataReader

        Private m_adpModelos As ModelosDataAdapter
        Private m_drdModelos As SqlClient.SqlDataReader

        Private m_drdAgendas As SqlClient.SqlDataReader
        Private m_adpAgendas As New AgendaDataAdapter

#End Region

#Region "Variables"

        'Variables para la búsqueda
        Private m_intIDAgenda As Integer
        Private m_dtFechaDesde As Date
        Private m_dtFechaHasta As Date
        Private m_strNoCita As String
        Private m_strCardCode As String
        Private m_strNoVehiculo As String
        Private m_strPlaca As String
        Private m_strCodMarca As String
        Private m_strCodEstilo As String
        Private m_strCodModelo As String

#End Region

#End Region

#Region "Eventos"
        Private Sub frmCita_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
            Try

                If Asc(e.KeyChar) = Keys.Escape Then Me.Close()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub
        Private Sub tlbClientes_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbClientes.Click_Nuevo

            Dim Forma_Nueva As Form

            Dim blnExisteForm As Boolean

            Try

                For Each Forma_Nueva In Me.MdiParent.MdiChildren
                    If Forma_Nueva.Name = "frmDetalleCita" Then
                        blnExisteForm = True
                    End If
                Next

                If Not blnExisteForm Then


                    objfrmOpenCita = New frmDetalleCita(True)

                    With objfrmOpenCita

                        '.inicializaTipoInsercion(1)
                        .MdiParent = Me.MdiParent
                        .Show()
                        tlbClientes.Buttons(SCGToolBar.enumButton.Imprimir).Visible = False
                        tlbClientes.EstadoActual = SCGToolBar.enumEstadoToolBar.Modificando
                    End With

                    'llama a la busqueda de cita para refrescar
                    Call Me.BusquedaCita()


                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub
        Private Sub tlbClientes_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbClientes.Click_Cerrar

            Try

                Me.Close()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub
        Private Sub tlbClientes_Click_Buscar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbClientes.Click_Buscar
            Try

                BusquedaCita()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub
        Private Sub tlbClientes_Click_Eliminar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbClientes.Click_Eliminar

            Try

                Eliminar()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub
        Private Sub dtgCitas_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

            Try

                If e.KeyCode = Keys.Delete Then

                    Eliminar()

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub
        Private Sub tlbClientes_Click_Cancelar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbClientes.Click_Cancelar

            Try

                LimpiarCriteriosBusqueda()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub
        Private Sub Manejador_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    BusquedaCita()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try



        End Sub
        Private Sub objBuscador_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles objBuscador.AppAceptar

            Try

                Select Case sender.name

                    Case "picCliente"
                        txtCliente.Text = Campo_Llave
                        txtnombre.Text = Arreglo_Campos(1)

                End Select

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub
        Private Sub objfrmOpenCita_eDatosGuardados(ByVal strNumeroCita As String) Handles objfrmOpenCita.eDatosGuardados


            Try

                Call LimpiarCriteriosBusqueda()


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub
        Private Sub frmCita_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                dtpDesde.Value = objUtilitarios.CargarFechaHoraServidor
                dtpHasta.Value = objUtilitarios.CargarFechaHoraServidor
                tlbClientes.Buttons(SCGToolBar.enumButton.Exportar).Visible = False
                tlbClientes.Buttons(SCGToolBar.enumButton.Guardar).Visible = False
                tlbClientes.Buttons(SCGToolBar.enumButton.Imprimir).Visible = False

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub
        Private Sub picCliente_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picCliente.Click
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                objBuscador.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorClientes
                objBuscador.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre
                objBuscador.Criterios = "CardCode, CardName"
                objBuscador.Tabla = "SCGTA_VW_Clientes"
                objBuscador.Where = ""
                objBuscador.Criterios_Ocultos = 0
                objBuscador.Activar_Buscador(sender)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub
        Private Sub chkMarca_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMarca.CheckedChanged
            Try

                If chkMarca.Checked Then

                    m_adpMarcas = New MarcaDataAdapter
                    m_drdMarcas = Nothing

                    m_adpMarcas.CargaMarcasdeVehiculo(m_drdMarcas)
                    Utilitarios.CargarComboSourceByReader(cboMarca, m_drdMarcas)

                Else

                    cboMarca.DataSource = Nothing
                    cboEstilos.DataSource = Nothing
                    chkEstilo.Checked = False
                    chkModelo.Checked = False

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

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
        Private Sub chkEstilo_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEstilo.CheckedChanged

            Try

                If chkMarca.Checked Then
                    If chkEstilo.Checked Then

                        m_adpEstilos = New EstiloDataAdapter
                        m_drdEstilos = Nothing

                        m_adpEstilos.CargaEstilosdeVehiculo(m_drdEstilos, cboMarca.SelectedValue)
                        Utilitarios.CargarComboSourceByReader(cboEstilos, m_drdEstilos)
                    Else
                        cboEstilos.DataSource = Nothing
                        chkModelo.Checked = False
                        cboModelo.DataSource = Nothing

                    End If
                Else

                    chkModelo.Checked = False
                    cboModelo.DataSource = Nothing
                    cboEstilos.DataSource = Nothing
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
        Private Sub chkModelo_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkModelo.CheckedChanged

            Try

                If chkEstilo.Checked Then
                    If chkModelo.Checked Then

                        m_adpModelos = New ModelosDataAdapter
                        m_drdModelos = Nothing

                        m_adpModelos.CargaModelosdeVehiculo(m_drdModelos, cboEstilos.SelectedValue)
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
        Private Sub cboMarca_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMarca.SelectedIndexChanged
            Try

                If chkEstilo.Checked Then

                    m_adpEstilos = New EstiloDataAdapter
                    m_drdEstilos = Nothing

                    m_adpEstilos.CargaEstilosdeVehiculo(m_drdEstilos, cboMarca.SelectedValue)
                    Utilitarios.CargarComboSourceByReader(cboEstilos, m_drdEstilos)
                Else
                    cboEstilos.DataSource = Nothing

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
                'Agregado 01072010
                If m_drdEstilos IsNot Nothing Then
                    If Not m_drdEstilos.IsClosed Then
                        Call m_drdEstilos.Close()
                    End If
                End If
            End Try
        End Sub
        Private Sub cboEstilos_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEstilos.SelectedIndexChanged
            Try
                If chkModelo.Checked Then

                    m_adpModelos = New ModelosDataAdapter
                    m_drdModelos = Nothing

                    m_adpModelos.CargaModelosdeVehiculo(m_drdModelos, cboEstilos.SelectedValue)
                    Utilitarios.CargarComboSourceByReader(cboModelo, m_drdModelos)
                Else

                    cboModelo.DataSource = Nothing

                End If
            Catch
                Throw
            Finally
                'Agregado 02072010
                If m_drdModelos IsNot Nothing Then
                    If Not m_drdModelos.IsClosed Then
                        Call m_drdModelos.Close()
                    End If
                End If
            End Try
        End Sub
        Private Sub chkAgenda_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAgenda.CheckedChanged

            Try

                If chkAgenda.Checked Then
                    Call m_adpAgendas.Fill(m_drdAgendas)
                    Call Utilitarios.CargarComboSourceByReader(cboAgenda, m_drdAgendas)
                    m_drdAgendas.Close()
                Else

                    cboAgenda.DataSource = Nothing

                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally
                'Agregado
                If m_drdAgendas IsNot Nothing Then
                    If Not m_drdAgendas.IsClosed Then
                        Call m_drdAgendas.Close()
                    End If
                End If

            End Try

        End Sub
        Private Sub dtgCitas_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgCitas.CellClick

            Try
                Me.Cursor = Cursors.WaitCursor

                'llama a la función que  cambia la razón de la cita segun sea la celda seleccionada
                Call MostrarRazon()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
                Me.Cursor = Cursors.Arrow
            End Try

        End Sub
        Private Sub dtgCitas_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgCitas.CellDoubleClick

            Dim intIDCita As Integer

            Dim Forma_Nueva As Form

            Dim blnExisteForm As Boolean

            Try

                'valida que el datagrid no este vacio
                If dtgCitas.CurrentRow IsNot Nothing Then

                    intIDCita = CInt(dtgCitas.CurrentRow.Cells(0).Value)

                    drwCita = m_dstCita.SCGTA_TB_Citas.FindByIDCita(intIDCita)
                    If drwCita IsNot Nothing Then

                        For Each Forma_Nueva In Me.MdiParent.MdiChildren
                            If Forma_Nueva.Name = "frmDetalleCita" Then
                                blnExisteForm = True
                            End If
                        Next

                        If Not blnExisteForm Then
                            If objfrmOpenCita IsNot Nothing Then

                                objfrmOpenCita.Dispose()
                                objfrmOpenCita = Nothing

                            End If
                            objfrmOpenCita = New frmDetalleCita(2, drwCita)
                            With objfrmOpenCita

                                .MdiParent = Me.MdiParent
                                .Show()

                            End With
                        End If

                    End If

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub
#End Region
#Region "Métodos"

        Private Sub BusquedaCita()
            Try
                m_adpCita = New SCGDataAccess.CitasDataAdapter

                If Not m_dstCita Is Nothing Then

                    Call m_dstCita.Dispose()
                    m_dstCita = Nothing
                End If

                m_dstCita = New CitasDataset

                If PasarDatosBusqueda() Then

                    'm_dstCita = Nothing
                    'm_dstCita = New CitasDataset

                    m_adpCita.Fill(m_dstCita, m_dtFechaDesde, m_dtFechaHasta, m_strNoCita, m_strCardCode, m_intIDAgenda, m_strCodMarca, m_strCodEstilo, m_strCodModelo, m_strPlaca, m_strNoVehiculo)
                    dtgCitas.DataSource = m_dstCita.SCGTA_TB_Citas
                    'Coloca la razon de la cita en el txtRazon, inicializa el dtsCita

                    MostrarRazon()
                    'LimpiarCriteriosBusqueda()

                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub LimpiarCriteriosBusqueda()

            txtCliente.Clear()
            txtnombre.Clear()
            txtNoCita.Clear()
            txtPlaca.Clear()
            txtRazon.Clear()
            chkAgenda.Checked = False
            chkEstilo.Checked = False
            chkMarca.Checked = False
            chkModelo.Checked = False
            chkDesde.Checked = False
            chkHasta.Checked = False
            dtpDesde.Value = objUtilitarios.CargarFechaHoraServidor
            dtpHasta.Value = objUtilitarios.CargarFechaHoraServidor

            dtgCitas.DataSource = Nothing
            txtRazon.Clear()

            If m_dstCita IsNot Nothing Then
                m_dstCita.Dispose()
                m_dstCita = New CitasDataset
            End If

            m_intIDAgenda = 0
            m_dtFechaDesde = Nothing
            m_dtFechaHasta = Nothing
            m_strNoCita = ""
            m_strCardCode = ""
            m_strNoVehiculo = ""
            m_strPlaca = ""
            m_strCodMarca = ""
            m_strCodEstilo = ""
            m_strCodModelo = ""

        End Sub

        Private Function PasarDatosBusqueda() As Boolean

            'Variable que determina el si existen o no criterios de búsqueda en caso que no exista ninguno 
            'se devuelve la variable con un false en caso que exista uno o más se devuelve con un true.
            Dim blnValido As Boolean
            Try


                'Se inicia como false...en caso que exista al menos un criterio se convierte en true
                blnValido = False

                If txtCliente.Text <> "" Then
                    m_strCardCode = txtCliente.Text
                    blnValido = True
                Else
                    m_strCardCode = ""
                End If

                If cboMarca.SelectedIndex > -1 Then

                    m_strCodMarca = cboMarca.SelectedValue
                    If cboEstilos.SelectedIndex > -1 Then

                        m_strCodEstilo = cboEstilos.SelectedValue
                        If cboModelo.SelectedIndex > -1 Then

                            m_strCodModelo = cboModelo.SelectedValue
                        Else

                            m_strCodModelo = ""
                        End If
                    Else
                        m_strCodEstilo = ""
                        m_strCodModelo = ""
                    End If
                    blnValido = True
                Else
                    m_strCodMarca = ""
                    m_strCodEstilo = ""
                    m_strCodModelo = ""
                End If

                If txtNoCita.Text <> "" Then
                    m_strNoCita = txtNoCita.Text
                    blnValido = True
                Else
                    m_strNoCita = ""
                End If

                If chkAgenda.Checked Then
                    If cboAgenda.SelectedIndex > -1 Then

                        m_intIDAgenda = cboAgenda.SelectedValue
                        blnValido = True
                    Else
                        m_intIDAgenda = 0

                    End If
                Else
                    m_intIDAgenda = 0
                End If

                If txtPlaca.Text <> "" Then

                    m_strPlaca = txtPlaca.Text
                    blnValido = True
                Else
                    m_strPlaca = ""

                End If

                If txtNoVehiculo.Text <> "" Then

                    m_strNoVehiculo = txtNoVehiculo.Text
                    blnValido = True
                Else
                    m_strNoVehiculo = ""

                End If

                If chkDesde.Checked Then

                    m_dtFechaDesde = New Date(dtpDesde.Value.Year, dtpDesde.Value.Month, dtpDesde.Value.Day, 0, 0, 0)
                    blnValido = True
                Else
                    m_dtFechaDesde = Nothing

                End If

                If chkHasta.Checked Then

                    m_dtFechaHasta = New Date(dtpHasta.Value.Year, dtpHasta.Value.Month, dtpHasta.Value.Day, 23, 59, 59)
                    blnValido = True

                Else

                    m_dtFechaHasta = Nothing

                End If

                Return blnValido

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Function

        Private Sub Eliminar()

            Dim intIDCita As Integer

            Try

                'valida que el datagrid no este vacio
                If dtgCitas.CurrentRow IsNot Nothing Then
                    If drwCita IsNot Nothing Then

                        drwCita = Nothing

                    End If
                    intIDCita = dtgCitas.CurrentRow.Cells("IDCita").Value
                    drwCita = m_dstCita.SCGTA_TB_Citas.FindByIDCita(intIDCita)

                    If drwCita IsNot Nothing Then

                        drwCita.Delete()
                        m_adpCita.Update(m_dstCita)
                        BusquedaCita()

                    End If

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub MostrarRazon()

            Dim intIDCita As Integer

            Try

                'Limpia la razon de cita relacionada con la busqueda anterior
                txtRazon.Clear()

                'valida que el datagrid no este vacio
                If dtgCitas.CurrentRow IsNot Nothing Then
                    If drwCita IsNot Nothing Then

                        drwCita = Nothing

                    End If
                    intIDCita = dtgCitas.CurrentRow.Cells(0).Value
                    drwCita = m_dstCita.SCGTA_TB_Citas.FindByIDCita(intIDCita)

                    If drwCita IsNot Nothing Then
                        If Not drwCita.IsRazonNull Then
                            txtRazon.Text = drwCita.Razon
                            If Not drwCita.IsObservacionesNull Then
                                txtRazon.Text = txtRazon.Text + " (" + drwCita.Observaciones + ")"
                            End If
                        Else
                            If Not drwCita.IsObservacionesNull Then
                                txtRazon.Text = drwCita.Observaciones
                            End If
                        End If
                    End If
                End If


            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

#End Region

        Private Sub tlbClientes_ButtonClick(sender As System.Object, e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbClientes.ButtonClick

        End Sub
    End Class

End Namespace
