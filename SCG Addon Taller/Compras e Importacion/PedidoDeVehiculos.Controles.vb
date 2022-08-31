Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany


Partial Public Class PedidoDeVehiculos : Implements IFormularioSBO

    'maneja informacion de la aplicacion
    Private WithEvents _applicationSbo As Application
    'maneja informacion de la compania 
    Private _companySbo As ICompany
    Private _formType As String
    Private _formularioSBO As IForm
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

    Private txtCodProv As EditTextSBO
    Private txtDesProv As EditTextSBO
    Private txtTipoCam As EditTextSBO
    Private txtDocEntry As EditTextSBO
    Private txtFhaPedi As EditTextSBO
    Private txtFhaFabr As EditTextSBO
    Private txtCodTitu As EditTextSBO
    Private txtNamTitu As EditTextSBO
    Private txtObserv As EditTextSBO
    Private txtCant As EditTextSBO
    Private txtCantPendiente As EditTextSBO
    Private txtCantRecibida As EditTextSBO
    Private txtTotal As EditTextSBO
    Private txtFhaArribo As EditTextSBO
    Private txtDocNum As EditTextSBO

    Private cboMoneda As ComboBoxSBO
    Private cboEncarg As ComboBoxSBO
    Private cboSeries As ComboBoxSBO
    Private cboEstado As ComboBoxSBO
    Private cboContac As ComboBoxSBO
    Private btnMas As ButtonSBO
    Private btnMenos As ButtonSBO
    Private cbxCancelado As CheckBoxSBO
    Private oForm As Form
    Private dtLocal As DataTable
    Private matrixPedidoVehiculos As MatrizPedidoDeVehiculos
    Private n As NumberFormatInfo


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

#End Region

#Region "Contructor"
    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_StrPedidoVehiculos As String)

        _companySbo = companySbo
        _applicationSbo = application
        n = DIHelper.GetNumberFormatInfo(_companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioPedidoDeUnidades
        MenuPadre = "SCGD_CEIM"
        Nombre = My.Resources.Resource.SubMenuPedidoVehi
        IdMenu = p_StrPedidoVehiculos
        Titulo = My.Resources.Resource.SubMenuPedidoVehi
        Posicion = 1
        FormType = p_StrPedidoVehiculos

    End Sub

#End Region

#Region "Metodos / Funciones"
    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario


        If Not FormularioSBO Is Nothing Then
            oForm = ApplicationSBO.Forms.Item("SCGD_PDV")

            CargarFormulario()
        End If
    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        Try

            txtCodProv = New EditTextSBO("txtCodProv", True, m_strTablaPedidos, "U_Cod_Prov", FormularioSBO)
            txtDesProv = New EditTextSBO("txtNamProv", True, m_strTablaPedidos, "U_Name_Prov", FormularioSBO)
            txtTipoCam = New EditTextSBO("txtTipoCam", True, m_strTablaPedidos, "U_DocRate", FormularioSBO)
            txtDocEntry = New EditTextSBO("txtNumPed", True, m_strTablaPedidos, "DocEntry", FormularioSBO)
            txtFhaPedi = New EditTextSBO("txtFhaPed", True, m_strTablaPedidos, "U_Fha_Pedido", FormularioSBO)
            txtFhaFabr = New EditTextSBO("txtFhaFabr", True, m_strTablaPedidos, "U_Fha_Est_Fabrica", FormularioSBO)
            txtCodTitu = New EditTextSBO("txtCodTit", True, m_strTablaPedidos, "U_Cod_Titular", FormularioSBO)
            txtNamTitu = New EditTextSBO("txtNamTit", True, m_strTablaPedidos, "U_Name_Titular", FormularioSBO)
            txtObserv = New EditTextSBO("txtObs", True, m_strTablaPedidos, "U_Observ", FormularioSBO)
            txtCant = New EditTextSBO("txtCant", True, m_strTablaPedidos, "U_Cant_Veh", FormularioSBO)
            txtCantRecibida = New EditTextSBO("txtRecib", True, m_strTablaPedidos, "U_Recib_Veh", FormularioSBO)
            txtCantPendiente = New EditTextSBO("txtPend", True, m_strTablaPedidos, "U_Pend_Veh", FormularioSBO)
            txtTotal = New EditTextSBO("txtTotal", True, m_strTablaPedidos, "U_Total_Doc", FormularioSBO)
            txtFhaArribo = New EditTextSBO("txtFhaArr", True, m_strTablaPedidos, "U_Fha_Est_Arribo", FormularioSBO)
            txtDocNum = New EditTextSBO("txtDocNum", True, m_strTablaPedidos, "DocNum", FormularioSBO)

            cboMoneda = New ComboBoxSBO("cboMoneda", FormularioSBO, True, m_strTablaPedidos, "U_DocCurr")
            cboEncarg = New ComboBoxSBO("cboEnc", FormularioSBO, True, m_strTablaPedidos, "U_Enc_Compras")

            cboSeries = New ComboBoxSBO("cboSerie", FormularioSBO, True, m_strTablaPedidos, "Series")
            cboEstado = New ComboBoxSBO("cboEstado", FormularioSBO, True, m_strTablaPedidos, "Status")
            cboContac = New ComboBoxSBO("cboPerCont", FormularioSBO, True, m_strTablaPedidos, "U_CodContac")

            btnMas = New ButtonSBO("btnAdd", FormularioSBO)
            btnMenos = New ButtonSBO("btnMenos", FormularioSBO)

            cbxCancelado = New CheckBoxSBO("cbxCancel", True, m_strTablaPedidos, "Canceled", FormularioSBO)

            txtCodProv.AsignaBinding()
            txtDesProv.AsignaBinding()
            txtTipoCam.AsignaBinding()
            txtDocEntry.AsignaBinding()
            txtFhaPedi.AsignaBinding()
            txtFhaFabr.AsignaBinding()
            txtCodTitu.AsignaBinding()
            txtNamTitu.AsignaBinding()
            txtObserv.AsignaBinding()
            txtCant.AsignaBinding()
            txtCantPendiente.AsignaBinding()
            txtCantRecibida.AsignaBinding()
            txtTotal.AsignaBinding()
            txtFhaArribo.AsignaBinding()

            cboEncarg.AsignaBinding()
            cboMoneda.AsignaBinding()
            cboSeries.AsignaBinding()
            cboEstado.AsignaBinding()
            cboContac.AsignaBinding()
            cbxCancelado.AsignaBinding()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub CargarFormulario()
        Try

            FormularioSBO.Freeze(True)

            matrixPedidoVehiculos = New MatrizPedidoDeVehiculos("mtx_Ped", FormularioSBO, "SCGD_PEDIDOS_LINEAS")
            matrixPedidoVehiculos.CreaColumnas()
            LigarColumnasPedidos(matrixPedidoVehiculos)

            AddChooseFromList(FormularioSBO, "4", "CFL_Item2")
            AsignaCFLColumn("mtx_Ped", "col_Cod", "CFL_Item2", "ItemCode")

            Call CargarCombos()
            Call CargarSerieDocumento()

            Call AgregarPrimerLinea()

            txtFhaPedi.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd"))

            CargarMonedaLocal()

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub CargarCombos()

        Try
            Dim oMatrix As SAPbouiCOM.Matrix
            Dim oitems As SAPbouiCOM.Item
            Dim cboCombos As SAPbouiCOM.ComboBox

            'COLUMNA COLOR
            oMatrix = DirectCast(oForm.Items.Item("mtx_Ped").Specific, SAPbouiCOM.Matrix)

            dtLocal = FormularioSBO.DataSources.DataTables.Add("local")
            dtLocal.ExecuteQuery("select code, name from [@SCGD_COLOR]")

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oMatrix.Columns.Item("col_Col").ValidValues.Add(dtLocal.GetValue("code", i), dtLocal.GetValue("name", i))
            Next

            Call oMatrix.LoadFromDataSource()

            'MONEDA
            dtLocal.Clear()
            dtLocal.ExecuteQuery("select CurrCode, CurrName from OCRN")

            oitems = oForm.Items.Item(cboMoneda.UniqueId)
            cboCombos = CType(oitems.Specific, SAPbouiCOM.ComboBox)

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                cboCombos.ValidValues.Add(dtLocal.GetValue("CurrCode", i), dtLocal.GetValue("CurrName", i))
            Next

            'ENCARGADO
            dtLocal.Clear()
            dtLocal.ExecuteQuery("Select SlpCode, SlpName from OSLP ")

            oitems = oForm.Items.Item(cboEncarg.UniqueId)
            cboCombos = CType(oitems.Specific, SAPbouiCOM.ComboBox)

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                cboCombos.ValidValues.Add(dtLocal.GetValue("SlpCode", i), dtLocal.GetValue("SlpName", i))
            Next

            cboEncarg.AsignaValorDataSource(-1)

            'SERIES
            oitems = oForm.Items.Item(cboSeries.UniqueId)
            cboCombos = CType(oitems.Specific, SAPbouiCOM.ComboBox)
            cboCombos.ValidValues.LoadSeries("SCGD_PDV", SAPbouiCOM.BoSeriesMode.sf_Add)


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

            oItems = oForm.Items.Item(cboSeries.UniqueId)
            oCombo = CType(oItems.Specific, SAPbouiCOM.ComboBox)
            dim val =oCombo.ValidValues.Item(0).Value

            oCombo.Select(oCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)

            dtLocal = FormularioSBO.DataSources.DataTables.Item("local")
            dtLocal.Clear()

            l_strSQL = "Select NextNumber from nnm1 where ObjectCode = 'SCGD_PDV' and Series = '{0}'"

            l_strSerie = cboSeries.ObtieneValorDataSource()

            dtLocal.ExecuteQuery(String.Format(l_strSQL, l_strSerie))

            If Not String.IsNullOrEmpty(dtLocal.GetValue("NextNumber", 0)) Then
                txtDocNum.AsignaValorDataSource(dtLocal.GetValue("NextNumber", 0))
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Function LigarColumnasPedidos(ByRef oMatrix As MatrizPedidoDeVehiculos)
        Dim oColumna As ColumnaMatrixSBO(Of String)

        Try

            oColumna = oMatrix.ColumnaSel
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_PEDIDOS_LINEAS", "U_Cod_Art")

            oColumna = oMatrix.ColumnaCod
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_PEDIDOS_LINEAS", "U_Desc_Art")

            oColumna = oMatrix.ColumnaAno
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_PEDIDOS_LINEAS", "U_Ano_Veh")

            oColumna = oMatrix.ColumnaCol
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_PEDIDOS_LINEAS", "U_Cod_Col")

            oColumna = oMatrix.ColumnaCan
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_PEDIDOS_LINEAS", "U_Cant")

            oColumna = oMatrix.ColumnaCos
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_PEDIDOS_LINEAS", "U_Cost_Art")

            oColumna = oMatrix.ColumnaTot
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_PEDIDOS_LINEAS", "U_Cost_Tot")

            oColumna = oMatrix.ColumnaRec
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_PEDIDOS_LINEAS", "U_Cant_Rec")

            oColumna = oMatrix.ColumnaLin
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_PEDIDOS_LINEAS", "LineId")

            oColumna = oMatrix.ColumnaLin
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_PEDIDOS_LINEAS", "U_Cerrada")

            Return True

        Catch ex As Exception
            ' Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Private Sub AddChooseFromList(ByVal oform As SAPbouiCOM.Form, ByVal ObjectType As String, ByVal UniqueID As String)
        Try

            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection

            oCFLs = oform.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = ObjectType
            oCFLCreationParams.UniqueID = UniqueID

            oCFL = oCFLs.Add(oCFLCreationParams)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub AsignaCFLColumn(ByVal p_strMatriz As String, ByVal p_strColumn As String, ByVal p_strCFL As String, ByVal p_Alias As String)
        Try
            Dim oitem As SAPbouiCOM.Item
            Dim oMatrix As SAPbouiCOM.Matrix

            oitem = FormularioSBO.Items.Item(p_strMatriz)
            oMatrix = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)

            oMatrix.Columns.Item(p_strColumn).ChooseFromListUID = p_strCFL
            oMatrix.Columns.Item(p_strColumn).ChooseFromListAlias = p_Alias
            '-----------------------------------------------
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Sub

#End Region






End Class


