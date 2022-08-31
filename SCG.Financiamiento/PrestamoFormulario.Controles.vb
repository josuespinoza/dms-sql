Imports System.Globalization
Imports System.Threading
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany

Public Delegate Function CargaFormularioPlanDelegate(ByVal form As IFormularioSBO) As Form

'Clase para manejar los controles del formulario de préstamo del modulo de financiamiento

Partial Public Class PrestamoFormulario : Implements IFormularioSBO, IUsaMenu

    Private _cargaFormulario As CargaFormularioPlanDelegate

    Private _formType As String

    Private _nombreXml As String

    Private _titulo As String

    Private _menuPadre As String

    Private _nombreMenu As String

    Private _idMenu As String

    Private _posicion As Integer

    Private _formularioSbo As IForm

    Private _inicializado As Boolean

    Private WithEvents _applicationSbo As Application

    Private _companySbo As ICompany

    Private _formPlanPlagos As PlanPagosFormulario

    Private dataTablePago As DataTable

    Private _strConexion As String

    Private _strDireccionReportes As String

    Private _strUsuarioBD As String

    Private _strContraseñaBD As String

    Private dataTableReversar As DataTable

    Private dataTablePagosAsociados As DataTable

    Private dataTablePagosPendientes As DataTable

    Private dataTableIntereses As DataTable

    Private dataTableDepositos As DataTable
    Private dataTableConsulta As DataTable

    Public EditTextPrestamo As EditTextSBO
    Public EditTextContrato As EditTextSBO
    Public EditTextEstado As EditTextSBO
    Public EditTextMoneda As EditTextSBO
    Public EditTextEnte As EditTextSBO
    Public EditTextCodCliente As EditTextSBO
    Public EditTextDesCliente As EditTextSBO
    Public EditTextCodEmpleado As EditTextSBO
    Public EditTextDesEmpleado As EditTextSBO
    Public EditTextPrecioVenta As EditTextSBO
    Public EditTextMontoFin As EditTextSBO
    Public EditTextIntNormal As EditTextSBO
    Public EditTextPlazo As EditTextSBO
    Public EditTextFecha As EditTextSBO
    Public EditTextDiaPago As EditTextSBO
    Public EditTextIntMora As EditTextSBO
    Public EditTextTipoCuota As EditTextSBO
    Public EditTextAsiento As EditTextSBO
    Public EditTextUnidad As EditTextSBO
    Public EditTextMontoCancelar As EditTextSBO

    Public EditTextNumero As EditTextSBO
    Public EditTextSalIni As EditTextSBO
    Public EditTextFechaPago As EditTextSBO
    Public EditTextMontoAbo As EditTextSBO
    Public EditTextAboCap As EditTextSBO
    Public EditTextAboInt As EditTextSBO
    Public EditTextAboMor As EditTextSBO
    Public EditTextSalFin As EditTextSBO
    Public EditTextCapPend As EditTextSBO
    Public EditTextIntPend As EditTextSBO
    Public EditTextMoraPend As EditTextSBO
    Public EditTextDiasInt As EditTextSBO
    Public EditTextDiasMora As EditTextSBO
    Public EditTextFechaUltimo As EditTextSBO
    Shared EditTextAsientoRevalorizacion As EditTextSBO
    Shared EditTextRecargoCobranza As EditTextSBO

    Public EditTextFeVen As EditTextSBO
    Public EditTextImp As EditTextSBO
    Public ComboBoxPai As ComboBoxSBO
    Public ComboBoxNBan As ComboBoxSBO
    Public ComboBoxSuc As ComboBoxSBO
    Public EditTextCuen As EditTextSBO
    Public EditTextNChe As EditTextSBO
    Public ComboBoxEnd As ComboBoxSBO

    Public CheckBoxDisminucion As CheckBoxSBO
    Public CheckBoxCancelarCobro As CheckBoxSBO
    Public CheckBoxCheque As CheckBoxSBO

    Public ButtonAbonar As ButtonSBO
    Public ButtonCalcular As ButtonSBO
    Public ButtonActualizar As ButtonSBO
    Public ButtonReversar As ButtonSBO
    Public ButtonImprimirPago As ButtonSBO
    Public ButtonImprimirReversados As ButtonSBO

    Public ButtonAgreCheque As ButtonSBO
    Public ButtonActCheque As ButtonSBO
    Public ButtonEliCheque As ButtonSBO
    Public ButtonAplicaCheque As ButtonSBO

    Public BtnRevalorizacion As ButtonSBO

    Public FolderDatosFin As FolderSBO
    Public FolderPagos As FolderSBO
    Public FolderReversion As FolderSBO
    Public FolderCheques As FolderSBO
    Public FolderPlanReal As FolderSBO
    Public FolderPlanTeorico As FolderSBO

    Public MatrixPagosReversar As MatrixSBOPagosReversar

    Public CheckBoxCancelarMora As CheckBoxSBO
    Private CheckBoxPagoDeuda As CheckBoxSBO

    Public n As NumberFormatInfo

    Private m_formPlanPlagos As PlanPagosFormulario

    'Se inicializa el objeto Company y Application de SBO, y se maneja el lenguaje de los formularios de financiamiento

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        _companySbo = companySbo
        _applicationSbo = application
        DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)
        
        n = DIHelper.GetNumberFormatInfo(companySbo)
    End Sub

    Public Property FormType() As String Implements IFormularioSBO.FormType
        Get
            Return _formType
        End Get
        Set(ByVal value As String)
            _formType = value
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

    Public Property CargaFormulario() As CargaFormularioPlanDelegate
        Get
            Return _cargaFormulario
        End Get
        Set(ByVal value As CargaFormularioPlanDelegate)
            _cargaFormulario = value
        End Set
    End Property

    Public Property FormPlanPlagos() As PlanPagosFormulario
        Get
            Return _formPlanPlagos
        End Get
        Set(ByVal value As PlanPagosFormulario)
            _formPlanPlagos = value
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

    Public Property MenuPadre() As String Implements IUsaMenu.MenuPadre
        Get
            Return _menuPadre
        End Get
        Set(ByVal value As String)
            _menuPadre = value
        End Set
    End Property

    Public Property NombreMenu() As String Implements IUsaMenu.Nombre
        Get
            Return _nombreMenu
        End Get
        Set(ByVal value As String)
            _nombreMenu = value
        End Set
    End Property

    Public Property IdMenu() As String Implements IUsaMenu.IdMenu
        Get
            Return _idMenu
        End Get
        Set(ByVal value As String)
            _idMenu = value
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


    'Inicializa los controles de la pantalla de préstamo, DataTables, UserDataSource, y liga de campos y matrices a fuentes de datos

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

        FormularioSBO.Freeze(True)

        dataTablePago = FormularioSBO.DataSources.DataTables.Add("DatosPago")

        dataTablePagosAsociados = FormularioSBO.DataSources.DataTables.Add("PagosAsociados")

        dataTablePagosPendientes = FormularioSBO.DataSources.DataTables.Add("PagosPendientes")

        dataTableIntereses = FormularioSBO.DataSources.DataTables.Add("Intereses")

        dataTableReversar = FormularioSBO.DataSources.DataTables.Add("PagosReversar")

        dataTableDepositos = FormularioSBO.DataSources.DataTables.Add("Depositos")

        'Data Table que contiene configuracion general del financiamiento
        dataTableConsulta = FormularioSBO.DataSources.DataTables.Add("Consulta")

        Dim dataTableListaReversados As DataTable = FormularioSBO.DataSources.DataTables.Add("ReversadosMatrix")
        dataTableListaReversados.Columns.Add("numero", BoFieldsType.ft_Integer, 100)
        dataTableListaReversados.Columns.Add("fecha", BoFieldsType.ft_Date, 100)
        dataTableListaReversados.Columns.Add("cuota", BoFieldsType.ft_Float, 100)
        dataTableListaReversados.Columns.Add("capital", BoFieldsType.ft_Float, 100)
        dataTableListaReversados.Columns.Add("interes", BoFieldsType.ft_Float, 100)
        dataTableListaReversados.Columns.Add("intMora", BoFieldsType.ft_Float, 100)
        dataTableListaReversados.Columns.Add("capPend", BoFieldsType.ft_Float, 100)
        dataTableListaReversados.Columns.Add("intPend", BoFieldsType.ft_Float, 100)
        dataTableListaReversados.Columns.Add("diasInt", BoFieldsType.ft_Integer, 100)
        dataTableListaReversados.Columns.Add("moraPend", BoFieldsType.ft_Float, 100)
        dataTableListaReversados.Columns.Add("diasMora", BoFieldsType.ft_Integer, 100)

        Dim userDataSources As UserDataSources = FormularioSBO.DataSources.UserDataSources
        userDataSources.Add("numero", BoDataType.dt_LONG_NUMBER, 100)
        userDataSources.Add("salInicial", BoDataType.dt_PRICE, 100)
        userDataSources.Add("fecha", BoDataType.dt_DATE, 100)
        userDataSources.Add("cuota", BoDataType.dt_PRICE, 100)
        userDataSources.Add("capital", BoDataType.dt_PRICE, 100)
        userDataSources.Add("intNormal", BoDataType.dt_PRICE, 100)
        userDataSources.Add("intMora", BoDataType.dt_PRICE, 100)
        userDataSources.Add("salFinal", BoDataType.dt_PRICE, 100)
        userDataSources.Add("capPend", BoDataType.dt_PRICE, 100)
        userDataSources.Add("intPend", BoDataType.dt_PRICE, 100)
        userDataSources.Add("moraPend", BoDataType.dt_PRICE, 100)
        userDataSources.Add("diasInt", BoDataType.dt_LONG_NUMBER, 100)
        userDataSources.Add("diasMora", BoDataType.dt_LONG_NUMBER, 100)
        userDataSources.Add("fechaUlt", BoDataType.dt_DATE, 100)
        userDataSources.Add("montoCanc", BoDataType.dt_PRICE, 100)
        userDataSources.Add("recargoCob", BoDataType.dt_PRICE, 100)

        userDataSources.Add("FeVen", BoDataType.dt_DATE, 100)
        userDataSources.Add("Imp", BoDataType.dt_PRICE, 100)
        userDataSources.Add("Pai", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("NBan", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("Suc", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("Cuen", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("NChe", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("End", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("chkPagoTo", BoDataType.dt_LONG_TEXT, 50)

        EditTextPrestamo = New EditTextSBO("txtPrest", True, "@SCGD_PRESTAMO", "DocNum", FormularioSBO)
        EditTextContrato = New EditTextSBO("txtContrat", True, "@SCGD_PRESTAMO", "U_Cont_Ven", FormularioSBO)
        EditTextEstado = New EditTextSBO("txtEstado", True, "@SCGD_PRESTAMO", "U_Des_Est", FormularioSBO)
        EditTextMoneda = New EditTextSBO("txtMoneda", True, "@SCGD_PRESTAMO", "U_Des_Mon", FormularioSBO)
        EditTextEnte = New EditTextSBO("txtEnte", True, "@SCGD_PRESTAMO", "U_Ent_Fin", FormularioSBO)
        EditTextCodCliente = New EditTextSBO("txtCod_Cli", True, "@SCGD_PRESTAMO", "U_Cod_Cli", FormularioSBO)
        EditTextDesCliente = New EditTextSBO("txtDes_Cli", True, "@SCGD_PRESTAMO", "U_Des_Cli", FormularioSBO)
        EditTextCodEmpleado = New EditTextSBO("txtCod_Emp", True, "@SCGD_PRESTAMO", "U_Cod_Emp", FormularioSBO)
        EditTextDesEmpleado = New EditTextSBO("txtDes_Emp", True, "@SCGD_PRESTAMO", "U_Des_Emp", FormularioSBO)
        EditTextPrecioVenta = New EditTextSBO("txtPrecioV", True, "@SCGD_PRESTAMO", "U_Pre_Vta", FormularioSBO)
        EditTextMontoFin = New EditTextSBO("txtMonFin", True, "@SCGD_PRESTAMO", "U_Mon_Fin", FormularioSBO)
        EditTextIntNormal = New EditTextSBO("txtIntNor", True, "@SCGD_PRESTAMO", "U_Interes", FormularioSBO)
        EditTextPlazo = New EditTextSBO("txtPlazo", True, "@SCGD_PRESTAMO", "U_Plazo", FormularioSBO)
        EditTextFecha = New EditTextSBO("txtFecha", True, "@SCGD_PRESTAMO", "U_Fec_Pres", FormularioSBO)
        EditTextDiaPago = New EditTextSBO("txtDiaPago", True, "@SCGD_PRESTAMO", "U_DiaPago", FormularioSBO)
        EditTextIntMora = New EditTextSBO("txtIntMor", True, "@SCGD_PRESTAMO", "U_Int_Mora", FormularioSBO)
        EditTextTipoCuota = New EditTextSBO("txtTipoCuo", True, "@SCGD_PRESTAMO", "U_Des_Tipo", FormularioSBO)
        EditTextAsiento = New EditTextSBO("txtAsiento", True, "@SCGD_PRESTAMO", "U_Asiento", FormularioSBO)
        EditTextUnidad = New EditTextSBO("txtUnidad", True, "@SCGD_PRESTAMO", "U_Cod_Unid", FormularioSBO)

        EditTextNumero = New EditTextSBO("txtNumero", True, "", "numero", FormularioSBO)
        EditTextSalIni = New EditTextSBO("txtSalIni", True, "", "salInicial", FormularioSBO)
        EditTextFechaPago = New EditTextSBO("txtFecPago", True, "", "fecha", FormularioSBO)
        EditTextMontoAbo = New EditTextSBO("txtMontoAb", True, "", "cuota", FormularioSBO)
        EditTextAboCap = New EditTextSBO("txtAboCap", True, "", "capital", FormularioSBO)
        EditTextAboInt = New EditTextSBO("txtAboInt", True, "", "intNormal", FormularioSBO)
        EditTextAboMor = New EditTextSBO("txtAboMor", True, "", "intMora", FormularioSBO)
        EditTextSalFin = New EditTextSBO("txtSalFin", True, "", "salFinal", FormularioSBO)
        EditTextCapPend = New EditTextSBO("txtCapPend", True, "", "capPend", FormularioSBO)
        EditTextIntPend = New EditTextSBO("txtIntPend", True, "", "intPend", FormularioSBO)
        EditTextMoraPend = New EditTextSBO("txtMorPend", True, "", "moraPend", FormularioSBO)
        EditTextDiasInt = New EditTextSBO("txtDiasInt", True, "", "diasInt", FormularioSBO)
        EditTextDiasMora = New EditTextSBO("txtDiasMor", True, "", "diasMora", FormularioSBO)
        EditTextFechaUltimo = New EditTextSBO("txtFeUlPa", True, "", "fechaUlt", FormularioSBO)
        EditTextMontoCancelar = New EditTextSBO("txtMonCanc", True, "", "montoCanc", FormularioSBO)
        EditTextAsientoRevalorizacion = New EditTextSBO("txtAsR", True, "@SCGD_PRESTAMO", "U_AsientoRe", FormularioSBO)
        EditTextRecargoCobranza = New EditTextSBO("txtReCo", True, "", "recargoCob", FormularioSBO)

        EditTextFeVen = New EditTextSBO("txtFeVen", True, "", "FeVen", FormularioSBO)
        EditTextImp = New EditTextSBO("txtImp", True, "", "Imp", FormularioSBO)
        ComboBoxPai = New ComboBoxSBO("cboPai", FormularioSBO, True, "", "Pai")
        ComboBoxNBan = New ComboBoxSBO("cboNBan", FormularioSBO, True, "", "NBan")
        ComboBoxSuc = New ComboBoxSBO("cboSuc", FormularioSBO, True, "", "Suc")
        EditTextCuen = New EditTextSBO("txtCuen", True, "", "Cuen", FormularioSBO)
        EditTextNChe = New EditTextSBO("txtNChe", True, "", "NChe", FormularioSBO)
        ComboBoxEnd = New ComboBoxSBO("cboEnd", FormularioSBO, True, "", "End")

        CheckBoxDisminucion = New CheckBoxSBO("chkModPlaz", True, "@SCGD_PRESTAMO", "U_ModPlazo", FormularioSBO)
        CheckBoxCancelarCobro = New CheckBoxSBO("chkCanMora", True, "@SCGD_PRESTAMO", "U_Can_Mora", FormularioSBO)
        CheckBoxCheque = New CheckBoxSBO("chkCheq", True, "@SCGD_PRESTAMO", "U_Chk", FormularioSBO)
        CheckBoxPagoDeuda = New CheckBoxSBO("chkPagoTo", True, "", "chkPagoTo", FormularioSBO)

        EditTextPrestamo.AsignaBinding()
        EditTextContrato.AsignaBinding()
        EditTextEstado.AsignaBinding()
        EditTextMoneda.AsignaBinding()
        EditTextEnte.AsignaBinding()
        EditTextCodCliente.AsignaBinding()
        EditTextDesCliente.AsignaBinding()
        EditTextCodEmpleado.AsignaBinding()
        EditTextDesEmpleado.AsignaBinding()
        EditTextPrecioVenta.AsignaBinding()
        EditTextMontoFin.AsignaBinding()
        EditTextIntNormal.AsignaBinding()
        EditTextPlazo.AsignaBinding()
        EditTextFecha.AsignaBinding()
        EditTextDiaPago.AsignaBinding()
        EditTextIntMora.AsignaBinding()
        EditTextTipoCuota.AsignaBinding()
        EditTextAsiento.AsignaBinding()
        EditTextUnidad.AsignaBinding()

        EditTextNumero.AsignaBinding()
        EditTextSalIni.AsignaBinding()
        EditTextFechaPago.AsignaBinding()
        EditTextMontoAbo.AsignaBinding()
        EditTextAboCap.AsignaBinding()
        EditTextAboInt.AsignaBinding()
        EditTextAboMor.AsignaBinding()
        EditTextSalFin.AsignaBinding()
        EditTextCapPend.AsignaBinding()
        EditTextIntPend.AsignaBinding()
        EditTextMoraPend.AsignaBinding()
        EditTextDiasInt.AsignaBinding()
        EditTextDiasMora.AsignaBinding()
        EditTextFechaUltimo.AsignaBinding()
        EditTextMontoCancelar.AsignaBinding()
        EditTextAsientoRevalorizacion.AsignaBinding()
        EditTextRecargoCobranza.AsignaBinding()

        EditTextFeVen.AsignaBinding()
        EditTextImp.AsignaBinding()
        ComboBoxPai.AsignaBinding()
        ComboBoxNBan.AsignaBinding()
        ComboBoxSuc.AsignaBinding()
        EditTextCuen.AsignaBinding()
        EditTextNChe.AsignaBinding()
        ComboBoxEnd.AsignaBinding()

        CheckBoxDisminucion.AsignaBinding()
        CheckBoxCancelarCobro.AsignaBinding()
        CheckBoxCheque.AsignaBinding()
        CheckBoxPagoDeuda.AsignaBinding()
        EditTextPrestamo.HabilitarBuscar()
        EditTextContrato.HabilitarBuscar()
        EditTextCodCliente.HabilitarBuscar()

        CheckBoxPagoDeuda.AsignaValorUserDataSource("N")

        ButtonAbonar = New ButtonSBO("btnAbonar", FormularioSBO)
        ButtonCalcular = New ButtonSBO("btnCalcPag", FormularioSBO)
        ButtonActualizar = New ButtonSBO("1", FormularioSBO)
        ButtonReversar = New ButtonSBO("btnReversa", FormularioSBO)
        ButtonImprimirPago = New ButtonSBO("btnImpPago", FormularioSBO)
        ButtonImprimirReversados = New ButtonSBO("btnImpPRev", FormularioSBO)

        ButtonActCheque = New ButtonSBO("btnAct", FormularioSBO)
        ButtonAgreCheque = New ButtonSBO("btnAdd", FormularioSBO)
        ButtonEliCheque = New ButtonSBO("btnEli", FormularioSBO)
        ButtonAplicaCheque = New ButtonSBO("btnApliCh", FormularioSBO)

        BtnRevalorizacion = New ButtonSBO("btnReval", FormularioSBO)

        ButtonAbonar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
        ButtonCalcular.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
        ButtonReversar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
        ButtonImprimirPago.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
        ButtonImprimirReversados.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

        ButtonActCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
        ButtonAgreCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
        ButtonEliCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
        ButtonAplicaCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

        FolderDatosFin = New FolderSBO("Folder1")
        FolderPagos = New FolderSBO("Folder2")
        FolderReversion = New FolderSBO("Folder3")
        FolderCheques = New FolderSBO("Folder4")
        FolderPlanReal = New FolderSBO("Folder5")
        FolderPlanTeorico = New FolderSBO("Folder6")

        MatrixPagosReversar = New MatrixSBOPagosReversar("mtx_Pagos", FormularioSBO, "ReversadosMatrix")
        MatrixPagosReversar.CreaColumnas()
        MatrixPagosReversar.LigaColumnas()

        CheckBoxCancelarMora = New CheckBoxSBO("chkCanMora", FormularioSBO)


        Dim oCombo As SAPbouiCOM.ComboBox
        Dim oMatrix As SAPbouiCOM.Matrix

        General.DBUser = StrUsuarioBD
        General.DBPassword = StrContraseñaBD

        oCombo = DirectCast(FormularioSBO.Items.Item("cboSuc").Specific, SAPbouiCOM.ComboBox)
        General.CargarValidValuesEnCombos(oCombo.ValidValues, " select Code, Name from OUBR ", CompanySBO)

        oCombo = DirectCast(FormularioSBO.Items.Item("cboPai").Specific, SAPbouiCOM.ComboBox)
        General.CargarValidValuesEnCombos(oCombo.ValidValues, " select Code, Name from OCRY ", CompanySBO)

        oCombo = DirectCast(FormularioSBO.Items.Item("cboNBan").Specific, SAPbouiCOM.ComboBox)
        General.CargarValidValuesEnCombos(oCombo.ValidValues, " select BankCode, BankName from ODSC ", CompanySBO)

        oCombo = DirectCast(FormularioSBO.Items.Item("cboEnd").Specific, SAPbouiCOM.ComboBox)
        oCombo.ValidValues.Add("Y", My.Resources.Resource.Si)
        oCombo.ValidValues.Add("N", My.Resources.Resource.No)

        ComboBoxEnd.AsignaValorUserDataSource("N")

        ManejaControlesChequesPostFechados(False, True, True)

        dataTableConsulta.ExecuteQuery(" Select TOP (1) U_AsRe_Loc, U_AsRe_Sis, U_Pago_Men,U_MonMoF,U_MontM,U_Fin_Loc,U_Fin_Sis,U_Gen_As,U_Cuo_Loc,U_Cuo_Sis From [@SCGD_CONF_FINANC] WITH (nolock) ")

        'Dim oMatrix As SAPbouiCOM.Matrix
        'oMatrix = DirectCast(FormularioSBO.Items.Item("mtxChPF").Specific, SAPbouiCOM.Matrix)

        Dim oItem As SAPbouiCOM.Item
        oItem = FormularioSBO.Items.Item("Folder1")
        oItem.Click()

        FormularioSBO.Freeze(False)

        'FormularioSBO.PaneLevel = 1

    End Sub

    'Inicializa el formulario de préstamo, se maneja que cambios en los campos no cambien el modo en que se encuentra el formulario

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario

        Dim oItem As SAPbouiCOM.Item

        m_blnEjecutarMetodo = False

        m_blnCalculadoIntMora = False

        m_blnPermitirMoraMenor = True

        m_strCodPrestRev = String.Empty

        FormularioSBO.Mode = BoFormMode.fm_OK_MODE

        FormularioSBO.PaneLevel = 1

        If FormularioSBO IsNot Nothing Then

            For Each oItem In FormularioSBO.Items

                If Not oItem.UniqueID = "txtIntNor" AndAlso Not oItem.UniqueID = EditTextPrestamo.UniqueId _
                    AndAlso Not oItem.UniqueID = EditTextContrato.UniqueId AndAlso Not oItem.UniqueID = EditTextCodCliente.UniqueId Then

                    oItem.AffectsFormMode = False

                End If

            Next

        End If

    End Sub

    'Carga inicial del préstamo, carga de pago a cancelar, pagos a reversar, monto actual a cancelar, manejo de estado de pantalla

    Public Sub CargarPrestamo(ByVal strPrestamo As String)

        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition

        Dim oItem As SAPbouiCOM.Item
        Dim strTipoCuota As String = ""
        Dim strUnidad As String
        Dim oMatrix As SAPbouiCOM.Matrix

        Try

            If FormularioSBO IsNot Nothing Then

                oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add

                oCondition.Alias = "DocEntry"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = strPrestamo

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").Query(oConditions)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_TEORICO").Query(oConditions)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").Query(oConditions)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").Query(oConditions)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").Query(oConditions)

                If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").Size = 0 Then
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").InsertRecord(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").Size)
                End If
                oMatrix = DirectCast(FormularioSBO.Items.Item("mtxReal").Specific, SAPbouiCOM.Matrix)
                oMatrix.LoadFromDataSource()
                oMatrix = DirectCast(FormularioSBO.Items.Item("mtxTeori").Specific, SAPbouiCOM.Matrix)
                oMatrix.LoadFromDataSource()
                oMatrix = DirectCast(FormularioSBO.Items.Item("mtxChPF").Specific, SAPbouiCOM.Matrix)
                oMatrix.LoadFromDataSource()

                strTipoCuota = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Tipo_Cuo", 0).Trim()

                Call CargarDatosPago(strPrestamo)
                If strTipoCuota <> "1" Then
                    Call CargarPagosReversar(strPrestamo)
                    FormularioSBO.Items.Item("Folder3").Enabled = True
                    FormularioSBO.Items.Item("Folder3").Visible = True
                    manejoEstadoColumnas(False)
                Else
                    FormularioSBO.Items.Item("Folder3").Enabled = False
                    FormularioSBO.Items.Item("Folder3").Visible = False
                    manejoEstadoColumnas(True)
                End If

                Call CargarMontoActualCancelar(strPrestamo)
                Call ManejarEstadoPrestamo()
                
                If strTipoCuota = "3" Then
                    FormularioSBO.Items.Item("chkModPlaz").Enabled = False
                    EditTextMontoAbo.ItemSBO.Enabled = False
                End If

                If strTipoCuota = "1" Then
                    FormularioSBO.ActiveItem = "txtPrest"
                    EditTextIntNormal.ItemSBO.Enabled = False
                End If

                strUnidad = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Cod_Unid", 0)
                strUnidad = strUnidad.Trim()

                If strUnidad = My.Resources.Resource.Multiples Then
                    FormularioSBO.Items.Item("lkUnidad").Visible = False
                Else
                    FormularioSBO.Items.Item("lkUnidad").Visible = True
                End If

                m_dtFechaPagoCalculo = Nothing

                m_strCodPrestRev = String.Empty

                If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Reval", 0).Trim() = "Y" Then
                    BtnRevalorizacion.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                Else
                    BtnRevalorizacion.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                End If

                m_blnEjecutarMetodo = False

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    ''' <summary>
    ''' Cargar los datos de pago para el préstamo
    ''' </summary>
    ''' <param name="strPrestamo">DocEntry del Préstamo</param>
    ''' <remarks></remarks>
    Private Sub CargarDatosPago(ByVal strPrestamo As String)

        Dim strConsulta As String
        Dim strFechaReal As String
        Dim dtFechaReal As Date = Nothing
        Dim strNumero As String
        Dim decSaldoFinal As Decimal = 0
        Dim intNumero As Integer = 0
        Dim strFechaUltimo As String = ""
        Dim dtFechaUltimo As Date
        Dim strNumUltimoPago As String

        Dim decSaldoInicial As Decimal
        Dim decCuota As Decimal
        Dim decCapital As Decimal
        Dim decInteres As Decimal
        Dim decMora As Decimal
        Dim decCapPend As Decimal
        Dim decIntPend As Decimal
        Dim decMoraPend As Decimal

        Dim n As NumberFormatInfo

        Try

            n = DIHelper.GetNumberFormatInfo(CompanySBO)

            dataTablePago = FormularioSBO.DataSources.DataTables.Item("DatosPago")

            strConsulta =
                String.Format(
                    "SELECT TOP 1 U_Numero,U_Fecha,U_Sal_Ini,U_Cuota,U_Capital,U_Interes,U_Int_Mora,U_Sal_Fin,U_Cred_Cap,U_Cap_Pend,U_Int_Pend,U_Mor_Pend,U_Dias_Int,U_Dias_Mor " & _
                    " FROM [dbo].[@SCGD_PLAN_REAL] WHERE U_Pagado in ('N','P') AND DocEntry = '{0}' ORDER BY U_Numero",
                    strPrestamo.Trim())

            dataTablePago.ExecuteQuery(strConsulta)

            strNumero = dataTablePago.GetValue("U_Numero", 0)

            If Not strNumero = "0" Then
                If Not String.IsNullOrEmpty(strNumero) Then intNumero = Integer.Parse(strNumero)
                If intNumero > 1 Then
                    strNumUltimoPago =
                        General.EjecutarConsulta(
                            String.Format("SELECT TOP 1 U_Numero FROM [@SCGD_PLAN_REAL] WHERE U_Pagado='Y' AND DocEntry = '{0}' AND U_Cuota > 0 ORDER BY U_Numero DESC",
                                          strPrestamo),
                                      StrConexion)
                    If Not String.IsNullOrEmpty(strNumUltimoPago) Then
                        strFechaUltimo =
                            General.EjecutarConsulta(
                                String.Format("SELECT U_Fecha FROM [@SCGD_PLAN_REAL] WHERE DocEntry='{0}' AND U_Numero='{1}'",
                                              strPrestamo, strNumUltimoPago),
                                StrConexion)
                        If Not String.IsNullOrEmpty(strFechaUltimo) Then dtFechaUltimo = Date.Parse(strFechaUltimo)
                    End If
                End If

                strFechaReal = dataTablePago.GetValue("U_Fecha", 0)
                If Not String.IsNullOrEmpty(strFechaReal) Then dtFechaReal = Date.Parse(strFechaReal)

                If Not String.IsNullOrEmpty(strFechaUltimo) Then
                    EditTextFechaUltimo.AsignaValorUserDataSource(dtFechaUltimo.ToString("yyyyMMdd"))
                ElseIf String.IsNullOrEmpty(strFechaUltimo) Then
                    EditTextFechaUltimo.AsignaValorUserDataSource("")
                End If

                EditTextNumero.AsignaValorUserDataSource(strNumero)
                If Not dtFechaReal = Nothing Then EditTextFechaPago.AsignaValorUserDataSource(dtFechaReal.ToString("yyyyMMdd"))

                decSaldoInicial = dataTablePago.GetValue("U_Sal_Ini", 0)
                decCuota = dataTablePago.GetValue("U_Cuota", 0)
                decCapital = dataTablePago.GetValue("U_Capital", 0)
                decInteres = dataTablePago.GetValue("U_Interes", 0)
                decMora = dataTablePago.GetValue("U_Int_Mora", 0)
                decCapPend = dataTablePago.GetValue("U_Cap_Pend", 0)
                decIntPend = dataTablePago.GetValue("U_Int_Pend", 0)
                decMoraPend = dataTablePago.GetValue("U_Mor_Pend", 0)

                EditTextSalIni.AsignaValorUserDataSource(decSaldoInicial.ToString(n))
                EditTextMontoAbo.AsignaValorUserDataSource(decCuota.ToString(n))
                EditTextAboCap.AsignaValorUserDataSource(decCapital.ToString(n))
                EditTextAboInt.AsignaValorUserDataSource(decInteres.ToString(n))
                EditTextAboMor.AsignaValorUserDataSource(decMora.ToString(n))
                EditTextCapPend.AsignaValorUserDataSource(decCapPend.ToString(n))
                EditTextIntPend.AsignaValorUserDataSource(decIntPend.ToString(n))
                EditTextMoraPend.AsignaValorUserDataSource(decMoraPend.ToString(n))
                EditTextDiasInt.AsignaValorUserDataSource(dataTablePago.GetValue("U_Dias_Int", 0))
                EditTextDiasMora.AsignaValorUserDataSource(dataTablePago.GetValue("U_Dias_Mor", 0))
                decSaldoFinal = dataTablePago.GetValue("U_Sal_Fin", 0)
                If decSaldoFinal < 0 Then decSaldoFinal = 0
                EditTextSalFin.AsignaValorUserDataSource(decSaldoFinal.ToString(n))
                EditTextRecargoCobranza.AsignaValorUserDataSource(0)
                ButtonAbonar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                ButtonCalcular.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                ButtonImprimirPago.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                ButtonImprimirReversados.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

            End If

            FormularioSBO.Mode = BoFormMode.fm_OK_MODE

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Calculo de monto actual a cancelar según fecha de sistema, calculo de interes y total de capital y otros pendientes de abonar

    Private Sub CargarMontoActualCancelar(ByVal strPrestamo As String)

        Dim strCapital As String
        Dim decCapital As Decimal
        Dim dtFechaActual As Date
        Dim strFechaPagoCancelado As String = ""
        Dim dtFechaPagoCancelado As Date
        Dim strTipoCuota As String
        Dim strNumeroPago As String
        Dim intNumeroPago As Integer
        Dim intDiasInt As Integer
        Dim decInteres As Decimal = 0
        Dim strTasaIntNormal As String
        Dim decTasaIntNormal As Decimal
        Dim strIntPend As String
        Dim decIntPend As Decimal = 0
        Dim strMoraPend As String
        Dim decMoraPend As Decimal = 0
        Dim strConsulta As String
        Dim strFechaPagoPend As String
        Dim dtFechaPagoPend As Date
        Dim intDiasMora As Integer
        Dim strTasaMora As String
        Dim decTasaMora As Decimal
        Dim strCuotaPagoPend As String
        Dim decCuotaPagoPend As Decimal
        Dim strDiasIntPagoPend As String
        Dim intDiasIntPagoPend As Integer
        Dim decMoraPagoPend As Decimal = 0
        Dim decMoratorios As Decimal = 0
        Dim decMontoCancelar As Decimal = 0
        Dim strInteresPagos As String
        Dim decInteresPagos As Decimal

        Dim n As NumberFormatInfo

        Try

            n = DIHelper.GetNumberFormatInfo(CompanySBO)
            If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Tipo_Cuo", 0).Trim() <> "1" Then
                strCapital =
                General.EjecutarConsulta(
                    String.Format("Select TOP 1 U_Sal_Ini From [@SCGD_PLAN_REAL] Where DocEntry = '{0}' And U_Pagado in ('N','P') ORDER BY U_Numero",
                                  strPrestamo),
                              StrConexion)
            Else
                strCapital =
                General.EjecutarConsulta(
                    String.Format("Select TOP 1 U_Sal_Fin + U_Capital From [@SCGD_PLAN_REAL] Where DocEntry = '{0}' And U_Pagado in ('N','P') ORDER BY U_Numero",
                                  strPrestamo),
                              StrConexion)
            End If


            If Not String.IsNullOrEmpty(strCapital) Then

                decCapital = Decimal.Parse(strCapital)

                strTasaIntNormal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Interes", 0).Trim()
                If Not String.IsNullOrEmpty(strTasaIntNormal) Then
                    decTasaIntNormal = Decimal.Parse(strTasaIntNormal, n)
                    decTasaIntNormal = decTasaIntNormal / 100
                End If

                strTipoCuota = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Tipo_Cuo", 0).Trim()

                dtFechaActual = Now.Date

                If strTipoCuota = "2" Then

                    strFechaPagoCancelado =
                        General.EjecutarConsulta(
                            String.Format("Select TOP 1 U_Fecha From [@SCGD_PLAN_REAL] Where DocEntry = '{0}' And U_Pagado = 'Y' And U_Cuota > 0 ORDER BY U_Numero DESC",
                                          strPrestamo),
                                      StrConexion)

                    If Not String.IsNullOrEmpty(strFechaPagoCancelado) Then
                        dtFechaPagoCancelado = Date.Parse(strFechaPagoCancelado)
                    ElseIf String.IsNullOrEmpty(strFechaPagoCancelado) Then
                        strFechaPagoCancelado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Fec_Pres", 0).Trim()
                        If Not String.IsNullOrEmpty(strFechaPagoCancelado) Then dtFechaPagoCancelado = Date.ParseExact(strFechaPagoCancelado, "yyyyMMdd", Nothing)
                    End If

                    If dtFechaActual > dtFechaPagoCancelado Then
                        Call DeterminarDiasEntrePagos(dtFechaActual, dtFechaPagoCancelado, intDiasInt)
                    ElseIf dtFechaActual <= dtFechaPagoCancelado Then
                        intDiasInt = 0
                    End If

                    decInteres = ((decCapital * decTasaIntNormal) / 360) * intDiasInt

                ElseIf strTipoCuota = "1" OrElse strTipoCuota = "3" OrElse strTipoCuota = "4" Then

                    strNumeroPago =
                        General.EjecutarConsulta(
                            String.Format("Select TOP 1 U_Numero From [@SCGD_PLAN_REAL] Where DocEntry = '{0}' And U_Pagado = 'Y' And U_Cuota > 0 ORDER BY U_Numero DESC",
                                          strPrestamo),
                                      StrConexion)

                    If Not String.IsNullOrEmpty(strNumeroPago) Then

                        intNumeroPago = Integer.Parse(strNumeroPago)
                        strFechaPagoCancelado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_TEORICO").GetValue("U_Fecha", intNumeroPago - 1).Trim()

                        If Not String.IsNullOrEmpty(strFechaPagoCancelado) Then dtFechaPagoCancelado = Date.ParseExact(strFechaPagoCancelado, "yyyyMMdd", Nothing)

                    ElseIf String.IsNullOrEmpty(strNumeroPago) Then

                        strFechaPagoCancelado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Fec_Pres", 0).Trim()
                        If Not String.IsNullOrEmpty(strFechaPagoCancelado) Then dtFechaPagoCancelado = Date.ParseExact(strFechaPagoCancelado, "yyyyMMdd", Nothing)

                    End If

                    dataTableIntereses = FormularioSBO.DataSources.DataTables.Item("Intereses")
                    dataTableIntereses.Clear()

                    strConsulta =
                        String.Format(
                            "Select U_Interes From [@SCGD_PLAN_REAL] Where DocEntry = '{0}' And U_Pagado in ('N','P') And U_Fecha > '{1}' And U_Fecha <= '{2}'",
                            strPrestamo, dtFechaPagoCancelado.ToString("yyyyMMdd"), dtFechaActual.ToString("yyyyMMdd"))

                    dataTableIntereses.ExecuteQuery(strConsulta)

                    For i As Integer = 0 To dataTableIntereses.Rows.Count - 1

                        strInteresPagos = dataTableIntereses.GetValue("U_Interes", i)
                        If Not String.IsNullOrEmpty(strInteresPagos) Then decInteresPagos = Decimal.Parse(strInteresPagos)

                        decInteres += decInteresPagos

                    Next

                End If

                strIntPend =
                    General.EjecutarConsulta(
                        String.Format("Select TOP 1 U_Int_Pend From [@SCGD_PLAN_REAL] Where DocEntry = '{0}' And U_Pagado in ('N','P') ORDER BY U_Numero",
                                      strPrestamo),
                                  StrConexion)
                If Not String.IsNullOrEmpty(strIntPend) Then decIntPend = Decimal.Parse(strIntPend)

                strMoraPend =
                    General.EjecutarConsulta(
                        String.Format("Select TOP 1 U_Mor_Pend From [@SCGD_PLAN_REAL] Where DocEntry = '{0}' And U_Pagado in ('N','P') ORDER BY U_Numero",
                                                 strPrestamo),
                                             StrConexion)
                If Not String.IsNullOrEmpty(strMoraPend) Then decMoraPend = Decimal.Parse(strMoraPend)

                'Moratorios
                strTasaMora = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Int_Mora", 0).Trim()
                If Not String.IsNullOrEmpty(strTasaMora) Then
                    decTasaMora = Decimal.Parse(strTasaMora, n)
                    decTasaMora = decTasaMora / 100
                End If

                dataTablePagosPendientes = FormularioSBO.DataSources.DataTables.Item("PagosPendientes")
                dataTablePagosPendientes.Clear()
                If strTipoCuota <> "1" Then
                    strConsulta =
                    String.Format("Select U_Fecha, U_Cuota, U_Dias_Int, U_Int_Mora From [@SCGD_PLAN_REAL] Where DocEntry = '{0}' And U_Pagado in ('N','P') And U_Fecha < '{1}'",
                                  strPrestamo, dtFechaActual.ToString("yyyyMMdd"))
                Else
                    strConsulta =
                    String.Format("Select U_Fecha, U_Capital AS U_Cuota, U_Dias_Int, U_Int_Mora From [@SCGD_PLAN_REAL] Where DocEntry = '{0}' And U_Pagado in ('N','P') And U_Fecha < '{1}'",
                                  strPrestamo, dtFechaActual.ToString("yyyyMMdd"))
                End If


                dataTablePagosPendientes.ExecuteQuery(strConsulta)

                For i As Integer = 0 To dataTablePagosPendientes.Rows.Count - 1

                    strFechaPagoPend = dataTablePagosPendientes.GetValue("U_Fecha", i)

                    If Not String.IsNullOrEmpty(strFechaPagoPend) Then

                        dtFechaPagoPend = Date.Parse(strFechaPagoPend)

                        Call DeterminarDiasEntrePagos(dtFechaActual, dtFechaPagoPend, intDiasMora)

                        strCuotaPagoPend = dataTablePagosPendientes.GetValue("U_Cuota", i)
                        If Not String.IsNullOrEmpty(strCuotaPagoPend) Then decCuotaPagoPend = Decimal.Parse(strCuotaPagoPend)

                        strDiasIntPagoPend = dataTablePagosPendientes.GetValue("U_Dias_Int", i)
                        If Not String.IsNullOrEmpty(strDiasIntPagoPend) Then intDiasIntPagoPend = Integer.Parse(strDiasIntPagoPend)

                        If intDiasIntPagoPend > 0 Then
                            decMoraPagoPend = ((decCuotaPagoPend * decTasaMora) / intDiasIntPagoPend) * intDiasMora
                            decMoraPagoPend += dataTablePagosPendientes.GetValue("U_Int_Mora", i)
                        End If
                        decMoratorios += decMoraPagoPend

                    End If
                Next

                'Suma de todos los valores
                decMontoCancelar = decCapital + decInteres + decMoratorios + decIntPend + decMoraPend
                EditTextMontoCancelar.AsignaValorUserDataSource(decMontoCancelar.ToString(n))

            ElseIf String.IsNullOrEmpty(strCapital) Then

                decMontoCancelar = 0
                EditTextMontoCancelar.AsignaValorUserDataSource(decMontoCancelar.ToString(n))

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    ''' <summary>
    ''' Carga lista de pagos que se pueden reversar
    ''' </summary>
    ''' <param name="strPrestamo">DocEntry del préstamo</param>
    ''' <remarks></remarks>
    Private Sub CargarPagosReversar(ByVal strPrestamo As String)

        Dim strConsulta As String
        Dim dataTableListaReversados As DataTable

        Try

            dataTableReversar = FormularioSBO.DataSources.DataTables.Item("PagosReversar")
            dataTableReversar.Clear()

            strConsulta =
                String.Format(" SELECT U_Numero, U_Fecha, U_Cuota, U_Capital, U_Interes, U_Int_Mora, U_Cap_Pend, U_Int_Pend, U_Mor_Pend, U_Dias_Int, U_Dias_Mor " & _
                " FROM [dbo].[@SCGD_PLAN_REAL] WHERE DocEntry = '{0}' AND U_Pagado = 'Y' AND U_Numero IS NOT NULL AND U_Cuota > 0 ORDER BY U_Numero", strPrestamo)

            dataTableReversar.ExecuteQuery(strConsulta)

            dataTableListaReversados = FormularioSBO.DataSources.DataTables.Item("ReversadosMatrix")
            dataTableListaReversados.Rows.Clear()
            MatrixPagosReversar.Matrix.Clear()

            If Not dataTableReversar.GetValue("U_Numero", 0) = 0 Then
                For i As Integer = 0 To dataTableReversar.Rows.Count - 1
                    dataTableListaReversados.Rows.Add()
                    dataTableListaReversados.SetValue("numero", i, dataTableReversar.GetValue("U_Numero", i))
                    If Not String.IsNullOrEmpty(dataTableReversar.GetValue("U_Fecha", i)) Then dataTableListaReversados.SetValue("fecha", i, dataTableReversar.GetValue("U_Fecha", i))
                    dataTableListaReversados.SetValue("cuota", i, dataTableReversar.GetValue("U_Cuota", i))
                    dataTableListaReversados.SetValue("capital", i, dataTableReversar.GetValue("U_Capital", i))
                    dataTableListaReversados.SetValue("interes", i, dataTableReversar.GetValue("U_Interes", i))
                    dataTableListaReversados.SetValue("intMora", i, dataTableReversar.GetValue("U_Int_Mora", i))
                    dataTableListaReversados.SetValue("capPend", i, dataTableReversar.GetValue("U_Cap_Pend", i))
                    dataTableListaReversados.SetValue("intPend", i, dataTableReversar.GetValue("U_Int_Pend", i))
                    dataTableListaReversados.SetValue("moraPend", i, dataTableReversar.GetValue("U_Mor_Pend", i))
                    dataTableListaReversados.SetValue("diasInt", i, dataTableReversar.GetValue("U_Dias_Int", i))
                    dataTableListaReversados.SetValue("diasMora", i, dataTableReversar.GetValue("U_Dias_Mor", i))
                Next
                MatrixPagosReversar.Matrix.LoadFromDataSource()
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    'Manejo de eventos de pantalla de préstamo

    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, Optional ByVal strPath As String = "")

        If Not pVal.FormTypeEx = FormType Then Return

        If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

            If pVal.ItemUID = FolderDatosFin.UniqueId Then

                FormularioSBO.PaneLevel = 1

            ElseIf pVal.ItemUID = FolderPagos.UniqueId Then

                FormularioSBO.PaneLevel = 2

            ElseIf pVal.ItemUID = FolderReversion.UniqueId Then

                FormularioSBO.PaneLevel = 3

            ElseIf pVal.ItemUID = FolderCheques.UniqueId Then

                FormularioSBO.PaneLevel = 4

            ElseIf pVal.ItemUID = FolderPlanReal.UniqueId Then

                FormularioSBO.PaneLevel = 5

            ElseIf pVal.ItemUID = FolderPlanTeorico.UniqueId Then

                FormularioSBO.PaneLevel = 6

            ElseIf pVal.ItemUID = ButtonAbonar.UniqueId Then

                ButtonSBOAbonarItemPresed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = ButtonCalcular.UniqueId Then

                ButtonSBOCalcularItemPresed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = ButtonActualizar.UniqueId Then

                ButtonSBOActualizarItemPresed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = ButtonReversar.UniqueId Then

                ButtonSBOReversarItemPresed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = ButtonImprimirPago.UniqueId Then

                ButtonSBOImprimirPagoItemPresed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = CheckBoxCancelarMora.UniqueId Then

                CheckBoxSBOCancelarMoraItemPresed(FormUID, pVal)

            ElseIf pVal.ItemUID = ButtonImprimirReversados.UniqueId Then

                ButtonSBOImprimirReversadosItemPresed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = ButtonAgreCheque.UniqueId Then

                ButtonSBOAgregaChequeItemPresed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = ButtonActCheque.UniqueId Then

                ButtonSBOActualizaChequeItemPresed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = ButtonEliCheque.UniqueId Then

                ButtonSBOEliminaChequeItemPresed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = ButtonAplicaCheque.UniqueId Then

                ButtonSBOAplicaChequeItemPresed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = CheckBoxCheque.UniqueId Then

                CheckBoxSBOAbonaChequeItemPresed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = "mtxChPF" Then

                MatrixChequesPostItemPresed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = BtnRevalorizacion.UniqueId Then

                ButtonSBORevalorizaciónItemPresed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = CheckBoxPagoDeuda.UniqueId Then

                CheckBoxSBOPagoDeudaItemPresed(FormUID, pVal)

            End If

        ElseIf pVal.EventType = BoEventTypes.et_MATRIX_LINK_PRESSED Then
            MatrixRealItemPresed(FormUID, pVal, BubbleEvent, strPath)
        End If

    End Sub

    Private Sub MatrixRealItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal strPath As String)

        Try
            If pVal.BeforeAction Then

            Else
                Select Case pVal.ColUID
                    Case "Col_Pagos"
                        AbrirFormularioPagos(pVal, BubbleEvent, strPath, FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocEntry", 0).Trim())
                End Select
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub AbrirFormularioPagos(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal p_strPath As String, ByVal p_intPrestamo As Integer)

        Dim intNumeroPrestamo As Integer
        Dim fcp As SAPbouiCOM.FormCreationParams
        Dim strXMLACargar As String
        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String
        Dim oForm As SAPbouiCOM.Form

        Try

            intNumeroPrestamo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocEntry", 0)

            If Not General.ValidarSiFormularioAbierto(ApplicationSBO, "SCGD_PAGOS_PRESTAMOS", False) Then


                fcp = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
                fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
                fcp.UniqueID = "SCGD_PAGOS_PRESTAMOS"
                fcp.FormType = "SCGD_PAGOS_PRESTAMOS"
                fcp.ObjectType = "SCGD_Prestamo"

                strXMLACargar = My.Resources.Resource.PagoPrestramos

                strPath = p_strPath & "\" & strXMLACargar
                oXMLDoc = New Xml.XmlDataDocument

                If Not oXMLDoc Is Nothing Then
                    oXMLDoc.Load(strPath)
                End If

                fcp.XmlData = oXMLDoc.InnerXml

                oForm = ApplicationSBO.Forms.AddEx(fcp)
                oForm.SupportedModes = 4
                oForm.Mode = BoFormMode.fm_FIND_MODE

                oForm.Items.Item("txtNumPres").Specific.Value = p_intPrestamo

                'If m_SBO_Application.Menus.Item("1281").Enabled Then m_SBO_Application.Menus.Item("1281").Activate()

                oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                oForm.SupportedModes = 1
                oForm.Mode = BoFormMode.fm_OK_MODE

                oForm.Items.Item("txtNumPres").Enabled = False
            Else

                ApplicationSBO.Forms.Item("SCGD_PAGOS_PRESTAMOS").Select()
                ApplicationSBO.Forms.Item("SCGD_PAGOS_PRESTAMOS").SupportedModes = 4
                ApplicationSBO.Forms.Item("SCGD_PAGOS_PRESTAMOS").Mode = BoFormMode.fm_FIND_MODE
                ApplicationSBO.Forms.Item("SCGD_PAGOS_PRESTAMOS").Items.Item("txtNumPres").Enabled = True
                ApplicationSBO.Forms.Item("SCGD_PAGOS_PRESTAMOS").Items.Item("txtNumPres").Specific.Value = p_intPrestamo
                ApplicationSBO.Forms.Item("SCGD_PAGOS_PRESTAMOS").Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                ApplicationSBO.Forms.Item("SCGD_PAGOS_PRESTAMOS").SupportedModes = 1
                ApplicationSBO.Forms.Item("SCGD_PAGOS_PRESTAMOS").Mode = BoFormMode.fm_OK_MODE
                ApplicationSBO.Forms.Item("SCGD_PAGOS_PRESTAMOS").Items.Item("txtNumPres").Enabled = False

            End If
            ' CargarPagosReversar()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub


    Private Sub RecargarPagos(ByVal FormUID As String, _
                                          ByRef pVal As SAPbouiCOM.ItemEvent, _
                                          ByRef BubbleEvent As Boolean)

        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition
        Dim oMatrix As SAPbouiCOM.Matrix

        oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
        oCondition = oConditions.Add

        oCondition.Alias = "DocEntry"
        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
        oCondition.CondVal = _applicationSbo.Forms.Item("SCGD_PAGOS_PRESTAMOS").Items.Item("txtNumPres").Specific.Value

        _applicationSbo.Forms.Item("SCGD_PAGOS_PRESTAMOS").DataSources.DBDataSources.Item("@SCGD_PRESTAMO").Query(oConditions)
        _applicationSbo.Forms.Item("SCGD_PAGOS_PRESTAMOS").DataSources.DBDataSources.Item("@SCGD_PLAN_TEORICO").Query(oConditions)
        _applicationSbo.Forms.Item("SCGD_PAGOS_PRESTAMOS").DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").Query(oConditions)
        _applicationSbo.Forms.Item("SCGD_PAGOS_PRESTAMOS").DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").Query(oConditions)
        _applicationSbo.Forms.Item("SCGD_PAGOS_PRESTAMOS").DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").Query(oConditions)

        oMatrix = DirectCast(_applicationSbo.Forms.Item("SCGD_PAGOS_PRESTAMOS").Items.Item("mtxPagos").Specific, SAPbouiCOM.Matrix)
        oMatrix.LoadFromDataSource()

    End Sub



    ''' <summary>
    ''' Manejo de los eventos de tipo DATA en el formulario de Préstamos
    ''' </summary>
    ''' <param name="businessObjectInfo"></param>
    ''' <remarks></remarks>
    Public Sub ApplicationSBOOnDataEvent(ByRef businessObjectInfo As BusinessObjectInfo, ByRef BubbleEvent As Boolean)

        Dim strPrestamo As String
        Dim strTipoCuota As String = String.Empty
        Dim strUnidad As String

        If Not businessObjectInfo.FormTypeEx = FormType Then Return

        If businessObjectInfo.BeforeAction Then

            If businessObjectInfo.EventType = BoEventTypes.et_FORM_DATA_LOAD Then
                Call LimpiarPago()
                EditTextMontoFin.ItemSBO.Enabled = False
            End If

        ElseIf businessObjectInfo.ActionSuccess Then

            Select Case businessObjectInfo.EventType
                Case BoEventTypes.et_FORM_DATA_LOAD

                    strPrestamo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocEntry", 0).Trim()

                    Call CargarDatosPago(strPrestamo)

                    strTipoCuota = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Tipo_Cuo", 0).Trim()

                    If strTipoCuota <> "1" Then
                        Call CargarPagosReversar(strPrestamo)
                        FormularioSBO.Items.Item("Folder3").Enabled = True
                        FormularioSBO.Items.Item("Folder3").Visible = True
                        manejoEstadoColumnas(False)
                    Else
                        FormularioSBO.Items.Item("Folder3").Enabled = False
                        FormularioSBO.Items.Item("Folder3").Visible = False
                        manejoEstadoColumnas(True)
                    End If
                    Call CargarMontoActualCancelar(strPrestamo)
                    Call ManejarEstadoPrestamo()



                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_ModPlazo", 0, "N")
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Can_Mora", 0, "N")

                    If strTipoCuota = "3" Then

                        FormularioSBO.Items.Item("chkModPlaz").Enabled = False
                        EditTextMontoAbo.ItemSBO.Enabled = False

                    End If

                    If strTipoCuota = "1" Then EditTextIntNormal.ItemSBO.Enabled = False

                    strUnidad = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Cod_Unid", 0).Trim()

                    If strUnidad = My.Resources.Resource.Multiples Then
                        FormularioSBO.Items.Item("lkUnidad").Visible = False
                    Else
                        FormularioSBO.Items.Item("lkUnidad").Visible = True
                    End If

                    m_dtFechaPagoCalculo = Nothing
                    m_strCodPrestRev = String.Empty

                    If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Reval", 0).Trim() = "Y" Then
                        BtnRevalorizacion.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    Else
                        BtnRevalorizacion.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    End If

                    m_blnEjecutarMetodo = False

            End Select

        End If

    End Sub

    Private Sub manejoEstadoColumnas(ByVal blnEstado As Boolean)
        Try

            Dim oMatriz As SAPbouiCOM.Matrix
            oMatriz = CType(FormularioSBO.Items.Item("mtxReal").Specific, SAPbouiCOM.Matrix)
            oMatriz.Columns.Item("col_ToCuo").Visible = blnEstado
            oMatriz.Columns.Item("col_ToCap").Visible = blnEstado
            oMatriz.Columns.Item("col_ToIn").Visible = blnEstado
            oMatriz.Columns.Item("col_ToMo").Visible = blnEstado
            oMatriz.Columns.Item("Col_Pagos").Visible = blnEstado
            oMatriz.Columns.Item("col_CapPe").Visible = Not blnEstado
            oMatriz.Columns.Item("colo_IntPe").Visible = Not blnEstado
            oMatriz.Columns.Item("col_MorPe").Visible = Not blnEstado
            oMatriz.Columns.Item("col_CredC").Visible = Not blnEstado
            oMatriz.Columns.Item("col_DocIn").Visible = Not blnEstado
            oMatriz.Columns.Item("col_DocF").Visible = Not blnEstado
            oMatriz.Columns.Item("Col_BorrP").Visible = Not blnEstado
            oMatriz.Columns.Item("V_1").Visible = Not blnEstado
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Carga de un Formulario desde un Link u otro objeto (Componente)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ManejarEstadoPrestamo()

        Dim strEstado As String
        
        Try

            strEstado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Estado", 0).Trim()

            Select Case strEstado
                Case "1"
                    EditTextFechaPago.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    EditTextMontoAbo.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    ButtonAbonar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    ButtonCalcular.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    ButtonReversar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    If DMS_Connector.Helpers.PermisosMenu("SCGD_RPA") Then
                        ButtonReversar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                        BtnRevalorizacion.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    Else
                        ButtonReversar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                        BtnRevalorizacion.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    End If
                    ManejaControlesChequesPostFechados(True, False, True)
                Case "2"
                    ButtonAbonar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    ButtonCalcular.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    ButtonImprimirPago.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    ButtonImprimirReversados.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    ButtonReversar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    BtnRevalorizacion.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    ManejaControlesChequesPostFechados(False, True, True)
                Case "3"
                    ButtonAbonar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    ButtonCalcular.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    ButtonImprimirPago.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    ButtonReversar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    ButtonImprimirReversados.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    BtnRevalorizacion.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    ManejaControlesChequesPostFechados(False, True, True)
            End Select

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Public Sub ManejaControlesRevalorización(ByVal blnActivar As Boolean)

        If blnActivar Then
            EditTextIntNormal.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            EditTextPlazo.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            EditTextFecha.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            EditTextDiaPago.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            EditTextIntMora.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

            ButtonAbonar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            ButtonCalcular.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            EditTextFechaPago.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            EditTextMontoAbo.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            ButtonReversar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
        Else
            EditTextIntNormal.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            EditTextPlazo.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            EditTextFecha.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            EditTextDiaPago.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            EditTextIntMora.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

            ButtonAbonar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            ButtonCalcular.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            EditTextFechaPago.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            EditTextMontoAbo.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            ButtonReversar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
        End If

    End Sub


    Public Sub ManejaControlesChequesPostFechados(ByVal blnActivar As Boolean, ByVal blnDesactivar As Boolean, ByVal blnLimpiar As Boolean)
        If blnActivar Then
            EditTextFeVen.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            EditTextImp.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            ComboBoxPai.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            ComboBoxNBan.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            ComboBoxSuc.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            EditTextCuen.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            EditTextNChe.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            ComboBoxEnd.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

            'ButtonActCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            ButtonAgreCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            ButtonEliCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            ButtonAplicaCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

            CheckBoxDisminucion.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            CheckBoxCancelarMora.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            CheckBoxPagoDeuda.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            'CheckBoxCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
        End If
        If blnDesactivar Then
            EditTextFeVen.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            EditTextImp.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            ComboBoxPai.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            ComboBoxNBan.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            ComboBoxSuc.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            EditTextCuen.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            EditTextNChe.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            ComboBoxEnd.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

            'ButtonActCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            ButtonAgreCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            ButtonEliCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            ButtonAplicaCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

            CheckBoxDisminucion.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            CheckBoxCancelarMora.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            CheckBoxPagoDeuda.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            CheckBoxCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
        End If
        If blnLimpiar Then
            EditTextFeVen.AsignaValorUserDataSource("")
            EditTextImp.AsignaValorUserDataSource(0)
            ComboBoxPai.AsignaValorUserDataSource("")
            ComboBoxNBan.AsignaValorUserDataSource("")
            ComboBoxSuc.AsignaValorUserDataSource("")
            EditTextCuen.AsignaValorUserDataSource("")
            EditTextNChe.AsignaValorUserDataSource("")
            ComboBoxEnd.AsignaValorUserDataSource("")
            CheckBoxDisminucion.AsignaValorDataSource("N")
            CheckBoxCancelarCobro.AsignaValorDataSource("N")
            CheckBoxCheque.AsignaValorDataSource("N")
        End If
    End Sub

    'Limpiar campos de pestaña de Realizar Abono

    Public Sub LimpiarPago()

        Try

            EditTextNumero.Especifico.Value = Nothing
            EditTextSalIni.AsignaValorUserDataSource(0)
            EditTextMontoAbo.AsignaValorUserDataSource(0)
            EditTextAboCap.AsignaValorUserDataSource(0)
            EditTextAboInt.AsignaValorUserDataSource(0)
            EditTextAboMor.AsignaValorUserDataSource(0)
            EditTextFechaPago.AsignaValorUserDataSource("")
            EditTextSalFin.AsignaValorUserDataSource(0)
            EditTextFechaUltimo.AsignaValorUserDataSource("")
            EditTextCapPend.AsignaValorUserDataSource(0)
            EditTextIntPend.AsignaValorUserDataSource(0)
            EditTextMoraPend.AsignaValorUserDataSource(0)
            EditTextDiasInt.AsignaValorUserDataSource(0)
            EditTextDiasMora.AsignaValorUserDataSource(0)
            CheckBoxPagoDeuda.AsignaValorUserDataSource("N")
            EditTextRecargoCobranza.AsignaValorUserDataSource(0)
            ManejaControlesChequesPostFechados(True, False, True)

        Catch ex As Exception

            Throw ex

        End Try

    End Sub



#Region "Eventos"

    Private Sub SBO_Application_ItemEvent(ByVal FormUID As String, _
                                          ByRef pVal As SAPbouiCOM.ItemEvent, _
                                          ByRef BubbleEvent As Boolean) Handles _applicationSbo.ItemEvent

        If pVal.ActionSuccess Then

            Select Case FormUID
                Case "SCGD_PAGOS_PRESTAMOS"

                    Select Case pVal.ItemUID
                        Case "btnUPD"
                            RecargarPagos(FormUID, pVal, BubbleEvent)
                        Case "btnRePa"
                            ReversarPagosCuotaNivelada(FormUID, pVal, BubbleEvent)
                        Case "btnGenera"
                            GeneraDocumentoIntereses(FormUID, pVal, BubbleEvent)
                    End Select

            End Select

        Else
            Select Case FormUID
                Case "SCGD_PAGOS_PRESTAMOS"
                    Select Case pVal.ItemUID
                        Case "btnRePa"
                            ReversarPagosCuotaNivelada(FormUID, pVal, BubbleEvent)
                        
                    End Select

            End Select
        End If
    End Sub

#End Region

End Class
