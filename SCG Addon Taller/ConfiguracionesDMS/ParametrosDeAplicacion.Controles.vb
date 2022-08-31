Imports System.Collections.Generic
Imports System.Linq
Imports DMS_Connector.Business_Logic
Imports DMS_Connector.Business_Logic.DataContract.Configuracion.Configuracion_Sucursal
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class ParametrosDeAplicacion : Implements IFormularioSBO, IUsaMenu

#Region "Definiciones"

    Private m_oCompany As SAPbobsCOM.Company
    Private m_oApplication As Application
    Private blnCargaConf As Boolean
    Private _formType As String
    Private _nombreXml As String
    Private _titulo As String
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _applicationSbo As IApplication
    Private _companySbo As ICompany
    Private _idMenu As String
    Private _menuPadre As String
    Private _posicion As Integer
    Private _nombre As String
    Private _strConexion As String
    Private _strDireccionReportes As String
    Private _strUsuarioBD As String
    Private _strContraseñaBD As String

    Private oForm As SAPbouiCOM.Form

    Public FolderGeneral As FolderSBO
    Public FolderSeries As FolderSBO
    Public FolderMensajeria As FolderSBO
    Public FolderImpuestos As FolderSBO
    Public FolderCosteo As FolderSBO
    Public FolderCitas As FolderSBO
    Public FolderAprobaciones As FolderSBO
    Public FolderConOTInt As FolderSBO
    Public FolderAva As FolderSBO
    Public FolderConTipoOrden As FolderSBO

    Public EditTextSerieOfertaCompra As EditTextSBO
    Public EditTextSerieOfertaVenta As EditTextSBO
    Public EditTextSerieOrdenCompra As EditTextSBO
    Public EditTextSerieOrdenVenta As EditTextSBO
    Public EditTextTrasladoAlmacen As EditTextSBO
    Public EditTextDesSerieOfertaCompra As EditTextSBO
    Public EditTextDesSerieOfertaVenta As EditTextSBO
    Public EditTextDesSerieOrdenCompra As EditTextSBO
    Public EditTextDesSerieOrdenVenta As EditTextSBO
    Public EditTextDesTrasladoAlmacen As EditTextSBO
    Public EditTextArtCotizacion As EditTextSBO
    Public EditTextHoraInicio As EditTextSBO
    Public EditTextHoraFin As EditTextSBO

    Public ComboBoxSucursal As ComboBoxSBO
    ' Public CheckBoxUsaDurE As CheckBoxSBO
    Public CheckBoxUsaEquiposTra As CheckBoxSBO

    Public ButtonVerOfertaCompra As ButtonSBO
    Public ButtonVerOfertaVenta As ButtonSBO
    Public ButtonVerOrdenCompra As ButtonSBO
    Public ButtonVerOrdenVenta As ButtonSBO
    Public ButtonVerTrasAlmac As ButtonSBO
    Public ButtonCrear As ButtonSBO
    Public ButtonCancelar As ButtonSBO
    Public ButtonAgregarLinAprob As ButtonSBO
    Public ButtonEliminaLinAprob As ButtonSBO
    Public ButtonAddConBCC As ButtonSBO
    Public ButtonDelConBCC As ButtonSBO
    Public ButtonAddOTInt As ButtonSBO
    Public ButtonDelOTInt As ButtonSBO

    Public ButtonAddTipoOrden As ButtonSBO
    Public ButtonDelTipoOrden As ButtonSBO

    Public EditCboCitaNueva As ComboBoxSBO
    Public EditCboCitaCancelada As ComboBoxSBO
    Public EditCboCitaTardia As ComboBoxSBO
    Public EditCboCitaAnulada As ComboBoxSBO
    Public EditCboTipOTAva As ComboBoxSBO

    Public EditTextImpServicios As EditTextSBO
    Public EditTextImpRepuestos As EditTextSBO
    Public EditTextImpSuministros As EditTextSBO
    Public EditTextImpServExternos As EditTextSBO
    Public EditTextImpGastos As EditTextSBO
    Public EditTextImpRepCompra As EditTextSBO
    Public EditTextImpSECompra As EditTextSBO

    Public EditTextItmAva As EditTextSBO
    Public EditTextItmAvaN As EditTextSBO

    Public EditTextMinutosTarde As EditTextSBO
    Public EditTextHorasTarde As EditTextSBO

    Public EditTextMoneda As EditTextSBO
    Public EditTextSysC As EditTextSBO
    Public EditTextDescC As EditTextSBO
    Public EditTextCtaAcreditaGasto As EditTextSBO
    Public EditTextCtaDebitaGasto As EditTextSBO
    Public EditTextDescCtaAcredita As EditTextSBO
    Public EditTextDescCtaDebita As EditTextSBO
    Public EditTextMonedaGastos As EditTextSBO

    Public EditTextSysD As EditTextSBO
    Public EditTextDescD As EditTextSBO
    Public EditTextCtaDebCosto As EditTextSBO
    Public EditTextDescDebCosto As EditTextSBO
    Public EditTextCtaDotacion As EditTextSBO
    Public EditTextDescCtaDotacion As EditTextSBO
    Public EditTextCtaGastosSE As EditTextSBO
    Public EditTextDescCtaGastosSE As EditTextSBO
    Public EditTextCtaDifPrecioSE As EditTextSBO
    Public EditTextDescCtaDifPrecioSE As EditTextSBO
    Public EditTextCtaCostoBVSE As EditTextSBO
    Public EditTextDescCtaCostoBVSE As EditTextSBO

    Public CheckBoxCosteoManoObra As CheckBoxSBO
    Public CheckBoxUsaFactProvGastos As CheckBoxSBO
    Public CheckBoxUsaAsientoGastos As CheckBoxSBO
    Public CheckBoxUsaSolicitudOTEsp As CheckBoxSBO

    Private Const tablaConfigSucursales = "@SCGD_CONF_SUCURSAL"
    Private Const tablaConfigAprobaciones = "@SCGD_CONF_APROBAC"
    Private Const tablaConfigBodegasCC = "@SCGD_CONF_BODXCC"
    Private Const tablaConfigOTInt = "@SCGD_CONF_OT_INT"
    Private Const tablaConfigTipoOrden = "@SCGD_CONF_TIP_ORDEN"

    Public oMatrizAprobacioens As SAPbouiCOM.Matrix
    Public Const mc_strmtx_Aprobacion As String = "mtx_Aprob"
    Public Const mc_strCol_TipoOT As String = "ColTipOT"

    Public oMatrizBodegasCentroCosto As SAPbouiCOM.Matrix
    Public Const mc_strmtx_BCC As String = "mtxBXCC"

    Public oMatrizConfOTInt As SAPbouiCOM.Matrix
    Public Const mc_strmtx_OTI As String = "mtxOT_Int"

    Public Const mc_str_Repuestos As String = "U_Rep"
    Public Const mc_str_Servicios As String = "U_Ser"
    Public Const mc_str_Suministros As String = "U_Sum"
    Public Const mc_str_ServiciosExternos As String = "U_SE"
    Public Const mc_str_Proceso As String = "U_Pro"
    Public Const mc_str_CentroCosto As String = "U_CC"
    Public Const mc_str_UbicacionDefecto As String = "U_UbiDBP"

    Public Const mc_strCol_Repuestos As String = "col_Rep"
    Public Const mc_strCol_Servicios As String = "Col_Ser"
    Public Const mc_strCol_Suministros As String = "col_Sum"
    Public Const mc_strCol_ServiciosExternos As String = "col_SE"
    Public Const mc_strCol_Proceso As String = "col_Pro"
    Public Const mc_strCol_CentroCosto As String = "col_CC"
    Public Const mc_strCol_IdUbicacionDefecto As String = "colUbiDf"

    Public Const mc_strCol_TipoOTInt As String = "col_tot"
    Public Const mc_strCol_NumCuenta As String = "col_cc"
    Public Const mc_strCol_Tran As String = "col_tra"

    Public Const mc_str_TipoOTInt As String = "U_Tipo_OT"
    Public Const mc_str_NumCuenta As String = "U_NumCuent"
    Public Const mc_str_Tran As String = "U_Tran_Com"

    Public CheckBoxManoObra As CheckBoxSBO
    Public CheckBoxTiempoReal As CheckBoxSBO
    Public CheckBoxTiempoEstandar As CheckBoxSBO
    Public CheckBoxPrecioOfertaVentas As CheckBoxSBO

    Public ChkUsLisPre As CheckBoxSBO
    Public EditTextListPre As EditTextSBO
    Public ChkUsOfeCompra As CheckBoxSBO
    Public ChkUsOrdCompra As CheckBoxSBO
    Public ChkUsFiltroCli As CheckBoxSBO
    Public ChkCitaClienteInactivo As CheckBoxSBO
    Public ChkUsGenOrdEspecial As CheckBoxSBO
    Public ChkValOTCreaEsp As CheckBoxSBO
    Public ChkUsRepuestos As CheckBoxSBO
    Public ChkUsServicios As CheckBoxSBO
    Public ChkUsSuministros As CheckBoxSBO
    Public ChkUsServExt As CheckBoxSBO
    Public ChkUsRequisiciones As CheckBoxSBO
    Public ChkCambiaPrecioTaller As CheckBoxSBO
    Public ChkNoFinOtCantSol As CheckBoxSBO
    Public ChkAsigUnicaMecanico As CheckBoxSBO
    Public ChkAsigTecnicoOT As CheckBoxSBO
    Public ChkValTiempEst As CheckBoxSBO

    Public EditCboUniTiempo As ComboBoxSBO
    Public EditTextCantCopias As EditTextSBO

    Public ChooseFromListPrices As ChooseFromList

    Public CheckBoxCostoSimple As CheckBoxSBO
    Public CheckBoxCostoDetallado As CheckBoxSBO
    Public CheckBoxHijaPendTras As CheckBoxSBO
    Public CheckBoxSolounaLabor As CheckBoxSBO


    Public oMatrizTipoOrden As Matrix
    Public Const mc_strmtx_TipoOrden As String = "mtx_TipOrd"

    Public Const mc_strcolCode As String = "colCode"
    Public Const mc_strcolName As String = "colName"
    Public Const mc_strcolUsaDim As String = "colUsDim"
    Public Const mc_strcolUDmAEM As String = "colUDimAEM"
    Public Const mc_strcolUDmAFP As String = "colUDimAFP"
    Public Const mc_strcolInterna As String = "colInterna"
    Public Const mc_strcolCentCos As String = "colCentCos"
    Public Const mc_strcolUsaLtP As String = "coluspre"

    Public Const mc_str_CodTipoOrden As String = "U_Code"
    Public Const mc_str_NombreTipoOrden As String = "U_Name"
    Public Const mc_str_UsaDimension As String = "U_UsaDim"
    Public Const mc_str_UsaDimensionAEM As String = "U_UsDmAEM"
    Public Const mc_str_UsaDimensionAFP As String = "U_UsDmAFP"
    Public Const mc_str_OTInterna As String = "U_Interna"
    Public Const mc_str_CentroCosto_TipoOrden As String = "U_CodCtCos"



    Private Enum TipoConfiguracionSerie

        OrdenVenta = 1
        OrdenCompra = 2
        OfertaVenta = 3
        OfertaCompra = 4
        InvBodega = 5

    End Enum


#End Region

#Region "Propiedades"
    Public Property FormType() As String Implements IFormularioSBO.FormType
        Get
            Return _formType
        End Get
        Set(ByVal value As String)
            _formType = value
        End Set
    End Property

    Public Property NombreXml() As String Implements IFormularioSBO.NombreXml
        Get
            Return _nombreXml
        End Get
        Set(ByVal value As String)
            _nombreXml = value
        End Set
    End Property

    Public Property Titulo() As String Implements IFormularioSBO.Titulo
        Get
            Return _titulo
        End Get
        Set(ByVal value As String)
            _titulo = value
        End Set
    End Property

    Public Property StrConexion() As String
        Get
            Return _strConexion
        End Get
        Set(ByVal value As String)
            _strConexion = value
        End Set
    End Property

    Public Property StrDireccionReportes() As String
        Get
            Return _strDireccionReportes
        End Get
        Set(ByVal value As String)
            _strDireccionReportes = value
        End Set
    End Property

    Public Property StrUsuarioBD() As String
        Get
            Return _strUsuarioBD
        End Get
        Set(ByVal value As String)
            _strUsuarioBD = value
        End Set
    End Property

    Public Property StrContraseñaBD() As String
        Get
            Return _strContraseñaBD
        End Get
        Set(ByVal value As String)
            _strContraseñaBD = value
        End Set
    End Property

    Public Property FormularioSBO() As IForm Implements IFormularioSBO.FormularioSBO
        Get
            Return _formularioSbo
        End Get
        Set(ByVal value As IForm)
            _formularioSbo = value
        End Set
    End Property

    Public Property Inicializado() As Boolean Implements IFormularioSBO.Inicializado
        Get
            Return _inicializado
        End Get
        Set(ByVal value As Boolean)
            _inicializado = value
        End Set
    End Property

    Public ReadOnly Property ApplicationSBO() As IApplication Implements IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
        End Get
    End Property

    Public ReadOnly Property CompanySBO() As ICompany Implements IFormularioSBO.CompanySBO
        Get
            Return _companySbo
        End Get
    End Property

    Public Property IdMenu() As String Implements IUsaMenu.IdMenu
        Get
            Return _idMenu
        End Get
        Set(ByVal value As String)
            _idMenu = value
        End Set
    End Property

    Public Property MenuPadre() As String Implements IUsaMenu.MenuPadre
        Get
            Return _menuPadre
        End Get
        Set(ByVal value As String)
            _menuPadre = value
        End Set
    End Property

    Public Property Posicion() As Integer Implements IUsaMenu.Posicion
        Get
            Return _posicion
        End Get
        Set(ByVal value As Integer)
            _posicion = value
        End Set
    End Property

    Public Property Nombre() As String Implements IUsaMenu.Nombre
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

#End Region

#Region "Metodos"

    Public Sub New(ByVal application As Application, ByVal companySbo As SAPbobsCOM.Company)
        _companySbo = companySbo
        _applicationSbo = application
        m_oCompany = companySbo
        m_oApplication = application
    End Sub

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

        'Impuestos de Venta
        DirectCast(FormularioSBO.Items.Item("txtImpServ").Specific, SAPbouiCOM.EditText).ChooseFromListUID = DMS_Connector.Helpers.TipodeImpuesto("CFL_ImpSer").ToString.Trim
        DirectCast(FormularioSBO.Items.Item("txtImpRep").Specific, SAPbouiCOM.EditText).ChooseFromListUID = DMS_Connector.Helpers.TipodeImpuesto("CFL_ImpRep").ToString.Trim
        DirectCast(FormularioSBO.Items.Item("txtImpSum").Specific, SAPbouiCOM.EditText).ChooseFromListUID = DMS_Connector.Helpers.TipodeImpuesto("CFL_ImpSum").ToString.Trim
        DirectCast(FormularioSBO.Items.Item("txtImpSeEx").Specific, SAPbouiCOM.EditText).ChooseFromListUID = DMS_Connector.Helpers.TipodeImpuesto("CFL_ImpSeE").ToString.Trim
        DirectCast(FormularioSBO.Items.Item("txtImpGas").Specific, SAPbouiCOM.EditText).ChooseFromListUID = DMS_Connector.Helpers.TipodeImpuesto("CFL_Imp_G").ToString.Trim

        'Impuesto de Compra
        DirectCast(FormularioSBO.Items.Item("txtImpRepC").Specific, SAPbouiCOM.EditText).ChooseFromListUID = DMS_Connector.Helpers.TipodeImpuesto("CFL_ImpReC").ToString.Trim
        DirectCast(FormularioSBO.Items.Item("txtImpSEC").Specific, SAPbouiCOM.EditText).ChooseFromListUID = DMS_Connector.Helpers.TipodeImpuesto("CFL_ImpSEC").ToString.Trim


        If FormularioSBO IsNot Nothing Then

            FormularioSBO.Freeze(True)

            FolderGeneral = New FolderSBO("Folder1", FormularioSBO)
            FolderSeries = New FolderSBO("Folder2", FormularioSBO)
            FolderMensajeria = New FolderSBO("Folder3", FormularioSBO)
            FolderImpuestos = New FolderSBO("Folder4", FormularioSBO)
            FolderCosteo = New FolderSBO("Folder5", FormularioSBO)
            FolderCitas = New FolderSBO("Folder6", FormularioSBO)
            FolderAprobaciones = New FolderSBO("Folder7", FormularioSBO)
            FolderConOTInt = New FolderSBO("Folder8", FormularioSBO)
            FolderAva = New FolderSBO("Folder12", FormularioSBO)
            FolderConTipoOrden = New FolderSBO("Folder9", FormularioSBO)

            ButtonCrear = New ButtonSBO("1", FormularioSBO)

            ButtonVerOfertaCompra = New ButtonSBO("btnVerOfC", FormularioSBO)
            ButtonVerOrdenCompra = New ButtonSBO("btnVerOrC", FormularioSBO)
            ButtonVerOfertaVenta = New ButtonSBO("btnVerOfeV", FormularioSBO)
            ButtonVerOrdenVenta = New ButtonSBO("btnVerOrdV", FormularioSBO)
            ButtonVerTrasAlmac = New ButtonSBO("btnVerInv", FormularioSBO)

            ButtonAgregarLinAprob = New ButtonSBO("btnAdd", FormularioSBO)
            ButtonEliminaLinAprob = New ButtonSBO("btnEli", FormularioSBO)

            ButtonAddConBCC = New ButtonSBO("btnAddCB", FormularioSBO)
            ButtonDelConBCC = New ButtonSBO("btnEliCB", FormularioSBO)

            ButtonAddOTInt = New ButtonSBO("bnAddOTI", FormularioSBO)
            ButtonDelOTInt = New ButtonSBO("bnDelOTI", FormularioSBO)

            ButtonAddTipoOrden = New ButtonSBO("btnAddTipO", FormularioSBO)
            ButtonDelTipoOrden = New ButtonSBO("btnDelTipO", FormularioSBO)

            EditTextSerieOrdenCompra = New EditTextSBO("txtOrdComp", True, tablaConfigSucursales, "U_SerOrC", FormularioSBO)
            EditTextSerieOfertaCompra = New EditTextSBO("txtOfeComp", True, tablaConfigSucursales, "U_SerOfC", FormularioSBO)
            EditTextSerieOrdenVenta = New EditTextSBO("txtOrdVent", True, tablaConfigSucursales, "U_SerOrV", FormularioSBO)
            EditTextSerieOfertaVenta = New EditTextSBO("txtOfrVent", True, tablaConfigSucursales, "U_SerOfV", FormularioSBO)
            EditTextTrasladoAlmacen = New EditTextSBO("txtTrasAlm", True, tablaConfigSucursales, "U_SerInv", FormularioSBO)
            EditTextDesSerieOrdenCompra = New EditTextSBO("txtDOrdCom", True, tablaConfigSucursales, "U_DesSOrC", FormularioSBO)
            EditTextDesSerieOfertaCompra = New EditTextSBO("txtDOfeCom", True, tablaConfigSucursales, "U_DesSOfC", FormularioSBO)
            EditTextDesSerieOrdenVenta = New EditTextSBO("txtDOrdVen", True, tablaConfigSucursales, "U_DesSOrV", FormularioSBO)
            EditTextDesSerieOfertaVenta = New EditTextSBO("txtDOfrVen", True, tablaConfigSucursales, "U_DesSOfV", FormularioSBO)
            EditTextDesTrasladoAlmacen = New EditTextSBO("txtDTrasAl", True, tablaConfigSucursales, "U_DesSInv", FormularioSBO)
            EditTextArtCotizacion = New EditTextSBO("txtArtCot", True, tablaConfigSucursales, "U_ArtCita", FormularioSBO)
            EditTextHoraInicio = New EditTextSBO("txtHoraIni", True, tablaConfigSucursales, "U_HoraInicio", FormularioSBO)
            EditTextHoraFin = New EditTextSBO("txtHoraFin", True, tablaConfigSucursales, "U_HoraFin", FormularioSBO)
            EditTextHorasTarde = New EditTextSBO("txtMinTard", True, tablaConfigSucursales, "U_CantMinTarde", FormularioSBO)
            EditTextMinutosTarde = New EditTextSBO("txtHoraTar", True, tablaConfigSucursales, "U_CantHorasValida", FormularioSBO)

            EditTextImpServicios = New EditTextSBO("txtImpServ", True, tablaConfigSucursales, "U_Imp_Serv", FormularioSBO)
            EditTextImpRepuestos = New EditTextSBO("txtImpRep", True, tablaConfigSucursales, "U_Imp_Repuestos", FormularioSBO)
            EditTextImpServExternos = New EditTextSBO("txtImpSeEx", True, tablaConfigSucursales, "U_Imp_ServExt", FormularioSBO)
            EditTextImpSuministros = New EditTextSBO("txtImpSum", True, tablaConfigSucursales, "U_Imp_Suminis", FormularioSBO)
            EditTextImpGastos = New EditTextSBO("txtImpGas", True, tablaConfigSucursales, "U_Imp_Gastos", FormularioSBO)
            EditTextImpRepCompra = New EditTextSBO("txtImpRepC", True, tablaConfigSucursales, "U_ImpRepCom", FormularioSBO)
            EditTextImpSECompra = New EditTextSBO("txtImpSEC", True, tablaConfigSucursales, "U_ImpSECom", FormularioSBO)

            EditTextItmAva = New EditTextSBO("txtItmAva", True, tablaConfigSucursales, "U_ItmAva", FormularioSBO)
            EditTextItmAvaN = New EditTextSBO("txtItmN", True, tablaConfigSucursales, "U_ItmAvaN", FormularioSBO)

            EditTextMoneda = New EditTextSBO("txtMoneda", True, tablaConfigSucursales, "U_Moneda_C", FormularioSBO)
            EditTextSysC = New EditTextSBO("txtSys_C", True, tablaConfigSucursales, "U_CuentaSys_C", FormularioSBO)
            EditTextDescC = New EditTextSBO("txtDesc_C", True, tablaConfigSucursales, "U_DescCuenta_C", FormularioSBO)
            EditTextCtaAcreditaGasto = New EditTextSBO("txtCtaAcrG", True, tablaConfigSucursales, "U_CtaAcreGast", FormularioSBO)
            EditTextCtaDebitaGasto = New EditTextSBO("txtCtaDebG", True, tablaConfigSucursales, "U_CtaDebGast", FormularioSBO)
            EditTextDescCtaAcredita = New EditTextSBO("txDesCtaAc", True, tablaConfigSucursales, "U_DescCtaAcreGast", FormularioSBO)
            EditTextDescCtaDebita = New EditTextSBO("txDesCtaDe", True, tablaConfigSucursales, "U_DescCtaDebGast", FormularioSBO)
            EditTextMonedaGastos = New EditTextSBO("txtMonGast", True, tablaConfigSucursales, "U_MonDocGastos", FormularioSBO)

            CheckBoxCosteoManoObra = New CheckBoxSBO("chkMO", True, tablaConfigSucursales, "U_CosteoMO_C", FormularioSBO)
            CheckBoxUsaFactProvGastos = New CheckBoxSBO("chxFAGast", True, tablaConfigSucursales, "U_GenFAGastos", FormularioSBO)
            CheckBoxUsaAsientoGastos = New CheckBoxSBO("chxAsGasto", True, tablaConfigSucursales, "U_GenASGastos", FormularioSBO)
            CheckBoxUsaSolicitudOTEsp = New CheckBoxSBO("chkOTEsp", True, tablaConfigSucursales, "U_USolOTEsp", FormularioSBO)

            EditCboCitaAnulada = New ComboBoxSBO("cboAnulaC", FormularioSBO, True, tablaConfigSucursales, "U_CodCitaAnula")
            EditCboCitaCancelada = New ComboBoxSBO("cboCancelC", FormularioSBO, True, tablaConfigSucursales, "U_CodCitaCancel")
            EditCboCitaNueva = New ComboBoxSBO("cboNuevaC", FormularioSBO, True, tablaConfigSucursales, "U_CodCitaNueva")
            EditCboCitaTardia = New ComboBoxSBO("cboTardeC", FormularioSBO, True, tablaConfigSucursales, "U_CodCitaTarde")
            EditCboTipOTAva = New ComboBoxSBO("cboTipoOT", FormularioSBO, True, tablaConfigSucursales, "U_TOTAva")

            ComboBoxSucursal = New ComboBoxSBO("cboSucu", FormularioSBO, True, tablaConfigSucursales, "U_Sucurs")
            'CheckBoxUsaDurE = New CheckBoxSBO("chkDurEst", True, tablaConfigSucursales, "U_UsaDurEC", FormularioSBO)
            CheckBoxUsaEquiposTra = New CheckBoxSBO("chkAgeEqu", True, tablaConfigSucursales, "U_GrpTrabajo", FormularioSBO)

            CheckBoxTiempoReal = New CheckBoxSBO("chkTR_C1", True, tablaConfigSucursales, "U_TiempoReal_C", FormularioSBO)
            CheckBoxTiempoEstandar = New CheckBoxSBO("chkTEst_C1", True, tablaConfigSucursales, "U_TiempoEst_C", FormularioSBO)
            CheckBoxPrecioOfertaVentas = New CheckBoxSBO("chkOFV_C1", True, tablaConfigSucursales, "U_TiempoOFV_C", FormularioSBO)

            ChkUsLisPre = New CheckBoxSBO("chkULiPre", True, tablaConfigSucursales, "U_UseLisPreCli", FormularioSBO)
            EditTextListPre = New EditTextSBO("txt_ListaP", True, tablaConfigSucursales, "U_ListaPrecios", FormularioSBO)
            ChkUsOfeCompra = New CheckBoxSBO("chkUsaOfer", True, tablaConfigSucursales, "U_UsaOfeVenta", FormularioSBO)
            ChkUsOrdCompra = New CheckBoxSBO("chkUsaOrd", True, tablaConfigSucursales, "U_UsaOrdVenta", FormularioSBO)
            ChkUsFiltroCli = New CheckBoxSBO("chkUCliFil", True, tablaConfigSucursales, "U_UseCliFilter", FormularioSBO)
            ChkCitaClienteInactivo = New CheckBoxSBO("chkCiCliIn", True, tablaConfigSucursales, "U_CitCliInac", FormularioSBO)
            ChkUsGenOrdEspecial = New CheckBoxSBO("chkGeOTEsp", True, tablaConfigSucursales, "U_GenOTEsp", FormularioSBO)
            ChkValOTCreaEsp = New CheckBoxSBO("chkVOTEsp", True, tablaConfigSucursales, "U_ValOTCreEsp", FormularioSBO)
            ChkUsRepuestos = New CheckBoxSBO("chkParts", True, tablaConfigSucursales, "U_UseParts", FormularioSBO)
            ChkUsServicios = New CheckBoxSBO("chkServ", True, tablaConfigSucursales, "U_UseServ", FormularioSBO)
            ChkUsSuministros = New CheckBoxSBO("chkSum", True, tablaConfigSucursales, "U_UseSum", FormularioSBO)
            ChkUsServExt = New CheckBoxSBO("chkSE", True, tablaConfigSucursales, "U_UseSE", FormularioSBO)
            ChkUsRequisiciones = New CheckBoxSBO("chkURequis", True, tablaConfigSucursales, "U_Requis", FormularioSBO)
            ChkCambiaPrecioTaller = New CheckBoxSBO("chkCamPre", True, tablaConfigSucursales, "U_CambPreTall", FormularioSBO)
            ChkNoFinOtCantSol = New CheckBoxSBO("chkFiOTSol", True, tablaConfigSucursales, "U_FinOTCanSol", FormularioSBO)
            ChkAsigUnicaMecanico = New CheckBoxSBO("chkAsigUni", True, tablaConfigSucursales, "U_AsigUniMec", FormularioSBO)
            ChkAsigTecnicoOT = New CheckBoxSBO("chkAsTeOT", True, tablaConfigSucursales, "U_AsigTecOT", FormularioSBO)
            ChkValTiempEst = New CheckBoxSBO("chkTiEst", True, tablaConfigSucursales, "U_ValTiemEst", FormularioSBO)
            CheckBoxCostoSimple = New CheckBoxSBO("chkCSp", True, tablaConfigSucursales, "U_CostoSimp", FormularioSBO)
            CheckBoxCostoDetallado = New CheckBoxSBO("chkCDt", True, tablaConfigSucursales, "U_CostoDet", FormularioSBO)
            CheckBoxHijaPendTras = New CheckBoxSBO("chkHiCPe", True, tablaConfigSucursales, "U_HjaCanPen", FormularioSBO)
            CheckBoxSolounaLabor = New CheckBoxSBO("chkSolaUna", True, tablaConfigSucursales, "U_SolaUna", FormularioSBO)

            EditCboUniTiempo = New ComboBoxSBO("cboUnTiemp", FormularioSBO, True, tablaConfigSucursales, "U_UnidadTiemp")
            EditTextCantCopias = New EditTextSBO("txtCopOT", True, tablaConfigSucursales, "U_CopiasOT", FormularioSBO)

            EditTextSysD = New EditTextSBO("txtSys_D", True, tablaConfigSucursales, "U_CtaDebitoMO", FormularioSBO)
            EditTextDescD = New EditTextSBO("txtDesc_D", True, tablaConfigSucursales, "U_DesCtaDebitoMO", FormularioSBO)
            EditTextCtaDebCosto = New EditTextSBO("txtCtaDebC", True, tablaConfigSucursales, "U_CtaDebitoCosto", FormularioSBO)
            EditTextDescDebCosto = New EditTextSBO("txDesCtaCo", True, tablaConfigSucursales, "U_DesCtaDebitoCosto", FormularioSBO)
            EditTextCtaDotacion = New EditTextSBO("txtCtaDota", True, tablaConfigSucursales, "U_CtaDotacionSE", FormularioSBO)
            EditTextDescCtaDotacion = New EditTextSBO("txDesCtaDo", True, tablaConfigSucursales, "U_DesCtaDotacionSE", FormularioSBO)
            EditTextCtaGastosSE = New EditTextSBO("txtCtaGast", True, tablaConfigSucursales, "U_CtaGastosSE", FormularioSBO)
            EditTextDescCtaGastosSE = New EditTextSBO("txDesCtaGa", True, tablaConfigSucursales, "U_DesCtaGastosSE", FormularioSBO)
            EditTextCtaDifPrecioSE = New EditTextSBO("txtCtaDifP", True, tablaConfigSucursales, "U_CtaDifPrecioSE", FormularioSBO)
            EditTextDescCtaDifPrecioSE = New EditTextSBO("txDesCtaDP", True, tablaConfigSucursales, "U_DesCtaDifPrecioSE", FormularioSBO)
            EditTextCtaCostoBVSE = New EditTextSBO("txtctaCOBI", True, tablaConfigSucursales, "U_CtaCostosBVSE", FormularioSBO)
            EditTextDescCtaCostoBVSE = New EditTextSBO("txDesCtaBV", True, tablaConfigSucursales, "U_DesCtaCostosBVSE", FormularioSBO)

            EditTextSerieOrdenCompra.AsignaBinding()
            EditTextSerieOfertaCompra.AsignaBinding()
            EditTextSerieOrdenVenta.AsignaBinding()
            EditTextSerieOfertaVenta.AsignaBinding()
            EditTextTrasladoAlmacen.AsignaBinding()
            EditTextDesSerieOrdenCompra.AsignaBinding()
            EditTextDesSerieOfertaCompra.AsignaBinding()
            EditTextDesSerieOrdenVenta.AsignaBinding()
            EditTextDesSerieOfertaVenta.AsignaBinding()
            EditTextDesTrasladoAlmacen.AsignaBinding()
            EditTextArtCotizacion.AsignaBinding()
            EditTextHoraInicio.AsignaBinding()
            EditTextHoraFin.AsignaBinding()
            EditTextHorasTarde.AsignaBinding()
            EditTextMinutosTarde.AsignaBinding()

            EditTextImpRepuestos.AsignaBinding()
            EditTextImpServExternos.AsignaBinding()
            EditTextImpServicios.AsignaBinding()
            EditTextImpSuministros.AsignaBinding()
            EditTextImpRepCompra.AsignaBinding()
            EditTextImpSECompra.AsignaBinding()

            EditTextMoneda.AsignaBinding()
            EditTextSysC.AsignaBinding()
            EditTextDescC.AsignaBinding()
            EditTextCtaAcreditaGasto.AsignaBinding()
            EditTextCtaDebitaGasto.AsignaBinding()
            EditTextDescCtaAcredita.AsignaBinding()
            EditTextDescCtaDebita.AsignaBinding()
            EditTextMonedaGastos.AsignaBinding()

            EditTextItmAva.AsignaBinding()
            EditTextItmAvaN.AsignaBinding()

            CheckBoxCosteoManoObra.AsignaBinding()
            CheckBoxUsaFactProvGastos.AsignaBinding()
            CheckBoxUsaAsientoGastos.AsignaBinding()
            CheckBoxUsaSolicitudOTEsp.AsignaBinding()

            EditCboCitaAnulada.AsignaBinding()
            EditCboCitaCancelada.AsignaBinding()
            EditCboCitaNueva.AsignaBinding()
            EditCboCitaTardia.AsignaBinding()

            ComboBoxSucursal.AsignaBinding()
            CheckBoxUsaEquiposTra.AsignaBinding()

            CheckBoxTiempoReal.AsignaBinding()
            CheckBoxTiempoEstandar.AsignaBinding()
            CheckBoxPrecioOfertaVentas.AsignaBinding()

            ChkUsLisPre.AsignaBinding()
            EditTextListPre.AsignaBinding()
            ChkUsOfeCompra.AsignaBinding()
            ChkUsOrdCompra.AsignaBinding()
            ChkUsFiltroCli.AsignaBinding()
            ChkCitaClienteInactivo.AsignaBinding()
            ChkUsGenOrdEspecial.AsignaBinding()
            ChkValOTCreaEsp.AsignaBinding()
            ChkUsRepuestos.AsignaBinding()
            ChkUsServicios.AsignaBinding()
            ChkUsSuministros.AsignaBinding()
            ChkUsServExt.AsignaBinding()
            ChkUsRequisiciones.AsignaBinding()
            ChkCambiaPrecioTaller.AsignaBinding()
            ChkNoFinOtCantSol.AsignaBinding()
            ChkAsigUnicaMecanico.AsignaBinding()
            ChkAsigTecnicoOT.AsignaBinding()
            ChkValTiempEst.AsignaBinding()
            CheckBoxCostoSimple.AsignaBinding()
            CheckBoxCostoDetallado.AsignaBinding()
            CheckBoxHijaPendTras.AsignaBinding()
            CheckBoxSolounaLabor.AsignaBinding()

            EditCboUniTiempo.AsignaBinding()
            EditTextCantCopias.AsignaBinding()
            EditCboTipOTAva.AsignaBinding()

            EditTextSysD.AsignaBinding()
            EditTextDescD.AsignaBinding()
            EditTextCtaDebCosto.AsignaBinding()
            EditTextDescDebCosto.AsignaBinding()
            EditTextCtaDotacion.AsignaBinding()
            EditTextDescCtaDotacion.AsignaBinding()
            EditTextCtaGastosSE.AsignaBinding()
            EditTextDescCtaGastosSE.AsignaBinding()
            EditTextCtaDifPrecioSE.AsignaBinding()
            EditTextDescCtaDifPrecioSE.AsignaBinding()
            EditTextCtaCostoBVSE.AsignaBinding()
            EditTextDescCtaCostoBVSE.AsignaBinding()

            FormularioSBO.Items.Item("Folder1").Click()
            FormularioSBO.Freeze(False)
        End If
    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario
        If FormularioSBO IsNot Nothing Then
            FormularioSBO.Freeze(True)
            CargarCombos()
            FormularioSBO.Mode = BoFormMode.fm_FIND_MODE
            FormularioSBO.PaneLevel = 1
            FormularioSBO.Freeze(False)
        End If
    End Sub

    Sub ApplicationSboOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.FormTypeEx <> FormType Then Exit Sub

        Select Case pVal.EventType
            Case BoEventTypes.et_ITEM_PRESSED
                ManejadorEventosItemPressed(FormUID, pVal, BubbleEvent)

            Case BoEventTypes.et_CHOOSE_FROM_LIST
                ManejadorEventoChooseFromList(FormUID, pVal, BubbleEvent)

            Case BoEventTypes.et_FORM_CLOSE
                ManejadorEventoFormClose(FormUID, pVal, BubbleEvent)

        End Select

    End Sub

    Private Function CrearListadoValidValuesImpuestos() As Generic.List(Of Utilitarios.ListadoValidValues)

        Dim oListadoValidValues As New Generic.List(Of Utilitarios.ListadoValidValues)
        Dim oValidValue As Utilitarios.ListadoValidValues

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = My.Resources.Resource.UnidadTiempoHoraValue
        oValidValue.strName = My.Resources.Resource.UnidadTiempoHoraText
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = My.Resources.Resource.UnidadTiempoMinutoValue
        oValidValue.strName = My.Resources.Resource.UnidadTiempoMinutoText
        oListadoValidValues.Add(oValidValue)

        oListadoValidValues.Add(oValidValue)

        Return oListadoValidValues

    End Function

    Private Sub ManejadorEventosItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        FormularioSBO.Freeze(True)

        Select Case pVal.ItemUID

            Case FolderGeneral.UniqueId
                FormularioSBO.PaneLevel = 1

            Case FolderSeries.UniqueId
                FormularioSBO.PaneLevel = 2

            Case FolderMensajeria.UniqueId
                FormularioSBO.PaneLevel = 3

            Case FolderImpuestos.UniqueId
                FormularioSBO.PaneLevel = 4

            Case FolderCosteo.UniqueId
                FormularioSBO.PaneLevel = 6

            Case FolderCitas.UniqueId
                FormularioSBO.PaneLevel = 7

            Case FolderAprobaciones.UniqueId
                FormularioSBO.PaneLevel = 8

            Case FolderConOTInt.UniqueId()
                FormularioSBO.PaneLevel = 10

            Case FolderConTipoOrden.UniqueId()
                FormularioSBO.PaneLevel = 11

            Case FolderAva.UniqueId()
                FormularioSBO.PaneLevel = 12

            Case ButtonCrear.UniqueId
                ButtonCrearItemPressed(FormUID, pVal, BubbleEvent)

            Case ButtonVerOfertaCompra.UniqueId
                ButtonVerOfertaCompraItemPressed(FormUID, pVal, BubbleEvent)

            Case ButtonVerOrdenCompra.UniqueId
                ButtonVerOrdenCompraItemPressed(FormUID, pVal, BubbleEvent)

            Case ButtonVerOfertaVenta.UniqueId
                ButtonVerOfertaVentaItemPressed(FormUID, pVal, BubbleEvent)

            Case ButtonVerOrdenVenta.UniqueId
                ButtonVerOrdenVentaItemPressed(FormUID, pVal, BubbleEvent)

            Case (ButtonVerTrasAlmac.UniqueId)
                ButtonVerBodegaInvItemPressed(FormUID, pVal, BubbleEvent)

            Case (ButtonAgregarLinAprob.UniqueId)
                ButtonAgregarLinAprobItemPressed(FormUID, pVal, BubbleEvent)

            Case (ButtonEliminaLinAprob.UniqueId)
                ButtonEliminarLinAprobItemPressed(FormUID, pVal, BubbleEvent)

            Case (ButtonAddConBCC.UniqueId)
                ButtonAddConfBodxCCItemPressed(FormUID, pVal, BubbleEvent)

            Case (ButtonDelConBCC.UniqueId)
                ButtonDelConfBodxCCItemPressed(FormUID, pVal, BubbleEvent)

            Case (mc_strmtx_Aprobacion)
                MatrizAprobacionItemPressed(FormUID, pVal, BubbleEvent)

            Case ("chkTR_C1")
                If CheckBoxTiempoReal.Especifico.Checked Then

                    CheckBoxTiempoEstandar.Especifico.ValOff = "N"
                    CheckBoxPrecioOfertaVentas.Especifico.ValOff = "N"

                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoReal_C", 0, "Y")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoEst_C", 0, "N")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoOFV_C", 0, "N")

                End If

            Case ("chkTEst_C1")
                If CheckBoxTiempoEstandar.Especifico.Checked Then

                    CheckBoxTiempoReal.Especifico.ValOff = "N"
                    CheckBoxPrecioOfertaVentas.Especifico.ValOff = "N"

                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoEst_C", 0, "Y")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoReal_C", 0, "N")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoOFV_C", 0, "N")

                End If

            Case ("chkOFV_C1")
                If CheckBoxPrecioOfertaVentas.Especifico.Checked Then

                    CheckBoxTiempoEstandar.Especifico.ValOff = "N"
                    CheckBoxTiempoReal.Especifico.ValOff = "N"

                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoOFV_C", 0, "Y")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoEst_C", 0, "N")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoReal_C", 0, "N")

                End If

            Case ("chkMO")
                If CheckBoxCosteoManoObra.Especifico.Checked Then
                    CheckBoxPrecioOfertaVentas.ItemSBO.Enabled = True
                    CheckBoxTiempoEstandar.ItemSBO.Enabled = True
                    CheckBoxTiempoReal.ItemSBO.Enabled = True
                    CheckBoxCostoSimple.ItemSBO.Enabled = True
                    CheckBoxCostoDetallado.ItemSBO.Enabled = True
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoOFV_C", 0, "N")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoEst_C", 0, "N")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoReal_C", 0, "N")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_CostoSimp", 0, "N")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_CostoDet", 0, "N")
                Else
                    CheckBoxPrecioOfertaVentas.ItemSBO.Enabled = False
                    CheckBoxTiempoEstandar.ItemSBO.Enabled = False
                    CheckBoxTiempoReal.ItemSBO.Enabled = False
                    CheckBoxCostoSimple.ItemSBO.Enabled = False
                    CheckBoxCostoDetallado.ItemSBO.Enabled = False
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoOFV_C", 0, "N")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoEst_C", 0, "N")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_TiempoReal_C", 0, "N")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_CostoSimp", 0, "N")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_CostoDet", 0, "N")
                End If
            Case ("chkUsaOfer")
                If pVal.BeforeAction Then
                    FormularioSBO.Freeze(True)
                    If ChkUsOfeCompra.Especifico.Checked Then
                        If ChkUsOrdCompra.Especifico.Checked Then
                            ChkUsOrdCompra.Especifico.Checked = False
                            FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_UsaOrdVenta", 0, "N")
                        End If
                    End If
                    FormularioSBO.Freeze(False)
                End If
            Case ("chkUsaOrd")
                If pVal.BeforeAction Then
                    FormularioSBO.Freeze(True)
                    If ChkUsOrdCompra.Especifico.Checked Then
                        If ChkUsOfeCompra.Especifico.Checked Then
                            ChkUsOfeCompra.Especifico.Checked = False
                            FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_UsaOfeVenta", 0, "N")
                        End If
                    End If
                    FormularioSBO.Freeze(False)
                End If
            Case "btnColor"
                ButtonSeleccionListaPrecios(FormUID, pVal, BubbleEvent)

            Case (ButtonAddOTInt.UniqueId)
                ButtonAddOTIntItemPressed(FormUID, pVal, BubbleEvent)

            Case (ButtonDelOTInt.UniqueId)
                ButtonDelOTIntItemPressed(FormUID, pVal, BubbleEvent)

            Case ("chkCSp")
                If CheckBoxCostoSimple.Especifico.Checked Then

                    CheckBoxCostoDetallado.Especifico.ValOff = "N"
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_CostoSimp", 0, "Y")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_CostoDet", 0, "N")
                End If

            Case ("chkCDt")
                If CheckBoxCostoDetallado.Especifico.Checked Then

                    CheckBoxCostoSimple.Especifico.ValOff = "N"

                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_CostoSimp", 0, "N")
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_CostoDet", 0, "Y")
                End If
            Case ("chkSolaUna")
                If CheckBoxSolounaLabor.Especifico.Checked Then
                    FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).SetValue("U_SolaUna", 0, "Y")
                End If
            Case ButtonAddTipoOrden.UniqueId
                ButtonAddTipOrdentemPressed(FormUID, pVal, BubbleEvent)

            Case ButtonDelTipoOrden.UniqueId
                ButtonDelTipoOrdenItemPressed(FormUID, pVal, BubbleEvent)

            Case mc_strmtx_OTI

                If pVal.ColUID = mc_strCol_TipoOTInt Then
                    If pVal.ActionSuccess = False Then
                        CargarComboTipoOT_PorSucursal()
                    End If


                End If


        End Select
        FormularioSBO.Freeze(False)

    End Sub

    Public Sub ManejadorEventoFormDataLoad(ByRef oTmpForm As SAPbouiCOM.Form)
        Try
            Dim btnExist As Boolean = False

            Dim l_intTopActual As Integer = EditTextListPre.ItemSBO.Top - 3
            Dim l_intLeftActual As Integer = EditTextListPre.ItemSBO.Left + EditTextListPre.ItemSBO.Width + 1
            Dim item As SAPbouiCOM.Item
            For Each item In oTmpForm.Items
                If item.UniqueID = "btnColor" Then
                    btnExist = True
                End If
            Next
            If Not btnExist Then
                Call AgregaButtonPic(oTmpForm, "btnColor", l_intLeftActual, l_intTopActual, 1, 1, SAPbouiCOM.BoButtonTypes.bt_Image, System.Windows.Forms.Application.StartupPath.ToString & "\CFL.BMP", "")
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub


    Public Sub ManejadorEventoChooseFromList(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        Try
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

            Dim intNumeroLinea As Integer = pVal.Row

            If oCFLEvento.BeforeAction = False Then

                Dim oDataTable As SAPbouiCOM.DataTable
                oDataTable = oCFLEvento.SelectedObjects

                If Not oCFLEvento.SelectedObjects Is Nothing Then

                    If Not oDataTable Is Nothing And FormularioSBO.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then

                        Select Case pVal.ItemUID

                            'Impuestos
                            Case EditTextImpServicios.UniqueId
                                AsignaValoresEditTextImpuestos(pVal, oDataTable, EditTextImpServicios)
                            Case EditTextImpRepuestos.UniqueId
                                AsignaValoresEditTextImpuestos(pVal, oDataTable, EditTextImpRepuestos)
                            Case EditTextImpSuministros.UniqueId
                                AsignaValoresEditTextImpuestos(pVal, oDataTable, EditTextImpSuministros)
                            Case EditTextImpServExternos.UniqueId
                                AsignaValoresEditTextImpuestos(pVal, oDataTable, EditTextImpServExternos)
                            Case EditTextImpRepCompra.UniqueId
                                AsignaValoresEditTextImpuestos(pVal, oDataTable, EditTextImpRepCompra)
                            Case EditTextImpSECompra.UniqueId
                                AsignaValoresEditTextImpuestos(pVal, oDataTable, EditTextImpSECompra)
                            Case EditTextImpGastos.UniqueId
                                AsignaValoresEditTextImpuestos(pVal, oDataTable, EditTextImpGastos)

                                'Otros
                            Case EditTextArtCotizacion.UniqueId
                                AsignaValoresEditTextArtCotizacion(pVal, oDataTable)
                            Case EditTextMoneda.UniqueId
                                AsignaValoresMoneda(pVal, oDataTable)
                            Case EditTextMonedaGastos.UniqueId
                                AsignaValoresMonedaGastos(pVal, oDataTable)
                            Case mc_strmtx_BCC
                                AsignaValoresMatrizConfBodegas(pVal, oDataTable)
                            Case EditTextListPre.UniqueId
                                AsignaValoresListaPrecios(pVal, oDataTable)
                            Case mc_strmtx_OTI
                                AsignaValoresMatrizConfOTInt(pVal, oDataTable)
                            Case EditTextItmAva.UniqueId
                                AsignaValoresEditTextItmAva(pVal, oDataTable)

                                'Cuentas
                            Case EditTextCtaAcreditaGasto.UniqueId
                                AsignaValoresCuentasChoosefromList(pVal, oDataTable, EditTextCtaAcreditaGasto, EditTextDescCtaAcredita)
                            Case EditTextCtaDebitaGasto.UniqueId
                                AsignaValoresCuentasChoosefromList(pVal, oDataTable, EditTextCtaDebitaGasto, EditTextDescCtaDebita)
                            Case EditTextSysC.UniqueId
                                AsignaValoresCuentasChoosefromList(pVal, oDataTable, EditTextSysC, EditTextDescC)
                            Case EditTextSysD.UniqueId
                                AsignaValoresCuentasChoosefromList(pVal, oDataTable, EditTextSysD, EditTextDescD)
                            Case EditTextCtaDebCosto.UniqueId
                                AsignaValoresCuentasChoosefromList(pVal, oDataTable, EditTextCtaDebCosto, EditTextDescDebCosto)
                            Case EditTextCtaDotacion.UniqueId
                                AsignaValoresCuentasChoosefromList(pVal, oDataTable, EditTextCtaDotacion, EditTextDescCtaDotacion)
                            Case EditTextCtaGastosSE.UniqueId
                                AsignaValoresCuentasChoosefromList(pVal, oDataTable, EditTextCtaGastosSE, EditTextDescCtaGastosSE)
                            Case EditTextCtaDifPrecioSE.UniqueId
                                AsignaValoresCuentasChoosefromList(pVal, oDataTable, EditTextCtaDifPrecioSE, EditTextDescCtaDifPrecioSE)
                            Case EditTextCtaCostoBVSE.UniqueId
                                AsignaValoresCuentasChoosefromList(pVal, oDataTable, EditTextCtaCostoBVSE, EditTextDescCtaCostoBVSE)
                        End Select

                    End If
                End If
            ElseIf oCFLEvento.BeforeAction = True Then

                Select Case pVal.ItemUID
                    Case EditTextArtCotizacion.UniqueId
                        oConditions = _applicationSbo.CreateObject(BoCreatableObjectType.cot_Conditions)

                        oCondition = oConditions.Add

                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_SCGD_TipoArticulo"
                        oCondition.Operation = BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "1"
                        oCondition.BracketCloseNum = 1
                        oCondition.Relationship = BoConditionRelationship.cr_OR

                        oCondition = oConditions.Add

                        oCondition.BracketOpenNum = 2
                        oCondition.Alias = "U_SCGD_TipoArticulo"
                        oCondition.Operation = BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "2"
                        oCondition.BracketCloseNum = 2
                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR

                        oCondition = oConditions.Add

                        oCondition.BracketOpenNum = 3
                        oCondition.Alias = "U_SCGD_TipoArticulo"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "3"
                        oCondition.BracketCloseNum = 3
                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR

                        oCondition = oConditions.Add

                        oCondition.BracketOpenNum = 4
                        oCondition.Alias = "U_SCGD_TipoArticulo"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "4"
                        oCondition.BracketCloseNum = 4
                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR

                        oCondition = oConditions.Add

                        oCondition.BracketOpenNum = 5
                        oCondition.Alias = "U_SCGD_TipoArticulo"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "5"
                        oCondition.BracketCloseNum = 5
                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR

                        oCondition = oConditions.Add

                        oCondition.BracketOpenNum = 6
                        oCondition.Alias = "U_SCGD_TipoArticulo"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "6"
                        oCondition.BracketCloseNum = 6
                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR

                        oCondition = oConditions.Add

                        oCondition.BracketOpenNum = 7
                        oCondition.Alias = "U_SCGD_TipoArticulo"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "10"
                        oCondition.BracketCloseNum = 7

                        oCFL.SetConditions(oConditions)

                    Case mc_strmtx_BCC

                        If pVal.ColUID = "colUbiDf" Then

                            Dim strAlmacenProceso As String = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CONF_BODXCC").GetValue("U_Pro", intNumeroLinea - 1).Trim()


                            oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                            oCondition = oConditions.Add

                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "WhsCode"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.CondVal = strAlmacenProceso
                            oCondition.BracketCloseNum = 1

                            oCFL.SetConditions(oConditions)

                        End If
                    Case "mtx_TipOrd"
                        If pVal.ColUID = "colClien" Then

                            oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                            oCondition = oConditions.Add

                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "CardType"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.CondVal = "C"
                            oCondition.BracketCloseNum = 1
                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                            oCondition = oConditions.Add

                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "frozenFor"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.CondVal = "N"
                            oCondition.BracketCloseNum = 1

                            oCFL.SetConditions(oConditions)

                        End If

                        'Impuestos de Venta
                    Case EditTextImpServicios.UniqueId, EditTextImpRepuestos.UniqueId, EditTextImpSuministros.UniqueId, EditTextImpServExternos.UniqueId, EditTextImpGastos.UniqueId

                        oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                        oCondition = oConditions.Add()
                        If (DMS_Connector.Configuracion.ParamGenAddon.U_UsaVATGroup = "Y") Then
                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "Category"
                            oCondition.CondVal = "O"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.BracketCloseNum = 1

                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                            oCondition = oConditions.Add
                            oCondition.BracketOpenNum = 2
                            oCondition.Alias = "Locked"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.CondVal = "N"
                            oCondition.BracketCloseNum = 2
                        Else
                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "ValidForAR"
                            oCondition.CondVal = "Y"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.BracketCloseNum = 1

                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                            oCondition = oConditions.Add
                            oCondition.BracketOpenNum = 2
                            oCondition.Alias = "Lock"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.CondVal = "N"
                            oCondition.BracketCloseNum = 2
                        End If
                        oCFL.SetConditions(oConditions)

                        'Impuestos de Compra
                    Case EditTextImpRepCompra.UniqueId, EditTextImpSECompra.UniqueId

                        oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                        oCondition = oConditions.Add()
                        If (DMS_Connector.Configuracion.ParamGenAddon.U_UsaVATGroup = "Y") Then
                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "Category"
                            oCondition.CondVal = "I"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.BracketCloseNum = 1

                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                            oCondition = oConditions.Add
                            oCondition.BracketOpenNum = 2
                            oCondition.Alias = "Locked"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.CondVal = "N"
                            oCondition.BracketCloseNum = 2
                        Else
                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "ValidForAP"
                            oCondition.CondVal = "Y"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.BracketCloseNum = 1

                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                            oCondition = oConditions.Add
                            oCondition.BracketOpenNum = 2
                            oCondition.Alias = "Lock"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.CondVal = "N"
                            oCondition.BracketCloseNum = 2
                        End If
                        oCFL.SetConditions(oConditions)

                End Select
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try
    End Sub

    Private Sub ManejadorEventoFormClose(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        FormularioSBO.Freeze(True)

        FormFormClose(FormUID, pVal, BubbleEvent)

        FormularioSBO.Freeze(False)

    End Sub

    Public Sub CargarCombos()

        Dim ocombo As SAPbouiCOM.ComboBox
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim fcp As SAPbouiCOM.FormCreationParams

        Dim sboItem As SAPbouiCOM.Item
        Dim sboCombo As SAPbouiCOM.ComboBox
        Dim ltValidValues As List(Of Utilitarios.ListadoValidValues)

        sboItem = FormularioSBO.Items.Item(ComboBoxSucursal.UniqueId)
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, "  SELECT ""Code"",""Name"" FROM ""@SCGD_SUCURSALES""")

        ltValidValues = New List(Of Utilitarios.ListadoValidValues)()
        For Each row As DataRow In Utilitarios.EjecutarConsultaDataTable("SELECT ""Code"",""Name"",""U_Descripcion"" FROM ""@SCGD_CITA_ESTADOS""").Rows
            ltValidValues.Add(New Utilitarios.ListadoValidValues() With {
                               .strCode = row.Item(0).ToString,
                               .strName = row.Item(1).ToString,
                               .blnExistente = False})
        Next
        sboItem = FormularioSBO.Items.Item(EditCboCitaNueva.UniqueId)
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, ltValidValues)

        sboItem = FormularioSBO.Items.Item(EditCboCitaCancelada.UniqueId)
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, ltValidValues)

        sboItem = FormularioSBO.Items.Item(EditCboCitaTardia.UniqueId)
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, ltValidValues)

        sboItem = FormularioSBO.Items.Item(EditCboCitaAnulada.UniqueId)
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, ltValidValues)



        ltValidValues.Clear()
        For Each tipoOt As TipoOT In DMS_Connector.Configuracion.TipoOt
            ltValidValues.Add(New Utilitarios.ListadoValidValues() With {
                              .strCode = tipoOt.Code.Trim,
                              .strName = tipoOt.Name.Trim,
                              .blnExistente = False
                              })
        Next


        oMatrix = DirectCast(FormularioSBO.Items.Item(mc_strmtx_Aprobacion).Specific, SAPbouiCOM.Matrix)
        Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strCol_TipoOT).ValidValues, ltValidValues)

        sboItem = FormularioSBO.Items.Item(EditCboTipOTAva.UniqueId)
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, ltValidValues)

        ltValidValues.Clear()
        For Each row As DataRow In Utilitarios.EjecutarConsultaDataTable("Select ""Code"",""Name"" From ""@SCGD_CENTROSCOSTO"" Order by ""Name""").Rows
            ltValidValues.Add(New Utilitarios.ListadoValidValues() With {
                               .strCode = row.Item(0).ToString,
                               .strName = row.Item(1).ToString,
                               .blnExistente = False})
        Next
        oMatrix = DirectCast(FormularioSBO.Items.Item(mc_strmtx_BCC).Specific, SAPbouiCOM.Matrix)
        Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strCol_CentroCosto).ValidValues, ltValidValues)

        oMatrix = DirectCast(FormularioSBO.Items.Item(mc_strmtx_TipoOrden).Specific, SAPbouiCOM.Matrix)
        Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcolCentCos).ValidValues, ltValidValues)


    End Sub

    Public Sub AsignaValoresEditTextItmAva(ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            If pVal.ActionSuccess Then
                EditTextItmAva.AsignaValorDataSource("")
                EditTextItmAvaN.AsignaValorDataSource("")
                EditTextItmAva.AsignaValorDataSource(oDataTable.GetValue("ItemCode", 0))
                EditTextItmAvaN.AsignaValorDataSource(oDataTable.GetValue("ItemName", 0))

                If FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                    FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresEditTextArtCotizacion(ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            EditTextArtCotizacion.AsignaValorDataSource("")
            EditTextArtCotizacion.AsignaValorDataSource(oDataTable.GetValue("ItemCode", 0))

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresMoneda(ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            EditTextMoneda.AsignaValorDataSource("")
            EditTextMoneda.AsignaValorDataSource(oDataTable.GetValue("CurrCode", 0))

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresMonedaGastos(ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            EditTextMonedaGastos.AsignaValorDataSource("")
            EditTextMonedaGastos.AsignaValorDataSource(oDataTable.GetValue("CurrCode", 0))

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresMatrizConfBodegas(ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            oForm = m_oApplication.Forms.Item(pVal.FormUID)
            oMatrizBodegasCentroCosto = DirectCast(oForm.Items.Item(mc_strmtx_BCC).Specific, SAPbouiCOM.Matrix)
            oMatrizBodegasCentroCosto.FlushToDataSource()

            Select Case pVal.ColUID
                Case mc_strCol_Repuestos
                    oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).SetValue(mc_str_Repuestos, pVal.Row - 1, oDataTable.GetValue("WhsCode", 0))
                Case mc_strCol_Servicios
                    oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).SetValue(mc_str_Servicios, pVal.Row - 1, oDataTable.GetValue("WhsCode", 0))
                Case mc_strCol_Suministros
                    oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).SetValue(mc_str_Suministros, pVal.Row - 1, oDataTable.GetValue("WhsCode", 0))
                Case mc_strCol_ServiciosExternos
                    oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).SetValue(mc_str_ServiciosExternos, pVal.Row - 1, oDataTable.GetValue("WhsCode", 0))
                Case mc_strCol_Proceso
                    oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).SetValue(mc_str_Proceso, pVal.Row - 1, oDataTable.GetValue("WhsCode", 0))
                Case "col_Res"
                    oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).SetValue("U_Res", pVal.Row - 1, oDataTable.GetValue("WhsCode", 0))
                Case mc_strCol_IdUbicacionDefecto
                    oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).SetValue(mc_str_UbicacionDefecto, pVal.Row - 1, oDataTable.GetValue("AbsEntry", 0))
            End Select

            oMatrizBodegasCentroCosto.LoadFromDataSource()

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresMatrizConfOTInt(ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            oForm = m_oApplication.Forms.Item(pVal.FormUID)
            oMatrizConfOTInt = DirectCast(oForm.Items.Item(mc_strmtx_OTI).Specific, SAPbouiCOM.Matrix)
            oMatrizConfOTInt.FlushToDataSource()

            Select Case pVal.ColUID
                Case mc_strCol_NumCuenta
                    oForm.DataSources.DBDataSources.Item(tablaConfigOTInt).SetValue(mc_str_NumCuenta, pVal.Row - 1, oDataTable.GetValue("AcctCode", 0))
                Case mc_strCol_Tran
                    oForm.DataSources.DBDataSources.Item(tablaConfigOTInt).SetValue(mc_str_Tran, pVal.Row - 1, oDataTable.GetValue("Code", 0))
            End Select

            oMatrizConfOTInt.LoadFromDataSource()

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresListaPrecios(ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            EditTextListPre.AsignaValorDataSource("")
            EditTextListPre.AsignaValorDataSource(oDataTable.GetValue("ListNum", 0))

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub CargarComboTipoOT_PorSucursal()

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim sboItem As SAPbouiCOM.Item
        Dim sboCombo As SAPbouiCOM.ComboBox
        Dim ltValidValues As List(Of Utilitarios.ListadoValidValues)
        Dim docentry As String

        sboItem = FormularioSBO.Items.Item(ComboBoxSucursal.UniqueId)
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        docentry = FormularioSBO.DataSources.DBDataSources.Item(tablaConfigSucursales).GetValue("DocEntry", 0).Trim()
        ltValidValues = New List(Of Utilitarios.ListadoValidValues)()
        For Each configuracionTipoOrden As Configuracion_Tipo_Orden In DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confS) confS.DocEntry = docentry).Configuracion_Tipo_Orden.Where(Function(tipoO) tipoO.U_Interna.Trim.Equals("Y"))
            ltValidValues.Add(New Utilitarios.ListadoValidValues() With {
                              .strCode = configuracionTipoOrden.U_Code.ToString(),
                              .strName = configuracionTipoOrden.U_Name.Trim(),
                              .blnExistente = False})
        Next

        oMatrix = DirectCast(FormularioSBO.Items.Item(mc_strmtx_OTI).Specific, SAPbouiCOM.Matrix)
        Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strCol_TipoOTInt).ValidValues, ltValidValues)
        oMatrix.Columns.Item(mc_strCol_TipoOTInt).DisplayDesc = True

    End Sub

    Public Sub AsignaValoresCuentasChoosefromList(ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable, ByVal editTextCta As EditTextSBO, ByVal editTextDesc As EditTextSBO)

        Try
            editTextCta.AsignaValorDataSource("")
            editTextCta.AsignaValorDataSource(oDataTable.GetValue("AcctCode", 0))
            editTextDesc.AsignaValorDataSource(oDataTable.GetValue("AcctName", 0))

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresEditTextImpuestos(ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable, ByVal editTextImpuesto As EditTextSBO)

        Try
            editTextImpuesto.AsignaValorDataSource("")
            editTextImpuesto.AsignaValorDataSource(oDataTable.GetValue("Code", 0))

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

#End Region

End Class
