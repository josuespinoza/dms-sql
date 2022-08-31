
'*******************************************
'*Maneja los controles del formulario para incluir repuestos en la OT
'*******************************************

Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class IncluirRepuestosOT : Implements IFormularioSBO, IUsaMenu

#Region "Declaraciones"

    'maneja informacion de la aplicacion
    Private _applicationSbo As Application
    'maneja informacion de la compania 
    Private _companySbo As ICompany
    Private _formType As String
    Private _formularioSBO As SAPbouiCOM.IForm
    Private _inicializado As Boolean

    'propiedades
    Private _nombreXml As String
    Private _titulo As String
    Private _idMenu As String
    Private _menuPadre As String
    Private _nombre As String
    Private _posicion As Integer

    Private Shared _codeCliente As String
    Private Shared _noOt As String
    Private Shared _Sucursal As String
    'valores cotizacion
    Private Shared _strMoneda As String
    Private Shared _dcTCCot As Decimal
    Private Shared _strFechaCot As String
    
    'matriz
    Private MatrizRepuestosOT As MatrizRepuestosOT

    'lista a eliminar
    Private lsListaEliminar As Generic.IList(Of Integer) = New Generic.List(Of Integer)


    'datatable
    Dim dtRepuestos As DataTable
    Dim dtLocal As DataTable
    Dim dtBusqueda As DataTable

    'Nombre de datatable 
    Private strDTRepuestos = "tRepuestos"
    Private strDTLocal = "tLocal"

    'formulario
    Dim m_oFormularioSeleccionaRepuestos As SeleccionarRepuestosOT
    Dim oGestorFormularios As GestorFormularios

    Dim UDS_IncluyeRepuestos As UserDataSources

    Dim txtNoOrden As EditTextSBO
    Dim txtNoUni As EditTextSBO
    Dim txtTiOr As EditTextSBO
    Dim txtEsOT As EditTextSBO
    Dim txtMarca As EditTextSBO
    Dim txtEstilo As EditTextSBO
    Dim txtModelo As EditTextSBO
    Dim txtNoCono As EditTextSBO
    Dim txtNoVin As EditTextSBO
    Dim txtKim As EditTextSBO
    Dim txtPlaca As EditTextSBO
    Dim txtDocE As EditTextSBO
    Dim txtCodCli As EditTextSBO
    Dim txtMonCot As EditTextSBO
    Dim txtTCCot As EditTextSBO
    Dim txtFechaCot As EditTextSBO

    Dim n As NumberFormatInfo

    Dim strAdicionalesAprobadosSuc As String

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="application">Objeto application</param>
    ''' <param name="companySbo">Objeto Company</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_strMenuIncluirRepOT As String)
        Dim strCadenaConexionBDTaller As String
        _companySbo = companySbo
        _applicationSbo = application
        'manejador de formulario
        'oGestorFormularios = New GestorFormularios(ApplicationSBO)
        Utilitarios.DevuelveCadenaConexionBDTaller(ApplicationSBO, strCadenaConexionBDTaller)
        objTransferenciaStock = New TransferenciaItems(ApplicationSBO, companySbo, strCadenaConexionBDTaller)
        n = DIHelper.GetNumberFormatInfo(_companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLIncluirRepuestosOT
        MenuPadre = "43540"
        Nombre = My.Resources.Resource.TituloIncluirRepOT
        IdMenu = p_strMenuIncluirRepOT
        Posicion = 6
        FormType = p_strMenuIncluirRepOT
    End Sub

#End Region

#Region "Propiedades"
    'Manejo de propiedades para la aplicacion

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
            Return _formularioSBO
        End Get
        Set(ByVal value As SAPbouiCOM.IForm)
            _formularioSBO = value
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
            Return _idMenu
        End Get
        Set(ByVal value As String)
            _idMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements SCG.SBOFramework.UI.IUsaMenu.MenuPadre
        Get
            Return _menuPadre
        End Get
        Set(ByVal value As String)
            _menuPadre = value
        End Set
    End Property

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _posicion
        End Get
        Set(ByVal value As Integer)
            _posicion = value
        End Set
    End Property

    Public Shared Property CodeCliente As String
        Get
            Return _codeCliente
        End Get
        Set(ByVal value As String)
            _codeCliente = value
        End Set
    End Property

    Public Shared Property NoOT As String
        Get
            Return _noOt
        End Get
        Set(ByVal value As String)
            _noOt = value
        End Set
    End Property

    Public Shared Property Sucursal As String
        Get
            Return _Sucursal
        End Get
        Set(ByVal value As String)
            _Sucursal = value
        End Set
    End Property

    Public Shared Property strMoneda As String
        Get
            Return _strMoneda
        End Get
        Set(ByVal value As String)
            _strMoneda = value
        End Set
    End Property

    Public Shared Property dcTCCot As Decimal
        Get
            Return _dcTCCot
        End Get
        Set(ByVal value As Decimal)
            _dcTCCot = value
        End Set
    End Property

    Public Shared Property strFechaCot As String
        Get
            Return _strFechaCot
        End Get
        Set(ByVal value As String)
            _strFechaCot = value
        End Set
    End Property

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Metodo para inicializacion del formulario
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

        Dim oItem As SAPbouiCOM.Item

        If Not FormularioSBO Is Nothing Then
            FormularioSBO.Freeze(True)
            CargarFormularioIncluirRepuestos()
            FormularioSBO.Freeze(False)
        End If
    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles

    End Sub

    ''' <summary>
    ''' Método que realiza la carga del formulario
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarFormularioIncluirRepuestos()

        Try
            'modificacion para botones de agregar y eliminar repuestos
            FormularioSBO.Items.Item("btnAdd").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            FormularioSBO.Items.Item("btnDel").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            'manejo de opciones de navegacion para el formulario
            FormularioSBO.EnableMenu("1288", False)
            FormularioSBO.EnableMenu("1289", False)
            FormularioSBO.EnableMenu("1290", False)
            FormularioSBO.EnableMenu("1291", False)

            'asocia componentes de interfaz
            UDS_IncluyeRepuestos = FormularioSBO.DataSources.UserDataSources
            UDS_IncluyeRepuestos.Add("NoOrden", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("NoUni", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("TiOr", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("EsOT", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("Marca", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("Estilo", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("Modelo", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("NoCono", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("NoVin", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("Kim", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("Placa", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("DocE", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("CodCli", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("Moneda", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("TipoCam", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("FechaCot", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("LineNum", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("Compra", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeRepuestos.Add("IdEsT", BoDataType.dt_LONG_TEXT, 100)

            txtNoOrden = New EditTextSBO("txtNoOrden", True, "", "NoOrden", FormularioSBO)
            txtNoUni = New EditTextSBO("txtNoUni", True, "", "NoUni", FormularioSBO)
            txtTiOr = New EditTextSBO("txtTiOr", True, "", "TiOr", FormularioSBO)
            txtEsOT = New EditTextSBO("txtEsOT", True, "", "EsOT", FormularioSBO)
            txtMarca = New EditTextSBO("txtMarca", True, "", "Marca", FormularioSBO)
            txtEstilo = New EditTextSBO("txtEstilo", True, "", "Estilo", FormularioSBO)
            txtModelo = New EditTextSBO("txtModelo", True, "", "Modelo", FormularioSBO)
            txtNoCono = New EditTextSBO("txtNoCono", True, "", "NoCono", FormularioSBO)
            txtNoVin = New EditTextSBO("txtNoVIN", True, "", "NoVin", FormularioSBO)
            txtKim = New EditTextSBO("txtKim", True, "", "Kim", FormularioSBO)
            txtPlaca = New EditTextSBO("txtPlaca", True, "", "Placa", FormularioSBO)
            txtDocE = New EditTextSBO("txtDocE", True, "", "DocE", FormularioSBO)
            txtCodCli = New EditTextSBO("txtCodCli", True, "", "CodCli", FormularioSBO)
            txtMonCot = New EditTextSBO("txtMonCot", True, "", "Moneda", FormularioSBO)
            txtTCCot = New EditTextSBO("txtTCCot", True, "", "TipoCam", FormularioSBO)
            txtFechaCot = New EditTextSBO("txtFechaC", True, "", "FechaCot", FormularioSBO)
            txtFechaCot = New EditTextSBO("txtFechaC", True, "", "FechaCot", FormularioSBO)

            txtNoOrden.AsignaBinding()
            txtNoUni.AsignaBinding()
            txtTiOr.AsignaBinding()
            txtEsOT.AsignaBinding()
            txtMarca.AsignaBinding()
            txtEstilo.AsignaBinding()
            txtModelo.AsignaBinding()
            txtNoCono.AsignaBinding()
            txtNoVin.AsignaBinding()
            txtKim.AsignaBinding()
            txtPlaca.AsignaBinding()
            txtDocE.AsignaBinding()
            txtCodCli.AsignaBinding()
            txtMonCot.AsignaBinding()
            txtTCCot.AsignaBinding()
            txtFechaCot.AsignaBinding()

            'datatable que es la matriz de tramites
            dtRepuestos = FormularioSBO.DataSources.DataTables.Add(strDTRepuestos)
            dtRepuestos.Columns.Add("sel", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("per", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("des", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("can", BoFieldsType.ft_Quantity, 100)
            dtRepuestos.Columns.Add("mon", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("pre", BoFieldsType.ft_Price, 100)
            dtRepuestos.Columns.Add("apr", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("tra", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("ln", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("com", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("rec", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("sol", BoFieldsType.ft_AlphaNumeric, 100)

            'crea matriz
            MatrizRepuestosOT = New MatrizRepuestosOT("mtxRep", FormularioSBO, strDTRepuestos)
            MatrizRepuestosOT.CreaColumnas()
            MatrizRepuestosOT.LigaColumnas()

            MatrizRepuestosOT.Matrix.Columns.Item("Col_sel").Editable = True
            MatrizRepuestosOT.Matrix.Columns.Item("Col_pre").Editable = DMS_Connector.Configuracion.ParamGenAddon.U_RepPre = "Y"
            strAdicionalesAprobadosSuc = DMS_Connector.Configuracion.ParamGenAddon.U_AdcApr

            dtLocal = FormularioSBO.DataSources.DataTables.Add("dtConsulta")
            dtLocal = FormularioSBO.DataSources.DataTables.Add("tLocal")
            dtAprobado = FormularioSBO.DataSources.DataTables.Add("tAprobado")
            dtTrasladado = FormularioSBO.DataSources.DataTables.Add("tTrasladado")
            dtBusqueda = FormularioSBO.DataSources.DataTables.Add(g_strdtBusqueda)

            FormularioSBO.Items.Item("btnAct").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el formulario de repuestos
    ''' </summary>
    ''' <remarks></remarks>
    'Private Sub CargarFormularioSeleccionRepuestos()

    '    m_oFormularioSeleccionaRepuestos = New SeleccionarRepuestosOT(ApplicationSBO, CompanySBO, )

    '    m_oFormularioSeleccionaRepuestos.NombreXml = System.Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLSeleccionaRepuestosOT
    '    m_oFormularioSeleccionaRepuestos.Titulo = My.Resources.Resource.TituloFormularioSeleccionRepuestos
    '    m_oFormularioSeleccionaRepuestos.FormType = "SCGD_SROT"
    '    m_oFormularioSeleccionaRepuestos.p_strSucursal = g_strSucursal
    'CodeCliente = txtCodCli.ObtieneValorUserDataSource()
    'NoOT = txtNoOrden.ObtieneValorUserDataSource()

    '    If Not oGestorFormularios.FormularioAbierto(m_oFormularioSeleccionaRepuestos, activarSiEstaAbierto:=True) Then

    '        m_oFormularioSeleccionaRepuestos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioSeleccionaRepuestos)

    '    End If

    'End Sub
    ''' <summary>
    ''' Valida Estado de la OT
    ''' </summary>
    ''' <remarks></remarks>
    'Private Sub ManejarEstadoBotones()
    '    Dim intEstado As Integer
    '    If Not UDS_IncluyeRepuestos.Item("IdEsT").Value = "1" AndAlso Not UDS_IncluyeRepuestos.Item("IdEsT").Value = "2" Then
    '        intEstado = 0
    '    Else
    '        intEstado = 1
    '    End If
    '    FormularioSBO.Items.Item("btnAdd").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, intEstado)
    '    FormularioSBO.Items.Item("btnDel").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, intEstado)
    '    FormularioSBO.Items.Item("btnAct").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, intEstado)
    '    FormularioSBO.Items.Item("btnDesA").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, intEstado)
    'End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Manejo de eventos
    ''' </summary>
    ''' <param name="FormUID">Identificador de formulario</param>
    ''' <param name="pVal">Objeto de tipo ItemEvent</param>
    ''' <param name="BubbleEvent">Objeto Burbuja</param>
    ''' <remarks></remarks>
    Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.FormTypeEx <> FormType Then Exit Sub

        If pVal.EventType <> BoEventTypes.et_FORM_ACTIVATE Then
            Select Case pVal.EventType
                Case BoEventTypes.et_ITEM_PRESSED
                    ManejadorEventosItemPressed(FormUID, pVal, BubbleEvent)
                Case BoEventTypes.et_MATRIX_LOAD
                    ManejadorEventosMatrixLoad(FormUID, pVal, BubbleEvent)
                Case BoEventTypes.et_CHOOSE_FROM_LIST
                    ManejadorEventosChooseFromList(FormUID, pVal, BubbleEvent)
            End Select
        End If

    End Sub

    ''' <summary>
    ''' manejo de los eventos de tipo Item
    ''' </summary>
    ''' <param name="FormUID">Identificador del formulario</param>
    ''' <param name="pVal">Objeto ItemEvent</param>
    ''' <param name="BubbleEvent">Evento burbuja para la aplicacion</param>
    ''' <remarks></remarks>
    Private Sub ManejadorEventosItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oForm As IForm
        Dim oMatrix As Matrix
        Dim intResultMsj As Integer
        Dim intResultMsj2 As Integer


        Try
            oForm = ApplicationSBO.Forms.Item(FormUID)

            If pVal.BeforeAction = True Then
                Select Case pVal.ItemUID
                    Case "btnBus"

                        If ValidaCambios() Then

                            intResultMsj2 = ApplicationSBO.MessageBox(My.Resources.Resource.MensajeNoGuardaraCambios, 1,
                                                          My.Resources.Resource.Si,
                                                          My.Resources.Resource.No,
                                                          My.Resources.Resource.btnCancelar)
                            If intResultMsj2 <> 1 Then
                                BubbleEvent = False
                            End If
                        End If
                    Case "btnAct"
                        intResultMsj = ApplicationSBO.MessageBox(My.Resources.Resource.MensajeActualizacionCotizacion, 1,
                                                      My.Resources.Resource.Si,
                                                      My.Resources.Resource.No,
                                                      My.Resources.Resource.btnCancelar)
                        If intResultMsj <> 1 Then
                            BubbleEvent = False
                        End If
                End Select
            ElseIf pVal.ActionSuccess = True Then

                Select Case pVal.ItemUID
                    'Case "btnAdd"
                    '    CargarFormularioSeleccionRepuestos()
                    Case "btnDel"
                        oForm.Freeze(True)
                        EliminarRepuestosSeleccionados(FormUID)
                        oForm.Freeze(False)
                    Case "btnBus"
                        ManejadorBotonBuscar()
                    Case "btnAct"
                        ActualizaCotizacion(FormUID)
                    Case "mtxRep"
                        oForm.Freeze(True)
                        If pVal.ColUID = "Col_sel" And pVal.Row > 0 Then

                            dtRepuestos = oForm.DataSources.DataTables.Item(strDTRepuestos)
                            oMatrix = DirectCast(oForm.Items.Item("mtxRep").Specific, Matrix)
                            oMatrix.FlushToDataSource()

                            If pVal.Row <= dtRepuestos.Rows.Count Then
                                If dtRepuestos.GetValue("sel", pVal.Row - 1) = "Y" Then
                                    lsListaEliminar.Add(pVal.Row)
                                End If
                            End If
                        End If
                        oForm.Freeze(False)
                    Case "btnDesA"
                        oForm.Freeze(True)
                        If DesaprobarRepuestosOT(FormUID) Then
                            ActualizaCotizacion(FormUID, True)
                        End If
                        oForm.Freeze(False)
                End Select
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub ManejadorBotonBuscar()
        Dim DocEntryCotizacion As String = String.Empty
        Dim NumeroOT As String = String.Empty
        Try
            FormularioSBO.Freeze(True)
            NumeroOT = FormularioSBO.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).Trim()
            DocEntryCotizacion = ObtenerDocEntryCotizacion(NumeroOT)
            If Not String.IsNullOrEmpty(DocEntryCotizacion) Then
                BuscarCotizacion(DocEntryCotizacion)
            End If
            FormularioSBO.Freeze(False)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Function ObtenerDocEntryCotizacion(ByVal NumeroOT As String) As String
        Dim Query As String = "SELECT U_DocEntry FROM ""@SCGD_OT"" WITH (nolock) WHERE Code = '{0}' "
        Dim DocEntry As String = String.Empty
        Try
            If String.IsNullOrEmpty(NumeroOT) Then
                Return String.Empty
            Else
                Query = String.Format(Query, NumeroOT)
                DocEntry = DMS_Connector.Helpers.EjecutarConsulta(Query)
                Return DocEntry
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Maneja eventos de tipo carga de matriz
    ''' </summary>
    ''' <param name="FormUID">Identificador del formulario</param>
    ''' <param name="pVal">objeto ItemEvent</param>
    ''' <param name="BubbleEvent">Evento burbuja para la aplicacion</param>
    ''' <remarks></remarks>
    Private Sub ManejadorEventosMatrixLoad(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByVal BubbleEvent As Boolean)
        If pVal.BeforeAction Then
        ElseIf pVal.ActionSuccess Then
            'CargaRepuestos()
        End If
    End Sub

#End Region

    Private Sub ManejadorEventosChooseFromList(p_FormUID As String, pVal As SAPbouiCOM.ItemEvent, BubbleEvent As Boolean)
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim Respuesta As Integer
        Try
            FormularioSBO.Freeze(True)
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

            If oCFLEvento.ChooseFromListUID = "cflOT" Then
                If pVal.BeforeAction Then
                    oConditions = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                    oCondition = oConditions.Add
                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "Code"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                    oCondition.BracketCloseNum = 1
                    oCondition.Relationship = BoConditionRelationship.cr_AND

                    oCondition = oConditions.Add
                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "U_EstO"
                    oCondition.Operation = BoConditionOperation.co_BETWEEN
                    oCondition.CondVal = 1
                    oCondition.CondEndVal = 4
                    oCondition.BracketCloseNum = 1

                    oCFL.SetConditions(oConditions)
                    If ValidaCambios() Then
                        Respuesta = ApplicationSBO.MessageBox(My.Resources.Resource.MensajeNoGuardaraCambios, 1, My.Resources.Resource.Si, My.Resources.Resource.No, My.Resources.Resource.btnCancelar)
                        If Respuesta <> 1 Then
                            'Se detiene el procesamiento del evento
                            BubbleEvent = False
                        End If
                    End If
                Else
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        BuscarCotizacion(oCFLEvento.SelectedObjects.GetValue("U_DocEntry", 0))
                        FormularioSBO.DataSources.DBDataSources.Item("OQUT").SetValue("U_SCGD_Numero_OT", 0, oCFLEvento.SelectedObjects.GetValue("Code", 0))
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            FormularioSBO.Freeze(False)
        End Try
    End Sub


End Class
