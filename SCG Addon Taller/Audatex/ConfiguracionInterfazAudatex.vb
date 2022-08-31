Imports DMS_Addon.ControlesSBO
Imports System.Collections.Generic
Imports SAPbouiCOM

Imports System.Globalization
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Public Class ConfiguracionInterfazAudatex : Implements IFormularioSBO, IUsaPermisos

#Region "Clase"
#End Region

#Region "Controles"

#Region "Declaraciones"
    Private Const tablaConfig = "@SCGD_AI_AUDATEX"

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
    Public EditTextSerieOrdenCompra As SCG.SBOFramework.UI.EditTextSBO

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Contructor para la aplicacion
    ''' </summary>
    ''' <param name="application"></param>
    ''' <param name="companySbo"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub New(ByVal application As SAPbouiCOM.Application, ByVal companySbo As ICompany, ByVal mc_strUISCGD_ConfAudatexInterface As String)
        _companySbo = companySbo
        _applicationSbo = application
        NombreXml = System.Windows.Forms.Application.StartupPath & My.Resources.Resource.XMLFormInterfazAudatex
        MenuPadre = "SCGD_CFG"
        Nombre = My.Resources.Resource.MenuConfAIC
        IdMenu = mc_strUISCGD_ConfAudatexInterface
        Titulo = My.Resources.Resource.MenuConfAIC
        Posicion = 79
        FormType = mc_strUISCGD_ConfAudatexInterface
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
    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles
        'If FormularioSBO IsNot Nothing Then

        '    FormularioSBO.Freeze(True)

        '    EditTextSerieOrdenCompra = New EditTextSBO("txtFromD", True, tablaConfig, "U_FromDate", FormularioSBO)
        '    EditTextSerieOrdenCompra.AsignaBinding()

        '    FormularioSBO.Freeze(False)
        'End If
    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario
        If FormularioSBO IsNot Nothing Then
            FormularioSBO.Freeze(True)
            ApplicationSBO.Menus.Item("1291").Activate()
            FormularioSBO.Freeze(False)
        End If
    End Sub
#End Region

#End Region

End Class

