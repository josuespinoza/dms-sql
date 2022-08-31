Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SCG.SBOFramework
Imports SCG.DMSOne.Framework
Imports System.Collections.Generic
Imports SCG.SBOFramework.DI

Public Class ObjetoGoodReceiptCls

#Region "Declaraciones"

#Region "Objetos BD"

    Private m_cnnSCGTaller As SqlClient.SqlConnection
    Private m_cnnSCGSeries As SqlClient.SqlConnection

    Private objDAConexion As DAConexion

    Private m_dtsGoodReceipt As GoodReceiptDataset
    Private m_dttGoodReceipt As GoodReceiptDataset.__SCG_GOODRECEIVEDataTable
    Private m_dttGoodReceiptLines As GoodReceiptDataset.__SCG_GRLINESDataTable
    Private m_dtrGoodReceipt As GoodReceiptDataset.__SCG_GOODRECEIVERow
    Private m_dtrGoodReceiptLines As GoodReceiptDataset.__SCG_GRLINESRow

    Private m_dtaGoodReceipt As GoodReceiptDatasetTableAdapters._SCG_GOODRECEIVETableAdapter
    Private m_dtaGoodReceiptNumerosSerie As GoodReceiptDatasetTableAdapters._SCG_GOODRECEIVETableAdapter
    Private m_dtaGoodReceiptLines As GoodReceiptDatasetTableAdapters._SCG_GRLINESTableAdapter

#End Region

#Region "Variables Suma"

    'Variables para montos en Moneda Locales
    Private m_decComisionApertura As Decimal
    Private m_decSeguroLocal As Decimal
    Private m_decFOB As Decimal
    Private m_decFLETE As Decimal
    Private m_decSEeguroFactura As Decimal
    Private m_decComisionFormalizacion As Decimal
    Private m_decComisionNegocion As Decimal
    Private m_decCIF As Decimal
    Private m_decTraslado As Decimal
    Private m_decRedestino As Decimal
    Private m_decBodegaAlmacenaje As Decimal
    Private m_decDesalmacenaje As Decimal
    Private m_decImpuestoVenta As Decimal
    Private m_decAgencia As Decimal
    Private m_decReserva As Decimal
    Private m_decAccesoriosInternos As Decimal
    Private m_decAccesoriosExternos As Decimal
    Private m_decOtros As Decimal
    Private m_decTaller As Decimal
    Private m_decFleteLocal As Decimal
    Private m_decSaldoInicial As Decimal
    Private m_decTotalMonedaLocal As Decimal
    Private m_decTotalLocal As Decimal

    'Variables para montos en moneda sistema
    Private m_decComisionAperturaSistema As Decimal
    Private m_decSeguroLocalSistema As Decimal
    Private m_decFOBSistema As Decimal
    Private m_decFLETESistema As Decimal
    Private m_decSEeguroFacturaSistema As Decimal
    Private m_decComisionFormalizacionSistema As Decimal
    Private m_decComisionNegocionSistema As Decimal
    Private m_decCIFSistema As Decimal
    Private m_decTrasladoSistema As Decimal
    Private m_decRedestinoSistema As Decimal
    Private m_decBodegaAlmacenajeSistema As Decimal
    Private m_decDesalmacenajeSistema As Decimal
    Private m_decImpuestoVentaSistema As Decimal
    Private m_decAgenciaSistema As Decimal
    Private m_decReservaSistema As Decimal
    Private m_decAccesoriosInternosSistema As Decimal
    Private m_decAccesoriosExternosSistema As Decimal
    Private m_decOtrosSistema As Decimal
    Private m_decTallerSistema As Decimal
    Private m_decFleteLocalSistema As Decimal
    Private m_decSaldoInicialSistema As Decimal
    Private m_decTotalMonedaSistema As Decimal
    Private m_decTotalSistema As Decimal

#End Region

#Region "Variables Monedas"

    Private m_strMonedaLocal As String
    Private m_strMonedaSistema As String

    Private m_decTipoCambio As Decimal
    Private m_datFechaContabilizacion As String

#End Region

#Region "Objetos SBO"

    Dim m_SBO_Application As SAPbouiCOM.Application

#End Region

#Region "Variables Encabezado"

    Private m_intDocEntry As Integer
    Private m_intSerie As Integer

    'Private intTempSerie As Integer

#End Region

#Region "Enumeraciones"

    Public Enum enumTipoCargo

        ComisionApertura = 1
        SeguroLocal = 2
        FOB = 3
        Flete = 4
        SeguroFactura = 5
        ComisionFormalizacion = 6
        ComisionNegocion = 7
        CIF = 8
        Traslado = 8
        Redestino = 9
        BodegaAlmacenaje = 10
        Desalmacenaje = 11
        ImpuestoVenta = 12
        Agencia = 13
        Reserva = 14
        AccesoriosInternos = 15
        AccesoriosExternos = 16
        Otros = 17
        Taller = 18
        FleteLocal = 19
        SaldoInicial = 20

    End Enum

#End Region

#End Region


    Private intTempSerie As String
    Public Property SeriesDoc() As String
        Get
            Return intTempSerie
        End Get
        Set(ByVal value As String)
            intTempSerie = value
        End Set
    End Property


    Public ReadOnly Property IDGoodReceipt() As Integer
        Get
            Return m_intDocEntry
        End Get
    End Property

#Region "Constructor"

    Public Sub New(ByVal strCadenaConexion As String, ByVal strMonedaLocal As String, _
     ByVal strMonedaSistema As String, ByVal decTipoCambio As Decimal, _
     ByRef SBO_Application As SAPbouiCOM.Application)

        Dim strCadenaConeccion As String = String.Empty
        Configuracion.CrearCadenaDeconexion(SBO_Application.Company.ServerName, SBO_Application.Company.DatabaseName, strCadenaConeccion)

        Dim m_objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strCadenaConeccion)

        m_cnnSCGTaller = New SqlClient.SqlConnection(strCadenaConexion)
        m_cnnSCGSeries = New SqlClient.SqlConnection(strCadenaConexion)
        m_cnnSCGTaller.Open()
        ' m_cnnSCGSeries.Open()

        m_dtsGoodReceipt = New GoodReceiptDataset()
        m_dttGoodReceipt = m_dtsGoodReceipt.__SCG_GOODRECEIVE
        m_dttGoodReceiptLines = m_dtsGoodReceipt.__SCG_GRLINES

        m_strMonedaLocal = strMonedaLocal
        m_strMonedaSistema = strMonedaSistema
        m_decTipoCambio = decTipoCambio
        m_datFechaContabilizacion = m_objUtilitarios.CargarFechaHoraServidor

        m_dtaGoodReceipt = New GoodReceiptDatasetTableAdapters._SCG_GOODRECEIVETableAdapter
        m_dtaGoodReceiptNumerosSerie = New GoodReceiptDatasetTableAdapters._SCG_GOODRECEIVETableAdapter
        m_dtaGoodReceiptLines = New GoodReceiptDatasetTableAdapters._SCG_GRLINESTableAdapter

        m_dtaGoodReceipt.Connection = m_cnnSCGTaller
        m_dtaGoodReceiptNumerosSerie.Connection = m_cnnSCGSeries
        m_dtaGoodReceiptLines.Connection = m_cnnSCGTaller

        m_SBO_Application = SBO_Application

    End Sub

    Public Sub New(ByRef cnConeccion As SqlClient.SqlConnection, ByRef p_tnTransaccion As SqlClient.SqlTransaction, ByVal strMonedaLocal As String, _
                   ByVal strMonedaSistema As String, ByVal decTipoCambio As Decimal, _
                   ByRef SBO_Application As SAPbouiCOM.Application)

        Dim strCadenaConeccion As String = String.Empty

        Configuracion.CrearCadenaDeconexion(SBO_Application.Company.ServerName, SBO_Application.Company.DatabaseName, strCadenaConeccion)
        Dim m_objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strCadenaConeccion)

        m_cnnSCGTaller = cnConeccion
        m_cnnSCGSeries = New SqlClient.SqlConnection
        m_cnnSCGSeries.ConnectionString = strCadenaConeccion
        ' m_cnnSCGSeries.Open()

        m_dtsGoodReceipt = New GoodReceiptDataset()
        m_dttGoodReceipt = m_dtsGoodReceipt.__SCG_GOODRECEIVE
        m_dttGoodReceiptLines = m_dtsGoodReceipt.__SCG_GRLINES

        m_strMonedaLocal = strMonedaLocal
        m_strMonedaSistema = strMonedaSistema
        m_decTipoCambio = decTipoCambio
        m_datFechaContabilizacion = m_objUtilitarios.CargarFechaHoraServidor

        m_dtaGoodReceipt = New GoodReceiptDatasetTableAdapters._SCG_GOODRECEIVETableAdapter
        m_dtaGoodReceiptNumerosSerie = New GoodReceiptDatasetTableAdapters._SCG_GOODRECEIVETableAdapter
        m_dtaGoodReceiptLines = New GoodReceiptDatasetTableAdapters._SCG_GRLINESTableAdapter

        m_dtaGoodReceipt.Connection = m_cnnSCGTaller
        m_dtaGoodReceipt.SetTransaction(p_tnTransaccion)
        m_dtaGoodReceiptLines.Connection = m_cnnSCGTaller
        m_dtaGoodReceiptLines.SetTransaction(p_tnTransaccion)
        m_dtaGoodReceiptNumerosSerie.Connection = m_cnnSCGSeries

        m_SBO_Application = SBO_Application

    End Sub

    Public Sub EncabezadoUDO(ByVal p_oCompany As SAPbobsCOM.Company, ByVal p_strUnidad As String, ByVal p_strMarca As String, ByVal p_strEstilo As String, ByVal p_strModelo As String,
                             ByVal p_strVIN As String, ByVal p_strIDVehiculo As String, ByVal p_strTipo As String, ByVal strContrato As String,
                             ByVal udoEntradaVehiculo As UDOEntradaVehiculo, ByVal p_intSerie As Integer, ByVal p_intDocEntry As Integer, _
                             Optional ByVal p_fechaDocumento As Date = Nothing)

        udoEntradaVehiculo.Encabezado = New SCG.DMSOne.Framework.EncabezadoUDOEntradaVehiculo

        udoEntradaVehiculo.Encabezado.Series = p_intSerie
        udoEntradaVehiculo.Encabezado.DocNum = p_intDocEntry
        udoEntradaVehiculo.Encabezado.NoUnidad = p_strUnidad
        udoEntradaVehiculo.Encabezado.Marca = p_strMarca
        udoEntradaVehiculo.Encabezado.Estilo = p_strEstilo
        udoEntradaVehiculo.Encabezado.Modelo = p_strModelo
        udoEntradaVehiculo.Encabezado.Vin = p_strVIN
        udoEntradaVehiculo.Encabezado.ID_Vehiculo = p_strIDVehiculo
        udoEntradaVehiculo.Encabezado.Tipo = p_strTipo
        udoEntradaVehiculo.Encabezado.SCGD_DocSalida = Nothing
        udoEntradaVehiculo.Encabezado.ContratoVenta = strContrato

        If p_fechaDocumento <> Nothing Then
            udoEntradaVehiculo.Encabezado.Fec_Cont = p_fechaDocumento
            udoEntradaVehiculo.Encabezado.CreateDate = p_fechaDocumento
        Else
            udoEntradaVehiculo.Encabezado.Fec_Cont = m_datFechaContabilizacion
            udoEntradaVehiculo.Encabezado.CreateDate = m_datFechaContabilizacion
        End If

        udoEntradaVehiculo.Encabezado.Cambio = m_decTipoCambio

        udoEntradaVehiculo.Encabezado.EsTraslado = "N"

    End Sub

    Public Sub AgregarLineaUDO(ByVal p_strConcepto As String, _
                            ByVal decMonto As Decimal, ByVal strMonedaRegistro As String, _
                            ByVal intNumeroAsiento As Int64, ByVal strCuenta As String, _
                            ByVal intTipoTransaccion As enumTipoCargo, ByVal udoEntrada As UDOEntradaVehiculo, ByRef blnLineaAgregada As Boolean)

        If blnLineaAgregada = False Then
            udoEntrada.ListaLineas = New ListaUDOEntradaVehiculo()
            udoEntrada.ListaLineas.LineasUDO = New List(Of ILineaUDO)(1)
            blnLineaAgregada = True
        End If

        Dim lineaEntrada As LineaUDOEntradaVehiculo = New LineaUDOEntradaVehiculo()

        lineaEntrada.Concepto = p_strConcepto
        lineaEntrada.Cuenta = strCuenta

        If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then
            lineaEntrada.Mon_Loc = decMonto
            lineaEntrada.Mon_Sis = decMonto / m_decTipoCambio
        Else
            lineaEntrada.Mon_Loc = decMonto * m_decTipoCambio
            lineaEntrada.Mon_Sis = decMonto
        End If
        lineaEntrada.Mon_Reg = strMonedaRegistro
        lineaEntrada.NoAsient = intNumeroAsiento
        lineaEntrada.Tip_Cam = m_decTipoCambio

        Call SumarMonto(decMonto, strMonedaRegistro, intTipoTransaccion)

        udoEntrada.ListaLineas.LineasUDO.Add(lineaEntrada)

    End Sub

    Public Sub AsigarValoresSumaUDO(ByVal udoEntrada As UDOEntradaVehiculo)

        udoEntrada.Encabezado.ACCEXT = m_decAccesoriosExternos
        udoEntrada.Encabezado.ACCEXT_S = m_decAccesoriosExternosSistema
        udoEntrada.Encabezado.ACCINT = m_decAccesoriosInternos
        udoEntrada.Encabezado.ACCINT_S = m_decAccesoriosInternosSistema
        udoEntrada.Encabezado.AGENCIA = m_decAgencia
        udoEntrada.Encabezado.AGENCI_S = m_decAgenciaSistema
        udoEntrada.Encabezado.BODALM = m_decBodegaAlmacenaje
        udoEntrada.Encabezado.BODALM_S = m_decBodegaAlmacenajeSistema
        udoEntrada.Encabezado.CIF_L = m_decCIF
        udoEntrada.Encabezado.CIF_S = m_decCIFSistema
        udoEntrada.Encabezado.COMAPE = m_decComisionApertura
        udoEntrada.Encabezado.COMAPE_S = m_decComisionAperturaSistema
        udoEntrada.Encabezado.COMFOR = m_decComisionFormalizacion
        udoEntrada.Encabezado.COMFOR_S = m_decComisionFormalizacionSistema
        udoEntrada.Encabezado.COMNEG = m_decComisionNegocion
        udoEntrada.Encabezado.COMNEG_S = m_decComisionNegocionSistema
        udoEntrada.Encabezado.DESALM = m_decDesalmacenaje
        udoEntrada.Encabezado.DESALM_S = m_decDesalmacenajeSistema
        udoEntrada.Encabezado.FLETE = m_decFLETE
        udoEntrada.Encabezado.FLETE_S = m_decFLETESistema
        udoEntrada.Encabezado.FLELOC = m_decFleteLocal
        udoEntrada.Encabezado.FLETE_S = m_decFleteLocalSistema
        udoEntrada.Encabezado.FOB = m_decFOB
        udoEntrada.Encabezado.FOB_S = m_decFOBSistema
        udoEntrada.Encabezado.IMPVTA = m_decImpuestoVenta
        udoEntrada.Encabezado.IMPVTA_S = m_decImpuestoVentaSistema
        udoEntrada.Encabezado.OTROS = m_decOtros
        udoEntrada.Encabezado.OTROS_S = m_decOtrosSistema
        udoEntrada.Encabezado.REDEST = m_decRedestino
        udoEntrada.Encabezado.REDEST_S = m_decRedestinoSistema
        udoEntrada.Encabezado.RESERVA = m_decReserva
        udoEntrada.Encabezado.RESERV_S = m_decReservaSistema
        udoEntrada.Encabezado.VALHAC = m_decSaldoInicial
        udoEntrada.Encabezado.VALHAC_S = m_decSaldoInicialSistema
        udoEntrada.Encabezado.SEGFAC = m_decSEeguroFactura
        udoEntrada.Encabezado.SEGFAC_S = m_decSEeguroFacturaSistema
        udoEntrada.Encabezado.SEGLOC = m_decSeguroLocal
        udoEntrada.Encabezado.SEGLOC_S = m_decSeguroLocalSistema
        udoEntrada.Encabezado.TALLER = m_decTaller
        udoEntrada.Encabezado.TALLER_S = m_decTallerSistema
        udoEntrada.Encabezado.TRASLA = m_decTraslado
        udoEntrada.Encabezado.TRASLA_S = m_decTrasladoSistema
        udoEntrada.Encabezado.Tot_Loc = m_decTotalMonedaLocal
        udoEntrada.Encabezado.Tot_Sis = m_decTotalMonedaSistema
        udoEntrada.Encabezado.GASTRA = m_decTotalLocal
        udoEntrada.Encabezado.GASTRA_S = m_decTotalSistema

    End Sub


    Public Sub Encabezado(ByVal p_strUnidad As String,
                          ByVal p_strMarca As String,
                          ByVal p_strEstilo As String,
                          ByVal p_strModelo As String,
                          ByVal p_strVIN As String,
                          ByVal p_strIDVehiculo As String,
                          ByVal p_strTipo As String,
                          ByVal strContrato As String,
                          ByVal p_strDocRecepcion As String,
                          ByVal p_StrDocPedido As String,
                          ByVal udoEntradaVehiculo As UDOEntradaVehiculo,
                          Optional ByVal p_fechaDocumento As Date = Nothing,
                          Optional ByVal p_intAsiento As Integer = 0,
                          Optional ByVal p_intContNumEntrada As Integer = 0,
                          Optional ByRef p_intSerie As Integer = 0)

        'Dim oCompanyService As SAPbobsCOM.CompanyService
        'Dim oGeneralService As SAPbobsCOM.GeneralService
        'Dim oGeneralData As SAPbobsCOM.GeneralData
        'Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        'Dim oChild As SAPbobsCOM.GeneralData
        'Dim oChildren As SAPbobsCOM.GeneralDataCollection

        'Dim oCompanyServiceEntradaVehiculo As SAPbobsCOM.CompanyService 
        'Dim oGeneralServiceEntradaVehiculo As SAPbobsCOM.GeneralService
        'Dim oGeneralDataEntradaVehiculo As SAPbobsCOM.GeneralData
        'Dim oGeneralParamsEntradaVehiculo As SAPbobsCOM.GeneralDataParams

        udoEntradaVehiculo.Encabezado = New SCG.DMSOne.Framework.EncabezadoUDOEntradaVehiculo


        ' m_dtaGoodReceiptNumerosSerie.Connection.Open()

        'Se van a comentar las suguientes tres lineas
        If p_intContNumEntrada = 0 Then
            m_intSerie = m_dtaGoodReceiptNumerosSerie.SeleccionarSerie

            udoEntradaVehiculo.Encabezado.Series = m_intSerie
            m_intDocEntry = m_dtaGoodReceiptNumerosSerie.SeleccionarNumeroSiguiente()
            p_intSerie = m_intSerie

        Else
            m_intSerie = p_intSerie
            udoEntradaVehiculo.Encabezado.Series = m_intSerie

            m_intDocEntry = p_intContNumEntrada + 1
        End If



        'm_intDocEntry = Utilitarios.EjecutarConsulta("Select Auto FROM NNM1 WHERE Series = " & m_intSerie, m_SBO_Application.Company.DatabaseName, m_SBO_Application.Company.ServerName)
        'm_dtaGoodReceiptNumerosSerie.ActualizarNumeroSiguiente(m_intDocEntry + 1)


        m_dtrGoodReceipt = m_dttGoodReceipt.New__SCG_GOODRECEIVERow

        m_dtrGoodReceipt.DocEntry = m_intDocEntry
        m_dtrGoodReceipt.DocNum = m_intDocEntry
        udoEntradaVehiculo.Encabezado.DocNum = m_intDocEntry

        m_dtrGoodReceipt.Series = m_intSerie
        udoEntradaVehiculo.Encabezado.Series = m_intSerie

        m_dtrGoodReceipt.U_Unidad = p_strUnidad
        udoEntradaVehiculo.Encabezado.NoUnidad = p_strUnidad

        m_dtrGoodReceipt.U_Marca = p_strMarca
        udoEntradaVehiculo.Encabezado.Marca = p_strMarca

        m_dtrGoodReceipt.U_Estilo = p_strEstilo
        udoEntradaVehiculo.Encabezado.Estilo = p_strEstilo

        m_dtrGoodReceipt.U_Modelo = p_strModelo
        udoEntradaVehiculo.Encabezado.Modelo = p_strModelo

        m_dtrGoodReceipt.U_VIN = p_strVIN
        udoEntradaVehiculo.Encabezado.Vin = p_strVIN

        m_dtrGoodReceipt.U_ID_Vehiculo = p_strIDVehiculo
        udoEntradaVehiculo.Encabezado.ID_Vehiculo = p_strIDVehiculo

        m_dtrGoodReceipt.U_Tipo = p_strTipo
        udoEntradaVehiculo.Encabezado.Tipo = p_strTipo

        m_dtrGoodReceipt.U_DocRecep = p_strDocRecepcion
        udoEntradaVehiculo.Encabezado.DocRecepcion = p_strDocRecepcion

        m_dtrGoodReceipt.U_DocPedido = p_StrDocPedido
        udoEntradaVehiculo.Encabezado.DocPedido = p_StrDocPedido

        m_dtrGoodReceipt.SetU_SCGD_DocSalidaNull()
        udoEntradaVehiculo.Encabezado.SCGD_DocSalida = Nothing

        m_dtrGoodReceipt.U_Num_Cont = strContrato
        udoEntradaVehiculo.Encabezado.ContratoVenta = strContrato

        If p_fechaDocumento <> Nothing Then

            m_dtrGoodReceipt.U_Fec_Cont = p_fechaDocumento
            udoEntradaVehiculo.Encabezado.Fec_Cont = p_fechaDocumento

            m_dtrGoodReceipt.CreateDate = p_fechaDocumento
            udoEntradaVehiculo.Encabezado.CreateDate = p_fechaDocumento

        Else

            m_dtrGoodReceipt.U_Fec_Cont = m_datFechaContabilizacion
            udoEntradaVehiculo.Encabezado.Fec_Cont = m_datFechaContabilizacion

            m_dtrGoodReceipt.CreateDate = m_datFechaContabilizacion
            udoEntradaVehiculo.Encabezado.CreateDate = m_datFechaContabilizacion

        End If



        m_dtrGoodReceipt.U_Cambio = m_decTipoCambio
        udoEntradaVehiculo.Encabezado.Cambio = m_decTipoCambio

        If p_intAsiento > 0 Then
            m_dtrGoodReceipt.U_As_Entr = p_intAsiento
            udoEntradaVehiculo.Encabezado.AsientoEntrada = p_intAsiento
        Else
            m_dtrGoodReceipt.SetU_As_EntrNull()
        End If

        udoEntradaVehiculo.Encabezado.EsTraslado = "N"

        m_dttGoodReceipt.Add__SCG_GOODRECEIVERow(m_dtrGoodReceipt)


    End Sub

    Public Sub AgregarLinea(ByVal p_strConcepto As String, _
                            ByVal decMonto As Decimal, ByVal strMonedaRegistro As String, _
                            ByVal intNumeroAsiento As Int64, ByVal strCuenta As String, _
                            ByVal intTipoTransaccion As enumTipoCargo, ByVal udoEntrada As UDOEntradaVehiculo, ByRef blnLineaAgregada As Boolean)

        If blnLineaAgregada = False Then

            udoEntrada.ListaLineas = New ListaUDOEntradaVehiculo()
            udoEntrada.ListaLineas.LineasUDO = New List(Of ILineaUDO)(1)

            blnLineaAgregada = True

        End If

        Dim lineaEntrada As LineaUDOEntradaVehiculo = New LineaUDOEntradaVehiculo()

        m_dtrGoodReceiptLines = m_dttGoodReceiptLines.New__SCG_GRLINESRow
        m_dtrGoodReceiptLines.DocEntry = m_intDocEntry
        m_dtrGoodReceiptLines.SetU_No_FCNull()
        m_dtrGoodReceiptLines.SetU_NoFPNull()

        m_dtrGoodReceiptLines.U_Concepto = p_strConcepto
        lineaEntrada.Concepto = p_strConcepto

        m_dtrGoodReceiptLines.U_Cuenta = strCuenta
        lineaEntrada.Cuenta = strCuenta

        If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then
            m_dtrGoodReceiptLines.U_Mon_Loc = decMonto
            lineaEntrada.Mon_Loc = decMonto
            m_dtrGoodReceiptLines.U_Mon_Sis = decMonto / m_decTipoCambio
            lineaEntrada.Mon_Sis = decMonto / m_decTipoCambio
        Else
            m_dtrGoodReceiptLines.U_Mon_Loc = decMonto * m_decTipoCambio
            lineaEntrada.Mon_Loc = decMonto * m_decTipoCambio
            m_dtrGoodReceiptLines.U_Mon_Sis = decMonto
            lineaEntrada.Mon_Sis = decMonto

        End If
        m_dtrGoodReceiptLines.U_Mon_Reg = strMonedaRegistro
        lineaEntrada.Mon_Reg = strMonedaRegistro

        m_dtrGoodReceiptLines.U_NoAsient = intNumeroAsiento
        lineaEntrada.NoAsient = intNumeroAsiento

        m_dtrGoodReceiptLines.U_Tip_Cam = m_decTipoCambio
        lineaEntrada.Tip_Cam = m_decTipoCambio

        Call SumarMonto(decMonto, strMonedaRegistro, intTipoTransaccion) 'm_strMonedaLocal, intTipoTransaccion)

        m_dttGoodReceiptLines.Add__SCG_GRLINESRow(m_dtrGoodReceiptLines)

        udoEntrada.ListaLineas.LineasUDO.Add(lineaEntrada)

    End Sub

    Private Sub SumarMonto(ByVal decMonto As Decimal, ByVal strMonedaRegistro As String, _
                           ByVal intTipoTransaccion As enumTipoCargo)

        Select Case intTipoTransaccion
            Case enumTipoCargo.AccesoriosExternos
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then

                    m_decAccesoriosExternos += decMonto

                Else
                    m_decAccesoriosExternosSistema += decMonto
                End If
            Case enumTipoCargo.AccesoriosInternos
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then

                    m_decAccesoriosInternos += decMonto
                Else
                    m_decAccesoriosInternosSistema += decMonto
                End If
            Case enumTipoCargo.Agencia
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then

                    m_decAgencia += decMonto
                Else
                    m_decAgenciaSistema += decMonto
                End If
            Case enumTipoCargo.BodegaAlmacenaje
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then

                    m_decBodegaAlmacenaje += decMonto
                Else
                    m_decBodegaAlmacenajeSistema += decMonto
                End If
            Case enumTipoCargo.CIF
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then

                    m_decCIF += decMonto
                Else
                    m_decCIFSistema += decMonto
                End If
            Case enumTipoCargo.ComisionApertura
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then

                    m_decComisionApertura += decMonto
                Else
                    m_decComisionAperturaSistema += decMonto
                End If
            Case enumTipoCargo.ComisionFormalizacion
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then

                    m_decComisionFormalizacion += decMonto
                Else
                    m_decComisionFormalizacionSistema += decMonto
                End If
            Case enumTipoCargo.ComisionNegocion
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then

                    m_decComisionNegocion += decMonto
                Else
                    m_decComisionNegocionSistema += decMonto
                End If
            Case enumTipoCargo.Desalmacenaje
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then

                    m_decDesalmacenaje += decMonto
                Else
                    m_decDesalmacenajeSistema += decMonto
                End If
            Case enumTipoCargo.Flete
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then

                    m_decFLETE += decMonto
                Else
                    m_decFLETESistema += decMonto
                End If
            Case enumTipoCargo.FleteLocal
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then

                    m_decFleteLocal += decMonto
                Else
                    m_decFleteLocalSistema += decMonto
                End If
            Case enumTipoCargo.FOB
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then
                    m_decFOB += decMonto
                Else
                    m_decFOBSistema += decMonto
                End If
            Case enumTipoCargo.ImpuestoVenta
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then
                    m_decImpuestoVenta += decMonto
                Else
                    m_decImpuestoVentaSistema += decMonto
                End If
            Case enumTipoCargo.Otros
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then
                    m_decOtros += decMonto
                Else
                    m_decOtrosSistema += decMonto
                End If
            Case enumTipoCargo.Redestino
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then
                    m_decRedestino += decMonto
                Else
                    m_decRedestinoSistema += decMonto
                End If
            Case enumTipoCargo.Reserva
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then
                    m_decReserva += decMonto
                Else
                    m_decReservaSistema += decMonto
                End If
            Case enumTipoCargo.SaldoInicial
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then
                    m_decSaldoInicial += decMonto
                Else
                    m_decSaldoInicialSistema += decMonto
                End If
            Case enumTipoCargo.SeguroFactura
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then
                    m_decSEeguroFactura += decMonto
                Else
                    m_decSEeguroFacturaSistema += decMonto
                End If
            Case enumTipoCargo.SeguroLocal
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then
                    m_decSeguroLocal += decMonto
                Else
                    m_decSeguroLocalSistema += decMonto
                End If

            Case enumTipoCargo.Taller
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then
                    m_decTaller += decMonto
                Else
                    m_decTallerSistema += decMonto
                End If
            Case enumTipoCargo.Traslado
                If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then
                    m_decTraslado += decMonto
                Else
                    m_decTrasladoSistema += decMonto
                End If
        End Select

        If strMonedaRegistro = m_strMonedaLocal Or String.IsNullOrEmpty(strMonedaRegistro) Then
            m_decTotalMonedaLocal += decMonto
            m_decTotalLocal += decMonto
            m_decTotalSistema += decMonto / m_decTipoCambio
        Else
            m_decTotalMonedaSistema += decMonto
            m_decTotalLocal += decMonto * m_decTipoCambio
            m_decTotalSistema += decMonto
        End If

    End Sub

    Public Sub AsigarValoresSuma(ByVal udoEntrada As UDOEntradaVehiculo)

        m_dtrGoodReceipt.U_ACCEXT = m_decAccesoriosExternos
        udoEntrada.Encabezado.ACCEXT = m_decAccesoriosExternos

        m_dtrGoodReceipt.U_ACCEXT_S = m_decAccesoriosExternosSistema
        udoEntrada.Encabezado.ACCEXT_S = m_decAccesoriosExternosSistema

        m_dtrGoodReceipt.U_ACCINT = m_decAccesoriosInternos
        udoEntrada.Encabezado.ACCINT = m_decAccesoriosInternos

        m_dtrGoodReceipt.U_ACCINT_S = m_decAccesoriosInternosSistema
        udoEntrada.Encabezado.ACCINT_S = m_decAccesoriosInternosSistema

        m_dtrGoodReceipt.U_AGENCIA = m_decAgencia
        udoEntrada.Encabezado.AGENCIA = m_decAgencia

        m_dtrGoodReceipt.U_AGENCI_S = m_decAgenciaSistema
        udoEntrada.Encabezado.AGENCI_S = m_decAgenciaSistema

        m_dtrGoodReceipt.U_BODALM = m_decBodegaAlmacenaje
        udoEntrada.Encabezado.BODALM = m_decBodegaAlmacenaje

        m_dtrGoodReceipt.U_BODALM_S = m_decBodegaAlmacenajeSistema
        udoEntrada.Encabezado.BODALM_S = m_decBodegaAlmacenajeSistema

        m_dtrGoodReceipt.U_CIF_L = m_decCIF
        udoEntrada.Encabezado.CIF_L = m_decCIF

        m_dtrGoodReceipt.U_CIF_S = m_decCIFSistema
        udoEntrada.Encabezado.CIF_S = m_decCIFSistema

        m_dtrGoodReceipt.U_COMAPE = m_decComisionApertura
        udoEntrada.Encabezado.COMAPE = m_decComisionApertura

        m_dtrGoodReceipt.U_COMAPE_S = m_decComisionAperturaSistema
        udoEntrada.Encabezado.COMAPE_S = m_decComisionAperturaSistema

        m_dtrGoodReceipt.U_COMFOR = m_decComisionFormalizacion
        udoEntrada.Encabezado.COMFOR = m_decComisionFormalizacion

        m_dtrGoodReceipt.U_COMFOR_S = m_decComisionFormalizacionSistema
        udoEntrada.Encabezado.COMFOR_S = m_decComisionFormalizacionSistema

        m_dtrGoodReceipt.U_COMNEG = m_decComisionNegocion
        udoEntrada.Encabezado.COMNEG = m_decComisionNegocion

        m_dtrGoodReceipt.U_COMNEG_S = m_decComisionNegocionSistema
        udoEntrada.Encabezado.COMNEG_S = m_decComisionNegocionSistema

        m_dtrGoodReceipt.U_DESALM = m_decDesalmacenaje
        udoEntrada.Encabezado.DESALM = m_decDesalmacenaje

        m_dtrGoodReceipt.U_DESALM_S = m_decDesalmacenajeSistema
        udoEntrada.Encabezado.DESALM_S = m_decDesalmacenajeSistema

        m_dtrGoodReceipt.U_FLETE = m_decFLETE
        udoEntrada.Encabezado.FLETE = m_decFLETE

        m_dtrGoodReceipt.U_FLETE_S = m_decFLETESistema
        udoEntrada.Encabezado.FLETE_S = m_decFLETESistema

        m_dtrGoodReceipt.U_FLELOC = m_decFleteLocal
        udoEntrada.Encabezado.FLELOC = m_decFleteLocal

        m_dtrGoodReceipt.U_FLELOC_S = m_decFleteLocalSistema
        udoEntrada.Encabezado.FLETE_S = m_decFleteLocalSistema

        m_dtrGoodReceipt.U_FOB = m_decFOB
        udoEntrada.Encabezado.FOB = m_decFOB

        m_dtrGoodReceipt.U_FOB_S = m_decFOBSistema
        udoEntrada.Encabezado.FOB_S = m_decFOBSistema

        m_dtrGoodReceipt.U_IMPVTA = m_decImpuestoVenta
        udoEntrada.Encabezado.IMPVTA = m_decImpuestoVenta

        m_dtrGoodReceipt.U_IMPVTA_S = m_decImpuestoVentaSistema
        udoEntrada.Encabezado.IMPVTA_S = m_decImpuestoVentaSistema

        m_dtrGoodReceipt.U_OTROS = m_decOtros
        udoEntrada.Encabezado.OTROS = m_decOtros

        m_dtrGoodReceipt.U_OTROS_S = m_decOtrosSistema
        udoEntrada.Encabezado.OTROS_S = m_decOtrosSistema

        m_dtrGoodReceipt.U_REDEST = m_decRedestino
        udoEntrada.Encabezado.REDEST = m_decRedestino

        m_dtrGoodReceipt.U_REDEST_S = m_decRedestinoSistema
        udoEntrada.Encabezado.REDEST_S = m_decRedestinoSistema

        m_dtrGoodReceipt.U_RESERVA = m_decReserva
        udoEntrada.Encabezado.RESERVA = m_decReserva

        m_dtrGoodReceipt.U_RESERV_S = m_decReservaSistema
        udoEntrada.Encabezado.RESERV_S = m_decReservaSistema

        m_dtrGoodReceipt.U_VALHAC = m_decSaldoInicial
        udoEntrada.Encabezado.VALHAC = m_decSaldoInicial

        m_dtrGoodReceipt.U_VALHAC_S = m_decSaldoInicialSistema
        udoEntrada.Encabezado.VALHAC_S = m_decSaldoInicialSistema

        m_dtrGoodReceipt.U_SEGFAC = m_decSEeguroFactura
        udoEntrada.Encabezado.SEGFAC = m_decSEeguroFactura

        m_dtrGoodReceipt.U_SEGFAC_S = m_decSEeguroFacturaSistema
        udoEntrada.Encabezado.SEGFAC_S = m_decSEeguroFacturaSistema

        m_dtrGoodReceipt.U_SEGLOC = m_decSeguroLocal
        udoEntrada.Encabezado.SEGLOC = m_decSeguroLocal

        m_dtrGoodReceipt.U_SEGLOC_S = m_decSeguroLocalSistema
        udoEntrada.Encabezado.SEGLOC_S = m_decSeguroLocalSistema

        m_dtrGoodReceipt.U_TALLER = m_decTaller
        udoEntrada.Encabezado.TALLER = m_decTaller

        m_dtrGoodReceipt.U_TALLER_S = m_decTallerSistema
        udoEntrada.Encabezado.TALLER_S = m_decTallerSistema

        m_dtrGoodReceipt.U_TRASLA = m_decTraslado
        udoEntrada.Encabezado.TRASLA = m_decTraslado

        m_dtrGoodReceipt.U_TRASLA_S = m_decTrasladoSistema
        udoEntrada.Encabezado.TRASLA_S = m_decTrasladoSistema

        m_dtrGoodReceipt.U_Tot_Loc = m_decTotalMonedaLocal
        udoEntrada.Encabezado.Tot_Loc = m_decTotalMonedaLocal

        m_dtrGoodReceipt.U_Tot_Sis = m_decTotalMonedaSistema
        udoEntrada.Encabezado.Tot_Sis = m_decTotalMonedaSistema

        m_dtrGoodReceipt.U_GASTRA = m_decTotalLocal
        udoEntrada.Encabezado.GASTRA = m_decTotalLocal

        m_dtrGoodReceipt.U_GASTRA_S = m_decTotalSistema
        udoEntrada.Encabezado.GASTRA_S = m_decTotalSistema


    End Sub

    Public Sub GuardarValores()

        'm_dtaGoodReceipt.Update(m_dtsGoodReceipt)
        'm_dtaGoodReceiptLines.Update(m_dtsGoodReceipt)

    End Sub

    Public Sub ActualizarSerie()

        m_dtaGoodReceiptNumerosSerie.ActualizarSerie(m_intDocEntry + 1, m_intSerie)

    End Sub

#End Region

End Class
