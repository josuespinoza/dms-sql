Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmCtrlInformacionVehiculos
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
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCtrlInformacionVehiculos))
            Me.tlbVehiculos = New Proyecto_SCGToolBar.SCGToolBar()
            Me.txtCardCode = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picCliente = New System.Windows.Forms.PictureBox()
            Me.lblCliente = New System.Windows.Forms.Label()
            Me.txtPlaca = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblLineaPlaca = New System.Windows.Forms.Label()
            Me.lblPlaca = New System.Windows.Forms.Label()
            Me.lblEstilo = New System.Windows.Forms.Label()
            Me.cboEstilo = New SCGComboBox.SCGComboBox()
            Me.txtAño = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblAño = New System.Windows.Forms.Label()
            Me.txtCardName = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.cboMarca = New SCGComboBox.SCGComboBox()
            Me.lblMarca = New System.Windows.Forms.Label()
            Me.txtNoUnidad = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblNoUnidad = New System.Windows.Forms.Label()
            Me.cboModelo = New SCGComboBox.SCGComboBox()
            Me.lblModelo = New System.Windows.Forms.Label()
            Me.tbcDatosVehículo = New System.Windows.Forms.TabControl()
            Me.tbpGeneral = New System.Windows.Forms.TabPage()
            Me.Panel4 = New System.Windows.Forms.Panel()
            Me.Label25 = New System.Windows.Forms.Label()
            Me.txtNoPedidoFab = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label26 = New System.Windows.Forms.Label()
            Me.Label13 = New System.Windows.Forms.Label()
            Me.Panel5 = New System.Windows.Forms.Panel()
            Me.dtpFechaVencimientoReserva = New System.Windows.Forms.DateTimePicker()
            Me.chkFechaVencimientoReserva = New System.Windows.Forms.CheckBox()
            Me.Panel2 = New System.Windows.Forms.Panel()
            Me.dtpFechaReserva = New System.Windows.Forms.DateTimePicker()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.chkFechaReserva = New System.Windows.Forms.CheckBox()
            Me.Panel3 = New System.Windows.Forms.Panel()
            Me.Panel1 = New System.Windows.Forms.Panel()
            Me.dtpFechaUltimoServicio = New System.Windows.Forms.DateTimePicker()
            Me.dtpFechaVenta = New System.Windows.Forms.DateTimePicker()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.dtpFechaPxServicio = New System.Windows.Forms.DateTimePicker()
            Me.chkFechaPxServicio = New System.Windows.Forms.CheckBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.chkFechaUltimoServicio = New System.Windows.Forms.CheckBox()
            Me.Label34 = New System.Windows.Forms.Label()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label17 = New System.Windows.Forms.Label()
            Me.Label16 = New System.Windows.Forms.Label()
            Me.Label15 = New System.Windows.Forms.Label()
            Me.Label14 = New System.Windows.Forms.Label()
            Me.Label12 = New System.Windows.Forms.Label()
            Me.txtObservaciones = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblObservaciones = New System.Windows.Forms.Label()
            Me.cboColorTapiceria = New SCGComboBox.SCGComboBox()
            Me.cboColor = New SCGComboBox.SCGComboBox()
            Me.cboEstado = New SCGComboBox.SCGComboBox()
            Me.cboTipo = New SCGComboBox.SCGComboBox()
            Me.lblTipo = New System.Windows.Forms.Label()
            Me.cboUbicacion = New SCGComboBox.SCGComboBox()
            Me.lblUbicacion = New System.Windows.Forms.Label()
            Me.txtVIN = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblVIN = New System.Windows.Forms.Label()
            Me.lblColorTapiceria = New System.Windows.Forms.Label()
            Me.lblColor = New System.Windows.Forms.Label()
            Me.lblEstado = New System.Windows.Forms.Label()
            Me.chkFechaVenta = New System.Windows.Forms.CheckBox()
            Me.tbpDatosEsp = New System.Windows.Forms.TabPage()
            Me.btnArchivos = New System.Windows.Forms.Button()
            Me.Label33 = New System.Windows.Forms.Label()
            Me.Label32 = New System.Windows.Forms.Label()
            Me.Label31 = New System.Windows.Forms.Label()
            Me.Label30 = New System.Windows.Forms.Label()
            Me.Label27 = New System.Windows.Forms.Label()
            Me.Label24 = New System.Windows.Forms.Label()
            Me.Label23 = New System.Windows.Forms.Label()
            Me.Label22 = New System.Windows.Forms.Label()
            Me.Label21 = New System.Windows.Forms.Label()
            Me.Label20 = New System.Windows.Forms.Label()
            Me.Label19 = New System.Windows.Forms.Label()
            Me.Label18 = New System.Windows.Forms.Label()
            Me.Label11 = New System.Windows.Forms.Label()
            Me.txtGarantiaAños = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblGarantiaAños = New System.Windows.Forms.Label()
            Me.txtGarantiaKM = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblGarantiaKM = New System.Windows.Forms.Label()
            Me.txtPotenciaKW = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblPontenciaKW = New System.Windows.Forms.Label()
            Me.txtCilindrada = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblCilindrada = New System.Windows.Forms.Label()
            Me.txtPeso = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblPeso = New System.Windows.Forms.Label()
            Me.txtNoCilindros = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtNoPuertas = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtNoEjes = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblNoEjes = New System.Windows.Forms.Label()
            Me.txtNoPasajeros = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtNoMotor = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblNoMotor = New System.Windows.Forms.Label()
            Me.cboCategoria = New SCGComboBox.SCGComboBox()
            Me.lblCategoria = New System.Windows.Forms.Label()
            Me.cboTecho = New SCGComboBox.SCGComboBox()
            Me.lblLineaTecho = New System.Windows.Forms.Label()
            Me.lblTecho = New System.Windows.Forms.Label()
            Me.cboCombustible = New SCGComboBox.SCGComboBox()
            Me.lblLineaCombustible = New System.Windows.Forms.Label()
            Me.lblCombustible = New System.Windows.Forms.Label()
            Me.cboCabina = New SCGComboBox.SCGComboBox()
            Me.lblLineaCabina = New System.Windows.Forms.Label()
            Me.lblCabina = New System.Windows.Forms.Label()
            Me.cboTraccion = New SCGComboBox.SCGComboBox()
            Me.lblTraccion = New System.Windows.Forms.Label()
            Me.cboCarroceria = New SCGComboBox.SCGComboBox()
            Me.lblLineaCarroceria = New System.Windows.Forms.Label()
            Me.lblCarroceria = New System.Windows.Forms.Label()
            Me.cboTransmision = New SCGComboBox.SCGComboBox()
            Me.lblTransmision = New System.Windows.Forms.Label()
            Me.cboMarcaMotor = New SCGComboBox.SCGComboBox()
            Me.lblLineaMarcaMotor = New System.Windows.Forms.Label()
            Me.lblMarcaMotor = New System.Windows.Forms.Label()
            Me.lblNoPasajeros = New System.Windows.Forms.Label()
            Me.lblNoPuertas = New System.Windows.Forms.Label()
            Me.lblNoCilindros = New System.Windows.Forms.Label()
            Me.errVehiculos = New System.Windows.Forms.ErrorProvider(Me.components)
            Me.rptVehiculo = New ComponenteCristalReport.SubReportView()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.Label9 = New System.Windows.Forms.Label()
            Me.Label10 = New System.Windows.Forms.Label()
            Me.mnuImprimir = New System.Windows.Forms.ContextMenu()
            Me.mnuFichaVehiculo = New System.Windows.Forms.MenuItem()
            Me.mnuHistorialResumido = New System.Windows.Forms.MenuItem()
            Me.VisualizarUDFVehiculo = New ControlUDF.VisualizarUDF()
            CType(Me.picCliente, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tbcDatosVehículo.SuspendLayout()
            Me.tbpGeneral.SuspendLayout()
            Me.tbpDatosEsp.SuspendLayout()
            CType(Me.errVehiculos, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'tlbVehiculos
            '
            resources.ApplyResources(Me.tlbVehiculos, "tlbVehiculos")
            Me.tlbVehiculos.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.tlbVehiculos.Name = "tlbVehiculos"
            '
            'txtCardCode
            '
            Me.txtCardCode.AceptaNegativos = False
            Me.txtCardCode.BackColor = System.Drawing.Color.White
            Me.txtCardCode.EstiloSBO = True
            resources.ApplyResources(Me.txtCardCode, "txtCardCode")
            Me.txtCardCode.MaxDecimales = 0
            Me.txtCardCode.MaxEnteros = 0
            Me.txtCardCode.Millares = False
            Me.txtCardCode.Name = "txtCardCode"
            Me.txtCardCode.Size_AdjustableHeight = 20
            Me.txtCardCode.TeclasDeshacer = True
            Me.txtCardCode.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picCliente
            '
            Me.picCliente.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picCliente, "picCliente")
            Me.picCliente.Name = "picCliente"
            Me.picCliente.TabStop = False
            '
            'lblCliente
            '
            resources.ApplyResources(Me.lblCliente, "lblCliente")
            Me.lblCliente.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCliente.Name = "lblCliente"
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
            'lblLineaPlaca
            '
            Me.lblLineaPlaca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLineaPlaca, "lblLineaPlaca")
            Me.lblLineaPlaca.Name = "lblLineaPlaca"
            '
            'lblPlaca
            '
            resources.ApplyResources(Me.lblPlaca, "lblPlaca")
            Me.lblPlaca.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblPlaca.Name = "lblPlaca"
            '
            'lblEstilo
            '
            resources.ApplyResources(Me.lblEstilo, "lblEstilo")
            Me.lblEstilo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblEstilo.Name = "lblEstilo"
            '
            'cboEstilo
            '
            Me.cboEstilo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboEstilo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstilo.EstiloSBO = True
            resources.ApplyResources(Me.cboEstilo, "cboEstilo")
            Me.cboEstilo.FormattingEnabled = True
            Me.cboEstilo.Name = "cboEstilo"
            '
            'txtAño
            '
            Me.txtAño.AceptaNegativos = False
            Me.txtAño.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtAño.EstiloSBO = True
            resources.ApplyResources(Me.txtAño, "txtAño")
            Me.txtAño.MaxDecimales = 0
            Me.txtAño.MaxEnteros = 0
            Me.txtAño.Millares = False
            Me.txtAño.Name = "txtAño"
            Me.txtAño.Size_AdjustableHeight = 20
            Me.txtAño.TeclasDeshacer = True
            Me.txtAño.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.Numeric
            '
            'lblAño
            '
            resources.ApplyResources(Me.lblAño, "lblAño")
            Me.lblAño.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblAño.Name = "lblAño"
            '
            'txtCardName
            '
            Me.txtCardName.AceptaNegativos = False
            Me.txtCardName.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtCardName.EstiloSBO = True
            resources.ApplyResources(Me.txtCardName, "txtCardName")
            Me.txtCardName.MaxDecimales = 0
            Me.txtCardName.MaxEnteros = 0
            Me.txtCardName.Millares = False
            Me.txtCardName.Name = "txtCardName"
            Me.txtCardName.Size_AdjustableHeight = 20
            Me.txtCardName.TeclasDeshacer = True
            Me.txtCardName.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'cboMarca
            '
            Me.cboMarca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboMarca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboMarca.EstiloSBO = True
            resources.ApplyResources(Me.cboMarca, "cboMarca")
            Me.cboMarca.FormattingEnabled = True
            Me.cboMarca.Name = "cboMarca"
            '
            'lblMarca
            '
            resources.ApplyResources(Me.lblMarca, "lblMarca")
            Me.lblMarca.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblMarca.Name = "lblMarca"
            '
            'txtNoUnidad
            '
            Me.txtNoUnidad.AceptaNegativos = False
            Me.txtNoUnidad.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoUnidad.EstiloSBO = True
            resources.ApplyResources(Me.txtNoUnidad, "txtNoUnidad")
            Me.txtNoUnidad.MaxDecimales = 0
            Me.txtNoUnidad.MaxEnteros = 0
            Me.txtNoUnidad.Millares = False
            Me.txtNoUnidad.Name = "txtNoUnidad"
            Me.txtNoUnidad.Size_AdjustableHeight = 20
            Me.txtNoUnidad.TeclasDeshacer = True
            Me.txtNoUnidad.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblNoUnidad
            '
            resources.ApplyResources(Me.lblNoUnidad, "lblNoUnidad")
            Me.lblNoUnidad.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoUnidad.Name = "lblNoUnidad"
            '
            'cboModelo
            '
            Me.cboModelo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboModelo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboModelo.EstiloSBO = True
            resources.ApplyResources(Me.cboModelo, "cboModelo")
            Me.cboModelo.FormattingEnabled = True
            Me.cboModelo.Name = "cboModelo"
            '
            'lblModelo
            '
            resources.ApplyResources(Me.lblModelo, "lblModelo")
            Me.lblModelo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblModelo.Name = "lblModelo"
            '
            'tbcDatosVehículo
            '
            Me.tbcDatosVehículo.Controls.Add(Me.tbpGeneral)
            Me.tbcDatosVehículo.Controls.Add(Me.tbpDatosEsp)
            resources.ApplyResources(Me.tbcDatosVehículo, "tbcDatosVehículo")
            Me.tbcDatosVehículo.Name = "tbcDatosVehículo"
            Me.tbcDatosVehículo.SelectedIndex = 0
            '
            'tbpGeneral
            '
            Me.tbpGeneral.BackColor = System.Drawing.SystemColors.Control
            Me.tbpGeneral.Controls.Add(Me.Panel4)
            Me.tbpGeneral.Controls.Add(Me.Label25)
            Me.tbpGeneral.Controls.Add(Me.txtNoPedidoFab)
            Me.tbpGeneral.Controls.Add(Me.Label26)
            Me.tbpGeneral.Controls.Add(Me.Label13)
            Me.tbpGeneral.Controls.Add(Me.Panel5)
            Me.tbpGeneral.Controls.Add(Me.dtpFechaVencimientoReserva)
            Me.tbpGeneral.Controls.Add(Me.chkFechaVencimientoReserva)
            Me.tbpGeneral.Controls.Add(Me.Panel2)
            Me.tbpGeneral.Controls.Add(Me.dtpFechaReserva)
            Me.tbpGeneral.Controls.Add(Me.Label7)
            Me.tbpGeneral.Controls.Add(Me.chkFechaReserva)
            Me.tbpGeneral.Controls.Add(Me.Panel3)
            Me.tbpGeneral.Controls.Add(Me.Panel1)
            Me.tbpGeneral.Controls.Add(Me.dtpFechaUltimoServicio)
            Me.tbpGeneral.Controls.Add(Me.dtpFechaVenta)
            Me.tbpGeneral.Controls.Add(Me.Label3)
            Me.tbpGeneral.Controls.Add(Me.dtpFechaPxServicio)
            Me.tbpGeneral.Controls.Add(Me.chkFechaPxServicio)
            Me.tbpGeneral.Controls.Add(Me.Label2)
            Me.tbpGeneral.Controls.Add(Me.chkFechaUltimoServicio)
            Me.tbpGeneral.Controls.Add(Me.Label34)
            Me.tbpGeneral.Controls.Add(Me.Label1)
            Me.tbpGeneral.Controls.Add(Me.Label17)
            Me.tbpGeneral.Controls.Add(Me.Label16)
            Me.tbpGeneral.Controls.Add(Me.Label15)
            Me.tbpGeneral.Controls.Add(Me.Label14)
            Me.tbpGeneral.Controls.Add(Me.Label12)
            Me.tbpGeneral.Controls.Add(Me.txtObservaciones)
            Me.tbpGeneral.Controls.Add(Me.lblObservaciones)
            Me.tbpGeneral.Controls.Add(Me.cboColorTapiceria)
            Me.tbpGeneral.Controls.Add(Me.cboColor)
            Me.tbpGeneral.Controls.Add(Me.cboEstado)
            Me.tbpGeneral.Controls.Add(Me.cboTipo)
            Me.tbpGeneral.Controls.Add(Me.lblTipo)
            Me.tbpGeneral.Controls.Add(Me.cboUbicacion)
            Me.tbpGeneral.Controls.Add(Me.lblUbicacion)
            Me.tbpGeneral.Controls.Add(Me.txtVIN)
            Me.tbpGeneral.Controls.Add(Me.lblVIN)
            Me.tbpGeneral.Controls.Add(Me.lblColorTapiceria)
            Me.tbpGeneral.Controls.Add(Me.lblColor)
            Me.tbpGeneral.Controls.Add(Me.lblEstado)
            Me.tbpGeneral.Controls.Add(Me.chkFechaVenta)
            resources.ApplyResources(Me.tbpGeneral, "tbpGeneral")
            Me.tbpGeneral.Name = "tbpGeneral"
            '
            'Panel4
            '
            resources.ApplyResources(Me.Panel4, "Panel4")
            Me.Panel4.Name = "Panel4"
            '
            'Label25
            '
            Me.Label25.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label25, "Label25")
            Me.Label25.Name = "Label25"
            '
            'txtNoPedidoFab
            '
            Me.txtNoPedidoFab.AceptaNegativos = False
            Me.txtNoPedidoFab.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoPedidoFab.EstiloSBO = True
            resources.ApplyResources(Me.txtNoPedidoFab, "txtNoPedidoFab")
            Me.txtNoPedidoFab.MaxDecimales = 0
            Me.txtNoPedidoFab.MaxEnteros = 0
            Me.txtNoPedidoFab.Millares = False
            Me.txtNoPedidoFab.Name = "txtNoPedidoFab"
            Me.txtNoPedidoFab.Size_AdjustableHeight = 20
            Me.txtNoPedidoFab.TeclasDeshacer = True
            Me.txtNoPedidoFab.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label26
            '
            resources.ApplyResources(Me.Label26, "Label26")
            Me.Label26.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label26.Name = "Label26"
            '
            'Label13
            '
            Me.Label13.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label13, "Label13")
            Me.Label13.Name = "Label13"
            '
            'Panel5
            '
            resources.ApplyResources(Me.Panel5, "Panel5")
            Me.Panel5.Name = "Panel5"
            '
            'dtpFechaVencimientoReserva
            '
            Me.dtpFechaVencimientoReserva.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaVencimientoReserva.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaVencimientoReserva.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaVencimientoReserva.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaVencimientoReserva.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpFechaVencimientoReserva, "dtpFechaVencimientoReserva")
            Me.dtpFechaVencimientoReserva.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpFechaVencimientoReserva.Name = "dtpFechaVencimientoReserva"
            Me.dtpFechaVencimientoReserva.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'chkFechaVencimientoReserva
            '
            resources.ApplyResources(Me.chkFechaVencimientoReserva, "chkFechaVencimientoReserva")
            Me.chkFechaVencimientoReserva.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkFechaVencimientoReserva.Name = "chkFechaVencimientoReserva"
            Me.chkFechaVencimientoReserva.UseVisualStyleBackColor = False
            '
            'Panel2
            '
            resources.ApplyResources(Me.Panel2, "Panel2")
            Me.Panel2.Name = "Panel2"
            '
            'dtpFechaReserva
            '
            Me.dtpFechaReserva.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaReserva.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaReserva.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaReserva.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaReserva.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpFechaReserva, "dtpFechaReserva")
            Me.dtpFechaReserva.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpFechaReserva.Name = "dtpFechaReserva"
            Me.dtpFechaReserva.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'Label7
            '
            Me.Label7.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label7, "Label7")
            Me.Label7.Name = "Label7"
            '
            'chkFechaReserva
            '
            resources.ApplyResources(Me.chkFechaReserva, "chkFechaReserva")
            Me.chkFechaReserva.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkFechaReserva.Name = "chkFechaReserva"
            Me.chkFechaReserva.UseVisualStyleBackColor = False
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
            'dtpFechaUltimoServicio
            '
            Me.dtpFechaUltimoServicio.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaUltimoServicio.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaUltimoServicio.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaUltimoServicio.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaUltimoServicio.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpFechaUltimoServicio, "dtpFechaUltimoServicio")
            Me.dtpFechaUltimoServicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpFechaUltimoServicio.Name = "dtpFechaUltimoServicio"
            Me.dtpFechaUltimoServicio.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'dtpFechaVenta
            '
            Me.dtpFechaVenta.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaVenta.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaVenta.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaVenta.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaVenta.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpFechaVenta, "dtpFechaVenta")
            Me.dtpFechaVenta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpFechaVenta.Name = "dtpFechaVenta"
            Me.dtpFechaVenta.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'Label3
            '
            Me.Label3.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.Name = "Label3"
            '
            'dtpFechaPxServicio
            '
            Me.dtpFechaPxServicio.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaPxServicio.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaPxServicio.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaPxServicio.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaPxServicio.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpFechaPxServicio, "dtpFechaPxServicio")
            Me.dtpFechaPxServicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpFechaPxServicio.Name = "dtpFechaPxServicio"
            Me.dtpFechaPxServicio.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'chkFechaPxServicio
            '
            resources.ApplyResources(Me.chkFechaPxServicio, "chkFechaPxServicio")
            Me.chkFechaPxServicio.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkFechaPxServicio.Name = "chkFechaPxServicio"
            Me.chkFechaPxServicio.UseVisualStyleBackColor = False
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'chkFechaUltimoServicio
            '
            resources.ApplyResources(Me.chkFechaUltimoServicio, "chkFechaUltimoServicio")
            Me.chkFechaUltimoServicio.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkFechaUltimoServicio.Name = "chkFechaUltimoServicio"
            Me.chkFechaUltimoServicio.UseVisualStyleBackColor = False
            '
            'Label34
            '
            Me.Label34.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label34, "Label34")
            Me.Label34.Name = "Label34"
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'Label17
            '
            Me.Label17.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label17, "Label17")
            Me.Label17.Name = "Label17"
            '
            'Label16
            '
            Me.Label16.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label16, "Label16")
            Me.Label16.Name = "Label16"
            '
            'Label15
            '
            Me.Label15.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label15, "Label15")
            Me.Label15.Name = "Label15"
            '
            'Label14
            '
            Me.Label14.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label14, "Label14")
            Me.Label14.Name = "Label14"
            '
            'Label12
            '
            Me.Label12.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label12, "Label12")
            Me.Label12.Name = "Label12"
            '
            'txtObservaciones
            '
            Me.txtObservaciones.AceptaNegativos = False
            Me.txtObservaciones.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtObservaciones.EstiloSBO = True
            resources.ApplyResources(Me.txtObservaciones, "txtObservaciones")
            Me.txtObservaciones.MaxDecimales = 0
            Me.txtObservaciones.MaxEnteros = 0
            Me.txtObservaciones.Millares = False
            Me.txtObservaciones.Name = "txtObservaciones"
            Me.txtObservaciones.Size_AdjustableHeight = 81
            Me.txtObservaciones.TeclasDeshacer = True
            Me.txtObservaciones.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblObservaciones
            '
            resources.ApplyResources(Me.lblObservaciones, "lblObservaciones")
            Me.lblObservaciones.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblObservaciones.Name = "lblObservaciones"
            '
            'cboColorTapiceria
            '
            Me.cboColorTapiceria.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboColorTapiceria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboColorTapiceria.EstiloSBO = True
            resources.ApplyResources(Me.cboColorTapiceria, "cboColorTapiceria")
            Me.cboColorTapiceria.FormattingEnabled = True
            Me.cboColorTapiceria.Name = "cboColorTapiceria"
            '
            'cboColor
            '
            Me.cboColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboColor.EstiloSBO = True
            resources.ApplyResources(Me.cboColor, "cboColor")
            Me.cboColor.FormattingEnabled = True
            Me.cboColor.Name = "cboColor"
            '
            'cboEstado
            '
            Me.cboEstado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstado.EstiloSBO = True
            resources.ApplyResources(Me.cboEstado, "cboEstado")
            Me.cboEstado.FormattingEnabled = True
            Me.cboEstado.Name = "cboEstado"
            '
            'cboTipo
            '
            Me.cboTipo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboTipo.EstiloSBO = True
            resources.ApplyResources(Me.cboTipo, "cboTipo")
            Me.cboTipo.FormattingEnabled = True
            Me.cboTipo.Name = "cboTipo"
            '
            'lblTipo
            '
            resources.ApplyResources(Me.lblTipo, "lblTipo")
            Me.lblTipo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblTipo.Name = "lblTipo"
            '
            'cboUbicacion
            '
            Me.cboUbicacion.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboUbicacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboUbicacion.EstiloSBO = True
            resources.ApplyResources(Me.cboUbicacion, "cboUbicacion")
            Me.cboUbicacion.FormattingEnabled = True
            Me.cboUbicacion.Name = "cboUbicacion"
            '
            'lblUbicacion
            '
            resources.ApplyResources(Me.lblUbicacion, "lblUbicacion")
            Me.lblUbicacion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblUbicacion.Name = "lblUbicacion"
            '
            'txtVIN
            '
            Me.txtVIN.AceptaNegativos = False
            Me.txtVIN.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtVIN.EstiloSBO = True
            resources.ApplyResources(Me.txtVIN, "txtVIN")
            Me.txtVIN.MaxDecimales = 0
            Me.txtVIN.MaxEnteros = 0
            Me.txtVIN.Millares = False
            Me.txtVIN.Name = "txtVIN"
            Me.txtVIN.Size_AdjustableHeight = 20
            Me.txtVIN.TeclasDeshacer = True
            Me.txtVIN.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblVIN
            '
            resources.ApplyResources(Me.lblVIN, "lblVIN")
            Me.lblVIN.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblVIN.Name = "lblVIN"
            '
            'lblColorTapiceria
            '
            resources.ApplyResources(Me.lblColorTapiceria, "lblColorTapiceria")
            Me.lblColorTapiceria.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblColorTapiceria.Name = "lblColorTapiceria"
            '
            'lblColor
            '
            resources.ApplyResources(Me.lblColor, "lblColor")
            Me.lblColor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblColor.Name = "lblColor"
            '
            'lblEstado
            '
            resources.ApplyResources(Me.lblEstado, "lblEstado")
            Me.lblEstado.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblEstado.Name = "lblEstado"
            '
            'chkFechaVenta
            '
            resources.ApplyResources(Me.chkFechaVenta, "chkFechaVenta")
            Me.chkFechaVenta.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkFechaVenta.Name = "chkFechaVenta"
            Me.chkFechaVenta.UseVisualStyleBackColor = False
            '
            'tbpDatosEsp
            '
            Me.tbpDatosEsp.BackColor = System.Drawing.SystemColors.Control
            Me.tbpDatosEsp.Controls.Add(Me.btnArchivos)
            Me.tbpDatosEsp.Controls.Add(Me.Label33)
            Me.tbpDatosEsp.Controls.Add(Me.Label32)
            Me.tbpDatosEsp.Controls.Add(Me.Label31)
            Me.tbpDatosEsp.Controls.Add(Me.Label30)
            Me.tbpDatosEsp.Controls.Add(Me.Label27)
            Me.tbpDatosEsp.Controls.Add(Me.Label24)
            Me.tbpDatosEsp.Controls.Add(Me.Label23)
            Me.tbpDatosEsp.Controls.Add(Me.Label22)
            Me.tbpDatosEsp.Controls.Add(Me.Label21)
            Me.tbpDatosEsp.Controls.Add(Me.Label20)
            Me.tbpDatosEsp.Controls.Add(Me.Label19)
            Me.tbpDatosEsp.Controls.Add(Me.Label18)
            Me.tbpDatosEsp.Controls.Add(Me.Label11)
            Me.tbpDatosEsp.Controls.Add(Me.txtGarantiaAños)
            Me.tbpDatosEsp.Controls.Add(Me.lblGarantiaAños)
            Me.tbpDatosEsp.Controls.Add(Me.txtGarantiaKM)
            Me.tbpDatosEsp.Controls.Add(Me.lblGarantiaKM)
            Me.tbpDatosEsp.Controls.Add(Me.txtPotenciaKW)
            Me.tbpDatosEsp.Controls.Add(Me.lblPontenciaKW)
            Me.tbpDatosEsp.Controls.Add(Me.txtCilindrada)
            Me.tbpDatosEsp.Controls.Add(Me.lblCilindrada)
            Me.tbpDatosEsp.Controls.Add(Me.txtPeso)
            Me.tbpDatosEsp.Controls.Add(Me.lblPeso)
            Me.tbpDatosEsp.Controls.Add(Me.txtNoCilindros)
            Me.tbpDatosEsp.Controls.Add(Me.txtNoPuertas)
            Me.tbpDatosEsp.Controls.Add(Me.txtNoEjes)
            Me.tbpDatosEsp.Controls.Add(Me.lblNoEjes)
            Me.tbpDatosEsp.Controls.Add(Me.txtNoPasajeros)
            Me.tbpDatosEsp.Controls.Add(Me.txtNoMotor)
            Me.tbpDatosEsp.Controls.Add(Me.lblNoMotor)
            Me.tbpDatosEsp.Controls.Add(Me.cboCategoria)
            Me.tbpDatosEsp.Controls.Add(Me.lblCategoria)
            Me.tbpDatosEsp.Controls.Add(Me.cboTecho)
            Me.tbpDatosEsp.Controls.Add(Me.lblLineaTecho)
            Me.tbpDatosEsp.Controls.Add(Me.lblTecho)
            Me.tbpDatosEsp.Controls.Add(Me.cboCombustible)
            Me.tbpDatosEsp.Controls.Add(Me.lblLineaCombustible)
            Me.tbpDatosEsp.Controls.Add(Me.lblCombustible)
            Me.tbpDatosEsp.Controls.Add(Me.cboCabina)
            Me.tbpDatosEsp.Controls.Add(Me.lblLineaCabina)
            Me.tbpDatosEsp.Controls.Add(Me.lblCabina)
            Me.tbpDatosEsp.Controls.Add(Me.cboTraccion)
            Me.tbpDatosEsp.Controls.Add(Me.lblTraccion)
            Me.tbpDatosEsp.Controls.Add(Me.cboCarroceria)
            Me.tbpDatosEsp.Controls.Add(Me.lblLineaCarroceria)
            Me.tbpDatosEsp.Controls.Add(Me.lblCarroceria)
            Me.tbpDatosEsp.Controls.Add(Me.cboTransmision)
            Me.tbpDatosEsp.Controls.Add(Me.lblTransmision)
            Me.tbpDatosEsp.Controls.Add(Me.cboMarcaMotor)
            Me.tbpDatosEsp.Controls.Add(Me.lblLineaMarcaMotor)
            Me.tbpDatosEsp.Controls.Add(Me.lblMarcaMotor)
            Me.tbpDatosEsp.Controls.Add(Me.lblNoPasajeros)
            Me.tbpDatosEsp.Controls.Add(Me.lblNoPuertas)
            Me.tbpDatosEsp.Controls.Add(Me.lblNoCilindros)
            Me.tbpDatosEsp.ForeColor = System.Drawing.SystemColors.Control
            resources.ApplyResources(Me.tbpDatosEsp, "tbpDatosEsp")
            Me.tbpDatosEsp.Name = "tbpDatosEsp"
            '
            'btnArchivos
            '
            resources.ApplyResources(Me.btnArchivos, "btnArchivos")
            Me.btnArchivos.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.btnArchivos.Name = "btnArchivos"
            '
            'Label33
            '
            Me.Label33.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label33, "Label33")
            Me.Label33.Name = "Label33"
            '
            'Label32
            '
            Me.Label32.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label32, "Label32")
            Me.Label32.Name = "Label32"
            '
            'Label31
            '
            Me.Label31.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label31, "Label31")
            Me.Label31.Name = "Label31"
            '
            'Label30
            '
            Me.Label30.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label30, "Label30")
            Me.Label30.Name = "Label30"
            '
            'Label27
            '
            Me.Label27.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label27, "Label27")
            Me.Label27.Name = "Label27"
            '
            'Label24
            '
            Me.Label24.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label24, "Label24")
            Me.Label24.Name = "Label24"
            '
            'Label23
            '
            Me.Label23.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label23, "Label23")
            Me.Label23.Name = "Label23"
            '
            'Label22
            '
            Me.Label22.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label22, "Label22")
            Me.Label22.Name = "Label22"
            '
            'Label21
            '
            Me.Label21.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label21, "Label21")
            Me.Label21.Name = "Label21"
            '
            'Label20
            '
            Me.Label20.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label20, "Label20")
            Me.Label20.Name = "Label20"
            '
            'Label19
            '
            Me.Label19.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label19, "Label19")
            Me.Label19.Name = "Label19"
            '
            'Label18
            '
            Me.Label18.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label18, "Label18")
            Me.Label18.Name = "Label18"
            '
            'Label11
            '
            Me.Label11.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label11, "Label11")
            Me.Label11.Name = "Label11"
            '
            'txtGarantiaAños
            '
            Me.txtGarantiaAños.AceptaNegativos = False
            Me.txtGarantiaAños.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtGarantiaAños.EstiloSBO = True
            resources.ApplyResources(Me.txtGarantiaAños, "txtGarantiaAños")
            Me.txtGarantiaAños.MaxDecimales = 0
            Me.txtGarantiaAños.MaxEnteros = 0
            Me.txtGarantiaAños.Millares = False
            Me.txtGarantiaAños.Name = "txtGarantiaAños"
            Me.txtGarantiaAños.Size_AdjustableHeight = 20
            Me.txtGarantiaAños.TeclasDeshacer = True
            Me.txtGarantiaAños.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblGarantiaAños
            '
            resources.ApplyResources(Me.lblGarantiaAños, "lblGarantiaAños")
            Me.lblGarantiaAños.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblGarantiaAños.Name = "lblGarantiaAños"
            '
            'txtGarantiaKM
            '
            Me.txtGarantiaKM.AceptaNegativos = False
            Me.txtGarantiaKM.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtGarantiaKM.EstiloSBO = True
            resources.ApplyResources(Me.txtGarantiaKM, "txtGarantiaKM")
            Me.txtGarantiaKM.MaxDecimales = 0
            Me.txtGarantiaKM.MaxEnteros = 0
            Me.txtGarantiaKM.Millares = False
            Me.txtGarantiaKM.Name = "txtGarantiaKM"
            Me.txtGarantiaKM.Size_AdjustableHeight = 20
            Me.txtGarantiaKM.TeclasDeshacer = True
            Me.txtGarantiaKM.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblGarantiaKM
            '
            resources.ApplyResources(Me.lblGarantiaKM, "lblGarantiaKM")
            Me.lblGarantiaKM.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblGarantiaKM.Name = "lblGarantiaKM"
            '
            'txtPotenciaKW
            '
            Me.txtPotenciaKW.AceptaNegativos = False
            Me.txtPotenciaKW.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtPotenciaKW.EstiloSBO = True
            resources.ApplyResources(Me.txtPotenciaKW, "txtPotenciaKW")
            Me.txtPotenciaKW.MaxDecimales = 0
            Me.txtPotenciaKW.MaxEnteros = 0
            Me.txtPotenciaKW.Millares = False
            Me.txtPotenciaKW.Name = "txtPotenciaKW"
            Me.txtPotenciaKW.Size_AdjustableHeight = 20
            Me.txtPotenciaKW.TeclasDeshacer = True
            Me.txtPotenciaKW.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblPontenciaKW
            '
            resources.ApplyResources(Me.lblPontenciaKW, "lblPontenciaKW")
            Me.lblPontenciaKW.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblPontenciaKW.Name = "lblPontenciaKW"
            '
            'txtCilindrada
            '
            Me.txtCilindrada.AceptaNegativos = False
            Me.txtCilindrada.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtCilindrada.EstiloSBO = True
            resources.ApplyResources(Me.txtCilindrada, "txtCilindrada")
            Me.txtCilindrada.MaxDecimales = 0
            Me.txtCilindrada.MaxEnteros = 0
            Me.txtCilindrada.Millares = False
            Me.txtCilindrada.Name = "txtCilindrada"
            Me.txtCilindrada.Size_AdjustableHeight = 20
            Me.txtCilindrada.TeclasDeshacer = True
            Me.txtCilindrada.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblCilindrada
            '
            resources.ApplyResources(Me.lblCilindrada, "lblCilindrada")
            Me.lblCilindrada.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCilindrada.Name = "lblCilindrada"
            '
            'txtPeso
            '
            Me.txtPeso.AceptaNegativos = False
            Me.txtPeso.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtPeso.EstiloSBO = True
            resources.ApplyResources(Me.txtPeso, "txtPeso")
            Me.txtPeso.MaxDecimales = 0
            Me.txtPeso.MaxEnteros = 0
            Me.txtPeso.Millares = False
            Me.txtPeso.Name = "txtPeso"
            Me.txtPeso.Size_AdjustableHeight = 20
            Me.txtPeso.TeclasDeshacer = True
            Me.txtPeso.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblPeso
            '
            resources.ApplyResources(Me.lblPeso, "lblPeso")
            Me.lblPeso.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblPeso.Name = "lblPeso"
            '
            'txtNoCilindros
            '
            Me.txtNoCilindros.AceptaNegativos = False
            Me.txtNoCilindros.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoCilindros.EstiloSBO = True
            resources.ApplyResources(Me.txtNoCilindros, "txtNoCilindros")
            Me.txtNoCilindros.MaxDecimales = 0
            Me.txtNoCilindros.MaxEnteros = 0
            Me.txtNoCilindros.Millares = False
            Me.txtNoCilindros.Name = "txtNoCilindros"
            Me.txtNoCilindros.Size_AdjustableHeight = 20
            Me.txtNoCilindros.TeclasDeshacer = True
            Me.txtNoCilindros.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtNoPuertas
            '
            Me.txtNoPuertas.AceptaNegativos = False
            Me.txtNoPuertas.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoPuertas.EstiloSBO = True
            resources.ApplyResources(Me.txtNoPuertas, "txtNoPuertas")
            Me.txtNoPuertas.MaxDecimales = 0
            Me.txtNoPuertas.MaxEnteros = 0
            Me.txtNoPuertas.Millares = False
            Me.txtNoPuertas.Name = "txtNoPuertas"
            Me.txtNoPuertas.Size_AdjustableHeight = 20
            Me.txtNoPuertas.TeclasDeshacer = True
            Me.txtNoPuertas.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtNoEjes
            '
            Me.txtNoEjes.AceptaNegativos = False
            Me.txtNoEjes.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoEjes.EstiloSBO = True
            resources.ApplyResources(Me.txtNoEjes, "txtNoEjes")
            Me.txtNoEjes.MaxDecimales = 0
            Me.txtNoEjes.MaxEnteros = 0
            Me.txtNoEjes.Millares = False
            Me.txtNoEjes.Name = "txtNoEjes"
            Me.txtNoEjes.Size_AdjustableHeight = 20
            Me.txtNoEjes.TeclasDeshacer = True
            Me.txtNoEjes.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblNoEjes
            '
            resources.ApplyResources(Me.lblNoEjes, "lblNoEjes")
            Me.lblNoEjes.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoEjes.Name = "lblNoEjes"
            '
            'txtNoPasajeros
            '
            Me.txtNoPasajeros.AceptaNegativos = False
            Me.txtNoPasajeros.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoPasajeros.EstiloSBO = True
            resources.ApplyResources(Me.txtNoPasajeros, "txtNoPasajeros")
            Me.txtNoPasajeros.MaxDecimales = 0
            Me.txtNoPasajeros.MaxEnteros = 0
            Me.txtNoPasajeros.Millares = False
            Me.txtNoPasajeros.Name = "txtNoPasajeros"
            Me.txtNoPasajeros.Size_AdjustableHeight = 20
            Me.txtNoPasajeros.TeclasDeshacer = True
            Me.txtNoPasajeros.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtNoMotor
            '
            Me.txtNoMotor.AceptaNegativos = False
            Me.txtNoMotor.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoMotor.EstiloSBO = True
            resources.ApplyResources(Me.txtNoMotor, "txtNoMotor")
            Me.txtNoMotor.MaxDecimales = 0
            Me.txtNoMotor.MaxEnteros = 0
            Me.txtNoMotor.Millares = False
            Me.txtNoMotor.Name = "txtNoMotor"
            Me.txtNoMotor.Size_AdjustableHeight = 20
            Me.txtNoMotor.TeclasDeshacer = True
            Me.txtNoMotor.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblNoMotor
            '
            resources.ApplyResources(Me.lblNoMotor, "lblNoMotor")
            Me.lblNoMotor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoMotor.Name = "lblNoMotor"
            '
            'cboCategoria
            '
            Me.cboCategoria.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboCategoria.EstiloSBO = True
            resources.ApplyResources(Me.cboCategoria, "cboCategoria")
            Me.cboCategoria.FormattingEnabled = True
            Me.cboCategoria.Name = "cboCategoria"
            '
            'lblCategoria
            '
            resources.ApplyResources(Me.lblCategoria, "lblCategoria")
            Me.lblCategoria.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCategoria.Name = "lblCategoria"
            '
            'cboTecho
            '
            Me.cboTecho.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboTecho.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboTecho.EstiloSBO = True
            resources.ApplyResources(Me.cboTecho, "cboTecho")
            Me.cboTecho.FormattingEnabled = True
            Me.cboTecho.Name = "cboTecho"
            '
            'lblLineaTecho
            '
            Me.lblLineaTecho.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLineaTecho, "lblLineaTecho")
            Me.lblLineaTecho.Name = "lblLineaTecho"
            '
            'lblTecho
            '
            resources.ApplyResources(Me.lblTecho, "lblTecho")
            Me.lblTecho.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblTecho.Name = "lblTecho"
            '
            'cboCombustible
            '
            Me.cboCombustible.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboCombustible.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboCombustible.EstiloSBO = True
            resources.ApplyResources(Me.cboCombustible, "cboCombustible")
            Me.cboCombustible.FormattingEnabled = True
            Me.cboCombustible.Name = "cboCombustible"
            '
            'lblLineaCombustible
            '
            Me.lblLineaCombustible.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLineaCombustible, "lblLineaCombustible")
            Me.lblLineaCombustible.Name = "lblLineaCombustible"
            '
            'lblCombustible
            '
            resources.ApplyResources(Me.lblCombustible, "lblCombustible")
            Me.lblCombustible.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCombustible.Name = "lblCombustible"
            '
            'cboCabina
            '
            Me.cboCabina.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboCabina.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboCabina.EstiloSBO = True
            resources.ApplyResources(Me.cboCabina, "cboCabina")
            Me.cboCabina.FormattingEnabled = True
            Me.cboCabina.Name = "cboCabina"
            '
            'lblLineaCabina
            '
            Me.lblLineaCabina.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLineaCabina, "lblLineaCabina")
            Me.lblLineaCabina.Name = "lblLineaCabina"
            '
            'lblCabina
            '
            resources.ApplyResources(Me.lblCabina, "lblCabina")
            Me.lblCabina.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCabina.Name = "lblCabina"
            '
            'cboTraccion
            '
            Me.cboTraccion.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboTraccion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboTraccion.EstiloSBO = True
            resources.ApplyResources(Me.cboTraccion, "cboTraccion")
            Me.cboTraccion.FormattingEnabled = True
            Me.cboTraccion.Name = "cboTraccion"
            '
            'lblTraccion
            '
            resources.ApplyResources(Me.lblTraccion, "lblTraccion")
            Me.lblTraccion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblTraccion.Name = "lblTraccion"
            '
            'cboCarroceria
            '
            Me.cboCarroceria.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboCarroceria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboCarroceria.EstiloSBO = True
            resources.ApplyResources(Me.cboCarroceria, "cboCarroceria")
            Me.cboCarroceria.FormattingEnabled = True
            Me.cboCarroceria.Name = "cboCarroceria"
            '
            'lblLineaCarroceria
            '
            Me.lblLineaCarroceria.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLineaCarroceria, "lblLineaCarroceria")
            Me.lblLineaCarroceria.Name = "lblLineaCarroceria"
            '
            'lblCarroceria
            '
            resources.ApplyResources(Me.lblCarroceria, "lblCarroceria")
            Me.lblCarroceria.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCarroceria.Name = "lblCarroceria"
            '
            'cboTransmision
            '
            Me.cboTransmision.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboTransmision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboTransmision.EstiloSBO = True
            resources.ApplyResources(Me.cboTransmision, "cboTransmision")
            Me.cboTransmision.FormattingEnabled = True
            Me.cboTransmision.Name = "cboTransmision"
            '
            'lblTransmision
            '
            resources.ApplyResources(Me.lblTransmision, "lblTransmision")
            Me.lblTransmision.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblTransmision.Name = "lblTransmision"
            '
            'cboMarcaMotor
            '
            Me.cboMarcaMotor.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboMarcaMotor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboMarcaMotor.EstiloSBO = True
            resources.ApplyResources(Me.cboMarcaMotor, "cboMarcaMotor")
            Me.cboMarcaMotor.FormattingEnabled = True
            Me.cboMarcaMotor.Name = "cboMarcaMotor"
            '
            'lblLineaMarcaMotor
            '
            Me.lblLineaMarcaMotor.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLineaMarcaMotor, "lblLineaMarcaMotor")
            Me.lblLineaMarcaMotor.Name = "lblLineaMarcaMotor"
            '
            'lblMarcaMotor
            '
            resources.ApplyResources(Me.lblMarcaMotor, "lblMarcaMotor")
            Me.lblMarcaMotor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblMarcaMotor.Name = "lblMarcaMotor"
            '
            'lblNoPasajeros
            '
            resources.ApplyResources(Me.lblNoPasajeros, "lblNoPasajeros")
            Me.lblNoPasajeros.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoPasajeros.Name = "lblNoPasajeros"
            '
            'lblNoPuertas
            '
            resources.ApplyResources(Me.lblNoPuertas, "lblNoPuertas")
            Me.lblNoPuertas.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoPuertas.Name = "lblNoPuertas"
            '
            'lblNoCilindros
            '
            resources.ApplyResources(Me.lblNoCilindros, "lblNoCilindros")
            Me.lblNoCilindros.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoCilindros.Name = "lblNoCilindros"
            '
            'errVehiculos
            '
            Me.errVehiculos.ContainerControl = Me
            '
            'rptVehiculo
            '
            Me.rptVehiculo.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.rptVehiculo, "rptVehiculo")
            Me.rptVehiculo.Name = "rptVehiculo"
            Me.rptVehiculo.P_BarraTitulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptVehiculo.P_CompanyName = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptVehiculo.P_DataBase = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptVehiculo.P_Filename = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptVehiculo.P_NCopias = 0
            Me.rptVehiculo.P_Owner = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptVehiculo.P_ParArray = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptVehiculo.P_Password = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptVehiculo.P_Server = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptVehiculo.P_User = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptVehiculo.P_WorkFolder = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'Label4
            '
            Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Name = "Label4"
            '
            'Label5
            '
            Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.Name = "Label5"
            '
            'Label6
            '
            Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label6, "Label6")
            Me.Label6.Name = "Label6"
            '
            'Label8
            '
            Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label8, "Label8")
            Me.Label8.Name = "Label8"
            '
            'Label9
            '
            Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label9, "Label9")
            Me.Label9.Name = "Label9"
            '
            'Label10
            '
            Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label10, "Label10")
            Me.Label10.Name = "Label10"
            '
            'mnuImprimir
            '
            Me.mnuImprimir.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFichaVehiculo, Me.mnuHistorialResumido})
            '
            'mnuFichaVehiculo
            '
            Me.mnuFichaVehiculo.Index = 0
            resources.ApplyResources(Me.mnuFichaVehiculo, "mnuFichaVehiculo")
            '
            'mnuHistorialResumido
            '
            Me.mnuHistorialResumido.Index = 1
            resources.ApplyResources(Me.mnuHistorialResumido, "mnuHistorialResumido")
            '
            'VisualizarUDFVehiculo
            '
            resources.ApplyResources(Me.VisualizarUDFVehiculo, "VisualizarUDFVehiculo")
            Me.VisualizarUDFVehiculo.CampoLlave = Nothing
            Me.VisualizarUDFVehiculo.CodigoFormularioSBO = 0
            Me.VisualizarUDFVehiculo.CodigoUsuario = 0
            Me.VisualizarUDFVehiculo.Conexion = Nothing
            Me.VisualizarUDFVehiculo.Form = Nothing
            Me.VisualizarUDFVehiculo.Name = "VisualizarUDFVehiculo"
            Me.VisualizarUDFVehiculo.NombreBaseDatosSBO = Nothing
            Me.VisualizarUDFVehiculo.Tabla = Nothing
            Me.VisualizarUDFVehiculo.VisualizarUDFSBO = False
            Me.VisualizarUDFVehiculo.Where = Nothing
            '
            'frmCtrlInformacionVehiculos
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.VisualizarUDFVehiculo)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.lblLineaPlaca)
            Me.Controls.Add(Me.Label6)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.rptVehiculo)
            Me.Controls.Add(Me.lblCliente)
            Me.Controls.Add(Me.lblNoUnidad)
            Me.Controls.Add(Me.Label8)
            Me.Controls.Add(Me.Label10)
            Me.Controls.Add(Me.Label9)
            Me.Controls.Add(Me.txtNoUnidad)
            Me.Controls.Add(Me.tbcDatosVehículo)
            Me.Controls.Add(Me.lblMarca)
            Me.Controls.Add(Me.txtPlaca)
            Me.Controls.Add(Me.lblAño)
            Me.Controls.Add(Me.cboMarca)
            Me.Controls.Add(Me.txtCardCode)
            Me.Controls.Add(Me.lblPlaca)
            Me.Controls.Add(Me.lblModelo)
            Me.Controls.Add(Me.txtAño)
            Me.Controls.Add(Me.cboModelo)
            Me.Controls.Add(Me.picCliente)
            Me.Controls.Add(Me.txtCardName)
            Me.Controls.Add(Me.lblEstilo)
            Me.Controls.Add(Me.cboEstilo)
            Me.Controls.Add(Me.tlbVehiculos)
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.MaximizeBox = False
            Me.Name = "frmCtrlInformacionVehiculos"
            CType(Me.picCliente, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tbcDatosVehículo.ResumeLayout(False)
            Me.tbpGeneral.ResumeLayout(False)
            Me.tbpGeneral.PerformLayout()
            Me.tbpDatosEsp.ResumeLayout(False)
            Me.tbpDatosEsp.PerformLayout()
            CType(Me.errVehiculos, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents tlbVehiculos As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents txtCardCode As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picCliente As System.Windows.Forms.PictureBox
        Friend WithEvents lblCliente As System.Windows.Forms.Label
        Friend WithEvents txtPlaca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblLineaPlaca As System.Windows.Forms.Label
        Friend WithEvents lblPlaca As System.Windows.Forms.Label
        Friend WithEvents lblEstilo As System.Windows.Forms.Label
        Friend WithEvents cboEstilo As SCGComboBox.SCGComboBox
        Friend WithEvents txtAño As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblAño As System.Windows.Forms.Label
        Friend WithEvents txtCardName As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents cboMarca As SCGComboBox.SCGComboBox
        Friend WithEvents lblMarca As System.Windows.Forms.Label
        Friend WithEvents txtNoUnidad As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblNoUnidad As System.Windows.Forms.Label
        Friend WithEvents cboModelo As SCGComboBox.SCGComboBox
        Friend WithEvents lblModelo As System.Windows.Forms.Label
        Friend WithEvents tbcDatosVehículo As System.Windows.Forms.TabControl
        Friend WithEvents errVehiculos As System.Windows.Forms.ErrorProvider
        Friend WithEvents rptVehiculo As ComponenteCristalReport.SubReportView
        Friend WithEvents mnuImprimir As System.Windows.Forms.ContextMenu
        Friend WithEvents mnuFichaVehiculo As System.Windows.Forms.MenuItem
        Friend WithEvents mnuHistorialResumido As System.Windows.Forms.MenuItem
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents tbpGeneral As System.Windows.Forms.TabPage
        Friend WithEvents Label17 As System.Windows.Forms.Label
        Friend WithEvents Label16 As System.Windows.Forms.Label
        Friend WithEvents Label15 As System.Windows.Forms.Label
        Friend WithEvents Label14 As System.Windows.Forms.Label
        Friend WithEvents Label12 As System.Windows.Forms.Label
        Friend WithEvents txtObservaciones As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblObservaciones As System.Windows.Forms.Label
        Friend WithEvents cboColorTapiceria As SCGComboBox.SCGComboBox
        Friend WithEvents cboColor As SCGComboBox.SCGComboBox
        Friend WithEvents cboEstado As SCGComboBox.SCGComboBox
        Friend WithEvents cboTipo As SCGComboBox.SCGComboBox
        Friend WithEvents lblTipo As System.Windows.Forms.Label
        Friend WithEvents cboUbicacion As SCGComboBox.SCGComboBox
        Friend WithEvents lblUbicacion As System.Windows.Forms.Label
        Friend WithEvents txtVIN As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblVIN As System.Windows.Forms.Label
        Friend WithEvents lblColorTapiceria As System.Windows.Forms.Label
        Friend WithEvents lblColor As System.Windows.Forms.Label
        Friend WithEvents lblEstado As System.Windows.Forms.Label
        Friend WithEvents chkFechaVenta As System.Windows.Forms.CheckBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label34 As System.Windows.Forms.Label
        Friend WithEvents tbpDatosEsp As System.Windows.Forms.TabPage
        Friend WithEvents Label33 As System.Windows.Forms.Label
        Friend WithEvents Label32 As System.Windows.Forms.Label
        Friend WithEvents Label31 As System.Windows.Forms.Label
        Friend WithEvents Label30 As System.Windows.Forms.Label
        Friend WithEvents Label24 As System.Windows.Forms.Label
        Friend WithEvents Label23 As System.Windows.Forms.Label
        Friend WithEvents Label22 As System.Windows.Forms.Label
        Friend WithEvents Label21 As System.Windows.Forms.Label
        Friend WithEvents Label20 As System.Windows.Forms.Label
        Friend WithEvents Label19 As System.Windows.Forms.Label
        Friend WithEvents Label18 As System.Windows.Forms.Label
        Friend WithEvents Label11 As System.Windows.Forms.Label
        Friend WithEvents txtGarantiaAños As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblGarantiaAños As System.Windows.Forms.Label
        Friend WithEvents txtGarantiaKM As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblGarantiaKM As System.Windows.Forms.Label
        Friend WithEvents txtPotenciaKW As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblPontenciaKW As System.Windows.Forms.Label
        Friend WithEvents txtCilindrada As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblCilindrada As System.Windows.Forms.Label
        Friend WithEvents txtPeso As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblPeso As System.Windows.Forms.Label
        Friend WithEvents txtNoCilindros As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtNoPuertas As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtNoEjes As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblNoEjes As System.Windows.Forms.Label
        Friend WithEvents txtNoPasajeros As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtNoMotor As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblNoMotor As System.Windows.Forms.Label
        Friend WithEvents cboCategoria As SCGComboBox.SCGComboBox
        Friend WithEvents lblCategoria As System.Windows.Forms.Label
        Friend WithEvents cboTecho As SCGComboBox.SCGComboBox
        Friend WithEvents lblTecho As System.Windows.Forms.Label
        Friend WithEvents cboCombustible As SCGComboBox.SCGComboBox
        Friend WithEvents lblCombustible As System.Windows.Forms.Label
        Friend WithEvents cboCabina As SCGComboBox.SCGComboBox
        Friend WithEvents lblLineaCabina As System.Windows.Forms.Label
        Friend WithEvents lblCabina As System.Windows.Forms.Label
        Friend WithEvents cboTraccion As SCGComboBox.SCGComboBox
        Friend WithEvents lblTraccion As System.Windows.Forms.Label
        Friend WithEvents cboCarroceria As SCGComboBox.SCGComboBox
        Friend WithEvents lblCarroceria As System.Windows.Forms.Label
        Friend WithEvents cboTransmision As SCGComboBox.SCGComboBox
        Friend WithEvents lblTransmision As System.Windows.Forms.Label
        Friend WithEvents cboMarcaMotor As SCGComboBox.SCGComboBox
        Friend WithEvents lblLineaMarcaMotor As System.Windows.Forms.Label
        Friend WithEvents lblMarcaMotor As System.Windows.Forms.Label
        Friend WithEvents lblNoPasajeros As System.Windows.Forms.Label
        Friend WithEvents lblNoPuertas As System.Windows.Forms.Label
        Friend WithEvents lblNoCilindros As System.Windows.Forms.Label
        Friend WithEvents Label27 As System.Windows.Forms.Label
        Friend WithEvents lblLineaTecho As System.Windows.Forms.Label
        Friend WithEvents lblLineaCombustible As System.Windows.Forms.Label
        Friend WithEvents lblLineaCarroceria As System.Windows.Forms.Label
        Friend WithEvents btnArchivos As System.Windows.Forms.Button
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents dtpFechaUltimoServicio As System.Windows.Forms.DateTimePicker
        Friend WithEvents Panel3 As System.Windows.Forms.Panel
        Friend WithEvents chkFechaUltimoServicio As System.Windows.Forms.CheckBox
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Panel4 As System.Windows.Forms.Panel
        Friend WithEvents dtpFechaPxServicio As System.Windows.Forms.DateTimePicker
        Friend WithEvents chkFechaPxServicio As System.Windows.Forms.CheckBox
        Friend WithEvents Label13 As System.Windows.Forms.Label
        Friend WithEvents Panel5 As System.Windows.Forms.Panel
        Friend WithEvents dtpFechaVencimientoReserva As System.Windows.Forms.DateTimePicker
        Friend WithEvents chkFechaVencimientoReserva As System.Windows.Forms.CheckBox
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents dtpFechaReserva As System.Windows.Forms.DateTimePicker
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents chkFechaReserva As System.Windows.Forms.CheckBox
        Friend WithEvents dtpFechaVenta As System.Windows.Forms.DateTimePicker
        Friend WithEvents Label25 As System.Windows.Forms.Label
        Friend WithEvents txtNoPedidoFab As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label26 As System.Windows.Forms.Label
        Friend WithEvents VisualizarUDFVehiculo As ControlUDF.VisualizarUDF
    End Class

End Namespace