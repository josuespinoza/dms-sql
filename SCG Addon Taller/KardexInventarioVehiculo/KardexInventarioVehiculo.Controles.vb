Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI.Extensions
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany
Imports DMS_Addon.ControlesSBO

Partial Public Class KardexInventarioVehiculo : Implements IFormularioSBO, IUsaPermisos

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

    Private EditTxtUnidad As SCG.SBOFramework.UI.EditTextSBO
    Private EditTxtFechaDesde As SCG.SBOFramework.UI.EditTextSBO
    Private EditTxtFechaHasta As SCG.SBOFramework.UI.EditTextSBO

    Private MatrizVehiculo As MatrizSBOVehiculo


    Private dataTableVehiculo As SAPbouiCOM.DataTable


    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements IFormularioSBO.ApplicationSBO
        Get

            Return _applicationSbo

        End Get
    End Property

    Public ReadOnly Property CompanySBO As SAPbobsCOM.ICompany Implements IFormularioSBO.CompanySBO
        Get
            Return _companySbo
        End Get
    End Property

    Public Property FormType As String Implements IFormularioSBO.FormType
        Get
            Return _formType
        End Get
        Set(value As String)
            _formType = value
        End Set
    End Property

    Public Property FormularioSBO As SAPbouiCOM.IForm Implements IFormularioSBO.FormularioSBO
        Get
            Return _formularioSbo
        End Get
        Set(value As SAPbouiCOM.IForm)
            _formularioSbo = value
        End Set
    End Property

    Public Property Inicializado As Boolean Implements IFormularioSBO.Inicializado
        Get
            Return _inicializado
        End Get
        Set(value As Boolean)
            _inicializado = value
        End Set
    End Property

    Public Property NombreXml As String Implements IFormularioSBO.NombreXml
        Get
            Return _nombreXml
        End Get
        Set(value As String)
            _nombreXml = value
        End Set
    End Property

    Public Property Titulo As String Implements IFormularioSBO.Titulo
        Get
            Return _titulo
        End Get
        Set(value As String)
            _titulo = value
        End Set
    End Property

    Public Property IdMenu As String Implements IUsaMenu.IdMenu
        Get
            Return _idMenu
        End Get
        Set(value As String)
            _idMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements IUsaMenu.MenuPadre
        Get
            Return _menuPadre
        End Get
        Set(value As String)
            _menuPadre = value
        End Set
    End Property

    Public Property Nombre As String Implements IUsaMenu.Nombre
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property Posicion As Integer Implements IUsaMenu.Posicion
        Get
            Return _posicion
        End Get
        Set(value As Integer)
            _posicion = value
        End Set
    End Property


    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        _companySbo = companySbo
        _applicationSbo = application
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLKardexInventarioVehiculo
        MenuPadre = "SCGD_MNO"
        Nombre = "KARDEX"
        IdMenu = "SCGD_KDX"
        Posicion = 75
        FormType = "SCGD_KDEX"
    End Sub


    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario

        FormularioSBO.Freeze(True)

        FormularioSBO.Freeze(False)


    End Sub

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

        FormularioSBO.Freeze(True)

        Dim userDataSourceVehiculo As UserDataSources = FormularioSBO.DataSources.UserDataSources
        userDataSourceVehiculo.Add("unidad", BoDataType.dt_SHORT_TEXT, 100)
        userDataSourceVehiculo.Add("FechaDesde", BoDataType.dt_DATE, 100)
        userDataSourceVehiculo.Add("FechaHasta", BoDataType.dt_DATE, 100)

        EditTxtUnidad = New SCG.SBOFramework.UI.EditTextSBO("txtVehi", True, "", "unidad", FormularioSBO)
        EditTxtFechaDesde = New SCG.SBOFramework.UI.EditTextSBO("txtFecD", True, "", "FechaDesde", FormularioSBO)
        EditTxtFechaHasta = New SCG.SBOFramework.UI.EditTextSBO("txtFecH", True, "", "FechaHasta", FormularioSBO)


        EditTxtUnidad.AsignaBinding()
        EditTxtFechaDesde.AsignaBinding()
        EditTxtFechaHasta.AsignaBinding()

        dataTableVehiculo = FormularioSBO.DataSources.DataTables.Add("Vehiculos")
        dtDisponibilidad = FormularioSBO.DataSources.DataTables.Add("dtDisponibilidad")

        dataTableVehiculo.Columns.Add("TipoDocumento", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableVehiculo.Columns.Add("FechaContabilizacion", BoFieldsType.ft_Date, 100)
        dataTableVehiculo.Columns.Add("docentry", BoFieldsType.ft_Integer, 100)
        dataTableVehiculo.Columns.Add("Unidad", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableVehiculo.Columns.Add("Asiento", BoFieldsType.ft_Integer, 100)
        dataTableVehiculo.Columns.Add("Total_EntradaLocal", BoFieldsType.ft_Float, 200)
        dataTableVehiculo.Columns.Add("Total_EntradaSistema", BoFieldsType.ft_Float, 200)
        dataTableVehiculo.Columns.Add("Total_SalidaLocal", BoFieldsType.ft_Float, 200)
        dataTableVehiculo.Columns.Add("Total_SalidaSistema", BoFieldsType.ft_Float, 200)
        dataTableVehiculo.Columns.Add("Tipo", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableVehiculo.Columns.Add("Trasladado", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableVehiculo.Columns.Add("NombreInventario", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableVehiculo.Columns.Add("IdVehiculo", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableVehiculo.Columns.Add("ValorAcumulado", BoFieldsType.ft_Float, 200)
        dataTableVehiculo.Columns.Add("DescTraslado", BoFieldsType.ft_AlphaNumeric, 200)
 

        Dim dataTableVH As DataTable = FormularioSBO.DataSources.DataTables.Add("VH")

        MatrizVehiculo = New MatrizSBOVehiculo("mtxVehi2", FormularioSBO, "Vehiculos")

        MatrizVehiculo.CreaColumnas()
        MatrizVehiculo.LigaColumnas()

        AddChooseFromList(FormularioSBO, "SCGD_VEH", "CFL_Vehi")
        AsignarCFLButton("btnUni", "CFL_Vehi")


        FormularioSBO.Freeze(False)

    End Sub

    Private Sub AddChooseFromList(ByVal oform As SAPbouiCOM.Form, ByVal ObjectType As String, ByVal UniqueID As String)
        Try
            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection

            oCFLs = oform.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = ObjectType
            oCFLCreationParams.UniqueID = UniqueID

            oCFL = oCFLs.Add(oCFLCreationParams)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub AsignarCFLButton(ByVal p_strControl As String, ByVal p_strCFL As String)

        Try

            Dim oitem As SAPbouiCOM.Item
            Dim oButton As SAPbouiCOM.Button

            oitem = FormularioSBO.Items.Item(p_strControl)
            oButton = CType(oitem.Specific, SAPbouiCOM.Button)

            oButton.Type = SAPbouiCOM.BoButtonTypes.bt_Caption
            oButton.ChooseFromListUID = p_strCFL

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub
End Class
