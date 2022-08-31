Imports System.Globalization
Imports System.Threading
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class CosteoDeEntradas : Implements IFormularioSBO


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
    'valores cotizacion
    Private Shared _strMoneda As String
    Private Shared _dcTCCot As Decimal
    Private Shared _strFechaCot As String

    Private dtVehiculos As SAPbouiCOM.DataTable
    Private dtPedidos As SAPbouiCOM.DataTable

    Private txtCodProv As EditTextSBO
    Private txtNamProv As EditTextSBO
    Private txtFhaDoc As EditTextSBO
    Private txtFhaCont As EditTextSBO
    Private txtTipoC As EditTextSBO
    Private txtMontoPror As EditTextSBO
    Private txtCant As EditTextSBO
    Private txtTotal As EditTextSBO
    Private txtImp As EditTextSBO
    Private txtTotalD As EditTextSBO
    Private txtObs As EditTextSBO
    Private txtNumRef As EditTextSBO
    Private txtDocNum As EditTextSBO
    Private txtFhaVenc As EditTextSBO
    Private txtCodTitular As EditTextSBO
    Private txtNamTitular As EditTextSBO
    Private txtCodFactProv As EditTextSBO
    Private txtCodDraft As EditTextSBO
    Private txtDocEntry As EditTextSBO

    Private cboSerie As ComboBoxSBO
    Private cboContac As ComboBoxSBO
    Private cboTrans As ComboBoxSBO
    Private cboMoneda As ComboBoxSBO
    Private cboTipo As ComboBoxSBO
    Private cboPror As ComboBoxSBO
    Private cboStatus As ComboBoxSBO
    Private cboEncarg As ComboBoxSBO

    Private cbxCancelar As CheckBoxSBO
    Private cbxAplicaCosteo As CheckBoxSBO

    Private btnCalcula As ButtonSBO
    Private btnCopy As ButtonSBO
    Private btnFactura As ButtonSBO
    Private btnMas As ButtonSBO
    Private btnMenos As ButtonSBO

    Private dtLocal As SAPbouiCOM.DataTable
    Private dtLocal2 As SAPbouiCOM.DataTable
    Private dtVehi As SAPbouiCOM.DataTable
    Private dtCuenta As SAPbouiCOM.DataTable

    '------------------------------------------------------
    Private MatrixCostArticulos As MatrizCosteoArticulos
    Private MatrixCostPedidos As MatrizCosteoPedidos

    Private UDS_Datos As UserDataSources

    Private m_strGroupNum As String
    Private m_strImpuestoSocio As String
    Private m_strGeneraDraft As String
    Private m_strFactPorUnid As String
    Private m_strUsaCosteoAuto As String
    Private m_strAplicaCosteo As String

    Dim m_oGestorFormularios As GestorFormularios
    Dim m_oFormularioSeleccionLineasRecepcion As SeleccionLineasRecepcion

#Region "Propiedades"


    'Manejo de propiedades para la aplicacion

    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
        End Get
    End Property

    Public ReadOnly Property CompanySBO As SAPbobsCOM.ICompany Implements IFormularioSBO.CompanySBO
        Get
            Return _companySbo
        End Get
    End Property

    Public Property FormType As String Implements IFormularioSBO.FormType
        Get
            Return _formType
        End Get
        Set(ByVal value As String)
            _formType = value
        End Set
    End Property

    Public Property FormularioSBO As SAPbouiCOM.IForm Implements IFormularioSBO.FormularioSBO
        Get
            Return _formularioSBO
        End Get
        Set(ByVal value As SAPbouiCOM.IForm)
            _formularioSBO = value
        End Set
    End Property

    Public Property Inicializado As Boolean Implements IFormularioSBO.Inicializado
        Get
            Return _inicializado
        End Get
        Set(ByVal value As Boolean)
            _inicializado = value
        End Set
    End Property

    Public Property NombreXml As String Implements IFormularioSBO.NombreXml
        Get
            Return _nombreXml
        End Get
        Set(ByVal value As String)
            _nombreXml = value
        End Set
    End Property

    Public Property Titulo As String Implements IFormularioSBO.Titulo
        Get
            Return _titulo
        End Get
        Set(ByVal value As String)
            _titulo = value
        End Set
    End Property

    Public Property IdMenu As String Implements IUsaMenu.IdMenu
        Get
            Return _idMenu
        End Get
        Set(ByVal value As String)
            _idMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements IUsaMenu.MenuPadre
        Get
            Return _menuPadre
        End Get
        Set(ByVal value As String)
            _menuPadre = value
        End Set
    End Property

    Public Property Nombre As String Implements IUsaMenu.Nombre
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Public Property Posicion As Integer Implements IUsaMenu.Posicion
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

#End Region

#Region "Contructor"
    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByRef p_SeleccionLineasRecepcion As SeleccionLineasRecepcion, ByVal p_strCosteoDeEntradas As String)
        Dim strCadenaConexionBDTaller As String

        _companySbo = companySbo
        _applicationSbo = application
        m_oGestorFormularios = New GestorFormularios(_applicationSbo)
        m_oFormularioSeleccionLineasRecepcion = p_SeleccionLineasRecepcion
        n = DIHelper.GetNumberFormatInfo(_companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioCosteoDeEntradas
        MenuPadre = "SCGD_CEIM"
        Nombre = My.Resources.Resource.SubMenuCosteoEntrada
        IdMenu = p_strCosteoDeEntradas
        Titulo = My.Resources.Resource.SubMenuCosteoEntrada
        Posicion = 3
        FormType = p_strCosteoDeEntradas
    End Sub

#End Region


#Region "Metodos / Funciones"
    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario

        Dim oItem As SAPbouiCOM.Item

        If Not FormularioSBO Is Nothing Then

            CargarFormulario()

        End If
    End Sub

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

    End Sub

    Public Sub CargarFormulario()
        Try

            Call LigarControles()

            MatrixCostArticulos = New MatrizCosteoArticulos("mtx_Vehi", FormularioSBO, "@SCGD_COST_ART")
            MatrixCostArticulos.CreaColumnas()
            LigarColumnasArticulos(MatrixCostArticulos)

            MatrixCostPedidos = New MatrizCosteoPedidos("mtx_Pedido", FormularioSBO, "@SCGD_COST_LIN")
            MatrixCostPedidos.CreaColumnas()
            LigarColumnasPedidos(MatrixCostPedidos)

            dtPedidos = FormularioSBO.DataSources.DataTables.Add("dtPed")
            dtVehiculos = FormularioSBO.DataSources.DataTables.Add("dtVeh")

            dtLocal = FormularioSBO.DataSources.DataTables.Add("dtLocal")
            dtLocal2 = FormularioSBO.DataSources.DataTables.Add("dtLocal2")
            dtVehi = FormularioSBO.DataSources.DataTables.Add("dtVehi")
            dtCuenta = FormularioSBO.DataSources.DataTables.Add("dtCuenta")

            UDS_Datos = FormularioSBO.DataSources.UserDataSources
            UDS_Datos.Add("cTipo", BoDataType.dt_SHORT_TEXT, 100)
            cboTipo = New ComboBoxSBO("cboTipo", FormularioSBO, True, "", "cTipo")
            cboTipo.AsignaBinding()

            Call CargarCombos()

            If (DMS_Connector.Configuracion.ParamGenAddon.U_UsaVATGroup = "Y") Then
                AddChooseFromList(FormularioSBO, "5", "CFL_Imp")
                AddChooseFromList(FormularioSBO, "5", "CFL_Imp2")
            Else
                AddChooseFromList(FormularioSBO, "128", "CFL_Imp")
                AddChooseFromList(FormularioSBO, "128", "CFL_Imp2")
            End If

            AddChooseFromList(FormularioSBO, "1", "CFL_Cta")

            AsignaCFLColumn("mtx_Vehi", "col_Cta", "CFL_Cta", "AcctCode")
            AsignaCFLColumn("mtx_Vehi", "col_Imp", "CFL_Imp", "Code")
            AsignaCFLColumn("mtx_Pedido", "col_Imp", "CFL_Imp2", "Code")

            CargarSerieDocumento()
            CargarMonedaLocal()
            ObtenerConfiguraciones()
            ManejaEstadoBntFactura(False)
            ManejaItemsCosteoAutomatico()


            FormularioSBO.Items.Item(btnCopy.UniqueId).Enabled = True

            If m_strGeneraDraft.Equals("Y") Then

                FormularioSBO.Items.Item(txtCodDraft.UniqueId).Visible = True
                FormularioSBO.Items.Item("lkbDraft").Visible = True
                FormularioSBO.Items.Item("lblDraft").Visible = True

            ElseIf m_strGeneraDraft.Equals("N") Then

                FormularioSBO.Items.Item(txtCodDraft.UniqueId).Visible = False
                FormularioSBO.Items.Item("lkbDraft").Visible = False
                FormularioSBO.Items.Item("lblDraft").Visible = False

            End If

            If (DMS_Connector.Configuracion.ParamGenAddon.U_UsaFactPorUnid <> "Y") Then
                MatrixCostArticulos.ColumnaColFac.Columna.Visible = False
            End If

            txtFhaDoc.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd"))

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Function LigarColumnasArticulos(ByRef oMatrix As MatrizCosteoArticulos)
        Dim oColumna As ColumnaMatrixSBO(Of String)

        Try

            oColumna = oMatrix.ColumnaColIDVeh
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_ART", "U_ID_Unid")

            oColumna = oMatrix.ColumnaColPed
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_ART", "U_Cod_Pedido")

            oColumna = oMatrix.ColumnaColEnt
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_ART", "U_Cod_Entrada")

            oColumna = oMatrix.ColumnaColCod
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_ART", "U_Cod_Unid")

            oColumna = oMatrix.ColumnaColVin
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_ART", "U_Num_VIN")

            oColumna = oMatrix.ColumnaColMot
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_ART", "U_Num_Motor")

            oColumna = oMatrix.ColumnaColMar
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_ART", "U_Cod_Marca")

            oColumna = oMatrix.ColumnaColEst
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_ART", "U_Cod_Estilo")

            oColumna = oMatrix.ColumnaColMod
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_ART", "U_Cod_Modelo")

            oColumna = oMatrix.ColumnaColCol
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_ART", "U_Cod_Color")

            oColumna = oMatrix.ColumnaColTImp
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_ART", "U_Mnt_Imp")

            oColumna = oMatrix.ColumnaColRef
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_ART", "U_Line_Ref")

            oColumna = oMatrix.ColumnaColArt
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_ART", "U_Cod_Art")

            oColumna = oMatrix.ColumnaColAno
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_ART", "U_Ano_Veh")
            Return True

        Catch ex As Exception
            ' Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function LigarColumnasPedidos(ByRef oMatrix As MatrizCosteoPedidos)
        Dim oColumna As ColumnaMatrixSBO(Of String)

        Try

            oColumna = oMatrix.ColumnaPed
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_LIN", "U_Cod_Pedido")

            oColumna = oMatrix.ColumnaEnt
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_LIN", "U_Cod_Entrada")

            oColumna = oMatrix.ColumnaCodArt
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_LIN", "U_Cod_Art")

            oColumna = oMatrix.ColumnaDesArt
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_LIN", "U_Nam_Art")

            oColumna = oMatrix.ColumnaCodCol
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_LIN", "U_Cod_Color")

            oColumna = oMatrix.ColumnaCan
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_LIN", "U_Cant")

            oColumna = oMatrix.ColumnaAno
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_LIN", "U_Ano_Veh")

            oColumna = oMatrix.ColumnaCost
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_LIN", "U_Mnt_Linea")

            oColumna = oMatrix.ColumnaTImp
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_LIN", "U_Mnt_Impuesto")

            oColumna = oMatrix.ColumnaImp
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_LIN", "U_Cod_Imp")

            oColumna = oMatrix.ColumnaRef
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_COST_LIN", "U_Line_Ref")

            Return True

        Catch ex As Exception
            ' Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Private Sub CargarFormularioSeleccionVehiculos()
        Dim strProveedor As String
        Try
            strProveedor = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_COSTEO_ENT").GetValue("U_Cod_Prov", 0).Trim()
            m_oFormularioSeleccionLineasRecepcion.strCodProveedor = strProveedor
            m_oFormularioSeleccionLineasRecepcion.MOCosteoDeEntradas = Me
            If Not m_oGestorFormularios.FormularioAbierto(m_oFormularioSeleccionLineasRecepcion, True) Then

                m_oGestorFormularios.CargaFormulario(m_oFormularioSeleccionLineasRecepcion)

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub CargarCombos()
        Try
            Dim oMatriz As SAPbouiCOM.Matrix
            Dim oItem As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox


            '----------------- MATRIZ Pedidos ----------------- 
            oMatriz = DirectCast(FormularioSBO.Items.Item("mtx_Pedido").Specific, SAPbouiCOM.Matrix)

            dtLocal.Clear()
            dtLocal.ExecuteQuery("select code, name from [@SCGD_COLOR] ")

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oMatriz.Columns.Item("col_Col").ValidValues.Add(dtLocal.GetValue("code", i), dtLocal.GetValue("name", i))
            Next

            '----------------- MATRIZ UNIDADES ----------------- 

            oMatriz = DirectCast(FormularioSBO.Items.Item("mtx_Vehi").Specific, SAPbouiCOM.Matrix)

            ' dtLocal.Clear()
            'dtLocal.ExecuteQuery("select code, name from [@SCGD_COLOR]")

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oMatriz.Columns.Item("col_Col").ValidValues.Add(dtLocal.GetValue("code", i), dtLocal.GetValue("name", i))
            Next

            dtLocal.Clear()
            dtLocal.ExecuteQuery("SELECT  Code,U_Cod_Esti,U_Descripcion FROM [@SCGD_MODELO]")
            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oMatriz.Columns.Item("col_Mod").ValidValues.Add(dtLocal.GetValue("Code", i), dtLocal.GetValue("U_Descripcion", i))
            Next

            dtLocal.Clear()
            dtLocal.ExecuteQuery("SELECT Code,Name,U_Cod_Marc FROM [@SCGD_ESTILO]")
            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oMatriz.Columns.Item("col_Est").ValidValues.Add(dtLocal.GetValue("Code", i), dtLocal.GetValue("Name", i))
            Next

            dtLocal.Clear()
            dtLocal.ExecuteQuery("SELECT Code,Name FROM [@SCGD_MARCA]")
            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oMatriz.Columns.Item("col_Mar").ValidValues.Add(dtLocal.GetValue("Code", i), dtLocal.GetValue("Name", i))
            Next

            dtLocal.Clear()
            dtLocal.ExecuteQuery("SELECT Code ,Name, DocEntry  FROM [@SCGD_TRAN_COMP]")
            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oMatriz.Columns.Item("col_Tra").ValidValues.Add(dtLocal.GetValue("Code", i), dtLocal.GetValue("Name", i))
            Next

            '----------------- Encabezado ----------------- 
            oItem = FormularioSBO.Items.Item("cboTipo")
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            If oCombo.ValidValues.Count = 0 Then
                oCombo.ValidValues.Add("A", My.Resources.Resource.MensajeCosteoEntradasUnidades)
                oCombo.ValidValues.Add("P", My.Resources.Resource.MensajeCosteoEntradasPedidos)
                oCombo.Select("A")
            End If

            oItem = FormularioSBO.Items.Item(cboTrans.UniqueId)
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            dtLocal.Clear()
            dtLocal.ExecuteQuery("SELECT Code ,Name, DocEntry  FROM [@SCGD_TRAN_COMP]")

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oCombo.ValidValues.Add(dtLocal.GetValue("Code", i), dtLocal.GetValue("Name", i))
            Next


            oItem = FormularioSBO.Items.Item(cboMoneda.UniqueId)
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            dtLocal.Clear()
            dtLocal.ExecuteQuery("select CurrCode, CurrName from OCRN")

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oCombo.ValidValues.Add(dtLocal.GetValue("CurrCode", i), dtLocal.GetValue("CurrName", i))
            Next

            oItem = FormularioSBO.Items.Item(cboPror.UniqueId)
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            If oCombo.ValidValues.Count = 0 Then
                oCombo.ValidValues.Add("S", "Simple")
                cboPror.AsignaValorDataSource("S")
                'oCombo.Select("S")
            End If

            dtLocal.Clear()
            dtLocal.ExecuteQuery("Select SlpCode, SlpName from OSLP ")

            oItem = FormularioSBO.Items.Item(cboEncarg.UniqueId)
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oCombo.ValidValues.Add(dtLocal.GetValue("SlpCode", i), dtLocal.GetValue("SlpName", i))
            Next

            oItem = FormularioSBO.Items.Item(cboStatus.UniqueId)
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            If oCombo.ValidValues.Count = 0 Then

                oCombo.ValidValues.Add("O", My.Resources.Resource.EstadoDocumentoAbierto)
                oCombo.ValidValues.Add("C", My.Resources.Resource.EstadoDocumentoCerrado)

            End If

            oItem = FormularioSBO.Items.Item(cboSerie.UniqueId)
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
            oCombo.ValidValues.LoadSeries("SCGD_CDP", SAPbouiCOM.BoSeriesMode.sf_Add)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub LigarControles()
        Try

            Dim mc_strTableCosteo As String = "@SCGD_COSTEO_ENT"

            txtCodProv = New EditTextSBO("txtCPro", True, mc_strTableCosteo, "U_Cod_Prov", FormularioSBO)
            txtNamProv = New EditTextSBO("txtNPro", True, mc_strTableCosteo, "U_Nam_Prov", FormularioSBO)
            txtNumRef = New EditTextSBO("txtNumRef", True, mc_strTableCosteo, "U_Num_Ref", FormularioSBO)
            txtTipoC = New EditTextSBO("txtDocRate", True, mc_strTableCosteo, "U_Doc_Rate", FormularioSBO)
            txtDocNum = New EditTextSBO("txtDocNum", True, mc_strTableCosteo, "DocNum", FormularioSBO)
            'txtStatus = New EditTextSBO("txtStatus", True, mc_strTableCosteo, "Status", FormularioSBO)
            txtFhaCont = New EditTextSBO("txtFhaCon", True, mc_strTableCosteo, "U_Fha_Cont", FormularioSBO)
            txtFhaVenc = New EditTextSBO("txtFhaVenc", True, mc_strTableCosteo, "U_Fha_Venc", FormularioSBO)
            txtFhaDoc = New EditTextSBO("txtFhaDoc", True, mc_strTableCosteo, "U_Fha_Doc", FormularioSBO)
            txtMontoPror = New EditTextSBO("txtMonto", True, mc_strTableCosteo, "U_Mnt_Prorrateo", FormularioSBO)
            txtCant = New EditTextSBO("txtCant", True, mc_strTableCosteo, "U_Cant", FormularioSBO)
            txtTotal = New EditTextSBO("txtTotal", True, mc_strTableCosteo, "U_Mnt_Total", FormularioSBO)
            txtImp = New EditTextSBO("txtImp", True, mc_strTableCosteo, "U_Mtn_Impuesto", FormularioSBO)
            txtTotalD = New EditTextSBO("txtTotDoc", True, mc_strTableCosteo, "U_Mnt_Total_Doc", FormularioSBO)
            txtObs = New EditTextSBO("txtObs", True, mc_strTableCosteo, "U_Comment", FormularioSBO)
            txtNamTitular = New EditTextSBO("txtNamTi", True, mc_strTableCosteo, "U_Nam_Titular", FormularioSBO)
            txtCodTitular = New EditTextSBO("txtCodTi", True, mc_strTableCosteo, "U_Cod_Titutlar", FormularioSBO)
            txtCodFactProv = New EditTextSBO("txtFacPro", True, mc_strTableCosteo, "U_NumFactura", FormularioSBO)
            txtDocEntry = New EditTextSBO("txtDocEnt", True, mc_strTableCosteo, "DocEntry", FormularioSBO)
            txtCodDraft = New EditTextSBO("txtDraft", True, mc_strTableCosteo, "U_NumDraft", FormularioSBO)

            cboTrans = New ComboBoxSBO("cboTransac", FormularioSBO, True, mc_strTableCosteo, "U_Cod_Transac")
            cboMoneda = New ComboBoxSBO("cboMoneda", FormularioSBO, True, mc_strTableCosteo, "U_Doc_Curr")
            ' cboTipo = New ComboBoxSBO("cboTipo", FormularioSBO, True, mc_strTableCosteo, "cTipo")
            cboPror = New ComboBoxSBO("cboPror", FormularioSBO, True, mc_strTableCosteo, "U_Cod_Prorrateo")
            cboContac = New ComboBoxSBO("cboContact", FormularioSBO, True, mc_strTableCosteo, "U_Per_Cont")
            cboSerie = New ComboBoxSBO("cboSerie", FormularioSBO, True, mc_strTableCosteo, "Series")
            cboStatus = New ComboBoxSBO("cboStatus", FormularioSBO, True, mc_strTableCosteo, "Status")
            cboEncarg = New ComboBoxSBO("cboEncComp", FormularioSBO, True, mc_strTableCosteo, "U_Cod_Enc_Comp")

            cbxCancelar = New CheckBoxSBO("cbxCancel", True, mc_strTableCosteo, "Canceled", FormularioSBO)
            cbxAplicaCosteo = New CheckBoxSBO("chkCosteo", True, mc_strTableCosteo, "U_AplicaCosteo", FormularioSBO)

            btnCalcula = New ButtonSBO("btnCalcula", FormularioSBO)
            btnCopy = New ButtonSBO("btnCopy", FormularioSBO)
            btnFactura = New ButtonSBO("btnFactura", FormularioSBO)
            btnMas = New ButtonSBO("btnMas", FormularioSBO)
            btnMenos = New ButtonSBO("btnMenos", FormularioSBO)

            txtCodProv.AsignaBinding()
            txtNamProv.AsignaBinding()
            txtNumRef.AsignaBinding()
            txtFhaCont.AsignaBinding()
            txtFhaDoc.AsignaBinding()
            txtFhaVenc.AsignaBinding()
            txtTipoC.AsignaBinding()
            ' txtDocNum.AsignaBinding()
            txtMontoPror.AsignaBinding()
            txtCant.AsignaBinding()
            txtTotal.AsignaBinding()
            txtImp.AsignaBinding()
            txtTotalD.AsignaBinding()
            txtObs.AsignaBinding()
            txtNamTitular.AsignaBinding()
            txtCodTitular.AsignaBinding()
            txtCodFactProv.AsignaBinding()
            txtDocEntry.AsignaBinding()
            txtCodDraft.AsignaBinding()

            cbxCancelar.AsignaBinding()
            cbxAplicaCosteo.AsignaBinding()

            cboTrans.AsignaBinding()
            cboMoneda.AsignaBinding()
            cboPror.AsignaBinding()
            cboSerie.AsignaBinding()
            cboContac.AsignaBinding()
            cboStatus.AsignaBinding()
            cboEncarg.AsignaBinding()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub CargarSerieDocumento()
        Try
            Dim l_strSQL As String
            Dim l_strSerie As String
            Dim oItems As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox

            oItems = FormularioSBO.Items.Item(cboSerie.UniqueId)
            oCombo = CType(oItems.Specific, SAPbouiCOM.ComboBox)
            oCombo.Select(oCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            l_strSQL = "Select NextNumber from nnm1 where ObjectCode = 'SCGD_CDP' and Series = '{0}'"

            l_strSerie = cboSerie.ObtieneValorDataSource()

            dtLocal.ExecuteQuery(String.Format(l_strSQL, l_strSerie))

            If Not String.IsNullOrEmpty(dtLocal.GetValue("NextNumber", 0)) Then
                txtDocNum.AsignaValorDataSource(dtLocal.GetValue("NextNumber", 0))
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub CargarMonedaLocal(Optional ByVal p_blnDocNuevo As Boolean = True)
        Try
            Dim l_StrSQLSys As String

            Dim l_strMonLocal As String
            Dim l_strMonSist As String

            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()
            dtLocal.ExecuteQuery(l_StrSQLSys)

            ' FormularioSBO.Freeze(True)

            If Not String.IsNullOrEmpty(dtLocal.GetValue("MainCurncy", 0)) Then
                l_strMonLocal = dtLocal.GetValue("MainCurncy", 0)
                l_strMonSist = dtLocal.GetValue("SysCurrncy", 0)
            End If

            FormularioSBO.Items.Item(cboMoneda.UniqueId).Visible = True
            FormularioSBO.Items.Item(txtTipoC.UniqueId).Visible = False


            If p_blnDocNuevo Then
                cboMoneda.AsignaValorDataSource(l_strMonLocal)
                txtTipoC.AsignaValorDataSource(1)
            Else
                If cboMoneda.ObtieneValorDataSource <> l_strMonLocal Then
                    FormularioSBO.Items.Item(txtTipoC.UniqueId).Visible = True
                Else
                    FormularioSBO.Items.Item(txtTipoC.UniqueId).Visible = False
                    txtTipoC.AsignaValorDataSource(1)
                End If
            End If


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub CargarMonedaSocio(ByVal p_strCardCode As String)
        Try
            Dim l_strSQLProv As String
            Dim l_StrSQLSys As String
            Dim l_strMoneda As String
            Dim l_strMonSys As String
            Dim l_strMonLoc As String

            l_strSQLProv = "Select  CardCode, CardName, Currency  from OCRD OC where CardCode = '{0}'"
            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            dtLocal.ExecuteQuery(l_StrSQLSys)
            l_strMonLoc = dtLocal.GetValue("MainCurncy", 0)
            l_strMonSys = dtLocal.GetValue("SysCurrncy", 0)

            dtLocal.Clear()
            dtLocal.ExecuteQuery(String.Format(l_strSQLProv, p_strCardCode))

            If Not String.IsNullOrEmpty(dtLocal.GetValue("CardCode", 0)) Then
                l_strMoneda = dtLocal.GetValue("Currency", 0)

                If l_strMoneda = My.Resources.Resource.MonedasTodas Then
                    FormularioSBO.Items.Item(cboMoneda.UniqueId).Enabled = True
                    FormularioSBO.Items.Item(cboMoneda.UniqueId).Visible = True
                    cboMoneda.AsignaValorDataSource(l_strMonLoc)
                Else
                    FormularioSBO.Items.Item(cboMoneda.UniqueId).Enabled = False
                    FormularioSBO.Items.Item(cboMoneda.UniqueId).Visible = True
                    cboMoneda.AsignaValorDataSource(l_strMoneda)
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub ObtenerConfiguraciones()
        Try

            Dim l_strValor As String
            Dim l_strSQL As String

            dtLocal = _formularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            ' l_strValor = Utilitarios.EjecutarConsulta("Select Code, U_Gen_Draft_Cost, U_UsaFactPorUnid FROM [@SCGD_ADMIN] where CODE = 'DMS'", _companySbo.CompanyDB, _companySbo.Server)
            l_strSQL = "Select Code, U_Gen_Draft_Cost, U_UsaFactPorUnid, U_UsaCostAuto FROM [@SCGD_ADMIN] where CODE = 'DMS' "
            dtLocal.ExecuteQuery(l_strSQL)

            If Not String.IsNullOrEmpty(dtLocal.GetValue("Code", 0)) Then

                If String.IsNullOrEmpty(dtLocal.GetValue("U_Gen_Draft_Cost", 0)) Or dtLocal.GetValue("U_Gen_Draft_Cost", 0).Equals("N") Then
                    m_strGeneraDraft = "N"
                ElseIf dtLocal.GetValue("U_Gen_Draft_Cost", 0).Equals("Y") Then
                    m_strGeneraDraft = "Y"
                End If

                If String.IsNullOrEmpty(dtLocal.GetValue("U_UsaFactPorUnid", 0)) Or dtLocal.GetValue("U_UsaFactPorUnid", 0).Equals("N") Then
                    m_strFactPorUnid = "N"
                ElseIf dtLocal.GetValue("U_UsaFactPorUnid", 0).Equals("Y") Then
                    m_strFactPorUnid = "Y"
                End If

                If String.IsNullOrEmpty(dtLocal.GetValue("U_UsaCostAuto", 0)) Or dtLocal.GetValue("U_UsaCostAuto", 0).Equals("N") Then
                    m_strUsaCosteoAuto = "N"
                ElseIf dtLocal.GetValue("U_UsaCostAuto", 0).Equals("Y") Then
                    m_strUsaCosteoAuto = "Y"
                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Sub

    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.FormTypeEx <> FormType Then Exit Sub

        Select Case pVal.EventType
            Case BoEventTypes.et_ITEM_PRESSED
                ManejadorEventosItemPressed(FormUID, pVal, BubbleEvent)
            Case BoEventTypes.et_CHOOSE_FROM_LIST
                ManejadorEventoChooseFromList(pVal, FormUID, BubbleEvent)
            Case BoEventTypes.et_COMBO_SELECT
                ManejadorEventoCombo(FormUID, pVal, BubbleEvent)
            Case BoEventTypes.et_CLICK
        End Select

    End Sub

    Private Sub AsignaCFLColumna()
        Try

            Dim oItem As SAPbouiCOM.Item
            Dim oMatrix As SAPbouiCOM.Matrix

            oItem = FormularioSBO.Items.Item("mtx_Pedido")
            oMatrix = DirectCast(oItem.Specific, SAPbouiCOM.Matrix)

            oMatrix.Columns.Item("col_Imp").ChooseFromListUID = "CFL_Imp"
            oMatrix.Columns.Item("col_Imp").ChooseFromListAlias = "Code"

            oItem = FormularioSBO.Items.Item("mtx_Vehi")
            oMatrix = DirectCast(oItem.Specific, SAPbouiCOM.Matrix)

            oMatrix.Columns.Item("col_Imp").ChooseFromListUID = "CFL_Imp2"
            oMatrix.Columns.Item("col_Imp").ChooseFromListAlias = "Code"

        Catch ex As Exception

        End Try
    End Sub

    Public Sub AsignaValoresTxtProvedor(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try

            Dim l_strSQL As String
            Dim oitems As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox

            txtCodProv.AsignaValorDataSource(oDataTable.GetValue("CardCode", 0))
            txtNamProv.AsignaValorDataSource(oDataTable.GetValue("CardName", 0))


            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            l_strSQL = "Select CntctCode, Name from OCPR	where CardCode = '{0}'"

            dtLocal.ExecuteQuery(String.Format(l_strSQL, oDataTable.GetValue("CardCode", 0)))

            If Not String.IsNullOrEmpty(dtLocal.GetValue("CntctCode", 0)) Then

                oitems = FormularioSBO.Items.Item(cboContac.UniqueId)
                oCombo = CType(oitems.Specific, SAPbouiCOM.ComboBox)

                If oCombo.ValidValues.Count <> 0 Then
                    For i As Integer = 0 To oCombo.ValidValues.Count - 1
                        oCombo.ValidValues.Remove(oCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                    Next
                End If

                For i As Integer = 0 To dtLocal.Rows.Count - 1
                    oCombo.ValidValues.Add(dtLocal.GetValue("CntctCode", i), dtLocal.GetValue("Name", i))
                Next

                cboContac.AsignaValorDataSource(dtLocal.GetValue("CntctCode", 0))
            End If
            txtFhaVenc.AsignaValorDataSource(Nothing)
            m_strGroupNum = oDataTable.GetValue("GroupNum", 0)
            m_strImpuestoSocio = oDataTable.GetValue("VatGroup", 0)

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub


    Private Sub ManejaCheckCosteo(ByVal pVal As ItemEvent, ByVal BubbleEvent As Boolean)
        Try

            If cbxAplicaCosteo.ObtieneValorDataSource = "Y" Then
                m_strAplicaCosteo = "Y"
            Else
                m_strAplicaCosteo = "N"
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub ManejaItemsCosteoAutomatico()
        Try

            If m_strUsaCosteoAuto.Equals("Y") Then
                _formularioSBO.Items.Item(cbxAplicaCosteo.UniqueId).Visible = True
            Else
                _formularioSBO.Items.Item(cbxAplicaCosteo.UniqueId).Visible = False
            End If

            m_strAplicaCosteo = "Y"

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Sub

    Public Sub AsignaValoresTxtTitular(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try

            txtNamTitular.AsignaValorDataSource(oDataTable.GetValue("firstName", 0) & " " & oDataTable.GetValue("lastName", 0))
            txtCodTitular.AsignaValorDataSource(oDataTable.GetValue("empID", 0))

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresColImpuestoPed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            Dim oMat As SAPbouiCOM.Matrix

            FormularioSBO.Freeze(True)

            MatrixCostPedidos.Matrix.FlushToDataSource()
            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Imp", pVal.Row - 1, oDataTable.GetValue("Code", 0))
            MatrixCostPedidos.Matrix.LoadFromDataSource()


            oMat = DirectCast(FormularioSBO.Items.Item("mtx_Pedido").Specific, SAPbouiCOM.Matrix)
            oMat.Columns.Item("col_Imp").Cells.Item(pVal.Row).Click()

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresColImpuestoVeh(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            Dim oMat As SAPbouiCOM.Matrix

            FormularioSBO.Freeze(True)

            MatrixCostArticulos.Matrix.FlushToDataSource()
            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Impuesto", pVal.Row - 1, oDataTable.GetValue("Code", 0))

            MatrixCostArticulos.Matrix.LoadFromDataSource()

            oMat = DirectCast(FormularioSBO.Items.Item("mtx_Vehi").Specific, SAPbouiCOM.Matrix)
            oMat.Columns.Item("col_Imp").Cells.Item(pVal.Row).Click()

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            FormularioSBO.Freeze(False)
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresColNumCuenta(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            Dim oMat As SAPbouiCOM.Matrix

            FormularioSBO.Freeze(True)

            MatrixCostArticulos.Matrix.FlushToDataSource()
            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_Cta", pVal.Row - 1, oDataTable.GetValue("AcctCode", 0))

            MatrixCostArticulos.Matrix.LoadFromDataSource()

            oMat = DirectCast(FormularioSBO.Items.Item("mtx_Vehi").Specific, SAPbouiCOM.Matrix)
            oMat.Columns.Item("col_Cta").Cells.Item(pVal.Row).Click()

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            FormularioSBO.Freeze(False)
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaFechaVencimiento(ByVal p_strGroup As String)
        Try

            Dim l_strSQL As String
            Dim l_fhaCont As Date
            Dim intDias As Integer

            l_strSQL = "select (ExtraMonth * 30 + ExtraDays) as CantDias from OCTG with (nolock) where GroupNum ='{0}'"

            If Not String.IsNullOrEmpty(txtFhaCont.ObtieneValorDataSource) Then

                dtLocal2 = FormularioSBO.DataSources.DataTables.Item("dtLocal2")
                dtLocal2.Clear()

                dtLocal2.ExecuteQuery(String.Format(l_strSQL, p_strGroup))

                If Not String.IsNullOrEmpty(dtLocal2.GetValue("CantDias", 0)) Then
                    intDias = Integer.Parse(dtLocal2.GetValue("CantDias", 0))

                    l_fhaCont = Date.ParseExact(txtFhaCont.ObtieneValorDataSource, "yyyyMMdd", Nothing)
                    txtFhaCont.AsignaValorDataSource(l_fhaCont.ToString("yyyyMMdd"))

                    l_fhaCont = l_fhaCont.AddDays(intDias)
                    txtFhaVenc.AsignaValorDataSource(l_fhaCont.ToString("yyyyMMdd"))

                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    ''' <summary>
    ''' Para obtener el plazo para el vencimiento de los documentos.
    ''' obtener el Indicador de impuestos para el proveedor.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarInfoProveedor()
        Try
            Dim l_StrSQL As String

            l_StrSQL = "select GroupNum, VatGroup from OCRD where CardCode = '{0}'"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            dtLocal.ExecuteQuery(String.Format(l_StrSQL, txtCodProv.ObtieneValorDataSource()))

            If Not String.IsNullOrEmpty(dtLocal.GetValue("GroupNum", 0)) Then
                m_strGroupNum = dtLocal.GetValue("GroupNum", 0)
                m_strImpuestoSocio = dtLocal.GetValue("VatGroup", 0)
            Else
                m_strGroupNum = String.Empty
                m_strImpuestoSocio = String.Empty
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub




#End Region


End Class


