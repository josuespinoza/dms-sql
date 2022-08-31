Imports SAPbobsCOM
Imports SCG.SBOFramework
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGCommon
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Threading
Imports SAPbouiCOM
Imports SCG.DMSOne.Framework.UDOOrden

Public Class CotizacionCLS

#Region "Declaraciones"

#Region "Enums"

    Private m_intNoCopiasRep As Integer

    Private Enum GeneraOrdenTrabajo
        scgSiGenera = 1
        scgNoGenera = 2
    End Enum

    Private Enum TiposArticulos

        scgRepuesto = 1
        scgActividad = 2
        scgSuministro = 3
        scgServicioExt = 4
        scgPaquete = 5
        scgNinguno = 0
        scgOtrosGastos_Costos = 11
        scgOtrosIngresos = 12

    End Enum

    Private Enum ArticuloAprobado

        scgSi = 1
        scgNo = 2
        scgFalta = 3

    End Enum

    Private Enum CotizacionEstado

        creada = 1
        modificada = 2
        sinCambio = 3

    End Enum

    Private Enum ImprimirOT

        scgSi = 1
        scgNo = 2

    End Enum

    Private Enum enumItemValidacionResult
        scgSinCambio = 0
        scgNoAprobar = 1
        scgModQtyCoti = 2
        scgPendTransf = 3
        scgPendBodega = 4
        scgComprar = 5
    End Enum

    Private Enum enumRealizarTraslados
        scgSi = 1
        scgNo = 0
    End Enum

    Private Enum enumTipoRow

        scgRepuestoRow = 1
        scgSuministroRow = 2
        scgActividadRow = 3

    End Enum

    Private Enum enumTrasladadoOTHija

        scgOTHijaSI = 1
        scgOTHijaNO = 2

    End Enum

    Private Enum LineaAProcesar

        scgSi = 1
        scgNo = 2

    End Enum
#End Region

#Region "Estructuras"

    Structure stTipoListaCantAnteriores
        Dim ItemCode As String
        Dim LineNum As Integer
        Dim Cantidad As Decimal
    End Structure

    'para cargar filas con LineNum erroneos
    Private Structure LineasLineNumErroneos

        Dim NoOrden As String
        Dim IdItem As String
        Dim Id As String
        Dim intLineNum As Integer
        Dim TipoRow As Integer


    End Structure

    Private Structure ValoresConfiguracionSucursalCotizacion

        Dim m_strCuentaTipoOrdenInternaConfiSucursal As String
        Dim m_strTransaccionLineas As String
        Dim m_strTipoMoneda As String
        Dim m_blnServicosExternosInventariables As Boolean
        Dim m_strCodigoCuentaExistenciasConfiSucursal As String
        Dim m_strTipoCostoPorSucursal As String
        Dim m_strCentroCosto As String
        Dim m_blnAsignacionAutomaticaColaborador As Boolean
        Dim m_blnDraft As Boolean
        Dim m_strNoCopias As String
        Dim m_strIDSerieDocTrasnf As String
        Dim m_strIDSerieDocOrdenVenta As String

    End Structure

#End Region

#Region "Costantes"

    Private Const mc_strIdSucursal As String = "U_SCGD_idSucursal"
    Private Const mc_strCrear As String = "Crear"
    Private Const mc_strActualizar As String = "Actualizar"
    Private Const mc_strUpdate As String = "Update"
    Private Const mc_strAdd As String = "Add"
    Private Const mc_strComboGeneraOT As String = "SCGD_cbGOT"
    Private Const mc_strComboImprimeOR As String = "SCGD_cbRec"
    Private Const mc_strIDBotonEjecucion As String = "1"
    Private Const mc_strbtnGenerar As String = "btnGenerar"
    Private Const mc_strIDMatriz As String = "38"
    Private Const mc_strDocNumCotización As String = "8"

    Private Const m_strRecepcionesIngresa As String = "Select DocEntry from OQUT Where DocNum = "

    'Constantes para sacar datos de SBO
    Private Const mc_strNum_Visita As String = "U_SCGD_No_Visita"

    Private Const mc_strImprimirOT As String = "U_SCGD_GeneraOR"
    Private Const mc_strNumUnidad As String = "U_SCGD_Cod_Unidad"
    Private Const mc_strNumVehiculo As String = "U_SCGD_Num_Vehiculo"
    Private Const mc_strOTPadre As String = "U_SCGD_OT_Padre"
    Private Const mc_strProcesad As String = "U_SCGD_Procesad"
    Private Const mc_strOtRef As String = "U_SCGD_NoOtRef"
    Private Const mc_strGenerarOT As String = "U_SCGD_Genera_OT"
    Private Const mc_strNum_OT As String = "U_SCGD_Numero_OT"
    Private Const mc_strNoOT As String = "U_SCGD_NoOT"
    Private Const mc_strTipoOT As String = "U_SCGD_Tipo_OT"
    Private Const mc_strCardCode As String = "CardCode"
    Private Const mc_strCardName As String = "CardName"
    Private Const mc_strClienteOT As String = "U_SCGD_CCliOT"
    Private Const mc_strNombreClienteOT As String = "U_SCGD_NCliOT"
    Private Const mc_strFech_Recep As String = "U_SCGD_Fech_Recep"
    Private Const mc_strHora_Recep As String = "U_SCGD_Hora_Recep"
    Private Const mc_strFech_Comp As String = "U_SCGD_Fech_Comp"
    Private Const mc_strHora_Comp As String = "U_SCGD_Hora_Comp"
    Private Const mc_strEstadoCotizacion As String = "U_SCGD_Estado_Cot"
    Private Const mc_strEstadoCotizacionID As String = "U_SCGD_Estado_CotID"
    Private Const mc_strNGas As String = "U_SCGD_Gasolina"
    Private Const mc_strHorasMotor As String = "U_SCGD_HoSr"
    Private Const mc_strCono As String = "U_SCGD_Gorro_Veh"
    Private Const mc_strDescMarca As String = "U_SCGD_Des_Marc"
    Private Const mc_strDesc_Estilo As String = "U_SCGD_Des_Esti"
    Private Const mc_strDescModelo As String = "U_SCGD_Des_Mode"
    Private Const mc_strCod_Estilo As String = "U_SCGD_Cod_Estilo"
    Private Const mc_strCod_Modelo As String = "U_SCGD_Cod_Modelo"
    Private Const mc_strCod_Marca As String = "U_SCGD_Cod_Marca"
    Private Const mc_strPlaca As String = "U_SCGD_Num_Placa"
    Private Const mc_strVIN As String = "U_SCGD_Num_VIN"
    Private Const mc_strAño As String = "U_SCGD_Ano_Vehi"

    'Variables para la línea de la cotizacion
    Private Const mc_strTipoArticulo As String = "U_SCGD_TipoArticulo"
    Private Const mc_strCodCentroCosto As String = "U_SCGD_CodCtroCosto"
    Private Const mc_strDuracion As String = "U_SCGD_Duracion"
    Private Const mc_strFase As String = "U_SCGD_T_Fase"
    Private Const mc_strItemAprobado As String = "U_SCGD_Aprobado"
    Private Const mc_strEmpRealiza As String = "U_SCGD_EmpAsig"
    Private Const mc_strNombEmpleado As String = "U_SCGD_NombEmpleado"
    Private Const mc_strGenerico As String = "U_SCGD_Generico"
    Private Const mc_strTrasladado As String = "U_SCGD_Traslad"
    Private Const mc_strIdRepxOrd As String = "U_SCGD_IdRepxOrd"
    Private Const mc_strCompra As String = "U_SCGD_Compra"
    Private Const mc_strCSol As String = "U_SCGD_CSol"
    Private Const mc_strCPen As String = "U_SCGD_CPen"
    Private Const mc_strCRec As String = "U_SCGD_CRec"
    Private Const mc_strCPenBod As String = "U_SCGD_CPBo"
    Private Const mc_strCPenDev As String = "U_SCGD_CPDe"

    'proyecto
    Private Const mc_strProyecto As String = "U_SCGD_Proyec"

    Private Const mc_strCodeEspecifico As String = "U_SCGD_CodEspecifico"
    Private Const mc_strNameEspecifico As String = "U_SCGD_NombEspecific"

    Private Const mc_strResultado As String = "U_SCGD_Resultado"

    Private Const mc_strCopiasRepRecepcion As String = "CopiasRepRecepcion"

    'Bodegas
    Private Const mc_strBodegaRepuestos As String = "BodegaRepuestos"
    Private Const mc_strBodegaServiciosExternos As String = "BodegaServiciosExternos"
    Private Const mc_strBodegaServicios As String = "BodegaServicios"

    'Linea trasladada a OT Hija

    Private Const mc_strTrasladadoOTHija As String = "U_SCGD_OTHija"

    Private Const mc_strItemAProcesar As String = "U_SCGD_Procesar"

    Private Const mc_strBtnSolOtEsp As String = "btnSotE"
    Private Const g_strFormSolicitaOTEsp As String = "SCGD_SOTE"
    Private Const g_strDtConsul As String = "dtConsul"

    Private Const mc_strcbSucursal As String = "SCGD_cbSuc"

    Private Const mc_strColTipoLinea As String = "257"

    Private Const g_strAsignacionMultiple As String = "SCGD_ASM"
    Private Const mc_strBtnAsigMult As String = "btnAsM"

    Private Const mc_stTipoPago As String = "stTipoPago"
    Private Const mc_stDptoSrv As String = "stDptoSrv"
    Private Const mc_strCboTipoPago As String = "cboTipPago"
    Private Const mc_strCboDptoSrv As String = "cboDptoSrv"

    Private Const mc_strUDFTipoPago As String = "U_SCGD_TipoPago"
    Private Const mc_strUDFServDpto As String = "U_SCGD_ServDpto"
    Public Const ConsultaAsignacionesOTInterna As String = "select distinct cc.Code ID, q1.U_SCGD_Sucur+'-'+Cast(q1.LineNum as varchar)+'-'+cc.Code as IDRepXOrd, itm.U_SCGD_T_Fase NoFase, q1.ItemCode, q1.U_SCGD_DurSt DuracionAprobada, cc.Code NoOrden, q1.Dscription ItemName, " & _
                                                                "cc.U_Estad Estado, fp.Name Descripcion, emp.empID IDEmp, (emp.firstName + ' ' + emp.lastName) as NombreEmp, " & _
                                                                "q1.docEntry as NumCot, q1.LineNum, 0 U_SCGD_DurSt, 0	U_SCGD_TiempoReal, 0 as Price " & _
                                                            "from [@SCGD_CTRLCOL] cc with (nolock) " & _
                                                                "inner Join QUT1 q1 with (nolock) on cc.code = q1.U_SCGD_NoOT and cc.U_IdAct = q1.U_SCGD_ID " & _
                                                                "inner join OITM itm with (nolock) on q1.ItemCode = itm.ItemCode " & _
                                                                "left join [@SCGD_FASEPRODUCCION] fp with (nolock) on itm.U_SCGD_T_Fase = fp.Code " & _
                                                                "inner join OHEM emp with (nolock) on cc. U_Colab = emp.empID " & _
                                                            "WHERE cc.Code = '{0}'	"

    Public Const ConsultaAsignacionesOTExterna As String = "SELECT distinct axo.ID, axo.NoFase, itm.ItemCode, axo.DuracionAprobada, axo.NoOrden, itm.ItemName, axo.Estado, fp.Descripcion, " & _
                                                                "ccol.EmpID as IDEmp, (oh.firstName + ' ' + oh.lastName) as NombreEmp, q1.docEntry as NumCot, q1.LineNum, q1.U_SCGD_DurSt, q1.U_SCGD_TiempoReal, q1.Price " & _
                                                            "FROM [{1}].[dbo].[SCGTA_TB_ActividadesxOrden] axo with (nolock) " & _
                                                                "Inner Join [{1}].[dbo].[SCGTA_VW_OITM] itm with (nolock) " & _
                                                                    "On axo.NoActividad = itm.ItemCode " & _
                                                                "Left Join [{1}].[dbo].[SCGTA_TB_FasesProduccion] fp with (nolock) " & _
                                                                "on axo.NoFase = fp.NoFase " & _
                                                                "Left Join [{1}].[dbo].[SCGTA_VW_OQUT_QUT1] q1 with (nolock) " & _
                                                                    "on axo.NoActividad = q1.ItemCode and axo.NoOrden = q1.U_SCGD_Numero_Ot and axo.ID = q1.U_SCGD_IdRepxOrd  " & _
                                                                "Left Join [{1}].[dbo].[SCGTA_TB_ControlColaborador] ccol with (nolock) " & _
                                                                    "on axo.NoOrden = ccol.NoOrden and axo.ID = ccol.IDActividad " & _
                                                                "left join [{1}].[dbo].[SCGTA_VW_OHEM] oh " & _
                                                                "on ccol.EmpID = oh.empID " & _
                                                            "WHERE axo.NoOrden = '{0}'"
    Private Const mc_strWAN As String = "U_SCGD_WAN"
    Private Const mc_strPoolAsig As String = "U_SCGD_PoolAsig"
    Private Const mc_strCompOri As String = "U_SCGD_CompOri"
    Private Const mc_strNumeroCaso As String = "U_SCGD_NoCas"
    Private Const mc_strNoPol As String = "U_SCGD_NoPol"
    Private Const mc_strCompS As String = "U_SCGD_CompS"
    Private Const mc_strPeri As String = "U_SCGD_Peri"
    Private Const mc_strOwnerCode As String = "OwnerCode"
#End Region

#Region "Objetos SBO"

    Private WithEvents SBO_Application As Application
    Private m_oCotizacionAnterior As Documents
    Public m_oCotizacion As Documents
    Private m_objCotizacionPadre As Documents
    Private m_oCompany As SAPbobsCOM.Company
    Private m_oForm As Form
    Private oCotizacionlocal As Documents

#End Region

#Region "Variables"

    Private m_strWAN As String
    Private m_strPoolAsig As String
    Private m_strCompOri As String
    Private m_strNumeroCaso As String
    Private m_strNoPol As String
    Private m_strCompS As String
    Private m_strPeri As String
    Private m_strOwnerCode As String
    Private m_strIDSucursal As String
    Public m_strNoOrden As String
    Private m_strBDConfiguracion As String
    Private m_strBDTalller As String
    'Campos Cotización
    Private m_strCodigoCliente As String
    Private m_strNombreCliente As String
    Private m_strEmpleadoRecibe As String
    Private m_intGenerarOT As Integer
    Private m_strEstadoCotizacionID As String
    Private m_intDocEntry As Integer
    Private m_intDocNum As Integer
    Private m_strNumeroVehiculo As String
    Private m_strNumeroOT As String
    Private m_strShiptoCode As String
    Private m_EspecifVehi As String
    Private m_UsaAsocxEspc As String
    Private m_UsaFilSerEspeci As String
    Private m_strDocEntry As String
    Private m_intTipoOT As Integer
    Private m_strNumeroVisita As String
    Private m_strNumeroUnidad As String
    Private m_strVIN As String
    Private m_strPlaca As String
    Private m_dbkilometraje As Double
    Private m_strEstadoCotizacion As String
    Private m_strDescMarca As String
    Private m_strDescEstilo As String
    Private m_strDescModelo As String
    Private m_strCodeMarca As String
    Private m_strCodeEstilo As String
    Private m_strCodeModelo As String
    Private m_strOTPadre As String
    Private m_strClienteOT As String
    Private m_strNombreClienteOT As String
    Private m_strAno As String
    Private m_dtFechaRecepcion As DateTime
    Private m_dtHoraRecepcion As DateTime
    Private m_dtFechaCompromiso As DateTime
    Private m_dtHoraCompromiso As DateTime
    Private m_strOtReferencia As String
    Private m_strOtNivelGas As String
    Private m_intHorasMotor As Integer
    Private m_strCono As String
    Private m_strObservaciones As String
    Private m_strCentroCosto As String
    Private m_strNoSerieCita As String
    Private m_strNoCita As String
    Private m_strSerieCompletaCita As String
    Private m_intCodigoTecnico As Nullable(Of Integer)
    Private m_blnActualizar As Boolean
    Private strCadenaConexionBDTaller As String = ""
    Dim m_blnIniciarTransaccion As Boolean = False
    Private m_intEstCotizacion As Integer
    Private m_strCaptionBefore As String
    Private m_intRealizarTraslados As enumRealizarTraslados
    Private m_strDocNumNuevo As String
    Private m_intGeneraOTPantalla As Integer
    Private m_intImprimeORPantalla As Integer
    Private m_strNoBodegaRepu As String = ""
    Private m_strNoBodegaSumi As String = ""
    Private m_strNoBodegaSeEx As String = ""
    Private m_strNoBodegaServ As String = ""
    Private m_strNoBodegaProceso As String = ""
    Private m_strIDSerieDocTrasnf As String = ""
    Private m_lstRepuestos As New List(Of TransferenciaItems.LineasTransferenciaStock)
    Private m_lstSuministros As New List(Of TransferenciaItems.LineasTransferenciaStock)
    Private m_lstServiociosEX As New List(Of TransferenciaItems.LineasTransferenciaStock)
    Private m_lstItemsEliminarRepuestos As New List(Of TransferenciaItems.LineasTransferenciaStock)
    Private m_lstItemsEliminarSuministros As New List(Of TransferenciaItems.LineasTransferenciaStock)
    Private m_lstItemACambiarEstado As New List(Of TransferenciaItems.LineasCambiarEstado)
    Private m_lstItemACambiarEstadoAdicional As New List(Of TransferenciaItems.LineasCambiarEstado)
    Private m_lstCantidadesAnteriores As New List(Of stTipoListaCantAnteriores)
    Private blnDraft As Boolean = False
    Public blnModificaItemsAdicionales As Boolean = False
    Private blnAsignacionAutomaticaColaborador As Boolean = False
    Private objItemsLineasLineNumErroneos As New List(Of LineasLineNumErroneos)
    Private objItemLineNumErroneo As New LineasLineNumErroneos
    Private ListaLineNumPaquetes As List(Of Integer) = New List(Of Integer)
    Public Shared ListaItemsCodeOTEspeciales As List(Of String) = New List(Of String)
    Public Shared ListaIdRepxOrdenOTEspeciales As List(Of String) = New List(Of String)
    Public Shared ListaNoOrdenOTEspeciales As List(Of String) = New List(Of String)
    Private ListaTargetEntryEntradaMercancia As List(Of Integer) = New List(Of Integer)
    Private ListaTargetEntryFacturaProveedor As List(Of Integer) = New List(Of Integer)
    Private ListaTargetEntryOrdenCompra As List(Of Integer) = New List(Of Integer)
    Private ListaTargetType As List(Of Integer) = New List(Of Integer)
    'Orden de compra a buscar
    Private strNumeroOT_En_OrdenCompra As String = String.Empty
    Private blnRevisionLineasOT As Boolean = False
    Public blnServExtOTEspeciales As Boolean = False
    Private g_dtConsulta As DataTable
    Private oGestorFormularios As GestorFormularios
    Private oFormSolOTEspecial As SolicitaOTEspecial
    Private oFormAsignacionMultiple As AsignacionMultiple
    Public strIdSucursal As String
    Public blnActualizaValoresHS As Boolean = False
    Public blnActualizaValoresKm As Boolean = False
    Public blnValidarCamposHS_KM As Boolean = False
    Public strIDVehiculoHS_KM As String = String.Empty
    Public decHorasServicio As Decimal = 0
    Public decKilometraje As Decimal = 0
    Public m_blnServicosExternosInventariables As Boolean = False
    Public m_strNoCopias As String = "1"
    Private objValoresConfiguracionSucursalQT As ValoresConfiguracionSucursalCotizacion
    Public m_blnUsaConfiguracionInternaTaller As Boolean = False
    Public oDataTableConfiguracionesSucursal As Data.DataTable
    Public Shared ListaIdOrdenOTEspecialesSolOTEsp As Generic.List(Of String) = New Generic.List(Of String)
    Dim strNombreColumnaID As String = String.Empty
    Private strDocEntryOT As String = String.Empty
    Private objListaActividades As New List(Of ListaActividadesCotizacion)()
    Public m_blnControlColaborador As Boolean = False
    Public g_strCreaHjaCanPend As String = String.Empty
    Private m_strNoCitaACancelar As String
    Private m_strNoSerieCitaACancelar As String

    Public Shared g_strTiemEsta As String = String.Empty
    Public Shared g_strReducCant As String = String.Empty
    Public UDOOrden As UDOOrden

#End Region

#Region "Acceso a datos"

    Private dtbRepuestosxOrden As New RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable
    Private dtbSuministrosxOrden As New SuministrosDataset.SCGTA_VW_SuministrosDataTable
    Private dtbActividadesXOrden As New ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable

    Private m_cnnSCGTaller As SqlClient.SqlConnection
    Private m_trnTransaccion As SqlClient.SqlTransaction

    'Datasets y datadapters
    Private m_dstVisita As New VisitaDataset
    Private m_adpVisita As VisitasDataAdapter

    Private m_dstOrdenTrabajo As OrdenTrabajoDataset
    ' Private m_dstOrdenTrabajoAnterior As OrdenTrabajoDataset
    Private m_adpOrdenTrabajo As OrdenTrabajoDataAdapter

    Private m_dstRepuestosxOrden As RepuestosxOrdenDataset
    Private m_adpRepuestosxOrden As RepuestosxOrdenDataAdapter

    Private m_dstSuministrosxOrden As SuministrosDataset
    Private m_adpSuministrosxOrden As SuministrosDataAdapter

    Private m_dstActividadesxOrden As ActividadesXFaseDataset
    Private m_adpActividadesxOrden As ActividadesXFaseDataAdapter

    Private m_dstPaquetesxOrden As PaquetesDataSet
    Private m_adpPaqutesxOrden As PaquetesxOrdenDataAdapter

    Private m_dstAsignacionesColaboradores As New ColaboradorDataset

    'datarows
    Private m_drwVisita As VisitaDataset.SCGTA_TB_VisitaRow
    Private m_drwOrdenTrabajo As OrdenTrabajoDataset.SCGTA_TB_OrdenRow
    Private m_drwRepuestos As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
    Private m_drwSuministros As SuministrosDataset.SCGTA_VW_SuministrosRow
    Private m_drwActividades As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
    Private m_drwPaquetes As PaquetesDataSet.PaquetesDataSetRow

    'Actualización de la cotización
    Private dtbLineasActualizadas As New dtsMovimientoStock.LineaActualizadaDataTable
    Private dstAsignacionesColaboradores As New ColaboradorDataset

    'Dim objTransferenciaStock As New TransferenciaItems(SBO_Application, m_oCompany, strCadenaConexionBDTaller)
    Private objTransferenciaStock As TransferenciaItems

    Private objUtilitarios As New SCGDataAccess.Utilitarios(strCadenaConexionBDTaller)

    Private m_dtsCambioAlmacenProceso As New CambioBodegaProcesoDataset.SCGTA_SP_SelCambioBodegaProcesoDataTable
    Private m_adpCambioAlmacenProceso As CambioBodegaProcesoDatasetTableAdapters.SCGTA_SP_SelCambioBodegaProcesoTableAdapter

    Private m_dtsCambioCuentaProceso As New CambioCuentaProcesoDataset.SCGTA_SP_SelCambioCuentaProcesoDataTable
    Private m_adpCambioCuentaProceso As CambioCuentaProcesoDatasetTableAdapters.SCGTA_SP_SelCambioCuentaProcesoTableAdapter

    Private m_dtsLineNumsOTOriginal As New LineNumsOTOriginal.SCGTA_SP_LineNumsOTOriginalDataTable
    Private m_adpLineNumsOTOriginal As LineNumsOTOriginalTableAdapters.SCGTA_SP_LineNumsOTOriginalTableAdapter

#End Region

#End Region

#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByVal p_SBO_Application As SAPbouiCOM.Application, ByVal ocompany As SAPbobsCOM.Company)
        Try

            SBO_Application = p_SBO_Application
            m_oCompany = ocompany
            DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)
            Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, strCadenaConexionBDTaller)
            objTransferenciaStock = New TransferenciaItems(SBO_Application, m_oCompany, strCadenaConexionBDTaller)
            m_strIDSucursal = Utilitarios.ObtieneIdSucursal(DMS_Connector.Company.ApplicationSBO).ToString
            UDOOrden = New UDOOrden(m_oCompany)
            m_blnActualizar = False
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

#End Region

#Region "Propiedades"

    Public Property CodigoCliente() As String

        Get

            Return m_strCodigoCliente

        End Get

        Set(ByVal value As String)

            m_strCodigoCliente = value

        End Set

    End Property

    Private _dtMecAsignados As DataTable
    Public Property dtMecAsignados() As DataTable
        Get
            Return _dtMecAsignados
        End Get
        Set(ByVal value As DataTable)
            _dtMecAsignados = value
        End Set
    End Property


    Public Property NombreCliente() As String

        Get

            Return m_strNombreCliente

        End Get

        Set(ByVal value As String)

            m_strNombreCliente = value

        End Set

    End Property

    Public Property FechaApertura() As DateTime

        Get

            Return m_dtFechaRecepcion

        End Get

        Set(ByVal value As DateTime)

            m_dtFechaRecepcion = value

        End Set

    End Property

    Public Property EmpleadoRecibe() As String

        Get

            Return m_strEmpleadoRecibe

        End Get

        Set(ByVal value As String)

            m_strEmpleadoRecibe = value

        End Set

    End Property

    Public Property GenerarOT() As Integer

        Get

            Return m_intGenerarOT

        End Get

        Set(ByVal value As Integer)

            m_intGenerarOT = value

        End Set

    End Property

    Public Property TipoOT() As Integer

        Get

            Return m_intTipoOT

        End Get

        Set(ByVal value As Integer)

            m_intTipoOT = value

        End Set

    End Property

    Public Property EstadoCotizacion() As Integer

        Get

            Return m_strEstadoCotizacion

        End Get

        Set(ByVal value As Integer)

            m_strEstadoCotizacion = value

        End Set

    End Property

    Public Property NumeroVisita() As String

        Get

            Return m_strNumeroVisita

        End Get

        Set(ByVal value As String)

            m_strNumeroVisita = value

        End Set

    End Property

    Public Property DocEntry() As Integer

        Get

            Return m_intDocEntry

        End Get

        Set(ByVal value As Integer)

            m_intDocEntry = value

        End Set

    End Property

    Public Property DocNum() As Integer

        Get

            Return m_intDocNum

        End Get

        Set(ByVal value As Integer)

            m_intDocNum = value

        End Set

    End Property

    Public Property NumeroVehiculo() As String

        Get

            Return m_strNumeroUnidad

        End Get

        Set(ByVal value As String)

            m_strNumeroUnidad = value

        End Set

    End Property

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

    Public Property NumeroOT() As String

        Get

            Return m_strNumeroOT

        End Get

        Set(ByVal value As String)

            m_strNumeroOT = value

        End Set

    End Property

    Public WriteOnly Property DocNumNuevo() As String

        Set(ByVal value As String)

            m_strDocNumNuevo = value

        End Set

    End Property

    Public Shared NoOT As String
    Public Shared IdSucursal As String



#End Region

#Region "Métodos"

    Public Sub ObtieneNumeroDocumentoACancelar(ByVal p_StrIDForm As String, ByRef BubbleEvent As Boolean)

        m_oFormGenCotizacion = SBO_Application.Forms.Item(p_StrIDForm)

        m_strNoCitaACancelar = m_oFormGenCotizacion.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_NoCita", 0).ToString().Trim()
        m_strNoCitaACancelar = m_strNoCitaACancelar.Trim

        m_strNoSerieCitaACancelar = m_oFormGenCotizacion.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_NoSerieCita", 0).ToString().Trim()
        m_strNoSerieCitaACancelar = m_strNoSerieCitaACancelar.Trim

    End Sub

    Public Sub CancelaCitaLigadaOT(ByVal p_StrIDForm As String, ByRef BubbleEvent As Boolean)


        If (Not String.IsNullOrEmpty(m_strNoCitaACancelar)) And (Not String.IsNullOrEmpty(m_strNoSerieCitaACancelar)) Then
            Utilitarios.EjecutarConsulta("Update [@SCGD_CITA] SET U_Estado = (Select U_CodCitaCancel From [@SCGD_CONF_SUCURSAL] Where U_Sucurs = U_Cod_Sucursal ) Where  U_NumCita = '" & m_strNoCitaACancelar & "' and U_Num_Serie = '" & m_strNoSerieCitaACancelar & "'")
        End If

    End Sub

    Public Sub PermitirCancelar(ByVal p_StrIDForm As String, ByRef BubbleEvent As Boolean)

        Dim strNoOT As String

        m_oFormGenCotizacion = SBO_Application.Forms.Item(p_StrIDForm)

        strNoOT = m_oFormGenCotizacion.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).ToString().Trim()
        strNoOT = strNoOT.Trim

        If Not String.IsNullOrEmpty(strNoOT) Then
            BubbleEvent = False
            SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeCancelarCotizacion, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
        Else
            If Not ValidarCitasPendientes(m_oFormGenCotizacion) Then
                BubbleEvent = False
                SBO_Application.StatusBar.SetText(My.Resources.Resource.CancelarCotizacionCitaReserva, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
            End If
        End If
    End Sub

    Private Function ValidarCitasPendientes(ByRef Formulario As SAPbouiCOM.Form) As Boolean
        Dim Resultado As Boolean = True
        Dim Sucursal As String = String.Empty
        Dim SerieCita As String = String.Empty
        Dim NumeroCita As String = String.Empty
        Dim UsaRequisicionReserva As String = String.Empty
        Dim Cuenta As Integer = 0
        Dim Query As String = " SELECT COUNT(*) FROM ""@SCGD_REQUISICIONES"" T0 WHERE T0.""U_SerieCita"" = '{0}' AND T0.""U_NumeroCita"" = '{1}' AND T0.""U_SCGD_CodEst"" = '1' "
        Try
            SerieCita = Formulario.DataSources.DBDataSources().Item("OQUT").GetValue("U_SCGD_NoSerieCita", 0).Trim
            NumeroCita = Formulario.DataSources.DBDataSources().Item("OQUT").GetValue("U_SCGD_NoCita", 0).Trim

            If Not String.IsNullOrEmpty(SerieCita) AndAlso Not String.IsNullOrEmpty(NumeroCita) Then
                Resultado = False
            End If
            'Sucursal = Formulario.DataSources.DBDataSources().Item("OQUT").GetValue("U_SCGD_idSucursal", 0)
            'If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)) IsNot Nothing Then
            '    UsaRequisicionReserva = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_UsePrepicking.Trim
            'End If

            'If UsaRequisicionReserva = "Y" AndAlso Not String.IsNullOrEmpty(SerieCita) AndAlso Not String.IsNullOrEmpty(NumeroCita) Then
            '    Query = String.Format(Query, SerieCita, NumeroCita)
            '    Cuenta = DMS_Connector.Helpers.EjecutarConsulta(Query)
            '    If Cuenta > 0 Then
            '        Resultado = False
            '    End If
            'End If

            Return Resultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return Resultado
        End Try
    End Function

    Public Sub ManejadorEventoMenu(ByRef oForm As Form,
                                   ByVal pval As MenuEvent, _
                                          ByRef BubbleEvent As Boolean)

        Try
            Select Case pval.MenuUID
                Case "1282"
                    Dim oCombo As ComboBox = DirectCast(oForm.Items.Item(mc_strcbSucursal).Specific, ComboBox)
                    oCombo.Select(Utilitarios.ObtieneIdSucursal(DMS_Connector.Company.ApplicationSBO).ToString, BoSearchKey.psk_ByDescription)
                    oForm.Items.Item(mc_strcbSucursal).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                    oForm.Items.Item("SCGD_etCOT").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                    oForm.Items.Item("SCGD_etNOT").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                    oForm.Items.Item("btnSN").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)

                    If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                        oCombo.Select(oForm.DataSources.DBDataSources.Item("OQUT").GetValue("BPLId", 0).Trim(), BoSearchKey.psk_ByValue)
                    Else
                        oCombo.Select(Utilitarios.ObtieneIdSucursal(DMS_Connector.Company.ApplicationSBO).ToString, BoSearchKey.psk_ByDescription)
                    End If

                Case "1287"
                    If Not String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).Trim()) AndAlso oForm.DataSources.DBDataSources.Item("OQUT").GetValue("CANCELED", 0).Trim() = "N" Then
                        BubbleEvent = False
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.NoPermiteDuplicar, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                    End If
            End Select
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub
    Public Sub ManejadorEventoComboBox(ByRef oForm As Form,
                                        ByVal pval As ItemEvent, _
                                        ByRef BubbleEvent As Boolean)
        Dim strIdSucur As String = String.Empty
        Dim cboSucu As ComboBox
        Dim strSerie As String = String.Empty
        Try
            If Not pval.BeforeAction Then
                Dim oCombo As ComboBox = DirectCast(oForm.Items.Item("88").Specific, ComboBox)

                Select Case pval.ItemUID
                    Case mc_strcbSucursal

                        strIdSucur = oForm.DataSources.DBDataSources.Item("OQUT").GetValue(mc_strIdSucursal, 0).TrimEnd()

                        If Not oForm.Mode = BoFormMode.fm_FIND_MODE And oForm.Items.Item("88").Enabled Then
                            strSerie = Utilitarios.EjecutarConsulta(String.Format(" SELECT U_DesSOfV FROM [@SCGD_CONF_SUCURSAL] with (nolock) WHERE U_Sucurs = '{0}' ",
                                                                                                               oForm.DataSources.DBDataSources.Item("OQUT").GetValue(mc_strIdSucursal, 0).TrimEnd()),
                                                                                                 m_oCompany.CompanyDB, m_oCompany.Server)
                            If Not String.IsNullOrEmpty(strSerie) Then
                                oCombo.Select(strSerie, BoSearchKey.psk_ByDescription)
                            Else
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.DefinaSerieValida, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                            End If

                            Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, oForm.DataSources.DBDataSources.Item("OQUT").GetValue(mc_strIdSucursal, 0).TrimEnd(), strCadenaConexionBDTaller)
                            objTransferenciaStock = New TransferenciaItems(SBO_Application, m_oCompany, strCadenaConexionBDTaller)

                        End If

                    Case "2001"
                        Dim selectedBranchID As String = oForm.DataSources.DBDataSources.Item("OQUT").GetValue("BPLId", 0).TrimEnd()
                        cboSucu = DirectCast(oForm.Items.Item("SCGD_cbSuc").Specific, ComboBox)

                        cboSucu.Select(selectedBranchID, BoSearchKey.psk_ByValue)
                        'oForm.DataSources.DBDataSources.Item("OQUT").SetValue(mc_strIdSucursal, 0, selectedBranchID)

                        strIdSucur = oForm.DataSources.DBDataSources.Item("OQUT").GetValue(mc_strIdSucursal, 0).TrimEnd()

                        If Not oForm.Mode = BoFormMode.fm_FIND_MODE And oForm.Items.Item("88").Enabled Then
                            strSerie = Utilitarios.EjecutarConsulta(String.Format(" SELECT U_DesSOfV FROM [@SCGD_CONF_SUCURSAL] with (nolock) WHERE U_Sucurs = '{0}' ",
                                                                                 oForm.DataSources.DBDataSources.Item("OQUT").GetValue(mc_strIdSucursal, 0).TrimEnd()),
                                                                   m_oCompany.CompanyDB, m_oCompany.Server)
                            If Not String.IsNullOrEmpty(strSerie) Then
                                oCombo.Select(strSerie, BoSearchKey.psk_ByDescription)
                            Else
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.DefinaSerieValida, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                            End If

                            Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, oForm.DataSources.DBDataSources.Item("OQUT").GetValue(mc_strIdSucursal, 0).TrimEnd(), strCadenaConexionBDTaller)
                            objTransferenciaStock = New TransferenciaItems(SBO_Application, m_oCompany, strCadenaConexionBDTaller)

                        End If
                End Select
            Else

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try



    End Sub

    Public Sub AsignaAutomatica(ByVal p_strDocNum As String)

        Try
            Dim strDocEntry As String
            Dim intDocEntry As Integer

            If Not String.IsNullOrEmpty(p_strDocNum) Then

                strDocEntry = Utilitarios.EjecutarConsulta("Select DocEntry from OQUT with(nolock) where U_SCGD_NoOT = '" & p_strDocNum & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                If IsNumeric(strDocEntry) Then
                    intDocEntry = CInt(strDocEntry)
                Else
                    intDocEntry = 0
                End If

                m_oCotizacionAnterior = CType(m_oCompany.GetBusinessObject(BoObjectTypes.oQuotations),  _
                                      Documents)

                oCotizacionlocal = CType(m_oCompany.GetBusinessObject(BoObjectTypes.oQuotations),  _
                                      Documents)


            End If



        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Sub


    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPressed_TallerExterno(ByVal FormUID As String, _
                                          ByRef pVal As SAPbouiCOM.ItemEvent, _
                                          ByRef BubbleEvent As Boolean)

        Dim oitem As SAPbouiCOM.Item
        Dim sbutton As SAPbouiCOM.Button
        Dim oEditText As SAPbouiCOM.EditText
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim strDocNum As String
        Dim oform As SAPbouiCOM.Form
        Dim oComboTipoPago As SAPbouiCOM.ComboBox
        Dim oComboDptoServ As SAPbouiCOM.ComboBox

        Try
            If Not CatchingEvents.m_blnUsaOrdenesDeTrabajo Then
                Exit Sub
            End If
            '*****Valida si Usa OT en SAP *****
            If DMS_Connector.Configuracion.ParamGenAddon.U_OT_SAP <> "Y" Then

                m_oFormGenCotizacion = SBO_Application.Forms.Item(pVal.FormUID)
                If pVal.ItemUID = mc_strIDBotonEjecucion _
                    AndAlso pVal.BeforeAction _
                    AndAlso Not pVal.ActionSuccess Then

                    m_blnActualizar = False

                    blnValidarCamposHS_KM = True

                    oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                    m_oForm = oform
                    oitem = oform.Items.Item(mc_strIDBotonEjecucion)
                    sbutton = CType(oitem.Specific, SAPbouiCOM.Button)


                    If oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or oform.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then

                        strIdSucursal = m_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).TrimEnd
                        Dim strNoOT As String = String.Empty
                        Dim strGeneraOT As String = String.Empty

                        oitem = oform.Items.Item("SCGD_etOT")
                        oEditText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
                        strNoOT = oEditText.String

                        oitem = oform.Items.Item("SCGD_cbGOT")
                        oCombo = DirectCast(oitem.Specific, SAPbouiCOM.ComboBox)
                        strGeneraOT = oCombo.Selected.Value

                        If Not String.IsNullOrEmpty(strIdSucursal) AndAlso strGeneraOT.Trim() = "1" Then
                            If Not ValidarKilometraje_HorasServicio(BubbleEvent) Then
                                BubbleEvent = False
                                Exit Sub
                            End If
                        End If

                        If Not String.IsNullOrEmpty(strNoOT) And strGeneraOT.Trim() = "2" Then
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.CampoGeneraOTNo, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                            Exit Sub
                        End If

                        Dim usaInterFazFord = Utilitarios.UsaInterfazFord(m_oCompany)
                        If usaInterFazFord Then
                            Dim socioNegTip = Utilitarios.ValidaIFTipoSN(m_oCompany, oform.DataSources.DBDataSources.Item("OQUT").GetValue("CardCode", 0))

                            If Not socioNegTip Then
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.TXTValidaTipoSN, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                BubbleEvent = False
                                Exit Sub
                            End If

                            oComboTipoPago = oform.Items.Item(mc_strCboTipoPago).Specific
                            oComboDptoServ = oform.Items.Item(mc_strCboDptoSrv).Specific

                            If String.IsNullOrEmpty(oComboDptoServ.Value) Or String.IsNullOrEmpty(oComboTipoPago.Value) Then
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.TXTValidaTipoPagoDptoServ, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                BubbleEvent = False
                                Exit Sub
                            End If
                        End If


                        If SBO_Application.MessageBox(My.Resources.Resource.GuardarCambios, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then

                            oitem = oform.Items.Item(mc_strDocNumCotización)
                            oEditText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
                            If IsNumeric(oEditText.String) Then
                                DocNum = CInt(oEditText.String)

                                If oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then

                                    m_blnActualizar = True
                                    oitem = oform.Items.Item(mc_strDocNumCotización)
                                    oEditText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
                                    strDocNum = oEditText.Value
                                    CargarCotizacionAnterior(strDocNum)

                                End If

                            End If
                            strIdSucursal = m_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).TrimEnd
                            If (strIdSucursal <> "") Then
                                oitem = oform.Items.Item(mc_strComboGeneraOT)
                                oCombo = DirectCast(oitem.Specific, SAPbouiCOM.ComboBox)
                                m_intGeneraOTPantalla = IIf(IsNumeric(oCombo.Selected.Value), oCombo.Selected.Value, 1)

                                oitem = oform.Items.Item(mc_strComboImprimeOR)
                                oCombo = DirectCast(oitem.Specific, SAPbouiCOM.ComboBox)
                                m_intImprimeORPantalla = IIf(IsNumeric(oCombo.Selected.Value), oCombo.Selected.Value, 1)
                            End If

                        Else

                            BubbleEvent = False

                        End If

                        'End If

                    End If

                    m_strCaptionBefore = sbutton.Caption

                Else
                    Dim boolExisteForm As Boolean = False
                    If pVal.ItemUID = mc_strIDBotonEjecucion _
                    AndAlso Not pVal.BeforeAction _
                    AndAlso pVal.ActionSuccess Then

                        If m_blnActualizar AndAlso (m_strCaptionBefore = mc_strActualizar Or m_strCaptionBefore = mc_strUpdate) Then
                            RecorrerCotizacionesSinProcesar(pVal)
                        Else
                            If Not m_blnActualizar AndAlso (m_strCaptionBefore = mc_strCrear Or m_strCaptionBefore = mc_strAdd) Then
                                RecorrerCotizacionesSinProcesar(pVal)
                            Else
                                m_blnActualizar = False
                            End If
                        End If

                    Else
                        Select Case pVal.ItemUID
                            Case mc_strBtnSolOtEsp
                                If (pVal.Before_Action) Then
                                    oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                                    If oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrFormQuotationUpdateMode, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                        BubbleEvent = False
                                    ElseIf oform.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                        boolExisteForm = Utilitarios.ValidarSiFormularioAbierto(g_strFormSolicitaOTEsp, True, SBO_Application)
                                        If Not boolExisteForm Then
                                            CargarFormularioSolicitaOTEspecial(pVal, BubbleEvent)
                                        End If
                                    End If
                                ElseIf Not pVal.Before_Action Then
                                    Dim numOT As String
                                    Dim DocEntry As String
                                    oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                                    numOT = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).Trim()
                                    DocEntry = oform.DataSources.DBDataSources.Item("OQUT").GetValue("DocEntry", 0).Trim()

                                    Dim query As String = String.Format("Select DocStatus from OQUT with (nolock) where U_SCGD_Numero_OT = '{0}'", numOT)
                                    Dim result As String = Utilitarios.EjecutarConsulta(query, SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)
                                    'boolExisteForm = Utilitarios.ValidarSiFormularioAbierto(g_strFormSolicitaOTEsp, True, SBO_Application)
                                    'If Not boolExisteForm Then
                                    If result = "O" Then

                                        Dim blnUsaTallerOTSAP As Boolean = False
                                        If Utilitarios.ValidarOTInternaConfiguracion(m_oCompany) Then
                                            blnUsaTallerOTSAP = True
                                        End If
                                        oFormSolOTEspecial.CargaCOT_OT(pVal, numOT, DocEntry)
                                        oFormSolOTEspecial.LoadMatrixLines(blnUsaTallerOTSAP, g_strCreaHjaCanPend)
                                    End If
                                End If

                            Case mc_strBtnAsigMult

                                oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                                Dim DocEntry As String = oform.DataSources.DBDataSources.Item("OQUT").GetValue("DocEntry", 0).Trim
                                Dim DocStatus As String = oform.DataSources.DBDataSources.Item("OQUT").GetValue("DocStatus", 0).Trim
                                Dim idSuc As String = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).Trim
                                Dim numOT As String = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).Trim
                                Dim queryServ As String = String.Empty
                                Dim resultServ As String = String.Empty
                                Dim itemCode As SAPbouiCOM.EditText
                                Dim strCode = String.Empty

                                Dim mtxCot As SAPbouiCOM.Matrix = DirectCast(oform.Items.Item("38").Specific, SAPbouiCOM.Matrix)


                                If pVal.BeforeAction Then

                                    If oform.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or oform.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                        boolExisteForm = Utilitarios.ValidarSiFormularioAbierto(g_strAsignacionMultiple, True, SBO_Application)

                                        If Not boolExisteForm Then

                                            queryServ = "select count(q.docentry) from QUT1 q with (nolock) left join OITM i with (nolock) on q.ItemCode = i.ItemCode where q.docentry = '{0}' and i.U_SCGD_TipoArticulo =2"
                                            queryServ = String.Format(queryServ, DocEntry)
                                            resultServ = Utilitarios.EjecutarConsulta(queryServ, SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                                            If CInt(resultServ) > 0 Then
                                                CargarFormularioAsignacionMultiple(pVal, BubbleEvent, DocStatus, numOT, idSuc)
                                            Else
                                                If mtxCot.RowCount - 1 > 0 Then
                                                    queryServ = String.Empty
                                                    queryServ = "select COUNT(ItemCode) from OITM q with (nolock) where q.U_SCGD_TipoArticulo = 2 and q.itemCode in ({0})"

                                                    For y As Integer = 1 To mtxCot.RowCount - 1
                                                        itemCode = DirectCast(mtxCot.Columns.Item("1").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                                                        If String.IsNullOrEmpty(strCode) Then
                                                            strCode = String.Format("'{0}'", itemCode.Value.Trim())
                                                        Else
                                                            strCode = String.Format("{0}, '{1}'", strCode, itemCode.Value.Trim())
                                                        End If
                                                    Next

                                                    queryServ = String.Format(queryServ, strCode)
                                                    resultServ = Utilitarios.EjecutarConsulta(queryServ, SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                                                    If CInt(resultServ) > 0 Then
                                                        CargarFormularioAsignacionMultiple(pVal, BubbleEvent, DocStatus, numOT, idSuc)
                                                    Else
                                                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrNoWorksToAssign, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                                    End If
                                                Else
                                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrNoWorksToAssign, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                                End If
                                            End If
                                        End If
                                    End If

                                ElseIf (pVal.ActionSuccess) AndAlso BubbleEvent Then

                                    If DocStatus = "O" Then
                                        queryServ = "select count(q.docentry) from QUT1 q with (nolock) left join OITM i with (nolock) on q.ItemCode = i.ItemCode where q.docentry = '{0}' and i.U_SCGD_TipoArticulo =2"
                                        queryServ = String.Format(queryServ, DocEntry)
                                        resultServ = Utilitarios.EjecutarConsulta(queryServ, SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                                        If CInt(resultServ) > 0 Then
                                            oFormAsignacionMultiple.CargaCOT_OT(pVal, numOT, DocEntry, idSuc)
                                            oFormAsignacionMultiple.LoadMatrixLines(pVal.FormTypeEx)
                                        Else
                                            If mtxCot.RowCount - 1 > 0 Then
                                                queryServ = String.Empty
                                                queryServ = "select COUNT(ItemCode) from OITM q with (nolock) where q.U_SCGD_TipoArticulo = 2 and q.itemCode in ({0})"

                                                For y As Integer = 1 To mtxCot.RowCount - 1
                                                    itemCode = DirectCast(mtxCot.Columns.Item("1").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                                                    If String.IsNullOrEmpty(strCode) Then
                                                        strCode = String.Format("'{0}'", itemCode.Value.Trim())
                                                    Else
                                                        strCode = String.Format("{0}, '{1}'", strCode, itemCode.Value.Trim())
                                                    End If
                                                Next

                                                queryServ = String.Format(queryServ, strCode)
                                                resultServ = Utilitarios.EjecutarConsulta(queryServ, SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                                                If CInt(resultServ) > 0 Then
                                                    oFormAsignacionMultiple.CargaCOT_OT(pVal, "", "", idSuc)
                                                    oFormAsignacionMultiple.LoadMatrixLines(pVal.FormTypeEx)
                                                Else
                                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrNoWorksToAssign, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                                End If
                                            Else
                                                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrNoWorksToAssign, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                            End If
                                        End If
                                    End If
                                End If

                        End Select

                    End If
                End If
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                          ByRef pVal As SAPbouiCOM.ItemEvent, _
                                          ByRef BubbleEvent As Boolean)

        Dim oitem As SAPbouiCOM.Item
        Dim sbutton As SAPbouiCOM.Button
        Dim oEditText As SAPbouiCOM.EditText
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim strDocNum As String
        Dim oform As SAPbouiCOM.Form
        Dim oComboTipoPago As SAPbouiCOM.ComboBox
        Dim oComboDptoServ As SAPbouiCOM.ComboBox

        m_oFormGenCotizacion = SBO_Application.Forms.Item(pVal.FormUID)

        Try
            If pVal.ItemUID = mc_strIDBotonEjecucion _
                AndAlso pVal.BeforeAction _
                AndAlso Not pVal.ActionSuccess Then

                m_blnActualizar = False

                blnValidarCamposHS_KM = True

                oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                m_oForm = oform
                oitem = oform.Items.Item(mc_strIDBotonEjecucion)
                sbutton = CType(oitem.Specific, SAPbouiCOM.Button)


                If oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or oform.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then

                    strIdSucursal = m_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).TrimEnd
                    Dim strNoOT As String = String.Empty
                    Dim strGeneraOT As String = String.Empty

                    oitem = oform.Items.Item("SCGD_etOT")
                    oEditText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
                    strNoOT = oEditText.String

                    oitem = oform.Items.Item("SCGD_cbGOT")
                    oCombo = DirectCast(oitem.Specific, SAPbouiCOM.ComboBox)
                    strGeneraOT = oCombo.Selected.Value

                    If Not String.IsNullOrEmpty(strIdSucursal) AndAlso strGeneraOT.Trim() = "1" AndAlso oform.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                        If Not ValidarKilometraje_HorasServicio(BubbleEvent) Then
                            BubbleEvent = False
                            Exit Sub
                        End If
                    End If

                    If Not String.IsNullOrEmpty(strNoOT) And strGeneraOT.Trim() = "2" Then
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.CampoGeneraOTNo, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        BubbleEvent = False
                        Exit Sub
                    End If

                    Dim usaInterFazFord = Utilitarios.UsaInterfazFord(m_oCompany)
                    If usaInterFazFord Then
                        Dim socioNegTip = Utilitarios.ValidaIFTipoSN(m_oCompany, oform.DataSources.DBDataSources.Item("OQUT").GetValue("CardCode", 0))

                        If Not socioNegTip Then
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.TXTValidaTipoSN, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                            BubbleEvent = False
                            Exit Sub
                        End If

                        oComboTipoPago = oform.Items.Item(mc_strCboTipoPago).Specific
                        oComboDptoServ = oform.Items.Item(mc_strCboDptoSrv).Specific

                        If String.IsNullOrEmpty(oComboDptoServ.Value) Or String.IsNullOrEmpty(oComboTipoPago.Value) Then
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.TXTValidaTipoPagoDptoServ, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                            BubbleEvent = False
                            Exit Sub
                        End If
                    End If


                    If SBO_Application.MessageBox(My.Resources.Resource.GuardarCambios, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then

                        oitem = oform.Items.Item(mc_strDocNumCotización)
                        oEditText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
                        If IsNumeric(oEditText.String) Then
                            DocNum = CInt(oEditText.String)

                            If oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then

                                m_blnActualizar = True
                                oitem = oform.Items.Item(mc_strDocNumCotización)
                                oEditText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
                                strDocNum = oEditText.Value
                                CargarCotizacionAnterior(strDocNum)

                            End If

                        End If
                        strIdSucursal = m_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).TrimEnd
                        If (strIdSucursal <> "") Then
                            oitem = oform.Items.Item(mc_strComboGeneraOT)
                            oCombo = DirectCast(oitem.Specific, SAPbouiCOM.ComboBox)
                            m_intGeneraOTPantalla = IIf(IsNumeric(oCombo.Selected.Value), oCombo.Selected.Value, 1)

                            oitem = oform.Items.Item(mc_strComboImprimeOR)
                            oCombo = DirectCast(oitem.Specific, SAPbouiCOM.ComboBox)
                            m_intImprimeORPantalla = IIf(IsNumeric(oCombo.Selected.Value), oCombo.Selected.Value, 1)
                        End If

                    Else

                        BubbleEvent = False

                    End If

                    'End If

                End If

                m_strCaptionBefore = sbutton.Caption

            Else
                Dim boolExisteForm As Boolean = False
                If pVal.ItemUID = mc_strIDBotonEjecucion _
                AndAlso Not pVal.BeforeAction _
                AndAlso pVal.ActionSuccess Then

                    If m_blnActualizar AndAlso (m_strCaptionBefore = mc_strActualizar Or m_strCaptionBefore = mc_strUpdate) Then
                        RecorrerCotizacionesSinProcesar(pVal)
                    Else
                        If Not m_blnActualizar AndAlso (m_strCaptionBefore = mc_strCrear Or m_strCaptionBefore = mc_strAdd) Then
                            RecorrerCotizacionesSinProcesar(pVal)
                        Else
                            m_blnActualizar = False
                        End If
                    End If

                Else
                    Select Case pVal.ItemUID
                        Case mc_strBtnSolOtEsp
                            If (pVal.Before_Action) Then
                                oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                                If oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrFormQuotationUpdateMode, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                ElseIf oform.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                    boolExisteForm = Utilitarios.ValidarSiFormularioAbierto(g_strFormSolicitaOTEsp, True, SBO_Application)
                                    If Not boolExisteForm Then
                                        CargarFormularioSolicitaOTEspecial(pVal, BubbleEvent)
                                    End If
                                End If
                            ElseIf Not pVal.Before_Action Then
                                Dim numOT As String
                                Dim DocEntry As String
                                oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                                numOT = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).Trim()
                                DocEntry = oform.DataSources.DBDataSources.Item("OQUT").GetValue("DocEntry", 0).Trim()

                                Dim query As String = String.Format("Select DocStatus from OQUT with (nolock) where U_SCGD_Numero_OT = '{0}'", numOT)
                                Dim result As String = Utilitarios.EjecutarConsulta(query, SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)
                                'boolExisteForm = Utilitarios.ValidarSiFormularioAbierto(g_strFormSolicitaOTEsp, True, SBO_Application)
                                'If Not boolExisteForm Then
                                If result = "O" Then

                                    Dim blnUsaTallerOTSAP As Boolean = False
                                    If Utilitarios.ValidarOTInternaConfiguracion(m_oCompany) Then
                                        blnUsaTallerOTSAP = True
                                    End If
                                    oFormSolOTEspecial.CargaCOT_OT(pVal, numOT, DocEntry)
                                    oFormSolOTEspecial.LoadMatrixLines(blnUsaTallerOTSAP, g_strCreaHjaCanPend)
                                End If
                            End If

                        Case mc_strBtnAsigMult

                            oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                            Dim DocEntry As String = oform.DataSources.DBDataSources.Item("OQUT").GetValue("DocEntry", 0).Trim
                            Dim DocStatus As String = oform.DataSources.DBDataSources.Item("OQUT").GetValue("DocStatus", 0).Trim
                            Dim idSuc As String = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).Trim
                            Dim numOT As String = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).Trim
                            Dim queryServ As String = String.Empty
                            Dim resultServ As String = String.Empty
                            Dim itemCode As SAPbouiCOM.EditText
                            Dim strCode = String.Empty

                            Dim mtxCot As SAPbouiCOM.Matrix = DirectCast(oform.Items.Item("38").Specific, SAPbouiCOM.Matrix)


                            If pVal.BeforeAction Then

                                If oform.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or oform.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                    boolExisteForm = Utilitarios.ValidarSiFormularioAbierto(g_strAsignacionMultiple, True, SBO_Application)

                                    If Not boolExisteForm Then

                                        queryServ = "select count(q.docentry) from QUT1 q with (nolock) left join OITM i with (nolock) on q.ItemCode = i.ItemCode where q.docentry = '{0}' and i.U_SCGD_TipoArticulo =2"
                                        queryServ = String.Format(queryServ, DocEntry)
                                        resultServ = Utilitarios.EjecutarConsulta(queryServ, SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                                        If CInt(resultServ) > 0 Then
                                            CargarFormularioAsignacionMultiple(pVal, BubbleEvent, DocStatus, numOT, idSuc)
                                        Else
                                            If mtxCot.RowCount - 1 > 0 Then
                                                queryServ = String.Empty
                                                queryServ = "select COUNT(ItemCode) from OITM q with (nolock) where q.U_SCGD_TipoArticulo = 2 and q.itemCode in ({0})"

                                                For y As Integer = 1 To mtxCot.RowCount - 1
                                                    itemCode = DirectCast(mtxCot.Columns.Item("1").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                                                    If String.IsNullOrEmpty(strCode) Then
                                                        strCode = String.Format("'{0}'", itemCode.Value.Trim())
                                                    Else
                                                        strCode = String.Format("{0}, '{1}'", strCode, itemCode.Value.Trim())
                                                    End If
                                                Next

                                                queryServ = String.Format(queryServ, strCode)
                                                resultServ = Utilitarios.EjecutarConsulta(queryServ, SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                                                If CInt(resultServ) > 0 Then
                                                    CargarFormularioAsignacionMultiple(pVal, BubbleEvent, DocStatus, numOT, idSuc)
                                                Else
                                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrNoWorksToAssign, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                                End If
                                            Else
                                                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrNoWorksToAssign, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                            End If
                                        End If
                                    End If
                                End If

                            ElseIf (pVal.ActionSuccess) AndAlso BubbleEvent Then

                                If DocStatus = "O" Then
                                    queryServ = "select count(q.docentry) from QUT1 q with (nolock) left join OITM i with (nolock) on q.ItemCode = i.ItemCode where q.docentry = '{0}' and i.U_SCGD_TipoArticulo =2"
                                    queryServ = String.Format(queryServ, DocEntry)
                                    resultServ = Utilitarios.EjecutarConsulta(queryServ, SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                                    If CInt(resultServ) > 0 Then
                                        oFormAsignacionMultiple.CargaCOT_OT(pVal, numOT, DocEntry, idSuc)
                                        oFormAsignacionMultiple.LoadMatrixLines(pVal.FormTypeEx)
                                    Else
                                        If mtxCot.RowCount - 1 > 0 Then
                                            queryServ = String.Empty
                                            queryServ = "select COUNT(ItemCode) from OITM q with (nolock) where q.U_SCGD_TipoArticulo = 2 and q.itemCode in ({0})"

                                            For y As Integer = 1 To mtxCot.RowCount - 1
                                                itemCode = DirectCast(mtxCot.Columns.Item("1").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                                                If String.IsNullOrEmpty(strCode) Then
                                                    strCode = String.Format("'{0}'", itemCode.Value.Trim())
                                                Else
                                                    strCode = String.Format("{0}, '{1}'", strCode, itemCode.Value.Trim())
                                                End If
                                            Next

                                            queryServ = String.Format(queryServ, strCode)
                                            resultServ = Utilitarios.EjecutarConsulta(queryServ, SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                                            If CInt(resultServ) > 0 Then
                                                oFormAsignacionMultiple.CargaCOT_OT(pVal, "", "", idSuc)
                                                oFormAsignacionMultiple.LoadMatrixLines(pVal.FormTypeEx)
                                            Else
                                                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrNoWorksToAssign, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                            End If
                                        Else
                                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrNoWorksToAssign, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                        End If
                                    End If
                                End If
                            End If

                    End Select

                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    <CLSCompliant(False)> _
    Public Sub ManejadorEventoClickedPress(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        'Validacion para evitar que se le cambie el tipo a una linea ya procesada
        If pVal.ItemUID = mc_strIDMatriz AndAlso pVal.ColUID = mc_strColTipoLinea _
                AndAlso pVal.BeforeAction AndAlso Not pVal.ActionSuccess Then
            Dim oform As SAPbouiCOM.Form
            Dim idRepXOrd As String
            Dim oMatrix As SAPbouiCOM.Matrix

            oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
            oform.Freeze(True)
            oMatrix = DirectCast(oform.Items.Item(mc_strIDMatriz).Specific, SAPbouiCOM.Matrix)
            If oMatrix.RowCount - 1 >= pVal.Row And pVal.Row <> 0 Then
                'idRepXOrd = oform.DataSources.DBDataSources.Item("QUT1").GetValue("U_SCGD_IdRepxOrd", (pVal.Row - 1)).Trim()
                idRepXOrd = oMatrix.Columns.Item("U_SCGD_IdRepxOrd").Cells.Item(pVal.Row).Specific.Value.ToString().Trim()
                If Not String.IsNullOrEmpty(idRepXOrd) Then
                    BubbleEvent = False
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCambioTipoLineaCot, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                End If
            End If
            oform.Freeze(False)
        End If
    End Sub

    Public Sub ManejadorEventoFormData(ByVal oform As Form)

        Dim oitem As Item
        Dim oMatrix As Matrix
        Dim strEstadoCotizacion As String
        Dim strNoOt As String
        Dim m_dtMecanicosAsignados As DataTable
        Dim strBloquearColumnaAprobacion As String = String.Empty

        Try

            g_strTiemEsta = DMS_Connector.Configuracion.ParamGenAddon.U_TiemEsta.Trim
            g_strReducCant = DMS_Connector.Configuracion.ParamGenAddon.U_ReduceCant.Trim

            strNoOt = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).Trim
            strEstadoCotizacion = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Estado_CotID", 0).Trim

            oMatrix = DirectCast(oform.Items.Item(mc_strIDMatriz).Specific, Matrix)

            'Bloqueo la Columna Trasladado permanentemente
            If oMatrix.Columns.Item("U_SCGD_Traslad").Visible Then
                oMatrix.Columns.Item("U_SCGD_Traslad").Editable = False
            End If

            'Bloqueo la columna Aprobado cuando la Cotizacion esta en: Cerrada o Cancelado
            If oMatrix.Columns.Item(mc_strItemAprobado).Visible Then

                If Not String.IsNullOrEmpty(strEstadoCotizacion) Then

                    If (strEstadoCotizacion <> "1" And strEstadoCotizacion <> "2" And strEstadoCotizacion <> "3") Then
                        oMatrix = DirectCast(oform.Items.Item(mc_strIDMatriz).Specific, Matrix)
                        oMatrix.Columns.Item(mc_strItemAprobado).Editable = False
                    Else
                        oMatrix = DirectCast(oform.Items.Item(mc_strIDMatriz).Specific, Matrix)
                        oMatrix.Columns.Item(mc_strItemAprobado).Editable = True
                    End If
                Else
                    oMatrix = DirectCast(oform.Items.Item(mc_strIDMatriz).Specific, Matrix)
                    oMatrix.Columns.Item(mc_strItemAprobado).Editable = True
                End If

            End If

            strBloquearColumnaAprobacion = Utilitarios.EjecutarConsulta("SELECT U_BloqApro FROM [@SCGD_ADMIN] with (nolock)", m_oCompany.CompanyDB, m_oCompany.Server)

            If strBloquearColumnaAprobacion.ToUpper() = "Y" Then
                'Inhabilita la columna aprobado de la cotización para los usuarios indicados.
                If DMS_Connector.Helpers.PermisosMenu("SCGD_BEA") Then
                    'Desactiva la columna aprobado
                    oMatrix.Columns.Item(mc_strItemAprobado).Editable = False
                End If
            End If

            ''Validaciones para agregar boton de solicitud de ot especial
            If oform.DataSources.UserDataSources.Item("btnSolOT").Value = "Y" Then
                oitem = oform.Items.Item(mc_strBtnSolOtEsp)
                Dim strIdSucurs As String = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).ToString().Trim()

                Dim strConsulta As String = String.Format("select U_USolOTEsp from [@SCGD_CONF_SUCURSAL] with (nolock) where U_Sucurs ='{0}'", strIdSucurs)
                Dim strCode As String = Utilitarios.EjecutarConsulta(strConsulta,
                                                       SBO_Application.Company.DatabaseName,
                                                       SBO_Application.Company.ServerName)
                If strCode = "N" Then
                    oitem.Visible = False
                Else
                    oitem.Visible = True

                    If String.IsNullOrEmpty(strNoOt) Then
                        oitem.Visible = False
                    Else
                        If oform.DataSources.DBDataSources.Item("OQUT").GetValue("DocStatus", 0).Trim() = "O" Then
                            oitem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_True)
                        Else
                            oitem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)
                        End If
                    End If
                End If

            End If

            If oform.DataSources.UserDataSources.Item("btnAsMul").Value = "Y" Then
                oitem = oform.Items.Item(mc_strBtnAsigMult)

                oitem.Visible = True

                If oform.DataSources.DBDataSources.Item("OQUT").GetValue("DocStatus", 0).Trim() = "O" Then
                    If strEstadoCotizacion <> "4" AndAlso strEstadoCotizacion <> "5" Then
                        oitem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                    Else
                        oitem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)
                    End If
                Else
                    oitem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)
                End If
            End If

            m_dtMecanicosAsignados = oform.DataSources.DataTables.Item("MecanicosAsignados")
            If m_dtMecanicosAsignados.Rows.Count > 0 Then
                m_dtMecanicosAsignados.Rows.Clear()
            End If

            If Not String.IsNullOrEmpty(oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).Trim()) OrElse
                Not String.IsNullOrEmpty(oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_NoOtRef", 0).Trim()) OrElse
                Not String.IsNullOrEmpty(oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_NoCita", 0).Trim()) Then
                oform.Items.Item("16").Click()
                oform.Items.Item(mc_strcbSucursal).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_False)
                oform.Items.Item("SCGD_etCOT").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_False)
                oform.Items.Item("SCGD_etNOT").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_False)
                oform.Items.Item("btnSN").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_False)
            Else
                oform.Items.Item(mc_strcbSucursal).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                oform.Items.Item("SCGD_etCOT").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                oform.Items.Item("SCGD_etNOT").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                oform.Items.Item("btnSN").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
            End If
            Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, oform.DataSources.DBDataSources.Item("OQUT").GetValue(mc_strIdSucursal, 0).TrimEnd(), strCadenaConexionBDTaller)
            objTransferenciaStock = New TransferenciaItems(SBO_Application, m_oCompany, strCadenaConexionBDTaller)
            ValidaModoVistaForm(oform)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Public Sub ManejadorEventoLoad(ByVal FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
     

        Try
            If pVal.BeforeAction Then
               

                'If oForm IsNot Nothing Then
                '    g_dtConsulta = oForm.DataSources.DataTables.Add(g_strDtConsul)
                'End If

                'oForm.DataSources.DataTables.Add("dtVehiculo")
                'oForm.DataSources.DataTables.Add("dtConsulta")


                'Dim dtMecanicosAsig As DataTable
                'dtMecanicosAsig = oForm.DataSources.DataTables.Add("MecanicosAsignados")

                'dtMecanicosAsig.Columns.Add("col_CodAct", BoFieldsType.ft_AlphaNumeric, 100)
                'dtMecanicosAsig.Columns.Add("col_CodEmp", BoFieldsType.ft_AlphaNumeric, 100)
                'dtMecanicosAsig.Columns.Add("col_LineNum", BoFieldsType.ft_AlphaNumeric, 100)
                'dtMecanicosAsig.Columns.Add("col_IdRepXOrd", BoFieldsType.ft_AlphaNumeric, 100)
                'dtMecanicosAsig.Columns.Add("col_NoOrden", BoFieldsType.ft_AlphaNumeric, 100)
                'dtMecanicosAsig.Columns.Add("col_Estado", BoFieldsType.ft_AlphaNumeric, 100)
                'dtMecanicosAsig.Columns.Add("col_NoFase", BoFieldsType.ft_AlphaNumeric, 100)
                'dtMecanicosAsig.Columns.Add("col_NomEmp", BoFieldsType.ft_AlphaNumeric, 100)
                'dtMecanicosAsig.Columns.Add("col_Added", BoFieldsType.ft_AlphaNumeric, 100)
                'dtMecanicosAsig.Columns.Add("col_DurEst", BoFieldsType.ft_AlphaNumeric, 100)
                'dtMecanicosAsig.Columns.Add("col_DurRe", BoFieldsType.ft_AlphaNumeric, 100)
                'dtMecanicosAsig.Columns.Add("col_PrecioSt", BoFieldsType.ft_AlphaNumeric, 100)
                'dtMecanicosAsig.Columns.Add("col_DesNoFase", BoFieldsType.ft_AlphaNumeric, 100)

                'Dim userDS As UserDataSources = oForm.DataSources.UserDataSources
                'userDS.Add("btnSolOT", BoDataType.dt_LONG_TEXT, 100)
                'userDS.Add("btnAsMul", BoDataType.dt_SHORT_TEXT, 1)

              

                ''Boton Solicitud de OT Especial
                'If AgregaBTNSolOtEsp(oForm, SBO_Application) Then
                '    userDS.Item("btnSolOT").Value = "Y"
                'End If

                ''Interfaz Ford
                'Dim usaInterFazFord = Utilitarios.UsaInterfazFord(m_oCompany)
                'If usaInterFazFord Then
                '    AgregaCamoposFI(oForm, SBO_Application)
                'End If

                'Boton Asingnacion Multiple
                'If AgregaBTNAsigMul(oForm, SBO_Application) Then
                '    userDS.Item("btnAsMul").Value = "Y"
                '    oItem = oForm.Items.Item(mc_strBtnAsigMult)
                '    oItem.Visible = True
                '    oItem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                'End If

                'oCombo = DirectCast(oForm.Items.Item(mc_strcbSucursal).Specific, SAPbouiCOM.ComboBox)

                'Call Utilitarios.CargarValidValuesEnCombos(oCombo.ValidValues, " SELECT Code, Name FROM [@SCGD_SUCURSALES] with (nolock) ")

                'If String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).TrimEnd()) AndAlso
                ' String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_NoOtRef", 0).Trim()) AndAlso
                ' String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_NoCita", 0).Trim()) Then
                '    If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                '        oCombo.Select(oForm.DataSources.DBDataSources.Item("OQUT").GetValue("BPLId", 0).Trim(), BoSearchKey.psk_ByValue)
                '    Else
                '        oCombo.Select(Utilitarios.ObtieneIdSucursal(DMS_Connector.Company.ApplicationSBO).ToString, BoSearchKey.psk_ByDescription)
                '    End If
                'End If

                'oForm.Items.Item("SCGD_etCOT").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                'oForm.Items.Item("SCGD_etNOT").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el formulario de tipo de ot para generar la factura
    ''' </summary>
    Private Sub CargarFormularioSolicitaOTEspecial(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim strPath As String
        Dim oForm As SAPbouiCOM.Form

        Try
            oForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            Dim strDocEntry As String = oForm.DataSources.DBDataSources.Item("OQUT").GetValue("DocEntry", 0).Trim
            Dim docStatus As String = oForm.DataSources.DBDataSources.Item("OQUT").GetValue("DocStatus", 0).Trim
            Dim strIdSucurs As String = oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).ToString().Trim

            If docStatus = "O" Then
                oGestorFormularios = New GestorFormularios(SBO_Application)
                oFormSolOTEspecial = New SolicitaOTEspecial(m_oCompany, SBO_Application)

                g_strCreaHjaCanPend = Utilitarios.EjecutarConsulta(String.Format("SELECT U_HjaCanPen FROM [@SCGD_CONF_SUCURSAL] with (nolock) WHERE U_Sucurs ='{0}'", strIdSucurs), SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

                If (oFormSolOTEspecial.VerificarEstadoTrasladoFilasCotizacion(CInt(strDocEntry), g_strCreaHjaCanPend)) Then
                    oFormSolOTEspecial.FormType = g_strFormSolicitaOTEsp '"SCGD_SOTE"
                    oFormSolOTEspecial.Titulo = My.Resources.Resource.TituloSolicitaOTEspecial
                    DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)
                    strPath = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormSolicitaOTEsp
                    oFormSolOTEspecial.NombreXml = strPath
                    oFormSolOTEspecial.FormularioSBO = oGestorFormularios.CargaFormulario(oFormSolOTEspecial)
                Else
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeNoCreaOTEspecialesPendienteTraslado, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    BubbleEvent = False
                End If
            Else
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ERR_SalesOferClosed, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el formulario de Asignacion Multiple
    ''' </summary>
    Private Sub CargarFormularioAsignacionMultiple(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal p_DocStatus As String, ByVal p_NoOT As String, ByVal p_IdSucursal As String)

        Dim strPath As String

        Try

            If p_DocStatus <> "C" Then
                'Variable Global
                NoOT = p_NoOT
                IdSucursal = p_IdSucursal
                oGestorFormularios = New GestorFormularios(SBO_Application)
                oFormAsignacionMultiple = New AsignacionMultiple(m_oCompany, SBO_Application)
                oFormAsignacionMultiple.FormType = g_strAsignacionMultiple
                oFormAsignacionMultiple.Titulo = My.Resources.Resource.TituloAsigancionMultiple
                strPath = System.Windows.Forms.Application.StartupPath & My.Resources.Resource.XMLFormAsignacionMultiple
                oFormAsignacionMultiple.NombreXml = strPath
                oFormAsignacionMultiple.IDSucursal = p_IdSucursal
                oFormAsignacionMultiple.FormularioSBO = oGestorFormularios.CargaFormulario(oFormAsignacionMultiple)
                oFormAsignacionMultiple.CargaMecanicosAsignados(pVal.FormUID)
            Else
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ERR_SalesOferClosed, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                BubbleEvent = False
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' Agrega Boton de Solicitar OT Especial desde la oferta de ventas
    ''' </summary>
    ''' <param name="oform">Objeto de Formulario</param>
    ''' <remarks></remarks>
    Public Shared Function AgregaBTNSolOtEsp(ByVal oform As SAPbouiCOM.Form, ByVal p_SBO_Application As SAPbouiCOM.Application) As Boolean

        Dim oItem As SAPbouiCOM.Item
        Dim result As Boolean = True
        Dim oButton As SAPbouiCOM.Button
        Dim intTop As Integer
        Dim intLeft As Integer
        Dim intHeight As Integer
        Dim intWidth As Integer

        Try

            If Utilitarios.MostrarMenu("SCGD_SOE", p_SBO_Application.Company.UserName) Then

                intTop = oform.Items.Item("10000329").Top
                intLeft = oform.Items.Item("10000329").Left
                intWidth = oform.Items.Item("10000329").Width
                intHeight = oform.Items.Item("10000329").Height

                oItem = oform.Items.Add(mc_strBtnSolOtEsp, SAPbouiCOM.BoFormItemTypes.it_BUTTON)
                oItem.Top = intTop - 23
                oItem.Left = intLeft
                oItem.Width = 65
                oItem.Height = intHeight
                oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 6, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 9, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                oItem.Enabled = False
                oItem.Visible = False

                oButton = oItem.Specific
                oButton.Type = SAPbouiCOM.BoButtonTypes.bt_Caption
                oButton.Caption = My.Resources.Resource.btn_SolOTEsp
            Else
                result = False
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Agrega Boton de Asignacion Multiple de tareas
    ''' </summary>
    ''' <param name="oform">Objeto de Formulario</param>
    ''' <remarks></remarks>
    Public Shared Function AgregaBTNAsigMul(ByVal oform As SAPbouiCOM.Form, ByVal p_SBO_Application As SAPbouiCOM.Application) As Boolean

        Dim oItem As SAPbouiCOM.Item
        Dim result As Boolean = True
        Dim oButton As SAPbouiCOM.Button
        Dim intTop As Integer
        Dim intLeft As Integer
        Dim intHeight As Integer
        Dim intWidth As Integer
        Dim strIdSucurs As String

        Try
            intTop = oform.Items.Item("230").Top
            intLeft = oform.Items.Item("230").Left
            intWidth = oform.Items.Item("1").Width
            intHeight = oform.Items.Item("1").Height

            oItem = oform.Items.Add(mc_strBtnAsigMult, SAPbouiCOM.BoFormItemTypes.it_BUTTON)
            oItem.Top = intTop + 34
            oItem.Left = intLeft
            oItem.Width = 65
            oItem.Height = intHeight
            oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 6, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 9, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oItem.Enabled = False
            oItem.Visible = False

            oButton = oItem.Specific
            oButton.Type = SAPbouiCOM.BoButtonTypes.bt_Caption
            oButton.Caption = My.Resources.Resource.btn_AsMul

        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function

    Public Shared Function AgregaCamoposFI(ByVal oform As SAPbouiCOM.Form, ByVal p_SBO_Application As SAPbouiCOM.Application) As Boolean

        Dim oItem As SAPbouiCOM.Item
        Dim result As Boolean = True
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim oStaticText As SAPbouiCOM.StaticText
        Dim intTop As Integer
        Dim intLeft As Integer
        Dim intHeight As Integer
        Dim intWidth As Integer

        Try
            ''Agrega combo tipo pago
            intTop = oform.Items.Item("SCGD_etNOT").Top
            intLeft = oform.Items.Item("SCGD_etNOT").Left
            intWidth = oform.Items.Item("SCGD_etNOT").Width
            intHeight = oform.Items.Item("SCGD_etNOT").Height

            oItem = oform.Items.Add(mc_strCboTipoPago, SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX)
            oItem.Top = intTop + 16
            oItem.Left = intLeft
            oItem.Width = intWidth
            oItem.Height = intHeight
            oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oItem.Enabled = True
            oItem.Visible = True
            oItem.DisplayDesc = True
            'oItem.FromPane = oform.Items.Item("SCGD_cbRTa").FromPane
            'oItem.ToPane = oform.Items.Item("SCGD_cbRTa").ToPane

            oCombo = oItem.Specific
            Call oCombo.DataBind.SetBound(True, mc_strOQUT, mc_strUDFTipoPago)

            ''Agrega combo Departamento Servicio
            intTop = oform.Items.Item("SCGD_cbSuc").Top
            intLeft = oform.Items.Item("SCGD_cbSuc").Left
            intWidth = oform.Items.Item("SCGD_cbSuc").Width
            intHeight = oform.Items.Item("SCGD_cbSuc").Height

            oItem = Nothing
            oItem = oform.Items.Add(mc_strCboDptoSrv, SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX)
            oItem.Top = intTop + 16
            oItem.Left = intLeft
            oItem.Width = intWidth
            oItem.Height = intHeight
            oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oItem.Enabled = True
            oItem.Visible = True
            oItem.DisplayDesc = True
            oItem.FromPane = oform.Items.Item(mc_strCboTipoPago).FromPane
            oItem.ToPane = oform.Items.Item(mc_strCboTipoPago).ToPane

            oCombo = Nothing
            oCombo = oItem.Specific
            Call oCombo.DataBind.SetBound(True, mc_strOQUT, mc_strUDFServDpto)

            ''agrega texto tipo pago
            intTop = oform.Items.Item("SCGD_stNOT").Top
            intLeft = oform.Items.Item("SCGD_stNOT").Left
            intWidth = oform.Items.Item("SCGD_stNOT").Width
            intHeight = oform.Items.Item("SCGD_stNOT").Height

            oItem = Nothing
            oItem = oform.Items.Add(mc_stTipoPago, SAPbouiCOM.BoFormItemTypes.it_STATIC)
            oItem.Top = intTop + 16
            oItem.Left = intLeft
            oItem.Width = intWidth
            oItem.Height = intHeight
            oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oItem.Enabled = True
            oItem.Visible = True
            'oItem.FromPane = oform.Items.Item("SCGD_stRTa").FromPane
            'oItem.ToPane = oform.Items.Item("SCGD_stRTa").ToPane

            oStaticText = oItem.Specific
            oStaticText.Item.LinkTo = mc_strCboTipoPago
            oStaticText.Caption = My.Resources.Resource.TXTTipoPago '"Tipo de Pago"

            ''agrega texto departamento servicio
            intTop = oform.Items.Item("SCGD_stSuc").Top
            intLeft = oform.Items.Item("SCGD_stSuc").Left
            intWidth = oform.Items.Item("SCGD_stSuc").Width
            intHeight = oform.Items.Item("SCGD_stSuc").Height

            oItem = Nothing
            oItem = oform.Items.Add(mc_stDptoSrv, SAPbouiCOM.BoFormItemTypes.it_STATIC)
            oItem.Top = intTop + 16
            oItem.Left = intLeft
            oItem.Width = intWidth
            oItem.Height = intHeight
            oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oItem.Enabled = True
            oItem.Visible = True
            oItem.FromPane = oform.Items.Item(mc_stTipoPago).FromPane
            oItem.ToPane = oform.Items.Item(mc_stTipoPago).ToPane

            oStaticText = Nothing
            oStaticText = oItem.Specific
            oStaticText.Item.LinkTo = mc_strCboDptoSrv
            oStaticText.Caption = My.Resources.Resource.TXTDptoServ '"Departamento de Servicio"


        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function

    Public Function RecorrerCotizacionesSinProcesar(Optional ByRef pVal As SAPbouiCOM.ItemEvent = Nothing) As Boolean

        Dim strConsulta As String = ""
        Dim m_dataTable As DataTable

        Try
            Dim m_blnConf_TallerEnSAP As Boolean = Utilitarios.ValidarOTInternaConfiguracion(m_oCompany)

            If Not m_blnConf_TallerEnSAP Then
                If m_cnnSCGTaller IsNot Nothing Then
                    If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                        m_cnnSCGTaller.Close()
                    End If
                    m_cnnSCGTaller = Nothing
                End If
            End If

            If Not m_blnActualizar Then

                If m_strDocNumNuevo <> "" Then

                    strConsulta = m_strRecepcionesIngresa & m_strDocNumNuevo

                Else

                    strConsulta = m_strRecepcionesIngresa & m_intDocNum

                End If

            Else
                If m_intDocNum > 0 Then

                    strConsulta = m_strRecepcionesIngresa & m_intDocNum

                End If

            End If
            If Not m_blnConf_TallerEnSAP Then
                Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application,
                                                           strIdSucursal,
                                                           strCadenaConexionBDTaller)
            End If
            'Erick Sanabria Bravo. Correción de errores al actualizar una oferta de venta 
            'abierta desde la Oportunidad de Venta si el empleado no tiene base de datos
            'de taller. 15.10.2013

            If (strIdSucursal <> "") Then
                m_dataTable = m_oForm.DataSources.DataTables.Item("dtConsulta")
                m_dataTable.ExecuteQuery(strConsulta)


                If m_oForm.Mode = BoFormMode.fm_ADD_MODE Then
                    Dim oCombo As ComboBox
                    oCombo = DirectCast(m_oForm.Items.Item(mc_strcbSucursal).Specific, ComboBox)

                    If String.IsNullOrEmpty(m_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).TrimEnd()) Then
                        oCombo.Select(Utilitarios.ObtieneIdSucursal(DMS_Connector.Company.ApplicationSBO).ToString, BoSearchKey.psk_ByDescription)
                    End If
                End If
                If m_dataTable.Rows.Count > 0 Then
                    m_intDocEntry = CInt(m_dataTable.GetValue("DocEntry", 0))
                    If m_intDocEntry > 0 Then
                        If Not pVal Is Nothing Then
                            Call ManejarCotizacion(m_blnConf_TallerEnSAP, 0, False, False, 0, pVal)
                        Else
                            Call ManejarCotizacion(m_blnConf_TallerEnSAP)
                        End If
                    End If
                End If

                Return True

            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            If ex.Message <> "No imprimir" Then

                SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

            End If
            Return False

        End Try

    End Function

    Private Sub CargarCotizacion(Optional ByVal p_blnSolicitud As Boolean = False, Optional ByVal p_blnCrearOT As Boolean = False)

        Try

            m_oCotizacion = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                                                SAPbobsCOM.Documents)
            If m_oCotizacion.GetByKey(m_intDocEntry) Then

                If Not p_blnSolicitud Then

                    If m_oCotizacion.UserFields.Fields.Item(mc_strIdSucursal).Value = "" Then

                        m_oCotizacion.UserFields.Fields.Item(mc_strIdSucursal).Value = m_strIDSucursal
                        If m_oCotizacion.UserFields.Fields.Item(mc_strGenerarOT).Value <> m_intGeneraOTPantalla Then
                            m_oCotizacion.UserFields.Fields.Item(mc_strGenerarOT).Value = m_intGeneraOTPantalla
                        End If

                        If m_oCotizacion.UserFields.Fields.Item(mc_strImprimirOT).Value <> m_intImprimeORPantalla Then
                            m_oCotizacion.UserFields.Fields.Item(mc_strImprimirOT).Value = m_intImprimeORPantalla
                        End If
                        m_oCotizacion.Update()

                    End If
                ElseIf p_blnSolicitud And p_blnCrearOT Then

                    If m_oCotizacion.UserFields.Fields.Item(mc_strIdSucursal).Value = "" Then
                        m_oCotizacion.UserFields.Fields.Item(mc_strIdSucursal).Value = m_strIDSucursal
                    End If

                    m_oCotizacion.Update()
                Else
                    m_oCotizacion.Update()

                End If
            End If


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex

        End Try

    End Sub

    Public Sub CargarCotizacionAnterior(ByVal p_strDocNum As String, Optional ByVal p_CreaOT As Boolean = False, Optional ByVal p_docEntry As Integer = 0)

        Try
            Dim strError As String = ""
            Dim strDocEntry As String
            Dim intDocEntry As Integer

            If p_docEntry = 0 Then

                If Not String.IsNullOrEmpty(p_strDocNum) Then

                    strDocEntry = Utilitarios.EjecutarConsulta("Select DocEntry from OQUT with(nolock) where DocNum = '" & p_strDocNum & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                    If IsNumeric(strDocEntry) Then
                        intDocEntry = CInt(strDocEntry)
                    Else
                        intDocEntry = 0
                    End If
                    m_oCotizacionAnterior = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                          SAPbobsCOM.Documents)

                    oCotizacionlocal = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                          SAPbobsCOM.Documents)
                    If Not m_oCotizacionAnterior.GetByKey(intDocEntry) Then

                        strError = m_oCompany.GetLastErrorDescription

                        If Not String.IsNullOrEmpty(strError) Then
                            Throw New Exception(strError)
                        End If

                    End If
                    If Not oCotizacionlocal.GetByKey(intDocEntry) Then

                        strError = m_oCompany.GetLastErrorDescription

                        If Not String.IsNullOrEmpty(strError) Then
                            Throw New Exception(strError)
                        End If

                    End If
                End If
            Else
                strDocEntry = p_docEntry

                If IsNumeric(strDocEntry) Then
                    intDocEntry = CInt(strDocEntry)
                Else
                    intDocEntry = 0
                End If

                m_oCotizacionAnterior = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                      SAPbobsCOM.Documents)

                oCotizacionlocal = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                        SAPbobsCOM.Documents)

                If Not m_oCotizacionAnterior.GetByKey(intDocEntry) Then

                    strError = m_oCompany.GetLastErrorDescription

                    If Not String.IsNullOrEmpty(strError) Then
                        Throw New Exception(strError)
                    End If

                End If


                If Not oCotizacionlocal.GetByKey(intDocEntry) Then

                    strError = m_oCompany.GetLastErrorDescription

                    If Not String.IsNullOrEmpty(strError) Then
                        Throw New Exception(strError)
                    End If

                End If

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

        End Try

    End Sub

    Public Sub ManejarCotizacion(ByVal m_blnConf_TallerEnSAP As Boolean, Optional ByVal p_docEntry As Integer = 0, Optional ByVal p_blnSolicitudOTEspecial As Boolean = False, Optional ByVal p_blnCrearOT As Boolean = False, Optional ByVal p_dECotRef As Integer = 0, Optional ByRef pVal As SAPbouiCOM.ItemEvent = Nothing, _
                                 Optional ByVal p_formSOTE As SAPbouiCOM.Form = Nothing, Optional ByVal p_blnSolOTEspecial_Contiene_SE As Boolean = False)
        '''''00002
        Try
            Dim intError As Integer = 0
            Dim strError As String = String.Empty
            Dim strDocEntrysTransfREP As String = String.Empty
            Dim strDocEntrysTransfSUM As String = String.Empty
            Dim strDocEntrysTransfELIMRepuestos As String = String.Empty
            Dim strDocEntrysTransfELIMSuministros As String = String.Empty
            Dim strUsaLead As String = String.Empty
            Dim strUsaAudatex As String = String.Empty
            Dim Bandera As Boolean = False
            Dim BanderaKilometraje As Boolean = False
            Dim BanderaHoraServicio As Boolean = False
            Dim BanderaVisitaCreada As Boolean = True
            Dim m_strOTHijaCreada As String = String.Empty
            Dim strNoIniciada As String = String.Empty
            Dim strConsultaEspecialesAprob As String
            Dim strIDEstadoOT As String = String.Empty
            Dim strMensaje As String = String.Empty

            Dim strCollecDocEntrys As String = String.Empty
            Dim strDocEntry As String = String.Empty

            'Permite identificar si es Orden de Trabajo hija o no
            Dim CreaHija As Boolean = False
            Dim strOTPadre As String = String.Empty
            Dim blnCreaUdoOTSAP As Boolean = False


            objValoresConfiguracionSucursalQT = New ValoresConfiguracionSucursalCotizacion

            If p_blnSolicitudOTEspecial Then
                m_intDocEntry = p_docEntry
                m_oForm = p_formSOTE
            End If

            m_EspecifVehi = DMS_Connector.Configuracion.ParamGenAddon.U_EspVehic.Trim()
            m_UsaAsocxEspc = DMS_Connector.Configuracion.ParamGenAddon.U_UsaAXEV.Trim()
            m_UsaFilSerEspeci = DMS_Connector.Configuracion.ParamGenAddon.U_UsaFilRep.Trim()
            strUsaLead = DMS_Connector.Configuracion.ParamGenAddon.U_UsaLed.Trim()

            strNoIniciada = Utilitarios.EjecutarConsulta(" select Name from [@SCGD_ESTADOS_OT] with(nolock) where code = '1' ", m_oCompany.CompanyDB, m_oCompany.Server)


            '*************************************Inicio: Usando Configuracion Taller Interno***************************************************
            'configuracion Interna de Taller DMS

            If m_blnConf_TallerEnSAP Then
                Call CargarValoresConfiguracionTaller(True, strIdSucursal, objValoresConfiguracionSucursalQT)
            Else
                Call CargarValoresConfiguracionTaller(False, strIdSucursal)
            End If
            'configuracion Interna de Taller DMS

            '*************************************Fin: usando Configuracion Taller Interno*******************************************************

            m_intEstCotizacion = CotizacionEstado.sinCambio
            LimpiarVariables()


            Call CargarCotizacion(p_blnSolicitudOTEspecial, p_blnCrearOT)
            m_blnIniciarTransaccion = True

            strIDEstadoOT = m_oCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacionID).Value.ToString.Trim

            If strIDEstadoOT <> "4" AndAlso strIDEstadoOT <> "5" _
                    AndAlso m_oCotizacion.Cancelled = SAPbobsCOM.BoYesNoEnum.tNO _
                    AndAlso m_oCotizacion.DocumentStatus = SAPbobsCOM.BoStatus.bost_Open Then

                If PasarDatos(m_oCotizacion) Then

                    If Not m_blnConf_TallerEnSAP Then

                        m_dstActividadesxOrden = Nothing
                        m_dstRepuestosxOrden = Nothing
                        m_dstSuministrosxOrden = Nothing

                        dtbRepuestosxOrden = Nothing
                        dtbSuministrosxOrden = Nothing
                        dtbActividadesXOrden = Nothing

                        dtbRepuestosxOrden = New RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable
                        dtbSuministrosxOrden = New SuministrosDataset.SCGTA_VW_SuministrosDataTable
                        dtbActividadesXOrden = New ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable

                        m_dstRepuestosxOrden = New RepuestosxOrdenDataset
                        m_adpRepuestosxOrden = New RepuestosxOrdenDataAdapter(strCadenaConexionBDTaller)

                        m_dstSuministrosxOrden = New SuministrosDataset
                        m_adpSuministrosxOrden = New SuministrosDataAdapter(strCadenaConexionBDTaller)

                        m_dstActividadesxOrden = New ActividadesXFaseDataset
                        m_adpActividadesxOrden = New ActividadesXFaseDataAdapter(strCadenaConexionBDTaller)

                    End If
                    '****************************************************
                    Dim strCardcode As String = String.Empty
                    strCardcode = m_oCotizacion.CardCode.Trim

                    Dim strConsultaXCCAsignado As String = " select EmpId from SCGTA_TB_ControlColaborador with(nolock) where IDActividad = '{0}' "
                    Dim strEmpID As String = String.Empty
                    Dim strNombreTaller As String = String.Empty
                    Dim strIdActividad As String = String.Empty

                    'Dim strConsultaNumVisita As String = " select top (1) U_SCGD_No_Visita from OQUT as coti with(nolock) where coti.U_SCGD_idSucursal = '{0}' order by U_SCGD_No_Visita desc "
                    'Dim strConsultaNumVisitaInicio As String = " select U_InicioV from [@SCGD_CONF_SUCURSAL] with(nolock)  where U_Sucurs = '{0}' "
                    'Dim strConsultaNumVisitaForm As String = String.Empty
                    'Dim strConsultaNumVisitaInicioForm As String = String.Empty
                    'Dim strNumVisita As String = String.Empty
                    Dim intNumVisita As Integer = 0
                    Dim dtConsulta As DataTable

                    Dim strEsCliente As String = Utilitarios.EjecutarConsulta(String.Format("SELECT CARDTYPE FROM OCRD with (nolock) WHERE CardCode = '{0}'", strCardcode), m_oCompany.CompanyDB, m_oCompany.Server)

                    Select Case strEsCliente
                        Case "C"
                            Bandera = True
                        Case Else
                            Bandera = False
                    End Select

                    Dim strIdSucursal As String = m_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()

                    If Bandera = False AndAlso strUsaLead = "N" Then
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorTipoCliente, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                    Else

                        If m_intGenerarOT = GeneraOrdenTrabajo.scgSiGenera Then

                            If m_strNumeroVisita = "" Then
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.CreandoVisita, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                                If Not m_blnConf_TallerEnSAP Then
                                    Call CrearVisita()
                                    m_trnTransaccion.Commit()
                                Else
                                    If Not ConsultaDocEntryOT(m_oCotizacion, m_oCompany.CompanyDB, m_oCompany.Server) Then
                                        Exit Sub
                                    Else
                                        dtConsulta = m_oForm.DataSources.DataTables.Item("dtConsulta")
                                        intNumVisita = Utilitarios.ObtieneNumeracionPorSucursalObjeto(dtConsulta, strIdSucursal, "SCGD_OT", m_oCompany)
                                        m_strNumeroVisita = intNumVisita.ToString()
                                        m_oCotizacion.UserFields.Fields.Item(mc_strNum_Visita).Value = m_strNumeroVisita
                                    End If
                                End If

                                m_blnIniciarTransaccion = False
                            Else
                                BanderaVisitaCreada = False
                            End If

                            If m_strOTPadre <> "" AndAlso m_strNoOrden = "" Then

                                CreaHija = True
                                strOTPadre = m_strOTPadre

                                If p_dECotRef > 0 Then
                                    Call ManejarCreacionOTEspecial(m_blnConf_TallerEnSAP, m_strOTHijaCreada, p_dECotRef)
                                Else
                                    Call ManejarCreacionOTEspecial(m_blnConf_TallerEnSAP, m_strOTHijaCreada)
                                End If

                                SBO_Application.StatusBar.SetText(My.Resources.Resource.GuardandoResultados, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                                If Not m_blnConf_TallerEnSAP Then

                                    If dstAsignacionesColaboradores.SCGTA_TB_ControlColaborador.Rows.Count > 0 Then
                                        Call FinalizarAsignacion(dstAsignacionesColaboradores)
                                    End If

                                    If m_trnTransaccion.Connection Is Nothing Then
                                        m_blnIniciarTransaccion = True
                                    Else
                                        m_blnIniciarTransaccion = False
                                    End If
                                    If m_dstActividadesxOrden.SCGTA_TB_ActividadesxOrden.Rows.Count > 0 Then
                                        m_adpActividadesxOrden.Update(m_dstActividadesxOrden.SCGTA_TB_ActividadesxOrden, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion, False, True)
                                    End If

                                    If m_trnTransaccion.Connection Is Nothing Then
                                        m_blnIniciarTransaccion = True
                                    Else
                                        m_blnIniciarTransaccion = False
                                    End If
                                    If m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows.Count > 0 Then
                                        m_adpRepuestosxOrden.Update(m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion, False, True)
                                    End If

                                    If m_trnTransaccion.Connection Is Nothing Then
                                        m_blnIniciarTransaccion = True
                                    Else
                                        m_blnIniciarTransaccion = False
                                    End If

                                    If m_dstSuministrosxOrden.SCGTA_VW_Suministros.Rows.Count > 0 Then
                                        m_adpSuministrosxOrden.Update(m_dstSuministrosxOrden.SCGTA_VW_Suministros, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion)
                                    End If

                                    intError = m_oCotizacion.Update()
                                    If intError <> 0 Then
                                        m_oCompany.GetLastError(intError, strMensaje)
                                        Throw New ExceptionsSBO(intError, strMensaje)
                                    End If
                                Else
                                    intError = m_oCotizacion.Update()
                                    If intError <> 0 Then
                                        m_oCompany.GetLastError(intError, strMensaje)
                                        Throw New ExceptionsSBO(intError, strMensaje)
                                    Else

                                        ManejoInsertarOThijaControlColaborador(m_strOTHijaCreada, m_strOTPadre, m_oCotizacion.DocEntry)

                                    End If

                                End If


                            ElseIf m_strNoOrden = "" Then

                                SBO_Application.StatusBar.SetText(My.Resources.Resource.CreandoOrden, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                                If BanderaVisitaCreada Then
                                    m_strNoOrden = String.Format("{0}-01", m_strNumeroVisita)
                                Else
                                    Dim strNoOrdenSiguiente As String = Utilitarios.EjecutarConsulta(
                                                                    String.Format("SELECT count(DocEntry) + 1 FROM OQUT with (nolock) WHERE U_SCGD_No_Visita= '{0}' and (U_SCGD_Numero_OT is not null and U_SCGD_Numero_OT <> '') ", m_strNumeroVisita),
                                                                    m_oCompany.CompanyDB, m_oCompany.Server)

                                    If Integer.Parse(strNoOrdenSiguiente) < 10 Then strNoOrdenSiguiente = String.Format("0{0}", strNoOrdenSiguiente)

                                    m_strNoOrden = String.Format("{0}-{1}", m_strNumeroVisita, strNoOrdenSiguiente)
                                End If

                                m_oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value = m_strNoOrden
                                m_oCotizacion.UserFields.Fields.Item(mc_strEstadoCot).Value = My.Resources.Resource.EstadoOrdenNoIniciada
                                m_oCotizacion.UserFields.Fields.Item(mc_strEstadoCotID).Value = "1"

                                SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoLineas, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                                Call ProcesarLineasAlCrear(m_blnConf_TallerEnSAP)

                                Call AsignarFechayHoraOT(m_oCotizacion)

                                SBO_Application.StatusBar.SetText(My.Resources.Resource.GuardandoResultados, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                                m_blnIniciarTransaccion = True

                                If m_blnConf_TallerEnSAP Then
                                    blnCreaUdoOTSAP = CrearOrdenTrabajoSAP(m_strNoOrden, strIdSucursal, strNoIniciada, m_oCotizacion.DocumentsOwner)

                                    'If m_blnControlColaborador Then
                                    '    Call AgregaDatosControlColaborador(strDocEntryOT)
                                    '    objListaActividades.Clear()
                                    'End If
                                Else
                                    Call CrearOrdenTrabajo(m_strNoOrden)
                                End If


                                m_blnIniciarTransaccion = False

                                If Not m_blnConf_TallerEnSAP Then
                                    m_adpActividadesxOrden.Update(dtbActividadesXOrden, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion, False)

                                    m_blnIniciarTransaccion = False

                                    For Each m_drwActividades In dtbActividadesXOrden.Rows
                                        If m_drwActividades.IDEmpleado <> 0 Then
                                            Call AsignarColaborador(m_drwActividades.NoFase, m_drwActividades.ID, m_drwActividades.IDEmpleado, m_dstAsignacionesColaboradores)
                                        End If
                                    Next

                                    If m_dstAsignacionesColaboradores.SCGTA_TB_ControlColaborador.Rows.Count > 0 Then
                                        Call FinalizarAsignacion(m_dstAsignacionesColaboradores)
                                    End If

                                    ActualizarListasRep(m_lstCantidadesAnteriores, dtbRepuestosxOrden)

                                    If blnDraft Then
                                        m_adpRepuestosxOrden.UpdateDraft(dtbRepuestosxOrden, m_cnnSCGTaller, m_trnTransaccion)
                                    Else
                                        m_adpRepuestosxOrden.Update(dtbRepuestosxOrden, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion)
                                    End If

                                    ActualizarListasSum(m_lstCantidadesAnteriores, dtbSuministrosxOrden)

                                    m_adpSuministrosxOrden.Update(dtbSuministrosxOrden, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion)

                                    m_lstCantidadesAnteriores.Clear()

                                    m_oCompany.StartTransaction()



                                    If m_blnUsaConfiguracionInternaTaller Then
                                        If objTransferenciaStock Is Nothing Then
                                            objTransferenciaStock = New TransferenciaItems(SBO_Application, m_oCompany, strCadenaConexionBDTaller)
                                        End If
                                    End If

                                    'cargo el docEntry de la Cotizacion
                                    objTransferenciaStock.intCodigoCotizacion = m_intDocEntry

                                    strDocEntrysTransfELIMRepuestos = objTransferenciaStock.CrearTrasladoAddOnNuevo(m_lstRepuestos, m_lstSuministros, m_lstServiociosEX, m_lstItemsEliminarRepuestos, m_lstItemsEliminarSuministros, m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, m_strNoOrden, m_strNoBodegaRepu, _
                                                                                        m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, m_strIDSerieDocTrasnf, m_cnnSCGTaller, m_trnTransaccion, False, strDocEntrysTransfREP, strDocEntrysTransfSUM, strDocEntrysTransfELIMSuministros, m_strDescMarca, m_strDescEstilo, m_strDescModelo, m_strPlaca, m_strVIN, m_strEmpleadoRecibe, m_strCodigoCliente,
                                                                                        False, False, strIdSucursal)


                                    If Not m_blnConf_TallerEnSAP Then
                                        Call AsignarIDsLineas(True)
                                    End If
                                    'Transaccion 3
                                    intError = m_oCotizacion.Update()

                                    If intError <> 0 Then
                                        m_oCompany.GetLastError(intError, strMensaje)
                                        Throw New ExceptionsSBO(intError, strMensaje)
                                    End If

                                Else

                                    m_lstCantidadesAnteriores.Clear()

                                    m_oCompany.StartTransaction()

                                    If blnCreaUdoOTSAP Then
                                        UDOOrden.Insert()
                                        'Al momento de crear la OT, si hay líneas de la cita que estén asignadas a un colaborador, inmediatamente
                                        'se crea su respectiva línea en la tabla [@SCGD_CTRLCOL]
                                        If m_blnControlColaborador Then
                                            Call AgregaDatosControlColaborador(strDocEntryOT)
                                            objListaActividades.Clear()
                                        End If
                                    End If

                                    If m_blnUsaConfiguracionInternaTaller Then
                                        If objTransferenciaStock Is Nothing Then
                                            objTransferenciaStock = New TransferenciaItems(SBO_Application, m_oCompany, strCadenaConexionBDTaller)
                                        End If
                                    End If

                                    'cargo el docEntry de la Cotizacion
                                    objTransferenciaStock.intCodigoCotizacion = m_intDocEntry

                                    strDocEntrysTransfELIMRepuestos = objTransferenciaStock.CrearTrasladoAddOnNuevo(m_lstRepuestos, m_lstSuministros, m_lstServiociosEX, m_lstItemsEliminarRepuestos, m_lstItemsEliminarSuministros, m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, m_strNoOrden, m_strNoBodegaRepu, _
                                                                                        m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, m_strIDSerieDocTrasnf, m_cnnSCGTaller, m_trnTransaccion, False, strDocEntrysTransfREP, strDocEntrysTransfSUM, strDocEntrysTransfELIMSuministros, m_strDescMarca, m_strDescEstilo, m_strDescModelo, m_strPlaca, m_strVIN, m_strEmpleadoRecibe, m_strCodigoCliente,
                                                                                        False, False, strIdSucursal)

                                    If Not m_blnConf_TallerEnSAP Then
                                        Call AsignarIDsLineas(True)
                                    End If
                                    'Transaccion 3
                                    intError = m_oCotizacion.Update()

                                    If intError <> 0 Then
                                        m_oCompany.GetLastError(intError, strMensaje)
                                        Throw New ExceptionsSBO(intError, strMensaje)
                                    Else

                                        'If (strUsaAudatex.Equals("Y")) Then
                                        '    Dim m_strCreadoPor = SBO_Application.Company.UserName().ToString()
                                        '    'FALTA VALIDAR => Apellido, Fecha_accidente, Descr_accidente, Taller, ompania_asignada,  
                                        '    'If (m_strOwnerCode <> "" And m_strNumeroOT <> "" And m_strNombreCliente <> "" And m_strPlaca <> "" And
                                        '    '    m_strDescMarca <> "" And m_strAno <> "" And m_strNoPol <> "" And m_strVIN <> "" And
                                        '    '    m_strCreadoPor <> "" And m_strNumeroCaso <> "") Then
                                        '    If (1 = 1) Then

                                        '        '*INICIO de llamado al WS
                                        '        Dim objConAudatex As New ConexionAudatex()
                                        '        Dim respAudaTex As String = objConAudatex.AddExpedient(m_strOwnerCode, m_strNumeroOT, m_strNombreClienteOT, "Apellido", "17/12/2015", "Descripcion accidente", m_strPlaca, m_strDescMarca, m_strAno, m_strNoPol, m_strVIN,
                                        '                                   m_strPlaca, m_strPeri, m_strPoolAsig, "CORD", "CORD", m_strCompOri, m_strCreadoPor, m_strNumeroCaso, "Tarifa carrocería", "Tarifa pintura", "AsignacionDirecta")

                                        '        If (respAudaTex.Contains("odata.error")) Then
                                        '            respAudaTex = respAudaTex.Replace("odata.error", "RootObject")
                                        '            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
                                        '            Dim Testobject As Response.Responseerror = serializer.Deserialize(Of Response.Responseerror)(respAudaTex)
                                        '            ' setear el mensaje de error de Audatex
                                        '        End If

                                        '        If (respAudaTex.Contains("odata.metadata")) Then
                                        '            respAudaTex = respAudaTex.Replace("odata.metadata", "RootObject")
                                        '            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
                                        '            Dim Testobject As Response.Responseok = serializer.Deserialize(Of Response.Responseok)(respAudaTex)
                                        '            m_oCotizacion.UserFields.Fields.Item(mc_strWAN).Value = Testobject.value
                                        '            intError = m_oCotizacion.Update()

                                        '            If intError <> 0 Then
                                        '                m_oCompany.GetLastError(intError, strMensaje)
                                        '                Throw New ExceptionsSBO(intError, strMensaje)
                                        '            End If
                                        '        End If
                                        '        '*FIN de llamado al WS

                                        '    End If
                                        'End If

                                        ManejoInsertaLineasServicios(m_strNoOrden, m_oCotizacion.DocEntry)
                                        If m_blnControlColaborador Then
                                            Call AgregaDatosControlColaborador(strDocEntryOT)
                                            objListaActividades.Clear()
                                        End If
                                    End If
                                End If

                            Else
                                dtbActividadesXOrden.Clear()
                                Call ProcesarLineasAlActualizar(m_blnConf_TallerEnSAP)

                                blnAsignacionAutomaticaColaborador = False

                                SBO_Application.StatusBar.SetText(My.Resources.Resource.GuardandoResultados, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                If m_cnnSCGTaller IsNot Nothing Then
                                    If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                                        m_cnnSCGTaller.Close()
                                    End If
                                End If

                                m_blnIniciarTransaccion = True

                                If Not m_blnConf_TallerEnSAP Then

                                    'Transaccion 4
                                    m_adpActividadesxOrden.Update(dtbActividadesXOrden, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion)
                                    m_blnIniciarTransaccion = False

                                    If String.IsNullOrEmpty(strNombreTaller) Then Utilitarios.DevuelveNombreBDTaller(SBO_Application, strIdSucursal, strNombreTaller)

                                    For Each m_drwActividades In dtbActividadesXOrden.Rows
                                        If m_drwActividades.IDEmpleado <> 0 Then
                                            Call AsignarColaborador(m_drwActividades.NoFase, m_drwActividades.ID, m_drwActividades.IDEmpleado, dstAsignacionesColaboradores)
                                        ElseIf m_drwActividades.IDEmpleado <> 0 AndAlso m_drwActividades.RowState = DataRowState.Modified Then
                                            'modifica el colaborador ingresado en la actividad
                                            Call ModificaColaborador(m_drwActividades.LineNum, m_drwActividades.ID, m_drwActividades.IDEmpleado, dstAsignacionesColaboradores, strNombreTaller, m_drwActividades.Duracion)
                                        End If
                                    Next

                                    For Each m_drwActividades In m_dstActividadesxOrden.SCGTA_TB_ActividadesxOrden.Rows
                                        If m_drwActividades.RowState <> DataRowState.Deleted Then

                                            'asigna un nuevo colaborador o modifica el ya existente
                                            If m_drwActividades.IDEmpleado <> 0 AndAlso m_drwActividades.RowState <> DataRowState.Unchanged Then

                                                strIdActividad = m_drwActividades.ID
                                                strIdActividad = strIdActividad.Trim()

                                                strEmpID = Utilitarios.EjecutarConsulta(String.Format(strConsultaXCCAsignado, strIdActividad), strNombreTaller, SBO_Application.Company.ServerName)

                                                If String.IsNullOrEmpty(strEmpID) Then
                                                    Call AsignarColaborador(m_drwActividades.NoFase, m_drwActividades.ID, m_drwActividades.IDEmpleado, dstAsignacionesColaboradores)
                                                Else
                                                    Call ModificaColaborador(m_drwActividades.LineNum, m_drwActividades.ID, m_drwActividades.IDEmpleado, dstAsignacionesColaboradores, strNombreTaller, m_drwActividades.Duracion)
                                                End If
                                            Else
                                                Utilitarios.EjecutarConsulta(String.Format("Update SCGTA_TB_ActividadesxOrden set DuracionAprobada = {0} where ID = '{1}'", m_drwActividades.Duracion, m_drwActividades.ID), strNombreTaller, SBO_Application.Company.ServerName)
                                            End If

                                        End If
                                    Next

                                    If dstAsignacionesColaboradores.SCGTA_TB_ControlColaborador.Rows.Count > 0 Then
                                        Call FinalizarAsignacion(dstAsignacionesColaboradores)
                                    End If
                                    m_adpActividadesxOrden.Update(m_dstActividadesxOrden.SCGTA_TB_ActividadesxOrden, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion, False, True)


                                    ActualizarListasRep(m_lstCantidadesAnteriores, dtbRepuestosxOrden)

                                    '''''''''''''''''''''''''para documentos Draft'''''''''''''''''''''

                                    If blnDraft Then
                                        blnModificaItemsAdicionales = True
                                        m_adpRepuestosxOrden.UpdateDraft(dtbRepuestosxOrden, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion)
                                        m_adpRepuestosxOrden.UpdateDraft(m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion, False, True)
                                    Else
                                        blnModificaItemsAdicionales = False
                                        m_adpRepuestosxOrden.Update(dtbRepuestosxOrden, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion)
                                        m_adpRepuestosxOrden.Update(m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion, False, True)
                                    End If


                                    ActualizarListasSum(m_lstCantidadesAnteriores, dtbSuministrosxOrden)

                                    m_adpSuministrosxOrden.Update(dtbSuministrosxOrden, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion)
                                    m_adpSuministrosxOrden.Update(m_dstSuministrosxOrden.SCGTA_VW_Suministros, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion)

                                    m_lstCantidadesAnteriores.Clear()

                                    objTransferenciaStock.intCodigoCotizacion = m_intDocEntry


                                    If Not m_oCompany.InTransaction Then
                                        m_oCompany.StartTransaction()
                                    End If

                                    Call AsignarIDsLineas(True)

                                    strDocEntrysTransfELIMRepuestos = objTransferenciaStock.CrearTrasladoAddOnNuevo(m_lstRepuestos, m_lstSuministros, m_lstServiociosEX, m_lstItemsEliminarRepuestos, m_lstItemsEliminarSuministros, m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, m_strNumeroOT, m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, m_strIDSerieDocTrasnf, m_cnnSCGTaller, m_trnTransaccion, True, strDocEntrysTransfREP, strDocEntrysTransfSUM, strDocEntrysTransfELIMSuministros, m_strDescMarca, m_strDescEstilo, m_strDescModelo, m_strPlaca, m_strVIN, m_strEmpleadoRecibe, m_strCodigoCliente, False, False, strIdSucursal)

                                    intError = m_oCotizacion.Update()

                                    If intError <> 0 Then
                                        m_oCompany.GetLastError(intError, strMensaje)
                                        Throw New ExceptionsSBO(intError, strMensaje)
                                    End If


                                Else

                                    m_lstCantidadesAnteriores.Clear()

                                    'Elimina de la lista los articulso con cantidades en 0 para que no genere requisicion informativa y en su vez actualize un estado
                                    m_lstItemsEliminarRepuestos = LimpiaListaconCantZero(m_lstItemsEliminarRepuestos, m_oCotizacion)
                                    m_lstItemsEliminarSuministros = LimpiaListaconCantZero(m_lstItemsEliminarSuministros, m_oCotizacion)

                                    If m_blnConf_TallerEnSAP Then
                                        If objTransferenciaStock Is Nothing Then
                                            objTransferenciaStock = New TransferenciaItems(SBO_Application, m_oCompany, strCadenaConexionBDTaller)
                                        End If
                                    End If

                                    m_oCompany.StartTransaction()

                                    objTransferenciaStock.intCodigoCotizacion = m_intDocEntry

                                    strDocEntrysTransfELIMRepuestos = objTransferenciaStock.CrearTrasladoAddOnNuevo(m_lstRepuestos, m_lstSuministros, m_lstServiociosEX, m_lstItemsEliminarRepuestos, m_lstItemsEliminarSuministros, m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, m_strNumeroOT, m_strNoBodegaRepu, _
                                                                                                   m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, m_strIDSerieDocTrasnf, m_cnnSCGTaller, m_trnTransaccion, True, strDocEntrysTransfREP, strDocEntrysTransfSUM, strDocEntrysTransfELIMSuministros, m_strDescMarca, m_strDescEstilo, m_strDescModelo, m_strPlaca, m_strVIN, m_strEmpleadoRecibe, m_strCodigoCliente,
                                                                                                   False, False, strIdSucursal)
                                    intError = m_oCotizacion.Update()

                                    If intError <> 0 Then
                                        m_oCompany.GetLastError(intError, strMensaje)
                                        Throw New ExceptionsSBO(intError, strMensaje)
                                    Else
                                        If Not String.IsNullOrEmpty(m_strNoOrden) Then
                                            ActualizaUDOOT(m_strNoOrden)
                                        End If
                                        ManejoInsertaLineasServicios(m_strNoOrden, m_oCotizacion.DocEntry)
                                        If m_blnControlColaborador Then
                                            Call AgregaDatosControlColaborador(m_strNoOrden)
                                            objListaActividades.Clear()
                                        End If
                                    End If
                                End If
                            End If

                        End If

                    End If

                    If m_trnTransaccion IsNot Nothing Then

                        m_trnTransaccion.Commit()
                        m_trnTransaccion = Nothing
                    End If

                    If m_oCompany.InTransaction Then
                        m_oCompany.EndTransaction(BoWfTransOpt.wf_Commit)
                    End If

                    Utilitarios.DestruirObjeto(m_objCotizacionPadre)

                    '***********************************************************************************
                    'Se actualiza la tabla TB_Orden para los servicios que se encuentran en la OT padre finalizados y se pasaron a la OT hija
                    'Esto con el fin de que cree el asiento contable por Mano de Obra
                    If Not m_blnConf_TallerEnSAP Then
                        If (CreaHija) Then
                            Dim strOrdenTrabajoHija As String
                            strOrdenTrabajoHija = m_drwOrdenTrabajo.Item("NoOrden").ToString.Trim()

                            ReasignarTiemposOTHijayPadre(strOTPadre, strOrdenTrabajoHija)
                        End If

                        Call ActualizaAsesoryTipoOT(m_oCotizacion)

                    End If

                    '********************************************************************************

                    'Envia mensaje al encargado de Taller para indicar que la cotizacion fue creada o actualizada
                    If m_intEstCotizacion = CotizacionEstado.modificada Or m_intEstCotizacion = CotizacionEstado.creada Or strDocEntrysTransfREP <> "" Then
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.EnviandoAlertas, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        EnviarMensaje(strDocEntrysTransfELIMRepuestos, strDocEntrysTransfREP, strDocEntrysTransfSUM, strDocEntrysTransfELIMSuministros, m_blnConf_TallerEnSAP, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoProduccion))
                        'EnviarMensaje(strDocEntrysTransfELIMRepuestos, strDocEntrysTransfREP, strDocEntrysTransfSUM, strDocEntrysTransfELIMSuministros, m_blnConf_TallerEnSAP)
                    End If

                    If m_oCotizacion.UserFields.Fields.Item(mc_strImprimirOT).Value <> ImprimirOT.scgSi Then
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizadoConExito, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                    End If

                    If Not m_blnConf_TallerEnSAP Then

                        '********validacion para los LineNumErroneos************************************************

                        If objItemsLineasLineNumErroneos.Count <> 0 Then
                            Call EliminarRegistroLineNumErroneo(objItemsLineasLineNumErroneos)
                            objItemsLineasLineNumErroneos.Clear()
                        End If
                        '*******************************************************************************************

                        ''Cambio para actualizar el costo de los repuestos en la tabla RepuestosxOrden 
                        If Not blnDraft Then
                            If m_lstRepuestos.Count > 0 And strDocEntrysTransfREP <> String.Empty And strDocEntrysTransfREP <> "" And m_strNoOrden <> String.Empty And m_strNoOrden <> "" Then
                                objTransferenciaStock.ActualizarCostoRepuestosXOrden(m_lstRepuestos, strDocEntrysTransfREP, m_strNoOrden, m_cnnSCGTaller)
                            End If

                        End If
                    End If

                    If Not String.IsNullOrEmpty(m_strNoOrden) And Not p_blnSolicitudOTEspecial Then
                        BotonAsignacionMultiple(pVal, m_blnConf_TallerEnSAP)
                    End If

                End If

            Else
                SBO_Application.StatusBar.SetText(My.Resources.Resource.CambiosNoAplicadosOT, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
            End If

            If m_oCotizacion.UserFields.Fields.Item(mc_strImprimirOT).Value = ImprimirOT.scgSi Then
                If m_oCotizacion.UserFields.Fields.Item(mc_strGenerarOT).Value = GeneraOrdenTrabajo.scgSiGenera Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ImprimirOT, BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning)
                    ImprimirCotizacion(m_oCotizacion)
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizadoConExito, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                Else
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ImprimirOTFallo, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                End If
            End If

            If Not m_blnConf_TallerEnSAP Then

                'asigna de manera automatica los colaboradores en la orden de trabajo
                If blnAsignacionAutomaticaColaborador Then
                    If Not m_intCodigoTecnico Is Nothing Then
                        AsignarCodTecnicoAColaborador(dtbActividadesXOrden, m_cnnSCGTaller, m_trnTransaccion)
                    End If
                End If

                'atualiza el campo fechasync de los items de la orden en dms
                Dim ordenAdapter As OrdenTrabajoDataAdapter = New OrdenTrabajoDataAdapter(strCadenaConexionBDTaller)

                If Not m_strNumeroOT Is Nothing AndAlso m_strNumeroOT <> "" Then

                    If m_strEstadoCotizacionID <> "4" AndAlso m_strEstadoCotizacionID <> "5" AndAlso m_strEstadoCotizacionID <> "6" AndAlso m_strEstadoCotizacionID <> "7" Then
                        ordenAdapter.UpdFechaSyncItemsOrden(m_cnnSCGTaller, m_trnTransaccion, m_strNumeroOT)
                    End If

                End If

                If blnModificaItemsAdicionales Then
                    'modifica las lineas en la orden de trabajo que sean Adicionales 
                    'Para prueba se van a comentar las siguientes dos lineas
                    'Call ActualizarLineasOrdenTrabajoDesdeCotizacion(m_oCotizacion, m_dstRepuestosxOrden)

                    'Call ActualizarLineasAdicionales(m_oCotizacion)

                    'Cambios actualización Estado Repuestos adicionales
                    Call ActualizarEstadoRepuestosDesdeCotizacion(m_oCotizacion)
                End If
            End If

            If blnActualizaValoresHS Or blnActualizaValoresKm Then

                If Not String.IsNullOrEmpty(m_strNumeroVehiculo) Then
                    ActualizarDatosVehiculo(m_strNumeroVehiculo)
                    blnActualizaValoresHS = False
                    blnActualizaValoresKm = False
                    blnValidarCamposHS_KM = False
                    strIDVehiculoHS_KM = String.Empty
                End If

            End If

        Catch ex As ExceptionsSBO
            If m_trnTransaccion IsNot Nothing Then
                m_trnTransaccion.Rollback()
            End If
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        Catch ex As Exception
            If m_trnTransaccion IsNot Nothing Then
                m_trnTransaccion.Rollback()
            End If
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex
        Finally
            If m_cnnSCGTaller IsNot Nothing Then
                m_cnnSCGTaller.Close()
            End If
            m_cnnSCGTaller = Nothing
            m_trnTransaccion = Nothing

            'listaVisOrderCotizacionAnterior.Clear()

        End Try

    End Sub
    ''' <summary>
    ''' Valida que la cotización en proceso no esté ligada a alguna OT antes de crear el número de Visita
    ''' </summary>
    ''' <param name="p_DocEntry"></param>
    ''' <param name="p_companyDb"></param>
    ''' <param name="p_server"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConsultaDocEntryOT(p_Cotizacion As Documents, p_companyDb As String, p_server As String) As Boolean
        Dim Code As String
        Try
            Code = Utilitarios.EjecutarConsulta(String.Format("Select Code From [@SCGD_OT] Where U_DocEntry ={0}", p_Cotizacion.DocEntry), p_companyDb, p_server)
            If Not (String.IsNullOrEmpty(Code)) Then '''Si el Docentry se encontro dentro de la tabla OT se procede a actualizar la cotizacion con el respectivo número de OT y Visita
                '''Definicion de General Services
                Dim oCompanyService As SAPbobsCOM.CompanyService
                Dim oGeneralService As SAPbobsCOM.GeneralService
                Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
                Dim oOT As SAPbobsCOM.GeneralData
                '''Inicia Transaccion para actualizar la cotizacion
                m_oCompany.StartTransaction()
                oCompanyService = m_oCompany.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_OT")
                oGeneralParams = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("Code", Code)
                oOT = oGeneralService.GetByParams(oGeneralParams)
                p_Cotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = oOT.GetProperty("Code").ToString()
                p_Cotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value = oOT.GetProperty("U_NoVis").ToString()
                If (p_Cotizacion.Update() = 0) Then
                    m_oCompany.EndTransaction(BoWfTransOpt.wf_Commit)
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.DocEntryEnTablaOT, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    Return False
                    '''Fin de la transaccion
                Else
                    Throw New Exception()
                End If

            End If
            Return True
        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorAlActualizarCotVisita, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try

    End Function

    Private Sub ActualizaUDOOT(p_strNoOT)

        Dim m_oGeneralData As SAPbobsCOM.GeneralData
        Dim m_oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim m_oGeneralService As SAPbobsCOM.GeneralService
        Dim m_oCompanyService As SAPbobsCOM.CompanyService
        Dim blnActualizaUDO As Boolean

        Try
            m_oCompanyService = m_oCompany.GetCompanyService
            m_oGeneralService = m_oCompanyService.GetGeneralService("SCGD_OT")
            m_oGeneralParams = m_oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            m_oGeneralParams.SetProperty("Code", p_strNoOT)
            m_oGeneralData = m_oGeneralService.GetByParams(m_oGeneralParams)

            If m_oGeneralData.GetProperty("U_NoCon") <> m_strCono Then
                m_oGeneralData.SetProperty("U_NoCon", m_strCono)
                blnActualizaUDO = True
            End If

            If CDbl(m_oGeneralData.GetProperty("U_km")) <> m_dbkilometraje Then
                m_oGeneralData.SetProperty("U_km", CInt(m_dbkilometraje))
                blnActualizaUDO = True
            End If

            If CStr(m_oGeneralData.GetProperty("U_TipOT")) <> CStr(m_intTipoOT) Then
                m_oGeneralData.SetProperty("U_TipOT", m_intTipoOT.ToString())
                blnActualizaUDO = True
            End If

            If m_oGeneralData.GetProperty("U_CodCOT") <> m_strClienteOT Then
                m_oGeneralData.SetProperty("U_CodCOT", m_strClienteOT)
                blnActualizaUDO = True
            End If

            If m_oGeneralData.GetProperty("U_NCliOT") <> m_strNombreClienteOT Then
                m_oGeneralData.SetProperty("U_NCliOT", m_strNombreClienteOT)
                blnActualizaUDO = True
            End If

            If m_oGeneralData.GetProperty("U_FRec") <> m_dtFechaRecepcion Then
                m_oGeneralData.SetProperty("U_FRec", m_dtFechaRecepcion)
                blnActualizaUDO = True
            End If

            If m_oGeneralData.GetProperty("U_HRec") <> m_dtHoraRecepcion Then
                m_oGeneralData.SetProperty("U_HRec", m_dtHoraRecepcion)
                blnActualizaUDO = True
            End If

            If m_oGeneralData.GetProperty("U_FCom") <> m_dtFechaCompromiso Then
                m_oGeneralData.SetProperty("U_FCom", m_dtFechaCompromiso)
                blnActualizaUDO = True
            End If

            If m_oGeneralData.GetProperty("U_HCom") <> m_dtHoraCompromiso Then
                m_oGeneralData.SetProperty("U_HCom", m_dtHoraCompromiso)
                blnActualizaUDO = True
            End If

            If m_oGeneralData.GetProperty("U_NGas") <> m_strOtNivelGas Then
                m_oGeneralData.SetProperty("U_NGas", m_strOtNivelGas)
                blnActualizaUDO = True
            End If

            If m_oGeneralData.GetProperty("U_Ase") <> CStr(m_oCotizacion.DocumentsOwner) Then
                m_oGeneralData.SetProperty("U_Ase", CStr(m_oCotizacion.DocumentsOwner))
                blnActualizaUDO = True
            End If

            If m_oGeneralData.GetProperty("U_Obse") <> m_strObservaciones Then
                m_oGeneralData.SetProperty("U_Obse", m_strObservaciones)
                blnActualizaUDO = True
            End If

            If blnActualizaUDO Then
                m_oGeneralService.Update(m_oGeneralData)
            End If

        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            Throw
        End Try

    End Sub

    Function LimpiaListaconCantZero(ByVal m_lstItemsEliminarRepuestos As List(Of TransferenciaItems.LineasTransferenciaStock), ByRef p_Cotizacion As SAPbobsCOM.Documents) As List(Of TransferenciaItems.LineasTransferenciaStock)

        Dim lstItemsActualizada As New List(Of TransferenciaItems.LineasTransferenciaStock)

        Try
            For Each item As TransferenciaItems.LineasTransferenciaStock In m_lstItemsEliminarRepuestos
                If item.decCantidad > 0 Then
                    lstItemsActualizada.Add(item)
                Else
                    For i As Integer = 0 To p_Cotizacion.Lines.Count - 1
                        p_Cotizacion.Lines.SetCurrentLine(i)
                        If item.intLineNum = p_Cotizacion.Lines.LineNum Then
                            p_Cotizacion.Lines.UserFields.Fields.Item("U_SCGD_ItemRecha").Value = "Y"
                            Exit For
                        End If
                    Next
                End If
            Next

            Return lstItemsActualizada

        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            Throw
        End Try

    End Function

    ''' <summary>
    ''' Inserta lineas de servicios si existe en la OT
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ManejoInsertaLineasServicios(ByVal p_strOT As String, p_intDocEntry As Integer)

        Dim m_oGeneralData As SAPbobsCOM.GeneralData
        Dim m_oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim m_oGeneralService As SAPbobsCOM.GeneralService
        Dim m_oCompanyService As SAPbobsCOM.CompanyService
        Dim m_childs As SAPbobsCOM.GeneralDataCollection
        Dim m_childdata As SAPbobsCOM.GeneralData
        Dim m_oLineasCotizacion As SAPbobsCOM.Document_Lines
        Dim m_objCotizacion As SAPbobsCOM.Documents
        Dim i As Integer
        Dim j As Integer
        Dim m_strConsultaConfig As String = String.Empty
        Dim mbExisteVAlor As Boolean = False
        Dim blnHayCambios As Boolean = False
        Dim strHora, strMinutos As String

        Try
            m_strConsultaConfig = String.Format("select U_AsigUniMec from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}'", m_objCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim())
            If Utilitarios.EjecutarConsulta(m_strConsultaConfig, m_oCompany.CompanyDB, m_oCompany.Server).Trim = "Y" Then
                If Not String.IsNullOrEmpty(p_strOT) AndAlso p_strOT IsNot Nothing AndAlso p_intDocEntry <> 0 Then
                    m_objCotizacion = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                    m_objCotizacion.GetByKey(p_intDocEntry)
                    m_oLineasCotizacion = m_objCotizacion.Lines
                    m_oCompanyService = m_oCompany.GetCompanyService
                    m_oGeneralService = m_oCompanyService.GetGeneralService("SCGD_OT")
                    m_oGeneralParams = m_oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                    m_oGeneralParams.SetProperty("Code", p_strOT)
                    m_oGeneralData = m_oGeneralService.GetByParams(m_oGeneralParams)
                    m_childs = m_oGeneralData.Child("SCGD_CTRLCOL")

                    For i = 0 To m_oLineasCotizacion.Count - 1
                        m_oLineasCotizacion.SetCurrentLine(i)
                        mbExisteVAlor = False
                        If m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value IsNot Nothing Then
                            If m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString.Trim = "2" And Not String.IsNullOrEmpty(m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim) Then
                                If m_childs.Count > 0 Then
                                    For j = 0 To m_childs.Count - 1
                                        m_childdata = m_childs.Item(j)
                                        If m_childdata.GetProperty("U_IdAct") = m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_ID").Value.ToString AndAlso m_childdata.GetProperty("U_Colab") = m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_EmpAsig").Value Then
                                            mbExisteVAlor = True
                                            Exit For
                                        End If
                                    Next
                                    If Not mbExisteVAlor Then
                                        m_childdata = m_childs.Add()
                                        m_childdata.SetProperty("U_Estad", 1)
                                        m_childdata.SetProperty("U_Colab", m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_EmpAsig").Value)
                                        m_childdata.SetProperty("U_IdAct", m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_ID").Value.ToString)
                                        m_childdata.SetProperty("U_CosRe", 0)
                                        m_childdata.SetProperty("U_CosEst", 0)
                                        strHora = DateTime.Now.Hour.ToString()
                                        If strHora.Length = 1 Then strHora = String.Format("0{0}", strHora)
                                        strMinutos = DateTime.Now.Minute.ToString()
                                        If (strMinutos.Length = 1) Then strMinutos = String.Format("0{0}", strMinutos)
                                        m_childdata.SetProperty("U_FechPro", DateTime.Now)
                                        m_childdata.SetProperty("U_HoraIni", strHora)
                                        blnHayCambios = True
                                    End If
                                End If
                            End If
                        End If
                    Next
                    If blnHayCambios Then
                        m_oGeneralService.Update(m_oGeneralData)
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Maneja Insertar en Control colaborados y SCGD_OT  
    ''' </summary>
    ''' <param name="p_strOtHijaCreada"></param>
    ''' <remarks></remarks>
    Private Sub ManejoInsertarOThijaControlColaborador(ByVal p_strOtHijaCreada As String, ByVal p_strOtPadre As String, ByVal p_intDocEntry As Integer)

        Dim m_objCotizacionHija As SAPbobsCOM.Documents
        Dim m_objLineasCotizacionHija As SAPbobsCOM.Document_Lines

        Dim m_oCompanyService As SAPbobsCOM.CompanyService
        Dim m_oGeneralService As SAPbobsCOM.GeneralService
        Dim m_oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim m_oGeneralData As SAPbobsCOM.GeneralData
        Dim m_childs As SAPbobsCOM.GeneralDataCollection
        Dim m_childdata As SAPbobsCOM.GeneralData

        Dim m_oCompanyServicePadre As SAPbobsCOM.CompanyService
        Dim m_oGeneralServicePadre As SAPbobsCOM.GeneralService
        Dim m_oGeneralParamsPadre As SAPbobsCOM.GeneralDataParams
        Dim m_oGeneralDataPadre As SAPbobsCOM.GeneralData
        Dim m_childsPadre As SAPbobsCOM.GeneralDataCollection
        Dim m_childdataPadre As SAPbobsCOM.GeneralData

        Dim m_strConsultaDocEntry As String
        Dim m_strConsultaCodePadre As String
        Dim m_strConsultaCodeHija As String
        Dim m_strResultadoCodeHija As String
        Dim m_strResultadoDocEntry As String
        Dim m_strResultadoCodePadre As String
        Dim i As Integer
        Dim j As Integer
        Dim y As Integer

        Try
            m_strConsultaCodePadre = String.Format(" Select Code from [@SCGD_OT] with(nolock) where U_NoOT  = '{0}' ", p_strOtPadre)
            m_strResultadoCodePadre = Utilitarios.EjecutarConsulta(m_strConsultaCodePadre, m_oCompany.CompanyDB, m_oCompany.Server)

            m_strConsultaCodeHija = String.Format(" Select Code from [@SCGD_OT] with(nolock) where U_NoOT  = '{0}' ", p_strOtHijaCreada)
            m_strResultadoCodeHija = Utilitarios.EjecutarConsulta(m_strConsultaCodeHija, m_oCompany.CompanyDB, m_oCompany.Server)

            m_objCotizacionHija = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
            m_objCotizacionHija.GetByKey(p_intDocEntry)
            m_objLineasCotizacionHija = m_objCotizacionHija.Lines

            m_oCompanyServicePadre = m_oCompany.GetCompanyService()
            m_oGeneralServicePadre = m_oCompanyServicePadre.GetGeneralService("SCGD_OT")
            m_oGeneralParamsPadre = m_oGeneralServicePadre.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            m_oGeneralParamsPadre.SetProperty("Code", m_strResultadoCodePadre)
            m_oGeneralDataPadre = m_oGeneralServicePadre.GetByParams(m_oGeneralParamsPadre)
            m_childsPadre = m_oGeneralDataPadre.Child("SCGD_CTRLCOL")

            m_oCompanyService = m_oCompany.GetCompanyService()
            m_oGeneralService = m_oCompanyService.GetGeneralService("SCGD_OT")
            m_oGeneralParams = m_oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            m_oGeneralParams.SetProperty("Code", m_strResultadoCodeHija)
            m_oGeneralData = m_oGeneralService.GetByParams(m_oGeneralParams)
            m_childs = m_oGeneralData.Child("SCGD_CTRLCOL")


            For i = 0 To m_objLineasCotizacionHija.Count - 1
                m_objLineasCotizacionHija.SetCurrentLine(i)
                If m_objLineasCotizacionHija.UserFields.Fields.Item("U_SCGD_EmpAsig").Value IsNot Nothing Then
                    If Not String.IsNullOrEmpty(m_objLineasCotizacionHija.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim) AndAlso m_objLineasCotizacionHija.UserFields.Fields.Item("U_SCGD_TipArt").Value = "2" Then
                        For j = 0 To m_childsPadre.Count - 1
                            m_childdataPadre = m_childsPadre.Item(j)
                            If m_childdataPadre.GetProperty("U_IdAct") = m_objLineasCotizacionHija.UserFields.Fields.Item("U_SCGD_ID").Value.ToString Then
                                m_childdata = m_childs.Add()
                                m_childdata.SetProperty("U_Estad", m_childdataPadre.GetProperty("U_Estad"))
                                m_childdata.SetProperty("U_IdAct", m_childdataPadre.GetProperty("U_IdAct"))
                                m_childdata.SetProperty("U_NoFas", m_childdataPadre.GetProperty("U_NoFas"))
                                m_childdata.SetProperty("U_Colab", m_childdataPadre.GetProperty("U_Colab"))
                                m_childdata.SetProperty("U_TMin", m_childdataPadre.GetProperty("U_TMin"))
                                m_childdata.SetProperty("U_CosRe", m_childdataPadre.GetProperty("U_CosRe"))
                                m_childdata.SetProperty("U_CosEst", m_childdataPadre.GetProperty("U_CosEst"))
                                m_childdata.SetProperty("U_ReAsig", m_childdataPadre.GetProperty("U_ReAsig"))
                                m_childdata.SetProperty("U_DFIni", m_childdataPadre.GetProperty("U_DFIni"))
                                m_childdata.SetProperty("U_HFIni", m_childdataPadre.GetProperty("U_HFIni"))
                                m_childdata.SetProperty("U_DFFin", m_childdataPadre.GetProperty("U_DFFin"))
                                m_childdata.SetProperty("U_HFFin", m_childdataPadre.GetProperty("U_HFFin"))
                                m_childdata.SetProperty("U_HoraIni", m_childdataPadre.GetProperty("U_HoraIni"))
                                m_childdata.SetProperty("U_FechPro", m_childdataPadre.GetProperty("U_FechPro"))
                                m_childdata.SetProperty("U_CodFas", m_childdataPadre.GetProperty("U_CodFas"))

                            End If
                        Next
                        For y = m_childsPadre.Count - 1 To 0 Step -1
                            m_childdataPadre = m_childsPadre.Item(y)
                            If m_childdataPadre.GetProperty("U_IdAct") = m_objLineasCotizacionHija.UserFields.Fields.Item("U_SCGD_ID").Value.ToString Then
                                m_childsPadre.Remove(y)
                            End If
                        Next
                    End If
                End If
            Next
            m_oGeneralService.Update(m_oGeneralData)
            m_oGeneralServicePadre.Update(m_oGeneralDataPadre)
        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    '*********************************************************************************************************************************

    Private Sub EliminarRegistroLineNumErroneo(ByVal p_ListaLineNum As IList)

        Dim strConectionString As String = ""
        Dim strNombreTaller As String = String.Empty
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Dim strConsulta As String = ""
        Dim cmdAsiento As New SqlClient.SqlCommand

        Utilitarios.DevuelveNombreBDTaller(SBO_Application, strIdSucursal, strNombreTaller)

        Dim baseDatos As String
        baseDatos = SBO_Application.Company.DatabaseName
        Dim Server As String
        Server = SBO_Application.Company.ServerName

        For Each objlist As LineasLineNumErroneos In p_ListaLineNum

            Select Case objlist.TipoRow
                Case 1

                    strConsulta = "DELETE FROM SCGTA_TB_RepuestosxEstado WHERE IdRepuestosxOrden = '" & objlist.Id & "'"
                    Utilitarios.EjecutarConsulta(strConsulta, strNombreTaller, Server)

                    strConsulta = String.Empty

                    strConsulta = "DELETE FROM SCGTA_TB_RepuestosxOrden Where  NoOrden = '" & objlist.NoOrden & "' and NoRepuesto = '" & objlist.IdItem & "' and LineNum = " & objlist.intLineNum & ""
                    Utilitarios.EjecutarConsulta(strConsulta, strNombreTaller, Server)
                Case 2

                    strConsulta = "DELETE FROM SCGTA_TB_SuministroxOrden Where  NoOrden = '" & objlist.NoOrden & "' and NoSuministro = '" & objlist.IdItem & "' and LineNum = " & objlist.intLineNum & ""
                    Utilitarios.EjecutarConsulta(strConsulta, strNombreTaller, Server)
                Case 3
                    strConsulta = "DELETE FROM SCGTA_TB_ActividadesxOrden Where  NoOrden  = '" & objlist.NoOrden & "' and NoActividad = '" & objlist.IdItem & "' and LineNum = " & objlist.intLineNum & ""
                    Utilitarios.EjecutarConsulta(strConsulta, strNombreTaller, Server)
            End Select

        Next

    End Sub

    Private Sub AsignarIDsLineas(ByVal p_blnEsActualizacion As Boolean)

        If Not p_blnEsActualizacion Then
            m_oCotizacion.GetByKey(m_intDocEntry)
        End If


        Dim visOrder As Integer
        Dim cadenaConexion As String = String.Empty
        Dim nombreTabla As String = "QUT1"

        Dim oItem As SAPbobsCOM.Items
        Dim intTipoArticulo As Integer
        Dim blnItemNuevo As Boolean = False

        Dim intIdrepXOrd As Integer
        Dim strIdrepXOrd As String = String.Empty

        'dr de Articulos
        Dim drAdentroArt As System.Data.DataRowCollection
        Dim drAfueraArt As System.Data.DataRowCollection

        'dr de Servicios
        Dim drAdentroServ As System.Data.DataRowCollection
        Dim drAfueraServ As System.Data.DataRowCollection

        'dr de Suministros
        Dim drAdentroSum As System.Data.DataRowCollection
        Dim drAfueraSum As System.Data.DataRowCollection


        Try

            drAdentroArt = dtbRepuestosxOrden.Copy.Rows
            drAfueraArt = m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Copy.Rows

            drAdentroServ = dtbActividadesXOrden.Copy.Rows
            drAfueraServ = m_dstActividadesxOrden.SCGTA_TB_ActividadesxOrden.Rows

            drAdentroSum = dtbSuministrosxOrden.Copy.Rows
            drAfueraSum = m_dstSuministrosxOrden.SCGTA_VW_Suministros.Rows

            oItem = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)

            For index As Integer = 0 To m_oCotizacion.Lines.Count - 1
                blnItemNuevo = False
                m_oCotizacion.Lines.SetCurrentLine(index)

                intIdrepXOrd = m_oCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value
                strIdrepXOrd = Convert.ToString(intIdrepXOrd)

                If Not Integer.TryParse(m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString, intTipoArticulo) Then intTipoArticulo = 0

                Select Case intTipoArticulo

                    Case 1, 4

                        If String.IsNullOrEmpty(strIdrepXOrd) OrElse strIdrepXOrd = 0 Then

                            For Each m_drwRepuestos In drAdentroArt

                                If m_oCotizacion.Lines.LineNum = m_drwRepuestos.LineNum Then
                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = m_drwRepuestos.ID
                                    drAdentroArt.Remove(m_drwRepuestos)
                                    blnItemNuevo = True
                                    Exit For
                                End If

                            Next

                        End If

                        If Not blnItemNuevo Then

                            For Each m_drwRepuestos In drAfueraArt

                                If m_oCotizacion.Lines.LineNum = m_drwRepuestos.LineNum Then
                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = m_drwRepuestos.ID
                                    drAfueraArt.Remove(m_drwRepuestos)
                                    Exit For
                                End If


                            Next
                        End If

                        'Servicio
                    Case 2

                        If String.IsNullOrEmpty(strIdrepXOrd) OrElse strIdrepXOrd = 0 Then

                            For Each m_drwActividades In drAdentroServ

                                If m_oCotizacion.Lines.LineNum = m_drwActividades.LineNum Then
                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = m_drwActividades.ID
                                    drAdentroServ.Remove(m_drwActividades)
                                    blnItemNuevo = True
                                    Exit For
                                End If

                            Next

                        End If

                        If Not blnItemNuevo Then

                            For Each m_drwActividades In drAfueraServ

                                If m_oCotizacion.Lines.LineNum = m_drwActividades.LineNum Then
                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = m_drwActividades.ID
                                    drAfueraServ.Remove(m_drwActividades)
                                    Exit For
                                End If


                            Next
                        End If


                        'Suministro
                    Case 3

                        If String.IsNullOrEmpty(strIdrepXOrd) OrElse strIdrepXOrd = 0 Then

                            For Each m_drwSuministros In drAdentroSum

                                If m_oCotizacion.Lines.LineNum = m_drwSuministros.LineNum Then
                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = m_drwSuministros.ID
                                    drAdentroSum.Remove(m_drwSuministros)
                                    blnItemNuevo = True
                                    Exit For
                                End If

                            Next

                        End If

                        If Not blnItemNuevo Then

                            For Each m_drwSuministros In drAfueraSum

                                If m_oCotizacion.Lines.LineNum = m_drwSuministros.LineNum Then
                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = m_drwSuministros.ID
                                    drAfueraSum.Remove(m_drwSuministros)
                                    Exit For
                                End If

                            Next

                        End If


                End Select

            Next

            Utilitarios.DestruirObjeto(oItem)

            'Items que se van a borrar -- ARTICULOS, SERV EXTERNOS
            If drAfueraArt.Count > 0 Then

                For Each m_drwRepuestos In drAfueraArt
                    objItemLineNumErroneo.NoOrden = m_drwRepuestos.NoOrden
                    objItemLineNumErroneo.IdItem = m_drwRepuestos.NoRepuesto
                    objItemLineNumErroneo.Id = m_drwRepuestos.ID
                    objItemLineNumErroneo.intLineNum = m_drwRepuestos.LineNum
                    objItemLineNumErroneo.TipoRow = enumTipoRow.scgRepuestoRow
                    objItemsLineasLineNumErroneos.Add(objItemLineNumErroneo)
                Next

            End If

            'Items que se van a borrar -- SERVICIOS
            If drAfueraServ.Count > 0 Then

                For Each m_drwActividades In drAfueraServ
                    objItemLineNumErroneo.NoOrden = m_drwRepuestos.NoOrden
                    objItemLineNumErroneo.IdItem = m_drwRepuestos.NoRepuesto
                    objItemLineNumErroneo.Id = m_drwRepuestos.ID
                    objItemLineNumErroneo.intLineNum = m_drwRepuestos.LineNum
                    objItemLineNumErroneo.TipoRow = enumTipoRow.scgRepuestoRow
                    objItemsLineasLineNumErroneos.Add(objItemLineNumErroneo)
                Next

            End If

            'Items que se van a borrar -- SUMINISTROS
            If drAfueraSum.Count > 0 Then

                For Each m_drwSuministros In drAfueraSum
                    objItemLineNumErroneo.NoOrden = m_drwRepuestos.NoOrden
                    objItemLineNumErroneo.IdItem = m_drwRepuestos.NoRepuesto
                    objItemLineNumErroneo.Id = m_drwRepuestos.ID
                    objItemLineNumErroneo.intLineNum = m_drwRepuestos.LineNum
                    objItemLineNumErroneo.TipoRow = enumTipoRow.scgRepuestoRow
                    objItemsLineasLineNumErroneos.Add(objItemLineNumErroneo)
                Next

            End If



        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Private Sub AgregaDatosControlColaborador(ByVal p_strDocEntry As String)

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildOT As SAPbobsCOM.GeneralData
        Dim oChildrenOT As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim dtFechaInicio As Date
        Dim strFechaInicio As String
        Dim strConsultaFaseProduccion As String = String.Empty
        Dim strHora, strMinutos, strFechaProduccion As String
        Dim FechaProduccion, auxFechaProduccion As DateTime
        Dim HoraInicio As TimeSpan

        oCompanyService = m_oCompany.GetCompanyService()
        oGeneralService = oCompanyService.GetGeneralService("SCGD_OT")
        oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
        oGeneralParams.SetProperty("Code", p_strDocEntry)
        oGeneralData = oGeneralService.GetByParams(oGeneralParams)


        oChildrenOT = oGeneralData.Child("SCGD_CTRLCOL")

        strFechaProduccion = Utilitarios.EjecutarConsulta(" Select IsNull(ci.U_FhaServ,'') From [@SCGD_OT] ot WITH (NOLOCK) INNER JOIN OQUT oq WITH (NOLOCK) ON ot.U_DocEntry = oq.DocEntry  INNER JOIN [@SCGD_CITA] ci WITH (NOLOCK) on ci.U_Num_Cot = oq.DocEntry Where ot.Code = '" + p_strDocEntry + "'", m_oCompany.CompanyDB, m_oCompany.Server)
        strHora = Utilitarios.EjecutarConsulta("SELECT IsNull(U_HoraServ,'') From [@SCGD_OT] ot WITH (NOLOCK) INNER JOIN OQUT oq WITH (NOLOCK) ON ot.U_DocEntry = oq.DocEntry  INNER JOIN [@SCGD_CITA] ci WITH (NOLOCK) on ci.U_Num_Cot = oq.DocEntry Where ot.Code = '" + p_strDocEntry + "'", m_oCompany.CompanyDB, m_oCompany.Server)


        Dim strFase As String = Utilitarios.EjecutarConsulta("SELECT Name FROM dbo.[@SCGD_FASEPRODUCCION] where Code = '1'", m_oCompany.CompanyDB, m_oCompany.Server)

        For Each linea As ListaActividadesCotizacion In objListaActividades

            oChildOT = oChildrenOT.Add()

            oChildOT.SetProperty("U_IdAct", linea.SCGD_ID)
            oChildOT.SetProperty("U_Colab", linea.SCGD_EmpAsig)
            oChildOT.SetProperty("U_Estad", "1")
            oChildOT.SetProperty("U_NoFas", strFase)
            oChildOT.SetProperty("U_CosEst", 0)

            If (strFechaProduccion <> "" And DateTime.TryParse(strFechaProduccion, FechaProduccion)) And ((strHora <> "0") And (strHora <> "") And TimeSpan.TryParse(strHora, HoraInicio)) Then
                FechaProduccion = DateTime.Parse(strFechaProduccion)
                If (strHora.Contains(":") = False) Then
                    strHora = strHora.Substring(0, (strHora.Length - 2)) + ":" + strHora.Substring(strHora.Length - 2, 2) + ":00"
                End If


                HoraInicio = HoraInicio.Parse(strHora)
                FechaProduccion = FechaProduccion.Add(HoraInicio)


                oChildOT.SetProperty("U_FechPro", FechaProduccion.ToString())
  
                Dim strHours As String = String.Empty
                Dim strMinutes As String = String.Empty
                Dim strHoraFormateada = "{0}:{1}"

                strHours = HoraInicio.Hours.ToString()
                strMinutes = HoraInicio.Minutes.ToString()

                If strHours.Length = 1 Then
                    strHours = "0" + strHours
                End If

                If strMinutes = "0" Then
                    strMinutes = "00"
                End If

                strHoraFormateada = String.Format(strHoraFormateada, strHours, strMinutes)

                oChildOT.SetProperty("U_HoraIni", HoraInicio.Hours.ToString + ":" + HoraInicio.Minutes.ToString())

                oChildOT.SetProperty("U_HoraIni", strHoraFormateada)

                HoraInicio = HoraInicio + (TimeSpan.FromMinutes(linea.DuracionLabor.ToString))
                FechaProduccion = FechaProduccion.AddMinutes(linea.DuracionLabor) '.Add(HoraInicio)
            End If
        Next

        oGeneralService.Update(oGeneralData)

    End Sub


    Public Function ProcesarLineasAlCrear(ByVal p_blnConf_TallerEnSAP As Boolean) As Boolean

        Dim intNumLineaCotizacion As Integer
        Dim intCantidadLineasXPaquete As Integer
        Dim intEstadoPaquete As Integer
        Dim intTipoArticulo As TiposArticulos
        Dim strTipoArticulo As String

        'validacion para tiempo estandar
        Dim strValidacionTiempoEstandar As String
        Dim strTiempoEstandar As String
        Dim blTiempoEstandar As Boolean

        Dim strConexionDBSucursal As String

        Dim blnRechazarItem As Boolean
        Dim intGenerico As Integer
        Dim intTotalLineasPaquete As Integer
        Dim intEstadoTraslado As Integer
        Dim decCantidadItem As Decimal
        Dim intEstadoItem As ArticuloAprobado
        Dim intLineaNumFather As Integer = -1
        Dim blnTipoNoAdmitido As Boolean = False

        Dim blnLineaEliminadaPaquete As Boolean
        Dim blnArticuloBienConfigurado As Boolean
        Dim intCodFase As Integer
        Dim strCodFase As String
        Dim strNoCopias As String = "1"
        Dim strCentroCosto As String
        Dim strServicosExternosInventariables As String
        Dim strBodegaProcesoPorTipo As String

        Dim strDuracionEstandar As String

        Dim blnProcesarNo As Boolean = False
        Dim blnProcesarSi As Boolean = False
        Dim dtFechaServicio As Date
        Dim intHoraInicio As Integer

        Dim oItemArticulo As SAPbobsCOM.IItems

        Try

            If Not p_blnConf_TallerEnSAP Then

                m_adpActividadesxOrden = New ActividadesXFaseDataAdapter(strCadenaConexionBDTaller)
                m_adpRepuestosxOrden = New RepuestosxOrdenDataAdapter(strCadenaConexionBDTaller)
                m_adpSuministrosxOrden = New SuministrosDataAdapter(strCadenaConexionBDTaller)


                m_lstRepuestos.Clear()
                m_lstSuministros.Clear()
                m_lstServiociosEX.Clear()
                m_lstItemsEliminarRepuestos.Clear()
                m_lstItemsEliminarSuministros.Clear()
                m_lstItemACambiarEstado.Clear()
                m_lstItemACambiarEstadoAdicional.Clear()
                Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, strIdSucursal, strCadenaConexionBDTaller)
                objTransferenciaStock = New TransferenciaItems(SBO_Application, m_oCompany, strCadenaConexionBDTaller)
                objUtilitarios = New SCGDataAccess.Utilitarios(strCadenaConexionBDTaller)
                Utilitarios.DevuelveNombreBDTaller(SBO_Application, strIdSucursal, m_strBDTalller)

                Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, strIdSucursal, strConexionDBSucursal)
                adpConf = New ConfiguracionDataAdapter(strConexionDBSucursal)
                adpConf.Fill(dstConf)
                'adpConf.FillBodegasXCC(dstConfBXCC)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, mc_strCopiasRepRecepcion, strNoCopias)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, TransferenciaItems.mc_strIDSerieDocumentosTraslado, m_strIDSerieDocTrasnf)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, "SEInventariables", strServicosExternosInventariables)
                'strServicosExternosInventariables = Utilitarios.EjecutarConsulta("Select Valor from SCGTA_TB_Configuracion where Propiedad = 'SEInventariables'", m_strBDTalller, m_oCompany.Server)
                strBodegaProcesoPorTipo = Utilitarios.EjecutarConsulta("Select CodCentroCosto from dbo.SCGTA_TB_TipoOrden with(nolock) where CodTipoOrden = " & m_intTipoOT, m_strBDTalller, m_oCompany.Server)

            Else

                m_lstRepuestos.Clear()
                m_lstSuministros.Clear()
                m_lstServiociosEX.Clear()
                m_lstItemsEliminarRepuestos.Clear()
                m_lstItemsEliminarSuministros.Clear()
                m_lstItemACambiarEstado.Clear()
                m_lstItemACambiarEstadoAdicional.Clear()

                If m_blnServicosExternosInventariables = True Then
                    strServicosExternosInventariables = 1
                Else
                    strServicosExternosInventariables = 0
                End If
                m_blnControlColaborador = False
                Dim strCentroCostoPorTipoOT As String = Utilitarios.EjecutarConsulta("SELECT [@SCGD_CONF_TIP_ORDEN].U_CodCtCos " & _
                                                                                     "FROM [@SCGD_CONF_SUCURSAL] INNER JOIN " & _
                                                                                     "[@SCGD_CONF_TIP_ORDEN] ON [@SCGD_CONF_SUCURSAL].DocEntry = [@SCGD_CONF_TIP_ORDEN].DocEntry " & _
                                                                                     "WHERE [@SCGD_CONF_SUCURSAL].U_Sucurs ='" & strIdSucursal & "' AND [@SCGD_CONF_TIP_ORDEN].U_Code ='" & m_intTipoOT & "'",
                                                                                     m_oCompany.CompanyDB, m_oCompany.Server)

                strBodegaProcesoPorTipo = strCentroCostoPorTipoOT

            End If

            If m_strNoCopias = "" Then
                m_intNoCopiasRep = 1
            Else
                m_intNoCopiasRep = CInt(m_strNoCopias)
            End If

            oItemArticulo = m_oCompany.GetBusinessObject(BoObjectTypes.oItems)
            TransferenciaItems.intCodCCosto = 0

            'Validacion Tiempo Estandar
            strValidacionTiempoEstandar = DMS_Connector.Configuracion.ParamGenAddon.U_TiemEsta.Trim()

            Dim strNumeroSerieCita As String = m_oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value.ToString.Trim()

            'RECORRO LAS LINEAS DE LA COTIZACION
            For intNumLineaCotizacion = 0 To m_oCotizacion.Lines.Count - 1

                m_oCotizacion.Lines.SetCurrentLine(intNumLineaCotizacion)
                intEstadoTraslado = 0
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoItem & (m_oCotizacion.Lines.LineNum + 1) & My.Resources.Resource.Separador & m_oCotizacion.Lines.ItemDescription, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                oItemArticulo.GetByKey(m_oCotizacion.Lines.ItemCode)

                'Carga el tipo de artículo
                If String.IsNullOrEmpty(m_oCotizacion.Lines.ItemCode) Then
                    strTipoArticulo = TiposArticulos.scgNinguno
                Else
                    strTipoArticulo = oItemArticulo.UserFields.Fields.Item(mc_strTipoArticulo).Value
                    If Not String.IsNullOrEmpty(strTipoArticulo) Then
                        intTipoArticulo = IIf(IsNumeric(strTipoArticulo), CInt(strTipoArticulo), -1)
                    Else
                        blnTipoNoAdmitido = True
                        intTipoArticulo = -1
                    End If
                End If

                'Carga si el artículo es generico
                intGenerico = oItemArticulo.UserFields.Fields.Item(mc_strGenerico).Value
                If intGenerico = 0 Then
                    blnTipoNoAdmitido = True
                End If

                'Si el tipo del artículo es un artículo de cita se le asocia el tipo de artículo ninguno para que no sea procesado en la cotizacion
                If intTipoArticulo = 10 Then
                    intTipoArticulo = TiposArticulos.scgNinguno
                End If

                'validacion de tiempo standar
                blTiempoEstandar = True

                If strValidacionTiempoEstandar.Trim() = "Y" Then
                    If intTipoArticulo = TiposArticulos.scgActividad Then
                        strTiempoEstandar = oItemArticulo.UserFields.Fields.Item("U_SCGD_Duracion").Value.ToString.Trim
                        ValidaTiempoEstandar(strTiempoEstandar, blTiempoEstandar, m_oCotizacion.Lines.ItemCode)
                    End If
                End If

                If blTiempoEstandar Then

                    'Carga el estado del artículo
                    If intTipoArticulo > 0 Or (intTipoArticulo = TiposArticulos.scgRepuesto AndAlso intGenerico <> 0) Then

                        strCentroCosto = oItemArticulo.UserFields.Fields.Item(mc_strCodCentroCosto).Value.ToString().Trim()

                        If IsNumeric(strCentroCosto) Then
                            TransferenciaItems.intCodCCosto = CInt(strCentroCosto)
                        End If

                        Select Case intTipoArticulo

                            Case TiposArticulos.scgActividad
                                blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tNO, False)
                                If blnArticuloBienConfigurado Then
                                    strCodFase = oItemArticulo.UserFields.Fields.Item(mc_strFase).Value.ToString.Trim
                                    intCodFase = IIf(IsNumeric(strCodFase), strCodFase, 0)
                                    If intCodFase = 0 Then
                                        blnArticuloBienConfigurado = False
                                    End If
                                End If
                                blnTipoNoAdmitido = True
                                If Not IsNumeric(strCentroCosto) Then
                                    blnArticuloBienConfigurado = False
                                End If
                            Case TiposArticulos.scgPaquete
                                blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tNO, True)
                                blnTipoNoAdmitido = True
                            Case TiposArticulos.scgRepuesto
                                blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True)
                                blnTipoNoAdmitido = True
                                If Not IsNumeric(strCentroCosto) Then
                                    blnArticuloBienConfigurado = False
                                End If
                            Case TiposArticulos.scgServicioExt
                                If strServicosExternosInventariables = 0 Then
                                    blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True)
                                Else
                                    blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True)
                                End If
                                blnTipoNoAdmitido = True
                                If Not IsNumeric(strCentroCosto) Then
                                    blnArticuloBienConfigurado = False
                                End If
                            Case TiposArticulos.scgSuministro
                                blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True)
                                blnTipoNoAdmitido = True
                                If Not IsNumeric(strCentroCosto) Then
                                    blnArticuloBienConfigurado = False
                                End If
                            Case TiposArticulos.scgOtrosIngresos
                                blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True, False)
                                blnTipoNoAdmitido = True
                            Case TiposArticulos.scgOtrosGastos_Costos
                                blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True, False)
                                blnTipoNoAdmitido = True
                        End Select

                        If blnArticuloBienConfigurado Then

                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = m_strNoOrden
                            m_oCotizacion.Lines.ShipToCode = m_strShiptoCode

                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = strTipoArticulo
                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = String.Format("{0}-{1}-{2}", strIdSucursal, m_oCotizacion.Lines.LineNum, m_strNoOrden)

                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = strIdSucursal
                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = strCentroCosto

                            ' Valida que sea distinto a tipo articulo "Otros Ingresos"
                            If intTipoArticulo <> TiposArticulos.scgOtrosIngresos And intTipoArticulo <> TiposArticulos.scgOtrosGastos_Costos Then

                                Select Case intTipoArticulo

                                    Case TiposArticulos.scgActividad

                                        If String.IsNullOrEmpty(m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value) Or
                                           m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value.ToString.Trim() = 0 Then

                                            strDuracionEstandar = oItemArticulo.UserFields.Fields.Item("U_SCGD_Duracion").Value.ToString.Trim

                                            If Not String.IsNullOrEmpty(strDuracionEstandar) Then
                                                m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = strDuracionEstandar
                                            Else
                                                m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = 0
                                            End If

                                        End If

                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = "1"

                                    Case TiposArticulos.scgServicioExt
                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = "Y"

                                End Select


                                If Not String.IsNullOrEmpty(strIdSucursal) Then
                                    m_strNoBodegaRepu = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, mc_strBodegaRepuestos, strIdSucursal, SBO_Application)
                                    m_strNoBodegaSumi = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, TransferenciaItems.mc_strBodegaSuministros, strIdSucursal, SBO_Application)
                                    m_strNoBodegaSeEx = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, mc_strBodegaServiciosExternos, strIdSucursal, SBO_Application)
                                    m_strNoBodegaServ = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, mc_strBodegaServicios, strIdSucursal, SBO_Application)

                                    If String.IsNullOrEmpty(strBodegaProcesoPorTipo) Then
                                        m_strNoBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, TransferenciaItems.mc_strBodegaProceso, strIdSucursal, SBO_Application)
                                    Else
                                        m_strNoBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strBodegaProcesoPorTipo, TransferenciaItems.mc_strBodegaProceso, strIdSucursal, SBO_Application)
                                    End If
                                End If

                                decCantidadItem = 0
                                intEstadoItem = m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value


                                Select Case intTipoArticulo
                                    Case TiposArticulos.scgActividad
                                        If p_blnConf_TallerEnSAP Then
                                            If Not String.IsNullOrEmpty(strNumeroSerieCita) AndAlso Not String.IsNullOrEmpty(m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()) Then
                                                m_blnControlColaborador = True
                                                objListaActividades.Add(New ListaActividadesCotizacion() With {.SCGD_ID = m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim(),
                                                                                                           .SCGD_EmpAsig = m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim(),
                                                                                                           .SCGD_NombEmpleado = m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim(),
                                                                                                           .CostoEstandar = 0,
                                                                                                           .CostoReal = 0,
                                                                                                           .Estado = m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim(),
                                                                                                           .FechaInicioActividad = dtFechaServicio,
                                                                                                           .HoraInicio = intHoraInicio,
                                                                                                           .DuracionLabor = m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value.ToString.Trim()})
                                            End If
                                        End If
                                        m_oCotizacion.Lines.WarehouseCode = m_strNoBodegaServ
                                    Case TiposArticulos.scgRepuesto
                                        m_oCotizacion.Lines.WarehouseCode = m_strNoBodegaRepu
                                    Case TiposArticulos.scgSuministro
                                        m_oCotizacion.Lines.WarehouseCode = m_strNoBodegaSumi
                                    Case TiposArticulos.scgServicioExt
                                        m_oCotizacion.Lines.WarehouseCode = m_strNoBodegaSeEx
                                End Select

                                If intCantidadLineasXPaquete <= 0 Then
                                    intLineaNumFather = -1
                                End If
                                If (intEstadoItem <> ArticuloAprobado.scgFalta AndAlso intEstadoItem <> ArticuloAprobado.scgNo AndAlso intCantidadLineasXPaquete <= 0) Or (intEstadoPaquete = 1 AndAlso intCantidadLineasXPaquete > 0) Then
                                    If intTipoArticulo <> -1 Or Not (intTipoArticulo = 1 AndAlso intGenerico = 0) Then
                                        blnRechazarItem = False

                                        If intTipoArticulo = TiposArticulos.scgRepuesto Or intTipoArticulo = TiposArticulos.scgSuministro Then

                                            RevisionStock(m_oCotizacion.Lines, m_oCotizacion.DocEntry, m_strNoBodegaRepu, m_strNoBodegaSumi, intTipoArticulo, intGenerico, decCantidadItem, intEstadoTraslado, intCantidadLineasXPaquete, intTotalLineasPaquete, intEstadoPaquete, blnRechazarItem, False)

                                            If intEstadoTraslado <> 5 AndAlso intEstadoTraslado <> 3 AndAlso (intTipoArticulo = TiposArticulos.scgRepuesto OrElse intTipoArticulo = TiposArticulos.scgSuministro) Then

                                                If blnDraft AndAlso intEstadoTraslado = 4 Then
                                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 4
                                                Else
                                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = intEstadoTraslado
                                                End If

                                            ElseIf intEstadoTraslado = 5 Then
                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 1
                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strResultado).Value = My.Resources.Resource.ParaComprar

                                            ElseIf intEstadoTraslado = 3 AndAlso (intTipoArticulo = TiposArticulos.scgSuministro Or intTipoArticulo = TiposArticulos.scgRepuesto) Then
                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 3
                                            End If

                                            If m_oCotizacion.Lines.Quantity <> decCantidadItem AndAlso decCantidadItem <> 0 Then
                                                m_oCotizacion.Lines.Quantity = decCantidadItem
                                            End If

                                            If p_blnConf_TallerEnSAP Then

                                                Select Case intEstadoTraslado

                                                    Case 1
                                                        If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCompra).Value = "Y" Then
                                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = decCantidadItem.ToString(n)
                                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                                                        Else
                                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = decCantidadItem.ToString(n)
                                                        End If
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0

                                                    Case 3
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = decCantidadItem.ToString(n)
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0

                                                    Case 4
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = decCantidadItem.ToString(n)
                                                End Select

                                            End If

                                        End If

                                        If Not blnRechazarItem Then

                                            Call ActualizarItemsOT(m_oCotizacion.Lines, intCantidadLineasXPaquete, intEstadoPaquete, intTipoArticulo, intLineaNumFather, intCodFase)

                                            If intTipoArticulo = TiposArticulos.scgRepuesto Or intTipoArticulo = TiposArticulos.scgSuministro Or intTipoArticulo = TiposArticulos.scgServicioExt Then
                                                If (m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 0 AndAlso intEstadoTraslado <> 5) _
                                                 Or m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 3 Or m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 4 Then

                                                    Select Case intTipoArticulo

                                                        Case TiposArticulos.scgRepuesto

                                                            objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfRepuestos, m_lstRepuestos, m_oCotizacion.Lines, _
                                                                    m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                                    m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, False, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, False, blnDraft, m_oForm, 0, m_oCotizacion.DocEntry)

                                                        Case TiposArticulos.scgSuministro

                                                            ''Genera la Lista de los Suministros que se van a trasladar
                                                            objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfSuministros, m_lstSuministros, m_oCotizacion.Lines, _
                                                                    m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                                    m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, False, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, False, blnDraft, m_oForm, 0, m_oCotizacion.DocEntry)

                                                        Case TiposArticulos.scgServicioExt
                                                            ''Genera la Lista de los Servicios Externos que se van a trasladar
                                                            objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfServiciosEx, m_lstServiociosEX, m_oCotizacion.Lines, _
                                                                m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                                    m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, False, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, False, blnDraft, m_oForm, 0, m_oCotizacion.DocEntry)
                                                    End Select

                                                End If
                                            End If
                                        Else

                                            If intLineaNumFather <> -1 Then
                                                Dim intLineaActual As Integer

                                                intLineaActual = m_oCotizacion.Lines.LineNum
                                                m_oCotizacion.Lines.SetCurrentLine(intLineaNumFather)
                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = 2
                                                m_oCotizacion.Lines.SetCurrentLine(intLineaActual)
                                                intEstadoPaquete = 2
                                                If Not p_blnConf_TallerEnSAP Then
                                                    If m_dstRepuestosxOrden IsNot Nothing Then
                                                        blnLineaEliminadaPaquete = True
                                                        Do While blnLineaEliminadaPaquete
                                                            blnLineaEliminadaPaquete = False
                                                            For Each m_drwRepuestos In m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows
                                                                If m_drwRepuestos.RowState <> DataRowState.Deleted Then
                                                                    If m_drwRepuestos.LineNumFather = intLineaNumFather Then
                                                                        m_drwRepuestos.Delete()
                                                                        blnLineaEliminadaPaquete = True
                                                                        Exit For
                                                                    End If
                                                                End If
                                                            Next
                                                        Loop
                                                    End If
                                                    If dtbActividadesXOrden IsNot Nothing Then
                                                        blnLineaEliminadaPaquete = True
                                                        Do While blnLineaEliminadaPaquete
                                                            blnLineaEliminadaPaquete = False
                                                            For Each m_drwActividades In dtbActividadesXOrden.Rows
                                                                If m_drwActividades.RowState <> DataRowState.Deleted Then
                                                                    If m_drwActividades.LineNumFather = intLineaNumFather Then
                                                                        m_drwActividades.Delete()
                                                                        blnLineaEliminadaPaquete = True
                                                                        Exit For
                                                                    End If
                                                                End If
                                                            Next
                                                        Loop
                                                    End If
                                                    ' End If
                                                    If m_dstSuministrosxOrden IsNot Nothing Then
                                                        blnLineaEliminadaPaquete = True
                                                        Do While blnLineaEliminadaPaquete
                                                            blnLineaEliminadaPaquete = False
                                                            For Each m_drwSuministros In m_dstSuministrosxOrden.SCGTA_VW_Suministros.Rows
                                                                If m_drwSuministros.RowState <> DataRowState.Deleted Then
                                                                    If m_drwSuministros.LineNumFather = intLineaNumFather Then
                                                                        m_drwSuministros.Delete()
                                                                        blnLineaEliminadaPaquete = True
                                                                        Exit For
                                                                    End If
                                                                End If
                                                            Next
                                                        Loop
                                                    End If
                                                End If
                                                intCantidadLineasXPaquete -= 1
                                            Else

                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = 2

                                            End If
                                        End If
                                    Else
                                        SBO_Application.MessageBox(m_strCentroCosto + My.Resources.Resource.El_Item & m_oCotizacion.Lines.ItemDescription & My.Resources.Resource.MalConfigurado)
                                    End If
                                ElseIf intTipoArticulo = TiposArticulos.scgPaquete Then
                                    intCantidadLineasXPaquete = objUtilitarios.CantidadLineasPaquetes(m_oCotizacion.Lines.ItemCode)
                                    intEstadoPaquete = ArticuloAprobado.scgNo
                                    intLineaNumFather = m_oCotizacion.Lines.LineNum
                                ElseIf intCantidadLineasXPaquete > 0 Then
                                    intCantidadLineasXPaquete -= 1
                                End If
                                If intCantidadLineasXPaquete <= 0 Then
                                    intEstadoPaquete = ArticuloAprobado.scgNo
                                End If

                                If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi And m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                                    blnProcesarSi = True
                                ElseIf m_oCotizacion.Lines.TreeType <> SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                    blnProcesarSi = False
                                End If

                                If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo And m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                                    blnProcesarNo = True
                                ElseIf m_oCotizacion.Lines.TreeType <> SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                    blnProcesarNo = False
                                End If

                                If blnProcesarSi = True Then
                                    If m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                        m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAProcesar).Value = LineaAProcesar.scgSi
                                    End If
                                ElseIf blnProcesarNo = True Then
                                    If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo And m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                        m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAProcesar).Value = LineaAProcesar.scgNo
                                    End If
                                End If
                            Else
                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strNoOT).Value = m_oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value
                            End If

                        Else
                            SBO_Application.MessageBox(m_strCentroCosto + My.Resources.Resource.El_Item & m_oCotizacion.Lines.ItemDescription & My.Resources.Resource.MalConfigurado)
                        End If

                    Else
                        If blnTipoNoAdmitido Then
                            If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi Then
                                SBO_Application.MessageBox(m_strCentroCosto + My.Resources.Resource.El_Item & m_oCotizacion.Lines.ItemDescription & My.Resources.Resource.MalConfigurado)
                            End If
                        End If
                    End If

                Else
                    SBO_Application.MessageBox(My.Resources.Resource.El_Item & m_oCotizacion.Lines.ItemDescription & My.Resources.Resource.ValidacionTiempoEstandar)
                End If 'validacion tiempo standar
            Next

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        Finally
            Utilitarios.DestruirObjeto(oItemArticulo)
        End Try

    End Function

    Private Function ValidaProcesaItem(ByRef p_oCotizacion As SAPbobsCOM.Documents,
                                       ByRef p_intTipoArticulo As Integer,
                                       ByRef p_blnAprobacionSER As Boolean,
                                       ByRef p_blnAprobacionSE As Boolean) As Boolean
        Try
            Select Case p_intTipoArticulo
                Case TiposArticulos.scgActividad
                    If Not p_blnAprobacionSER Then
                        If p_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo And Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()) And p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value.ToString.Trim() <> "1" Then
                            p_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi
                            SBO_Application.MessageBox(p_oCotizacion.Lines.ItemDescription & ": " & My.Resources.Resource.ValidacionMOAprobada)
                        End If
                    End If
                Case TiposArticulos.scgServicioExt
                    If Not p_blnAprobacionSE Then
                        If p_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo And p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value > 0 And p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value.ToString.Trim() <> "1" Then
                            p_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi
                            SBO_Application.MessageBox(p_oCotizacion.Lines.ItemDescription & ": " & My.Resources.Resource.ValidacionSEAprobado)
                        End If
                    End If
            End Select
            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        End Try
    End Function

    Private Sub ProcesarLineasAlActualizar(ByVal p_blnConf_TallerEnSAP As Boolean)

        Dim intNumLineaCotizacion As Integer
        Dim intCantidadLineasXPaquete As Integer
        Dim intEstadoPaquete As Integer

        Dim strTiempoEstandar As String = String.Empty
        Dim blTiempoEstandar As Boolean = False

        Dim blnCodePermiso As Boolean

        Dim blnLineaEliminada As Boolean = False
        Dim intTipoArticulo As TiposArticulos
        Dim strTipoArticulo As String
        Dim intCodFase As Integer
        Dim strCodFase As String

        Dim blnRechazarItem As Boolean
        Dim intGenerico As Integer
        Dim intTotalLineasPaquete As Integer
        Dim intEstadoTraslado As Integer
        Dim decCantidadItem As Decimal
        Dim intEstadoItem As ArticuloAprobado
        Dim intLineaNumFather As Integer = -1
        Dim blnEsLineaNueva As Boolean
        Dim blnLineaEliminadaPaquete As Boolean
        Dim blnArticuloBienConfigurado As Boolean = True
        Dim decCantidadAdicional As Decimal
        Dim blnDisminuirCantidad As Boolean = False
        Dim blnTipoNoAdmitido As Boolean = False
        Dim strCentroCosto As String
        Dim strServicosExternosInventariables As String
        Dim strBodegaProcesoPorTipo As String

        Dim blnMsjDevolverEnviado As Boolean = False

        Dim strDuracionEstandar As String = String.Empty

        Dim blnPaqueteNoAprobado As Boolean = False

        'Valor bolleano para identificar si la linea correspoindiente a un kit de tipo venta le asigno el valor de Procesar=NO 
        Dim blnProcesarNo As Boolean = False
        Dim blnProcesarSi As Boolean = False

        Dim oItemArticulo As SAPbobsCOM.IItems
        Dim strIdSucursal As String = String.Empty
        Dim blnValidaAprobacionSE As Boolean = False
        Dim blnValidaAprobacionSER As Boolean = False
        Try

            If objTransferenciaStock IsNot Nothing Then
                objTransferenciaStock = Nothing
            End If

            If Not p_blnConf_TallerEnSAP Then

                Utilitarios.DevuelveNombreBDTaller(SBO_Application, strIdSucursal, m_strBDTalller)
                m_dstRepuestosxOrden = New RepuestosxOrdenDataset
                m_adpRepuestosxOrden = New RepuestosxOrdenDataAdapter(strCadenaConexionBDTaller)
                m_adpRepuestosxOrden.Fill(m_dstRepuestosxOrden, m_strNoOrden)

                m_dstSuministrosxOrden = New SuministrosDataset
                m_adpSuministrosxOrden = New SuministrosDataAdapter(strCadenaConexionBDTaller)
                m_adpSuministrosxOrden.Fill(m_dstSuministrosxOrden, m_strNoOrden, -1, -1)

                m_dstActividadesxOrden = New ActividadesXFaseDataset
                m_adpActividadesxOrden = New ActividadesXFaseDataAdapter(strCadenaConexionBDTaller)
                m_adpActividadesxOrden.FillbyFilters(m_dstActividadesxOrden, m_strNoOrden, 0, 1)

                m_lstRepuestos.Clear()
                m_lstSuministros.Clear()
                m_lstServiociosEX.Clear()
                m_lstItemsEliminarRepuestos.Clear()
                m_lstItemsEliminarSuministros.Clear()
                m_lstItemACambiarEstado.Clear()
                m_lstItemACambiarEstadoAdicional.Clear()
                Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, strIdSucursal, strCadenaConexionBDTaller)
                objTransferenciaStock = New TransferenciaItems(SBO_Application, m_oCompany, strCadenaConexionBDTaller)
                objUtilitarios = New SCGDataAccess.Utilitarios(strCadenaConexionBDTaller)

                'Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, strIdSucursal, strConexionDBSucursal)
                adpConf = New ConfiguracionDataAdapter(strCadenaConexionBDTaller)
                adpConf.Fill(dstConf)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, TransferenciaItems.mc_strIDSerieDocumentosTraslado, m_strIDSerieDocTrasnf)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, "SEInventariables", strServicosExternosInventariables)
                strBodegaProcesoPorTipo = Utilitarios.EjecutarConsulta("Select CodCentroCosto from dbo.SCGTA_TB_TipoOrden where CodTipoOrden = " & m_intTipoOT, m_strBDTalller, m_oCompany.Server)
            Else

                m_lstRepuestos.Clear()
                m_lstSuministros.Clear()
                m_lstServiociosEX.Clear()
                m_lstItemsEliminarRepuestos.Clear()
                m_lstItemsEliminarSuministros.Clear()
                m_lstItemACambiarEstado.Clear()
                m_lstItemACambiarEstadoAdicional.Clear()

                m_blnControlColaborador = False

                If objTransferenciaStock Is Nothing Then
                    objTransferenciaStock = New TransferenciaItems(SBO_Application, m_oCompany, "")
                End If

                If m_blnServicosExternosInventariables Then
                    strServicosExternosInventariables = 1
                Else
                    strServicosExternosInventariables = 0
                End If

                strIdSucursal = m_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()

                Dim strCentroCostoPorTipoOT As String = Utilitarios.EjecutarConsulta("SELECT [@SCGD_CONF_TIP_ORDEN].U_CodCtCos " & _
                                                                                     "FROM [@SCGD_CONF_SUCURSAL] INNER JOIN " & _
                                                                                     "[@SCGD_CONF_TIP_ORDEN] ON [@SCGD_CONF_SUCURSAL].DocEntry = [@SCGD_CONF_TIP_ORDEN].DocEntry " & _
                                                                                     "WHERE [@SCGD_CONF_SUCURSAL].U_Sucurs ='" & strIdSucursal & "' AND [@SCGD_CONF_TIP_ORDEN].U_Code ='" & m_intTipoOT & "'",
                                                                                     m_oCompany.CompanyDB, m_oCompany.Server)

                strBodegaProcesoPorTipo = strCentroCostoPorTipoOT
            End If

            intEstadoPaquete = 0
            intCantidadLineasXPaquete = 0
            SBO_Application.StatusBar.SetText(My.Resources.Resource.ActulizarLineasOT, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)


            If String.IsNullOrEmpty(g_strReducCant) Then
                g_strReducCant = "N"
            End If

            Dim m_blnConfOTSAP As Boolean = Utilitarios.ValidarOTInternaConfiguracion(m_oCompany)

            blnCodePermiso = Utilitarios.MostrarMenu("SCGD_RED", SBO_Application.Company.UserName)


            strIdSucursal = m_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()

            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(strIdSucursal)) Then
                With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(strIdSucursal))
                    If .U_DesAproSER = "Y" Then blnValidaAprobacionSER = True
                    If .U_DesAproSE = "Y" Then blnValidaAprobacionSE = True
                End With
            End If
            'SE COMIENZA A RECORRER LAS LINEAS
            For intNumLineaCotizacion = 0 To m_oCotizacion.Lines.Count - 1

                m_oCotizacion.Lines.SetCurrentLine(intNumLineaCotizacion)

                intEstadoTraslado = 0
                blnTipoNoAdmitido = False
                blnLineaEliminada = False
                blnDisminuirCantidad = False
                decCantidadAdicional = 0
                decCantidadItem = 0

                SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoItem & (intNumLineaCotizacion + 1) & My.Resources.Resource.Separador & m_oCotizacion.Lines.ItemDescription, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                oItemArticulo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
                oItemArticulo.GetByKey(m_oCotizacion.Lines.ItemCode)

                strTipoArticulo = oItemArticulo.UserFields.Fields.Item(mc_strTipoArticulo).Value
                If Not String.IsNullOrEmpty(strTipoArticulo) Then
                    intTipoArticulo = IIf(IsNumeric(strTipoArticulo), CInt(strTipoArticulo), -1)
                Else
                    blnTipoNoAdmitido = True
                    intTipoArticulo = -1
                End If

                intGenerico = oItemArticulo.UserFields.Fields.Item(mc_strGenerico).Value
                If intGenerico = 0 Then
                    blnTipoNoAdmitido = True
                End If

                'Si el tipo del artículo es un artículo de cita se le asocia el tipo de artículo ninguno para que no sea procesado en la cotizacion
                If intTipoArticulo = 10 Then
                    intTipoArticulo = TiposArticulos.scgNinguno
                End If

                'validacion de tiempo standar
                blTiempoEstandar = True

                If g_strTiemEsta = "Y" Then
                    If intTipoArticulo = TiposArticulos.scgActividad Then
                        'tiempo estandar configurado
                        strTiempoEstandar = oItemArticulo.UserFields.Fields.Item("U_SCGD_Duracion").Value.ToString.Trim
                        ValidaTiempoEstandar(strTiempoEstandar, blTiempoEstandar, m_oCotizacion.Lines.ItemCode)
                    End If
                End If

                If blTiempoEstandar Then

                    If ((intTipoArticulo > 0 AndAlso intTipoArticulo <> TiposArticulos.scgRepuesto) Or (intTipoArticulo = TiposArticulos.scgRepuesto AndAlso intGenerico <> 0)) Then

                        strCentroCosto = oItemArticulo.UserFields.Fields.Item(mc_strCodCentroCosto).Value.ToString.Trim

                        Select Case intTipoArticulo

                            Case TiposArticulos.scgActividad
                                blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tNO, False)
                                If blnArticuloBienConfigurado Then

                                    strCodFase = oItemArticulo.UserFields.Fields.Item(mc_strFase).Value.ToString.Trim
                                    intCodFase = IIf(IsNumeric(strCodFase), strCodFase, 0)
                                    If intCodFase = 0 Then
                                        blnArticuloBienConfigurado = False
                                    End If
                                End If
                                blnTipoNoAdmitido = True
                                If Not IsNumeric(strCentroCosto) Then
                                    blnArticuloBienConfigurado = False
                                End If

                            Case TiposArticulos.scgPaquete
                                blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tNO, True)
                                blnTipoNoAdmitido = True

                            Case TiposArticulos.scgRepuesto
                                blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True)
                                blnTipoNoAdmitido = True
                                If Not IsNumeric(strCentroCosto) Then
                                    blnArticuloBienConfigurado = False
                                End If

                            Case TiposArticulos.scgServicioExt
                                If strServicosExternosInventariables = 0 Then
                                    blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True)
                                Else
                                    blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True)
                                End If

                                blnTipoNoAdmitido = True
                                If Not IsNumeric(strCentroCosto) Then
                                    blnArticuloBienConfigurado = False
                                End If

                            Case TiposArticulos.scgSuministro
                                blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True)
                                blnTipoNoAdmitido = True
                                If Not IsNumeric(strCentroCosto) Then
                                    blnArticuloBienConfigurado = False
                                End If

                            Case TiposArticulos.scgOtrosIngresos
                                blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True, False)
                                blnTipoNoAdmitido = True

                            Case TiposArticulos.scgOtrosGastos_Costos
                                blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True, False)
                                blnTipoNoAdmitido = True

                        End Select

                        If blnArticuloBienConfigurado Then
                            If ValidaProcesaItem(m_oCotizacion, intTipoArticulo, blnValidaAprobacionSER, blnValidaAprobacionSE) Then
                                m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = m_strNoOrden

                                m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = strTipoArticulo
                                If String.IsNullOrEmpty(m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()) Then
                                    m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = String.Format("{0}-{1}-{2}", strIdSucursal, m_oCotizacion.Lines.LineNum, m_strNoOrden)
                                    If (strTipoArticulo.ToString().Trim() = "2") Then
                                        If p_blnConf_TallerEnSAP Then
                                            If Not String.IsNullOrEmpty(m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()) Then
                                                m_blnControlColaborador = True
                                                objListaActividades.Add(New ListaActividadesCotizacion() With {.SCGD_ID = m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim(),
                                                                                                           .SCGD_EmpAsig = m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim(),
                                                                                                           .SCGD_NombEmpleado = m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim(),
                                                                                                           .CostoEstandar = 0,
                                                                                                           .CostoReal = 0,
                                                                                                           .Estado = m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim(),
                                                                                                           .FechaInicioActividad = Date.Now,
                                                                                                           .HoraInicio = String.Empty,
                                                                                                           .DuracionLabor = m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value.ToString.Trim()})
                                            End If
                                        End If
                                    End If

                                End If

                                m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = strIdSucursal
                                m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = strCentroCosto
                                ' Valida que sea distinto a tipo articulo "Otros Ingresos"
                                If intTipoArticulo <> TiposArticulos.scgOtrosIngresos And intTipoArticulo <> TiposArticulos.scgOtrosGastos_Costos Then

                                    Select Case intTipoArticulo
                                        Case TiposArticulos.scgActividad

                                            If String.IsNullOrEmpty(m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value) Or
                                               m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value.ToString.Trim() = 0 Then

                                                strDuracionEstandar = oItemArticulo.UserFields.Fields.Item("U_SCGD_Duracion").Value.ToString.Trim

                                                If Not String.IsNullOrEmpty(strDuracionEstandar) Then
                                                    m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = strDuracionEstandar
                                                Else
                                                    m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = 0
                                                End If

                                            End If
                                            If String.IsNullOrEmpty(m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value) Then
                                                m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = "1"
                                            End If


                                        Case TiposArticulos.scgServicioExt

                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = "Y"

                                    End Select

                                    If Not String.IsNullOrEmpty(strIdSucursal) Then
                                        If m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CamEsp").Value = "1" Then
                                            m_strNoBodegaRepu = m_oCotizacion.Lines.WarehouseCode.ToString()
                                        Else
                                            m_strNoBodegaRepu = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, mc_strBodegaRepuestos, strIdSucursal, SBO_Application)
                                        End If
                                        m_strNoBodegaSumi = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, TransferenciaItems.mc_strBodegaSuministros, strIdSucursal, SBO_Application)
                                        m_strNoBodegaSeEx = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, mc_strBodegaServiciosExternos, strIdSucursal, SBO_Application)
                                        m_strNoBodegaServ = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, mc_strBodegaServicios, strIdSucursal, SBO_Application)

                                        If String.IsNullOrEmpty(strBodegaProcesoPorTipo) Then
                                            m_strNoBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, TransferenciaItems.mc_strBodegaProceso, strIdSucursal, SBO_Application)
                                        Else
                                            m_strNoBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strBodegaProcesoPorTipo, TransferenciaItems.mc_strBodegaProceso, strIdSucursal, SBO_Application)
                                        End If
                                    End If

                                    decCantidadItem = 0
                                    'm_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = m_strNoOrden

                                    'm_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = strTipoArticulo
                                    'm_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = String.Format("{0}-{1}-{2}", strIdSucursal, m_oCotizacion.Lines.LineNum, m_strNoOrden)

                                    'm_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = strIdSucursal
                                    'm_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = strCentroCosto

                                    If intCantidadLineasXPaquete <= 0 Then
                                        intLineaNumFather = -1
                                        intEstadoPaquete = 2
                                    End If
                                    blnRechazarItem = False


                                    Select Case intTipoArticulo
                                        Case TiposArticulos.scgActividad
                                            m_oCotizacion.Lines.WarehouseCode = m_strNoBodegaServ
                                        Case TiposArticulos.scgRepuesto
                                            m_oCotizacion.Lines.WarehouseCode = m_strNoBodegaRepu
                                        Case TiposArticulos.scgSuministro
                                            m_oCotizacion.Lines.WarehouseCode = m_strNoBodegaSumi
                                        Case TiposArticulos.scgServicioExt
                                            m_oCotizacion.Lines.WarehouseCode = m_strNoBodegaSeEx
                                    End Select


                                    If (intTipoArticulo = TiposArticulos.scgRepuesto Or intTipoArticulo = TiposArticulos.scgSuministro) Then
                                        If blnPaqueteNoAprobado Then
                                            If m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = 2
                                            End If
                                        Else
                                            If m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = 1
                                            End If

                                        End If
                                    End If

                                    intEstadoItem = m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value

                                    If (intTipoArticulo = TiposArticulos.scgRepuesto OrElse intTipoArticulo = TiposArticulos.scgSuministro) AndAlso _
                                        (m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 0 OrElse m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 3) Then

                                        RevisionStock(m_oCotizacion.Lines, m_oCotizacion.DocEntry, m_strNoBodegaRepu, m_strNoBodegaSumi, intTipoArticulo, intGenerico,
                                                      decCantidadItem, intEstadoTraslado, intCantidadLineasXPaquete, intTotalLineasPaquete, intEstadoPaquete, blnRechazarItem, False)

                                        If Not blnRechazarItem Then
                                            If (intEstadoTraslado <> 2 AndAlso intEstadoTraslado <> 5 AndAlso m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 0) AndAlso intTipoArticulo = TiposArticulos.scgRepuesto Then
                                                If blnDraft And intEstadoTraslado = 4 Then
                                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 4
                                                Else
                                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = intEstadoTraslado
                                                End If

                                            ElseIf intEstadoTraslado = 5 Then
                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 1
                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strResultado).Value = My.Resources.Resource.ParaComprar

                                            ElseIf intEstadoTraslado = 3 AndAlso intTipoArticulo = TiposArticulos.scgSuministro Then
                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 3
                                            End If

                                            If m_oCotizacion.Lines.Quantity <> decCantidadItem AndAlso decCantidadItem <> 0 Then
                                                m_oCotizacion.Lines.Quantity = decCantidadItem
                                            End If

                                            If p_blnConf_TallerEnSAP Then

                                                Select Case intEstadoTraslado

                                                    Case 1
                                                        If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCompra).Value = "Y" Then
                                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = decCantidadItem.ToString(n)
                                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                                                        Else
                                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = decCantidadItem.ToString(n)
                                                        End If
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0
                                                    Case 3
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = decCantidadItem.ToString(n)
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0

                                                    Case 4
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = 0
                                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = decCantidadItem.ToString(n)
                                                End Select
                                            End If

                                        End If


                                    ElseIf intTipoArticulo = TiposArticulos.scgSuministro OrElse intTipoArticulo = TiposArticulos.scgRepuesto Then

                                        For index2 As Integer = 0 To oCotizacionlocal.Lines.Count - 1
                                            oCotizacionlocal.Lines.SetCurrentLine(index2)
                                            If m_oCotizacion.Lines.LineNum = oCotizacionlocal.Lines.LineNum Then
                                                Exit For
                                            End If
                                        Next

                                        If oCotizacionlocal.Lines.Quantity < m_oCotizacion.Lines.Quantity AndAlso m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value <> 0 AndAlso intEstadoTraslado <> 5 Then
                                            SBO_Application.MessageBox(My.Resources.Resource.LacantidadDelItem + oCotizacionlocal.Lines.ItemDescription + My.Resources.Resource.CantidadNoAumenta + vbCrLf + My.Resources.Resource.AgregueLineaParaCantidad)
                                            m_oCotizacion.Lines.Quantity = oCotizacionlocal.Lines.Quantity
                                            decCantidadItem = m_oCotizacion.Lines.Quantity
                                        ElseIf oCotizacionlocal.Lines.Quantity > m_oCotizacion.Lines.Quantity Then
                                            If m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 2 Then
                                                If g_strReducCant = "Y" Then
                                                    If blnCodePermiso Then
                                                        blnDisminuirCantidad = True
                                                        decCantidadAdicional = oCotizacionlocal.Lines.Quantity - m_oCotizacion.Lines.Quantity
                                                        decCantidadItem = m_oCotizacion.Lines.Quantity
                                                    Else
                                                        blnDisminuirCantidad = False
                                                        decCantidadAdicional = 0
                                                        decCantidadItem = oCotizacionlocal.Lines.Quantity
                                                        m_oCotizacion.Lines.Quantity = oCotizacionlocal.Lines.Quantity
                                                        SBO_Application.MessageBox(My.Resources.Resource.CantidadNoDisminuye + oCotizacionlocal.Lines.ItemDescription)
                                                    End If
                                                Else
                                                    blnDisminuirCantidad = True
                                                    decCantidadAdicional = oCotizacionlocal.Lines.Quantity - m_oCotizacion.Lines.Quantity
                                                    decCantidadItem = m_oCotizacion.Lines.Quantity
                                                End If
                                            Else
                                                If g_strReducCant = "Y" Then
                                                    If Not blnCodePermiso Then
                                                        m_oCotizacion.Lines.Quantity = oCotizacionlocal.Lines.Quantity
                                                        SBO_Application.MessageBox(My.Resources.Resource.CantidadNoDisminuye + oCotizacionlocal.Lines.ItemDescription)
                                                    End If
                                                End If
                                            End If
                                        End If

                                    ElseIf intTipoArticulo = TiposArticulos.scgActividad Then
                                        If blnPaqueteNoAprobado Then
                                            If m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = 2
                                                intEstadoItem = m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value
                                            End If
                                        Else
                                            If m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = 1

                                                intEstadoItem = m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value
                                            End If
                                        End If
                                    ElseIf intTipoArticulo = TiposArticulos.scgServicioExt AndAlso m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 0 AndAlso m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = 1 Then
                                        m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 1
                                        m_oCotizacion.Lines.UserFields.Fields.Item(mc_strResultado).Value = My.Resources.Resource.ParaComprar
                                        decCantidadItem = m_oCotizacion.Lines.Quantity

                                        If p_blnConf_TallerEnSAP Then
                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = decCantidadItem.ToString(n)
                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = 0
                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0
                                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0
                                        End If

                                    ElseIf intTipoArticulo = TiposArticulos.scgServicioExt AndAlso m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = 2 Then
                                        m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 0

                                        'Validacion cuando el paquete es Aprobado = No
                                    ElseIf intTipoArticulo = TiposArticulos.scgPaquete AndAlso m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = 2 Then

                                        If m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                                            blnPaqueteNoAprobado = True
                                        End If
                                    ElseIf intTipoArticulo = TiposArticulos.scgPaquete AndAlso m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = 1 Then

                                        If m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                                            blnPaqueteNoAprobado = False
                                        End If
                                    End If

                                    If Not blnRechazarItem Then
                                        blnEsLineaNueva = False
                                        If (intEstadoItem = ArticuloAprobado.scgSi AndAlso intCantidadLineasXPaquete <= 0) Or (intEstadoPaquete = ArticuloAprobado.scgSi AndAlso intCantidadLineasXPaquete > 0) Then
                                            If Not p_blnConf_TallerEnSAP Then
                                                blnEsLineaNueva = ActualizarOrdenTrabajoAgregar(m_oCotizacion.Lines, intEstadoPaquete, intCantidadLineasXPaquete, intLineaNumFather, intTipoArticulo, intCodFase)
                                            Else
                                                blnEsLineaNueva = ActualizarOrdenTrabajoAgregar_OT_SAP(m_oCotizacion.Lines, intEstadoPaquete, intCantidadLineasXPaquete, intLineaNumFather, intTipoArticulo, intCodFase)
                                            End If
                                            m_blnIniciarTransaccion = False
                                            If intTipoArticulo = TiposArticulos.scgActividad Or intTipoArticulo = TiposArticulos.scgServicioExt Then
                                                intCantidadLineasXPaquete -= 1
                                            ElseIf blnEsLineaNueva AndAlso intTipoArticulo <> TiposArticulos.scgPaquete Then
                                                intCantidadLineasXPaquete -= 1
                                            End If
                                        Else
                                            blnEsLineaNueva = False
                                        End If

                                        If Not blnEsLineaNueva Then
                                            If intTipoArticulo <> TiposArticulos.scgPaquete Then

                                                If Not p_blnConf_TallerEnSAP Then
                                                    blnLineaEliminada = ActualizarOrdenTrabajoEliminar(m_oCotizacion.Lines, intEstadoPaquete, intTipoArticulo)
                                                    m_blnIniciarTransaccion = False
                                                Else
                                                    blnLineaEliminada = True
                                                    m_blnIniciarTransaccion = False
                                                End If

                                                If intTipoArticulo = TiposArticulos.scgActividad Or intTipoArticulo = TiposArticulos.scgServicioExt Then
                                                    intCantidadLineasXPaquete -= 1
                                                End If

                                            Else

                                                intEstadoPaquete = intEstadoItem
                                                intCantidadLineasXPaquete = objUtilitarios.CantidadLineasPaquetes(m_oCotizacion.Lines.ItemCode)
                                                intLineaNumFather = m_oCotizacion.Lines.LineNum
                                            End If
                                        End If


                                        If (m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 0 AndAlso intEstadoTraslado <> 5) _
                                                    Or m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 3 Or (decCantidadAdicional > 0 AndAlso Not blnDisminuirCantidad) Then


                                            Select Case intTipoArticulo

                                                Case TiposArticulos.scgRepuesto

                                                    If decCantidadAdicional = 0 Then
                                                        ''Genera la Lista de los Repuestos que se van a trasladar
                                                        objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfRepuestos, m_lstRepuestos, m_oCotizacion.Lines, _
                                                                m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                                m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, False, blnDraft, m_oForm, 0, m_oCotizacion.DocEntry)
                                                    Else
                                                        objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfRepuestos, m_lstRepuestos, m_oCotizacion.Lines, _
                                                                m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                                m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, True, blnDraft, m_oForm, decCantidadAdicional, m_oCotizacion.DocEntry)
                                                    End If
                                                Case TiposArticulos.scgSuministro
                                                    If decCantidadAdicional = 0 Then
                                                        'Genera la Lista de los Suministros que se van a trasladar
                                                        If m_intRealizarTraslados = enumRealizarTraslados.scgSi Then
                                                            objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfSuministros, m_lstSuministros, m_oCotizacion.Lines, _
                                                                    m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                                    m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, False, blnDraft, m_oForm, 0, m_oCotizacion.DocEntry)
                                                            'se agrega para cuando es Draft y no debe ser trasladado
                                                        ElseIf blnDraft And m_intRealizarTraslados = enumRealizarTraslados.scgNo Then
                                                            objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfSuministros, m_lstSuministros, m_oCotizacion.Lines, _
                                                                   m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                                   m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, False, blnDraft, m_oForm, 0, m_oCotizacion.DocEntry)
                                                        End If
                                                    Else
                                                        If m_intRealizarTraslados = enumRealizarTraslados.scgSi Then
                                                            objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfSuministros, m_lstSuministros, m_oCotizacion.Lines, _
                                                                    m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                                    m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, True, blnDraft, m_oForm, decCantidadAdicional, m_oCotizacion.DocEntry)
                                                        End If
                                                    End If
                                                Case TiposArticulos.scgServicioExt
                                                    'Genera la Lista de los Servicios Externos que se van a trasladar
                                                    objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfServiciosEx, m_lstServiociosEX, m_oCotizacion.Lines, _
                                                        m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                            m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, False, blnDraft, m_oForm, m_oCotizacion.DocEntry)
                                            End Select

                                            If intTipoArticulo <> 5 AndAlso intTipoArticulo <> TiposArticulos.scgActividad AndAlso intTipoArticulo <> TiposArticulos.scgServicioExt Then
                                                intCantidadLineasXPaquete -= 1
                                            End If

                                            '''''''''''''''''*******************************************'''''''''''''''''''''''''''
                                            'se agrega el if para verificar que el estado sea "Pendiente en Bodega"
                                            'y que sea draft
                                        ElseIf (blnDraft AndAlso intEstadoTraslado = 4 AndAlso m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 4) Or m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = 2 Then

                                            'ElseIf (blnDraft AndAlso intEstadoTraslado = 5 AndAlso strTrasladado = 4) Or strAprobado = 2 Then

                                            Select Case intTipoArticulo

                                                Case TiposArticulos.scgRepuesto

                                                    If decCantidadAdicional = 0 Then
                                                        ''Genera la Lista de los Repuestos que se van a trasladar
                                                        objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfRepuestos, m_lstRepuestos, m_oCotizacion.Lines, _
                                                                m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                                m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, False, blnDraft, m_oForm, 0, m_oCotizacion.DocEntry)
                                                    Else
                                                        objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfRepuestos, m_lstRepuestos, m_oCotizacion.Lines, _
                                                                m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                                m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, True, blnDraft, m_oForm, decCantidadAdicional, m_oCotizacion.DocEntry)
                                                    End If
                                                Case TiposArticulos.scgSuministro
                                                    If decCantidadAdicional = 0 Then
                                                        ''Genera la Lista de los Suministros que se van a trasladar
                                                        If m_intRealizarTraslados = enumRealizarTraslados.scgSi Then
                                                            objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfSuministros, m_lstSuministros, m_oCotizacion.Lines, _
                                                                    m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                                    m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, False, blnDraft, m_oForm, 0, m_oCotizacion.DocEntry)
                                                            'se agrega para cuando es Draft y no debe ser trasladado
                                                        ElseIf blnDraft And m_intRealizarTraslados = enumRealizarTraslados.scgNo Then
                                                            objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfSuministros, m_lstSuministros, m_oCotizacion.Lines, _
                                                                   m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                                   m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, False, blnDraft, m_oForm, 0, m_oCotizacion.DocEntry)
                                                        End If
                                                    Else
                                                        If m_intRealizarTraslados = enumRealizarTraslados.scgSi Then
                                                            objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfSuministros, m_lstSuministros, m_oCotizacion.Lines, _
                                                                    m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                                    m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, True, blnDraft, m_oForm, decCantidadAdicional, m_oCotizacion.DocEntry)
                                                        End If
                                                    End If
                                                Case TiposArticulos.scgServicioExt
                                                    ''Genera la Lista de los Servicios Externos que se van a trasladar
                                                    objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfServiciosEx, m_lstServiociosEX, m_oCotizacion.Lines, _
                                                        m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                            m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, False, blnDraft, m_oForm, m_oCotizacion.DocEntry)
                                            End Select
                                            If intTipoArticulo <> 5 AndAlso intTipoArticulo <> TiposArticulos.scgActividad AndAlso intTipoArticulo <> TiposArticulos.scgServicioExt Then
                                                intCantidadLineasXPaquete -= 1
                                            End If

                                        End If


                                    Else
                                        If intLineaNumFather <> -1 Then
                                            If Not p_blnConf_TallerEnSAP Then
                                                m_oCotizacion.Lines.SetCurrentLine(intLineaNumFather)
                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = 2
                                                m_oCotizacion.Lines.SetCurrentLine(intNumLineaCotizacion)
                                                intEstadoPaquete = 2
                                                If m_dstRepuestosxOrden IsNot Nothing Then
                                                    blnLineaEliminadaPaquete = True
                                                    Do While blnLineaEliminadaPaquete
                                                        blnLineaEliminadaPaquete = False
                                                        For Each m_drwRepuestos In m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows
                                                            If m_drwRepuestos.RowState <> DataRowState.Deleted Then
                                                                If m_drwRepuestos.LineNumFather = intLineaNumFather Then
                                                                    m_drwRepuestos.Delete()
                                                                    blnLineaEliminadaPaquete = True
                                                                    Exit For
                                                                End If
                                                            End If
                                                        Next
                                                    Loop
                                                End If
                                                If dtbActividadesXOrden IsNot Nothing Then
                                                    blnLineaEliminadaPaquete = True
                                                    Do While blnLineaEliminadaPaquete
                                                        blnLineaEliminadaPaquete = False
                                                        For Each m_drwActividades In dtbActividadesXOrden.Rows
                                                            If m_drwActividades.RowState <> DataRowState.Deleted Then
                                                                If m_drwActividades.LineNumFather = intLineaNumFather Then
                                                                    m_drwActividades.Delete()
                                                                    blnLineaEliminadaPaquete = True
                                                                    Exit For
                                                                End If
                                                            End If
                                                        Next
                                                    Loop
                                                End If
                                                ' End If
                                                If m_dstSuministrosxOrden IsNot Nothing Then
                                                    blnLineaEliminadaPaquete = True
                                                    Do While blnLineaEliminadaPaquete
                                                        blnLineaEliminadaPaquete = False
                                                        For Each m_drwSuministros In m_dstSuministrosxOrden.SCGTA_VW_Suministros.Rows
                                                            If m_drwSuministros.RowState <> DataRowState.Deleted Then
                                                                If m_drwSuministros.LineNumFather = intLineaNumFather Then
                                                                    m_drwSuministros.Delete()
                                                                    blnLineaEliminadaPaquete = True
                                                                    Exit For
                                                                End If
                                                            End If
                                                        Next
                                                    Loop
                                                End If
                                                intCantidadLineasXPaquete -= 1
                                            End If

                                        Else

                                            m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = 2

                                        End If
                                    End If

                                    Dim estadoTrasladado As String = m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value.ToString
                                    Dim cPenBod As Integer = 0
                                    Dim cRec As Double = 0
                                    Dim cPDe As Double = 0
                                    If (blnLineaEliminada AndAlso (intTipoArticulo <> TiposArticulos.scgActividad AndAlso intTipoArticulo <> TiposArticulos.scgPaquete AndAlso intTipoArticulo <> TiposArticulos.scgServicioExt)) Or (decCantidadAdicional > 0 AndAlso blnDisminuirCantidad) Then

                                        If intTipoArticulo = TiposArticulos.scgRepuesto Then
                                            If Not blnDisminuirCantidad Then
                                                ''Genera la Lista de los Items que se van a trasladar de regreso a su bodega de origen
                                                objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfItemsEliminar, m_lstItemsEliminarRepuestos, m_oCotizacion.Lines, _
                                                        m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                        m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, False, blnDraft, m_oForm, 0, m_oCotizacion.DocEntry, oCotizacionlocal.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value, m_blnConfOTSAP)

                                                If (m_blnConfOTSAP AndAlso (estadoTrasladado = 2 OrElse estadoTrasladado = 4)) Then
                                                    cPenBod = m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCPenBod).Value
                                                    cRec = m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCRec).Value
                                                    cPDe = m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCPenDev).Value
                                                    'm_oCotizacion.Lines.UserFields.Fields.Item(mc_strCPenBod).Value = cPenBod + cRec
                                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCRec).Value = 0
                                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCPenDev).Value = cPDe + cRec
                                                    m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Entregado").Value = "N"
                                                End If

                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 0
                                            Else
                                                objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfItemsEliminar, m_lstItemsEliminarRepuestos, m_oCotizacion.Lines, _
                                                       m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                       m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, True, blnDraft, m_oForm, decCantidadAdicional, m_oCotizacion.DocEntry, oCotizacionlocal.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value, m_blnConfOTSAP)
                                            End If

                                            If m_lstItemsEliminarRepuestos.Count <> 0 AndAlso blnMsjDevolverEnviado = False AndAlso blnDraft Then
                                                SBO_Application.MessageBox(My.Resources.Resource.DevolverItemNoAprob)
                                                blnMsjDevolverEnviado = True
                                            End If

                                        ElseIf intTipoArticulo = TiposArticulos.scgSuministro Then
                                            If Not blnDisminuirCantidad Then
                                                ''Genera la Lista de los Items que se van a trasladar de regreso a su bodega de origen
                                                objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfItemsEliminar, m_lstItemsEliminarSuministros, m_oCotizacion.Lines, _
                                                        m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                        m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, False, blnDraft, m_oForm, 0, m_oCotizacion.DocEntry, oCotizacionlocal.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value, m_blnConfOTSAP)
                                                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 0

                                                If (m_blnConfOTSAP AndAlso (estadoTrasladado = 2 OrElse estadoTrasladado = 4)) Then
                                                    cPenBod = m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCPenBod).Value
                                                    cRec = m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCRec).Value
                                                    cPDe = m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCPenDev).Value
                                                    'm_oCotizacion.Lines.UserFields.Fields.Item(mc_strCPenBod).Value = cPenBod + cRec
                                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCRec).Value = 0
                                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCPenDev).Value = cPDe + cRec
                                                    m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Entregado").Value = "N"
                                                End If
                                            Else
                                                objTransferenciaStock.GeneraLista(TransferenciaItems.scgTiposMovimientoXBodega.TransfItemsEliminar, m_lstItemsEliminarSuministros, m_oCotizacion.Lines, _
                                                        m_strNoBodegaRepu, m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, _
                                                        m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, intTipoArticulo, intEstadoPaquete, intCantidadLineasXPaquete, intGenerico, True, blnDraft, m_oForm, decCantidadAdicional, m_oCotizacion.DocEntry, oCotizacionlocal.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value, m_blnConfOTSAP)

                                            End If

                                            If m_lstItemsEliminarSuministros.Count <> 0 AndAlso blnMsjDevolverEnviado = False AndAlso blnDraft Then
                                                SBO_Application.MessageBox(My.Resources.Resource.DevolverItemNoAprob)
                                                blnMsjDevolverEnviado = True
                                            End If

                                        End If
                                        blnLineaEliminada = False
                                        intCantidadLineasXPaquete -= 1

                                    End If

                                    'Verifica si no han habido cambios, pero el estado de aprobación de la línea ha cambiado
                                    If m_intEstCotizacion = CotizacionEstado.sinCambio Then

                                        If intNumLineaCotizacion < m_oCotizacionAnterior.Lines.Count Then

                                            If m_oCotizacionAnterior.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value <> m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value Then
                                                m_intEstCotizacion = CotizacionEstado.modificada
                                            End If

                                        End If

                                    End If

                                    'Cuando pongo el articulo Aprobado NO, borro el IdRepxOrd
                                    If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo AndAlso m_oCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value <> 0 Then
                                        m_oCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = String.Empty
                                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = String.Empty
                                    End If

                                    If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo And
                                        m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCompra).Value = "Y" Then

                                        m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCPen).Value = 0
                                        m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCSol).Value = 0
                                        m_oCotizacion.Lines.UserFields.Fields.Item(mc_strCRec).Value = 0
                                    End If

                                    If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi And m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                                        blnProcesarSi = True
                                    ElseIf m_oCotizacion.Lines.TreeType <> SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                        blnProcesarSi = False
                                    End If

                                    If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo And m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                                        blnProcesarNo = True
                                    ElseIf m_oCotizacion.Lines.TreeType <> SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                        blnProcesarNo = False
                                    End If

                                    If blnProcesarSi = True Then
                                        If m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                            m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAProcesar).Value = LineaAProcesar.scgSi
                                        End If
                                    ElseIf blnProcesarNo = True Then
                                        If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo And m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                            m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAProcesar).Value = LineaAProcesar.scgNo
                                        End If
                                    End If

                                Else
                                    m_oCotizacion.Lines.UserFields.Fields.Item(mc_strNoOT).Value = m_oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value
                                End If
                            End If

                        Else
                            If blnTipoNoAdmitido Then
                                If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi Then
                                    SBO_Application.MessageBox(m_strCentroCosto + My.Resources.Resource.El_Item & m_oCotizacion.Lines.ItemDescription & My.Resources.Resource.MalConfigurado)
                                End If

                            End If
                        End If

                    Else
                        If blnTipoNoAdmitido Then
                            If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi Then
                                SBO_Application.MessageBox(m_strCentroCosto + My.Resources.Resource.El_Item & m_oCotizacion.Lines.ItemDescription & My.Resources.Resource.MalConfigurado)
                            End If

                        End If
                    End If
                Else
                    SBO_Application.MessageBox(My.Resources.Resource.El_Item & m_oCotizacion.Lines.ItemDescription & My.Resources.Resource.ValidacionTiempoEstandar)
                End If 'validacion tiempo standar


                'Objeto Cotizacion para la mejora del procesamiento de las lineas (Rendimeinto)
                For i As Integer = 0 To oCotizacionlocal.Lines.Count - 1
                    oCotizacionlocal.Lines.SetCurrentLine(i)
                    If m_oCotizacion.Lines.LineNum = oCotizacionlocal.Lines.LineNum Then
                        If oCotizacionlocal.Lines.TreeType = BoItemTreeTypes.iNotATree Then
                            oCotizacionlocal.Lines.Delete()
                        End If
                        Exit For
                    End If
                Next

            Next

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        Finally
            Utilitarios.DestruirObjeto(oItemArticulo)
            Utilitarios.DestruirObjeto(oCotizacionlocal)
        End Try

    End Sub


    Private Sub ActualizarItemsOT(ByRef p_oLineaCotizacion As SAPbobsCOM.Document_Lines, _
                                       ByRef p_intCantidadLineasPaquete As Integer, _
                                       ByRef p_intEstadoPaquete As ArticuloAprobado, _
                                       ByVal p_intTipoArticulo As TiposArticulos, _
                                       ByRef p_intLineNumFather As Integer, _
                                       ByVal p_intCodFase As Integer)

        Dim decCantidad As Double
        Dim strItemCode As String
        Dim intLineNum As Integer
        Dim decDuracion As Decimal
        Dim intEstadoTransf As Integer
        Dim intItemAprobado As ArticuloAprobado
        Dim intIDEmpleado As Integer
        Dim objUtilitarios As New SCGDataAccess.Utilitarios(strCadenaConexionBDTaller)
        Dim strCodeEspecifico As String
        Dim strNameEspecifico As String
        Dim strDuracionActividad As String = String.Empty

        Dim strEsCompra As String = String.Empty

        decCantidad = p_oLineaCotizacion.Quantity
        strItemCode = p_oLineaCotizacion.ItemCode
        intLineNum = p_oLineaCotizacion.LineNum

        intItemAprobado = p_oLineaCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value
        intEstadoTransf = p_oLineaCotizacion.UserFields.Fields.Item(mc_strTrasladado).Value

        strCodeEspecifico = p_oLineaCotizacion.UserFields.Fields.Item(mc_strCodeEspecifico).Value
        strNameEspecifico = p_oLineaCotizacion.UserFields.Fields.Item(mc_strNameEspecifico).Value

        strEsCompra = p_oLineaCotizacion.UserFields.Fields.Item("U_SCGD_Compra").Value
        strEsCompra = strEsCompra.ToString.Trim()

        If ((intItemAprobado = ArticuloAprobado.scgSi AndAlso p_intCantidadLineasPaquete <= 0) _
            Or (intItemAprobado = ArticuloAprobado.scgSi AndAlso p_intTipoArticulo = TiposArticulos.scgPaquete)) _
            Or (p_intEstadoPaquete = ArticuloAprobado.scgSi AndAlso p_intCantidadLineasPaquete > 0) Then

            If p_intCantidadLineasPaquete <= 0 Then
                p_intLineNumFather = -1
            End If

            Select Case p_intTipoArticulo

                Case TiposArticulos.scgRepuesto
                    Call AgregarRepuesto(strItemCode, decCantidad, intLineNum, intEstadoTransf, p_intLineNumFather, strCodeEspecifico, strNameEspecifico, p_intTipoArticulo, strEsCompra)

                    If p_intCantidadLineasPaquete > 0 Then
                        p_intCantidadLineasPaquete -= 1
                    Else
                        p_intEstadoPaquete = ArticuloAprobado.scgNo
                    End If

                Case TiposArticulos.scgActividad

                    intIDEmpleado = IIf(IsNumeric(p_oLineaCotizacion.UserFields.Fields.Item(mc_strEmpRealiza).Value), p_oLineaCotizacion.UserFields.Fields.Item(mc_strEmpRealiza).Value, 0)

                    'Usa la configuracion de asociación de artículos por estilo. Carlos Céspedes
                    If m_UsaAsocxEspc.Equals("Y") Then

                        Dim strConsulta As String
                        Dim m_strConsultaArticulos As String = "  Select U_ItemCode from [@SCGD_ARTXESP] where U_TipoArt = '2' "
                        Dim m_strFiltroMod As String = " and U_CodMod = '{0}' "
                        Dim m_strFiltroArt As String = " and U_CodEsti = '{0}' "
                        Dim bExisteRep As Boolean
                        Dim m_dtRep As System.Data.DataTable
                        Dim strDuracion As String = ""

                        If m_EspecifVehi.Equals("E") Then
                            Dim strCodEstilo As String = m_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value
                            m_strConsultaArticulos = m_strConsultaArticulos & String.Format(m_strFiltroArt, strCodEstilo)
                            m_dtRep = Utilitarios.EjecutarConsultaDataTable(m_strConsultaArticulos, m_oCompany.CompanyDB, m_oCompany.Server)
                            If m_dtRep.Rows.Count > 0 Then
                                bExisteRep = True

                            Else
                                bExisteRep = False
                            End If

                            If bExisteRep Then

                                strConsulta = String.Format("Select art.U_Duracion From OITM as oi with(nolock) inner join [@SCGD_ARTXESP] as art with(nolock) on oi.ItemCode  = art.U_ItemCode " &
                                                             " where art.U_ItemCode = '{0}' and art.U_CodEsti = '{1}' and art.U_TipoArt = '2'", strItemCode, strCodEstilo)
                                strDuracion = Utilitarios.EjecutarConsulta(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)
                            Else
                                strDuracionActividad = p_oLineaCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value

                                If Not String.IsNullOrEmpty(strDuracionActividad) Then
                                    decDuracion = CType(strDuracionActividad, Decimal)
                                Else
                                    decDuracion = 0
                                End If
                            End If


                        ElseIf m_EspecifVehi.Equals("M") Then

                            Dim strCodModelo As String = m_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value
                            m_strConsultaArticulos = m_strConsultaArticulos & String.Format(m_strFiltroMod, strCodModelo)
                            m_dtRep = Utilitarios.EjecutarConsultaDataTable(m_strConsultaArticulos, m_oCompany.CompanyDB,
                                           m_oCompany.Server)
                            If m_dtRep.Rows.Count > 0 Then
                                bExisteRep = True

                            Else
                                bExisteRep = False
                            End If

                            If bExisteRep Then
                                strConsulta = String.Format("Select art.U_Duracion From OITM as oi with(nolock) inner join [@SCGD_ARTXESP] as art with(nolock) on oi.ItemCode  = art.U_ItemCode " &
                                                             " where art.U_ItemCode = '{0}' and art.U_CodMod = '{1}' and art.U_TipoArt = '2'", strItemCode, strCodModelo)
                                strDuracion = Utilitarios.EjecutarConsulta(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)
                            Else

                                strDuracionActividad = p_oLineaCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value

                                If Not String.IsNullOrEmpty(strDuracionActividad) Then
                                    decDuracion = CType(strDuracionActividad, Decimal)
                                Else
                                    decDuracion = 0
                                End If


                            End If

                        End If

                        If Not String.IsNullOrEmpty(strDuracion) Then
                            decDuracion = CType(strDuracion, Decimal)
                        Else
                            decDuracion = 0
                        End If

                        p_oLineaCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value = decDuracion.ToString()

                    Else

                        strDuracionActividad = p_oLineaCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value

                        If Not String.IsNullOrEmpty(strDuracionActividad) Then
                            decDuracion = CType(strDuracionActividad, Decimal)
                        Else
                            decDuracion = 0
                        End If

                    End If

                    Call AgregarActividades(strItemCode, decCantidad, p_intCodFase, decDuracion, intLineNum, intIDEmpleado, p_intLineNumFather)

                    If p_intCantidadLineasPaquete > 0 Then
                        p_intCantidadLineasPaquete -= 1
                    Else
                        p_intEstadoPaquete = ArticuloAprobado.scgNo
                    End If

                Case TiposArticulos.scgSuministro

                    Call AgregarSuministro(strItemCode, decCantidad, intLineNum, p_intLineNumFather)
                    If p_intCantidadLineasPaquete > 0 Then
                        p_intCantidadLineasPaquete -= 1
                    Else
                        p_intEstadoPaquete = ArticuloAprobado.scgNo
                    End If

                Case TiposArticulos.scgServicioExt

                    p_oLineaCotizacion.UserFields.Fields.Item(mc_strTrasladado).Value = 1
                    p_oLineaCotizacion.UserFields.Fields.Item(mc_strResultado).Value = My.Resources.Resource.ParaComprar

                    p_oLineaCotizacion.UserFields.Fields.Item("U_SCGD_CPen").Value = decCantidad
                    p_oLineaCotizacion.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                    p_oLineaCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                    p_oLineaCotizacion.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                    p_oLineaCotizacion.UserFields.Fields.Item("U_SCGD_CPTr").Value = 0
                    p_oLineaCotizacion.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0

                    Call AgregarRepuesto(strItemCode, decCantidad, intLineNum, intEstadoTransf, p_intLineNumFather, strCodeEspecifico, strNameEspecifico, TiposArticulos.scgServicioExt, strEsCompra)
                    If p_intCantidadLineasPaquete > 0 Then
                        p_intCantidadLineasPaquete -= 1
                    Else
                        p_intEstadoPaquete = ArticuloAprobado.scgNo
                        p_intLineNumFather = -1
                    End If

                Case TiposArticulos.scgPaquete
                    p_intCantidadLineasPaquete = objUtilitarios.CantidadLineasPaquetes(strItemCode)
                    p_intEstadoPaquete = ArticuloAprobado.scgSi
                    p_intLineNumFather = p_oLineaCotizacion.LineNum

                Case Else

            End Select
        Else
            If p_intTipoArticulo = TiposArticulos.scgPaquete Then

                p_intEstadoPaquete = intItemAprobado

                p_intCantidadLineasPaquete = objUtilitarios.CantidadLineasPaquetes(strItemCode)
            Else
                If p_intCantidadLineasPaquete > 0 Then
                    p_intCantidadLineasPaquete -= 1
                    p_intLineNumFather = -1
                End If
            End If
            p_oLineaCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo
        End If

    End Sub

    Private Function DevuelveValorItem(ByVal strItemcode As String, _
                                       ByVal strUDfName As String) As String

        Dim oItemArticulo As SAPbobsCOM.IItems
        Dim valorUDF As String

        oItemArticulo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
        oItemArticulo.GetByKey(strItemcode)
        valorUDF = oItemArticulo.UserFields.Fields.Item(strUDfName).Value

        If Not oItemArticulo Is Nothing Then
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oItemArticulo)
            oItemArticulo = Nothing
        End If

        Return valorUDF

    End Function

    Private Function DevuelveConfiguracionItem(ByRef p_Oitem As SAPbobsCOM.IItems, _
                                               ByVal p_objInventariable As SAPbobsCOM.BoYesNoEnum, _
                                               ByVal p_objDeVenta As SAPbobsCOM.BoYesNoEnum, _
                                               ByVal p_objDeCompra As SAPbobsCOM.BoYesNoEnum, _
                                               ByVal p_blnTomaEnCuentaVenta As Boolean, _
                                               Optional ByVal p_blnValidarCentroCosto As Boolean = True) As Boolean

        'Dim oItemArticulo As SAPbobsCOM.IItems
        Dim blnValidado As Boolean = True
        Dim strCentroCosto As String = String.Empty

        'oItemArticulo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
        'oItemArticulo.GetByKey(strItemcode)

        strCentroCosto = p_Oitem.UserFields.Fields.Item(mc_strCodCentroCosto).Value

        If p_Oitem.InventoryItem <> p_objInventariable Then
            Return False
        ElseIf (p_Oitem.PurchaseItem <> p_objDeCompra AndAlso p_blnTomaEnCuentaVenta) Then
            Return False
        ElseIf p_Oitem.SalesItem <> p_objDeVenta Then
            Return False
        End If

        If p_blnValidarCentroCosto = True Then

            blnValidado = ValidarCentroCosto(p_Oitem.ItemCode, p_Oitem)

            If blnValidado = False Then

                Select Case SBO_Application.Language
                    Case SAPbouiCOM.BoLanguages.ln_English
                        m_strCentroCosto = "Cost Center incorrectly configured: "
                    Case SAPbouiCOM.BoLanguages.ln_English_Cy
                        m_strCentroCosto = "Cost Center incorrectly configured: "
                    Case SAPbouiCOM.BoLanguages.ln_English_Gb
                        m_strCentroCosto = "Cost Center incorrectly configured: "
                    Case SAPbouiCOM.BoLanguages.ln_English_Sg
                        m_strCentroCosto = "Cost Center incorrectly configured: "
                    Case Else
                        m_strCentroCosto = "Centro de Costo mal configurado: "
                End Select

            Else
                m_strCentroCosto = String.Empty

            End If

        End If

        Return blnValidado

    End Function

    Private Function PasarDatos(ByVal oCotizacion As Documents) As Boolean

        m_intRealizarTraslados = enumRealizarTraslados.scgNo
        m_strNumeroVisita = oCotizacion.UserFields.Fields.Item(mc_strNum_Visita).Value
        m_intGenerarOT = oCotizacion.UserFields.Fields.Item(mc_strGenerarOT).Value
        m_intTipoOT = IIf(IsNumeric(oCotizacion.UserFields.Fields.Item(mc_strTipoOT).Value), oCotizacion.UserFields.Fields.Item(mc_strTipoOT).Value, 0)
        m_strNumeroOT = oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value
        m_strEmpleadoRecibe = IIf(IsNumeric(oCotizacion.DocumentsOwner), oCotizacion.DocumentsOwner, "")
        m_strNumeroUnidad = oCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value
        m_strNumeroVehiculo = oCotizacion.UserFields.Fields.Item(mc_strNumVehiculo).Value
        m_strEstadoCotizacion = oCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacion).Value
        m_strEstadoCotizacionID = oCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacionID).Value
        m_strNoOrden = oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value
        m_strOTPadre = oCotizacion.UserFields.Fields.Item(mc_strOTPadre).Value
        m_strPlaca = oCotizacion.UserFields.Fields.Item(mc_strPlaca).Value
        m_strVIN = oCotizacion.UserFields.Fields.Item(mc_strVIN).Value
        m_strDescMarca = oCotizacion.UserFields.Fields.Item(mc_strDescMarca).Value
        m_strDescModelo = oCotizacion.UserFields.Fields.Item(mc_strDescModelo).Value
        m_strDescEstilo = oCotizacion.UserFields.Fields.Item(mc_strDesc_Estilo).Value
        m_strCodeMarca = oCotizacion.UserFields.Fields.Item(mc_strCod_Marca).Value
        m_strCodeModelo = oCotizacion.UserFields.Fields.Item(mc_strCod_Modelo).Value
        m_strCodeEstilo = oCotizacion.UserFields.Fields.Item(mc_strCod_Estilo).Value
        m_strNoSerieCita = oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value
        m_strNoCita = oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value
        m_strShiptoCode = oCotizacion.ShipToCode
        m_dbkilometraje = oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value
        m_strClienteOT = oCotizacion.UserFields.Fields.Item(mc_strClienteOT).Value
        m_strNombreClienteOT = oCotizacion.UserFields.Fields.Item(mc_strNombreClienteOT).Value
        m_strAno = oCotizacion.UserFields.Fields.Item(mc_strAño).Value
        m_strCono = oCotizacion.UserFields.Fields.Item(mc_strCono).Value
        m_strCodigoCliente = oCotizacion.UserFields.Fields.Item(mc_strCardCode).Value
        m_strNombreCliente = oCotizacion.UserFields.Fields.Item(mc_strCardName).Value
        m_dtFechaRecepcion = oCotizacion.UserFields.Fields.Item(mc_strFech_Recep).Value
        m_dtHoraRecepcion = oCotizacion.UserFields.Fields.Item(mc_strHora_Recep).Value
        m_dtHoraCompromiso = oCotizacion.UserFields.Fields.Item(mc_strHora_Comp).Value
        m_dtFechaCompromiso = oCotizacion.UserFields.Fields.Item(mc_strFech_Comp).Value
        m_strOtReferencia = oCotizacion.UserFields.Fields.Item(mc_strOtRef).Value
        m_strOtNivelGas = oCotizacion.UserFields.Fields.Item(mc_strNGas).Value
        m_intHorasMotor = oCotizacion.UserFields.Fields.Item(mc_strHorasMotor).Value
        m_strObservaciones = oCotizacion.Comments

        If m_dtFechaRecepcion.Year = 1899 Then
            m_dtHoraRecepcion = Date.Now
        End If
        If m_dtFechaCompromiso.Year = 1899 Then
            m_dtHoraCompromiso = Date.Now
        End If

        m_strDocEntry = oCotizacion.DocEntry

        m_strSerieCompletaCita = m_strNoSerieCita & "-" & m_strNoCita

        m_intCodigoTecnico = ObtenerIdTecnicoCita(m_oForm, m_strSerieCompletaCita)

        If m_intCodigoTecnico Is Nothing Then
            m_intCodigoTecnico = ObtenerIdTecnicoAgenda(m_oForm, m_strSerieCompletaCita)
        End If

        If m_intGenerarOT = GeneraOrdenTrabajo.scgSiGenera AndAlso m_strNumeroOT = "" Then
            If m_intTipoOT = 0 Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.CotizacionSinTipo, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                Return False
            End If
            If m_strEmpleadoRecibe = "0" Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.CotizacionSinAsesor, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                Return False
                m_strEmpleadoRecibe = "0"
            End If
            If m_strNumeroVehiculo = "0" Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.CotizacionSinVehiculo, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                Return False
            End If
        End If
        m_strWAN = oCotizacion.UserFields.Fields.Item(mc_strWAN).Value
        m_strPoolAsig = oCotizacion.UserFields.Fields.Item(mc_strPoolAsig).Value
        m_strCompOri = oCotizacion.UserFields.Fields.Item(mc_strCompOri).Value
        m_strNumeroCaso = oCotizacion.UserFields.Fields.Item(mc_strNumeroCaso).Value
        m_strNoPol = oCotizacion.UserFields.Fields.Item(mc_strNoPol).Value
        m_strCompS = oCotizacion.UserFields.Fields.Item(mc_strCompS).Value
        m_strPeri = oCotizacion.UserFields.Fields.Item(mc_strPeri).Value
        m_strOwnerCode = oCotizacion.UserFields.Fields.Item(mc_strOwnerCode).Value

        Return True

    End Function


    Public Function ObtenerIdTecnicoCita(ByVal p_oform As SAPbouiCOM.Form, ByVal p_SerieCompleta As String) As Nullable(Of Integer)

        Dim strConexionDBSucursal As String = String.Empty
        Dim m_dataTable As SAPbouiCOM.DataTable

        ValidarDataTable(p_oform, m_dataTable)

        m_dataTable = p_oform.DataSources.DataTables.Item("dtConsulta")
        m_dataTable.ExecuteQuery(" Select U_Cod_Tecnico from [@SCGD_CITA] with(nolock) where U_NumCita = '" & p_SerieCompleta & "' ")

        Dim CodigoTecnicoCita As String = m_dataTable.GetValue("U_Cod_Tecnico", 0)

        If String.IsNullOrEmpty(CodigoTecnicoCita) Then
            Return Nothing
        Else
            Return CodigoTecnicoCita
        End If

    End Function

    Public Function ObtenerIdTecnicoAgenda(ByVal p_oform As SAPbouiCOM.Form, ByVal p_SerieCompleta As String) As Nullable(Of Integer)


        Dim strConexionDBSucursal As String = String.Empty
        Dim m_dataTable As SAPbouiCOM.DataTable

        ValidarDataTable(p_oform, m_dataTable)

        m_dataTable = p_oform.DataSources.DataTables.Item("dtConsulta")
        m_dataTable.ExecuteQuery(" select age.U_CodTecnico from [@SCGD_AGENDA] as age with(nolock) inner join [@SCGD_CITA] as cit with(nolock) on age.DocNum = cit.U_Cod_Agenda where cit.U_NumCita = '" & p_SerieCompleta & "' ")

        Dim CodigoTecnicoAgenda As String = m_dataTable.GetValue("U_CodTecnico", 0)

        If String.IsNullOrEmpty(CodigoTecnicoAgenda) Then
            Return Nothing
        Else
            Return CodigoTecnicoAgenda
        End If


    End Function


    Private Sub CrearVisita()

        Dim decNoVisita As Decimal
        Dim intVisita As Integer
        Dim m_intError As Integer

        decNoVisita = 1

        'Paso #1 Insertar la información del Tab General.

        m_adpVisita = New DMSOneFramework.SCGDataAccess.VisitasDataAdapter(strCadenaConexionBDTaller)
        m_dstVisita = New VisitaDataset
        m_drwVisita = m_dstVisita.SCGTA_TB_Visita.NewRow()

        'Se modifica el row con los atributos modificados y en caso que los dejen en blanco se cargan los dataColumns en blanco

        m_drwVisita.CardCode = m_strCodigoCliente
        m_drwVisita.NoVehiculo = m_strNumeroUnidad
        If m_strEmpleadoRecibe <> "" Then
            m_drwVisita.Asesor = m_strEmpleadoRecibe
        End If
        m_drwVisita.IdentCliente = m_strCodigoCliente
        If m_strNumeroVehiculo <> "" Then
            m_drwVisita.IDVehiculo = m_strNumeroVehiculo
        End If
        m_drwVisita.NoVehiculo = m_strNumeroUnidad
        m_drwVisita.NoVisita = 0
        m_drwVisita.Fecha_apertura = Date.Now
        m_drwVisita.Fecha_cierre = Date.Now
        m_drwVisita.Fecha_compromiso = Date.Now
        m_drwVisita.Cotizacion = m_intDocEntry

        '-- Inserta el row en el Dataset 
        m_dstVisita.SCGTA_TB_Visita.AddSCGTA_TB_VisitaRow(m_drwVisita)

        'Se modifica en la base de datos mediante los metodos de la capa de negocios.
        intVisita = m_adpVisita.Update(m_dstVisita, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion)
        m_strNumeroVisita = intVisita.ToString

        m_oCompany.StartTransaction()
        m_oCotizacion.UserFields.Fields.Item(mc_strNum_Visita).Value = m_strNumeroVisita
        m_intError = m_oCotizacion.Update()

        If m_intError = 0 Then
            If m_oCompany.InTransaction Then m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
        Else
            If m_oCompany.InTransaction Then m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
        End If

    End Sub

    Private Sub CrearOrdenTrabajo(ByVal p_strNoOT As String)

        m_adpOrdenTrabajo = New DMSOneFramework.SCGDataAccess.OrdenTrabajoDataAdapter(strCadenaConexionBDTaller)
        m_dstOrdenTrabajo = New OrdenTrabajoDataset
        m_drwOrdenTrabajo = m_dstOrdenTrabajo.SCGTA_TB_Orden.NewSCGTA_TB_OrdenRow

        'Se modifica el row con los atributos 

        m_drwOrdenTrabajo.NoOrden = p_strNoOT
        m_drwOrdenTrabajo.CodTipoOrden = m_intTipoOT
        m_drwOrdenTrabajo.NoVisita = m_strNumeroVisita
        m_drwOrdenTrabajo.ClienteFacturar = m_strCodigoCliente
        If m_strEmpleadoRecibe <> "" Then
            m_drwOrdenTrabajo.Asesor = m_strEmpleadoRecibe
        End If
        m_drwOrdenTrabajo.IDVehiculo = m_strNumeroVehiculo
        m_drwOrdenTrabajo.NoVehiculo = m_strNumeroUnidad
        m_drwOrdenTrabajo.NoCotizacion = m_intDocEntry
        'm_drwOrdenTrabajo.Kilometraje = m_kilometraje

        If m_intCodigoTecnico Is Nothing Then
            m_drwOrdenTrabajo.SetCodTecnicoNull()
        Else
            m_drwOrdenTrabajo.CodTecnico = m_intCodigoTecnico
        End If

        '-- Inserta el row en el Dataset 
        m_dstOrdenTrabajo.SCGTA_TB_Orden.AddSCGTA_TB_OrdenRow(m_drwOrdenTrabajo)
        'Se modifica en la base de datos mediante los metodos de la capa de negocios.
        'm_strNoOrden = m_adpOrdenTrabajo.Update(m_dstOrdenTrabajo, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion)
        m_adpOrdenTrabajo.Update(m_dstOrdenTrabajo, m_cnnSCGTaller, m_trnTransaccion, m_blnIniciarTransaccion)

        m_strNoOrden = p_strNoOT
        m_intEstCotizacion = CotizacionEstado.creada

    End Sub

    Private Function getOwnerName(ByVal p_intOwnerCode As Integer) As String
        Dim oEmployees As SAPbobsCOM.EmployeesInfo
        Dim strName As String
        Try

            oEmployees = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oEmployeesInfo), SAPbobsCOM.EmployeesInfo)
            If oEmployees.GetByKey(p_intOwnerCode) Then
                strName = oEmployees.FirstName.Trim & " " & oEmployees.LastName.Trim
                If strName.Length > 30 Then strName = strName.Substring(0, 30)
            End If
            Return strName

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        Finally
            Utilitarios.DestruirObjeto(oEmployees)
        End Try
    End Function

    Private Function CrearOrdenTrabajoSAP(ByVal p_strNoOT As String, ByVal p_strSucursal As String, ByVal p_strNoIniciada As String, ByVal p_intOwnerCode As Integer) As Boolean
        Try

            Dim UDOEncabezado As EncabezadoUDOOrden
            Dim m_dtConsulta As DataTable


            UDOEncabezado = New EncabezadoUDOOrden()

            UDOEncabezado.Code = p_strNoOT

            With UDOEncabezado

                .U_DocEntry = m_strDocEntry
                .U_NoOT = p_strNoOT
                .U_NoUni = m_strNumeroUnidad
                .U_NoCon = m_strCono
                .U_Ano = m_strAno
                .U_Plac = m_strPlaca
                .U_Marc = m_strDescMarca
                .U_Esti = m_strDescEstilo
                .U_Mode = m_strDescModelo
                .U_CMar = m_strCodeMarca
                .U_CEst = m_strCodeEstilo
                .U_CMod = m_strCodeModelo
                .U_NoVis = m_strNumeroVisita
                ' .U_EstVis
                .U_VIN = m_strVIN
                .U_km = CInt(m_dbkilometraje)
                .U_TipOT = m_intTipoOT
                ' .U_EstW = 
                .U_Sucu = p_strSucursal

                .U_CodCli = m_strCodigoCliente
                .U_NCli = m_strNombreCliente
                .U_CodCOT = m_strClienteOT
                .U_NCliOT = m_strNombreClienteOT
                '.U_Tel = ""
                '.U_Cor = ""
                .U_FRec = m_dtFechaRecepcion

                .U_HRec = m_dtHoraRecepcion

                .U_FCom = m_dtFechaCompromiso

                .U_HCom = m_dtHoraCompromiso

                .U_FApe = Date.Now

                .U_HApe = Date.Now

                .U_FFin = Nothing
                .U_HFin = Nothing
                .U_FCerr = Nothing
                .U_FFact = Nothing
                .U_FEntr = Nothing
                .U_OTRef = m_strOtReferencia
                .U_NGas = m_strOtNivelGas
                .U_HMot = m_intHorasMotor
                .U_EstO = "1"
                .U_DEstO = p_strNoIniciada
                .U_Ase = CStr(p_intOwnerCode)
                .U_EncO = ""
                .U_Obse = m_strObservaciones
                .U_NoCita = m_strSerieCompletaCita
            End With

            UDOOrden.Encabezado = UDOEncabezado
            UDOOrden.Company = m_oCompany
            'UDOOrden.Insert() //Se modifica para incluir dentro de la trassacion de insercion 
            m_intEstCotizacion = CotizacionEstado.creada

            strDocEntryOT = UDOOrden.Encabezado.Code
            Return True
            'Dim objType As Type
            'objType = UDOOrden.GetType()

            'Utilitarios.DestruirObjeto(UDOOrden)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        End Try
    End Function

    Private Function CreaCodeOT(ByVal p_strNumeroVisita As String, ByVal p_strNoOt As String) As String

        Dim m_strValores As String()
        Dim m_strValor As String
        Dim m_intCont As Integer = 0
        Try

            m_strValor = Utilitarios.EjecutarConsulta(" select MAX(DocEntry + 1) from [@SCGD_OT] with(nolock) ",
                                                        m_oCompany.CompanyDB, m_oCompany.Server)
            If String.IsNullOrEmpty(m_strValor) Then
                Return "1"
            Else
                Return m_strValor
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Function

    Private Function ActualizarOrdenTrabajoAgregar(ByRef p_oLineasCotizacion As SAPbobsCOM.Document_Lines, _
                                                   ByRef p_intEstadoPaquete As ArticuloAprobado, _
                                                   ByRef p_intCantidadLineasPaquete As Integer, _
                                                   ByRef p_intLineNumFather As Integer, _
                                                   ByVal p_intTipoArticulo As ArticuloAprobado, _
                                                   ByVal p_intCodFase As Integer) As Boolean

        Dim decCantidad As Decimal
        Dim strItemCode As String
        Dim intLineNum As Integer
        Dim decDuracion As Decimal
        Dim intItemAprobado As Integer
        Dim intEstadoTransf As Integer
        Dim blnYaAgregada As Boolean = False
        Dim intIDEmpleado As Integer
        Dim drwLineaActualizada As dtsMovimientoStock.LineaActualizadaRow
        Dim objUtilitarios As New SCGDataAccess.Utilitarios(strCadenaConexionBDTaller)
        Dim m_strConsultaArticulos As String = "  Select U_ItemCode from [@SCGD_ARTXESP] where U_ItemCode = '{0}' "
        Dim m_strFiltroMod As String = " and U_CodMod = '{1}' "
        Dim m_strFiltroEsti As String = " and U_CodEsti = '{1}' "
        Dim strConsultaArtExist As String
        Dim dtArtExis As System.Data.DataTable

        Dim blnLineaNueva As Boolean
        Dim strNameEspecifico As String
        Dim strCodeEspecifico As String

        Dim strDuracionActividad As String = String.Empty

        Dim strEsCompra As String = String.Empty
        Dim strCantPen As String = String.Empty
        Dim strCantSol As String = String.Empty
        Dim strCantRec As String = String.Empty

        decCantidad = p_oLineasCotizacion.Quantity
        strItemCode = p_oLineasCotizacion.ItemCode
        intLineNum = p_oLineasCotizacion.LineNum

        intItemAprobado = p_oLineasCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value
        intEstadoTransf = p_oLineasCotizacion.UserFields.Fields.Item(mc_strTrasladado).Value

        strCodeEspecifico = p_oLineasCotizacion.UserFields.Fields.Item(mc_strCodeEspecifico).Value
        strNameEspecifico = p_oLineasCotizacion.UserFields.Fields.Item(mc_strNameEspecifico).Value

        strEsCompra = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Compra").Value
        strEsCompra = strEsCompra.ToString.Trim()

        If ((intItemAprobado = ArticuloAprobado.scgSi AndAlso p_intCantidadLineasPaquete <= 0) _
            Or (intItemAprobado = ArticuloAprobado.scgSi AndAlso p_intTipoArticulo = TiposArticulos.scgPaquete)) _
            Or (p_intEstadoPaquete = ArticuloAprobado.scgSi AndAlso p_intCantidadLineasPaquete > 0) Then

            If p_intCantidadLineasPaquete <= 0 Then
                p_intLineNumFather = -1
            End If
            If p_intTipoArticulo <> TiposArticulos.scgNinguno Then
                Select Case p_intTipoArticulo

                    Case TiposArticulos.scgRepuesto

                        For Each m_drwRepuestos In m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows
                            If m_drwRepuestos.RowState <> DataRowState.Deleted Then
                                If m_drwRepuestos.NoRepuesto = strItemCode Then
                                    If p_oLineasCotizacion.LineNum = m_drwRepuestos.LineNum Then
                                        blnYaAgregada = True
                                        If p_oLineasCotizacion.Quantity <> m_drwRepuestos.Cantidad Then
                                            drwLineaActualizada = dtbLineasActualizadas.NewLineaActualizadaRow
                                            drwLineaActualizada.ItemCode = p_oLineasCotizacion.ItemCode
                                            drwLineaActualizada.ItemName = p_oLineasCotizacion.ItemDescription
                                            drwLineaActualizada.LineNum = p_oLineasCotizacion.LineNum
                                            drwLineaActualizada.Cantidad = p_oLineasCotizacion.Quantity - m_drwRepuestos.Cantidad
                                            dtbLineasActualizadas.AddLineaActualizadaRow(drwLineaActualizada)
                                            m_drwRepuestos.Cantidad = p_oLineasCotizacion.Quantity
                                            m_drwRepuestos.Trasladado = p_oLineasCotizacion.UserFields.Fields.Item(mc_strTrasladado).Value
                                            If p_oLineasCotizacion.UserFields.Fields.Item(mc_strCodeEspecifico).Value <> "" Then
                                                m_drwRepuestos.ItemCodeEspecifico = p_oLineasCotizacion.UserFields.Fields.Item(mc_strCodeEspecifico).Value
                                                If p_oLineasCotizacion.UserFields.Fields.Item(mc_strNameEspecifico).Value <> "" Then
                                                    m_drwRepuestos.ItemNameEspecifico = p_oLineasCotizacion.UserFields.Fields.Item(mc_strNameEspecifico).Value
                                                End If
                                            End If
                                            m_intEstCotizacion = CotizacionEstado.modificada
                                        End If
                                        Exit For
                                    End If

                                End If
                            End If
                        Next
                        If Not blnYaAgregada Then
                            Call AgregarRepuesto(strItemCode, decCantidad, intLineNum, intEstadoTransf, p_intLineNumFather, strCodeEspecifico, strNameEspecifico, TiposArticulos.scgRepuesto, strEsCompra)
                            m_intEstCotizacion = CotizacionEstado.modificada
                            blnLineaNueva = True
                        Else
                            blnLineaNueva = True
                            blnYaAgregada = False
                        End If

                    Case TiposArticulos.scgActividad

                        intIDEmpleado = IIf(IsNumeric(p_oLineasCotizacion.UserFields.Fields.Item(mc_strEmpRealiza).Value), p_oLineasCotizacion.UserFields.Fields.Item(mc_strEmpRealiza).Value, 0)
                        strDuracionActividad = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value
                        If Not String.IsNullOrEmpty(strDuracionActividad) Then
                            decDuracion = CType(strDuracionActividad, Decimal)
                        End If
                        For Each m_drwActividades In m_dstActividadesxOrden.SCGTA_TB_ActividadesxOrden.Rows
                            If m_drwActividades.RowState <> DataRowState.Deleted Then
                                If m_drwActividades.NoActividad = strItemCode AndAlso p_oLineasCotizacion.LineNum = m_drwActividades.LineNum Then
                                    If m_drwActividades.IDEmpleado <> intIDEmpleado Then

                                        m_drwActividades.IDEmpleado = intIDEmpleado
                                        m_intEstCotizacion = CotizacionEstado.modificada

                                    End If
                                    If m_drwActividades.Cantidad <> p_oLineasCotizacion.Quantity Then

                                        m_drwActividades.Cantidad = p_oLineasCotizacion.Quantity
                                        m_intEstCotizacion = CotizacionEstado.modificada

                                    End If
                                    If m_drwActividades.Duracion <> decDuracion Then

                                        m_drwActividades.Duracion = decDuracion
                                        m_intEstCotizacion = CotizacionEstado.modificada

                                    End If
                                    blnYaAgregada = True
                                    'Exit For

                                End If
                            End If
                        Next

                        If Not blnYaAgregada Then

                            'Usa la configuracion de asociación de artículos por estilo. Carlos Céspedes
                            If m_UsaAsocxEspc.Equals("Y") Then
                                If m_UsaFilSerEspeci.Equals("Y") Then

                                    'Dim strConsultaEspV As String = "Select U_EspVehic from [@SCGD_ADMIN]"
                                    Dim strConsulta As String
                                    Dim strDuracion As String = ""

                                    If m_EspecifVehi.Equals("E") Then
                                        Dim strCodEstilo As String = m_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value
                                        strConsultaArtExist = m_strConsultaArticulos & m_strFiltroEsti

                                        dtArtExis = Utilitarios.EjecutarConsultaDataTable(String.Format(strConsultaArtExist, strItemCode, strCodEstilo), m_oCompany.CompanyDB, m_oCompany.Server)

                                        If dtArtExis.Rows.Count > 0 Then
                                            strConsulta = String.Format("Select U_Duracion from [@SCGD_ARTXESP] where U_ItemCode = '{0}' and U_CodEsti = '{1}'", strItemCode, strCodEstilo)
                                            strDuracion = Utilitarios.EjecutarConsulta(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)
                                        Else
                                            strDuracionActividad = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value

                                            If Not String.IsNullOrEmpty(strDuracionActividad) Then
                                                decDuracion = CType(strDuracionActividad, Decimal)
                                            End If
                                        End If



                                    ElseIf m_EspecifVehi.Equals("M") Then

                                        Dim strCodModelo As String = m_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value

                                        strConsultaArtExist = m_strConsultaArticulos & m_strFiltroMod


                                        dtArtExis = Utilitarios.EjecutarConsultaDataTable(String.Format(strConsultaArtExist, strItemCode, strCodModelo), m_oCompany.CompanyDB, m_oCompany.Server)

                                        If dtArtExis.Rows.Count > 0 Then
                                            strConsulta = String.Format("Select U_Duracion from [@SCGD_ARTXESP] where U_ItemCode = '{0}' and U_CodMod = '{1}'", strItemCode, strCodModelo)
                                            strDuracion = Utilitarios.EjecutarConsulta(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)
                                        Else
                                            strDuracionActividad = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value

                                            If Not String.IsNullOrEmpty(strDuracionActividad) Then
                                                decDuracion = CType(strDuracionActividad, Decimal)
                                            End If
                                        End If


                                    End If

                                    If Not String.IsNullOrEmpty(strDuracion) Then
                                        decDuracion = CType(strDuracion, Decimal)
                                    Else
                                        decDuracion = 0
                                    End If
                                Else

                                    strDuracionActividad = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value

                                    If Not String.IsNullOrEmpty(strDuracionActividad) Then
                                        decDuracion = CType(strDuracionActividad, Decimal)
                                    End If

                                End If

                            Else

                                strDuracionActividad = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value

                                If Not String.IsNullOrEmpty(strDuracionActividad) Then
                                    decDuracion = CType(strDuracionActividad, Decimal)
                                End If

                            End If

                            p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value = decDuracion.ToString()

                            Call AgregarActividades(strItemCode, decCantidad, p_intCodFase, decDuracion, intLineNum, intIDEmpleado, p_intLineNumFather)
                            m_intEstCotizacion = CotizacionEstado.modificada
                            blnLineaNueva = True

                        Else
                            blnYaAgregada = False
                            blnLineaNueva = True
                        End If

                    Case TiposArticulos.scgSuministro
                        For Each m_drwSuministros In m_dstSuministrosxOrden.SCGTA_VW_Suministros.Rows
                            If m_drwSuministros.RowState <> DataRowState.Deleted Then
                                If m_drwSuministros.NoSuministro = strItemCode Then
                                    If p_oLineasCotizacion.LineNum = m_drwSuministros.LineNum Then
                                        blnYaAgregada = True
                                        If p_oLineasCotizacion.Quantity <> m_drwSuministros.Cantidad Then
                                            drwLineaActualizada = dtbLineasActualizadas.NewLineaActualizadaRow
                                            drwLineaActualizada.ItemCode = p_oLineasCotizacion.ItemCode
                                            drwLineaActualizada.ItemName = p_oLineasCotizacion.ItemDescription
                                            drwLineaActualizada.LineNum = p_oLineasCotizacion.LineNum
                                            drwLineaActualizada.Cantidad = p_oLineasCotizacion.Quantity - m_drwSuministros.Cantidad
                                            dtbLineasActualizadas.AddLineaActualizadaRow(drwLineaActualizada)
                                            m_drwSuministros.Cantidad = p_oLineasCotizacion.Quantity
                                            m_intEstCotizacion = CotizacionEstado.modificada
                                        End If
                                        Exit For
                                    End If

                                End If
                            End If
                        Next

                        If Not blnYaAgregada Then

                            Call AgregarSuministro(strItemCode, decCantidad, intLineNum, p_intLineNumFather)
                            m_intEstCotizacion = CotizacionEstado.modificada
                            blnLineaNueva = True

                        Else
                            blnLineaNueva = True
                            blnYaAgregada = False

                        End If

                    Case TiposArticulos.scgServicioExt

                        For Each m_drwRepuestos In m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows
                            If m_drwRepuestos.RowState <> DataRowState.Deleted Then
                                If m_drwRepuestos.NoRepuesto = strItemCode Then
                                    If p_oLineasCotizacion.LineNum = m_drwRepuestos.LineNum Then
                                        blnYaAgregada = True
                                        If p_oLineasCotizacion.Quantity <> m_drwRepuestos.Cantidad Then
                                            drwLineaActualizada = dtbLineasActualizadas.NewLineaActualizadaRow
                                            drwLineaActualizada.ItemCode = p_oLineasCotizacion.ItemCode
                                            drwLineaActualizada.ItemName = p_oLineasCotizacion.ItemDescription
                                            drwLineaActualizada.LineNum = p_oLineasCotizacion.LineNum
                                            drwLineaActualizada.Cantidad = p_oLineasCotizacion.Quantity - m_drwRepuestos.Cantidad
                                            dtbLineasActualizadas.AddLineaActualizadaRow(drwLineaActualizada)
                                            m_drwRepuestos.Cantidad = p_oLineasCotizacion.Quantity
                                            m_intEstCotizacion = CotizacionEstado.modificada
                                        End If
                                        Exit For

                                    End If
                                End If
                            End If
                        Next
                        If Not blnYaAgregada Then

                            Call AgregarRepuesto(strItemCode, decCantidad, intLineNum, intEstadoTransf, p_intLineNumFather, strCodeEspecifico, strNameEspecifico, TiposArticulos.scgServicioExt, strEsCompra)
                            m_intEstCotizacion = CotizacionEstado.modificada
                            p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPen").Value = p_oLineasCotizacion.Quantity
                            blnLineaNueva = True
                        Else
                            strCantPen = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPen").Value
                            If String.IsNullOrEmpty(strCantPen) Then
                                strCantPen = 0
                            End If
                            strCantSol = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CSol").Value
                            If String.IsNullOrEmpty(strCantSol) Then
                                strCantSol = 0
                            End If
                            strCantRec = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value
                            If String.IsNullOrEmpty(strCantRec) Then
                                strCantRec = 0
                            End If

                            Dim dblCPen As Double
                            Dim dblCSol As Double
                            Dim dblCRec As Double

                            dblCPen = Double.Parse(strCantPen.ToString(n))
                            dblCSol = Double.Parse(strCantSol.ToString(n))
                            dblCRec = Double.Parse(strCantRec.ToString(n))

                            If dblCSol = 0 AndAlso dblCRec = 0 AndAlso p_oLineasCotizacion.Quantity <> dblCPen Then
                                dblCPen = p_oLineasCotizacion.Quantity
                                p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPen").Value = dblCPen
                            End If

                            'Dim decCantPen As Decimal = Decimal.Parse(strCantPen)
                            'Dim decCantSol As Decimal = Decimal.Parse(strCantSol)
                            'Dim decCantRec As Decimal = Decimal.Parse(strCantRec)

                            If strCantPen = "0" And strCantSol = "0" And strCantRec = "0" Then
                                p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPen").Value = p_oLineasCotizacion.Quantity
                            End If

                            blnLineaNueva = True
                            blnYaAgregada = False
                        End If

                    Case TiposArticulos.scgPaquete
                        p_intCantidadLineasPaquete = objUtilitarios.CantidadLineasPaquetes(strItemCode)
                        p_intEstadoPaquete = ArticuloAprobado.scgSi
                        p_intLineNumFather = p_oLineasCotizacion.LineNum
                        blnLineaNueva = True
                    Case Else

                End Select
            Else

                SBO_Application.MessageBox("El artículo " + strItemCode + My.Resources.Resource.MalConfigurado)

            End If
        Else
            If p_intTipoArticulo = TiposArticulos.scgPaquete Then

                p_intEstadoPaquete = intItemAprobado

                p_intCantidadLineasPaquete = objUtilitarios.CantidadLineasPaquetes(strItemCode)
            Else
                If p_intCantidadLineasPaquete > 0 Then
                    p_intCantidadLineasPaquete -= 1
                End If
                blnLineaNueva = False
            End If
            p_oLineasCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo
        End If

        Return blnLineaNueva

    End Function
    Private Function ActualizarOrdenTrabajoAgregar_OT_SAP(ByRef p_oLineasCotizacion As SAPbobsCOM.Document_Lines, _
                                                   ByRef p_intEstadoPaquete As ArticuloAprobado, _
                                                   ByRef p_intCantidadLineasPaquete As Integer, _
                                                   ByRef p_intLineNumFather As Integer, _
                                                   ByVal p_intTipoArticulo As ArticuloAprobado, _
                                                   ByVal p_intCodFase As Integer) As Boolean

        Dim decCantidad As Decimal
        Dim strItemCode As String
        Dim intLineNum As Integer
        Dim decDuracion As Decimal
        Dim intItemAprobado As Integer
        Dim intEstadoTransf As Integer
        Dim blnYaAgregada As Boolean = False
        Dim intIDEmpleado As Integer
        Dim drwLineaActualizada As dtsMovimientoStock.LineaActualizadaRow
        Dim objUtilitarios As New SCGDataAccess.Utilitarios(strCadenaConexionBDTaller)


        Dim blnLineaNueva As Boolean
        Dim strNameEspecifico As String
        Dim strCodeEspecifico As String
        Dim m_strConsultaArticulos As String = "  Select U_ItemCode from [@SCGD_ARTXESP] where U_ItemCode = '{0}' "
        Dim m_strFiltroMod As String = " and U_CodMod = '{1}' "
        Dim m_strFiltroEsti As String = " and U_CodEsti = '{1}' "
        Dim strConsultaArtExist As String
        Dim dtArtExis As System.Data.DataTable

        Dim strDuracionActividad As String = String.Empty

        Dim strEsCompra As String = String.Empty
        Dim strCantPen As String = String.Empty
        Dim strCantSol As String = String.Empty
        Dim strCantRec As String = String.Empty

        decCantidad = p_oLineasCotizacion.Quantity
        strItemCode = p_oLineasCotizacion.ItemCode
        intLineNum = p_oLineasCotizacion.LineNum

        intItemAprobado = p_oLineasCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value
        intEstadoTransf = p_oLineasCotizacion.UserFields.Fields.Item(mc_strTrasladado).Value

        strCodeEspecifico = p_oLineasCotizacion.UserFields.Fields.Item(mc_strCodeEspecifico).Value
        strNameEspecifico = p_oLineasCotizacion.UserFields.Fields.Item(mc_strNameEspecifico).Value

        strEsCompra = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Compra").Value
        strEsCompra = strEsCompra.ToString.Trim()

        If ((intItemAprobado = ArticuloAprobado.scgSi AndAlso p_intCantidadLineasPaquete <= 0) _
            Or (intItemAprobado = ArticuloAprobado.scgSi AndAlso p_intTipoArticulo = TiposArticulos.scgPaquete)) _
            Or (p_intEstadoPaquete = ArticuloAprobado.scgSi AndAlso p_intCantidadLineasPaquete > 0) Then

            If p_intCantidadLineasPaquete <= 0 Then
                p_intLineNumFather = -1
            End If
            If p_intTipoArticulo <> TiposArticulos.scgNinguno Then
                Select Case p_intTipoArticulo

                    Case TiposArticulos.scgRepuesto
                        Dim blnCantidadModificadaRep As Boolean = False
                        For intLineaCotizacionAnterior As Integer = 0 To oCotizacionlocal.Lines.Count - 1
                            oCotizacionlocal.Lines.SetCurrentLine(intLineaCotizacionAnterior)
                            If (oCotizacionlocal.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = "2" OrElse oCotizacionlocal.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = "3") AndAlso p_oLineasCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value = "1" Then
                                Exit For
                            Else
                                If p_oLineasCotizacion.ItemCode = oCotizacionlocal.Lines.ItemCode Then
                                    If p_oLineasCotizacion.LineNum = oCotizacionlocal.Lines.LineNum Then
                                        blnYaAgregada = True
                                        If p_oLineasCotizacion.Quantity <> oCotizacionlocal.Lines.Quantity Then
                                            m_intEstCotizacion = CotizacionEstado.modificada
                                            blnCantidadModificadaRep = True
                                        End If
                                        Exit For
                                    End If
                                End If
                            End If
                        Next

                        If Not blnYaAgregada Then
                            m_intEstCotizacion = CotizacionEstado.modificada
                            blnLineaNueva = True
                        Else
                            If blnCantidadModificadaRep Then
                                Call CambiarEstadoColumnasOT_SAP(p_oLineasCotizacion, blnCantidadModificadaRep)
                            End If

                            blnLineaNueva = True
                            blnYaAgregada = False
                        End If


                    Case TiposArticulos.scgActividad

                        intIDEmpleado = IIf(IsNumeric(p_oLineasCotizacion.UserFields.Fields.Item(mc_strEmpRealiza).Value), p_oLineasCotizacion.UserFields.Fields.Item(mc_strEmpRealiza).Value, 0)
                        strDuracionActividad = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value
                        If Not String.IsNullOrEmpty(strDuracionActividad) Then
                            decDuracion = CType(strDuracionActividad, Decimal)
                        End If

                        For intLineaCotizacionAnterior As Integer = 0 To oCotizacionlocal.Lines.Count - 1
                            oCotizacionlocal.Lines.SetCurrentLine(intLineaCotizacionAnterior)
                            If p_oLineasCotizacion.ItemCode = oCotizacionlocal.Lines.ItemCode Then
                                If p_oLineasCotizacion.LineNum = oCotizacionlocal.Lines.LineNum Then
                                    Dim intIDEmpleadoCotizacionAnterior As Integer = IIf(IsNumeric(oCotizacionlocal.Lines.UserFields.Fields.Item(mc_strEmpRealiza).Value), oCotizacionlocal.Lines.UserFields.Fields.Item(mc_strEmpRealiza).Value, 0)
                                    If intIDEmpleado <> intIDEmpleadoCotizacionAnterior Then
                                        m_intEstCotizacion = CotizacionEstado.modificada
                                    End If

                                    If p_oLineasCotizacion.Quantity <> oCotizacionlocal.Lines.Quantity Then
                                        m_intEstCotizacion = CotizacionEstado.modificada
                                    End If

                                    Dim strDuracionActividadCotizacionAnterior As String = oCotizacionlocal.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value
                                    Dim decDuracionCotizacionAnterior As Decimal = 0
                                    If Not String.IsNullOrEmpty(strDuracionActividadCotizacionAnterior) Then
                                        decDuracionCotizacionAnterior = CType(strDuracionActividadCotizacionAnterior, Decimal)
                                    End If

                                    If decDuracion <> decDuracionCotizacionAnterior Then
                                        m_intEstCotizacion = CotizacionEstado.modificada
                                    End If

                                    blnYaAgregada = True
                                End If
                            End If

                        Next

                        If Not blnYaAgregada Then

                            'Usa la configuracion de asociación de artículos por estilo. Carlos Céspedes
                            'Usa la configuracion de asociación de artículos por estilo. Carlos Céspedes
                            If m_UsaAsocxEspc.Equals("Y") Then

                                If m_UsaFilSerEspeci.Equals("Y") Then

                                    'Dim strConsultaEspV As String = "Select U_EspVehic from [@SCGD_ADMIN]"
                                    Dim strConsulta As String
                                    Dim strDuracion As String = ""

                                    If m_EspecifVehi.Equals("E") Then
                                        Dim strCodEstilo As String = m_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value
                                        strConsultaArtExist = m_strConsultaArticulos & m_strFiltroEsti

                                        dtArtExis = Utilitarios.EjecutarConsultaDataTable(String.Format(strConsultaArtExist, strItemCode, strCodEstilo), m_oCompany.CompanyDB, m_oCompany.Server)

                                        If dtArtExis.Rows.Count > 0 Then
                                            strConsulta = String.Format("Select U_Duracion from [@SCGD_ARTXESP] where U_ItemCode = '{0}' and U_CodEsti = '{1}'", strItemCode, strCodEstilo)
                                            strDuracion = Utilitarios.EjecutarConsulta(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)
                                        Else
                                            strDuracionActividad = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value

                                            If Not String.IsNullOrEmpty(strDuracionActividad) Then
                                                decDuracion = CType(strDuracionActividad, Decimal)
                                            End If
                                        End If
                                    ElseIf m_EspecifVehi.Equals("M") Then
                                        Dim strCodModelo As String = m_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value

                                        strConsultaArtExist = m_strConsultaArticulos & m_strFiltroMod
                                        dtArtExis = Utilitarios.EjecutarConsultaDataTable(String.Format(strConsultaArtExist, strItemCode, strCodModelo), m_oCompany.CompanyDB, m_oCompany.Server)

                                        If dtArtExis.Rows.Count > 0 Then
                                            strConsulta = String.Format("Select U_Duracion from [@SCGD_ARTXESP] where U_ItemCode = '{0}' and U_CodMod = '{1}'", strItemCode, strCodModelo)
                                            strDuracion = Utilitarios.EjecutarConsulta(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)
                                        Else
                                            strDuracionActividad = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value

                                            If Not String.IsNullOrEmpty(strDuracionActividad) Then
                                                decDuracion = CType(strDuracionActividad, Integer)
                                            End If
                                        End If


                                    End If

                                    If Not String.IsNullOrEmpty(strDuracion) Then
                                        decDuracion = CType(strDuracion, Integer)
                                    Else
                                        decDuracion = 0
                                    End If
                                Else

                                    strDuracionActividad = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value

                                    If Not String.IsNullOrEmpty(strDuracionActividad) Then
                                        decDuracion = CType(strDuracionActividad, Integer)
                                    End If

                                End If



                            Else

                                strDuracionActividad = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value

                                If Not String.IsNullOrEmpty(strDuracionActividad) Then
                                    decDuracion = CType(strDuracionActividad, Decimal)
                                End If

                            End If

                            p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value = decDuracion.ToString()

                            Call AgregarActividades(strItemCode, decCantidad, p_intCodFase, decDuracion, intLineNum, intIDEmpleado, p_intLineNumFather)
                            m_intEstCotizacion = CotizacionEstado.modificada
                            blnLineaNueva = True
                        Else
                            blnYaAgregada = False
                            blnLineaNueva = True
                        End If

                    Case TiposArticulos.scgSuministro
                        Dim blnCantidadModificadaSum As Boolean = False
                        For intLineaCotizacionAnterior As Integer = 0 To oCotizacionlocal.Lines.Count - 1
                            oCotizacionlocal.Lines.SetCurrentLine(intLineaCotizacionAnterior)
                            If (oCotizacionlocal.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = "2" OrElse oCotizacionlocal.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = "3") AndAlso p_oLineasCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value = "1" Then
                                Exit For
                            Else
                                If p_oLineasCotizacion.ItemCode = oCotizacionlocal.Lines.ItemCode Then
                                    If p_oLineasCotizacion.LineNum = oCotizacionlocal.Lines.LineNum Then
                                        blnYaAgregada = True
                                        If p_oLineasCotizacion.Quantity <> oCotizacionlocal.Lines.Quantity Then
                                            m_intEstCotizacion = CotizacionEstado.modificada
                                            blnCantidadModificadaSum = True

                                        End If
                                        Exit For
                                    End If
                                End If
                            End If
                        Next

                        If Not blnYaAgregada Then
                            m_intEstCotizacion = CotizacionEstado.modificada
                            blnLineaNueva = True
                        Else
                            If blnCantidadModificadaSum Then
                                Call CambiarEstadoColumnasOT_SAP(p_oLineasCotizacion, blnCantidadModificadaSum)
                            End If

                            blnLineaNueva = True
                            blnYaAgregada = False
                        End If

                    Case TiposArticulos.scgServicioExt

                        For intLineaCotizacionAnterior As Integer = 0 To oCotizacionlocal.Lines.Count - 1
                            oCotizacionlocal.Lines.SetCurrentLine(intLineaCotizacionAnterior)
                            If p_oLineasCotizacion.ItemCode = oCotizacionlocal.Lines.ItemCode Then
                                If p_oLineasCotizacion.LineNum = oCotizacionlocal.Lines.LineNum Then
                                    blnYaAgregada = True
                                    If p_oLineasCotizacion.Quantity <> oCotizacionlocal.Lines.Quantity Then
                                        m_intEstCotizacion = CotizacionEstado.modificada
                                    End If
                                    Exit For
                                End If
                            End If
                        Next
                        If Not blnYaAgregada Then
                            m_intEstCotizacion = CotizacionEstado.modificada
                            p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPen").Value = p_oLineasCotizacion.Quantity
                            blnLineaNueva = True
                        Else

                            Call CambiarEstadoColumnasOT_SAP(p_oLineasCotizacion)

                            blnLineaNueva = True
                            blnYaAgregada = False
                        End If

                    Case TiposArticulos.scgPaquete
                        p_intCantidadLineasPaquete = objUtilitarios.CantidadLineasPaquetes(strItemCode)
                        p_intEstadoPaquete = ArticuloAprobado.scgSi
                        p_intLineNumFather = p_oLineasCotizacion.LineNum
                        blnLineaNueva = True
                    Case Else

                End Select
            Else

                SBO_Application.MessageBox("El artículo " + strItemCode + My.Resources.Resource.MalConfigurado)

            End If
        Else
            If p_intTipoArticulo = TiposArticulos.scgPaquete Then

                p_intEstadoPaquete = intItemAprobado

                p_intCantidadLineasPaquete = objUtilitarios.CantidadLineasPaquetes(strItemCode)
            Else
                If p_intCantidadLineasPaquete > 0 Then
                    p_intCantidadLineasPaquete -= 1
                End If
                blnLineaNueva = False
            End If
            p_oLineasCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo
        End If

        Return blnLineaNueva

    End Function

    Public Sub CambiarEstadoColumnasOT_SAP(ByRef p_oLineasCotizacion As SAPbobsCOM.Document_Lines, Optional ByVal p_blnCantidadModificada As Boolean = False)

        Dim dblCPen As Double
        Dim dblCSol As Double
        Dim dblCRec As Double
        Dim dblCPenDev As Double
        Dim dblCPenBod As Double
        Dim strTipoArticulo As String = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_TipArt").Value

        dblCPen = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPen").Value
        dblCSol = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CSol").Value
        dblCRec = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value
        dblCPenDev = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPDe").Value
        dblCPenBod = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPBo").Value

        Select Case strTipoArticulo

            Case "1"

                If p_blnCantidadModificada Then
                    p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value = p_oLineasCotizacion.Quantity
                    p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPDe").Value = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPDe").Value + (dblCRec - p_oLineasCotizacion.Quantity)
                End If


            Case "3"
                If p_blnCantidadModificada Then
                    p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value = p_oLineasCotizacion.Quantity
                    p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPDe").Value = p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPDe").Value + (dblCRec - p_oLineasCotizacion.Quantity)
                End If
            Case "4"

                If dblCSol = 0 AndAlso dblCRec = 0 AndAlso p_oLineasCotizacion.Quantity <> dblCPen Then
                    dblCPen = p_oLineasCotizacion.Quantity
                    p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPen").Value = dblCPen
                End If

                If dblCPen = 0 And dblCSol = 0 And dblCRec = 0 Then
                    p_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPen").Value = p_oLineasCotizacion.Quantity
                End If


        End Select

    End Sub


    Private Function ActualizarOrdenTrabajoEliminar(ByVal p_oLineaCotizacion As SAPbobsCOM.Document_Lines, _
                                                    ByRef p_intEstadoPaquete As ArticuloAprobado, _
                                                    ByVal p_intTipoArticulo As TiposArticulos) As Boolean

        Dim blnSiEsta As Boolean = False
        Dim blnFaltaAprobacion As Boolean = False
        Dim blnItemEliminado As Boolean = False

        Select Case p_intTipoArticulo

            Case TiposArticulos.scgRepuesto

                For Each m_drwRepuestos In m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows
                    blnSiEsta = False
                    If m_drwRepuestos.RowState <> DataRowState.Deleted Then
                        If m_drwRepuestos.NoRepuesto = p_oLineaCotizacion.ItemCode Then

                            If m_drwRepuestos.LineNum = p_oLineaCotizacion.LineNum Then

                                If m_drwRepuestos.LineNumFather <> -1 Then
                                    Select Case p_intEstadoPaquete

                                        Case ArticuloAprobado.scgSi
                                            blnSiEsta = True
                                            Exit For

                                        Case ArticuloAprobado.scgFalta
                                            blnSiEsta = True
                                            blnFaltaAprobacion = True
                                            Exit For

                                    End Select
                                ElseIf p_oLineaCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi Then

                                    blnSiEsta = True
                                    Exit For

                                ElseIf p_oLineaCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgFalta Then

                                    blnSiEsta = True
                                    blnFaltaAprobacion = True
                                    Exit For

                                End If
                                Exit For

                            End If

                        End If

                    End If
                Next
                If Not blnSiEsta Then
                    If m_drwRepuestos IsNot Nothing Then
                        m_drwRepuestos.Delete()
                        m_intRealizarTraslados = enumRealizarTraslados.scgSi
                        m_intEstCotizacion = CotizacionEstado.modificada
                        blnItemEliminado = True
                    End If
                Else
                    blnSiEsta = False

                End If

            Case TiposArticulos.scgServicioExt

                'Repuestos
                For Each m_drwRepuestos In m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows
                    blnSiEsta = False
                    If m_drwRepuestos.RowState <> DataRowState.Deleted Then
                        If m_drwRepuestos.NoRepuesto = p_oLineaCotizacion.ItemCode Then

                            If m_drwRepuestos.LineNum = p_oLineaCotizacion.LineNum Then

                                If m_drwRepuestos.LineNumFather <> -1 Then
                                    Select Case p_intEstadoPaquete

                                        Case ArticuloAprobado.scgSi
                                            blnSiEsta = True
                                            Exit For

                                        Case ArticuloAprobado.scgFalta
                                            blnSiEsta = True
                                            blnFaltaAprobacion = True
                                            Exit For

                                    End Select
                                ElseIf p_oLineaCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi Then

                                    blnSiEsta = True
                                    Exit For

                                ElseIf p_oLineaCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgFalta Then

                                    blnSiEsta = True
                                    blnFaltaAprobacion = True
                                    Exit For

                                End If
                                Exit For
                            Else
                                blnSiEsta = True
                            End If
                        Else
                            blnSiEsta = True
                        End If

                    End If
                Next
                If Not blnSiEsta Then
                    If m_drwRepuestos IsNot Nothing Then
                        m_drwRepuestos.Delete()
                        m_intRealizarTraslados = enumRealizarTraslados.scgSi
                        m_intEstCotizacion = CotizacionEstado.modificada
                        blnItemEliminado = True
                    End If
                Else
                    blnSiEsta = False

                End If

            Case TiposArticulos.scgActividad

                'Servicios
                For Each m_drwActividades In m_dstActividadesxOrden.SCGTA_TB_ActividadesxOrden.Rows
                    blnSiEsta = False
                    If m_drwActividades.RowState <> DataRowState.Deleted Then
                        If m_drwActividades.NoActividad = p_oLineaCotizacion.ItemCode Then

                            If m_drwActividades.LineNum = p_oLineaCotizacion.LineNum Then

                                If m_drwActividades.LineNumFather <> -1 Then
                                    Select Case p_intEstadoPaquete

                                        Case ArticuloAprobado.scgSi
                                            blnSiEsta = True
                                            Exit For

                                        Case ArticuloAprobado.scgFalta
                                            blnSiEsta = True
                                            blnFaltaAprobacion = True
                                            Exit For

                                    End Select
                                ElseIf p_oLineaCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi Then

                                    blnSiEsta = True
                                    Exit For

                                ElseIf p_oLineaCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgFalta Then

                                    blnSiEsta = True
                                    blnFaltaAprobacion = True
                                    Exit For

                                End If
                                Exit For
                            Else
                                blnSiEsta = True
                            End If
                        Else
                            blnSiEsta = True
                        End If
                    End If
                Next

                If Not blnSiEsta Then
                    If m_drwActividades IsNot Nothing Then
                        m_drwActividades.Delete()
                        m_intEstCotizacion = CotizacionEstado.modificada
                        blnItemEliminado = True
                    End If
                Else
                    blnSiEsta = False

                End If

            Case TiposArticulos.scgSuministro

                'Suministros
                For Each m_drwSuministros In m_dstSuministrosxOrden.SCGTA_VW_Suministros.Rows
                    blnSiEsta = False
                    If m_drwSuministros.RowState <> DataRowState.Deleted Then
                        If m_drwSuministros.NoSuministro = p_oLineaCotizacion.ItemCode Then

                            If m_drwSuministros.LineNum = p_oLineaCotizacion.LineNum Then

                                If m_drwSuministros.LineNumFather <> -1 Then
                                    Select Case p_intEstadoPaquete

                                        Case ArticuloAprobado.scgSi
                                            blnSiEsta = True
                                            Exit For

                                        Case ArticuloAprobado.scgFalta
                                            blnSiEsta = True
                                            blnFaltaAprobacion = True
                                            Exit For

                                    End Select
                                ElseIf p_oLineaCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi Then

                                    blnSiEsta = True
                                    Exit For

                                ElseIf p_oLineaCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgFalta Then

                                    blnSiEsta = True
                                    blnFaltaAprobacion = True
                                    Exit For

                                End If
                                Exit For
                            Else
                                blnSiEsta = True
                            End If
                        Else
                            blnSiEsta = True
                        End If
                    End If
                Next

                If Not blnSiEsta Then
                    If m_drwSuministros IsNot Nothing Then
                        m_drwSuministros.Delete()
                        m_intRealizarTraslados = enumRealizarTraslados.scgSi
                        m_intEstCotizacion = CotizacionEstado.modificada
                        blnItemEliminado = True
                    End If

                Else
                    blnSiEsta = False
                End If

        End Select

        Return blnItemEliminado

    End Function

    Private Sub AgregarRepuesto(ByVal NoRepuesto As String, _
                                ByVal decCantidad As Double, _
                                ByVal intLineNum As Integer, _
                                ByVal intTransf As Integer, _
                                ByVal p_intLineNumFather As Integer, _
                                ByVal p_strCodeEspecifico As String, _
                                ByVal p_strNameEspecifico As String, _
                                ByVal p_intTipo As Integer,
                                Optional ByVal p_strEsCompra As String = "")
        Try
            Dim drwRepuesto As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow

            drwRepuesto = dtbRepuestosxOrden.NewSCGTA_TB_RepuestosxOrdenRow

            With drwRepuesto

                .NoOrden = m_strNoOrden
                .NoRepuesto = NoRepuesto
                .Cantidad = decCantidad
                .Adicional = 0
                .TipoArticulo = p_intTipo
                .LineNum = intLineNum
                .EstadoTransf = intTransf
                .LineNumFather = p_intLineNumFather

                Dim intLineNumOriginal As Integer = intLineNum

                .LineNumOriginal = intLineNumOriginal

                If p_strCodeEspecifico = String.Empty Then

                    .IsItemCodeEspecificoNull()
                Else

                    .ItemCodeEspecifico = p_strCodeEspecifico
                End If

                .Itemname = p_strNameEspecifico

                If p_intTipo = 4 Then
                    .Compra = "Y"
                Else
                    If Not String.IsNullOrEmpty(p_strEsCompra) Then
                        .Compra = p_strEsCompra
                    Else
                        .Compra = "N"
                    End If
                End If


            End With

            Call dtbRepuestosxOrden.AddSCGTA_TB_RepuestosxOrdenRow(drwRepuesto)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex

        End Try

    End Sub

    Private Sub AgregarSuministro(ByVal NoSuministro As String, _
                                ByVal decCantidad As Double, _
                                ByVal intLineNum As Integer, _
                                ByVal p_intLineNumFather As Integer)
        Try
            Dim drwSuministro As SuministrosDataset.SCGTA_VW_SuministrosRow

            drwSuministro = dtbSuministrosxOrden.NewSCGTA_VW_SuministrosRow

            With drwSuministro

                .NoOrden = m_strNoOrden
                .NoSuministro = NoSuministro
                .Cantidad = decCantidad
                .Adicional = 0
                .LineNum = intLineNum
                .TipoArticulo = 3
                .LineNumFather = p_intLineNumFather

                Dim intLineNumOriginal As Integer = intLineNum
                .LineNumOriginal = intLineNumOriginal
            End With

            Call dtbSuministrosxOrden.AddSCGTA_VW_SuministrosRow(drwSuministro)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex

        End Try

    End Sub

    Private Sub AgregarActividades(ByVal p_strNoActividad As String, _
                                ByVal p_intCantidad As Double, _
                                ByVal p_intNoFase As Integer, _
                                ByVal p_decDuracion As Decimal, _
                                ByVal p_intLineNum As Integer, _
                                ByVal p_intIDEmpleado As Integer, _
                                ByVal p_intLineNumFather As Integer)
        Try
            Dim drwActividad As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
            Dim blnYaAgregada As Boolean = False

            For Each drwActividad In dtbActividadesXOrden
                If drwActividad.NoActividad = p_strNoActividad AndAlso drwActividad.LineNum = p_intLineNum Then
                    drwActividad.Cantidad = drwActividad.Cantidad + p_intCantidad
                    If drwActividad.IDEmpleado = 0 Then
                        drwActividad.IDEmpleado = p_intIDEmpleado
                    End If
                    blnYaAgregada = True
                    Exit For
                End If
            Next
            If Not blnYaAgregada Then
                drwActividad = Nothing
                drwActividad = dtbActividadesXOrden.NewSCGTA_TB_ActividadesxOrdenRow

                With drwActividad

                    .NoOrden = m_strNoOrden
                    .NoActividad = p_strNoActividad
                    .Adicional = 0
                    .NoFase = p_intNoFase
                    .Duracion = p_decDuracion
                    .LineNum = p_intLineNum
                    .Cantidad = p_intCantidad
                    .IDEmpleado = p_intIDEmpleado
                    .LineNumFather = p_intLineNumFather

                End With

                Call dtbActividadesXOrden.AddSCGTA_TB_ActividadesxOrdenRow(drwActividad)
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex

        End Try

    End Sub

    Private Sub LimpiarVariables()

        m_strNombreCliente = ""
        m_strEmpleadoRecibe = ""
        m_intGenerarOT = 0
        m_intTipoOT = 0
        m_strEstadoCotizacion = ""
        m_strNumeroVisita = ""
        m_strNumeroUnidad = ""
        m_strNumeroVehiculo = ""
        m_strNumeroOT = ""
        m_strNoOrden = ""
        dtbActividadesXOrden = New ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable
        dtbRepuestosxOrden = New RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable
        dtbSuministrosxOrden = New SuministrosDataset.SCGTA_VW_SuministrosDataTable

    End Sub

    Private Sub EnviarMensaje(ByVal p_strDocEntryTrasfElimRep As String, _
                              ByVal p_strDocEntryTrasfRep As String, _
                              ByVal p_strDocEntryTrasfSum As String, _
                              ByVal p_strDocEntryTrasfElimSum As String, _
                              ByVal p_blnConf_TallerEnSAP As Boolean, _
                              Optional ByVal p_TipoMensaje As String = "", _
                              Optional ByVal p_oForm As SAPbouiCOM.Form = Nothing)

        Dim clsUtilitarios As New Utilitarios
        Dim dtConsulta As DataTable
        Dim strConsultaRequisiciones As String = " select U_Requis from [@SCGD_CONF_SUCURSAL] with(nolock) where U_Sucurs = '{0}' "
        Dim strConsultaAsesor As String = " Select T1.firstName + ' ' + T1.lastName  From [OQUT] T0 ,[OHEM] T1 Where T0.[OwnerCode] = T1.[empID] and T0.U_SCGD_Numero_OT = '{0}' "
        Dim strConsultaRequisicionesForm As String = String.Empty
        Dim strRequisiciones As String = String.Empty
        Dim strAsesor As String = String.Empty

        Utilitarios.CargarCulturaActual()

        Try
            Dim clsMensajeria As New MensajeriaCls(SBO_Application, m_oCompany)

            dtConsulta = m_oForm.DataSources.DataTables.Item("dtConsulta")

            strConsultaRequisicionesForm = String.Format(strConsultaRequisiciones, strIdSucursal)
            dtConsulta.ExecuteQuery(strConsultaRequisicionesForm)
            strRequisiciones = dtConsulta.GetValue(0, 0)


            strConsultaRequisicionesForm = String.Format(strConsultaAsesor, m_strNoOrden)
            dtConsulta.ExecuteQuery(strConsultaRequisicionesForm)
            strAsesor = dtConsulta.GetValue(0, 0)


            If Not String.IsNullOrEmpty(strRequisiciones) Then
                If strRequisiciones = "Y" Then
                    blnDraft = True
                Else
                    blnDraft = False
                End If
            Else
                blnDraft = False
            End If

            If Not p_blnConf_TallerEnSAP Then
                'Envia mensaje al encargado de Taller para avisar de una creación o actualización de la cotización
                If ((m_strNoOrden <> "") AndAlso (Not m_strNumeroVisita Is Nothing)) Then

                    If m_intEstCotizacion = CotizacionEstado.creada Then
                        clsMensajeria.CreaMensajeSBO_DMS(My.Resources.Resource.MensajeCotizacionCreada, m_strNoOrden, m_intDocEntry, MensajeriaCls.RecibeMensaje.EncargadoTaller, 0, m_strNumeroVisita, strIdSucursal)


                    ElseIf m_intEstCotizacion = CotizacionEstado.modificada Or p_strDocEntryTrasfElimRep <> "" Then
                        clsMensajeria.CreaMensajeSBO_DMS(My.Resources.Resource.MensajeCotizacionActualizada, m_strNoOrden, m_intDocEntry, MensajeriaCls.RecibeMensaje.EncargadoTaller, 0, m_strNumeroVisita, strIdSucursal)

                    End If
                End If
            Else
                If ((m_strNoOrden <> "") AndAlso (Not m_strNumeroVisita Is Nothing)) Then

                    If m_intEstCotizacion = CotizacionEstado.creada Then
                        clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeCotizacionCreada, m_intDocEntry, m_strNoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgPeticionRepuestos, blnDraft, m_oForm, "dtConsulta", strIdSucursal, p_TipoMensaje, True, p_blnConf_TallerEnSAP)
                    ElseIf m_intEstCotizacion = CotizacionEstado.modificada Or p_strDocEntryTrasfElimRep <> "" Then
                        clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeCotizacionActualizada, m_intDocEntry, m_strNoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgPeticionRepuestos, blnDraft, m_oForm, "dtConsulta", strIdSucursal, p_TipoMensaje, True, p_blnConf_TallerEnSAP)
                    End If
                End If
            End If

            'Envía mensaje al bodeguero para informarle de un traslado
            If p_strDocEntryTrasfElimRep <> "" Then
                clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeTraslado, p_strDocEntryTrasfElimRep, m_strNoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgDevolucionRepuestos, blnDraft, m_oForm, "dtConsulta", strIdSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoRepuestos), False, p_blnConf_TallerEnSAP, strAsesor)
            End If
            If p_strDocEntryTrasfElimSum <> "" Then
                clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeTraslado, p_strDocEntryTrasfElimSum, m_strNoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgDevolucionSuministros, blnDraft, m_oForm, "dtConsulta", strIdSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoSuministros), False, p_blnConf_TallerEnSAP, strAsesor)
            End If
            If p_strDocEntryTrasfRep <> "" Then
                clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeTraslado, p_strDocEntryTrasfRep, m_strNoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgPeticionRepuestos, blnDraft, m_oForm, "dtConsulta", strIdSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoRepuestos), False, p_blnConf_TallerEnSAP, strAsesor)
            End If
            If p_strDocEntryTrasfSum <> "" Then
                clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeTraslado, p_strDocEntryTrasfSum, m_strNoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgPeticionSuministros, blnDraft, m_oForm, "dtConsulta", strIdSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoSuministros), False, p_blnConf_TallerEnSAP, strAsesor)
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Private Sub ImprimirCotizacion(ByRef oCotizacion As Documents)

        Dim strDireccionReporte As String
        Dim strDocEntry As String


        strDireccionReporte = DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim()

        If Not String.IsNullOrEmpty(m_strNoOrden) Then

            If Utilitarios.ValidarOTInternaConfiguracion(m_oCompany) Then
                strDocEntry = oCotizacion.DocEntry

                strDireccionReporte = strDireccionReporte & "\" & My.Resources.Resource.rptOrdenRecepcionInterna & ".rpt"

                Call Utilitarios.ImprimirReporte(strDireccionReporte, My.Resources.Resource.rptOrdenRecepcionInterna, strDocEntry, CatchingEvents.DBUser, CatchingEvents.DBPassword, m_oCompany.CompanyDB, m_oCompany.Server)

            Else
                Dim strNombreBDTaller As String = String.Empty
                Utilitarios.DevuelveNombreBDTaller(SBO_Application, strNombreBDTaller)

                strDireccionReporte = strDireccionReporte & "\" & My.Resources.Resource.rptOrdenRecepcion & ".rpt"

                Call Utilitarios.ImprimirReporte(strDireccionReporte, My.Resources.Resource.OrdenRecepcion, m_strNoOrden, CatchingEvents.DBUser, CatchingEvents.DBPassword, strNombreBDTaller, m_oCompany.Server)
            End If

        End If

        'Utilitarios.DevuelveDireccionReportes(SBO_Application, strDireccionReporte)

        'If m_cnnSCGTaller IsNot Nothing Then
        '    strDBDMSOne = m_cnnSCGTaller.Database
        'Else
        '    Utilitarios.DevuelveNombreBDTaller(SBO_Application, strIdSucursal, strDBDMSOne)
        'End If

        'If (m_intNoCopiasRep = 0) Then
        '    Dim strNoCopias As String = "1"
        '    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, mc_strCopiasRepRecepcion, strNoCopias)

        '    If strNoCopias = "" Then
        '        m_intNoCopiasRep = 1
        '    Else
        '        m_intNoCopiasRep = CInt(strNoCopias)
        '    End If
        'End If

        'objReporte.P_BarraTitulo = My.Resources.Resource.OrdenRecepcion
        'objReporte.P_CompanyName = m_oCompany.CompanyName
        'objReporte.P_DataBase = strDBDMSOne
        ''objReporte.P_NCopias = m_intNoCopiasRep
        'objReporte.P_Filename = My.Resources.Resource.rptOrdenRecepcion & ".rpt"
        'objReporte.P_ParArray = m_strNoOrden
        'objReporte.P_Password = CatchingEvents.DBPassword
        'objReporte.P_Server = m_oCompany.Server
        'objReporte.P_User = m_oCompany.DbUserName
        'objReporte.P_WorkFolder = strDireccionReporte

        'For i As Integer = 1 To m_intNoCopiasRep
        '    objReporte.PrintReporte(False)
        'Next

        oCotizacion.UserFields.Fields.Item(mc_strImprimirOT).Value = CStr(ImprimirOT.scgNo)
        oCotizacion.Update()

    End Sub

    Private Sub VerificarLineasActualizadas(ByVal oCotizacion As SAPbobsCOM.Documents)
        Dim intLineNum As Integer

        For intLineNum = 0 To m_oCotizacionAnterior.Lines.Count - 1

            m_oCotizacionAnterior.Lines.SetCurrentLine(intLineNum)
            oCotizacion.Lines.SetCurrentLine(intLineNum)
            If oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value <> m_oCotizacionAnterior.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value Then
                m_intEstCotizacion = CotizacionEstado.modificada
                Exit For
            End If

        Next

    End Sub

    Private Sub RevisionStock(ByRef p_oLineaCotizacion As SAPbobsCOM.Document_Lines, _
                                   ByVal p_intDocEntry As Integer, _
                                   ByVal p_strNoBodegaRepu As String, _
                                   ByVal p_strNoBodegaSu As String, _
                                   ByVal p_intTipoArticulo As TiposArticulos, _
                                   ByVal p_intGenerico As Integer, _
                                   ByRef p_decCantidad As Decimal, _
                                   ByRef p_intEstadoTraslado As Integer, _
                                   ByRef p_intCantidadItemsPaquete As Integer, _
                                   ByRef p_intCantidadItemsTotal As Integer, _
                                   ByRef p_intEstadoPaquete As ArticuloAprobado, _
                                   ByRef p_blnRechazarItem As Boolean, _
                                   ByVal p_blnActualizarCantidad As Boolean, _
                                   Optional ByVal p_decCantidadAdicional As Decimal = 0)

        Dim decCantidad As Decimal
        Dim decCantidadDisponible As Decimal = 0
        Dim objUtilitarios As New SCGDataAccess.Utilitarios(strCadenaConexionBDTaller)
        Dim l_enumValidacionResult As enumItemValidacionResult

        With p_oLineaCotizacion
            If p_intTipoArticulo <> TiposArticulos.scgPaquete Then
                If (((.UserFields.Fields.Item(mc_strItemAprobado).Value = 1 AndAlso p_intCantidadItemsPaquete <= 0) _
                    Or (p_intEstadoPaquete = ArticuloAprobado.scgSi AndAlso p_intCantidadItemsPaquete > 0)) _
                    AndAlso .UserFields.Fields.Item(mc_strTrasladado).Value = 0) Or p_blnActualizarCantidad Then

                    .UserFields.Fields.Item("U_SCGD_Compra").Value = "N"

                    If p_intTipoArticulo = TiposArticulos.scgRepuesto _
                        AndAlso p_intGenerico = 1 Then
                        If Not p_blnActualizarCantidad Then
                            decCantidad = .Quantity
                        Else
                            decCantidad = p_decCantidadAdicional
                        End If

                        l_enumValidacionResult = ValidarCantidadDisponibleRepuesto(.ItemCode, .ItemDescription, .LineNum, p_intDocEntry, decCantidad, p_strNoBodegaRepu, p_blnActualizarCantidad, .UserFields.Fields.Item(mc_strTrasladado).Value)

                        If l_enumValidacionResult = enumItemValidacionResult.scgNoAprobar Then

                            p_blnRechazarItem = True
                            If Not String.IsNullOrEmpty(.UserFields.Fields.Item("U_SCGD_ContR").Value) Then
                                Dim cant = Convert.ToInt32(.UserFields.Fields.Item("U_SCGD_ContR").Value)
                                .UserFields.Fields.Item("U_SCGD_ContR").Value = cant + 1
                            Else
                                .UserFields.Fields.Item("U_SCGD_ContR").Value = 1
                            End If

                        ElseIf l_enumValidacionResult = enumItemValidacionResult.scgModQtyCoti Then
                            If blnDraft Then
                                p_intEstadoTraslado = 4
                            Else
                                p_intEstadoTraslado = 2
                            End If
                            p_decCantidad = decCantidad
                            m_intRealizarTraslados = enumRealizarTraslados.scgSi

                        ElseIf l_enumValidacionResult = enumItemValidacionResult.scgPendTransf Then

                            p_intEstadoTraslado = 3
                            p_decCantidad = decCantidad
                            m_intRealizarTraslados = enumRealizarTraslados.scgSi

                            If Not String.IsNullOrEmpty(.UserFields.Fields.Item("U_SCGD_ContT").Value) Then
                                Dim cant = Convert.ToInt32(.UserFields.Fields.Item("U_SCGD_ContT").Value)
                                .UserFields.Fields.Item("U_SCGD_ContT").Value = cant + 1
                            Else
                                .UserFields.Fields.Item("U_SCGD_ContT").Value = 1
                            End If

                        ElseIf l_enumValidacionResult = enumItemValidacionResult.scgComprar Then

                            If Not p_blnActualizarCantidad Then
                                p_decCantidad = .Quantity
                            Else
                                decCantidad = p_decCantidadAdicional
                            End If
                            p_intEstadoTraslado = enumItemValidacionResult.scgComprar
                            m_intRealizarTraslados = enumRealizarTraslados.scgSi
                            .UserFields.Fields.Item("U_SCGD_CPen").Value = .Quantity
                            .UserFields.Fields.Item("U_SCGD_Compra").Value = "Y"

                            If Not String.IsNullOrEmpty(.UserFields.Fields.Item("U_SCGD_ContC").Value) Then
                                Dim cant = Convert.ToInt32(.UserFields.Fields.Item("U_SCGD_ContC").Value)
                                .UserFields.Fields.Item("U_SCGD_ContC").Value = cant + 1
                            Else
                                .UserFields.Fields.Item("U_SCGD_ContC").Value = 1
                            End If

                        ElseIf l_enumValidacionResult = enumItemValidacionResult.scgSinCambio Then
                            p_decCantidad = .Quantity
                            m_intRealizarTraslados = enumRealizarTraslados.scgNo
                        Else

                            ''''''''Se agrega para Documentos Draf'''''''''''
                            If blnDraft Then

                                If Not p_blnActualizarCantidad Then
                                    p_decCantidad = .Quantity
                                Else
                                    decCantidad = p_decCantidadAdicional
                                End If
                                p_intEstadoTraslado = 4
                                m_intRealizarTraslados = enumRealizarTraslados.scgNo
                                '''''''''''''''''''''''''''''''''''''''''''''''''''

                            Else

                                If Not p_blnActualizarCantidad Then
                                    p_decCantidad = .Quantity
                                Else
                                    decCantidad = p_decCantidadAdicional
                                End If
                                p_intEstadoTraslado = 2
                                m_intRealizarTraslados = enumRealizarTraslados.scgSi

                            End If
                        End If

                    ElseIf p_intTipoArticulo = TiposArticulos.scgSuministro Then

                        If Not p_blnActualizarCantidad Then
                            decCantidad = .Quantity
                        Else
                            decCantidad = p_decCantidadAdicional
                        End If

                        l_enumValidacionResult = ValidarCantidadDisponibleSuministro(.ItemCode, .ItemDescription, decCantidad, p_strNoBodegaSu, .LineNum, p_intDocEntry)

                        If l_enumValidacionResult = enumItemValidacionResult.scgPendTransf Then

                            p_intEstadoTraslado = 3
                            m_intRealizarTraslados = enumRealizarTraslados.scgNo
                            p_decCantidad = decCantidad

                        Else
                            ''''''''Se agrega para Documentos Draf'''''''''''
                            If blnDraft Then

                                p_decCantidad = .Quantity
                                p_intEstadoTraslado = 4
                                m_intRealizarTraslados = enumRealizarTraslados.scgNo

                            Else
                                p_decCantidad = .Quantity
                                p_intEstadoTraslado = 2
                                m_intRealizarTraslados = enumRealizarTraslados.scgSi
                            End If

                        End If
                    End If

                ElseIf ((.UserFields.Fields.Item(mc_strItemAprobado).Value = 1 AndAlso p_intCantidadItemsPaquete <= 0) _
                    Or (p_intEstadoPaquete = ArticuloAprobado.scgSi AndAlso p_intCantidadItemsPaquete > 0)) _
                    AndAlso .UserFields.Fields.Item(mc_strTrasladado).Value = 3 Then

                    If (CInt(p_intTipoArticulo) = TiposArticulos.scgRepuesto _
                        AndAlso CInt(p_intGenerico) = 1) Then

                        decCantidad = .Quantity

                        l_enumValidacionResult = ValidarCantidadDisponibleRepuesto(.ItemCode, .ItemDescription, .LineNum, p_intDocEntry, decCantidad, p_strNoBodegaRepu, p_blnActualizarCantidad, .UserFields.Fields.Item(mc_strTrasladado).Value)

                        If l_enumValidacionResult = enumItemValidacionResult.scgSinCambio And blnDraft Then
                            m_intRealizarTraslados = enumRealizarTraslados.scgNo
                            p_decCantidad = .Quantity
                            p_intEstadoTraslado = 3

                        ElseIf l_enumValidacionResult = enumItemValidacionResult.scgPendBodega Then

                            m_intRealizarTraslados = enumRealizarTraslados.scgNo
                            p_decCantidad = .Quantity
                            p_intEstadoTraslado = 4

                        ElseIf l_enumValidacionResult = enumItemValidacionResult.scgSinCambio Then

                            m_intRealizarTraslados = enumRealizarTraslados.scgSi
                            p_decCantidad = .Quantity
                            p_intEstadoTraslado = 2
                        End If

                    ElseIf CInt(p_intTipoArticulo) = TiposArticulos.scgSuministro Then

                        decCantidad = .Quantity

                        l_enumValidacionResult = ValidarCantidadDisponibleSuministro(.ItemCode, .ItemDescription, decCantidad, p_strNoBodegaSu, .LineNum, p_intDocEntry)

                        If l_enumValidacionResult = enumItemValidacionResult.scgPendTransf Then
                            p_decCantidad = .Quantity
                            p_intEstadoTraslado = 3
                            m_intRealizarTraslados = enumRealizarTraslados.scgNo
                        ElseIf l_enumValidacionResult = enumItemValidacionResult.scgPendBodega Then
                            If blnDraft Then
                                m_intRealizarTraslados = enumRealizarTraslados.scgNo
                                p_decCantidad = .Quantity
                                p_intEstadoTraslado = 4
                            Else
                                m_intRealizarTraslados = enumRealizarTraslados.scgSi
                                p_decCantidad = .Quantity
                                p_intEstadoTraslado = 2
                            End If
                        Else
                            p_decCantidad = .Quantity
                            p_intEstadoTraslado = 2
                            m_intRealizarTraslados = enumRealizarTraslados.scgSi
                        End If

                    End If
                Else
                    p_decCantidad = .Quantity
                End If
            Else
                p_intCantidadItemsPaquete = objUtilitarios.CantidadLineasPaquetes(.ItemCode)
                p_intCantidadItemsTotal = p_intCantidadItemsPaquete
                p_intEstadoPaquete = .UserFields.Fields.Item(mc_strItemAprobado).Value

            End If

        End With

    End Sub

    Private Function ValidarCantidadDisponibleRepuesto(ByVal p_strItemCode As String, _
                                                        ByVal p_strItemDescripcion As String, _
                                                        ByVal p_intLineNum As Integer, _
                                                        ByVal p_intDocEntry As Integer, _
                                                        ByRef p_decCantidad As Decimal, _
                                                        ByVal p_strNoBodega As String, _
                                                        ByVal p_blnActualizar As Boolean, _
                                                        ByVal p_intEstadoItem As Integer) As enumItemValidacionResult

        Dim decCantidad As Decimal
        Dim decCantXLineasAnteriores As Decimal
        Dim intMsgResult As Integer
        Dim l_enumResult As enumItemValidacionResult
        Dim ItemCantAnterior As stTipoListaCantAnteriores

        decCantidad = DevuelveStockDisponibleItem(p_strItemCode, p_strNoBodega)
        decCantXLineasAnteriores = DevuelveCantXLineasAnteriores(p_strItemCode, p_intLineNum, p_intDocEntry)

        If decCantXLineasAnteriores <> 0 Then

            With ItemCantAnterior
                .Cantidad = decCantXLineasAnteriores
                .ItemCode = p_strItemCode
                .LineNum = p_intLineNum
            End With

            m_lstCantidadesAnteriores.Add(ItemCantAnterior)

        End If

        If (decCantidad - decCantXLineasAnteriores) <= 0 AndAlso p_intEstadoItem = 0 Then

            If Not p_blnActualizar Then
                intMsgResult = SBO_Application.MessageBox(My.Resources.Resource.El_Item & p_strItemDescripcion & My.Resources.Resource.SinInventario, 1, My.Resources.Resource.Comprar, My.Resources.Resource.Rechazar, My.Resources.Resource.Trasladar)
            Else
                intMsgResult = SBO_Application.MessageBox(My.Resources.Resource.El_Item & p_strItemDescripcion & My.Resources.Resource.SinInventario, 1, My.Resources.Resource.Comprar, My.Resources.Resource.Rechazar)
            End If

            If intMsgResult = 1 Then
                l_enumResult = enumItemValidacionResult.scgComprar
            ElseIf intMsgResult = 2 Then
                l_enumResult = enumItemValidacionResult.scgNoAprobar
            ElseIf intMsgResult = 3 Then
                l_enumResult = enumItemValidacionResult.scgPendTransf
            End If

        ElseIf (decCantidad - decCantXLineasAnteriores) < p_decCantidad AndAlso p_intEstadoItem = 0 Then

            intMsgResult = SBO_Application.MessageBox(My.Resources.Resource.ItemCantidadInventario & p_strItemDescripcion & My.Resources.Resource.InventarioInsuficiente, 1, My.Resources.Resource.PendienteTraslado, My.Resources.Resource.Rechazar, My.Resources.Resource.Trasladar)

            If intMsgResult = 1 Then
                l_enumResult = enumItemValidacionResult.scgPendTransf
            ElseIf intMsgResult = 2 Then
                l_enumResult = enumItemValidacionResult.scgNoAprobar
            ElseIf intMsgResult = 3 Then
                l_enumResult = enumItemValidacionResult.scgModQtyCoti
                p_decCantidad = decCantidad
            End If

        ElseIf (decCantidad - decCantXLineasAnteriores) < p_decCantidad AndAlso p_intEstadoItem = 3 Then
            l_enumResult = enumItemValidacionResult.scgSinCambio

        Else
            If blnDraft Then
                m_intRealizarTraslados = enumRealizarTraslados.scgNo
                l_enumResult = enumItemValidacionResult.scgPendBodega
            Else
                m_intRealizarTraslados = enumRealizarTraslados.scgSi
                l_enumResult = enumItemValidacionResult.scgSinCambio
            End If

        End If

        Return l_enumResult

    End Function

    Private Function ValidarCantidadDisponibleSuministro(ByVal p_strItemCode As String, ByVal p_strItemDescripcion As String, ByRef p_decCantidad As Decimal, ByVal p_strNoBodega As String, _
                                ByVal p_intLineNum As Integer, ByVal p_intDocEntry As Integer) As enumItemValidacionResult

        Dim decCantidad As Decimal
        Dim decCantXLineasAnteriores As Decimal
        Dim l_enumResult As enumItemValidacionResult

        decCantidad = DevuelveStockDisponibleItem(p_strItemCode, p_strNoBodega)
        decCantXLineasAnteriores = DevuelveCantXLineasAnteriores(p_strItemCode, p_intLineNum, p_intDocEntry)

        If decCantidad - decCantXLineasAnteriores <= 0 Then

            l_enumResult = enumItemValidacionResult.scgPendTransf

        ElseIf decCantidad - decCantXLineasAnteriores < p_decCantidad Then

            l_enumResult = enumItemValidacionResult.scgPendTransf

        Else

            ''''''''''para documentos Draft''''''''''''''''''''
            If blnDraft Then
                m_intRealizarTraslados = enumRealizarTraslados.scgNo
                l_enumResult = enumItemValidacionResult.scgPendBodega
            Else
                m_intRealizarTraslados = enumRealizarTraslados.scgSi
                l_enumResult = enumItemValidacionResult.scgSinCambio
            End If

        End If

        Return l_enumResult

    End Function

    Private Function DevuelveStockDisponibleItem(ByVal strItemcode As String, _
                                       ByVal strWhsCode As String) As Double

        Dim oItemArticulo As SAPbobsCOM.IItems
        Dim oItemWhsInfo As SAPbobsCOM.IItemWarehouseInfo
        Dim intCount As Integer
        Dim dblStock As Double

        oItemArticulo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
        oItemArticulo.GetByKey(strItemcode)

        oItemWhsInfo = oItemArticulo.WhsInfo

        For intCount = 0 To oItemWhsInfo.Count - 1
            With oItemWhsInfo
                .SetCurrentLine(intCount)
                If .WarehouseCode = strWhsCode Then
                    dblStock = .InStock - .Committed
                    Exit For
                End If
            End With
        Next

        Return dblStock

    End Function

    Private Function DevuelveCantXLineasAnteriores(ByVal p_strItemCode As String, ByVal p_intLineNum As Integer, _
                            ByVal p_intDocEntry As Integer) As Decimal

        Dim m_dtConsulta As SAPbouiCOM.DataTable
        Dim m_strConsulta As String = " SELECT DocEntry, LineNum, ItemCode, Quantity, OpenQty, U_SCGD_IdRepxOrd, U_SCGD_Aprobado, U_SCGD_Traslad, " & _
                                      " U_SCGD_CodEspecifico FROM QUT1 with(nolock) WHERE DocEntry = '{0}' AND ItemCode = '{1}' ORDER BY LineNum "
        m_strConsulta = String.Format(m_strConsulta, p_intDocEntry, p_strItemCode)
        If m_oForm Is Nothing Then
            m_oForm = _SBO_Application.Forms.ActiveForm
        End If

        If Utilitarios.ValidaExisteDataTable(m_oForm, "dtConsulta") Then
            m_dtConsulta = m_oForm.DataSources.DataTables.Item("dtConsulta")
        Else
            m_dtConsulta = m_oForm.DataSources.DataTables.Add("dtConsulta")
        End If

        m_dtConsulta.ExecuteQuery(m_strConsulta)

        Dim intContLineas As Decimal
        Dim decCantidadAnterior As Decimal = 0
        Dim strAprobado As String = String.Empty
        Dim strTrasladado As String = String.Empty
        Dim strCantidad As String = String.Empty
        Dim decCantidad As Decimal = 0
        Dim intLineNum As Integer

        For intContLineas = 0 To m_dtConsulta.Rows.Count - 1
            strAprobado = m_dtConsulta.GetValue("U_SCGD_Aprobado", intContLineas).ToString.Trim()
            strTrasladado = m_dtConsulta.GetValue("U_SCGD_Traslad", intContLineas).ToString.Trim()
            strCantidad = m_dtConsulta.GetValue("Quantity", intContLineas).ToString.Trim()
            intLineNum = m_dtConsulta.GetValue("LineNum", intContLineas).ToString.Trim()

            If Not String.IsNullOrEmpty(strCantidad) Then
                decCantidad = Decimal.Parse(strCantidad)
            End If

            If intLineNum < p_intLineNum Then
                If strAprobado = "1" AndAlso (strTrasladado = "0" Or strTrasladado = "3") Then
                    decCantidadAnterior += decCantidad
                End If
            Else
                Exit For
            End If
        Next

        Return decCantidadAnterior

    End Function

    Private Sub AsignarColaborador(ByVal p_intNoFase As Integer, _
                                   ByVal p_intIDActividad As Integer, _
                                   ByVal p_intIDColaborador As Integer, _
                                   ByRef p_dtsAsignados As ColaboradorDataset)
        Dim dtrAsignando As ColaboradorDataset.SCGTA_TB_ControlColaboradorRow

        dtrAsignando = p_dtsAsignados.SCGTA_TB_ControlColaborador.NewSCGTA_TB_ControlColaboradorRow

        With dtrAsignando
            .NoFase = p_intNoFase
            .NoOrden = m_strNoOrden
            .Reproceso = 0
            .EmpID = p_intIDColaborador
            .EmpNombre = " "
            .FechaInicio = Nothing
            .FechaFin = Nothing
            .TiempoHoras = 0
            .Estado = "No iniciado"
            .Costo = 0
            .IDActividad = p_intIDActividad
            .CostoEstandar = 0
        End With

        p_dtsAsignados.SCGTA_TB_ControlColaborador.AddSCGTA_TB_ControlColaboradorRow(dtrAsignando)

    End Sub

    Private Sub ModificaColaborador(ByVal p_intLineNum As Integer, _
                                   ByVal p_intIDActividad As Integer, _
                                   ByVal p_intIDColaborador As Integer, _
                                   ByRef p_dtsAsignados As ColaboradorDataset, _
                                   ByVal strNombreTaller As String, _
                                   ByVal p_decDuracion As Decimal)

        Dim strID As String = String.Empty

        For Each m_drwActividades In m_dstActividadesxOrden.SCGTA_TB_ActividadesxOrden.Rows

            If m_drwActividades.RowState <> DataRowState.Deleted Then

                If m_drwActividades.ID = p_intIDActividad AndAlso m_drwActividades.LineNum = p_intLineNum Then
                    strID = m_drwActividades.ID
                    strID = strID.Trim()

                    If strID = p_intIDActividad Then
                        Dim strConsultaColaborador As String = "Update SCGTA_TB_ControlColaborador set EmpID = {0} where IDActividad = '{1}'"
                        Dim strConsultaDuracionEstandar As String = "Update SCGTA_TB_ActividadesxOrden set DuracionAprobada = {0} where ID = '{1}'"

                        Utilitarios.EjecutarConsulta(String.Format(strConsultaColaborador, p_intIDColaborador, p_intIDActividad), strNombreTaller, SBO_Application.Company.ServerName)
                        Utilitarios.EjecutarConsulta(String.Format(strConsultaDuracionEstandar, p_decDuracion, p_intIDActividad), strNombreTaller, SBO_Application.Company.ServerName)
                        Exit For
                    End If
                End If

            End If
        Next
    End Sub

    Private Sub FinalizarAsignacion(ByRef p_dtsAsignados As ColaboradorDataset)
        Dim objDA As New SCGDataAccess.ColaboradorDataAdapter(False)
        objDA.InsertarNuevo(p_dtsAsignados, m_cnnSCGTaller, m_trnTransaccion)
    End Sub

    Private Function PaquetesCotizacion(ByVal p_oCotizacion As SAPbobsCOM.Documents) As Generic.Dictionary(Of Integer, ArticuloAprobado)
        Dim objPaquetes As New Generic.Dictionary(Of Integer, ArticuloAprobado)
        Dim intLineNum As Integer = 0
        Dim intTipoArticulo As TiposArticulos
        Dim intItemAprobado As ArticuloAprobado
        Dim strItemCode As String = ""
        Dim strTipoArt As String = ""

        For intLineNum = 0 To p_oCotizacion.Lines.Count - 1
            p_oCotizacion.Lines.SetCurrentLine(intLineNum)
            strItemCode = p_oCotizacion.Lines.ItemCode
            strTipoArt = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString().Trim()
            intTipoArticulo = IIf(IsNumeric(strTipoArt), strTipoArt, 0)
            intItemAprobado = p_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value
            If intTipoArticulo = TiposArticulos.scgPaquete Then
                objPaquetes.Add(intLineNum, intItemAprobado)
            End If
        Next

        Return objPaquetes
    End Function

    Private Function ManejarCreacionOTEspecial(ByVal p_blnConf_TallerEnSAP As Boolean, ByRef p_OTHijaCreada As String, Optional ByVal p_DECotRef As Integer = 0) As Boolean

        Dim intCantidadLineasTrasladadas As Integer = 0
        Dim intError As Integer
        Dim strMensaje As String = ""
        Dim strConexionDBSucursal As String = ""
        Dim strDocEntrysTransfELIMSuministros As String = ""
        Dim strDocEntrysTransfSUM As String = ""
        Dim blnOfertaCompra As Boolean = False
        Dim blnCreaUdoOTSAP As Boolean = False

        Dim strDocEntrysTransfREP As String = ""

        Dim strNoIniciada As String = String.Empty

        If p_DECotRef > 0 Then
            m_objCotizacionPadre = ObtenerCotizacionAnterior(p_DECotRef, p_blnConf_TallerEnSAP)
        Else
            m_objCotizacionPadre = ObtenerCotizacionAnterior(0, p_blnConf_TallerEnSAP)
        End If

        strNoIniciada = Utilitarios.EjecutarConsulta(" select Name from [@SCGD_ESTADOS_OT] with(nolock) where code = '1' ", m_oCompany.CompanyDB, m_oCompany.Server)

        If Not p_blnConf_TallerEnSAP Then

            Utilitarios.DevuelveNombreBDTaller(SBO_Application, strIdSucursal, m_strBDTalller)
            Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application,
            strIdSucursal,
            strConexionDBSucursal)

            m_adpCambioAlmacenProceso = New CambioBodegaProcesoDatasetTableAdapters.SCGTA_SP_SelCambioBodegaProcesoTableAdapter
            m_adpCambioAlmacenProceso.Connection = New SqlClient.SqlConnection(strConexionDBSucursal)
            m_adpCambioAlmacenProceso.Connection.Open()
            m_adpCambioAlmacenProceso.Fill(m_dtsCambioAlmacenProceso, m_oCotizacion.DocEntry)

            m_adpCambioCuentaProceso = New CambioCuentaProcesoDatasetTableAdapters.SCGTA_SP_SelCambioCuentaProcesoTableAdapter
            m_adpCambioCuentaProceso.Connection = New SqlClient.SqlConnection(strConexionDBSucursal)
            m_adpCambioCuentaProceso.Connection.Open()
            m_adpCambioCuentaProceso.Fill(m_dtsCambioCuentaProceso, m_oCotizacion.DocEntry)

            m_adpLineNumsOTOriginal = New LineNumsOTOriginalTableAdapters.SCGTA_SP_LineNumsOTOriginalTableAdapter
            m_adpLineNumsOTOriginal.Connection = New SqlClient.SqlConnection(strConexionDBSucursal)
            m_adpLineNumsOTOriginal.Connection.Open()
            m_adpLineNumsOTOriginal.Fill(m_dtsLineNumsOTOriginal, m_oCotizacion.DocEntry)

        End If

        SBO_Application.StatusBar.SetText(My.Resources.Resource.CreandoOrden, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

        Dim strNoVisita As String = m_oCotizacion.UserFields.Fields.Item(mc_strNum_Visita).Value
        Dim strNoOrdenSiguiente As String = Utilitarios.EjecutarConsulta(
            String.Format("SELECT COUNT(DocEntry) + 1 FROM OQUT with (nolock) WHERE U_SCGD_No_Visita= '{0}' and (U_SCGD_Numero_OT is not null and U_SCGD_Numero_OT <> '') ", strNoVisita),
                                                               m_oCompany.CompanyDB, m_oCompany.Server)

        If Integer.Parse(strNoOrdenSiguiente) < 10 Then strNoOrdenSiguiente = String.Format("0{0}", strNoOrdenSiguiente)



        m_strNoOrden = String.Format("{0}-{1}", strNoVisita.Trim(), strNoOrdenSiguiente.Trim())



        m_oCotizacion.UserFields.Fields.Item(mc_strNum_OT).Value = m_strNoOrden

        SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoLineas, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

        ProcesarLineasOTEspecial(p_blnConf_TallerEnSAP, intCantidadLineasTrasladadas)

        Call AsignarFechayHoraOT(m_oCotizacion)

        If blnServExtOTEspeciales Then
            Dim strNoOrdeReferencia As String = m_oCotizacion.UserFields.Fields.Item(mc_strOTPadre).Value
            blnOfertaCompra = BuscarOT_En_OfertaCompra(strNoOrdeReferencia)
            ActualizarNumeroOT_EnLineas(strNoOrdeReferencia, blnOfertaCompra, p_blnConf_TallerEnSAP)
            blnOfertaCompra = False
        End If

        If Not p_blnConf_TallerEnSAP Then

            Call LlenarListadoPartes()

        End If
        m_blnIniciarTransaccion = True

        Dim strIdSucu As String = m_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()

        If Not p_blnConf_TallerEnSAP Then
            Call CrearOrdenTrabajo(m_strNoOrden)
        Else
            blnCreaUdoOTSAP = CrearOrdenTrabajoSAP(m_strNoOrden, strIdSucu, strNoIniciada, m_oCotizacion.DocumentsOwner)
        End If

        p_OTHijaCreada = m_strNoOrden
        m_blnIniciarTransaccion = False

        m_oCompany.StartTransaction()

        If blnCreaUdoOTSAP Then
            UDOOrden.Insert()
        End If

        If p_blnConf_TallerEnSAP Then
            'm_cnnSCGTaller
            If objTransferenciaStock Is Nothing Then
                objTransferenciaStock = New TransferenciaItems(SBO_Application, m_oCompany, strCadenaConexionBDTaller)
            End If
        End If

        Call objTransferenciaStock.CrearTrasladoAddOnNuevo(m_lstRepuestos, m_lstSuministros, m_lstServiociosEX, m_lstItemsEliminarRepuestos, m_lstItemsEliminarSuministros, m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, m_strNoOrden, m_strNoBodegaRepu, _
                                                                               m_strNoBodegaSumi, m_strNoBodegaSeEx, m_strNoBodegaProceso, m_strIDSerieDocTrasnf, m_cnnSCGTaller, m_trnTransaccion, False, strDocEntrysTransfREP, strDocEntrysTransfSUM, strDocEntrysTransfELIMSuministros, m_strDescMarca, m_strDescEstilo, m_strDescModelo, m_strPlaca, m_strVIN, m_strEmpleadoRecibe, m_strCodigoCliente, True, False,
                                                                               strIdSucursal)

        Call GenerarAsientosAjustes()

        SBO_Application.StatusBar.SetText(My.Resources.Resource.OrdenInicial, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        If m_objCotizacionPadre.DocumentStatus = SAPbobsCOM.BoStatus.bost_Open Then
            If m_objCotizacionPadre.Update() <> 0 Then
                m_oCompany.GetLastError(intError, strMensaje)
                If intError <> 0 Then
                    Throw New ExceptionsSBO(intError, strMensaje)
                End If
            End If
        End If

        If CancelarOTAnterior(intCantidadLineasTrasladadas) Then
            Call CancelarOrden(p_blnConf_TallerEnSAP)
            m_objCotizacionPadre.UserFields.Fields.Item(mc_strEstadoCot).Value = My.Resources.Resource.EstadoOrdenCancelada
            m_objCotizacionPadre.UserFields.Fields.Item(mc_strEstadoCotID).Value = "5"
            m_objCotizacionPadre.Update()
            m_objCotizacionPadre.Cancel()
        End If

    End Function

    Private Sub LlenarListadoPartes()

        Dim drwCambioAlmacenProceso As CambioBodegaProcesoDataset.SCGTA_SP_SelCambioBodegaProcesoRow
        Dim blnCrearTransaferencia As Boolean = False

        Dim strBodegaActual As String = ""
        Dim strBodegaDestino As String = ""

        m_lstRepuestos.Clear()
        m_lstSuministros.Clear()
        m_lstServiociosEX.Clear()
        m_lstItemsEliminarRepuestos.Clear()
        m_lstItemsEliminarSuministros.Clear()
        m_lstItemACambiarEstado.Clear()
        m_lstItemACambiarEstadoAdicional.Clear()

        objTransferenciaStock = New TransferenciaItems(SBO_Application, m_oCompany)

        For Each drwCambioAlmacenProceso In m_dtsCambioAlmacenProceso.Rows

            If Not drwCambioAlmacenProceso.IsProcesoOrdNueNull Then
                If drwCambioAlmacenProceso.IsProcesoOrdOriNull Then
                    strBodegaActual = drwCambioAlmacenProceso.ProcesoArt
                    strBodegaDestino = drwCambioAlmacenProceso.ProcesoOrdNue
                Else
                    strBodegaActual = drwCambioAlmacenProceso.ProcesoOrdOri
                    strBodegaDestino = drwCambioAlmacenProceso.ProcesoOrdNue
                End If
                blnCrearTransaferencia = True
            Else
                If Not drwCambioAlmacenProceso.IsProcesoOrdOriNull Then

                    strBodegaActual = drwCambioAlmacenProceso.ProcesoOrdOri
                    strBodegaDestino = drwCambioAlmacenProceso.ProcesoArt
                    blnCrearTransaferencia = True
                End If

            End If
            If blnCrearTransaferencia Then
                Select Case drwCambioAlmacenProceso.Tipo

                    Case TiposArticulos.scgRepuesto

                        ''Genera la Lista de los Repuestos que se van a trasladar
                        objTransferenciaStock.GeneraListaCambioBodegaProceso(TransferenciaItems.scgTiposMovimientoXBodega.TransfRepuestos, m_lstRepuestos, _
                                strBodegaActual, strBodegaActual, strBodegaActual, strBodegaDestino, _
                                 drwCambioAlmacenProceso.Tipo, 2, -1, 0, m_oCotizacion.DocEntry, drwCambioAlmacenProceso.NoParte, drwCambioAlmacenProceso.Cantidad)

                    Case TiposArticulos.scgSuministro

                        ''Genera la Lista de los Suministros que se van a trasladar
                        objTransferenciaStock.GeneraListaCambioBodegaProceso(TransferenciaItems.scgTiposMovimientoXBodega.TransfSuministros, m_lstSuministros, _
                                strBodegaActual, strBodegaActual, strBodegaActual, strBodegaDestino, _
                                 drwCambioAlmacenProceso.Tipo, 2, -1, 0, m_oCotizacion.DocEntry, drwCambioAlmacenProceso.NoParte, drwCambioAlmacenProceso.Cantidad)

                    Case TiposArticulos.scgServicioExt
                        ''Genera la Lista de los Servicios Externos que se van a trasladar
                        objTransferenciaStock.GeneraListaCambioBodegaProceso(TransferenciaItems.scgTiposMovimientoXBodega.TransfServiciosEx, m_lstServiociosEX, _
                                strBodegaActual, strBodegaActual, strBodegaActual, strBodegaDestino, _
                                 drwCambioAlmacenProceso.Tipo, 2, -1, 0, m_oCotizacion.DocEntry, drwCambioAlmacenProceso.NoParte, drwCambioAlmacenProceso.Cantidad)
                End Select
            End If
        Next

    End Sub

    Private Sub GenerarAsientosAjustes()

        Dim strCuenta As String = ""
        Dim strContracuenta As String = ""
        Dim m_objBLSBO As New BLSBO.GlobalFunctionsSBO
        Dim strMonedaLocal As String
        m_objBLSBO.Set_Compania(m_oCompany)
        strMonedaLocal = m_objBLSBO.RetornarMonedaLocal()

        Dim drwCuentaACambiar As CambioCuentaProcesoDataset.SCGTA_SP_SelCambioCuentaProcesoRow
        For Each drwCuentaACambiar In m_dtsCambioCuentaProceso.Rows
            If Not drwCuentaACambiar.IsProcesoOrdOriNull Then
                strContracuenta = drwCuentaACambiar.ProcesoOrdOri
            Else
                If Not drwCuentaACambiar.IsProcesoArtGoANull Then
                    strContracuenta = drwCuentaACambiar.ProcesoArtGoA
                Else
                    strContracuenta = drwCuentaACambiar.ProcesoArt
                End If

            End If

            If Not drwCuentaACambiar.IsProcesoOrdNueNull Then
                strCuenta = drwCuentaACambiar.ProcesoOrdNue
            Else
                If Not drwCuentaACambiar.IsProcesoArtGoANull Then
                    strCuenta = drwCuentaACambiar.ProcesoArtGoA
                Else
                    strCuenta = drwCuentaACambiar.ProcesoArt
                End If
            End If
            If strCuenta <> strContracuenta _
            AndAlso Not drwCuentaACambiar.IsU_CostoNull _
            AndAlso drwCuentaACambiar.U_Costo <> 0 Then

                Call Utilitarios.CrearAsiento(m_oCompany, strCuenta, strContracuenta, "", My.Resources.Resource.MensajeAsientoAjusteOTEspecial, drwCuentaACambiar.U_Costo, Date.Now, "", "", False, True, strMonedaLocal, "", m_strNumeroOT, drwCuentaACambiar.NoParte)

            End If

        Next
    End Sub

    Private Sub CancelarOrden(ByVal p_blnConf_TallerEnSAP As Boolean)

        Dim cnConeccion As New SqlClient.SqlConnection
        Dim cmdConsultarCotizaciones As New SqlClient.SqlCommand
        Dim m_oCompanyService As SAPbobsCOM.CompanyService
        Dim m_oGeneralService As SAPbobsCOM.GeneralService
        Dim m_oGeneralData As SAPbobsCOM.GeneralData
        Dim m_oGenralParam As SAPbobsCOM.GeneralDataParams
        Dim m_strConsultaCode As String = String.Empty
        Dim m_strResultado As String = String.Empty



        If Not p_blnConf_TallerEnSAP Then
            cnConeccion = New SqlClient.SqlConnection(strCadenaConexionBDTaller)
            cnConeccion.Open()

            cmdConsultarCotizaciones.Connection = m_cnnSCGTaller
            cmdConsultarCotizaciones.Transaction = m_trnTransaccion

            cmdConsultarCotizaciones.CommandText = "Update SCGTA_TB_Orden set Estado = 5 where NoOrden = '" & m_strOTPadre & "'"
            cmdConsultarCotizaciones.CommandType = CommandType.Text
            cmdConsultarCotizaciones.ExecuteNonQuery()
        Else

            m_strConsultaCode = String.Format("Select Code from [@SCGD_OT] where U_NoOT = '{0}'", m_strOTPadre)
            m_strResultado = Utilitarios.EjecutarConsulta(m_strConsultaCode, m_oCompany.CompanyDB, m_oCompany.Server)

            m_oCompanyService = m_oCompany.GetCompanyService
            m_oGeneralService = m_oCompanyService.GetGeneralService("SCGD_OT")
            m_oGenralParam = m_oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            m_oGenralParam.SetProperty("Code", m_strResultado)
            m_oGeneralData = m_oGeneralService.GetByParams(m_oGenralParam)
            m_oGeneralData.SetProperty("U_EstO", "5")
            m_oGeneralData.SetProperty("U_DEstO", My.Resources.Resource.EstadoOrdenCancelada)
            m_oGeneralService.Update(m_oGeneralData)



        End If


    End Sub

    Private Function CancelarOTAnterior(ByVal p_intCantidadLineasTrasladadas As Integer) As Boolean

        Dim blnCerrarOT As Boolean = True
        Dim strTipoArticulo As String = String.Empty
        Dim intLineaCotizacionPadre As Integer

        If m_objCotizacionPadre.DocumentStatus = SAPbobsCOM.BoStatus.bost_Open Then
            For intLineaCotizacionPadre = 0 To m_objCotizacionPadre.Lines.Count - 1

                m_objCotizacionPadre.Lines.SetCurrentLine(intLineaCotizacionPadre)

                If m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi Then
                    Return False
                End If
            Next
        End If

        Return blnCerrarOT
    End Function

    Private Sub ProcesarLineasOTEspecial(ByVal p_blnConf_TallerEnSAP As Boolean, ByRef p_intCantidadLineasTrasladadas As Integer)

        Dim intNumLineaCotizacion As Integer
        Dim intCantidadLineasXPaquete As Integer
        Dim intEstadoPaquete As Integer

        Dim strConexionDBSucursal As String = String.Empty

        Dim intTipoArticulo As TiposArticulos
        Dim strTipoArticulo As String
        Dim intCodFase As Integer
        Dim strCodFase As String

        Dim blnRechazarItem As Boolean
        Dim intGenerico As Integer
        Dim strGenerico As String
        Dim intEstadoItem As ArticuloAprobado
        Dim intLineaNumFather As Integer = -1
        Dim blnEsLineaNueva As Boolean
        Dim blnArticuloBienConfigurado As Boolean = True
        Dim decCantidadAdicional As Decimal
        Dim blnDisminuirCantidad As Boolean = False
        Dim blnTipoAdmitido As Boolean = False
        Dim strServicosExternosInventariables As String

        Dim blnProcesar As Boolean = False
        Dim oItemArticulo As SAPbobsCOM.IItems

        Try

            If Not p_blnConf_TallerEnSAP Then

                Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application,
                                                              strIdSucursal,
                                                              strConexionDBSucursal)

                adpConf = New ConfiguracionDataAdapter(strConexionDBSucursal)
                adpConf.Fill(dstConf)

                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, mc_strBodegaRepuestos, m_strNoBodegaRepu)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, TransferenciaItems.mc_strBodegaSuministros, m_strNoBodegaSumi)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, mc_strBodegaServiciosExternos, m_strNoBodegaSeEx)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, TransferenciaItems.mc_strBodegaProceso, m_strNoBodegaProceso)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, TransferenciaItems.mc_strIDSerieDocumentosTraslado, m_strIDSerieDocTrasnf)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, "SEInventariables", strServicosExternosInventariables)

                m_dstRepuestosxOrden = New RepuestosxOrdenDataset
                m_adpRepuestosxOrden = New RepuestosxOrdenDataAdapter(strCadenaConexionBDTaller)
                m_adpRepuestosxOrden.Fill(m_dstRepuestosxOrden, m_strOTPadre)

                m_dstSuministrosxOrden = New SuministrosDataset
                m_adpSuministrosxOrden = New SuministrosDataAdapter(strCadenaConexionBDTaller)
                m_adpSuministrosxOrden.Fill(m_dstSuministrosxOrden, m_strOTPadre, -1, -1)

                m_dstActividadesxOrden = New ActividadesXFaseDataset
                m_adpActividadesxOrden = New ActividadesXFaseDataAdapter(strCadenaConexionBDTaller)
                m_adpActividadesxOrden.FillbyFilters(m_dstActividadesxOrden, m_strOTPadre, 0, 1)

                m_dstPaquetesxOrden = New PaquetesDataSet
                m_adpPaqutesxOrden = New PaquetesxOrdenDataAdapter(strCadenaConexionBDTaller)
                m_adpPaqutesxOrden.Fill(m_dstPaquetesxOrden, m_strOTPadre)

                m_lstRepuestos.Clear()
                m_lstSuministros.Clear()
                m_lstServiociosEX.Clear()
                m_lstItemsEliminarRepuestos.Clear()
                m_lstItemsEliminarSuministros.Clear()
                m_lstItemACambiarEstado.Clear()
                m_lstItemACambiarEstadoAdicional.Clear()
                Utilitarios.DevuelveNombreBDTaller(SBO_Application, strIdSucursal, m_strBDTalller)
                objUtilitarios = New SCGDataAccess.Utilitarios(strCadenaConexionBDTaller)

            Else
                m_lstRepuestos.Clear()
                m_lstSuministros.Clear()
                m_lstServiociosEX.Clear()
                m_lstItemsEliminarRepuestos.Clear()
                m_lstItemsEliminarSuministros.Clear()
                m_lstItemACambiarEstado.Clear()
                m_lstItemACambiarEstadoAdicional.Clear()

                If m_blnServicosExternosInventariables Then
                    strServicosExternosInventariables = 1
                Else
                    strServicosExternosInventariables = 0
                End If

            End If


            If objTransferenciaStock IsNot Nothing Then
                objTransferenciaStock = Nothing
            End If


            intEstadoPaquete = 0
            intCantidadLineasXPaquete = 0

            For intNumLineaCotizacion = 0 To m_oCotizacion.Lines.Count - 1

                m_oCotizacion.Lines.SetCurrentLine(intNumLineaCotizacion)

                SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoItem & (m_oCotizacion.Lines.LineNum + 1) & My.Resources.Resource.Separador & m_oCotizacion.Lines.ItemDescription, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)


                oItemArticulo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
                oItemArticulo.GetByKey(m_oCotizacion.Lines.ItemCode)

                strTipoArticulo = oItemArticulo.UserFields.Fields.Item(mc_strTipoArticulo).Value
                intTipoArticulo = IIf(IsNumeric(strTipoArticulo), CInt(strTipoArticulo), -1)

                strGenerico = oItemArticulo.UserFields.Fields.Item(mc_strGenerico).Value
                intGenerico = IIf(IsNumeric(strGenerico), CInt(strGenerico), 0)

                blnDisminuirCantidad = False
                decCantidadAdicional = 0

                If (intTipoArticulo <> 6) Then

                    Select Case intTipoArticulo
                        Case TiposArticulos.scgActividad
                            blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tNO, False)
                            If blnArticuloBienConfigurado Then

                                strCodFase = oItemArticulo.UserFields.Fields.Item(mc_strFase).Value.ToString.Trim
                                intCodFase = IIf(IsNumeric(strCodFase), strCodFase, 0)
                                If intCodFase = 0 Then
                                    blnArticuloBienConfigurado = False
                                End If
                            End If
                            blnTipoAdmitido = True
                        Case TiposArticulos.scgPaquete
                            blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tNO, True)
                            blnTipoAdmitido = True
                        Case TiposArticulos.scgRepuesto
                            blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True)
                            blnTipoAdmitido = True
                        Case TiposArticulos.scgServicioExt
                            If strServicosExternosInventariables = 0 Then
                                blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True)
                            Else
                                blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True)
                            End If
                            blnTipoAdmitido = True
                        Case TiposArticulos.scgSuministro
                            blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True)
                            blnTipoAdmitido = True
                        Case TiposArticulos.scgOtrosIngresos
                            blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True, False)
                            blnTipoAdmitido = True
                        Case TiposArticulos.scgOtrosGastos_Costos
                            blnArticuloBienConfigurado = DevuelveConfiguracionItem(oItemArticulo, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tYES, True, False)
                            blnTipoAdmitido = True
                    End Select

                    If blnArticuloBienConfigurado Then

                        intEstadoItem = m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value

                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = m_strNoOrden

                        If intCantidadLineasXPaquete <= 0 Then
                            intLineaNumFather = -1
                            intEstadoPaquete = 2
                        End If
                        blnRechazarItem = False

                        If intTipoArticulo = TiposArticulos.scgSuministro Or intTipoArticulo = TiposArticulos.scgRepuesto Then
                            Dim strIdRepuestoXOrden As String = String.Empty
                            Dim strIdRepuestoXOrdenAnterior As String = String.Empty

                            If intNumLineaCotizacion < m_oCotizacionAnterior.Lines.Count Then
                                strIdRepuestoXOrden = m_oCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value.ToString.Trim()
                                For intLineaCotAnterior As Integer = 0 To m_oCotizacionAnterior.Lines.Count - 1
                                    m_oCotizacionAnterior.Lines.SetCurrentLine(intLineaCotAnterior)

                                    Dim strIsLinePadre As Boolean = False
                                    If m_oCotizacionAnterior.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                                        strIsLinePadre = True
                                    End If
                                    If Not strIsLinePadre Then
                                        strIdRepuestoXOrdenAnterior = m_oCotizacionAnterior.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value.ToString.Trim()
                                        If Not String.IsNullOrEmpty(strIdRepuestoXOrdenAnterior) And Not String.IsNullOrEmpty(strIdRepuestoXOrden) Then
                                            'If strIdRepuestoXOrdenAnterior = strIdRepuestoXOrden Then
                                            If strIdRepuestoXOrdenAnterior = strIdRepuestoXOrden And (m_oCotizacionAnterior.Lines.ItemCode = m_oCotizacion.Lines.ItemCode) Then
                                                If m_oCotizacionAnterior.Lines.Quantity < m_oCotizacion.Lines.Quantity Then
                                                    SBO_Application.MessageBox(My.Resources.Resource.LacantidadDelItem + m_oCotizacion.Lines.ItemDescription + My.Resources.Resource.CantidadNoAumenta + vbCrLf + My.Resources.Resource.AgregueLineaParaCantidad)
                                                    m_oCotizacion.Lines.Quantity = m_oCotizacionAnterior.Lines.Quantity
                                                ElseIf m_oCotizacionAnterior.Lines.Quantity > m_oCotizacion.Lines.Quantity AndAlso m_oCotizacionAnterior.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 2 Then
                                                    blnDisminuirCantidad = True
                                                    decCantidadAdicional = m_oCotizacionAnterior.Lines.Quantity - m_oCotizacion.Lines.Quantity
                                                End If
                                                Exit For
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        End If
                        If Not blnRechazarItem Then
                            If (intEstadoItem = ArticuloAprobado.scgSi AndAlso intCantidadLineasXPaquete <= 0) Or (intEstadoPaquete = ArticuloAprobado.scgSi AndAlso intCantidadLineasXPaquete > 0) Then
                                If Not m_blnUsaConfiguracionInternaTaller Then
                                    blnEsLineaNueva = AgregarLineasOTEspecial(m_oCotizacion.Lines, intCantidadLineasXPaquete, intTipoArticulo, intEstadoPaquete, intLineaNumFather, intCodFase, p_intCantidadLineasTrasladadas)
                                Else
                                    'blnEsLineaNueva = AgregarLineasSolicitudOTEspecial(m_oCotizacion.Lines, intCantidadLineasXPaquete, intTipoArticulo, intEstadoPaquete, intLineaNumFather, intCodFase, p_intCantidadLineasTrasladadas)
                                End If

                                m_blnIniciarTransaccion = False
                                If intTipoArticulo = TiposArticulos.scgActividad Or intTipoArticulo = TiposArticulos.scgServicioExt Then
                                    intCantidadLineasXPaquete -= 1
                                ElseIf blnEsLineaNueva AndAlso intTipoArticulo <> TiposArticulos.scgPaquete Then
                                    intCantidadLineasXPaquete -= 1
                                End If
                            Else
                                If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo And m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                                    blnProcesar = True
                                ElseIf m_oCotizacion.Lines.TreeType <> SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                    blnProcesar = False
                                End If
                                If blnProcesar = True Then
                                    If m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo And m_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                        m_oCotizacion.Lines.UserFields.Fields.Item(mc_strItemAProcesar).Value = LineaAProcesar.scgNo
                                    End If
                                End If
                                blnEsLineaNueva = False
                                SBO_Application.MessageBox(My.Resources.Resource.El_Item & m_oCotizacion.Lines.ItemDescription & My.Resources.Resource.NoAprobadoItemEspecial)
                            End If
                        Else
                            SBO_Application.MessageBox(My.Resources.Resource.El_Item & m_oCotizacion.Lines.ItemDescription & My.Resources.Resource.MalConfigurado)
                        End If
                    Else
                        If blnTipoAdmitido Then
                            SBO_Application.MessageBox(My.Resources.Resource.El_Item & m_oCotizacion.Lines.ItemDescription & My.Resources.Resource.MalConfigurado)
                        End If
                    End If
                End If

            Next
            ListaLineNumPaquetes.Clear()


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        Finally
            Utilitarios.DestruirObjeto(oItemArticulo)
        End Try

    End Sub
    Private Function AgregarLineasSolicitudOTEspecial(ByRef p_oLineasCotizacion As SAPbobsCOM.Document_Lines, _
                                        ByVal p_intCantidadLineasPaquete As Integer, _
                                        ByVal p_intTipoArticulo As Integer, _
                                        ByVal p_intEstadoPaquete As Integer, _
                                        ByVal p_intLineNumFather As Integer, _
                                        ByVal p_intCodFase As Integer, _
                                        ByRef p_intCantidadLineasAgregadas As Integer) As Boolean

        Dim intCantidad As Integer
        Dim strItemCode As String
        Dim intLineNum As Integer
        Dim intItemAprobado As Integer
        Dim intEstadoTransf As Integer
        Dim blnYaAgregada As Boolean = False
        Dim intIDEmpleado As Integer
        Dim objUtilitarios As New SCGDataAccess.Utilitarios(strCadenaConexionBDTaller)

        Dim visOrder As Integer
        Dim cadenaConexion As String = String.Empty
        Dim nombreTabla As String = "QUT1"

        Dim blnLineaNueva As Boolean
        Dim strObservacionesLinea As String

        Dim strEsCompra As String = String.Empty

        'intCantidad = p_oLineasCotizacion.Quantity
        strItemCode = p_oLineasCotizacion.ItemCode
        'intLineNum = p_oLineasCotizacion.LineNum

        intItemAprobado = p_oLineasCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value
        intEstadoTransf = p_oLineasCotizacion.UserFields.Fields.Item(mc_strTrasladado).Value

        If ((intItemAprobado = ArticuloAprobado.scgSi AndAlso p_intCantidadLineasPaquete <= 0) _
           Or (intItemAprobado = ArticuloAprobado.scgSi AndAlso p_intTipoArticulo = TiposArticulos.scgPaquete)) _
           Or (p_intEstadoPaquete = ArticuloAprobado.scgSi AndAlso p_intCantidadLineasPaquete > 0) Then

            If p_intTipoArticulo <> TiposArticulos.scgNinguno Then
                Select Case p_intTipoArticulo
                    Case TiposArticulos.scgServicioExt

                        ListaItemsCodeOTEspeciales.Add(m_drwRepuestos.NoRepuesto)
                        ListaIdRepxOrdenOTEspeciales.Add(m_drwRepuestos.ID)
                        ListaNoOrdenOTEspeciales.Add(m_drwRepuestos.NoOrden)

                        blnServExtOTEspeciales = True

                    Case TiposArticulos.scgRepuesto
                        If p_oLineasCotizacion.UserFields.Fields.Item(mc_strCompra).Value = "Y" Then
                            ListaItemsCodeOTEspeciales.Add(m_drwRepuestos.NoRepuesto)
                            ListaIdRepxOrdenOTEspeciales.Add(m_drwRepuestos.ID)
                            ListaNoOrdenOTEspeciales.Add(m_drwRepuestos.NoOrden)
                            blnServExtOTEspeciales = True
                        End If
                End Select

            End If

        End If



    End Function

    Private Function AgregarLineasOTEspecial(ByRef p_oLineasCotizacion As SAPbobsCOM.Document_Lines, _
                                        ByVal p_intCantidadLineasPaquete As Integer, _
                                        ByVal p_intTipoArticulo As Integer, _
                                        ByVal p_intEstadoPaquete As Integer, _
                                        ByVal p_intLineNumFather As Integer, _
                                        ByVal p_intCodFase As Integer, _
                                        ByRef p_intCantidadLineasAgregadas As Integer) As Boolean
        Dim intCantidad As Integer
        Dim strItemCode As String
        Dim intLineNum As Integer
        Dim intItemAprobado As Integer
        Dim intEstadoTransf As Integer
        Dim blnYaAgregada As Boolean = False
        Dim intIDEmpleado As Integer
        Dim objUtilitarios As New SCGDataAccess.Utilitarios(strCadenaConexionBDTaller)

        Dim visOrder As Integer
        Dim cadenaConexion As String = String.Empty
        Dim nombreTabla As String = "QUT1"

        Dim blnLineaNueva As Boolean
        Dim strObservacionesLinea As String

        Dim strEsCompra As String = String.Empty
        Try


            intCantidad = p_oLineasCotizacion.Quantity
            strItemCode = p_oLineasCotizacion.ItemCode
            intLineNum = p_oLineasCotizacion.LineNum

            intItemAprobado = p_oLineasCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value
            intEstadoTransf = p_oLineasCotizacion.UserFields.Fields.Item(mc_strTrasladado).Value

            If ((intItemAprobado = ArticuloAprobado.scgSi AndAlso p_intCantidadLineasPaquete <= 0) _
                Or (intItemAprobado = ArticuloAprobado.scgSi AndAlso p_intTipoArticulo = TiposArticulos.scgPaquete)) _
                Or (p_intEstadoPaquete = ArticuloAprobado.scgSi AndAlso p_intCantidadLineasPaquete > 0) Then

                If p_intCantidadLineasPaquete <= 0 Then
                    p_intLineNumFather = -1
                End If
                If p_intTipoArticulo <> TiposArticulos.scgNinguno Then
                    Select Case p_intTipoArticulo

                        Case TiposArticulos.scgRepuesto

                            For Each m_drwRepuestos In m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows
                                If m_drwRepuestos.RowState <> DataRowState.Deleted Then

                                    'Dim strPrueba As String = p_oLineasCotizacion.UserFields.Fields.Item(mc_strIdRepxOrd).Value

                                    If p_oLineasCotizacion.UserFields.Fields.Item(mc_strIdRepxOrd).Value = m_drwRepuestos.ID Then
                                        If p_oLineasCotizacion.Quantity = m_drwRepuestos.Cantidad Then
                                            m_drwRepuestos.NoOrden = m_strNoOrden
                                            For index As Integer = 0 To m_objCotizacionPadre.Lines.Count - 1
                                                m_objCotizacionPadre.Lines.SetCurrentLine(index)
                                                If m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = p_oLineasCotizacion.UserFields.Fields.Item(mc_strIdRepxOrd).Value Then

                                                    m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo
                                                    m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strTrasladadoOTHija).Value = enumTrasladadoOTHija.scgOTHijaSI

                                                    strObservacionesLinea = m_objCotizacionPadre.Lines.FreeText & My.Resources.Resource.LineaTrasladadaOrden & m_strNoOrden
                                                    If strObservacionesLinea.Length <= 100 Then
                                                        m_objCotizacionPadre.Lines.FreeText = strObservacionesLinea
                                                    Else
                                                        m_objCotizacionPadre.Lines.FreeText = strObservacionesLinea.Substring(0, 100)
                                                    End If
                                                    m_drwRepuestos.LineNum = p_oLineasCotizacion.LineNum
                                                    p_oLineasCotizacion.Quantity = m_drwRepuestos.Cantidad
                                                    p_oLineasCotizacion.UserFields.Fields.Item(mc_strTrasladado).Value = m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strTrasladado).Value
                                                    m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strTrasladado).Value = 0
                                                    m_objCotizacionPadre.Lines.Price = 0
                                                    m_drwRepuestos.LineNumFather = p_intLineNumFather
                                                    m_intEstCotizacion = CotizacionEstado.creada

                                                    p_intCantidadLineasAgregadas += 1

                                                    If p_oLineasCotizacion.UserFields.Fields.Item(mc_strCompra).Value = "Y" Then
                                                        ListaItemsCodeOTEspeciales.Add(m_drwRepuestos.NoRepuesto)
                                                        ListaIdRepxOrdenOTEspeciales.Add(m_drwRepuestos.ID)
                                                        ListaNoOrdenOTEspeciales.Add(m_drwRepuestos.NoOrden)
                                                        blnServExtOTEspeciales = True
                                                    End If

                                                    Exit For
                                                End If
                                            Next

                                        End If
                                        Exit For
                                    End If

                                End If
                            Next

                        Case Utilitarios.TiposArticulos.scgActividad
                            intIDEmpleado = IIf(IsNumeric(p_oLineasCotizacion.UserFields.Fields.Item(mc_strEmpRealiza).Value), p_oLineasCotizacion.UserFields.Fields.Item(mc_strEmpRealiza).Value, 0)
                            For Each m_drwActividades In m_dstActividadesxOrden.SCGTA_TB_ActividadesxOrden.Rows
                                If m_drwActividades.RowState <> DataRowState.Deleted Then
                                    Dim at As String = p_oLineasCotizacion.UserFields.Fields.Item(mc_strIdRepxOrd).Value
                                    If p_oLineasCotizacion.UserFields.Fields.Item(mc_strIdRepxOrd).Value = m_drwActividades.ID Then
                                        If m_drwActividades.IDEmpleado <> intIDEmpleado Then

                                            m_drwActividades.IDEmpleado = intIDEmpleado
                                            m_intEstCotizacion = CotizacionEstado.modificada

                                        End If
                                        For index As Integer = 0 To m_objCotizacionPadre.Lines.Count - 1
                                            m_objCotizacionPadre.Lines.SetCurrentLine(index)
                                            If m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = p_oLineasCotizacion.UserFields.Fields.Item(mc_strIdRepxOrd).Value Then

                                                m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo
                                                m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strTrasladadoOTHija).Value = enumTrasladadoOTHija.scgOTHijaSI

                                                strObservacionesLinea = m_objCotizacionPadre.Lines.FreeText & My.Resources.Resource.LineaTrasladadaOrden & m_strNoOrden
                                                If strObservacionesLinea.Length <= 100 Then
                                                    m_objCotizacionPadre.Lines.FreeText = strObservacionesLinea
                                                Else
                                                    m_objCotizacionPadre.Lines.FreeText = strObservacionesLinea.Substring(0, 100)
                                                End If
                                                p_oLineasCotizacion.UserFields.Fields.Item(mc_strEmpRealiza).Value = m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strEmpRealiza).Value
                                                p_oLineasCotizacion.UserFields.Fields.Item(mc_strNombEmpleado).Value = m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strNombEmpleado).Value
                                                m_drwActividades.LineNum = p_oLineasCotizacion.LineNum
                                                m_drwActividades.NoOrden = m_strNoOrden
                                                p_oLineasCotizacion.Quantity = m_drwActividades.Cantidad
                                                m_drwActividades.LineNumFather = p_intLineNumFather
                                                m_intEstCotizacion = CotizacionEstado.creada
                                                p_intCantidadLineasAgregadas += 1

                                                Exit For
                                            End If
                                        Next

                                        Exit For

                                    End If
                                End If
                            Next

                        Case TiposArticulos.scgSuministro
                            For Each m_drwSuministros In m_dstSuministrosxOrden.SCGTA_VW_Suministros.Rows
                                If m_drwSuministros.RowState <> DataRowState.Deleted Then

                                    If p_oLineasCotizacion.UserFields.Fields.Item(mc_strIdRepxOrd).Value = m_drwSuministros.ID Then
                                        m_drwSuministros.NoOrden = m_strNoOrden
                                        For index As Integer = 0 To m_objCotizacionPadre.Lines.Count - 1
                                            m_objCotizacionPadre.Lines.SetCurrentLine(index)
                                            If m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = p_oLineasCotizacion.UserFields.Fields.Item(mc_strIdRepxOrd).Value Then


                                                m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo
                                                m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strTrasladadoOTHija).Value = enumTrasladadoOTHija.scgOTHijaSI

                                                p_oLineasCotizacion.UserFields.Fields.Item(mc_strTrasladado).Value = m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strTrasladado).Value
                                                strObservacionesLinea = m_objCotizacionPadre.Lines.FreeText & My.Resources.Resource.LineaTrasladadaOrden & m_strNoOrden
                                                If strObservacionesLinea.Length <= 100 Then
                                                    m_objCotizacionPadre.Lines.FreeText = strObservacionesLinea
                                                Else
                                                    m_objCotizacionPadre.Lines.FreeText = strObservacionesLinea.Substring(0, 100)
                                                End If
                                                m_drwSuministros.LineNum = p_oLineasCotizacion.LineNum
                                                p_oLineasCotizacion.Quantity = m_drwSuministros.Cantidad
                                                m_drwSuministros.LineNumFather = p_intLineNumFather
                                                m_intEstCotizacion = CotizacionEstado.creada
                                                p_intCantidadLineasAgregadas += 1
                                                Exit For

                                            End If
                                        Next

                                        Exit For
                                    End If

                                End If
                            Next

                        Case TiposArticulos.scgServicioExt


                            For Each m_drwRepuestos In m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows
                                If m_drwRepuestos.RowState <> DataRowState.Deleted Then

                                    If p_oLineasCotizacion.UserFields.Fields.Item(mc_strIdRepxOrd).Value = m_drwRepuestos.ID Then
                                        If p_oLineasCotizacion.Quantity = m_drwRepuestos.Cantidad Then
                                            m_drwRepuestos.NoOrden = m_strNoOrden
                                            For index As Integer = 0 To m_objCotizacionPadre.Lines.Count - 1
                                                m_objCotizacionPadre.Lines.SetCurrentLine(index)
                                                If m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = p_oLineasCotizacion.UserFields.Fields.Item(mc_strIdRepxOrd).Value Then

                                                    m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo
                                                    m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strTrasladadoOTHija).Value = enumTrasladadoOTHija.scgOTHijaSI

                                                    strObservacionesLinea = m_objCotizacionPadre.Lines.FreeText & My.Resources.Resource.LineaTrasladadaOrden & m_strNoOrden
                                                    If strObservacionesLinea.Length <= 100 Then
                                                        m_objCotizacionPadre.Lines.FreeText = strObservacionesLinea
                                                    Else
                                                        m_objCotizacionPadre.Lines.FreeText = strObservacionesLinea.Substring(0, 100)
                                                    End If
                                                    p_oLineasCotizacion.UserFields.Fields.Item(mc_strTrasladado).Value = m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strTrasladado).Value
                                                    m_drwRepuestos.LineNum = p_oLineasCotizacion.LineNum
                                                    p_oLineasCotizacion.Quantity = m_drwRepuestos.Cantidad
                                                    p_oLineasCotizacion.UserFields.Fields.Item(mc_strTrasladado).Value = m_drwRepuestos.Trasladado
                                                    m_drwRepuestos.LineNumFather = p_intLineNumFather
                                                    m_intEstCotizacion = CotizacionEstado.creada
                                                    p_intCantidadLineasAgregadas += 1

                                                    ListaItemsCodeOTEspeciales.Add(m_drwRepuestos.NoRepuesto)
                                                    ListaIdRepxOrdenOTEspeciales.Add(m_drwRepuestos.ID)
                                                    ListaNoOrdenOTEspeciales.Add(m_drwRepuestos.NoOrden)



                                                    blnServExtOTEspeciales = True

                                                    Exit For

                                                End If
                                            Next
                                        End If
                                        Exit For
                                    End If
                                End If
                            Next

                        Case TiposArticulos.scgPaquete
                            p_intCantidadLineasPaquete = objUtilitarios.CantidadLineasPaquetes(strItemCode)
                            Dim lineNum_Padre = m_objCotizacionPadre.Lines.LineNum

                            ' blnActualizarLineasPaquetes = True
                            For Each m_drwPaquetes In m_dstPaquetesxOrden._PaquetesDataSet.Rows

                                If strItemCode = m_drwPaquetes.ItemCode Then
                                    If Not ListaLineNumPaquetes.Contains(m_drwPaquetes.LineNum) Then
                                        For index As Integer = 0 To m_objCotizacionPadre.Lines.Count - 1
                                            m_objCotizacionPadre.Lines.SetCurrentLine(index)
                                            If m_objCotizacionPadre.Lines.ItemCode = p_oLineasCotizacion.ItemCode Then
                                                m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo
                                                m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strTrasladadoOTHija).Value = enumTrasladadoOTHija.scgOTHijaSI
                                                p_intLineNumFather = m_drwPaquetes.LineNum
                                                blnLineaNueva = True
                                                p_intCantidadLineasAgregadas += 1
                                                ListaLineNumPaquetes.Add(m_drwPaquetes.LineNum)
                                                Exit For
                                            End If
                                        Next
                                        Exit For
                                    End If
                                End If

                            Next

                        Case TiposArticulos.scgOtrosIngresos

                            Dim lineNum_Padre As Integer = m_objCotizacionPadre.Lines.LineNum
                            Dim intLineaCotizacionPadre As Integer = 0
                            Dim strTipoArticulo As String = String.Empty

                            'Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, cadenaConexion)
                            'visOrder = Utilitarios.ObtieneVisOrder(m_oCompany, nombreTabla, cadenaConexion, lineNum_Padre, strItemCode, m_objCotizacionPadre.DocEntry)
                            'm_objCotizacionPadre.Lines.SetCurrentLine(visOrder)

                            If m_objCotizacionPadre.DocumentStatus = SAPbobsCOM.BoStatus.bost_Open Then
                                For intLineaCotizacionPadre = 0 To m_objCotizacionPadre.Lines.Count - 1

                                    strTipoArticulo = String.Empty
                                    m_objCotizacionPadre.Lines.SetCurrentLine(intLineaCotizacionPadre)


                                    If strItemCode = m_objCotizacionPadre.Lines.ItemCode Then
                                        'Carga el tipo de artículo

                                        strTipoArticulo = m_objCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString().Trim()

                                        If Not String.IsNullOrEmpty(strTipoArticulo) Then

                                            If m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi And CInt(strTipoArticulo) = TiposArticulos.scgOtrosIngresos Then

                                                m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo
                                                m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strTrasladadoOTHija).Value = enumTrasladadoOTHija.scgOTHijaSI

                                                strObservacionesLinea = m_objCotizacionPadre.Lines.FreeText & My.Resources.Resource.LineaTrasladadaOrden & m_strNoOrden
                                                If strObservacionesLinea.Length <= 100 Then
                                                    m_objCotizacionPadre.Lines.FreeText = strObservacionesLinea
                                                Else
                                                    m_objCotizacionPadre.Lines.FreeText = strObservacionesLinea.Substring(0, 100)
                                                End If

                                                blnLineaNueva = True
                                                p_intCantidadLineasAgregadas += 1

                                                Exit For
                                            End If
                                        End If
                                    End If
                                Next
                            End If

                            'If strItemCode = m_objCotizacionPadre.Lines.ItemCode Then
                            '    m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo
                            '    m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strTrasladadoOTHija).Value = enumTrasladadoOTHija.scgOTHijaSI

                            '    strObservacionesLinea = m_objCotizacionPadre.Lines.FreeText & My.Resources.Resource.LineaTrasladadaOrden & m_strNoOrden
                            '    If strObservacionesLinea.Length <= 100 Then
                            '        m_objCotizacionPadre.Lines.FreeText = strObservacionesLinea
                            '    Else
                            '        m_objCotizacionPadre.Lines.FreeText = strObservacionesLinea.Substring(0, 100)
                            '    End If

                            '    blnLineaNueva = True
                            '    p_intCantidadLineasAgregadas += 1
                            'End If
                        Case TiposArticulos.scgOtrosGastos_Costos

                            Dim lineNum_Padre As Integer = m_objCotizacionPadre.Lines.LineNum
                            Dim intLineaCotizacionPadre As Integer = 0
                            Dim strTipoArticulo As String = String.Empty

                            'Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, cadenaConexion)
                            'visOrder = Utilitarios.ObtieneVisOrder(m_oCompany, nombreTabla, cadenaConexion, lineNum_Padre, strItemCode, m_objCotizacionPadre.DocEntry)
                            'm_objCotizacionPadre.Lines.SetCurrentLine(visOrder)

                            If m_objCotizacionPadre.DocumentStatus = SAPbobsCOM.BoStatus.bost_Open Then
                                For intLineaCotizacionPadre = 0 To m_objCotizacionPadre.Lines.Count - 1

                                    strTipoArticulo = String.Empty
                                    m_objCotizacionPadre.Lines.SetCurrentLine(intLineaCotizacionPadre)


                                    If strItemCode = m_objCotizacionPadre.Lines.ItemCode Then
                                        'Carga el tipo de artículo

                                        strTipoArticulo = m_objCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString().Trim()

                                        If Not String.IsNullOrEmpty(strTipoArticulo) Then

                                            If m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgSi And CInt(strTipoArticulo) = TiposArticulos.scgOtrosGastos_Costos Then

                                                m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo
                                                m_objCotizacionPadre.Lines.UserFields.Fields.Item(mc_strTrasladadoOTHija).Value = enumTrasladadoOTHija.scgOTHijaSI

                                                strObservacionesLinea = m_objCotizacionPadre.Lines.FreeText & My.Resources.Resource.LineaTrasladadaOrden & m_strNoOrden
                                                If strObservacionesLinea.Length <= 100 Then
                                                    m_objCotizacionPadre.Lines.FreeText = strObservacionesLinea
                                                Else
                                                    m_objCotizacionPadre.Lines.FreeText = strObservacionesLinea.Substring(0, 100)
                                                End If

                                                blnLineaNueva = True
                                                p_intCantidadLineasAgregadas += 1

                                                Exit For
                                            End If
                                        End If
                                    End If
                                Next
                            End If

                    End Select
                Else

                    SBO_Application.MessageBox(My.Resources.Resource.El_Item + strItemCode + My.Resources.Resource.MalConfigurado)

                End If
            Else
                If p_intTipoArticulo = TiposArticulos.scgPaquete Then

                    p_intEstadoPaquete = intItemAprobado

                    p_intCantidadLineasPaquete = objUtilitarios.CantidadLineasPaquetes(strItemCode)
                Else
                    If p_intCantidadLineasPaquete > 0 Then
                        p_intCantidadLineasPaquete -= 1
                    End If
                    blnLineaNueva = False
                End If
                p_oLineasCotizacion.UserFields.Fields.Item(mc_strItemAprobado).Value = ArticuloAprobado.scgNo
            End If

            Return blnLineaNueva

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Function

    ''' <summary>
    ''' Valido si el Numero de OT se encuentra en una Oferta de Compra, de lo contrario 
    ''' se revisa en las lineas para realizar el cambio de Numero de OT
    ''' </summary>
    ''' <param name="p_strNoOT"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function BuscarOT_En_OfertaCompra(ByVal p_strNoOT As String) As Boolean

        Dim dtDocEntriesOrdenCompra As System.Data.DataTable
        Dim drwDocEntries As System.Data.DataRow
        Dim dtDocEntriesOfertaCompra As System.Data.DataTable

        Dim intOfertaCompra As String = Utilitarios.EjecutarConsulta("SELECT DocEntry FROM [OPQT] with (nolock) where U_SCGD_Numero_OT = '" & p_strNoOT & "'", m_oCompany.CompanyDB, m_oCompany.Server)

        If Not String.IsNullOrEmpty(intOfertaCompra) Then

            Return True
        Else

            dtDocEntriesOfertaCompra = Utilitarios.EjecutarConsultaDataTable("SELECT p.U_SCGD_Numero_OT FROM OPQT AS P with (nolock) INNER JOIN PQT1 AS P1 with (nolock) ON P.DocEntry = P1.DocEntry  WHERE P1.U_SCGD_NoOT = '" & p_strNoOT & "'",
                                           m_oCompany.CompanyDB,
                                           m_oCompany.Server)

            If dtDocEntriesOfertaCompra.Rows.Count <> 0 Then
                Return True
            Else
                Return False
            End If

            Return False
        End If

        'dtDocEntriesDocumentos = Utilitarios.EjecutarConsultaDataTable("SELECT DocEntry FROM [OPOR] where U_SCGD_Numero_OT = '" & p_strNoOT & "'", m_oCompany.CompanyDB, m_oCompany.Server)

    End Function
    ''' <summary>
    ''' Realiza el cambio del Numero de Orden de Trabajo, en las lineas de los documentos de Oferta de Compra, 
    ''' Orden de Compra, Entrada de Mercancia de Compras y Factura de Proveedores
    ''' </summary>
    ''' <param name="p_strNoOT"></param>
    ''' <param name="p_blnOfertaCompra"></param>
    ''' <remarks></remarks>
    Private Sub ActualizarNumeroOT_EnLineas(ByVal p_strNoOT As String, ByVal p_blnOfertaCompra As Boolean, Optional ByVal p_blnUsaTallerSAP As Boolean = False)


        Dim DocEntryOrdenCompra As Integer = 0
        Dim TargetEntryEntradaMercancia As String = String.Empty
        Dim TargetEntryFacturaProveedor As String = String.Empty
        Dim TargetEntryOrdenCompra As String = String.Empty

        Dim TargetType As String = String.Empty
        Dim strNombreTabla As String = String.Empty

        Dim DocEntryEntradaMercancia As Integer = 0
        Dim CostosSExFP As String = String.Empty
        Dim intDocEntryFacturaProv As Integer = 0
        Dim intDocEntryEntradaMerc As Integer = 0

        Dim objDocumento As SAPbobsCOM.Documents
        Dim objDocumentoLineas As SAPbobsCOM.Document_Lines

        Dim dtDocEntriesOrdenCompra As System.Data.DataTable
        Dim drwDocEntries As System.Data.DataRow
        Dim blnOfertaCompra As Boolean = False
        Dim ListaItemsCodeOTEspecialesLocal As Generic.List(Of String)
        Dim ListaIdRepxOrdenOTEspecialesLocal As Generic.List(Of String)
        Dim intLineP As Integer = -1

        If Not blnRevisionLineasOT Then

            If p_blnOfertaCompra Then

                dtDocEntriesOrdenCompra = Utilitarios.EjecutarConsultaDataTable("SELECT distinct DocEntry FROM PQT1 with (nolock) where U_SCGD_NoOT = '" & p_strNoOT & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                objDocumento = CType(m_oCompany.GetBusinessObject(540000006), SAPbobsCOM.Documents)
                strNombreTabla = "PQT1"
            Else

                dtDocEntriesOrdenCompra = Utilitarios.EjecutarConsultaDataTable("SELECT distinct DocEntry FROM POR1 with (nolock) where U_SCGD_NoOT = '" & p_strNoOT & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                objDocumento = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders), SAPbobsCOM.Documents)
                strNombreTabla = "POR1"

            End If

        Else

            If p_blnOfertaCompra Then

                dtDocEntriesOrdenCompra = Utilitarios.EjecutarConsultaDataTable("SELECT distinct DocEntry FROM PQT1 with (nolock) where U_SCGD_NoOT  = '" & strNumeroOT_En_OrdenCompra & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                objDocumento = CType(m_oCompany.GetBusinessObject(540000006), SAPbobsCOM.Documents)
                strNombreTabla = "PQT1"
            Else
                dtDocEntriesOrdenCompra = Utilitarios.EjecutarConsultaDataTable("SELECT distinct DocEntry FROM POR1 with (nolock) where U_SCGD_NoOT = '" & strNumeroOT_En_OrdenCompra & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                objDocumento = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders), SAPbobsCOM.Documents)
                strNombreTabla = "POR1"

            End If

        End If

        If Not p_blnUsaTallerSAP Then
            strNombreColumnaID = "U_SCGD_IdRepxOrd"
        Else
            strNombreColumnaID = "U_SCGD_ID"
        End If

        Try
            ListaItemsCodeOTEspecialesLocal = ListaItemsCodeOTEspeciales.GetRange(0, ListaItemsCodeOTEspeciales.Count)
            ListaIdRepxOrdenOTEspecialesLocal = ListaIdRepxOrdenOTEspeciales.GetRange(0, ListaItemsCodeOTEspeciales.Count)
            If dtDocEntriesOrdenCompra.Rows.Count <> 0 Then

                For Each drwDocEntries In dtDocEntriesOrdenCompra.Rows

                    'se obtiene el docentry del documento (Oferta u Orden de Compra), en caso de que no esten en Estatus = Cerrado
                    'se actualizará el documento directamente en el objeto Orden de Compra

                    If objDocumento.GetByKey(drwDocEntries.Item("DocEntry")) Then

                        objDocumentoLineas = objDocumento.Lines

                        For i As Integer = 0 To objDocumentoLineas.Count - 1

                            objDocumentoLineas.SetCurrentLine(i)
                            intLineP = -1
                            For j As Integer = 0 To ListaItemsCodeOTEspecialesLocal.Count - 1

                                If objDocumentoLineas.ItemCode.ToString.Trim = ListaItemsCodeOTEspecialesLocal.Item(j) And _
                                    objDocumentoLineas.UserFields.Fields.Item(strNombreColumnaID).Value = ListaIdRepxOrdenOTEspecialesLocal.Item(j) Then
                                    intLineP = j

                                    TargetType = (Utilitarios.EjecutarConsulta("SELECT TargetType FROM [" & strNombreTabla & "] with (nolock) where DocEntry = " & drwDocEntries.Item("DocEntry") & " and " & strNombreColumnaID & " = '" & ListaIdRepxOrdenOTEspecialesLocal.Item(j) & "'",
                                                m_oCompany.CompanyDB,
                                                m_oCompany.Server))

                                    If TargetType = 20 Then
                                        'Entradas de Mercancia
                                        TargetEntryEntradaMercancia = (Utilitarios.EjecutarConsulta("SELECT TrgetEntry FROM [" & strNombreTabla & "] with (nolock) where DocEntry = " & drwDocEntries.Item("DocEntry") & " and " & strNombreColumnaID & " = '" & ListaIdRepxOrdenOTEspecialesLocal.Item(j) & "'",
                                              m_oCompany.CompanyDB,
                                              m_oCompany.Server))

                                    ElseIf TargetType = 18 Then
                                        'Facturas de Proveedor
                                        TargetEntryFacturaProveedor = (Utilitarios.EjecutarConsulta("SELECT TrgetEntry FROM [" & strNombreTabla & "] with (nolock) where DocEntry = " & drwDocEntries.Item("DocEntry") & " and " & strNombreColumnaID & " = '" & ListaIdRepxOrdenOTEspecialesLocal.Item(j) & "'",
                                              m_oCompany.CompanyDB,
                                              m_oCompany.Server))

                                    ElseIf TargetType = 22 Then
                                        'Ordenes de compra
                                        TargetEntryOrdenCompra = (Utilitarios.EjecutarConsulta("SELECT TrgetEntry FROM [" & strNombreTabla & "] with (nolock) where DocEntry = " & drwDocEntries.Item("DocEntry") & " and " & strNombreColumnaID & " = '" & ListaIdRepxOrdenOTEspecialesLocal.Item(j) & "'",
                                              m_oCompany.CompanyDB,
                                              m_oCompany.Server))
                                    End If

                                    If objDocumento.DocumentStatus <> SAPbobsCOM.BoStatus.bost_Close Then
                                        objDocumentoLineas.UserFields.Fields.Item("U_SCGD_NoOT").Value = ListaNoOrdenOTEspeciales.Item(j)
                                    Else
                                        Utilitarios.EjecutarConsulta("UPDATE [dbo].[" & strNombreTabla & "] SET [U_SCGD_NoOT] = '" & ListaNoOrdenOTEspeciales.Item(j) & "' WHERE docentry = " & drwDocEntries.Item("DocEntry") & " and ItemCode = '" & ListaItemsCodeOTEspecialesLocal.Item(j) & "' and  " & strNombreColumnaID & " = '" & ListaIdRepxOrdenOTEspecialesLocal.Item(j) & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                    End If


                                    Select Case (TargetType)

                                        Case 20
                                            If Not String.IsNullOrEmpty(TargetEntryEntradaMercancia) Then
                                                If Not ListaTargetEntryEntradaMercancia.Contains(TargetEntryEntradaMercancia) Then
                                                    ListaTargetEntryEntradaMercancia.Add(TargetEntryEntradaMercancia)
                                                End If
                                                If Not ListaTargetType.Contains(TargetType) Then
                                                    ListaTargetType.Add(TargetType)
                                                End If

                                            End If

                                        Case 18

                                            If Not String.IsNullOrEmpty(TargetEntryFacturaProveedor) Then
                                                ListaTargetEntryFacturaProveedor.Add(TargetEntryFacturaProveedor)

                                                If Not ListaTargetType.Contains(TargetType) Then
                                                    ListaTargetType.Add(TargetType)
                                                End If
                                            End If

                                        Case 22

                                            If Not String.IsNullOrEmpty(TargetEntryOrdenCompra) Then
                                                If Not ListaTargetEntryOrdenCompra.Contains(TargetEntryOrdenCompra) Then
                                                    ListaTargetEntryOrdenCompra.Add(TargetEntryOrdenCompra)
                                                End If
                                                If Not ListaTargetType.Contains(TargetType) Then
                                                    ListaTargetType.Add(TargetType)
                                                End If

                                            End If

                                    End Select
                                    Exit For
                                End If
                            Next
                            If intLineP <> -1 Then
                                ListaItemsCodeOTEspecialesLocal.RemoveAt(intLineP)
                                ListaIdRepxOrdenOTEspecialesLocal.RemoveAt(intLineP)
                            End If
                        Next
                        If objDocumento.DocumentStatus <> SAPbobsCOM.BoStatus.bost_Close Then
                            objDocumento.Update()
                        End If

                    End If

                Next

                Utilitarios.DestruirObjeto(objDocumento)

                'verifico que la lista con los Documentos Destino contega al menos un valor 
                'para ir a actulizar dicho documento

                If ListaTargetType.Count > 0 Then

                    For i As Integer = 0 To ListaTargetType.Count - 1

                        If ListaTargetType.Item(i) = 20 Then

                            ActualizarNumeroOT_EnLineas_EntradaMercancia(ListaTargetEntryEntradaMercancia)

                        ElseIf ListaTargetType.Item(i) = 18 Then

                            ActualizaNumeroOT_EnLineas_FacturaProveedor(ListaTargetEntryFacturaProveedor)

                        ElseIf ListaTargetType.Item(i) = 22 Then

                            ActualizarNumeroOT_EnLineas_OrdenCompra(ListaTargetEntryOrdenCompra)

                        End If
                    Next i

                End If
            Else
                'en caso de que la Orden de Compra este ligada a la cotizacion padre -01, este busca por lineas en las ordenes de compra
                blnRevisionLineasOT = True

                If BuscarDocEntryPorNoOT_EnLineas(p_strNoOT, p_blnOfertaCompra) Then

                    Call ActualizarNumeroOT_EnLineas(strNumeroOT_En_OrdenCompra, p_blnOfertaCompra)

                End If

                blnRevisionLineasOT = False

            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex

        Finally

            ListaItemsCodeOTEspeciales.Clear()
            ListaIdRepxOrdenOTEspeciales.Clear()
            ListaNoOrdenOTEspeciales.Clear()
            ListaTargetEntryEntradaMercancia.Clear()
            ListaTargetEntryFacturaProveedor.Clear()
            ListaTargetEntryOrdenCompra.Clear()
            ListaTargetType.Clear()
        End Try

    End Sub

    'realizo la busqueda del numero de Orden de trabajo mediante las lineas, 
    'en caso de que la Oferta o la Orden de Compra esten ligadas a una OT Padre
    Private Function BuscarDocEntryPorNoOT_EnLineas(ByVal p_NumeroOT As String, ByVal p_blnOfertaCompra As Boolean) As Boolean

        Dim dtDocEntriesOrdenCompra As System.Data.DataTable
        Dim drwDocEntries As DataRow

        If p_blnOfertaCompra Then

            dtDocEntriesOrdenCompra = Utilitarios.EjecutarConsultaDataTable("SELECT p.U_SCGD_Numero_OT FROM OPQT AS P with (nolock) INNER JOIN PQT1 AS P1 with (nolock) ON P.DocEntry = P1.DocEntry  WHERE P1.U_SCGD_NoOT = '" & p_NumeroOT & "'",
                                           m_oCompany.CompanyDB,
                                           m_oCompany.Server)

        Else

            dtDocEntriesOrdenCompra = Utilitarios.EjecutarConsultaDataTable("SELECT p.U_SCGD_Numero_OT FROM OPOR AS P with (nolock) INNER JOIN POR1 AS P1 with (nolock) ON P.DocEntry = P1.DocEntry  WHERE P1.U_SCGD_NoOT = '" & p_NumeroOT & "'",
                                            m_oCompany.CompanyDB,
                                            m_oCompany.Server)

        End If

        If dtDocEntriesOrdenCompra.Rows.Count <> 0 Then

            drwDocEntries = dtDocEntriesOrdenCompra.Rows(0)

            If Not String.IsNullOrEmpty(drwDocEntries.Item("U_SCGD_Numero_OT").ToString().Trim()) Then
                strNumeroOT_En_OrdenCompra = drwDocEntries.Item("U_SCGD_Numero_OT")
                Return True
            End If
            strNumeroOT_En_OrdenCompra = String.Empty
            Return False
        Else
            strNumeroOT_En_OrdenCompra = String.Empty
            Return False
        End If

    End Function

    Private Sub ActualizarNumeroOT_EnLineas_OrdenCompra(ByVal p_ListaTargetEntry As Generic.List(Of Integer))

        Dim objDocumento As SAPbobsCOM.Documents
        Dim objDocumentoLineas As SAPbobsCOM.Document_Lines
        Dim dtConsulta As System.Data.DataTable
        Dim strListaIdRepXOrden As String = String.Empty
        Dim StrUPDATE As String = String.Empty


        Try

            If p_ListaTargetEntry.Count > 0 Then

                For i As Integer = 0 To p_ListaTargetEntry.Count - 1

                    objDocumento = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders), SAPbobsCOM.Documents)

                    'Limpio los Strings
                    strListaIdRepXOrden = String.Empty
                    StrUPDATE = String.Empty

                    If objDocumento.GetByKey(p_ListaTargetEntry.Item(i)) Then

                        objDocumentoLineas = objDocumento.Lines

                        For lineas As Integer = 0 To objDocumentoLineas.Count - 1

                            objDocumentoLineas.SetCurrentLine(lineas)

                            If ListaItemsCodeOTEspeciales.Contains(objDocumentoLineas.UserFields.Fields.Item("ItemCode").Value) AndAlso
                                ListaIdRepxOrdenOTEspeciales.Contains(objDocumentoLineas.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value) Then

                                strListaIdRepXOrden = strListaIdRepXOrden + objDocumentoLineas.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString().Trim() + ","

                                StrUPDATE = StrUPDATE & String.Format("UPDATE [dbo].[POR1] SET [U_SCGD_NoOT] = '{0}' WHERE docentry = {1} and ItemCode = '{2}' and  U_SCGD_IdRepxOrd = {3} ",
                                    ListaNoOrdenOTEspeciales.Item(0),
                                    p_ListaTargetEntry(i),
                                    objDocumentoLineas.UserFields.Fields.Item("ItemCode").Value,
                                    objDocumentoLineas.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value)

                            End If

                        Next

                        If Not String.IsNullOrEmpty(StrUPDATE) Then
                            Utilitarios.EjecutarConsulta(String.Format(StrUPDATE), m_oCompany.CompanyDB, m_oCompany.Server)
                        End If

                        'Elimino la ultima , para ejecutar el select
                        If Not String.IsNullOrEmpty(strListaIdRepXOrden) Then

                            strListaIdRepXOrden = Mid(strListaIdRepXOrden, 1, Len(strListaIdRepXOrden) - 1)

                            'Consulta que me trae todos los documentos destinos y el tipo
                            dtConsulta = Utilitarios.EjecutarConsultaDataTable(String.Format("SELECT TrgetEntry, TargetType FROM [POR1] with (nolock) where DocEntry = {0} and U_SCGD_IdRepxOrd in ({1})",
                                                                                             p_ListaTargetEntry.Item(i), strListaIdRepXOrden), m_oCompany.CompanyDB, m_oCompany.Server)


                            For Each dr As DataRow In dtConsulta.Rows

                                Select Case dr("TargetType")
                                    Case 18
                                        If Not String.IsNullOrEmpty(dr("TrgetEntry").ToString().Trim()) AndAlso dr.Item("TrgetEntry").ToString() <> 0 Then
                                            For a As Integer = 0 To ListaTargetEntryFacturaProveedor.Count
                                                If Not ListaTargetEntryFacturaProveedor.Contains(dr("TrgetEntry").ToString().Trim()) Then
                                                    ListaTargetEntryFacturaProveedor.Add(dr("TrgetEntry").ToString().Trim())
                                                End If
                                            Next

                                        End If

                                    Case 20
                                        If Not String.IsNullOrEmpty(dr("TrgetEntry").ToString().Trim()) AndAlso dr.Item("TrgetEntry").ToString() <> 0 Then
                                            For a As Integer = 0 To ListaTargetEntryEntradaMercancia.Count
                                                If Not ListaTargetEntryEntradaMercancia.Contains(dr("TrgetEntry").ToString().Trim()) Then
                                                    ListaTargetEntryEntradaMercancia.Add(dr("TrgetEntry").ToString().Trim())
                                                End If
                                            Next
                                        End If

                                End Select

                            Next
                        End If

                    End If


                    Utilitarios.DestruirObjeto(objDocumento)

                Next

                ActualizarNumeroOT_EnLineas_EntradaMercancia(ListaTargetEntryEntradaMercancia)

            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex

        End Try



    End Sub
    ''' <summary>
    ''' en caso de que tenga una Entrada de Mercancia asociada
    ''' no se hace el update con el objeto EntradaMercancia porque SAP no permite 
    ''' actualizar lineas de detalles para estos documentos
    ''' </summary>
    ''' <param name="p_ListaTargetEntry"></param>
    ''' <remarks></remarks>
    Private Sub ActualizarNumeroOT_EnLineas_EntradaMercancia(ByVal p_ListaTargetEntry As Generic.List(Of Integer), Optional ByVal p_bln_UsaTallerSAP As Boolean = False)

        Dim objEntradaMercancia As SAPbobsCOM.Documents
        Dim objEntradaMercanciaLineas As SAPbobsCOM.Document_Lines
        Dim strListaIdRepXOrden As String = String.Empty
        Dim dtConsulta As System.Data.DataTable


        Dim StrUPDATE As String = String.Empty

        If Not p_bln_UsaTallerSAP Then
            strNombreColumnaID = "U_SCGD_IdRepxOrd"
        Else
            strNombreColumnaID = "U_SCGD_ID"
        End If

        Try

            If p_ListaTargetEntry.Count > 0 Then

                For i As Integer = 0 To p_ListaTargetEntry.Count - 1

                    objEntradaMercancia = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes), SAPbobsCOM.Documents)

                    'Limpio los Strings
                    strListaIdRepXOrden = String.Empty
                    StrUPDATE = String.Empty

                    If objEntradaMercancia.GetByKey(p_ListaTargetEntry.Item(i)) Then

                        objEntradaMercanciaLineas = objEntradaMercancia.Lines

                        For lineas As Integer = 0 To objEntradaMercanciaLineas.Count - 1

                            objEntradaMercanciaLineas.SetCurrentLine(lineas)

                            If ListaItemsCodeOTEspeciales.Contains(objEntradaMercanciaLineas.UserFields.Fields.Item("ItemCode").Value) AndAlso
                                ListaIdRepxOrdenOTEspeciales.Contains(objEntradaMercanciaLineas.UserFields.Fields.Item(strNombreColumnaID).Value) Then

                                strListaIdRepXOrden = strListaIdRepXOrden + objEntradaMercanciaLineas.UserFields.Fields.Item(strNombreColumnaID).Value.ToString().Trim() + ","

                                StrUPDATE = StrUPDATE & String.Format("UPDATE [dbo].[PDN1] SET [U_SCGD_NoOT] = '{0}' WHERE docentry = {1} and ItemCode = '{2}' and " & strNombreColumnaID & " = '{3}' ",
                                                                        ListaNoOrdenOTEspeciales.Item(0),
                                                                        p_ListaTargetEntry(i),
                                                                        objEntradaMercanciaLineas.UserFields.Fields.Item("ItemCode").Value,
                                                                        objEntradaMercanciaLineas.UserFields.Fields.Item(strNombreColumnaID).Value)

                            End If

                        Next

                        'Ejecuto el Update una unica vez
                        If Not String.IsNullOrEmpty(StrUPDATE) Then
                            Utilitarios.EjecutarConsulta(String.Format(StrUPDATE), m_oCompany.CompanyDB, m_oCompany.Server)
                        End If

                        'Elimino la ultima , para ejecutar el select
                        If Not String.IsNullOrEmpty(strListaIdRepXOrden) Then
                            strListaIdRepXOrden = Mid(strListaIdRepXOrden, 1, Len(strListaIdRepXOrden) - 1)

                            'Consulta que me trae todos los documentos destinos y el tipo
                            dtConsulta = Utilitarios.EjecutarConsultaDataTable(String.Format("SELECT TrgetEntry FROM [PDN1] with (nolock) where DocEntry = {0} and " & strNombreColumnaID & " in ({1})",
                                                                                             p_ListaTargetEntry.Item(i), strListaIdRepXOrden), m_oCompany.CompanyDB, m_oCompany.Server)


                            For Each dr As DataRow In dtConsulta.Rows
                                If Not String.IsNullOrEmpty(dr("TrgetEntry").ToString().Trim()) AndAlso dr.Item("TrgetEntry").ToString() <> 0 Then
                                    For a As Integer = 0 To ListaTargetEntryFacturaProveedor.Count
                                        If Not ListaTargetEntryFacturaProveedor.Contains(dr("TrgetEntry").ToString().Trim()) Then
                                            ListaTargetEntryFacturaProveedor.Add(dr("TrgetEntry").ToString().Trim())
                                        End If
                                    Next
                                End If
                            Next
                        End If

                    End If


                    Utilitarios.DestruirObjeto(objEntradaMercancia)

                Next

            End If

            ActualizaNumeroOT_EnLineas_FacturaProveedor(ListaTargetEntryFacturaProveedor)

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex

        End Try

    End Sub

    Private Sub ActualizaNumeroOT_EnLineas_FacturaProveedor(ByVal p_ListaTargetEntry As Generic.List(Of Integer))

        Dim objFacturaProveedor As SAPbobsCOM.Documents
        Dim objFacturaProveedorLineas As SAPbobsCOM.Document_Lines

        Dim StrUPDATE As String = String.Empty

        Try
            If p_ListaTargetEntry.Count > 0 Then

                For i As Integer = 0 To p_ListaTargetEntry.Count - 1

                    objFacturaProveedor = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices), SAPbobsCOM.Documents)

                    StrUPDATE = String.Empty

                    If objFacturaProveedor.GetByKey(p_ListaTargetEntry.Item(i)) Then

                        objFacturaProveedorLineas = objFacturaProveedor.Lines

                        For lineas As Integer = 0 To objFacturaProveedorLineas.Count - 1
                            objFacturaProveedorLineas.SetCurrentLine(lineas)

                            If ListaItemsCodeOTEspeciales.Contains(objFacturaProveedorLineas.UserFields.Fields.Item("ItemCode").Value) AndAlso
                                ListaIdRepxOrdenOTEspeciales.Contains(objFacturaProveedorLineas.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value) Then

                                StrUPDATE = StrUPDATE & String.Format("UPDATE [dbo].[PCH1] SET [U_SCGD_NoOT] = '{0}' WHERE docentry = {1} and ItemCode = '{2}' and  U_SCGD_IdRepxOrd = {3} ",
                                    ListaNoOrdenOTEspeciales.Item(0),
                                    p_ListaTargetEntry(i),
                                    objFacturaProveedorLineas.UserFields.Fields.Item("ItemCode").Value,
                                    objFacturaProveedorLineas.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value)
                            End If

                        Next

                        If Not String.IsNullOrEmpty(StrUPDATE) Then
                            Utilitarios.EjecutarConsulta(String.Format(StrUPDATE), m_oCompany.CompanyDB, m_oCompany.Server)
                        End If

                    End If

                    Utilitarios.DestruirObjeto(objFacturaProveedor)

                Next

            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex

        Finally

            ListaItemsCodeOTEspeciales.Clear()
            ListaIdRepxOrdenOTEspeciales.Clear()
            ListaTargetEntryEntradaMercancia.Clear()
            ListaNoOrdenOTEspeciales.Clear()
            ListaTargetEntryFacturaProveedor.Clear()
            ListaTargetEntryOrdenCompra.Clear()

        End Try

    End Sub

    Private Function ObtenerCotizacionAnterior(Optional ByVal p_DocEntryCotRef As Integer = 0, Optional ByVal p_blnConf_TallerEnSAP As Boolean = False) As SAPbobsCOM.Documents

        Dim objCotizacion As SAPbobsCOM.Documents
        Dim cmdConsultarCotizaciones As New SqlClient.SqlCommand
        Dim drdConsultarCotizaciones As SqlClient.SqlDataReader
        Dim strConsulta As String
        Dim intDocEntry As Integer
        Dim intError As Integer
        Dim strMensaje As String = ""

        objCotizacion = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)


        If p_DocEntryCotRef = 0 Then

            If Not p_blnConf_TallerEnSAP Then
                strConsulta = "Select DocEntry from SCGTA_VW_OQUT with(nolock) WHERE " + mc_strNum_OT + " = '" + m_strOTPadre & "'"

                m_cnnSCGTaller = New SqlClient.SqlConnection(strCadenaConexionBDTaller)
                m_cnnSCGTaller.Open()
                cmdConsultarCotizaciones.Connection = m_cnnSCGTaller
                cmdConsultarCotizaciones.CommandText = strConsulta
                cmdConsultarCotizaciones.CommandType = CommandType.Text
                drdConsultarCotizaciones = cmdConsultarCotizaciones.ExecuteReader()

                Do While drdConsultarCotizaciones.Read
                    intDocEntry = CInt(drdConsultarCotizaciones.Item("DocEntry"))
                    If intDocEntry > 0 Then

                        If objCotizacion.GetByKey(intDocEntry) = False Then

                            m_oCompany.GetLastError(intError, strMensaje)

                            Throw New ExceptionsSBO(intError, strMensaje)

                        End If

                    End If
                Loop
            Else
                strConsulta = Utilitarios.EjecutarConsulta("Select DocEntry from OQUT with(nolock) WHERE " + mc_strNum_OT + " = '" + m_strOTPadre & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                intDocEntry = CInt(strConsulta)

                If intDocEntry > 0 Then

                    If objCotizacion.GetByKey(intDocEntry) = False Then

                        m_oCompany.GetLastError(intError, strMensaje)

                        Throw New ExceptionsSBO(intError, strMensaje)

                    End If

                End If

            End If


        Else
            If objCotizacion.GetByKey(p_DocEntryCotRef) = False Then
                m_oCompany.GetLastError(intError, strMensaje)
                Throw New ExceptionsSBO(intError, strMensaje)
            End If
        End If
        Return objCotizacion

    End Function

    Public Function RealizarCosteo(ByVal p_strNoDoc As String, ByVal p_blnEsFactura As Boolean) As Boolean

        Dim strServiciosExternosInventariables As String
        Dim strCosteoManoObra As String

        Dim strConsultaFacturaPrimeraParte As String = "Select INV1.DocEntry, INV1.ItemCode, Items.Tipo,  " & _
                                                        "Isnull(INV1.U_SCGD_Costo,0) U_SCGD_Costo, Items.NoOrden,  " & _
                                                        "Case Tipo when 4 then isnull(dbo.SCGTA_FC_CuentaGastosxTipoOrden(INV1.ItemCode,Orden.CodTipoOrden),dbo.SCGTA_FC_CuentaGastos(INV1.ItemCode)) " & _
                                                         "else isnull(dbo.SCGTA_FC_CuentaExistenciaxTipoOrden (INV1.ItemCode,Orden.CodTipoOrden),dbo.SCGTA_FC_CuentaExistencia (INV1.ItemCode)) end CuentaCredito,  " & _
                                                        "isnull(dbo.SCGTA_FC_CuentaCostosxTipoOrden(INV1.ItemCode,Orden.CodTipoOrden),dbo.SCGTA_FC_CuentaCostos(INV1.ItemCode)) CuentaDebito  " & _
                                                        "from dbo.SCGTA_VW_INV1 INV1  " & _
                                                        "Inner join dbo.SCGTA_VW_OITM OITM on oitm.ItemCode = INV1.ItemCode " & _
                                                        "INNER JOIN ("

        Dim strConsultaEntregaPrimeraParte As String = "Select INV1.DocEntry, INV1.ItemCode, Items.Tipo, " & _
                                                        "Isnull(INV1.U_SCGD_Costo,0) U_SCGD_Costo, Items.NoOrden, " & _
                                                        "Case Tipo when 4 then isnull(dbo.SCGTA_FC_CuentaGastosxTipoOrden(INV1.ItemCode,Orden.CodTipoOrden),dbo.SCGTA_FC_CuentaGastos(INV1.ItemCode)) " & _
                                                         "else isnull(dbo.SCGTA_FC_CuentaExistenciaxTipoOrden (INV1.ItemCode,Orden.CodTipoOrden),dbo.SCGTA_FC_CuentaExistencia (INV1.ItemCode)) end CuentaCredito, " & _
                                                        "isnull(dbo.SCGTA_FC_CuentaCostosxTipoOrden(INV1.ItemCode,Orden.CodTipoOrden),dbo.SCGTA_FC_CuentaCostos(INV1.ItemCode)) CuentaDebito " & _
                                                        "from dbo.SCGTA_VW_DLN1 INV1 Inner join dbo.SCGTA_VW_OITM OITM on oitm.ItemCode = INV1.ItemCode INNER JOIN ("

        Dim strConsultaSegundaParte As String = ") Items on  INV1.U_SCGD_IDRepxOrd = Items.ID and OITM.U_SCGD_TipoArticulo = Items.Tipo " & _
                                                "inner join SCGTA_TB_Orden Orden on Orden.NoOrden = Items.NoOrden where INV1.U_SCGD_Costo Is Not null and INV1.U_SCGD_Costo > 0 " & _
                                                " and INV1.BaseType = 17 and INV1.DocEntry = "

        Dim strConsultaActividades As String = "Select ID,NoActividad,2 as Tipo, NoOrden, LineNum from dbo.SCGTA_TB_ActividadesxOrden "
        Dim strConsultaServiciosExternos As String = "Select ID, NoRepuesto, 4 as Tipo, NoOrden, LineNum  from dbo.SCGTA_TB_RepuestosxOrden "
        Dim strConsulta As String
        Dim blnEjecutarConsulta As Boolean = False

        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Dim cmdContraCuentas As New SqlClient.SqlCommand
        Dim drdContraCuentas As SqlClient.SqlDataReader
        Dim oJournalEntry As SAPbobsCOM.JournalEntries
        Dim decTotal As Decimal
        Dim blnPrimeraCuenta As Boolean = True
        Dim blnEntradaInvalida As Boolean
        Dim intError As Integer
        Dim strMensajeError As String = ""
        Dim strNumeroFactura As String

        Dim strCentroBeneficio As String = String.Empty
        Dim strConexionDBSucursal As String = ""

        Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application,
                                                      strIdSucursal,
                                                      strConexionDBSucursal)

        Utilitarios.DevuelveNombreBDTaller(SBO_Application, strIdSucursal, m_strBDTalller)
        strServiciosExternosInventariables = Utilitarios.EjecutarConsulta("Select Valor from SCGTA_TB_Configuracion where Propiedad = 'SEInventariables'", m_strBDTalller, m_oCompany.Server)
        strCosteoManoObra = Utilitarios.EjecutarConsulta("Select Valor from SCGTA_TB_Configuracion where Propiedad = 'CosteoServicios'", m_strBDTalller, m_oCompany.Server)
        If String.IsNullOrEmpty(strServiciosExternosInventariables) Then
            strServiciosExternosInventariables = 0
        End If

        If p_blnEsFactura Then
            strConsulta = strConsultaFacturaPrimeraParte
        Else
            strConsulta = strConsultaEntregaPrimeraParte
        End If
        If strCosteoManoObra <> "0" Then
            strConsulta &= strConsultaActividades
            blnEjecutarConsulta = True
        End If
        If Not CBool(strServiciosExternosInventariables) Then
            If blnEjecutarConsulta Then
                strConsulta &= " union " & strConsultaServiciosExternos
            Else
                strConsulta &= strConsultaServiciosExternos
                blnEjecutarConsulta = True
            End If
        End If
        strConsulta &= strConsultaSegundaParte & p_strNoDoc
        If blnEjecutarConsulta Then
            oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_strBDTalller, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString
            cn_Coneccion.Open()

            cmdContraCuentas.Connection = cn_Coneccion

            cmdContraCuentas.CommandType = CommandType.Text
            cmdContraCuentas.CommandText = strConsulta
            drdContraCuentas = cmdContraCuentas.ExecuteReader()

            decTotal = 0

            Do While drdContraCuentas.Read
                If Not blnPrimeraCuenta Then
                    oJournalEntry.Lines.Add()
                Else
                    blnPrimeraCuenta = False
                End If

                strNumeroFactura = drdContraCuentas.GetValue(0)
                oJournalEntry.Lines.AccountCode = drdContraCuentas.GetString(5)

                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.Credit = drdContraCuentas.GetDecimal(3)
                oJournalEntry.Reference = drdContraCuentas.GetString(4)
                oJournalEntry.Reference2 = drdContraCuentas.GetString(1)
                decTotal += drdContraCuentas.GetDecimal(3)

                strCentroBeneficio = ConfiguracionDataAdapter.RetornaCentroBeneficioByNoOrden(drdContraCuentas.GetString(4), strConexionDBSucursal)
                If String.IsNullOrEmpty(strCentroBeneficio) Then
                    strCentroBeneficio = ConfiguracionDataAdapter.RetornaCentroBeneficioByItem(drdContraCuentas.GetString(1), strConexionDBSucursal)
                End If
                If Not String.IsNullOrEmpty(strCentroBeneficio) Then oJournalEntry.Lines.CostingCode = strCentroBeneficio

                oJournalEntry.Lines.Add()
                oJournalEntry.Lines.AccountCode = drdContraCuentas.GetString(6)
                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.Reference1 = drdContraCuentas.GetString(4)
                oJournalEntry.Lines.Reference2 = drdContraCuentas.GetString(1)
                oJournalEntry.Lines.Debit = drdContraCuentas.GetDecimal(3)

                '                strCentroBeneficio = ConfiguracionDataAdapter.RetornaCentroBeneficioByNoOrden(drdContraCuentas.GetString(4), strConexionDBSucursal)
                '                If String.IsNullOrEmpty(strCentroBeneficio) Then
                '                    strCentroBeneficio = ConfiguracionDataAdapter.RetornaCentroBeneficioByItem(drdContraCuentas.GetString(1), strConexionDBSucursal)
                '                End If
                If Not String.IsNullOrEmpty(strCentroBeneficio) Then oJournalEntry.Lines.CostingCode = strCentroBeneficio
            Loop
            ' oJournalEntry.Reference = strNumeroFactura
            drdContraCuentas.Close()
            cn_Coneccion.Close()
            If oJournalEntry.Add <> 0 Then
                If decTotal = 0 Then
                    blnEntradaInvalida = True
                Else
                    m_oCompany.GetLastError(intError, strMensajeError)
                    If m_oCompany.InTransaction Then
                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    End If
                    Throw New ExceptionsSBO(intError, strMensajeError)
                End If
            Else
                If m_oCompany.InTransaction Then
                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                End If
            End If
        End If

        Return blnEntradaInvalida

    End Function

    Private Sub ActualizarListasRep(ByRef p_lstCantLineasAnt As Generic.List(Of stTipoListaCantAnteriores), ByRef p_dtbRepXOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable)
        Dim intContCant As Integer
        Dim drwRepXOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow

        For intContCant = 0 To p_lstCantLineasAnt.Count - 1

            For Each drwRepXOrden In p_dtbRepXOrden

                If drwRepXOrden.LineNum = p_lstCantLineasAnt(intContCant).LineNum AndAlso _
                    drwRepXOrden.NoRepuesto = p_lstCantLineasAnt(intContCant).ItemCode Then

                    If drwRepXOrden.IsCantidadLineasAnteNull Then
                        drwRepXOrden.CantidadLineasAnte = p_lstCantLineasAnt(intContCant).Cantidad
                    Else
                        drwRepXOrden.CantidadLineasAnte += p_lstCantLineasAnt(intContCant).Cantidad
                    End If

                    Exit For

                End If

            Next

        Next

    End Sub

    Private Sub ActualizarListasSum(ByRef p_lstCantLineasAnt As Generic.List(Of stTipoListaCantAnteriores), ByRef p_dtbSumXOrden As SuministrosDataset.SCGTA_VW_SuministrosDataTable)
        Dim intContCant As Integer
        Dim drwSumXOrden As SuministrosDataset.SCGTA_VW_SuministrosRow

        For intContCant = 0 To p_lstCantLineasAnt.Count - 1

            For Each drwSumXOrden In p_dtbSumXOrden

                If drwSumXOrden.LineNum = p_lstCantLineasAnt(intContCant).LineNum AndAlso _
                    drwSumXOrden.NoSuministro = p_lstCantLineasAnt(intContCant).ItemCode Then

                    If drwSumXOrden.IsCantidadLineasAnteNull Then
                        drwSumXOrden.CantidadLineasAnte = p_lstCantLineasAnt(intContCant).Cantidad
                    Else
                        drwSumXOrden.CantidadLineasAnte += p_lstCantLineasAnt(intContCant).Cantidad
                    End If

                    Exit For

                End If

            Next

        Next

        'p_lstCantLineasAnt.Clear()

    End Sub
    Public Sub ActualizarLineasDeLaCotizacion(ByVal p_DocEntryCotizacion As Integer, ByVal p_oDocumentsLine As SAPbobsCOM.StockTransfer_Lines, ByVal p_listLineNum As Generic.List(Of Integer))

        Try
            Dim m_oBuscarCotizacion As SAPbobsCOM.Documents
            Dim m_oLineasCotizacion As SAPbobsCOM.Document_Lines

            m_oBuscarCotizacion = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            If m_oBuscarCotizacion.GetByKey(p_DocEntryCotizacion) Then

                m_oLineasCotizacion = m_oBuscarCotizacion.Lines

                For m As Integer = 0 To p_listLineNum.Count - 1

                    For i As Integer = 0 To m_oLineasCotizacion.Count - 1

                        m_oLineasCotizacion.SetCurrentLine(i)

                        If p_listLineNum(m) = m_oLineasCotizacion.LineNum Then
                            m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 2
                            Exit For
                        End If
                    Next

                Next

                m_oBuscarCotizacion.Update()

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex

        End Try
    End Sub


    'Cambios actualización estado repuestos requisicion
    Private Sub ActualizarEstadoRepuestosDesdeCotizacion(ByVal p_oDocuments As SAPbobsCOM.Documents)

        Dim intDocEntryCotizacion As Integer
        Dim oDocumentsLine As SAPbobsCOM.Document_Lines
        Dim ListaLineNumPB As New Generic.List(Of Integer)
        Dim strNumeroOT As String = String.Empty

        Dim m_dstRepuestosAdicionalesxOrden As RepuestosxOrdenDataset
        Dim m_adpRepuestosAdicionalesxOrden As RepuestosxOrdenDataAdapter
        'Pasar para arriba
        m_dstRepuestosAdicionalesxOrden = New RepuestosxOrdenDataset
        m_adpRepuestosAdicionalesxOrden = New RepuestosxOrdenDataAdapter(strCadenaConexionBDTaller)
        m_adpRepuestosAdicionalesxOrden.FillRepuestosxOrdenAdicionales(m_dstRepuestosAdicionalesxOrden, m_strNoOrden)



        intDocEntryCotizacion = p_oDocuments.DocEntry
        strNumeroOT = p_oDocuments.UserFields.Fields.Item("U_SCGD_Numero_OT").Value




        oDocumentsLine = p_oDocuments.Lines
        If m_dstRepuestosAdicionalesxOrden.SCGTA_TB_RepuestosxOrden.Rows.Count > 0 Then
            For i As Integer = 0 To oDocumentsLine.Count - 1

                oDocumentsLine.SetCurrentLine(i)
                If oDocumentsLine.UserFields.Fields.Item("U_SCGD_Traslad").Value = 3 And oDocumentsLine.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 1 Then
                    For Each drwRep As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow In m_dstRepuestosAdicionalesxOrden.SCGTA_TB_RepuestosxOrden.Rows

                        If oDocumentsLine.LineNum = drwRep.LineNum Then
                            drwRep.CodEstadoRep = 5
                            Exit For
                        End If
                    Next
                ElseIf oDocumentsLine.UserFields.Fields.Item("U_SCGD_Traslad").Value = 4 And oDocumentsLine.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 1 Then
                    For Each drwRep As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow In m_dstRepuestosAdicionalesxOrden.SCGTA_TB_RepuestosxOrden.Rows

                        If oDocumentsLine.LineNum = drwRep.LineNum Then
                            drwRep.CodEstadoRep = 6
                            Exit For
                        End If
                    Next
                End If

            Next
            m_adpRepuestosxOrden.UpdateCodigoRepuesto(m_dstRepuestosAdicionalesxOrden)

        End If

    End Sub




    '-------------Para drafts--------
    ''' <summary>
    ''' Carga la cotizacion y modifica las lineas lineas en la orden de Trabajo que sean Adicionales
    ''' para cambiar su estado de "Cant. Pendiente" a "Pendiente Bodega"
    ''' </summary>
    ''' <param name="p_oDocuments">Cotizacion Actual</param>
    ''' <remarks></remarks>
    Private Sub ActualizarLineasOrdenTrabajoDesdeCotizacion(ByVal p_oDocuments As SAPbobsCOM.Documents, ByVal p_dstRepuestosxOrden As RepuestosxOrdenDataset)

        Dim intDocEntryCotizacion As Integer
        Dim oDocumentsLine As SAPbobsCOM.Document_Lines
        Dim ListaLineNumPB As New Generic.List(Of Integer)
        Dim strNumeroOT As String = String.Empty


        intDocEntryCotizacion = p_oDocuments.DocEntry
        strNumeroOT = p_oDocuments.UserFields.Fields.Item("U_SCGD_Numero_OT").Value

        oDocumentsLine = p_oDocuments.Lines

        For i As Integer = 0 To oDocumentsLine.Count - 1
            oDocumentsLine.SetCurrentLine(i)
            If oDocumentsLine.UserFields.Fields.Item("U_SCGD_Traslad").Value = 4 And oDocumentsLine.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 1 Then
                ListaLineNumPB.Add(oDocumentsLine.LineNum)
            End If
        Next

        If ListaLineNumPB.Count = 0 Then
            Exit Sub
        End If

        For m As Integer = 0 To ListaLineNumPB.Count - 1

            For Each drwRep As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow In p_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows 'm_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows

                If ListaLineNumPB(m) = drwRep.LineNum Then
                    If drwRep.Adicional = 1 Then
                        With drwRep
                            .CodEstadoRep = 6
                        End With
                        Exit For
                    Else
                        drwRep.CodEstadoRep = 6
                        Exit For
                    End If

                End If

            Next

        Next
        ' Next
        m_adpRepuestosxOrden.UpdateCodigoRepuesto(m_dstRepuestosxOrden)

    End Sub


    Private Sub ActualizarLineasAdicionales(ByVal p_oDocuments As SAPbobsCOM.Documents) ', ByVal p_dstRepuestosxOrden As RepuestosxOrdenDataset)

        Dim intDocEntryCotizacion As Integer
        Dim oDocumentsLine As SAPbobsCOM.Document_Lines
        Dim ListaLineNumAdicionales As New Generic.List(Of Integer)
        Dim strNumeroOT As String = String.Empty


        intDocEntryCotizacion = p_oDocuments.DocEntry
        strNumeroOT = p_oDocuments.UserFields.Fields.Item("U_SCGD_Numero_OT").Value

        oDocumentsLine = p_oDocuments.Lines

        For i As Integer = 0 To oDocumentsLine.Count - 1
            oDocumentsLine.SetCurrentLine(i)
            If oDocumentsLine.UserFields.Fields.Item("U_SCGD_Traslad").Value = 3 And oDocumentsLine.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 1 Then
                ListaLineNumAdicionales.Add(oDocumentsLine.LineNum)
            End If
        Next

        If ListaLineNumAdicionales.Count = 0 Then
            Exit Sub
        End If

        Dim strCadenaConexionBDTaller As String = ""

        ' Private m_dstOrdenTrabajoAnterior As OrdenTrabajoDataset
        Dim m_adpOrdenTrabajo As New DMSOneFramework.SCGDataAccess.OrdenTrabajoDataAdapter
        Dim m_dstOrdenTrabajo As New DMSOneFramework.OrdenTrabajoDataset

        Dim m_dstRepuestosxOrden As DMSOneFramework.RepuestosxOrdenDataset
        Dim m_adpRepuestosxOrden As DMSOneFramework.SCGDataAccess.RepuestosxOrdenDataAdapter

        Dim m_dstSuministrosxOrden As DMSOneFramework.SuministrosDataset
        Dim m_adpSuministrosxOrden As DMSOneFramework.SCGDataAccess.SuministrosDataAdapter

        Dim m_dstActividadesxOrden As DMSOneFramework.ActividadesXFaseDataset
        Dim m_adpActividadesxOrden As ActividadesXFaseDataAdapter

        'Actualización de la cotización

        m_dstActividadesxOrden = Nothing
        m_dstRepuestosxOrden = Nothing
        m_dstSuministrosxOrden = Nothing

        Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application,
                                                      strIdSucursal,
                                                      strCadenaConexionBDTaller)

        m_dstRepuestosxOrden = New DMSOneFramework.RepuestosxOrdenDataset
        m_adpRepuestosxOrden = New RepuestosxOrdenDataAdapter(strCadenaConexionBDTaller)

        m_dstSuministrosxOrden = New DMSOneFramework.SuministrosDataset
        m_adpSuministrosxOrden = New SuministrosDataAdapter(strCadenaConexionBDTaller)

        m_dstActividadesxOrden = New DMSOneFramework.ActividadesXFaseDataset
        m_adpActividadesxOrden = New ActividadesXFaseDataAdapter(strCadenaConexionBDTaller)


        m_dstOrdenTrabajo.EnforceConstraints = False
        m_adpOrdenTrabajo.Fill_x_OrdenTrabajo(m_dstRepuestosxOrden, strNumeroOT)

        For m As Integer = 0 To ListaLineNumAdicionales.Count - 1

            For Each drwRep As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow In m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows

                If ListaLineNumAdicionales(m) = drwRep.LineNum Then
                    If drwRep.Adicional = 1 Then
                        With drwRep
                            .CodEstadoRep = 5
                        End With
                        Exit For
                    Else
                        Exit For
                    End If

                End If

            Next

        Next
        ' Next
        m_adpRepuestosxOrden.UpdateCodigoRepuesto(m_dstRepuestosxOrden)

    End Sub

    Public Sub AsignarCodTecnicoAColaborador(Of T As {System.Data.DataTable})(ByVal p_dtbActividadesXOrden As T, ByRef cn As SqlClient.SqlConnection, _
                                         ByRef tran As SqlClient.SqlTransaction)


        Dim objDA As New SCGDataAccess.ColaboradorDataAdapter
        Dim dtsAsignados As New ColaboradorDataset
        Dim dtrAsignando As ColaboradorDataset.SCGTA_TB_ControlColaboradorRow

        For i As Integer = 0 To p_dtbActividadesXOrden.Rows.Count - 1

            Dim strNoOrden As String = p_dtbActividadesXOrden.Rows(i)("NoOrden")
            Dim strLineNum As String = p_dtbActividadesXOrden.Rows(i)("LineNum")
            Dim strNoActividad As String = p_dtbActividadesXOrden.Rows(i)("ID")

            Dim CodFase As Integer = ObtenerFase(strNoOrden, strLineNum, strNoActividad, cn, tran)

            dtrAsignando = dtsAsignados.SCGTA_TB_ControlColaborador.NewSCGTA_TB_ControlColaboradorRow

            With dtrAsignando
                .NoFase = CodFase
                .NoOrden = strNoOrden
                .Reproceso = 0
                .EmpID = m_intCodigoTecnico
                .TiempoHoras = 0
                .Estado = "No iniciado"
                .Costo = 0
                .IDActividad = strNoActividad
            End With

            dtsAsignados.SCGTA_TB_ControlColaborador.AddSCGTA_TB_ControlColaboradorRow(dtrAsignando)

            objDA.InsertarNuevo(dtsAsignados)
        Next


    End Sub

    Public Function ObtenerFase(ByVal p_Orden As String, ByVal p_LineNum As Integer, ByVal p_NoActividad As String, ByRef p_cn As SqlClient.SqlConnection, _
                                         ByRef p_tran As SqlClient.SqlTransaction) As Integer


        Dim strConexionDBSucursal As String = ""

        Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application,
                                                       strIdSucursal,
                                                       strConexionDBSucursal)


        Utilitarios.DevuelveNombreBDTaller(SBO_Application, strIdSucursal, m_strBDTalller)

        Dim CodigoFase As Integer = Utilitarios.EjecutarConsultaCodigos("select Art.U_SCGD_T_Fase from dbo.SCGTA_VW_OITM as Art " & _
                                                                 "inner join dbo.SCGTA_TB_ActividadesxOrden as AxO " & _
                                                                 "on Art.ItemCode = AxO.NoActividad " & _
                                                                 "where NoOrden = '" & p_Orden & "' and LineNum = '" & p_LineNum & "'", m_strBDTalller, m_oCompany.Server, p_cn, p_tran)


        Return CodigoFase

    End Function

    Public Function FilaTieneNumeroOT(ByVal p_form As SAPbouiCOM.Form, ByVal row As Integer) As Boolean

        Dim oitem As SAPbouiCOM.Item
        Dim oitemCombo As SAPbouiCOM.Item
        Dim oEditText As SAPbouiCOM.EditText
        Dim oCombo As SAPbouiCOM.ComboBox

        Dim strNoOT As String = String.Empty
        Dim intGeneraOT As Integer = 0
        Dim r As Integer = row

        If row <> -1 And row <> 0 Then
            oitem = p_form.Items.Item("SCGD_etOT")
            oEditText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
            strNoOT = oEditText.String

            oitemCombo = p_form.Items.Item("SCGD_cbGOT")
            oCombo = DirectCast(oitemCombo.Specific, SAPbouiCOM.ComboBox)
            intGeneraOT = oCombo.Value

            If Not String.IsNullOrEmpty(strNoOT) AndAlso intGeneraOT = 1 Then
                Return True
            Else
                Return False
            End If
        End If

    End Function


    Public Function ValidarCentroCosto(ByRef strItemcode As String, _
                                       ByRef p_Oitem As SAPbobsCOM.IItems, _
                                       Optional ByVal p_strIdSucursal As String = "") As Boolean

        Try
            Dim strCentroCosto As String = String.Empty
            Dim strBodegaProcesoItem As String = String.Empty
            Dim strIDSucu As String = String.Empty

            If p_Oitem Is Nothing Then
                p_Oitem = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
                p_Oitem.GetByKey(strItemcode)
            End If

            strCentroCosto = p_Oitem.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString.Trim

            If IsNumeric(strCentroCosto) Then

                If Integer.TryParse(strCentroCosto, Nothing) Then

                    If String.IsNullOrEmpty(p_strIdSucursal) Then
                        strBodegaProcesoItem = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, TransferenciaItems.mc_strBodegaProceso, strIdSucursal, SBO_Application).Trim
                    Else
                        strBodegaProcesoItem = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, SBO_Application).Trim
                    End If

                    If String.IsNullOrEmpty(strBodegaProcesoItem) Then
                        Return False
                    Else
                        For index As Integer = 0 To p_Oitem.WhsInfo.Count - 1
                            p_Oitem.WhsInfo.SetCurrentLine(index)
                            'valida la existencia de la bodega de proceso para ese item
                            If p_Oitem.WhsInfo.WarehouseCode = strBodegaProcesoItem Then
                                Return True
                            End If
                        Next
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.El_Item + strItemcode + My.Resources.Resource.NoEncontradoEnAlmacen + strBodegaProcesoItem,
                                                                                      SAPbouiCOM.BoMessageTime.bmt_Medium,
                                                                                      SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return False
                    End If
                Else
                    Return False
                End If
            Else
                Return False
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Function

    Private Sub ValidaTiempoEstandar(ByVal strTiempoEstandarItem As String, ByRef blTiempoEstandar As Boolean, ByVal strItemCode As String)
        Dim strUsaAsocxEspc As String = String.Empty

        strUsaAsocxEspc = Utilitarios.EjecutarConsulta("Select U_UsaAXEV from [@SCGD_ADMIN] with(nolock)", m_oCompany.CompanyDB, m_oCompany.Server)

        If strUsaAsocxEspc.Equals("Y") Then
            Dim strConsulta As String
            Dim strDuracion As String = ""

            If m_EspecifVehi.Equals("E") Then

                Dim strCodEstilo As String = m_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value
                strConsulta = String.Format("Select oi.U_SCGD_duracion From OITM as oi with(nolock) inner join [@SCGD_ARTXESP] as art with(nolock) on oi.ItemCode  = art.U_ItemCode " &
                                            " where art.U_ItemCode = '{0}' and art.U_CodEsti = '{1}' and art.U_TipoArt = '2'", strItemCode, strCodEstilo)
                strDuracion = Utilitarios.EjecutarConsulta(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)

            ElseIf m_EspecifVehi.Equals("M") Then

                Dim strCodModelo As String = m_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value
                strConsulta = String.Format("Select oi.U_SCGD_duracion From OITM as oi with(nolock) inner join [@SCGD_ARTXESP] as art with(nolock) on oi.ItemCode  = art.U_ItemCode " &
                                            " where art.U_ItemCode = '{0}' and art.U_CodMod = '{1}' and art.U_TipoArt = '2'", strItemCode, strCodModelo)
                strDuracion = Utilitarios.EjecutarConsulta(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)

            End If

            If Not String.IsNullOrEmpty(strDuracion) Then
                blTiempoEstandar = True
            Else
                blTiempoEstandar = False
            End If
        Else
            If String.IsNullOrEmpty(strTiempoEstandarItem) Then
                blTiempoEstandar = False
            Else
                If Integer.Parse(strTiempoEstandarItem) = 0 Then
                    blTiempoEstandar = False
                Else
                    blTiempoEstandar = True
                End If
            End If
        End If

    End Sub

    Private Sub ReasignarTiemposOTHijayPadre(ByVal p_strOTPadre As String, ByVal p_strOrdenTrabajoHija As String)

        Dim dtServicios As New System.Data.DataTable
        Dim dtCostosOT As New System.Data.DataTable
        Dim DecCosto As Decimal = 0
        Dim DecCostoEst As Decimal = 0
        Dim strCosto As String
        Dim strCostoEst As String

        Dim dstOrdenTrabajoHija As New OrdenHijaActualizaCostosDataset
        Dim dtaOrdenTrabajoHija As New OrdenHijaActualizaCostosDatasetTableAdapters.OrdenHijaTableAdapter
        Dim drwOrdenTrabajoHija As OrdenHijaActualizaCostosDataset.OrdenHijaRow

        dtaOrdenTrabajoHija.Connection = m_cnnSCGTaller

        'Servicios de la Orden HIJA
        dtServicios = objUtilitarios.RetornaDataTable(String.Format("SELECT IDActividad AS IDHija,SUM([Costo]) AS COSTO, SUM([CostoEstandar]) AS COSTOESTANDAR FROM [dbo].[SCGTA_TB_ControlColaborador] WHERE NoOrden = '{0}' GROUP BY IDActividad ",
                                                             p_strOrdenTrabajoHija))

        dtaOrdenTrabajoHija.Fill(dstOrdenTrabajoHija.OrdenHija, p_strOrdenTrabajoHija)

        drwOrdenTrabajoHija = dstOrdenTrabajoHija.OrdenHija.Rows(0)

        For Each dr As DataRow In dtServicios.Rows

            If Not String.IsNullOrEmpty(dr("COSTO")) Then
                DecCosto += dr("COSTO")
            Else
                DecCosto += 0
            End If

            If Not String.IsNullOrEmpty(dr("COSTOESTANDAR")) Then
                DecCostoEst += dr("COSTOESTANDAR")
            Else
                DecCostoEst += 0
            End If

        Next

        'Asigno los montos
        With drwOrdenTrabajoHija

            .CostoManoObra = DecCosto
            .CostoManoObraEst = DecCostoEst

        End With

        'Actulizo el costo real y estandar de Tabla TB.Orden de la Orden de Trabajo HIJA 
        dtaOrdenTrabajoHija.Update(dstOrdenTrabajoHija.OrdenHija)


        'Variables para la actualizacion de los Costos de los Servicios de la TB.Orden Padre
        Dim decCostoPadre As Decimal = 0
        Dim decCostoEstPadre As Decimal = 0
        Dim strCostoPadre As String = String.Empty
        Dim strCostoEstPadre As String = String.Empty

        'Obtengo los costos Estandar y Real de la OT Padre
        dtCostosOT = objUtilitarios.RetornaDataTable(String.Format("SELECT IDActividad AS IDPadre,SUM([Costo]) AS COSTOREAL, SUM([CostoEstandar]) AS COSTOESTANDAR FROM [dbo].[SCGTA_TB_ControlColaborador] WHERE NoOrden = '{0}' GROUP BY IDActividad ",
                                                             p_strOTPadre))

        'Validar que la OT padre tenga al menos 1 Servicio para hacer el Update sino pone los costos en 0
        If (dtCostosOT.Rows.Count = 0) Then
            decCostoPadre = 0
            decCostoEstPadre = 0
        Else
            For Each dr As DataRow In dtCostosOT.Rows

                If Not String.IsNullOrEmpty(dr("COSTOREAL")) Then
                    decCostoPadre += dr("COSTOREAL")
                Else
                    decCostoPadre += 0
                End If

                If Not String.IsNullOrEmpty(dr("COSTOESTANDAR")) Then
                    decCostoEstPadre += dr("COSTOESTANDAR")
                Else
                    decCostoEstPadre += 0
                End If

            Next
        End If

        'Convierto el decimal a String
        strCostoEstPadre = decCostoEstPadre.ToString()
        strCostoPadre = decCostoPadre.ToString()

        'Elimino la "," por un "." para actualizar en BD
        strCostoEst = strCostoEstPadre.Replace(",", ".")
        strCosto = strCostoPadre.Replace(",", ".")

        'Actulizo el costo real y estandar de Tabla TB.Orden de la Orden de Trabajo PADRE 
        Utilitarios.EjecutarConsulta(String.Format("UPDATE dbo.SCGTA_TB_Orden SET CostoManoObraEst = '{0}', CostoManoObra = '{1}' WHERE NoOrden = '{2}'",
                                                strCostoEst, strCosto, p_strOTPadre), m_strBDTalller, m_oCompany.Server)


    End Sub

    Public Function ValidarKilometraje_HorasServicio(ByRef p_BubbleEvent As Boolean) As Boolean

        Dim BanderaKilometraje As Boolean = False
        Dim BanderaHoraServicio As Boolean = False
        Dim decHoraServicioActual As Decimal
        Dim decHoraServicioBD As Decimal
        Dim decKilometrajeActual As Decimal
        Dim decKilometrajeBD As Decimal
        'carga los valores para Validar Kilometraje y Validar Horas Servicio
        Dim dtConfiguracionDMS As System.Data.DataTable
        Dim drwConfiguracionDMS As System.Data.DataRow

        If m_oFormGenCotizacion.Mode = BoFormMode.fm_UPDATE_MODE OrElse m_oFormGenCotizacion.Mode = BoFormMode.fm_ADD_MODE Then
            n = DIHelper.GetNumberFormatInfo(m_oCompany)
            Dim strIdSucursal As String = m_oFormGenCotizacion.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).TrimEnd

            g_dtConsulta = m_oFormGenCotizacion.DataSources.DataTables.Item(g_strDtConsul)
            dtConfiguracionDMS = New System.Data.DataTable

            dtConfiguracionDMS = Utilitarios.EjecutarConsultaDataTable(String.Format("select U_ValKm, U_ValHS from [@SCGD_CONF_SUCURSAL] with(nolock) where U_Sucurs = '{0}'", strIdSucursal),
                                                m_oCompany.CompanyDB, m_oCompany.Server)

            If dtConfiguracionDMS.Rows.Count <> 0 Then

                drwConfiguracionDMS = dtConfiguracionDMS.Rows.Item(0)

                Dim strUnidad As String = m_oFormGenCotizacion.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Cod_Unidad", 0).TrimEnd
                strIDVehiculoHS_KM = Utilitarios.EjecutarConsulta(String.Format("select Code from [@SCGD_VEHICULO] with(nolock) where U_Cod_Unid = '{0}'", strUnidad),
                                                                                      m_oCompany.CompanyDB, m_oCompany.Server).ToString().Trim()
                If IsDBNull(drwConfiguracionDMS.Item("U_ValKm")) Then
                    drwConfiguracionDMS.Item("U_ValKm") = "N"
                End If

                If drwConfiguracionDMS.Item("U_ValKm") <> "Y" Then
                    BanderaKilometraje = False
                Else

                    Dim strKilometrajeActual As String = m_oFormGenCotizacion.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Kilometraje", 0).TrimEnd

                    g_dtConsulta.ExecuteQuery(String.Format("select U_Km_Unid from [@SCGD_VEHICULO] with(nolock) where U_Cod_Unid = '{0}'", strUnidad))

                    Dim strKilometrajeBD As String = g_dtConsulta.GetValue(0, 0).ToString()
                    If String.IsNullOrEmpty(strKilometrajeActual) Then
                        strKilometrajeActual = 0
                    End If

                    If String.IsNullOrEmpty(strKilometrajeBD) Then
                        strKilometrajeBD = 0
                    End If

                    decKilometrajeActual = Utilitarios.ConvierteDecimal(strKilometrajeActual.ToString(n), n)
                    decKilometrajeBD = Utilitarios.ConvierteDecimal(strKilometrajeBD.ToString(n), n)

                    If decKilometrajeActual < decKilometrajeBD Then
                        BanderaKilometraje = True
                    ElseIf decKilometrajeActual = decKilometrajeBD Then

                        blnActualizaValoresKm = False

                    Else
                        blnActualizaValoresKm = True
                        decKilometraje = decKilometrajeActual
                    End If

                End If

                'End If


                If drwConfiguracionDMS.Item("U_ValHS") <> "Y" Then
                    BanderaHoraServicio = False
                Else

                    'Dim strUnidad As String = m_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Cod_Unidad", 0).TrimEnd
                    Dim strHorasServicioActual As String = m_oFormGenCotizacion.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_HoSr", 0).TrimEnd
                    'm_oCotizacion.UserFields.Fields.Item("U_SCGD_HoSr").Value.ToString().Trim()

                    Dim strHoraServicioBD As String = Utilitarios.EjecutarConsulta(String.Format("select U_HorSer from [@SCGD_VEHICULO] with(nolock) where U_Cod_Unid = '{0}'", strUnidad),
                                                                                  m_oCompany.CompanyDB, m_oCompany.Server).ToString().Trim()
                    If String.IsNullOrEmpty(strHorasServicioActual) Then
                        strHorasServicioActual = 0
                    End If
                    If String.IsNullOrEmpty(strHoraServicioBD) Then
                        strHoraServicioBD = 0
                    End If

                    decHoraServicioActual = Utilitarios.ConvierteDecimal(strHorasServicioActual, n)
                    decHoraServicioBD = Utilitarios.ConvierteDecimal(strHoraServicioBD, n)


                    If decHoraServicioActual < decHoraServicioBD Then
                        BanderaHoraServicio = True
                    ElseIf decHoraServicioActual = decHoraServicioBD Then

                        blnActualizaValoresHS = False

                    Else
                        blnActualizaValoresHS = True
                        decHorasServicio = decHoraServicioActual
                    End If

                End If

                dtConfiguracionDMS.Clear()

                If BanderaKilometraje Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorValidacionKilometraje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    Return p_BubbleEvent = False

                ElseIf BanderaHoraServicio Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorValidacionHorasServicio, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    Return p_BubbleEvent = False
                Else

                    Return True

                End If
            End If

        Else
            Return True
        End If


    End Function

    Public Sub ActualizarDatosVehiculo(ByVal p_strNumeroVehiculo As String)

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Dim strCosteoVeh As String = ""
        Dim decCosteoVeh As Decimal = 0

        oCompanyService = m_oCompany.GetCompanyService()
        oGeneralService = oCompanyService.GetGeneralService("SCGD_VEH")
        oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
        oGeneralParams.SetProperty("Code", p_strNumeroVehiculo)
        oGeneralData = oGeneralService.GetByParams(oGeneralParams)

        Dim strHorasServicio As String = CType(decHorasServicio, String)
        Dim intHrServ As Integer = CType(decHorasServicio, Integer)
        Dim dblKilometraj As Integer = CType(decKilometraje, Integer)
        oGeneralData.SetProperty("U_HorSer", intHrServ)
        oGeneralData.SetProperty("U_Km_Unid", dblKilometraj)

        oGeneralService.Update(oGeneralData)
        decKilometraje = 0
        decHorasServicio = 0

    End Sub

    Public Sub AsignarFechayHoraOT(ByRef p_Cotizacion As Documents)

        Dim fhaActual As DateTime

        Try

            'Asignación de los valores
            fhaActual = Utilitarios.RetornaFechaActual(m_oCompany.CompanyDB, m_oCompany.Server)

            If p_Cotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value <> Nothing AndAlso p_Cotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value <> Nothing Then

                p_Cotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value = fhaActual
                p_Cotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value = DateTime.Now

            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Sub

    Private Sub CargarValoresConfiguracionPorSucursal(ByVal p_IdSucursal As String, ByRef p_objValoresConfiguracionSucursalQT As ValoresConfiguracionSucursalCotizacion)

        Dim oDataTableConfiguracionesSucursal As System.Data.DataTable
        Dim oDataRowConfiguracionSucursal As System.Data.DataRow

        'Dim objValoresConfiguracion As New ValoresConfiguracionSucursalCotizacion

        oDataTableConfiguracionesSucursal = Utilitarios.ObtenerConsultaConfiguracionPorSucursal(strIdSucursal, m_oCompany)

        If oDataTableConfiguracionesSucursal.Rows.Count <> 0 Then
            oDataRowConfiguracionSucursal = oDataTableConfiguracionesSucursal.Rows(0)
        Else
            oDataRowConfiguracionSucursal = Nothing

        End If

        '*************************************Inicio: Usando Configuracion Taller Interno***************************************************
        If Not oDataRowConfiguracionSucursal Is Nothing Then

            If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_Requis")) Then
                'Verifico el valor para los Documento preliminares de transferencia de Stock
                If oDataRowConfiguracionSucursal.Item("U_Requis") = "Y" Then
                    blnDraft = True
                Else
                    blnDraft = False
                End If
            End If

            If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_AsigAutCol")) Then
                'Verifico el valor para RealizarAsignacionAutomaticaColaborador
                If oDataRowConfiguracionSucursal.Item("U_AsigAutCol") = "Y" Then
                    blnAsignacionAutomaticaColaborador = True
                Else
                    blnAsignacionAutomaticaColaborador = True
                End If
            End If

            If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_SEInvent")) Then
                'Verifico el valor para Servicios Externos Inventariables
                If oDataRowConfiguracionSucursal.Item("U_SEInvent") = "Y" Then
                    m_blnServicosExternosInventariables = True
                Else
                    m_blnServicosExternosInventariables = False

                End If
            End If

            If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_SerInv")) Then
                'Verifico el valor para SerieNumeracionTransferencia
                If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_SerInv")) Then
                    m_strIDSerieDocTrasnf = oDataRowConfiguracionSucursal.Item("U_SerInv")
                End If

            End If

            If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_CopiasOT")) Then
                'Verifico el valor para Numero de copias
                If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_CopiasOT")) Then
                    m_strNoCopias = oDataRowConfiguracionSucursal.Item("U_CopiasOT")
                End If

            End If

            If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_NoBodRep")) Then
                'Verifico el valor para SerieNumeracionTransferencia
                If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_NoBodRep")) Then
                    m_strNoBodegaRepu = oDataRowConfiguracionSucursal.Item("U_NoBodRep")
                End If

            End If

            If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_NoBodPro")) Then
                'Verifico el valor para SerieNumeracionTransferencia
                If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_NoBodPro")) Then
                    m_strNoBodegaProceso = oDataRowConfiguracionSucursal.Item("U_NoBodPro")
                End If

            End If

            If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_NoBodSE")) Then
                'Verifico el valor para SerieNumeracionTransferencia
                If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_NoBodSE")) Then
                    m_strNoBodegaSeEx = oDataRowConfiguracionSucursal.Item("U_NoBodSE")
                End If

            End If

            If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_NoBodSum")) Then
                'Verifico el valor para SerieNumeracionTransferencia
                If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_NoBodSum")) Then
                    m_strNoBodegaSumi = oDataRowConfiguracionSucursal.Item("U_NoBodSum")
                End If

            End If

            'p_objValoresConfiguracionSucursalQT = objValoresConfiguracion

        End If
        '*************************************Fin: usando Configuracion Taller Interno*******************************************************
    End Sub

    Public Function ActulizaLineasCot(ByVal strIdAct As String, ByVal strIdMecanico As String, ByVal strNombreMecanico As String, ByVal usaTallerSAP As Boolean,
                                      ByRef m_objCotizacion As SAPbobsCOM.Documents) As Boolean


        Dim oLineasCotizacion As SAPbobsCOM.Document_Lines
        Dim m_strValorId As String
        Dim idRep As String
        ''Inserta Mecanico en Cotizacion 09/05/2014

        oLineasCotizacion = m_objCotizacion.Lines
        If usaTallerSAP Then
            idRep = "U_SCGD_ID"
        Else
            idRep = "U_SCGD_IdRepxOrd"
        End If
        For i As Integer = 0 To oLineasCotizacion.Count - 1

            oLineasCotizacion.SetCurrentLine(i)
            m_strValorId = oLineasCotizacion.UserFields.Fields.Item(idRep).Value.ToString.Trim()

            If (strIdAct = m_strValorId) Then
                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = strIdMecanico
                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = strNombreMecanico 'cboColabora.Especifico.ValidValues.Item(strIdMecanico).Description.Trim()
                Exit For
            End If
        Next

        Return True

    End Function


    Private Sub CargarValidValuesEnComboTipoOrden(ByRef oForm As SAPbouiCOM.Form, _
                                                          Optional ByVal strIdSucursal As String = "", _
                                                          Optional ByVal p_blnUsaConfiguracionInternaTaller As Boolean = False)

        Dim intRecIndex As Integer
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item
        Dim strQueryTipos As String
        Dim drdResultadoConsulta As SqlClient.SqlDataReader
        Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection

        Try
            oItem = oForm.Items.Item("SCGD_cbTOT")
            cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString

            If Not p_blnUsaConfiguracionInternaTaller Then
                strQueryTipos = "Select Code, Name from [@SCGD_TIPO_ORDEN] Order By Code"

            Else
                strQueryTipos = "SELECT [@SCGD_CONF_TIP_ORDEN].U_Code, [@SCGD_CONF_TIP_ORDEN].U_Name " & _
                                "FROM   [@SCGD_CONF_SUCURSAL] INNER JOIN " & _
                                "[@SCGD_CONF_TIP_ORDEN] ON [@SCGD_CONF_SUCURSAL].DocEntry = [@SCGD_CONF_TIP_ORDEN].DocEntry " & _
                                "WHERE ([@SCGD_CONF_SUCURSAL].U_Sucurs = '" & strIdSucursal & "')  Order By [@SCGD_CONF_TIP_ORDEN].U_Code"
            End If


            cn_Coneccion.Open()
            cmdEjecutarConsulta.Connection = cn_Coneccion
            cmdEjecutarConsulta.CommandType = CommandType.Text
            cmdEjecutarConsulta.CommandText = strQueryTipos
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()

            'Borra los ValidValues de los combos. 
            If cboCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To cboCombo.ValidValues.Count - 1
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If
            'Agregar Valid Values
            Do While drdResultadoConsulta.Read
                If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then
                    cboCombo.ValidValues.Add(drdResultadoConsulta.GetValue(0), drdResultadoConsulta.GetString(1).Trim)
                End If
            Loop


            drdResultadoConsulta.Close()
            cn_Coneccion.Close()


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex
        End Try

    End Sub

    Private Sub CargarValoresConfiguracionTaller(ByRef p_blnConfiguracionInterna As Boolean, Optional ByVal p_IdSucursal As String = "", Optional ByRef p_objValoresConfiguracionSucursalQT As ValoresConfiguracionSucursalCotizacion = Nothing)


        If p_blnConfiguracionInterna = True Then


            CargarValoresConfiguracionPorSucursal(p_IdSucursal, objValoresConfiguracionSucursalQT)
            m_blnUsaConfiguracionInternaTaller = True

        ElseIf p_blnConfiguracionInterna = False Then

            Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, p_IdSucursal, strCadenaConexionBDTaller)
            'Verifico el valor de la propiedad para los Documento preliminares de transferencia de Stock
            Dim adpConf As New ConfiguracionDataAdapter(strCadenaConexionBDTaller)
            Dim dstConf As New ConfiguracionDataSet
            Dim objUtilitariosCls As New Utilitarios

            adpConf.Fill(dstConf)

            If objUtilitariosCls.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, "CreaDraftTransferenciasStock", "") Then
                blnDraft = True
            Else
                blnDraft = False
            End If

            'Verifico si la Asignacion se hace de manera automatica
            If objUtilitariosCls.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, "RealizarAsignacionAutomaticaColaborador", "") Then
                blnAsignacionAutomaticaColaborador = True
            Else
                blnAsignacionAutomaticaColaborador = False
            End If

            m_blnUsaConfiguracionInternaTaller = False

        End If

    End Sub

    Private Function CargarValoresConfiguracionPorSucursal(ByVal p_EsOTInterna As Boolean, ByVal p_IdSucursal As String, ByVal p_strTipoOrden As String, ByRef p_valoresConfiSuc As ValoresConfiguracionSucursalCotizacion) As Boolean
        If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_IdSucursal)) Then
            With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_IdSucursal))
                If p_EsOTInterna Then
                    If .Configuracion_OT_Interna.Any(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_strTipoOrden)) Then
                        If Not String.IsNullOrEmpty(.Configuracion_OT_Interna.FirstOrDefault(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_strTipoOrden)).U_NumCuent.Trim()) Then p_valoresConfiSuc.m_strCuentaTipoOrdenInternaConfiSucursal = .Configuracion_OT_Interna.FirstOrDefault(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_strTipoOrden)).U_NumCuent.Trim()
                        If Not String.IsNullOrEmpty(.Configuracion_OT_Interna.FirstOrDefault(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_strTipoOrden)).U_Tran_Com.Trim()) Then p_valoresConfiSuc.m_strTransaccionLineas = .Configuracion_OT_Interna.FirstOrDefault(Function(otInterna) otInterna.U_Tipo_OT.Trim().Equals(p_strTipoOrden)).U_Tran_Com.Trim()
                    End If
                End If
                If Not String.IsNullOrEmpty(.U_Moneda_C) Then
                    p_valoresConfiSuc.m_strTipoMoneda = .U_Moneda_C.Trim()
                Else
                    Return False
                End If
                If Not String.IsNullOrEmpty(.U_SEInvent) Then
                    p_valoresConfiSuc.m_blnServicosExternosInventariables = .U_SEInvent.Trim.Equals("Y")
                Else
                    Return False
                End If
                If Not String.IsNullOrEmpty(.U_CuentaSys_C) Then
                    p_valoresConfiSuc.m_strCodigoCuentaExistenciasConfiSucursal = .U_CuentaSys_C.Trim()
                End If
                If Not String.IsNullOrEmpty(.U_CostoSimp) Then
                    p_valoresConfiSuc.m_strTipoCostoPorSucursal = IIf(.U_CostoSimp.Trim.Equals("Y"), 1, 2)
                Else
                    Return False
                End If
                If Not String.IsNullOrEmpty(.U_CostoDet) Then
                    p_valoresConfiSuc.m_strTipoCostoPorSucursal = IIf(.U_CostoDet.Trim().Equals("Y"), 2, 1)
                Else
                    Return False
                End If
                If .Configuracion_Tipo_Orden.Any(Function(tipoOrden) tipoOrden.U_Code = CInt(p_strTipoOrden)) Then
                    If Not String.IsNullOrEmpty(.Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOrden) tipoOrden.U_Code = CInt(p_strTipoOrden)).U_CodCtCos) Then p_valoresConfiSuc.m_strCentroCosto = .Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOrden) tipoOrden.U_Code = CInt(p_strTipoOrden)).U_CodCtCos.Trim()
                End If
                If Not String.IsNullOrEmpty(.U_SerOrV) Then p_valoresConfiSuc.m_strIDSerieDocOrdenVenta = .U_SerOrV.Trim()
                If Not String.IsNullOrEmpty(.U_SerInv) Then p_valoresConfiSuc.m_strIDSerieDocTrasnf = .U_SerInv.Trim()
            End With
            Return True
        Else
            Return False
        End If
    End Function


    Public Sub ValidarDataTable(ByRef p_form As SAPbouiCOM.Form, ByRef p_DataTable As SAPbouiCOM.DataTable)

        'Dim ExisteDataTable As Boolean = Utilitarios.ValidarDataTable(p_form, "dtConsulta")
        Dim ExisteDataTable As Boolean = False

        If p_form.DataSources.DataTables.Count > 0 Then
            For i As Integer = 0 To p_form.DataSources.DataTables.Count - 1
                If p_form.DataSources.DataTables.Item(i).UniqueID = "dtConsulta" Then
                    ExisteDataTable = True
                    Exit For
                End If
            Next
        End If

        If Not ExisteDataTable Then
            p_DataTable = p_form.DataSources.DataTables.Add("dtConsulta")
        End If
    End Sub

    Public Sub ActualizaAsesoryTipoOT(ByVal p_oCotizacion As SAPbobsCOM.Documents)

        Dim intIdMecanico As Integer
        Dim intTipoOT As Integer
        Dim strNumeroOT As String = String.Empty

        intIdMecanico = p_oCotizacion.UserFields.Fields.Item("OwnerCode").Value.ToString.Trim
        intTipoOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value.ToString.Trim
        strNumeroOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim

        If Not String.IsNullOrEmpty(strNumeroOT) Then
            Utilitarios.EjecutarConsulta(String.Format("UPDATE SCGTA_TB_Orden SET CodTipoOrden = {0}, Asesor = {1} WHERE NoOrden = '{2}'", intTipoOT, intIdMecanico, strNumeroOT), m_strBDTalller, m_oCompany.Server)
        End If

    End Sub

    Public Sub BotonAsignacionMultiple(ByVal p_pval As SAPbouiCOM.ItemEvent, ByVal UsaTallerSap As Boolean)

        Dim oFormCot As SAPbouiCOM.Form
        Dim query As String = String.Empty
        Dim queryNF As String
        Dim strIdSucursales As String
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim OT As SAPbobsCOM.GeneralData
        Dim m_childs As SAPbobsCOM.GeneralDataCollection = Nothing
        Dim m_childdata As SAPbobsCOM.GeneralData = Nothing
        Dim dtQuery As DataTable
        Dim filters As String = String.Empty
        Dim rowAdded As String
        Dim strHora, strMinutos As String

        oFormCot = SBO_Application.Forms.Item(p_pval.FormUID)
        If oFormCot.DataSources.DataTables.Item("MecanicosAsignados").Rows.Count > 0 Then
            Dim numeroCot = m_oCotizacion.DocEntry
            dtMecAsignados = oFormCot.DataSources.DataTables.Item("MecanicosAsignados")
            strIdSucursales = m_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()

            If Utilitarios.ValidaExisteDataTable(oFormCot, "LocalDt") Then
                dtQuery = oFormCot.DataSources.DataTables.Item("LocalDt")
            Else
                dtQuery = oFormCot.DataSources.DataTables.Add("LocalDt")
            End If

            If Not String.IsNullOrEmpty(strIdSucursales) Then
                If Not UsaTallerSap Then
                    queryNF = ConsultaAsignacionesOTExterna
                    Dim queryComp As String = String.Format(queryNF, m_strNoOrden, strIdSucursales)
                    dtQuery.ExecuteQuery(queryComp)

                    Utilitarios.DevuelveNombreBDTaller(SBO_Application, m_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString(), strIdSucursales)

                    For i As Integer = 0 To dtMecAsignados.Rows.Count - 1
                        For line As Integer = 0 To m_oCotizacion.Lines.Count - 1
                            m_oCotizacion.Lines.SetCurrentLine(line)
                            If dtMecAsignados.GetValue("col_CodAct", i) = m_oCotizacion.Lines.UserFields.Fields.Item("ItemCode").Value AndAlso dtMecAsignados.GetValue("col_LineNum", i) = m_oCotizacion.Lines.LineNum.ToString().Trim() Then
                                dtMecAsignados.SetValue("col_IdRepXOrd", i, m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString().Trim())
                                Exit For
                            End If
                            dtMecAsignados.SetValue("col_NoOrden", i, m_strNoOrden)
                        Next

                    Next
                    query = "insert into [{0}].[dbo].[SCGTA_TB_ControlColaborador] " & _
                             "(EmpID, Reproceso, Costo, TiempoHoras, NoOrden, NoFase, Estado, IdActividad, CostoEstandar) " & _
                             "Values " & _
                             "{1}"

                    For i As Integer = 0 To dtMecAsignados.Rows.Count - 1
                        rowAdded = dtMecAsignados.GetValue("col_Added", i).ToString().Trim()
                        If rowAdded = "N" Then
                            For y As Integer = 0 To dtQuery.Rows.Count - 1
                                Dim strLn = dtMecAsignados.GetValue("col_LineNum", i).ToString().Trim()
                                Dim lineNum = 0
                                Integer.TryParse(strLn, lineNum)
                                If dtMecAsignados.GetValue("col_CodAct", i).ToString().Trim() = dtQuery.GetValue("ItemCode", y).ToString().Trim() AndAlso _
                                    (strLn).ToString() = dtQuery.GetValue("LineNum", y).ToString().Trim() Then

                                    dtMecAsignados.SetValue("col_IdRepXOrd", i, dtQuery.GetValue("ID", y).ToString().Trim())
                                    dtMecAsignados.SetValue("col_NoFase", i, dtQuery.GetValue("NoFase", y).ToString().Trim())
                                    dtMecAsignados.SetValue("col_Estado", i, dtQuery.GetValue("Estado", y).ToString().Trim())

                                End If
                            Next
                            If String.IsNullOrEmpty(filters) Then
                                filters = String.Format("('{0}', 0, 0, 0, '{1}', '{2}', '{3}', '{4}', 0) ", dtMecAsignados.GetValue("col_CodEmp", i).ToString().Trim(),
                                                        m_strNoOrden, dtMecAsignados.GetValue("col_NoFase", i).ToString().Trim(),
                                                        dtMecAsignados.GetValue("col_Estado", i).ToString().Trim(), dtMecAsignados.GetValue("col_IdRepXOrd", i).ToString().Trim())
                            Else
                                filters = String.Format("{0}, ('{1}', 0, 0, 0, '{2}', '{3}', '{4}', '{5}', 0) ", filters, dtMecAsignados.GetValue("col_CodEmp", i).ToString().Trim(),
                                                        m_strNoOrden, dtMecAsignados.GetValue("col_NoFase", i).ToString().Trim(),
                                                        dtMecAsignados.GetValue("col_Estado", i).ToString().Trim(), dtMecAsignados.GetValue("col_IdRepXOrd", i).ToString().Trim())
                            End If
                        End If
                    Next

                    query = String.Format(query, strIdSucursales, filters)
                    Try
                        If Not m_oCompany.InTransaction Then
                            m_oCompany.StartTransaction()
                        End If
                        dtQuery.ExecuteQuery(query)

                        For i As Integer = 0 To dtMecAsignados.Rows.Count - 1
                            ActulizaLineasCot(dtMecAsignados.GetValue("col_IdRepXOrd", i).ToString().Trim(), dtMecAsignados.GetValue("col_CodEmp", i).ToString().Trim(), dtMecAsignados.GetValue("col_NomEmp", i).ToString().Trim(), UsaTallerSap, m_oCotizacion)
                        Next

                        If m_oCotizacion.Update() = 0 Then
                            If m_oCompany.InTransaction Then
                                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                            End If
                            dtMecAsignados.Rows.Clear()
                        Else
                            If m_oCompany.InTransaction Then
                                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                            End If
                        End If
                    Catch ex As Exception
                        If m_oCompany.InTransaction Then
                            m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                        End If
                    End Try
                Else
                    queryNF = ConsultaAsignacionesOTInterna
                    Dim queryComp As String = String.Format(queryNF, m_strNoOrden, strIdSucursales)
                    dtQuery.ExecuteQuery(queryComp)

                    For i As Integer = 0 To dtMecAsignados.Rows.Count - 1
                        For line As Integer = 0 To m_oCotizacion.Lines.Count - 1
                            m_oCotizacion.Lines.SetCurrentLine(line)
                            If dtMecAsignados.GetValue("col_CodAct", i) = m_oCotizacion.Lines.UserFields.Fields.Item("ItemCode").Value AndAlso dtMecAsignados.GetValue("col_LineNum", i) = m_oCotizacion.Lines.LineNum.ToString().Trim() Then
                                dtMecAsignados.SetValue("col_IdRepXOrd", i, m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim())
                                Exit For
                            End If
                            dtMecAsignados.SetValue("col_NoOrden", i, m_strNoOrden)
                        Next
                    Next

                    Try
                        oCompanyService = m_oCompany.GetCompanyService()
                        oGeneralService = oCompanyService.GetGeneralService("SCGD_OT")
                        oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                        oGeneralParams.SetProperty("Code", m_strNoOrden)
                        OT = oGeneralService.GetByParams(oGeneralParams)
                        m_childs = OT.Child("SCGD_CTRLCOL")

                        For i As Integer = 0 To dtMecAsignados.Rows.Count - 1
                            rowAdded = dtMecAsignados.GetValue("col_Added", i).ToString().Trim()
                            If rowAdded = "N" Then
                                For y As Integer = 0 To dtQuery.Rows.Count - 1
                                    Dim strLn = dtMecAsignados.GetValue("col_LineNum", i).ToString().Trim()
                                    Dim lineNum = 0
                                    Integer.TryParse(strLn, lineNum)
                                    If dtMecAsignados.GetValue("col_CodAct", i).ToString().Trim() = dtQuery.GetValue("ItemCode", y).ToString().Trim() AndAlso _
                                        (strLn).ToString() = dtQuery.GetValue("LineNum", y).ToString().Trim() Then
                                        dtMecAsignados.SetValue("col_IdRepXOrd", i, dtQuery.GetValue("IDRepXOrd", y).ToString().Trim())
                                        dtMecAsignados.SetValue("col_NoFase", i, dtQuery.GetValue("NoFase", y).ToString().Trim())
                                        dtMecAsignados.SetValue("col_Estado", i, dtQuery.GetValue("Estado", y).ToString().Trim())
                                        Exit For
                                    End If
                                Next
                                m_childdata = m_childs.Add()

                                If String.IsNullOrEmpty(dtMecAsignados.GetValue("col_Estado", i)) Then
                                    m_childdata.SetProperty("U_Estad", "1")
                                Else
                                    m_childdata.SetProperty("U_Estad", dtMecAsignados.GetValue("col_Estado", i))
                                End If

                                m_childdata.SetProperty("U_IdAct", dtMecAsignados.GetValue("col_IdRepXOrd", i))
                                m_childdata.SetProperty("U_NoFas", dtMecAsignados.GetValue("col_DesNoFase", i))
                                m_childdata.SetProperty("U_Colab", dtMecAsignados.GetValue("col_CodEmp", i))
                                m_childdata.SetProperty("U_TMin", 0)
                                m_childdata.SetProperty("U_CosRe", 0)
                                m_childdata.SetProperty("U_CosEst", dtMecAsignados.GetValue("col_PrecioSt", i))
                                m_childdata.SetProperty("U_CodFas", dtMecAsignados.GetValue("col_NoFase", i))
                                strHora = DateTime.Now.Hour.ToString()
                                If strHora.Length = 1 Then strHora = String.Format("0{0}", strHora)
                                strMinutos = DateTime.Now.Minute.ToString()
                                If (strMinutos.Length = 1) Then strMinutos = String.Format("0{0}", strMinutos)
                                m_childdata.SetProperty("U_FechPro", DateTime.Now)
                                m_childdata.SetProperty("U_HoraIni", strHora)

                            End If
                        Next

                        For i As Integer = 0 To dtMecAsignados.Rows.Count - 1
                            ActulizaLineasCot(dtMecAsignados.GetValue("col_IdRepXOrd", i).ToString().Trim(), dtMecAsignados.GetValue("col_CodEmp", i).ToString().Trim(), dtMecAsignados.GetValue("col_NomEmp", i).ToString().Trim(), UsaTallerSap, m_oCotizacion)
                        Next

                        If Not m_oCompany.InTransaction Then
                            m_oCompany.StartTransaction()
                        End If

                        If m_oCotizacion.Update() = 0 Then
                            oGeneralService.Update(OT)
                            If m_oCompany.InTransaction Then
                                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                            End If
                            dtMecAsignados.Rows.Clear()
                        Else
                            If m_oCompany.InTransaction Then
                                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                            End If
                        End If
                    Catch ex As Exception
                        If m_oCompany.InTransaction Then
                            m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                        End If
                    End Try

                End If




            End If
        End If

    End Sub

    Public Sub ValidaModoVistaForm(ByRef p_oForm As SAPbouiCOM.Form)
        Try
            If DMS_Connector.Helpers.PermisosMenu("SCGD_OVV") Then
                If Not String.IsNullOrEmpty(p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).Trim) Then
                    If Not String.IsNullOrEmpty(p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_BloOT", 0).Trim) Then
                        If p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_BloOT", 0).Trim = "Y" Then
                            p_oForm.Mode = BoFormMode.fm_VIEW_MODE
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub
#End Region


End Class


Public Class ListaActividadesCotizacion


    Public Property SCGD_EmpAsig() As String
        Get
            Return strSCGD_EmpAsig
        End Get
        Set(ByVal value As String)
            strSCGD_EmpAsig = value
        End Set
    End Property
    Private strSCGD_EmpAsig As String

    Public Property SCGD_ID() As String
        Get
            Return strSCGD_ID
        End Get
        Set(ByVal value As String)
            strSCGD_ID = value
        End Set
    End Property
    Private strSCGD_ID As String
    Public Property SCGD_NombEmpleado() As String
        Get
            Return strSCGD_NombEmpleado
        End Get
        Set(ByVal value As String)
            strSCGD_NombEmpleado = value
        End Set
    End Property
    Private strSCGD_NombEmpleado As String

    Public Property CostoReal() As Decimal
        Get
            Return decCostoReal
        End Get
        Set(ByVal value As Decimal)
            decCostoReal = value
        End Set
    End Property
    Private decCostoReal As Decimal


    Public Property CostoEstandar() As Decimal
        Get
            Return decCostoEstandar
        End Get
        Set(ByVal value As Decimal)
            decCostoEstandar = value
        End Set
    End Property
    Private decCostoEstandar As Decimal

    Public Property FechaInicioActividad() As String
        Get
            Return strFechaInicioActividad
        End Get
        Set(ByVal value As String)
            strFechaInicioActividad = value
        End Set
    End Property
    Private strFechaInicioActividad As String

    Public Property FechaFinalActividad() As Date
        Get
            Return dtFechaFinalActividad
        End Get
        Set(ByVal value As Date)
            dtFechaFinalActividad = value
        End Set
    End Property
    Private dtFechaFinalActividad As Date

    Public Property HoraInicio() As String
        Get
            Return strHoraInicio
        End Get
        Set(ByVal value As String)
            strHoraInicio = value
        End Set
    End Property
    Private strHoraInicio As String

    Public Property Estado() As String
        Get
            Return strEstado
        End Get
        Set(ByVal value As String)
            strEstado = value
        End Set
    End Property
    Private strEstado As String

    Public Property FaseProduccion() As String
        Get
            Return strFaseProduccion
        End Get
        Set(ByVal value As String)
            strFaseProduccion = value
        End Set
    End Property
    Private strFaseProduccion As String

    Public Property DuracionLabor() As String
        Get
            Return strDuracionLabor
        End Get
        Set(value As String)
            strDuracionLabor = value
        End Set
    End Property
    Private strDuracionLabor As String

    Public Sub New()

    End Sub
End Class



