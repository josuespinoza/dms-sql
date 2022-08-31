Imports DMS_Addon.ControlesSBO
Imports System.Globalization
Imports SAPbouiCOM
Imports ICompany = SAPbobsCOM.ICompany
Imports SCG.SBOFramework.UI
Imports DMSOneFramework

Public Class ReporteServiciosExternosXOrden : Implements IUsaMenu, IFormularioSBO, IUsaPermisos

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
    Private _txtNOT As SCG.SBOFramework.UI.EditTextSBO

    Private _chbDate As CheckBoxSBO
    Private _chbALLT As CheckBoxSBO
    Private _chbNOT As CheckBoxSBO
    Private _chbALLS As CheckBoxSBO

    Private _CFLNumeroOT As ChooseFromListSBO

    Private g_mtxSucursales As MatrizRptOrdenesXEstado

    Private g_mtxTipoOT As MatrizRptOrdenesXEstado

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
    <System.CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application, ByVal p_menuInformesDMS As String, ByVal p_strUID_FORM_ReporteServiciosExternosXOrden As String)
        m_oCompany = ocompany
        m_SBO_Application = SBOAplication
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioReporteSExOT
        MenuPadre = p_menuInformesDMS
        Nombre = My.Resources.Resource.MenuReporteSExOT
        IdMenu = p_strUID_FORM_ReporteServiciosExternosXOrden
        Titulo = My.Resources.Resource.MenuReporteSExOT
        Posicion = 11
        FormType = p_strUID_FORM_ReporteServiciosExternosXOrden
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

            _udsFormulario.Add("chb_Date", BoDataType.dt_LONG_TEXT, 50)
            _udsFormulario.Add("chb_ALLT", BoDataType.dt_LONG_TEXT, 50)
            _udsFormulario.Add("chb_NOT", BoDataType.dt_LONG_TEXT, 50)
            _udsFormulario.Add("chb_ALLS", BoDataType.dt_LONG_TEXT, 50)
            _udsFormulario.Add("txt_DateS", BoDataType.dt_DATE, 50)
            _udsFormulario.Add("txt_DateF", BoDataType.dt_DATE, 50)
            _udsFormulario.Add("txt_NOT", BoDataType.dt_LONG_TEXT, 50)


            CargarMatrixSucursales()

            CargarMatrixTipo()

            _chbNOT = New CheckBoxSBO("chb_OT", True, "", "chb_NOT", FormularioSBO)
            _chbNOT.AsignaBinding()
            _chbNOT.AsignaValorUserDataSource("N")

            _chbDate = New CheckBoxSBO("chb_Date", True, "", "chb_Date", FormularioSBO)
            _chbDate.AsignaBinding()
            _chbDate.AsignaValorUserDataSource("Y")

            _chbALLT = New CheckBoxSBO("chb_ALLT", True, "", "chb_ALLT", FormularioSBO)
            _chbALLT.AsignaBinding()
            _chbALLT.AsignaValorUserDataSource("N")

            _chbALLS = New CheckBoxSBO("chb_ALLS", True, "", "chb_ALLS", FormularioSBO)
            _chbALLS.AsignaBinding()
            _chbALLS.AsignaValorUserDataSource("N")

            _txtDateS = New SCG.SBOFramework.UI.EditTextSBO("txt_DateS", True, "", "txt_DateS", FormularioSBO)
            _txtDateS.AsignaBinding()

            _txtDateF = New SCG.SBOFramework.UI.EditTextSBO("txt_DateF", True, "", "txt_DateF", FormularioSBO)
            _txtDateF.AsignaBinding()

            _txtNOT = New SCG.SBOFramework.UI.EditTextSBO("txt_NOT", True, "", "txt_NOT", FormularioSBO)
            _txtNOT.AsignaBinding()

            _CFLNumeroOT = New ChooseFromListSBO("CFL_OT")

            _txtDateF.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            _txtDateS.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            _txtNOT.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            _btnPrint = New SCG.SBOFramework.UI.ButtonSBO("btn_Print", FormularioSBO)
            _btnCancel = New SCG.SBOFramework.UI.ButtonSBO("2", FormularioSBO)

            _FormularioSBO.EnableMenu("1281", False)
            _FormularioSBO.EnableMenu("1282", False)
            _FormularioSBO.EnableMenu("1283", False)
            _FormularioSBO.EnableMenu("1284", False)
            _FormularioSBO.EnableMenu("1285", False)
            _FormularioSBO.EnableMenu("1286", False)
            _FormularioSBO.EnableMenu("1287", False)

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
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim strTextNumeroOT As String
        Dim strNumeroOT As String = "U_SCGD_Numero_OT"


        Try
            If oCFLEvento.BeforeAction Then

                oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

                Select Case pval.ItemUID

                    Case _txtNOT.UniqueId

                        oConditions = m_SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = strNumeroOT
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                        oCondition.CondVal = Nothing
                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)

                End Select


            ElseIf oCFLEvento.ActionSuccess Then

                oDataTable = oCFLEvento.SelectedObjects
                If Not oDataTable Is Nothing Then
                    strTextNumeroOT = String.Format("{0}", oDataTable.GetValue(strNumeroOT, 0))
                    _txtNOT.AsignaValorUserDataSource(strTextNumeroOT)

                End If
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub





    Private Sub CargarMatrixSucursales()
        Dim oMatriz As SAPbouiCOM.Matrix
        oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Suc").Specific, SAPbouiCOM.Matrix)

        Dim table As DataTable = _FormularioSBO.DataSources.DataTables.Add("dtSucur")
        table.Columns.Add("sel", BoFieldsType.ft_AlphaNumeric)
        table.Columns.Add("des", BoFieldsType.ft_AlphaNumeric)
        table.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric)

        g_mtxSucursales = New MatrizRptOrdenesXEstado("mtx_Suc", FormularioSBO, "dtSucur")
        g_mtxSucursales.CreaColumnas()
        g_mtxSucursales.LigaColumnas()

        table.ExecuteQuery("SELECT 'N' as sel, Name as des, Code as cod FROM [@SCGD_SUCURSALES]")

        oMatriz.LoadFromDataSource()

    End Sub


    Private Sub CargarMatrixTipo()
        Dim oMatriz As SAPbouiCOM.Matrix
        oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Tip").Specific, SAPbouiCOM.Matrix)

        Dim table As DataTable = _FormularioSBO.DataSources.DataTables.Add("dtTip")
        table.Columns.Add("sel", BoFieldsType.ft_AlphaNumeric)
        table.Columns.Add("des", BoFieldsType.ft_AlphaNumeric)
        table.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric)

        g_mtxTipoOT = New MatrizRptOrdenesXEstado("mtx_Tip", FormularioSBO, "dtTip")
        g_mtxTipoOT.CreaColumnas()
        g_mtxTipoOT.LigaColumnas()

        table.ExecuteQuery(" SELECT 'N' as sel, Name as des, Code as cod FROM [@SCGD_TIPO_ORDEN] ")

        oMatriz.LoadFromDataSource()

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

    Public Sub ManejadorEventoItemPress(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.Before_Action Then

                Select Case pVal.ItemUID
                    Case _chbALLS.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case _chbALLT.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case _chbDate.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case _btnPrint.UniqueId
                        ValidaDatos(formUID, pVal, BubbleEvent)
                    Case g_mtxSucursales.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case g_mtxTipoOT.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case _chbNOT.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)


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
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim oDT As DataTable
        Dim bool As Boolean = False

        If _chbNOT.ObtieneValorUserDataSource() = "Y" Then
            If String.IsNullOrEmpty(_txtNOT.ObtieneValorUserDataSource()) Then
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.RptSExOTValidaOT, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                BubbleEvent = False
                Exit Sub
            Else
                BubbleEvent = True
                Exit Sub
            End If
        End If
        oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Suc").Specific, SAPbouiCOM.Matrix)
        oDT = FormularioSBO.DataSources.DataTables.Item("dtSucur")

        For i As Integer = 0 To oMatriz.RowCount - 1

            If oDT.GetValue("sel", i) = "Y" Then
                bool = False
                Exit For
            Else
                bool = True
            End If
        Next
        If bool Then
            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.RptOTxEValidaSucursal, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
            BubbleEvent = False
            Exit Sub
        End If

        oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Tip").Specific, SAPbouiCOM.Matrix)
        oDT = FormularioSBO.DataSources.DataTables.Item("dtTip")

        For i As Integer = 0 To oMatriz.RowCount - 1
            If oDT.GetValue("sel", i) = "Y" Then
                bool = False
                Exit For
            Else
                bool = True
            End If
        Next
        If bool Then
            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.RptOTxEValidaTipoOT, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
            BubbleEvent = False
            Exit Sub
        End If

        If _chbDate.ObtieneValorUserDataSource() = "N" Then
            If String.IsNullOrEmpty(_txtDateS.ObtieneValorUserDataSource()) Then
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.RptOTxEValidaFechaInicio, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                BubbleEvent = False
                Exit Sub
            ElseIf String.IsNullOrEmpty(_txtDateF.ObtieneValorUserDataSource()) Then
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.RptOTxEValidaFechaLimite, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                BubbleEvent = False
                Exit Sub
            End If
        End If


    End Sub

    Public Sub CargarParametros(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim oDT As DataTable
        Dim fechaI As String
        Dim fechaF As String
        Dim WhereSucursales As String
        Dim WhereTiposOT As String
        Dim sucursales As String = " OQUT.U_SCGD_idSucursal = '{0}' "
        Dim tiposOT As String = " OQUT.U_SCGD_Tipo_OT = '{0}' "
        Dim parametros As String
        Dim m_strOR As String = " OR "
        Dim strNOT As String
        Dim dtOTEspecifica As System.Data.DataTable

        If _chbNOT.ObtieneValorUserDataSource() = "Y" Then
            strNOT = String.Format("{0} ", _txtNOT.ObtieneValorUserDataSource())
            dtOTEspecifica = Utilitarios.EjecutarConsultaDataTable(String.Format(" SELECT U_SCGD_Fech_CreaOT, U_SCGD_idSucursal ,U_SCGD_Tipo_OT  FROM OQUT WITH (NOLOCK) WHERE U_SCGD_Numero_OT = '{0}' ", strNOT), m_oCompany.CompanyDB, m_oCompany.Server)
            If dtOTEspecifica.Rows.Count = 0 Then
                m_SBO_Application.StatusBar.SetText(String.Format("{0} {1}", My.Resources.Resource.RptNoOT, strNOT), SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                BubbleEvent = False
                Exit Sub
            End If
            strNOT = String.Format(" = '{0}' ", _txtNOT.ObtieneValorUserDataSource())
            fechaI = Utilitarios.RetornaFechaFormatoDB(dtOTEspecifica.Rows(0)("U_SCGD_Fech_CreaOT"), m_oCompany.Server, False)
            fechaF = fechaI
            WhereSucursales = String.Format(sucursales, dtOTEspecifica.Rows(0)("U_SCGD_idSucursal"))
            WhereTiposOT = String.Format(tiposOT, dtOTEspecifica.Rows(0)("U_SCGD_Tipo_OT"))
        Else
            strNOT = " <> '' "
            If _chbDate.ObtieneValorUserDataSource() = "N" Then
                fechaI = Date.ParseExact(_txtDateS.ObtieneValorUserDataSource(), "yyyyMMdd", Nothing)
                fechaF = Date.ParseExact(_txtDateF.ObtieneValorUserDataSource(), "yyyyMMdd", Nothing)
                fechaI = Utilitarios.RetornaFechaFormatoDB(fechaI, m_oCompany.Server, False)
                fechaF = Utilitarios.RetornaFechaFormatoDB(fechaF, m_oCompany.Server, False)
            Else
                fechaI = Utilitarios.RetornaFechaFormatoDB(Date.ParseExact("18900101", "yyyyMMdd", Nothing), m_oCompany.Server, False)
                fechaF = DateTime.Now.Year
                If DateTime.Now.Month.ToString.Length = 1 Then fechaF = fechaF & "0" & DateTime.Now.Month Else fechaF = fechaF & DateTime.Now.Month
                If DateTime.Now.Day.ToString.Length = 1 Then fechaF = fechaF & "0" & DateTime.Now.Day Else fechaF = fechaF & DateTime.Now.Day
                fechaF = Utilitarios.RetornaFechaFormatoDB(Date.ParseExact(fechaF, "yyyyMMdd", Nothing), m_oCompany.Server, False)
            End If

            oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Suc").Specific, SAPbouiCOM.Matrix)
            oDT = FormularioSBO.DataSources.DataTables.Item("dtSucur")

            Dim m_blnExisteAgregado As Boolean = False

            For i As Integer = 0 To oMatriz.RowCount - 1
                If oDT.GetValue("sel", i) = "Y" Then
                    If m_blnExisteAgregado Then WhereSucursales = WhereSucursales + m_strOR
                    WhereSucursales = WhereSucursales + String.Format(sucursales, oDT.GetValue("cod", i))
                    m_blnExisteAgregado = True
                End If
            Next

            m_blnExisteAgregado = False

            oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Tip").Specific, SAPbouiCOM.Matrix)
            oDT = FormularioSBO.DataSources.DataTables.Item("dtTip")

            For i As Integer = 0 To oMatriz.RowCount - 1

                If oDT.GetValue("sel", i) = "Y" Then
                    If m_blnExisteAgregado Then WhereTiposOT = WhereTiposOT + m_strOR
                    WhereTiposOT = WhereTiposOT + String.Format(tiposOT, oDT.GetValue("cod", i))
                    m_blnExisteAgregado = True
                End If

            Next
        End If
        parametros = String.Format(" {0},{1}, {2} ,( {3} ),( {4} )", fechaF, fechaI, strNOT, WhereSucursales, WhereTiposOT)

        Call Print(My.Resources.Resource.RptSExOT & ".rpt", My.Resources.Resource.MenuReporteSExOT, parametros)

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

    Public Sub ManejadorEventoCheckBox(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)


        Dim oMatriz As SAPbouiCOM.Matrix
        Dim oDT As DataTable

        Select Case pVal.ItemUID
            Case _chbALLS.UniqueId
                oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Suc").Specific, SAPbouiCOM.Matrix)
                oDT = FormularioSBO.DataSources.DataTables.Item("dtSucur")

                For i As Integer = 0 To oMatriz.RowCount - 1
                    oDT.SetValue("sel", i, _chbALLS.ObtieneValorUserDataSource())
                Next

                oMatriz.LoadFromDataSource()

            Case _chbALLT.UniqueId
                oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Tip").Specific, SAPbouiCOM.Matrix)
                oDT = FormularioSBO.DataSources.DataTables.Item("dtTip")

                For i As Integer = 0 To oMatriz.RowCount - 1
                    oDT.SetValue("sel", i, _chbALLT.ObtieneValorUserDataSource())
                Next

                oMatriz.LoadFromDataSource()

            Case _chbDate.UniqueId
                If _chbDate.ObtieneValorUserDataSource() = "Y" Then
                    _txtDateF.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    _txtDateS.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    _txtDateF.AsignaValorUserDataSource("")
                    _txtDateS.AsignaValorUserDataSource("")
                Else
                    _txtDateF.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    _txtDateS.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    _txtDateS.AsignaValorUserDataSource("18900101")
                    Dim fechaF As String = DateTime.Now.Year
                    If DateTime.Now.Month.ToString.Length = 1 Then fechaF = fechaF & "0" & DateTime.Now.Month Else fechaF = fechaF & DateTime.Now.Month
                    If DateTime.Now.Day.ToString.Length = 1 Then fechaF = fechaF & "0" & DateTime.Now.Day Else fechaF = fechaF & DateTime.Now.Day

                    _txtDateF.AsignaValorUserDataSource(fechaF)

                End If

            Case g_mtxSucursales.UniqueId

                If pVal.Row - 1 >= 0 And pVal.ColUID = "Col_sel" Then
                    oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Suc").Specific, SAPbouiCOM.Matrix)
                    If pVal.Row <= oMatriz.RowCount Then
                        oDT = FormularioSBO.DataSources.DataTables.Item("dtSucur")
                        Dim a As String = oDT.GetValue("sel", pVal.Row - 1)
                        If oDT.GetValue("sel", pVal.Row - 1) = "N" Then
                            oDT.SetValue("sel", pVal.Row - 1, "Y")
                        Else
                            oDT.SetValue("sel", pVal.Row - 1, "N")
                        End If
                    End If
                End If


            Case g_mtxTipoOT.UniqueId
                If pVal.Row - 1 >= 0 And pVal.ColUID = "Col_sel" Then
                    oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Tip").Specific, SAPbouiCOM.Matrix)
                    If pVal.Row <= oMatriz.RowCount Then
                        oDT = FormularioSBO.DataSources.DataTables.Item("dtTip")
                        If oDT.GetValue("sel", pVal.Row - 1) = "N" Then
                            oDT.SetValue("sel", pVal.Row - 1, "Y")
                        Else
                            oDT.SetValue("sel", pVal.Row - 1, "N")
                        End If
                    End If
                End If

            Case _chbNOT.UniqueId
                If _chbNOT.ObtieneValorUserDataSource() = "Y" Then
                    _txtNOT.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    _chbALLS.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    _chbALLT.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    _chbDate.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    _txtDateF.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    _txtDateS.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    g_mtxSucursales.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    g_mtxTipoOT.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    _chbALLS.AsignaValorUserDataSource("N")
                    _chbALLT.AsignaValorUserDataSource("N")
                    _chbDate.AsignaValorUserDataSource("Y")
                    _txtDateF.AsignaValorUserDataSource("")
                    _txtDateS.AsignaValorUserDataSource("")

                    oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Suc").Specific, SAPbouiCOM.Matrix)
                    oDT = FormularioSBO.DataSources.DataTables.Item("dtSucur")

                    For i As Integer = 0 To oMatriz.RowCount - 1
                        oDT.SetValue("sel", i, "N")
                    Next
                    oMatriz.LoadFromDataSource()

                    oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Tip").Specific, SAPbouiCOM.Matrix)
                    oDT = FormularioSBO.DataSources.DataTables.Item("dtTip")

                    For i As Integer = 0 To oMatriz.RowCount - 1
                        oDT.SetValue("sel", i, "N")
                    Next
                    oMatriz.LoadFromDataSource()
                Else
                    _txtNOT.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    g_mtxSucursales.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    g_mtxTipoOT.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    _chbALLS.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    _chbALLT.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    _chbDate.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    _txtNOT.AsignaValorUserDataSource("")
                End If

        End Select

    End Sub

#End Region
End Class
