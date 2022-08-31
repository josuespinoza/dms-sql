Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports System.Collections.Generic
Imports SCG.DMSOne.Framework.MenuManager
Imports SAPbobsCOM
Imports SCG.UX.Windows
Imports System.Data.SqlClient
Imports SCG.SBOFramework.DI
Imports System.Globalization



Public Class CosteoCls


    Public Sub New(p_oCompany As SAPbobsCOM.Company, p_SboApplication As SAPbouiCOM.Application, p_strMonedaLocal As String, p_strMonedaSystema As String, p_blnDimension As Boolean)



        m_oCompany = p_oCompany
        m_oSBOApplication = p_SboApplication

        g_strMonedaLocal = p_strMonedaLocal
        g_strMonedaSystema = p_strMonedaSystema

        objConfiguracionGeneral = Nothing
        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
        If cn_Coneccion.State = ConnectionState.Open Then
            cn_Coneccion.Close()
        End If
        cn_Coneccion.ConnectionString = strConectionString
        objConfiguracionGeneral = New DMSOneFramework.SCGDataAccess.ConfiguracionesGeneralesAddon(cn_Coneccion)

        blnUsaDimension = p_blnDimension

    End Sub

#Region "Declaraciones"

    Public m_oCompany As SAPbobsCOM.Company
    Public m_oSBOApplication As SAPbouiCOM.Application

    Private strConectionString As String = ""
    Private cn_Coneccion As New SqlClient.SqlConnection

    Private objDataTable As SAPbouiCOM.DataTable
    Private blnUsaDimension As Boolean = False

    Public oListaLineasValoresCosteoPorUnidad As New List(Of ListaValoresCosteo)()
    Public oListaTransacciones As New List(Of ListaValoresCosteoLocal_Sistema)()

    Public g_strMonedaLocal As String = String.Empty
    Public g_strMonedaSystema As String = String.Empty

    Private CIFLocal As Decimal
    Private CIFSistema As Decimal

    Public TablaValoresTransacciones As New List(Of ListaValoresCosteoLocal_Sistema)()

    Public TablaValoresAcumulados As Hashtable = New Hashtable

    Private decTotalesMonedaLocal As Decimal
    Private decTotalesMonedaSistema As Decimal

    Private decCostoTotalMonedaLocal As Decimal
    Private decCostoTotalMonedaSistema As Decimal

    Private decSaldoInicialLocal As Decimal
    Private decSaldoInicialSistema As Decimal

    Private g_blnLineaAgregada As Boolean = False

    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon

    Private strUnidad As String = String.Empty
    Private strVIN As String = String.Empty
    Private strMarca As String = String.Empty
    Private strModelo As String = String.Empty
    Private strEstilo As String = String.Empty
    Private strIDVehiculo As String = String.Empty
    Private strTipoVehiculo As String = String.Empty
    Private strContrato As String = String.Empty
    Private strEntrada As String = String.Empty
    Private strDocRecepcion As String = String.Empty
    Private strCodigoMarca As String = String.Empty
    Private strCodigoPedido As String = String.Empty

    Public ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls
    Private oDataTableDimensiones As System.Data.DataTable

    Private blnAgregarDimension As Boolean = False
    Private blnNotaCredito As Boolean = False
    Private blnVieneAsientoSalidaTaller As Boolean = False

    Public n As NumberFormatInfo
    Private strFechaDocumento As String = String.Empty
    Private m_decTipoCambio As Decimal = 0




#End Region

#Region "Constantes"

    Public Const strFLETE As String = "FLETE"
    Public Const strFOB As String = "FOB"
    Public Const strSEGFAC As String = "SEGFAC"
    Public Const strCOMFOR As String = "COMFOR"
    Public Const strCOMNEG As String = "COMNEG"
    Public Const strCIF As String = "CIF"

    Public Const strACCINT As String = "ACCINT"
    Public Const strACCEXT As String = "ACCEXT"
    Public Const strCOMAPE As String = "COMAPE"
    Public Const strSEGLOC As String = "SEGLOC"
    Public Const strTRASLA As String = "TRASLA"
    Public Const strREDEST As String = "REDEST"
    Public Const strBODALM As String = "BODALM"
    Public Const strDESALM As String = "DESALM"
    Public Const strIMPVTA As String = "IMPVTA"
    Public Const strAGENCIA As String = "AGENCIA"
    Public Const strFLELOC As String = "FLELOC"
    Public Const strRESERVA As String = "RESERVA"
    Public Const strOTROS_FP As String = "OTROS_FP"
    Public Const strTALLER As String = "TALLER"


    Public Const g_strDataTableConsulta As String = "dtConsulta"


    Public Const srtFormulariosCompra As String = "SELECT OPCH.TransId, PCH1.U_SCGD_Cod_Tran + ' ' + ISNULL(PCH1.Dscription, '') AS Memo, OPCH.SysRate AS Rate, PCH1.LineTotal AS Local, " & _
                                                  " PCH1.LineTotal / OPCH.SysRate AS Systema, OPCH.DocEntry AS FP, NULL AS FC, OPCH.DocCur AS 'MonedaRegistro', PCH1.AcctCode, PCH1.U_SCGD_Cod_Tran, " & _
                                                  " (SELECT  U_View FROM [@SCGD_TRAN_COMP] WHERE (Code = PCH1.U_SCGD_Cod_Tran)) AS NombreTransaccion " & _
                                                  " FROM PCH1 WITH (nolock) INNER JOIN OPCH WITH (nolock) ON PCH1.DocEntry = OPCH.DocEntry " & _
                                                  " WHERE (PCH1.U_SCGD_Cod_Unid = '{0}') AND (OPCH.DocType = '{2}' OR OPCH.DocType = '{3}') AND (OPCH.TransId not in (SELECT L.U_NoAsient " & _
                                                  " FROM [@SCGD_GRLINES] AS L WITH (nolock) INNER JOIN [@SCGD_GOODRECEIVE] AS GR WITH (nolock) ON GR.DocEntry = L.DocEntry AND GR.U_Unidad = '{0}' AND L.U_NoAsient IS NOT NULL)) AND " & _
                                                  " (OPCH.DocDate <= CAST('{1}' + ' 23:59:59 ' AS datetime)) AND (PCH1.U_SCGD_Cod_Tran IN (SELECT Code FROM [@SCGD_TRAN_COMP] AS [@SCGD_TRAN_COMP_1])) " & _
                                                  " AND (OPCH.U_SCGD_AplicaCosteo <> 'N' or OPCH.U_SCGD_AplicaCosteo is null)"


    Public Const strAsientos As String = "SELECT OJDT_1.TransId, CAST(JDT1_1.U_SCGD_Cod_Tran AS varchar) + ' ' +  (SELECT Name FROM [@SCGD_TRAN_COMP] WHERE (Code = JDT1_1.U_SCGD_Cod_Tran)) AS Memo, " & _
                                                         "CASE OJDT_1.TransRate WHEN 0 THEN Isnull(ORTT_1.Rate, 1) ELSE Isnull(OJDT_1.TransRate, ORTT_1.Rate) END AS Rate, " & _
                                                         "CASE debit WHEN 0 THEN ((CASE SYSDeb WHEN 0 THEN FCdebit ELSE SYSDeb END) / " & _
                                                         "(SELECT rate FROM ORTT WHERE RateDate =  '{1}' AND Currency =  '{4}')) ELSE debit END AS Local, " & _
                                                         "CASE SYSDeb WHEN 0 THEN FCdebit ELSE SYSDeb END AS Systema, NULL AS FP, NULL AS FC, CASE (debit + FCdebit) WHEN CAST(0 AS decimal) " & _
                                                         "THEN  '{4}' ELSE ISNULL(JDT1_1.FCCurrency,  '{2}') END AS MonedaRegistro, JDT1_1.Account AS AcctCode, JDT1_1.U_SCGD_Cod_Tran, " & _
                                                         "(SELECT U_View FROM [@SCGD_TRAN_COMP] AS [@SCGD_TRAN_COMP_9] WHERE (Code = JDT1_1.U_SCGD_Cod_Tran)) AS NombreTransaccion " & _
                                                         "FROM JDT1 AS JDT1_1 WITH (nolock) INNER JOIN " & _
                                                         "OJDT AS OJDT_1 WITH (nolock) ON JDT1_1.TransId = OJDT_1.TransId LEFT OUTER JOIN " &
                                                         "ORTT AS ORTT_1 WITH (nolock) ON OJDT_1.RefDate = ORTT_1.RateDate AND (ORTT_1.Currency = JDT1_1.FCCurrency OR " & _
                                                         "(JDT1_1.FCCurrency IS NULL OR JDT1_1.FCCurrency =  '{3}') AND ORTT_1.Currency =  '{4}') " & _
                                                         "WHERE (JDT1_1.U_SCGD_Cod_Unidad = '{0}') AND (JDT1_1.SYSDeb <> 0) AND (OJDT_1.TransId NOT IN (SELECT L.U_NoAsient FROM [@SCGD_GRLINES] AS L WITH (nolock) INNER JOIN " & _
                                                         "[@SCGD_GOODRECEIVE] AS GR WITH (nolock) ON GR.DocEntry = L.DocEntry AND GR.U_Unidad = '{0}' AND L.U_NoAsient IS NOT NULL)) AND " & _
                                                         "(OJDT_1.RefDate <= CAST( '{1}' + ' 23:59:59 ' AS datetime)) AND (JDT1_1.U_SCGD_Cod_Tran IN " & _
                                                         "(SELECT Code FROM [@SCGD_TRAN_COMP] AS [@SCGD_TRAN_COMP_7])) OR (JDT1_1.U_SCGD_Cod_Unidad = '{0}') AND (OJDT_1.TransId NOT IN (SELECT L.U_NoAsient FROM [@SCGD_GRLINES] AS L WITH (nolock) INNER JOIN " & _
                                                         "[@SCGD_GOODRECEIVE] AS GR WITH (nolock) ON GR.DocEntry = L.DocEntry AND GR.U_Unidad = '{0}' AND L.U_NoAsient IS NOT NULL)) AND (OJDT_1.RefDate <= CAST( '{1}' + ' 23:59:59 ' AS datetime)) AND (JDT1_1.U_SCGD_Cod_Tran IN " & _
                                                         "(SELECT Code FROM [@SCGD_TRAN_COMP] AS [@SCGD_TRAN_COMP_5])) AND (JDT1_1.FCDebit <> 0) OR (JDT1_1.U_SCGD_Cod_Unidad = '{0}') AND (OJDT_1.TransId NOT IN (SELECT L.U_NoAsient FROM [@SCGD_GRLINES] AS L WITH (nolock) INNER JOIN " & _
                                                         "[@SCGD_GOODRECEIVE] AS GR WITH (nolock) ON GR.DocEntry = L.DocEntry AND GR.U_Unidad = '{0}' AND L.U_NoAsient IS NOT NULL)) AND (OJDT_1.RefDate <= CAST( '{1}' + ' 23:59:59 ' AS datetime)) AND (JDT1_1.U_SCGD_Cod_Tran IN " & _
                                                         "(SELECT Code FROM [@SCGD_TRAN_COMP] AS [@SCGD_TRAN_COMP_4])) AND (JDT1_1.Debit <> 0) " & _
                                                         "UNION " & _
                                                         "SELECT OJDT.TransId, CAST(JDT1.U_SCGD_Cod_Tran AS varchar) + ' ' + (SELECT Name FROM [@SCGD_TRAN_COMP] AS [@SCGD_TRAN_COMP_8] WHERE (Code = JDT1.U_SCGD_Cod_Tran)) AS Memo, CASE OJDT.TransRate WHEN 0 THEN Isnull(ORTT.Rate, 1) ELSE Isnull(OJDT.TransRate, ORTT.Rate) " & _
                                                         "END AS Rate, (CASE credit WHEN 0 THEN ((CASE SYSCred WHEN 0 THEN FCcredit ELSE SYSCred END) / " & _
                                                         "(SELECT rate FROM ORTT WHERE RateDate =  '{1}' AND Currency =  '{4}')) ELSE credit END) * - 1 AS Local, (CASE SYSCred WHEN 0 THEN FCcredit ELSE SYSCred END) " & _
                                                         "* - 1 AS Systema, NULL AS FP, NULL AS FC, CASE (credit + FCcredit) WHEN 0 THEN  '{4}' ELSE ISNULL(JDT1.FCCurrency,  '{3}') END AS 'Moneda Registro', JDT1.Account AS AcctCode, JDT1.U_SCGD_Cod_Tran, " & _
                                                         "(SELECT U_View FROM [@SCGD_TRAN_COMP] AS [@SCGD_TRAN_COMP_6] WHERE (Code = JDT1.U_SCGD_Cod_Tran)) AS NombreTransaccion FROM JDT1 AS JDT1 WITH (nolock) INNER JOIN " & _
                                                         "OJDT AS OJDT WITH (nolock) ON JDT1.TransId = OJDT.TransId LEFT OUTER JOIN ORTT AS ORTT WITH (nolock) ON OJDT.RefDate = ORTT.RateDate AND (ORTT.Currency = JDT1.FCCurrency OR " & _
                                                         "(JDT1.FCCurrency IS NULL OR JDT1.FCCurrency =  '{2}') AND ORTT.Currency =  '{4}') WHERE (JDT1.U_SCGD_Cod_Unidad = '{0}') AND (JDT1.SYSCred <> 0) AND (OJDT.TransId NOT IN " & _
                                                         "(SELECT L.U_NoAsient FROM [@SCGD_GRLINES] AS L WITH (nolock) INNER JOIN [@SCGD_GOODRECEIVE] AS GR WITH (nolock) ON GR.DocEntry = L.DocEntry AND GR.U_Unidad = '{0}' AND L.U_NoAsient IS NOT NULL)) AND " & _
                                                         "(OJDT.RefDate <= CAST( '{1}' + ' 23:59:59 ' AS datetime)) AND (JDT1.U_SCGD_Cod_Tran IN (SELECT Code FROM [@SCGD_TRAN_COMP] AS [@SCGD_TRAN_COMP_3])) OR (JDT1.U_SCGD_Cod_Unidad = '{0}') AND (OJDT.TransId NOT IN " & _
                                                         "(SELECT L.U_NoAsient FROM [@SCGD_GRLINES] AS L WITH (nolock) INNER JOIN [@SCGD_GOODRECEIVE] AS GR WITH (nolock) ON GR.DocEntry = L.DocEntry AND GR.U_Unidad = '{0}' AND L.U_NoAsient IS NOT NULL)) AND " & _
                                                         "(OJDT.RefDate <= CAST( '{1}' + ' 23:59:59 ' AS datetime)) AND (JDT1.U_SCGD_Cod_Tran IN (SELECT Code FROM [@SCGD_TRAN_COMP] AS [@SCGD_TRAN_COMP_2])) AND (JDT1.FCCredit <> 0) OR " & _
                                                         "(JDT1.U_SCGD_Cod_Unidad = '{0}') AND (OJDT.TransId NOT IN (SELECT L.U_NoAsient FROM [@SCGD_GRLINES] AS L WITH (nolock) INNER JOIN [@SCGD_GOODRECEIVE] AS GR WITH (nolock) ON GR.DocEntry = L.DocEntry AND GR.U_Unidad = '{0}' AND L.U_NoAsient IS NOT NULL)) AND " & _
                                                         "(OJDT.RefDate <= CAST( '{1}' + ' 23:59:59 ' AS datetime)) AND (JDT1.U_SCGD_Cod_Tran IN (SELECT Code FROM [@SCGD_TRAN_COMP] AS [@SCGD_TRAN_COMP_1])) AND (JDT1.Credit <> 0) "



    Public Const strSaldosIniciales As String = "SELECT - 1 AS TransId, 'Saldo Inicial Moneda Sistema' AS Memo, ISNULL (U_TCRSalIni, 1) AS Rate, ISNULL(U_SALINID, 0) * ISNULL " & _
                                                "(U_TCRSalIni, 1) AS Local, ISNULL(U_SALINID, 0) AS Systema, NULL AS FP, NULL AS FC, '{2}' AS 'MonedaRegistro', 'SaldoInicial' as NombreTransaccion, NULL AS AcctCode " & _
                                                "FROM [@SCGD_VEHICULO] WHERE (U_Cod_Unid = '{0}') AND (U_SALINID <> 0) AND (U_SALINID IS NOT NULL) " & _
                                                "UNION " & _
                                                "SELECT - 1 AS TransID, 'Saldo Inicial Moneda Local' AS Memo, ISNULL (U_TCRSalIni, 1) AS Rate, ISNULL(U_SALINIC, 0) AS Local, ISNULL(U_SALINIC, 0) / ISNULL " & _
                                                "(U_TCRSalIni, 1) AS Systema, NULL AS FP, NULL AS FC, '{1}' AS 'Moneda Registro', 'SaldoInicial' as NombreTransaccion, NULL AS AcctCode " & _
                                                "FROM [@SCGD_VEHICULO] AS [@SCGD_VEHICULO_1] WHERE (U_Cod_Unid = '{0}') AND (U_SALINIC <> 0) AND (U_SALINIC IS NOT NULL) "


    Public Const srtNotaCreditoProveedor As String = "SELECT OJDT.TransId, RPC1.U_SCGD_Cod_Tran + ' ' + ISNULL(RPC1.Dscription, '') AS Memo, ISNULL(ORPC.SysRate, 0) AS Rate, " & _
                                                     "RPC1.LineTotal * - 1 AS Local, RPC1.LineTotal / ORPC.SysRate * - 1 AS Systema, ORPC.DocEntry AS FP, NULL AS FC, ORPC.DocCur AS 'MonedaRegistro', " & _
                                                     "RPC1.AcctCode, RPC1.U_SCGD_Cod_Tran, (SELECT  U_View FROM [@SCGD_TRAN_COMP] WHERE (Code = RPC1.U_SCGD_Cod_Tran)) AS NombreTransaccion FROM RPC1 WITH (nolock) INNER JOIN " & _
                                                     "ORPC WITH (nolock) ON RPC1.DocEntry = ORPC.DocEntry AND RPC1.TrgetEntry IS NULL INNER JOIN OJDT WITH (nolock) ON ORPC.TransId = OJDT.TransId WHERE (RPC1.U_SCGD_Cod_Unid = '{0}') AND (ORPC.DocType = '{2}' OR " & _
                                                     "ORPC.DocType = '{3}') AND (OJDT.TransId NOT IN (SELECT L.U_NoAsient FROM [@SCGD_GRLINES] AS L WITH (nolock) INNER JOIN [@SCGD_GOODRECEIVE] AS GR WITH (nolock) ON GR.DocEntry = L.DocEntry AND GR.U_Unidad = '{0}' AND L.U_NoAsient IS NOT NULL)) AND " & _
                                                     "(ORPC.DocDate <= CAST('{1}' + ' 23:59:59 ' AS datetime)) AND (RPC1.U_SCGD_Cod_Tran IN (SELECT Code FROM [@SCGD_TRAN_COMP] AS [@SCGD_TRAN_COMP_1]))"


    Public Const strAsientoSalidaInventario As String = "SELECT OJDT.TransId, OIGE.U_SCGD_Numero_OT AS Memo, OIGE.SysRate AS Rate, SUM(IGE1.StockPrice * IGE1.Quantity) AS Local, " & _
                                                        "SUM(IGE1.StockPrice * IGE1.Quantity / OIGE.SysRate) AS Systema, NULL AS FP, NULL AS FC, OIGE.DocCur AS 'MonedaRegistro', IGE1.AcctCode, NULL " & _
                                                        "AS U_SCGD_Cod_Tran, NULL AS NombreTransaccion FROM OIGE with (nolock) INNER JOIN IGE1 with (nolock) ON IGE1.DocEntry = OIGE.DocEntry INNER JOIN " & _
                                                        "OJDT with (nolock) ON OIGE.TransId = OJDT.TransId WHERE (OIGE.U_SCGD_Procesad = 1) AND (OIGE.U_SCGD_Cod_Unidad = '{0}') AND (OIGE.U_SCGD_Num_Vehiculo IS NOT NULL) AND " & _
                                                        "(OJDT.RefDate <= CAST('{1}' + ' 23:59:59 ' AS datetime)) AND (OJDT.TransId NOT IN (SELECT L.U_NoAsient FROM [@SCGD_GRLINES] AS L with (nolock) INNER JOIN " & _
                                                        "[@SCGD_GOODRECEIVE] AS GR with (nolock) ON GR.DocEntry = L.DocEntry AND GR.U_Unidad = '{0}' AND L.U_NoAsient IS NOT NULL)) " & _
                                                        "GROUP BY OJDT.TransId, OIGE.DocEntry, OIGE.DocCur, OJDT.LocTotal, OIGE.VatSum, OIGE.DocNum, OIGE.DocTotal, OIGE.U_SCGD_Numero_OT, OIGE.SysRate, IGE1.AcctCode"


    Public Const strFacturaClientes As String = "Select OJDT.TransId, U_SCGD_Cod_Tran + ' ' + Isnull(INV1.Dscription, '') Memo, " & _
                                                "Case OINV.DocRate when 0 then Sysrate else Isnull(OINV.DocRate,Sysrate) end Rate, " & _
                                                "(INV1.LineTotal*-1) Local, (INV1.TotalSumSy*-1) Systema, null as FP, OINV.DocEntry as FC, " & _
                                                "OINV.DocCur as 'MonedaRegistro', AcctCode, U_SCGD_Cod_Tran, (SELECT U_View FROM [@SCGD_TRAN_COMP] WHERE (Code = INV1.U_SCGD_Cod_Tran)) AS NombreTransaccion " & _
                                                "From INV1 with (nolock) inner join OINV with (nolock) on INV1.DocEntry = OINV.DocEntry and TrgetEntry is null inner join OJDT with (nolock) on OINV.TransID = OJDT.TransId " & _
                                                "where OINV.U_SCGD_Cod_Unidad = '{0}' and OINV.DocType = 'S' and OJDT.TransID not in (Select U_NoAsient from dbo.[@SCGD_GRLINES] L with (nolock) " & _
                                                "inner join dbo.[@SCGD_GOODRECEIVE] GR with (nolock) on GR.DocEntry = L.DocEntry and GR.U_unidad = '{0}' and U_NoAsient is not null)  and OINV.DocDate <= cast ('{1}' + ' 23:59:59 ' AS datetime)"


#End Region

#Region "Metodos"

    Public Function CargarDataTableCosteoVehiculo(ByRef p_dataTable As SAPbouiCOM.DataTable,
                                                  ByVal p_Unidad As String,
                                                  ByVal p_strFecha As String, _
                                                  ByRef udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo,
                                                  Optional ByVal p_VehiculoSinCostear As Boolean = False,
                                                  Optional ByVal p_VehiculoRecosteo As Boolean = False, _
                                                  Optional ByVal p_Entrada As String = "",
                                                  Optional ByVal blnUsaAccesorioCosteo As Boolean = False,
                                                  Optional ByVal p_strSeparadorMilesSAP As String = "",
                                                  Optional ByVal p_strSeparadorDecimalesSAP As String = "",
                                                  Optional ByVal p_TipoCambio As Decimal = 0, _
                                                  Optional ByVal p_dtFecha As Date = Nothing) As DataTable

        Dim blnIniciaTransaccionSBO As Boolean = False


        Try

            strFechaDocumento = p_strFecha
            m_decTipoCambio = p_TipoCambio

            Dim strSysCurrency As String = Utilitarios.EjecutarConsulta("Select SysCurrncy from OADM with(nolock)", m_oCompany.CompanyDB, m_oCompany.Server)
            Dim strMainCurrency As String = Utilitarios.EjecutarConsulta("Select MainCurncy from OADM with(nolock)", m_oCompany.CompanyDB, m_oCompany.Server)

            oDataTableDimensiones = New System.Data.DataTable

            objDataTable = p_dataTable

            ClsLineasDocumentosDimension = New AgregarDimensionLineasDocumentosCls(m_oCompany, m_oSBOApplication)

            'Saldos_Iniciales
            p_dataTable.ExecuteQuery(String.Format(strSaldosIniciales, p_Unidad, g_strMonedaLocal, strSysCurrency))
            If Not p_dataTable.IsEmpty Then
                CargarValoresCosteo(p_dataTable)
                blnIniciaTransaccionSBO = True
            End If

            'Documentos Compra
            If Not blnUsaAccesorioCosteo Then
                p_dataTable.ExecuteQuery(String.Format(srtFormulariosCompra, p_Unidad, p_strFecha, "S", Nothing))
            Else
                p_dataTable.ExecuteQuery(String.Format(srtFormulariosCompra, p_Unidad, p_strFecha, "S", "I"))
            End If
            If Not p_dataTable.IsEmpty Then
                CargarValoresCosteo(p_dataTable)
                blnIniciaTransaccionSBO = True
            End If

            'Asientos Manuales
            p_dataTable.ExecuteQuery(String.Format(strAsientos, p_Unidad, p_strFecha, g_strMonedaLocal, strMainCurrency, strSysCurrency))
            If Not p_dataTable.IsEmpty Then
                CargarValoresCosteo(p_dataTable)
                blnIniciaTransaccionSBO = True
            End If

            'NotaCredito compra
            If Not blnUsaAccesorioCosteo Then
                p_dataTable.ExecuteQuery(String.Format(srtNotaCreditoProveedor, p_Unidad, p_strFecha, "S", Nothing))
            Else
                p_dataTable.ExecuteQuery(String.Format(srtNotaCreditoProveedor, p_Unidad, p_strFecha, "S", "I"))
            End If

            If Not p_dataTable.IsEmpty Then
                blnNotaCredito = True
                CargarValoresCosteo(p_dataTable)
                blnIniciaTransaccionSBO = True
            Else
                blnNotaCredito = False
            End If

            'Asiento Salida de Inventario vehiculo
            p_dataTable.ExecuteQuery(String.Format(strAsientoSalidaInventario, p_Unidad, p_strFecha))
            If Not p_dataTable.IsEmpty Then
                blnVieneAsientoSalidaTaller = True
                CargarValoresCosteo(p_dataTable)
                blnIniciaTransaccionSBO = True

            End If

            p_dataTable.ExecuteQuery(String.Format(strFacturaClientes, p_Unidad, p_strFecha))
            If Not p_dataTable.IsEmpty Then
                CargarValoresCosteo(p_dataTable)
                blnIniciaTransaccionSBO = True
            End If

            If blnIniciaTransaccionSBO Then

                'carga lo concerniente al encabezado de la entrada
                DevolverDatosVehiculo(p_Unidad, strVIN, strMarca, strCodigoMarca, strEstilo, strModelo, strIDVehiculo, strTipoVehiculo, strDocRecepcion, strCodigoPedido)

                'Suma los totales del documento de Entrada de Vehiculos
                SumarTotalesPorMoneda(oListaLineasValoresCosteoPorUnidad)

                If blnUsaDimension Then
                    oDataTableDimensiones.Clear()
                    oDataTableDimensiones = (ClsLineasDocumentosDimension.DatatableDimensionesContablesDMS(strTipoVehiculo, strCodigoMarca))

                    If oDataTableDimensiones.Rows.Count <> 0 Then
                        blnAgregarDimension = True
                    End If
                End If

                m_oCompany.StartTransaction()
                CrearEntradasCosteo(oListaLineasValoresCosteoPorUnidad, udoEntrada, p_Unidad, p_dtFecha)
            Else
                m_oSBOApplication.SetStatusBarMessage("La unidad: " & p_Unidad & " no tiene costeos", BoMessageTime.bmt_Short, False)
            End If

        Catch ex As Exception

            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If

            Call Utilitarios.ManejadorErrores(ex, m_oSBOApplication)

        End Try

    End Function

    Public Sub DevolverDatosVehiculo(ByRef p_strUnidad As String, _
                                      ByRef p_strVIN As String, _
                                      ByRef p_strMarca As String, _
                                      ByRef p_strCodigoMarca As String, _
                                      ByRef p_strEstilo As String, _
                                      ByRef p_strModelo As String, _
                                      ByRef p_strIDVehiculo As String, _
                                      ByRef p_tipoVehiculo As String, _
                                      ByRef p_strDocRecepcion As String, _
                                      ByRef p_strCodigoPedido As String)

        Dim strConsulta As String

        Dim strTipoVendido As String = objConfiguracionGeneral.InventarioVehiculoVendido

        strConsulta = "Select U_Des_Marc, U_Cod_Marc , U_Des_Mode, U_Des_Esti, U_Num_VIN, Code, U_Tipo, U_Tipo_Ven, U_DocRecepcion, U_DocPedido From dbo.[@SCGD_Vehiculo] with(nolock) Where U_Cod_Unid = '" + p_strUnidad + "'"

        objDataTable.Clear()
        objDataTable.ExecuteQuery(strConsulta)

        p_strMarca = objDataTable.GetValue("U_Des_Marc", 0)
        p_strCodigoMarca = objDataTable.GetValue("U_Cod_Marc", 0)
        p_strEstilo = objDataTable.GetValue("U_Des_Esti", 0)
        p_strModelo = objDataTable.GetValue("U_Des_Mode", 0)
        p_strVIN = objDataTable.GetValue("U_Num_VIN", 0)
        p_strIDVehiculo = objDataTable.GetValue("Code", 0)
        p_strDocRecepcion = objDataTable.GetValue("U_DocRecepcion", 0)
        p_strCodigoPedido = objDataTable.GetValue("U_DocPedido", 0)
        p_tipoVehiculo = objDataTable.GetValue("U_Tipo", 0)

        If p_tipoVehiculo = strTipoVendido Then

            p_tipoVehiculo = objDataTable.GetValue("U_Tipo_Ven", 0)

        End If

    End Sub

    Private Sub CargarValoresCosteo(ByRef p_dataTable As SAPbouiCOM.DataTable)
        Dim codigo As String
        Dim strCodigoTransaccion As String = String.Empty
        Dim strNombreTransaccion As String = String.Empty

        For i As Integer = 0 To p_dataTable.Rows.Count - 1
            If Not p_dataTable.GetValue("TransId", i).ToString().Trim() = "-1" Then
                If blnVieneAsientoSalidaTaller Then
                    strCodigoTransaccion = "TALLER"
                    strNombreTransaccion = "TALLER"
                Else
                    strCodigoTransaccion = p_dataTable.GetValue("U_SCGD_Cod_Tran", i).ToString().Trim()
                    strNombreTransaccion = p_dataTable.GetValue("NombreTransaccion", i).ToString().Trim()
                End If
            Else
                strNombreTransaccion = p_dataTable.GetValue("NombreTransaccion", i).ToString().Trim()
            End If

            oListaLineasValoresCosteoPorUnidad.Add(New ListaValoresCosteo() With {.TransId = p_dataTable.GetValue("TransId", i).ToString(),
                                                                                .Memo = p_dataTable.GetValue("Memo", i).ToString(),
                                                                                .Rate = p_dataTable.GetValue("Rate", i).ToString(),
                                                                                .Local = p_dataTable.GetValue("Local", i),
                                                                                .Sistema = p_dataTable.GetValue("Systema", i),
                                                                                .DocEntryFP = p_dataTable.GetValue("FP", i).ToString(),
                                                                                .DocEntryFC = p_dataTable.GetValue("FC", i).ToString(),
                                                                                .MonedaRegistro = p_dataTable.GetValue("MonedaRegistro", i).ToString(),
                                                                                .AcctCode = p_dataTable.GetValue("AcctCode", i).ToString(),
                                                                                .CodigoTransaccion = strCodigoTransaccion,
                                                                                .NombreTransaccion = strNombreTransaccion,
                                                                                .NotaCredito = blnNotaCredito})
            codigo = String.Empty
            strCodigoTransaccion = String.Empty
            strNombreTransaccion = String.Empty
            blnNotaCredito = False
            blnVieneAsientoSalidaTaller = False
        Next
    End Sub

    'Private Function GuardaValoresLista(p_codigo As String, p_NombreTransaccion As String, p_ValorLocal As Decimal, p_ValorSistema As Decimal, p_MonedaRegistro As String, Optional p_blnTransID As Boolean = False) As List(Of ListaValoresCosteoLocal_Sistema)

    '    Dim blnExisteCodigo As Boolean = False
    '    Dim MontoLocal As Decimal = 0
    '    Dim MontoSistema As Decimal = 0
    '    Dim MontoLocal_S As Decimal = 0
    '    Dim MontoSistema_S As Decimal = 0

    '    For Each linea As ListaValoresCosteoLocal_Sistema In TablaValoresTransacciones

    '        If Not p_blnTransID Then

    '            If p_MonedaRegistro = g_strMonedaLocal Then

    '                MontoLocal = p_ValorLocal
    '                MontoSistema = p_ValorSistema
    '                MontoLocal_S = 0
    '                MontoSistema_S = 0

    '                If linea.Transaccion = p_codigo And linea.MonedaRegistro = g_strMonedaLocal Then
    '                    linea.ValorLocal = linea.ValorLocal + MontoLocal
    '                    linea.ValorSistema = linea.ValorSistema + MontoSistema
    '                    linea.ValorLocal_S = 0
    '                    linea.ValorSistema_S = 0

    '                    Sumarizar_CIF(p_MonedaRegistro, linea.NombreTransaccion, linea.ValorLocal, linea.ValorSistema)
    '                    blnExisteCodigo = True
    '                    Exit For

    '                End If

    '            Else

    '                MontoLocal = 0
    '                MontoSistema = 0
    '                MontoLocal_S = p_ValorLocal
    '                MontoSistema_S = p_ValorSistema



    '                If linea.Transaccion = p_codigo And linea.MonedaRegistro = g_strMonedaSystema Then
    '                    linea.ValorLocal = 0
    '                    linea.ValorSistema = 0
    '                    linea.ValorLocal_S = linea.ValorLocal_S + MontoLocal_S
    '                    linea.ValorSistema_S = linea.ValorSistema_S + MontoSistema_S
    '                    Sumarizar_CIF(p_MonedaRegistro, linea.NombreTransaccion, linea.ValorLocal_S, linea.ValorSistema_S)
    '                    blnExisteCodigo = True
    '                    Exit For

    '                End If

    '            End If

    '        Else

    '            If p_MonedaRegistro = g_strMonedaLocal Then
    '                decSaldoInicialLocal = decSaldoInicialLocal + p_ValorLocal

    '            Else
    '                decSaldoInicialSistema = decSaldoInicialSistema + p_ValorSistema
    '            End If
    '            Exit For

    '        End If

    '        blnExisteCodigo = False

    '    Next

    '    If Not TablaValoresTransacciones.Count <> 0 Or blnExisteCodigo = False Then

    '        If p_MonedaRegistro = g_strMonedaLocal Then
    '            MontoLocal = p_ValorLocal
    '            MontoSistema = p_ValorSistema
    '        Else
    '            MontoLocal_S = p_ValorLocal
    '            MontoSistema_S = p_ValorSistema
    '        End If

    '        TablaValoresTransacciones.Add(New ListaValoresCosteoLocal_Sistema() With {.Transaccion = p_codigo,
    '                                                                               .ValorLocal = MontoLocal,
    '                                                                               .ValorSistema = MontoSistema,
    '                                                                               .MonedaRegistro = p_MonedaRegistro,
    '                                                                                .ValorLocal_S = MontoLocal_S,
    '                                                                                .ValorSistema_S = MontoSistema_S,
    '                                                                               .NombreTransaccion = p_NombreTransaccion})

    '    End If



    'End Function

    Private Sub SumarTotalesPorMoneda(ByRef p_list As List(Of ListaValoresCosteo))
        decTotalesMonedaLocal = 0
        decTotalesMonedaSistema = 0
        decCostoTotalMonedaLocal = 0
        decCostoTotalMonedaSistema = 0

        For Each linea As ListaValoresCosteo In p_list

            If linea.MonedaRegistro = g_strMonedaLocal Then
                decTotalesMonedaLocal = decTotalesMonedaLocal + Utilitarios.ConvierteDecimal(linea.Local, n)
            Else
                decTotalesMonedaSistema = decTotalesMonedaSistema + Utilitarios.ConvierteDecimal(linea.Sistema, n)
            End If

            decCostoTotalMonedaLocal = decCostoTotalMonedaLocal + Utilitarios.ConvierteDecimal(linea.Local, n)
            decCostoTotalMonedaSistema = decCostoTotalMonedaSistema + Utilitarios.ConvierteDecimal(linea.Sistema, n)
        Next
    End Sub

    Private Sub DatosEncabezadoEntrada(ByVal p_strUnidad As String, ByVal p_strMarca As String, ByVal p_strEstilo As String, ByVal p_strModelo As String, ByVal p_strVIN As String, ByVal p_strIDVehiculo As String, ByVal p_strTipo As String, ByVal strContrato As String, ByVal p_strDocRecepcion As String, ByVal p_strCodigoPedido As String, ByVal udoEntradaVehiculo As SCG.DMSOne.Framework.UDOEntradaVehiculo, Optional ByVal p_fechaDocumento As Date = Nothing, Optional ByVal p_intAsiento As Integer = 0, Optional ByVal p_intContNumEntrada As Integer = 0, Optional ByRef p_intSerie As Integer = 0)
        udoEntradaVehiculo.Encabezado = New SCG.DMSOne.Framework.EncabezadoUDOEntradaVehiculo

        udoEntradaVehiculo.Encabezado.Series = p_intSerie
        udoEntradaVehiculo.Encabezado.NoUnidad = p_strUnidad
        udoEntradaVehiculo.Encabezado.Marca = p_strMarca
        udoEntradaVehiculo.Encabezado.Estilo = p_strEstilo
        udoEntradaVehiculo.Encabezado.Modelo = p_strModelo
        udoEntradaVehiculo.Encabezado.Vin = p_strVIN
        udoEntradaVehiculo.Encabezado.ID_Vehiculo = p_strIDVehiculo
        udoEntradaVehiculo.Encabezado.Tipo = p_strTipo
        udoEntradaVehiculo.Encabezado.DocRecepcion = p_strDocRecepcion
        udoEntradaVehiculo.Encabezado.SCGD_DocSalida = Nothing
        udoEntradaVehiculo.Encabezado.ContratoVenta = strContrato
        udoEntradaVehiculo.Encabezado.DocPedido = p_strCodigoPedido

        If p_fechaDocumento <> Nothing Then
            udoEntradaVehiculo.Encabezado.Fec_Cont = p_fechaDocumento
            udoEntradaVehiculo.Encabezado.CreateDate = p_fechaDocumento
        Else
            udoEntradaVehiculo.Encabezado.Fec_Cont = Date.Now
            udoEntradaVehiculo.Encabezado.CreateDate = Date.Now
        End If

        udoEntradaVehiculo.Encabezado.Cambio = m_decTipoCambio

    End Sub

    Private Sub CrearEntradasCosteo(ByRef p_ListaValoresCosteo As List(Of ListaValoresCosteo), ByRef udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo, ByVal p_strUnidad As String, ByVal p_fecha As Date)

        Dim p_strCampoNombreTrasaccion As String = String.Empty
        Dim intAsientoEntrada As Integer = 0
        Dim blnCreacionEntrada As Boolean = False
        Dim blnActualizaVehiculo As Boolean = False
        Dim decMontoLocal As Decimal = 0
        Dim decMontoSistema As Decimal = 0

        udoEntrada.Encabezado = New SCG.DMSOne.Framework.EncabezadoUDOEntradaVehiculo

        DatosEncabezadoEntrada(p_strUnidad, strMarca, strEstilo, strModelo, strVIN, strIDVehiculo, strTipoVehiculo, strContrato, strDocRecepcion, strCodigoPedido, udoEntrada, p_fecha)

        For Each linea As ListaValoresCosteo In p_ListaValoresCosteo
            p_strCampoNombreTrasaccion = linea.NombreTransaccion
            If linea.MonedaRegistro = g_strMonedaLocal Then
                decMontoLocal = Utilitarios.ConvierteDecimal(linea.Local, n)
                Select Case p_strCampoNombreTrasaccion
                    Case strFOB
                        udoEntrada.Encabezado.FOB = udoEntrada.Encabezado.FOB + decMontoLocal
                        CIFLocal = CIFLocal + decMontoLocal
                    Case strFLETE
                        udoEntrada.Encabezado.FLETE = udoEntrada.Encabezado.FLETE + decMontoLocal
                        CIFLocal = CIFLocal + decMontoLocal
                    Case strSEGFAC
                        udoEntrada.Encabezado.SEGFAC = udoEntrada.Encabezado.SEGFAC + decMontoLocal
                        CIFLocal = CIFLocal + decMontoLocal
                    Case strCOMFOR
                        udoEntrada.Encabezado.COMFOR = udoEntrada.Encabezado.COMFOR + decMontoLocal
                        CIFLocal = CIFLocal + decMontoLocal
                    Case strCOMNEG
                        udoEntrada.Encabezado.COMNEG = udoEntrada.Encabezado.COMNEG + decMontoLocal
                        CIFLocal = CIFLocal + decMontoLocal
                    Case strCIF
                        CIFLocal = CIFLocal + decMontoLocal
                    Case strACCINT
                        udoEntrada.Encabezado.ACCINT = udoEntrada.Encabezado.ACCINT + decMontoLocal
                    Case strACCEXT
                        udoEntrada.Encabezado.ACCEXT = udoEntrada.Encabezado.ACCEXT + decMontoLocal
                    Case strCOMAPE 'Comisión Apertura
                        udoEntrada.Encabezado.COMAPE = udoEntrada.Encabezado.COMAPE + decMontoLocal
                    Case strSEGLOC 'Seguros locales
                        udoEntrada.Encabezado.SEGLOC = udoEntrada.Encabezado.SEGLOC + decMontoLocal
                    Case strTRASLA 'Traslado
                        udoEntrada.Encabezado.TRASLA = udoEntrada.Encabezado.TRASLA + decMontoLocal
                    Case strREDEST 'Redestino
                        udoEntrada.Encabezado.REDEST = udoEntrada.Encabezado.REDEST + decMontoLocal
                    Case strBODALM 'Bodega almacen fiscal
                        udoEntrada.Encabezado.BODALM = udoEntrada.Encabezado.BODALM + decMontoLocal
                    Case strDESALM 'Desalmacenaje
                        udoEntrada.Encabezado.DESALM = udoEntrada.Encabezado.DESALM + decMontoLocal
                    Case strIMPVTA 'Impuesto
                        udoEntrada.Encabezado.IMPVTA = udoEntrada.Encabezado.IMPVTA + decMontoLocal
                    Case strAGENCIA 'Agencia
                        udoEntrada.Encabezado.AGENCIA = udoEntrada.Encabezado.AGENCIA + decMontoLocal
                    Case strFLELOC 'Flete Local
                        udoEntrada.Encabezado.FLELOC = udoEntrada.Encabezado.FLELOC + decMontoLocal
                    Case strRESERVA   'Reserva
                        udoEntrada.Encabezado.RESERVA = udoEntrada.Encabezado.RESERVA + decMontoLocal
                    Case strOTROS_FP
                        udoEntrada.Encabezado.OTROS = udoEntrada.Encabezado.OTROS + decMontoLocal
                    Case strTALLER
                        udoEntrada.Encabezado.TALLER = udoEntrada.Encabezado.TALLER + decMontoLocal
                    Case "SaldoInicial"
                        udoEntrada.Encabezado.VALHAC = decMontoLocal
                End Select
                AgregarLineaCosto(udoEntrada, linea)
            Else
                decMontoSistema = Utilitarios.ConvierteDecimal(linea.Sistema, n)
                Select Case p_strCampoNombreTrasaccion
                    Case strFOB
                        udoEntrada.Encabezado.FOB_S = udoEntrada.Encabezado.FOB_S + decMontoSistema
                        CIFSistema = CIFSistema + decMontoSistema
                    Case strFLETE
                        udoEntrada.Encabezado.FLETE_S = udoEntrada.Encabezado.FLETE_S + decMontoSistema
                        CIFSistema = CIFSistema + decMontoSistema
                    Case strSEGFAC
                        udoEntrada.Encabezado.SEGFAC_S = udoEntrada.Encabezado.SEGFAC_S + decMontoSistema
                        CIFSistema = CIFSistema + decMontoSistema
                    Case strCOMFOR
                        udoEntrada.Encabezado.COMFOR_S = udoEntrada.Encabezado.COMFOR_S + decMontoSistema
                        CIFSistema = CIFSistema + decMontoSistema
                    Case strCOMNEG
                        udoEntrada.Encabezado.COMNEG_S = udoEntrada.Encabezado.COMNEG_S + decMontoSistema
                        CIFSistema = CIFSistema + decMontoSistema
                    Case strCIF
                        CIFSistema = CIFSistema + decMontoSistema
                    Case strACCINT
                        udoEntrada.Encabezado.ACCINT_S = udoEntrada.Encabezado.ACCINT_S + decMontoSistema
                    Case strACCEXT
                        udoEntrada.Encabezado.ACCEXT_S = udoEntrada.Encabezado.ACCEXT_S + decMontoSistema
                    Case strCOMAPE 'Comisión Apertura
                        udoEntrada.Encabezado.COMAPE_S = udoEntrada.Encabezado.COMAPE_S + decMontoSistema
                    Case strSEGLOC 'Seguros locales
                        udoEntrada.Encabezado.SEGLOC_S = udoEntrada.Encabezado.SEGLOC_S + decMontoSistema
                    Case strTRASLA 'Traslado
                        udoEntrada.Encabezado.TRASLA_S = udoEntrada.Encabezado.TRASLA_S + decMontoSistema
                    Case strREDEST 'Redestino
                        udoEntrada.Encabezado.REDEST_S = udoEntrada.Encabezado.REDEST_S + decMontoSistema
                    Case strBODALM 'Bodega almacen fiscal
                        udoEntrada.Encabezado.BODALM_S = udoEntrada.Encabezado.BODALM_S + decMontoSistema
                    Case strDESALM 'Desalmacenaje
                        udoEntrada.Encabezado.DESALM_S = udoEntrada.Encabezado.DESALM_S + decMontoSistema
                    Case strIMPVTA 'Impuesto
                        udoEntrada.Encabezado.IMPVTA_S = udoEntrada.Encabezado.IMPVTA_S + decMontoSistema
                    Case strAGENCIA 'Agencia
                        udoEntrada.Encabezado.AGENCI_S = udoEntrada.Encabezado.AGENCI_S + decMontoSistema
                    Case strFLELOC 'Flete Local
                        udoEntrada.Encabezado.FLELOC_S = udoEntrada.Encabezado.FLELOC_S + decMontoSistema
                    Case strRESERVA   'Reserva
                        udoEntrada.Encabezado.RESERVA_S = udoEntrada.Encabezado.RESERVA_S + decMontoSistema
                    Case strOTROS_FP
                        udoEntrada.Encabezado.OTROS_S = udoEntrada.Encabezado.OTROS_S + decMontoSistema
                    Case strTALLER
                        udoEntrada.Encabezado.TALLER_S = udoEntrada.Encabezado.TALLER_S + decMontoSistema
                    Case "SaldoInicial"
                        udoEntrada.Encabezado.VALHAC_S = decMontoSistema
                End Select
                AgregarLineaCosto(udoEntrada, linea)
            End If
        Next

        udoEntrada.Encabezado.Tot_Loc = decTotalesMonedaLocal
        udoEntrada.Encabezado.Tot_Sis = decTotalesMonedaSistema

        udoEntrada.Encabezado.CIF_L = CIFLocal
        udoEntrada.Encabezado.CIF_S = CIFSistema

        udoEntrada.Encabezado.GASTRA = decCostoTotalMonedaLocal
        udoEntrada.Encabezado.GASTRA_S = decCostoTotalMonedaSistema

        intAsientoEntrada = CrearAsiento(p_strUnidad, True, p_fecha, strTipoVehiculo)

        udoEntrada.Encabezado.AsientoEntrada = intAsientoEntrada

        udoEntrada.Encabezado.EsTraslado = "N"

        blnCreacionEntrada = udoEntrada.Insert()

        blnActualizaVehiculo = ActualizarDatosVehiculos(strIDVehiculo, decCostoTotalMonedaLocal, decCostoTotalMonedaSistema)


        If blnActualizaVehiculo Then

            If blnCreacionEntrada And intAsientoEntrada <> 0 Then

                If m_oCompany.InTransaction Then
                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)

                End If

            Else

                If m_oCompany.InTransaction Then
                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If

            End If
        Else
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If

        End If

    End Sub

    Public Function ActualizarDatosVehiculos(ByVal strIDUnidad As String, ByVal p_decCostoTotalMonedaLocal As Decimal, ByVal p_decCostoTotalMonedaSistema As Decimal) As Boolean

        Dim oCompanyServiceVH As SAPbobsCOM.CompanyService
        Dim oGeneralServiceVH As SAPbobsCOM.GeneralService
        Dim oGeneralDataVH As SAPbobsCOM.GeneralData
        Dim oGeneralParamsVH As SAPbobsCOM.GeneralDataParams
        Dim oChildTrazabilidad As SAPbobsCOM.GeneralData
        Dim oChildrenTrazabilidad As SAPbobsCOM.GeneralDataCollection
        Dim dblTotal As Double = 0
        Dim dblValorAcumulado As Double = 0
        Dim strTotal As String = String.Empty
        Dim strTotalSistema As String = String.Empty

        Try
            oCompanyServiceVH = m_oCompany.GetCompanyService()
            oGeneralServiceVH = oCompanyServiceVH.GetGeneralService("SCGD_VEH")
            oGeneralParamsVH = oGeneralServiceVH.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParamsVH.SetProperty("Code", strIDUnidad)
            oGeneralDataVH = oGeneralServiceVH.GetByParams(oGeneralParamsVH)
            oGeneralDataVH.SetProperty("U_TIPINV", "C")
            oGeneralDataVH.SetProperty("U_SALINIC", "0")
            oGeneralDataVH.SetProperty("U_SALINID", "0")
            oChildrenTrazabilidad = oGeneralDataVH.Child("SCGD_VEHITRAZA")

            If oGeneralDataVH.Child("SCGD_VEHITRAZA").Count = 0 Then
                oChildTrazabilidad = oChildrenTrazabilidad.Add()
            Else
                oChildTrazabilidad = oChildrenTrazabilidad.Item(0)
            End If
            'Total local
            strTotal = oChildTrazabilidad.GetProperty("U_ValVeh")
            If Not String.IsNullOrEmpty(strTotal) Then
                dblValorAcumulado = Double.Parse(strTotal)
            Else
                dblValorAcumulado = 0
            End If
            dblTotal = dblValorAcumulado + p_decCostoTotalMonedaLocal
            oChildTrazabilidad.SetProperty("U_ValVeh", dblTotal)
            'Total Sistema
            strTotalSistema = oChildTrazabilidad.GetProperty("U_ValVehS")
            If Not String.IsNullOrEmpty(strTotalSistema) Then
                dblValorAcumulado = Double.Parse(strTotalSistema)
            Else
                dblValorAcumulado = 0
            End If
            dblTotal = dblValorAcumulado + p_decCostoTotalMonedaSistema
            oChildTrazabilidad.SetProperty("U_ValVehS", dblTotal)

            oGeneralServiceVH.Update(oGeneralDataVH)
            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_oSBOApplication)
            Return False
        End Try
    End Function

    Private Sub AgregarLineaCosto(ByRef udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo, linea As ListaValoresCosteo)
        If g_blnLineaAgregada = False Then
            udoEntrada.ListaLineas = New SCG.DMSOne.Framework.ListaUDOEntradaVehiculo()
            udoEntrada.ListaLineas.LineasUDO = New List(Of ILineaUDO)(1)
            g_blnLineaAgregada = True
        End If

        Dim lineaEntrada As SCG.DMSOne.Framework.LineaUDOEntradaVehiculo = New SCG.DMSOne.Framework.LineaUDOEntradaVehiculo()
        lineaEntrada.Concepto = linea.Memo
        lineaEntrada.Cuenta = linea.AcctCode
        lineaEntrada.Mon_Loc = linea.Local
        lineaEntrada.Mon_Sis = linea.Sistema
        lineaEntrada.Mon_Reg = linea.MonedaRegistro
        lineaEntrada.NoAsient = linea.TransId
        lineaEntrada.Tip_Cam = linea.Rate
        lineaEntrada.No_FC = linea.DocEntryFC
        If Not linea.NotaCredito Then
            lineaEntrada.NoFP = linea.DocEntryFP
        End If
        udoEntrada.ListaLineas.LineasUDO.Add(lineaEntrada)
    End Sub

#End Region



End Class

Public Class ListaValoresCosteo

    Public Property TransId() As String
        Get
            Return strTransId
        End Get
        Set(ByVal value As String)
            strTransId = value
        End Set
    End Property
    Private strTransId As String

    Public Property Memo() As String
        Get
            Return strMemo
        End Get
        Set(ByVal value As String)
            strMemo = value
        End Set
    End Property
    Private strMemo As String

    Public Property Rate() As Decimal
        Get
            Return decRate
        End Get
        Set(ByVal value As Decimal)
            decRate = value
        End Set
    End Property
    Private decRate As Decimal

    Public Property Local() As Decimal
        Get
            Return decLocal
        End Get
        Set(ByVal value As Decimal)
            decLocal = value
        End Set
    End Property
    Private decLocal As Decimal


    Public Property Sistema() As Decimal
        Get
            Return decSistema
        End Get
        Set(ByVal value As Decimal)
            decSistema = value
        End Set
    End Property
    Private decSistema As Decimal

    Public Property DocEntryFP() As String
        Get
            Return strDocEntryFP
        End Get
        Set(ByVal value As String)
            strDocEntryFP = value
        End Set
    End Property
    Private strDocEntryFP As String

    Public Property DocEntryFC() As String
        Get
            Return strDocEntryFC
        End Get
        Set(ByVal value As String)
            strDocEntryFC = value
        End Set
    End Property
    Private strDocEntryFC As String

    Public Property MonedaRegistro() As String
        Get
            Return strMonedaRegistro
        End Get
        Set(ByVal value As String)
            strMonedaRegistro = value
        End Set
    End Property
    Private strMonedaRegistro As String

    Public Property AcctCode() As String
        Get
            Return strAcctCode
        End Get
        Set(ByVal value As String)
            strAcctCode = value
        End Set
    End Property
    Private strAcctCode As String


    Public Property CodigoTransaccion() As String
        Get
            Return strCodigoTransaccion
        End Get
        Set(ByVal value As String)
            strCodigoTransaccion = value
        End Set
    End Property
    Private strCodigoTransaccion As String


    Public Property NombreTransaccion() As String
        Get
            Return strNombreTransaccion
        End Get
        Set(ByVal value As String)
            strNombreTransaccion = value
        End Set
    End Property
    Private strNombreTransaccion As String

    Public Property NotaCredito() As Boolean
        Get
            Return blnNotaCredito
        End Get
        Set(ByVal value As Boolean)
            blnNotaCredito = value
        End Set
    End Property
    Private blnNotaCredito As Boolean
End Class
