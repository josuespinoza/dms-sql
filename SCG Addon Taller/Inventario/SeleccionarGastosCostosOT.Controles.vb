
Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class SeleccionarGastosCostosOT : Implements IFormularioSBO

#Region "Declaraciones"
    Public Shared oForm As SAPbouiCOM.Form

    'maneja informacion de la aplicacion
    Private _applicationSbo As Application
    'maneja informacion de la compania 
    Private _companySbo As ICompany
    Private _formType As String
    Private _formularioSBO As SAPbouiCOM.IForm
    Private _inicializado As Boolean

    'propiedades
    Private _nombreXml As String
    Private _titulo As String
    Private _idMenu As String
    Private _menuPadre As String
    Private _nombre As String
    Private _posicion As Integer

    Private _codeCliente As String

    'tabla para repuestos
    Private dtGastos As DataTable
    Private dtGastosTodos As DataTable
    Private Const strMatrizGastos As String = "mtxGas"
    Private Const strDataTableTodos As String = "tTodosGastos"
    Private Const strMatrizGasTodos As String = "mtxLsGas"
    'matriz repuestos
    'Private MatrizGastosSeleccionados As MatrizSeleccionaGastosOT
    Private MatrizGastosTodos As MatrizSeleccionaGastosOT

    'controles de interfaz
    Private Shared txtCod As EditTextSBO
    Private Shared txtDes As EditTextSBO

    Dim strConfiguracion As String

    'userDataSource
    Private UDS_SeleccionaGastos As UserDataSources

    Dim n As NumberFormatInfo

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Contructor para la aplicacion
    ''' </summary>
    ''' <param name="application"></param>
    ''' <param name="companySbo"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        _companySbo = companySbo
        _applicationSbo = application

        n = DIHelper.GetNumberFormatInfo(_companySbo)
    End Sub

#End Region

#Region "Propiedades"
    'propiedades de la aplicación

    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements SCG.SBOFramework.UI.IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
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

    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _formularioSBO
        End Get
        Set(ByVal value As SAPbouiCOM.IForm)
            _formularioSBO = value
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
#End Region

#Region "Metodos"
    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario
        Try

            AsociaControles()
            CargarFormulario()
            CargarMatrizGastos(FormularioSBO, FormularioSBO.UniqueID)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa los controles 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles

    End Sub

    Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If Not pVal.FormTypeEx = "SCGD_SGOT" Then Return

        Select Case pVal.EventType
            Case BoEventTypes.et_ITEM_PRESSED
                ManejadorEventosItemPressed(FormUID, pVal, BubbleEvent)
            Case BoEventTypes.et_MATRIX_LOAD
                ' ManejadorEventosMatrixLoad(FormUID, pVal, BubbleEvent)
        End Select
        ' End If

    End Sub
    Private Sub ManejadorEventosItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try

            If pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case "btn"
                    Case "btn"

                End Select
            ElseIf pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case "btnSel"
                        AgregaGastosCotizacion(FormUID, False, BubbleEvent)
                    Case "btnBus"
                        EjecutarFiltros(oForm, FormUID)

                End Select
            End If


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub CargarFormulario()
        Dim dtLocal As DataTable

        Try
            oForm = ApplicationSBO.Forms.Item("SCGD_SGOT")

            FormularioSBO.Freeze(True)

            'matriz para todos los repuestos
            dtGastosTodos = FormularioSBO.DataSources.DataTables.Add(strDataTableTodos)
            dtGastosTodos.Columns.Add("sel", BoFieldsType.ft_AlphaNumeric, 100)
            'dtGastosTodos.Columns.Add("per", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastosTodos.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastosTodos.Columns.Add("des", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastosTodos.Columns.Add("pre", BoFieldsType.ft_Price, 100)

            'crea matriz
            MatrizGastosTodos = New MatrizSeleccionaGastosOT(strMatrizGasTodos, FormularioSBO, strDataTableTodos)
            MatrizGastosTodos.CreaColumnas()
            MatrizGastosTodos.LigaColumnas()

            MatrizGastosTodos.Matrix.Columns.Item("Col_sel").Editable = True
            dtLocal = FormularioSBO.DataSources.DataTables.Add("local")

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    Private Sub AsociaControles()
        Try
            UDS_SeleccionaGastos = FormularioSBO.DataSources.UserDataSources
            UDS_SeleccionaGastos.Add("Cod", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaGastos.Add("Des", BoDataType.dt_LONG_TEXT, 100)

            txtCod = New EditTextSBO("txtCod", True, "", "Cod", FormularioSBO)
            txtCod.AsignaBinding()
            txtDes = New EditTextSBO("txtDes", True, "", "Des", FormularioSBO)
            txtDes.AsignaBinding()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

#End Region




    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class


