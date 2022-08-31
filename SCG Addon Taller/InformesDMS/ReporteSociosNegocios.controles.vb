'Herencia de las librerias necesarias para el formulario
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany


Partial Public Class ReporteSociosNegocios : Implements IFormularioSBO, IUsaMenu
#Region "Declaraciones"

    'maneja informacion de la aplicacion
    Private _applicationSbo As Application

    'maneja informacion de la compania 
    Private _companySbo As ICompany
    Private _formType As String
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _nombreXml As String
    Private _titulo As String
    Private _strConexion As String

    'Campos EditText - Botones de la pantalla

    Public BtnPrintSbo As ButtonSBO

    Private _IdMenu As String
    Private _MenuPadre As String
    Private _Posicion As Integer
    Private _Nombre As String

    Private _Conexion As String
    Private _DireccionReportes As String
    Private _UsuarioBD As String
    Private _ContraseñaBD As String

    Public cboSucursal As ComboBoxSBO
    Public cboBodega As ComboBoxSBO

    Public txtNumeroOT As EditTextSBO

    Public cbxBodega As CheckBoxSBO


#End Region

#Region "Propiedades"

    Public ReadOnly Property CompanySBO As SAPbobsCOM.ICompany Implements SCG.SBOFramework.UI.IFormularioSBO.CompanySBO
        Get
            Return _companySbo
        End Get
    End Property

    Public Property FormType As String Implements SCG.SBOFramework.UI.IFormularioSBO.FormType
        Get
            Return _formType
        End Get
        Set(ByVal value As String)
            _formType = value
        End Set
    End Property

    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _formularioSbo
        End Get
        Set(ByVal value As SAPbouiCOM.IForm)
            _formularioSbo = value
        End Set
    End Property

    Public Property Inicializado As Boolean Implements SCG.SBOFramework.UI.IFormularioSBO.Inicializado
        Get
            Return _inicializado
        End Get
        Set(ByVal value As Boolean)
            _inicializado = value
        End Set
    End Property

    Public Property NombreXml As String Implements SCG.SBOFramework.UI.IFormularioSBO.NombreXml
        Get
            Return _nombreXml
        End Get
        Set(ByVal value As String)
            _nombreXml = value
        End Set
    End Property

    Public Property Titulo As String Implements SCG.SBOFramework.UI.IFormularioSBO.Titulo
        Get
            Return _titulo
        End Get
        Set(ByVal value As String)
            _titulo = value
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

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _Posicion
        End Get
        Set(ByVal value As Integer)
            _Posicion = value
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

    Public ReadOnly Property ApplicationSBO() As IApplication Implements IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
        End Get
    End Property

#End Region

#Region "Metodos"
    <CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, ByVal SBOAplication As Application, p_menuInformesDMS As String, p_strUID As String)
        Try
            m_oCompany = ocompany
            m_SBO_Application = SBOAplication
            m_strDireccionConfiguracion = CatchingEvents.DireccionConfiguracion
            NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLReporteSociosNegocios
            MenuPadre = p_menuInformesDMS
            Nombre = My.Resources.Resource.MenuReporteSociosNegocios
            IdMenu = p_strUID
            Titulo = My.Resources.Resource.MenuReporteSociosNegocios
            Posicion = 40
            FormType = p_strUID
            DireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
            UsuarioBd = CatchingEvents.DBUser
            ContraseñaBd = CatchingEvents.DBPassword
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario
        Try
            If FormularioSBO IsNot Nothing Then
                CargarFormulario()
                CargarComboMarcas()
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        Try
            'Dim userDS As UserDataSources = FormularioSBO.DataSources.UserDataSources
            'FormularioSBO.Freeze(True)

            ''agrega columnas al ds
            'userDS.Add("c_Sucursal", BoDataType.dt_LONG_TEXT, 100)
            'userDS.Add("c_Bodega", BoDataType.dt_LONG_TEXT, 100)
            'userDS.Add("c_NumeroOT", BoDataType.dt_LONG_TEXT, 100)
            'userDS.Add("c_allBod", BoDataType.dt_LONG_TEXT, 10)
            'userDS.Add("c_allNOT", BoDataType.dt_LONG_TEXT, 10)

            'cboSucursal = New ComboBoxSBO("cboSucur", FormularioSBO, True, "", "c_Sucursal")
            'cboBodega = New ComboBoxSBO("cboBodegaP", FormularioSBO, True, "", "c_Bodega")

            'txtNumeroOT = New EditTextSBO("txtNoOT", True, "", "c_NumeroOT", FormularioSBO)

            'cbxBodega = New CheckBoxSBO("cbxBodega", True, "", "c_allBod", FormularioSBO)

            'BtnPrintSbo = New ButtonSBO("btnPrint", FormularioSBO)

            ''********************************
            'cboSucursal.AsignaBinding()
            'cboBodega.AsignaBinding()
            'txtNumeroOT.AsignaBinding()

            'cbxBodega.AsignaBinding()

            'FormularioSBO.Freeze(False)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Sub ApplicationSboOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.FormTypeEx = FormType Then
                ManejadorEventoItemPress(FormUID, pVal, BubbleEvent)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

#End Region
End Class
