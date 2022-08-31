Imports DMSOneFramework
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.SCGDataAccess

Partial Public Class CrearDocumentosGastosCostos

#Region "Declaraciones"
    Private m_dtItemSeleccionados As SAPbouiCOM.DataTable
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon

    Private m_strUDFFacturaProv As String = "U_SerFacPro"
    Public Shared m_strMoneda As String


#End Region


#Region "Metodos"
    Public Sub IncluirGastosSeleccionados(ByVal dtSeleccionados As SAPbouiCOM.DataTable,
                                          ByVal p_strCodUnid As String,
                                          ByVal p_strNoOrder As String,
                                          ByVal p_strTipoOrder As String,
                                          ByVal p_strDocEntry As String,
                                        ByVal Validacion As Boolean,
                                        ByRef BubbleEvent As Boolean)

        Dim oMatrix As Matrix
        Dim Posicion As Integer = 0
        Dim dcCostoF As Decimal
        Dim dcPrecioF As Decimal

        'Dim strConsultaAprobaciones As String =
        '    " select U_ItmAprob from [@SCGD_CONF_APROBAC] as cap inner join [@SCGD_CONF_SUCURSAL] as csu on csu.DocEntry = cap.DocEntry " & _
        '    " where csu.U_Sucurs in ( select U_SCGD_idSucursal from [OQUT] where U_SCGD_Numero_OT = '{0}') " & _
        '    " and cap.U_TipoOT in ( select U_SCGD_Tipo_OT from [OQUT] where U_SCGD_Numero_OT = '{0}')"
        Dim strNoOT As String = String.Empty

        Try
            ' If Validacion Then ValidaPrecios(dtSeleccionados, BubbleEvent)

            '   If Validacion Then Exit Try

            oForm = ApplicationSBO.Forms.Item("SCGD_GenDoc")
            dtGastos = oForm.DataSources.DataTables.Item(strDTGastos)
            oMatrix = DirectCast(oForm.Items.Item("mtxGas").Specific, Matrix)
            Posicion = dtGastos.Rows.Count

            oForm.Freeze(True)

            txtNoOrden.AsignaValorUserDataSource(p_strNoOrder)
            txtNoUnid.AsignaValorUserDataSource(p_strCodUnid)
            txtTipoOrden.AsignaValorUserDataSource(p_strTipoOrder)
            txtDocE.AsignaValorUserDataSource(p_strDocEntry)

            oForm.Freeze(False)


            For i As Integer = 0 To dtSeleccionados.Rows.Count - 1
                If dtSeleccionados.GetValue("sel", i) = "Y" Then
                    dtGastos.Rows.Add(1)

                    If Not String.IsNullOrEmpty(dtSeleccionados.GetValue("cos", i)) Then
                        dcCostoF = Decimal.Parse(dtSeleccionados.GetValue("cos", i))
                    Else
                        dcCostoF = 0
                    End If

                    If Not String.IsNullOrEmpty(dtSeleccionados.GetValue("pre", i)) Then
                        dcPrecioF = Decimal.Parse(dtSeleccionados.GetValue("pre", i))
                    Else
                        dcPrecioF = 0
                    End If

                    dtGastos.SetValue("cod", Posicion, dtSeleccionados.GetValue("cod", i))
                    dtGastos.SetValue("des", Posicion, dtSeleccionados.GetValue("des", i))
                    dtGastos.SetValue("mon", Posicion, dtSeleccionados.GetValue("mon", i))
                    dtGastos.SetValue("can", Posicion, dtSeleccionados.GetValue("can", i))
                    dtGastos.SetValue("cos", Posicion, dcCostoF.ToString(n))
                    dtGastos.SetValue("pre", Posicion, dcPrecioF.ToString(n))
                    dtGastos.SetValue("imp", Posicion, dtSeleccionados.GetValue("imp", i))
                    dtGastos.SetValue("lnum", Posicion, dtSeleccionados.GetValue("lnum", i))


                    Posicion += 1
                End If
            Next

            ' oForm.Items.Item("btnAct").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

            oMatrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub


    Public Sub ControlesCrearFactura()
        Try
            oForm.Freeze(True)

            oForm.Items.Item(txtProveedor.UniqueId).Click()

            oForm.Items.Item(txtProveedor.UniqueId).Enabled = True

            chxAsiento.AsignaValorUserDataSource("N")
            chxFactura.AsignaValorUserDataSource("Y")

            oForm.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub ControlesCrearAsiento()
        Try
            oForm.Freeze(True)
            oForm.Items.Item(txtObs.UniqueId).Click()

            oForm.Items.Item(txtProveedor.UniqueId).Enabled = False
            oForm.Items.Item(txtProveedorNam.UniqueId).Enabled = False

            chxAsiento.AsignaValorUserDataSource("Y")
            chxFactura.AsignaValorUserDataSource("N")
            oForm.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub



    Public Function CrearFactura()
        Dim l_decTotalFactura As Decimal
        Dim l_intError As Integer
        Dim l_strNumSerie As Integer
        Dim l_strSerieFacturaProv As String
        Dim l_strCodeSucursal As String
        Dim l_strMoneda As String
        Dim l_decTipoC As Decimal
        Dim l_strNuevaFac As Integer
        Dim l_intDocEntry As Integer

        Dim l_oFacturaProv As SAPbobsCOM.Documents
        Dim l_oFacturaProvLines As SAPbobsCOM.Document_Lines

        Dim l_strSQL As String = " SELECT DocEntry , DocNum, DocRate, DocCur, U_SCGD_idSucursal, U_SCGD_Numero_OT " +
                                " FROM OQUT oq WHERE DocEntry = '{0}'"

        l_decTotalFactura = TotalLIneasFactura()

        Try

            l_strSQL = String.Format(l_strSQL, txtDocE.ObtieneValorUserDataSource())

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Rows.Clear()
            dtLocal.ExecuteQuery(l_strSQL)

            If dtLocal.Rows.Count <> 0 Then
                l_decTipoC = dtLocal.GetValue("DocRate", 0)
                l_strCodeSucursal = dtLocal.GetValue("U_SCGD_idSucursal", 0)
                l_strMoneda = dtLocal.GetValue("DocCur", 0)
                l_intDocEntry = dtLocal.GetValue("DocEntry", 0)
            End If

            l_strSerieFacturaProv = DevuelveValorItem(l_strCodeSucursal, m_strUDFFacturaProv)

            If l_decTotalFactura <> 0 Then

                l_oFacturaProv = CType(_companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices), SAPbobsCOM.Documents)
                l_oFacturaProvLines = l_oFacturaProv.Lines

                l_oFacturaProv.CardCode = txtProveedor.ObtieneValorUserDataSource.Trim
                l_oFacturaProv.CardName = txtProveedorNam.ObtieneValorUserDataSource.Trim

                l_oFacturaProv.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items


                l_oFacturaProv.DocRate = l_decTipoC
                l_oFacturaProv.DocDate = Date.Parse(Date.Now)
                l_oFacturaProv.Series = l_strSerieFacturaProv
                l_oFacturaProv.Comments = txtObs.ObtieneValorUserDataSource()

                l_oFacturaProv.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = txtNoOrden.ObtieneValorUserDataSource.ToString()


                CrearLineasFactura(l_oFacturaProvLines)

                ' _companySbo.StartTransaction()

                l_intError = l_oFacturaProv.Add

                If l_intError = 0 Then
                    _companySbo.GetNewObjectCode(l_strNuevaFac)
                    ActualizaCotizacion(l_intDocEntry, l_strNuevaFac, "", TipoDocumento.Factura)

                    'If _companySbo.InTransaction Then
                    '    _companySbo.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                    'End If

                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCreaDocGastoDocCreado, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                ElseIf l_intError <> 0 Then

                    '_companySbo.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCreaDocGastoErrorCrear, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)

                End If

            End If

        Catch ex As Exception
            If _companySbo.InTransaction Then
                _companySbo.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If


            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Sub CrearLineasFactura(ByRef l_oFacLines As SAPbobsCOM.Document_Lines)
        Try
            Dim l_blnAgrega As Boolean = False

            For i As Integer = 0 To dtGastos.Rows.Count - 1

                If l_blnAgrega Then
                    l_oFacLines.Add()
                Else
                    l_blnAgrega = True
                End If

                If dtGastos.GetValue("sel", i) = "Y" Then
                    l_oFacLines.ItemCode = dtGastos.GetValue("cod", i)
                    l_oFacLines.Currency = dtGastos.GetValue("mon", i)
                    l_oFacLines.Quantity = dtGastos.GetValue("can", i)
                    l_oFacLines.TaxCode = dtGastos.GetValue("imp", i)
                    l_oFacLines.VatGroup = dtGastos.GetValue("imp", i)
                    l_oFacLines.UnitPrice = dtGastos.GetValue("pre", i)

                End If
            Next

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Function ActualizaCotizacion(ByVal p_intDocEnt As Integer, ByVal p_strFacPro As String, ByVal p_strAsiento As String, ByVal p_strTipoDoc As String) As String
        Try
            Dim l_oCotizacion As SAPbobsCOM.Documents
            Dim l_oLineasCot As SAPbobsCOM.Document_Lines

            l_oCotizacion = CType(_companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations), SAPbobsCOM.Documents)

            l_oCotizacion.GetByKey(p_intDocEnt)
            l_oLineasCot = l_oCotizacion.Lines


            For i As Integer = 0 To dtGastos.Rows.Count - 1

                If dtGastos.GetValue("sel", i) = "Y" Then
                    Dim t As String = dtGastos.GetValue("sel", i)

                    For j As Integer = 0 To l_oLineasCot.Count - 1
                        l_oLineasCot.SetCurrentLine(j)

                        If dtGastos.GetValue("cod", i) = l_oLineasCot.ItemCode AndAlso
                            dtGastos.GetValue("lnum", i) = CStr(l_oLineasCot.LineNum) Then
                            If p_strTipoDoc = TipoDocumento.Factura Then
                                l_oLineasCot.UserFields.Fields.Item("U_SCGD_NoFacPro").Value = p_strFacPro
                                l_oLineasCot.UnitPrice = dtGastos.GetValue("pre", i)
                            ElseIf p_strTipoDoc = TipoDocumento.Asiento Then
                                l_oLineasCot.UserFields.Fields.Item("U_SCGD_NoAsGastos").Value = p_strAsiento
                                l_oLineasCot.UserFields.Fields.Item("U_SCGD_Costo").Value = dtGastos.GetValue("cos", i)
                                l_oLineasCot.UnitPrice = dtGastos.GetValue("pre", i)
                            End If
                            Exit For
                        End If
                    Next
                End If

            Next

            l_oCotizacion.Update()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Function TotalLIneasFactura() As Decimal
        Try
            Dim l_decTotal As Decimal = 0

            For i As Integer = 0 To dtGastos.Rows.Count - 1
                If dtGastos.GetValue("sel", i) = "Y" Then
                    l_decTotal += dtGastos.GetValue("cos", i)
                End If
            Next
            Return l_decTotal
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Function CrearAsientoGastos()
        Try

            Dim l_strCtaDebito As String
            Dim l_strCtaCredito As String
            Dim l_decMontoAsi As String
            Dim l_strMonedaAsi As String
            Dim strUsuario As String
            Dim strSucursalUsuario As String
            Dim l_strSQLConfig As String
            Dim l_strComment As String
            Dim l_intAsiento As Integer
            Dim l_strNoOT As String
            'Dim l_strSQL As String = " SELECT DocEntry , DocNum, DocRate, DocCur, U_SCGD_idSucursal, U_SCGD_Numero_OT " +
            '                    " FROM OQUT oq WHERE DocEntry = '{0}'"


            l_strSQLConfig = " SELECT U_Sucurs, U_CtaAcreGast, U_CtaDebGast, U_MonDocGastos FROM [@SCGD_CONF_SUCURSAL] WHERE U_Sucurs = '{0}'"

            strUsuario = ApplicationSBO.Company.UserName
            strSucursalUsuario = Utilitarios.EjecutarConsulta(
                String.Format("Select Branch from OUSR where USER_CODE = '{0}'", strUsuario),
                                                              CompanySBO.CompanyDB,
                                                              CompanySBO.Server)
            l_decMontoAsi = TotalLIneasFactura()

            If l_decMontoAsi <> 0 Then

                l_strSQLConfig = String.Format(l_strSQLConfig, strSucursalUsuario)

                dtConfig = FormularioSBO.DataSources.DataTables.Item("DatosConfig")
                dtConfig.Clear()
                dtConfig.ExecuteQuery(l_strSQLConfig)

                If dtConfig.Rows.Count <> 0 Then
                    l_strCtaCredito = dtConfig.GetValue("U_CtaAcreGast", 0).ToString
                    l_strCtaDebito = dtConfig.GetValue("U_CtaDebGast", 0).ToString
                    l_strMonedaAsi = dtConfig.GetValue("U_MonDocGastos", 0).ToString
                End If

                l_strNoOT = txtNoOrden.ObtieneValorUserDataSource

                l_strComment = txtObs.ObtieneValorUserDataSource()
                l_intAsiento = CrearAsiento(l_strCtaCredito, l_strCtaDebito, l_strMonedaAsi, l_decMontoAsi, l_strNoOT, l_strComment)

                If l_intAsiento <> 0 Then

                    ActualizaCotizacion(txtDocE.ObtieneValorUserDataSource(), "", l_intAsiento, TipoDocumento.Asiento)
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCreaDocGastoDocCreado, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)

                    FormularioSBO.Mode = BoFormMode.fm_VIEW_MODE
                    FormularioSBO.Items.Item("btnCancel").Enabled = True

                    Dim oItem As SAPbouiCOM.Item
                    oItem = FormularioSBO.Items.Item("btnCancel")
                    oItem.Click()

                    oFormularioIncluirGastos.EjecutaBusqueda("SCGD_AGOT")

                ElseIf l_intAsiento = 0 Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCreaDocGastoErrorCrear, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    'Creación de mano de obra
    Public Function CrearAsiento(ByVal p_strCuentaAcredita As String, _
                                        ByVal p_strCuentaDebita As String, _
                                        ByVal p_strMoneda As String, _
                                        ByVal p_decMontoAsiento As Decimal, _
                                        ByVal p_strNoOT As String, _
                                        ByVal p_strObs As String) As Integer

        Dim oJournalEntry As SAPbobsCOM.JournalEntries
        Dim strMonedaLocal As String
        Dim intError As Integer
        Dim strMensajeError As String = ""
        Dim strNoAsiento As String

        strNoAsiento = 0

        strMonedaLocal = Utilitarios.EjecutarConsulta("Select mainCurncy from OADM", _companySbo.CompanyDB, _companySbo.Server)

        oJournalEntry = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

        oJournalEntry.Memo &= p_strObs
        oJournalEntry.Reference = p_strNoOT
        oJournalEntry.ReferenceDate = CDate(Date.Now)


        '*****************
        'Cuenta Debito
        '*****************
        oJournalEntry.Lines.AccountCode = p_strCuentaDebita

        If strMonedaLocal = p_strMoneda Then
            oJournalEntry.Lines.Debit = p_decMontoAsiento
        Else
            oJournalEntry.Lines.FCDebit = p_decMontoAsiento
            oJournalEntry.Lines.FCCurrency = p_strMoneda

        End If
        oJournalEntry.Lines.Reference1 = p_strNoOT
        oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
        oJournalEntry.Lines.Add()

        '*********************
        ' Contra cuenta
        'Cuenta Credito
        '*********************
        oJournalEntry.Lines.Reference1 = p_strNoOT
        oJournalEntry.Lines.AccountCode = p_strCuentaAcredita
        oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

        If strMonedaLocal = p_strMoneda Then
            oJournalEntry.Lines.Credit = p_decMontoAsiento
        Else
            oJournalEntry.Lines.FCCredit = p_decMontoAsiento
            oJournalEntry.Lines.FCCurrency = p_strMoneda
        End If


        If oJournalEntry.Add <> 0 Then
            strNoAsiento = 0
            _companySbo.GetLastError(intError, strMensajeError)
            Throw New ExceptionsSBO(intError, strMensajeError)
        Else
            _companySbo.GetNewObjectCode(strNoAsiento)
        End If

        Return CInt(strNoAsiento)

    End Function


    Private Function DevuelveValorItem(ByVal strSucur As String, _
                               ByVal strUDfName As String) As String
        Try

            Dim strSQL As String
            Dim strResult As String
            strSQL = "SELECT {0} FROM [@SCGD_CONF_SUCURSAL] WHERE U_Sucurs = '{1}'"
            strSQL = String.Format(strSQL, strUDfName, strSucur)

            strResult = Utilitarios.EjecutarConsulta(strSQL, _companySbo.CompanyDB, _companySbo.Server)

            If String.IsNullOrEmpty(strResult) Then
                strResult = -1
            End If

            Return strResult
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try



    End Function

    Public Function ValidarDatos(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean) As Boolean
        Try
            Dim l_Result As Boolean
            Dim l_decTipoCambio As Decimal

            '  l_decTipoCambio = Utilitarios.EjecutarConsultaPrecios("Select Rate from [ORTT] where Currency = '" & strMonedaOrigen & "' and RateDate = '" & CDate(strFecha).ToString("yyyyMMdd") & "'", m_oCompany.CompanyDB, m_oCompany.Server)

            If chxFactura.ObtieneValorUserDataSource = "Y" Then

                If String.IsNullOrEmpty(txtProveedor.ObtieneValorUserDataSource) Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCreaDocGastoFaltaProveedor, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    l_Result = False
                    BubbleEvent = False
                End If

            End If
            
            If TotalLIneasFactura() = 0 Then
                If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCreaDocGastoMontoFactCero, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                    l_Result = False
                    BubbleEvent = False
                End If
            End If

            Return l_Result
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function
#End Region

End Class
