Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports System.Globalization

Namespace SCG_User_Interface
    Public Class frmCtrlInformacionClientes
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
        Friend WithEvents tlbClientes As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents txtDetalleCliente As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtFax As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtCorreo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtCodigo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtOficina As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtCliente As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblCliente As System.Windows.Forms.Label
        Friend WithEvents lblDetalle As System.Windows.Forms.Label
        Friend WithEvents lblCorreo As System.Windows.Forms.Label
        Friend WithEvents lblCasa As System.Windows.Forms.Label
        Friend WithEvents lblOficina As System.Windows.Forms.Label
        Friend WithEvents lblFax As System.Windows.Forms.Label
        Friend WithEvents lblCodigo As System.Windows.Forms.Label
        Friend WithEvents lblCelular As System.Windows.Forms.Label
        Friend WithEvents SubBuscador1 As Buscador.SubBuscador
        Friend WithEvents picCliente As System.Windows.Forms.PictureBox
        Friend WithEvents lblRFC As System.Windows.Forms.Label
        Friend WithEvents txtRFC As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtCasa As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtSitioWeb As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label11 As System.Windows.Forms.Label
        Friend WithEvents lblTipo As System.Windows.Forms.Label
        Friend WithEvents cboTipoSocio As SCGComboBox.SCGComboBox
        Friend WithEvents errClientes As System.Windows.Forms.ErrorProvider
        Friend WithEvents ContextMenu1 As System.Windows.Forms.ContextMenu
        Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
        Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
        Friend WithEvents Label13 As System.Windows.Forms.Label
        Friend WithEvents Label14 As System.Windows.Forms.Label
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents Label17 As System.Windows.Forms.Label
        Friend WithEvents Label16 As System.Windows.Forms.Label
        Friend WithEvents Label15 As System.Windows.Forms.Label
        Friend WithEvents Label19 As System.Windows.Forms.Label
        Friend WithEvents Label18 As System.Windows.Forms.Label
        Friend WithEvents Label21 As System.Windows.Forms.Label
        Friend WithEvents Label20 As System.Windows.Forms.Label
        Friend WithEvents txtCelular As NEWTEXTBOX.NEWTEXTBOX_CTRL
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCtrlInformacionClientes))
            Me.tlbClientes = New Proyecto_SCGToolBar.SCGToolBar()
            Me.txtDetalleCliente = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtFax = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtCorreo = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtCodigo = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtOficina = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtCasa = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtCelular = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtCliente = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblCliente = New System.Windows.Forms.Label()
            Me.lblDetalle = New System.Windows.Forms.Label()
            Me.lblCorreo = New System.Windows.Forms.Label()
            Me.lblCasa = New System.Windows.Forms.Label()
            Me.lblOficina = New System.Windows.Forms.Label()
            Me.lblFax = New System.Windows.Forms.Label()
            Me.lblCodigo = New System.Windows.Forms.Label()
            Me.lblCelular = New System.Windows.Forms.Label()
            Me.SubBuscador1 = New Buscador.SubBuscador()
            Me.picCliente = New System.Windows.Forms.PictureBox()
            Me.lblRFC = New System.Windows.Forms.Label()
            Me.txtRFC = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtSitioWeb = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label11 = New System.Windows.Forms.Label()
            Me.lblTipo = New System.Windows.Forms.Label()
            Me.cboTipoSocio = New SCGComboBox.SCGComboBox()
            Me.errClientes = New System.Windows.Forms.ErrorProvider(Me.components)
            Me.Label13 = New System.Windows.Forms.Label()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.Label14 = New System.Windows.Forms.Label()
            Me.Label15 = New System.Windows.Forms.Label()
            Me.Label16 = New System.Windows.Forms.Label()
            Me.Label17 = New System.Windows.Forms.Label()
            Me.Label18 = New System.Windows.Forms.Label()
            Me.Label19 = New System.Windows.Forms.Label()
            Me.Label20 = New System.Windows.Forms.Label()
            Me.Label21 = New System.Windows.Forms.Label()
            Me.ContextMenu1 = New System.Windows.Forms.ContextMenu()
            Me.MenuItem1 = New System.Windows.Forms.MenuItem()
            Me.MenuItem2 = New System.Windows.Forms.MenuItem()
            CType(Me.picCliente, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.errClientes, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'tlbClientes
            '
            resources.ApplyResources(Me.tlbClientes, "tlbClientes")
            Me.tlbClientes.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.tlbClientes.Name = "tlbClientes"
            '
            'txtDetalleCliente
            '
            resources.ApplyResources(Me.txtDetalleCliente, "txtDetalleCliente")
            Me.txtDetalleCliente.AceptaNegativos = False
            Me.txtDetalleCliente.BackColor = System.Drawing.Color.White
            Me.errClientes.SetError(Me.txtDetalleCliente, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtDetalleCliente.EstiloSBO = True
            Me.errClientes.SetIconAlignment(Me.txtDetalleCliente, CType(resources.GetObject("txtDetalleCliente.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.txtDetalleCliente, CType(resources.GetObject("txtDetalleCliente.IconPadding"), Integer))
            Me.txtDetalleCliente.MaxDecimales = 0
            Me.txtDetalleCliente.MaxEnteros = 0
            Me.txtDetalleCliente.Millares = False
            Me.txtDetalleCliente.Name = "txtDetalleCliente"
            Me.txtDetalleCliente.Size_AdjustableHeight = 30
            Me.txtDetalleCliente.TeclasDeshacer = True
            Me.txtDetalleCliente.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtFax
            '
            resources.ApplyResources(Me.txtFax, "txtFax")
            Me.txtFax.AceptaNegativos = False
            Me.txtFax.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.errClientes.SetError(Me.txtFax, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtFax.EstiloSBO = True
            Me.errClientes.SetIconAlignment(Me.txtFax, CType(resources.GetObject("txtFax.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.txtFax, CType(resources.GetObject("txtFax.IconPadding"), Integer))
            Me.txtFax.MaxDecimales = 0
            Me.txtFax.MaxEnteros = 0
            Me.txtFax.Millares = False
            Me.txtFax.Name = "txtFax"
            Me.txtFax.Size_AdjustableHeight = 20
            Me.txtFax.TeclasDeshacer = True
            Me.txtFax.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtCorreo
            '
            resources.ApplyResources(Me.txtCorreo, "txtCorreo")
            Me.txtCorreo.AceptaNegativos = False
            Me.txtCorreo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.errClientes.SetError(Me.txtCorreo, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtCorreo.EstiloSBO = True
            Me.errClientes.SetIconAlignment(Me.txtCorreo, CType(resources.GetObject("txtCorreo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.txtCorreo, CType(resources.GetObject("txtCorreo.IconPadding"), Integer))
            Me.txtCorreo.MaxDecimales = 0
            Me.txtCorreo.MaxEnteros = 0
            Me.txtCorreo.Millares = False
            Me.txtCorreo.Name = "txtCorreo"
            Me.txtCorreo.Size_AdjustableHeight = 20
            Me.txtCorreo.TeclasDeshacer = True
            Me.txtCorreo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtCodigo
            '
            resources.ApplyResources(Me.txtCodigo, "txtCodigo")
            Me.txtCodigo.AceptaNegativos = False
            Me.txtCodigo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.errClientes.SetError(Me.txtCodigo, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtCodigo.EstiloSBO = True
            Me.errClientes.SetIconAlignment(Me.txtCodigo, CType(resources.GetObject("txtCodigo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.txtCodigo, CType(resources.GetObject("txtCodigo.IconPadding"), Integer))
            Me.txtCodigo.MaxDecimales = 0
            Me.txtCodigo.MaxEnteros = 0
            Me.txtCodigo.Millares = False
            Me.txtCodigo.Name = "txtCodigo"
            Me.txtCodigo.Size_AdjustableHeight = 20
            Me.txtCodigo.TeclasDeshacer = True
            Me.txtCodigo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtOficina
            '
            resources.ApplyResources(Me.txtOficina, "txtOficina")
            Me.txtOficina.AceptaNegativos = False
            Me.txtOficina.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.errClientes.SetError(Me.txtOficina, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtOficina.EstiloSBO = True
            Me.errClientes.SetIconAlignment(Me.txtOficina, CType(resources.GetObject("txtOficina.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.txtOficina, CType(resources.GetObject("txtOficina.IconPadding"), Integer))
            Me.txtOficina.MaxDecimales = 0
            Me.txtOficina.MaxEnteros = 0
            Me.txtOficina.Millares = False
            Me.txtOficina.Name = "txtOficina"
            Me.txtOficina.Size_AdjustableHeight = 20
            Me.txtOficina.TeclasDeshacer = True
            Me.txtOficina.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtCasa
            '
            resources.ApplyResources(Me.txtCasa, "txtCasa")
            Me.txtCasa.AceptaNegativos = False
            Me.txtCasa.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.errClientes.SetError(Me.txtCasa, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtCasa.EstiloSBO = True
            Me.errClientes.SetIconAlignment(Me.txtCasa, CType(resources.GetObject("txtCasa.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.txtCasa, CType(resources.GetObject("txtCasa.IconPadding"), Integer))
            Me.txtCasa.MaxDecimales = 0
            Me.txtCasa.MaxEnteros = 0
            Me.txtCasa.Millares = False
            Me.txtCasa.Name = "txtCasa"
            Me.txtCasa.Size_AdjustableHeight = 20
            Me.txtCasa.TeclasDeshacer = True
            Me.txtCasa.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtCelular
            '
            resources.ApplyResources(Me.txtCelular, "txtCelular")
            Me.txtCelular.AceptaNegativos = False
            Me.txtCelular.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.errClientes.SetError(Me.txtCelular, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtCelular.EstiloSBO = True
            Me.errClientes.SetIconAlignment(Me.txtCelular, CType(resources.GetObject("txtCelular.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.txtCelular, CType(resources.GetObject("txtCelular.IconPadding"), Integer))
            Me.txtCelular.MaxDecimales = 0
            Me.txtCelular.MaxEnteros = 0
            Me.txtCelular.Millares = False
            Me.txtCelular.Name = "txtCelular"
            Me.txtCelular.Size_AdjustableHeight = 20
            Me.txtCelular.TeclasDeshacer = True
            Me.txtCelular.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtCliente
            '
            resources.ApplyResources(Me.txtCliente, "txtCliente")
            Me.txtCliente.AceptaNegativos = False
            Me.txtCliente.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.errClientes.SetError(Me.txtCliente, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtCliente.EstiloSBO = True
            Me.errClientes.SetIconAlignment(Me.txtCliente, CType(resources.GetObject("txtCliente.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.txtCliente, CType(resources.GetObject("txtCliente.IconPadding"), Integer))
            Me.txtCliente.MaxDecimales = 0
            Me.txtCliente.MaxEnteros = 0
            Me.txtCliente.Millares = False
            Me.txtCliente.Name = "txtCliente"
            Me.txtCliente.Size_AdjustableHeight = 20
            Me.txtCliente.TeclasDeshacer = True
            Me.txtCliente.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblCliente
            '
            resources.ApplyResources(Me.lblCliente, "lblCliente")
            Me.errClientes.SetError(Me.lblCliente, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.errClientes.SetIconAlignment(Me.lblCliente, CType(resources.GetObject("lblCliente.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.lblCliente, CType(resources.GetObject("lblCliente.IconPadding"), Integer))
            Me.lblCliente.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblCliente.Name = "lblCliente"
            '
            'lblDetalle
            '
            resources.ApplyResources(Me.lblDetalle, "lblDetalle")
            Me.errClientes.SetError(Me.lblDetalle, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.errClientes.SetIconAlignment(Me.lblDetalle, CType(resources.GetObject("lblDetalle.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.lblDetalle, CType(resources.GetObject("lblDetalle.IconPadding"), Integer))
            Me.lblDetalle.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblDetalle.Name = "lblDetalle"
            '
            'lblCorreo
            '
            resources.ApplyResources(Me.lblCorreo, "lblCorreo")
            Me.errClientes.SetError(Me.lblCorreo, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.errClientes.SetIconAlignment(Me.lblCorreo, CType(resources.GetObject("lblCorreo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.lblCorreo, CType(resources.GetObject("lblCorreo.IconPadding"), Integer))
            Me.lblCorreo.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblCorreo.Name = "lblCorreo"
            '
            'lblCasa
            '
            resources.ApplyResources(Me.lblCasa, "lblCasa")
            Me.errClientes.SetError(Me.lblCasa, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.errClientes.SetIconAlignment(Me.lblCasa, CType(resources.GetObject("lblCasa.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.lblCasa, CType(resources.GetObject("lblCasa.IconPadding"), Integer))
            Me.lblCasa.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblCasa.Name = "lblCasa"
            '
            'lblOficina
            '
            resources.ApplyResources(Me.lblOficina, "lblOficina")
            Me.errClientes.SetError(Me.lblOficina, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.errClientes.SetIconAlignment(Me.lblOficina, CType(resources.GetObject("lblOficina.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.lblOficina, CType(resources.GetObject("lblOficina.IconPadding"), Integer))
            Me.lblOficina.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblOficina.Name = "lblOficina"
            '
            'lblFax
            '
            resources.ApplyResources(Me.lblFax, "lblFax")
            Me.errClientes.SetError(Me.lblFax, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.errClientes.SetIconAlignment(Me.lblFax, CType(resources.GetObject("lblFax.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.lblFax, CType(resources.GetObject("lblFax.IconPadding"), Integer))
            Me.lblFax.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblFax.Name = "lblFax"
            '
            'lblCodigo
            '
            resources.ApplyResources(Me.lblCodigo, "lblCodigo")
            Me.errClientes.SetError(Me.lblCodigo, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.errClientes.SetIconAlignment(Me.lblCodigo, CType(resources.GetObject("lblCodigo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.lblCodigo, CType(resources.GetObject("lblCodigo.IconPadding"), Integer))
            Me.lblCodigo.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblCodigo.Name = "lblCodigo"
            '
            'lblCelular
            '
            resources.ApplyResources(Me.lblCelular, "lblCelular")
            Me.errClientes.SetError(Me.lblCelular, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.errClientes.SetIconAlignment(Me.lblCelular, CType(resources.GetObject("lblCelular.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.lblCelular, CType(resources.GetObject("lblCelular.IconPadding"), Integer))
            Me.lblCelular.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblCelular.Name = "lblCelular"
            '
            'SubBuscador1
            '
            resources.ApplyResources(Me.SubBuscador1, "SubBuscador1")
            Me.SubBuscador1.BackColor = System.Drawing.Color.Black
            Me.SubBuscador1.Barra_Titulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador1.ConsultarDBPorFiltrado = False
            Me.SubBuscador1.Criterios = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador1.Criterios_Ocultos = 0
            Me.SubBuscador1.Criterios_OcultosEx = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.errClientes.SetError(Me.SubBuscador1, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.errClientes.SetIconAlignment(Me.SubBuscador1, CType(resources.GetObject("SubBuscador1.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.SubBuscador1, CType(resources.GetObject("SubBuscador1.IconPadding"), Integer))
            Me.SubBuscador1.IN_DataTable = Nothing
            Me.SubBuscador1.MultiSeleccion = False
            Me.SubBuscador1.Name = "SubBuscador1"
            Me.SubBuscador1.SQL_Cnn = Nothing
            Me.SubBuscador1.Tabla = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador1.Titulos = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBuscador1.Where = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'picCliente
            '
            resources.ApplyResources(Me.picCliente, "picCliente")
            Me.errClientes.SetError(Me.picCliente, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.errClientes.SetIconAlignment(Me.picCliente, CType(resources.GetObject("picCliente.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.picCliente, CType(resources.GetObject("picCliente.IconPadding"), Integer))
            Me.picCliente.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picCliente.Name = "picCliente"
            Me.picCliente.TabStop = False
            '
            'lblRFC
            '
            resources.ApplyResources(Me.lblRFC, "lblRFC")
            Me.errClientes.SetError(Me.lblRFC, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.errClientes.SetIconAlignment(Me.lblRFC, CType(resources.GetObject("lblRFC.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.lblRFC, CType(resources.GetObject("lblRFC.IconPadding"), Integer))
            Me.lblRFC.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblRFC.Name = "lblRFC"
            '
            'txtRFC
            '
            resources.ApplyResources(Me.txtRFC, "txtRFC")
            Me.txtRFC.AceptaNegativos = False
            Me.txtRFC.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.errClientes.SetError(Me.txtRFC, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtRFC.EstiloSBO = True
            Me.errClientes.SetIconAlignment(Me.txtRFC, CType(resources.GetObject("txtRFC.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.txtRFC, CType(resources.GetObject("txtRFC.IconPadding"), Integer))
            Me.txtRFC.MaxDecimales = 0
            Me.txtRFC.MaxEnteros = 0
            Me.txtRFC.Millares = False
            Me.txtRFC.Name = "txtRFC"
            Me.txtRFC.Size_AdjustableHeight = 20
            Me.txtRFC.TeclasDeshacer = True
            Me.txtRFC.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtSitioWeb
            '
            resources.ApplyResources(Me.txtSitioWeb, "txtSitioWeb")
            Me.txtSitioWeb.AceptaNegativos = False
            Me.txtSitioWeb.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.errClientes.SetError(Me.txtSitioWeb, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtSitioWeb.EstiloSBO = True
            Me.errClientes.SetIconAlignment(Me.txtSitioWeb, CType(resources.GetObject("txtSitioWeb.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.txtSitioWeb, CType(resources.GetObject("txtSitioWeb.IconPadding"), Integer))
            Me.txtSitioWeb.MaxDecimales = 0
            Me.txtSitioWeb.MaxEnteros = 0
            Me.txtSitioWeb.Millares = False
            Me.txtSitioWeb.Name = "txtSitioWeb"
            Me.txtSitioWeb.Size_AdjustableHeight = 20
            Me.txtSitioWeb.TeclasDeshacer = True
            Me.txtSitioWeb.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label11
            '
            resources.ApplyResources(Me.Label11, "Label11")
            Me.errClientes.SetError(Me.Label11, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.errClientes.SetIconAlignment(Me.Label11, CType(resources.GetObject("Label11.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.Label11, CType(resources.GetObject("Label11.IconPadding"), Integer))
            Me.Label11.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label11.Name = "Label11"
            '
            'lblTipo
            '
            resources.ApplyResources(Me.lblTipo, "lblTipo")
            Me.errClientes.SetError(Me.lblTipo, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.errClientes.SetIconAlignment(Me.lblTipo, CType(resources.GetObject("lblTipo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.lblTipo, CType(resources.GetObject("lblTipo.IconPadding"), Integer))
            Me.lblTipo.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblTipo.Name = "lblTipo"
            '
            'cboTipoSocio
            '
            resources.ApplyResources(Me.cboTipoSocio, "cboTipoSocio")
            Me.cboTipoSocio.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.errClientes.SetError(Me.cboTipoSocio, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.cboTipoSocio.EstiloSBO = True
            Me.errClientes.SetIconAlignment(Me.cboTipoSocio, CType(resources.GetObject("cboTipoSocio.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.cboTipoSocio, CType(resources.GetObject("cboTipoSocio.IconPadding"), Integer))
            Me.cboTipoSocio.Items.AddRange(New Object() {resources.GetString("cboTipoSocio.Items"), resources.GetString("cboTipoSocio.Items1")})
            Me.cboTipoSocio.Name = "cboTipoSocio"
            '
            'errClientes
            '
            Me.errClientes.ContainerControl = Me
            resources.ApplyResources(Me.errClientes, "errClientes")
            '
            'Label13
            '
            resources.ApplyResources(Me.Label13, "Label13")
            Me.Label13.BackColor = System.Drawing.Color.White
            Me.errClientes.SetError(Me.Label13, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.errClientes.SetIconAlignment(Me.Label13, CType(resources.GetObject("Label13.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.Label13, CType(resources.GetObject("Label13.IconPadding"), Integer))
            Me.Label13.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label13.Name = "Label13"
            '
            'Label7
            '
            resources.ApplyResources(Me.Label7, "Label7")
            Me.Label7.BackColor = System.Drawing.Color.White
            Me.errClientes.SetError(Me.Label7, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.errClientes.SetIconAlignment(Me.Label7, CType(resources.GetObject("Label7.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.Label7, CType(resources.GetObject("Label7.IconPadding"), Integer))
            Me.Label7.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label7.Name = "Label7"
            '
            'Label14
            '
            resources.ApplyResources(Me.Label14, "Label14")
            Me.Label14.BackColor = System.Drawing.Color.White
            Me.errClientes.SetError(Me.Label14, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.errClientes.SetIconAlignment(Me.Label14, CType(resources.GetObject("Label14.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.Label14, CType(resources.GetObject("Label14.IconPadding"), Integer))
            Me.Label14.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label14.Name = "Label14"
            '
            'Label15
            '
            resources.ApplyResources(Me.Label15, "Label15")
            Me.Label15.BackColor = System.Drawing.Color.White
            Me.errClientes.SetError(Me.Label15, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label15.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.errClientes.SetIconAlignment(Me.Label15, CType(resources.GetObject("Label15.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.Label15, CType(resources.GetObject("Label15.IconPadding"), Integer))
            Me.Label15.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label15.Name = "Label15"
            '
            'Label16
            '
            resources.ApplyResources(Me.Label16, "Label16")
            Me.Label16.BackColor = System.Drawing.Color.White
            Me.errClientes.SetError(Me.Label16, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label16.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.errClientes.SetIconAlignment(Me.Label16, CType(resources.GetObject("Label16.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.Label16, CType(resources.GetObject("Label16.IconPadding"), Integer))
            Me.Label16.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label16.Name = "Label16"
            '
            'Label17
            '
            resources.ApplyResources(Me.Label17, "Label17")
            Me.Label17.BackColor = System.Drawing.Color.White
            Me.errClientes.SetError(Me.Label17, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label17.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.errClientes.SetIconAlignment(Me.Label17, CType(resources.GetObject("Label17.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.Label17, CType(resources.GetObject("Label17.IconPadding"), Integer))
            Me.Label17.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label17.Name = "Label17"
            '
            'Label18
            '
            resources.ApplyResources(Me.Label18, "Label18")
            Me.Label18.BackColor = System.Drawing.Color.White
            Me.errClientes.SetError(Me.Label18, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label18.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.errClientes.SetIconAlignment(Me.Label18, CType(resources.GetObject("Label18.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.Label18, CType(resources.GetObject("Label18.IconPadding"), Integer))
            Me.Label18.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label18.Name = "Label18"
            '
            'Label19
            '
            resources.ApplyResources(Me.Label19, "Label19")
            Me.Label19.BackColor = System.Drawing.Color.White
            Me.errClientes.SetError(Me.Label19, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label19.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.errClientes.SetIconAlignment(Me.Label19, CType(resources.GetObject("Label19.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.Label19, CType(resources.GetObject("Label19.IconPadding"), Integer))
            Me.Label19.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label19.Name = "Label19"
            '
            'Label20
            '
            resources.ApplyResources(Me.Label20, "Label20")
            Me.Label20.BackColor = System.Drawing.Color.White
            Me.errClientes.SetError(Me.Label20, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label20.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.errClientes.SetIconAlignment(Me.Label20, CType(resources.GetObject("Label20.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.Label20, CType(resources.GetObject("Label20.IconPadding"), Integer))
            Me.Label20.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label20.Name = "Label20"
            '
            'Label21
            '
            resources.ApplyResources(Me.Label21, "Label21")
            Me.Label21.BackColor = System.Drawing.Color.White
            Me.errClientes.SetError(Me.Label21, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label21.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.errClientes.SetIconAlignment(Me.Label21, CType(resources.GetObject("Label21.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errClientes.SetIconPadding(Me.Label21, CType(resources.GetObject("Label21.IconPadding"), Integer))
            Me.Label21.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label21.Name = "Label21"
            '
            'ContextMenu1
            '
            Me.ContextMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1, Me.MenuItem2})
            resources.ApplyResources(Me.ContextMenu1, "ContextMenu1")
            '
            'MenuItem1
            '
            resources.ApplyResources(Me.MenuItem1, "MenuItem1")
            Me.MenuItem1.Index = 0
            '
            'MenuItem2
            '
            resources.ApplyResources(Me.MenuItem2, "MenuItem2")
            Me.MenuItem2.Index = 1
            '
            'frmCtrlInformacionClientes
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.Label13)
            Me.Controls.Add(Me.Label7)
            Me.Controls.Add(Me.Label14)
            Me.Controls.Add(Me.Label16)
            Me.Controls.Add(Me.lblCodigo)
            Me.Controls.Add(Me.lblCelular)
            Me.Controls.Add(Me.lblOficina)
            Me.Controls.Add(Me.lblRFC)
            Me.Controls.Add(Me.Label11)
            Me.Controls.Add(Me.Label21)
            Me.Controls.Add(Me.Label20)
            Me.Controls.Add(Me.Label19)
            Me.Controls.Add(Me.Label18)
            Me.Controls.Add(Me.Label17)
            Me.Controls.Add(Me.Label15)
            Me.Controls.Add(Me.txtDetalleCliente)
            Me.Controls.Add(Me.txtSitioWeb)
            Me.Controls.Add(Me.cboTipoSocio)
            Me.Controls.Add(Me.lblTipo)
            Me.Controls.Add(Me.txtCodigo)
            Me.Controls.Add(Me.txtRFC)
            Me.Controls.Add(Me.picCliente)
            Me.Controls.Add(Me.SubBuscador1)
            Me.Controls.Add(Me.txtFax)
            Me.Controls.Add(Me.txtCorreo)
            Me.Controls.Add(Me.txtOficina)
            Me.Controls.Add(Me.txtCasa)
            Me.Controls.Add(Me.txtCelular)
            Me.Controls.Add(Me.txtCliente)
            Me.Controls.Add(Me.lblCliente)
            Me.Controls.Add(Me.lblDetalle)
            Me.Controls.Add(Me.lblCorreo)
            Me.Controls.Add(Me.lblCasa)
            Me.Controls.Add(Me.lblFax)
            Me.Controls.Add(Me.tlbClientes)
            Me.Name = "frmCtrlInformacionClientes"
            CType(Me.picCliente, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.errClientes, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region "Declaraciones"
        Private m_adpCliente As SCGDataAccess.ClsClientesSBO

        Private m_intTipoInsercion As Integer

        'Utilizado para determinar si el usuario ha presionado el btnGuardar del SCGToolbar
        Private m_blnHaGuardado As Boolean

        Public Event RetornarDatos(ByVal p_strCardCode As String, ByVal p_strCardName As String)

        Private m_objUtilitarios As New Utilitarios(strConectionString)

        Private m_strConfiguracionCompañia As String

        Private Const mc_strConfiguracionCR = "CR"
        Private Const mc_strConfiguracionMX = "MX"

#End Region

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)

            MyBase.New()
            InitializeComponent()

        End Sub

        Public Sub New(ByVal p_intTipoInsercion As Integer, Optional ByVal p_strCardCode As String = "")

            MyBase.New()
            InitializeComponent()
            m_intTipoInsercion = p_intTipoInsercion
            If m_intTipoInsercion = 1 Then
                tlbClientes.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Nuevo
                m_adpCliente = New ClsClientesSBO()
                txtCodigo.Text = m_adpCliente.ObtenerCodCliente()
                txtCodigo.ReadOnly = False
                txtCliente.ReadOnly = False
            End If
            If p_strCardCode <> "" Then

                Call CargarCliente(p_strCardCode)

            End If

        End Sub

#End Region

#Region "Eventos"

        Private Sub frmCtrlInformacionClientes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                m_blnHaGuardado = False 'Establece la bandera en el valor original 

                'El usuario no podrá modificar el código
                'txtCodigo.ReadOnly = True

                'Se ocultan los botones del toolbar que no se van utilizar
                tlbClientes.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
                tlbClientes.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Buscar).Visible = False
                tlbClientes.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Visible = False
                tlbClientes.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False
                'Los campos Cliente y Codigo se habilitan solo con el botón Nuevo
                If m_intTipoInsercion <> 1 Then
                    inhabilitarCampos()
                End If
                m_strConfiguracionCompañia = m_objUtilitarios.ObtenerConfiguracionCompañia()
                cboTipoSocio.Text = "Sociedades"
                'm_intTipoInsercion = 0

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub tlbClientes_Click_Cancelar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbClientes.Click_Cancelar
            Try
                m_blnHaGuardado = False 'Establece la bandera en el valor original 
                limpiarCampos()
                'Los campos Cliente y Codigo se habilitan solo con el botón Nuevo
                inhabilitarCampos()
                'm_intTipoInsercion = 0

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub tlbClientes_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbClientes.Click_Cerrar

            Try
                If guardarAntesCerrar() Then
                    Me.Dispose()
                    Me.Close()
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub tlbClientes_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbClientes.Click_Guardar
            Try

                tlbClientes.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True
                guardar()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub tlbClientes1_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbClientes.Click_Nuevo
            ''''''
            Dim strCodigoCliente As String
            '''''''''''
            Try
                m_blnHaGuardado = False 'Establece la bandera en el valor original 
                'Limpia los campos de texto en caso de que tengan informacion
                limpiarCampos()
                'Habilita el campo Cliente para escribir
                habilitarCampos()

                ''''''
                'Selecciona el nuevo codigo para el cliente
                m_adpCliente = New SCGDataAccess.ClsClientesSBO
                strCodigoCliente = m_adpCliente.ObtenerCodCliente()
                If (strCodigoCliente <> "") Then
                    txtCodigo.Text = strCodigoCliente
                End If

                '''''''''''

                'Establece el tipo de insercion para indicar que se va a crear un nuevo usuario
                m_intTipoInsercion = 1

                txtCliente.Focus()
                txtCodigo.ReadOnly = False
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub picCliente_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picCliente.Click
            Try
                m_intTipoInsercion = 0

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                SubBuscador1.SQL_Cnn = DATemp.ObtieneConexion
                SubBuscador1.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorClientes
                SubBuscador1.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre & _
                "," & My.Resources.ResourceUI.Telefono & " 1" & "," & My.Resources.ResourceUI.Telefono & " 2" & _
                "," & My.Resources.ResourceUI.Celular & "," & My.Resources.ResourceUI.Fax & _
                "," & My.Resources.ResourceUI.Email & "," & "NOTES,LICTRADNUM"

                '"Código, Nombre,Phone1, Phone2, Cellular, Fax, E_Mail, Notes, LicTradNum"
                SubBuscador1.Criterios = "CardCode, CardName,Phone1, Phone2, Cellular, Fax, E_Mail, Notes, LicTradNum"
                SubBuscador1.Criterios_OcultosEx = "3,4,5,6,7,8,9"
                SubBuscador1.Criterios_Ocultos = 0

                SubBuscador1.Tabla = "SCGTA_VW_Clientes"
                SubBuscador1.Where = ""
                SubBuscador1.Activar_Buscador(sender)



            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub SubBuscador1_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles SubBuscador1.AppAceptar
            Try
                txtCliente.Text = Arreglo_Campos(1)
                txtCodigo.Text = Arreglo_Campos(0)
                txtCasa.Text = Arreglo_Campos(2)
                txtOficina.Text = Arreglo_Campos(3)
                txtCelular.Text = Arreglo_Campos(4)
                txtFax.Text = Arreglo_Campos(5)
                txtCorreo.Text = Arreglo_Campos(6)
                txtDetalleCliente.Text = Arreglo_Campos(7)
                txtRFC.Text = Arreglo_Campos(8)
                txtCodigo.ReadOnly = True
                txtCliente.ReadOnly = True
                'Se establece el tipo de insercion para indicar que se van a modificar los datos de un cliente
                m_intTipoInsercion = 2


                'Se permite actualizar todos los datos menos el nombre y codigo del cliente
                'Los campos Cliente y Codigo se habilitan solo con el botón Nuevo
                inhabilitarCampos()



            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub frmCtrlInformacionClientes_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

            Try
                If Not guardarAntesCerrar() Then
                    e.Cancel = True
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub frmCtrlInformacionClientes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

            Try

                If Asc(e.KeyChar) = Keys.Escape Then
                    If guardarAntesCerrar() Then
                        Me.Dispose()
                        Me.Close()
                    End If
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

#End Region

#Region "Procedimientos"

        Private Sub guardar()

            Dim lngResultado As Long

            Try
                m_adpCliente = New SCGDataAccess.ClsClientesSBO
                If (m_intTipoInsercion = 1) Then 'Se quiere crear un nuevo cliente

                    If validaCamposRequeridos() Then
                        lngResultado = m_adpCliente.CrearUsuario(txtCliente.Text, txtCodigo.Text, txtCasa.Text, txtOficina.Text _
                                                                , txtCelular.Text, txtFax.Text, txtCorreo.Text, txtDetalleCliente.Text, txtRFC.Text, txtSitioWeb.Text, cboTipoSocio.Text)
                        If (lngResultado = 0) Then
                            objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeCreadoNuevoCliente)



                            inhabilitarCampos()
                            m_blnHaGuardado = True 'Establece la bandera para indicar que el usuario guardó los datos
                            txtCodigo.ReadOnly = True
                            picCliente.Enabled = True
                            RaiseEvent RetornarDatos(txtCodigo.Text, txtCliente.Text)
                            limpiarCampos()

                        ElseIf (lngResultado = -1) Then
                            objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeNosepuedeAgregarClienteYaExiste)
                        ElseIf (lngResultado = -5002) Then
                            objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeNoSePuedeAgregarRFC12Caracteres)
                        End If

                    Else
                        'objSCGMSGBox.msgExclamationCustom("Faltan datos requeridos. Por favor verifique que se haya ingresado el nombre y código del cliente, así como el RFC")
                        picCliente.Enabled = True
                    End If



                End If 'Nuevo cliente

                If (m_intTipoInsercion = 2) Then 'Modificar datos de un cliente

                    If validaCamposRequeridos() Then

                        lngResultado = m_adpCliente.ActualizarDatosUsuario(txtCliente.Text, txtCodigo.Text, txtCasa.Text, txtOficina.Text _
                                                                          , txtCelular.Text, txtFax.Text, txtCorreo.Text, txtDetalleCliente.Text, txtRFC.Text)
                        If (lngResultado = 0) Then
                            objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.ModificadoSatisfactoriamente)
                            limpiarCampos()
                            inhabilitarCampos()
                            m_blnHaGuardado = True 'Establece la bandera para indicar que el usuario guardó los datos
                            RaiseEvent RetornarDatos(txtCodigo.Text, txtCliente.Text)

                        ElseIf (lngResultado = -5002) Then
                            objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeVerifiqueRFC12Caracteres)
                        End If
                        'Else
                        '    objSCGMSGBox.msgExclamationCustom("Faltan datos requeridos. Por favor verifique que se haya ingresado el nombre y código del cliente, así como el RFC")
                    End If
                End If ' Modificar datos


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub habilitarCampos()

            txtCliente.ReadOnly = False

        End Sub

        Private Sub inhabilitarCampos()

            txtCliente.ReadOnly = True

        End Sub

        Private Function validaCamposRequeridos() As Boolean

            Dim blnValido As Boolean

            Try

                errClientes.Clear()

                If (txtCliente.Text.Trim <> "" And txtCodigo.Text.Trim <> "" And cboTipoSocio.Text <> "") Then
                    blnValido = True
                Else

                    errClientes.SetError(txtCodigo, My.Resources.ResourceUI.MensajeIngreseNombreCodCliente)
                    errClientes.SetIconAlignment(txtCodigo, ErrorIconAlignment.MiddleRight)
                    blnValido = False

                End If

                If ValidarRFC() Then

                    blnValido = True

                Else

                    errClientes.SetError(txtRFC, My.Resources.ResourceUI.MensajeDebeIngresarIDClienteTamanoValido)
                    errClientes.SetIconAlignment(txtRFC, ErrorIconAlignment.MiddleRight)
                    blnValido = False

                End If

                Return blnValido

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try


        End Function

        Private Sub limpiarCampos()

            txtCliente.Clear()
            txtCodigo.Clear()
            txtCasa.Clear()
            txtCelular.Clear()
            txtOficina.Clear()
            txtCorreo.Clear()
            txtFax.Clear()
            txtDetalleCliente.Clear()
            txtRFC.Clear()
            errClientes.Clear()

        End Sub

        Private Function guardarAntesCerrar() As Boolean

            'Permite al usuario guardar los cambios hechos antes de cerrar el frm

            Try
                Dim evento As New System.Windows.Forms.ToolBarButtonClickEventArgs(tlbClientes.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar))
                Dim blnProcesoExitoso As Boolean = True

                If tlbClientes.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Nuevo Or _
                   m_intTipoInsercion = 1 Then

                    If hayDatos() Then ' Si el usuario ha llenado al menos un campo requerido

                        If Not m_blnHaGuardado Then 'Si no ha guardado antes de cerrar
                            If objSCGMSGBox.msgPregunta(My.Resources.ResourceUI.PreguntaDeseaGuardarCambiosFormulario) = MsgBoxResult.Yes Then

                                If validaCamposRequeridos() Then
                                    tlbClientes_Click_Guardar(Me, evento)
                                Else
                                    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeFaltanCamposVerifique)
                                    blnProcesoExitoso = False 'Si el usuario quiere guardar antes de cerrar pero faltan datos requeridos, debe darsele la oportunidad de digitarlos antes de cerrar el frm
                                    'así que mediante este boolean se le indica al evento que no debe cerrar el frm
                                End If 'validaCampos()

                            End If
                        End If 'm_blnHaGuardado

                    End If 'HayDatos()
                End If 'EstadoNuevo

                Return blnProcesoExitoso

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try

        End Function

        Private Function hayDatos() As Boolean
            'Permite determinar si el usuario ha llenado al menos un campo requerido

            Dim blnValido As Boolean
            blnValido = False

            Try
                If txtCliente.Text.Trim <> "" Then
                    blnValido = True
                End If

                If txtCodigo.Text.Trim <> "" Then
                    blnValido = True
                End If

                If txtRFC.Text.Trim <> "" Then
                    blnValido = True
                End If

                Return blnValido

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
                Return False
            End Try
        End Function

        Private Sub CargarCliente(ByVal p_strCardCode As String)

            Dim objClientes As New ClsClientesSBO.stcCliente
            m_adpCliente = New SCGDataAccess.ClsClientesSBO()
            objClientes = m_adpCliente.CargarCliente(p_strCardCode)
            txtCodigo.Text = objClientes.strCardCode
            txtCliente.Text = objClientes.strCardName
            txtCelular.Text = objClientes.strCelular
            txtCorreo.Text = objClientes.strCorreo
            txtDetalleCliente.Text = objClientes.strDetalle
            txtFax.Text = objClientes.strFax
            txtRFC.Text = objClientes.strRFC
            txtCasa.Text = objClientes.strTelfCasa
            txtOficina.Text = objClientes.strTelfOficina

        End Sub

        Private Function ValidarRFC() As Boolean
            Dim blnRFCCorrecto As Boolean = True
            Select Case m_strConfiguracionCompañia
                Case mc_strConfiguracionCR
                    If txtRFC.TextLength > 32 Then
                        blnRFCCorrecto = False
                    End If
                Case mc_strConfiguracionMX
                    If cboTipoSocio.Text = "Sociedades" Then
                        If txtRFC.TextLength <> 12 Then
                            blnRFCCorrecto = False
                        End If
                    Else
                        If txtRFC.TextLength <> 13 Then
                            blnRFCCorrecto = False
                        End If
                    End If

            End Select


            Return blnRFCCorrecto
        End Function

#End Region

    End Class

End Namespace