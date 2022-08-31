Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class VehiculoArticuloVenta

#Region "Declaraciones"

    Public Shared oForm As SAPbouiCOM.Form
    Public Shared m_oCompany As SAPbobsCOM.Company

    Private m_strBDConfiguracion As String
    Private m_strBDTalller As String
    Private m_SBO_Application As SAPbouiCOM.Application
    Private Shared m_strFormType As String
    Private Shared m_intTipoConfiguracion As Integer

    Private Shared _formConfiguracion As SAPbouiCOM.Form

    Private MatrizNumeracion As MatrixVehiculoArticuloVenta

    Private oMatrix As SAPbouiCOM.Matrix
    Private Shared dtArticulos As SAPbouiCOM.DataTable

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

#End Region

End Class
