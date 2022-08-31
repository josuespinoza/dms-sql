﻿Imports System.Globalization
Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany
Imports DMSOneFramework

Public Class OrdenesDeTrabajoPorEstado : Implements IUsaMenu, IFormularioSBO, IUsaPermisos

#Region "Declaraciones"

    'General
    Private m_oCompany As SAPbobsCOM.Company
    Private m_SBO_Application As Application

    Public n As NumberFormatInfo
    
    'ObjDataTable 
    Private _dt As DataTable
    
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

    Private _chbDate As CheckBoxSBO
    Private _chbALLT As CheckBoxSBO
    Private _chbALLE As CheckBoxSBO
    Private _chbALLS As CheckBoxSBO

    Private g_mtxSucursales As MatrizRptOrdenesXEstado
    Private g_mtxEstado As MatrizRptOrdenesXEstado
    Private g_mtxTipoOT As MatrizRptOrdenesXEstado

    Private _rbtnDetallado As OptionBtnSBO
    Private _rbtnResumido As OptionBtnSBO

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
                   ByVal SBOAplication As Application, p_menuInformesDMS As String, p_strUID_FORM_OrdenesTrabajoEstado As String)
        m_oCompany = ocompany
        m_SBO_Application = SBOAplication
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioReporteOrdenesDeTrabajoPorEstado
        MenuPadre = p_menuInformesDMS
        Nombre = My.Resources.Resource.MenuOrdenesdeTrabajoPorEstado
        IdMenu = p_strUID_FORM_OrdenesTrabajoEstado
        Titulo = My.Resources.Resource.MenuOrdenesdeTrabajoPorEstado
        Posicion = 7
        FormType = p_strUID_FORM_OrdenesTrabajoEstado
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
            _udsFormulario.Add("chb_ALLE", BoDataType.dt_LONG_TEXT, 50)
            _udsFormulario.Add("chb_ALLS", BoDataType.dt_LONG_TEXT, 50)
            _udsFormulario.Add("txt_DateS", BoDataType.dt_DATE, 50)
            _udsFormulario.Add("txt_DateF", BoDataType.dt_DATE, 50)
            _udsFormulario.Add("rbtnResu", BoDataType.dt_LONG_TEXT, 150)
            _udsFormulario.Add("rbtnDet", BoDataType.dt_LONG_TEXT, 150)

            CargarMatrixSucursales()
            CargarMatrixEstados()
            CargarMatrixTipo()

            _chbDate = New CheckBoxSBO("chb_Date", True, "", "chb_Date", FormularioSBO)
            _chbDate.AsignaBinding()
            _chbDate.AsignaValorUserDataSource("Y")

            _chbALLT = New CheckBoxSBO("chb_ALLT", True, "", "chb_ALLT", FormularioSBO)
            _chbALLT.AsignaBinding()
            _chbALLT.AsignaValorUserDataSource("N")

            _chbALLE = New CheckBoxSBO("chb_ALLE", True, "", "chb_ALLE", FormularioSBO)
            _chbALLE.AsignaBinding()
            _chbALLE.AsignaValorUserDataSource("N")

            _chbALLS = New CheckBoxSBO("chb_ALLS", True, "", "chb_ALLS", FormularioSBO)
            _chbALLS.AsignaBinding()
            _chbALLS.AsignaValorUserDataSource("N")


            _txtDateS = New SCG.SBOFramework.UI.EditTextSBO("txt_DateS", True, "", "txt_DateS", FormularioSBO)
            _txtDateS.AsignaBinding()

            _txtDateF = New SCG.SBOFramework.UI.EditTextSBO("txt_DateF", True, "", "txt_DateF", FormularioSBO)
            _txtDateF.AsignaBinding()

            _rbtnDetallado = New OptionBtnSBO("rbtnDet", True, "", "rbtnDet", FormularioSBO)
            _rbtnDetallado.AsignaBinding()
            _rbtnDetallado.AsignaValorUserDataSource("N")
            _rbtnResumido = New OptionBtnSBO("rbtnResu", True, "", "rbtnResu", FormularioSBO)
            _rbtnResumido.AsignaBinding()
            _rbtnResumido.AsignaValorUserDataSource("N")

            _txtDateF.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            _txtDateS.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            _btnPrint = New SCG.SBOFramework.UI.ButtonSBO("btn_Print", FormularioSBO)
            _btnCancel = New SCG.SBOFramework.UI.ButtonSBO("2", FormularioSBO)

            _FormularioSBO.EnableMenu("1281", False)
            _FormularioSBO.EnableMenu("1282", False)
            _FormularioSBO.EnableMenu("1283", False)
            _FormularioSBO.EnableMenu("1284", False)
            _FormularioSBO.EnableMenu("1285", False)
            _FormularioSBO.EnableMenu("1286", False)
            _FormularioSBO.EnableMenu("1287", False)
            _FormularioSBO.EnableMenu("1288", False)

            FormularioSBO.Freeze(False)

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


    Private Sub CargarMatrixEstados()
        Dim oMatriz As SAPbouiCOM.Matrix
        oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Est").Specific, SAPbouiCOM.Matrix)

        Dim table As DataTable = _FormularioSBO.DataSources.DataTables.Add("dtEst")
        table.Columns.Add("sel", BoFieldsType.ft_AlphaNumeric)
        table.Columns.Add("des", BoFieldsType.ft_AlphaNumeric)
        table.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric)

        g_mtxEstado = New MatrizRptOrdenesXEstado("mtx_Est", FormularioSBO, "dtEst")
        g_mtxEstado.CreaColumnas()
        g_mtxEstado.LigaColumnas()

        table.ExecuteQuery("SELECT 'N' as sel, Name as des, Code as cod FROM [@SCGD_ESTADOS_OT]")

        oMatriz.LoadFromDataSource()
        oMatriz.Columns.Item("Col_sel").Editable = True

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

        table.ExecuteQuery("SELECT 'N' as sel, Name as des, Code as cod FROM [@SCGD_TIPO_ORDEN]")

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


            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
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
                    Case _chbALLS.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case _chbALLE.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case _chbALLT.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case _chbDate.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case _btnPrint.UniqueId
                        ValidaDatos(formUID, pVal, BubbleEvent)
                    Case g_mtxSucursales.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case g_mtxEstado.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case g_mtxTipoOT.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case _rbtnResumido.UniqueId
                        ManejadorEventoRadioButton(formUID, pVal, BubbleEvent)
                    Case _rbtnDetallado.UniqueId
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
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim oDT As DataTable
        Dim bool As Boolean = False
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

        oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Est").Specific, SAPbouiCOM.Matrix)
        oDT = FormularioSBO.DataSources.DataTables.Item("dtEst")

        For i As Integer = 0 To oMatriz.RowCount - 1
            If oDT.GetValue("sel", i) = "Y" Then
                bool = False
                Exit For
            Else
                bool = True
            End If
        Next
        If bool Then
            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.RptOTxEValidaEstado, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
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
        Dim WhereEstados As String
        Dim WhereTiposOT As String
        Dim sucursales As String = " OQ.U_SCGD_idSucursal = '{0}' "
        Dim estados As String = " ES.Code = '{0}' "
        Dim tiposOT As String = " OQ.U_SCGD_Tipo_OT = '{0}' "
        Dim parametros As String
        Dim m_strOR As String = " OR "

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

        oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Est").Specific, SAPbouiCOM.Matrix)
        oDT = FormularioSBO.DataSources.DataTables.Item("dtEst")

        For i As Integer = 0 To oMatriz.RowCount - 1

            If oDT.GetValue("sel", i) = "Y" Then
                If m_blnExisteAgregado Then WhereEstados = WhereEstados + m_strOR
                WhereEstados = WhereEstados + String.Format(estados, oDT.GetValue("cod", i))
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


        If _rbtnResumido.ObtieneValorUserDataSource() = "Y" Then
            parametros = String.Format("( {0} ),{1},{2},( {3} ),( {4} )", WhereEstados, fechaF, fechaI, WhereSucursales, WhereTiposOT)
            Call Print(My.Resources.Resource.rptOTxEstadoResumido, My.Resources.Resource.TituloOTResumido, parametros)
        ElseIf _rbtnDetallado.ObtieneValorUserDataSource() = "Y" Then
            parametros = String.Format("( {0} ),{1},{2},( {3} ),( {4} )", WhereEstados, fechaF, fechaI, WhereSucursales, WhereTiposOT)
            Call Print(My.Resources.Resource.rptOTEstadoDetallado, My.Resources.Resource.TituloOTDetallado, parametros)
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


            Case _chbALLE.UniqueId
                oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Est").Specific, SAPbouiCOM.Matrix)
                oDT = FormularioSBO.DataSources.DataTables.Item("dtEst")

                For i As Integer = 0 To oMatriz.RowCount - 1
                    oDT.SetValue("sel", i, _chbALLE.ObtieneValorUserDataSource())
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
                    _txtDateS.AsignaValorUserDataSource("20000101")
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

            Case g_mtxEstado.UniqueId
                If pVal.Row - 1 >= 0 And pVal.ColUID = "Col_sel" Then
                    oMatriz = DirectCast(_FormularioSBO.Items.Item("mtx_Est").Specific, SAPbouiCOM.Matrix)
                    If pVal.Row <= oMatriz.RowCount Then
                        oDT = FormularioSBO.DataSources.DataTables.Item("dtEst")
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
        End Select

    End Sub

#End Region
End Class
