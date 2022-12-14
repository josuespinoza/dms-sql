Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany


Public Class ControlVisita: Implements IFormularioSBO, IUsaMenu

    Private _applicationSBO As Application
    Private _companySBO As ICompany
    Private _formtype As String
    Private _formularioSBO As SAPbouiCOM.IForm
    Private _inicializado As Boolean

    Private _nombreXML As String
    Private _titulo As String
    Private _idmenu As String
    Private _menupadre As String
    Private _nombre As String
    Private _posicion As String

    Public Property FormType() As String Implements IFormularioSBO.FormType
        Get
            Return _formtype
        End Get
        Set(ByVal value As String)
            _formtype = value
        End Set
    End Property

    Public Property NombreXml() As String Implements IFormularioSBO.NombreXml
        Get
            Return _nombreXML
        End Get
        Set(ByVal value As String)
            _nombreXML = value
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
            Return _formularioSBO
        End Get
        Set(ByVal value As IForm)
            _formularioSBO = value
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
            Return _applicationSBO
        End Get
    End Property

    Public ReadOnly Property CompanySBO() As ICompany Implements IFormularioSBO.CompanySBO
        Get
            Return _companySBO
        End Get
    End Property


    Public Property IdMenu() As String Implements IUsaMenu.IdMenu
        Get
            Return _idmenu
        End Get
        Set(ByVal value As String)
            _idmenu = value
        End Set
    End Property

    Public Property MenuPadre() As String Implements IUsaMenu.MenuPadre
        Get
            Return _menupadre
        End Get
        Set(ByVal value As String)
            _menupadre = value
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

    Public Sub New(ByVal p_application As Application,
                    ByVal p_company As ICompany)
        _companySBO = p_company
        _applicationSBO = p_application
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioControlVisita
        MenuPadre = "SCGD_PRO"
        Nombre = "Control Visita"
        IdMenu = "SCGD_CVS"
        Titulo = "Control Visita"
        Posicion = 4
        FormType = "SCGD_CVS"
    End Sub

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario

    End Sub

    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByVal BubbleEvent As Boolean)

        If pVal.FormTypeEx <> FormType Then Exit Sub

        Select Case pVal.EventType
            Case BoEventTypes.et_ITEM_PRESSED
                ManejadorEventosItemPressed(FormUID, pVal, BubbleEvent)

        End Select

    End Sub

    Private Sub ManejadorEventosItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        FormularioSBO.Freeze(True)
        Select pVal.ItemUID

            Case "19"
                FormularioSBO.PaneLevel = 1

            Case "21"
                FormularioSBO.PaneLevel = 2

        End Select
        FormularioSBO.Freeze(False)
    End Sub

End Class
