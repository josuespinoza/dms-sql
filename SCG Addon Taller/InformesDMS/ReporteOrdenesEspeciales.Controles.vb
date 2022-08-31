Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany


Partial Public Class ReporteOrdenesEspeciales : Implements IUsaMenu, IFormularioSBO

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
    Private _strParametros As String

    Private _applicationSbo As Application
    Private _companySbo As ICompany

    Private _txtFDesde As EditTextSBO
    Private _txtFHasta As EditTextSBO
    Private _txtNoOT As EditTextSBO
    Private _rbtDet As OptionBtnSBO
    Private _rbtRes As OptionBtnSBO

    Private UDS_dtFormulario As UserDataSources

    Public BtnPrintSbo As ButtonSBO
    Public EditTextNumOT As EditTextSBO


#End Region

#Region "Contructor"

    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
               ByVal SBOAplication As Application, p_menuInformesDMS As String, p_strUISCGD_ReporteOrdenes As String)

        _companySbo = ocompany
        _applicationSbo = SBOAplication

        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioReporteOrdenEsp
        MenuPadre = p_menuInformesDMS
        Nombre = My.Resources.Resource.TituloReporteTrazabilidadOT
        IdMenu = p_strUISCGD_ReporteOrdenes
        Posicion = 2
        FormType = p_strUISCGD_ReporteOrdenes
        DireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        UsuarioBd = CatchingEvents.DBUser
        ContraseñaBd = CatchingEvents.DBPassword

    End Sub


#End Region

#Region "PROPIEDADES"
    Public Property IdMenu() As String Implements IUsaMenu.IdMenu
        Get
            Return _IdMenu
        End Get
        Set(ByVal value As String)
            _IdMenu = value
        End Set
    End Property

    Public Property MenuPadre() As String Implements IUsaMenu.MenuPadre
        Get
            Return _MenuPadre
        End Get
        Set(ByVal value As String)
            _MenuPadre = value
        End Set
    End Property

    Public Property Posicion() As Integer Implements IUsaMenu.Posicion
        Get
            Return _Posicion
        End Get
        Set(ByVal value As Integer)
            _Posicion = value
        End Set
    End Property

    Public Property Nombre() As String Implements IUsaMenu.Nombre
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
        End Set
    End Property

    Public Property FormType() As String Implements IFormularioSBO.FormType
        Get
            Return _FormType
        End Get
        Set(ByVal value As String)
            _FormType = value
        End Set
    End Property

    Public Property NombreXml() As String Implements IFormularioSBO.NombreXml
        Get
            Return _NombreXML
        End Get
        Set(ByVal value As String)
            _NombreXML = value
        End Set
    End Property

    Public Property Titulo() As String Implements IFormularioSBO.Titulo
        Get
            Return _Titulo
        End Get
        Set(ByVal value As String)
            _Titulo = value
        End Set
    End Property

    Public Property FormularioSBO() As IForm Implements IFormularioSBO.FormularioSBO
        Get
            Return _FormularioSBO
        End Get
        Set(ByVal value As IForm)
            _FormularioSBO = value
        End Set
    End Property

    Public Property Inicializado() As Boolean Implements IFormularioSBO.Inicializado
        Get
            Return _Inicializado
        End Get
        Set(ByVal value As Boolean)
            _Inicializado = value
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

    Public Property DireccionReportes As String
        Get
            Return _DireccionReportes
        End Get
        Set(ByVal value As String)
            _DireccionReportes = value
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

    Public Property StrParametros As String
        Get
            Return _strParametros
        End Get
        Set(ByVal value As String)
            _strParametros = value
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

#End Region

#Region "Metodos"


    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles
        Try
            Dim userDS As UserDataSources = FormularioSBO.DataSources.UserDataSources
            FormularioSBO.Freeze(True)

            'agrega columnas al ds
            userDS.Add("c_NumOT", BoDataType.dt_LONG_TEXT, 100)

            EditTextNumOT = New EditTextSBO("txtNumOT", True, "", "c_NumOT", FormularioSBO)
            EditTextNumOT.AsignaBinding()

            BtnPrintSbo = New ButtonSBO("btnPrint", FormularioSBO)


            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario
        Try
            CargarFormulario()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de Eventos de Formularios
    ''' </summary>
    ''' <param name="FormUID"></param>
    ''' <param name="pVal"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    ''' 
    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If Not pVal.FormTypeEx = FormType Then Return

        If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

            ManejadorEventoItemPress(FormUID, pVal, BubbleEvent)

        ElseIf pVal.EventType = BoEventTypes.et_COMBO_SELECT Then


        End If

    End Sub


#End Region
End Class


