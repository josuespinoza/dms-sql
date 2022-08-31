
'*******************************************
'*Maneja los controles del formulario SeleccionarRepuestosOT
'*******************************************

Imports System.Globalization
Imports System.Linq
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class SeleccionarRepuestosOT
    : Implements IFormularioSBO

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
    Private _oIncluirRepuestosOT As IncluirRepuestosOT
    Private _idMenu As String
    Private _menuPadre As String
    Private _nombre As String
    Private _posicion As Integer

    Private _codeCliente As String

    'tabla para repuestos
    Private dtRepuestos As DataTable
    Private dtRepuestosTodos As DataTable
    Public dtConf As System.Data.DataTable
    Public g_oMatrix As SAPbouiCOM.Matrix
    Public g_oForm As Form
    Private dtCodeEstiMod As System.Data.DataTable
    Private Const strDataTable As String = "tRepuestosSeleccionados"
    Private Const strMatrizRep As String = "mtxRep"
    Private Const strDataTableTodos As String = "tTodosRepuestos"
    Private Const strMatrizRepTodos As String = "mtxLsRep"
    'matriz repuestos
    Private MatrizRepuestosSeleccionados As MatrizSeleccionaRepuestosOT
    Private MatrizRepuestosTodos As MatrizSeleccionaRepuestosOT

    'controles de interfaz
    Private Shared txtCod As EditTextSBO
    Private Shared txtDes As EditTextSBO
    Private Shared txtCodBar As EditTextSBO

    Private Shared cboGrp As ComboBoxSBO
    Private Shared cboModEst As ComboBoxSBO
    Private Shared cboFam As ComboBoxSBO
    Private Shared cboPro As ComboBoxSBO

    Private Shared chkModEst As CheckBoxSBO
    Private Shared chkGrp As CheckBoxSBO
    Private Shared chkFam As CheckBoxSBO
    Private Shared chkPro As CheckBoxSBO
    Public Shared g_strNoOT As String
    Public Shared g_strEsti As String
    Public Shared g_strMod As String
    Public Shared g_strConsultaListaPreciosCliente As String = "Select ListNum from OCRD with(nolock) where CardCode = '{0}'"
    Public Shared g_strUsaListaPrecCliente As String
    Public Shared g_strListaPreciosConfigurada As String
    Public Shared g_strDocEntry As String
    Public Shared g_strCodListPrecio As String
    Public Shared strConfiguracion As String
    Private Shared g_strSucursal As String
    Public Shared dtConfAdmin As System.Data.DataTable
    Public Shared g_strUsaAsocxEspecif As String = ""
    Public Shared mBExisteArt As Boolean = False
    Public Shared g_strEspecifVehi As String = ""
    Public Shared g_strConsultaConfiguracionList As String
    Public Shared m_strCodEstilo As String = ""
    Public Shared m_strCodModelo As String = ""
    Public Shared g_strUsaFilRep As String
    Public Shared g_strConsulta As String
    Public Shared g_strConsultaArtiEspXModeEsti As String
    Public Shared g_strUsaConsultaSegunConf As String
    Public Shared m_strConsulta As String = " select top(100) '' as sel, oi.ItemCode, cfnb.U_Rep as bod, " +
                                   " ow.OnHand  as stk, oi.ItemName, " +
                                   " 1 as cantidad, it.Price, it.Currency,oi.CodeBars " +
                                   " from OITM as oi with (nolock) " +
                                   " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " +
                                   " inner join ITM1 as it with(nolock) on oi.ItemCode = it.ItemCode   " +
                                   " inner join OITW as ow with(nolock) on ow.WhsCode = cfnb.U_Rep and ow.ItemCode = oi.ItemCode" +
                                   " where cfnb.DocEntry = ( select DocEntry from [@SCGD_CONF_SUCURSAL]  with(nolock) where U_Sucurs =  ( select U_SCGD_idSucursal from OQUT where U_SCGD_Numero_OT = '{0}'  ) ) " +
                                   " and oi.U_SCGD_TipoArticulo = '1' " +
                                   " and it.PriceList = '{1}' "

    Public Shared m_strConsultaArtiEspXModeEsti As String =
                    " select top(100) '' as sele, art.U_ItemCode as ItemCode, cfnb.U_Rep as bod, " +
                    " ow.OnHand  as stk,art.U_ItemName as ItemName, " +
                    " 1 as cantidad, it.Price, it.Currency,oi.CodeBars" +
                    " from OITM as oi with (nolock) " +
                    " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " +
                    " inner join ITM1 as it with (nolock) on oi.ItemCode = it.ItemCode   " +
                    " inner join [@SCGD_ARTXESP] as Art with(nolock) on oi.ItemCode = art.U_ItemCode  " +
                    "  inner join OITW as ow with(nolock) on ow.WhsCode = cfnb.U_Rep and ow.ItemCode = oi.ItemCode" +
                    " where  oi.U_SCGD_TipoArticulo = '1' and  it.PriceList = '{0}' and cfnb.DocEntry = '{1}'  {2}"






    'userDataSource
    Private UDS_SeleccionaRepuestos As UserDataSources

    Dim n As NumberFormatInfo

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Contructor para la aplicacion
    ''' </summary>
    ''' <param name="application"></param>
    ''' <param name="companySbo"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByRef p_oIncluirRepuestosOT As IncluirRepuestosOT)
        _companySbo = companySbo
        _applicationSbo = application
        NombreXml = System.Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLSeleccionaRepuestosOT
        Titulo = My.Resources.Resource.TituloFormularioSeleccionRepuestos
        FormType = "SCGD_SROT"
        _oIncluirRepuestosOT = p_oIncluirRepuestosOT
        n = DIHelper.GetNumberFormatInfo(_companySbo)
    End Sub

#End Region

#Region "Propiedades"
    'propiedades de la aplicación

    Public Property p_strSucursal As String
        Get
            Return g_strSucursal
        End Get
        Set(ByVal value As String)
            g_strSucursal = value
        End Set
    End Property

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

    Public Property oIncluirRepuestosOT As IncluirRepuestosOT
        Get
            Return _oIncluirRepuestosOT
        End Get
        Set(ByVal value As IncluirRepuestosOT)
            _oIncluirRepuestosOT = value
        End Set
    End Property






    'Public Property CodeCliente As String
    '    Get
    '        Return _codeCliente
    '    End Get
    '    Set(ByVal value As String)
    '        _codeCliente = value
    '    End Set
    'End Property




#End Region

#Region "Métodos"

    ''' <summary>
    ''' Inicializa el formulario
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario
        Try
            CargaFormulario()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa los controles 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles

    End Sub

    ''' <summary>
    ''' Carga el formulario
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaFormulario()
        Try

            FormularioSBO.Freeze(True)

            FormularioSBO.DataSources.DataTables.Add("local")

            'asocia controles de interfaz
            UDS_SeleccionaRepuestos = FormularioSBO.DataSources.UserDataSources
            UDS_SeleccionaRepuestos.Add("Cod", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("Des", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("selGru", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("selModEst", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("selFam", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("selPro", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("Grp", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("ModEst", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("Fam", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("Pro", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("CodeBars", BoDataType.dt_LONG_TEXT, 100)

            txtCod = New EditTextSBO("txtCod", True, "", "Cod", FormularioSBO)
            txtCod.AsignaBinding()
            txtDes = New EditTextSBO("txtDes", True, "", "Des", FormularioSBO)
            txtDes.AsignaBinding()
            txtCodBar = New EditTextSBO("txtCodBar", True, "", "CodeBars", FormularioSBO)
            txtCodBar.AsignaBinding()
            chkGrp = New CheckBoxSBO("chkGru", True, "", "selGru", FormularioSBO)
            chkGrp.AsignaBinding()
            chkFam = New CheckBoxSBO("chkFam", True, "", "selFam", FormularioSBO)
            chkFam.AsignaBinding()
            chkPro = New CheckBoxSBO("chkPro", True, "", "selPro", FormularioSBO)
            chkPro.AsignaBinding()
            cboGrp = New ComboBoxSBO("cboGrp", FormularioSBO, True, "", "Grp")
            cboGrp.AsignaBinding()
            cboFam = New ComboBoxSBO("cboFam", FormularioSBO, True, "", "Fam")
            cboFam.AsignaBinding()
            cboPro = New ComboBoxSBO("cboPro", FormularioSBO, True, "", "Pro")
            cboPro.AsignaBinding()

            'maneja estado de combos
            FormularioSBO.Items.Item("cboGrp").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            FormularioSBO.Items.Item("cboPro").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            FormularioSBO.Items.Item("cboFam").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            '' FormularioSBO.Items.Item("cboModEst").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            'matriz para todos los repuestos
            dtRepuestosTodos = FormularioSBO.DataSources.DataTables.Add(strDataTableTodos)
            dtRepuestosTodos.Columns.Add("sel", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestosTodos.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestosTodos.Columns.Add("des", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestosTodos.Columns.Add("bod", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestosTodos.Columns.Add("onH", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestosTodos.Columns.Add("can", BoFieldsType.ft_Quantity, 100)
            dtRepuestosTodos.Columns.Add("pre", BoFieldsType.ft_Price, 100)
            dtRepuestosTodos.Columns.Add("mon", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestosTodos.Columns.Add("CodBar", BoFieldsType.ft_AlphaNumeric, 100)

            'crea matriz
            MatrizRepuestosTodos = New MatrizSeleccionaRepuestosOT(strMatrizRepTodos, FormularioSBO, strDataTableTodos)
            MatrizRepuestosTodos.CreaColumnas()
            MatrizRepuestosTodos.LigaColumnas()

            'datatable para repuestos seleccionados
            dtRepuestos = FormularioSBO.DataSources.DataTables.Add(strDataTable)
            dtRepuestos.Columns.Add("sel", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("des", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("bod", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("onH", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("can", BoFieldsType.ft_Quantity, 100)
            dtRepuestos.Columns.Add("pre", BoFieldsType.ft_Price, 100)
            dtRepuestos.Columns.Add("mon", BoFieldsType.ft_AlphaNumeric, 100)
            dtRepuestos.Columns.Add("CodBar", BoFieldsType.ft_AlphaNumeric, 100)

            'crea matriz
            MatrizRepuestosSeleccionados = New MatrizSeleccionaRepuestosOT(strMatrizRep, FormularioSBO, strDataTable)
            MatrizRepuestosSeleccionados.CreaColumnas()
            MatrizRepuestosSeleccionados.LigaColumnas()

            MatrizRepuestosTodos.Matrix.Columns.Item("Col_sel").Editable = True
            MatrizRepuestosSeleccionados.Matrix.Columns.Item("Col_sel").Editable = True

            g_strEspecifVehi = DMS_Connector.Configuracion.ParamGenAddon.U_EspVehic
            g_strUsaFilRep = DMS_Connector.Configuracion.ParamGenAddon.U_UsaFilRep
            g_strUsaAsocxEspecif = DMS_Connector.Configuracion.ParamGenAddon.U_UsaAXEV
            g_strDocEntry = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(x) x.U_Sucurs = oIncluirRepuestosOT.Sucursal).DocEntry
            g_strUsaListaPrecCliente = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(x) x.U_Sucurs = oIncluirRepuestosOT.Sucursal).U_UseLisPreCli
            g_strListaPreciosConfigurada = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(x) x.U_Sucurs = oIncluirRepuestosOT.Sucursal).U_CodLisPre

            MatrizRepuestosSeleccionados.Matrix.Columns.Item("Col_pre").Editable = DMS_Connector.Configuracion.ParamGenAddon.U_RepPre = "Y"

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

#End Region

    ''' <summary>
    ''' Asocia controles con la interfaz
    ''' </summary>
    ''' <remarks></remarks>
    'Private Sub AsociaControlesInterfaz()
    '    Try
    '        UDS_SeleccionaRepuestos = FormularioSBO.DataSources.UserDataSources
    '        UDS_SeleccionaRepuestos.Add("Cod", BoDataType.dt_LONG_TEXT, 100)
    '        UDS_SeleccionaRepuestos.Add("Des", BoDataType.dt_LONG_TEXT, 100)
    '        UDS_SeleccionaRepuestos.Add("selGru", BoDataType.dt_LONG_TEXT, 100)
    '        UDS_SeleccionaRepuestos.Add("selModEst", BoDataType.dt_LONG_TEXT, 100)
    '        UDS_SeleccionaRepuestos.Add("selFam", BoDataType.dt_LONG_TEXT, 100)
    '        UDS_SeleccionaRepuestos.Add("selPro", BoDataType.dt_LONG_TEXT, 100)
    '        UDS_SeleccionaRepuestos.Add("Grp", BoDataType.dt_LONG_TEXT, 100)
    '        UDS_SeleccionaRepuestos.Add("ModEst", BoDataType.dt_LONG_TEXT, 100)
    '        UDS_SeleccionaRepuestos.Add("Fam", BoDataType.dt_LONG_TEXT, 100)
    '        UDS_SeleccionaRepuestos.Add("Pro", BoDataType.dt_LONG_TEXT, 100)
    '        UDS_SeleccionaRepuestos.Add("CodeBars", BoDataType.dt_LONG_TEXT, 100)

    '        txtCod = New EditTextSBO("txtCod", True, "", "Cod", FormularioSBO)
    '        txtCod.AsignaBinding()
    '        txtDes = New EditTextSBO("txtDes", True, "", "Des", FormularioSBO)
    '        txtDes.AsignaBinding()
    '        txtCodBar = New EditTextSBO("txtCodBar", True, "", "CodeBars", FormularioSBO)
    '        txtCodBar.AsignaBinding()

    '        chkGrp = New CheckBoxSBO("chkGru", True, "", "selGru", FormularioSBO)
    '        chkGrp.AsignaBinding()
    '        chkFam = New CheckBoxSBO("chkFam", True, "", "selFam", FormularioSBO)
    '        chkFam.AsignaBinding()
    '        chkPro = New CheckBoxSBO("chkPro", True, "", "selPro", FormularioSBO)
    '        chkPro.AsignaBinding()

    '        cboGrp = New ComboBoxSBO("cboGrp", FormularioSBO, True, "", "Grp")
    '        cboGrp.AsignaBinding()
    '        cboFam = New ComboBoxSBO("cboFam", FormularioSBO, True, "", "Fam")
    '        cboFam.AsignaBinding()
    '        cboPro = New ComboBoxSBO("cboPro", FormularioSBO, True, "", "Pro")
    '        cboPro.AsignaBinding()

    '    Catch ex As Exception
    '        Utilitarios.ManejadorErrores(ex, ApplicationSBO)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Obtiene Estilo y modelo de La OT
    ''' Obtiene Lista de Precios Configurada
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ObtieneEstiModYConfListaPrecios()
        Dim m_strConsultaEstiMod As String

        Dim m_strUsaFiltro As String
        Dim DtExistArt As System.Data.DataTable
        Dim m_strConsultaArticulos As String = "  Select U_ItemCode from [@SCGD_ARTXESP] as Art with(nolock) where U_TipoArt = '{0}' "




        Try

            If (g_strUsaListaPrecCliente.Equals("Y")) Then
                g_strCodListPrecio = Utilitarios.EjecutarConsulta(String.Format(g_strConsultaListaPreciosCliente, _oIncluirRepuestosOT.CodeCliente), CompanySBO.CompanyDB, CompanySBO.Server)

            Else
                g_strCodListPrecio = g_strListaPreciosConfigurada
            End If

            g_strNoOT = _oIncluirRepuestosOT.NoOT

            If g_strUsaAsocxEspecif.Equals("Y") Then

                m_strConsultaEstiMod = String.Format("Select U_SCGD_Cod_Estilo,U_SCGD_Cod_Modelo from OQUT with(nolock) where U_SCGD_Numero_OT = '{0}' and U_SCGD_idSucursal = '{1}'  ", g_strNoOT, Utilitarios.ObtieneIdSucursal(DMS_Connector.Company.ApplicationSBO).ToString())
                dtCodeEstiMod = Utilitarios.EjecutarConsultaDataTable(m_strConsultaEstiMod, CompanySBO.CompanyDB, CompanySBO.Server)
                g_strEsti = dtCodeEstiMod.Rows(0)("U_SCGD_Cod_Estilo").ToString().Trim()
                g_strMod = dtCodeEstiMod.Rows(0)("U_SCGD_Cod_Modelo").ToString().Trim()

                If g_strEspecifVehi.Equals("E") Then
                    m_strUsaFiltro = String.Format(" and Art.U_CodEsti='{0}' and U_TipoArt = '1' ", g_strEsti)
                    m_strConsultaArticulos = String.Format(m_strConsultaArticulos, 1)
                    m_strConsultaArticulos = m_strConsultaArticulos & m_strUsaFiltro
                    DtExistArt = Utilitarios.EjecutarConsultaDataTable(m_strConsultaArticulos, CompanySBO.CompanyDB, CompanySBO.Server)
                    If DtExistArt.Rows.Count > 0 Then
                        mBExisteArt = True
                    End If
                ElseIf g_strEspecifVehi.Equals("M") Then
                    m_strUsaFiltro = String.Format(" and Art.U_CodMod='{0}' and U_TipoArt = '1'", g_strMod)
                    m_strConsultaArticulos = String.Format(m_strConsultaArticulos, 1)
                    m_strConsultaArticulos = m_strConsultaArticulos & m_strUsaFiltro
                    DtExistArt = Utilitarios.EjecutarConsultaDataTable(m_strConsultaArticulos, CompanySBO.CompanyDB, CompanySBO.Server)
                    If DtExistArt.Rows.Count > 0 Then
                        mBExisteArt = True
                    End If
                End If

                If mBExisteArt Then
                    If g_strEspecifVehi.Equals("E") Then
                        If g_strUsaFilRep.Equals("Y") Then
                            m_strUsaFiltro = String.Format(" and Art.U_CodEsti='{0}'", g_strEsti)
                            g_strUsaConsultaSegunConf = String.Format(g_strConsultaArtiEspXModeEsti, g_strCodListPrecio, g_strDocEntry, m_strUsaFiltro)
                        Else
                            g_strUsaConsultaSegunConf = String.Format(g_strConsulta, g_strDocEntry, g_strCodListPrecio)
                        End If
                    Else
                        If g_strUsaFilRep.Equals("Y") Then
                            m_strUsaFiltro = String.Format(" and Art.U_CodMod='{0}'", g_strMod)
                            g_strUsaConsultaSegunConf = String.Format(g_strConsultaArtiEspXModeEsti, g_strCodListPrecio, g_strDocEntry, m_strUsaFiltro)
                        Else
                            g_strUsaConsultaSegunConf = String.Format(g_strConsulta, g_strDocEntry, g_strCodListPrecio)
                        End If
                    End If
                Else
                    g_strUsaConsultaSegunConf = String.Format(g_strConsulta, g_strDocEntry, g_strCodListPrecio)
                End If

            Else
                g_strUsaConsultaSegunConf = String.Format(g_strConsulta, g_strDocEntry, g_strCodListPrecio)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

End Class
