Imports System.Collections.Generic
Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGCommon
Imports SCG.SBOFramework
Imports DMSOneFramework
Imports DMS_Addon.DocumentoProcesoCompra
Imports System.Linq
Imports System.Timers


Public Class FacturaProveedores

#Region "Definiciones"

    Private SBO_Application As SAPbouiCOM.Application
    Private SBO_Company As SAPbobsCOM.Company

    Private dtSE As SAPbouiCOM.DataTable
    Private dtImpuestos As SAPbouiCOM.DataTable
    Private dtInfoImpuestos As SAPbouiCOM.DataTable
    Private dtItemsOITM As SAPbouiCOM.DataTable
    Private ListaArticulosSE As Generic.IList(Of String)
    Private Shared oTimer As System.Timers.Timer

    Public n As NumberFormatInfo

    Private strNoAsiento As String = ""

    Private _CreaAsiento As Boolean
    Private _FormFactPro As SAPbouiCOM.Form
    Private _Burbuja As Boolean
    Private _strDocEntry As String

    'Private oDataTableDimensionesContablesDMS As SAPbouiCOM.DataTable

    'Private ListaConfiguracionOT As Hashtable
    Private ListaConfiguracionOT As List(Of LineasConfiguracionOT)

    Public Const mc_strDataTableDimensionesOT As String = "DimensionesContablesDMSOT"

    Private Const mc_strDataTableItems As String = "TablaOITM"

    Public ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls

    Private blnUsaDimensiones As Boolean = False
    Private blnUsaConfiguracionTallerInterno As Boolean = False

    'Asiento SE
    Public Const mc_strBodegaProceso As String = "BodegaProceso"

    Private blnDocAutorizacion As Boolean = False
    Private Const mc_oVentanaAutorizaciones As String = "50106"

    Private oDataTableDimensionesContablesDMS As SAPbouiCOM.DataTable
    Private oDataTableDimensiones As System.Data.DataTable
    Private oDataTableConfiguracionDocumentosDimensiones As SAPbouiCOM.DataTable
    Private ListaConfiguracion As Hashtable

    'DocumentoProcesoCompra
    Private m_oDocumentoProcesoCompra As DocumentoProcesoCompra
    Private mc_strSCGD_NoOT As String = "U_SCGD_NoOT"
    Private blnReprocesaFactura As Boolean = False

#End Region

#Region "Constructor"
    <System.CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application)
        
        SBO_Application = SBOAplication
        SBO_Company = ocompany

        n = DIHelper.GetNumberFormatInfo(SBO_Company)

    End Sub

#End Region

#Region "Enumeradores"
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
        OtrosCostosGastos = 11
        OtrosIngresos = 12
    End Enum

    Private Enum TipoDocumentoMarketingBase
        OfertaCompra = 540000006
        OrdenCompra = 22
        EntradaMercancia = 20
        FacturaProveedor = 18
        NotaCredito = 19
        DevolucionMercancia = 21
    End Enum

    Private Enum Account
        ExpensesAc = 0
        TransferAc = 1
    End Enum

    Private Enum TipoDocumentoMarketing
        OfertaCompra = 540000006
        OrdenCompra = 22
        EntradaMercancia = 20
        FacturaProveedor = 18
        NotaCredito = 19
        DevolucionMercancia = 21
    End Enum
#End Region

#Region "Propiedades"

    Public Property CreaAsiento As Boolean
        Get
            Return _CreaAsiento
        End Get
        Set(ByVal value As Boolean)
            _CreaAsiento = value
        End Set
    End Property

    Public Property FormFacPro As Form
        Get
            Return _FormFactPro
        End Get
        Set(ByVal value As Form)
            _FormFactPro = value
        End Set
    End Property


    Public Property strDocEntry As String
        Get
            Return _strDocEntry
        End Get
        Set(ByVal value As String)
            _strDocEntry = value
        End Set
    End Property

    Public Property Burbuja As Boolean
        Get
            Return _Burbuja
        End Get
        Set(ByVal value As Boolean)
            _Burbuja = value
        End Set
    End Property

#End Region

#Region "Manejo de eventos"

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent, _
                                                 ByVal FormUID As String, _
                                                 ByRef BubbleEvent As Boolean)
        Dim oForm As SAPbouiCOM.Form
        Dim Existe As Boolean
        Dim ExisteDataSourceDimensiones As Boolean
        Dim ExisteTablaItems As Boolean

        Try

            oForm = SBO_Application.Forms.GetForm(pval.FormTypeEx, pval.FormTypeCount)

            If oForm IsNot Nothing Then
                If pval.BeforeAction Then
                    If pval.ItemUID = "1" AndAlso oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then

                        blnDocAutorizacion = False
                        If pval.FormTypeEx = mc_oVentanaAutorizaciones Then
                            blnDocAutorizacion = True
                        End If


                        Existe = False
                        ExisteDataSourceDimensiones = False
                        ExisteTablaItems = False
                        If oForm.DataSources.DataTables.Count > 0 Then
                            For i As Integer = 0 To oForm.DataSources.DataTables.Count - 1

                                If oForm.DataSources.DataTables.Item(i).UniqueID = mc_strDataTableDimensionesOT Then
                                    ExisteDataSourceDimensiones = True
                                    Continue For
                                End If

                                If oForm.DataSources.DataTables.Item(i).UniqueID = mc_strDataTableItems Then
                                    ExisteTablaItems = True
                                    Continue For
                                End If

                                If oForm.DataSources.DataTables.Item(i).UniqueID = "SE" Then
                                    Existe = True
                                    Continue For
                                End If
                            Next
                        End If

                        If Not Existe Then
                            dtSE = oForm.DataSources.DataTables.Add("SE")
                            dtSE.Columns.Add("LineId", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("ItemCode", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("WhsCode", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("ImpCode", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("LineVat", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("LineVatlF", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("CtaDebe", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("CtaDebe2", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("CtaHaber", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("LineTotal", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("TotalFrgn", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("U_SCGD_NoOT", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("U_SCGD_IdRepxOrd", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("Currency", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("U_SCGD_ID", BoFieldsType.ft_AlphaNumeric, 100)


                        End If

                        If Not ExisteDataSourceDimensiones Then
                            oDataTableDimensionesContablesDMS = oForm.DataSources.DataTables.Add(mc_strDataTableDimensionesOT)
                        End If

                        If Not ExisteTablaItems Then
                            dtItemsOITM = oForm.DataSources.DataTables.Add(mc_strDataTableItems)
                        End If

                        CreaAsiento = True
                        FormFacPro = oForm


                    End If

                    Select Case pval.FormMode
                        Case SAPbouiCOM.BoFormMode.fm_ADD_MODE
                            'Implementar aquí operaciones en modo crear
                        Case SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                            If pval.ItemUID = "1" Then
                                FormFacPro = oForm
                                blnReprocesaFactura = True
                            End If
                    End Select
                ElseIf pval.ActionSuccess Then
                    Select pval.FormMode
                        Case pval.ItemUID = "1" And SAPbouiCOM.BoFormMode.fm_ADD_MODE
                            If Not blnDocAutorizacion And CreaAsiento = True Then
                                'Crear asiento de servicio externo
                                'GeneraAsientoServicioExterno()
                            End If
                            'If Not blnDocAutorizacion Then
                            '    m_oDocumentoProcesoCompra = New DocumentoProcesoCompra(SBO_Company, SBO_Application)
                            '    Call m_oDocumentoProcesoCompra.ProcesaDocumentoMarketing(FormFacPro, 0)
                            'End If
                        Case SAPbouiCOM.BoFormMode.fm_OK_MODE
                            If pval.ItemUID = "1" Then
                                If blnReprocesaFactura Then
                                    ReprocesarFactura()
                                End If
                                blnReprocesaFactura = False
                            End If
                            
                    End Select
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        Finally
            If pval.ActionSuccess Then
                If SBO_Company.InTransaction Then
                    SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                    strNoAsiento = String.Empty
                End If
            End If
        End Try
    End Sub
#End Region

#Region "Metodos Originales"
    Public Function FinalizaTransaccion() As Boolean

        'inicio de transacciones 
        Try
            Dim ProvieneEntrada As Boolean = False

            If Not String.IsNullOrEmpty(FormFacPro.DataSources.DBDataSources.Item("OPCH").GetValue("DocDate", 0)) AndAlso
                            Not String.IsNullOrEmpty(FormFacPro.DataSources.DBDataSources.Item("OPCH").GetValue("CardCode", 0)) AndAlso
                            FormFacPro.DataSources.DBDataSources.Item("PCH1").Size > 0 Then
                If SBO_Company.InTransaction Then
                    SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                    strNoAsiento = String.Empty
                End If
                If FormFacPro.DataSources.DBDataSources.Item("PCH1").GetValue("BaseType", 0).Trim = "20" Then
                    ProvieneEntrada = True
                Else
                    ProvieneEntrada = False
                End If
                If ProvieneEntrada Then

                    If Utilitarios.ValidarOTInternaConfiguracion(SBO_Company) Then

                        blnUsaConfiguracionTallerInterno = True

                    Else
                        blnUsaConfiguracionTallerInterno = False

                    End If

                    'SBO_Company.StartTransaction()
                    'strNoAsiento = CrearAsientoFacturaProveedores(SBO_Company, FormFacPro)
                    strNoAsiento = CrearAsientoFacturaProveedores(SBO_Company, FormFacPro)
                End If
            Else
                strNoAsiento = String.Empty
            End If
            'verifica que haya creado un asiento para servicios externos 
            If Not String.IsNullOrEmpty(strNoAsiento) _
                And Not strNoAsiento = "0" Then
                'commit en la transaccion 
                SBO_Company.EndTransaction(BoWfTransOpt.wf_Commit)
                strNoAsiento = String.Empty
            Else
                If SBO_Company.InTransaction Then
                    SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                    strNoAsiento = String.Empty
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                strNoAsiento = String.Empty
            End If
        Finally
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                strNoAsiento = String.Empty
            End If
        End Try
    End Function

    Public Function CrearAsientoFacturaProveedores(ByRef ocompany As SAPbobsCOM.Company,
                                                         ByVal oForm As SAPbouiCOM.Form) As Integer

        Dim oJournalEntry As SAPbobsCOM.JournalEntries
        Dim objGlobal As DMSOneFramework.BLSBO.GlobalFunctionsSBO

        Dim intError As Integer
        Dim strMensajeError As String = ""
        Dim strNoAsiento As String
        Dim decAjuste As Decimal
        Dim strContraCuenta As String
        Dim strTipo As String

        'Bodegas por servicios externos 
        Dim htBodegas_SE As New Hashtable
        'servicios externos
        Dim oListaSE As IList(Of String) = New Generic.List(Of String)

        'Lista para almacenar el numero de Orden de Trabajo
        Dim oListaNumeroOT As IList(Of String) = New Generic.List(Of String)
        Dim oListaBaseRef As IList(Of String) = New Generic.List(Of String)
        Dim oListaBodegasServiciosExternos As IList(Of String) = New Generic.List(Of String)
        Dim oListaIdRepxOrd As IList(Of String) = New Generic.List(Of String)
        'Entrega de recibidas no facturadas
        Dim htCuentas_Debe As New Hashtable
        'Servicios externos por asignar
        Dim htCuentas_Haber As New Hashtable

        'monedas
        Dim strMonedaLocal As String = String.Empty
        Dim strMonedaSistema As String = String.Empty
        Dim strMonedaFacturaProveedor As String = String.Empty
        Dim strMonedaEntradaMercancia As String = String.Empty

        'manejo de precios
        Dim strPrecio As String = String.Empty
        Dim dcPrecio As Decimal = 0
        Dim dcPrecioAcumulado As Decimal = 0
        Dim strPrecioEntrada As String
        Dim dcPrecioEntrada As Decimal = 0
        Dim dcPrecioAcumuladoEntrada As Decimal = 0
        Dim strFechaFacturaProveedor As String = String.Empty
        Dim strTipoCambioFactura As String = String.Empty

        'manejo de impuestos 
        Dim strCodeImp As String = String.Empty
        Dim strCtaImp As String = String.Empty
        Dim strCantImpuestos As String = String.Empty
        Dim dcICantImpuestos As Decimal = 0
        Dim dcICantImpuestosAcumulado As Decimal = 0
        Dim dcImp As Decimal = 0
        Dim dcDiferencia As Decimal = 0

        Dim dcValorRetorno As Decimal = 0
        Dim strMemo As String = String.Empty
        Dim strNumeroOT As String = String.Empty
        Dim strBaseRef As String = String.Empty

        Dim counter As Integer = 0

        Dim strFechaDoc1 As Date
        Dim dtFechaDoc1 As Date

        Dim strCampoConsulta As String = ""

        'proyecto SAP
        Dim strProyecto As String = String.Empty

        Dim oListaNumeroOTValidados As IList(Of String) = New Generic.List(Of String)

        Dim DataTableValoresCotizacion As System.Data.DataTable

        Dim blnAgregarDimension As Boolean = False

        Dim strTipoOT As String = String.Empty

        ValidarConfiguracionDimensiones(oForm)

        Dim strNombreColumnaID As String = String.Empty

        'carga servicios externos
        If Not oForm.DataSources.DBDataSources.Item("PCH1") Is Nothing Then
            'htBodegas_SE = CargaServiciosExternos(oForm, oListaSE, strBaseRef, oListaNumeroOT, oListaBaseRef)
            oListaBodegasServiciosExternos = CargaServiciosExternos(oForm, oListaSE, strBaseRef, oListaNumeroOT, oListaBaseRef, oListaIdRepxOrd)
        End If

        If oForm.DataSources.DataTables.Item("SE").Rows.Count <= 0 Then
            Exit Function
        End If

        If oForm.DataSources.DataTables.Item("SE").Rows.Count > 0 Then
            ObtieneCuentasYBodegas(oListaSE, htCuentas_Debe, htCuentas_Haber, oForm, htBodegas_SE, oListaBodegasServiciosExternos)
            ObtieneImpuestos(oForm)
        End If
        strNoAsiento = 0

        oJournalEntry = ocompany.GetBusinessObject(BoObjectTypes.oJournalEntries)
        strNumeroOT = oForm.DataSources.DBDataSources.Item("OPCH").GetValue("U_SCGD_Numero_OT", 0).Trim()

        'proyecto sap
        'strProyecto = Utilitarios.DevuelveCodeProyecto(strNumeroOT, SBO_Application)

        oJournalEntry.Reference = strNumeroOT
        strFechaFacturaProveedor = oForm.DataSources.DBDataSources.Item("OPCH").GetValue("DocDate", 0).Trim()
        strFechaDoc1 = Date.ParseExact(strFechaFacturaProveedor, "yyyyMMdd", Nothing)

        dtFechaDoc1 = Utilitarios.RetornaFechaFormatoRegional(strFechaDoc1)

        strMemo = My.Resources.Resource.AsientoFacturaProveedores +
        oForm.DataSources.DBDataSources.Item("OPCH").GetValue("DocNum", 0).Trim()

        strMonedaFacturaProveedor = oForm.DataSources.DBDataSources.Item("OPCH").GetValue("DocCur", 0).Trim()
        strTipoCambioFactura = oForm.DataSources.DBDataSources.Item("OPCH").GetValue("DocRate", 0).Trim()

        oJournalEntry.ReferenceDate = dtFechaDoc1

        strMonedaLocal = RetornarMonedaLocal()
        strMonedaSistema = RetornarMonedaSistema()

        oJournalEntry.Memo = strMemo
        Dim Contador As Integer = 0

        'SERVICIO EXTERNO ************************************************************** 
        Dim row As System.Data.DataRow
        Dim strValorDimension As String
        Dim strTipoOTLista As String = String.Empty

        Dim strOTs As String = String.Empty
        Dim numOT As String = String.Empty
        Dim query As String = String.Empty
        Dim dtOTs As System.Data.DataTable

        '''''Se obtiene la info de las OT de la factura**********************************************************************************

        For Each numOT In oListaNumeroOT
            If Not strOTs.Contains(numOT) Then
                strOTs = strOTs & String.Format("'{0}', ", numOT)
            End If
        Next
        If (strOTs.Length > 0) Then
            strOTs = strOTs.Substring(0, strOTs.Length - 2)
            query = String.Format("select Q.U_SCGD_Tipo_OT, Q.U_SCGD_idSucursal, Q.U_SCGD_Cod_Marca, Q.U_SCGD_Numero_OT from OQUT Q with (nolock) where Q.U_SCGD_Numero_OT in ({0})", strOTs)
            dtOTs = Utilitarios.EjecutarConsultaDataTable(query, SBO_Company.CompanyDB, SBO_Company.Server)
        End If

        '''''Se obtiene la info de las OT de la factura**********************************************************************************

        If dtOTs.Rows.Count > 0 Then
            Dim lapso = 5
            Dim contadorLapso = 0

            For Each row In dtOTs.Rows
                contadorLapso = contadorLapso + 1
                If ((contadorLapso Mod lapso) = 0) Or contadorLapso = 1 Then
                    Dim message As String = My.Resources.Resource.TXTProcessItemsExternalService
                    Dim mess = String.Format(message, contadorLapso.ToString(), dtOTs.Rows.Count.ToString())
                    SBO_Application.StatusBar.SetText(mess, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                End If
                Dim strNoOt As String = row.Item("U_SCGD_Numero_OT").ToString().Trim()

                If blnUsaDimensiones Then

                    strTipoOT = row.Item("U_SCGD_Tipo_OT").ToString().Trim()

                    strValorDimension = ClsLineasDocumentosDimension.ValidacionAsientosDimensiones(ListaConfiguracionOT, strTipoOT, False, True)

                    '******************************************************************************************
                    'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                    If Not String.IsNullOrEmpty(strValorDimension) Then
                        If strValorDimension = "Y" Then
                            oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContablesOrdenTrabajo(oForm, row.Item(1), row.Item(2), oDataTableDimensionesContablesDMS))

                            If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                                blnAgregarDimension = True
                            End If

                        End If
                    End If
                    '******************************************************************************************
                End If

                dcValorRetorno = 0
                If strMonedaFacturaProveedor = strMonedaLocal Then
                    dcPrecio = RetornaCampo(oForm, 0, "LineTotal", True, False, strNoOt)
                Else
                    dcPrecio = RetornaCampo(oForm, 0, "TotalFrgn", True, False, strNoOt)
                End If

                Contador = Contador + 1
                dcPrecioAcumulado = Decimal.Parse(dcPrecioAcumulado) + Decimal.Parse(dcPrecio)

                Dim strFilters As String = String.Empty
                Dim filters As String = String.Empty
                Dim querySE As String = String.Empty
                Dim dtSE_Pdn1 As System.Data.DataTable
                Dim rowDtSe As System.Data.DataRow

                For Each ServExt As String In oListaSE
                    counter = counter + 1
                    If Not blnUsaConfiguracionTallerInterno Then
                        filters = String.Format("(P1.U_SCGD_NoOT = '{0}' AND P1.ItemCode = '{1}' AND P.DocNum = '{2}' and P1.U_SCGD_IdRepxOrd = '{3}') or ", strNoOt, ServExt, oListaBaseRef.Item(counter - 1), oListaIdRepxOrd.Item(counter - 1))
                    Else
                        filters = String.Format("(P1.U_SCGD_NoOT = '{0}' AND P1.ItemCode = '{1}' AND P.DocNum = '{2}' and P1.U_SCGD_ID = '{3}') or ", strNoOt, ServExt, oListaBaseRef.Item(counter - 1), oListaIdRepxOrd.Item(counter - 1))
                    End If

                    If Not String.IsNullOrEmpty(filters) Then
                        strFilters = String.Format("{0}{1}", strFilters, filters.Trim())
                    End If
                Next

                strFilters = strFilters.Substring(0, strFilters.Length - 3)

                If Not blnUsaConfiguracionTallerInterno Then
                    querySE = String.Format("SELECT P1.ItemCode, P1.Currency, P1.LineTotal, P1.TotalFrgn, P1.U_SCGD_IdRepxOrd FROM PDN1 AS P1 with (nolock) INNER JOIN OPDN AS P with (nolock) ON P1.DocEntry = P.DocEntry WHERE {0} GROUP BY P1.ItemCode , P1.Currency, P1.LineTotal, P1.TotalFrgn, P1.U_SCGD_IdRepxOrd", strFilters)
                Else
                    querySE = String.Format("SELECT P1.ItemCode, P1.Currency, P1.LineTotal, P1.TotalFrgn, P1.U_SCGD_ID FROM PDN1 AS P1 with (nolock) INNER JOIN OPDN AS P with (nolock) ON P1.DocEntry = P.DocEntry WHERE {0} GROUP BY P1.ItemCode , P1.Currency, P1.LineTotal, P1.TotalFrgn, P1.U_SCGD_ID", strFilters)
                End If


                dtSE_Pdn1 = Utilitarios.EjecutarConsultaDataTable(querySE, SBO_Company.CompanyDB, SBO_Company.Server)

                If oForm.DataSources.DataTables.Item("SE").Rows.Count > 0 Then

                    'For Each ServExt As String In oListaSE
                    If Not blnUsaConfiguracionTallerInterno Then
                        strNombreColumnaID = "U_SCGD_IdRepxOrd"
                    Else
                        strNombreColumnaID = "U_SCGD_ID"
                    End If

                    For j As Integer = 0 To oForm.DataSources.DataTables.Item("SE").Rows.Count - 1
                        If oForm.DataSources.DataTables.Item("SE").GetValue("U_SCGD_NoOT", j).ToString().Trim() = strNoOt Then
                            'Exit For

                            'U_SCGD_IdRepxOrd
                            Dim idRepuestosXOrden As String = oForm.DataSources.DataTables.Item("SE").GetValue("U_SCGD_IdRepxOrd", j).ToString().Trim()
                            Dim idLineas As String = oForm.DataSources.DataTables.Item("SE").GetValue("U_SCGD_ID", j).ToString().Trim()

                            Dim rowSe As System.Data.DataRow() = dtSE_Pdn1.Select("ItemCode ='" & oForm.DataSources.DataTables.Item("SE").GetValue("ItemCode", j).ToString().Trim() & "' and " & strNombreColumnaID & " = '" & _
                                                                                  oForm.DataSources.DataTables.Item("SE").GetValue(strNombreColumnaID, j).ToString().Trim() & "'")
                            If (rowSe.Length > 0) Then

                                strMonedaEntradaMercancia = oForm.DataSources.DataTables.Item("SE").GetValue("Currency", j).ToString().Trim()

                                If strMonedaEntradaMercancia = strMonedaLocal Then
                                    strCampoConsulta = "LineTotal"
                                Else
                                    strCampoConsulta = "TotalFrgn"
                                End If

                                'costo moneda local
                                strPrecioEntrada = ""

                                Dim decSumPrecio As Decimal = 0
                                For Each itemDR As System.Data.DataRow In rowSe
                                    Dim precio As String = itemDR.Item(strCampoConsulta).ToString().Trim()
                                    Dim decPrecio As Decimal = 0
                                    If Not String.IsNullOrEmpty(precio) Then
                                        decPrecio = Convert.ToDecimal(precio)
                                    End If
                                    decSumPrecio = decSumPrecio + decPrecio
                                Next
                                dcPrecioEntrada = 0

                                If decSumPrecio > 0 Then dcPrecioEntrada = decSumPrecio

                                dcPrecioEntrada = Utilitarios.ManejoMultimoneda(dcPrecioEntrada, strMonedaLocal, strMonedaSistema,
                                                                                strMonedaEntradaMercancia, strMonedaFacturaProveedor,
                                                                                strTipoCambioFactura, dtFechaDoc1, n,
                                                                                SBO_Company)

                                dcPrecioAcumuladoEntrada = dcPrecioAcumuladoEntrada + dcPrecioEntrada
                            End If
                        End If
                    Next
                End If
                'SERVICIO EXTERNO ************************************************************** 
                'GENERA ASIENTOS ****************************************************************

                counter = 0
                oJournalEntry.Lines.AccountCode = oForm.DataSources.DataTables.Item("SE").GetValue("CtaHaber", 0).Trim()

                oJournalEntry.Lines.Reference1 = strNoOt

                If strMonedaFacturaProveedor = strMonedaLocal Then
                    oJournalEntry.Lines.Credit = dcPrecioAcumulado
                Else
                    oJournalEntry.Lines.FCCredit = dcPrecioAcumulado
                    oJournalEntry.Lines.FCCurrency = strMonedaFacturaProveedor
                End If
                'oJournalEntry.Lines.Reference1 = strRef1

                'proyectos
                oJournalEntry.Lines.ProjectCode = strProyecto

                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

                If blnAgregarDimension Then
                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)
                End If

                oJournalEntry.Lines.Add()

                'COSTOS ************************************************************************ 

                oJournalEntry.Lines.AccountCode = oForm.DataSources.DataTables.Item("SE").GetValue("CtaDebe", 0).Trim()
                dcValorRetorno = 0
                oJournalEntry.Lines.Reference1 = strNoOt

                If strMonedaFacturaProveedor = strMonedaLocal Then
                    oJournalEntry.Lines.Debit = Decimal.Parse(dcPrecioAcumuladoEntrada)
                Else
                    oJournalEntry.Lines.FCDebit = Decimal.Parse(dcPrecioAcumuladoEntrada)
                    oJournalEntry.Lines.FCCurrency = strMonedaFacturaProveedor
                End If

                'proyectos
                oJournalEntry.Lines.ProjectCode = strProyecto

                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

                If blnAgregarDimension Then
                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)
                End If

                oJournalEntry.Lines.Add()


                dcDiferencia = 0
                dcDiferencia = dcPrecioAcumulado - dcPrecioAcumuladoEntrada

                If dcDiferencia <> 0 Then
                    oJournalEntry.Lines.AccountCode = oForm.DataSources.DataTables.Item("SE").GetValue("CtaDebe2", 0).Trim()
                    dcValorRetorno = 0
                    oJournalEntry.Lines.Reference1 = strNoOt

                    If strMonedaFacturaProveedor = strMonedaLocal Then
                        oJournalEntry.Lines.Debit = Decimal.Parse(dcDiferencia)
                    Else
                        oJournalEntry.Lines.FCDebit = Decimal.Parse(dcDiferencia)
                        oJournalEntry.Lines.FCCurrency = strMonedaFacturaProveedor
                    End If

                    'proyectos
                    oJournalEntry.Lines.ProjectCode = strProyecto

                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    oJournalEntry.Lines.Add()
                End If

                dcPrecioAcumuladoEntrada = 0
                dcDiferencia = 0
                dcPrecioAcumulado = 0
            Next
        End If
        'COSTOS *************************************************************************
        'GENERA ASIENTOS ****************************************************************

        If SBO_Company.InTransaction = False Then
            SBO_Company.StartTransaction()
        End If
        If oJournalEntry.Add <> 0 Then
            strNoAsiento = "0"
            ocompany.GetLastError(intError, strMensajeError)
            Throw New ExceptionsSBO(intError, strMensajeError)
        Else
            dcPrecio = 0
            dcPrecioAcumulado = 0
            dcICantImpuestos = 0
            dcICantImpuestosAcumulado = 0
            ocompany.GetNewObjectCode(strNoAsiento)
            oListaBaseRef.Clear()
            oListaBodegasServiciosExternos.Clear()
            oListaNumeroOT.Clear()
            oListaSE.Clear()
            oListaIdRepxOrd.Clear()
        End If
        Return CInt(strNoAsiento)
    End Function

    ''' <summary>
    ''' Carga datatable con Codigos y Cuentas de impuestos 
    ''' </summary>
    ''' <param name="oForm">Objeto formulario</param>
    ''' <remarks></remarks>
    Private Sub ObtieneImpuestos(ByVal oForm As Form)

        Dim Existe As Boolean

        Existe = False
        If oForm.DataSources.DataTables.Count > 0 Then
            For i As Integer = 0 To oForm.DataSources.DataTables.Count - 1
                If oForm.DataSources.DataTables.Item(i).UniqueID = "IMP" Then
                    oForm.DataSources.DataTables.Item("IMP").Clear()
                    dtImpuestos.Columns.Add("Code", BoFieldsType.ft_AlphaNumeric, 100)
                    dtImpuestos.Columns.Add("SalesTax", BoFieldsType.ft_AlphaNumeric, 100)
                    Existe = True
                    Exit For
                End If
            Next
        End If

        If Not Existe Then
            dtImpuestos = oForm.DataSources.DataTables.Add("IMP")
            dtImpuestos.Columns.Add("Code", BoFieldsType.ft_AlphaNumeric, 100)
            dtImpuestos.Columns.Add("SalesTax", BoFieldsType.ft_AlphaNumeric, 100)
        End If

        dtImpuestos.ExecuteQuery("SELECT Code , SalesTax FROM OSTA")

    End Sub

    ''' <summary>
    ''' crea un datatable para manejo de la informaicon de impuestos 
    ''' </summary>
    ''' <param name="oForm">Objeto formulario</param>
    ''' <remarks></remarks>
    Private Sub CreaDTInfoImp(ByVal oForm As Form)
        Dim Existe As Boolean

        Existe = False
        If oForm.DataSources.DataTables.Count > 0 Then
            For i As Integer = 0 To oForm.DataSources.DataTables.Count - 1
                If oForm.DataSources.DataTables.Item(i).UniqueID = "INFOIMP" Then
                    oForm.DataSources.DataTables.Item("INFOIMP").Clear()
                    dtInfoImpuestos.Columns.Add("SE", BoFieldsType.ft_AlphaNumeric, 100)
                    dtInfoImpuestos.Columns.Add("ImpCode", BoFieldsType.ft_AlphaNumeric, 100)
                    dtInfoImpuestos.Columns.Add("SalesTax", BoFieldsType.ft_AlphaNumeric, 100)
                    dtInfoImpuestos.Columns.Add("LineVat", BoFieldsType.ft_AlphaNumeric, 100)
                    Existe = True
                    Exit For
                End If
            Next
        End If

        If Not Existe Then
            dtInfoImpuestos = oForm.DataSources.DataTables.Add("INFOIMP")
            dtInfoImpuestos.Columns.Add("SE", BoFieldsType.ft_AlphaNumeric, 100)
            dtInfoImpuestos.Columns.Add("ImpCode", BoFieldsType.ft_AlphaNumeric, 100)
            dtInfoImpuestos.Columns.Add("SalesTax", BoFieldsType.ft_AlphaNumeric, 100)
            dtInfoImpuestos.Columns.Add("LineVat", BoFieldsType.ft_AlphaNumeric, 100)
        End If
    End Sub

    ''' <summary>
    ''' Carga Un datatable con la informacion de los servicios externos
    ''' Asi como un HashTable con las bodegas de cada servicio externo
    ''' </summary>
    ''' <param name="oForm">Objeto formulario</param>
    ''' <param name="oListaSE">Lista con los servicios externos</param>
    ''' <returns>HashTable con las bodegas de cada servicio externo</returns>
    ''' <remarks></remarks>
    Private Function CargaServiciosExternos(ByVal oForm As SAPbouiCOM.Form,
                                            ByRef oListaSE As IList(Of String),
                                            ByRef strNumEntrada As String, Optional ByRef oListaNumeroOT As IList(Of String) = Nothing, _
                                            Optional ByRef oListaBaseRef As IList(Of String) = Nothing, _
                                            Optional ByRef oListaIdRepxOrd As List(Of String) = Nothing) As Generic.List(Of String)

        'servicios externos
        'servicios externos
        Dim oBodegas_SE As New Hashtable
        Dim listaBodegas_SE As Generic.List(Of String) = New Generic.List(Of String)
        Dim listaIdRxO As Generic.List(Of String) = New Generic.List(Of String)

        Dim strTipoArticulo As String = ""
        Dim strInventariable As String = ""
        'Dim Contador As Integer = 0
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim xmlDocMatrix As Xml.XmlDocument
        Dim XmlNode As Xml.XmlNode
        Dim matrixXml As String
        Dim Contador As Integer = 0
        Dim ContadorLinea As Integer = 0

        Dim strConsultaBodProcXCC As String = String.Empty
        Dim strBodProceso As String = String.Empty
        Dim strNombreTaller As String = String.Empty
        Dim DocEntrySucursal As String = String.Empty


        '*********************************
        If Not blnUsaConfiguracionTallerInterno Then

            strConsultaBodProcXCC =
          " select Proceso " & _
            " from [dbo].[SCGTA_TB_ConfBodegasXCentroCosto] as ccc with (nolock)" & _
            " inner join [dbo].[SCGTA_VW_OITM] as itm with (nolock) " & _
            " on ccc.IDCentroCosto = itm.[U_SCGD_CodCtroCosto] where itm.ItemCode = '{0}'"


        Else
            DocEntrySucursal = Utilitarios.EjecutarConsulta(String.Format("Select DocEntry From dbo.[@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}'",
                                                                 oForm.DataSources.DBDataSources.Item("OPCH").GetValue("U_SCGD_idSucursal", 0).ToString().Trim()),
                                                             SBO_Application.Company.DatabaseName,
                                                             SBO_Application.Company.ServerName)

            strConsultaBodProcXCC =
          " Select U_Pro " & _
          " from [dbo].[@SCGD_CONF_BODXCC] as ccc with (nolock) " & _
          " inner join [dbo].[OITM] as itm with (nolock) on ccc.U_CC = itm.[U_SCGD_CodCtroCosto] " & _
          " where itm.ItemCode = '{0}' and ccc.DocEntry = '{1}'"


        End If



        '*********************************



        Utilitarios.DevuelveNombreBDTaller(SBO_Application, oForm.DataSources.DBDataSources.Item("OPCH").GetValue("U_SCGD_idSucursal", 0).ToString().Trim(), strNombreTaller)

        dtItemsOITM = CargarDataTableArticulosSE(oForm, dtItemsOITM)

        oForm.DataSources.DataTables.Item("SE").Rows.Clear()

        '******************************
        oMatriz = oForm.Items.Item("38").Specific
        matrixXml = oMatriz.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

        xmlDocMatrix = New Xml.XmlDocument
        xmlDocMatrix.LoadXml(matrixXml)

        Contador = 1
        For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")
            Dim elementoItemCode As Xml.XmlNode
            Dim elementoDescripcion As Xml.XmlNode
            Dim elementoCantidad As Xml.XmlNode
            Dim elementoAlmacen As Xml.XmlNode
            Dim elementoImpuesto As Xml.XmlNode
            Dim elementoLineVat As Xml.XmlNode
            Dim elementoLineVatlF As Xml.XmlNode
            Dim elementoNumeroOT As Xml.XmlNode

            Dim elementoIdRXO As Xml.XmlNode
            Dim elementoCosto As Xml.XmlNode
            Dim elementoSeleccion As Xml.XmlNode

            elementoItemCode = node.SelectSingleNode("Columns/Column/Value[../ID = '1']")
            elementoDescripcion = node.SelectSingleNode("Columns/Column/Value[../ID = '3']")
            elementoCantidad = node.SelectSingleNode("Columns/Column/Value[../ID = '11']")
            elementoAlmacen = node.SelectSingleNode("Columns/Column/Value[../ID = '24']")
            elementoImpuesto = node.SelectSingleNode("Columns/Column/Value[../ID = '160']")
            'elementoLineVat = node.SelectSingleNode("Columns/Column/Value[../ID = '82']")
            'elementoLineVatlF = node.SelectSingleNode("Columns/Column/Value[../ID = '85']")
            elementoNumeroOT = node.SelectSingleNode("Columns/Column/Value[../ID = 'U_SCGD_NoOT']")

            If ListaArticulosSE.Contains(elementoItemCode.InnerText.Trim) Then

                If Not String.IsNullOrEmpty(elementoNumeroOT.InnerText) Then

                    If oForm.DataSources.DBDataSources.Item("PCH1").GetValue("BaseType", Contador - 1).Trim = "20" Then

                        strNumEntrada = oForm.DataSources.DBDataSources.Item("PCH1").GetValue("BaseRef", Contador - 1).Trim()

                        oForm.DataSources.DataTables.Item("SE").Rows.Add(1)

                        oForm.DataSources.DataTables.Item("SE").SetValue("LineId",
                                                                         ContadorLinea,
                                                                         ContadorLinea)
                        oForm.DataSources.DataTables.Item("SE").SetValue("ItemCode",
                                                                        ContadorLinea,
                                                                         oForm.DataSources.DBDataSources.Item("PCH1").GetValue("ItemCode", Contador - 1).Trim())
                        oForm.DataSources.DataTables.Item("SE").SetValue("WhsCode",
                                                                        ContadorLinea,
                                                                         oForm.DataSources.DBDataSources.Item("PCH1").GetValue("WhsCode", Contador - 1).Trim())
                        oForm.DataSources.DataTables.Item("SE").SetValue("ImpCode",
                                                                         ContadorLinea,
                                                                         oForm.DataSources.DBDataSources.Item("PCH1").GetValue("TaxCode", Contador - 1).Trim())
                        oForm.DataSources.DataTables.Item("SE").SetValue("LineVat",
                                                                         ContadorLinea,
                                                                         oForm.DataSources.DBDataSources.Item("PCH1").GetValue("LineVat", Contador - 1).Trim())
                        oForm.DataSources.DataTables.Item("SE").SetValue("LineVatlF",
                                                                        ContadorLinea,
                                                                        oForm.DataSources.DBDataSources.Item("PCH1").GetValue("LineVatlF", Contador - 1).Trim())
                        oForm.DataSources.DataTables.Item("SE").SetValue("CtaDebe",
                                                                        ContadorLinea,
                                                                         "")
                        oForm.DataSources.DataTables.Item("SE").SetValue("CtaDebe2",
                                                                        ContadorLinea,
                                                                         "")
                        oForm.DataSources.DataTables.Item("SE").SetValue("CtaHaber",
                                                                         ContadorLinea,
                                                                         "")
                        oForm.DataSources.DataTables.Item("SE").SetValue("LineTotal",
                                                                        ContadorLinea,
                                                                        oForm.DataSources.DBDataSources.Item("PCH1").GetValue("LineTotal", Contador - 1).Trim())
                        oForm.DataSources.DataTables.Item("SE").SetValue("TotalFrgn",
                                                                         ContadorLinea,
                                                                         oForm.DataSources.DBDataSources.Item("PCH1").GetValue("TotalFrgn", Contador - 1).Trim())

                        oForm.DataSources.DataTables.Item("SE").SetValue("U_SCGD_NoOT",
                                                                       ContadorLinea,
                                                                       oForm.DataSources.DBDataSources.Item("PCH1").GetValue("U_SCGD_NoOT", Contador - 1).Trim())
                        oForm.DataSources.DataTables.Item("SE").SetValue("U_SCGD_IdRepxOrd",
                                                                         ContadorLinea,
                                                                         oForm.DataSources.DBDataSources.Item("PCH1").GetValue("U_SCGD_IdRepxOrd", Contador - 1).Trim())

                        '***********************************************'
                        oForm.DataSources.DataTables.Item("SE").SetValue("Currency",
                                                         ContadorLinea,
                                                         oForm.DataSources.DBDataSources.Item("PCH1").GetValue("Currency", Contador - 1).Trim())
                        '***********************************************'

                        oForm.DataSources.DataTables.Item("SE").SetValue("U_SCGD_ID",
                                                                       ContadorLinea,
                                                                       oForm.DataSources.DBDataSources.Item("PCH1").GetValue("U_SCGD_ID", Contador - 1).Trim())


                        'If Not oListaSE.Contains(oForm.DataSources.DBDataSources.Item("PCH1").GetValue("ItemCode", i).Trim()) Then
                        oListaSE.Add(oForm.DataSources.DBDataSources.Item("PCH1").GetValue("ItemCode", Contador - 1).Trim())
                        oListaBaseRef.Add(strNumEntrada)

                        If Not blnUsaConfiguracionTallerInterno Then
                            oListaIdRepxOrd.Add(oForm.DataSources.DBDataSources.Item("PCH1").GetValue("U_SCGD_IdRepxOrd", Contador - 1).Trim())
                        Else
                            oListaIdRepxOrd.Add(oForm.DataSources.DBDataSources.Item("PCH1").GetValue("U_SCGD_ID", Contador - 1).Trim())
                        End If

                        'agrego el numero de OT por linea
                        If Not oListaNumeroOT.Contains(oForm.DataSources.DBDataSources.Item("PCH1").GetValue("U_SCGD_NoOT", Contador - 1).Trim()) Then
                            oListaNumeroOT.Add(oForm.DataSources.DBDataSources.Item("PCH1").GetValue("U_SCGD_NoOT", Contador - 1).Trim())
                        End If

                        If Not blnUsaConfiguracionTallerInterno Then
                            strBodProceso =
                            Utilitarios.EjecutarConsulta(String.Format(strConsultaBodProcXCC,
                                                                       oForm.DataSources.DBDataSources.Item("PCH1").GetValue("ItemCode", Contador - 1).Trim()),
                                                                   strNombreTaller,
                                                                   SBO_Application.Company.ServerName)
                        Else
                            strBodProceso =
                                Utilitarios.EjecutarConsulta(String.Format(strConsultaBodProcXCC,
                                                                           oForm.DataSources.DBDataSources.Item("PCH1").GetValue("ItemCode", Contador - 1).Trim(), DocEntrySucursal),
                                                                       SBO_Application.Company.DatabaseName,
                                                                       SBO_Application.Company.ServerName)

                        End If


                        listaBodegas_SE.Add(strBodProceso)
                        'oBodegas_SE.Add(ContadorLista, strBodProceso)

                        ContadorLinea = ContadorLinea + 1
                    End If
                End If
            End If
            Contador = Contador + 1
        Next

        Return listaBodegas_SE

    End Function


    ''' <summary>
    ''' Ingresa en el DataTable de Servicios externos Las cuentas ERNF y SEPA
    ''' </summary>
    ''' <param name="oListaSE">Lista de servicios externos</param>
    ''' <param name="htCuentasErnf">HashTable con cuentas ERNF</param>
    ''' <param name="htCuentasSepa">HashTable con cuentas SEPA</param>
    ''' <param name="oForm">Objeto formulario</param>
    ''' <param name="htBodegas_SE">HashTable con servicios externos</param>
    ''' <remarks></remarks>
    Private Sub ObtieneCuentasYBodegas(ByVal oListaSE As IList(Of String),
                               ByRef htCuentasErnf As Hashtable,
                               ByRef htCuentasSepa As Hashtable,
                               ByVal oForm As SAPbouiCOM.Form,
                               ByVal htBodegas_SE As Hashtable,
                               Optional ByVal p_listaBodegasSE As Generic.List(Of String) = Nothing)

        'Entrega recibidos no facturados
        'Servicios externos por asignar 
        Dim strCuentaDebe As String = ""
        Dim strCuentaDebe2 As String = ""
        Dim strCuentaHaber As String = ""

        'Dim oCtas_DebeXBod As New Hashtable
        'Dim oCtas_Debe2XBod As New Hashtable
        'Dim oCtas_HaberXBod As New Hashtable

        Dim oCtas_DebeXBod As New Generic.List(Of String)
        Dim oCtas_Debe2XBod As New Generic.List(Of String)
        Dim oCtas_HaberXBod As New Generic.List(Of String)



        Dim strServicioExterno As String
        Dim strCampoCuentaDebe As String = ""
        Dim strCampoCuentaDebe2 As String = ""
        Dim strCampoCuentaHaber As String = ""

        strCampoCuentaDebe = "TransferAc"
        strCampoCuentaDebe2 = "PriceDifAc"
        strCampoCuentaHaber = "ExpensesAc"
        'Dim x As Integer = 0

        For x As Integer = 0 To oForm.DataSources.DataTables.Item("SE").Rows.Count - 1

            Dim strNombreServicioExterno As String = oListaSE.Item(x)
            Dim strBodega As String = p_listaBodegasSE.Item(x)
            strCuentaDebe = ""
            strCuentaDebe2 = ""
            strCuentaHaber = ""
            strServicioExterno = ""

            strServicioExterno = oForm.DataSources.DataTables.Item("SE").Columns.Item("ItemCode").Cells.Item(x).Value

            If oCtas_DebeXBod.Contains(strCuentaDebe) Then
                Dim position As Integer
                position = oCtas_DebeXBod.IndexOf(strCuentaDebe)
                strCuentaDebe = oCtas_DebeXBod.Item(position)
            End If

            'strCuentaDebe = oCtas_DebeXBod(htBodegas_SE(strServicioExterno))

            If String.IsNullOrEmpty(strCuentaDebe) Then
                strCuentaDebe = Utilitarios.EjecutarConsulta(
                                String.Format("SELECT {0} FROM OWHS	with (nolock) WHERE WhsCode = '{1}'",
                                              strCampoCuentaDebe,
                                              strBodega),
                                          SBO_Company.CompanyDB,
                                          SBO_Company.Server)
                oCtas_DebeXBod.Add(strCuentaDebe)
            Else
                strCuentaDebe = strCuentaDebe
            End If

            oForm.DataSources.DataTables.Item("SE").SetValue("CtaDebe",
                                                             x,
                                                             strCuentaDebe)

            If oCtas_Debe2XBod.Contains(strCuentaDebe2) Then
                Dim position As Integer
                position = oCtas_DebeXBod.IndexOf(strCuentaDebe2)
                strCuentaDebe2 = oCtas_Debe2XBod.Item(position)
            End If

            'strCuentaDebe2 = oCtas_Debe2XBod(htBodegas_SE(strServicioExterno))

            If String.IsNullOrEmpty(strCuentaDebe2) Then
                strCuentaDebe2 = Utilitarios.EjecutarConsulta(
                                String.Format("SELECT {0} FROM OWHS with (nolock) WHERE WhsCode = '{1}'",
                                              strCampoCuentaDebe2,
                                              strBodega),
                                          SBO_Company.CompanyDB,
                                          SBO_Company.Server)
                oCtas_Debe2XBod.Add(strCuentaDebe2)
            Else
                strCuentaDebe2 = strCuentaDebe2
            End If

            oForm.DataSources.DataTables.Item("SE").SetValue("CtaDebe2",
                                                             x,
                                                             strCuentaDebe2)

            If oCtas_HaberXBod.Contains(strCuentaHaber) Then
                Dim position As Integer
                position = oCtas_HaberXBod.IndexOf(strCuentaHaber)
                strCuentaHaber = oCtas_HaberXBod.Item(position)
            End If
            'strCuentaHaber = oCtas_HaberXBod(htBodegas_SE(strServicioExterno))

            If String.IsNullOrEmpty(strCuentaHaber) Then
                strCuentaHaber = Utilitarios.EjecutarConsulta(
                                String.Format("SELECT {0} FROM OWHS	with (nolock) WHERE WhsCode = '{1}'",
                                              strCampoCuentaHaber,
                                             strBodega),
                                          SBO_Company.CompanyDB,
                                          SBO_Company.Server)
                oCtas_HaberXBod.Add(strCuentaHaber)
            Else
                strCuentaHaber = strCuentaHaber
            End If

            oForm.DataSources.DataTables.Item("SE").SetValue("CtaHaber",
                                                             x,
                                                             strCuentaHaber)

            'x = x + 1
        Next

    End Sub

    ''' <summary>
    ''' Retorna el campo deseado tanto del Datatable de Servicios externos como del de Impuestos 
    ''' </summary>
    ''' <param name="oForm">Objeto Formulario</param>
    ''' <param name="Condicion">Condicios para seleccionar el row</param>
    ''' <param name="Campo">Campo a retornar</param>
    ''' <param name="EsServExterno">Si va a obtener valores de la tabla de SE</param>
    ''' <param name="EsCtaImpuesto">Si va a obtener valores de la tabla de IMP</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RetornaCampo(ByVal oForm As Form,
                                          ByVal Condicion As String,
                                          ByVal Campo As String,
                                          ByVal EsServExterno As Boolean,
                                          ByVal EsCtaImpuesto As Boolean, Optional ByVal p_strNoOT As String = "") As Decimal
        Dim strImpuesto As String = ""
        Dim valorAcumulado As Decimal = 0
        Dim valor As String = String.Empty

        Dim strSeparadorDecimalesSAP As String = String.Empty
        Dim strSeparadorMilesSAP As String = String.Empty

        Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, SBO_Company.CompanyDB, SBO_Company.Server)


        If EsServExterno Then
            If oForm.DataSources.DataTables.Item("SE").Rows.Count > 0 Then
                For i As Integer = 0 To oForm.DataSources.DataTables.Item("SE").Rows.Count - 1
                    If oForm.DataSources.DataTables.Item("SE").GetValue("U_SCGD_NoOT", i) = p_strNoOT Then

                        Dim decPrecio As Decimal = Decimal.Parse(oForm.DataSources.DataTables.Item("SE").GetValue(Campo, i), n)
                        valorAcumulado = valorAcumulado + decPrecio

                    End If

                Next

                If valorAcumulado <> 0 Then
                    Return valorAcumulado 'CStr(valorAcumulado).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                Else
                    Return 0
                End If

            End If
        End If


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

            oSBObob = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
            oRecordset = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            oRecordset = oSBObob.GetLocalCurrency()
            strResult = oRecordset.Fields.Item(0).Value

            Return strResult

        Catch ex As Exception
            Return -1
        End Try

    End Function

    ''' <summary>
    ''' Retorna moneda Sistema
    ''' </summary>
    ''' <returns>Retorna moneda Sistema</returns>
    ''' <remarks></remarks>
    Public Function RetornarMonedaSistema() As String
        Dim oSBObob As SAPbobsCOM.SBObob
        Dim sToday As String
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim strResult As String

        Try

            oSBObob = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
            oRecordset = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            oRecordset = oSBObob.GetSystemCurrency()
            strResult = oRecordset.Fields.Item(0).Value

            Return strResult

        Catch ex As Exception
            Return -1
        End Try

    End Function

    ''' <summary>
    ''' Calcula costos por tipo de moneda del sistema, local y la entrada
    ''' </summary>
    ''' <param name="p_ocompany">Objeto compania</param>
    ''' <param name="p_montoDocumento">Monto de la entrada</param>
    ''' <param name="strMonedaEntrada">Moneda de la entrada</param>
    ''' <param name="strFechaEntrada">Fecha de la entrada</param>
    ''' <param name="strTipoCambioEntrada">Tipo de cambios de la entrada</param>
    ''' <returns>Costo de acierdo a la moneda de la entrada y del sistema</returns>
    ''' <remarks></remarks>
    Public Function CalcularCostosPorCambioMoneda(ByVal p_ocompany As SAPbobsCOM.Company, ByVal p_montoDocumento As Decimal, _
                                                  ByVal strMonedaEntrada As String, ByVal strFechaEntrada As String, _
                                                  ByVal strTipoCambioEntrada As String) As Decimal

        Dim m_objBLSBO As New BLSBO.GlobalFunctionsSBO
        Dim n As NumberFormatInfo
        Dim m_strMonedaLocal As String
        Dim m_strMonedaSistema As String
        Dim strTipoCambioSistema As String

        Dim decTipoCambioOrigen As Decimal
        Dim decValorDevuelto As Decimal
        Dim strMonedaBase As String
        Dim dtFecha As Date
        Dim valor As Decimal

        m_objBLSBO.Set_Compania(p_ocompany)

        m_strMonedaLocal = m_objBLSBO.RetornarMonedaLocal()
        m_strMonedaSistema = m_objBLSBO.RetornarMonedaSistema

        dtFecha = Date.ParseExact(strFechaEntrada, "yyyyMMdd", Nothing)

        strTipoCambioSistema = m_objBLSBO.RetornarTipoCambioMonedaRS(m_strMonedaSistema, dtFecha)

        n = DIHelper.GetNumberFormatInfo(p_ocompany)

        If Trim(strMonedaEntrada) <> Trim(m_strMonedaSistema) And
            Trim(strMonedaEntrada) = Trim(m_strMonedaLocal) Then

            Return Decimal.Parse(p_montoDocumento.ToString)

        ElseIf Trim(strMonedaEntrada) = Trim(m_strMonedaSistema) And
            Trim(strMonedaEntrada) <> Trim(m_strMonedaLocal) Then

            p_montoDocumento = p_montoDocumento * strTipoCambioSistema
            valor = Decimal.Parse(p_montoDocumento.ToString)
            Return Decimal.Parse(valor.ToString)

        ElseIf Trim(strMonedaEntrada) <> Trim(m_strMonedaSistema) And
        Trim(strMonedaEntrada) <> Trim(m_strMonedaLocal) Then

            p_montoDocumento = p_montoDocumento * strTipoCambioEntrada
            valor = Decimal.Parse(p_montoDocumento.ToString)
            Return Decimal.Parse(valor.ToString)

        End If

    End Function

    Private Sub ValidarConfiguracionDimensiones(ByVal p_form As SAPbouiCOM.Form)

        'configuraciones para Dimensiones para OTs
        Dim strUsaDimension As String = Utilitarios.EjecutarConsulta("Select U_UsaDimC from dbo.[@SCGD_ADMIN] ", SBO_Company.CompanyDB, SBO_Company.Server)

        If strUsaDimension = "Y" Then

            oDataTableDimensionesContablesDMS = p_form.DataSources.DataTables.Item(mc_strDataTableDimensionesOT)
            blnUsaDimensiones = True

            'hago el llamado para cargar la configuracion de los documentos
            'que usaran Dimensiones
            ClsLineasDocumentosDimension = New AgregarDimensionLineasDocumentosCls(SBO_Company, SBO_Application)
            'ListaConfiguracionOT = New Hashtable
            ListaConfiguracionOT = New List(Of LineasConfiguracionOT)()
            ListaConfiguracionOT = ClsLineasDocumentosDimension.DatatableConfiguracionDocumentosDimensionesOT(p_form)

        End If


    End Sub

    Public Function CargarDataTableArticulosSE(ByVal p_form As SAPbouiCOM.Form, ByRef p_DT As SAPbouiCOM.DataTable) As SAPbouiCOM.DataTable

        Dim strConsulta As String
        ListaArticulosSE = New Generic.List(Of String)

        p_DT = p_form.DataSources.DataTables.Item(mc_strDataTableItems)

        strConsulta = "Select ItemCode from dbo.[OITM] where U_SCGD_TipoArticulo = 4 and InvntItem = 'N'"

        p_DT.ExecuteQuery(strConsulta)

        For i As Integer = 0 To p_DT.Rows.Count - 1

            If Not ListaArticulosSE.Contains(p_DT.GetValue("ItemCode", i)) Then
                ListaArticulosSE.Add(p_DT.GetValue("ItemCode", i))
            End If



        Next

        Return p_DT

    End Function
#End Region

    Public Sub ProcesaDocumentoCompra()
        Try
            Dim strDocEntry As String = String.Empty
            If Not String.IsNullOrEmpty(FormFacPro.DataSources.DBDataSources.Item("OPCH").GetValue("DocEntry", 0)) Then
                strDocEntry = FormFacPro.DataSources.DBDataSources.Item("OPCH").GetValue("DocEntry", 0)
                m_oDocumentoProcesoCompra = New DocumentoProcesoCompra(SBO_Company, SBO_Application)
                Call m_oDocumentoProcesoCompra.ProcesaDocumentoMarketing(strDocEntry, 0)
            End If
        Catch ex As Exception

        End Try
    End Sub

#Region "Manejo de Metodos"

    Public Sub ValidaGeneraAsientoServicioExterno()
        Try
            Dim strCreaAsiento As String = String.Empty
            Dim blnCreaAsiento As Boolean = False

            blnCreaAsiento = False
            strCreaAsiento = Utilitarios.EjecutarConsulta("Select U_GenAsSE from dbo.[@SCGD_ADMIN] with (nolock)", SBO_Company.CompanyDB, SBO_Company.Server)
            If String.IsNullOrEmpty(strCreaAsiento) Then
                blnCreaAsiento = False
            ElseIf strCreaAsiento = "Y" Then
                blnCreaAsiento = True
            End If
            If blnCreaAsiento Then
                GeneraAsientoServicioExterno()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function GeneraAsientoServicioExterno() As Boolean
        Try
            Dim ProvieneEntrada As Boolean = False
            Dim oListaServExterno As New List(Of ListaLineasDocumento)()
            Dim oListaNoOrden As Generic.List(Of String) = New Generic.List(Of String)
            Dim oListaAlmacen As Generic.List(Of String) = New Generic.List(Of String)
            Dim oListaNoOrdenxSucursal As New List(Of ListaNoOrdenxSucursal)()
            Dim oListaCuentasxAlmacen As New List(Of ListaCuentasxAlmacen)()
            Dim oListaProyectosxNoOrden As New List(Of ListaProyectosxNoOrden)()
            Dim strDebitAcc As String = String.Empty
            Dim strCreditAcc As String = String.Empty
            Dim strDebitDiferencialAcc As String = String.Empty
            Dim blnCargarSucursales As Boolean = False
            Dim blnCargarInfoDimensiones As Boolean = False
            Dim blnCargarProyecto As Boolean = False
            'Info dimensiones
            Dim strUsaDimensiones As String = String.Empty
            Dim oListaDimensionesxNoOrden As New List(Of ListaDimensionesxNoOrden)()
            Dim strOTs As String = String.Empty
            Dim query As String = String.Empty
            Dim dtOTs As System.Data.DataTable
            Dim oListLineaAsientoServExterno As New List(Of ListaLineaAsientoServExterno)()
            Dim dateFechaConta As Date = Nothing
            Dim strDocNum As String = String.Empty
            Dim blnUsaDimen As Boolean = False


            If Not String.IsNullOrEmpty(FormFacPro.DataSources.DBDataSources.Item("OPCH").GetValue("DocDate", 0)) AndAlso
                            Not String.IsNullOrEmpty(FormFacPro.DataSources.DBDataSources.Item("OPCH").GetValue("CardCode", 0)) AndAlso
                            FormFacPro.DataSources.DBDataSources.Item("PCH1").Size > 0 Then
                If SBO_Company.InTransaction Then
                    SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                    strNoAsiento = String.Empty
                End If
                If FormFacPro.DataSources.DBDataSources.Item("PCH1").GetValue("BaseType", 0).Trim = "20" Then
                    ProvieneEntrada = True
                Else
                    ProvieneEntrada = False
                End If
                'Si proviene de una entrada de mercancia entonces se genera el asiento por servicios externos, para compensar la parte contable
                If ProvieneEntrada Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoAsientoServExt, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    oListaServExterno = CargaDatosServicioExterno(FormFacPro, oListaNoOrden, blnCargarSucursales, blnCargarInfoDimensiones, blnCargarProyecto, dateFechaConta, strDocNum)
                    If blnCargarSucursales Then
                        CargarSucursalesxOrden(oListaNoOrden, oListaNoOrdenxSucursal)
                        For Each rowNoOrdenxSucursal As ListaNoOrdenxSucursal In oListaNoOrdenxSucursal
                            For Each rowSE As ListaLineasDocumento In oListaServExterno
                                If rowNoOrdenxSucursal.NoOrden = rowSE.NoOrden Then
                                    rowSE.IdSucursal = rowNoOrdenxSucursal.IdSucursal
                                End If
                            Next
                        Next
                    End If
                    If blnCargarProyecto Then
                        CargarProyectosxOrden(oListaNoOrden, oListaProyectosxNoOrden)
                        For Each rowProyectosxNoOrden As ListaProyectosxNoOrden In oListaProyectosxNoOrden
                            For Each rowSE As ListaLineasDocumento In oListaServExterno
                                If rowProyectosxNoOrden.NoOrden = rowSE.NoOrden Then
                                    rowSE.CodProyecto = rowProyectosxNoOrden.CodProyecto
                                End If
                            Next
                        Next
                    End If

                    'Valida si a nivel general si se usan dimensiones, ya que en la parametrizacion de documentos de Venta puede que el documento se le haya 
                    'marcado para no generar dimensiones
                    strUsaDimensiones = Utilitarios.EjecutarConsulta("Select U_UsaDimC from dbo.[@SCGD_ADMIN] with (nolock)", SBO_Company.CompanyDB, SBO_Company.Server)
                    If String.IsNullOrEmpty(strUsaDimensiones) Then
                        blnUsaDimen = False
                    ElseIf strUsaDimensiones = "Y" Then
                        blnUsaDimen = True
                    End If
                    'Si usa dimensiones a nivel general
                    If blnUsaDimen Then
                        'Se completa información de tipo de OT y Cod Marca para las dimensiones contables
                        If blnCargarInfoDimensiones Then
                            For Each rowOT As String In oListaNoOrden
                                If Not strOTs.Contains(rowOT) Then
                                    strOTs = strOTs & String.Format("'{0}', ", rowOT)
                                End If
                            Next
                            If (strOTs.Length > 0) Then
                                strOTs = strOTs.Substring(0, strOTs.Length - 2)
                                query = String.Format("select Q.U_SCGD_Tipo_OT, Q.U_SCGD_Cod_Marca, Q.U_SCGD_Numero_OT from OQUT Q with (nolock) where Q.U_SCGD_Numero_OT in ({0})", strOTs)
                                dtOTs = Utilitarios.EjecutarConsultaDataTable(query, SBO_Company.CompanyDB, SBO_Company.Server)
                            End If
                            For Each rowOT As DataRow In dtOTs.Rows
                                For Each rowServExt As ListaLineasDocumento In oListaServExterno
                                    If rowOT.Item("U_SCGD_Numero_OT").ToString().Trim() = rowServExt.NoOrden Then
                                        If Not String.IsNullOrEmpty(rowOT.Item("U_SCGD_Tipo_OT")) Then
                                            rowServExt.TipoOT = rowOT.Item("U_SCGD_Tipo_OT").ToString.Trim()
                                        End If
                                        If Not String.IsNullOrEmpty(rowOT.Item("U_SCGD_Cod_Marca")) Then
                                            rowServExt.CodMarca = rowOT.Item("U_SCGD_Cod_Marca").ToString.Trim()
                                        End If
                                    End If
                                Next
                            Next
                        End If
                        'Cargar dimensiones contables a la lista de servicios externos
                        ClsLineasDocumentosDimension = New AgregarDimensionLineasDocumentosCls(SBO_Company, SBO_Application)
                        ClsLineasDocumentosDimension.CargarDimensionesOrdenTrabajo(FormFacPro, oListaNoOrden, oListaServExterno)
                    End If

                    For Each row As ListaLineasDocumento In oListaServExterno
                        row.AlmacenProceso = Utilitarios.GetBodegaXCentroCosto(row.CentroCosto, mc_strBodegaProceso, row.IdSucursal, SBO_Application)
                        If Not oListaAlmacen.Contains(row.AlmacenProceso) Then
                            oListaAlmacen.Add(row.AlmacenProceso)
                        End If
                    Next
                    For Each row As String In oListaAlmacen
                        strDebitAcc = String.Empty
                        strCreditAcc = String.Empty
                        strDebitDiferencialAcc = String.Empty

                        ObtenerCuentaAlmacen(row, strDebitAcc, strCreditAcc, strDebitDiferencialAcc, SBO_Company)
                        If Not String.IsNullOrEmpty(strDebitAcc) And Not String.IsNullOrEmpty(strCreditAcc) And Not String.IsNullOrEmpty(strDebitDiferencialAcc) Then
                            oListaCuentasxAlmacen.Add(New ListaCuentasxAlmacen() _
                                                         With {.Almacen = row,
                                                               .DebitAccount = strDebitAcc,
                                                               .CreditAccount = strCreditAcc,
                                                               .DebitDiferencialAccount = strDebitDiferencialAcc})
                        End If
                    Next
                    For Each rowAlmacen As ListaCuentasxAlmacen In oListaCuentasxAlmacen
                        For Each row As ListaLineasDocumento In oListaServExterno
                            If row.AlmacenProceso = rowAlmacen.Almacen Then
                                row.DebitAccount = rowAlmacen.DebitAccount
                                row.CreditAccount = rowAlmacen.CreditAccount
                                row.DebitAccountDiferencial = rowAlmacen.DebitDiferencialAccount
                            End If
                        Next
                    Next
                    'Prepara los costos con respecto a la lista de servicios externos previamente cargada
                    CalculaCostosServiciosExternos(oListaServExterno, oListaCuentasxAlmacen, oListaNoOrden, oListLineaAsientoServExterno)

                    If oListLineaAsientoServExterno.Count > 0 Then
                        SBO_Company.StartTransaction()
                        strNoAsiento = CrearAsientoServicioExterno(oListLineaAsientoServExterno, blnUsaDimen, dateFechaConta, strDocNum)
                    End If
                End If
            Else
                strNoAsiento = String.Empty
            End If
            'verifica que haya creado un asiento para servicios externos 
            If Not String.IsNullOrEmpty(strNoAsiento) _
                And Not strNoAsiento = "0" Then
                'commit en la transaccion 
                SBO_Company.EndTransaction(BoWfTransOpt.wf_Commit)
                strNoAsiento = String.Empty
                SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoGeneradoCorrectamente, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            Else
                If SBO_Company.InTransaction Then
                    SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                    strNoAsiento = String.Empty
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                strNoAsiento = String.Empty
            End If
            SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoNoCreado, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                strNoAsiento = String.Empty
            End If
        End Try
    End Function

    Public Function CargaDatosServicioExterno(ByVal p_oForm As SAPbouiCOM.Form, _
                                              ByRef p_oListaNoOrden As Generic.List(Of String), _
                                              ByRef p_blnCargarSucursales As Boolean, _
                                              ByRef p_blnCargarInfoDimensiones As Boolean, _
                                              ByRef p_blnCargarProyecto As Boolean, _
                                              ByRef p_dateFechaConta As Date, _
                                              ByRef p_strDocNum As String) As List(Of ListaLineasDocumento)
        Dim oFacturaProveedor As SAPbobsCOM.Documents
        Dim oEntradaMercancia As SAPbobsCOM.Documents
        Try
            Dim intDocEntry As Integer = 0
            Dim oListaLineasFacPro As New List(Of ListaLineasDocumento)()
            Dim oListaLineasEntrada As New List(Of ListaLineasDocumento)()
            Dim oListaServiciosExternos As New List(Of ListaLineasDocumento)()
            Dim strItemCode As String = String.Empty
            Dim strCentroCosto As String = String.Empty
            Dim strTipoArticulo As String = String.Empty
            Dim oListaBaseRef As Generic.List(Of String) = New Generic.List(Of String)
            Dim oListaIdRepxOrd As Generic.List(Of String) = New Generic.List(Of String)
            Dim strIdentificadorItemFactura As String = String.Empty
            Dim strIdentificadorItemEntrada As String = String.Empty

            If Not String.IsNullOrEmpty(p_oForm.DataSources.DBDataSources.Item("OPCH").GetValue("DocEntry", 0)) Then
                oFacturaProveedor = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices),  _
                                                                           SAPbobsCOM.Documents)
                oEntradaMercancia = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes),  _
                                                                          SAPbobsCOM.Documents)
                intDocEntry = Convert.ToInt32(p_oForm.DataSources.DBDataSources.Item("OPCH").GetValue("DocEntry", 0))
                If oFacturaProveedor.GetByKey(intDocEntry) Then
                    'ObtenerFecha Contabilización
                    p_dateFechaConta = oFacturaProveedor.DocDate
                    p_strDocNum = oFacturaProveedor.DocNum.ToString.Trim()

                    For cont As Integer = 0 To oFacturaProveedor.Lines.Count - 1
                        oFacturaProveedor.Lines.SetCurrentLine(cont)
                        If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                            strItemCode = oFacturaProveedor.Lines.ItemCode.ToString.Trim()

                            strTipoArticulo = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString.Trim()
                            strCentroCosto = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString.Trim()

                            If String.IsNullOrEmpty(strTipoArticulo) Or String.IsNullOrEmpty(strCentroCosto) Then
                                ObtenerDatosItem(strItemCode, strCentroCosto, strTipoArticulo, SBO_Company)
                            End If

                            If strTipoArticulo = "4" Then

                                If Not String.IsNullOrEmpty(strItemCode) And Not String.IsNullOrEmpty(strCentroCosto) And Not String.IsNullOrEmpty(strTipoArticulo) Then
                                    oListaLineasFacPro.Add(New ListaLineasDocumento() _
                                                           With {.ItemCode = strItemCode,
                                                                 .TipoArticulo = strTipoArticulo,
                                                                 .CentroCosto = strCentroCosto,
                                                                 .LineTotalFactura = oFacturaProveedor.Lines.LineTotal,
                                                                 .BaseEntry = oFacturaProveedor.Lines.BaseEntry,
                                                                 .NoOrden = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value.ToString.Trim(),
                                                                 .IdRepxOrd = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString.Trim(),
                                                                 .ID = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim(),
                                                                 .IdSucursal = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString.Trim(),
                                                                 .TipoOT = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value.ToString.Trim(),
                                                                 .CodProyecto = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_CodProy").Value.ToString.Trim()})

                                    If Not oListaBaseRef.Contains(oFacturaProveedor.Lines.BaseEntry) Then
                                        oListaBaseRef.Add(oFacturaProveedor.Lines.BaseEntry)
                                    End If
                                End If
                            End If
                        End If
                    Next
                    'Recorre la lista de entradas para cargar cada una de ellas
                    For Each rowBaseRef As String In oListaBaseRef
                        oEntradaMercancia.GetByKey(CInt(rowBaseRef))
                        For rowEntrada As Integer = 0 To oEntradaMercancia.Lines.Count - 1
                            oEntradaMercancia.Lines.SetCurrentLine(rowEntrada)
                            oListaLineasEntrada.Add(New ListaLineasDocumento() _
                                                         With {.ItemCode = oEntradaMercancia.Lines.ItemCode.ToString.Trim(),
                                                               .LineTotalEntrada = oEntradaMercancia.Lines.LineTotal,
                                                               .NoOrden = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value.ToString.Trim(),
                                                               .IdRepxOrd = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString.Trim(),
                                                               .ID = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim(),
                                                               .IdSucursal = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString.Trim(),
                                                               .TipoOT = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value.ToString.Trim()})
                        Next
                    Next
                    oListaServiciosExternos.Clear()

                    For Each rowLineaFactura As ListaLineasDocumento In oListaLineasFacPro
                        For Each rowLineaEntrada As ListaLineasDocumento In oListaLineasEntrada
                            strIdentificadorItemFactura = String.Empty
                            strIdentificadorItemEntrada = String.Empty
                            If Not String.IsNullOrEmpty(rowLineaFactura.ID) And Not String.IsNullOrEmpty(rowLineaEntrada.ID) Then
                                strIdentificadorItemFactura = rowLineaFactura.ID
                                strIdentificadorItemEntrada = rowLineaEntrada.ID
                            Else
                                strIdentificadorItemFactura = rowLineaFactura.IdRepxOrd
                                strIdentificadorItemEntrada = rowLineaEntrada.IdRepxOrd
                            End If
                            If rowLineaFactura.ItemCode = rowLineaEntrada.ItemCode And rowLineaFactura.NoOrden = rowLineaEntrada.NoOrden And strIdentificadorItemFactura = strIdentificadorItemEntrada Then
                                oListaServiciosExternos.Add(New ListaLineasDocumento() _
                                                         With {.ItemCode = oEntradaMercancia.Lines.ItemCode.ToString.Trim(),
                                                               .LineTotalFactura = rowLineaFactura.LineTotalFactura,
                                                               .LineTotalEntrada = rowLineaEntrada.LineTotalEntrada,
                                                               .NoOrden = rowLineaFactura.NoOrden,
                                                               .ID = strIdentificadorItemFactura,
                                                               .CentroCosto = rowLineaFactura.CentroCosto,
                                                               .TipoArticulo = rowLineaFactura.TipoArticulo,
                                                               .IdSucursal = rowLineaFactura.IdSucursal,
                                                               .TipoOT = rowLineaFactura.TipoOT,
                                                               .AplicadoCargaDimensiones = False})
                                Exit For
                            End If
                        Next
                        If Not p_oListaNoOrden.Contains(rowLineaFactura.NoOrden) Then
                            p_oListaNoOrden.Add(rowLineaFactura.NoOrden)
                        End If
                        If String.IsNullOrEmpty(rowLineaFactura.IdSucursal) Then
                            p_blnCargarSucursales = True
                        End If
                        If String.IsNullOrEmpty(rowLineaFactura.TipoOT) Or String.IsNullOrEmpty(rowLineaFactura.CodMarca) Then
                            p_blnCargarInfoDimensiones = True
                        End If
                        If String.IsNullOrEmpty(rowLineaFactura.CodProyecto) Then
                            p_blnCargarProyecto = True
                        End If
                    Next
                    Return oListaServiciosExternos
                End If
            End If
        Catch ex As Exception
        Finally
            If Not oFacturaProveedor Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oFacturaProveedor)
                oFacturaProveedor = Nothing
            End If
            If Not oEntradaMercancia Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oEntradaMercancia)
                oEntradaMercancia = Nothing
            End If
        End Try
    End Function

    Public Sub CalculaCostosServiciosExternos(ByRef p_oListaServiciosExternos As List(Of ListaLineasDocumento), _
                                              ByVal p_oListaCuentasxAlmacen As List(Of ListaCuentasxAlmacen), _
                                              ByVal p_oListaNoOrden As Generic.List(Of String), _
                                              ByRef p_oListaLineaAsientoServExterno As List(Of ListaLineaAsientoServExterno))
        Dim decMontoCosto As Decimal = 0
        Dim decMontoDebitDiferencial As Decimal = 0
        Dim decMontoCreditDiferencial As Decimal = 0
        Dim decValorDiferencial As Decimal = 0
        Dim decMontoFactTemp As Decimal = 0
        Dim decMontoEntradaTemp As Decimal = 0
        Dim strCodProyecto As String = String.Empty
        Dim blnAplicaDimensiones As Boolean = False
        Dim strCostingCode As String = String.Empty
        Dim strCostingCode2 As String = String.Empty
        Dim strCostingCode3 As String = String.Empty
        Dim strCostingCode4 As String = String.Empty
        Dim strCostingCode5 As String = String.Empty
        Dim blnCargaDimensionesPrevia As Boolean = False
        Try
            For Each rowOT As String In p_oListaNoOrden
                blnAplicaDimensiones = False
                blnCargaDimensionesPrevia = False
                strCodProyecto = String.Empty
                For Each rowAccount As ListaCuentasxAlmacen In p_oListaCuentasxAlmacen
                    decMontoFactTemp = 0
                    decMontoEntradaTemp = 0
                    For Each rowServExterno As ListaLineasDocumento In p_oListaServiciosExternos
                        If rowOT = rowServExterno.NoOrden And rowAccount.Almacen = rowServExterno.AlmacenProceso And rowServExterno.AplicaCosto = False Then
                            decMontoFactTemp += rowServExterno.LineTotalFactura
                            decMontoEntradaTemp += rowServExterno.LineTotalEntrada
                            If rowServExterno.AplicadoCargaDimensiones = True And blnCargaDimensionesPrevia = False Then
                                If Not String.IsNullOrEmpty(rowServExterno.CostingCode) Then
                                    strCostingCode = rowServExterno.CostingCode
                                End If
                                If Not String.IsNullOrEmpty(rowServExterno.CostingCode2) Then
                                    strCostingCode2 = rowServExterno.CostingCode2
                                End If
                                If Not String.IsNullOrEmpty(rowServExterno.CostingCode3) Then
                                    strCostingCode3 = rowServExterno.CostingCode3
                                End If
                                If Not String.IsNullOrEmpty(rowServExterno.CostingCode4) Then
                                    strCostingCode4 = rowServExterno.CostingCode4
                                End If
                                If Not String.IsNullOrEmpty(rowServExterno.CostingCode5) Then
                                    strCostingCode5 = rowServExterno.CostingCode5
                                End If
                                blnAplicaDimensiones = True
                                blnCargaDimensionesPrevia = True
                            End If
                            strCodProyecto = rowServExterno.CodProyecto
                            rowServExterno.AplicaCosto = True
                        End If
                    Next

                    decValorDiferencial = (decMontoFactTemp - decMontoEntradaTemp)
                    If decValorDiferencial < 0 Then
                        decMontoDebitDiferencial = decValorDiferencial * (-1)
                        decMontoCreditDiferencial = 0
                    ElseIf decValorDiferencial > 0 Then
                        decMontoCreditDiferencial = decValorDiferencial
                        decMontoDebitDiferencial = 0
                    End If

                    If decMontoFactTemp > 0 And decMontoEntradaTemp > 0 Then
                        p_oListaLineaAsientoServExterno.Add(New ListaLineaAsientoServExterno() _
                                                                             With {.NoOrden = rowOT,
                                                                                   .AccountDebit = rowAccount.DebitAccount,
                                                                                   .AccountCredit = rowAccount.CreditAccount,
                                                                                   .AccountDebitDiferencial = rowAccount.DebitDiferencialAccount,
                                                                                   .Debit = decMontoFactTemp,
                                                                                   .Credit = decMontoEntradaTemp,
                                                                                   .DebitDiferencial = decMontoDebitDiferencial,
                                                                                   .CreditDiferencial = decMontoCreditDiferencial,
                                                                                   .CostingCode = strCostingCode,
                                                                                   .CostingCode2 = strCostingCode2,
                                                                                   .CostingCode3 = strCostingCode3,
                                                                                   .CostingCode4 = strCostingCode4,
                                                                                   .CostingCode5 = strCostingCode5,
                                                                                   .CodProyecto = strCodProyecto,
                                                                                   .AplicadoCargaDimensiones = blnAplicaDimensiones})
                    End If
                Next
            Next
        Catch ex As Exception

        End Try
    End Sub
    Public Shared Sub ObtenerCuentaAlmacen(ByVal strAlmacen As String, ByRef p_strDebitAccount As String, ByRef p_strCreditAccount As String, ByRef p_strDiferencialAccount As String, ByVal p_oCompany As SAPbobsCOM.Company)

        Dim strDebitAccount As String = String.Empty
        Dim strCreeditAccount As String = String.Empty
        Dim strDiferencialAccount As String = String.Empty
        Try
            p_strDebitAccount = Utilitarios.EjecutarConsulta(String.Format("Select {1} FROM OWHS with (nolock) Where WhsCode = '{0}'",
                                                        strAlmacen, "TransferAc"), p_oCompany.CompanyDB, p_oCompany.Server)
            p_strCreditAccount = Utilitarios.EjecutarConsulta(String.Format("Select {1} FROM OWHS with (nolock) Where WhsCode = '{0}'",
                                                       strAlmacen, "ExpensesAc"), p_oCompany.CompanyDB, p_oCompany.Server)
            p_strDiferencialAccount = Utilitarios.EjecutarConsulta(String.Format("Select {1} FROM OWHS with (nolock) Where WhsCode = '{0}'",
                                                       strAlmacen, "PriceDifAc"), p_oCompany.CompanyDB, p_oCompany.Server)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub CargarSucursalesxOrden(ByVal p_oListaNoOrden As Generic.List(Of String), ByRef p_oListaNoOrdenxSucursal As List(Of ListaNoOrdenxSucursal))
        Dim strIdSucursal As String = String.Empty
        Try
            For Each NoOrden As String In p_oListaNoOrden
                strIdSucursal = Utilitarios.EjecutarConsulta(String.Format("select U_SCGD_idSucursal from OQUT with (nolock) where U_SCGD_Numero_OT= '{0}'",
                                                                          NoOrden),
                                                                            SBO_Application.Company.DatabaseName,
                                                                            SBO_Application.Company.ServerName)
                If Not String.IsNullOrEmpty(NoOrden) And Not String.IsNullOrEmpty(strIdSucursal) Then
                    p_oListaNoOrdenxSucursal.Add(New ListaNoOrdenxSucursal() _
                                                With {.NoOrden = NoOrden,
                                                      .IdSucursal = strIdSucursal})
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub
    Public Sub CargarProyectosxOrden(ByVal p_oListaNoOrden As Generic.List(Of String), ByRef p_oListaProyectosxNoOrden As List(Of ListaProyectosxNoOrden))
        Dim strCodProyecto As String = String.Empty
        Try
            For Each NoOrden As String In p_oListaNoOrden
                strCodProyecto = Utilitarios.EjecutarConsulta(String.Format("select U_SCGD_Proyec from OQUT with (nolock) where U_SCGD_Numero_OT= '{0}'",
                                                                          NoOrden),
                                                                            SBO_Application.Company.DatabaseName,
                                                                            SBO_Application.Company.ServerName)
                If Not String.IsNullOrEmpty(NoOrden) And Not String.IsNullOrEmpty(strCodProyecto) Then
                    p_oListaProyectosxNoOrden.Add(New ListaProyectosxNoOrden() _
                                                With {.NoOrden = NoOrden,
                                                      .CodProyecto = strCodProyecto})
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub

    Public Function CrearAsientoServicioExterno(ByVal p_oListaAsientoServicioExterno As List(Of ListaLineaAsientoServExterno), _
                                                ByVal p_blnUsaDimensiones As Boolean, _
                                                ByVal p_dateFechaConta As Date, _
                                                ByVal p_strDocNum As String) As String
        Try

            Dim oJE_Lines As SAPbobsCOM.JournalEntries_Lines
            Dim oJournalEntry As SAPbobsCOM.JournalEntries
            Dim strAsiento As String = String.Empty
            Dim strAsientoGenerado As String = "0"
            Dim intError As Integer
            Dim strMensajeError As String = ""
            Dim formato As String
            Dim dateFechaRegistro As Date = Nothing

            strAsientoGenerado = "0"

            oJournalEntry = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            oJournalEntry.Memo = My.Resources.Resource.AsientoFacturaProveedores + p_strDocNum
            If p_dateFechaConta <> Nothing Then
                oJournalEntry.ReferenceDate = p_dateFechaConta
            End If

            For Each row As ListaLineaAsientoServExterno In p_oListaAsientoServicioExterno
                '*********************
                'Cuenta Credito
                '*********************
                oJournalEntry.Lines.AccountCode = row.AccountCredit
                oJournalEntry.Lines.Credit = row.Credit
                oJournalEntry.Lines.FCCredit = 0

                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.Reference1 = row.NoOrden
                If Not String.IsNullOrEmpty(row.CodProyecto) Then
                    oJournalEntry.Lines.ProjectCode = row.CodProyecto
                End If
                If p_blnUsaDimensiones Then
                    If row.AplicadoCargaDimensiones = True Then
                        If Not String.IsNullOrEmpty(row.CostingCode) Then
                            oJournalEntry.Lines.CostingCode = row.CostingCode
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode2) Then
                            oJournalEntry.Lines.CostingCode2 = row.CostingCode2
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode3) Then
                            oJournalEntry.Lines.CostingCode3 = row.CostingCode3
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode4) Then
                            oJournalEntry.Lines.CostingCode4 = row.CostingCode4
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode5) Then
                            oJournalEntry.Lines.CostingCode5 = row.CostingCode5
                        End If
                    End If
                End If
                oJournalEntry.Lines.Add()
                '*****************
                'Cuenta Debito
                '*****************
                oJournalEntry.Lines.AccountCode = row.AccountDebit
                oJournalEntry.Lines.Debit = row.Debit
                oJournalEntry.Lines.FCDebit = 0

                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.Reference1 = row.NoOrden
                If Not String.IsNullOrEmpty(row.CodProyecto) Then
                    oJournalEntry.Lines.ProjectCode = row.CodProyecto
                End If
                If p_blnUsaDimensiones Then
                    If row.AplicadoCargaDimensiones = True Then
                        If Not String.IsNullOrEmpty(row.CostingCode) Then
                            oJournalEntry.Lines.CostingCode = row.CostingCode
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode2) Then
                            oJournalEntry.Lines.CostingCode2 = row.CostingCode2
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode3) Then
                            oJournalEntry.Lines.CostingCode3 = row.CostingCode3
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode4) Then
                            oJournalEntry.Lines.CostingCode4 = row.CostingCode4
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode5) Then
                            oJournalEntry.Lines.CostingCode5 = row.CostingCode5
                        End If
                    End If
                End If
                oJournalEntry.Lines.Add()
                '*****************
                'Cuenta Debito Diferencia
                '*****************
                If row.DebitDiferencial > 0 Then
                    oJournalEntry.Lines.AccountCode = row.AccountDebitDiferencial
                    oJournalEntry.Lines.Debit = row.DebitDiferencial
                    oJournalEntry.Lines.FCDebit = 0

                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    oJournalEntry.Lines.Reference1 = row.NoOrden
                    If Not String.IsNullOrEmpty(row.CodProyecto) Then
                        oJournalEntry.Lines.ProjectCode = row.CodProyecto
                    End If
                    If p_blnUsaDimensiones Then
                        If row.AplicadoCargaDimensiones = True Then
                            If Not String.IsNullOrEmpty(row.CostingCode) Then
                                oJournalEntry.Lines.CostingCode = row.CostingCode
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode2) Then
                                oJournalEntry.Lines.CostingCode2 = row.CostingCode2
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode3) Then
                                oJournalEntry.Lines.CostingCode3 = row.CostingCode3
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode4) Then
                                oJournalEntry.Lines.CostingCode4 = row.CostingCode4
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode5) Then
                                oJournalEntry.Lines.CostingCode5 = row.CostingCode5
                            End If
                        End If
                    End If
                    oJournalEntry.Lines.Add()
                ElseIf row.CreditDiferencial > 0 Then
                    oJournalEntry.Lines.AccountCode = row.AccountDebitDiferencial
                    oJournalEntry.Lines.Credit = row.CreditDiferencial
                    oJournalEntry.Lines.FCCredit = 0

                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    oJournalEntry.Lines.Reference1 = row.NoOrden
                    If Not String.IsNullOrEmpty(row.CodProyecto) Then
                        oJournalEntry.Lines.ProjectCode = row.CodProyecto
                    End If
                    If p_blnUsaDimensiones Then
                        If row.AplicadoCargaDimensiones = True Then
                            If Not String.IsNullOrEmpty(row.CostingCode) Then
                                oJournalEntry.Lines.CostingCode = row.CostingCode
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode2) Then
                                oJournalEntry.Lines.CostingCode2 = row.CostingCode2
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode3) Then
                                oJournalEntry.Lines.CostingCode3 = row.CostingCode3
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode4) Then
                                oJournalEntry.Lines.CostingCode4 = row.CostingCode4
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode5) Then
                                oJournalEntry.Lines.CostingCode5 = row.CostingCode5
                            End If
                        End If
                    End If
                    oJournalEntry.Lines.Add()
                End If
            Next
            If oJournalEntry.Add <> 0 Then
                strAsientoGenerado = "0"
                SBO_Company.GetLastError(intError, strMensajeError)
                Utilitarios.DestruirObjeto(oJournalEntry)
                SBO_Application.StatusBar.SetText(strMensajeError, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Throw New ExceptionsSBO(intError, strMensajeError)
            Else
                SBO_Company.GetNewObjectCode(strAsientoGenerado)
            End If
            Utilitarios.DestruirObjeto(oJournalEntry)
            Return strAsientoGenerado
        Catch ex As Exception
        End Try
    End Function


    Public Shared Sub ObtenerDatosItem(ByVal p_itemCode As String, ByRef p_strCentroCosto As String, ByRef p_strTipoArticulo As String, ByVal p_oCompany As SAPbobsCOM.Company) ' As Generic.List(Of String)
        Dim oItemArticulo As SAPbobsCOM.IItems
        Dim cuentaContable As String = String.Empty
        Try
            oItemArticulo = p_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            oItemArticulo.GetByKey(p_itemCode)

            If Not oItemArticulo Is Nothing Then
                p_strCentroCosto = oItemArticulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value
                p_strTipoArticulo = oItemArticulo.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value
            End If
        Catch ex As Exception
        Finally
            If Not oItemArticulo Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oItemArticulo)
                oItemArticulo = Nothing
            End If
        End Try
    End Sub
#End Region



#Region "Nuevos metodos"
    Public Sub ManejaFacturaProveedor()
        Dim strDocEntry As String = String.Empty
        Dim oListaCotizacion As List(Of SAPbobsCOM.Documents)
        Dim oJournalEntry As SAPbobsCOM.JournalEntries
        Dim oGeneralDataList As List(Of SAPbobsCOM.GeneralData)
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim intError As Integer = 0
        Dim strMensajeError As String = String.Empty
        Dim blnProcesar As Boolean = False
        Try
            strDocEntry = FormFacPro.DataSources.DBDataSources.Item("OPCH").GetValue("DocEntry", 0)
            InicializarTimer()
            If Not String.IsNullOrEmpty(strDocEntry) Then
                oListaCotizacion = New List(Of SAPbobsCOM.Documents)
                oGeneralDataList = New List(Of SAPbobsCOM.GeneralData)
                oCompanyService = SBO_Company.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_OT")
                If ProcesaCantidadesyCostosCotizacion(strDocEntry, oListaCotizacion) Then blnProcesar = True
                If ProcesaFacturaProveedor(strDocEntry, oJournalEntry, oGeneralDataList) Then blnProcesar = True

                If blnProcesar Then
                    ResetTransaction()
                    StartTransaction()
                    '****************Actualiza Cotización - Cantidades y Costos ********************
                    If Not oListaCotizacion Is Nothing Then
                        For Each rowCotizacion As SAPbobsCOM.Documents In oListaCotizacion
                            If rowCotizacion.Update() <> 0 Then
                                SBO_Company.GetLastError(intError, strMensajeError)
                                Throw New ExceptionsSBO(intError, strMensajeError)
                            End If
                        Next
                    End If
                    '****************Asiento Servicio Externo********************
                    If Not oJournalEntry Is Nothing Then
                        If oJournalEntry.Add <> 0 Then
                            SBO_Company.GetLastError(intError, strMensajeError)
                            Throw New ExceptionsSBO(intError, strMensajeError)
                        End If
                    End If
                   
                    '****************Tracking OT********************

                    If Not oGeneralDataList Is Nothing Then
                        For Each rowoGeneralData As SAPbobsCOM.GeneralData In oGeneralDataList
                            oGeneralService.Update(rowoGeneralData)
                        Next
                    End If
                    '****************Actualizar entrada mercancia********************
                    If Not ActualizarFacturaProveedores(strDocEntry) Then
                        RollbackTransaction()
                    End If

                    CommitTransaction()
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizadoConExito, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                Else
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorTransaccion, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            RollbackTransaction()
        Finally
            DetenerTimer()
        End Try
    End Sub

    ''' <summary>
    ''' Crea una nueva instancia del Timer para evitar que el add-on se caiga en procesos muy largos
    ''' cada cierto tiempo se limpia la cola de mensajes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InicializarTimer()
        Try
            'Inicializa un timer que se ejecuta cada 30 segundos
            'y llama al método LimpiarColaMensajes
            oTimer = New System.Timers.Timer(10000)
            RemoveHandler oTimer.Elapsed, AddressOf LimpiarColaMensajes
            AddHandler oTimer.Elapsed, AddressOf LimpiarColaMensajes
            oTimer.AutoReset = True
            oTimer.Enabled = True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Tiene el Timer que es utilizado para limpiar la cola de mensajes y evitar que el add-on se caiga
    ''' en procesos muy largos (Muchos minutos)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DetenerTimer()
        Try
            oTimer.Stop()
            oTimer.Dispose()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Limpia la cola de mensajes para evitar que el Add-On se caiga en procesos muy extensos
    ''' evitando que se generen errores del tipo RPC Server Call y otros similares
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LimpiarColaMensajes()
        Try
            'En las operaciones muy largas, la cola de mensajes se llena ocasionando que el add-on se desconecte y genere errores como
            'RPC Server call o similares. Para solucionarlo se debe ejecutar este método cada cierto tiempo (30 o 60 segundos) para limpiar
            'la cola de mensajes
            DMS_Connector.Company.ApplicationSBO.RemoveWindowsMessage(SAPbouiCOM.BoWindowsMessageType.bo_WM_TIMER, True)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo encargado de volver a procesar la factura para actualizar cantidades, costos y generar asiento de servicios externos
    ''' en caso de que por algún motivo, se haya presentado un error ocasionando que no se actualicen los datos.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ReprocesarFactura()
        Dim strProcesaAsientoSE As String = String.Empty
        Dim strDocEntry As String = String.Empty
        Try 'FormFacPro = oForm
            strDocEntry = FormFacPro.DataSources.DBDataSources.Item("OPCH").GetValue("DocEntry", 0)
            strProcesaAsientoSE = FormFacPro.DataSources.DBDataSources.Item("OPCH").GetValue("U_SCGD_ProASEF", 0)

            If Not String.IsNullOrEmpty(strProcesaAsientoSE) AndAlso strProcesaAsientoSE.ToUpper().Equals("Y") Then
                'If Not ExisteAsientoSE(strDocEntry) Then
                '   ManejaFacturaProveedor()
                'End If
                ManejaFacturaProveedor()
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Valida si ya existe un asiento por servicios externos en relacionado a la factura de compras
    ''' </summary>
    ''' <param name="DocEntry">DocEntry de la factura de compras</param>
    ''' <returns>False = No existe el asiento por servicios externos. True = Si existe el asiento por servicios externos.</returns>
    ''' <remarks></remarks>
    Public Function ExisteAsientoSE(ByVal DocEntry As String) As Boolean
        Dim strQuery As String = "SELECT COUNT(*) FROM OJDT T0 WHERE T0.""Ref1"" = '{0}' AND T0.""Memo"" = '{1}'"
        Dim strDocNum As String = String.Empty
        Dim strNoOT As String = String.Empty
        Dim strTextoComentario As String = String.Empty
        Dim strCuenta As String = String.Empty
        Dim blnResultado As Boolean = False
        Try
            strNoOT = FormFacPro.DataSources.DBDataSources.Item("OPCH").GetValue("U_SCGD_Numero_OT", 0).Trim()

            If Not String.IsNullOrEmpty(strNoOT) Then
                strDocNum = FormFacPro.DataSources.DBDataSources.Item("OPCH").GetValue("DocNum", 0)

                strTextoComentario = String.Format("{0}{1}", My.Resources.Resource.AsientoFacturaProveedores, strDocNum)
                strQuery = String.Format(strQuery, strNoOT, strTextoComentario)
                strCuenta = DMS_Connector.Helpers.EjecutarConsulta(strQuery)

                If Not String.IsNullOrEmpty(strCuenta) Then
                    If strCuenta.Equals("0") Then
                        blnResultado = False
                    Else
                        blnResultado = True
                    End If
                End If
            End If

            Return blnResultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function


    Public Function ProcesaFacturaProveedor(ByVal p_strDocEntry As String, ByRef p_oJournalEntry As SAPbobsCOM.JournalEntries, ByRef p_oGeneralDataList As List(Of SAPbobsCOM.GeneralData)) As Boolean
        Try
            '**********DataContract****************
            Dim oLineaFacturaProveedorList As DocumentoMarketing_List = New DocumentoMarketing_List
            Dim oTipoOTList As ConfiguracionOrdenTrabajo_List = New ConfiguracionOrdenTrabajo_List
            Dim oDatosGeneralesList As DatoGenerico_List = New DatoGenerico_List
            Dim oConfiguracionGeneralList As ConfiguracionGeneral_List = New ConfiguracionGeneral_List
            '********Listas genericas*************
            Dim oSucursalList As List(Of String) = New Generic.List(Of String)
            Dim oNoOrdenList As List(Of String) = New Generic.List(Of String)
            Dim oCodigoMarcaList As List(Of String) = New Generic.List(Of String)
            Dim oBaseEntryList As List(Of Integer) = New Generic.List(Of Integer)
            Dim oBodegaCentroCostoList As BodegaCentroCosto_List = New BodegaCentroCosto_List()
            '**********Declaración Variables*****************
            Dim blnProcesaFacturaProveedor As Boolean = False
            '*************Clases**************************
            Dim clsDocumentoProcesoCompra As DocumentoProcesoCompra = New DocumentoProcesoCompra(SBO_Company, SBO_Application)
            '********Carga información lineas de entrada mercancia*************
            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                CargaConfiguracionGeneral(oConfiguracionGeneralList)
                blnProcesaFacturaProveedor = CargaFacturaProveedor(CInt(p_strDocEntry), oLineaFacturaProveedorList, oSucursalList, oNoOrdenList, oCodigoMarcaList, oTipoOTList, oDatosGeneralesList, oBaseEntryList)
            End If
            '********Valida si existen lineas en la entrada de mercancia que sean de tipo(Servicio Externo) que esten ligadas a una OT y que necesite procesar para saber si genera asiento*************
            If blnProcesaFacturaProveedor Then
                '**********************************************
                '*********** Actualiza Valores Cotizacion******
                '**********************************************
                'ActualizaValoresCotizacion(oNoOrdenList, oLineaFacturaProveedorList, oConfiguracionGeneralList)
                '**********************************************
                '*********** Genera Asiento Servicio Externo******
                '**********************************************
                If Not ManejarAsientoServicioExterno(oLineaFacturaProveedorList, oConfiguracionGeneralList, oSucursalList, oCodigoMarcaList, oTipoOTList, oDatosGeneralesList, oBaseEntryList, p_oJournalEntry) Then Return False
                '**********************************************
                '*********** Maneja Tracking******
                '**********************************************
                If oConfiguracionGeneralList.Item(0).UsaOTInterna Then
                    If Not clsDocumentoProcesoCompra.ManejarTrackingOT(oNoOrdenList, oLineaFacturaProveedorList, oDatosGeneralesList, TipoDocumentoMarketing.FacturaProveedor, p_oGeneralDataList, False) Then Return False
                End If
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function


    Public Function ManejarAsientoServicioExterno(ByRef p_oLineaFacturaProveedorList As DocumentoMarketing_List, _
                                             ByRef p_oConfiguracionGeneralList As ConfiguracionGeneral_List, _
                                             ByRef p_oSucursalList As List(Of String), _
                                             ByRef p_oCodigoMarcaList As List(Of String), _
                                             ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                             ByRef p_oDatosGeneralesList As DatoGenerico_List, _
                                             ByRef p_oBaseEntryList As List(Of Integer), _
                                             ByRef p_oJournalEntry As SAPbobsCOM.JournalEntries) As Boolean
        Try
            '**************Data Contract****************
            Dim oConfiguracionSucursalList As ConfiguracionSucursal_List = New ConfiguracionSucursal_List
            Dim oServicioExternoList As DocumentoMarketing_List = New DocumentoMarketing_List
            Dim oDimensionesContablesList As DimensionesContables_List = New DimensionesContables_List
            Dim oAsientoServicioExternoList As Asiento_List = New Asiento_List
            Dim oBodegaCentroCostoList As BodegaCentroCosto_List = New BodegaCentroCosto_List
            '**************Variables ********************
            Dim blnDimensionesYaCargadas As Boolean = False
            Dim blnAsientoServicioExternoExitoso As Boolean = False
            '*************Clases**************************
            Dim ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls = New AgregarDimensionLineasDocumentosCls(SBO_Company, SBO_Application)
            If p_oConfiguracionGeneralList.Item(0).UsaAsientoServicioExterno Then
                If p_oLineaFacturaProveedorList.Item(0).BaseDocType = TipoDocumentoMarketingBase.EntradaMercancia Then
                    '********Obtiene costos de las entradas*************
                    ObtieneCostoEntradaMercancia(p_oLineaFacturaProveedorList, p_oBaseEntryList)
                    '********Carga configuración sucursal*************
                    If p_oSucursalList.Count > 0 Then
                        CargaConfiguracionSucursal(p_oSucursalList, oConfiguracionSucursalList, oBodegaCentroCostoList)
                    End If
                    '********Si a nivel de compañia se usan dimensiones, valida si lo hace a nivel de Tipo OT*************
                    For Each rowConfiguracionSucursal As ConfiguracionSucursal In oConfiguracionSucursalList
                        If rowConfiguracionSucursal.UsaAsientoServicioExterno Then
                            If rowConfiguracionSucursal.UsaDimensiones Then
                                If p_oTipoOTList.Count > 0 Then
                                    ValidaUsaDimensionesTipoOT(p_oTipoOTList)
                                End If
                                If Not blnDimensionesYaCargadas Then
                                    ClsLineasDocumentosDimension.CargaCentrosCostoDimensionesOT(p_oSucursalList, p_oCodigoMarcaList, oDimensionesContablesList)
                                    blnDimensionesYaCargadas = True
                                End If
                            End If
                            CargaListasTipoArticulo(p_oLineaFacturaProveedorList, oServicioExternoList, p_oTipoOTList, rowConfiguracionSucursal, oDimensionesContablesList, oBodegaCentroCostoList)
                        End If
                    Next
                    If DMS_Connector.Configuracion.ParamGenAddon.U_GenAsSE = "Y" Then ProcesaAsientoServicioExterno(oServicioExternoList, oAsientoServicioExternoList)
                    '************Verifica si genera asiento para servicio externo****************
                    If oAsientoServicioExternoList.Count > 0 Then
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoAsientoServExt, SAPbouiCOM.BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning)
                        If Not CrearAsiento(p_oDatosGeneralesList, oAsientoServicioExternoList, TipoArticulo.ServicioExterno, p_oJournalEntry) Then Return False
                        '****************Maneja transacción**************
                        'ResetTransaction()
                        'StartTransaction()
                        'If CrearAsiento(p_oDatosGeneralesList, oAsientoServicioExternoList, TipoArticulo.ServicioExterno) > 0 Then
                        '    ActualizarFacturaProveedores(p_oDatosGeneralesList)
                        '    '*****************Realiza commit ala transaccion**************
                        '    CommitTransaction()
                        '    '*****************Mensaje asiento generado correctamente*****************
                        '    SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioExternoExitoso, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                        'Else
                        '    RollbackTransaction()
                        '    SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioExternoError, SAPbouiCOM.BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error)
                        '    Exit Function
                        'End If
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Actualiza la factura de proveedores posterior al procesamiento de DMS
    ''' </summary>
    ''' <param name="p_oDatosGeneralesList"></param>
    ''' <remarks></remarks>
    Public Function ActualizarFacturaProveedores(ByRef p_strDocEntry As String) As Boolean
        Dim oFactura As SAPbobsCOM.Documents
        Try
            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                'Busca la factura de proveedores y actualiza todos los campos necesarios
                oFactura = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices)
                If oFactura.GetByKey(CInt(p_strDocEntry)) Then
                    oFactura.UserFields.Fields.Item("U_SCGD_ProASEF").Value = "N"
                    oFactura.Update()
                End If
            End If
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Sub ObtieneCostoEntradaMercancia(ByRef p_oLineasDocumentoMarketingList As DocumentoMarketing_List, ByRef p_oBaseEntryList As List(Of Integer))
        Dim oDocumentoMarketingBase As SAPbobsCOM.Documents
        Try
            '*************Objetos SAP *******************
            Dim oListaDocumentoMarketing As List(Of SAPbobsCOM.Documents) = New List(Of SAPbobsCOM.Documents)
            '**************Variables **************************
            Dim strIdItemDocMarketing As String = String.Empty
            Dim strIdItemDocMarketingBase As String = String.Empty
            Dim strNombreColumna As String = String.Empty
            Dim blnActualizaDocumentoMarketingBase As Boolean = False
            Dim intResultado As Integer = 1

            For Each DocEntry As Integer In p_oBaseEntryList
                oDocumentoMarketingBase = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes), SAPbobsCOM.Documents)
                If oDocumentoMarketingBase.GetByKey(DocEntry) Then
                    blnActualizaDocumentoMarketingBase = False
                    For row As Integer = 0 To oDocumentoMarketingBase.Lines.Count - 1
                        oDocumentoMarketingBase.Lines.SetCurrentLine(row)
                        For Each rowLinesDocMarketing As DocumentoMarketing In p_oLineasDocumentoMarketingList
                            If Not rowLinesDocMarketing.CostoAplicado And rowLinesDocMarketing.TipoArticulo = TipoArticulo.ServicioExterno Then
                                strIdItemDocMarketingBase = String.Empty
                                strIdItemDocMarketing = String.Empty
                                If Not String.IsNullOrEmpty(rowLinesDocMarketing.ID) Then
                                    strIdItemDocMarketing = rowLinesDocMarketing.ID
                                    strNombreColumna = "U_SCGD_ID"
                                ElseIf Not String.IsNullOrEmpty(rowLinesDocMarketing.IdRepxOrd) Then
                                    strIdItemDocMarketing = rowLinesDocMarketing.IdRepxOrd.ToString.Trim()
                                    strNombreColumna = "U_SCGD_IdRepxOrd"
                                End If
                                If Not String.IsNullOrEmpty(oDocumentoMarketingBase.Lines.UserFields.Fields.Item(strNombreColumna).Value) Then
                                    strIdItemDocMarketingBase = oDocumentoMarketingBase.Lines.UserFields.Fields.Item(strNombreColumna).Value.ToString.Trim()
                                End If
                                If strIdItemDocMarketingBase = strIdItemDocMarketing Then
                                    rowLinesDocMarketing.Costo = oDocumentoMarketingBase.Lines.LineTotal
                                    rowLinesDocMarketing.CostoAplicado = True
                                    Exit For
                                End If
                            End If
                        Next
                    Next
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            RollbackTransaction()
        Finally
            Utilitarios.DestruirObjeto(oDocumentoMarketingBase)
        End Try
    End Sub

    Public Sub ManejaCantidadesyCosto(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                      ByRef p_rowFactura As DocumentoMarketing, _
                                      ByRef p_oConfiguracionGeneralList As ConfiguracionGeneral_List)
        Try
            Select Case p_rowFactura.TipoArticulo
                Case TipoArticulo.ServicioExterno
                    If p_oConfiguracionGeneralList.Item(0).UsaCostosSEPorFacturaProveedor Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = p_rowFactura.Costo
                    End If
                    If p_rowFactura.BaseDocType <> TipoDocumentoMarketingBase.EntradaMercancia Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value += p_rowFactura.Cantidad
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value -= p_rowFactura.Cantidad
                        If p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value < 0 Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                        End If
                    End If
                Case Else
                    If p_rowFactura.BaseDocType <> TipoDocumentoMarketingBase.EntradaMercancia Then
                        If p_oConfiguracionGeneralList.Item(0).UsaBackOrder Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value += p_rowFactura.Cantidad
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value -= p_rowFactura.Cantidad
                            If p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value < 0 Then
                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                            End If
                        Else
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value += p_rowFactura.Cantidad
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value += (p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value) - p_rowFactura.Cantidad
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                        End If
                    End If
            End Select
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ActualizaValoresCotizacion(ByRef p_oNoOrdenList As Generic.List(Of String), _
                                          ByRef p_oLineaFacturaProveedorList As DocumentoMarketing_List, _
                                          ByRef p_oConfiguracionGeneralList As ConfiguracionGeneral_List)
        Dim oCotizacion As SAPbobsCOM.Documents
        Try
            '*************Objetos SAP *******************
            Dim oListaCotizacion As List(Of SAPbobsCOM.Documents) = New List(Of SAPbobsCOM.Documents)
            '***********Listas Genericas **********
            Dim oDocEntryCotizacionList As List(Of String) = New List(Of String)
            '*************Variables *********************
            Dim intDocEntry As Integer = 0
            Dim strCampo As String = String.Empty
            Dim blnUsaIdRepXOrd As Boolean = False
            Dim blnProcesaLinea As Boolean = False
            Dim blnActualizaCotizacion As Boolean = False
            Dim intResultado As Integer = 1

            If p_oConfiguracionGeneralList.Item(0).UsaCostosSEPorFacturaProveedor Or p_oLineaFacturaProveedorList.Item(0).BaseDocType <> TipoDocumentoMarketingBase.EntradaMercancia Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ActualizaCotizacion, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                CargarDocEntryCotizacion(p_oNoOrdenList, oDocEntryCotizacionList)
                For Each rowDocEntry As String In oDocEntryCotizacionList
                    If Not String.IsNullOrEmpty(rowDocEntry) Then
                        oCotizacion = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                                               SAPbobsCOM.Documents)
                        intDocEntry = Convert.ToInt32(rowDocEntry)
                        If oCotizacion.GetByKey(intDocEntry) Then
                            For Each rowFactura As DocumentoMarketing In p_oLineaFacturaProveedorList
                                blnActualizaCotizacion = False
                                If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                                    If oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim() = rowFactura.NoOrden Then
                                        strCampo = String.Empty
                                        blnUsaIdRepXOrd = False
                                        If Not String.IsNullOrEmpty(rowFactura.ID) Then
                                            strCampo = "U_SCGD_ID"
                                            blnUsaIdRepXOrd = False
                                        ElseIf Not String.IsNullOrEmpty(rowFactura.IdRepxOrd) Then
                                            strCampo = "U_SCGD_IdRepxOrd"
                                            blnUsaIdRepXOrd = True
                                        End If
                                        For contador As Integer = 0 To oCotizacion.Lines.Count - 1
                                            oCotizacion.Lines.SetCurrentLine(contador)
                                            blnProcesaLinea = False
                                            If blnUsaIdRepXOrd Then
                                                If oCotizacion.Lines.UserFields.Fields.Item(strCampo).Value = rowFactura.IdRepxOrd Then
                                                    blnProcesaLinea = True
                                                End If
                                            Else
                                                If oCotizacion.Lines.UserFields.Fields.Item(strCampo).Value.ToString.Trim() = rowFactura.ID Then
                                                    blnProcesaLinea = True
                                                End If
                                            End If
                                            If blnProcesaLinea Then
                                                ManejaCantidadesyCosto(oCotizacion, rowFactura, p_oConfiguracionGeneralList)
                                                blnActualizaCotizacion = True
                                                Exit For
                                            End If
                                        Next
                                    End If
                                End If
                            Next
                            oListaCotizacion.Add(oCotizacion)
                        End If
                    End If
                Next
                '****************Manejo Transaccion SAP ********************
                ResetTransaction()
                StartTransaction()
                For Each rowCotizacion As SAPbobsCOM.Documents In oListaCotizacion
                    intResultado = rowCotizacion.Update()
                    If intResultado <> 0 Then
                        RollbackTransaction()
                        Exit Sub
                    End If
                Next
                CommitTransaction()
            End If
        Catch ex As Exception
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            Utilitarios.DestruirObjeto(oCotizacion)
        End Try
    End Sub

    Public Function CargaFacturaProveedor(ByVal p_intDocEntry As Integer, _
                                          ByRef p_oLineaFacturaProveedorList As DocumentoMarketing_List, _
                                          ByRef p_oSucursalList As Generic.List(Of String), _
                                          ByRef p_oNoOrdenList As Generic.List(Of String), _
                                          ByRef p_oCodigoMarcaList As Generic.List(Of String), _
                                          ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                          ByRef p_oDatosGeneralesList As DatoGenerico_List, _
                                          ByRef p_oBaseEntryList As Generic.List(Of Integer)) As Boolean
        Dim oFacturaProveedor As SAPbobsCOM.Documents
        Try
            '**************Declaracion de data contract**********
            Dim oLineaFacturaProveedor As DocumentoMarketing
            Dim oTipoOT As ConfiguracionOrdenTrabajo
            Dim oDatosGenerales As DatoGenerico
            '************Variables********************************
            Dim intTipoArticulo As Integer = 0
            Dim strTipoArticulo As String = String.Empty
            Dim strCentroCosto As String = String.Empty
            Dim strSucursal As String = String.Empty
            Dim strNoOrden As String = String.Empty
            Dim strCodigoMarca As String = String.Empty
            Dim blnProcesaFacturaProveedor As Boolean = False
            Dim strMonedaLocal As String = String.Empty

            '****Consulta moneda local*********
            strMonedaLocal = ConsultaMonedaLocal()
            '************Verifica si DocEntry posee valor válido********************************
            If p_intDocEntry > 0 Then
                oFacturaProveedor = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices), SAPbobsCOM.Documents)
                '************Carga Objeto Factura de Proveedor********************************
                If oFacturaProveedor.GetByKey(p_intDocEntry) Then
                    oDatosGenerales = New DatoGenerico
                    With oDatosGenerales
                        .DocEntry = oFacturaProveedor.DocEntry
                        .DocNum = oFacturaProveedor.DocNum
                        .FechaContabilizacion = oFacturaProveedor.DocDate
                        .FechaCreacion = oFacturaProveedor.CreationDate
                        .CardCode = oFacturaProveedor.CardCode
                        .CardName = oFacturaProveedor.CardName
                        .MonedaLocal = strMonedaLocal
                        .Observaciones = oFacturaProveedor.Comments
                        If Not String.IsNullOrEmpty(oFacturaProveedor.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                            .NoOrden = oFacturaProveedor.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim()
                        End If
                    End With
                    p_oDatosGeneralesList.Add(oDatosGenerales)
                    '********Recorre lineas de la Factura de Proveedor***********************
                    For rowEntrada As Integer = 0 To oFacturaProveedor.Lines.Count - 1
                        oFacturaProveedor.Lines.SetCurrentLine(rowEntrada)
                        intTipoArticulo = 0
                        strTipoArticulo = String.Empty
                        strSucursal = String.Empty
                        strNoOrden = String.Empty
                        '************Valido si la linea pertenece a una OT********************************
                        If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                            If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString()) Then
                                intTipoArticulo = CInt(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                            Else
                                strTipoArticulo = DevuelveValorArticulo(oFacturaProveedor.Lines.ItemCode, "U_SCGD_TipoArticulo")
                                If Not String.IsNullOrEmpty(strTipoArticulo) Then
                                    intTipoArticulo = CInt(strTipoArticulo)
                                End If
                            End If
                            If intTipoArticulo = TipoArticulo.ServicioExterno Or intTipoArticulo = TipoArticulo.Repuesto Or intTipoArticulo = TipoArticulo.Suministro Then
                                oLineaFacturaProveedor = New DocumentoMarketing()
                                With oLineaFacturaProveedor
                                    .ItemCode = oFacturaProveedor.Lines.ItemCode
                                    .BodegaOrigen = oFacturaProveedor.Lines.WarehouseCode
                                    .TipoArticulo = intTipoArticulo
                                    .Cantidad = oFacturaProveedor.Lines.Quantity
                                    .BaseDocType = oFacturaProveedor.Lines.BaseType
                                    .BaseDocEntry = oFacturaProveedor.Lines.BaseEntry
                                    If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                        .NoOrden = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                    End If
                                    If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value) Then
                                        .TipoOT = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value
                                    ElseIf Not String.IsNullOrEmpty(oFacturaProveedor.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value) Then
                                        .TipoOT = oFacturaProveedor.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                                    End If
                                    If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_CodProy").Value) Then
                                        .CodigoProyecto = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_CodProy").Value
                                    End If
                                    .CostoFactura = oFacturaProveedor.Lines.LineTotal
                                    If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString()) Then
                                        .Sucursal = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value
                                    ElseIf Not String.IsNullOrEmpty(oFacturaProveedor.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                                        .Sucursal = oFacturaProveedor.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                                    End If
                                    If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value.ToString()) Then
                                        .CodigoMarca = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value
                                    ElseIf Not String.IsNullOrEmpty(oFacturaProveedor.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value) Then
                                        .CodigoMarca = oFacturaProveedor.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value
                                    End If
                                    If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value) Then
                                        .IdRepxOrd = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                    End If
                                    If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_ID").Value) Then
                                        .ID = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                                    End If
                                    If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString()) Then
                                        .CentroCosto = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString.Trim()
                                    Else
                                        .CentroCosto = DevuelveValorArticulo(oFacturaProveedor.Lines.ItemCode, "U_SCGD_CodCtroCosto")
                                    End If
                                End With
                                p_oLineaFacturaProveedorList.Add(oLineaFacturaProveedor)
                                '***************Agrega Sucursal al List*************************
                                If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value) Then
                                    strSucursal = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString()
                                    If Not p_oSucursalList.Contains(strSucursal) Then
                                        p_oSucursalList.Add(strSucursal)
                                    End If
                                ElseIf Not String.IsNullOrEmpty(oFacturaProveedor.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                                    strSucursal = oFacturaProveedor.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString()
                                    If Not p_oSucursalList.Contains(strSucursal) Then
                                        p_oSucursalList.Add(strSucursal)
                                    End If
                                End If
                                '**************Agrega NoOrden al List******************
                                If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                    strNoOrden = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                    If Not p_oNoOrdenList.Contains(strNoOrden) Then
                                        p_oNoOrdenList.Add(strNoOrden)
                                    End If
                                End If
                                '**************Agrega Codigo Marca al List******************
                                If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value) Then
                                    strCodigoMarca = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value
                                    If Not p_oCodigoMarcaList.Contains(strCodigoMarca) Then
                                        p_oCodigoMarcaList.Add(strCodigoMarca)
                                    End If
                                ElseIf Not String.IsNullOrEmpty(oFacturaProveedor.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value) Then
                                    strCodigoMarca = oFacturaProveedor.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value
                                    If Not p_oCodigoMarcaList.Contains(strCodigoMarca) Then
                                        p_oCodigoMarcaList.Add(strCodigoMarca)
                                    End If
                                End If
                                '**************Agrega TipoOT al List******************
                                If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value) Then
                                    oTipoOT = New ConfiguracionOrdenTrabajo
                                    With oTipoOT
                                        .TipoOT = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value
                                    End With
                                    If Not p_oTipoOTList.Contains(oTipoOT) Then
                                        p_oTipoOTList.Add(oTipoOT)
                                    End If
                                ElseIf Not String.IsNullOrEmpty(oFacturaProveedor.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value) Then
                                    oTipoOT = New ConfiguracionOrdenTrabajo
                                    With oTipoOT
                                        .TipoOT = oFacturaProveedor.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                                    End With
                                    If Not p_oTipoOTList.Contains(oTipoOT) Then
                                        p_oTipoOTList.Add(oTipoOT)
                                    End If
                                End If
                                '**************Agrega Base Entry al List******************
                                If Not p_oBaseEntryList.Contains(oFacturaProveedor.Lines.BaseEntry) Then
                                    p_oBaseEntryList.Add(oFacturaProveedor.Lines.BaseEntry)
                                End If
                                blnProcesaFacturaProveedor = True
                            End If

                        End If
                    Next
                End If
            End If
            Return blnProcesaFacturaProveedor
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            Utilitarios.DestruirObjeto(oFacturaProveedor)
        End Try
    End Function

    Public Sub AsignaCentrosCostoDimensiones(ByRef p_rowLineaFactura As DocumentoMarketing, _
                                             ByRef p_oListaTipoArticulo As DocumentoMarketing, _
                                             ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                             ByRef p_oDimensionesContablesList As DimensionesContables_List)
        Try
            For Each rowTipoOT As ConfiguracionOrdenTrabajo In p_oTipoOTList
                If p_rowLineaFactura.TipoOT = rowTipoOT.TipoOT Then
                    If rowTipoOT.UsaDimensionAsientoFacturaProveedor Then
                        For Each rowDimensionesContables As DimensionesContables In p_oDimensionesContablesList
                            If p_rowLineaFactura.Sucursal = rowDimensionesContables.Sucursal And p_rowLineaFactura.CodigoMarca = rowDimensionesContables.CodigoMarca Then
                                With p_oListaTipoArticulo
                                    .CostingCode = rowDimensionesContables.CostingCode
                                    .CostingCode2 = rowDimensionesContables.CostingCode2
                                    .CostingCode3 = rowDimensionesContables.CostingCode3
                                    .CostingCode4 = rowDimensionesContables.CostingCode4
                                    .CostingCode5 = rowDimensionesContables.CostingCode5
                                    .UsaDimensiones = True
                                End With
                                Exit For
                            End If
                        Next
                    End If
                    Exit For
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub CargaConfiguracionGeneral(ByRef p_oConfiguracionGeneralList As ConfiguracionGeneral_List)
        Try
            '********Declaración de data contract*************
            Dim oConfiguracionGeneral As ConfiguracionGeneral
            '********Declaración de variables*****************
            Dim oDataTableConfiguracionGeneral As System.Data.DataTable = Nothing
            Dim oDataRowConfiguracionGeneral As System.Data.DataRow
            '******************************************************************************
            '******************** Carga Configuración de tabla ConfiguracionSucursal*******
            '******************************************************************************
            oDataTableConfiguracionGeneral = Utilitarios.EjecutarConsultaDataTable(String.Format("Select U_GenAsSE, U_BO_Parc, U_CostSExFP From dbo.[@SCGD_ADMIN] with (nolock)"),
                                                       SBO_Company.CompanyDB,
                                                       SBO_Company.Server)
            '******************************************************************************
            '******************** Recorre configuraciones y agrega a objeto list*******
            '******************************************************************************
            For Each oDataRowConfiguracionGeneral In oDataTableConfiguracionGeneral.Rows
                oConfiguracionGeneral = New ConfiguracionGeneral()
                With oConfiguracionGeneral
                    '*********************************************************************
                    '**************Valida si genera asientos servicio externo*************
                    '*********************************************************************
                    If Not IsDBNull(oDataRowConfiguracionGeneral.Item("U_GenAsSE")) Then
                        If oDataRowConfiguracionGeneral.Item("U_GenAsSE").ToString.Trim() = "Y" Then
                            .UsaAsientoServicioExterno = True
                        Else
                            .UsaAsientoServicioExterno = False
                        End If
                    Else
                        .UsaAsientoServicioExterno = False
                    End If
                    '*********************************************************************
                    '**************Valida si usa back order*************
                    '*********************************************************************
                    If Not IsDBNull(oDataRowConfiguracionGeneral.Item("U_BO_Parc")) Then
                        If oDataRowConfiguracionGeneral.Item("U_BO_Parc").ToString.Trim() = "Y" Then
                            .UsaBackOrder = True
                        Else
                            .UsaBackOrder = False
                        End If
                    Else
                        .UsaBackOrder = False
                    End If
                    '***************************************************************************************************
                    '**************Valida si usa los costos de servicios externos de la factura de proveedor*************
                    '***************************************************************************************************
                    If Not IsDBNull(oDataRowConfiguracionGeneral.Item("U_CostSExFP")) Then
                        If oDataRowConfiguracionGeneral.Item("U_CostSExFP").ToString.Trim() = "Y" Then
                            .UsaCostosSEPorFacturaProveedor = True
                        Else
                            .UsaCostosSEPorFacturaProveedor = False
                        End If
                    Else
                        .UsaCostosSEPorFacturaProveedor = False
                    End If
                    '*********************************************************************
                    '**************Valida si usa OT SAP*************
                    '*********************************************************************
                    .UsaOTInterna = Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO)

                End With
                p_oConfiguracionGeneralList.Add(oConfiguracionGeneral)
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub CargaListasTipoArticulo(ByRef p_oLineaFacturaProveedorList As DocumentoMarketing_List, _
                                       ByRef p_oServicioExternoList As DocumentoMarketing_List, _
                                       ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                       ByRef p_rowConfiguracionSucursal As ConfiguracionSucursal, _
                                       ByRef p_oDimensionesContablesList As DimensionesContables_List, _
                                       ByRef p_oBodegaCentroCostoList As BodegaCentroCosto_List)
        Try
            '**************Declaracion de data contract**********
            Dim oServicioExterno As DocumentoMarketing
            Dim oTipoOT As ConfiguracionOrdenTrabajo
            '************Variables********************************

            For Each rowLineaFactura As DocumentoMarketing In p_oLineaFacturaProveedorList
                '********************Valida si la sucursal es la misma de la cual se esta recorriendo************
                If rowLineaFactura.Sucursal = p_rowConfiguracionSucursal.SucursalID Then
                    '************Según tipo de articulo valida que lista cargar********************************
                    Select Case rowLineaFactura.TipoArticulo
                        Case TipoArticulo.ServicioExterno
                            If p_rowConfiguracionSucursal.UsaAsientoServicioExterno Then
                                oServicioExterno = New DocumentoMarketing()
                                With oServicioExterno
                                    .ItemCode = rowLineaFactura.ItemCode
                                    .BodegaOrigen = rowLineaFactura.BodegaOrigen
                                    .TipoArticulo = rowLineaFactura.TipoArticulo
                                    .NoOrden = rowLineaFactura.NoOrden
                                    .TipoOT = rowLineaFactura.TipoOT
                                    .CodigoProyecto = rowLineaFactura.CodigoProyecto
                                    .Costo = rowLineaFactura.Costo
                                    .CostoFactura = rowLineaFactura.CostoFactura
                                    .Sucursal = rowLineaFactura.Sucursal
                                    .CodigoMarca = rowLineaFactura.CodigoMarca
                                    '*********************Asignación almacen segun centro de costo*********
                                    If Not String.IsNullOrEmpty(rowLineaFactura.CentroCosto) Then
                                        .CentroCosto = rowLineaFactura.CentroCosto
                                        AsignaBodegaCentroCosto(p_oBodegaCentroCostoList, rowLineaFactura, oServicioExterno)
                                    End If
                                    '*********************Valida que usa dimensiones y asigna centro de costo dimensiones*********
                                    If p_rowConfiguracionSucursal.UsaDimensiones Then
                                        AsignaCentrosCostoDimensiones(rowLineaFactura, oServicioExterno, p_oTipoOTList, p_oDimensionesContablesList)
                                    End If
                                End With
                                p_oServicioExternoList.Add(oServicioExterno)
                            End If
                    End Select
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub AsignaBodegaCentroCosto(ByRef p_oBodegaCentroCostoList As BodegaCentroCosto_List, _
                                       ByRef p_rowLineaFactura As DocumentoMarketing, _
                                       ByRef p_oServicioExterno As DocumentoMarketing)
        Try
            For Each row As BodegaCentroCosto In p_oBodegaCentroCostoList
                If row.CentroCosto = p_rowLineaFactura.CentroCosto AndAlso row.Sucursal = p_rowLineaFactura.Sucursal Then
                    p_oServicioExterno.Almacen = row.BodegaServicioExterno
                    Exit For
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function CrearAsiento(ByRef p_oDatosGeneralesList As DatoGenerico_List, _
                                ByRef p_oAsientoList As Asiento_List, _
                                ByVal p_intTipoArticulo As Integer, _
                                 ByRef p_oJournalEntry As SAPbobsCOM.JournalEntries) As Boolean
        Try
            '************Objetos*********************
            'Dim oJournalEntry As SAPbobsCOM.JournalEntries
            '************Variables*******************
            Dim intAsientoGenerado As Integer = 0
            Dim strAsientoGenerado As String = String.Empty
            Dim intDocEntry As Integer = 0
            Dim intDocNum As Integer = 0
            Dim dateFechaContabilizacion As Date = Nothing
            Dim strMonedaLocal As String = String.Empty
            Dim intError As Integer = 0
            Dim strMensajeError As String = String.Empty
            Dim strNoOrden As String = String.Empty

            For Each rowGeneral As DatoGenerico In p_oDatosGeneralesList
                With rowGeneral
                    intDocEntry = .DocEntry
                    intDocNum = .DocNum
                    dateFechaContabilizacion = .FechaContabilizacion
                    strMonedaLocal = .MonedaLocal
                    strNoOrden = .NoOrden
                End With
                Exit For
            Next

            If p_oAsientoList.Count > 0 Then
                p_oJournalEntry = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                If Not dateFechaContabilizacion = Nothing Then
                    p_oJournalEntry.ReferenceDate = dateFechaContabilizacion
                End If
                If Not String.IsNullOrEmpty(strNoOrden) Then
                    p_oJournalEntry.Reference = strNoOrden
                End If

                Select Case p_intTipoArticulo
                    Case TipoArticulo.Servicio
                        p_oJournalEntry.Memo = String.Empty
                    Case TipoArticulo.ServicioExterno
                        p_oJournalEntry.Memo = My.Resources.Resource.AsientoFacturaProveedores + intDocNum.ToString()
                    Case TipoArticulo.OtrosCostosGastos
                        p_oJournalEntry.Memo = String.Empty
                End Select


                For Each rowAsiento As Asiento In p_oAsientoList
                    '*********************
                    'Cuenta Credito
                    '*********************
                    p_oJournalEntry.Lines.AccountCode = rowAsiento.CuentaCredito

                    If rowAsiento.Moneda = strMonedaLocal Or rowAsiento.Moneda = Nothing Then
                        p_oJournalEntry.Lines.Credit = rowAsiento.Costo
                    Else
                        p_oJournalEntry.Lines.FCCredit = rowAsiento.Costo
                        p_oJournalEntry.Lines.FCCurrency = rowAsiento.Moneda
                    End If

                    p_oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    p_oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = rowAsiento.NoOrden
                    p_oJournalEntry.Lines.Reference1 = rowAsiento.NoOrden
                    If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If Not String.IsNullOrEmpty(rowAsiento.IDSucursal) Then p_oJournalEntry.Lines.BPLID = rowAsiento.IDSucursal
                    End If
                    If rowAsiento.UsaDimensiones Then
                        p_oJournalEntry.Lines.CostingCode = rowAsiento.CostingCode
                        p_oJournalEntry.Lines.CostingCode2 = rowAsiento.CostingCode2
                        p_oJournalEntry.Lines.CostingCode3 = rowAsiento.CostingCode3
                        p_oJournalEntry.Lines.CostingCode4 = rowAsiento.CostingCode4
                        p_oJournalEntry.Lines.CostingCode5 = rowAsiento.CostingCode5
                    End If

                    p_oJournalEntry.Lines.Add()

                    '*****************
                    'Cuenta Debito
                    '*****************
                    p_oJournalEntry.Lines.AccountCode = rowAsiento.CuentaDebito

                    If rowAsiento.Moneda = strMonedaLocal Or rowAsiento.Moneda = Nothing Then
                        p_oJournalEntry.Lines.Debit = rowAsiento.Costo
                    Else
                        p_oJournalEntry.Lines.FCDebit = rowAsiento.Costo
                        p_oJournalEntry.Lines.FCCurrency = rowAsiento.Moneda
                    End If

                    p_oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    p_oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = rowAsiento.NoOrden
                    p_oJournalEntry.Lines.Reference1 = rowAsiento.NoOrden
                    If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If Not String.IsNullOrEmpty(rowAsiento.IDSucursal) Then p_oJournalEntry.Lines.BPLID = rowAsiento.IDSucursal
                    End If
                    If rowAsiento.UsaDimensiones Then
                        p_oJournalEntry.Lines.CostingCode = rowAsiento.CostingCode
                        p_oJournalEntry.Lines.CostingCode2 = rowAsiento.CostingCode2
                        p_oJournalEntry.Lines.CostingCode3 = rowAsiento.CostingCode3
                        p_oJournalEntry.Lines.CostingCode4 = rowAsiento.CostingCode4
                        p_oJournalEntry.Lines.CostingCode5 = rowAsiento.CostingCode5
                    End If

                    p_oJournalEntry.Lines.Add()

                    '*****************
                    'Cuenta Diferencia
                    '*****************
                    If rowAsiento.CostoDiferencia <> 0 Then
                        AgregaCuentaDiferencia(rowAsiento, p_oJournalEntry, strMonedaLocal)
                    End If

                Next

                'If oJournalEntry.Add <> 0 Then
                '    intAsientoGenerado = 0
                '    SBO_Company.GetLastError(intError, strMensajeError)
                '    Throw New ExceptionsSBO(intError, strMensajeError)
                'Else
                '    SBO_Company.GetNewObjectCode(strAsientoGenerado)
                '    If Not String.IsNullOrEmpty(strAsientoGenerado) Then
                '        intAsientoGenerado = CInt(strAsientoGenerado)
                '    Else
                '        intAsientoGenerado = 0
                '    End If
                'End If
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function AgregaCuentaDiferencia(ByRef p_rowAsiento As Asiento, ByRef p_oJournalEntry As SAPbobsCOM.JournalEntries, ByRef p_strMonedaLocal As String) As Boolean
        Try
            '*****************
            'Cuenta Diferencia
            '*****************
            '****Cuando la factura es mayor a la entrada *****
            If p_rowAsiento.CostoDiferencia > 0 Then              
                '*********************
                'Cuenta Diferencia Credito
                '*********************
                p_oJournalEntry.Lines.AccountCode = p_rowAsiento.CuentaCredito

                If p_rowAsiento.Moneda = p_strMonedaLocal Or p_rowAsiento.Moneda = Nothing Then
                    p_oJournalEntry.Lines.Credit = p_rowAsiento.CostoDiferencia
                Else
                    p_oJournalEntry.Lines.FCCredit = p_rowAsiento.CostoDiferencia
                    p_oJournalEntry.Lines.FCCurrency = p_rowAsiento.Moneda
                End If

                p_oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                p_oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = p_rowAsiento.NoOrden
                p_oJournalEntry.Lines.Reference1 = p_rowAsiento.NoOrden
                If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                    If Not String.IsNullOrEmpty(p_rowAsiento.IDSucursal) Then p_oJournalEntry.Lines.BPLID = p_rowAsiento.IDSucursal
                End If
                If p_rowAsiento.UsaDimensiones Then
                    p_oJournalEntry.Lines.CostingCode = p_rowAsiento.CostingCode
                    p_oJournalEntry.Lines.CostingCode2 = p_rowAsiento.CostingCode2
                    p_oJournalEntry.Lines.CostingCode3 = p_rowAsiento.CostingCode3
                    p_oJournalEntry.Lines.CostingCode4 = p_rowAsiento.CostingCode4
                    p_oJournalEntry.Lines.CostingCode5 = p_rowAsiento.CostingCode5
                End If

                p_oJournalEntry.Lines.Add()

                '*****************
                'Cuenta Diferencia Debito
                '*****************
                p_oJournalEntry.Lines.AccountCode = p_rowAsiento.CuentaDiferencia

                If p_rowAsiento.Moneda = p_strMonedaLocal Or p_rowAsiento.Moneda = Nothing Then
                    p_oJournalEntry.Lines.Debit = p_rowAsiento.CostoDiferencia
                Else
                    p_oJournalEntry.Lines.FCDebit = p_rowAsiento.CostoDiferencia
                    p_oJournalEntry.Lines.FCCurrency = p_rowAsiento.Moneda
                End If

                p_oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                p_oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = p_rowAsiento.NoOrden
                p_oJournalEntry.Lines.Reference1 = p_rowAsiento.NoOrden
                If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                    If Not String.IsNullOrEmpty(p_rowAsiento.IDSucursal) Then p_oJournalEntry.Lines.BPLID = p_rowAsiento.IDSucursal
                End If
                If p_rowAsiento.UsaDimensiones Then
                    p_oJournalEntry.Lines.CostingCode = p_rowAsiento.CostingCode
                    p_oJournalEntry.Lines.CostingCode2 = p_rowAsiento.CostingCode2
                    p_oJournalEntry.Lines.CostingCode3 = p_rowAsiento.CostingCode3
                    p_oJournalEntry.Lines.CostingCode4 = p_rowAsiento.CostingCode4
                    p_oJournalEntry.Lines.CostingCode5 = p_rowAsiento.CostingCode5
                End If

                p_oJournalEntry.Lines.Add()


                '****Cuando la entrada es mayor a la factura *****
            ElseIf p_rowAsiento.CostoDiferencia < 0 Then
                p_rowAsiento.CostoDiferencia = p_rowAsiento.CostoDiferencia * -1
                '*********************
                'Cuenta Diferencia Credito
                '*********************
                p_oJournalEntry.Lines.AccountCode = p_rowAsiento.CuentaDiferencia

                If p_rowAsiento.Moneda = p_strMonedaLocal Or p_rowAsiento.Moneda = Nothing Then
                    p_oJournalEntry.Lines.Credit = p_rowAsiento.CostoDiferencia
                Else
                    p_oJournalEntry.Lines.FCCredit = p_rowAsiento.CostoDiferencia
                    p_oJournalEntry.Lines.FCCurrency = p_rowAsiento.Moneda
                End If

                p_oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                p_oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = p_rowAsiento.NoOrden
                p_oJournalEntry.Lines.Reference1 = p_rowAsiento.NoOrden
                If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                    If Not String.IsNullOrEmpty(p_rowAsiento.IDSucursal) Then p_oJournalEntry.Lines.BPLID = p_rowAsiento.IDSucursal
                End If
                If p_rowAsiento.UsaDimensiones Then
                    p_oJournalEntry.Lines.CostingCode = p_rowAsiento.CostingCode
                    p_oJournalEntry.Lines.CostingCode2 = p_rowAsiento.CostingCode2
                    p_oJournalEntry.Lines.CostingCode3 = p_rowAsiento.CostingCode3
                    p_oJournalEntry.Lines.CostingCode4 = p_rowAsiento.CostingCode4
                    p_oJournalEntry.Lines.CostingCode5 = p_rowAsiento.CostingCode5
                End If

                p_oJournalEntry.Lines.Add()

                '*****************
                'Cuenta Diferencia Debito
                '*****************
                p_oJournalEntry.Lines.AccountCode = p_rowAsiento.CuentaCredito

                If p_rowAsiento.Moneda = p_strMonedaLocal Or p_rowAsiento.Moneda = Nothing Then
                    p_oJournalEntry.Lines.Debit = p_rowAsiento.CostoDiferencia
                Else
                    p_oJournalEntry.Lines.FCDebit = p_rowAsiento.CostoDiferencia
                    p_oJournalEntry.Lines.FCCurrency = p_rowAsiento.Moneda
                End If

                p_oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                p_oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = p_rowAsiento.NoOrden
                p_oJournalEntry.Lines.Reference1 = p_rowAsiento.NoOrden
                If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                    If Not String.IsNullOrEmpty(p_rowAsiento.IDSucursal) Then p_oJournalEntry.Lines.BPLID = p_rowAsiento.IDSucursal
                End If
                If p_rowAsiento.UsaDimensiones Then
                    p_oJournalEntry.Lines.CostingCode = p_rowAsiento.CostingCode
                    p_oJournalEntry.Lines.CostingCode2 = p_rowAsiento.CostingCode2
                    p_oJournalEntry.Lines.CostingCode3 = p_rowAsiento.CostingCode3
                    p_oJournalEntry.Lines.CostingCode4 = p_rowAsiento.CostingCode4
                    p_oJournalEntry.Lines.CostingCode5 = p_rowAsiento.CostingCode5
                End If

                p_oJournalEntry.Lines.Add()
            End If
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub StartTransaction()
        Try
            If Not SBO_Company.InTransaction Then
                SBO_Company.StartTransaction()
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ResetTransaction()
        Try
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub CommitTransaction()
        Try
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_Commit)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub RollbackTransaction()
        Try
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function ObtenerCuentaAlmacen(ByRef p_strAlmacen As String, _
                                         ByRef p_intCuenta As Integer) As String
        Dim oAlmacen As SAPbobsCOM.Warehouses
        Try
            oAlmacen = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouses)
            If oAlmacen.GetByKey(p_strAlmacen) Then
                Select Case p_intCuenta
                    Case Account.ExpensesAc
                        Return oAlmacen.ExpenseAccount
                    Case Account.TransferAc
                        Return oAlmacen.TransfersAcc
                End Select
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            Utilitarios.DestruirObjeto(oAlmacen)
        End Try
    End Function

    Public Sub ProcesaAsientoServicioExterno(ByRef p_oServicioExternoList As DocumentoMarketing_List, _
                                             ByRef p_oLineaAsientoList As Asiento_List)
        Try
            '***********Data Contracts*********
            Dim oLineaAsiento As Asiento
            Dim oLineaAsientoTemporal As Asiento
            Dim oLineaAsientoTemporalList As Asiento_List = New Asiento_List
            '*****Variable***********
            Dim strCuentaDebito As String = String.Empty
            Dim strCuentaCredito As String = String.Empty
            Dim dblCosto As Double = 0
            Dim dblCostoDiferencia As Double = 0
            Dim blnAgregar As Boolean = False
            Dim strCuentaDiferencia As String = String.Empty
            '*************Recorre lineas ServicioList*****************
            For Each rowServicioExterno As DocumentoMarketing In p_oServicioExternoList
                strCuentaDebito = String.Empty
                strCuentaCredito = String.Empty
                strCuentaDiferencia = String.Empty
                oLineaAsientoTemporal = New Asiento
                With oLineaAsientoTemporal
                    .NoOrden = rowServicioExterno.NoOrden
                    .Costo = rowServicioExterno.Costo
                    .CostoDiferencia = rowServicioExterno.CostoFactura - rowServicioExterno.Costo
                    .Moneda = Nothing
                    '******Cuenta debito y cuenta credito************
                    strCuentaDebito = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.TransferAc, rowServicioExterno.Sucursal, rowServicioExterno.Almacen)
                    strCuentaCredito = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.ExpensesAc, rowServicioExterno.Sucursal, rowServicioExterno.Almacen)
                    strCuentaDiferencia = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.CtaDifPrecioSE, rowServicioExterno.Sucursal, rowServicioExterno.Almacen)
                    If Not String.IsNullOrEmpty(strCuentaDebito) Then
                        .CuentaDebito = strCuentaDebito
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                    End If
                    If Not String.IsNullOrEmpty(strCuentaCredito) Then
                        .CuentaCredito = strCuentaCredito
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaCreditoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                    End If
                    If Not String.IsNullOrEmpty(strCuentaDiferencia) Then
                        .CuentaDiferencia = strCuentaDiferencia
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCuentaDiferencia, SAPbouiCOM.BoMessageTime.bmt_Short)
                    End If
                    If Not String.IsNullOrEmpty(rowServicioExterno.Sucursal) Then oLineaAsientoTemporal.IDSucursal = rowServicioExterno.Sucursal
                    If rowServicioExterno.UsaDimensiones Then
                        .UsaDimensiones = True
                        .CostingCode = rowServicioExterno.CostingCode
                        .CostingCode2 = rowServicioExterno.CostingCode2
                        .CostingCode3 = rowServicioExterno.CostingCode3
                        .CostingCode4 = rowServicioExterno.CostingCode4
                        .CostingCode5 = rowServicioExterno.CostingCode5
                    End If
                End With
                oLineaAsientoTemporalList.Add(oLineaAsientoTemporal)
            Next
            'Recorre lineas de objeto temporal para agrupar el definitivo
            For Each rowAsiento1 As Asiento In oLineaAsientoTemporalList
                dblCosto = 0
                dblCostoDiferencia = 0
                blnAgregar = False
                For Each rowAsiento2 As Asiento In oLineaAsientoTemporalList
                    If rowAsiento2.NoOrden = rowAsiento1.NoOrden And rowAsiento2.CuentaDebito = rowAsiento1.CuentaDebito And rowAsiento2.CuentaCredito = rowAsiento1.CuentaCredito And rowAsiento2.Aplicado = False Then
                        dblCosto += rowAsiento2.Costo
                        dblCostoDiferencia += rowAsiento2.CostoDiferencia
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
                        .CuentaDiferencia = rowAsiento1.CuentaDiferencia
                        .Costo = dblCosto
                        .CostoDiferencia = dblCostoDiferencia
                        .Moneda = rowAsiento1.Moneda
                        .IDSucursal = rowAsiento1.IDSucursal
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
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ProcesaAsientoServicio(ByRef p_oServicioList As DocumentoMarketing_List, _
                                      ByRef p_oLineaAsientoList As Asiento_List)
        Try
            '***********Data Contracts*********
            Dim oLineaAsiento As Asiento
            Dim oLineaAsientoTemporal As Asiento
            Dim oLineaAsientoTemporalList As Asiento_List = New Asiento_List
            '*****Variable***********
            Dim strCuentaDebito As String = String.Empty
            Dim dblCosto As Double = 0
            Dim blnAgregar As Boolean = False
            '*************Recorre lineas ServicioList*****************
            For Each rowServicio As DocumentoMarketing In p_oServicioList
                strCuentaDebito = String.Empty
                oLineaAsientoTemporal = New Asiento
                With oLineaAsientoTemporal
                    .NoOrden = rowServicio.NoOrden
                    .CuentaCredito = rowServicio.CuentaCreditoManoObra
                    .Costo = rowServicio.Costo
                    .Moneda = rowServicio.MonedaManoObra
                    If Not String.IsNullOrEmpty(rowServicio.ItemCode) And Not String.IsNullOrEmpty(rowServicio.BodegaOrigen) Then
                        strCuentaDebito = ObtenerCuentaArticulo(rowServicio.ItemCode, rowServicio.BodegaOrigen, "SaleCostAc")
                    End If
                    If Not String.IsNullOrEmpty(strCuentaDebito) Then
                        .CuentaDebito = strCuentaDebito
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                    End If
                    If rowServicio.UsaDimensiones Then
                        .UsaDimensiones = True
                        .CostingCode = rowServicio.CostingCode
                        .CostingCode2 = rowServicio.CostingCode2
                        .CostingCode3 = rowServicio.CostingCode3
                        .CostingCode4 = rowServicio.CostingCode4
                        .CostingCode5 = rowServicio.CostingCode5
                    End If
                End With
                oLineaAsientoTemporalList.Add(oLineaAsientoTemporal)
            Next
            'Recorre lineas de objeto temporal para agrupar el definitivo
            For Each rowAsiento1 As Asiento In oLineaAsientoTemporalList
                dblCosto = 0
                blnAgregar = False
                For Each rowAsiento2 As Asiento In oLineaAsientoTemporalList
                    If rowAsiento2.NoOrden = rowAsiento1.NoOrden And rowAsiento2.CuentaDebito = rowAsiento1.CuentaDebito And rowAsiento2.Aplicado = False Then
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
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ProcesaAsientoOtrosCostosGastos(ByRef p_oOtrosGastosList As DocumentoMarketing_List, _
                                               ByRef p_oLineaAsientoList As Asiento_List)
        Try
            '***********Data Contracts*********
            Dim oLineaAsiento As Asiento
            Dim oLineaAsientoTemporal As Asiento
            Dim oLineaAsientoTemporalList As Asiento_List = New Asiento_List
            '*****Variable***********
            Dim strCuentaDebito As String = String.Empty
            Dim dblCosto As Double = 0
            Dim blnAgregar As Boolean = False
            '*************Recorre lineas ServicioList*****************
            For Each rowOtroGasto As DocumentoMarketing In p_oOtrosGastosList
                strCuentaDebito = String.Empty
                oLineaAsientoTemporal = New Asiento
                With oLineaAsientoTemporal
                    .NoOrden = rowOtroGasto.NoOrden
                    .CuentaCredito = rowOtroGasto.CuentaCreditoOtrosGastos
                    .Costo = rowOtroGasto.Costo
                    .Moneda = rowOtroGasto.MonedaOtrosGastos
                    If Not String.IsNullOrEmpty(rowOtroGasto.ItemCode) And Not String.IsNullOrEmpty(rowOtroGasto.BodegaOrigen) Then
                        strCuentaDebito = ObtenerCuentaArticulo(rowOtroGasto.ItemCode, rowOtroGasto.BodegaOrigen, "SaleCostAc")
                    End If
                    If Not String.IsNullOrEmpty(strCuentaDebito) Then
                        .CuentaDebito = strCuentaDebito
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                    End If
                    If rowOtroGasto.UsaDimensiones Then
                        .UsaDimensiones = True
                        .CostingCode = rowOtroGasto.CostingCode
                        .CostingCode2 = rowOtroGasto.CostingCode2
                        .CostingCode3 = rowOtroGasto.CostingCode3
                        .CostingCode4 = rowOtroGasto.CostingCode4
                        .CostingCode5 = rowOtroGasto.CostingCode5
                    End If
                End With
                oLineaAsientoTemporalList.Add(oLineaAsientoTemporal)
            Next
            'Recorre lineas de objeto temporal para agrupar el definitivo
            For Each rowAsiento1 As Asiento In oLineaAsientoTemporalList
                dblCosto = 0
                blnAgregar = False
                For Each rowAsiento2 As Asiento In oLineaAsientoTemporalList
                    If rowAsiento2.NoOrden = rowAsiento1.NoOrden And rowAsiento2.CuentaDebito = rowAsiento1.CuentaDebito And rowAsiento2.Aplicado = False Then
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
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function ObtenerCuentaArticulo(ByVal p_strItemCode As String, _
                                          ByVal p_strAlmacen As String, _
                                          ByVal p_strValor As String) As String
        Dim oItemArticulo As SAPbobsCOM.IItems
        Try
            '**********Variables****************
            Dim cuentaContable As String = String.Empty

            oItemArticulo = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            oItemArticulo.GetByKey(p_strItemCode)
            '*********Obtiene cuenta según configuración contable del articulo
            Select Case oItemArticulo.GLMethod
                Case SAPbobsCOM.BoGLMethods.glm_WH
                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select {0} FROM OWHS with(nolock) Where WhsCode = '{1}'",
                                                        p_strValor, p_strAlmacen), SBO_Company.CompanyDB, SBO_Company.Server)

                Case SAPbobsCOM.BoGLMethods.glm_ItemClass
                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select {0}  From OITB with(nolock) Where ItmsGrpCod = '{1}'",
                                                        p_strValor, oItemArticulo.ItemsGroupCode.ToString.Trim()),
                                                        SBO_Company.CompanyDB,
                                                        SBO_Company.Server)
                Case SAPbobsCOM.BoGLMethods.glm_ItemLevel
                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select {0} From OITW with(nolock) Where ItemCode= '{1}' AND WhsCode = '{2}'",
                                                        p_strValor, p_strItemCode, p_strAlmacen), SBO_Company.CompanyDB, SBO_Company.Server)
                Case Else
                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select {0} FROM OWHS with(nolock) Where WhsCode = '{1}'",
                                                        p_strValor, p_strAlmacen), SBO_Company.CompanyDB, SBO_Company.Server)
            End Select
            Return cuentaContable
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            If Not oItemArticulo Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oItemArticulo)
                oItemArticulo = Nothing
            End If
        End Try
    End Function

    Public Sub CargaConfiguracionSucursal(ByRef p_oSucursalList As Generic.List(Of String), _
                                          ByRef p_oConfiguracionSucursalList As ConfiguracionSucursal_List, _
                                          ByRef p_oBodegaCentroCostoList As BodegaCentroCosto_List)
        Try
            '********Declaración de data contract*************
            Dim oConfiguracionSucursal As ConfiguracionSucursal
            '********Declaración de variables*****************
            Dim oDataTableConfiguracionSucursal As System.Data.DataTable = Nothing
            Dim oDataRowConfiguracionSucursal As System.Data.DataRow
            Dim strIDSucursales As String = String.Empty
            Dim blnUsaAsientoServicioExterno As Boolean = False
            Dim intContSucursalList As Integer = 0
            Dim intContTemporal As Integer = 0
            '******************************************************************************
            '******************** Carga Configuración de tabla ConfiguracionSucursal*******
            '******************************************************************************
            intContSucursalList = p_oSucursalList.Count()
            For Each rowSucursal As String In p_oSucursalList
                intContTemporal += 1
                If intContTemporal = intContSucursalList Then
                    strIDSucursales = strIDSucursales & String.Format("'{0}'", rowSucursal)
                Else
                    strIDSucursales = strIDSucursales & String.Format("'{0}', ", rowSucursal)
                End If
            Next
            If (strIDSucursales.Length > 0) Then
                strIDSucursales = strIDSucursales.Substring(0, strIDSucursales.Length - 0)
                oDataTableConfiguracionSucursal = Utilitarios.EjecutarConsultaDataTable(String.Format("Select U_GenAsSE, U_UsaDimC,U_Sucurs From [@SCGD_CONF_SUCURSAL] with (nolock), dbo.[@SCGD_ADMIN] with (nolock)  Where U_Sucurs in ({0})",
                                                           strIDSucursales), SBO_Company.CompanyDB, SBO_Company.Server)
                Utilitarios.ObtenerAlmacenXCentroCosto(p_oSucursalList, SBO_Company, p_oBodegaCentroCostoList)
            End If
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
                    '*********************************************************************
                    '**************Valida si dimensiones*************
                    '*********************************************************************
                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_UsaDimC")) Then
                        If oDataRowConfiguracionSucursal.Item("U_UsaDimC").ToString.Trim() = "Y" Then
                            .UsaDimensiones = True
                        Else
                            .UsaDimensiones = False
                        End If
                    Else
                        .UsaDimensiones = False
                    End If
                End With
                p_oConfiguracionSucursalList.Add(oConfiguracionSucursal)
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub


    Private Function DevuelveValorArticulo(ByVal strItemcode As String, _
                                           ByVal strUDfName As String) As String
        Try
            Dim oItemArticulo As SAPbobsCOM.IItems
            Dim valorUDF As String = String.Empty

            oItemArticulo = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            oItemArticulo.GetByKey(strItemcode)
            If oItemArticulo IsNot Nothing Then
                valorUDF = oItemArticulo.UserFields.Fields.Item(strUDfName).Value
                If Not String.IsNullOrEmpty(valorUDF) Then
                    Return valorUDF
                Else
                    Return String.Empty
                End If
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Function

    Private Sub ValidaUsaDimensionesTipoOT(ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List)
        Try
            '**************Declaración DataContracts****************
            Dim oConfiguracionOrdenTrabajoList As ConfiguracionOrdenTrabajo_List = New ConfiguracionOrdenTrabajo_List()
            '**************Declaración de variables******************************
            Dim ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls = New AgregarDimensionLineasDocumentosCls(SBO_Company, SBO_Application)
            ClsLineasDocumentosDimension.ObtieneConfiguracionDimensionesOT(oConfiguracionOrdenTrabajoList)
            For Each rowTipoOT As ConfiguracionOrdenTrabajo In p_oTipoOTList
                For Each rowConfiguracion As ConfiguracionOrdenTrabajo In oConfiguracionOrdenTrabajoList
                    If rowTipoOT.TipoOT = rowConfiguracion.TipoOT Then
                        rowTipoOT.UsaDimensiones = rowConfiguracion.UsaDimensiones
                        rowTipoOT.UsaDimensionAsientoEntradaMercancia = rowConfiguracion.UsaDimensionAsientoEntradaMercancia
                        rowTipoOT.UsaDimensionAsientoFacturaProveedor = rowConfiguracion.UsaDimensionAsientoFacturaProveedor
                        Exit For
                    End If
                Next
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function ConsultaMonedaLocal() As String
        Try
            '*****Variables*******
            Dim strMonedaLocal As String = String.Empty

            strMonedaLocal = Utilitarios.EjecutarConsulta("Select mainCurncy from OADM with(nolock)", SBO_Company.CompanyDB, SBO_Company.Server)

            Return strMonedaLocal
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Function

    Public Sub CargarDocEntryCotizacion(ByVal p_oListaNoOrden As Generic.List(Of String), _
                                        ByRef p_oListaCotizacion As Generic.List(Of String))
        Try
            Dim strNoOrden As String = String.Empty
            Dim strQuery As String = String.Empty
            Dim dtCotizacion As System.Data.DataTable
            Dim intDocEntry As Integer = 0

            For Each rowOT As String In p_oListaNoOrden
                If Not strNoOrden.Contains(rowOT) Then
                    strNoOrden = strNoOrden & String.Format("'{0}', ", rowOT)
                End If
            Next
            If (strNoOrden.Length > 0) Then
                strNoOrden = strNoOrden.Substring(0, strNoOrden.Length - 2)
                strQuery = String.Format("select Q.DocEntry from OQUT Q with (nolock) where Q.U_SCGD_Numero_OT in ({0})", strNoOrden)
                dtCotizacion = Utilitarios.EjecutarConsultaDataTable(strQuery, SBO_Company.CompanyDB, SBO_Company.Server)
            End If
            For Each rowCotizacion As DataRow In dtCotizacion.Rows
                If Not String.IsNullOrEmpty(rowCotizacion.Item("DocEntry")) Then
                    If Not p_oListaCotizacion.Contains(rowCotizacion.Item("DocEntry")) Then
                        p_oListaCotizacion.Add(rowCotizacion.Item("DocEntry"))
                    End If
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub
#End Region

    Public Function CrearAsientoServicioExternoORIGINAL(ByVal p_oListaAsientoServicioExterno As List(Of ListaLineaAsientoServExterno), _
                                               ByVal p_blnUsaDimensiones As Boolean, _
                                               ByVal p_dateFechaConta As Date, _
                                               ByVal p_strDocNum As String) As String
        Try

            Dim oJE_Lines As SAPbobsCOM.JournalEntries_Lines
            Dim oJournalEntry As SAPbobsCOM.JournalEntries
            Dim strAsiento As String = String.Empty
            Dim strAsientoGenerado As String = "0"
            Dim intError As Integer
            Dim strMensajeError As String = ""
            Dim formato As String
            Dim dateFechaRegistro As Date = Nothing

            strAsientoGenerado = "0"

            oJournalEntry = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            oJournalEntry.Memo = My.Resources.Resource.AsientoFacturaProveedores + p_strDocNum
            If p_dateFechaConta <> Nothing Then
                oJournalEntry.ReferenceDate = p_dateFechaConta
            End If

            For Each row As ListaLineaAsientoServExterno In p_oListaAsientoServicioExterno
                '*********************
                'Cuenta Credito
                '*********************
                oJournalEntry.Lines.AccountCode = row.AccountCredit
                oJournalEntry.Lines.Credit = row.Credit
                oJournalEntry.Lines.FCCredit = 0

                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.Reference1 = row.NoOrden
                If Not String.IsNullOrEmpty(row.CodProyecto) Then
                    oJournalEntry.Lines.ProjectCode = row.CodProyecto
                End If
                If p_blnUsaDimensiones Then
                    If row.AplicadoCargaDimensiones = True Then
                        If Not String.IsNullOrEmpty(row.CostingCode) Then
                            oJournalEntry.Lines.CostingCode = row.CostingCode
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode2) Then
                            oJournalEntry.Lines.CostingCode2 = row.CostingCode2
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode3) Then
                            oJournalEntry.Lines.CostingCode3 = row.CostingCode3
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode4) Then
                            oJournalEntry.Lines.CostingCode4 = row.CostingCode4
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode5) Then
                            oJournalEntry.Lines.CostingCode5 = row.CostingCode5
                        End If
                    End If
                End If
                oJournalEntry.Lines.Add()
                '*****************
                'Cuenta Debito
                '*****************
                oJournalEntry.Lines.AccountCode = row.AccountDebit
                oJournalEntry.Lines.Debit = row.Debit
                oJournalEntry.Lines.FCDebit = 0

                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.Reference1 = row.NoOrden
                If Not String.IsNullOrEmpty(row.CodProyecto) Then
                    oJournalEntry.Lines.ProjectCode = row.CodProyecto
                End If
                If p_blnUsaDimensiones Then
                    If row.AplicadoCargaDimensiones = True Then
                        If Not String.IsNullOrEmpty(row.CostingCode) Then
                            oJournalEntry.Lines.CostingCode = row.CostingCode
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode2) Then
                            oJournalEntry.Lines.CostingCode2 = row.CostingCode2
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode3) Then
                            oJournalEntry.Lines.CostingCode3 = row.CostingCode3
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode4) Then
                            oJournalEntry.Lines.CostingCode4 = row.CostingCode4
                        End If
                        If Not String.IsNullOrEmpty(row.CostingCode5) Then
                            oJournalEntry.Lines.CostingCode5 = row.CostingCode5
                        End If
                    End If
                End If
                oJournalEntry.Lines.Add()
                '*****************
                'Cuenta Debito Diferencia
                '*****************
                If row.DebitDiferencial > 0 Then
                    oJournalEntry.Lines.AccountCode = row.AccountDebitDiferencial
                    oJournalEntry.Lines.Debit = row.DebitDiferencial
                    oJournalEntry.Lines.FCDebit = 0

                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    oJournalEntry.Lines.Reference1 = row.NoOrden
                    If Not String.IsNullOrEmpty(row.CodProyecto) Then
                        oJournalEntry.Lines.ProjectCode = row.CodProyecto
                    End If
                    If p_blnUsaDimensiones Then
                        If row.AplicadoCargaDimensiones = True Then
                            If Not String.IsNullOrEmpty(row.CostingCode) Then
                                oJournalEntry.Lines.CostingCode = row.CostingCode
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode2) Then
                                oJournalEntry.Lines.CostingCode2 = row.CostingCode2
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode3) Then
                                oJournalEntry.Lines.CostingCode3 = row.CostingCode3
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode4) Then
                                oJournalEntry.Lines.CostingCode4 = row.CostingCode4
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode5) Then
                                oJournalEntry.Lines.CostingCode5 = row.CostingCode5
                            End If
                        End If
                    End If
                    oJournalEntry.Lines.Add()
                ElseIf row.CreditDiferencial > 0 Then
                    oJournalEntry.Lines.AccountCode = row.AccountDebitDiferencial
                    oJournalEntry.Lines.Credit = row.CreditDiferencial
                    oJournalEntry.Lines.FCCredit = 0

                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    oJournalEntry.Lines.Reference1 = row.NoOrden
                    If Not String.IsNullOrEmpty(row.CodProyecto) Then
                        oJournalEntry.Lines.ProjectCode = row.CodProyecto
                    End If
                    If p_blnUsaDimensiones Then
                        If row.AplicadoCargaDimensiones = True Then
                            If Not String.IsNullOrEmpty(row.CostingCode) Then
                                oJournalEntry.Lines.CostingCode = row.CostingCode
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode2) Then
                                oJournalEntry.Lines.CostingCode2 = row.CostingCode2
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode3) Then
                                oJournalEntry.Lines.CostingCode3 = row.CostingCode3
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode4) Then
                                oJournalEntry.Lines.CostingCode4 = row.CostingCode4
                            End If
                            If Not String.IsNullOrEmpty(row.CostingCode5) Then
                                oJournalEntry.Lines.CostingCode5 = row.CostingCode5
                            End If
                        End If
                    End If
                    oJournalEntry.Lines.Add()
                End If
            Next
            If oJournalEntry.Add <> 0 Then
                strAsientoGenerado = "0"
                SBO_Company.GetLastError(intError, strMensajeError)
                Utilitarios.DestruirObjeto(oJournalEntry)
                SBO_Application.StatusBar.SetText(strMensajeError, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Throw New ExceptionsSBO(intError, strMensajeError)
            Else
                SBO_Company.GetNewObjectCode(strAsientoGenerado)
            End If
            Utilitarios.DestruirObjeto(oJournalEntry)
            Return strAsientoGenerado
        Catch ex As Exception
        End Try
    End Function

#Region "Nuevos metodos actualiza costos y cantidades cotizacion"
    Public Function ProcesaCantidadesyCostosCotizacion(ByRef p_strDocEntry As String, ByRef p_oListCotizacion As List(Of SAPbobsCOM.Documents)) As Boolean
        Try
            Dim oLineaFacturaProveedorList As DocumentoMarketing_List = New DocumentoMarketing_List
            Dim oNoOrdenList As List(Of String) = New Generic.List(Of String)
            Dim strDocEntryCotizacion As String = String.Empty
            Dim oCotizacion As SAPbobsCOM.Documents
            Dim CancelStatus As SAPbobsCOM.CancelStatusEnum

            If CargaDocumentoFacturaProveedor(Convert.ToInt32(p_strDocEntry), oLineaFacturaProveedorList, oNoOrdenList, CancelStatus) Then
                If oLineaFacturaProveedorList.Count > 0 Then
                    For Each rowNoOrden As String In oNoOrdenList
                        Try
                            InicializarTimer()
                            If Not String.IsNullOrEmpty(rowNoOrden) Then
                                strDocEntryCotizacion = CargaDocEntryCotizacion(rowNoOrden)
                                If Not String.IsNullOrEmpty(strDocEntryCotizacion) Then
                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ActualizaCotizacion + " " + rowNoOrden.ToString(), SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                    If ActualizaCantidadesyCostosCotizacion(strDocEntryCotizacion, oLineaFacturaProveedorList, CancelStatus, oCotizacion) Then
                                        If Not IsNothing(oCotizacion) Then
                                            p_oListCotizacion.Add(oCotizacion)
                                        End If
                                    End If
                                End If
                            End If
                        Finally
                            DetenerTimer()
                        End Try
                    Next
                End If
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function CargaDocEntryCotizacion(ByRef p_strNoOrden As String) As String
        Try
            Dim strQuery As String = String.Empty
            Dim strDocEntryCotizacion As String = String.Empty
            If Not String.IsNullOrEmpty(p_strNoOrden) Then
                strQuery = String.Format("select Q.DocEntry from OQUT Q with (nolock) where Q.U_SCGD_Numero_OT = '{0}'", p_strNoOrden.Trim())
                strDocEntryCotizacion = Utilitarios.EjecutarConsulta(strQuery, SBO_Company.CompanyDB, SBO_Company.Server)
            End If
            Return strDocEntryCotizacion
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return String.Empty
        End Try
    End Function

    Public Function ActualizaCantidadesyCostosCotizacion(ByRef p_strDocEntry As String, ByRef p_oLineaFacturaProveedorList As DocumentoMarketing_List, ByRef CancelStatus As SAPbobsCOM.CancelStatusEnum, ByRef p_oCotizacion As SAPbobsCOM.Documents) As Boolean
        Dim oDocumento As DMS_Connector.Business_Logic.DataContract.SAPDocumento.oDocumento
        Dim temp_LineaFacturaProveedorListXOT As List(Of DocumentoMarketing) = New List(Of DocumentoMarketing)
        Dim strNoOT As String
        Dim strIDSucur As String
        Dim strTipoOT As String
        Dim strMonedaLocal As String
        Dim strMonedaSistema As String
        Dim dblCostoItm As Double
        Dim dblPrice As Double
        Dim dblPocentaje As Double
        Dim SapItem As SAPbobsCOM.Items
        Dim strID As String
        Dim intPosicion As Integer
        Dim CantidadOfertaVentas As Double = 0
        Dim CantidadRecibida As Double = 0
        Dim CantidadPendiente As Double = 0
        Dim CantidadSolicitada As Double = 0
        Dim CantidadAbiertaDocumentoCompra As Double = 0
        Dim GeneraMovimientoInventario As Boolean = False
        Dim TipoMovimiento As CalculoCantidades.TipoMovimiento
        Dim CostoOfertaVentas As Double = 0
        Dim CostoDocumentoCompra As Double = 0
        Try
            '*************Variables *********************
            Dim intDocEntry As Integer = 0
            Dim blnActualizaCotizacion As Boolean = False
            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                intDocEntry = Convert.ToInt32(p_strDocEntry)
                p_oCotizacion = Nothing
                oDocumento = DMS_Connector.Helpers.CargaCotizacionConPosiciones(intDocEntry, p_oCotizacion)
                If Not IsNothing(p_oCotizacion) Then
                    DMS_Connector.Helpers.GetCurrencies(strMonedaLocal, strMonedaSistema)
                    strNoOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim()
                    'Filtro de lineas por OT, para no recorrer todo la Factura
                    temp_LineaFacturaProveedorListXOT = p_oLineaFacturaProveedorList.FindAll(Function(row) row.NoOrden.Trim = strNoOT)
                    For Each rowFactura As DocumentoMarketing In temp_LineaFacturaProveedorListXOT
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString().Trim()) Then
                            If p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim() = rowFactura.NoOrden.Trim Then
                                strID = rowFactura.ID.Trim()
                                intPosicion = DMS_Connector.Helpers.GetLinePosition(oDocumento.Lineas, strID)
                                If intPosicion <> -1 Then
                                    p_oCotizacion.Lines.SetCurrentLine(intPosicion)
                                    strIDSucur = rowFactura.Sucursal.Trim()
                                    If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(x) x.U_Sucurs = strIDSucur) AndAlso rowFactura.TipoArticulo = TipoArticulo.ServicioExterno Then
                                        If DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs = strIDSucur).U_UsaPreAutSE = "Y" Then
                                            dblCostoItm = rowFactura.Costo
                                            strTipoOT = rowFactura.TipoOT
                                            SapItem = DMS_Connector.Helpers.GetItem(p_oCotizacion.Lines.ItemCode)
                                            If Not IsNothing(SapItem) Then
                                                dblPocentaje = Convert.ToDouble(SapItem.UserFields.Fields.Item("U_SCGD_PorcSE").Value)
                                                If p_oCotizacion.Lines.Currency <> strMonedaLocal Then
                                                    dblPrice = Utilitarios.ManejoMultimoneda((dblCostoItm + (dblCostoItm * (dblPocentaje / 100))), strMonedaLocal, strMonedaSistema, strMonedaLocal, p_oCotizacion.DocCurrency, p_oCotizacion.DocRate, p_oCotizacion.CreationDate, n, SBO_Company)
                                                Else
                                                    dblPrice = dblCostoItm + (dblCostoItm * (dblPocentaje / 100))
                                                End If
                                                p_oCotizacion.Lines.UnitPrice = dblPrice / p_oCotizacion.Lines.Quantity
                                            End If
                                        End If
                                    End If
                                    'If DMS_Connector.Configuracion.ParamGenAddon.U_CostSExFP = "Y" Then
                                    '    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = rowFactura.Costo
                                    'ElseIf (oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = 0) Then
                                    '    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = rowFactura.Costo
                                    'End If

                                    If CancelStatus = CancelStatusEnum.csCancellation Then
                                        TipoMovimiento = CalculoCantidades.TipoMovimiento.Cancelacion
                                        If rowFactura.BaseDocType = SAPbobsCOM.BoAPARDocumentTypes.bodt_PurchaseInvoice Then
                                            If ExistenEntradas(rowFactura.DocEntry, rowFactura.LineNum) Then
                                                GeneraMovimientoInventario = False
                                            Else
                                                GeneraMovimientoInventario = True
                                            End If
                                        Else
                                            GeneraMovimientoInventario = True
                                        End If
                                    Else
                                        TipoMovimiento = CalculoCantidades.TipoMovimiento.Creacion
                                        If rowFactura.BaseDocType = SAPbobsCOM.BoAPARDocumentTypes.bodt_PurchaseDeliveryNote Then
                                            GeneraMovimientoInventario = False
                                        Else
                                            GeneraMovimientoInventario = True
                                        End If
                                    End If

                                    CantidadAbiertaDocumentoCompra = rowFactura.Cantidad
                                    CantidadSolicitada = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value
                                    CantidadPendiente = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value
                                    CantidadRecibida = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value

                                    CalculoCantidades.RecalcularCantidades(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices, TipoMovimiento, GeneraMovimientoInventario, p_oCotizacion.Lines.Quantity, CantidadAbiertaDocumentoCompra, CantidadSolicitada, CantidadPendiente, CantidadRecibida)

                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = CantidadRecibida
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = CantidadSolicitada
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = CantidadPendiente

                                    CostoOfertaVentas = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value
                                    CostoDocumentoCompra = rowFactura.Costo

                                    CalculoCantidades.RecalcularCostos(BoObjectTypes.oPurchaseInvoices, TipoMovimiento, GeneraMovimientoInventario, p_oCotizacion.Lines.Quantity, CostoOfertaVentas, CantidadAbiertaDocumentoCompra, CostoDocumentoCompra)

                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = CostoOfertaVentas

                                    blnActualizaCotizacion = True
                                    p_oLineaFacturaProveedorList.Remove(rowFactura)
                                End If
                            End If
                        End If
                    Next
                End If
            End If
            ''****************Manejo Transaccion SAP ********************
            'If blnActualizaCotizacion Then
            '    ResetTransaction()
            '    StartTransaction()
            '    If oCotizacion.Update() <> 0 Then
            '        SBO_Application.StatusBar.SetText(String.Format("{0}", SBO_Company.GetLastErrorDescription), SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            '        SCG.ServicioPostVenta.Utilitarios.ManejadorErrores(New Exception(String.Format("{0}: {1}", SBO_Company.GetLastErrorDescription, p_strDocEntry)), SBO_Application)
            '        RollbackTransaction()
            '    Else
            '        CommitTransaction()
            '    End If
            'End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            Utilitarios.DestruirObjeto(SapItem)
        End Try
    End Function

    Public Function ExistenEntradas(ByVal DocEntry As Integer, ByVal LineNum As Integer) As Boolean
        Dim Query As String = "SELECT COUNT(*) AS ""Cuenta"" FROM ""PCH1"" T0 WITH (nolock) INNER JOIN ""PCH1"" T1 WITH (nolock) ON T1.""DocEntry"" = T0.""BaseEntry"" AND T1.""LineNum"" = T0.""BaseLine"" AND T1.""ObjType"" = T0.""BaseType"" INNER JOIN ""PDN1"" T2 WITH (nolock) ON T2.""DocEntry"" = T1.""BaseEntry"" AND T2.""LineNum"" = T1.""BaseLine"" AND T2.""ObjType"" = T1.""BaseType"" WHERE T0.""DocEntry"" = '{0}' AND T0.""LineNum"" = '{1}'"
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim Cuenta As Integer = 0
        Try
            ExistenEntradas = False
            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.BoRecordset)
            Query = String.Format(Query, DocEntry, LineNum)
            oRecordset.DoQuery(Query)
            Cuenta = oRecordset.Fields.Item("Cuenta").Value
            If Cuenta > 0 Then
                ExistenEntradas = True
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            ExistenEntradas = False
        End Try
    End Function

    Public Function CargaDocumentoFacturaProveedor(ByVal p_intDocEntry As Integer, _
                                                   ByRef p_oLineaFacturaProveedorList As DocumentoMarketing_List, _
                                                   ByRef p_oNoOrdenList As Generic.List(Of String), ByRef CancelStatus As SAPbobsCOM.CancelStatusEnum) As Boolean
        Dim oFacturaProveedor As SAPbobsCOM.Documents
        Try
            '**************Declaracion de data contract**********
            Dim oLineaFacturaProveedor As DocumentoMarketing
            '************Variables********************************
            Dim intTipoArticulo As Integer = 0
            Dim strTipoArticulo As String = String.Empty
            Dim strNoOrden As String = String.Empty
            Dim blnProcesaFacturaProveedor As Boolean = False

            '************Verifica si DocEntry posee valor válido********************************
            If p_intDocEntry > 0 Then
                oFacturaProveedor = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices), SAPbobsCOM.Documents)
                '************Carga Objeto Entrada Mercancia********************************
                If oFacturaProveedor.GetByKey(p_intDocEntry) Then
                    CancelStatus = oFacturaProveedor.CancelStatus
                    '********Recorre lineas de la Factura de Proveedor***********************
                    For rowFactura As Integer = 0 To oFacturaProveedor.Lines.Count - 1
                        oFacturaProveedor.Lines.SetCurrentLine(rowFactura)
                        intTipoArticulo = 0
                        strTipoArticulo = String.Empty
                        strNoOrden = String.Empty
                        '************Valido si la linea pertenece a una OT********************************
                        If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                            If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString()) Then
                                intTipoArticulo = CInt(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                            Else
                                strTipoArticulo = DevuelveValorArticulo(oFacturaProveedor.Lines.ItemCode, "U_SCGD_TipoArticulo")
                                If Not String.IsNullOrEmpty(strTipoArticulo) Then
                                    intTipoArticulo = CInt(strTipoArticulo)
                                End If
                            End If
                            oLineaFacturaProveedor = New DocumentoMarketing()
                            With oLineaFacturaProveedor
                                .ItemCode = oFacturaProveedor.Lines.ItemCode
                                .TipoArticulo = intTipoArticulo
                                .Cantidad = oFacturaProveedor.Lines.Quantity
                                If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                    .NoOrden = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                End If
                                .Costo = oFacturaProveedor.Lines.LineTotal
                                If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_ID").Value) Then
                                    .ID = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                                End If
                                If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value) Then
                                    .Sucursal = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value
                                End If
                                If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value) Then
                                    .TipoOT = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value
                                End If
                                .BaseDocType = oFacturaProveedor.Lines.BaseType
                                .DocEntry = oFacturaProveedor.Lines.DocEntry
                                .LineNum = oFacturaProveedor.Lines.LineNum
                            End With
                            p_oLineaFacturaProveedorList.Add(oLineaFacturaProveedor)
                            '**************Agrega NoOrden al List******************
                            If Not String.IsNullOrEmpty(oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                strNoOrden = oFacturaProveedor.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                If Not p_oNoOrdenList.Contains(strNoOrden) Then
                                    p_oNoOrdenList.Add(strNoOrden)
                                End If
                            End If
                            blnProcesaFacturaProveedor = True
                        End If
                    Next
                End If
            End If
            Return blnProcesaFacturaProveedor
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            If Not oFacturaProveedor Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oFacturaProveedor)
                oFacturaProveedor = Nothing
            End If
        End Try
    End Function
#End Region
End Class

' Clase para la definición de la lista
Public Class ListaLineaAsientoServExterno
    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String
    Public Property AccountDebit() As String
        Get
            Return strAccountDebit
        End Get
        Set(ByVal value As String)
            strAccountDebit = value
        End Set
    End Property
    Private strAccountDebit As String

    Public Property AccountCredit() As String
        Get
            Return strAccountCredit
        End Get
        Set(ByVal value As String)
            strAccountCredit = value
        End Set
    End Property
    Private strAccountCredit As String

    Public Property AccountDebitDiferencial() As String
        Get
            Return strAccountDebitDiferencial
        End Get
        Set(ByVal value As String)
            strAccountDebitDiferencial = value
        End Set
    End Property
    Private strAccountDebitDiferencial As String

    Public Property Debit() As Decimal
        Get
            Return decDebit
        End Get
        Set(ByVal value As Decimal)
            decDebit = value
        End Set
    End Property
    Private decDebit As Decimal

    Public Property Credit() As Decimal
        Get
            Return decCredit
        End Get
        Set(ByVal value As Decimal)
            decCredit = value
        End Set
    End Property
    Private decCredit As Decimal

    Public Property DebitDiferencial() As Decimal
        Get
            Return decDebitDiferencial
        End Get
        Set(ByVal value As Decimal)
            decDebitDiferencial = value
        End Set
    End Property
    Private decDebitDiferencial As Decimal

    Public Property CreditDiferencial() As Decimal
        Get
            Return decCreditDiferencial
        End Get
        Set(ByVal value As Decimal)
            decCreditDiferencial = value
        End Set
    End Property
    Private decCreditDiferencial As Decimal

    Public Property CostingCode() As String
        Get
            Return strCostingCode
        End Get
        Set(ByVal value As String)
            strCostingCode = value
        End Set
    End Property
    Private strCostingCode As String

    Public Property CostingCode2() As String
        Get
            Return strCostingCode2
        End Get
        Set(ByVal value As String)
            strCostingCode2 = value
        End Set
    End Property
    Private strCostingCode2 As String


    Public Property CostingCode3() As String
        Get
            Return strCostingCode3
        End Get
        Set(ByVal value As String)
            strCostingCode3 = value
        End Set
    End Property
    Private strCostingCode3 As String

    Public Property CostingCode4() As String
        Get
            Return strCostingCode4
        End Get
        Set(ByVal value As String)
            strCostingCode4 = value
        End Set
    End Property
    Private strCostingCode4 As String

    Public Property CostingCode5() As String
        Get
            Return strCostingCode5
        End Get
        Set(ByVal value As String)
            strCostingCode5 = value
        End Set
    End Property
    Private strCostingCode5 As String

    Public Property CodProyecto() As String
        Get
            Return strCodProyecto
        End Get
        Set(ByVal value As String)
            strCodProyecto = value
        End Set
    End Property
    Private strCodProyecto As String

    Public Property AplicadoCargaDimensiones() As Boolean
        Get
            Return blnAplicadoCargaDimensiones
        End Get
        Set(ByVal value As Boolean)
            blnAplicadoCargaDimensiones = value
        End Set
    End Property
    Private blnAplicadoCargaDimensiones As Boolean

    Public Property Aplicado() As Boolean
        Get
            Return blnAplicado
        End Get
        Set(ByVal value As Boolean)
            blnAplicado = value
        End Set
    End Property
    Private blnAplicado As Boolean

    Public Property ImpNeg() As String
        Get
            Return strImpNeg
        End Get
        Set(ByVal value As String)
            strImpNeg = value
        End Set
    End Property
    Private strImpNeg As String

End Class


Public Class ListaLineasDocumento

    Public Property ItemCode() As String
        Get
            Return strItemCode
        End Get
        Set(ByVal value As String)
            strItemCode = value
        End Set
    End Property
    Private strItemCode As String

    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property IdRepxOrd() As String
        Get
            Return strIdRepxOrd
        End Get
        Set(ByVal value As String)
            strIdRepxOrd = value
        End Set
    End Property
    Private strIdRepxOrd As String

    Public Property ID() As String
        Get
            Return strID
        End Get
        Set(ByVal value As String)
            strID = value
        End Set
    End Property
    Private strID As String


    Public Property BaseEntry() As String
        Get
            Return strBaseEntry
        End Get
        Set(ByVal value As String)
            strBaseEntry = value
        End Set
    End Property
    Private strBaseEntry As String

    Public Property CentroCosto() As String
        Get
            Return strCentroCosto
        End Get
        Set(ByVal value As String)
            strCentroCosto = value
        End Set
    End Property
    Private strCentroCosto As String

    Public Property LineTotalFactura() As Decimal
        Get
            Return decLineTotalFactura
        End Get
        Set(ByVal value As Decimal)
            decLineTotalFactura = value
        End Set
    End Property
    Private decLineTotalFactura As Decimal

    Public Property LineTotalEntrada() As Decimal
        Get
            Return decLineTotalEntrada
        End Get
        Set(ByVal value As Decimal)
            decLineTotalEntrada = value
        End Set
    End Property
    Private decLineTotalEntrada As Decimal

    Public Property Debit() As Decimal
        Get
            Return decDebit
        End Get
        Set(ByVal value As Decimal)
            decDebit = value
        End Set
    End Property
    Private decDebit As Decimal

    Public Property Credit() As Decimal
        Get
            Return decCredit
        End Get
        Set(ByVal value As Decimal)
            decCredit = value
        End Set
    End Property
    Private decCredit As Decimal

    Public Property DebitAccount() As String
        Get
            Return strDebitAccount
        End Get
        Set(ByVal value As String)
            strDebitAccount = value
        End Set
    End Property
    Private strDebitAccount As String

    Public Property DebitAccountDiferencial() As String
        Get
            Return strDebitAccountDiferencial
        End Get
        Set(ByVal value As String)
            strDebitAccountDiferencial = value
        End Set
    End Property
    Private strDebitAccountDiferencial As String

    Public Property CreditAccount() As String
        Get
            Return strCreditAccount
        End Get
        Set(ByVal value As String)
            strCreditAccount = value
        End Set
    End Property
    Private strCreditAccount As String

    Public Property IdSucursal() As String
        Get
            Return strIdSucursal
        End Get
        Set(ByVal value As String)
            strIdSucursal = value
        End Set
    End Property
    Private strIdSucursal As String

    Public Property TipoOT() As String
        Get
            Return strTipoOT
        End Get
        Set(ByVal value As String)
            strTipoOT = value
        End Set
    End Property
    Private strTipoOT As String

    Public Property CodProyecto() As String
        Get
            Return strCodProyecto
        End Get
        Set(ByVal value As String)
            strCodProyecto = value
        End Set
    End Property
    Private strCodProyecto As String

    Public Property CodMarca() As String
        Get
            Return strCodMarca
        End Get
        Set(ByVal value As String)
            strCodMarca = value
        End Set
    End Property
    Private strCodMarca As String

    Public Property AlmacenProceso() As String
        Get
            Return strAlmacenProceso
        End Get
        Set(ByVal value As String)
            strAlmacenProceso = value
        End Set
    End Property
    Private strAlmacenProceso As String

    Public Property TipoArticulo() As String
        Get
            Return strTipoArticulo
        End Get
        Set(ByVal value As String)
            strTipoArticulo = value
        End Set
    End Property
    Private strTipoArticulo As String
    Public Property CostingCode() As String
        Get
            Return strCostingCode
        End Get
        Set(ByVal value As String)
            strCostingCode = value
        End Set
    End Property
    Private strCostingCode As String

    Public Property CostingCode2() As String
        Get
            Return strCostingCode2
        End Get
        Set(ByVal value As String)
            strCostingCode2 = value
        End Set
    End Property
    Private strCostingCode2 As String


    Public Property CostingCode3() As String
        Get
            Return strCostingCode3
        End Get
        Set(ByVal value As String)
            strCostingCode3 = value
        End Set
    End Property
    Private strCostingCode3 As String

    Public Property CostingCode4() As String
        Get
            Return strCostingCode4
        End Get
        Set(ByVal value As String)
            strCostingCode4 = value
        End Set
    End Property
    Private strCostingCode4 As String

    Public Property CostingCode5() As String
        Get
            Return strCostingCode5
        End Get
        Set(ByVal value As String)
            strCostingCode5 = value
        End Set
    End Property
    Private strCostingCode5 As String

    Public Property AplicadoCargaDimensiones() As Boolean
        Get
            Return blnAplicadoCargaDimensiones
        End Get
        Set(ByVal value As Boolean)
            blnAplicadoCargaDimensiones = value
        End Set
    End Property
    Private blnAplicadoCargaDimensiones As Boolean

    Public Property AplicaCosto() As Boolean
        Get
            Return blnAplicaCosto
        End Get
        Set(ByVal value As Boolean)
            blnAplicaCosto = value
        End Set
    End Property

    Private blnAplicaCosto As Boolean

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

Public Class ListaCuentasxAlmacen
    Public Property Almacen() As String
        Get
            Return strAlmacen
        End Get
        Set(ByVal value As String)
            strAlmacen = value
        End Set
    End Property
    Private strAlmacen As String

    Public Property DebitAccount() As String
        Get
            Return strDebitAccount
        End Get
        Set(ByVal value As String)
            strDebitAccount = value
        End Set
    End Property
    Private strDebitAccount As String


    Public Property CreditAccount() As String
        Get
            Return strCreditAccount
        End Get
        Set(ByVal value As String)
            strCreditAccount = value
        End Set
    End Property
    Private strCreditAccount As String


    Public Property DebitDiferencialAccount() As String
        Get
            Return strDebitDiferencialAccount
        End Get
        Set(ByVal value As String)
            strDebitDiferencialAccount = value
        End Set
    End Property
    Private strDebitDiferencialAccount As String
End Class

Public Class ListaNoOrdenxSucursal
    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property IdSucursal() As String
        Get
            Return strIdSucursal
        End Get
        Set(ByVal value As String)
            strIdSucursal = value
        End Set
    End Property
    Private strIdSucursal As String
End Class

Public Class ListaDimensionesxNoOrden
    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property CostingCode() As String
        Get
            Return strCostingCode
        End Get
        Set(ByVal value As String)
            strCostingCode = value
        End Set
    End Property
    Private strCostingCode As String

    Public Property CostingCode2() As String
        Get
            Return strCostingCode2
        End Get
        Set(ByVal value As String)
            strCostingCode2 = value
        End Set
    End Property
    Private strCostingCode2 As String


    Public Property CostingCode3() As String
        Get
            Return strCostingCode3
        End Get
        Set(ByVal value As String)
            strCostingCode3 = value
        End Set
    End Property
    Private strCostingCode3 As String

    Public Property CostingCode4() As String
        Get
            Return strCostingCode4
        End Get
        Set(ByVal value As String)
            strCostingCode4 = value
        End Set
    End Property
    Private strCostingCode4 As String

    Public Property CostingCode5() As String
        Get
            Return strCostingCode5
        End Get
        Set(ByVal value As String)
            strCostingCode5 = value
        End Set
    End Property
    Private strCostingCode5 As String
End Class
Public Class ListaProyectosxNoOrden
    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property CodProyecto() As String
        Get
            Return strCodProyecto
        End Get
        Set(ByVal value As String)
            strCodProyecto = value
        End Set
    End Property
    Private strCodProyecto As String
End Class

