'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
'Manejo de controles de la pantalla de Citas por Fecha y tipo de Agenda
'------ Inicializacion de controles en pantalla
'
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''Herencia de las librerias necesarias para el formulario
Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class CitasPorTipoAgendaFecha : Implements IFormularioSBO, IUsaMenu


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


    Public BtnPrintSbo As ButtonSBO

    Dim oDataTable As SAPbouiCOM.DataTable

    Private _IdMenu As String
    Private _MenuPadre As String
    Private _Posicion As Integer
    Private _Nombre As String

    Private _Conexion As String
    Private _DireccionReportes As String
    Private _UsuarioBD As String
    Private _ContraseñaBD As String
    
    Public EditTextEmpCode As EditTextSBO
    Public EditTextEmpName As EditTextSBO
    Public EditTextFhaDesde As EditTextSBO
    Public EditTextFhaHasta As EditTextSBO

    Public EditCboSucursal As ComboBoxSBO
    Public EditCboAgenda As ComboBoxSBO

    Public EditCbxAgenda As CheckBoxSBO
    Public EditCbxTecnico As CheckBoxSBO
    Public EditCbxSucursal As CheckBoxSBO
    Public EditCboOrdenar As ComboBoxSBO



#End Region

#Region "Propiedades"

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

    Public ReadOnly Property ApplicationSBO() As IApplication Implements IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
        End Get
    End Property

#End Region

#Region "Métodos"
    <CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application, ByVal p_menuInformesDMS As String, ByVal p_strUIDCitasXTipo As String)

        m_oCompany = ocompany
        m_SBO_Application = SBOAplication
        n = DIHelper.GetNumberFormatInfo(m_oCompany)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioReporteCitas
        MenuPadre = p_menuInformesDMS
        Nombre = My.Resources.Resource.TituloCitas
        IdMenu = p_strUIDCitasXTipo
        Posicion = 1
        FormType = "SCGD_CITASXT"
        DireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        UsuarioBd = CatchingEvents.DBUser
        ContraseñaBd = CatchingEvents.DBPassword

    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario
        If FormularioSBO IsNot Nothing Then

            CargarFormulario()
            CargarCombos()

            Dim cboCombo As ComboBox
            Dim oItem As Item
            
            oItem = FormularioSBO.Items.Item("cboOrden")
            cboCombo = CType(oItem.Specific, ComboBox)
            If cboCombo.ValidValues.Count = 0 Then
                cboCombo.ValidValues.Add("1", My.Resources.Resource.FechaCita)
                cboCombo.ValidValues.Add("2", Mid(My.Resources.Resource.NumCita, 1, Len(My.Resources.Resource.NumCita) - 2))
                cboCombo.Select("1")
            End If

        End If
    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles


        Dim userDS As UserDataSources = FormularioSBO.DataSources.UserDataSources
        FormularioSBO.Freeze(True)

        'agrega columnas al ds
        userDS.Add("c_EmpCode", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("c_EmpName", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("c_CodSuc", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("c_CodAgn", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("c_FhaDesd", BoDataType.dt_DATE, 100)
        userDS.Add("c_FhaHast", BoDataType.dt_DATE, 100)
        userDS.Add("c_allAge", BoDataType.dt_LONG_TEXT, 10)
        userDS.Add("c_allAse", BoDataType.dt_LONG_TEXT, 10)
        userDS.Add("c_allSucu", BoDataType.dt_LONG_TEXT, 10)
        userDS.Add("c_Orden", BoDataType.dt_LONG_TEXT, 10)

        EditTextEmpName = New EditTextSBO("txtEmpNom", True, "", "c_EmpName", FormularioSBO)
        EditTextEmpCode = New EditTextSBO("txtCodEmp", True, "", "c_EmpCode", FormularioSBO)
        EditTextFhaDesde = New EditTextSBO("txtFDes", True, "", "c_FhaDesd", FormularioSBO)
        EditTextFhaHasta = New EditTextSBO("txtFHas", True, "", "c_FhaHast", FormularioSBO)

        EditCboSucursal = New ComboBoxSBO("cboSucur", FormularioSBO, True, "", "c_CodSuc")  ' ("cboEstado", FormularioSBO, True, m_strCita, "U_Estado")
        EditCboAgenda = New ComboBoxSBO("cboAgenda", FormularioSBO, True, "", "c_CodAgn")
        EditCboOrdenar = New ComboBoxSBO("cboOrden", FormularioSBO, True, "", "c_Orden")

        BtnPrintSbo = New ButtonSBO("btnPrint", FormularioSBO)

        EditCbxAgenda = New CheckBoxSBO("cbxAgenda", True, "", "c_allAge", FormularioSBO)
        EditCbxTecnico = New CheckBoxSBO("cbxAsesor", True, "", "c_allAse", FormularioSBO)
        EditCbxSucursal = New CheckBoxSBO("cbxSucu", True, "", "c_allSucu", FormularioSBO)

        EditTextEmpName.AsignaBinding()
        EditTextEmpCode.AsignaBinding()
        EditTextFhaDesde.AsignaBinding()
        EditTextFhaHasta.AsignaBinding()

        EditCboAgenda.AsignaBinding()
        EditCboSucursal.AsignaBinding()
        EditCboOrdenar.AsignaBinding()

        EditCbxAgenda.AsignaBinding()
        EditCbxTecnico.AsignaBinding()
        EditCbxSucursal.AsignaBinding()

        FormularioSBO.Freeze(False)
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
                    If pval.ItemUID = EditTextEmpCode.UniqueId Then
                        EditTextEmpCode.AsignaValorUserDataSource(oDataTable.GetValue("empID", 0))
                        EditTextEmpName.AsignaValorUserDataSource(oDataTable.GetValue("firstName", 0) & " " & oDataTable.GetValue("lastName", 0))
                    End If
                End If
            End If


        Catch ex As Exception

        End Try




    End Sub

#End Region


#Region "Metodos"
    Sub ApplicationSboOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If Not pVal.FormTypeEx = FormType Then Return

            If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

                ManejadorEventoItemPress(FormUID, pVal, BubbleEvent)

            ElseIf pVal.EventType = BoEventTypes.et_CHOOSE_FROM_LIST Then

                ManejadorEventoChooseFromList(FormUID, pVal, BubbleEvent)

            ElseIf pVal.EventType = BoEventTypes.et_COMBO_SELECT Then

                ManejoEventosCombo(FormUID, pVal, BubbleEvent)

            End If


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

#End Region
End Class
