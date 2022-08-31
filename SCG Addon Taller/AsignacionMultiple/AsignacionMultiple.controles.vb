Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.CitasTableAdapters
Imports DMSOneFramework

Partial Public Class AsignacionMultiple : Implements IFormularioSBO

#Region "... Declaraciones ..."
    'propiedades
    Private _nombreXml As String
    Private _titulo As String
    Private _strConexion As String
    Private _companySbo As SAPbobsCOM.ICompany
    Private _formType As String
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _applicationSBO As IApplication

    Public num_OT As String

    'controles de interfaz
    Private g_oEditNoOT As EditText
    Private g_oEditNoCot As EditText
    Private g_oEditIdSuc As EditText
   Private g_oChkSelAll As CheckBox
    Private g_oMtxJobs As Matrix
    Private Shared cboColabora As ComboBoxSBO
    Private sboItem As Item
    Private sboCombo As ComboBox
    Private sboComboFas As ComboBox
    Private sboItemFas As Item
    'userDataSource
    Private UDS_AsignaJobs As UserDataSources

    'matriz AsignaMultiple
    Private MatrizAsignaMultiple As MatrizAsignaMultiple

    'tabla para repuestos
    Private dtAsigMultiple As DataTable

    'constantes
    Private Const strDataTableLineas As String = "tTodosLineas"
    Public Const mc_strCboColabor As String = "cboColabor"
    Public Const mc_strMatrizJobsLines As String = "mtxTareas"
    Private Shared _formCotizacion As Form
    Private _IDSucursal As String

#End Region

#Region "Constructor"

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

    Public Shared Property FormCotizacion As Form
        Get
            Return _formCotizacion
        End Get
        Set(ByVal value As Form)
            _formCotizacion = value
        End Set
    End Property

    Public Property IDSucursal As String
        Get
            Return _IDSucursal
        End Get
        Set(ByVal value As String)
            _IDSucursal = value
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

            'asocia controles de interfaz
            AsociaControlesInterfaz()

            'matriz para todos los repuestos
            dtAsigMultiple = FormularioSBO.DataSources.DataTables.Add(strDataTableLineas)
            dtAsigMultiple.Columns.Add("col_code", BoFieldsType.ft_AlphaNumeric, 100)
            dtAsigMultiple.Columns.Add("col_sele", BoFieldsType.ft_AlphaNumeric, 100)
            dtAsigMultiple.Columns.Add("col_desc", BoFieldsType.ft_AlphaNumeric, 100)
            dtAsigMultiple.Columns.Add("col_esta", BoFieldsType.ft_AlphaNumeric, 100)
            dtAsigMultiple.Columns.Add("col_fase", BoFieldsType.ft_AlphaNumeric, 100)
            dtAsigMultiple.Columns.Add("col_idfa", BoFieldsType.ft_AlphaNumeric, 100)
            dtAsigMultiple.Columns.Add("col_desfa", BoFieldsType.ft_AlphaNumeric, 100)
            dtAsigMultiple.Columns.Add("col_asig", BoFieldsType.ft_AlphaNumeric, 100)
            dtAsigMultiple.Columns.Add("col_idac", BoFieldsType.ft_AlphaNumeric, 100)
            dtAsigMultiple.Columns.Add("col_dura", BoFieldsType.ft_AlphaNumeric, 100)
            dtAsigMultiple.Columns.Add("col_IDEmpA", BoFieldsType.ft_AlphaNumeric, 100)
            dtAsigMultiple.Columns.Add("col_NoOrd", BoFieldsType.ft_AlphaNumeric, 100)
            dtAsigMultiple.Columns.Add("col_NoCot", BoFieldsType.ft_AlphaNumeric, 100)
            dtAsigMultiple.Columns.Add("col_LnNum", BoFieldsType.ft_AlphaNumeric, 100)

            'crea matriz
            MatrizAsignaMultiple = New MatrizAsignaMultiple(mc_strMatrizJobsLines, FormularioSBO, strDataTableLineas)
            MatrizAsignaMultiple.CreaColumnas()
            MatrizAsignaMultiple.LigaColumnas()

            dtLocal = FormularioSBO.DataSources.DataTables.Add("local")
            dtLocal = FormularioSBO.DataSources.DataTables.Add("dtConsulta")

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Throw
            'Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub


   ''' <summary>
    ''' Asocia controles con la interfaz
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AsociaControlesInterfaz()
        Try
            ''crea userDataSource de Mecaicos
            UDS_AsignaJobs = FormularioSBO.DataSources.UserDataSources
            UDS_AsignaJobs.Add("ASM", BoDataType.dt_LONG_TEXT, 100)

            ''CargaFormulario dataSource de mecanicos al combo
            cboColabora = New ComboBoxSBO(mc_strCboColabor, FormularioSBO, True, "", "ASM")
            cboColabora.AsignaBinding()

        Catch ex As Exception
            Throw
            'Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

#End Region
End Class
