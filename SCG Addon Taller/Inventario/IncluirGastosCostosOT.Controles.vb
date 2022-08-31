Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class IncluirGastosCostosOT : Implements IUsaMenu, IFormularioSBO

#Region "Declaraciones"

    Private _applicationSbo As Application

    Private _companySbo As ICompany
    Private _formType As String
    Private _formularioSBO As SAPbouiCOM.IForm
    Private _inicializado As Boolean

    Private _nombreXml As String
    Private _titulo As String
    Private _idMenu As String
    Private _menuPadre As String
    Private _nombre As String
    Private _posicion As Integer

    Private Shared _codeCliente As String

    Private Shared _strMoneda As String
    Private Shared _dcTCCot As Decimal
    Private Shared _strFechaCot As String
    Private m_blnActualizaCot As Boolean = False

    Private MatrizGastosOT As MatrizGastosOT

    Private lsListaEliminar As Generic.IList(Of Integer) = New Generic.List(Of Integer)


    'datatable
    Dim dtGastos As DataTable
    Dim dtLocal As DataTable
    Dim dtBusqueda As DataTable

    'datatable
    Dim dtAprobado As SAPbouiCOM.DataTable
    Dim g_strdtBusqueda As String = "tBusqueda"
    Dim _blnCambio As Boolean


    'Nombre de datatable 
    Private strDTGastos = "tGastos"
    Private strDTLocal = "tLocal"



    'formulario
    Public m_oFormularioSeleccionaGastos As SeleccionarGastosCostosOT
    Public m_oFormularioCrearDocumentos As CrearDocumentosGastosCostos

    'Impuesto para Gastos/Costos
    Dim m_strImpuestosRepuestos

    Dim oGestorFormularios As GestorFormularios

    Dim UDS_IncluyeGastos As UserDataSources
    Dim n As NumberFormatInfo

    Public Shared txtNoOrden As EditTextSBO
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


#End Region

#Region "Constructor"

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_strMenuIncluirGastosOT As String)
        _companySbo = companySbo
        _applicationSbo = application
        'manejador de formulario
        oGestorFormularios = New GestorFormularios(_applicationSbo)
        n = DIHelper.GetNumberFormatInfo(_companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLIncluirGastosOT
        MenuPadre = "43540"
        Nombre = My.Resources.Resource.TituloIncluirGastosOT
        IdMenu = p_strMenuIncluirGastosOT
        Posicion = 7
        FormType = p_strMenuIncluirGastosOT
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

    Private Sub CargarFormularioGastos()
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
            UDS_IncluyeGastos = FormularioSBO.DataSources.UserDataSources
            UDS_IncluyeGastos.Add("NoOrden", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("NoUni", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("TiOr", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("EsOT", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("Marca", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("Estilo", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("Modelo", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("NoCono", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("NoVin", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("Kim", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("Placa", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("DocE", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("CodCli", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("Moneda", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("TipoCam", BoDataType.dt_LONG_TEXT, 100)
            UDS_IncluyeGastos.Add("FechaCot", BoDataType.dt_LONG_TEXT, 100)

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
            dtGastos = FormularioSBO.DataSources.DataTables.Add(strDTGastos)
            dtGastos.Columns.Add("sel", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastos.Columns.Add("per", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastos.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastos.Columns.Add("des", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastos.Columns.Add("can", BoFieldsType.ft_Quantity, 100)
            dtGastos.Columns.Add("mon", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastos.Columns.Add("pre", BoFieldsType.ft_Price, 100)
            dtGastos.Columns.Add("cos", BoFieldsType.ft_Price, 100)
            dtGastos.Columns.Add("apr", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastos.Columns.Add("asi", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastos.Columns.Add("fac", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastos.Columns.Add("imp", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastos.Columns.Add("lnum", BoFieldsType.ft_AlphaNumeric, 100)

            'crea matriz
            MatrizGastosOT = New MatrizGastosOT("mtxGas", FormularioSBO, strDTGastos)
            MatrizGastosOT.CreaColumnas()
            MatrizGastosOT.LigaColumnas()

            MatrizGastosOT.Matrix.Columns.Item("Col_sel").Editable = True

            If Utilitarios.EjecutarConsulta(" Select U_RepPre from [@SCGD_ADMIN] ", CompanySBO.CompanyDB, CompanySBO.Server).Trim() = "Y" Then
                MatrizGastosOT.Matrix.Columns.Item("Col_pre").Editable = True
            Else
                MatrizGastosOT.Matrix.Columns.Item("Col_pre").Editable = False
            End If

            dtLocal = FormularioSBO.DataSources.DataTables.Add("tLocal")
            dtAprobado = FormularioSBO.DataSources.DataTables.Add("tAprobado")
            dtBusqueda = FormularioSBO.DataSources.DataTables.Add(g_strdtBusqueda)

            'FormularioSBO.Items.Item("1").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            FormularioSBO.Items.Item("btnDoc").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            CargaValoresUDF(FormularioSBO)
            'FormularioSBO.Items.Item("btnAdd").Enabled = False
            'FormularioSBO.Items.Item("btnDel").Enabled = False

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    ''' <summary>
    ''' Valida Estado de la OT
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ManejarEstadoBotones()
        Dim intEstado As Integer
        If Not UDS_IncluyeGastos.Item("EsOT").Value = "No iniciada" And Not UDS_IncluyeGastos.Item("EsOT").Value = "Proceso" Then
            intEstado = 0
        Else
            intEstado = 1
        End If
        FormularioSBO.Items.Item("btnAdd").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, intEstado)
        FormularioSBO.Items.Item("btnDel").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, intEstado)
    End Sub

#End Region

#Region "EVentos"
    Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.FormTypeEx <> FormType Then Exit Sub

        If pVal.EventType <> BoEventTypes.et_FORM_ACTIVATE Then
            Select Case pVal.EventType
                Case BoEventTypes.et_ITEM_PRESSED
                    ManejadorEventosItemPressed(FormUID, pVal, BubbleEvent)
                Case BoEventTypes.et_MATRIX_LOAD
                    ' ManejadorEventosMatrixLoad(FormUID, pVal, BubbleEvent)
            End Select
        End If

    End Sub

    Private Sub ManejadorEventosItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oForm As IForm
        Dim oMatrix As Matrix
        Dim intResultMsj As Integer
        Dim intResultMsj2 As Integer

        Try
            oForm = ApplicationSBO.Forms.Item(FormUID)

            If pVal.BeforeAction = True Then
                Select Case pVal.ItemUID
                    Case "btnBus", "2"

                        If ValidaCambios() Then

                            intResultMsj2 = ApplicationSBO.MessageBox(My.Resources.Resource.MensajeNoGuardaraCambios, 1,
                                                          My.Resources.Resource.Si,
                                                          My.Resources.Resource.No,
                                                          My.Resources.Resource.btnCancelar)
                            If intResultMsj2 <> 1 Then
                                BubbleEvent = False
                            End If
                        End If
                    Case "1"
                        intResultMsj = ApplicationSBO.MessageBox(My.Resources.Resource.MensajeActualizacionCotizacion, 1,
                                                      My.Resources.Resource.Si,
                                                      My.Resources.Resource.No,
                                                      My.Resources.Resource.btnCancelar)

                        If oForm.Mode = BoFormMode.fm_UPDATE_MODE OrElse
                            oForm.Mode = BoFormMode.fm_ADD_MODE Then
                            m_blnActualizaCot = True
                        End If

                        If intResultMsj <> 1 Then
                            BubbleEvent = False
                        End If
                    Case "btnDoc"
                        If Not ValidarDocumento(BubbleEvent) Then
                            BubbleEvent = False
                        ElseIf Not ValidarLineasDoc(BubbleEvent) Then
                            BubbleEvent = False
                        End If
                End Select
            ElseIf pVal.ActionSuccess = True Then

                Select Case pVal.ItemUID
                    Case "btnAdd"
                        CargarFormularioSeleccionGastos()

                    Case "btnDel"
                        oForm.Freeze(True)
                        EliminarGastosSeleccionados(FormUID)
                        oForm.Freeze(False)
                    Case "btnBus"
                        oForm.Freeze(True)

                        dtGastos = oForm.DataSources.DataTables.Item(strDTGastos)
                        dtGastos.Rows.Clear()

                        oMatrix = DirectCast(oForm.Items.Item("mtxGas").Specific, Matrix)
                        oMatrix.LoadFromDataSource()

                        BuscarCotizacion(FormUID)
                        ManejarEstadoBotones()

                        oForm.Freeze(False)
                    Case "1"

                        If m_blnActualizaCot Then
                            ActualizaCotizacion(FormUID)
                        End If

                    Case "mtxGas"
                        oForm.Freeze(True)
                        If pVal.ColUID = "Col_sel" Then
                            If pVal.Row - 1 <= dtGastos.Rows.Count - 1 Then
                                dtGastos = oForm.DataSources.DataTables.Item(strDTGastos)
                                oMatrix = DirectCast(oForm.Items.Item("mtxGas").Specific, Matrix)
                                oMatrix.FlushToDataSource()

                                If dtGastos.GetValue("sel", pVal.Row - 1) = "Y" Then
                                    lsListaEliminar.Add(pVal.Row)
                                End If

                                ManejaModoFormulario(pVal)

                            End If
                        End If
                        oForm.Freeze(False)
                    Case "btnDoc"

                        'oForm.Freeze(True)

                        CargarFormularioCrearDocumentos()
                        AgregaGastosDocumento(FormUID, False, BubbleEvent)

                        ' oForm.Close()
                        'oForm.Freeze(False)

                End Select
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub
#End Region

#Region "Metodos / Funciones"
    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario

        Dim oItem As SAPbouiCOM.Item

        If Not FormularioSBO Is Nothing Then
            FormularioSBO.Freeze(True)
            For Each oItem In FormularioSBO.Items

                oItem.AffectsFormMode = False

            Next
            CargarFormularioGastos()
            FormularioSBO.Freeze(False)
        End If

    End Sub

    Private Sub CargarFormularioSeleccionGastos()
        m_oFormularioSeleccionaGastos = New SeleccionarGastosCostosOT(ApplicationSBO, CompanySBO)

        m_oFormularioSeleccionaGastos.NombreXml = System.Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLSeleccionaGastosOT
        m_oFormularioSeleccionaGastos.Titulo = My.Resources.Resource.TituloFormularioSeleccionGastosCostos
        m_oFormularioSeleccionaGastos.FormType = "SCGD_SGOT"
        ' CodeCliente = txtCodCli.ObtieneValorUserDataSource()

        If Not oGestorFormularios.FormularioAbierto(m_oFormularioSeleccionaGastos, activarSiEstaAbierto:=True) Then
            m_oFormularioSeleccionaGastos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioSeleccionaGastos)
        End If

    End Sub

    Private Sub CargarFormularioCrearDocumentos()
        m_oFormularioCrearDocumentos = New CrearDocumentosGastosCostos(ApplicationSBO, CompanySBO)

        m_oFormularioCrearDocumentos.NombreXml = System.Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioCrearDocumentoGastos
        m_oFormularioCrearDocumentos.Titulo = My.Resources.Resource.TituloCrearDocumentosGastosCostos
        m_oFormularioCrearDocumentos.FormType = "SCGD_GenDoc"

        If Not oGestorFormularios.FormularioAbierto(m_oFormularioCrearDocumentos, activarSiEstaAbierto:=True) Then
            m_oFormularioCrearDocumentos.FormularioSBO = oGestorFormularios.CargaFormulario(m_oFormularioCrearDocumentos)
        End If

    End Sub

    Public Sub BuscarCotizacion(ByVal FormUID As String, Optional ByVal p_strNoOT As String = "")

        Dim strConsulta As String = "select U_SCGD_Numero_OT, U_SCGD_Cod_Unidad, U_SCGD_Tipo_OT, " +
            " U_SCGD_Estado_Cot, U_SCGD_Des_Marc, U_SCGD_Des_Esti, U_SCGD_Des_Mode, U_SCGD_Gorro_Veh, " +
            " U_SCGD_Num_VIN, U_SCGD_Kilometraje, U_SCGD_Num_Placa, DocEntry, CardCode, DocCur, DocRate, DocDate " +
            " from OQUT where U_SCGD_Numero_OT = '{0}' and U_SCGD_idSucursal = '{1}'"

        Dim strNumeroOT As String = ""
        Dim oForm As Form
        Dim strCodigo As String = String.Empty
        Dim strUsuario As String = String.Empty
        Dim strSucursalUsuario As String = String.Empty

        Try
            oForm = ApplicationSBO.Forms.Item(FormUID)

            strUsuario = ApplicationSBO.Company.UserName
            strSucursalUsuario = Utilitarios.EjecutarConsulta(
                String.Format("Select Branch from OUSR where USER_CODE = '{0}'", strUsuario),
                                                              CompanySBO.CompanyDB,
                                                              CompanySBO.Server)

            dtBusqueda = oForm.DataSources.DataTables.Item(g_strdtBusqueda)
            If String.IsNullOrEmpty(p_strNoOT) Then
                strNumeroOT = txtNoOrden.ObtieneValorUserDataSource()
            Else
                strNumeroOT = p_strNoOT
            End If

            If Not String.IsNullOrEmpty(strNumeroOT) Then
                strConsulta = String.Format(strConsulta, strNumeroOT.Trim(), strSucursalUsuario)

                dtBusqueda.Rows.Clear()
                dtBusqueda.ExecuteQuery(strConsulta)

                If Not String.IsNullOrEmpty(dtBusqueda.GetValue("U_SCGD_Numero_OT", 0).ToString()) Then

                    For i As Integer = 0 To dtBusqueda.Rows.Count - 1

                        UDS_IncluyeGastos.Item("NoOrden").Value = dtBusqueda.GetValue("U_SCGD_Numero_OT", i).ToString()
                        UDS_IncluyeGastos.Item("NoUni").Value = dtBusqueda.GetValue("U_SCGD_Cod_Unidad", i).ToString()
                        UDS_IncluyeGastos.Item("TiOr").Value = Utilitarios.EjecutarConsulta(String.Format("select name from [@SCGD_TIPO_ORDEN] where Code = '{0}'", dtBusqueda.GetValue("U_SCGD_Tipo_OT", i).ToString().Trim()),
                                                                                               CompanySBO.CompanyDB,
                                                                                               CompanySBO.Server)
                        UDS_IncluyeGastos.Item("EsOT").Value = dtBusqueda.GetValue("U_SCGD_Estado_Cot", i).ToString()
                        UDS_IncluyeGastos.Item("Marca").Value = dtBusqueda.GetValue("U_SCGD_Des_Marc", i).ToString()
                        UDS_IncluyeGastos.Item("Estilo").Value = dtBusqueda.GetValue("U_SCGD_Des_Esti", i).ToString()
                        UDS_IncluyeGastos.Item("Modelo").Value = dtBusqueda.GetValue("U_SCGD_Des_Mode", i).ToString()
                        UDS_IncluyeGastos.Item("NoCono").Value = dtBusqueda.GetValue("U_SCGD_Gorro_Veh", i).ToString()
                        UDS_IncluyeGastos.Item("NoVin").Value = dtBusqueda.GetValue("U_SCGD_Num_VIN", i).ToString()
                        UDS_IncluyeGastos.Item("Kim").Value = dtBusqueda.GetValue("U_SCGD_Kilometraje", i).ToString()
                        UDS_IncluyeGastos.Item("Placa").Value = dtBusqueda.GetValue("U_SCGD_Num_Placa", i).ToString()
                        UDS_IncluyeGastos.Item("DocE").Value = dtBusqueda.GetValue("DocEntry", i).ToString()
                        UDS_IncluyeGastos.Item("CodCli").Value = dtBusqueda.GetValue("CardCode", i).ToString()
                        UDS_IncluyeGastos.Item("Moneda").Value = dtBusqueda.GetValue("DocCur", i).ToString()
                        UDS_IncluyeGastos.Item("TipoCam").Value = dtBusqueda.GetValue("DocRate", i).ToString()
                        UDS_IncluyeGastos.Item("FechaCot").Value = dtBusqueda.GetValue("DocDate", i).ToString()

                    Next

                    strMoneda = txtMonCot.ObtieneValorUserDataSource()
                    strFechaCot = txtFechaCot.ObtieneValorUserDataSource()

                    If Not String.IsNullOrEmpty(txtTCCot.ObtieneValorUserDataSource().ToString()) Then
                        dcTCCot = Decimal.Parse(txtTCCot.ObtieneValorUserDataSource().ToString())
                    Else
                        dcTCCot = 110
                    End If

                    CargarGastos()
                    FormularioSBO.Items.Item("btnDoc").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

                Else

                    'txtNoOrden.AsignaValorUserDataSource(String.Empty)
                    txtNoUni.AsignaValorUserDataSource(String.Empty)
                    txtTiOr.AsignaValorUserDataSource(String.Empty)
                    txtEsOT.AsignaValorUserDataSource(String.Empty)
                    txtMarca.AsignaValorUserDataSource(String.Empty)
                    txtEstilo.AsignaValorUserDataSource(String.Empty)
                    txtModelo.AsignaValorUserDataSource(String.Empty)
                    txtNoCono.AsignaValorUserDataSource(String.Empty)
                    txtNoVin.AsignaValorUserDataSource(String.Empty)
                    txtKim.AsignaValorUserDataSource(String.Empty)
                    txtPlaca.AsignaValorUserDataSource(String.Empty)
                    txtDocE.AsignaValorUserDataSource(String.Empty)
                    txtCodCli.AsignaValorUserDataSource(String.Empty)
                    txtMonCot.AsignaValorUserDataSource(String.Empty)
                    txtTCCot.AsignaValorUserDataSource(String.Empty)
                    txtFechaCot.AsignaValorUserDataSource(String.Empty)
                    FormularioSBO.Items.Item("btnDoc").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.NoOTNoExiste, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)


                End If

                FormularioSBO.Mode = BoFormMode.fm_OK_MODE
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Function ValidaCambios() As Boolean
        Dim oMatrix As Matrix
        Dim oForm As Form
        Dim Posicion As Integer = 0

        Try
            oForm = ApplicationSBO.Forms.Item("SCGD_AGOT")
            dtGastos = oForm.DataSources.DataTables.Item(strDTGastos)
            oMatrix = DirectCast(oForm.Items.Item("mtxGas").Specific, Matrix)

            For i As Integer = 0 To dtGastos.Rows.Count - 1

                If dtGastos.GetValue("per", i) = My.Resources.Resource.No Then
                    FormularioSBO.Items.Item("btnDoc").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    Return True
                End If
            Next

             Return False
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Public Sub ManejaModoFormulario(ByRef pVal As SAPbouiCOM.ItemEvent)
        Try
            Dim l_blnTienePendientes As Boolean = False

            For i As Integer = 0 To dtGastos.Rows.Count - 1

                If dtGastos.GetValue("per", i) = My.Resources.Resource.No Then
                    l_blnTienePendientes = True
                End If
            Next

            If l_blnTienePendientes Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            Else
                FormularioSBO.Mode = BoFormMode.fm_OK_MODE
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub
    Public Function ValidarDocumento(ByRef bubbleEvent As Boolean) As Boolean


        Dim strEstado As String
        Dim strEstadoCot As String
        Dim blnResult As Boolean = True

        Dim strConsultaEstado As String = "Select code from [@SCGD_ESTADOS_OT] where Name = '{0}'"
        Dim strConsultaCot As String = "Select DocStatus from OQUT where DocEntry =  '{0}'"

        Try

            strEstado = Utilitarios.EjecutarConsulta(String.Format(strConsultaEstado, txtEsOT.ObtieneValorUserDataSource.Trim()), CompanySBO.CompanyDB, CompanySBO.Server)
            strEstadoCot = Utilitarios.EjecutarConsulta(String.Format(strConsultaCot, txtDocE.ObtieneValorUserDataSource.Trim()), CompanySBO.CompanyDB, CompanySBO.Server)

            'Valira estado de la OT
            If strEstado <> "1" AndAlso strEstado <> "2" Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeIncluirGastosEstadoOt, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                bubbleEvent = False
                blnResult = False
            ElseIf strEstadoCot <> "O" Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeIncluirGastosEstadoCotizacion, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                bubbleEvent = False
                blnResult = False
            End If

            Return blnResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Function ValidarLineasDoc(ByRef bubbleEvent As Boolean)
        Dim oMatrix As Matrix
        Dim oForm As Form
        Dim Posicion As Integer = 0
        Dim l_Result As Boolean = True
        Dim l_Selec As Boolean = False

        Try

            oForm = ApplicationSBO.Forms.Item("SCGD_AGOT")
            dtGastos = oForm.DataSources.DataTables.Item(strDTGastos)
            oMatrix = DirectCast(oForm.Items.Item("mtxGas").Specific, Matrix)

            For i As Integer = 0 To dtGastos.Rows.Count - 1

                If dtGastos.GetValue("sel", i) = "Y" AndAlso
                    (Not String.IsNullOrEmpty(dtGastos.GetValue("fac", i)) OrElse
                     Not String.IsNullOrEmpty(dtGastos.GetValue("asi", i))) Then

                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeIncluirGastosTieneDocumento, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    l_Result = False
                    bubbleEvent = False
                    Exit Function
                ElseIf dtGastos.GetValue("per", i) = My.Resources.Resource.No Then

                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeIncluirGastosTienePendientesdeActualizar, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    l_Result = False
                    bubbleEvent = False
                    Exit Function
                End If
            Next

            For i As Integer = 0 To dtGastos.Rows.Count - 1
                If dtGastos.GetValue("sel", i) = "Y" Then
                    l_Selec = True
                    Exit For
                End If
            Next

            If l_Selec = False Then
                If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeIncluirGastosSinLineas, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                    l_Result = False
                    bubbleEvent = False
                End If
            End If
            Return l_Result
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

#End Region

End Class


