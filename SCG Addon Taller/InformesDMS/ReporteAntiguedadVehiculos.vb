Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany
Imports DMSOneFramework

Public Class ReporteAntiguedadVehiculos : Implements IUsaMenu, IFormularioSBO, IUsaPermisos

#Region "Declaraciones"
    'General
    Private m_oCompany As SAPbobsCOM.Company
    Private m_SBO_Application As Application

    'Connection
    Private m_strConectionString As String
    Dim m_cn_Coneccion As New SqlClient.SqlConnection

    'Variables
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
    Private _txtFromDate As SCG.SBOFramework.UI.EditTextSBO
    Private _txtToDate As SCG.SBOFramework.UI.EditTextSBO
    Private _rbtnDetallado As OptionBtnSBO
    Private _rbtnResumido As OptionBtnSBO
    Private _chbDate As CheckBoxSBO
    Private chkBoxFecha As CheckBoxSBO
    Private chkBoxTodo As CheckBoxSBO
    Private chkBoxTodoInv As CheckBoxSBO
    Private g_mtxSucursales As MatrizRptOrdenesXEstado
    Private g_mtxUbicaciones As MatrizRptOrdenesXEstado
    Private g_mtxTipoInventario As MatrizRptOrdenesXEstado
    Private _udsFormulario As UserDataSources
    Private _btnPrint As SCG.SBOFramework.UI.ButtonSBO
    Private _btnCancel As SCG.SBOFramework.UI.ButtonSBO
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon

#End Region

#Region "Propiedades"
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
        Set(value As String)
            _FormType = value
        End Set
    End Property

    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _FormularioSBO
        End Get
        Set(value As SAPbouiCOM.IForm)
            _FormularioSBO = value
        End Set
    End Property

    Public Property Inicializado As Boolean Implements SCG.SBOFramework.UI.IFormularioSBO.Inicializado
        Get
            Return _Inicializado
        End Get
        Set(value As Boolean)
            _Inicializado = value
        End Set
    End Property

    

    Public Property NombreXml As String Implements SCG.SBOFramework.UI.IFormularioSBO.NombreXml
        Get
            Return _NombreXML
        End Get
        Set(value As String)
            _NombreXML = value
        End Set
    End Property

    Public Property Titulo As String Implements SCG.SBOFramework.UI.IFormularioSBO.Titulo
        Get
            Return _Titulo
        End Get
        Set(value As String)
            _Titulo = value
        End Set
    End Property

    Public Property IdMenu As String Implements SCG.SBOFramework.UI.IUsaMenu.IdMenu
        Get
            Return _IdMenu
        End Get
        Set(value As String)
            _IdMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements SCG.SBOFramework.UI.IUsaMenu.MenuPadre
        Get
            Return _MenuPadre
        End Get
        Set(value As String)
            _MenuPadre = value
        End Set
    End Property

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _Nombre
        End Get
        Set(value As String)
            _Nombre = value
        End Set
    End Property

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _Posicion
        End Get
        Set(value As Integer)
            _Posicion = value
        End Set
    End Property

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



#End Region

#Region "Constructor"
    <CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application, ByVal p_menuInformesDMS As String, ByVal p_strUID As String)
        m_oCompany = ocompany
        m_SBO_Application = SBOAplication
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioReporteAntiguedadVehiculos
        MenuPadre = p_menuInformesDMS
        Nombre = My.Resources.Resource.MenuReporteAntiguedadVehiculos
        IdMenu = p_strUID
        Titulo = My.Resources.Resource.MenuReporteAntiguedadVehiculos
        Posicion = 14
        FormType = p_strUID
    End Sub
#End Region

#Region "Metodos"
    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles

        Try
            _udsFormulario = FormularioSBO.DataSources.UserDataSources
            '
            _udsFormulario.Add("chkHist", BoDataType.dt_LONG_TEXT, 50)
            _udsFormulario.Add("txtFrom", BoDataType.dt_DATE, 50)
            _udsFormulario.Add("txtTo", BoDataType.dt_DATE, 50)
            _udsFormulario.Add("chkTodo", BoDataType.dt_LONG_TEXT, 50)
            _udsFormulario.Add("chkTInv", BoDataType.dt_LONG_TEXT, 50)

            CargarMatrizUbicacion()
            CargarMatrizTipoInventario()

            'Checkbox utilizar fecha historica
            chkBoxFecha = New CheckBoxSBO("chkHist", True, "", "chkHist", FormularioSBO)
            chkBoxFecha.AsignaBinding()
            chkBoxFecha.AsignaValorUserDataSource("Y")

            'Checkbox utilizado para seleccionar todas las ubicaciones al mismo tiempo
            chkBoxTodo = New CheckBoxSBO("chkTodo", True, "", "chkTodo", FormularioSBO)
            chkBoxTodo.AsignaBinding()
            chkBoxTodo.AsignaValorUserDataSource("N")

            'Checkbox utilizado para seleccionar todas los tipos de inventario al mismo tiempo
            chkBoxTodoInv = New CheckBoxSBO("chkTInv", True, "", "chkTInv", FormularioSBO)
            chkBoxTodoInv.AsignaBinding()
            chkBoxTodoInv.AsignaValorUserDataSource("N")

            'Campo de texto "Fecha Desde"
            _txtFromDate = New SCG.SBOFramework.UI.EditTextSBO("txtFrom", True, "", "txtFrom", FormularioSBO)
            _txtFromDate.AsignaBinding()
            _txtFromDate.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            'Campo de texto "Fecha Hasta"
            _txtToDate = New SCG.SBOFramework.UI.EditTextSBO("txtTo", True, "", "txtTo", FormularioSBO)
            _txtToDate.AsignaBinding()
            _txtToDate.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            'Boton Imprimir reporte
            _btnPrint = New SCG.SBOFramework.UI.ButtonSBO("btn_Print", FormularioSBO)
            'Boton Cancelar
            _btnCancel = New SCG.SBOFramework.UI.ButtonSBO("2", FormularioSBO)



            'Deshabilita el menú superior
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

    Public Sub ApplicationSBOItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.FormTypeEx = _FormType Then

                If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then
                    ManejadorEventoItemPress(FormUID, pVal, BubbleEvent)
                End If
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub ManejadorEventoItemPress(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.Before_Action Then
                Select Case pVal.ItemUID
                    Case g_mtxUbicaciones.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case g_mtxTipoInventario.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case _btnPrint.UniqueId
                        ValidaDatos(formUID, pVal, BubbleEvent)
                    Case chkBoxFecha.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case chkBoxTodo.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                    Case chkBoxTodoInv.UniqueId
                        ManejadorEventoCheckBox(formUID, pVal, BubbleEvent)
                End Select
            ElseIf pVal.ActionSuccess Then
                If pVal.ItemUID = _btnPrint.UniqueId Then
                    CargarParametros(formUID, pVal, BubbleEvent)
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Private Sub ValidaDatos(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim oDataTable As DataTable
        Dim blnUbicacionSeleccionada As Boolean = False
        Dim blnTipoInventarioSeleccionado As Boolean = False

        Try
            'Valida que se haya seleccionado una ubicación
            oMatriz = DirectCast(_FormularioSBO.Items.Item("mtxLoc").Specific, SAPbouiCOM.Matrix)
            oDataTable = FormularioSBO.DataSources.DataTables.Item("dataTableUbicaciones")

            For i As Integer = 0 To oMatriz.RowCount - 1

                If oDataTable.GetValue("sel", i) = "Y" Then
                    blnUbicacionSeleccionada = True
                    Exit For
                End If
            Next

            If blnUbicacionSeleccionada = False Then
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorSeleccionarUbicacion, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                BubbleEvent = False
                Exit Sub
            End If

            'Valida que se haya seleccionado un tipo de inventario
            oMatriz = DirectCast(_FormularioSBO.Items.Item("mtxTip").Specific, SAPbouiCOM.Matrix)
            oDataTable = FormularioSBO.DataSources.DataTables.Item("dataTableTipoInv")

            For i As Integer = 0 To oMatriz.RowCount - 1

                If oDataTable.GetValue("sel", i) = "Y" Then
                    blnTipoInventarioSeleccionado = True
                    Exit For
                End If
            Next

            If blnTipoInventarioSeleccionado = False Then
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorSeleccionarTipoInventario, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                BubbleEvent = False
                Exit Sub
            End If

            'Valida que se haya escogido una fecha
            If chkBoxFecha.ObtieneValorUserDataSource() = "N" Then
                If String.IsNullOrEmpty(_txtFromDate.ObtieneValorUserDataSource()) Then
                    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.RptOTxEValidaFechaInicio, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                    BubbleEvent = False
                    Exit Sub
                ElseIf String.IsNullOrEmpty(_txtToDate.ObtieneValorUserDataSource()) Then
                    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.RptOTxEValidaFechaLimite, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                    BubbleEvent = False
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
        


    End Sub

    Public Sub CargarParametros(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim oDataTable As DataTable
        Dim oMatrizTipoInventario As SAPbouiCOM.Matrix
        Dim oDataTableTipoInventario As DataTable
        Dim fechaInicial As String
        Dim fechaFinal As String
        'Dim WhereSucursales As String
        'Dim WhereTiposOT As String
        Dim WhereUbicaciones As String
        Dim WhereTipoInventario As String
        Dim WhereCompleto As String = String.Empty
        'Dim sucursales As String = " OQUT.U_SCGD_idSucursal = '{0}' "
        'Dim tiposOT As String = " OQUT.U_SCGD_Tipo_OT = '{0}' "
        Dim strUbicacion As String = " T0.U_Cod_Ubic = '{0}' "
        Dim strTipoInventario As String = " T0.U_Tipo = '{0}' "
        Dim parametros As String
        Dim m_strOR As String = " OR "
        Dim blnExisteAgregado As Boolean = False

        Try
            If chkBoxFecha.ObtieneValorUserDataSource() = "N" Then
                fechaInicial = Date.ParseExact(_txtFromDate.ObtieneValorUserDataSource(), "yyyyMMdd", Nothing)
                fechaFinal = Date.ParseExact(_txtToDate.ObtieneValorUserDataSource(), "yyyyMMdd", Nothing)
                fechaInicial = Utilitarios.RetornaFechaFormatoDB(fechaInicial, m_oCompany.Server, False)
                fechaFinal = Utilitarios.RetornaFechaFormatoDB(fechaFinal, m_oCompany.Server, False)
            Else
                fechaInicial = Utilitarios.RetornaFechaFormatoDB(Date.ParseExact("18900101", "yyyyMMdd", Nothing), m_oCompany.Server, False)
                fechaFinal = DateTime.Now.Year
                If DateTime.Now.Month.ToString.Length = 1 Then fechaFinal = fechaFinal & "0" & DateTime.Now.Month Else fechaFinal = fechaFinal & DateTime.Now.Month
                If DateTime.Now.Day.ToString.Length = 1 Then fechaFinal = fechaFinal & "0" & DateTime.Now.Day Else fechaFinal = fechaFinal & DateTime.Now.Day
                fechaFinal = Utilitarios.RetornaFechaFormatoDB(Date.ParseExact(fechaFinal, "yyyyMMdd", Nothing), m_oCompany.Server, False)
            End If
            oMatriz = DirectCast(_FormularioSBO.Items.Item("mtxLoc").Specific, SAPbouiCOM.Matrix)
            oDataTable = FormularioSBO.DataSources.DataTables.Item("dataTableUbicaciones")

            oMatrizTipoInventario = DirectCast(_FormularioSBO.Items.Item("mtxTip").Specific, SAPbouiCOM.Matrix)
            oDataTableTipoInventario = FormularioSBO.DataSources.DataTables.Item("dataTableTipoInv")

            blnExisteAgregado = False

            'Creamos el condicional Where
            For i As Integer = 0 To oMatriz.RowCount - 1
                If oDataTable.GetValue("sel", i) = "Y" Then
                    If blnExisteAgregado Then WhereUbicaciones = WhereUbicaciones + m_strOR
                    WhereUbicaciones = WhereUbicaciones + String.Format(strUbicacion, oDataTable.GetValue("cod", i))
                    blnExisteAgregado = True
                End If
            Next

            blnExisteAgregado = False

            For i As Integer = 0 To oMatrizTipoInventario.RowCount - 1
                If oDataTableTipoInventario.GetValue("sel", i) = "Y" Then
                    If blnExisteAgregado Then WhereTipoInventario = WhereTipoInventario + m_strOR
                    WhereTipoInventario = WhereTipoInventario + String.Format(strTipoInventario, oDataTableTipoInventario.GetValue("cod", i))
                    blnExisteAgregado = True
                End If
            Next

            If Not String.IsNullOrEmpty(WhereUbicaciones) Then
                WhereUbicaciones = String.Format(" ({0}) ", WhereUbicaciones)
            End If

            If Not String.IsNullOrEmpty(WhereTipoInventario) Then
                WhereTipoInventario = String.Format(" ({0}) ", WhereTipoInventario)
            End If

            If Not String.IsNullOrEmpty(WhereUbicaciones) And Not String.IsNullOrEmpty(WhereTipoInventario) Then
                WhereCompleto = String.Format(" {0} AND {1} ", WhereUbicaciones, WhereTipoInventario)
            Else
                WhereCompleto = String.Format(" {0} {1} ", WhereUbicaciones, WhereTipoInventario)
            End If

            parametros = String.Format(" {0}, {1}, {2}", fechaInicial, WhereCompleto, fechaFinal)

            Call Print(My.Resources.Resource.rptAntiguedadVehiculos & ".rpt", My.Resources.Resource.MenuReporteAntiguedadVehiculos, parametros)




        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    Public Sub ManejadorEventoCheckBox(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oMatriz As SAPbouiCOM.Matrix
        Dim oDataTable As DataTable

        Try
            'Marca/desmarca las casillas de selección de la matriz ubicación
            If pVal.ColUID = "Col_sel" And pVal.ItemUID = "mtxLoc" Then
                oMatriz = DirectCast(_FormularioSBO.Items.Item("mtxLoc").Specific, SAPbouiCOM.Matrix)
                If pVal.Row <= oMatriz.RowCount Then
                    oDataTable = FormularioSBO.DataSources.DataTables.Item("dataTableUbicaciones")
                    If oDataTable.GetValue("sel", pVal.Row - 1) = "N" Then
                        oDataTable.SetValue("sel", pVal.Row - 1, "Y")
                    Else
                        oDataTable.SetValue("sel", pVal.Row - 1, "N")
                    End If
                End If
            End If

            'Marca/desmarca las casillas de selección de la matriz tipo de inventario
            If pVal.ColUID = "Col_sel" And pVal.ItemUID = "mtxTip" Then
                oMatriz = DirectCast(_FormularioSBO.Items.Item("mtxTip").Specific, SAPbouiCOM.Matrix)
                If pVal.Row <= oMatriz.RowCount Then
                    oDataTable = FormularioSBO.DataSources.DataTables.Item("dataTableTipoInv")
                    If oDataTable.GetValue("sel", pVal.Row - 1) = "N" Then
                        oDataTable.SetValue("sel", pVal.Row - 1, "Y")
                    Else
                        oDataTable.SetValue("sel", pVal.Row - 1, "N")
                    End If
                End If
            End If

            If pVal.ItemUID = chkBoxFecha.UniqueId Then
                If chkBoxFecha.ObtieneValorUserDataSource = "Y" Then
                    'Al marcar el check, no se utilizan fechas, por lo que los campos Fecha Desde y Fecha Hasta se ponen en blanco
                    _txtFromDate.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    _txtToDate.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    _txtFromDate.AsignaValorUserDataSource("")
                    _txtToDate.AsignaValorUserDataSource("")

                Else
                    'Al desmarcar el check, se le asigna la fecha actual a los campos de texto Fecha Desde y Fecha Hasta
                    _txtFromDate.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    _txtToDate.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    Dim fechaHasta As String = DateTime.Now.Year
                    If DateTime.Now.Month.ToString.Length = 1 Then fechaHasta = fechaHasta & "0" & DateTime.Now.Month Else fechaHasta = fechaHasta & DateTime.Now.Month
                    If DateTime.Now.Day.ToString.Length = 1 Then fechaHasta = fechaHasta & "0" & DateTime.Now.Day Else fechaHasta = fechaHasta & DateTime.Now.Day
                    _txtFromDate.AsignaValorUserDataSource(fechaHasta)
                    _txtToDate.AsignaValorUserDataSource(fechaHasta)
                End If
            End If

            'Marca/Desmarca todas las casillas de selección de sucursal
            If pVal.ItemUID = chkBoxTodo.UniqueId Then
                oMatriz = DirectCast(_FormularioSBO.Items.Item("mtxLoc").Specific, SAPbouiCOM.Matrix)
                oDataTable = FormularioSBO.DataSources.DataTables.Item("dataTableUbicaciones")

                For i As Integer = 0 To oMatriz.RowCount - 1
                    oDataTable.SetValue("sel", i, chkBoxTodo.ObtieneValorUserDataSource())
                Next

                oMatriz.LoadFromDataSource()
            End If

            'Marca/Desmarca todas las casillas de selección de tipo de inventario
            If pVal.ItemUID = chkBoxTodoInv.UniqueId Then
                oMatriz = DirectCast(_FormularioSBO.Items.Item("mtxTip").Specific, SAPbouiCOM.Matrix)
                oDataTable = FormularioSBO.DataSources.DataTables.Item("dataTableTipoInv")

                For i As Integer = 0 To oMatriz.RowCount - 1
                    oDataTable.SetValue("sel", i, chkBoxTodoInv.ObtieneValorUserDataSource())
                Next

                oMatriz.LoadFromDataSource()
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub





    Private Sub CargarMatrizUbicacion()
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim oDataTable As DataTable
        Dim strQuery As String = String.Empty

        Try
            strQuery = "SELECT 'N' AS 'sel', T0.""Code"" AS 'cod', T0.""Name"" AS 'des' FROM ""@SCGD_UBICACIONES""  T0 Order By T0.""Name"""
            oMatriz = DirectCast(_FormularioSBO.Items.Item("mtxLoc").Specific, SAPbouiCOM.Matrix)
            oDataTable = _FormularioSBO.DataSources.DataTables.Add("dataTableUbicaciones")
            oDataTable.Columns.Add("sel", BoFieldsType.ft_AlphaNumeric)
            oDataTable.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric)
            oDataTable.Columns.Add("des", BoFieldsType.ft_AlphaNumeric)

            g_mtxUbicaciones = New MatrizRptOrdenesXEstado("mtxLoc", FormularioSBO, "dataTableUbicaciones")
            g_mtxUbicaciones.CreaColumnas()
            g_mtxUbicaciones.LigaColumnas()

            oDataTable.ExecuteQuery(strQuery)
            oMatriz.LoadFromDataSource()


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    Private Sub CargarMatrizTipoInventario()
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim oDataTable As DataTable
        Dim strQuery As String = String.Empty

        Try
            strQuery = "SELECT 'N' AS 'sel', T0.""Code"" AS 'cod', T0.""Name"" AS 'des' FROM ""@SCGD_TIPOVEHICULO""  T0 Order By T0.""Name"""
            oMatriz = DirectCast(_FormularioSBO.Items.Item("mtxTip").Specific, SAPbouiCOM.Matrix)
            oDataTable = _FormularioSBO.DataSources.DataTables.Add("dataTableTipoInv")
            oDataTable.Columns.Add("sel", BoFieldsType.ft_AlphaNumeric)
            oDataTable.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric)
            oDataTable.Columns.Add("des", BoFieldsType.ft_AlphaNumeric)

            g_mtxTipoInventario = New MatrizRptOrdenesXEstado("mtxTip", FormularioSBO, "dataTableTipoInv")
            g_mtxTipoInventario.CreaColumnas()
            g_mtxTipoInventario.LigaColumnas()

            oDataTable.ExecuteQuery(strQuery)
            oMatriz.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

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
