Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany
Imports System
Imports System.IO
Imports System.Threading

Partial Public Class UnidadesVendidas : Implements IFormularioSBO, IUsaMenu
#Region "Declaraciones"

    'maneja informacion de la aplicacion
    Private _applicationSbo As Application
    'maneja informacion de la compania 
    Private _companySbo As ICompany
    Private _formType As String
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _nombreXml As String
    Private _titulo As String
    Private _strConexion As String

    'Campos EditText - Botones de la pantalla

    Public EditTextFDesde As EditTextSBO
    Public EditTextFHasta As EditTextSBO

    Public CboTipoSucursal As ComboBoxSBO
    Public EditTextCodCliente As EditTextSBO
    Public EditTextDesCliente As EditTextSBO
    Public EditTextCodVendedor As EditTextSBO
    Public EditTextDesVendedor As EditTextSBO

    Public OpbRptCompleto As OptionBtn
    Public OpbRptResumido As OptionBtn
    
    Public BtnCargar As ButtonSBO
    Public BtnBuscaCliente As ButtonSBO
    Public BtnBuscaVendedor As ButtonSBO
    Public BtnImprimirRpt As ButtonSBO
    Public BtnCancelar As ButtonSBO

    Public cbTodasSucursales As CheckBoxSBO
    Public cbResumenRpt As CheckBoxSBO


    Private _IdMenu As String
    Private _MenuPadre As String
    Private _Posicion As Integer
    Private _Nombre As String

    Private _Conexion As String
    Private _DireccionReportes As String
    Private _UsuarioBD As String
    Private _ContraseñaBD As String

    Private ConnectionStringTaller As String = String.Empty
    
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
        'Set(ByVal value As SAPbouiCOM.IForm)
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

    Public Property IdMenu As String Implements SCG.SBOFramework.UI.IUsaMenu.IdMenu
        Get
            Return _IdMenu
        End Get
        Set(ByVal value As String)
            _IdMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements SCG.SBOFramework.UI.IUsaMenu.MenuPadre
        Get
            Return _MenuPadre
        End Get
        Set(ByVal value As String)
            _MenuPadre = value
        End Set
    End Property

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _Posicion
        End Get
        Set(ByVal value As Integer)
            _Posicion = value
        End Set
    End Property

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
        End Set
    End Property

    Public Property Conexion As String
        Get
            Return _Conexion
        End Get
        Set(ByVal value As String)
            _Conexion = value
        End Set
    End Property

    Public Property DireccionReportes As String
        Get
            Return _DireccionReportes
        End Get
        Set(ByVal value As String)
            _DireccionReportes = value
        End Set
    End Property

    Public Property UsuarioBd As String
        Get
            Return _UsuarioBD
        End Get
        Set(ByVal value As String)
            _UsuarioBD = value
        End Set
    End Property

    Public Property ContraseñaBd As String
        Get
            Return _ContraseñaBD
        End Get
        Set(ByVal value As String)
            _ContraseñaBD = value
        End Set
    End Property



#End Region

#Region "Métodos"
    Public Sub CFLCliente(ByVal FormUID As String, ByVal pval As ItemEvent)
        Try

            Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
            oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)
            Dim sCFL_ID As String
            sCFL_ID = oCFLEvento.ChooseFromListUID

            Dim oCFl As SAPbouiCOM.ChooseFromList
            oCFl = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

            Dim oCondition As SAPbouiCOM.Condition
            Dim oConditions As SAPbouiCOM.Conditions

            Dim oDataTable As SAPbouiCOM.DataTable

            If pval.ActionSuccess = True AndAlso pval.BeforeAction = False Then
                If Not IsNothing(oCFLEvento.SelectedObjects) Then
                    ' EditTextCodCliente.AsignaValorDataSource("")
                    EditTextCodCliente.AsignaValorUserDataSource("")
                    EditTextDesCliente.AsignaValorUserDataSource("")

                    oDataTable = oCFLEvento.SelectedObjects

                    EditTextCodCliente.AsignaValorUserDataSource(oDataTable.GetValue("CardCode", 0))
                    EditTextDesCliente.AsignaValorUserDataSource(oDataTable.GetValue("CardName", 0))
                End If
            ElseIf pval.BeforeAction = True Then
                'oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                'oCondition = oConditions.Add()
                'oCondition.Alias = "CardType"
                'oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                'oCondition.CondVal = "C"
                'oCondition.BracketOpenNum = 1
                'oCFl.SetConditions(oConditions)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Public Sub CFLVendedor(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent)
        Try
            Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            Dim sCFL_ID As String
            sCFL_ID = oCFLEvento.ChooseFromListUID
            Dim oCFL As SAPbouiCOM.ChooseFromList
            oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)
            Dim oCondition As SAPbouiCOM.Condition
            Dim oConditions As SAPbouiCOM.Conditions
            Dim oDataTable As SAPbouiCOM.DataTable
            If pVal.ActionSuccess = True AndAlso pVal.BeforeAction = False Then
                If Not IsNothing(oCFLEvento.SelectedObjects) Then
                    EditTextCodVendedor.AsignaValorUserDataSource("")
                    EditTextDesVendedor.AsignaValorUserDataSource("")
                    oDataTable = oCFLEvento.SelectedObjects
                    EditTextCodVendedor.AsignaValorUserDataSource(oDataTable.GetValue("SlpCode", 0))
                    EditTextDesVendedor.AsignaValorUserDataSource(oDataTable.GetValue("SlpName", 0))
                End If
            ElseIf pVal.BeforeAction = True Then
                oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "Locked"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "N"
                oCondition.BracketCloseNum = 1
                oCFL.SetConditions(oConditions)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub ButtonSBOImprimirRptUnidadesVendidasItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim ls_DireccionReporte As String = ""
        Dim ls_Cliente As String = ""
        Dim ls_CodCliente As String = ""
        Dim ls_Vendedor As String = ""
        Dim ls_CodVendedor As String = ""
        Dim ls_Sucursal As String = ""
        Dim ls_FechaDesde As String = ""
        Dim ls_FechaHasta As String = ""

        Dim ld_Desde As Date
        Dim ld_Hasta As Date

        Dim ls_Resumen As String = ""
        Dim ls_TodasSuc As String = ""


        Dim ls_Parametros As String = ""

        ls_FechaDesde = EditTextFDesde.ObtieneValorUserDataSource()
        ls_FechaHasta = EditTextFHasta.ObtieneValorUserDataSource()

        ls_Cliente = EditTextCodCliente.ObtieneValorUserDataSource()
        ls_CodCliente = EditTextCodCliente.ObtieneValorUserDataSource()
        ls_CodVendedor = EditTextCodVendedor.ObtieneValorUserDataSource()
        ls_Sucursal = CboTipoSucursal.ObtieneValorUserDataSource()
        ls_TodasSuc = cbTodasSucursales.ObtieneValorUserDataSource()
        ls_Resumen = cbResumenRpt.ObtieneValorUserDataSource()


        Dim strNombreBDTaller As String = ""

        Utilitarios.DevuelveNombreBDTaller(_applicationSbo, strNombreBDTaller)

        If pVal.BeforeAction = True Then
            If String.IsNullOrEmpty(ls_FechaDesde) OrElse String.IsNullOrEmpty(ls_FechaHasta) Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCargaReporte, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                Exit Sub
            End If

        ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess Then

            ld_Desde = Date.ParseExact(ls_FechaDesde, "yyyyMMdd", Nothing)
            ld_Hasta = Date.ParseExact(ls_FechaHasta, "yyyyMMdd", Nothing)

            ld_Desde = New Date(ld_Desde.Year, ld_Desde.Month, ld_Desde.Day, 0, 0, 0)
            ld_Hasta = New Date(ld_Hasta.Year, ld_Hasta.Month, ld_Hasta.Day, 0, 0, 0)

            If ls_Resumen = "Y" Then
                ls_DireccionReporte = DireccionReportes & My.Resources.Resource.rptUnidadesVendidasResumen
            Else
                ls_DireccionReporte = DireccionReportes & My.Resources.Resource.rptUnidadesVendidas
            End If

            If ls_TodasSuc = "Y" OrElse String.IsNullOrEmpty(ls_Sucursal) Then
                ls_Sucursal = My.Resources.Resource.TodosLosCampos
            End If

            If String.IsNullOrEmpty(ls_CodCliente) Then
                ls_CodCliente = My.Resources.Resource.TodosLosCampos
            End If
            If String.IsNullOrEmpty(ls_CodVendedor) Then
                ls_CodVendedor = My.Resources.Resource.TodosLosCamposNum
            End If

            ls_Parametros = ld_Desde & "," & ld_Hasta & "," & ls_Sucursal & "," & ls_CodVendedor & "," & ls_CodCliente

            Call Utilitarios.ImprimirReporte(ls_DireccionReporte, My.Resources.Resource.TituloReporteUnidadesVendidas, ls_Parametros, UsuarioBd, ContraseñaBd, strNombreBDTaller, m_oCompany.Server)
            ' Call .ImprimirReporte(_companySbo, ls_DireccionReporte, My.Resources.Resource.TituloReporteUnidadesVendidas, ls_Parametros, StrUsuarioBD, StrContraseñaBD)
        End If

    End Sub

    'Public Sub CFLCliente(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent)

    '    Try

    '        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
    '        oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
    '        Dim sCFL_ID As String
    '        sCFL_ID = oCFLEvento.ChooseFromListUID
    '        Dim oCFL As SAPbouiCOM.ChooseFromList
    '        oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)
    '        Dim oCondition As SAPbouiCOM.Condition
    '        Dim oConditions As SAPbouiCOM.Conditions
    '        Dim oDataTable As SAPbouiCOM.DataTable
    '        If pVal.ActionSuccess = True AndAlso pVal.BeforeAction = False Then
    '            If Not oCFLEvento.SelectedObjects Is Nothing Then
    '                EditTextCodCliente.AsignaValorUserDataSource("")
    '                EditTextNombreCliente.AsignaValorUserDataSource("")
    '                oDataTable = oCFLEvento.SelectedObjects
    '                EditTextCodCliente.AsignaValorUserDataSource(oDataTable.GetValue("CardCode", 0))
    '                EditTextNombreCliente.AsignaValorUserDataSource(oDataTable.GetValue("CardName", 0))
    '                End If
    '            ElseIf pVal.BeforeAction = True Then
    '            oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
    '            oCondition = oConditions.Add
    '            oCondition.BracketOpenNum = 1
    '            oCondition.Alias = "CardType"
    '            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
    '            oCondition.CondVal = "C"
    '            oCondition.BracketCloseNum = 1
    '            oCFL.SetConditions(oConditions)

    '        End If

    '    Catch ex As Exception

    '        Throw ex

    '    End Try

    'End Sub


    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        _companySbo = companySbo
        _applicationSbo = application
        m_oCompany = companySbo
        DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)

    End Sub

    Public Sub ApplicationSBOOnItemEvent(ByVal formUid As String, ByRef pVal As ItemEvent, ByRef bubbleEvent As Boolean)

        If Not pVal.FormTypeEx = FormType Then Return

        If pVal.EventType = BoEventTypes.et_CHOOSE_FROM_LIST Then
            If pVal.ItemUID = BtnBuscaCliente.UniqueId Then
                CFLCliente(formUid, pVal)
            End If
            If pVal.ItemUID = BtnBuscaVendedor.UniqueId Then
                CFLVendedor(formUid, pVal)
            End If
            If FormularioSBO.Mode <> BoFormMode.fm_FIND_MODE Then

            End If

        End If
        If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then
            If pVal.ItemUID = BtnImprimirRpt.UniqueId Then
                ButtonSBOImprimirRptUnidadesVendidasItemPresed(formUid, pVal, bubbleEvent)

            End If
        End If




    End Sub

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario
        
    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles

        

        Try
            Dim fcp As SAPbouiCOM.FormCreationParams
            Dim sboItem As SAPbouiCOM.Item
            Dim sboCombo As SAPbouiCOM.ComboBox
            
            '  oForm = m_SBO_Application.Forms.AddEx(fcp)

            If FormularioSBO IsNot Nothing Then

                FormularioSBO.Freeze(True)

                Dim userDataSources As UserDataSources = FormularioSBO.DataSources.UserDataSources
                userDataSources.Add("FechaIni", BoDataType.dt_DATE, 100)
                userDataSources.Add("FechaFin", BoDataType.dt_DATE, 100)
                userDataSources.Add("CodSucu", BoDataType.dt_LONG_TEXT, 100)
                userDataSources.Add("CodVend", BoDataType.dt_LONG_TEXT, 100)
                userDataSources.Add("CodCli", BoDataType.dt_LONG_TEXT, 100)
                userDataSources.Add("DesCli", BoDataType.dt_LONG_TEXT, 100)
                userDataSources.Add("DesVend", BoDataType.dt_LONG_TEXT, 100)
                userDataSources.Add("cbxResum", BoDataType.dt_LONG_TEXT, 1)
                userDataSources.Add("obCompleto", BoDataType.dt_LONG_TEXT, 1)
                userDataSources.Add("TodasSuc", BoDataType.dt_LONG_TEXT, 1)


                EditTextFDesde = New EditTextSBO("txtDesde", True, "", "FechaIni", FormularioSBO)
                EditTextFDesde.AsignaBinding()
                EditTextFHasta = New EditTextSBO("txtHasta", True, "", "FechaFin", FormularioSBO)
                EditTextFHasta.AsignaBinding()
                CboTipoSucursal = New ComboBoxSBO("cboSucur", FormularioSBO, True, "", "CodSucu")
                CboTipoSucursal.AsignaBinding()
                EditTextCodCliente = New EditTextSBO("txtCodClie", True, "", "CodCli", FormularioSBO)
                EditTextCodCliente.AsignaBinding()
                EditTextCodVendedor = New EditTextSBO("txtCodVen", True, "", "CodVend", FormularioSBO)
                EditTextCodVendedor.AsignaBinding()
                EditTextDesCliente = New EditTextSBO("txtDesCli", True, "", "DesCli", FormularioSBO)
                EditTextDesCliente.AsignaBinding()
                EditTextDesVendedor = New EditTextSBO("txtDesVen", True, "", "DesVend", FormularioSBO)
                EditTextDesVendedor.AsignaBinding()
                cbTodasSucursales = New CheckBoxSBO("cbAllSuc", True, "", "TodasSuc", FormularioSBO)
                cbTodasSucursales.AsignaBinding()
                cbResumenRpt = New CheckBoxSBO("cbResum", True, "", "cbxResum", FormularioSBO)
                cbResumenRpt.AsignaBinding()

                BtnCargar = New ButtonSBO("btnImpr", FormularioSBO)
                BtnBuscaCliente = New ButtonSBO("btnCli", FormularioSBO)
                BtnBuscaVendedor = New ButtonSBO("btnVen", FormularioSBO)
                BtnImprimirRpt = New ButtonSBO("btnImpr", FormularioSBO)
                BtnCancelar = New ButtonSBO("btnCanc")


                sboItem = FormularioSBO.Items.Item("cboSucur")
                sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
                Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, "SELECT Code,Name   FROM [@SCGD_SUCURSALES]")

                If sboCombo.Selected Is Nothing And sboCombo.ValidValues.Count <> 0 Then sboCombo.Select(0, BoSearchKey.psk_Index)
                FormularioSBO.Items.Item("cboSucur").Update()

                FormularioSBO.Freeze(False)

            End If




        Catch ex As Exception
            Throw (ex)
        End Try
       

    End Sub

#End Region
End Class
