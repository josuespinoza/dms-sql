Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.DMSOne.Framework
Imports SCG.DMSOne.Framework.MenuManager
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports DMS_Connector.Business_Logic.DataContract.Configuracion.Configuracion_Sucursal
Imports DMS_Connector.Business_Logic.DataContract.SAPDocumento
Imports Microsoft.Office.Interop.Excel

Partial Class CotizacionCLS

#Region "GeneraOrdenesDeVenta"

#Region "Declaraciones"

    Private Const mc_strUIDSubGeneraFI As String = "SCGD_GFI"
    Public n As NumberFormatInfo

    Private strSCGD_TipoArticulo As String = "U_SCGD_TipoArticulo"
    Private mc_strSCGD_NoOT As String = "U_SCGD_NoOT"

    Private Structure ItemsSalida
        Dim strCodigoItem As String
        Dim strCodigoAlmacen As String
        Dim intCantidad As Decimal
        Dim intLineNum As Integer
        Dim intCentroCosto As Integer
        Dim Costo As Double

    End Structure

    'para unir Repuestos con suministros y crear el documento de salida
    Private Structure ItemsRepuestosSuministrosSalida
        Dim strCodigoItem As String
        Dim strCodigoAlmacen As String
        Dim intCantidad As Decimal
        Dim intLineNum As Integer
        Dim intLineNumOriginal As Integer

    End Structure

    Private Structure ItemsAsiento
        Dim strCodigoItem As String
        Dim strCodigoCuentaExistencias As String
        Dim decMonto As Decimal


    End Structure

    'estructura para asiento de Servicio Externo
    Private Structure ItemAsientoSE
        Dim strCuenta As String
        Dim dcTotal As Decimal
        Dim dcTotalFrg As Decimal

    End Structure

    Public Const mc_strCodUnidad As String = "U_SCGD_Cod_Unidad"
    Public Const mc_strTransaccion As String = "U_SCGD_Cod_Tran"

    Public Const mc_strUTipoTransferencia As String = "U_SCGD_TipoTransf"

    Public blnAgregarDimension As Boolean = False

    Private Enum TiposTiempo
        Estandar = 1
        Real = 2
        PrecioCotizacion = 3
        Ninguno = 0
    End Enum


    Private m_blnServicosExternosInventariablesFI As Boolean = False
    'Private blnConfiguracionTallerInterno As Boolean = False



#End Region
#Region "Enum"
    Private Enum TipoArticulo
        Repuesto = 1
        Servicio = 2
        Suministro = 3
        ServicioExterno = 4
        Paquete = 5
        Otros = 6
        Accesorio = 7
        Vehiculo = 8
        Tramite = 9
        ArticuloCita = 10
        OtrosCostos = 11
        OtrosIngresos = 12
    End Enum
#End Region
#Region "Metodos"

    Protected Friend Sub AddMenuItemsFI()

        Dim strEtiquetaMenu As String


        If Utilitarios.MostrarMenu("SCGD_GFI", SBO_Application.Company.UserName) Then
            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_GFI", SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDSubGeneraFI, SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 10, False, True, mc_strUIDGeneraOV))
        End If

    End Sub

    Protected Friend Sub CargaFormularioGeneraFI()

        Dim fcp As SAPbouiCOM.FormCreationParams
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oEdit As SAPbouiCOM.EditText
        Dim strXMLACargar As String

        Try
            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_GeneraFI"

            strXMLACargar = My.Resources.Resource.CotizacionFI
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

            oMatrix = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix)
            oMatrix.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Auto


            m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretNoAsesor).Value = Utilitarios.ObtieneEmpid(SBO_Application)

            If m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretNoAsesor).Value <> "0" Then
                m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_strtxtAsesor).Value = m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretNoAsesor).Value
                m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretAsesor).Value = Utilitarios.ObtieneEmpname(SBO_Application, m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_strtxtAsesor).Value)
            End If

            Call CargaFacturasInternas(m_oFormGenCotizacion, "DocEntry=-1")

            If EnlazaColumnasMatrixaDatasource(oMatrix) Then

                Call CargarMatrix(oMatrix, m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretNoAsesor).Value, m_oFormGenCotizacion, m_dbCotizacion, True)

                m_oFormGenCotizacion.Visible = True

            End If

            If DMS_Connector.Configuracion.ParamGenAddon.U_UsaDimC.Trim.Equals("Y") Then
                oDataTableDimensionesContablesDMS = m_oFormGenCotizacion.DataSources.DataTables.Add(mc_strDataTableDimensionesOT)
                blnUsaDimensiones = True
            End If

            Call m_oFormGenCotizacion.DataSources.DataTables.Add("dtConsulta")

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    <CLSCompliant(False)> _
    Public Sub ManejadorEventoChooseFromListFI(ByVal FormUID As String, _
                                             ByRef pVal As SAPbouiCOM.ItemEvent, _
                                             ByRef BubbleEvent As Boolean)

        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim sCFL_ID As String
        Dim oCFL As SAPbouiCOM.ChooseFromList
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
                    Dim oDataTable As SAPbouiCOM.DataTable
                    oDataTable = oCFLEvento.SelectedObjects

                    If Not oDataTable Is Nothing Then

                        If pVal.ItemUID = mc_strtxtAsesor Then

                            m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_strtxtAsesor).Value = oDataTable.GetValue("empID", 0)
                            m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretAsesor).Value = oDataTable.GetValue("firstName", 0).ToString() + " " + oDataTable.GetValue("lastName", 0).ToString()
                            m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretNoAsesor).Value = oDataTable.GetValue("empID", 0)

                            Call CargarMatrix(DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix), _
                                              m_oFormGenCotizacion.DataSources.UserDataSources.Item(mc_stretNoAsesor).Value, _
                                              m_oFormGenCotizacion, m_dbCotizacion, True)

                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub


    Private Function RecorreLineasSeleccionadasFI(ByVal oMatrix As Matrix, ByVal p_oFormGenCotizacion As Form) As Boolean

        Dim intFilaMatrix As Integer
        Dim strDocEntry As String = ""
        Dim strCondicionOv As String = ""
        Dim chEliminaOR() As Char = {"O", "R", " "}
        Dim blnOrdenVentaGenerada As Boolean

        Try

            If oMatrix.GetNextSelectedRow <> -1 Then


                For intFilaMatrix = 1 To oMatrix.RowCount

                    If oMatrix.IsRowSelected(intFilaMatrix) Then

                        blnOrdenVentaGenerada = ProcesaCotizacionFI(oMatrix.Columns.Item(1).Cells.Item(intFilaMatrix).Specific.value, m_oCompany, strDocEntry)

                        If Not String.IsNullOrEmpty(strDocEntry) AndAlso strDocEntry <> "-2" Then
                            strCondicionOv &= "DocEntry=" & strDocEntry & " OR "
                        End If

                    End If

                Next intFilaMatrix

                strCondicionOv = strCondicionOv.TrimEnd(chEliminaOR)

                If Not String.IsNullOrEmpty(strCondicionOv) Then

                    Call CargaFacturasInternas(m_oFormGenCotizacion, strCondicionOv)
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizadoConExito, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success)

                End If

            End If

            Return True
        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        End Try

    End Function

    Private Function ProcesaCotizacionFI(ByVal NoCotizacion As Integer, ByVal oCompany As SAPbobsCOM.Company, ByRef strDocEntry As String) As Boolean
        Dim oCotizacion As SAPbobsCOM.Documents
        Dim blnOrdenGenerada As Boolean
        Dim strSucursalCot As String
        Try
            If NoCotizacion > 0 Then
                blnOrdenGenerada = CrearFacturasInternasNUEVO(NoCotizacion, strDocEntry)
            End If
            Return blnOrdenGenerada
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        End Try

    End Function

    Public Sub CargaConfiguracionCentrosCosto(ByRef p_strIDSucursal As String, _
                                              ByRef p_oConfiguracionSucursalList As ConfiguracionSucursal_List)
        Try
            '********Declaración de data contract*************
            Dim oConfiguracionSucursal As ConfiguracionSucursal
            '********Declaración de variables*****************
            Dim oDataTableConfiguracionSucursal As System.Data.DataTable = Nothing
            Dim oDataRowConfiguracionSucursal As System.Data.DataRow
            Dim blnUsaAsientoServicioExterno As Boolean = False
            Dim intContSucursalList As Integer = 0
            Dim intContTemporal As Integer = 0
            '******************************************************************************
            '******************** Carga Configuración de tabla ConfiguracionSucursal*******
            '******************************************************************************
            oDataTableConfiguracionSucursal = Utilitarios.EjecutarConsultaDataTable(String.Format("Select U_GenAsSE, S.U_Sucurs,U_CC, U_Pro From [@SCGD_CONF_SUCURSAL] as S with (nolock) inner join [@SCGD_CONF_BODXCC] as CC with (nolock) on S.DocEntry=CC.DocEntry , dbo.[@SCGD_ADMIN] with (nolock)  Where U_Sucurs =  '{0}'",
                                                       p_strIDSucursal),
                                                       m_oCompany.CompanyDB,
                                                       m_oCompany.Server)
            '******************************************************************************
            '******************** Recorre configuraciones y agrega a objeto list*******
            '******************************************************************************
            For Each oDataRowConfiguracionSucursal In oDataTableConfiguracionSucursal.Rows
                blnUsaAsientoServicioExterno = False
                oConfiguracionSucursal = New ConfiguracionSucursal()
                With oConfiguracionSucursal
                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_Sucurs")) Then
                        .SucursalID = oDataRowConfiguracionSucursal.Item("U_Sucurs").ToString.Trim()
                    End If
                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_CC")) Then
                        .CentroCosto = oDataRowConfiguracionSucursal.Item("U_CC").ToString.Trim()
                    End If
                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_Pro")) Then
                        .BodegaProceso = oDataRowConfiguracionSucursal.Item("U_Pro").ToString.Trim()
                    End If
                    '*********************************************************************
                    '**************Valida si genera asientos servicio externo*************
                    '*********************************************************************
                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_GenAsSE")) Then
                        If oDataRowConfiguracionSucursal.Item("U_GenAsSE").ToString.Trim() = "Y" Then
                            .UsaAsientoServicioExterno = True
                        Else
                            .UsaAsientoServicioExterno = False
                        End If
                    Else
                        .UsaAsientoServicioExterno = False
                    End If
                End With
                p_oConfiguracionSucursalList.Add(oConfiguracionSucursal)
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub AsignaAlmacenProceso(ByRef p_strCentroCosto As String, _
                                    ByRef p_oConfiguracionSucursalList As ConfiguracionSucursal_List, _
                                    ByRef p_strAlmacenProceso As String)
        Try
            For Each row As ConfiguracionSucursal In p_oConfiguracionSucursalList
                If p_strCentroCosto = row.CentroCosto Then
                    p_strAlmacenProceso = row.BodegaProceso
                    Exit For
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function CrearFacturasInternas(ByVal oCotizacion As SAPbobsCOM.Documents, ByRef strDocEntry As String, Optional ByVal p_strDECot As String = "", _
                                        Optional ByVal p_strDNCot As String = "", Optional ByVal p_strIdSucursal As String = "", _
                                        Optional ByVal p_blnUsaDimensiones As Boolean = False, Optional ByVal p_form As SAPbouiCOM.Form = Nothing, _
                                        Optional ByRef p_Matrix As SAPbouiCOM.Matrix = Nothing, Optional ByVal strTipoOT As String = Nothing) As Boolean

        Dim oSalidaMercancia As SAPbobsCOM.Documents
        Dim oAsientoContable As SAPbobsCOM.JournalEntries
        Dim oItem As SAPbobsCOM.Items

        Dim oConfiguracionSucursalList As ConfiguracionSucursal_List = New ConfiguracionSucursal_List
        Dim strAlmacenProceso As String
        'asiento para servicios externos
        Dim oAsientoServExt As SAPbobsCOM.JournalEntries
        Dim strCuentaGastosSE As String
        Dim strCuentaTipoOrden As String
        Dim decTotalSE As Decimal
        Dim decTotalFrgSE As Decimal
        Dim dcTotalAcumulado_JE As Decimal
        Dim blnLineasAgregadasSE As Boolean
        'Manejo de items para asientos de servicios externos
        Dim objItemAsientoSE As New ItemAsientoSE
        Dim objItemsAsientoSE As New Generic.List(Of ItemAsientoSE)
        Dim strIdAsientoSE As New Integer
        Dim strMemo As String
        Dim idLinea As EditText
        Dim strNombreBDTaller As String = ""
        Dim intFila As Integer = 0
        Dim strError As String = ""
        Dim blnLineasSinAprobar As Boolean = False
        Dim blnHayLineasAprobadas As Boolean = False
        Dim strCentroCostoAsociado As String = ""
        Dim strBodegaProceso As String = ""
        Dim objItemsSalida As New List(Of ItemsSalida)
        Dim objItemsAsiento As New List(Of ItemsAsiento)
        Dim objItemSalida As New ItemsSalida
        Dim objItemAsiento As New ItemsAsiento

        Dim tadFactura As New FacturaInternaDatasetTableAdapters.SCG_FACTURAINTERNATableAdapter
        Dim tadFacturaSerie As New FacturaInternaDatasetTableAdapters.SCG_FACTURAINTERNATableAdapter
        Dim udoFacturaInterna As UDOFacturaInterna
        Dim udoEncFactura As EncabezadoUDOFacturaInterna
        Dim cnConeccionBD As SqlClient.SqlConnection
        Dim strConectionString As String = ""

        Dim strContraCuenta As String
        Dim strTipoArticulo As String
        Dim strTipoOrden As String

        Dim blnIncluirEnAsiento As Boolean
        Dim blnIcluirEnSalida As Boolean
        Dim blnIcluirAsientoServicios As Boolean = True
        Dim blnLineasAgregadas As Boolean
        Dim strIdSalida As String = ""
        Dim strIdAsiento As String = ""
        Dim intError As Integer
        Dim decMonto As Decimal
        Dim decMontoAsiento As Decimal
        Dim strMoneda As String
        Dim strServicosExternosInventariables As String = ""
        Dim strTipoCosto As String
        Dim objItemWharehouse As SAPbobsCOM.ItemWarehouseInfo
        Dim strCentroCostoTipoOrden As String
        Dim strTransaccionLineas As String
        Dim CostosSExFP As String
        Dim strCostoML As String
        Dim strNoOT As String
        Dim strNoArticulo As String
        Dim strConexionDBSucursal As String = String.Empty
        Dim strCentroBeneficio As String
        Dim strTipoMoneda As String
        Dim decTotalMonto As Decimal = 0
        Dim lsItemsSE As IList = New List(Of String)
        Dim intEtiqueta As Integer
        Dim dtTipoCostoAsiento As Data.DataTable
        Dim strTipoCostoPorSucursal As String = ""
        Dim strCentroBeneficioConfOT As String = String.Empty
        Dim objValoresConfiguracionSucursal As New ValoresConfiguracionSucursalCotizacion
        Dim blnUsaAsientoSE As Boolean = False
        Dim m_oCotizacion As SAPbobsCOM.Documents
        Dim dblCostoManoObra As Double = 0
        Dim blnContinuaProceso As Boolean = False
        Try

            oItem = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            m_oCotizacion = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations), SAPbobsCOM.Documents)

            m_blnUsaConfiguracionInternaTaller = Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO)
            If m_blnUsaConfiguracionInternaTaller Then oDataTableConfiguracionesSucursal = New Data.DataTable

            udoFacturaInterna = New UDOFacturaInterna(m_oCompany)
            udoEncFactura = New EncabezadoUDOFacturaInterna()

            'hago el llamado para cargar la configuracion de los documentos
            'que usaran Dimensiones
            If p_blnUsaDimensiones Then
                ListaConfiguracionOT = New List(Of LineasConfiguracionOT)()
                ListaConfiguracionOT = ClsLineasDocumentosDimension.DatatableConfiguracionDocumentosDimensionesOT(p_form)
            End If

            If String.IsNullOrEmpty(p_strIdSucursal) Then
                p_strIdSucursal = oCotizacion.UserFields.Fields.Item(mc_strIdSucursal).Value.ToString.Trim()
            End If

            If Not strTipoOT Is Nothing Then
                strTipoOrden = strTipoOT
            Else
                If p_Matrix Is Nothing Then
                    strTipoOrden = oCotizacion.UserFields.Fields.Item(mc_strTipoOT).Value
                Else
                    strTipoOrden = strTipoOT
                End If
            End If


            If m_blnUsaConfiguracionInternaTaller Then

                If CargarValoresConfiguracionPorSucursal(True, p_strIdSucursal, strTipoOrden, objValoresConfiguracionSucursal) Then
                    strTipoMoneda = objValoresConfiguracionSucursal.m_strTipoMoneda
                    Utilitarios.DevuelveNombreBDTaller(SBO_Application, p_strIdSucursal, m_strBDTalller)
                Else
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.NoExistenConfiguraciones, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                    Exit Try
                End If
            Else
                Utilitarios.DevuelveNombreBDTaller(SBO_Application, p_strIdSucursal, m_strBDTalller)
                Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, p_strIdSucursal, strConexionDBSucursal)
                strTipoMoneda = Utilitarios.EjecutarConsulta(String.Format("Select Valor From SCGTA_TB_Configuracion with (nolock) where Propiedad = 'TipoMoneda'"), m_strBDTalller, m_oCompany.Server)
            End If

            If strTipoMoneda <> "" Then
                If ValidarTipoCambioMoneda(strTipoMoneda, m_blnUsaConfiguracionInternaTaller) = True Then

                    If String.IsNullOrEmpty(p_strIdSucursal) Then
                        p_strIdSucursal = oCotizacion.UserFields.Fields.Item(mc_strIdSucursal).Value.ToString.Trim()
                    End If

                    If Not String.IsNullOrEmpty(p_strDNCot) Then
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoCotización + CStr(p_strDNCot), SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoCotización + CStr(oCotizacion.DocNum), SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End If

                    If m_blnUsaConfiguracionInternaTaller Then
                        strContraCuenta = objValoresConfiguracionSucursal.m_strCuentaTipoOrdenInternaConfiSucursal
                        strTipoCostoPorSucursal = objValoresConfiguracionSucursal.m_strTipoCostoPorSucursal
                    Else
                        strContraCuenta = Utilitarios.EjecutarConsulta("Select Numero_Cuenta_Contable from SCGTA_TB_Conf_Ot_Iterna with (nolock) where ID_Tipo_Ot = " & strTipoOrden, m_strBDTalller, m_oCompany.Server)
                    End If

                    If Not String.IsNullOrEmpty(strContraCuenta) Then

                        oSalidaMercancia = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)
                        oAsientoContable = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                        'asiento para servicios externos
                        oAsientoServExt = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                        strMoneda = oCotizacion.DocCurrency

                        If m_blnUsaConfiguracionInternaTaller Then
                            strTipoCosto = strTipoCostoPorSucursal
                        Else
                            strTipoCosto = Utilitarios.EjecutarConsulta("Select Valor from SCGTA_TB_Configuracion with (nolock) where Propiedad = 'TipoCosto'", m_strBDTalller, m_oCompany.Server)
                            strServicosExternosInventariables = Utilitarios.EjecutarConsulta("Select Valor from SCGTA_TB_Configuracion with (nolock) where Propiedad = 'SEInventariables'", m_strBDTalller, m_oCompany.Server)
                        End If

                        dtTipoCostoAsiento = Utilitarios.EjecutarConsultaDataTable(String.Format("SELECT U_TiempoOFV_C, U_TiempoEst_C, U_TiempoReal_C FROM dbo.[@SCGD_CONF_SUCURSAL] with (nolock) WHERE U_Sucurs = {0} ", p_strIdSucursal), m_oCompany.CompanyDB, m_oCompany.Server)

                        If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(x) x.U_Sucurs = p_strIdSucursal).U_CosteoMO_I = "Y" Then
                            For Each dr As DataRow In dtTipoCostoAsiento.Rows
                                If Not String.IsNullOrEmpty(dr("U_TiempoEst_C").ToString().Trim()) AndAlso dr.Item("U_TiempoEst_C").ToString().Trim() = "Y" Then
                                    intEtiqueta = TiposTiempo.Estandar
                                    Exit For
                                ElseIf Not String.IsNullOrEmpty(dr("U_TiempoReal_C").ToString().Trim()) AndAlso dr.Item("U_TiempoReal_C").ToString().Trim() = "Y" Then
                                    intEtiqueta = TiposTiempo.Real
                                    Exit For
                                ElseIf Not String.IsNullOrEmpty(dr("U_TiempoOFV_C").ToString().Trim()) AndAlso dr.Item("U_TiempoOFV_C").ToString().Trim() = "Y" Then
                                    intEtiqueta = TiposTiempo.PrecioCotizacion
                                    Exit For
                                Else
                                    intEtiqueta = TiposTiempo.Ninguno
                                    Exit For
                                End If
                            Next
                        Else
                            intEtiqueta = TiposTiempo.Ninguno
                        End If

                        If String.IsNullOrEmpty(strServicosExternosInventariables) Then
                            strServicosExternosInventariables = 0
                        End If
                        'strTipoOrden = oCotizacion.UserFields.Fields.Item(mc_strTipoOT).Value

                        If m_blnUsaConfiguracionInternaTaller Then
                            Dim dtConfiguracionOTInterna As System.Data.DataTable
                            Dim drConfiguracionOTInternaRow As System.Data.DataRow
                            Dim strConsultaCentroBeneficio As String = "SELECT [@SCGD_CENTROSCOSTO].U_Norma " & _
                                                                       "FROM [@SCGD_CONF_TIP_ORDEN] INNER JOIN " & _
                                                                       "[@SCGD_CENTROSCOSTO] ON [@SCGD_CONF_TIP_ORDEN].U_CodCtCos = [@SCGD_CENTROSCOSTO].Code RIGHT OUTER JOIN " & _
                                                                       "[@SCGD_CONF_SUCURSAL] ON [@SCGD_CONF_TIP_ORDEN].DocEntry = [@SCGD_CONF_SUCURSAL].DocEntry " & _
                                                                       "WHERE ([@SCGD_CONF_SUCURSAL].U_Sucurs = '" & p_strIdSucursal & "') AND ([@SCGD_CONF_TIP_ORDEN].U_Code ='" & strTipoOrden & "')"

                            strCentroBeneficio = Utilitarios.EjecutarConsulta(strConsultaCentroBeneficio, m_oCompany.CompanyDB, m_oCompany.Server)
                            strTransaccionLineas = objValoresConfiguracionSucursal.m_strTransaccionLineas
                            strCentroCostoTipoOrden = objValoresConfiguracionSucursal.m_strCentroCosto
                        Else
                            strTransaccionLineas = Utilitarios.DevuelveTransaccionFacturaInterna(strTipoOrden, m_strBDTalller, m_oCompany.Server)
                            strCentroCostoTipoOrden = Utilitarios.EjecutarConsulta("Select CodCentroCosto from dbo.SCGTA_TB_TipoOrden with (nolock) where CodTipoOrden = " & strTipoOrden, m_strBDTalller, m_oCompany.Server)
                            strCentroBeneficio = ConfiguracionDataAdapter.RetornaCentroBeneficioByTipoOrden(CInt(strTipoOrden), strConexionDBSucursal)
                        End If

                        'se obtiene la configuracion para la obtencion de costo
                        CostosSExFP = Utilitarios.EjecutarConsulta("SELECT U_CostSExFP FROM [@SCGD_ADMIN] with (nolock)",
                                                           m_oCompany.CompanyDB,
                                                           m_oCompany.Server)

                        strNoOT = oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value

                        If p_blnUsaDimensiones Then
                            Dim strValorDimension As String = ClsLineasDocumentosDimension.ValidacionAsientosDimensiones(ListaConfiguracionOT, strTipoOrden, False, False)
                            '******************************************************************************************
                            'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                            If Not String.IsNullOrEmpty(strValorDimension) Then
                                If strValorDimension = "Y" Then
                                    Dim strCodigoMarca As String = oCotizacion.UserFields.Fields.Item(mc_strCod_Marca).Value
                                    Dim strCodigoSucursal As String = oCotizacion.UserFields.Fields.Item(mc_strIdSucursal).Value
                                    oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContablesOrdenTrabajo(p_form, strCodigoSucursal, strCodigoMarca, oDataTableDimensionesContablesDMS))

                                    If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                                        blnAgregarDimension = True
                                    End If

                                End If
                            End If
                            '******************************************************************************************
                        End If

                        '********************************Carga Almacenes por centro de costo***************************
                        If Not String.IsNullOrEmpty(p_strIdSucursal) Then
                            CargaConfiguracionCentrosCosto(p_strIdSucursal, oConfiguracionSucursalList)
                            blnUsaAsientoSE = oConfiguracionSucursalList.Item(0).UsaAsientoServicioExterno
                        End If
                        '**********************************************************************************************

                        For intFila = 0 To oCotizacion.Lines.Count - 1
                            Call oCotizacion.Lines.SetCurrentLine(intFila)

                            If Not oItem.GetByKey(oCotizacion.Lines.ItemCode) Then
                                Continue For
                            End If

                            If p_Matrix Is Nothing Then
                                If oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi Then

                                    blnHayLineasAprobadas = True
                                    blnIncluirEnAsiento = False
                                    blnIcluirEnSalida = False
                                    decTotalMonto += oCotizacion.Lines.LineTotal
                                    strTipoArticulo = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value
                                    strNoArticulo = oCotizacion.Lines.ItemCode

                                    Select Case strTipoArticulo
                                        Case TiposArticulos.scgActividad
                                            If intEtiqueta <> 0 Then
                                                blnIncluirEnAsiento = True
                                            Else
                                                blnIncluirEnAsiento = False
                                            End If
                                            blnIcluirEnSalida = False
                                        Case TiposArticulos.scgRepuesto
                                            blnIncluirEnAsiento = False
                                            blnIcluirEnSalida = True
                                        Case TiposArticulos.scgServicioExt
                                            If Not lsItemsSE.Contains(strNoArticulo) Then

                                                lsItemsSE.Add(strNoArticulo)
                                                ' Valido Cuenta Contable por Centro de costo 
                                                strAlmacenProceso = String.Empty
                                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value) Then
                                                    AsignaAlmacenProceso(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString.Trim(), _
                                                                         oConfiguracionSucursalList, strAlmacenProceso)
                                                    If DMS_Connector.Company.AdminInfo.EnableAdvancedGLAccountDetermination = SAPbobsCOM.BoYesNoEnum.tYES Then
                                                        strCuentaGastosSE = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.ExpensesAc, p_strIdSucursal, strAlmacenProceso)
                                                    Else
                                                        strCuentaGastosSE = Utilitarios.EjecutarConsulta(String.Format("SELECT ExpensesAc FROM OWHS with (nolock) WHERE WhsCode = '{0}'",
                                                                                                                          strAlmacenProceso),
                                                                                                                      m_oCompany.CompanyDB,
                                                                                                                      m_oCompany.Server)
                                                    End If

                                                End If
                                                strCostoML = ""
                                                'La busqueda de los costos para la generacion del asiento por servicios Externos se hace por el campo U_SCGD_NoOT de las
                                                'lineas de detalle del documento

                                                'Servicio Externo por Factura Provedor
                                                'If CostosSExFP = "Y" Then
                                                '    strCostoML = Utilitarios.EjecutarConsulta(String.Format("SELECT SUM(P1.LineTotal) FROM OPCH AS P with (nolock) INNER JOIN PCH1 AS P1 with (nolock) ON P.DocEntry = P1.DocEntry  WHERE P1.U_SCGD_NoOT = '{0}' AND P1.ItemCode = '{1}' and (P1.TrgetEntry is not null or p1.TrgetEntry is null) GROUP BY P1.ItemCode",
                                                '        strNoOT, strNoArticulo), m_oCompany.CompanyDB, m_oCompany.Server)
                                                '    decTotalSE = 0

                                                '    'Servicio Externo por Entrada Mercancia
                                                'ElseIf CostosSExFP = "N" Then
                                                '    strCostoML = Utilitarios.EjecutarConsulta(String.Format("SELECT SUM(P1.LineTotal) FROM OPDN AS P with (nolock) INNER JOIN PDN1 AS P1 with (nolock) ON P.DocEntry = P1.DocEntry  WHERE P1.U_SCGD_NoOT = '{0}' AND P1.ItemCode = '{1}' and P1.TargetType <> 21 GROUP BY P1.ItemCode",
                                                '        strNoOT, strNoArticulo), m_oCompany.CompanyDB, m_oCompany.Server)
                                                '    decTotalSE = 0
                                                'End If

                                                'If Not String.IsNullOrEmpty(strCostoML) Then decTotalSE = Decimal.Parse(strCostoML)

                                                decTotalSE = Decimal.Parse(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value)

                                                decTotalFrgSE = 0

                                                If m_blnUsaConfiguracionInternaTaller Then

                                                    strCuentaTipoOrden = objValoresConfiguracionSucursal.m_strCuentaTipoOrdenInternaConfiSucursal
                                                Else
                                                    'obtengo el nombre de BD de taller 
                                                    Utilitarios.DevuelveNombreBDTaller(SBO_Application, p_strIdSucursal, strNombreBDTaller)

                                                    strCuentaTipoOrden = Utilitarios.EjecutarConsulta(
                                                        String.Format(
                                                            "select numero_cuenta_contable from {0}.dbo.SCGTA_TB_Conf_Ot_Iterna with (nolock) where ID_Tipo_Ot = '{1}'",
                                                            strNombreBDTaller,
                                                            strTipoOrden),
                                                        m_oCompany.CompanyDB,
                                                        m_oCompany.Server)
                                                End If
                                                If decTotalSE > 0 OrElse decTotalFrgSE > 0 Then

                                                    'retorna el obj item para una linea del asiento de servicios externos
                                                    objItemAsientoSE = AgregaServExterno(strCuentaTipoOrden, decTotalSE, decTotalFrgSE)

                                                    'cargo los items de los asientos de servicios externos a 
                                                    'la lista con las lineas del asiento
                                                    objItemsAsientoSE.Add(objItemAsientoSE)

                                                End If
                                            End If
                                        Case TiposArticulos.scgSuministro
                                            blnIncluirEnAsiento = False
                                            blnIcluirEnSalida = True
                                    End Select

                                    If blnIncluirEnAsiento Then
                                        If Not String.IsNullOrEmpty(p_strIdSucursal) Then
                                            If m_blnUsaConfiguracionInternaTaller Then
                                                If String.IsNullOrEmpty(strCentroCostoTipoOrden) Then
                                                    strCentroCostoAsociado = oItem.UserFields.Fields.Item(mc_strCodCentroCosto).Value.ToString().Trim()
                                                    strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoAsociado, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                Else
                                                    strCentroCostoAsociado = strCentroCostoTipoOrden
                                                    strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoTipoOrden, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                End If
                                            Else
                                                Dim nameDbTaller As String
                                                Utilitarios.DevuelveNombreBDTaller(SBO_Application, p_strIdSucursal, nameDbTaller)
                                                If Not String.IsNullOrEmpty(nameDbTaller) Then
                                                    If String.IsNullOrEmpty(strCentroCostoTipoOrden) Then
                                                        strCentroCostoAsociado = oItem.UserFields.Fields.Item(mc_strCodCentroCosto).Value.ToString().Trim()
                                                        strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoAsociado, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                    Else
                                                        strCentroCostoAsociado = strCentroCostoTipoOrden
                                                        strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoTipoOrden, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                    End If
                                                End If
                                            End If
                                        End If
                                        If strTipoArticulo = TiposArticulos.scgServicioExt Then
                                            Dim strCuenta As String = String.Empty
                                            objItemAsiento.decMonto = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value ' .AvgStdPrice
                                            decMonto += objItemAsiento.decMonto

                                            strCuenta = Utilitarios.ObtenerCuentaContableArticulo(Utilitarios.TiposArticulos.scgServicioExt, oCotizacion.Lines.ItemCode, Utilitarios.Account.ExpensesAc, p_strIdSucursal, strBodegaProceso)
                                            If Not String.IsNullOrEmpty(strCuenta) Then
                                                objItemAsiento.strCodigoCuentaExistencias = strCuenta
                                            Else
                                                SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaCreditoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                                            End If

                                            'objItemAsiento.strCodigoCuentaExistencias = Utilitarios.ObtenerCuentaItem(oCotizacion.Lines.ItemCode, strBodegaProceso, Cuentas.CuentaGastos, m_oCompany, oItem)

                                        ElseIf strTipoArticulo = TiposArticulos.scgActividad Then
                                            'Se debe solo ingresar una vez, para generar asiento de servicios cuenta contra cuenta, de solo dos lineas
                                            If blnIcluirAsientoServicios = True Then
                                                'Verifica el tipo de costo que utiliza, 1 = Costo Simple, campo U_CostoSimp de las configuraciones generales del add-on
                                                If strTipoCosto.Trim() = "1" Then
                                                    'Se debe obtener el costo por actividad segun el salario del mecánico, luego se debe agregar el monto
                                                    'para generar el respectivo asiento contable
                                                    Dim NoOT As String = oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value.ToString.Trim()
                                                    Dim Costo As Decimal = ObtenerCostoPorActividad(oCotizacion, intEtiqueta, NoOT, p_strIdSucursal)
                                                    objItemAsiento.decMonto = Costo

                                                    'Se debe obtener la cuenta crédito
                                                    'sobre las cual se debe montar el asiento de costos por mano de obra

                                                    If m_blnUsaConfiguracionInternaTaller Then
                                                        objItemAsiento.strCodigoCuentaExistencias = objValoresConfiguracionSucursal.m_strCodigoCuentaExistenciasConfiSucursal
                                                        'objItemAsiento.strCodigoCuentaExistencias = strCodigoCuentaExistenciasConfiSucursal
                                                        blnIcluirAsientoServicios = False
                                                    Else
                                                        objItemAsiento.strCodigoCuentaExistencias = Utilitarios.EjecutarConsulta("Select Valor from SCGTA_TB_Configuracion with (nolock) where Propiedad = 'CuentaContableAcre'", m_strBDTalller, m_oCompany.Server)
                                                        blnIcluirAsientoServicios = False
                                                    End If
                                                Else
                                                    objItemAsiento.decMonto = 0
                                                    decMonto += objItemAsiento.decMonto

                                                    'Se debe obtener la cuenta crédito
                                                    'sobre las cual se debe montar el asiento de costos por mano de obra
                                                    If m_blnUsaConfiguracionInternaTaller Then
                                                        objItemAsiento.strCodigoCuentaExistencias = objValoresConfiguracionSucursal.m_strCodigoCuentaExistenciasConfiSucursal
                                                        blnIcluirAsientoServicios = False
                                                    Else
                                                        objItemAsiento.strCodigoCuentaExistencias = Utilitarios.EjecutarConsulta("Select Valor from SCGTA_TB_Configuracion with (nolock) where Propiedad = 'CuentaContableAcre'", m_strBDTalller, m_oCompany.Server)
                                                        blnIcluirAsientoServicios = False
                                                    End If
                                                End If
                                            End If
                                        Else
                                            objItemAsiento.decMonto = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value
                                            decMonto += objItemAsiento.decMonto
                                            Select Case oItem.GLMethod
                                                Case SAPbobsCOM.BoGLMethods.glm_ItemClass
                                                    objItemAsiento.strCodigoCuentaExistencias = Utilitarios.EjecutarConsulta("select BalInvntAc from OITB with (nolock) where ItmsGrpCod = (Select ItmsGrpCod from OITM with (nolock) where ItemCode = '" & oItem.ItemCode & "')", m_oCompany.CompanyDB, m_oCompany.Server)
                                                Case SAPbobsCOM.BoGLMethods.glm_ItemLevel
                                                    objItemAsiento.strCodigoCuentaExistencias = Utilitarios.EjecutarConsulta("Select BalInvntAc from OITW with (nolock) where ItemCode = '" & oItem.ItemCode & "' and WhsCode = '" & strBodegaProceso & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                                Case SAPbobsCOM.BoGLMethods.glm_WH
                                                    objItemAsiento.strCodigoCuentaExistencias = Utilitarios.EjecutarConsulta("Select BalInvntAc from OWHS with (nolock) where WhsCode = '" & strBodegaProceso & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                            End Select
                                        End If
                                        objItemAsiento.strCodigoItem = oCotizacion.Lines.ItemCode
                                        If objItemAsiento.decMonto > 0 Then
                                            objItemsAsiento.Add(objItemAsiento)
                                        End If
                                    ElseIf blnIcluirEnSalida Then
                                        'oItem = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
                                        'oItem.GetByKey(oCotizacion.Lines.ItemCode)
                                        If Not String.IsNullOrEmpty(p_strIdSucursal) Then
                                            If m_blnUsaConfiguracionInternaTaller Then
                                                If String.IsNullOrEmpty(strCentroCostoTipoOrden) Then
                                                    strCentroCostoAsociado = oItem.UserFields.Fields.Item(mc_strCodCentroCosto).Value.ToString().Trim()
                                                    strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoAsociado, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                Else
                                                    strCentroCostoAsociado = strCentroCostoTipoOrden
                                                    strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoTipoOrden, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                End If
                                            Else
                                                Dim nameDbTaller As String
                                                Utilitarios.DevuelveNombreBDTaller(SBO_Application, p_strIdSucursal, nameDbTaller)
                                                If Not String.IsNullOrEmpty(nameDbTaller) Then
                                                    If String.IsNullOrEmpty(strCentroCostoTipoOrden) Then
                                                        strCentroCostoAsociado = oItem.UserFields.Fields.Item(mc_strCodCentroCosto).Value.ToString().Trim()
                                                        strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoAsociado, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                    Else
                                                        strCentroCostoAsociado = strCentroCostoTipoOrden
                                                        strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoTipoOrden, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                    End If
                                                End If
                                            End If
                                        End If

                                        'validacion de stock en bodega de proceso 
                                        Dim strConsultaOnHand As String = "select OnHand from OITW with (nolock) where ItemCode = '{0}' and WhsCode = '{1}' "
                                        Dim strCantidadOnHand As String
                                        Dim decCantidadOnHand As Decimal = 0

                                        strConsultaOnHand = String.Format(strConsultaOnHand,
                                                                          oCotizacion.Lines.ItemCode,
                                                                          strBodegaProceso.Trim())

                                        strCantidadOnHand = Utilitarios.EjecutarConsulta(strConsultaOnHand, m_oCompany.CompanyDB, m_oCompany.Server)

                                        If Not String.IsNullOrEmpty(strCantidadOnHand) Then
                                            decCantidadOnHand = Decimal.Parse(strCantidadOnHand)
                                        Else
                                            decCantidadOnHand = -1
                                        End If

                                        If decCantidadOnHand < oCotizacion.Lines.Quantity Then
                                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ItemCantidadInventario + oCotizacion.Lines.ItemCode + My.Resources.Resource.InsuficienteSalidaInventario)

                                            Return False
                                            'Throw New ExceptionsSBO(intError, strError)
                                        End If

                                        objItemSalida.strCodigoItem = oCotizacion.Lines.ItemCode
                                        objItemSalida.intCantidad = oCotizacion.Lines.Quantity
                                        objItemSalida.strCodigoAlmacen = strBodegaProceso
                                        objItemSalida.intLineNum = oCotizacion.Lines.LineNum
                                        objItemSalida.intCentroCosto = strCentroCostoAsociado
                                        objItemSalida.Costo = oItem.AvgStdPrice

                                        objItemsSalida.Add(objItemSalida)

                                        If oItem.ManageStockByWarehouse = SAPbobsCOM.BoYesNoEnum.tNO Then
                                            decMonto += oItem.AvgStdPrice * oCotizacion.Lines.Quantity
                                        Else
                                            Dim intWarehouse As Integer
                                            objItemWharehouse = oItem.WhsInfo
                                            For intWarehouse = 0 To objItemWharehouse.Count - 1
                                                objItemWharehouse.SetCurrentLine(intWarehouse)
                                                If objItemWharehouse.WarehouseCode = strBodegaProceso Then
                                                    decMonto += objItemWharehouse.StandardAveragePrice * oCotizacion.Lines.Quantity
                                                End If
                                            Next
                                        End If
                                    End If
                                Else
                                    blnLineasSinAprobar = True
                                End If
                            Else
                                Dim incluirLinea = False
                                Dim chk As SAPbouiCOM.CheckBox
                                For index As Integer = 1 To p_Matrix.RowCount
                                    chk = DirectCast(p_Matrix.Columns.Item("col_Sel").Cells.Item(index).Specific, SAPbouiCOM.CheckBox)

                                    If chk.Checked Then
                                        If m_blnUsaConfiguracionInternaTaller Then
                                            idLinea = DirectCast(p_Matrix.Columns.Item("col_IDLine").Cells.Item(index).Specific, SAPbouiCOM.EditText)
                                            If Not String.IsNullOrEmpty(idLinea.Value.Trim) Then
                                                If oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim = idLinea.Value.Trim Then
                                                    incluirLinea = True
                                                    Exit For
                                                End If
                                            End If
                                        Else
                                            idLinea = DirectCast(p_Matrix.Columns.Item("col_IdRXOr").Cells.Item(index).Specific, SAPbouiCOM.EditText)
                                            If oCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value.ToString.Trim = idLinea.Value.Trim Then
                                                incluirLinea = True
                                                Exit For
                                            End If
                                        End If
                                    End If
                                Next

                                If oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi Then
                                    If incluirLinea Then

                                        blnHayLineasAprobadas = True
                                        blnIncluirEnAsiento = False
                                        blnIcluirEnSalida = False
                                        decTotalMonto += oCotizacion.Lines.LineTotal
                                        strTipoArticulo = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString().Trim()
                                        strNoArticulo = oCotizacion.Lines.ItemCode

                                        Select Case strTipoArticulo
                                            Case TiposArticulos.scgActividad
                                                If intEtiqueta <> 0 Then
                                                    blnIncluirEnAsiento = True
                                                Else
                                                    blnIncluirEnAsiento = False
                                                End If
                                                blnIcluirEnSalida = False
                                            Case TiposArticulos.scgRepuesto
                                                blnIncluirEnAsiento = False
                                                blnIcluirEnSalida = True
                                            Case TiposArticulos.scgServicioExt
                                                If Not lsItemsSE.Contains(strNoArticulo) Then

                                                    lsItemsSE.Add(strNoArticulo)
                                                    'es un servicio externo incluir en el asiento de Serv Externos

                                                    If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value) Then
                                                        AsignaAlmacenProceso(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString.Trim(), oConfiguracionSucursalList, strAlmacenProceso)
                                                    End If

                                                    strCuentaGastosSE = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.ExpensesAc, p_strIdSucursal, strAlmacenProceso)
                                                    strCostoML = ""

                                                    'La busqueda de los costos para la generacion del asiento por servicios Externos se hace por el campo U_SCGD_NoOT de las
                                                    'lineas de detalle del documento

                                                    'Servicio Externo por Factura Provedor
                                                    'If CostosSExFP = "Y" Then
                                                    '    strCostoML = Utilitarios.EjecutarConsulta(String.Format("SELECT SUM(P1.LineTotal) FROM OPCH AS P with (nolock) INNER JOIN PCH1 AS P1 with (nolock) ON P.DocEntry = P1.DocEntry  WHERE P1.U_SCGD_NoOT = '{0}' AND P1.ItemCode = '{1}' and (P1.TrgetEntry is not null or p1.TrgetEntry is null) GROUP BY P1.ItemCode",
                                                    '        strNoOT, strNoArticulo), m_oCompany.CompanyDB, m_oCompany.Server)
                                                    '    decTotalSE = 0

                                                    '    'Servicio Externo por Entrada Mercancia
                                                    'ElseIf CostosSExFP = "N" Then
                                                    '    strCostoML = Utilitarios.EjecutarConsulta(String.Format("SELECT SUM(P1.LineTotal) FROM OPDN AS P with (nolock) INNER JOIN PDN1 AS P1 with (nolock) ON P.DocEntry = P1.DocEntry  WHERE P1.U_SCGD_NoOT = '{0}' AND P1.ItemCode = '{1}' and P1.TargetType <> 21 GROUP BY P1.ItemCode",
                                                    '        strNoOT, strNoArticulo), m_oCompany.CompanyDB, m_oCompany.Server)
                                                    '    decTotalSE = 0
                                                    'End If

                                                    decTotalSE = Decimal.Parse(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value)
                                                    'If Not String.IsNullOrEmpty(strCostoML) Then decTotalSE = Decimal.Parse(strCostoML)

                                                    decTotalFrgSE = 0

                                                    If m_blnUsaConfiguracionInternaTaller Then
                                                        strCuentaTipoOrden = objValoresConfiguracionSucursal.m_strCuentaTipoOrdenInternaConfiSucursal
                                                    Else
                                                        'obtengo el nombre de BD de taller 
                                                        Utilitarios.DevuelveNombreBDTaller(SBO_Application, p_strIdSucursal, strNombreBDTaller)

                                                        strCuentaTipoOrden = Utilitarios.EjecutarConsulta(String.Format("select numero_cuenta_contable from {0}.dbo.SCGTA_TB_Conf_Ot_Iterna with (nolock) where ID_Tipo_Ot = '{1}'",
                                                                strNombreBDTaller, strTipoOrden), m_oCompany.CompanyDB, m_oCompany.Server)
                                                    End If
                                                    If decTotalSE > 0 OrElse decTotalFrgSE > 0 Then

                                                        'retorna el obj item para una linea del asiento de servicios externos
                                                        objItemAsientoSE = AgregaServExterno(strCuentaTipoOrden, decTotalSE, decTotalFrgSE)

                                                        'cargo los items de los asientos de servicios externos a 
                                                        'la lista con las lineas del asiento
                                                        objItemsAsientoSE.Add(objItemAsientoSE)

                                                    End If
                                                End If
                                            Case TiposArticulos.scgSuministro
                                                blnIncluirEnAsiento = False
                                                blnIcluirEnSalida = True
                                        End Select

                                        If blnIncluirEnAsiento Then

                                            'oItem = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
                                            'oItem.GetByKey(oCotizacion.Lines.ItemCode)

                                            If Not String.IsNullOrEmpty(p_strIdSucursal) Then

                                                If m_blnUsaConfiguracionInternaTaller Then
                                                    If String.IsNullOrEmpty(strCentroCostoTipoOrden) Then
                                                        strCentroCostoAsociado = oItem.UserFields.Fields.Item(mc_strCodCentroCosto).Value.ToString().Trim()
                                                        strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoAsociado, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                    Else
                                                        strCentroCostoAsociado = strCentroCostoTipoOrden
                                                        strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoTipoOrden, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                    End If
                                                Else
                                                    Dim nameDbTaller As String
                                                    Utilitarios.DevuelveNombreBDTaller(SBO_Application, p_strIdSucursal, nameDbTaller)
                                                    If Not String.IsNullOrEmpty(nameDbTaller) Then
                                                        If String.IsNullOrEmpty(strCentroCostoTipoOrden) Then
                                                            strCentroCostoAsociado = oItem.UserFields.Fields.Item(mc_strCodCentroCosto).Value.ToString().Trim()
                                                            strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoAsociado, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                        Else
                                                            strCentroCostoAsociado = strCentroCostoTipoOrden
                                                            strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoTipoOrden, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                        End If
                                                    End If
                                                End If
                                            End If

                                            If strTipoArticulo = TiposArticulos.scgServicioExt Then
                                                objItemAsiento.decMonto = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value ' .AvgStdPrice
                                                decMonto += objItemAsiento.decMonto

                                                objItemAsiento.strCodigoCuentaExistencias = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.ExpensesAc, p_strIdSucursal, strBodegaProceso)

                                            ElseIf strTipoArticulo = TiposArticulos.scgActividad Then
                                                'Se debe solo ingresar una vez, para generar asiento de servicios cuenta contra cuenta, de solo dos lineas
                                                If blnIcluirAsientoServicios = True Then
                                                    If strTipoCosto.Trim() = "1" Then
                                                        'Se debe obtener el costo por actividad segun el salario del mecánico, luego se debe agregar el monto
                                                        'para generar el respectivo asiento contable
                                                        Dim NoOT As String = oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value.ToString.Trim()
                                                        Dim Costo As Decimal = ObtenerCostoTotalActividades(oCotizacion, intEtiqueta, p_Matrix)
                                                        objItemAsiento.decMonto = Costo
                                                        'Se debe obtener la cuenta crédito
                                                        'sobre las cual se debe montar el asiento de costos por mano de obra

                                                        If m_blnUsaConfiguracionInternaTaller Then
                                                            objItemAsiento.strCodigoCuentaExistencias = objValoresConfiguracionSucursal.m_strCodigoCuentaExistenciasConfiSucursal
                                                            'objItemAsiento.strCodigoCuentaExistencias = strCodigoCuentaExistenciasConfiSucursal
                                                            blnIcluirAsientoServicios = False
                                                        Else
                                                            objItemAsiento.strCodigoCuentaExistencias = Utilitarios.EjecutarConsulta("Select Valor from SCGTA_TB_Configuracion with (nolock) where Propiedad = 'CuentaContableAcre'", m_strBDTalller, m_oCompany.Server)
                                                            blnIcluirAsientoServicios = False
                                                        End If
                                                    Else
                                                        objItemAsiento.decMonto = 0
                                                        decMonto += objItemAsiento.decMonto

                                                        'Se debe obtener la cuenta crédito
                                                        'sobre las cual se debe montar el asiento de costos por mano de obra
                                                        If m_blnUsaConfiguracionInternaTaller Then
                                                            objItemAsiento.strCodigoCuentaExistencias = objValoresConfiguracionSucursal.m_strCodigoCuentaExistenciasConfiSucursal
                                                            blnIcluirAsientoServicios = False
                                                        Else
                                                            objItemAsiento.strCodigoCuentaExistencias = Utilitarios.EjecutarConsulta("Select Valor from SCGTA_TB_Configuracion with (nolock) where Propiedad = 'CuentaContableAcre'", m_strBDTalller, m_oCompany.Server)
                                                            blnIcluirAsientoServicios = False
                                                        End If
                                                    End If
                                                End If
                                            Else
                                                objItemAsiento.decMonto = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value
                                                decMonto += objItemAsiento.decMonto
                                                Select Case oItem.GLMethod
                                                    Case SAPbobsCOM.BoGLMethods.glm_ItemClass
                                                        objItemAsiento.strCodigoCuentaExistencias = Utilitarios.EjecutarConsulta("select BalInvntAc from OITB with (nolock) where ItmsGrpCod = (Select ItmsGrpCod from OITM with (nolock) where ItemCode = '" & oItem.ItemCode & "')", m_oCompany.CompanyDB, m_oCompany.Server)
                                                    Case SAPbobsCOM.BoGLMethods.glm_ItemLevel
                                                        objItemAsiento.strCodigoCuentaExistencias = Utilitarios.EjecutarConsulta("Select BalInvntAc from OITW with (nolock) where ItemCode = '" & oItem.ItemCode & "' and WhsCode = '" & strBodegaProceso & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                                    Case SAPbobsCOM.BoGLMethods.glm_WH
                                                        objItemAsiento.strCodigoCuentaExistencias = Utilitarios.EjecutarConsulta("Select BalInvntAc from OWHS with (nolock) where WhsCode = '" & strBodegaProceso & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                                End Select
                                            End If
                                            objItemAsiento.strCodigoItem = oCotizacion.Lines.ItemCode
                                            If objItemAsiento.decMonto > 0 Then
                                                objItemsAsiento.Add(objItemAsiento)
                                            End If
                                        ElseIf blnIcluirEnSalida Then
                                            'oItem = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
                                            'oItem.GetByKey(oCotizacion.Lines.ItemCode)
                                            If Not String.IsNullOrEmpty(p_strIdSucursal) Then
                                                If m_blnUsaConfiguracionInternaTaller Then
                                                    If String.IsNullOrEmpty(strCentroCostoTipoOrden) Then
                                                        strCentroCostoAsociado = oItem.UserFields.Fields.Item(mc_strCodCentroCosto).Value.ToString().Trim()
                                                        strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoAsociado, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                    Else
                                                        strCentroCostoAsociado = strCentroCostoTipoOrden
                                                        strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoTipoOrden, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                    End If
                                                Else
                                                    Dim nameDbTaller As String
                                                    Utilitarios.DevuelveNombreBDTaller(SBO_Application, p_strIdSucursal, nameDbTaller)
                                                    If Not String.IsNullOrEmpty(nameDbTaller) Then
                                                        If String.IsNullOrEmpty(strCentroCostoTipoOrden) Then
                                                            strCentroCostoAsociado = oItem.UserFields.Fields.Item(mc_strCodCentroCosto).Value.ToString().Trim()
                                                            strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoAsociado, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                        Else
                                                            strCentroCostoAsociado = strCentroCostoTipoOrden
                                                            strBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCostoTipoOrden, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application)
                                                        End If
                                                    End If
                                                End If
                                            End If

                                            'validacion de stock en bodega de proceso 
                                            Dim strConsultaOnHand As String = "select OnHand from OITW with (nolock) where ItemCode = '{0}' and WhsCode = '{1}' "
                                            Dim strCantidadOnHand As String
                                            Dim decCantidadOnHand As Decimal = 0

                                            strConsultaOnHand = String.Format(strConsultaOnHand, oCotizacion.Lines.ItemCode, strBodegaProceso.Trim())
                                            strCantidadOnHand = Utilitarios.EjecutarConsulta(strConsultaOnHand, m_oCompany.CompanyDB, m_oCompany.Server)

                                            If Not String.IsNullOrEmpty(strCantidadOnHand) Then
                                                decCantidadOnHand = Decimal.Parse(strCantidadOnHand)
                                            Else
                                                decCantidadOnHand = -1
                                            End If

                                            If decCantidadOnHand < oCotizacion.Lines.Quantity Then
                                                SBO_Application.StatusBar.SetText(My.Resources.Resource.ItemCantidadInventario + oCotizacion.Lines.ItemCode + My.Resources.Resource.InsuficienteSalidaInventario)
                                                Return False
                                            End If

                                            objItemSalida.strCodigoItem = oCotizacion.Lines.ItemCode
                                            objItemSalida.intCantidad = oCotizacion.Lines.Quantity
                                            objItemSalida.strCodigoAlmacen = strBodegaProceso
                                            objItemSalida.intLineNum = oCotizacion.Lines.LineNum
                                            objItemSalida.intCentroCosto = strCentroCostoAsociado
                                            'Error detectado en un cliente, se revisó con Josué y la causa es por el método de costeo
                                            'objItemSalida.Costo = oItem.ProdStdCost
                                            objItemSalida.Costo = oItem.AvgStdPrice
                                            objItemsSalida.Add(objItemSalida)

                                            If oItem.ManageStockByWarehouse = SAPbobsCOM.BoYesNoEnum.tNO Then
                                                decMonto += oItem.AvgStdPrice * oCotizacion.Lines.Quantity
                                            Else
                                                Dim intWarehouse As Integer
                                                objItemWharehouse = oItem.WhsInfo
                                                For intWarehouse = 0 To objItemWharehouse.Count - 1
                                                    objItemWharehouse.SetCurrentLine(intWarehouse)
                                                    If objItemWharehouse.WarehouseCode = strBodegaProceso Then
                                                        decMonto += objItemWharehouse.StandardAveragePrice * oCotizacion.Lines.Quantity
                                                    End If
                                                Next
                                            End If
                                        End If
                                    End If
                                Else
                                    blnLineasSinAprobar = True
                                End If
                            End If
                        Next intFila

                        If blnHayLineasAprobadas Then
                            blnLineasAgregadas = False
                            If objItemsSalida.Count > 0 Then
                                Dim intNoOT As String

                                blnDraft = False

                                If blnDraft Then
                                    ''variable para obtener el UDF de codigo de cotizacion que se inserta cuando 
                                    ''se crea un documento draft
                                    Dim visOrder As Integer
                                    Dim cadenaConexion As String = String.Empty
                                    Dim nombreTabla As String = "WTR1"

                                    Dim u_codigo_cotizacion As Integer = oCotizacion.DocEntry
                                    If p_strDECot <> "" Then
                                        u_codigo_cotizacion = Convert.ToInt32(p_strDECot)
                                    End If

                                    Dim u_ordentrabajoPadre As String = oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value

                                    Dim m_dstTransferencias As New TransferenciasPorCotizacionDataSet
                                    Dim m_drwTransferencias As TransferenciasPorCotizacionDataSet.TransferenciasPorCotizacionRow
                                    Dim m_adpSeries As New SeriesLotesDataAdapter
                                    Dim m_dstSeries As New SeriesLotesDataSet
                                    Dim m_dtsCotizacionPorOTPadre As New TransferenciasPorCotizacionDataSet
                                    Dim m_drwCotizacionPorOTPadre As TransferenciasPorCotizacionDataSet.CotizacionPorOTPadreRow

                                    Dim drwSeriesLotes As SeriesLotesDataSet.SeriesLotesRow

                                    Dim strCadenaConexionBDTaller As String = ""

                                    Dim m_adpOrdenTrabajo As New DMSOneFramework.SCGDataAccess.OrdenTrabajoDataAdapter
                                    Dim m_dstOrdenTrabajo As New DMSOneFramework.OrdenTrabajoDataset

                                    Dim adpItems As New DMSOneFramework.SCGDataAccess.ItemsRepuestosSuministrosDataAdapter
                                    Dim dstItems As New DMSOneFramework.ItemsRepuestosSuministrosDataset
                                    Dim drwItems As DMSOneFramework.ItemsRepuestosSuministrosDataset.ItemsRepuestosSuministrosRow

                                    '******************Inicio - documento Salida de inventario *******************
                                    If Not String.IsNullOrEmpty(strMoneda) Then
                                        oSalidaMercancia.DocCurrency = strMoneda
                                    End If
                                    oSalidaMercancia.UserFields.Fields.Item(mc_strNum_OT).Value = oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value
                                    oSalidaMercancia.UserFields.Fields.Item(mc_strNumUnidad).Value = oCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value
                                    If Not String.IsNullOrEmpty(strTransaccionLineas) Then
                                        oSalidaMercancia.UserFields.Fields.Item(mc_strProcesad).Value = "1"
                                    Else
                                        oSalidaMercancia.UserFields.Fields.Item(mc_strProcesad).Value = "2"
                                    End If
                                    oSalidaMercancia.UserFields.Fields.Item(mc_strNumVehiculo).Value = oCotizacion.UserFields.Fields.Item(mc_strNumVehiculo).Value
                                    'proyecto
                                    oSalidaMercancia.Project = oCotizacion.UserFields.Fields.Item(mc_strProyecto).Value

                                    If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                                        If Not String.IsNullOrEmpty(p_strIdSucursal) Then
                                            oSalidaMercancia.BPL_IDAssignedToInvoice = Integer.Parse(p_strIdSucursal)
                                        End If
                                    End If


                                    If Not u_ordentrabajoPadre = String.Empty Then

                                        intNoOT = oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value

                                        Dim objItemsRepuestosSuministros As New Generic.List(Of ItemsRepuestosSuministrosSalida)

                                        Dim objItemRepSumSalida As New ItemsRepuestosSuministrosSalida

                                        Dim strVisita As String = oCotizacion.UserFields.Fields.Item(mc_strNum_Visita).Value

                                        Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, p_strIdSucursal, strCadenaConexionBDTaller)

                                        m_dstOrdenTrabajo.EnforceConstraints = False


                                        'busco las transferencias de la cotizacion hija
                                        m_adpSeries.Fill_Transferencias(m_dstTransferencias, u_codigo_cotizacion)

                                        If m_dstTransferencias.TransferenciasPorCotizacion.Rows.Count = 0 Then

                                            m_adpSeries.Fill_CotizacionPorOTPadre(m_dtsCotizacionPorOTPadre, strVisita & "-01") 'u_ordentrabajoPadre)

                                            'obtengo las lineas del registro padre
                                            m_drwCotizacionPorOTPadre = m_dtsCotizacionPorOTPadre.CotizacionPorOTPadre.Rows(0)


                                            m_adpSeries.Fill_Transferencias(m_dstTransferencias, m_drwCotizacionPorOTPadre.DocEntry)

                                        Else

                                            m_adpSeries.Fill_CotizacionPorOTPadre(m_dtsCotizacionPorOTPadre, strVisita & "-01")

                                            'obtengo las lineas del registro padre
                                            m_drwCotizacionPorOTPadre = m_dtsCotizacionPorOTPadre.CotizacionPorOTPadre.Rows(0)


                                            m_adpSeries.Fill_Transferencias(m_dstTransferencias, m_drwCotizacionPorOTPadre.DocEntry)

                                        End If

                                        'se cargan los datasets para repuestos y suministros
                                        adpItems.Fill_ItemsRepuestosSuministros(dstItems, intNoOT)

                                        For Each drwItems In dstItems.ItemsRepuestosSuministros.Rows
                                            objItemRepSumSalida.strCodigoItem = drwItems.Items
                                            objItemRepSumSalida.strCodigoAlmacen = strContraCuenta
                                            objItemRepSumSalida.intCantidad = drwItems.Cantidad
                                            objItemRepSumSalida.intLineNum = drwItems.LineNum
                                            objItemRepSumSalida.intLineNumOriginal = drwItems.LineNumOriginal
                                            objItemsRepuestosSuministros.Add(objItemRepSumSalida)
                                        Next

                                        For Each m_drwTransferencias In m_dstTransferencias.TransferenciasPorCotizacion.Rows

                                            Dim m_oBuscarTransfer As SAPbobsCOM.StockTransfer
                                            Dim m_oLineasTransfer As SAPbobsCOM.StockTransfer_Lines

                                            m_oBuscarTransfer = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)

                                            If m_oBuscarTransfer.GetByKey(m_drwTransferencias.DocEntry) Then

                                                If m_oBuscarTransfer.UserFields.Fields.Item(mc_strUTipoTransferencia).Value = 1 Then

                                                    m_oLineasTransfer = m_oBuscarTransfer.Lines

                                                    For i As Integer = 0 To m_oLineasTransfer.Count - 1

                                                        m_oLineasTransfer.SetCurrentLine(i)

                                                        For Each objItemRepSumSalida In objItemsRepuestosSuministros
                                                            If m_oLineasTransfer.UserFields.Fields.Item("U_SCGD_LinenumOrigen").Value = objItemRepSumSalida.intLineNumOriginal Then

                                                                If blnLineasAgregadas Then
                                                                    oSalidaMercancia.Lines.Add()
                                                                Else
                                                                    blnLineasAgregadas = True
                                                                End If

                                                                oSalidaMercancia.Lines.ItemCode = m_oLineasTransfer.ItemCode
                                                                oSalidaMercancia.Lines.WarehouseCode = m_oLineasTransfer.WarehouseCode
                                                                oSalidaMercancia.Lines.Quantity = m_oLineasTransfer.Quantity
                                                                oSalidaMercancia.Lines.AccountCode = strContraCuenta
                                                                oSalidaMercancia.Reference2 = strNoOT
                                                                'proyecto
                                                                oSalidaMercancia.Lines.ProjectCode = oCotizacion.UserFields.Fields.Item(mc_strProyecto).Value

                                                                If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                                                                    ClsLineasDocumentosDimension.AgregarDimensionesLineasDocumentos(oSalidaMercancia.Lines, oDataTableDimensionesContablesDMS)
                                                                End If

                                                                'cargo los lotes y series de cada linea de la transferencia
                                                                m_adpSeries.Fill_SeriesLotes(m_dstSeries, 67, m_drwTransferencias.DocEntry, "Y")
                                                                '****************************inicio Obtener VisOrder**********************

                                                                Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, cadenaConexion)
                                                                visOrder = Utilitarios.ObtieneVisOrder(m_oCompany, nombreTabla, cadenaConexion, m_oLineasTransfer.LineNum, m_oLineasTransfer.ItemCode, m_drwTransferencias.DocEntry)
                                                                '****************************fin Obtener VisOrder**********************

                                                                For Each drwSeriesLotes In m_dstSeries.SeriesLotes.Rows
                                                                    If drwSeriesLotes.RowNoInBaseDocument = visOrder Then
                                                                        If drwSeriesLotes.IsBatchNumNull Then
                                                                            oSalidaMercancia.Lines.SerialNumbers.SystemSerialNumber = drwSeriesLotes.SysSerial
                                                                            oSalidaMercancia.Lines.SerialNumbers.InternalSerialNumber = drwSeriesLotes.SerialNumber
                                                                            oSalidaMercancia.Lines.SerialNumbers.ManufacturerSerialNumber = drwSeriesLotes.ManufacturerSerial
                                                                            oSalidaMercancia.Lines.SerialNumbers.Add()
                                                                        Else
                                                                            oSalidaMercancia.Lines.BatchNumbers.BatchNumber = drwSeriesLotes.BatchNum
                                                                            oSalidaMercancia.Lines.BatchNumbers.Quantity = drwSeriesLotes.Quantity
                                                                            oSalidaMercancia.Lines.BatchNumbers.Add()
                                                                        End If
                                                                    End If
                                                                Next
                                                                m_dstSeries.Clear()
                                                            End If
                                                        Next
                                                    Next
                                                End If
                                            End If
                                        Next
                                        m_dstTransferencias.Clear()
                                    Else
                                        m_adpSeries.Fill_Transferencias(m_dstTransferencias, u_codigo_cotizacion)
                                    End If

                                    'recorre cada una de las transferencias y sus lineas
                                    For Each m_drwTransferencias In m_dstTransferencias.TransferenciasPorCotizacion.Rows

                                        Dim m_oBuscarTransfer As SAPbobsCOM.StockTransfer
                                        Dim m_oLineasTransfer As SAPbobsCOM.StockTransfer_Lines

                                        intNoOT = oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value

                                        m_oBuscarTransfer = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)

                                        If m_oBuscarTransfer.GetByKey(m_drwTransferencias.DocEntry) Then

                                            'recorro las lineas de la transferencia y adhiero los numeros de lotes o series a 
                                            'cada linea de la transferencia
                                            If m_oBuscarTransfer.UserFields.Fields.Item(mc_strUTipoTransferencia).Value = 1 Then
                                                m_oLineasTransfer = m_oBuscarTransfer.Lines
                                                For Each objItemSalida In objItemsSalida
                                                    For i As Integer = 0 To m_oLineasTransfer.Count - 1
                                                        m_oLineasTransfer.SetCurrentLine(i)

                                                        If objItemSalida.intLineNum = m_oLineasTransfer.UserFields.Fields.Item("U_SCGD_LinenumOrigen").Value Then

                                                            If blnLineasAgregadas Then
                                                                oSalidaMercancia.Lines.Add()
                                                            Else
                                                                blnLineasAgregadas = True
                                                            End If

                                                            oSalidaMercancia.Lines.ItemCode = m_oLineasTransfer.ItemCode
                                                            oSalidaMercancia.Lines.WarehouseCode = m_oLineasTransfer.WarehouseCode
                                                            oSalidaMercancia.Lines.Quantity = m_oLineasTransfer.Quantity
                                                            oSalidaMercancia.Lines.AccountCode = strContraCuenta
                                                            oSalidaMercancia.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = objItemSalida.Costo
                                                            'proyecto
                                                            oSalidaMercancia.Lines.ProjectCode = oCotizacion.UserFields.Fields.Item(mc_strProyecto).Value

                                                            'cargo los lotes y series de cada linea de la transferencia
                                                            m_adpSeries.Fill_SeriesLotes(m_dstSeries, 67, m_drwTransferencias.DocEntry, "Y")
                                                            '****************************inicio Obtener VisOrder**********************

                                                            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, cadenaConexion)

                                                            visOrder = Utilitarios.ObtieneVisOrder(m_oCompany, nombreTabla, cadenaConexion, m_oLineasTransfer.LineNum, m_oLineasTransfer.ItemCode, m_drwTransferencias.DocEntry)
                                                            '****************************fin Obtener VisOrder**********************
                                                            For Each drwSeriesLotes In m_dstSeries.SeriesLotes.Rows

                                                                If drwSeriesLotes.RowNoInBaseDocument = visOrder Then

                                                                    If drwSeriesLotes.IsBatchNumNull Then
                                                                        oSalidaMercancia.Lines.SerialNumbers.SystemSerialNumber = drwSeriesLotes.SysSerial
                                                                        oSalidaMercancia.Lines.SerialNumbers.InternalSerialNumber = drwSeriesLotes.SerialNumber
                                                                        oSalidaMercancia.Lines.SerialNumbers.ManufacturerSerialNumber = drwSeriesLotes.ManufacturerSerial
                                                                        oSalidaMercancia.Lines.SerialNumbers.Add()
                                                                    Else
                                                                        oSalidaMercancia.Lines.BatchNumbers.BatchNumber = drwSeriesLotes.BatchNum
                                                                        oSalidaMercancia.Lines.BatchNumbers.Quantity = drwSeriesLotes.Quantity
                                                                        oSalidaMercancia.Lines.BatchNumbers.Add()
                                                                    End If
                                                                End If
                                                            Next
                                                            m_dstSeries.Clear()
                                                            Exit For
                                                        End If
                                                    Next
                                                Next
                                            End If
                                        End If
                                    Next

                                    If m_blnUsaConfiguracionInternaTaller Then
                                        'agregar el centro de beneficio (Norma de reparto) por Tipo de orden
                                        If Not String.IsNullOrEmpty(strCentroBeneficio) Then oSalidaMercancia.Lines.CostingCode = strCentroBeneficio
                                    Else
                                        If String.IsNullOrEmpty(strCentroBeneficio) Then strCentroBeneficioConfOT = ConfiguracionDataAdapter.RetornaCentroBeneficioByItem(oCotizacion.Lines.ItemCode, strConexionDBSucursal)
                                        If Not String.IsNullOrEmpty(strCentroBeneficioConfOT) Then oSalidaMercancia.Lines.CostingCode = strCentroBeneficioConfOT
                                    End If

                                    'If oSalidaMercancia.Add() = 0 Then
                                    '    Call m_oCompany.GetNewObjectCode(strIdSalida)
                                    '    oSalidaMercancia.GetByKey(strIdSalida)
                                    'Else
                                    '    m_oCompany.GetLastError(intError, strError)
                                    '    Throw New ExceptionsSBO(intError, strError)
                                    'End If

                                Else

                                    'procedimiento normal sin draft, ni series ni lotes

                                    ''NO BORRAR POR EL MOMENTO
                                    ''Se comenta para dejar las salida de mercancia sin el proceso de ubicaciones
                                    'Dim dtBodegasXCentroCosto As System.Data.DataTable
                                    'dtBodegasXCentroCosto = New System.Data.DataTable
                                    'dtBodegasXCentroCosto = Utilitarios.LlenarTablaconUbicacionDefectoenBodegoProcesoXCentroCosto(p_strIdSucursal, m_oCompany)

                                    oSalidaMercancia.UserFields.Fields.Item(mc_strNum_OT).Value = oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value
                                    oSalidaMercancia.UserFields.Fields.Item(mc_strNumUnidad).Value = oCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value
                                    If Not String.IsNullOrEmpty(strTransaccionLineas) Then
                                        oSalidaMercancia.UserFields.Fields.Item(mc_strProcesad).Value = "1"
                                    Else
                                        oSalidaMercancia.UserFields.Fields.Item(mc_strProcesad).Value = "2"
                                    End If
                                    oSalidaMercancia.UserFields.Fields.Item(mc_strNumVehiculo).Value = oCotizacion.UserFields.Fields.Item(mc_strNumVehiculo).Value

                                    If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                                        If Not String.IsNullOrEmpty(p_strIdSucursal) Then
                                            oSalidaMercancia.BPL_IDAssignedToInvoice = Integer.Parse(p_strIdSucursal)
                                        End If
                                    End If

                                    For Each objItemSalida In objItemsSalida
                                        If blnLineasAgregadas Then
                                            oSalidaMercancia.Lines.Add()
                                        Else
                                            blnLineasAgregadas = True
                                        End If
                                        oSalidaMercancia.Lines.ItemCode = objItemSalida.strCodigoItem
                                        oSalidaMercancia.Lines.WarehouseCode = objItemSalida.strCodigoAlmacen
                                        oSalidaMercancia.Lines.Quantity = objItemSalida.intCantidad
                                        oSalidaMercancia.Reference2 = strNoOT
                                        oSalidaMercancia.Lines.AccountCode = strContraCuenta
                                        oSalidaMercancia.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = objItemSalida.Costo


                                        ''NO BORRAR POR EL MOMENTO
                                        ''Se comenta para dejar las salida de mercancia sin el proceso de ubicaciones
                                        ''Dim intUbicacionDefectoProceso As Integer = Utilitarios.DevolverUbicacionDefectoProceso(dtBodegasXCentroCosto, objItemSalida.intCentroCosto)
                                        ''If intUbicacionDefectoProceso <> 0 Then
                                        ''    oSalidaMercancia.Lines.BinAllocations.BinAbsEntry = intUbicacionDefectoProceso
                                        ''    oSalidaMercancia.Lines.BinAllocations.Quantity = objItemSalida.intCantidad
                                        ''    oSalidaMercancia.Lines.BinAllocations.Add()
                                        ''End If

                                        If blnAgregarDimension Then
                                            If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                                                ClsLineasDocumentosDimension.AgregarDimensionesLineasDocumentos(oSalidaMercancia.Lines, oDataTableDimensionesContablesDMS)
                                            End If
                                        End If
                                        'proyecto
                                        oSalidaMercancia.Lines.ProjectCode = oCotizacion.UserFields.Fields.Item(mc_strProyecto).Value

                                        If m_blnUsaConfiguracionInternaTaller Then
                                            'se debe agregar funcionalidad para agregar el centro de beneficio (Norma de reparto) por Tipo de orden
                                            If Not String.IsNullOrEmpty(strCentroBeneficio) Then oSalidaMercancia.Lines.CostingCode = strCentroBeneficio
                                        Else
                                            If String.IsNullOrEmpty(strCentroBeneficio) Then strCentroBeneficioConfOT = ConfiguracionDataAdapter.RetornaCentroBeneficioByItem(oCotizacion.Lines.ItemCode, strConexionDBSucursal)
                                            If Not String.IsNullOrEmpty(strCentroBeneficioConfOT) Then oSalidaMercancia.Lines.CostingCode = strCentroBeneficioConfOT
                                        End If
                                    Next
                                    'If oSalidaMercancia.Add() = 0 Then
                                    '    Call m_oCompany.GetNewObjectCode(strIdSalida)
                                    '    oSalidaMercancia.GetByKey(strIdSalida)
                                    'Else
                                    '    m_oCompany.GetLastError(intError, strError)
                                    '    Throw New ExceptionsSBO(intError, strError)'End If
                                End If
                            End If

                            If objItemsAsiento.Count > 0 Then

                                'Validación de cuenta acrredora en caso de que sea nula

                                If objItemAsiento.strCodigoCuentaExistencias = String.Empty Then

                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ValidarCuentaCredito, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    Exit Function

                                End If
                                'If Not String.IsNullOrEmpty(strCodigoTransaccionAsiento) Then
                                '    oAsientoContable.TransactionCode = strCodigoTransaccionAsiento
                                'End If
                                oAsientoContable.Memo = oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value
                                'proyecto
                                oAsientoContable.ProjectCode = oCotizacion.UserFields.Fields.Item(mc_strProyecto).Value


                                decMontoAsiento = 0
                                blnLineasAgregadas = False
                                ' For Each objItemAsiento In objItemsAsiento
                                If blnLineasAgregadas Then
                                    oAsientoContable.Lines.Add()
                                Else
                                    blnLineasAgregadas = True
                                End If
                                oAsientoContable.Lines.AccountCode = objItemAsiento.strCodigoCuentaExistencias

                                If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                                    If Not String.IsNullOrEmpty(p_strIdSucursal) Then oAsientoContable.Lines.BPLID = Integer.Parse(p_strIdSucursal)
                                End If

                                If Not String.IsNullOrEmpty(strCentroBeneficio) Then oAsientoContable.Lines.CostingCode = strCentroBeneficio

                                Dim strMonedaLocalValidacion As String = String.Empty
                                Dim strMonedaSystemaValidacion As String = String.Empty

                                strMonedaLocalValidacion = RetornarMonedaLocal()
                                strMonedaSystemaValidacion = RetornarMonedaSistema()

                                'Validacion para saber el tipo tipo de moneda a la hora de crear el asiento contable
                                'CREDITO del asiento
                                If Not String.IsNullOrEmpty(strTipoMoneda) Then
                                    If (strMonedaLocalValidacion = strTipoMoneda) Then 'Or (strMonedaSystemaValidacion = strTipoMoneda) Then
                                        'Asiento en la Columna Moneda Local Y Moneda Sistema
                                        oAsientoContable.Lines.Credit = objItemAsiento.decMonto
                                        decMontoAsiento += objItemAsiento.decMonto
                                    Else
                                        'Asiento en la Columna Moneda Extranjera
                                        oAsientoContable.Lines.FCCurrency = strTipoMoneda.Trim()
                                        oAsientoContable.Lines.FCCredit = objItemAsiento.decMonto
                                        decMontoAsiento += objItemAsiento.decMonto
                                    End If
                                End If

                                If blnAgregarDimension Then
                                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oAsientoContable.Lines, Nothing, oDataTableDimensionesContablesDMS)
                                End If

                                'Next
                                If decMontoAsiento > 0 Then

                                    If strContraCuenta = String.Empty Then

                                        'Se debe cambiar numero por cuenta debito
                                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ValidarCuentaDebito, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                        Exit Function
                                    End If

                                    oAsientoContable.Lines.Add()
                                    oAsientoContable.Lines.AccountCode = strContraCuenta

                                    If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                                        If Not String.IsNullOrEmpty(p_strIdSucursal) Then oAsientoContable.Lines.BPLID = Integer.Parse(p_strIdSucursal)
                                    End If

                                    If Not String.IsNullOrEmpty(strTransaccionLineas) Then
                                        oAsientoContable.Lines.UserFields.Fields.Item(mc_strTransaccion).Value = strTransaccionLineas
                                    End If
                                    oAsientoContable.Lines.UserFields.Fields.Item(mc_strCodUnidad).Value = oCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value
                                    If Not String.IsNullOrEmpty(strCentroBeneficio) Then oAsientoContable.Lines.CostingCode = strCentroBeneficio

                                    'Validacion para saber el tipo tipo de moneda a la hora de crear el asiento contable
                                    'DEBITO del asiento
                                    If Not String.IsNullOrEmpty(strTipoMoneda) Then
                                        If (strMonedaLocalValidacion = strTipoMoneda) Then 'Or (strMonedaSystemaValidacion = strTipoMoneda) Then
                                            'Asiento en la Columna Moneda Local Y Moneda Sistema
                                            oAsientoContable.Lines.Debit = objItemAsiento.decMonto
                                            decMontoAsiento += objItemAsiento.decMonto
                                        Else
                                            'Asiento en la Columna Moneda Extranjera
                                            oAsientoContable.Lines.FCCurrency = strTipoMoneda.Trim()
                                            oAsientoContable.Lines.FCDebit = objItemAsiento.decMonto
                                            decMontoAsiento += objItemAsiento.decMonto
                                        End If
                                    End If

                                    If blnAgregarDimension Then
                                        ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oAsientoContable.Lines, Nothing, oDataTableDimensionesContablesDMS)
                                    End If

                                    'If oAsientoContable.Add() = 0 Then
                                    '    Call m_oCompany.GetNewObjectCode(strIdAsiento)
                                    'Else
                                    '    m_oCompany.GetLastError(intError, strError)
                                    '    Throw New ExceptionsSBO(intError, strError)
                                    'End If
                                End If
                            End If

                            Dim strMonedaLocal As String
                            strMonedaLocal = RetornarMonedaLocal()

                            'creo el asiento contable para servicios externos
                            If objItemsAsientoSE.Count > 0 And blnUsaAsientoSE Then
                                'lista de items de asientos de servicios externos
                                blnLineasAgregadasSE = False
                                If p_strDNCot <> "" Then
                                    strMemo = My.Resources.Resource.AsientoCotizacion + p_strDNCot.Trim()
                                Else
                                    strMemo = My.Resources.Resource.AsientoCotizacion + oCotizacion.DocNum.ToString()
                                End If
                                oAsientoServExt.Memo = strMemo
                                oAsientoServExt.Reference = oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value
                                'proyecto
                                oAsientoServExt.ProjectCode = oCotizacion.UserFields.Fields.Item(mc_strProyecto).Value
                                dcTotalAcumulado_JE = 0
                                For Each objItemAsientoSE In objItemsAsientoSE
                                    If objItemAsientoSE.dcTotal > 0 Or objItemAsientoSE.dcTotalFrg > 0 Then

                                        ' For Each objItemAsiento In objItemsAsiento
                                        If blnLineasAgregadasSE Then
                                            oAsientoServExt.Lines.Add()
                                        Else
                                            blnLineasAgregadasSE = True
                                        End If
                                        oAsientoServExt.Lines.AccountCode = objItemAsientoSE.strCuenta

                                        If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                                            If Not String.IsNullOrEmpty(p_strIdSucursal) Then oAsientoServExt.Lines.BPLID = Integer.Parse(p_strIdSucursal)
                                        End If
                                        'valida que la moneda no sea igual a la moneda local 
                                        'If Not String.IsNullOrEmpty(strMoneda) _
                                        '    And strMoneda.Trim() <> strMonedaLocal.Trim() Then
                                        '    oAsientoServExt.Lines.FCCurrency = strMoneda
                                        '    oAsientoServExt.Lines.FCDebit = objItemAsientoSE.dcTotalFrg
                                        '    dcTotalAcumulado_JE += objItemAsientoSE.dcTotalFrg
                                        'Else
                                        '    oAsientoServExt.Lines.Debit = objItemAsientoSE.dcTotal
                                        '    dcTotalAcumulado_JE += objItemAsientoSE.dcTotal
                                        'End If

                                        oAsientoServExt.Lines.Debit = objItemAsientoSE.dcTotal

                                        dcTotalAcumulado_JE += objItemAsientoSE.dcTotal

                                        If Not String.IsNullOrEmpty(strTransaccionLineas) Then
                                            oAsientoServExt.Lines.UserFields.Fields.Item(mc_strTransaccion).Value = strTransaccionLineas
                                        End If


                                        oAsientoServExt.Lines.UserFields.Fields.Item(mc_strCodUnidad).Value = oCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value

                                        If blnAgregarDimension Then
                                            ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oAsientoServExt.Lines, Nothing, oDataTableDimensionesContablesDMS)
                                        End If
                                    End If
                                Next
                                If dcTotalAcumulado_JE > 0 Then
                                    If strCuentaGastosSE = String.Empty Then
                                        'Se debe asignar numero de cuenta para la bodega de proceso 
                                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ValidarCuentaGastosBodProceso, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                                        Exit Function
                                    End If

                                    oAsientoServExt.Lines.Add()
                                    oAsientoServExt.Lines.AccountCode = strCuentaGastosSE

                                    If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                                        If Not String.IsNullOrEmpty(p_strIdSucursal) Then oAsientoServExt.Lines.BPLID = Integer.Parse(p_strIdSucursal)
                                    End If

                                    oAsientoServExt.Lines.UserFields.Fields.Item(mc_strCodUnidad).Value = oCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value

                                    'se incorpora el total para el asiento de SE
                                    oAsientoServExt.Lines.Credit = dcTotalAcumulado_JE

                                    If blnAgregarDimension Then
                                        ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oAsientoServExt.Lines, Nothing, oDataTableDimensionesContablesDMS)
                                    End If

                                End If
                            End If
                            'Inicio: Asiento Contable para Articulos de tipos Otros 
                            '*********************************************************

                            Dim dtConfSucursal As System.Data.DataTable
                            Dim drwConfSucursal As System.Data.DataRow
                            Dim idSucursal As String = String.Empty

                            Dim blnCreaAsientoGastos As String = False
                            Dim strCuentaDebitaGastos As String = String.Empty
                            Dim strMonedaGastos As String = String.Empty

                            Dim intNoAsientoGastos As Integer = 0

                            idSucursal = oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value()
                            If idSucursal <> String.Empty Then

                                dtConfSucursal = Utilitarios.EjecutarConsultaDataTable(String.Format("Select U_Sucurs,U_CosteoMO_C, U_TiempoEst_C, U_TiempoReal_C, U_Moneda_C, U_CuentaSys_C, U_DescCuenta_C, U_GenASGastos,U_MonDocGastos,U_CtaDebGast From [@SCGD_CONF_SUCURSAL]Where U_Sucurs = '{0}'",
                                                                        idSucursal.Trim()),
                                                                        m_oCompany.CompanyDB,
                                                                        m_oCompany.Server)
                                If dtConfSucursal.Rows.Count > 0 Then
                                    drwConfSucursal = dtConfSucursal.Rows(0)

                                    'Costeo de Articulos Otros Gastos
                                    If drwConfSucursal.Item("U_GenASGastos").ToString.Trim() = "Y" Then
                                        strMonedaGastos = drwConfSucursal.Item("U_MonDocGastos").ToString.Trim()
                                        strCuentaDebitaGastos = drwConfSucursal.Item("U_CtaDebGast").ToString.Trim()
                                        If Not String.IsNullOrEmpty(strMonedaGastos) And Not String.IsNullOrEmpty(strCuentaDebitaGastos) Then
                                            blnCreaAsientoGastos = True
                                        Else
                                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ConfOtrosGastos, SAPbouiCOM.BoMessageTime.bmt_Short)
                                            blnCreaAsientoGastos = False
                                        End If
                                    Else
                                        blnCreaAsientoGastos = False
                                    End If
                                Else
                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ConfOtrosGastos, SAPbouiCOM.BoMessageTime.bmt_Short)
                                    blnCreaAsientoGastos = False
                                End If
                            End If

                            'If blnCreaAsientoGastos = True Then
                            '    intNoAsientoGastos = CrearAsientoOtrosGastos(m_oCompany, strMonedaGastos, strCuentaDebitaGastos, strContraCuenta, oCotizacion, String.Empty, strTransaccionLineas)
                            'End If

                            'FIN: Asiento Contable para Articulos de tipos Otros 
                            '*****************************************************


                            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, _
                                                                   m_oCompany.CompanyDB, _
                                                                   strConectionString)
                            cnConeccionBD = New SqlClient.SqlConnection
                            cnConeccionBD.ConnectionString = strConectionString
                            cnConeccionBD.Open()
                            tadFacturaSerie.Connection = New SqlClient.SqlConnection(strConectionString)
                            tadFactura.Connection = cnConeccionBD
                            udoEncFactura.Ano = oCotizacion.UserFields.Fields.Item(mc_strAño).Value


                            udoEncFactura.CardCode = oCotizacion.CardCode
                            udoEncFactura.CardName = oCotizacion.CardName
                            udoEncFactura.CodigoEstilo = oCotizacion.UserFields.Fields.Item(mc_strCod_Estilo).Value
                            udoEncFactura.CodigoMarca = oCotizacion.UserFields.Fields.Item(mc_strCod_Marca).Value
                            udoEncFactura.CodigoModelo = oCotizacion.UserFields.Fields.Item(mc_strCod_Modelo).Value
                            udoEncFactura.CodigoUnidad = oCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value
                            udoEncFactura.CodigoVehiculo = oCotizacion.UserFields.Fields.Item(mc_strNumVehiculo).Value
                            udoEncFactura.Moneda = strTipoMoneda

                            udoEncFactura.Monto = decTotalMonto

                            If p_strDECot <> "" Then
                                udoEncFactura.NoCotización = Convert.ToInt32(p_strDECot)
                            Else
                                udoEncFactura.NoCotización = oCotizacion.DocEntry
                                p_strDECot = oCotizacion.DocEntry
                            End If

                            udoEncFactura.NoOT = oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value
                            'udoEncFactura.NoDocumentoSalida = strIdSalida
                            udoEncFactura.Placa = oCotizacion.UserFields.Fields.Item(mc_strNum_Placa).Value
                            udoEncFactura.TipoOrden = strTipoOrden 'oCotizacion.UserFields.Fields.Item(mc_strTipoOT).Value
                            udoEncFactura.VIN = oCotizacion.UserFields.Fields.Item(mc_strVIN).Value

                            If oCotizacion.DocObjectCode = SAPbobsCOM.BoObjectTypes.oOrders Then
                                udoEncFactura.NumeroOrdenVenta = oCotizacion.DocEntry
                            End If

                            If Not String.IsNullOrEmpty(p_strDECot) Then
                                If m_oCotizacion.GetByKey(p_strDECot) Then
                                    '''''''''''Inicia Transaccion'''''''''''
                                    If Not m_oCompany.InTransaction Then
                                        m_oCompany.StartTransaction()
                                    End If
                                    '''''''''''Inicia Transaccion'''''''''''

                                    ''Crea Salida Mercancía
                                    If objItemsSalida.Count > 0 Then
                                        If oSalidaMercancia.Add() = 0 Then
                                            Call m_oCompany.GetNewObjectCode(strIdSalida)
                                        Else
                                            m_oCompany.GetLastError(intError, strError)
                                            Throw New ExceptionsSBO(intError, strError)
                                        End If
                                    End If

                                    ''Crea Asiento Contable
                                    If objItemsAsiento.Count > 0 Then
                                        If oAsientoContable.Add() = 0 Then
                                            Call m_oCompany.GetNewObjectCode(strIdAsiento)
                                        Else
                                            m_oCompany.GetLastError(intError, strError)
                                            Throw New ExceptionsSBO(intError, strError)
                                        End If
                                    End If

                                    ''Crea Asiento Servicios Externos
                                    If objItemsAsientoSE.Count > 0 And blnUsaAsientoSE Then
                                        If oAsientoServExt.Add() = 0 Then
                                            Call m_oCompany.GetNewObjectCode(strIdAsientoSE)
                                        Else
                                            m_oCompany.GetLastError(intError, strError)
                                            Throw New ExceptionsSBO(intError, strError)
                                        End If
                                    End If

                                    ''Crea Asientos otros 
                                    If blnCreaAsientoGastos = True Then
                                        intNoAsientoGastos = CrearAsientoOtrosGastos(m_oCompany, strMonedaGastos, strCuentaDebitaGastos, strContraCuenta, oCotizacion, String.Empty, strTransaccionLineas)
                                    End If

                                    udoFacturaInterna.Encabezado = New EncabezadoUDOFacturaInterna
                                    udoFacturaInterna.Encabezado = udoEncFactura

                                    If IsNumeric(strIdAsiento) Then
                                        udoEncFactura.Asiento = strIdAsiento
                                    Else
                                        udoEncFactura.Asiento = Nothing
                                    End If

                                    If intNoAsientoGastos > 0 Then
                                        udoEncFactura.AsientoGastos = intNoAsientoGastos
                                    Else
                                        udoEncFactura.AsientoGastos = 0
                                    End If

                                    udoEncFactura.NoDocumentoSalida = strIdSalida
                                    udoEncFactura.Asiento_SE = strIdAsientoSE

                                    If udoFacturaInterna.Insert() Then


                                        Dim cantLin As Integer = oCotizacion.Lines.Count
                                        Dim canLinSel = 0
                                        Dim chk As SAPbouiCOM.CheckBox

                                        If Not p_Matrix Is Nothing Then
                                            For index As Integer = 1 To p_Matrix.RowCount
                                                chk = DirectCast(p_Matrix.Columns.Item("col_Sel").Cells.Item(index).Specific, SAPbouiCOM.CheckBox)
                                                If chk.Checked Then
                                                    canLinSel += 1
                                                End If
                                            Next
                                        End If

                                        strDocEntry = udoFacturaInterna.Encabezado.DocEntry

                                        If Not p_Matrix Is Nothing Then
                                            For row As Integer = 0 To oCotizacion.Lines.Count - 1
                                                Call oCotizacion.Lines.SetCurrentLine(row)
                                                For index As Integer = 1 To p_Matrix.RowCount
                                                    chk = DirectCast(p_Matrix.Columns.Item("col_Sel").Cells.Item(index).Specific, SAPbouiCOM.CheckBox)
                                                    If chk.Checked Then
                                                        idLinea = DirectCast(p_Matrix.Columns.Item("col_IDLine").Cells.Item(index).Specific, SAPbouiCOM.EditText)
                                                        'El ID de la línea no puede estar en blanco
                                                        If Not String.IsNullOrEmpty(idLinea.Value.Trim) Then
                                                            If oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim = idLinea.Value.Trim Then
                                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoFin").Value = strDocEntry
                                                                'oCotizacion.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close
                                                                Exit For
                                                            End If
                                                        End If
                                                    End If
                                                Next
                                            Next
                                        End If
                                        ActualizaCotizacion(m_oCotizacion, strTipoOT)
                                        ActualizarDocumentoOrdenVenta(oCotizacion, strTipoOT, strDocEntry)
                                        If cantLin = canLinSel Then
                                            m_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = My.Resources.Resource.EstadoOrdenFacturada
                                            m_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "7"
                                        End If

                                        If m_oCotizacion.Update() = 0 Then
                                            blnContinuaProceso = True
                                        Else
                                            Call m_oCompany.GetLastError(intError, strError)
                                            If intError = -4013 Then
                                                blnContinuaProceso = True
                                            Else
                                                Call SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCode & Convert.ToString(intError) + ": " + My.Resources.Resource.LaCotizacionNo & oCotizacion.DocEntry & My.Resources.Resource.NoPudoCrear & strError, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                                                strDocEntry = -2
                                                If m_oCompany.InTransaction Then
                                                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                                End If
                                                Return False
                                            End If
                                        End If
                                        If oCotizacion.Update() = 0 And blnContinuaProceso Then

                                            If cantLin = canLinSel OrElse p_Matrix Is Nothing Then
                                                If oCotizacion.Close() = 0 Then
                                                    ActualizarOT(oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value, oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value, True, m_blnUsaConfiguracionInternaTaller, strTipoOT)
                                                    If m_oCompany.InTransaction Then
                                                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                                                        SBO_Application.StatusBar.SetText(String.Format(My.Resources.Resource.TXT_FactIntGen, strDocEntry), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                                                    End If
                                                    Return True
                                                Else
                                                    Call m_oCompany.GetLastError(intError, strError)
                                                    Call SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCode & Convert.ToString(intError) + ": " + My.Resources.Resource.LaCotizacionNo & oCotizacion.DocEntry & My.Resources.Resource.NoPudoCrear & strError, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                                                    strDocEntry = -2
                                                    If m_oCompany.InTransaction Then
                                                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                                    End If
                                                    Return False
                                                End If
                                            Else

                                                If Not p_Matrix Is Nothing Then
                                                    For row As Integer = 0 To oCotizacion.Lines.Count - 1
                                                        Call oCotizacion.Lines.SetCurrentLine(row)
                                                        For index As Integer = 1 To p_Matrix.RowCount
                                                            chk = DirectCast(p_Matrix.Columns.Item("col_Sel").Cells.Item(index).Specific, SAPbouiCOM.CheckBox)
                                                            If chk.Checked Then
                                                                idLinea = DirectCast(p_Matrix.Columns.Item("col_IDLine").Cells.Item(index).Specific, SAPbouiCOM.EditText)
                                                                'El ID de la línea no puede estar en blanco
                                                                If Not String.IsNullOrEmpty(idLinea.Value.Trim) Then
                                                                    If oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim = idLinea.Value.Trim Then
                                                                        'oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoFin").Value = strDocEntry
                                                                        oCotizacion.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close
                                                                        Exit For
                                                                    End If
                                                                End If
                                                            End If
                                                        Next
                                                    Next
                                                End If

                                                If oCotizacion.Update() = 0 Then
                                                    If m_oCompany.InTransaction Then
                                                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                                                        SBO_Application.StatusBar.SetText(String.Format(My.Resources.Resource.TXT_FactIntGen, strDocEntry), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                                                    End If
                                                Else
                                                    Call m_oCompany.GetLastError(intError, strError)
                                                    Call SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCode & Convert.ToString(intError) + ": " + My.Resources.Resource.LaCotizacionNo & oCotizacion.DocEntry & My.Resources.Resource.NoPudoCrear & strError, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                                                    If m_oCompany.InTransaction Then
                                                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                                    End If
                                                End If
                                                Return True
                                            End If
                                        Else
                                            Call m_oCompany.GetLastError(intError, strError)
                                            Call SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCode & Convert.ToString(intError) + ": " + My.Resources.Resource.LaCotizacionNo & oCotizacion.DocEntry & My.Resources.Resource.NoPudoCrear & strError, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                                            strDocEntry = -2
                                            If m_oCompany.InTransaction Then
                                                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                            End If
                                            Return False
                                        End If
                                    Else
                                        Call m_oCompany.GetLastError(intError, strError)
                                        Call SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCode & Convert.ToString(intError) + ": " + My.Resources.Resource.LaCotizacionNo & oCotizacion.DocEntry & My.Resources.Resource.NoPudoCrear & strError, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                                        If m_oCompany.InTransaction Then
                                            m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                        End If
                                        Return False
                                    End If
                                End If
                            End If
                        Else
                            If SBO_Application.MessageBox(My.Resources.Resource.CotizacionSinLineas, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                                Call oCotizacion.Close()
                                ActualizarOT(oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value, oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value, True, m_blnUsaConfiguracionInternaTaller)
                                If m_oCompany.InTransaction Then
                                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                                End If
                            Else
                                SBO_Application.MessageBox(My.Resources.Resource.RecuerdeCerrarCotizacion)
                            End If
                            strDocEntry = -2
                            Return False
                        End If
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeTiposinCuentaAsociada, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    End If
                End If
            Else
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorTipoMonedaNulo, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
            End If
        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        End Try

    End Function

    Public Function ActualizaCotizacion(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByRef nuevoTipoOT As String) As Boolean
        Try
            Dim strAntTipoOT As String = String.Empty
            If Not String.IsNullOrEmpty(nuevoTipoOT) Then
                strAntTipoOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value()
                p_oCotizacion.UserFields.Fields.Item("U_SCGD_AntTipoOT").Value() = strAntTipoOT
                p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value() = nuevoTipoOT
            End If
            p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = My.Resources.Resource.EstadoOrdenFacturada
            p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "7"
            p_oCotizacion.UserFields.Fields.Item("U_SCGD_FCierre").Value = System.DateTime.Now()
            p_oCotizacion.UserFields.Fields.Item("U_SCGD_FFact").Value = System.DateTime.Now()
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Function ActualizarDocumentoOrdenVenta(ByRef p_oOrdenVenta As SAPbobsCOM.Documents, ByRef nuevoTipoOT As String, ByRef p_strNoFI As String) As Boolean
        Try
            Dim strAntTipoOT As String = String.Empty
            If Not String.IsNullOrEmpty(nuevoTipoOT) Then
                strAntTipoOT = p_oOrdenVenta.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value()
                p_oOrdenVenta.UserFields.Fields.Item("U_SCGD_AntTipoOT").Value() = strAntTipoOT
                p_oOrdenVenta.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value() = nuevoTipoOT
                p_oOrdenVenta.UserFields.Fields.Item("U_SCGD_NoFI").Value() = p_strNoFI
            End If
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function



    ''' <summary>
    ''' retorna un item de tipo servicio externo para 
    ''' agregar a la lista de serv ext para cada linea del asiento
    ''' </summary>
    ''' <param name="strCuentaSE">Cuenta de tipo de orden</param>
    ''' <param name="dcTotalSE">total en moneda local</param>
    ''' <param name="dcTotalFrgSE">total en moneda sistema</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AgregaServExterno(ByVal strCuentaSE As String, ByVal dcTotalSE As Decimal, ByVal dcTotalFrgSE As Decimal) As ItemAsientoSE

        Dim objAsientoSE As New ItemAsientoSE

        objAsientoSE.strCuenta = strCuentaSE
        objAsientoSE.dcTotal = dcTotalSE
        objAsientoSE.dcTotalFrg = dcTotalFrgSE
        Return objAsientoSE

    End Function

    ''' <summary>
    ''' Retorna moneda local
    ''' </summary>
    ''' <returns>Retorna moneda local</returns>
    ''' <remarks></remarks>
    Public Function RetornarMonedaLocal() As String
        Dim oSBObob As SAPbobsCOM.SBObob
        Dim sToday As String
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim strResult As String

        Try

            oSBObob = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
            oRecordset = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            oRecordset = oSBObob.GetLocalCurrency()
            strResult = oRecordset.Fields.Item(0).Value

            Return strResult

        Catch ex As Exception
            Return -1
        End Try

    End Function

    Public Function RetornarMonedaSistema() As String
        Dim oSBObob As SAPbobsCOM.SBObob
        Dim sToday As String
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim strResult As String

        Try

            oSBObob = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
            oRecordset = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            oRecordset = oSBObob.GetSystemCurrency()
            strResult = oRecordset.Fields.Item(0).Value

            Return strResult

        Catch ex As Exception
            Return -1
        End Try

    End Function

    Private Function ObtenerCostoPorActividad(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByVal pvalEtiqueta As Integer, ByVal pvalNoOT As String, Optional ByVal p_IdSucursal As Integer = 0, Optional ByVal p_EsParcial As Boolean = False) As Decimal

        Dim Costo As Decimal = -1

        If Not m_blnUsaConfiguracionInternaTaller Then

            Select Case pvalEtiqueta

                Case TiposTiempo.PrecioCotizacion
                    Costo = Utilitarios.EjecutarConsultaPrecios(String.Format("SELECT SUM(QUT1.Price * QUT1.Quantity) FROM QUT1 with (nolock) " & _
                                        "INNER JOIN OQUT with (nolock) on OQUT.DocEntry = QUT1.DocEntry " & _
                                        "INNER JOIN OITM with (nolock) on OITM.Itemcode = QUT1.Itemcode " & _
                                        "WHERE(OITM.U_SCGD_TipoArticulo = 2) " & _
                                        "and QUT1.U_SCGD_Aprobado = 1 and OQUT.U_SCGD_Numero_OT = '{0}'", pvalNoOT), m_oCompany.CompanyDB, m_oCompany.Server)
                    Return Costo

                Case TiposTiempo.Estandar
                    Costo = Utilitarios.EjecutarConsultaPrecios("select CostoManoObraEst from [SCGTA_TB_Orden] with (nolock) where NoOrden = '" & pvalNoOT & "'", m_strBDTalller, m_oCompany.Server)
                    Return Costo

                Case TiposTiempo.Real
                    Costo = Utilitarios.EjecutarConsultaPrecios("select CostoManoObra from [SCGTA_TB_Orden] with (nolock) where NoOrden = '" & pvalNoOT & "'", m_strBDTalller, m_oCompany.Server)
                    Return Costo

            End Select

        Else

            Select Case pvalEtiqueta

                Case TiposTiempo.PrecioCotizacion
                    Costo = Utilitarios.EjecutarConsultaPrecios(String.Format("SELECT SUM(QUT1.Price * QUT1.Quantity) FROM QUT1 with (nolock) " & _
                                        "INNER JOIN OQUT with (nolock) on OQUT.DocEntry = QUT1.DocEntry " & _
                                        "INNER JOIN OITM with (nolock) on OITM.Itemcode = QUT1.Itemcode " & _
                                        "WHERE(OITM.U_SCGD_TipoArticulo = 2) " & _
                                        "and QUT1.U_SCGD_Aprobado = 1 and OQUT.U_SCGD_Numero_OT = '{0}'", pvalNoOT), m_oCompany.CompanyDB, m_oCompany.Server)
                    Return Costo

                Case TiposTiempo.Estandar
                    If p_EsParcial Then
                        Costo = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value
                    Else
                        Costo = Utilitarios.EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCostoManoObraCotizacion"), pvalNoOT), m_oCompany.CompanyDB, m_oCompany.Server)
                    End If
                    Return Costo

                Case TiposTiempo.Real
                    If p_EsParcial Then
                        Costo = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value
                    Else
                        Costo = Utilitarios.EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCostoManoObraCotizacion"), pvalNoOT), m_oCompany.CompanyDB, m_oCompany.Server)
                    End If
                    Return Costo

            End Select

        End If

        Return Costo

    End Function

    Private Function ObtenerCostoTotalActividades(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByVal pvalEtiqueta As Integer, ByRef p_Matrix As SAPbouiCOM.Matrix) As Decimal
        Dim Costo As Double = 0
        Dim chk As SAPbouiCOM.CheckBox
        Dim idLinea As EditText
        Try
            Select Case pvalEtiqueta
                Case TiposTiempo.PrecioCotizacion
                    For index As Integer = 1 To p_Matrix.RowCount
                        chk = DirectCast(p_Matrix.Columns.Item("col_Sel").Cells.Item(index).Specific, SAPbouiCOM.CheckBox)
                        If chk.Checked Then
                            idLinea = DirectCast(p_Matrix.Columns.Item("col_IDLine").Cells.Item(index).Specific, SAPbouiCOM.EditText)
                            If Not String.IsNullOrEmpty(idLinea.Value.Trim) Then
                                For intFila As Integer = 0 To p_oCotizacion.Lines.Count - 1
                                    p_oCotizacion.Lines.SetCurrentLine(intFila)
                                    If p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim = idLinea.Value.Trim Then
                                        Costo += p_oCotizacion.Lines.LineTotal
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    Next
                Case TiposTiempo.Estandar, TiposTiempo.Real
                    For index As Integer = 1 To p_Matrix.RowCount
                        chk = DirectCast(p_Matrix.Columns.Item("col_Sel").Cells.Item(index).Specific, SAPbouiCOM.CheckBox)
                        If chk.Checked Then
                            idLinea = DirectCast(p_Matrix.Columns.Item("col_IDLine").Cells.Item(index).Specific, SAPbouiCOM.EditText)
                            If Not String.IsNullOrEmpty(idLinea.Value.Trim) Then
                                For intFila As Integer = 0 To p_oCotizacion.Lines.Count - 1
                                    p_oCotizacion.Lines.SetCurrentLine(intFila)
                                    If p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim = idLinea.Value.Trim Then
                                        Costo += p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    Next
            End Select

            Return Costo
        Catch ex As Exception
            Return 0
        End Try
    End Function


    Public Sub CreaFacturaInternaUDO(ByVal dtrFactura As FacturaInternaDataset.SCG_FACTURAINTERNARow)
        Dim udoFI As UDOFacturaInterna = New UDOFacturaInterna(m_oCompany)
        udoFI.Encabezado = New EncabezadoUDOFacturaInterna()
        udoFI.Encabezado.DocNum = dtrFactura.DocNum
        udoFI.Encabezado.Ano = dtrFactura.U_Ano
        If Not dtrFactura.IsU_AsientoNull Then udoFI.Encabezado.Asiento = dtrFactura.U_Asiento
        udoFI.Encabezado.CardCode = dtrFactura.U_CardCode
        udoFI.Encabezado.CardName = dtrFactura.U_CardName
        udoFI.Encabezado.CodigoEstilo = dtrFactura.U_Cod_Esti
        udoFI.Encabezado.CodigoMarca = dtrFactura.U_Cod_Marc
        udoFI.Encabezado.CodigoModelo = dtrFactura.U_Cod_Mode
        udoFI.Encabezado.CodigoUnidad = dtrFactura.U_Cod_Unid
        udoFI.Encabezado.CodigoVehiculo = dtrFactura.U_ID_Vehi
        udoFI.Encabezado.Moneda = dtrFactura.U_Moneda
        udoFI.Encabezado.Monto = dtrFactura.U_Monto
        udoFI.Encabezado.NoCotización = dtrFactura.U_No_Cot
        udoFI.Encabezado.NoOT = dtrFactura.U_No_OT
        udoFI.Encabezado.NoDocumentoSalida = dtrFactura.U_No_Sal
        udoFI.Encabezado.Placa = dtrFactura.U_Placa
        udoFI.Encabezado.TipoOrden = dtrFactura.U_Tipo
        udoFI.Encabezado.VIN = dtrFactura.U_VIN
        udoFI.Encabezado.Asiento_SE = dtrFactura.U_Asien_SE
        udoFI.Encabezado.AsientoGastos = dtrFactura.U_AsientoGastos
        udoFI.Insert()
    End Sub

    Public Sub ManejadorEventoItemPressedGenFI(ByVal FormUID As String, _
                                               ByRef pVal As SAPbouiCOM.ItemEvent, _
                                               ByRef BubbleEvent As Boolean)
        Try

            Dim oMatrix As SAPbouiCOM.Matrix
            m_oFormGenCotizacion = SBO_Application.Forms.Item(pVal.FormUID)

            If Not m_oFormGenCotizacion Is Nothing _
                AndAlso pVal.ActionSuccess _
                AndAlso pVal.ItemUID = mc_strbtnGenerar Then


                oMatrix = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix)

                If Not oMatrix Is Nothing Then

                    Call RecorreLineasSeleccionadasFI(oMatrix, m_oFormGenCotizacion)

                    Call CargarMatrix(DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix), _
                                      DirectCast(m_oFormGenCotizacion.Items.Item(mc_stretNoAsesor).Specific, SAPbouiCOM.EditText).String, _
                                      m_oFormGenCotizacion, _
                                      m_dbCotizacion, True)

                End If

            ElseIf Not m_oFormGenCotizacion Is Nothing _
                    AndAlso pVal.ActionSuccess _
                    AndAlso (pVal.ItemUID = "btnCancel" OrElse pVal.ItemUID = "btClose") Then
                Call m_oFormGenCotizacion.Close()

            ElseIf Not m_oFormGenCotizacion Is Nothing _
            AndAlso pVal.ActionSuccess _
            AndAlso (pVal.ItemUID = "btnAct") Then

                m_dbCotizacion = m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strOQUT)

                oMatrix = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix)
                Call CargarMatrix(oMatrix, _
                                  DirectCast(m_oFormGenCotizacion.Items.Item(mc_stretNoAsesor).Specific, SAPbouiCOM.EditText).String, _
                                  m_oFormGenCotizacion, m_dbCotizacion, True)

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("ManejadorEventoItemPressedGenFI" & "" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Sub CargaFacturasInternas(ByRef oForm As SAPbouiCOM.Form, _
                                    ByVal strOrdenesCompraOR As String)


        Dim strOrdenesDeVenta As String = ""
        Dim oGrid As SAPbouiCOM.Grid
        Dim oEditTC As SAPbouiCOM.EditTextColumn

        Try

            strOrdenesDeVenta = "Select DocEntry '" & My.Resources.Resource.CapNoFacturaInterna & "',U_No_Cot '" & My.Resources.Resource.CapNoCotizacion & "'," & _
                                      " U_No_OT '" & My.Resources.Resource.CapNoOrdenTrabajo & "',U_CardCode '" & My.Resources.Resource.CapIDCliente & "'," & _
                                      " U_Cardname '" & My.Resources.Resource.CapCliente & "',U_Placa '" & My.Resources.Resource.CapPlaca & "'," & _
                                      " M.Name '" & My.Resources.Resource.CapMarca & "', E.Name '" & My.Resources.Resource.CapModelo & "'" & _
                                      " from dbo.[@SCGD_FACTURAINTERNA]" & _
                                        " inner join [@SCGD_MARCA] M" & _
                                            " on M.Code = U_Cod_Marc" & _
                                        " left outer join [@SCGD_Estilo] E" & _
                                            " on E.Code = U_Cod_Esti"

            oGrid = oForm.Items.Item(mc_strGridOV).Specific

            If oForm.DataSources.DataTables.Count < 1 Then
                Call oForm.DataSources.DataTables.Add("FInternas")
            End If

            strOrdenesDeVenta &= " Where " & strOrdenesCompraOR

            Call oForm.DataSources.DataTables.Item("FInternas").ExecuteQuery(strOrdenesDeVenta)

            oGrid.DataTable = oForm.DataSources.DataTables.Item("FInternas")

            oGrid.Columns.Item(0).Width = 80
            oGrid.Columns.Item(1).Width = 80
            oGrid.Columns.Item(2).Width = 80
            oGrid.Columns.Item(3).Width = 80
            oGrid.Columns.Item(4).Width = 120
            oGrid.Columns.Item(5).Width = 80
            oGrid.Columns.Item(6).Width = 80
            oGrid.Columns.Item(7).Width = 80

            oEditTC = oGrid.Columns.Item(0)

            oEditTC.LinkedObjectType = SAPbouiCOM.BoLinkedObject.lf_UserDefaults


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

        End Try

    End Sub

    Public Function DevolverIDFactura(ByVal p_intRow As Integer, _
                                        ByVal p_strIDForm As String, _
                                        ByVal p_strColumna As String) As String

        Dim oGrid As SAPbouiCOM.Matrix
        Dim strIDContrato As String

        oGrid = DirectCast(SBO_Application.Forms.Item(p_strIDForm).Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix)
        strIDContrato = SBO_Application.Forms.Item(p_strIDForm).DataSources.DataTables.Item("FInternas").GetValue(p_strColumna, p_intRow)

        Return strIDContrato

    End Function

    Public Sub DesActivarLinkBotton(ByVal p_strIDForm As String)
        Dim oGrid As SAPbouiCOM.Grid
        Dim oEditText As SAPbouiCOM.EditTextColumn

        oGrid = DirectCast(SBO_Application.Forms.Item(p_strIDForm).Items.Item(mc_strGridOV).Specific, SAPbouiCOM.Grid)
        oEditText = oGrid.Columns.Item(0)
        oEditText.LinkedObjectType = SAPbouiCOM.BoLinkedObject.lf_None

    End Sub

    Public Sub ActivarLinkBotton(ByVal p_strIDForm As String)
        Dim oGrid As SAPbouiCOM.Grid
        Dim oEditText As SAPbouiCOM.EditTextColumn

        oGrid = DirectCast(SBO_Application.Forms.Item(p_strIDForm).Items.Item(mc_strGridOV).Specific, SAPbouiCOM.Grid)
        oEditText = oGrid.Columns.Item(0)
        oEditText.LinkedObjectType = SAPbouiCOM.BoLinkedObject.lf_UserDefaults

    End Sub

    'Public Sub CrearSalidasSuministros(ByRef p_dstSuministros As SuministrosDataset, ByRef p_dstTransferencias As TransferenciasPorCotizacionDataSet, _
    '                                    ByRef p_oSalidaMercancia As SAPbobsCOM.Documents, ByVal p_cuenta As String, ByVal p_blnLineasAgregadas As Boolean)

    '    If p_dstSuministros.SCGTA_TB_SuministroxOrden.Rows.Count = 0 Then
    '        Exit Sub
    '    End If

    '    Dim m_drwTransferencias As TransferenciasPorCotizacionDataSet.TransferenciasPorCotizacionRow
    '    Dim m_drwSuministros As SuministrosDataset.SCGTA_TB_SuministroxOrdenRow

    '    Dim m_adpSeries As New SeriesLotesDataAdapter
    '    Dim m_dstSeries As New SeriesLotesDataSet
    '    Dim drwSeriesLotes As SeriesLotesDataSet.SeriesLotesRow

    '    Dim visOrder As Integer
    '    Dim cadenaConexion As String
    '    Dim nombreTabla As String = "WTR1"

    '    For Each m_drwTransferencias In p_dstTransferencias.TransferenciasPorCotizacion.Rows

    '        Dim m_oBuscarTransfer As SAPbobsCOM.StockTransfer
    '        Dim m_oLineasTransfer As SAPbobsCOM.StockTransfer_Lines

    '        m_oBuscarTransfer = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)

    '        If m_oBuscarTransfer.GetByKey(m_drwTransferencias.DocEntry) Then

    '            If m_oBuscarTransfer.UserFields.Fields.Item(mc_strUTipoTransferencia).Value = 1 Then

    '                m_oLineasTransfer = m_oBuscarTransfer.Lines

    '                ' For Each objItemSalida In objItemsSalida
    '                For i As Integer = 0 To m_oLineasTransfer.Count - 1

    '                    m_oLineasTransfer.SetCurrentLine(i)

    '                    'Dim intlin As Integer = m_oLineasTransfer.UserFields.Fields.Item("U_LinenumOrigen").Value

    '                    For Each m_drwSuministros In p_dstSuministros.SCGTA_TB_SuministroxOrden.Rows

    '                        If m_oLineasTransfer.UserFields.Fields.Item("U_LinenumOrigen").Value = m_drwSuministros.LineNumOriginal Then

    '                            If p_blnLineasAgregadas Then
    '                                p_oSalidaMercancia.Lines.Add()
    '                            Else
    '                                p_blnLineasAgregadas = True
    '                            End If

    '                            p_oSalidaMercancia.Lines.ItemCode = m_oLineasTransfer.ItemCode
    '                            p_oSalidaMercancia.Lines.WarehouseCode = m_oLineasTransfer.WarehouseCode
    '                            p_oSalidaMercancia.Lines.Quantity = m_oLineasTransfer.Quantity
    '                            p_oSalidaMercancia.Lines.AccountCode = p_cuenta

    '                            'cargo los lotes y series de cada linea de la transferencia
    '                            m_adpSeries.Fill_SeriesLotes(m_dstSeries, 67, m_drwTransferencias.DocEntry, "Y")
    '                            '****************************inicio Obtener VisOrder**********************

    '                            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, cadenaConexion)

    '                            visOrder = Utilitarios.ObtieneVisOrder(m_oCompany, nombreTabla, cadenaConexion, m_oLineasTransfer.LineNum, m_oLineasTransfer.ItemCode, m_drwTransferencias.DocEntry)
    '                            '****************************fin Obtener VisOrder**********************

    '                            'If m_dstSeries.SeriesLotes.Rows.Count = 0 Then
    '                            For Each drwSeriesLotes In m_dstSeries.SeriesLotes.Rows

    '                                If drwSeriesLotes.RowNoInBaseDocument = visOrder Then

    '                                    If drwSeriesLotes.IsBatchNumNull Then

    '                                        p_oSalidaMercancia.Lines.SerialNumbers.SystemSerialNumber = drwSeriesLotes.SysSerial
    '                                        p_oSalidaMercancia.Lines.SerialNumbers.InternalSerialNumber = drwSeriesLotes.SerialNumber
    '                                        p_oSalidaMercancia.Lines.SerialNumbers.ManufacturerSerialNumber = drwSeriesLotes.ManufacturerSerial
    '                                        p_oSalidaMercancia.Lines.SerialNumbers.Add()

    '                                    Else
    '                                        p_oSalidaMercancia.Lines.BatchNumbers.BatchNumber = drwSeriesLotes.BatchNum
    '                                        p_oSalidaMercancia.Lines.BatchNumbers.Quantity = drwSeriesLotes.Quantity
    '                                        p_oSalidaMercancia.Lines.BatchNumbers.Add()

    '                                    End If
    '                                End If

    '                            Next
    '                            m_dstSeries.Clear()

    '                        End If
    '                    Next
    '                Next
    '                'Next
    '            End If

    '        End If
    '    Next
    '    p_dstTransferencias.Clear()


    'End Sub


    Private Function ValidarTipoCambioMoneda(ByVal strTipoMoneda As String, Optional ByVal p_blnConfiguracionDMSInterno As Boolean = False) As Boolean

        Dim objBLSBO As BLSBO.GlobalFunctionsSBO
        objBLSBO = New BLSBO.GlobalFunctionsSBO
        Dim strMonedaLocal As String = String.Empty
        Dim strMonedasistema As String = String.Empty
        Dim decTipoCambio As Integer = 0

        Dim strTipoCambio As String = String.Empty
        Dim stoday As String = String.Empty

        strMonedaLocal = RetornarMonedaLocal()
        strMonedasistema = RetornarMonedaSistema()

        stoday = Utilitarios.RetornaFechaFormatoDB(Today, m_oCompany.Server)

        If p_blnConfiguracionDMSInterno Then

            strTipoCambio = Utilitarios.EjecutarConsulta(String.Format("SELECT Rate FROM dbo.[ORTT] WHERE Currency= '{0}' and RateDate= '{1}'",
                                                    strTipoMoneda, stoday), m_oCompany.CompanyDB, m_oCompany.Server)

        Else
            strTipoCambio = Utilitarios.EjecutarConsulta(String.Format("SELECT Rate FROM SCGTA_VW_ORTT WHERE Currency= '{0}' and RateDate= '{1}'",
                                                     strTipoMoneda, stoday), m_strBDTalller, m_oCompany.Server)

        End If

        'Valida que el tipo de cambio de la moneda en Configuracion Taller, no este vacio
        If strTipoCambio = "" And strMonedaLocal <> strTipoMoneda Then
            SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorTipoCambio & strTipoMoneda, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
            Return False
        End If

        Return True
    End Function

#End Region

#End Region


    'Creación de mano de obra
    Public Function CrearAsientoOtrosGastos(ByRef ocompany As SAPbobsCOM.Company,
                                        ByVal p_strMoneda As String, _
                                        ByVal p_strCuentaDebitaGastos As String, _
                                        ByVal p_strCuentaCreditoGastos As String, _
                                        ByVal p_oCotizacion As SAPbobsCOM.Documents, _
                                        Optional ByVal p_unidad As String = "", Optional ByVal p_codigoTransaccion As String = "") As Integer

        Dim oJournalEntry As SAPbobsCOM.JournalEntries
        Dim strMonedaLocal As String
        Dim intError As Integer
        Dim strMensajeError As String = ""
        Dim strAsientoGenerado As String = "0"
        Dim strItemCode As String = String.Empty
        Dim strTipoArticulo As String = String.Empty
        Dim strIDRepXOrden As String = String.Empty
        Dim decCosto As Decimal = 0
        Dim strNoOrden As String = String.Empty
        Dim strAlmacen As String = String.Empty
        Dim strCuentaCredito As String = String.Empty
        Dim strIDSucursal As String = String.Empty

        Dim strAprobado As String = String.Empty

        Dim oListaGastos As New List(Of ListaCuentasInterna)()

        Dim oListaAsientoGastos As New List(Of ListaCuentasInterna)()

        Utilitarios.DevuelveNombreBDTaller(SBO_Application, m_strBDTalller)

        'hago el llamado para cargar la configuracion de los documentos
        'que usaran Dimensiones
        If blnUsaDimensiones Then
            'ListaConfiguracionOT = New Hashtable
            ListaConfiguracionOT = New List(Of LineasConfiguracionOT)()
            ListaConfiguracionOT = ClsLineasDocumentosDimension.DatatableConfiguracionDocumentosDimensionesOT(m_oFormGenCotizacion)
        End If
        strIDSucursal = p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString.Trim()

        For i As Integer = 0 To p_oCotizacion.Lines.Count - 1
            p_oCotizacion.Lines.SetCurrentLine(i)
            strItemCode = p_oCotizacion.Lines.ItemCode.ToString.Trim()

            If Not String.IsNullOrEmpty(strItemCode) Then
                strTipoArticulo = DevuelveValorItemGastos(strItemCode, strSCGD_TipoArticulo).ToString.Trim()
                strAprobado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString.Trim()

                'El valor de TipoArticulo 11 es de Otros Gastos/Costos
                If Not String.IsNullOrEmpty(strTipoArticulo) And strTipoArticulo = "11" And Not String.IsNullOrEmpty(strAprobado) And strAprobado = "1" Then
                    strNoOrden = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim()

                    If Not String.IsNullOrEmpty(strNoOrden) Then
                        decCosto = Decimal.Parse(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString.Trim(), n)

                        strAlmacen = p_oCotizacion.Lines.WarehouseCode.ToString.Trim()

                        'strCuentaDebito = ObtenerCuentaItem(strItemCode, strAlmacen)
                        strCuentaCredito = p_strCuentaCreditoGastos
                        If Not String.IsNullOrEmpty(strCuentaCredito) Then
                            If decCosto > 0 Then
                                oListaGastos.Add(New ListaCuentasInterna() With {.NoOrden = strNoOrden, .Cuenta = strCuentaCredito, .Costo = decCosto, .Aplicado = False})
                            End If
                        Else
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                            Return 0
                        End If
                    End If
                End If
            End If
        Next

        If blnUsaDimensiones Then

            Dim strTipoOt As String = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value.ToString.Trim
            Dim strValorDimension As String = ClsLineasDocumentosDimension.ValidacionAsientosDimensiones(ListaConfiguracionOT, strTipoOt, False, False)
            '******************************************************************************************
            'lleno el datatable de dimensiones para el Sucursal y la marca del vehiculo
            If Not String.IsNullOrEmpty(strValorDimension) Then
                If strValorDimension = "Y" Then
                    Dim strCodigoMarca As String = p_oCotizacion.UserFields.Fields.Item(mc_strCod_Marca).Value
                    Dim strCodigoSucursal As String = p_oCotizacion.UserFields.Fields.Item(mc_strIdSucursal).Value
                    oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContablesOrdenTrabajo(m_oFormGenCotizacion, strCodigoSucursal, strCodigoMarca, oDataTableDimensionesContablesDMS))
                End If
            End If

            If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                blnAgregarDimension = True
            End If


            '******************************************************************************************
        End If

        Dim NoOrdenTemp As String = String.Empty
        Dim CuentaTemp As String = String.Empty
        Dim decMontoTemp As Decimal = 0
        Dim strCuentaDebitoTemp As String = String.Empty
        Dim blnAgregar As Boolean = False

        For Each C1 As ListaCuentasInterna In oListaGastos

            NoOrdenTemp = C1.NoOrden
            CuentaTemp = C1.Cuenta
            decMontoTemp = 0
            blnAgregar = False

            For Each C2 As ListaCuentasInterna In oListaGastos

                If C2.NoOrden = NoOrdenTemp And C2.Cuenta = CuentaTemp And C2.Aplicado = False Then
                    strCuentaDebitoTemp = C2.Cuenta
                    C2.Aplicado = True
                    decMontoTemp += C2.Costo
                    blnAgregar = True
                End If

            Next
            If blnAgregar = True And Not String.IsNullOrEmpty(NoOrdenTemp) And Not String.IsNullOrEmpty(CuentaTemp) And decMontoTemp > 0 Then
                oListaAsientoGastos.Add(New ListaCuentasInterna() With {.NoOrden = NoOrdenTemp, .Cuenta = CuentaTemp, .Costo = decMontoTemp, .Aplicado = True})
            End If
        Next

        If oListaAsientoGastos.Count() > 0 Then

            strAsientoGenerado = "0"

            strMonedaLocal = Utilitarios.EjecutarConsulta("Select mainCurncy from OADM", ocompany.CompanyDB, ocompany.Server)

            oJournalEntry = ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            oJournalEntry.Memo = My.Resources.Resource.AsientoOtrosGastos
            'oJournalEntry.UserFields.Fields.Item("U_SCGD_FacC").Value = p_oCotizacion.DocEntry.ToString.Trim()

            For Each row As ListaCuentasInterna In oListaAsientoGastos
                '*****************
                'Cuenta Debito
                '*****************

                'oJournalEntry.Lines.AccountCode = row.Cuenta
                oJournalEntry.Lines.AccountCode = p_strCuentaDebitaGastos
                oJournalEntry.Lines.ProjectCode = p_oCotizacion.Project

                If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                    If Not String.IsNullOrEmpty(strIDSucursal) Then oJournalEntry.Lines.BPLID = Integer.Parse(strIDSucursal)
                End If
                'If strMonedaLocal = p_strMoneda Then
                '    oJournalEntry.Lines.Debit = row.Costo
                'Else
                '    oJournalEntry.Lines.FCDebit = row.Costo
                '    oJournalEntry.Lines.FCCurrency = p_strMoneda

                'End If

                If strMonedaLocal = p_strMoneda Then
                    oJournalEntry.Lines.Credit = row.Costo
                Else
                    oJournalEntry.Lines.FCCredit = row.Costo
                    oJournalEntry.Lines.FCCurrency = p_strMoneda
                End If

                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = row.NoOrden
                oJournalEntry.Lines.Reference1 = row.NoOrden

                If blnAgregarDimension Then
                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)
                End If

                oJournalEntry.Lines.Add()

                '*********************
                ' Contra cuenta
                'Cuenta Credito
                '*********************
                oJournalEntry.Lines.AccountCode = row.Cuenta ' En este Caso aplica como cuenta de credito 
                'oJournalEntry.Lines.AccountCode = p_strCuentaCreditoGastos
                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = row.NoOrden
                oJournalEntry.Lines.Reference1 = row.NoOrden
                oJournalEntry.Lines.ProjectCode = p_oCotizacion.Project

                If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                    If Not String.IsNullOrEmpty(strIDSucursal) Then oJournalEntry.Lines.BPLID = Integer.Parse(strIDSucursal)
                End If

                If Not String.IsNullOrEmpty(p_codigoTransaccion) Then
                    oJournalEntry.Lines.UserFields.Fields.Item(mc_strTransaccion).Value = p_codigoTransaccion
                    oJournalEntry.Lines.UserFields.Fields.Item(mc_strCodUnidad).Value = p_oCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value
                End If


                'If strMonedaLocal = p_strMoneda Then
                '    oJournalEntry.Lines.Credit = row.Costo
                'Else
                '    oJournalEntry.Lines.FCCredit = row.Costo
                '    oJournalEntry.Lines.FCCurrency = p_strMoneda
                'End If

                If strMonedaLocal = p_strMoneda Then
                    oJournalEntry.Lines.Debit = row.Costo
                Else
                    oJournalEntry.Lines.FCDebit = row.Costo
                    oJournalEntry.Lines.FCCurrency = p_strMoneda

                End If

                If blnAgregarDimension Then
                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)
                End If

                oJournalEntry.Lines.Add()
            Next


            If oJournalEntry.Add <> 0 Then
                strAsientoGenerado = "0"
                ocompany.GetLastError(intError, strMensajeError)
                Throw New ExceptionsSBO(intError, strMensajeError)
            Else

                ocompany.GetNewObjectCode(strAsientoGenerado)
            End If
        End If

        Return CInt(strAsientoGenerado)

    End Function
    Public Function ObtenerCuentaItem(ByVal p_itemCode As String, ByVal strAlmacen As String) As String
        Try
            Dim oItemArticulo As SAPbobsCOM.IItems
            Dim cuentaContable As String = String.Empty


            oItemArticulo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            oItemArticulo.GetByKey(p_itemCode)

            'Almacen= SAPbobsCOM.BoGLMethods.glm_WH
            'Grupo de articulos= SAPbobsCOM.BoGLMethods.glm_ItemClass
            'Nivel de artículos= SAPbobsCOM.BoGLMethods.glm_ItemLevel

            Select Case oItemArticulo.GLMethod
                Case SAPbobsCOM.BoGLMethods.glm_WH
                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select SaleCostAc FROM OWHS Where WhsCode = '{0}'",
                                                        strAlmacen), m_oCompany.CompanyDB, m_oCompany.Server)
                    Return cuentaContable
                Case SAPbobsCOM.BoGLMethods.glm_ItemClass
                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select SaleCostAc From OITB Where ItmsGrpCod = '{0}'",
                                                        oItemArticulo.ItemsGroupCode.ToString.Trim()),
                                                        m_oCompany.CompanyDB,
                                                        m_oCompany.Server)
                    Return cuentaContable
                Case SAPbobsCOM.BoGLMethods.glm_ItemLevel

                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select SaleCostAc From OITW Where ItemCode= '{0}' WhsCode = '{1}'",
                                                        p_itemCode, strAlmacen), m_oCompany.CompanyDB, m_oCompany.Server)
                    Return cuentaContable
                Case Else
                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select SaleCostAc FROM OWHS Where WhsCode = '{0}'",
                                                        strAlmacen), m_oCompany.CompanyDB, m_oCompany.Server)
                    Return cuentaContable
            End Select

            Return cuentaContable
        Catch ex As Exception

        End Try
    End Function


    Private Function DevuelveValorItemGastos(ByVal strItemcode As String, _
                                      ByVal strUDfName As String) As String

        Dim oItemArticulo As SAPbobsCOM.IItems
        Dim valorUDF As String

        oItemArticulo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
        oItemArticulo.GetByKey(strItemcode)
        valorUDF = oItemArticulo.UserFields.Fields.Item(strUDfName).Value

        Return valorUDF

    End Function


#Region "Metodos Nuevos"
    Public Function CrearFacturasInternasNUEVO(ByRef p_intDocEntry As Integer, ByRef p_strDocEntryFacturaInterna As String)
        Dim oCotizacion As SAPbobsCOM.Documents
        Dim oSalidaMercancia As SAPbobsCOM.Documents
        '*************************Data Contract ***********************
        Dim oDocumentoCotizacion As oDocumento = New oDocumento()
        Dim oListServicios As List(Of oLineasDocumento) = New List(Of oLineasDocumento)
        Dim oListServicioExternos As List(Of oLineasDocumento) = New List(Of oLineasDocumento)
        Dim oListOtrosGastos As List(Of oLineasDocumento) = New List(Of oLineasDocumento)
        Dim oListRepuestoSuministro As List(Of oLineasDocumento) = New List(Of oLineasDocumento)
        Dim oListAsientoServicio As Asiento_List = New Asiento_List
        Dim oListAsientoServicioExterno As Asiento_List = New Asiento_List
        Dim oListAsientoOtrosGastos As Asiento_List = New Asiento_List
        '*************************UDOs ***********************
        Dim udoFacturaInterna As UDOFacturaInterna
        '*************************Variables ***********************
        Dim blnCreaSalidaMercancia As Boolean = False
        Dim intAsientoServicio As Integer = 0
        Dim intAsientOtrosGastos As Integer = 0
        Dim intAsientoServicioExterno As Integer = 0
        Dim blnMensajeServicioExitoso As Boolean = False
        Dim blnMensajeServicioExternoExitoso As Boolean = False
        Dim blnMensajeOtrosGastosExitoso As Boolean = False
        Dim strNumeroSalidaMercancia As String = String.Empty
        Dim intError As Integer = 0
        Dim strMensajeError As String = String.Empty
        Dim dblMontoTotal As Double = 0
        Dim blnError As Boolean = False
        Dim blnUsaDraft As Boolean = False
        Try
            udoFacturaInterna = New UDOFacturaInterna(m_oCompany)

            If p_intDocEntry > 0 Then
                oCotizacion = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations), SAPbobsCOM.Documents)
                If oCotizacion.GetByKey(p_intDocEntry) Then
                    CargarCotizacion(oCotizacion, oDocumentoCotizacion, oListServicios, oListServicioExternos, oListOtrosGastos, oListRepuestoSuministro, dblMontoTotal, blnError)
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoCotización + CStr(oDocumentoCotizacion.DocNum), SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    'Valida si usa documentos draf
                    blnUsaDraft = ValidaUsaDocumentosDraft(oDocumentoCotizacion.IDSucursal)
                    'Procesar salida mercancia
                    If oListRepuestoSuministro.Count > 0 Then
                        If CrearSalidaMercancia(oSalidaMercancia, oListRepuestoSuministro, oDocumentoCotizacion, blnError, blnUsaDraft) Then
                            blnCreaSalidaMercancia = True
                        Else
                            SBO_Application.StatusBar.SetText("Error generando Salida de Mercancía", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            Exit Function
                        End If
                    Else
                        blnCreaSalidaMercancia = False
                    End If
                    'Procesar creación de asientos
                    If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(x) x.U_Sucurs = oDocumentoCotizacion.IDSucursal).U_CosteoMO_I = "Y" Then
                        ProcesaAsientoManoObra(oListServicios, oListAsientoServicio, oDocumentoCotizacion, blnError)
                    End If

                    If DMS_Connector.Configuracion.ParamGenAddon.U_GenAsSE = "Y" Then ProcesaAsientoServiciosExternos(oListServicioExternos, oListAsientoServicioExterno, oDocumentoCotizacion, blnError)
                    If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(x) x.U_Sucurs = oDocumentoCotizacion.IDSucursal).U_GenASGastos = "Y" Then
                        ProcesaAsientoOtrosGastos(oListOtrosGastos, oListAsientoOtrosGastos, oDocumentoCotizacion, blnError)
                    End If

                    '** Si se produce error finaliza la funcion***
                    If blnError Then Exit Function
                    '****************Maneja transacción**************
                    ResetTransaction()
                    StartTransaction()
                    '************Crear Salida de mercancia***************
                    If blnCreaSalidaMercancia Then
                        If oSalidaMercancia.Add <> 0 Then
                            m_oCompany.GetLastError(intError, strMensajeError)
                            Throw New ExceptionsSBO(intError, strMensajeError)
                        Else
                            m_oCompany.GetNewObjectCode(strNumeroSalidaMercancia)
                        End If
                    End If
                    '************Verifica si genera asiento para servicio****************
                    If oListAsientoServicio.Count > 0 Then
                        intAsientoServicio = CrearAsiento(oListAsientoServicio, TipoArticulo.Servicio, oDocumentoCotizacion)
                        If intAsientoServicio > 0 Then
                            blnMensajeServicioExitoso = True
                        Else
                            RollbackTransaction()
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioError, SAPbouiCOM.BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error)
                            Exit Function
                        End If
                    Else
                        blnMensajeServicioExitoso = False
                    End If
                    '************Verifica si genera asiento para servicio externo****************
                    If oListAsientoServicioExterno.Count > 0 Then
                        intAsientoServicioExterno = CrearAsiento(oListAsientoServicioExterno, TipoArticulo.ServicioExterno, oDocumentoCotizacion)
                        If intAsientoServicioExterno > 0 Then
                            blnMensajeServicioExternoExitoso = True
                        Else
                            RollbackTransaction()
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioExternoError, SAPbouiCOM.BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error)
                            Exit Function
                        End If
                    Else
                        blnMensajeServicioExternoExitoso = False
                    End If
                    '************Verifica si genera asiento para otros gastos****************
                    If oListAsientoOtrosGastos.Count > 0 Then
                        intAsientOtrosGastos = CrearAsiento(oListAsientoOtrosGastos, TipoArticulo.OtrosCostos, oDocumentoCotizacion)
                        If intAsientOtrosGastos > 0 Then
                            blnMensajeOtrosGastosExitoso = True
                        Else
                            RollbackTransaction()
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoOtrosGastosError, SAPbouiCOM.BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error)
                            Exit Function
                        End If
                    Else
                        blnMensajeOtrosGastosExitoso = False
                    End If
                    If CrearUDOFacturaInterna(udoFacturaInterna, oDocumentoCotizacion, intAsientoServicio, intAsientoServicioExterno, intAsientOtrosGastos, strNumeroSalidaMercancia, p_strDocEntryFacturaInterna, dblMontoTotal, blnUsaDraft) Then
                        If ActualizaCotizacion(oDocumentoCotizacion, oCotizacion) Then
                            If ActualizarOrdenTrabajo(oDocumentoCotizacion) Then
                                'If p_blnCerrarExpedientes = True Then
                                '    CerrarExpediente(oCotizacion)
                                'End If
                                '*****************Realiza commit ala transaccion**************
                                CommitTransaction()
                                '*****************Mensaje asiento generado correctamente*****************
                                If blnMensajeServicioExitoso Then SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioExitoso, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                                If blnMensajeServicioExternoExitoso Then SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioExternoExitoso, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                                If blnMensajeOtrosGastosExitoso Then SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoOtrosGastosExitoso, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                            Else
                                RollbackTransaction()
                            End If
                        Else
                            RollbackTransaction()
                        End If
                    Else
                        RollbackTransaction()
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            RollbackTransaction()
        End Try
    End Function

    ' ''' <summary>
    ' ''' Cierra del expediente ligado a la oferta de ventas
    ' ''' </summary>
    ' ''' <param name="p_oCotizacion">Oferta de ventas a la cual se le desea cerrar el expediente</param>
    ' ''' <remarks></remarks>
    'Private Sub CerrarExpediente(ByRef p_oCotizacion As SAPbobsCOM.Documents)
    '    Dim oCompanyService As SAPbobsCOM.CompanyService
    '    Dim oGeneralService As SAPbobsCOM.GeneralService
    '    Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
    '    Dim oVehiculo As SAPbobsCOM.GeneralData
    '    Dim oChilds As SAPbobsCOM.GeneralDataCollection
    '    Dim oChildData As SAPbobsCOM.GeneralData
    '    Dim strIDExpediente As String = String.Empty
    '    Dim strDocEntryVehiculo As String = String.Empty

    '    Try
    '        strIDExpediente = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Exp").Value
    '        strDocEntryVehiculo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value

    '        If Not String.IsNullOrEmpty(strIDExpediente) And Not String.IsNullOrEmpty(strDocEntryVehiculo) Then
    '            'Se procede a cerrar el expediente
    '            oCompanyService = m_oCompany.GetCompanyService()
    '            oGeneralService = oCompanyService.GetGeneralService("SCGD_VEH")
    '            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
    '            oGeneralParams.SetProperty("Code", strDocEntryVehiculo)
    '            oVehiculo = oGeneralService.GetByParams(oGeneralParams)
    '            oChilds = oVehiculo.Child("SCGD_VEH_EXP_OT")
    '            'Recorre todas las líneas hijas, busca la que contenga el expediente y le cambia los estados correspondientes al cierre
    '            For Each oTmpChild As SAPbobsCOM.GeneralData In oChilds
    '                If oTmpChild.GetProperty("U_IDExp").ToString() = strIDExpediente Then
    '                    oTmpChild.SetProperty("U_Activo", "N")
    '                    oTmpChild.SetProperty("U_FechaCierre", Now())
    '                    oGeneralService.Update(oVehiculo)
    '                    Exit For
    '                End If
    '            Next
    '        End If
    '    Catch ex As Exception
    '        DMS_Connector.Helpers.ManejoErrores(ex)
    '    End Try
    'End Sub

    Public Function ActualizaCotizacion(ByRef p_oDocumentoCotizacion As oDocumento, ByRef p_oCotizacion As SAPbobsCOM.Documents) As Boolean
        Try
            If Not p_oCotizacion Is Nothing Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ActualizaCotizacion, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = My.Resources.Resource.EstadoOrdenFacturada
                p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "7"
                p_oCotizacion.UserFields.Fields.Item("U_SCGD_FCierre").Value = System.DateTime.Now()
                p_oCotizacion.UserFields.Fields.Item("U_SCGD_FFact").Value = System.DateTime.Now()
                If p_oCotizacion.Update() <> 0 Then
                    Return False
                End If
                If p_oCotizacion.Close() <> 0 Then
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function


    Private Function ActualizarOrdenTrabajo(ByRef p_oDocumentoCotizacion As oDocumento) As Boolean
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Try
            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_OT")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("Code", p_oDocumentoCotizacion.NoOrden)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            oGeneralData.SetProperty("U_FCerr", Date.Now)

            oGeneralData.SetProperty("U_FFact", Date.Now)
            oGeneralData.SetProperty("U_DEstO", My.Resources.Resource.Facturada)
            oGeneralData.SetProperty("U_EstO", "7")
            oGeneralService.Update(oGeneralData)
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Function ObtenerAlmacenxCentroCosto(ByVal p_strIDSucursal As String,
                                               ByVal p_strCentroCosto As String,
                                               ByRef p_strTipoCentroCosto As String) As String
        Try
            Dim strAlmacen As String = String.Empty
            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_strIDSucursal)) Then
                With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_strIDSucursal))
                    If .Bodegas_CentroCosto.Any(Function(bodegas) bodegas.U_CC.Trim().Equals(p_strCentroCosto)) Then
                        Select Case p_strTipoCentroCosto
                            Case Utilitarios.TipoCentroCosto.U_Rep
                                If Not String.IsNullOrEmpty(.Bodegas_CentroCosto.FirstOrDefault(Function(bodegas) bodegas.U_CC.Trim().Equals(p_strCentroCosto)).U_Rep.Trim()) Then strAlmacen = .Bodegas_CentroCosto.FirstOrDefault(Function(bodegas) bodegas.U_CC.Trim().Equals(p_strCentroCosto)).U_Rep.Trim()
                            Case Utilitarios.TipoCentroCosto.U_Ser
                                If Not String.IsNullOrEmpty(.Bodegas_CentroCosto.FirstOrDefault(Function(bodegas) bodegas.U_CC.Trim().Equals(p_strCentroCosto)).U_Ser.Trim()) Then strAlmacen = .Bodegas_CentroCosto.FirstOrDefault(Function(bodegas) bodegas.U_CC.Trim().Equals(p_strCentroCosto)).U_Ser.Trim()
                            Case Utilitarios.TipoCentroCosto.U_Sum
                                If Not String.IsNullOrEmpty(.Bodegas_CentroCosto.FirstOrDefault(Function(bodegas) bodegas.U_CC.Trim().Equals(p_strCentroCosto)).U_Sum.Trim()) Then strAlmacen = .Bodegas_CentroCosto.FirstOrDefault(Function(bodegas) bodegas.U_CC.Trim().Equals(p_strCentroCosto)).U_Sum.Trim()
                            Case Utilitarios.TipoCentroCosto.U_SE
                                If Not String.IsNullOrEmpty(.Bodegas_CentroCosto.FirstOrDefault(Function(bodegas) bodegas.U_CC.Trim().Equals(p_strCentroCosto)).U_SE.Trim()) Then strAlmacen = .Bodegas_CentroCosto.FirstOrDefault(Function(bodegas) bodegas.U_CC.Trim().Equals(p_strCentroCosto)).U_SE.Trim()
                            Case Utilitarios.TipoCentroCosto.U_Pro
                                If Not String.IsNullOrEmpty(.Bodegas_CentroCosto.FirstOrDefault(Function(bodegas) bodegas.U_CC.Trim().Equals(p_strCentroCosto)).U_Pro.Trim()) Then strAlmacen = .Bodegas_CentroCosto.FirstOrDefault(Function(bodegas) bodegas.U_CC.Trim().Equals(p_strCentroCosto)).U_Pro.Trim()
                        End Select
                    End If
                End With
            End If
            Return strAlmacen
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function


    Public Sub ValidaUsaDimensiones(ByVal p_oDocumentoCotizacion As oDocumento,
                                    ByRef p_blnUsaDimensiones As Boolean,
                                    ByRef p_blnUsaDimensionesOFV As Boolean)
        Try
            Dim strUsaDimensiones As String = String.Empty
            Dim strUsaDimensionesOFV As String = String.Empty
            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_oDocumentoCotizacion.IDSucursal)) Then
                With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_oDocumentoCotizacion.IDSucursal))
                    If .Configuracion_Tipo_Orden.Any(Function(tipoOT) tipoOT.U_Code.Equals(p_oDocumentoCotizacion.TipoOT)) Then
                        If Not String.IsNullOrEmpty(.Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(p_oDocumentoCotizacion.TipoOT)).U_UsaDim) Then strUsaDimensiones = .Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(p_oDocumentoCotizacion.TipoOT)).U_UsaDim
                        If Not String.IsNullOrEmpty(strUsaDimensiones) Then
                            If strUsaDimensiones = "Y" Then
                                p_blnUsaDimensiones = True
                            Else
                                p_blnUsaDimensiones = False
                            End If
                        Else
                            p_blnUsaDimensiones = False
                        End If
                        If Not String.IsNullOrEmpty(.Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(p_oDocumentoCotizacion.TipoOT)).U_UsaDOFV) Then strUsaDimensionesOFV = .Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(p_oDocumentoCotizacion.TipoOT)).U_UsaDOFV
                        If Not String.IsNullOrEmpty(strUsaDimensionesOFV) Then
                            If strUsaDimensionesOFV = "Y" Then
                                p_blnUsaDimensionesOFV = True
                            Else
                                p_blnUsaDimensionesOFV = False
                            End If
                        Else
                            p_blnUsaDimensionesOFV = False
                        End If
                    End If
                End With
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Function ValidaUsaDocumentosDraft(ByVal p_IDSucursal As String) As Boolean
        Try
            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_IDSucursal)) Then
                With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_IDSucursal))
                    If Not String.IsNullOrEmpty(.U_DraftD) Then
                        If .U_DraftD = "Y" Then Return True
                    End If
                End With
            End If
            Return False
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Function CrearSalidaMercancia(ByRef p_oSalidaMercancia As SAPbobsCOM.Documents,
                                         ByRef p_oListRepuestoSuministro As List(Of oLineasDocumento),
                                         ByVal p_oDocumentoCotizacion As oDocumento, ByRef p_blnError As Boolean, ByRef p_blnUsaDraft As Boolean) As Boolean
        Dim strTransaccionLineas As String = String.Empty
        Dim strCentroCostoTipoOT As String = String.Empty
        Dim strCentroCosto As String = String.Empty
        Dim strAlmacen As String = String.Empty
        Dim strContraCuenta As String = String.Empty
        Dim strUsaDimensiones As String = String.Empty
        Dim blnUsaDimensiones As Boolean = False
        Dim strUsaDimensionOFV As String = String.Empty
        Dim blnUsaDimensionesOFV As Boolean = False
        Dim oSerialNumbers As SAPbobsCOM.SerialNumbers
        Try
            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_oDocumentoCotizacion.IDSucursal.Trim())) Then
                With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_oDocumentoCotizacion.IDSucursal))
                    If .Configuracion_OT_Interna.Any(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString)) Then
                        If Not String.IsNullOrEmpty(.Configuracion_OT_Interna.FirstOrDefault(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString())).U_Tran_Com.Trim()) Then strTransaccionLineas = .Configuracion_OT_Interna.FirstOrDefault(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString())).U_Tran_Com.Trim()
                    End If
                    If .Configuracion_Tipo_Orden.Any(Function(tipoOT) tipoOT.U_Code.Equals(p_oDocumentoCotizacion.TipoOT)) Then
                        If Not String.IsNullOrEmpty(.Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(p_oDocumentoCotizacion.TipoOT)).U_CodCtCos) Then strCentroCostoTipoOT = .Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(p_oDocumentoCotizacion.TipoOT)).U_CodCtCos
                    End If
                End With
            End If
            If p_blnUsaDraft Then
                p_oSalidaMercancia = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts)
                p_oSalidaMercancia.DocObjectCodeEx = "60"
            Else
                p_oSalidaMercancia = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)
            End If
            'Usa dimensiones
            ValidaUsaDimensiones(p_oDocumentoCotizacion, blnUsaDimensiones, blnUsaDimensionesOFV)
            'Obtener cuenta contable
            strContraCuenta = CargarContraCuenta(p_oDocumentoCotizacion)
            With p_oDocumentoCotizacion
                If Not String.IsNullOrEmpty(.NoOrden) Then p_oSalidaMercancia.UserFields.Fields.Item(mc_strNum_OT).Value = .NoOrden
                If Not String.IsNullOrEmpty(.CodigoUnidad) Then p_oSalidaMercancia.UserFields.Fields.Item(mc_strNumUnidad).Value = .CodigoUnidad
                If Not String.IsNullOrEmpty(.CodigoProyecto) Then p_oSalidaMercancia.UserFields.Fields.Item(mc_strProyecto).Value = .CodigoProyecto
                If Not String.IsNullOrEmpty(strTransaccionLineas) Then
                    p_oSalidaMercancia.UserFields.Fields.Item(mc_strProcesad).Value = "1"
                Else
                    p_oSalidaMercancia.UserFields.Fields.Item(mc_strProcesad).Value = "2"
                End If
                p_oSalidaMercancia.UserFields.Fields.Item(mc_strNumVehiculo).Value = .NumeroVehiculo

                If p_blnUsaDraft Then
                    p_oSalidaMercancia.UserFields.Fields.Item("U_SCGD_Draft").Value = "Y"
                End If

                For Each rowRepSum As oLineasDocumento In p_oListRepuestoSuministro
                    p_oSalidaMercancia.Lines.ItemCode = rowRepSum.ItemCode
                    If Not String.IsNullOrEmpty(strCentroCostoTipoOT) Then
                        strCentroCosto = strCentroCostoTipoOT
                    Else
                        strCentroCosto = Utilitarios.DevuelveValorArticulo(rowRepSum.ItemCode, "U_SCGD_CodCtroCosto")
                    End If
                    If Not String.IsNullOrEmpty(strCentroCosto) Then
                        strAlmacen = ObtenerAlmacenxCentroCosto(.IDSucursal, strCentroCosto, Utilitarios.TipoCentroCosto.U_Pro)
                    End If
                    If Not String.IsNullOrEmpty(strAlmacen) Then p_oSalidaMercancia.Lines.WarehouseCode = strAlmacen
                    p_oSalidaMercancia.Lines.Quantity = rowRepSum.Quantity
                    If Not String.IsNullOrEmpty(.NoVisita) Then p_oSalidaMercancia.Reference2 = .NoOrden
                    If Not String.IsNullOrEmpty(strContraCuenta) Then p_oSalidaMercancia.Lines.AccountCode = strContraCuenta
                    If Not String.IsNullOrEmpty(.CodigoProyecto) Then p_oSalidaMercancia.Lines.ProjectCode = .CodigoProyecto
                    If blnUsaDimensiones Then
                        If blnUsaDimensionesOFV Then
                            If Not String.IsNullOrEmpty(rowRepSum.CostingCode) Then p_oSalidaMercancia.Lines.CostingCode = rowRepSum.CostingCode
                            If Not String.IsNullOrEmpty(rowRepSum.CostingCode2) Then p_oSalidaMercancia.Lines.CostingCode2 = rowRepSum.CostingCode2
                            If Not String.IsNullOrEmpty(rowRepSum.CostingCode3) Then p_oSalidaMercancia.Lines.CostingCode3 = rowRepSum.CostingCode3
                            If Not String.IsNullOrEmpty(rowRepSum.CostingCode4) Then p_oSalidaMercancia.Lines.CostingCode4 = rowRepSum.CostingCode4
                            If Not String.IsNullOrEmpty(rowRepSum.CostingCode5) Then p_oSalidaMercancia.Lines.CostingCode5 = rowRepSum.CostingCode5
                        Else
                            ClsLineasDocumentosDimension.AsignaDimensionesOTDocumento(p_oSalidaMercancia.Lines, p_oDocumentoCotizacion.IDSucursal, p_oDocumentoCotizacion.CodigoMarca)
                        End If
                    End If

                    If Not String.IsNullOrEmpty(rowRepSum.ID) Then
                        p_oSalidaMercancia.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = rowRepSum.ID
                        If Not String.IsNullOrEmpty(rowRepSum.Comprar) Then p_oSalidaMercancia.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = rowRepSum.Comprar
                        'CompletarSeriesArticulo(p_oSalidaMercancia.Lines.SerialNumbers, p_oSalidaMercancia.Lines.ItemCode, p_oSalidaMercancia.Lines.Quantity, p_oSalidaMercancia.Lines.UserFields.Fields.Item("U_SCGD_ID").Value, p_oSalidaMercancia.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value)
                    End If

                    p_oSalidaMercancia.Lines.Add()
                Next
            End With
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            p_blnError = True
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Completa las series de numeración en forma automática a partir de las series utilizadas en la requisición o mediante el proceso de compras
    ''' </summary>
    ''' <param name="p_oSerialNumbers">Objeto números de serie</param>
    ''' <param name="p_strItemCode">Código del artículo</param>
    ''' <param name="p_dblQuantity">Cantidad</param>
    ''' <param name="p_strIDActividad">ID interno de la línea de DMS</param>
    ''' <param name="p_strCompra">Y = Artículo comprado, N = Artículo transferido por medio de una requisición</param>
    ''' <remarks></remarks>
    Public Sub CompletarSeriesArticulo(ByRef p_oSerialNumbers As SAPbobsCOM.SerialNumbers, ByVal p_strItemCode As String, ByVal p_dblQuantity As Double, ByVal p_strIDActividad As String, ByVal p_strCompra As String)
        Dim strQuerySeries As String = String.Empty
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim intContador As Integer = 0
        Dim strSerieNumeracion As String = String.Empty
        Dim strSysNumber As String = String.Empty

        Try
            If Not String.IsNullOrEmpty(p_strIDActividad) Then
                If (p_strCompra.Equals("Y")) Then
                    'Consulta las series creadas en el proceso de compras
                    strQuerySeries = DMS_Connector.Queries.GetStrSpecificQuery("strConsultaSeriesXID")
                Else
                    'Consulta las series utilizadas durante la requisición
                    strQuerySeries = DMS_Connector.Queries.GetStrSpecificQuery("strConsultaSeriesRequisicion")
                End If

                strQuerySeries = String.Format(strQuerySeries, p_strIDActividad)

                oRecordset = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(strQuerySeries)

                While Not oRecordset.EoF And intContador < p_dblQuantity
                    'Agrega la serie
                    strSerieNumeracion = oRecordset.Fields.Item(0).Value.ToString()
                    strSysNumber = oRecordset.Fields.Item(1).Value.ToString()
                    p_oSerialNumbers.SystemSerialNumber = strSysNumber
                    p_oSerialNumbers.InternalSerialNumber = strSerieNumeracion
                    oRecordset.MoveNext()
                    p_oSerialNumbers.Add()
                    intContador += 1
                End While
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub



    Public Function ProcesaAsientoOtrosGastos(ByRef p_oListOtrosGastos As List(Of oLineasDocumento),
                                                  ByRef p_oLineaAsientoList As Asiento_List,
                                                  ByVal p_oDocumentoCotizacion As oDocumento,
                                                  ByRef p_blnError As Boolean) As Boolean
        Try
            '***********Data Contracts*********
            Dim oLineaAsiento As Asiento
            Dim oLineaAsientoTemporal As Asiento
            Dim oLineaAsientoTemporalList As Asiento_List = New Asiento_List
            '*****Variable***********
            Dim strCuentaDebito As String = String.Empty
            Dim strCuentaCredito As String = String.Empty
            Dim dblCosto As Double = 0
            Dim blnAgregar As Boolean = False
            Dim blnUsaDimensiones As Boolean = False
            Dim blnUsaDimensionesOFV As Boolean = False
            Dim strCentroCostoTipoOT As String = String.Empty
            Dim strTransaccionLineas As String = String.Empty
            Dim strMonedaGastos As String = String.Empty
            '*************Recorre lineas ServicioList*****************
            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(x) x.U_Sucurs = p_oDocumentoCotizacion.IDSucursal) Then
                With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_oDocumentoCotizacion.IDSucursal))
                    If .Configuracion_OT_Interna.Any(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString())) Then
                        If Not String.IsNullOrEmpty(.Configuracion_OT_Interna.FirstOrDefault(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString())).U_Tran_Com.Trim()) Then strTransaccionLineas = .Configuracion_OT_Interna.FirstOrDefault(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString())).U_Tran_Com.Trim()
                    End If
                    If .Configuracion_Tipo_Orden.Any(Function(tipoOT) tipoOT.U_Code.Equals(p_oDocumentoCotizacion.TipoOT)) Then
                        If Not String.IsNullOrEmpty(.Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(p_oDocumentoCotizacion.TipoOT)).U_CodCtCos) Then strCentroCostoTipoOT = .Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(p_oDocumentoCotizacion.TipoOT)).U_CodCtCos
                    End If
                    If .U_GenASGastos.Trim() = "Y" Then
                        strMonedaGastos = .U_MonDocGastos.Trim()
                        strCuentaCredito = .U_CtaDebGast.Trim()
                    End If
                End With

                'Carga cuenta de débito
                strCuentaDebito = CargarContraCuenta(p_oDocumentoCotizacion)
                If String.IsNullOrEmpty(strCuentaDebito) Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                    Return False
                End If
            End If
            'Usa Dimensiones
            ValidaUsaDimensiones(p_oDocumentoCotizacion, blnUsaDimensiones, blnUsaDimensionesOFV)
            For Each rowOtrosGastos As oLineasDocumento In p_oListOtrosGastos
                oLineaAsientoTemporal = New Asiento
                With oLineaAsientoTemporal
                    .NoOrden = rowOtrosGastos.NoOrden
                    .Costo = rowOtrosGastos.Costo
                    .Moneda = Nothing
                    '******Cuenta debito y cuenta credito************
                    .CuentaDebito = strCuentaDebito
                    .CuentaCredito = strCuentaCredito
                    If Not String.IsNullOrEmpty(strTransaccionLineas) Then
                        .U_SCGD_Cod_Tran = strTransaccionLineas
                    End If
                    If blnUsaDimensiones Then
                        .UsaDimensiones = True
                        If blnUsaDimensionesOFV Then
                            If Not String.IsNullOrEmpty(rowOtrosGastos.CostingCode) Then .CostingCode = rowOtrosGastos.CostingCode
                            If Not String.IsNullOrEmpty(rowOtrosGastos.CostingCode2) Then .CostingCode2 = rowOtrosGastos.CostingCode2
                            If Not String.IsNullOrEmpty(rowOtrosGastos.CostingCode3) Then .CostingCode3 = rowOtrosGastos.CostingCode3
                            If Not String.IsNullOrEmpty(rowOtrosGastos.CostingCode4) Then .CostingCode4 = rowOtrosGastos.CostingCode4
                            If Not String.IsNullOrEmpty(rowOtrosGastos.CostingCode5) Then .CostingCode5 = rowOtrosGastos.CostingCode5
                        Else
                            ClsLineasDocumentosDimension.AsignaDimensionesOTAsiento(oLineaAsientoTemporal, p_oDocumentoCotizacion.IDSucursal, p_oDocumentoCotizacion.CodigoMarca)
                        End If
                    End If
                End With
                oLineaAsientoTemporalList.Add(oLineaAsientoTemporal)
            Next
            ' Recorre lineas de objeto temporal para agrupar el definitivo
            For Each rowAsiento1 As Asiento In oLineaAsientoTemporalList
                dblCosto = 0
                blnAgregar = False
                For Each rowAsiento2 As Asiento In oLineaAsientoTemporalList
                    If rowAsiento2.CuentaCredito = rowAsiento1.CuentaCredito And rowAsiento2.CostingCode = rowAsiento1.CostingCode And rowAsiento2.CostingCode2 = rowAsiento1.CostingCode2 And rowAsiento2.CostingCode3 = rowAsiento1.CostingCode3 And rowAsiento2.CostingCode4 = rowAsiento1.CostingCode4 And rowAsiento2.CostingCode5 = rowAsiento1.CostingCode5 And rowAsiento2.Aplicado = False Then
                        dblCosto += rowAsiento2.Costo
                        rowAsiento2.Aplicado = True
                        If dblCosto > 0 Then
                            blnAgregar = True
                        End If
                    End If
                Next
                If blnAgregar Then
                    oLineaAsiento = New Asiento
                    With oLineaAsiento
                        .NoOrden = rowAsiento1.NoOrden
                        .CuentaDebito = rowAsiento1.CuentaDebito
                        .CuentaCredito = rowAsiento1.CuentaCredito
                        .Costo = dblCosto
                        .Moneda = rowAsiento1.Moneda
                        .U_SCGD_Cod_Tran = rowAsiento1.U_SCGD_Cod_Tran
                        If rowAsiento1.UsaDimensiones Then
                            .UsaDimensiones = True
                            .CostingCode = rowAsiento1.CostingCode
                            .CostingCode2 = rowAsiento1.CostingCode2
                            .CostingCode3 = rowAsiento1.CostingCode3
                            .CostingCode4 = rowAsiento1.CostingCode4
                            .CostingCode5 = rowAsiento1.CostingCode5
                        End If
                    End With
                    p_oLineaAsientoList.Add(oLineaAsiento)
                End If
            Next
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            p_blnError = True
            Return False
        End Try
    End Function

    Public Function ProcesaAsientoServiciosExternos(ByRef p_oListServiciosExternos As List(Of oLineasDocumento),
                                                    ByRef p_oLineaAsientoList As Asiento_List,
                                                    ByVal p_oDocumentoCotizacion As oDocumento,
                                                    ByRef p_blnError As Boolean) As Boolean
        Try
            '***********Data Contracts*********
            Dim oLineaAsiento As Asiento
            Dim oLineaAsientoTemporal As Asiento
            Dim oLineaAsientoTemporalList As Asiento_List = New Asiento_List
            '*****Variable***********
            Dim strCuentaDebito As String = String.Empty
            Dim strCuentaCredito As String = String.Empty
            Dim dblCosto As Double = 0
            Dim blnAgregar As Boolean = False
            Dim intTipoCosto As Integer = 0
            Dim blnUsaDimensiones As Boolean = False
            Dim blnUsaDimensionesOFV As Boolean = False
            Dim strCentroCostoTipoOT As String = String.Empty
            Dim strTransaccionLineas As String = String.Empty
            Dim strCentroCosto As String = String.Empty
            Dim strAlmacen As String = String.Empty
            '*************Recorre lineas ServicioList*****************
            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(x) x.U_Sucurs = p_oDocumentoCotizacion.IDSucursal) Then
                With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_oDocumentoCotizacion.IDSucursal))
                    If .Configuracion_OT_Interna.Any(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString())) Then
                        If Not String.IsNullOrEmpty(.Configuracion_OT_Interna.FirstOrDefault(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString())).U_Tran_Com.Trim()) Then strTransaccionLineas = .Configuracion_OT_Interna.FirstOrDefault(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString())).U_Tran_Com.Trim()
                    End If
                    If .Configuracion_Tipo_Orden.Any(Function(tipoOT) tipoOT.U_Code.Equals(p_oDocumentoCotizacion.TipoOT)) Then
                        If Not String.IsNullOrEmpty(.Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(p_oDocumentoCotizacion.TipoOT)).U_CodCtCos) Then strCentroCostoTipoOT = .Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(p_oDocumentoCotizacion.TipoOT)).U_CodCtCos
                    End If
                End With
                'Carga cuenta de débito
                strCuentaDebito = CargarContraCuenta(p_oDocumentoCotizacion)
                If String.IsNullOrEmpty(strCuentaDebito) Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                    Return False
                End If
            End If
            'Usa Dimensiones
            ValidaUsaDimensiones(p_oDocumentoCotizacion, blnUsaDimensiones, blnUsaDimensionesOFV)
            For Each rowServicioExterno As oLineasDocumento In p_oListServiciosExternos
                oLineaAsientoTemporal = New Asiento
                With oLineaAsientoTemporal
                    .NoOrden = rowServicioExterno.NoOrden
                    .Costo = rowServicioExterno.Costo
                    .Moneda = Nothing
                    '******Cuenta debito y cuenta credito************
                    .CuentaDebito = strCuentaDebito
                    If Not String.IsNullOrEmpty(strCentroCostoTipoOT) Then
                        strCentroCosto = strCentroCostoTipoOT
                    Else
                        strCentroCosto = rowServicioExterno.CentroCosto
                    End If
                    If Not String.IsNullOrEmpty(strCentroCosto) Then
                        strAlmacen = ObtenerAlmacenxCentroCosto(p_oDocumentoCotizacion.IDSucursal, strCentroCosto, Utilitarios.TipoCentroCosto.U_Pro)
                        If Not String.IsNullOrEmpty(strAlmacen) Then
                            strCuentaCredito = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.ExpensesAc, p_oDocumentoCotizacion.IDSucursal, strAlmacen)
                            If Not String.IsNullOrEmpty(strCuentaCredito) Then .CuentaCredito = strCuentaCredito
                        End If
                    End If
                    If Not String.IsNullOrEmpty(strTransaccionLineas) Then
                        .U_SCGD_Cod_Tran = strTransaccionLineas
                    End If
                    If blnUsaDimensiones Then
                        .UsaDimensiones = True
                        If blnUsaDimensionesOFV Then
                            If Not String.IsNullOrEmpty(rowServicioExterno.CostingCode) Then .CostingCode = rowServicioExterno.CostingCode
                            If Not String.IsNullOrEmpty(rowServicioExterno.CostingCode2) Then .CostingCode2 = rowServicioExterno.CostingCode2
                            If Not String.IsNullOrEmpty(rowServicioExterno.CostingCode3) Then .CostingCode3 = rowServicioExterno.CostingCode3
                            If Not String.IsNullOrEmpty(rowServicioExterno.CostingCode4) Then .CostingCode4 = rowServicioExterno.CostingCode4
                            If Not String.IsNullOrEmpty(rowServicioExterno.CostingCode5) Then .CostingCode5 = rowServicioExterno.CostingCode5
                        Else
                            ClsLineasDocumentosDimension.AsignaDimensionesOTAsiento(oLineaAsientoTemporal, p_oDocumentoCotizacion.IDSucursal, p_oDocumentoCotizacion.CodigoMarca)
                        End If
                    End If
                End With
                oLineaAsientoTemporalList.Add(oLineaAsientoTemporal)
            Next
            ' Recorre lineas de objeto temporal para agrupar el definitivo
            For Each rowAsiento1 As Asiento In oLineaAsientoTemporalList
                dblCosto = 0
                blnAgregar = False
                For Each rowAsiento2 As Asiento In oLineaAsientoTemporalList
                    If rowAsiento2.CuentaCredito = rowAsiento1.CuentaCredito And rowAsiento2.CostingCode = rowAsiento1.CostingCode And rowAsiento2.CostingCode2 = rowAsiento1.CostingCode2 And rowAsiento2.CostingCode3 = rowAsiento1.CostingCode3 And rowAsiento2.CostingCode4 = rowAsiento1.CostingCode4 And rowAsiento2.CostingCode5 = rowAsiento1.CostingCode5 And rowAsiento2.Aplicado = False Then
                        dblCosto += rowAsiento2.Costo
                        rowAsiento2.Aplicado = True
                        If dblCosto > 0 Then
                            blnAgregar = True
                        End If
                    End If
                Next
                If blnAgregar Then
                    oLineaAsiento = New Asiento
                    With oLineaAsiento
                        .NoOrden = rowAsiento1.NoOrden
                        .CuentaDebito = rowAsiento1.CuentaDebito
                        .CuentaCredito = rowAsiento1.CuentaCredito
                        .Costo = dblCosto
                        .Moneda = rowAsiento1.Moneda
                        .Proyecto = p_oDocumentoCotizacion.CodigoProyecto
                        .U_SCGD_Cod_Tran = rowAsiento1.U_SCGD_Cod_Tran
                        If rowAsiento1.UsaDimensiones Then
                            .UsaDimensiones = True
                            .CostingCode = rowAsiento1.CostingCode
                            .CostingCode2 = rowAsiento1.CostingCode2
                            .CostingCode3 = rowAsiento1.CostingCode3
                            .CostingCode4 = rowAsiento1.CostingCode4
                            .CostingCode5 = rowAsiento1.CostingCode5
                        End If
                    End With
                    p_oLineaAsientoList.Add(oLineaAsiento)
                End If
            Next
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            p_blnError = True
            Return False
        End Try
    End Function

    Public Function ProcesaAsientoManoObra(ByRef p_oListServicios As List(Of oLineasDocumento),
                                           ByRef p_oLineaAsientoList As Asiento_List,
                                           ByVal p_oDocumentoCotizacion As oDocumento,
                                           ByRef p_blnError As Boolean) As Boolean
        Try
            '***********Data Contracts*********
            Dim oLineaAsiento As Asiento
            Dim oLineaAsientoTemporal As Asiento
            Dim oLineaAsientoTemporalList As Asiento_List = New Asiento_List
            '*****Variable***********
            Dim strCuentaDebito As String = String.Empty
            Dim strCuentaCredito As String = String.Empty
            Dim dblCosto As Double = 0
            Dim blnAgregar As Boolean = False
            Dim intTipoCosto As Integer = 0
            Dim blnUsaDimensiones As Boolean = False
            Dim blnUsaDimensionesOFV As Boolean = False
            Dim strCodigoTransaccion As String = String.Empty
            '*************Recorre lineas ServicioList*****************
            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(x) x.U_Sucurs = p_oDocumentoCotizacion.IDSucursal) Then
                If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(x) x.U_Sucurs = p_oDocumentoCotizacion.IDSucursal).U_TiempoEst_C = "Y" Then
                    intTipoCosto = TiposTiempo.Estandar
                ElseIf DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(x) x.U_Sucurs = p_oDocumentoCotizacion.IDSucursal).U_TiempoReal_C = "Y" Then
                    intTipoCosto = TiposTiempo.Real
                ElseIf DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(x) x.U_Sucurs = p_oDocumentoCotizacion.IDSucursal).U_TiempoOFV_C = "Y" Then
                    intTipoCosto = TiposTiempo.PrecioCotizacion
                End If
                'Cargar cuenta credito
                If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(x) x.U_Sucurs = p_oDocumentoCotizacion.IDSucursal).U_CuentaSys_C) Then
                    strCuentaCredito = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(x) x.U_Sucurs = p_oDocumentoCotizacion.IDSucursal).U_CuentaSys_C.ToString.Trim()
                Else
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaCreditoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                    Return False
                End If
                'Carga cuenta de débito
                strCuentaDebito = CargarContraCuenta(p_oDocumentoCotizacion)
                If String.IsNullOrEmpty(strCuentaDebito) Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                    Return False
                End If
                'Carga Transaction Costos
                With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_oDocumentoCotizacion.IDSucursal))
                    If .Configuracion_OT_Interna.Any(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString())) Then
                        If Not String.IsNullOrEmpty(.Configuracion_OT_Interna.FirstOrDefault(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString())).U_Tran_Com.Trim()) Then strCodigoTransaccion = .Configuracion_OT_Interna.FirstOrDefault(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString())).U_Tran_Com.Trim()
                    End If
                End With
            End If
            'Usa Dimensiones
            ValidaUsaDimensiones(p_oDocumentoCotizacion, blnUsaDimensiones, blnUsaDimensionesOFV)
            For Each rowServicio As oLineasDocumento In p_oListServicios
                oLineaAsientoTemporal = New Asiento
                With oLineaAsientoTemporal
                    .NoOrden = rowServicio.NoOrden
                    If intTipoCosto = TiposTiempo.PrecioCotizacion Then
                        .Costo = rowServicio.Price * rowServicio.Quantity
                    Else
                        .Costo = rowServicio.Costo
                    End If
                    If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(x) x.U_Sucurs = p_oDocumentoCotizacion.IDSucursal).U_Moneda_C) Then
                        .Moneda = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(x) x.U_Sucurs = p_oDocumentoCotizacion.IDSucursal).U_Moneda_C.Trim()
                    Else
                        .Moneda = Nothing
                    End If
                    '******Cuenta debito y cuenta credito************
                    .CuentaDebito = strCuentaDebito
                    .CuentaCredito = strCuentaCredito
                    If blnUsaDimensiones Then
                        .UsaDimensiones = True
                        If blnUsaDimensionesOFV Then
                            If Not String.IsNullOrEmpty(rowServicio.CostingCode) Then .CostingCode = rowServicio.CostingCode
                            If Not String.IsNullOrEmpty(rowServicio.CostingCode2) Then .CostingCode2 = rowServicio.CostingCode2
                            If Not String.IsNullOrEmpty(rowServicio.CostingCode3) Then .CostingCode3 = rowServicio.CostingCode3
                            If Not String.IsNullOrEmpty(rowServicio.CostingCode4) Then .CostingCode4 = rowServicio.CostingCode4
                            If Not String.IsNullOrEmpty(rowServicio.CostingCode5) Then .CostingCode5 = rowServicio.CostingCode5
                        Else
                            ClsLineasDocumentosDimension.AsignaDimensionesOTAsiento(oLineaAsientoTemporal, p_oDocumentoCotizacion.IDSucursal, p_oDocumentoCotizacion.CodigoMarca)
                        End If
                    End If
                End With
                oLineaAsientoTemporalList.Add(oLineaAsientoTemporal)
            Next
            ' Recorre lineas de objeto temporal para agrupar el definitivo
            For Each rowAsiento1 As Asiento In oLineaAsientoTemporalList
                dblCosto = 0
                blnAgregar = False
                For Each rowAsiento2 As Asiento In oLineaAsientoTemporalList
                    If rowAsiento2.CostingCode = rowAsiento1.CostingCode And rowAsiento2.CostingCode2 = rowAsiento1.CostingCode2 And rowAsiento2.CostingCode3 = rowAsiento1.CostingCode3 And rowAsiento2.CostingCode4 = rowAsiento1.CostingCode4 And rowAsiento2.CostingCode5 = rowAsiento1.CostingCode5 And rowAsiento2.Aplicado = False Then
                        dblCosto += rowAsiento2.Costo
                        rowAsiento2.Aplicado = True
                        If dblCosto > 0 Then
                            blnAgregar = True
                        End If
                    End If
                Next
                If blnAgregar Then
                    oLineaAsiento = New Asiento
                    With oLineaAsiento
                        .NoOrden = rowAsiento1.NoOrden
                        .CuentaDebito = rowAsiento1.CuentaDebito
                        .CuentaCredito = rowAsiento1.CuentaCredito
                        .Costo = dblCosto
                        .Proyecto = p_oDocumentoCotizacion.CodigoProyecto
                        .Moneda = rowAsiento1.Moneda
                        If Not String.IsNullOrEmpty(strCodigoTransaccion) Then .U_SCGD_Cod_Tran = strCodigoTransaccion
                        If rowAsiento1.UsaDimensiones Then
                            .UsaDimensiones = True
                            .CostingCode = rowAsiento1.CostingCode
                            .CostingCode2 = rowAsiento1.CostingCode2
                            .CostingCode3 = rowAsiento1.CostingCode3
                            .CostingCode4 = rowAsiento1.CostingCode4
                            .CostingCode5 = rowAsiento1.CostingCode5
                        End If
                    End With
                    p_oLineaAsientoList.Add(oLineaAsiento)
                End If
            Next
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            p_blnError = True
            Return False
        End Try
    End Function

    Public Function CrearAsiento(ByRef p_oAsientoList As Asiento_List, _
                                 ByRef p_intTipoArticulo As Integer,
                                 ByRef p_oDocumentoCotizacion As oDocumento) As Integer
        Try
            '************Objetos*********************
            Dim oJournalEntry As SAPbobsCOM.JournalEntries
            '************Variables*******************
            Dim intAsientoGenerado As Integer = 0
            Dim strAsientoGenerado As String = String.Empty
            Dim strMonedaLocal As String = String.Empty
            Dim intError As Integer = 0
            Dim strMensajeError As String = String.Empty
            Dim strNoOrden As String = String.Empty

            strMonedaLocal = DMS_Connector.Company.AdminInfo.LocalCurrency
            If p_oAsientoList.Count > 0 Then
                oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                If Not String.IsNullOrEmpty(p_oDocumentoCotizacion.NoOrden) Then
                    oJournalEntry.Reference = p_oDocumentoCotizacion.NoOrden
                    oJournalEntry.Reference3 = p_oDocumentoCotizacion.NoOrden
                End If

                Select Case p_intTipoArticulo
                    Case TipoArticulo.Servicio
                        oJournalEntry.Memo = My.Resources.Resource.AsientoManoObra + ": " + p_oDocumentoCotizacion.NoOrden()
                    Case TipoArticulo.ServicioExterno
                        oJournalEntry.Memo = My.Resources.Resource.AsientoServiciosExternos + p_oDocumentoCotizacion.NoOrden()
                    Case TipoArticulo.OtrosCostos
                        oJournalEntry.Memo = My.Resources.Resource.AsientoOtrosGastos + ": " + p_oDocumentoCotizacion.NoOrden()
                End Select
                For Each rowAsiento As Asiento In p_oAsientoList
                    '*********************
                    'Cuenta Credito
                    '*********************
                    oJournalEntry.Lines.AccountCode = rowAsiento.CuentaCredito
                    oJournalEntry.Lines.ProjectCode = p_oDocumentoCotizacion.CodigoProyecto
                    If rowAsiento.Moneda = strMonedaLocal Or rowAsiento.Moneda = Nothing Then
                        oJournalEntry.Lines.Credit = rowAsiento.Costo
                    Else
                        oJournalEntry.Lines.FCCredit = rowAsiento.Costo
                        oJournalEntry.Lines.FCCurrency = rowAsiento.Moneda
                    End If
                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = rowAsiento.NoOrden
                    oJournalEntry.Lines.Reference1 = rowAsiento.NoOrden
                    If rowAsiento.UsaDimensiones Then
                        oJournalEntry.Lines.CostingCode = rowAsiento.CostingCode
                        oJournalEntry.Lines.CostingCode2 = rowAsiento.CostingCode2
                        oJournalEntry.Lines.CostingCode3 = rowAsiento.CostingCode3
                        oJournalEntry.Lines.CostingCode4 = rowAsiento.CostingCode4
                        oJournalEntry.Lines.CostingCode5 = rowAsiento.CostingCode5
                    End If
                    oJournalEntry.Lines.Add()
                    '*****************
                    'Cuenta Debito
                    '*****************
                    oJournalEntry.Lines.AccountCode = rowAsiento.CuentaDebito
                    oJournalEntry.Lines.ProjectCode = p_oDocumentoCotizacion.CodigoProyecto
                    If rowAsiento.Moneda = strMonedaLocal Or rowAsiento.Moneda = Nothing Then
                        oJournalEntry.Lines.Debit = rowAsiento.Costo
                    Else
                        oJournalEntry.Lines.FCDebit = rowAsiento.Costo
                        oJournalEntry.Lines.FCCurrency = rowAsiento.Moneda
                    End If
                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = rowAsiento.NoOrden
                    oJournalEntry.Lines.Reference1 = rowAsiento.NoOrden
                    If Not String.IsNullOrEmpty(rowAsiento.U_SCGD_Cod_Tran) Then
                        oJournalEntry.Lines.UserFields.Fields.Item(mc_strTransaccion).Value = rowAsiento.U_SCGD_Cod_Tran
                        oJournalEntry.Lines.UserFields.Fields.Item(mc_strCodUnidad).Value = p_oDocumentoCotizacion.CodigoUnidad
                    End If
                    If rowAsiento.UsaDimensiones Then
                        oJournalEntry.Lines.CostingCode = rowAsiento.CostingCode
                        oJournalEntry.Lines.CostingCode2 = rowAsiento.CostingCode2
                        oJournalEntry.Lines.CostingCode3 = rowAsiento.CostingCode3
                        oJournalEntry.Lines.CostingCode4 = rowAsiento.CostingCode4
                        oJournalEntry.Lines.CostingCode5 = rowAsiento.CostingCode5
                    End If
                    oJournalEntry.Lines.Add()
                Next
                If oJournalEntry.Add <> 0 Then
                    intAsientoGenerado = 0
                    m_oCompany.GetLastError(intError, strMensajeError)
                    Throw New ExceptionsSBO(intError, strMensajeError)
                Else
                    m_oCompany.GetNewObjectCode(strAsientoGenerado)
                    If Not String.IsNullOrEmpty(strAsientoGenerado) Then
                        intAsientoGenerado = CInt(strAsientoGenerado)
                    Else
                        intAsientoGenerado = 0
                    End If
                End If
            End If
            Return intAsientoGenerado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Public Function CrearUDOFacturaInterna(ByRef p_udoFacturaInterna As UDOFacturaInterna,
                                      ByRef p_oDocumentoCotizacion As oDocumento,
                                      ByRef p_intAsientoServicio As Integer,
                                      ByRef p_intAsientoServicioExterno As Integer,
                                      ByRef p_intOtrosGastos As Integer,
                                      ByRef p_strNoSalida As String,
                                      ByRef p_strDocEntryFacturaInterna As String,
                                      ByRef p_dblMontoTotal As Double,
                                      ByRef p_blnUsaDraft As Boolean) As Boolean
        Dim udoEncFactura As EncabezadoUDOFacturaInterna
        Try
            udoEncFactura = New EncabezadoUDOFacturaInterna()
            p_udoFacturaInterna.Encabezado = New EncabezadoUDOFacturaInterna
            With p_oDocumentoCotizacion
                udoEncFactura.CardCode = p_oDocumentoCotizacion.CardCode
                udoEncFactura.CardName = p_oDocumentoCotizacion.CardName
                udoEncFactura.CodigoEstilo = p_oDocumentoCotizacion.CodigoEstilo
                udoEncFactura.CodigoMarca = p_oDocumentoCotizacion.CodigoMarca
                udoEncFactura.CodigoModelo = p_oDocumentoCotizacion.CodigoModelo
                udoEncFactura.CodigoUnidad = p_oDocumentoCotizacion.CodigoUnidad
                udoEncFactura.CodigoVehiculo = p_oDocumentoCotizacion.NumeroVehiculo
                udoEncFactura.Moneda = p_oDocumentoCotizacion.DocCurrency
                udoEncFactura.Ano = p_oDocumentoCotizacion.Year
                udoEncFactura.Monto = p_dblMontoTotal
                udoEncFactura.NoCotización = p_oDocumentoCotizacion.DocEntry
                udoEncFactura.NoOT = p_oDocumentoCotizacion.NoOrden
                udoEncFactura.Placa = p_oDocumentoCotizacion.Placa
                udoEncFactura.TipoOrden = p_oDocumentoCotizacion.TipoOT.ToString()
                udoEncFactura.VIN = p_oDocumentoCotizacion.NumeroVIN

                If p_intAsientoServicio > 0 Then udoEncFactura.Asiento = p_intAsientoServicio
                If p_intOtrosGastos > 0 Then udoEncFactura.AsientoGastos = p_intOtrosGastos
                If Not p_blnUsaDraft Then
                    If Not String.IsNullOrEmpty(p_strNoSalida) Then udoEncFactura.NoDocumentoSalida = p_strNoSalida
                End If
                If p_intAsientoServicioExterno > 0 Then udoEncFactura.Asiento_SE = p_intAsientoServicioExterno
            End With
            p_udoFacturaInterna.Encabezado = udoEncFactura
            If p_udoFacturaInterna.Insert() Then
                p_strDocEntryFacturaInterna = p_udoFacturaInterna.Encabezado.DocEntry
                Return True
            End If
            Return False
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Sub CargarCotizacion(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                ByRef p_oDocumentoCotizacion As oDocumento, _
                                ByRef p_oListServicios As List(Of oLineasDocumento), _
                                ByRef p_oListServicioExternos As List(Of oLineasDocumento), _
                                ByRef p_oListOtrosGastos As List(Of oLineasDocumento), _
                                ByRef p_oListRepuestoSuministro As List(Of oLineasDocumento),
                                ByRef p_dblMontoTotal As Double, ByRef p_blnError As Boolean)
        Dim rowCotizacion As oLineasDocumento
        Try
            '**********************************
            'Carga Encabezado de la Cotizacion
            '**********************************
            With p_oDocumentoCotizacion
                .DocEntry = p_oCotizacion.DocEntry
                .DocNum = p_oCotizacion.DocNum
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                    .NoOrden = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                    .IDSucursal = p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value) Then
                    .OTPadre = p_oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value) Then
                    .NoOTReferencia = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value) Then
                    .NumeroVIN = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value) Then
                    .NumeroVehiculo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value) Then
                    .CodigoUnidad = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.DocumentsOwner.ToString()) Then
                    .CodigoAsesor = p_oCotizacion.DocumentsOwner
                Else
                    .CodigoAsesor = 0
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value.ToString()) Then
                    .TipoOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                Else
                    .TipoOT = 0
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value) Then
                    .CodigoProyecto = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value
                End If
                .CardCode = p_oCotizacion.CardCode
                .CardName = p_oCotizacion.CardName
                .DocCurrency = p_oCotizacion.DocCurrency
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value) Then
                    .NoVisita = p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value) Then
                    .Cono = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value.ToString.Trim()) Then
                    .Year = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString.Trim()) Then
                    .DescripcionMarca = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString.Trim()) Then
                    .DescripcionModelo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString.Trim()) Then
                    .DescripcionEstilo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value.ToString.Trim()) Then
                    .CodigoMarca = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString.Trim()) Then
                    .CodigoEstilo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString.Trim()) Then
                    .CodigoModelo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString.Trim()) Then
                    .Placa = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString().Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value.ToString.Trim()) Then
                    .NombreClienteOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value.ToString().Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value.ToString.Trim()) Then
                    .CodigoClienteOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value.ToString().Trim()
                End If
            End With
            For lineaCotizacion As Integer = 0 To p_oCotizacion.Lines.Count - 1
                p_oCotizacion.Lines.SetCurrentLine(lineaCotizacion)
                If p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi Then
                    rowCotizacion = New oLineasDocumento()
                    With rowCotizacion
                        .Aprobado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                            .NoOrden = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                            .Sucursal = p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                        End If
                        .DocEntry = p_oCotizacion.Lines.DocEntry
                        .LineNum = p_oCotizacion.Lines.LineNum
                        .ItemCode = p_oCotizacion.Lines.ItemCode
                        .Price = p_oCotizacion.Lines.Price
                        .Quantity = p_oCotizacion.Lines.Quantity
                        .VisOrder = p_oCotizacion.Lines.VisualOrder
                        If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()) Then
                            .ID = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value.ToString()) Then
                            .Comprar = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value
                        Else
                            .Comprar = String.Empty
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString()) Then
                            .CentroCosto = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value
                        End If
                        .Trasladado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value
                        If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString()) Then
                            .TipoArticulo = CInt(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                        End If
                        .Costo = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value
                        .CantidadRecibida = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value
                        .CantidadSolicitada = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value
                        .CantidadPendiente = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value
                        .CantidadPendienteBodega = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value
                        .CantidadPendienteTraslado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value
                        .CantidadPendienteDevolucion = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value
                        If Not String.IsNullOrEmpty(p_oCotizacion.Lines.CostingCode) Then .CostingCode = p_oCotizacion.Lines.CostingCode
                        If Not String.IsNullOrEmpty(p_oCotizacion.Lines.CostingCode2) Then .CostingCode2 = p_oCotizacion.Lines.CostingCode2
                        If Not String.IsNullOrEmpty(p_oCotizacion.Lines.CostingCode3) Then .CostingCode3 = p_oCotizacion.Lines.CostingCode3
                        If Not String.IsNullOrEmpty(p_oCotizacion.Lines.CostingCode4) Then .CostingCode4 = p_oCotizacion.Lines.CostingCode4
                        If Not String.IsNullOrEmpty(p_oCotizacion.Lines.CostingCode5) Then .CostingCode5 = p_oCotizacion.Lines.CostingCode5
                    End With
                    Select Case rowCotizacion.TipoArticulo
                        Case TipoArticulo.Repuesto
                            p_oListRepuestoSuministro.Add(rowCotizacion)
                            p_dblMontoTotal += p_oCotizacion.Lines.LineTotal
                        Case TipoArticulo.Suministro
                            p_oListRepuestoSuministro.Add(rowCotizacion)
                            p_dblMontoTotal += p_oCotizacion.Lines.LineTotal
                        Case TipoArticulo.Servicio
                            p_oListServicios.Add(rowCotizacion)
                            p_dblMontoTotal += p_oCotizacion.Lines.LineTotal
                        Case TipoArticulo.ServicioExterno
                            p_oListServicioExternos.Add(rowCotizacion)
                            p_dblMontoTotal += p_oCotizacion.Lines.LineTotal
                        Case TipoArticulo.OtrosCostos
                            p_oListOtrosGastos.Add(rowCotizacion)
                            p_dblMontoTotal += p_oCotizacion.Lines.LineTotal
                    End Select
                End If
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            p_blnError = True
        End Try
    End Sub

    Private Function CargarContraCuenta(ByVal p_oDocumentoCotizacion As oDocumento) As String
        Dim strContraCuenta As String = String.Empty
        Try
            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_oDocumentoCotizacion.IDSucursal)) Then
                With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_oDocumentoCotizacion.IDSucursal))
                    If .Configuracion_OT_Interna.Any(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString())) Then
                        If Not String.IsNullOrEmpty(.Configuracion_OT_Interna.FirstOrDefault(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString())).U_NumCuent.Trim()) Then strContraCuenta = .Configuracion_OT_Interna.FirstOrDefault(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_oDocumentoCotizacion.TipoOT.ToString())).U_NumCuent.Trim()
                    End If
                End With
            End If
            Return strContraCuenta
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Public Sub StartTransaction()
        Try
            If Not m_oCompany.InTransaction Then
                m_oCompany.StartTransaction()
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ResetTransaction()
        Try
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub CommitTransaction()
        Try
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(BoWfTransOpt.wf_Commit)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub RollbackTransaction()
        Try
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub
#End Region

End Class

' Clase para la definición de la lista
Public Class ListaCuentasInterna

    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property Cuenta() As String
        Get
            Return strCuentaDebito
        End Get
        Set(ByVal value As String)
            strCuentaDebito = value
        End Set
    End Property
    Private strCuentaDebito As String

    Public Property Costo() As Decimal
        Get
            Return decCosto
        End Get
        Set(ByVal value As Decimal)
            decCosto = value
        End Set
    End Property
    Private decCosto As Decimal

    Public Property Aplicado() As Boolean
        Get
            Return blnAplicado
        End Get
        Set(ByVal value As Boolean)
            blnAplicado = value
        End Set
    End Property
    Private blnAplicado As Boolean
End Class
