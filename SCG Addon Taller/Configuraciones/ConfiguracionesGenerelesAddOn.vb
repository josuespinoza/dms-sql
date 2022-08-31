Imports DMSOneFramework.SCGDataAccess
Imports System.Collections.Generic
Imports System.Linq
Imports SAPbobsCOM
Imports SCG.DMSOne.Framework.MenuManager
Imports SAPbouiCOM

Public Class ConfiguracionesGenerelesAddOn

    Public Shared IDMenu As String = "SCGD_Config"

#Region "Declaraciones"

    Private Const mc_strUIDConfig As String = "SCGD_CFG"
    Private Const mc_strUIDAdmin As String = "SCGD_ADM"

    'Folders
    Public Const mc_strFolder1 As String = "tab_0"
    Public Const mc_strFolder2 As String = "tab_1"
    Public Const mc_strFolder3 As String = "tab_2"
    Public Const mc_strFolder4 As String = "tab_3"
    Public Const mc_strFolder5 As String = "tab_4"
    Public Const mc_strFolder6 As String = "tab_5"
    Public Const mc_strFolder7 As String = "tab_6"
    Public Const mc_strFolder9 As String = "tab_8"

    'Choose from Lists IDs
    Public Const mc_strCFL_Acct_C As String = "CFL_Acct_C"
    Public Const mc_strCFL_Acct_S As String = "CFL_Acct_S"
    Public Const mc_strCFL_Acct_T As String = "CFL_Acct_T"
    Public Const mc_strCFL_Item As String = "CFL_Item"

    'Nombres de los campos de texto, combos y columnas
    Public Const mc_strpicReporte As String = "picReporte"
    Public Const mc_strbtn_add As String = "btn_add"
    Public Const mc_strbtn_del As String = "btn_del"

    Public Const mc_strbtn_Ventas As String = "btn_Ventas"
    Public Const mc_strbtn_Imp As String = "btn_Imp"
    Public Const mc_strbtn_OC As String = "btn_OC"
    Public Const mc_strbtn_Gastos As String = "btn_Gastos"
    Public Const mc_strbtn_Series As String = "btn_Series"

    Public Const mc_strcol_Ing As String = "col_Ing"
    Public Const mc_strcol_Cost As String = "col_Cost"
    Public Const mc_strcol_Stock As String = "col_Stock"
    Public Const mc_strcol_Tran As String = "col_Tran"
    Public Const mc_strcol_Dev As String = "col_Dev"
    'para almacen por sucursal
    Public Const mc_strcol_AccxAlm As String = "col_AxA"
    Public Const mc_strcol_AlmTram As String = "col_BodTra"
    Public Const mc_strcol_AlmLog As String = "Col_BodLog"
    '
    Public Const mc_strcol_Item As String = "col_Item"
    Public Const mc_strcol_Name As String = "col_Name"
    Public Const mc_strcol_TipoC As String = "col_TipoC"
    Public Const mc_strcol_Imp As String = "col_Imp"
    Public Const mc_strcol_Cuenta As String = "col_Cuenta"
    Public Const mc_strcboInven_V As String = "cboInven_V"
    Public Const mc_strcboInv_R As String = "cboInv_R"
    Public Const mc_strcboOtrosTC As String = "cboOtrosTC"
    Public Const mc_strcboDevolucion As String = "cboDevuelt"
    Public Const mc_strcboOtrosAC As String = "cboOtrosAC"
    Public Const mc_strcboTipoDocumentos As String = "cboTipoDD"
    Public Const mc_strcboCodSNLeasing As String = "cboCodSNL"
    Public Const mc_strtxtPlacaPr As String = "txtPlacaPr"
    Public Const mc_strcboChkValSNL As String = "chkValSNL"
    Public Const mc_strcboTipoItemsVentas As String = "cboTipo1"
    Public Const mc_strcboTipoImpuestos As String = "cboTipo2"
    Public Const mc_strcboTipoGastosVentas As String = "cboTipo3"
    Public Const mc_strcboTipoOtrasCuentas As String = "cboTipo4"
    Public Const mc_strcboTipoSeries As String = "cboTipo5"
    Public Const mc_strcboDisp_V As String = "cboDisp_V"
    Public Const mc_strcboDisp_R As String = "cboDisp_R"
    Public Const mc_strcboDisp_Res As String = "cboDispRes"
    Public Const mc_strcboCRM As String = "cboCRM"
    Public Const mc_strcboEtapaCV As String = "cboEtapaCV"
    Public Const mc_strcol_GA As String = "col_GA"
    Public Const mc_strmtx_Items As String = "mtx_0"
    Public Const mc_strmtx_GastosAdicionales As String = "mtx_1"
    Public Const mc_strmtx_Cuentas As String = "mtx_3"
    Public Const mc_strmtx_Impuestos As String = "mtx_2"
    Public Const mc_strmtx_Series As String = "mtx_Series"
    Public Const mc_strmtx_OtrasCuentas As String = "mtx_OC"
    Public Const mc_strtxtReporte As String = "txtReporte"
    Public Const mc_strcol_Tipo As String = "col_Tipo"
    Public Const mc_strcol_Serie As String = "col_Serie"
    Public Const mc_strcol_SerEx As String = "col_SerEx"
    Public Const mc_strmtx_Indic As String = "mtx_Indic" 'Matriz códigos indicadores
    Public Const mc_strcol_TipoD As String = "Col_Tipo"  'Tipo de documento
    Public Const mc_strcol_Indic As String = "Col_Indic" 'Código indicador

    'Manejo de Niveles de Aprobacion
    Public Const mc_strmtx_NApr As String = "mtx_NApr"
    Public Const mc_strmtx_NAprF As String = "mtx_NivF"
    'Manejo de Mensajeria

    'Código Aplicación
    Private Const mc_strCodigoAplicacion As String = "DMS"

    'Nombre Tablas
    Public Const mc_strSCG_ADMIN As String = "@SCGD_ADMIN"
    Public Const mc_strSCG_ADMIN1 As String = "@SCGD_ADMIN1"
    Public Const mc_strSCG_ADMIN2 As String = "@SCGD_ADMIN2"
    Public Const mc_strSCG_ADMIN3 As String = "@SCGD_ADMIN3"
    Public Const mc_strSCG_ADMIN4 As String = "@SCGD_ADMIN4"
    Public Const mc_strSCG_ADMIN5 As String = "@SCGD_ADMIN5"
    Public Const mc_strSCG_ADMIN6 As String = "@SCGD_ADMIN6"
    Public Const mc_strSCG_ADMIN7 As String = "@SCGD_ADMIN7"
    Public Const mc_strSCG_ADMIN8 As String = "@SCGD_ADMIN8" 'Tabla de Códigos de Indicadores
    Public Const mc_strSCG_ADMIN9 As String = "@SCGD_ADMIN9"
    Public Const mc_strSCG_ADMIN10 As String = "@SCGD_ADMIN10"


    Public Const mc_strSerieFacturaVenta As String = "SerieFacturaVenta"
    Public Const mc_strSerieNotaReciboUsado As String = "SerieNotaReciboUsado"
    Public Const mc_strSerieNotasDebito As String = "SerieNotasDebito"
    Public Const mc_strSerieFacturasDeuda As String = "SerieFacturasDeuda"
    Public Const mc_strSerieNotasDescuento As String = "erieNotasDescuento"

    Public Const mc_strCuentaFacturasDeuda As String = "CuentaFacturasDeuda "
    Public Const mc_strCuentaNotasDescuento As String = "CuentaNotasDescuento"
    Public Const mc_strCuentaNotasDebito As String = "CuentaNotasDebito"
    Public Const mc_strCostoInventario As String = "CostoInventario"
    Public Const mc_strInventarioTransito As String = "InventarioTransito"
    Public Const mc_strInventario As String = "Inventario"
    Public Const mc_strImpuestoFacturaProveedor As String = "ImpuestoFacturaProveedor"
    Public Const mc_strImpuestoNotasDebito As String = "ImpuestoNotasDebito"
    Public Const mc_strImpuestoNotasCredito As String = "ImpuestoNotasCredito"
    Public Const mc_strU_Combusti As String = "U_Combusti"
    Public Const mc_strImpuestoFactura As String = "ImpuestoFactura"
    Public Const mc_strItemInscripcion As String = "ItemInscripcion"
    Public Const mc_strItemVehiculo As String = "ItemVehiculo"
    Public Const mc_strItemPrenda As String = "ItemPrenda"
    Public Const mc_strItemLocales As String = "ItemLocales"

    '********************************************************************************************
    'Agregado 22/02/2012: Cambio CB_perido 
    Public Const mc_strPeriodoCobro As String = "cboPeriodo"

    '********************************************************************************************

    Private m_strTipoFinal As String

    Private m_oForm As Form
    Private SBO_Application As Application

    Private m_lstSeries As List(Of Utilitarios.ListadoValidValues)
    Private m_lstOtrasCuentas As List(Of Utilitarios.ListadoValidValues)
    Private m_lstTipoImpuestos As List(Of Utilitarios.ListadoValidValues)

#End Region

#Region "Constructor"

    <CLSCompliant(False)> _
    Public Sub New(ByRef p_SBO_Aplication As Application)
        SBO_Application = p_SBO_Aplication
    End Sub

#End Region

#Region "Metodos"

    Protected Friend Sub AddMenuItems()

        Dim strEtiquetaMenu As String

        If Utilitarios.MostrarMenu(mc_strUIDAdmin, SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu(mc_strUIDAdmin, SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDAdmin, BoMenuType.mt_STRING, strEtiquetaMenu, 10, False, True, mc_strUIDConfig))

        End If

    End Sub

    Protected Friend Sub CargaFormulario()

        Try
            Dim ltAdmin9 As List(Of Utilitarios.ListadoValidValues)
            Dim oButton As Button
            Dim fcp As FormCreationParams
            Dim oMatrix As Matrix
            Dim strXMLACargar As String
            Dim oConditions As Conditions
            Dim oCondition As Condition

            Dim oConditionsInd As Conditions
            Dim oConditionInd As Condition

            Dim sboItem As Item
            Dim sboCombo As ComboBox
            Dim sboCheck As CheckBox


            fcp = SBO_Application.CreateObject(BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_ADMIN"

            strXMLACargar = My.Resources.Resource.ADMINForm
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oForm = SBO_Application.Forms.AddEx(fcp)

            m_oForm.Items.Item(mc_strFolder1).Specific.Select()
            Call m_oForm.EnableMenu("1281", False)
            Call m_oForm.EnableMenu("1282", False)
            Call m_oForm.EnableMenu("1291", False)
            Call m_oForm.EnableMenu("1288", False)
            Call m_oForm.EnableMenu("1289", False)
            Call m_oForm.EnableMenu("1290", False)
            Call m_oForm.EnableMenu("1293", False)

            oButton = m_oForm.Items.Item(mc_strpicReporte).Specific
            oButton.Image = Windows.Forms.Application.StartupPath.ToString & "\CFL.BMP"

            'Cargo los impuestos segun configuracion
            If (DMS_Connector.Configuracion.ParamGenAddon.U_UsaVATGroup = "Y") Then
                AddChooseFromList(m_oForm, "5", "CFL_Imp")
            Else
                AddChooseFromList(m_oForm, "128", "CFL_Imp")
            End If
            AsignaCFLColumn("mtx_2", "col_Imp", "CFL_Imp", "Code")

            oConditions = SBO_Application.CreateObject(BoCreatableObjectType.cot_Conditions)
            oCondition = oConditions.Add

            'condiciones de Indicadores
            oConditionsInd = SBO_Application.CreateObject(BoCreatableObjectType.cot_Conditions)
            oConditionInd = oConditionsInd.Add

            oConditionInd.Alias = "U_Cod_Ind"
            oConditionInd.Operation = BoConditionOperation.co_EQUAL
            oConditionInd.CondVal = "100"

            m_oForm.Mode = BoFormMode.fm_OK_MODE
            m_oForm.Visible = True

            oCondition.Alias = "Code"
            oCondition.Operation = BoConditionOperation.co_EQUAL
            oCondition.CondVal = mc_strCodigoAplicacion

            Call m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN).Query(oConditions)
            Call m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).Query(oConditions)
            Call m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).Query(oConditions)
            Call m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).Query(oConditions)
            Call m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).Query(oConditions)
            Call m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).Query(oConditions)
            Call m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).Query(oConditions)
            Call m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN7).Query(oConditions)
            Call m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN8).Query(oConditions)
            Call m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN9).Query(oConditions)


            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Cuentas).Specific, Matrix)
            oMatrix.LoadFromDataSource()
            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_GastosAdicionales).Specific, Matrix)
            oMatrix.LoadFromDataSource()
            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Impuestos).Specific, Matrix)
            oMatrix.LoadFromDataSource()
            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Items).Specific, Matrix)
            oMatrix.LoadFromDataSource()
            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_OtrasCuentas).Specific, Matrix)
            oMatrix.LoadFromDataSource()
            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Series).Specific, Matrix)
            oMatrix.LoadFromDataSource()
            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Indic).Specific, Matrix)  'cargar pantalla de indicadores
            oMatrix.LoadFromDataSource()

            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_NApr).Specific, Matrix)  'cargar pantalla de Niveles de Apobacion
            oMatrix.LoadFromDataSource()
            'cargar combo
            For x As Integer = 2 To oMatrix.RowCount - 2
                oMatrix.Columns.Item("Col_Prio").ValidValues.Add(x, "")
            Next

            If m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN).Size = 1 Then
                m_strTipoFinal = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN).GetValue("U_Inven_V", 0).Trim
                m_lstSeries = CrearListadoValidValuesTiposSeries()
                m_lstOtrasCuentas = CrearListadoValidValuesTiposOtrasCuentas()
                m_lstTipoImpuestos = CrearListadoValidValuesImpuestos()

                Call CargarCombos()

                'Agregado 24/09/2010: Conexión entre udf y comboBox de etapas de CV

                sboItem = m_oForm.Items.Item("cboEtapaCV")
                sboCombo = DirectCast(sboItem.Specific, ComboBox)
                Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, DMS_Connector.Queries.GetStrSpecificQuery("strCboEtapaCV"))
                'm_oForm.Items.Item(mc_strcboEtapaCV).Update()
                sboCombo.DataBind.SetBound(True, mc_strSCG_ADMIN, "U_SCGD_EtapaCV")
                If sboCombo.Selected Is Nothing And sboCombo.ValidValues.Count <> 0 Then sboCombo.Select(0, BoSearchKey.psk_Index)

                'Erick Sanabria Bravo (Conexión de UDF con checkbox de validación de fecha reserva.
                sboItem = m_oForm.Items.Item("chkFechRes")
                sboCheck = DirectCast(sboItem.Specific, CheckBox)
                sboCheck.DataBind.SetBound(True, mc_strSCG_ADMIN, "U_FechaRes")

                'Agregado 07/10/2010: Conexion para udf de modificar precio de venta del CV
                sboItem = m_oForm.Items.Item("ckbModPrec")
                sboCheck = DirectCast(sboItem.Specific, CheckBox)
                sboCheck.DataBind.SetBound(True, mc_strSCG_ADMIN, "U_SCGD_ModPrecio")

                'Agregado 13/10/2010: Conexion entre udf y comboBox de bodega de accesorios
                sboItem = m_oForm.Items.Item("cboBodAcc")
                sboCombo = DirectCast(sboItem.Specific, ComboBox)
                Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, DMS_Connector.Queries.GetStrSpecificQuery("strCboBodAcc"))
                'm_oForm.Items.Item(mc_strcboEtapaCV).Update()
                sboCombo.DataBind.SetBound(True, mc_strSCG_ADMIN, "U_SCGD_BodAcc")
                If sboCombo.Selected Is Nothing And sboCombo.ValidValues.Count <> 0 Then sboCombo.Select(0, BoSearchKey.psk_Index)

                'Agregado 14/10/2010: Conexion entre udf y checkBox de accesorios inventariables
                sboItem = m_oForm.Items.Item("ckbAccInv")
                sboCheck = DirectCast(sboItem.Specific, CheckBox)
                sboCheck.DataBind.SetBound(True, mc_strSCG_ADMIN, "U_SCGD_AccInv")

                '********************************************************************************************
                'Agregado 22/02/2012: Conexion entre udf y comboBox de periodo de contrato de ventas
                'Autor: José Soto
                sboItem = m_oForm.Items.Item("cboPeriodo")
                sboCombo = DirectCast(sboItem.Specific, ComboBox)
                Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, DMS_Connector.Queries.GetStrSpecificQuery("strCboPeriodo"))
                'sboCombo.DataBind.SetBound(True, mc_strSCG_ADMIN, "U_Periodo")
                If sboCombo.Selected Is Nothing And sboCombo.ValidValues.Count <> 0 Then sboCombo.Select(0, BoSearchKey.psk_Index)
                m_oForm.Items.Item("cboPeriodo").Update()

                '********************************************************************************************
                ltAdmin9 = New List(Of Utilitarios.ListadoValidValues)()
                For Each admin9 As DMS_Connector.Business_Logic.DataContract.Configuracion.Parametrizaciones_Generales.Admin9 In DMS_Connector.Configuracion.ParamGenAddon.Admin9.OrderBy(Function(cAdmin9) cAdmin9.U_Prio)
                    ltAdmin9.Add(New Utilitarios.ListadoValidValues() With {
                                .strCode = admin9.U_Prio,
                                .strName = admin9.U_Estado,
                                .blnExistente = False
                                })
                Next
                'Cargar el combo de nivel de aprobación para vehículos usados
                sboItem = m_oForm.Items.Item("cboNivApro")
                sboCombo = DirectCast(sboItem.Specific, ComboBox)
                Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, ltAdmin9)
                sboCombo.ValidValues.Add("--", My.Resources.Resource.Ninguno)
                m_oForm.Items.Item("cboNivApro").Update()


                sboItem = m_oForm.Items.Item("cboNivFin")
                sboCombo = DirectCast(sboItem.Specific, ComboBox)
                Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, ltAdmin9)
                sboCombo.ValidValues.Add("--", My.Resources.Resource.Ninguno)
                m_oForm.Items.Item("cboNivFin").Update()

                sboItem = m_oForm.Items.Item("cboNivUsa")
                sboCombo = DirectCast(sboItem.Specific, ComboBox)
                Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, ltAdmin9)
                sboCombo.ValidValues.Add("--", My.Resources.Resource.Ninguno)
                m_oForm.Items.Item("cboNivUsa").Update()

                sboItem = m_oForm.Items.Item("cboMonDef")
                sboCombo = DirectCast(sboItem.Specific, ComboBox)
                Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, String.Format("Select ""CurrCode"", ""CurrName"" from ""OCRN"" {0}", DMS_Connector.Queries.GetStrQueryFormat("strNoLock")))
                If sboCombo.Selected Is Nothing And sboCombo.ValidValues.Count <> 0 Then sboCombo.Select(0, BoSearchKey.psk_Index)
                m_oForm.Items.Item("cboMonDef").Update()

                sboItem = m_oForm.Items.Item("cboAlmTra")
                sboCombo = DirectCast(sboItem.Specific, ComboBox)
                Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, String.Format("Select ""WhsCode"", ""WhsName"" from ""OWHS"" {0}", DMS_Connector.Queries.GetStrQueryFormat("strNoLock")))
                If sboCombo.Selected Is Nothing And sboCombo.ValidValues.Count <> 0 Then sboCombo.Select(0, BoSearchKey.psk_Index)
                m_oForm.Items.Item("cboAlmTra").Update()

                sboItem = m_oForm.Items.Item("cboCTCA")
                sboCombo = DirectCast(sboItem.Specific, ComboBox)
                Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, "Select ""Code"", ""Name"" from ""@SCGD_TRAN_COMP"" ")
                If sboCombo.Selected Is Nothing And sboCombo.ValidValues.Count <> 0 Then sboCombo.Select(0, BoSearchKey.psk_Index)
                m_oForm.Items.Item("cboCTCA").Update()

                sboItem = m_oForm.Items.Item("cboAlmOC")
                sboCombo = DirectCast(sboItem.Specific, ComboBox)
                Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, "Select ""WhsCode"", ""WhsName"" from ""OWHS"" ")
                If sboCombo.Selected Is Nothing And sboCombo.ValidValues.Count <> 0 Then sboCombo.Select(0, BoSearchKey.psk_Index)
                m_oForm.Items.Item("cboAlmOC").Update()


                ValidaEstadoComproSNL(m_oForm)
            Else
                Call CrearConfInicial()
                m_oForm.Close()
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ConfiguracionInicializada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try
    End Sub

    Private Sub CrearConfInicial()
        Dim oCompanyService As CompanyService = Nothing
        Dim oGeneralService As GeneralService = Nothing
        Dim oGeneralData As GeneralData = Nothing

        Try
            oCompanyService = DMS_Connector.Company.CompanySBO.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_ADMIN")
            oGeneralData = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData)
            oGeneralData.SetProperty("Code", "DMS")
            oGeneralData.SetProperty("Name", "DMS")
            oGeneralService.Add(oGeneralData)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, DMS_Connector.Company.ApplicationSBO)
        Finally
            Utilitarios.DestruirObjeto(oCompanyService)
            Utilitarios.DestruirObjeto(oGeneralService)
            Utilitarios.DestruirObjeto(oGeneralData)
        End Try
    End Sub

    Private Sub CargarCombos()

        Dim ocombo As ComboBox
        Dim oMatrix As Matrix
        Dim lsGenericList As List(Of Utilitarios.ListadoValidValues)

        ''********************************************************************************************
        ''Agregado 22/02/2012: Variable periodo
        'Dim strEstadoPeriodo As String
        ''********************************************************************************************


        Dim strConsultaSeries As String = String.Format(DMS_Connector.Queries.GetStrQueryFormat("strNumeracionesFacturas"), My.Resources.Resource.DescripcionFacturaVentas, My.Resources.Resource.DescripcionNotasCredito, My.Resources.Resource.DescripcionDocumentosDeuda)

        ocombo = DirectCast(m_oForm.Items.Item(mc_strcboCRM).Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, DMS_Connector.Queries.GetStrSpecificQuery("strOOST"))
        m_oForm.Items.Item(mc_strcboCRM).Update()

        lsGenericList = New List(Of Utilitarios.ListadoValidValues)
        For Each drRow As DataRow In Utilitarios.EjecutarConsultaDataTable(DMS_Connector.Queries.GetStrSpecificQuery("strDisponibilidad")).Rows
            lsGenericList.Add(New Utilitarios.ListadoValidValues With {
                               .strCode = drRow.Item(0).ToString.Trim,
                               .strName = drRow.Item(1).ToString.Trim,
                               .blnExistente = False})
        Next

        ocombo = DirectCast(m_oForm.Items.Item(mc_strcboDisp_R).Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, lsGenericList)
        m_oForm.Items.Item(mc_strcboDisp_R).Update()

        ocombo = DirectCast(m_oForm.Items.Item(mc_strcboDisp_V).Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, lsGenericList)
        m_oForm.Items.Item(mc_strcboDisp_V).Update()

        ocombo = DirectCast(m_oForm.Items.Item(mc_strcboDisp_Res).Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, lsGenericList)
        m_oForm.Items.Item(mc_strcboDisp_Res).Update()

        ocombo = DirectCast(m_oForm.Items.Item(mc_strcboDevolucion).Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, lsGenericList)
        m_oForm.Items.Item(mc_strcboDevolucion).Update()

        lsGenericList.Clear()
        For Each drRow As DataRow In Utilitarios.EjecutarConsultaDataTable(DMS_Connector.Queries.GetStrSpecificQuery("strTipoVehiculo")).Rows
            lsGenericList.Add(New Utilitarios.ListadoValidValues With {
                               .strCode = drRow.Item(0).ToString.Trim,
                               .strName = drRow.Item(1).ToString.Trim,
                               .blnExistente = False})
        Next

        ocombo = DirectCast(m_oForm.Items.Item(mc_strcboInv_R).Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, lsGenericList)
        m_oForm.Items.Item(mc_strcboInv_R).Update()

        ocombo = DirectCast(m_oForm.Items.Item(mc_strcboInven_V).Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, lsGenericList)
        m_oForm.Items.Item(mc_strcboInven_V).Update()

        ocombo = DirectCast(m_oForm.Items.Item(mc_strcboOtrosAC).Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, DMS_Connector.Queries.GetStrSpecificQuery("strOTRC"))
        m_oForm.Items.Item(mc_strcboOtrosAC).Update()

        ocombo = DirectCast(m_oForm.Items.Item(mc_strcboOtrosTC).Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, DMS_Connector.Queries.GetStrSpecificQuery("strTranComp"))
        m_oForm.Items.Item(mc_strcboOtrosTC).Update()

        If Not String.IsNullOrEmpty(m_strTipoFinal) Then

            lsGenericList.Clear()
            For Each drRow As DataRow In Utilitarios.EjecutarConsultaDataTable(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strTipoVehiculoWhere"), m_strTipoFinal.Trim)).Rows
                lsGenericList.Add(New Utilitarios.ListadoValidValues With {
                                   .strCode = drRow.Item(0).ToString.Trim,
                                   .strName = drRow.Item(1).ToString.Trim,
                                   .blnExistente = False})
            Next

            ocombo = DirectCast(m_oForm.Items.Item(mc_strcboTipoItemsVentas).Specific, SAPbouiCOM.ComboBox)
            Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, lsGenericList)

            ocombo = DirectCast(m_oForm.Items.Item(mc_strcboTipoGastosVentas).Specific, SAPbouiCOM.ComboBox)
            Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, lsGenericList)

            ocombo = DirectCast(m_oForm.Items.Item(mc_strcboTipoImpuestos).Specific, SAPbouiCOM.ComboBox)
            Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, lsGenericList)

            ocombo = DirectCast(m_oForm.Items.Item(mc_strcboTipoOtrasCuentas).Specific, SAPbouiCOM.ComboBox)
            Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, lsGenericList)

            ocombo = DirectCast(m_oForm.Items.Item(mc_strcboTipoSeries).Specific, SAPbouiCOM.ComboBox)
            Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, lsGenericList)

            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Impuestos).Specific, SAPbouiCOM.Matrix)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_Tipo).ValidValues, lsGenericList)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_Tran).ValidValues, m_lstTipoImpuestos)

            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Cuentas).Specific, SAPbouiCOM.Matrix)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_TipoC).ValidValues, lsGenericList)

            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Items).Specific, SAPbouiCOM.Matrix)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_Tipo).ValidValues, lsGenericList)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_Tran).ValidValues, CrearListadoValidValuesItems)

            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_GastosAdicionales).Specific, SAPbouiCOM.Matrix)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_Tipo).ValidValues, lsGenericList)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_Tran).ValidValues, CrearListadoValidValuesItems)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_Tran).ValidValues, CrearListadoValidValuesItems)

            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Series).Specific, SAPbouiCOM.Matrix)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_Tipo).ValidValues, lsGenericList)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_Tran).ValidValues, m_lstSeries)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_Serie).ValidValues, strConsultaSeries)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_SerEx).ValidValues, strConsultaSeries)

            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_OtrasCuentas).Specific, SAPbouiCOM.Matrix)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_Tipo).ValidValues, lsGenericList)
            Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_Tran).ValidValues, m_lstOtrasCuentas)

        End If

        ocombo = DirectCast(m_oForm.Items.Item(mc_strcboTipoDocumentos).Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, CrearListadoValidValuesTiposDocumentosDeuda)
        m_oForm.Items.Item(mc_strcboTipoDocumentos).Update()
        ocombo = DirectCast(m_oForm.Items.Item(mc_strcboCodSNLeasing).Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, DMS_Connector.Queries.GetStrSpecificQuery("strGruoposSN"))
        m_oForm.Items.Item(mc_strcboCodSNLeasing).Update()
        oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_GastosAdicionales).Specific, SAPbouiCOM.Matrix)
        Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item(mc_strcol_GA).ValidValues, DMS_Connector.Queries.GetStrSpecificQuery("strExpense"))

    End Sub

    Private Function CrearListadoValidValuesItems() As Generic.List(Of Utilitarios.ListadoValidValues)

        Dim oListadoValidValues As New Generic.List(Of Utilitarios.ListadoValidValues)
        Dim oValidValue As Utilitarios.ListadoValidValues

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = CStr(ConfiguracionesGeneralesAddon.scgItemsFactura.PrecioVehículo)
        oValidValue.strName = My.Resources.Resource.DescripcionPrecioVehículo
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = CStr(ConfiguracionesGeneralesAddon.scgItemsFactura.PrecioAccesorios)
        oValidValue.strName = My.Resources.Resource.DescripciónPrecioAccesorios
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = CStr(ConfiguracionesGeneralesAddon.scgItemsFactura.gastosIncripcion)
        oValidValue.strName = My.Resources.Resource.DescripcionGastosInscripcion
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = CStr(ConfiguracionesGeneralesAddon.scgItemsFactura.GastosPrenda)
        oValidValue.strName = My.Resources.Resource.DescripcionGastosPrenda
        oListadoValidValues.Add(oValidValue)

        Return oListadoValidValues

    End Function

    Private Function CrearListadoValidValuesTiposDocumentosDeuda() As Generic.List(Of Utilitarios.ListadoValidValues)

        Dim oListadoValidValues As New Generic.List(Of Utilitarios.ListadoValidValues)
        Dim oValidValue As Utilitarios.ListadoValidValues

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = SAPbobsCOM.BoDocumentSubType.bod_InvoiceExempt
        oValidValue.strName = My.Resources.Resource.bod_InvoiceExempt
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = SAPbobsCOM.BoDocumentSubType.bod_DebitMemo
        oValidValue.strName = My.Resources.Resource.bod_DebitMemo
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = SAPbobsCOM.BoDocumentSubType.bod_Bill
        oValidValue.strName = My.Resources.Resource.bod_Bill
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = SAPbobsCOM.BoDocumentSubType.bod_ExemptBill
        oValidValue.strName = My.Resources.Resource.bod_ExemptBill
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = SAPbobsCOM.BoDocumentSubType.bod_PurchaseDebitMemo
        oValidValue.strName = My.Resources.Resource.bod_PurchaseDebitMemo
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = SAPbobsCOM.BoDocumentSubType.bod_ExportInvoice
        oValidValue.strName = My.Resources.Resource.bod_ExportInvoice
        oListadoValidValues.Add(oValidValue)

        Return oListadoValidValues

    End Function

    Private Function CrearListadoValidValuesTiposSeries() As List(Of Utilitarios.ListadoValidValues)

        Dim oListadoValidValues As New List(Of Utilitarios.ListadoValidValues)
        Dim oValidValue As Utilitarios.ListadoValidValues

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.DocumentosDeuda
        oValidValue.strName = My.Resources.Resource.DescripcionDocumentosDeuda
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaProveedor
        oValidValue.strName = My.Resources.Resource.DescripcionFacturaProveedor
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaVentas
        oValidValue.strName = My.Resources.Resource.DescripcionFacturaVentas
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotasCreditoDescuentos
        oValidValue.strName = My.Resources.Resource.DescripcionNotasCreditoDescuentos
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotasCreditoUsados
        oValidValue.strName = My.Resources.Resource.DescripcionNotasCreditoUsados
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotasCreditoOtros
        oValidValue.strName = My.Resources.Resource.DescripcionNotasCreditoOtros
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.DocumentosDeudaOtros
        oValidValue.strName = My.Resources.Resource.DescripcionDocumentosDeudaOtros
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.PrimaVenta
        oValidValue.strName = My.Resources.Resource.DescripcionPrimaVenta
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotasCreditoReversion
        oValidValue.strName = My.Resources.Resource.DescripcionNotasCreditoReversion
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaAccesorios
        oValidValue.strName = My.Resources.Resource.DescripcionFacturaAccesorios
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado
        oValidValue.strName = My.Resources.Resource.DescripcionFacturaExentaUsados
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaProveedoresDocumentoReciboUsadoSociedades
        oValidValue.strName = My.Resources.Resource.DescripcionFacturaProveedorDocumentoReciboUsadoSociedades
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaProveedoresDocumentoReciboUsadoPrivado
        oValidValue.strName = My.Resources.Resource.DescripcionFacturaProveedorDocumentoReciboUsadoPrivado
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotaCreditoReciboUsadoSociedades
        oValidValue.strName = My.Resources.Resource.NotaCreditoReciboUsadoSociedades
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotaCreditoReciboUsadoPrivado
        oValidValue.strName = My.Resources.Resource.NotaCreditoReciboUsadoPrivado
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.TramitesFacturables
        oValidValue.strName = My.Resources.Resource.DescripcionTramitesFacturables
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotaCreditoReversionTramites
        oValidValue.strName = My.Resources.Resource.DescripcionNotaCreditoReversionTramites
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotaCreditoReversionAccesorios
        oValidValue.strName = My.Resources.Resource.DescripcionNotaCreditoReversionAccesorios
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaComisionConsignados
        oValidValue.strName = My.Resources.Resource.FacturaComisionConsignados
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotaCreditoComisionConsignados
        oValidValue.strName = My.Resources.Resource.NotaCreditoComisionConsignados
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotaDebitoClienteReversionNCUsados
        oValidValue.strName = My.Resources.Resource.DescripcionNotaDebitoClienteReversionNCUsados
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaGastos
        oValidValue.strName = My.Resources.Resource.DescripcionFacturaGastos
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotaCreditoReversionGastos
        oValidValue.strName = My.Resources.Resource.DescripcionNotaCreditoReversionGastos
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotaDebitoReversionNCDescuento
        oValidValue.strName = My.Resources.Resource.DescripcionNotaDebitoReversionNCDescuento
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotaCreditoReversionFacturaDeudaUsado
        oValidValue.strName = My.Resources.Resource.DescripcionNotaCreditoReversionFacturaDeudaUsado
        oListadoValidValues.Add(oValidValue)

        Return oListadoValidValues

    End Function

    Private Function CrearListadoValidValuesImpuestos() As List(Of Utilitarios.ListadoValidValues)

        Dim oListadoValidValues As New List(Of Utilitarios.ListadoValidValues)
        Dim oValidValue As Utilitarios.ListadoValidValues

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.DocumentosDeuda
        oValidValue.strName = My.Resources.Resource.DescripcionDocumentosDeuda
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaProveedor
        oValidValue.strName = My.Resources.Resource.DescripcionFacturaProveedor
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaVentas
        oValidValue.strName = My.Resources.Resource.DescripcionFacturaVentas
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotasCreditoDescuentos
        oValidValue.strName = My.Resources.Resource.DescripcionNotasCreditoDescuentos
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotasCreditoUsados
        oValidValue.strName = My.Resources.Resource.DescripcionNotasCreditoUsados
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado
        oValidValue.strName = My.Resources.Resource.DescripcionFacturaExentaUsados
        oListadoValidValues.Add(oValidValue)

        Return oListadoValidValues

    End Function

    Private Function CrearListadoValidValuesTiposOtrasCuentas() As List(Of Utilitarios.ListadoValidValues)

        Dim oListadoValidValues As New List(Of Utilitarios.ListadoValidValues)
        Dim oValidValue As Utilitarios.ListadoValidValues

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.DocumentosDeuda
        oValidValue.strName = My.Resources.Resource.DescripcionDocumentosDeuda
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaProveedor
        oValidValue.strName = My.Resources.Resource.DescripcionFacturaProveedor
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.NotasCreditoDescuentos
        oValidValue.strName = My.Resources.Resource.DescripcionNotasCreditoDescuentos
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = ConfiguracionesGeneralesAddon.scgTipoSeries.PrimaVenta
        oValidValue.strName = My.Resources.Resource.DescripcionPrimaVenta
        oListadoValidValues.Add(oValidValue)

        Return oListadoValidValues

    End Function

    Private Function CargarDesdeXML(ByRef strFileName As String) As String

        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String

        strPath = Windows.Forms.Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml

    End Function

    <CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                                   ByRef pVal As ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)
        Try


            Dim oForm As Form
            oForm = SBO_Application.Forms.Item(FormUID)
            If pVal.ItemUID = mc_strFolder1 Or _
            pVal.ItemUID = mc_strFolder2 Or _
            pVal.ItemUID = mc_strFolder3 Or _
            pVal.ItemUID = mc_strFolder4 Or _
            pVal.ItemUID = mc_strFolder5 Or _
            pVal.ItemUID = mc_strFolder6 Or _
            pVal.ItemUID = mc_strFolder7 Then

                Call ManejoEventosTab(oForm, pVal)

            Else


                If Not oForm Is Nothing _
                        AndAlso pVal.ActionSuccess Then

                    Select Case pVal.ItemUID
                        Case mc_strpicReporte
                            Call IniciarProcesoFolderDialog()
                        Case mc_strbtn_add
                            Call AgregarLineaCuentas(pVal.FormUID)
                        Case mc_strbtn_del
                            Call EliminarLíneasCuentas(pVal.FormUID)
                        Case mc_strbtn_Ventas
                            Call CargarGridItems()

                        Case mc_strbtn_Gastos
                            Call CargarGridGastos()

                        Case mc_strbtn_Imp
                            Call CargarGridImpuestos()

                        Case mc_strbtn_Series
                            Call CargarGridSeries()

                        Case mc_strbtn_OC
                            Call CargarOtrasCuentas()
                        Case "tab_7"
                            oForm.PaneLevel = 8
                        Case mc_strFolder9
                            oForm.PaneLevel = 9
                        Case "btnAddNA"
                            Call AgregarLineaNivAprob(FormUID)
                            oForm.Items.Item("btnAddNA").Enabled = False
                        Case "btnEliNA"
                            Call EliminarLíneasNivAprob(FormUID)
                        Case "1"
                            DMS_Connector.Configuracion.Carga_ParametrizacionesGenerales()
                            Call ActualizaNivelesAprobación(oForm)
                        Case mc_strcboChkValSNL
                            ValidaEstadoComproSNL(oForm)
                    End Select
                ElseIf Not oForm Is Nothing _
                    AndAlso Not pVal.ActionSuccess _
                    AndAlso pVal.BeforeAction Then

                    Select Case pVal.ItemUID
                        Case "1"
                            'valida los niveles a actualizar
                            ValidaNiveles(BubbleEvent, oForm)
                    End Select
                End If
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("ManejadorEventoItemPressedGenOV" & "" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ValidaNiveles(ByRef BubbleEvent As Boolean, ByVal oForm As Form)
        'matriz
        Dim oMatrizNiveles As Matrix
        'prioridades
        Dim strPrioridadPrimera As String
        Dim strPrioridadSegunda As String
        Dim strPrioridadFinal As String = ""
        Dim strPrioridades As New List(Of String)
        Dim strCodigos As New List(Of String)
        Dim InsertaPrioridad As Boolean = True
        Dim InsertaCodigo As Boolean = True
        Dim strCodigo As String = ""

        'Matrices
        oMatrizNiveles = DirectCast(oForm.Items.Item("mtx_NApr").Specific, Matrix)

        If oMatrizNiveles.RowCount >= 3 Then
            strPrioridadPrimera = oMatrizNiveles.Columns.Item("Col_Prio").Cells.Item(1).Specific.Value
            strPrioridadSegunda = oMatrizNiveles.Columns.Item("Col_Prio").Cells.Item(2).Specific.Value
            strPrioridadFinal = oMatrizNiveles.Columns.Item("Col_Prio").Cells.Item(3).Specific.Value

            'Verifica las prioridades qeu no pueden cambiar
            If Not String.IsNullOrEmpty(strPrioridadPrimera) _
               AndAlso Not String.IsNullOrEmpty(strPrioridadSegunda) _
                AndAlso Not String.IsNullOrEmpty(strPrioridadFinal) Then
                If DMS_Connector.Configuracion.ParamGenAddon.Admin9.Count > 3 Then
                    'validaciond e las 2 primeras prioridades y la ultima
                    If Not strPrioridadPrimera = CStr(DMS_Connector.Configuracion.ParamGenAddon.Admin9.Item(0).U_Prio) _
                        OrElse Not strPrioridadSegunda = CStr(DMS_Connector.Configuracion.ParamGenAddon.Admin9.Item(1).U_Prio) _
                        OrElse Not strPrioridadFinal = oMatrizNiveles.RowCount - 1 Then
                        oForm.DataSources.DBDataSources.Item("@SCGD_ADMIN9").SetValue("U_Prio", 2, oMatrizNiveles.RowCount - 1)
                        oMatrizNiveles.LoadFromDataSource()
                        'ERROR
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorNivelesAprobacion, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                        BubbleEvent = False
                        Exit Sub
                    End If
                End If
            Else
                'error prioridad nulas
                BubbleEvent = False
            End If
        End If

        For i As Integer = 1 To oMatrizNiveles.RowCount

            If String.IsNullOrEmpty(oMatrizNiveles.Columns.Item("Col_Name").Cells.Item(i).Specific.Value) Then
                'error nombre en Blanco
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorNombreEnBlanco, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
                Exit Sub
            End If
            If String.IsNullOrEmpty(oMatrizNiveles.Columns.Item("Col_Prio").Cells.Item(i).Specific.Value) Then
                'error prioridad en Blanco
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorPrioridadEnBlanco, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
                Exit Sub
            End If

            If Not String.IsNullOrEmpty(strPrioridadFinal) Then

                'validacion prioridades mayores a Facturada
                If oMatrizNiveles.Columns.Item("Col_Prio").Cells.Item(i).Specific.Value > Integer.Parse(strPrioridadFinal) Then
                    'error prioridad Mayor
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorPrioridadesMayores, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                    BubbleEvent = False
                    Exit Sub
                End If
            End If

            'validacion prioridades repetidas
            For Each Prioridad As String In strPrioridades
                If Prioridad = oMatrizNiveles.Columns.Item("Col_Prio").Cells.Item(i).Specific.Value Then
                    'ya existe la prioridad
                    InsertaPrioridad = False
                    Exit For
                End If
            Next

            If InsertaPrioridad Then
                strPrioridades.Add(oMatrizNiveles.Columns.Item("Col_Prio").Cells.Item(i).Specific.Value)
            Else
                'error prioridad repetida
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorPrioridadesRepetidas, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
                Exit Sub
            End If

            'validacion niveles en blanco
            If String.IsNullOrEmpty(oMatrizNiveles.Columns.Item("Col_Cod").Cells.Item(i).Specific.Value) Then
                'error Nivel en Blanco
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCodigoEnBlanco, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
                Exit Sub
            End If

            'validacion estados en blanco
            If String.IsNullOrEmpty(oMatrizNiveles.Columns.Item("Col_Estado").Cells.Item(i).Specific.Value) Then
                'error Nivel en Blanco
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorEstadoEnBlanco, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
                Exit Sub
            End If

            'validacion tamano de codigos
            strCodigo = oMatrizNiveles.Columns.Item("Col_Cod").Cells.Item(i).Specific.Value
            If strCodigo.Length > 8 Then
                'error tamano codigo
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorTamanoCodigo, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
                Exit Sub
            End If

            'validacion de codigos repetidos
            For Each Codigo As String In strCodigos
                If Codigo = oMatrizNiveles.Columns.Item("Col_Cod").Cells.Item(i).Specific.Value Then
                    'ya existe el codigo
                    InsertaCodigo = False
                    Exit For
                End If
            Next

            If InsertaCodigo Then
                strCodigos.Add(oMatrizNiveles.Columns.Item("Col_Cod").Cells.Item(i).Specific.Value)
            Else
                'error codigo repetida
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCodigosRepetidas, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
                Exit Sub
            End If
        Next
        If BubbleEvent Then
            oForm.Items.Item("btnAddNA").Enabled = True
        End If
    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoChooseFromList(ByRef pval As SAPbouiCOM.ItemEvent, _
                                              ByVal FormUID As String, _
                                              ByRef BubbleEvent As Boolean)


        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)

        Dim sCFL_ID As String
        sCFL_ID = oCFLEvento.ChooseFromListUID
        Dim oForm As SAPbouiCOM.Form
        oForm = SBO_Application.Forms.Item(FormUID)
        Dim oCFL As SAPbouiCOM.ChooseFromList
        oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
        Dim oDataTable As SAPbouiCOM.DataTable

        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        If oCFLEvento.ActionSuccess Then

            oDataTable = oCFLEvento.SelectedObjects
            If Not oCFLEvento.SelectedObjects Is Nothing Then
                Dim intTipoCuenta As ConfiguracionesGeneralesAddon.scgTipoCuenta

                oDataTable = oCFLEvento.SelectedObjects

                'case "Col_tax":
                '    if (oCFLEvento.SelectedObjects != null)
                '    {
                '        tax = oDataTable.GetValue("Code", 0).ToString().Trim();
                '        oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(g_strmtxDocCompra).Specific;
                '        oMatrix.FlushToDataSource();
                '        oForm.DataSources.DataTables.Item(g_strdtDocCompra).SetValue("tax", pVal.Row - 1, tax);
                '        oMatrix.LoadFromDataSource();
                '    }


                Select Case pval.ColUID
                    Case mc_strcol_Tran
                        intTipoCuenta = ConfiguracionesGeneralesAddon.scgTipoCuenta.scgCuentaTransito
                    Case mc_strcol_Stock
                        intTipoCuenta = ConfiguracionesGeneralesAddon.scgTipoCuenta.scgCuentaStock
                    Case mc_strcol_Cost
                        intTipoCuenta = ConfiguracionesGeneralesAddon.scgTipoCuenta.scgCuentaCosto
                    Case mc_strcol_Ing
                        intTipoCuenta = ConfiguracionesGeneralesAddon.scgTipoCuenta.scgCuentaIngreso
                    Case mc_strcol_AccxAlm
                        intTipoCuenta = ConfiguracionesGeneralesAddon.scgTipoCuenta.scgAlmacenSucursal
                    Case mc_strcol_AlmTram
                        intTipoCuenta = ConfiguracionesGeneralesAddon.scgTipoCuenta.scgAlmacenTramites
                    Case mc_strcol_AlmLog
                        intTipoCuenta = ConfiguracionesGeneralesAddon.scgTipoCuenta.scgAlmacenLogistica
                    Case mc_strcol_Dev
                        intTipoCuenta = ConfiguracionesGeneralesAddon.scgTipoCuenta.scgCuentaDevolucion
                    Case mc_strcol_Imp
                        Call AsignarImpuesto(pval, oDataTable)

                End Select

                Select Case pval.ItemUID

                    Case mc_strmtx_Cuentas
                        Call AsignarCuentas(pval, oDataTable, intTipoCuenta)
                    Case mc_strmtx_Items
                        Call AsignarItems(pval, oDataTable)
                    Case mc_strmtx_OtrasCuentas
                        Call AsignarOtrasCuentas(pval, oDataTable)
                End Select

            End If

        ElseIf oCFLEvento.BeforeAction Then
            'se condiciona la columna ya que este alias funciona para el chooseFromList de Cuentas de Stocks y
            'no para el choosefromlist de Almacenes
            If (pval.ItemUID = mc_strmtx_Cuentas Or pval.ItemUID = mc_strmtx_OtrasCuentas) AndAlso Not (pval.ColUID = mc_strcol_AccxAlm OrElse pval.ColUID = mc_strcol_AlmTram OrElse pval.ColUID = mc_strcol_AlmLog) Then

                oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "FatherNum"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                oCondition.BracketCloseNum = 1
                oCFL.SetConditions(oConditions)

            End If

            If (pval.ItemUID = mc_strmtx_Impuestos) Then

                If pval.ColUID = mc_strcol_Imp Then

                    oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                    oCondition = oConditions.Add()
                    If (DMS_Connector.Configuracion.ParamGenAddon.U_UsaVATGroup = "Y") Then
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "Locked"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "N"
                        oCondition.BracketCloseNum = 1
                    Else
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "Lock"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "N"
                        oCondition.BracketCloseNum = 1
                    End If
                    oCFL.SetConditions(oConditions)

                End If

            End If

        End If

    End Sub

    <MTAThread()> _
    Private Sub MostrarFolderDialog()
        Dim strSelectedPath As String
        Try
            Dim objFolderDialog As New Windows.Forms.FolderBrowserDialog
            Dim nw As New NativeWindow


            nw.AssignHandle(System.Diagnostics.Process.GetProcessesByName("SAP Business One")(SBO_Application.AppId).MainWindowHandle)

            objFolderDialog.ShowNewFolderButton = False
            objFolderDialog.ShowDialog(nw)
            strSelectedPath = objFolderDialog.SelectedPath
            If Not String.IsNullOrEmpty(strSelectedPath) Then
                m_oForm.Items.Item(mc_strtxtReporte).Specific.String = strSelectedPath
                m_oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
            End If


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("ManejadorEventoItemPressedGenOV" & "" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

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

        'Dim oItem As SAPbouiCOM.Item

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
        ElseIf pval.ItemUID = mc_strFolder6 Then
            oTmpForm.PaneLevel = 6
        ElseIf pval.ItemUID = mc_strFolder7 Then
            oTmpForm.PaneLevel = 7
        End If

    End Sub

    Private Sub IniciarProcesoFolderDialog()

        Dim threadGetFile As Threading.Thread
        Try
            threadGetFile = New Threading.Thread(New Threading.ThreadStart(AddressOf MostrarFolderDialog))
            threadGetFile.SetApartmentState(Threading.ApartmentState.STA)

            threadGetFile.Start()
            While threadGetFile.IsAlive
                Threading.Thread.Sleep(1)
                threadGetFile.Join()
            End While
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("ManejadorEventoItemPressedGenOV" & "" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

        Finally
            threadGetFile = Nothing

        End Try
    End Sub

    Private Sub AsignarCuentas(ByVal pval As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable, ByVal p_intTipoCuenta As ConfiguracionesGeneralesAddon.scgTipoCuenta)
        Dim omatrix As SAPbouiCOM.Matrix

        m_oForm = SBO_Application.Forms.Item(pval.FormUID)
        omatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Cuentas).Specific, SAPbouiCOM.Matrix)
        omatrix.FlushToDataSource()
        Select Case p_intTipoCuenta
            Case ConfiguracionesGeneralesAddon.scgTipoCuenta.scgCuentaStock
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).SetValue("U_Stock", pval.Row - 1, oDataTable.GetValue("AcctCode", 0))
            Case ConfiguracionesGeneralesAddon.scgTipoCuenta.scgCuentaTransito
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).SetValue("U_Transito", pval.Row - 1, oDataTable.GetValue("AcctCode", 0))
            Case ConfiguracionesGeneralesAddon.scgTipoCuenta.scgCuentaCosto
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).SetValue("U_Costo", pval.Row - 1, oDataTable.GetValue("AcctCode", 0))
            Case ConfiguracionesGeneralesAddon.scgTipoCuenta.scgCuentaIngreso
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).SetValue("U_Ingreso", pval.Row - 1, oDataTable.GetValue("AcctCode", 0))
            Case ConfiguracionesGeneralesAddon.scgTipoCuenta.scgAlmacenSucursal
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).SetValue("U_AccXAlm", pval.Row - 1, oDataTable.GetValue("WhsCode", 0))
            Case ConfiguracionesGeneralesAddon.scgTipoCuenta.scgAlmacenTramites
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).SetValue("U_Bod_Tram", pval.Row - 1, oDataTable.GetValue("WhsCode", 0))
            Case ConfiguracionesGeneralesAddon.scgTipoCuenta.scgAlmacenLogistica
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).SetValue("U_Bod_Log", pval.Row - 1, oDataTable.GetValue("WhsCode", 0))
            Case ConfiguracionesGeneralesAddon.scgTipoCuenta.scgCuentaDevolucion
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).SetValue("U_Devolucion", pval.Row - 1, oDataTable.GetValue("AcctCode", 0))

        End Select

        omatrix.LoadFromDataSource()

        m_oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

    End Sub

    Private Sub AsignarOtrasCuentas(ByVal pval As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)
        Dim omatrix As SAPbouiCOM.Matrix

        m_oForm = SBO_Application.Forms.Item(pval.FormUID)
        omatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_OtrasCuentas).Specific, SAPbouiCOM.Matrix)
        omatrix.FlushToDataSource()
        m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).SetValue("U_Cuenta", pval.Row - 1, oDataTable.GetValue("AcctCode", 0))

        omatrix.LoadFromDataSource()

        m_oForm.Mode = BoFormMode.fm_UPDATE_MODE

    End Sub

    Private Sub AsignarImpuesto(ByVal pval As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Dim oMatrix As SAPbouiCOM.Matrix

        oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Impuestos).Specific, SAPbouiCOM.Matrix)
        m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).SetValue("U_Cod_Imp", pval.Row - 1, oDataTable.GetValue("Code", 0).ToString.Trim)

        oMatrix.LoadFromDataSource()

        m_oForm.Mode = BoFormMode.fm_UPDATE_MODE

    End Sub

    Private Sub AsignarItems(ByVal pval As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)
        Dim omatrix As SAPbouiCOM.Matrix

        m_oForm = SBO_Application.Forms.Item(pval.FormUID)
        omatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Items).Specific, SAPbouiCOM.Matrix)
        omatrix.FlushToDataSource()

        m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).SetValue("U_ItemCode", pval.Row - 1, oDataTable.GetValue("ItemCode", 0))
        m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).SetValue("U_ItemName", pval.Row - 1, oDataTable.GetValue("ItemName", 0))


        omatrix.LoadFromDataSource()
    End Sub

    Private Sub AgregarLineaCuentas(ByVal p_strFormID As String)

        Dim oform As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intNuevoRegisto As Integer
        Dim blnLineasAgregadas As Boolean = False
        Dim strUsuario As String

        oform = SBO_Application.Forms.Item(p_strFormID)
        oMatriz = DirectCast(oform.Items.Item(mc_strmtx_Cuentas).Specific, SAPbouiCOM.Matrix)

        intNuevoRegisto = oform.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).Size
        If intNuevoRegisto = 0 Then

            oform.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).InsertRecord(intNuevoRegisto)
            intNuevoRegisto += 1

        Else
            strUsuario = oform.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).GetValue("U_Tipo", intNuevoRegisto - 1)
            If Not String.IsNullOrEmpty(strUsuario.Trim()) Then
                oform.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).InsertRecord(intNuevoRegisto)

                intNuevoRegisto += 1
            ElseIf intNuevoRegisto = 1 Then
                oform.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).SetValue("U_Stock", 0, " ")
            End If
        End If
        blnLineasAgregadas = True

        If blnLineasAgregadas Then
            oMatriz.LoadFromDataSource()
            oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If

    End Sub

    Private Sub EliminarLíneasCuentas(ByVal p_strFormID As String)

        Dim oform As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intRegistoEliminar As Integer
        Dim blnLineasEliminadas As Boolean = False

        oform = SBO_Application.Forms.Item(p_strFormID)
        oMatriz = DirectCast(oform.Items.Item(mc_strmtx_Cuentas).Specific, SAPbouiCOM.Matrix)
        intRegistoEliminar = oMatriz.GetNextSelectedRow()
        Do While intRegistoEliminar > -1

            oform.DataSources.DBDataSources.Item(mc_strSCG_ADMIN4).RemoveRecord(intRegistoEliminar - 1)

            blnLineasEliminadas = True
            intRegistoEliminar = oMatriz.GetNextSelectedRow(intRegistoEliminar)

        Loop
        If blnLineasEliminadas Then
            oMatriz.LoadFromDataSource()
            oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If

    End Sub

    Private Sub AgregarLineaNivAprob(ByVal p_strFormID As String)

        'Dim oform As SAPbouiCOM.Form
        Dim oMatrixNA As SAPbouiCOM.Matrix
        'Dim intNuevoRegisto As Integer
        'Dim intPrioridad As Integer
        Dim intTamano As Integer
        'Dim blnLineasAgregadas As Boolean = False

        m_oForm = SBO_Application.Forms.Item(p_strFormID)

        'ambas matrices
        oMatrixNA = DirectCast(m_oForm.Items.Item(mc_strmtx_NApr).Specific, SAPbouiCOM.Matrix)

        intTamano = oMatrixNA.RowCount

        'si no hay registros en la matriz si limpia el datasource
        If intTamano = 0 Then
            m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN9).Clear()
        End If

        m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN9).InsertRecord(intTamano)
        oMatrixNA.LoadFromDataSource()
        'blnLineasAgregadas = True

        'If blnLineasAgregadas Then
        'ubico el ds en Niveles Fijos
        m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN9).SetValue("U_Prio", 2, intTamano)
        oMatrixNA.LoadFromDataSource()
        'modo actualizar al form
        m_oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        'cargo el combo
        If intTamano > 1 Then
            If oMatrixNA.Columns.Item("Col_Prio").ValidValues.Count <= intTamano Then
                oMatrixNA.Columns.Item("Col_Prio").ValidValues.Add(intTamano - 1, "")
            End If
        ElseIf intTamano = 0 Then
            m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN9).SetValue("U_Prio", 0, intTamano)
        ElseIf intTamano = 1 Then
            m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN9).SetValue("U_Prio", 1, intTamano)
        End If
        oMatrixNA.LoadFromDataSource()

    End Sub

    Private Sub EliminarLíneasNivAprob(ByVal p_strFormID As String)

        Dim oMatrixNA As SAPbouiCOM.Matrix
        Dim oMatrixNAF As SAPbouiCOM.Matrix
        Dim intRegistoEliminar As Integer
        Dim intUltimo As Integer
        Dim intTamanoPrioridades As Integer
        Dim blnLineasEliminadas As Boolean = False
        'Dim strNA As String = ""
        Dim strCodigo As String = ""
        Dim ErrorNA As Boolean = False

        m_oForm = SBO_Application.Forms.Item(p_strFormID)

        oMatrixNA = DirectCast(m_oForm.Items.Item(mc_strmtx_NApr).Specific, SAPbouiCOM.Matrix)
        For i As Integer = 1 To oMatrixNA.RowCount
            If oMatrixNA.IsRowSelected(i) Then
                'codigo a eliminar
                strCodigo = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN9).GetValue("U_Codigo", i - 1)
                strCodigo = strCodigo.Trim
                intRegistoEliminar = i
                Exit For
            End If
        Next

        If Not String.IsNullOrEmpty(strCodigo) Then
            'posee un usuario asociado??

            If CInt(Utilitarios.EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCountCNAp"), strCodigo))) = 0 Then
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN9).RemoveRecord(intRegistoEliminar - 1)
                blnLineasEliminadas = True
                ErrorNA = False
                oMatrixNA.LoadFromDataSource()
            Else
                'error
                GoTo ErrorEliminaNA
            End If
        End If

        If blnLineasEliminadas Then
            intTamanoPrioridades = oMatrixNA.Columns.Item("Col_Prio").ValidValues.Count
            If intTamanoPrioridades > 0 Then
                oMatrixNA.Columns.Item("Col_Prio").ValidValues.Remove(intTamanoPrioridades - 1, BoSearchKey.psk_Index)
            End If
            m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN9).SetValue("U_Prio", 2, oMatrixNA.Columns.Item("Col_Prio").ValidValues.Count + 2)
            oMatrixNA.LoadFromDataSource()
            m_oForm.Mode = BoFormMode.fm_UPDATE_MODE
        End If

        If ErrorNA Then
ErrorEliminaNA:
            SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorEliminaNA, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
        End If
    End Sub

    Private Sub CargarGridItems()

        Dim oMatrix As Matrix
        Dim oCombo As ComboBox
        Dim strTipoSeleccionado As String = ""

        Dim blnPrimerRegistro As Boolean = False
        Dim blnAgregarPrecioventa As Boolean = True
        Dim blnAgregarPrecioAccesorios As Boolean = True

        Dim intLineId As Integer = 0
        Dim strLineID As String = ""

        Dim intNumeroRegistro As Integer = 0

        oCombo = DirectCast(m_oForm.Items.Item(mc_strcboTipoItemsVentas).Specific, ComboBox)
        If oCombo.Selected IsNot Nothing Then
            strTipoSeleccionado = oCombo.Selected.Value
        End If
        If Not String.IsNullOrEmpty(strTipoSeleccionado) Then
            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Items).Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()

            For intNumeroRegistro = 0 To m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).Size - 1

                If String.IsNullOrEmpty(m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).GetValue("U_Tipo", intNumeroRegistro)) Then
                    blnPrimerRegistro = True
                ElseIf m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).GetValue("U_Tipo", intNumeroRegistro).Trim = strTipoSeleccionado Then
                    Select Case CType(m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).GetValue("U_Cod_Item", intNumeroRegistro).Trim, ConfiguracionesGeneralesAddon.scgItemsFactura)
                        Case ConfiguracionesGeneralesAddon.scgItemsFactura.PrecioAccesorios
                            blnAgregarPrecioAccesorios = False
                        Case ConfiguracionesGeneralesAddon.scgItemsFactura.PrecioVehículo
                            blnAgregarPrecioventa = False
                    End Select
                End If
            Next

            intNumeroRegistro = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).Size

            If blnAgregarPrecioventa Then
                If Not blnPrimerRegistro Then
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).InsertRecord(intNumeroRegistro - 1)
                Else
                    intNumeroRegistro = 0
                End If
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).SetValue("U_Cod_Item", intNumeroRegistro, ConfiguracionesGeneralesAddon.scgItemsFactura.PrecioVehículo)
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).SetValue("U_Tipo", intNumeroRegistro, strTipoSeleccionado)
                If intNumeroRegistro > 0 Then
                    strLineID = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).GetValue("LineId", intNumeroRegistro - 1)
                    If IsNumeric(strLineID) Then
                        intLineId = CInt(strLineID)
                    Else
                        intLineId = -1
                    End If
                    If intLineId > -1 Then
                        m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).SetValue("LineId", intNumeroRegistro, intLineId + 1)
                    End If
                End If
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).SetValue("U_ItemCode", intNumeroRegistro, "")
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).SetValue("U_ItemName", intNumeroRegistro, "")
                intNumeroRegistro += 1
            End If

            If blnAgregarPrecioAccesorios Then
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).InsertRecord(intNumeroRegistro - 1)
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).SetValue("U_Cod_Item", intNumeroRegistro, ConfiguracionesGeneralesAddon.scgItemsFactura.PrecioAccesorios)
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).SetValue("U_Tipo", intNumeroRegistro, strTipoSeleccionado)
                strLineID = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).GetValue("LineId", intNumeroRegistro - 1)
                If IsNumeric(strLineID) Then
                    intLineId = CInt(strLineID)
                Else
                    intLineId = -1
                End If
                If intLineId > -1 Then
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).SetValue("LineId", intNumeroRegistro, intLineId + 1)
                End If
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).SetValue("U_ItemCode", intNumeroRegistro, "")
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN1).SetValue("U_ItemName", intNumeroRegistro, "")
            End If

            oMatrix.LoadFromDataSource()

            m_oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If

    End Sub

    Private Sub CargarGridGastos()

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim strTipoSeleccionado As String = ""

        Dim blnPrimerRegistro As Boolean = False
        Dim blnAgregarGastosPrenda As Boolean = True
        Dim blnAgregarGastosInscripcion As Boolean = True

        Dim intLineId As Integer = 0
        Dim strLineId As String = ""

        Dim intNumeroRegistro As Integer = 0

        oCombo = DirectCast(m_oForm.Items.Item(mc_strcboTipoGastosVentas).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            strTipoSeleccionado = oCombo.Selected.Value
        End If
        If Not String.IsNullOrEmpty(strTipoSeleccionado) Then
            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_GastosAdicionales).Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()

            For intNumeroRegistro = 0 To m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).Size - 1

                If String.IsNullOrEmpty(m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).GetValue("U_Tipo", intNumeroRegistro)) Then
                    blnPrimerRegistro = True
                ElseIf m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).GetValue("U_Tipo", intNumeroRegistro).Trim = strTipoSeleccionado Then
                    Select Case CType(m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).GetValue("U_Cod_Item", intNumeroRegistro).Trim, ConfiguracionesGeneralesAddon.scgItemsFactura)
                        Case ConfiguracionesGeneralesAddon.scgItemsFactura.GastosPrenda
                            blnAgregarGastosPrenda = False
                        Case ConfiguracionesGeneralesAddon.scgItemsFactura.gastosIncripcion
                            blnAgregarGastosInscripcion = False
                    End Select
                End If
            Next

            intNumeroRegistro = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).Size

            If blnAgregarGastosInscripcion Then
                If Not blnPrimerRegistro Then
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).InsertRecord(intNumeroRegistro - 1)
                Else
                    intNumeroRegistro = 0
                End If
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).SetValue("U_Cod_Item", intNumeroRegistro, ConfiguracionesGeneralesAddon.scgItemsFactura.gastosIncripcion)
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).SetValue("U_Tipo", intNumeroRegistro, strTipoSeleccionado)
                If intNumeroRegistro > 0 Then
                    strLineId = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).GetValue("LineId", intNumeroRegistro - 1)
                    If IsNumeric(strLineId) Then
                        intLineId = CInt(strLineId)
                    Else
                        intLineId = -1
                    End If
                    If intLineId > -1 Then
                        m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).SetValue("LineId", intNumeroRegistro, intLineId + 1)
                    End If
                End If
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).SetValue("U_Cod_GA", intNumeroRegistro, "")
                intNumeroRegistro += 1
            End If

            If blnAgregarGastosPrenda Then
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).InsertRecord(intNumeroRegistro - 1)
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).SetValue("U_Cod_Item", intNumeroRegistro, ConfiguracionesGeneralesAddon.scgItemsFactura.GastosPrenda)
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).SetValue("U_Tipo", intNumeroRegistro, strTipoSeleccionado)
                strLineId = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).GetValue("LineId", intNumeroRegistro - 1)
                If IsNumeric(strLineId) Then
                    intLineId = CInt(strLineId)
                Else
                    intLineId = -1
                End If
                If intLineId > -1 Then
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).SetValue("LineId", intNumeroRegistro, intLineId)

                End If
                m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN2).SetValue("U_Cod_GA", intNumeroRegistro, "")
            End If
            oMatrix.LoadFromDataSource()

            m_oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If

    End Sub

    Private Sub CargarGridSeries()

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim strTipoSeleccionado As String = String.Empty

        Dim blnPrimerRegistro As Boolean = False

        Dim intLineId As Integer = 0
        Dim strLineId As String

        Dim intNumeroRegistro As Integer = 0
        Dim objSeries As Utilitarios.ListadoValidValues
        Dim blnYaEstaRegistrada As Boolean = False

        Dim objSeriesAAgregar As Utilitarios.ListadoValidValues
        Dim lstListaValoresAAgregar As New Generic.List(Of Utilitarios.ListadoValidValues)

        oCombo = DirectCast(m_oForm.Items.Item(mc_strcboTipoSeries).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            strTipoSeleccionado = oCombo.Selected.Value
        End If
        If Not String.IsNullOrEmpty(strTipoSeleccionado) Then
            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Series).Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()

            For Each objSeries In m_lstSeries
                For intNumeroRegistro = 0 To m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).Size - 1

                    If String.IsNullOrEmpty(m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).GetValue("U_Tipo", intNumeroRegistro)) Then
                        blnPrimerRegistro = True
                        Exit For
                    ElseIf m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).GetValue("U_Tipo", intNumeroRegistro).Trim = strTipoSeleccionado Then


                        If m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).GetValue("U_Cod_Item", intNumeroRegistro).Trim = objSeries.strCode Then

                            blnYaEstaRegistrada = True
                            Exit For
                        End If
                    End If
                Next
                If Not blnYaEstaRegistrada Then
                    objSeriesAAgregar = New Utilitarios.ListadoValidValues
                    objSeriesAAgregar.strCode = objSeries.strCode
                    objSeriesAAgregar.strName = objSeries.strName
                    lstListaValoresAAgregar.Add(objSeriesAAgregar)
                Else
                    blnYaEstaRegistrada = False
                End If
            Next
            intNumeroRegistro = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).Size

            If Not blnPrimerRegistro Then
                For Each objSeries In lstListaValoresAAgregar

                    If Not blnPrimerRegistro Then
                        m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).InsertRecord(intNumeroRegistro - 1)
                    Else
                        intNumeroRegistro = 0
                        blnPrimerRegistro = False
                    End If
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).SetValue("U_Cod_Item", intNumeroRegistro, objSeries.strCode)
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).SetValue("U_Tipo", intNumeroRegistro, strTipoSeleccionado)
                    If intNumeroRegistro > 0 Then
                        strLineId = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).GetValue("LineId", intNumeroRegistro - 1)
                        If IsNumeric(strLineId) Then
                            intLineId = CInt(strLineId)
                        Else
                            intLineId = -1
                        End If
                        If intLineId > -1 Then
                            m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).SetValue("LineId", intNumeroRegistro, intLineId + 1)
                        End If
                    End If
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).SetValue("U_Serie", intNumeroRegistro, "")
                    intNumeroRegistro += 1

                Next
            Else
                For Each objSeries In m_lstSeries

                    If Not blnPrimerRegistro Then
                        m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).InsertRecord(intNumeroRegistro - 1)
                    Else
                        intNumeroRegistro = 0
                        blnPrimerRegistro = False
                    End If
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).SetValue("U_Cod_Item", intNumeroRegistro, objSeries.strCode)
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).SetValue("U_Tipo", intNumeroRegistro, strTipoSeleccionado)
                    If intNumeroRegistro > 0 Then
                        strLineId = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).GetValue("LineId", intNumeroRegistro - 1)
                        If IsNumeric(strLineId) Then
                            intLineId = CInt(strLineId)
                        Else
                            intLineId = -1
                        End If
                        If intLineId > -1 Then
                            m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).SetValue("LineId", intNumeroRegistro, intLineId + 1)
                        End If
                    End If
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN6).SetValue("U_Serie", intNumeroRegistro, "")
                    intNumeroRegistro += 1

                Next
            End If
            oMatrix.LoadFromDataSource()

            m_oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If

    End Sub

    Private Sub CargarOtrasCuentas()

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim strTipoSeleccionado As String = String.Empty

        Dim blnPrimerRegistro As Boolean = False

        Dim intLineId As Integer = 0
        Dim strLineId As String

        Dim intNumeroRegistro As Integer = 0
        Dim objOtrasCuentas As Utilitarios.ListadoValidValues
        Dim blnYaEstaRegistrada As Boolean = False

        Dim objOtrasCuentasAAgregar As Utilitarios.ListadoValidValues
        Dim lstListaValoresAAgregar As New Generic.List(Of Utilitarios.ListadoValidValues)

        oCombo = DirectCast(m_oForm.Items.Item(mc_strcboTipoOtrasCuentas).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            strTipoSeleccionado = oCombo.Selected.Value
        End If
        If Not String.IsNullOrEmpty(strTipoSeleccionado) Then
            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_OtrasCuentas).Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()

            For Each objOtrasCuentas In m_lstOtrasCuentas
                For intNumeroRegistro = 0 To m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).Size - 1

                    If String.IsNullOrEmpty(m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).GetValue("U_Tipo", intNumeroRegistro)) Then
                        blnPrimerRegistro = True
                        Exit For
                    ElseIf m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).GetValue("U_Tipo", intNumeroRegistro).Trim = strTipoSeleccionado Then


                        If m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).GetValue("U_Cod_Item", intNumeroRegistro).Trim = objOtrasCuentas.strCode Then

                            blnYaEstaRegistrada = True
                            Exit For
                        End If
                    End If
                Next
                If Not blnYaEstaRegistrada Then
                    objOtrasCuentasAAgregar = New Utilitarios.ListadoValidValues
                    objOtrasCuentasAAgregar.strCode = objOtrasCuentas.strCode
                    objOtrasCuentasAAgregar.strName = objOtrasCuentas.strName
                    lstListaValoresAAgregar.Add(objOtrasCuentasAAgregar)
                Else
                    blnYaEstaRegistrada = False
                End If
            Next
            intNumeroRegistro = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).Size

            If Not blnPrimerRegistro Then
                For Each objOtrasCuentas In lstListaValoresAAgregar

                    If Not blnPrimerRegistro Then
                        m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).InsertRecord(intNumeroRegistro - 1)
                    Else
                        intNumeroRegistro = 0
                        blnPrimerRegistro = False
                    End If
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).SetValue("U_Cod_Item", intNumeroRegistro, objOtrasCuentas.strCode)
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).SetValue("U_Tipo", intNumeroRegistro, strTipoSeleccionado)
                    If intNumeroRegistro > 0 Then
                        strLineId = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).GetValue("LineId", intNumeroRegistro - 1)
                        If IsNumeric(strLineId) Then
                            intLineId = CInt(strLineId)
                        Else
                            intLineId = -1
                        End If
                        If intLineId > -1 Then
                            m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).SetValue("LineId", intNumeroRegistro, intLineId + 1)
                        End If
                    End If
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).SetValue("U_Cuenta", intNumeroRegistro, "")
                    intNumeroRegistro += 1

                Next
            Else
                For Each objOtrasCuentas In m_lstOtrasCuentas

                    If Not blnPrimerRegistro Then
                        m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).InsertRecord(intNumeroRegistro - 1)
                    Else
                        intNumeroRegistro = 0
                        blnPrimerRegistro = False
                    End If
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).SetValue("U_Cod_Item", intNumeroRegistro, objOtrasCuentas.strCode)
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).SetValue("U_Tipo", intNumeroRegistro, strTipoSeleccionado)
                    If intNumeroRegistro > 0 Then
                        strLineId = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).GetValue("LineId", intNumeroRegistro - 1)
                        If IsNumeric(strLineId) Then
                            intLineId = CInt(strLineId)
                        Else
                            intLineId = -1
                        End If
                        If intLineId > -1 Then
                            m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).SetValue("LineId", intNumeroRegistro, intLineId + 1)
                        End If
                    End If
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN5).SetValue("U_Cuenta", intNumeroRegistro, "")
                    intNumeroRegistro += 1

                Next
            End If
            oMatrix.LoadFromDataSource()

            m_oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If

    End Sub

    Private Sub CargarGridImpuestos()

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim strTipoSeleccionado As String = String.Empty

        Dim blnPrimerRegistro As Boolean = False

        Dim intLineId As Integer = 0
        Dim strLineId As String

        Dim intNumeroRegistro As Integer = 0
        Dim objSeries As Utilitarios.ListadoValidValues
        Dim blnYaEstaRegistrada As Boolean = False

        Dim objSeriesAAgregar As Utilitarios.ListadoValidValues
        Dim lstListaValoresAAgregar As New Generic.List(Of Utilitarios.ListadoValidValues)

        oCombo = DirectCast(m_oForm.Items.Item(mc_strcboTipoImpuestos).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            strTipoSeleccionado = oCombo.Selected.Value
        End If
        If Not String.IsNullOrEmpty(strTipoSeleccionado) Then
            oMatrix = DirectCast(m_oForm.Items.Item(mc_strmtx_Impuestos).Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()

            For Each objSeries In m_lstTipoImpuestos
                For intNumeroRegistro = 0 To m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).Size - 1

                    If String.IsNullOrEmpty(m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).GetValue("U_Tipo", intNumeroRegistro)) Then
                        blnPrimerRegistro = True
                        Exit For
                    ElseIf m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).GetValue("U_Tipo", intNumeroRegistro).Trim = strTipoSeleccionado Then


                        If m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).GetValue("U_Cod_Item", intNumeroRegistro).Trim = objSeries.strCode Then

                            blnYaEstaRegistrada = True
                            Exit For
                        End If
                    End If
                Next
                If Not blnYaEstaRegistrada Then
                    objSeriesAAgregar = New Utilitarios.ListadoValidValues
                    objSeriesAAgregar.strCode = objSeries.strCode
                    objSeriesAAgregar.strName = objSeries.strName
                    lstListaValoresAAgregar.Add(objSeriesAAgregar)
                Else
                    blnYaEstaRegistrada = False
                End If
            Next
            intNumeroRegistro = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).Size

            If Not blnPrimerRegistro Then
                For Each objSeries In lstListaValoresAAgregar

                    If Not blnPrimerRegistro Then
                        m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).InsertRecord(intNumeroRegistro - 1)
                    Else
                        intNumeroRegistro = 0
                        blnPrimerRegistro = False
                    End If
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).SetValue("U_Cod_Item", intNumeroRegistro, objSeries.strCode)
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).SetValue("U_Tipo", intNumeroRegistro, strTipoSeleccionado)
                    If intNumeroRegistro > 0 Then
                        strLineId = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).GetValue("LineId", intNumeroRegistro - 1)
                        If IsNumeric(strLineId) Then
                            intLineId = CInt(strLineId)
                        Else
                            intLineId = -1
                        End If
                        If intLineId > -1 Then
                            m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).SetValue("LineId", intNumeroRegistro, intLineId + 1)
                        End If
                    End If
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).SetValue("U_Cod_Imp", intNumeroRegistro, "")
                    intNumeroRegistro += 1

                Next
            Else
                For Each objSeries In m_lstTipoImpuestos

                    If Not blnPrimerRegistro Then
                        m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).InsertRecord(intNumeroRegistro - 1)
                    Else
                        intNumeroRegistro = 0
                        blnPrimerRegistro = False
                    End If
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).SetValue("U_Cod_Item", intNumeroRegistro, objSeries.strCode)
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).SetValue("U_Tipo", intNumeroRegistro, strTipoSeleccionado)
                    If intNumeroRegistro > 0 Then
                        strLineId = m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).GetValue("LineId", intNumeroRegistro - 1)
                        If IsNumeric(strLineId) Then
                            intLineId = CInt(strLineId)
                        Else
                            intLineId = -1
                        End If
                        If intLineId > -1 Then
                            m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).SetValue("LineId", intNumeroRegistro, intLineId + 1)
                        End If
                    End If
                    m_oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN3).SetValue("U_Cod_Imp", intNumeroRegistro, "")
                    intNumeroRegistro += 1

                Next
            End If
            oMatrix.LoadFromDataSource()

            m_oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If

    End Sub

    Private Sub ActualizaNivelesAprobación(ByVal oForm As Form)

        Dim strUltimoDocEntry As String = String.Empty
        Dim intUltimoDocEntry As Integer = 0
        Dim strConsultaDocEntry As String = "SELECT MAX(""DocEntry"") FROM ""@SCGD_NIVELES_PV"" "
        Dim strConsultaInsert As String = " INSERT INTO ""@SCGD_NIVELES_PV"" (""Code"", ""Name"", ""DocEntry"", ""Canceled"", ""Object"", ""Transfered"", ""DataSource"", ""U_Estado"") VALUES (N'{0}', N'{1}', {2}, N'N', N'SCGD_NIVELES_PV', N'N', N'I', N'Y') "
        Dim strConsultaEliminar As String = "DELETE FROM ""@SCGD_NIVELES_PV"" WHERE ""U_Estado"" = 'Y'"

        Try

            Utilitarios.EjecutarConsulta(strConsultaEliminar)

            strUltimoDocEntry = Utilitarios.EjecutarConsulta(strConsultaDocEntry)
            intUltimoDocEntry = Integer.Parse(strUltimoDocEntry)

            For i As Integer = 1 To oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN9).Size

                Utilitarios.EjecutarConsulta(
                    String.Format(strConsultaInsert,
                                  oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN9).GetValue("U_Codigo", i - 1).Trim(),
                                  oForm.DataSources.DBDataSources.Item(mc_strSCG_ADMIN9).GetValue("U_Name", i - 1).Trim(),
                                  intUltimoDocEntry + i))
            Next

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub
    Private Sub ValidaEstadoComproSNL(ByVal oForm As Form)
        Dim chkValSNL As SAPbouiCOM.CheckBox
        chkValSNL = DirectCast(oForm.Items.Item(mc_strcboChkValSNL).Specific, CheckBox)

        If chkValSNL.Checked Then
            oForm.Items.Item(mc_strcboCodSNLeasing).Enabled = True
        Else
            oForm.Items.Item(mc_strtxtPlacaPr).Click()
            oForm.Items.Item(mc_strcboCodSNLeasing).Enabled = False
        End If
    End Sub

    Private Sub AddChooseFromList(ByVal oform As SAPbouiCOM.Form, ByVal ObjectType As String, ByVal UniqueID As String)
        Try

            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection

            oCFLs = oform.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = ObjectType
            oCFLCreationParams.UniqueID = UniqueID

            oCFL = oCFLs.Add(oCFLCreationParams)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Private Sub AsignaCFLColumn(ByVal p_strMatriz As String, ByVal p_strColumn As String, ByVal p_strCFL As String, ByVal p_Alias As String)
        Try
            Dim oitem As SAPbouiCOM.Item
            Dim oMatrix As SAPbouiCOM.Matrix

            oitem = m_oForm.Items.Item(p_strMatriz)
            oMatrix = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)

            oMatrix.Columns.Item(p_strColumn).ChooseFromListUID = p_strCFL
            oMatrix.Columns.Item(p_strColumn).ChooseFromListAlias = p_Alias
            '-----------------------------------------------
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

#End Region

End Class
