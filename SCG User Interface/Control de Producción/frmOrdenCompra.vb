'Option Strict On
'Option Explicit On 

Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Class frmOrdenCompra
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
        Public WithEvents lblCliente As System.Windows.Forms.Label
        Friend WithEvents btnAceptar As System.Windows.Forms.Button
        Friend WithEvents btnCancelar As System.Windows.Forms.Button
        Friend WithEvents dtgRepuestos As System.Windows.Forms.DataGrid
        Friend WithEvents DataGridTableStyle1 As System.Windows.Forms.DataGridTableStyle
        Friend WithEvents DataGridTextBoxColumn1 As System.Windows.Forms.DataGridTextBoxColumn
        Friend WithEvents DataGridTextBoxColumn2 As System.Windows.Forms.DataGridTextBoxColumn
        Friend WithEvents DataGridTextBoxColumn3 As System.Windows.Forms.DataGridTextBoxColumn
        Friend WithEvents DataGridTextBoxColumn4 As System.Windows.Forms.DataGridTextBoxColumn
        Friend WithEvents grbDatosAuto As System.Windows.Forms.GroupBox
        Public WithEvents lblMarcayModelo As System.Windows.Forms.Label
        Public WithEvents lblNoChasis As System.Windows.Forms.Label
        Public WithEvents lblAnio As System.Windows.Forms.Label
        Friend WithEvents txtDetalle As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Friend WithEvents lblRepuesto As System.Windows.Forms.Label
        Friend WithEvents txtProveedor As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picProveedor As System.Windows.Forms.PictureBox
        Friend WithEvents btnPedidoEspecial As System.Windows.Forms.Button
        Friend WithEvents SubBProveedor As Buscador.SubBuscador
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrdenCompra))
            Me.lblCliente = New System.Windows.Forms.Label()
            Me.btnCancelar = New System.Windows.Forms.Button()
            Me.btnAceptar = New System.Windows.Forms.Button()
            Me.dtgRepuestos = New System.Windows.Forms.DataGrid()
            Me.DataGridTableStyle1 = New System.Windows.Forms.DataGridTableStyle()
            Me.DataGridTextBoxColumn1 = New System.Windows.Forms.DataGridTextBoxColumn()
            Me.DataGridTextBoxColumn2 = New System.Windows.Forms.DataGridTextBoxColumn()
            Me.DataGridTextBoxColumn3 = New System.Windows.Forms.DataGridTextBoxColumn()
            Me.DataGridTextBoxColumn4 = New System.Windows.Forms.DataGridTextBoxColumn()
            Me.grbDatosAuto = New System.Windows.Forms.GroupBox()
            Me.lblAnio = New System.Windows.Forms.Label()
            Me.lblNoChasis = New System.Windows.Forms.Label()
            Me.lblMarcayModelo = New System.Windows.Forms.Label()
            Me.txtDetalle = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtProveedor = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblLine1 = New System.Windows.Forms.Label()
            Me.lblRepuesto = New System.Windows.Forms.Label()
            Me.picProveedor = New System.Windows.Forms.PictureBox()
            Me.SubBProveedor = New Buscador.SubBuscador()
            Me.btnPedidoEspecial = New System.Windows.Forms.Button()
            CType(Me.dtgRepuestos, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grbDatosAuto.SuspendLayout()
            CType(Me.picProveedor, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblCliente
            '
            resources.ApplyResources(Me.lblCliente, "lblCliente")
            Me.lblCliente.ForeColor = System.Drawing.Color.Black
            Me.lblCliente.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblCliente.Name = "lblCliente"
            '
            'btnCancelar
            '
            resources.ApplyResources(Me.btnCancelar, "btnCancelar")
            Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancelar.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.btnCancelar.Name = "btnCancelar"
            '
            'btnAceptar
            '
            resources.ApplyResources(Me.btnAceptar, "btnAceptar")
            Me.btnAceptar.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.btnAceptar.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.btnAceptar.Name = "btnAceptar"
            Me.btnAceptar.UseVisualStyleBackColor = False
            '
            'dtgRepuestos
            '
            resources.ApplyResources(Me.dtgRepuestos, "dtgRepuestos")
            Me.dtgRepuestos.BackColor = System.Drawing.Color.White
            Me.dtgRepuestos.BackgroundColor = System.Drawing.Color.White
            Me.dtgRepuestos.CaptionText = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgRepuestos.CaptionVisible = False
            Me.dtgRepuestos.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgRepuestos.HeaderForeColor = System.Drawing.SystemColors.ControlText
            Me.dtgRepuestos.Name = "dtgRepuestos"
            Me.dtgRepuestos.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.DataGridTableStyle1})
            '
            'DataGridTableStyle1
            '
            Me.DataGridTableStyle1.DataGrid = Me.dtgRepuestos
            Me.DataGridTableStyle1.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn1, Me.DataGridTextBoxColumn2, Me.DataGridTextBoxColumn3, Me.DataGridTextBoxColumn4})
            resources.ApplyResources(Me.DataGridTableStyle1, "DataGridTableStyle1")
            Me.DataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText
            '
            'DataGridTextBoxColumn1
            '
            resources.ApplyResources(Me.DataGridTextBoxColumn1, "DataGridTextBoxColumn1")
            Me.DataGridTextBoxColumn1.Format = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.DataGridTextBoxColumn1.FormatInfo = Nothing
            Me.DataGridTextBoxColumn1.MappingName = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'DataGridTextBoxColumn2
            '
            resources.ApplyResources(Me.DataGridTextBoxColumn2, "DataGridTextBoxColumn2")
            Me.DataGridTextBoxColumn2.Format = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.DataGridTextBoxColumn2.FormatInfo = Nothing
            Me.DataGridTextBoxColumn2.MappingName = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'DataGridTextBoxColumn3
            '
            resources.ApplyResources(Me.DataGridTextBoxColumn3, "DataGridTextBoxColumn3")
            Me.DataGridTextBoxColumn3.Format = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.DataGridTextBoxColumn3.FormatInfo = Nothing
            Me.DataGridTextBoxColumn3.MappingName = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'DataGridTextBoxColumn4
            '
            resources.ApplyResources(Me.DataGridTextBoxColumn4, "DataGridTextBoxColumn4")
            Me.DataGridTextBoxColumn4.Format = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.DataGridTextBoxColumn4.FormatInfo = Nothing
            Me.DataGridTextBoxColumn4.MappingName = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'grbDatosAuto
            '
            resources.ApplyResources(Me.grbDatosAuto, "grbDatosAuto")
            Me.grbDatosAuto.Controls.Add(Me.lblAnio)
            Me.grbDatosAuto.Controls.Add(Me.lblNoChasis)
            Me.grbDatosAuto.Controls.Add(Me.lblMarcayModelo)
            Me.grbDatosAuto.Name = "grbDatosAuto"
            Me.grbDatosAuto.TabStop = False
            '
            'lblAnio
            '
            resources.ApplyResources(Me.lblAnio, "lblAnio")
            Me.lblAnio.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblAnio.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblAnio.Name = "lblAnio"
            '
            'lblNoChasis
            '
            resources.ApplyResources(Me.lblNoChasis, "lblNoChasis")
            Me.lblNoChasis.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoChasis.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblNoChasis.Name = "lblNoChasis"
            '
            'lblMarcayModelo
            '
            resources.ApplyResources(Me.lblMarcayModelo, "lblMarcayModelo")
            Me.lblMarcayModelo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblMarcayModelo.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblMarcayModelo.Name = "lblMarcayModelo"
            '
            'txtDetalle
            '
            resources.ApplyResources(Me.txtDetalle, "txtDetalle")
            Me.txtDetalle.AceptaNegativos = False
            Me.txtDetalle.BackColor = System.Drawing.Color.White
            Me.txtDetalle.EstiloSBO = True
            Me.txtDetalle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.txtDetalle.MaxDecimales = 0
            Me.txtDetalle.MaxEnteros = 0
            Me.txtDetalle.Millares = False
            Me.txtDetalle.Name = "txtDetalle"
            Me.txtDetalle.Size_AdjustableHeight = 45
            Me.txtDetalle.TeclasDeshacer = True
            Me.txtDetalle.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtProveedor
            '
            resources.ApplyResources(Me.txtProveedor, "txtProveedor")
            Me.txtProveedor.AceptaNegativos = False
            Me.txtProveedor.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtProveedor.EstiloSBO = True
            Me.txtProveedor.MaxDecimales = 0
            Me.txtProveedor.MaxEnteros = 0
            Me.txtProveedor.Millares = False
            Me.txtProveedor.Name = "txtProveedor"
            Me.txtProveedor.ReadOnly = True
            Me.txtProveedor.Size_AdjustableHeight = 20
            Me.txtProveedor.TeclasDeshacer = True
            Me.txtProveedor.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblLine1
            '
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.lblLine1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine1.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblLine1.Name = "lblLine1"
            '
            'lblRepuesto
            '
            resources.ApplyResources(Me.lblRepuesto, "lblRepuesto")
            Me.lblRepuesto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblRepuesto.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblRepuesto.Name = "lblRepuesto"
            '
            'picProveedor
            '
            resources.ApplyResources(Me.picProveedor, "picProveedor")
            Me.picProveedor.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picProveedor.Name = "picProveedor"
            Me.picProveedor.TabStop = False
            '
            'SubBProveedor
            '
            resources.ApplyResources(Me.SubBProveedor, "SubBProveedor")
            Me.SubBProveedor.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.SubBProveedor.Barra_Titulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBProveedor.ConsultarDBPorFiltrado = False
            Me.SubBProveedor.Criterios = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBProveedor.Criterios_Ocultos = 0
            Me.SubBProveedor.Criterios_OcultosEx = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBProveedor.IN_DataTable = Nothing
            Me.SubBProveedor.MultiSeleccion = False
            Me.SubBProveedor.Name = "SubBProveedor"
            Me.SubBProveedor.SQL_Cnn = Nothing
            Me.SubBProveedor.Tabla = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBProveedor.Titulos = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBProveedor.Where = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'btnPedidoEspecial
            '
            resources.ApplyResources(Me.btnPedidoEspecial, "btnPedidoEspecial")
            Me.btnPedidoEspecial.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.btnPedidoEspecial.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.btnPedidoEspecial.Name = "btnPedidoEspecial"
            Me.btnPedidoEspecial.UseVisualStyleBackColor = False
            '
            'frmOrdenCompra
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.CancelButton = Me.btnCancelar
            Me.Controls.Add(Me.btnPedidoEspecial)
            Me.Controls.Add(Me.SubBProveedor)
            Me.Controls.Add(Me.picProveedor)
            Me.Controls.Add(Me.txtProveedor)
            Me.Controls.Add(Me.lblLine1)
            Me.Controls.Add(Me.lblRepuesto)
            Me.Controls.Add(Me.txtDetalle)
            Me.Controls.Add(Me.grbDatosAuto)
            Me.Controls.Add(Me.dtgRepuestos)
            Me.Controls.Add(Me.btnCancelar)
            Me.Controls.Add(Me.btnAceptar)
            Me.Controls.Add(Me.lblCliente)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmOrdenCompra"
            CType(Me.dtgRepuestos, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grbDatosAuto.ResumeLayout(False)
            CType(Me.picProveedor, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region "Declaraciones"
        Private m_dstRepuestosProveeduria As New RepuestosProveduriaDataset
        Private m_adpRepuestosProveeduria As New RepuestosProveeduriaDataAdapter
        Private m_dtbRepuestosxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable
        Private objUtilitarios As New SCGDataAccess.Utilitarios(strConectionString)

        Private Const mc_strPkRepuestoxOrdenesdeCompraPro As String = "PkRepuestoxOrdenesdeCompraPro"
        Private Const mc_strNoRepuesto As String = "NoRepuesto"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strFechaSolicitud As String = "FechaSolicitud"
        Private Const mc_strFechaCompromiso As String = "FechaCompromiso"
        Private Const mc_strFechaEntrega As String = "FechaEntrega"
        Private Const mc_strCardCode As String = "CardCode"
        Private Const mc_strCantSolicitados As String = "CantSolicitados"
        Private Const mc_strCantSuministrados As String = "CantSuministrados"
        Private Const mc_strNoAdicional As String = "NoAdicional"
        Private Const mc_strNoOrdendeCompra As String = "NoOrdendeCompra"
        Private Const mc_strNoFactura As String = "NoFactura"
        Private Const mc_strCostoRepuesto As String = "CostoRepuesto"
        Private Const mc_strPrecioCompraReal As String = "PrecioCompraReal"
        Private Const mc_strPrecioCompraDesc As String = "MontoDesc"
        Private Const mc_strDescuento As String = "Descuento"
        Private Const mc_strDescRepuesto As String = "Descripcion Rep"
        Private Const mc_strSeccion As String = "Seccion"
        Private Const mc_strBodegaProceso As String = "BodegaProceso"
        Private Const mc_strIDSerieDocumentosCompra As String = "IDSerieDocumentosCompra"
        Private Const mc_strIDSerieOfertaCompra As String = "IDSerieOfertaCompra"
        Private Const mc_strHoraSolicitud As String = "HoraSolicitud"

        Private Const mc_intCommit As Integer = 0
        Private Const mc_intRollBack As Integer = 1

        Private m_buscador As New Buscador.SubBuscador

        'Variables
        Private m_strMarca As String
        Private m_intTipo As Integer
        Private m_intTipoArt As Integer
        Private m_strModelo As String
        Private m_intAnio As Integer
        Private m_strNoChasis As String
        Private m_intEstadoSelec As Integer
        Private m_intNoCotizacion As Integer
        Private m_Ok As Boolean
        Private m_strCodMarca As String
        Private m_strEstilo As String
        Private m_strPlaca As String
        Private m_strAsesor As String
#End Region

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        Public Sub New(ByVal dtbRepuestosxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable, _
                       ByVal Marca As String, _
                       ByVal Modelo As String, _
                       ByVal Anio As Integer, _
                       ByVal NoChasis As String, _
                       ByVal TipoArticulo As Integer, _
                       ByVal NoOrden As String, _
                       ByVal p_intEstadoSelec As Integer, _
                       ByVal p_intNoCotizacion As Integer, _
                       ByVal p_strCodMarca As String, _
                       ByVal p_strEstilo As String, _
                       ByVal p_strPlaca As String, _
                       ByVal p_strAsesor As String, _
                       ByVal p_intTipo As Integer)

            MyBase.New()

            m_strMarca = Marca
            m_strModelo = Modelo
            m_intAnio = Anio
            m_strNoChasis = NoChasis
            m_intEstadoSelec = p_intEstadoSelec
            m_intNoCotizacion = p_intNoCotizacion
            m_strCodMarca = p_strCodMarca
            m_strEstilo = p_strEstilo
            m_strPlaca = p_strPlaca
            m_strAsesor = p_strAsesor
            m_intTipo = p_intTipo
            m_intTipoArt = TipoArticulo

            'This call is required by the Windows Form Designer.
            InitializeComponent()
            'Add any initialization after the InitializeComponent() call
            m_dtbRepuestosxOrden = dtbRepuestosxOrden

            If TipoArticulo = 4 Then
                lblCliente.Text = lblCliente.Text.Replace("repuestos", "Servicios Externos")
            End If

        End Sub

#End Region

#Region "Eventos"

        Private Sub frmOrdenCompra_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Alejandra 16/05/06 El combo fue sustituido por un Buscador
            'Call objUtilitarios.CargarCombos(cboProveedores, 28)
            'cboProveedores.SelectedIndex = 0

            lblMarcayModelo.Text &= m_strMarca & "/" & m_strModelo
            lblNoChasis.Text &= m_strNoChasis
            lblAnio.Text &= CStr(m_intAnio)

            If AgregaFilasRepuestosProveeduria(m_dtbRepuestosxOrden, _
                                               m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria) Then

                'm_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.DefaultView.AllowEdit = False
                m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.DefaultView.AllowNew = False
                m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.DefaultView.AllowDelete = False

                dtgRepuestos.DataSource = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria

                Call EstiloGridRepuestos()

            End If

            'AddHandler m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.SCGTA_TB_RepuestosxOrden_ProveduriaRowChanging, _
            'AddressOf CambiaFila

        End Sub

        'Private Sub CambiaFila(ByVal Sender As Object, ByVal e As DMSOneFramework.RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRowChangeEvent)
        '    Try
        '        If e.Row.PkRepuestoxOrdenesdeCompraPro = 1 Then

        '            e.Row.EndEdit()

        '        End If
        '    Catch ex As Exception
        '        MsgBox(ex.Message)
        '    End Try
        'End Sub
        Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click

            ''No se usa Try...Catch porque está en el método
            If objUtilitarios.TraerTipoCompra Then
                GenerarOfertaCompra(False)
            Else
                GenerarOrdenCompra(False)

            End If

        End Sub

        Private Function ValidarCodigosEspecificos() As Boolean
            Dim drwRepuestos As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow
            Dim blnCodigosValidos As Boolean = True
            For Each drwRepuestos In m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Rows
                If drwRepuestos.IsCodEspecificoNull Or drwRepuestos.CodEspecifico = "" Then
                    blnCodigosValidos = False
                    Exit For
                End If
            Next
            Return blnCodigosValidos
        End Function

        Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
            Me.m_Ok = False
            Me.Close()
        End Sub

        'Private Sub picRepuesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picProveedor.Click

        '    'm_buscador.IN_DataTable= 

        '    'Call m_buscador.Show()

        'End Sub

        'Alejandra 16/05/06 Se agregó un buscador de proveedores
        Private Sub picProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picProveedor.Click
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                SubBProveedor.SQL_Cnn = DATemp.ObtieneConexion
                SubBProveedor.Barra_Titulo = My.Resources.ResourceUI.busBarraTitulosBuscadorProveedores
                SubBProveedor.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre
                SubBProveedor.Criterios = "CardCode, CardName"
                SubBProveedor.Tabla = "SCGTA_VW_Proveedores"
                SubBProveedor.Where = ""
                SubBProveedor.Activar_Buscador(sender)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub



        Private Sub SubBProveedor_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles SubBProveedor.AppAceptar
            Try
                txtProveedor.Text = Arreglo_Campos(1)
                txtProveedor.Tag = Arreglo_Campos(0)
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnPedidoEspecial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPedidoEspecial.Click

            ''No se usa Try...Catch porque está en el método
            If objUtilitarios.TraerTipoCompra Then
                GenerarOfertaCompra(True)
            Else
                GenerarOrdenCompra(True)

            End If
        End Sub

#End Region

#Region "Metodos"

        Private Function DebeGenerarXML(ByVal p_strCodMarca As String, ByVal p_strCardCodeProveedor As String) As Boolean

            Dim blnDebeGenerarXML As Boolean = False

            Dim adpConfRepuestosXMarca As New ConfCatalogoRepXMarcaDataAdapter
            Dim dtsConfRepuestosXMarca As New ConfCatalogoRepXMarcaDataset
            Dim drwConfRepuestosXMarca As ConfCatalogoRepXMarcaDataset.SCGTA_TB_ConfCatalogoRepxMarcaRow
            Dim ProveedoresXMarca As New ProveedorXMarcaDataset.SCGTB_TA_ProveedorXMarcaDataTable
            Dim Proveedor As ProveedorXMarcaDataset.SCGTB_TA_ProveedorXMarcaRow
            Dim ProveedoresXMarcaAdapter As New ProveedorXMarcaDatasetTableAdapters.SCGTB_TA_ProveedorXMarcaTableAdapter
            Dim cnnSCGTaller As New SqlClient.SqlConnection

            If cnnSCGTaller.State = ConnectionState.Closed Then
                If cnnSCGTaller.ConnectionString = "" Then
                    cnnSCGTaller.ConnectionString = strConexionADO
                End If
                Call cnnSCGTaller.Open()
            End If
            ProveedoresXMarcaAdapter.Connection = cnnSCGTaller

            If g_blnCatalogosExternos And g_strDireccionB2B <> "" Then

                adpConfRepuestosXMarca.Fill(dtsConfRepuestosXMarca, , p_strCodMarca)
                If dtsConfRepuestosXMarca.SCGTA_TB_ConfCatalogoRepxMarca.Rows.Count > 0 Then

                    drwConfRepuestosXMarca = dtsConfRepuestosXMarca.SCGTA_TB_ConfCatalogoRepxMarca.Rows(0)
                    ProveedoresXMarcaAdapter.Fill(ProveedoresXMarca, drwConfRepuestosXMarca.ID)
                    For Each Proveedor In ProveedoresXMarca.Rows
                        If Proveedor.CardCodeProveedor = p_strCardCodeProveedor Then
                            blnDebeGenerarXML = True
                        End If
                    Next


                End If

            End If

            Return blnDebeGenerarXML

        End Function

        Private Function AgregaFilasRepuestosProveeduria(ByVal dtbRepuestosxOrdenseleccionados As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable, _
                                                         ByRef dtbRepuestosProveeduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaDataTable) As Boolean

            Try
                Dim drwRepuestosProveeduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow
                Dim drwRepuestosxOrdenSeleccionados As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
                Const strCriterio As String = "Check=true and CodEstadoLinea <> 3" 'and Trasladado <> 0"


                For Each drwRepuestosxOrdenSeleccionados In dtbRepuestosxOrdenseleccionados.Select(strCriterio)

                    'drwRepuestosProveeduria.IsFechaCompromisoNull()

                    drwRepuestosProveeduria = dtbRepuestosProveeduria.NewSCGTA_TB_RepuestosxOrden_ProveduriaRow

                    If Not drwRepuestosxOrdenSeleccionados.IsCantidadPendienteNull Then
                        drwRepuestosProveeduria.CantSolicitados = drwRepuestosxOrdenSeleccionados.CantidadPendiente
                    Else
                        drwRepuestosProveeduria.CantSolicitados = drwRepuestosxOrdenSeleccionados.CantidadEstado
                    End If

                    drwRepuestosProveeduria.NoOrden = drwRepuestosxOrdenSeleccionados.NoOrden
                    drwRepuestosProveeduria.NoRepuesto = drwRepuestosxOrdenSeleccionados.NoRepuesto
                    'Agregado 06/07/06. Alejandra. Para que no afecte si la descripcion del componente es null
                    If drwRepuestosxOrdenSeleccionados.IsItemnameNull Then
                        drwRepuestosProveeduria.Descripcion_Rep = ""
                    Else
                        drwRepuestosProveeduria.Descripcion_Rep = drwRepuestosxOrdenSeleccionados.Itemname
                    End If
                    ''''''''''''
                    'drwRepuestosProveeduria.FechaCompromiso = System.DateTime.Now.Today
                    'drwRepuestosProveeduria.FechaEntrega = System.DateTime.Now.Today
                    drwRepuestosProveeduria.FechaSolicitud = System.DateTime.Now
                    drwRepuestosProveeduria.HoraSolicitud = System.DateTime.Now

                    drwRepuestosProveeduria.idRepuestosxOrden = drwRepuestosxOrdenSeleccionados.ID

                    If Not drwRepuestosxOrdenSeleccionados.IsItemCodeEspecificoNull Then
                        drwRepuestosProveeduria.CodEspecifico = drwRepuestosxOrdenSeleccionados.ItemCodeEspecifico
                        If Not drwRepuestosxOrdenSeleccionados.IsItemNameEspecificoNull Then
                            drwRepuestosProveeduria.NomEspecifico = drwRepuestosxOrdenSeleccionados.ItemNameEspecifico
                        End If
                    End If

                    Call dtbRepuestosProveeduria.AddSCGTA_TB_RepuestosxOrden_ProveduriaRow(drwRepuestosProveeduria)

                Next drwRepuestosxOrdenSeleccionados

                Return True
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                ' MsgBox("Metodo: AgregaFilasRepuestosProveeduria" & " " & ex.Message)
                Return False
            Finally

            End Try

        End Function

        Private Sub EstiloGridRepuestos()

            'Dim mensaje As String
            'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.

            'Declaraciones generales
            Dim tsRepuestosProv As New DataGridTableStyle

            Call dtgRepuestos.TableStyles.Clear()

            Dim tcPkRepuestoxOrdenesdeCompraPro As New DataGridTextBoxColumn
            Dim tcNoRepuesto As New DataGridTextBoxColumn
            Dim tcNoOrden As New DataGridTextBoxColumn
            Dim tcFechaSolicitud As New DataGridTextBoxColumn
            Dim tcFechaCompromiso As New DataGridTextBoxColumn
            Dim tcFechaEntrega As New DataGridTextBoxColumn
            Dim tcCardCode As New DataGridTextBoxColumn
            Dim tcCantSolicitados As New DataGridTextBoxColumn
            Dim tcCantSuministrados As New DataGridTextBoxColumn
            Dim tcNoAdicional As New DataGridTextBoxColumn
            Dim tcNoOrdendeCompra As New DataGridTextBoxColumn
            Dim tcNoFactura As New DataGridTextBoxColumn
            Dim tcCostoRepuesto As New DataGridTextBoxColumn
            Dim tcPrecioCompraReal As New DataGridTextBoxColumn
            Dim tcPrecioCompraDesc As New DataGridTextBoxColumn
            Dim tcDescuento As New DataGridTextBoxColumn
            Dim tcDescRepuesto As New DataGridTextBoxColumn
            Dim tcHoraSolicitud As New DataGridTextBoxColumn
            'Agregado 12/06/06. Alejandra. Muestra la seccion del repuesto
            Dim tcSeccion As New DataGridTextBoxColumn


            Try

                tsRepuestosProv.MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.TableName

                With tcPkRepuestoxOrdenesdeCompraPro
                    .Width = 0
                    .HeaderText = ""
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strPkRepuestoxOrdenesdeCompraPro).ColumnName
                End With

                With tcNoRepuesto
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.NoRepuesto
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strNoRepuesto).ColumnName

                End With

                With tcDescRepuesto
                    .Width = 373
                    .HeaderText = My.Resources.ResourceUI.DescRepuesto
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strDescRepuesto).ColumnName
                    .ReadOnly = True
                End With

                'Agregado 12/06/06. Alejandra. Muestra la seccion del repuesto
                With tcSeccion
                    .Width = 140
                    .HeaderText = My.Resources.ResourceUI.Seccion
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strSeccion).ColumnName
                    .ReadOnly = True
                End With


                With tcNoOrden
                    .Width = 48
                    .HeaderText = My.Resources.ResourceUI.NoOrden
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strNoOrden).ColumnName
                    .Format = "###"
                End With



                With tcCardCode
                    .Width = 75
                    .HeaderText = My.Resources.ResourceUI.CodProveedor
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strCardCode).ColumnName
                    .Format = "###"
                End With

                With tcFechaSolicitud
                    .Width = 97
                    .HeaderText = My.Resources.ResourceUI.FechaSolicitud
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strFechaSolicitud).ColumnName
                    .Format = "dd/MM/yyyy"

                    .ReadOnly = True
                End With

                With tcHoraSolicitud
                    .Width = 97
                    .HeaderText = My.Resources.ResourceUI.HoraSolicitud
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strHoraSolicitud).ColumnName
                    .Format = "hh:mm tt"
                    .ReadOnly = True
                End With


                With tcFechaCompromiso
                    .Width = 85
                    .HeaderText = My.Resources.ResourceUI.FechaCompromiso
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strFechaCompromiso).ColumnName
                End With

                With tcNoAdicional
                    .Width = 75
                    .HeaderText = My.Resources.ResourceUI.NoAdicional
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strNoAdicional).ColumnName
                End With


                With tcNoOrdendeCompra
                    .Width = 75
                    .HeaderText = My.Resources.ResourceUI.NoOrdenCompra
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strNoOrdendeCompra).ColumnName
                End With

                With tcNoFactura
                    .Width = 75
                    .HeaderText = My.Resources.ResourceUI.NoFactura
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strNoFactura).ColumnName
                End With

                With tcCostoRepuesto
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.CostoRepuesto
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strCostoRepuesto).ColumnName
                End With

                With tcPrecioCompraReal
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.PrecioCompra
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strPrecioCompraReal).ColumnName
                    .ReadOnly = False
                End With

                With tcPrecioCompraDesc
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.CompraDescuento
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strPrecioCompraDesc).ColumnName

                End With

                With tcDescuento
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.Descuento
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strDescuento).ColumnName
                End With

                With tcCantSolicitados
                    .Width = 65
                    .HeaderText = My.Resources.ResourceUI.Solicitados
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strCantSolicitados).ColumnName
                    .ReadOnly = True
                End With


                'Agrega las columnas al tableStyle

                With tsRepuestosProv.GridColumnStyles

                    '.Add(tcPkRepuestoxOrdenesdeCompraPro)
                    '.Add(tcNoRepuesto)
                    '.Add(tcNoOrden)
                    .Add(tcDescRepuesto)
                    'Agregado 12/06/06. Alejandra. Muestra la seccion del repuesto
                    '.Add(tcSeccion)
                    .Add(tcFechaSolicitud)
                    .Add(tcHoraSolicitud)
                    '.Add(tcFechaCompromiso)
                    '.Add(tcFechaEntrega)
                    '.Add(tcCardCode)
                    .Add(tcCantSolicitados)
                    '.Add(tcCantSuministrados)
                    '.Add(tcNoAdicional)
                    '.Add(tcNoOrdendeCompra)
                    '.Add(tcNoFactura)
                    '.Add(tcCostoRepuesto)
                    .Add(tcPrecioCompraReal)
                    '.Add(tcPrecioCompraDesc)
                    '.Add(tcDescuento)


                End With

                'Establece propiedades del datagrid (colores estándares).
                tsRepuestosProv.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
                tsRepuestosProv.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                tsRepuestosProv.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
                tsRepuestosProv.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))

                'Hace que el datagrid adopte las propiedades del TableStyle.
                dtgRepuestos.TableStyles.Add(tsRepuestosProv)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Function AsignaProveedoryidPedidoenSAPaTarcking(ByRef dtbRepuestosProveeduria As  _
                                                                RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaDataTable, _
                                                                ByVal CodProveedor As String, _
                                                                ByVal NopedidoEnSap As String, _
                                                                ByVal Observaciones As String) As Boolean

            Try

                Dim drwRepuestosProveeduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow

                For Each drwRepuestosProveeduria In dtbRepuestosProveeduria.Rows

                    drwRepuestosProveeduria.CardCode = CodProveedor
                    drwRepuestosProveeduria.NoOrdendeCompra = NopedidoEnSap
                    drwRepuestosProveeduria.Observaciones = Observaciones

                Next drwRepuestosProveeduria
                Return True
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
                Return False
            Finally

            End Try

        End Function

        Private Function GuardarTrackingRepuestos(ByVal p_dstRepuestosProveeduria As RepuestosProveduriaDataset, _
                                                  ByVal p_adpRepuestosProveeduria As RepuestosProveeduriaDataAdapter) As Integer

            Try

                If Not p_dstRepuestosProveeduria Is Nothing AndAlso p_dstRepuestosProveeduria.HasChanges Then

                    Return p_adpRepuestosProveeduria.Update(p_dstRepuestosProveeduria)

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally

            End Try

        End Function

        Private Sub AgregarRepuestosSBO(ByVal dtbRepuestos As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaDataTable)
            'Agregado 26/05/06. Alejandra
            Dim intIndice As Integer
            Dim adpRepuestos As New SCGDataAccess.ClsRepuestosSBO


            Try
                For intIndice = 0 To dtbRepuestos.Rows.Count - 1

                    adpRepuestos.agregarRepuesto(dtbRepuestos(intIndice).NoRepuesto, dtbRepuestos(intIndice).Descripcion_Rep, COMPANIA, strDATABASESCG)

                Next
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub CreaEstadoSolicitado(ByVal NoOrden As String, _
                                              ByVal dtbRepuestosxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable)

            Try

                Dim adpRepuestosxEstado As New RepuestosxEstadoDataAdapter
                Dim dstRepuestosxEstado As New EstadoxRepuestosDataset
                Dim drwRepuestxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
                Dim drwRepuestosxEstado As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow
                Const strCriterio As String = "Check=true and CodEstadoLinea <> 3" 'and Trasladado <> 0"
                Dim intCantSolicitaAnterior As Integer
                Dim blnExisten As Boolean = False

                Call adpRepuestosxEstado.Fill(dstRepuestosxEstado, NoOrden)

                If dtbRepuestosxOrden.Select(strCriterio).Length <> 0 Then

                    Call MetodosCompartidosSBOCls.IniciarCotizacion(m_intNoCotizacion)

                End If

                For Each drwRepuestxOrden In dtbRepuestosxOrden.Select(strCriterio)

                    drwRepuestosxEstado = Nothing

                    If m_intEstadoSelec = 0 Then
                        drwRepuestosxEstado = dstRepuestosxEstado.SCGTA_TB_RepuestosxEstado.FindByIdRepuestosxOrdenCodEstadoRep(drwRepuestxOrden.ID, 1)
                    Else
                        drwRepuestosxEstado = dstRepuestosxEstado.SCGTA_TB_RepuestosxEstado.FindByIdRepuestosxOrdenCodEstadoRep(drwRepuestxOrden.ID, drwRepuestxOrden.CodEstadoRep)
                    End If

                    If Not drwRepuestosxEstado Is Nothing Then

                        Call drwRepuestosxEstado.Delete()

                        drwRepuestosxEstado = dstRepuestosxEstado.SCGTA_TB_RepuestosxEstado.FindByIdRepuestosxOrdenCodEstadoRep(drwRepuestxOrden.ID, 2)

                        If Not drwRepuestosxEstado Is Nothing Then

                            intCantSolicitaAnterior = drwRepuestosxEstado.Cantidad

                            Call drwRepuestosxEstado.Delete()

                        End If

                        drwRepuestosxEstado = dstRepuestosxEstado.SCGTA_TB_RepuestosxEstado.NewSCGTA_TB_RepuestosxEstadoRow
                        drwRepuestosxEstado.IdRepuestosxOrden = drwRepuestxOrden.ID
                        drwRepuestosxEstado.CodEstadoRep = 2

                        If m_intEstadoSelec = 0 Then
                            drwRepuestosxEstado.Cantidad = drwRepuestxOrden.CantidadPendiente + intCantSolicitaAnterior
                        Else
                            drwRepuestosxEstado.Cantidad = drwRepuestxOrden.CantidadEstado + intCantSolicitaAnterior
                        End If

                        Call dstRepuestosxEstado.SCGTA_TB_RepuestosxEstado.AddSCGTA_TB_RepuestosxEstadoRow(drwRepuestosxEstado)

                        blnExisten = MetodosCompartidosSBOCls.ActualizarItemsCotizacionEstadoTrasl(drwRepuestxOrden)

                    End If

                Next drwRepuestxOrden

                If blnExisten Then
                    Call MetodosCompartidosSBOCls.ActualizarCotizacion()
                End If

                Call adpRepuestosxEstado.Update(dstRepuestosxEstado)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            Finally

            End Try

        End Sub

        Private Function RevisarBodegasCorrectas(ByVal p_dstRepuestos As RepuestosProveduriaDataset) As Boolean
            Dim drwRepuesto As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow
            Dim objTransacciones As New DMSOneFramework.SCGBusinessLogic.TransferenciaItems(G_objCompany)
            Dim blnResult As Boolean = True
            Dim strBodega As String

            For Each drwRepuesto In p_dstRepuestos.SCGTA_TB_RepuestosxOrden_Proveduria

                strBodega = objTransacciones.RetornaBodegaProcesoByItem(drwRepuesto.NoRepuesto)

                If String.IsNullOrEmpty(strBodega) Then
                    blnResult = False
                End If

            Next

            objTransacciones.CerrarConexion()

            Return blnResult

        End Function

        Private Sub GenerarOrdenCompra(ByVal p_blnTipoEspecial As Boolean)

            Try
                'Dim intCantidadDeRegistrosEnTracking As Integer
                Dim NoPedidoEnSap As String = ""
                Dim strSerie As String = ""
                Dim intDocNum As Integer
                Dim blnDebeGenerarXML As Boolean
                Dim blnGenerarOrdenCompra As Boolean = True
                Dim adpMensajeria As New MensajeriaSBOTallerDataAdapter
                Dim strNoBodega As String = ""
                Dim objTransferencia As New TransferenciaItems(G_objCompany)
                Dim strCentroBeneficio As String = String.Empty

                Dim strDetalle As String = String.Empty

                strDetalle = txtDetalle.Text

                strNoBodega = objTransferencia.RetornaBodegaProcesoByTipoOrden(m_intTipo)
                strCentroBeneficio = objTransferencia.RetornaCentroBeneficioByTipoOrden(m_intTipo)

                If Not RevisarBodegasCorrectas(m_dstRepuestosProveeduria) Then
                    Call objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeOCnopuedeCrearsepor & vbCrLf & _
                                                            My.Resources.ResourceUI.MensajeNoExisteBodega & vbCrLf & _
                                                            My.Resources.ResourceUI.MensajeLaBodegaNotieneValorValido)

                Else

                    If Not ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, _
                                                                              mc_strIDSerieDocumentosCompra, _
                                                                              strSerie) Then

                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeOCnopuedeCrearsepor & vbCrLf & _
                                                            My.Resources.ResourceUI.MensajeNoTieneSeries & vbCrLf & _
                                                            My.Resources.ResourceUI.MensajeSerieValorNoValido)

                    Else

                        If txtProveedor.Text <> "" Then 'Seleccionar un proveedor para generar la orden


                            If m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Rows.Count > 0 Then

                                Call MetodosCompartidosSBOCls.IniciarCotizacion(m_intNoCotizacion)

                                Call MetodosCompartidosSBOCls.IniciaTransaccion()

                                'Agrega a SBO los repuestos que se van a comprar antes de generar la orden
                                'Call AgregarRepuestosSBO(m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria)
                                blnDebeGenerarXML = DebeGenerarXML(m_strCodMarca, txtProveedor.Tag)
                                If blnDebeGenerarXML Then
                                    blnGenerarOrdenCompra = ValidarCodigosEspecificos()
                                Else
                                    blnGenerarOrdenCompra = True
                                End If
                                If blnGenerarOrdenCompra Then
                                    If MetodosCompartidosSBOCls.GeneraOrdenDeCompra(m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria(0)(mc_strNoOrden), _
                                                                                    System.DateTime.Today, _
                                                                                    txtProveedor.Tag, _
                                                                                    m_strMarca, _
                                                                                    m_strModelo.Trim(), _
                                                                                    m_strNoChasis, _
                                                                                    m_intAnio, _
                                                                                    m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria, _
                                                                                   G_strIDSucursal, _
                                                                                    NoPedidoEnSap, _
                                                                                    strSerie, _
                                                                                     intDocNum, _
                                                                                     blnDebeGenerarXML, _
                                                                                     g_strDireccionB2B, m_strAsesor, m_strEstilo, m_strPlaca, strNoBodega, strCentroBeneficio,
                                                                                     strDetalle, m_strCodMarca, m_intTipo, m_intTipoArt) Then

                                        If AsignaProveedoryidPedidoenSAPaTarcking(m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria, _
                                                                                  txtProveedor.Tag, _
                                                                                  NoPedidoEnSap, _
                                                                                  txtDetalle.Text) Then

                                            If GuardarTrackingRepuestos(m_dstRepuestosProveeduria, m_adpRepuestosProveeduria) <> -1 Then

                                                Call CreaEstadoSolicitado(m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria(0)(mc_strNoOrden), _
                                                                          m_dtbRepuestosxOrden)

                                                Call MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Commit)

                                                m_Ok = True
                                            Else 'GuardarTrackingRepuestos

                                                Call MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                                                m_Ok = False
                                            End If 'GuardarTrackingRepuestos

                                        Else 'AsignaProveedor_a_Repuestos
                                            Call MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                                            m_Ok = False
                                        End If 'AsignaProveedor_a_Repuestos
                                        'Genera mensaje en SBO para notificar al bodeguero la creación de una orden de compra
                                        adpMensajeria.CreaMensajeDMS_SBO_OrdenCompra(My.Resources.ResourceUI.MensajeNuevaOrdenCompra, intDocNum, m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria(0)(mc_strNoOrden))

                                    Else

                                        Call MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                                    End If 'MetodosCompartidosSBOCls.GeneraOrdenDeCompra
                                Else
                                    MessageBox.Show(My.Resources.ResourceUI.MensajeNosePuedeCrearOrdenSinEspecificos)
                                End If
                            End If 'm_dstRepuestosProveeduria

                            Call Me.Close()
                        Else
                            objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarProveedor)

                        End If 'Seleccionar proveedor
                    End If
                End If


                'Call Me.Close()
            Catch ex As Exception
                Call MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                m_Ok = False
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub GenerarOfertaCompra(ByVal p_blnTipoEspecial As Boolean)

            Try
                'Dim intCantidadDeRegistrosEnTracking As Integer
                Dim NoPedidoEnSap As String = ""
                Dim strSerie As String = ""
                Dim intDocNum As Integer
                Dim blnDebeGenerarXML As Boolean
                Dim blnGenerarOfertaCompra As Boolean = True
                Dim adpMensajeria As New MensajeriaSBOTallerDataAdapter
                Dim strNoBodega As String = ""
                Dim objTransferencia As New TransferenciaItems(G_objCompany)
                Dim strCentroBeneficio As String = String.Empty

                Dim strDetalle As String = String.Empty

                strDetalle = txtDetalle.Text

                strNoBodega = objTransferencia.RetornaBodegaProcesoByTipoOrden(m_intTipo)
                strCentroBeneficio = objTransferencia.RetornaCentroBeneficioByTipoOrden(m_intTipo)

                If Not RevisarBodegasCorrectas(m_dstRepuestosProveeduria) Then
                    Call objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeOfCnopuedeCrearsepor & vbCrLf & _
                                                            My.Resources.ResourceUI.MensajeNoExisteBodega & vbCrLf & _
                                                            My.Resources.ResourceUI.MensajeLaBodegaNotieneValorValido)

                Else

                    If Not ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, _
                                                                              mc_strIDSerieOfertaCompra, _
                                                                              strSerie) Then

                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeOfCnopuedeCrearsepor & vbCrLf & _
                                                            My.Resources.ResourceUI.MensajeNoTieneSeries & vbCrLf & _
                                                            My.Resources.ResourceUI.MensajeSerieValorNoValido)

                    Else

                        If txtProveedor.Text <> "" Then 'Seleccionar un proveedor para generar la orden


                            If m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Rows.Count > 0 Then

                                Call MetodosCompartidosSBOCls.IniciarCotizacion(m_intNoCotizacion)

                                Call MetodosCompartidosSBOCls.IniciaTransaccion()

                                'Agrega a SBO los repuestos que se van a comprar antes de generar la orden
                                'Call AgregarRepuestosSBO(m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria)
                                blnDebeGenerarXML = DebeGenerarXML(m_strCodMarca, txtProveedor.Tag)
                                If blnDebeGenerarXML Then
                                    blnGenerarOfertaCompra = ValidarCodigosEspecificos()
                                Else
                                    blnGenerarOfertaCompra = True
                                End If
                                If blnGenerarOfertaCompra Then
                                    If MetodosCompartidosSBOCls.GeneraOfertaDeCompra(m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria(0)(mc_strNoOrden), _
                                                                                    System.DateTime.Today, _
                                                                                    txtProveedor.Tag, _
                                                                                    m_strMarca, _
                                                                                    m_strModelo.Trim(), _
                                                                                    m_strNoChasis, _
                                                                                    m_intAnio, _
                                                                                    m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria, _
                                                                                   G_strIDSucursal, _
                                                                                    NoPedidoEnSap, _
                                                                                    strSerie, _
                                                                                     intDocNum, _
                                                                                     blnDebeGenerarXML, _
                                                                                     g_strDireccionB2B, m_strAsesor, m_strEstilo, m_strPlaca, strNoBodega, strCentroBeneficio,
                                                                                     strDetalle, m_strCodMarca, m_intTipo, m_intTipoArt) Then

                                        If AsignaProveedoryidPedidoenSAPaTarcking(m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria, _
                                                                                  txtProveedor.Tag, _
                                                                                  NoPedidoEnSap, _
                                                                                  txtDetalle.Text) Then

                                            If GuardarTrackingRepuestos(m_dstRepuestosProveeduria, m_adpRepuestosProveeduria) <> -1 Then

                                                Call CreaEstadoSolicitado(m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria(0)(mc_strNoOrden), _
                                                                          m_dtbRepuestosxOrden)

                                                Call MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Commit)

                                                m_Ok = True
                                            Else 'GuardarTrackingRepuestos

                                                Call MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                                                m_Ok = False
                                            End If 'GuardarTrackingRepuestos

                                        Else 'AsignaProveedor_a_Repuestos
                                            Call MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                                            m_Ok = False
                                        End If 'AsignaProveedor_a_Repuestos
                                        'Genera mensaje en SBO para notificar al bodeguero la creación de una orden de compra
                                        'adpMensajeria.CreaMensajeDMS_SBO_OrdenCompra(My.Resources.ResourceUI.MensajeNuevaOrdenCompra, intDocNum, m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria(0)(mc_strNoOrden))

                                        adpMensajeria.CreaMensajeDMS_SBO_OfertaCompra(My.Resources.ResourceUI.MensajeNuevaOfertaCompra, intDocNum, m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria(0)(mc_strNoOrden))

                                    Else

                                        Call MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                                    End If 'MetodosCompartidosSBOCls.GeneraOrdenDeCompra
                                Else
                                    MessageBox.Show(My.Resources.ResourceUI.MensajeNosePuedeCrearOrdenSinEspecificos)
                                End If
                            End If 'm_dstRepuestosProveeduria

                            Call Me.Close()
                        Else
                            objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarProveedor)

                        End If 'Seleccionar proveedor
                    End If
                End If


                'Call Me.Close()
            Catch ex As Exception
                Call MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                m_Ok = False
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub


#End Region

#Region "Propiedades"

        Public ReadOnly Property Ok() As Boolean
            Get
                Return m_Ok
            End Get
        End Property


#End Region

    End Class
End Namespace











