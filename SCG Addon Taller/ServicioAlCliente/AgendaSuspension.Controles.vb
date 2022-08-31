Imports System.Collections.Generic
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany
Imports System.Runtime.InteropServices

Partial Public Class AgendaSuspension : Implements IFormularioSBO, IUsaMenu

#Region "Declaraciones"

    Private _formType As String
    Private _nombreXml As String
    Private _titulo As String
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _applicationSbo As IApplication
    Private _companySbo As ICompany
    Private _idMenu As String
    Private _menuPadre As String
    Private _nombre As String
    Private _posicion As String
    Private _strConexion As String
    Private _strDireccionReportes As String
    Private _strUsuarioBD As String
    Private _strContraseñaBD As String


    Dim m_oCompany As SAPbobsCOM.Company
    Dim m_oApplicacion As Application

    Public EditTextFechaDesde As EditTextSBO
    Public EditTextHoraDesde As EditTextSBO
    Public EditTextFechaHasta As EditTextSBO
    Public EditTextHoraHasta As EditTextSBO
    Public EditTextObserv As EditTextSBO
    Public EditTextDocEntry As EditTextSBO

    Public EditCboSucursal As ComboBoxSBO
    Public EditCboAgenda As ComboBoxSBO
    Public EditCbxActivo As CheckBoxSBO

    Public EditBtnAgenda1 As ButtonSBO
    Public EditBtnAgenda2 As ButtonSBO
    Public EditBtnAceptar As ButtonSBO
    Public EditBtnCancel As ButtonSBO
    Public EditBtnLimpiar As ButtonSBO
    Public EditBtnAgendaMult As ButtonSBO
    
    Public EditOptRango As OptionBtnSBO
    Public EditOptMultiple As OptionBtnSBO
    
    Private m_strSuspender As String = "@SCGD_AGENDA_SUSP"
    Private WithEvents _frmAgendaCitas As frmCalendario
    Private WithEvents _frmAgendaCitasColor As frmCalendarioColor
    Private m_blnUsaAgenda1 As Boolean = False

    Private md_Configuracion As SAPbouiCOM.DataTable
    Private md_Citas As SAPbouiCOM.DataTable
    Private md_Suspension As SAPbouiCOM.DataTable

    Private MatrizSusp As MatrizSuspension
    
    Private m_strCodCitasCancel As String
    Private m_HoraInicioTaller As String
    Private m_HoraCierreTaller As String
    Private m_strCodSucur As String
    Private m_StrCodAgenda As String
    Private m_strObservaciones As String
    
    Private m_ListaSuspencion As List(Of frmCalendario.Reservacion)
    Private m_ListaSuspencionColor As List(Of frmCalendarioColor.Reservacion)

#End Region


#Region "Propieadades"


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

    Public Property Nombre() As String Implements IUsaMenu.Nombre
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
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


#End Region

#Region "Metodos / Funciones"

    Public Sub New(ByVal application As Application, ByVal companySbo As SAPbobsCOM.Company, ByVal p_menuCitas As String, ByVal p_strUISCGD_SuspenderAgenda As String)
        _companySbo = companySbo
        _applicationSbo = application
        m_oCompany = companySbo
        m_oApplicacion = application
        NombreXml = Windows.Forms.Application.StartupPath & My.Resources.Resource.XMLAgendaSuspension
        MenuPadre = p_menuCitas
        Nombre = "Suspesion de Agenda"
        IdMenu = p_strUISCGD_SuspenderAgenda
        Titulo = My.Resources.Resource.TituloAgendaSuspencion
        Posicion = 4
        FormType = p_strUISCGD_SuspenderAgenda
        StrDireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        StrUsuarioBD = CatchingEvents.DBUser
        StrContraseñaBD = CatchingEvents.DBPassword
    End Sub


    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles
        FormularioSBO.Freeze(True)

        EditTextFechaDesde = New EditTextSBO("txtFhaDesd", True, m_strSuspender, "U_Fha_Desde", FormularioSBO)
        EditTextHoraDesde = New EditTextSBO("txtHoraDes", True, m_strSuspender, "U_Hora_Desde", FormularioSBO)
        EditTextFechaHasta = New EditTextSBO("txtFhaHast", True, m_strSuspender, "U_Fha_Hasta", FormularioSBO)
        EditTextHoraHasta = New EditTextSBO("txtHoraHas", True, m_strSuspender, "U_Hora_Hasta", FormularioSBO)
        EditTextObserv = New EditTextSBO("txtObserv", True, m_strSuspender, "U_Observ", FormularioSBO)
        EditTextDocEntry = New EditTextSBO("txDocEntry", True, m_strSuspender, "DocEntry", FormularioSBO)

        EditCboAgenda = New ComboBoxSBO("cboAgenda", FormularioSBO, True, m_strSuspender, "U_Cod_Agenda")
        EditCboSucursal = New ComboBoxSBO("cboSucur", FormularioSBO, True, m_strSuspender, "U_Cod_Sucur")

        EditBtnAceptar = New ButtonSBO("1", FormularioSBO)
        EditBtnAgenda1 = New ButtonSBO("btnAgDesde", FormularioSBO)
        EditBtnAgenda2 = New ButtonSBO("btnAgHasta", FormularioSBO)
        EditBtnCancel = New ButtonSBO("2", FormularioSBO)
        EditBtnLimpiar = New ButtonSBO("btnLimpiar", FormularioSBO)
        EditBtnAgendaMult = New ButtonSBO("btnAgMulti", FormularioSBO)

        EditOptRango = New OptionBtnSBO("rbnRango", FormularioSBO)
        EditOptMultiple = New OptionBtnSBO("rbnMultip", FormularioSBO)

        EditTextFechaDesde.AsignaBinding()
        EditTextHoraDesde.AsignaBinding()
        EditTextFechaHasta.AsignaBinding()
        EditTextHoraHasta.AsignaBinding()
        EditTextObserv.AsignaBinding()
        EditTextDocEntry.AsignaBinding()

        EditCboAgenda.AsignaBinding()
        EditCboSucursal.AsignaBinding()

        FormularioSBO.Freeze(False)
    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario
        Try

            CargarCombos()

            md_Configuracion = FormularioSBO.DataSources.DataTables.Add("DatosConfig")
            md_Citas = FormularioSBO.DataSources.DataTables.Add("DatosCitas")

            FormularioSBO.DataSources.UserDataSources.Add("ValOption", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 20)

            CargarFormulario()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    Public Sub CargarFormulario()
        Dim oItem As SAPbouiCOM.Item
        Dim optBtn As SAPbouiCOM.OptionBtn

        Try
            md_Suspension = FormularioSBO.DataSources.DataTables.Add("TablaSusp")

            md_Suspension.Columns.Add("fhaSusp", BoFieldsType.ft_Date)
            md_Suspension.Columns.Add("HraDesde", BoFieldsType.ft_AlphaNumeric, 50)
            md_Suspension.Columns.Add("HraHasta", BoFieldsType.ft_AlphaNumeric, 50)

            MatrizSusp = New MatrizSuspension("mtxSuspen", FormularioSBO, "TablaSusp")

            MatrizSusp.CreaColumnas()
            MatrizSusp.LigaColumnas()

            'Ligar OptionButtons
            oItem = FormularioSBO.Items.Item(EditOptRango.UniqueId)
            optBtn = DirectCast(oItem.Specific, SAPbouiCOM.OptionBtn)

            optBtn.DataBind.SetBound(True, , "ValOption")

            oItem = FormularioSBO.Items.Item(EditOptMultiple.UniqueId)
            optBtn = DirectCast(oItem.Specific, SAPbouiCOM.OptionBtn)

            optBtn.GroupWith(EditOptRango.UniqueId)
            optBtn.DataBind.SetBound(True, , "ValOption")
            

            Call HabilitaControlesMultiple(False)
            Call HabilitaControlesRango(False)

            'EditOptMultiple.AsignaValorUserDataSource("N")

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub


    Public Sub CargarCombos()

        Try
            Dim sboItem As SAPbouiCOM.Item
            Dim sboCombo As SAPbouiCOM.ComboBox

            sboItem = FormularioSBO.Items.Item(EditCboSucursal.UniqueId)
            sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
            Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, "SELECT Code, Name FROM [@SCGD_SUCURSALES]  ORDER BY name")

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub


    


    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If Not pVal.FormTypeEx = FormType Then Return

        If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

            ManejadorEventosItemPressed(FormUID, pVal, BubbleEvent)

        ElseIf pVal.EventType = BoEventTypes.et_COMBO_SELECT Then

            ManejadorEventoCombos(FormUID, pVal, BubbleEvent)

        End If

    End Sub


    Public Sub ManejadorEventosMenus(ByVal pval As SAPbouiCOM.MenuEvent, ByVal formUID As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Try

            If pval.MenuUID = "1281" OrElse
                pval.MenuUID = "1282" OrElse
                pval.MenuUID = "1290" OrElse
                 pval.MenuUID = "1288" OrElse
                pval.MenuUID = "1289" OrElse
                pval.MenuUID = "1291" Then

                If Not IsNothing(m_ListaSuspencion) Then

                    md_Suspension.Rows.Clear()
                    m_ListaSuspencion.Clear()
                End If
                MatrizSusp.Matrix.LoadFromDataSource()

                HabilitaControlesMultiple(False)
                Select Case pval.MenuUID

                    Case "1281"
                        FormularioSBO.Items.Item(EditTextDocEntry.UniqueId).Enabled = True
                    Case Else
                        FormularioSBO.Items.Item(EditTextDocEntry.UniqueId).Enabled = False
                End Select
            End If

            'If pval.MenuUID = "1281" Then
            '    FormularioSBO.Items.Item(EditTextAgenda.UniqueId).Enabled = True
            'End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub


    Public Sub ManejadorEventosItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim sboCombo As SAPbouiCOM.ComboBox
            Dim descripcionAgenda As String
            Dim l_StrCodAgenda As String
            Dim l_strCodSucur As String

            Dim sboItem As SAPbouiCOM.Item
            Dim sboRadio As SAPbouiCOM.OptionBtn
            Dim versionSap As Integer
            Dim m_blnVersion9 As Boolean = True

            If pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then
                Select Case pVal.ItemUID
                    Case EditBtnAgenda1.UniqueId, EditBtnAgenda2.UniqueId, EditBtnAgendaMult.UniqueId


                        sboCombo = DirectCast(FormularioSBO.Items.Item(EditCboAgenda.UniqueId).Specific, ComboBox)
                        descripcionAgenda = sboCombo.Selected.Description
                        l_StrCodAgenda = sboCombo.Selected.Value

                        sboCombo = DirectCast(FormularioSBO.Items.Item(EditCboSucursal.UniqueId).Specific, ComboBox)
                        l_strCodSucur = sboCombo.Selected.Value

                        Dim fecha As Date
                        fecha = DateTime.Now
                        Utilitarios.RetornaFechaFormatoRegional(fecha.ToString("yyyy-MM-dd"))

                        Dim ptr As IntPtr = GetForegroundWindow()
                        Dim wrapper As New WindowWrapper(ptr)


                        versionSap = m_oCompany.Version
                        If versionSap < 900000 Then
                            m_blnVersion9 = False
                        End If

                        If pVal.ItemUID = EditBtnAgenda1.UniqueId OrElse pVal.ItemUID = EditBtnAgenda2.UniqueId Then
                            _frmAgendaCitas = New frmCalendario(True, Date.Parse(fecha), descripcionAgenda, l_StrCodAgenda, l_strCodSucur, m_strCodCitasCancel, m_blnVersion9, True, m_oCompany, ApplicationSBO)
                        ElseIf pVal.ItemUID = EditBtnAgendaMult.UniqueId Then
                            _frmAgendaCitas = New frmCalendario(True, Date.Parse(fecha), descripcionAgenda, l_StrCodAgenda, l_strCodSucur, m_strCodCitasCancel, m_blnVersion9, True, m_oCompany, ApplicationSBO, pVal.FormTypeEx)
                        End If
                        _frmAgendaCitas.ShowInTaskbar = False

                        If m_blnVersion9 Then
                            IniciaTimer()
                            _frmAgendaCitas.ShowDialog(wrapper)
                            FinalizaTimer()
                        Else
                            _frmAgendaCitas.ShowDialog(wrapper)
                        End If



                    Case EditBtnAceptar.UniqueId

                        sboItem = FormularioSBO.Items.Item(EditOptMultiple.UniqueId)
                        sboRadio = DirectCast(sboItem.Specific, SAPbouiCOM.OptionBtn)

                        If sboRadio.Selected Then

                            If m_ListaSuspencion.Count > 1 Then


                                CrearReservasDesdeLista(m_strCodSucur, m_StrCodAgenda)
                            End If

                        End If


                End Select
            ElseIf pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then

                m_strObservaciones = String.Empty

                Select Case pVal.ItemUID
                    Case EditBtnAceptar.UniqueId

                        sboItem = FormularioSBO.Items.Item(EditOptMultiple.UniqueId)
                        sboRadio = DirectCast(sboItem.Specific, SAPbouiCOM.OptionBtn)

                        If sboRadio.Selected Then
                            UsoSeleccionMultiple(FormUID, pVal, BubbleEvent)
                        End If

                        Call ValidarDatos(FormUID, pVal, BubbleEvent)

                        If BubbleEvent = False Then
                            Exit Sub
                        Else
                            Call ValidarSuspension(FormUID, pVal, BubbleEvent)
                        End If

                    Case EditBtnCancel.UniqueId

                    Case EditBtnAgenda1.UniqueId, EditBtnAgenda2.UniqueId, EditBtnAgendaMult.UniqueId

                        If pVal.ItemUID = EditBtnAgenda1.UniqueId Then
                            m_blnUsaAgenda1 = True
                        ElseIf pVal.ItemUID = EditBtnAgenda2.UniqueId Then
                            m_blnUsaAgenda1 = False
                        End If

                        sboCombo = DirectCast(FormularioSBO.Items.Item(EditCboAgenda.UniqueId).Specific, ComboBox)
                        If IsNothing(sboCombo.Selected) Then
                            BubbleEvent = False
                            _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinAgenda, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        End If

                    Case EditOptRango.UniqueId
                        HabilitaControlesRango(True)
                        HabilitaControlesMultiple(False)

                        sboItem = FormularioSBO.Items.Item(EditOptRango.UniqueId)
                        sboRadio = DirectCast(sboItem.Specific, SAPbouiCOM.OptionBtn)

                    Case EditOptMultiple.UniqueId

                        HabilitaControlesRango(False)
                        HabilitaControlesMultiple(True)

                    Case EditBtnLimpiar.UniqueId
                        ValidarMatrizReservas(pVal, BubbleEvent)

                End Select
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub UsoSeleccionMultiple(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim l_strCod_Sucur As String
            Dim l_strCodAgenda As String
            Dim l_fhaFDesde As Date
            Dim l_fhaFHasta As Date
            Dim l_strHoraDesde As String
            Dim l_strHoraHasta As String
            Dim l_strEstado As String
            Dim l_strObserv As String

            l_strCodAgenda = EditCboAgenda.ObtieneValorDataSource()
            l_strCod_Sucur = EditCboSucursal.ObtieneValorDataSource()
            
            If m_ListaSuspencion.Count > 0 Then

                l_fhaFDesde = m_ListaSuspencion(0).fhaDesde
                l_fhaFHasta = m_ListaSuspencion(0).fhaHasta

                l_strHoraDesde = m_ListaSuspencion(0).fhaDesde.ToString("HH") & m_ListaSuspencion(0).fhaDesde.ToString("mm")
                l_strHoraHasta = m_ListaSuspencion(0).fhaHasta.ToString("HH") & m_ListaSuspencion(0).fhaHasta.ToString("mm")

                EditTextFechaDesde.AsignaValorDataSource(l_fhaFDesde.ToString("yyyyMMdd"))
                EditTextHoraDesde.AsignaValorDataSource(l_strHoraDesde)
                EditTextFechaHasta.AsignaValorDataSource(l_fhaFHasta.ToString("yyyyMMdd"))
                EditTextHoraHasta.AsignaValorDataSource(l_strHoraHasta)

            End If

            m_strObservaciones = EditTextObserv.ObtieneValorDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub CrearReservasDesdeLista(ByVal p_strCodSucur As String, ByVal p_strCodAgenda As String)

        Try

            Dim l_strHraDesde As String
            Dim l_strHraHasta As String
            Dim l_fhaDesde As Date
            Dim l_fhaHasta As Date
            Dim l_strComentario As String

            Dim fhaDesde As Date
            Dim fhaHasta As Date


            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData

            If m_ListaSuspencion.Count <> 0 Then

                oCompanyService = m_oCompany.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_AgnSusp")

                For i As Integer = 1 To m_ListaSuspencion.Count - 1
                    oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)

                    l_strHraDesde = Format(m_ListaSuspencion(i).fhaDesde, "HHmm")
                    l_strHraHasta = Format(m_ListaSuspencion(i).fhaHasta, "HHmm")
                    l_fhaDesde = Date.Parse(m_ListaSuspencion(i).fhaDesde)
                    l_fhaHasta = Date.Parse(m_ListaSuspencion(i).fhaHasta)
                    l_strComentario = m_strObservaciones

                    fhaDesde = Date.Parse(m_ListaSuspencion(i).fhaDesde)
                    fhaHasta = Date.Parse(m_ListaSuspencion(i).fhaHasta)

                    oGeneralData.SetProperty("U_Cod_Sucur", p_strCodSucur)
                    oGeneralData.SetProperty("U_Cod_Agenda", p_strCodAgenda)

                    oGeneralData.SetProperty("U_Fha_Desde", Date.ParseExact(fhaDesde.ToString("yyyyMMdd"), "yyyyMMdd", Nothing))
                    oGeneralData.SetProperty("U_Fha_Hasta", Date.ParseExact(fhaHasta.ToString("yyyyMMdd"), "yyyyMMdd", Nothing))

                    oGeneralData.SetProperty("U_Hora_Desde", Convert.ToDateTime(Utilitarios.FormatoHora(l_strHraDesde)))
                    oGeneralData.SetProperty("U_Hora_Hasta", Convert.ToDateTime(Utilitarios.FormatoHora(l_strHraHasta)))

                    ' oGeneralData.SetProperty("U_Fha_Desde", Utilitarios.RetornaFechaFormatoDB(l_fhaDesde, m_oCompany.Server))
                    ' oGeneralData.SetProperty("U_Hora_Desde", Convert.ToDateTime(Utilitarios.FormatoHora(l_strHraDesde)))
                    ' oGeneralData.SetProperty("U_Hora_Desde", Date.ParseExact(fhaDesde.ToString("yyyyMMdd"), "yyyyMMdd", Nothing))
                    ' oGeneralData.SetProperty("U_Fha_Hasta", Utilitarios.RetornaFechaFormatoDB(l_fhaHasta, m_oCompany.Server))
                    'oGeneralData.SetProperty("U_Hora_Hasta", Convert.ToDateTime(Utilitarios.FormatoHora(l_strHraHasta)))

                    oGeneralData.SetProperty("U_Observ", l_strComentario)
                    oGeneralData.SetProperty("U_Estado", "Y")
                    oGeneralService.Add(oGeneralData)

                Next

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub


    Public Sub ManejadorEventoCombos(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim cboCombo As SAPbouiCOM.ComboBox
            Dim oItem As SAPbouiCOM.Item
            Dim l_strSucursal As String
            Dim l_strSQLAgendas As String
            Dim l_strSQLConfig As String

            l_strSQLAgendas = "SELECT DocNum, U_Agenda, U_CodTecnico, U_NameTecnico FROM [@SCGD_AGENDA] where U_Cod_Sucursal = '{0}' AND U_EstadoLogico = 'Y'"
            l_strSQLConfig = " SELECT U_CodCitaCancel ,U_CodCitaNueva, U_HoraInicio, U_HoraFin, U_UsaDurEC FROM [@SCGD_CONF_SUCURSAL] WHERE U_Sucurs = '{0}'"


            If pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then
                Select Case pVal.ItemUID
                    Case EditCboAgenda.UniqueId
                        m_StrCodAgenda = EditCboAgenda.ObtieneValorDataSource

                    Case EditCboSucursal.UniqueId
                        m_strCodSucur = EditCboSucursal.ObtieneValorDataSource

                        If pVal.ItemUID = EditCboSucursal.UniqueId Then
                            oItem = FormularioSBO.Items.Item(EditCboSucursal.UniqueId)
                            cboCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
                            l_strSucursal = cboCombo.Selected.Value

                            If cboCombo.Active Then
                                oItem = FormularioSBO.Items.Item(EditCboAgenda.UniqueId)
                                cboCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
                                Call Utilitarios.CargarValidValuesEnCombos(cboCombo.ValidValues, _
                                                                            String.Format(l_strSQLAgendas, l_strSucursal))
                            End If

                            l_strSQLConfig = String.Format(l_strSQLConfig, l_strSucursal)

                            md_Configuracion = FormularioSBO.DataSources.DataTables.Item("DatosConfig")
                            md_Configuracion.Clear()
                            md_Configuracion.ExecuteQuery(l_strSQLConfig)

                            If md_Configuracion.Rows.Count <> 0 Then
                                m_strCodCitasCancel = md_Configuracion.GetValue("U_CodCitaCancel", 0)
                                m_HoraInicioTaller = md_Configuracion.GetValue("U_HoraInicio", 0)
                                m_HoraCierreTaller = md_Configuracion.GetValue("U_HoraFin", 0)

                            End If


                        End If
                End Select

            ElseIf pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then
                Select Case pVal.ItemUID
                    Case EditCboAgenda.UniqueId
                    Case EditCboSucursal.UniqueId
                End Select
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub CargarCombosLoad(ByRef oTmpForm As SAPbouiCOM.Form)

        Dim l_strCodSucursal As String
        Dim sboItem As SAPbouiCOM.Item
        Dim sboCombo As SAPbouiCOM.ComboBox

        l_strCodSucursal = EditCboSucursal.ObtieneValorDataSource()

        Call HabilitarCombos(FormularioSBO, EditCboAgenda.UniqueId)
        sboItem = FormularioSBO.Items.Item(EditCboAgenda.UniqueId)
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues,
                                                   String.Format(" SELECT DocNum, U_Agenda FROM [DBO].[@SCGD_AGENDA] where U_Cod_Sucursal = '{0}'", l_strCodSucursal))


    End Sub

    Protected Friend Sub HabilitarCombos(ByRef oForm As SAPbouiCOM.Form, _
                                        ByVal strIDItem As String)
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item

        Try
            If oForm IsNot Nothing Then
                oItem = oForm.Items.Item(strIDItem)
                oItem.Enabled = True
                cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try
    End Sub


    Public Sub HabilitaControlesRango(ByVal p_Valor As Boolean)
        Try

            With FormularioSBO.Items
                .Item(EditTextFechaDesde.UniqueId).Enabled = p_Valor
                .Item(EditTextFechaHasta.UniqueId).Enabled = p_Valor
                .Item(EditTextHoraDesde.UniqueId).Enabled = p_Valor
                .Item(EditTextHoraHasta.UniqueId).Enabled = p_Valor
                .Item(EditBtnAgenda1.UniqueId).Enabled = p_Valor
                .Item(EditBtnAgenda2.UniqueId).Enabled = p_Valor

            End With


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub HabilitaControlesMultiple(ByVal p_Valor As Boolean)
        Try
            With FormularioSBO.Items
                .Item(EditBtnLimpiar.UniqueId).Enabled = p_Valor
                .Item(EditBtnAgendaMult.UniqueId).Enabled = p_Valor
            End With
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub ValidarMatrizReservas(ByVal pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If m_ListaSuspencion.Count <> 0 Then
                If ApplicationSBO.MessageBox("Existe suspesiones pendientes, desea continuar", 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then

                    md_Suspension.Rows.Clear()
                    m_ListaSuspencion.Clear()

                    MatrizSusp.Matrix.LoadFromDataSource()

                    BubbleEvent = False
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub ValidarDatos(ByVal FormUID As String, _
                             ByVal pVal As SAPbouiCOM.ItemEvent, _
                             ByRef BubbleEvent As Boolean)
        Try
            Dim l_strFhaDesde As String
            Dim l_strFhaHasta As String
            Dim l_strHoraDesde As String
            Dim l_strHoraHasta As String
            Dim l_strCodeSucur As String
            Dim l_strCodeAgenda As String
            Dim blnResult As Boolean = False

            Dim FechaDesde As Date
            Dim FechaHasta As Date

            Dim l_FhaDesde As Date
            Dim l_FhaHasta As Date

            l_strFhaDesde = EditTextFechaDesde.ObtieneValorDataSource()
            l_strFhaHasta = EditTextFechaHasta.ObtieneValorDataSource()
            l_strHoraDesde = EditTextHoraDesde.ObtieneValorDataSource()
            l_strHoraHasta = EditTextHoraHasta.ObtieneValorDataSource()
            l_strCodeSucur = EditCboSucursal.ObtieneValorDataSource()
            l_strCodeAgenda = EditCboAgenda.ObtieneValorDataSource()

            If String.IsNullOrEmpty(l_strCodeSucur) Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorAgendaSinSucursal, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            ElseIf String.IsNullOrEmpty(l_strCodeAgenda) Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorAgendaSinAgenda, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            ElseIf String.IsNullOrEmpty(l_strFhaDesde) OrElse String.IsNullOrEmpty(l_strFhaHasta) Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorAgendaSinFecha, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            ElseIf String.IsNullOrEmpty(l_strHoraDesde) OrElse String.IsNullOrEmpty(l_strHoraHasta) Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorAgendaSinHora, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            ElseIf l_strFhaDesde = l_strFhaHasta AndAlso l_strHoraDesde = l_strHoraHasta Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorAgendaFechaHoraIgual, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub

            Else
                blnResult = True
            End If

            FechaDesde = DateTime.ParseExact(l_strFhaDesde, "yyyyMMdd", Nothing) ' FormatDateTime(l_strFhaDesde, DateFormat.ShortDate) 
            FechaHasta = DateTime.ParseExact(l_strFhaHasta, "yyyyMMdd", Nothing)

            l_FhaDesde = DateTime.Parse(FechaDesde & " " & Utilitarios.FormatoHora(l_strHoraDesde))
            l_FhaHasta = DateTime.Parse(FechaHasta & " " & Utilitarios.FormatoHora(l_strHoraHasta))

            If l_FhaDesde >= l_FhaHasta Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorAgendaSuspesionFechas, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            End If

            If ValidarFechas(FechaDesde, FechaHasta, _
                             l_strHoraDesde, l_strHoraHasta, l_strCodeSucur, l_strCodeAgenda) <> String.Empty Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorAgendaExisteChoqueCita, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            End If

        Catch ex As Exception
            BubbleEvent = False
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Function ValidarFechas(ByVal p_FhaDesde As DateTime, ByVal p_FhaHasta As DateTime, _
                                  ByVal p_HoraDesde As String, ByVal p_HoraHasta As String, _
                                  ByVal p_strSucursal As String, ByVal p_strAgenda As String) As String
        Try
            Dim l_strSQLCita As String
            Dim l_strSQLConfig As String
            Dim l_strCita As String = ""
            Dim l_strCodCitaCancel As String
            Dim l_blnUsaIntevStarndar As Boolean = False
            Dim l_fhaReservInicio As DateTime
            Dim l_fhaReservFin As DateTime
            Dim l_fhaCitaFinal As DateTime
            Dim l_fhaCitaInicio As DateTime


            l_strSQLConfig = " SELECT U_Sucurs, U_HoraInicio, U_HoraFin, U_UsaDurEC, U_CodCitaCancel FROM [@SCGD_CONF_SUCURSAL] " & _
                                " WHERE U_Sucurs = '{0}'"

            'l_strSQLCita = "SELECT CI.DocEntry, CI.U_NumCita, CI.U_FechaCita, CI.U_HoraCita, CI.U_Cod_Sucursal, CI.U_Cod_Agenda, CI.U_Num_Serie,  " & _
            '                    " ISNULL( SUM (IT.U_SCGD_Duracion), 0) as U_SCGD_Duracion,CI.U_Num_Cot" & _
            '                    " FROM [dbo].[@SCGD_CITA] CI" & _
            '                    " LEFT OUTER JOIN  OQUT QU ON	QU.DocEntry = CI.U_Num_Cot	AND QU.U_SCGD_NoSerieCita is not null AND QU.U_SCGD_NoCita is not null" & _
            '                    " LEFT OUTER JOIN QUT1 Q1 ON Q1.DocEntry = QU.DocEntry	AND Q1.U_SCGD_Aprobado in (1, 4)" & _
            '                    " INNER JOIN OITM IT ON IT.ItemCode = Q1.ItemCode" & _
            '                    " WHERE  (U_FechaCita BETWEEN '{0}' AND '{1}') AND U_Cod_Sucursal = '{2}' AND U_Cod_Agenda = '{3}' AND U_Estado <> '{4}'" & _
            '                    " group by CI.DocEntry,   CI.U_NumCita, CI.U_FechaCita, CI.U_HoraCita, CI.U_Cod_Sucursal, CI.U_Cod_Agenda, CI.U_Num_Serie, CI.U_Num_Cot"



            l_strSQLCita = " SELECT	CI.DocEntry, CI.U_NumCita, CI.U_FechaCita, CI.U_HoraCita, CI.U_Cod_Sucursal, CI.U_Cod_Agenda, CI.U_Num_Serie, ISNULL( SUM (IT.U_SCGD_Duracion), 0) as U_SCGD_Duracion,CI.U_Num_Cot " +
                            " FROM [dbo].[@SCGD_CITA] CI " +
                            " LEFT OUTER JOIN  OQUT QU ON	QU.DocEntry = CI.U_Num_Cot 	" +
                                    " AND QU.U_SCGD_NoSerieCita is not null " +
                                    " AND QU.U_SCGD_NoCita is not null " +
                            " LEFT OUTER JOIN QUT1 Q1 ON Q1.DocEntry = QU.DocEntry	" +
                                    " AND Q1.U_SCGD_Aprobado in (1, 4) " +
                            " INNER JOIN OITM IT ON IT.ItemCode = Q1.ItemCode " +
                            " WHERE  (CI.U_FechaCita BETWEEN '{0}' AND '{1}') " +
                              " AND CI.U_Cod_Sucursal = '{2}' " +
                              " AND CI.U_Cod_Agenda = '{3}' " +
                              " AND Ci.U_Estado <> '{4}' " +
                            " GROUP BY CI.DocEntry,CI.U_NumCita, CI.U_FechaCita,CI.U_HoraCita,CI.U_Cod_Sucursal,CI.U_Cod_Agenda,CI.U_Num_Serie,CI.U_Num_Cot"

            l_strSQLConfig = String.Format(l_strSQLConfig, p_strSucursal)

            md_Configuracion.Clear()
            md_Configuracion.ExecuteQuery(l_strSQLConfig)


            If md_Configuracion.Rows.Count > 0 AndAlso
                md_Configuracion.GetValue("U_Sucurs", 0) <> "" Then

                If String.IsNullOrEmpty(md_Configuracion.GetValue("U_UsaDurEC", 0)) OrElse
                   md_Configuracion.GetValue("U_UsaDurEC", 0) = "N" Then
                    l_blnUsaIntevStarndar = False
                Else
                    l_blnUsaIntevStarndar = True
                End If

                l_strCodCitaCancel = md_Configuracion.GetValue("U_CodCitaCancel", 0)

            End If

            l_strSQLCita = String.Format(l_strSQLCita, _
                                         Utilitarios.RetornaFechaFormatoDB(p_FhaDesde, m_oCompany.Server), _
                                          Utilitarios.RetornaFechaFormatoDB(p_FhaHasta, m_oCompany.Server), _
                                         p_strSucursal, p_strAgenda, l_strCodCitaCancel)

            md_Citas.Clear()
            md_Citas.ExecuteQuery(l_strSQLCita)

            l_fhaReservInicio = DateTime.Parse(p_FhaDesde & " " & Utilitarios.FormatoHora(p_HoraDesde))
            l_fhaReservFin = DateTime.Parse(p_FhaHasta & " " & Utilitarios.FormatoHora(p_HoraHasta))

            If md_Citas.Rows.Count <> 0 Then
                If md_Citas.GetValue("DocEntry", 0) <> 0 Then
                    For i As Integer = 0 To md_Citas.Rows.Count - 1

                        l_fhaCitaInicio = DateTime.Parse(md_Citas.GetValue("U_FechaCita", i) & " " & Utilitarios.FormatoHora(md_Citas.GetValue("U_HoraCita", i)))
                        l_fhaCitaFinal = DateTime.Parse(l_fhaCitaInicio.AddMinutes(md_Citas.GetValue("U_SCGD_Duracion", i) - 1))

                        If l_blnUsaIntevStarndar Then

                            If (l_fhaReservInicio >= l_fhaCitaInicio AndAlso l_fhaReservInicio <= l_fhaCitaFinal) OrElse
                                (l_fhaReservFin >= l_fhaCitaInicio AndAlso l_fhaReservFin <= l_fhaCitaFinal) OrElse
                                (l_fhaReservInicio <= l_fhaCitaInicio AndAlso l_fhaReservFin >= l_fhaCitaFinal) Then

                                l_strCita = md_Citas.GetValue("U_Num_Serie", i) & "-" & md_Citas.GetValue("U_NumCita", i)
                                Exit For
                            End If

                        ElseIf l_blnUsaIntevStarndar = False Then
                            If l_fhaReservInicio <= l_fhaCitaInicio AndAlso l_fhaCitaInicio <= l_fhaReservInicio Then

                                l_strCita = md_Citas.GetValue("U_Num_Serie", i) & "-" & md_Citas.GetValue("U_NumCita", i)
                                Exit For

                            End If
                        End If
                    Next
                End If
            Else
                l_strCita = String.Empty
            End If

            Return l_strCita

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub ValidarSuspension(ByVal FormUID As String, _
                             ByVal pVal As SAPbouiCOM.ItemEvent, _
                             ByRef BubbleEvent As Boolean)
        Try
            Dim l_strFhaDesde As String
            Dim l_strFhaHasta As String
            Dim l_strHoraDesde As String
            Dim l_strHoraHasta As String

            Dim l_strCodeSucur As String
            Dim l_strCodeAgenda As String
            Dim blnResult As Boolean = False
            Dim l_strSQLConfig As String
            Dim l_strEstado As String = "Y"
            Dim l_strObserv As String

            Dim FechaDesde As Date
            Dim FechaHasta As Date

            Dim l_FhaDesdeTmp As String
            'Dim l_FhaHastaTmp As String
            Dim l_HoraDesdeTmp As String
            Dim l_HoraHastaTmp As String
            Dim l_horaInicioSuc As String
            Dim l_horaFinalSuc As String

            Dim l_HoraCitaDesde As String
            Dim l_horaCitaHasta As String


            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData


            l_strSQLConfig = " SELECT U_Sucurs, U_HoraInicio, U_HoraFin, U_UsaDurEC, U_CodCitaCancel FROM [@SCGD_CONF_SUCURSAL] " & _
                               " WHERE U_Sucurs = '{0}'"

            l_strFhaDesde = EditTextFechaDesde.ObtieneValorDataSource()
            l_strFhaHasta = EditTextFechaHasta.ObtieneValorDataSource()
            l_strHoraDesde = EditTextHoraDesde.ObtieneValorDataSource()
            l_strHoraHasta = EditTextHoraHasta.ObtieneValorDataSource()
            l_strCodeSucur = EditCboSucursal.ObtieneValorDataSource()
            l_strCodeAgenda = EditCboAgenda.ObtieneValorDataSource()
            l_strObserv = EditTextObserv.ObtieneValorDataSource()

            FechaDesde = DateTime.ParseExact(l_strFhaDesde, "yyyyMMdd", Nothing) ' FormatDateTime(l_strFhaDesde, DateFormat.ShortDate) 
            FechaHasta = DateTime.ParseExact(l_strFhaHasta, "yyyyMMdd", Nothing)

            l_HoraCitaDesde = l_strHoraDesde
            l_horaCitaHasta = l_strHoraHasta


            l_strSQLConfig = String.Format(l_strSQLConfig, l_strCodeSucur)
            md_Configuracion.Clear()
            md_Configuracion.ExecuteQuery(l_strSQLConfig)

            If md_Configuracion.Rows.Count > 0 Then
                l_horaInicioSuc = md_Configuracion.GetValue("U_HoraInicio", 0)
                l_horaFinalSuc = md_Configuracion.GetValue("U_HoraFin", 0)
            End If

            If FechaDesde <> FechaHasta Then
                While FechaDesde < FechaHasta

                    l_HoraDesdeTmp = l_strHoraDesde
                    l_HoraHastaTmp = l_horaFinalSuc

                    oCompanyService = m_oCompany.GetCompanyService()
                    oGeneralService = oCompanyService.GetGeneralService("SCGD_AgnSusp")

                    oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
                    oGeneralData.SetProperty("U_Cod_Sucur", l_strCodeSucur)
                    oGeneralData.SetProperty("U_Cod_Agenda", l_strCodeAgenda)
                    oGeneralData.SetProperty("U_Fha_Desde", FechaDesde)
                    oGeneralData.SetProperty("U_Hora_Desde", Convert.ToDateTime(Utilitarios.FormatoHora(l_HoraDesdeTmp)))
                    oGeneralData.SetProperty("U_Fha_Hasta", Utilitarios.RetornaFechaFormatoDB(FechaDesde, m_oCompany.Server))
                    oGeneralData.SetProperty("U_Hora_Hasta", Convert.ToDateTime(Utilitarios.FormatoHora(l_HoraHastaTmp)))
                    oGeneralData.SetProperty("U_Estado", l_strEstado)
                    oGeneralData.SetProperty("U_Observ", l_strObserv)

                    oGeneralService.Add(oGeneralData)

                    FechaDesde = FechaDesde.AddDays(1)

                    If FechaDesde = FechaHasta Then
                        ' l_strFhaDesde = FechaDesde
                        l_strHoraDesde = l_horaInicioSuc
                        ' l_strFhaHasta = FechaHasta
                        ' l_strHoraHasta = l_horaCitaHasta

                        EditTextFechaDesde.AsignaValorDataSource(FechaDesde.ToString("yyyyMMdd"))
                        EditTextHoraDesde.AsignaValorDataSource(l_horaInicioSuc)
                        EditTextFechaHasta.AsignaValorDataSource(FechaDesde.ToString("yyyyMMdd"))
                        EditTextHoraHasta.AsignaValorDataSource(l_horaCitaHasta)
                    Else
                        '  l_strFhaDesde = FechaDesde
                        l_strHoraDesde = l_horaInicioSuc
                        '  l_strFhaHasta = FechaDesde
                        '  l_strHoraHasta = l_horaFinalSuc
                    End If
                End While
            End If



        Catch ex As Exception
            Throw ex
        End Try
    End Sub


#End Region

    <DllImport("user32.dll", SetLastError:=True)> _
    Private Shared Function GetForegroundWindow() As IntPtr
    End Function


    Private Sub _frmAgenda_eFechaYHoraSeleccionada(ByVal p_dtFechaYHora As Date, ByVal p_strNombreAgenda As String, ByVal p_intCodigoAgenda As Integer) Handles _frmAgendaCitas.eFechaYHoraSeleccionada

        Dim sboItem As Item
        Dim sboEdit As EditText
        Dim fechaCita As String
        Dim horaCita As String
        Dim minutosCita As String
        ' Dim fhaCierreTaller As DateTime

        fechaCita = p_dtFechaYHora.ToString("yyyyMMdd")
        ' fhaCierreTaller = DateTime.Parse(p_dtFechaYHora.ToString("yyyy/MM/dd") & " " & Utilitarios.FormatoHora(m_HoraCierreTaller))

        horaCita = p_dtFechaYHora.ToString("HH")
        minutosCita = p_dtFechaYHora.ToString("mm")
        horaCita = horaCita & minutosCita

        If m_blnUsaAgenda1 Then
            EditTextFechaDesde.AsignaValorDataSource(fechaCita)
            EditTextHoraDesde.AsignaValorDataSource(horaCita)

        ElseIf m_blnUsaAgenda1 = False Then
            EditTextFechaHasta.AsignaValorDataSource(fechaCita)
            EditTextHoraHasta.AsignaValorDataSource(horaCita)

        End If

        _frmAgendaCitas.Close()
        _frmAgendaCitas = Nothing

    End Sub

    Private Sub _frmAgenda_eListaSuspecionesAgenda(ByVal p_ListaSuspencion As List(Of frmCalendario.Reservacion), ByVal p_strNombreAgenda As String, ByVal p_intCodigoAgenda As Integer) Handles _frmAgendaCitas.eListaSuspecionesAgenda
        Try
            Dim l_numFila As Integer
            Dim l_fhaSusp As Date
            Dim l_fhaDesde As Date
            Dim l_fhaHasta As Date
            Dim l_strHoraDesde As String
            Dim l_strHoraHasta As String


            md_Suspension.Rows.Clear()
            'md_Suspension = FormularioSBO.DataSources.DataTables.Add("TablaSusp")

            m_ListaSuspencion = p_ListaSuspencion

            For i As Integer = 0 To m_ListaSuspencion.Count - 1

                l_fhaDesde = m_ListaSuspencion(i).fhaDesde
                l_fhaHasta = m_ListaSuspencion(i).fhaHasta

                l_fhaSusp = Date.Parse(l_fhaDesde)
                l_strHoraDesde = Format(l_fhaDesde, "HH:mm")
                l_strHoraHasta = Format(l_fhaHasta, "HH:mm")

                If md_Suspension.Rows.Count = 0 Then
                    l_numFila = 0
                Else
                    l_numFila = md_Suspension.Rows.Count
                End If

                md_Suspension.Rows.Add()
                md_Suspension.SetValue("fhaSusp", l_numFila, l_fhaSusp)
                md_Suspension.SetValue("HraDesde", l_numFila, l_strHoraDesde)
                md_Suspension.SetValue("HraHasta", l_numFila, l_strHoraHasta)


            Next

            MatrizSusp.Matrix.LoadFromDataSource()

            _frmAgendaCitas.Close()
            _frmAgendaCitas = Nothing

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub _frmAgendaColor_eFechaYHoraSeleccionadaColor(ByVal p_dtFechaYHora As Date, ByVal p_strNombreAgenda As String, ByVal p_intCodigoAgenda As Integer) Handles _frmAgendaCitasColor.eFechaYHoraSeleccionadaColor

        Dim sboItem As Item
        Dim sboEdit As EditText
        Dim fechaCita As String
        Dim horaCita As String
        Dim minutosCita As String
        ' Dim fhaCierreTaller As DateTime

        fechaCita = p_dtFechaYHora.ToString("yyyyMMdd")
        ' fhaCierreTaller = DateTime.Parse(p_dtFechaYHora.ToString("yyyy/MM/dd") & " " & Utilitarios.FormatoHora(m_HoraCierreTaller))

        horaCita = p_dtFechaYHora.ToString("HH")
        minutosCita = p_dtFechaYHora.ToString("mm")
        horaCita = horaCita & minutosCita

        If m_blnUsaAgenda1 Then
            EditTextFechaDesde.AsignaValorDataSource(fechaCita)
            EditTextHoraDesde.AsignaValorDataSource(horaCita)

        ElseIf m_blnUsaAgenda1 = False Then
            EditTextFechaHasta.AsignaValorDataSource(fechaCita)
            EditTextHoraHasta.AsignaValorDataSource(horaCita)

        End If

        _frmAgendaCitasColor.Close()
        _frmAgendaCitasColor = Nothing

    End Sub

    Private Sub _frmAgendaColor_eListaSuspecionesAgendaColor(ByVal p_ListaSuspencionColor As List(Of frmCalendarioColor.Reservacion), ByVal p_strNombreAgenda As String, ByVal p_intCodigoAgenda As Integer) Handles _frmAgendaCitasColor.eListaSuspecionesAgendaColor
        Try
            Dim l_numFila As Integer
            Dim l_fhaSusp As Date
            Dim l_fhaDesde As Date
            Dim l_fhaHasta As Date
            Dim l_strHoraDesde As String
            Dim l_strHoraHasta As String


            md_Suspension.Rows.Clear()

            m_ListaSuspencionColor = p_ListaSuspencionColor

            For i As Integer = 0 To m_ListaSuspencion.Count - 1

                l_fhaDesde = m_ListaSuspencion(i).fhaDesde
                l_fhaHasta = m_ListaSuspencion(i).fhaHasta

                l_fhaSusp = Date.Parse(l_fhaDesde)
                l_strHoraDesde = Format(l_fhaDesde, "HH:mm")
                l_strHoraHasta = Format(l_fhaHasta, "HH:mm")

                If md_Suspension.Rows.Count = 0 Then
                    l_numFila = 0
                Else
                    l_numFila = md_Suspension.Rows.Count
                End If

                md_Suspension.Rows.Add()
                md_Suspension.SetValue("fhaSusp", l_numFila, l_fhaSusp)
                md_Suspension.SetValue("HraDesde", l_numFila, l_strHoraDesde)
                md_Suspension.SetValue("HraHasta", l_numFila, l_strHoraHasta)


            Next

            MatrizSusp.Matrix.LoadFromDataSource()

            _frmAgendaCitas.Close()
            _frmAgendaCitas = Nothing

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

End Class
