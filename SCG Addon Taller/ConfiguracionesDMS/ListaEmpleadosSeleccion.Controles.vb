Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework

Partial Public Class ListaEmpleadosSeleccion

#Region "Declaraciones"
    Private _nombreXml As String
    Private _titulo As String
    Private _strConexion As String
    Private _companySbo As SAPbobsCOM.ICompany
    Private _formType As String
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _applicationSBO As IApplication
    Private Shared _formConfMsj As Form

    Private oForm As Form
    
    Public n As NumberFormatInfo
    Private Shared dtSelEmpMatriz As DataTable
    Private Shared dtUsuariosConsulta As DataTable
    Private Shared oMatrix As Matrix
    Private g_oEditIDRol As EditText
    Private g_oEditIDSuc As EditText
    Private g_oEditDE As EditText
    Private MatrizSelListEmp As MatrizListaEmpSel
    Private Const strtb_LocalUserConsulta As String = "dtUserConsulta"
    Private Const strtb_LocalUser As String = "dtUser"
    Private Const strMatrizUsuarios As String = "mtx_User"
    Private Const strFormMsjUId As String = "SCGD_CMSJ"
    Private Const g_strdtConfRol As String = "dtConfigUsrRol"
    Private Const FormUID As String = "SCGD_VSEP"



#End Region

#Region "Propiedades"

    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements SCG.SBOFramework.UI.IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSBO
        End Get
    End Property

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

    'Propiedad Formulario
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

    Public Property StrConexion As String
        Get
            Return _strConexion
        End Get
        Set(ByVal value As String)
            _strConexion = value
        End Set
    End Property

    Public Shared Property FormConfMSJ As Form
        Get
            Return _formConfMsj
        End Get
        Set(ByVal value As Form)
            _formConfMsj = value
        End Set
    End Property

#End Region

#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByVal application As SAPbouiCOM.Application, ByVal companySbo As SAPbobsCOM.ICompany)
        _companySbo = companySbo
        _applicationSBO = application

    End Sub

#End Region

#Region "Metodos"


    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario
        If FormularioSBO IsNot Nothing Then
            Dim userDS As UserDataSources = FormularioSBO.DataSources.UserDataSources
            userDS.Add("idRol", BoDataType.dt_LONG_TEXT, 100)
            userDS.Add("idSuc", BoDataType.dt_LONG_TEXT, 100)
            userDS.Add("DE", BoDataType.dt_LONG_TEXT, 100)

            g_oEditIDRol = DirectCast(FormularioSBO.Items.Item("txtIDRol").Specific, SAPbouiCOM.EditText)
            g_oEditIDSuc = DirectCast(FormularioSBO.Items.Item("txtIDSuc").Specific, SAPbouiCOM.EditText)
            g_oEditDE = DirectCast(FormularioSBO.Items.Item("txtDE").Specific, SAPbouiCOM.EditText)

            g_oEditIDRol.DataBind.SetBound(True, "", "idRol")
            g_oEditIDSuc.DataBind.SetBound(True, "", "idSuc")
            g_oEditDE.DataBind.SetBound(True, "", "DE")

            CargaFormulario()
        End If
    End Sub

    'Inicializa los controles de la pantalla 
    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        '
    End Sub

    Public Sub CargaFormulario()
        LinkMatriz()
    End Sub

    Private Sub LinkMatriz()
        'datatable que es la matriz de usuarios
        dtUsuariosConsulta = FormularioSBO.DataSources.DataTables.Add(strtb_LocalUserConsulta)
        dtSelEmpMatriz = FormularioSBO.DataSources.DataTables.Add(strtb_LocalUser)
        dtSelEmpMatriz.Columns.Add("Col_Name", BoFieldsType.ft_AlphaNumeric, 100)
        dtSelEmpMatriz.Columns.Add("Col_UCode", BoFieldsType.ft_AlphaNumeric, 100)
        dtSelEmpMatriz.Columns.Add("Col_UN", BoFieldsType.ft_AlphaNumeric, 100)
        dtSelEmpMatriz.Columns.Add("Col_EmId", BoFieldsType.ft_AlphaNumeric, 100)
        dtSelEmpMatriz.Columns.Add("col_sele", BoFieldsType.ft_AlphaNumeric, 100)

        'Instancia de la matriz de usuarios
        MatrizSelListEmp = New MatrizListaEmpSel(strMatrizUsuarios, FormularioSBO, strtb_LocalUser)
        MatrizSelListEmp.CreaColumnas()
        MatrizSelListEmp.LigaColumnas()
    End Sub


#End Region

End Class
