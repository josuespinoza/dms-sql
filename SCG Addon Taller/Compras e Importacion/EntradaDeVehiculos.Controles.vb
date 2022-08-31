Imports System.Globalization
Imports System.Threading
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class EntradaDeVehiculos : Implements IFormularioSBO


    'maneja informacion de la aplicacion
    Private WithEvents _applicationSbo As Application
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
    Private m_blnUsaDimensiones As Boolean = False
    Private m_blnUsaCostoAuto As Boolean = False
    Private m_strCodTipoTransAuto As String = String.Empty

    Private txtCodProv As EditTextSBO
    Private txtNamProv As EditTextSBO
    Private txtEmbarque As EditTextSBO
    Private txtNamBarco As EditTextSBO
    Private txtBooking As EditTextSBO
    Private txtFhaEmbarque As EditTextSBO
    Private txtFhaArribo As EditTextSBO
    Private txtDocEntry As EditTextSBO
    Private txtEstado As EditTextSBO
    Private txtObserv As EditTextSBO
    Private txtCantidad As EditTextSBO
    Private txtTotal As EditTextSBO
    Private txtTipoC As EditTextSBO
    Private txtFhaDoc As EditTextSBO
    Private txtDocNum As EditTextSBO
    Private txtFhaCont As EditTextSBO

    Private txtPrefijo As EditTextSBO
    Private txtConsecutivo As EditTextSBO

    Private cboUbica As ComboBoxSBO
    Private cboDisponibilidad As ComboBoxSBO
    Private cboMoneda As ComboBoxSBO

    Private cboContact As ComboBoxSBO
    Private cboSerie As ComboBoxSBO
    Private cboEstadoDoc As ComboBoxSBO
    Private cboTipoInv As ComboBoxSBO

    Private btnAddUnid As ButtonSBO
    Private btnDelUnid As ButtonSBO

    Private btnAddPed As ButtonSBO
    Private btnDelPed As ButtonSBO
    Private btnCosteo As ButtonSBO

    Private btnCrea As ButtonSBO
    Private btnGenera As ButtonSBO
    Private btnActualiza As ButtonSBO

    Public FolderPedidos As FolderSBO
    Public FolderUnidades As FolderSBO

    Private cbxCancelado As CheckBoxSBO

    Private m_strTableEntrada As String = "@SCGD_ENTRADA_VEH"
    Private m_strTableEntradaPed As String = "@SCGD_ENTRADA_LINEAS"
    Private m_strTableEntradaVehi As String = "@SCGD_ENTRADA_UNID"

    Private MatrixEntradaVeh As MatrizEntradaVehiculos
    Private MatrixEntradaPed As MatrizEntradaPedido

    Private dtLocal As DataTable
    Private dtLocal2 As DataTable
    Private dtModelo As DataTable
    Private dtModeloLocal As DataTable
    Private dtEstilo As DataTable
    Private dtEstiloLocal As DataTable
    Private dtDimensiones As DataTable
    Private oForm As Form
    Public m_oFormularioSeleccionLineasPedidos As SeleccionLineasPedidos
    Public m_oGestorFormularios As GestorFormularios

    Private boolCambiarMoneda As Boolean = False

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
    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_strEntradaDeVehiculos As String)

        _companySbo = companySbo
        _applicationSbo = application
        n = DIHelper.GetNumberFormatInfo(_companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioEntradaDeVehiculos
        MenuPadre = "SCGD_CEIM"
        Nombre = My.Resources.Resource.SubMenuEntradaVehi
        IdMenu = p_strEntradaDeVehiculos
        Titulo = My.Resources.Resource.SubMenuEntradaVehi
        Posicion = 2
        FormType = p_strEntradaDeVehiculos

    End Sub

#End Region

#Region "Metodos / Funciones"
    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario

        oForm = ApplicationSBO.Forms.Item("SCGD_EDV")
        CargarFormulario()

        l_oSeleccionMarcaEstilo = New VehiculoSeleccionMarcaEstilo(_companySbo, _applicationSbo)

    End Sub

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles
        Try

            txtCodProv = New EditTextSBO("txtCodProv", True, m_strTableEntrada, "U_Cod_Prov", FormularioSBO)
            txtNamProv = New EditTextSBO("txtNamProv", True, m_strTableEntrada, "U_Name_Prov", FormularioSBO)

            txtEmbarque = New EditTextSBO("txtEmbarq", True, m_strTableEntrada, "U_Num_Embarque", FormularioSBO)
            txtNamBarco = New EditTextSBO("txtBarco", True, m_strTableEntrada, "U_Nombre_Barco", FormularioSBO)
            txtBooking = New EditTextSBO("txtBook", True, m_strTableEntrada, "U_Num_Booking", FormularioSBO)
            txtFhaEmbarque = New EditTextSBO("txtFhaEmb", True, m_strTableEntrada, "U_Fha_Embarque", FormularioSBO)
            txtFhaArribo = New EditTextSBO("txtFhaEst", True, m_strTableEntrada, "U_Fha_Est_Arribo", FormularioSBO)
            txtDocEntry = New EditTextSBO("txtDocEnt", True, m_strTableEntrada, "DocEntry", FormularioSBO)
            ' txtEstado = New EditTextSBO("", True, m_strTableEntradaVeh, "U_Estado_Veh", FormularioSBO)
            txtObserv = New EditTextSBO("txtObserv", True, m_strTableEntrada, "U_Observ", FormularioSBO)
            txtCantidad = New EditTextSBO("txtCant", True, m_strTableEntrada, "U_Cant_Veh", FormularioSBO)
            txtTotal = New EditTextSBO("txtTotal", True, m_strTableEntrada, "U_Total_Doc", FormularioSBO)
            txtTipoC = New EditTextSBO("txtTipoC", True, m_strTableEntrada, "U_TipoCambio", FormularioSBO)
            txtFhaDoc = New EditTextSBO("txtFhaDoc", True, m_strTableEntrada, "U_Fha_Doc", FormularioSBO)
            txtDocNum = New EditTextSBO("txtDocNum", True, m_strTableEntrada, "DocNum", FormularioSBO)
            txtFhaCont = New EditTextSBO("txtFhaCont", True, m_strTableEntrada, "U_Fha_Cont", FormularioSBO)

            cboUbica = New ComboBoxSBO("cboUbic", FormularioSBO, True, m_strTableEntrada, "U_Ubicacion_Veh")
            cboDisponibilidad = New ComboBoxSBO("cboEstadoV", FormularioSBO, True, m_strTableEntrada, "U_Estado_Veh")
            cboMoneda = New ComboBoxSBO("cboMoneda", FormularioSBO, True, m_strTableEntrada, "U_Moneda")
            cboContact = New ComboBoxSBO("cboContact", FormularioSBO, True, m_strTableEntrada, "U_Contact")
            cboSerie = New ComboBoxSBO("cboSeries", FormularioSBO, True, m_strTableEntrada, "Series")
            cboEstadoDoc = New ComboBoxSBO("cboEstado", FormularioSBO, True, m_strTableEntrada, "Status")
            cboTipoInv = New ComboBoxSBO("cboTipoInv", FormularioSBO, True, m_strTableEntrada, "U_TipoInv")

            cbxCancelado = New CheckBoxSBO("cbxCancel", True, m_strTableEntrada, "Canceled", FormularioSBO)

            btnAddPed = New ButtonSBO("btnAddPed", FormularioSBO)
            btnDelPed = New ButtonSBO("btnDelPed", FormularioSBO)
            btnCosteo = New ButtonSBO("btnCosteo", FormularioSBO)
            btnAddUnid = New ButtonSBO("btnAddVeh", FormularioSBO)
            btnDelUnid = New ButtonSBO("btnDelVeh", FormularioSBO)

            btnGenera = New ButtonSBO("btnGen", FormularioSBO)
            btnCrea = New ButtonSBO("btnCrea", FormularioSBO)
            btnActualiza = New ButtonSBO("btnAct", FormularioSBO)

            txtCodProv.AsignaBinding()
            txtNamProv.AsignaBinding()
            txtEmbarque.AsignaBinding()
            txtNamBarco.AsignaBinding()
            txtBooking.AsignaBinding()
            txtFhaEmbarque.AsignaBinding()
            txtFhaArribo.AsignaBinding()
            txtDocEntry.AsignaBinding()
            txtObserv.AsignaBinding()
            txtCantidad.AsignaBinding()
            txtTotal.AsignaBinding()
            txtTipoC.AsignaBinding()
            txtFhaDoc.AsignaBinding()
            txtDocNum.AsignaBinding()
            txtFhaCont.AsignaBinding()

            cboUbica.AsignaBinding()
            cboDisponibilidad.AsignaBinding()
            cboMoneda.AsignaBinding()
            cboContact.AsignaBinding()
            cboSerie.AsignaBinding()
            cboEstadoDoc.AsignaBinding()
            cboTipoInv.AsignaBinding()

            cbxCancelado.AsignaBinding()

            FolderPedidos = New FolderSBO("Folder1", FormularioSBO)
            FolderUnidades = New FolderSBO("Folder2", FormularioSBO)

            Dim userDS As UserDataSources = FormularioSBO.DataSources.UserDataSources

            userDS.Add("pref", BoDataType.dt_LONG_TEXT, 100)
            userDS.Add("cons", BoDataType.dt_LONG_NUMBER, 100)

            txtPrefijo = New EditTextSBO("txtPref", True, "", "pref", FormularioSBO)
            txtConsecutivo = New EditTextSBO("txtConsec", True, "", "cons", FormularioSBO)

            txtPrefijo.AsignaBinding()
            txtConsecutivo.AsignaBinding()


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub Cargardtlocales()

        Dim sql1 As String = "SELECT Code, Name, U_Cod_Marc FROM [@SCGD_ESTILO] with (nolock)"
        Dim sql2 As String = "SELECT Code,Name,U_Cod_Esti,U_Descripcion ,U_CodigoFabrica FROM [@SCGD_MODELO] with (nolock)"

        dtEstilo = _formularioSBO.DataSources.DataTables.Item("dtEstilo")
        dtEstilo.Clear()
        dtEstilo.ExecuteQuery(sql1)

        dtModelo = _formularioSBO.DataSources.DataTables.Item("dtModelo")
        dtModelo.Clear()
        dtModelo.ExecuteQuery(sql2)

        dtEstiloLocal = _formularioSBO.DataSources.DataTables.Item("dtEstiloLocal")
        dtEstiloLocal.Columns.Add("Code", BoFieldsType.ft_AlphaNumeric)
        dtEstiloLocal.Columns.Add("Name", BoFieldsType.ft_AlphaNumeric)

        dtModeloLocal = _formularioSBO.DataSources.DataTables.Item("dtModeloLocal")
        dtModeloLocal.Columns.Add("Code", BoFieldsType.ft_AlphaNumeric)
        dtModeloLocal.Columns.Add("Name", BoFieldsType.ft_AlphaNumeric)


    End Sub

    Public Sub CargarFormulario()
        Try
            FormularioSBO.Freeze(True)

            dtLocal = FormularioSBO.DataSources.DataTables.Add("dtLocal")
            dtLocal2 = FormularioSBO.DataSources.DataTables.Add("dtLocal2")
            dtModelo = FormularioSBO.DataSources.DataTables.Add("dtModelo")
            dtModeloLocal = FormularioSBO.DataSources.DataTables.Add("dtModeloLocal")
            dtEstilo = FormularioSBO.DataSources.DataTables.Add("dtEstilo")
            dtEstiloLocal = FormularioSBO.DataSources.DataTables.Add("dtEstiloLocal")
            dtDimensiones = FormularioSBO.DataSources.DataTables.Add("dtDimensiones")
            
            Cargardtlocales()

            m_TablePedidos = FormularioSBO.DataSources.DataTables.Add("dtPedidos")

            MatrixEntradaVeh = New MatrizEntradaVehiculos("mtx_Unidad", FormularioSBO, m_strTableEntradaVehi)
            MatrixEntradaVeh.CreaColumnas()
            LigarColumnasVehiculos(MatrixEntradaVeh)

            MatrixEntradaPed = New MatrizEntradaPedido("mtx_Pedido", FormularioSBO, m_strTableEntradaPed)
            MatrixEntradaPed.CreaColumnas()
            LigarColumnasPedidos(MatrixEntradaPed)

            CargarCombos()
            CargarSerieDocumento()
            CargarMonedaLocal()
            CargaDatosIniciales()

            ManejoBtnUnidades(1)
            txtFhaDoc.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd", n))


                ManejoControlesCosteoAutomatico()

                FormularioSBO.Items.Item(btnActualiza.UniqueId).Enabled = False
                MatrixEntradaVeh.Matrix.Columns.Item(MatrixEntradaVeh.ColumnaColCod.UniqueId).Editable = True
                MatrixEntradaVeh.Matrix.Columns.Item(MatrixEntradaVeh.ColumnaColTip.UniqueId).Editable = True

                oForm.Items.Item("Folder1").Click()

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub ManejoControlesCosteoAutomatico()
        Try
            Dim l_strSQL As String = ""

            If m_blnUsaCostoAuto Then

                MatrixEntradaVeh.Matrix.Columns.Item(MatrixEntradaVeh.ColumnaColCos.UniqueId).Visible = True

                _formularioSBO.Items.Item(txtFhaCont.UniqueId).Visible = True
                _formularioSBO.Items.Item("64").Visible = True

            Else
                MatrixEntradaVeh.Matrix.Columns.Item(MatrixEntradaVeh.ColumnaColEnt.UniqueId).Visible = False
                MatrixEntradaVeh.Matrix.Columns.Item(MatrixEntradaVeh.ColumnaColAsi.UniqueId).Visible = False
                ' MatrixEntradaVeh.Matrix.Columns.Item(MatrixEntradaVeh.ColumnaColTra.UniqueId).Visible = False

                _formularioSBO.Items.Item("64").Visible = False
                _formularioSBO.Items.Item(txtFhaCont.UniqueId).Visible = False
                _formularioSBO.Items.Item(btnCosteo.UniqueId).Visible = False

            End If


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub CargaDatosIniciales()
        Try
            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            dtLocal.ExecuteQuery("Select Code, U_UsaDimC, U_UsaCostAuto, U_TipoTransCostAuto from dbo.[@SCGD_ADMIN] WITH (nolock)")
            If Not String.IsNullOrEmpty(dtLocal.GetValue("Code", 0)) Then
                If Not String.IsNullOrEmpty(dtLocal.GetValue("U_UsaDimC", 0)) Then
                    If dtLocal.GetValue("U_UsaDimC", 0).Equals("Y") Then
                        m_blnUsaDimensiones = True
                    Else
                        m_blnUsaDimensiones = False
                    End If
                End If

                If Not String.IsNullOrEmpty(dtLocal.GetValue("U_UsaCostAuto", 0)) Then
                    If dtLocal.GetValue("U_UsaCostAuto", 0).Equals("Y") Then
                        m_blnUsaCostoAuto = True
                    Else
                        m_blnUsaCostoAuto = False
                    End If
                End If
                If Not String.IsNullOrEmpty(dtLocal.GetValue("U_TipoTransCostAuto", 0)) Then
                    m_strCodTipoTransAuto = dtLocal.GetValue("U_TipoTransCostAuto", 0)
                Else
                    m_strCodTipoTransAuto = String.Empty
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub AsignarCFLButton(ByVal p_strControl As String, ByVal p_strCFL As String)

        Try

            Dim oitem As SAPbouiCOM.Item
            Dim oButton As SAPbouiCOM.Button

            oitem = FormularioSBO.Items.Item(p_strControl)
            oButton = CType(oitem.Specific, SAPbouiCOM.Button)

            oButton.Type = SAPbouiCOM.BoButtonTypes.bt_Caption
            oButton.ChooseFromListUID = p_strCFL

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

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

    Public Sub CargarCombos()
        Try
            Dim oMatriz As SAPbouiCOM.Matrix
            Dim oItems As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox


            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")

            '----------------- MATRIZ PEDIDOS ----------------- 
            oMatriz = DirectCast(oForm.Items.Item("mtx_Pedido").Specific, SAPbouiCOM.Matrix)

            dtLocal.Clear()
            dtLocal.ExecuteQuery("select code, name from [@SCGD_COLOR] with (nolock)")

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oMatriz.Columns.Item("col_Col").ValidValues.Add(dtLocal.GetValue("code", i), dtLocal.GetValue("name", i))
            Next

            '----------------- MATRIZ UNIDADES ----------------- 

            oMatriz = DirectCast(oForm.Items.Item("mtx_Unidad").Specific, SAPbouiCOM.Matrix)

            dtLocal.Clear()
            dtLocal.ExecuteQuery("select code, name from [@SCGD_COLOR] with (nolock)")
            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oMatriz.Columns.Item("col_Col").ValidValues.Add(dtLocal.GetValue("code", i), dtLocal.GetValue("name", i))
            Next

            dtLocal.Clear()
            dtLocal.ExecuteQuery("SELECT Code, Name , U_AgContV  FROM [@SCGD_DISPONIBILIDAD] with (nolock)")
            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oMatriz.Columns.Item("col_Esta").ValidValues.Add(dtLocal.GetValue("Code", i), dtLocal.GetValue("Name", i))
            Next

            dtLocal.Clear()
            dtLocal.ExecuteQuery("SELECT Code,Name  from [@SCGD_UBICACIONES] with (nolock)")
            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oMatriz.Columns.Item("col_Ubic").ValidValues.Add(dtLocal.GetValue("Code", i), dtLocal.GetValue("Name", i))
            Next


            dtLocal.Clear()
            dtLocal.ExecuteQuery(" SELECT Code, Name, U_Usado  FROM [@SCGD_TIPOVEHICULO] with (nolock) where U_Usado <> 'Y' ")

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oMatriz.Columns.Item("col_Tipo").ValidValues.Add(dtLocal.GetValue("Code", i), dtLocal.GetValue("Name", i))
            Next

            '----------------- ENCABEZADO ----------------- 
            dtLocal.Clear()
            dtLocal.ExecuteQuery("SELECT Code, Name , U_AgContV  FROM [@SCGD_DISPONIBILIDAD] with (nolock)")

            oItems = oForm.Items.Item(cboDisponibilidad.UniqueId)
            oCombo = CType(oItems.Specific, SAPbouiCOM.ComboBox)
            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oCombo.ValidValues.Add(dtLocal.GetValue("Code", i), dtLocal.GetValue("Name", i))
            Next

            dtLocal.Clear()
            dtLocal.ExecuteQuery("SELECT Code,Name  from [@SCGD_UBICACIONES] with (nolock)")

            oItems = oForm.Items.Item(cboUbica.UniqueId)
            oCombo = CType(oItems.Specific, SAPbouiCOM.ComboBox)
            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oCombo.ValidValues.Add(dtLocal.GetValue("Code", i), dtLocal.GetValue("Name", i))
            Next

            'TIPO DE INVENTARIO
            dtLocal.Clear()
            dtLocal.ExecuteQuery("SELECT Code,Name  from [@SCGD_TIPOVEHICULO] with (nolock) WHERE U_Usado <> 'Y'")

            oItems = oForm.Items.Item(cboTipoInv.UniqueId)
            oCombo = CType(oItems.Specific, SAPbouiCOM.ComboBox)
            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oCombo.ValidValues.Add(dtLocal.GetValue("Code", i), dtLocal.GetValue("Name", i))
            Next


            'MONEDA
            dtLocal.Clear()
            dtLocal.ExecuteQuery("select CurrCode, CurrName from OCRN with (nolock)")

            oItems = oForm.Items.Item(cboMoneda.UniqueId)
            oCombo = CType(oItems.Specific, SAPbouiCOM.ComboBox)

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oCombo.ValidValues.Add(dtLocal.GetValue("CurrCode", i), dtLocal.GetValue("CurrName", i))
            Next


            'SERIES
            oItems = oForm.Items.Item(cboSerie.UniqueId)
            oCombo = CType(oItems.Specific, SAPbouiCOM.ComboBox)
            oCombo.ValidValues.LoadSeries("SCGD_EDV", SAPbouiCOM.BoSeriesMode.sf_Add)

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

            oItems = oForm.Items.Item(cboSerie.UniqueId)
            oCombo = CType(oItems.Specific, SAPbouiCOM.ComboBox)
            oForm.Select()
            oCombo.Active = True
            oCombo.Select(oCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()
            ' l_strSerie = cboSerie.ObtieneValorDataSource()
            l_strSerie = oCombo.Value.Trim

            l_strSQL = "Select NextNumber from nnm1 where ObjectCode = 'SCGD_EDV' and Series = '{0}'"

            dtLocal.ExecuteQuery(String.Format(l_strSQL, l_strSerie))

            If Not String.IsNullOrEmpty(dtLocal.GetValue("NextNumber", 0)) Then
                txtDocNum.AsignaValorDataSource(dtLocal.GetValue("NextNumber", 0))
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub CargarMonedaLocal(Optional ByVal p_blnNuevo As Boolean = True)
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

                m_strMonLocal = l_strMonLocal
                m_strMonSistema = l_strMonSist
            End If

            FormularioSBO.Items.Item(cboMoneda.UniqueId).Visible = True
            FormularioSBO.Items.Item(txtTipoC.UniqueId).Visible = False

            If p_blnNuevo Then
                cboMoneda.AsignaValorDataSource(l_strMonLocal)
                txtTipoC.AsignaValorDataSource(1)
            Else
                If cboMoneda.ObtieneValorDataSource <> l_strMonLocal Then
                    FormularioSBO.Items.Item(txtTipoC.UniqueId).Visible = True
                Else
                    cboMoneda.AsignaValorDataSource(l_strMonLocal)
                    txtTipoC.AsignaValorDataSource(1)
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Function CargaTipoCambio()
        Try
            Dim l_strSQLTipoC As String
            Dim l_StrSQLSys As String
            Dim l_FhaConta As Date

            Dim decTC As Decimal
            Dim strTC As String

            Dim l_strMonLocal As String
            Dim l_StrMonSist As String
            Dim l_StrMoneda As String

            l_strSQLTipoC = "select RateDate, Currency, Rate  from ORTT where RateDate = '{0}' and Currency = '{1}'"
            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"
            l_StrMoneda = cboMoneda.ObtieneValorDataSource()

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()
            dtLocal.ExecuteQuery(l_StrSQLSys)

            If Not String.IsNullOrEmpty(dtLocal.GetValue("MainCurncy", 0)) Then
                l_strMonLocal = dtLocal.GetValue("MainCurncy", 0)
                l_StrMonSist = dtLocal.GetValue("SysCurrncy", 0)
            End If

            If l_StrMoneda = l_strMonLocal Then
                txtTipoC.AsignaValorDataSource(1)
            ElseIf l_StrMoneda = l_StrMonSist Then
                If Not String.IsNullOrEmpty(txtFhaDoc.ObtieneValorDataSource) Then
                    l_FhaConta = DateTime.ParseExact(txtFhaDoc.ObtieneValorDataSource, "yyyyMMdd", Nothing)
                Else
                    l_FhaConta = Date.Now
                End If

                l_strSQLTipoC = String.Format(l_strSQLTipoC, Utilitarios.RetornaFechaFormatoDB(l_FhaConta, _companySbo.Server), cboMoneda.ObtieneValorDataSource)

                dtLocal.Clear()
                dtLocal.ExecuteQuery(l_strSQLTipoC)

                If String.IsNullOrEmpty(dtLocal.GetValue("Rate", 0)) OrElse dtLocal.GetValue("Rate", 0) = 0 Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorTipoCambioDoc, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    cboMoneda.AsignaValorDataSource(m_strMonedaOrigen)
                Else
                    strTC = dtLocal.GetValue("Rate", 0)
                    decTC = Decimal.Parse(strTC)

                    FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).SetValue("U_TipoCambio", 0, decTC.ToString(n))

                End If
                FormularioSBO.Items.Item(txtTipoC.UniqueId).Visible = True
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

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
                ManejadorEventoClick(FormUID, pVal, BubbleEvent)
            Case BoEventTypes.et_FORM_ACTIVATE
                ManejadorEventoFormActivate(FormUID, pVal, BubbleEvent)


        End Select

    End Sub

    Public Sub ManejadorEventoFormActivate(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If boolCambiarMoneda Then
            CambiarMoneda()
        End If
    End Sub

    Private Function LigarColumnasVehiculos(ByRef oMatrix As MatrizEntradaVehiculos)
        Dim oColumna As ColumnaMatrixSBO(Of String)
        Dim oColumna2 As ColumnaMatrixSBO(Of Decimal)
        Try

            oColumna = oMatrix.ColumnaColPed
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Num_Ped")

            oColumna = oMatrix.ColumnaColCod
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Cod_Uni")

            oColumna = oMatrix.ColumnaColMar
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Cod_Mar")

            oColumna = oMatrix.ColumnaColEst
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Cod_Est")

            oColumna = oMatrix.ColumnaColMod
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Cod_Mod")

            oColumna = oMatrix.ColumnaColVin
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Num_VIN")

            oColumna = oMatrix.ColumnaColMot
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Num_Mot")

            oColumna = oMatrix.ColumnaColUbi
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Cod_Ubi")

            oColumna = oMatrix.ColumnaColSta
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Estado")

            oColumna = oMatrix.ColumnaColAno
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Ano_Veh")

            oColumna = oMatrix.ColumnaColTip
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Cod_Tip")

            oColumna = oMatrix.ColumnaColCol
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Cod_Col")

            oColumna = oMatrix.ColumnaColRef
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Line_Ref")

            oColumna = oMatrix.ColumnaColArt
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Cod_Art")

            oColumna = oMatrix.ColumnaColID
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_ID_Veh")

            oColumna2 = oMatrix.ColumnaColCos
            oColumna2.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Monto_Gr")

            'oColumna = oMatrix.ColumnaColTra
            'oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Tipo_Trans")

            oColumna = oMatrix.ColumnaColAsi
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Num_Asiento")

            oColumna = oMatrix.ColumnaColEnt
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Num_Entrada")

            oColumna = oMatrix.ColumnaColDesMar
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Des_Mar")

            oColumna = oMatrix.ColumnaColDesEst
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Des_Est")

            oColumna = oMatrix.ColumnaColDesMod
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_UNID", "U_Des_Mod")

            Return True

        Catch ex As Exception
            ' Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function LigarColumnasPedidos(ByRef oMatrix As MatrizEntradaPedido)
        Dim oColumna As ColumnaMatrixSBO(Of String)

        Try
            oColumna = oMatrix.ColumnaPed
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_LINEAS", "U_Num_Ped")

            oColumna = oMatrix.ColumnaCodArt
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_LINEAS", "U_Cod_Art")

            oColumna = oMatrix.ColumnaDesArt
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_LINEAS", "U_Desc_Art")

            oColumna = oMatrix.ColumnaCodCol
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_LINEAS", "U_Cod_Col")

            oColumna = oMatrix.ColumnaAno
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_LINEAS", "U_Ano_Veh")

            oColumna = oMatrix.ColumnaCanR
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_LINEAS", "U_Cant_Ent")

            oColumna = oMatrix.ColumnaCanS
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_LINEAS", "U_Cant_Veh")

            oColumna = oMatrix.ColumnaCost
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_LINEAS", "U_Cost_Veh")

            oColumna = oMatrix.ColumnaTot
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_LINEAS", "U_Total_L")

            oColumna = oMatrix.ColumnaRef
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_ENTRADA_LINEAS", "U_Line_Ref")


            Return True

        Catch ex As Exception
            ' Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Private Sub CambiarMoneda()
        Try
            boolCambiarMoneda = False
            If Not String.IsNullOrEmpty(m_strMonedaOrigen) Then

                m_strMonedaDestino = cboMoneda.ObtieneValorDataSource()

                If m_strMonedaOrigen <> m_strMonedaDestino Then
                    If ManejaTipoCambio(True) Then
                        m_decTCDestino = Decimal.Parse(txtTipoC.ObtieneValorDataSource(), n)

                        ManejoCambioMoneda()
                        ActualizaCostosValores()

                    End If
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try


    End Sub

    Private Function AgregaButtonPic(ByRef oform As SAPbouiCOM.Form, _
                                   ByVal strNombrectrl As String, _
                                   ByVal intLeft As Integer, _
                                   ByVal intTop As Integer, _
                                   ByVal intFromPane As Integer, _
                                   ByVal intTopane As Integer, _
                                   ByVal ButtonType As SAPbouiCOM.BoButtonTypes, _
          ByVal UDO As String) As SAPbouiCOM.Item

        Dim oitem As SAPbouiCOM.Item
        Dim oButton As SAPbouiCOM.Button
        Try

            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_BUTTON)
            oitem.Left = intLeft
            oitem.Top = intTop
            oButton = oitem.Specific
            oButton.Type = ButtonType
            oitem.Width = 78
            oitem.Height = 19
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane
            oButton.Caption = "Copiar Pedidos."
            '  oButton.Image = PathImagen

            If UDO <> "" Then
                oButton.ChooseFromListUID = UDO
            End If

            Return oitem
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        End Try

    End Function

    Private Sub AddChooseFromList(ByVal oform As SAPbouiCOM.Form)
        Try

            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection

            oCFLs = oform.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            ' Adding 2 CFL, one for the button and one for the edit text.
            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = "SCGD_PDV"
            oCFLCreationParams.UniqueID = "CFL_Ped"
            oCFL = oCFLs.Add(oCFLCreationParams)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Sub CargarFormularioModoAdd()
        Try

            FormularioSBO.Freeze(True)

            FormularioSBO.Mode = BoFormMode.fm_ADD_MODE

            Call CargarSerieDocumento()
            Call CargarMonedaLocal()

            'ManejoBtnCrearUnidades(EnableBtn.Mostrar)
            ManejoBtnUnidades(1)

            MatrixEntradaVeh.Matrix.Columns.Item(MatrixEntradaVeh.ColumnaColCod.UniqueId).Editable = True
            MatrixEntradaVeh.Matrix.Columns.Item(MatrixEntradaVeh.ColumnaColTip.UniqueId).Editable = True

            'FormularioSBO.Items.Item(btnActualiza.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)
            ' FormularioSBO.Items.Item(btnCrea.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)
            FormularioSBO.Items.Item(cboSerie.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_True)
            txtFhaDoc.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd", n))
            oForm.Items.Item("Folder1").Click()

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub CargarFormularioSeleccionPedidos(Optional ByVal p_strCodProveedor As String = "")
        Try

            m_oGestorFormularios = New GestorFormularios(_applicationSbo)

            m_oFormularioSeleccionLineasPedidos = New SeleccionLineasPedidos(_applicationSbo, CompanySBO)
            DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)
            m_oFormularioSeleccionLineasPedidos.NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioSeleccionLineasPedidos

            m_oFormularioSeleccionLineasPedidos.FormType = "SCGD_SLP"
            m_oFormularioSeleccionLineasPedidos.CodProveedor = p_strCodProveedor

            If m_oGestorFormularios.FormularioAbierto(m_oFormularioSeleccionLineasPedidos, True) = False Then

                m_oGestorFormularios.CargaFormulario(m_oFormularioSeleccionLineasPedidos)

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub


#End Region


End Class


