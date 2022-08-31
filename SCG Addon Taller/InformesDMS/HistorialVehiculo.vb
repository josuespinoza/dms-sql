Imports System.Globalization
Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports DMSOneFramework
Imports ICompany = SAPbobsCOM.ICompany

Public Class HistorialVehiculo : Implements IUsaMenu, IFormularioSBO, IUsaPermisos

#Region "Declaraciones"

    'General
    Private m_oCompany As SAPbobsCOM.Company
    Private m_SBO_Application As Application

    Public n As NumberFormatInfo
    
    'Conection
    Private m_strConectionString As String
    Dim m_cn_Coneccion As New SqlClient.SqlConnection

    Public EditTextCdV As SCG.SBOFramework.UI.EditTextSBO

#End Region

    ''' <summary>
    ''' 
    ''' Declaracion Variables
    ''' </summary>
    ''' <remarks></remarks>
#Region "Variables"


    Private _Direccion_Reportes As String
    Private _ConexionSBO As String
    Private _Usuario_BD As String
    Private _ContraseñaBD As String
    Public BtnPrintSbo As SCG.SBOFramework.UI.ButtonSBO

    Private _IdMenu As String
    Private _MenuPadre As String
    Private _Nombre As String
    Private _Posicion As String
    Private _FormType As String
    Private _FormularioSBO As SAPbouiCOM.IForm
    Private _Inicializado As Boolean
    Private _NombreXML As String
    Private _Titulo As String

    Dim oDataTable As SAPbouiCOM.DataTable
    Private _applicationSbo As System.Windows.Forms.Application
    Private _company_Sbo As ICompany

    Private _txtCliente As SCG.SBOFramework.UI.EditTextSBO
    Private _txtPlaca As SCG.SBOFramework.UI.EditTextSBO
    Private _txtCodUnid As SCG.SBOFramework.UI.EditTextSBO

    Private _rbtnDetallado As OptionBtnSBO
    Private _rbtnResumido As OptionBtnSBO

    Private _CFLVehiculos As ChooseFromListSBO

    Private _udsFormulario As UserDataSources

    Private _btnPrint As SCG.SBOFramework.UI.ButtonSBO
    Private _btnCancel As SCG.SBOFramework.UI.ButtonSBO

    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon

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
#Region "Contructor"
    <CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application, p_menuInformesDMS As String, p_strUID_FORM_ReporteHistorialVehiculo As String)
        m_oCompany = ocompany
        m_SBO_Application = SBOAplication
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioReporteHistorialVehiculo
        MenuPadre = p_menuInformesDMS
        Nombre = My.Resources.Resource.MenuReporteHistorialVehiculo
        IdMenu = p_strUID_FORM_ReporteHistorialVehiculo
        Titulo = My.Resources.Resource.MenuReporteHistorialVehiculo
        Posicion = 8
        FormType = p_strUID_FORM_ReporteHistorialVehiculo

    End Sub
#End Region

#Region "Metodos"

    ''' <summary>
    ''' Incia Formulario
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario


    End Sub

    ''' <summary>
    ''' Inicia Controladores
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        Try

            _udsFormulario = FormularioSBO.DataSources.UserDataSources

            _udsFormulario.Add("txtCliente", BoDataType.dt_LONG_TEXT, 150)
            _udsFormulario.Add("txtPlaca", BoDataType.dt_LONG_TEXT, 150)
            _udsFormulario.Add("rbtnDet", BoDataType.dt_LONG_TEXT, 150)
            _udsFormulario.Add("rbtnResu", BoDataType.dt_LONG_TEXT, 150)
            _udsFormulario.Add("txtCodUnid", BoDataType.dt_LONG_TEXT, 150)


            _txtCliente = New SCG.SBOFramework.UI.EditTextSBO("txtCliente", True, "", "txtCliente", FormularioSBO)
            _txtCliente.AsignaBinding()

            _txtPlaca = New SCG.SBOFramework.UI.EditTextSBO("txtPlaca", True, "", "txtPlaca", FormularioSBO)
            _txtPlaca.AsignaBinding()

            _txtCodUnid = New SCG.SBOFramework.UI.EditTextSBO("txtCodUnid", True, "", "txtCodUnid", FormularioSBO)
            _txtCodUnid.AsignaBinding()

            _CFLVehiculos = New ChooseFromListSBO("CFL_VEH")

            _rbtnDetallado = New OptionBtnSBO("rbtnDet", True, "", "rbtnDet", FormularioSBO)
            _rbtnDetallado.AsignaBinding()
            _rbtnDetallado.AsignaValorUserDataSource("Y")
            _rbtnResumido = New OptionBtnSBO("rbtnResu", True, "", "rbtnResu", FormularioSBO)
            _rbtnResumido.AsignaBinding()

            _txtCliente.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            _txtPlaca.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            _btnPrint = New SCG.SBOFramework.UI.ButtonSBO("btnPrint", FormularioSBO)
            _btnCancel = New SCG.SBOFramework.UI.ButtonSBO("2", FormularioSBO)

            _FormularioSBO.EnableMenu("1281", False)
            _FormularioSBO.EnableMenu("1282", False)
            _FormularioSBO.EnableMenu("1283", False)
            _FormularioSBO.EnableMenu("1284", False)
            _FormularioSBO.EnableMenu("1285", False)
            _FormularioSBO.EnableMenu("1286", False)
            _FormularioSBO.EnableMenu("1287", False)

            _txtCodUnid.ItemSBO.Click()

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

        Dim strTextPlaca As String
        Dim strTextCliente As String
        Dim strTextCodUnid As String
        Dim strCodPlaca As String = "U_Num_Plac"
        Dim strCodUnid As String = "U_Cod_Unid"
        Dim strCarName As String = "U_CardName"
        Dim m_Placa As String = String.Empty


        Try
            oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

            If oCFLEvento.ActionSuccess Then

                oDataTable = oCFLEvento.SelectedObjects
                If Not oDataTable Is Nothing Then
                    strTextPlaca = String.Format("{0}", oDataTable.GetValue(strCodPlaca, 0))
                    _txtPlaca.AsignaValorUserDataSource(strTextPlaca)
                    strTextCliente = String.Format("{0}", oDataTable.GetValue(strCarName, 0))
                    _txtCliente.AsignaValorUserDataSource(strTextCliente)
                    strTextCodUnid = String.Format("{0}", oDataTable.GetValue(strCodUnid, 0))
                    _txtCodUnid.AsignaValorUserDataSource(strTextCodUnid)
                End If
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Public Sub ManejadorEventoRadioButton(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Select Case pVal.ItemUID
            Case _rbtnDetallado.UniqueId
                _rbtnDetallado.AsignaValorUserDataSource("Y")
                _rbtnResumido.AsignaValorUserDataSource("N")
            Case _rbtnResumido.UniqueId
                _rbtnDetallado.AsignaValorUserDataSource("N")
                _rbtnResumido.AsignaValorUserDataSource("Y")
        End Select
    End Sub

    Public Sub ManejadorEventoItemPress(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.Before_Action Then

                Select Case pVal.ItemUID

                    Case _btnPrint.UniqueId
                        ValidaDatos(formUID, pVal, BubbleEvent)
                    Case _rbtnDetallado.UniqueId
                        ManejadorEventoRadioButton(formUID, pVal, BubbleEvent)
                    Case _rbtnResumido.UniqueId
                        ManejadorEventoRadioButton(formUID, pVal, BubbleEvent)
                End Select

            ElseIf pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case _btnPrint.UniqueId
                        CargarParametros(formUID, pVal, BubbleEvent)
                End Select
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Private Sub ValidaDatos(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If String.IsNullOrEmpty(_txtCodUnid.ObtieneValorUserDataSource()) Then
            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.RptHVSinCodUnid, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
            BubbleEvent = False
        End If

    End Sub

    Public Sub CargarParametros(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If _rbtnDetallado.ObtieneValorUserDataSource = "Y" Then
            Call Print(My.Resources.Resource.rptHistorialVehiculo & ".rpt", My.Resources.Resource.TituloHistorialVehiculo, _txtCodUnid.ObtieneValorUserDataSource.Trim)
        Else
            Call Print(My.Resources.Resource.rptHistorialVehiculoResumido & ".rpt", My.Resources.Resource.TituloRPTHistorialVehiculoResumido, _txtCodUnid.ObtieneValorUserDataSource.Trim)
        End If


    End Sub

    Private Sub Print(ByVal strDireccionReporte As String, _
                              ByVal strBarraTitulo As String, _
                              ByVal strParametros As String)
        Try
            Dim strPathExe As String = String.Empty

            objConfiguracionGeneral = Nothing

            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, m_strConectionString)
            If m_cn_Coneccion.State = ConnectionState.Open Then
                m_cn_Coneccion.Close()
            End If
            m_cn_Coneccion.ConnectionString = m_strConectionString

            objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(m_cn_Coneccion)


            strDireccionReporte = objConfiguracionGeneral.DireccionReportes & strDireccionReporte
            strDireccionReporte = strDireccionReporte.Replace(" ", "°")
            strParametros = strParametros.Replace(" ", "°")
            strBarraTitulo = strBarraTitulo.Replace(" ", "°")

            strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

            strPathExe &= strBarraTitulo & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & m_oCompany.Server & "," & m_oCompany.CompanyDB & " " & strParametros
            Shell(strPathExe, AppWinStyle.MaximizedFocus)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub


#End Region
End Class
