Imports DMSOneFramework.CitasTableAdapters
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports System.Reflection
Imports System.Resources
Imports SCG.UX.Windows.SAP
Imports SCG.UX.Windows

'Imports SCG_ComponenteImagenes.SCG_Imagenes

Namespace SCG_User_Interface

    Partial Public Class frmOrden
        Inherits frmPlantillaSAP

        Dim m_ObjResources As New ResourceManager("SCG_User_Interface.ResourceUI", [Assembly].GetExecutingAssembly)

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
        Friend WithEvents grpOrdenInfo As System.Windows.Forms.GroupBox
        Friend WithEvents lblNoOrden As System.Windows.Forms.Label
        Friend WithEvents tabRepuestos As System.Windows.Forms.TabPage
        Public WithEvents lblModelo As System.Windows.Forms.Label
        Public WithEvents lblNoCono As System.Windows.Forms.Label
        Friend WithEvents imglst_SCG As System.Windows.Forms.ImageList
        Friend WithEvents tbr_SCG As System.Windows.Forms.ToolBar
        Friend WithEvents btnIniciar As System.Windows.Forms.ToolBarButton
        Friend WithEvents btnRechazar As System.Windows.Forms.ToolBarButton
        Friend WithEvents btnReproceso As System.Windows.Forms.ToolBarButton
        Friend WithEvents btnSuspension As System.Windows.Forms.ToolBarButton
        Friend WithEvents btnCalidad As System.Windows.Forms.ToolBarButton
        Friend WithEvents imglst_ProcProd As System.Windows.Forms.ImageList
        Friend WithEvents btnFinalizar As System.Windows.Forms.ToolBarButton
        Friend WithEvents tabFasesProd As System.Windows.Forms.TabPage
        Public WithEvents Label12 As System.Windows.Forms.Label
        Public WithEvents lblEstadoO As System.Windows.Forms.Label
        Friend WithEvents lblMarca As System.Windows.Forms.Label
        Public WithEvents lblTipoOrdenO As System.Windows.Forms.Label
        Friend WithEvents lblFechaSalidaFase As System.Windows.Forms.Label
        Friend WithEvents tabRendimiento As System.Windows.Forms.TabPage
        Friend WithEvents txtEstilo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtMarca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtNoOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtNoCono As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtFSalida As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
        Friend WithEvents cboEstadoRep As SCGComboBox.SCGComboBox
        Public WithEvents lblEstado As System.Windows.Forms.Label
        Friend WithEvents Label18 As System.Windows.Forms.Label
        Friend WithEvents cboEstadoRep2 As SCGComboBox.SCGComboBox
        Friend WithEvents txtPlaca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblPlaca As System.Windows.Forms.Label
        Friend WithEvents tabPrincipal As System.Windows.Forms.TabPage
        Friend WithEvents tipResponsable As System.Windows.Forms.ToolTip
        Friend WithEvents btnOrdenCompra As System.Windows.Forms.Button
        Friend WithEvents tabActividades As System.Windows.Forms.TabPage
        Friend WithEvents grpActividadesProduccion As System.Windows.Forms.GroupBox
        Friend WithEvents cboFasesProdF As SCGComboBox.SCGComboBox
        Public WithEvents Label36 As System.Windows.Forms.Label
        Public WithEvents Label47 As System.Windows.Forms.Label
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents cboEstado As SCGComboBox.SCGComboBox
        Public WithEvents Label52 As System.Windows.Forms.Label
        Friend WithEvents cboFases_Producción As SCGComboBox.SCGComboBox
        Friend WithEvents Label54 As System.Windows.Forms.Label
        Friend WithEvents tbbVisita As System.Windows.Forms.ToolBarButton
        Friend WithEvents ttbVehiculo As System.Windows.Forms.ToolBarButton
        Friend WithEvents ttbCliente As System.Windows.Forms.ToolBarButton
        Friend WithEvents imgOrdenPrincipal As System.Windows.Forms.ImageList
        Friend WithEvents tbbPrincipal As System.Windows.Forms.ToolBar
        Friend WithEvents txtEstado As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents chkAdicionalRep As System.Windows.Forms.CheckBox
        Friend WithEvents chkAdicionalAct As System.Windows.Forms.CheckBox
        Friend WithEvents btnAceptar As System.Windows.Forms.Button
        Friend WithEvents tabOrden As System.Windows.Forms.TabControl
        Friend WithEvents rptorden As ComponenteCristalReport.SubReportView
        Friend WithEvents btnRepuesto As System.Windows.Forms.Button
        Friend WithEvents btnAdicional As System.Windows.Forms.Button
        Friend WithEvents btnActAdicional As System.Windows.Forms.Button
        Friend WithEvents btnActividad As System.Windows.Forms.Button
        Friend WithEvents dtgRepuestos As System.Windows.Forms.DataGrid
        Friend WithEvents btnCambiarEstadoRepuesto As System.Windows.Forms.Button
        Friend WithEvents dtgActividades As System.Windows.Forms.DataGrid
        Friend WithEvents cbocolaborador As SCGComboBox.SCGComboBox
        Friend WithEvents btnAsignar As System.Windows.Forms.Button
        Friend WithEvents dtgcolaborador As System.Windows.Forms.DataGrid
        Friend WithEvents btnSuspende As System.Windows.Forms.Button
        Friend WithEvents btnFinaliza As System.Windows.Forms.Button
        Friend WithEvents btnInicioFecha As System.Windows.Forms.Button
        Friend WithEvents btnCambiarEstadoActividad As System.Windows.Forms.Button
        Friend WithEvents cboEstadoOrden As SCGComboBox.SCGComboBox
        Friend WithEvents txtfechacierre As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtfechaapertura As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtresponsable As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label14 As System.Windows.Forms.Label
        Friend WithEvents Label133 As System.Windows.Forms.Label
        Friend WithEvents Label135 As System.Windows.Forms.Label
        Friend WithEvents lblCompromiso As System.Windows.Forms.Label
        Public WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
        Public WithEvents lblLine12 As System.Windows.Forms.Label
        Public WithEvents lblLine15 As System.Windows.Forms.Label
        Public WithEvents lblLine10 As System.Windows.Forms.Label
        Public WithEvents lblLine11 As System.Windows.Forms.Label
        Public WithEvents lblLine14 As System.Windows.Forms.Label
        Friend WithEvents lblLine4 As System.Windows.Forms.Label
        Friend WithEvents lblLine5 As System.Windows.Forms.Label
        Public WithEvents lblLine7 As System.Windows.Forms.Label
        Public WithEvents lblLine8 As System.Windows.Forms.Label
        Public WithEvents lblLine2 As System.Windows.Forms.Label
        Public WithEvents lblLine3 As System.Windows.Forms.Label
        Public WithEvents lblLine16 As System.Windows.Forms.Label
        Public WithEvents lblLine17 As System.Windows.Forms.Label
        Public WithEvents line20 As System.Windows.Forms.Label
        Public WithEvents line19 As System.Windows.Forms.Label
        Public WithEvents lblLine21 As System.Windows.Forms.Label
        Public WithEvents lblLine23 As System.Windows.Forms.Label
        Public WithEvents lblLine22 As System.Windows.Forms.Label
        Friend WithEvents btnImprimirListaCalidad As System.Windows.Forms.Button
        Friend WithEvents rptCalidad As ComponenteCristalReport.SubReportView
        Friend WithEvents tipSuministros As System.Windows.Forms.ToolTip
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents txtObservacionesOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtTipoOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents btnDocumentos As System.Windows.Forms.ToolBarButton
        Friend WithEvents mnuDocumentos As System.Windows.Forms.ContextMenu
        Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
        Friend WithEvents dropmnuProduccion As System.Windows.Forms.MenuItem
        Friend WithEvents dropmnuOficina As System.Windows.Forms.MenuItem
        Friend WithEvents dropmnuReprocesos As System.Windows.Forms.MenuItem
        Friend WithEvents dropmnuSuspenciones As System.Windows.Forms.MenuItem
        Friend WithEvents rptReprocesos As ComponenteCristalReport.SubReportView
        Friend WithEvents rptSuspensiones As ComponenteCristalReport.SubReportView
        Friend WithEvents chkReproceso As System.Windows.Forms.CheckBox
        Friend WithEvents btnAgregarAct As System.Windows.Forms.Button
        Friend WithEvents btnAgregarRep As System.Windows.Forms.Button
        Friend WithEvents chkRefSuperiores As System.Windows.Forms.CheckBox
        Friend WithEvents btnEliminarAct As System.Windows.Forms.Button
        Friend WithEvents btnEliminarRep As System.Windows.Forms.Button
        Friend WithEvents btnCheckAll As System.Windows.Forms.Button
        Friend WithEvents btnEliminarColaborador As System.Windows.Forms.Button
        Friend WithEvents dtgMontoReparacion As System.Windows.Forms.DataGrid
        Friend WithEvents dtgRendimientosBarras As System.Windows.Forms.DataGrid
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents btnActualizar As System.Windows.Forms.Button
        Friend WithEvents dtpFechaCompromiso As System.Windows.Forms.DateTimePicker
        Friend WithEvents btnFechaComp As System.Windows.Forms.Button
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents mnuFases As System.Windows.Forms.ContextMenu
        Friend WithEvents tabSuministros As System.Windows.Forms.TabPage
        Friend WithEvents tabServiciosExternos As System.Windows.Forms.TabPage
        Friend WithEvents txtNoVehiculo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents txtNoVisita As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Friend WithEvents lblNoVisita As System.Windows.Forms.Label
        Friend WithEvents grbSuministros As System.Windows.Forms.GroupBox
        Friend WithEvents btnRequisiciones As System.Windows.Forms.Button
        Friend WithEvents btnDevoluciones As System.Windows.Forms.Button
        Friend WithEvents btnSuministros As System.Windows.Forms.Button
        Friend WithEvents btnMenuFases As System.Windows.Forms.Button
        Friend WithEvents grbServiciosExternos As System.Windows.Forms.GroupBox
        Friend WithEvents dtgSuministros As System.Windows.Forms.DataGrid
        Friend WithEvents btnEliminarSE As System.Windows.Forms.Button
        Friend WithEvents btnAgregarSE As System.Windows.Forms.Button
        Friend WithEvents dtgSE As System.Windows.Forms.DataGrid
        Friend WithEvents chkAdicionalesSE As System.Windows.Forms.CheckBox
        Public WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents cbEstadoSE As SCGComboBox.SCGComboBox
        Public WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents lbllinea As System.Windows.Forms.Label
        Friend WithEvents cboActividadesAsignables As SCGComboBox.SCGComboBox
        Friend WithEvents btnEliminaSum As System.Windows.Forms.Button
        Friend WithEvents btnAgregaSum As System.Windows.Forms.Button
        Friend WithEvents chkAdicionalesSu As System.Windows.Forms.CheckBox
        Friend WithEvents btnOrdenCompraSE As System.Windows.Forms.Button
        Friend WithEvents TTColaboras As System.Windows.Forms.ToolTip
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents cboRampas As SCGComboBox.SCGComboBox
        Public WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents btnAsignarRampa As System.Windows.Forms.Button
        Friend WithEvents btnQuitarRampa As System.Windows.Forms.Button
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents dtpRampaFecha As System.Windows.Forms.DateTimePicker
        Public WithEvents Label20 As System.Windows.Forms.Label
        Friend WithEvents Label21 As System.Windows.Forms.Label
        Public WithEvents Label17 As System.Windows.Forms.Label
        Friend WithEvents Label19 As System.Windows.Forms.Label
        Public WithEvents Label15 As System.Windows.Forms.Label
        Friend WithEvents Label16 As System.Windows.Forms.Label
        Friend WithEvents txtRampaDuracion As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents dtpRampaHora As System.Windows.Forms.DateTimePicker
        Friend WithEvents dtgRampas As System.Windows.Forms.DataGrid
        Friend WithEvents txtHoraComp As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtHoraApert As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtFechaComp As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents btnOcupacion As System.Windows.Forms.Button
        Public WithEvents Label13 As System.Windows.Forms.Label
        Public WithEvents Label11 As System.Windows.Forms.Label
        Friend WithEvents btnAsignacionMultiple As System.Windows.Forms.ToolBarButton
        Friend WithEvents btnSolicitudes As System.Windows.Forms.Button
        Friend WithEvents ttbOrdenesEspeciales As System.Windows.Forms.ToolBarButton
        Friend WithEvents ttbSeparador As System.Windows.Forms.ToolBarButton
        Friend WithEvents btnSolicitar As System.Windows.Forms.Button
        Friend WithEvents btnAsignarARepuesto As System.Windows.Forms.Button
        Friend WithEvents btnAsignarTiempos As System.Windows.Forms.ToolBarButton
        Friend WithEvents lblUnidadTiempo As System.Windows.Forms.Label
        Friend WithEvents TTDuracionEN As System.Windows.Forms.ToolTip
        Friend WithEvents dropmnuCostos As System.Windows.Forms.MenuItem
        Friend WithEvents dropmnuItemsNoAprobados As System.Windows.Forms.MenuItem
        Friend WithEvents tbbArchivos As System.Windows.Forms.ToolBarButton
        Friend WithEvents tabOtrosGastos As System.Windows.Forms.TabPage
        Friend WithEvents m_dstOtrosGastos As DMSOneFramework.OtrosGastosDataSet
        Friend WithEvents m_bsrcOtrosGastosResumido As System.Windows.Forms.BindingSource
        Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents Panel3 As System.Windows.Forms.Panel
        Friend WithEvents txtTotalOtrosGastos As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label22 As System.Windows.Forms.Label
        Friend WithEvents Label23 As System.Windows.Forms.Label
        Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
        Friend WithEvents lblOtrosGastos As System.Windows.Forms.Label
        Friend WithEvents btnActualizarOtrosGastos As System.Windows.Forms.Button
        Friend WithEvents VisualizarUDFOrden As ControlUDF.VisualizarUDF
        Friend WithEvents btnCerrarFormulario As System.Windows.Forms.Button
        Friend WithEvents txtTecnico As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents LabelTecnico As System.Windows.Forms.Label
        Friend WithEvents picTecnico As System.Windows.Forms.PictureBox
        Public WithEvents Label24 As System.Windows.Forms.Label
        Friend WithEvents txtFEntregado As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtFFacturado As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtFCerrado As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblFEntregado As System.Windows.Forms.Label
        Friend WithEvents lblFFacturado As System.Windows.Forms.Label
        Friend WithEvents lblFCerrado As System.Windows.Forms.Label
        Friend WithEvents cboEstadoWeb As SCGComboBox.SCGComboBox
        Public WithEvents Label25 As System.Windows.Forms.Label
        Public WithEvents lblEstadoWeb As System.Windows.Forms.Label
        Public WithEvents Label27 As System.Windows.Forms.Label
        Friend WithEvents txtKilometraje As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label26 As System.Windows.Forms.Label
        Friend WithEvents lblKilometraje As System.Windows.Forms.Label
        Friend WithEvents txtVIN As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label28 As System.Windows.Forms.Label
        Friend WithEvents lblVIN As System.Windows.Forms.Label
        Friend WithEvents txtfechafinalizacion As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents dropmnuBalanceOT As System.Windows.Forms.MenuItem
        Friend WithEvents picEstado As System.Windows.Forms.PictureBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrden))
            Me.tabOrden = New System.Windows.Forms.TabControl()
            Me.tabPrincipal = New System.Windows.Forms.TabPage()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.txtRampaDuracion = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.dtgRampas = New System.Windows.Forms.DataGrid()
            Me.dtpRampaHora = New System.Windows.Forms.DateTimePicker()
            Me.Panel2 = New System.Windows.Forms.Panel()
            Me.dtpRampaFecha = New System.Windows.Forms.DateTimePicker()
            Me.Label20 = New System.Windows.Forms.Label()
            Me.Label21 = New System.Windows.Forms.Label()
            Me.Label17 = New System.Windows.Forms.Label()
            Me.Label19 = New System.Windows.Forms.Label()
            Me.Label15 = New System.Windows.Forms.Label()
            Me.Label16 = New System.Windows.Forms.Label()
            Me.btnAsignarRampa = New System.Windows.Forms.Button()
            Me.btnQuitarRampa = New System.Windows.Forms.Button()
            Me.cboRampas = New SCGComboBox.SCGComboBox()
            Me.Label9 = New System.Windows.Forms.Label()
            Me.Label10 = New System.Windows.Forms.Label()
            Me.btnOcupacion = New System.Windows.Forms.Button()
            Me.grpOrdenInfo = New System.Windows.Forms.GroupBox()
            Me.txtfechafinalizacion = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtFEntregado = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtFFacturado = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtFCerrado = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblFEntregado = New System.Windows.Forms.Label()
            Me.lblFFacturado = New System.Windows.Forms.Label()
            Me.lblFCerrado = New System.Windows.Forms.Label()
            Me.Label24 = New System.Windows.Forms.Label()
            Me.picTecnico = New System.Windows.Forms.PictureBox()
            Me.txtTecnico = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.LabelTecnico = New System.Windows.Forms.Label()
            Me.txtFechaComp = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtresponsable = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label13 = New System.Windows.Forms.Label()
            Me.Label11 = New System.Windows.Forms.Label()
            Me.txtHoraComp = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtHoraApert = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.cboEstadoOrden = New SCGComboBox.SCGComboBox()
            Me.txtfechacierre = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtfechaapertura = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblLine12 = New System.Windows.Forms.Label()
            Me.lblLine15 = New System.Windows.Forms.Label()
            Me.lblLine10 = New System.Windows.Forms.Label()
            Me.lblLine11 = New System.Windows.Forms.Label()
            Me.lblLine14 = New System.Windows.Forms.Label()
            Me.Label14 = New System.Windows.Forms.Label()
            Me.Label133 = New System.Windows.Forms.Label()
            Me.Label135 = New System.Windows.Forms.Label()
            Me.lblCompromiso = New System.Windows.Forms.Label()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.txtObservacionesOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.tbbPrincipal = New System.Windows.Forms.ToolBar()
            Me.ttbOrdenesEspeciales = New System.Windows.Forms.ToolBarButton()
            Me.ttbSeparador = New System.Windows.Forms.ToolBarButton()
            Me.ttbVehiculo = New System.Windows.Forms.ToolBarButton()
            Me.ttbCliente = New System.Windows.Forms.ToolBarButton()
            Me.tbbVisita = New System.Windows.Forms.ToolBarButton()
            Me.tbbArchivos = New System.Windows.Forms.ToolBarButton()
            Me.imgOrdenPrincipal = New System.Windows.Forms.ImageList(Me.components)
            Me.tabRepuestos = New System.Windows.Forms.TabPage()
            Me.btnAsignarARepuesto = New System.Windows.Forms.Button()
            Me.btnSolicitar = New System.Windows.Forms.Button()
            Me.btnSolicitudes = New System.Windows.Forms.Button()
            Me.lblLine16 = New System.Windows.Forms.Label()
            Me.cboEstadoRep2 = New SCGComboBox.SCGComboBox()
            Me.GroupBox5 = New System.Windows.Forms.GroupBox()
            Me.Panel1 = New System.Windows.Forms.Panel()
            Me.btnFechaComp = New System.Windows.Forms.Button()
            Me.dtpFechaCompromiso = New System.Windows.Forms.DateTimePicker()
            Me.btnCheckAll = New System.Windows.Forms.Button()
            Me.btnEliminarRep = New System.Windows.Forms.Button()
            Me.btnAgregarRep = New System.Windows.Forms.Button()
            Me.dtgRepuestos = New System.Windows.Forms.DataGrid()
            Me.lblLine17 = New System.Windows.Forms.Label()
            Me.chkAdicionalRep = New System.Windows.Forms.CheckBox()
            Me.cboEstadoRep = New SCGComboBox.SCGComboBox()
            Me.btnCambiarEstadoRepuesto = New System.Windows.Forms.Button()
            Me.Label18 = New System.Windows.Forms.Label()
            Me.btnOrdenCompra = New System.Windows.Forms.Button()
            Me.lblEstado = New System.Windows.Forms.Label()
            Me.btnAdicional = New System.Windows.Forms.Button()
            Me.btnRepuesto = New System.Windows.Forms.Button()
            Me.tabFasesProd = New System.Windows.Forms.TabPage()
            Me.lblUnidadTiempo = New System.Windows.Forms.Label()
            Me.cboFases_Producción = New SCGComboBox.SCGComboBox()
            Me.txtFSalida = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblLine21 = New System.Windows.Forms.Label()
            Me.GroupBox2 = New System.Windows.Forms.GroupBox()
            Me.cboActividadesAsignables = New SCGComboBox.SCGComboBox()
            Me.lbllinea = New System.Windows.Forms.Label()
            Me.btnMenuFases = New System.Windows.Forms.Button()
            Me.btnEliminarColaborador = New System.Windows.Forms.Button()
            Me.chkRefSuperiores = New System.Windows.Forms.CheckBox()
            Me.chkReproceso = New System.Windows.Forms.CheckBox()
            Me.cbocolaborador = New SCGComboBox.SCGComboBox()
            Me.dtgcolaborador = New System.Windows.Forms.DataGrid()
            Me.btnSuspende = New System.Windows.Forms.Button()
            Me.lblLine23 = New System.Windows.Forms.Label()
            Me.Label54 = New System.Windows.Forms.Label()
            Me.btnAsignar = New System.Windows.Forms.Button()
            Me.btnFinaliza = New System.Windows.Forms.Button()
            Me.btnInicioFecha = New System.Windows.Forms.Button()
            Me.lblLine22 = New System.Windows.Forms.Label()
            Me.lblFechaSalidaFase = New System.Windows.Forms.Label()
            Me.Label47 = New System.Windows.Forms.Label()
            Me.tbr_SCG = New System.Windows.Forms.ToolBar()
            Me.btnAsignacionMultiple = New System.Windows.Forms.ToolBarButton()
            Me.btnAsignarTiempos = New System.Windows.Forms.ToolBarButton()
            Me.btnIniciar = New System.Windows.Forms.ToolBarButton()
            Me.btnRechazar = New System.Windows.Forms.ToolBarButton()
            Me.btnReproceso = New System.Windows.Forms.ToolBarButton()
            Me.btnSuspension = New System.Windows.Forms.ToolBarButton()
            Me.btnCalidad = New System.Windows.Forms.ToolBarButton()
            Me.btnDocumentos = New System.Windows.Forms.ToolBarButton()
            Me.mnuDocumentos = New System.Windows.Forms.ContextMenu()
            Me.dropmnuProduccion = New System.Windows.Forms.MenuItem()
            Me.dropmnuCostos = New System.Windows.Forms.MenuItem()
            Me.dropmnuItemsNoAprobados = New System.Windows.Forms.MenuItem()
            Me.dropmnuOficina = New System.Windows.Forms.MenuItem()
            Me.dropmnuBalanceOT = New System.Windows.Forms.MenuItem()
            Me.MenuItem3 = New System.Windows.Forms.MenuItem()
            Me.dropmnuReprocesos = New System.Windows.Forms.MenuItem()
            Me.dropmnuSuspenciones = New System.Windows.Forms.MenuItem()
            Me.btnFinalizar = New System.Windows.Forms.ToolBarButton()
            Me.imglst_ProcProd = New System.Windows.Forms.ImageList(Me.components)
            Me.btnImprimirListaCalidad = New System.Windows.Forms.Button()
            Me.picEstado = New System.Windows.Forms.PictureBox()
            Me.tabActividades = New System.Windows.Forms.TabPage()
            Me.btnActAdicional = New System.Windows.Forms.Button()
            Me.btnActividad = New System.Windows.Forms.Button()
            Me.cboFasesProdF = New SCGComboBox.SCGComboBox()
            Me.grpActividadesProduccion = New System.Windows.Forms.GroupBox()
            Me.btnEliminarAct = New System.Windows.Forms.Button()
            Me.btnAgregarAct = New System.Windows.Forms.Button()
            Me.dtgActividades = New System.Windows.Forms.DataGrid()
            Me.btnCambiarEstadoActividad = New System.Windows.Forms.Button()
            Me.line20 = New System.Windows.Forms.Label()
            Me.cboEstado = New SCGComboBox.SCGComboBox()
            Me.Label52 = New System.Windows.Forms.Label()
            Me.chkAdicionalAct = New System.Windows.Forms.CheckBox()
            Me.line19 = New System.Windows.Forms.Label()
            Me.Label36 = New System.Windows.Forms.Label()
            Me.tabSuministros = New System.Windows.Forms.TabPage()
            Me.grbSuministros = New System.Windows.Forms.GroupBox()
            Me.chkAdicionalesSu = New System.Windows.Forms.CheckBox()
            Me.btnEliminaSum = New System.Windows.Forms.Button()
            Me.btnAgregaSum = New System.Windows.Forms.Button()
            Me.dtgSuministros = New System.Windows.Forms.DataGrid()
            Me.btnRequisiciones = New System.Windows.Forms.Button()
            Me.btnDevoluciones = New System.Windows.Forms.Button()
            Me.btnSuministros = New System.Windows.Forms.Button()
            Me.tabServiciosExternos = New System.Windows.Forms.TabPage()
            Me.grbServiciosExternos = New System.Windows.Forms.GroupBox()
            Me.btnOrdenCompraSE = New System.Windows.Forms.Button()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.cbEstadoSE = New SCGComboBox.SCGComboBox()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.btnEliminarSE = New System.Windows.Forms.Button()
            Me.btnAgregarSE = New System.Windows.Forms.Button()
            Me.dtgSE = New System.Windows.Forms.DataGrid()
            Me.chkAdicionalesSE = New System.Windows.Forms.CheckBox()
            Me.tabRendimiento = New System.Windows.Forms.TabPage()
            Me.btnActualizar = New System.Windows.Forms.Button()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.dtgRendimientosBarras = New System.Windows.Forms.DataGrid()
            Me.dtgMontoReparacion = New System.Windows.Forms.DataGrid()
            Me.tabOtrosGastos = New System.Windows.Forms.TabPage()
            Me.GroupBox4 = New System.Windows.Forms.GroupBox()
            Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
            Me.lblOtrosGastos = New System.Windows.Forms.Label()
            Me.Panel3 = New System.Windows.Forms.Panel()
            Me.txtTotalOtrosGastos = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.btnActualizarOtrosGastos = New System.Windows.Forms.Button()
            Me.Label22 = New System.Windows.Forms.Label()
            Me.Label23 = New System.Windows.Forms.Label()
            Me.txtPlaca = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtEstilo = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtMarca = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtTipoOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtNoOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtNoCono = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtEstado = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblLine4 = New System.Windows.Forms.Label()
            Me.lblLine5 = New System.Windows.Forms.Label()
            Me.Label12 = New System.Windows.Forms.Label()
            Me.lblLine7 = New System.Windows.Forms.Label()
            Me.lblLine8 = New System.Windows.Forms.Label()
            Me.lblLine2 = New System.Windows.Forms.Label()
            Me.lblLine3 = New System.Windows.Forms.Label()
            Me.lblPlaca = New System.Windows.Forms.Label()
            Me.lblMarca = New System.Windows.Forms.Label()
            Me.lblModelo = New System.Windows.Forms.Label()
            Me.lblNoOrden = New System.Windows.Forms.Label()
            Me.lblNoCono = New System.Windows.Forms.Label()
            Me.lblTipoOrdenO = New System.Windows.Forms.Label()
            Me.lblEstadoO = New System.Windows.Forms.Label()
            Me.rptorden = New ComponenteCristalReport.SubReportView()
            Me.imglst_SCG = New System.Windows.Forms.ImageList(Me.components)
            Me.tipResponsable = New System.Windows.Forms.ToolTip(Me.components)
            Me.GroupBox3 = New System.Windows.Forms.GroupBox()
            Me.Label27 = New System.Windows.Forms.Label()
            Me.txtKilometraje = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label26 = New System.Windows.Forms.Label()
            Me.lblKilometraje = New System.Windows.Forms.Label()
            Me.txtVIN = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label28 = New System.Windows.Forms.Label()
            Me.lblVIN = New System.Windows.Forms.Label()
            Me.cboEstadoWeb = New SCGComboBox.SCGComboBox()
            Me.Label25 = New System.Windows.Forms.Label()
            Me.txtNoVehiculo = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblEstadoWeb = New System.Windows.Forms.Label()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.txtNoVisita = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblLine1 = New System.Windows.Forms.Label()
            Me.lblNoVisita = New System.Windows.Forms.Label()
            Me.rptCalidad = New ComponenteCristalReport.SubReportView()
            Me.rptReprocesos = New ComponenteCristalReport.SubReportView()
            Me.rptSuspensiones = New ComponenteCristalReport.SubReportView()
            Me.btnAceptar = New System.Windows.Forms.Button()
            Me.VisualizarUDFOrden = New ControlUDF.VisualizarUDF()
            Me.btnCerrarFormulario = New System.Windows.Forms.Button()
            Me.tipSuministros = New System.Windows.Forms.ToolTip(Me.components)
            Me.mnuFases = New System.Windows.Forms.ContextMenu()
            Me.TTColaboras = New System.Windows.Forms.ToolTip(Me.components)
            Me.TTDuracionEN = New System.Windows.Forms.ToolTip(Me.components)
            Me.m_bsrcOtrosGastosResumido = New System.Windows.Forms.BindingSource(Me.components)
            Me.m_dstOtrosGastos = New DMSOneFramework.OtrosGastosDataSet()
            Me.tabOrden.SuspendLayout()
            Me.tabPrincipal.SuspendLayout()
            Me.GroupBox1.SuspendLayout()
            CType(Me.dtgRampas, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpOrdenInfo.SuspendLayout()
            CType(Me.picTecnico, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabRepuestos.SuspendLayout()
            Me.GroupBox5.SuspendLayout()
            CType(Me.dtgRepuestos, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabFasesProd.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            CType(Me.dtgcolaborador, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picEstado, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabActividades.SuspendLayout()
            Me.grpActividadesProduccion.SuspendLayout()
            CType(Me.dtgActividades, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabSuministros.SuspendLayout()
            Me.grbSuministros.SuspendLayout()
            CType(Me.dtgSuministros, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabServiciosExternos.SuspendLayout()
            Me.grbServiciosExternos.SuspendLayout()
            CType(Me.dtgSE, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabRendimiento.SuspendLayout()
            CType(Me.dtgRendimientosBarras, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtgMontoReparacion, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabOtrosGastos.SuspendLayout()
            Me.GroupBox4.SuspendLayout()
            Me.FlowLayoutPanel1.SuspendLayout()
            Me.Panel3.SuspendLayout()
            Me.GroupBox3.SuspendLayout()
            CType(Me.m_bsrcOtrosGastosResumido, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.m_dstOtrosGastos, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'tabOrden
            '
            resources.ApplyResources(Me.tabOrden, "tabOrden")
            Me.tabOrden.Controls.Add(Me.tabPrincipal)
            Me.tabOrden.Controls.Add(Me.tabRepuestos)
            Me.tabOrden.Controls.Add(Me.tabFasesProd)
            Me.tabOrden.Controls.Add(Me.tabActividades)
            Me.tabOrden.Controls.Add(Me.tabSuministros)
            Me.tabOrden.Controls.Add(Me.tabServiciosExternos)
            Me.tabOrden.Controls.Add(Me.tabRendimiento)
            Me.tabOrden.Controls.Add(Me.tabOtrosGastos)
            Me.tabOrden.Name = "tabOrden"
            Me.tabOrden.SelectedIndex = 0
            '
            'tabPrincipal
            '
            Me.tabPrincipal.Controls.Add(Me.GroupBox1)
            Me.tabPrincipal.Controls.Add(Me.grpOrdenInfo)
            Me.tabPrincipal.Controls.Add(Me.tbbPrincipal)
            resources.ApplyResources(Me.tabPrincipal, "tabPrincipal")
            Me.tabPrincipal.Name = "tabPrincipal"
            '
            'GroupBox1
            '
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            Me.GroupBox1.Controls.Add(Me.txtRampaDuracion)
            Me.GroupBox1.Controls.Add(Me.dtgRampas)
            Me.GroupBox1.Controls.Add(Me.dtpRampaHora)
            Me.GroupBox1.Controls.Add(Me.Panel2)
            Me.GroupBox1.Controls.Add(Me.dtpRampaFecha)
            Me.GroupBox1.Controls.Add(Me.Label20)
            Me.GroupBox1.Controls.Add(Me.Label21)
            Me.GroupBox1.Controls.Add(Me.Label17)
            Me.GroupBox1.Controls.Add(Me.Label19)
            Me.GroupBox1.Controls.Add(Me.Label15)
            Me.GroupBox1.Controls.Add(Me.Label16)
            Me.GroupBox1.Controls.Add(Me.btnAsignarRampa)
            Me.GroupBox1.Controls.Add(Me.btnQuitarRampa)
            Me.GroupBox1.Controls.Add(Me.cboRampas)
            Me.GroupBox1.Controls.Add(Me.Label9)
            Me.GroupBox1.Controls.Add(Me.Label10)
            Me.GroupBox1.Controls.Add(Me.btnOcupacion)
            Me.GroupBox1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.TabStop = False
            '
            'txtRampaDuracion
            '
            Me.txtRampaDuracion.AceptaNegativos = False
            Me.txtRampaDuracion.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtRampaDuracion.EstiloSBO = True
            resources.ApplyResources(Me.txtRampaDuracion, "txtRampaDuracion")
            Me.txtRampaDuracion.ForeColor = System.Drawing.Color.Black
            Me.txtRampaDuracion.MaxDecimales = 2
            Me.txtRampaDuracion.MaxEnteros = 10
            Me.txtRampaDuracion.Millares = False
            Me.txtRampaDuracion.Name = "txtRampaDuracion"
            Me.txtRampaDuracion.Size_AdjustableHeight = 21
            Me.txtRampaDuracion.TeclasDeshacer = True
            Me.txtRampaDuracion.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.NumDecimal
            '
            'dtgRampas
            '
            resources.ApplyResources(Me.dtgRampas, "dtgRampas")
            Me.dtgRampas.BackgroundColor = System.Drawing.Color.White
            Me.dtgRampas.CaptionBackColor = System.Drawing.Color.White
            Me.dtgRampas.CaptionVisible = False
            Me.dtgRampas.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgRampas.FlatMode = True
            Me.dtgRampas.HeaderBackColor = System.Drawing.Color.White
            Me.dtgRampas.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.dtgRampas.HeaderForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgRampas.Name = "dtgRampas"
            Me.dtgRampas.RowHeadersVisible = False
            '
            'dtpRampaHora
            '
            resources.ApplyResources(Me.dtpRampaHora, "dtpRampaHora")
            Me.dtpRampaHora.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpRampaHora.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpRampaHora.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpRampaHora.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpRampaHora.Cursor = System.Windows.Forms.Cursors.Default
            Me.dtpRampaHora.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpRampaHora.Name = "dtpRampaHora"
            Me.dtpRampaHora.ShowUpDown = True
            Me.dtpRampaHora.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'Panel2
            '
            resources.ApplyResources(Me.Panel2, "Panel2")
            Me.Panel2.Name = "Panel2"
            '
            'dtpRampaFecha
            '
            resources.ApplyResources(Me.dtpRampaFecha, "dtpRampaFecha")
            Me.dtpRampaFecha.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpRampaFecha.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpRampaFecha.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpRampaFecha.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpRampaFecha.Cursor = System.Windows.Forms.Cursors.Default
            Me.dtpRampaFecha.CustomFormat = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtpRampaFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpRampaFecha.Name = "dtpRampaFecha"
            Me.dtpRampaFecha.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'Label20
            '
            Me.Label20.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label20, "Label20")
            Me.Label20.Name = "Label20"
            '
            'Label21
            '
            resources.ApplyResources(Me.Label21, "Label21")
            Me.Label21.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label21.Name = "Label21"
            '
            'Label17
            '
            Me.Label17.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label17, "Label17")
            Me.Label17.Name = "Label17"
            '
            'Label19
            '
            resources.ApplyResources(Me.Label19, "Label19")
            Me.Label19.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label19.Name = "Label19"
            '
            'Label15
            '
            Me.Label15.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label15, "Label15")
            Me.Label15.Name = "Label15"
            '
            'Label16
            '
            resources.ApplyResources(Me.Label16, "Label16")
            Me.Label16.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label16.Name = "Label16"
            '
            'btnAsignarRampa
            '
            resources.ApplyResources(Me.btnAsignarRampa, "btnAsignarRampa")
            Me.btnAsignarRampa.ForeColor = System.Drawing.Color.Maroon
            Me.btnAsignarRampa.Name = "btnAsignarRampa"
            '
            'btnQuitarRampa
            '
            resources.ApplyResources(Me.btnQuitarRampa, "btnQuitarRampa")
            Me.btnQuitarRampa.ForeColor = System.Drawing.Color.Maroon
            Me.btnQuitarRampa.Name = "btnQuitarRampa"
            '
            'cboRampas
            '
            Me.cboRampas.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboRampas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboRampas.EstiloSBO = True
            resources.ApplyResources(Me.cboRampas, "cboRampas")
            Me.cboRampas.Name = "cboRampas"
            '
            'Label9
            '
            Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label9, "Label9")
            Me.Label9.Name = "Label9"
            '
            'Label10
            '
            resources.ApplyResources(Me.Label10, "Label10")
            Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label10.Name = "Label10"
            '
            'btnOcupacion
            '
            resources.ApplyResources(Me.btnOcupacion, "btnOcupacion")
            Me.btnOcupacion.BackgroundImage = Global.SCG_User_Interface.My.Resources.Resources.Boton_SCG
            Me.btnOcupacion.Image = Global.SCG_User_Interface.My.Resources.Resources.calendario2
            Me.btnOcupacion.Name = "btnOcupacion"
            Me.TTColaboras.SetToolTip(Me.btnOcupacion, resources.GetString("btnOcupacion.ToolTip"))
            Me.tipResponsable.SetToolTip(Me.btnOcupacion, resources.GetString("btnOcupacion.ToolTip1"))
            Me.tipSuministros.SetToolTip(Me.btnOcupacion, resources.GetString("btnOcupacion.ToolTip2"))
            Me.btnOcupacion.UseVisualStyleBackColor = True
            '
            'grpOrdenInfo
            '
            resources.ApplyResources(Me.grpOrdenInfo, "grpOrdenInfo")
            Me.grpOrdenInfo.Controls.Add(Me.txtfechafinalizacion)
            Me.grpOrdenInfo.Controls.Add(Me.txtFEntregado)
            Me.grpOrdenInfo.Controls.Add(Me.txtFFacturado)
            Me.grpOrdenInfo.Controls.Add(Me.txtFCerrado)
            Me.grpOrdenInfo.Controls.Add(Me.lblFEntregado)
            Me.grpOrdenInfo.Controls.Add(Me.lblFFacturado)
            Me.grpOrdenInfo.Controls.Add(Me.lblFCerrado)
            Me.grpOrdenInfo.Controls.Add(Me.Label24)
            Me.grpOrdenInfo.Controls.Add(Me.picTecnico)
            Me.grpOrdenInfo.Controls.Add(Me.txtTecnico)
            Me.grpOrdenInfo.Controls.Add(Me.LabelTecnico)
            Me.grpOrdenInfo.Controls.Add(Me.txtFechaComp)
            Me.grpOrdenInfo.Controls.Add(Me.txtresponsable)
            Me.grpOrdenInfo.Controls.Add(Me.Label13)
            Me.grpOrdenInfo.Controls.Add(Me.Label11)
            Me.grpOrdenInfo.Controls.Add(Me.txtHoraComp)
            Me.grpOrdenInfo.Controls.Add(Me.txtHoraApert)
            Me.grpOrdenInfo.Controls.Add(Me.Label1)
            Me.grpOrdenInfo.Controls.Add(Me.cboEstadoOrden)
            Me.grpOrdenInfo.Controls.Add(Me.txtfechacierre)
            Me.grpOrdenInfo.Controls.Add(Me.txtfechaapertura)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine12)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine15)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine10)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine11)
            Me.grpOrdenInfo.Controls.Add(Me.lblLine14)
            Me.grpOrdenInfo.Controls.Add(Me.Label14)
            Me.grpOrdenInfo.Controls.Add(Me.Label133)
            Me.grpOrdenInfo.Controls.Add(Me.Label135)
            Me.grpOrdenInfo.Controls.Add(Me.lblCompromiso)
            Me.grpOrdenInfo.Controls.Add(Me.Label8)
            Me.grpOrdenInfo.Controls.Add(Me.Label2)
            Me.grpOrdenInfo.Controls.Add(Me.txtObservacionesOrden)
            Me.grpOrdenInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.grpOrdenInfo.Name = "grpOrdenInfo"
            Me.grpOrdenInfo.TabStop = False
            '
            'txtfechafinalizacion
            '
            Me.txtfechafinalizacion.AceptaNegativos = False
            Me.txtfechafinalizacion.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtfechafinalizacion.EstiloSBO = True
            resources.ApplyResources(Me.txtfechafinalizacion, "txtfechafinalizacion")
            Me.txtfechafinalizacion.ForeColor = System.Drawing.Color.Black
            Me.txtfechafinalizacion.MaxDecimales = 0
            Me.txtfechafinalizacion.MaxEnteros = 0
            Me.txtfechafinalizacion.Millares = False
            Me.txtfechafinalizacion.Name = "txtfechafinalizacion"
            Me.txtfechafinalizacion.ReadOnly = True
            Me.txtfechafinalizacion.Size_AdjustableHeight = 20
            Me.txtfechafinalizacion.TeclasDeshacer = True
            Me.txtfechafinalizacion.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtFEntregado
            '
            Me.txtFEntregado.AceptaNegativos = False
            resources.ApplyResources(Me.txtFEntregado, "txtFEntregado")
            Me.txtFEntregado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtFEntregado.EstiloSBO = True
            Me.txtFEntregado.ForeColor = System.Drawing.Color.Black
            Me.txtFEntregado.MaxDecimales = 0
            Me.txtFEntregado.MaxEnteros = 0
            Me.txtFEntregado.Millares = False
            Me.txtFEntregado.Name = "txtFEntregado"
            Me.txtFEntregado.Size_AdjustableHeight = 20
            Me.txtFEntregado.TeclasDeshacer = True
            Me.txtFEntregado.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtFFacturado
            '
            Me.txtFFacturado.AceptaNegativos = False
            resources.ApplyResources(Me.txtFFacturado, "txtFFacturado")
            Me.txtFFacturado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtFFacturado.EstiloSBO = True
            Me.txtFFacturado.ForeColor = System.Drawing.Color.Black
            Me.txtFFacturado.MaxDecimales = 0
            Me.txtFFacturado.MaxEnteros = 0
            Me.txtFFacturado.Millares = False
            Me.txtFFacturado.Name = "txtFFacturado"
            Me.txtFFacturado.Size_AdjustableHeight = 20
            Me.txtFFacturado.TeclasDeshacer = True
            Me.txtFFacturado.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtFCerrado
            '
            Me.txtFCerrado.AceptaNegativos = False
            resources.ApplyResources(Me.txtFCerrado, "txtFCerrado")
            Me.txtFCerrado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtFCerrado.EstiloSBO = True
            Me.txtFCerrado.ForeColor = System.Drawing.Color.Black
            Me.txtFCerrado.MaxDecimales = 0
            Me.txtFCerrado.MaxEnteros = 0
            Me.txtFCerrado.Millares = False
            Me.txtFCerrado.Name = "txtFCerrado"
            Me.txtFCerrado.Size_AdjustableHeight = 20
            Me.txtFCerrado.TeclasDeshacer = True
            Me.txtFCerrado.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblFEntregado
            '
            resources.ApplyResources(Me.lblFEntregado, "lblFEntregado")
            Me.lblFEntregado.Name = "lblFEntregado"
            '
            'lblFFacturado
            '
            resources.ApplyResources(Me.lblFFacturado, "lblFFacturado")
            Me.lblFFacturado.Name = "lblFFacturado"
            '
            'lblFCerrado
            '
            resources.ApplyResources(Me.lblFCerrado, "lblFCerrado")
            Me.lblFCerrado.Name = "lblFCerrado"
            '
            'Label24
            '
            resources.ApplyResources(Me.Label24, "Label24")
            Me.Label24.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            Me.Label24.Name = "Label24"
            '
            'picTecnico
            '
            Me.picTecnico.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picTecnico, "picTecnico")
            Me.picTecnico.Name = "picTecnico"
            Me.picTecnico.TabStop = False
            '
            'txtTecnico
            '
            Me.txtTecnico.AceptaNegativos = False
            resources.ApplyResources(Me.txtTecnico, "txtTecnico")
            Me.txtTecnico.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtTecnico.EstiloSBO = True
            Me.txtTecnico.ForeColor = System.Drawing.Color.Black
            Me.txtTecnico.MaxDecimales = 0
            Me.txtTecnico.MaxEnteros = 0
            Me.txtTecnico.Millares = False
            Me.txtTecnico.Name = "txtTecnico"
            Me.txtTecnico.Size_AdjustableHeight = 20
            Me.txtTecnico.TeclasDeshacer = True
            Me.txtTecnico.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'LabelTecnico
            '
            resources.ApplyResources(Me.LabelTecnico, "LabelTecnico")
            Me.LabelTecnico.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.LabelTecnico.Name = "LabelTecnico"
            '
            'txtFechaComp
            '
            Me.txtFechaComp.AceptaNegativos = False
            Me.txtFechaComp.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtFechaComp.EstiloSBO = True
            resources.ApplyResources(Me.txtFechaComp, "txtFechaComp")
            Me.txtFechaComp.ForeColor = System.Drawing.Color.Black
            Me.txtFechaComp.MaxDecimales = 0
            Me.txtFechaComp.MaxEnteros = 0
            Me.txtFechaComp.Millares = False
            Me.txtFechaComp.Name = "txtFechaComp"
            Me.txtFechaComp.ReadOnly = True
            Me.txtFechaComp.Size_AdjustableHeight = 20
            Me.txtFechaComp.TeclasDeshacer = True
            Me.txtFechaComp.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtresponsable
            '
            Me.txtresponsable.AceptaNegativos = False
            resources.ApplyResources(Me.txtresponsable, "txtresponsable")
            Me.txtresponsable.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtresponsable.EstiloSBO = True
            Me.txtresponsable.ForeColor = System.Drawing.Color.Black
            Me.txtresponsable.MaxDecimales = 0
            Me.txtresponsable.MaxEnteros = 0
            Me.txtresponsable.Millares = False
            Me.txtresponsable.Name = "txtresponsable"
            Me.txtresponsable.Size_AdjustableHeight = 20
            Me.txtresponsable.TeclasDeshacer = True
            Me.txtresponsable.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label13
            '
            resources.ApplyResources(Me.Label13, "Label13")
            Me.Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            Me.Label13.Name = "Label13"
            '
            'Label11
            '
            Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label11, "Label11")
            Me.Label11.Name = "Label11"
            '
            'txtHoraComp
            '
            Me.txtHoraComp.AceptaNegativos = False
            Me.txtHoraComp.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtHoraComp.EstiloSBO = True
            resources.ApplyResources(Me.txtHoraComp, "txtHoraComp")
            Me.txtHoraComp.ForeColor = System.Drawing.Color.Black
            Me.txtHoraComp.MaxDecimales = 0
            Me.txtHoraComp.MaxEnteros = 0
            Me.txtHoraComp.Millares = False
            Me.txtHoraComp.Name = "txtHoraComp"
            Me.txtHoraComp.ReadOnly = True
            Me.txtHoraComp.Size_AdjustableHeight = 20
            Me.txtHoraComp.TeclasDeshacer = True
            Me.txtHoraComp.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtHoraApert
            '
            Me.txtHoraApert.AceptaNegativos = False
            Me.txtHoraApert.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtHoraApert.EstiloSBO = True
            resources.ApplyResources(Me.txtHoraApert, "txtHoraApert")
            Me.txtHoraApert.ForeColor = System.Drawing.Color.Black
            Me.txtHoraApert.MaxDecimales = 0
            Me.txtHoraApert.MaxEnteros = 0
            Me.txtHoraApert.Millares = False
            Me.txtHoraApert.Name = "txtHoraApert"
            Me.txtHoraApert.ReadOnly = True
            Me.txtHoraApert.Size_AdjustableHeight = 20
            Me.txtHoraApert.TeclasDeshacer = True
            Me.txtHoraApert.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'cboEstadoOrden
            '
            resources.ApplyResources(Me.cboEstadoOrden, "cboEstadoOrden")
            Me.cboEstadoOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboEstadoOrden.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstadoOrden.EstiloSBO = True
            Me.cboEstadoOrden.Name = "cboEstadoOrden"
            '
            'txtfechacierre
            '
            Me.txtfechacierre.AceptaNegativos = False
            Me.txtfechacierre.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtfechacierre.EstiloSBO = True
            resources.ApplyResources(Me.txtfechacierre, "txtfechacierre")
            Me.txtfechacierre.ForeColor = System.Drawing.Color.Black
            Me.txtfechacierre.MaxDecimales = 0
            Me.txtfechacierre.MaxEnteros = 0
            Me.txtfechacierre.Millares = False
            Me.txtfechacierre.Name = "txtfechacierre"
            Me.txtfechacierre.ReadOnly = True
            Me.txtfechacierre.Size_AdjustableHeight = 20
            Me.txtfechacierre.TeclasDeshacer = True
            Me.txtfechacierre.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtfechaapertura
            '
            Me.txtfechaapertura.AceptaNegativos = False
            Me.txtfechaapertura.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtfechaapertura.EstiloSBO = True
            resources.ApplyResources(Me.txtfechaapertura, "txtfechaapertura")
            Me.txtfechaapertura.ForeColor = System.Drawing.Color.Black
            Me.txtfechaapertura.MaxDecimales = 0
            Me.txtfechaapertura.MaxEnteros = 0
            Me.txtfechaapertura.Millares = False
            Me.txtfechaapertura.Name = "txtfechaapertura"
            Me.txtfechaapertura.ReadOnly = True
            Me.txtfechaapertura.Size_AdjustableHeight = 20
            Me.txtfechaapertura.TeclasDeshacer = True
            Me.txtfechaapertura.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblLine12
            '
            Me.lblLine12.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine12, "lblLine12")
            Me.lblLine12.Name = "lblLine12"
            '
            'lblLine15
            '
            resources.ApplyResources(Me.lblLine15, "lblLine15")
            Me.lblLine15.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            Me.lblLine15.Name = "lblLine15"
            '
            'lblLine10
            '
            Me.lblLine10.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine10, "lblLine10")
            Me.lblLine10.Name = "lblLine10"
            '
            'lblLine11
            '
            Me.lblLine11.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine11, "lblLine11")
            Me.lblLine11.Name = "lblLine11"
            '
            'lblLine14
            '
            resources.ApplyResources(Me.lblLine14, "lblLine14")
            Me.lblLine14.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            Me.lblLine14.Name = "lblLine14"
            '
            'Label14
            '
            resources.ApplyResources(Me.Label14, "Label14")
            Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label14.Name = "Label14"
            '
            'Label133
            '
            resources.ApplyResources(Me.Label133, "Label133")
            Me.Label133.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label133.Name = "Label133"
            '
            'Label135
            '
            resources.ApplyResources(Me.Label135, "Label135")
            Me.Label135.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label135.Name = "Label135"
            '
            'lblCompromiso
            '
            resources.ApplyResources(Me.lblCompromiso, "lblCompromiso")
            Me.lblCompromiso.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCompromiso.Name = "lblCompromiso"
            '
            'Label8
            '
            resources.ApplyResources(Me.Label8, "Label8")
            Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label8.Name = "Label8"
            '
            'Label2
            '
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label2.Name = "Label2"
            '
            'txtObservacionesOrden
            '
            Me.txtObservacionesOrden.AceptaNegativos = False
            resources.ApplyResources(Me.txtObservacionesOrden, "txtObservacionesOrden")
            Me.txtObservacionesOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtObservacionesOrden.EstiloSBO = True
            Me.txtObservacionesOrden.ForeColor = System.Drawing.Color.Black
            Me.txtObservacionesOrden.MaxDecimales = 0
            Me.txtObservacionesOrden.MaxEnteros = 0
            Me.txtObservacionesOrden.Millares = False
            Me.txtObservacionesOrden.Name = "txtObservacionesOrden"
            Me.txtObservacionesOrden.Size_AdjustableHeight = 94
            Me.txtObservacionesOrden.TeclasDeshacer = True
            Me.txtObservacionesOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'tbbPrincipal
            '
            resources.ApplyResources(Me.tbbPrincipal, "tbbPrincipal")
            Me.tbbPrincipal.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.ttbOrdenesEspeciales, Me.ttbSeparador, Me.ttbVehiculo, Me.ttbCliente, Me.tbbVisita, Me.tbbArchivos})
            Me.tbbPrincipal.ImageList = Me.imgOrdenPrincipal
            Me.tbbPrincipal.Name = "tbbPrincipal"
            '
            'ttbOrdenesEspeciales
            '
            resources.ApplyResources(Me.ttbOrdenesEspeciales, "ttbOrdenesEspeciales")
            Me.ttbOrdenesEspeciales.Name = "ttbOrdenesEspeciales"
            '
            'ttbSeparador
            '
            Me.ttbSeparador.Name = "ttbSeparador"
            Me.ttbSeparador.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
            '
            'ttbVehiculo
            '
            resources.ApplyResources(Me.ttbVehiculo, "ttbVehiculo")
            Me.ttbVehiculo.Name = "ttbVehiculo"
            '
            'ttbCliente
            '
            resources.ApplyResources(Me.ttbCliente, "ttbCliente")
            Me.ttbCliente.Name = "ttbCliente"
            '
            'tbbVisita
            '
            resources.ApplyResources(Me.tbbVisita, "tbbVisita")
            Me.tbbVisita.Name = "tbbVisita"
            '
            'tbbArchivos
            '
            resources.ApplyResources(Me.tbbArchivos, "tbbArchivos")
            Me.tbbArchivos.Name = "tbbArchivos"
            '
            'imgOrdenPrincipal
            '
            Me.imgOrdenPrincipal.ImageStream = CType(resources.GetObject("imgOrdenPrincipal.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.imgOrdenPrincipal.TransparentColor = System.Drawing.Color.Empty
            Me.imgOrdenPrincipal.Images.SetKeyName(0, "")
            Me.imgOrdenPrincipal.Images.SetKeyName(1, "")
            Me.imgOrdenPrincipal.Images.SetKeyName(2, "")
            Me.imgOrdenPrincipal.Images.SetKeyName(3, "")
            Me.imgOrdenPrincipal.Images.SetKeyName(4, "")
            Me.imgOrdenPrincipal.Images.SetKeyName(5, "")
            Me.imgOrdenPrincipal.Images.SetKeyName(6, "")
            Me.imgOrdenPrincipal.Images.SetKeyName(7, "")
            Me.imgOrdenPrincipal.Images.SetKeyName(8, "")
            Me.imgOrdenPrincipal.Images.SetKeyName(9, "")
            Me.imgOrdenPrincipal.Images.SetKeyName(10, "")
            Me.imgOrdenPrincipal.Images.SetKeyName(11, "")
            Me.imgOrdenPrincipal.Images.SetKeyName(12, "sbo_humanRe.gif")
            Me.imgOrdenPrincipal.Images.SetKeyName(13, "S_B_INTE copy.gif")
            Me.imgOrdenPrincipal.Images.SetKeyName(14, "producción.gif")
            Me.imgOrdenPrincipal.Images.SetKeyName(15, "OpenFolder1.gif")
            Me.imgOrdenPrincipal.Images.SetKeyName(16, "ordenes16x16.gif")
            Me.imgOrdenPrincipal.Images.SetKeyName(17, "recursos humanos .gif")
            '
            'tabRepuestos
            '
            Me.tabRepuestos.Controls.Add(Me.btnAsignarARepuesto)
            Me.tabRepuestos.Controls.Add(Me.btnSolicitar)
            Me.tabRepuestos.Controls.Add(Me.btnSolicitudes)
            Me.tabRepuestos.Controls.Add(Me.lblLine16)
            Me.tabRepuestos.Controls.Add(Me.cboEstadoRep2)
            Me.tabRepuestos.Controls.Add(Me.GroupBox5)
            Me.tabRepuestos.Controls.Add(Me.lblEstado)
            Me.tabRepuestos.Controls.Add(Me.btnAdicional)
            Me.tabRepuestos.Controls.Add(Me.btnRepuesto)
            resources.ApplyResources(Me.tabRepuestos, "tabRepuestos")
            Me.tabRepuestos.Name = "tabRepuestos"
            '
            'btnAsignarARepuesto
            '
            resources.ApplyResources(Me.btnAsignarARepuesto, "btnAsignarARepuesto")
            Me.btnAsignarARepuesto.ForeColor = System.Drawing.Color.Black
            Me.btnAsignarARepuesto.Name = "btnAsignarARepuesto"
            '
            'btnSolicitar
            '
            resources.ApplyResources(Me.btnSolicitar, "btnSolicitar")
            Me.btnSolicitar.ForeColor = System.Drawing.Color.Black
            Me.btnSolicitar.Name = "btnSolicitar"
            '
            'btnSolicitudes
            '
            resources.ApplyResources(Me.btnSolicitudes, "btnSolicitudes")
            Me.btnSolicitudes.ForeColor = System.Drawing.Color.Black
            Me.btnSolicitudes.Name = "btnSolicitudes"
            '
            'lblLine16
            '
            Me.lblLine16.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine16, "lblLine16")
            Me.lblLine16.Name = "lblLine16"
            '
            'cboEstadoRep2
            '
            Me.cboEstadoRep2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboEstadoRep2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstadoRep2.EstiloSBO = True
            resources.ApplyResources(Me.cboEstadoRep2, "cboEstadoRep2")
            Me.cboEstadoRep2.Name = "cboEstadoRep2"
            '
            'GroupBox5
            '
            resources.ApplyResources(Me.GroupBox5, "GroupBox5")
            Me.GroupBox5.Controls.Add(Me.Panel1)
            Me.GroupBox5.Controls.Add(Me.btnFechaComp)
            Me.GroupBox5.Controls.Add(Me.dtpFechaCompromiso)
            Me.GroupBox5.Controls.Add(Me.btnCheckAll)
            Me.GroupBox5.Controls.Add(Me.btnEliminarRep)
            Me.GroupBox5.Controls.Add(Me.btnAgregarRep)
            Me.GroupBox5.Controls.Add(Me.dtgRepuestos)
            Me.GroupBox5.Controls.Add(Me.lblLine17)
            Me.GroupBox5.Controls.Add(Me.chkAdicionalRep)
            Me.GroupBox5.Controls.Add(Me.cboEstadoRep)
            Me.GroupBox5.Controls.Add(Me.btnCambiarEstadoRepuesto)
            Me.GroupBox5.Controls.Add(Me.Label18)
            Me.GroupBox5.Controls.Add(Me.btnOrdenCompra)
            Me.GroupBox5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.GroupBox5.Name = "GroupBox5"
            Me.GroupBox5.TabStop = False
            '
            'Panel1
            '
            resources.ApplyResources(Me.Panel1, "Panel1")
            Me.Panel1.Name = "Panel1"
            '
            'btnFechaComp
            '
            resources.ApplyResources(Me.btnFechaComp, "btnFechaComp")
            Me.btnFechaComp.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnFechaComp.ForeColor = System.Drawing.Color.Black
            Me.btnFechaComp.Name = "btnFechaComp"
            '
            'dtpFechaCompromiso
            '
            resources.ApplyResources(Me.dtpFechaCompromiso, "dtpFechaCompromiso")
            Me.dtpFechaCompromiso.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaCompromiso.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaCompromiso.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaCompromiso.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaCompromiso.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaCompromiso.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpFechaCompromiso.Name = "dtpFechaCompromiso"
            Me.dtpFechaCompromiso.Value = New Date(2005, 12, 6, 0, 0, 0, 0)
            '
            'btnCheckAll
            '
            resources.ApplyResources(Me.btnCheckAll, "btnCheckAll")
            Me.btnCheckAll.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCheckAll.ForeColor = System.Drawing.Color.Black
            Me.btnCheckAll.Name = "btnCheckAll"
            '
            'btnEliminarRep
            '
            resources.ApplyResources(Me.btnEliminarRep, "btnEliminarRep")
            Me.btnEliminarRep.ForeColor = System.Drawing.Color.Maroon
            Me.btnEliminarRep.Name = "btnEliminarRep"
            '
            'btnAgregarRep
            '
            resources.ApplyResources(Me.btnAgregarRep, "btnAgregarRep")
            Me.btnAgregarRep.ForeColor = System.Drawing.Color.Maroon
            Me.btnAgregarRep.Name = "btnAgregarRep"
            '
            'dtgRepuestos
            '
            resources.ApplyResources(Me.dtgRepuestos, "dtgRepuestos")
            Me.dtgRepuestos.BackgroundColor = System.Drawing.Color.White
            Me.dtgRepuestos.CaptionVisible = False
            Me.dtgRepuestos.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgRepuestos.HeaderBackColor = System.Drawing.Color.White
            Me.dtgRepuestos.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.dtgRepuestos.HeaderForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgRepuestos.Name = "dtgRepuestos"
            Me.dtgRepuestos.RowHeadersVisible = False
            '
            'lblLine17
            '
            Me.lblLine17.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine17, "lblLine17")
            Me.lblLine17.Name = "lblLine17"
            '
            'chkAdicionalRep
            '
            resources.ApplyResources(Me.chkAdicionalRep, "chkAdicionalRep")
            Me.chkAdicionalRep.ForeColor = System.Drawing.Color.Maroon
            Me.chkAdicionalRep.Name = "chkAdicionalRep"
            Me.chkAdicionalRep.UseVisualStyleBackColor = False
            '
            'cboEstadoRep
            '
            Me.cboEstadoRep.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboEstadoRep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstadoRep.EstiloSBO = True
            resources.ApplyResources(Me.cboEstadoRep, "cboEstadoRep")
            Me.cboEstadoRep.Items.AddRange(New Object() {Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation})
            Me.cboEstadoRep.Name = "cboEstadoRep"
            '
            'btnCambiarEstadoRepuesto
            '
            resources.ApplyResources(Me.btnCambiarEstadoRepuesto, "btnCambiarEstadoRepuesto")
            Me.btnCambiarEstadoRepuesto.ForeColor = System.Drawing.Color.Black
            Me.btnCambiarEstadoRepuesto.Name = "btnCambiarEstadoRepuesto"
            '
            'Label18
            '
            resources.ApplyResources(Me.Label18, "Label18")
            Me.Label18.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label18.Name = "Label18"
            '
            'btnOrdenCompra
            '
            resources.ApplyResources(Me.btnOrdenCompra, "btnOrdenCompra")
            Me.btnOrdenCompra.ForeColor = System.Drawing.Color.Black
            Me.btnOrdenCompra.Name = "btnOrdenCompra"
            '
            'lblEstado
            '
            resources.ApplyResources(Me.lblEstado, "lblEstado")
            Me.lblEstado.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblEstado.Name = "lblEstado"
            '
            'btnAdicional
            '
            resources.ApplyResources(Me.btnAdicional, "btnAdicional")
            Me.btnAdicional.ForeColor = System.Drawing.Color.Black
            Me.btnAdicional.Name = "btnAdicional"
            '
            'btnRepuesto
            '
            resources.ApplyResources(Me.btnRepuesto, "btnRepuesto")
            Me.btnRepuesto.ForeColor = System.Drawing.Color.Black
            Me.btnRepuesto.Name = "btnRepuesto"
            '
            'tabFasesProd
            '
            Me.tabFasesProd.Controls.Add(Me.lblUnidadTiempo)
            Me.tabFasesProd.Controls.Add(Me.cboFases_Producción)
            Me.tabFasesProd.Controls.Add(Me.txtFSalida)
            Me.tabFasesProd.Controls.Add(Me.lblLine21)
            Me.tabFasesProd.Controls.Add(Me.GroupBox2)
            Me.tabFasesProd.Controls.Add(Me.lblLine22)
            Me.tabFasesProd.Controls.Add(Me.lblFechaSalidaFase)
            Me.tabFasesProd.Controls.Add(Me.Label47)
            Me.tabFasesProd.Controls.Add(Me.tbr_SCG)
            Me.tabFasesProd.Controls.Add(Me.btnImprimirListaCalidad)
            Me.tabFasesProd.Controls.Add(Me.picEstado)
            resources.ApplyResources(Me.tabFasesProd, "tabFasesProd")
            Me.tabFasesProd.Name = "tabFasesProd"
            '
            'lblUnidadTiempo
            '
            resources.ApplyResources(Me.lblUnidadTiempo, "lblUnidadTiempo")
            Me.lblUnidadTiempo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblUnidadTiempo.Name = "lblUnidadTiempo"
            '
            'cboFases_Producción
            '
            Me.cboFases_Producción.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboFases_Producción.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboFases_Producción.EstiloSBO = True
            resources.ApplyResources(Me.cboFases_Producción, "cboFases_Producción")
            Me.cboFases_Producción.Name = "cboFases_Producción"
            '
            'txtFSalida
            '
            Me.txtFSalida.AceptaNegativos = False
            resources.ApplyResources(Me.txtFSalida, "txtFSalida")
            Me.txtFSalida.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtFSalida.EstiloSBO = True
            Me.txtFSalida.ForeColor = System.Drawing.Color.Black
            Me.txtFSalida.MaxDecimales = 2
            Me.txtFSalida.MaxEnteros = 5
            Me.txtFSalida.Millares = False
            Me.txtFSalida.Name = "txtFSalida"
            Me.txtFSalida.ReadOnly = True
            Me.txtFSalida.Size_AdjustableHeight = 20
            Me.txtFSalida.TeclasDeshacer = True
            Me.txtFSalida.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.NumDecimal
            '
            'lblLine21
            '
            Me.lblLine21.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine21, "lblLine21")
            Me.lblLine21.Name = "lblLine21"
            '
            'GroupBox2
            '
            resources.ApplyResources(Me.GroupBox2, "GroupBox2")
            Me.GroupBox2.Controls.Add(Me.cboActividadesAsignables)
            Me.GroupBox2.Controls.Add(Me.lbllinea)
            Me.GroupBox2.Controls.Add(Me.btnMenuFases)
            Me.GroupBox2.Controls.Add(Me.btnEliminarColaborador)
            Me.GroupBox2.Controls.Add(Me.chkRefSuperiores)
            Me.GroupBox2.Controls.Add(Me.chkReproceso)
            Me.GroupBox2.Controls.Add(Me.cbocolaborador)
            Me.GroupBox2.Controls.Add(Me.dtgcolaborador)
            Me.GroupBox2.Controls.Add(Me.btnSuspende)
            Me.GroupBox2.Controls.Add(Me.lblLine23)
            Me.GroupBox2.Controls.Add(Me.Label54)
            Me.GroupBox2.Controls.Add(Me.btnAsignar)
            Me.GroupBox2.Controls.Add(Me.btnFinaliza)
            Me.GroupBox2.Controls.Add(Me.btnInicioFecha)
            Me.GroupBox2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.TabStop = False
            '
            'cboActividadesAsignables
            '
            resources.ApplyResources(Me.cboActividadesAsignables, "cboActividadesAsignables")
            Me.cboActividadesAsignables.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboActividadesAsignables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboActividadesAsignables.EstiloSBO = True
            Me.cboActividadesAsignables.FormattingEnabled = True
            Me.cboActividadesAsignables.Name = "cboActividadesAsignables"
            '
            'lbllinea
            '
            resources.ApplyResources(Me.lbllinea, "lbllinea")
            Me.lbllinea.BackColor = System.Drawing.Color.Gray
            Me.lbllinea.Name = "lbllinea"
            '
            'btnMenuFases
            '
            Me.btnMenuFases.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.btnMenuFases, "btnMenuFases")
            Me.btnMenuFases.ForeColor = System.Drawing.Color.Black
            Me.btnMenuFases.Name = "btnMenuFases"
            Me.btnMenuFases.UseVisualStyleBackColor = False
            '
            'btnEliminarColaborador
            '
            Me.btnEliminarColaborador.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.btnEliminarColaborador, "btnEliminarColaborador")
            Me.btnEliminarColaborador.ForeColor = System.Drawing.Color.Black
            Me.btnEliminarColaborador.Name = "btnEliminarColaborador"
            Me.btnEliminarColaborador.UseVisualStyleBackColor = False
            '
            'chkRefSuperiores
            '
            resources.ApplyResources(Me.chkRefSuperiores, "chkRefSuperiores")
            Me.chkRefSuperiores.Checked = True
            Me.chkRefSuperiores.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkRefSuperiores.Name = "chkRefSuperiores"
            '
            'chkReproceso
            '
            resources.ApplyResources(Me.chkReproceso, "chkReproceso")
            Me.chkReproceso.Name = "chkReproceso"
            '
            'cbocolaborador
            '
            Me.cbocolaborador.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cbocolaborador.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbocolaborador.EstiloSBO = True
            resources.ApplyResources(Me.cbocolaborador, "cbocolaborador")
            Me.cbocolaborador.Name = "cbocolaborador"
            '
            'dtgcolaborador
            '
            resources.ApplyResources(Me.dtgcolaborador, "dtgcolaborador")
            Me.dtgcolaborador.BackgroundColor = System.Drawing.Color.White
            Me.dtgcolaborador.CaptionVisible = False
            Me.dtgcolaborador.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgcolaborador.HeaderBackColor = System.Drawing.Color.White
            Me.dtgcolaborador.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.dtgcolaborador.HeaderForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgcolaborador.Name = "dtgcolaborador"
            Me.dtgcolaborador.RowHeadersVisible = False
            '
            'btnSuspende
            '
            resources.ApplyResources(Me.btnSuspende, "btnSuspende")
            Me.btnSuspende.ForeColor = System.Drawing.Color.Black
            Me.btnSuspende.Name = "btnSuspende"
            '
            'lblLine23
            '
            Me.lblLine23.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine23, "lblLine23")
            Me.lblLine23.Name = "lblLine23"
            '
            'Label54
            '
            resources.ApplyResources(Me.Label54, "Label54")
            Me.Label54.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label54.Name = "Label54"
            '
            'btnAsignar
            '
            Me.btnAsignar.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.btnAsignar, "btnAsignar")
            Me.btnAsignar.ForeColor = System.Drawing.Color.Black
            Me.btnAsignar.Name = "btnAsignar"
            Me.btnAsignar.UseVisualStyleBackColor = False
            '
            'btnFinaliza
            '
            resources.ApplyResources(Me.btnFinaliza, "btnFinaliza")
            Me.btnFinaliza.ForeColor = System.Drawing.Color.Black
            Me.btnFinaliza.Name = "btnFinaliza"
            '
            'btnInicioFecha
            '
            resources.ApplyResources(Me.btnInicioFecha, "btnInicioFecha")
            Me.btnInicioFecha.ForeColor = System.Drawing.Color.Black
            Me.btnInicioFecha.Name = "btnInicioFecha"
            '
            'lblLine22
            '
            resources.ApplyResources(Me.lblLine22, "lblLine22")
            Me.lblLine22.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            Me.lblLine22.Name = "lblLine22"
            '
            'lblFechaSalidaFase
            '
            resources.ApplyResources(Me.lblFechaSalidaFase, "lblFechaSalidaFase")
            Me.lblFechaSalidaFase.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblFechaSalidaFase.Name = "lblFechaSalidaFase"
            '
            'Label47
            '
            resources.ApplyResources(Me.Label47, "Label47")
            Me.Label47.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label47.Name = "Label47"
            '
            'tbr_SCG
            '
            resources.ApplyResources(Me.tbr_SCG, "tbr_SCG")
            Me.tbr_SCG.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.btnAsignacionMultiple, Me.btnAsignarTiempos, Me.btnIniciar, Me.btnRechazar, Me.btnReproceso, Me.btnSuspension, Me.btnCalidad, Me.btnDocumentos, Me.btnFinalizar})
            Me.tbr_SCG.ImageList = Me.imglst_ProcProd
            Me.tbr_SCG.Name = "tbr_SCG"
            '
            'btnAsignacionMultiple
            '
            resources.ApplyResources(Me.btnAsignacionMultiple, "btnAsignacionMultiple")
            Me.btnAsignacionMultiple.Name = "btnAsignacionMultiple"
            '
            'btnAsignarTiempos
            '
            resources.ApplyResources(Me.btnAsignarTiempos, "btnAsignarTiempos")
            Me.btnAsignarTiempos.Name = "btnAsignarTiempos"
            '
            'btnIniciar
            '
            resources.ApplyResources(Me.btnIniciar, "btnIniciar")
            Me.btnIniciar.Name = "btnIniciar"
            '
            'btnRechazar
            '
            resources.ApplyResources(Me.btnRechazar, "btnRechazar")
            Me.btnRechazar.Name = "btnRechazar"
            '
            'btnReproceso
            '
            resources.ApplyResources(Me.btnReproceso, "btnReproceso")
            Me.btnReproceso.Name = "btnReproceso"
            '
            'btnSuspension
            '
            resources.ApplyResources(Me.btnSuspension, "btnSuspension")
            Me.btnSuspension.Name = "btnSuspension"
            '
            'btnCalidad
            '
            resources.ApplyResources(Me.btnCalidad, "btnCalidad")
            Me.btnCalidad.Name = "btnCalidad"
            '
            'btnDocumentos
            '
            Me.btnDocumentos.DropDownMenu = Me.mnuDocumentos
            resources.ApplyResources(Me.btnDocumentos, "btnDocumentos")
            Me.btnDocumentos.Name = "btnDocumentos"
            Me.btnDocumentos.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
            '
            'mnuDocumentos
            '
            Me.mnuDocumentos.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.dropmnuProduccion, Me.dropmnuCostos, Me.dropmnuItemsNoAprobados, Me.dropmnuOficina, Me.dropmnuBalanceOT, Me.MenuItem3, Me.dropmnuReprocesos, Me.dropmnuSuspenciones})
            '
            'dropmnuProduccion
            '
            Me.dropmnuProduccion.Index = 0
            resources.ApplyResources(Me.dropmnuProduccion, "dropmnuProduccion")
            '
            'dropmnuCostos
            '
            Me.dropmnuCostos.Index = 1
            resources.ApplyResources(Me.dropmnuCostos, "dropmnuCostos")
            '
            'dropmnuItemsNoAprobados
            '
            Me.dropmnuItemsNoAprobados.Index = 2
            resources.ApplyResources(Me.dropmnuItemsNoAprobados, "dropmnuItemsNoAprobados")
            '
            'dropmnuOficina
            '
            Me.dropmnuOficina.Index = 3
            resources.ApplyResources(Me.dropmnuOficina, "dropmnuOficina")
            '
            'dropmnuBalanceOT
            '
            Me.dropmnuBalanceOT.Index = 4
            resources.ApplyResources(Me.dropmnuBalanceOT, "dropmnuBalanceOT")
            '
            'MenuItem3
            '
            Me.MenuItem3.Index = 5
            resources.ApplyResources(Me.MenuItem3, "MenuItem3")
            '
            'dropmnuReprocesos
            '
            Me.dropmnuReprocesos.Index = 6
            resources.ApplyResources(Me.dropmnuReprocesos, "dropmnuReprocesos")
            '
            'dropmnuSuspenciones
            '
            Me.dropmnuSuspenciones.Index = 7
            resources.ApplyResources(Me.dropmnuSuspenciones, "dropmnuSuspenciones")
            '
            'btnFinalizar
            '
            resources.ApplyResources(Me.btnFinalizar, "btnFinalizar")
            Me.btnFinalizar.Name = "btnFinalizar"
            '
            'imglst_ProcProd
            '
            Me.imglst_ProcProd.ImageStream = CType(resources.GetObject("imglst_ProcProd.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.imglst_ProcProd.TransparentColor = System.Drawing.Color.Empty
            Me.imglst_ProcProd.Images.SetKeyName(0, "")
            Me.imglst_ProcProd.Images.SetKeyName(1, "")
            Me.imglst_ProcProd.Images.SetKeyName(2, "")
            Me.imglst_ProcProd.Images.SetKeyName(3, "")
            Me.imglst_ProcProd.Images.SetKeyName(4, "")
            Me.imglst_ProcProd.Images.SetKeyName(5, "")
            Me.imglst_ProcProd.Images.SetKeyName(6, "")
            Me.imglst_ProcProd.Images.SetKeyName(7, "")
            Me.imglst_ProcProd.Images.SetKeyName(8, "")
            Me.imglst_ProcProd.Images.SetKeyName(9, "")
            Me.imglst_ProcProd.Images.SetKeyName(10, "")
            Me.imglst_ProcProd.Images.SetKeyName(11, "")
            Me.imglst_ProcProd.Images.SetKeyName(12, "ALTO.gif")
            Me.imglst_ProcProd.Images.SetKeyName(13, "asig multiple3.gif")
            Me.imglst_ProcProd.Images.SetKeyName(14, "iniciar.gif")
            Me.imglst_ProcProd.Images.SetKeyName(15, "orden de trabajo osc.gif")
            Me.imglst_ProcProd.Images.SetKeyName(16, "reloj.gif")
            Me.imglst_ProcProd.Images.SetKeyName(17, "suspención.gif")
            '
            'btnImprimirListaCalidad
            '
            resources.ApplyResources(Me.btnImprimirListaCalidad, "btnImprimirListaCalidad")
            Me.btnImprimirListaCalidad.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnImprimirListaCalidad.Name = "btnImprimirListaCalidad"
            '
            'picEstado
            '
            resources.ApplyResources(Me.picEstado, "picEstado")
            Me.picEstado.Name = "picEstado"
            Me.picEstado.TabStop = False
            '
            'tabActividades
            '
            Me.tabActividades.Controls.Add(Me.btnActAdicional)
            Me.tabActividades.Controls.Add(Me.btnActividad)
            Me.tabActividades.Controls.Add(Me.cboFasesProdF)
            Me.tabActividades.Controls.Add(Me.grpActividadesProduccion)
            Me.tabActividades.Controls.Add(Me.line19)
            Me.tabActividades.Controls.Add(Me.Label36)
            resources.ApplyResources(Me.tabActividades, "tabActividades")
            Me.tabActividades.Name = "tabActividades"
            '
            'btnActAdicional
            '
            resources.ApplyResources(Me.btnActAdicional, "btnActAdicional")
            Me.btnActAdicional.ForeColor = System.Drawing.Color.Black
            Me.btnActAdicional.Name = "btnActAdicional"
            '
            'btnActividad
            '
            resources.ApplyResources(Me.btnActividad, "btnActividad")
            Me.btnActividad.ForeColor = System.Drawing.Color.Black
            Me.btnActividad.Name = "btnActividad"
            '
            'cboFasesProdF
            '
            Me.cboFasesProdF.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboFasesProdF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboFasesProdF.EstiloSBO = True
            resources.ApplyResources(Me.cboFasesProdF, "cboFasesProdF")
            Me.cboFasesProdF.Name = "cboFasesProdF"
            '
            'grpActividadesProduccion
            '
            resources.ApplyResources(Me.grpActividadesProduccion, "grpActividadesProduccion")
            Me.grpActividadesProduccion.Controls.Add(Me.btnEliminarAct)
            Me.grpActividadesProduccion.Controls.Add(Me.btnAgregarAct)
            Me.grpActividadesProduccion.Controls.Add(Me.dtgActividades)
            Me.grpActividadesProduccion.Controls.Add(Me.btnCambiarEstadoActividad)
            Me.grpActividadesProduccion.Controls.Add(Me.line20)
            Me.grpActividadesProduccion.Controls.Add(Me.cboEstado)
            Me.grpActividadesProduccion.Controls.Add(Me.Label52)
            Me.grpActividadesProduccion.Controls.Add(Me.chkAdicionalAct)
            Me.grpActividadesProduccion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.grpActividadesProduccion.Name = "grpActividadesProduccion"
            Me.grpActividadesProduccion.TabStop = False
            '
            'btnEliminarAct
            '
            resources.ApplyResources(Me.btnEliminarAct, "btnEliminarAct")
            Me.btnEliminarAct.ForeColor = System.Drawing.Color.Maroon
            Me.btnEliminarAct.Name = "btnEliminarAct"
            '
            'btnAgregarAct
            '
            resources.ApplyResources(Me.btnAgregarAct, "btnAgregarAct")
            Me.btnAgregarAct.ForeColor = System.Drawing.Color.Maroon
            Me.btnAgregarAct.Name = "btnAgregarAct"
            '
            'dtgActividades
            '
            resources.ApplyResources(Me.dtgActividades, "dtgActividades")
            Me.dtgActividades.BackgroundColor = System.Drawing.Color.White
            Me.dtgActividades.CaptionVisible = False
            Me.dtgActividades.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgActividades.HeaderBackColor = System.Drawing.Color.White
            Me.dtgActividades.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.dtgActividades.HeaderForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgActividades.Name = "dtgActividades"
            Me.dtgActividades.RowHeadersVisible = False
            '
            'btnCambiarEstadoActividad
            '
            resources.ApplyResources(Me.btnCambiarEstadoActividad, "btnCambiarEstadoActividad")
            Me.btnCambiarEstadoActividad.ForeColor = System.Drawing.Color.Black
            Me.btnCambiarEstadoActividad.Name = "btnCambiarEstadoActividad"
            '
            'line20
            '
            Me.line20.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.line20, "line20")
            Me.line20.Name = "line20"
            '
            'cboEstado
            '
            Me.cboEstado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstado.EstiloSBO = True
            resources.ApplyResources(Me.cboEstado, "cboEstado")
            Me.cboEstado.Items.AddRange(New Object() {resources.GetString("cboEstado.Items"), resources.GetString("cboEstado.Items1")})
            Me.cboEstado.Name = "cboEstado"
            '
            'Label52
            '
            resources.ApplyResources(Me.Label52, "Label52")
            Me.Label52.BackColor = System.Drawing.SystemColors.Control
            Me.Label52.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label52.Name = "Label52"
            '
            'chkAdicionalAct
            '
            resources.ApplyResources(Me.chkAdicionalAct, "chkAdicionalAct")
            Me.chkAdicionalAct.ForeColor = System.Drawing.Color.Maroon
            Me.chkAdicionalAct.Name = "chkAdicionalAct"
            '
            'line19
            '
            Me.line19.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.line19, "line19")
            Me.line19.Name = "line19"
            '
            'Label36
            '
            resources.ApplyResources(Me.Label36, "Label36")
            Me.Label36.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label36.Name = "Label36"
            '
            'tabSuministros
            '
            Me.tabSuministros.Controls.Add(Me.grbSuministros)
            resources.ApplyResources(Me.tabSuministros, "tabSuministros")
            Me.tabSuministros.Name = "tabSuministros"
            '
            'grbSuministros
            '
            resources.ApplyResources(Me.grbSuministros, "grbSuministros")
            Me.grbSuministros.Controls.Add(Me.chkAdicionalesSu)
            Me.grbSuministros.Controls.Add(Me.btnEliminaSum)
            Me.grbSuministros.Controls.Add(Me.btnAgregaSum)
            Me.grbSuministros.Controls.Add(Me.dtgSuministros)
            Me.grbSuministros.Controls.Add(Me.btnRequisiciones)
            Me.grbSuministros.Controls.Add(Me.btnDevoluciones)
            Me.grbSuministros.Controls.Add(Me.btnSuministros)
            Me.grbSuministros.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.grbSuministros.Name = "grbSuministros"
            Me.grbSuministros.TabStop = False
            '
            'chkAdicionalesSu
            '
            resources.ApplyResources(Me.chkAdicionalesSu, "chkAdicionalesSu")
            Me.chkAdicionalesSu.ForeColor = System.Drawing.Color.Maroon
            Me.chkAdicionalesSu.Name = "chkAdicionalesSu"
            '
            'btnEliminaSum
            '
            resources.ApplyResources(Me.btnEliminaSum, "btnEliminaSum")
            Me.btnEliminaSum.ForeColor = System.Drawing.Color.Maroon
            Me.btnEliminaSum.Name = "btnEliminaSum"
            '
            'btnAgregaSum
            '
            resources.ApplyResources(Me.btnAgregaSum, "btnAgregaSum")
            Me.btnAgregaSum.ForeColor = System.Drawing.Color.Maroon
            Me.btnAgregaSum.Name = "btnAgregaSum"
            '
            'dtgSuministros
            '
            resources.ApplyResources(Me.dtgSuministros, "dtgSuministros")
            Me.dtgSuministros.BackgroundColor = System.Drawing.Color.White
            Me.dtgSuministros.CaptionVisible = False
            Me.dtgSuministros.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgSuministros.HeaderBackColor = System.Drawing.Color.White
            Me.dtgSuministros.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.dtgSuministros.HeaderForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgSuministros.Name = "dtgSuministros"
            Me.dtgSuministros.RowHeadersVisible = False
            '
            'btnRequisiciones
            '
            resources.ApplyResources(Me.btnRequisiciones, "btnRequisiciones")
            Me.btnRequisiciones.Name = "btnRequisiciones"
            Me.TTColaboras.SetToolTip(Me.btnRequisiciones, resources.GetString("btnRequisiciones.ToolTip"))
            Me.tipResponsable.SetToolTip(Me.btnRequisiciones, resources.GetString("btnRequisiciones.ToolTip1"))
            Me.tipSuministros.SetToolTip(Me.btnRequisiciones, resources.GetString("btnRequisiciones.ToolTip2"))
            '
            'btnDevoluciones
            '
            resources.ApplyResources(Me.btnDevoluciones, "btnDevoluciones")
            Me.btnDevoluciones.Name = "btnDevoluciones"
            Me.TTColaboras.SetToolTip(Me.btnDevoluciones, resources.GetString("btnDevoluciones.ToolTip"))
            Me.tipResponsable.SetToolTip(Me.btnDevoluciones, resources.GetString("btnDevoluciones.ToolTip1"))
            Me.tipSuministros.SetToolTip(Me.btnDevoluciones, resources.GetString("btnDevoluciones.ToolTip2"))
            '
            'btnSuministros
            '
            resources.ApplyResources(Me.btnSuministros, "btnSuministros")
            Me.btnSuministros.Name = "btnSuministros"
            Me.TTColaboras.SetToolTip(Me.btnSuministros, resources.GetString("btnSuministros.ToolTip"))
            Me.TTDuracionEN.SetToolTip(Me.btnSuministros, resources.GetString("btnSuministros.ToolTip1"))
            Me.tipResponsable.SetToolTip(Me.btnSuministros, resources.GetString("btnSuministros.ToolTip2"))
            Me.tipSuministros.SetToolTip(Me.btnSuministros, resources.GetString("btnSuministros.ToolTip3"))
            '
            'tabServiciosExternos
            '
            Me.tabServiciosExternos.Controls.Add(Me.grbServiciosExternos)
            resources.ApplyResources(Me.tabServiciosExternos, "tabServiciosExternos")
            Me.tabServiciosExternos.Name = "tabServiciosExternos"
            '
            'grbServiciosExternos
            '
            resources.ApplyResources(Me.grbServiciosExternos, "grbServiciosExternos")
            Me.grbServiciosExternos.Controls.Add(Me.btnOrdenCompraSE)
            Me.grbServiciosExternos.Controls.Add(Me.Label3)
            Me.grbServiciosExternos.Controls.Add(Me.cbEstadoSE)
            Me.grbServiciosExternos.Controls.Add(Me.Label7)
            Me.grbServiciosExternos.Controls.Add(Me.btnEliminarSE)
            Me.grbServiciosExternos.Controls.Add(Me.btnAgregarSE)
            Me.grbServiciosExternos.Controls.Add(Me.dtgSE)
            Me.grbServiciosExternos.Controls.Add(Me.chkAdicionalesSE)
            Me.grbServiciosExternos.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.grbServiciosExternos.Name = "grbServiciosExternos"
            Me.grbServiciosExternos.TabStop = False
            '
            'btnOrdenCompraSE
            '
            resources.ApplyResources(Me.btnOrdenCompraSE, "btnOrdenCompraSE")
            Me.btnOrdenCompraSE.ForeColor = System.Drawing.Color.Black
            Me.btnOrdenCompraSE.Name = "btnOrdenCompraSE"
            '
            'Label3
            '
            Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.Name = "Label3"
            '
            'cbEstadoSE
            '
            Me.cbEstadoSE.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cbEstadoSE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbEstadoSE.EstiloSBO = True
            resources.ApplyResources(Me.cbEstadoSE, "cbEstadoSE")
            Me.cbEstadoSE.Items.AddRange(New Object() {resources.GetString("cbEstadoSE.Items"), resources.GetString("cbEstadoSE.Items1")})
            Me.cbEstadoSE.Name = "cbEstadoSE"
            '
            'Label7
            '
            resources.ApplyResources(Me.Label7, "Label7")
            Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label7.Name = "Label7"
            '
            'btnEliminarSE
            '
            resources.ApplyResources(Me.btnEliminarSE, "btnEliminarSE")
            Me.btnEliminarSE.ForeColor = System.Drawing.Color.Maroon
            Me.btnEliminarSE.Name = "btnEliminarSE"
            '
            'btnAgregarSE
            '
            resources.ApplyResources(Me.btnAgregarSE, "btnAgregarSE")
            Me.btnAgregarSE.ForeColor = System.Drawing.Color.Maroon
            Me.btnAgregarSE.Name = "btnAgregarSE"
            '
            'dtgSE
            '
            resources.ApplyResources(Me.dtgSE, "dtgSE")
            Me.dtgSE.BackgroundColor = System.Drawing.Color.White
            Me.dtgSE.CaptionVisible = False
            Me.dtgSE.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgSE.HeaderBackColor = System.Drawing.Color.White
            Me.dtgSE.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.dtgSE.HeaderForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgSE.Name = "dtgSE"
            Me.dtgSE.RowHeadersVisible = False
            '
            'chkAdicionalesSE
            '
            resources.ApplyResources(Me.chkAdicionalesSE, "chkAdicionalesSE")
            Me.chkAdicionalesSE.ForeColor = System.Drawing.Color.Maroon
            Me.chkAdicionalesSE.Name = "chkAdicionalesSE"
            '
            'tabRendimiento
            '
            Me.tabRendimiento.Controls.Add(Me.btnActualizar)
            Me.tabRendimiento.Controls.Add(Me.Label4)
            Me.tabRendimiento.Controls.Add(Me.dtgRendimientosBarras)
            Me.tabRendimiento.Controls.Add(Me.dtgMontoReparacion)
            resources.ApplyResources(Me.tabRendimiento, "tabRendimiento")
            Me.tabRendimiento.Name = "tabRendimiento"
            '
            'btnActualizar
            '
            resources.ApplyResources(Me.btnActualizar, "btnActualizar")
            Me.btnActualizar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnActualizar.Name = "btnActualizar"
            '
            'Label4
            '
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label4.Name = "Label4"
            '
            'dtgRendimientosBarras
            '
            resources.ApplyResources(Me.dtgRendimientosBarras, "dtgRendimientosBarras")
            Me.dtgRendimientosBarras.BackgroundColor = System.Drawing.Color.White
            Me.dtgRendimientosBarras.CaptionVisible = False
            Me.dtgRendimientosBarras.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgRendimientosBarras.HeaderBackColor = System.Drawing.Color.White
            Me.dtgRendimientosBarras.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.dtgRendimientosBarras.HeaderForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgRendimientosBarras.Name = "dtgRendimientosBarras"
            Me.dtgRendimientosBarras.PreferredRowHeight = 41
            Me.dtgRendimientosBarras.RowHeadersVisible = False
            '
            'dtgMontoReparacion
            '
            Me.dtgMontoReparacion.BackgroundColor = System.Drawing.Color.White
            resources.ApplyResources(Me.dtgMontoReparacion, "dtgMontoReparacion")
            Me.dtgMontoReparacion.CaptionVisible = False
            Me.dtgMontoReparacion.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgMontoReparacion.HeaderBackColor = System.Drawing.Color.White
            Me.dtgMontoReparacion.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.dtgMontoReparacion.HeaderForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgMontoReparacion.Name = "dtgMontoReparacion"
            Me.dtgMontoReparacion.RowHeadersVisible = False
            '
            'tabOtrosGastos
            '
            Me.tabOtrosGastos.Controls.Add(Me.GroupBox4)
            Me.tabOtrosGastos.Controls.Add(Me.Panel3)
            resources.ApplyResources(Me.tabOtrosGastos, "tabOtrosGastos")
            Me.tabOtrosGastos.Name = "tabOtrosGastos"
            '
            'GroupBox4
            '
            Me.GroupBox4.Controls.Add(Me.FlowLayoutPanel1)
            resources.ApplyResources(Me.GroupBox4, "GroupBox4")
            Me.GroupBox4.Name = "GroupBox4"
            Me.GroupBox4.TabStop = False
            '
            'FlowLayoutPanel1
            '
            resources.ApplyResources(Me.FlowLayoutPanel1, "FlowLayoutPanel1")
            Me.FlowLayoutPanel1.Controls.Add(Me.lblOtrosGastos)
            Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
            '
            'lblOtrosGastos
            '
            resources.ApplyResources(Me.lblOtrosGastos, "lblOtrosGastos")
            Me.lblOtrosGastos.ForeColor = System.Drawing.Color.Maroon
            Me.lblOtrosGastos.Name = "lblOtrosGastos"
            '
            'Panel3
            '
            Me.Panel3.Controls.Add(Me.txtTotalOtrosGastos)
            Me.Panel3.Controls.Add(Me.btnActualizarOtrosGastos)
            Me.Panel3.Controls.Add(Me.Label22)
            Me.Panel3.Controls.Add(Me.Label23)
            resources.ApplyResources(Me.Panel3, "Panel3")
            Me.Panel3.Name = "Panel3"
            '
            'txtTotalOtrosGastos
            '
            Me.txtTotalOtrosGastos.AceptaNegativos = False
            Me.txtTotalOtrosGastos.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtTotalOtrosGastos.EstiloSBO = True
            resources.ApplyResources(Me.txtTotalOtrosGastos, "txtTotalOtrosGastos")
            Me.txtTotalOtrosGastos.ForeColor = System.Drawing.Color.Black
            Me.txtTotalOtrosGastos.MaxDecimales = 0
            Me.txtTotalOtrosGastos.MaxEnteros = 0
            Me.txtTotalOtrosGastos.Millares = False
            Me.txtTotalOtrosGastos.Name = "txtTotalOtrosGastos"
            Me.txtTotalOtrosGastos.ReadOnly = True
            Me.txtTotalOtrosGastos.Size_AdjustableHeight = 20
            Me.txtTotalOtrosGastos.TeclasDeshacer = True
            Me.txtTotalOtrosGastos.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'btnActualizarOtrosGastos
            '
            Me.btnActualizarOtrosGastos.DialogResult = System.Windows.Forms.DialogResult.Cancel
            resources.ApplyResources(Me.btnActualizarOtrosGastos, "btnActualizarOtrosGastos")
            Me.btnActualizarOtrosGastos.Name = "btnActualizarOtrosGastos"
            '
            'Label22
            '
            Me.Label22.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label22, "Label22")
            Me.Label22.Name = "Label22"
            '
            'Label23
            '
            resources.ApplyResources(Me.Label23, "Label23")
            Me.Label23.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label23.Name = "Label23"
            '
            'txtPlaca
            '
            Me.txtPlaca.AceptaNegativos = False
            Me.txtPlaca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtPlaca.EstiloSBO = True
            resources.ApplyResources(Me.txtPlaca, "txtPlaca")
            Me.txtPlaca.ForeColor = System.Drawing.Color.Black
            Me.txtPlaca.MaxDecimales = 0
            Me.txtPlaca.MaxEnteros = 0
            Me.txtPlaca.Millares = False
            Me.txtPlaca.Name = "txtPlaca"
            Me.txtPlaca.ReadOnly = True
            Me.txtPlaca.Size_AdjustableHeight = 20
            Me.txtPlaca.TeclasDeshacer = True
            Me.txtPlaca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtEstilo
            '
            Me.txtEstilo.AceptaNegativos = False
            Me.txtEstilo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtEstilo.EstiloSBO = True
            resources.ApplyResources(Me.txtEstilo, "txtEstilo")
            Me.txtEstilo.ForeColor = System.Drawing.Color.Black
            Me.txtEstilo.MaxDecimales = 0
            Me.txtEstilo.MaxEnteros = 0
            Me.txtEstilo.Millares = False
            Me.txtEstilo.Name = "txtEstilo"
            Me.txtEstilo.ReadOnly = True
            Me.txtEstilo.Size_AdjustableHeight = 20
            Me.txtEstilo.TeclasDeshacer = True
            Me.txtEstilo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtMarca
            '
            Me.txtMarca.AceptaNegativos = False
            Me.txtMarca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtMarca.EstiloSBO = True
            resources.ApplyResources(Me.txtMarca, "txtMarca")
            Me.txtMarca.ForeColor = System.Drawing.Color.Black
            Me.txtMarca.MaxDecimales = 0
            Me.txtMarca.MaxEnteros = 0
            Me.txtMarca.Millares = False
            Me.txtMarca.Name = "txtMarca"
            Me.txtMarca.ReadOnly = True
            Me.txtMarca.Size_AdjustableHeight = 20
            Me.txtMarca.TeclasDeshacer = True
            Me.txtMarca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtTipoOrden
            '
            Me.txtTipoOrden.AceptaNegativos = False
            Me.txtTipoOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtTipoOrden.EstiloSBO = True
            resources.ApplyResources(Me.txtTipoOrden, "txtTipoOrden")
            Me.txtTipoOrden.ForeColor = System.Drawing.Color.Black
            Me.txtTipoOrden.MaxDecimales = 0
            Me.txtTipoOrden.MaxEnteros = 0
            Me.txtTipoOrden.Millares = False
            Me.txtTipoOrden.Name = "txtTipoOrden"
            Me.txtTipoOrden.ReadOnly = True
            Me.txtTipoOrden.Size_AdjustableHeight = 20
            Me.txtTipoOrden.TeclasDeshacer = True
            Me.txtTipoOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtNoOrden
            '
            Me.txtNoOrden.AceptaNegativos = False
            Me.txtNoOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoOrden.EstiloSBO = True
            resources.ApplyResources(Me.txtNoOrden, "txtNoOrden")
            Me.txtNoOrden.ForeColor = System.Drawing.Color.Black
            Me.txtNoOrden.MaxDecimales = 0
            Me.txtNoOrden.MaxEnteros = 0
            Me.txtNoOrden.Millares = False
            Me.txtNoOrden.Name = "txtNoOrden"
            Me.txtNoOrden.ReadOnly = True
            Me.txtNoOrden.Size_AdjustableHeight = 20
            Me.txtNoOrden.TeclasDeshacer = True
            Me.txtNoOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtNoCono
            '
            Me.txtNoCono.AceptaNegativos = False
            Me.txtNoCono.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoCono.EstiloSBO = True
            resources.ApplyResources(Me.txtNoCono, "txtNoCono")
            Me.txtNoCono.ForeColor = System.Drawing.Color.Black
            Me.txtNoCono.MaxDecimales = 0
            Me.txtNoCono.MaxEnteros = 0
            Me.txtNoCono.Millares = False
            Me.txtNoCono.Name = "txtNoCono"
            Me.txtNoCono.ReadOnly = True
            Me.txtNoCono.Size_AdjustableHeight = 20
            Me.txtNoCono.TeclasDeshacer = True
            Me.txtNoCono.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtEstado
            '
            Me.txtEstado.AceptaNegativos = False
            Me.txtEstado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtEstado.EstiloSBO = True
            resources.ApplyResources(Me.txtEstado, "txtEstado")
            Me.txtEstado.ForeColor = System.Drawing.Color.Black
            Me.txtEstado.MaxDecimales = 0
            Me.txtEstado.MaxEnteros = 0
            Me.txtEstado.Millares = False
            Me.txtEstado.Name = "txtEstado"
            Me.txtEstado.ReadOnly = True
            Me.txtEstado.Size_AdjustableHeight = 20
            Me.txtEstado.TeclasDeshacer = True
            Me.txtEstado.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblLine4
            '
            Me.lblLine4.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine4, "lblLine4")
            Me.lblLine4.Name = "lblLine4"
            '
            'lblLine5
            '
            Me.lblLine5.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine5, "lblLine5")
            Me.lblLine5.Name = "lblLine5"
            '
            'Label12
            '
            Me.Label12.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label12, "Label12")
            Me.Label12.Name = "Label12"
            '
            'lblLine7
            '
            Me.lblLine7.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine7, "lblLine7")
            Me.lblLine7.Name = "lblLine7"
            '
            'lblLine8
            '
            Me.lblLine8.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine8, "lblLine8")
            Me.lblLine8.Name = "lblLine8"
            '
            'lblLine2
            '
            Me.lblLine2.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine2, "lblLine2")
            Me.lblLine2.Name = "lblLine2"
            '
            'lblLine3
            '
            Me.lblLine3.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine3, "lblLine3")
            Me.lblLine3.Name = "lblLine3"
            '
            'lblPlaca
            '
            resources.ApplyResources(Me.lblPlaca, "lblPlaca")
            Me.lblPlaca.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblPlaca.Name = "lblPlaca"
            '
            'lblMarca
            '
            resources.ApplyResources(Me.lblMarca, "lblMarca")
            Me.lblMarca.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblMarca.Name = "lblMarca"
            '
            'lblModelo
            '
            resources.ApplyResources(Me.lblModelo, "lblModelo")
            Me.lblModelo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblModelo.Name = "lblModelo"
            '
            'lblNoOrden
            '
            resources.ApplyResources(Me.lblNoOrden, "lblNoOrden")
            Me.lblNoOrden.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoOrden.Name = "lblNoOrden"
            '
            'lblNoCono
            '
            resources.ApplyResources(Me.lblNoCono, "lblNoCono")
            Me.lblNoCono.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoCono.Name = "lblNoCono"
            '
            'lblTipoOrdenO
            '
            resources.ApplyResources(Me.lblTipoOrdenO, "lblTipoOrdenO")
            Me.lblTipoOrdenO.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblTipoOrdenO.Name = "lblTipoOrdenO"
            '
            'lblEstadoO
            '
            resources.ApplyResources(Me.lblEstadoO, "lblEstadoO")
            Me.lblEstadoO.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblEstadoO.Name = "lblEstadoO"
            '
            'rptorden
            '
            Me.rptorden.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.rptorden, "rptorden")
            Me.rptorden.Name = "rptorden"
            Me.rptorden.P_Authentication = False
            Me.rptorden.P_BarraTitulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptorden.P_CompanyName = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptorden.P_DataBase = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptorden.P_Filename = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptorden.P_NCopias = 0
            Me.rptorden.P_Owner = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptorden.P_ParArray = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptorden.P_Password = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptorden.P_Server = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptorden.P_User = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptorden.P_WorkFolder = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'imglst_SCG
            '
            Me.imglst_SCG.ImageStream = CType(resources.GetObject("imglst_SCG.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.imglst_SCG.TransparentColor = System.Drawing.Color.Transparent
            Me.imglst_SCG.Images.SetKeyName(0, "")
            Me.imglst_SCG.Images.SetKeyName(1, "")
            Me.imglst_SCG.Images.SetKeyName(2, "")
            Me.imglst_SCG.Images.SetKeyName(3, "")
            Me.imglst_SCG.Images.SetKeyName(4, "")
            '
            'GroupBox3
            '
            Me.GroupBox3.Controls.Add(Me.Label27)
            Me.GroupBox3.Controls.Add(Me.txtKilometraje)
            Me.GroupBox3.Controls.Add(Me.Label26)
            Me.GroupBox3.Controls.Add(Me.lblKilometraje)
            Me.GroupBox3.Controls.Add(Me.txtVIN)
            Me.GroupBox3.Controls.Add(Me.Label28)
            Me.GroupBox3.Controls.Add(Me.lblVIN)
            Me.GroupBox3.Controls.Add(Me.cboEstadoWeb)
            Me.GroupBox3.Controls.Add(Me.Label25)
            Me.GroupBox3.Controls.Add(Me.txtNoVehiculo)
            Me.GroupBox3.Controls.Add(Me.lblEstadoWeb)
            Me.GroupBox3.Controls.Add(Me.Label5)
            Me.GroupBox3.Controls.Add(Me.Label6)
            Me.GroupBox3.Controls.Add(Me.txtNoVisita)
            Me.GroupBox3.Controls.Add(Me.lblLine1)
            Me.GroupBox3.Controls.Add(Me.lblNoVisita)
            Me.GroupBox3.Controls.Add(Me.txtEstado)
            Me.GroupBox3.Controls.Add(Me.txtPlaca)
            Me.GroupBox3.Controls.Add(Me.txtEstilo)
            Me.GroupBox3.Controls.Add(Me.txtMarca)
            Me.GroupBox3.Controls.Add(Me.txtTipoOrden)
            Me.GroupBox3.Controls.Add(Me.txtNoOrden)
            Me.GroupBox3.Controls.Add(Me.txtNoCono)
            Me.GroupBox3.Controls.Add(Me.lblLine4)
            Me.GroupBox3.Controls.Add(Me.lblLine5)
            Me.GroupBox3.Controls.Add(Me.Label12)
            Me.GroupBox3.Controls.Add(Me.lblLine7)
            Me.GroupBox3.Controls.Add(Me.lblLine8)
            Me.GroupBox3.Controls.Add(Me.lblLine2)
            Me.GroupBox3.Controls.Add(Me.lblLine3)
            Me.GroupBox3.Controls.Add(Me.lblPlaca)
            Me.GroupBox3.Controls.Add(Me.lblMarca)
            Me.GroupBox3.Controls.Add(Me.lblModelo)
            Me.GroupBox3.Controls.Add(Me.lblNoOrden)
            Me.GroupBox3.Controls.Add(Me.lblNoCono)
            Me.GroupBox3.Controls.Add(Me.lblTipoOrdenO)
            Me.GroupBox3.Controls.Add(Me.lblEstadoO)
            resources.ApplyResources(Me.GroupBox3, "GroupBox3")
            Me.GroupBox3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.GroupBox3.Name = "GroupBox3"
            Me.GroupBox3.TabStop = False
            '
            'Label27
            '
            Me.Label27.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label27, "Label27")
            Me.Label27.Name = "Label27"
            '
            'txtKilometraje
            '
            Me.txtKilometraje.AceptaNegativos = False
            Me.txtKilometraje.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtKilometraje.EstiloSBO = True
            resources.ApplyResources(Me.txtKilometraje, "txtKilometraje")
            Me.txtKilometraje.ForeColor = System.Drawing.Color.Black
            Me.txtKilometraje.MaxDecimales = 0
            Me.txtKilometraje.MaxEnteros = 0
            Me.txtKilometraje.Millares = False
            Me.txtKilometraje.Name = "txtKilometraje"
            Me.txtKilometraje.ReadOnly = True
            Me.txtKilometraje.Size_AdjustableHeight = 20
            Me.txtKilometraje.TeclasDeshacer = True
            Me.txtKilometraje.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label26
            '
            Me.Label26.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label26, "Label26")
            Me.Label26.Name = "Label26"
            '
            'lblKilometraje
            '
            resources.ApplyResources(Me.lblKilometraje, "lblKilometraje")
            Me.lblKilometraje.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblKilometraje.Name = "lblKilometraje"
            '
            'txtVIN
            '
            Me.txtVIN.AceptaNegativos = False
            Me.txtVIN.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtVIN.EstiloSBO = True
            resources.ApplyResources(Me.txtVIN, "txtVIN")
            Me.txtVIN.ForeColor = System.Drawing.Color.Black
            Me.txtVIN.MaxDecimales = 0
            Me.txtVIN.MaxEnteros = 0
            Me.txtVIN.Millares = False
            Me.txtVIN.Name = "txtVIN"
            Me.txtVIN.ReadOnly = True
            Me.txtVIN.Size_AdjustableHeight = 20
            Me.txtVIN.TeclasDeshacer = True
            Me.txtVIN.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label28
            '
            Me.Label28.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label28, "Label28")
            Me.Label28.Name = "Label28"
            '
            'lblVIN
            '
            resources.ApplyResources(Me.lblVIN, "lblVIN")
            Me.lblVIN.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblVIN.Name = "lblVIN"
            '
            'cboEstadoWeb
            '
            resources.ApplyResources(Me.cboEstadoWeb, "cboEstadoWeb")
            Me.cboEstadoWeb.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboEstadoWeb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboEstadoWeb.EstiloSBO = True
            Me.cboEstadoWeb.Name = "cboEstadoWeb"
            '
            'Label25
            '
            resources.ApplyResources(Me.Label25, "Label25")
            Me.Label25.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            Me.Label25.Name = "Label25"
            '
            'txtNoVehiculo
            '
            Me.txtNoVehiculo.AceptaNegativos = False
            Me.txtNoVehiculo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoVehiculo.EstiloSBO = True
            resources.ApplyResources(Me.txtNoVehiculo, "txtNoVehiculo")
            Me.txtNoVehiculo.ForeColor = System.Drawing.Color.Black
            Me.txtNoVehiculo.MaxDecimales = 0
            Me.txtNoVehiculo.MaxEnteros = 0
            Me.txtNoVehiculo.Millares = False
            Me.txtNoVehiculo.Name = "txtNoVehiculo"
            Me.txtNoVehiculo.ReadOnly = True
            Me.txtNoVehiculo.Size_AdjustableHeight = 20
            Me.txtNoVehiculo.TeclasDeshacer = True
            Me.txtNoVehiculo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblEstadoWeb
            '
            resources.ApplyResources(Me.lblEstadoWeb, "lblEstadoWeb")
            Me.lblEstadoWeb.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblEstadoWeb.Name = "lblEstadoWeb"
            '
            'Label5
            '
            Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.Name = "Label5"
            '
            'Label6
            '
            resources.ApplyResources(Me.Label6, "Label6")
            Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label6.Name = "Label6"
            '
            'txtNoVisita
            '
            Me.txtNoVisita.AceptaNegativos = False
            Me.txtNoVisita.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoVisita.EstiloSBO = True
            resources.ApplyResources(Me.txtNoVisita, "txtNoVisita")
            Me.txtNoVisita.ForeColor = System.Drawing.Color.Black
            Me.txtNoVisita.MaxDecimales = 0
            Me.txtNoVisita.MaxEnteros = 0
            Me.txtNoVisita.Millares = False
            Me.txtNoVisita.Name = "txtNoVisita"
            Me.txtNoVisita.ReadOnly = True
            Me.txtNoVisita.Size_AdjustableHeight = 20
            Me.txtNoVisita.TeclasDeshacer = True
            Me.txtNoVisita.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblLine1
            '
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.Name = "lblLine1"
            '
            'lblNoVisita
            '
            resources.ApplyResources(Me.lblNoVisita, "lblNoVisita")
            Me.lblNoVisita.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoVisita.Name = "lblNoVisita"
            '
            'rptCalidad
            '
            Me.rptCalidad.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.rptCalidad, "rptCalidad")
            Me.rptCalidad.Name = "rptCalidad"
            Me.rptCalidad.P_Authentication = False
            Me.rptCalidad.P_BarraTitulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptCalidad.P_CompanyName = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptCalidad.P_DataBase = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptCalidad.P_Filename = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptCalidad.P_NCopias = 0
            Me.rptCalidad.P_Owner = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptCalidad.P_ParArray = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptCalidad.P_Password = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptCalidad.P_Server = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptCalidad.P_User = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptCalidad.P_WorkFolder = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'rptReprocesos
            '
            Me.rptReprocesos.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.rptReprocesos, "rptReprocesos")
            Me.rptReprocesos.Name = "rptReprocesos"
            Me.rptReprocesos.P_Authentication = False
            Me.rptReprocesos.P_BarraTitulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReprocesos.P_CompanyName = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReprocesos.P_DataBase = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReprocesos.P_Filename = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReprocesos.P_NCopias = 0
            Me.rptReprocesos.P_Owner = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReprocesos.P_ParArray = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReprocesos.P_Password = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReprocesos.P_Server = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReprocesos.P_User = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReprocesos.P_WorkFolder = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'rptSuspensiones
            '
            Me.rptSuspensiones.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.rptSuspensiones, "rptSuspensiones")
            Me.rptSuspensiones.Name = "rptSuspensiones"
            Me.rptSuspensiones.P_Authentication = False
            Me.rptSuspensiones.P_BarraTitulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptSuspensiones.P_CompanyName = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptSuspensiones.P_DataBase = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptSuspensiones.P_Filename = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptSuspensiones.P_NCopias = 0
            Me.rptSuspensiones.P_Owner = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptSuspensiones.P_ParArray = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptSuspensiones.P_Password = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptSuspensiones.P_Server = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptSuspensiones.P_User = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptSuspensiones.P_WorkFolder = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'btnAceptar
            '
            resources.ApplyResources(Me.btnAceptar, "btnAceptar")
            Me.btnAceptar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnAceptar.Name = "btnAceptar"
            '
            'VisualizarUDFOrden
            '
            resources.ApplyResources(Me.VisualizarUDFOrden, "VisualizarUDFOrden")
            Me.VisualizarUDFOrden.CampoLlave = Nothing
            Me.VisualizarUDFOrden.CodigoFormularioSBO = 0
            Me.VisualizarUDFOrden.CodigoUsuario = 0
            Me.VisualizarUDFOrden.Conexion = Nothing
            Me.VisualizarUDFOrden.Form = Nothing
            Me.VisualizarUDFOrden.Name = "VisualizarUDFOrden"
            Me.VisualizarUDFOrden.NombreBaseDatosSBO = Nothing
            Me.VisualizarUDFOrden.Tabla = Nothing
            Me.VisualizarUDFOrden.VisualizarUDFSBO = False
            Me.VisualizarUDFOrden.Where = Nothing
            '
            'btnCerrarFormulario
            '
            resources.ApplyResources(Me.btnCerrarFormulario, "btnCerrarFormulario")
            Me.btnCerrarFormulario.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCerrarFormulario.Name = "btnCerrarFormulario"
            '
            'm_bsrcOtrosGastosResumido
            '
            Me.m_bsrcOtrosGastosResumido.DataMember = "SCGTA_VW_OtrosGastosResumido"
            Me.m_bsrcOtrosGastosResumido.DataSource = Me.m_dstOtrosGastos
            '
            'm_dstOtrosGastos
            '
            Me.m_dstOtrosGastos.DataSetName = "OtrosGastosDataSet"
            Me.m_dstOtrosGastos.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'frmOrden
            '
            resources.ApplyResources(Me, "$this")
            Me.Controls.Add(Me.btnCerrarFormulario)
            Me.Controls.Add(Me.VisualizarUDFOrden)
            Me.Controls.Add(Me.rptSuspensiones)
            Me.Controls.Add(Me.rptReprocesos)
            Me.Controls.Add(Me.GroupBox3)
            Me.Controls.Add(Me.btnAceptar)
            Me.Controls.Add(Me.tabOrden)
            Me.Controls.Add(Me.rptorden)
            Me.Controls.Add(Me.rptCalidad)
            Me.KeyPreview = True
            Me.Name = "frmOrden"
            Me.tabOrden.ResumeLayout(False)
            Me.tabPrincipal.ResumeLayout(False)
            Me.tabPrincipal.PerformLayout()
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            CType(Me.dtgRampas, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpOrdenInfo.ResumeLayout(False)
            Me.grpOrdenInfo.PerformLayout()
            CType(Me.picTecnico, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabRepuestos.ResumeLayout(False)
            Me.tabRepuestos.PerformLayout()
            Me.GroupBox5.ResumeLayout(False)
            CType(Me.dtgRepuestos, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabFasesProd.ResumeLayout(False)
            Me.tabFasesProd.PerformLayout()
            Me.GroupBox2.ResumeLayout(False)
            Me.GroupBox2.PerformLayout()
            CType(Me.dtgcolaborador, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picEstado, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabActividades.ResumeLayout(False)
            Me.tabActividades.PerformLayout()
            Me.grpActividadesProduccion.ResumeLayout(False)
            Me.grpActividadesProduccion.PerformLayout()
            CType(Me.dtgActividades, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabSuministros.ResumeLayout(False)
            Me.grbSuministros.ResumeLayout(False)
            CType(Me.dtgSuministros, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabServiciosExternos.ResumeLayout(False)
            Me.grbServiciosExternos.ResumeLayout(False)
            Me.grbServiciosExternos.PerformLayout()
            CType(Me.dtgSE, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabRendimiento.ResumeLayout(False)
            CType(Me.dtgRendimientosBarras, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtgMontoReparacion, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabOtrosGastos.ResumeLayout(False)
            Me.GroupBox4.ResumeLayout(False)
            Me.FlowLayoutPanel1.ResumeLayout(False)
            Me.FlowLayoutPanel1.PerformLayout()
            Me.Panel3.ResumeLayout(False)
            Me.Panel3.PerformLayout()
            Me.GroupBox3.ResumeLayout(False)
            Me.GroupBox3.PerformLayout()
            CType(Me.m_bsrcOtrosGastosResumido, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.m_dstOtrosGastos, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region "Constructor"

        Public Sub New(ByRef p_dtsOrden As DMSOneFramework.OrdenTrabajoDataset, ByVal p_strNoOrden As String)
            MyBase.New()

            InitializeComponent()

            m_dtsOrden = p_dtsOrden
            m_strNoOrden = p_strNoOrden


        End Sub

#End Region

#Region "Declaraciones"

#Region "Formularios"

        Private WithEvents F_objfrmCtrlVisita As frmDetalleVisita
        Private WithEvents F_objfrmCtrlVehiculo As frmCtrlInformacionVehiculos
        Private WithEvents frmChild As frmCtrlSuspension
        Private WithEvents frmSuspensiones As frmSuspensiones

        Private WithEvents ObjfrmReprocesos As frmReprocesos
        Private WithEvents m_objOcupacion As frmOcupacionPatio
        Private WithEvents m_objOrdenesEspeciales As frmOrdenesEspeciales

        Private m_objClientes As frmCtrlInformacionClientes

#End Region

#Region "Constantes"

        'Principal
        Private mc_PriEstado_NoIniciada As String = My.Resources.ResourceUI.NoIniciada
        Private mc_PriEstado_Proceso As String = My.Resources.ResourceUI.Enproceso
        Private mc_PriEstado_Suspendida As String = My.Resources.ResourceUI.Suspendida
        Private mc_PriEstado_Finalizada As String = My.Resources.ResourceUI.Finalizada
        Private mc_PriEstado_Cerrada As String = My.Resources.ResourceUI.Cerrada
        Private mc_PriEstado_Cancelada As String = My.Resources.ResourceUI.Cancelada
        Private mc_PriEstado_Facturada As String = My.Resources.ResourceUI.Facturada

        Private Const mc_NumEstado_NoIniciada As String = "1"
        Private Const mc_NumEstado_Proceso As String = "2"
        Private Const mc_NumEstado_Suspendida As String = "3"
        Private Const mc_NumEstado_Finalizada As String = "4"
        Private Const mc_NumEstado_Cancelada As String = "5"
        Private Const mc_NumEstado_Cerrada As String = "6"
        Private Const mc_NumEstado_Facturada As String = "7"

        'Suministros
        Private Const mcsum_strItemCode As String = "ItemCode"
        Private Const mcsum_strItemName As String = "ItemName"
        Private Const mcsum_dblQuantity As String = "Quantity"
        Private Const mcsum_dblCantidadFinal As String = "CantidadFinal"
        Private Const mcsum_dtDocDate As String = "DocDate"
        Private Const mcsum_dblMonto As String = "Monto"
        Private Const mcsum_intEmpId As String = "EmpId"
        Private Const mcsum_strNoOrden As String = "NoOrden"
        Private Const mcsum_strNombreEmp As String = "NombreEmp"
        Private Const mcsum_intNoCentroCosto As String = "NoCentroCosto"
        Private Const mcsum_strDescCentroCosto As String = "DescCentroCosto"
        Private Const mcsum_strCentroCosto As String = "CentroCosto"

        Private Const mcsum_strTableName As String = "SCGTA_VW_Suministros"

        'Rendimientos
        Private Const mc_RenMontosRep_strTableName As String = "SCGTA_SP_SELMontoOtorgadoVsAcumulado"

        Private Const mc_RenMontosRep_strDescripcion As String = "Descripcion"
        Private Const mc_RenMontosRep_strValorOtorgado As String = "ValorOtorgado"
        Private Const mc_RenMontosRep_strValorAcumulado As String = "ValorAcumulado"
        Private Const mc_RenMontosRep_strPorcentaje As String = "Porcentaje"

        Private Const mc_RenDurXFase_strTableName As String = "SCGTA_SP_SELDuracionXFase"

        Private Const mc_RenDurXFase_strDescripcion As String = "Descripcion"
        Private Const mc_RenDurXFase_strDuracionHorasAprobadas As String = "DuracionHorasAprobadas"
        Private Const mc_RenDurXFase_strCantidadHoraManoObra As String = "CantidadHoraManoObra"
        Private Const mc_RenDurXFase_strTiempoRestante As String = "TiempoRestante"
        Private Const mc_RenDurXFase_strPorcentaje As String = "Porcentaje"

        Private Const mc_intLimiteAmarillo As Integer = 100
        Private Const mc_intLimiteVerde As Integer = 75

        Private Const mc_intIDCol As Integer = 11

        'Validaciones
        Private Const mc_FinalizaOTCantSolicitada As String = "FinalizaOTCantSolicitada"
        Private Const mc_AsignacionUnicaMO As String = "AsignacionUnicaMO"

#End Region

#Region "Variables"

        Private intTipoInsercionCol As Integer
        Private m_intNoVisita As Integer
        Private m_strNoOrden As String
        Private intTipoInsercion As Integer
        Private m_strEstado As String
        Public intNoRep As Integer
        Public strNoOrden As String
        Public intNoPieza As Integer
        Public intNoSeccion As Integer
        Private intTipoInsercionAct As Integer

        Private m_strNoOrdenAct As String
        Private intPrimaryKey As Integer

        Public strNoOrdenAct As String
        Public intNoActividadAct As Integer
        Public intNoFaseAct As Integer
        Private mcol_strNoOrden As String
        Private mcol_intNoFase As Integer
        Private mcol_intcodcolabora As Integer
        Public mcol_indicador As Integer
        Public m_bolAdicional As Boolean

        ''Estados
        Private mf_strEstado As String
        Private mo_strEstado As String

        Private m_strNoChasis As String
        Private m_strNoMotor As String
        Private m_intAnio As Integer
        Private m_intTipo As Integer

        ''Fases
        Private m_alstFases As ArrayList
        Private m_alstFasesProduccion As ArrayList

        Private m_blnAgregaAdicional As Boolean
        Private m_blnBtnCerrarOAceptar As Boolean
        Private m_strDescripcionUnidadTiempo As String
        Private m_dblValorUnidadTiempo As Double

        ''------------para Documentos Drafts---------------
        Private intNumeroCotizacion As Integer

        ''se agrego 7/12/2009***************
        Private WithEvents m_objBuscador As New Buscador.SubBuscador
        'Private m_intCodigoTecnico As Integer
        Private m_intCodigoTecnico As Nullable(Of Integer)


        Private m_adpEstadoWeb As EstadoWebDataAdapter
        Private m_dstEstadoWeb As EstadoWebDataset
        Private m_drwEstadoWeb As EstadoWebDataset.SCGTA_TB_EstadoWebRow

#End Region

#Region "Objetos"

#Region "General"

        Private objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)
        Private m_tcFecha_Compromiso As New DataGridTextBoxColumn

#End Region

#Region "Datasets"

        Private m_dtsOrden As DMSOneFramework.OrdenTrabajoDataset
        Private m_dtsVisita As DMSOneFramework.VisitaDataset

        Private m_dtsCurrentFaseXOrdenEstado As FaseXOrdenEstadosDataset

        Public m_dstSum As New SuministrosDataset

#End Region

#Region "Adapters"

        Private m_AdpSum As SCGDataAccess.SuministrosDataAdapter
        Private adpMensajeria As New MensajeriaSBOTallerDataAdapter
        Private m_adpRepuestosxOrden As RepuestosxOrdenDataAdapter
#End Region

#Region "DataRows"

        Private m_drdOrdenCurrent As DMSOneFramework.OrdenTrabajoDataset.SCGTA_TB_OrdenRow
        Private m_drdVisitaCurrent As DMSOneFramework.VisitaDataset.SCGTA_TB_VisitaRow
        Private drwSum As SuministrosDataset.SCGTA_VW_SuministrosRow

#End Region

#End Region

#Region "Eventos"

        Friend Event RetornaDatos()
        Public Event NuevaSuspension(ByVal ok As Boolean, ByVal sender As Object)

#End Region

#End Region

#Region "Procedimientos"

#Region "General"

        Private Sub ActualizarOrden()
            Dim objDataset As New DMSOneFramework.OrdenTrabajoDataset
            Dim objDA As New SCGDataAccess.OrdenTrabajoDataAdapter

            objDA.Fill(objDataset, m_intNoVisita)

            With objDataset
                If .SCGTA_TB_Orden.Rows.Count <> 0 Then
                    m_dtsOrden = objDataset

                    CargaCompletaOrden()
                End If
            End With

        End Sub

        Public Sub ActualizarTiempoReal(ByVal p_strNoCotizacion As String, ByVal p_strNumOT As String)
            Dim lo_Table As New DataTable
            Dim l_decTiempoReal As Decimal

            lo_Table = objUtilitarios.RetornaDataTable(String.Format("SELECT  SUM(TiempoHoras)AS TiempoRealHoras, IDActividad FROM [dbo].[SCGTA_TB_ControlColaborador] with (nolock) where NoOrden = '{0}' group by IDActividad", p_strNumOT))

            For Each loRowTiempo As DataRow In lo_Table.Rows
                With loRowTiempo


                    If Not String.IsNullOrEmpty(.Item("TiempoRealHoras")) Then
                        l_decTiempoReal = .Item("TiempoRealHoras")
                        l_decTiempoReal = l_decTiempoReal * 60

                    Else
                        l_decTiempoReal = 0
                    End If

                    Call DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.ActualizarTiempoReal(p_strNoCotizacion, .Item("IDActividad"), l_decTiempoReal)
                End With
            Next
        End Sub


        'Actualiza el costo de los servicios por OT's
        'Dependiendo de la configuracion sobre costo Estimado o Real
        Public Sub ActualizarCosto(ByVal NoCotizacion As String, ByVal NoOT As String)

            'Dim drdColaboradorDV As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorRow

            Dim dt As New DataTable

            dt = objUtilitarios.RetornaDataTable("SELECT IDActividad AS ID,SUM([Costo]) AS COSTO, SUM([CostoEstandar]) AS COSTOESTANDAR FROM [dbo].[SCGTA_TB_ControlColaborador] with (nolock)  WHERE NoOrden =  '" & NoOT & "' GROUP BY IDActividad ")

            For Each dr As DataRow In dt.Rows

                Dim strCosto As String
                Dim strCostoEst As String
                If Not IsDBNull(dr("COSTO")) Then
                    If Not String.IsNullOrEmpty(dr("COSTO")) Then
                        strCosto = dr("COSTO")
                    Else
                        strCosto = 0
                    End If
                Else
                    strCosto = 0
                End If

                If Not IsDBNull(dr("COSTOESTANDAR")) Then
                    If Not String.IsNullOrEmpty(dr("COSTOESTANDAR")) Then
                        strCostoEst = dr("COSTOESTANDAR")
                    Else
                        strCostoEst = 0
                    End If
                Else
                    strCostoEst = 0
                End If


                Call DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.ActualizarCostosServicios(NoCotizacion, dr("ID"), strCosto, strCostoEst)
            Next

        End Sub

        ''' <summary>
        ''' 
        ''' Valida Requisiciones Pendiente
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ValidaRequisicionPendiente() As Boolean

            Dim strConsultaRequiPend As String

            strConsultaRequiPend = Utilitarios.EjecutarConsulta(String.Format(" Select COUNT(LQ.U_SCGD_CodEst) from [@SCGD_REQUISICIONES] as RQ with (nolock) " +
                                                                              " inner join [@SCGD_LINEAS_REQ] AS LQ with (nolock) on RQ.DocEntry = LQ.DocEntry " +
                                                                              " where U_SCGD_NoOrden = '{0}'and LQ.U_SCGD_CodEst <> 2  and LQ.U_SCGD_CodEst <> 3 ", _txtNoOrden.Text), strConexionSBO)

            If strConsultaRequiPend > 0 Then
                Return False
            End If

            Return True


        End Function


        ''' <summary>
        ''' Valida Repuestos Aprobados y Comprados
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ValidaRepuestosCompraRecibidos() As Boolean

            Dim strConsultaCompraRepuestos As String


            strConsultaCompraRepuestos = Utilitarios.EjecutarConsulta(String.Format(" Select Count(U_SCGD_CRec) From QUT1 as qu with(nolock) " +
                                                                                    " inner join OQUT as oq with(nolock) on qu.DocEntry = oq.DocEntry " +
                                                                                    " where qu.U_SCGD_NoOT = '{0}' and qu.U_SCGD_CRec <> 0 and oq.U_SCGD_idSucursal = '{1}' and qu.U_SCGD_Compra = 'Y' and qu.U_SCGD_Aprobado = '1' ", _txtNoOrden.Text, G_strIDSucursal), strConexionSBO)

            If strConsultaCompraRepuestos > 0 Then
                Return False
            End If

            Return True


        End Function

        Private Function ValidaEstadoLineas(ByVal p_intNumeroCotizacion As Integer, ByVal p_blnValidaFinalizaOTCantSolicitada As Boolean) As Boolean
            Dim l_strSQL As String
            Dim l_strValidadEntrega As String
            Dim strConsulta As String
            Dim dtConsulta As System.Data.DataTable
            Dim blnConsulta As Boolean = True

            l_strSQL = "Select U_Entrega_Rep FROM SCGTA_VW_CONF_SUCURSAL with (nolock) " +
                        " WHERE U_Sucurs = (select U_SCGD_idSucursal from dbo.SCGTA_VW_OQUT with (nolock) where Docentry = '{0}') "

            l_strValidadEntrega = Utilitarios.EjecutarConsulta(
                    String.Format(l_strSQL, p_intNumeroCotizacion), strConexionADO)

            If Not String.IsNullOrEmpty(l_strValidadEntrega) AndAlso
                l_strValidadEntrega = "Y" Then
                strConsulta = " SELECT SUM(PT) AS PT, SUM(PA) AS PA, SUM(PE) AS PE " & _
                                " FROM( " & _
                                " SELECT COUNT(1) AS PT ,0 AS PA, 0 AS PE FROM QUT1 with (nolock) " & _
                                " WHERE (DocEntry = {0} AND " & _
                                " ((U_SCGD_Traslad IN (3,4) AND U_SCGD_Aprobado = 1) OR (U_SCGD_Aprobado =1 AND " & _
                                " U_SCGD_Compra ='Y' AND ISNULL(U_SCGD_CRec,0) <> Quantity ))) " & _
                                " UNION ALL " & _
                                " SELECT 0 AS PT ,COUNT(1) AS PA, 0 AS PE FROM QUT1 with (nolock) " & _
                                " WHERE (DocEntry = {0} And U_SCGD_Aprobado = 3) " & _
                                " UNION ALL " & _
                                " SELECT 0 AS PT,0 AS PA, COUNT(1) AS PE FROM QUT1 WITH (nolock) " & _
                                " INNER JOIN OITM WITH (nolock) ON QUT1.ItemCode = OITM.ItemCode " & _
                                " WHERE OITM.U_SCGD_TipoArticulo IN (1,3) AND QUT1.U_SCGD_Entregado = 'N' " & _
                                " AND QUT1.DocEntry = {0} AND QUT1.U_SCGD_Aprobado = 1 AND U_SCGD_Compra= 'N' " & _
                                " ) AS TB "
            Else
                strConsulta = " SELECT SUM(PT) AS PT, SUM(PA) AS PA, SUM(PE) AS PE " & _
                                " FROM ( " & _
                                " SELECT COUNT(1) AS PT ,0 AS PA, 0 AS PE FROM QUT1 with (nolock) " & _
                                " WHERE (DocEntry = {0} AND " & _
                                " ((U_SCGD_Traslad IN (3,4) AND U_SCGD_Aprobado = 1) OR (U_SCGD_Aprobado =1 AND " & _
                                " U_SCGD_Compra ='Y' AND ISNULL(U_SCGD_CRec,0) <> Quantity ))) " & _
                                " UNION ALL " & _
                                " SELECT 0 AS PT ,COUNT(1) AS PA, 0 AS PE FROM QUT1 with (nolock) " & _
                                " WHERE (DocEntry = {0} And U_SCGD_Aprobado = 3) " & _
                                " ) AS TB "

            End If
            dtConsulta = Utilitarios.EjecutarConsultaDataTable(String.Format(strConsulta, p_intNumeroCotizacion), strConexionSBO)
            If dtConsulta.Rows(0).Item("PT") > 0 Then
                If p_blnValidaFinalizaOTCantSolicitada Then
                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeNoPuedeFinalizarOTXRepPendientes)
                    blnConsulta = False
                End If
            ElseIf dtConsulta.Rows(0).Item("PA") > 0 Then
                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeNoPuedeFinalizarFaltaAprobacion)
                blnConsulta = False
            ElseIf dtConsulta.Rows(0).Item("PE") > 0 Then
                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeArticulosPendientesEntregar)
                blnConsulta = False
            End If
            Return blnConsulta
        End Function

        Private Function ValidaKITOT(p_docEntry As Integer) As Boolean
            Dim strConsulta As String = " SELECT Count(1) FROM QUT1 WITH (nolock) WHERE DocEntry = {0} AND TreeType = 'S' "
            If Utilitarios.EjecutarConsulta(String.Format(strConsulta, p_docEntry), strConexionSBO) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function ActualizarOrdenTrabajo() As Boolean

            Dim intResultVerific As Integer
            Dim drdOrden As OrdenTrabajoDataset.SCGTA_TB_OrdenRow
            Dim objAdapter As SCGDataAccess.OrdenTrabajoDataAdapter
            Dim strMensajeError As String = ""
            Dim strNoSerie As String
            Dim strNoCita As String
            Dim strValorCancelarCita As String
            Dim strDocEntryCita As String

            Dim blnFinalizarOrden As Boolean
            Dim blnSuspenderOrden As Boolean
            Dim blnCancelarOrden As Boolean
            Dim intUpdateResult As Integer = 0
            Dim objDARepuestos As RepuestosxOrdenDataAdapter
            Dim objDASuministros As SuministrosDataAdapter
            Dim blnOmitirFinalizar As Boolean = False



            Dim adpConf As New ConfiguracionDataAdapter
            Dim dstConf As New ConfiguracionDataSet
            Dim blnValidaFinalizaOTCantSolicitada As Boolean = False
            adpConf.Fill(dstConf)

            If ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracionValorBooleano(dstConf.SCGTA_TB_Configuracion, mc_FinalizaOTCantSolicitada, blnValidaFinalizaOTCantSolicitada) Then
                blnValidaFinalizaOTCantSolicitada = True
            End If


            intResultVerific = VerificarCambiaEstado()
            blnFinalizarOrden = False

            'Actualiza las bodegas de los repuestos
            objDARepuestos = New RepuestosxOrdenDataAdapter
            objDASuministros = New SuministrosDataAdapter
            objDARepuestos.UpdateBodega(m_dstRep.SCGTA_TB_RepuestosxOrden)
            objDARepuestos.UpdateBodega(m_dstServiciosExternos.SCGTA_TB_RepuestosxOrden)
            objDASuministros.UpdateBodega(m_dstSuministros.SCGTA_VW_Suministros)

            If intResultVerific >= 0 Then

                ''se agrega esta validacion para verifica que la OT no sea finalizada si tiene 
                ''lineas con estado 'Pendiente Bodega
                '-----------------------------------------------------------------------------------------------------------
                If cboEstadoOrden.SelectedValue = mc_PriEstado_Finalizada Then

                    'verifica que al menos una linea de la cotizacion este con estado PendienteBodega
                    If Not ValidaEstadoLineas(intNumeroCotizacion, blnValidaFinalizaOTCantSolicitada) Then
                        Exit Function
                    End If
                    If ValidaKITOT(intNumeroCotizacion) Then
                        Dim objVerificarCotizacionCLS As New CotizacionCLS(G_objCompany)
                        'verifica que al menos una linea de la cotizacion este en Falta de Aprobacion o Aprobado No
                        If Not objVerificarCotizacionCLS.VerificarFilasCotizacionEnFaltaAprobacionOAprobadoNoKits(intNumeroCotizacion) Then
                            objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeNoFinalizarOTKit)
                            Exit Function
                        End If
                    End If
                End If


                'Agregado 29/02/2012: Agregar validación de tiempo estándar
                'Autor: José Soto

                If objUtilitarios.TraerValorTiempo() = True Then
                    Dim objDA As New DMSOneFramework.SCGDataAccess.ActividadesXFaseDataAdapter
                    Dim drdActividadesDV As DMSOneFramework.ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow

                    If cboEstadoOrden.SelectedValue = mc_PriEstado_Finalizada Then
                        For Each drdActividadesDV In CType(dtgActividades.DataSource, DataView).Table.Rows

                            If drdActividadesDV.Duracion <= 0 Then
                                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeValidarTiempoEstandar + ": " + drdActividadesDV.ItemName)
                                Exit Function
                            End If

                        Next

                    End If

                End If


                If ActualizaFasesYColab(strMensajeError) Then

                    'Agregado 10/08/06. Alejandra. Al finalizar la orden desde el tab Principal,
                    ' debe finalizar las fases de produccion que tienen estado "En Proceso" o "Suspendida"
                    If (m_drdOrdenCurrent.Estado <> mc_NumEstado_Finalizada And m_drdOrdenCurrent.Estado <> mc_NumEstado_Cancelada And cboEstadoOrden.SelectedValue = mc_PriEstado_Finalizada) Then
                        If objSCGMSGBox.msgPregunta(My.Resources.ResourceUI.PreguntaDeseaFinalizarOT) = MsgBoxResult.Yes Then

                            If ValidarDatosSAP() Then 'En la siguiente llamada a la funcion se calcularán costos, por lo tanto debe validar algunos campos antes
                                blnFinalizarOrden = FinalizarTodasFasesOrden(False)
                            Else
                                blnFinalizarOrden = False
                            End If

                        End If

                    ElseIf (m_drdOrdenCurrent.Estado <> mc_NumEstado_Finalizada And m_drdOrdenCurrent.Estado <> mc_NumEstado_Cancelada And cboEstadoOrden.SelectedValue = mc_PriEstado_Cancelada) Then
                        If objSCGMSGBox.msgPregunta(My.Resources.ResourceUI.PreguntaDeseaCancelarOT) = MsgBoxResult.Yes Then

                            If ValidarDatosSAP() Then 'En la siguiente llamada a la funcion se calcularán costos, por lo tanto debe validar algunos campos antes
                                blnCancelarOrden = FinalizarTodasFasesOrden(True)
                            Else
                                blnCancelarOrden = False
                            End If

                        End If

                    ElseIf (m_drdOrdenCurrent.Estado <> mc_NumEstado_Finalizada And m_drdOrdenCurrent.Estado <> mc_NumEstado_Cancelada And cboEstadoOrden.SelectedValue = mc_PriEstado_Suspendida) Then
                        If objSCGMSGBox.msgPregunta(My.Resources.ResourceUI.PreguntaDeseaSuspenderOT) = MsgBoxResult.Yes Then

                            If ValidarDatosSAP() Then 'En la siguiente llamada a la funcion se calcularán costos, por lo tanto debe validar algunos campos antes
                                blnSuspenderOrden = SuspenderTodasFasesOrden()
                            Else
                                blnSuspenderOrden = False
                            End If

                        End If
                    End If


                    With m_dtsOrden

                        drdOrden = .SCGTA_TB_Orden.FindByNoOrden(m_strNoOrden)

                        If Not IsNothing(drdOrden) Then

                            With drdOrden
                                'Actualizar Estado Web
                                If Not String.IsNullOrEmpty(cboEstadoWeb.SelectedItem) Then
                                    drdOrden.IDEstadoWeb = CInt(Busca_Codigo_Texto(cboEstadoWeb.SelectedItem, True))
                                End If
                                'validacion para actualizacion de duraciones en las lineas de la cotizacion
                                If cboEstadoOrden.SelectedValue <> mc_PriEstado_Finalizada Then
                                    ActualizaDuracion(m_dstAct.SCGTA_TB_ActividadesxOrden, drdOrden.NoCotizacion)
                                End If

                                If cboEstadoOrden.SelectedValue = mc_PriEstado_Cancelada Then
                                    If ValidaOrdenLigadaACita(drdOrden.NoOrden, strNoSerie, strNoCita, strValorCancelarCita, strDocEntryCita) Then
                                        CancelarCita(strNoSerie, strNoCita, strValorCancelarCita, strDocEntryCita)
                                    End If

                                End If


                                '.Fecha_compromiso = dtpCompromiso.Value
                                .Observacion = txtObservacionesOrden.Text
                                DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.ActualizarObservacionCotizacion(drdOrden.NoCotizacion, txtObservacionesOrden.Text)
                                If (cboEstadoOrden.SelectedValue <> mc_PriEstado_Finalizada) And (cboEstadoOrden.SelectedValue <> mc_PriEstado_Suspendida) And (cboEstadoOrden.SelectedValue <> mc_PriEstado_Cancelada) Then
                                    .Estado = cboEstadoOrden.SelectedIndex + 1
                                    .EstadoDesc = cboEstadoOrden.SelectedValue
                                    .DescipcionEstado = GlobalesUI.CargarEstadoOTResources(cboEstadoOrden.SelectedValue)
                                    .SetFecha_cierreNull()

                                    '''''''''''''''''''''''''''''''''''
                                    If m_intCodigoTecnico Is Nothing Then
                                        .SetCodTecnicoNull()
                                    Else
                                        .CodTecnico = m_intCodigoTecnico
                                    End If

                                    DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.ActualizarEstadoCotizacion(drdOrden.NoCotizacion, cboEstadoOrden.SelectedValue)


                                    ActualizarCosto(.NoCotizacion, .NoOrden)
                                    ActualizarTiempoReal(.NoCotizacion, .NoOrden)

                                ElseIf (cboEstadoOrden.SelectedValue = mc_PriEstado_Finalizada And blnFinalizarOrden) Then
                                    .Estado = cboEstadoOrden.SelectedIndex + 1
                                    .EstadoDesc = cboEstadoOrden.SelectedValue
                                    .DescipcionEstado = GlobalesUI.CargarEstadoOTResources(cboEstadoOrden.SelectedValue)
                                    .Fecha_cierre = objUtilitarios.CargarFechaHoraServidor

                                    blnFinalizarOrden = True

                                    If DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.ActualizarEstadoCotizacion(drdOrden.NoCotizacion, cboEstadoOrden.SelectedValue) <> 0 Then
                                        blnOmitirFinalizar = True
                                    End If

                                    If DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.Actualiza_ValorOTFin_LineasCotizacion(drdOrden.NoCotizacion) <> 0 Then
                                        blnOmitirFinalizar = True
                                    End If

                                    ActualizarCosto(.NoCotizacion, .NoOrden)
                                    ActualizarTiempoReal(.NoCotizacion, .NoOrden)

                                    'Genera mensaje en SBO para el asesor
                                    adpMensajeria.CreaMensajeDMS_SBO_Cotizacion(My.Resources.ResourceUI.MensajeLaOTHaSidoFinalizada, _
                                         My.Resources.ResourceUI.Finalizada, MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)

                                ElseIf cboEstadoOrden.SelectedValue = mc_PriEstado_Cancelada And blnCancelarOrden Then
                                    .Estado = cboEstadoOrden.SelectedIndex + 1
                                    .EstadoDesc = cboEstadoOrden.SelectedValue
                                    .DescipcionEstado = GlobalesUI.CargarEstadoOTResources(cboEstadoOrden.SelectedValue)
                                    .Fecha_cierre = objUtilitarios.CargarFechaHoraServidor
                                    blnFinalizarOrden = True

                                    Try
                                        ' Inicia la Transaccion que se encarga de Ejecutar todos los procesos en SBO (Cancelar la OT y realizar las Transferencias de Stock)
                                        DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.IniciaTransaccion()

                                        DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.ActualizarEstadoCotizacion(drdOrden.NoCotizacion, cboEstadoOrden.SelectedValue, True)

                                        ActualizarCosto(.NoCotizacion, .NoOrden)
                                        ActualizarTiempoReal(.NoCotizacion, .NoOrden)
                                        RealizarTransferenciasStock(m_strNoOrden, drdOrden.NoCotizacion)
                                        adpMensajeria.CreaMensajeDMS_SBO_Cotizacion(My.Resources.ResourceUI.MensajeLaOTHaSidoCancelada, _
                                            My.Resources.ResourceUI.Cancelada, MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)

                                        DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Commit)


                                    Catch exep As Exception
                                        DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                                    End Try


                                ElseIf cboEstadoOrden.SelectedValue = mc_PriEstado_Suspendida And blnSuspenderOrden Then
                                    .Estado = cboEstadoOrden.SelectedIndex + 1
                                    .EstadoDesc = cboEstadoOrden.SelectedValue
                                    .DescipcionEstado = GlobalesUI.CargarEstadoOTResources(cboEstadoOrden.SelectedValue)
                                    .SetFecha_cierreNull()
                                    blnFinalizarOrden = False

                                    DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.ActualizarEstadoCotizacion(drdOrden.NoCotizacion, cboEstadoOrden.SelectedValue)

                                    ActualizarCosto(.NoCotizacion, .NoOrden)
                                    ActualizarTiempoReal(.NoCotizacion, .NoOrden)

                                    'Genera mensaje en SBO para el asesor
                                    adpMensajeria.CreaMensajeDMS_SBO_Cotizacion(My.Resources.ResourceUI.MensajeLaOTHaSidoSuspendida, _
                                        My.Resources.ResourceUI.Suspendida, MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)
                                End If 'cboEstado <> Finalizada

                            End With

                            If Not blnOmitirFinalizar Then

                                objAdapter = New SCGDataAccess.OrdenTrabajoDataAdapter

                                intUpdateResult = objAdapter.Actualizar(m_dtsOrden)

                            End If

                            If blnFinalizarOrden AndAlso Not String.IsNullOrEmpty(drdOrden.IDVehiculo) Then

                                Dim adpVehiculo As SCGTA_VW_Vehiculos2TableAdapter = New SCGTA_VW_Vehiculos2TableAdapter()
                                adpVehiculo.CadenaConexion = strConexionADO
                                adpVehiculo.ActualizaFechaUltimoServicio(drdOrden.Fecha_cierre, drdOrden.NoVehiculo)

                            End If

                            If g_blnCosteaActividades AndAlso blnFinalizarOrden AndAlso intUpdateResult <> 0 Then

                                If objUtilitarios.TraerTipoCosto Then
                                    CalculoCostosCierreOrden(drdOrden.NoOrden, drdOrden.NoCotizacion, m_intTipo)
                                End If

                            End If

                        End If

                    End With

                    If Not blnFinalizarOrden Then
                        Return False
                    Else
                        Return True
                    End If



                Else

                    objSCGMSGBox.msgExclamationCustom(strMensajeError)

                    Return False

                End If

            Else

                Select Case intResultVerific

                    Case -1
                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeNoSePuedeCambiarEstadoaNoIniciada)
                    Case -2, -3, -4, -5
                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeNoSePuedeModificarEstadoOTFinalizada)

                End Select


                Return False

            End If

        End Function

        Private Sub CargaCompletaOrden()

            If Not IsNothing(m_dtsOrden) Then
                m_drdOrdenCurrent = m_dtsOrden.SCGTA_TB_Orden.FindByNoOrden(m_strNoOrden)

                'utilizo esta variable para guardar el Numero de la cotizacion actual
                intNumeroCotizacion = m_drdOrdenCurrent.NoCotizacion

                If Not IsNothing(m_drdOrdenCurrent) Then
                    m_intNoVisita = m_drdOrdenCurrent.NoVisita
                Else
                    m_intNoVisita = 0
                End If

            End If
            m_intTipo = m_drdOrdenCurrent.CodTipoOrden
            If Not IsNothing(m_drdOrdenCurrent) Then

                Dim kilometraje As Integer

                If m_drdOrdenCurrent.IsKilometrajeNull() Then
                    kilometraje = 0
                Else
                    kilometraje = m_drdOrdenCurrent.Kilometraje
                End If


                If m_drdOrdenCurrent.Estado = "6" Or
                    m_drdOrdenCurrent.Estado = "7" Or
                    m_drdOrdenCurrent.Estado = "8" Then
                    'carga unicamente los estados que se pueden seleccionar
                    'carga todos los estados para las ot
                    cargarComboEstadoOrden(cboEstadoOrden, True)
                Else
                    'por el usuario para la ot
                    cargarComboEstadoOrden(cboEstadoOrden, False)
                End If


                CargarVisitas()

                objUtilitarios.CargarCombos(cboFasesProdF, 1)
                objUtilitarios.CargarCombos(cboEstadoRep2, 14)
                objUtilitarios.CargarCombos(cboEstadoRep, 14)
                objUtilitarios.CargarCombos(cbEstadoSE, 29)

                objUtilitarios.CargarCombos(cboEstadoWeb, 1)

                cboFasesProdF.Items.Insert(0, My.Resources.ResourceUI.Todos & Space(100) & "0")
                cboFasesProdF.Text = cboFasesProdF.Items.Item(0)

                cboEstadoRep2.Items.Insert(0, My.Resources.ResourceUI.Todos & Space(100) & "0")
                cboEstadoRep2.Text = cboEstadoRep2.Items.Item(0)

                cbEstadoSE.Items.Insert(0, My.Resources.ResourceUI.Todos & Space(100) & "0")
                cbEstadoSE.Text = cbEstadoSE.Items.Item(0)



                CargarDatosOrden(m_strNoChasis, m_strNoMotor, m_intAnio, kilometraje)
                CargarDatosPrincipal()

                objUtilitarios.CargarComboFaseXOrden(cboFases_Producción, txtNoOrden.Text)

                'Cargar Estado Web
                cargarEstadoWeb()
                'Establece lel estado web
                If Not m_drdOrdenCurrent.IsIDEstadoWebNull Then
                    Busca_Item_Combo(cboEstadoWeb, m_drdOrdenCurrent.IDEstadoWeb)
                End If

            Else
                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeErrorCargarInfoOT)
            End If

        End Sub

        Private Sub CargarVisitas()
            Dim objDA As New DMSOneFramework.SCGDataAccess.VisitasDataAdapter

            m_dtsVisita = New DMSOneFramework.VisitaDataset
            'objDA.Fill(m_dtsVisita, Nothing, Nothing, CStr(m_intNoVisita), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            objDA.Fill(m_dtsVisita, Nothing, Nothing, m_intNoVisita, Nothing, Nothing, Nothing, Nothing, Nothing, _
                            Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If m_dtsVisita.SCGTA_TB_Visita.Rows.Count <> 0 Then
                m_drdVisitaCurrent = m_dtsVisita.SCGTA_TB_Visita.FindByNoVisita(m_intNoVisita)
            End If
        End Sub

        Private Sub CargarDatosOrden(ByRef p_NoChasis As String, _
                                     ByRef p_Nomotor As String, _
                                     ByRef p_Anio As Integer, _
                                     ByRef p_Kilometraje As Integer)

            Dim objDA As New DMSOneFramework.SCGDataAccess.VehiculosDataAdapter
            Dim dstVehiculo As New DMSOneFramework.VehiculosDataset
            Dim drdVehiculoCurrent As DMSOneFramework.VehiculosDataset.SCGTA_VW_VehiculosRow = Nothing

            'Estados
            Dim objDAOQUT As New DMSOneFramework.SCGDataAccess.OQUTDataAdapter
            Dim dstOQUT As New DMSOneFramework.OrdenEspecialDataset
            Dim drdOQUTCurrent As DMSOneFramework.OrdenEspecialDataset.SCGTA_TB_OrdenRow = Nothing

            objDA.Fill(dstVehiculo, m_drdVisitaCurrent.IDVehiculo)

            objDAOQUT.Fill(dstOQUT, m_drdOrdenCurrent.NoCotizacion)

            If dstVehiculo.SCGTA_VW_Vehiculos.Rows.Count <> 0 Then
                drdVehiculoCurrent = dstVehiculo.SCGTA_VW_Vehiculos.Rows(0)

                If drdVehiculoCurrent.VIN IsNot DBNull.Value Then
                    p_NoChasis = drdVehiculoCurrent.VIN
                End If
                If drdVehiculoCurrent.Num_Motor IsNot DBNull.Value Then
                    p_Nomotor = drdVehiculoCurrent.Num_Motor
                End If
                If Not drdVehiculoCurrent.IsAnoVehiculoNull Then
                    If drdVehiculoCurrent.AnoVehiculo <> "" Then
                        p_Anio = drdVehiculoCurrent.AnoVehiculo
                    End If
                End If

            End If

            If dstOQUT.SCGTA_TB_Orden.Rows.Count <> 0 Then
                drdOQUTCurrent = dstOQUT.SCGTA_TB_Orden.Rows(0)
            End If

            If Not IsNothing(drdVehiculoCurrent) Then

                With drdVehiculoCurrent
                    Me.txtNoVehiculo.Text = IIf(m_drdOrdenCurrent.Item("NoVehiculo") Is DBNull.Value, "", m_drdOrdenCurrent.Item("NoVehiculo"))
                    Me.txtNoVisita.Text = IIf(m_drdOrdenCurrent.Item("NoVisita") Is DBNull.Value, "", m_drdOrdenCurrent.Item("NoVisita"))
                    Me.txtMarca.Text = IIf(.Item("DescMarca") Is DBNull.Value, "", .Item("DescMarca"))
                    Me.txtEstilo.Text = IIf(.Item("DescEstilo") Is DBNull.Value, "", .Item("DescEstilo"))
                    Me.txtPlaca.Text = IIf(.Item("Placa") Is DBNull.Value, "", .Item("Placa"))
                    Me.txtNoCono.Text = IIf(m_drdOrdenCurrent.Item("Cono") Is DBNull.Value, "", m_drdOrdenCurrent.Item("Cono"))
                    Me.txtNoOrden.Text = m_drdOrdenCurrent.NoOrden
                    Me.txtVIN.Text = IIf(.Item("VIN") Is DBNull.Value, "", .Item("VIN"))
                    Me.txtKilometraje.Text = p_Kilometraje

                    If Not IsNothing(drdOQUTCurrent) Then
                        'se pintal los valores de las fechas en pantalla
                        If Not drdOQUTCurrent.Item("FCierre") Is DBNull.Value Then
                            Me.txtFCerrado.Text = drdOQUTCurrent.Item("FCierre")
                        End If
                        If Not drdOQUTCurrent.Item("FFact") Is DBNull.Value Then
                            Me.txtFFacturado.Text = drdOQUTCurrent.Item("FFact")
                        End If
                        If Not drdOQUTCurrent.Item("FEnt") Is DBNull.Value Then
                            Me.txtFEntregado.Text = drdOQUTCurrent.Item("FEnt")
                        End If

                    End If
                End With
            End If

        End Sub

        Private Sub CargarDatosPrincipal()

            Dim strHora As String
            Dim strMinutos As String
            Dim datHora As Date
            With m_drdOrdenCurrent

                If .Item("Fecha_apertura") Is DBNull.Value Then
                    txtfechaapertura.Text = ""
                Else
                    txtfechaapertura.Text = CType(.Item("Fecha_apertura"), Date).ToShortDateString
                    txtHoraApert.Text = CType(.Item("Fecha_apertura"), Date).ToShortTimeString
                End If
                If .Item("Fecha_Comp") Is DBNull.Value Then
                    If .Item("Fecha_apertura") Is DBNull.Value Then
                        txtFechaComp.Text = ""
                    Else
                        txtFechaComp.Text = CType(.Item("Fecha_apertura"), Date).ToShortDateString
                    End If

                Else
                    txtFechaComp.Text = CType(.Item("Fecha_Comp"), Date).ToShortDateString
                End If
                If .Item("Hora_Comp") Is DBNull.Value Then
                    txtFechaComp.Text = ""
                Else
                    If CInt(.Item("Hora_Comp")) > 0 Then

                        If CStr(.Item("Hora_Comp")).Length <= 2 Then
                            strHora = 0
                            strMinutos = .Item("Hora_Comp")
                            If String.IsNullOrEmpty(strMinutos) Then
                                strMinutos = 0
                            End If
                        ElseIf CStr(.Item("Hora_Comp")).Length = 3 Then
                            strHora = .Item("Hora_Comp").ToString.Substring(0, 1)
                            strMinutos = .Item("Hora_Comp").ToString.Substring(1, 2)
                            If String.IsNullOrEmpty(strMinutos) Then
                                strMinutos = 0
                            End If
                        Else
                            strHora = .Item("Hora_Comp").ToString.Substring(0, 2)
                            strMinutos = .Item("Hora_Comp").ToString.Substring(2, 2)
                            If String.IsNullOrEmpty(strMinutos) Then
                                strMinutos = 0
                            End If
                        End If

                        datHora = New Date(Now.Year, Now.Month, Now.Day, strHora, strMinutos, 0)

                        txtHoraComp.Text = datHora.ToShortTimeString
                    Else
                        txtFechaComp.Text = ""
                    End If
                End If
                If .Item("Fecha_cierre") Is DBNull.Value Then
                    txtfechacierre.Text = ""
                    txtfechafinalizacion.Text = ""
                Else
                    txtfechacierre.Text = CType(.Item("Fecha_cierre"), Date).ToShortTimeString
                    txtfechafinalizacion.Text = CType(.Item("Fecha_cierre"), Date).ToShortDateString()
                End If


                Select Case m_drdOrdenCurrent.Item("EstadoDesc")

                    Case My.Resources.ResourceUI.NoIniciada
                        cboEstadoOrden.Text = My.Resources.ResourceUI.NoIniciada
                    Case My.Resources.ResourceUI.Enproceso
                        cboEstadoOrden.Text = My.Resources.ResourceUI.Enproceso
                    Case My.Resources.ResourceUI.Finalizada
                        cboEstadoOrden.Text = My.Resources.ResourceUI.Finalizada
                    Case My.Resources.ResourceUI.Suspendida
                        cboEstadoOrden.Text = My.Resources.ResourceUI.Suspendida
                    Case My.Resources.ResourceUI.Cancelada
                        cboEstadoOrden.Text = My.Resources.ResourceUI.Cancelada
                    Case My.Resources.ResourceUI.Cerrada
                        cboEstadoOrden.Text = My.Resources.ResourceUI.Cerrada
                        cboEstadoOrden.Enabled = False
                    Case My.Resources.ResourceUI.Facturada
                        cboEstadoOrden.Text = My.Resources.ResourceUI.Facturada
                        cboEstadoOrden.Enabled = False
                    Case My.Resources.ResourceUI.Entregada
                        cboEstadoOrden.Text = My.Resources.ResourceUI.Entregada
                        cboEstadoOrden.Enabled = False
                    Case Else
                        cboEstadoOrden.Text = ""
                End Select

                If Not m_drdOrdenCurrent.IsNombreAsesorNull AndAlso Not String.IsNullOrEmpty(m_drdOrdenCurrent.NombreAsesor.Trim) Then
                    txtresponsable.Text = m_drdOrdenCurrent.NombreAsesor
                Else
                    txtresponsable.Text = IIf(m_drdVisitaCurrent.Item("AsesorNombre") Is DBNull.Value, "", m_drdVisitaCurrent.Item("AsesorNombre"))
                End If
                txtObservacionesOrden.Text = IIf(.Item("Observacion") Is DBNull.Value, "", .Item("Observacion"))
                txtTipoOrden.Text = IIf(.Item("TipoDesc") Is DBNull.Value, "", .Item("TipoDesc"))


                If Not .Item("CodTecnico") Is DBNull.Value AndAlso Not m_drdOrdenCurrent.CodTecnico = 0 Then

                    m_intCodigoTecnico = m_drdOrdenCurrent.CodTecnico

                    If m_drdOrdenCurrent.IsDescripcionTecnicoNull Then
                        'atualiza el campo fechasync de los items de la orden en dms
                        Dim ordenAdapter As OrdenTrabajoDataAdapter = New OrdenTrabajoDataAdapter
                        txtTecnico.Text = ordenAdapter.SelCodigoTecnico(m_intCodigoTecnico)
                    Else
                        txtTecnico.Text = m_drdOrdenCurrent.DescripcionTecnico
                    End If
                Else
                    txtTecnico.Text = ""
                End If

            End With
        End Sub

        Private Sub OrdenarTabs()

            Me.tabOrden.Controls.Clear()

            Me.tabOrden.Controls.Add(Me.tabPrincipal)

            If g_blnUsaServicios Then

                Me.tabOrden.Controls.Add(Me.tabFasesProd)
            End If

            If g_blnUsaRepuestos Then
                Me.tabOrden.Controls.Add(Me.tabRepuestos)
            End If


            If g_blnUsaServicios Then
                Me.tabOrden.Controls.Add(Me.tabActividades)
            End If

            If g_blnUsaSuministros Then
                Me.tabOrden.Controls.Add(Me.tabSuministros)
            End If

            If g_blnUsaServiciosExternos Then
                Me.tabOrden.Controls.Add(Me.tabServiciosExternos)
            End If

            If g_blnUsaOtrosGastos Then Me.tabOrden.Controls.Add(Me.tabOtrosGastos)

            If g_blnUsaServicios Then
                Me.tabOrden.Controls.Add(Me.tabRendimiento)
            End If

        End Sub

        Private Function ValidarFormularios(ByVal p_strNombreFrm As String) As System.Windows.Forms.Form
            Dim frmTemp As System.Windows.Forms.Form

            For Each frmTemp In Me.MdiParent.MdiChildren
                If frmTemp.Name = p_strNombreFrm Then
                    Return frmTemp
                    Exit Function
                End If
            Next frmTemp

            Return Nothing

        End Function

        Private Function BuscarTipoOrdenDescripcion(ByVal intCodTipo As Integer) As String
            Dim objDA As New DMSOneFramework.SCGDataAccess.TipoOrdenDataAdapter
            Dim dtsTiposOrden As New DMSOneFramework.TipoOrdenDataset
            Dim drdCurrentRow As DMSOneFramework.TipoOrdenDataset.SCGTA_TB_TipoOrdenRow = Nothing

            objDA.Fill(dtsTiposOrden)

            If dtsTiposOrden.SCGTA_TB_TipoOrden.Rows.Count <> 0 Then
                drdCurrentRow = dtsTiposOrden.SCGTA_TB_TipoOrden.FindByCodTipoOrden(intCodTipo)
            End If

            If Not IsNothing(drdCurrentRow) Then
                Return IIf(drdCurrentRow.IsDescripcionNull, CStr(intCodTipo), drdCurrentRow.Descripcion)
            End If

            Return CStr(intCodTipo)
        End Function

        Public Function Busca_Codigo_Texto(ByVal strTempItem As String, Optional ByVal blnGetCodigo As Boolean = True) As String

            Dim strCod_Item_Comp As String = ""
            Dim strTemp As String = ""
            Dim intCharCont As Integer
            Dim strTextoNoCodigo As String = ""

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

        End Function

        Public Sub Busca_Item_Combo(ByRef Combo As ComboBox, ByVal Cod_Item As String)

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
        End Sub

        Private Sub ControlEstadosOrden()

            CambiarEstadoATodos()

            Select Case m_drdOrdenCurrent.Estado

                Case mc_NumEstado_NoIniciada

                    EstadoNoIniciada()
                    mo_strEstado = mc_PriEstado_NoIniciada

                Case mc_NumEstado_Proceso

                    EstadoProceso()
                    mo_strEstado = mc_PriEstado_Proceso

                Case mc_NumEstado_Suspendida

                    EstadoSuspendida()
                    mo_strEstado = mc_PriEstado_Suspendida

                Case mc_NumEstado_Finalizada

                    EstadoFinalizada()
                    mo_strEstado = mc_PriEstado_Finalizada

                Case mc_NumEstado_Cancelada

                    EstadoFinalizada()
                    mo_strEstado = mc_PriEstado_Cancelada

                Case mc_NumEstado_Cerrada

                    EstadoFinalizada()
                    mo_strEstado = mc_PriEstado_Cerrada

                Case mc_NumEstado_Facturada

                    EstadoFinalizada()
                    mo_strEstado = mc_PriEstado_Facturada

            End Select

        End Sub

        Private Sub CambiarEstadoATodos()

            ''ToolBar Producción y Tab Produccion
            CambiarEstadoTabProduccion()
            ''

            ''Otros Tabs
            btnAceptar.Enabled = True
            btnCambiarEstadoActividad.Enabled = True
            btnAgregarAct.Enabled = True
            btnCambiarEstadoRepuesto.Enabled = True
            btnOrdenCompra.Enabled = True
            btnAgregarRep.Enabled = True
            ''

        End Sub

        Private Sub EstadoNoIniciada()
            ''ToolBar Producción
            btnRechazar.Enabled = False
            btnReproceso.Enabled = False
            btnSuspension.Enabled = False
            btnCalidad.Enabled = False
            btnFinalizar.Enabled = False
            ''

            ''Tab Producción
            btnInicioFecha.Enabled = False
            'btnSuspende.Enabled = False
            btnFinaliza.Enabled = False
            ''
        End Sub

        Private Sub EstadoProceso()

        End Sub

        Private Sub EstadoSuspendida()

        End Sub

        Private Sub EstadoCerrada()
            ''ToolBar Producción
            btnIniciar.Enabled = False
            btnRechazar.Enabled = False
            btnReproceso.Enabled = False
            'btnSuspension.Enabled = False
            btnCalidad.Enabled = False
            btnFinalizar.Enabled = False
            ''

            ''Otros Tabs
            btnInicioFecha.Enabled = False
            btnSuspende.Enabled = False
            btnFinaliza.Enabled = False
            btnAceptar.Enabled = False
            btnCambiarEstadoActividad.Enabled = False
            btnAgregarAct.Enabled = False
            btnCambiarEstadoRepuesto.Enabled = False
            btnOrdenCompra.Enabled = False
            btnAgregarRep.Enabled = False
            btnAsignar.Enabled = False
            btnSolicitar.Enabled = False
            ''
            btnAsignacionMultiple.Enabled = False
        End Sub

        Private Sub EstadoFinalizada()

            cboEstadoOrden.Enabled = False

            btnAsignarRampa.Enabled = False
            btnQuitarRampa.Enabled = False

            ''ToolBar Producción
            btnIniciar.Enabled = False
            btnRechazar.Enabled = False
            btnReproceso.Enabled = False
            btnSuspension.Enabled = False
            btnCalidad.Enabled = False
            btnFinalizar.Enabled = False
            ''

            ''Otros Tabs
            btnInicioFecha.Enabled = False
            btnSuspende.Enabled = False
            btnFinaliza.Enabled = False
            btnAceptar.Enabled = False
            btnCambiarEstadoActividad.Enabled = False
            btnAgregarAct.Enabled = False
            btnEliminarAct.Enabled = False
            btnCambiarEstadoRepuesto.Enabled = False
            btnOrdenCompra.Enabled = False
            btnEliminarRep.Enabled = False
            btnAgregarRep.Enabled = False
            btnAsignar.Enabled = False
            btnEliminarColaborador.Enabled = False
            btnMenuFases.Enabled = False
            txtFSalida.ReadOnly = True
            btnAgregaSum.Enabled = False
            btnEliminaSum.Enabled = False
            btnOrdenCompraSE.Enabled = False
            btnAgregarSE.Enabled = False
            btnEliminarSE.Enabled = False
            btnSolicitar.Enabled = False
            ''
            btnAsignacionMultiple.Enabled = False
        End Sub

        Private Function VerificarCambiaEstado() As Integer
            Dim intValueResult As Integer

            If cboEstadoOrden.SelectedValue = mc_PriEstado_NoIniciada Then

                If m_drdOrdenCurrent.Estado = mc_NumEstado_NoIniciada Then
                    intValueResult = 0
                Else
                    intValueResult = -1
                End If

            End If

            If cboEstadoOrden.SelectedValue = mc_PriEstado_Proceso Then

                If m_drdOrdenCurrent.Estado = mc_NumEstado_NoIniciada Or _
                    m_drdOrdenCurrent.Estado = mc_NumEstado_Proceso Or _
                    m_drdOrdenCurrent.Estado = mc_NumEstado_Suspendida Then
                    intValueResult = 0
                Else
                    intValueResult = -2
                End If

            End If

            If cboEstadoOrden.SelectedValue = mc_PriEstado_Suspendida Then

                If m_drdOrdenCurrent.Estado = mc_NumEstado_NoIniciada Or _
                    m_drdOrdenCurrent.Estado = mc_NumEstado_Proceso Or _
                    m_drdOrdenCurrent.Estado = mc_NumEstado_Suspendida Then
                    intValueResult = 0
                Else
                    intValueResult = -3
                End If

            End If

            If cboEstadoOrden.SelectedValue = mc_PriEstado_Finalizada Then

                If m_drdOrdenCurrent.Estado = mc_NumEstado_NoIniciada Or _
                    m_drdOrdenCurrent.Estado = mc_NumEstado_Proceso Or _
                    m_drdOrdenCurrent.Estado = mc_NumEstado_Suspendida Then
                    intValueResult = 0
                Else
                    intValueResult = -4
                End If

            End If

            If cboEstadoOrden.SelectedValue = mc_PriEstado_Cancelada Then

                If m_drdOrdenCurrent.Estado = mc_NumEstado_NoIniciada Or _
                    m_drdOrdenCurrent.Estado = mc_NumEstado_Proceso Or _
                    m_drdOrdenCurrent.Estado = mc_NumEstado_Suspendida Then
                    intValueResult = 0
                Else
                    intValueResult = -5
                End If

            End If

            Return intValueResult

        End Function

        Private Function OrdenIniciada() As Boolean
            Dim blnResult As Boolean

            If m_drdOrdenCurrent.Estado = mc_NumEstado_Proceso Then
                blnResult = True
            Else
                blnResult = False
            End If

            Return blnResult
        End Function

        Private Sub IniciarOrden()

            Dim drdOrden As OrdenTrabajoDataset.SCGTA_TB_OrdenRow
            Dim objAdapter As SCGDataAccess.OrdenTrabajoDataAdapter

            With m_dtsOrden
                drdOrden = .SCGTA_TB_Orden.FindByNoOrden(m_strNoOrden)
                If Not IsNothing(drdOrden) Then
                    With drdOrden
                        .Estado = Utilitarios.GEnum_EstadoOrden.dmsProceso
                    End With

                    objAdapter = New SCGDataAccess.OrdenTrabajoDataAdapter
                    objAdapter.Actualizar(m_dtsOrden)

                    m_drdOrdenCurrent = .SCGTA_TB_Orden.FindByNoOrden(m_strNoOrden)
                End If
            End With

            cboEstadoOrden.SelectedValue = mc_PriEstado_Proceso

            DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.ActualizarEstadoCotizacion(m_drdOrdenCurrent.NoCotizacion, cboEstadoOrden.SelectedValue)

            ControlEstadosOrden()

        End Sub

        Private Sub SuspenderOrden()

            Dim drdOrden As OrdenTrabajoDataset.SCGTA_TB_OrdenRow
            Dim objAdapter As SCGDataAccess.OrdenTrabajoDataAdapter

            With m_dtsOrden
                drdOrden = .SCGTA_TB_Orden.FindByNoOrden(m_strNoOrden)
                If Not IsNothing(drdOrden) Then
                    With drdOrden
                        .Estado = Utilitarios.GEnum_EstadoOrden.dmsSuspendida
                    End With

                    objAdapter = New SCGDataAccess.OrdenTrabajoDataAdapter
                    objAdapter.Actualizar(m_dtsOrden)

                    m_drdOrdenCurrent = .SCGTA_TB_Orden.FindByNoOrden(m_strNoOrden)
                End If
            End With

            cboEstadoOrden.SelectedValue = mc_PriEstado_Suspendida

            DMSOneFramework.SCGBusinessLogic.MetodosCompartidosSBOCls.ActualizarEstadoCotizacion(m_drdOrdenCurrent.NoCotizacion, cboEstadoOrden.SelectedValue)

            ControlEstadosOrden()

        End Sub

        Private Function ActualizaFasesYColab(ByRef strMensajeError As String) As Boolean
            Return True
        End Function

       Private Function VerificaExistenPendientes() As Boolean
            Dim blnResult As Boolean = True

            If ValidarColaboradoresAsignados() <> 0 Then
                blnResult = False
                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeNoPuedeFinalizarOTxAcitividadessinColaboradores)
            End If
            If ValidarSolicitudEspecificosPendientes() <> 0 Then
                blnResult = False
                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeNoPuedeFinalizarOTXSolicitudes)
            End If
            If ValidarSuministrosNoTrasladados() <> 0 Then
                blnResult = False
                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeNoPuedeFinalizarOTxSumPendientes)
            End If

            Return blnResult

        End Function

        Private Function ValidarSuministrosNoTrasladados() As Integer

            Dim intResult As Integer
            Dim a_drwSuministros() As System.Data.DataRow
            m_AdpSum = New SuministrosDataAdapter()
            m_AdpSum.Fill(m_dstSum, m_strNoOrden, -1, -1)

            a_drwSuministros = m_dstSum.SCGTA_VW_Suministros.Select("Trasladada = 3")

            intResult = a_drwSuministros.Length
            Return intResult

        End Function

        Private Function ValidarColaboradoresAsignados() As Integer
            Dim adpControlColaborador As New SCGDataAccess.ColaboradorDataAdapter(True)
            Dim intResult As Integer

            intResult = adpControlColaborador.VerificarColAsig(m_strNoOrden)

            Return intResult

        End Function

        Private Function ValidarSolicitudEspecificosPendientes() As Integer
            Dim adpSolicitudEspecificos As New SCGDataAccess.SolicitudEspecificosDataAdapter
            Dim dtsSolicitudEspecificos As New SolicitudEspecificosDataset
            adpSolicitudEspecificos.Fill(dtsSolicitudEspecificos, , m_strNoOrden, , , , 0)
            Dim intResult As Integer

            intResult = dtsSolicitudEspecificos.SCGTA_SP_SelSolicitudEspecifico.Rows.Count

            Return intResult

        End Function

        Private Function ValidarItemsPendientes() As Integer
            Dim adpRepXEstado As New SCGDataAccess.RepuestosxEstadoDataAdapter
            Dim intResult As Integer

            intResult = adpRepXEstado.ValidarItemsPendientes(m_strNoOrden)

            Return intResult

        End Function

        Private Sub RealizarTransferenciasStock(ByVal p_strNoOrden As String, ByVal intNoCotizacion As Integer)
            Dim adpTransferencias As DMSOneFramework.SCGBusinessLogic.TransferenciaItems

            Dim strIDBodegaRep As String = ""
            Dim strIDBodegaSum As String = ""
            Dim strIDBodegaSer As String = ""
            Dim strIDBodegaProceso As String = ""
            Dim strIDSeriesTrasl As String = ""

            Try
                '--------agregado para transferencias de Stock a borrador
                Dim adpConf As New ConfiguracionDataAdapter
                Dim dstConf As New ConfiguracionDataSet
                '    Dim drwConf As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow
                Dim blnDraft As Boolean = False

                adpConf.Fill(dstConf)

                If ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracionDraft(dstConf.SCGTA_TB_Configuracion, "CreaDraftTransferenciasStock", "") Then
                    blnDraft = True
                Else
                    blnDraft = False
                End If
                '---------------------------------------------------------

                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "BodegaRepuestos", strIDBodegaRep)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "BodegaSuministros", strIDBodegaSum)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "BodegaServiciosExternos", strIDBodegaSer)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "BodegaProceso", strIDBodegaProceso)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "IDSerieDocumentosTraslado", strIDSeriesTrasl)

                adpTransferencias = New DMSOneFramework.SCGBusinessLogic.TransferenciaItems(G_objCompany)


                adpTransferencias.CrearTrasladoByCancel(p_strNoOrden, strIDBodegaRep, strIDBodegaSum, strIDBodegaSer, strIDBodegaProceso, strIDSeriesTrasl, blnDraft, intNoCotizacion)


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Function EliminarPaquete(ByVal p_intLineNumPaquete As Integer, ByRef p_strMensaje As String) As Boolean

            Dim drwActividades As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
            Dim drwSuministros As DMSOneFramework.SuministrosDataset.SCGTA_VW_SuministrosRow
            Dim drwRepuestos As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow

            Dim adpActividades As New DMSOneFramework.SCGDataAccess.ActividadesXFaseDataAdapter
            Dim adpRepuestos As New DMSOneFramework.SCGDataAccess.RepuestosxOrdenDataAdapter
            Dim adpSuministros As New DMSOneFramework.SCGDataAccess.SuministrosDataAdapter

            Dim tnTransaccion As SqlClient.SqlTransaction = Nothing
            Dim cnConeccion As New SqlClient.SqlConnection(strConectionString)

            Dim intEstadoCombo As Integer
            Dim blnEliminarLinea As Boolean = True
            Dim blnEliminarRepuesto As Boolean = False
            Dim IntCodEstado As Integer
            Dim intCodFase As Integer

            Try
                m_dstRep.RejectChanges()
                m_dstServiciosExternos.RejectChanges()
                m_dstAct.RejectChanges()
                m_dstSum.RejectChanges()

                'Chequeo de las actividades
                For Each drwActividades In m_dstAct.SCGTA_TB_ActividadesxOrden.Rows
                    If drwActividades.LineNumFather = p_intLineNumPaquete Then
                        If (drwActividades.CodEstadoLinea = SCGEstadoLinea.scgFaltaAprobacion) Or (drwActividades.Estado = mc_strNoIniciada) Then

                            If blnEliminarLinea Then
                                g_AgregaAdicionales = True
                                If p_strMensaje = "" Then
                                    p_strMensaje = "'" & drwActividades.ItemName & "'"
                                Else
                                    p_strMensaje = p_strMensaje & ", '" & drwActividades.ItemName & "'"
                                End If
                                drwActividades.Delete()
                            End If
                        Else
                            If blnEliminarLinea Then
                                p_strMensaje = ""
                                blnEliminarLinea = False
                            End If
                            If p_strMensaje = "" Then
                                p_strMensaje = "'" & drwActividades.ItemName & "'"
                            Else
                                p_strMensaje = p_strMensaje & ", '" & drwActividades.ItemName & "'"
                            End If
                            g_AgregaAdicionales = False
                        End If
                    End If

                Next

                'Chequeo de los suministros
                For Each drwSuministros In m_dstSuministros.SCGTA_VW_Suministros.Rows

                    If drwSuministros.LineNumFather = p_intLineNumPaquete Then
                        If blnEliminarLinea Then
                            g_AgregaAdicionales = True
                            If p_strMensaje = "" Then
                                p_strMensaje = "'" & drwSuministros.itemName & "'"
                            Else
                                p_strMensaje = p_strMensaje & ", '" & drwSuministros.itemName & "'"
                            End If
                            drwSuministros.Delete()
                        End If
                    Else
                        If blnEliminarLinea Then
                            p_strMensaje = ""
                            blnEliminarLinea = False
                        End If
                        If p_strMensaje = "" Then
                            p_strMensaje = "'" & drwSuministros.itemName & "'"
                        Else
                            p_strMensaje = p_strMensaje & ", '" & drwSuministros.itemName & "'"
                        End If
                    End If

                Next

                'Chequeo de los servicios externos
                intEstadoCombo = CInt(Busca_Codigo_Texto(cboEstadoRep.Text, True))
                For Each drwRepuestos In m_dstServiciosExternos.SCGTA_TB_RepuestosxOrden.Rows
                    If drwRepuestos.LineNumFather = p_intLineNumPaquete Then
                        If Not VerificarEstadoRepPend(drwRepuestos.ID) Then

                            If blnEliminarLinea Then
                                p_strMensaje = ""
                                blnEliminarLinea = False
                            End If
                            'If p_strMensaje = "" Then
                            '    p_strMensaje = "'" & drwRepuestos.Itemname & "'"
                            'Else
                            '    p_strMensaje = p_strMensaje & ", '" & drwRepuestos.Itemname & "'"
                            'End If
                            blnEliminarRepuesto = False
                        Else

                            If (drwRepuestos.CodEstadoLinea = SCGEstadoLinea.scgFaltaAprobacion) _
                                Or Not (drwRepuestos.IsCantidadPendienteNull) _
                                Or Not (drwRepuestos.IsCantidadPendienteTrasladoNull) Then

                                If (drwRepuestos.CantidadPendiente = drwRepuestos.Cantidad) _
                                Or (drwRepuestos.CantidadPendienteTraslado = drwRepuestos.Cantidad) Then

                                    blnEliminarRepuesto = True

                                End If

                            Else
                                If blnEliminarLinea Then
                                    If Not drwRepuestos.IsCantidadEstadoNull Then

                                        If (drwRepuestos.CantidadEstado = drwRepuestos.Cantidad And _
                                            (intEstadoCombo = 1 Or intEstadoCombo = 5)) Then

                                            blnEliminarRepuesto = True

                                        End If
                                    Else
                                        blnEliminarRepuesto = False
                                    End If
                                End If
                            End If
                        End If
                        If blnEliminarRepuesto Then
                            blnEliminarRepuesto = False

                            g_AgregaAdicionales = True
                            If p_strMensaje = "" Then
                                p_strMensaje = "'" & drwRepuestos.Itemname & "'"
                            Else
                                p_strMensaje = p_strMensaje & ", '" & drwRepuestos.Itemname & "'"
                            End If
                            drwRepuestos.Delete()
                        Else
                            blnEliminarRepuesto = False
                            If blnEliminarLinea Then
                                p_strMensaje = ""
                                blnEliminarLinea = False
                            End If
                            If p_strMensaje = "" Then
                                p_strMensaje = "'" & drwRepuestos.Itemname & "'"
                            Else
                                p_strMensaje = p_strMensaje & ", '" & drwRepuestos.Itemname & "'"
                            End If

                        End If
                    End If
                Next

                'Chequeo de los repuestos
                intEstadoCombo = CInt(Busca_Codigo_Texto(cboEstadoRep2.Text, True))

                For Each drwRepuestos In m_dstRep.SCGTA_TB_RepuestosxOrden.Rows
                    If drwRepuestos.LineNumFather = p_intLineNumPaquete Then
                        If Not VerificarEstadoRepPend(drwRepuestos.ID) Then

                            If blnEliminarLinea Then
                                p_strMensaje = ""
                                blnEliminarLinea = False
                            End If
                            'If p_strMensaje = "" Then
                            '    p_strMensaje = "'" & drwRepuestos.Itemname & "'"
                            'Else
                            '    p_strMensaje = p_strMensaje & ", '" & drwRepuestos.Itemname & "'"
                            'End If
                            blnEliminarRepuesto = False
                        Else

                            If (drwRepuestos.CodEstadoLinea = SCGEstadoLinea.scgFaltaAprobacion) _
                                Or Not (drwRepuestos.IsCantidadPendienteNull) _
                                Or Not (drwRepuestos.IsCantidadPendienteTrasladoNull) Then

                                If (drwRepuestos.CantidadPendiente = drwRepuestos.Cantidad) _
                                Or (drwRepuestos.CantidadPendienteTraslado = drwRepuestos.Cantidad) Then

                                    blnEliminarRepuesto = True

                                End If

                            Else
                                If blnEliminarLinea Then
                                    If Not drwRepuestos.IsCantidadEstadoNull Then

                                        If (drwRepuestos.CantidadEstado = drwRepuestos.Cantidad And _
                                            (intEstadoCombo = 1 Or intEstadoCombo = 5)) Then

                                            blnEliminarRepuesto = True

                                        End If
                                    Else
                                        blnEliminarRepuesto = False
                                    End If
                                End If
                            End If
                        End If

                        If blnEliminarRepuesto Then
                            If blnEliminarLinea Then
                                blnEliminarRepuesto = False

                                g_AgregaAdicionales = True
                                If p_strMensaje = "" Then
                                    p_strMensaje = "'" & drwRepuestos.Itemname & "'"
                                Else
                                    p_strMensaje = p_strMensaje & ", '" & drwRepuestos.Itemname & "'"
                                End If
                                drwRepuestos.Delete()
                            End If
                        Else
                            blnEliminarRepuesto = False
                            If blnEliminarLinea Then
                                p_strMensaje = ""
                                blnEliminarLinea = False
                            End If
                            If p_strMensaje = "" Then
                                p_strMensaje = "'" & drwRepuestos.Itemname & "'"
                            Else
                                p_strMensaje = p_strMensaje & ", '" & drwRepuestos.Itemname & "'"
                            End If

                        End If
                    End If

                Next
                If blnEliminarLinea Then

                    'Actualizar actividades
                    Call adpActividades.Update(m_dstAct.SCGTA_TB_ActividadesxOrden, cnConeccion, tnTransaccion, True, False)

                    'Actualiza Suministros
                    Call adpSuministros.Update(m_dstSuministros.SCGTA_VW_Suministros, cnConeccion, tnTransaccion, False, False)

                    'Actualizar Servicios Externos
                    Call adpRepuestos.Update(m_dstServiciosExternos.SCGTA_TB_RepuestosxOrden, cnConeccion, tnTransaccion, False, False)

                    'Actualizar Repuestos
                    Call adpRepuestos.Update(m_dstRep.SCGTA_TB_RepuestosxOrden, cnConeccion, tnTransaccion, False, False)

                    MetodosCompartidosSBOCls.EliminarItemCotizacion(p_intLineNumPaquete)
                    MetodosCompartidosSBOCls.ActualizarCotizacion()
                    tnTransaccion.Commit()
                    cnConeccion.Close()
                    MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Commit)

                    'Grid de actividades
                    intCodFase = CInt(Busca_Codigo_Texto(cboFasesProdF.Text, True))
                    CargarGridActividades(intCodFase, IIf(chkAdicionalAct.Checked, 1, 0))

                    'Grid de Suministros
                    CargaGridSuministros1(0)

                    'Grid de servicios externos
                    IntCodEstado = CInt(Busca_Codigo_Texto(cboEstado.Text, True))
                    CargarGridRepuesto(IntCodEstado, IIf(chkAdicionalesSE.Checked, 1, 0), _
                                       enTipoArticulo.ServicioExterno, m_dstServiciosExternos, dtgSE, mc_strServicioExterno)

                    CargarEstadoLineaResources(m_dstServiciosExternos)

                    'grid de repuestos
                    IntCodEstado = CInt(Busca_Codigo_Texto(cboEstado.Text, True))
                    CargarGridRepuesto(IntCodEstado, IIf(chkAdicionalRep.Checked, 1, 0), _
                                       enTipoArticulo.Repuesto, m_dstRep, dtgRepuestos, mc_strComponenteEtiqueta)

                Else

                    m_dstRep.RejectChanges()
                    m_dstServiciosExternos.RejectChanges()
                    m_dstAct.RejectChanges()
                    m_dstSuministros.RejectChanges()

                End If

                Return blnEliminarLinea
            Catch ex As Exception
                m_dstRep.RejectChanges()
                m_dstServiciosExternos.RejectChanges()
                m_dstAct.RejectChanges()
                m_dstSuministros.RejectChanges()
                If cnConeccion IsNot Nothing Then
                    If cnConeccion.State <> ConnectionState.Closed Then
                        tnTransaccion.Rollback()
                        cnConeccion.Close()
                        m_dstRep.RejectChanges()
                        m_dstServiciosExternos.RejectChanges()
                        m_dstAct.RejectChanges()
                        m_dstSum.RejectChanges()
                    End If
                End If
                MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

                Throw ex
            Finally
                'Agregado 06072010
                Call cnConeccion.Close()
            End Try
        End Function

        Private Sub CargarUnidadesTiempoGlobales()

            If g_intUnidadTiempo <> -1 Then

                Dim adpUnidadTiempoDataAdapter As New DMSONEDKFramework.UnidadTiempoDataAdapter
                Dim dstUnidadTiempoDataSet As New DMSONEDKFramework.UnidadTiempoDataSet
                Dim drwFila() As DataRow
                adpUnidadTiempoDataAdapter.Fill(dstUnidadTiempoDataSet)
                drwFila = dstUnidadTiempoDataSet.SCGTA_TB_UnidadTiempo.Select("CodigoUnidadTiempo = " & g_intUnidadTiempo)
                m_strDescripcionUnidadTiempo = drwFila(0)("DescripcionUnidadTiempo")
                m_dblValorUnidadTiempo = drwFila(0)("TiempoMinutosUnidadTiempo")

            End If

        End Sub

        Public Sub Visualizacion_UDF()

            VisualizarUDFOrden.Tabla = "SCGTA_TB_Orden"

            VisualizarUDFOrden.Conexion = SCGDataAccess.DAConexion.ConnectionString

            VisualizarUDFOrden.CampoLlave = "NoOrden = '" & txtNoOrden.Text & "'"

            VisualizarUDFOrden.Form = Me

            VisualizarUDFOrden.VisualizarUDF()

            VisualizarUDFOrden.Where = "NoOrden = '" & txtNoOrden.Text & "'"

            VisualizarUDFOrden.CargarComboCategorias()

        End Sub


        Private Function ValidarAsignacionUnicaMO(ByVal p_intIDActividad As Integer) As Boolean
            Try
                '--------agregado para transferencias de Stock a borrador
                Dim adpConf As New ConfiguracionDataAdapter
                Dim dstConf As New ConfiguracionDataSet
                '    Dim drwConf As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow
                Dim blnValida As Boolean = False

                Dim strEstadoActividad As String = String.Empty
                Dim objDAColaborador As DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
                Dim dstColaborador As New ColaboradorDataset
                Dim drwControlColaborador As ColaboradorDataset.SCGTA_TB_ControlColaboradorRow

                Dim valorRetorno As Boolean = False

                adpConf.Fill(dstConf)

                If ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracionDraft(dstConf.SCGTA_TB_Configuracion, mc_AsignacionUnicaMO, "") Then
                    blnValida = True
                Else
                    blnValida = False
                End If
                '---------------------------------------------------------

                If blnValida = True Then

                    objDAColaborador = New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter

                    objDAColaborador.SelControlColaboradorxActividad(dstColaborador, txtNoOrden.Text, p_intIDActividad)


                    For Each drwControlColaborador In dstColaborador.SCGTA_TB_ControlColaborador
                        If drwControlColaborador.Estado <> "Suspendido" Then
                            valorRetorno = True
                            Return valorRetorno
                        End If
                    Next

                    Return False
                End If



            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            End Try
        End Function


#End Region

#Region "Principal"

        Private Sub CambiarEstiloBotonesSuministros(ByVal intBotonActivo As Integer)
            btnRequisiciones.FlatStyle = FlatStyle.Standard
            btnDevoluciones.FlatStyle = FlatStyle.Standard
            btnSuministros.FlatStyle = FlatStyle.Standard

            Select Case intBotonActivo
                Case 1
                    btnRequisiciones.FlatStyle = FlatStyle.Flat
                Case 2
                    btnDevoluciones.FlatStyle = FlatStyle.Flat
                Case 3
                    btnSuministros.FlatStyle = FlatStyle.Flat
            End Select

            dtgSuministros.Tag = intBotonActivo
        End Sub

        Private Sub CargarGridSuministros()
            Dim dtsSuministros As SuministrosXOrdenDataset
            Dim dtsSuministrosFull As SuministrosFullDataset
            Dim dtvSuministros As New DataView

            m_AdpSum = New SCGDataAccess.SuministrosDataAdapter

            Select Case CInt(dtgSuministros.Tag)
                Case 1
                    dtsSuministros = New SuministrosXOrdenDataset

                    'If optFacturable.Checked Then
                    '    m_AdpSum.CargarRequisicionesFacturables(dtsSuministros, txtNoOrden.Text, CInt(Busca_Codigo_Texto(cboCentroCostoR.Text)), "1")
                    'ElseIf optNoFacturables.Checked Then
                    '    m_AdpSum.CargarRequisicionesFacturables(dtsSuministros, txtNoOrden.Text, CInt(Busca_Codigo_Texto(cboCentroCostoR.Text)), "2")
                    'Else
                    '    m_AdpSum.CargarRequisiciones(dtsSuministros, txtNoOrden.Text, CInt(Busca_Codigo_Texto(cboCentroCostoR.Text)))
                    'End If

                    dtvSuministros.Table = dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida
                    EstiloGridSuministrosXOrden(dtsSuministros)

                Case 2

                    dtsSuministros = New SuministrosXOrdenDataset
                    'If optFacturable.Checked Then
                    '    m_AdpSum.CargarDevolucionesFacturables(dtsSuministros, txtNoOrden.Text, Busca_Codigo_Texto(cboCentroCostoR.Text), "1")
                    'ElseIf optNoFacturables.Checked Then
                    '    m_AdpSum.CargarDevolucionesFacturables(dtsSuministros, txtNoOrden.Text, Busca_Codigo_Texto(cboCentroCostoR.Text), "2")
                    'Else
                    '    m_AdpSum.CargarDevoluciones(dtsSuministros, txtNoOrden.Text, Busca_Codigo_Texto(cboCentroCostoR.Text))
                    'End If

                    dtvSuministros.Table = dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida
                    EstiloGridSuministrosXOrden(dtsSuministros)

                Case 3
                    dtsSuministrosFull = New SuministrosFullDataset

                    'If optFacturable.Checked Then
                    '    m_AdpSum.CargarSuministrosFacturables(dtsSuministrosFull, txtNoOrden.Text, Busca_Codigo_Texto(cboCentroCostoR.Text), "1")
                    'ElseIf optNoFacturables.Checked Then
                    '    m_AdpSum.CargarSuministrosFacturables(dtsSuministrosFull, txtNoOrden.Text, Busca_Codigo_Texto(cboCentroCostoR.Text), "2")
                    'Else
                    '    m_AdpSum.CargarSuministros(dtsSuministrosFull, txtNoOrden.Text, Busca_Codigo_Texto(cboCentroCostoR.Text))
                    'End If

                    dtvSuministros.Table = dtsSuministrosFull.SCGTA_SP_SelSuministrosFull
                    EstiloGridSuministrosFull(dtsSuministrosFull)
            End Select

            With dtvSuministros
                .AllowDelete = False
                .AllowEdit = False
                .AllowNew = False
            End With

            dtgSuministros.DataSource = dtvSuministros

        End Sub

        'Private Sub CargarComboEstadosOrden()
        '    With cboEstadoOrden
        '        .Items.Clear()

        '        .Items.Add(mc_PriEstado_NoIniciada)
        '        .Items.Add(mc_PriEstado_Proceso)
        '        .Items.Add(mc_PriEstado_Suspendida)
        '        .Items.Add(mc_PriEstado_Finalizada)
        '        '''''''''''''''''''''''''''''''''''
        '        -.Items.Add(mc_PriEstado_Cerrada)
        '    End With


        'End Sub

        Private Sub EstiloGridSuministrosXOrden(ByRef dtsSuministros As SuministrosXOrdenDataset)
            Dim tsConfiguracion As New DataGridTableStyle

            Dim tcItemCode As New DataGridTextBoxColumn
            Dim tcItemName As New DataGridTextBoxColumn
            Dim tcQuantity As New DataGridTextBoxColumn
            Dim tcDocDate As New DataGridTextBoxColumn
            Dim tcMonto As New DataGridTextBoxColumn
            Dim tcEmpID As New DataGridTextBoxColumn
            Dim tcNoOrden As New DataGridTextBoxColumn
            Dim tcNombreEmp As New DataGridTextBoxColumn
            Dim tcNoCentroCosto As New DataGridTextBoxColumn
            Dim tcDescCentroCosto As New DataGridTextBoxColumn
            Dim tcResultado As New DataGridValidatedTextColumn
            Dim tcFechaInsercion As New DataGridConditionalColumn

            dtgSuministros.TableStyles.Clear()

            tsConfiguracion.MappingName = dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida.TableName

            With tcItemCode
                .Width = 60
                .HeaderText = My.Resources.ResourceUI.NoItem
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida.Columns(mcsum_strItemCode).ColumnName
            End With

            tcResultado.Width = 300
            tcResultado.HeaderText = My.Resources.ResourceUI.Resultados
            tcResultado.MappingName = "ResultadoActividad"
            tcResultado.NullText = ""
            tcResultado.ReadOnly = False
            AddHandler tcResultado.Cambio_Valor, AddressOf CambiaResultadoSuministros

            tcFechaInsercion.Width = 300
            tcFechaInsercion.HeaderText = "FechaInsercion"
            tcFechaInsercion.MappingName = "FechaInsercion"
            tcFechaInsercion.ReadOnly = True
            '            tcFechaInsercion.P_Formato = "{0:d}"
            tcFechaInsercion.NullText = ""


            With tcItemName
                .Width = 200
                .HeaderText = My.Resources.ResourceUI.Descripcion  '"Descripción"
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida.Columns(mcsum_strItemName).ColumnName
                .NullText = "- - -"
            End With

            With tcQuantity
                .Width = 50
                .HeaderText = My.Resources.ResourceUI.Cantidad  '"Cant."
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida.Columns(mcsum_dblQuantity).ColumnName
                .NullText = "0"
                .Format = "#,##0.00"
            End With

            With tcDocDate
                .Width = 67
                .HeaderText = My.Resources.ResourceUI.Fecha  '"Fecha"
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida.Columns(mcsum_dtDocDate).ColumnName
            End With

            With tcMonto
                .Width = 90
                .HeaderText = My.Resources.ResourceUI.Monto  '"Monto"
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida.Columns(mcsum_dblMonto).ColumnName
                .NullText = "0"
                .Format = "#,##0.00"
            End With

            With tcEmpID
                .Width = 40
                .HeaderText = My.Resources.ResourceUI.Cod
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida.Columns(mcsum_intEmpId).ColumnName
                .NullText = "- - -"
            End With

            With tcNoOrden
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.NoOrden  '"No Orden"
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida.Columns(mcsum_strNoOrden).ColumnName
                .NullText = "- - -"
            End With

            With tcNombreEmp
                .Width = 150
                .HeaderText = My.Resources.ResourceUI.Colaboradores  '"Colaborador"
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida.Columns(mcsum_strNombreEmp).ColumnName
                .NullText = "- - -"
            End With

            With tcNoCentroCosto
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.NoCentroCosto   '"No C.C."
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida.Columns(mcsum_intNoCentroCosto).ColumnName
                .NullText = ""
            End With

            With tcDescCentroCosto
                .Width = 100
                .HeaderText = My.Resources.ResourceUI.CentroCosto  '"Centro de Costo"
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida.Columns(mcsum_strDescCentroCosto).ColumnName
                .NullText = "- - -"
            End With

            tsConfiguracion.GridColumnStyles.Add(tcItemCode)
            tsConfiguracion.GridColumnStyles.Add(tcItemName)
            tsConfiguracion.GridColumnStyles.Add(tcQuantity)
            tsConfiguracion.GridColumnStyles.Add(tcMonto)
            tsConfiguracion.GridColumnStyles.Add(tcDocDate)
            tsConfiguracion.GridColumnStyles.Add(tcEmpID)
            tsConfiguracion.GridColumnStyles.Add(tcNombreEmp)
            tsConfiguracion.GridColumnStyles.Add(tcNoOrden)
            tsConfiguracion.GridColumnStyles.Add(tcNoCentroCosto)
            tsConfiguracion.GridColumnStyles.Add(tcDescCentroCosto)
            tsConfiguracion.GridColumnStyles.Add(tcResultado)
            tsConfiguracion.GridColumnStyles.Add(tcFechaInsercion)

            tsConfiguracion.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
            tsConfiguracion.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
            tsConfiguracion.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
            tsConfiguracion.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
            tsConfiguracion.RowHeadersVisible = False

            dtgSuministros.TableStyles.Add(tsConfiguracion)
        End Sub

        Private Sub EstiloGridSuministrosFull(ByRef dtsSuministros As SuministrosFullDataset)
            Dim tsConfiguracion As New DataGridTableStyle

            Dim tcItemCode As New DataGridTextBoxColumn
            Dim tcItemName As New DataGridTextBoxColumn
            Dim tcQuantity As New DataGridTextBoxColumn
            Dim tcDocDate As New DataGridTextBoxColumn
            Dim tcMonto As New DataGridTextBoxColumn
            Dim tcEmpID As New DataGridTextBoxColumn
            Dim tcNoOrden As New DataGridTextBoxColumn
            Dim tcNombreEmp As New DataGridTextBoxColumn
            Dim tcNoCentroCosto As New DataGridTextBoxColumn
            Dim tcDescCentroCosto As New DataGridTextBoxColumn
            Dim tcResultado As New DataGridValidatedTextColumn
            Dim tcFechaInsercion As New DataGridConditionalColumn

            dtgSuministros.TableStyles.Clear()

            tsConfiguracion.MappingName = dtsSuministros.SCGTA_SP_SelSuministrosFull.TableName

            With tcItemCode
                .Width = 60
                .HeaderText = My.Resources.ResourceUI.NoItem
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosFull.Columns(mcsum_strItemCode).ColumnName
            End With

            tcResultado.Width = 300
            tcResultado.HeaderText = My.Resources.ResourceUI.Resultados
            tcResultado.MappingName = "ResultadoActividad"
            tcResultado.NullText = ""
            tcResultado.ReadOnly = False
            AddHandler tcResultado.Cambio_Valor, AddressOf CambiaResultadoSuministros

            tcFechaInsercion.Width = 300
            tcFechaInsercion.HeaderText = "FechaInsercion"
            tcFechaInsercion.MappingName = "FechaInsercion"
            tcFechaInsercion.ReadOnly = True
            '            tcFechaInsercion.P_Formato = "{0:d}"
            tcFechaInsercion.NullText = ""

            With tcItemName
                .Width = 200
                .HeaderText = My.Resources.ResourceUI.Descripcion  '"Descripción"
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosFull.Columns(mcsum_strItemName).ColumnName
                .NullText = "- - -"
            End With

            With tcQuantity
                .Width = 50
                .HeaderText = My.Resources.ResourceUI.Cantidad
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosFull.Columns(mcsum_dblCantidadFinal).ColumnName
                .NullText = "0"
                .Format = "#,##0.00"
            End With

            'With tcDocDate
            '    .Width = 67
            '    .HeaderText = "Fecha"
            '    .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosFull.Columns(mcsum_dtDocDate).ColumnName
            'End With

            With tcMonto
                .Width = 90
                .HeaderText = My.Resources.ResourceUI.Monto
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosFull.Columns(mcsum_dblMonto).ColumnName
                .NullText = "0"
                .Format = "#,##0.00"
            End With

            'With tcEmpID
            '    .Width = 40
            '    .HeaderText = "Cod."
            '    .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosFull.Columns(mcsum_intEmpId).ColumnName
            '    .NullText = "- - -"
            'End With

            With tcNoOrden
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.NoOrden
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosFull.Columns(mcsum_strNoOrden).ColumnName
                .NullText = "- - -"
            End With

            'With tcNombreEmp
            '    .Width = 150
            '    .HeaderText = "Colaborador"
            '    .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosFull.Columns(mcsum_strNombreEmp).ColumnName
            '    .NullText = "- - -"
            'End With

            With tcNoCentroCosto
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.NoCentroCosto  '"No C.C."
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosFull.Columns(mcsum_intNoCentroCosto).ColumnName
                .NullText = ""
            End With

            With tcDescCentroCosto
                .Width = 107
                .HeaderText = My.Resources.ResourceUI.CentroCosto
                .MappingName = dtsSuministros.SCGTA_SP_SelSuministrosFull.Columns(mcsum_strCentroCosto).ColumnName
                .NullText = "- - -"
            End With

            tsConfiguracion.GridColumnStyles.Add(tcItemCode)
            tsConfiguracion.GridColumnStyles.Add(tcItemName)
            tsConfiguracion.GridColumnStyles.Add(tcQuantity)
            tsConfiguracion.GridColumnStyles.Add(tcMonto)
            'tsConfiguracion.GridColumnStyles.Add(tcDocDate)
            'tsConfiguracion.GridColumnStyles.Add(tcEmpID)
            'tsConfiguracion.GridColumnStyles.Add(tcNombreEmp)
            tsConfiguracion.GridColumnStyles.Add(tcNoOrden)
            tsConfiguracion.GridColumnStyles.Add(tcNoCentroCosto)
            tsConfiguracion.GridColumnStyles.Add(tcDescCentroCosto)
            tsConfiguracion.GridColumnStyles.Add(tcResultado)
            tsConfiguracion.GridColumnStyles.Add(tcFechaInsercion)

            tsConfiguracion.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
            tsConfiguracion.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
            tsConfiguracion.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
            tsConfiguracion.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
            tsConfiguracion.RowHeadersVisible = False

            dtgSuministros.TableStyles.Add(tsConfiguracion)
        End Sub

#End Region

#Region "Rendimiento"

        Private Sub RendimientoCargar()
            'Dim dstMontoRep As New DMSOneFramework.RendMontoReparacionDataset
            Dim dstDurXFase As New DMSOneFramework.DuracionXFaseDataset

            'RendimientosMontoRepCargar(dstMontoRep)

            RendimientoDurXFaseCargar(dstDurXFase)

        End Sub

        Private Sub RendimientosMontoRepCargar(ByRef p_dstMontoRep As DMSOneFramework.RendMontoReparacionDataset)
            Dim adpRendimiento As New SCGDataAccess.RendimientoDataAdapter
            Dim dvwMontoRep As DataView

            adpRendimiento.CargarMontos(p_dstMontoRep, txtNoOrden.Text)

            dvwMontoRep = p_dstMontoRep.SCGTA_SP_SELMontoOtorgadoVsAcumulado.DefaultView

            With dvwMontoRep
                .AllowDelete = False
                .AllowEdit = False
                .AllowNew = False
            End With

            dtgMontoReparacion.DataSource = dvwMontoRep

        End Sub

        Private Sub RendimientoDurXFaseCargar(ByRef p_dstDurXFase As DMSOneFramework.DuracionXFaseDataset)
            Dim adpRendimiento As New SCGDataAccess.RendimientoDataAdapter
            Dim dvwDurXFase As DataView

            adpRendimiento.CargarDuracionXFase(p_dstDurXFase, txtNoOrden.Text)

            CargarUnidadesTiempoProduccionEnDataset(p_dstDurXFase)

            dvwDurXFase = p_dstDurXFase.SCGTA_SP_SELDuracionXFase.DefaultView

            With dvwDurXFase
                .AllowNew = False
                .AllowEdit = False
                .AllowDelete = False
            End With

            dtgRendimientosBarras.DataSource = dvwDurXFase
        End Sub

        Private Sub CargarUnidadesTiempoProduccionEnDataset(ByRef p_dstDurXFase As DMSOneFramework.DuracionXFaseDataset)
            Dim intIndice As Integer
            For intIndice = 0 To p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows.Count - 1

                If m_dblValorUnidadTiempo > 0 Then
                    If Not p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("CantidadHoraManoObra") Is System.DBNull.Value Then
                        p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("CantidadHoraManoObraUnidadTiempo") = Math.Round(p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("CantidadHoraManoObra") / m_dblValorUnidadTiempo, 4)
                    Else
                        p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("CantidadHoraManoObraUnidadTiempo") = 0
                    End If

                Else
                    p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("CantidadHoraManoObraUnidadTiempo") = 0
                End If


                If m_dblValorUnidadTiempo > 0 Then
                    If Not p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("DuracionHorasAprobadas") Is System.DBNull.Value Then
                        p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("HorasAprobadasUnidadTiempo") = Math.Round(p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("DuracionHorasAprobadas") / m_dblValorUnidadTiempo, 4)
                    Else
                        p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("HorasAprobadasUnidadTiempo") = 0
                    End If
                Else
                    p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("HorasAprobadasUnidadTiempo") = 0
                End If


                If m_dblValorUnidadTiempo > 0 Then
                    If Not p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("TiempoRestante") Is System.DBNull.Value Then
                        p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("TiempoRestanteUnidadTiempo") = Math.Round(p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("TiempoRestante") / m_dblValorUnidadTiempo, 4)
                    Else
                        p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("TiempoRestanteUnidadTiempo") = 0
                    End If
                Else
                    p_dstDurXFase.SCGTA_SP_SELDuracionXFase.Rows(intIndice)("TiempoRestanteUnidadTiempo") = 0
                End If


            Next
        End Sub

        Private Sub EstiloGridMontoRepar()
            Dim objTableStyle As DataGridTableStyle
            Dim objDescripCol As DataGridLabelColumn
            Dim objPorcCol As DataGridColumnProgressBar
            Dim objOtorga As DataGridTextBoxColumn
            Dim objAcumula As DataGridTextBoxColumn

            objTableStyle = New DataGridTableStyle

            objTableStyle.MappingName = mc_RenMontosRep_strTableName

            objDescripCol = New DataGridLabelColumn

            With objDescripCol
                .HeaderText = ""
                .MappingName = mc_RenMontosRep_strDescripcion
                '.scgFuenteNegrita = True
                .ReadOnly = True
                .Width = 125
            End With

            objPorcCol = New DataGridColumnProgressBar

            With objPorcCol
                .HeaderText = My.Resources.ResourceUI.Porcentaje  '"Porcentaje"
                .MappingName = mc_RenMontosRep_strPorcentaje
                .scgAllowEdit = False
                .scgLimiteAmarillo = mc_intLimiteAmarillo
                .scgLimiteVerde = mc_intLimiteVerde
                .scgMostrarValor = True
                .Width = 422
            End With

            objOtorga = New DataGridTextBoxColumn

            With objOtorga
                .HeaderText = My.Resources.ResourceUI.Otorgado
                .MappingName = mc_RenMontosRep_strValorOtorgado
                .ReadOnly = True
                .Width = 90
                .Format = "#,##0.00"
            End With

            objAcumula = New DataGridTextBoxColumn

            With objAcumula
                .HeaderText = My.Resources.ResourceUI.Acumulado '"Acumulado"
                .MappingName = mc_RenMontosRep_strValorAcumulado
                .ReadOnly = True
                .Width = 90
                .Format = "#,##0.00"
            End With

            With objTableStyle

                .GridColumnStyles.Add(objDescripCol)
                .GridColumnStyles.Add(objPorcCol)
                .GridColumnStyles.Add(objOtorga)
                .GridColumnStyles.Add(objAcumula)

                .PreferredRowHeight = 34
                .SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
                .SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                .HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
                .AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
                .RowHeadersVisible = False

            End With


            dtgMontoReparacion.TableStyles.Add(objTableStyle)

        End Sub

        Private Sub EstiloGridRendBarras()
            Dim objTableStyle As DataGridTableStyle
            Dim objDescripCol As DataGridLabelColumn
            Dim objPorcCol As DataGridColumnProgressBar
            Dim objTiempoOtor As DataGridLabelColumn
            Dim objTiempoTaller As DataGridLabelColumn
            Dim objTiempoRest As DataGridLabelColumn


            objTableStyle = New DataGridTableStyle

            objTableStyle.MappingName = mc_RenDurXFase_strTableName

            objDescripCol = New DataGridLabelColumn

            With objDescripCol
                .HeaderText = ""
                .MappingName = mc_RenDurXFase_strDescripcion
                .ReadOnly = True
                .Width = 175
            End With

            objPorcCol = New DataGridColumnProgressBar

            With objPorcCol
                .HeaderText = My.Resources.ResourceUI.Porcentaje
                .MappingName = mc_RenDurXFase_strPorcentaje
                .scgAllowEdit = False
                .scgLimiteAmarillo = mc_intLimiteAmarillo
                .scgLimiteVerde = mc_intLimiteVerde
                .scgMostrarValor = True
                .Width = 280
            End With

            objTiempoOtor = New DataGridLabelColumn

            With objTiempoOtor
                .HeaderText = My.Resources.ResourceUI.DuracionEstandar

                If g_intUnidadTiempo = -1 Then
                    .MappingName = mc_RenDurXFase_strDuracionHorasAprobadas
                Else
                    .MappingName = "HorasAprobadasUnidadTiempo"
                End If


                .ReadOnly = True
                .Width = 92
            End With

            objTiempoTaller = New DataGridLabelColumn

            With objTiempoTaller

                .HeaderText = My.Resources.ResourceUI.DuracionReal '"Tiempo Real"

                If g_intUnidadTiempo = -1 Then
                    .MappingName = mc_RenDurXFase_strCantidadHoraManoObra
                Else
                    .MappingName = "CantidadHoraManoObraUnidadTiempo"
                End If

                .ReadOnly = True
                .Width = 92
            End With

            objTiempoRest = New DataGridLabelColumn

            With objTiempoRest
                .HeaderText = My.Resources.ResourceUI.DuracionRestante '"Tiempo Restante"

                If g_intUnidadTiempo = -1 Then
                    .MappingName = mc_RenDurXFase_strTiempoRestante
                Else
                    .MappingName = "TiempoRestanteUnidadTiempo"
                End If

                .ReadOnly = True
                .Width = 92
            End With

            With objTableStyle

                .GridColumnStyles.Add(objDescripCol)
                .GridColumnStyles.Add(objPorcCol)
                .GridColumnStyles.Add(objTiempoOtor)
                .GridColumnStyles.Add(objTiempoTaller)
                .GridColumnStyles.Add(objTiempoRest)

                .PreferredRowHeight = 41
                .SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
                .SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                .HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
                .AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
                .RowHeadersVisible = False

            End With

            dtgRendimientosBarras.TableStyles.Add(objTableStyle)

        End Sub

#End Region

#Region "Costos"

        Private Sub CalculoCostosCierreOrden(ByVal p_strNoOrden As String, ByVal p_strNoCotizacion As String, ByVal p_intCodTipoOrden As Integer)
            Dim adpCostos As New DMSOneFramework.SCGDataAccess.CostosDataAdapter

            adpCostos.CostosPorCierre(p_strNoOrden, p_strNoCotizacion, g_intCosteoServicios, p_intCodTipoOrden)

        End Sub

        Private Sub CalculoCostosInicioFase(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer)
            Dim adpCostos As New DMSOneFramework.SCGDataAccess.CostosDataAdapter

            adpCostos.CostosPorInicioFase(p_strNoOrden, p_intNoFase)

        End Sub

        Private Sub CalculoCostosDtD(ByRef p_dtbColaboradores As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorDataTable, _
                                        ByRef p_dtbColabModif As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorDataTable)

            Dim adpCostos As New DMSOneFramework.SCGDataAccess.CostosDataAdapter
            Dim dtbColModifAfterUpdate As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorDataTable
            Dim drwColaModif As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorRow
            Dim intID As Integer

            dtbColModifAfterUpdate = (New DMSOneFramework.ColaboradorDataset).SCGTA_TB_ControlColaborador

            For Each drwColaModif In p_dtbColabModif
                If drwColaModif.Estado = mc_Estado_Iniciado Then

                    intID = drwColaModif.ID

                    dtbColModifAfterUpdate.ImportRow(p_dtbColaboradores.FindByID(intID))

                End If
            Next

            If dtbColModifAfterUpdate.Rows.Count <> 0 Then
                adpCostos.CostosPorDtD(dtbColModifAfterUpdate)
            End If

        End Sub

#End Region

#End Region

#Region "Eventos"

#Region "General"

        Private Sub frmOrden_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            Try
                'If Not m_blnBtnCerrarOAceptar Then
                If g_AgregaAdicionales And cboEstadoOrden.SelectedValue <> mc_PriEstado_Finalizada Then
                    'Genera mensaje en SBO para el asesor
                    adpMensajeria.CreaMensajeDMS_SBO_Cotizacion(My.Resources.ResourceUI.MensajeCotizacionActualizada, _
                        My.Resources.ResourceUI.Actualizada, MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)
                End If
                'Me.Close()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub frmOrden_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                CargarUnidadesTiempoGlobales()

                If g_intUnidadTiempo = -1 Then
                    TTDuracionEN.SetToolTip(txtRampaDuracion, My.Resources.ResourceUI.DuracionEN & " " & My.Resources.ResourceUI.Minutos)
                Else
                    TTDuracionEN.SetToolTip(txtRampaDuracion, My.Resources.ResourceUI.DuracionEN & " " & m_strDescripcionUnidadTiempo)
                End If

                g_AgregaAdicionales = False
                m_blnBtnCerrarOAceptar = False

                'Agregado 27/06/06. Alejandra
                dtpFechaCompromiso.Value = objUtilitarios.CargarFechaHoraServidor.Date

                btnSolicitudes.Enabled = g_blnCatalogosExternos
                btnSolicitar.Enabled = g_blnCatalogosExternos

                If Not Me.MdiParent Is Nothing Then
                    Me.MdiParent.Cursor = Cursors.WaitCursor
                End If

                OrdenarTabs()

                IniciarTBProduccionDocs() ''Agregado código para ocultar botones del toolbar

                CargaCompletaOrden()

                PrepararInfoRampas()

                EstiloGridRampas()
                CargarInfoRampas()

                'Agregado 03/07/06. Alejandra
                CargarMenuFases(txtNoOrden.Text)
                ''''''''''''''''''

                CargarGridColaborador(0)

                ControlEstadosOrden()

                'Call EstiloGridRepuestos(dtgRepuestos, mc_strComponenteEtiqueta)

                'Llama para servicios externos
                'Call EstiloGridRepuestos(dtgSE, mc_strServicioExterno)

                Call objUtilitarios.CargarCombos(cboEstadoRep, 29)

                'Call objUtilitarios.CargarCombos(cboEstadoRep2, 29)

                'CargaCombo de Servicios Externos

                'cboEstadoRep2.SelectedIndex = 0

                cbEstadoSE.SelectedIndex = 0

                If Not Me.MdiParent Is Nothing Then
                    Me.MdiParent.Cursor = Cursors.Arrow
                End If

                EstiloGridMontoRepar()

                EstiloGridRendBarras()

                Call CargaGridSuministros1(0)
                chkAdicionalesSE.Checked = True
                chkAdicionalesSu.Checked = True
                chkAdicionalRep.Checked = True
                chkAdicionalAct.Checked = True


                If g_intUnidadTiempo <> -1 Then
                    lblUnidadTiempo.Text = m_strDescripcionUnidadTiempo
                End If

                Call Visualizacion_UDF()

            Catch ex As Exception
                If Not Me.MdiParent Is Nothing Then
                    Me.MdiParent.Cursor = Cursors.Arrow
                End If
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub tbbPrincipal_ButtonClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tbbPrincipal.ButtonClick
            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean
            Dim caso As String = String.Empty
            Dim strEstadoCotizacion As String = String.Empty
            Dim intValidaConfigOTHijas As Integer
            Dim strValidaConfigOTHijas As String = String.Empty
            Dim strValidaRepPendientes As String = String.Empty

            Try
                Me.MdiParent.Cursor = Cursors.WaitCursor

                caso = e.Button.Text

                Select Case caso

                    Case Is = tbbVisita.Text
                        For Each Forma_Nueva In Me.MdiParent.MdiChildren
                            If Forma_Nueva.Name = "frmCtrlVisita" Then
                                blnExisteForm = True
                            End If
                        Next

                        If Not blnExisteForm Then
                            F_objfrmCtrlVisita = New frmDetalleVisita()

                            F_objfrmCtrlVisita = New frmDetalleVisita

                            F_objfrmCtrlVisita.MdiParent = Me.MdiParent

                            F_objfrmCtrlVisita.cargarDatos(m_dtsVisita, txtNoVisita.Text)

                            F_objfrmCtrlVisita.Show()
                        End If

                    Case Is = ttbVehiculo.Text
                        For Each Forma_Nueva In Me.MdiParent.MdiChildren
                            If Forma_Nueva.Name = "frmCtrlInformacionVehiculos" Then
                                blnExisteForm = True
                            End If
                        Next

                        If Not blnExisteForm Then
                            F_objfrmCtrlVehiculo = Nothing
                            F_objfrmCtrlVehiculo = New frmCtrlInformacionVehiculos(frmCtrlInformacionVehiculos.enumModoInsercion.scgModificarPreseleccionado, m_drdVisitaCurrent.IDVehiculo)

                            F_objfrmCtrlVehiculo.MdiParent = Me.MdiParent
                            F_objfrmCtrlVehiculo.Show()
                        End If

                    Case Is = ttbCliente.Text

                        m_objClientes = New frmCtrlInformacionClientes(2, m_drdVisitaCurrent.CardCode)

                        For Each Forma_Nueva In Me.MdiParent.MdiChildren
                            If Forma_Nueva.Name = "frmCtrlInformacionClientes" Then
                                blnExisteForm = True
                            End If
                        Next

                        If Not blnExisteForm Then
                            With m_objClientes
                                .MdiParent = Me.MdiParent
                                .Show()
                            End With

                        End If
                    Case Is = tbbArchivos.Text

                        Dim archivoDigital As FrmArchivoDigital = New FrmArchivoDigital(My.Resources.ResourceUI.TituloArchivosDigitales, "SCGTA_TB_Visita", m_drdOrdenCurrent.NoVisita, g_strTablaArchivosDigitales, SCGDataAccess.DAConexion.strConectionString, 10, GlobalesUI.g_TipoSkin)
                        archivoDigital.StartPosition = FormStartPosition.CenterParent
                        archivoDigital.ShowDialog()

                    Case ttbOrdenesEspeciales.Text

                        Dim objVerificarCotizacionCLS As New CotizacionCLS(G_objCompany)

                        strValidaRepPendientes = Utilitarios.EjecutarConsulta(String.Format("SELECT U_HjaCanPen FROM [@SCGD_CONF_SUCURSAL] with (nolock) WHERE U_Sucurs = '{0}'", G_strIDSucursal), strConexionSBO)

                        'verifica que al menos una linea de la cotizacion este con estado PendienteBodega
                        If Not objVerificarCotizacionCLS.VerificarFilasCotizacionEstadoPendienteBodega(intNumeroCotizacion) AndAlso strValidaRepPendientes = "N" Then

                            objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeNoCreaOTEspecialesPendienteBodega)
                            Exit Select

                        End If

                        For Each Forma_Nueva In Me.MdiParent.MdiChildren
                            If Forma_Nueva.Name = "frmOrdenesEspeciales" Then
                                blnExisteForm = True
                            End If
                        Next

                        If blnExisteForm Then

                            m_objOrdenesEspeciales = Nothing

                        End If


                        'Verifica el valor en Configuracion para validar la creacion de Ordenes Hijas
                        strValidaConfigOTHijas = Utilitarios.EjecutarConsulta(
                                                                 String.Format("SELECT Valor FROM SCGTA_TB_Configuracion with (nolock) where Propiedad = 'ValidaEstadoOTPadre'"),
                                                                 strConexionADO)

                        If Not String.IsNullOrEmpty(strValidaConfigOTHijas) Then intValidaConfigOTHijas = Integer.Parse(strValidaConfigOTHijas)


                        If intValidaConfigOTHijas = 1 Then

                            'Valido el estado de la OT padre para la creacion de la hija
                            strEstadoCotizacion = Utilitarios.EjecutarConsulta(
                                                                     String.Format("SELECT Estado FROM SCGTA_TB_Orden with (nolock) where NoOrden = '{0}'",
                                                                                     m_strNoOrden.ToString.Trim()),
                                                                     strConexionADO)

                            If (strEstadoCotizacion = 1 Or strEstadoCotizacion = 2 Or strEstadoCotizacion = 3) Then

                                m_objOrdenesEspeciales = New frmOrdenesEspeciales(m_strNoOrden, m_drdOrdenCurrent, m_drdVisitaCurrent.CardCode, m_drdOrdenCurrent.CardName, strValidaRepPendientes)

                                With m_objOrdenesEspeciales
                                    .MdiParent = Me.MdiParent
                                    .Show()
                                End With
                            Else
                                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeErrorCreacionOtHijas)
                            End If

                            'Else de validacion de la Configuracion
                        Else
                            m_objOrdenesEspeciales = New frmOrdenesEspeciales(m_strNoOrden, m_drdOrdenCurrent, m_drdVisitaCurrent.CardCode, m_drdOrdenCurrent.CardName, strValidaRepPendientes)

                            With m_objOrdenesEspeciales
                                .MdiParent = Me.MdiParent
                                .Show()
                            End With
                        End If

                End Select

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub btnCerrarFormulario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrarFormulario.Click
            Try
                Me.Close()
            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

            End Try
        End Sub

        Private Sub frmOrden_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
            If Asc(e.KeyChar) = Keys.Escape Then Me.Close()
        End Sub

        Private Sub frmOrden_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed

            F_objfrmCtrlVisita = Nothing
            F_objfrmCtrlVehiculo = Nothing
            frmChild = Nothing
            frmSuspensiones = Nothing
            ObjfrmReprocesos = Nothing

        End Sub

        Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
            Dim blnCerrar As Boolean
            Dim strValidaReqPen As String
            Dim strValidaCanOTConArtApro As String
            Dim dtConfiguracion As DataTable



            Try
                dtConfiguracion = Utilitarios.EjecutarConsultaDataTable(String.Format("Select U_ValReqPen,U_PerCanOT from [@SCGD_CONF_SUCURSAL] with (nolock) where U_Sucurs = '{0}'", G_strIDSucursal), strConexionSBO)

                strValidaCanOTConArtApro = dtConfiguracion.Rows(0)("U_PerCanOT").ToString
                strValidaReqPen = dtConfiguracion.Rows(0)("U_ValReqPen").ToString


                Me.MdiParent.Cursor = Cursors.WaitCursor

                Select Case cboEstadoOrden.Text

                    Case mc_PriEstado_Finalizada

                        If strValidaReqPen = "Y" Then

                            If ValidaRequisicionPendiente() Then

                                blnCerrar = ActualizarOrdenTrabajo()

                                Me.MdiParent.Cursor = Cursors.Arrow

                                If blnCerrar Then
                                    ''m_blnBtnCerrarOAceptar = True
                                    'If g_AgregaAdicionales And cboEstadoOrden.Text <> mc_PriEstado_Finalizada Then
                                    '    'Genera mensaje en SBO para el asesor
                                    '    adpMensajeria.CreaMensajeDMS_SBO_Cotizacion("Cotización actualizada desde la orden de trabajo", MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)
                                    'ElseIf cboEstadoOrden.Text = mc_PriEstado_Finalizada Then
                                    '    'Genera mensaje en SBO para el asesor
                                    '    adpMensajeria.CreaMensajeDMS_SBO_Cotizacion("La orden de trabajo ha sido finalizada", MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)
                                    'End If

                                    'UDFS

                                    VisualizarUDFOrden.UpdateDatosUDF(Me)
                                    VisualizarUDFOrden.LimpiarUDF()

                                    Me.Close()

                                End If

                            Else

                                MessageBox.Show(My.Resources.ResourceUI.RequisicionVal, "SCG DMS One", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            End If

                        Else

                            blnCerrar = ActualizarOrdenTrabajo()

                            Me.MdiParent.Cursor = Cursors.Arrow

                            If blnCerrar Then
                                ''m_blnBtnCerrarOAceptar = True
                                'If g_AgregaAdicionales And cboEstadoOrden.Text <> mc_PriEstado_Finalizada Then
                                '    'Genera mensaje en SBO para el asesor
                                '    adpMensajeria.CreaMensajeDMS_SBO_Cotizacion("Cotización actualizada desde la orden de trabajo", MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)
                                'ElseIf cboEstadoOrden.Text = mc_PriEstado_Finalizada Then
                                '    'Genera mensaje en SBO para el asesor
                                '    adpMensajeria.CreaMensajeDMS_SBO_Cotizacion("La orden de trabajo ha sido finalizada", MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)
                                'End If

                                'UDFS

                                VisualizarUDFOrden.UpdateDatosUDF(Me)
                                VisualizarUDFOrden.LimpiarUDF()

                                Me.Close()

                            End If

                        End If

                    Case mc_PriEstado_Cancelada
                        If strValidaCanOTConArtApro = "Y" Then

                            If ValidaRepuestosCompraRecibidos() Then
                                blnCerrar = ActualizarOrdenTrabajo()

                                Me.MdiParent.Cursor = Cursors.Arrow

                                If blnCerrar Then
                                    ''m_blnBtnCerrarOAceptar = True
                                    'If g_AgregaAdicionales And cboEstadoOrden.Text <> mc_PriEstado_Finalizada Then
                                    '    'Genera mensaje en SBO para el asesor
                                    '    adpMensajeria.CreaMensajeDMS_SBO_Cotizacion("Cotización actualizada desde la orden de trabajo", MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)
                                    'ElseIf cboEstadoOrden.Text = mc_PriEstado_Finalizada Then
                                    '    'Genera mensaje en SBO para el asesor
                                    '    adpMensajeria.CreaMensajeDMS_SBO_Cotizacion("La orden de trabajo ha sido finalizada", MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)
                                    'End If

                                    'UDFS

                                    VisualizarUDFOrden.UpdateDatosUDF(Me)
                                    VisualizarUDFOrden.LimpiarUDF()

                                    Me.Close()

                                End If

                            Else
                                MessageBox.Show(My.Resources.ResourceUI.MsjNoSePuedeCancelarOTArtCompraRec, "SCG DMS One", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            End If




                        Else
                            blnCerrar = ActualizarOrdenTrabajo()

                            Me.MdiParent.Cursor = Cursors.Arrow

                            If blnCerrar Then
                                ''m_blnBtnCerrarOAceptar = True
                                'If g_AgregaAdicionales And cboEstadoOrden.Text <> mc_PriEstado_Finalizada Then
                                '    'Genera mensaje en SBO para el asesor
                                '    adpMensajeria.CreaMensajeDMS_SBO_Cotizacion("Cotización actualizada desde la orden de trabajo", MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)
                                'ElseIf cboEstadoOrden.Text = mc_PriEstado_Finalizada Then
                                '    'Genera mensaje en SBO para el asesor
                                '    adpMensajeria.CreaMensajeDMS_SBO_Cotizacion("La orden de trabajo ha sido finalizada", MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)
                                'End If

                                'UDFS

                                VisualizarUDFOrden.UpdateDatosUDF(Me)
                                VisualizarUDFOrden.LimpiarUDF()

                                Me.Close()

                            End If

                        End If

                    Case Else

                        blnCerrar = ActualizarOrdenTrabajo()

                        Me.MdiParent.Cursor = Cursors.Arrow

                        If blnCerrar Then
                            ''m_blnBtnCerrarOAceptar = True
                            'If g_AgregaAdicionales And cboEstadoOrden.Text <> mc_PriEstado_Finalizada Then
                            '    'Genera mensaje en SBO para el asesor
                            '    adpMensajeria.CreaMensajeDMS_SBO_Cotizacion("Cotización actualizada desde la orden de trabajo", MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)
                            'ElseIf cboEstadoOrden.Text = mc_PriEstado_Finalizada Then
                            '    'Genera mensaje en SBO para el asesor
                            '    adpMensajeria.CreaMensajeDMS_SBO_Cotizacion("La orden de trabajo ha sido finalizada", MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)
                            'End If

                            'UDFS

                            VisualizarUDFOrden.UpdateDatosUDF(Me)
                            VisualizarUDFOrden.LimpiarUDF()

                            Me.Close()
                        End If

                End Select



       

                   


                 


            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub m_objOcupacion_e_SeleccionarOcupacion() Handles m_objOcupacion.e_SeleccionarOcupacion

            Try
                Dim objItemCombo As Object
                If m_objOcupacion.EsDobleClick Then
                    dtpRampaFecha.Value = m_objOcupacion.FechaSeleccionada
                    dtpRampaHora.Value = m_objOcupacion.FechaSeleccionada
                    cboRampas.SelectedText = m_objOcupacion.Rampa

                    For Each objItemCombo In cboRampas.Items
                        If objItemCombo.Descripcion = m_objOcupacion.Rampa Then
                            cboRampas.SelectedItem = objItemCombo
                            Exit For
                        End If
                    Next
                End If

                m_objOcupacion.Close()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub frmOrden_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged

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

        'Private Sub m_objOcupacion_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles m_objOcupacion.FormClosing
        'End Sub

        'Private Sub m_objOcupacion_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_objOcupacion.VisibleChanged
        '    If m_objOcupacion.EsDobleClick Then
        '        dtpRampaFecha.Value = m_objOcupacion.FechaSeleccionada
        '        dtpRampaHora.Value = m_objOcupacion.FechaSeleccionada
        '    End If
        'End Sub


#End Region

#Region "Formularios"

        'Private Sub F_objfrmCtrlVisita_RetornaValores() Handles F_objfrmCtrlVisita.RetornaValores
        '    Try

        '        Me.MdiParent.Cursor = Cursors.WaitCursor

        '        ActualizarOrden()

        '        Me.MdiParent.Cursor = Cursors.Arrow

        '    Catch ex As Exception
        '        Me.MdiParent.Cursor = Cursors.Arrow

        '        clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
        '    End Try
        'End Sub

        Private Sub F_objfrmCtrlVehiculo_RetornaValores(ByRef p_drwVehiculo As VehiculosDataset.SCGTA_VW_VehiculosRow) Handles F_objfrmCtrlVehiculo.RetornaValores
            Try

                Me.MdiParent.Cursor = Cursors.WaitCursor

                ActualizarOrden()
                F_objfrmCtrlVehiculo.Close()
                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

#End Region

#Region "Principal"

        Private Sub btnOcupacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOcupacion.Click

            m_objOcupacion = New frmOcupacionPatio

            m_objOcupacion.MdiParent = Me.MdiParent

            m_objOcupacion.Show()

        End Sub

        Private Sub cboCentroCostoR_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Me.MdiParent.Cursor = Cursors.WaitCursor

                CargarGridSuministros()

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnReporteSuministros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim strParametros As String = ""
            Dim objBLConexion As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                Me.MdiParent.Cursor = Cursors.WaitCursor

                objBLConexion = New DMSOneFramework.SCGDataAccess.DAConexion

                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)
                If txtNoOrden.Text <> "" Then


                    strParametros = strParametros & txtNoOrden.Text.Trim


                    With rptorden
                        .P_BarraTitulo = My.Resources.ResourceUI.rptTituloDocumentoSuministros
                        .P_WorkFolder = PATH_REPORTES
                        .P_Filename = My.Resources.ResourceUI.rptNombreDocumentoSuministros
                        .P_Server = Server
                        .P_DataBase = strDATABASESCG
                        .P_User = UserSCGInternal
                        .P_Password = Password
                        .P_ParArray = strParametros
                    End With

                    rptorden.VerReporte()
                Else
                    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarOT)
                End If

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception
                Me.MdiParent.Cursor = Cursors.Arrow
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnRequisiciones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Try
                Me.MdiParent.Cursor = Cursors.WaitCursor

                CambiarEstiloBotonesSuministros(1)
                CargarGridSuministros()

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnSuministros_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Me.MdiParent.Cursor = Cursors.WaitCursor

                CambiarEstiloBotonesSuministros(3)
                CargarGridSuministros()

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnDevoluciones_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Me.MdiParent.Cursor = Cursors.WaitCursor

                CambiarEstiloBotonesSuministros(2)
                CargarGridSuministros()

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub optsFact_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                If Not IsNothing(Me.MdiParent) Then
                    Me.MdiParent.Cursor = Cursors.WaitCursor
                End If

                CargarGridSuministros()

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally

                If Not IsNothing(Me.MdiParent) Then
                    Me.MdiParent.Cursor = Cursors.Arrow
                End If

            End Try
        End Sub

        Private Sub btnSolicitudes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSolicitudes.Click

            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean
            Dim objfrmSolicitudesXOrden As frmSolicitudesXOrden

            Try

                For Each Forma_Nueva In Me.MdiParent.MdiChildren
                    If Forma_Nueva.Name = "frmSolicitudEspecificos" Then
                        blnExisteForm = True
                    End If
                Next

                If Not blnExisteForm Then
                    objfrmSolicitudesXOrden = New frmSolicitudesXOrden(m_strNoOrden)
                    objfrmSolicitudesXOrden.MdiParent = Me.MdiParent
                    objfrmSolicitudesXOrden.Show()
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

#End Region

#Region "Rendimiento"

        Private Sub tabOrden_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabOrden.SelectedIndexChanged

            Try

                Me.Cursor = Cursors.WaitCursor

                If Not tabOrden.SelectedTab Is Nothing Then

                    If tabOrden.SelectedTab.Name = tabRendimiento.Name Then
                        RendimientoCargar()
                    ElseIf tabOrden.SelectedTab Is tabOtrosGastos Then
                        CargaOtrosGastos()
                    End If


                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
                Me.Cursor = Cursors.Arrow
            End Try
        End Sub

        Private Sub btnActualizar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActualizar.Click
            Try

                Me.Cursor = Cursors.WaitCursor
                RendimientoCargar()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
                Me.Cursor = Cursors.Arrow
            End Try
        End Sub

#End Region

#End Region
        Private Sub objBuscador_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_objBuscador.AppAceptar

            Try

                Select Case sender.name

                    Case "picTecnico"
                        'm_intCodigoTecnico = Arreglo_Campos(0)
                        m_intCodigoTecnico = CType(Arreglo_Campos(0), Integer)
                        txtTecnico.Text = Arreglo_Campos(1)

                End Select

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub



        Private Sub picTecnico_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picTecnico.Click
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador = New Buscador.SubBuscador
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.TituloEmpleados
                m_objBuscador.Titulos = My.Resources.ResourceUI.Cod & "," & My.Resources.ResourceUI.Apellido & "," & My.Resources.ResourceUI.Nombre  '"Codigo, Nombre, Apellido"
                m_objBuscador.Criterios = "empID,firstName, lastName"
                m_objBuscador.Tabla = "SCGTA_VW_OHEM"
                m_objBuscador.Where = ""
                m_objBuscador.Activar_Buscador(sender)

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub




        Private Sub btnOrdenCompraSE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrdenCompraSE.Click

        End Sub


        Private Sub cargarEstadoWeb()
            Try
                Dim strCultura As String
                m_adpEstadoWeb = New EstadoWebDataAdapter()
                m_dstEstadoWeb = New EstadoWebDataset()


                Call m_adpEstadoWeb.FillEstadoWeb(m_dstEstadoWeb)

                strCultura = System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString

                If m_dstEstadoWeb.SCGTA_TB_EstadoWeb.Rows.Count > 0 Then

                    'Se limpia el combo en caso de que traiga valores ya configurados anteriormente.
                    cboEstadoWeb.Items.Clear()


                    For i As Integer = 0 To m_dstEstadoWeb.SCGTA_TB_EstadoWeb.Rows.Count - 1
                        m_drwEstadoWeb = m_dstEstadoWeb.SCGTA_TB_EstadoWeb.Rows(i)

                        If strCultura = "en-US" Then
                            CargarValorComboEstadoWeb(cboEstadoWeb, m_drwEstadoWeb.DescripcionEnUS, m_drwEstadoWeb.IDEstadoWeb, True)
                        Else
                            CargarValorComboEstadoWeb(cboEstadoWeb, m_drwEstadoWeb.Descripcion, m_drwEstadoWeb.IDEstadoWeb, True)
                        End If


                    Next
                End If


                'Funcion que ingresa los valores en el combo.
                ' objUtilitarios.CargarValorCombo(cboEstadoWeb, drd.Item(1), drd.Item(0), True)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            End Try



        End Sub

        Public Sub CargarValorComboEstadoWeb(ByRef p_objCombo As ComboBox, ByVal p_strValorVisible As String, ByVal p_strValorInvisible As String, ByVal blnDerecha As Boolean)

            '-------------------------------------------- Documentacion SCG --------------------------------------------------
            'Sirve para cargar los combos con los valores que se hayan en los datareaders que consultan 
            'la Base de Datos usualmente esta función se manda a llamar desde un ciclo.
            '-----------------------------------------------------------------------------------------------------------------------

            Dim strValor As String

            If blnDerecha Then

                strValor = p_strValorVisible & Space(100) & p_strValorInvisible

            Else

                strValor = p_strValorInvisible & "- " & p_strValorVisible

            End If

            p_objCombo.Items.Add(strValor)




        End Sub


        Private Sub cboEstadoWeb_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEstadoWeb.SelectedIndexChanged
            Try
                Dim strNoOrden As String = String.Empty
                Dim intIDEstadoWeb As Integer = 0

                If Not String.IsNullOrEmpty(txtNoOrden.Text) Then
                    strNoOrden = txtNoOrden.Text
                End If
                If Not String.IsNullOrEmpty(cboEstadoWeb.SelectedItem) Then
                    intIDEstadoWeb = CInt(Busca_Codigo_Texto(cboEstadoWeb.SelectedItem, True))
                End If
                If Not String.IsNullOrEmpty(strNoOrden) And intIDEstadoWeb <> 0 Then
                    ActualizarEstadoWeb(strNoOrden, intIDEstadoWeb)
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' Cancela la Cita ligada a la orden
        ''' </summary>
        ''' <param name="p_strNoSerie"></param>
        ''' <param name="p_strNoCita"></param>
        ''' <param name="p_strValorCancelarCita"></param>
        ''' <param name="p_strDocEntryCita"></param>
        ''' <remarks></remarks>
        Private Sub CancelarCita(ByVal p_strNoSerie As String, ByVal p_strNoCita As String, ByVal p_strValorCancelarCita As String, ByVal p_strDocEntryCita As String)


            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim m_intDocEntry As Integer
            Try

                m_intDocEntry = Convert.ToInt32(p_strDocEntryCita)
                oCompanyService = G_objCompany.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_CIT")
                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("DocEntry", Convert.ToInt32(m_intDocEntry))
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                oGeneralData.SetProperty("U_Estado", p_strValorCancelarCita)
                oGeneralService.Update(oGeneralData)
            Catch ex As Exception
                Throw ex
            End Try
        End Sub


        ''' <summary>
        ''' Valida si la OT Esta ligada a una cita
        ''' </summary>
        ''' <param name="p_strNoOrden"></param>
        ''' <param name="p_strNoSerie"></param>
        ''' <param name="p_strNoCita"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ValidaOrdenLigadaACita(ByVal p_strNoOrden As String, ByRef p_strNoSerie As String, ByRef p_strNoCita As String, ByRef p_strValorcan As String, ByRef p_strDocEntryCita As String)
            Try
                Dim p_strConsultaCitaSerie As String = " Select cit.DocEntry ,oq.U_SCGD_NoCita, oq.U_SCGD_NoSerieCita   from OQUT as oq with(nolock) " +
                                                       " inner join [@SCGD_CITA] as cit with(nolock) on oq.DocEntry = cit.U_Num_Cot " +
                                                       " where oq.U_SCGD_Numero_OT = '{0}'"

                Dim m_strConcultaValorCancel As String = "Select U_CodCitaCancel  from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}' "
                Dim dtDetalleCita As System.Data.DataTable
                Dim m_strCita As String
                Dim m_strSerie As String
                Dim m_strDocEntryCita As String
                '' Dim m_strValorCan As String

                dtDetalleCita = Utilitarios.EjecutarConsultaDataTable(String.Format(p_strConsultaCitaSerie, p_strNoOrden), strConexionSBO)

                If dtDetalleCita.Rows.Count > 0 Then
                    m_strSerie = dtDetalleCita.Rows(0)("U_SCGD_NoSerieCita").ToString()
                    m_strCita = dtDetalleCita.Rows(0)("U_SCGD_NoSerieCita").ToString()
                    m_strDocEntryCita = dtDetalleCita.Rows(0)("DocEntry").ToString()

                    If String.IsNullOrEmpty(m_strSerie) And String.IsNullOrEmpty(m_strCita) Then
                        Return False
                    Else
                        p_strNoSerie = m_strSerie
                        p_strNoCita = m_strCita
                        p_strDocEntryCita = m_strDocEntryCita
                        p_strValorcan = Utilitarios.EjecutarConsulta(String.Format(m_strConcultaValorCancel, G_strIDSucursal), strConexionSBO)

                        Return True

                    End If


                End If

                Return False
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        ''' <summary>
        ''' Actualiza las duraciones en las lineas de la cotizacion
        ''' </summary>
        ''' <param name="sCGTA_TB_ActividadesxOrdenDataTable">Datatable de actividades por orden</param>
        ''' <remarks></remarks>
        Private Sub ActualizaDuracion(ByVal sCGTA_TB_ActividadesxOrdenDataTable As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable,
                                      ByVal strNoCotizacion As String)

            Dim intResults As Integer
            Dim strResults As String
            Dim m_oBuscarCotizacion As SAPbobsCOM.Documents
            Dim m_oLineasCotizacion As SAPbobsCOM.Document_Lines
            Dim drwActividad As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow

            Try

                m_oBuscarCotizacion = G_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

                If m_oBuscarCotizacion.GetByKey(strNoCotizacion) Then
                    m_oLineasCotizacion = m_oBuscarCotizacion.Lines

                    For i As Integer = 0 To m_oLineasCotizacion.Count - 1
                        m_oLineasCotizacion.SetCurrentLine(i)

                        For Each drwActividad In sCGTA_TB_ActividadesxOrdenDataTable

                            If m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = drwActividad.ID Then
                                m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value = drwActividad.Duracion.ToString()
                                Exit For
                            End If

                        Next
                    Next

                    intResults = m_oBuscarCotizacion.Update()
                    If Not m_oBuscarCotizacion Is Nothing Then
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(m_oBuscarCotizacion)
                        m_oBuscarCotizacion = Nothing
                    End If
                    If intResults <> 0 Then

                        strResults = G_objCompany.GetLastErrorDescription

                        Throw New ExceptionsSBO(intResults, strResults)

                    End If

                End If

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub ActualizarEstadoWeb(ByVal p_NoOrden As String, _
                                       ByVal p_IDEstadoWeb As Integer)
            Try
                Dim oAdaptaerOrden As SCGDataAccess.OrdenTrabajoDataAdapter = New SCGDataAccess.OrdenTrabajoDataAdapter
                Dim drdOrdenTrabajo As OrdenTrabajoDataset.SCGTA_TB_OrdenRow
                oAdaptaerOrden.UpdateEstadoWeb(p_NoOrden, p_IDEstadoWeb)

                drdOrdenTrabajo = m_dtsOrden.SCGTA_TB_Orden.FindByNoOrden(m_strNoOrden)
                drdOrdenTrabajo.IDEstadoWeb = p_IDEstadoWeb
            Catch ex As Exception
            End Try
        End Sub

    End Class

End Namespace
