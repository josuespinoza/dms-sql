
Imports SCG.SBOFramework
Imports System.Globalization
Imports SCG.SBOFramework.UI
Imports SAPbouiCOM
Public Class HistorialAprobaciones : Implements IFormularioSBO

#Region "... Declaraciones ..."
    'propiedades
    Private m_strDireccionConfiguracion As String
    Private _nombreXml As String
    Private _titulo As String
    Private _strConexion As String
    Private _companySbo As SAPbobsCOM.ICompany
    Private _formType As String
    Private _formularioSbo As SAPbouiCOM.IForm
    Private _inicializado As Boolean
    Private _applicationSBO As SAPbouiCOM.IApplication

    Public num_OT As String
    Private m_objMensajeria As New Proyecto_SCGMSGBox.SCGMSGBox

    'controles de interfaz


    Private g_oMtxHist As SAPbouiCOM.Matrix
    Private m_dbLineas As SAPbouiCOM.DBDataSource
    Private Shared cboColabora As ComboBoxSBO

    Private sboItem As SAPbouiCOM.Item
    Private sboCombo As SAPbouiCOM.ComboBox

    'userDataSource
    Private UDS_AsignaJobs As UserDataSources

    'matriz AsignaMultiple
    Private MatrizAsignaMultiple As MatrizHistCV

    'tabla para repuestos
    Private dtHistorial As DataTable

    'constantes
    Private Const strDataTableLineas As String = "tHistorial"
    Public Const mc_strMatrizHist As String = "mtxHist"
    Private Const g_strFormcomentariosApr As String = "SCGD_CHCV"

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Contructor para la aplicacion
    ''' </summary>
    ''' <param name="application"></param>
    ''' <param name="companySbo"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal application As System.Windows.Forms.Application, ByVal companySbo As SAPbobsCOM.ICompany)
        _companySbo = companySbo
        _applicationSBO = application

        n = DIHelper.GetNumberFormatInfo(_companySbo)
    End Sub

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
#End Region

#Region "Métodos"

    ''' <summary>
    ''' Carga el formulario
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaFormulario()
        Dim dtLocal As DataTable
        'Dim dtMecanicosAsig As DataTable
        Try
            FormularioSBO.Freeze(True)

            'matriz para todos los repuestos
            dtHistorial = FormularioSBO.DataSources.DataTables.Add(strDataTableLineas)
            dtHistorial.Columns.Add("Col_Usr", BoFieldsType.ft_AlphaNumeric, 100)
            dtHistorial.Columns.Add("Col_Niv", BoFieldsType.ft_AlphaNumeric, 100)
            dtHistorial.Columns.Add("Col_Dat", BoFieldsType.ft_AlphaNumeric, 100)
            dtHistorial.Columns.Add("Col_Hor", BoFieldsType.ft_AlphaNumeric, 100)
            dtHistorial.Columns.Add("Col_Com", BoFieldsType.ft_AlphaNumeric, 254)

            'crea matriz
            MatrizAsignaMultiple = New MatrizHistCV(mc_strMatrizHist, FormularioSBO, strDataTableLineas)
            MatrizAsignaMultiple.CreaColumnas()
            MatrizAsignaMultiple.LigaColumnas()

            dtLocal = FormularioSBO.DataSources.DataTables.Add("local")
            dtLocal = FormularioSBO.DataSources.DataTables.Add("dtConsulta")

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

#End Region
End Class


