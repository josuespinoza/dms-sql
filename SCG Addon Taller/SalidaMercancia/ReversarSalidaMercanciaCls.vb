Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SCG.DMSOne.Framework.MenuManager
Imports SAPbouiCOM
Imports SAPbobsCOM
Imports SCG.UX.Windows
Imports System.Data.SqlClient
Imports SCG.SBOFramework
Imports SCG.SBOFramework.DI
Imports System.Collections.Generic
Imports System.Globalization


Public Class ReversarSalidaMercanciaCls

#Region "Declaraciones"
    Private m_strDireccionConfiguracion As String
    Private m_oCompany As SAPbobsCOM.Company

    Private m_objGoodReceive As ObjetoGoodReceiptCls

    Private Const mc_strUIDrCosteoMultiple As String = "SCGD_CMU"
    Private Const mc_strUIDVehículos As String = "SCGD_MNO"

    Private Const mc_strSCG_VEHICULO As String = "@SCGD_VEHICULO"
    Private Const mc_strEstadoInventario As String = "U_TIPINV"

    Public n As NumberFormatInfo

    Private WithEvents SBO_Application As SAPbouiCOM.Application
    Private m_cn_Coneccion As New SqlClient.SqlConnection
    Private m_strConectionString As String
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon


    Private m_cnConeccionTransaccion As New SqlClient.SqlConnection
    Private m_tnTransaccion As SqlClient.SqlTransaction

    Private m_decTipoCambio As Decimal
    Private m_strMonedaLocal As String
    Private m_strMonedaSistema As String
    Public m_objBLSBO As New BLSBO.GlobalFunctionsSBO

    Private m_dtsGoodReceipt As GoodReceiptDataset
    Private m_dttGoodReceipt As GoodReceiptDataset.__SCG_GOODRECEIVEDataTable
    Private m_dttGoodReceiptLines As GoodReceiptDataset.__SCG_GRLINESDataTable
    Private m_dtrGoodReceipt As GoodReceiptDataset.__SCG_GOODRECEIVERow
    Private m_dtrGoodReceiptLines As GoodReceiptDataset.__SCG_GRLINESRow

    Private strMonedaLocal As String = ""
    Private strMonedaSistema As String = ""

    Private ListaCantidadLocal As Generic.IList(Of Decimal) = New Generic.List(Of Decimal)
    Private ListaCantidadSistema As Generic.IList(Of Decimal) = New Generic.List(Of Decimal)

    Private CIFLocal As Decimal
    Private CIFSistema As Decimal


    Private intDocEntryT As Integer
    Private intSerieT As Integer

    Private ReferenciaAsientoMemo As String

    Private g_blnLineaAgregada As Boolean = False

    Private m_oJournalEntries As SAPbobsCOM.JournalEntries

    Private dtsAsientosSalidasInventario As New RecosteoDataSet
    Private dtaAsientosSalidasInventario As New RecosteoDataSetTableAdapters.AsientoSalidaInventarioTableAdapter
    Private drwAsientosSalidas As RecosteoDataSet.AsientoSalidaInventarioRow

    Private dtsAsientoContable As New AsientoContableDataSet
    Private dtaAsientoContable As New AsientoContableDataSetTableAdapters.OJDTTableAdapter
    Private drwAsientos As AsientoContableDataSet.OJDTRow

    Private intAsientoReversado As Nullable(Of Integer)


    Private ListaTipoTotal As Generic.IList(Of String) = New Generic.List(Of String)
    Private ListaTotalLocalSitema As Generic.IList(Of Decimal) = New Generic.List(Of Decimal)

    Private ListaTotalMontos As Generic.IList(Of String) = New Generic.List(Of String)



    Private Structure ItemsAsientoSalida

        Dim strCuenta As String
        Dim decValorCredit As Decimal
        Dim decValorDebit As Decimal
        Dim fechaDocDate As Date
        Dim decFvalorCredit As Decimal
        Dim decFvalorDebit As Decimal
        Dim FCurrency As String

    End Structure


    Private _dtEncabezado As SAPbouiCOM.DataTable

    Public Property dtEncabezado As DataTable
        Get
            Return _dtEncabezado
        End Get
        Set(ByVal value As DataTable)
            _dtEncabezado = value
        End Set
    End Property


#End Region

#Region "Constructor"


    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, ByRef p_ocompania As SAPbobsCOM.Company)

        m_strDireccionConfiguracion = CatchingEvents.DireccionConfiguracion
        SBO_Application = p_SBO_Aplication
        m_oCompany = p_ocompania
        Dim strCadenaConeccion As String = String.Empty
        Configuracion.CrearCadenaDeconexion(SBO_Application.Company.ServerName, SBO_Application.Company.DatabaseName, strCadenaConeccion)
        Dim m_objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strCadenaConeccion)
        'm_datFechaContabilizacion = m_objUtilitarios.CargarFechaHoraServidor


    End Sub

#End Region

#Region "Metodos"




    Public Sub ManejadorEventoItemPressedBCV(ByVal FormUID As String, _
                                                 ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                 ByRef BubbleEvent As Boolean)
        Try



        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try
    End Sub


    'ByVal p_strTransID As String, _
    '                ByVal p_strMemo As String, _
    '                ByVal p_decRate As Decimal, _
    '                ByVal p_decLocal As Decimal, _
    '                ByVal p_decSistema As Decimal, _
    '                ByVal p_strFacturaProveedor As String, _
    '                ByVal p_strFacturaCliente As String, _
    '                ByVal p_strMoneda As String, _
    '                ByVal p_strCuenta As String, _

    Public Sub CrearEntradas(ByVal p_AsientoSalida As String, _
                             ByVal p_rowSalidaMercancia As SalidaContableDataset.__SCGD_GOODISSUERow, _
                             ByVal p_dtFechaReversion As Date, _
                             ByRef p_intContNumEntrada As Integer, _
                             Optional ByVal p_Entrada As String = Nothing, _
                             Optional ByRef p_intSerie As Integer = 0) ', ByVal p_unidad As String)

        Dim m_oGoodReceive As New GoodReceiveCls(SBO_Application, m_oCompany, objConfiguracionGeneral)
        Dim strConectionString As String = ""
        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, _
                                             m_oCompany.CompanyDB, _
                                             strConectionString)
        Try
            m_strMonedaSistema = RetornarMonedaSistema()
            m_decTipoCambio = RetornarTipoCambioMoneda(m_strMonedaSistema, p_dtFechaReversion, strConectionString, False)
            m_objGoodReceive = New ObjetoGoodReceiptCls(m_cnConeccionTransaccion, m_tnTransaccion, m_strMonedaLocal, "", m_decTipoCambio, SBO_Application)

            Dim strUnidad As String = ""
            Dim strVIN As String = ""
            Dim strMarca As String = ""
            Dim strModelo As String = ""
            Dim strEstilo As String = ""
            Dim strIDVehiculo As String = ""
            Dim strTipoVehiculo As String = ""
            Dim strContrato As String = ""

            With p_rowSalidaMercancia
                If Not .IsU_UnidadNull Then
                    strUnidad = .U_Unidad
                End If
                If Not .IsU_VINNull Then
                    strVIN = .U_VIN
                End If
                If Not .IsU_MarcaNull Then
                    strMarca = .U_Marca
                End If
                If Not .IsU_ModeloNull Then
                    strModelo = .U_Modelo
                End If
                If Not .IsU_EstiloNull Then
                    strEstilo = .U_Estilo
                End If
                If Not .IsU_ID_VehNull Then
                    strIDVehiculo = .U_ID_Veh
                End If
            End With

            Dim udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo = New SCG.DMSOne.Framework.UDOEntradaVehiculo(m_oCompany)

            strTipoVehiculo = Utilitarios.EjecutarConsulta("Select U_Tipo_Ven from [@SCGD_VEHICULO] where U_Cod_Unid = '" & strUnidad & "'", m_oCompany.CompanyDB, m_oCompany.Server)



            m_objGoodReceive.Encabezado(strUnidad, strMarca, strEstilo, strModelo, strVIN, strIDVehiculo, strTipoVehiculo, strContrato, "", "", udoEntrada, p_dtFechaReversion, Convert.ToInt32(p_AsientoSalida), p_intContNumEntrada, p_intSerie) 'dtFechaDocumento)

            g_blnLineaAgregada = False

            CosteoEntrada(p_rowSalidaMercancia, udoEntrada, p_dtFechaReversion, p_AsientoSalida)

            udoEntrada.Insert()


            intDocEntryT = udoEntrada.Encabezado.DocEntry

            p_intContNumEntrada = intDocEntryT

            ' m_oGoodReceive.CrearAsientoParaNumeroEntradaEspecifico(intDocEntryT, strTipoVehiculo, dt)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Public Sub CosteoEntrada(ByVal p_rowSalidaMercancia As SalidaContableDataset.__SCGD_GOODISSUERow, _
                      ByVal udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo, _
                      ByVal p_date As Date, _
                      ByVal p_AsientoSalida As String)



        Dim objAsiento As SAPbobsCOM.JournalEntries

        Dim decSaldoInicialLocal As Decimal
        Dim decSaldoInicialSistema As Decimal

        Dim decTotalesMonedaLocal As Decimal
        Dim decTotalesMonedaSistema As Decimal



        Dim strTransID As String = ""
        Dim strMemo As String = ""
        Dim decRate As Decimal
        Dim decLocal As Decimal
        Dim decSistema As Decimal
        Dim strFacturaProveedor As String = ""
        Dim strFacturaCliente As String = ""
        Dim strMoneda As String = ""
        Dim strCuenta As String = ""

        '****************************************************************
        'datasets y datarows
        '****************************************************************

        Dim strConectionString As String = ""
        Dim cnConeccionBD As SqlClient.SqlConnection

        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, _
                                             m_oCompany.CompanyDB, _
                                             strConectionString)

        cnConeccionBD = New SqlClient.SqlConnection
        cnConeccionBD.ConnectionString = strConectionString

        cnConeccionBD.Open()

        Try

            strMemo = My.Resources.Resource.MensajeAsientoReversado & ": " & p_rowSalidaMercancia.U_As_Sali & " - " & p_rowSalidaMercancia.U_Unidad
            decLocal = Convert.ToDecimal(p_rowSalidaMercancia.U_Cos_Loc)
            decSistema = Convert.ToDecimal(p_rowSalidaMercancia.U_Cos_Sis)

            CIFLocal = 0
            CIFSistema = 0

            Dim strSeparadorDecimalesSAP As String = String.Empty
            Dim strSeparadorMilesSAP As String = String.Empty

            Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

            AgregarTotales(udoEntrada, CIFLocal, CIFSistema, strSeparadorDecimalesSAP, strSeparadorMilesSAP, decSaldoInicialLocal, decSaldoInicialSistema, decLocal, decSistema)

            AgregarLineaCosto(p_AsientoSalida, strMemo, m_decTipoCambio, decLocal, decSistema, "", "", "0", udoEntrada, "")

            cnConeccionBD.Close()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Private Sub AgregarLineaCosto(ByVal p_strTransID As String, _
                                  ByVal p_strMemo As String, _
                                  ByVal p_decRate As Decimal, _
                                  ByVal p_decLocal As Decimal, _
                                  ByVal p_decSistema As Decimal, _
                                  ByVal p_strFacturaProveedor As String, _
                                  ByVal p_strFacturaCliente As String, _
                                  ByVal p_strMoneda As String, _
                                  ByRef udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo, _
                                  ByVal p_strCuenta As String)

        If g_blnLineaAgregada = False Then

            udoEntrada.ListaLineas = New SCG.DMSOne.Framework.ListaUDOEntradaVehiculo()
            udoEntrada.ListaLineas.LineasUDO = New List(Of ILineaUDO)(1)

            g_blnLineaAgregada = True

        End If

        Dim lineaEntrada As SCG.DMSOne.Framework.LineaUDOEntradaVehiculo = New SCG.DMSOne.Framework.LineaUDOEntradaVehiculo()

        lineaEntrada.Concepto = p_strMemo
        lineaEntrada.Cuenta = p_strCuenta
        lineaEntrada.Mon_Loc = p_decLocal
        lineaEntrada.Mon_Sis = p_decSistema
        lineaEntrada.Mon_Reg = p_strMoneda
        lineaEntrada.NoAsient = p_strTransID
        lineaEntrada.Tip_Cam = p_decRate
        lineaEntrada.Cuenta = p_strCuenta
        lineaEntrada.No_FC = p_strFacturaCliente
        lineaEntrada.NoFP = p_strFacturaProveedor
        udoEntrada.ListaLineas.LineasUDO.Add(lineaEntrada)

    End Sub




    Private Function CalcularMontosTotales(Of U As {Generic.IList(Of Decimal)})(ByVal p As U) As Decimal

        Dim decTotales As Decimal
        Dim decMonto As Decimal

        For i As Integer = 0 To p.Count - 1

            decMonto = p.Item(i)

            decTotales = decTotales + decMonto

        Next

        Return decTotales

    End Function

    Private Sub AgregarTotales(ByRef udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo, ByVal p_CIFLocal As Decimal, ByVal p_CIFSistema As Decimal, _
                                        ByVal p_strSeparadorDecimalesSAP As String, ByVal p_strSeparadorMilesSAP As String, _
                                        ByVal p_decSaldoInicialLocal As Decimal, ByVal p_decSaldoInicialSistema As Decimal, _
                                        ByVal p_decTotalLocal As Decimal, ByVal p_decTotalSistema As Decimal)



        'Dim SumTotalL As Decimal =  (p_decTotalSistema * m_decTipoCambio) 
        Dim SumTotalL As Decimal = p_decTotalLocal
        Dim strSumTotalL As Decimal = Utilitarios.ConvierteDecimal(SumTotalL, n)

        udoEntrada.Encabezado.GASTRA = strSumTotalL

        If m_decTipoCambio <> 0 Then

            'Dim SumTotalS As Decimal = (p_decTotalLocal / m_decTipoCambio) 'p_decTotalSistema + 
            Dim SumTotalS As Decimal = p_decTotalSistema
            Dim strTotalS As Decimal = Utilitarios.ConvierteDecimal(SumTotalS, n)
            udoEntrada.Encabezado.GASTRA_S = strTotalS

        Else

            Dim strTotalS As Decimal = Utilitarios.ConvierteDecimal(p_decTotalSistema, n)
            udoEntrada.Encabezado.GASTRA_S = strTotalS

        End If

        Dim strCIFLocal As Decimal = Utilitarios.ConvierteDecimal(p_CIFLocal, n)
        Dim strCIFSistema As Decimal = Utilitarios.ConvierteDecimal(p_CIFSistema, n)
        Dim strTipoCambio As Decimal = Utilitarios.ConvierteDecimal(m_decTipoCambio, n)

        Dim strSaldoInicialLocal As Decimal = Utilitarios.ConvierteDecimal(p_decSaldoInicialLocal, n)
        Dim strSaldoInicialSistema As Decimal = Utilitarios.ConvierteDecimal(p_decSaldoInicialSistema, n)
        Dim strTotalLocal As Decimal = Utilitarios.ConvierteDecimal(p_decTotalLocal, n)
        Dim strTotalSistema As Decimal = Utilitarios.ConvierteDecimal(p_decTotalSistema, n)

        udoEntrada.Encabezado.Cambio = strTipoCambio
        udoEntrada.Encabezado.CIF_L = strCIFLocal
        udoEntrada.Encabezado.CIF_S = strCIFSistema
        udoEntrada.Encabezado.VALHAC = strSaldoInicialLocal
        udoEntrada.Encabezado.VALHAC_S = strSaldoInicialSistema
        udoEntrada.Encabezado.Tot_Loc = strTotalLocal
        udoEntrada.Encabezado.Tot_Sis = strTotalSistema

    End Sub
    'Public Function DevolverCostosPorUnidad(ByVal p_unidad As String, ByVal p_blnSinTrasladar As Boolean) As Generic.List(Of Decimal)

    '    Dim strConectionString As String = ""
    '    Dim decTotalLocalUnidad As Decimal = 0
    '    Dim decTotalSistemaUnidad As Decimal = 0
    '    Dim decTotalMontoLocal As Decimal = 0
    '    Dim decTotalMontoSistema As Decimal = 0
    '    Dim cnConeccionBD As SqlClient.SqlConnection
    '    Dim strcTotales As Totales = New Totales

    '    Dim strUnidad As String

    '    Dim dstTraslado As New DMS_Addon.TrasladoCostosDeUnidadesDataSet
    '    Dim dtTraslado As New DMS_Addon.TrasladoCostosDeUnidadesDataSetTableAdapters.SCGD_GOODRECEIVETableAdapter
    '    Dim drwTraslado As DMS_Addon.TrasladoCostosDeUnidadesDataSet.SCGD_GOODRECEIVERow


    '    Configuracion.CrearCadenaDeconexion(m_oCompany.Server, _
    '                                         m_oCompany.CompanyDB, _
    '                                         strConectionString)

    '    cnConeccionBD = New SqlClient.SqlConnection
    '    cnConeccionBD.ConnectionString = strConectionString
    '    cnConeccionBD.Open()
    '    dtTraslado.Connection = New SqlClient.SqlConnection(strConectionString)
    '    dtTraslado.Connection = cnConeccionBD

    '    dstTraslado.EnforceConstraints = False

    '    dtTraslado.Fill_Entradas(dstTraslado.SCGD_GOODRECEIVE, p_unidad)



    '    For Each drwTraslado In dstTraslado.SCGD_GOODRECEIVE.Rows

    '        If p_blnSinTrasladar Then

    '            If drwTraslado.U_Mon_Reg = strMonedaLocal Then

    '                decTotalMontoLocal = decTotalMontoLocal + drwTraslado.U_Mon_Loc
    '            Else
    '                decTotalMontoSistema = decTotalMontoSistema + drwTraslado.U_Mon_Sis

    '            End If


    '        End If

    '        decTotalLocalUnidad = decTotalLocalUnidad + drwTraslado.U_Mon_Loc
    '        decTotalSistemaUnidad = decTotalSistemaUnidad + drwTraslado.U_Mon_Sis




    '    Next

    '    'Dim strSeparadorDecimalesSAP As String = String.Empty
    '    'Dim strSeparadorMilesSAP As String = String.Empty
    '    'Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

    '    'decTotalLocalUnidad = CDec(Utilitarios.CambiarValoresACultureActual(decTotalLocalUnidad, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
    '    'decTotalSistemaUnidad = CDec(Utilitarios.CambiarValoresACultureActual(decTotalSistemaUnidad, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
    '    'decTotalMontoLocal = CDec(Utilitarios.CambiarValoresACultureActual(decTotalMontoLocal, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
    '    'decTotalMontoSistema = CDec(Utilitarios.CambiarValoresACultureActual(decTotalMontoSistema, strSeparadorMilesSAP, strSeparadorDecimalesSAP))


    '    ListaTipoTotal.Add(m_Local)
    '    ListaTotalLocalSitema.Add(decTotalLocalUnidad)
    '    ListaTotalMontos.Add(decTotalMontoLocal)


    '    ListaTipoTotal.Add(m_Sistema)
    '    ListaTotalLocalSitema.Add(decTotalSistemaUnidad)
    '    ListaTotalMontos.Add(decTotalMontoSistema)

    '    cnConeccionBD.Close()

    '    Return ListaTotalLocalSitema
    'End Function



    'Public Function CrearAsientoReversionSalida(ByVal p_AsSalidaMercancia As Integer) As Integer

    '    Dim intErrorAsiento As Integer

    '    Dim intNumAsiento As String = ""
    '    Dim objJournalEntries As SAPbobsCOM.JournalEntries
    '    Dim objJournalEntriesLines As SAPbobsCOM.JournalEntries_Lines
    '    Dim objItemsAsientoSalida As New Generic.List(Of ItemsAsientoSalida)
    '    Dim objItemAsSalida As New ItemsAsientoSalida
    '    Dim fechaAsiento As Date

    '    intNumAsiento = p_AsSalidaMercancia

    '    objJournalEntries = CargarAsiento(CInt(intNumAsiento))

    '    fechaAsiento = m_oJournalEntries.ReferenceDate


    '    'fechaAsiento = dtFechaDocumento ' m_oJournalEntries.ReferenceDate


    '    objJournalEntriesLines = objJournalEntries.Lines

    '    ReferenciaAsientoMemo = objJournalEntries.Reference

    '    For i As Integer = 0 To objJournalEntriesLines.Count - 1

    '        objJournalEntriesLines.SetCurrentLine(i)
    '        With objJournalEntriesLines

    '            objItemAsSalida.strCuenta = .AccountCode
    '            objItemAsSalida.decValorCredit = .Credit
    '            objItemAsSalida.decValorDebit = .Debit

    '            objItemAsSalida.decFvalorCredit = .FCCredit
    '            objItemAsSalida.FCurrency = .FCCurrency

    '            objItemAsSalida.fechaDocDate = objJournalEntries.ReferenceDate
    '            objItemsAsientoSalida.Add(objItemAsSalida)

    '        End With

    '    Next

    '    intErrorAsiento = CrearAsiento(intNumAsiento, objItemsAsientoSalida, fechaAsiento)

    '    'If blnAsientoEntradaMercancia And blnProvieneEntradaMercancia Then
    '    '    Call ActualizarEstadoEntradaMercancia(p_EntradaUsado, True)

    '    'End If


    '    'If intErrorAsiento = 0 Then
    '    '    blnAsientoEntradaMercancia = False
    '    '    blnProvieneEntradaMercancia = False
    '    '    Return 0
    '    'End If

    '    'limpio la lista 
    '    objItemsAsientoSalida.Clear()

    'End Function

    'Public Function CrearAsiento(ByVal p_intDocEntryAsientoReversar As Integer, ByVal p_lista As IList, ByVal p_fechaAsiento As Date) As Integer

    '    Dim oJournalEntry As SAPbobsCOM.JournalEntries

    '    Dim intError As Integer
    '    Dim strMensajeError As String = ""
    '    Dim strMonedaLocal As String = ""
    '    Dim m_strMonedaSistema As String = String.Empty
    '    Dim strNoAsiento As String = ""

    '    Dim strCuenta As String = ""

    '    Dim strConectionString As String = ""
    '    Dim blnPrimeraCuenta As Boolean = True
    '    Dim blnEntradaInvalida As Boolean = False

    '    Try

    '        strNoAsiento = 0
    '        oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

    '        oJournalEntry.Memo = My.Resources.Resource.MensajeAsientoReversado & ": " & p_intDocEntryAsientoReversar & " - " & ReferenciaAsientoMemo
    '        oJournalEntry.ReferenceDate = p_fechaAsiento

    '        oJournalEntry.UserFields.Fields.Item("U_SCGD_AplVal").Value = "0"

    '        For Each objlist As ItemsAsientoSalida In p_lista

    '            If Not blnPrimeraCuenta Then
    '                oJournalEntry.Lines.Add()
    '            Else
    '                blnPrimeraCuenta = False
    '            End If
    '            oJournalEntry.Lines.AccountCode = objlist.strCuenta
    '            oJournalEntry.Lines.Debit = objlist.decValorCredit
    '            oJournalEntry.Lines.FCDebit = objlist.decFvalorCredit

    '            oJournalEntry.Lines.Credit = objlist.decValorDebit
    '            oJournalEntry.Lines.FCCredit = objlist.decFvalorDebit
    '            oJournalEntry.Lines.FCCurrency = objlist.FCurrency

    '            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
    '        Next

    '        Dim Verificar As Integer = oJournalEntry.Add()

    '        If Verificar <> 0 Then
    '            strNoAsiento = "0"
    '            m_oCompany.GetLastError(intError, strMensajeError)
    '            Throw New ExceptionsSBO(Verificar, strMensajeError)
    '        Else
    '            'If blnProvieneEntradaMercancia Then
    '            '    intAsientoReversadoEntradaMercancia = m_oCompany.GetNewObjectKey
    '            'Else
    '            intAsientoReversado = m_oCompany.GetNewObjectKey
    '            'End If
    '            Return 0
    '        End If

    '        Return CInt(strNoAsiento)

    '    Catch ex As Exception

    '        Call Utilitarios.ManejadorErrores(ex, SBO_Application)

    '    End Try

    'End Function

    Public Sub ReversarAsientoSalidaMercancia(ByVal p_AsSalidaMercancia As Integer, ByVal p_strFechaReversion As String)
        Try
            Dim strAsientoReversionGenerado As String = String.Empty

            ReversaAsiento(p_AsSalidaMercancia, strAsientoReversionGenerado, p_strFechaReversion)
        Catch ex As Exception

        End Try
    End Sub




    Private Sub ReversaAsiento(ByVal intAsientoReversar As Integer, ByRef strAsientoGenerado As String, ByVal p_strFechaReversion As String)

        Dim intError As Integer
        Dim strMensajeError As String = ""
        Dim intVerificar As Integer

        Dim strFechaReversion As String
        Dim dtFechaFechaReversion As Date

        Dim objAsiento As SAPbobsCOM.JournalEntries
        Dim objAsientoLines As SAPbobsCOM.JournalEntries_Lines
        Dim oJournalEntry As SAPbobsCOM.JournalEntries

        Try

            objAsiento = CargarAsiento(intAsientoReversar)


            objAsientoLines = objAsiento.Lines

            oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            oJournalEntry.Memo = My.Resources.Resource.MensajeAsientoReversado & ": " & intAsientoReversar & " - " & ReferenciaAsientoMemo
            'oJournalEntry.ReferenceDate = p_fechaAsiento


            'strFechaReversion = oForm.DataSources.DBDataSources.Item("@SCGD_CVENTA").GetValue("U_SCGD_FDr", 0)
            'strFechaReversion = strFechaReversion.Trim()

            strFechaReversion = p_strFechaReversion

            If Not String.IsNullOrEmpty(strFechaReversion) Then
                dtFechaFechaReversion = Date.ParseExact(strFechaReversion, "yyyyMMdd", Nothing)
                dtFechaFechaReversion = New Date(dtFechaFechaReversion.Year, dtFechaFechaReversion.Month, dtFechaFechaReversion.Day, 0, 0, 0)

                oJournalEntry.ReferenceDate = dtFechaFechaReversion
            End If

            'oJournalEntry.Memo = My.Resources.Resource.AsientoReversaCont & intNumeroContrato

            For i As Integer = 0 To objAsientoLines.Count - 1

                objAsientoLines.SetCurrentLine(i)

                With objAsientoLines

                    oJournalEntry.Lines.ShortName = .ShortName
                    oJournalEntry.Lines.AccountCode = .AccountCode
                    oJournalEntry.Lines.Debit = .Credit
                    oJournalEntry.Lines.FCDebit = .FCCredit
                    oJournalEntry.Lines.Credit = .Debit
                    oJournalEntry.Lines.FCCredit = .FCDebit
                    If Not String.IsNullOrEmpty(.FCCurrency) Then
                        oJournalEntry.Lines.FCCurrency = .FCCurrency
                    End If
                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

                    oJournalEntry.Lines.Add()

                End With

            Next

            intVerificar = oJournalEntry.Add()
            If intVerificar <> 0 Then
                m_oCompany.GetLastError(intError, strMensajeError)
                Throw New ExceptionsSBO(intVerificar, strMensajeError)
            Else
                strAsientoGenerado = m_oCompany.GetNewObjectKey
            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Sub
#End Region

#Region "Cargar Objetos de SAP"
    Private Function CargarAsiento(ByVal p_NumAsiento As Integer) As SAPbobsCOM.JournalEntries

        Try
            m_oJournalEntries = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            If m_oJournalEntries.GetByKey(p_NumAsiento) Then

                Return m_oJournalEntries

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex

        End Try
        Return Nothing
    End Function

    Private Function CargarSalidaMercancia(ByVal p_NumAsiento As Integer) As DataSet

        Try


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex

        End Try
        Return Nothing
    End Function


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

        oSBObob = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
        oRecordset = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

        oRecordset = oSBObob.GetSystemCurrency()
        strResult = oRecordset.Fields.Item(0).Value

        Return strResult

    End Function

    Private Function CargarTipoCambio(ByVal p_oform As SAPbouiCOM.Form) As Boolean

        Dim strMoneda As String
        Dim strConectionString As String = String.Empty
        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)

        Dim m_objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)

        m_objBLSBO.Set_Compania(m_oCompany)
        strMonedaSistema = RetornarMonedaSistema()
        strMonedaLocal = RetornarMonedaLocal()
        If strMonedaLocal <> strMonedaSistema Then
            m_decTipoCambio = RetornarTipoCambioMoneda(strMonedaSistema, m_objUtilitarios.CargarFechaHoraServidor(), strConectionString, False)
            If m_decTipoCambio = -1 Then
                SBO_Application.MessageBox(My.Resources.Resource.TipoCambioNoActualizado)
                Return False
            End If
        Else
            m_decTipoCambio = 1
        End If

        Return True

    End Function

    Public Function RetornarTipoCambioMoneda(ByVal Moneda As String, ByVal p_Hoy As Date, ByVal strConectionString As String, ByVal blnBDExterna As Boolean) As Decimal

        Dim drdResultadoConsulta As SqlClient.SqlDataReader
        Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
        Dim cn_Coneccion As New SqlClient.SqlConnection

        Dim strValor As String = ""
        Dim sToday As String
        Dim dblResult As Double = -1

        Try
            cn_Coneccion.ConnectionString = strConectionString
            cn_Coneccion.Open()
            sToday = p_Hoy
            cmdEjecutarConsulta.Connection = cn_Coneccion

            cmdEjecutarConsulta.CommandType = CommandType.Text
            If blnBDExterna Then
                cmdEjecutarConsulta.CommandText = "SELECT Rate FROM SCGTA_VW_ORTT WHERE Currency='" & Moneda & "'" & _
                              " AND RateDate='" & CDate(sToday).ToString("yyyyMMdd") & "'"
            Else
                cmdEjecutarConsulta.CommandText = "SELECT Rate FROM ORTT WHERE Currency='" & Moneda & "'" & _
                              " AND RateDate='" & CDate(sToday).ToString("yyyyMMdd") & "'"

            End If
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()
            Do While drdResultadoConsulta.Read
                If drdResultadoConsulta.Item(0) IsNot DBNull.Value Then
                    dblResult = drdResultadoConsulta.GetDecimal(0)
                    If dblResult = 0 Then dblResult = -1
                    Exit Do
                End If
            Loop
        Catch
            Throw
        Finally
            drdResultadoConsulta.Close()
            cmdEjecutarConsulta.Connection.Close()
        End Try
        Return dblResult
    End Function

#End Region







End Class
