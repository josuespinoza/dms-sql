Imports SCG.SBOFramework.DI
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports System.Globalization
Imports System.Collections.Generic
Imports SAPbouiCOM
Imports SAPbobsCOM
Imports SCG.SBOFramework
Imports SCG.UX.Windows
Imports System.Data.SqlClient
Imports DMSOneFramework.SCGDataAccess
Imports SCG.DMSOne.Framework


Public Class ReversarContratosCls

    Public Sub New(ByVal p_sbo As SAPbouiCOM.Application, ByVal p_objCompany As SAPbobsCOM.Company)

        SBO_Application = p_sbo
        m_oCompany = p_objCompany
        n = DIHelper.GetNumberFormatInfo(m_oCompany)
    End Sub

#Region "Declaraciones"

    Private SBO_Application As SAPbouiCOM.Application 'WithEvents SBO_Application As SAPbouiCOM.Application
    Private m_oCompany As SAPbobsCOM.Company
    Private m_objBLSBO As New BLSBO.GlobalFunctionsSBO

    Private otmpForm As SAPbouiCOM.Form
    Private oEdit As SAPbouiCOM.EditText
    Private oItem As SAPbouiCOM.Item
    Private oRefItem As SAPbouiCOM.Item
    Private oForm As SAPbouiCOM.Form
    Public n As NumberFormatInfo

    Private dataTableUsadosReversion As SAPbouiCOM.DataTable
    Private dtConsulta As SAPbouiCOM.DataTable

    Dim DocNumFactura As Long
    Dim DocNotaCreditoxDesc As Long
    Dim DocFactDeudores As Long
    Dim DocNotaDebito As Long
    Dim DocNotaCreditoUsd As Long
    Dim DocAsientoAjuste As Long
    Dim DocNotaDebitoUsd As Long
    Dim DocEntradaMercancia As Long
    Dim DocSalidaMercancia As Long
    Dim DocEntradaReversion As Long
    Dim DocAsientoAjusteCosto As Long
    Dim DocAsientoAjusteReversion As Long
    Dim DocFactAccs As Long
    Dim DocFactConsignados As Long
    Dim DocFactGastos As Long
    Dim DocFacturaTramites As Long


    Private intAsientoFinExt As Integer
    Private strAsientoReversaFinExt As String

    Private intAsientoTramite As Integer
    Private strAsientoReversaTramite As String

    Private strPrestamo As String
    Private strPagoRealizado As String
    Private strAsientoRevPrestamo As String
    Private strPrima As String
    Private strAsientoReversaPrima As String

    Dim intAsEntradaMercancia As Nullable(Of Integer)
    Dim intAsSalidaMercancia As Nullable(Of Integer)

    Private intAsientoBonos As Integer
    Private intAsientoPrimerCuotaSeguro As Integer
    Private intAsientoSalidaCostoProyectado As Integer
    Private intAsientoRConsignados As Integer
    Private strAsientoReversaBonos As String
    Private strAsientoReversaCuotaSeguro As String
    Private strAsientoReversaCostoProyectado As String
    Private strAsientoReversaConsignados As String
    Private intAsientoComisiones As Integer
    Private intAsientoOtrosCostos As Integer
    Private intAsientoTramitesFacturables As Integer
    Private intNoFPU As Integer
    Private intAsientoAdicionalFPU As Integer
    Private strAsientoReversaAdicionalFPU As String

    Private strAsientoReversaComisiones As String
    Private strAsientoReversaOtrosCostos As String
    Private strAsientoReversaTramitesFacturables As String
    Private strAsientoAdicionalFPVU As String

    'Private intAsEntMercancia As String

    Private ReferenciaAsientoMemo As String

    Private m_strMonedaLocal As String
    Private m_decTipoCambio As Decimal


    Private m_oInvoice As SAPbobsCOM.Documents
    Private m_oJournalEntries As SAPbobsCOM.JournalEntries
    Private m_oCreditMemo As SAPbobsCOM.Documents

    Private intGroupNum As Integer

    Private blnAsientoEntradaMercancia As Boolean = False
    Private blnProvieneEntradaMercancia As Boolean = False
    Private blnReversaDatosTrazabilidad As Boolean = True

    'Reversar Vehiculos
    Private dtsReversarContratos As New ReversarContratoDataSet
    Private dtaReversarContratos As New ReversarContratoDataSetTableAdapters.SCG_VEHICULOTableAdapter
    'Private dtsVehiculo As New DMS_Addon.ReversarContratoDataSet
    'Private dtaVehiculo As New DMS_Addon.ReversarContratoDataSetTableAdapters.SCG_VEHICULO_FechasTableAdapter
    Private drwReversar As ReversarContratoDataSet.SCG_VEHICULORow

    Private drwTrazabilidad As ReversarContratoDataSet.SCG_VEHICULO_TRAZRow
    Private dtaReversarTraz As New ReversarContratoDataSetTableAdapters.SCG_VEHICULO_TRAZTableAdapter

    'Reversar Entradas Vehiculos
    Private dtsSalidaContable As New ReversarContratoDataSet
    Private dtaSalidaContable As New ReversarContratoDataSetTableAdapters.SalidasContablesTableAdapter
    Private dtsReversarEntradas As New ReversarContratoDataSet
    Private dtaReversarEntradas As New ReversarContratoDataSetTableAdapters.ReversarEntradasTableAdapter
    Private drwSalida As ReversarContratoDataSet.SalidasContables_Row

    Private strFechaDocumento As String
    Private dtFechaDocumento As String
    Private Valor As String = String.Empty

    Private m_IDVehiculo As String = String.Empty
    Private ListaVehiculos As Generic.IList(Of String) = New Generic.List(Of String)

    Private ListaItemsUnidades As New List(Of ItemUnidad)


    'Reversion de vehiculos recibidos
    Private ListaVehiculosUsados As Generic.IList(Of String) = New Generic.List(Of String)
    Private ListaItemsUnidadesUsados As New List(Of ItemUnidadUsado)

    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon
    Private m_cn_Coneccion As New SqlClient.SqlConnection


    Private Structure ItemsAsientoEntrada

        Dim strCuenta As String
        Dim decValorCredit As Decimal
        Dim decValorDebit As Decimal
        Dim fechaDocDate As Date
        Dim decFvalorCredit As Decimal
        Dim decFvalorDebit As Decimal
        Dim FCurrency As String
        Dim Dimension1 As String
        Dim Dimension2 As String
        Dim Dimension3 As String
        Dim Dimension4 As String
        Dim Dimension5 As String

    End Structure

    Public Structure ItemUnidad
        Dim strUnidad As String
        Dim decCosto As Decimal
        Dim decCostoS As Decimal
    End Structure

    'Reversion Usados
    Public Structure ItemUnidadUsado
        Dim strUnidad As String
    End Structure

    'Cambios reversion de salida de mercancia
    Private m_objReversarSalidaMercancia As ReversarSalidaMercanciaCls

    Private dtsSalidaMercancia As New SalidaContableDataset
    Private dtaSalidaMercancia As New SalidaContableDatasetTableAdapters._SCGD_GOODISSUETableAdapter

    Private intDocEntrySalida As Integer = 0
    Private blnUsaDimensiones As Boolean = False

    Private m_blnDocumentoReversionNoCreado As Boolean = False

    Private strAsientoTramitesFacturables As String
    Private strAsientoReversionTramitesFacturables As String

    Private strFacturaTramitesFacturables As String
    Private strNotaCreditoTramitesFacturables As String

    Private strFacturaProveedorVehiculoUsado As String
    Private strNotaCreditoPFVehiculoUsado As String




#Region "Constantes"

    Private Const m_strTxtNofac As String = "txtNofac"
    Private Const m_strTxtNot_cre As String = "txtNot_cre"
    Private Const m_strTxtFac_Acr As String = "txtFac_Acr"
    Private Const m_strTxtNota_De As String = "txtNota_De"
    Private Const m_strTxtNot_us As String = "txtNot_us"
    Private Const m_strTxtAjus_Co As String = "txtAjus_Co"
    Private Const m_strTxtAs_CPC As String = "txtAs_CPC"
    Private Const m_strTxtEntrada As String = "txtEntrada"
    Private Const m_strTxtSalida As String = "txtSalida"
    Private Const m_strTxtPrestamo As String = "txtPrestam"
    Private Const m_strTxtNCPrima As String = "txtNCPri"
    Private Const m_strTxtFactAccs As String = "txtFactAcc"
    Private Const m_strTxtFactGastos As String = "txtFactGA"
    Private Const m_strTxtAsientoFinExt As String = "txtAsFinEx"
    Private Const m_strTxtAsientoTramite As String = "txtAsTram"
    Private Const m_strTxtAsientoBonos As String = "txtAsBon"
    Private Const m_strTxtAsientoComisiones As String = "txtAsCom"
    Private Const m_strTxtAsientoOtrosCostos As String = "txtAsOCos"
    Private Const m_strTxtFacturaTramites As String = "txtFaTram"
    Private Const m_strTxtAsientoTramitesFacturables As String = "txtAsTrFc"
    Private Const m_strTxtNoFPVU As String = "txtNoFPU"
    Private Const m_strTxtAsientoAdicionaFPU As String = "txtAsFPU"


    Private Const m_strTxtNumContrato As String = "txtNumCont"


    Private Const m_itFolder6 As String = "Folder6"

    Private Const mc_strTablaContratoVenta As String = "@SCGD_CVENTA"

    Private blnDocumentosFPVU As Boolean = False


#End Region

#Region "Nuevos valores"

    Private intNotaCreditoProvenienteFactura As Integer
    Private intNotaCreditoPorCmsConsignados As Integer
    Private intNotaDebitoPorVehiculoUsado As Integer
    Private intAsientoReversadoEntradaMercancia As Integer
    Private intAsientoReversado As Nullable(Of Integer)
    Private intNumeroContrato As Integer
    Private intNotaCreditoProvFactAccs As Integer
    Private intNotaCreditoProvFactGastos As Integer
    Private intNotaCredito_FacturaProveedorDeudaUsado As Integer
    Private intNotaCredito_FacturaClienteDeudaUsado As Integer
    Private intNotaDebitoxDescuento As Integer
    Private intNotaCredito_FacturaTramites As Integer

    Private oTmpSalvarDatosReversion As SAPbouiCOM.Form

    'Cambios reversion salidas contables
    Private intTempAsientoReversado As Integer

#End Region

#End Region


#Region "Crear nuevos documentos en SAP"

    Private Sub CancelarDraft(ByVal oForm As SAPbouiCOM.Form, ByVal strNumeroDraft As String)

        Dim oDraft As SAPbobsCOM.Documents

        Dim intError As Integer
        Dim strError As String = ""

        Try

            oDraft = m_oCompany.GetBusinessObject(BoObjectTypes.oDrafts)

            oDraft.GetByKey(CInt(strNumeroDraft))

            intError = oDraft.Cancel()
            If intError <> 0 Then
                m_oCompany.GetLastError(intError, strError)
                Throw New ExceptionsSBO(intError, strError)
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Public Sub ReversarDocumentosContratoVentas(ByRef p_form As SAPbouiCOM.Form, ByVal oDataTableFacturas As SAPbouiCOM.DataTable, ByVal oDataTableValFacts As SAPbouiCOM.DataTable)
        Try
            Dim intError As Integer
            Dim strDocCurrency As String
            Dim strNumeroSalida As String
            Dim strNumeroDraft As String
            Dim oMatrixUsado As SAPbouiCOM.Matrix
            Dim oMatrix As SAPbouiCOM.Matrix

            oMatrixUsado = DirectCast(p_form.Items.Item("mtx_Usado").Specific, SAPbouiCOM.Matrix)
            oMatrix = DirectCast(p_form.Items.Item("mtx_Vehi").Specific, SAPbouiCOM.Matrix)

            strDocCurrency = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_Moneda", 0).Trim
            intGroupNum = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_GroupNum", 0)

            Dim strUsaDimension As String = DMS_Connector.Configuracion.ParamGenAddon.U_UsaDimC.Trim()
            Dim strGenFacConsignados As String = DMS_Connector.Configuracion.ParamGenAddon.U_GenFacCns.Trim()
            Dim strGenAsCuotaSeguro As String = DMS_Connector.Configuracion.ParamGenAddon.U_GenAsSeg.Trim()


            If strUsaDimension = "Y" Then blnUsaDimensiones = True
            Call CargarTipoCambio(p_form)

            m_oCompany.StartTransaction()

            strNumeroSalida = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_SCGD_NoSalida", p_form.DataSources.DBDataSources.Item("@SCGD_CVENTA").Offset)
            strNumeroDraft = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_SCGD_DocPreliminar", 0).Trim()

            If String.IsNullOrEmpty(strNumeroSalida) Then If Not String.IsNullOrEmpty(strNumeroDraft) Then Call CancelarDraft(oForm, strNumeroDraft)

            strFechaDocumento = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_SCGD_FDr", 0).Trim()

            If Not String.IsNullOrEmpty(strFechaDocumento) Then

                If Not strDocCurrency = m_strMonedaLocal Then
                    Valor =
                        Utilitarios.EjecutarConsulta("Select Rate from ORTT with (nolock) where RateDate = '" & strFechaDocumento & "' and Currency = '" & strDocCurrency & "'",
                                                     m_oCompany.CompanyDB,
                                                     m_oCompany.Server)
                End If

                If Not String.IsNullOrEmpty(Valor) OrElse strDocCurrency = m_strMonedaLocal Then
                    dtFechaDocumento = Date.ParseExact(strFechaDocumento, "yyyyMMdd", Nothing)
                Else
                    SBO_Application.SetStatusBarMessage(String.Format(My.Resources.Resource.MensajeFechaSinTipoCambio, strDocCurrency), BoMessageTime.bmt_Medium, True)
                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    Exit Sub
                End If

            Else
                dtFechaDocumento = Date.Now
            End If

            oEdit = p_form.Items.Item(m_strTxtNumContrato).Specific
            intNumeroContrato = CInt(oEdit.String)

            'validación de factura de accesorios
            oEdit = p_form.Items.Item(m_strTxtFactAccs).Specific

            Dim blnValidacionFactAcc As Boolean = True
            Dim blnReversaFacturaAccesorios As Boolean = False

            If oEdit.Value <> String.Empty Then
                blnReversaFacturaAccesorios = True
                DocFactAccs = CLng(oEdit.String)

                Call ValidarFactura(blnValidacionFactAcc, False, DocFactAccs)
                If blnValidacionFactAcc = False Then
                    'm_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    Exit Sub
                End If
            End If

            'reversión factura
            oEdit = p_form.Items.Item(m_strTxtNofac).Specific
            If oEdit.Value <> String.Empty Then

                Dim StrConsultaFacts As String
                Dim blnFacturar As Boolean = True

                oDataTableFacturas.Rows.Clear()
                oDataTableFacturas = p_form.DataSources.DataTables.Item("Facturas")

                StrConsultaFacts = "Select U_SCGD_NoContrato, DocEntry, U_SCGD_Cod_Unidad From [OINV]  with (nolock) Where U_SCGD_NoContrato = '" & intNumeroContrato & "'"

                oDataTableFacturas.ExecuteQuery(StrConsultaFacts)

                If Not String.IsNullOrEmpty(oDataTableFacturas.GetValue("U_SCGD_NoContrato", 0)) Then


                    Call ValidarFactura(blnFacturar, True, 0, oDataTableFacturas, oDataTableValFacts, p_form)

                    If blnFacturar = True Then
                        For i As Integer = 0 To oDataTableFacturas.Rows.Count - 1
                            DocNumFactura = oDataTableFacturas.GetValue("DocEntry", i)
                            If Not DocNumFactura = DocFactAccs Then
                                intError = ReversarFactura(p_form, DocNumFactura, "VEH", oDataTableFacturas.GetValue("U_SCGD_Cod_Unidad", i))
                            Else
                                intError = ReversarFactura(p_form, DocNumFactura, "ACC", oDataTableFacturas.GetValue("U_SCGD_Cod_Unidad", i))
                            End If

                        Next
                    ElseIf blnFacturar = False Then
                        'm_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                        Exit Sub
                    End If
                End If

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If

            End If

            'Case m_strTxtFactAccs

            'factura de accesorios
            oEdit = p_form.Items.Item(m_strTxtFactAccs).Specific

            If oEdit.Value <> String.Empty Then
                If blnValidacionFactAcc = True And blnReversaFacturaAccesorios = True Then

                    intError = ReversarFactura(p_form, DocFactAccs, "ACC")

                    If m_blnDocumentoReversionNoCreado Then
                        Exit Try
                    End If

                End If
            End If


            'Reversión factura de consignados
            oEdit = p_form.Items.Item("txtFcCs").Specific

            If oEdit.Value <> String.Empty Then
                DocFactConsignados = CLng(oEdit.String)
                
                intError = ReversarFacturaComisionConsignado(p_form, DocFactConsignados)

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If

            End If


            'Case m_strTxtNot_cre

            'reversión notacredito
            oEdit = p_form.Items.Item(m_strTxtNot_cre).Specific
            If oEdit.Value <> String.Empty Then

                DocNotaCreditoxDesc = CLng(oEdit.String)

                intError = ReversarNotaCreditoxDescuento(p_form, DocNotaCreditoxDesc)

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If

            End If

            'Case m_strTxtFac_Acr

            'factura acreedora
            oEdit = p_form.Items.Item(m_strTxtFac_Acr).Specific
            If oEdit.Value <> String.Empty Then

                DocFactDeudores = CLng(oEdit.String)

                intError = ReversarFacturaAcredoraDeudaUsado(p_form, DocFactDeudores, "VEH")

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If

            End If

            'Case m_strTxtNota_De

            'nota debito
            oEdit = p_form.Items.Item(m_strTxtNota_De).Specific

            If oEdit.Value <> String.Empty Then
                DocNotaDebito = CLng(oEdit.String)
                SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeDocumentosNoSoportados, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
            End If

            'Case m_strTxtPrestamo

            'prestamo
            oEdit = p_form.Items.Item(m_strTxtPrestamo).Specific

            If oEdit.Value <> String.Empty Then

                strPrestamo = oEdit.String
                strPrestamo = strPrestamo.Trim()

                strPagoRealizado = Utilitarios.EjecutarConsulta("Select TOP 1 U_Numero from [@SCGD_PLAN_REAL] with (nolock) where DocEntry = '" & strPrestamo & "' And U_Pagado = 'Y'",
                                                                m_oCompany.CompanyDB, m_oCompany.Server)

                If String.IsNullOrEmpty(strPagoRealizado) Then
                    Call ReversarPrestamo(strPrestamo, p_form)

                    If m_blnDocumentoReversionNoCreado Then
                        Exit Try
                    End If

                ElseIf Not String.IsNullOrEmpty(strPagoRealizado) Then
                    m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorReversaPrest & strPrestamo & My.Resources.Resource.ExplicaErrorRevPrest,
                                                      SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                End If

            End If

            'Case m_strTxtNCPrima

            'nota credito prima
            oEdit = p_form.Items.Item(m_strTxtNCPrima).Specific

            If oEdit.Value <> String.Empty Then
                strPrima = oEdit.String
                strPrima = strPrima.Trim()
                intError = ReversarPagoPrima(strPrima, p_form)

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If
            End If

            'Case m_strTxtNot_us

            'nota usado
            oEdit = p_form.Items.Item(m_strTxtNot_us).Specific

            If oEdit.Value <> String.Empty Then

                DocNotaCreditoUsd = CLng(oEdit.String)

                Dim strCreaNCparaVehiculoUsado As String = DMS_Connector.Configuracion.ParamGenAddon.U_NCSalNeg.Trim
                Dim strNegativeAmount As String = Utilitarios.EjecutarConsulta(" Select NegAmount from OADM with (nolock) ", m_oCompany.CompanyDB, m_oCompany.Server)

                If strCreaNCparaVehiculoUsado = "Y" Then

                    If strNegativeAmount = "Y" Then
                        intError = CrearNCparaVehiculoUsado(p_form, DocNotaCreditoUsd)
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.NotaCredNegat)
                        intError = 1
                    End If
                Else
                    intError = CrearNotaDebito_Por_Usado(p_form, DocNotaCreditoUsd)

                    If m_blnDocumentoReversionNoCreado Then
                        Exit Try
                    End If
                End If

            End If

            'Case m_strTxtAjus_Co

            'ajuste de costo
            oEdit = p_form.Items.Item(m_strTxtAjus_Co).Specific

            If oEdit.Value <> String.Empty Then
                DocAsientoAjuste = CLng(oEdit.String)
                CrearDocumentoAsientoAjusteCostoReversion(DocAsientoAjuste)
                DocAsientoAjusteReversion = intAsientoReversado
                intAsientoReversado = Nothing

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If

            End If

            'Case m_strTxtAs_CPC

            'Nota debito deuda usado
            oEdit = p_form.Items.Item(m_strTxtAs_CPC).Specific

            If oEdit.Value <> String.Empty Then
                DocNotaDebitoUsd = CLng(oEdit.String)
                intError = ReversarFacturaClienteDeudaUsado(p_form, DocNotaDebitoUsd, "VEH")
                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If
            End If

            'Case m_strTxtEntrada

            'entradas usados
            If oMatrixUsado.RowCount > 0 Then

                Dim strContrato As String
                Dim strUnidadUsado As String = p_form.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Unid", 0).Trim
                Dim strEntradaMercancia As String = ""

                strContrato = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("DocEntry", 0).Trim

                If Not String.IsNullOrEmpty(strUnidadUsado) Then

                    For i As Integer = 0 To oMatrixUsado.RowCount - 1

                        strUnidadUsado = p_form.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Unid", i).Trim

                        strEntradaMercancia = Utilitarios.EjecutarConsulta("Select DocEntry from [@SCGD_GOODRECEIVE] with (nolock) where U_Unidad = '" & strUnidadUsado & "' and U_Num_Cont = '" & strContrato & "'", m_oCompany.CompanyDB, m_oCompany.Server).Trim()

                        If Not String.IsNullOrEmpty(strEntradaMercancia) Then

                            DocEntradaMercancia = CLng(strEntradaMercancia)

                            intAsEntradaMercancia = BuscarEntrada(DocEntradaMercancia)

                            If intAsEntradaMercancia Is Nothing OrElse intAsEntradaMercancia = -1 Then

                                Dim strFechaReversion As String = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_SCGD_FDr", 0)

                                Call ActualizarEstadoEntradaMercancia(DocEntradaMercancia, False, strFechaReversion)

                            Else
                                blnAsientoEntradaMercancia = True
                                blnProvieneEntradaMercancia = True

                                CrearDocumentoAsientoEntradaRevertido(intAsEntradaMercancia, DocEntradaMercancia)

                                If m_blnDocumentoReversionNoCreado Then
                                    Exit Try
                                End If

                            End If
                        End If
                    Next
                End If
            End If

            'Case m_strTxtSalida

            'asiento salida
            oEdit = p_form.Items.Item(m_strTxtSalida).Specific

            If oEdit.Value <> String.Empty Then
                DocSalidaMercancia = CLng(oEdit.String)
                Call ReversarSalidaMercancia(DocSalidaMercancia)

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If

            End If

            'Case m_strTxtFactGastos

            'factura de gastos
            oEdit = p_form.Items.Item(m_strTxtFactGastos).Specific

            If oEdit.Value <> String.Empty Then

                Dim blnFacturar As Boolean = True

                DocFactGastos = CLng(oEdit.String)

                Call ValidarFactura(blnFacturar, False, DocFactGastos)

                If blnFacturar = True Then
                    intError = ReversarFactura(p_form, DocFactGastos, "GAS")

                    If m_blnDocumentoReversionNoCreado Then
                        Exit Try
                    End If

                End If

            End If

            'Case m_strTxtAsientoFinExt

            'asiento financiamiento externos
            oEdit = p_form.Items.Item(m_strTxtAsientoFinExt).Specific

            If oEdit.Value <> String.Empty Then

                intAsientoFinExt = CInt(oEdit.String)
                Call ReversaAsiento(intAsientoFinExt, p_form, strAsientoReversaFinExt)

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If

            End If

            'Case m_strTxtAsientoTramite

            'asientos trámites
            oEdit = p_form.Items.Item(m_strTxtAsientoTramite).Specific

            If oEdit.Value <> String.Empty Then

                intAsientoTramite = CInt(oEdit.String)
                Call ReversaAsiento(intAsientoTramite, p_form, strAsientoReversaTramite)

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If

            End If

            'Case m_strTxtAsientoBonos

            'asiento bonos
            oEdit = p_form.Items.Item(m_strTxtAsientoBonos).Specific

            If oEdit.Value <> String.Empty Then

                intAsientoBonos = CInt(oEdit.String)
                Call ReversaAsiento(intAsientoBonos, p_form, strAsientoReversaBonos)

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If

            End If

            'Asiento primer cuota seguro
            oEdit = p_form.Items.Item("txtAsCS").Specific

            If oEdit.Value <> String.Empty Then

                intAsientoPrimerCuotaSeguro = CInt(oEdit.String)
                Call ReversaAsiento(intAsientoPrimerCuotaSeguro, p_form, strAsientoReversaCuotaSeguro)

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If
            End If

            ''Asiento salida costo proyectado vehiculo
            'oEdit = p_form.Items.Item("txtACS").Specific

            'If oEdit.Value <> String.Empty Then

            '    intAsientoSalidaCostoProyectado = CInt(oEdit.String)
            '    Call ReversaAsiento(intAsientoSalidaCostoProyectado, p_form, strAsientoReversaCostoProyectado)

            '    If m_blnDocumentoReversionNoCreado Then
            '        Exit Try
            '    End If
            'End If

            'Asiento regularizacion contable
            oEdit = p_form.Items.Item("txtAsReg").Specific

            If oEdit.Value <> String.Empty Then

                intAsientoRConsignados = CInt(oEdit.String)
                Call ReversaAsiento(intAsientoRConsignados, p_form, strAsientoReversaConsignados)

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If
            End If

            'Case m_strTxtAsientoComisiones

            'asiento comisiones
            oEdit = p_form.Items.Item(m_strTxtAsientoComisiones).Specific

            If oEdit.Value <> String.Empty Then

                intAsientoComisiones = CInt(oEdit.String)
                Call ReversaAsiento(intAsientoComisiones, p_form, strAsientoReversaComisiones)

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If

            End If

            'Case m_strTxtAsientoOtrosCostos

            'asiento otros costos
            oEdit = p_form.Items.Item(m_strTxtAsientoOtrosCostos).Specific

            If oEdit.Value <> String.Empty Then

                intAsientoOtrosCostos = CInt(oEdit.String)
                Call ReversaAsiento(intAsientoOtrosCostos, p_form, strAsientoReversaOtrosCostos)

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If

            End If

            'factura de Tramites
            oEdit = p_form.Items.Item(m_strTxtFacturaTramites).Specific

            If oEdit.Value <> String.Empty Then

                DocFacturaTramites = CInt(oEdit.String)
                'intError = ReversarFacturaTramites(p_form, DocFacturaTramites)
                intError = ReversarFactura(p_form, DocFacturaTramites, "TRA")

                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If


            End If

            'Asiento de Tramites Facturables
            oEdit = p_form.Items.Item(m_strTxtAsientoTramitesFacturables).Specific

            If oEdit.Value <> String.Empty Then

                intAsientoTramitesFacturables = CInt(oEdit.String)
                ReversaAsiento(intAsientoTramitesFacturables, p_form, strAsientoReversaTramitesFacturables)



                If m_blnDocumentoReversionNoCreado Then
                    Exit Try
                End If


            End If

            Dim strUsaDSNRU As String = DMS_Connector.Configuracion.ParamGenAddon.U_UsaFPVU.Trim
            Dim strUsaFPVU As String = DMS_Connector.Configuracion.ParamGenAddon.U_UsaDSNRU.Trim

            If Not String.IsNullOrEmpty(strUsaFPVU) Then

                If strUsaFPVU.Trim = "Y" Then

                    If oMatrixUsado.RowCount > 0 Then
                        For i As Integer = 0 To oMatrixUsado.RowCount - 1
                            Dim strFPVU As String = p_form.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_N_FP", i).Trim()
                            Dim strAsAd As String = p_form.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_N_AsAd", i).Trim()

                            If Not String.IsNullOrEmpty(strFPVU) Then

                                If Not String.IsNullOrEmpty(strUsaDSNRU) Then

                                    strFacturaProveedorVehiculoUsado = strFPVU

                                    If strUsaDSNRU = "Y" Then
                                        ReversarFacturaAcredoraDeudaUsado(p_form, strFPVU, "VEH", True, True, strNotaCreditoPFVehiculoUsado)
                                    Else
                                        ReversarFacturaAcredoraDeudaUsado(p_form, strFPVU, "VEH")
                                    End If
                                Else
                                    ReversarFacturaAcredoraDeudaUsado(p_form, strFPVU, "VEH", True, False, strNotaCreditoPFVehiculoUsado)
                                End If

                            End If

                            If Not String.IsNullOrEmpty(strAsAd) Then
                                strAsientoAdicionalFPVU = strAsAd

                                ReversaAsiento(strAsAd, p_form, strAsientoReversaAdicionalFPU)

                            End If

                        Next
                    End If

                End If

            End If

            If intError = 0 Then

                If m_oCompany.InTransaction Then

                    'aqui va lo concerniente a la reversion del vehiculo vendido....

                    Dim NumContrato As Integer
                    Dim strFechaReversion As String
                    Dim dtFechaFechaReversion As Date = Nothing

                    Dim ListUsadosConSalidasOContratos As List(Of ItemUnidadUsado)
                    Dim UnidadUsado As ItemUnidadUsado
                    'arreglo de items unidades

                    strFechaReversion = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_SCGD_FDr", 0)
                    strFechaReversion = strFechaReversion.Trim()

                    If Not String.IsNullOrEmpty(strFechaReversion) Then
                        dtFechaFechaReversion = Date.ParseExact(strFechaReversion, "yyyyMMdd", Nothing)
                        dtFechaFechaReversion = New Date(dtFechaFechaReversion.Year, dtFechaFechaReversion.Month, dtFechaFechaReversion.Day, 0, 0, 0)
                    End If

                    NumContrato = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("DocNum", 0)

                    ListaItemsUnidades = New List(Of ItemUnidad)
                    'Reversion usados
                    ListaItemsUnidadesUsados = New List(Of ItemUnidadUsado)


                    CargaUnidades(p_form, ListaItemsUnidades, ListaItemsUnidadesUsados)

                    'Se genera una lista con los vehiculos usados que poseen una salida de mercancia, o que se encuentran en un contrato de ventas como unidad de venta
                    ListUsadosConSalidasOContratos = New List(Of ItemUnidadUsado)
                    'Se incorpora ValidarSalidasyContratosVehiculoUsado
                    If Not IsNothing(ListaItemsUnidadesUsados) Then

                        UnidadUsado = New ItemUnidadUsado()

                        If ListaItemsUnidadesUsados.Count > 0 Then
                            For contador As Integer = 0 To ListaItemsUnidadesUsados.Count - 1
                                If ValidarSalidasyContratosVehiculoUsado(ListaItemsUnidadesUsados.Item(contador).strUnidad, NumContrato) = True Then
                                    UnidadUsado.strUnidad = ListaItemsUnidadesUsados.Item(contador).strUnidad
                                    ListUsadosConSalidasOContratos.Add(UnidadUsado)
                                End If
                            Next
                        End If
                    End If

                    Call Me.ReversarEntradasVehiculo(NumContrato, dtFechaFechaReversion)

                    ReversarVehiculoUDO(p_form, ListaItemsUnidades, ListUsadosConSalidasOContratos)

                    GuardarDatosContratoReversado(p_form, oMatrixUsado, oMatrix, oDataTableFacturas)
                    ActualizarTipoCosteo()
                    'Ojo solo para prueba se comenta el commit y se soloca el rollback
                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)

                    Dim strSalida As String
                    strSalida = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_SCGD_NoSalida", 0)

                    If Not String.IsNullOrEmpty(strSalida) Then
                        Call Utilitarios.EnviarMensajeMovimientoAccs(m_oCompany, SBO_Application, oForm.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_Sucu", 0).ToString())
                    End If

                    SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeContratoRevertido, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)

                    p_form.Close()
                Else
                    'm_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If
            Else
                m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
                m_blnDocumentoReversionNoCreado = False
            End If


        Catch ex As Exception

            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)

            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                ListaVehiculos.Clear()
                Call Utilitarios.ManejadorErrores(ex, SBO_Application)
                m_blnDocumentoReversionNoCreado = False
            End If

        Finally

            If m_blnDocumentoReversionNoCreado Then
                If m_oCompany.InTransaction Then
                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    ListaVehiculos.Clear()
                End If
            End If

            m_blnDocumentoReversionNoCreado = False

            blnDocumentosFPVU = False
        End Try

    End Sub

    Private Sub EjecutaConsultaValidacion(ByRef blnFacturar As Boolean, ByRef strFactura As String, ByVal strConsulta As String, _
                                          ByVal oDataTableValidaFacturas As SAPbouiCOM.DataTable, ByVal p_form As SAPbouiCOM.Form, ByVal blnValExiste As Boolean)

        Dim strFact As String
        Dim intFact As Integer = 0
        Dim strEstado As String = ""
        Dim strValor As String

        Try

            oDataTableValidaFacturas.Rows.Clear()
            oDataTableValidaFacturas = p_form.DataSources.DataTables.Item("ValFact")

            oDataTableValidaFacturas.ExecuteQuery(strConsulta)

            If oDataTableValidaFacturas.Rows.Count > 0 Then

                For i As Integer = 0 To oDataTableValidaFacturas.Rows.Count - 1

                    strFact = oDataTableValidaFacturas.GetValue("Factura", i)

                    If Not String.IsNullOrEmpty(strFact) Then

                        intFact = Integer.Parse(strFact)

                        If intFact > 0 Then

                            strValor = oDataTableValidaFacturas.GetValue("Valor", i)

                            If blnValExiste = True AndAlso Not String.IsNullOrEmpty(strValor) AndAlso Not strValor = "0" Then

                                strFactura = oDataTableValidaFacturas.GetValue("Factura", i)

                                blnFacturar = False

                                Exit Sub

                            ElseIf blnValExiste = False Then

                                strEstado = oDataTableValidaFacturas.GetValue("Valor", i)

                                If strEstado = "C" Then

                                    strFactura = oDataTableValidaFacturas.GetValue("Factura", i)

                                    blnFacturar = False

                                    Exit Sub

                                End If

                            End If

                        End If

                    End If

                Next

            End If

        Catch ex As Exception

            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            End If

        End Try

    End Sub

    Private Sub ValidarFactura(ByRef blnFacturar As Boolean, ByVal blnVeh As Boolean, Optional ByVal p_DocNumFactura As Long = 0, Optional ByVal oDataTableFacturas As SAPbouiCOM.DataTable = Nothing, _
                               Optional ByVal oDataTableValidaFacturas As SAPbouiCOM.DataTable = Nothing, Optional ByVal p_form As SAPbouiCOM.Form = Nothing)

        Dim baseDatos As String
        baseDatos = SBO_Application.Company.DatabaseName
        Dim Server As String
        Server = SBO_Application.Company.ServerName
        Dim strNumPago As String = ""
        Dim StrConsulta As String
        Dim strDocEntryFact As String
        Dim strFacturas As String = ""
        Dim strFacturaMsj As String = ""

        Try

            If blnVeh = True Then

                For i As Integer = 0 To oDataTableFacturas.Rows.Count - 1

                    strDocEntryFact = oDataTableFacturas.GetValue("DocEntry", i)

                    If i = 0 Then

                        strFacturas = strDocEntryFact

                    Else

                        strFacturas = strFacturas & "," & strDocEntryFact

                    End If

                Next

                If Not String.IsNullOrEmpty(strFacturas) Then

                    'Pagos asociados a la factura

                    StrConsulta = "SELECT OINV.DocEntry AS Factura, RCT2.DocNum AS Valor " & _
                                    "FROM INV1 WITH (NOLOCK) INNER JOIN OINV WITH (NOLOCK) " & _
                                    "ON INV1.DocEntry = OINV.DocEntry " & _
                                    "INNER JOIN RCT2 WITH (NOLOCK) ON INV1.DocEntry = RCT2.DocEntry " & _
                                    "INNER JOIN ORCT WITH (NOLOCK) ON RCT2.DocNum = ORCT.DocEntry " & _
                                    "WHERE (ORCT.Canceled = 'N') AND RCT2.InvType = 13 AND INV1.DocEntry IN(" & strFacturas & ") AND OINV.DocEntry IN(" & strFacturas & ")"

                    Call EjecutaConsultaValidacion(blnFacturar, strFacturaMsj, StrConsulta, oDataTableValidaFacturas, p_form, True)

                    If blnFacturar = False Then

                        SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeFacturaReversion & " " & strFacturaMsj & " " & My.Resources.Resource.MensajeFacturaReversionUltimoPago, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)

                        Exit Sub

                    End If

                    'Documentos que provienen de la factura

                    StrConsulta = "SELECT OINV.DocEntry AS Factura, INV1.TrgetEntry AS Valor " & _
                                    "FROM OINV WITH (NOLOCK) INNER JOIN " & _
                                    "INV1 WITH (NOLOCK) ON OINV.DocEntry = INV1.DocEntry " & _
                                    "WHERE OINV.DocEntry IN(" & strFacturas & ") AND OINV.DocType = 'I'"

                    Call EjecutaConsultaValidacion(blnFacturar, strFacturaMsj, StrConsulta, oDataTableValidaFacturas, p_form, True)

                    If blnFacturar = False Then

                        SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeFacturaReversion & " " & strFacturaMsj & " " & My.Resources.Resource.MensajeFacturaReversionUltimoNotaCredito, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)

                        Exit Sub

                    End If

                    'Estado de la factura

                    StrConsulta = "Select DocEntry AS Factura, DocStatus AS Valor " & _
                                    "FROM OINV WITH (NOLOCK) " & _
                                    "WHERE DocEntry IN(" & strFacturas & ") AND DocType = 'I'"

                    Call EjecutaConsultaValidacion(blnFacturar, strFacturaMsj, StrConsulta, oDataTableValidaFacturas, p_form, False)

                    If blnFacturar = False Then

                        SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeFacturaReversion & " " & strFacturaMsj & " " & My.Resources.Resource.MensajeFacturaReversionUltimoCerrada, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)

                        Exit Sub

                    End If

                End If

            ElseIf blnVeh = False Then

                'Pagos asociados a la factura

                StrConsulta = "SELECT RCT2.DocNum AS DeLineasPago " & _
                                            "FROM INV1 WITH (NOLOCK) INNER JOIN " & _
                                            "OINV WITH (NOLOCK) ON INV1.DocEntry = " & p_DocNumFactura & " AND OINV.DocEntry = " & p_DocNumFactura & _
                                            " INNER JOIN " & _
                                            "RCT2 WITH (NOLOCK) ON INV1.DocEntry = RCT2.DocEntry INNER JOIN " & _
                                            "ORCT WITH (NOLOCK) ON RCT2.DocNum = ORCT.DocEntry " & _
                                            "WHERE (ORCT.Canceled = 'N')"

                strNumPago = Utilitarios.EjecutarConsulta(StrConsulta, baseDatos, Server)

                If Not strNumPago = String.Empty Then

                    SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeFacturaReversion & " " & p_DocNumFactura & " " & My.Resources.Resource.MensajeFacturaReversionUltimoPago, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)

                    blnFacturar = False

                    Exit Sub

                End If

                'Documentos que provienen de la factura

                StrConsulta = "SELECT INV1.TrgetEntry " & _
                                "FROM OINV WITH (NOLOCK) INNER JOIN " & _
                                "INV1 WITH (NOLOCK) ON OINV.DocEntry = INV1.DocEntry " & _
                                "WHERE (OINV.DocEntry = " & p_DocNumFactura & ") AND (OINV.DocType = 'I')"

                Dim strValorConsulta As String = Utilitarios.EjecutarConsulta(StrConsulta, baseDatos, Server)

                If Not String.IsNullOrEmpty(strValorConsulta) Then

                    SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeFacturaReversion & " " & p_DocNumFactura & " " & My.Resources.Resource.MensajeFacturaReversionUltimoNotaCredito, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)

                    blnFacturar = False

                    Exit Sub

                End If

                'Estado de la factura

                StrConsulta = String.Empty


                StrConsulta = "Select DocStatus " & _
                                  "FROM OINV WITH (NOLOCK) " & _
                                  "WHERE (DocEntry = " & p_DocNumFactura & ") AND (DocType = 'I') "

                Dim strDocStatus As String = Utilitarios.EjecutarConsulta(StrConsulta, baseDatos, Server)

                If strDocStatus = "C" Then

                    SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeFacturaReversion & " " & p_DocNumFactura & " " & My.Resources.Resource.MensajeFacturaReversionUltimoCerrada, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)

                    blnFacturar = False

                End If

            End If

        Catch ex As Exception

            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            End If

        End Try

    End Sub

    'Agregado 22/09/2011: Reversar préstamo y asiento generado

    Private Sub ReversarPrestamo(ByVal strPrestamo As String, ByVal oForm As SAPbouiCOM.Form)

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Dim strDescEstado As String
        Dim strAsiento As String
        Dim intAsiento As Integer

        Try

            strDescEstado = Utilitarios.EjecutarConsulta("Select Name from [@SCGD_EST_PREST] where Code = '3'", m_oCompany.CompanyDB, m_oCompany.Server)

            strAsiento = Utilitarios.EjecutarConsulta("Select U_Asiento from [@SCGD_PRESTAMO] where DocEntry = '" & strPrestamo & "'", m_oCompany.CompanyDB, m_oCompany.Server)

            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_Prestamo")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", strPrestamo)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            oGeneralData.SetProperty("U_Estado", "3")
            oGeneralData.SetProperty("U_Des_Est", strDescEstado)
            oGeneralService.Update(oGeneralData)

            'Erick Sanabria. Para que en caso de que no genere asiento por el prestamo no reverse. 21.03.2014 
            Dim strPrestamoGeneraAsiento As String = Utilitarios.EjecutarConsulta("Select U_Gen_As from [@SCGD_CONF_FINANC]", m_oCompany.CompanyDB, m_oCompany.Server)

            If (strPrestamoGeneraAsiento <> "N") Then

                If Not String.IsNullOrEmpty(strAsiento) Then

                    intAsiento = Integer.Parse(strAsiento)

                    Call ReversaAsiento(intAsiento, oForm, strAsientoRevPrestamo)

                End If
            End If


        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Sub

    Private Sub ReversaAsiento(ByVal intAsientoReversar As Integer, ByVal oForm As SAPbouiCOM.Form, ByRef strAsientoGenerado As String)

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

            strFechaReversion = oForm.DataSources.DBDataSources.Item("@SCGD_CVENTA").GetValue("U_SCGD_FDr", 0)
            strFechaReversion = strFechaReversion.Trim()

            If Not String.IsNullOrEmpty(strFechaReversion) Then
                dtFechaFechaReversion = Date.ParseExact(strFechaReversion, "yyyyMMdd", Nothing)
                dtFechaFechaReversion = New Date(dtFechaFechaReversion.Year, dtFechaFechaReversion.Month, dtFechaFechaReversion.Day, 0, 0, 0)

                oJournalEntry.ReferenceDate = dtFechaFechaReversion
            End If
            oJournalEntry.Reference2 = intNumeroContrato
            oJournalEntry.Memo = My.Resources.Resource.AsientoReversaCont & intNumeroContrato

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
                    oJournalEntry.Lines.Reference2 = intNumeroContrato
                    If blnUsaDimensiones Then

                        oJournalEntry.Lines.CostingCode = .CostingCode
                        oJournalEntry.Lines.CostingCode2 = .CostingCode2
                        oJournalEntry.Lines.CostingCode3 = .CostingCode3
                        oJournalEntry.Lines.CostingCode4 = .CostingCode4
                        oJournalEntry.Lines.CostingCode5 = .CostingCode5

                    End If


                    oJournalEntry.Lines.Add()

                End With

            Next

            intVerificar = oJournalEntry.Add()
            If intVerificar <> 0 Then
                m_oCompany.GetLastError(intError, strMensajeError)
                Throw New ExceptionsSBO(intVerificar, strMensajeError)

                m_blnDocumentoReversionNoCreado = True
            Else
                strAsientoGenerado = m_oCompany.GetNewObjectKey
            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

            m_blnDocumentoReversionNoCreado = True

        End Try

    End Sub

    Private Function ReversarPagoPrima(ByVal strPrima As String, ByVal oForm As SAPbouiCOM.Form) As Integer

        Dim oPagoRecibido As SAPbobsCOM.Payments

        Dim oJournalEntry As SAPbobsCOM.JournalEntries

        Dim intError As Integer
        Dim strMensajeError As String = ""
        Dim intVerificar As Integer

        Dim strFechaReversion As String
        Dim dtFechaFechaReversion As Date
        Dim NumContrato As String

        Try

            oPagoRecibido = CargarPagoRecibido(CInt(strPrima))

            oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            If Not oPagoRecibido Is Nothing Then

                m_strMonedaLocal = m_objBLSBO.RetornarMonedaLocal()

                strFechaReversion = oForm.DataSources.DBDataSources.Item("@SCGD_CVENTA").GetValue("U_SCGD_FDr", 0)
                strFechaReversion = strFechaReversion.Trim()
                If Not String.IsNullOrEmpty(strFechaReversion) Then
                    dtFechaFechaReversion = Date.ParseExact(strFechaReversion, "yyyyMMdd", Nothing)
                    dtFechaFechaReversion = New Date(dtFechaFechaReversion.Year, dtFechaFechaReversion.Month, dtFechaFechaReversion.Day, 0, 0, 0)
                    oJournalEntry.ReferenceDate = dtFechaFechaReversion
                End If
                NumContrato = oForm.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("DocNum", 0)
                NumContrato = NumContrato.TrimEnd(" ")
                oJournalEntry.Memo = String.Format(My.Resources.Resource.ComentarioPagoRecibido, oPagoRecibido.DocNum, NumContrato)

                oJournalEntry.Lines.ShortName = oPagoRecibido.CardCode
                If oPagoRecibido.DocCurrency = m_strMonedaLocal Then
                    oJournalEntry.Lines.Debit = oPagoRecibido.CashSum
                Else
                    oJournalEntry.Lines.FCDebit = oPagoRecibido.CashSumFC
                    oJournalEntry.Lines.FCCurrency = oPagoRecibido.DocCurrency
                End If
                oJournalEntry.Lines.Reference1 = My.Resources.Resource.PagoRecibido & oPagoRecibido.DocNum
                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.Add()

                oJournalEntry.Lines.AccountCode = oPagoRecibido.CashAccount
                If oPagoRecibido.DocCurrency = m_strMonedaLocal Then
                    oJournalEntry.Lines.Credit = oPagoRecibido.CashSum
                Else
                    oJournalEntry.Lines.FCCredit = oPagoRecibido.CashSumFC
                    oJournalEntry.Lines.FCCurrency = oPagoRecibido.DocCurrency
                End If
                oJournalEntry.Lines.Reference1 = My.Resources.Resource.PagoRecibido & oPagoRecibido.DocNum
                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.Add()

                If oJournalEntry.Add <> 0 Then

                    m_oCompany.GetLastError(intError, strMensajeError)
                    Throw New ExceptionsSBO(intVerificar, strMensajeError)

                    m_blnDocumentoReversionNoCreado = True

                Else

                    strAsientoReversaPrima = m_oCompany.GetNewObjectKey
                    Return 0

                End If

            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

            m_blnDocumentoReversionNoCreado = True

        End Try

    End Function

    Private Function CargarPagoRecibido(ByVal p_intPagoRecibido As Integer)

        Dim oPagoRecibido As SAPbobsCOM.Payments = Nothing

        Try

            oPagoRecibido = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments)

            If oPagoRecibido.GetByKey(p_intPagoRecibido) Then

                Return oPagoRecibido

            End If

        Catch ex As Exception

            Throw ex

        End Try

        Return Nothing

    End Function

    'Agregado 09/11/2010: Reversa salida de mercancia creando una entrada de mercancia
    Public Sub ReversarSalidaMercancia(ByVal p_docNumSalida As Long)

        Dim oSalidaMercancia As Documents
        Dim oSalidaLineas As Document_Lines
        Dim oEntradaMercancia As Documents
        Dim strComentario As String
        Dim strPrecio As String

        Dim intError As Integer
        Dim strError As String = String.Empty

        Try

            oEntradaMercancia = m_oCompany.GetBusinessObject(BoObjectTypes.oDrafts)
            oEntradaMercancia.DocObjectCode = BoObjectTypes.oInventoryGenEntry

            oSalidaMercancia = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit), SAPbobsCOM.Documents)

            If oSalidaMercancia.GetByKey(p_docNumSalida) Then

                oSalidaLineas = oSalidaMercancia.Lines

                For i As Integer = 0 To oSalidaLineas.Count - 1

                    oSalidaLineas.SetCurrentLine(i)

                    With oEntradaMercancia

                        .Lines.ItemCode = oSalidaLineas.ItemCode
                        .Lines.ItemDescription = oSalidaLineas.ItemDescription
                        .Lines.Quantity = oSalidaLineas.Quantity
                        .Lines.AccountCode = oSalidaLineas.AccountCode
                        .Lines.WarehouseCode = oSalidaLineas.WarehouseCode
                        strPrecio = Utilitarios.EjecutarConsulta("Select StockPrice from IGE1 where DocEntry = '" + CStr(p_docNumSalida) + "' and ItemCode = '" + oSalidaLineas.ItemCode + "'", m_oCompany.CompanyDB, m_oCompany.Server)
                        .Lines.Price = CDbl(strPrecio)

                        oEntradaMercancia.Lines.Add()

                    End With

                Next i

                strComentario = My.Resources.Resource.ReferenciaCV + CStr(intNumeroContrato)

                oEntradaMercancia.DocDate = dtFechaDocumento 'Now.Date
                oEntradaMercancia.TaxDate = dtFechaDocumento 'Now.Date
                oEntradaMercancia.Comments = strComentario
                oEntradaMercancia.UserFields.Fields.Item("U_SCGD_NoContrato").Value = CStr(intNumeroContrato)

                intError = oEntradaMercancia.Add()


                If intError <> 0 Then
                    m_oCompany.GetLastError(intError, strError)
                    Throw New ExceptionsSBO(intError, strError)

                    m_blnDocumentoReversionNoCreado = True

                End If

                DocEntradaReversion = m_oCompany.GetNewObjectKey

                Utilitarios.m_strDocumentoMensaje = m_oCompany.GetNewObjectKey

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Public Sub ReversarVehiculoUDO(ByVal p_form As Form, ByVal itemsUnidades As List(Of ItemUnidad),
                                ByVal itemsUnidadesUsadosValidacion As List(Of ItemUnidadUsado))

        Dim strConectionString As String = ""
        Dim cnConeccionBD As SqlClient.SqlConnection
        Dim strIDVehiculo As String
        Dim strTipoVehiculo As String
        Dim strTipoVenta As String

        Dim strUnidad As String
        Dim oMatrix As SAPbouiCOM.Matrix

        Dim strEstadoDisponible As String
        Dim intEstadoDisponible As Integer
        '----------------------------------
        Dim dtDatosVenta As System.Data.DataTable
        Dim dtDatosVentaSDK As SAPbouiCOM.DataTable
        Dim strNumCV As String
        Dim dtFechaContrato As Date

        Dim ListaVehiculosUsados As List(Of String) = New List(Of String)

        Try
            strEstadoDisponible = Utilitarios.EjecutarConsulta("Select U_Disp_R from [@SCGD_ADMIN] with (nolock) where Code = 'DMS'", m_oCompany.CompanyDB, m_oCompany.Server)

            If Not String.IsNullOrEmpty(strEstadoDisponible) Then intEstadoDisponible = Integer.Parse(strEstadoDisponible)
            oMatrix = DirectCast(p_form.Items.Item("mtx_Vehi").Specific, SAPbouiCOM.Matrix)
            ListaVehiculos.Clear()
            For i As Integer = 0 To oMatrix.RowCount - 1

                strUnidad = p_form.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Cod_Unid", i).Trim()

                strIDVehiculo = Utilitarios.EjecutarConsulta("Select Code from [@SCGD_Vehiculo] with (nolock) where U_Cod_Unid = '" & strUnidad & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                ListaVehiculos.Add(strIDVehiculo)

            Next i

            strNumCV = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("DocNum", 0)

            dtDatosVentaSDK = ObtenerDatosVentaReversionUDO(p_form, ListaVehiculos, strNumCV)

            oMatrix = DirectCast(p_form.Items.Item("mtx_Usado").Specific, SAPbouiCOM.Matrix)
            For i As Integer = 0 To oMatrix.RowCount - 1

                strUnidad = p_form.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Unid", i).Trim()

                strIDVehiculo = Utilitarios.EjecutarConsulta("Select Code from [@SCGD_Vehiculo] with (nolock) where U_Cod_Unid = '" & strUnidad & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                ListaVehiculosUsados.Add(strIDVehiculo)

            Next i
            Call ManejoVehiculosRecibidosUDO(p_form, itemsUnidades, itemsUnidadesUsadosValidacion)

            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oChildTrazabilidad As SAPbobsCOM.GeneralData
            Dim oChildrenTrazabilidad As SAPbobsCOM.GeneralDataCollection

            Dim m_strTipoVenta As String = String.Empty

            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_VEH")
            For itm As Integer = 0 To ListaVehiculos.Count - 1

                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("Code", ListaVehiculos(itm))
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                m_strTipoVenta = oGeneralData.GetProperty("U_Tipo_Ven")

                If Not String.IsNullOrEmpty(m_strTipoVenta) Then
                    strTipoVenta = m_strTipoVenta
                Else
                    strTipoVenta = String.Empty
                End If

                If ListaVehiculos(itm) <> String.Empty Then
                    oGeneralData.SetProperty("U_CTOVTA", 0)
                    oGeneralData.SetProperty("U_NUMFAC", 0)

                    If Not String.IsNullOrEmpty(strTipoVenta) Then oGeneralData.SetProperty("U_Tipo", strTipoVenta)
                    oGeneralData.SetProperty("U_CardCode", "")
                    oGeneralData.SetProperty("U_CardName", "")
                    oGeneralData.SetProperty("U_VENRES", "")

                    oGeneralData.SetProperty("U_Dispo", intEstadoDisponible)
                    oGeneralData.SetProperty("U_FchRsva", "")
                    oGeneralData.SetProperty("U_FechaVen", "")

                    If blnReversaDatosTrazabilidad Then

                        oChildrenTrazabilidad = oGeneralData.Child("SCGD_VEHITRAZA")
                        dtFechaContrato = Date.ParseExact(p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_DocDate", 0).Trim, "yyyyMMdd", Nothing)


                        If oChildrenTrazabilidad.Count > 0 AndAlso dtDatosVentaSDK.Rows.Count > 0 Then
                            For cont As Integer = 0 To dtDatosVentaSDK.Rows.Count - 1

                                If ListaVehiculos(itm) = dtDatosVentaSDK.GetValue("Code", cont) Then
                                    oChildTrazabilidad = oChildrenTrazabilidad.Item(0)

                                    oChildTrazabilidad.SetProperty("U_NumCV_I", Convert.ToString(dtDatosVentaSDK.GetValue("NumCV", cont)))
                                    oChildTrazabilidad.SetProperty("U_FhaCV_I", dtDatosVentaSDK.GetValue("FhaCV", cont))
                                    oChildTrazabilidad.SetProperty("U_NumFac_V", Convert.ToString(dtDatosVentaSDK.GetValue("NumFact", cont)))
                                    oChildTrazabilidad.SetProperty("U_FhaFac_V", dtDatosVentaSDK.GetValue("FhaFact", cont))
                                    oChildTrazabilidad.SetProperty("U_TotCV_V", dtDatosVentaSDK.GetValue("U_Pre_Tot", cont))
                                    oChildTrazabilidad.SetProperty("U_CodVen_V", dtDatosVentaSDK.GetValue("Vendedor", cont))
                                    oChildTrazabilidad.SetProperty("U_FecEntCV", "")
                                    oChildTrazabilidad.SetProperty("U_Km_Venta", 0)
                                End If
                            Next
                        End If
                    End If
                End If
                oGeneralService.Update(oGeneralData)
            Next
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Private Sub CargaUnidades(ByVal p_form As Form, ByRef itemsUnidades As List(Of ItemUnidad), ByRef itemsUnidadUsado As List(Of ItemUnidadUsado))
        Dim oMatrix As Matrix
        Dim strUnidad As String
        Dim strCodeVeh As String
        Dim decCosto As Decimal
        Dim decCosto_S As Decimal
        Dim itemUnidad As ItemUnidad
        Dim strNumCV As String
        'Reversion Usados
        Dim itemUnidadUsado As ItemUnidadUsado

        Dim dtUnidad As System.Data.DataTable

        oMatrix = DirectCast(p_form.Items.Item("mtx_Usado").Specific, SAPbouiCOM.Matrix)

        strNumCV = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("DocNum", 0)
        strNumCV = strNumCV.Trim()

        For i As Integer = 0 To oMatrix.RowCount - 1

            itemUnidad = New ItemUnidad()

            itemUnidadUsado = New ItemUnidadUsado()


            strUnidad = p_form.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Unid", i)
            strUnidad = strUnidad.Trim()
            strCodeVeh = Utilitarios.EjecutarConsulta(String.Format("Select Code from dbo.[@SCGD_Vehiculo] where U_Cod_Unid = '{0}'", strUnidad), m_oCompany.CompanyDB, m_oCompany.Server)

            dtUnidad = Utilitarios.EjecutarConsultaDataTable(
                String.Format("select top(1) GR.U_GASTRA, GR.U_GASTRA_S from [@SCGD_GOODRECEIVE] GR  where GR.U_Unidad = '{0}' and GR.U_Num_Cont <> '{1}' and GR.Status = 'O'order by DocEntry desc",
                              strUnidad,
                              strNumCV),
                          m_oCompany.CompanyDB,
                          m_oCompany.Server)
            If dtUnidad.Rows.Count > 0 Then

                Dim cont As Integer = 0
                For Each loRow As DataRow In dtUnidad.Rows
                    decCosto = Decimal.Parse(dtUnidad.Rows(cont)("U_GASTRA").ToString())
                    decCosto_S = Decimal.Parse(dtUnidad.Rows(cont)("U_GASTRA_S").ToString())
                    cont += 1
                Next

            End If

            itemUnidad.strUnidad = strCodeVeh
            itemUnidad.decCosto = decCosto
            itemUnidad.decCostoS = decCosto_S

            itemsUnidades.Add(itemUnidad)

            'Solamente Usados
            itemUnidadUsado.strUnidad = strUnidad
            itemsUnidadUsado.Add(itemUnidadUsado)

        Next i
    End Sub

    Public Function ObtenerDatosVentaReversion(ByVal p_strIDVehiculo As String, ByVal p_strNumCVActual As String) As System.Data.DataTable
        Dim dtResult As System.Data.DataTable
        Dim strEstadoAprov As String

        Dim strSQLVenta As String = ""
        Dim srtSQLEstadoAprov As String = String.Empty

        srtSQLEstadoAprov = "SELECT TOP (1) U_Prio FROM [DBO].[@SCGD_ADMIN9] with (nolock) ORDER BY U_Prio DESC"
        strEstadoAprov = Utilitarios.EjecutarConsulta(srtSQLEstadoAprov, m_oCompany.CompanyDB, m_oCompany.Server)

        strSQLVenta = " SELECT top(1)" & _
                    " VEHI.CODE, CVENTA.DocEntry NumCV, CVENTA.U_DocDate FhaCV,  FACT.DocEntry NumFact,CVENTA.U_SCGD_FDc FhaFact, ISNULL(CVENTA.U_Slpname ,'')as Vendedor, ISNULL(VEXC.U_Pre_Tot,0) as U_Pre_Tot, CVenta.U_SCGD_FDc" & _
                    " from " & _
                    " [@SCGD_CVENTA] CVENTA with (nolock) " & _
                    " inner join [@SCGD_VEHIXCONT] VEXC with (nolock) ON VEXC.DocEntry = CVENTA.DocEntry" & _
                    " inner join [@SCGD_VEHICULO] VEHI with (nolock) ON VEHI.U_Cod_Unid = VEXC.U_Cod_Unid " & _
                    " inner join [OINV] FACT with (nolock) ON FACT.U_SCGD_NoContrato = CVENTA.DocEntry " & _
                    " WHERE	" & _
                    "       VEHI.Code = '{0}' AND " & _
                    "       CVENTA.U_Reversa = 'N' AND " & _
                    "       CVENTA.DocNum < {1} AND " & _
                    "       CVENTA.U_Estado = {2}" & _
                    " order by CVENTA.CreateDate desc, CVENTA.CreateTime desc"

        strSQLVenta = String.Format(strSQLVenta, p_strIDVehiculo, p_strNumCVActual, strEstadoAprov)

        dtResult = Utilitarios.EjecutarConsultaDataTable(strSQLVenta, m_oCompany.CompanyDB, m_oCompany.Server)

        If IsNothing(dtResult) OrElse dtResult.Rows.Count = 0 Then
            dtResult = Nothing
        End If
        Return dtResult

    End Function

    Public Function ObtenerDatosVentaReversionUDO(ByRef p_form As Form, ByVal p_strIDVehiculo As IList(Of String), ByVal p_strNumCVActual As String) As SAPbouiCOM.DataTable
        Dim strEstadoAprov As String
        Dim dt As SAPbouiCOM.DataTable

        Dim strSQLVenta As String = ""
        Dim srtSQLEstadoAprov As String = String.Empty
        Dim strListaIDVehiculos As String = String.Empty

        For itm As Integer = 0 To ListaVehiculos.Count - 1
            If itm > 0 Then strListaIDVehiculos = String.Format("{0}, ", strListaIDVehiculos)

            strListaIDVehiculos = String.Format("{0} {1}", strListaIDVehiculos, ListaVehiculos(itm))
        Next

        srtSQLEstadoAprov = "SELECT TOP (1) U_Prio FROM [@SCGD_ADMIN9] with (nolock) ORDER BY U_Prio DESC"
        strEstadoAprov = Utilitarios.EjecutarConsulta(srtSQLEstadoAprov, m_oCompany.CompanyDB, m_oCompany.Server)


        strSQLVenta = " SELECT " & _
                    " VEHI.CODE Code, CVENTA.DocEntry NumCV, CVENTA.U_DocDate FhaCV,  FACT.DocEntry NumFact,CVENTA.U_SCGD_FDc FhaFact, ISNULL(CVENTA.U_Slpname ,'')as Vendedor, ISNULL(VEXC.U_Pre_Tot,0) as U_Pre_Tot, CVenta.U_SCGD_FDc" & _
                    " from " & _
                    " [@SCGD_CVENTA] CVENTA with (nolock) " & _
                    " inner join [@SCGD_VEHIXCONT] VEXC with (nolock) ON VEXC.DocEntry = CVENTA.DocEntry" & _
                    " inner join [@SCGD_VEHICULO] VEHI with (nolock) ON VEHI.U_Cod_Unid = VEXC.U_Cod_Unid " & _
                    " inner join [OINV] FACT with (nolock) ON FACT.U_SCGD_NoContrato = CVENTA.DocEntry " & _
                    " WHERE	" & _
                    "       VEHI.Code = '{0}' AND " & _
                    "       CVENTA.U_Reversa = 'N' AND " & _
                    "       CVENTA.DocNum = {1} AND " & _
                    "       CVENTA.U_Estado = {2}" & _
                    " order by CVENTA.CreateDate desc, CVENTA.CreateTime desc"


        strSQLVenta = String.Format(strSQLVenta, ListaVehiculos(0), p_strNumCVActual, strEstadoAprov)

        dt = p_form.DataSources.DataTables.Item("dtInfoReversion")
        dt.ExecuteQuery(strSQLVenta)

        Return dt

    End Function


    Public Function ObtenerDatosIngresoReversion(ByVal p_strIDVehiculo As String, ByVal p_strNumCVActual As String)
        Dim dtResult As System.Data.DataTable
        Dim strSQL As String = String.Empty
        Dim srtSQLEstadoAprov As String = String.Empty
        Dim strEstadoAprov As String
        ' Dim strSQL2 As String

        srtSQLEstadoAprov = "SELECT TOP (1)  U_Prio FROM [DBO].[@SCGD_ADMIN9]ORDER BY U_Prio DESC"
        strEstadoAprov = Utilitarios.EjecutarConsulta(srtSQLEstadoAprov, m_oCompany.CompanyDB, m_oCompany.Server)

        strSQL = "SELECT TOP(1) " & _
                " vehi.DocEntry, CVENTA.U_Cod_N_Us NumFact, CVENTA.U_SCGD_FDc FhaFact, CVENTA.DocEntry NumCV, CVENTA.U_DocDate FhaCV, CVENTA.U_SlpName Vendedor, USXC.U_Val_Rec TotalRec, isnull(USXC.U_KmUs,0) KmIngreso" & _
        " FROM " & _
                " ([dbo].[@SCGD_CVENTA] CVENTA " & _
                        " INNER JOIN dbo.[@SCGD_USADOXCONT] USXC  ON USXC.DocEntry = CVENTA.DocEntry " & _
                                " AND USXC.U_Cod_Unid IS NOT NULL AND CVENTA.U_SCGD_FDc IS NOT NULL " & _
                        " INNER JOIN [dbo].[@SCGD_VEHICULO] VEHI	ON VEHI.U_Cod_Unid = USXC.U_Cod_Unid) " & _
        " WHERE " & _
            " (CVENTA.U_Reversa = 'N') AND" & _
            " (CVENTA.U_Estado = '{0}') AND" & _
            " (CVENTA.DocNum < '{1}')AND" & _
            " (VEHI.Code = '{2}')" & _
        " ORDER BY CVENTA.DocNum DESC"
        strSQL = String.Format(strSQL, strEstadoAprov, p_strNumCVActual, p_strIDVehiculo)

        dtResult = Utilitarios.EjecutarConsultaDataTable(strSQL, m_oCompany.CompanyDB, m_oCompany.Server)

        If IsNothing(dtResult) OrElse dtResult.Rows.Count = 0 Then
            dtResult = Nothing
        End If
        Return dtResult
    End Function

    Public Function ValidarSalidasyContratosVehiculoUsado(ByVal p_strCodUnidad As String, ByVal p_strNumCV As String) As Boolean
        Try


            Dim strSQLConsulta As String
            Dim l_dtDatos As System.Data.DataTable
            Dim l_result As Boolean = True

            strSQLConsulta = "SELECT " & _
            " GI.DocEntry" & _
            " FROM [dbo].[@SCGD_GOODISSUE] GI with (nolock) " & _
            " INNER JOIN [dbo].[@SCGD_GRLINES] LINES with (nolock) ON  GI.DocEntry = LINES.DocEntry " & _
            " WHERE U_Unidad = '{0}' AND  GI.U_Reversa = 'N' " & _
            " union " & _
            " Select CV.DocEntry " & _
            " FROM [dbo].[@SCGD_CVENTA] CV with (nolock) " & _
            " INNER JOIN [dbo].[@SCGD_VEHIXCONT] VXC with (nolock) ON VXC.DocEntry = CV.DocEntry " & _
            " INNER JOIN [dbo].[@SCGD_VEHICULO] VEHI with (nolock) ON VEHI.U_Cod_Unid = VXC.U_Cod_Unid " & _
            " WHERE VXC.U_Cod_Unid = '{0}' AND CV.U_Reversa = 'N' AND CV.DocNum <> '{1}' "

            strSQLConsulta = String.Format(strSQLConsulta, p_strCodUnidad, p_strNumCV)

            l_dtDatos = Utilitarios.EjecutarConsultaDataTable(strSQLConsulta, m_oCompany.CompanyDB, m_oCompany.Server)
            If l_dtDatos.Rows.Count = 0 OrElse IsNothing(l_dtDatos) Then
                l_result = False
            ElseIf l_dtDatos.Rows.Count > 0 Then
                l_result = True
            End If
            Return l_result


        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub ActualizarTipoCosteo()

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim strEntradaReferencia As String = String.Empty


        For itm As Integer = 0 To ListaVehiculos.Count - 1


            strEntradaReferencia = Utilitarios.EjecutarConsulta("Select DocEntry from dbo.[@SCGD_GOODRECEIVE] with (nolock) where U_ID_Vehiculo = '" & ListaVehiculos(itm) & "' AND (U_SCGD_Trasl = 'N') AND (U_As_Entr <> - 1) ", m_oCompany.CompanyDB, m_oCompany.Server)
            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_VEH")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("Code", ListaVehiculos(itm))
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            If strEntradaReferencia <> String.Empty Then
                oGeneralData.SetProperty("U_TIPINV", "C")
            Else
                oGeneralData.SetProperty("U_TIPINV", "S")
            End If
            oGeneralService.Update(oGeneralData)
        Next

        ListaVehiculos.Clear()

    End Sub



    Private Sub ManejoVehiculosRecibidos(ByVal p_form As Form, ByVal itemsUnidades As List(Of ItemUnidad), ByVal itemsUnidadesUsados As List(Of ItemUnidadUsado))

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strCodUnidadUsado As String

        Dim strClienteVentaUsado As String = ""
        Dim strNombreCliente As String = ""
        Dim strTipoVendido As String = ""
        Dim strEstadoVendido As String = ""
        Dim strIDVehUsado As String = ""
        Dim strTipoReingreso As String = ""
        Dim strConsulta As String = ""

        Dim strIDVehiculo As String() = Nothing
        Dim strCliente As String() = Nothing
        Dim strNombCliente As String() = Nothing
        Dim strTipo As String() = Nothing
        Dim intUsadosReingreso As Integer = 0

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Dim oChildTrazabilidad As SAPbobsCOM.GeneralData
        Dim oChildrenTrazabilidad As SAPbobsCOM.GeneralDataCollection
        Dim dtDatosIngreso As System.Data.DataTable

        Dim strNumCV As String = String.Empty
        Dim strMonedaVehi As String = String.Empty
        Dim strMonedaLocal As String = String.Empty


        Try

            strEstadoVendido = Utilitarios.EjecutarConsulta("Select U_Disp_V from [@SCGD_ADMIN] with (nolock) where Code = 'DMS'", m_oCompany.CompanyDB, m_oCompany.Server)

            strTipoVendido = Utilitarios.EjecutarConsulta("Select U_Inven_V from [@SCGD_ADMIN] with (nolock) where Code = 'DMS'", m_oCompany.CompanyDB, m_oCompany.Server)

            oMatrix = DirectCast(p_form.Items.Item("mtx_Usado").Specific, SAPbouiCOM.Matrix)

            dataTableUsadosReversion = p_form.DataSources.DataTables.Add("UsadosReingreso")

            dataTableUsadosReversion = p_form.DataSources.DataTables.Item("UsadosReingreso")

            strNumCV = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("DocNum", 0)
            strNumCV = strNumCV.Trim()
            strMonedaVehi = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_Moneda", 0)
            strMonedaVehi = strMonedaVehi.Trim()
            strMonedaLocal = Utilitarios.EjecutarConsulta("select MainCurncy from [OADM]", m_oCompany.CompanyDB, m_oCompany.Server)
            strMonedaLocal = strMonedaLocal.Trim()

            For i As Integer = 0 To oMatrix.RowCount - 1

                strCodUnidadUsado = p_form.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Unid", i)
                strCodUnidadUsado = strCodUnidadUsado.Trim()

                If Not String.IsNullOrEmpty(strCodUnidadUsado) Then

                    dataTableUsadosReversion.Clear()

                    strConsulta = "Select Code, U_Cli_Ven, U_ClNo_Ven, U_Tipo_Reing from [@SCGD_VEHICULO] with (nolock) where U_Cod_Unid = '" & strCodUnidadUsado & "'"

                    dataTableUsadosReversion.ExecuteQuery(strConsulta)

                    strClienteVentaUsado = dataTableUsadosReversion.GetValue("U_Cli_Ven", 0)

                    If String.IsNullOrEmpty(strClienteVentaUsado) And (ValidaSalidaOPerteneceAContrato(strCodUnidadUsado, itemsUnidadesUsados) = False) Then 'And ValidaSiExistenVariasEntradas(strCodUnidadUsado) = False

                        Dim baseDatos As String
                        baseDatos = SBO_Application.Company.DatabaseName
                        Dim Server As String
                        Server = SBO_Application.Company.ServerName
                        Dim strConsultaBorrar As String
                        Dim strSQLBorrarTraz As String

                        strConsultaBorrar = "Delete From dbo.[@SCGD_VEHICULO] with (nolock) where U_Cod_Unid = '" & strCodUnidadUsado & "'"
                        Utilitarios.EjecutarConsulta(strConsultaBorrar, baseDatos, Server)

                        strIDVehUsado = dataTableUsadosReversion.GetValue("Code", 0)
                        strSQLBorrarTraz = "DELETE FROM [DBO].[@SCGD_VEHITRAZA] with (nolock) Where code = '{0}'"
                        strSQLBorrarTraz = String.Format(strSQLBorrarTraz, strIDVehUsado)
                        Utilitarios.EjecutarConsulta(strSQLBorrarTraz, m_oCompany.CompanyDB, m_oCompany.Server)


                    ElseIf Not String.IsNullOrEmpty(strClienteVentaUsado) Then

                        strNombreCliente = dataTableUsadosReversion.GetValue("U_ClNo_Ven", 0)

                        strIDVehUsado = dataTableUsadosReversion.GetValue("Code", 0)

                        strTipoReingreso = dataTableUsadosReversion.GetValue("U_Tipo_Reing", 0)

                        ReDim Preserve strIDVehiculo(intUsadosReingreso)
                        strIDVehiculo(intUsadosReingreso) = strIDVehUsado
                        ReDim Preserve strCliente(intUsadosReingreso)
                        strCliente(intUsadosReingreso) = strClienteVentaUsado
                        ReDim Preserve strNombCliente(intUsadosReingreso)
                        strNombCliente(intUsadosReingreso) = strNombreCliente
                        ReDim Preserve strTipo(intUsadosReingreso)
                        strTipo(intUsadosReingreso) = strTipoReingreso

                        intUsadosReingreso = intUsadosReingreso + 1

                    End If

                End If

            Next

            For i As Integer = 0 To intUsadosReingreso - 1

                If blnReversaDatosTrazabilidad Then
                    dtDatosIngreso = ObtenerDatosIngresoReversion(strIDVehiculo(i), strNumCV)
                    'dtValoresEntrada = ObtenerValorEntradaReversion(strIDVehiculo(i), strNumCV, strNumGR)
                End If

                oCompanyService = m_oCompany.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_VEH")
                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("Code", strIDVehiculo(i))
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                oChildrenTrazabilidad = oGeneralData.Child("SCGD_VEHITRAZA")

                oGeneralData.SetProperty("U_CardCode", strCliente(i))
                oGeneralData.SetProperty("U_CardName", strNombCliente(i))
                oGeneralData.SetProperty("U_Tipo", strTipoVendido)
                oGeneralData.SetProperty("U_Dispo", strEstadoVendido)
                oGeneralData.SetProperty("U_Tipo_Ven", strTipo(i))
                oGeneralData.SetProperty("U_Cli_Ven", String.Empty)
                oGeneralData.SetProperty("U_ClNo_Ven", String.Empty)
                oGeneralData.SetProperty("U_Tipo_Reing", String.Empty)
                '--------------------------------------------------------------

                If blnReversaDatosTrazabilidad Then

                    If String.IsNullOrEmpty(strIDVehiculo(i)) Then
                        oChildTrazabilidad = oChildrenTrazabilidad.Add()
                    Else
                        oChildTrazabilidad = oChildrenTrazabilidad.Item(0)
                    End If

                    If Not IsNothing(dtDatosIngreso) Then
                        If dtDatosIngreso.Rows.Count > 0 Then

                            For Each loRow As DataRow In dtDatosIngreso.Rows
                                With loRow
                                    oChildTrazabilidad.SetProperty("U_NumDoc_I", .Item("NumFact"))
                                    oChildTrazabilidad.SetProperty("U_FhaDoc_I", .Item("FhaFact"))
                                    oChildTrazabilidad.SetProperty("U_NumCV_I", .Item("NumCV").ToString())
                                    oChildTrazabilidad.SetProperty("U_FhaCV_I", .Item("FhaCV"))
                                    oChildTrazabilidad.SetProperty("U_CodVen_I", .Item("Vendedor"))
                                    oChildTrazabilidad.SetProperty("U_TotDoc_I", .Item("TotalRec").ToString())
                                End With
                            Next
                        End If
                    Else
                        oChildTrazabilidad.SetProperty("U_NumDoc_I", String.Empty)
                        oChildTrazabilidad.SetProperty("U_FhaDoc_I", String.Empty)
                        oChildTrazabilidad.SetProperty("U_NumCV_I", String.Empty)
                        oChildTrazabilidad.SetProperty("U_FhaCV_I", String.Empty)
                        oChildTrazabilidad.SetProperty("U_CodVen_I", String.Empty)
                        oChildTrazabilidad.SetProperty("U_TotDoc_I", 0)
                    End If
                End If
                'Dim row As System.Data.DataRow

                If Not IsNothing(itemsUnidades) Then
                    If itemsUnidades.Count > 0 Then
                        For contador As Integer = 0 To itemsUnidades.Count - 1
                            If strIDVehiculo(i) = itemsUnidades(contador).strUnidad Then
                                If strMonedaLocal = strMonedaVehi Then
                                    oChildTrazabilidad.SetProperty("U_ValVeh", CStr(itemsUnidades(contador).decCosto))
                                Else
                                    oChildTrazabilidad.SetProperty("U_ValVeh", CStr(itemsUnidades(contador).decCostoS))
                                End If
                            End If
                        Next
                    End If
                End If

                oGeneralService.Update(oGeneralData)
                ' strCodeTraza = String.Empty
            Next


        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Private Sub ManejoVehiculosRecibidosUDO(ByVal p_form As Form, ByVal itemsUnidades As List(Of ItemUnidad), ByVal itemsUnidadesUsados As List(Of ItemUnidadUsado))

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strCodUnidadUsado As String

        Dim strClienteVentaUsado As String = String.Empty
        Dim strNombreCliente As String = String.Empty
        Dim strTipoVendido As String = String.Empty
        Dim strEstadoVendido As String = String.Empty
        Dim strIDVehUsado As String = String.Empty
        Dim strTipoReingreso As String = String.Empty
        Dim strConsulta As String = String.Empty

        Dim strIDVehiculo As String() = Nothing
        Dim strCliente As String() = Nothing
        Dim strNombCliente As String() = Nothing
        Dim strTipo As String() = Nothing
        Dim intUsadosReingreso As Integer = 0

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim oChildTrazabilidad As SAPbobsCOM.GeneralData
        Dim oChildrenTrazabilidad As SAPbobsCOM.GeneralDataCollection

        Dim dtDatosIngreso As System.Data.DataTable

        Dim strNumCV As String = String.Empty
        Dim strMonedaVehi As String = String.Empty
        Dim strMonedaLocal As String = String.Empty


        Try
            'strEstadoVendido = Utilitarios.EjecutarConsulta("Select U_Disp_V from [@SCGD_ADMIN] with (nolock) where Code = 'DMS'", m_oCompany.CompanyDB, m_oCompany.Server)
            'strTipoVendido = Utilitarios.EjecutarConsulta("Select U_Inven_V from [@SCGD_ADMIN] with (nolock) where Code = 'DMS'", m_oCompany.CompanyDB, m_oCompany.Server)
            If (Utilitarios.ValidaExisteDataTable(p_form, "dtConsulta")) Then
                dtConsulta = p_form.DataSources.DataTables.Item("dtConsulta")
            Else
                dtConsulta = p_form.DataSources.DataTables.Add("dtConsulta")
            End If


            Dim query = "Select U_Disp_V, U_Inven_V from [@SCGD_ADMIN] with (nolock) where Code = 'DMS'"
            dtConsulta.ExecuteQuery(query)
            If dtConsulta.Rows.Count > 0 Then
                strEstadoVendido = dtConsulta.GetValue("U_Disp_V", 0)
                strTipoVendido = dtConsulta.GetValue("U_Inven_V", 0)
            End If
            oMatrix = DirectCast(p_form.Items.Item("mtx_Usado").Specific, SAPbouiCOM.Matrix)

            dataTableUsadosReversion = p_form.DataSources.DataTables.Item("UsadosReingreso")

            strNumCV = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("DocNum", 0)
            strNumCV = strNumCV.Trim()
            strMonedaVehi = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_Moneda", 0)
            strMonedaVehi = strMonedaVehi.Trim()

            dtConsulta.Clear()
            dtConsulta.ExecuteQuery("select MainCurncy from [OADM] with (nolock) ")
            'strMonedaLocal = Utilitarios.EjecutarConsulta("select MainCurncy from [OADM] with (nolock) ", m_oCompany.CompanyDB, m_oCompany.Server)
            'strMonedaLocal = strMonedaLocal.Trim()
            strMonedaLocal = dtConsulta.GetValue("MainCurncy", 0).ToString().Trim()

            For i As Integer = 0 To oMatrix.RowCount - 1

                strCodUnidadUsado = p_form.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Unid", i)
                strCodUnidadUsado = strCodUnidadUsado.Trim()

                dataTableUsadosReversion.Clear()

                strConsulta = "Select Code, U_Cli_Ven, U_ClNo_Ven, U_Tipo_Reing from [@SCGD_VEHICULO] with (nolock) where U_Cod_Unid = '" & strCodUnidadUsado & "'"

                dataTableUsadosReversion.ExecuteQuery(strConsulta)

                strClienteVentaUsado = dataTableUsadosReversion.GetValue("U_Cli_Ven", 0)

                If String.IsNullOrEmpty(strClienteVentaUsado) And (ValidaSalidaOPerteneceAContrato(strCodUnidadUsado, itemsUnidadesUsados) = False) Then 'And ValidaSiExistenVariasEntradas(strCodUnidadUsado) = False

                    Dim baseDatos As String
                    baseDatos = SBO_Application.Company.DatabaseName
                    Dim Server As String
                    Server = SBO_Application.Company.ServerName
                    Dim strConsultaBorrar As String
                    Dim strSQLBorrarTraz As String

                    dtConsulta.Clear()

                    strConsultaBorrar = " Delete From dbo.[@SCGD_VEHICULO] where U_Cod_Unid = '" & strCodUnidadUsado & "'"
                    'Utilitarios.EjecutarConsulta(strConsultaBorrar, baseDatos, Server)
                    dtConsulta.ExecuteQuery(strConsultaBorrar)

                    strIDVehUsado = dataTableUsadosReversion.GetValue("Code", 0)
                    strSQLBorrarTraz = " DELETE FROM [DBO].[@SCGD_VEHITRAZA] Where code = '{0}'"
                    strSQLBorrarTraz = String.Format(strSQLBorrarTraz, strIDVehUsado)
                    'Utilitarios.EjecutarConsulta(strSQLBorrarTraz, m_oCompany.CompanyDB, m_oCompany.Server)
                    dtConsulta.Clear()
                    dtConsulta.ExecuteQuery(strSQLBorrarTraz)


                ElseIf Not String.IsNullOrEmpty(strClienteVentaUsado) Then

                    strNombreCliente = dataTableUsadosReversion.GetValue("U_ClNo_Ven", 0)

                    strIDVehUsado = dataTableUsadosReversion.GetValue("Code", 0)

                    strTipoReingreso = dataTableUsadosReversion.GetValue("U_Tipo_Reing", 0)

                    ReDim Preserve strIDVehiculo(intUsadosReingreso)
                    strIDVehiculo(intUsadosReingreso) = strIDVehUsado
                    ReDim Preserve strCliente(intUsadosReingreso)
                    strCliente(intUsadosReingreso) = strClienteVentaUsado
                    ReDim Preserve strNombCliente(intUsadosReingreso)
                    strNombCliente(intUsadosReingreso) = strNombreCliente
                    ReDim Preserve strTipo(intUsadosReingreso)
                    strTipo(intUsadosReingreso) = strTipoReingreso

                    intUsadosReingreso = intUsadosReingreso + 1

                End If

            Next

            For i As Integer = 0 To intUsadosReingreso - 1

                If blnReversaDatosTrazabilidad Then
                    dtDatosIngreso = ObtenerDatosIngresoReversion(strIDVehiculo(i), strNumCV)
                End If

                oCompanyService = m_oCompany.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_VEH")
                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("Code", strIDVehiculo(i))
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                'oChildrenTrazabilidad = oGeneralData.Child("SCGD_VEHITRAZA")

                oGeneralData.SetProperty("U_CardCode", strCliente(i))
                oGeneralData.SetProperty("U_CardName", strNombCliente(i))
                oGeneralData.SetProperty("U_Tipo", strTipoVendido)
                oGeneralData.SetProperty("U_Dispo", strEstadoVendido)
                oGeneralData.SetProperty("U_Tipo_Ven", strTipo(i))
                oGeneralData.SetProperty("U_Cli_Ven", String.Empty)
                oGeneralData.SetProperty("U_ClNo_Ven", String.Empty)
                oGeneralData.SetProperty("U_Tipo_Reing", String.Empty)

                oChildrenTrazabilidad = oGeneralData.Child("SCGD_VEHITRAZA")

                If oChildrenTrazabilidad.Count > 0 Then

                    If String.IsNullOrEmpty(strIDVehiculo(i)) Then
                        oChildTrazabilidad = oChildrenTrazabilidad.Add()
                    Else
                        oChildTrazabilidad = oChildrenTrazabilidad.Item(0)
                    End If

                    If Not IsNothing(dtDatosIngreso) Then
                        If dtDatosIngreso.Rows.Count > 0 Then

                            For Each loRow As DataRow In dtDatosIngreso.Rows
                                With loRow
                                    oChildTrazabilidad.SetProperty("U_NumDoc_I", .Item("NumFact"))
                                    oChildTrazabilidad.SetProperty("U_FhaDoc_I", .Item("FhaFact"))
                                    oChildTrazabilidad.SetProperty("U_NumCV_I", .Item("NumCV").ToString())
                                    oChildTrazabilidad.SetProperty("U_FhaCV_I", .Item("FhaCV"))
                                    oChildTrazabilidad.SetProperty("U_CodVen_I", .Item("Vendedor"))
                                    oChildTrazabilidad.SetProperty("U_TotDoc_I", CDbl(.Item("TotalRec")))
                                    oChildTrazabilidad.SetProperty("U_Km_Ingreso", CDbl(.Item("KmIngreso")))
                                End With
                            Next
                        End If
                    Else
                        oChildTrazabilidad.SetProperty("U_NumDoc_I", String.Empty)
                        oChildTrazabilidad.SetProperty("U_FhaDoc_I", String.Empty)
                        oChildTrazabilidad.SetProperty("U_NumCV_I", String.Empty)
                        oChildTrazabilidad.SetProperty("U_FhaCV_I", String.Empty)
                        oChildTrazabilidad.SetProperty("U_CodVen_I", String.Empty)
                        oChildTrazabilidad.SetProperty("U_TotDoc_I", 0)
                        oChildTrazabilidad.SetProperty("U_Km_Ingreso", 0)
                    End If
                End If

                If Not IsNothing(itemsUnidades) Then
                    If itemsUnidades.Count > 0 Then
                        For contador As Integer = 0 To itemsUnidades.Count - 1
                            If strIDVehiculo(i) = itemsUnidades(contador).strUnidad Then
                                If strMonedaLocal = strMonedaVehi Then
                                    oChildTrazabilidad.SetProperty("U_ValVeh", CStr(itemsUnidades(contador).decCosto))
                                Else
                                    oChildTrazabilidad.SetProperty("U_ValVeh", CStr(itemsUnidades(contador).decCostoS))
                                End If
                            End If
                        Next
                    End If
                End If
                oGeneralService.Update(oGeneralData)
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ValidaSalidaOPerteneceAContrato(ByVal p_NoUnidad As String, ByVal itemsUnidadesUsados As List(Of ItemUnidadUsado)) As Boolean
        Try
            If Not IsNothing(itemsUnidadesUsados) Then
                If itemsUnidadesUsados.Count > 0 Then
                    For contador As Integer = 0 To itemsUnidadesUsados.Count - 1
                        If itemsUnidadesUsados.Item(contador).strUnidad = p_NoUnidad Then
                            Return True
                        End If
                    Next
                    Return False
                End If
                Return False
            End If
            Return False
        Catch ex As Exception

        End Try
    End Function

    Public Sub ReversarEntradasVehiculo(ByVal p_variable As Integer, ByVal p_dtFechaReversion As Date)

        '*********************************************REAL************************************
        Dim strConectionString As String = ""
        Dim cnConeccionBD As SqlClient.SqlConnection
        m_objReversarSalidaMercancia = New ReversarSalidaMercanciaCls(SBO_Application, m_oCompany)

        Dim intAsientoReversion As Integer = 0
        Dim intContNumEntrada As Integer = 0
        Dim intSeries As Integer = 0

        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, _
                                             m_oCompany.CompanyDB, _
                                             strConectionString)
        cnConeccionBD = New SqlClient.SqlConnection
        cnConeccionBD.ConnectionString = strConectionString

        dtaReversarEntradas.Connection = New SqlClient.SqlConnection(strConectionString)
        dtaReversarEntradas.Connection = cnConeccionBD
        dtaSalidaContable.Connection = New SqlClient.SqlConnection(strConectionString)
        dtaSalidaContable.Connection = cnConeccionBD
        'Cambio reversion salidas mercancia
        dtaSalidaMercancia.Connection = New SqlClient.SqlConnection(strConectionString)
        dtaSalidaMercancia.Connection = cnConeccionBD
        cnConeccionBD.Open()

        dtaSalidaContable.FillSalidaContable(dtsSalidaContable.SalidasContables_, p_variable)
        dtaSalidaMercancia.FillSalidaContable(dtsSalidaMercancia.__SCGD_GOODISSUE, p_variable)
        If dtsSalidaContable.SalidasContables_.Rows.Count > 0 Then
            If dtsSalidaMercancia.__SCGD_GOODISSUE.Rows.Count > 0 Then
                intContNumEntrada = 0
                'For Each drwSal As DMS_Addon.ReversarContratoDataSet.SalidasContables_Row In dtsSalidaContable.SalidasContables_
                For Each drwSal As SalidaContableDataset.__SCGD_GOODISSUERow In dtsSalidaMercancia.__SCGD_GOODISSUE
                    If Not String.IsNullOrEmpty(drwSal.U_As_Sali) Then
                        intTempAsientoReversado = 0
                        CrearDocumentoAsientoEntradaRevertido(drwSal.U_As_Sali)
                        If intTempAsientoReversado > 0 Then
                            m_objReversarSalidaMercancia.CrearEntradas(intTempAsientoReversado, drwSal, p_dtFechaReversion, intContNumEntrada, 0, intSeries)
                            Call ActualizarSalida(drwSal.DocEntry)
                        End If
                    End If
                Next
            End If
        End If
    End Sub

    Public Sub ActualizarSalida(ByVal p_docentrySalida As Integer)

        Dim baseDatos As String
        baseDatos = SBO_Application.Company.DatabaseName
        Dim Server As String
        Server = SBO_Application.Company.ServerName
        Dim strConsulta As String

        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim sCmp As SAPbobsCOM.CompanyService
        Dim transaccionExterna As Boolean
        Try
            sCmp = m_oCompany.GetCompanyService
            'Obtiene el manejador del UDO
            oGeneralService = sCmp.GetGeneralService("SCGD_GOODISSUE")
            'Crea los parametros y borra el registro del UDO
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", p_docentrySalida)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            oGeneralData.SetProperty("U_Reversa", "Y")
            'oGeneralService.Delete(oGeneralParams)
            oGeneralService.Update(oGeneralData)

        Catch ex As Exception

            Throw
        End Try


    End Sub

    'Este es el metodo EliminarSalida original
    Public Sub EliminarSalida(ByVal p_docentrySalida As Integer)

        Dim baseDatos As String
        baseDatos = SBO_Application.Company.DatabaseName
        Dim Server As String
        Server = SBO_Application.Company.ServerName
        Dim strConsulta As String


        'strConsulta = "Delete From dbo. [@SCGD_GILINES] where docentry = " & p_docentrySalida & "; " & "Delete From dbo.[@SCGD_GOODISSUE] where docentry = " & p_docentrySalida
        'Utilitarios.EjecutarConsulta(strConsulta, baseDatos, Server)

        'strConsulta = "Delete From dbo.[@SCGD_GOODISSUE] where docentry = " & p_docentrySalida
        'Utilitarios.EjecutarConsulta(strConsulta, baseDatos, Server)

        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim sCmp As SAPbobsCOM.CompanyService
        Dim transaccionExterna As Boolean
        Try

            transaccionExterna = m_oCompany.InTransaction
            If Not transaccionExterna Then
                'Inicia transacción
                Call m_oCompany.StartTransaction()
            End If
            sCmp = m_oCompany.GetCompanyService
            'Obtiene el manejador del UDO
            oGeneralService = sCmp.GetGeneralService("SCGD_GOODISSUE")
            'Crea los parametros y borra el registro del UDO
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", p_docentrySalida)
            oGeneralService.Delete(oGeneralParams)


            If Not transaccionExterna Then
                Call m_oCompany.EndTransaction(BoWfTransOpt.wf_Commit)
            End If

        Catch ex As Exception
            If Not transaccionExterna Then
                Call m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If

            Throw
        End Try


    End Sub


    Public Function ReversarFacturaAcredoraDeudaUsado(ByVal p_fom As SAPbouiCOM.Form, ByVal p_docNumFacturaAcredora As Long, ByVal strOrigenFact As String, _
                                                      Optional p_blnUsaFPVU As Boolean = False, Optional p_blnUsaDistincionSocioNegocio As Boolean = False, Optional ByRef strNCFPVU As String = "") As Integer

        Dim objDocumentoNC_Proveedor As SAPbobsCOM.Documents
        Dim objFacturaAcredora As SAPbobsCOM.Documents
        Dim objFacturaAcredoraLines As SAPbobsCOM.Document_Lines

        Dim strComentario As String = String.Empty
        Dim strTipoVeh As String
        Dim strInventarioUsado As String

        objDocumentoNC_Proveedor = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes)

        objFacturaAcredora = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices),  _
                                                                SAPbobsCOM.Documents)

        If objFacturaAcredora.GetByKey(p_docNumFacturaAcredora) Then

            If Not objFacturaAcredora.DocumentStatus = BoStatus.bost_Close Then

                Dim intDocNum As Integer = objFacturaAcredora.DocNum

                strComentario = My.Resources.Resource.ComentarioReversaFact & intDocNum.ToString()

                objDocumentoNC_Proveedor.DocDate = objFacturaAcredora.DocDate

                objFacturaAcredoraLines = objFacturaAcredora.Lines

                For i As Integer = 0 To objFacturaAcredoraLines.Count - 1

                    objFacturaAcredoraLines.SetCurrentLine(i)


                    With objDocumentoNC_Proveedor

                        .DocType = BoDocumentTypes.dDocument_Service
                        .Comments = strComentario
                        .Lines.Quantity = objFacturaAcredoraLines.Quantity
                        .Lines.BaseType = 18
                        .Lines.BaseLine = objFacturaAcredoraLines.LineNum
                        .Lines.BaseEntry = CInt(p_docNumFacturaAcredora)

                        objDocumentoNC_Proveedor.Lines.Add()

                    End With

                Next

                objDocumentoNC_Proveedor.UserFields.Fields.Item("U_SCGD_NoContrato").Value = objFacturaAcredora.UserFields.Fields.Item("U_SCGD_NoContrato").Value
                objDocumentoNC_Proveedor.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = objFacturaAcredora.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value

                objDocumentoNC_Proveedor.DocDate = dtFechaDocumento
                objDocumentoNC_Proveedor.DocDueDate = dtFechaDocumento
                objDocumentoNC_Proveedor.TaxDate = dtFechaDocumento

                strInventarioUsado = p_fom.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Tipo", 0).Trim
                strTipoVeh = Utilitarios.EjecutarConsulta(String.Format("SELECT code FROM [@SCGD_TIPOVEHICULO] with(nolock) WHERE Name = '{0}'", strInventarioUsado), m_oCompany.CompanyDB, m_oCompany.Server).Trim()

                If String.IsNullOrEmpty(strTipoVeh) Then

                    Dim strUnidad As String = objFacturaAcredora.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value.ToString().Trim

                    If Not String.IsNullOrEmpty(strUnidad) Then
                        strTipoVeh = Utilitarios.EjecutarConsulta("Select U_Tipo from dbo.[@SCGD_Vehiculo] WITH (nolock) where U_Cod_Unid = '" & strUnidad & "'", m_oCompany.CompanyDB, m_oCompany.Server).Trim()
                    End If

                End If

                If p_blnUsaFPVU Then

                    If p_blnUsaDistincionSocioNegocio Then

                        Dim blnBEvento As Boolean
                        objConfiguracionGeneral = Nothing
                        objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(strTipoVeh, m_cn_Coneccion, blnBEvento)

                        Dim strConsulta As String = "select U_TipSoc from [OCRD] with (nolock) where CardCode ='{0}'"
                        Dim strTipoSocioNegocio As String = Utilitarios.EjecutarConsulta(String.Format(strConsulta, objFacturaAcredora.CardCode), m_oCompany.CompanyDB, m_oCompany.Server)

                        If Not String.IsNullOrEmpty(strTipoSocioNegocio.Trim()) Then
                            If strTipoSocioNegocio = "S" Then

                                Dim strIndicador As String = Utilitarios.DevuelveCodIndicadores(SBO_Application, "14")

                                If Not String.IsNullOrEmpty(strIndicador) Then

                                    objDocumentoNC_Proveedor.Indicator = strIndicador

                                End If


                                'intSerieDocumento = objConfiguracionGeneral.Serie(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.NotaCreditoReciboUsadoSociedades)
                                If Not String.IsNullOrEmpty(strTipoVeh) Then
                                    Dim strSerieDocumento As String = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Serie FROM [dbo].[@SCGD_ADMIN6] with (nolock) where U_Tipo= '{0}' and U_Cod_Item= '16'", _
                                                                                                                                                 strTipoVeh), m_oCompany.CompanyDB, m_oCompany.Server)
                                    If Not String.IsNullOrEmpty(strSerieDocumento) Then
                                        objDocumentoNC_Proveedor.Series = CInt(strSerieDocumento)
                                    End If
                                End If


                            ElseIf strTipoSocioNegocio = "P" Then

                                Dim strIndicador As String = Utilitarios.DevuelveCodIndicadores(SBO_Application, "15")

                                If Not String.IsNullOrEmpty(strIndicador) Then

                                    objDocumentoNC_Proveedor.Indicator = strIndicador

                                End If

                                'intSerieDocumento = objConfiguracionGeneral.Serie(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.NotaCreditoReciboUsadoPrivado)
                                If Not String.IsNullOrEmpty(strTipoVeh) Then
                                    Dim strSerieDocumento As String = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Serie FROM [dbo].[@SCGD_ADMIN6] with (nolock) where U_Tipo= '{0}' and U_Cod_Item= '17'", _
                                                                                                                 strTipoVeh), m_oCompany.CompanyDB, m_oCompany.Server)
                                    If Not String.IsNullOrEmpty(strSerieDocumento) Then
                                        objDocumentoNC_Proveedor.Series = CInt(strSerieDocumento)
                                    End If
                                End If

                            End If
                        End If
                    End If
                End If

                Dim Verificar As Integer = objDocumentoNC_Proveedor.Add

                If Verificar <> 0 Then

                    Dim strErrMsg As String = m_oCompany.GetLastErrorDescription()
                    SBO_Application.SetStatusBarMessage(strErrMsg, BoMessageTime.bmt_Short, True)

                    m_blnDocumentoReversionNoCreado = True

                Else
                    If p_blnUsaFPVU Then
                        strNCFPVU = m_oCompany.GetNewObjectKey
                        Return 0
                    Else
                        intNotaCredito_FacturaProveedorDeudaUsado = m_oCompany.GetNewObjectKey
                        Return 0
                    End If


                End If
            End If

        End If

    End Function

    Public Function ReversarFacturaClienteDeudaUsado(ByVal p_form As SAPbouiCOM.Form, ByVal p_docNumFacturaClienteDeuda As Long, ByVal strOrigenFact As String) As Integer

        Dim objDocumentoNC_DeudaUsado As SAPbobsCOM.Documents
        Dim objFactura As SAPbobsCOM.Documents
        Dim objFacturaLines As SAPbobsCOM.Document_Lines
        Dim strComentario As String = String.Empty
        Dim intSerieDocumento As Integer = -1
        Dim strTipoInventario As String = String.Empty

        objDocumentoNC_DeudaUsado = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes)

        objFactura = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices),  _
                                                                SAPbobsCOM.Documents)

        If objFactura.GetByKey(p_docNumFacturaClienteDeuda) Then

            If Not objFactura.DocumentStatus = BoStatus.bost_Close Then

                Dim intDocNum As Integer = objFactura.DocNum

                strTipoInventario = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_TipIn", 0).Trim
                intSerieDocumento = DMS_Connector.Helpers.GetSerie(strTipoInventario, DMS_Connector.Data_Access.GeneralEnums.scgTipoSeries.NotaCreditoReversionFacturaDeudaUsado, False)

                If intSerieDocumento <> -1 Then
                    objDocumentoNC_DeudaUsado.Series = intSerieDocumento
                End If

                objDocumentoNC_DeudaUsado.DocDate = objFactura.DocDate

                strComentario = My.Resources.Resource.ComentarioReversaFact & intDocNum.ToString()

                objFacturaLines = objFactura.Lines

                For i As Integer = 0 To objFacturaLines.Count - 1

                    objFacturaLines.SetCurrentLine(i)

                    With objDocumentoNC_DeudaUsado

                        .DocType = BoDocumentTypes.dDocument_Service

                        .PaymentGroupCode = intGroupNum

                        .Comments = strComentario

                        .Lines.Quantity = objFacturaLines.Quantity
                        .Lines.BaseType = 13
                        .Lines.BaseLine = objFacturaLines.LineNum
                        .Lines.BaseEntry = CInt(p_docNumFacturaClienteDeuda)

                        objDocumentoNC_DeudaUsado.Lines.Add()

                    End With

                Next
                objDocumentoNC_DeudaUsado.DocDate = dtFechaDocumento
                objDocumentoNC_DeudaUsado.DocDueDate = dtFechaDocumento
                objDocumentoNC_DeudaUsado.TaxDate = dtFechaDocumento

                Dim Verificar As Integer = objDocumentoNC_DeudaUsado.Add

                If Verificar <> 0 Then

                    Dim strErrMsg As String = m_oCompany.GetLastErrorDescription()
                    SBO_Application.SetStatusBarMessage(strErrMsg, BoMessageTime.bmt_Short, True)

                    m_blnDocumentoReversionNoCreado = True

                Else

                    intNotaCredito_FacturaClienteDeudaUsado = m_oCompany.GetNewObjectKey
                    Return 0
                End If

            End If

        End If

    End Function

    Public Function ReversarFacturaComisionConsignado(ByVal p_form As SAPbouiCOM.Form, ByVal p_numFacturaConsignado As Long) As Integer

        Dim oCreditMemo As SAPbobsCOM.Documents
        Dim oInvoice As SAPbobsCOM.Documents
        Dim oInvoiceLines As SAPbobsCOM.Document_Lines
        Dim strComentario As String = String.Empty
        Dim intSerie
        Dim strTipoInventario As String = String.Empty


        Try

            oCreditMemo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes)
            oInvoice = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices), SAPbobsCOM.Documents)

            If oInvoice.GetByKey(p_numFacturaConsignado) Then

                If Not oInvoice.DocumentStatus = BoStatus.bost_Close Then
                    strTipoInventario = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_TipIn", 0).Trim
                    intSerie = DMS_Connector.Helpers.GetSerie(strTipoInventario, DMS_Connector.Data_Access.GeneralEnums.scgTipoSeries.NotaCreditoComisionConsignados, True)

                    If intSerie <> -1 Then
                        oCreditMemo.Series = intSerie
                        Dim intDocNum As Integer = oInvoice.DocNum
                        oCreditMemo.NumAtCard = oInvoice.NumAtCard
                        oCreditMemo.DocDate = oInvoice.DocDate
                        strComentario = My.Resources.Resource.ComentarioReversaFact & intDocNum.ToString()

                        oInvoiceLines = oInvoice.Lines

                        For i As Integer = 0 To oInvoiceLines.Count - 1

                            oInvoiceLines.SetCurrentLine(i)

                            With oCreditMemo

                                .DocType = BoDocumentTypes.dDocument_Service
                                .PaymentGroupCode = intGroupNum
                                .Comments = strComentario
                                .Lines.Quantity = oInvoiceLines.Quantity
                                .Lines.BaseType = 13
                                .Lines.BaseLine = oInvoiceLines.LineNum
                                .Lines.BaseEntry = CInt(p_numFacturaConsignado)
                                .Lines.CostingCode = oInvoiceLines.CostingCode
                                .Lines.CostingCode2 = oInvoiceLines.CostingCode2
                                .Lines.CostingCode3 = oInvoiceLines.CostingCode3
                                .Lines.CostingCode4 = oInvoiceLines.CostingCode4
                                .Lines.CostingCode5 = oInvoiceLines.CostingCode5


                                oCreditMemo.Lines.Add()

                            End With

                        Next
                        oCreditMemo.DocDate = dtFechaDocumento
                        oCreditMemo.DocDueDate = dtFechaDocumento
                        oCreditMemo.TaxDate = dtFechaDocumento

                        Dim Verificar As Integer = oCreditMemo.Add

                        If Verificar <> 0 Then

                            Dim strErrMsg As String = m_oCompany.GetLastErrorDescription()
                            SBO_Application.SetStatusBarMessage(strErrMsg, BoMessageTime.bmt_Short, True)

                            m_blnDocumentoReversionNoCreado = True
                        Else
                            intNotaCreditoPorCmsConsignados = m_oCompany.GetNewObjectKey
                            Return 0
                        End If
                    Else
                        m_blnDocumentoReversionNoCreado = True
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorSerieNumCmsConsignados, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    End If

                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            m_blnDocumentoReversionNoCreado = True
        End Try
    End Function


    Public Function ReversarFactura(ByVal p_fom As SAPbouiCOM.Form, ByVal p_docNumFactura As Long, ByVal strOrigenFact As String, Optional ByVal strUnidadFact As String = "") As Integer

        Dim objDocumentoNC As SAPbobsCOM.Documents
        Dim objFactura As SAPbobsCOM.Documents
        Dim objFacturaLines As SAPbobsCOM.Document_Lines
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim Vehiculo As String
        Dim Marca As String
        Dim Estilo As String
        Dim strComentario As String

        Dim blnBEvento As Boolean
        objConfiguracionGeneral = Nothing
        Dim m_strConectionString As String

        objDocumentoNC = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes)

        oMatrix = DirectCast(p_fom.Items.Item("mtx_Vehi").Specific, SAPbouiCOM.Matrix)

        objFactura = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices), SAPbobsCOM.Documents)

        Dim strIndicador As String = Utilitarios.DevuelveCodIndicadores(SBO_Application, "3")

        If Not String.IsNullOrEmpty(strIndicador) Then
            objDocumentoNC.Indicator = strIndicador
        End If

        Dim strConsignado As String = Utilitarios.EjecutarConsulta(String.Format("Select U_Consig from [@SCGD_VEHICULO] WITH (nolock) where U_Cod_Unid = '{0}'",
                                                      p_fom.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Cod_Unid", 0).Trim))

        Dim strTipoInventarioVehiculo = p_fom.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_TipIn", 0).Trim


        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, m_strConectionString)

        If m_cn_Coneccion.State = ConnectionState.Open Then
            m_cn_Coneccion.Close()
        End If

        m_cn_Coneccion.ConnectionString = m_strConectionString

        Select Case strOrigenFact

            Case "VEH"
                Dim intSerieFactura As Integer

                objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(strTipoInventarioVehiculo, m_cn_Coneccion, blnBEvento)

                If strConsignado = "Y" Then
                    intSerieFactura = objConfiguracionGeneral.SerieExenta(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.NotasCreditoReversion)
                Else
                    intSerieFactura = objConfiguracionGeneral.Serie(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.NotasCreditoReversion)
                End If

                If intSerieFactura <> -1 Then
                    objDocumentoNC.Series = intSerieFactura
                End If
            Case "ACC"
                objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(strTipoInventarioVehiculo, m_cn_Coneccion, blnBEvento)

                Dim intSerieFactura As Integer = objConfiguracionGeneral.Serie(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.NotaCreditoReversionAccesorios)

                If intSerieFactura <> -1 Then
                    objDocumentoNC.Series = intSerieFactura
                End If

            Case "TRA"
                objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(strTipoInventarioVehiculo, m_cn_Coneccion, blnBEvento)

                Dim intSerieFactura As Integer = objConfiguracionGeneral.Serie(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.NotaCreditoReversionTramites)

                If intSerieFactura <> -1 Then
                    objDocumentoNC.Series = intSerieFactura
                End If
            Case "GAS"
                objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(strTipoInventarioVehiculo, m_cn_Coneccion, blnBEvento)

                Dim intSerieFactura As Integer = objConfiguracionGeneral.Serie(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.NotaCreditoReversionGastos)

                If intSerieFactura <> -1 Then
                    objDocumentoNC.Series = intSerieFactura
                End If
        End Select


        If objFactura.GetByKey(p_docNumFactura) Then

            If Not objFactura.DocumentStatus = BoStatus.bost_Close Then

                objDocumentoNC.DocDate = objFactura.DocDate

                If Not String.IsNullOrEmpty(strUnidadFact) Then

                    For intVehi As Integer = 0 To oMatrix.RowCount - 1

                        Vehiculo = p_fom.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Cod_Unid", intVehi).Trim()

                        If strUnidadFact = Vehiculo Then

                            Marca = p_fom.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Des_Marc", intVehi).Trim()
                            Estilo = p_fom.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Des_Esti", intVehi).Trim()

                            strComentario = String.Format(My.Resources.Resource.MensajeNCReversion, p_docNumFactura, Vehiculo) & " " & Marca & " " & Estilo
                            Exit For

                        End If

                    Next

                ElseIf String.IsNullOrEmpty(strUnidadFact) Then

                    If strOrigenFact = "VEH" Then
                        strComentario = String.Format(My.Resources.Resource.DocUnidMultiplesRevers)
                    Else
                        strComentario = My.Resources.Resource.ComentarioReversaFact & p_docNumFactura.ToString()
                    End If

                End If

                objFacturaLines = objFactura.Lines

                For i As Integer = 0 To objFacturaLines.Count - 1

                    objFacturaLines.SetCurrentLine(i)

                    With objDocumentoNC
                        .Lines.AccountCode = objFacturaLines.AccountCode
                        .PaymentGroupCode = intGroupNum
                        .Comments = strComentario
                        .Lines.Quantity = objFacturaLines.Quantity
                        .Lines.BaseType = 13
                        .Lines.BaseLine = objFacturaLines.LineNum
                        .Lines.BaseEntry = CInt(p_docNumFactura)
                        '******************INICIO | CABYS **************
                        If DMS_Connector.Configuracion.ParamGenAddon.U_CABYS_CR = "Y" Then
                            .Lines.UserFields.Fields.Item("U_SCG_IVA2_Act_Econ").Value = objFacturaLines.UserFields.Fields.Item("U_SCG_IVA2_Act_Econ").Value
                            .Lines.UserFields.Fields.Item("U_SCG_IVA2_TipoItem").Value = objFacturaLines.UserFields.Fields.Item("U_SCG_IVA2_TipoItem").Value
                            .Lines.UserFields.Fields.Item("U_SCG_IVA2_CodItem").Value = objFacturaLines.UserFields.Fields.Item("U_SCG_IVA2_CodItem").Value
                        End If
                        '******************FIN | CABYS ******************
                        objDocumentoNC.Lines.Add()

                    End With

                Next

                m_strMonedaLocal = m_objBLSBO.RetornarMonedaLocal()


                For i As Integer = 0 To objFactura.Expenses.Count - 1

                    objFactura.Expenses.SetCurrentLine(i)

                    If objFactura.Expenses.LineTotal > 0 Then
                        With objDocumentoNC
                            '.Lines.AccountCode = objFactura.Lines.AccountCode
                            .Expenses.BaseDocEntry = objFactura.DocEntry
                            .Expenses.BaseDocLine = objFactura.Expenses.LineNum
                            .Expenses.BaseDocType = 13

                            objDocumentoNC.Expenses.Add()

                        End With
                    End If

                Next

                objDocumentoNC.DocDate = dtFechaDocumento
                objDocumentoNC.DocDueDate = dtFechaDocumento
                objDocumentoNC.TaxDate = dtFechaDocumento
                '******************INICIO | CABYS **************
                If DMS_Connector.Configuracion.ParamGenAddon.U_CABYS_CR = "Y" Then
                    If Not String.IsNullOrEmpty(objFactura.UserFields.Fields.Item("U_SCG_IVA2_LugarCons").Value) Then objDocumentoNC.UserFields.Fields.Item("U_SCG_IVA2_LugarCons").Value = objFactura.UserFields.Fields.Item("U_SCG_IVA2_LugarCons").Value
                    If Not String.IsNullOrEmpty(objFactura.UserFields.Fields.Item("U_SCG_IVA2_TipoExo").Value) Then objDocumentoNC.UserFields.Fields.Item("U_SCG_IVA2_TipoExo").Value = objFactura.UserFields.Fields.Item("U_SCG_IVA2_TipoExo").Value
                End If
                '******************FIN | CABYS ******************
                Dim Verificar As Integer = objDocumentoNC.Add

                If Verificar <> 0 Then

                    Dim strErrMsg As String = m_oCompany.GetLastErrorDescription()
                    SBO_Application.SetStatusBarMessage(strErrMsg, BoMessageTime.bmt_Short, True)

                    m_blnDocumentoReversionNoCreado = True

                Else

                    m_blnDocumentoReversionNoCreado = False

                    If strOrigenFact = "VEH" Then
                        intNotaCreditoProvenienteFactura = m_oCompany.GetNewObjectKey
                    ElseIf strOrigenFact = "ACC" Then
                        intNotaCreditoProvFactAccs = m_oCompany.GetNewObjectKey
                    ElseIf strOrigenFact = "GAS" Then
                        intNotaCreditoProvFactGastos = m_oCompany.GetNewObjectKey
                    End If

                    Return 0

                End If

            End If

        End If

    End Function

    Public Function ReversarFacturaTramites(ByVal p_fom As SAPbouiCOM.Form, ByVal p_docNumFacturaTramites As Long) As Integer

        Dim objDocumentoNC As SAPbobsCOM.Documents
        Dim objFacturaTramites As SAPbobsCOM.Documents
        Dim objFacturaTramitesLines As SAPbobsCOM.Document_Lines

        Dim strComentario As String = String.Empty

        objDocumentoNC = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes)

        objFacturaTramites = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices),  _
                                                                SAPbobsCOM.Documents)

        If objFacturaTramites.GetByKey(p_docNumFacturaTramites) Then

            If Not objFacturaTramites.DocumentStatus = BoStatus.bost_Close Then

                Dim intDocNum As Integer = objFacturaTramites.DocNum

                objDocumentoNC.DocDate = objFacturaTramites.DocDate

                strComentario = My.Resources.Resource.ComentarioReversaFact & intDocNum.ToString()

                objFacturaTramitesLines = objFacturaTramites.Lines

                For i As Integer = 0 To objFacturaTramitesLines.Count - 1

                    objFacturaTramitesLines.SetCurrentLine(i)

                    With objDocumentoNC

                        .DocType = BoDocumentTypes.dDocument_Items

                        .PaymentGroupCode = intGroupNum

                        .Comments = strComentario

                        .Lines.Quantity = objFacturaTramitesLines.Quantity
                        .Lines.BaseType = 13
                        .Lines.BaseLine = objFacturaTramitesLines.LineNum
                        .Lines.BaseEntry = CInt(p_docNumFacturaTramites)

                        objDocumentoNC.Lines.Add()

                    End With

                Next
                objDocumentoNC.DocDate = dtFechaDocumento
                objDocumentoNC.DocDueDate = dtFechaDocumento
                objDocumentoNC.TaxDate = dtFechaDocumento

                Dim Verificar As Integer = objDocumentoNC.Add

                If Verificar <> 0 Then

                    Dim strErrMsg As String = m_oCompany.GetLastErrorDescription()
                    SBO_Application.SetStatusBarMessage(strErrMsg, BoMessageTime.bmt_Short, True)

                    m_blnDocumentoReversionNoCreado = True

                Else

                    intNotaCredito_FacturaTramites = m_oCompany.GetNewObjectKey
                    Return 0
                End If

            End If

        End If

    End Function

    Private Function BuscarEntrada(ByVal p_DocEntryEntrada As Integer) As Nullable(Of Integer)

        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Dim strConsulta As String = ""
        Dim cmdAsiento As New SqlClient.SqlCommand
        Try
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString
            cn_Coneccion.Open()

            cmdAsiento.Connection = cn_Coneccion

            strConsulta = "Select U_As_Entr from [@SCGD_GOODRECEIVE] " & _
                            "where DocEntry = " & p_DocEntryEntrada & ""

            cmdAsiento.Connection = cn_Coneccion

            cmdAsiento.CommandType = CommandType.Text
            cmdAsiento.CommandText = strConsulta

            If cmdAsiento.ExecuteScalar Is DBNull.Value Then

                'cn_Coneccion.Close()

                Return Nothing

            Else

                Dim intUAsEntr As Integer = cmdAsiento.ExecuteScalar

                Return intUAsEntr

            End If


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex
            'Return intErrorAsiento
        Finally
            cn_Coneccion.Close()

        End Try

    End Function

    Public Function BuscarSalida(ByVal p_DocEntrySalida As Integer) As Nullable(Of Integer)

        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Dim strConsulta As String = ""
        Dim cmdAsiento As New SqlClient.SqlCommand
        Try
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString
            cn_Coneccion.Open()

            cmdAsiento.Connection = cn_Coneccion

            strConsulta = "Select U_As_Entr from [@SCGD_GOODRECEIVE] " & _
                            "where DocEntry = " & p_DocEntrySalida & ""

            cmdAsiento.Connection = cn_Coneccion

            cmdAsiento.CommandType = CommandType.Text
            cmdAsiento.CommandText = strConsulta

            If cmdAsiento.ExecuteScalar Is DBNull.Value Then

                'cn_Coneccion.Close()

                Return Nothing

            Else

                Dim intUAsSalida As Integer = cmdAsiento.ExecuteScalar

                Return intUAsSalida

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        Finally
            cn_Coneccion.Close()
        End Try

    End Function

    Private Function CrearDocumentoAsientoEntradaRevertido(ByVal p_AsEntradaMercancia As Integer, Optional ByVal p_EntradaUsado As Long = 0) As Integer

        Dim intErrorAsiento As Integer

        Dim intNumAsiento As String = ""
        Dim objJournalEntries As SAPbobsCOM.JournalEntries
        Dim objJournalEntriesLines As SAPbobsCOM.JournalEntries_Lines
        Dim objItemsAsientoEntrada As New Generic.List(Of ItemsAsientoEntrada)
        Dim objItemAsEntrada As New ItemsAsientoEntrada
        Dim fechaAsiento As Date



        intNumAsiento = p_AsEntradaMercancia

        objJournalEntries = CargarAsiento(CInt(intNumAsiento))

        fechaAsiento = dtFechaDocumento ' m_oJournalEntries.ReferenceDate

        objJournalEntriesLines = objJournalEntries.Lines

        ReferenciaAsientoMemo = objJournalEntries.Reference

        For i As Integer = 0 To objJournalEntriesLines.Count - 1

            objJournalEntriesLines.SetCurrentLine(i)
            With objJournalEntriesLines

                objItemAsEntrada.strCuenta = .AccountCode
                objItemAsEntrada.decValorCredit = .Credit
                objItemAsEntrada.decValorDebit = .Debit

                objItemAsEntrada.decFvalorCredit = .FCCredit
                objItemAsEntrada.decFvalorDebit = .FCDebit
                objItemAsEntrada.FCurrency = .FCCurrency

                objItemAsEntrada.fechaDocDate = objJournalEntries.ReferenceDate

                objItemAsEntrada.Dimension1 = .CostingCode
                objItemAsEntrada.Dimension2 = .CostingCode2
                objItemAsEntrada.Dimension3 = .CostingCode3
                objItemAsEntrada.Dimension4 = .CostingCode4
                objItemAsEntrada.Dimension5 = .CostingCode5

                objItemsAsientoEntrada.Add(objItemAsEntrada)

            End With

        Next

        intErrorAsiento = CrearAsiento(intNumAsiento, objItemsAsientoEntrada, fechaAsiento)

        If blnAsientoEntradaMercancia And blnProvieneEntradaMercancia Then
            'Se comento por cambio en reversion de salida de mercancia 10/10/2012

            Call ActualizarEstadoEntradaMercancia(p_EntradaUsado, True, strFechaDocumento)

        End If


        If intErrorAsiento = 0 Then
            blnAsientoEntradaMercancia = False
            blnProvieneEntradaMercancia = False
            Return 0
        End If

        'limpio la lista 

        objItemsAsientoEntrada.Clear()

    End Function


    Private Function CrearDocumentoAsientoAjusteCostoReversion(ByVal p_AsientoAjusteCosto As Integer) As Integer

        Dim intNumAsiento As String = ""
        Dim objJournalEntries As SAPbobsCOM.JournalEntries
        Dim objJournalEntriesLines As SAPbobsCOM.JournalEntries_Lines
        Dim objItemsAsientoEntrada As New Generic.List(Of ItemsAsientoEntrada)
        Dim objItemAsEntrada As New ItemsAsientoEntrada
        Dim fechaAsiento As Date

        intNumAsiento = p_AsientoAjusteCosto

        objJournalEntries = CargarAsiento(CInt(intNumAsiento))

        fechaAsiento = dtFechaDocumento

        objJournalEntriesLines = objJournalEntries.Lines

        ReferenciaAsientoMemo = objJournalEntries.Reference

        For i As Integer = 0 To objJournalEntriesLines.Count - 1

            objJournalEntriesLines.SetCurrentLine(i)
            With objJournalEntriesLines

                objItemAsEntrada.strCuenta = .AccountCode
                objItemAsEntrada.decValorCredit = .Credit
                objItemAsEntrada.decValorDebit = .Debit

                objItemAsEntrada.decFvalorCredit = .FCCredit
                objItemAsEntrada.decFvalorDebit = .FCDebit
                objItemAsEntrada.FCurrency = .FCCurrency

                objItemAsEntrada.fechaDocDate = objJournalEntries.ReferenceDate
                objItemsAsientoEntrada.Add(objItemAsEntrada)

            End With

        Next

        Call CrearAsiento(intNumAsiento, objItemsAsientoEntrada, fechaAsiento)

        'limpio la lista 
        objItemsAsientoEntrada.Clear()

    End Function

    Private Function CrearDocumentoAsientoBonosReversion(ByVal p_Asiento As Integer) As Integer

        Dim intNumAsiento As String = ""
        Dim objJournalEntries As SAPbobsCOM.JournalEntries
        Dim objJournalEntriesLines As SAPbobsCOM.JournalEntries_Lines
        Dim objItemsAsientoEntrada As New Generic.List(Of ItemsAsientoEntrada)
        Dim objItemAsEntrada As New ItemsAsientoEntrada
        Dim fechaAsiento As Date

        intNumAsiento = p_Asiento

        objJournalEntries = CargarAsiento(CInt(intNumAsiento))

        fechaAsiento = dtFechaDocumento

        objJournalEntriesLines = objJournalEntries.Lines

        ReferenciaAsientoMemo = objJournalEntries.Reference

        For i As Integer = 0 To objJournalEntriesLines.Count - 1

            objJournalEntriesLines.SetCurrentLine(i)
            With objJournalEntriesLines

                objItemAsEntrada.strCuenta = .AccountCode
                objItemAsEntrada.decValorCredit = .Credit
                objItemAsEntrada.decValorDebit = .Debit

                objItemAsEntrada.decFvalorCredit = .FCCredit
                objItemAsEntrada.decFvalorDebit = .FCDebit
                objItemAsEntrada.FCurrency = .FCCurrency

                objItemAsEntrada.fechaDocDate = objJournalEntries.ReferenceDate
                objItemsAsientoEntrada.Add(objItemAsEntrada)

            End With

        Next

        Call CrearAsiento(intNumAsiento, objItemsAsientoEntrada, fechaAsiento)

        'limpio la lista 
        objItemsAsientoEntrada.Clear()

    End Function

    Public Function CrearAsiento(ByVal p_intDocEntryAsientoReversar As Integer, ByVal p_lista As IList, ByVal p_fechaAsiento As Date) As Integer

        Dim oJournalEntry As SAPbobsCOM.JournalEntries

        Dim intError As Integer
        Dim strMensajeError As String = ""
        Dim strNoAsiento As String = ""

        Dim blnPrimeraCuenta As Boolean = True

        Try

            oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            oJournalEntry.Memo = My.Resources.Resource.MensajeAsientoReversado & ": " & p_intDocEntryAsientoReversar & " - " & ReferenciaAsientoMemo
            oJournalEntry.ReferenceDate = p_fechaAsiento

            oJournalEntry.UserFields.Fields.Item("U_SCGD_AplVal").Value = "0"

            For Each objlist As ItemsAsientoEntrada In p_lista

                If Not blnPrimeraCuenta Then
                    oJournalEntry.Lines.Add()
                Else
                    blnPrimeraCuenta = False
                End If
                oJournalEntry.Lines.AccountCode = objlist.strCuenta
                oJournalEntry.Lines.Debit = objlist.decValorCredit
                oJournalEntry.Lines.FCDebit = objlist.decFvalorCredit

                oJournalEntry.Lines.Credit = objlist.decValorDebit
                oJournalEntry.Lines.FCCredit = objlist.decFvalorDebit
                If Not String.IsNullOrEmpty(objlist.FCurrency) Then
                    oJournalEntry.Lines.FCCurrency = objlist.FCurrency
                End If
                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

                oJournalEntry.Lines.CostingCode = objlist.Dimension1
                oJournalEntry.Lines.CostingCode2 = objlist.Dimension2
                oJournalEntry.Lines.CostingCode3 = objlist.Dimension3
                oJournalEntry.Lines.CostingCode4 = objlist.Dimension4
                oJournalEntry.Lines.CostingCode5 = objlist.Dimension5


            Next

            Dim Verificar As Integer = oJournalEntry.Add()

            If Verificar <> 0 Then
                strNoAsiento = "0"
                m_oCompany.GetLastError(intError, strMensajeError)
                blnReversaDatosTrazabilidad = False
                Throw New ExceptionsSBO(Verificar, strMensajeError)

                m_blnDocumentoReversionNoCreado = True

            Else
                If blnProvieneEntradaMercancia Then
                    intAsientoReversadoEntradaMercancia = m_oCompany.GetNewObjectKey
                Else
                    intAsientoReversado = m_oCompany.GetNewObjectKey

                    intTempAsientoReversado = intAsientoReversado
                End If
                Return 0
            End If

            Return CInt(strNoAsiento)

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Function

    Private Function CrearNotaDebito_Por_Usado(ByVal p_form As SAPbouiCOM.Form, ByVal p_intDocNumNotaCredito As Integer) As Integer


        Dim intError As Integer
        Dim strMensajeError As String = String.Empty
        Dim oNotaDebito As SAPbobsCOM.Documents
        Dim objNotaCredito As SAPbobsCOM.Documents
        Dim objNotaCreditoLines As SAPbobsCOM.Document_Lines
        Dim intSerieDocumento As Integer = -1
        Dim strTipoInventario As String = String.Empty




        oNotaDebito = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices),  _
                                                                    SAPbobsCOM.Documents)

        objNotaCredito = CargarNotaCredito(CInt(p_intDocNumNotaCredito))


        If Not objNotaCredito Is Nothing Then

            Dim NumContrato As String = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("DocNum", 0)
            NumContrato = NumContrato.TrimEnd(" ")

            objNotaCreditoLines = objNotaCredito.Lines

            strTipoInventario = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_TipIn", 0).Trim
            intSerieDocumento = DMS_Connector.Helpers.GetSerie(strTipoInventario, DMS_Connector.Data_Access.GeneralEnums.scgTipoSeries.NotaDebitoClienteReversionNCUsados, False)

            If intSerieDocumento <> -1 Then
                oNotaDebito.Series = intSerieDocumento
            End If
            oNotaDebito.CardCode = m_oCreditMemo.CardCode
            oNotaDebito.Comments = String.Format(My.Resources.Resource.ComentarioNotaDebito, m_oCreditMemo.DocEntry, NumContrato)
            oNotaDebito.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service
            oNotaDebito.DocumentSubType = BoDocumentSubType.bod_DebitMemo
            oNotaDebito.DocDate = dtFechaDocumento 'objNotaCredito.DocDate
            oNotaDebito.DocCurrency = objNotaCredito.DocCurrency 'm_strMonedaLocal

            '******************INICIO | CABYS **************
            If DMS_Connector.Configuracion.ParamGenAddon.U_CABYS_CR = "Y" Then
                If Not String.IsNullOrEmpty(m_oCreditMemo.UserFields.Fields.Item("U_SCG_IVA2_LugarCons").Value) Then oNotaDebito.UserFields.Fields.Item("U_SCG_IVA2_LugarCons").Value = m_oCreditMemo.UserFields.Fields.Item("U_SCG_IVA2_LugarCons").Value
                If Not String.IsNullOrEmpty(m_oCreditMemo.UserFields.Fields.Item("U_SCG_IVA2_TipoExo").Value) Then oNotaDebito.UserFields.Fields.Item("U_SCG_IVA2_TipoExo").Value = m_oCreditMemo.UserFields.Fields.Item("U_SCG_IVA2_TipoExo").Value
            End If
            '******************FIN | CABYS ******************
            '---------------------------------------Manejo de indicadores: 09/05/2012------------------------------------------------
            'Obtiene el indicador por default para el tipo de documento: Nota de Débito Descuento
            'Nota de Débito [Cliente] [Tipo 2]
            Dim strIndicador As String = Utilitarios.DevuelveCodIndicadores(SBO_Application, "2")

            If Not String.IsNullOrEmpty(strIndicador) Then

                oNotaDebito.Indicator = strIndicador

            End If

            For i As Integer = 0 To objNotaCreditoLines.Count - 1

                objNotaCreditoLines.SetCurrentLine(i)

                With oNotaDebito

                    .PaymentGroupCode = intGroupNum
                    .Lines.Quantity = objNotaCreditoLines.Quantity
                    .Lines.AccountCode = objNotaCreditoLines.AccountCode
                    .Lines.TaxCode = objNotaCreditoLines.TaxCode
                    .Lines.VatGroup = objNotaCreditoLines.VatGroup
                    .Lines.UnitPrice = objNotaCreditoLines.Price


                    'Dim intprecio As Integer = m_oCreditMemo.Lines.Price

                    .Lines.ItemDescription = objNotaCreditoLines.ItemDescription

                    If blnUsaDimensiones Then

                        .Lines.CostingCode = objNotaCreditoLines.CostingCode
                        .Lines.CostingCode2 = objNotaCreditoLines.CostingCode2
                        .Lines.CostingCode3 = objNotaCreditoLines.CostingCode3
                        .Lines.CostingCode4 = objNotaCreditoLines.CostingCode4
                        .Lines.CostingCode5 = objNotaCreditoLines.CostingCode5


                    End If
                    '******************INICIO | CABYS **************
                    If DMS_Connector.Configuracion.ParamGenAddon.U_CABYS_CR = "Y" Then
                        .Lines.UserFields.Fields.Item("U_SCG_IVA2_Act_Econ").Value = objNotaCreditoLines.UserFields.Fields.Item("U_SCG_IVA2_Act_Econ").Value
                        .Lines.UserFields.Fields.Item("U_SCG_IVA2_TipoItem").Value = objNotaCreditoLines.UserFields.Fields.Item("U_SCG_IVA2_TipoItem").Value
                        .Lines.UserFields.Fields.Item("U_SCG_IVA2_CodItem").Value = objNotaCreditoLines.UserFields.Fields.Item("U_SCG_IVA2_CodItem").Value
                    End If
                    '******************FIN | CABYS ******************

                    oNotaDebito.Lines.Add()

                End With
            Next

            If Not objNotaCredito.DocCurrency = m_strMonedaLocal Then
                'es para darle el tipo de cambio de la moneda extrajera
                oNotaDebito.DocRate = objNotaCredito.DocRate
                'oNotaDebito.DocRate = Double.Parse(Valor)

            End If

            Dim Verificar As Integer = oNotaDebito.Add

            If Verificar <> 0 Then

                m_oCompany.GetLastError(intError, strMensajeError)
                SBO_Application.StatusBar.SetText(intError & ": " & My.Resources.Resource.MensajeError & " " & strMensajeError, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                'Throw New Exception(strMensajeError)
                m_blnDocumentoReversionNoCreado = True
                'Return Verificar
            Else
                intNotaDebitoPorVehiculoUsado = m_oCompany.GetNewObjectKey
                Return 0
            End If

        End If

    End Function

    'Private Sub GuardarDatosContratoReversado(ByVal p_FormContrato As SAPbouiCOM.Form, ByVal oMatrixUsado As SAPbouiCOM.Matrix, ByVal oMatrix As SAPbouiCOM.Matrix, ByVal oDataTableFacturas As SAPbouiCOM.DataTable)

    '    Dim strCardCode As String = String.Empty
    '    Dim strCardName As String = String.Empty


    '    'Agregado 14/03/2012
    '    '************************************************

    '    Dim strFechaReversion As String
    '    Dim dtFechareversion As Date


    '    'oItem = p_FormContrato.Items.Item("txtFecDocR").Specific
    '    strFechaReversion = p_FormContrato.Items.Item("txtFecDocR").Specific.value


    '    dtFechareversion = Date.ParseExact(strFechaReversion, "yyyyMMdd", Nothing)

    '    strFechaReversion = String.Empty
    '    strFechaReversion = Utilitarios.RetornaFechaFormatoDB(dtFechareversion, m_oCompany.Server)

    '    If dtFechareversion = Nothing Then
    '        strFechaReversion = Nothing
    '    End If


    '    '************************************************


    '    oItem = p_FormContrato.Items.Item("txtCl")
    '    oEdit = oItem.Specific



    '    If oEdit.Value <> String.Empty Then
    '        strCardCode = oEdit.String
    '    End If

    '    oItem = p_FormContrato.Items.Item("txtDetCl")
    '    oEdit = oItem.Specific

    '    If oEdit.Value <> String.Empty Then
    '        strCardName = oEdit.String
    '    End If

    '    Dim strConectionString As String = ""
    '    Dim cnConeccionBD As SqlClient.SqlConnection

    '    Configuracion.CrearCadenaDeconexion(m_oCompany.Server, _
    '                                         m_oCompany.CompanyDB, _
    '                                         strConectionString)

    '    Dim dtsReversarContratos As New DMS_Addon.ReversarContratoDataSet
    '    Dim dtaReversarContratos As New DMS_Addon.ReversarContratoDataSetTableAdapters.SCG_CV_REVERTIRTableAdapter
    '    Dim dtaRevesarContratosLineas As New DMS_Addon.ReversarContratoDataSetTableAdapters.SCG_CV_REVERLINEATableAdapter

    '    Dim drwReversar As DMS_Addon.ReversarContratoDataSet.SCG_CV_REVERTIRRow
    '    Dim drwReversarLineas As DMS_Addon.ReversarContratoDataSet.SCG_CV_REVERLINEARow


    '    cnConeccionBD = New SqlClient.SqlConnection
    '    cnConeccionBD.ConnectionString = strConectionString
    '    cnConeccionBD.Open()
    '    dtaReversarContratos.Connection = New SqlClient.SqlConnection(strConectionString)
    '    dtaReversarContratos.Connection = cnConeccionBD

    '    dtaRevesarContratosLineas.Connection = New SqlClient.SqlConnection(strConectionString)
    '    dtaRevesarContratosLineas.Connection = cnConeccionBD


    '    drwReversar = dtsReversarContratos.SCG_CV_REVERTIR.NewSCG_CV_REVERTIRRow
    '    drwReversarLineas = dtsReversarContratos.SCG_CV_REVERLINEA.NewSCG_CV_REVERLINEARow

    '    With drwReversar


    '        .DocEntry = dtaReversarContratos.SelectONNM
    '        .DocNum = dtaReversarContratos.SelectONNM

    '        dtaReversarContratos.UpdateONNM(.DocEntry)

    '        .U_CardCo = strCardCode
    '        .U_CardNa = strCardName
    '        .U_NumC = intNumeroContrato
    '        .U_FecRev = Date.Now


    '        Dim intDocEntry As Integer = .DocEntry

    '        With drwReversarLineas

    '            .DocEntry = CStr(intDocEntry)
    '            .LineId = 1

    '            If oDataTableFacturas.Rows.Count = 1 AndAlso Not String.IsNullOrEmpty(oDataTableFacturas.GetValue("DocEntry", 0)) Then

    '                .U_NoFacC = DocNumFactura
    '                .U_NCFRev = intNotaCreditoProvenienteFactura

    '            End If

    '            .U_NoCUsC = DocNotaCreditoUsd
    '            .U_NDURev = intNotaDebitoPorVehiculoUsado

    '            .U_SCGD_AsAj = DocAsientoAjuste
    '            .U_SCGD_AsAjR = DocAsientoAjusteReversion

    '            ' .U_EntMeC = DocEntradaMercancia
    '            If intAsEntradaMercancia Is Nothing Then

    '                .U_EntMeC = 0

    '            Else

    '                If oMatrixUsado.RowCount > 1 Then

    '                    .U_EntMeC = My.Resources.Resource.VariosUsados
    '                    '.U_EntMeC = 0

    '                Else

    '                    .U_EntMeC = intAsEntradaMercancia

    '                End If

    '            End If

    '            If oMatrixUsado.RowCount > 1 Then

    '                .U_AsERev = My.Resources.Resource.VariosUsados
    '                '.U_AsERev = 0

    '            Else

    '                .U_AsERev = intAsientoReversadoEntradaMercancia

    '            End If

    '            If intAsientoReversado Is Nothing Then
    '                .U_SCGD_SaCoVeh = 0
    '            Else

    '                If oMatrix.RowCount > 1 Then

    '                    .U_SCGD_SaCoVeh = My.Resources.Resource.UnidadesMultiples

    '                Else

    '                    .U_SCGD_SaCoVeh = intAsientoReversado

    '                End If

    '            End If

    '            .U_NumC = intNumeroContrato


    '            .U_SCGD_SalMerc = DocSalidaMercancia

    '            .U_Prestamo = strPrestamo

    '            .U_AsRevPre = strAsientoRevPrestamo

    '            .U_NC_Pri = strPrima

    '            .U_DocRePri = strAsientoReversaPrima

    '            .U_FactAcc = DocFactAccs

    '            .U_RevAcc = intNotaCreditoProvFactAccs

    '            .U_FactGas = DocFactGastos

    '            .U_RevGas = intNotaCreditoProvFactGastos

    '            .U_As_FiExt = intAsientoFinExt

    '            .U_RevFiExt = strAsientoReversaFinExt

    '            .U_As_Tram = intAsientoTramite

    '            .U_Rev_Tram = strAsientoReversaTramite

    '            .U_FPDeuUs = DocFactDeudores

    '            .U_NCFP_DU = intNotaCredito_FacturaProveedorDeudaUsado

    '            .U_FCDeu_Us = DocNotaDebitoUsd

    '            .U_NCFC_DU = intNotaCredito_FacturaClienteDeudaUsado

    '            .U_NCxDesc = DocNotaCreditoxDesc

    '            .U_NDxDesR = intNotaDebitoxDescuento

    '            .U_AsBon = strAsientoReversaBonos

    '            .U_AsCom = strAsientoReversaComisiones

    '            .U_AsOCos = strAsientoReversaOtrosCostos

    '        End With

    '    End With

    '    dtsReversarContratos.SCG_CV_REVERTIR.AddSCG_CV_REVERTIRRow(drwReversar)
    '    dtsReversarContratos.SCG_CV_REVERLINEA.AddSCG_CV_REVERLINEARow(drwReversarLineas)

    '    dtaReversarContratos.Update(dtsReversarContratos)
    '    dtaRevesarContratosLineas.Update(dtsReversarContratos)


    '    ActualizarCampoReversadoContrato(intNumeroContrato, strFechaReversion)


    'End Sub

    'Private Sub ActualizarCampoReversadoContrato(ByVal p_intNumContrato As Integer, ByVal p_strFecRev As String)

    '    Dim strConectionString As String = ""
    '    Dim cn_Coneccion As New SqlClient.SqlConnection
    '    Dim strConsulta As String = ""
    '    Dim cmdAsiento As New SqlClient.SqlCommand
    '    'Dim str_formatHora As String = " 00:00:00.000"

    '    Try
    '        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
    '        cn_Coneccion.ConnectionString = strConectionString
    '        cn_Coneccion.Open()

    '        cmdAsiento.Connection = cn_Coneccion

    '        strConsulta = "UPDATE [@SCGD_CVENTA] SET [U_Reversa] = 'Y', [U_SCGD_FDr] = '" & p_strFecRev & "' WHERE [DocEntry] = " & p_intNumContrato & ""


    '        cmdAsiento.Connection = cn_Coneccion
    '        cmdAsiento.CommandType = CommandType.Text
    '        cmdAsiento.CommandText = strConsulta
    '        cmdAsiento.ExecuteNonQuery()
    '        cn_Coneccion.Close()

    '    Catch ex As Exception
    '        Call Utilitarios.ManejadorErrores(ex, SBO_Application)
    '    End Try


    'End Sub



    Private Sub GuardarDatosContratoReversado(ByVal p_FormContrato As SAPbouiCOM.Form, ByVal oMatrixUsado As SAPbouiCOM.Matrix, ByVal oMatrix As SAPbouiCOM.Matrix, ByVal oDataTableFacturas As SAPbouiCOM.DataTable)

        Dim strCardCode As String = String.Empty
        Dim strCardName As String = String.Empty


        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildRevertir As SAPbobsCOM.GeneralData
        Dim oChildrenRevertir As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams


        'Agregado 14/03/2012
        '************************************************






        oItem = p_FormContrato.Items.Item("txtCl")
        oEdit = oItem.Specific

        If oEdit.Value <> String.Empty Then
            strCardCode = oEdit.String
        End If

        oItem = p_FormContrato.Items.Item("txtDetCl")
        oEdit = oItem.Specific

        If oEdit.Value <> String.Empty Then
            strCardName = oEdit.String
        End If

        oCompanyService = m_oCompany.GetCompanyService()
        oGeneralService = oCompanyService.GetGeneralService("SCGD_ContRevertir")
        oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)


        If Not String.IsNullOrEmpty(strCardCode) Then
            oGeneralData.SetProperty("U_CardCo", strCardCode)
        End If
        If Not String.IsNullOrEmpty(strCardName) Then
            oGeneralData.SetProperty("U_CardNa", strCardName)
        End If

        oGeneralData.SetProperty("U_NumC", intNumeroContrato.ToString())
        oGeneralData.SetProperty("U_FecRev", Date.Now)

        oChildrenRevertir = oGeneralData.Child("SCGD_CV_REVERLINEA")
        oChildRevertir = oChildrenRevertir.Add()

        If oDataTableFacturas.Rows.Count = 1 AndAlso Not String.IsNullOrEmpty(oDataTableFacturas.GetValue("DocEntry", 0)) Then
            oChildRevertir.SetProperty("U_NoFacC", DocNumFactura.ToString())
            oChildRevertir.SetProperty("U_NCFRev", intNotaCreditoProvenienteFactura.ToString())
        End If

        oChildRevertir.SetProperty("U_NoCUsC", DocNotaCreditoUsd.ToString())
        oChildRevertir.SetProperty("U_NDURev", intNotaDebitoPorVehiculoUsado.ToString())
        oChildRevertir.SetProperty("U_SCGD_AsAj", DocAsientoAjuste.ToString())
        oChildRevertir.SetProperty("U_SCGD_AsAjR", DocAsientoAjusteReversion.ToString())
        If intAsEntradaMercancia Is Nothing Then
            oChildRevertir.SetProperty("U_EntMeC", "0")
        Else
            If oMatrixUsado.RowCount > 1 Then
                oChildRevertir.SetProperty("U_EntMeC", My.Resources.Resource.VariosUsados)
            Else
                oChildRevertir.SetProperty("U_EntMeC", intAsEntradaMercancia.ToString())
            End If
        End If
        If oMatrixUsado.RowCount > 1 Then
            oChildRevertir.SetProperty("U_AsERev", My.Resources.Resource.VariosUsados)
        Else
            oChildRevertir.SetProperty("U_AsERev", intAsientoReversadoEntradaMercancia.ToString())
        End If

        If intAsientoReversado Is Nothing Then
            oChildRevertir.SetProperty("U_SCGD_SaCoVeh", "0")
        Else
            If oMatrix.RowCount > 1 Then
                oChildRevertir.SetProperty("U_SCGD_SaCoVeh", My.Resources.Resource.UnidadesMultiples)
            Else
                oChildRevertir.SetProperty("U_SCGD_SaCoVeh", intAsientoReversado.ToString())
            End If
        End If

        oChildRevertir.SetProperty("U_NumC", intNumeroContrato.ToString())
        oChildRevertir.SetProperty("U_SCGD_SalMerc", DocSalidaMercancia.ToString())
        If Not String.IsNullOrEmpty(strPrestamo) Then
            oChildRevertir.SetProperty("U_Prestamo", strPrestamo)
        End If
        If Not String.IsNullOrEmpty(strAsientoRevPrestamo) Then
            oChildRevertir.SetProperty("U_AsRevPre", strAsientoRevPrestamo)
        End If
        If Not String.IsNullOrEmpty(strPrima) Then
            oChildRevertir.SetProperty("U_NC_Pri", strPrima)
        End If
        If Not String.IsNullOrEmpty(strAsientoReversaPrima) Then
            oChildRevertir.SetProperty("U_DocRePri", strAsientoReversaPrima)
        End If
        oChildRevertir.SetProperty("U_FactAcc", DocFactAccs.ToString())
        oChildRevertir.SetProperty("U_RevAcc", intNotaCreditoProvFactAccs.ToString())
        oChildRevertir.SetProperty("U_FactGas", DocFactGastos.ToString())
        oChildRevertir.SetProperty("U_RevGas", intNotaCreditoProvFactGastos.ToString())
        oChildRevertir.SetProperty("U_As_FiExt", intAsientoFinExt.ToString())

        If Not String.IsNullOrEmpty(strAsientoReversaFinExt) Then
            oChildRevertir.SetProperty("U_RevFiExt", strAsientoReversaFinExt)
        End If

        oChildRevertir.SetProperty("U_As_Tram", intAsientoTramite.ToString())

        If Not String.IsNullOrEmpty(strAsientoReversaTramite) Then
            oChildRevertir.SetProperty("U_Rev_Tram", strAsientoReversaTramite)
        End If

        oChildRevertir.SetProperty("U_FPDeuUs", DocFactDeudores.ToString())
        oChildRevertir.SetProperty("U_NCFP_DU", intNotaCredito_FacturaProveedorDeudaUsado.ToString())
        oChildRevertir.SetProperty("U_FCDeu_Us", DocNotaDebitoUsd.ToString())
        oChildRevertir.SetProperty("U_NCFC_DU", intNotaCredito_FacturaClienteDeudaUsado.ToString())
        oChildRevertir.SetProperty("U_NCxDesc", DocNotaCreditoxDesc.ToString())
        oChildRevertir.SetProperty("U_NDxDesR", intNotaDebitoxDescuento.ToString())

        If Not String.IsNullOrEmpty(strAsientoReversaBonos) Then
            oChildRevertir.SetProperty("U_AsBon", strAsientoReversaBonos)
        End If

        If Not String.IsNullOrEmpty(strAsientoReversaComisiones) Then
            oChildRevertir.SetProperty("U_AsCom", strAsientoReversaComisiones)
        End If

        If Not String.IsNullOrEmpty(strAsientoReversaOtrosCostos) Then
            oChildRevertir.SetProperty("U_AsOCos", strAsientoReversaOtrosCostos)
        End If

        If Not String.IsNullOrEmpty(intAsientoTramitesFacturables.ToString()) Then
            oChildRevertir.SetProperty("U_AsRTrFac", intAsientoTramitesFacturables.ToString())
        End If
        If Not String.IsNullOrEmpty(strAsientoReversaTramitesFacturables) Then
            oChildRevertir.SetProperty("U_AsRTrFac", strAsientoReversaTramitesFacturables)
        End If


        If Not String.IsNullOrEmpty(DocFacturaTramites.ToString()) Then
            oChildRevertir.SetProperty("U_FacTram", DocFacturaTramites.ToString())
        End If
        If Not String.IsNullOrEmpty(intNotaCredito_FacturaTramites.ToString()) Then
            oChildRevertir.SetProperty("U_NCTraFac", intNotaCredito_FacturaTramites.ToString())
        End If

        If oMatrixUsado.RowCount > 1 Then

            oChildRevertir.SetProperty("U_FactPVU", My.Resources.Resource.VariosUsados)
            oChildRevertir.SetProperty("U_NCFacPVH", My.Resources.Resource.VariosUsados)
            oChildRevertir.SetProperty("U_AsAdFPVU", My.Resources.Resource.VariosUsados)
            oChildRevertir.SetProperty("U_AsAdRev", My.Resources.Resource.VariosUsados)
        Else

            If strFacturaProveedorVehiculoUsado <> Nothing Then
                If Not String.IsNullOrEmpty(strFacturaProveedorVehiculoUsado.ToString()) Then

                    oChildRevertir.SetProperty("U_FactPVU", strFacturaProveedorVehiculoUsado.ToString())
                End If
            End If

            If strNotaCreditoPFVehiculoUsado <> Nothing Then
                If Not String.IsNullOrEmpty(strNotaCreditoPFVehiculoUsado.ToString()) Then
                    oChildRevertir.SetProperty("U_NCFacPVH", strNotaCreditoPFVehiculoUsado.ToString())
                End If

            End If

            If strAsientoAdicionalFPVU <> Nothing Then
                If Not String.IsNullOrEmpty(strAsientoAdicionalFPVU.ToString()) Then
                    oChildRevertir.SetProperty("U_AsAdFPVU", strAsientoAdicionalFPVU.ToString())
                End If
            End If

            If strAsientoReversaAdicionalFPU <> Nothing Then
                If Not String.IsNullOrEmpty(strAsientoReversaAdicionalFPU.ToString()) Then
                    oChildRevertir.SetProperty("U_AsAdRev", strAsientoReversaAdicionalFPU.ToString())
                End If
            End If



        End If


        Dim resultado As Integer
        oGeneralService.Add(oGeneralData)

        ActualizarCampoReversadoContrato(intNumeroContrato, p_FormContrato)
    End Sub

    Private Sub ActualizarCampoReversadoContrato(ByVal p_intNumContrato As Integer, ByRef p_FormContrato As SAPbouiCOM.Form)

        Dim strConsulta As String = ""
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        'Dim str_formatHora As String = " 00:00:00.000"

        Try
            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_CVT")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", p_intNumContrato)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            Dim strFechaCita = p_FormContrato.DataSources.DBDataSources.Item("@SCGD_CVENTA").GetValue("U_SCGD_FDr", 0)

            Dim dtFecha As Date = New Date(CInt(strFechaCita.Substring(0, 4)), CInt(strFechaCita.Substring(4, 2)), CInt(strFechaCita.Substring(6, 2)), 0, 0, 0)


            oGeneralData.SetProperty("U_Reversa", "Y")
            oGeneralData.SetProperty("U_SCGD_FDr", dtFecha)

            oGeneralService.Update(oGeneralData)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try


    End Sub

    Private Sub ActualizarEstadoEntradaMercancia(ByVal p_intEntrada As Long, ByVal blnTieneAsientoEntrada As Boolean, Optional ByVal docFechaReversion As String = Nothing)

        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Dim strConsulta As String = ""
        Dim cmdAsiento As New SqlClient.SqlCommand

        Try
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString
            cn_Coneccion.Open()
            cmdAsiento.Connection = cn_Coneccion

            If blnTieneAsientoEntrada Then
                strConsulta = "UPDATE [@SCGD_GOODRECEIVE] SET [Status] = 'C' ,[U_As_Entr] = -1, [U_FecERv] = '" & docFechaReversion & "', [U_RevCV] = 'Y' WHERE [DocEntry] = " & p_intEntrada & ""
            Else
                strConsulta = "UPDATE [@SCGD_GOODRECEIVE] SET [Status] = 'C' ,[U_As_Entr] = -1, [U_FecERv] = '" & docFechaReversion & "', [U_RevCV] = 'Y'  WHERE [DocEntry] = " & p_intEntrada & ""
            End If

            cmdAsiento.Connection = cn_Coneccion
            cmdAsiento.CommandType = CommandType.Text
            cmdAsiento.CommandText = strConsulta
            cmdAsiento.ExecuteNonQuery()
            cn_Coneccion.Close()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Private Function BuscarNCReferenciaFacturaActual(ByVal p_docNumFactura As Long) As Boolean

        Dim objDocumentoNC As SAPbobsCOM.Documents = Nothing
        Dim objFactura As SAPbobsCOM.Documents
        Dim objFacturaLines As SAPbobsCOM.Document_Lines

        objFactura = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices),  _
        SAPbobsCOM.Documents)

        If objFactura.GetByKey(p_docNumFactura) Then

            objFacturaLines = objFactura.Lines

            For i As Integer = 0 To objFacturaLines.Count - 1

                objFacturaLines.SetCurrentLine(i)

                Dim intLineNum As Integer = objFacturaLines.LineNum

                Dim objUtilitarios As New Utilitarios
                Dim baseDatos As String
                baseDatos = SBO_Application.Company.DatabaseName
                Dim Server As String
                Server = SBO_Application.Company.ServerName

                ' DocNumFactura = CLng(oEdit.String)

                'se valida si existe algun pago de la factura
                Dim StrConsulta As String = "SELECT INV1.TrgetEntry " & _
                                            "FROM OINV INNER JOIN " & _
                                            "INV1 ON OINV.DocEntry = INV1.DocEntry " & _
                                            "WHERE (OINV.DocEntry = " & objFactura.DocEntry & ") AND (OINV.DocType = 'I') AND (INV1.LineNum = " & intLineNum & ")"

                Dim strValorConsulta As String = Utilitarios.EjecutarConsulta(StrConsulta, baseDatos, Server)

                If Not strValorConsulta = String.Empty Then
                    Return True
                End If

            Next

        End If

    End Function

    Public Function CrearNCparaVehiculoUsado(ByVal p_form As SAPbouiCOM.Form, ByVal p_intDocNumNotaCredito As Integer) As Integer

        Dim intError As Integer
        Dim strMensajeError As String = String.Empty
        Dim oNotaCreditoUsadoContrato As SAPbobsCOM.Documents
        Dim oNotaCreditoUsadoContratoLines As SAPbobsCOM.Document_Lines

        Dim objNCReversado As SAPbobsCOM.Documents

        objNCReversado = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes),  _
                                                                        SAPbobsCOM.Documents)

        oNotaCreditoUsadoContrato = CargarNotaCredito(CInt(p_intDocNumNotaCredito))


        If Not oNotaCreditoUsadoContrato Is Nothing Then

            Dim NumContrato As String = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("DocNum", 0)
            NumContrato = NumContrato.TrimEnd(" ")

            oNotaCreditoUsadoContratoLines = oNotaCreditoUsadoContrato.Lines
            objNCReversado.Series = oNotaCreditoUsadoContrato.Series
            objNCReversado.CardCode = m_oCreditMemo.CardCode
            objNCReversado.Comments = String.Format(My.Resources.Resource.ReversionUsadoNC, m_oCreditMemo.DocEntry, NumContrato)
            objNCReversado.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service
            'objNCReversado.DocObjectCode = BoObjectTypes.oCreditNotes
            objNCReversado.DocDate = oNotaCreditoUsadoContrato.DocDate

            objNCReversado.DocCurrency = m_strMonedaLocal

            '---------------------------------------Manejo de indicadores: 09/05/2012------------------------------------------------
            'Obtiene el indicador por default para el tipo de documento: Nota de Crédito Descuento
            'Nota de Crédito Descuento [Cliente] [Tipo 3]
            Dim strIndicador As String = Utilitarios.DevuelveCodIndicadores(SBO_Application, "3")

            If Not String.IsNullOrEmpty(strIndicador) Then

                objNCReversado.Indicator = strIndicador

            End If

            For i As Integer = 0 To oNotaCreditoUsadoContratoLines.Count - 1

                oNotaCreditoUsadoContratoLines.SetCurrentLine(i)

                With objNCReversado

                    .PaymentGroupCode = intGroupNum
                    .Lines.Quantity = -1
                    ' Dim cantidad As Integer = -1
                    .Lines.AccountCode = oNotaCreditoUsadoContratoLines.AccountCode
                    .Lines.TaxCode = oNotaCreditoUsadoContratoLines.TaxCode
                    .Lines.VatGroup = oNotaCreditoUsadoContratoLines.VatGroup
                    'Dim precio As Integer = oNotaCreditoUsadoContrato.Lines.Price * -1
                    ''.Lines.Quantity = -1
                    .Lines.LineTotal = oNotaCreditoUsadoContratoLines.Price * -1
                    '.Lines.UnitPrice = oNotaCreditoUsadoContratoLines.Price * -1
                    '.Lines.Price = oNotaCreditoUsadoContratoLines.Price * -1
                    'Dim intprecio As Integer = m_oCreditMemo.Lines.Price
                    .Lines.ItemDescription = oNotaCreditoUsadoContratoLines.ItemDescription

                    If blnUsaDimensiones Then

                        .Lines.CostingCode = oNotaCreditoUsadoContratoLines.CostingCode
                        .Lines.CostingCode2 = oNotaCreditoUsadoContratoLines.CostingCode2
                        .Lines.CostingCode3 = oNotaCreditoUsadoContratoLines.CostingCode3
                        .Lines.CostingCode4 = oNotaCreditoUsadoContratoLines.CostingCode4
                        .Lines.CostingCode5 = oNotaCreditoUsadoContratoLines.CostingCode5


                    End If

                    objNCReversado.Lines.Add()

                End With
            Next

            Dim Verificar As Integer = objNCReversado.Add

            If Verificar <> 0 Then

                m_oCompany.GetLastError(intError, strMensajeError)
                SBO_Application.StatusBar.SetText(intError & ": " & My.Resources.Resource.MensajeError & " " & strMensajeError, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                'Throw New Exception(strMensajeError)
                m_blnDocumentoReversionNoCreado = True

                Return Verificar
            Else
                intNotaDebitoPorVehiculoUsado = m_oCompany.GetNewObjectKey
                Return 0
            End If





        End If


    End Function

    Private Function ReversarNotaCreditoxDescuento(ByVal p_form As SAPbouiCOM.Form, ByVal p_intDocNumNotaCreditoxDescuento As Integer) As Integer


        Dim intError As Integer
        Dim strMensajeError As String = String.Empty
        Dim oNotaDebito As SAPbobsCOM.Documents
        Dim objNotaCredito As SAPbobsCOM.Documents
        Dim objNotaCreditoLines As SAPbobsCOM.Document_Lines
        Dim strTipoInventario As String = String.Empty
        Dim intSerieDocumento As Integer = -1

        oNotaDebito = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices),  _
                                                                        SAPbobsCOM.Documents)

        objNotaCredito = CargarNotaCredito(CInt(p_intDocNumNotaCreditoxDescuento))


        If Not objNotaCredito Is Nothing Then

            If objNotaCredito.DocumentStatus = BoStatus.bost_Open Then


                Dim NumContrato As String = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("DocNum", 0)
                NumContrato = NumContrato.TrimEnd(" ")

                objNotaCreditoLines = objNotaCredito.Lines

                strTipoInventario = p_form.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_TipIn", 0).Trim
                intSerieDocumento = DMS_Connector.Helpers.GetSerie(strTipoInventario, DMS_Connector.Data_Access.GeneralEnums.scgTipoSeries.NotaDebitoReversionNCDescuento, False)

                If intSerieDocumento <> -1 Then
                    oNotaDebito.Series = intSerieDocumento
                End If


                oNotaDebito.CardCode = m_oCreditMemo.CardCode
                oNotaDebito.Comments = String.Format(My.Resources.Resource.ComentarioNotaDebito, m_oCreditMemo.DocEntry, NumContrato)
                oNotaDebito.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service
                oNotaDebito.DocumentSubType = BoDocumentSubType.bod_DebitMemo
                oNotaDebito.DocDate = dtFechaDocumento 'objNotaCredito.DocDate
                oNotaDebito.DocCurrency = objNotaCredito.DocCurrency 'm_strMonedaLocal

                '---------------------------------------Manejo de indicadores: 09/05/2012------------------------------------------------
                'Obtiene el indicador por default para el tipo de documento: Nota de Débito Descuento
                'Nota de Débito [Cliente] [Tipo 2]
                Dim strIndicador As String = Utilitarios.DevuelveCodIndicadores(SBO_Application, "2")

                If Not String.IsNullOrEmpty(strIndicador) Then

                    oNotaDebito.Indicator = strIndicador

                End If

                For i As Integer = 0 To objNotaCreditoLines.Count - 1

                    objNotaCreditoLines.SetCurrentLine(i)

                    With oNotaDebito

                        .PaymentGroupCode = intGroupNum
                        .Lines.Quantity = objNotaCreditoLines.Quantity
                        .Lines.AccountCode = objNotaCreditoLines.AccountCode
                        .Lines.TaxCode = objNotaCreditoLines.TaxCode
                        .Lines.VatGroup = objNotaCreditoLines.VatGroup
                        .Lines.UnitPrice = objNotaCreditoLines.Price

                        'Dim intprecio As Integer = m_oCreditMemo.Lines.Price

                        .Lines.ItemDescription = objNotaCreditoLines.ItemDescription

                        oNotaDebito.Lines.Add()

                    End With
                Next

                If Not objNotaCredito.DocCurrency = m_strMonedaLocal Then
                    'es para darle el tipo de cambio de la moneda extrajera
                    oNotaDebito.DocRate = objNotaCredito.DocRate
                    'oNotaDebito.DocRate = Double.Parse(Valor)

                End If

                Dim Verificar As Integer = oNotaDebito.Add

                If Verificar <> 0 Then

                    m_oCompany.GetLastError(intError, strMensajeError)
                    SBO_Application.StatusBar.SetText(intError & ": " & My.Resources.Resource.MensajeError & " " & strMensajeError, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                    'Throw New Exception(strMensajeError)
                    m_blnDocumentoReversionNoCreado = True
                    Return Verificar
                Else
                    intNotaDebitoxDescuento = m_oCompany.GetNewObjectKey
                    Return 0
                End If
            End If

        End If


    End Function




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

    Public Function CargarNotaCredito(ByVal p_intNotaCredito As Integer) As SAPbobsCOM.Documents

        Try
            m_oCreditMemo = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes), SAPbobsCOM.Documents)

            If m_oCreditMemo.GetByKey(p_intNotaCredito) Then

                If Not m_oCreditMemo.DocumentStatus = BoStatus.bost_Close Then

                    Return m_oCreditMemo

                End If

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex

        End Try

        Return Nothing

    End Function

#End Region

#Region "Cargar Formulario con los resultados de la Reversion"

    Protected Friend Sub DibujarFormularioResultadoReversion(ByVal p_NotaCredito As Integer, ByVal p_NotaDebito As Integer, ByVal p_Asiento As Integer)

        Dim strXMLACargar As String

        Dim fcp As SAPbouiCOM.FormCreationParams

        fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
        fcp.UniqueID = "SCGD_ContRevers"
        fcp.FormType = "SCGD_ContRevers"
        'fcp.ObjectType = "SCG_REVERSION"

        strXMLACargar = "frmReversionSBO.xml"
        fcp.XmlData = CargarDesdeXML(strXMLACargar)

        oForm = SBO_Application.Forms.AddEx(fcp)

        oEdit = oForm.Items.Item("txtNCRF").Specific
        oEdit.String = intNotaCreditoProvenienteFactura

        oEdit = oForm.Items.Item("txtNDRNCU").Specific
        oEdit.String = intNotaDebitoPorVehiculoUsado

        oEdit = oForm.Items.Item("txtAR").Specific
        oEdit.String = intAsientoReversadoEntradaMercancia

    End Sub

    Private Function CargarDesdeXML(ByRef strFileName As String) As String

        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String

        strPath = System.Windows.Forms.Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml

    End Function

    Private Sub CargarTipoCambio(ByVal p_oform As SAPbouiCOM.Form)

        Dim strMoneda As String
        Dim strConectionString As String = String.Empty
        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)

        Dim m_objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        strMoneda = p_oform.DataSources.DBDataSources.Item(mc_strTablaContratoVenta).GetValue("U_Moneda", 0)

        m_objBLSBO.Set_Compania(m_oCompany)
        m_strMonedaLocal = m_objBLSBO.RetornarMonedaLocal()
        If m_strMonedaLocal <> Trim(strMoneda) Then
            m_decTipoCambio = m_objBLSBO.RetornarTipoCambioMoneda(strMoneda, m_objUtilitarios.CargarFechaHoraServidor(), strConectionString, False)
            If m_decTipoCambio = -1 Then
                Throw New Exception(My.Resources.Resource.TipoCambioNoActualizado)
            End If
        Else
            m_decTipoCambio = 1
        End If

    End Sub


#End Region

#Region "codigo"
    'oItem = p_form.Items.Item("txtNofac")
    'oEdit = oItem.Specific
    'If oEdit.Value <> String.Empty Then
    '    DocNumFactura = CLng(oEdit.String)
    'End If




    'codigo para investigar sobre la clase GeneralService

    'Dim oGeneralService As SAPbobsCOM.GeneralService
    'Dim oGeneralData As SAPbobsCOM.GeneralData
    ''Dim oChild As SAPbobsCOM.GeneralData
    ''Dim oChildren As SAPbobsCOM.GeneralDataCollection
    'Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

    ''Get GeneralService (oCmpSrv is the CompanyService)
    ''  m_oCompany.GetCompanyService()

    'oGeneralService = m_oCompany.GetGeneralService("@SCG_CV_REVERSADOS")

    ''Create data for new row in main UDO
    'oGeneralData = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData)
    'oGeneralData.SetProperty("Code", "First")
    'oGeneralData.SetProperty("U_NumContrato", intNumeroContrato)
    'oGeneralData.SetProperty("U_NC_Ref_Factura", intNotaCreditoProvenienteFactura)
    'oGeneralData.SetProperty("U_ND_Ref_NotCredUsad", intNotaDebitoPorVehiculoUsado)
    'oGeneralData.SetProperty("U_As_por_EMercancia", intAsientoReversado)
    'oGeneralService.Add(oGeneralData)

    'oTmpSalvarDatosReversion.DataSources.DBDataSources.Item("@SCG_CV_REVERSADOS").SetValue("U_NumContrato", 0, intNumeroContrato)
    'oTmpSalvarDatosReversion.DataSources.DBDataSources.Item("@SCG_CV_REVERSADOS").SetValue("U_NC_Ref_Factura", 0, intNotaCreditoProvenienteFactura)
    'oTmpSalvarDatosReversion.DataSources.DBDataSources.Item("@SCG_CV_REVERSADOS").SetValue("U_ND_Ref_NotCredUsad", 0, intNotaDebitoPorVehiculoUsado)
    'oTmpSalvarDatosReversion.DataSources.DBDataSources.Item("@SCG_CV_REVERSADOS").SetValue("U_As_por_EMercancia", 0, intAsientoReversado)

    'Private Function CargarFactura(ByVal p_NumFactura As Integer) As SAPbobsCOM.Documents

    '    Dim a As Integer
    '    Try
    '        m_oInvoice = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices),  _
    '                                                            SAPbobsCOM.Documents)
    '        If m_oInvoice.GetByKey(p_NumFactura) Then

    '            If Not m_oInvoice.DocumentStatus = BoStatus.bost_Close Then

    '                Return m_oInvoice
    '            Else

    '                Dim StrConsulta As String = "SELECT RCT2.DocNum AS DeLineasPago " & _
    '                                            "FROM INV1 INNER JOIN " & _
    '                                            "OINV ON INV1.DocEntry = " & p_NumFactura & " AND OINV.DocEntry = " & p_NumFactura & " INNER JOIN " & _
    '                                            "RCT2 ON INV1.DocEntry = RCT2.DocEntry INNER JOIN " & _
    '                                            "ORCT ON RCT2.DocNum = ORCT.DocEntry " & _
    '                                            "WHERE     (ORCT.Canceled = 'N')"

    '                '    a = m_oInvoice.DocEntry
    '                '    'vienen algunas validaciones:
    '                '    '1 - La factura cerrada por pagos
    '                '    '2 - La factura cerrada por Nota de Credito
    '                '    '3 - La factura cerrada por cancelacion

    '                Dim objUtilitarios As New Utilitarios
    '                Dim baseDatos As String
    '                baseDatos = SBO_Application.Company.DatabaseName
    '                Dim Server As String
    '                Server = SBO_Application.Company.ServerName

    '                Dim ver As String = Utilitarios.EjecutarConsulta(StrConsulta, baseDatos, Server)
    '                SBO_Application.StatusBar.SetText("Existen pagos que deberan ser cancelados manualmente antes de continuar con la reversion ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

    '            End If

    '        End If

    '        'Dim m As SAPbobsCOM.Payments
    '        'Dim pa As SAPbobsCOM.Payments_Invoices
    '        'Dim ot As SAPbobsCOM.Payments_Invoices


    '        'm = CType(m_oCompany.GetBusinessObject(BoObjectTypes.oIncomingPayments), SAPbobsCOM.Payments)

    '        'If m.GetByKey(5345) Then

    '        '    If m.Cancelled = BoYesNoEnum.tNO Then

    '        '        ot = m.Invoices

    '        '        For i As Integer = 0 To m.Invoices.Count - 1

    '        '            ot.SetCurrentLine(i)

    '        '            Dim b As Integer = ot.DocEntry

    '        '            If m.Cancel <> 0 Then



    '        '            End If


    '        '        Next


    '        '    End If

    '        'End If

    '        'm.Update()


    '    Catch ex As Exception

    '        Throw ex

    '    End Try

    'End Function
#End Region

End Class

