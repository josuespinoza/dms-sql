Imports System.Globalization
Imports Deklarit.Utils
Imports DMSOneFramework.SCGCommon
Imports DMS_Addon.ControlesSBO
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports SCG.DMSOne.Framework.UDOOrden
Imports SCG.SBOFramework

Public Class AvaluoUsados : Implements IUsaPermisos, IFormularioSBO

#Region "Declaraciones"

    Private Const strTablaAva = "@SCGD_AVALUOS"
    Private Const strTablaLinAva = "@SCGD_AVAVEXT"
    Private Const strDtConsulta = "dtConsulta"
    Private Const strdtQuery = "dtQuery"
    Public Const g_strdtConfSucursal As String = "tConfSuc"
    Private Const strUDFOfertaVenta As String = "U_SerOfV"
    Public Const m_strConsultaListaPreciosCliente As String = "Select ListNum from OCRD where CardCode = '{0}'"
    Public Const g_strConsultaArti As String = " select top(1) '' as sele, oi.ItemCode as code, oi.ItemName as 'desc', cfnb.U_Rep as bode, " + " (select OnHand from OITW with (nolock) where oitw.WhsCode = cfnb.U_Rep and oitw.ItemCode = oi.ItemCode) as csto, " + " 1 as cant, it.Price as prec, it.Currency as mone, oi.U_SCGD_TipoArticulo as tiar, oi.U_SCGD_CodCtroCosto as ccos " + " from OITM as oi with (nolock) " + " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " + " inner join ITM1 as it with (nolock) on oi.ItemCode = it.ItemCode   " + " where it.PriceList = '{0}' and cfnb.DocEntry = '{1}' and oi.ItemCode = '{2}' "
    Public Const g_strConsultaConfSucursal As String = " select U_DesSInv, U_Imp_Repuestos, U_Imp_Serv, U_Imp_ServExt, U_Imp_Suminis, U_Requis, U_UsaOfeVenta, U_UsaOrdVenta, U_SerOfC, U_SerOrC, U_USolOTEsp, U_ValReqPen, U_Entrega_Rep, U_FinOTCanSol, U_FOTAPen, U_TiempoEst_C, U_TiempoReal_C, U_SerInv, ISNULL(U_AsigUniMec,'N') U_AsigUniMec, U_CanOTSer, U_CanOTArAp, ISNULL(U_SolaUna,'N') as U_SolaUna " + " from [@SCGD_CONF_SUCURSAL] with (nolock) where U_Sucurs = '{0}' "
    Private Const mc_strNum_OT As String = "U_SCGD_Numero_OT"
    Private Const mc_strEstadoCot As String = "U_SCGD_Estado_Cot"
    Private Const mc_strEstadoCotID As String = "U_SCGD_Estado_CotID"

    Private _formType As String
    Private _nombreXml As String
    Private _titulo As String
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _applicationSbo As IApplication
    Private _companySbo As SAPbobsCOM.ICompany
    Private _idMenu As String
    Private _menuPadre As String
    Private _posicion As Integer
    Private _nombre As String
    Private _strConexion As String
    Private _strDireccionReportes As String
    Private _strUsuarioBD As String
    Private _strContraseñaBD As String
    Private _strCotID As String
    Private _strTipoOT As String
    Private btnCerrAva As Boolean

    Private txtCodProp As SCG.SBOFramework.UI.EditTextSBO
    Private txtCodTec As SCG.SBOFramework.UI.EditTextSBO
    Private txtNomProp As SCG.SBOFramework.UI.EditTextSBO
    Private txtNomTec As SCG.SBOFramework.UI.EditTextSBO
    Private txtCodUnid As SCG.SBOFramework.UI.EditTextSBO

    Private cboMarca As ComboBox
    Private cboEstilo As ComboBox
    Private cboModelo As ComboBox
    Private cboColor As ComboBox
    Private cboCombustible As ComboBox
    Private cboTrans As ComboBox
    Private cboSucur As ComboBox
    Private cboMone As ComboBox
    Private cboVende As ComboBox

    Private dtConsulta As DataTable
    Private dtQuery As DataTable
    Private dtExtras As DataTable

    Private mtxAva As SAPbouiCOM.Matrix

    Private oCotizacion As Documents

    Private cotCls As CotizacionCLS

    Private mtxAvaluos As MatrixAvaluos

    Private Shared n As NumberFormatInfo
    Private WithEvents m_oVehiculo As DMS_Addon.VehiculosCls
    Private m_TypeVehiculo As String = String.Empty
    Private m_TypeCountVehiculo As String

    Private strAvaDE As String
#End Region

#Region "Constructor"

    ''' <summary>
    ''' Contructor para la aplicacion
    ''' </summary>
    ''' <param name="application"></param>
    ''' <param name="companySbo"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub New(ByVal application As Application, ByVal companySbo As SAPbobsCOM.ICompany, ByVal mc_strUISCGD_FormAVA As String)
        _companySbo = companySbo
        _applicationSbo = application
        NombreXml = System.Windows.Forms.Application.StartupPath & My.Resources.Resource.XMLAvaluos
        MenuPadre = "SCGD_MNO"
        Nombre = My.Resources.Resource.TxtAvaluoUsados
        IdMenu = mc_strUISCGD_FormAVA
        Titulo = My.Resources.Resource.TxtAvaluoUsados
        Posicion = 75
        FormType = mc_strUISCGD_FormAVA
    End Sub

#End Region

#Region "Propiedades"

    Public Property AvaluoDE() As String
        Get
            Return strAvaDE
        End Get
        Set(ByVal value As String)
            strAvaDE = value
        End Set
    End Property

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

    Public Property Posicion() As Integer Implements IUsaMenu.Posicion
        Get
            Return _posicion
        End Get
        Set(ByVal value As Integer)
            _posicion = value
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

    Public ReadOnly Property ApplicationSBO() As IApplication Implements IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
        End Get
    End Property

    Public ReadOnly Property CompanySBO() As SAPbobsCOM.ICompany Implements IFormularioSBO.CompanySBO
        Get
            Return _companySbo
        End Get
    End Property

    Public Property CotID() As String
        Get
            Return _strCotID
        End Get
        Set(ByVal value As String)
            _strCotID = value
        End Set
    End Property

    Public Property TipoOT() As String
        Get
            Return _strTipoOT
        End Get
        Set(ByVal value As String)
            _strTipoOT = value
        End Set
    End Property

#End Region

#Region "Metodos"
    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario
        Dim strMonLocal As String
        Dim strMonSys As String
        If FormularioSBO IsNot Nothing Then
            FormularioSBO.Freeze(True)
            FormularioSBO.Mode = BoFormMode.fm_ADD_MODE
            btnCerrAva = False

            n = DIHelper.GetNumberFormatInfo(CompanySBO)

            m_oVehiculo = New DMS_Addon.VehiculosCls(CompanySBO, ApplicationSBO)

            dtConsulta = FormularioSBO.DataSources.DataTables.Add(strDtConsulta)
            dtQuery = FormularioSBO.DataSources.DataTables.Add(strdtQuery)
            FormularioSBO.DataSources.DataTables.Add(g_strdtConfSucursal)

            txtCodProp = New SCG.SBOFramework.UI.EditTextSBO("txtCodProp", True, strTablaAva, "U_PropCed", FormularioSBO)
            txtNomProp = New SCG.SBOFramework.UI.EditTextSBO("txtNomProp", True, strTablaAva, "U_PropNom", FormularioSBO)
            txtCodTec = New SCG.SBOFramework.UI.EditTextSBO("txtCodTec", True, strTablaAva, "U_TecCode", FormularioSBO)
            txtNomTec = New SCG.SBOFramework.UI.EditTextSBO("txtNomTec", True, strTablaAva, "U_TecNom", FormularioSBO)
            txtCodUnid = New SCG.SBOFramework.UI.EditTextSBO("txtCodUnid", True, strTablaAva, "U_CodUnid", FormularioSBO)

            cboMarca = DirectCast(FormularioSBO.Items.Item("cboMarca").Specific, ComboBox)
            cboEstilo = DirectCast(FormularioSBO.Items.Item("cboEstilo").Specific, ComboBox)
            cboModelo = DirectCast(FormularioSBO.Items.Item("cboModelo").Specific, ComboBox)
            cboColor = DirectCast(FormularioSBO.Items.Item("cboColor").Specific, ComboBox)
            cboCombustible = DirectCast(FormularioSBO.Items.Item("cboCombus").Specific, ComboBox)
            cboTrans = DirectCast(FormularioSBO.Items.Item("cboTrans").Specific, ComboBox)
            cboSucur = DirectCast(FormularioSBO.Items.Item("cboIDSuc").Specific, ComboBox)
            cboMone = DirectCast(FormularioSBO.Items.Item("cboMon").Specific, ComboBox)
            cboVende = DirectCast(FormularioSBO.Items.Item("cboVende").Specific, ComboBox)

            txtCodProp.AsignaBinding()
            txtCodTec.AsignaBinding()
            txtCodUnid.AsignaBinding()

            Utilitarios.CargarValidValuesEnCombos(cboMarca.ValidValues, "select Code, Name from [@SCGD_MARCA] with(nolock)")
            Utilitarios.CargarValidValuesEnCombos(cboColor.ValidValues, "select Code, Name from [@SCGD_COLOR] with(nolock)")
            Utilitarios.CargarValidValuesEnCombos(cboTrans.ValidValues, "select Code, Name from [@SCGD_TRANSMISION] with(nolock)")
            Utilitarios.CargarValidValuesEnCombos(cboSucur.ValidValues, " SELECT Code, Name FROM [@SCGD_SUCURSALES] with (nolock) ")
            Utilitarios.CargarValidValuesEnCombos(cboMone.ValidValues, " select CurrCode Code, CurrName Name from OCRN with (nolock) ")
            Utilitarios.CargarValidValuesEnCombos(cboMone.ValidValues, " select CurrCode Code, CurrName Name from OCRN with (nolock) ")
            Utilitarios.CargarValidValuesEnCombos(cboVende.ValidValues, " select SlpCode Code, SlpName Name from OSLP with(nolock) where Active = 'y' and Locked = 'N' ")
            Utilitarios.CargarValidValuesEnCombos(cboCombustible.ValidValues, " select Code, Name from [@SCGD_COMBUSTIBLE]P with(nolock) ")

            DMS_Connector.Helpers.GetCurrencies(strMonLocal, strMonSys)
            If cboMone.ValidValues.Count > 0 Then
                cboMone.Select(strMonSys)
            End If

            FormularioSBO.Freeze(False)
            oCotizacion = _companySbo.GetBusinessObject(BoObjectTypes.oQuotations)

            Call AgregaButtonPic(FormularioSBO, "btLkUnid", 76, 115, 0, 0, SAPbouiCOM.BoButtonTypes.bt_Image, System.Windows.Forms.Application.StartupPath.ToString & "\Flecha.BMP", "")
            'LinkMatriz()
            'CargaMatrix()
        End If
    End Sub

    Private Sub LinkMatriz()

        dtExtras = FormularioSBO.DataSources.DataTables.Add("dtVehiExt")
        dtExtras.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100)
        dtExtras.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100)
        dtExtras.Columns.Add("chk", BoFieldsType.ft_AlphaNumeric, 100)
        dtExtras.Columns.Add("obs", BoFieldsType.ft_AlphaNumeric, 100)

        mtxAvaluos = New MatrixAvaluos("mtxExt", FormularioSBO, "dtVehiExt")
        mtxAvaluos.CreaColumnas()
        mtxAvaluos.LigaColumnas()

    End Sub

    Private Sub CargaMatrix()
        dtExtras = FormularioSBO.DataSources.DataTables.Item("dtVehiExt")
        dtExtras.ExecuteQuery("select v.Code as code, v.Name as [desc], 'N' as chk, '' as obs  from [@SCGD_VEH_INV] v	with (nolock) order by Cast(v.Code as int) ")

        mtxAva = DirectCast(FormularioSBO.Items.Item("mtxExt").Specific, SAPbouiCOM.Matrix)
        mtxAva.LoadFromDataSource()

    End Sub

    Public Sub AsignaValoresPropietario(ByRef dtResult As DataTable)
        If (dtResult.Rows.Count > 0) Then
            txtCodProp.AsignaValorDataSource(dtResult.GetValue("CardCode", 0))
            txtNomProp.AsignaValorDataSource(dtResult.GetValue("CardName", 0))
        End If
    End Sub

    Public Sub AsignaValoresTecnico(ByRef dtResult As DataTable)
        Dim tecName As String
        If (dtResult.Rows.Count > 0) Then
            txtCodTec.AsignaValorDataSource(dtResult.GetValue("empID", 0))
            If Not String.IsNullOrEmpty(dtResult.GetValue("middleName", 0)) Then
                tecName = String.Format("{0} {1} {2}", dtResult.GetValue("firstName", 0), dtResult.GetValue("middleName", 0), dtResult.GetValue("lastName", 0))
            Else
                tecName = String.Format("{0} {1}", dtResult.GetValue("firstName", 0), dtResult.GetValue("lastName", 0))
            End If
            txtNomTec.AsignaValorDataSource(tecName)
        End If
    End Sub

    'Public Sub AsignaValoresVendedor(ByRef dtResult As DataTable)
    '    If (dtResult.Rows.Count > 0) Then
    '        txtCodVen.AsignaValorDataSource(dtResult.GetValue("SlpCode", 0).ToString().Trim())
    '        txtNomVen.AsignaValorDataSource(dtResult.GetValue("SlpName", 0).ToString().Trim())
    '    End If
    'End Sub

    Public Sub AsignaValoresVehiculo(ByRef dtResult As DataTable)

        If (dtResult.Rows.Count > 0) Then
            FormularioSBO.Items.Item("txtOtros").Click()

            txtCodUnid.AsignaValorDataSource(dtResult.GetValue("U_Cod_Unid", 0))
            DirectCast(FormularioSBO.Items.Item("txtVehCod").Specific, EditText).Value = dtResult.GetValue("Code", 0)

            If Not dtResult.GetValue("U_Num_Plac", 0) Is Nothing Then
                DirectCast(FormularioSBO.Items.Item("txtNoPlac").Specific, EditText).Value = dtResult.GetValue("U_Num_Plac", 0)
            End If
            If Not dtResult.GetValue("U_Num_Plac", 0) Is Nothing Then
                DirectCast(FormularioSBO.Items.Item("txtVin").Specific, EditText).Value = dtResult.GetValue("U_Num_VIN", 0)
            End If
            If Not dtResult.GetValue("U_Num_Plac", 0) Is Nothing Then
                DirectCast(FormularioSBO.Items.Item("txtAno").Specific, EditText).Value = dtResult.GetValue("U_Ano_Vehi", 0)
            End If
            If Not dtResult.GetValue("U_Num_Plac", 0) Is Nothing Then
                DirectCast(FormularioSBO.Items.Item("txtKilIn").Specific, EditText).Value = dtResult.GetValue("U_Km_Unid", 0)
            End If

            If (Not String.IsNullOrEmpty(dtResult.GetValue("U_Cod_Marc", 0))) Then
                DirectCast(FormularioSBO.Items.Item("cboMarca").Specific, ComboBox).Select(dtResult.GetValue("U_Cod_Marc", 0))
                Utilitarios.CargarValidValuesEnCombos(cboEstilo.ValidValues, String.Format("select Code, Name from [@SCGD_ESTILO] with(nolock) where U_Cod_Marc = '{0}'", dtResult.GetValue("U_Cod_Marc", 0).Trim()))
            End If
            If (Not String.IsNullOrEmpty(dtResult.GetValue("U_Cod_Esti", 0))) Then
                DirectCast(FormularioSBO.Items.Item("cboEstilo").Specific, ComboBox).Select(dtResult.GetValue("U_Cod_Esti", 0))
                Utilitarios.CargarValidValuesEnCombos(cboModelo.ValidValues, String.Format("select Code, Name from [@SCGD_MODELO] with(nolock) where U_Cod_Esti = '{0}'", dtResult.GetValue("U_Cod_Esti", 0).Trim()))
            End If
            If (Not String.IsNullOrEmpty(dtResult.GetValue("U_Cod_Mode", 0))) Then
                DirectCast(FormularioSBO.Items.Item("cboModelo").Specific, ComboBox).Select(dtResult.GetValue("U_Cod_Mode", 0))
            End If
            If (Not String.IsNullOrEmpty(dtResult.GetValue("U_Combusti", 0))) Then
                DirectCast(FormularioSBO.Items.Item("cboCombus").Specific, ComboBox).Select(dtResult.GetValue("U_Combusti", 0))
            End If
            If (Not String.IsNullOrEmpty(dtResult.GetValue("U_Cod_Col", 0))) Then
                DirectCast(FormularioSBO.Items.Item("cboColor").Specific, ComboBox).Select(dtResult.GetValue("U_Cod_Col", 0))
            End If
            If (Not String.IsNullOrEmpty(dtResult.GetValue("U_Transmis", 0))) Then
                DirectCast(FormularioSBO.Items.Item("cboTrans").Specific, ComboBox).Select(dtResult.GetValue("U_Transmis", 0))
            End If

        End If
    End Sub

    Private Sub CambiaEstadoControles(ByVal mode As BoModeVisualBehavior)

        FormularioSBO.Items.Item("txtCodProp").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("txtNomProp").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("txtCodUnid").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("cboMarca").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("cboEstilo").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("cboModelo").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        'FormularioSBO.Items.Item("txtNoPuer").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("txtNoPlac").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("txtVin").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        'FormularioSBO.Items.Item("txtNoMot").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("txtAno").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("cboCombus").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        'FormularioSBO.Items.Item("txtCapa").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("txtNomTec").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("txtCodTec").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("cboVende").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        'FormularioSBO.Items.Item("chkMill").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        ' FormularioSBO.Items.Item("txtKilom").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        'FormularioSBO.Items.Item("txtNoCili").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        'FormularioSBO.Items.Item("txtCilin").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("cboColor").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        'FormularioSBO.Items.Item("cboTrac").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        'FormularioSBO.Items.Item("txtValFis").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        'FormularioSBO.Items.Item("txtTurbo").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        'FormularioSBO.Items.Item("txtInterC").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        'FormularioSBO.Items.Item("txtRtv").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("txtKilIn").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("cboTrans").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("cboIDSuc").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        FormularioSBO.Items.Item("cboMon").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
    End Sub

    Private Function ValidaExisteVehículo(ByRef BubbleEvent As Boolean) As Boolean
        Dim numUnid As String
        Dim query As String
        Dim existe = False
        Try
            numUnid = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_CodUnid", 0).Trim()
            If String.IsNullOrEmpty(numUnid) Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.txtNoUnitCode, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
            Else
                dtConsulta = FormularioSBO.DataSources.DataTables.Item(strDtConsulta)
                query = String.Format("Select count(1) as Cant from [@SCGD_VEHICULO] where U_Cod_Unid = '{0}' ", numUnid)
                dtConsulta.ExecuteQuery(query)
                If dtConsulta.Rows.Count > 0 Then
                    If CInt(dtConsulta.GetValue("Cant", 0) > 0) Then
                        existe = True
                    End If
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
            BubbleEvent = False
        End Try
        Return existe
    End Function

    Private Function CargarDatosVehículo(ByRef p_oGeneralDataVehi As GeneralData, ByRef BubbleEvent As Boolean) As Boolean
        Try
            p_oGeneralDataVehi.SetProperty("U_Cod_Unid", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_CodUnid", 0).Trim())
            p_oGeneralDataVehi.SetProperty("U_Cod_Marc", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_CodMarc", 0).Trim())
            p_oGeneralDataVehi.SetProperty("U_Cod_Mode", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_CodMode", 0).Trim())
            p_oGeneralDataVehi.SetProperty("U_Cod_Esti", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_CodEsti", 0).Trim())
            p_oGeneralDataVehi.SetProperty("U_Ano_Vehi", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_Ano", 0).Trim())
            p_oGeneralDataVehi.SetProperty("U_Num_Plac", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_Placa", 0).Trim())
            p_oGeneralDataVehi.SetProperty("U_Combusti", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_Combusti", 0).Trim())
            p_oGeneralDataVehi.SetProperty("U_Transmis", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_Transmis", 0).Trim())
            p_oGeneralDataVehi.SetProperty("U_CardCode", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_PropCed", 0).Trim())
            p_oGeneralDataVehi.SetProperty("U_CardName", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_PropNom", 0).Trim())
            p_oGeneralDataVehi.SetProperty("U_Cod_Col", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_CodCol", 0).Trim())
            p_oGeneralDataVehi.SetProperty("U_Num_VIN", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_Vin", 0).Trim())
            p_oGeneralDataVehi.SetProperty("U_Km_Unid", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_Km_Ing", 0).Trim())
            Return True
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
            BubbleEvent = False
            Return False
        End Try

    End Function

    Private Function CargarDatosCotizacion(ByRef oCotizacion As Documents) As Boolean

        Dim strCodSucursal As String
        Dim strSerieCotizacion As String
        Dim strMoneda As String = String.Empty
        Dim strMonedaSys As String
        Dim query As String

        Try
            strCodSucursal = cboSucur.Selected.Value
            strSerieCotizacion = DevuelveValorItemConfig(strCodSucursal, strUDFOfertaVenta)

            query = String.Format("select Currency from OCRD with (nolock) where CardCode = '{0}'", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_PropCed", 0).Trim())
            dtConsulta.ExecuteQuery(query)

            If (dtConsulta.Rows.Count > 0) Then
                If Not String.IsNullOrEmpty(dtConsulta.GetValue(0, 0).ToString()) Then
                    strMoneda = dtConsulta.GetValue(0, 0).ToString().Trim()
                Else
                    DMS_Connector.Helpers.GetCurrencies(strMoneda, strMonedaSys)
                End If
            End If

            oCotizacion.CardCode = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_PropCed", 0).Trim()
            oCotizacion.CardName = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_PropNom", 0).Trim()

            oCotizacion.DocCurrency = strMoneda

            If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_TecCode", 0).Trim()) Then
                oCotizacion.DocumentsOwner = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_TecCode", 0).Trim()
            End If

            If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_VenCod", 0).Trim()) Then
                oCotizacion.SalesPersonCode = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_VenCod", 0).Trim()
            End If

            oCotizacion.Series = strSerieCotizacion
            oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_CodUnid", 0).Trim()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value = DirectCast(FormularioSBO.Items.Item("txtVehCod").Specific, EditText).Value.Trim()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_Ano", 0).Trim()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_Placa", 0).Trim()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_CodMarc", 0).Trim()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_CodMode", 0).Trim()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_CodEsti", 0).Trim()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value = cboMarca.Selected.Description
            oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value = cboModelo.Selected.Description
            oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value = cboEstilo.Selected.Description
            oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_TecCode", 0).Trim()

            oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value = oCotizacion.CardCode
            oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value = oCotizacion.CardName
            oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value = strCodSucursal
            oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value = "1"
            oCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value = "2"

            Return CargaLineaCotizacionAva(oCotizacion)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
            Return False
        End Try
    End Function

    Private Function CargaLineaCotizacionAva(ByRef p_oCotizacion As Documents) As Boolean

        Dim strCodSucursal As String
        Dim query As String = String.Empty
        Dim strTipoItem As String
        Dim strDocEntry As String
        Dim strUsaListaPrecCliente As String
        Dim strCodListPrecio As String
        Dim g_strUsaConsultaSegunConf As String
        Dim strItmPrec As String
        Dim strTaxCode As String
        Dim strCCosEsp As String
        Dim strItmCurr As String

        Dim itemAva As SAPbobsCOM.IItems

        Try
            strCodSucursal = cboSucur.Selected.Value
            itemAva = _companySbo.GetBusinessObject(BoObjectTypes.oItems)

            query = "Select DocEntry, U_CodLisPre, U_UseLisPreCli, U_ItmAva, U_ItmAvaN, U_Imp_Repuestos, U_Imp_Serv, U_Imp_Suminis, U_Imp_ServExt, U_TOTAva from [@SCGD_CONF_SUCURSAL] with(nolock) where U_Sucurs = '{0}'"
            query = String.Format(query, strCodSucursal)
            dtConsulta.ExecuteQuery(query)
            If dtConsulta.Rows.Count > 0 Then
                If Not String.IsNullOrEmpty(dtConsulta.GetValue("U_ItmAva", 0)) Then
                    itemAva.GetByKey(dtConsulta.GetValue("U_ItmAva", 0).ToString().Trim())
                End If
            End If
            If String.IsNullOrEmpty(TipoOT) Then
                TipoOT = dtConsulta.GetValue("U_TOTAva", 0).ToString().Trim()
            End If
            If Not String.IsNullOrEmpty(itemAva.ItemCode) Then
                If p_oCotizacion.Lines.Count > 0 AndAlso Not String.IsNullOrEmpty(p_oCotizacion.Lines.ItemCode) Then
                    p_oCotizacion.Lines.Add()
                End If
                p_oCotizacion.Lines.ItemCode = itemAva.ItemCode
                p_oCotizacion.Lines.Quantity = 1

                strTipoItem = itemAva.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value.ToString().Trim()

                Select Case strTipoItem
                    Case "1", "5" ''Repuestos - Paquetes
                        strTaxCode = dtConsulta.GetValue("U_Imp_Repuestos", 0)
                    Case "2", "10" 'Servicios - Art Cita
                        strTaxCode = dtConsulta.GetValue("U_Imp_Serv", 0)
                    Case "3" 'Suministro
                        strTaxCode = dtConsulta.GetValue("U_Imp_Suminis", 0)
                    Case "4" 'Serv Externo
                        strTaxCode = dtConsulta.GetValue("U_Imp_ServExt", 0)
                End Select

                strDocEntry = dtConsulta.GetValue("DocEntry", 0).ToString().Trim()
                strUsaListaPrecCliente = dtConsulta.GetValue("U_UseLisPreCli", 0).ToString().Trim()

                If strUsaListaPrecCliente.Equals("Y") Then
                    dtQuery.ExecuteQuery(String.Format(m_strConsultaListaPreciosCliente, p_oCotizacion.CardCode))
                    strCodListPrecio = dtQuery.GetValue("ListNum", 0).ToString()
                Else
                    strCodListPrecio = dtConsulta.GetValue("U_CodLisPre", 0).ToString()
                End If

                TipoOT = dtConsulta.GetValue("U_TOTAva", 0).ToString().Trim()

                g_strUsaConsultaSegunConf = String.Format(g_strConsultaArti, strCodListPrecio, strDocEntry, itemAva.ItemCode.Trim())

                dtQuery.Rows.Clear()
                dtQuery.ExecuteQuery(g_strUsaConsultaSegunConf)
                strItmPrec = dtQuery.GetValue("prec", 0).ToString().Trim()
                strItmCurr = dtQuery.GetValue("mone", 0).ToString().Trim()
                strCCosEsp = dtQuery.GetValue("ccos", 0).ToString().Trim()

                p_oCotizacion.Lines.Currency = strItmCurr
                p_oCotizacion.Lines.UnitPrice = strItmPrec

                If Not String.IsNullOrEmpty(strTaxCode) Then
                    p_oCotizacion.Lines.TaxCode = strTaxCode
                    p_oCotizacion.Lines.VatGroup = strTaxCode
                End If

                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = strCCosEsp
                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = strTipoItem
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
            Return False
        End Try

    End Function

    Private Function CreaCotizacion(ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) As Boolean
        Dim oCompanyService As CompanyService
        Dim oGeneralServiceVehi As GeneralService
        Dim oGeneralDataVehi As GeneralData
        Dim oGeneralParams As GeneralDataParams

        Dim existeVehi As Boolean = True
        Dim strIDVehiculo As String
        Dim strCotID As String
        Dim strCotDN As String
        Dim strNoOrden As String
        Dim query As String
        Dim intNumVisita As Integer
        Dim result As Integer
        Try
            If pVal.BeforeAction Then
                oCompanyService = _companySbo.GetCompanyService()
                oGeneralServiceVehi = oCompanyService.GetGeneralService("SCGD_VEH")

                If Not ValidaExisteVehículo(BubbleEvent) Then
                    existeVehi = False
                    oGeneralDataVehi = DirectCast(oGeneralServiceVehi.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData), GeneralData)
                Else
                    strIDVehiculo = DirectCast(FormularioSBO.Items.Item("txtVehCod").Specific, EditText).Value.Trim()

                    oGeneralParams = oGeneralServiceVehi.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams)
                    oGeneralParams.SetProperty("Code", strIDVehiculo)
                    oGeneralDataVehi = oGeneralServiceVehi.GetByParams(oGeneralParams)
                End If

                If CargarDatosVehículo(oGeneralDataVehi, BubbleEvent) Then
                    CargarDatosCotizacion(oCotizacion)

                    dtConsulta = FormularioSBO.DataSources.DataTables.Item(strDtConsulta)

                    intNumVisita = Utilitarios.ObtieneNumeracionPorSucursalObjeto(dtConsulta, oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value, "SCGD_OT", CompanySBO)
                    oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value = intNumVisita.ToString()

                    strNoOrden = String.Format("{0}-01", intNumVisita.ToString())
                    oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value = strNoOrden
                    oCotizacion.UserFields.Fields.Item(mc_strEstadoCot).Value = My.Resources.Resource.EstadoOrdenNoIniciada
                    oCotizacion.UserFields.Fields.Item(mc_strEstadoCotID).Value = "1"

                    If String.IsNullOrEmpty(TipoOT) Then
                        query = "Select U_TOTAva from [@SCGD_CONF_SUCURSAL] with(nolock) where U_Sucurs = '{0}'"
                        query = String.Format(query, oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value)

                        dtConsulta = FormularioSBO.DataSources.DataTables.Item(strDtConsulta)
                        dtConsulta.ExecuteQuery(query)
                        TipoOT = dtConsulta.GetValue(0, 0).ToString().Trim()
                    End If

                    oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value = TipoOT

                    _companySbo.StartTransaction()

                    If existeVehi Then
                        oGeneralServiceVehi.Update(oGeneralDataVehi)
                    Else
                        oGeneralServiceVehi.Add(oGeneralDataVehi)
                    End If

                    result = oCotizacion.Add()
                    If result <> 0 Then
                        If _companySbo.InTransaction Then
                            _companySbo.EndTransaction(BoWfTransOpt.wf_RollBack)
                        End If
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCode & result & ": " & _companySbo.GetLastErrorDescription(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        BubbleEvent = False
                        Return False
                    Else
                        If _companySbo.InTransaction Then
                            _companySbo.EndTransaction(BoWfTransOpt.wf_Commit)
                        End If

                        _companySbo.GetNewObjectCode(strCotID)
                        FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).SetValue("U_CotID", 0, strCotID)

                        cotCls = New CotizacionCLS(ApplicationSBO, CompanySBO)
                        cotCls.strIdSucursal = oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                        cotCls.m_strNoOrden = strNoOrden
                        oCotizacion.GetByKey(strCotID)

                        strCotDN = oCotizacion.DocNum
                        FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).SetValue("U_CotDocN", 0, strCotDN)

                        cotCls.m_oCotizacion = oCotizacion
                        cotCls.ProcesarLineasAlCrear(True)
                        CotID = strCotID
                        AsignarFechayHoraOT(oCotizacion)

                        dtConsulta.ExecuteQuery("Select AutoKey from ONNM WITH (NOLOCK) where ObjectCode = 'SCGD_AVA'")
                        If dtConsulta.Rows.Count > 0 Then
                            AvaluoDE = dtConsulta.GetValue(0, 0).ToString().Trim()
                            oCotizacion.UserFields.Fields.Item("U_SCGD_NoAvaU").Value = AvaluoDE
                        End If

                        If Not _companySbo.InTransaction Then
                            _companySbo.StartTransaction()
                        End If

                        result = oCotizacion.Update()

                        If result <> 0 Then
                            If _companySbo.InTransaction Then
                                _companySbo.EndTransaction(BoWfTransOpt.wf_RollBack)
                            End If
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCode & result & ": " & _companySbo.GetLastErrorDescription(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                            Return False
                        Else
                            CrearOrdenTrabajoSAP(TipoOT, oCotizacion)
                            FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).SetValue("U_NoOT", 0, strNoOrden)
                            If _companySbo.InTransaction Then
                                _companySbo.EndTransaction(BoWfTransOpt.wf_Commit)
                                Return True
                            End If
                        End If
                    End If

                End If
            Else
                oCotizacion.GetByKey(CotID)
                If (String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_NoAva").Value)) Then
                    oCotizacion.UserFields.Fields.Item("U_SCGD_NoAvaU").Value = AvaluoDE
                    CompanySBO.StartTransaction()
                    result = oCotizacion.Update()

                    If result = 0 Then
                        CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit)
                        query = "select usr.USER_CODE  from OUSR usr right join OHEM emp on usr.USERID = emp.userId where emp.empID='{0}'"
                        query = String.Format(query, oCotizacion.DocumentsOwner)
                        dtConsulta.ExecuteQuery(query)
                        EnviarMensaje(String.Format(My.Resources.Resource.MSJAvaluoCreado, AvaluoDE), dtConsulta.GetValue(0, 0).ToString().Trim())
                        Return True
                    Else
                        If _companySbo.InTransaction Then
                            _companySbo.EndTransaction(BoWfTransOpt.wf_RollBack)
                        End If
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCode & result & ": " & _companySbo.GetLastErrorDescription(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        BubbleEvent = False
                    End If
                End If
            End If
        Catch ex As Exception
            BubbleEvent = False
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
            If _companySbo.InTransaction Then
                _companySbo.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            Return False
        End Try
    End Function

    Private Function AgregaButtonPic(ByRef oform As SAPbouiCOM.Form, ByVal strNombrectrl As String, ByVal intLeft As Integer, ByVal intTop As Integer, ByVal intFromPane As Integer, ByVal intTopane As Integer, ByVal ButtonType As SAPbouiCOM.BoButtonTypes, ByVal PathImagen As String, ByVal UDO As String) As SAPbouiCOM.Item
        Dim oitem As SAPbouiCOM.Item
        Dim oButton As SAPbouiCOM.Button
        Try

            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_BUTTON)
            oitem.Left = intLeft + 30
            oitem.Top = intTop
            oButton = oitem.Specific
            oButton.Type = ButtonType
            oitem.Width = 16
            oitem.Height = 14
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane
            oButton.Image = PathImagen

            If UDO <> "" Then
                oButton.ChooseFromListUID = UDO
            End If

            Return oitem
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Return Nothing
        End Try

    End Function

    Public Function ActualizarAvaluo(ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) As Boolean
        Dim result As Integer
        Dim noOT As String = String.Empty
        Dim intNumVisita As Integer
        Dim query As String
        Try
            If Not String.IsNullOrEmpty(CotID) Then
                oCotizacion.GetByKey(CotID)

                If oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value <> Nothing Then
                    noOT = oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString().Trim()
                End If
                If String.IsNullOrEmpty(noOT) Then
                    cotCls = New CotizacionCLS(ApplicationSBO, CompanySBO)
                    If String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value) AndAlso Not String.IsNullOrEmpty(cboSucur.Selected.Value) Then
                        oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value = cboSucur.Selected.Value.Trim()
                    End If
                    cotCls.strIdSucursal = oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value

                    intNumVisita = Utilitarios.ObtieneNumeracionPorSucursalObjeto(dtConsulta, oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value, "SCGD_OT", CompanySBO)
                    oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value = intNumVisita.ToString()
                    Dim noava As String = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("DocEntry", 0)
                    oCotizacion.UserFields.Fields.Item("U_SCGD_NoAvaU").Value = noava.Trim

                    noOT = String.Format("{0}-01", intNumVisita.ToString())

                    cotCls.m_strNoOrden = noOT

                    If oCotizacion.Lines.Count = 1 Then
                        oCotizacion.Lines.SetCurrentLine(0)
                        If String.IsNullOrEmpty(oCotizacion.Lines.ItemCode) Then
                            If Not CargaLineaCotizacionAva(oCotizacion) Then
                                BubbleEvent = False
                                Return False
                            End If
                        End If
                    End If
                    If String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                        oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = noOT
                    End If
                    cotCls.m_oCotizacion = oCotizacion
                    cotCls.ProcesarLineasAlCrear(True)

                    AsignarFechayHoraOT(oCotizacion)
                    FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).SetValue("U_NoOT", 0, noOT)
                    CompanySBO.StartTransaction()
                    result = oCotizacion.Update()

                    If result = 0 Then
                        If String.IsNullOrEmpty(TipoOT) Then
                            query = String.Format("Select U_TOTAva from [@SCGD_CONF_SUCURSAL] with(nolock) where U_Sucurs = '{0}'", oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value)
                            dtConsulta = FormularioSBO.DataSources.DataTables.Item(strDtConsulta)
                            dtConsulta.ExecuteQuery(query)
                            TipoOT = dtConsulta.GetValue(0, 0).ToString().Trim()
                        End If

                        CrearOrdenTrabajoSAP(TipoOT, oCotizacion)
                        CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit)
                        Return True
                    Else
                        If _companySbo.InTransaction Then
                            _companySbo.EndTransaction(BoWfTransOpt.wf_RollBack)
                        End If
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCode & result & ": " & _companySbo.GetLastErrorDescription(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        BubbleEvent = False
                        Return False
                    End If
                Else
                    Dim intNoAva As Integer = oCotizacion.UserFields.Fields.Item("U_SCGD_NoAvaU").Value
                    If intNoAva = 0 Then
                        Dim noava As String = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("DocEntry", 0)
                        oCotizacion.UserFields.Fields.Item("U_SCGD_NoAvaU").Value = noava.Trim
                        CompanySBO.StartTransaction()
                        result = oCotizacion.Update()

                        If result = 0 Then
                            CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit)
                            Return True
                        Else
                            If _companySbo.InTransaction Then
                                _companySbo.EndTransaction(BoWfTransOpt.wf_RollBack)
                            End If
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCode & result & ": " & _companySbo.GetLastErrorDescription(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                            Return False
                        End If
                    End If

                End If
            Else
                Return CreaCotizacion(pVal, BubbleEvent)
            End If
            Return True
        Catch ex As Exception
            If CompanySBO.InTransaction Then
                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            BubbleEvent = False

            Utilitarios.ManejadorErrores(ex, _applicationSbo)
            Return False
        End Try
    End Function

    Private Function DevuelveValorItemConfig(ByVal p_strSucur As String, ByVal strUDfName As String) As String
        Try
            Dim strSQL As String
            Dim strResult As String
            strSQL = "SELECT {0} FROM [@SCGD_CONF_SUCURSAL] with (nolock) WHERE U_Sucurs = '{1}'"
            strSQL = String.Format(strSQL, strUDfName, p_strSucur)

            strResult = Utilitarios.EjecutarConsulta(strSQL, _companySbo.CompanyDB, _companySbo.Server)

            If String.IsNullOrEmpty(strResult) Then
                strResult = -1
            End If

            Return strResult
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try

    End Function

    Public Sub AsignarFechayHoraOT(ByRef p_Cotizacion As Documents)
        Dim fhaActual As Date
        Try
            fhaActual = Utilitarios.RetornaFechaActual(CompanySBO.CompanyDB, CompanySBO.Server)

            If p_Cotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value <> Nothing AndAlso p_Cotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value <> Nothing Then
                p_Cotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value = fhaActual
                p_Cotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value = DateTime.Now
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub CrearOrdenTrabajoSAP(ByVal p_tipoOt As String, ByRef oCotizacion As Documents)

        Dim UDOOrden As UDOOrden
        Dim UDOEncabezado As EncabezadoUDOOrden
        Dim strNoIniciada As String

        UDOOrden = New UDOOrden(_companySbo)
        UDOEncabezado = New EncabezadoUDOOrden()
        dtConsulta = FormularioSBO.DataSources.DataTables.Item(strDtConsulta)
        dtConsulta.ExecuteQuery(" select Name from [@SCGD_ESTADOS_OT] with(nolock) where code = '1' ")
        strNoIniciada = dtConsulta.GetValue(0, 0).ToString().Trim()

        UDOEncabezado.Code = oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value

        With UDOEncabezado

            .U_DocEntry = CotID
            .U_NoOT = oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
            .U_NoUni = oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value
            '.U_NoCon = m_strCono
            .U_Ano = oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value
            .U_Plac = oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value
            .U_Marc = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value
            .U_Esti = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value
            .U_Mode = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value
            .U_CMar = oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value
            .U_CEst = oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value
            .U_CMod = oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value
            .U_NoVis = oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value
            .U_VIN = oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value
            .U_km = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_Km_Unid", 0).Trim()
            .U_TipOT = p_tipoOt
            .U_Sucu = oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value

            .U_CodCli = oCotizacion.CardCode
            .U_NCli = oCotizacion.CardName
            .U_CodCOT = oCotizacion.CardCode
            .U_NCliOT = oCotizacion.CardName
            .U_FRec = DateTime.Now
            .U_HRec = DateTime.Now
            .U_FApe = Date.Now
            .U_HApe = Date.Now
            .U_FFin = Nothing
            .U_HFin = Nothing
            .U_FCerr = Nothing
            .U_FFact = Nothing
            .U_FEntr = Nothing
            .U_OTRef = String.Empty
            .U_EstO = "1"
            .U_DEstO = strNoIniciada
            .U_Ase = oCotizacion.DocumentsOwner.ToString()
            .U_EncO = ""
            .U_Obse = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_Obs", 0).Trim()
        End With

        UDOOrden.Encabezado = UDOEncabezado
        UDOOrden.Company = _companySbo
        UDOOrden.Insert()
    End Sub

    Public Sub LinkOT(ByRef pval As ItemEvent, ByRef BubbleEvent As Boolean, ByRef formOT As SCG.ServicioPostVenta.OrdenTrabajo)

        Dim OtId As String = String.Empty

        OtId = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_NoOT", 0).Trim()
        formOT.CargarOT(OtId)
    End Sub

    Private Sub EnviarMensaje(ByVal p_strMensaje As String, ByVal p_strUserCode As String)
        Dim oMsg As Messages
        Dim intResultado As Integer
        Dim strError As String = String.Empty
        Dim intError As Integer
        Try
            oMsg = CompanySBO.GetBusinessObject(BoObjectTypes.oMessages)
            oMsg.MessageText = p_strMensaje
            oMsg.Subject = oMsg.MessageText

            oMsg.Recipients.Add()
            oMsg.Recipients.SetCurrentLine(0)
            oMsg.Recipients.UserCode = p_strUserCode
            oMsg.Recipients.NameTo = p_strUserCode
            oMsg.Recipients.SendInternal = BoYesNoEnum.tYES

            intResultado = oMsg.Add()
            If (intResultado <> 0) Then
                CompanySBO.GetLastError(intError, strError)
                Throw New ExceptionsSBO(intError, strError)
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub FinalizaAvaluo()
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralServiceAva As SAPbobsCOM.GeneralService
        Dim oGeneralDataAva As SAPbobsCOM.GeneralData
        Dim oGeneralParamsAva As SAPbobsCOM.GeneralDataParams

        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition

        Try

            oCompanyService = CompanySBO.GetCompanyService()

            oGeneralServiceAva = oCompanyService.GetGeneralService("SCGD_AVA")
            oGeneralParamsAva = DirectCast(oGeneralServiceAva.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams), SAPbobsCOM.GeneralDataParams)
            oGeneralParamsAva.SetProperty("DocEntry", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("DocEntry", 0))
            oGeneralDataAva = oGeneralServiceAva.GetByParams(oGeneralParamsAva)

            oGeneralDataAva.SetProperty("U_Estado", "2")

            CompanySBO.StartTransaction()
            oGeneralServiceAva.Update(oGeneralDataAva)
            CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit)

            FormularioSBO.Freeze(True)
            oConditions = DirectCast(ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions), SAPbouiCOM.Conditions)
            oCondition = oConditions.Add()

            oCondition.[Alias] = "DocEntry"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("DocEntry", 0)

            FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).Query(oConditions)

            FormularioSBO.Refresh()
            FormularioSBO.Mode = BoFormMode.fm_OK_MODE

            Dim query = "select usr.USER_CODE from OUSR usr right join OHEM emp on usr.USERID = emp.userId right join OSLP slp on emp.salesPrson = slp.SlpCode where slp.SlpCode = '{0}' "
            query = String.Format(query, cboVende.Selected.Value)
            dtConsulta.ExecuteQuery(query)

            If dtConsulta.Rows.Count > 0 AndAlso Not dtConsulta.GetValue(0, 0) Is Nothing Then
                EnviarMensaje(String.Format(My.Resources.Resource.MSJAvaluoFinalizado, FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("DocEntry", 0).Trim()), dtConsulta.GetValue(0, 0).ToString().Trim())
            End If

            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.TXTAvaluoFinalizado, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            If CompanySBO.InTransaction Then
                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
        End Try
    End Sub

    Private Sub CerrarAvaluo()
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralServiceAva As SAPbobsCOM.GeneralService
        Dim oGeneralParamsAva As SAPbobsCOM.GeneralDataParams
        Dim status As Boolean
        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition

        Try

            oCompanyService = CompanySBO.GetCompanyService()

            oGeneralServiceAva = oCompanyService.GetGeneralService("SCGD_AVA")
            oGeneralParamsAva = DirectCast(oGeneralServiceAva.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams), SAPbobsCOM.GeneralDataParams)
            oGeneralParamsAva.SetProperty("DocEntry", FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("DocEntry", 0))

            CompanySBO.StartTransaction()
            oGeneralServiceAva.Close(oGeneralParamsAva)
            CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit)

            FormularioSBO.Freeze(True)
            oConditions = DirectCast(ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions), SAPbouiCOM.Conditions)
            oCondition = oConditions.Add()

            oCondition.Alias = "DocEntry"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("DocEntry", 0)

            FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).Query(oConditions)

            FormularioSBO.Refresh()
            FormularioSBO.Mode = BoFormMode.fm_OK_MODE
            FormularioSBO.Freeze(False)

            status = True
            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.TXTAvaluoCerrado, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)

            ManejadorEventoFormDataLoad(status)

        Catch ex As Exception
            If CompanySBO.InTransaction Then
                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
        End Try
    End Sub

    Private Sub DeshabilitarControles(ByVal mode As BoModeVisualBehavior)
        For i As Integer = 0 To FormularioSBO.Items.Count - 1
            FormularioSBO.Items.Item(i).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
        Next
    End Sub

    ''' <summary>
    ''' Agrega Boton de Cerrar Documento
    ''' </summary>
    ''' <remarks></remarks>
    Private Function AgregaBTNCerrarDoc() As Boolean

        Dim oItem As SAPbouiCOM.Item
        Dim result As Boolean = True
        Dim oButton As SAPbouiCOM.Button
        Dim intTop As Integer
        Dim intLeft As Integer
        Dim intHeight As Integer
        Dim intWidth As Integer

        Try

            If Utilitarios.MostrarMenu("SCGD_CAV", CompanySBO.UserName) Then
                FormularioSBO.Freeze(True)
                intTop = FormularioSBO.Items.Item("2").Top
                intLeft = FormularioSBO.Items.Item("2").Left
                intWidth = FormularioSBO.Items.Item("2").Width
                intHeight = FormularioSBO.Items.Item("2").Height

                oItem = FormularioSBO.Items.Add("btnCerrar", SAPbouiCOM.BoFormItemTypes.it_BUTTON)
                oItem.Top = intTop
                oItem.Left = intLeft + 70
                oItem.Width = intWidth
                oItem.Height = intHeight
                oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 6, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 9, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                oItem.Enabled = True
                oItem.Visible = True

                oButton = oItem.Specific
                oButton.Type = SAPbouiCOM.BoButtonTypes.bt_Caption
                oButton.Caption = My.Resources.Resource.btnCerrarAvaluo
                btnCerrAva = True
                FormularioSBO.Freeze(False)
            Else
                result = False
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Agrega Boton de Cerrar Documento
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OcultarMostrarItem(ByRef estado As Boolean, ByVal itemUID As String, ByVal mode As BoModeVisualBehavior)
        For i As Integer = 0 To FormularioSBO.Items.Count - 1
            If FormularioSBO.Items.Item(i).UniqueID = itemUID Then
                FormularioSBO.Items.Item(i).Visible = estado
                FormularioSBO.Items.Item(i).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, mode)
            End If
        Next
    End Sub

    Private Sub CalcularTotales()
        Dim valValIni As Decimal
        Dim valRepMec As Decimal
        Dim valRepCar As Decimal
        Dim valBonExt As Decimal
        Dim valTotal As Decimal

        Dim total As Decimal

        Try
            valValIni = 0
            valRepMec = 0
            valRepCar = 0
            valBonExt = 0
            valTotal = 0

            valValIni = Utilitarios.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_ValIRec", 0), n)
            valRepMec = Utilitarios.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_RepMeca", 0), n)
            valRepCar = Utilitarios.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_RepCarr", 0), n)
            valBonExt = Utilitarios.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_BonExt", 0), n)
            valTotal = Utilitarios.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_ValRecA", 0), n)

            total = (valValIni + valBonExt) - (valRepMec + valRepCar)
            If total <> valTotal Then
                FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).SetValue("U_ValRecA", 0, total.ToString(n))
            End If


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    'Private Sub ActualizaLineasAvaluos(ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
    '    Try
    '        mtxAva = DirectCast(FormularioSBO.Items.Item("mtxExt").Specific, SAPbouiCOM.Matrix)

    '        mtxAva.FlushToDataSource()
    '        dtExtras = FormularioSBO.DataSources.DataTables.Item("dtVehiExt")

    '        For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(strTablaLinAva).Size - 1
    '            For j As Integer = 0 To dtExtras.Rows.Count-1
    '                If FormularioSBO.DataSources.DBDataSources.Item(strTablaLinAva).GetValue("", i) = dtExtras.GetValue("", j)
    '                    j
    '                End If
    '            Next

    '        Next

    '    Catch ex As Exception
    '        Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
    '    End Try
    'End Sub

#End Region

#Region "Eventos"

    Public Sub ApplicationSBOOnItemEvent(ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean, ByRef oDetVehiculos As DMS_Addon.VehiculosCls)
        Try
            If Not pVal.FormTypeEx = FormType Then Return

            If pVal.EventType <> BoEventTypes.et_FORM_ACTIVATE AndAlso pVal.EventType <> BoEventTypes.et_LOST_FOCUS AndAlso pVal.EventType <> BoEventTypes.et_GOT_FOCUS AndAlso pVal.EventType <> BoEventTypes.et_VALIDATE AndAlso pVal.EventType <> BoEventTypes.et_FORM_DEACTIVATE Then
                Select Case pVal.EventType
                    Case BoEventTypes.et_CHOOSE_FROM_LIST
                        ManejadorEventoChooseFromList(pVal, BubbleEvent)
                    Case BoEventTypes.et_ITEM_PRESSED
                        ManejadorEventoItemPressed(pVal, BubbleEvent, oDetVehiculos)
                    Case BoEventTypes.et_COMBO_SELECT
                        ManejadroEventoCombo(pVal, BubbleEvent)
                End Select
            Else
                If pVal.ItemUID = "txtValIRec" OrElse pVal.ItemUID = "txtRepMec" OrElse pVal.ItemUID = "txtRepCar" OrElse pVal.ItemUID = "txtExtBon" OrElse pVal.ItemUID = "txtValReAu" Then
                    CalcularTotales()
                End If
            End If
        Catch ex As Exception
            BubbleEvent = False
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Sub

    Public Sub ManejadorEventoFormDataLoad(ByRef BubbleEvent As Boolean)
        Dim avaEstado As String
        Try
            Utilitarios.CargarValidValuesEnCombos(cboEstilo.ValidValues, String.Format("select Code, Name from [@SCGD_ESTILO] with(nolock) where U_Cod_Marc = '{0}'", cboMarca.Value.Trim()))
            Utilitarios.CargarValidValuesEnCombos(cboModelo.ValidValues, String.Format("select Code, Name from [@SCGD_MODELO] with(nolock) where U_Cod_Esti = '{0}'", cboEstilo.Value.Trim()))

            '**************************Deshabilita Controles Encabezado**************************'
            CambiaEstadoControles(BoModeVisualBehavior.mvb_False)
            CotID = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_CotID", 0).Trim()
            avaEstado = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_Estado", 0).Trim()
            If avaEstado = "2" Then
                DeshabilitarControles(BoModeVisualBehavior.mvb_False)
                If FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("Status", 0).Trim() = "O" Then
                    If Not btnCerrAva Then
                        AgregaBTNCerrarDoc()
                    Else
                        OcultarMostrarItem(True, "btnCerrar", BoModeVisualBehavior.mvb_True)
                    End If
                End If

            Else
                DeshabilitarControles(BoModeVisualBehavior.mvb_True)
                CambiaEstadoControles(BoModeVisualBehavior.mvb_False)
                Dim tecCode As String = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_TecCode", 0)
                Dim venCode As String = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_VenCod", 0)
                If String.IsNullOrEmpty(tecCode) Then
                    FormularioSBO.Items.Item("txtCodTec").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                End If
                If String.IsNullOrEmpty(venCode) Then
                    FormularioSBO.Items.Item("cboVende").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                End If
                OcultarMostrarItem(True, "txtNoAva", BoModeVisualBehavior.mvb_False)
                OcultarMostrarItem(True, "txtNoOT", BoModeVisualBehavior.mvb_False)
                OcultarMostrarItem(True, "txtCotNum", BoModeVisualBehavior.mvb_False)
                OcultarMostrarItem(True, "txtNomTec", BoModeVisualBehavior.mvb_False)

                If btnCerrAva Then
                    OcultarMostrarItem(False, "btnCerrar", BoModeVisualBehavior.mvb_False)
                End If
            End If

        Catch ex As Exception
            BubbleEvent = False
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub ManejadorEventoChooseFromList(ByRef pval As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oCFLEvent As IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim strCFL_Id As String
        Dim oCondition As Condition
        Dim oConditions As Conditions

        Dim oDataTable As DataTable
        FormularioSBO.Freeze(True)
        Try
            oCFLEvent = CType(pval, IChooseFromListEvent)
            strCFL_Id = oCFLEvent.ChooseFromListUID
            oCFL = FormularioSBO.ChooseFromLists.Item(strCFL_Id)

            If oCFLEvent.ActionSuccess Then

                oDataTable = oCFLEvent.SelectedObjects

                If Not oCFLEvent.SelectedObjects Is Nothing Then

                    If Not oDataTable Is Nothing And
                        FormularioSBO.Mode <> BoFormMode.fm_FIND_MODE Then
                        Select Case pval.ItemUID
                            Case "txtCodProp"
                                AsignaValoresPropietario(oDataTable)
                            Case "txtCodTec"
                                AsignaValoresTecnico(oDataTable)
                            Case "txtCodUnid"
                                AsignaValoresVehiculo(oDataTable)
                        End Select
                    End If
                End If

            ElseIf oCFLEvent.BeforeAction Then
                Select Case pval.ItemUID
                    Case "txtCodUnid"
                        If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_PropCed", 0)) Then
                            oConditions = ApplicationSBO.CreateObject(BoCreatableObjectType.cot_Conditions)
                            oCondition = oConditions.Add
                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "U_CardCode"
                            oCondition.Operation = BoConditionOperation.co_EQUAL
                            oCondition.CondVal = FormularioSBO.DataSources.DBDataSources.Item(strTablaAva).GetValue("U_PropCed", 0).Trim()
                            oCondition.BracketCloseNum = 1
                            oCFL.SetConditions(oConditions)
                        End If
                    Case "txtCodTec"
                        oConditions = ApplicationSBO.CreateObject(BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "Active"
                        oCondition.Operation = BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "Y"
                        oCondition.BracketCloseNum = 1
                        If Not cboSucur Is Nothing AndAlso Not cboSucur.Selected Is Nothing Then
                            oCondition.Relationship = BoConditionRelationship.cr_AND
                            oCondition = oConditions.Add
                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "branch"
                            oCondition.Operation = BoConditionOperation.co_EQUAL
                            oCondition.CondVal = cboSucur.Selected.Value.Trim()
                            oCondition.BracketCloseNum = 1
                        End If
                        oCFL.SetConditions(oConditions)
                End Select
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

        FormularioSBO.Freeze(False)
    End Sub

    Public Sub ManejadroEventoCombo(ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim m_intRespuesta As Integer = 0
        Dim m_objItem As SAPbouiCOM.Item
        Dim m_objCombo As SAPbouiCOM.ComboBox
        Dim m_strValorCombo As String = String.Empty
        Try
            If pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case "cboMarca"
                        If cboMarca Is Nothing Then
                            cboMarca = DirectCast(FormularioSBO.Items.Item("cboMarca").Specific, ComboBox)
                        End If
                        If Not String.IsNullOrEmpty(cboMarca.Value) Then
                            If cboEstilo Is Nothing Then
                                cboEstilo = DirectCast(FormularioSBO.Items.Item("cboEstilo").Specific, ComboBox)
                            End If
                            Utilitarios.CargarValidValuesEnCombos(cboEstilo.ValidValues, String.Format("select Code, Name from [@SCGD_ESTILO] with(nolock) where U_Cod_Marc = '{0}'", cboMarca.Value.Trim()))
                        End If
                    Case "cboEstilo"
                        If cboEstilo Is Nothing Then
                            cboEstilo = DirectCast(FormularioSBO.Items.Item("cboEstilo").Specific, ComboBox)
                        End If
                        If Not String.IsNullOrEmpty(cboMarca.Value) Then
                            If cboModelo Is Nothing Then
                                cboModelo = DirectCast(FormularioSBO.Items.Item("cboModelo").Specific, ComboBox)
                            End If
                            Utilitarios.CargarValidValuesEnCombos(cboModelo.ValidValues, String.Format("select Code, Name from [@SCGD_MODELO] with(nolock) where U_Cod_Esti = '{0}'", cboEstilo.Value.Trim()))
                        End If
                    Case "cboEsta"
                        m_objItem = FormularioSBO.Items.Item("cboEsta")
                        m_objCombo = DirectCast(m_objItem.Specific, ComboBox)

                        m_strValorCombo = m_objCombo.Value.Trim()

                        Select Case m_strValorCombo
                            Case "2"
                                m_intRespuesta = ApplicationSBO.MessageBox(My.Resources.Resource.MsjFinalizarAvaluo, 1, My.Resources.Resource.Si, My.Resources.Resource.No)
                                If m_intRespuesta = 1 Then
                                    FinalizaAvaluo()
                                End If
                        End Select
                End Select
            End If
        Catch ex As Exception
            BubbleEvent = False
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub ManejadorEventoItemPressed(ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean, ByRef oDetVehiculos As DMS_Addon.VehiculosCls)
        Dim m_intRespuesta As Integer = 0
        Try
            FormularioSBO.Freeze(True)
            If pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case "1"
                        'ActualizaLineasAvaluos(pVal, BubbleEvent)
                        'If BubbleEvent Then
                        If FormularioSBO.Mode = BoFormMode.fm_ADD_MODE Then
                            CreaCotizacion(pVal, BubbleEvent)
                        ElseIf FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE Then
                            ActualizarAvaluo(pVal, BubbleEvent)
                        End If
                        'End If
                    Case "btnCerrar"
                        m_intRespuesta = ApplicationSBO.MessageBox(My.Resources.Resource.MsjCerrarAvaluo, 1, My.Resources.Resource.Si, My.Resources.Resource.No)
                        If m_intRespuesta = 1 Then
                            CerrarAvaluo()
                        End If
                End Select
            Else
                Select Case pVal.ItemUID
                    Case "Fol1"
                        FormularioSBO.PaneLevel = 1
                    Case "fol2"
                        FormularioSBO.PaneLevel = 2
                    Case "FolRevInf"
                        FormularioSBO.PaneLevel = 3
                    Case "folPru"
                        FormularioSBO.PaneLevel = 4
                    Case "FolDiag"
                        FormularioSBO.PaneLevel = 5
                    Case "folRep"
                        FormularioSBO.PaneLevel = 6
                    Case "FolRevLlan"
                        FormularioSBO.PaneLevel = 7
                    Case "1"
                        If FormularioSBO.Mode = BoFormMode.fm_ADD_MODE Then
                            CreaCotizacion(pVal, BubbleEvent)
                        End If
                    Case "btLkUnid"
                        dtConsulta = FormularioSBO.DataSources.DataTables.Item(strDtConsulta)
                        dtConsulta.ExecuteQuery("SELECT Code From [@SCGD_VEHICULO] with (nolock) WHERE U_Cod_Unid = '" & txtCodUnid.ObtieneValorDataSource().Trim() & "'")
                        Dim m_strIDVehi As String = dtConsulta.GetValue(0, 0)
                        Dim strCardCode As String = txtCodProp.ObtieneValorDataSource()
                        m_oVehiculo = oDetVehiculos

                        If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_DET_1", False, ApplicationSBO) Then
                            VehiculosCls.blnDesdeCita = True
                            VehiculosCls.blnDesdeCotizacion = False
                            Call m_oVehiculo.DibujarFormularioDetalleInformacionVehiculo(strCardCode.Trim(), m_strIDVehi, True, m_TypeVehiculo, m_TypeCountVehiculo, False, False, VehiculosCls.ModoFormulario.scgTaller)
                        End If
                End Select
            End If
            FormularioSBO.Freeze(False)
        Catch ex As Exception
            BubbleEvent = False
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub ManejadorEventosMenus(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean)
        Dim strMonLocal As String
        Dim strMonSys As String
        If Not pVal.BeforeAction Then
            Select Case pVal.MenuUID
                Case "1282"
                    FormularioSBO.Freeze(True)
                    DeshabilitarControles(BoModeVisualBehavior.mvb_True)

                    OcultarMostrarItem(True, "txtNoAva", BoModeVisualBehavior.mvb_False)
                    OcultarMostrarItem(True, "txtNoOT", BoModeVisualBehavior.mvb_False)
                    OcultarMostrarItem(True, "txtCotNum", BoModeVisualBehavior.mvb_False)
                    OcultarMostrarItem(True, "txtNomTec", BoModeVisualBehavior.mvb_False)

                    DMS_Connector.Helpers.GetCurrencies(strMonLocal, strMonSys)
                    If cboMone.ValidValues.Count > 0 Then
                        cboMone.Select(strMonSys)
                    End If
                    If btnCerrAva Then
                        OcultarMostrarItem(False, "btnCerrar", BoModeVisualBehavior.mvb_False)
                    End If
                    FormularioSBO.Freeze(False)
            End Select
        End If
    End Sub
#End Region
    
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
