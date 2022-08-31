Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class VehiculoSeleccionMarcaEstilo

#Region "Declaracion Variables"

    Public Shared oForm As SAPbouiCOM.Form
    Public Shared m_oCompany As SAPbobsCOM.Company
    Private m_strBDConfiguracion As String
    Private m_strBDTalller As String
    Private _applicationSBO As SAPbouiCOM.Application
    Private _NombreXML As String
    Private _FormularioSBO As SAPbouiCOM.IForm
    Private Shared m_strFormType As String
    Private Shared m_intTipoConfiguracion As Integer
    Private Shared _formConfiguracion As SAPbouiCOM.Form
    Private Shared _MatrixVeh As MatrizEntradaVehiculos

    ' Private m_udsLocal As SAPbouiCOM.UserDataSources
    Private Shared dtMarca As DataTable
    Private Shared dtEstilo As DataTable
    Private Shared dtModelo As DataTable
    Private Shared dtLocal As DataTable

#End Region



#Region "Constructor"

    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application)
        m_oCompany = ocompany
        _applicationSBO = SBOAplication

    End Sub

#End Region

#Region "Propiedades"

    Public Property SAPCompany() As SAPbobsCOM.Company
        Get
            Return m_oCompany
        End Get
        Set(ByVal value As SAPbobsCOM.Company)
            m_oCompany = value
        End Set
    End Property

    Public Shared Property FormConfiguracion As Form
        Get
            Return _formConfiguracion
        End Get
        Set(ByVal value As Form)
            _formConfiguracion = value
        End Set
    End Property

    Public Shared Property MatrizVeh As MatrizEntradaVehiculos
        Get
            Return _MatrixVeh
        End Get
        Set(ByVal value As MatrizEntradaVehiculos)
            _MatrixVeh = value
        End Set
    End Property

    Public Property StrFormType As String
        Get
            Return m_strFormType
        End Get
        Set(ByVal value As String)
            m_strFormType = value
        End Set
    End Property

    Public Property NombreXML As String
        Get
            Return _NombreXML
        End Get
        Set(ByVal value As String)
            _NombreXML = value
        End Set
    End Property

    Public Property FormularioSBO As SAPbouiCOM.IForm
        Get
            Return _FormularioSBO
        End Get
        Set(ByVal value As SAPbouiCOM.IForm)
            _FormularioSBO = value
        End Set
    End Property


#End Region


End Class
