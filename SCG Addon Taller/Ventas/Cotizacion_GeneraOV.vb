Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports System.Collections.Generic
Imports SCG.DMSOne.Framework.MenuManager
Imports SAPbouiCOM
Imports System.Linq

Partial Class CotizacionCLS

#Region "GeneraOrdenesDeVenta"

#Region "Enumeradores"

    Private Enum TiposArt

        scgRepuesto = 1
        scgActividad = 2
        scgSuministro = 3
        scgServicioExt = 4
        scgPaquete = 5
        scgOtros = 6
        scgAccesorio = 7
        scgVehiculo = 8
        scgTramite = 9
        scgCita = 10
        scgNinguno = 0
        scgOtrosGastos_Costos = 11
        scgOtrosIngresos = 12

    End Enum

#End Region
#Region "Estructuras"

    Private Structure Cuentas

        Const CuentaIngresos As String = "RevenuesAc"
        Const CuentaCostos As String = "SaleCostAc"
        Const CuentaGastos As String = "ExpensesAc"

    End Structure

#End Region
#Region "Declaraciones"

    Private Const mc_strIdMainMenu As String = "43520"

    Public Const mc_strUIDGeneraOV As String = "SCGD_GOV"
    Private Const mc_strUIDSubGeneraOV As String = "SCGD_CEO"

    Private Const mc_strOQUT As String = "OQUT"
    Private Const mc_strEmpId As String = "OwnerCode"
    Private Const mc_strEstadoCot As String = "U_SCGD_Estado_Cot"
    Private Const mc_strEstadoCotID As String = "U_SCGD_Estado_CotID"
    Private Const mc_strDocStatus As String = "DocStatus"

    'Matriz
    Private Const mc_strMTZCotizacion As String = "mtCoti"

    'Nombres de columnas de matrix
    Private Const mc_strUIDNoCotizacion As String = "col_NoCot"
    Private Const mc_strUIDNoOT As String = "col_OT"
    Private Const mc_strUIDEmpid As String = "col_empid"
    Private Const mc_strUIDNombreEmp As String = "col_emp"
    Private Const mc_strUIDPlaca As String = "col_Placa"
    Private Const mc_strUIDMarca As String = "col_marca"
    Private Const mc_strUIDModelo As String = "col_Mod"

    'Parametros de Configuracion
    Dim adpConf As ConfiguracionDataAdapter
    Dim dstConf As New ConfiguracionDataSet
    Dim dstConfBXCC As New ConfBodegasXCentroCostoDataSet

    'Nombres de campos del datasource
    Private Const mc_strDocNum As String = "DocEntry" '"DocNum"
    Private Const mc_strNumeroOT As String = "U_SCGD_Numero_OT"
    Private Const mc_strNum_Placa As String = "U_SCGD_Num_Placa"
    Private Const mc_strDes_Marc As String = "U_SCGD_Des_Marc"
    Private Const mc_strDes_Mode As String = "U_SCGD_Des_Mode"

    Private Const mc_strGridOV As String = "grdOV"
    
    Private Const mc_stretNoAsesor As String = "etNoAsesor"
    Private Const mc_stretAsesor As String = "et_Asesor"

    Private Const mc_strtxtAsesor As String = "txtAses"

    Private m_dbCotizacion As DBDataSource

    Private m_oFormGenCotizacion As Form
    
    Private Const mc_intErrorOperationNoSupported As Integer = -5006
    
    Private strValidaUsaMontoTotalKit As String
    Private strCodeItem As String
    Private strFather As String
    Private strTreeType As String
    Private strChildNum As String
    
    Private oDataTableDimensionesContablesDMS As DataTable
    'Private ListaConfiguracionOT As Hashtable
    Private ListaConfiguracionOT As List(Of LineasConfiguracionOT)

    Public Const mc_strDataTableDimensionesOT As String = "DimensionesContablesDMSOT"

    Public ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls = New AgregarDimensionLineasDocumentosCls(m_oCompany, SBO_Application)

    Private blnUsaDimensiones As Boolean = False

    Public oDataTableConfiguracionesSucursalOV As Data.DataTable
    Public oDataRowConfiguracionSucursalOV As DataRow

    Private _udsFormulario As UserDataSources
    Private _txt As SCG.SBOFramework.UI.EditTextSBO

#End Region

#Region "Metodos"

    Protected Friend Sub AddMenuItems()

        Dim strEtiquetaMenu As String
        Dim sPath As String

        sPath = Windows.Forms.Application.StartupPath
        strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_GOV", SBO_Application.Language)

        GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDGeneraOV, BoMenuType.mt_POPUP, strEtiquetaMenu, 15, False, True, sPath & "\etiqueta.bmp", mc_strIdMainMenu))

        If Utilitarios.MostrarMenu("SCGD_CEO", SBO_Application.Company.UserName) Then
            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_CEO", SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDSubGeneraOV, BoMenuType.mt_STRING, strEtiquetaMenu, 5, False, True, mc_strUIDGeneraOV))
        End If

    End Sub

    Protected Friend Sub CargaFormularioGeneraOV()

        Dim fcp As FormCreationParams
        Dim oMatrix As Matrix
        Dim oEdit As SAPbouiCOM.EditText
        Dim strXMLACargar As String

        Try
            fcp = SBO_Application.CreateObject(BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_DET_2"

            strXMLACargar = My.Resources.Resource.Cotizacion
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)

            'Bloqueo del nombre del asesor
            oEdit = m_oFormGenCotizacion.Items.Item(mc_strtxtAsesor).Specific
            oEdit.Value = String.Empty

            oEdit = m_oFormGenCotizacion.Items.Item(mc_stretAsesor).Specific
            oEdit.Item.Enabled = False

            'Agrego los Data Sources
            _udsFormulario = m_oFormGenCotizacion.DataSources.UserDataSources
            _udsFormulario.Add(mc_stretAsesor, BoDataType.dt_LONG_TEXT, 100)
            _udsFormulario.Add(mc_strtxtAsesor, BoDataType.dt_SHORT_NUMBER, 10)
            _udsFormulario.Add(mc_stretNoAsesor, BoDataType.dt_SHORT_NUMBER, 10)

            _txt = New SCG.SBOFramework.UI.EditTextSBO(mc_stretAsesor, True, "", mc_stretAsesor, m_oFormGenCotizacion)
            _txt.AsignaBinding()

            _txt = New SCG.SBOFramework.UI.EditTextSBO(mc_strtxtAsesor, True, "", mc_strtxtAsesor, m_oFormGenCotizacion)
            _txt.AsignaBinding()

            _txt = New SCG.SBOFramework.UI.EditTextSBO(mc_stretNoAsesor, True, "", mc_stretNoAsesor, m_oFormGenCotizacion)
            _txt.AsignaBinding()

            Call m_oFormGenCotizacion.DataSources.DBDataSources.Add(mc_strOQUT)

            m_dbCotizacion = m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strOQUT)

            oMatrix = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Specific, Matrix)
            oMatrix.SelectionMode = BoMatrixSelect.ms_Auto


            m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretNoAsesor).Value = Utilitarios.ObtieneEmpid(SBO_Application)

            If m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretNoAsesor).Value <> "0" Then
                m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_strtxtAsesor).Value = m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretNoAsesor).Value
                m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretAsesor).Value = Utilitarios.ObtieneEmpname(SBO_Application, m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_strtxtAsesor).Value)
            End If

            Call CargaOrdenesdeVenta(m_oFormGenCotizacion, "T0.DocEntry=-1")

            If EnlazaColumnasMatrixaDatasource(oMatrix) Then

                Call CargarMatrix(oMatrix, m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretNoAsesor).Value, m_oFormGenCotizacion, m_dbCotizacion, False)

                m_oFormGenCotizacion.Visible = True

            End If

            If DMS_Connector.Configuracion.ParamGenAddon.U_UsaDimC.Trim.Equals("Y") Then
                oDataTableDimensionesContablesDMS = m_oFormGenCotizacion.DataSources.DataTables.Add(mc_strDataTableDimensionesOT)
                blnUsaDimensiones = True
            End If

            If Utilitarios.ValidarOTInternaConfiguracion(m_oCompany) Then
                m_blnUsaConfiguracionInternaTaller = True
            Else
                m_blnUsaConfiguracionInternaTaller = False
            End If

            m_oFormGenCotizacion.DataSources.DataTables.Add("dtConsulta")

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

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

    Public Sub ManejadorEventoChooseFromList(ByVal FormUID As String, _
                                             ByRef pVal As ItemEvent, _
                                             ByRef BubbleEvent As Boolean)

        Dim oCFLEvento As IChooseFromListEvent
        Dim sCFL_ID As String
        Dim oCFL As ChooseFromList
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        Try
            m_oFormGenCotizacion = SBO_Application.Forms.Item(pVal.FormUID)

            If Not m_oFormGenCotizacion Is Nothing Then

                oCFLEvento = pVal

                sCFL_ID = oCFLEvento.ChooseFromListUID

                oCFL = m_oFormGenCotizacion.ChooseFromLists.Item(sCFL_ID)

                If oCFLEvento.BeforeAction Then
                    oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                    oCondition = oConditions.Add
                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "Active"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    oCondition.CondVal = "Y"
                    oCondition.BracketCloseNum = 1

                    oCFL.SetConditions(oConditions)

                End If

                If oCFLEvento.ActionSuccess Then
                    Dim oDataTable As DataTable
                    oDataTable = oCFLEvento.SelectedObjects

                    If Not oDataTable Is Nothing Then

                        If pVal.ItemUID = mc_strtxtAsesor Then

                            m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_strtxtAsesor).Value = oDataTable.GetValue("empID", 0)
                            m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretAsesor).Value = oDataTable.GetValue("firstName", 0).ToString() + " " + oDataTable.GetValue("lastName", 0).ToString()
                            m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretNoAsesor).Value = oDataTable.GetValue("empID", 0)

                            Call CargarMatrix(DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix), _
                                              m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretNoAsesor).Value, _
                                              m_oFormGenCotizacion, m_dbCotizacion, False)
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Sub

    Public Function CargarMatrix(ByRef oMatrix As Matrix, _
                                 ByVal empId As String, _
                                 ByVal oform As Form, _
                                 ByVal dbCotizacion As DBDataSource, _
                                 ByVal blnOtdenesInternas As Boolean) As Boolean


        Dim oCondition As Condition
        Dim oConditions As Conditions
        Dim objTiposInternos As List(Of String)
        Dim strTipoOtInterna As String
        Dim intCantidadOrdenesInternas As Integer

        Try

            objTiposInternos = Utilitarios.DevuelveOTsInternas(SBO_Application)

            oConditions = SBO_Application.CreateObject(BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add

            oCondition.BracketOpenNum = 1
            oCondition.Alias = mc_strEmpId
            oCondition.Operation = BoConditionOperation.co_EQUAL
            oCondition.CondVal = empId
            oCondition.BracketCloseNum = 1
            oCondition.Relationship = BoConditionRelationship.cr_AND

            oCondition = oConditions.Add
            oCondition.BracketOpenNum = 1
            oCondition.Alias = mc_strEstadoCotID
            oCondition.Operation = BoConditionOperation.co_EQUAL
            oCondition.CondVal = "4"
            oCondition.BracketCloseNum = 1
            oCondition.Relationship = BoConditionRelationship.cr_AND

            oCondition = oConditions.Add
            oCondition.BracketOpenNum = 1
            oCondition.Alias = mc_strDocStatus
            oCondition.Operation = BoConditionOperation.co_EQUAL
            oCondition.CondVal = "O"
            oCondition.BracketCloseNum = 1

            If objTiposInternos.Count > 0 Then
                oCondition.Relationship = BoConditionRelationship.cr_AND
            End If

            intCantidadOrdenesInternas = 0
            For Each strTipoOtInterna In objTiposInternos

                intCantidadOrdenesInternas += 1

                oCondition = oConditions.Add

                If intCantidadOrdenesInternas = 1 Then
                    oCondition.BracketOpenNum = 2
                Else
                    oCondition.BracketOpenNum = 1
                End If

                oCondition.Alias = mc_strTipoOT

                If Not blnOtdenesInternas Then
                    oCondition.Operation = BoConditionOperation.co_NOT_EQUAL
                Else
                    oCondition.Operation = BoConditionOperation.co_EQUAL
                End If

                oCondition.CondVal = strTipoOtInterna

                If intCantidadOrdenesInternas = objTiposInternos.Count Then
                    oCondition.BracketCloseNum = 2
                Else
                    oCondition.BracketCloseNum = 1
                End If

                If intCantidadOrdenesInternas < objTiposInternos.Count Then

                    If Not blnOtdenesInternas Then
                        oCondition.Relationship = BoConditionRelationship.cr_AND
                    Else
                        oCondition.Relationship = BoConditionRelationship.cr_OR
                    End If

                End If

            Next

            oMatrix.Clear()

            dbCotizacion.Clear()
            dbCotizacion.Query(oConditions)

            oMatrix.LoadFromDataSource()


            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

            Return False
        End Try

    End Function

    Private Function EnlazaColumnasMatrixaDatasource(ByRef oMatrix As SAPbouiCOM.Matrix) As Boolean

        Dim oColumna As SAPbouiCOM.Column

        Try

            oColumna = oMatrix.Columns.Item(mc_strUIDNoCotizacion)
            oColumna.DataBind.SetBound(True, mc_strOQUT, mc_strDocNum)

            oColumna = oMatrix.Columns.Item(mc_strUIDNoOT)
            oColumna.DataBind.SetBound(True, mc_strOQUT, mc_strNumeroOT)

            oColumna = oMatrix.Columns.Item(mc_strUIDEmpid)
            oColumna.DataBind.SetBound(True, mc_strOQUT, mc_strCardCode)

            oColumna = oMatrix.Columns.Item(mc_strUIDNombreEmp)
            oColumna.DataBind.SetBound(True, mc_strOQUT, mc_strCardName)

            oColumna = oMatrix.Columns.Item(mc_strUIDPlaca)
            oColumna.DataBind.SetBound(True, mc_strOQUT, mc_strNum_Placa)

            oColumna = oMatrix.Columns.Item(mc_strUIDMarca)
            oColumna.DataBind.SetBound(True, mc_strOQUT, mc_strDes_Marc)

            oColumna = oMatrix.Columns.Item(mc_strUIDModelo)
            oColumna.DataBind.SetBound(True, mc_strOQUT, mc_strDes_Mode)

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        End Try
    End Function

    Private Function RecorreLineasSeleccionadas(ByVal oMatrix As Matrix, ByRef m_oFormGenCotizacion As Form) As Boolean

        Dim intFilaMatrix As Integer
        Dim strDocEntry As String = ""
        Dim strCondicionOv As String = ""
        Dim chEliminaOR() As Char = {"O", "R", " "}
        Dim blnOrdenVentaGenerada As Boolean
        Dim strSerieOrdenesVenta As String = String.Empty
        Dim idCotizacion As String
        Dim idOT As String

        Try
            If oMatrix.GetNextSelectedRow <> -1 Then
                For intFilaMatrix = 1 To oMatrix.RowCount
                    If oMatrix.IsRowSelected(intFilaMatrix) Then
                        idCotizacion = oMatrix.Columns.Item(1).Cells.Item(intFilaMatrix).Specific.value()
                        idOT = oMatrix.Columns.Item(2).Cells.Item(intFilaMatrix).Specific.value()
                        blnOrdenVentaGenerada = ProcesaCotizacion(oMatrix.Columns.Item(1).Cells.Item(intFilaMatrix).Specific.value, _
                                               m_oCompany, _
                                               strDocEntry, strSerieOrdenesVenta)

                        If Not String.IsNullOrEmpty(strDocEntry) AndAlso strDocEntry <> "-2" Then
                            strCondicionOv &= "T0.DocEntry=" & strDocEntry & " OR "
                        End If
                    End If
                Next
                strCondicionOv = strCondicionOv.TrimEnd(chEliminaOR)
                If Not String.IsNullOrEmpty(strCondicionOv) Then
                    Call CargaOrdenesdeVenta(m_oFormGenCotizacion, strCondicionOv)
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizadoConExito, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                End If

            End If

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        End Try

    End Function

    Private Function ProcesaCotizacion(ByVal NoCotizacion As Integer, _
                                         ByRef oCompany As SAPbobsCOM.Company, _
                                         ByRef strDocEntry As String, _
                                         ByVal p_strSerieOrdenesVenta As String) As Boolean

        Dim oCotizacion As SAPbobsCOM.Documents
        Dim blnOrdenGenerada As Boolean
        Try
            oCotizacion = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
            If oCotizacion.GetByKey(NoCotizacion) Then
                blnOrdenGenerada = CrearOrdenDeVentas(oCotizacion, strDocEntry, p_strSerieOrdenesVenta, NoCotizacion)
            End If
            Return blnOrdenGenerada
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        End Try

    End Function

    Private Sub ActualizarOT(p_strNumeroOT As String, p_strIdSucursal As String, p_blnInterna As Boolean, p_blnTallerSAP As Boolean, Optional ByRef p_strTipoOT As String = "")
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim strNombreTaller As String = ""

        Try
            If p_blnTallerSAP Then
                oCompanyService = m_oCompany.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_OT")
                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("Code", p_strNumeroOT)
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                oGeneralData.SetProperty("U_FCerr", Date.Now)

                If p_blnInterna Then
                    oGeneralData.SetProperty("U_FFact", Date.Now)
                    oGeneralData.SetProperty("U_DEstO", My.Resources.Resource.Facturada)
                    oGeneralData.SetProperty("U_EstO", "7")
                    If Not String.IsNullOrEmpty(p_strTipoOT) Then oGeneralData.SetProperty("U_TipOT", p_strTipoOT)
                Else
                    oGeneralData.SetProperty("U_DEstO", My.Resources.Resource.Cerrada)
                    oGeneralData.SetProperty("U_EstO", "6")
                End If
                oGeneralService.Update(oGeneralData)
            Else
                Utilitarios.DevuelveNombreBDTaller(SBO_Application, p_strIdSucursal, strNombreTaller)
                If p_blnInterna Then
                    Call Utilitarios.EjecutarConsulta(" UPDATE [SCGTA_TB_Orden] SET Estado = 7 WHERE NoOrden = '" & p_strNumeroOT & "' ", strNombreTaller, m_oCompany.Server)
                Else
                    Call Utilitarios.EjecutarConsulta(" UPDATE [SCGTA_TB_Orden] SET Estado = 6 WHERE NoOrden = '" & p_strNumeroOT & "' ", strNombreTaller, m_oCompany.Server)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Function CrearOrdenDeVentas(ByRef oCotizacion As SAPbobsCOM.Documents, _
                                        ByRef strDocEntry As String, _
                                        ByVal p_strSerieOrdenVenta As String, _
                                        ByVal NoCotizacion As Integer) As Boolean

        Dim oOrdendeVenta As SAPbobsCOM.Documents
        Dim intFila As Integer = 0
        Dim interrorCode As Integer
        Dim strError As String = String.Empty
        Dim blnPrimeraLineaOV As Boolean = True
        Dim strOTRefencia As String = My.Resources.Resource.OT_Referencia & oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value
        Dim blnLineasSinAprobar As Boolean = False
        Dim blnHayLineasAprobadas As Boolean = False
        Dim strCentroCostoAsociado As String = ""
        Dim strBodegaProceso As String = ""
        Dim strTipoArticulo As String
        Dim strTipoOrden As String
        Dim strCentroCostoTipoOrden As String
        Dim strCuentaIngresos As String
        Dim strCuentaCostos As String
        Dim strConexionDBSucursal As String = ""
        Dim strCentroBeneficio As String = String.Empty
        Dim blnAgregarDimension As Boolean = False
        Dim objValoresConfiguracionSucursalCotizacion As New ValoresConfiguracionSucursalCotizacion
        Dim strCentroBeneficioConfOT As String = String.Empty
        Dim oItem As SAPbobsCOM.IItems

        Try
            strIdSucursal = oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()

            'hago el llamado para cargar la configuracion de los documentos
            'que usaran Dimensiones
            If blnUsaDimensiones Then
                ListaConfiguracionOT = New List(Of LineasConfiguracionOT)()
                ListaConfiguracionOT = ClsLineasDocumentosDimension.DatatableConfiguracionDocumentosDimensionesOT(m_oFormGenCotizacion)
            End If

            m_oForm = SBO_Application.Forms.ActiveForm

            oItem = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)

            m_blnUsaConfiguracionInternaTaller = Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO)

            If m_blnUsaConfiguracionInternaTaller Then
                If Not CargarValoresConfiguracionPorSucursal(False, oCotizacion.UserFields.Fields.Item(mc_strIdSucursal).Value,
                                                                  oCotizacion.UserFields.Fields.Item(mc_strTipoOT).Value,
                                                                  objValoresConfiguracionSucursalCotizacion) Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.NoExistenConfiguraciones, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                    Exit Try
                End If
            Else

                Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, oCotizacion.UserFields.Fields.Item(mc_strIdSucursal).Value, strConexionDBSucursal)
                Utilitarios.DevuelveNombreBDTaller(SBO_Application, oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value, m_strBDTalller)

                adpConf = New ConfiguracionDataAdapter(strConexionDBSucursal)
                Call adpConf.Fill(dstConf)
                Call adpConf.FillBodegasXCC(dstConfBXCC)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, "IDSerieDocumentosVentas", p_strSerieOrdenVenta)
            End If

            SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoCotización + CStr(oCotizacion.DocNum), BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
            oOrdendeVenta = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)

            If DMS_Connector.Configuracion.ParamGenAddon.U_ValLeaSN.Trim.Equals("Y") Then
                If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_CodLeaSN.Trim) Then
                    If Utilitarios.ValidadSocioNegociosLeasing(oCotizacion.CardCode, m_oCompany, DMS_Connector.Configuracion.ParamGenAddon.U_CodLeaSN.Trim) Then
                        If Not Utilitarios.ValidaInfoLeasing(oCotizacion.UserFields.Fields.Item("U_SCGD_ConEjeBan").Value.ToString.Trim(),
                                                             oCotizacion.UserFields.Fields.Item("U_SCGD_NrOC").Value.ToString.Trim(),
                                                             oCotizacion.UserFields.Fields.Item("U_SCGD_NrOL").Value.ToString.Trim(),
                                                             SBO_Application) Then
                            strDocEntry = -2
                            Return False
                        End If
                    End If
                Else
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.LeasingSNnoConfigurado, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
                    strDocEntry = -2
                    Return False
                End If
            End If

            If m_blnUsaConfiguracionInternaTaller Then
                oOrdendeVenta.Series = objValoresConfiguracionSucursalCotizacion.m_strIDSerieDocOrdenVenta
            Else
                If (Not String.IsNullOrEmpty(p_strSerieOrdenVenta.Trim)) Then
                    oOrdendeVenta.Series = p_strSerieOrdenVenta
                End If
            End If


            '---------------------------------------Manejo de indicadores: 09/05/2012------------------------------------------------
            'Obtiene el indicador por default para el tipo de documento: Orden de venta
            'Orden de Venta [Cliente] [Tipo 9]
            Dim strIndicador As String = Utilitarios.DevuelveCodIndicadores(SBO_Application, "9")

            If (Not String.IsNullOrEmpty(strIndicador)) Then

                oOrdendeVenta.Indicator = strIndicador

            End If

            oOrdendeVenta.DocDueDate = System.DateTime.Now
            oOrdendeVenta.DocRate = oCotizacion.DocRate
            oOrdendeVenta.DocCurrency = oCotizacion.DocCurrency
            oOrdendeVenta.UserFields.Fields.Item(mc_strNum_OT).Value = oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value
            oOrdendeVenta.UserFields.Fields.Item(mc_strTipoOT).Value = oCotizacion.UserFields.Fields.Item(mc_strTipoOT).Value
            oOrdendeVenta.UserFields.Fields.Item(mc_strNumUnidad).Value = oCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value
            oOrdendeVenta.DocumentsOwner = oCotizacion.DocumentsOwner
            'agregado por error con pl 49
            oOrdendeVenta.CardCode = oCotizacion.CardCode
            strTipoOrden = oCotizacion.UserFields.Fields.Item(mc_strTipoOT).Value
            oOrdendeVenta.UserFields.Fields.Item("U_SCGD_ConEjeBan").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_ConEjeBan").Value.ToString.Trim
            oOrdendeVenta.UserFields.Fields.Item("U_SCGD_NrOC").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_NrOC").Value.ToString.Trim
            oOrdendeVenta.UserFields.Fields.Item("U_SCGD_NrOL").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_NrOL").Value.ToString.Trim

            If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                If Not String.IsNullOrEmpty(strIdSucursal) Then
                    oOrdendeVenta.BPL_IDAssignedToInvoice = Integer.Parse(strIdSucursal)
                End If
            End If



            If m_blnUsaConfiguracionInternaTaller Then
                Dim strConsultaCentroBeneficio As String = "SELECT [@SCGD_CENTROSCOSTO].U_Norma " & _
                                                                     "FROM [@SCGD_CONF_TIP_ORDEN]  with (nolock) INNER JOIN " & _
                                                                     "[@SCGD_CENTROSCOSTO]  with (nolock) ON [@SCGD_CONF_TIP_ORDEN].U_CodCtCos = [@SCGD_CENTROSCOSTO].Code RIGHT OUTER JOIN " & _
                                                                     "[@SCGD_CONF_SUCURSAL]  with (nolock) ON [@SCGD_CONF_TIP_ORDEN].DocEntry = [@SCGD_CONF_SUCURSAL].DocEntry " & _
                                                                     "WHERE ([@SCGD_CONF_SUCURSAL].U_Sucurs = '" & strIdSucursal & "') AND ([@SCGD_CONF_TIP_ORDEN].U_Code ='" & strTipoOrden & "')"

                strCentroBeneficioConfOT = Utilitarios.EjecutarConsulta(strConsultaCentroBeneficio, m_oCompany.CompanyDB, m_oCompany.Server)
                strCentroCostoTipoOrden = objValoresConfiguracionSucursalCotizacion.m_strCentroCosto
            Else


                strCentroCostoTipoOrden = Utilitarios.EjecutarConsulta("Select CodCentroCosto from dbo.SCGTA_TB_TipoOrden  with (nolock) where CodTipoOrden = " & strTipoOrden, m_strBDTalller, m_oCompany.Server)
                strCentroBeneficio = ConfiguracionDataAdapter.RetornaCentroBeneficioByTipoOrden(CInt(strTipoOrden), strConexionDBSucursal)

            End If


            If blnUsaDimensiones Then

                Dim strValorDimension As String = ClsLineasDocumentosDimension.ValidacionAsientosDimensiones(ListaConfiguracionOT, strTipoOrden, False, False)
                '******************************************************************************************
                'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                If Not String.IsNullOrEmpty(strValorDimension) Then
                    If strValorDimension = "Y" Then
                        Dim strCodigoMarca As String = oCotizacion.UserFields.Fields.Item(mc_strCod_Marca).Value
                        Dim strCodigoSucursal As String = oCotizacion.UserFields.Fields.Item(mc_strIdSucursal).Value
                        oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContablesOrdenTrabajo(m_oFormGenCotizacion, strCodigoSucursal, strCodigoMarca, oDataTableDimensionesContablesDMS))
                        blnAgregarDimension = True
                    End If
                End If
                '******************************************************************************************
            End If

            For intFila = 0 To oCotizacion.Lines.Count - 1

                Call oCotizacion.Lines.SetCurrentLine(intFila)

                If (oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi AndAlso Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) AndAlso oCotizacion.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Open) Then

                    If Not oItem.GetByKey(oCotizacion.Lines.ItemCode.ToString.Trim) Then
                        Continue For
                    End If

                    SBO_Application.StatusBar.SetText(String.Format(My.Resources.Resource.TXTProcessLineas, (oCotizacion.Lines.LineNum + 1), oCotizacion.Lines.Count), SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    strTipoArticulo = oItem.UserFields.Fields.Item(mc_strTipoArticulo).Value.ToString.Trim()
                    If Not String.IsNullOrEmpty(strTipoArticulo) Then
                        If (strTipoArticulo.Equals("1") OrElse strTipoArticulo.Equals("2") OrElse strTipoArticulo.Equals("3") OrElse strTipoArticulo.Equals("4")) AndAlso _
                            Not ValidarCentroCosto(oCotizacion.Lines.ItemCode, oItem, strIdSucursal) Then
                            Call SBO_Application.StatusBar.SetText(My.Resources.Resource.Val_Centro_Costo & " " & oCotizacion.Lines.ItemDescription, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                            strDocEntry = -2
                            Return False
                        End If
                    End If

                    If (strTipoArticulo <> 10) Then
                        blnHayLineasAprobadas = True
                        If (Not blnPrimeraLineaOV) Then
                            Call oOrdendeVenta.Lines.Add()
                        Else
                            blnPrimeraLineaOV = False
                        End If

                        oOrdendeVenta.Lines.BaseEntry = oCotizacion.DocEntry
                        oOrdendeVenta.Lines.BaseLine = oCotizacion.Lines.LineNum
                        oOrdendeVenta.Lines.BaseType = SAPbobsCOM.BoObjectTypes.oQuotations
                        oOrdendeVenta.Lines.TaxCode = oCotizacion.Lines.TaxCode
                        oOrdendeVenta.Lines.VatGroup = oCotizacion.Lines.VatGroup

                        oOrdendeVenta.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value
                        oOrdendeVenta.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                        oOrdendeVenta.Lines.UserFields.Fields.Item("U_SCGD_CodProy").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value
                        oOrdendeVenta.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = oCotizacion.UserFields.Fields.Item(mc_strTipoOT).Value.ToString
                        'agregado por error con pl 49
                        oOrdendeVenta.Lines.ItemCode = oCotizacion.Lines.ItemCode
                        'proyecto
                        oOrdendeVenta.Lines.ProjectCode = oCotizacion.UserFields.Fields.Item(mc_strProyecto).Value


                        '------------Erick Sanabria 12.07.2013----------------------------------------------------------------------
                        '------------Validación si pasa a Orden de Venta monto de padre del kit o de los articulos hijos------------
                        If (strTipoArticulo = 5) Then
                            strFather = Trim(oCotizacion.Lines.ItemCode)
                            strTreeType = Utilitarios.EjecutarConsulta("Select [TreeType] " & _
                                                                       "From [dbo].[OITT] with (nolock) " & _
                                                                       "Where Code='" & strFather & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                            If (strTreeType = "T") Then
                                strValidaUsaMontoTotalKit = Utilitarios.EjecutarConsulta("Select [U_SCGD_Usa_Padre_KIT] " & _
                                                                                         "From [dbo].[OITT]  with (nolock) " & _
                                                                                         "Where Code='" & strFather & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                If (strValidaUsaMontoTotalKit = "H") Then
                                    oOrdendeVenta.Lines.UnitPrice = 0
                                End If
                            End If
                        Else
                            If (strValidaUsaMontoTotalKit = "P") Then
                                strCodeItem = Trim(oCotizacion.Lines.ItemCode)
                                strChildNum = Utilitarios.EjecutarConsulta("Select [ChildNum] " & _
                                                                           "From [dbo].[ITT1]  with (nolock) Where Code='" & _
                                                                            strCodeItem & "'" & _
                                                                           "And [Father] = '" & strFather & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                If (strChildNum <> "") Then
                                    oOrdendeVenta.Lines.UnitPrice = 0
                                Else
                                    strValidaUsaMontoTotalKit = "N"
                                End If
                            End If
                        End If
                    End If

                    If CInt(strTipoArticulo) <> TiposArt.scgOtrosIngresos AndAlso CInt(strTipoArticulo) <> TiposArt.scgOtrosGastos_Costos AndAlso CInt(strTipoArticulo) <> TiposArt.scgOtros AndAlso CInt(strTipoArticulo) <> TiposArt.scgCita Then
                        Dim strIdSucursal As String = oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString()

                        If Not String.IsNullOrEmpty(strIdSucursal) Then
                            Dim nameDbTaller As String = ""
                            Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, strIdSucursal, nameDbTaller)

                            If Not String.IsNullOrEmpty(nameDbTaller) Then

                                If (String.IsNullOrEmpty(strCentroCostoTipoOrden)) Then
                                    strCentroCostoAsociado = oItem.UserFields.Fields.Item(mc_strCodCentroCosto).Value.ToString.Trim()
                                    'DevuelveValorItem(oCotizacion.Lines.ItemCode, mc_strCodCentroCosto)
                                    strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoAsociado, TransferenciaItems.mc_strBodegaProceso, strIdSucursal, SBO_Application)
                                Else
                                    strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoTipoOrden, TransferenciaItems.mc_strBodegaProceso, strIdSucursal, SBO_Application)
                                End If
                            End If
                        End If

                        If (String.IsNullOrEmpty(strCentroBeneficio)) Then

                            If m_blnUsaConfiguracionInternaTaller Then
                                oOrdendeVenta.Lines.CostingCode = strCentroBeneficioConfOT
                            Else
                                strCentroBeneficioConfOT = ConfiguracionDataAdapter.RetornaCentroBeneficioByItem(oCotizacion.Lines.ItemCode, strConexionDBSucursal)

                                If (Not String.IsNullOrEmpty(strCentroBeneficioConfOT)) Then
                                    oOrdendeVenta.Lines.CostingCode = strCentroBeneficioConfOT
                                End If
                            End If

                            If (Not String.IsNullOrEmpty(strBodegaProceso)) Then
                                oOrdendeVenta.Lines.WarehouseCode = strBodegaProceso
                                strCuentaIngresos = Utilitarios.ObtenerCuentaItem(oCotizacion.Lines.ItemCode, strBodegaProceso, Cuentas.CuentaIngresos, m_oCompany, oItem)
                                strCuentaCostos = Utilitarios.ObtenerCuentaItem(oCotizacion.Lines.ItemCode, strBodegaProceso, Cuentas.CuentaCostos, m_oCompany, oItem)
                                strCuentaIngresos = strCuentaIngresos.Trim
                                strCuentaCostos = strCuentaCostos.Trim()
                                If (Not String.IsNullOrEmpty(strCuentaCostos)) Then
                                    oOrdendeVenta.Lines.COGSAccountCode = strCuentaCostos
                                End If
                                If (Not String.IsNullOrEmpty(strCuentaIngresos)) Then
                                    oOrdendeVenta.Lines.AccountCode = strCuentaIngresos
                                End If
                            Else
                                ''Agregar a recursos
                                Throw New Exception("Imposible determinar bodega de proceso, Item: " & oCotizacion.Lines.ItemCode & " Línea: " & oCotizacion.Lines.LineNum)
                            End If
                        Else
                            oOrdendeVenta.Lines.CostingCode = strCentroBeneficio
                        End If
                    End If

                    If blnAgregarDimension Then
                        If ValidaUsaDimensionOfertaVentas(strIdSucursal, strTipoOrden) Then
                            oOrdendeVenta.Lines.CostingCode = oCotizacion.Lines.CostingCode
                            oOrdendeVenta.Lines.CostingCode2 = oCotizacion.Lines.CostingCode2
                            oOrdendeVenta.Lines.CostingCode3 = oCotizacion.Lines.CostingCode3
                            oOrdendeVenta.Lines.CostingCode4 = oCotizacion.Lines.CostingCode4
                            oOrdendeVenta.Lines.CostingCode5 = oCotizacion.Lines.CostingCode5
                        Else
                            '*****************************************************************************
                            'Agrego dimensiones contables en las lineas de la facturas
                            If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                                ClsLineasDocumentosDimension.AgregarDimensionesLineasDocumentos(oOrdendeVenta.Lines, oDataTableDimensionesContablesDMS)
                            End If
                            '*****************************************************************************
                        End If
                    End If
                Else
                    blnLineasSinAprobar = True
                End If
            Next intFila

            oOrdendeVenta.Comments &= strOTRefencia
            oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = My.Resources.Resource.Cerrada
            oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "6"
            oCotizacion.UserFields.Fields.Item("U_SCGD_FCierre").Value = System.DateTime.Now()

            If blnHayLineasAprobadas Then
                If Not m_oCompany.InTransaction Then
                    m_oCompany.StartTransaction()
                End If

                If oCotizacion.Update() = 0 Then

                    interrorCode = oOrdendeVenta.Add()

                    If interrorCode <> 0 AndAlso interrorCode <> mc_intErrorOperationNoSupported Then
                        If m_oCompany.InTransaction Then
                            m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                        End If
                        Call m_oCompany.GetLastError(interrorCode, strError)
                        Call SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCode & Convert.ToString(interrorCode) + ": " + My.Resources.Resource.LaCotizacionNo & oCotizacion.DocEntry & My.Resources.Resource.NoPudoCrear & strError, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                        Return False
                    Else

                        Call m_oCompany.GetNewObjectCode(strDocEntry)
                        oOrdendeVenta.GetByKey(CInt(strDocEntry))

                        If blnLineasSinAprobar Then
                            oCotizacion.GetByKey(NoCotizacion)
                            If oCotizacion.DocumentStatus <> SAPbobsCOM.BoStatus.bost_Close Then
                                If oCotizacion.Close() = 0 Then
                                    ActualizarOT(oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value, oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value, False, m_blnUsaConfiguracionInternaTaller)
                                    If m_oCompany.InTransaction Then
                                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                                    End If
                                    Call SBO_Application.StatusBar.SetText(My.Resources.Resource.LaCotizacionNo & oCotizacion.DocEntry & My.Resources.Resource.GeneroOV & oOrdendeVenta.DocNum, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)

                                    Return True
                                Else
                                    If m_oCompany.InTransaction Then
                                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                    End If
                                    Call m_oCompany.GetLastError(interrorCode, strError)
                                    Call SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCode & Convert.ToString(interrorCode) + ": " + My.Resources.Resource.LaCotizacionNo & oCotizacion.DocEntry & My.Resources.Resource.NoPudoCrear & strError, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                                    strDocEntry = -2
                                    Return False
                                End If
                            End If
                        Else
                            ActualizarOT(oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value, oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value, False, m_blnUsaConfiguracionInternaTaller)
                            If m_oCompany.InTransaction Then
                                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                            End If
                            Call SBO_Application.StatusBar.SetText(My.Resources.Resource.LaCotizacionNo & oCotizacion.DocEntry & My.Resources.Resource.GeneroOV & oOrdendeVenta.DocNum, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)

                            Return True
                        End If

                    End If
                Else
                    If m_oCompany.InTransaction Then
                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    End If
                    Call m_oCompany.GetLastError(interrorCode, strError)
                    Call SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCode & Convert.ToString(interrorCode) + ": " + My.Resources.Resource.LaCotizacionNo & oCotizacion.DocEntry & My.Resources.Resource.NoPudoCrear & strError, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                    Return False
                End If
            Else
                If SBO_Application.MessageBox(My.Resources.Resource.CotizacionSinLineas, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                    Call oCotizacion.Close()
                    ActualizarOT(oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value, oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value, False, m_blnUsaConfiguracionInternaTaller)
                    If m_oCompany.InTransaction Then
                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                    End If
                Else
                    SBO_Application.MessageBox(My.Resources.Resource.RecuerdeCerrarCotizacion)
                End If
                strDocEntry = -2
                Return False
            End If
        Catch ex As Exception

            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            strDocEntry = -2
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False

        Finally
            Utilitarios.DestruirObjeto(oOrdendeVenta)
            Utilitarios.DestruirObjeto(oItem)
        End Try
    End Function


    'Cambia el estado de la cotizacion a facturado
    Public Shared Sub CambiaEstadoAFacturado(ByVal FormUID As String, _
                                             ByVal m_oCompany As SAPbobsCOM.Company, _
                                             ByVal m_oSBO_Application As Application, p_blnUsaOT_SAP As Boolean, ByRef listBaseEntry As List(Of String()), ByRef blnFactura As Boolean)

        Dim objSAP As SAPbobsCOM.Documents
        Dim objSAPOV As SAPbobsCOM.Documents
        Dim m_oFormFacturaCliente As Form
        Dim idOT As String
        Dim idCotizacion As String
        Dim strQuery As String
        Dim strQuery2 As String
        Dim strNombreTaller As String = String.Empty
        Dim idOTVieja As String = String.Empty
        Dim idOrdenV As String = String.Empty
        Dim idOrdenVVieja As String = "A"
        Dim strIDRepxOrdFac As String
        Dim strIDRepxOrdOV As String
        Dim strIDLineaFAC As String
        Dim strIDLineaOV As String
        Dim strItemCodeFactura As String
        Dim strItemCodeOrdenV As String
        Dim strEstado As String
        Dim dtConsulta As DataTable
        Dim strCardCodeFactura As String = String.Empty
        Dim strCardCodeOrdenVenta As String = String.Empty
        'Dim dblCantidadFacturada As Double = 0
        Dim strCerrarLineasOV As String = String.Empty
        Dim strIDSucursal As String = String.Empty

        Try
            objSAP = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations), SAPbobsCOM.Documents)
            objSAPOV = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders), SAPbobsCOM.Documents)

            m_oFormFacturaCliente = m_oSBO_Application.Forms.Item(FormUID)

            If Utilitarios.ValidaExisteDataTable(m_oFormFacturaCliente, "dtConsulta") Then
                dtConsulta = m_oFormFacturaCliente.DataSources.DataTables.Item("dtConsulta")
            Else
                dtConsulta = m_oFormFacturaCliente.DataSources.DataTables.Add("dtConsulta")
            End If

            dtConsulta.ExecuteQuery(" select Name from [@SCGD_ESTADOS_OT] with (nolock) where code = '7' ")
            strEstado = dtConsulta.GetValue(0, 0).ToString().Trim()

            If m_oFormFacturaCliente IsNot Nothing Then

                'ciclo por cada linea de matriz
                For i As Integer = 0 To m_oFormFacturaCliente.DataSources.DBDataSources.Item("INV1").Size - 1

                    'obtengo la ot referenciada en la matriz
                    idOT = m_oFormFacturaCliente.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_NoOT", i).Trim()
                    strCardCodeFactura = m_oFormFacturaCliente.DataSources.DBDataSources.Item("OINV").GetValue("CardCode", 0).Trim()

                    If Not String.IsNullOrEmpty(idOT) Then

                        'Saco el ID RepxOrden e ItemCode de la Factura -- Se utiliza en la validacion para cerrar la linea
                        strIDRepxOrdFac = m_oFormFacturaCliente.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_IdRepxOrd", i).Trim()
                        strItemCodeFactura = m_oFormFacturaCliente.DataSources.DBDataSources.Item("INV1").GetValue("ItemCode", i).Trim()
                        strIDLineaFAC = m_oFormFacturaCliente.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_ID", i).Trim()
                        'dblCantidadFacturada = m_oFormFacturaCliente.DataSources.DBDataSources.Item("INV1").GetValue("Quantity", i)
                        'Bandera para que solamente haga un select si la OT es la misma
                        If idOT.Trim <> idOTVieja.Trim Then
                            strQuery = String.Format(" SELECT DocEntry FROM OQUT with (nolock) WHERE U_SCGD_Numero_OT = '{0}' ", idOT)
                            idOTVieja = idOT

                            idCotizacion = Utilitarios.EjecutarConsulta(strQuery, m_oCompany.CompanyDB, m_oCompany.Server)

                            If Not String.IsNullOrEmpty(idCotizacion) Then
                                objSAP.GetByKey(idCotizacion)
                            End If

                            If p_blnUsaOT_SAP Then
                                CambiaEstadoFacturadoOT(idOT, strEstado, m_oCompany, m_oSBO_Application)
                            End If

                        End If

                        If objSAP IsNot Nothing Then
                            'verifico el estado de la cotizacion, si esta cerrada la pongo en facturada
                            ' si se encuentra entregada simplemente actualizo la fecha de facturacion 
                            'pero el estado queda igual

                            Select Case objSAP.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value.ToString.Trim()
                                Case "6", "4"
                                    objSAP.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = strEstado
                                    objSAP.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "7"
                                    objSAP.UserFields.Fields.Item("U_SCGD_FFact").Value = DateTime.Now()
                                    objSAP.Update()
                                    'cambia ot del externo en Facturada
                                    If Not p_blnUsaOT_SAP Then
                                        Utilitarios.DevuelveNombreBDTaller(m_oSBO_Application, m_oFormFacturaCliente.DataSources.DBDataSources.Item("OINV").GetValue("U_SCGD_idSucursal", 0).Trim(), strNombreTaller)
                                        Call Utilitarios.EjecutarConsulta(String.Format("UPDATE [SCGTA_TB_Orden] SET Estado = 7 WHERE NoOrden = '{0}'", idOT), strNombreTaller, m_oCompany.Server)
                                    End If

                                Case "8"
                                    objSAP.UserFields.Fields.Item("U_SCGD_FFact").Value = DateTime.Now()
                                    objSAP.Update()
                                    'cambia ot del externo en Entregada
                                    If Not p_blnUsaOT_SAP Then
                                        Utilitarios.DevuelveNombreBDTaller(m_oSBO_Application, m_oFormFacturaCliente.DataSources.DBDataSources.Item("OINV").GetValue("U_SCGD_idSucursal", 0).Trim(), strNombreTaller)
                                        Call Utilitarios.EjecutarConsulta(String.Format("UPDATE [SCGTA_TB_Orden] SET Estado = 8 WHERE NoOrden = '{0}'", idOT), strNombreTaller, m_oCompany.Server)
                                    End If

                            End Select

                            'Validacion para ejecutar una unica vez si la Orden de Ventas es la misma y cargar el objeto
                            If idOT.Trim <> idOrdenVVieja.Trim Then
                                If listBaseEntry.Count = 0 Then
                                    strQuery2 = String.Format("SELECT DocEntry FROM ORDR with (nolock) WHERE U_SCGD_Numero_OT = '{0}'", idOT)
                                    idOrdenV = Utilitarios.EjecutarConsulta(strQuery2, m_oCompany.CompanyDB, m_oCompany.Server)
                                    idOrdenVVieja = idOT
                                Else
                                    For index As Integer = 0 To listBaseEntry.Count - 1
                                        If listBaseEntry.Item(index)(1) = idOT Then
                                            idOrdenV = Utilitarios.EjecutarConsulta(String.Format("SELECT DocEntry FROM ORDR with (nolock) WHERE DocNum = '{0}'", listBaseEntry.Item(index)(0)), m_oCompany.CompanyDB, m_oCompany.Server)
                                            idOrdenVVieja = idOT
                                            Exit For
                                        End If
                                    Next
                                End If

                                If Not String.IsNullOrEmpty(idOrdenV) Then
                                    objSAPOV.GetByKey(idOrdenV)
                                End If
                            End If

                            'Agregar ciclo para repetir varias veces cerrando las líneas de las ordenes de venta donde se encuentra el ID

                            If objSAPOV IsNot Nothing AndAlso objSAPOV.DocumentStatus <> SAPbobsCOM.BoStatus.bost_Close Then
                                strIDSucursal = objSAPOV.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()
                                If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(strIDSucursal)) Then
                                    With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(strIDSucursal))
                                        strCerrarLineasOV = .U_CloseSOL
                                    End With
                                End If
                                For j As Integer = 0 To objSAPOV.Lines.Count - 1
                                    objSAPOV.Lines.SetCurrentLine(j)

                                    'Obtengo el IDRepxOrden o ItemCode
                                    strIDRepxOrdOV = objSAPOV.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString.Trim()
                                    strItemCodeOrdenV = objSAPOV.Lines.UserFields.Fields.Item("ItemCode").Value.ToString.Trim()
                                    strIDLineaOV = objSAPOV.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim()
                                    strCardCodeOrdenVenta = objSAPOV.CardCode

                                    If objSAPOV.Lines.LineStatus <> SAPbobsCOM.BoStatus.bost_Close Then

                                        If Not p_blnUsaOT_SAP Then
                                            If Not String.IsNullOrEmpty(strIDRepxOrdOV) AndAlso Not String.IsNullOrEmpty(strIDRepxOrdFac) Then
                                                If strIDRepxOrdOV = strIDRepxOrdFac Then
                                                    objSAPOV.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close
                                                    Exit For
                                                End If
                                            Else
                                                If strItemCodeFactura = strItemCodeOrdenV Then
                                                    objSAPOV.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close
                                                    Exit For
                                                End If
                                            End If

                                            'Else
                                            '    'Verifica el parámetro de sucursal que indica si se debe cerrar una línea al crear la factura de cliente.
                                            '    If Not strCerrarLineasOV.Equals("N") Then
                                            '        If strCardCodeOrdenVenta <> strCardCodeFactura Then
                                            '            'Agregar parámetro de sucursal para definir si se debe cerrar o no la línea de la orden de venta.
                                            '            If Not String.IsNullOrEmpty(strIDLineaOV) AndAlso Not String.IsNullOrEmpty(strIDLineaFAC) Then
                                            '                If strIDLineaOV = strIDLineaFAC AndAlso dblCantidadFacturada = objSAPOV.Lines.Quantity Then
                                            '                    objSAPOV.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close
                                            '                    Exit For
                                            '                End If
                                            '            Else
                                            '                If strItemCodeFactura = strItemCodeOrdenV Then
                                            '                    objSAPOV.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close
                                            '                    Exit For
                                            '                End If
                                            '            End If
                                            '        End If
                                            '    End If
                                        End If
                                    End If

                                Next
                                objSAPOV.Update()

                            End If

                        End If
                    End If
                Next
                blnFactura = False
                listBaseEntry.Clear()
                Utilitarios.DestruirObjeto(objSAPOV)
                Utilitarios.DestruirObjeto(objSAP)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Shared Sub CambiaEstadoFacturadoOT(ByVal p_strNoOT As String,
                           ByVal p_strEstado As String,
                           ByVal p_oCompany As SAPbobsCOM.Company,
                           ByVal p_oSBO_Application As Application)
        Try

            If Not String.IsNullOrEmpty(p_strNoOT) Then
                Dim oCompanyService As SAPbobsCOM.CompanyService
                Dim oGeneralService As SAPbobsCOM.GeneralService
                Dim oGeneralData As SAPbobsCOM.GeneralData
                Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

                oCompanyService = p_oCompany.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_OT")
                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("Code", p_strNoOT)
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                Dim fhaActual As DateTime
                fhaActual = Utilitarios.RetornaFechaActual(p_oCompany.CompanyDB, p_oCompany.Server)

                oGeneralData.SetProperty("U_FFact", fhaActual)
                oGeneralData.SetProperty("U_DEstO", p_strEstado)
                oGeneralData.SetProperty("U_EstO", "7")

                oGeneralService.Update(oGeneralData)
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, p_oSBO_Application)
        End Try
    End Sub

    Public Sub ManejadorEventoItemPressedGenOV(ByVal FormUID As String, _
                                               ByRef pVal As ItemEvent, _
                                               ByRef BubbleEvent As Boolean)
        Try
            'Dim strIdSucursal As String

            Dim oMatrix As Matrix
            m_oFormGenCotizacion = SBO_Application.Forms.Item(pVal.FormUID)

            If Not m_oFormGenCotizacion Is Nothing _
                AndAlso pVal.ActionSuccess _
                AndAlso pVal.ItemUID = mc_strbtnGenerar Then

                oMatrix = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Specific, Matrix)

                If Not oMatrix Is Nothing Then

                    Call RecorreLineasSeleccionadas(oMatrix, m_oFormGenCotizacion)

                    Call CargarMatrix(DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Specific, Matrix), _
                                      DirectCast(m_oFormGenCotizacion.Items.Item(mc_stretNoAsesor).Specific, EditText).String, _
                                      m_oFormGenCotizacion, _
                                      m_dbCotizacion, False)
                End If

            ElseIf Not m_oFormGenCotizacion Is Nothing _
                    AndAlso pVal.ActionSuccess _
                    AndAlso (pVal.ItemUID = "btnCancel" OrElse pVal.ItemUID = "btClose") Then

                Call m_oFormGenCotizacion.Close()

            ElseIf Not m_oFormGenCotizacion Is Nothing _
            AndAlso pVal.ActionSuccess _
            AndAlso (pVal.ItemUID = "btnAct") Then

                m_dbCotizacion = m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strOQUT)

                oMatrix = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Specific, Matrix)
                Call CargarMatrix(oMatrix, _
                                  DirectCast(m_oFormGenCotizacion.Items.Item(mc_stretNoAsesor).Specific, EditText).String, _
                                  m_oFormGenCotizacion, m_dbCotizacion, False)

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Private Sub CargaOrdenesdeVenta(ByRef oForm As Form, _
                                    ByVal strOrdenesCompraOR As String)


        Dim strOrdenesDeVenta As String = ""
        Dim oGrid As Grid
        Dim oEditTC As EditTextColumn

        Try

            strOrdenesDeVenta = "Select distinct T0.Docentry '" & My.Resources.Resource.CapNoOrdenVenta & "',T2.DocEntry '" & My.Resources.Resource.CapNoCotizacion & "'," & _
                                      "T0.U_SCGD_Numero_OT '" & My.Resources.Resource.CapNoOrdenTrabajo & "',T0.CardCode '" & My.Resources.Resource.CapIDCliente & "'," & _
                                      "T0.Cardname '" & My.Resources.Resource.CapCliente & "',T0.U_SCGD_Num_Placa '" & My.Resources.Resource.CapPlaca & "'," & _
                                      "T0.U_SCGD_Des_Marc '" & My.Resources.Resource.CapMarca & "', T0.U_SCGD_Des_Mode '" & My.Resources.Resource.CapModelo & "'" & _
                                      " From ORDR T0  with (nolock) " & _
                                        " inner join RDR1 T1  with (nolock) " & _
                                            " on T0.DocEntry=T1.DocEntry" & _
                                        " inner join OQUT T2  with (nolock) " & _
                                            " on T2.DocEntry=T1.BaseEntry"

            oGrid = oForm.Items.Item(mc_strGridOV).Specific

            If oForm.DataSources.DataTables.Count < 1 Then
                Call oForm.DataSources.DataTables.Add("OVentas")
            End If

            strOrdenesDeVenta &= " Where " & strOrdenesCompraOR
            Call oForm.DataSources.DataTables.Item("OVentas").ExecuteQuery(strOrdenesDeVenta)
            oGrid.DataTable = oForm.DataSources.DataTables.Item("OVentas")
            oGrid.Columns.Item(0).Width = 80
            oGrid.Columns.Item(1).Width = 80
            oGrid.Columns.Item(2).Width = 80
            oGrid.Columns.Item(3).Width = 80
            oGrid.Columns.Item(4).Width = 120
            oGrid.Columns.Item(5).Width = 80
            oGrid.Columns.Item(6).Width = 80
            oGrid.Columns.Item(7).Width = 80
            oEditTC = oGrid.Columns.Item(0)
            oEditTC.LinkedObjectType = BoLinkedObject.lf_Order
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

        End Try

    End Sub

    Public Function ValidaUsaDimensionOfertaVentas(ByVal p_strIDSucursal As String, ByVal p_strTipoOT As String) As Boolean
        Dim strUsaDimensiones As String = String.Empty
        Dim strUsaDimensionesOFV As String = String.Empty
        Dim intTipoOT As Integer = 0
        Try
            If Not String.IsNullOrEmpty(p_strIDSucursal) And Not String.IsNullOrEmpty(p_strTipoOT) Then
                intTipoOT = Convert.ToInt32(p_strTipoOT)
                If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_strIDSucursal)) Then
                    With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_strIDSucursal))
                        If .Configuracion_Tipo_Orden.Any(Function(tipoOT) tipoOT.U_Code.Equals(intTipoOT)) Then
                            If Not String.IsNullOrEmpty(.Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(intTipoOT)).U_UsaDim) Then strUsaDimensiones = .Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(intTipoOT)).U_UsaDim
                            If Not String.IsNullOrEmpty(strUsaDimensiones) Then
                                If strUsaDimensiones = "Y" Then
                                    If Not String.IsNullOrEmpty(.Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(intTipoOT)).U_UsaDOFV) Then strUsaDimensionesOFV = .Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(intTipoOT)).U_UsaDOFV
                                    If Not String.IsNullOrEmpty(strUsaDimensionesOFV) Then
                                        If strUsaDimensionesOFV = "Y" Then
                                            Return True
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End With
                End If
            End If

            Return False
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function
#End Region

#End Region

End Class
