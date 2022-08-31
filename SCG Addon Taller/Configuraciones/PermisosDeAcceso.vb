Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class PermisosDeAcceso : Implements IUsaPermisos, IFormularioSBO

#Region "Declaraciones"
    Private _FormType As String
    Private _NombreXml As String
    Private _Titulo As String
    Private _FormularioSBO As IForm
    Private _Inicializado As Boolean
    Private _ApplicationSBO As IApplication
    Private _CompanySBO As SAPbobsCOM.ICompany
    Private _IdMenu As String
    Private _MenuPadre As String
    Private _Posicion As Integer
    Private _Nombre As String
#End Region

#Region "Propiedades"

    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements SCG.SBOFramework.UI.IFormularioSBO.ApplicationSBO
        Get
            Return _ApplicationSBO
        End Get
    End Property

    Public ReadOnly Property CompanySBO As SAPbobsCOM.ICompany Implements SCG.SBOFramework.UI.IFormularioSBO.CompanySBO
        Get
            Return _CompanySBO
        End Get
    End Property

    Public Property FormType As String Implements SCG.SBOFramework.UI.IFormularioSBO.FormType
        Get
            Return _FormType
        End Get
        Set(value As String)
            _FormType = value
        End Set
    End Property

    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _FormularioSBO
        End Get
        Set(value As SAPbouiCOM.IForm)
            _FormularioSBO = value
        End Set
    End Property

    Public Property Inicializado As Boolean Implements SCG.SBOFramework.UI.IFormularioSBO.Inicializado
        Get
            Return _Inicializado
        End Get
        Set(value As Boolean)
            _Inicializado = value
        End Set
    End Property

    Public Property NombreXml As String Implements SCG.SBOFramework.UI.IFormularioSBO.NombreXml
        Get
            Return _NombreXml
        End Get
        Set(value As String)
            _NombreXml = value
        End Set
    End Property

    Public Property Titulo As String Implements SCG.SBOFramework.UI.IFormularioSBO.Titulo
        Get
            Return _Titulo
        End Get
        Set(value As String)
            _Titulo = value
        End Set
    End Property

    Public Property IdMenu As String Implements SCG.SBOFramework.UI.IUsaMenu.IdMenu
        Get
            Return _IdMenu
        End Get
        Set(value As String)
            _IdMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements SCG.SBOFramework.UI.IUsaMenu.MenuPadre
        Get
            Return _MenuPadre
        End Get
        Set(value As String)
            _MenuPadre = value
        End Set
    End Property

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _Nombre
        End Get
        Set(value As String)
            _Nombre = value
        End Set
    End Property

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _Posicion
        End Get
        Set(value As Integer)
            _Posicion = value
        End Set
    End Property

#End Region

#Region "Contrstructor"
    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="p_Application"></param>
    ''' <param name="p_CompanySbo"></param>
    ''' <param name="mc_strSCGD_NIVELES_PV"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub New(ByVal p_Application As Application, ByVal p_CompanySbo As SAPbobsCOM.ICompany, ByVal mc_strPermisosDeAcceso As String)
        _CompanySBO = p_CompanySbo
        _ApplicationSBO = p_Application
        NombreXml = System.Windows.Forms.Application.StartupPath & My.Resources.Resource.XMLNivelesPlanVentas
        'MenuPadre = "SCGD_CFG"
        'Nombre = My.Resources.Resource.txtPermisosAcceso
        'IdMenu = mc_strUISCGD_FormPermisos
        'Titulo = My.Resources.Resource.txtPermisosAcceso
        'Posicion = 5
        FormType = mc_strPermisosDeAcceso
    End Sub
#End Region
#Region "Eventos"
    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles

    End Sub
#End Region



End Class
