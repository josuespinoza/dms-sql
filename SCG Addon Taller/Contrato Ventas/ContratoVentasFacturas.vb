Imports System.Runtime.CompilerServices
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports DMS_Addon.LlamadaServicio
Imports DMS_Addon.GastosContratoVentas
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.DMSOne.Framework
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.UX.Windows
Imports System.Data.SqlClient
Imports DMS_Addon.Ventas
Imports System.Globalization
Imports DMSOneFramework.SCGDataAccess
Imports System.Xml.Serialization
Imports System.IO
Imports System.Collections.Generic
Imports System.Linq
Imports SCG.Financiamiento
Imports SCG.SBOFramework.DI
Imports Microsoft.Office.Interop
Imports DMS_Connector.Business_Logic.DataContract.Localizacion


Partial Public Class ContratoVentasCls

    Public oListaAgrupadaVehiculos As New List(Of ListaVehiculosAgrupadosPorTipo)()
    Public oListaEncabezado As New List(Of ListaEncabezadoFactura)()
    Public oListaAgrupadaUsados As New List(Of ListaVehiculosAgrupadosPorTipo)()
    Public oListaDocumentosFacturaProveedorUsados As New List(Of ListaDocumentosFacturaVehiculosUsados)()
    Public blnMultiplesFacturas As Boolean = False
    Public blnUsaTCContrato As Boolean = False
    Public blnManejoDescuentoFact As Boolean = False
    Public blnGeneraFacturaAccesorios As Boolean = False
    Public blnUsaFacturaExentaUsados As Boolean = False
    Public strDetalleAccs As String = String.Empty
    Public strNumeroContratoVenta As String = String.Empty

    Private decPrecioVehiculo As Decimal
    Private decBonoVehiculo As Decimal
    Private decAccesorios As Decimal
    Private decGastosInscripcion As Decimal
    Private decGastosLocales As Decimal
    Private decGastosPrenda As Decimal
    Private decOtrosGastos As String
    Private strPlacaProvisional As String


    Public strFacturasVehiculo() As String = Nothing

    Public strBodegaAccGeneral As String

    Public oMatriz As SAPbouiCOM.Matrix
    Public oMatrixAcc As SAPbouiCOM.Matrix
    Public blnGastosAdicionalesYaIngresa2 As Boolean = False

    Public objFacturaMultiple As SAPbobsCOM.Documents
    Public blnUsaFacturaExectaVentaVehiculoUsado As Boolean = False
    Public blnHayUsados As Boolean = False
    Public oListaTiposInventarios As Generic.List(Of String)
    Public g_dtUnidadesTotalOtrosCostos As DataTable
    Public g_StrPlacaProvisional As String = String.Empty
    Public g_StrCodUnidadNuevo As String = String.Empty

    Public strNFactura As String = String.Empty




    'segun la Configuración crea un documento de Factura Exenta o 
    'o un documento de Nota Credito
    Public Sub CrearDocumentoVentaVehiculoUsado(ByVal p_strNoFactura As String, _
                                                ByVal p_strCliente As String, _
                                                ByVal p_strIDVehiculo As String, _
                                                ByVal p_strNumeroContratoVenta As String, _
                                                ByVal p_strDocCurrency As String, _
                                                ByRef p_strNoNotaCreditoUsado As String, _
                                                ByRef p_strComentarioUsado As String, _
                                                ByRef p_strCodUnidad As String, _
                                                ByRef decMontoNotaCredito As Decimal, _
                                                ByRef blnNotaCredUsado As Boolean, ByVal oMatrixUsado As SAPbouiCOM.Matrix, Optional ByVal strMonedaConfigurada As String = "")


        Dim objDocumento As SAPbobsCOM.Documents
        Dim objDocumentoLineas As SAPbobsCOM.Document_Lines

        Dim decNotasCredito As Decimal
        Dim strComentarioParaLinea As String

        Dim intError As Integer
        Dim strMensajeError As String = String.Empty
        Dim decMontoReal As Decimal

        Dim oNotaCredito As SAPbobsCOM.Documents
        Dim strNotaCredito As String = String.Empty

        Dim intSerieFacturaExenta As Integer

        Dim strMensajeSuma As String
        Dim strImpuesto As String
        Dim strCuentaNotaCredito As String

        Dim decMontoUsado As Decimal
        Dim strDescMarca As String
        Dim strDescEstilo As String
        Dim strAñoVehiculo As String
        Dim strDescColor As String
        Dim strCodUnidad As String
        Dim strVIN As String
        Dim strPlaca As String
        Dim strTipo As String
        Dim strValorDimension As String = String.Empty

        'decNotasCredito = CDec(Utilitarios.CambiarValoresACultureActual(oForm.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Mon_Usa", 0), strSeparadorMilesSAP, strSeparadorDecimalesSAP))
        decNotasCredito = Utilitarios.ConvierteDecimal(oForm.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Mon_Usa", 0), n)

        'cambia el monto a moneda local
        decMontoReal = decNotasCredito
        decNotasCredito = decNotasCredito '* m_decTipoCambio
        decMontoNotaCredito = decNotasCredito

        If decMontoReal > 0 Then

            objDocumento = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices),  _
                                                                            SAPbobsCOM.Documents)

            objDocumento.DocumentSubType = SAPbobsCOM.BoDocumentSubType.bod_InvoiceExempt

            intSerieFacturaExenta = objConfiguracionGeneral.Serie(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado)


            p_strCodUnidad = p_strCodUnidad
            objDocumento.CardCode = p_strCliente

            'agrego fecha para el documento
            objDocumento.DocDate = dtFechaDocumento

            If Not String.IsNullOrEmpty(strMonedaConfigurada) Then
                objDocumento.DocCurrency = strMonedaConfigurada
            Else
                objDocumento.DocCurrency = p_strDocCurrency
            End If

            ' Usa Tipo Cambio Contrato 
            If strUsaTCContrato = "Y" And m_decTipoCambio > 0 Then
                If Not String.IsNullOrEmpty(strMonedaConfigurada) And p_strDocCurrency = strMonedaConfigurada Then
                    objDocumento.DocRate = m_decTipoCambio
                ElseIf String.IsNullOrEmpty(strMonedaConfigurada) Then
                    objDocumento.DocRate = m_decTipoCambio
                End If
            End If

            'Le pongo descuento 0 a la Nota de Credito
            objDocumento.DiscountPercent = 0

            If p_strDocCurrency = m_strMonedaLocal Then
                strMensajeSuma = My.Resources.Resource.EnUnValorDe & p_strDocCurrency & " " & String.Format("{0,10:N}", decNotasCredito)
            Else

                decNotasCredito = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, strMonedaConfigurada, decNotasCredito, p_strDocCurrency, 1, dtFechaDocumento)

                'para los comentarios en las facturas y notas de credito
                If decMontoReal = decNotasCredito Then

                    'Agregado  23/03/2012
                    '********************************************************************************************************************************************************

                    'm_decTipoCambio = CDec(Utilitarios.CambiarValoresACultureActual(oForm.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_SCGD_TipoCambio", 0), strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    'm_decTipoCambio = Utilitarios.ConvierteDecimal(oForm.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_SCGD_TipoCambio", 0), n)

                    '********************************************************************************************************************************************************

                    Dim DecNotasCreditoComentario As Decimal = decNotasCredito * m_decTipoCambio
                    Dim valor As Decimal = FormatNumber(DecNotasCreditoComentario, n.NumberDecimalDigits)

                    strMensajeSuma = My.Resources.Resource.EnUnValorDe & p_strDocCurrency & String.Format("{0,10:N}", decMontoReal) & " (" & m_strMonedaLocal & " " & String.Format("{0,10:N}", valor) & " " & My.Resources.Resource.TipoCambio & ": " & m_decTipoCambio.ToString("n2") & ")" 'String.Format("{0,10:N}", m_decTipoCambio) & ")"
                Else
                    strMensajeSuma = My.Resources.Resource.EnUnValorDe & p_strDocCurrency & String.Format("{0,10:N}", decMontoReal) & " (" & m_strMonedaLocal & " " & String.Format("{0,10:N}", decNotasCredito) & " " & My.Resources.Resource.TipoCambio & ": " & m_decTipoCambio.ToString("n2") & ")" '& String.Format("{0,10:N}", m_decTipoCambio) & ")"
                End If


            End If

            If oMatrixUsado.RowCount > 1 Then

                p_strComentarioUsado = p_strComentarioUsado & strMensajeSuma & " " & My.Resources.Resource.ReferenciaCV & ": " & p_strNumeroContratoVenta

            Else

                p_strComentarioUsado = My.Resources.Resource.RecibimosVehículo & p_strComentarioUsado & strMensajeSuma & " " & My.Resources.Resource.ReferenciaCV & ": " & p_strNumeroContratoVenta

            End If

            objDocumento.Comments = p_strComentarioUsado
            If Not String.IsNullOrEmpty(p_strNoFactura) Then
                objDocumento.NumAtCard = p_strNoFactura
            End If

            objDocumento.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service
            objDocumento.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = p_strCodUnidad.Trim()


            Dim strQueryInd As String = String.Empty
            For i As Integer = 0 To oMatrixUsado.RowCount - 1
                strQueryInd += String.Format(" Name = '{0}' OR", oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Tipo", i).TrimEnd(" "))
            Next
            strQueryInd = strQueryInd.TrimEnd("OR")
            strQueryInd = String.Format(" SELECT Name, U_Tipo, U_Cod_Imp FROM [@SCGD_ADMIN3] with (nolock) INNER JOIN [@SCGD_TIPOVEHICULO] AS TV with (nolock) ON U_Tipo = TV.Code WHERE ({0}) AND U_Cod_Item = {1} ",
                                        strQueryInd.Substring(0, strQueryInd.Length - 3), CInt(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.NotasCreditoUsados))

            Dim dtIndUsados As System.Data.DataTable = Utilitarios.EjecutarConsultaDataTable(strQueryInd, m_oCompany.CompanyDB, m_oCompany.Server)

            For i As Integer = 0 To oMatrixUsado.RowCount - 1

                'decMontoUsado = CDec(Utilitarios.CambiarValoresACultureActual(oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Val_Rec", i), strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                decMontoUsado = Utilitarios.ConvierteDecimal(oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Val_Rec", i), n)

                'CDec(Utilitarios.CambiarValoresACultureActual(oForm.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Mon_Usa", 0), strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                strDescMarca = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Marca", i)
                strDescMarca = strDescMarca.Trim()
                strDescEstilo = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Estilo", i)
                strDescEstilo = strDescEstilo.Trim()
                strAñoVehiculo = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Anio", i)
                strAñoVehiculo = strAñoVehiculo.Trim()
                strDescColor = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Color", i)
                strDescColor = strDescColor.Trim()
                strCodUnidad = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Unid", i)
                strCodUnidad = strCodUnidad.Trim()
                strVIN = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_VIN", i)
                strVIN = strVIN.Trim()
                strPlaca = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Placa", i)
                strPlaca = strPlaca.Trim()

                Dim drIndUsado() As System.Data.DataRow = dtIndUsados.Select(String.Format(" Name = '{0}'", oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Tipo", i).TrimEnd(" ")))
                strTipo = drIndUsado(0).Item("U_Tipo")
                strImpuesto = drIndUsado(0).Item("U_Cod_Imp")

                strCuentaNotaCredito = objConfiguracionGeneral.CuentaInventarioTransito(strTipo)

                'se realiza conversion de acuerdo a la mondena definida en BD
                Dim MontoAConvertir As Decimal = decMontoUsado
                Dim ValorReal As Decimal = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, strMonedaConfigurada, MontoAConvertir, p_strDocCurrency, 1, dtFechaDocumento)
                objDocumento.Lines.UnitPrice = ValorReal

                objDocumento.Lines.UserFields.Fields.Item("U_SCGD_Cod_Prov").Value = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Prov", i).ToString().Trim()
                objDocumento.Lines.UserFields.Fields.Item("U_SCGD_Nom_Prov").Value = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Nom_Prov", i).ToString().Trim()

                decMontoReal = ValorReal
                decNotasCredito = ValorReal '* m_decTipoCambio
                decMontoNotaCredito = ValorReal

                'oNotaCredito.Lines.UnitPrice = decMontoUsado
                If strImpuesto <> "" Then
                    objDocumento.Lines.TaxCode = strImpuesto
                    objDocumento.Lines.VatGroup = strImpuesto
                End If

                strComentarioParaLinea = strDescMarca & " " & strDescEstilo & " " & strAñoVehiculo & " " & strDescColor & " " & strCodUnidad & " " & strVIN & " " & strPlaca
                strComentarioParaLinea = My.Resources.Resource.RecibimosVehículo & strComentarioParaLinea

                If strComentarioParaLinea.Length <= 100 Then
                    objDocumento.Lines.ItemDescription = strComentarioParaLinea
                Else
                    objDocumento.Lines.ItemDescription = strComentarioParaLinea.Substring(0, 100)
                End If
                objDocumento.Lines.AccountCode = strCuentaNotaCredito

                If blnUsaDimensiones Then
                    '******************************************************************************************
                    'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                    If Not String.IsNullOrEmpty(strValorDimension) Then
                        If strValorDimension = "Y" Then
                            Dim strCodigoMarca As String = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Marca_Us", i).TrimEnd(" ")
                            oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContables(oForm, strTipo, strCodigoMarca, oDataTableDimensionesContablesDMS))
                        End If
                    End If
                    '******************************************************************************************

                    If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then

                        ClsLineasDocumentosDimension.AgregarDimensionesLineasDocumentos(objDocumento.Lines, oDataTableDimensionesContablesDMS)

                    End If
                End If

                objDocumento.Lines.Add()

            Next

            intError = objDocumento.Add()
            If intError <> 0 Then
                m_oCompany.GetLastError(intError, strMensajeError)

            Else
                m_oCompany.GetNewObjectCode(strNotaCredito)
                'oForm.Items.Item("txtNot_us").Specific.String = strNotaCredito
                p_strNoNotaCreditoUsado = strNotaCredito

                blnNotaCredUsado = True

            End If
        End If
    End Sub


    Public Sub TipoFactura(ByRef p_factura As SAPbobsCOM.Documents, p_serie As Integer)

        '****************************************************************************************
        'Le asigno el tipo de Factura con la cual se va a crear el documento dependiendo de la SERIE.

        '****************************************************************************************
        Dim TipoObjFactura As String = String.Empty
        TipoObjFactura = Utilitarios.EjecutarConsulta(String.Format("SELECT DocSubType FROM NNM1 with (nolock) WHERE Series = '{0}'", p_serie), m_oCompany.CompanyDB, m_oCompany.Server)

        Select Case TipoObjFactura

            Case "IB" 'Boleta
                p_factura.DocumentSubType = BoDocumentSubType.bod_Bill

            Case "IE" 'Factura Exenta
                p_factura.DocumentSubType = BoDocumentSubType.bod_InvoiceExempt

            Case "IX" 'Factura Exportacion
                p_factura.DocumentSubType = BoDocumentSubType.bod_ExportInvoice

            Case Else
                p_factura.DocumentSubType = BoDocumentSubType.bod_None

        End Select

    End Sub

    Public Sub Tipo_SerieNumeracionFactura(ByRef p_factura As SAPbobsCOM.Documents, p_CodigoUnidad As String, ByRef p_serie As Integer, ByRef p_impuesto As String, blnEsConsignado As Boolean)

        Dim strConsulta As String = "select tv.U_Usado from [@SCGD_VEHICULO] as ve  with (nolock) inner join [@SCGD_TIPOVEHICULO] as tv  with (nolock) on tv.Code = ve.U_Tipo where U_Cod_Unid ='{0}'"

        Dim strTipoInventarioUsado As String = Utilitarios.EjecutarConsulta(String.Format(strConsulta, p_CodigoUnidad), m_oCompany.CompanyDB, m_oCompany.Server)

        If strTipoInventarioUsado = "Y" Then

            If blnEsConsignado = True Then
                p_factura.DocumentSubType = BoDocumentSubType.bod_InvoiceExempt
                p_serie = objConfiguracionGeneral.SerieExenta(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado)
                p_impuesto = objConfiguracionGeneral.Impuesto(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado)
            Else
                p_factura.DocumentSubType = BoDocumentSubType.bod_InvoiceExempt
                p_serie = objConfiguracionGeneral.Serie(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado)
                p_impuesto = objConfiguracionGeneral.Impuesto(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado)
            End If

        ElseIf strTipoInventarioUsado = "N" Then

            If blnEsConsignado = True Then
                p_factura.DocumentSubType = BoDocumentSubType.bod_InvoiceExempt
                p_serie = objConfiguracionGeneral.SerieExenta(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado)
                p_impuesto = objConfiguracionGeneral.Impuesto(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado)
            Else
                p_serie = objConfiguracionGeneral.Serie(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaVentas)
                p_impuesto = objConfiguracionGeneral.Impuesto(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaVentas)

                Call TipoFactura(p_factura, p_serie)
            End If

        End If


    End Sub
    Public Function CrearEncabezadoFactura(ByRef p_form As SAPbouiCOM.Form) As SAPbobsCOM.Documents

        Dim oFactura As SAPbobsCOM.Documents
        Dim strMensajeSuma As String = String.Empty
        Dim decMontoDocumentoUsado As Decimal

        Dim DecNotasCreditoComentario As Decimal
        Dim valor As Decimal
        Dim strComentariosFactura As String = String.Empty
        Dim oMatrixUsados As SAPbouiCOM.Matrix
        Dim oCABYS As CABYS = New CABYS()
        Dim strCardCodeClienteVehiculo As String = String.Empty
        Dim strCardNameClienteVehiculo As String = String.Empty
        oFactura = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices), SAPbobsCOM.Documents)

        For Each linea As ListaEncabezadoFactura In oListaEncabezado

            oMatrixUsados = DirectCast(oForm.Items.Item("mtx_Usado").Specific, SAPbouiCOM.Matrix)

            strCardCodeClienteVehiculo = oForm.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_CCl_Veh", 0).Trim()
            strCardNameClienteVehiculo = oForm.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_NCl_Veh", 0).Trim()

            If oMatrixUsados.RowCount > 1 OrElse String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Unid", 0).ToString().Trim()) Then

                oFactura.Comments = linea.Comentarios

            Else
                strComentarioDeudaUsadoFactura += " " + oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Unid", 0).ToString().Trim()
                strComentarioDeudaUsadoFactura += " " + oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Marca", 0).ToString().Trim()
                strComentarioDeudaUsadoFactura += " " + oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Estilo", 0).ToString().Trim()
                strComentarioDeudaUsadoFactura += " " + oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Anio", 0).ToString().Trim()
                strComentarioDeudaUsadoFactura += " " + oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Color", 0).ToString().Trim()
                strComentarioDeudaUsadoFactura += " " + oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Placa", 0).ToString().Trim()

                decMontoDocumentoUsado = Utilitarios.ConvierteDecimal(oForm.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Mon_Usa", 0), n)

                If linea.DocCurrency = m_strMonedaLocal Then
                    strMensajeSuma = My.Resources.Resource.EnUnValorDe & linea.DocCurrency & " " & String.Format("{0,10:N}", decMontoDocumentoUsado)
                Else

                    decMontoDocumentoUsado = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, linea.MonedaConfigurada, decMontoDocumentoUsado, linea.DocCurrency, 1, dtFechaDocumento)

                    'm_decTipoCambio = Utilitarios.ConvierteDecimal(oForm.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_SCGD_TipoCambio", 0), n)

                    DecNotasCreditoComentario = decMontoDocumentoUsado * m_decTipoCambio
                    valor = FormatNumber(DecNotasCreditoComentario, n.NumberDecimalDigits)

                    strMensajeSuma = My.Resources.Resource.EnUnValorDe & linea.DocCurrency & String.Format("{0,10:N}", decMontoDocumentoUsado) & " (" & m_strMonedaLocal & " " & String.Format("{0,10:N}", valor) & " " & My.Resources.Resource.TipoCambio & ": " & m_decTipoCambio.ToString("n2") & ")"

                End If

                strComentariosFactura = My.Resources.Resource.RecibimosVehículo & strComentarioDeudaUsadoFactura & strMensajeSuma & " " & My.Resources.Resource.ReferenciaCV & ": " & linea.NumeroContratoVenta

                If strComentariosFactura.Length <= 254 Then
                    oFactura.Comments = strComentariosFactura
                Else
                    oFactura.Comments = strComentariosFactura.Substring(0, 254)
                End If
            End If



            'Comentarios en la Factura Cliente -- Si tiene un vehiculo usado el contrato de Ventas el despliega la informacion del vehiculo sino unicamente el contrato al que proviene

            'If String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Unid", 0).ToString().Trim()) Then

            '    oFactura.Comments = linea.Comentarios

            'Else

            'End If

            oFactura.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items
            oFactura.CardCode = linea.Cliente

            If Not String.IsNullOrEmpty(linea.MonedaConfigurada) Then 'p_strMonedaConfiguradaFV) Then
                oFactura.DocCurrency = linea.MonedaConfigurada 'p_strMonedaConfiguradaFV
            Else
                oFactura.DocCurrency = linea.DocCurrency 'p_strDocCurrency
            End If
            oFactura.PaymentGroupCode = linea.PeriodoPago ' p_intPeriodoPago
            oFactura.DocDate = dtFechaDocumento
            If Not String.IsNullOrEmpty(linea.Vendedor) Then 'p_strVendedor) Then
                oFactura.SalesPersonCode = linea.Vendedor 'p_strVendedor
            End If
            'si maneja porcentaje de descuento
            If blnManejoDescuentoFact = True Then
                Dim strPorcentajeDescuento As String = linea.PorcentajeDescuento 'p_form.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Por_Desc", 0)
                strPorcentajeDescuento = strPorcentajeDescuento.Trim()
                Dim decPorcentajeDescuento As Decimal
                If Not String.IsNullOrEmpty(linea.PorcentajeDescuento) Then 'strPorcentajeDescuento) Then
                    decPorcentajeDescuento = Decimal.Parse(linea.PorcentajeDescuento, n) 'strPorcentajeDescuento, n)
                Else
                    decPorcentajeDescuento = 0
                End If
                oFactura.DiscountPercent = decPorcentajeDescuento
            End If
            If Not String.IsNullOrEmpty(linea.Indicador) Then 'p_strIndicador) Then
                oFactura.Indicator = linea.Indicador 'p_strIndicador
            End If

            oFactura.UserFields.Fields.Item("U_SCGD_NoContrato").Value = linea.NumeroContratoVenta 'p_strNumeroContratoVenta

            If blnUsaTCContrato = True And m_decTipoCambio > 0 Then

                If Not String.IsNullOrEmpty(linea.MonedaConfigurada) And linea.DocCurrency = linea.MonedaConfigurada Then
                    oFactura.DocRate = m_decTipoCambio
                ElseIf String.IsNullOrEmpty(linea.MonedaConfigurada) Then
                    oFactura.DocRate = m_decTipoCambio
                End If
            End If

            If p_form.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_FinE", 0).Trim = "Y" Then
                oFactura.UserFields.Fields.Item("U_SCGD_ComFin").Value = String.Format(My.Resources.Resource.ComentarioFinanciamientoCV, p_form.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_EntFinE", 0).Trim, p_form.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Moneda", 0).Trim, p_form.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_FinanciaE", 0).Trim)
            End If

            If Not String.IsNullOrEmpty(strCardCodeClienteVehiculo) Then
                oFactura.UserFields.Fields.Item("U_SCGD_CCliOT").Value = strCardCodeClienteVehiculo
            End If
            If Not String.IsNullOrEmpty(strCardNameClienteVehiculo) Then
                oFactura.UserFields.Fields.Item("U_SCGD_NCliOT").Value = strCardNameClienteVehiculo
            End If

            oFactura.UserFields.Fields.Item("U_SCGD_ConEjeBan").Value = p_form.DataSources.DBDataSources.Item("@SCGD_CVENTA").GetValue("U_SCGD_ConEjeBan", 0).ToString.Trim
            oFactura.UserFields.Fields.Item("U_SCGD_NrOC").Value = p_form.DataSources.DBDataSources.Item("@SCGD_CVENTA").GetValue("U_SCGD_NrOC", 0).ToString.Trim
            oFactura.UserFields.Fields.Item("U_SCGD_NrOL").Value = p_form.DataSources.DBDataSources.Item("@SCGD_CVENTA").GetValue("U_SCGD_NrOL", 0).ToString.Trim
            '******************INICIO | CABYS **************
            If DMS_Connector.Configuracion.ParamGenAddon.U_CABYS_CR = "Y" Then
                oCABYS.CardCode = linea.Cliente
                ObtieneValoresExoneracionSN(oCABYS)
                If Not String.IsNullOrEmpty(oCABYS.OrigenTributario) Then oFactura.UserFields.Fields.Item("U_SCG_IVA2_LugarCons").Value = oCABYS.OrigenTributario
                If Not String.IsNullOrEmpty(oCABYS.TipoExoneracion) Then oFactura.UserFields.Fields.Item("U_SCG_IVA2_TipoExo").Value = oCABYS.TipoExoneracion
            End If
            '******************FIN | CABYS ******************
        Next

        If blnMultiplesFacturas Then
            objFacturaMultiple = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices),  _
                                                                 SAPbobsCOM.Documents)
            objFacturaMultiple = oFactura

        End If

        Return oFactura

    End Function

    Public Function CrearEncabezadoFacturaPorTipo(ByRef p_form As SAPbouiCOM.Form, ByRef oFactura As SAPbobsCOM.Documents, ByRef p_strCardCodeClienteFacturar As String, ByRef p_strNumeroContratoVenta As String, _
                                             ByRef p_strMonedaConfiguradaFV As String, ByRef p_strDocCurrency As String, ByRef p_intPeriodoPago As Integer, _
                                             ByRef p_strVendedor As String, ByRef p_strIndicador As String, ByRef p_strPlaca As String, ByRef p_strPlacaProvisional As String, _
                                             ByRef p_strCodUnidadNuevo As String, ByRef p_intSerieFactura As Integer, ByRef p_strItemCodeVehiculo As String, _
                                             ByRef p_blnMultiplesFact As Boolean, ByRef p_blnUsaTCContrato As Boolean, ByRef p_blnManejoDescuentoFact As Boolean, _
                                             ByRef p_blnGeneraFacturaAccesorios As Boolean, Optional ByVal p_blnUsaFacturaExentaUsados As Boolean = False) As Integer
        Dim strDescFactura As String
        Dim dtUnidadesTotalOtrosCostos As DataTable
        Dim intError As Integer = 0
        Dim strError As String = String.Empty

        decPrecioVehiculo = 0
        decBonoVehiculo = 0
        decAccesorios = 0
        decGastosInscripcion = 0
        decGastosLocales = 0
        decGastosPrenda = 0
        decOtrosGastos = 0


        blnMultiplesFacturas = p_blnMultiplesFact
        blnUsaTCContrato = p_blnUsaTCContrato
        blnManejoDescuentoFact = p_blnManejoDescuentoFact
        blnGeneraFacturaAccesorios = p_blnGeneraFacturaAccesorios
        strNumeroContratoVenta = p_strNumeroContratoVenta
        blnUsaFacturaExentaUsados = p_blnUsaFacturaExentaUsados

        g_StrPlacaProvisional = p_strPlacaProvisional
        g_StrCodUnidadNuevo = p_strCodUnidadNuevo

        strPlacaProvisional = objConfiguracionGeneral.PlacaProvisional()
        m_intTipoDocumentoCargoAdicional = objConfiguracionGeneral.TipoDocumentoDeuda

        oMatrixAcc = DirectCast(p_form.Items.Item("mtx_0").Specific, SAPbouiCOM.Matrix)
        oMatriz = DirectCast(p_form.Items.Item("mtx_Vehi").Specific, SAPbouiCOM.Matrix)

        Try

            dtUnidadesTotalOtrosCostos = p_form.DataSources.DataTables.Item("dtUnidTotalCos")

            BuscarUsadosEnContratos(oListaAgrupadaVehiculos)

            oListaEncabezado.Clear()
            oListaEncabezado.Add(New ListaEncabezadoFactura() With {.Cliente = p_strCardCodeClienteFacturar,
                                                                   .Comentarios = My.Resources.Resource.ContratoCorrespondiente & p_strNumeroContratoVenta,
                                                                   .MonedaConfigurada = p_strMonedaConfiguradaFV,
                                                                   .DocCurrency = p_strDocCurrency,
                                                                   .PeriodoPago = p_intPeriodoPago,
                                                                   .Vendedor = p_strVendedor,
                                                                   .PorcentajeDescuento = p_form.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Por_Desc", 0),
                                                                   .Indicador = p_strIndicador,
                                                                   .NumeroContratoVenta = p_strNumeroContratoVenta,
                                                                   .TipoCambio = m_decTipoCambio,
                                                                   .ConEjeBan = p_form.DataSources.DBDataSources.Item("@SCGD_CVENTA").GetValue("U_SCGD_ConEjeBan", 0).ToString.Trim,
                                                                    .NrOC = p_form.DataSources.DBDataSources.Item("@SCGD_CVENTA").GetValue("U_SCGD_NrOC", 0).ToString.Trim,
                                                                    .NrOL = p_form.DataSources.DBDataSources.Item("@SCGD_CVENTA").GetValue("U_SCGD_NrOL", 0).ToString.Trim,
                                                                    .PlacaProvisional = p_strPlacaProvisional})



            intError = CrearLineasDetalleFactura(p_form, oFactura, p_strMonedaConfiguradaFV, p_strDocCurrency)

            If intError <> 0 Then

                strError = m_oCompany.GetLastErrorDescription()
                Throw New ExceptionsSBO(intError, strError)
            Else

                Return intError

            End If


        Catch ex As Exception
            If intError <> 0 Then
                Throw New ExceptionsSBO(intError, strError)
            Else
                Throw New Exception(ex.Message)
            End If
        End Try

    End Function

    Public Function CrearLineasDetalleFactura(ByRef p_form As SAPbouiCOM.Form, ByVal p_factura As SAPbobsCOM.Documents, ByVal p_strMonedaConfiguradaFV As String, _
                                         ByVal p_DocCurrency As String, Optional ByVal blnUsaDimension As Boolean = False) As Integer

        Dim intAsientoAjuste As Integer
        Dim strCuentaCarroRecibe As String = String.Empty
        Dim strTipo As String = String.Empty
        Dim strAñoVehiculo As String = String.Empty
        Dim strCodUnidadNuevo As String = String.Empty
        Dim strCodUnidadUsado As String = String.Empty
        Dim strDescColor As String = String.Empty
        Dim strDescEstilo As String = String.Empty
        Dim strDescModelo As String = String.Empty
        Dim strKmVenta As String = String.Empty
        Dim decKmVenta As Double = 0
        Dim strDescMarca As String = String.Empty
        Dim strPlaca As String = String.Empty
        Dim strVIN As String = String.Empty
        Dim strCardName As String = String.Empty
        Dim strPlacaProvisional As String
        Dim strTipoVendido As String
        Dim strDescFactura As String
        Dim strItemCodeVehiculo As String
        Dim strBodegaTipoVeh As String = String.Empty
        Dim strImpuesto As String = String.Empty
        Dim strTipoVehiculo As String = String.Empty

        Dim decPrecioVehiculo As Decimal
        Dim decBonoVehiculo As Decimal
        Dim strIDVehiculo As String
        Dim strUnidad As String

        Dim strDescuentoVeh As String
        Dim decDescuentoVeh As Decimal

        Dim decAccesorios As Decimal
        Dim decGastosInscripcion As Decimal
        Dim decGastosLocales As Decimal
        Dim decGastosPrenda As Decimal
        Dim decOtrosGastos As String

        Dim strItemCodeLocales As String
        Dim strItemCodeInscripcion As String
        Dim strItemCodePrenda As String
        Dim strImpGasLoc As String

        Dim oMatrixOtrosCostos As SAPbouiCOM.Matrix
        Dim strOtrosCostos As String
        Dim decOtrosCostos As Decimal = 0
        Dim decOtrosCostosTotal As Decimal = 0
        Dim dtUnidadesTotalOtrosCostos As DataTable

        Dim ofacturaTemporal As SAPbobsCOM.Documents


        Dim intError As Integer = 0

        Dim intSerieFactura As Integer

        Dim counter As Integer = 0

        Dim m As String = ""

        Dim strSeparadorDecimalesSAP As String = String.Empty
        Dim strSeparadorMilesSAP As String = String.Empty
        Dim strValorSeleccionado As String = String.Empty

        Dim tipodeLista As String = String.Empty
        Dim blnBEvento As Boolean
        Dim strConsignado As String
        Dim blnConsignado As Boolean = False
        Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

        dtUnidadesTotalOtrosCostos = oForm.DataSources.DataTables.Item("dtUnidTotalCos")


        strDetalleAccs = p_form.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Det_Acc", 0).Trim()
        strBodegaAccGeneral = oDataTableConfiguracionesDMS.GetValue("U_SCGD_BodAcc", 0)


        decPrecioVehiculo = Utilitarios.ConvierteDecimal(p_form.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Pre_Vta", 0), n)
        decGastosLocales = Utilitarios.ConvierteDecimal(p_form.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Gas_Loc", 0), n)
        decAccesorios = Utilitarios.ConvierteDecimal(p_form.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Ext_Adi", 0), n)
        decGastosPrenda = Utilitarios.ConvierteDecimal(p_form.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Gas_Pre", 0), n)
        decGastosInscripcion = Utilitarios.ConvierteDecimal(p_form.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Gas_Ins", 0), n)
        decOtrosGastos = Utilitarios.ConvierteDecimal(p_form.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Otros_L", 0), n)

        Try


            For j As Integer = 0 To oListaTiposInventarios.Count - 1

                tipodeLista = String.Empty

                tipodeLista = oListaTiposInventarios.Item(j)

                p_factura = CrearEncabezadoFactura(p_form)


                For i As Integer = 0 To oListaAgrupadaVehiculos.Count - 1

                    If oListaAgrupadaVehiculos.Item(i).Aplicado = False And oListaAgrupadaVehiculos.Item(i).Tipo = tipodeLista Then

                        If blnMultiplesFacturas Then
                            p_factura = CrearEncabezadoFactura(p_form)
                        End If
                        strConsignado = Utilitarios.EjecutarConsulta(String.Format("Select U_Consig from [@SCGD_VEHICULO] WITH (nolock) where U_Cod_Unid = '{0}'",
                                                          oListaAgrupadaVehiculos.Item(i).CodigoUnidad.Trim())).Trim()

                        If Not String.IsNullOrEmpty(strConsignado) Then
                            If strConsignado = "Y" Then
                                blnConsignado = True
                            Else
                                blnConsignado = False
                            End If
                        Else
                            blnConsignado = False
                        End If
                        strImpuesto = String.Empty
                        AsignarNumeracionTipoFacturaDev(p_factura, strImpuesto, oListaAgrupadaVehiculos.Item(i).Tipo.Equals("Y"), oListaAgrupadaVehiculos.Item(i).TipoInventario, blnConsignado, blnUsaFacturaExentaUsados)

                        strUnidad = oListaAgrupadaVehiculos.Item(i).CodigoUnidad
                        strDescMarca = oListaAgrupadaVehiculos.Item(i).Marca 'oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Des_Marc", i).Trim()
                        strDescEstilo = oListaAgrupadaVehiculos.Item(i).Estilo 'oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Des_Esti", i).Trim()
                        strDescModelo = oListaAgrupadaVehiculos.Item(i).Modelo 'oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Des_Mode", i).Trim()
                        strDescColor = oListaAgrupadaVehiculos.Item(i).Color 'oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Des_Col", i).Trim()
                        strAñoVehiculo = oListaAgrupadaVehiculos.Item(i).Anno 'oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Ano_Vehi", i).Trim()
                        strAñoVehiculo = String.Format("{0} {1}", My.Resources.Resource.Año, strAñoVehiculo)
                        strCodUnidadNuevo = oListaAgrupadaVehiculos.Item(i).CodigoUnidad 'oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Cod_Unid", i).Trim()
                        strVIN = oListaAgrupadaVehiculos.Item(i).VIN 'oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Num_VIN", i).Trim()
                        strVIN = String.Format("{0} {1}", My.Resources.Resource.VIN, strVIN)
                        strPlaca = oListaAgrupadaVehiculos.Item(i).Placa 'oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Num_Plac", i).Trim()
                        strDescEstilo = String.Format("{0} {1}", strDescEstilo, strDescModelo)
                        strTipo = oListaAgrupadaVehiculos.Item(i).TipoInventario

                        strIDVehiculo = Utilitarios.EjecutarConsulta(
                                               String.Format("Select Code from [@SCGD_VEHICULO] WITH (nolock) where U_Cod_Unid = '{0}'",
                                                             oListaAgrupadaVehiculos.Item(i).CodigoUnidad),
                                               m_oCompany.CompanyDB,
                                               m_oCompany.Server)

                        strItemCodeVehiculo = Utilitarios.EjecutarConsulta("SELECT [@SCGD_CONF_ART_VENTA].U_ArtVent FROM [@SCGD_VEHICULO] WITH (nolock) INNER JOIN [@SCGD_CONF_ART_VENTA] WITH (nolock) ON [@SCGD_VEHICULO].U_ArtVent = [@SCGD_CONF_ART_VENTA].Code WHERE [@SCGD_VEHICULO].Code = '" & strIDVehiculo & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                        strItemCodeVehiculo = strItemCodeVehiculo.Trim()
                        strKmVenta = oListaAgrupadaVehiculos.Item(i).KmSale ' oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Km_Venta", i).Trim()

                        Dim strTipoVeh As String = oListaAgrupadaVehiculos.Item(i).TipoInventario  'strTipo.Trim()

                        strBodegaTipoVeh = objConfiguracionGeneral.AccesoriosXAlmacen(oListaAgrupadaVehiculos.Item(i).TipoInventario) 'objConfiguracionGeneral.AccesoriosXAlmacen(strTipoVeh)

                        If String.IsNullOrEmpty(strItemCodeVehiculo) Then
                            strItemCodeVehiculo =
                                Utilitarios.EjecutarConsulta(
                                    String.Format("Select U_ItemCode from [@SCGD_ADMIN1] WITH (nolock) where U_Tipo = '{0}' and U_Cod_Item = '1'",
                                                  strTipoVeh),
                                              m_oCompany.CompanyDB,
                                              m_oCompany.Server)
                        End If

                        If oMatriz.RowCount >= 1 AndAlso Not String.IsNullOrEmpty(strUnidad) Then
                            If String.IsNullOrEmpty(oListaAgrupadaVehiculos.Item(i).Placa) Then
                                If Not String.IsNullOrEmpty(g_StrPlacaProvisional) Then
                                    p_factura.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = g_StrPlacaProvisional
                                End If
                            Else
                                p_factura.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = oListaAgrupadaVehiculos.Item(i).Placa
                            End If
                            If Not String.IsNullOrEmpty(g_StrCodUnidadNuevo) Then
                                'ESanabria 14.01.2013
                                p_factura.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = g_StrCodUnidadNuevo.Trim
                            Else
                                p_factura.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = strUnidad
                            End If
                        End If

                        decPrecioVehiculo = Utilitarios.ConvierteDecimal(p_form.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Pre_Vta", oListaAgrupadaVehiculos.Item(i).Fila), n)
                        decBonoVehiculo = Utilitarios.ConvierteDecimal(p_form.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Bono", oListaAgrupadaVehiculos.Item(i).Fila), n)

                        'oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Pre_Vta", i), n)
                        'oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Bono", i), n)

                        'decPrecioVehiculo -= decBonoVehiculo

                        oMatrixOtrosCostos = DirectCast(oForm.Items.Item(g_str_mtxOtrosCostos).Specific, SAPbouiCOM.Matrix)


                        decOtrosCostosTotal = 0
                        For y As Integer = 0 To oMatrixOtrosCostos.RowCount - 1
                            decOtrosCostos = 0
                            If oForm.DataSources.DBDataSources.Item(g_str_OTROCXCV).GetValue("U_Unidad", y).Trim() = strUnidad Then



                                strOtrosCostos = oForm.DataSources.DBDataSources.Item(g_str_OTROCXCV).GetValue("U_Monto", y).Trim()

                                If Not String.IsNullOrEmpty(strOtrosCostos) Then decOtrosCostos = Decimal.Parse(strOtrosCostos, n)

                            End If
                            decOtrosCostosTotal += decOtrosCostos
                        Next

                        dtUnidadesTotalOtrosCostos.Rows.Add()
                        dtUnidadesTotalOtrosCostos.SetValue("Unidad", i, strUnidad)
                        dtUnidadesTotalOtrosCostos.SetValue("OtrosCostos", i, decOtrosCostosTotal.ToString(n))

                        g_dtUnidadesTotalOtrosCostos = dtUnidadesTotalOtrosCostos

                        'decPrecioVehiculo += decOtrosCostosTotal

                        If strItemCodeVehiculo <> "" Then
                            If decPrecioVehiculo > 0 Then

                                p_factura.Lines.UserFields.Fields.Item("U_SCGD_Cod_Unid").Value = strUnidad.Trim

                                p_factura.Lines.ItemCode = strItemCodeVehiculo

                                strDescFactura = My.Resources.Resource.Vehiculo & ": " & strUnidad & " " & strDescMarca & " " & strDescEstilo & " " & strVIN & " " & strAñoVehiculo & " " & strDescColor

                                If strDescFactura.Length() <= 100 Then
                                    p_factura.Lines.ItemDescription = strDescFactura
                                Else
                                    p_factura.Lines.ItemDescription = strDescFactura.Substring(0, 100)
                                End If

                                If Not blnUsaFacturaExentaUsados Then
                                    strImpuesto = oListaAgrupadaVehiculos.Item(i).Impuesto
                                    strImpuesto = strImpuesto.Trim()
                                End If

                                If strImpuesto <> "" Then
                                    p_factura.Lines.TaxCode = strImpuesto
                                    p_factura.Lines.VatGroup = strImpuesto

                                End If

                                Dim strCuentaIngreso As String = objConfiguracionGeneral.CuentaIngreso(strTipoVeh)

                                If Not String.IsNullOrEmpty(strCuentaIngreso) Then
                                    p_factura.Lines.AccountCode = strCuentaIngreso
                                End If

                                If blnManejoDescuentoFact = True Then
                                    strDescuentoVeh = oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Desc_Veh", i)
                                    strDescuentoVeh = strDescuentoVeh.Trim()
                                    If Not String.IsNullOrEmpty(strDescuentoVeh) Then
                                        decDescuentoVeh = Decimal.Parse(strDescuentoVeh, n)
                                        If Not decDescuentoVeh = 0 Then
                                            p_factura.Lines.DiscountPercent = decDescuentoVeh
                                        End If
                                    End If

                                End If

                                Dim strGastoLocalVeh As String
                                Dim decGastoLocalVeh As Decimal

                                'strGastoLocalVeh = oListaAgrupadaVehiculos.Item(i).GL  'oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Gas_Loc", i)
                                strGastoLocalVeh = oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Gas_Loc", oListaAgrupadaVehiculos.Item(i).Fila)
                                strGastoLocalVeh = strGastoLocalVeh.Trim()
                                If Not String.IsNullOrEmpty(strGastoLocalVeh) Then
                                    decGastoLocalVeh = Decimal.Parse(strGastoLocalVeh, n)
                                Else
                                    decGastoLocalVeh = 0
                                End If

                                Dim MontoAConvertir As Decimal
                                Dim ValorReal As Decimal

                                If decGastosLocales > 0 AndAlso Not blnMultiplesFacturas = True Then

                                    MontoAConvertir = decPrecioVehiculo - decGastosLocales
                                    ValorReal = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, p_strMonedaConfiguradaFV, MontoAConvertir, p_DocCurrency, 1, dtFechaDocumento)
                                    p_factura.Lines.UnitPrice = ValorReal

                                ElseIf decGastoLocalVeh > 0 AndAlso blnMultiplesFacturas = True Then

                                    MontoAConvertir = decPrecioVehiculo - decGastoLocalVeh
                                    ValorReal = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, p_strMonedaConfiguradaFV, MontoAConvertir, p_DocCurrency, 1, dtFechaDocumento)
                                    p_factura.Lines.UnitPrice = ValorReal

                                Else

                                    p_factura.Lines.UnitPrice = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, p_strMonedaConfiguradaFV, decPrecioVehiculo, p_DocCurrency, 1, dtFechaDocumento)

                                End If

                                '******************************************************************************************
                                'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                                If blnUsaDimensiones Then

                                    'If Not String.IsNullOrEmpty(strValorDimension) Then
                                    '    If strValorDimension = "Y" Then
                                    Dim strCodigoMarca As String = Utilitarios.EjecutarConsulta("Select U_Cod_Marc from dbo.[@SCGD_VEHICULO] WITH (nolock) where U_Cod_Unid = '" & strCodUnidadNuevo.Trim & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                    oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContables(p_form, strTipoVeh, strCodigoMarca, oDataTableDimensionesContablesDMS))
                                    'End If
                                    '    End If

                                    '*****************************************************************************
                                    'Agrego dimensiones contables en las lineas de la facturas
                                    If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                                        ClsLineasDocumentosDimension.AgregarDimensionesLineasDocumentos(p_factura.Lines, oDataTableDimensionesContablesDMS)
                                    End If
                                    '*****************************************************************************
                                End If
                                '******************************************************************************************
                                '******************INICIO | CABYS **************
                                If DMS_Connector.Configuracion.ParamGenAddon.U_CABYS_CR = "Y" Then
                                    If Not String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_CABYS_AE", i)) Then p_factura.Lines.UserFields.Fields.Item("U_SCG_IVA2_Act_Econ").Value = oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_CABYS_AE", i)
                                    If Not String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_CABYS_TI", i)) Then p_factura.Lines.UserFields.Fields.Item("U_SCG_IVA2_TipoItem").Value = oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_CABYS_TI", i)
                                    If Not String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_CABYS_CH", i)) Then p_factura.Lines.UserFields.Fields.Item("U_SCG_IVA2_CodItem").Value = oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_CABYS_CH", i)
                                End If
                                '******************FIN | CABYS ******************
                                p_factura.Lines.Add()
                                If decBonoVehiculo > 0 Then
                                    p_factura.Lines.ItemCode = strItemCodeVehiculo
                                    p_factura.Lines.ItemDescription = My.Resources.Resource.LineaBono
                                    p_factura.Lines.Quantity = -1
                                    If strImpuesto <> "" Then
                                        p_factura.Lines.TaxCode = strImpuesto
                                        p_factura.Lines.VatGroup = strImpuesto
                                    End If
                                    If Not String.IsNullOrEmpty(strCuentaIngreso) Then
                                        p_factura.Lines.AccountCode = strCuentaIngreso
                                    End If
                                    p_factura.Lines.UnitPrice = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, p_strMonedaConfiguradaFV, decBonoVehiculo, p_DocCurrency, 1, dtFechaDocumento)
                                    If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                                        ClsLineasDocumentosDimension.AgregarDimensionesLineasDocumentos(p_factura.Lines, oDataTableDimensionesContablesDMS)
                                    End If
                                    p_factura.Lines.Add()
                                End If
                                If decOtrosCostosTotal > 0 Then
                                    p_factura.Lines.ItemCode = strItemCodeVehiculo
                                    p_factura.Lines.ItemDescription = My.Resources.Resource.LineaOtrosCostos
                                    If strImpuesto <> "" Then
                                        p_factura.Lines.TaxCode = strImpuesto
                                        p_factura.Lines.VatGroup = strImpuesto
                                    End If
                                    If Not String.IsNullOrEmpty(strCuentaIngreso) Then
                                        p_factura.Lines.AccountCode = strCuentaIngreso
                                    End If
                                    p_factura.Lines.UnitPrice = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, p_strMonedaConfiguradaFV, decOtrosCostosTotal, p_DocCurrency, 1, dtFechaDocumento)
                                    If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                                        ClsLineasDocumentosDimension.AgregarDimensionesLineasDocumentos(p_factura.Lines, oDataTableDimensionesContablesDMS)
                                    End If
                                    p_factura.Lines.Add()
                                End If
                                ReDim Preserve strFacturasVehiculo(i)
                                If blnMultiplesFacturas Then

                                    strItemCodeLocales = Utilitarios.EjecutarConsulta("Select U_ItemCode from [@SCGD_ADMIN1] WITH (nolock) where U_Tipo = '" & strTipoVeh & "' and U_Cod_Item = '2'", m_oCompany.CompanyDB, m_oCompany.Server)
                                    strImpGasLoc = Utilitarios.EjecutarConsulta("Select U_Cod_Imp from [@SCGD_ADMIN3] WITH (nolock) where U_Tipo = '" & strTipoVeh & "' and U_Cod_Item = '1'", m_oCompany.CompanyDB, m_oCompany.Server)

                                    If (decGastoLocalVeh > 0 OrElse oMatrixAcc.RowCount > 0) AndAlso Not blnGeneraFacturaAccesorios = True AndAlso oMatriz.RowCount > 1 Then
                                        Dim strManejoDescuentoFact As String

                                        If blnManejoDescuentoFact Then
                                            strManejoDescuentoFact = "Y"
                                        End If

                                        Call ManejaAccesoriosFactura(oMatrixAcc, strManejoDescuentoFact, strDetalleAccs, p_strMonedaConfiguradaFV, p_DocCurrency, strBodegaAccGeneral, strBodegaTipoVeh, decAccesorios, decGastoLocalVeh, strItemCodeLocales, strImpGasLoc, p_factura, True, strTipoVeh, strUnidad)

                                    ElseIf blnGeneraFacturaAccesorios AndAlso decGastoLocalVeh > 0 Then

                                        If Not String.IsNullOrEmpty(strItemCodeLocales) Then

                                            p_factura.Lines.ItemCode = strItemCodeLocales
                                            p_factura.Lines.UnitPrice = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, p_strMonedaConfiguradaFV, decGastoLocalVeh, p_DocCurrency, 1, dtFechaDocumento)
                                            If Not String.IsNullOrEmpty(strImpGasLoc) Then
                                                p_factura.Lines.TaxCode = strImpGasLoc
                                                p_factura.Lines.VatGroup = strImpGasLoc
                                            End If
                                            If Not String.IsNullOrEmpty(strBodegaTipoVeh) Then
                                                p_factura.Lines.WarehouseCode = strBodegaTipoVeh
                                                Dim strRevenuesAcce As String = Utilitarios.EjecutarConsulta("SELECT RevenuesAc FROM OWHS WITH (nolock) WHERE WhsCode = '" & strBodegaTipoVeh & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                                If Not String.IsNullOrEmpty(strRevenuesAcce) Then
                                                    p_factura.Lines.AccountCode = strRevenuesAcce.Trim()
                                                End If
                                            ElseIf Not String.IsNullOrEmpty(strBodegaAccGeneral) Then
                                                p_factura.Lines.WarehouseCode = strBodegaAccGeneral
                                            End If

                                            '******************************************************************************************
                                            'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                                            If blnUsaDimensiones Then
                                                ' If Not String.IsNullOrEmpty(strValorDimension) Then
                                                ' If strValorDimension = "Y" Then
                                                Dim strCodigoMarca As String = Utilitarios.EjecutarConsulta("Select U_Cod_Marc from dbo.[@SCGD_VEHICULO] WITH (nolock) where U_Cod_Unid = '" & strCodUnidadNuevo.Trim & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                                oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContables(p_form, strTipoVeh, strCodigoMarca, oDataTableDimensionesContablesDMS))
                                                'End If
                                                'End If
                                                '*****************************************************************************
                                                'Agrego dimensiones contables en las lineas de la facturas
                                                If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                                                    ClsLineasDocumentosDimension.AgregarDimensionesLineasDocumentos(p_factura.Lines, oDataTableDimensionesContablesDMS)
                                                End If
                                                '*****************************************************************************
                                            End If
                                            '*******************

                                            p_factura.Lines.Add()

                                        End If

                                    End If

                                    'Manejo de gastos por unidad
                                    Dim strMontoOtrosVeh As String
                                    Dim decMontoOtrosVeh As Decimal
                                    ' strMontoOtrosVeh = oListaAgrupadaVehiculos.Item(i).OG   'oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Otro_Gas", i)
                                    strMontoOtrosVeh = oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Otro_Gas", oListaAgrupadaVehiculos.Item(i).Fila)
                                    strMontoOtrosVeh = strMontoOtrosVeh.Trim()
                                    If Not String.IsNullOrEmpty(strMontoOtrosVeh) Then
                                        decMontoOtrosVeh = Decimal.Parse(strMontoOtrosVeh, n)
                                    Else
                                        decMontoOtrosVeh = 0
                                    End If

                                    If decMontoOtrosVeh > 0 AndAlso oDataTableGastosUnidad.Rows.Count > 0 Then

                                        Dim strUnidadDT As String
                                        Dim strGuardaDT As String
                                        Dim strContratoDT As String
                                        Dim decMontoDT As Decimal
                                        Dim strConsulta As String
                                        Dim strTipoConfGasto As String
                                        Dim decMontoConvertidoDT As Decimal

                                        For intGastos As Integer = 0 To oDataTableGastosUnidad.Rows.Count - 1

                                            strContratoDT = oDataTableGastosUnidad.GetValue("cont", intGastos)
                                            strUnidadDT = oDataTableGastosUnidad.GetValue("unidad", intGastos)
                                            strGuardaDT = oDataTableGastosUnidad.GetValue("guarda", intGastos)
                                            decMontoDT = oDataTableGastosUnidad.GetValue("monto", intGastos)

                                            If decMontoDT > 0 And strNumeroContratoVenta = strContratoDT And strUnidad = strUnidadDT And strGuardaDT = "Y" Then

                                                oDataTableConfGastos.Rows.Clear()
                                                oDataTableConfGastos = oForm.DataSources.DataTables.Item("ConfGast")

                                                strConsulta = "Select Code, U_Tipo, U_Cod_Item, U_Cod_GA, U_Impuesto From [@SCGD_CONFLINEASSUM] WITH (nolock) Where Code = '" & oDataTableGastosUnidad.GetValue("codItem", intGastos) & "'"

                                                oDataTableConfGastos.ExecuteQuery(strConsulta)

                                                If Not String.IsNullOrEmpty(oDataTableConfGastos.GetValue("Code", 0)) Then

                                                    strTipoConfGasto = oDataTableConfGastos.GetValue("U_Tipo", 0)

                                                    decMontoConvertidoDT = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, p_DocCurrency, decMontoDT, oDataTableGastosUnidad.GetValue("moneda", intGastos), 1, dtFechaDocumento)

                                                    If strTipoConfGasto = "1" AndAlso Not String.IsNullOrEmpty(oDataTableConfGastos.GetValue("U_Cod_Item", 0)) Then

                                                        p_factura.Lines.ItemCode = oDataTableConfGastos.GetValue("U_Cod_Item", 0)
                                                        If Not String.IsNullOrEmpty(oDataTableConfGastos.GetValue("U_Impuesto", 0)) Then
                                                            p_factura.Lines.TaxCode = oDataTableConfGastos.GetValue("U_Impuesto", 0)
                                                            p_factura.Lines.VatGroup = oDataTableConfGastos.GetValue("U_Impuesto", 0)
                                                        End If
                                                        p_factura.Lines.UnitPrice = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, p_strMonedaConfiguradaFV, decMontoConvertidoDT, p_DocCurrency, 1, dtFechaDocumento)
                                                        p_factura.Lines.Add()

                                                    ElseIf strTipoConfGasto = "2" AndAlso Not String.IsNullOrEmpty(oDataTableConfGastos.GetValue("U_Cod_GA", 0)) Then

                                                        p_factura.Expenses.ExpenseCode = oDataTableConfGastos.GetValue("U_Cod_GA", 0)
                                                        If Not String.IsNullOrEmpty(oDataTableConfGastos.GetValue("U_Impuesto", 0)) Then
                                                            p_factura.Expenses.TaxCode = oDataTableConfGastos.GetValue("U_Impuesto", 0)
                                                            p_factura.Expenses.VatGroup = oDataTableConfGastos.GetValue("U_Impuesto", 0)
                                                        End If
                                                        p_factura.Expenses.LineTotal = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, p_strMonedaConfiguradaFV, decMontoConvertidoDT, p_DocCurrency, 1, dtFechaDocumento)
                                                        p_factura.Expenses.Add()

                                                    End If

                                                End If

                                            End If

                                        Next

                                    End If


                                    If oMatriz.RowCount > 1 Then

                                        intError = p_factura.Add()
                                        Dim strError As String = m_oCompany.GetLastErrorDescription()

                                        If intError <> 0 Then
                                            m_oCompany.GetLastError(intError, m)
                                            m_SBO_Application.SetStatusBarMessage(String.Format("{0} Error: {1} {2}", My.Resources.Resource.DocumentoFacturaVentas, intError, m))
                                            Throw New ExceptionsSBO(intError, strError)
                                        Else

                                            m_oCompany.GetNewObjectCode(strFacturasVehiculo(i))
                                            oForm.Items.Item("txtNofac").Specific.string = My.Resources.Resource.Multiples
                                            p_factura = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices), SAPbobsCOM.Documents)

                                        End If

                                    End If

                                End If
                            Else
                                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.PrecioVehículo, SAPbouiCOM.BoMessageTime.bmt_Medium)
                                If m_oCompany.InTransaction() Then
                                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                End If
                                If m_cnConeccionTransaccion.State = ConnectionState.Open Then
                                    m_tnTransaccion.Rollback()
                                    m_cnConeccionTransaccion.Close()

                                End If

                                m_dtsVehiculo.SCG_VEHICULO.Clear()

                                Throw New Exception(My.Resources.Resource.PrecioVehículo)
                            End If
                        Else
                            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.CodigosFacturasVehiculos, SAPbouiCOM.BoMessageTime.bmt_Medium)
                            If m_oCompany.InTransaction() Then
                                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                            End If
                            If m_cnConeccionTransaccion.State = ConnectionState.Open Then
                                m_tnTransaccion.Rollback()
                                m_cnConeccionTransaccion.Close()

                            End If

                            m_dtsVehiculo.SCG_VEHICULO.Clear()

                            Throw New Exception(My.Resources.Resource.CodigosFacturasVehiculos)

                        End If

                        'validamos que la linea sea aplicada y no se tome en cuenta en el siguiente for

                        oListaAgrupadaVehiculos.Item(i).Aplicado = True
                        counter = counter + 1
                    End If

                Next i

                strUnidad = p_form.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Cod_Unid", 0)
                strUnidad = strUnidad.Trim()

                strIDVehiculo = Utilitarios.EjecutarConsulta("Select Code from [@SCGD_VEHICULO] WITH (nolock) where U_Cod_Unid = '" & strUnidad & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                strTipoVehiculo = Utilitarios.EjecutarConsulta("Select U_Tipo from dbo.[@SCGD_Vehiculo] WITH (nolock) where Code = '" & strIDVehiculo & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                strItemCodeLocales = Utilitarios.EjecutarConsulta("Select U_ItemCode from [@SCGD_ADMIN1] WITH (nolock) where U_Tipo = '" & strTipoVehiculo & "' and U_Cod_Item = '2'", m_oCompany.CompanyDB, m_oCompany.Server)

                strImpGasLoc = Utilitarios.EjecutarConsulta("Select U_Cod_Imp from [@SCGD_ADMIN3] WITH (nolock) where U_Tipo = '" & strTipoVehiculo & "' and U_Cod_Item = '1'", m_oCompany.CompanyDB, m_oCompany.Server)

                strBodegaTipoVeh = objConfiguracionGeneral.AccesoriosXAlmacen(strTipoVehiculo)

                If (decGastosLocales > 0 OrElse oMatrixAcc.RowCount > 0) AndAlso Not blnGeneraFacturaAccesorios = True AndAlso (Not blnMultiplesFacturas = True OrElse (blnMultiplesFacturas = True AndAlso oMatriz.RowCount = 1)) Then
                    Dim strManejoDescuentoFact = String.Empty

                    If blnManejoDescuentoFact Then
                        strManejoDescuentoFact = "Y"
                    Else
                        strManejoDescuentoFact = "N"
                    End If

                    If oMatriz.RowCount = 1 Then
                        Call ManejaAccesoriosFactura(oMatrixAcc, strManejoDescuentoFact, strDetalleAccs, p_strMonedaConfiguradaFV, p_DocCurrency, strBodegaAccGeneral, strBodegaTipoVeh, decAccesorios, decGastosLocales, strItemCodeLocales, strImpGasLoc, p_factura, True, strTipoVehiculo, strUnidad)
                    Else
                        Call ManejaAccesoriosFactura(oMatrixAcc, strManejoDescuentoFact, strDetalleAccs, p_strMonedaConfiguradaFV, p_DocCurrency, strBodegaAccGeneral, strBodegaTipoVeh, decAccesorios, decGastosLocales, strItemCodeLocales, strImpGasLoc, p_factura)
                    End If
                ElseIf blnGeneraFacturaAccesorios = True AndAlso decGastosLocales > 0 AndAlso Not blnMultiplesFacturas Then

                    p_factura.Lines.ItemCode = strItemCodeLocales
                    p_factura.Lines.UnitPrice = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, p_strMonedaConfiguradaFV, decGastosLocales, p_DocCurrency, 1, dtFechaDocumento)
                    p_factura.Lines.TaxCode = strImpGasLoc
                    p_factura.Lines.VatGroup = strImpGasLoc
                    If Not String.IsNullOrEmpty(strBodegaTipoVeh) Then
                        p_factura.Lines.WarehouseCode = strBodegaTipoVeh
                        Dim strRevenuesAcce As String = Utilitarios.EjecutarConsulta("SELECT RevenuesAc FROM OWHS WITH (nolock) WHERE WhsCode = '" & strBodegaTipoVeh & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                        If Not String.IsNullOrEmpty(strRevenuesAcce) Then
                            p_factura.Lines.AccountCode = strRevenuesAcce.Trim()
                        End If
                    ElseIf Not String.IsNullOrEmpty(strBodegaAccGeneral) Then
                        p_factura.Lines.WarehouseCode = strBodegaAccGeneral
                    End If

                    '******************************************************************************************
                    'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                    If blnUsaDimensiones Then
                        ' If Not String.IsNullOrEmpty(strValorDimension) Then
                        'If strValorDimension = "Y" Then
                        Dim strCodigoMarca As String = Utilitarios.EjecutarConsulta("Select U_Cod_Marc from dbo.[@SCGD_VEHICULO] WITH (nolock) where U_Cod_Unid = '" & strCodUnidadNuevo.Trim & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                        oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContables(oForm, strTipoVehiculo, strCodigoMarca, oDataTableDimensionesContablesDMS))
                        'End If
                        'End If
                        '*****************************************************************************
                        'Agrego dimensiones contables en las lineas de la facturas
                        If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                            ClsLineasDocumentosDimension.AgregarDimensionesLineasDocumentos(p_factura.Lines, oDataTableDimensionesContablesDMS)
                        End If
                        '*****************************************************************************
                    End If
                    '*******************

                    p_factura.Lines.Add()
                End If

                strImpuesto = objConfiguracionGeneral.Impuesto(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaVentas)

                If decGastosInscripcion > 0 Then
                    strItemCodeInscripcion = objConfiguracionGeneral.GastosAdicionales(SCGDataAccess.ConfiguracionesGeneralesAddon.scgItemsFactura.gastosIncripcion)

                    If strItemCodeInscripcion <> "" Then
                        blnGastosAdicionalesYaIngresa2 = True
                        p_factura.Expenses.ExpenseCode = strItemCodeInscripcion
                        p_factura.Expenses.LineTotal = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, p_strMonedaConfiguradaFV, decGastosInscripcion, p_DocCurrency, 1, dtFechaDocumento)

                        If strImpuesto <> "" Then
                            p_factura.Expenses.TaxCode = strImpuesto
                            p_factura.Expenses.VatGroup = strImpuesto
                        End If
                    End If
                End If

                If decGastosPrenda > 0 Then
                    strItemCodePrenda = objConfiguracionGeneral.GastosAdicionales(SCGDataAccess.ConfiguracionesGeneralesAddon.scgItemsFactura.GastosPrenda)

                    If strItemCodePrenda <> "" Then
                        If blnGastosAdicionalesYaIngresa2 Then
                            p_factura.Expenses.Add()
                        End If
                        p_factura.Expenses.ExpenseCode = strItemCodePrenda

                        p_factura.Expenses.LineTotal = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, p_strMonedaConfiguradaFV, decGastosPrenda, p_DocCurrency, 1, dtFechaDocumento)

                        If strImpuesto <> "" Then
                            p_factura.Expenses.TaxCode = strImpuesto
                            p_factura.Expenses.VatGroup = strImpuesto
                        End If
                    End If
                End If

                strTipoVehiculo = Utilitarios.EjecutarConsulta("Select U_Tipo from dbo.[@SCGD_Vehiculo] WITH (nolock) where Code = '" & strIDVehiculo & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                If Not blnMultiplesFacturas = True Then

                    Call ManejarLineasFactura(p_factura, strNumeroContratoVenta, blnGastosAdicionalesYaIngresa2, strTipoVehiculo, oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT").GetValue("U_Impuesto", 0).Trim)

                End If

                If Not blnMultiplesFacturas = True OrElse (blnMultiplesFacturas = True AndAlso oMatriz.RowCount = 1) Then

                    intError = p_factura.Add()
                    m = m_oCompany.GetLastErrorDescription()

                    If intError <> 0 Then
                        m_oCompany.GetLastError(intError, m)
                        m_SBO_Application.SetStatusBarMessage(String.Format("{0} Error: {1} {2}", My.Resources.Resource.DocumentoFacturaVentas, intError, m))
                        Throw New ExceptionsSBO(intError, m)

                    Else
                        m_oCompany.GetNewObjectCode(strNFactura)
                        oForm.Items.Item("txtNofac").Specific.string = strNFactura

                    End If
                    '   m = m_oCompany.GetLastErrorDescription()
                    'CargarFactura(strNFactura)

                ElseIf blnMultiplesFacturas = True AndAlso oMatriz.RowCount > 1 Then
                    oForm.Items.Item("btnFacts").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                End If

            Next j


            Return intError

        Catch ex As Exception
            m_oCompany.GetLastError(intError, m)

            If intError <> 0 Then
                m_SBO_Application.SetStatusBarMessage(String.Format("{0} Error: {1} {2}", My.Resources.Resource.DocumentoFacturaVentas, intError, m))
                m = String.Format("{0} {1}", My.Resources.Resource.DocumentoFacturaVentas, m)
                Throw New ExceptionsSBO(intError, m)
            Else
                Throw New Exception(ex.Message)
            End If

        Finally
            Utilitarios.DestruirObjeto(p_factura)
            Utilitarios.DestruirObjeto(ofacturaTemporal)
            ActualizaTipoInventarioVeh(oListaAgrupadaVehiculos)
            oListaAgrupadaVehiculos.Clear()
            oListaTiposInventarios.Clear()
        End Try
    End Function

    Private Sub ActualizaTipoInventarioVeh(ByVal p_listaVehiculosAgrupadosPorTipos As List(Of ListaVehiculosAgrupadosPorTipo))
        Dim strCodUnid As String
        With oForm.DataSources.DBDataSources.Item("@SCGD_VEHIXCONT")
            For index As Integer = 0 To .Size - 1
                strCodUnid = .GetValue("U_Cod_Unid", index).Trim()
                If p_listaVehiculosAgrupadosPorTipos.Any(Function(veh) veh.CodigoUnidad.Trim().Equals(strCodUnid)) Then
                    .SetValue("U_TipIn", index, p_listaVehiculosAgrupadosPorTipos.First(Function(veh) veh.CodigoUnidad.Trim().Equals(strCodUnid)).TipoInventario)
                    Continue For
                End If
            Next
        End With
    End Sub

    'Public Function CargarFactura(ByVal p_intFactura As Integer) As SAPbobsCOM.Documents

    '    Dim m_oFac As SAPbobsCOM.Documents
    '    Dim m_lineoFac As SAPbobsCOM.Document_Lines

    '    Try
    '        m_oFac = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices), SAPbobsCOM.Documents)

    '        If m_oFac.GetByKey(p_intFactura) Then
    '            If Not m_oFac.DocumentStatus = BoStatus.bost_Close Then
    '                m_lineoFac = m_oFac.Lines
    '            End If
    '        End If

    '        For i As Integer = 0 To m_lineoFac.Count - 1
    '            m_lineoFac.SetCurrentLine(i)

    '            Dim item As String = m_lineoFac.ItemCode
    '            Dim des As String = m_lineoFac.ItemDescription
    '        Next
    '    Catch ex As Exception

    '    End Try

    '    Return Nothing

    'End Function

    Public Function BuscarUsadosEnContratos(p_oListaAgrupadaVehiculos As Generic.List(Of ListaVehiculosAgrupadosPorTipo)) As Boolean

        oListaTiposInventarios = New Generic.List(Of String)

        For Each linea As ListaVehiculosAgrupadosPorTipo In p_oListaAgrupadaVehiculos

            If Not oListaTiposInventarios.Contains(linea.Tipo) Then
                oListaTiposInventarios.Add(linea.Tipo)
            End If

        Next

    End Function

    Public Function Pasarvalor(p_valor As Decimal) As Decimal

        Dim strSeparadorDecimalesSAP As String = String.Empty
        Dim strSeparadorMilesSAP As String = String.Empty
        Dim strValorSeleccionado As String = String.Empty


        Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

        If Not String.IsNullOrEmpty(p_valor) And Not p_valor = 0 Then

            If strSeparadorDecimalesSAP <> "," Then
                strValorSeleccionado = p_valor
                strValorSeleccionado = strValorSeleccionado.Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                p_valor = Convert.ToDecimal(strValorSeleccionado)
            Else
                strValorSeleccionado = p_valor
                strValorSeleccionado = strValorSeleccionado.Replace(strSeparadorDecimalesSAP, strSeparadorMilesSAP)
                p_valor = Convert.ToDecimal(strValorSeleccionado)
            End If


            Return p_valor
        End If

    End Function

    Public Sub AgruparVehiculos(ByRef p_matrizVehiculosXContrato As SAPbouiCOM.Matrix, Optional p_TipoInventario As String = "", Optional p_Recorrer As Boolean = True)

        Dim xmlDocMatrix As Xml.XmlDocument
        Dim matrixXml As String

        Dim elementoUnidad As Xml.XmlNode
        Dim elementoMarca As Xml.XmlNode
        Dim elementoEstilo As Xml.XmlNode
        Dim elementoModelo As Xml.XmlNode
        Dim elementoColor As Xml.XmlNode
        Dim elementoMotor As Xml.XmlNode
        Dim elementoVIN As Xml.XmlNode
        Dim elementoAño As Xml.XmlNode
        Dim elementoPlaca As Xml.XmlNode
        Dim elementoTransmi As Xml.XmlNode
        Dim elementoTipoInv As Xml.XmlNode
        Dim elementoInterior As Xml.XmlNode
        Dim elementoObservacion As Xml.XmlNode
        Dim elementoPrecioNeto As Xml.XmlNode
        Dim elementoPrecio As Xml.XmlNode
        Dim elementoBono As Xml.XmlNode
        Dim elementoDescuento As Xml.XmlNode
        Dim elementoMDesc As Xml.XmlNode
        Dim elementoImpuesto As Xml.XmlNode
        Dim elementoPreTot As Xml.XmlNode
        Dim elementoPagos As Xml.XmlNode
        Dim elementoMAcc As Xml.XmlNode
        Dim elementoGL As Xml.XmlNode
        Dim elementoOG As Xml.XmlNode
        Dim elementoKmSale As Xml.XmlNode
        Dim decPrecioNeto As Decimal '= Utilitarios.ConvierteDecimal(elementoPrecioNeto.InnerText.Trim, n)
        Dim decPrecio As Decimal '= Utilitarios.ConvierteDecimal(elementoPrecio.InnerText.Trim, n)
        Dim decBono As Decimal '= Utilitarios.ConvierteDecimal(elementoBono.InnerText.Trim, n)
        Dim decDescuento As Decimal '= Utilitarios.ConvierteDecimal(elementoDescuento.InnerText.Trim, n)
        Dim decPrecioTotal As Decimal '= Utilitarios.ConvierteDecimal(elementoPreTot.InnerText.Trim, n)
        Dim decPagos As Decimal '= Utilitarios.ConvierteDecimal(elementoPagos.InnerText.Trim, n)
        Dim decMAcc As Decimal '= Utilitarios.ConvierteDecimal(elementoMAcc.InnerText.Trim, n)
        Dim intGL As Integer
        Dim intOG As Integer
        Dim intKmSale As Integer
        Dim strTipoInventario As String
        Dim counter As Integer = 0
        Dim strConsulta As String = "select tv.U_Usado from [@SCGD_VEHICULO] as ve  with (nolock) inner join [@SCGD_TIPOVEHICULO] as tv  with (nolock) on tv.Code = ve.U_Tipo where U_Cod_Unid ='{0}'"

        matrixXml = p_matrizVehiculosXContrato.SerializeAsXML(BoMatrixXmlSelect.mxs_All)
        xmlDocMatrix = New Xml.XmlDocument
        xmlDocMatrix.LoadXml(matrixXml)

        For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")

            elementoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Unidad']")
            elementoMarca = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Marca']")
            elementoEstilo = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Estilo']")
            elementoModelo = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Modelo']")
            elementoColor = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Color']")
            elementoMotor = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Motor']")
            elementoVIN = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_VIN']")
            elementoAño = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Año']")
            elementoPlaca = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Placa']")
            elementoTransmi = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Transm']")
            elementoTipoInv = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Tipinv']")
            elementoInterior = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Colint']")
            elementoObservacion = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Obser']")
            elementoPrecioNeto = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_PreNet']")
            elementoPrecio = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Prec']")
            elementoBono = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Bono']")
            elementoDescuento = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Desc']")
            elementoMDesc = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_MDesc']")
            elementoImpuesto = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Imp']")
            elementoPreTot = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_PreTot']")
            elementoPagos = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_Pagos']")
            elementoMAcc = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_MAcc']")
            elementoGL = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_GL']")
            elementoOG = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_OG']")
            elementoKmSale = node.SelectSingleNode("Columns/Column/Value[../ID = 'Col_KmSale']")

            If Not String.IsNullOrEmpty(elementoUnidad.InnerText) Then

                strTipoInventario = Utilitarios.EjecutarConsulta(String.Format(strConsulta, elementoUnidad.InnerText)).Trim()
                If String.IsNullOrEmpty(strTipoInventario) Then strTipoInventario = "N"
                If strTipoInventario = p_TipoInventario Then
                    strTipoInventario = Utilitarios.EjecutarConsulta(String.Format(" SELECT U_Tipo FROM [@SCGD_VEHICULO] WITH (nolock) WHERE U_Cod_Unid = '{0}' ", elementoUnidad.InnerText.Trim)).Trim()
                    If String.IsNullOrEmpty(strTipoInventario) Then strTipoInventario = elementoTipoInv.InnerText.Trim
                    oListaAgrupadaVehiculos.Add(New ListaVehiculosAgrupadosPorTipo() With {.CodigoUnidad = elementoUnidad.InnerText.Trim,
                                                                                                        .Marca = elementoMarca.InnerText.Trim,
                                                                                                        .Estilo = elementoEstilo.InnerText.Trim,
                                                                                                        .Modelo = elementoModelo.InnerText.Trim,
                                                                                                        .Motor = elementoMotor.InnerText.Trim,
                                                                                                        .VIN = elementoVIN.InnerText.Trim,
                                                                                                        .Anno = elementoAño.InnerText.Trim,
                                                                                                        .Placa = elementoPlaca.InnerText.Trim,
                                                                                                        .Transmision = elementoTransmi.InnerText.Trim,
                                                                                                        .TipoInventario = strTipoInventario,
                                                                                                        .ColorInterior = elementoColor.InnerText.Trim,
                                                                                                        .Observacion = elementoObservacion.InnerText.Trim,
                                                                                                        .PrecioNeto = decPrecioNeto,
                                                                                                        .Precio = decPrecio,
                                                                                                        .Bono = decBono,
                                                                                                        .Descuento = decDescuento,
                                                                                                        .MDesc = elementoMDesc.InnerText.Trim,
                                                                                                        .Impuesto = elementoImpuesto.InnerText.Trim,
                                                                                                        .PrecioTotal = decPrecioTotal,
                                                                                                        .Pagos = decPagos,
                                                                                                        .MAcc = decMAcc,
                                                                                                        .GL = intGL,
                                                                                                        .OG = intOG,
                                                                                                        .KmSale = intKmSale,
                                                                                                        .Fila = counter,
                                                                                                        .Tipo = p_TipoInventario,
                                                                                                        .Color = elementoColor.InnerText.Trim})


                End If
            End If

            counter += 1
        Next

        If p_Recorrer Then
            AgruparVehiculos(p_matrizVehiculosXContrato, "Y", False)
        End If
    End Sub

    Private Sub GenerarDocumentoParaVehiculosUsados(ByVal p_strNoFactura As String, _
                                               ByVal p_strCliente As String, _
                                               ByVal p_strIDVehiculo As String, _
                                               ByVal p_strNumeroContratoVenta As String, _
                                               ByVal p_strDocCurrency As String, _
                                               ByRef p_strNoDocumento As String, _
                                               ByRef p_strComentarioUsado As String, _
                                               ByRef p_strCodUnidad As String, _
                                               ByRef decMontoDocumento As Decimal, _
                                               ByRef blnNotaCredUsado As Boolean, ByVal oMatrixUsado As SAPbouiCOM.Matrix, p_TipoDocumento As TipoDocumentoVehiculoUsado, Optional ByVal strMonedaConfigurada As String = "", Optional blnUsaDistincionSN_ReciboUsado As Boolean = False)

        Dim decMontoDocumentoUsado As Decimal
        Dim strComentarioParaLinea As String

        Dim intError As Integer
        Dim strMensajeError As String = String.Empty
        Dim decMontoReal As Decimal

        Dim oDocumentoVehiculoUsado As SAPbobsCOM.Documents
        Dim strFacturaProveedor As String = String.Empty
        Dim strNotaCredito As String = String.Empty

        Dim intSerieDocumento As Integer

        Dim strMensajeSuma As String
        Dim strImpuesto As String
        Dim strCuentaFacturaProveedor As String

        Dim decMontoUsado As Decimal
        Dim strDescMarca As String
        Dim strDescEstilo As String
        Dim strAñoVehiculo As String
        Dim strDescColor As String
        Dim strCodUnidad As String
        Dim strVIN As String
        Dim strPlaca As String
        Dim strTipo As String
        Dim strValorDimension As String = String.Empty
        Dim strInventarioUsado As String
        Dim intInventarioUsado As String
        Dim objConfiguracionGeneralUsado As ConfiguracionesGeneralesAddon
        Dim blnBoolean As Boolean
        Dim oCABYS As CABYS
        decMontoDocumentoUsado = Utilitarios.ConvierteDecimal(oForm.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Mon_Usa", 0), n)

        'cambia el monto a moneda local
        decMontoReal = decMontoDocumentoUsado
        decMontoDocumentoUsado = decMontoDocumentoUsado
        decMontoDocumento = decMontoDocumentoUsado
        oCABYS = New CABYS()
        If decMontoReal > 0 Then

            Select Case p_TipoDocumento

                Case TipoDocumentoVehiculoUsado.FacturaProveedor


                    oDocumentoVehiculoUsado = Nothing

                    oDocumentoVehiculoUsado = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices),  _
                                                                          SAPbobsCOM.Documents)


                    If blnUsaDimensiones Then
                        Dim strFacturaProveedorVehiculoUsado As String = ConfiguracionesGeneralesAddon.scgTipoDocumentosCV.FacturaProveedorVehiculoUsado
                        strValorDimension = ListaConfiguracion.Item(strFacturaProveedorVehiculoUsado)
                    End If

                Case TipoDocumentoVehiculoUsado.NotaCredito

                    oDocumentoVehiculoUsado = Nothing

                    oDocumentoVehiculoUsado = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes),  _
                                                                             SAPbobsCOM.Documents)


                    Dim strIndicador As String = Utilitarios.DevuelveCodIndicadores(m_SBO_Application, "4")

                    If Not String.IsNullOrEmpty(strIndicador) Then

                        oDocumentoVehiculoUsado.Indicator = strIndicador

                    End If

                    If blnUsaDimensiones Then
                        Dim strNotaCreditoUsado As String = ConfiguracionesGeneralesAddon.scgTipoDocumentosCV.NotasCreditoUsados
                        strValorDimension = ListaConfiguracion.Item(strNotaCreditoUsado)
                    End If

            End Select

            'distinguir entre Sociedades o Privado en el Dato maestro de Socios de Negocio
            If Not blnUsaDistincionSN_ReciboUsado Then

                strInventarioUsado = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Tipo", 0).Trim
                intInventarioUsado = Utilitarios.EjecutarConsulta(String.Format("SELECT code FROM [@SCGD_TIPOVEHICULO] with(nolock) WHERE name = '{0}'", strInventarioUsado), m_oCompany.CompanyDB, m_oCompany.Server)

                objConfiguracionGeneralUsado = New ConfiguracionesGeneralesAddon(intInventarioUsado, m_cn_Coneccion, blnBoolean)

                Select Case p_TipoDocumento
                    Case TipoDocumentoVehiculoUsado.FacturaProveedor
                        intSerieDocumento = objConfiguracionGeneralUsado.Serie(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaProveedor)
                    Case TipoDocumentoVehiculoUsado.NotaCredito
                        intSerieDocumento = objConfiguracionGeneralUsado.Serie(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.NotasCreditoUsados)
                End Select

            End If

            If intSerieDocumento <> -1 Then
                oDocumentoVehiculoUsado.Series = intSerieDocumento
            End If

            p_strCodUnidad = p_strCodUnidad

            If p_TipoDocumento = TipoDocumentoVehiculoUsado.NotaCredito Then
                oDocumentoVehiculoUsado.CardCode = p_strCliente
            End If

            'agrego fecha para el documento
            oDocumentoVehiculoUsado.DocDate = dtFechaDocumento


            If Not String.IsNullOrEmpty(strMonedaConfigurada) Then
                oDocumentoVehiculoUsado.DocCurrency = strMonedaConfigurada
                'p_strDocCurrency = strMoneda
            Else
                oDocumentoVehiculoUsado.DocCurrency = p_strDocCurrency
            End If

            ' Usa Tipo Cambio Contrato 
            If strUsaTCContrato = "Y" And m_decTipoCambio > 0 Then
                If Not String.IsNullOrEmpty(strMonedaConfigurada) And p_strDocCurrency = strMonedaConfigurada Then
                    oDocumentoVehiculoUsado.DocRate = m_decTipoCambio
                ElseIf String.IsNullOrEmpty(strMonedaConfigurada) Then
                    oDocumentoVehiculoUsado.DocRate = m_decTipoCambio
                End If
            End If

            'Le pongo descuento 0 a la Nota de Credito
            oDocumentoVehiculoUsado.DiscountPercent = 0


            If p_strDocCurrency = m_strMonedaLocal Then
                strMensajeSuma = My.Resources.Resource.EnUnValorDe & p_strDocCurrency & " " & String.Format("{0,10:N}", decMontoDocumentoUsado)
            Else

                decMontoDocumentoUsado = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, strMonedaConfigurada, decMontoDocumentoUsado, p_strDocCurrency, 1, dtFechaDocumento)

                'para los comentarios en las facturas y notas de credito
                If decMontoReal = decMontoDocumentoUsado Then

                    'm_decTipoCambio = Utilitarios.ConvierteDecimal(oForm.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_SCGD_TipoCambio", 0), n)

                    Dim DecNotasCreditoComentario As Decimal = decMontoDocumentoUsado * m_decTipoCambio
                    Dim valor As Decimal = FormatNumber(DecNotasCreditoComentario, n.NumberDecimalDigits)

                    strMensajeSuma = My.Resources.Resource.EnUnValorDe & p_strDocCurrency & String.Format("{0,10:N}", decMontoReal) & " (" & m_strMonedaLocal & " " & String.Format("{0,10:N}", valor) & " " & My.Resources.Resource.TipoCambio & ": " & m_decTipoCambio.ToString("n2") & ")" 'String.Format("{0,10:N}", m_decTipoCambio) & ")"
                Else
                    strMensajeSuma = My.Resources.Resource.EnUnValorDe & p_strDocCurrency & String.Format("{0,10:N}", decMontoReal) & " (" & m_strMonedaLocal & " " & String.Format("{0,10:N}", decMontoDocumentoUsado) & " " & My.Resources.Resource.TipoCambio & ": " & m_decTipoCambio.ToString("n2") & ")" '& String.Format("{0,10:N}", m_decTipoCambio) & ")"
                End If


            End If

            If oMatrixUsado.RowCount > 1 Then

                p_strComentarioUsado = p_strComentarioUsado & strMensajeSuma & " " & My.Resources.Resource.ReferenciaCV & ": " & p_strNumeroContratoVenta

            Else

                p_strComentarioUsado = My.Resources.Resource.RecibimosVehículo & p_strComentarioUsado & strMensajeSuma & " " & My.Resources.Resource.ReferenciaCV & ": " & p_strNumeroContratoVenta

            End If

            oDocumentoVehiculoUsado.Comments = p_strComentarioUsado
            If Not String.IsNullOrEmpty(p_strNoFactura) Then
                oDocumentoVehiculoUsado.NumAtCard = p_strNoFactura
            End If

            oDocumentoVehiculoUsado.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service

            oDocumentoVehiculoUsado.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = p_strCodUnidad.Trim()
            oDocumentoVehiculoUsado.UserFields.Fields.Item("U_SCGD_NoContrato").Value = p_strNumeroContratoVenta.Trim()

            '******************INICIO | CABYS **************
            If DMS_Connector.Configuracion.ParamGenAddon.U_CABYS_CR = "Y" Then
                oCABYS.CardCode = p_strCliente
                ObtieneValoresExoneracionSN(oCABYS)
                If Not String.IsNullOrEmpty(oCABYS.OrigenTributario) Then oDocumentoVehiculoUsado.UserFields.Fields.Item("U_SCG_IVA2_LugarCons").Value = oCABYS.OrigenTributario
                If Not String.IsNullOrEmpty(oCABYS.TipoExoneracion) Then oDocumentoVehiculoUsado.UserFields.Fields.Item("U_SCG_IVA2_TipoExo").Value = oCABYS.TipoExoneracion
            End If
            '******************FIN | CABYS ******************
            'oDocumentoVehiculoUsado.Reference1 = p_strCodUnidad

            Dim strQueryInd As String = String.Empty
            For i As Integer = 0 To oMatrixUsado.RowCount - 1
                strQueryInd += String.Format(" Name = '{0}' OR", oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Tipo", i).TrimEnd(" "))
            Next
            strQueryInd = strQueryInd.TrimEnd("OR")
            strQueryInd = String.Format(" SELECT Name, U_Tipo, U_Cod_Imp FROM [@SCGD_ADMIN3] with (nolock) INNER JOIN [@SCGD_TIPOVEHICULO] AS TV with (nolock) ON U_Tipo = TV.Code WHERE ({0}) AND U_Cod_Item = {1} ",
                                        strQueryInd.Substring(0, strQueryInd.Length - 3), CInt(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.NotasCreditoUsados))

            Dim dtIndUsados As System.Data.DataTable = Utilitarios.EjecutarConsultaDataTable(strQueryInd, m_oCompany.CompanyDB, m_oCompany.Server)

            Dim FilaUsado As Integer = 0

            For i As Integer = 0 To oMatrixUsado.RowCount - 1

                If p_TipoDocumento = TipoDocumentoVehiculoUsado.FacturaProveedor And p_strCodUnidad = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Unid", i).Trim() Then

                    FilaUsado = i

                    Dim strCardCodeProveedor As String = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Prov", i).Trim()
                    'agrego nombre de proveedor para cada 
                    oDocumentoVehiculoUsado.CardCode = strCardCodeProveedor

                    'decMontoUsado = CDec(Utilitarios.CambiarValoresACultureActual(oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Val_Rec", i), strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    decMontoUsado = Utilitarios.ConvierteDecimal(oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Val_Rec", i), n)

                    'CDec(Utilitarios.CambiarValoresACultureActual(oForm.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Mon_Usa", 0), strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    strDescMarca = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Marca", i)
                    strDescMarca = strDescMarca.Trim()
                    strDescEstilo = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Estilo", i)
                    strDescEstilo = strDescEstilo.Trim()
                    strAñoVehiculo = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Anio", i)
                    strAñoVehiculo = strAñoVehiculo.Trim()
                    strDescColor = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Color", i)
                    strDescColor = strDescColor.Trim()
                    strCodUnidad = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Unid", i)
                    strCodUnidad = strCodUnidad.Trim()
                    strVIN = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_VIN", i)
                    strVIN = strVIN.Trim()
                    strPlaca = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Placa", i)
                    strPlaca = strPlaca.Trim()

                    Dim drIndUsado() As System.Data.DataRow = dtIndUsados.Select(String.Format(" Name = '{0}'", oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Tipo", i).TrimEnd(" ")))
                    strTipo = drIndUsado(0).Item("U_Tipo")
                    strImpuesto = drIndUsado(0).Item("U_Cod_Imp")

                    strCuentaFacturaProveedor = objConfiguracionGeneral.CuentaInventarioTransito(strTipo)

                    If blnUsaDistincionSN_ReciboUsado And p_TipoDocumento = TipoDocumentoVehiculoUsado.FacturaProveedor Then

                        Dim blnBEvento As Boolean
                        objConfiguracionGeneral = Nothing
                        objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(strTipo, m_cn_Coneccion, blnBEvento)
                        Dim strConsulta As String = "select U_TipSoc from [OCRD] with (nolock) where CardCode ='{0}'"
                        Dim strTipoSocioNegocio As String = Utilitarios.EjecutarConsulta(String.Format(strConsulta, strCardCodeProveedor), m_oCompany.CompanyDB, m_oCompany.Server)

                        If Not String.IsNullOrEmpty(strTipoSocioNegocio.Trim()) Then

                            If strTipoSocioNegocio = "S" Then
                                intSerieDocumento = objConfiguracionGeneral.Serie(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaProveedoresDocumentoReciboUsadoSociedades)

                                If intSerieDocumento <> -1 Then
                                    oDocumentoVehiculoUsado.Series = intSerieDocumento
                                End If

                                Dim strIndicador As String = Utilitarios.DevuelveCodIndicadores(m_SBO_Application, "12")

                                If Not String.IsNullOrEmpty(strIndicador) Then

                                    oDocumentoVehiculoUsado.Indicator = strIndicador

                                End If

                            ElseIf strTipoSocioNegocio = "P" Then

                                intSerieDocumento = objConfiguracionGeneral.Serie(SCGDataAccess.ConfiguracionesGeneralesAddon.scgTipoSeries.FacturaProveedoresDocumentoReciboUsadoPrivado)

                                If intSerieDocumento <> -1 Then
                                    oDocumentoVehiculoUsado.Series = intSerieDocumento
                                End If

                                Dim strIndicador As String = Utilitarios.DevuelveCodIndicadores(m_SBO_Application, "13")

                                If Not String.IsNullOrEmpty(strIndicador) Then

                                    oDocumentoVehiculoUsado.Indicator = strIndicador

                                End If

                            End If

                        End If

                    End If

                    'se realiza conversion de acuerdo a la mondena definida en BD
                    Dim MontoAConvertir As Decimal = decMontoUsado
                    Dim ValorReal As Decimal = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, strMonedaConfigurada, MontoAConvertir, p_strDocCurrency, 1, dtFechaDocumento)
                    oDocumentoVehiculoUsado.Lines.UnitPrice = ValorReal

                    oDocumentoVehiculoUsado.Lines.UserFields.Fields.Item("U_SCGD_Cod_Prov").Value = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Prov", i).ToString().Trim()
                    oDocumentoVehiculoUsado.Lines.UserFields.Fields.Item("U_SCGD_Nom_Prov").Value = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Nom_Prov", i).ToString().Trim()

                    decMontoReal = ValorReal
                    decMontoDocumentoUsado = ValorReal '* m_decTipoCambio
                    decMontoDocumento = ValorReal

                    'oNotaCredito.Lines.UnitPrice = decMontoUsado
                    If strImpuesto <> "" Then
                        oDocumentoVehiculoUsado.Lines.TaxCode = strImpuesto
                        oDocumentoVehiculoUsado.Lines.VatGroup = strImpuesto
                    End If

                    strComentarioParaLinea = p_strCodUnidad & " " & strDescMarca & " " & strDescEstilo & " " & strAñoVehiculo & " " & strDescColor & " " & strVIN & " " & strPlaca
                    strComentarioParaLinea = My.Resources.Resource.RecibimosVehículo & strComentarioParaLinea

                    If strComentarioParaLinea.Length <= 100 Then
                        oDocumentoVehiculoUsado.Lines.ItemDescription = strComentarioParaLinea
                    Else
                        oDocumentoVehiculoUsado.Lines.ItemDescription = strComentarioParaLinea.Substring(0, 100)
                    End If
                    oDocumentoVehiculoUsado.Lines.AccountCode = strCuentaFacturaProveedor

                    If blnUsaDimensiones Then
                        '******************************************************************************************
                        'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                        If Not String.IsNullOrEmpty(strValorDimension) Then
                            If strValorDimension = "Y" Then
                                Dim strCodigoMarca As String = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Marca_Us", i).TrimEnd(" ")
                                oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContables(oForm, strTipo, strCodigoMarca, oDataTableDimensionesContablesDMS))
                            End If
                        End If
                        '******************************************************************************************

                        If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then

                            ClsLineasDocumentosDimension.AgregarDimensionesLineasDocumentos(oDocumentoVehiculoUsado.Lines, oDataTableDimensionesContablesDMS)

                        End If

                        oDocumentoVehiculoUsado.Lines.Add()

                        Exit For

                    End If

                ElseIf p_TipoDocumento = TipoDocumentoVehiculoUsado.NotaCredito Then
                    'decMontoUsado = CDec(Utilitarios.CambiarValoresACultureActual(oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Val_Rec", i), strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    decMontoUsado = Utilitarios.ConvierteDecimal(oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Val_Rec", i), n)

                    'CDec(Utilitarios.CambiarValoresACultureActual(oForm.DataSources.DBDataSources.Item(TablaContrato).GetValue("U_Mon_Usa", 0), strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    strDescMarca = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Marca", i)
                    strDescMarca = strDescMarca.Trim()
                    strDescEstilo = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Estilo", i)
                    strDescEstilo = strDescEstilo.Trim()
                    strAñoVehiculo = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Anio", i)
                    strAñoVehiculo = strAñoVehiculo.Trim()
                    strDescColor = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Color", i)
                    strDescColor = strDescColor.Trim()
                    strCodUnidad = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Unid", i)
                    strCodUnidad = strCodUnidad.Trim()
                    strVIN = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_VIN", i)
                    strVIN = strVIN.Trim()
                    strPlaca = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Placa", i)
                    strPlaca = strPlaca.Trim()

                    Dim drIndUsado() As System.Data.DataRow = dtIndUsados.Select(String.Format(" Name = '{0}'", oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Tipo", i).TrimEnd(" ")))
                    strTipo = drIndUsado(0).Item("U_Tipo")
                    strImpuesto = drIndUsado(0).Item("U_Cod_Imp")

                    strCuentaFacturaProveedor = objConfiguracionGeneral.CuentaInventarioTransito(strTipo)

                    'se realiza conversion de acuerdo a la mondena definida en BD
                    Dim MontoAConvertir As Decimal = decMontoUsado
                    Dim ValorReal As Decimal = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, strMonedaConfigurada, MontoAConvertir, p_strDocCurrency, 1, dtFechaDocumento)
                    oDocumentoVehiculoUsado.Lines.UnitPrice = ValorReal

                    oDocumentoVehiculoUsado.Lines.UserFields.Fields.Item("U_SCGD_Cod_Prov").Value = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Prov", i).ToString().Trim()
                    oDocumentoVehiculoUsado.Lines.UserFields.Fields.Item("U_SCGD_Nom_Prov").Value = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Nom_Prov", i).ToString().Trim()

                    decMontoReal = ValorReal
                    decMontoDocumentoUsado = ValorReal '* m_decTipoCambio
                    decMontoDocumento = ValorReal

                    'oNotaCredito.Lines.UnitPrice = decMontoUsado
                    If strImpuesto <> "" Then
                        oDocumentoVehiculoUsado.Lines.TaxCode = strImpuesto
                        oDocumentoVehiculoUsado.Lines.VatGroup = strImpuesto
                    End If

                    strComentarioParaLinea = strCodUnidad & " " & strDescMarca & " " & strDescEstilo & " " & strAñoVehiculo & " " & strDescColor & " " & strVIN & " " & strPlaca
                    strComentarioParaLinea = My.Resources.Resource.RecibimosVehículo & strComentarioParaLinea

                    If strComentarioParaLinea.Length <= 100 Then
                        oDocumentoVehiculoUsado.Lines.ItemDescription = strComentarioParaLinea
                    Else
                        oDocumentoVehiculoUsado.Lines.ItemDescription = strComentarioParaLinea.Substring(0, 100)
                    End If
                    oDocumentoVehiculoUsado.Lines.AccountCode = strCuentaFacturaProveedor

                    If blnUsaDimensiones Then
                        '******************************************************************************************
                        'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                        If Not String.IsNullOrEmpty(strValorDimension) Then
                            If strValorDimension = "Y" Then
                                Dim strCodigoMarca As String = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Marca_Us", i).TrimEnd(" ")
                                oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContables(oForm, strTipo, strCodigoMarca, oDataTableDimensionesContablesDMS))
                            End If
                        End If
                        '******************************************************************************************

                        If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then

                            ClsLineasDocumentosDimension.AgregarDimensionesLineasDocumentos(oDocumentoVehiculoUsado.Lines, oDataTableDimensionesContablesDMS)

                        End If
                    End If
                    '******************INICIO | CABYS **************
                    If DMS_Connector.Configuracion.ParamGenAddon.U_CABYS_CR = "Y" Then
                        If Not String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_CABYS_AE", i)) Then oDocumentoVehiculoUsado.Lines.UserFields.Fields.Item("U_SCG_IVA2_Act_Econ").Value = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_CABYS_AE", i)
                        If Not String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_CABYS_TI", i)) Then oDocumentoVehiculoUsado.Lines.UserFields.Fields.Item("U_SCG_IVA2_TipoItem").Value = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_CABYS_TI", i)
                        If Not String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_CABYS_CH", i)) Then oDocumentoVehiculoUsado.Lines.UserFields.Fields.Item("U_SCG_IVA2_CodItem").Value = oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_CABYS_CH", i)
                    End If
                    '******************FIN | CABYS ******************
                    oDocumentoVehiculoUsado.Lines.Add()

                End If

            Next

            intError = oDocumentoVehiculoUsado.Add()
            If intError <> 0 Then
                '              
                m_oCompany.GetLastError(intError, strMensajeError)
                Throw New ExceptionsSBO(intError, strMensajeError)
                '                End If
            Else

                If p_TipoDocumento = TipoDocumentoVehiculoUsado.FacturaProveedor Then
                    m_oCompany.GetNewObjectCode(strFacturaProveedor)
                    'oForm.Items.Item("txtNoFPU").Specific.String = strFacturaProveedor
                    p_strNoDocumento = strFacturaProveedor

                    strNumeroFacturaProveedorVU = strFacturaProveedor

                    If oMatrixUsado.RowCount = 1 Then
                        oForm.Items.Item("txtNoFPU").Specific.String = strFacturaProveedor
                    ElseIf oMatrixUsado.RowCount > 1 Then
                        oForm.Items.Item("txtNoFPU").Specific.String = "Multiples Veh. Usados"
                    End If

                    blnNotaCredUsado = False

                ElseIf p_TipoDocumento = TipoDocumentoVehiculoUsado.NotaCredito Then

                    m_oCompany.GetNewObjectCode(strNotaCredito)
                    oForm.Items.Item("txtNot_us").Specific.String = strNotaCredito
                    p_strNoDocumento = strNotaCredito

                    blnNotaCredUsado = True

                End If

            End If

        End If

    End Sub

    Public Function GeneraAsientoAdicionalFacturaProveedorVU(p_CardCodeCliente As String, p_CardCodeProveedor As String, ByVal p_strMonedaConfiguradaAsientoAdicionalFVU As String, ByVal p_strDocCurrency As String,
                                    ByVal p_strContrato As String, p_MontoUsado As Double,
                                    ByVal p_strUnidad As String, Optional p_blnUsaCompensacionAsEnt As Boolean = False, Optional p_strFechaContrato As String = "") As Integer

        Dim oJournalEntry As SAPbobsCOM.JournalEntries

        Dim intError As Integer
        Dim strMensajeError As String = ""
        Dim strMoneda As String
        Dim strAsiento As String
        Dim blnAgregarDimensiones As Boolean = False
        Dim strFechaCont As String
        Dim dtFechaCont As Date

        If p_blnUsaCompensacionAsEnt Then
            strFechaCont = p_strFechaContrato.Trim()

            If Not String.IsNullOrEmpty(strFechaCont) Then
                dtFechaCont = Date.ParseExact(strFechaCont, "yyyyMMdd", Nothing)
                dtFechaCont = New Date(dtFechaCont.Year, dtFechaCont.Month, dtFechaCont.Day, 0, 0, 0)
            Else
                dtFechaCont = Date.Now
            End If

        End If

        Try

            If Not String.IsNullOrEmpty(p_strMonedaConfiguradaAsientoAdicionalFVU) Then
                strMoneda = p_strMonedaConfiguradaAsientoAdicionalFVU
            Else
                strMoneda = p_strDocCurrency
            End If

            oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            oJournalEntry.ReferenceDate = dtFechaDocumento

            oJournalEntry.Reference = p_strUnidad
            oJournalEntry.Reference2 = p_strContrato

            oJournalEntry.Memo = String.Format("{0} {1}", "Asiento Factura Prov VU - " & p_strUnidad, p_strContrato)

            If blnUsaDimensiones Then

                Dim strCodigounidad As String = p_strUnidad

                Dim strNombreInventario As String = Utilitarios.EjecutarConsulta("Select U_Tipo From dbo.[@SCGD_USADOXCONT] WITH (nolock) Where U_Cod_Unid = '" & strCodigounidad.Trim & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                Dim strTipoInventario As String = Utilitarios.EjecutarConsulta("Select Code from [@SCGD_TIPOVEHICULO] with (nolock) where Name = '" & strNombreInventario.Trim() & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                Dim strCodigoMarca As String = Utilitarios.EjecutarConsulta("Select U_Cod_Marca_Us from dbo.[@SCGD_USADOXCONT] WITH (nolock) where U_Cod_Unid = '" & strCodigounidad.Trim & "'", m_oCompany.CompanyDB, m_oCompany.Server)


                oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContables(oForm, strTipoInventario, strCodigoMarca, oDataTableDimensionesContablesDMS))

                If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then

                    blnAgregarDimensiones = True
                Else
                    blnAgregarDimensiones = False

                End If
            End If


            oJournalEntry.Lines.ShortName = p_CardCodeProveedor
            If strMoneda = m_strMonedaLocal Then
                oJournalEntry.Lines.Debit = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, strMoneda, p_MontoUsado, strMoneda, 1, dtFechaDocumento)
            Else
                'para valores no compensados en ME
                If p_blnUsaCompensacionAsEnt Then
                    oJournalEntry.Lines.Debit = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, m_strMonedaLocal, p_MontoUsado, strMoneda, 1, dtFechaCont)
                End If

                oJournalEntry.Lines.FCDebit = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, strMoneda, p_MontoUsado, strMoneda, 1, dtFechaDocumento)
                oJournalEntry.Lines.FCCurrency = strMoneda
            End If
            'oJournalEntry.Lines.Reference1 = .Comentarios
            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

            If blnAgregarDimensiones Then

                ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)

            End If

            oJournalEntry.Lines.Add()

            oJournalEntry.Lines.ShortName = p_CardCodeCliente
            If strMoneda = m_strMonedaLocal Then
                oJournalEntry.Lines.Credit = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, strMoneda, p_MontoUsado, strMoneda, 1, dtFechaDocumento)
            Else
                'para valores no compensados en ME
                If p_blnUsaCompensacionAsEnt Then
                    oJournalEntry.Lines.Credit = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, m_strMonedaLocal, p_MontoUsado, strMoneda, 1, dtFechaCont)
                End If

                oJournalEntry.Lines.FCCredit = Utilitarios.CalcularCostosPorCambioMoneda(m_oCompany, strMoneda, p_MontoUsado, strMoneda, 1, dtFechaDocumento)
                oJournalEntry.Lines.FCCurrency = strMoneda
            End If
            oJournalEntry.Lines.Reference1 = p_strUnidad
            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

            If blnAgregarDimensiones Then

                ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)

            End If

            oJournalEntry.Lines.Add()


            If oJournalEntry.Add <> 0 Then
                m_oCompany.GetLastError(intError, strMensajeError)
                If m_oCompany.InTransaction() Then
                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If
            Else
                m_oCompany.GetNewObjectCode(strAsiento)
                strNumeroAsientoAdicionalVU = strAsiento

            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try


    End Function

    Public Sub ActualizaLineaVehiculosUsados(ByRef p_form As SAPbouiCOM.Form, ByVal p_strDocEntry As String, p_lista As Generic.List(Of ListaDocumentosFacturaVehiculosUsados), ByRef p_matriz As SAPbouiCOM.Matrix)
        Try
            Dim oCompanyServiceTraslado As SAPbobsCOM.CompanyService
            Dim oGeneralServiceTraslado As SAPbobsCOM.GeneralService
            Dim oGeneralDataTraslado As SAPbobsCOM.GeneralData
            Dim oGeneralParamsTraslado As SAPbobsCOM.GeneralDataParams
            Dim oChildTraslado As SAPbobsCOM.GeneralData
            Dim oChildrenTraslado As SAPbobsCOM.GeneralDataCollection

            oCompanyServiceTraslado = m_oCompany.GetCompanyService()
            oGeneralServiceTraslado = oCompanyServiceTraslado.GetGeneralService("SCGD_CVT")
            oGeneralParamsTraslado = oGeneralServiceTraslado.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)

            oGeneralParamsTraslado.SetProperty("DocEntry", p_strDocEntry)
            oGeneralDataTraslado = oGeneralServiceTraslado.GetByParams(oGeneralParamsTraslado)
            oChildrenTraslado = oGeneralDataTraslado.Child("SCGD_USADOXCONT")

            Dim numerolineas As Integer = oChildrenTraslado.Count

            For j As Integer = 0 To p_lista.Count - 1

                oChildTraslado = oChildrenTraslado.Item(j)
                oChildTraslado.SetProperty("U_N_FP", p_lista.Item(j).FacturaProveedor)
                oChildTraslado.SetProperty("U_N_AsAd", p_lista.Item(j).Asiento)
                oGeneralServiceTraslado.Update(oGeneralDataTraslado)

                p_form.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").SetValue("U_N_FP", j, p_lista.Item(j).FacturaProveedor)
                p_form.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").SetValue("U_N_AsAd", j, p_lista.Item(j).Asiento)

            Next


        Catch ex As Exception

        Finally

            p_matriz.LoadFromDataSource()

        End Try
    End Sub

    Private Sub AsignarNumeracionTipoFacturaDev(ByRef p_oFactura As Documents, ByRef p_impuesto As String, ByVal p_blnTipoInventarioUsado As Boolean, ByVal p_strTipoInv As String, ByVal p_blnConsig As Boolean, ByVal p_blnFacExeVehiUsado As Boolean)
        Dim intSerieFactura As Integer

        Try
            If p_blnFacExeVehiUsado Then
                If p_blnTipoInventarioUsado Then
                    If p_blnConsig Then
                        intSerieFactura = DMS_Connector.Helpers.GetSerie(p_strTipoInv, DMS_Connector.Data_Access.GeneralEnums.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado, True)
                        p_impuesto = DMS_Connector.Helpers.GetImpuesto(p_strTipoInv, DMS_Connector.Data_Access.GeneralEnums.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado)
                    Else
                        intSerieFactura = DMS_Connector.Helpers.GetSerie(p_strTipoInv, DMS_Connector.Data_Access.GeneralEnums.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado, False)
                        p_impuesto = DMS_Connector.Helpers.GetImpuesto(p_strTipoInv, DMS_Connector.Data_Access.GeneralEnums.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado)
                    End If
                Else
                    If p_blnConsig Then
                        intSerieFactura = DMS_Connector.Helpers.GetSerie(p_strTipoInv, DMS_Connector.Data_Access.GeneralEnums.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado, True)
                        p_impuesto = DMS_Connector.Helpers.GetImpuesto(p_strTipoInv, DMS_Connector.Data_Access.GeneralEnums.scgTipoSeries.FacturaExentaDeudoresVehiculoUsado)
                    Else
                        intSerieFactura = DMS_Connector.Helpers.GetSerie(p_strTipoInv, DMS_Connector.Data_Access.GeneralEnums.scgTipoSeries.FacturaVentas, False)
                        p_impuesto = DMS_Connector.Helpers.GetImpuesto(p_strTipoInv, DMS_Connector.Data_Access.GeneralEnums.scgTipoSeries.FacturaVentas)
                    End If
                End If
            Else
                intSerieFactura = DMS_Connector.Helpers.GetSerie(p_strTipoInv, DMS_Connector.Data_Access.GeneralEnums.scgTipoSeries.FacturaVentas, False)
                p_impuesto = DMS_Connector.Helpers.GetImpuesto(p_strTipoInv, DMS_Connector.Data_Access.GeneralEnums.scgTipoSeries.FacturaVentas)
            End If
            If intSerieFactura <> -1 AndAlso intSerieFactura <> 0 Then
                p_oFactura.Series = intSerieFactura
                p_oFactura.DocumentSubType = TipoFactura2(intSerieFactura)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Función que asigna sub tipo de documento basado en la numeración configurada
    ''' </summary>
    ''' <param name="p_serie">Serie configurada</param>
    ''' <returns>Sub Tipo de documento</returns>
    ''' <remarks></remarks>
    Public Function TipoFactura2(p_serie As Integer) As BoDocumentSubType
        Dim oSeriesService As SeriesService
        Dim oSeriesCollection As SeriesCollection
        Dim oSeries As Series
        Dim oDocumentTypeParams As DocumentTypeParams
        Dim oSubType As BoDocumentSubType
        Dim blnExit As Boolean
        Try
            blnExit = False
            oSeriesService = DMS_Connector.Company.CompanyService.GetBusinessService(ServiceTypes.SeriesService)
            oSeriesCollection = oSeriesService.GetDataInterface(SeriesServiceDataInterfaces.ssdiSeriesCollection)
            oDocumentTypeParams = oSeriesService.GetDataInterface(SeriesServiceDataInterfaces.ssdiDocumentTypeParams)
            oDocumentTypeParams.Document = 13
            For Each row As DataRow In Utilitarios.EjecutarConsultaDataTable(" SELECT DISTINCT ""DocSubType"" FROM ""NNM1"" WHERE ""ObjectCode"" = '13' AND ""DocSubType"" IN ('--','IB','IE','IX') ").Rows
                oDocumentTypeParams.DocumentSubType = CStr(row.Item(0))
                oSeriesCollection = oSeriesService.GetDocumentSeries(oDocumentTypeParams)
                For index As Integer = 0 To oSeriesCollection.Count - 1
                    oSeries = oSeriesCollection.Item(index)
                    If oSeries.Series = p_serie Then
                        Select Case oSeries.DocumentSubType
                            Case "IB" 'Boleta
                                oSubType = BoDocumentSubType.bod_Bill
                            Case "IE" 'Factura Exenta
                                oSubType = BoDocumentSubType.bod_InvoiceExempt
                            Case "IX" 'Factura Exportacion
                                oSubType = BoDocumentSubType.bod_ExportInvoice
                            Case Else
                                oSubType = BoDocumentSubType.bod_None
                        End Select
                        blnExit = True
                        Exit For
                    End If
                Next
                If blnExit Then
                    Exit For
                End If
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            oSubType = BoDocumentSubType.bod_None
        Finally
            Utilitarios.DestruirObjeto(oSeriesService)
            Utilitarios.DestruirObjeto(oSeriesCollection)
            Utilitarios.DestruirObjeto(oSeries)
            Utilitarios.DestruirObjeto(oDocumentTypeParams)
        End Try
        Return oSubType
    End Function

End Class

Public Class ListaDocumentosFacturaVehiculosUsados

    Public _codigounidad As String

    Public Property CodigoUnidad As String
        Get
            Return _codigounidad

        End Get
        Set(value As String)
            _codigounidad = value

        End Set
    End Property

    Public _facturaproveedor As String

    Public Property FacturaProveedor As String
        Get
            Return _facturaproveedor

        End Get
        Set(value As String)
            _facturaproveedor = value

        End Set
    End Property

    Public _asiento As String

    Public Property Asiento As String
        Get
            Return _asiento

        End Get
        Set(value As String)
            _asiento = value

        End Set
    End Property

    Public _fila As String

    Public Property Fila As String
        Get
            Return _fila

        End Get
        Set(value As String)
            _fila = value

        End Set
    End Property
End Class


Public Class ListaVehiculosAgrupadosPorTipo

    Public Property CodigoUnidad As String
        Get
            Return _codigounidad
        End Get
        Set(ByVal value As String)
            _codigounidad = value
        End Set
    End Property
    Private _codigounidad As String

    Public Property Tipo As String
        Get
            Return _tipo
        End Get
        Set(ByVal value As String)
            _tipo = value
        End Set
    End Property
    Private _tipo As String


    Public Property Marca() As String
        Get
            Return _marca
        End Get
        Set(ByVal value As String)
            _marca = value
        End Set
    End Property
    Private _marca As String

    Public Property Estilo() As String
        Get
            Return _estilo
        End Get
        Set(ByVal value As String)
            _estilo = value
        End Set
    End Property
    Private _estilo As String


    Public Property Modelo() As String
        Get
            Return _modelo
        End Get
        Set(ByVal value As String)
            _modelo = value
        End Set
    End Property
    Private _modelo As String

    Public Property Color() As String
        Get
            Return _color
        End Get
        Set(ByVal value As String)
            _color = value
        End Set
    End Property
    Private _color As String

    Public Property Motor() As String
        Get
            Return _motor
        End Get
        Set(ByVal value As String)
            _motor = value
        End Set
    End Property
    Private _motor As String

    Public Property VIN() As String
        Get
            Return _vin
        End Get
        Set(ByVal value As String)
            _vin = value
        End Set
    End Property
    Private _vin As String

    Public Property Anno() As String
        Get
            Return _anno
        End Get
        Set(ByVal value As String)
            _anno = value
        End Set
    End Property
    Private _anno As String

    Public Property Placa() As String
        Get
            Return _placa
        End Get
        Set(ByVal value As String)
            _placa = value
        End Set
    End Property
    Private _placa As String


    Public Property Transmision() As String
        Get
            Return _transmision
        End Get
        Set(ByVal value As String)
            _transmision = value
        End Set
    End Property
    Private _transmision As String

    Public Property TipoInventario() As String
        Get
            Return _tipoinventario
        End Get
        Set(ByVal value As String)
            _tipoinventario = value
        End Set
    End Property
    Private _tipoinventario As String


    Public Property ColorInterior() As String
        Get
            Return _colorinterior
        End Get
        Set(ByVal value As String)
            _colorinterior = value
        End Set
    End Property
    Private _colorinterior As String

    Public Property Observacion() As String
        Get
            Return _observacion
        End Get
        Set(ByVal value As String)
            _observacion = value
        End Set
    End Property
    Private _observacion As String

    Public Property PrecioNeto() As Double
        Get
            Return _precioneto
        End Get
        Set(ByVal value As Double)
            _precioneto = value
        End Set
    End Property
    Private _precioneto As Double

    Public Property Precio() As Double
        Get
            Return _precio
        End Get
        Set(ByVal value As Double)
            _precio = value
        End Set
    End Property
    Private _precio As Double


    Public Property Bono() As Double
        Get
            Return _bono
        End Get
        Set(ByVal value As Double)
            _bono = value
        End Set
    End Property
    Private _bono As Double

    Public Property Descuento() As Double
        Get
            Return _descuento
        End Get
        Set(ByVal value As Double)
            _descuento = value
        End Set
    End Property
    Private _descuento As Double


    Public Property MDesc() As String
        Get
            Return _mdesc
        End Get
        Set(ByVal value As String)
            _mdesc = value
        End Set
    End Property
    Private _mdesc As String

    Public Property Impuesto() As String
        Get
            Return _impuesto
        End Get
        Set(ByVal value As String)
            _impuesto = value
        End Set
    End Property
    Private _impuesto As String


    Public Property PrecioTotal() As Double
        Get
            Return _preciototal
        End Get
        Set(ByVal value As Double)
            _preciototal = value
        End Set
    End Property
    Private _preciototal As Double

    Public Property Pagos() As Double
        Get
            Return _pagos
        End Get
        Set(ByVal value As Double)
            _pagos = value
        End Set
    End Property
    Private _pagos As Double


    Public Property MAcc() As Double
        Get
            Return _macc
        End Get
        Set(ByVal value As Double)
            _macc = value
        End Set
    End Property
    Private _macc As Double

    Public Property GL() As Integer
        Get
            Return _gl
        End Get
        Set(ByVal value As Integer)
            _gl = value
        End Set
    End Property
    Private _gl As Integer

    Public Property OG() As Integer
        Get
            Return _og
        End Get
        Set(ByVal value As Integer)
            _og = value
        End Set
    End Property
    Private _og As Integer

    Public Property KmSale() As Integer
        Get
            Return _kmsale
        End Get
        Set(ByVal value As Integer)
            _kmsale = value
        End Set
    End Property
    Private _kmsale As Integer

    Public Property Fila() As Integer
        Get
            Return _fila
        End Get
        Set(ByVal value As Integer)
            _fila = value
        End Set
    End Property
    Private _fila As Integer

    Public Property Aplicado() As Boolean
        Get
            Return _Aplicado
        End Get
        Set(ByVal value As Boolean)
            _Aplicado = value
        End Set
    End Property
    Private _Aplicado As Boolean

End Class


Public Class ListaEncabezadoFactura

    Public Property Cliente As String
        Get
            Return _cliente
        End Get
        Set(ByVal value As String)
            _cliente = value
        End Set
    End Property
    Private _cliente As String

    Public Property Comentarios As String
        Get
            Return _comentarios
        End Get
        Set(ByVal value As String)
            _comentarios = value
        End Set
    End Property
    Private _comentarios As String


    Public Property MonedaConfigurada() As String
        Get
            Return _monedaconfigurada
        End Get
        Set(ByVal value As String)
            _monedaconfigurada = value
        End Set
    End Property
    Private _monedaconfigurada As String

    Public Property DocCurrency() As String
        Get
            Return _doccurrency
        End Get
        Set(ByVal value As String)
            _doccurrency = value
        End Set
    End Property
    Private _doccurrency As String


    Public Property PeriodoPago() As Integer
        Get
            Return _periodopago
        End Get
        Set(ByVal value As Integer)
            _periodopago = value
        End Set
    End Property
    Private _periodopago As Integer

    Public Property Vendedor() As String
        Get
            Return _vendedor
        End Get
        Set(ByVal value As String)
            _vendedor = value
        End Set
    End Property
    Private _vendedor As String

    Public Property PorcentajeDescuento() As String
        Get
            Return _porcentajedescuento
        End Get
        Set(ByVal value As String)
            _porcentajedescuento = value
        End Set
    End Property
    Private _porcentajedescuento As String

    Public Property Indicador() As String
        Get
            Return _indicador
        End Get
        Set(ByVal value As String)
            _indicador = value
        End Set
    End Property
    Private _indicador As String

    Public Property NumeroContratoVenta() As String
        Get
            Return _numerocontratoventa
        End Get
        Set(ByVal value As String)
            _numerocontratoventa = value
        End Set
    End Property
    Private _numerocontratoventa As String

    Public Property TipoCambio() As Decimal
        Get
            Return _tipocambio
        End Get
        Set(ByVal value As Decimal)
            _tipocambio = value
        End Set
    End Property
    Private _tipocambio As Decimal

    Public Property ComFin() As String
        Get
            Return _comfin
        End Get
        Set(ByVal value As String)
            _comfin = value
        End Set
    End Property
    Private _comfin As String

    Public Property ConEjeBan() As String
        Get
            Return _conejeban
        End Get
        Set(ByVal value As String)
            _conejeban = value
        End Set
    End Property
    Private _conejeban As String

    Public Property NrOC() As String
        Get
            Return _nroc
        End Get
        Set(ByVal value As String)
            _nroc = value
        End Set
    End Property
    Private _nroc As String

    Public Property NrOL() As String
        Get
            Return _nrol
        End Get
        Set(ByVal value As String)
            _nrol = value
        End Set
    End Property
    Private _nrol As String

    Public Property PlacaProvisional As String
        Get
            Return _placaprovisional
        End Get
        Set(ByVal value As String)
            _placaprovisional = value
        End Set
    End Property
    Private _placaprovisional As String


End Class



