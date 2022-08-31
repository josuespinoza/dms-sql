
Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmConfiguracionApp
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim ConfCatalogoRepXMarcaDataset1 As DMSOneFramework.ConfCatalogoRepXMarcaDataset
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfiguracionApp))
            Me.lblSucursal = New System.Windows.Forms.Label()
            Me.tabConfiguracion = New System.Windows.Forms.TabControl()
            Me.tpGenerales = New System.Windows.Forms.TabPage()
            Me.gbx_Tipo_Compra = New System.Windows.Forms.GroupBox()
            Me.rb_OfertaCompra = New System.Windows.Forms.RadioButton()
            Me.rb_OrdenCompra = New System.Windows.Forms.RadioButton()
            Me.GroupBox13 = New System.Windows.Forms.GroupBox()
            Me.chkCitasCliInv = New System.Windows.Forms.CheckBox()
            Me.chkUsaFiltroClientes = New System.Windows.Forms.CheckBox()
            Me.GroupBox12 = New System.Windows.Forms.GroupBox()
            Me.chkUsaDraftTransferencia = New System.Windows.Forms.CheckBox()
            Me.GroupBox8 = New System.Windows.Forms.GroupBox()
            Me.picUnidadesTiempo = New System.Windows.Forms.PictureBox()
            Me.txtUnidadTiempo = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label28 = New System.Windows.Forms.Label()
            Me.lblUnidadTiempo = New System.Windows.Forms.Label()
            Me.GroupBox6 = New System.Windows.Forms.GroupBox()
            Me.txtCopiasRepRecepcion = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label23 = New System.Windows.Forms.Label()
            Me.lblCopiasRepRecepcion = New System.Windows.Forms.Label()
            Me.GroupBox5 = New System.Windows.Forms.GroupBox()
            Me.chkCrearOThijas = New System.Windows.Forms.CheckBox()
            Me.chkGeneraOTsEspeciales = New System.Windows.Forms.CheckBox()
            Me.GroupBox3 = New System.Windows.Forms.GroupBox()
            Me.chckUsaListaCliente = New System.Windows.Forms.CheckBox()
            Me.picListaPrecios = New System.Windows.Forms.PictureBox()
            Me.txtListaPrecios = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label12 = New System.Windows.Forms.Label()
            Me.gbArticulos = New System.Windows.Forms.GroupBox()
            Me.chkSolOTEsp = New System.Windows.Forms.CheckBox()
            Me.chkAsignacionUnicaMO = New System.Windows.Forms.CheckBox()
            Me.chkCambiaPrecio = New System.Windows.Forms.CheckBox()
            Me.chkFinalizaOTCantSolicitada = New System.Windows.Forms.CheckBox()
            Me.chkUsaAsignacionAutomaticaEncargadoOper = New System.Windows.Forms.CheckBox()
            Me.chkUsaSuministros = New System.Windows.Forms.CheckBox()
            Me.chkUsaServiciosExternos = New System.Windows.Forms.CheckBox()
            Me.chkUsaServicios = New System.Windows.Forms.CheckBox()
            Me.chkUsaRepuestos = New System.Windows.Forms.CheckBox()
            Me.chkUsaValTiempoEs = New System.Windows.Forms.CheckBox()
            Me.tpSeries = New System.Windows.Forms.TabPage()
            Me.gpDocInventario = New System.Windows.Forms.GroupBox()
            Me.ntxtTraslados = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picTraslados = New System.Windows.Forms.PictureBox()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.gbVentas = New System.Windows.Forms.GroupBox()
            Me.Label34 = New System.Windows.Forms.Label()
            Me.Label35 = New System.Windows.Forms.Label()
            Me.Label36 = New System.Windows.Forms.Label()
            Me.Label26 = New System.Windows.Forms.Label()
            Me.txtCotizaciones = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picCotizaciones = New System.Windows.Forms.PictureBox()
            Me.Label27 = New System.Windows.Forms.Label()
            Me.ntxtOrdenVentas = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picOrdVentas = New System.Windows.Forms.PictureBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.gbCompras = New System.Windows.Forms.GroupBox()
            Me.ntxtOfertadeCompra = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picOfertasdeCompra = New System.Windows.Forms.PictureBox()
            Me.Label41 = New System.Windows.Forms.Label()
            Me.Label42 = New System.Windows.Forms.Label()
            Me.ntxtOrdendeCompra = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picOrdenesdeCompra = New System.Windows.Forms.PictureBox()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.lblOrdenes = New System.Windows.Forms.Label()
            Me.tpBodega = New System.Windows.Forms.TabPage()
            Me.gbBodegas = New System.Windows.Forms.GroupBox()
            Me.ntxtSE = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.PicSE = New System.Windows.Forms.PictureBox()
            Me.ntxtSuministros = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picSuministros = New System.Windows.Forms.PictureBox()
            Me.ntxtRepuestos = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picRepuestos = New System.Windows.Forms.PictureBox()
            Me.ntxtProcesos = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picProceso = New System.Windows.Forms.PictureBox()
            Me.Label10 = New System.Windows.Forms.Label()
            Me.Label9 = New System.Windows.Forms.Label()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.lblSE = New System.Windows.Forms.Label()
            Me.lblSuministros = New System.Windows.Forms.Label()
            Me.lblRefacciones = New System.Windows.Forms.Label()
            Me.lblBodegaDeProcesos = New System.Windows.Forms.Label()
            Me.tpMensajeria = New System.Windows.Forms.TabPage()
            Me.GroupBox2 = New System.Windows.Forms.GroupBox()
            Me.chkUsaMensajeriaXCentroCosto = New System.Windows.Forms.CheckBox()
            Me.ntxtIntervaloMen = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.Label13 = New System.Windows.Forms.Label()
            Me.lblIntervaloMensajeria = New System.Windows.Forms.Label()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.picEncargadoAccesorios = New System.Windows.Forms.PictureBox()
            Me.txtEncargadoAccesorios = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label40 = New System.Windows.Forms.Label()
            Me.lblBodAccesorios = New System.Windows.Forms.Label()
            Me.picEncargadoOrdenCompra = New System.Windows.Forms.PictureBox()
            Me.txtEncargadoOrdenCompra = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label37 = New System.Windows.Forms.Label()
            Me.Label38 = New System.Windows.Forms.Label()
            Me.picEncargadoSuministros = New System.Windows.Forms.PictureBox()
            Me.txtEncargadoSuministros = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label24 = New System.Windows.Forms.Label()
            Me.Label25 = New System.Windows.Forms.Label()
            Me.picEncargadoRepuestos = New System.Windows.Forms.PictureBox()
            Me.txtEncargadoRepuestos = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label19 = New System.Windows.Forms.Label()
            Me.lblEncargadoRepuestos = New System.Windows.Forms.Label()
            Me.picencargadoproduccion = New System.Windows.Forms.PictureBox()
            Me.picEncargadoBodega = New System.Windows.Forms.PictureBox()
            Me.ntxtEncargadoProduccion = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.ntxtEncargadoBodega = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label15 = New System.Windows.Forms.Label()
            Me.Label17 = New System.Windows.Forms.Label()
            Me.lblBodeguero = New System.Windows.Forms.Label()
            Me.lblEncargadoProduccion = New System.Windows.Forms.Label()
            Me.tpImpuestos = New System.Windows.Forms.TabPage()
            Me.grpImpuestos = New System.Windows.Forms.GroupBox()
            Me.Label29 = New System.Windows.Forms.Label()
            Me.Label30 = New System.Windows.Forms.Label()
            Me.Label31 = New System.Windows.Forms.Label()
            Me.Label32 = New System.Windows.Forms.Label()
            Me.txtImpServiciosExternos = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picImpServiciosExternos = New System.Windows.Forms.PictureBox()
            Me.Label11 = New System.Windows.Forms.Label()
            Me.txtImpSuministros = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picImpSuministros = New System.Windows.Forms.PictureBox()
            Me.Label14 = New System.Windows.Forms.Label()
            Me.txtImpRefacciones = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picImpRefacciones = New System.Windows.Forms.PictureBox()
            Me.Label18 = New System.Windows.Forms.Label()
            Me.txtImpServicios = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picImpServicios = New System.Windows.Forms.PictureBox()
            Me.Label20 = New System.Windows.Forms.Label()
            Me.tabRepuestosExternos = New System.Windows.Forms.TabPage()
            Me.GroupBox4 = New System.Windows.Forms.GroupBox()
            Me.picDireccionB2B = New System.Windows.Forms.PictureBox()
            Me.txtDireccionB2b = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label21 = New System.Windows.Forms.Label()
            Me.Label22 = New System.Windows.Forms.Label()
            Me.btnEliminar = New System.Windows.Forms.Button()
            Me.btnAgregar = New System.Windows.Forms.Button()
            Me.dtgMarcasConfiguradas = New System.Windows.Forms.DataGridView()
            Me.Check = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DescMarcaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ServidorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CompañiaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.UsuarioServidorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.PasswordServidorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.BDCompañiaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CodAlmacenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CodListaPrecioDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.NombAlmacenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.NombListaPreciosDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.UsuarioSBODataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.PasswordSBODataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.chkCatalogosExternos = New System.Windows.Forms.CheckBox()
            Me.tabCosteo = New System.Windows.Forms.TabPage()
            Me.GroupBox15 = New System.Windows.Forms.GroupBox()
            Me.Label43 = New System.Windows.Forms.Label()
            Me.picTipoMoneda = New System.Windows.Forms.PictureBox()
            Me.txtTipoMoneda = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblCuentaContable = New System.Windows.Forms.Label()
            Me.txtNombreCuenta = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtNumeroCuenta = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.piCuentasContables = New System.Windows.Forms.PictureBox()
            Me.GroupBox10 = New System.Windows.Forms.GroupBox()
            Me.chkOtrosGastos = New System.Windows.Forms.CheckBox()
            Me.GroupBox9 = New System.Windows.Forms.GroupBox()
            Me.gbxTipoCostos = New System.Windows.Forms.GroupBox()
            Me.rbtDetallado = New System.Windows.Forms.RadioButton()
            Me.rbtSimple = New System.Windows.Forms.RadioButton()
            Me.gbxTipoCosteoServicios = New System.Windows.Forms.GroupBox()
            Me.rbtTiempoReal = New System.Windows.Forms.RadioButton()
            Me.rbtEstandar = New System.Windows.Forms.RadioButton()
            Me.chkCosteoServicios = New System.Windows.Forms.CheckBox()
            Me.GroupBox7 = New System.Windows.Forms.GroupBox()
            Me.chkSEInventariables = New System.Windows.Forms.CheckBox()
            Me.tabCitas = New System.Windows.Forms.TabPage()
            Me.GroupBox11 = New System.Windows.Forms.GroupBox()
            Me.picArticuloCotizacion = New System.Windows.Forms.PictureBox()
            Me.txtArtCotizacion = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label33 = New System.Windows.Forms.Label()
            Me.Label39 = New System.Windows.Forms.Label()
            Me.tabWeb = New System.Windows.Forms.TabPage()
            Me.GroupBox14 = New System.Windows.Forms.GroupBox()
            Me.chkOTTotales = New System.Windows.Forms.CheckBox()
            Me.chkOTRepuestos = New System.Windows.Forms.CheckBox()
            Me.bsMarcasConfiguradas = New System.Windows.Forms.BindingSource(Me.components)
            Me.lblName = New System.Windows.Forms.Label()
            Me.btnCancelar = New System.Windows.Forms.Button()
            Me.btnAceptar = New System.Windows.Forms.Button()
            Me.fbdDireccionB2B = New System.Windows.Forms.FolderBrowserDialog()
            ConfCatalogoRepXMarcaDataset1 = New DMSOneFramework.ConfCatalogoRepXMarcaDataset()
            CType(ConfCatalogoRepXMarcaDataset1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabConfiguracion.SuspendLayout()
            Me.tpGenerales.SuspendLayout()
            Me.gbx_Tipo_Compra.SuspendLayout()
            Me.GroupBox13.SuspendLayout()
            Me.GroupBox12.SuspendLayout()
            Me.GroupBox8.SuspendLayout()
            CType(Me.picUnidadesTiempo, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.GroupBox6.SuspendLayout()
            Me.GroupBox5.SuspendLayout()
            Me.GroupBox3.SuspendLayout()
            CType(Me.picListaPrecios, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.gbArticulos.SuspendLayout()
            Me.tpSeries.SuspendLayout()
            Me.gpDocInventario.SuspendLayout()
            CType(Me.picTraslados, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.gbVentas.SuspendLayout()
            CType(Me.picCotizaciones, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picOrdVentas, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.gbCompras.SuspendLayout()
            CType(Me.picOfertasdeCompra, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picOrdenesdeCompra, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpBodega.SuspendLayout()
            Me.gbBodegas.SuspendLayout()
            CType(Me.PicSE, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picSuministros, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picRepuestos, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picProceso, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpMensajeria.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            Me.GroupBox1.SuspendLayout()
            CType(Me.picEncargadoAccesorios, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picEncargadoOrdenCompra, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picEncargadoSuministros, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picEncargadoRepuestos, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picencargadoproduccion, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picEncargadoBodega, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpImpuestos.SuspendLayout()
            Me.grpImpuestos.SuspendLayout()
            CType(Me.picImpServiciosExternos, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picImpSuministros, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picImpRefacciones, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picImpServicios, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabRepuestosExternos.SuspendLayout()
            Me.GroupBox4.SuspendLayout()
            CType(Me.picDireccionB2B, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtgMarcasConfiguradas, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabCosteo.SuspendLayout()
            Me.GroupBox15.SuspendLayout()
            CType(Me.picTipoMoneda, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.piCuentasContables, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.GroupBox10.SuspendLayout()
            Me.GroupBox9.SuspendLayout()
            Me.gbxTipoCostos.SuspendLayout()
            Me.gbxTipoCosteoServicios.SuspendLayout()
            Me.GroupBox7.SuspendLayout()
            Me.tabCitas.SuspendLayout()
            Me.GroupBox11.SuspendLayout()
            CType(Me.picArticuloCotizacion, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabWeb.SuspendLayout()
            Me.GroupBox14.SuspendLayout()
            CType(Me.bsMarcasConfiguradas, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'ConfCatalogoRepXMarcaDataset1
            '
            ConfCatalogoRepXMarcaDataset1.DataSetName = "ConfCatalogoRepXMarcaDataset"
            ConfCatalogoRepXMarcaDataset1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'lblSucursal
            '
            resources.ApplyResources(Me.lblSucursal, "lblSucursal")
            Me.lblSucursal.Name = "lblSucursal"
            '
            'tabConfiguracion
            '
            Me.tabConfiguracion.Controls.Add(Me.tpGenerales)
            Me.tabConfiguracion.Controls.Add(Me.tpSeries)
            Me.tabConfiguracion.Controls.Add(Me.tpBodega)
            Me.tabConfiguracion.Controls.Add(Me.tpMensajeria)
            Me.tabConfiguracion.Controls.Add(Me.tpImpuestos)
            Me.tabConfiguracion.Controls.Add(Me.tabRepuestosExternos)
            Me.tabConfiguracion.Controls.Add(Me.tabCosteo)
            Me.tabConfiguracion.Controls.Add(Me.tabCitas)
            Me.tabConfiguracion.Controls.Add(Me.tabWeb)
            resources.ApplyResources(Me.tabConfiguracion, "tabConfiguracion")
            Me.tabConfiguracion.Name = "tabConfiguracion"
            Me.tabConfiguracion.SelectedIndex = 0
            '
            'tpGenerales
            '
            Me.tpGenerales.Controls.Add(Me.gbx_Tipo_Compra)
            Me.tpGenerales.Controls.Add(Me.GroupBox13)
            Me.tpGenerales.Controls.Add(Me.GroupBox12)
            Me.tpGenerales.Controls.Add(Me.GroupBox8)
            Me.tpGenerales.Controls.Add(Me.GroupBox6)
            Me.tpGenerales.Controls.Add(Me.GroupBox5)
            Me.tpGenerales.Controls.Add(Me.GroupBox3)
            Me.tpGenerales.Controls.Add(Me.gbArticulos)
            resources.ApplyResources(Me.tpGenerales, "tpGenerales")
            Me.tpGenerales.Name = "tpGenerales"
            '
            'gbx_Tipo_Compra
            '
            Me.gbx_Tipo_Compra.Controls.Add(Me.rb_OfertaCompra)
            Me.gbx_Tipo_Compra.Controls.Add(Me.rb_OrdenCompra)
            resources.ApplyResources(Me.gbx_Tipo_Compra, "gbx_Tipo_Compra")
            Me.gbx_Tipo_Compra.Name = "gbx_Tipo_Compra"
            Me.gbx_Tipo_Compra.TabStop = False
            '
            'rb_OfertaCompra
            '
            resources.ApplyResources(Me.rb_OfertaCompra, "rb_OfertaCompra")
            Me.rb_OfertaCompra.Name = "rb_OfertaCompra"
            Me.rb_OfertaCompra.TabStop = True
            Me.rb_OfertaCompra.UseVisualStyleBackColor = True
            '
            'rb_OrdenCompra
            '
            resources.ApplyResources(Me.rb_OrdenCompra, "rb_OrdenCompra")
            Me.rb_OrdenCompra.Name = "rb_OrdenCompra"
            Me.rb_OrdenCompra.TabStop = True
            Me.rb_OrdenCompra.UseVisualStyleBackColor = True
            '
            'GroupBox13
            '
            Me.GroupBox13.Controls.Add(Me.chkCitasCliInv)
            Me.GroupBox13.Controls.Add(Me.chkUsaFiltroClientes)
            resources.ApplyResources(Me.GroupBox13, "GroupBox13")
            Me.GroupBox13.Name = "GroupBox13"
            Me.GroupBox13.TabStop = False
            '
            'chkCitasCliInv
            '
            resources.ApplyResources(Me.chkCitasCliInv, "chkCitasCliInv")
            Me.chkCitasCliInv.Name = "chkCitasCliInv"
            Me.chkCitasCliInv.UseVisualStyleBackColor = True
            '
            'chkUsaFiltroClientes
            '
            resources.ApplyResources(Me.chkUsaFiltroClientes, "chkUsaFiltroClientes")
            Me.chkUsaFiltroClientes.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkUsaFiltroClientes.Name = "chkUsaFiltroClientes"
            Me.chkUsaFiltroClientes.UseVisualStyleBackColor = False
            '
            'GroupBox12
            '
            Me.GroupBox12.Controls.Add(Me.chkUsaDraftTransferencia)
            resources.ApplyResources(Me.GroupBox12, "GroupBox12")
            Me.GroupBox12.Name = "GroupBox12"
            Me.GroupBox12.TabStop = False
            '
            'chkUsaDraftTransferencia
            '
            resources.ApplyResources(Me.chkUsaDraftTransferencia, "chkUsaDraftTransferencia")
            Me.chkUsaDraftTransferencia.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkUsaDraftTransferencia.Name = "chkUsaDraftTransferencia"
            Me.chkUsaDraftTransferencia.UseVisualStyleBackColor = False
            '
            'GroupBox8
            '
            Me.GroupBox8.Controls.Add(Me.picUnidadesTiempo)
            Me.GroupBox8.Controls.Add(Me.txtUnidadTiempo)
            Me.GroupBox8.Controls.Add(Me.Label28)
            Me.GroupBox8.Controls.Add(Me.lblUnidadTiempo)
            resources.ApplyResources(Me.GroupBox8, "GroupBox8")
            Me.GroupBox8.Name = "GroupBox8"
            Me.GroupBox8.TabStop = False
            '
            'picUnidadesTiempo
            '
            Me.picUnidadesTiempo.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picUnidadesTiempo, "picUnidadesTiempo")
            Me.picUnidadesTiempo.Name = "picUnidadesTiempo"
            Me.picUnidadesTiempo.TabStop = False
            '
            'txtUnidadTiempo
            '
            Me.txtUnidadTiempo.AceptaNegativos = False
            Me.txtUnidadTiempo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtUnidadTiempo.EstiloSBO = True
            resources.ApplyResources(Me.txtUnidadTiempo, "txtUnidadTiempo")
            Me.txtUnidadTiempo.MaxDecimales = 0
            Me.txtUnidadTiempo.MaxEnteros = 0
            Me.txtUnidadTiempo.Millares = False
            Me.txtUnidadTiempo.Name = "txtUnidadTiempo"
            Me.txtUnidadTiempo.Size_AdjustableHeight = 20
            Me.txtUnidadTiempo.TeclasDeshacer = True
            Me.txtUnidadTiempo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label28
            '
            Me.Label28.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label28, "Label28")
            Me.Label28.Name = "Label28"
            '
            'lblUnidadTiempo
            '
            resources.ApplyResources(Me.lblUnidadTiempo, "lblUnidadTiempo")
            Me.lblUnidadTiempo.Name = "lblUnidadTiempo"
            '
            'GroupBox6
            '
            Me.GroupBox6.Controls.Add(Me.txtCopiasRepRecepcion)
            Me.GroupBox6.Controls.Add(Me.Label23)
            Me.GroupBox6.Controls.Add(Me.lblCopiasRepRecepcion)
            resources.ApplyResources(Me.GroupBox6, "GroupBox6")
            Me.GroupBox6.Name = "GroupBox6"
            Me.GroupBox6.TabStop = False
            '
            'txtCopiasRepRecepcion
            '
            Me.txtCopiasRepRecepcion.AceptaNegativos = False
            Me.txtCopiasRepRecepcion.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtCopiasRepRecepcion.EstiloSBO = True
            resources.ApplyResources(Me.txtCopiasRepRecepcion, "txtCopiasRepRecepcion")
            Me.txtCopiasRepRecepcion.MaxDecimales = 0
            Me.txtCopiasRepRecepcion.MaxEnteros = 0
            Me.txtCopiasRepRecepcion.Millares = False
            Me.txtCopiasRepRecepcion.Name = "txtCopiasRepRecepcion"
            Me.txtCopiasRepRecepcion.Size_AdjustableHeight = 20
            Me.txtCopiasRepRecepcion.TeclasDeshacer = True
            Me.txtCopiasRepRecepcion.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.Numeric
            '
            'Label23
            '
            Me.Label23.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label23, "Label23")
            Me.Label23.Name = "Label23"
            '
            'lblCopiasRepRecepcion
            '
            resources.ApplyResources(Me.lblCopiasRepRecepcion, "lblCopiasRepRecepcion")
            Me.lblCopiasRepRecepcion.Name = "lblCopiasRepRecepcion"
            '
            'GroupBox5
            '
            Me.GroupBox5.Controls.Add(Me.chkCrearOThijas)
            Me.GroupBox5.Controls.Add(Me.chkGeneraOTsEspeciales)
            resources.ApplyResources(Me.GroupBox5, "GroupBox5")
            Me.GroupBox5.Name = "GroupBox5"
            Me.GroupBox5.TabStop = False
            '
            'chkCrearOThijas
            '
            Me.chkCrearOThijas.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            resources.ApplyResources(Me.chkCrearOThijas, "chkCrearOThijas")
            Me.chkCrearOThijas.Name = "chkCrearOThijas"
            '
            'chkGeneraOTsEspeciales
            '
            resources.ApplyResources(Me.chkGeneraOTsEspeciales, "chkGeneraOTsEspeciales")
            Me.chkGeneraOTsEspeciales.Name = "chkGeneraOTsEspeciales"
            '
            'GroupBox3
            '
            Me.GroupBox3.Controls.Add(Me.chckUsaListaCliente)
            Me.GroupBox3.Controls.Add(Me.picListaPrecios)
            Me.GroupBox3.Controls.Add(Me.txtListaPrecios)
            Me.GroupBox3.Controls.Add(Me.Label12)
            resources.ApplyResources(Me.GroupBox3, "GroupBox3")
            Me.GroupBox3.Name = "GroupBox3"
            Me.GroupBox3.TabStop = False
            '
            'chckUsaListaCliente
            '
            resources.ApplyResources(Me.chckUsaListaCliente, "chckUsaListaCliente")
            Me.chckUsaListaCliente.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chckUsaListaCliente.Name = "chckUsaListaCliente"
            '
            'picListaPrecios
            '
            Me.picListaPrecios.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picListaPrecios, "picListaPrecios")
            Me.picListaPrecios.Name = "picListaPrecios"
            Me.picListaPrecios.TabStop = False
            '
            'txtListaPrecios
            '
            Me.txtListaPrecios.AceptaNegativos = False
            Me.txtListaPrecios.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtListaPrecios.EstiloSBO = True
            resources.ApplyResources(Me.txtListaPrecios, "txtListaPrecios")
            Me.txtListaPrecios.MaxDecimales = 0
            Me.txtListaPrecios.MaxEnteros = 0
            Me.txtListaPrecios.Millares = False
            Me.txtListaPrecios.Name = "txtListaPrecios"
            Me.txtListaPrecios.Size_AdjustableHeight = 20
            Me.txtListaPrecios.TeclasDeshacer = True
            Me.txtListaPrecios.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label12
            '
            resources.ApplyResources(Me.Label12, "Label12")
            Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label12.Name = "Label12"
            '
            'gbArticulos
            '
            Me.gbArticulos.Controls.Add(Me.chkSolOTEsp)
            Me.gbArticulos.Controls.Add(Me.chkAsignacionUnicaMO)
            Me.gbArticulos.Controls.Add(Me.chkCambiaPrecio)
            Me.gbArticulos.Controls.Add(Me.chkFinalizaOTCantSolicitada)
            Me.gbArticulos.Controls.Add(Me.chkUsaAsignacionAutomaticaEncargadoOper)
            Me.gbArticulos.Controls.Add(Me.chkUsaSuministros)
            Me.gbArticulos.Controls.Add(Me.chkUsaServiciosExternos)
            Me.gbArticulos.Controls.Add(Me.chkUsaServicios)
            Me.gbArticulos.Controls.Add(Me.chkUsaRepuestos)
            Me.gbArticulos.Controls.Add(Me.chkUsaValTiempoEs)
            resources.ApplyResources(Me.gbArticulos, "gbArticulos")
            Me.gbArticulos.Name = "gbArticulos"
            Me.gbArticulos.TabStop = False
            '
            'chkSolOTEsp
            '
            resources.ApplyResources(Me.chkSolOTEsp, "chkSolOTEsp")
            Me.chkSolOTEsp.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkSolOTEsp.Name = "chkSolOTEsp"
            '
            'chkAsignacionUnicaMO
            '
            Me.chkAsignacionUnicaMO.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            resources.ApplyResources(Me.chkAsignacionUnicaMO, "chkAsignacionUnicaMO")
            Me.chkAsignacionUnicaMO.Name = "chkAsignacionUnicaMO"
            '
            'chkCambiaPrecio
            '
            Me.chkCambiaPrecio.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            resources.ApplyResources(Me.chkCambiaPrecio, "chkCambiaPrecio")
            Me.chkCambiaPrecio.Name = "chkCambiaPrecio"
            '
            'chkFinalizaOTCantSolicitada
            '
            Me.chkFinalizaOTCantSolicitada.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            resources.ApplyResources(Me.chkFinalizaOTCantSolicitada, "chkFinalizaOTCantSolicitada")
            Me.chkFinalizaOTCantSolicitada.Name = "chkFinalizaOTCantSolicitada"
            '
            'chkUsaAsignacionAutomaticaEncargadoOper
            '
            resources.ApplyResources(Me.chkUsaAsignacionAutomaticaEncargadoOper, "chkUsaAsignacionAutomaticaEncargadoOper")
            Me.chkUsaAsignacionAutomaticaEncargadoOper.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkUsaAsignacionAutomaticaEncargadoOper.Name = "chkUsaAsignacionAutomaticaEncargadoOper"
            '
            'chkUsaSuministros
            '
            resources.ApplyResources(Me.chkUsaSuministros, "chkUsaSuministros")
            Me.chkUsaSuministros.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkUsaSuministros.Name = "chkUsaSuministros"
            '
            'chkUsaServiciosExternos
            '
            resources.ApplyResources(Me.chkUsaServiciosExternos, "chkUsaServiciosExternos")
            Me.chkUsaServiciosExternos.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkUsaServiciosExternos.Name = "chkUsaServiciosExternos"
            '
            'chkUsaServicios
            '
            resources.ApplyResources(Me.chkUsaServicios, "chkUsaServicios")
            Me.chkUsaServicios.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkUsaServicios.Name = "chkUsaServicios"
            '
            'chkUsaRepuestos
            '
            resources.ApplyResources(Me.chkUsaRepuestos, "chkUsaRepuestos")
            Me.chkUsaRepuestos.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkUsaRepuestos.Name = "chkUsaRepuestos"
            '
            'chkUsaValTiempoEs
            '
            Me.chkUsaValTiempoEs.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            resources.ApplyResources(Me.chkUsaValTiempoEs, "chkUsaValTiempoEs")
            Me.chkUsaValTiempoEs.Name = "chkUsaValTiempoEs"
            '
            'tpSeries
            '
            Me.tpSeries.Controls.Add(Me.gpDocInventario)
            Me.tpSeries.Controls.Add(Me.gbVentas)
            Me.tpSeries.Controls.Add(Me.gbCompras)
            resources.ApplyResources(Me.tpSeries, "tpSeries")
            Me.tpSeries.Name = "tpSeries"
            '
            'gpDocInventario
            '
            Me.gpDocInventario.Controls.Add(Me.ntxtTraslados)
            Me.gpDocInventario.Controls.Add(Me.picTraslados)
            Me.gpDocInventario.Controls.Add(Me.Label4)
            Me.gpDocInventario.Controls.Add(Me.Label3)
            resources.ApplyResources(Me.gpDocInventario, "gpDocInventario")
            Me.gpDocInventario.Name = "gpDocInventario"
            Me.gpDocInventario.TabStop = False
            '
            'ntxtTraslados
            '
            Me.ntxtTraslados.AceptaNegativos = False
            Me.ntxtTraslados.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.ntxtTraslados.EstiloSBO = True
            resources.ApplyResources(Me.ntxtTraslados, "ntxtTraslados")
            Me.ntxtTraslados.MaxDecimales = 0
            Me.ntxtTraslados.MaxEnteros = 0
            Me.ntxtTraslados.Millares = False
            Me.ntxtTraslados.Name = "ntxtTraslados"
            Me.ntxtTraslados.Size_AdjustableHeight = 20
            Me.ntxtTraslados.TeclasDeshacer = True
            Me.ntxtTraslados.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picTraslados
            '
            Me.picTraslados.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picTraslados, "picTraslados")
            Me.picTraslados.Name = "picTraslados"
            Me.picTraslados.TabStop = False
            '
            'Label4
            '
            Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Name = "Label4"
            '
            'Label3
            '
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.Name = "Label3"
            '
            'gbVentas
            '
            Me.gbVentas.Controls.Add(Me.Label34)
            Me.gbVentas.Controls.Add(Me.Label35)
            Me.gbVentas.Controls.Add(Me.Label36)
            Me.gbVentas.Controls.Add(Me.Label26)
            Me.gbVentas.Controls.Add(Me.txtCotizaciones)
            Me.gbVentas.Controls.Add(Me.picCotizaciones)
            Me.gbVentas.Controls.Add(Me.Label27)
            Me.gbVentas.Controls.Add(Me.ntxtOrdenVentas)
            Me.gbVentas.Controls.Add(Me.picOrdVentas)
            Me.gbVentas.Controls.Add(Me.Label2)
            Me.gbVentas.Controls.Add(Me.Label1)
            resources.ApplyResources(Me.gbVentas, "gbVentas")
            Me.gbVentas.Name = "gbVentas"
            Me.gbVentas.TabStop = False
            '
            'Label34
            '
            resources.ApplyResources(Me.Label34, "Label34")
            Me.Label34.Name = "Label34"
            '
            'Label35
            '
            Me.Label35.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label35, "Label35")
            Me.Label35.Name = "Label35"
            '
            'Label36
            '
            resources.ApplyResources(Me.Label36, "Label36")
            Me.Label36.Name = "Label36"
            '
            'Label26
            '
            Me.Label26.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label26, "Label26")
            Me.Label26.Name = "Label26"
            '
            'txtCotizaciones
            '
            Me.txtCotizaciones.AceptaNegativos = False
            Me.txtCotizaciones.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtCotizaciones.EstiloSBO = True
            resources.ApplyResources(Me.txtCotizaciones, "txtCotizaciones")
            Me.txtCotizaciones.MaxDecimales = 0
            Me.txtCotizaciones.MaxEnteros = 0
            Me.txtCotizaciones.Millares = False
            Me.txtCotizaciones.Name = "txtCotizaciones"
            Me.txtCotizaciones.Size_AdjustableHeight = 20
            Me.txtCotizaciones.TeclasDeshacer = True
            Me.txtCotizaciones.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picCotizaciones
            '
            Me.picCotizaciones.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picCotizaciones, "picCotizaciones")
            Me.picCotizaciones.Name = "picCotizaciones"
            Me.picCotizaciones.TabStop = False
            '
            'Label27
            '
            resources.ApplyResources(Me.Label27, "Label27")
            Me.Label27.Name = "Label27"
            '
            'ntxtOrdenVentas
            '
            Me.ntxtOrdenVentas.AceptaNegativos = False
            Me.ntxtOrdenVentas.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.ntxtOrdenVentas.EstiloSBO = True
            resources.ApplyResources(Me.ntxtOrdenVentas, "ntxtOrdenVentas")
            Me.ntxtOrdenVentas.MaxDecimales = 0
            Me.ntxtOrdenVentas.MaxEnteros = 0
            Me.ntxtOrdenVentas.Millares = False
            Me.ntxtOrdenVentas.Name = "ntxtOrdenVentas"
            Me.ntxtOrdenVentas.Size_AdjustableHeight = 20
            Me.ntxtOrdenVentas.TeclasDeshacer = True
            Me.ntxtOrdenVentas.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picOrdVentas
            '
            Me.picOrdVentas.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picOrdVentas, "picOrdVentas")
            Me.picOrdVentas.Name = "picOrdVentas"
            Me.picOrdVentas.TabStop = False
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'Label1
            '
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'gbCompras
            '
            Me.gbCompras.Controls.Add(Me.ntxtOfertadeCompra)
            Me.gbCompras.Controls.Add(Me.picOfertasdeCompra)
            Me.gbCompras.Controls.Add(Me.Label41)
            Me.gbCompras.Controls.Add(Me.Label42)
            Me.gbCompras.Controls.Add(Me.ntxtOrdendeCompra)
            Me.gbCompras.Controls.Add(Me.picOrdenesdeCompra)
            Me.gbCompras.Controls.Add(Me.Label5)
            Me.gbCompras.Controls.Add(Me.lblOrdenes)
            resources.ApplyResources(Me.gbCompras, "gbCompras")
            Me.gbCompras.Name = "gbCompras"
            Me.gbCompras.TabStop = False
            '
            'ntxtOfertadeCompra
            '
            Me.ntxtOfertadeCompra.AceptaNegativos = False
            Me.ntxtOfertadeCompra.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.ntxtOfertadeCompra.EstiloSBO = True
            resources.ApplyResources(Me.ntxtOfertadeCompra, "ntxtOfertadeCompra")
            Me.ntxtOfertadeCompra.MaxDecimales = 0
            Me.ntxtOfertadeCompra.MaxEnteros = 0
            Me.ntxtOfertadeCompra.Millares = False
            Me.ntxtOfertadeCompra.Name = "ntxtOfertadeCompra"
            Me.ntxtOfertadeCompra.Size_AdjustableHeight = 20
            Me.ntxtOfertadeCompra.TeclasDeshacer = True
            Me.ntxtOfertadeCompra.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picOfertasdeCompra
            '
            Me.picOfertasdeCompra.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picOfertasdeCompra, "picOfertasdeCompra")
            Me.picOfertasdeCompra.Name = "picOfertasdeCompra"
            Me.picOfertasdeCompra.TabStop = False
            '
            'Label41
            '
            Me.Label41.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label41, "Label41")
            Me.Label41.Name = "Label41"
            '
            'Label42
            '
            resources.ApplyResources(Me.Label42, "Label42")
            Me.Label42.Name = "Label42"
            '
            'ntxtOrdendeCompra
            '
            Me.ntxtOrdendeCompra.AceptaNegativos = False
            Me.ntxtOrdendeCompra.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.ntxtOrdendeCompra.EstiloSBO = True
            resources.ApplyResources(Me.ntxtOrdendeCompra, "ntxtOrdendeCompra")
            Me.ntxtOrdendeCompra.MaxDecimales = 0
            Me.ntxtOrdendeCompra.MaxEnteros = 0
            Me.ntxtOrdendeCompra.Millares = False
            Me.ntxtOrdendeCompra.Name = "ntxtOrdendeCompra"
            Me.ntxtOrdendeCompra.Size_AdjustableHeight = 20
            Me.ntxtOrdendeCompra.TeclasDeshacer = True
            Me.ntxtOrdendeCompra.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picOrdenesdeCompra
            '
            Me.picOrdenesdeCompra.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picOrdenesdeCompra, "picOrdenesdeCompra")
            Me.picOrdenesdeCompra.Name = "picOrdenesdeCompra"
            Me.picOrdenesdeCompra.TabStop = False
            '
            'Label5
            '
            Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.Name = "Label5"
            '
            'lblOrdenes
            '
            resources.ApplyResources(Me.lblOrdenes, "lblOrdenes")
            Me.lblOrdenes.Name = "lblOrdenes"
            '
            'tpBodega
            '
            Me.tpBodega.Controls.Add(Me.gbBodegas)
            resources.ApplyResources(Me.tpBodega, "tpBodega")
            Me.tpBodega.Name = "tpBodega"
            '
            'gbBodegas
            '
            Me.gbBodegas.Controls.Add(Me.ntxtSE)
            Me.gbBodegas.Controls.Add(Me.PicSE)
            Me.gbBodegas.Controls.Add(Me.ntxtSuministros)
            Me.gbBodegas.Controls.Add(Me.picSuministros)
            Me.gbBodegas.Controls.Add(Me.ntxtRepuestos)
            Me.gbBodegas.Controls.Add(Me.picRepuestos)
            Me.gbBodegas.Controls.Add(Me.ntxtProcesos)
            Me.gbBodegas.Controls.Add(Me.picProceso)
            Me.gbBodegas.Controls.Add(Me.Label10)
            Me.gbBodegas.Controls.Add(Me.Label9)
            Me.gbBodegas.Controls.Add(Me.Label8)
            Me.gbBodegas.Controls.Add(Me.Label7)
            Me.gbBodegas.Controls.Add(Me.lblSE)
            Me.gbBodegas.Controls.Add(Me.lblSuministros)
            Me.gbBodegas.Controls.Add(Me.lblRefacciones)
            Me.gbBodegas.Controls.Add(Me.lblBodegaDeProcesos)
            resources.ApplyResources(Me.gbBodegas, "gbBodegas")
            Me.gbBodegas.Name = "gbBodegas"
            Me.gbBodegas.TabStop = False
            '
            'ntxtSE
            '
            Me.ntxtSE.AceptaNegativos = False
            Me.ntxtSE.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.ntxtSE.EstiloSBO = True
            resources.ApplyResources(Me.ntxtSE, "ntxtSE")
            Me.ntxtSE.MaxDecimales = 0
            Me.ntxtSE.MaxEnteros = 0
            Me.ntxtSE.Millares = False
            Me.ntxtSE.Name = "ntxtSE"
            Me.ntxtSE.Size_AdjustableHeight = 20
            Me.ntxtSE.TeclasDeshacer = True
            Me.ntxtSE.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'PicSE
            '
            Me.PicSE.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.PicSE, "PicSE")
            Me.PicSE.Name = "PicSE"
            Me.PicSE.TabStop = False
            '
            'ntxtSuministros
            '
            Me.ntxtSuministros.AceptaNegativos = False
            Me.ntxtSuministros.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.ntxtSuministros.EstiloSBO = True
            resources.ApplyResources(Me.ntxtSuministros, "ntxtSuministros")
            Me.ntxtSuministros.MaxDecimales = 0
            Me.ntxtSuministros.MaxEnteros = 0
            Me.ntxtSuministros.Millares = False
            Me.ntxtSuministros.Name = "ntxtSuministros"
            Me.ntxtSuministros.Size_AdjustableHeight = 20
            Me.ntxtSuministros.TeclasDeshacer = True
            Me.ntxtSuministros.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picSuministros
            '
            Me.picSuministros.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picSuministros, "picSuministros")
            Me.picSuministros.Name = "picSuministros"
            Me.picSuministros.TabStop = False
            '
            'ntxtRepuestos
            '
            Me.ntxtRepuestos.AceptaNegativos = False
            Me.ntxtRepuestos.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.ntxtRepuestos.EstiloSBO = True
            resources.ApplyResources(Me.ntxtRepuestos, "ntxtRepuestos")
            Me.ntxtRepuestos.MaxDecimales = 0
            Me.ntxtRepuestos.MaxEnteros = 0
            Me.ntxtRepuestos.Millares = False
            Me.ntxtRepuestos.Name = "ntxtRepuestos"
            Me.ntxtRepuestos.Size_AdjustableHeight = 20
            Me.ntxtRepuestos.TeclasDeshacer = True
            Me.ntxtRepuestos.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picRepuestos
            '
            Me.picRepuestos.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picRepuestos, "picRepuestos")
            Me.picRepuestos.Name = "picRepuestos"
            Me.picRepuestos.TabStop = False
            '
            'ntxtProcesos
            '
            Me.ntxtProcesos.AceptaNegativos = False
            Me.ntxtProcesos.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.ntxtProcesos.EstiloSBO = True
            resources.ApplyResources(Me.ntxtProcesos, "ntxtProcesos")
            Me.ntxtProcesos.MaxDecimales = 0
            Me.ntxtProcesos.MaxEnteros = 0
            Me.ntxtProcesos.Millares = False
            Me.ntxtProcesos.Name = "ntxtProcesos"
            Me.ntxtProcesos.Size_AdjustableHeight = 20
            Me.ntxtProcesos.TeclasDeshacer = True
            Me.ntxtProcesos.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picProceso
            '
            Me.picProceso.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picProceso, "picProceso")
            Me.picProceso.Name = "picProceso"
            Me.picProceso.TabStop = False
            '
            'Label10
            '
            Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label10, "Label10")
            Me.Label10.Name = "Label10"
            '
            'Label9
            '
            Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label9, "Label9")
            Me.Label9.Name = "Label9"
            '
            'Label8
            '
            Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label8, "Label8")
            Me.Label8.Name = "Label8"
            '
            'Label7
            '
            Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label7, "Label7")
            Me.Label7.Name = "Label7"
            '
            'lblSE
            '
            resources.ApplyResources(Me.lblSE, "lblSE")
            Me.lblSE.Name = "lblSE"
            '
            'lblSuministros
            '
            resources.ApplyResources(Me.lblSuministros, "lblSuministros")
            Me.lblSuministros.Name = "lblSuministros"
            '
            'lblRefacciones
            '
            resources.ApplyResources(Me.lblRefacciones, "lblRefacciones")
            Me.lblRefacciones.Name = "lblRefacciones"
            '
            'lblBodegaDeProcesos
            '
            resources.ApplyResources(Me.lblBodegaDeProcesos, "lblBodegaDeProcesos")
            Me.lblBodegaDeProcesos.Name = "lblBodegaDeProcesos"
            '
            'tpMensajeria
            '
            Me.tpMensajeria.Controls.Add(Me.GroupBox2)
            Me.tpMensajeria.Controls.Add(Me.GroupBox1)
            resources.ApplyResources(Me.tpMensajeria, "tpMensajeria")
            Me.tpMensajeria.Name = "tpMensajeria"
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.chkUsaMensajeriaXCentroCosto)
            Me.GroupBox2.Controls.Add(Me.ntxtIntervaloMen)
            Me.GroupBox2.Controls.Add(Me.Label6)
            Me.GroupBox2.Controls.Add(Me.Label13)
            Me.GroupBox2.Controls.Add(Me.lblIntervaloMensajeria)
            resources.ApplyResources(Me.GroupBox2, "GroupBox2")
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.TabStop = False
            '
            'chkUsaMensajeriaXCentroCosto
            '
            resources.ApplyResources(Me.chkUsaMensajeriaXCentroCosto, "chkUsaMensajeriaXCentroCosto")
            Me.chkUsaMensajeriaXCentroCosto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkUsaMensajeriaXCentroCosto.Name = "chkUsaMensajeriaXCentroCosto"
            '
            'ntxtIntervaloMen
            '
            Me.ntxtIntervaloMen.AceptaNegativos = False
            Me.ntxtIntervaloMen.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.ntxtIntervaloMen.EstiloSBO = True
            resources.ApplyResources(Me.ntxtIntervaloMen, "ntxtIntervaloMen")
            Me.ntxtIntervaloMen.MaxDecimales = 0
            Me.ntxtIntervaloMen.MaxEnteros = 0
            Me.ntxtIntervaloMen.Millares = False
            Me.ntxtIntervaloMen.Name = "ntxtIntervaloMen"
            Me.ntxtIntervaloMen.Size_AdjustableHeight = 20
            Me.ntxtIntervaloMen.TeclasDeshacer = True
            Me.ntxtIntervaloMen.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.Numeric
            '
            'Label6
            '
            resources.ApplyResources(Me.Label6, "Label6")
            Me.Label6.Name = "Label6"
            '
            'Label13
            '
            Me.Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label13, "Label13")
            Me.Label13.Name = "Label13"
            '
            'lblIntervaloMensajeria
            '
            resources.ApplyResources(Me.lblIntervaloMensajeria, "lblIntervaloMensajeria")
            Me.lblIntervaloMensajeria.Name = "lblIntervaloMensajeria"
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.picEncargadoAccesorios)
            Me.GroupBox1.Controls.Add(Me.txtEncargadoAccesorios)
            Me.GroupBox1.Controls.Add(Me.Label40)
            Me.GroupBox1.Controls.Add(Me.lblBodAccesorios)
            Me.GroupBox1.Controls.Add(Me.picEncargadoOrdenCompra)
            Me.GroupBox1.Controls.Add(Me.txtEncargadoOrdenCompra)
            Me.GroupBox1.Controls.Add(Me.Label37)
            Me.GroupBox1.Controls.Add(Me.Label38)
            Me.GroupBox1.Controls.Add(Me.picEncargadoSuministros)
            Me.GroupBox1.Controls.Add(Me.txtEncargadoSuministros)
            Me.GroupBox1.Controls.Add(Me.Label24)
            Me.GroupBox1.Controls.Add(Me.Label25)
            Me.GroupBox1.Controls.Add(Me.picEncargadoRepuestos)
            Me.GroupBox1.Controls.Add(Me.txtEncargadoRepuestos)
            Me.GroupBox1.Controls.Add(Me.Label19)
            Me.GroupBox1.Controls.Add(Me.lblEncargadoRepuestos)
            Me.GroupBox1.Controls.Add(Me.picencargadoproduccion)
            Me.GroupBox1.Controls.Add(Me.picEncargadoBodega)
            Me.GroupBox1.Controls.Add(Me.ntxtEncargadoProduccion)
            Me.GroupBox1.Controls.Add(Me.ntxtEncargadoBodega)
            Me.GroupBox1.Controls.Add(Me.Label15)
            Me.GroupBox1.Controls.Add(Me.Label17)
            Me.GroupBox1.Controls.Add(Me.lblBodeguero)
            Me.GroupBox1.Controls.Add(Me.lblEncargadoProduccion)
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.TabStop = False
            '
            'picEncargadoAccesorios
            '
            Me.picEncargadoAccesorios.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picEncargadoAccesorios, "picEncargadoAccesorios")
            Me.picEncargadoAccesorios.Name = "picEncargadoAccesorios"
            Me.picEncargadoAccesorios.TabStop = False
            '
            'txtEncargadoAccesorios
            '
            Me.txtEncargadoAccesorios.AceptaNegativos = False
            Me.txtEncargadoAccesorios.BackColor = System.Drawing.Color.White
            Me.txtEncargadoAccesorios.EstiloSBO = True
            resources.ApplyResources(Me.txtEncargadoAccesorios, "txtEncargadoAccesorios")
            Me.txtEncargadoAccesorios.MaxDecimales = 0
            Me.txtEncargadoAccesorios.MaxEnteros = 0
            Me.txtEncargadoAccesorios.Millares = False
            Me.txtEncargadoAccesorios.Name = "txtEncargadoAccesorios"
            Me.txtEncargadoAccesorios.Size_AdjustableHeight = 20
            Me.txtEncargadoAccesorios.TeclasDeshacer = True
            Me.txtEncargadoAccesorios.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label40
            '
            Me.Label40.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label40, "Label40")
            Me.Label40.Name = "Label40"
            '
            'lblBodAccesorios
            '
            resources.ApplyResources(Me.lblBodAccesorios, "lblBodAccesorios")
            Me.lblBodAccesorios.Name = "lblBodAccesorios"
            '
            'picEncargadoOrdenCompra
            '
            Me.picEncargadoOrdenCompra.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picEncargadoOrdenCompra, "picEncargadoOrdenCompra")
            Me.picEncargadoOrdenCompra.Name = "picEncargadoOrdenCompra"
            Me.picEncargadoOrdenCompra.TabStop = False
            '
            'txtEncargadoOrdenCompra
            '
            Me.txtEncargadoOrdenCompra.AceptaNegativos = False
            Me.txtEncargadoOrdenCompra.BackColor = System.Drawing.Color.White
            Me.txtEncargadoOrdenCompra.EstiloSBO = True
            resources.ApplyResources(Me.txtEncargadoOrdenCompra, "txtEncargadoOrdenCompra")
            Me.txtEncargadoOrdenCompra.MaxDecimales = 0
            Me.txtEncargadoOrdenCompra.MaxEnteros = 0
            Me.txtEncargadoOrdenCompra.Millares = False
            Me.txtEncargadoOrdenCompra.Name = "txtEncargadoOrdenCompra"
            Me.txtEncargadoOrdenCompra.Size_AdjustableHeight = 20
            Me.txtEncargadoOrdenCompra.TeclasDeshacer = True
            Me.txtEncargadoOrdenCompra.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label37
            '
            Me.Label37.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label37, "Label37")
            Me.Label37.Name = "Label37"
            '
            'Label38
            '
            resources.ApplyResources(Me.Label38, "Label38")
            Me.Label38.Name = "Label38"
            '
            'picEncargadoSuministros
            '
            Me.picEncargadoSuministros.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picEncargadoSuministros, "picEncargadoSuministros")
            Me.picEncargadoSuministros.Name = "picEncargadoSuministros"
            Me.picEncargadoSuministros.TabStop = False
            '
            'txtEncargadoSuministros
            '
            Me.txtEncargadoSuministros.AceptaNegativos = False
            Me.txtEncargadoSuministros.BackColor = System.Drawing.Color.White
            Me.txtEncargadoSuministros.EstiloSBO = True
            resources.ApplyResources(Me.txtEncargadoSuministros, "txtEncargadoSuministros")
            Me.txtEncargadoSuministros.MaxDecimales = 0
            Me.txtEncargadoSuministros.MaxEnteros = 0
            Me.txtEncargadoSuministros.Millares = False
            Me.txtEncargadoSuministros.Name = "txtEncargadoSuministros"
            Me.txtEncargadoSuministros.Size_AdjustableHeight = 20
            Me.txtEncargadoSuministros.TeclasDeshacer = True
            Me.txtEncargadoSuministros.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
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
            Me.Label25.Name = "Label25"
            '
            'picEncargadoRepuestos
            '
            Me.picEncargadoRepuestos.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picEncargadoRepuestos, "picEncargadoRepuestos")
            Me.picEncargadoRepuestos.Name = "picEncargadoRepuestos"
            Me.picEncargadoRepuestos.TabStop = False
            '
            'txtEncargadoRepuestos
            '
            Me.txtEncargadoRepuestos.AceptaNegativos = False
            Me.txtEncargadoRepuestos.BackColor = System.Drawing.Color.White
            Me.txtEncargadoRepuestos.EstiloSBO = True
            resources.ApplyResources(Me.txtEncargadoRepuestos, "txtEncargadoRepuestos")
            Me.txtEncargadoRepuestos.MaxDecimales = 0
            Me.txtEncargadoRepuestos.MaxEnteros = 0
            Me.txtEncargadoRepuestos.Millares = False
            Me.txtEncargadoRepuestos.Name = "txtEncargadoRepuestos"
            Me.txtEncargadoRepuestos.Size_AdjustableHeight = 20
            Me.txtEncargadoRepuestos.TeclasDeshacer = True
            Me.txtEncargadoRepuestos.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label19
            '
            Me.Label19.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label19, "Label19")
            Me.Label19.Name = "Label19"
            '
            'lblEncargadoRepuestos
            '
            resources.ApplyResources(Me.lblEncargadoRepuestos, "lblEncargadoRepuestos")
            Me.lblEncargadoRepuestos.Name = "lblEncargadoRepuestos"
            '
            'picencargadoproduccion
            '
            Me.picencargadoproduccion.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picencargadoproduccion, "picencargadoproduccion")
            Me.picencargadoproduccion.Name = "picencargadoproduccion"
            Me.picencargadoproduccion.TabStop = False
            '
            'picEncargadoBodega
            '
            Me.picEncargadoBodega.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picEncargadoBodega, "picEncargadoBodega")
            Me.picEncargadoBodega.Name = "picEncargadoBodega"
            Me.picEncargadoBodega.TabStop = False
            '
            'ntxtEncargadoProduccion
            '
            Me.ntxtEncargadoProduccion.AceptaNegativos = False
            Me.ntxtEncargadoProduccion.BackColor = System.Drawing.Color.White
            Me.ntxtEncargadoProduccion.EstiloSBO = True
            resources.ApplyResources(Me.ntxtEncargadoProduccion, "ntxtEncargadoProduccion")
            Me.ntxtEncargadoProduccion.MaxDecimales = 0
            Me.ntxtEncargadoProduccion.MaxEnteros = 0
            Me.ntxtEncargadoProduccion.Millares = False
            Me.ntxtEncargadoProduccion.Name = "ntxtEncargadoProduccion"
            Me.ntxtEncargadoProduccion.Size_AdjustableHeight = 20
            Me.ntxtEncargadoProduccion.TeclasDeshacer = True
            Me.ntxtEncargadoProduccion.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'ntxtEncargadoBodega
            '
            Me.ntxtEncargadoBodega.AceptaNegativos = False
            Me.ntxtEncargadoBodega.BackColor = System.Drawing.Color.White
            Me.ntxtEncargadoBodega.EstiloSBO = True
            resources.ApplyResources(Me.ntxtEncargadoBodega, "ntxtEncargadoBodega")
            Me.ntxtEncargadoBodega.MaxDecimales = 0
            Me.ntxtEncargadoBodega.MaxEnteros = 0
            Me.ntxtEncargadoBodega.Millares = False
            Me.ntxtEncargadoBodega.Name = "ntxtEncargadoBodega"
            Me.ntxtEncargadoBodega.Size_AdjustableHeight = 20
            Me.ntxtEncargadoBodega.TeclasDeshacer = True
            Me.ntxtEncargadoBodega.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label15
            '
            Me.Label15.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label15, "Label15")
            Me.Label15.Name = "Label15"
            '
            'Label17
            '
            Me.Label17.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label17, "Label17")
            Me.Label17.Name = "Label17"
            '
            'lblBodeguero
            '
            resources.ApplyResources(Me.lblBodeguero, "lblBodeguero")
            Me.lblBodeguero.Name = "lblBodeguero"
            '
            'lblEncargadoProduccion
            '
            resources.ApplyResources(Me.lblEncargadoProduccion, "lblEncargadoProduccion")
            Me.lblEncargadoProduccion.Name = "lblEncargadoProduccion"
            '
            'tpImpuestos
            '
            Me.tpImpuestos.Controls.Add(Me.grpImpuestos)
            resources.ApplyResources(Me.tpImpuestos, "tpImpuestos")
            Me.tpImpuestos.Name = "tpImpuestos"
            '
            'grpImpuestos
            '
            Me.grpImpuestos.Controls.Add(Me.Label29)
            Me.grpImpuestos.Controls.Add(Me.Label30)
            Me.grpImpuestos.Controls.Add(Me.Label31)
            Me.grpImpuestos.Controls.Add(Me.Label32)
            Me.grpImpuestos.Controls.Add(Me.txtImpServiciosExternos)
            Me.grpImpuestos.Controls.Add(Me.picImpServiciosExternos)
            Me.grpImpuestos.Controls.Add(Me.Label11)
            Me.grpImpuestos.Controls.Add(Me.txtImpSuministros)
            Me.grpImpuestos.Controls.Add(Me.picImpSuministros)
            Me.grpImpuestos.Controls.Add(Me.Label14)
            Me.grpImpuestos.Controls.Add(Me.txtImpRefacciones)
            Me.grpImpuestos.Controls.Add(Me.picImpRefacciones)
            Me.grpImpuestos.Controls.Add(Me.Label18)
            Me.grpImpuestos.Controls.Add(Me.txtImpServicios)
            Me.grpImpuestos.Controls.Add(Me.picImpServicios)
            Me.grpImpuestos.Controls.Add(Me.Label20)
            resources.ApplyResources(Me.grpImpuestos, "grpImpuestos")
            Me.grpImpuestos.Name = "grpImpuestos"
            Me.grpImpuestos.TabStop = False
            '
            'Label29
            '
            resources.ApplyResources(Me.Label29, "Label29")
            Me.Label29.Name = "Label29"
            '
            'Label30
            '
            resources.ApplyResources(Me.Label30, "Label30")
            Me.Label30.Name = "Label30"
            '
            'Label31
            '
            resources.ApplyResources(Me.Label31, "Label31")
            Me.Label31.Name = "Label31"
            '
            'Label32
            '
            resources.ApplyResources(Me.Label32, "Label32")
            Me.Label32.Name = "Label32"
            '
            'txtImpServiciosExternos
            '
            Me.txtImpServiciosExternos.AceptaNegativos = False
            Me.txtImpServiciosExternos.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtImpServiciosExternos.EstiloSBO = True
            resources.ApplyResources(Me.txtImpServiciosExternos, "txtImpServiciosExternos")
            Me.txtImpServiciosExternos.MaxDecimales = 0
            Me.txtImpServiciosExternos.MaxEnteros = 0
            Me.txtImpServiciosExternos.Millares = False
            Me.txtImpServiciosExternos.Name = "txtImpServiciosExternos"
            Me.txtImpServiciosExternos.Size_AdjustableHeight = 20
            Me.txtImpServiciosExternos.TeclasDeshacer = True
            Me.txtImpServiciosExternos.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picImpServiciosExternos
            '
            Me.picImpServiciosExternos.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picImpServiciosExternos, "picImpServiciosExternos")
            Me.picImpServiciosExternos.Name = "picImpServiciosExternos"
            Me.picImpServiciosExternos.TabStop = False
            '
            'Label11
            '
            Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label11, "Label11")
            Me.Label11.Name = "Label11"
            '
            'txtImpSuministros
            '
            Me.txtImpSuministros.AceptaNegativos = False
            Me.txtImpSuministros.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtImpSuministros.EstiloSBO = True
            resources.ApplyResources(Me.txtImpSuministros, "txtImpSuministros")
            Me.txtImpSuministros.MaxDecimales = 0
            Me.txtImpSuministros.MaxEnteros = 0
            Me.txtImpSuministros.Millares = False
            Me.txtImpSuministros.Name = "txtImpSuministros"
            Me.txtImpSuministros.Size_AdjustableHeight = 20
            Me.txtImpSuministros.TeclasDeshacer = True
            Me.txtImpSuministros.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picImpSuministros
            '
            Me.picImpSuministros.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picImpSuministros, "picImpSuministros")
            Me.picImpSuministros.Name = "picImpSuministros"
            Me.picImpSuministros.TabStop = False
            '
            'Label14
            '
            Me.Label14.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label14, "Label14")
            Me.Label14.Name = "Label14"
            '
            'txtImpRefacciones
            '
            Me.txtImpRefacciones.AceptaNegativos = False
            Me.txtImpRefacciones.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtImpRefacciones.EstiloSBO = True
            resources.ApplyResources(Me.txtImpRefacciones, "txtImpRefacciones")
            Me.txtImpRefacciones.MaxDecimales = 0
            Me.txtImpRefacciones.MaxEnteros = 0
            Me.txtImpRefacciones.Millares = False
            Me.txtImpRefacciones.Name = "txtImpRefacciones"
            Me.txtImpRefacciones.Size_AdjustableHeight = 20
            Me.txtImpRefacciones.TeclasDeshacer = True
            Me.txtImpRefacciones.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picImpRefacciones
            '
            Me.picImpRefacciones.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picImpRefacciones, "picImpRefacciones")
            Me.picImpRefacciones.Name = "picImpRefacciones"
            Me.picImpRefacciones.TabStop = False
            '
            'Label18
            '
            Me.Label18.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label18, "Label18")
            Me.Label18.Name = "Label18"
            '
            'txtImpServicios
            '
            Me.txtImpServicios.AceptaNegativos = False
            Me.txtImpServicios.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtImpServicios.EstiloSBO = True
            resources.ApplyResources(Me.txtImpServicios, "txtImpServicios")
            Me.txtImpServicios.MaxDecimales = 0
            Me.txtImpServicios.MaxEnteros = 0
            Me.txtImpServicios.Millares = False
            Me.txtImpServicios.Name = "txtImpServicios"
            Me.txtImpServicios.Size_AdjustableHeight = 20
            Me.txtImpServicios.TeclasDeshacer = True
            Me.txtImpServicios.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picImpServicios
            '
            Me.picImpServicios.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picImpServicios, "picImpServicios")
            Me.picImpServicios.Name = "picImpServicios"
            Me.picImpServicios.TabStop = False
            '
            'Label20
            '
            Me.Label20.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label20, "Label20")
            Me.Label20.Name = "Label20"
            '
            'tabRepuestosExternos
            '
            Me.tabRepuestosExternos.Controls.Add(Me.GroupBox4)
            resources.ApplyResources(Me.tabRepuestosExternos, "tabRepuestosExternos")
            Me.tabRepuestosExternos.Name = "tabRepuestosExternos"
            '
            'GroupBox4
            '
            Me.GroupBox4.Controls.Add(Me.picDireccionB2B)
            Me.GroupBox4.Controls.Add(Me.txtDireccionB2b)
            Me.GroupBox4.Controls.Add(Me.Label21)
            Me.GroupBox4.Controls.Add(Me.Label22)
            Me.GroupBox4.Controls.Add(Me.btnEliminar)
            Me.GroupBox4.Controls.Add(Me.btnAgregar)
            Me.GroupBox4.Controls.Add(Me.dtgMarcasConfiguradas)
            Me.GroupBox4.Controls.Add(Me.chkCatalogosExternos)
            resources.ApplyResources(Me.GroupBox4, "GroupBox4")
            Me.GroupBox4.Name = "GroupBox4"
            Me.GroupBox4.TabStop = False
            '
            'picDireccionB2B
            '
            Me.picDireccionB2B.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picDireccionB2B, "picDireccionB2B")
            Me.picDireccionB2B.Name = "picDireccionB2B"
            Me.picDireccionB2B.TabStop = False
            '
            'txtDireccionB2b
            '
            Me.txtDireccionB2b.AceptaNegativos = False
            Me.txtDireccionB2b.BackColor = System.Drawing.Color.White
            Me.txtDireccionB2b.EstiloSBO = True
            resources.ApplyResources(Me.txtDireccionB2b, "txtDireccionB2b")
            Me.txtDireccionB2b.MaxDecimales = 0
            Me.txtDireccionB2b.MaxEnteros = 0
            Me.txtDireccionB2b.Millares = False
            Me.txtDireccionB2b.Name = "txtDireccionB2b"
            Me.txtDireccionB2b.Size_AdjustableHeight = 20
            Me.txtDireccionB2b.TeclasDeshacer = True
            Me.txtDireccionB2b.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label21
            '
            Me.Label21.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label21, "Label21")
            Me.Label21.Name = "Label21"
            '
            'Label22
            '
            resources.ApplyResources(Me.Label22, "Label22")
            Me.Label22.Name = "Label22"
            '
            'btnEliminar
            '
            resources.ApplyResources(Me.btnEliminar, "btnEliminar")
            Me.btnEliminar.ForeColor = System.Drawing.Color.Black
            Me.btnEliminar.Name = "btnEliminar"
            '
            'btnAgregar
            '
            resources.ApplyResources(Me.btnAgregar, "btnAgregar")
            Me.btnAgregar.ForeColor = System.Drawing.Color.Black
            Me.btnAgregar.Name = "btnAgregar"
            '
            'dtgMarcasConfiguradas
            '
            Me.dtgMarcasConfiguradas.AllowUserToAddRows = False
            Me.dtgMarcasConfiguradas.AllowUserToDeleteRows = False
            Me.dtgMarcasConfiguradas.AutoGenerateColumns = False
            Me.dtgMarcasConfiguradas.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgMarcasConfiguradas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgMarcasConfiguradas.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Check, Me.IDDataGridViewTextBoxColumn, Me.DescMarcaDataGridViewTextBoxColumn, Me.ServidorDataGridViewTextBoxColumn, Me.CompañiaDataGridViewTextBoxColumn, Me.UsuarioServidorDataGridViewTextBoxColumn, Me.PasswordServidorDataGridViewTextBoxColumn, Me.BDCompañiaDataGridViewTextBoxColumn, Me.CodAlmacenDataGridViewTextBoxColumn, Me.CodListaPrecioDataGridViewTextBoxColumn, Me.NombAlmacenDataGridViewTextBoxColumn, Me.NombListaPreciosDataGridViewTextBoxColumn, Me.UsuarioSBODataGridViewTextBoxColumn, Me.PasswordSBODataGridViewTextBoxColumn})
            Me.dtgMarcasConfiguradas.DataMember = "SCGTA_TB_ConfCatalogoRepxMarca"
            Me.dtgMarcasConfiguradas.DataSource = ConfCatalogoRepXMarcaDataset1
            resources.ApplyResources(Me.dtgMarcasConfiguradas, "dtgMarcasConfiguradas")
            Me.dtgMarcasConfiguradas.Name = "dtgMarcasConfiguradas"
            '
            'Check
            '
            Me.Check.DataPropertyName = "Check"
            Me.Check.HeaderText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
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
            'DescMarcaDataGridViewTextBoxColumn
            '
            Me.DescMarcaDataGridViewTextBoxColumn.DataPropertyName = "DescMarca"
            resources.ApplyResources(Me.DescMarcaDataGridViewTextBoxColumn, "DescMarcaDataGridViewTextBoxColumn")
            Me.DescMarcaDataGridViewTextBoxColumn.Name = "DescMarcaDataGridViewTextBoxColumn"
            Me.DescMarcaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ServidorDataGridViewTextBoxColumn
            '
            Me.ServidorDataGridViewTextBoxColumn.DataPropertyName = "Servidor"
            resources.ApplyResources(Me.ServidorDataGridViewTextBoxColumn, "ServidorDataGridViewTextBoxColumn")
            Me.ServidorDataGridViewTextBoxColumn.Name = "ServidorDataGridViewTextBoxColumn"
            Me.ServidorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CompañiaDataGridViewTextBoxColumn
            '
            Me.CompañiaDataGridViewTextBoxColumn.DataPropertyName = "Compañia"
            resources.ApplyResources(Me.CompañiaDataGridViewTextBoxColumn, "CompañiaDataGridViewTextBoxColumn")
            Me.CompañiaDataGridViewTextBoxColumn.Name = "CompañiaDataGridViewTextBoxColumn"
            Me.CompañiaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'UsuarioServidorDataGridViewTextBoxColumn
            '
            Me.UsuarioServidorDataGridViewTextBoxColumn.DataPropertyName = "UsuarioServidor"
            resources.ApplyResources(Me.UsuarioServidorDataGridViewTextBoxColumn, "UsuarioServidorDataGridViewTextBoxColumn")
            Me.UsuarioServidorDataGridViewTextBoxColumn.Name = "UsuarioServidorDataGridViewTextBoxColumn"
            '
            'PasswordServidorDataGridViewTextBoxColumn
            '
            Me.PasswordServidorDataGridViewTextBoxColumn.DataPropertyName = "PasswordServidor"
            resources.ApplyResources(Me.PasswordServidorDataGridViewTextBoxColumn, "PasswordServidorDataGridViewTextBoxColumn")
            Me.PasswordServidorDataGridViewTextBoxColumn.Name = "PasswordServidorDataGridViewTextBoxColumn"
            '
            'BDCompañiaDataGridViewTextBoxColumn
            '
            Me.BDCompañiaDataGridViewTextBoxColumn.DataPropertyName = "BDCompañia"
            resources.ApplyResources(Me.BDCompañiaDataGridViewTextBoxColumn, "BDCompañiaDataGridViewTextBoxColumn")
            Me.BDCompañiaDataGridViewTextBoxColumn.Name = "BDCompañiaDataGridViewTextBoxColumn"
            '
            'CodAlmacenDataGridViewTextBoxColumn
            '
            Me.CodAlmacenDataGridViewTextBoxColumn.DataPropertyName = "CodAlmacen"
            resources.ApplyResources(Me.CodAlmacenDataGridViewTextBoxColumn, "CodAlmacenDataGridViewTextBoxColumn")
            Me.CodAlmacenDataGridViewTextBoxColumn.Name = "CodAlmacenDataGridViewTextBoxColumn"
            '
            'CodListaPrecioDataGridViewTextBoxColumn
            '
            Me.CodListaPrecioDataGridViewTextBoxColumn.DataPropertyName = "CodListaPrecio"
            resources.ApplyResources(Me.CodListaPrecioDataGridViewTextBoxColumn, "CodListaPrecioDataGridViewTextBoxColumn")
            Me.CodListaPrecioDataGridViewTextBoxColumn.Name = "CodListaPrecioDataGridViewTextBoxColumn"
            '
            'NombAlmacenDataGridViewTextBoxColumn
            '
            Me.NombAlmacenDataGridViewTextBoxColumn.DataPropertyName = "NombAlmacen"
            resources.ApplyResources(Me.NombAlmacenDataGridViewTextBoxColumn, "NombAlmacenDataGridViewTextBoxColumn")
            Me.NombAlmacenDataGridViewTextBoxColumn.Name = "NombAlmacenDataGridViewTextBoxColumn"
            '
            'NombListaPreciosDataGridViewTextBoxColumn
            '
            Me.NombListaPreciosDataGridViewTextBoxColumn.DataPropertyName = "NombListaPrecios"
            resources.ApplyResources(Me.NombListaPreciosDataGridViewTextBoxColumn, "NombListaPreciosDataGridViewTextBoxColumn")
            Me.NombListaPreciosDataGridViewTextBoxColumn.Name = "NombListaPreciosDataGridViewTextBoxColumn"
            '
            'UsuarioSBODataGridViewTextBoxColumn
            '
            Me.UsuarioSBODataGridViewTextBoxColumn.DataPropertyName = "UsuarioSBO"
            resources.ApplyResources(Me.UsuarioSBODataGridViewTextBoxColumn, "UsuarioSBODataGridViewTextBoxColumn")
            Me.UsuarioSBODataGridViewTextBoxColumn.Name = "UsuarioSBODataGridViewTextBoxColumn"
            '
            'PasswordSBODataGridViewTextBoxColumn
            '
            Me.PasswordSBODataGridViewTextBoxColumn.DataPropertyName = "PasswordSBO"
            resources.ApplyResources(Me.PasswordSBODataGridViewTextBoxColumn, "PasswordSBODataGridViewTextBoxColumn")
            Me.PasswordSBODataGridViewTextBoxColumn.Name = "PasswordSBODataGridViewTextBoxColumn"
            '
            'chkCatalogosExternos
            '
            resources.ApplyResources(Me.chkCatalogosExternos, "chkCatalogosExternos")
            Me.chkCatalogosExternos.Name = "chkCatalogosExternos"
            '
            'tabCosteo
            '
            Me.tabCosteo.Controls.Add(Me.GroupBox15)
            Me.tabCosteo.Controls.Add(Me.lblCuentaContable)
            Me.tabCosteo.Controls.Add(Me.txtNombreCuenta)
            Me.tabCosteo.Controls.Add(Me.txtNumeroCuenta)
            Me.tabCosteo.Controls.Add(Me.piCuentasContables)
            Me.tabCosteo.Controls.Add(Me.GroupBox10)
            Me.tabCosteo.Controls.Add(Me.GroupBox9)
            Me.tabCosteo.Controls.Add(Me.GroupBox7)
            resources.ApplyResources(Me.tabCosteo, "tabCosteo")
            Me.tabCosteo.Name = "tabCosteo"
            '
            'GroupBox15
            '
            Me.GroupBox15.Controls.Add(Me.Label43)
            Me.GroupBox15.Controls.Add(Me.picTipoMoneda)
            Me.GroupBox15.Controls.Add(Me.txtTipoMoneda)
            resources.ApplyResources(Me.GroupBox15, "GroupBox15")
            Me.GroupBox15.Name = "GroupBox15"
            Me.GroupBox15.TabStop = False
            '
            'Label43
            '
            resources.ApplyResources(Me.Label43, "Label43")
            Me.Label43.Name = "Label43"
            '
            'picTipoMoneda
            '
            Me.picTipoMoneda.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picTipoMoneda, "picTipoMoneda")
            Me.picTipoMoneda.Name = "picTipoMoneda"
            Me.picTipoMoneda.TabStop = False
            '
            'txtTipoMoneda
            '
            Me.txtTipoMoneda.AceptaNegativos = False
            Me.txtTipoMoneda.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtTipoMoneda.EstiloSBO = True
            resources.ApplyResources(Me.txtTipoMoneda, "txtTipoMoneda")
            Me.txtTipoMoneda.MaxDecimales = 0
            Me.txtTipoMoneda.MaxEnteros = 0
            Me.txtTipoMoneda.Millares = False
            Me.txtTipoMoneda.Name = "txtTipoMoneda"
            Me.txtTipoMoneda.ReadOnly = True
            Me.txtTipoMoneda.Size_AdjustableHeight = 20
            Me.txtTipoMoneda.TeclasDeshacer = True
            Me.txtTipoMoneda.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblCuentaContable
            '
            resources.ApplyResources(Me.lblCuentaContable, "lblCuentaContable")
            Me.lblCuentaContable.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCuentaContable.Name = "lblCuentaContable"
            '
            'txtNombreCuenta
            '
            Me.txtNombreCuenta.AceptaNegativos = False
            Me.txtNombreCuenta.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNombreCuenta.EstiloSBO = True
            resources.ApplyResources(Me.txtNombreCuenta, "txtNombreCuenta")
            Me.txtNombreCuenta.MaxDecimales = 0
            Me.txtNombreCuenta.MaxEnteros = 0
            Me.txtNombreCuenta.Millares = False
            Me.txtNombreCuenta.Name = "txtNombreCuenta"
            Me.txtNombreCuenta.ReadOnly = True
            Me.txtNombreCuenta.Size_AdjustableHeight = 20
            Me.txtNombreCuenta.TeclasDeshacer = True
            Me.txtNombreCuenta.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtNumeroCuenta
            '
            Me.txtNumeroCuenta.AceptaNegativos = False
            Me.txtNumeroCuenta.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNumeroCuenta.EstiloSBO = True
            resources.ApplyResources(Me.txtNumeroCuenta, "txtNumeroCuenta")
            Me.txtNumeroCuenta.MaxDecimales = 0
            Me.txtNumeroCuenta.MaxEnteros = 0
            Me.txtNumeroCuenta.Millares = False
            Me.txtNumeroCuenta.Name = "txtNumeroCuenta"
            Me.txtNumeroCuenta.ReadOnly = True
            Me.txtNumeroCuenta.Size_AdjustableHeight = 20
            Me.txtNumeroCuenta.TeclasDeshacer = True
            Me.txtNumeroCuenta.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'piCuentasContables
            '
            Me.piCuentasContables.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.piCuentasContables, "piCuentasContables")
            Me.piCuentasContables.Name = "piCuentasContables"
            Me.piCuentasContables.TabStop = False
            '
            'GroupBox10
            '
            Me.GroupBox10.Controls.Add(Me.chkOtrosGastos)
            resources.ApplyResources(Me.GroupBox10, "GroupBox10")
            Me.GroupBox10.Name = "GroupBox10"
            Me.GroupBox10.TabStop = False
            '
            'chkOtrosGastos
            '
            Me.chkOtrosGastos.Checked = True
            Me.chkOtrosGastos.CheckState = System.Windows.Forms.CheckState.Checked
            resources.ApplyResources(Me.chkOtrosGastos, "chkOtrosGastos")
            Me.chkOtrosGastos.Name = "chkOtrosGastos"
            '
            'GroupBox9
            '
            Me.GroupBox9.Controls.Add(Me.gbxTipoCostos)
            Me.GroupBox9.Controls.Add(Me.gbxTipoCosteoServicios)
            Me.GroupBox9.Controls.Add(Me.chkCosteoServicios)
            resources.ApplyResources(Me.GroupBox9, "GroupBox9")
            Me.GroupBox9.Name = "GroupBox9"
            Me.GroupBox9.TabStop = False
            '
            'gbxTipoCostos
            '
            Me.gbxTipoCostos.Controls.Add(Me.rbtDetallado)
            Me.gbxTipoCostos.Controls.Add(Me.rbtSimple)
            resources.ApplyResources(Me.gbxTipoCostos, "gbxTipoCostos")
            Me.gbxTipoCostos.Name = "gbxTipoCostos"
            Me.gbxTipoCostos.TabStop = False
            '
            'rbtDetallado
            '
            resources.ApplyResources(Me.rbtDetallado, "rbtDetallado")
            Me.rbtDetallado.Name = "rbtDetallado"
            Me.rbtDetallado.TabStop = True
            Me.rbtDetallado.UseVisualStyleBackColor = True
            '
            'rbtSimple
            '
            resources.ApplyResources(Me.rbtSimple, "rbtSimple")
            Me.rbtSimple.Name = "rbtSimple"
            Me.rbtSimple.TabStop = True
            Me.rbtSimple.UseVisualStyleBackColor = True
            '
            'gbxTipoCosteoServicios
            '
            Me.gbxTipoCosteoServicios.Controls.Add(Me.rbtTiempoReal)
            Me.gbxTipoCosteoServicios.Controls.Add(Me.rbtEstandar)
            resources.ApplyResources(Me.gbxTipoCosteoServicios, "gbxTipoCosteoServicios")
            Me.gbxTipoCosteoServicios.Name = "gbxTipoCosteoServicios"
            Me.gbxTipoCosteoServicios.TabStop = False
            '
            'rbtTiempoReal
            '
            resources.ApplyResources(Me.rbtTiempoReal, "rbtTiempoReal")
            Me.rbtTiempoReal.Name = "rbtTiempoReal"
            Me.rbtTiempoReal.TabStop = True
            Me.rbtTiempoReal.UseVisualStyleBackColor = True
            '
            'rbtEstandar
            '
            resources.ApplyResources(Me.rbtEstandar, "rbtEstandar")
            Me.rbtEstandar.Name = "rbtEstandar"
            Me.rbtEstandar.TabStop = True
            Me.rbtEstandar.UseVisualStyleBackColor = True
            '
            'chkCosteoServicios
            '
            resources.ApplyResources(Me.chkCosteoServicios, "chkCosteoServicios")
            Me.chkCosteoServicios.Name = "chkCosteoServicios"
            '
            'GroupBox7
            '
            Me.GroupBox7.Controls.Add(Me.chkSEInventariables)
            resources.ApplyResources(Me.GroupBox7, "GroupBox7")
            Me.GroupBox7.Name = "GroupBox7"
            Me.GroupBox7.TabStop = False
            '
            'chkSEInventariables
            '
            resources.ApplyResources(Me.chkSEInventariables, "chkSEInventariables")
            Me.chkSEInventariables.Name = "chkSEInventariables"
            '
            'tabCitas
            '
            Me.tabCitas.Controls.Add(Me.GroupBox11)
            resources.ApplyResources(Me.tabCitas, "tabCitas")
            Me.tabCitas.Name = "tabCitas"
            '
            'GroupBox11
            '
            Me.GroupBox11.Controls.Add(Me.picArticuloCotizacion)
            Me.GroupBox11.Controls.Add(Me.txtArtCotizacion)
            Me.GroupBox11.Controls.Add(Me.Label33)
            Me.GroupBox11.Controls.Add(Me.Label39)
            resources.ApplyResources(Me.GroupBox11, "GroupBox11")
            Me.GroupBox11.Name = "GroupBox11"
            Me.GroupBox11.TabStop = False
            '
            'picArticuloCotizacion
            '
            Me.picArticuloCotizacion.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picArticuloCotizacion, "picArticuloCotizacion")
            Me.picArticuloCotizacion.Name = "picArticuloCotizacion"
            Me.picArticuloCotizacion.TabStop = False
            '
            'txtArtCotizacion
            '
            Me.txtArtCotizacion.AceptaNegativos = False
            Me.txtArtCotizacion.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtArtCotizacion.EstiloSBO = True
            resources.ApplyResources(Me.txtArtCotizacion, "txtArtCotizacion")
            Me.txtArtCotizacion.MaxDecimales = 0
            Me.txtArtCotizacion.MaxEnteros = 0
            Me.txtArtCotizacion.Millares = False
            Me.txtArtCotizacion.Name = "txtArtCotizacion"
            Me.txtArtCotizacion.ReadOnly = True
            Me.txtArtCotizacion.Size_AdjustableHeight = 20
            Me.txtArtCotizacion.TeclasDeshacer = True
            Me.txtArtCotizacion.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label33
            '
            Me.Label33.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label33, "Label33")
            Me.Label33.Name = "Label33"
            '
            'Label39
            '
            resources.ApplyResources(Me.Label39, "Label39")
            Me.Label39.Name = "Label39"
            '
            'tabWeb
            '
            Me.tabWeb.BackColor = System.Drawing.SystemColors.Control
            Me.tabWeb.Controls.Add(Me.GroupBox14)
            resources.ApplyResources(Me.tabWeb, "tabWeb")
            Me.tabWeb.Name = "tabWeb"
            '
            'GroupBox14
            '
            Me.GroupBox14.Controls.Add(Me.chkOTTotales)
            Me.GroupBox14.Controls.Add(Me.chkOTRepuestos)
            resources.ApplyResources(Me.GroupBox14, "GroupBox14")
            Me.GroupBox14.Name = "GroupBox14"
            Me.GroupBox14.TabStop = False
            '
            'chkOTTotales
            '
            resources.ApplyResources(Me.chkOTTotales, "chkOTTotales")
            Me.chkOTTotales.Checked = True
            Me.chkOTTotales.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkOTTotales.Name = "chkOTTotales"
            Me.chkOTTotales.UseVisualStyleBackColor = True
            '
            'chkOTRepuestos
            '
            resources.ApplyResources(Me.chkOTRepuestos, "chkOTRepuestos")
            Me.chkOTRepuestos.Checked = True
            Me.chkOTRepuestos.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkOTRepuestos.Name = "chkOTRepuestos"
            Me.chkOTRepuestos.UseVisualStyleBackColor = True
            '
            'lblName
            '
            resources.ApplyResources(Me.lblName, "lblName")
            Me.lblName.Name = "lblName"
            '
            'btnCancelar
            '
            resources.ApplyResources(Me.btnCancelar, "btnCancelar")
            Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancelar.ForeColor = System.Drawing.Color.Black
            Me.btnCancelar.Name = "btnCancelar"
            '
            'btnAceptar
            '
            resources.ApplyResources(Me.btnAceptar, "btnAceptar")
            Me.btnAceptar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnAceptar.ForeColor = System.Drawing.Color.Black
            Me.btnAceptar.Name = "btnAceptar"
            '
            'fbdDireccionB2B
            '
            Me.fbdDireccionB2B.RootFolder = System.Environment.SpecialFolder.MyComputer
            '
            'frmConfiguracionApp
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.lblName)
            Me.Controls.Add(Me.btnCancelar)
            Me.Controls.Add(Me.btnAceptar)
            Me.Controls.Add(Me.tabConfiguracion)
            Me.Controls.Add(Me.lblSucursal)
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.MaximizeBox = False
            Me.Name = "frmConfiguracionApp"
            CType(ConfCatalogoRepXMarcaDataset1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabConfiguracion.ResumeLayout(False)
            Me.tpGenerales.ResumeLayout(False)
            Me.gbx_Tipo_Compra.ResumeLayout(False)
            Me.gbx_Tipo_Compra.PerformLayout()
            Me.GroupBox13.ResumeLayout(False)
            Me.GroupBox13.PerformLayout()
            Me.GroupBox12.ResumeLayout(False)
            Me.GroupBox8.ResumeLayout(False)
            Me.GroupBox8.PerformLayout()
            CType(Me.picUnidadesTiempo, System.ComponentModel.ISupportInitialize).EndInit()
            Me.GroupBox6.ResumeLayout(False)
            Me.GroupBox6.PerformLayout()
            Me.GroupBox5.ResumeLayout(False)
            Me.GroupBox5.PerformLayout()
            Me.GroupBox3.ResumeLayout(False)
            Me.GroupBox3.PerformLayout()
            CType(Me.picListaPrecios, System.ComponentModel.ISupportInitialize).EndInit()
            Me.gbArticulos.ResumeLayout(False)
            Me.gbArticulos.PerformLayout()
            Me.tpSeries.ResumeLayout(False)
            Me.gpDocInventario.ResumeLayout(False)
            Me.gpDocInventario.PerformLayout()
            CType(Me.picTraslados, System.ComponentModel.ISupportInitialize).EndInit()
            Me.gbVentas.ResumeLayout(False)
            Me.gbVentas.PerformLayout()
            CType(Me.picCotizaciones, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picOrdVentas, System.ComponentModel.ISupportInitialize).EndInit()
            Me.gbCompras.ResumeLayout(False)
            Me.gbCompras.PerformLayout()
            CType(Me.picOfertasdeCompra, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picOrdenesdeCompra, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpBodega.ResumeLayout(False)
            Me.gbBodegas.ResumeLayout(False)
            Me.gbBodegas.PerformLayout()
            CType(Me.PicSE, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picSuministros, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picRepuestos, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picProceso, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpMensajeria.ResumeLayout(False)
            Me.GroupBox2.ResumeLayout(False)
            Me.GroupBox2.PerformLayout()
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            CType(Me.picEncargadoAccesorios, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picEncargadoOrdenCompra, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picEncargadoSuministros, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picEncargadoRepuestos, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picencargadoproduccion, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picEncargadoBodega, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpImpuestos.ResumeLayout(False)
            Me.grpImpuestos.ResumeLayout(False)
            Me.grpImpuestos.PerformLayout()
            CType(Me.picImpServiciosExternos, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picImpSuministros, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picImpRefacciones, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picImpServicios, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabRepuestosExternos.ResumeLayout(False)
            Me.GroupBox4.ResumeLayout(False)
            Me.GroupBox4.PerformLayout()
            CType(Me.picDireccionB2B, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtgMarcasConfiguradas, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabCosteo.ResumeLayout(False)
            Me.tabCosteo.PerformLayout()
            Me.GroupBox15.ResumeLayout(False)
            Me.GroupBox15.PerformLayout()
            CType(Me.picTipoMoneda, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.piCuentasContables, System.ComponentModel.ISupportInitialize).EndInit()
            Me.GroupBox10.ResumeLayout(False)
            Me.GroupBox9.ResumeLayout(False)
            Me.gbxTipoCostos.ResumeLayout(False)
            Me.gbxTipoCostos.PerformLayout()
            Me.gbxTipoCosteoServicios.ResumeLayout(False)
            Me.gbxTipoCosteoServicios.PerformLayout()
            Me.GroupBox7.ResumeLayout(False)
            Me.tabCitas.ResumeLayout(False)
            Me.GroupBox11.ResumeLayout(False)
            Me.GroupBox11.PerformLayout()
            CType(Me.picArticuloCotizacion, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabWeb.ResumeLayout(False)
            Me.GroupBox14.ResumeLayout(False)
            Me.GroupBox14.PerformLayout()
            CType(Me.bsMarcasConfiguradas, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents lblSucursal As System.Windows.Forms.Label
        Friend WithEvents tabConfiguracion As System.Windows.Forms.TabControl
        Friend WithEvents tpGenerales As System.Windows.Forms.TabPage
        Friend WithEvents btnAceptar As System.Windows.Forms.Button
        Friend WithEvents btnCancelar As System.Windows.Forms.Button
        Friend WithEvents tpBodega As System.Windows.Forms.TabPage
        Friend WithEvents tpMensajeria As System.Windows.Forms.TabPage
        Friend WithEvents gbArticulos As System.Windows.Forms.GroupBox
        Friend WithEvents chkUsaSuministros As System.Windows.Forms.CheckBox
        Friend WithEvents chkUsaServiciosExternos As System.Windows.Forms.CheckBox
        Friend WithEvents chkUsaServicios As System.Windows.Forms.CheckBox
        Friend WithEvents chkUsaRepuestos As System.Windows.Forms.CheckBox

        '********************************************************************************************
        'Agregado 29/02/2012: Agregar configuración validación de tiempo estándar
        'Autor: José Soto
        Friend WithEvents chkUsaValTiempoEs As System.Windows.Forms.CheckBox
        '********************************************************************************************


        Friend WithEvents gbBodegas As System.Windows.Forms.GroupBox
        Friend WithEvents ntxtProcesos As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picProceso As System.Windows.Forms.PictureBox
        Friend WithEvents lblBodegaDeProcesos As System.Windows.Forms.Label
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents ntxtRepuestos As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picRepuestos As System.Windows.Forms.PictureBox
        Friend WithEvents lblRefacciones As System.Windows.Forms.Label
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents ntxtSuministros As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picSuministros As System.Windows.Forms.PictureBox
        Friend WithEvents lblSuministros As System.Windows.Forms.Label
        Friend WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents ntxtSE As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents PicSE As System.Windows.Forms.PictureBox
        Friend WithEvents lblSE As System.Windows.Forms.Label
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents lblName As System.Windows.Forms.Label
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents ntxtEncargadoProduccion As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picencargadoproduccion As System.Windows.Forms.PictureBox
        Friend WithEvents lblEncargadoProduccion As System.Windows.Forms.Label
        Friend WithEvents Label15 As System.Windows.Forms.Label
        Friend WithEvents ntxtEncargadoBodega As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picEncargadoBodega As System.Windows.Forms.PictureBox
        Friend WithEvents lblBodeguero As System.Windows.Forms.Label
        Friend WithEvents Label17 As System.Windows.Forms.Label
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents ntxtIntervaloMen As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblIntervaloMensajeria As System.Windows.Forms.Label
        Friend WithEvents Label13 As System.Windows.Forms.Label
        Friend WithEvents tpImpuestos As System.Windows.Forms.TabPage
        Friend WithEvents grpImpuestos As System.Windows.Forms.GroupBox
        Friend WithEvents txtImpServiciosExternos As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picImpServiciosExternos As System.Windows.Forms.PictureBox
        Friend WithEvents Label11 As System.Windows.Forms.Label
        Friend WithEvents txtImpSuministros As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picImpSuministros As System.Windows.Forms.PictureBox
        Friend WithEvents Label14 As System.Windows.Forms.Label
        Friend WithEvents txtImpRefacciones As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picImpRefacciones As System.Windows.Forms.PictureBox
        Friend WithEvents Label18 As System.Windows.Forms.Label
        Friend WithEvents txtImpServicios As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picImpServicios As System.Windows.Forms.PictureBox
        Friend WithEvents Label20 As System.Windows.Forms.Label
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
        Friend WithEvents picListaPrecios As System.Windows.Forms.PictureBox
        Friend WithEvents txtListaPrecios As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label12 As System.Windows.Forms.Label
        Friend WithEvents tabRepuestosExternos As System.Windows.Forms.TabPage
        Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
        Friend WithEvents dtgMarcasConfiguradas As System.Windows.Forms.DataGridView
        Friend WithEvents chkCatalogosExternos As System.Windows.Forms.CheckBox
        Friend WithEvents bsMarcasConfiguradas As System.Windows.Forms.BindingSource
        Friend WithEvents btnEliminar As System.Windows.Forms.Button
        Friend WithEvents btnAgregar As System.Windows.Forms.Button
        Friend WithEvents CodMarcarDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents picEncargadoRepuestos As System.Windows.Forms.PictureBox
        Friend WithEvents txtEncargadoRepuestos As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label19 As System.Windows.Forms.Label
        Friend WithEvents lblEncargadoRepuestos As System.Windows.Forms.Label
        Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
        Friend WithEvents chkGeneraOTsEspeciales As System.Windows.Forms.CheckBox
        Friend WithEvents picDireccionB2B As System.Windows.Forms.PictureBox
        Friend WithEvents txtDireccionB2b As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label21 As System.Windows.Forms.Label
        Friend WithEvents Label22 As System.Windows.Forms.Label
        Friend WithEvents fbdDireccionB2B As System.Windows.Forms.FolderBrowserDialog
        Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
        Friend WithEvents txtCopiasRepRecepcion As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label23 As System.Windows.Forms.Label
        Friend WithEvents lblCopiasRepRecepcion As System.Windows.Forms.Label
        Friend WithEvents picEncargadoSuministros As System.Windows.Forms.PictureBox
        Friend WithEvents txtEncargadoSuministros As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label24 As System.Windows.Forms.Label
        Friend WithEvents Label25 As System.Windows.Forms.Label
        Friend WithEvents tpSeries As System.Windows.Forms.TabPage
        Friend WithEvents gpDocInventario As System.Windows.Forms.GroupBox
        Friend WithEvents ntxtTraslados As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picTraslados As System.Windows.Forms.PictureBox
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents gbVentas As System.Windows.Forms.GroupBox
        Friend WithEvents ntxtOrdenVentas As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picOrdVentas As System.Windows.Forms.PictureBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents gbCompras As System.Windows.Forms.GroupBox
        Friend WithEvents ntxtOrdendeCompra As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picOrdenesdeCompra As System.Windows.Forms.PictureBox
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents lblOrdenes As System.Windows.Forms.Label
        Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
        Friend WithEvents picUnidadesTiempo As System.Windows.Forms.PictureBox
        Friend WithEvents txtUnidadTiempo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label28 As System.Windows.Forms.Label
        Friend WithEvents lblUnidadTiempo As System.Windows.Forms.Label
        Friend WithEvents Label26 As System.Windows.Forms.Label
        Friend WithEvents txtCotizaciones As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picCotizaciones As System.Windows.Forms.PictureBox
        Friend WithEvents Label27 As System.Windows.Forms.Label
        Friend WithEvents Label29 As System.Windows.Forms.Label
        Friend WithEvents Label30 As System.Windows.Forms.Label
        Friend WithEvents Label31 As System.Windows.Forms.Label
        Friend WithEvents Label32 As System.Windows.Forms.Label
        Friend WithEvents Check As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents IDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescMarcaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ServidorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CompañiaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents UsuarioServidorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents PasswordServidorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents BDCompañiaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodAlmacenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodListaPrecioDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NombAlmacenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NombListaPreciosDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents UsuarioSBODataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents PasswordSBODataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents tabCosteo As System.Windows.Forms.TabPage
        Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
        Friend WithEvents chkSEInventariables As System.Windows.Forms.CheckBox
        Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
        Friend WithEvents chkCosteoServicios As System.Windows.Forms.CheckBox
        Friend WithEvents gbxTipoCosteoServicios As System.Windows.Forms.GroupBox
        Friend WithEvents rbtEstandar As System.Windows.Forms.RadioButton
        Friend WithEvents Label34 As System.Windows.Forms.Label
        Friend WithEvents Label35 As System.Windows.Forms.Label
        Friend WithEvents Label36 As System.Windows.Forms.Label
        Friend WithEvents picEncargadoOrdenCompra As System.Windows.Forms.PictureBox
        Friend WithEvents txtEncargadoOrdenCompra As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label37 As System.Windows.Forms.Label
        Friend WithEvents Label38 As System.Windows.Forms.Label
        Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
        Friend WithEvents chkOtrosGastos As System.Windows.Forms.CheckBox
        Friend WithEvents tabCitas As System.Windows.Forms.TabPage
        Friend WithEvents GroupBox11 As System.Windows.Forms.GroupBox
        Friend WithEvents picArticuloCotizacion As System.Windows.Forms.PictureBox
        Friend WithEvents txtArtCotizacion As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label33 As System.Windows.Forms.Label
        Friend WithEvents Label39 As System.Windows.Forms.Label
        Friend WithEvents GroupBox12 As System.Windows.Forms.GroupBox
        Friend WithEvents chkUsaDraftTransferencia As System.Windows.Forms.CheckBox
        Friend WithEvents chkUsaAsignacionAutomaticaEncargadoOper As System.Windows.Forms.CheckBox
        Friend WithEvents lblBodAccesorios As System.Windows.Forms.Label
        Friend WithEvents txtEncargadoAccesorios As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label40 As System.Windows.Forms.Label
        Friend WithEvents picEncargadoAccesorios As System.Windows.Forms.PictureBox
        Friend WithEvents chkUsaMensajeriaXCentroCosto As System.Windows.Forms.CheckBox
        Friend WithEvents GroupBox13 As System.Windows.Forms.GroupBox
        Friend WithEvents chkUsaFiltroClientes As System.Windows.Forms.CheckBox
        Friend WithEvents lblCuentaContable As System.Windows.Forms.Label
        Friend WithEvents txtNombreCuenta As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtNumeroCuenta As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents piCuentasContables As System.Windows.Forms.PictureBox
        Friend WithEvents gbxTipoCostos As System.Windows.Forms.GroupBox
        Friend WithEvents rbtDetallado As System.Windows.Forms.RadioButton
        Friend WithEvents rbtSimple As System.Windows.Forms.RadioButton
        Friend WithEvents gbx_Tipo_Compra As System.Windows.Forms.GroupBox
        Friend WithEvents rb_OfertaCompra As System.Windows.Forms.RadioButton
        Friend WithEvents rb_OrdenCompra As System.Windows.Forms.RadioButton
        Friend WithEvents ntxtOfertadeCompra As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picOfertasdeCompra As System.Windows.Forms.PictureBox
        Friend WithEvents Label41 As System.Windows.Forms.Label
        Friend WithEvents Label42 As System.Windows.Forms.Label
        Friend WithEvents chkCitasCliInv As System.Windows.Forms.CheckBox
        Friend WithEvents chkFinalizaOTCantSolicitada As System.Windows.Forms.CheckBox
        Friend WithEvents tabWeb As System.Windows.Forms.TabPage
        Friend WithEvents GroupBox14 As System.Windows.Forms.GroupBox
        Friend WithEvents chkOTTotales As System.Windows.Forms.CheckBox
        Friend WithEvents chkOTRepuestos As System.Windows.Forms.CheckBox
        Friend WithEvents GroupBox15 As System.Windows.Forms.GroupBox
        Friend WithEvents Label43 As System.Windows.Forms.Label
        Friend WithEvents picTipoMoneda As System.Windows.Forms.PictureBox
        Friend WithEvents txtTipoMoneda As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents chckUsaListaCliente As System.Windows.Forms.CheckBox
        Friend WithEvents chkCambiaPrecio As System.Windows.Forms.CheckBox
        Friend WithEvents chkCrearOThijas As System.Windows.Forms.CheckBox
        Friend WithEvents chkAsignacionUnicaMO As System.Windows.Forms.CheckBox
        Friend WithEvents chkSolOTEsp As System.Windows.Forms.CheckBox
        Friend WithEvents rbtTiempoReal As System.Windows.Forms.RadioButton

    End Class
End Namespace
