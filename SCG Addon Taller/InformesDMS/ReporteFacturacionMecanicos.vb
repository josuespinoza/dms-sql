Imports DMS_Addon.ControlesSBO
Imports System.Globalization
Imports SAPbouiCOM
Imports ICompany = SAPbobsCOM.ICompany
Imports SCG.SBOFramework.UI
Imports DMSOneFramework

Public Class ReporteFacturacionMecanicos : Implements IUsaMenu, IFormularioSBO, IUsaPermisos


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

    Private _txtDateS As SCG.SBOFramework.UI.EditTextSBO
    Private _txtDateF As SCG.SBOFramework.UI.EditTextSBO
    Private _txtDiasHab As SCG.SBOFramework.UI.EditTextSBO

    Private _cboMeca As SCG.SBOFramework.UI.ComboBoxSBO

    Private _Detalle As OptionBtnSBO
    Private _Resumen As OptionBtnSBO

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
                   ByVal SBOAplication As Application, ByVal p_menuInformesDMS As String, ByVal p_strUID_FORM_ReporteFacMecanicos As String)
        m_oCompany = ocompany
        m_SBO_Application = SBOAplication
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioReporteFPM
        MenuPadre = p_menuInformesDMS
        Nombre = My.Resources.Resource.MenuReporteFPM
        IdMenu = p_strUID_FORM_ReporteFacMecanicos
        Titulo = My.Resources.Resource.MenuReporteFPM
        Posicion = 13
        FormType = p_strUID_FORM_ReporteFacMecanicos
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

            _udsFormulario.Add("cboMeca", BoDataType.dt_LONG_TEXT, 50)
            _udsFormulario.Add("txt_DateS", BoDataType.dt_DATE, 50)
            _udsFormulario.Add("txt_DateF", BoDataType.dt_DATE, 50)
            _udsFormulario.Add("rbtnDet", BoDataType.dt_LONG_TEXT, 50)
            _udsFormulario.Add("rbtnResu", BoDataType.dt_LONG_TEXT, 50)
            _udsFormulario.Add("txtDiasHab", BoDataType.dt_SHORT_TEXT, 50)

            _cboMeca = New SCG.SBOFramework.UI.ComboBoxSBO("cboMeca", FormularioSBO, True, "", "cboMeca")
            _cboMeca.AsignaBinding()

            _txtDateS = New SCG.SBOFramework.UI.EditTextSBO("txt_DateS", True, "", "txt_DateS", FormularioSBO)
            _txtDateS.AsignaBinding()

            _txtDateF = New SCG.SBOFramework.UI.EditTextSBO("txt_DateF", True, "", "txt_DateF", FormularioSBO)
            _txtDateF.AsignaBinding()

            _Resumen = New OptionBtnSBO("rbtnResu", True, "", "rbtnResu", FormularioSBO)
            _Resumen.AsignaBinding()
            _Resumen.AsignaValorUserDataSource("N")

            _Detalle = New OptionBtnSBO("rbtnDet", True, "", "rbtnDet", FormularioSBO)
            _Detalle.AsignaBinding()
            _Detalle.AsignaValorUserDataSource("Y")

            _txtDiasHab = New SCG.SBOFramework.UI.EditTextSBO("txtDiasHab", True, "", "txtDiasHab", FormularioSBO)
            _txtDiasHab.AsignaBinding()


            _txtDateF.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            _txtDateS.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            _cboMeca.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
            _cboMeca.AsignaValorUserDataSource("")
            _btnPrint = New SCG.SBOFramework.UI.ButtonSBO("btn_Print", FormularioSBO)
            _btnCancel = New SCG.SBOFramework.UI.ButtonSBO("2", FormularioSBO)

            _FormularioSBO.EnableMenu("1281", False)
            _FormularioSBO.EnableMenu("1282", False)
            _FormularioSBO.EnableMenu("1283", False)
            _FormularioSBO.EnableMenu("1284", False)
            _FormularioSBO.EnableMenu("1285", False)

            CargarMecanicos()

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
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
        Dim strTextNumeroOT As String
        Dim strNumeroOT As String = "U_SCGD_Numero_OT"


        Try
            If oCFLEvento.BeforeAction Then

                oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

            ElseIf oCFLEvento.ActionSuccess Then

                oDataTable = oCFLEvento.SelectedObjects
                If Not oDataTable Is Nothing Then
                    strTextNumeroOT = String.Format("{0}", oDataTable.GetValue(strNumeroOT, 0))

                End If
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Private Sub CargarMecanicos()
        Try
            Dim sboItem As SAPbouiCOM.Item
            Dim sboCombo As SAPbouiCOM.ComboBox

            sboItem = FormularioSBO.Items.Item(_cboMeca.UniqueId)
            sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
            Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, "Select '' as code ,  '' as nombre UNION SELECT empID ,  firstName + ' ' + lastName as nombre FROM [OHEM] with(nolock) Where U_SCGD_TipoEmp = 'T' and Active = 'Y'")

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

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

    Public Sub ManejadorEventoRadioButton(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Select Case pVal.ItemUID
            Case _Detalle.UniqueId 'Debe selecionar un mecanico
                _Detalle.AsignaValorUserDataSource("Y")
                _Resumen.AsignaValorUserDataSource("N")
                _cboMeca.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                _cboMeca.AsignaValorUserDataSource("")
            Case (_Resumen.UniqueId) 'Despliega para todos los mecanicos
                _Detalle.AsignaValorUserDataSource("N")
                _Resumen.AsignaValorUserDataSource("Y")
                _cboMeca.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_False)
                _cboMeca.AsignaValorUserDataSource("")
        End Select
    End Sub

    Public Sub ManejadorEventoItemPress(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.Before_Action Then

                Select Case pVal.ItemUID

                    Case _btnPrint.UniqueId
                        ValidaDatos(formUID, pVal, BubbleEvent)
                    Case _Detalle.UniqueId
                        ManejadorEventoRadioButton(formUID, pVal, BubbleEvent)
                    Case _Resumen.UniqueId
                        ManejadorEventoRadioButton(formUID, pVal, BubbleEvent)
                End Select

            ElseIf pVal.ActionSuccess Then
                Select Case pVal.ItemUID

                End Select
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Private Sub ValidaDatos(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim bool As Boolean = False
        Dim fechaI As String
        Dim fechaF As String
        Dim Mecnico As String
        Dim strHorasHabiles As String
        Dim parametros As String
        Dim strContieneCOMA As String

        Mecnico = _cboMeca.ObtieneValorUserDataSource()
        strHorasHabiles = _txtDiasHab.ObtieneValorUserDataSource()

        If strHorasHabiles.Contains(",") Then
            strHorasHabiles = strHorasHabiles.Replace(",", ".")
            strContieneCOMA = "Y"
        Else
            strContieneCOMA = "N"
        End If

        If String.IsNullOrEmpty(strHorasHabiles) Then
            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.HorasHabiles, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
            BubbleEvent = False
            Exit Sub
        End If

        If String.IsNullOrEmpty(_txtDateS.ObtieneValorUserDataSource()) Or String.IsNullOrEmpty(_txtDateF.ObtieneValorUserDataSource()) Then
            bool = True
        End If

        fechaI = Date.ParseExact(_txtDateS.ObtieneValorUserDataSource(), "yyyyMMdd", Nothing)
        fechaF = Date.ParseExact(_txtDateF.ObtieneValorUserDataSource(), "yyyyMMdd", Nothing)

        If IsDate(fechaI) = False Or IsDate(fechaF) = False Then
            bool = True
        End If


        If bool = True Then
            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.RptOTxEValidaTipoOT, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
            BubbleEvent = False
            Exit Sub
        Else
            If _Detalle.ObtieneValorUserDataSource() = "Y" Then
                parametros = String.Format(" {0},{1},{2},{3},{4}", strHorasHabiles, fechaF, fechaI, Mecnico, strContieneCOMA)
                Call Print(My.Resources.Resource.rptFacturacionMecanico, My.Resources.Resource.TituloRPFacturacionOrdenesTrabajoDetalle, parametros)
            Else
                parametros = String.Format(" {0},{1},{2},{3}", strHorasHabiles, fechaF, fechaI, strContieneCOMA)
                Call Print(My.Resources.Resource.rptFacturacionMecanicoresumen, My.Resources.Resource.TituloRPFacturacionOrdenesTrabajoResumido, parametros)
            End If
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
