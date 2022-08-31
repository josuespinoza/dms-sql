Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Class frmOrdenes
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
        Friend WithEvents txtObservacion As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents cboEstilo As SCGComboBox.SCGComboBox
        Friend WithEvents lblNoexpediente As System.Windows.Forms.Label
        Friend WithEvents cboMarca As SCGComboBox.SCGComboBox
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents cboEstado As SCGComboBox.SCGComboBox
        Friend WithEvents txtNoOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblNoOrden As System.Windows.Forms.Label
        Friend WithEvents grpOrdenInfo As System.Windows.Forms.GroupBox
        Friend WithEvents ScgToolBar1 As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents SubBuscador As Buscador.SubBuscador
        Friend WithEvents dtgOrdenes As System.Windows.Forms.DataGrid
        Friend WithEvents txtVisita As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtcono As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtPlaca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents lblLine8 As System.Windows.Forms.Label
        Public WithEvents lblLine3 As System.Windows.Forms.Label
        Public WithEvents lblLine7 As System.Windows.Forms.Label
        Public WithEvents lblLine4 As System.Windows.Forms.Label
        Public WithEvents lblLine2 As System.Windows.Forms.Label
        Public WithEvents lblLine6 As System.Windows.Forms.Label
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents txtVehiculo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblNoVehiculo As System.Windows.Forms.Label
        Public WithEvents lblLine5 As System.Windows.Forms.Label
        Friend WithEvents cboModelo As SCGComboBox.SCGComboBox
        Friend WithEvents chkMarca As System.Windows.Forms.CheckBox
        Friend WithEvents chkEstilo As System.Windows.Forms.CheckBox
        Friend WithEvents chkModelo As System.Windows.Forms.CheckBox
        Friend WithEvents chkEstado As System.Windows.Forms.CheckBox
        Friend WithEvents Panel5 As System.Windows.Forms.Panel
        Friend WithEvents Panel6 As System.Windows.Forms.Panel
        Friend WithEvents Panel7 As System.Windows.Forms.Panel
        Friend WithEvents dtpCierreini As System.Windows.Forms.DateTimePicker
        Public WithEvents lblLine10 As System.Windows.Forms.Label
        Friend WithEvents dtpCompromisoini As System.Windows.Forms.DateTimePicker
        Friend WithEvents dtpAperturaini As System.Windows.Forms.DateTimePicker
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents dtpCierrefin As System.Windows.Forms.DateTimePicker
        Friend WithEvents Panel3 As System.Windows.Forms.Panel
        Friend WithEvents dtpCompromisofin As System.Windows.Forms.DateTimePicker
        Friend WithEvents Panel4 As System.Windows.Forms.Panel
        Friend WithEvents dtpAperturafin As System.Windows.Forms.DateTimePicker
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents Panel9 As System.Windows.Forms.Panel
        Public WithEvents lblLine9 As System.Windows.Forms.Label
        Friend WithEvents Panel10 As System.Windows.Forms.Panel
        Public WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents chkCompromiso As System.Windows.Forms.CheckBox
        Friend WithEvents chkCierre As System.Windows.Forms.CheckBox
        Friend WithEvents chkApertura As System.Windows.Forms.CheckBox
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrdenes))
            Me.grpCitas = New System.Windows.Forms.GroupBox
            Me.txtObservacion = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.dtgOrdenes = New System.Windows.Forms.DataGrid
            Me.lblLine8 = New System.Windows.Forms.Label
            Me.cboEstilo = New SCGComboBox.SCGComboBox
            Me.lblLine3 = New System.Windows.Forms.Label
            Me.lblNoexpediente = New System.Windows.Forms.Label
            Me.txtVisita = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.lblLine7 = New System.Windows.Forms.Label
            Me.cboMarca = New SCGComboBox.SCGComboBox
            Me.lblLine4 = New System.Windows.Forms.Label
            Me.txtcono = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label8 = New System.Windows.Forms.Label
            Me.lblLine2 = New System.Windows.Forms.Label
            Me.txtPlaca = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label6 = New System.Windows.Forms.Label
            Me.lblLine6 = New System.Windows.Forms.Label
            Me.cboEstado = New SCGComboBox.SCGComboBox
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.txtNoOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.lblNoOrden = New System.Windows.Forms.Label
            Me.grpOrdenInfo = New System.Windows.Forms.GroupBox
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
            Me.Panel4 = New System.Windows.Forms.Panel
            Me.dtpAperturafin = New System.Windows.Forms.DateTimePicker
            Me.Panel2 = New System.Windows.Forms.Panel
            Me.Panel9 = New System.Windows.Forms.Panel
            Me.lblLine9 = New System.Windows.Forms.Label
            Me.Panel10 = New System.Windows.Forms.Panel
            Me.Label2 = New System.Windows.Forms.Label
            Me.chkCompromiso = New System.Windows.Forms.CheckBox
            Me.chkCierre = New System.Windows.Forms.CheckBox
            Me.chkApertura = New System.Windows.Forms.CheckBox
            Me.cboModelo = New SCGComboBox.SCGComboBox
            Me.lblLine5 = New System.Windows.Forms.Label
            Me.chkModelo = New System.Windows.Forms.CheckBox
            Me.chkEstado = New System.Windows.Forms.CheckBox
            Me.chkEstilo = New System.Windows.Forms.CheckBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.txtVehiculo = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.lblNoVehiculo = New System.Windows.Forms.Label
            Me.SubBuscador = New Buscador.SubBuscador
            Me.chkMarca = New System.Windows.Forms.CheckBox
            Me.ScgToolBar1 = New Proyecto_SCGToolBar.SCGToolBar
            Me.grpCitas.SuspendLayout()
            CType(Me.dtgOrdenes, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpOrdenInfo.SuspendLayout()
            Me.SuspendLayout()
            '
            'grpCitas
            '
            resources.ApplyResources(Me.grpCitas, "grpCitas")
            Me.grpCitas.BackColor = System.Drawing.SystemColors.Control
            Me.grpCitas.Controls.Add(Me.txtObservacion)
            Me.grpCitas.Controls.Add(Me.dtgOrdenes)
            Me.grpCitas.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.grpCitas.Name = "grpCitas"
            Me.grpCitas.TabStop = False
            '
            'txtObservacion
            '
            Me.txtObservacion.AceptaNegativos = False
            resources.ApplyResources(Me.txtObservacion, "txtObservacion")
            Me.txtObservacion.BackColor = System.Drawing.Color.White
            Me.txtObservacion.EstiloSBO = True
            Me.txtObservacion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.txtObservacion.MaxDecimales = 0
            Me.txtObservacion.MaxEnteros = 0
            Me.txtObservacion.Millares = False
            Me.txtObservacion.Name = "txtObservacion"
            Me.txtObservacion.Size_AdjustableHeight = 45
            Me.txtObservacion.TeclasDeshacer = True
            Me.txtObservacion.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'dtgOrdenes
            '
            resources.ApplyResources(Me.dtgOrdenes, "dtgOrdenes")
            Me.dtgOrdenes.BackgroundColor = System.Drawing.Color.White
            Me.dtgOrdenes.CaptionVisible = False
            Me.dtgOrdenes.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgOrdenes.HeaderBackColor = System.Drawing.Color.White
            Me.dtgOrdenes.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.dtgOrdenes.HeaderForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgOrdenes.Name = "dtgOrdenes"
            '
            'lblLine8
            '
            resources.ApplyResources(Me.lblLine8, "lblLine8")
            Me.lblLine8.BackColor = System.Drawing.Color.White
            Me.lblLine8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine8.Name = "lblLine8"
            '
            'cboEstilo
            '
            resources.ApplyResources(Me.cboEstilo, "cboEstilo")
            Me.cboEstilo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboEstilo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstilo.EstiloSBO = True
            Me.cboEstilo.Name = "cboEstilo"
            '
            'lblLine3
            '
            Me.lblLine3.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLine3, "lblLine3")
            Me.lblLine3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine3.Name = "lblLine3"
            '
            'lblNoexpediente
            '
            resources.ApplyResources(Me.lblNoexpediente, "lblNoexpediente")
            Me.lblNoexpediente.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoexpediente.Name = "lblNoexpediente"
            '
            'txtVisita
            '
            Me.txtVisita.AceptaNegativos = False
            Me.txtVisita.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtVisita.EstiloSBO = True
            resources.ApplyResources(Me.txtVisita, "txtVisita")
            Me.txtVisita.MaxDecimales = 0
            Me.txtVisita.MaxEnteros = 0
            Me.txtVisita.Millares = False
            Me.txtVisita.Name = "txtVisita"
            Me.txtVisita.Size_AdjustableHeight = 20
            Me.txtVisita.TeclasDeshacer = True
            Me.txtVisita.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.Numeric
            '
            'lblLine7
            '
            resources.ApplyResources(Me.lblLine7, "lblLine7")
            Me.lblLine7.BackColor = System.Drawing.Color.White
            Me.lblLine7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine7.Name = "lblLine7"
            '
            'cboMarca
            '
            resources.ApplyResources(Me.cboMarca, "cboMarca")
            Me.cboMarca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboMarca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboMarca.EstiloSBO = True
            Me.cboMarca.Name = "cboMarca"
            '
            'lblLine4
            '
            Me.lblLine4.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLine4, "lblLine4")
            Me.lblLine4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine4.Name = "lblLine4"
            '
            'txtcono
            '
            Me.txtcono.AceptaNegativos = False
            Me.txtcono.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtcono.EstiloSBO = True
            resources.ApplyResources(Me.txtcono, "txtcono")
            Me.txtcono.MaxDecimales = 0
            Me.txtcono.MaxEnteros = 0
            Me.txtcono.Millares = False
            Me.txtcono.Name = "txtcono"
            Me.txtcono.Size_AdjustableHeight = 20
            Me.txtcono.TeclasDeshacer = True
            Me.txtcono.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label8
            '
            resources.ApplyResources(Me.Label8, "Label8")
            Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label8.Name = "Label8"
            '
            'lblLine2
            '
            Me.lblLine2.BackColor = System.Drawing.Color.White
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
            'lblLine6
            '
            resources.ApplyResources(Me.lblLine6, "lblLine6")
            Me.lblLine6.BackColor = System.Drawing.Color.White
            Me.lblLine6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine6.Name = "lblLine6"
            '
            'cboEstado
            '
            resources.ApplyResources(Me.cboEstado, "cboEstado")
            Me.cboEstado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstado.EstiloSBO = True
            Me.cboEstado.Name = "cboEstado"
            '
            'lblLine1
            '
            Me.lblLine1.BackColor = System.Drawing.Color.White
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
            'grpOrdenInfo
            '
            resources.ApplyResources(Me.grpOrdenInfo, "grpOrdenInfo")
            Me.grpOrdenInfo.BackColor = System.Drawing.SystemColors.Control
            Me.grpOrdenInfo.Controls.Add(Me.Panel5)
            Me.grpOrdenInfo.Controls.Add(Me.Panel6)
            Me.grpOrdenInfo.Controls.Add(Me.Panel7)
            Me.grpOrdenInfo.Controls.Add(Me.dtpCierreini)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine10)
            Me.grpOrdenInfo.Controls.Add(Me.dtpCompromisoini)
            Me.grpOrdenInfo.Controls.Add(Me.dtpAperturaini)
            Me.grpOrdenInfo.Controls.Add(Me.Panel1)
            Me.grpOrdenInfo.Controls.Add(Me.dtpCierrefin)
            Me.grpOrdenInfo.Controls.Add(Me.Panel3)
            Me.grpOrdenInfo.Controls.Add(Me.dtpCompromisofin)
            Me.grpOrdenInfo.Controls.Add(Me.Panel4)
            Me.grpOrdenInfo.Controls.Add(Me.dtpAperturafin)
            Me.grpOrdenInfo.Controls.Add(Me.Panel2)
            Me.grpOrdenInfo.Controls.Add(Me.Panel9)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine9)
            Me.grpOrdenInfo.Controls.Add(Me.Panel10)
            Me.grpOrdenInfo.Controls.Add(Me.Label2)
            Me.grpOrdenInfo.Controls.Add(Me.chkCompromiso)
            Me.grpOrdenInfo.Controls.Add(Me.chkCierre)
            Me.grpOrdenInfo.Controls.Add(Me.chkApertura)
            Me.grpOrdenInfo.Controls.Add(Me.txtVisita)
            Me.grpOrdenInfo.Controls.Add(Me.cboEstado)
            Me.grpOrdenInfo.Controls.Add(Me.cboMarca)
            Me.grpOrdenInfo.Controls.Add(Me.cboModelo)
            Me.grpOrdenInfo.Controls.Add(Me.cboEstilo)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine5)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine8)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine7)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine6)
            Me.grpOrdenInfo.Controls.Add(Me.chkModelo)
            Me.grpOrdenInfo.Controls.Add(Me.chkEstado)
            Me.grpOrdenInfo.Controls.Add(Me.chkEstilo)
            Me.grpOrdenInfo.Controls.Add(Me.Label1)
            Me.grpOrdenInfo.Controls.Add(Me.txtVehiculo)
            Me.grpOrdenInfo.Controls.Add(Me.lblNoVehiculo)
            Me.grpOrdenInfo.Controls.Add(Me.SubBuscador)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine3)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine4)
            Me.grpOrdenInfo.Controls.Add(Me.txtcono)
            Me.grpOrdenInfo.Controls.Add(Me.Label8)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine2)
            Me.grpOrdenInfo.Controls.Add(Me.txtPlaca)
            Me.grpOrdenInfo.Controls.Add(Me.Label6)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine1)
            Me.grpOrdenInfo.Controls.Add(Me.txtNoOrden)
            Me.grpOrdenInfo.Controls.Add(Me.lblNoOrden)
            Me.grpOrdenInfo.Controls.Add(Me.lblNoexpediente)
            Me.grpOrdenInfo.Controls.Add(Me.chkMarca)
            Me.grpOrdenInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.grpOrdenInfo.Name = "grpOrdenInfo"
            Me.grpOrdenInfo.TabStop = False
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
            resources.ApplyResources(Me.dtpCierreini, "dtpCierreini")
            Me.dtpCierreini.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCierreini.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCierreini.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCierreini.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCierreini.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCierreini.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpCierreini.Name = "dtpCierreini"
            Me.dtpCierreini.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'lblLine10
            '
            resources.ApplyResources(Me.lblLine10, "lblLine10")
            Me.lblLine10.BackColor = System.Drawing.Color.White
            Me.lblLine10.Name = "lblLine10"
            '
            'dtpCompromisoini
            '
            resources.ApplyResources(Me.dtpCompromisoini, "dtpCompromisoini")
            Me.dtpCompromisoini.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCompromisoini.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCompromisoini.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCompromisoini.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCompromisoini.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCompromisoini.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpCompromisoini.Name = "dtpCompromisoini"
            Me.dtpCompromisoini.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'dtpAperturaini
            '
            resources.ApplyResources(Me.dtpAperturaini, "dtpAperturaini")
            Me.dtpAperturaini.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpAperturaini.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpAperturaini.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpAperturaini.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpAperturaini.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
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
            resources.ApplyResources(Me.dtpCierrefin, "dtpCierrefin")
            Me.dtpCierrefin.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCierrefin.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCierrefin.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCierrefin.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCierrefin.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
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
            resources.ApplyResources(Me.dtpCompromisofin, "dtpCompromisofin")
            Me.dtpCompromisofin.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCompromisofin.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCompromisofin.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpCompromisofin.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpCompromisofin.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
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
            resources.ApplyResources(Me.dtpAperturafin, "dtpAperturafin")
            Me.dtpAperturafin.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpAperturafin.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpAperturafin.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpAperturafin.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpAperturafin.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpAperturafin.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpAperturafin.MaxDate = New Date(3000, 12, 31, 0, 0, 0, 0)
            Me.dtpAperturafin.Name = "dtpAperturafin"
            Me.dtpAperturafin.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'Panel2
            '
            resources.ApplyResources(Me.Panel2, "Panel2")
            Me.Panel2.Name = "Panel2"
            '
            'Panel9
            '
            resources.ApplyResources(Me.Panel9, "Panel9")
            Me.Panel9.Name = "Panel9"
            '
            'lblLine9
            '
            resources.ApplyResources(Me.lblLine9, "lblLine9")
            Me.lblLine9.BackColor = System.Drawing.Color.White
            Me.lblLine9.Name = "lblLine9"
            '
            'Panel10
            '
            resources.ApplyResources(Me.Panel10, "Panel10")
            Me.Panel10.Name = "Panel10"
            '
            'Label2
            '
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.BackColor = System.Drawing.Color.White
            Me.Label2.Name = "Label2"
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
            'cboModelo
            '
            resources.ApplyResources(Me.cboModelo, "cboModelo")
            Me.cboModelo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboModelo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboModelo.EstiloSBO = True
            Me.cboModelo.Name = "cboModelo"
            '
            'lblLine5
            '
            resources.ApplyResources(Me.lblLine5, "lblLine5")
            Me.lblLine5.BackColor = System.Drawing.Color.White
            Me.lblLine5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine5.Name = "lblLine5"
            '
            'chkModelo
            '
            resources.ApplyResources(Me.chkModelo, "chkModelo")
            Me.chkModelo.Name = "chkModelo"
            Me.chkModelo.UseVisualStyleBackColor = True
            '
            'chkEstado
            '
            resources.ApplyResources(Me.chkEstado, "chkEstado")
            Me.chkEstado.Name = "chkEstado"
            Me.chkEstado.UseVisualStyleBackColor = True
            '
            'chkEstilo
            '
            resources.ApplyResources(Me.chkEstilo, "chkEstilo")
            Me.chkEstilo.Name = "chkEstilo"
            Me.chkEstilo.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label1.Name = "Label1"
            '
            'txtVehiculo
            '
            Me.txtVehiculo.AceptaNegativos = False
            Me.txtVehiculo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtVehiculo.EstiloSBO = True
            resources.ApplyResources(Me.txtVehiculo, "txtVehiculo")
            Me.txtVehiculo.MaxDecimales = 0
            Me.txtVehiculo.MaxEnteros = 0
            Me.txtVehiculo.Millares = False
            Me.txtVehiculo.Name = "txtVehiculo"
            Me.txtVehiculo.Size_AdjustableHeight = 20
            Me.txtVehiculo.TeclasDeshacer = True
            Me.txtVehiculo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblNoVehiculo
            '
            resources.ApplyResources(Me.lblNoVehiculo, "lblNoVehiculo")
            Me.lblNoVehiculo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoVehiculo.Name = "lblNoVehiculo"
            '
            'SubBuscador
            '
            Me.SubBuscador.BackColor = System.Drawing.Color.Black
            Me.SubBuscador.Barra_Titulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador.ConsultarDBPorFiltrado = False
            Me.SubBuscador.Criterios = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador.Criterios_Ocultos = 0
            Me.SubBuscador.Criterios_OcultosEx = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador.IN_DataTable = Nothing
            resources.ApplyResources(Me.SubBuscador, "SubBuscador")
            Me.SubBuscador.MultiSeleccion = False
            Me.SubBuscador.Name = "SubBuscador"
            Me.SubBuscador.SQL_Cnn = Nothing
            Me.SubBuscador.Tabla = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador.Titulos = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador.Where = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'chkMarca
            '
            resources.ApplyResources(Me.chkMarca, "chkMarca")
            Me.chkMarca.Name = "chkMarca"
            Me.chkMarca.UseVisualStyleBackColor = True
            '
            'ScgToolBar1
            '
            resources.ApplyResources(Me.ScgToolBar1, "ScgToolBar1")
            Me.ScgToolBar1.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.ScgToolBar1.Name = "ScgToolBar1"
            '
            'frmOrdenes
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.grpOrdenInfo)
            Me.Controls.Add(Me.grpCitas)
            Me.Controls.Add(Me.ScgToolBar1)
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.KeyPreview = True
            Me.Name = "frmOrdenes"
            Me.Tag = "Operaciones y Producción,1"
            Me.grpCitas.ResumeLayout(False)
            Me.grpCitas.PerformLayout()
            CType(Me.dtgOrdenes, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpOrdenInfo.ResumeLayout(False)
            Me.grpOrdenInfo.PerformLayout()
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
        Private m_adpOrdenTrabajo As SCGDataAccess.OrdenTrabajoDataAdapter
        Public m_dstOrdenTrabajo As OrdenTrabajoDataset

        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strCodMarca As String = "CodMarca"
        Private Const mc_strDescMarca As String = "DescMarca"
        Private Const mc_strPlaca As String = "Placa"
        Private Const mc_strNoVisita As String = "NoVisita"
        Private Const mc_strCono As String = "Cono"
        Private Const mc_datFechacompromiso As String = "Fecha_compromiso"
        Private Const mc_datFechaCierre As String = "Fecha_Cierre"
        Private Const mc_datFechaApertura As String = "Fecha_Apertura"
        Private Const mc_strNoVehiculo As String = "NoVehiculo"
        Private Const mc_strCodEstilo As String = "CodEstilo"
        Private Const mc_strDescEstilo As String = "DescEstilo"
        Private Const mc_strEstado As String = "Estado"
        Private Const mc_strEstadoDesc As String = "EstadoDesc"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"
        Private Const mc_strCodigoCliente As String = "ClienteFacturar"
        Private Const mc_strNombreCliente As String = "CardName"
        Private Const mc_strDescripcionTipoOrden As String = "TipoDesc"
        Private Const mc_strDescripcionUbicacion As String = "DescUbicacion"


        Private Const mc_strDescripcionEstado As String = "DescipcionEstado"

        'Nombre de la constante de la tabla
        Private mc_strTableName As String = "SCGTA_TB_Orden"

        'Tipo de inserción si es una actualización en la base de datos o una inserción.
        Private intTipoInsercion As Integer

        'Declaracion de un row del dataset, el cual sirve para insertar como para modificar y eliminar.
        Private drwOrden As OrdenTrabajoDataset.SCGTA_TB_OrdenRow

        Private objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        'Variables para la búsqueda
        Private m_strNoOrden As String
        Private m_strNoVehiculo As String
        Private m_strPlaca As String
        Private m_intNoVisita As Integer
        Private m_strCono As String
        Private m_strCodEstado As String
        Private m_strCodMarca As String
        Private m_strCodEstilo As String
        Private m_strCodModelo As String

        Private m_dtFechaperturaini As Date
        Private m_dtFechacierreini As Date
        Private m_dtFechacompromisoini As Date
        Private m_dtFechaperturaFin As Date
        Private m_dtFechacierreFin As Date
        Private m_dtFechacompromisoFin As Date

        Private m_strsimbolocierre As String
        Private m_strsimbolocompromiso As String
        Private m_strsimboloapertura As String

        Private WithEvents objfrmOpenOrden As frmOrden

#End Region

#Region "Eventos Internos"

#Region "Eventos keypress de los criterios de busqueda"


        Private Sub cboEstado_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboEstado.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub cboFases_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub cboMarca_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboMarca.KeyPress, cboModelo.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub cboprioridad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboEstilo.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub txtexpediente_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtVisita.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                    e.Handled = True
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub txtNoOrden_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNoOrden.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                    e.Handled = True
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub txtcono_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtcono.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                    e.Handled = True
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub txtplaca_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPlaca.KeyPress, txtVehiculo.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                    e.Handled = True
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub cboDecApertura_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub cbocompromiso_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub cbocierre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtpApertura_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtpCierre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtpCompromiso_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub txtVehiculo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtVehiculo.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Enter Then
                    busquedaOrden()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

#End Region

        Private Sub frmOrdenes_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                txtObservacion.ReadOnly = True
                'Para que tome fecha del servidor
                dtpAperturaini.Value = objUtilitarios.CargarFechaHoraServidor.Date
                dtpCompromisoini.Value = dtpAperturaini.Value
                dtpCompromisofin.Value = dtpAperturaini.Value
                dtpAperturafin.Value = dtpAperturaini.Value
                dtpCierreini.Value = dtpAperturaini.Value
                dtpCierrefin.Value = dtpAperturaini.Value

                cargarOrden()

                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Visible = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Visible = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Visible = False

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub ScgToolBar1_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Cerrar
            Try
                Me.Close()
                Me.Dispose()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtgOrdenes_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgOrdenes.DoubleClick

            Dim Forma_Nueva As Form

            Dim blnExisteForm As Boolean
            Try

                If dtgOrdenes.CurrentRowIndex <> -1 Then


                    For Each Forma_Nueva In Me.MdiParent.MdiChildren
                        If Forma_Nueva.Name = "frmOrden" Then
                            blnExisteForm = True
                        End If
                    Next

                    If Not blnExisteForm Then
                        objfrmOpenOrden = New frmOrden(m_dstOrdenTrabajo, CStr(dtgOrdenes.Item(dtgOrdenes.CurrentRowIndex, 0)))

                        objfrmOpenOrden.MdiParent = Me.MdiParent
                        objfrmOpenOrden.Show()
                    End If

                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub dtgOrdenes_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgOrdenes.CurrentCellChanged
            Try
                Me.Cursor = Cursors.WaitCursor

                'llama a la función que  cambia la observación segun sea la celda seleccionada
                Call MostrarObservacion()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
                Me.Cursor = Cursors.Arrow
            End Try
        End Sub

        Private Sub ScgToolBar1_Click_Buscar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Buscar
            Try
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = False
                busquedaOrden()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub frmOrdenes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
            If Asc(e.KeyChar) = Keys.Escape Then Me.Close()
        End Sub

        Private Sub chks_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEstado.CheckedChanged, chkEstilo.CheckedChanged, chkMarca.CheckedChanged, chkModelo.CheckedChanged

            Try

                CheckOptions(CType(sender, CheckBox))

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub cboMarcaEstiloModelo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMarca.SelectedIndexChanged, cboEstilo.SelectedIndexChanged, cboModelo.SelectedIndexChanged

            Try
                Me.Cursor = Cursors.WaitCursor

                CambiaCombos(CType(sender, SCGComboBox.SCGComboBox))

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally

                Me.Cursor = Cursors.Arrow

            End Try

        End Sub

        Private Sub frmOrdenes_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged

            Dim objSize As Size

            If Me.WindowState = FormWindowState.Maximized Then
                Me.WindowState = FormWindowState.Normal
                Me.Dock = DockStyle.Fill
                objSize = Me.Size
                Me.Dock = DockStyle.None
                Me.Top = 0
                Me.Left = 0
                Me.Size = objSize
            End If

        End Sub

#End Region

#Region "Métodos"

        Private Sub limpiarCriteriosBusqueda()
            Try
                Me.cboMarca.Text = ""
                txtcono.Clear()
                txtVisita.Clear()
                txtNoOrden.Clear()
                txtPlaca.Clear()
                cboEstilo.Text = ""
                cboEstado.Text = ""
                cboModelo.Text = ""
                txtVehiculo.Text = ""
                dtpAperturafin.Value = Date.Now
                dtpAperturaini.Value = Date.Now
                dtpCierrefin.Value = Date.Now
                dtpCierreini.Value = Date.Now
                dtpCompromisofin.Value = Date.Now
                dtpCompromisoini.Value = Date.Now
                chkApertura.Checked = False
                chkCierre.Checked = False
                chkCompromiso.Checked = False
                Me.txtObservacion.Text = ""
                chkMarca.Checked = False
                chkEstado.Checked = False

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub busquedaOrden()

            Try

                m_adpOrdenTrabajo = New SCGDataAccess.OrdenTrabajoDataAdapter

                m_dstOrdenTrabajo.Dispose()

                m_dstOrdenTrabajo = Nothing

                m_dstOrdenTrabajo = New OrdenTrabajoDataset

                m_strNoOrden = txtNoOrden.Text
                m_strNoVehiculo = txtVehiculo.Text
                m_strPlaca = txtPlaca.Text
                m_intNoVisita = IIf(IsNumeric(txtVisita.Text), txtVisita.Text, 0)
                m_strCono = txtcono.Text

                If cboEstado.SelectedIndex = -1 Then
                    m_strCodEstado = ""
                Else
                    m_strCodEstado = cboEstado.SelectedIndex + 1
                End If

                If cboMarca.SelectedValue Is Nothing Then
                    m_strCodMarca = ""
                Else
                    m_strCodMarca = cboMarca.SelectedValue
                End If

                If cboEstilo.SelectedValue Is Nothing Then
                    m_strCodEstilo = ""
                Else
                    m_strCodEstilo = cboEstilo.SelectedValue
                End If

                If cboModelo.SelectedValue Is Nothing Then
                    m_strCodModelo = ""
                Else
                    m_strCodModelo = cboModelo.SelectedValue
                End If
                If chkApertura.Checked Then
                    m_dtFechaperturaini = New Date(dtpAperturaini.Value.Year, dtpAperturaini.Value.Month, dtpAperturaini.Value.Day, 0, 0, 0)
                    m_dtFechaperturaFin = New Date(dtpAperturafin.Value.Year, dtpAperturafin.Value.Month, dtpAperturafin.Value.Day, 23, 59, 59)
                Else
                    m_dtFechaperturaini = Nothing
                    m_dtFechaperturaFin = Nothing
                End If
                If chkCierre.Checked Then

                    m_dtFechacierreini = New Date(dtpCierreini.Value.Year, dtpCierreini.Value.Month, dtpCierreini.Value.Day, 0, 0, 0)
                    m_dtFechacierreFin = New Date(dtpCierrefin.Value.Year, dtpCierrefin.Value.Month, dtpCierrefin.Value.Day, 23, 59, 59)
                Else
                    m_dtFechacierreini = Nothing
                    m_dtFechacierreFin = Nothing
                End If

                If chkCompromiso.Checked Then
                    m_dtFechacompromisoini = New Date(dtpCompromisoini.Value.Year, dtpCompromisoini.Value.Month, dtpCompromisoini.Value.Day, 0, 0, 0)
                    m_dtFechacompromisoFin = New Date(dtpCompromisofin.Value.Year, dtpCompromisofin.Value.Month, dtpCompromisofin.Value.Day, 23, 59, 59)
                Else
                    m_dtFechacompromisoini = Nothing
                    m_dtFechacompromisoFin = Nothing
                End If
                estiloGrid()

                Call m_adpOrdenTrabajo.Fill(m_dstOrdenTrabajo, m_strNoOrden, m_strNoVehiculo, m_strPlaca, m_intNoVisita, _
                m_strCono, m_strCodEstado, m_strCodMarca, m_strCodEstilo, m_strCodModelo, m_dtFechaperturaini, m_dtFechacompromisoini, m_dtFechacierreini, _
                m_dtFechaperturaFin, m_dtFechacompromisoFin, m_dtFechacierreFin)

                LlenarEstadoOrdenTrabajoResources(m_dstOrdenTrabajo)


                'Se valida que no se pueda eliminar, editar y agregar en el dataset (datagrid)
                With m_dstOrdenTrabajo.SCGTA_TB_Orden.DefaultView
                    .AllowDelete = False
                    .AllowEdit = False
                    .AllowNew = False
                End With

                txtObservacion.Text = ""

                dtgOrdenes.DataSource = m_dstOrdenTrabajo.SCGTA_TB_Orden

                'limpiarCriteriosBusqueda()

                'm_strPlaca = ""
                'm_strEstado = ""
                'm_strprioridad = ""
                'm_strsimboloapertura = ""
                'm_strsimbolocierre = ""
                'm_strsimbolocompromiso = ""
                'm_intcono = Nothing
                'm_intCodMarca = Nothing
                'm_intexpediente = Nothing
                'm_intNoOrden = ""
                'm_intFaseActual = Nothing

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub


        Private Sub estiloGrid()

            'Declaraciones generales
            Dim tsConfiguracion As New DataGridTableStyle
            dtgOrdenes.TableStyles.Clear()

            Dim tcNoOrden As New DataGridLabelColumn
            Dim tcNoVehi As New DataGridLabelColumn
            Dim tcCono As New DataGridLabelColumn
            Dim tcPlaca As New DataGridLabelColumn
            Dim tcEstado As New DataGridLabelColumn
            Dim tcMarca As New DataGridLabelColumn
            Dim tcEstilo As New DataGridLabelColumn
            Dim tcApertura As New DataGridLabelColumn
            Dim tcCompromiso As New DataGridLabelColumn
            Dim tcCierre As New DataGridLabelColumn
            Dim tcDescripEstado As New DataGridLabelColumn
            Dim tcCodigoCliente As New DataGridLabelColumn
            Dim tcNombreCliente As New DataGridLabelColumn
            Dim tcDescripcionTipoOrden As New DataGridLabelColumn
            Dim tcDescripcionUbicacion As New DataGridLabelColumn




            tsConfiguracion.MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.TableName()

            Try

                With tcNoOrden
                    .Width = 70
                    .HeaderText = My.Resources.ResourceUI.NoOrden
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Columns(mc_strNoOrden).ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With

                With tcNoVehi
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.NoUnidad
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Columns(mc_strNoVehiculo).ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With

                With tcCono
                    .Width = 40
                    .HeaderText = My.Resources.ResourceUI.Cono
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Columns(mc_strCono).ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With

                With tcPlaca
                    .Width = 80
                    .HeaderText = My.Resources.ResourceUI.Placa
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Columns(mc_strPlaca).ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With

                With tcEstado
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.Estado
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Columns(mc_strEstadoDesc).ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With

                '----------------
                With tcCodigoCliente
                    .Width = 80
                    .HeaderText = My.Resources.ResourceUI.CodCliente
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Columns(mc_strCodigoCliente).ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With

                With tcNombreCliente
                    .Width = 150
                    .HeaderText = My.Resources.ResourceUI.NombreCliente
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Columns(mc_strNombreCliente).ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With

                With tcDescripcionTipoOrden
                    .Width = 150
                    .HeaderText = My.Resources.ResourceUI.DescripcionTipoOT
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Columns(mc_strDescripcionTipoOrden).ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With

                With tcDescripcionUbicacion
                    .Width = 150
                    .HeaderText = My.Resources.ResourceUI.DescripcionUbicacion
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Columns(mc_strDescripcionUbicacion).ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With
                '----------------

                With tcDescripEstado
                    .Width = 80
                    .HeaderText = My.Resources.ResourceUI.Estado
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Columns(mc_strDescripcionEstado).ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With

                With tcMarca
                    .Width = 96
                    .HeaderText = My.Resources.ResourceUI.Marca
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Columns(mc_strDescMarca).ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With

                With tcEstilo
                    .Width = 96
                    .HeaderText = My.Resources.ResourceUI.Estilo
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Columns(mc_strDescEstilo).ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With

                With tcApertura
                    .Width = 160
                    .HeaderText = My.Resources.ResourceUI.FechaApertura
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Columns(mc_datFechaApertura).ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With

                With tcCompromiso
                    .Width = 160
                    .HeaderText = My.Resources.ResourceUI.FechaCompromiso
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Fecha_CompColumn.ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With


                With tcCierre
                    .Width = 160
                    .HeaderText = My.Resources.ResourceUI.FechaCierre
                    .MappingName = m_dstOrdenTrabajo.SCGTA_TB_Orden.Fecha_cierreColumn.ColumnName
                    .NullText = "- - -"
                    .ReadOnly = True
                End With

                If g_blnCampoVisible Then
                    'Agrega las columnas al tableStyle
                    tsConfiguracion.GridColumnStyles.Add(tcNoOrden)
                    tsConfiguracion.GridColumnStyles.Add(tcDescripcionTipoOrden)
                    tsConfiguracion.GridColumnStyles.Add(tcNoVehi)
                    tsConfiguracion.GridColumnStyles.Add(tcDescripcionUbicacion)
                    tsConfiguracion.GridColumnStyles.Add(tcCono)
                    tsConfiguracion.GridColumnStyles.Add(tcPlaca)
                    tsConfiguracion.GridColumnStyles.Add(tcEstado)
                    tsConfiguracion.GridColumnStyles.Add(tcDescripEstado)
                    tsConfiguracion.GridColumnStyles.Add(tcCodigoCliente)
                    tsConfiguracion.GridColumnStyles.Add(tcNombreCliente)
                    tsConfiguracion.GridColumnStyles.Add(tcMarca)
                    tsConfiguracion.GridColumnStyles.Add(tcEstilo)
                    tsConfiguracion.GridColumnStyles.Add(tcApertura)
                    tsConfiguracion.GridColumnStyles.Add(tcCompromiso)
                    tsConfiguracion.GridColumnStyles.Add(tcCierre)

                Else
                    'Agrega las columnas al tableStyle
                    tsConfiguracion.GridColumnStyles.Add(tcNoOrden)
                    tsConfiguracion.GridColumnStyles.Add(tcDescripcionTipoOrden)
                    tsConfiguracion.GridColumnStyles.Add(tcNoVehi)
                    tsConfiguracion.GridColumnStyles.Add(tcCono)
                    tsConfiguracion.GridColumnStyles.Add(tcPlaca)
                    tsConfiguracion.GridColumnStyles.Add(tcEstado)
                    tsConfiguracion.GridColumnStyles.Add(tcDescripEstado)
                    tsConfiguracion.GridColumnStyles.Add(tcCodigoCliente)
                    tsConfiguracion.GridColumnStyles.Add(tcNombreCliente)
                    tsConfiguracion.GridColumnStyles.Add(tcMarca)
                    tsConfiguracion.GridColumnStyles.Add(tcEstilo)
                    tsConfiguracion.GridColumnStyles.Add(tcDescripcionUbicacion)
                    tsConfiguracion.GridColumnStyles.Add(tcApertura)
                    tsConfiguracion.GridColumnStyles.Add(tcCompromiso)
                    tsConfiguracion.GridColumnStyles.Add(tcCierre)
                End If




                'Establece propiedades del datagrid (colores estándares).
                tsConfiguracion.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
                tsConfiguracion.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                tsConfiguracion.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
                tsConfiguracion.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))

                'Hace que el datagrid adopte las propiedades del TableStyle.
                dtgOrdenes.TableStyles.Add(tsConfiguracion)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
                'clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub MostrarObservacion()
            Dim strNoOrden As String

            Try

                Me.txtObservacion.Clear()

                'Se valida que almenos exista un valor en el datagrid (o sino se cae al seleccionar)
                If dtgOrdenes.CurrentRowIndex <> -1 Then


                    'Se asignan los codigos correspondientes a las variables según la selección en el datagrid
                    strNoOrden = dtgOrdenes.Item(dtgOrdenes.CurrentRowIndex, 0)

                    drwOrden = m_dstOrdenTrabajo.SCGTA_TB_Orden.FindByNoOrden(strNoOrden)

                    'Se habilita tanto la modificación como la eliminación del row.
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = True
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = True
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True

                    'Se valida que el campo observacion no sea un dbnull en caso de no ser null se carga en el txtObservacion.text
                    If Not drwOrden.IsObservacionNull Then
                        Me.txtObservacion.Text = drwOrden.Observacion
                    End If



                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = True

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Public Function Busca_Codigo_Texto(ByVal strTempItem As String, Optional ByVal blnGetCodigo As Boolean = True) As String

            Dim strCod_Item_Comp As String
            Dim strTemp As String
            Dim intCharCont As Integer
            Dim strTextoNoCodigo As String = ""
            Try
                strTemp = ""
                strCod_Item_Comp = ""

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
                Else
                    Return ""
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex 'clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Function

        Public Sub Busca_Item_Combo(ByRef Combo As ComboBox, ByVal Cod_Item As String)

            Dim intItemCont As Integer
            Dim strTempItem As String
            Dim strCod_Item_Comp As String
            Dim blnExiste As Boolean

            Try
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

        Private Sub cargarOrden()

            Try

                m_adpOrdenTrabajo = New SCGDataAccess.OrdenTrabajoDataAdapter

                m_dstOrdenTrabajo = New OrdenTrabajoDataset

                estiloGrid()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub ScgToolBar1_Click_Cancelar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Cancelar

            Try

                'Me.txtcono.Clear()
                'Me.txtVisita.Clear()
                'Me.txtNoOrden.Clear()
                'Me.txtObservacion.Clear()
                'Me.txtPlaca.Clear()
                'Me.cboMarca.Text = ""
                'Me.cboModelo.Text = ""
                'Me.cboEstado.Text = ""
                'Me.cboEstilo.Text = ""
                'Limpiar Grid
                Call limpiarCriteriosBusqueda()
                dtgOrdenes.DataSource = Nothing

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub CheckOptions(ByRef p_chkSender As CheckBox)

            Select Case p_chkSender.Name

                Case chkEstado.Name

                    If chkEstado.Checked Then
                        cargarComboEstadoOrden(cboEstado, True)
                        cboEstado.SelectedIndex = 0

                    Else
                        'cboEstado.Items.Clear()
                        cboEstado.DataSource = Nothing
                    End If

                Case chkMarca.Name

                    If chkMarca.Checked Then
                        Utilitarios.CargarCombosMarcasVehiculos(cboMarca)
                    Else
                        chkModelo.Checked = False
                        chkEstilo.Checked = False
                        cboMarca.DataSource = Nothing
                    End If

                Case chkEstilo.Name

                    If chkEstilo.Checked Then
                        If chkMarca.Checked Then
                            Utilitarios.CargarComboEstilosVehiculos(cboEstilo, cboMarca.SelectedValue)
                        Else
                            chkEstilo.Checked = False
                        End If
                    Else
                        chkModelo.Checked = False
                        cboEstilo.DataSource = Nothing
                    End If

                Case chkModelo.Name

                    If chkModelo.Checked Then
                        If chkEstilo.Checked Then
                            Utilitarios.CargarComboModelosVehiculos(cboModelo, cboEstilo.SelectedValue)
                        Else
                            chkModelo.Checked = False
                        End If
                    Else
                        cboModelo.DataSource = Nothing
                    End If

            End Select

        End Sub

        Private Sub CambiaCombos(ByRef p_cboSelected As SCGComboBox.SCGComboBox)

            Select Case p_cboSelected.Name

                Case cboMarca.Name

                    If chkEstilo.Checked Then

                        Utilitarios.CargarComboEstilosVehiculos(cboEstilo, cboMarca.SelectedValue)

                    End If

                Case cboEstilo.Name

                    If chkModelo.Checked Then

                        Utilitarios.CargarComboModelosVehiculos(cboModelo, cboEstilo.SelectedValue)

                    End If

            End Select

        End Sub

#End Region

    End Class
End Namespace
