Imports DMSOneFramework.SCGCommon
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Threading
Imports SAPbobsCOM
Imports SCG.SBOFramework
Imports SCG.DMSOne.Framework.MenuManager
Imports SAPbouiCOM
Imports SCG.UX.Windows
Imports SCG.SBOFramework.UI


Public Class VehiculosCls

#Region "Estructuras"
    Public Structure VehiculoUDT

        Public NoPlaca As Integer
        Public DescMArca As String
        Public DescModelo As String
        Public DescEstilo As String
        Public DescVehiculo As String
        Public Vin As String
        Public Año As Integer
        Public NoUnidad As Integer
        Public NumCliente As String
        Public DetCliente As String

    End Structure
#End Region

#Region "Enumerados"

    Public Enum TipoControl
        EditText = 1
        ComboBox = 2
        Button = 3
    End Enum

    Public Enum ModoFormulario
        scgVentas = 1
        scgTaller = 2
    End Enum

#End Region

#Region "Declaraciones"
    Private m_oCompany As SAPbobsCOM.Company
    Private m_strBDConfiguracion As String
    Private m_strBDTalller As String
    Private m_SBO_Application As Application


    Private m_strCodigoVehiculo As String
    Private m_strCodeReservado As String
    Private m_strCodeFacturado As String


    Public Const mc_strFolder1 As String = "Folder1"
    Public Const mc_strFolder2 As String = "Folder2"
    Public Const mc_strFolder3 As String = "Folder3"
    Public Const mc_strFolder4 As String = "Folder4"
    Public Const mc_strFolder5 As String = "Folder5"
    Public Const mc_strNumBP As String = "NumBP"
    Public Const mc_strNombreBP As String = "NomBP"
    Public Const mc_strCFLSCG As String = "CFL1"
    Public Const mc_strCFLBusinessPartner As String = "CFL2"
    Private Const mc_strUDFNoUnidad As String = "U_Cod_Unid"
    Private Const mc_strTablaArchivosDigitales As String = "SCGTA_Archivos"

    Private Const mc_strbtnArchivos As String = "btnArch"
    Private Const mc_strVehiculo As String = "@SCGD_VEHICULO"
    Private Const mc_CardCode As String = "CardCode"
    Private Const mc_CardName As String = "CardName"
    Private Const mc_U_CardCode As String = "U_CardCode"
    Private Const mc_U_CardName As String = "U_CardName"
    Public Const mc_strAnio As String = "txtanio"
    Public Const mc_strUnidad As String = "txtUnid"
    Public Const mc_strPasajeros As String = "txtPas"
    Public Const mc_strPlaca As String = "txtPlaca"
    Public Const mc_strCliente As String = "txtCl"
    Public Const mc_strEmpleado As String = "1000002"
    Public Const mc_strDetCliente As String = "txtDetCl"
    Public Const mc_strNoMotor As String = "txtMotor"
    Public Const mc_strMarcaMotor As String = "cboMarcM"
    Public Const mc_strEjes As String = "txtEjs"
    Public Const mc_strPuertas As String = "txtPrt"
    Public Const mc_strVIN As String = "txtVIN"
    Public Const mc_strObservaciones As String = "txtObs"
    Public Const mc_strNoPuertas As String = "txtPrt"
    Public Const mc_strPeso As String = "txtPeso"
    Public Const mc_strCilindrada As String = "txtCilind"
    Public Const mc_strCategoria As String = "cboCat"
    Public Const mc_strTipoContrato As String = "cboTipCo"
    Public Const mc_strDisponibilidad As String = "cboDisp"
    Public Const mc_strNoCilindros As String = "txtCil"
    Public Const mc_strGarantiaKM As String = "txtGaKm"
    Public Const mc_strGarantiaTiempo As String = "txtGaTM"
    Public Const mc_strPotenciaKW As String = "txtPot"
    Public Const mc_strTraccion As String = "cboTrac"
    Public Const mc_strCombustible As String = "cboComb"
    Public Const mc_strEstado As String = "cboSta"
    Public Const mc_strTipo As String = "cboTipo"
    Public Const mc_strUbicaciones As String = "cboUbi"
    Public Const mc_strNuevoUsado As String = "cboNuevoUs"
    Public Const mc_strCabina As String = "cboCab"
    Public Const mc_strTecho As String = "cboTech"
    Public Const mc_strClasificacion As String = "cboClasif"
    Public Const mc_strCarroceria As String = "cboCarr"
    Public Const mc_strTransmision As String = "cboTrans"
    Public Const mc_strEstilo As String = "cboEst"
    Public Const mc_strMarca As String = "cboMarca"
    Public Const mc_strModelo As String = "cboModelo"
    Public Const mc_strNumVehiculo As String = "txtNumVeh"
    Public Const mc_strBuscar As String = "btnBuscar"
    Public Const mc_strAdd As String = "add"
    Public Const mc_strDelete As String = "del"
    Public Const mc_strPrecio As String = "txtPrecio"
    Public Const mc_strReservadoPor As String = "1000002"
    Public Const mc_strRemarks As String = "1000007"
    Public Const mc_strFechaArribo As String = "1000008"
    Public Const mc_strCodigoFabrica As String = "txtCod_Fab"
    Public Const mc_strCosPro As String = "txtCosPro"
    'MONEDA PARA COSTO PROYECTADO
    Public Const mc_strMoneda As String = "cboMoneda"
    'CAMPOS A DESHABILITAR POR RECEPCION
    Public Const mc_strTxtMarcaCom As String = "txt_Des_MC"
    Public Const mc_strComboMoneda As String = "cboMoneda"
    Public Const mc_strComboEstado As String = "cboSta"

    Public Const mc_strBtnArtVenta As String = "btnArtVent"
    Public Const mc_strBtnColor As String = "btnColor"
    Public Const mc_strBtnColTap As String = "btnColTap"

    Private oForm As Form
    Private m_blnAgregarComponentes As Boolean = True
    Private m_blnHayVehículo As Boolean = True

    Public Const mc_strBono As String = "txtBono"
    Public Const mc_strValorNeto As String = "txtValorNe"
    Public Const mc_strInvPreVenta As String = "cboInvPreV"

    <CLSCompliant(False)> _
    Public Event AgregoVehiculo(ByVal sender As Object, ByVal NoVehiculo As String, ByVal oForm As Form)
    Public Event AgregoVehiculoCita(ByVal sender As Object, ByVal NoVehiculo As String, ByVal oForm As Form)

    Private m_blnCierraForma As Boolean

    Public m_intModoForulario As ModoFormulario

    Public EditTxtNumCV As EditTextSBO
    Public EditTxtFhaCV As EditTextSBO
    Public EditTxtNumFacV As EditTextSBO
    Public EditTxtFhaFacV As EditTextSBO
    Public EditTxtNomVendV As EditTextSBO
    Public EditTxtNomClientV As EditTextSBO

    Private Shared _blnDesdeCotizacion As Boolean = False
    Private Shared _blnDesdeCita As Boolean = False

    Dim cboCombos As ComboBox
    Dim oItems As Item
    Private strVal As String = String.Empty

    Public Const g_str_mtxBonos As String = "mtx_Bonos"
    Public Const g_str_mtxComponentes As String = "mtx_0"
    Public Const g_str_BONOXVEH As String = "@SCGD_BONOXVEH"
    Public Const g_str_ACCXVEH As String = "@SCGD_ACCXVEH"
    Public Const g_str_ColBono As String = "Col_Bono"
    Public Const g_str_ColMonto As String = "Col_Monto"
    Public Const g_strUBono As String = "U_Bono"
    Public Const g_strUTotalAcc As String = "U_TotalAcc"
    Public Const g_strUMonto As String = "U_Monto"
    Public Const g_strUTotal As String = "U_Total"
    Public EditTextBono As EditTextSBO
    Public n As NumberFormatInfo

    Public Shared g_strSeparadorMillares As String
    Public Shared g_strSeparadorDecimales As String
    Private oVehiculo As Vehiculo

#End Region

#Region "Constructor"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ocompany"></param>
    ''' <param name="SBOAplication"></param>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application)

        m_oCompany = ocompany
        m_SBO_Application = SBOAplication
        n = DIHelper.GetNumberFormatInfo(m_oCompany)

    End Sub

#End Region

#Region "Properties"
    <CLSCompliant(False)> _
    Public Property SAPCompany() As SAPbobsCOM.Company
        Get
            Return m_oCompany
        End Get
        Set(ByVal value As SAPbobsCOM.Company)
            m_oCompany = value
        End Set
    End Property

    Public Property BDConfiguracion() As String
        Get
            Return m_strBDConfiguracion
        End Get
        Set(ByVal value As String)
            m_strBDConfiguracion = value
        End Set
    End Property

    Public Property BDTaller() As String
        Get
            Return m_strBDTalller
        End Get
        Set(ByVal value As String)
            m_strBDTalller = value
        End Set
    End Property

    Public ReadOnly Property CierraForma() As Boolean
        Get
            Return m_blnCierraForma
        End Get
    End Property

    Public Property AgregarComponentesPorDefecto() As Boolean
        Get
            Return m_blnAgregarComponentes
        End Get
        Set(ByVal value As Boolean)
            m_blnAgregarComponentes = value
            m_blnHayVehículo = Not value
        End Set
    End Property

    Public Property p_StrCodeReservado As String
        Get
            Return m_strCodeReservado
        End Get
        Set(ByVal value As String)
            m_strCodeReservado = value
        End Set
    End Property

    Public Property p_StrCodeFacturado As String
        Get
            Return m_strCodeFacturado
        End Get
        Set(ByVal value As String)
            m_strCodeFacturado = value
        End Set
    End Property

    Public Shared Property blnDesdeCotizacion As Boolean
        Get
            Return _blnDesdeCotizacion
        End Get
        Set(ByVal value As Boolean)
            _blnDesdeCotizacion = value
        End Set
    End Property

    Public Shared Property blnDesdeCita As Boolean
        Get
            Return _blnDesdeCita
        End Get
        Set(ByVal value As Boolean)
            _blnDesdeCita = value
        End Set
    End Property

#End Region

#Region "Metodos"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Protected Friend Sub AddMenuItems()

        Dim strEtiquetaMenu As String
        Dim sPath As String

        sPath = Windows.Forms.Application.StartupPath

        strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_MNO", m_SBO_Application.Language)
        GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_MNO", BoMenuType.mt_POPUP, strEtiquetaMenu, 15, False, True, sPath & "\sbo.bmp", "43520"))


        If Utilitarios.MostrarMenu("SCGD_VEH", m_SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_VEH", m_SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_VEH", BoMenuType.mt_STRING, strEtiquetaMenu, 5, False, True, "SCGD_MNO"))
        End If

        If Utilitarios.MostrarMenu("SCGD_VHE", m_SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_VHE", m_SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_VHE", BoMenuType.mt_STRING, strEtiquetaMenu, 10, False, True, "SCGD_MNO"))
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strCardCode"></param>
    ''' <param name="stridNoVehiculo"></param>
    ''' <param name="CierraFormaDespuesAgegarVehiculo"></param>
    ''' <param name="TypeVehiculo"></param>
    ''' <param name="TypeCountVehiculo"></param>
    ''' <param name="ReadOnlyCar"></param>
    ''' <param name="CargarComponentes"></param>
    ''' <param name="TipoLlamada"></param>
    ''' <param name="EnableEditFields"></param>
    ''' <remarks></remarks>
    Protected Friend Sub DibujarFormularioDetalleInformacionVehiculo(ByVal strCardCode As String, _
                                                                     ByVal stridNoVehiculo As String, _
                                                                     ByVal CierraFormaDespuesAgegarVehiculo As Boolean, _
                                                                     ByRef TypeVehiculo As String, _
                                                                     ByRef TypeCountVehiculo As Integer, _
                                                                     ByVal ReadOnlyCar As Boolean, _
                                                                     ByVal CargarComponentes As Boolean, _
                                                                     ByVal TipoLlamada As ModoFormulario, _
                                                                     Optional ByVal EnableEditFields As Boolean = True)

        '*******************************************************************    
        'Propósito: Se encarga de establecer los filtros para los eventos de la
        '            aplicacion que se van a manejar y posteriormente se los
        '            agrega al objeto aplicacion donde se esta almacenando la
        '            aplicacion SBO que esta corriendo
        '
        'Acepta:    Ninguno
        'Retorna:   Ninguno
        'Desarrollador: Yeiner
        'Fecha: 19 Abril 2006
        '********************************************************************
        Try

            Dim strValorSeleccionado As String = ""
            Dim oitem As Item
            Dim oedit As EditText
            Dim strXMLACargar As String
            Dim fcp As FormCreationParams
            Dim oMatrix As Matrix
            Dim oMatrixBonos As Matrix
            Dim strMonedaDefecto As String

            m_blnAgregarComponentes = CargarComponentes

            fcp = m_SBO_Application.CreateObject(BoCreatableObjectType.cot_FormCreationParams)
            strXMLACargar = My.Resources.Resource.DetalleVehiculos
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            fcp.UniqueID = "SCGD_DET_1"
            fcp.FormType = "SCGD_DET_1"
            fcp.ObjectType = "SCGD_VEH"

            m_blnCierraForma = CierraFormaDespuesAgegarVehiculo


            If Utilitarios.ValidarSiFormularioAbierto("SCGD_DET_1", True, m_SBO_Application) Then
                m_SBO_Application.Forms.Item("SCGD_DET_1").Close()
            End If



            oForm = m_SBO_Application.Forms.AddEx(fcp)
            oForm.Items.Item("Folder1").Specific.Select()
            oForm.Freeze(True)

            '---------------------------------------------------------------
            ' MODIFICADO EL 17/12/12  Permisos sobre la pestaña de trazabilidad.

            If Not Utilitarios.MostrarMenu("SCGD_VTR", m_SBO_Application.Company.UserName) Then
                oForm.Items.Item("Folder5").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_False)
            Else
                oForm.Items.Item("Folder5").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
            End If

            '---------------------------------------------------------------
            If strCardCode <> "" Then

                oedit = oForm.Items.Item(mc_strCliente).Specific
                oedit.String = strCardCode

            End If
            If stridNoVehiculo <> "" Then
                m_blnHayVehículo = True
            Else
                m_blnHayVehículo = False
            End If

            m_strCodigoVehiculo = ""
            oForm.EnableMenu("6915", True)

            Call CargarComboNuevoUsado(oForm)

            oitem = oForm.Items.Item(mc_strNumVehiculo)
            oMatrix = DirectCast(oForm.Items.Item("mtx_0").Specific, Matrix)
            oMatrix.Columns.Item("col_f").Editable = False

            oMatrix.Columns.Item("Col_3").Visible = False

            m_intModoForulario = TipoLlamada

            If stridNoVehiculo <> "" Then

                Dim oConditions As Conditions
                Dim oCondition As Condition
                Dim oCombo As ComboBox

                oConditions = m_SBO_Application.CreateObject(BoCreatableObjectType.cot_Conditions)

                oCondition = oConditions.Add

                oCondition.Alias = "Code"
                oCondition.Operation = BoConditionOperation.co_EQUAL
                oCondition.CondVal = stridNoVehiculo

                oedit = oitem.Specific
                oedit.String = stridNoVehiculo

                Call oForm.DataSources.DBDataSources.Item(mc_strVehiculo).Query(oConditions)
                Call oForm.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").Query(oConditions)
                Call oForm.DataSources.DBDataSources.Item("@SCGD_VEHITRAZA").Query(oConditions)
                Call oForm.DataSources.DBDataSources.Item("@SCGD_BONOXVEH").Query(oConditions)

                oMatrix.LoadFromDataSource()

                oMatrixBonos = DirectCast(oForm.Items.Item("mtx_Bonos").Specific, Matrix)
                oMatrixBonos.LoadFromDataSource()

                oCombo = DirectCast(oForm.Items.Item(mc_strMarca).Specific, ComboBox)
                oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE
                If oCombo.Selected IsNot Nothing Then
                    strValorSeleccionado = oCombo.Selected.Value
                End If

                If Not String.IsNullOrEmpty(strValorSeleccionado) Then
                    CargarEstilos(oForm)
                End If
                Call DesabilitarCombos(oForm, mc_strEstilo)

                oCombo = DirectCast(oForm.Items.Item(mc_strEstilo).Specific, ComboBox)
                If oCombo.Selected IsNot Nothing Then
                    strValorSeleccionado = oCombo.Selected.Value
                End If


                If Not String.IsNullOrEmpty(strValorSeleccionado) Then
                    CargarModelos(oForm)
                End If

            Else
                strMonedaDefecto = DMS_Connector.Configuracion.ParamGenAddon.U_Mon_Def
                If Not String.IsNullOrEmpty(strMonedaDefecto) Then
                    oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_Moneda", 0, strMonedaDefecto)
                End If
            End If

            oForm.DataBrowser.BrowseBy = mc_strNumVehiculo

            TypeVehiculo = oForm.TypeEx
            TypeCountVehiculo = oForm.TypeCount

            Dim l_intTopActual As Integer = 0
            Dim l_intLeftActual As Integer = 0

            Call AgregaButtonPic(oForm, "btnArtVent", l_intLeftActual + 570, l_intTopActual + 133, 3, 3, SAPbouiCOM.BoButtonTypes.bt_Image, System.Windows.Forms.Application.StartupPath.ToString & "\CFL.BMP", "")
            Call AgregaButtonPic(oForm, "btnColor", l_intLeftActual + 570, l_intTopActual + 131, 1, 1, SAPbouiCOM.BoButtonTypes.bt_Image, System.Windows.Forms.Application.StartupPath.ToString & "\CFL.BMP", "")
            Call AgregaButtonPic(oForm, "btnColTap", l_intLeftActual + 570, l_intTopActual + 146, 1, 1, SAPbouiCOM.BoButtonTypes.bt_Image, System.Windows.Forms.Application.StartupPath.ToString & "\CFL.BMP", "")

            Call CargarDescripcionCombo(oForm)
            'Call CargarComboTipo(oForm, False, String.Empty)
            Call ManejarModoFormulario(oForm, EnableEditFields)
            'Call ManejarCampoCostoTrazabilidad(oForm)


            oForm.Visible = True

            If ReadOnlyCar Then
                Utilitarios.FormularioSoloLectura(oForm, False)
            End If

            CargarComboTiposBono(oForm)
            AgregaLineaMatrizBonos(oForm)

            oForm.DataSources.DataTables.Add("tConsulta")

            Utilitarios.ObtenerSeparadoresNumerosSAP(g_strSeparadorMillares, g_strSeparadorDecimales, m_oCompany.CompanyDB, m_oCompany.Server)

            oForm.Freeze(False)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oForm"></param>
    ''' <remarks></remarks>
    Private Sub CalculaTotalBonos(ByVal p_oForm As Form)

        Dim m_intTamano As Integer = 0
        Dim m_intMontoTotal As Decimal = 0
        Dim m_intMonto As Decimal = 0
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strMontoTotal As String = String.Empty
        Dim strMonto As String = String.Empty

        Try
            oMatrix = DirectCast(p_oForm.Items.Item(g_str_mtxBonos).Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()
            m_intTamano = p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).Size
            m_intMontoTotal = 0

            For i As Integer = 0 To m_intTamano - 1
                strMonto = p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).GetValue(g_strUMonto, i).ToString(n)
                m_intMonto = Decimal.Parse(strMonto, n)
                m_intMontoTotal = m_intMontoTotal + m_intMonto
            Next

            strMontoTotal = Utilitarios.ObtenerFormatoSAP(m_intMontoTotal, g_strSeparadorMillares, g_strSeparadorDecimales)
            p_oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue(g_strUBono, 0, strMontoTotal)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub


   

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oForm"></param>
    ''' <remarks></remarks>
    Public Sub CargarComboNuevoUsado(ByRef p_oForm As SAPbouiCOM.Form)
        Try
            Dim cboCombo As SAPbouiCOM.ComboBox
            Dim oItem As SAPbouiCOM.Item
            DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)
            oItem = oForm.Items.Item(mc_strNuevoUsado)
            cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
            If cboCombo.ValidValues.Count = 0 Then
                cboCombo.ValidValues.Add("N", My.Resources.Resource.Valor_Vehiculo_Nuevo)
                cboCombo.ValidValues.Add("U", My.Resources.Resource.Valor_Vehiculo_Usado)
            End If


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oForm"></param>
    ''' <param name="EnableEditFields"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub ManejarModoFormulario(ByRef p_oForm As SAPbouiCOM.Form, Optional ByVal EnableEditFields As Boolean = True)

        Dim cboTipo As SAPbouiCOM.ComboBox
        Dim strTipoTaller As String

        p_oForm.Freeze(True)
        If EnableEditFields Then
            If p_oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then

                If m_intModoForulario = ModoFormulario.scgTaller Then

                    strTipoTaller = DMS_Connector.Configuracion.ParamGenAddon.U_Inven_V

                    If Not String.IsNullOrEmpty(strTipoTaller) AndAlso p_oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                        Call CargarComboTipo(oForm, False, String.Empty)
                        cboTipo = DirectCast(p_oForm.Items.Item(mc_strTipo).Specific, SAPbouiCOM.ComboBox)
                        cboTipo.Select(strTipoTaller)
                        p_oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Tipo", 0, strTipoTaller)
                    End If

                    '
                    'Maestro Vehiculo de Tipo Servicio'
                    '
                    m_SBO_Application.Forms.Item(p_oForm.UniqueID).Select()
                    p_oForm.ActiveItem = mc_strCliente

                    p_oForm.Items.Item(mc_strTipo).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    p_oForm.Items.Item(mc_strDisponibilidad).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    p_oForm.Items.Item(mc_strPrecio).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    p_oForm.Items.Item(mc_strCosPro).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    p_oForm.Items.Item(mc_strRemarks).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    p_oForm.Items.Item(mc_strReservadoPor).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    p_oForm.Items.Item(mc_strFechaArribo).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    p_oForm.Items.Item(mc_strCodigoFabrica).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    p_oForm.Items.Item(mc_strTxtMarcaCom).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    p_oForm.Items.Item(mc_strComboMoneda).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    p_oForm.Items.Item(mc_strBono).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    p_oForm.Items.Item(mc_strValorNeto).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    p_oForm.Items.Item(mc_strBtnArtVenta).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    ' p_oForm.Items.Item(mc_strUnidad).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    'p_oForm.Items.Item(mc_strComboEstado).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

                    If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_VerCostoS = "Y") Then
                        If DMS_Connector.Configuracion.ParamGenAddon.U_VerCostoS = "Y" Then
                            p_oForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                            p_oForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                        Else
                            p_oForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                            p_oForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                        End If
                    Else
                        p_oForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                        p_oForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    End If
                    'Se deshabilita las opciones del menu de SAP, dado que no son necesarias para este formulario
                    'pero se deja la opcion de crear uno nuevo 
                    Call p_oForm.EnableMenu("43520", False)
                    Call p_oForm.EnableMenu("1290", False)
                    Call p_oForm.EnableMenu("1291", False)
                    Call p_oForm.EnableMenu("1288", False)
                    Call p_oForm.EnableMenu("1289", False)
                    Call p_oForm.EnableMenu("1290", False)
                    Call p_oForm.EnableMenu("1281", False)
                    If p_oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE OrElse p_oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                        Call p_oForm.EnableMenu("1282", False)
                    Else
                        Call p_oForm.EnableMenu("1282", True)
                    End If

                Else
                    p_oForm.Items.Item(mc_strTipo).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    p_oForm.Items.Item(mc_strDisponibilidad).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    p_oForm.Items.Item(mc_strPrecio).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    p_oForm.Items.Item(mc_strCosPro).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    p_oForm.Items.Item(mc_strRemarks).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    p_oForm.Items.Item(mc_strReservadoPor).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    p_oForm.Items.Item(mc_strFechaArribo).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    p_oForm.Items.Item(mc_strCodigoFabrica).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    p_oForm.Items.Item(mc_strTxtMarcaCom).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    p_oForm.Items.Item(mc_strComboMoneda).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    p_oForm.Items.Item(mc_strBono).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    p_oForm.Items.Item(mc_strValorNeto).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    p_oForm.Items.Item(mc_strBtnArtVenta).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    'p_oForm.Items.Item(mc_strUnidad).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    'p_oForm.Items.Item(mc_strComboEstado).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_VerCostoS = "Y") Then
                        If DMS_Connector.Configuracion.ParamGenAddon.U_VerCostoS = "Y" Then
                            p_oForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                            p_oForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                        Else
                            p_oForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                            p_oForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                        End If
                    Else
                        p_oForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                        p_oForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    End If
                    
                    If p_oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                        Call p_oForm.EnableMenu("1282", False)
                    Else
                        Call p_oForm.EnableMenu("1282", True)
                    End If
                End If
            Else
                p_oForm.Items.Item(mc_strUnidad).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                p_oForm.Items.Item(mc_strTipo).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                p_oForm.Items.Item(mc_strDisponibilidad).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                p_oForm.Items.Item(mc_strPrecio).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                p_oForm.Items.Item(mc_strCosPro).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                p_oForm.Items.Item(mc_strRemarks).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                p_oForm.Items.Item(mc_strReservadoPor).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                p_oForm.Items.Item(mc_strFechaArribo).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                p_oForm.Items.Item(mc_strCodigoFabrica).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                p_oForm.Items.Item(mc_strTxtMarcaCom).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                p_oForm.Items.Item(mc_strComboMoneda).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                p_oForm.Items.Item(mc_strBono).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                p_oForm.Items.Item(mc_strValorNeto).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                p_oForm.Items.Item(mc_strBtnArtVenta).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                'p_oForm.Items.Item(mc_strUnidad).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                'p_oForm.Items.Item(mc_strComboEstado).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

                If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_VerCostoS = "Y") Then
                    If DMS_Connector.Configuracion.ParamGenAddon.U_VerCostoS = "Y" Then
                        p_oForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                        p_oForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    Else
                        p_oForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                        p_oForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    End If
                Else
                    p_oForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    p_oForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                End If
                Call p_oForm.EnableMenu("1282", True)

               

            End If
            If p_oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE OrElse p_oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                If m_intModoForulario = ModoFormulario.scgTaller Then
                    HabilitarControles(False, oForm)
                End If
            End If
        Else
            HabilitarControles(False, oForm)
            'For Each item As SAPbouiCOM.Item In p_oForm.Items
            '    If item.Type = BoFormItemTypes.it_EDIT Or item.Type = BoFormItemTypes.it_COMBO_BOX Or item.Type = BoFormItemTypes.it_MATRIX Then
            '        If Not (item.UniqueID = mc_strCliente) AndAlso Not (item.UniqueID = mc_strPlaca) Then
            '            item.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            '        End If
            '    End If
            'Next
            Call oForm.EnableMenu("1281", False)
            Call oForm.EnableMenu("1282", False)
            Call oForm.EnableMenu("1291", False)
            Call oForm.EnableMenu("1288", False)
            Call oForm.EnableMenu("1289", False)
            Call oForm.EnableMenu("1290", False)
        End If

        DeshabilitarNumeroUnidad(p_oForm)

        If m_intModoForulario = ModoFormulario.scgTaller Then
            DeshabilitarSocioNegocios(p_oForm)
        End If


        oForm.Freeze(False)

    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejarCampoCostoTrazabilidad(ByRef p_oForm As SAPbouiCOM.Form)
        Try
            If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_VerCostoS = "Y") Then
                If DMS_Connector.Configuracion.ParamGenAddon.U_VerCostoS = "Y" Then
                    p_oForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    p_oForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                Else
                    p_oForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    p_oForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                End If
            Else
                p_oForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                p_oForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oform"></param>
    ''' <param name="strNombrectrl"></param>
    ''' <param name="intLeft"></param>
    ''' <param name="intTop"></param>
    ''' <param name="intFromPane"></param>
    ''' <param name="intTopane"></param>
    ''' <param name="ButtonType"></param>
    ''' <param name="PathImagen"></param>
    ''' <param name="UDO"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AgregaButtonPic(ByRef oform As SAPbouiCOM.Form, _
                               ByVal strNombrectrl As String, _
                               ByVal intLeft As Integer, _
                               ByVal intTop As Integer, _
                               ByVal intFromPane As Integer, _
                               ByVal intTopane As Integer, _
                               ByVal ButtonType As SAPbouiCOM.BoButtonTypes, _
                               ByVal PathImagen As String, _
                               ByVal UDO As String) As SAPbouiCOM.Item

        Dim oitem As SAPbouiCOM.Item
        Dim oButton As SAPbouiCOM.Button
        Try

            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_BUTTON)
            oitem.Left = intLeft
            oitem.Top = intTop
            oButton = oitem.Specific
            oButton.Type = ButtonType
            oitem.Width = 20
            oitem.Height = 20
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane
            oButton.Image = PathImagen

            If UDO <> "" Then
                oButton.ChooseFromListUID = UDO
            End If

            Return oitem
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CardCode"></param>
    ''' <remarks></remarks>
    Public Sub AsignaValoresDeCliente(ByVal CardCode As String)
        Dim oEdit As SAPbouiCOM.EditText

        Try

            oEdit = oForm.Items.Item(mc_strCliente).Specific
            oEdit.String = CardCode

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Protected Friend Sub DeshabilitarSocioNegocios(ByRef oForm As SAPbouiCOM.Form)
        Try
            Dim l_strInvVenta As String = DMS_Connector.Configuracion.ParamGenAddon.U_Inven_V.Trim()
            Dim l_strValidarUnidad As String = DMS_Connector.Configuracion.ParamGenAddon.U_ValTipoInv.Trim()

            Dim l_strTipoVeh As String = oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_Tipo", 0).Trim

            If l_strValidarUnidad = "Y" AndAlso l_strInvVenta <> l_strTipoVeh Then 'Entonces el vehiculo esta dentro del Stock

                oForm.Items.Item(mc_strCliente).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)

        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Protected Friend Sub DeshabilitarNumeroUnidad(ByRef oForm As SAPbouiCOM.Form)
        If oForm.Mode <> BoFormMode.fm_FIND_MODE Then

            Dim tieneDocumentos As Boolean

            Dim strCodeReservado As String = DMS_Connector.Configuracion.ParamGenAddon.U_Disp_Res.Trim()
            Dim strCodeFacturado As String = DMS_Connector.Configuracion.ParamGenAddon.U_Disp_V.Trim()

            Dim strDisponibilidad As String = oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_Dispo", 0).Trim

            Try
                Dim otmpForm As Form

                If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_DET_1", False, m_SBO_Application) Then
                    m_SBO_Application.Forms.GetForm("SCGD_DET_1", 0).Select()
                End If


                otmpForm = m_SBO_Application.Forms.ActiveForm

                Dim oEditText As EditText
                Dim oitem As Item = oForm.Items.Item(mc_strUnidad)
                oEditText = oitem.Specific

                Dim strDocentryVehiculo As String = oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("Docentry", 0).Trim
                Dim strCodUnidad As String = oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_Cod_Unid", 0).Trim

                If (strDisponibilidad.Equals(strCodeReservado) Or strDisponibilidad.Equals(strCodeFacturado)) And Not String.IsNullOrEmpty(strDisponibilidad) Then

                    If oForm.AutoManaged Then
                        oitem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    Else
                        oitem.Enabled = False
                    End If
                Else
                    If Not String.IsNullOrEmpty(oEditText.Value.Trim()) Then
                        tieneDocumentos = ValidaVehiculoDocumentos(strCodUnidad, strDocentryVehiculo)
                    Else
                        tieneDocumentos = False
                    End If
                    If oForm.AutoManaged Then
                        oitem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, Not tieneDocumentos)
                    Else
                        oitem.Enabled = Not tieneDocumentos
                    End If
                End If
            Catch ex As Exception
                Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
                Throw ex
            End Try
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="strIDItem"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Protected Friend Sub DesabilitarCombos(ByRef oForm As SAPbouiCOM.Form, _
                                           ByVal strIDItem As String)
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item

        Try
            oItem = oForm.Items.Item(mc_strCliente)
            oItem.Click()

            oItem = oForm.Items.Item(strIDItem)
            oItem.Enabled = False
            cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
            Throw ex
        End Try


    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="strIDItem"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Protected Friend Sub HabilitarCombos(ByRef oForm As SAPbouiCOM.Form, _
                                          ByVal strIDItem As String)
        Dim cboCombo As ComboBox
        Dim oItem As Item
        Try
            If oForm IsNot Nothing Then
                oItem = oForm.Items.Item(strIDItem)
                oItem.Enabled = True
                cboCombo = CType(oItem.Specific, ComboBox)
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="status"></param>
    ''' <param name="p_oForm"></param>
    ''' <remarks></remarks>
    Private Sub HabilitarControles(ByRef status As Boolean, ByRef p_oForm As SAPbouiCOM.Form)
        Try
            For Each item As SAPbouiCOM.Item In p_oForm.Items
                If item.Type = BoFormItemTypes.it_EDIT Or item.Type = BoFormItemTypes.it_COMBO_BOX Or item.Type = BoFormItemTypes.it_MATRIX Then
                    If Not (item.UniqueID = mc_strCliente) AndAlso Not (item.UniqueID = mc_strPlaca) Then
                        item.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    End If
                End If
            Next
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="strQuery"></param>
    ''' <param name="strIDItem"></param>
    ''' <param name="blnSeleccionarValor"></param>
    ''' <param name="strIDCampoBD"></param>
    ''' <param name="blnCargandoVehículo"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Protected Friend Sub CargarValidValuesEnCombos(ByRef oForm As Form, _
                                                   ByVal strQuery As String, _
                                                   ByRef strIDItem As String, _
                                                   ByVal blnSeleccionarValor As Boolean, _
                                                   ByVal strIDCampoBD As String, _
                                                   ByVal blnCargandoVehículo As Boolean)
        Dim cboCombo As ComboBox
        Dim oItem As Item
        Dim strValorASeleccionar As String = ""

        Try
            oItem = oForm.Items.Item(strIDItem)
            cboCombo = CType(oItem.Specific, ComboBox)

            If Not blnSeleccionarValor AndAlso Not String.IsNullOrEmpty(strIDCampoBD) Then
                strValorASeleccionar = oForm.DataSources.DBDataSources.Item(mc_strVehiculo).GetValue(strIDCampoBD, 0)
                strValorASeleccionar = strValorASeleccionar.Trim()
            End If
            Utilitarios.CargarValidValuesEnCombos(cboCombo.ValidValues, strQuery)
            If strValorASeleccionar <> "" _
            AndAlso oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE _
            AndAlso (blnSeleccionarValor Or (Not blnSeleccionarValor AndAlso Not m_blnAgregarComponentes AndAlso m_blnHayVehículo)) Then
                oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue(strIDCampoBD, 0, strValorASeleccionar)
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pval"></param>
    ''' <param name="oTmpForm"></param>
    ''' <param name="FormUID"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Protected Friend Sub VincularChooseFromList(ByRef pval As SAPbouiCOM.ItemEvent, _
                                                ByRef oTmpForm As SAPbouiCOM.Form, _
                                                ByRef FormUID As String)

        '*******************************************************************    
        'Propósito:  Vincular el ChooseFromList a los items respectivos
        '
        'Acepta:    ByRef pval As SAPbouiCOM.ItemEvent,
        '           ByRef oTmpForm As SAPbouiCOM.Form, 
        '           ByRef FormUID As String
        'Retorna:   Ninguno
        'Desarrollador: Yeiner
        'Fecha: 16 Nov 2006
        '********************************************************************
        Try
            Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
            oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)

            Dim sCFL_ID As String
            sCFL_ID = oCFLEvento.ChooseFromListUID

            Dim oCFL As SAPbouiCOM.ChooseFromList
            oCFL = oTmpForm.ChooseFromLists.Item(sCFL_ID)

            If Not oTmpForm Is Nothing Then
                If pval.ItemUID = mc_strCliente Then

                    If Not oCFLEvento.SelectedObjects Is Nothing Then

                        Dim oDataTable As SAPbouiCOM.DataTable
                        oDataTable = oCFLEvento.SelectedObjects
                        Dim val As String

                        If oDataTable.Rows.Count = 0 Then Exit Sub

                        val = CStr(oDataTable.GetValue("CardCode", 0))
                        oTmpForm.DataSources.UserDataSources.Item(mc_strNumBP).ValueEx = val
                        val = CStr(oDataTable.GetValue("CardName", 0))
                        oTmpForm.DataSources.UserDataSources.Item(mc_strNombreBP).ValueEx = val
                    End If

                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CargarDesdeXML(ByRef strFileName As String) As String
        '*******************************************************************    
        'Propósito:  Se encarga de cargar las formas desde el archivo XML,
        '             tomando como parámetro el nombre del archivo.
        '
        'Acepta:    Ninguno
        'Retorna:   Ninguno
        'Desarrollador: Yeiner
        'Fecha: 19 Abril 2006
        '********************************************************************
        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String

        strPath = System.Windows.Forms.Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="udtVehiculo"></param>
    ''' <param name="oDataTable"></param>
    ''' <param name="strCardCode"></param>
    ''' <param name="strCardName"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub AsignaValoresVehiculo(ByRef udtVehiculo As VehiculoUDT, _
                                          ByRef oDataTable As SAPbouiCOM.DataTable, ByVal strCardCode As String, ByVal strCardName As String)

        Try
            udtVehiculo = New VehiculoUDT

            With udtVehiculo
                .NumCliente = oDataTable.GetValue(strCardCode, 0)
                .DetCliente = oDataTable.GetValue(strCardName, 0)
            End With

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="udtVehiculo"></param>
    ''' <param name="oForm"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub AsignaValoresEditTextUI(ByRef udtVehiculo As VehiculoUDT, _
                                       ByRef oForm As SAPbouiCOM.Form)
        '*******************************************************************    
        'Nombre: AsignaValoresEditTextUI()
        'Propósito:  Asigna el Codigo del Cliente y la Descripción a los campos de texto.
        '
        'Acepta:    ByRef udtVehiculo As VehiculoUDT,
        '           ByRef oform As SAPbouiCOM.Form
        'Retorna:   Ninguno
        'Desarrollador: Yeiner
        'Fecha: 29 Nov 2006
        '********************************************************************

        Try
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_CardCode", 0, udtVehiculo.NumCliente)
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_CardName", 0, udtVehiculo.DetCliente)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' Maneja los eventos de validación
    ''' </summary>
    ''' <param name="FormUID"></param>
    ''' <param name="pVal"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Public Sub ManejadorEventoValidate(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.ActionSuccess Then
                If pVal.ItemUID = "mtx_0" Then
                    Select Case pVal.ColUID
                        Case "Col_4"
                            Dim oform As SAPbouiCOM.Form
                            oform = m_SBO_Application.Forms.Item(FormUID)
                            CalculaTotalLineasAcc(oform)
                            CalculaTotalAccesorios(oform)
                    End Select
                End If

            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FormUID"></param>
    ''' <param name="pVal"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <param name="p_blnUsaCosteoVehículo"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                        ByRef pVal As SAPbouiCOM.ItemEvent, _
                                        ByRef BubbleEvent As Boolean, _
                                        ByRef p_blnUsaCosteoVehículo As Boolean)

        Static blnActualizarCombos As Boolean = False

        If pVal.ItemUID = mc_strbtnArchivos AndAlso pVal.ActionSuccess AndAlso (oForm.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE OrElse oForm.Mode = SAPbouiCOM.BoFormMode.fm_EDIT_MODE OrElse oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE OrElse oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then

            Dim tr As System.Threading.Thread = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf CargaDialogo))
            tr.CurrentUICulture = My.Resources.Resource.Culture
            tr.SetApartmentState(Threading.ApartmentState.STA)
            tr.Start()

        End If

        Select Case pVal.ItemUID

            Case "btnArtVent"
                ButtonSeleccionArticuloVenta(FormUID, pVal, BubbleEvent)

            Case "btnColor", "btnColTap"
                ButtonSeleccionColorVehiculo(FormUID, pVal, BubbleEvent)

            Case "1"
                If pVal.BeforeAction Then 'BEFORE

                    If pVal.FormMode = BoFormMode.fm_UPDATE_MODE Then
                        CargaVehiculo(FormUID)
                    End If

                    If pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                        Dim strValidaCantidadVIN As String = String.Empty
                        Dim strValidaVIN As String = String.Empty
                        Dim strValidaUnidadVacia As String = String.Empty
                        Dim strDispoReservado As String = String.Empty
                        Dim strFechaRes As String = String.Empty
                        Dim strModeoEsti As String = String.Empty
                        Dim strValidaPlaca As String = String.Empty
                        Dim strTipoInventario As String = oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_Tipo", 0).Trim
                        Dim strDispoVehiculo As String = oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_Dispo", 0).Trim
                        Dim strFechaReservaVehiculo As String = oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_FchRsva", 0).Trim
                        Dim DocentryVehiculo As String = oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("Docentry", 0)

                        strValidaCantidadVIN = DMS_Connector.Configuracion.ParamGenAddon.U_ValongVIN.Trim()
                        strValidaVIN = DMS_Connector.Configuracion.ParamGenAddon.U_SCGD_VIN.Trim()
                        strValidaUnidadVacia = DMS_Connector.Configuracion.ParamGenAddon.U_SCGD_Uni.Trim()
                        strDispoReservado = DMS_Connector.Configuracion.ParamGenAddon.U_Disp_Res.Trim()
                        strFechaRes = DMS_Connector.Configuracion.ParamGenAddon.U_FechaRes.Trim()
                        strModeoEsti = DMS_Connector.Configuracion.ParamGenAddon.U_EspVehic.Trim()
                        strValidaPlaca = DMS_Connector.Configuracion.ParamGenAddon.U_SCGD_Pla.Trim()

                        'Valida que la unidad tenga un Inventario Definido
                        If String.IsNullOrEmpty(strTipoInventario) Then
                            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ValidaTipoInvCreaUnidad, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                            BubbleEvent = False
                            Exit Sub
                        End If


                        'Valida el VIN
                        If Not String.IsNullOrEmpty(strValidaVIN) AndAlso strValidaVIN = "Y" Then
                            If ValidarNumeroVIN(DocentryVehiculo) Then
                                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeExisteVIN, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                BubbleEvent = False
                                Exit Sub
                            End If
                        End If

                        'Valida el total de Caracteres del VIN
                        If Not String.IsNullOrEmpty(strValidaCantidadVIN) AndAlso strValidaCantidadVIN = "Y" Then

                            If Utilitarios.ValidarLongitudVIN(oForm, m_oCompany, "@SCGD_VEHICULO", "U_Num_VIN") Then
                                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorLongitudVIN, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                BubbleEvent = False
                                Exit Sub
                            End If
                        End If

                        'Valida Fecha de Reserva
                        If Not String.IsNullOrEmpty(strFechaRes) AndAlso (strFechaRes = "Y") Then

                            If (strDispoVehiculo = strDispoReservado) Then

                                If String.IsNullOrEmpty(strFechaReservaVehiculo) Then
                                    m_SBO_Application.StatusBar.SetText("Debe Ingresar Fecha de Reserva", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                    BubbleEvent = False
                                    Exit Sub
                                End If
                            Else
                                If (strDispoVehiculo <> strDispoReservado) Then
                                    oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_FchRsva", 0, "")
                                    oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_VenRes", 0, "")
                                End If
                            End If
                        End If


                        Select Case pVal.FormMode

                            Case SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

                                ValidarUnidadExista(BubbleEvent, True, strValidaPlaca)
                                If BubbleEvent Then
                                    ValidaEstiloyModelo(BubbleEvent, strModeoEsti)
                                End If

                            Case SAPbouiCOM.BoFormMode.fm_ADD_MODE

                                'Valida la Unidad no este repetida de acuerdo a la placa y Codigo Unidad
                                If BubbleEvent = True Then
                                    ValidarUnidadExista(BubbleEvent, False, strValidaPlaca)
                                End If


                                'Valida el Codigo de Unidad
                                If Not String.IsNullOrEmpty(strValidaUnidadVacia) AndAlso strValidaUnidadVacia = "Y" AndAlso BubbleEvent = True Then
                                    Dim codigoVehiculo As String = oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_Cod_Unid", 0).Trim()

                                    If String.IsNullOrEmpty(codigoVehiculo) Then
                                        m_SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeUnidadVacia, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                End If


                                'Valida que la unidad tenga Marca, Estilo y/o Modelo
                                If BubbleEvent = True Then
                                    ValidaEstiloyModelo(BubbleEvent, strModeoEsti)
                                End If


                                'Se valida que la unidad no se busco con el metodo de "ValidaOpcionNoRepetida" ya que el formulario queda en modo "Crear"
                                If pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE AndAlso BubbleEvent = True Then
                                    'Le asigno el Code que sigue
                                    If String.IsNullOrEmpty(DocentryVehiculo) AndAlso BubbleEvent Then
                                        DocentryVehiculo = Utilitarios.EjecutarConsulta(DMS_Connector.Queries.GetStrSpecificQuery("strAutoKeyVEH"))
                                        oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("Code", 0, DocentryVehiculo)
                                    End If
                                End If

                        End Select

                    End If
                    If BubbleEvent Then
                        ManejarModoFormulario(oForm)
                    End If

                ElseIf pVal.ActionSuccess Then  'ACTION SUCCESS
                    AgregaLineaMatrizBonos(oForm)
                    If Not oVehiculo Is Nothing Then
                        ActualizaBonosContrato(oForm, oVehiculo)
                    End If
                End If

            Case "btnEliBon"
                If pVal.ActionSuccess Then
                    EliminarLíneasMatrizBonos(FormUID)
                End If

            Case "Folder1"
                oForm = m_SBO_Application.Forms.Item(FormUID)
                CalculaTotalBonos(oForm)
            Case "del"
                CalculaTotalAccesorios(oForm)
        End Select

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oForm"></param>
    ''' <param name="p_oVehiculo"></param>
    ''' <remarks></remarks>
    Private Sub ActualizaBonosContrato(ByVal p_oForm As Form, ByVal p_oVehiculo As Vehiculo)

        Dim dbTipoCambio As Double
        Dim strMonedaSistema As String
        Dim strMonedaLocal As String

        Try

            DMS_Connector.Helpers.GetCurrencies(strMonedaLocal, strMonedaSistema)
            If strMonedaLocal <> strMonedaSistema Then
                dbTipoCambio = DMS_Connector.Helpers.GetCurrencyRate(strMonedaSistema, Date.Now)
            Else
                dbTipoCambio = 1
            End If

            If Not String.IsNullOrEmpty(p_oVehiculo.U_ContratoV.Trim) Then
                If ValidaContrato(p_oVehiculo.U_ContratoV.Trim, p_oForm) Then
                    AgregaBonosUnidad(p_oForm, dbTipoCambio, Date.Now, p_oVehiculo)
                End If
            End If

        Catch ex As Exception
            m_SBO_Application.SetStatusBarMessage(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strDocEntry"></param>
    ''' <param name="p_oForm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidaContrato(ByVal p_strDocEntry As String, ByVal p_oForm As Form) As Boolean
        Try
            Dim strConsulta As String = String.Empty
            Dim m_oDataTableConsulta As SAPbouiCOM.DataTable
            Dim strRetorno As String = String.Empty

            strConsulta = "select isnull(max(DocEntry),'-1') from [@SCGD_CVENTA] with(nolock) where U_Estado not in ((select MAX(U_Prio) from dbo.[@SCGD_ADMIN9] with(nolock)),'0') and DocEntry = '{0}' "
            strConsulta = String.Format(strConsulta, p_strDocEntry)

            If Utilitarios.ValidaExisteDataTable(p_oForm, "tConsulta") Then
                m_oDataTableConsulta = p_oForm.DataSources.DataTables.Item("tConsulta")
            Else
                m_oDataTableConsulta = p_oForm.DataSources.DataTables.Add("tConsulta")
            End If

            m_oDataTableConsulta.ExecuteQuery(strConsulta)
            strRetorno = m_oDataTableConsulta.GetValue(0, 0).ToString.Trim

            If String.IsNullOrEmpty(strRetorno) OrElse strRetorno = "-1" Then
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oForm"></param>
    ''' <param name="p_dbTipoCambioSistema"></param>
    ''' <param name="p_dtFecha"></param>
    ''' <param name="p_oVehiculo"></param>
    ''' <remarks></remarks>
    Private Sub AgregaBonosUnidad(ByRef p_oForm As Form, ByRef p_dbTipoCambioSistema As Double, ByRef p_dtFecha As Date, ByRef p_oVehiculo As Vehiculo)

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oContratoVentas As SAPbobsCOM.GeneralData
        Dim oChildrenBonos As SAPbobsCOM.GeneralDataCollection
        Dim oChildrenVehiculos As SAPbobsCOM.GeneralDataCollection
        Dim oChildBono As SAPbobsCOM.GeneralData
        Dim oChildBonoVeh As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim oGeneralServiceVeh As SAPbobsCOM.GeneralService
        Dim oVehiculo As SAPbobsCOM.GeneralData
        Dim oChildrenBonosVeh As SAPbobsCOM.GeneralDataCollection
        Dim listLineasBonos As List(Of Integer)
        Dim objGlobal As DMSOneFramework.BLSBO.GlobalFunctionsSBO
        Dim strMonedaLocal As String = String.Empty
        Dim strMonedaSistema As String = String.Empty
        Dim strMonedaContrato As String = String.Empty
        Dim strUnidad As String = String.Empty
        Dim dbMonto As Double = 0
        Dim strMoneda As String = String.Empty
        Dim dbTotalBonos As Double = 0
        Dim m_strCSucu As String = String.Empty
        Dim m_strSucu As String = String.Empty
        Dim m_strEstado As String = String.Empty
        Dim m_strCliente As String = String.Empty
        Dim m_strCodVen As String = String.Empty
        Dim m_strVendedor As String = String.Empty
        Dim strListaUsuarios As New List(Of String)


        Try
            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralServiceVeh = oCompanyService.GetGeneralService("SCGD_VEH")
            oGeneralParams = oGeneralServiceVeh.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("Code", p_oVehiculo.Code.Trim)
            oVehiculo = oGeneralServiceVeh.GetByParams(oGeneralParams)
            oChildrenBonosVeh = oVehiculo.Child("SCGD_BONOXVEH")

            If ActualizarBonosContratos(p_oVehiculo.BonosXVehiculo, oChildrenBonosVeh) Then
                DMS_Connector.Helpers.GetCurrencies(strMonedaLocal, strMonedaSistema)

                oGeneralService = oCompanyService.GetGeneralService("SCGD_CVT")
                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("DocEntry", p_oVehiculo.U_ContratoV.Trim)
                oContratoVentas = oGeneralService.GetByParams(oGeneralParams)

                strMonedaContrato = oContratoVentas.GetProperty("U_Moneda").ToString.Trim()
                m_strCSucu = oContratoVentas.GetProperty("U_CSucu").ToString.Trim()
                m_strSucu = oContratoVentas.GetProperty("U_Sucu").ToString.Trim()
                m_strEstado = oContratoVentas.GetProperty("U_Estado").ToString.Trim()
                m_strCliente = oContratoVentas.GetProperty("U_CardName").ToString.Trim()
                m_strVendedor = oContratoVentas.GetProperty("U_SlpName").ToString.Trim()
                m_strCodVen = oContratoVentas.GetProperty("U_FooVend").ToString.Trim()
                oChildrenBonos = oContratoVentas.Child("SCGD_BONOXCONT")
                oChildrenVehiculos = oContratoVentas.Child("SCGD_VEHIXCONT")
                dbTotalBonos = 0
                strUnidad = p_oVehiculo.U_Cod_Unid.Trim
                strMoneda = p_oVehiculo.U_Moneda
                listLineasBonos = RetornaLineasBonosUnidad(oChildrenBonos, strUnidad)

                For i As Integer = 0 To oChildrenBonosVeh.Count - 1
                    oChildBonoVeh = oChildrenBonosVeh.Item(i)
                    If Not String.IsNullOrEmpty(oChildBonoVeh.GetProperty("U_Bono")) Then
                        dbMonto = oChildBonoVeh.GetProperty("U_Monto")
                        dbMonto = Utilitarios.ManejoMultimoneda(dbMonto, strMonedaLocal, strMonedaSistema, strMoneda, strMonedaContrato, p_dbTipoCambioSistema.ToString, p_dtFecha, n, m_oCompany)
                        If listLineasBonos.Count > 0 Then
                            For index As Integer = 0 To oChildrenBonos.Count - 1
                                oChildBono = oChildrenBonos.Item(index)
                                If CInt(oChildBono.GetProperty("LineId")) = listLineasBonos.Item(0) Then
                                    oChildBono.SetProperty("U_Bono", oChildBonoVeh.GetProperty("U_Bono"))
                                    oChildBono.SetProperty("U_Monto", dbMonto)
                                    listLineasBonos.Remove(listLineasBonos.Item(0))
                                    Exit For
                                End If
                            Next
                        Else
                            oChildBono = oChildrenBonos.Add()
                            oChildBono.SetProperty("U_Unidad", strUnidad)
                            oChildBono.SetProperty("U_Bono", oChildBonoVeh.GetProperty("U_Bono"))
                            oChildBono.SetProperty("U_Monto", dbMonto)
                        End If
                        dbTotalBonos += dbMonto
                    End If
                Next

                If listLineasBonos.Count > 0 Then
                    For index As Integer = oChildrenBonos.Count - 1 To 0 Step -1
                        oChildBono = oChildrenBonos.Item(index)
                        If CInt(oChildBono.GetProperty("LineId")) = listLineasBonos.Item(listLineasBonos.Count - 1) Then
                            oChildrenBonos.Remove(index)
                            listLineasBonos.Remove(listLineasBonos.Item(listLineasBonos.Count - 1))
                            If listLineasBonos.Count = 0 Then Exit For
                        End If
                    Next
                End If
                ActualizaBonosVehiculosVenta(strUnidad, dbTotalBonos, oChildrenVehiculos)
                dbTotalBonos = 0
                For x As Integer = oChildrenBonos.Count - 1 To 0 Step -1
                    oChildBono = oChildrenBonos.Item(x)
                    dbTotalBonos += oChildBono.GetProperty("U_Monto")
                Next
                CalculaDatosVentaContrato(oContratoVentas, dbTotalBonos)

                oGeneralService.Update(oContratoVentas)

                strListaUsuarios = GeneraListaUsuariosMensajes(p_oForm, m_strCSucu, m_strSucu, m_strEstado, m_strCodVen)
                If strListaUsuarios.Count > 0 AndAlso Not String.IsNullOrEmpty(strListaUsuarios(0)) Then
                    EnviarMensajesAprobacion(p_oForm, m_strEstado, p_oVehiculo.U_ContratoV.Trim, m_strCliente, m_strVendedor, strListaUsuarios)
                End If

            End If
        Catch ex As Exception
            m_SBO_Application.SetStatusBarMessage(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oForm"></param>
    ''' <param name="p_strCodSucu"></param>
    ''' <param name="p_strSucursal"></param>
    ''' <param name="p_strEstado"></param>
    ''' <param name="p_strVendedor"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GeneraListaUsuariosMensajes(ByVal p_oForm As Form, ByVal p_strCodSucu As String, ByVal p_strSucursal As String,
                                                ByVal p_strEstado As String, ByVal p_strVendedor As String) As List(Of String)
        Dim strCodigoNivel As String = String.Empty
        Dim strPEmp As String = String.Empty
        Dim strUsuario As String = String.Empty
        Dim strUsua As String = String.Empty
        Dim strLista As New List(Of String)
        Dim m_oDataTableConsulta As SAPbouiCOM.DataTable

        m_oDataTableConsulta = p_oForm.DataSources.DataTables.Item("tConsulta")

        m_oDataTableConsulta.ExecuteQuery(String.Format("SELECT U_PEmp FROM [@SCGD_ADMIN9] with(nolock) WHERE U_Prio = '{0}'", p_strEstado))
        strPEmp = m_oDataTableConsulta.GetValue(0, 0)

        If String.IsNullOrEmpty(strPEmp) Or strPEmp = "N" Then

            m_oDataTableConsulta.ExecuteQuery(String.Format("SELECT U_Codigo FROM [@SCGD_ADMIN9] with(nolock) WHERE U_Prio = '{0}'", p_strEstado))
            strCodigoNivel = m_oDataTableConsulta.GetValue(0, 0)
            m_oDataTableConsulta.ExecuteQuery("SELECT U_Usua FROM [@SCGD_MSJS1] with(nolock) WHERE U_CNAp = '" & strCodigoNivel & "' AND U_CSucu = '" & p_strCodSucu & "' and U_RMsj = 'Y'")

            For posicion As Integer = 0 To m_oDataTableConsulta.Rows.Count - 1
                strUsua = m_oDataTableConsulta.GetValue("U_Usua", posicion)

                If Not String.IsNullOrEmpty(strUsua) Then
                    strLista.Add(strUsua)
                End If
            Next
        ElseIf strPEmp = "Y" Then
            m_oDataTableConsulta.ExecuteQuery(String.Format("select ou.USER_CODE from OHEM oh with(nolock) inner join OUSR ou with(nolock) on oh.userId = ou.USERID where salesPrson = '{0}'", p_strVendedor))
            strUsuario = m_oDataTableConsulta.GetValue(0, 0)
            strLista.Add(strUsuario)
        End If

        Return strLista
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oForm"></param>
    ''' <param name="p_intEstado"></param>
    ''' <param name="p_strIDContrato"></param>
    ''' <param name="p_strNombreCliente"></param>
    ''' <param name="p_strNombreVendedor"></param>
    ''' <param name="p_strLista"></param>
    ''' <remarks></remarks>
    Private Sub EnviarMensajesAprobacion(ByVal p_oForm As Form, ByVal p_intEstado As Integer, ByVal p_strIDContrato As String, _
                                         ByVal p_strNombreCliente As String, ByVal p_strNombreVendedor As String, ByVal p_strLista As List(Of String))
        Dim strMensaje As String
        Dim oMsg As SAPbobsCOM.Messages
        Dim intResultado As Integer
        Dim strError As String = ""
        Dim strUsuarioMensaje As String = ""
        Dim strEstado As String
        Dim hashUsuarios As Hashtable
        Dim m_oDataTableConsulta As SAPbouiCOM.DataTable

        m_oDataTableConsulta = p_oForm.DataSources.DataTables.Item("tConsulta")

        m_oDataTableConsulta.ExecuteQuery(String.Format("SELECT U_Estado FROM [@SCGD_ADMIN9] with(nolock) WHERE U_Prio = '{0}'", p_intEstado))
        strEstado = m_oDataTableConsulta.GetValue(0, 0)

        If p_strLista.Count > 0 Then
            strMensaje = My.Resources.Resource.ElContratoVenta & p_strIDContrato & My.Resources.Resource.ContratoVendedor & p_strNombreVendedor & " " & My.Resources.Resource.Contratode & p_strNombreCliente & My.Resources.Resource.RequiereRevision
            hashUsuarios = New Hashtable()

            oMsg = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
            oMsg.Priority = SAPbobsCOM.BoMsgPriorities.pr_High
            oMsg.MessageText = strMensaje
            oMsg.Subject = My.Resources.Resource.MensajeContratoAprobado

            For Each strUsuarioMensaje In p_strLista
                If Not hashUsuarios.ContainsKey(strUsuarioMensaje) Then
                    hashUsuarios.Add(strUsuarioMensaje, strUsuarioMensaje)

                    oMsg.Recipients.Add()
                    oMsg.Recipients.UserCode = strUsuarioMensaje
                    oMsg.Recipients.NameTo = strUsuarioMensaje
                    oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
                End If
            Next

            intResultado = oMsg.Add
            If (intResultado <> 0) Then
                m_oCompany.GetLastError(intResultado, strError)
                Throw New ExceptionsSBO(intResultado, strError)
            End If
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strUnidad"></param>
    ''' <param name="p_dbTotalBonos"></param>
    ''' <param name="p_oChildrenVehiculos"></param>
    ''' <remarks></remarks>
    Private Sub ActualizaBonosVehiculosVenta(ByVal p_strUnidad As String, ByVal p_dbTotalBonos As Double, ByRef p_oChildrenVehiculos As GeneralDataCollection)
        Dim oChildVehiculo As SAPbobsCOM.GeneralData

        Try
            For i As Integer = 0 To p_oChildrenVehiculos.Count - 1
                oChildVehiculo = p_oChildrenVehiculos.Item(i)
                If oChildVehiculo.GetProperty("U_Cod_Unid").ToString.Trim() = p_strUnidad Then
                    oChildVehiculo.SetProperty("U_Bono", p_dbTotalBonos)
                    Exit For
                End If
            Next
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oContratoVentas"></param>
    ''' <param name="p_dbTotalBonos"></param>
    ''' <remarks></remarks>
    Private Sub CalculaDatosVentaContrato(ByRef p_oContratoVentas As GeneralData, ByVal p_dbTotalBonos As Double)
        Dim strGeneraFAccesorios As String
        Dim dbPrecioVenta,
            dbValor,
            dbImpuestosAccesorios,
            dbGastosInscripcion,
            dbGastosPrenda,
            dbOtrosL,
            dbOtrosCostos,
            dbPorcentajeDescuento,
            dbDescuento,
            dbTotalAntesImpuestos,
            dbImpuestos,
            dbTotal,
            dbPrima,
            dbUsado,
            dbDeudaUsado,
            dbNotaDebito,
            dbFinanciamiento,
            dbOtrosC,
            dbPagos,
            dbTramites,
            dbPagoContraEntrega,
            dbAccesorios,
            dbTotalAccesorios,
            dbDescuentoLineaAccesorios As Double

        Try
            strGeneraFAccesorios = p_oContratoVentas.GetProperty("U_GenFaAcc")
            dbPrecioVenta = p_oContratoVentas.GetProperty("U_Pre_Vta")
            dbImpuestosAccesorios = p_oContratoVentas.GetProperty("U_Acc_Imp")
            dbGastosInscripcion = p_oContratoVentas.GetProperty("U_Gas_Ins")
            dbGastosPrenda = p_oContratoVentas.GetProperty("U_Gas_Pre")
            dbOtrosL = p_oContratoVentas.GetProperty("U_Otros_L")
            dbOtrosCostos = p_oContratoVentas.GetProperty("U_OtrCos")
            dbPorcentajeDescuento = p_oContratoVentas.GetProperty("U_Por_Desc")
            dbDescuento = p_oContratoVentas.GetProperty("U_Nota_Cre")
            dbTotalAntesImpuestos = p_oContratoVentas.GetProperty("U_AntImp")
            dbImpuestos = p_oContratoVentas.GetProperty("U_Pre_Imp")
            dbTotal = p_oContratoVentas.GetProperty("U_DocTotal")
            dbPrima = p_oContratoVentas.GetProperty("U_Deposito")
            dbUsado = p_oContratoVentas.GetProperty("U_Mon_Usa")
            dbDeudaUsado = p_oContratoVentas.GetProperty("U_Deu_Usa")
            dbNotaDebito = p_oContratoVentas.GetProperty("U_Nota_Deb")
            dbFinanciamiento = p_oContratoVentas.GetProperty("U_Financia")
            dbOtrosC = p_oContratoVentas.GetProperty("U_Otros_C")
            dbPagos = p_oContratoVentas.GetProperty("U_Pagos")
            dbTramites = p_oContratoVentas.GetProperty("U_Tot_Tram")
            dbPagoContraEntrega = p_oContratoVentas.GetProperty("U_Pag_ent")

            dbDescuentoLineaAccesorios = CalculaDecuentoAccesorios(p_oContratoVentas, dbAccesorios, dbTotalAccesorios)

            dbValor = dbPrecioVenta

            If strGeneraFAccesorios = "N" Then
                dbValor += dbAccesorios
            End If

            dbValor += dbGastosInscripcion
            dbValor += dbGastosPrenda
            dbValor += dbOtrosL
            dbValor -= p_dbTotalBonos
            dbValor += dbOtrosCostos
            dbDescuento = (dbValor / 100) * dbPorcentajeDescuento

            If strGeneraFAccesorios = "N" Then
                dbValor -= dbDescuentoLineaAccesorios
            End If

            dbValor -= dbDescuento
            dbTotalAntesImpuestos = dbValor
            dbImpuestos = CalculaImpuestosContrato(oVehiculo.U_Cod_Unid, p_oContratoVentas, p_dbTotalBonos, strGeneraFAccesorios)
            dbValor += dbImpuestos
            dbTotal = dbValor
            dbValor -= dbPrima
            dbValor -= dbUsado
            dbValor += dbDeudaUsado
            dbValor += dbNotaDebito
            dbValor -= dbFinanciamiento
            dbValor += dbOtrosC
            dbValor -= dbPagos
            dbValor += dbTramites

            If strGeneraFAccesorios = "N" Then
                dbPagoContraEntrega = dbValor
            Else
                dbPagoContraEntrega = dbValor + dbTotalAccesorios + dbImpuestosAccesorios
            End If

            p_oContratoVentas.SetProperty("U_BonoDV", p_dbTotalBonos)
            p_oContratoVentas.SetProperty("U_BonoDV2", p_dbTotalBonos)
            p_oContratoVentas.SetProperty("U_AntImp", dbTotalAntesImpuestos)
            p_oContratoVentas.SetProperty("U_Pre_Imp", dbImpuestos)
            p_oContratoVentas.SetProperty("U_Nota_Cre", dbDescuento)
            p_oContratoVentas.SetProperty("U_DocTotal", dbTotal)
            p_oContratoVentas.SetProperty("U_Pag_ent", dbPagoContraEntrega)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oContratoVentas"></param>
    ''' <param name="p_dbAccesorios"></param>
    ''' <param name="p_dbTotalAccesorios"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CalculaDecuentoAccesorios(ByRef p_oContratoVentas As GeneralData, ByRef p_dbAccesorios As Double, ByRef p_dbTotalAccesorios As Double) As Double
        Dim strAccesorio As String
        Dim dbCantidad,
            dbPrecio,
            dbPrecioTotal,
            dbPorcDescuento,
            dbDescuentoLinea,
            dbDescuentoLineaTotal As Double
        Dim oChildrenAccesorios As GeneralDataCollection
        Dim oChildAccesorio As GeneralData
        Try
            dbDescuentoLineaTotal = 0
            oChildrenAccesorios = p_oContratoVentas.Child("SCGD_ACCXCONT")
            If oChildrenAccesorios.Count > 0 Then
                For i As Integer = 0 To oChildrenAccesorios.Count - 1
                    oChildAccesorio = oChildrenAccesorios.Item(i)
                    strAccesorio = oChildAccesorio.GetProperty("U_Acc")
                    If Not String.IsNullOrEmpty(strAccesorio) Then
                        dbCantidad = oChildAccesorio.GetProperty("U_Cant_Acc")
                        dbPrecio = oChildAccesorio.GetProperty("U_SCGD_AccPrecio")
                        dbPorcDescuento = oChildAccesorio.GetProperty("U_Desc_Acc")
                        dbPrecioTotal = oChildAccesorio.GetProperty("U_PrTo_Acc")
                        p_dbAccesorios += dbCantidad * dbPrecio
                        p_dbTotalAccesorios += dbPrecioTotal
                        dbDescuentoLinea = ((dbCantidad * dbPrecio) / 100) * dbPorcDescuento
                        dbDescuentoLineaTotal += dbDescuentoLinea
                    End If
                Next
            End If
            Return dbDescuentoLineaTotal
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strCodUnid"></param>
    ''' <param name="p_oContratoVentas"></param>
    ''' <param name="p_dbTotalBonos"></param>
    ''' <param name="p_strGeneraFAccesorios"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CalculaImpuestosContrato(ByVal p_strCodUnid As String, ByVal p_oContratoVentas As GeneralData, ByVal p_dbTotalBonos As Double, ByVal p_strGeneraFAccesorios As String) As Double

        Dim oChildrenVehiculos As GeneralDataCollection
        Dim oChildrenAccesorios As GeneralDataCollection
        Dim oChildrenOtrosCostos As GeneralDataCollection
        Dim oChildVehiculos As GeneralData
        Dim oChildAccesorios As GeneralData
        Dim oChildOtrosCostos As GeneralData
        Dim m_strUnidadVehiculo As String = String.Empty
        Dim m_strUnidad As String = String.Empty
        Dim dbPrecioVenta As Double = 0
        Dim dbBono As Double = 0
        Dim dbMontoAcumulado As Double = 0
        Dim m_strImpuestos As String = String.Empty
        Dim dbPrecioTotal As Double = 0
        Dim dbAcumuladoOtrosCostos As Double = 0
        Dim m_htUnidadOtrosCostos As Hashtable
        Dim dbRate As Double = 0
        Dim dbPrecioImpTotal As Double = 0
        Dim dbAcumuladoImpuestos As Double = 0
        Dim dbPorcDescuentoLinea As Decimal = 0
        Dim dbPorcDescuentoGlobal As Double = 0
        Dim m_decDescuentoGlobal As Decimal = 0
        Dim dtFechaContrato As DateTime
        Try
            m_htUnidadOtrosCostos = New Hashtable()
            m_strUnidadVehiculo = p_strCodUnid

            dbPorcDescuentoGlobal = p_oContratoVentas.GetProperty("U_Por_Desc")

            If dbPorcDescuentoGlobal <> 0 Then
                dbPorcDescuentoGlobal /= 100
            End If
            If New Date(1899, 12, 30) <> p_oContratoVentas.GetProperty("U_SCGD_FDc") Then
                dtFechaContrato = p_oContratoVentas.GetProperty("U_SCGD_FDc")
            ElseIf New Date(1899, 12, 30) <> p_oContratoVentas.GetProperty("U_DocDate") Then
                dtFechaContrato = p_oContratoVentas.GetProperty("U_DocDate")
            Else
                dtFechaContrato = DateTime.Now
            End If
            oChildrenOtrosCostos = p_oContratoVentas.Child("SCGD_OTROCXCV")
            oChildrenVehiculos = p_oContratoVentas.Child("SCGD_VEHIXCONT")
            oChildrenAccesorios = p_oContratoVentas.Child("SCGD_ACCXCONT")
            For i As Integer = 0 To oChildrenOtrosCostos.Count - 1

                oChildOtrosCostos = oChildrenOtrosCostos.Item(i)

                m_strUnidad = oChildOtrosCostos.GetProperty("U_Unidad")

                If Not m_htUnidadOtrosCostos.Contains(m_strUnidad) Then
                    m_htUnidadOtrosCostos.Add(m_strUnidad, p_dbTotalBonos)
                Else
                    dbMontoAcumulado = CDbl(m_htUnidadOtrosCostos(m_strUnidad))
                    dbMontoAcumulado += p_dbTotalBonos
                    m_htUnidadOtrosCostos(m_strUnidad) = dbMontoAcumulado
                End If
            Next

            dbPrecioImpTotal = 0

            For i As Integer = 0 To oChildrenVehiculos.Count - 1
                dbPorcDescuentoLinea = 0
                oChildVehiculos = oChildrenVehiculos.Item(i)

                m_strUnidad = oChildVehiculos.GetProperty("U_Cod_Unid")
                dbPrecioVenta = oChildVehiculos.GetProperty("U_Pre_Vta")
                dbBono = oChildVehiculos.GetProperty("U_Bono")
                m_strImpuestos = oChildVehiculos.GetProperty("U_Impuesto")
                dbAcumuladoOtrosCostos = CDbl(m_htUnidadOtrosCostos(m_strUnidad))

                dbPorcDescuentoLinea = oChildVehiculos.GetProperty("U_Desc_Veh")

                If dbPorcDescuentoLinea <> 0 Then
                    dbPorcDescuentoLinea /= 100
                End If

                dbPrecioVenta -= (dbPrecioVenta * dbPorcDescuentoLinea)

                dbPrecioVenta += (dbAcumuladoOtrosCostos - dbBono)

                m_decDescuentoGlobal = dbPrecioVenta * dbPorcDescuentoGlobal
                dbPrecioVenta -= m_decDescuentoGlobal

                If Not String.IsNullOrEmpty(m_strImpuestos) Then
                    dbRate = Utilitarios.RetornaImpuestoVenta(m_strImpuestos, dtFechaContrato)
                    If dbRate <> 0 Then
                        dbRate /= 100
                    End If
                Else
                    dbRate = 0
                End If

                dbPrecioImpTotal += (dbPrecioVenta * dbRate)
            Next

            dbAcumuladoImpuestos += dbPrecioImpTotal

            If p_strGeneraFAccesorios = "N" Then
                dbPrecioImpTotal = 0

                For i As Integer = 0 To oChildrenAccesorios.Count - 1
                    oChildAccesorios = oChildrenAccesorios.Item(i)
                    m_strImpuestos = oChildAccesorios.GetProperty("U_Imp_Acc")
                    dbPrecioTotal = oChildAccesorios.GetProperty("U_PrTo_Acc")

                    If Not String.IsNullOrEmpty(m_strImpuestos) Then
                        dbRate = Utilitarios.RetornaImpuestoVenta(m_strImpuestos, dtFechaContrato)
                        If dbRate <> 0 Then
                            dbRate /= 100
                        End If
                    Else
                        dbRate = 0
                    End If

                    dbPrecioImpTotal += (dbPrecioTotal * dbRate)
                Next

                dbAcumuladoImpuestos += dbPrecioImpTotal
            End If

            Return dbAcumuladoImpuestos
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strNumUnit"></param>
    ''' <param name="p_strCodeVehi"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidaVehiculoDocumentos(ByVal p_strNumUnit As String, ByVal p_strCodeVehi As String) As Boolean
        If Utilitarios.EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrQueryFormat("strConsultaDocumentos"), p_strNumUnit, p_strCodeVehi)) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pVal"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Public Sub EliminarVehiculo(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean)
        Dim numUnidad As String
        Dim strCodeVeh As Integer

        If pVal.BeforeAction Then
            oForm = m_SBO_Application.Forms.Item("SCGD_DET_1")
            numUnidad = oForm.DataSources.DBDataSources.Item(mc_strVehiculo).GetValue(mc_strUDFNoUnidad, 0).Trim
            strCodeVeh = oForm.DataSources.DBDataSources.Item(mc_strVehiculo).GetValue("Docentry", 0).Trim

            If ValidaVehiculoDocumentos(numUnidad, strCodeVeh) Then
                BubbleEvent = False
                Dim mensaje As String = String.Format(My.Resources.Resource.ErrorEliminarVehiculo, numUnidad.Trim())
                m_SBO_Application.StatusBar.SetText(mensaje, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
            End If
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FormUID"></param>
    ''' <param name="pVal"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Public Sub ButtonSeleccionArticuloVenta(ByVal FormUID As String, ByVal pVal As ItemEvent, ByRef BubbleEvent As Boolean)

        If Not pVal.BeforeAction AndAlso pVal.ActionSuccess Then
            oForm = m_SBO_Application.Forms.Item(FormUID)

            If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_VAV", False, m_SBO_Application) Then
                Dim objArticuloVenta As New VehiculoArticuloVenta(m_oCompany, m_SBO_Application)
                objArticuloVenta.FormConfiguracion = oForm
                Call objArticuloVenta.CargaFormulario()
            End If
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FormUID"></param>
    ''' <param name="pVal"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Public Sub ButtonSeleccionColorVehiculo(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.ActionSuccess Then
            oForm = m_SBO_Application.Forms.Item(FormUID)

            If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_VSC", False, m_SBO_Application) Then
                Dim objVehSelecColor As New VehiculoColoresSeleccion(m_oCompany, m_SBO_Application)
                objVehSelecColor.FormConfiguracion = oForm

                If pVal.ItemUID = "btnColor" Then
                    objVehSelecColor.IntTipoConfiguracion = 1
                ElseIf pVal.ItemUID = "btnColTap" Then
                    objVehSelecColor.IntTipoConfiguracion = 2
                End If

                Call objVehSelecColor.CargaFormularioColores()
            End If
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaDialogo()
        Dim strConectionString As String = String.Empty
        Dim oform As SAPbouiCOM.Form
        Dim strNombreTaller As String = String.Empty
        Dim strVeh As String = String.Empty

        Try
            Utilitarios.DevuelveNombreBDTaller(m_SBO_Application, strNombreTaller)
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, strNombreTaller, strConectionString)
            oform = m_SBO_Application.Forms.ActiveForm

            strVeh = oform.DataSources.DBDataSources.Item(mc_strVehiculo).GetValue("Code", 0)
            Dim value As Integer = CInt(strVeh)

            Dim tipoSkin As Integer = Utilitarios.CargarTipoSkin()

            Dim archivoDigital As FrmArchivoDigital = New FrmArchivoDigital(My.Resources.Resource.TituloDialogoArchivo, mc_strVehiculo, value, mc_strTablaArchivosDigitales, strConectionString, 10, tipoSkin)
            archivoDigital.Tag = value

            Dim MyProcs() As Process
            MyProcs = Process.GetProcessesByName("SAP Business One")
            Dim currentProcess As Process = Process.GetCurrentProcess()

            If MyProcs.Length <> 0 Then
                For i As Integer = 0 To MyProcs.Length - 1
                    If MyProcs(i).SessionId = currentProcess.SessionId Then
                        Dim MyWindow As New WindowWrapper(MyProcs(i).MainWindowHandle)
                        archivoDigital.ShowInTaskbar = False
                        archivoDigital.ShowDialog(MyWindow)
                    End If
                Next
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pval"></param>
    ''' <param name="FormUID"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoChooseFromList(ByRef pval As SAPbouiCOM.ItemEvent, _
                                             ByVal FormUID As String, _
                                             ByRef BubbleEvent As Boolean)
        '*******************************************************************    
        'Nombre: ManejadorEventoChooseFromList()
        'Propósito: Se encarga de manejar el evento que genera el ChooseFromList
        'Acepta:    ByRef pval As SAPbouiCOM.ItemEvent, 
        '           ByVal FormUID As String, 
        '           ByRef BubbleEvent As Boolean
        'Retorna:   Ninguno
        'Desarrollador: Yeiner
        'Fecha: 29 Nov 2006
        '********************************************************************

        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)
        Dim sCFL_ID As String
        sCFL_ID = oCFLEvento.ChooseFromListUID
        Dim oForm As SAPbouiCOM.Form
        oForm = m_SBO_Application.Forms.Item(FormUID)
        Dim oCFL As SAPbouiCOM.ChooseFromList
        oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        If oCFLEvento.ActionSuccess Then
            Dim oDataTable As SAPbouiCOM.DataTable
            oDataTable = oCFLEvento.SelectedObjects

            If (pval.ItemUID = mc_strAdd) AndAlso Not (pval.FormMode = BoFormMode.fm_FIND_MODE Or pval.FormMode = BoFormMode.fm_VIEW_MODE) Then

                If Not oCFLEvento.SelectedObjects Is Nothing Then
                    Call AsignarComponente(oDataTable, FormUID)

                End If

            ElseIf pval.ItemUID = mc_strCliente Then

                Dim val As VehiculosCls.VehiculoUDT = Nothing

                If Not oCFLEvento.SelectedObjects Is Nothing Then

                    If pval.FormMode = BoFormMode.fm_FIND_MODE Then
                        Call AsignaValoresVehiculo(val, oDataTable, mc_U_CardCode, mc_U_CardName)
                    Else
                        Call AsignaValoresVehiculo(val, oDataTable, mc_CardCode, mc_CardName)
                    End If


                    Call AsignaValoresEditTextUI(val, oForm)

                End If

            ElseIf pval.ItemUID = mc_strEmpleado Then

                If Not oCFLEvento.SelectedObjects Is Nothing Then

                    oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_VENRES", 0, oDataTable.GetValue("SlpName", 0))

                End If

            End If

            If oForm.Mode = BoFormMode.fm_OK_MODE Then

                oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

            End If



        Else
            If (pval.ItemUID = mc_strAdd) AndAlso Not (pval.FormMode = BoFormMode.fm_FIND_MODE Or pval.FormMode = BoFormMode.fm_VIEW_MODE) Then

                oConditions = m_SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 1
                oCondition.Alias = "U_SCGD_TipoArticulo"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "7"
                oCondition.BracketCloseNum = 1
                oCFL.SetConditions(oConditions)
            End If
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strFormID"></param>
    ''' <param name="p_strCodEstilo"></param>
    ''' <param name="accesorioXEstilo"></param>
    ''' <param name="p_strCodEspecif"></param>
    ''' <param name="p_EspecXEstilo"></param>
    ''' <remarks></remarks>
    Private Sub CargarComponentesPorDefecto(ByVal p_strFormID As String, ByVal p_strCodEstilo As String, ByVal accesorioXEstilo As Boolean, ByVal p_strCodEspecif As String, ByVal p_EspecXEstilo As Boolean)
        Dim strMensajeCompBasicos As String
        Try
            If accesorioXEstilo Then
                strMensajeCompBasicos = My.Resources.Resource.PreguntaComponentesBasicosE
            Else
                strMensajeCompBasicos = My.Resources.Resource.PreguntaComponentesBasicosM
            End If

            If CInt(Utilitarios.EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strComponentesPorDefecto"), p_strCodEstilo)) > 0) Then
                If m_SBO_Application.MessageBox(strMensajeCompBasicos, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                    CargarComponentesPorEstilo(p_strFormID, p_strCodEstilo)
                    CargarEspeficacionesPorEstilo(p_strFormID, p_strCodEspecif, p_EspecXEstilo)
                    Exit Sub
                End If
            Else
                LimpiarAccesoriosXModelo(p_strFormID)
                If CInt(Utilitarios.EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCountExpeXMode"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), IIf(p_EspecXEstilo, "U_Cod_Estilo", "U_Cod_Modelo"), IIf(p_EspecXEstilo, p_strCodEspecif, p_strCodEstilo))))) > 0 Then
                    If m_SBO_Application.MessageBox(strMensajeCompBasicos, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                        CargarEspeficacionesPorEstilo(p_strFormID, p_strCodEspecif, p_EspecXEstilo)
                        Exit Sub
                    End If
                End If
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Matriz"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function BorrarCompDupl(ByVal Matriz As Matrix) As Matrix

        Dim intFilaActual As Integer
        Dim intFilaActual2 As Integer
        Dim contador As Integer = 0
        Dim intMatrizRowCount As Integer = Matriz.RowCount
        For intFilaActual = 1 To intMatrizRowCount
            If intFilaActual <= intMatrizRowCount Then
                Dim strCodArt As String = Matriz.GetCellSpecific("col_0", intFilaActual).value.ToString
                For intFilaActual2 = 1 To intMatrizRowCount
                    If strCodArt.Trim() = Matriz.GetCellSpecific("col_0", intFilaActual2).value.ToString.Trim() Then
                        contador += 1
                        If contador > 1 Then
                            Matriz.DeleteRow(intFilaActual2)
                        End If
                    End If
                Next
                Matriz.FlushToDataSource()
                intMatrizRowCount = Matriz.RowCount
                contador = 0
            End If
        Next
        Return Matriz
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ps_FormID"></param>
    ''' <remarks></remarks>
    Private Sub LimpiarAccesoriosXModelo(ByVal ps_FormID As String)
        Dim lo_Form As Form
        Dim oMatrix As Matrix
        lo_Form = m_SBO_Application.Forms.Item(ps_FormID)
        oMatrix = DirectCast(lo_Form.Items.Item("mtx_0").Specific, Matrix)
        For i As Integer = lo_Form.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").Size - 1 To 0 Step -1
            lo_Form.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").RemoveRecord(i)
        Next
        oMatrix.LoadFromDataSource()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ps_FormID"></param>
    ''' <param name="ps_Codigo"></param>
    ''' <param name="pl_EspecifXEstilo"></param>
    ''' <remarks></remarks>
    Private Sub CargarEspeficacionesPorEstilo(ByVal ps_FormID As String, ByVal ps_Codigo As String, ByVal pl_EspecifXEstilo As Boolean)

        Dim lo_Form As Form
        Dim ls_Peso As String
        Dim ls_Cilind As String
        Dim ls_Potencia As String
        Dim ls_Categoria As String
        Dim ls_MarcaMot As String
        Dim ls_Transimision As String
        Dim ls_Carroceria As String
        Dim ls_Traccion As String
        Dim ls_TipoCabina As String
        Dim ls_Combustible As String
        Dim ls_TipoTecho As String
        Dim ls_CodTec As String
        Dim ls_MarcaComer As String
        Dim ls_ArtVenta As String
        Dim ln_GarantiaTM As Integer
        Dim ln_GarantiaKM As Integer
        Dim ln_NumCilindros As Integer
        Dim ln_Puertas As Integer
        Dim ln_Ejes As Integer
        Dim ln_Pasajeros As Integer

        Try
            lo_Form = m_SBO_Application.Forms.Item(ps_FormID)
            For Each drRow As DataRow In Utilitarios.EjecutarConsultaDataTable(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strExpeXModelo"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), IIf(pl_EspecifXEstilo, "U_Cod_Estilo", "U_Cod_Modelo"), ps_Codigo))).Rows
                With drRow
                    ln_NumCilindros = IIf(.Item(5) Is DBNull.Value, Nothing, .Item(5).ToString().Trim())
                    ln_Puertas = IIf(.Item(6) Is DBNull.Value, Nothing, .Item(6))
                    ln_Pasajeros = IIf(.Item(7) Is DBNull.Value, Nothing, .Item(7))
                    ln_Ejes = IIf(.Item(8) Is DBNull.Value, Nothing, .Item(8))
                    ls_Peso = IIf(.Item(9) Is DBNull.Value, String.Empty, .Item(9))
                    ls_Cilind = IIf(.Item(10) Is DBNull.Value, String.Empty, .Item(10))
                    ls_Potencia = IIf(.Item(11) Is DBNull.Value, String.Empty, .Item(11))
                    ln_GarantiaKM = IIf(.Item(19) Is DBNull.Value, Nothing, .Item(19))
                    ln_GarantiaTM = IIf(.Item(20) Is DBNull.Value, Nothing, .Item(20))
                    ls_Categoria = IIf(.Item(12) Is DBNull.Value, String.Empty, .Item(12))
                    ls_MarcaMot = IIf(.Item(13) Is DBNull.Value, String.Empty, .Item(13))
                    ls_Transimision = IIf(.Item(14) Is DBNull.Value, String.Empty, .Item(14))
                    ls_Carroceria = IIf(.Item(15) Is DBNull.Value, String.Empty, .Item(15))
                    ls_Traccion = IIf(.Item(16) Is DBNull.Value, String.Empty, .Item(16))
                    ls_TipoCabina = IIf(.Item(17) Is DBNull.Value, String.Empty, .Item(17))
                    ls_Combustible = IIf(.Item(18) Is DBNull.Value, String.Empty, .Item(18))
                    ls_TipoTecho = IIf(.Item(21) Is DBNull.Value, String.Empty, .Item(21))
                    ls_CodTec = IIf(.Item(22) Is DBNull.Value, String.Empty, .Item(22))
                    ls_MarcaComer = IIf(.Item(25) Is DBNull.Value, String.Empty, .Item(25))
                    ls_ArtVenta = IIf(.Item(24) Is DBNull.Value, String.Empty, .Item(24))
                End With
                With lo_Form.DataSources.DBDataSources
                    .Item("@SCGD_VEHICULO").SetValue("U_Num_Cili", 0, ln_NumCilindros)
                    .Item("@SCGD_VEHICULO").SetValue("U_CantPuer", 0, ln_Puertas)
                    .Item("@SCGD_VEHICULO").SetValue("U_Cant_Pas", 0, ln_Pasajeros)
                    .Item("@SCGD_VEHICULO").SetValue("U_Cant_Eje", 0, ln_Ejes)
                    .Item("@SCGD_VEHICULO").SetValue("U_Peso", 0, ls_Peso)
                    .Item("@SCGD_VEHICULO").SetValue("U_Cilindra", 0, ls_Cilind)
                    .Item("@SCGD_VEHICULO").SetValue("U_Potencia", 0, ls_Potencia)
                    .Item("@SCGD_VEHICULO").SetValue("U_GarantKM", 0, ln_GarantiaKM)
                    .Item("@SCGD_VEHICULO").SetValue("U_GarantTM", 0, ln_GarantiaTM)
                    .Item("@SCGD_VEHICULO").SetValue("U_TipTecho", 0, ls_TipoTecho)
                    .Item("@SCGD_VEHICULO").SetValue("U_Cod_Tec", 0, ls_CodTec)
                    .Item("@SCGD_VEHICULO").SetValue("U_Categori", 0, ls_Categoria)
                    .Item("@SCGD_VEHICULO").SetValue("U_MarcaMot", 0, ls_MarcaMot)
                    .Item("@SCGD_VEHICULO").SetValue("U_Transmis", 0, ls_Transimision)
                    .Item("@SCGD_VEHICULO").SetValue("U_Carrocer", 0, ls_Carroceria)
                    .Item("@SCGD_VEHICULO").SetValue("U_Tipo_Tra", 0, ls_Traccion)
                    .Item("@SCGD_VEHICULO").SetValue("U_Tip_Cabi", 0, ls_TipoCabina)
                    .Item("@SCGD_VEHICULO").SetValue("U_Combusti", 0, ls_Combustible)
                    .Item("@SCGD_VEHICULO").SetValue("U_ArtVentDesc", 0, ls_MarcaComer)
                    .Item("@SCGD_VEHICULO").SetValue("U_ArtVent", 0, ls_ArtVenta)
                End With
            Next
            Call CargarDescripcionCombo(oForm)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strFormID"></param>
    ''' <param name="p_strCodEstilo"></param>
    ''' <remarks></remarks>
    Private Sub CargarComponentesPorEstilo(ByVal p_strFormID As String, ByVal p_strCodEstilo As String)
        Dim oform As Form
        Dim oMatriz As Matrix
        Dim intFilaActual As Integer
        oform = m_SBO_Application.Forms.Item(p_strFormID)
        oMatriz = DirectCast(oform.Items.Item("mtx_0").Specific, Matrix)
        oMatriz.FlushToDataSource()
        For intFilaActual = oMatriz.RowCount To 1 Step -1
            If oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").GetValue("U_Tipo", intFilaActual - 1).Trim() = "N" Then
                oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").RemoveRecord(intFilaActual - 1)
            End If
            oMatriz.LoadFromDataSource()
        Next
        For Each dRow As DataRow In Utilitarios.EjecutarConsultaDataTable(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strComponentesPorDefectoDatos"), p_strCodEstilo)).Rows
            If dRow.Item(0) IsNot DBNull.Value AndAlso dRow.Item(1) IsNot DBNull.Value Then
                Call AsignarComponente(dRow.Item(0).ToString().Trim(), dRow.Item(2).ToString().Trim(), p_strFormID)
            End If
        Next
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oDataTable"></param>
    ''' <param name="p_strFormID"></param>
    ''' <remarks></remarks>
    Private Overloads Sub AsignarComponente(ByRef oDataTable As DataTable, ByVal p_strFormID As String)
        Dim intCantidad As Integer
        Dim oform As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intNuevoRegisto As Integer
        Dim blnLineasAgregadas As Boolean = False
        Dim strItemCode As String
        Dim intCantidadMinima As Integer = 1
        Dim decPrecio As Decimal = 0
        Dim decTotalLinea As Decimal = 0
        Dim decMontoTotal As Decimal = 0
        Dim strMontoTotal As String = String.Empty
        Dim strMonedaVehiculo As String = String.Empty
        Dim strMonedaListaPrecios As String = String.Empty

        Try
            oform = m_SBO_Application.Forms.Item(p_strFormID)
            oMatriz = DirectCast(oform.Items.Item("mtx_0").Specific, Matrix)

            'Obtenemos la moneda del vehículo, posteriormente se utiliza para convertir el precio del componente o accesorio a la moneda
            'del vehículo
            strMonedaVehiculo = oform.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_Moneda", 0).Trim()

            For intCantidad = 0 To oDataTable.Rows.Count - 1

                intCantidadMinima = 1
                decPrecio = 0
                decTotalLinea = 0
                strMonedaListaPrecios = String.Empty

                intNuevoRegisto = oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").Size
                If intNuevoRegisto = 1 Then
                    strItemCode = oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").GetValue("U_Acc", 0)
                    strItemCode = strItemCode.Trim()
                    If Not String.IsNullOrEmpty(strItemCode) Then
                        oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").InsertRecord(intNuevoRegisto)
                        intNuevoRegisto += 1
                    Else
                        intNuevoRegisto = 1
                    End If
                Else
                    oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").InsertRecord(intNuevoRegisto)
                    intNuevoRegisto += 1
                End If

                oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").SetValue("U_Acc", intNuevoRegisto - 1, oDataTable.GetValue("ItemCode", intCantidad))
                oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").SetValue("U_N_Acc", intNuevoRegisto - 1, oDataTable.GetValue("ItemName", intCantidad))
                oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").SetValue("U_Tipo", intNuevoRegisto - 1, "Y")
                oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").SetValue("U_Cantidad", intNuevoRegisto - 1, intCantidadMinima)

                'Obtiene el precio del artículo convertido a la moneda del vehículo
                decPrecio = ObtenerPrecioComponente(strMonedaVehiculo, oDataTable.GetValue("ItemCode", intCantidad))

                oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").SetValue("U_Precio", intNuevoRegisto - 1, decPrecio.ToString(n))
                decTotalLinea = intCantidadMinima * decPrecio
                oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").SetValue("U_Total", intNuevoRegisto - 1, decTotalLinea.ToString(n))
                blnLineasAgregadas = True


            Next intCantidad

            'Codigo para borrar componentes duplicados
            oMatriz.LoadFromDataSource()
            oMatriz = BorrarCompDupl(oMatriz)
            oMatriz.LoadFromDataSource()
            'Codigo para borrar componentes duplicados

            If blnLineasAgregadas Then
                oMatriz.LoadFromDataSource()

                'Calcula la sumatoria total de las lineas
                CalculaTotalAccesorios(oform)

                If oform.Mode <> BoFormMode.fm_ADD_MODE Then
                    oform.Mode = BoFormMode.fm_UPDATE_MODE
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Private Sub CalculaTotalLineasAcc(ByVal p_oForm As Form)
        Dim m_intTamano As Integer = 0
        Dim intCantidad As Integer = 1
        Dim strCantidad As String = String.Empty
        Dim m_decMontoTotal As Decimal = 0
        Dim m_decPrecioLista As Decimal = 0
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strMontoTotal As String = String.Empty
        Dim strPrecioLista As String = String.Empty

        Try
            oMatrix = DirectCast(p_oForm.Items.Item(g_str_mtxComponentes).Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()
            m_intTamano = p_oForm.DataSources.DBDataSources.Item(g_str_ACCXVEH).Size
            m_decMontoTotal = 0
            'Recorre las líneas de la tabla de accesorios y calcula el total por línea para cada una
            For i As Integer = 0 To m_intTamano - 1
                strCantidad = p_oForm.DataSources.DBDataSources.Item(g_str_ACCXVEH).GetValue("U_Cantidad", i).ToString(n)
                intCantidad = Integer.Parse(strCantidad, n)
                strPrecioLista = p_oForm.DataSources.DBDataSources.Item(g_str_ACCXVEH).GetValue("U_Precio", i).ToString(n)
                m_decPrecioLista = Decimal.Parse(strPrecioLista, n)
                m_decMontoTotal = intCantidad * m_decPrecioLista
                oForm.DataSources.DBDataSources.Item(g_str_ACCXVEH).SetValue("U_Total", i, m_decMontoTotal.ToString(n))
            Next

            oMatrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try


    End Sub


    ''' <summary>
    ''' Calcula la sumatoria total de los accesorios
    ''' </summary>
    ''' <param name="p_oForm">Formulario de SAP</param>
    ''' <returns>Número decimal con la sumatoria de las líneas de los accesorios/componentes</returns>
    ''' <remarks></remarks>
    Private Function CalculaTotalAccesorios(ByVal p_oForm As Form) As Decimal
        Dim m_intTamano As Integer = 0
        Dim m_decMontoTotal As Decimal = 0
        Dim m_decMonto As Decimal = 0
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strMontoTotal As String = String.Empty
        Dim strMonto As String = String.Empty

        Try
            oMatrix = DirectCast(p_oForm.Items.Item(g_str_mtxComponentes).Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()

            m_intTamano = p_oForm.DataSources.DBDataSources.Item(g_str_ACCXVEH).Size
            m_decMontoTotal = 0
            'Recorre las líneas de la tabla de accesorios y las suma en un total
            For i As Integer = 0 To m_intTamano - 1
                strMonto = p_oForm.DataSources.DBDataSources.Item(g_str_ACCXVEH).GetValue(g_strUTotal, i).ToString(n)
                m_decMonto = Decimal.Parse(strMonto, n)
                m_decMontoTotal = m_decMontoTotal + m_decMonto
            Next

            strMontoTotal = Utilitarios.ObtenerFormatoSAP(m_decMontoTotal, g_strSeparadorMillares, g_strSeparadorDecimales)

            'Asigna el total al campo U_TotalAcc
            p_oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue(g_strUTotalAcc, 0, strMontoTotal)

            Return m_decMontoTotal

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Function

    ''' <summary>
    ''' Devuelve el precio del componente/accesorio en la moneda seleccionada en el vehículo
    ''' </summary>
    ''' <param name="p_strMonedaVehiculo">Moneda del vehículo campo U_Moneda de la tabla [@SCGD_VEHICULO]</param>
    ''' <param name="p_strItemCode">Código del componente/accesorio seleccionado de la tabla OITM</param>
    ''' <returns>Precio del componente/accesorio convertido a la moneda del vehículo</returns>
    ''' <remarks></remarks>
    Private Function ObtenerPrecioComponente(ByVal p_strMonedaVehiculo As String, ByVal p_strItemCode As String) As Decimal
        Dim decPrecioComponente As Decimal = 0
        Dim decPrecioConvertido As Decimal = 0
        Dim strListaPrecio As String = String.Empty
        Dim strMonedaListaPrecios As String = String.Empty
        Dim strUsaControlAccesorios As String = String.Empty

        Try

            'Trae la lista de precios definida en las Parametrizaciones Generales utilizada para el precio de los accesorios cuando no hay clientes seleccionados
            strListaPrecio = DMS_Connector.Configuracion.ParamGenAddon.U_CodLisPre
            strUsaControlAccesorios = DMS_Connector.Configuracion.ParamGenAddon.U_CtrlAcc

            If Not String.IsNullOrEmpty(strListaPrecio) Then

                'Moneda de la lista de precios
                strMonedaListaPrecios = Utilitarios.EjecutarConsulta("Select Currency from ITM1 with (nolock) where ItemCode = '" & p_strItemCode & "' and PriceList = '" & strListaPrecio & "'", m_oCompany.CompanyDB, m_oCompany.Server).Trim
                'Precio del componente/accesorio
                decPrecioComponente = Utilitarios.EjecutarConsultaPrecios("Select Price from ITM1  with (nolock) where ItemCode = '" & p_strItemCode & "' and PriceList = '" & strListaPrecio & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                'Si la moneda del vehículo es diferente a la moneda de la lista de precios
                'se procede a realizar la conversión hacia la moneda del vehículo
                If Not String.IsNullOrEmpty(p_strMonedaVehiculo) Then
                    If Not String.IsNullOrEmpty(strMonedaListaPrecios) Then
                        If p_strMonedaVehiculo = strMonedaListaPrecios Then
                            'Devuelve el precio sin realizar ninguna conversión
                            decPrecioConvertido = decPrecioComponente
                        Else
                            'Realizar la conversión de acuerdo al tipo de cambio del día
                            decPrecioConvertido = ConvertirPrecio(p_strMonedaVehiculo, strMonedaListaPrecios, decPrecioComponente)
                        End If
                    End If
                Else
                    'La moneda no existe o no se ha seleccionado para este vehículo, por lo que se devuelve cero
                    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorMonedaVehiculo, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    decPrecioConvertido = 0
                End If

            Else
                'No se ha definido la lista de precios para accesorios campo U_CodLisPre
                If strUsaControlAccesorios.ToUpper() = "Y" Then
                    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorListaPreciosAcc, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                End If
                decPrecioConvertido = 0
            End If

            Return decPrecioConvertido

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Function

    ''' <summary>
    ''' Convierte el precio del componente/accesorio desde la lista de precios seleccionada hacia la moneda del vehículo
    ''' </summary>
    ''' <param name="strMonedaVehiculo">Moneda del vehículo</param>
    ''' <param name="strMonedaListaPrecios">Moneda de la lista de precios del componente/accesorio</param>
    ''' <param name="decPrecio">Precio del componente/accesorio</param>
    ''' <returns>Precio convertido a la moneda del vehículo de acuerdo al tipo de cambio del hoy</returns>
    ''' <remarks></remarks>
    Private Function ConvertirPrecio(ByVal strMonedaVehiculo As String, ByVal strMonedaListaPrecios As String, ByVal decPrecio As Decimal) As Decimal
        Try
            Dim decPrecioConvertido As Decimal = 0
            Dim strMonedaLocal As String = String.Empty
            Dim strMonedaSistema As String = String.Empty
            Dim decTipoCambioVehiculo As Decimal = 0
            Dim decTipoCambioSistema As Decimal = 0
            Dim boolTipoCambioValido = True

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Obtiene la moneda local, la moneda de sistema y su tipo de cambio del día
            DMS_Connector.Helpers.GetCurrencies(strMonedaLocal, strMonedaSistema)
            decTipoCambioSistema = DMS_Connector.Helpers.GetCurrencyRate(strMonedaSistema, Date.Now)

            If decTipoCambioSistema = -1 Or decTipoCambioSistema = 0 Then
                boolTipoCambioValido = False
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorTipoCambio & strMonedaSistema, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Obtiene el tipo de cambio de la moneda utilizada en el vehículo
            If strMonedaLocal = strMonedaVehiculo Then
                decTipoCambioVehiculo = 1
            Else
                decTipoCambioVehiculo = DMS_Connector.Helpers.GetCurrencyRate(strMonedaVehiculo, Date.Now)

                If decTipoCambioVehiculo = -1 Or decTipoCambioVehiculo = 0 Then
                    boolTipoCambioValido = False
                    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorTipoCambio & strMonedaVehiculo, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                End If

            End If

            If boolTipoCambioValido Then
                Select Case strMonedaVehiculo
                    Case strMonedaLocal
                        If strMonedaListaPrecios = strMonedaLocal Then
                            decPrecioConvertido = decPrecio
                        ElseIf strMonedaListaPrecios = strMonedaSistema Then
                            decPrecioConvertido = decPrecio * decTipoCambioSistema
                        Else
                            decPrecioConvertido = decPrecio * decTipoCambioVehiculo
                        End If
                    Case strMonedaSistema
                        If strMonedaListaPrecios = strMonedaLocal Then
                            decPrecioConvertido = decPrecio / decTipoCambioVehiculo
                        ElseIf strMonedaListaPrecios = strMonedaSistema Then
                            decPrecioConvertido = decPrecio
                        Else
                            decPrecioConvertido = (decPrecio / decTipoCambioSistema) * decTipoCambioVehiculo
                        End If
                    Case Else
                        If strMonedaListaPrecios = strMonedaLocal Then
                            decPrecioConvertido = decPrecio / decTipoCambioVehiculo
                        ElseIf strMonedaListaPrecios = strMonedaSistema Then
                            decPrecioConvertido = (decPrecio * decTipoCambioSistema) / decTipoCambioVehiculo
                        Else
                            decPrecioConvertido = decPrecio
                        End If
                End Select

            Else
                decPrecioConvertido = 0
            End If

            Return decPrecioConvertido

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strCodigoComponente"></param>
    ''' <param name="p_strNombreComponente"></param>
    ''' <param name="p_strFormID"></param>
    ''' <remarks></remarks>
    Private Overloads Sub AsignarComponente(ByVal p_strCodigoComponente As String, ByVal p_strNombreComponente As String, ByVal p_strFormID As String)

        Dim oform As Form
        Dim oMatriz As Matrix
        Dim intNuevoRegisto As Integer
        Dim blnLineasAgregadas As Boolean
        Dim strItemCode As String
        Dim intCantidad As Integer
        Dim intCantidadMinima As Integer = 1
        Dim decPrecio As Decimal = 0
        Dim decTotalLinea As Decimal = 0
        Dim decMontoTotal As Decimal = 0
        Dim strMontoTotal As String = String.Empty
        Dim strMonedaVehiculo As String = String.Empty
        Dim strMonedaListaPrecios As String = String.Empty

        Try
            oform = m_SBO_Application.Forms.Item(p_strFormID)
            oMatriz = DirectCast(oform.Items.Item("mtx_0").Specific, Matrix)

            intCantidadMinima = 1
            decPrecio = 0
            decTotalLinea = 0
            strMonedaListaPrecios = String.Empty

            'Obtenemos la moneda del vehículo, posteriormente se utiliza para convertir el precio del componente o accesorio a la moneda
            'del vehículo
            strMonedaVehiculo = oform.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_Moneda", 0).Trim()

            intNuevoRegisto = oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").Size
            If intNuevoRegisto = 1 Then
                strItemCode = oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").GetValue("U_Acc", 0)
                strItemCode = strItemCode.Trim()
                If Not String.IsNullOrEmpty(strItemCode) Then

                    oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").InsertRecord(intNuevoRegisto)

                    intNuevoRegisto += 1
                Else
                    intNuevoRegisto = 1
                End If
            Else
                oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").InsertRecord(intNuevoRegisto)
                intNuevoRegisto += 1
            End If

            oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").SetValue("U_Acc", intNuevoRegisto - 1, p_strCodigoComponente)
            oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").SetValue("U_N_Acc", intNuevoRegisto - 1, p_strNombreComponente)
            oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").SetValue("U_Tipo", intNuevoRegisto - 1, "N")
            oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").SetValue("U_Cantidad", intNuevoRegisto - 1, intCantidadMinima)

            If Not String.IsNullOrEmpty(p_strCodigoComponente) Then
                'Obtiene el precio del artículo convertido a la moneda del vehículo
                decPrecio = ObtenerPrecioComponente(strMonedaVehiculo, p_strCodigoComponente)
            End If

            oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").SetValue("U_Precio", intNuevoRegisto - 1, decPrecio.ToString(n))
            decTotalLinea = intCantidadMinima * decPrecio
            oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").SetValue("U_Total", intNuevoRegisto - 1, decTotalLinea.ToString(n))
            blnLineasAgregadas = True


            'Codigo para borrar componentes duplicados
            oMatriz.LoadFromDataSource()
            oMatriz = BorrarCompDupl(oMatriz)
            oMatriz.LoadFromDataSource()
            'Codigo para borrar componentes duplicados

            If blnLineasAgregadas Then
                oMatriz.LoadFromDataSource()
                'Calcula la sumatoria de los accesorios
                CalculaTotalAccesorios(oform)

                If oform.Mode <> SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                    oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strFormID"></param>
    ''' <remarks></remarks>
    Public Sub EliminarComponente(ByVal p_strFormID As String)
        Dim oform As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intRegistoEliminar As Integer
        Dim blnLineasEliminadas As Boolean = False
        Dim boolPermisosAccesorios As Boolean = True
        Dim strUsaControlAccesorios As String = String.Empty

        'Verifica si se utiliza el manejo de accesorios por permisos
        strUsaControlAccesorios = DMS_Connector.Configuracion.ParamGenAddon.U_CtrlAcc
        If strUsaControlAccesorios.ToUpper() = "Y" Then
            'True = tiene permisos para eliminar la línea, False = no tiene permisos para eliminar accesorios
            boolPermisosAccesorios = DMS_Connector.Helpers.PermisosMenu("SCGD_AAV")
        End If

        If boolPermisosAccesorios = True Then
            oform = m_SBO_Application.Forms.Item(p_strFormID)
            oform.Freeze(True)
            oMatriz = DirectCast(oform.Items.Item("mtx_0").Specific, SAPbouiCOM.Matrix)

            intRegistoEliminar = oMatriz.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder)
            Do While intRegistoEliminar > -1
                oMatriz.FlushToDataSource()
                m_strCodigoVehiculo = oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").GetValue("Code", intRegistoEliminar - 1)
                oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").RemoveRecord(intRegistoEliminar - 1)
                blnLineasEliminadas = True
                intRegistoEliminar = oMatriz.GetNextSelectedRow(intRegistoEliminar, SAPbouiCOM.BoOrderType.ot_RowOrder)
            Loop

            If oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").Size = 0 Then
                oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").InsertRecord(0)
                oform.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").SetValue("U_Tipo", 0, "N")
                blnLineasEliminadas = True
            End If

            If blnLineasEliminadas Then
                oMatriz.LoadFromDataSource()
                CalculaTotalAccesorios(oform)
                If oform.Mode <> SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                    oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                End If
            End If
            oform.Freeze(False)
        Else
            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorPermisosAccesorios, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oTmpForm"></param>
    ''' <param name="pval"></param>
    ''' <param name="FormUID"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub ManejoEventosCombo(ByRef oTmpForm As Form, _
                                  ByVal pval As ItemEvent, _
                                  ByVal FormUID As String, _
                                  ByRef BubbleEvent As Boolean)

        Dim strMonedaSistema As String
        Dim strMonedaLocal As String
        Dim strUnidad As String
        Dim strTipoActual As String
        Dim intDocEntryEntradas As Integer
        Dim blnUtilizaCosteoAccesorios As String

        Try

            If pval.ActionSuccess Then

                Dim cboCombo As ComboBox

                If pval.ItemUID = mc_strEstilo _
                    Or pval.ItemUID = mc_strModelo Then

                    Dim id As String = String.Empty
                    Dim idCombo As String = String.Empty
                    Dim accesorioXEstilo As Boolean = False
                    Dim EspecXEstilo As Boolean = False
                    Dim cboComboLocal As SAPbouiCOM.ComboBox
                    Dim cboComboEspec As SAPbouiCOM.ComboBox
                    Dim oItemLocal As SAPbouiCOM.Item
                    Dim oItemEspec As SAPbouiCOM.Item
                    Dim strEspecEstilo As String

                    strEspecEstilo = DMS_Connector.Configuracion.ParamGenAddon.U_EspVehic.Trim()

                    If strEspecEstilo = "E" Then
                        oItemLocal = oTmpForm.Items.Item(mc_strEstilo)
                        cboComboLocal = CType(oItemLocal.Specific, SAPbouiCOM.ComboBox)
                        accesorioXEstilo = True
                        oItemEspec = oTmpForm.Items.Item(mc_strEstilo)
                        cboComboEspec = CType(oItemEspec.Specific, SAPbouiCOM.ComboBox)
                        If cboComboEspec.Selected IsNot Nothing Then
                            idCombo = IIf(IsNothing(cboComboEspec.Selected.Value), "", CStr(cboComboEspec.Selected.Value))
                        End If
                        EspecXEstilo = True
                    ElseIf strEspecEstilo = "M" Then
                        oItemLocal = oTmpForm.Items.Item(mc_strModelo)
                        cboComboLocal = CType(oItemLocal.Specific, SAPbouiCOM.ComboBox)
                        accesorioXEstilo = False
                        oItemEspec = oTmpForm.Items.Item(mc_strModelo)
                        cboComboEspec = CType(oItemEspec.Specific, SAPbouiCOM.ComboBox)
                        If cboComboEspec.Selected IsNot Nothing Then
                            idCombo = IIf(IsNothing(cboComboEspec.Selected.Value), "", CStr(cboComboEspec.Selected.Value))
                        End If
                        EspecXEstilo = False
                    End If


                    'si se selecciono un combo
                    If cboComboLocal.Selected IsNot Nothing Then
                        id = CStr(cboComboLocal.Selected.Value)
                    End If

                    'Si se obtiene un id para cargar componentes 
                    If Not String.IsNullOrEmpty(id) Then
                        CargarComponentesPorDefecto(FormUID, id, accesorioXEstilo, idCombo, EspecXEstilo)
                    End If
                End If

                If pval.ItemUID = mc_strTipo Then

                    If oTmpForm.Mode = BoFormMode.fm_UPDATE_MODE Then

                        DMS_Connector.Helpers.GetCurrencies(strMonedaLocal, strMonedaSistema)
                        strUnidad = oTmpForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_Cod_Unid", 0).Trim
                        strTipoActual = oTmpForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_Tipo", 0).Trim
                        intDocEntryEntradas = Utilitarios.EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strConsultaGoodReceive"), strUnidad))
                        blnUtilizaCosteoAccesorios = DMS_Connector.Configuracion.ParamGenAddon.U_UsaAxC.Trim()
                        If intDocEntryEntradas > 0 Then
                            If m_SBO_Application.MessageBox(Text:=My.Resources.Resource.LaUnidad & strUnidad & " " & My.Resources.Resource.MensajeUnidadEntradasAsociadas, Btn1Caption:=My.Resources.Resource.Si) = 1 Then
                                oTmpForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_Tipo", 0, strTipoActual)
                                oTmpForm.Mode = BoFormMode.fm_OK_MODE
                                BubbleEvent = False
                            End If
                        ElseIf Utilitarios.ConsultaCosteos(strUnidad, m_oCompany.CompanyDB, m_oCompany.Server, strMonedaSistema, strMonedaLocal, blnUtilizaCosteoAccesorios) Then
                            If m_SBO_Application.MessageBox(Text:=My.Resources.Resource.LaUnidad & strUnidad & " " & My.Resources.Resource.UnidadCosteosPendientes, Btn1Caption:=My.Resources.Resource.Si) = 1 Then
                                oTmpForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_Tipo", 0, strTipoActual)
                                oTmpForm.Mode = BoFormMode.fm_OK_MODE
                                BubbleEvent = False
                            End If
                        End If
                    End If
                End If

                Select Case pval.ItemUID
                    Case mc_strMarca
                        Dim strMarca As String = String.Empty

                        cboCombo = DirectCast(oForm.Items.Item(mc_strMarca).Specific, ComboBox)
                        strMarca = cboCombo.Value.Trim

                        Call CargarComboEstilos(oForm, False, String.Empty)

                    Case mc_strEstilo
                        Call CargarComboModelo(oForm, False, String.Empty)

                    Case g_str_mtxBonos
                        If pval.ColUID = g_str_ColBono Then
                            AgregaLineaMatrizBonos(oForm)
                        End If

                End Select


            ElseIf pval.ActionSuccess = False AndAlso pval.BeforeAction = True Then

                Dim oCombo As SAPbouiCOM.ComboBox
                Dim oItem As SAPbouiCOM.Item
                Dim strValorSeleccionado As String = String.Empty

                Select Case pval.ItemUID

                    Case mc_strMarca
                        oCombo = DirectCast(oForm.Items.Item(mc_strMarca).Specific, SAPbouiCOM.ComboBox)
                        If oCombo.Selected IsNot Nothing Then
                            strValorSeleccionado = oCombo.Selected.Value
                        End If

                        If oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboMarca(oForm)
                        End If

                    Case mc_strEstilo
                        Dim strMarca As String = String.Empty

                        oCombo = DirectCast(oForm.Items.Item(mc_strMarca).Specific, SAPbouiCOM.ComboBox)

                        oForm.ActiveItem = mc_strEstilo

                        strMarca = oCombo.Value.Trim

                        If Not String.IsNullOrEmpty(strMarca) Then
                            oCombo = DirectCast(oForm.Items.Item(mc_strEstilo).Specific, SAPbouiCOM.ComboBox)
                            strValorSeleccionado = oCombo.Value.Trim

                            If String.IsNullOrEmpty(strValorSeleccionado) Then
                                Call CargarComboEstilos(oForm, False, String.Empty)
                            Else
                                Call CargarComboEstilos(oForm, True, strValorSeleccionado)
                            End If

                        End If


                    Case mc_strModelo

                        Dim strEstilo As String

                        oCombo = DirectCast(oForm.Items.Item(mc_strEstilo).Specific, SAPbouiCOM.ComboBox)

                        oForm.ActiveItem = mc_strModelo

                        strEstilo = oCombo.Value.Trim

                        If Not String.IsNullOrEmpty(strEstilo) Then
                            oCombo = DirectCast(oForm.Items.Item(mc_strModelo).Specific, SAPbouiCOM.ComboBox)
                            strValorSeleccionado = oCombo.Value.Trim

                            If String.IsNullOrEmpty(strValorSeleccionado) Then
                                Call CargarComboModelo(oForm, False, String.Empty)
                            Else
                                Call CargarComboModelo(oForm, True, strValorSeleccionado)
                            End If

                        End If

                    Case mc_strDisponibilidad
                        Dim strDisponibilidad As String

                        oItem = oForm.Items.Item(mc_strDisponibilidad)

                        oForm.ActiveItem = mc_strDisponibilidad

                        oCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)

                        strDisponibilidad = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strDisponibilidad) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboDiponibilidad(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboDiponibilidad(oForm, True, strDisponibilidad)
                        End If

                    Case mc_strUbicaciones
                        oCombo = DirectCast(oForm.Items.Item(mc_strUbicaciones).Specific, SAPbouiCOM.ComboBox)

                        oForm.ActiveItem = mc_strUbicaciones

                        strValorSeleccionado = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strValorSeleccionado) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboUbicacion(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboUbicacion(oForm, True, strValorSeleccionado)
                        End If

                    Case mc_strTipo
                        oCombo = DirectCast(oForm.Items.Item(mc_strTipo).Specific, SAPbouiCOM.ComboBox)
                        oForm.ActiveItem = mc_strTipo
                        strValorSeleccionado = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strValorSeleccionado) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboTipo(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboTipo(oForm, True, strValorSeleccionado)
                        End If

                    Case mc_strEstado
                        oCombo = DirectCast(oForm.Items.Item(mc_strEstado).Specific, SAPbouiCOM.ComboBox)
                        oForm.ActiveItem = mc_strEstado
                        strValorSeleccionado = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strValorSeleccionado) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboEstado(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboEstado(oForm, True, strValorSeleccionado)
                        End If

                    Case mc_strMoneda
                        oCombo = DirectCast(oForm.Items.Item(mc_strMoneda).Specific, SAPbouiCOM.ComboBox)
                        oForm.ActiveItem = mc_strMoneda
                        strValorSeleccionado = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strValorSeleccionado) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboMoneda(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboMoneda(oForm, True, strValorSeleccionado)
                        End If

                    Case mc_strCategoria
                        oCombo = DirectCast(oForm.Items.Item(mc_strCategoria).Specific, SAPbouiCOM.ComboBox)
                        oForm.ActiveItem = mc_strCategoria
                        strValorSeleccionado = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strValorSeleccionado) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboCategoria(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboCategoria(oForm, True, strValorSeleccionado)
                        End If
                    Case mc_strTipoContrato
                        oCombo = DirectCast(oForm.Items.Item(mc_strTipoContrato).Specific, SAPbouiCOM.ComboBox)
                        oForm.ActiveItem = mc_strTipoContrato
                        strValorSeleccionado = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strValorSeleccionado) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboTipoContrato(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboTipoContrato(oForm, True, strValorSeleccionado)
                        End If
                    Case mc_strMarcaMotor
                        oCombo = DirectCast(oForm.Items.Item(mc_strMarcaMotor).Specific, SAPbouiCOM.ComboBox)
                        oForm.ActiveItem = mc_strMarcaMotor
                        strValorSeleccionado = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strValorSeleccionado) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboMarcaMot(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboMarcaMot(oForm, True, strValorSeleccionado)
                        End If

                    Case mc_strTransmision
                        oCombo = DirectCast(oForm.Items.Item(mc_strTransmision).Specific, SAPbouiCOM.ComboBox)
                        oForm.ActiveItem = mc_strTransmision
                        strValorSeleccionado = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strValorSeleccionado) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboTransmision(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboTransmision(oForm, True, strValorSeleccionado)
                        End If

                    Case mc_strCarroceria
                        oCombo = DirectCast(oForm.Items.Item(mc_strCarroceria).Specific, SAPbouiCOM.ComboBox)
                        oForm.ActiveItem = mc_strCarroceria
                        strValorSeleccionado = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strValorSeleccionado) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboCarroceria(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboCarroceria(oForm, True, strValorSeleccionado)
                        End If

                    Case mc_strTraccion
                        oCombo = DirectCast(oForm.Items.Item(mc_strTraccion).Specific, SAPbouiCOM.ComboBox)
                        oForm.ActiveItem = mc_strTraccion
                        strValorSeleccionado = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strValorSeleccionado) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboTraccion(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboTraccion(oForm, True, strValorSeleccionado)
                        End If

                    Case mc_strCabina
                        oCombo = DirectCast(oForm.Items.Item(mc_strCabina).Specific, SAPbouiCOM.ComboBox)
                        oForm.ActiveItem = mc_strCabina
                        strValorSeleccionado = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strValorSeleccionado) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboCabina(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboCabina(oForm, True, strValorSeleccionado)
                        End If

                    Case mc_strCombustible
                        oCombo = DirectCast(oForm.Items.Item(mc_strCombustible).Specific, SAPbouiCOM.ComboBox)
                        oForm.ActiveItem = mc_strCombustible
                        strValorSeleccionado = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strValorSeleccionado) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboCombustible(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboCombustible(oForm, True, strValorSeleccionado)
                        End If

                    Case mc_strTecho
                        oCombo = DirectCast(oForm.Items.Item(mc_strTecho).Specific, SAPbouiCOM.ComboBox)
                        oForm.ActiveItem = mc_strTecho
                        strValorSeleccionado = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strValorSeleccionado) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboTecho(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboTecho(oForm, True, strValorSeleccionado)
                        End If

                    Case mc_strClasificacion
                        oCombo = DirectCast(oForm.Items.Item(mc_strClasificacion).Specific, SAPbouiCOM.ComboBox)
                        oForm.ActiveItem = mc_strClasificacion
                        strValorSeleccionado = oCombo.Value.Trim

                        If String.IsNullOrEmpty(strValorSeleccionado) AndAlso oCombo.ValidValues.Count = 0 Then
                            Call CargarComboClasificacion(oForm, False, String.Empty)
                        ElseIf oCombo.ValidValues.Count <= 1 Then
                            Call CargarComboClasificacion(oForm, True, strValorSeleccionado)
                        End If
                End Select

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oTmpForm"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub CargarEstilos(ByRef oTmpForm As Form)

        Dim strValorSeleccionado As String

        m_strCodigoVehiculo = ""

        strValorSeleccionado = oTmpForm.DataSources.DBDataSources.Item(mc_strVehiculo).GetValue("U_Cod_Marc", 0)
        strValorSeleccionado = strValorSeleccionado.Trim
        Call HabilitarCombos(oTmpForm, mc_strEstilo)
        Call CargarValidValuesEnCombos(oTmpForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbEstilo"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "U_Cod_Marc", strValorSeleccionado)), "cboEst", False, "U_Cod_Esti", True)
        oTmpForm.Mode = BoFormMode.fm_OK_MODE

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oTmpForm"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub CargarModelos(ByRef oTmpForm As Form)

        Dim strValorSeleccionado As String
        Dim cboCombo As ComboBox
        Dim oItem As Item

        oItem = oTmpForm.Items.Item(mc_strEstilo)
        cboCombo = CType(oItem.Specific, ComboBox)

        strValorSeleccionado = oForm.DataSources.DBDataSources.Item(mc_strVehiculo).GetValue("U_Cod_Esti", 0)
        strValorSeleccionado = strValorSeleccionado.Trim
        Call HabilitarCombos(oForm, mc_strModelo)
        Call CargarValidValuesEnCombos(oTmpForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbModelo"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "U_Cod_Esti", strValorSeleccionado)), "cboModelo", False, "U_Cod_Mode", True)
        oTmpForm.Refresh()
        oTmpForm.Mode = BoFormMode.fm_OK_MODE

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oTmpForm"></param>
    ''' <param name="pval"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub ManejoEventosTab(ByRef oTmpForm As SAPbouiCOM.Form, _
                                ByRef pval As SAPbouiCOM.ItemEvent)
        '*******************************************************************    
        'Nombre: ManejoEventosTab()
        'Propósito: Asigna el PanelLevel del Form, dependiendo de el Tab que se haya seleccionado 
        'Acepta:    ByRef oTmpForm As SAPbouiCOM.Form, 
        '           ByRef pval As SAPbouiCOM.ItemEvent
        'Retorna:   Ninguno
        'Desarrollador: Yeiner
        'Fecha: 29 Nov 2006
        '********************************************************************

        If pval.ItemUID = mc_strFolder1 Then

            oTmpForm.PaneLevel = 1
        ElseIf pval.ItemUID = mc_strFolder2 Then

            oTmpForm.PaneLevel = 2
        ElseIf pval.ItemUID = mc_strFolder3 Then
            oTmpForm.PaneLevel = 3
        ElseIf pval.ItemUID = mc_strFolder4 Then
            oTmpForm.PaneLevel = 4
        ElseIf pval.ItemUID = mc_strFolder5 Then
            oTmpForm.PaneLevel = 5
            If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_VerCostoS = "Y") Then
                If DMS_Connector.Configuracion.ParamGenAddon.U_VerCostoS = "Y" Then
                    oTmpForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    oTmpForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                Else
                    oTmpForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    oTmpForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                End If
            Else
                oTmpForm.Items.Item("lblCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                oTmpForm.Items.Item("txtCostoS").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            End If
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strItemID"></param>
    ''' <param name="strTipoControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Protected Friend Function ObtenerIDControles(ByVal strItemID As String, _
                                                 ByVal strTipoControl As TipoControl) As String
        '*******************************************************************    
        'Nombre: InsertarDescripcionCombos()
        'Propósito: Este procedimiento se encarga de obtener el value de los controles para poder actualizar
        '           Marca, Estilo y Modelo.
        'Acepta:    ByRef oForm As SAPbouiCOM.Form, 
        '           ByVal strItemID As String, 
        '           ByVal strTipoControl As TipoControl
        'Retorna:   String
        'Desarrollador: Yeiner
        'Fecha: 29 Nov 2006
        '********************************************************************
        Dim oItem As Item
        Dim oComboBox As ComboBox
        Dim oEditText As EditText
        Dim strReturnValue As String

        Try
            Select Case strTipoControl

                Case TipoControl.ComboBox
                    oItem = oForm.Items.Item(strItemID)
                    oComboBox = CType(oItem.Specific, ComboBox)

                    If Not oComboBox.Selected Is Nothing Then

                        strReturnValue = oComboBox.Selected.Description
                        Return strReturnValue

                    ElseIf oComboBox.ValidValues.Count = 0 Then
                        strReturnValue = Nothing
                        Return strReturnValue

                    ElseIf oComboBox.ValidValues.Count > 0 AndAlso oForm.Items.Item(strItemID).UniqueID <> "cboModelo" Then
                        strReturnValue = ""
                        Return strReturnValue

                    ElseIf oComboBox.ValidValues.Count > 0 Then
                        strReturnValue = Nothing
                        Return strReturnValue
                    End If


                Case TipoControl.EditText
                    oItem = oForm.Items.Item(strItemID)
                    oEditText = CType(oItem.Specific, EditText)

                    If Not oEditText Is Nothing Then
                        strReturnValue = oEditText.Value
                        Return strReturnValue
                    End If

            End Select

            Return Nothing

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="BubbleEvent"></param>
    ''' <param name="update"></param>
    ''' <param name="p_strValidaPlaca"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Private Sub ValidarUnidadExista(ByRef BubbleEvent As Boolean, ByVal update As Boolean, ByVal p_strValidaPlaca As String)

        Dim strQuery As String
        Dim strValorRetornado As String
        Dim strCodigoUnidad As String
        Dim strPlaca As String
        Dim strCode As String

        Try
            strCodigoUnidad = oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_Cod_Unid", 0).Trim
            strPlaca = oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_Num_Plac", 0).Trim
            strCode = oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("Code", 0).Trim

            'Validacion para cargar la pantalla con el Codigo de Unidad
            If Not String.IsNullOrEmpty(strCodigoUnidad) Then
                strQuery = String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCodeVehCodUnid"), strCodigoUnidad)
                If update Then
                    strQuery = String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCodeDi"), strQuery, strCode)
                End If
                strValorRetornado = Utilitarios.EjecutarConsulta(strQuery).Trim
                'Se valida que la unidad no exista.
                If Not String.IsNullOrEmpty(strValorRetornado) Then
                    BubbleEvent = False
                    'Desea Cargar la Unidad Encontrada ?
                    If m_SBO_Application.MessageBox(My.Resources.Resource.UnidadYaRegistrada, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                        CargarVehiculo(strValorRetornado)
                    End If
                End If
            End If

            If BubbleEvent Then
                'Validacion para cargar la pantalla con la Placa
                If Not String.IsNullOrEmpty(strPlaca) AndAlso p_strValidaPlaca = "Y" Then
                    strQuery = String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCodeVehPlaca"), strPlaca)
                    If update Then
                        strQuery = String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCodeDi"), strQuery, strCode)
                    End If
                    strValorRetornado = Utilitarios.EjecutarConsulta(strQuery).Trim
                    'Se valida que la unidad no exista.
                    If Not String.IsNullOrEmpty(strValorRetornado) Then
                        BubbleEvent = False
                        'Desea Cargar la Unidad Encontrada ?
                        If m_SBO_Application.MessageBox(My.Resources.Resource.PlacaYaRegistrada, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                            CargarVehiculo(strValorRetornado)
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_DocentryVehiculo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Protected Friend Function ValidarNumeroVIN(ByVal p_DocentryVehiculo As String) As Boolean

        Dim strQuery As String = String.Empty
        Dim strValorRetornado As String
        Dim strNumeroVIN As String
        Dim strCode As String

        Try

            strNumeroVIN = oForm.DataSources.DBDataSources.Item(mc_strVehiculo).GetValue("U_Num_VIN", 0).Trim()

            If Not String.IsNullOrEmpty(strNumeroVIN) Then

                strQuery = String.Format("Select Docentry From [@SCGD_VEHICULO] with(nolock) where U_Num_VIN = '{0}' and DocEntry <> '{1}'", strNumeroVIN, p_DocentryVehiculo)

                If Not String.IsNullOrEmpty(strQuery) Then

                    strValorRetornado = Utilitarios.EjecutarConsulta(strQuery, m_oCompany.CompanyDB, m_oCompany.Server).Trim

                    If Not String.IsNullOrEmpty(strValorRetornado) Then
                        Return True
                    Else
                        Return False
                    End If

                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strCode"></param>
    ''' <remarks></remarks>
    Public Sub CargarVehiculo(ByVal p_strCode As String)

        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition

        Dim oitem As SAPbouiCOM.Item
        Dim oedit As SAPbouiCOM.EditText

        Dim strIdVehiculo As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            oForm.Freeze(True)

            strIdVehiculo = p_strCode

            If Not String.IsNullOrEmpty(strIdVehiculo) Then
                oitem = oForm.Items.Item(mc_strNumVehiculo)
                oConditions = m_SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add
                oCondition.Alias = "Code"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = strIdVehiculo
                oedit = oitem.Specific
                oedit.String = strIdVehiculo
                oMatrix = DirectCast(oForm.Items.Item(g_str_mtxBonos).Specific, SAPbouiCOM.Matrix)
                Call oForm.EnableMenu("1281", True)
                Call oForm.EnableMenu("1282", True)
                Call oForm.EnableMenu("1291", True)
                Call oForm.EnableMenu("1288", True)
                Call oForm.EnableMenu("1289", True)
                Call oForm.EnableMenu("1290", True)
                Call oForm.EnableMenu("1293", True)
                Call oForm.DataSources.DBDataSources.Item(mc_strVehiculo).Query(oConditions)
                Call oForm.DataSources.DBDataSources.Item("@SCGD_ACCXVEH").Query(oConditions)
                Call oForm.DataSources.DBDataSources.Item("@SCGD_VEHITRAZA").Query(oConditions)
                Call oForm.DataSources.DBDataSources.Item("@SCGD_BONOXVEH").Query(oConditions)
                oitem = oForm.Items.Item(mc_strUnidad)
                oedit = oitem.Specific
                oedit.Active = False
                oMatrix.LoadFromDataSource()
                CargarComboMarca(oForm)
                CargarEstilos(oForm)
                CargarModelos(oForm)
                DeshabilitarNumeroUnidad(oForm)
                oForm.Mode = BoFormMode.fm_OK_MODE
                ManejarModoFormulario(oForm)
                oForm.Freeze(False)
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FormUID"></param>
    ''' <param name="pVal"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoUnLoad(ByVal FormUID As String, _
                                    ByRef pVal As SAPbouiCOM.ItemEvent, _
                                    ByRef BubbleEvent As Boolean)

        oForm = Nothing

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pval"></param>
    ''' <param name="formUID"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventosMenus(ByVal pval As SAPbouiCOM.MenuEvent, ByVal formUID As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Try
            AgregaLineaMatrizBonos(oForm)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="formUID"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoFormDataLoad(ByVal formUID As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Dim oItem As SAPbouiCOM.Item
        Try
            AgregaLineaMatrizBonos(oForm)
            CargarDescripcionCombo(formUID)
            oVehiculo = Nothing
            Call oForm.EnableMenu("1284", False)
            Call oForm.EnableMenu("1285", False)

            If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_VerCostoS = "Y") Then
                If DMS_Connector.Configuracion.ParamGenAddon.U_VerCostoS = "Y" Then
                    oItem = oForm.Items.Item("lblCostoS")
                    oItem.Visible = True
                    oItem = oForm.Items.Item("txtCostoS")
                    oItem.Visible = True
                Else
                    oItem = oForm.Items.Item("lblCostoS")
                    oItem.Visible = False
                    oItem = oForm.Items.Item("txtCostoS")
                    oItem.Visible = False
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FormUID"></param>
    ''' <param name="pVal"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub ImprimirFichaVehículo(ByVal FormUID As String, _
                                    ByRef pVal As SAPbouiCOM.ItemEvent, _
                                    ByRef BubbleEvent As Boolean)

        Dim strDireccionReporte As String = ""
        Dim strDBDMSOne As String = ""
        Dim strPathExe As String
        Dim strParametros As String

        strDBDMSOne = m_SBO_Application.Company.DatabaseName
        strParametros = m_oCompany.CompanyName & "," & oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("Docentry", 0).Trim
        strParametros = strParametros.Replace(" ", "°")

        strDireccionReporte = DMS_Connector.Configuracion.ParamGenAddon.U_Reportes & "\" & My.Resources.Resource.rptFichaVehículo & ".rpt"
        strDireccionReporte = strDireccionReporte.Replace(" ", "°")
        strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

        strPathExe &= My.Resources.Resource.FichaVehículo.Replace(" ", "°") & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & m_oCompany.Server & "," & m_oCompany.CompanyDB & " " & strParametros
        Shell(strPathExe, AppWinStyle.MaximizedFocus)

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="BubbleEvent"></param>
    ''' <param name="p_strModeoEsti"></param>
    ''' <remarks></remarks>
    Private Sub ValidaEstiloyModelo(ByRef BubbleEvent As Boolean, ByVal p_strModeoEsti As String)

        Dim strMarcaSelected As String = String.Empty
        Dim strEstiloSelected As String = String.Empty
        Dim strModeloSelected As String = String.Empty

        strMarcaSelected = ObtenerIDControles(mc_strMarca, VehiculosCls.TipoControl.ComboBox)
        strEstiloSelected = ObtenerIDControles(mc_strEstilo, VehiculosCls.TipoControl.ComboBox)
        strModeloSelected = ObtenerIDControles("cboModelo", VehiculosCls.TipoControl.ComboBox)

        'Valida que la unidad tenga Marca y Modelo
        If String.IsNullOrEmpty(strMarcaSelected) Or String.IsNullOrEmpty(strEstiloSelected) Then
            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.RequeridasMarcaYEstilo, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            BubbleEvent = False
            Exit Sub
        End If

        'Le pongo un nullo si no tiene Modelo y usa 2 Niveles en la creacion del vehiculo
        If p_strModeoEsti = "M" AndAlso String.IsNullOrEmpty(strModeloSelected) Then
            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.RequeridoModelo, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            BubbleEvent = False
            Exit Sub
        Else 'Asigno los valores una vez validado la Marca, Estilo y Modelo
            oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_Des_Marc", 0, strMarcaSelected)
            oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_Des_Esti", 0, strEstiloSelected)
            oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_Des_Mode", 0, strModeloSelected)
        End If

    End Sub

#End Region

#Region "Cargar Combos"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboMarca(ByRef oForm As Form)
        Try

            oItems = oForm.Items.Item(mc_strMarca)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbMarca"), String.Empty))
            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboEstilos(ByRef oForm As Form,
                                            ByVal p_blnSeleccionaValor As Boolean,
                                            ByVal p_strIDValSelect As String)
        Try
            Dim strCodMarca As String
            oItems = oForm.Items.Item(mc_strMarca)
            cboCombos = CType(oItems.Specific, ComboBox)
            strCodMarca = CStr(cboCombos.Value).Trim

            oItems = oForm.Items.Item(mc_strEstilo)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbEstilo"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "U_Cod_Marc", strCodMarca)))
            If p_blnSeleccionaValor Then
                oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Cod_Esti", 0, p_strIDValSelect)
            Else
                oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Cod_Esti", 0, strVal)
            End If
            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboModelo(ByRef oForm As Form,
                                        ByVal p_blnSeleccionaValor As Boolean,
                                        ByVal p_strIDValSelect As String)
        Try
            Dim strCodEstilo As String

            strVal = String.Empty

            oItems = oForm.Items.Item(mc_strEstilo)
            cboCombos = CType(oItems.Specific, ComboBox)
            strCodEstilo = CStr(cboCombos.Selected.Value).Trim

            oItems = oForm.Items.Item(mc_strModelo)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbModelo"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "U_Cod_Esti", strCodEstilo)))
            If p_blnSeleccionaValor Then
                oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Cod_Mode", 0, p_strIDValSelect)
            Else
                oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Cod_Mode", 0, strVal)
            End If
            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboDiponibilidad(ByRef oForm As Form,
                                                  ByVal p_blnSeleccionaValor As Boolean,
                                                ByVal p_strIDValSelect As String)
        Try
            strVal = String.Empty

            oItems = oForm.Items.Item(mc_strDisponibilidad)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbDisponibilidad"), String.Empty))
            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboUbicacion(ByRef oForm As Form,
                                                ByVal p_blnSeleccionaValor As Boolean,
                                                ByVal p_strIDValSelect As String)
        Try

            strVal = String.Empty
            oItems = oForm.Items.Item(mc_strUbicaciones)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbUbicaciones"), String.Empty))
            If p_blnSeleccionaValor Then
                strVal = p_strIDValSelect
            End If
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Cod_Ubic", 0, p_strIDValSelect)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oForm"></param>
    ''' <remarks></remarks>
    Public Sub CargarComboTiposBono(ByRef p_oForm As Form)
        Dim oMatrix As Matrix
        Try
            oMatrix = DirectCast(p_oForm.Items.Item(g_str_mtxBonos).Specific, Matrix)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(g_str_ColBono).ValidValues,
                                                       DMS_Connector.Queries.GetStrSpecificQuery("strCbBonos"))

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oForm"></param>
    ''' <remarks></remarks>
    Public Sub AgregaLineaMatrizBonos(ByRef p_oForm As Form)

        Dim m_intPosicion As Integer = 0
        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            oMatrix = DirectCast(p_oForm.Items.Item(g_str_mtxBonos).Specific, Matrix)
            oMatrix.FlushToDataSource()

            m_intPosicion = p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).Size

            If m_intPosicion = 1 Then
                If String.IsNullOrEmpty(p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).GetValue(g_strUBono, 0).Trim()) Then
                    m_intPosicion = 0
                    p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).SetValue(g_strUBono, m_intPosicion, String.Empty)
                    p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).SetValue(g_strUMonto, m_intPosicion, 0)
                Else
                    m_intPosicion = 1
                    p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).InsertRecord(m_intPosicion)
                    p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).SetValue(g_strUBono, m_intPosicion, String.Empty)
                    p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).SetValue(g_strUMonto, m_intPosicion, 0)
                End If
            Else
                If Not String.IsNullOrEmpty(p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).GetValue(g_strUBono, m_intPosicion - 1).Trim()) Then
                    p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).InsertRecord(m_intPosicion)
                    p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).SetValue(g_strUBono, m_intPosicion, String.Empty)
                    p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).SetValue(g_strUMonto, m_intPosicion, 0)
                End If
            End If

            oMatrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oForm"></param>
    ''' <remarks></remarks>
    Private Sub EliminaLineasBlancoBonos(ByRef p_oForm As Form)
        Dim oMatrix As Matrix
        Try
            oMatrix = DirectCast(p_oForm.Items.Item(g_str_mtxBonos).Specific, Matrix)
            oMatrix.FlushToDataSource()

            For i As Integer = 0 To p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).Size - 1
                If String.IsNullOrEmpty(p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).GetValue(g_strUBono, i).Trim()) Then p_oForm.DataSources.DBDataSources.Item(g_str_BONOXVEH).RemoveRecord(i)
            Next
            oMatrix.LoadFromDataSource()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strFormID"></param>
    ''' <remarks></remarks>
    Private Sub EliminarLíneasMatrizBonos(ByVal p_strFormID As String)

        Dim oform As Form
        Dim oMatriz As Matrix
        Dim intRegistoEliminar As Integer
        Dim blnLineasEliminadas As Boolean = False

        oform = m_SBO_Application.Forms.Item(p_strFormID)
        oMatriz = DirectCast(oform.Items.Item(g_str_mtxBonos).Specific, Matrix)
        intRegistoEliminar = oMatriz.GetNextSelectedRow()
        Do While intRegistoEliminar > -1

            oform.DataSources.DBDataSources.Item(g_str_BONOXVEH).RemoveRecord(intRegistoEliminar - 1)

            blnLineasEliminadas = True
            intRegistoEliminar = oMatriz.GetNextSelectedRow(intRegistoEliminar)

        Loop
        If blnLineasEliminadas Then
            oMatriz.LoadFromDataSource()
            oform.Mode = BoFormMode.fm_UPDATE_MODE
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboTipo(ByRef oForm As Form,
                                               ByVal p_blnSeleccionaValor As Boolean,
                                               ByVal p_strIDValSelect As String)
        Try

            strVal = String.Empty

            oItems = oForm.Items.Item(mc_strTipo)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbTipoVehiculo"), String.Empty))
            If p_blnSeleccionaValor Then
                strVal = p_strIDValSelect
            End If
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Tipo", 0, p_strIDValSelect)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboEstado(ByRef oForm As Form,
                                              ByVal p_blnSeleccionaValor As Boolean,
                                              ByVal p_strIDValSelect As String)
        Try

            strVal = String.Empty

            oItems = oForm.Items.Item(mc_strEstado)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbEstado"), String.Empty))
            If p_blnSeleccionaValor Then
                strVal = p_strIDValSelect
            End If
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Estatus", 0, p_strIDValSelect)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboMoneda(ByRef oForm As Form,
                                      ByVal p_blnSeleccionaValor As Boolean,
                                      ByVal p_strIDValSelect As String)
        Try
            strVal = String.Empty
            oItems = oForm.Items.Item(mc_strMoneda)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbMoneda"), String.Empty))
            If p_blnSeleccionaValor Then
                strVal = p_strIDValSelect
            End If
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Moneda", 0, p_strIDValSelect)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboCategoria(ByRef oForm As Form,
                                      ByVal p_blnSeleccionaValor As Boolean,
                                      ByVal p_strIDValSelect As String)
        Try
            strVal = String.Empty

            oItems = oForm.Items.Item(mc_strCategoria)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbCategoriaVehiculo"), String.Empty))
            If p_blnSeleccionaValor Then
                strVal = p_strIDValSelect
            End If
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Categori", 0, p_strIDValSelect)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    
    Protected Friend Sub CargarComboTipoContrato(ByRef oForm As Form,
                                      ByVal p_blnSeleccionaValor As Boolean,
                                      ByVal p_strIDValSelect As String)
        Try
            strVal = String.Empty

            oItems = oForm.Items.Item(mc_strTipoContrato)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbTipoContrato"), String.Empty))
            If p_blnSeleccionaValor Then
                strVal = p_strIDValSelect
            End If
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_TipoCo", 0, p_strIDValSelect)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboMarcaMot(ByRef oForm As Form,
                                  ByVal p_blnSeleccionaValor As Boolean,
                                  ByVal p_strIDValSelect As String)
        Try
            strVal = String.Empty
            oItems = oForm.Items.Item(mc_strMarcaMotor)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbMarcaMotor"), String.Empty))
            If p_blnSeleccionaValor Then
                strVal = p_strIDValSelect
            End If
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_MarcaMot", 0, p_strIDValSelect)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboTransmision(ByRef oForm As Form,
                                  ByVal p_blnSeleccionaValor As Boolean,
                                  ByVal p_strIDValSelect As String)
        Try
            strVal = String.Empty

            oItems = oForm.Items.Item(mc_strTransmision)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbTrasmision"), String.Empty))
            If p_blnSeleccionaValor Then
                strVal = p_strIDValSelect
            End If
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Transmis", 0, p_strIDValSelect)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboCarroceria(ByRef oForm As Form,
                              ByVal p_blnSeleccionaValor As Boolean,
                              ByVal p_strIDValSelect As String)
        Try
            strVal = String.Empty

            oItems = oForm.Items.Item(mc_strCarroceria)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbCarroceria"), String.Empty))
            If p_blnSeleccionaValor Then
                strVal = p_strIDValSelect
            End If
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Carrocer", 0, p_strIDValSelect)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboTraccion(ByRef oForm As Form,
                           ByVal p_blnSeleccionaValor As Boolean,
                           ByVal p_strIDValSelect As String)
        Try
            strVal = String.Empty

            oItems = oForm.Items.Item(mc_strTraccion)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbTraccion"), String.Empty))
            If p_blnSeleccionaValor Then
                strVal = p_strIDValSelect
            End If
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Tipo_Tra", 0, p_strIDValSelect)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboCabina(ByRef oForm As Form,
                           ByVal p_blnSeleccionaValor As Boolean,
                           ByVal p_strIDValSelect As String)
        Try
            strVal = String.Empty

            oItems = oForm.Items.Item(mc_strCabina)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbCabina"), String.Empty))

            If p_blnSeleccionaValor Then
                strVal = p_strIDValSelect
            End If
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Tip_Cabi", 0, p_strIDValSelect)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboCombustible(ByRef oForm As Form,
                       ByVal p_blnSeleccionaValor As Boolean,
                       ByVal p_strIDValSelect As String)
        Try
            strVal = String.Empty

            oItems = oForm.Items.Item(mc_strCombustible)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbCombustible"), String.Empty))
            If p_blnSeleccionaValor Then
                strVal = p_strIDValSelect
            End If
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Combusti", 0, p_strIDValSelect)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboTecho(ByRef oForm As Form,
                   ByVal p_blnSeleccionaValor As Boolean,
                   ByVal p_strIDValSelect As String)
        Try
            strVal = String.Empty

            oItems = oForm.Items.Item(mc_strTecho)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbTecho"), String.Empty))

            If p_blnSeleccionaValor Then
                strVal = p_strIDValSelect
            End If
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_TipTecho", 0, p_strIDValSelect)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="p_blnSeleccionaValor"></param>
    ''' <param name="p_strIDValSelect"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarComboClasificacion(ByRef oForm As Form,
                  ByVal p_blnSeleccionaValor As Boolean,
                  ByVal p_strIDValSelect As String)
        Try
            strVal = String.Empty

            oItems = oForm.Items.Item(mc_strClasificacion)
            cboCombos = CType(oItems.Specific, ComboBox)

            oForm.Freeze(True)
            Utilitarios.CargarValidValuesEnCombos(cboCombos.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbClasificacion"), String.Empty))

            If p_blnSeleccionaValor Then
                strVal = p_strIDValSelect
            End If
            oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Clasificacion", 0, p_strIDValSelect)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarDescripcionCombo(ByRef oForm As Form)
        Try

            Dim cboCombo As ComboBox
            Dim oItem As Item
            Dim strValor As String

            oForm.Freeze(True)
            'Marca
            oItem = oForm.Items.Item(mc_strMarca)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbMarca"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strMarca, False, "U_Cod_Marc", True)
            'cboCombo.Active = True
            'Estilo
            oItem = oForm.Items.Item(mc_strEstilo)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbEstilo"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strEstilo, False, "U_Cod_Marc", True)
            'Modelo
            oItem = oForm.Items.Item(mc_strModelo)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbModelo"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strModelo, False, "U_Cod_Marc", True)
            'Ubicaciones
            oItem = oForm.Items.Item(mc_strUbicaciones)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbUbicaciones"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strUbicaciones, False, "U_Cod_Marc", True)
            'Tipo Vehiculo
            oItem = oForm.Items.Item(mc_strTipo)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbTipoVehiculo"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strTipo, False, "U_Cod_Marc", True)
            'Estado
            oItem = oForm.Items.Item(mc_strEstado)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbEstado"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strEstado, False, "U_Cod_Marc", True)
            'Disponibilidad
            oItem = oForm.Items.Item(mc_strDisponibilidad)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbDisponibilidad"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strDisponibilidad, False, "U_Cod_Marc", True)
            'Moneda
            oItem = oForm.Items.Item(mc_strMoneda)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbMoneda"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "CurrCode", strValor)), mc_strMoneda, False, "U_Cod_Marc", True)
            'Categoria
            oItem = oForm.Items.Item(mc_strCategoria)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbCategoriaVehiculo"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strCategoria, False, "U_Cod_Marc", True)
            'Tipo contrato
            oItem = oForm.Items.Item(mc_strTipoContrato)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbTipoContrato"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strTipoContrato, False, "U_Cod_Marc", True)
            'marcaMotor
            oItem = oForm.Items.Item(mc_strMarcaMotor)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbMarcaMotor"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strMarcaMotor, False, "U_Cod_Marc", True)
            'Transmision
            oItem = oForm.Items.Item(mc_strTransmision)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbTrasmision"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strTransmision, False, "U_Cod_Marc", True)
            'Carroceria
            oItem = oForm.Items.Item(mc_strCarroceria)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbCarroceria"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strCarroceria, False, "U_Cod_Marc", True)
            'Traccion
            oItem = oForm.Items.Item(mc_strTraccion)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbTraccion"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strTraccion, False, "U_Cod_Marc", True)
            'Cabina
            oItem = oForm.Items.Item(mc_strCabina)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbCabina"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strCabina, False, "U_Cod_Marc", True)
            'Combustible
            oItem = oForm.Items.Item(mc_strCombustible)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbCombustible"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strCombustible, False, "U_Cod_Marc", True)
            'Techo
            oItem = oForm.Items.Item(mc_strTecho)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbTecho"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strTecho, False, "U_Cod_Marc", True)
            oForm.Freeze(False)
            'Clasificacion
            oItem = oForm.Items.Item(mc_strClasificacion)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbClasificacion"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strClasificacion, False, "U_Clasificacion", True)
            'Tipo de PreVenta
            oItem = oForm.Items.Item(mc_strInvPreVenta)
            cboCombo = CType(oItem.Specific, ComboBox)
            strValor = CStr(cboCombo.Value).Trim
            Call CargarValidValuesEnCombos(oForm, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbTipoVehiculo"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "Code", strValor)), mc_strInvPreVenta, False, "U_Tipo_Ven", True)

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FormUID"></param>
    ''' <remarks></remarks>
    Private Sub CargaVehiculo(ByVal FormUID As String)

        Try
            oForm = m_SBO_Application.Forms.Item(FormUID)
            If Not String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("U_ContratoV", 0).ToString.Trim()) Then
                oVehiculo = Carga_Vehiculo.Carga_Vehiculo(m_oCompany, oForm.DataSources.DBDataSources.Item("@SCGD_VEHICULO").GetValue("Code", 0).ToString.Trim())
            End If
            EliminaLineasBlancoBonos(oForm)
            CalculaTotalBonos(oForm)
        Catch ex As Exception
            m_SBO_Application.SetStatusBarMessage(ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oBonosVehiculo"></param>
    ''' <param name="p_oChildrenBonos"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ActualizarBonosContratos(ByVal p_oBonosVehiculo As List(Of BonosXVehiculo), ByVal p_oChildrenBonos As GeneralDataCollection) As Boolean

        Dim blnModificar As Boolean
        Dim blnNuevo As Boolean
        Dim oChildBono As SAPbobsCOM.GeneralData

        Try
            For index As Integer = 0 To p_oChildrenBonos.Count - 1
                oChildBono = p_oChildrenBonos.Item(index)
                blnNuevo = True
                For Each oBono As BonosXVehiculo In p_oBonosVehiculo
                    If oBono.LineId = oChildBono.GetProperty("LineId") Then
                        If oBono.U_Bono <> oChildBono.GetProperty("U_Bono") OrElse oBono.U_Monto <> CDbl(oChildBono.GetProperty("U_Monto")) Then
                            blnModificar = True
                        End If
                        blnNuevo = False
                        p_oBonosVehiculo.Remove(oBono)
                        Exit For
                    End If
                Next
                If blnNuevo Then blnModificar = True
                If blnModificar Then Exit For
            Next
            If p_oBonosVehiculo.Count > 0 Then blnModificar = True
            Return blnModificar
        Catch ex As Exception
            m_SBO_Application.SetStatusBarMessage(ex.Message)
            Return False

        Finally
            Utilitarios.DestruirObjeto(oChildBono)
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oChildrenBonos"></param>
    ''' <param name="p_strUnidad"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RetornaLineasBonosUnidad(p_oChildrenBonos As GeneralDataCollection, p_strUnidad As String) As List(Of Integer)
        Dim listLineasBonos As List(Of Integer)
        Dim oGeneralData As GeneralData
        Try
            listLineasBonos = New List(Of Integer)
            For index As Integer = 0 To p_oChildrenBonos.Count - 1
                oGeneralData = p_oChildrenBonos.Item(index)
                If CStr(oGeneralData.GetProperty("U_Unidad")).Equals(p_strUnidad) Then
                    If Not listLineasBonos.Contains(CInt(oGeneralData.GetProperty("LineId"))) Then
                        listLineasBonos.Add(CInt(oGeneralData.GetProperty("LineId")))
                    End If
                End If
            Next
            Return listLineasBonos
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

#End Region

End Class
