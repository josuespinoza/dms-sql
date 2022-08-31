Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI.Extensions
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany
Imports DMS_Addon.ControlesSBO

Partial Public Class CosteoMultiple : Implements IFormularioSBO, IUsaPermisos

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

    Private EditTextUnidad As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextFecha As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextRecepcion As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextPedido As SCG.SBOFramework.UI.EditTextSBO
    Private CheckBoxFacturadas As SCG.SBOFramework.UI.CheckBoxSBO
    Private CheckBoxSelTodas As SCG.SBOFramework.UI.CheckBoxSBO
    Private CheckBoxSelRecost As SCG.SBOFramework.UI.CheckBoxSBO
    Private ComboBoxTipo As SCG.SBOFramework.UI.ComboBoxSBO
    Private ButtonCostear As SCG.SBOFramework.UI.ButtonSBO
    Private ButtonActualizar As SCG.SBOFramework.UI.ButtonSBO
    Private EditTextFechaInicio As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextFechaFin As SCG.SBOFramework.UI.EditTextSBO

    Private MatrixSinCostear As MatrixSBOSinCostear
    Private MatrixRecosteo As MatrixSBORecosteo

    Private dataTableSinCostear As DataTable
    Private dataTableRecosteo As DataTable
    Private dataTableValoresCosteo As DataTable
    'Private dataTableAsientos As DataTable

    Public objCosteo As CosteoCls


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

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        _companySbo = companySbo
        _applicationSbo = application
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioCosteoMultiplesUnidades
        MenuPadre = "SCGD_MNO"
        Nombre = "Costeo Múltiple"
        IdMenu = "SCGD_CMU"
        Titulo = My.Resources.Resource.TituloCosteoMultiple
        Posicion = 25
        FormType = "SCGD_frm_CMU"
    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario

        FormularioSBO.Freeze(True)

        FormularioSBO.PaneLevel = 1

        Call CargaFormularioCosteoMultiplesUnidades(True)

        FormularioSBO.Freeze(False)

    End Sub

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

        FormularioSBO.Freeze(True)

        Dim userDataSources As UserDataSources = FormularioSBO.DataSources.UserDataSources
        userDataSources.Add("unidad", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("fecha", BoDataType.dt_DATE, 100)
        userDataSources.Add("factura", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("tipo", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("selTod", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("selRec", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("fecInicio", BoDataType.dt_DATE, 100)
        userDataSources.Add("fecFin", BoDataType.dt_DATE, 100)
        userDataSources.Add("recep", BoDataType.dt_SHORT_TEXT, 100)
        userDataSources.Add("pedid", BoDataType.dt_SHORT_TEXT, 100)

        EditTextUnidad = New SCG.SBOFramework.UI.EditTextSBO("txtUnidad", True, "", "unidad", FormularioSBO)
        EditTextFecha = New SCG.SBOFramework.UI.EditTextSBO("txt_FecCon", True, "", "fecha", FormularioSBO)
        EditTextRecepcion = New SCG.SBOFramework.UI.EditTextSBO("txtRecVeh", True, "", "recep", FormularioSBO)
        EditTextPedido = New SCG.SBOFramework.UI.EditTextSBO("txtCodPed", True, "", "pedid", FormularioSBO)
        CheckBoxFacturadas = New SCG.SBOFramework.UI.CheckBoxSBO("chkFac", True, "", "factura", FormularioSBO)
        ComboBoxTipo = New SCG.SBOFramework.UI.ComboBoxSBO("cboTipo", FormularioSBO, True, "", "tipo")
        CheckBoxSelTodas = New SCG.SBOFramework.UI.CheckBoxSBO("chkSelAll", True, "", "selTod", FormularioSBO)
        CheckBoxSelRecost = New SCG.SBOFramework.UI.CheckBoxSBO("chkSelRec", True, "", "selRec", FormularioSBO)
        EditTextFechaInicio = New UI.EditTextSBO("txtFecIni", True, "", "fecInicio", FormularioSBO)
        EditTextFechaFin = New UI.EditTextSBO("txtFecFin", True, "", "fecFin", FormularioSBO)

        EditTextUnidad.AsignaBinding()
        EditTextFecha.AsignaBinding()
        EditTextRecepcion.AsignaBinding()
        EditTextPedido.AsignaBinding()

        CheckBoxFacturadas.AsignaBinding()
        ComboBoxTipo.AsignaBinding()
        CheckBoxSelTodas.AsignaBinding()
        CheckBoxSelRecost.AsignaBinding()
        EditTextFechaInicio.AsignaBinding()
        EditTextFechaFin.AsignaBinding()

        ButtonCostear = New SCG.SBOFramework.UI.ButtonSBO("btnCU", FormularioSBO)
        ButtonActualizar = New SCG.SBOFramework.UI.ButtonSBO("btnActuali", FormularioSBO)

        ButtonCostear.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
        ButtonActualizar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

        dataTableSinCostear = FormularioSBO.DataSources.DataTables.Add("SinCostear")
        dataTableSinCostear.Columns.Add("seleccion", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableSinCostear.Columns.Add("unidad", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableSinCostear.Columns.Add("marca", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableSinCostear.Columns.Add("estilo", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableSinCostear.Columns.Add("contrato", BoFieldsType.ft_AlphaNumeric, 100)

        MatrixSinCostear = New MatrixSBOSinCostear("mtx_VehSin", FormularioSBO, "SinCostear")
        MatrixSinCostear.CreaColumnas()
        MatrixSinCostear.LigaColumnas()

        dataTableRecosteo = FormularioSBO.DataSources.DataTables.Add("Recosteo")
        dataTableRecosteo.Columns.Add("seleccion", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableRecosteo.Columns.Add("unidad", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableRecosteo.Columns.Add("marca", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableRecosteo.Columns.Add("estilo", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableRecosteo.Columns.Add("vin", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableRecosteo.Columns.Add("doc", BoFieldsType.ft_AlphaNumeric, 100)

        MatrixRecosteo = New MatrixSBORecosteo("mtx_Recost", FormularioSBO, "Recosteo")
        MatrixRecosteo.CreaColumnas()
        MatrixRecosteo.LigaColumnas()

        Dim dataTablaSC As DataTable = FormularioSBO.DataSources.DataTables.Add("SC")
        Dim dataTablaRC As DataTable = FormularioSBO.DataSources.DataTables.Add("RC")
        Dim dataTablaConf As DataTable = FormularioSBO.DataSources.DataTables.Add("Conf")
        Dim dataTableVehiculo As DataTable = FormularioSBO.DataSources.DataTables.Add("Veh")
        Dim dataTableEntrada As DataTable = FormularioSBO.DataSources.DataTables.Add("Ent")

        FormularioSBO.Freeze(False)


        AddChooseFromList(FormularioSBO, "SCGD_EDV", "CFL_Rec")
        AddChooseFromList(FormularioSBO, "SCGD_PDV", "CFL_Ped")

        AgregaCFLRecepcion(EditTextRecepcion.UniqueId, "CFL_Rec", "DocEntry")
        AgregaCFLRecepcion(EditTextPedido.UniqueId, "CFL_Ped", "DocEntry")

        Dim strTablaConsulta As String = "dtConsulta"
        'Dim strTablaConsultaAsientos As String = "dtConsultaAsientos"
        dataTableValoresCosteo = FormularioSBO.DataSources.DataTables.Add(strTablaConsulta)
        'dataTableAsientos = FormularioSBO.DataSources.DataTables.Add(strTablaConsultaAsientos)

    End Sub
    
End Class
