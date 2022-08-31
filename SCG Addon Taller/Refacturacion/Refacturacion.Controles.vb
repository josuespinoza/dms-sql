Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI.Extensions
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany
Imports DMS_Addon.ControlesSBO

Partial Public Class Refacturacion : Implements IFormularioSBO, IUsaPermisos

    Private _applicationSbo As Application
    Private _companySbo As ICompany
    Private Shared _formType As String
    Private _nombreXml As String
    Private _titulo As String
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _idMenu As String
    Private _menuPadre As String
    Private _posicion As Integer
    Private _nombre As String

    Public EditTextContrato As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextFecha As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextFechaInicio As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextFechaFin As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextAnoVeh As SCG.SBOFramework.UI.EditTextSBO


    Private ButtonBuscar As SCG.SBOFramework.UI.ButtonSBO
    Private ButtonRefacturar As SCG.SBOFramework.UI.ButtonSBO
    Private ButtonCargar As SCG.SBOFramework.UI.ButtonSBO
    Private ButtonPrueba As SCG.SBOFramework.UI.ButtonSBO

    Private CheckBoxRefacturarTodos As SCG.SBOFramework.UI.CheckBoxSBO
    Private CheckBoxUsaFiltroCV As SCG.SBOFramework.UI.CheckBoxSBO
    Private CheckBoxAutoFacturas As SCG.SBOFramework.UI.CheckBoxSBO


    Private MatrixFacturas As MatrixSBOFacturas

    Private dataTableFacturas As DataTable
    Private dataTableContrato As DataTable
    Private dataTableValidaFact As DataTable
    Private dataTableUnidades As DataTable

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        _companySbo = companySbo
        _applicationSbo = application
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLRefacturacion
        MenuPadre = "SCGD_CTT"
        Nombre = "Refacturación"
        IdMenu = "SCGD_RFT"
        Titulo = My.Resources.Resource.TituloRefacturacion
        Posicion = 35
        FormType = "SCGD_Refact"
    End Sub

    Public Property FormType() As String Implements IFormularioSBO.FormType
        Get
            Return _formType
        End Get
        Set(ByVal value As String)
            _formType = value
        End Set
    End Property

    Public Property NombreXml() As String Implements IFormularioSBO.NombreXml
        Get
            Return _nombreXml
        End Get
        Set(ByVal value As String)
            _nombreXml = value
        End Set
    End Property

    Public Property Titulo() As String Implements IFormularioSBO.Titulo
        Get
            Return _titulo
        End Get
        Set(ByVal value As String)
            _titulo = value
        End Set
    End Property

    Public Property FormularioSBO() As IForm Implements IFormularioSBO.FormularioSBO
        Get
            Return _formularioSbo
        End Get
        Set(ByVal value As IForm)
            _formularioSbo = value
        End Set
    End Property

    Public Property Inicializado() As Boolean Implements IFormularioSBO.Inicializado
        Get
            Return _inicializado
        End Get
        Set(ByVal value As Boolean)
            _inicializado = value
        End Set
    End Property

    Public Property IdMenu() As String Implements IUsaMenu.IdMenu
        Get
            Return _idMenu
        End Get
        Set(ByVal value As String)
            _idMenu = value
        End Set
    End Property

    Public Property MenuPadre() As String Implements IUsaMenu.MenuPadre
        Get
            Return _menuPadre
        End Get
        Set(ByVal value As String)
            _menuPadre = value
        End Set
    End Property

    Public Property Posicion() As Integer Implements IUsaMenu.Posicion
        Get
            Return _posicion
        End Get
        Set(ByVal value As Integer)
            _posicion = value
        End Set
    End Property

    Public Property Nombre() As String Implements IUsaMenu.Nombre
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
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

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario



    End Sub

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

        dataTableContrato = FormularioSBO.DataSources.DataTables.Add("Contrato")
        dataTableValidaFact = FormularioSBO.DataSources.DataTables.Add("Valida")
        dataTableUnidades = FormularioSBO.DataSources.DataTables.Add("Unidades")

        Dim userDataSources As UserDataSources = FormularioSBO.DataSources.UserDataSources
        userDataSources.Add("contrato", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("todos", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("fecha", BoDataType.dt_DATE, 100)
        userDataSources.Add("fechaI", BoDataType.dt_DATE, 100)
        userDataSources.Add("fechaF", BoDataType.dt_DATE, 100)
        userDataSources.Add("usaCV", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("anoVeh", BoDataType.dt_SHORT_NUMBER, 4)
        userDataSources.Add("autoFact", BoDataType.dt_LONG_TEXT, 100)

        EditTextContrato = New SCG.SBOFramework.UI.EditTextSBO("txtCont", True, "", "contrato", FormularioSBO)
        EditTextFecha = New SCG.SBOFramework.UI.EditTextSBO("txtFecha", True, "", "fecha", FormularioSBO)
        CheckBoxRefacturarTodos = New SCG.SBOFramework.UI.CheckBoxSBO("chkReFact", True, "", "todos", FormularioSBO)
        EditTextFechaInicio = New SCG.SBOFramework.UI.EditTextSBO("txtFechaIn", True, "", "fechaI", FormularioSBO)
        EditTextFechaFin = New SCG.SBOFramework.UI.EditTextSBO("txtFechaFi", True, "", "fechaF", FormularioSBO)
        CheckBoxUsaFiltroCV = New SCG.SBOFramework.UI.CheckBoxSBO("chk_UsaCV", True, "", "usaCV", FormularioSBO)
        EditTextAnoVeh = New SCG.SBOFramework.UI.EditTextSBO("txtAnnoVeh", True, "", "anoVeh", FormularioSBO)
        CheckBoxAutoFacturas = New SCG.SBOFramework.UI.CheckBoxSBO("chk_SelAut", True, "", "autoFact", FormularioSBO)

        EditTextContrato.AsignaBinding()
        EditTextFecha.AsignaBinding()
        CheckBoxRefacturarTodos.AsignaBinding()
        EditTextFechaInicio.AsignaBinding()
        EditTextFechaFin.AsignaBinding()
        CheckBoxUsaFiltroCV.AsignaBinding()
        EditTextAnoVeh.AsignaBinding()
        CheckBoxAutoFacturas.AsignaBinding()


        dataTableFacturas = FormularioSBO.DataSources.DataTables.Add("Facturas")
        dataTableFacturas.Columns.Add("col_Refac", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableFacturas.Columns.Add("NoContrato", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableFacturas.Columns.Add("FechaContabilizacion", BoFieldsType.ft_Date, 100)
        dataTableFacturas.Columns.Add("vieja", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableFacturas.Columns.Add("reversa", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableFacturas.Columns.Add("nueva", BoFieldsType.ft_AlphaNumeric, 100)
        dataTableFacturas.Columns.Add("NumFactura", BoFieldsType.ft_AlphaNumeric, 100)

        ' Dim col As SAPbouiCOM.Column




        MatrixFacturas = New MatrixSBOFacturas("mtx_Facts", FormularioSBO, "Facturas")
        MatrixFacturas.CreaColumnas()
        MatrixFacturas.LigaColumnas()

        'col = MatrixFacturas.Matrix.Columns.Item("col_Refac")
        'col.ValOff = "0"
        'col.ValOn = "1"

        ButtonBuscar = New SCG.SBOFramework.UI.ButtonSBO("btnBuscar", FormularioSBO)
        ButtonRefacturar = New SCG.SBOFramework.UI.ButtonSBO("btnRefact", FormularioSBO)
        ButtonCargar = New SCG.SBOFramework.UI.ButtonSBO("btnCargar", FormularioSBO)


        ButtonBuscar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
        ButtonCargar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

    End Sub

    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If Not pVal.FormTypeEx = FormType Then Return

        If pVal.EventType = BoEventTypes.et_CHOOSE_FROM_LIST Then

            If pVal.ItemUID = ButtonBuscar.UniqueId Then

                Call ButtonSBOBuscarCFL(FormUID, pVal, BubbleEvent)

            End If

        ElseIf pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

            If pVal.ItemUID = ButtonRefacturar.UniqueId Then


                Call ButtonSBORefacturar(FormUID, pVal, BubbleEvent)

            End If

            If pVal.ItemUID = ButtonCargar.UniqueId Then

                Call BusquedaFacturas(FormUID, pVal, BubbleEvent)
                ' Call CargarFacturasRefacturarByFecha(BubbleEvent)

            End If



            'maneja evento de matriz
            If pVal.ItemUID = MatrixFacturas.UniqueId Then

                If pVal.ActionSuccess = True Then
                    If pVal.ColUID = "col_Refac" And pVal.Row > 0 Then

                        Call SeleccionFacturas(pVal)

                    End If
                End If

            End If


            If pVal.ItemUID = CheckBoxRefacturarTodos.UniqueId Then
                If pVal.ActionSuccess = True Then
                    Call SeleccionTodasFacturas()
                End If
            End If

        ElseIf pVal.EventType = BoEventTypes.et_CLICK Then
            If pVal.ItemUID = CheckBoxUsaFiltroCV.UniqueId Then

                Call HabilitaCampos()

            End If

            

        End If

    End Sub
    
End Class
