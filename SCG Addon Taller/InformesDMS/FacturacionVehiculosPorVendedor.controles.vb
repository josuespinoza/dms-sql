'Herencia de las librerias necesarias para el formulario
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class FacturacionVehiculosPorVendedor : Implements IUsaMenu, IFormularioSBO

#Region "Declaraciones"

    ''' <summary>
    ''' 
    ''' Declaracion Variables
    ''' </summary>
    ''' <remarks></remarks>

    Private _Direccion_Reportes As String
    Private _ConexionSBO As String
    Private _Usuario_BD As String
    Private _ContraseñaBD As String
    Public BtnPrintSbo As ButtonSBO
    
    Private _IdMenu As String
    Private _MenuPadre As String
    Private _Nombre As String
    Private _Posicion As String
    Private _FormType As String
    Private _FormularioSBO As IForm
    Private _Inicializado As Boolean
    Private _NombreXML As String
    Private _Titulo As String

    Dim oDataTable As DataTable
    Private _applicationSbo As Application
    Private _company_Sbo As ICompany

    Private _txtFechaDesde As EditTextSBO
    Private _txtFechaHasta As EditTextSBO
    Private _txtVendedores As EditTextSBO
    Private _rb_Marca As OptionBtnSBO
    Private _rb_Vendedor As OptionBtnSBO

    Private _cbo_Sucursal As ComboBoxSBO
    Private _cbo_TipoVehe As ComboBoxSBO
    Private _cbo_Marca As ComboBoxSBO
    Private _cbxTipVe As CheckBoxSBO
    Private _cbxMarc As CheckBoxSBO
    Private _cbxVend As CheckBoxSBO

    Private _udsFormulario As UserDataSources

    'Private intCodVendedor As Integer
    Private strCodVendedor As String = String.Empty

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Declaracion de Get's y set's
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DireccionReportes As String
        Get
            Return _Direccion_Reportes
        End Get
        Set(ByVal value As String)
            _Direccion_Reportes = value
        End Set
    End Property

    Public Property Conexion As String
        Get
            Return _ConexionSBO
        End Get
        Set(ByVal value As String)
            _ConexionSBO = value
        End Set
    End Property

    Public Property UsuarioBd As String
        Get
            Return _Usuario_BD
        End Get
        Set(ByVal value As String)
            _Usuario_BD = value
        End Set
    End Property

    Public Property ContraseñaBaseDatos As String
        Get
            Return _ContraseñaBD
        End Get
        Set(ByVal value As String)
            _ContraseñaBD = value
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

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
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

    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements SCG.SBOFramework.UI.IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
        End Get
    End Property

    Public ReadOnly Property CompanySBO As SAPbobsCOM.ICompany Implements SCG.SBOFramework.UI.IFormularioSBO.CompanySBO
        Get
            Return _company_Sbo
        End Get
    End Property

    Public Property FormType As String Implements SCG.SBOFramework.UI.IFormularioSBO.FormType
        Get
            Return _FormType
        End Get
        Set(ByVal value As String)
            _FormType = value
        End Set
    End Property

    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _FormularioSBO
        End Get
        Set(ByVal value As SAPbouiCOM.IForm)
            _FormularioSBO = value
        End Set
    End Property

    Public Property Inicializado As Boolean Implements SCG.SBOFramework.UI.IFormularioSBO.Inicializado
        Get
            Return _Inicializado
        End Get
        Set(ByVal value As Boolean)
            _Inicializado = value
        End Set
    End Property

    Public Property NombreXml As String Implements SCG.SBOFramework.UI.IFormularioSBO.NombreXml
        Get
            Return _NombreXML
        End Get
        Set(ByVal value As String)
            _NombreXML = value
        End Set
    End Property

    Public Property Titulo As String Implements SCG.SBOFramework.UI.IFormularioSBO.Titulo
        Get
            Return _Titulo
        End Get
        Set(ByVal value As String)
            _Titulo = value
        End Set
    End Property

#End Region

#Region "Metodos"

    <System.CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application, p_menuInformesDMS As String, p_strUID_FORM_FacturacioVehi As String)
        m_oCompany = ocompany
        m_SBO_Application = SBOAplication
        m_strDireccionConfiguracion = CatchingEvents.DireccionConfiguracion
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioReporteFacturacionVeh
        MenuPadre = p_menuInformesDMS
        Nombre = My.Resources.Resource.MenuFacturacionVehiculos
        IdMenu = p_strUID_FORM_FacturacioVehi
        Titulo = My.Resources.Resource.MenuFacturacionVehiculos
        Posicion = 6
        FormType = p_strUID_FORM_FacturacioVehi
    End Sub

    ''' <summary>
    ''' Incia Formulario
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario
        CargarFormulario()
        CargaCombos()
    End Sub

    ''' <summary>
    ''' Inicia Controladores
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        Try

            _udsFormulario = FormularioSBO.DataSources.UserDataSources

            _udsFormulario.Add("txtVend", BoDataType.dt_LONG_TEXT, 100)
            _udsFormulario.Add("txtFD", BoDataType.dt_DATE, 100)
            _udsFormulario.Add("txtFH", BoDataType.dt_DATE, 100)
            _udsFormulario.Add("cbo_Sucur", BoDataType.dt_LONG_TEXT, 100)
            _udsFormulario.Add("rb_Mar", BoDataType.dt_LONG_TEXT, 100)
            _udsFormulario.Add("rb_Ven", BoDataType.dt_LONG_TEXT, 100)
            _udsFormulario.Add("cbo_TipoV", BoDataType.dt_LONG_TEXT, 100)
            _udsFormulario.Add("cbo_Mar", BoDataType.dt_LONG_TEXT, 100)
            _udsFormulario.Add("CFL_Ven", BoDataType.dt_LONG_TEXT, 100)

            _udsFormulario.Add("cbxSucu", BoDataType.dt_LONG_TEXT, 10)
            _udsFormulario.Add("cbxTipVe", BoDataType.dt_LONG_TEXT, 10)
            _udsFormulario.Add("cbxMarc", BoDataType.dt_LONG_TEXT, 10)
            _udsFormulario.Add("cbxVend", BoDataType.dt_LONG_TEXT, 10)

            _txtFechaDesde = New EditTextSBO("txtFechDes", True, "", "txtFD", FormularioSBO)
            _txtFechaHasta = New EditTextSBO("txtFechHas", True, "", "txtFH", FormularioSBO)
            _txtVendedores = New EditTextSBO("txtVend", True, "", "txtVend", FormularioSBO)
            
            _cbo_Sucursal = New ComboBoxSBO("cbo_Sucur", FormularioSBO, True, "", "cbo_Sucur")
            _cbo_TipoVehe = New ComboBoxSBO("cbo_TipSu", FormularioSBO, True, "", "cbo_TipoV")
            _cbo_Marca = New ComboBoxSBO("cbo_Marc", FormularioSBO, True, "", "cbo_Mar")
            _rb_Marca = New OptionBtnSBO("rbMarca", True, "", "rb_Mar", FormularioSBO)
            _rb_Vendedor = New OptionBtnSBO("rbVend", True, "", "rb_Ven", FormularioSBO)

            _cbxTipVe = New CheckBoxSBO("cbxTipVe", True, "", "cbxTipVe", FormularioSBO)
            _cbxMarc = New CheckBoxSBO("cbxMarc", True, "", "cbxMarc", FormularioSBO)
            _cbxVend = New CheckBoxSBO("cbxVend", True, "", "cbxVend", FormularioSBO)

            BtnPrintSbo = New ButtonSBO("BtnImp", FormularioSBO)


            _txtFechaDesde.AsignaBinding()
            _txtFechaHasta.AsignaBinding()
            _txtVendedores.AsignaBinding()
            _cbo_Sucursal.AsignaBinding()
            _cbo_TipoVehe.AsignaBinding()
            _cbo_Marca.AsignaBinding()
            _rb_Marca.AsignaBinding()
            _rb_Vendedor.AsignaBinding()

            _cbxTipVe.AsignaBinding()
            _cbxMarc.AsignaBinding()
            _cbxVend.AsignaBinding()

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' ItemEvent
    ''' </summary>
    ''' <param name="FormUID"></param>
    ''' <param name="pVal"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Sub ApplicationSboOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If Not pVal.FormTypeEx = _FormType Then Return

            If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

                ManejadorEventoItemPress(FormUID, pVal, BubbleEvent)

            ElseIf pVal.EventType = BoEventTypes.et_CHOOSE_FROM_LIST Then

                ManejadorEventoChooseFromList(FormUID, pVal, BubbleEvent)

                'ElseIf pVal.EventType = BoEventTypes.et_COMBO_SELECT Then

                '    ManejoEventosCombo(FormUID, pVal, BubbleEvent)

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Sub

    ''' <summary>
    ''' Manejo del evento ChooseFromList
    ''' </summary>
    ''' <param name="formUId"></param>
    ''' <param name="pval"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Public Sub ManejadorEventoChooseFromList(ByVal formUId As String, ByVal pval As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String
        Dim cboComboSucursal As ComboBox
        Dim oItem As Item
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim strTextVen As String = "txtVend"
        Dim strVendedor As String = "salesPrson"
        Dim strSucursal As String = "Branch"
        Dim strFistName As String = "firstName"
        Dim strLastNaem As String = "lastName"
        Dim m_vendedor As String = String.Empty
        Dim l_strSucursal As Integer

        Try
            oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

            If oCFLEvento.BeforeAction = True Then

                oItem = FormularioSBO.Items.Item(_cbo_Sucursal.UniqueId)
                cboComboSucursal = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
                l_strSucursal = cboComboSucursal.Selected.Value

                Select Case pval.ItemUID

                    Case strTextVen

                        oConditions = m_SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = strVendedor
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                        oCondition.CondVal = Nothing
                        oCondition.BracketCloseNum = 1
                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 2
                        oCondition.Alias = strVendedor
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                        oCondition.CondVal = " "
                        oCondition.BracketCloseNum = 2
                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 3
                        oCondition.Alias = strSucursal
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = l_strSucursal
                        oCondition.BracketCloseNum = 3

                        oCFL.SetConditions(oConditions)

                End Select
            ElseIf oCFLEvento.ActionSuccess Then

                oDataTable = oCFLEvento.SelectedObjects
                If Not oDataTable Is Nothing Then
                    m_vendedor = String.Format("{0} {1}", oDataTable.GetValue(strFistName, 0), oDataTable.GetValue(strLastNaem, 0))
                    _txtVendedores.AsignaValorUserDataSource(m_vendedor)
                    strCodVendedor = oDataTable.GetValue("empID", 0)
                End If
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

#End Region
    
End Class
