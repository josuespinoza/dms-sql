Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports System.Text.RegularExpressions
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGCommon
Namespace SCG_User_Interface
    Public Class frmConfiguracionServidordeCorreo
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP
#Region "Declaraciones"
        Private m_dstConfigServidorCorreo As New ConfigServidorCorreoDataset
        Private m_adpConfigServidorCorreo As New PublicidadEnviosAdapter
        Public WithEvents Label1 As System.Windows.Forms.Label
        Public WithEvents Label2 As System.Windows.Forms.Label
        Public WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents txtPuerto As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents chkUsaSSL As System.Windows.Forms.CheckBox
        Private m_drwConfigServidorCorreo As ConfigServidorCorreoDataset.SCGTA_TB_ConfiguracionDeCorreoRow
#End Region

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()
            InitializeComponent()
        End Sub

        Public Sub New(ByVal carga As Boolean)
            MyBase.New()
            InitializeComponent()
        End Sub
        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
            If Disposing Then
                If Not components Is Nothing Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(Disposing)
        End Sub
        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer
        Public ToolTip1 As System.Windows.Forms.ToolTip
        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.
        'Do not modify it using the code editor.


        Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents dlgFolderArchMarcas As System.Windows.Forms.FolderBrowserDialog
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents txtDirEnvia As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtServidorCorreo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtUserCorreo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label12 As System.Windows.Forms.Label
        Public WithEvents Label20 As System.Windows.Forms.Label
        Public WithEvents Label22 As System.Windows.Forms.Label
        Public WithEvents Label23 As System.Windows.Forms.Label
        Public WithEvents Label24 As System.Windows.Forms.Label
        Public WithEvents btnCancelar As System.Windows.Forms.Button
        Public WithEvents btnAplicar As System.Windows.Forms.Button


        Friend WithEvents txtPassWCorreo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label13 As System.Windows.Forms.Label
        Friend WithEvents EPConfigCorreos As System.Windows.Forms.ErrorProvider
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfiguracionServidordeCorreo))
            Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
            Me.GroupBox3 = New System.Windows.Forms.GroupBox()
            Me.chkUsaSSL = New System.Windows.Forms.CheckBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.txtPuerto = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label24 = New System.Windows.Forms.Label()
            Me.Label23 = New System.Windows.Forms.Label()
            Me.Label20 = New System.Windows.Forms.Label()
            Me.txtPassWCorreo = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label13 = New System.Windows.Forms.Label()
            Me.txtUserCorreo = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label12 = New System.Windows.Forms.Label()
            Me.txtServidorCorreo = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label10 = New System.Windows.Forms.Label()
            Me.txtDirEnvia = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.Label22 = New System.Windows.Forms.Label()
            Me.btnCancelar = New System.Windows.Forms.Button()
            Me.btnAplicar = New System.Windows.Forms.Button()
            Me.dlgFolderArchMarcas = New System.Windows.Forms.FolderBrowserDialog()
            Me.EPConfigCorreos = New System.Windows.Forms.ErrorProvider(Me.components)
            Me.GroupBox3.SuspendLayout()
            CType(Me.EPConfigCorreos, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'GroupBox3
            '
            resources.ApplyResources(Me.GroupBox3, "GroupBox3")
            Me.GroupBox3.Controls.Add(Me.chkUsaSSL)
            Me.GroupBox3.Controls.Add(Me.Label2)
            Me.GroupBox3.Controls.Add(Me.Label3)
            Me.GroupBox3.Controls.Add(Me.txtPuerto)
            Me.GroupBox3.Controls.Add(Me.Label4)
            Me.GroupBox3.Controls.Add(Me.Label1)
            Me.GroupBox3.Controls.Add(Me.Label24)
            Me.GroupBox3.Controls.Add(Me.Label23)
            Me.GroupBox3.Controls.Add(Me.Label20)
            Me.GroupBox3.Controls.Add(Me.txtPassWCorreo)
            Me.GroupBox3.Controls.Add(Me.Label13)
            Me.GroupBox3.Controls.Add(Me.txtUserCorreo)
            Me.GroupBox3.Controls.Add(Me.Label12)
            Me.GroupBox3.Controls.Add(Me.txtServidorCorreo)
            Me.GroupBox3.Controls.Add(Me.Label10)
            Me.GroupBox3.Controls.Add(Me.txtDirEnvia)
            Me.GroupBox3.Controls.Add(Me.Label8)
            Me.EPConfigCorreos.SetError(Me.GroupBox3, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.GroupBox3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPConfigCorreos.SetIconAlignment(Me.GroupBox3, CType(resources.GetObject("GroupBox3.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.GroupBox3, CType(resources.GetObject("GroupBox3.IconPadding"), Integer))
            Me.GroupBox3.Name = "GroupBox3"
            Me.GroupBox3.TabStop = False
            Me.ToolTip1.SetToolTip(Me.GroupBox3, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'chkUsaSSL
            '
            resources.ApplyResources(Me.chkUsaSSL, "chkUsaSSL")
            Me.EPConfigCorreos.SetError(Me.chkUsaSSL, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.chkUsaSSL.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPConfigCorreos.SetIconAlignment(Me.chkUsaSSL, CType(resources.GetObject("chkUsaSSL.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.chkUsaSSL, CType(resources.GetObject("chkUsaSSL.IconPadding"), Integer))
            Me.chkUsaSSL.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.chkUsaSSL.Name = "chkUsaSSL"
            Me.ToolTip1.SetToolTip(Me.chkUsaSSL, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.chkUsaSSL.UseVisualStyleBackColor = False
            '
            'Label2
            '
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.BackColor = System.Drawing.Color.White
            Me.EPConfigCorreos.SetError(Me.Label2, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.EPConfigCorreos.SetIconAlignment(Me.Label2, CType(resources.GetObject("Label2.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.Label2, CType(resources.GetObject("Label2.IconPadding"), Integer))
            Me.Label2.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label2.Name = "Label2"
            Me.ToolTip1.SetToolTip(Me.Label2, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'Label3
            '
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText
            Me.EPConfigCorreos.SetError(Me.Label3, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPConfigCorreos.SetIconAlignment(Me.Label3, CType(resources.GetObject("Label3.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.Label3, CType(resources.GetObject("Label3.IconPadding"), Integer))
            Me.Label3.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label3.Name = "Label3"
            Me.ToolTip1.SetToolTip(Me.Label3, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'txtPuerto
            '
            resources.ApplyResources(Me.txtPuerto, "txtPuerto")
            Me.txtPuerto.AceptaNegativos = False
            Me.txtPuerto.BackColor = System.Drawing.Color.White
            Me.EPConfigCorreos.SetError(Me.txtPuerto, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtPuerto.EstiloSBO = True
            Me.EPConfigCorreos.SetIconAlignment(Me.txtPuerto, CType(resources.GetObject("txtPuerto.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.txtPuerto, CType(resources.GetObject("txtPuerto.IconPadding"), Integer))
            Me.txtPuerto.MaxDecimales = 0
            Me.txtPuerto.MaxEnteros = 0
            Me.txtPuerto.Millares = False
            Me.txtPuerto.Name = "txtPuerto"
            Me.txtPuerto.Size_AdjustableHeight = 20
            Me.txtPuerto.TeclasDeshacer = True
            Me.txtPuerto.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            Me.ToolTip1.SetToolTip(Me.txtPuerto, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'Label4
            '
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
            Me.EPConfigCorreos.SetError(Me.Label4, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPConfigCorreos.SetIconAlignment(Me.Label4, CType(resources.GetObject("Label4.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.Label4, CType(resources.GetObject("Label4.IconPadding"), Integer))
            Me.Label4.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label4.Name = "Label4"
            Me.ToolTip1.SetToolTip(Me.Label4, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'Label1
            '
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.BackColor = System.Drawing.Color.White
            Me.EPConfigCorreos.SetError(Me.Label1, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.EPConfigCorreos.SetIconAlignment(Me.Label1, CType(resources.GetObject("Label1.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.Label1, CType(resources.GetObject("Label1.IconPadding"), Integer))
            Me.Label1.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label1.Name = "Label1"
            Me.ToolTip1.SetToolTip(Me.Label1, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'Label24
            '
            resources.ApplyResources(Me.Label24, "Label24")
            Me.Label24.BackColor = System.Drawing.SystemColors.ActiveCaptionText
            Me.EPConfigCorreos.SetError(Me.Label24, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label24.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPConfigCorreos.SetIconAlignment(Me.Label24, CType(resources.GetObject("Label24.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.Label24, CType(resources.GetObject("Label24.IconPadding"), Integer))
            Me.Label24.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label24.Name = "Label24"
            Me.ToolTip1.SetToolTip(Me.Label24, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'Label23
            '
            resources.ApplyResources(Me.Label23, "Label23")
            Me.Label23.BackColor = System.Drawing.SystemColors.ActiveCaptionText
            Me.EPConfigCorreos.SetError(Me.Label23, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.EPConfigCorreos.SetIconAlignment(Me.Label23, CType(resources.GetObject("Label23.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.Label23, CType(resources.GetObject("Label23.IconPadding"), Integer))
            Me.Label23.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label23.Name = "Label23"
            Me.ToolTip1.SetToolTip(Me.Label23, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'Label20
            '
            resources.ApplyResources(Me.Label20, "Label20")
            Me.Label20.BackColor = System.Drawing.SystemColors.ActiveCaptionText
            Me.EPConfigCorreos.SetError(Me.Label20, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.EPConfigCorreos.SetIconAlignment(Me.Label20, CType(resources.GetObject("Label20.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.Label20, CType(resources.GetObject("Label20.IconPadding"), Integer))
            Me.Label20.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label20.Name = "Label20"
            Me.ToolTip1.SetToolTip(Me.Label20, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'txtPassWCorreo
            '
            resources.ApplyResources(Me.txtPassWCorreo, "txtPassWCorreo")
            Me.txtPassWCorreo.AceptaNegativos = False
            Me.txtPassWCorreo.BackColor = System.Drawing.Color.White
            Me.EPConfigCorreos.SetError(Me.txtPassWCorreo, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtPassWCorreo.EstiloSBO = True
            Me.EPConfigCorreos.SetIconAlignment(Me.txtPassWCorreo, CType(resources.GetObject("txtPassWCorreo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.txtPassWCorreo, CType(resources.GetObject("txtPassWCorreo.IconPadding"), Integer))
            Me.txtPassWCorreo.MaxDecimales = 0
            Me.txtPassWCorreo.MaxEnteros = 0
            Me.txtPassWCorreo.Millares = False
            Me.txtPassWCorreo.Name = "txtPassWCorreo"
            Me.txtPassWCorreo.Size_AdjustableHeight = 20
            Me.txtPassWCorreo.TeclasDeshacer = True
            Me.txtPassWCorreo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            Me.ToolTip1.SetToolTip(Me.txtPassWCorreo, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'Label13
            '
            resources.ApplyResources(Me.Label13, "Label13")
            Me.Label13.Cursor = System.Windows.Forms.Cursors.Default
            Me.EPConfigCorreos.SetError(Me.Label13, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPConfigCorreos.SetIconAlignment(Me.Label13, CType(resources.GetObject("Label13.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.Label13, CType(resources.GetObject("Label13.IconPadding"), Integer))
            Me.Label13.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label13.Name = "Label13"
            Me.ToolTip1.SetToolTip(Me.Label13, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'txtUserCorreo
            '
            resources.ApplyResources(Me.txtUserCorreo, "txtUserCorreo")
            Me.txtUserCorreo.AceptaNegativos = False
            Me.txtUserCorreo.BackColor = System.Drawing.Color.White
            Me.EPConfigCorreos.SetError(Me.txtUserCorreo, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtUserCorreo.EstiloSBO = True
            Me.EPConfigCorreos.SetIconAlignment(Me.txtUserCorreo, CType(resources.GetObject("txtUserCorreo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.txtUserCorreo, CType(resources.GetObject("txtUserCorreo.IconPadding"), Integer))
            Me.txtUserCorreo.MaxDecimales = 0
            Me.txtUserCorreo.MaxEnteros = 0
            Me.txtUserCorreo.Millares = False
            Me.txtUserCorreo.Name = "txtUserCorreo"
            Me.txtUserCorreo.Size_AdjustableHeight = 20
            Me.txtUserCorreo.TeclasDeshacer = True
            Me.txtUserCorreo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            Me.ToolTip1.SetToolTip(Me.txtUserCorreo, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'Label12
            '
            resources.ApplyResources(Me.Label12, "Label12")
            Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
            Me.EPConfigCorreos.SetError(Me.Label12, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPConfigCorreos.SetIconAlignment(Me.Label12, CType(resources.GetObject("Label12.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.Label12, CType(resources.GetObject("Label12.IconPadding"), Integer))
            Me.Label12.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label12.Name = "Label12"
            Me.ToolTip1.SetToolTip(Me.Label12, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'txtServidorCorreo
            '
            resources.ApplyResources(Me.txtServidorCorreo, "txtServidorCorreo")
            Me.txtServidorCorreo.AceptaNegativos = False
            Me.txtServidorCorreo.BackColor = System.Drawing.Color.White
            Me.EPConfigCorreos.SetError(Me.txtServidorCorreo, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtServidorCorreo.EstiloSBO = True
            Me.EPConfigCorreos.SetIconAlignment(Me.txtServidorCorreo, CType(resources.GetObject("txtServidorCorreo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.txtServidorCorreo, CType(resources.GetObject("txtServidorCorreo.IconPadding"), Integer))
            Me.txtServidorCorreo.MaxDecimales = 0
            Me.txtServidorCorreo.MaxEnteros = 0
            Me.txtServidorCorreo.Millares = False
            Me.txtServidorCorreo.Name = "txtServidorCorreo"
            Me.txtServidorCorreo.Size_AdjustableHeight = 20
            Me.txtServidorCorreo.TeclasDeshacer = True
            Me.txtServidorCorreo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            Me.ToolTip1.SetToolTip(Me.txtServidorCorreo, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'Label10
            '
            resources.ApplyResources(Me.Label10, "Label10")
            Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
            Me.EPConfigCorreos.SetError(Me.Label10, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPConfigCorreos.SetIconAlignment(Me.Label10, CType(resources.GetObject("Label10.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.Label10, CType(resources.GetObject("Label10.IconPadding"), Integer))
            Me.Label10.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label10.Name = "Label10"
            Me.ToolTip1.SetToolTip(Me.Label10, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'txtDirEnvia
            '
            resources.ApplyResources(Me.txtDirEnvia, "txtDirEnvia")
            Me.txtDirEnvia.AceptaNegativos = False
            Me.txtDirEnvia.BackColor = System.Drawing.Color.White
            Me.EPConfigCorreos.SetError(Me.txtDirEnvia, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.txtDirEnvia.EstiloSBO = True
            Me.EPConfigCorreos.SetIconAlignment(Me.txtDirEnvia, CType(resources.GetObject("txtDirEnvia.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.txtDirEnvia, CType(resources.GetObject("txtDirEnvia.IconPadding"), Integer))
            Me.txtDirEnvia.MaxDecimales = 0
            Me.txtDirEnvia.MaxEnteros = 0
            Me.txtDirEnvia.Millares = False
            Me.txtDirEnvia.Name = "txtDirEnvia"
            Me.txtDirEnvia.Size_AdjustableHeight = 20
            Me.txtDirEnvia.TeclasDeshacer = True
            Me.txtDirEnvia.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            Me.ToolTip1.SetToolTip(Me.txtDirEnvia, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'Label8
            '
            resources.ApplyResources(Me.Label8, "Label8")
            Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
            Me.EPConfigCorreos.SetError(Me.Label8, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPConfigCorreos.SetIconAlignment(Me.Label8, CType(resources.GetObject("Label8.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.Label8, CType(resources.GetObject("Label8.IconPadding"), Integer))
            Me.Label8.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label8.Name = "Label8"
            Me.ToolTip1.SetToolTip(Me.Label8, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'Label22
            '
            resources.ApplyResources(Me.Label22, "Label22")
            Me.Label22.BackColor = System.Drawing.Color.White
            Me.EPConfigCorreos.SetError(Me.Label22, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.EPConfigCorreos.SetIconAlignment(Me.Label22, CType(resources.GetObject("Label22.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.Label22, CType(resources.GetObject("Label22.IconPadding"), Integer))
            Me.Label22.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label22.Name = "Label22"
            Me.ToolTip1.SetToolTip(Me.Label22, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'btnCancelar
            '
            resources.ApplyResources(Me.btnCancelar, "btnCancelar")
            Me.EPConfigCorreos.SetError(Me.btnCancelar, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.EPConfigCorreos.SetIconAlignment(Me.btnCancelar, CType(resources.GetObject("btnCancelar.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.btnCancelar, CType(resources.GetObject("btnCancelar.IconPadding"), Integer))
            Me.btnCancelar.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.btnCancelar.Name = "btnCancelar"
            Me.ToolTip1.SetToolTip(Me.btnCancelar, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'btnAplicar
            '
            resources.ApplyResources(Me.btnAplicar, "btnAplicar")
            Me.EPConfigCorreos.SetError(Me.btnAplicar, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.EPConfigCorreos.SetIconAlignment(Me.btnAplicar, CType(resources.GetObject("btnAplicar.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.EPConfigCorreos.SetIconPadding(Me.btnAplicar, CType(resources.GetObject("btnAplicar.IconPadding"), Integer))
            Me.btnAplicar.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.btnAplicar.Name = "btnAplicar"
            Me.ToolTip1.SetToolTip(Me.btnAplicar, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            '
            'dlgFolderArchMarcas
            '
            Me.dlgFolderArchMarcas.Description = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dlgFolderArchMarcas.SelectedPath = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'EPConfigCorreos
            '
            Me.EPConfigCorreos.ContainerControl = Me
            resources.ApplyResources(Me.EPConfigCorreos, "EPConfigCorreos")
            '
            'frmConfiguracionServidordeCorreo
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
            Me.Controls.Add(Me.btnCancelar)
            Me.Controls.Add(Me.btnAplicar)
            Me.Controls.Add(Me.Label22)
            Me.Controls.Add(Me.GroupBox3)
            Me.Name = "frmConfiguracionServidordeCorreo"
            Me.ToolTip1.SetToolTip(Me, Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation)
            Me.GroupBox3.ResumeLayout(False)
            Me.GroupBox3.PerformLayout()
            CType(Me.EPConfigCorreos, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
#End Region

        Private Sub frmConfiguracionServidordeCorreo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                'Dim tamaño As New Size
                Call m_adpConfigServidorCorreo.Fill(m_dstConfigServidorCorreo)

                If m_dstConfigServidorCorreo.SCGTA_TB_ConfiguracionDeCorreo.Rows.Count > 0 Then

                    m_drwConfigServidorCorreo = m_dstConfigServidorCorreo.SCGTA_TB_ConfiguracionDeCorreo.Rows(0)

                    txtDirEnvia.Text = m_drwConfigServidorCorreo.DireccionCorreoEnvia
                    txtPassWCorreo.Text = m_drwConfigServidorCorreo.PasswordSMTP
                    txtServidorCorreo.Text = m_drwConfigServidorCorreo.ServidorDeCorreo
                    txtUserCorreo.Text = m_drwConfigServidorCorreo.UsuarioSMTP

                    Try

                        txtPuerto.Text = m_drwConfigServidorCorreo.Puerto
                        chkUsaSSL.Checked = m_drwConfigServidorCorreo.UsaSSL


                    Catch ex As StrongTypingException

                    End Try


                    'txtPuerto.Text = m_drwConfigServidorCorreo.Puerto
                    'chkUsaSSL.Checked = m_drwConfigServidorCorreo.UsaSSL

                    'tamaño.Width = 360
                    'tamaño.Height = 192
                    'Me.Size = tamaño

                End If

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Function GuardaConfigServidorDeCorreo(ByVal Nuevo As Boolean, _
                                                      ByRef dtbConfigServidorDeCorreo As ConfigServidorCorreoDataset.SCGTA_TB_ConfiguracionDeCorreoDataTable, _
                                                      ByVal strServidordeCorreo As String, _
                                                      ByVal strCorreoqueEnvia As String, _
                                                      ByVal strSMTPusuario As String, _
                                                      ByVal strSMTPpassword As String, _
                                                      ByVal strPuerto As String, _
                                                      ByVal strUsaSSL As Boolean) As Boolean

            Dim drwConfigServidordeCorreo As ConfigServidorCorreoDataset.SCGTA_TB_ConfiguracionDeCorreoRow

            Try
                If Nuevo Then

                    drwConfigServidordeCorreo = dtbConfigServidorDeCorreo.NewSCGTA_TB_ConfiguracionDeCorreoRow

                Else

                    drwConfigServidordeCorreo = CType(dtbConfigServidorDeCorreo.Rows(0), ConfigServidorCorreoDataset.SCGTA_TB_ConfiguracionDeCorreoRow)

                End If

                drwConfigServidordeCorreo.DireccionCorreoEnvia = strCorreoqueEnvia
                drwConfigServidordeCorreo.PasswordSMTP = strSMTPpassword
                drwConfigServidordeCorreo.ServidorDeCorreo = strServidordeCorreo
                drwConfigServidordeCorreo.UsuarioSMTP = strSMTPusuario
                drwConfigServidordeCorreo.Puerto = strPuerto
                drwConfigServidordeCorreo.UsaSSL = strUsaSSL


                If Nuevo Then
                    dtbConfigServidorDeCorreo.AddSCGTA_TB_ConfiguracionDeCorreoRow(drwConfigServidordeCorreo)
                End If
                Return True
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Return False

                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Function

        Private Sub btnAplicar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAplicar.Click
            Try

                If Not FaltanCamposRequeridos() Then
                    If m_dstConfigServidorCorreo.SCGTA_TB_ConfiguracionDeCorreo.Rows.Count = 0 Then


                        If GuardaConfigServidorDeCorreo(True, m_dstConfigServidorCorreo.SCGTA_TB_ConfiguracionDeCorreo, _
                                                         txtServidorCorreo.Text, txtDirEnvia.Text, txtUserCorreo.Text, _
                                                         txtPassWCorreo.Text, txtPuerto.Text, chkUsaSSL.Checked) Then

                            Call m_adpConfigServidorCorreo.Update(m_dstConfigServidorCorreo)
                            g_strServidordeCorreo = txtServidorCorreo.Text
                            g_strDirEnviaCorreo = txtDirEnvia.Text
                            g_strUsuarioSMTP = txtUserCorreo.Text
                            g_strPasswordSMTP = txtPassWCorreo.Text
                            g_strPuerto = txtPuerto.Text
                            g_chkUsaSSL = chkUsaSSL.Checked

                            Me.Close()
                        End If

                    Else
                        If GuardaConfigServidorDeCorreo(False, m_dstConfigServidorCorreo.SCGTA_TB_ConfiguracionDeCorreo, _
                                                        txtServidorCorreo.Text, txtDirEnvia.Text, txtUserCorreo.Text, _
                                                        txtPassWCorreo.Text, txtPuerto.Text, chkUsaSSL.Checked) Then

                            Call m_adpConfigServidorCorreo.Update(m_dstConfigServidorCorreo)
                            g_strServidordeCorreo = txtServidorCorreo.Text
                            g_strDirEnviaCorreo = txtDirEnvia.Text
                            g_strUsuarioSMTP = txtUserCorreo.Text
                            g_strPasswordSMTP = txtPassWCorreo.Text
                            g_strPuerto = txtPuerto.Text
                            g_chkUsaSSL = chkUsaSSL.Checked

                            Me.Close()
                        End If

                    End If
                End If

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Function FaltanCamposRequeridos() As Boolean

            Dim blnFaltanCamposRequerido As Boolean = False

            Try
                If txtDirEnvia.Text = "" Then
                    EPConfigCorreos.SetError(txtDirEnvia, My.Resources.ResourceUI.MensajeDebeingresarDirCorreo)
                    blnFaltanCamposRequerido = True
                Else
                    EPConfigCorreos.SetError(txtDirEnvia, My.Resources.ResourceUI.MensajeDebeingresarDirCorreo)


                    If Not Mensajeria.EmailValido(txtDirEnvia.Text) Then

                        EPConfigCorreos.SetError(txtDirEnvia, My.Resources.ResourceUI.MensajeDireccionCorreoFormatoNoValido)
                        blnFaltanCamposRequerido = True
                    Else
                        EPConfigCorreos.SetError(txtDirEnvia, "")
                    End If

                End If

                If txtPassWCorreo.Text = "" Then
                    EPConfigCorreos.SetError(txtPassWCorreo, My.Resources.ResourceUI.MensajeDebeSeleccionarContrasena)

                    blnFaltanCamposRequerido = True
                Else
                    EPConfigCorreos.SetError(txtPassWCorreo, "")

                End If

                If txtServidorCorreo.Text = "" Then
                    EPConfigCorreos.SetError(txtServidorCorreo, My.Resources.ResourceUI.MensajeDebeIngresarSrvCorreo)
                    blnFaltanCamposRequerido = True
                Else
                    EPConfigCorreos.SetError(txtServidorCorreo, "")

                End If

                If txtUserCorreo.Text = "" Then
                    blnFaltanCamposRequerido = True
                    EPConfigCorreos.SetError(txtUserCorreo, My.Resources.ResourceUI.MensajeDebeSeleccionarUsuario)
                Else
                    EPConfigCorreos.SetError(txtUserCorreo, "")

                End If

                If txtPuerto.Text = "" Then
                    blnFaltanCamposRequerido = True
                    EPConfigCorreos.SetError(txtPuerto, My.Resources.ResourceUI.MensajeDebeSeleccionarPuerto)
                Else
                    EPConfigCorreos.SetError(txtPuerto, "")

                End If

                Return blnFaltanCamposRequerido
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Function

        Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
            Me.Close()
        End Sub
    End Class
End Namespace

