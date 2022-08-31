Imports System.Globalization
Imports System.Threading
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany
'Imports System.Diagnostics

Partial Public Class BusquedasCitas : Implements IFormularioSBO, IUsaMenu

#Region "Declaraciones"

    Private _cargaFormulario As CargaFormularioAsociaxEspDelegate

    Private _formType As String

    Private _nombreXml As String

    Private _titulo As String

    Private _menuPadre As String

    Private _nombreMenu As String

    Private _idMenu As String

    Private _posicion As Integer

    Private _formularioSbo As IForm

    Private _inicializado As Boolean

    Private _applicationSbo As Application

    Private _companySbo As ICompany

    Private _strConexion As String

    Private _strDireccionReportes As String

    Private _strUsuarioBD As String

    Private _strContraseñaBD As String

    Private EditTextNoCita As EditTextSBO
    Private EditTextNoCitaAb As EditTextSBO
    Private EditTextNoOt As EditTextSBO
    Private EditTextNoUnidad As EditTextSBO
    Private EditTextPlaca As EditTextSBO
    Private EditTextNoVisita As EditTextSBO
    Private EditTextNoCono As EditTextSBO
    Private EditTextDesde As EditTextSBO
    Private EditTextHasta As EditTextSBO
    Private EditTextDiasPrev As EditTextSBO
    Private EditTextCodAsesor As EditTextSBO
    Private EditTextNamAsesor As EditTextSBO

    Private CheckBoxConfirmacion As CheckBoxSBO
    Private CheckBoxMarca As CheckBoxSBO
    Private CheckBoxEstilo As CheckBoxSBO
    Private CheckBoxModelo As CheckBoxSBO
    Private CheckBoxMecanico As CheckBoxSBO
    Private CheckBoxSucursal As CheckBoxSBO
    Private CheckBoxDesde As CheckBoxSBO
    Private CheckBoxHasta As CheckBoxSBO
    Private CheckBoxDiasPrev As CheckBoxSBO
    Private CheckBoxUAge As CheckBoxSBO

    Private ComboBoxConfirmacion As ComboBoxSBO
    Private ComboBoxMarca As ComboBoxSBO
    Private ComboBoxEstilo As ComboBoxSBO
    Private ComboBoxModelo As ComboBoxSBO
    Private ComboBoxMecanico As ComboBoxSBO
    Private ComboBoxSucursal As ComboBoxSBO
    Private ComboBoxAgenda As ComboBoxSBO

    Private MatrixBusqueda As MatrizBusquedaCitas

    Private ButtonBuscar As ButtonSBO
    Private ButtonIrACita As ButtonSBO

    Private UserDataSourceBusqueda As SAPbouiCOM.UserDataSources

    Private DataTableBusqueda As SAPbouiCOM.DataTable
    Dim oDataTable As SAPbouiCOM.DataTable

#End Region

#Region "Constructor"

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        _companySbo = companySbo
        _applicationSbo = application
        DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)
    End Sub

#End Region

#Region "Propiedades"

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
            Return _formularioSbo
        End Get
        Set(ByVal value As IForm)
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

    Public Property IdMenu As String Implements SCG.SBOFramework.UI.IUsaMenu.IdMenu
        Get
            Return _idMenu
        End Get
        Set(ByVal value As String)
            _idMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements SCG.SBOFramework.UI.IUsaMenu.MenuPadre
        Get
            Return _menuPadre
        End Get
        Set(ByVal value As String)
            _menuPadre = value
        End Set
    End Property

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _nombreMenu
        End Get
        Set(ByVal value As String)
            _nombreMenu = value
        End Set
    End Property

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _posicion
        End Get
        Set(ByVal value As Integer)
            _posicion = value
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

    Public Property StrDireccionReportes As String
        Get
            Return _strDireccionReportes
        End Get
        Set(ByVal value As String)
            _strDireccionReportes = value
        End Set
    End Property

    Public Property StrUsuarioBD As String
        Get
            Return _strUsuarioBD
        End Get
        Set(ByVal value As String)
            _strUsuarioBD = value
        End Set
    End Property

    Public Property StrContraseñaBD As String
        Get
            Return _strContraseñaBD
        End Get
        Set(ByVal value As String)
            _strContraseñaBD = value
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

        If Not FormularioSBO Is Nothing Then

            FormularioSBO.Freeze(True)

            For Each Item As SAPbouiCOM.Item In FormularioSBO.Items

                Item.AffectsFormMode = False

            Next

            CargarFormulario()
            FormularioSBO.Freeze(False)

        End If

    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles

        If Not FormularioSBO Is Nothing Then

            FormularioSBO.Freeze(True)

            UserDataSourceBusqueda = FormularioSBO.DataSources.UserDataSources
            UserDataSourceBusqueda.Add("noCitaAb", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("noCita", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("noUnidad", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("placa", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("noOt", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("noVisita", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("noCono", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("chkConf", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("chkMarca", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("chkEstilo", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("chkModelo", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("chkMeca", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("chkSucur", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("confir", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("marca", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("estilo", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("modelo", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("mecanic", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("sucursal", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("agenda", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("chkDesd", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("chkHast", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("chkDPrev", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("chkUAge", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("desde", BoDataType.dt_DATE, 200)
            UserDataSourceBusqueda.Add("hasta", BoDataType.dt_DATE, 200)
            UserDataSourceBusqueda.Add("dPrev", BoDataType.dt_LONG_NUMBER, 100)
            UserDataSourceBusqueda.Add("codAse", BoDataType.dt_LONG_TEXT, 200)
            UserDataSourceBusqueda.Add("namAse", BoDataType.dt_LONG_TEXT, 200)

            EditTextNoCitaAb = New EditTextSBO("txtNoCitAb", True, "", "noCitaAb", FormularioSBO)
            EditTextNoCita = New EditTextSBO("txtNoCita", True, "", "noCita", FormularioSBO)
            EditTextNoUnidad = New EditTextSBO("txtNoUni", True, "", "noUnidad", FormularioSBO)
            EditTextPlaca = New EditTextSBO("txtPlac", True, "", "placa", FormularioSBO)
            EditTextNoOt = New EditTextSBO("txtNoOT", True, "", "noOt", FormularioSBO)
            EditTextNoVisita = New EditTextSBO("txtNoVisi", True, "", "noVisita", FormularioSBO)
            EditTextNoCono = New EditTextSBO("txtNoCon", True, "", "noCono", FormularioSBO)
            EditTextDesde = New EditTextSBO("txtDesde", True, "", "desde", FormularioSBO)
            EditTextHasta = New EditTextSBO("txtHasta", True, "", "hasta", FormularioSBO)
            EditTextDiasPrev = New EditTextSBO("txtDiasP", True, "", "dPrev", FormularioSBO)
            EditTextCodAsesor = New EditTextSBO("txtCodAse", True, "", "codAse", FormularioSBO)
            EditTextNamAsesor = New EditTextSBO("txtNamAse", True, "", "namAse", FormularioSBO)

            EditTextNoCitaAb.AsignaBinding()
            EditTextNoCita.AsignaBinding()
            EditTextNoUnidad.AsignaBinding()
            EditTextPlaca.AsignaBinding()
            EditTextNoOt.AsignaBinding()
            EditTextNoVisita.AsignaBinding()
            EditTextNoCono.AsignaBinding()
            EditTextDesde.AsignaBinding()
            EditTextHasta.AsignaBinding()
            EditTextDiasPrev.AsignaBinding()
            EditTextCodAsesor.AsignaBinding()
            EditTextNamAsesor.AsignaBinding()

            CheckBoxConfirmacion = New CheckBoxSBO("chkConf", True, "", "chkConf", FormularioSBO)
            CheckBoxMarca = New CheckBoxSBO("chkMar", True, "", "chkMarca", FormularioSBO)
            CheckBoxEstilo = New CheckBoxSBO("chkEsti", True, "", "chkEstilo", FormularioSBO)
            CheckBoxModelo = New CheckBoxSBO("chkMod", True, "", "chkModelo", FormularioSBO)
            CheckBoxMecanico = New CheckBoxSBO("chkMec", True, "", "chkMeca", FormularioSBO)
            CheckBoxSucursal = New CheckBoxSBO("chkSucur", True, "", "chkSucur", FormularioSBO)
            CheckBoxDesde = New CheckBoxSBO("chkDesd", True, "", "chkDesd", FormularioSBO)
            CheckBoxHasta = New CheckBoxSBO("chkHast", True, "", "chkHast", FormularioSBO)
            CheckBoxDiasPrev = New CheckBoxSBO("chkDPre", True, "", "chkDPrev", FormularioSBO)
            CheckBoxUAge = New CheckBoxSBO("chkAgen", True, "", "chkUAge", FormularioSBO)


            CheckBoxConfirmacion.AsignaBinding()
            CheckBoxMarca.AsignaBinding()
            CheckBoxEstilo.AsignaBinding()
            CheckBoxModelo.AsignaBinding()
            CheckBoxMecanico.AsignaBinding()
            CheckBoxSucursal.AsignaBinding()
            CheckBoxDesde.AsignaBinding()
            CheckBoxHasta.AsignaBinding()
            CheckBoxDiasPrev.AsignaBinding()
            CheckBoxUAge.AsignaBinding()

            ComboBoxConfirmacion = New ComboBoxSBO("cboConf", FormularioSBO, True, "", "confir")
            ComboBoxMarca = New ComboBoxSBO("cboMar", FormularioSBO, True, "", "marca")
            ComboBoxEstilo = New ComboBoxSBO("cboEsti", FormularioSBO, True, "", "estilo")
            ComboBoxModelo = New ComboBoxSBO("cboMod", FormularioSBO, True, "", "modelo")
            ComboBoxMecanico = New ComboBoxSBO("cboMec", FormularioSBO, True, "", "mecanic")
            ComboBoxSucursal = New ComboBoxSBO("cboSucur", FormularioSBO, True, "", "sucursal")
            ComboBoxAgenda = New ComboBoxSBO("cboAgen", FormularioSBO, True, "", "agenda")

            ComboBoxConfirmacion.AsignaBinding()
            ComboBoxMarca.AsignaBinding()
            ComboBoxEstilo.AsignaBinding()
            ComboBoxModelo.AsignaBinding()
            ComboBoxMecanico.AsignaBinding()
            ComboBoxSucursal.AsignaBinding()
            ComboBoxAgenda.AsignaBinding()

            ButtonBuscar = New ButtonSBO("btnBuscar", FormularioSBO)
            ButtonIrACita = New ButtonSBO("btnCitas", FormularioSBO)

            DataTableBusqueda = FormularioSBO.DataSources.DataTables.Add("dtBusq")
            DataTableBusqueda.Columns.Add("docCita", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("cita", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("fcita", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("hcita", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("docentry", BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("noot", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("tipot", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("sucur", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("nouni", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("placa", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("conf", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("cono", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("visita", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("codcli", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("nomcl", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("mar", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("esti", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("mode", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("mecanic", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
            DataTableBusqueda.Columns.Add("asesor", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)

            MatrixBusqueda = New MatrizBusquedaCitas("mtxBusq", FormularioSBO, "dtBusq")
            MatrixBusqueda.CreaColumnas()
            MatrixBusqueda.LigaColumnas()

            'AddChooseFromList(FormularioSBO, "171", "CFL_Emp")
            ' AsignarCFLText(EditTextCodAsesor.UniqueId, "CFL_Emp", "empID")

            FormularioSBO.Freeze(False)
        End If
    End Sub

    Private Sub CargarFormulario()
        Try
            Call CargarValidValuesEnCombos(FormularioSBO, "SELECT Code, Name FROM [@SCGD_CITA_ESTADOS] with (nolock) ORDER BY Code ASC", "cboConf")
            Call CargarValidValuesEnCombos(FormularioSBO, "SELECT Code, Name FROM [@SCGD_MARCA] with (nolock) ORDER BY Code ASC", "cboMar")
            'Call CargarValidValuesEnCombos(FormularioSBO, "SELECT Code, Name FROM [@SCGD_ESTILO] with (nolock) ORDER BY Code ASC", "cboEsti")
            'Call CargarValidValuesEnCombos(FormularioSBO, "SELECT Code, Name FROM [@SCGD_MODELO] with (nolock) ORDER BY Code ASC", "cboMod")
            Call CargarValidValuesEnCombos(FormularioSBO, "SELECT EmpId, firstName + ' ' + lastName FROM [OHEM] with (nolock) ORDER BY empId ASC", "cboMec")
            Call CargarValidValuesEnCombos(FormularioSBO, "SELECT Code, Name FROM [@SCGD_SUCURSALES] with (nolock) ORDER BY Code ASC", "cboSucur")
            Call CargarValidValuesEnCombos(FormularioSBO, "select DocEntry as Code, U_Agenda as Name from [@SCGD_AGENDA] with (nolock) ORDER BY DocEntry ASC", "cboAgen")
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If Not pVal.FormTypeEx = FormType Then Return

        If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

            ManejadorEventosItemPressed(FormUID, pVal, BubbleEvent)

        ElseIf pVal.EventType = BoEventTypes.et_CHOOSE_FROM_LIST Then

            ManejadorEventoChooseFromList(FormUID, pVal, BubbleEvent)
        End If

    End Sub

    Private Sub ManejadorEventosItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        FormularioSBO.Freeze(True)

        ' SetActiveItem(FormUID, pVal, BubbleEvent)
        MatrixBusqueda.Matrix.FlushToDataSource()

        Select Case pVal.ItemUID

            Case CheckBoxConfirmacion.UniqueId
                CheckBoxConfirmacionItemPressed(FormUID, pVal, BubbleEvent)

            Case CheckBoxMarca.UniqueId
                CheckBoxMarcaItemPressed(FormUID, pVal, BubbleEvent)

            Case CheckBoxEstilo.UniqueId
                CheckBoxEstiloItemPressed(FormUID, pVal, BubbleEvent)

            Case CheckBoxUAge.UniqueId
                CheckBoxAgendaItemPressed(FormUID, pVal, BubbleEvent)

            Case CheckBoxModelo.UniqueId
                CheckBoxModeloItemPressed(FormUID, pVal, BubbleEvent)

            Case CheckBoxMecanico.UniqueId
                CheckBoxMecanicoItemPressed(FormUID, pVal, BubbleEvent)

            Case CheckBoxSucursal.UniqueId
                CheckBoxSucursalItemPressed(FormUID, pVal, BubbleEvent)

            Case CheckBoxDesde.UniqueId
                CheckBoxDesdeItemPressed(FormUID, pVal, BubbleEvent)

            Case CheckBoxHasta.UniqueId
                CheckBoxHastaItemPressed(FormUID, pVal, BubbleEvent)

            Case CheckBoxDiasPrev.UniqueId
                CheckBoxDiasPrevItemPressed(FormUID, pVal, BubbleEvent)

            Case ButtonBuscar.UniqueId
                ButtonBuscarItemPressed(FormUID, pVal, BubbleEvent)

        End Select

        FormularioSBO.Freeze(False)

    End Sub

    Public Sub ManejadorEventoDobleClick(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If Not pVal.FormTypeEx = "SCGD_BCT" Then Return

            If pVal.FormTypeEx = "SCGD_BCT" Then

                If pVal.BeforeAction Then
                    If pVal.ColUID = "V_-1" Then
                        BubbleEvent = False
                    End If
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    Public Sub ManejadorEventoChooseFromList(ByVal formUId As String, ByVal pval As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String

        Try
            oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

            If pval.ActionSuccess Then
                oDataTable = oCFLEvento.SelectedObjects

                If Not oDataTable Is Nothing Then
                    If pval.ItemUID = EditTextCodAsesor.UniqueId Then
                        EditTextCodAsesor.AsignaValorUserDataSource(oDataTable.GetValue("empID", 0))
                        EditTextNamAsesor.AsignaValorUserDataSource(oDataTable.GetValue("firstName", 0) & " " & oDataTable.GetValue("lastName", 0))
                    End If
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

#End Region
End Class
