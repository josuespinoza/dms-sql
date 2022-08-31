Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany

'Clase para manejo de controles de formulario de reporte de saldos del modulo de financiamiento

Partial Public Class Saldos : Implements IFormularioSBO, IUsaMenu

    Private _formType As String

    Private _nombreXml As String

    Private _titulo As String

    Private _formularioSbo As IForm

    Private _inicializado As Boolean

    Private _applicationSbo As IApplication

    Private _companySbo As ICompany

    Private _idMenu As String

    Private _menuPadre As String

    Private _posicion As Integer

    Private _nombre As String

    Private _strConexion As String

    Private _strDireccionReportes As String

    Private _strUsuarioBD As String

    Private _strContraseñaBD As String

    Public EditTextPrestamo As EditTextSBO
    Public EditTextCliente As EditTextSBO
    Public EditTextFechaPrestamo As EditTextSBO
    Public EditTextContrato As EditTextSBO
    Public EditTextFecha As EditTextSBO

    Public ChkTodos As CheckBoxSBO
    Public ButtonBuscar As ButtonSBO
    Public ButtonImprimirSaldo As ButtonSBO

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        _companySbo = companySbo
        _applicationSbo = application
    End Sub

    Public Property FormType() As String Implements IFormularioSBO.FormType
        Get
            Return _formType
        End Get
        Set (ByVal value As String)
            _formType = value
        End Set
    End Property

    Public Property StrConexion() As String
        Get
            Return _strConexion
        End Get
        Set(ByVal value As String)
            _strConexion = value
        End Set
    End Property

    Public Property StrDireccionReportes() As String
        Get
            Return _strDireccionReportes
        End Get
        Set(ByVal value As String)
            _strDireccionReportes = value
        End Set
    End Property

    Public Property StrUsuarioBD() As String
        Get
            Return _strUsuarioBD
        End Get
        Set(ByVal value As String)
            _strUsuarioBD = value
        End Set
    End Property

    Public Property StrContraseñaBD() As String
        Get
            Return _strContraseñaBD
        End Get
        Set(ByVal value As String)
            _strContraseñaBD = value
        End Set
    End Property

    Public Property NombreXml() As String Implements IFormularioSBO.NombreXml
        Get
            Return _nombreXml
        End Get
        Set (ByVal value As String)
            _nombreXml = value
        End Set
    End Property

    Public Property Titulo() As String Implements IFormularioSBO.Titulo
        Get
            Return _titulo
        End Get
        Set (ByVal value As String)
            _titulo = value
        End Set
    End Property

    Public Property FormularioSBO() As IForm Implements IFormularioSBO.FormularioSBO
        Get
            Return _formularioSbo
        End Get
        Set (ByVal value As IForm)
            _formularioSbo = value
        End Set
    End Property

    Public Property Inicializado() As Boolean Implements IFormularioSBO.Inicializado
        Get
            Return _inicializado
        End Get
        Set (ByVal value As Boolean)
            _inicializado = value
        End Set
    End Property

    Public ReadOnly Property ApplicationSBO() As IApplication Implements IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
        End Get
    End Property

    Public ReadOnly Property CompanySBO() As ICompany Implements IFormularioSBO.CompanySBO
        Get
            Return _companySbo
        End Get
    End Property

    Public Property IdMenu() As String Implements IUsaMenu.IdMenu
        Get
            Return _idMenu
        End Get
        Set (ByVal value As String)
            _idMenu = value
        End Set
    End Property

    Public Property MenuPadre() As String Implements IUsaMenu.MenuPadre
        Get
            Return _menuPadre
        End Get
        Set (ByVal value As String)
            _menuPadre = value
        End Set
    End Property

    Public Property Posicion() As Integer Implements IUsaMenu.Posicion
        Get
            Return _posicion
        End Get
        Set (ByVal value As Integer)
            _posicion = value
        End Set
    End Property

    Public Property Nombre() As String Implements IUsaMenu.Nombre
        Get
            Return _nombre
        End Get
        Set (ByVal value As String)
            _nombre = value
        End Set
    End Property

    'Inicializa controles de pantalla de reporte de saldos

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

        Dim userDataSources As UserDataSources = FormularioSBO.DataSources.UserDataSources
        userDataSources.Add("prestamo", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("cliente", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("fechaPrest", BoDataType.dt_DATE, 100)
        userDataSources.Add("contrato", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("fecha", BoDataType.dt_DATE, 100)
        userDataSources.Add("todas", BoDataType.dt_LONG_TEXT, 50)
        EditTextPrestamo = New EditTextSBO("txtPrest", True, "", "prestamo", FormularioSBO)
        EditTextCliente = New EditTextSBO("txtCliente", True, "", "cliente", FormularioSBO)
        EditTextFechaPrestamo = New EditTextSBO("txtFecPre", True, "", "fechaPrest", FormularioSBO)
        EditTextContrato = New EditTextSBO("txtContV", True, "", "contrato", FormularioSBO)
        EditTextFecha = New EditTextSBO("txtFecha", True, "", "fecha", FormularioSBO)
        ChkTodos = New CheckBoxSBO("cbTodos", True, "", "todas", FormularioSBO)
        EditTextPrestamo.AsignaBinding()
        EditTextCliente.AsignaBinding()
        EditTextFechaPrestamo.AsignaBinding()
        EditTextContrato.AsignaBinding()
        EditTextFecha.AsignaBinding()
        ChkTodos.AsignaBinding()
        ChkTodos.AsignaValorUserDataSource("N")
        ButtonBuscar = New ButtonSBO("btnBuscar", FormularioSBO)
        ButtonImprimirSaldo = New ButtonSBO("btnImpSald", FormularioSBO)

    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario



    End Sub

    'Manejo de eventos de pantalla de reporte de saldos

    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If Not pVal.FormTypeEx = FormType Then Return

        If pVal.EventType = BoEventTypes.et_CHOOSE_FROM_LIST Then

            If pVal.ItemUID = ButtonBuscar.UniqueId Then

                CFLPrestamo(FormUID, pVal)

            End If

        End If

        If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then
            Select Case pVal.ItemUID
                Case ButtonImprimirSaldo.UniqueId
                    ButtonSBOImprimirSaldosItemPresed(FormUID, pVal, BubbleEvent)
                Case ChkTodos.UniqueId
                    If pVal.BeforeAction Then
                        ManejadorCheckBoxTodos()
                    End If
            End Select
        End If

    End Sub

    Private Sub ManejadorCheckBoxTodos()
        If ChkTodos.ObtieneValorUserDataSource() = "Y" Then
            ButtonBuscar.ItemSBO.Enabled = False
            EditTextPrestamo.AsignaValorUserDataSource("")
            EditTextCliente.AsignaValorUserDataSource("")
            EditTextFechaPrestamo.AsignaValorUserDataSource("")
            EditTextContrato.AsignaValorUserDataSource("")
        Else
            ButtonBuscar.ItemSBO.Enabled = True
        End If

    End Sub

End Class
