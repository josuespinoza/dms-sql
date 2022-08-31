
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class BalanceOrdenesTrabajo : Implements IUsaMenu, IFormularioSBO

#Region "Declaraciones"

    Private _DireccionReportes As String
    Private _Conexion As String
    Private _UsuarioBD As String
    Private _ContraseñaBD As String

    Private _IdMenu As String
    Private _MenuPadre As String
    Private _Nombre As String
    Private _Posicion As String
    Private _FormType As String
    Private _FormularioSBO As SAPbouiCOM.IForm
    Private _Inicializado As Boolean
    Private _NombreXML As String
    Private _Titulo As String

    Private _applicationSbo As Application
    Private _companySbo As ICompany

    Private _txtFDesde As EditTextSBO
    Private _txtFHasta As EditTextSBO
    Private _txtNoOT As EditTextSBO
    Private _rbtDet As OptionBtnSBO
    Private _rbtRes As OptionBtnSBO

    Private UDS_dtFormulario As UserDataSources

#End Region

#Region "Constructor"

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, p_menuInformesDMS As String, p_strUIDFormBalanceOT As String)
        _companySbo = companySbo
        _applicationSbo = application
        NombreXml = System.Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormBalanceOT
        MenuPadre = p_menuInformesDMS
        Nombre = My.Resources.Resource.TituloBalanceOT
        IdMenu = p_strUIDFormBalanceOT
        Posicion = 4
        FormType = p_strUIDFormBalanceOT
        DireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        UsuarioBd = CatchingEvents.DBUser
        ContraseñaBd = CatchingEvents.DBPassword
    End Sub

#End Region

#Region "Propiedades"

    Public Property DireccionReportes As String
        Get
            Return _DireccionReportes
        End Get
        Set(ByVal value As String)
            _DireccionReportes = value
        End Set
    End Property

    Public Property Conexion As String
        Get
            Return _Conexion
        End Get
        Set(ByVal value As String)
            _Conexion = value
        End Set
    End Property

    Public Property UsuarioBd As String
        Get
            Return _UsuarioBD
        End Get
        Set(ByVal value As String)
            _UsuarioBD = value
        End Set
    End Property

    Public Property ContraseñaBd As String
        Get
            Return _ContraseñaBD
        End Get
        Set(ByVal value As String)
            _ContraseñaBD = value
        End Set
    End Property

    Public Property IdMenu As String Implements SCG.SBOFramework.UI.IUsaMenu.IdMenu
        Get
            Return _IdMenu
        End Get
        Set(ByVal value As String)
            _IdMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements SCG.SBOFramework.UI.IUsaMenu.MenuPadre
        Get
            Return _MenuPadre
        End Get
        Set(ByVal value As String)
            _MenuPadre = value
        End Set
    End Property

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
        End Set
    End Property

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _Posicion
        End Get
        Set(ByVal value As Integer)
            _Posicion = value
        End Set
    End Property

    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements SCG.SBOFramework.UI.IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
        End Get
    End Property

    Public ReadOnly Property CompanySBO As SAPbobsCOM.ICompany Implements SCG.SBOFramework.UI.IFormularioSBO.CompanySBO
        Get
            Return _companySbo
        End Get
    End Property

    Public Property FormType As String Implements SCG.SBOFramework.UI.IFormularioSBO.FormType
        Get
            Return _FormType
        End Get
        Set(ByVal value As String)
            _FormType = value
        End Set
    End Property

    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _FormularioSBO
        End Get
        Set(ByVal value As SAPbouiCOM.IForm)
            _FormularioSBO = value
        End Set
    End Property

    Public Property Inicializado As Boolean Implements SCG.SBOFramework.UI.IFormularioSBO.Inicializado
        Get
            Return _Inicializado
        End Get
        Set(ByVal value As Boolean)
            _Inicializado = value
        End Set
    End Property

    Public Property NombreXml As String Implements SCG.SBOFramework.UI.IFormularioSBO.NombreXml
        Get
            Return _NombreXML
        End Get
        Set(ByVal value As String)
            _NombreXML = value
        End Set
    End Property

    Public Property Titulo As String Implements SCG.SBOFramework.UI.IFormularioSBO.Titulo
        Get
            Return _Titulo
        End Get
        Set(ByVal value As String)
            _Titulo = value
        End Set
    End Property

    Public Property txtFDesde As EditTextSBO
        Get
            Return _txtFDesde
        End Get
        Set(ByVal value As EditTextSBO)
            _txtFDesde = value
        End Set
    End Property

    Public Property txtFHasta As EditTextSBO
        Get
            Return _txtFHasta
        End Get
        Set(ByVal value As EditTextSBO)
            _txtFHasta = value
        End Set
    End Property

    Public Property txtNoOt As EditTextSBO
        Get
            Return _txtNoOT
        End Get
        Set(ByVal value As EditTextSBO)
            _txtNoOT = value
        End Set
    End Property

    Public Property rbtDet As OptionBtnSBO
        Get
            Return _rbtDet
        End Get
        Set(ByVal value As OptionBtnSBO)
            _rbtDet = value
        End Set
    End Property

    Public Property rbtRes As OptionBtnSBO
        Get
            Return _rbtRes
        End Get
        Set(ByVal value As OptionBtnSBO)
            _rbtRes = value
        End Set
    End Property

#End Region

#Region "Métodos"
    
    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario
        CargaFormulario()
    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles

        UDS_dtFormulario = FormularioSBO.DataSources.UserDataSources

        UDS_dtFormulario.Add("fdesde", BoDataType.dt_DATE, 100)
        UDS_dtFormulario.Add("fhasta", BoDataType.dt_DATE, 100)
        UDS_dtFormulario.Add("noot", BoDataType.dt_LONG_TEXT, 100)
        UDS_dtFormulario.Add("det", BoDataType.dt_LONG_TEXT, 100)
        UDS_dtFormulario.Add("res", BoDataType.dt_LONG_TEXT, 100)

        txtFDesde = New EditTextSBO("txtFDesde", True, "", "fdesde", FormularioSBO)
        txtFHasta = New EditTextSBO("txtFHasta", True, "", "fhasta", FormularioSBO)
        txtNoOt = New EditTextSBO("txtNoOT", True, "", "noot", FormularioSBO)
        rbtDet = New OptionBtnSBO("rbtDet", True, "", "det", FormularioSBO)
        rbtRes = New OptionBtnSBO("rbtRes", True, "", "res", FormularioSBO)

        txtFDesde.AsignaBinding()
        txtFHasta.AsignaBinding()
        txtNoOt.AsignaBinding()
        rbtDet.AsignaBinding()
        rbtRes.AsignaBinding()

    End Sub

    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByVal BubbleEvent As Boolean)

        If pVal.FormTypeEx <> FormType Then Exit Sub

        Select Case pVal.EventType
            Case BoEventTypes.et_ITEM_PRESSED
                ManejadorEventosItemPressed(FormUID, pVal, BubbleEvent)

        End Select

    End Sub

#End Region

End Class
