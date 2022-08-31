Imports SAPbouiCOM

Partial Public Class ComentariosInventarioV
#Region "Declaraciones"

    Public Shared oForm As SAPbouiCOM.Form
    Public Shared m_oCompany As SAPbobsCOM.Company
    Private m_strBDConfiguracion As String
    Private m_strBDTalller As String
    Private m_SBO_Application As SAPbouiCOM.Application
    Private Shared m_strFormType As String
    Private Shared m_intTipoConfiguracion As Integer
    Private Shared _formIV As SAPbouiCOM.Form
    Private oEditComentarios As SAPbouiCOM.EditText
    Private oEditTitulo As SAPbouiCOM.StaticText

    Private Shared dtLisPre As SAPbouiCOM.DataTable

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

    Public Shared Property FormIV As Form
        Get
            Return _formIV
        End Get
        Set(ByVal value As Form)
            _formIV = value
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

    Private estado As Integer
    Public Property EstadoCV() As Integer
        Get
            Return estado
        End Get
        Set(ByVal value As Integer)
            estado = value
        End Set
    End Property
#End Region

End Class
