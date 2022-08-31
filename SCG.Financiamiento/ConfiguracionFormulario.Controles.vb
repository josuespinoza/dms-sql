Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany

'Clase para manejar controles del formulario de configuraciones del modulo de financiamiento

Partial Public Class ConfiguracionFormulario : Implements IFormularioSBO, IUsaMenu

    Private _formType As String

    Private _nombreXml As String

    Private _titulo As String

    Private _formularioSbo As IForm

    Private _inicializado As Boolean

    Private _applicationSbo As Application

    Private _companySbo As ICompany

    Private _menuPadre As String

    Private _nombreMenu As String

    Private _idMenu As String

    Private _posicion As Integer

    Private _strConexion As String

    Public EditTextFinLoc As EditTextSBO
    Public EditTextFinSis As EditTextSBO
    Public EditTextCuoLoc As EditTextSBO
    Public EditTextCuoSis As EditTextSBO
    Public EditTextIntLoc As EditTextSBO
    Public EditTextIntSis As EditTextSBO
    Public EditTextMorLoc As EditTextSBO
    Public EditTextMorSis As EditTextSBO
    Shared EditTextAsReLoc As EditTextSBO
    Shared EditTextAsReSis As EditTextSBO

    Public ComboBoxNumDoc As ComboBoxSBO
    Public ComboBoxNumNC As ComboBoxSBO
    Public EditTextCodImp As EditTextSBO

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        _companySbo = companySbo
        _applicationSbo = application
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

    Public Property MenuPadre() As String Implements IUsaMenu.MenuPadre
        Get
            Return _menuPadre
        End Get
        Set(ByVal value As String)
            _menuPadre = value
        End Set
    End Property

    Public Property NombreMenu() As String Implements IUsaMenu.Nombre
        Get
            Return _nombreMenu
        End Get
        Set(ByVal value As String)
            _nombreMenu = value
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

    Public Property Posicion() As Integer Implements IUsaMenu.Posicion
        Get
            Return _posicion
        End Get
        Set(ByVal value As Integer)
            _posicion = value
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

    Public Property StrConexion() As String
        Get
            Return _strConexion
        End Get
        Set(ByVal value As String)
            _strConexion = value
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

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario

        If FormularioSBO IsNot Nothing Then

            CargarFormulario()

        End If

    End Sub

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

    'Inicializa los controles de la pantalla de configuraciones de financiamiento

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

        EditTextFinLoc = New EditTextSBO("txtFinLoc", True, "@SCGD_CONF_FINANC", "U_Fin_Loc", FormularioSBO)
        EditTextFinSis = New EditTextSBO("txtFinSis", True, "@SCGD_CONF_FINANC", "U_Fin_Sis", FormularioSBO)
        EditTextCuoLoc = New EditTextSBO("txtCuoLoc", True, "@SCGD_CONF_FINANC", "U_Cuo_Loc", FormularioSBO)
        EditTextCuoSis = New EditTextSBO("txtCuoSis", True, "@SCGD_CONF_FINANC", "U_Cuo_Sis", FormularioSBO)
        EditTextIntLoc = New EditTextSBO("txtIntLoc", True, "@SCGD_CONF_FINANC", "U_Int_Loc", FormularioSBO)
        EditTextIntSis = New EditTextSBO("txtIntSis", True, "@SCGD_CONF_FINANC", "U_Int_Sis", FormularioSBO)
        EditTextMorLoc = New EditTextSBO("txtMorLoc", True, "@SCGD_CONF_FINANC", "U_Mor_Loc", FormularioSBO)
        EditTextMorSis = New EditTextSBO("txtMorSis", True, "@SCGD_CONF_FINANC", "U_Mor_Sis", FormularioSBO)
        ComboBoxNumDoc = New ComboBoxSBO("cboNumDoc", FormularioSBO, True, "@SCGD_CONF_FINANC", "U_NumDoc")
        ComboBoxNumNC = New ComboBoxSBO("cboNumNC", FormularioSBO, True, "@SCGD_CONF_FINANC", "U_NumNC")
        EditTextCodImp = New EditTextSBO("txtCodImp", True, "@SCGD_CONF_FINANC", "U_CodImp", FormularioSBO)
        EditTextAsReLoc = New EditTextSBO("txtAsReLoc", True, "@SCGD_CONF_FINANC", "U_AsRe_Loc", FormularioSBO)
        EditTextAsReSis = New EditTextSBO("txtAsReSis", True, "@SCGD_CONF_FINANC", "U_AsRe_Sis", FormularioSBO)
        
        EditTextFinLoc.AsignaBinding()
        EditTextFinSis.AsignaBinding()
        EditTextCuoLoc.AsignaBinding()
        EditTextCuoSis.AsignaBinding()
        EditTextIntLoc.AsignaBinding()
        EditTextIntSis.AsignaBinding()
        EditTextMorLoc.AsignaBinding()
        EditTextMorSis.AsignaBinding()
        ComboBoxNumDoc.AsignaBinding()
        ComboBoxNumNC.AsignaBinding()
        EditTextCodImp.AsignaBinding()
        EditTextAsReLoc.AsignaBinding()
        EditTextAsReSis.AsignaBinding()


    End Sub

    'Manejo de eventos de la pantalla de configuraciones de financiamiento

    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If Not pVal.FormTypeEx = FormType Then Return

        Dim strUDF As String

        If pVal.EventType = BoEventTypes.et_CHOOSE_FROM_LIST Then

            Select Case pVal.ItemUID
                Case EditTextFinLoc.UniqueId

                    strUDF = "U_Fin_Loc"

                Case EditTextFinSis.UniqueId

                    strUDF = "U_Fin_Sis"

                Case EditTextCuoLoc.UniqueId

                    strUDF = "U_Cuo_Loc"

                Case EditTextCuoSis.UniqueId

                    strUDF = "U_Cuo_Sis"

                Case EditTextIntLoc.UniqueId

                    strUDF = "U_Int_Loc"

                Case EditTextIntSis.UniqueId

                    strUDF = "U_Int_Sis"

                Case EditTextMorLoc.UniqueId

                    strUDF = "U_Mor_Loc"

                Case EditTextMorSis.UniqueId

                    strUDF = "U_Mor_Sis"
                Case EditTextAsReLoc.UniqueId
                    strUDF = "U_AsRe_Loc"

                Case EditTextAsReSis.UniqueId
                    strUDF = "U_AsRe_Sis"
                Case EditTextCodImp.UniqueId
                    strUDF = "U_CodImp"

            End Select
            CFLEvent(FormUID, pVal, strUDF)
            
        End If

    End Sub

    'Manejo de estado inicial de la pantalla de configuraciones de financiamiento, deshabilita botones de navegación de pantalla
    'Carga inicial de datos guardados en UDT de configuraciones de financiamiento

    Private Sub CargarFormulario()

        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition

        Try
            DirectCast(FormularioSBO.Items.Item("txtCodImp").Specific, SAPbouiCOM.EditText).ChooseFromListUID = DMS_Connector.Helpers.TipodeImpuesto("CFL_CodImp").ToString.Trim


            Call FormularioSBO.EnableMenu("1281", False)
            Call FormularioSBO.EnableMenu("1282", False)
            Call FormularioSBO.EnableMenu("1291", False)
            Call FormularioSBO.EnableMenu("1288", False)
            Call FormularioSBO.EnableMenu("1289", False)
            Call FormularioSBO.EnableMenu("1290", False)
            Call FormularioSBO.EnableMenu("1293", False)

            oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add

            FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE

            oCondition.Alias = "Code"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = "1"

            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CONF_FINANC").Query(oConditions)

            Dim strConsultaSeriesFac As String = "Select Series, ((Case ObjectCode when '13' then '" + My.Resources.Resource.DescripcionFacturaVentas + "' when '14' then '" + My.Resources.Resource.DescripcionNotasCredito + "' when '18' then '" + My.Resources.Resource.DescripcionDocumentosDeuda + "' end) " & _
                                           "+ ' - ' + SeriesName + (Case DocSubType when '--' then '' else ' - ' + DocSubType  end) ) Nombre from NNM1 " & _
                                           "where ObjectCode in ('13') order by Nombre"
            Dim strConsultaSeriesNC As String = "Select Series, ((Case ObjectCode when '13' then '" + My.Resources.Resource.DescripcionFacturaVentas + "' when '14' then '" + My.Resources.Resource.DescripcionNotasCredito + "' when '18' then '" + My.Resources.Resource.DescripcionDocumentosDeuda + "' end) " & _
                                           "+ ' - ' + SeriesName + (Case DocSubType when '--' then '' else ' - ' + DocSubType  end) ) Nombre from NNM1 " & _
                                           "where ObjectCode in ('14') order by Nombre"

            Dim oCombo As SAPbouiCOM.ComboBox
            
            oCombo = DirectCast(FormularioSBO.Items.Item("cboNumDoc").Specific, SAPbouiCOM.ComboBox)
            General.CargarValidValuesEnCombos(oCombo.ValidValues, strConsultaSeriesFac, CompanySBO)

            oCombo = DirectCast(FormularioSBO.Items.Item("cboNumNC").Specific, SAPbouiCOM.ComboBox)
            General.CargarValidValuesEnCombos(oCombo.ValidValues, strConsultaSeriesNC, CompanySBO)


        Catch ex As Exception

            Throw ex

        End Try

    End Sub

End Class
