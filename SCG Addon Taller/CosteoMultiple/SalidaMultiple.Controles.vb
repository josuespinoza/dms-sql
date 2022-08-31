
Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class SalidaMultiple : Implements IFormularioSBO, IUsaPermisos
    
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
    Private EditTextRecepcion As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextFecha As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextFechaFin As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextFechaInicio As SCG.SBOFramework.UI.EditTextSBO

    Private CheckBoxSelTodas As SCG.SBOFramework.UI.CheckBoxSBO
    Private CheckBoxFacturada As SCG.SBOFramework.UI.CheckBoxSBO

    Private ComboBoxTipo As SCG.SBOFramework.UI.ComboBoxSBO

    Private ButtonActualizar As SCG.SBOFramework.UI.ButtonSBO
    Private ButtonCostear As SCG.SBOFramework.UI.ButtonSBO

    Private MatrixSalidas As MatrixSBOSalidas
   
    Private dataTableSalidas As DataTable

#Region "Constructor"

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        _companySbo = companySbo
        _applicationSbo = application
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLSalidaMultiplesUnidades
        MenuPadre = "SCGD_MNO"
        Nombre = "Salida Múltiple"
        IdMenu = "SCGD_SMU"
        Titulo = My.Resources.Resource.TituloSalidaMultiple
        Posicion = 26
        FormType = "SCGD_frm_SMU"
    End Sub

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
    
  

   

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

        FormularioSBO.Freeze(True)

        Dim userDataSourcesSalida As UserDataSources = FormularioSBO.DataSources.UserDataSources
        userDataSourcesSalida.Add("unidad", BoDataType.dt_LONG_TEXT, 100)
        userDataSourcesSalida.Add("fecha", BoDataType.dt_DATE, 100)
        userDataSourcesSalida.Add("seltod", BoDataType.dt_LONG_TEXT, 100)
        userDataSourcesSalida.Add("tipo", BoDataType.dt_LONG_TEXT, 100)
        userDataSourcesSalida.Add("fechaIni", BoDataType.dt_DATE, 100)
        userDataSourcesSalida.Add("fechaFin", BoDataType.dt_DATE, 100)
        userDataSourcesSalida.Add("factura", BoDataType.dt_LONG_TEXT, 100)
        userDataSourcesSalida.Add("recep", BoDataType.dt_LONG_TEXT, 100)
    
        EditTextUnidad = New SCG.SBOFramework.UI.EditTextSBO("txtUnidad", True, "", "unidad", FormularioSBO)
        EditTextFecha = New SCG.SBOFramework.UI.EditTextSBO("txt_FecCon", True, "", "fecha", FormularioSBO)
        CheckBoxSelTodas = New SCG.SBOFramework.UI.CheckBoxSBO("chkSelAll", True, "", "seltod", FormularioSBO)
        ComboBoxTipo = New SCG.SBOFramework.UI.ComboBoxSBO("cboTipo", FormularioSBO, True, "", "tipo")
        EditTextFechaInicio = New SCG.SBOFramework.UI.EditTextSBO("txtFeIni", True, "", "fechaIni", FormularioSBO)
        EditTextFechaFin = New SCG.SBOFramework.UI.EditTextSBO("txtFeFin", True, "", "fechaFin", FormularioSBO)
        CheckBoxFacturada = New SCG.SBOFramework.UI.CheckBoxSBO("chkFac", True, "", "factura", FormularioSBO)
        EditTextRecepcion = New SCG.SBOFramework.UI.EditTextSBO("txtDocRec", True, "", "recep", FormularioSBO)

        EditTextUnidad.AsignaBinding()
        EditTextFecha.AsignaBinding()
        CheckBoxSelTodas.AsignaBinding()
        ComboBoxTipo.AsignaBinding()
        EditTextFechaInicio.AsignaBinding()
        EditTextFechaFin.AsignaBinding()
        CheckBoxFacturada.AsignaBinding()
        EditTextRecepcion.AsignaBinding()

        ButtonActualizar = New SCG.SBOFramework.UI.ButtonSBO("btnActuali", FormularioSBO)
        ButtonCostear = New SCG.SBOFramework.UI.ButtonSBO("btnCU", FormularioSBO)

        ButtonActualizar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
        ButtonCostear.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
        
        dataTableSalidas = FormularioSBO.DataSources.DataTables.Add("Salidas")
        dataTableSalidas.Columns.Add("seleccion", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableSalidas.Columns.Add("entrada", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableSalidas.Columns.Add("unidad", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableSalidas.Columns.Add("marca", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableSalidas.Columns.Add("estilo", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableSalidas.Columns.Add("vin", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableSalidas.Columns.Add("id", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableSalidas.Columns.Add("Gastra", BoFieldsType.ft_Float, 100)
        dataTableSalidas.Columns.Add("Gastra_S", BoFieldsType.ft_Float, 100)

        MatrixSalidas = New MatrixSBOSalidas("mtx_Recost", FormularioSBO, "Salidas")
        MatrixSalidas.CreaColumnas()
        MatrixSalidas.LigaColumnas()
        
        Dim dataTableSA As DataTable = FormularioSBO.DataSources.DataTables.Add("dtSA")
        
        FormularioSBO.Freeze(False)

    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario

        FormularioSBO.Freeze(True)

        Call CargaFormularioSalidaMultiplesUnidades()

        FormularioSBO.Freeze(False)

    End Sub

End Class
