Imports SAPbouiCOM

Partial Public Class NumeracionSeries

#Region "Declaraciones"

    Public Shared oForm As SAPbouiCOM.Form
    Public Shared m_oCompany As SAPbobsCOM.Company
    Private m_strBDConfiguracion As String
    Private m_strBDTalller As String
    Private m_SBO_Application As SAPbouiCOM.Application
    Private Shared m_strFormType As String
    Private Shared m_intTipoConfiguracion As Integer

    Private Shared _formConfiguracion As SAPbouiCOM.Form
    
    Private MatrizNumeracion As MatrixNumeracionSeries

    Private oMatrix As SAPbouiCOM.Matrix

    Private Shared dtNumeracion As SAPbouiCOM.DataTable

    Private Enum TipoConfiguracionSerie

        OrdenVenta = 1
        OrdenCompra = 2
        OfertaVenta = 3
        OfertaCompra = 4
        InvBodega = 5

    End Enum

#End Region
    
#Region "Constructor"

    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application)
        m_oCompany = ocompany
        m_SBO_Application = SBOAplication

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

    Public Property StrFormType As String
        Get
            Return m_strFormType
        End Get
        Set(ByVal value As String)
            m_strFormType = value
        End Set
    End Property

    Public Property IntTipoConfiguracion As Integer
        Get
            Return m_intTipoConfiguracion
        End Get
        Set(ByVal value As Integer)
            m_intTipoConfiguracion = value
        End Set
    End Property

#End Region

    


End Class
