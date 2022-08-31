Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon
Imports SCG_User_Interface.SCG_User_Interface

Public Class frmRendimientoxOrden
    Inherits SCG.UX.Windows.SAP.frmPlantillaSAP


#Region "Declaraciones"
    Private WithEvents m_buOrdenes As New Buscador.SubBuscador
    Private CodCliente As String
    Private m_adpRendimientoxOrden As New RendimientoDataAdapter
    Private m_dstRendimientoxOrden As New RendimientoxOrdenDataset

    Private Const mc_strGastos As String = "Gastos"
    Private Const mc_strRendimiento As String = "Rendimiento"
    Private Const mc_strValorReal As String = "Valor Real"
    Private Const mc_strValorOtorgado As String = "Valor Otorgado"
    Private Const mc_strAcumulado As String = "Acumulado"

#End Region


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    Public Sub New(ByVal CargaForma As Boolean)
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
    Friend WithEvents dtgRepuestos As System.Windows.Forms.DataGrid
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents txtEstado As NEWTEXTBOX.NEWTEXTBOX_CTRL
    Friend WithEvents txtPlaca As NEWTEXTBOX.NEWTEXTBOX_CTRL
    Friend WithEvents txtModelo As NEWTEXTBOX.NEWTEXTBOX_CTRL
    Friend WithEvents txtMarca As NEWTEXTBOX.NEWTEXTBOX_CTRL
    Friend WithEvents txtPrioridad As NEWTEXTBOX.NEWTEXTBOX_CTRL
    Friend WithEvents txtTipoOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
    Friend WithEvents txtNoOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
    Friend WithEvents txtNoCono As NEWTEXTBOX.NEWTEXTBOX_CTRL
    Friend WithEvents txtNoVisita As NEWTEXTBOX.NEWTEXTBOX_CTRL
    Friend WithEvents lblLine4 As System.Windows.Forms.Label
    Friend WithEvents lblLine5 As System.Windows.Forms.Label
    Public WithEvents Label12 As System.Windows.Forms.Label
    Public WithEvents lblLine7 As System.Windows.Forms.Label
    Public WithEvents lblLine9 As System.Windows.Forms.Label
    Public WithEvents lblLine8 As System.Windows.Forms.Label
    Public WithEvents lblLine2 As System.Windows.Forms.Label
    Public WithEvents lblLine3 As System.Windows.Forms.Label
    Public WithEvents lblLine1 As System.Windows.Forms.Label
    Friend WithEvents lblPlaca As System.Windows.Forms.Label
    Friend WithEvents lblMarca As System.Windows.Forms.Label
    Public WithEvents lblModelo As System.Windows.Forms.Label
    Friend WithEvents lblNoOrden As System.Windows.Forms.Label
    Public WithEvents lblNoCono As System.Windows.Forms.Label
    Friend WithEvents lblNoVisita As System.Windows.Forms.Label
    Public WithEvents lblTipoOrdenO As System.Windows.Forms.Label
    Public WithEvents lblPrioridad As System.Windows.Forms.Label
    Public WithEvents lblEstadoO As System.Windows.Forms.Label
    Friend WithEvents picRepuesto As System.Windows.Forms.PictureBox
    Friend WithEvents btnCerrar As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRendimientoxOrden))
        Me.dtgRepuestos = New System.Windows.Forms.DataGrid
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.picRepuesto = New System.Windows.Forms.PictureBox
        Me.txtEstado = New NEWTEXTBOX.NEWTEXTBOX_CTRL
        Me.txtPlaca = New NEWTEXTBOX.NEWTEXTBOX_CTRL
        Me.txtModelo = New NEWTEXTBOX.NEWTEXTBOX_CTRL
        Me.txtMarca = New NEWTEXTBOX.NEWTEXTBOX_CTRL
        Me.txtPrioridad = New NEWTEXTBOX.NEWTEXTBOX_CTRL
        Me.txtTipoOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
        Me.txtNoOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
        Me.txtNoCono = New NEWTEXTBOX.NEWTEXTBOX_CTRL
        Me.txtNoVisita = New NEWTEXTBOX.NEWTEXTBOX_CTRL
        Me.lblLine4 = New System.Windows.Forms.Label
        Me.lblLine5 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.lblLine7 = New System.Windows.Forms.Label
        Me.lblLine9 = New System.Windows.Forms.Label
        Me.lblLine8 = New System.Windows.Forms.Label
        Me.lblLine2 = New System.Windows.Forms.Label
        Me.lblLine3 = New System.Windows.Forms.Label
        Me.lblLine1 = New System.Windows.Forms.Label
        Me.lblPlaca = New System.Windows.Forms.Label
        Me.lblMarca = New System.Windows.Forms.Label
        Me.lblModelo = New System.Windows.Forms.Label
        Me.lblNoOrden = New System.Windows.Forms.Label
        Me.lblNoCono = New System.Windows.Forms.Label
        Me.lblNoVisita = New System.Windows.Forms.Label
        Me.lblTipoOrdenO = New System.Windows.Forms.Label
        Me.lblPrioridad = New System.Windows.Forms.Label
        Me.lblEstadoO = New System.Windows.Forms.Label
        Me.btnCerrar = New System.Windows.Forms.Button
        CType(Me.dtgRepuestos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dtgRepuestos
        '
        Me.dtgRepuestos.BackgroundColor = System.Drawing.Color.White
        Me.dtgRepuestos.CaptionVisible = False
        Me.dtgRepuestos.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
        Me.dtgRepuestos.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtgRepuestos.HeaderBackColor = System.Drawing.Color.White
        Me.dtgRepuestos.HeaderFont = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtgRepuestos.HeaderForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.dtgRepuestos.Location = New System.Drawing.Point(12, 126)
        Me.dtgRepuestos.Name = "dtgRepuestos"
        Me.dtgRepuestos.RowHeadersVisible = False
        Me.dtgRepuestos.Size = New System.Drawing.Size(576, 263)
        Me.dtgRepuestos.TabIndex = 9143
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.picRepuesto)
        Me.GroupBox3.Controls.Add(Me.txtEstado)
        Me.GroupBox3.Controls.Add(Me.txtPlaca)
        Me.GroupBox3.Controls.Add(Me.txtModelo)
        Me.GroupBox3.Controls.Add(Me.txtMarca)
        Me.GroupBox3.Controls.Add(Me.txtPrioridad)
        Me.GroupBox3.Controls.Add(Me.txtTipoOrden)
        Me.GroupBox3.Controls.Add(Me.txtNoOrden)
        Me.GroupBox3.Controls.Add(Me.txtNoCono)
        Me.GroupBox3.Controls.Add(Me.txtNoVisita)
        Me.GroupBox3.Controls.Add(Me.lblLine4)
        Me.GroupBox3.Controls.Add(Me.lblLine5)
        Me.GroupBox3.Controls.Add(Me.Label12)
        Me.GroupBox3.Controls.Add(Me.lblLine7)
        Me.GroupBox3.Controls.Add(Me.lblLine9)
        Me.GroupBox3.Controls.Add(Me.lblLine8)
        Me.GroupBox3.Controls.Add(Me.lblLine2)
        Me.GroupBox3.Controls.Add(Me.lblLine3)
        Me.GroupBox3.Controls.Add(Me.lblLine1)
        Me.GroupBox3.Controls.Add(Me.lblPlaca)
        Me.GroupBox3.Controls.Add(Me.lblMarca)
        Me.GroupBox3.Controls.Add(Me.lblModelo)
        Me.GroupBox3.Controls.Add(Me.lblNoOrden)
        Me.GroupBox3.Controls.Add(Me.lblNoCono)
        Me.GroupBox3.Controls.Add(Me.lblNoVisita)
        Me.GroupBox3.Controls.Add(Me.lblTipoOrdenO)
        Me.GroupBox3.Controls.Add(Me.lblPrioridad)
        Me.GroupBox3.Controls.Add(Me.lblEstadoO)
        Me.GroupBox3.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.GroupBox3.Location = New System.Drawing.Point(12, 11)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(576, 101)
        Me.GroupBox3.TabIndex = 9144
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Información general"
        '
        'picRepuesto
        '
        Me.picRepuesto.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
        Me.picRepuesto.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picRepuesto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.picRepuesto.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
        Me.picRepuesto.Location = New System.Drawing.Point(164, 23)
        Me.picRepuesto.Name = "picRepuesto"
        Me.picRepuesto.Size = New System.Drawing.Size(15, 15)
        Me.picRepuesto.TabIndex = 9118
        Me.picRepuesto.TabStop = False
        '
        'txtEstado
        '
        Me.txtEstado.AceptaNegativos = False
        Me.txtEstado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.txtEstado.EstiloSBO = True
        Me.txtEstado.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEstado.ForeColor = System.Drawing.Color.Black
        Me.txtEstado.Location = New System.Drawing.Point(472, 18)
        Me.txtEstado.MaxDecimales = 0
        Me.txtEstado.MaxEnteros = 0
        Me.txtEstado.Millares = False
        Me.txtEstado.Name = "txtEstado"
        Me.txtEstado.ReadOnly = True
        Me.txtEstado.Size = New System.Drawing.Size(90, 22)
        Me.txtEstado.Size_AdjustableHeight = 22
        Me.txtEstado.TabIndex = 6
        Me.txtEstado.TeclasDeshacer = True
        Me.txtEstado.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
        '
        'txtPlaca
        '
        Me.txtPlaca.AceptaNegativos = False
        Me.txtPlaca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.txtPlaca.EstiloSBO = True
        Me.txtPlaca.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPlaca.ForeColor = System.Drawing.Color.Black
        Me.txtPlaca.Location = New System.Drawing.Point(232, 18)
        Me.txtPlaca.MaxDecimales = 0
        Me.txtPlaca.MaxEnteros = 0
        Me.txtPlaca.Millares = False
        Me.txtPlaca.Name = "txtPlaca"
        Me.txtPlaca.ReadOnly = True
        Me.txtPlaca.Size = New System.Drawing.Size(150, 22)
        Me.txtPlaca.Size_AdjustableHeight = 22
        Me.txtPlaca.TabIndex = 3
        Me.txtPlaca.TeclasDeshacer = True
        Me.txtPlaca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
        '
        'txtModelo
        '
        Me.txtModelo.AceptaNegativos = False
        Me.txtModelo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.txtModelo.EstiloSBO = True
        Me.txtModelo.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtModelo.ForeColor = System.Drawing.Color.Black
        Me.txtModelo.Location = New System.Drawing.Point(232, 68)
        Me.txtModelo.MaxDecimales = 0
        Me.txtModelo.MaxEnteros = 0
        Me.txtModelo.Millares = False
        Me.txtModelo.Name = "txtModelo"
        Me.txtModelo.ReadOnly = True
        Me.txtModelo.Size = New System.Drawing.Size(150, 22)
        Me.txtModelo.Size_AdjustableHeight = 22
        Me.txtModelo.TabIndex = 5
        Me.txtModelo.TeclasDeshacer = True
        Me.txtModelo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
        '
        'txtMarca
        '
        Me.txtMarca.AceptaNegativos = False
        Me.txtMarca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.txtMarca.EstiloSBO = True
        Me.txtMarca.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMarca.ForeColor = System.Drawing.Color.Black
        Me.txtMarca.Location = New System.Drawing.Point(232, 43)
        Me.txtMarca.MaxDecimales = 0
        Me.txtMarca.MaxEnteros = 0
        Me.txtMarca.Millares = False
        Me.txtMarca.Name = "txtMarca"
        Me.txtMarca.ReadOnly = True
        Me.txtMarca.Size = New System.Drawing.Size(150, 22)
        Me.txtMarca.Size_AdjustableHeight = 22
        Me.txtMarca.TabIndex = 4
        Me.txtMarca.TeclasDeshacer = True
        Me.txtMarca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
        '
        'txtPrioridad
        '
        Me.txtPrioridad.AceptaNegativos = False
        Me.txtPrioridad.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.txtPrioridad.EstiloSBO = True
        Me.txtPrioridad.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPrioridad.ForeColor = System.Drawing.Color.Black
        Me.txtPrioridad.Location = New System.Drawing.Point(472, 68)
        Me.txtPrioridad.MaxDecimales = 0
        Me.txtPrioridad.MaxEnteros = 0
        Me.txtPrioridad.Millares = False
        Me.txtPrioridad.Name = "txtPrioridad"
        Me.txtPrioridad.ReadOnly = True
        Me.txtPrioridad.Size = New System.Drawing.Size(90, 22)
        Me.txtPrioridad.Size_AdjustableHeight = 22
        Me.txtPrioridad.TabIndex = 8
        Me.txtPrioridad.TeclasDeshacer = True
        Me.txtPrioridad.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
        '
        'txtTipoOrden
        '
        Me.txtTipoOrden.AceptaNegativos = False
        Me.txtTipoOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.txtTipoOrden.EstiloSBO = True
        Me.txtTipoOrden.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTipoOrden.ForeColor = System.Drawing.Color.Black
        Me.txtTipoOrden.Location = New System.Drawing.Point(472, 43)
        Me.txtTipoOrden.MaxDecimales = 0
        Me.txtTipoOrden.MaxEnteros = 0
        Me.txtTipoOrden.Millares = False
        Me.txtTipoOrden.Name = "txtTipoOrden"
        Me.txtTipoOrden.ReadOnly = True
        Me.txtTipoOrden.Size = New System.Drawing.Size(90, 22)
        Me.txtTipoOrden.Size_AdjustableHeight = 22
        Me.txtTipoOrden.TabIndex = 7
        Me.txtTipoOrden.TeclasDeshacer = True
        Me.txtTipoOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
        '
        'txtNoOrden
        '
        Me.txtNoOrden.AceptaNegativos = False
        Me.txtNoOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.txtNoOrden.EstiloSBO = True
        Me.txtNoOrden.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoOrden.ForeColor = System.Drawing.Color.Black
        Me.txtNoOrden.Location = New System.Drawing.Point(104, 18)
        Me.txtNoOrden.MaxDecimales = 0
        Me.txtNoOrden.MaxEnteros = 0
        Me.txtNoOrden.Millares = False
        Me.txtNoOrden.Name = "txtNoOrden"
        Me.txtNoOrden.ReadOnly = True
        Me.txtNoOrden.Size = New System.Drawing.Size(60, 22)
        Me.txtNoOrden.Size_AdjustableHeight = 22
        Me.txtNoOrden.TabIndex = 1
        Me.txtNoOrden.TeclasDeshacer = True
        Me.txtNoOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
        '
        'txtNoCono
        '
        Me.txtNoCono.AceptaNegativos = False
        Me.txtNoCono.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.txtNoCono.EstiloSBO = True
        Me.txtNoCono.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoCono.ForeColor = System.Drawing.Color.Black
        Me.txtNoCono.Location = New System.Drawing.Point(104, 68)
        Me.txtNoCono.MaxDecimales = 0
        Me.txtNoCono.MaxEnteros = 0
        Me.txtNoCono.Millares = False
        Me.txtNoCono.Name = "txtNoCono"
        Me.txtNoCono.ReadOnly = True
        Me.txtNoCono.Size = New System.Drawing.Size(60, 22)
        Me.txtNoCono.Size_AdjustableHeight = 22
        Me.txtNoCono.TabIndex = 2
        Me.txtNoCono.TeclasDeshacer = True
        Me.txtNoCono.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
        '
        'txtNoVisita
        '
        Me.txtNoVisita.AceptaNegativos = False
        Me.txtNoVisita.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.txtNoVisita.EstiloSBO = True
        Me.txtNoVisita.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoVisita.ForeColor = System.Drawing.Color.Black
        Me.txtNoVisita.Location = New System.Drawing.Point(104, 43)
        Me.txtNoVisita.MaxDecimales = 0
        Me.txtNoVisita.MaxEnteros = 0
        Me.txtNoVisita.Millares = False
        Me.txtNoVisita.Name = "txtNoVisita"
        Me.txtNoVisita.ReadOnly = True
        Me.txtNoVisita.Size = New System.Drawing.Size(60, 22)
        Me.txtNoVisita.Size_AdjustableHeight = 22
        Me.txtNoVisita.TabIndex = 0
        Me.txtNoVisita.TeclasDeshacer = True
        Me.txtNoVisita.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
        '
        'lblLine4
        '
        Me.lblLine4.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
        Me.lblLine4.Location = New System.Drawing.Point(185, 38)
        Me.lblLine4.Name = "lblLine4"
        Me.lblLine4.Size = New System.Drawing.Size(47, 1)
        Me.lblLine4.TabIndex = 494
        '
        'lblLine5
        '
        Me.lblLine5.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
        Me.lblLine5.Location = New System.Drawing.Point(185, 63)
        Me.lblLine5.Name = "lblLine5"
        Me.lblLine5.Size = New System.Drawing.Size(47, 1)
        Me.lblLine5.TabIndex = 472
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
        Me.Label12.Location = New System.Drawing.Point(185, 88)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(47, 1)
        Me.Label12.TabIndex = 470
        '
        'lblLine7
        '
        Me.lblLine7.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
        Me.lblLine7.Location = New System.Drawing.Point(401, 38)
        Me.lblLine7.Name = "lblLine7"
        Me.lblLine7.Size = New System.Drawing.Size(71, 1)
        Me.lblLine7.TabIndex = 465
        '
        'lblLine9
        '
        Me.lblLine9.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
        Me.lblLine9.Location = New System.Drawing.Point(401, 88)
        Me.lblLine9.Name = "lblLine9"
        Me.lblLine9.Size = New System.Drawing.Size(71, 1)
        Me.lblLine9.TabIndex = 463
        '
        'lblLine8
        '
        Me.lblLine8.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
        Me.lblLine8.Location = New System.Drawing.Point(401, 63)
        Me.lblLine8.Name = "lblLine8"
        Me.lblLine8.Size = New System.Drawing.Size(71, 1)
        Me.lblLine8.TabIndex = 461
        '
        'lblLine2
        '
        Me.lblLine2.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
        Me.lblLine2.Location = New System.Drawing.Point(9, 38)
        Me.lblLine2.Name = "lblLine2"
        Me.lblLine2.Size = New System.Drawing.Size(95, 1)
        Me.lblLine2.TabIndex = 456
        '
        'lblLine3
        '
        Me.lblLine3.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
        Me.lblLine3.Location = New System.Drawing.Point(9, 88)
        Me.lblLine3.Name = "lblLine3"
        Me.lblLine3.Size = New System.Drawing.Size(95, 1)
        Me.lblLine3.TabIndex = 403
        '
        'lblLine1
        '
        Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
        Me.lblLine1.Location = New System.Drawing.Point(9, 63)
        Me.lblLine1.Name = "lblLine1"
        Me.lblLine1.Size = New System.Drawing.Size(95, 1)
        Me.lblLine1.TabIndex = 387
        '
        'lblPlaca
        '
        Me.lblPlaca.Font = New System.Drawing.Font("Arial Unicode MS", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPlaca.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.lblPlaca.Location = New System.Drawing.Point(184, 24)
        Me.lblPlaca.Name = "lblPlaca"
        Me.lblPlaca.Size = New System.Drawing.Size(39, 15)
        Me.lblPlaca.TabIndex = 493
        Me.lblPlaca.Text = "Placa"
        '
        'lblMarca
        '
        Me.lblMarca.Font = New System.Drawing.Font("Arial Unicode MS", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMarca.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.lblMarca.Location = New System.Drawing.Point(184, 49)
        Me.lblMarca.Name = "lblMarca"
        Me.lblMarca.Size = New System.Drawing.Size(39, 15)
        Me.lblMarca.TabIndex = 464
        Me.lblMarca.Text = "Marca"
        '
        'lblModelo
        '
        Me.lblModelo.Font = New System.Drawing.Font("Arial Unicode MS", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblModelo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.lblModelo.Location = New System.Drawing.Point(184, 74)
        Me.lblModelo.Name = "lblModelo"
        Me.lblModelo.Size = New System.Drawing.Size(47, 15)
        Me.lblModelo.TabIndex = 460
        Me.lblModelo.Text = "Modelo"
        '
        'lblNoOrden
        '
        Me.lblNoOrden.Font = New System.Drawing.Font("Arial Unicode MS", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoOrden.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.lblNoOrden.Location = New System.Drawing.Point(8, 24)
        Me.lblNoOrden.Name = "lblNoOrden"
        Me.lblNoOrden.Size = New System.Drawing.Size(61, 15)
        Me.lblNoOrden.TabIndex = 455
        Me.lblNoOrden.Text = "No. Orden"
        '
        'lblNoCono
        '
        Me.lblNoCono.Font = New System.Drawing.Font("Arial Unicode MS", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoCono.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.lblNoCono.Location = New System.Drawing.Point(8, 74)
        Me.lblNoCono.Name = "lblNoCono"
        Me.lblNoCono.Size = New System.Drawing.Size(56, 15)
        Me.lblNoCono.TabIndex = 402
        Me.lblNoCono.Text = "No. Cono"
        '
        'lblNoVisita
        '
        Me.lblNoVisita.Font = New System.Drawing.Font("Arial Unicode MS", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoVisita.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.lblNoVisita.Location = New System.Drawing.Point(8, 49)
        Me.lblNoVisita.Name = "lblNoVisita"
        Me.lblNoVisita.Size = New System.Drawing.Size(80, 15)
        Me.lblNoVisita.TabIndex = 386
        Me.lblNoVisita.Text = "No Visita"
        '
        'lblTipoOrdenO
        '
        Me.lblTipoOrdenO.Font = New System.Drawing.Font("Arial Unicode MS", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTipoOrdenO.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.lblTipoOrdenO.Location = New System.Drawing.Point(400, 49)
        Me.lblTipoOrdenO.Name = "lblTipoOrdenO"
        Me.lblTipoOrdenO.Size = New System.Drawing.Size(64, 15)
        Me.lblTipoOrdenO.TabIndex = 490
        Me.lblTipoOrdenO.Text = "Tipo Orden"
        '
        'lblPrioridad
        '
        Me.lblPrioridad.Font = New System.Drawing.Font("Arial Unicode MS", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrioridad.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.lblPrioridad.Location = New System.Drawing.Point(400, 74)
        Me.lblPrioridad.Name = "lblPrioridad"
        Me.lblPrioridad.Size = New System.Drawing.Size(51, 15)
        Me.lblPrioridad.TabIndex = 491
        Me.lblPrioridad.Text = "Prioridad"
        '
        'lblEstadoO
        '
        Me.lblEstadoO.Font = New System.Drawing.Font("Arial Unicode MS", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEstadoO.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.lblEstadoO.Location = New System.Drawing.Point(400, 24)
        Me.lblEstadoO.Name = "lblEstadoO"
        Me.lblEstadoO.Size = New System.Drawing.Size(64, 15)
        Me.lblEstadoO.TabIndex = 482
        Me.lblEstadoO.Text = "Estado"
        '
        'btnCerrar
        '
        Me.btnCerrar.BackgroundImage = CType(resources.GetObject("btnCerrar.BackgroundImage"), System.Drawing.Image)
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCerrar.Location = New System.Drawing.Point(12, 393)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(68, 22)
        Me.btnCerrar.TabIndex = 9145
        Me.btnCerrar.Text = "Cerrar"
        '
        'frmRendimientoxOrden
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(596, 428)
        Me.Controls.Add(Me.btnCerrar)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.dtgRepuestos)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmRendimientoxOrden"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "<SCG> Rendimiento por Orden"
        CType(Me.dtgRepuestos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Eventos"

    Private Sub picRepuesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picRepuesto.Click
        Call CargarBuscador(sender)
    End Sub

    Private Sub m_buOrdenes_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_buOrdenes.AppAceptar

        Try
            txtNoOrden.Text = Campo_Llave
            txtNoVisita.Text = Arreglo_Campos(1)
            txtPrioridad.Text = Arreglo_Campos(2)
            txtPlaca.Text = Arreglo_Campos(6)
            txtModelo.Text = Arreglo_Campos(7)
            txtMarca.Text = Arreglo_Campos(8)
            txtNoCono.Text = Arreglo_Campos(9)
            txtEstado.Text = Arreglo_Campos(10)
            txtTipoOrden.Text = Arreglo_Campos(11)
            CodCliente = Arreglo_Campos(12)

            Call m_dstRendimientoxOrden.Clear()
            dtgRepuestos.DataSource = Nothing

            If m_adpRendimientoxOrden.Fill(m_dstRendimientoxOrden, txtNoOrden.Text, CodCliente, txtNoVisita.Text) Then

                dtgRepuestos.DataSource = m_dstRendimientoxOrden.SCGTA_SP_RendimientoxOrden
            End If

        Catch ex As Exception
            Call ManejoErrores(ex, CompanyName, GlobalesUI.g_TipoSkin)
            'MsgBox(ex.Message)
        End Try


    End Sub

    Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub

    Private Sub frmRendimientoxOrden_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call estiloGridReprocesos(dtgRepuestos)

        m_dstRendimientoxOrden.SCGTA_SP_RendimientoxOrden.DefaultView.AllowDelete = False
        m_dstRendimientoxOrden.SCGTA_SP_RendimientoxOrden.DefaultView.AllowEdit = False
        m_dstRendimientoxOrden.SCGTA_SP_RendimientoxOrden.DefaultView.AllowNew = False
    End Sub

#End Region

#Region "Metodos"

    Private Sub CargarBuscador(ByVal sender As System.Object)

        Try
            With m_buOrdenes

                'Me.Cursor = Cursors.WaitCursor
                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                .SQL_Cnn = DATemp.ObtieneConexion
                .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorOrden

                .Titulos = My.Resources.ResourceUI.NoOrden & "," & My.Resources.ResourceUI.Visita & _
                "," & My.Resources.ResourceUI.Prioridad & "," & My.Resources.ResourceUI.FechaCompromiso & _
                "," & My.Resources.ResourceUI.FechaApertura & "," & My.Resources.ResourceUI.FechaCierre & _
                "," & My.Resources.ResourceUI.Placa & "," & My.Resources.ResourceUI.Modelo & _
                "," & My.Resources.ResourceUI.Marca & "," & My.Resources.ResourceUI.Cono & _
                "," & My.Resources.ResourceUI.Estado & "," & My.Resources.ResourceUI.TipoOrden & _
                "," & My.Resources.ResourceUI.CodCliente

                '"Orden, Visita,Prioridad,Fecha compromiso,Fecha apertura, Fecha cierre, Placa,Modelo,Marca,Cono,estado,TipoOrden,Cardcode"
                .Criterios = "noOrden, noVisita, prioridad,fecha_compromiso,fecha_apertura,fecha_cierre,placa,modelo,marca,cono,estado,TipoOrden,CardCode"
                .Tabla = "SCGTA_VW_Orden"
                .Where = ""
                .Activar_Buscador(sender)

            End With
        Catch ex As Exception
            Call ManejoErrores(ex, CompanyName, GlobalesUI.g_TipoSkin)
            'MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub estiloGridReprocesos(ByRef dtgRequisito As DataGrid)

        'Dim mensaje As String
        'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.
        'Declaraciones generales
        Dim tsRendimientoxOrden As New DataGridTableStyle

        Call dtgRequisito.TableStyles.Clear()

        Dim tcGastos As New DataGridLabelColumn
        Dim tcRendimiento As New DataGridColumnProgressBar
        Dim tcValorReal As New DataGridLabelColumn
        Dim tcValorOtorgado As New DataGridLabelColumn
        Dim tcAcumulado As New DataGridLabelColumn

        Try

            tsRendimientoxOrden.MappingName = m_dstRendimientoxOrden.SCGTA_SP_RendimientoxOrden.TableName

            With tcGastos
                .Width = 112 '120
                .HeaderText = ""
                .MappingName = mc_strGastos
                .ReadOnly = True
            End With

            With tcRendimiento
                .Width = 194 '200
                .HeaderText = My.Resources.ResourceUI.Rendimiento
                .MappingName = mc_strRendimiento
                '.ReadOnly = True
                .scgAllowEdit = False
                .scgMostrarValor = True
                .scgNegritaValor = True
                .scgLimiteAmarillo = 100
                .scgLimiteVerde = 75
            End With

            With tcValorReal
                .Width = 75
                .HeaderText = My.Resources.ResourceUI.ValorReal
                .MappingName = mc_strValorReal
                .ReadOnly = True
                .NullText = 0
                .Format = "#,##0.00"
            End With

            With tcValorOtorgado
                .Width = 81 '75
                .HeaderText = My.Resources.ResourceUI.ValorOtrogado
                .MappingName = mc_strValorOtorgado
                .ReadOnly = True
                .NullText = 0
                .Format = "#,##0.00"
            End With

            With tcAcumulado
                .Width = 75
                .HeaderText = My.Resources.ResourceUI.Acumulado
                .MappingName = mc_strAcumulado
                .ReadOnly = True
                .NullText = 0
                .Format = "#,##0.00"
            End With


            'Agrega las columnas al tableStyle
            ' tsReprocesos.GridColumnStyles.Add(tcNoReprocesoxOrden)

            With tsRendimientoxOrden

                .GridColumnStyles.Add(tcGastos)
                .GridColumnStyles.Add(tcRendimiento)
                .GridColumnStyles.Add(tcValorReal)
                .GridColumnStyles.Add(tcValorOtorgado)
                .GridColumnStyles.Add(tcAcumulado)

            End With


            With tsRendimientoxOrden

                .SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
                .SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                .HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
                .AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))

            End With


            'Establece propiedades del datagrid (colores estándares).

            'Hace que el datagrid adopte las propiedades del TableStyle.

            tsRendimientoxOrden.PreferredRowHeight = 39

            dtgRequisito.TableStyles.Add(tsRendimientoxOrden)

        Catch ex As Exception
            Call ManejoErrores(ex, CompanyName, GlobalesUI.g_TipoSkin)
            'MsgBox(ex.Message)
        End Try

    End Sub

#End Region

End Class
