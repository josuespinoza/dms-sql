Imports SAPbobsCOM
Imports SAPbouiCOM
Imports System.Globalization
Imports SCG.SBOFramework

'Clase para controlar funcionalidad de financiamiento en pantalla de Pagos Recibidos de Gestión de Bancos de SBO
'Pagos en borrador con número de préstamo y número de pago del modulo de financiamiento

Public Class PagoRecibido

    Private SBO_Application As SAPbouiCOM.Application
    Private SBO_Company As SAPbobsCOM.Company

    Private _strConexion As String

    Public strPagoRecibido As String = String.Empty
    Private strNumeroPrestamo As String
    Private strNumeroPago As String
    Private strCliente As String
    Private strMoneda As String
    Private strFechaPago As String
    Private strUsaFinanc As String
    Private oDataTablePago As SAPbouiCOM.DataTable

    Private strCuentaValidaInt As String
    Private strCuentaValidaMora As String
    Private strNumeroPagoC As Integer
    Private strTipoCuo As String

    Public Sub New(ByVal p_SBO_Application As SAPbouiCOM.Application, ByVal m_oCompany As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Application
        SBO_Company = m_oCompany

    End Sub

    Public Property StrConexion() As String
        Get
            Return _strConexion
        End Get
        Set(ByVal value As String)
            _strConexion = value
        End Set
    End Property

    Public Sub ManejadorEventoMenu(ByVal pval As SAPbouiCOM.MenuEvent, ByVal formUID As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Select Case pval.MenuUID
            Case "1284"
                If Not String.IsNullOrEmpty(formUID.DataSources.DBDataSources.Item("ORCT").GetValue("U_SCGD_Prestamo", 0).ToString.Trim()) Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.NoEliminarPagosRecibidos, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
                    BubbleEvent = False
                    Exit Sub
                End If
        End Select
    End Sub

    'Manejo de evento del botón crear de pagos recibidos en borrador si usa financiamiento y si tiene un número de préstamo y pago del modulo de financiamiento
    'Validaciones: El borador no debe estar cancelado, la moneda del pago debe ser la misma que la del préstamo relacionado,
    'el monto total del pago recibido debe ser igual al monto del pago del préstamo, cuentas para generar asiento de intereses normales y moratorios
    'Moneda para generar los movimientos contables de asiento de intereses normales y moratorios
    'Genera asiento de intereses del pago y actualiza el plan de pagos real, poniendo el pago recibido, asiento de intereses y quitando el borrador

    Public Sub ManejadorEventoItemPressed(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oForm As SAPbouiCOM.Form

        Dim n As NumberFormatInfo

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildPago As SAPbobsCOM.GeneralData
        Dim oChildrenPago As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Dim strRef1 As String
        Dim strRef2 As String
        Dim strAsientoIntereses As String = ""
        Dim blnAsientoIntGenerado As Boolean = False
        Dim strComentario As String
        Dim strMonedaPrest As String
        Dim strMonedaLocal As String
        Dim strMonedaSistema As String
        Dim strCancelado As String
        Dim strConsulta As String
        Dim decCuota As Decimal
        Dim strDocTotal As String = ""
        Dim decDocTotal As Decimal
        Dim strCuentaCredInt As String = ""
        Dim strCuentaCredMora As String = ""
        Dim strMonedaCredInt As String
        Dim strMonedaCredMora As String
        Dim decInteres As Decimal
        Dim decIntPend As Decimal
        Dim decMora As Decimal
        Dim decMoraPend As Decimal
        Dim decAboTotalInt As Decimal = 0
        Dim decAboTotalMora As Decimal = 0
        Dim strGeneraAsiento As String
        Dim intNumeroPago As Integer
        Dim dtFechaPago As Date
        Dim strGeneraFactura As String = ""
        Dim dbRecargoCobranza As Double = 0


        Try

            n = DIHelper.GetNumberFormatInfo(SBO_Company)

            oForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            If pVal.BeforeAction Then
                If pVal.ItemUID = "1" AndAlso oForm.Mode = BoFormMode.fm_ADD_MODE Then

                    strUsaFinanc = General.EjecutarConsulta("Select U_Usa_Fin from [@SCGD_ADMIN] where Code = 'DMS'", StrConexion)

                    If strUsaFinanc = "Y" Then

                        strNumeroPrestamo = oForm.DataSources.DBDataSources.Item("ORCT").GetValue("U_SCGD_Prestamo", 0).Trim()

                        strNumeroPago = oForm.DataSources.DBDataSources.Item("ORCT").GetValue("U_SCGD_NumPago", 0).Trim()
                        If IsNumeric(oForm.DataSources.DBDataSources.Item("ORCT").GetValue("U_SCGD_NumPagoC", 0)) Then strNumeroPagoC = oForm.DataSources.DBDataSources.Item("ORCT").GetValue("U_SCGD_NumPagoC", 0)

                        If Not String.IsNullOrEmpty(strNumeroPrestamo) AndAlso Not String.IsNullOrEmpty(strNumeroPago) Then
                            strTipoCuo = General.EjecutarConsulta(String.Format(" Select U_Tipo_Cuo From [@SCGD_PRESTAMO] Where DocEntry = {0} ", strNumeroPrestamo), StrConexion)
                            strCancelado =
                                General.EjecutarConsulta(
                                    String.Format("Select Canceled from [OPDF] where U_SCGD_Prestamo = '{0}' And U_SCGD_NumPago = '{1}'",
                                                  strNumeroPrestamo, strNumeroPago),
                                    StrConexion)

                            If strCancelado = "N" OrElse strTipoCuo.Trim = "1" Then

                                strMoneda = oForm.DataSources.DBDataSources.Item("ORCT").GetValue("DocCurr", 0).Trim()

                                strMonedaPrest = General.EjecutarConsulta("Select U_Moneda from [@SCGD_PRESTAMO] where DocEntry = '" & strNumeroPrestamo & "'", StrConexion)

                                If Not strMoneda = strMonedaPrest Then

                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorMonedaPago, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    Exit Sub

                                Else

                                    If Not General.ValidaExisteDataTable(oForm, "Pagos") Then oDataTablePago = oForm.DataSources.DataTables.Add("Pagos")


                                    oDataTablePago = oForm.DataSources.DataTables.Item("Pagos")

                                    oDataTablePago.Clear()

                                    strConsulta = "Select 0.0 as U_SCGD_MIn, 0.0 as U_SCGD_MInMo, 0.0 as U_SCGD_MRC, U_Tipo_Cuo, U_Cuota, P1.U_Interes, U_Int_Pend, P2.U_Int_Mora, U_Mor_Pend,isnull(U_ReCo,0) AS U_ReCo,U_Pagado From [@SCGD_PLAN_REAL] P1 " & _
                                                    " INNER JOIN [@SCGD_PRESTAMO] P2 ON P1.DocEntry = P2.DocEntry  Where P1.DocEntry = '" & strNumeroPrestamo & "' And U_Numero = '" & strNumeroPago & "'"

                                    oDataTablePago.ExecuteQuery(strConsulta)

                                    decCuota = oDataTablePago.GetValue("U_Cuota", 0)

                                    If oDataTablePago.GetValue("U_Tipo_Cuo", 0).ToString.Trim = "1" Then
                                        Dim dbSCGD_MIn As Double = General.ConvierteDecimal(oForm.DataSources.DBDataSources.Item("ORCT").GetValue("U_SCGD_MIn", 0), n)
                                        Dim dbSCGD_MInMo As Double = General.ConvierteDecimal(oForm.DataSources.DBDataSources.Item("ORCT").GetValue("U_SCGD_MInMo", 0), n)
                                        Dim dbSCGD_MRC As Double = General.ConvierteDecimal(oForm.DataSources.DBDataSources.Item("ORCT").GetValue("U_SCGD_MRC", 0), n)

                                        oDataTablePago.SetValue("U_SCGD_MIn", 0, dbSCGD_MIn)
                                        oDataTablePago.SetValue("U_SCGD_MInMo", 0, dbSCGD_MInMo)
                                        oDataTablePago.SetValue("U_SCGD_MRC", 0, dbSCGD_MRC)

                                    End If

                                    strMonedaLocal = General.RetornarMonedaLocal(SBO_Company)
                                    strMonedaSistema = General.RetornarMonedaSistema(SBO_Company)

                                    If strMoneda = strMonedaLocal Then

                                        strDocTotal = oForm.DataSources.DBDataSources.Item("ORCT").GetValue("DocTotal", 0)
                                        strDocTotal = strDocTotal.Trim()

                                    ElseIf strMoneda = strMonedaSistema Then

                                        strDocTotal = oForm.DataSources.DBDataSources.Item("ORCT").GetValue("DocTotalFC", 0)
                                        strDocTotal = strDocTotal.Trim()

                                    End If

                                    If Not String.IsNullOrEmpty(strDocTotal) Then
                                        decDocTotal = Decimal.Parse(strDocTotal, n)
                                    End If

                                    If Math.Abs(decDocTotal - decCuota) > Math.Pow(10, -1 * 2) AndAlso oDataTablePago.GetValue("U_Pagado", 0).ToString.Trim = "N" Then

                                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorMontoPago, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                                        BubbleEvent = False
                                        Exit Sub

                                    End If

                                    If strMoneda = strMonedaLocal Then

                                        strCuentaCredInt = General.EjecutarConsulta("Select U_Int_Loc From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)
                                        strCuentaCredMora = General.EjecutarConsulta("Select U_Mor_Loc From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)

                                    ElseIf strMoneda = strMonedaSistema Then

                                        strCuentaCredInt = General.EjecutarConsulta("Select U_Int_Sis From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)
                                        strCuentaCredMora = General.EjecutarConsulta("Select U_Mor_Sis From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)

                                    End If

                                    strCuentaValidaInt = General.EjecutarConsulta("Select AcctCode from dbo.[OACT] where FormatCode = '" & strCuentaCredInt & "' And Postable = 'Y'", StrConexion)
                                    strMonedaCredInt = General.EjecutarConsulta("Select ActCurr from dbo.[OACT] where AcctCode = '" & strCuentaValidaInt & "'", StrConexion)

                                    strCuentaValidaMora = General.EjecutarConsulta("Select AcctCode from dbo.[OACT] where FormatCode = '" & strCuentaCredMora & "' And Postable = 'Y'", StrConexion)
                                    strMonedaCredMora = General.EjecutarConsulta("Select ActCurr from dbo.[OACT] where AcctCode = '" & strCuentaValidaMora & "'", StrConexion)

                                    If String.IsNullOrEmpty(strCuentaValidaInt) OrElse String.IsNullOrEmpty(strCuentaValidaMora) Then

                                        BubbleEvent = False
                                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorConfiguracion, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                                        Exit Sub

                                    Else

                                        If Not (strMonedaCredInt = "##" OrElse strMonedaCredMora = "##") AndAlso Not (strMonedaCredInt = strMoneda OrElse strMonedaCredMora = strMoneda) Then

                                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorMoneda, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                                            BubbleEvent = False
                                            Exit Sub

                                        End If

                                    End If

                                    strCliente = oForm.DataSources.DBDataSources.Item("ORCT").GetValue("CardCode", 0)
                                    strCliente = strCliente.Trim()

                                    strFechaPago = oForm.DataSources.DBDataSources.Item("ORCT").GetValue("DocDate", 0)
                                    strFechaPago = strFechaPago.Trim()

                                End If

                            ElseIf strCancelado = "Y" Then

                                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorBorradorInvalido & strNumeroPrestamo & My.Resources.Resource.DelPago & strNumeroPago, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub

                            End If

                        End If

                    End If
                End If
            ElseIf pVal.ActionSuccess Then
                If pVal.ItemUID = "1" AndAlso oForm.Mode = BoFormMode.fm_ADD_MODE Then

                    If strUsaFinanc = "Y" AndAlso Not String.IsNullOrEmpty(strNumeroPrestamo) AndAlso Not String.IsNullOrEmpty(strNumeroPago) Then

                        If strTipoCuo <> "1" Then
                            strComentario = My.Resources.Resource.DocumentoGenerado & strNumeroPago & My.Resources.Resource.DelPrestamo & strNumeroPrestamo
                        Else
                            strComentario = My.Resources.Resource.DocumentoGenerado & strNumeroPago & My.Resources.Resource.NumPagoPrestamo & strNumeroPagoC & My.Resources.Resource.DelPrestamo & strNumeroPrestamo
                        End If


                        strRef1 = My.Resources.Resource.Prestamo & strNumeroPrestamo
                        strRef2 = My.Resources.Resource.Pago & strNumeroPago

                        decInteres = oDataTablePago.GetValue("U_Interes", 0)
                        decIntPend = oDataTablePago.GetValue("U_Int_Pend", 0)
                        decMora = oDataTablePago.GetValue("U_Int_Mora", 0)
                        decMoraPend = oDataTablePago.GetValue("U_Mor_Pend", 0)

                        If oDataTablePago.GetValue("U_Tipo_Cuo", 0).ToString.Trim <> "1" Then
                            dbRecargoCobranza = oDataTablePago.GetValue("U_ReCo", 0)
                            decAboTotalInt = decInteres + decIntPend
                            decAboTotalMora = decMora + decMoraPend
                        Else
                            decAboTotalInt = oDataTablePago.GetValue("U_SCGD_MIn", 0)
                            decAboTotalMora = oDataTablePago.GetValue("U_SCGD_MInMo", 0)
                            dbRecargoCobranza = oDataTablePago.GetValue("U_SCGD_MRC", 0)
                        End If


                        If decAboTotalInt > 0 OrElse decAboTotalMora > 0 Then

                            strGeneraAsiento = General.EjecutarConsulta("Select U_Gen_As From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)

                            If Not String.IsNullOrEmpty(strFechaPago) Then
                                dtFechaPago = Date.ParseExact(strFechaPago, "yyyyMMdd", Nothing)
                                dtFechaPago = New Date(dtFechaPago.Year, dtFechaPago.Month, dtFechaPago.Day, 0, 0, 0)
                            End If

                            strGeneraFactura = General.EjecutarConsulta(" Select U_GenFD from [@SCGD_CONF_FINANC] ", StrConexion)
                            strGeneraFactura = strGeneraFactura.Trim

                            If strGeneraFactura = "Y" Then
                                Call GenerarFacturaIntereses(dtFechaPago, strComentario, "", strCliente, strMoneda, strRef1, strRef2, strCuentaValidaInt, decAboTotalInt, strCuentaValidaMora, decAboTotalMora, strGeneraAsiento, blnAsientoIntGenerado, strAsientoIntereses, dbRecargoCobranza, BubbleEvent)
                            Else
                                Call GenerarAsientoIntereses(dtFechaPago, strComentario, "", strCliente, strMoneda, strRef1, strRef2, strCuentaValidaInt, decAboTotalInt, strCuentaValidaMora, decAboTotalMora, strGeneraAsiento, blnAsientoIntGenerado, strAsientoIntereses, dbRecargoCobranza)
                            End If
                        End If

                        intNumeroPago = Integer.Parse(strNumeroPago)

                        oCompanyService = SBO_Company.GetCompanyService()
                        oGeneralService = oCompanyService.GetGeneralService("SCGD_Prestamo")
                        oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                        oGeneralParams.SetProperty("DocEntry", strNumeroPrestamo)
                        oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                        If oDataTablePago.GetValue("U_Tipo_Cuo", 0).ToString.Trim <> "1" Then

                            oChildrenPago = oGeneralData.Child("SCGD_PLAN_REAL")
                            oChildPago = oChildrenPago.Item(intNumeroPago - 1)
                            oChildPago.SetProperty("U_Cred_Cap", strPagoRecibido)
                            If strGeneraFactura <> "Y" And blnAsientoIntGenerado Then
                                oChildPago.SetProperty("U_Doc_Int", strAsientoIntereses)
                            ElseIf strGeneraFactura = "Y" And blnAsientoIntGenerado Then
                                oChildPago.SetProperty("U_DocFac", strAsientoIntereses)
                            End If
                            oChildPago.SetProperty("U_BorrPag", "")

                        Else

                            oChildrenPago = oGeneralData.Child("SCGD_PAGO_PRESTAMO")
                            oChildPago = oChildrenPago.Item(strNumeroPagoC - 1)
                            oChildPago.SetProperty("U_Pago", CInt(strPagoRecibido))
                            If strGeneraFactura <> "Y" And blnAsientoIntGenerado = True Then
                                oChildPago.SetProperty("U_NumAsie", CInt(strAsientoIntereses))
                            ElseIf strGeneraFactura = "Y" And blnAsientoIntGenerado = True Then
                                oChildPago.SetProperty("U_DocFac", CInt(strAsientoIntereses))
                            End If
                            oChildPago.SetProperty("U_BorrPag", 0)

                        End If

                        oGeneralService.Update(oGeneralData)

                    End If

                End If
            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Generación de asiento de intereses, se pone en Débito el cliente que realiza el pago y en Crédito las cuentas configuradas para interes normal y moratorios

    Private Sub GenerarAsientoIntereses(ByVal dtFechaPago As Date, ByVal strComentario As String, ByVal strCuentaDebitoCuota As String, ByVal strCliente As String, ByVal strMoneda As String, _
                                        ByVal strRef1 As String, ByVal strRef2 As String, ByVal strCuentaIntNormal As String, ByVal decIntNormal As Decimal, ByVal strCuentaIntMora As String, _
                                        ByVal decIntMora As Decimal, ByVal strGeneraAsiento As String, ByRef blnAsientoIntGenerado As Boolean, ByRef strAsientoInteres As String, _
                                        ByVal dbRecargoCobranza As Double)

        Dim oJournalEntry As SAPbobsCOM.JournalEntries

        Dim intError As Integer
        Dim strMensajeError As String = ""

        Dim decTotalIntereses As Decimal
        Dim strMonedaLocal As String

        Try

            strMonedaLocal = General.RetornarMonedaLocal(SBO_Company)

            oJournalEntry = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            oJournalEntry.ReferenceDate = dtFechaPago
            oJournalEntry.Memo = strComentario

            If strGeneraAsiento = "Y" Then
                oJournalEntry.Lines.AccountCode = strCuentaDebitoCuota
            Else
                oJournalEntry.Lines.ShortName = strCliente
            End If
            decTotalIntereses = decIntNormal + decIntMora + dbRecargoCobranza
            If strMoneda = strMonedaLocal Then
                oJournalEntry.Lines.Debit = decTotalIntereses
            Else
                oJournalEntry.Lines.FCDebit = decTotalIntereses
                oJournalEntry.Lines.FCCurrency = strMoneda
            End If
            oJournalEntry.Lines.Reference1 = strRef1
            oJournalEntry.Lines.Reference2 = strRef2
            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
            oJournalEntry.Lines.Add()

            If decIntNormal > 0 Then
                oJournalEntry.Lines.AccountCode = strCuentaIntNormal
                If strMoneda = strMonedaLocal Then
                    oJournalEntry.Lines.Credit = decIntNormal
                Else
                    oJournalEntry.Lines.FCCredit = decIntNormal
                    oJournalEntry.Lines.FCCurrency = strMoneda
                End If
                oJournalEntry.Lines.Reference1 = strRef1
                oJournalEntry.Lines.Reference2 = strRef2
                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.Add()
            End If

            If decIntMora > 0 Then
                oJournalEntry.Lines.AccountCode = strCuentaIntMora
                If strMoneda = strMonedaLocal Then
                    oJournalEntry.Lines.Credit = decIntMora
                Else
                    oJournalEntry.Lines.FCCredit = decIntMora
                    oJournalEntry.Lines.FCCurrency = strMoneda
                End If
                oJournalEntry.Lines.Reference1 = strRef1
                oJournalEntry.Lines.Reference2 = strRef2
                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.Add()
            End If

            If dbRecargoCobranza > 0 Then
                oJournalEntry.Lines.AccountCode = strCuentaIntMora
                If strMoneda = strMonedaLocal Then
                    oJournalEntry.Lines.Credit = dbRecargoCobranza
                Else
                    oJournalEntry.Lines.FCCredit = dbRecargoCobranza
                    oJournalEntry.Lines.FCCurrency = strMoneda
                End If
                oJournalEntry.Lines.Reference1 = strRef1
                oJournalEntry.Lines.Reference2 = strRef2
                oJournalEntry.Lines.LineMemo = My.Resources.Resource.RecargoCobranza
                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.Add()
            End If

            If oJournalEntry.Add <> 0 Then
                SBO_Company.GetLastError(intError, strMensajeError)
                If SBO_Company.InTransaction() Then
                    SBO_Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If
            Else
                SBO_Company.GetNewObjectCode(strAsientoInteres)
                blnAsientoIntGenerado = True
            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Private Sub GenerarFacturaIntereses(ByVal dtFechaPago As Date, ByVal strComentario As String, ByVal strCuentaDebitoCuota As String, ByVal strCliente As String, ByVal strMoneda As String, _
                                        ByVal strRef1 As String, ByVal strRef2 As String, ByVal strCuentaIntNormal As String, ByVal decIntNormal As Decimal, ByVal strCuentaIntMora As String, _
                                        ByVal decIntMora As Decimal, ByVal strGeneraAsiento As String, ByRef blnAsientoIntGenerado As Boolean, ByRef strAsientoInteres As String, _
                                        ByVal dbRecargoCobranza As Double, ByRef BubbleEvent As Boolean)

        Dim oFactura As SAPbobsCOM.Documents

        Dim intError As Integer
        Dim strMensajeError As String = ""

        Dim decTotalIntereses As Decimal
        Dim strMonedaLocal As String

        Dim strNumDoc As String = General.EjecutarConsulta(" Select U_NumDoc from [@SCGD_CONF_FINANC] ", StrConexion)
        strNumDoc = strNumDoc.Trim()


        Dim strCodImpuestos As String = General.EjecutarConsulta(" Select U_CodImp from [@SCGD_CONF_FINANC] ", StrConexion)
        strCodImpuestos = strCodImpuestos.Trim()

        Try

            strMonedaLocal = General.RetornarMonedaLocal(SBO_Company)

            oFactura = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)

            oFactura.DocDate = dtFechaPago
            oFactura.Comments = strComentario

            oFactura.CardCode = strCliente
            oFactura.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service
            oFactura.DocCurrency = strMoneda
            oFactura.Series = strNumDoc

            decTotalIntereses = decIntNormal + decIntMora

            If decIntNormal > 0 Then
                oFactura.Lines.AccountCode = strCuentaIntNormal
                If strMoneda = strMonedaLocal Then
                    oFactura.Lines.LineTotal = decIntNormal
                Else
                    oFactura.Lines.RowTotalFC = decIntNormal
                End If
                oFactura.Lines.Currency = strMoneda
                oFactura.Lines.TaxCode = strCodImpuestos
                oFactura.Lines.VatGroup = strCodImpuestos
                oFactura.Lines.ItemDescription = My.Resources.Resource.InteresNormal
                oFactura.Lines.Add()
            End If

            If decIntMora > 0 Then
                oFactura.Lines.AccountCode = strCuentaIntMora
                If strMoneda = strMonedaLocal Then
                    oFactura.Lines.LineTotal = decIntMora
                Else
                    oFactura.Lines.RowTotalFC = decIntMora
                End If
                oFactura.Lines.Currency = strMoneda
                oFactura.Lines.TaxCode = strCodImpuestos
                oFactura.Lines.VatGroup = strCodImpuestos
                oFactura.Lines.ItemDescription = My.Resources.Resource.InteresMora
                oFactura.Lines.Add()
            End If

            If dbRecargoCobranza > 0 Then
                oFactura.Lines.AccountCode = strCuentaIntMora
                If strMoneda = strMonedaLocal Then
                    oFactura.Lines.LineTotal = dbRecargoCobranza
                Else
                    oFactura.Lines.RowTotalFC = dbRecargoCobranza
                End If
                oFactura.Lines.Currency = strMoneda
                oFactura.Lines.TaxCode = strCodImpuestos
                oFactura.Lines.VatGroup = strCodImpuestos
                oFactura.Lines.ItemDescription = My.Resources.Resource.RecargoCobranza
                oFactura.Lines.Add()
            End If

            If oFactura.Add <> 0 Then
                SBO_Company.GetLastError(intError, strMensajeError)
                SBO_Application.SetStatusBarMessage(strMensajeError, BoMessageTime.bmt_Long, True)
                If SBO_Company.InTransaction() Then
                    SBO_Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If
                BubbleEvent = False
            Else
                SBO_Company.GetNewObjectCode(strAsientoInteres)
                blnAsientoIntGenerado = True
            End If

        Catch ex As Exception
            BubbleEvent = False
            Throw ex

        End Try


    End Sub

End Class
